using NPOI.HSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TPPDDB.Helpers;
using TPPDDB.Model.Const;
using TPPDDB.Service;

namespace TPPDDB._3_forleave
{
    public partial class C_OVERTIME_OVERWORK_OVER45HR_RPT : System.Web.UI.Page
    {
        /// <summary>
        /// 報表的名稱,因為很多地方都會使用到,提取成參數
        /// </summary>
        public const string ReportName = "加班時數45小時以上清冊";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //檢查權限
                C.check_power();

                //依群組權限建立機關選單內容
                chk_TPMGroup();

                //預設給予年月,上個月
                DateTime BEG = DateTime.Now.AddMonths(-1);
                this.TextBox_YEARMONTH.Text = (BEG.Year - 1911).ToString() + BEG.Month.ToString("D2");


            }
        }

        /// <summary>
        /// 依群組權限建立機關選單內容
        /// </summary>
        protected void chk_TPMGroup()
        {
            if (MZ_PowerService.isOK() == false)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click",
                       "alert('此人員查無超勤之權限資料，無法使用此功能'); window.location.href='/10-knowledge/J_Login_after.aspx?TPM_FION=-1';", true);

            }             

            DataTable dt;
            //是否可以抓取所有機關?
            if (MZ_PowerService.isAll_AD())
            {
                dt = ADUnitService.GetDataSet_AD();
            }
            else
            {
                //帶入三個機關資料 編制機關 現服機關 發薪機關 
                List<String> Depts = ADUnitService.GetList_AD_BySession();
                dt = ADUnitService.GetDataSet_AD(Depts);
            }
            //綁定 機關選單
            DropDownList_MZ_AD.DataSource = dt;
            DropDownList_MZ_AD.DataBind();
            //權限不可抓取 全部單位 ,且非新中和
            if (MZ_PowerService.isAll_UNIT() == false && !ADUnitService.IsNewZhonghe_BySession())
            {
                //機關單位都先鎖定住
                DropDownList_MZ_AD.Enabled = false;
                DropDownList_MZ_UNIT.Enabled = false;
            }


            try
            {
                //2023-05-02 以 現服機關單位 MZ_EXAD 跟 MZ_EXUNIT 為主
                //現服機關 MZ_EXAD
                string EXAD = Session[ConstSession.EXAD].ToString();
                DropDownList_MZ_AD.SelectedValue = EXAD;
                //現服單位 MZ_EXUNIT
                string EXUNIT = Session[ConstSession.EXUNIT].ToString();
                //綁定之
                DropDownList_MZ_UNIT.SelectedValue = EXUNIT;
            }
            catch (Exception)
            {
                DropDownList_MZ_AD.SelectedValue = "";
            }

            //判斷當前權限等級
            //設定機關下拉選單
            //設定單位下拉選單
            o_A_KTYPE.AddUNIT(DropDownList_MZ_AD.SelectedValue
                 , this.DropDownList_MZ_UNIT
                 , "全部");
        }

        //事件-更改機關單位選項

        /// <summary>
        /// 當機關 下拉選單變動時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DropDownList_AD_SelectedIndexChanged(object sender, EventArgs e)
        {
            //設定單位下拉選單
            o_A_KTYPE.AddUNIT(DropDownList_MZ_AD.SelectedValue
                , this.DropDownList_MZ_UNIT
                , "全部");
        }

        //按鈕-匯出Excel
        /// <summary>
        /// 列印按鈕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btPrintExcel_Click(object sender, EventArgs e)
        {
            //判斷機關是否選擇
            if (DropDownList_MZ_AD.SelectedIndex == 0 || DropDownList_MZ_AD.SelectedValue == "")
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('機關未選擇');", true);
                return;
            }
            //判斷民國年日期正確
            string YEARMONTH = this.TextBox_YEARMONTH.Text;
            if (ForDateTime.Check_TWYearMonth(YEARMONTH) == false)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('年月格式輸入錯誤');", true);
                return;
            }

            //抓取目前要找的機關單位
            string AD = DropDownList_MZ_AD.SelectedItem.Value;
            string UNIT = DropDownList_MZ_UNIT.SelectedItem.Value;
            string YEAR = YEARMONTH.Substring(0, 3);
            string MONTH = YEARMONTH.Substring(3, 2);
            //======查詢資料
            var dt = GetDataTable(AD, UNIT, YEAR, MONTH);

            //轉換報表需要的參數
            string ADName = DropDownList_MZ_AD.SelectedItem.Text;
            string UnitName = DropDownList_MZ_UNIT.SelectedItem.Text;
            //特殊:單位名稱如果是全部,則不顯示名稱
            UnitName = UnitName.Replace("全部", "");
            //產生Excel報表
            var workbook = GetWorkbook(dt, ADName, UnitName, YEAR, MONTH);
            //匯出
            OfficeHelpers.ExcelHelpers.Response_Workbook(workbook, ReportName);
        }
        /// <summary>
        /// 取消按鈕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btCancel_Click(object sender, EventArgs e)
        {
            TextBox_YEARMONTH.Text = string.Empty;
            //TextBox_MZ_ID.Text = string.Empty;
        }

        /// <summary>
        /// 讀取資料
        /// 
        /// </summary>
        /// <param name="MZ_EXAD">機關代碼</param>
        /// <param name="MZ_EXUNIT">單位代碼</param>
        /// <param name="YEARMONTH">民國年月,EX: 11201 </param>
        /// <returns></returns>
        public DataTable GetDataTable(string MZ_EXAD, string MZ_EXUNIT, string MZ_YEAR, string MZ_Month)
        {
            string SQL = @"
                
 
select 
K.MZ_KCHI as UNIT_NAME
,A.MZ_Name,A.mZ_ID
--職稱代碼,職稱等級,職稱名稱
,A.MZ_OCCC,A.MZ_TBDV,AKS.MZ_KCHI as MZ_OCCC_NAME
,sum(floor(T1.mz_budget_hour)) as TotalHour
FROM A_DLBASE A 
join 
(
    --每月超勤時數
    select mZ_ID,MZ_EXAD,MZ_EXUNIT ,NVL(d.mz_budget_hour,0) as mz_budget_hour
    from c_dutymonthovertime_hour d
    where 1=1 
    --篩選年月
    and d.MZ_YEAR=@MZ_YEAR and d.MZ_Month=@MZ_Month
    --篩選機關
    and d.MZ_EXAD=@MZ_EXAD
    --邏輯條件:單位代碼and d.MZ_EXUNIT=@MZ_EXUNIT
    --限定審核過的
    and d.MZ_VERIFY='Y'
    union all    
    --每月業務加班時數,注意這邊的時數其實是分鐘數
    select mZ_ID,MZ_EXAD,MZ_EXUNIT ,NVL(d.mz_budget_hour,0)/60 as mz_budget_hour
    from c_overtimemonth_hour d
    where 1=1
    --篩選年月
    and d.MZ_YEAR=@MZ_YEAR and d.MZ_Month=@MZ_Month
    --篩選機關
    and d.MZ_EXAD=@MZ_EXAD
    --邏輯條件:單位代碼and d.MZ_EXUNIT=@MZ_EXUNIT
    --限定審核過的
    and d.MZ_VERIFY='Y'
)  T1 on T1.MZ_ID=A.MZ_ID
--取得機關單位的等級
join A_UNIT_AD AUA on AUA.MZ_UNIT=T1.MZ_EXUNIT and AUA.MZ_AD=T1.MZ_EXAD
--代碼表,警局單位:
join A_KTYPE K on T1.MZ_EXUNIT=K.MZ_KCODE and MZ_KTYPE='25'--限定代碼類型 單位
--職稱
left join A_KTYPE AKS on AKS.MZ_KCODE = A.MZ_OCCC AND AKS.MZ_KTYPE = '26'
where 1 = 1
--實際加班時數 >= 45小時
  Having sum(T1.mz_budget_hour) >= 45
group by AUA.MZ_NO1,K.MZ_KCHI,A.MZ_Name,A.mZ_ID
--職稱代碼,職稱名稱
,A.MZ_OCCC,A.MZ_TBDV,AKS.MZ_KCHI
--依照MZ_NO1(單位等級),K.MZ_KCHI(單位名稱),MZ_TBDV(職稱順位)排序,官階高者在上面,之後:職稱/單位/證號
order by AUA.MZ_NO1 ASC,K.MZ_KCHI, A.MZ_TBDV,A.MZ_OCCC,K.MZ_KCHI,A.MZ_ID

";

            List<SqlParameter> SqlPrams = new List<SqlParameter>();
            SqlPrams.Add(new SqlParameter(":MZ_EXAD", MZ_EXAD));
            SqlPrams.Add(new SqlParameter(":MZ_YEAR", MZ_YEAR));
            SqlPrams.Add(new SqlParameter(":MZ_Month", MZ_Month));
            //特殊:有指定 單位,就解除備註,
            if (MZ_EXUNIT.IsNullOrEmpty() == false)
            {
                //否則就把這段註解拿掉,讓語法有效
                SQL = SQL.Replace("--邏輯條件:單位代碼", "");
                SqlPrams.Add(new SqlParameter(":MZ_EXUNIT", MZ_EXUNIT));
            }

            return o_DBFactory.ABC_toTest.Create_Table(SQL, SqlPrams);
        }

        public HSSFWorkbook GetWorkbook(DataTable dt
            , string ADName, string UnitName
            , string YEAR, string MONTH)
        {
            HSSFWorkbook workbook = null;
            //抓取範本
            string SamplePath = Server.MapPath("~/3-forleave/report/C_OVERTIME_OVERWORK_OVER45HR_RPT.xls");
            using (FileStream fileStream = new FileStream(SamplePath, FileMode.Open, FileAccess.Read))
            {
                workbook = new HSSFWorkbook(fileStream);
            }
            //第一個分業
            HSSFSheet sheet = workbook.GetSheetAt(0);

            //抓取表頭文字
            //新北市政府警察局ＸＸ分局○年○月加班時數45小時以上之清冊
            //#ADName##UnitName##YEAR#年#MONTH#月加班時數45小時以上之清冊

            string title = "#ADName##UnitName##YEAR#年#MONTH#月"+ReportName;
            title = title.Replace("#ADName#", ADName);
            title = title.Replace("#UnitName#", UnitName);
            title = title.Replace("#YEAR#", YEAR);
            title = title.Replace("#MONTH#", MONTH);
            //將資料綁到內容中
            sheet.GetRow(0).GetCell(0).SetCellValue(title);


            //一開始簽名區的index為3,也就是第4列
            int i_sheetRow = 3;

            //先把範本中的下半部往下搬動
            if (dt.Rows.Count > 0)
            {
                //從 index=3 到 index=4, DB抓到多少筆資料,就往下搬動多少筆
                sheet.ShiftRows(i_sheetRow, i_sheetRow + 1, dt.Rows.Count);
            }
            //備註這列比較高
            sheet.GetRow(i_sheetRow + dt.Rows.Count).Height = 4300;

            int i_MaxCell = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                //然後建立新的列
                var sheetRow = sheet.CreateRow(i_sheetRow);
                i_sheetRow++;


                //綁定數值
                DataRow dataRow = dt.Rows[i];
                int i_sheetCell = 0;//歸零
                                    //預期結果欄位
                                    /*
                                    UNIT_NAME	後勤科
                                    MZ_NAME	洪ＯＯ
                                    MZ_ID	N123456789
                                    MZ_OCCC_NAME	股長
                                    TOTALHOUR	52
                                    */
                                    //機關名稱
                var cell = sheetRow.CreateCell(i_sheetCell++);
                cell.SetCellValue(dataRow["UNIT_NAME"].ToString());
                //職稱
                cell = sheetRow.CreateCell(i_sheetCell++);
                cell.SetCellValue(dataRow["MZ_OCCC_NAME"].ToString());
                //姓名
                cell = sheetRow.CreateCell(i_sheetCell++);
                cell.SetCellValue(dataRow["MZ_NAME"].ToString());
                //核准加班總時數
                cell = sheetRow.CreateCell(i_sheetCell++);
                cell.SetCellValue(60);//寫死60即可
                //實際加班總時數
                cell = sheetRow.CreateCell(i_sheetCell++);
                cell.SetCellValue(dataRow["TOTALHOUR"].ToString().SafeToInt());
                //備註
                cell = sheetRow.CreateCell(i_sheetCell++);

                //紀錄Cell的index最大值,等一下有用
                i_MaxCell = i_sheetCell - 1;
            }

            //統一設定樣式 
            OfficeHelpers.ExcelHelpers.CellStyle style = new OfficeHelpers.ExcelHelpers.CellStyle();
            style.CenterText = true;//文字靠中
            style.UseBorder = true;//有框線
            style.FontName = "標楷體";//標楷體

            OfficeHelpers.ExcelHelpers.Set_CellStyle_Range(workbook, sheet, style
                , 3, i_sheetRow - 1
                , 0, i_MaxCell);

            return workbook;
        }
    }
}