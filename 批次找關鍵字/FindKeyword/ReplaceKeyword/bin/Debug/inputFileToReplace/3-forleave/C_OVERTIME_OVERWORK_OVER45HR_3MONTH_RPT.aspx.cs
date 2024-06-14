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
    public partial class C_OVERTIME_OVERWORK_OVER45HR_3MONTH_RPT : System.Web.UI.Page
    {
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
                ,"全部");
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
            //======查詢資料
            var dt = GetDataTable(AD, UNIT, YEARMONTH);

            //轉換報表需要的參數
            string ADName = DropDownList_MZ_AD.SelectedItem.Text;
            string UnitName = DropDownList_MZ_UNIT.SelectedItem.Text;
            //特殊:單位名稱如果是全部,則不顯示名稱
            UnitName = UnitName.Replace("全部", "");

            //產生Excel報表
            string YEAR = YEARMONTH.Substring(0, 3);
            string MONTH = YEARMONTH.Substring(3, 2);
            var workbook = GetWorkbook(dt, ADName, UnitName, YEAR, MONTH);
            //匯出
            OfficeHelpers.ExcelHelpers.Response_Workbook(workbook, "加班時數連續3個月45小時以上清冊");

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
        public DataTable GetDataTable(string MZ_EXAD, string MZ_EXUNIT, string YEARMONTH)
        {
            //預計的查詢結果
            /*
UNIT_NAME：後勤科
MZ_NAME：李OO
MZ_ID：P123456789
TOTALHOUR1：55
TOTALHOUR2：69
TOTALHOUR3：60

             */

            string SQL = @"
                
 
/*
382132200C

select * from c_dutymonthovertime_hour
select * from c_overtimemonth_hour
*/

WITH 
--三個月內,每月的加班資料
CTE01 AS (
    --每月超勤時數
    select 
    d.MZ_YEAR,d.MZ_Month
    ,mZ_ID,MZ_EXAD,MZ_EXUNIT ,NVL(d.mz_budget_hour,0) as mz_budget_hour
    from c_dutymonthovertime_hour d
    where 1=1 
    --篩選年月
    and  d.MZ_YEAR+d.MZ_Month in (@YEARMONTH1,@YEARMONTH2,@YEARMONTH3)
    --篩選機關
    and d.MZ_EXAD=@MZ_EXAD
    --篩選單位
    --邏輯條件:單位代碼 and d.MZ_EXUNIT=@MZ_EXUNIT
    --限定審核過的
    and d.MZ_VERIFY='Y'
    union all    
    --每月業務加班時數,注意這邊的時數其實是分鐘數
    select d.MZ_YEAR,d.MZ_Month
    ,mZ_ID,MZ_EXAD,MZ_EXUNIT ,NVL(d.mz_budget_hour,0)/60 as mz_budget_hour
    from c_overtimemonth_hour d
    where 1=1
    --篩選年月
    and  d.MZ_YEAR+d.MZ_Month in (@YEARMONTH1,@YEARMONTH2,@YEARMONTH3)
    --篩選機關
    and d.MZ_EXAD=@MZ_EXAD
    --篩選單位
    --邏輯條件:單位代碼 and d.MZ_EXUNIT=@MZ_EXUNIT
    --限定審核過的
    and d.MZ_VERIFY='Y'
),
--統計出每個人員每月的加班時數
CTE02 AS(
 select 
    MZ_YEAR, MZ_MONTH, MZ_ID, MZ_EXAD, MZ_EXUNIT
    ,sum(floor(mz_budget_hour)) as TotalHour 
    from CTE01
    group by MZ_YEAR, MZ_MONTH, MZ_ID, MZ_EXAD, MZ_EXUNIT
 
)
,
--找出三次都大於45hr的人員MZ_ID名單
Target_MZ_ID AS (
    
    SELECT MZ_ID ,count(*) CNT
    FROM CTE02 
    --篩出大於45小時者
    where TotalHour>=45
    --且出現次出達3次者
    having count(*)>=3
    GROUP by MZ_ID
)
select 
K.MZ_KCHI as UNIT_NAME
--員警姓名,員警ID
,A.MZ_Name,A.mZ_ID
--職稱代碼,職稱名稱
,A.MZ_OCCC,AKS.MZ_KCHI as MZ_OCCC_NAME
--第1個月的加班時數
,T1.TotalHour AS TotalHour1
--第2個月的加班時數
,T2.TotalHour AS TotalHour2
--第3個月的加班時數
,T3.TotalHour AS TotalHour3
FROM A_DLBASE A 
--統計資料
JOIN CTE02 T1 on A.MZ_ID=T1.MZ_ID and T1.MZ_YEAR+T1.MZ_MONTH=@YEARMONTH1
--統計資料
JOIN CTE02 T2 on A.MZ_ID=T2.MZ_ID and T2.MZ_YEAR+T2.MZ_MONTH=@YEARMONTH2
--統計資料
JOIN CTE02 T3 on A.MZ_ID=T3.MZ_ID and T3.MZ_YEAR+T3.MZ_MONTH=@YEARMONTH3
join A_UNIT_AD AUA on AUA.MZ_UNIT=T1.MZ_EXUNIT and AUA.MZ_AD=T1.MZ_EXAD
--代碼表,警局單位:
join A_KTYPE K on T1.MZ_EXUNIT=K.MZ_KCODE and MZ_KTYPE='25'--限定代碼類型單位
--職稱
left join A_KTYPE AKS on AKS.MZ_KCODE = A.MZ_OCCC AND AKS.MZ_KTYPE = '26'
where 1 = 1
and A.MZ_ID in (select MZ_ID from Target_MZ_ID)

--依照MZ_NO1(單位等級),K.MZ_KCHI(單位名稱),MZ_TBDV(職稱順位)排序,官階高者在上面,之後:職稱/單位/證號
order by AUA.MZ_NO1 ASC,K.MZ_KCHI, A.MZ_TBDV,A.MZ_OCCC,K.MZ_KCHI,A.MZ_ID
";

            //將民國年月字串,轉換成DateTime物件,以當月一號為主
            DateTime t = ForDateTime.TWYearMonthToDateTime(YEARMONTH).Value;
            //取得前三個月的民國年月字串
            //將DateTime物件 轉換成 民國年月
            string YEARMONTH1 = ForDateTime.DateTimeToTWYearMonth(t.AddMonths(-2));
            string YEARMONTH2 = ForDateTime.DateTimeToTWYearMonth(t.AddMonths(-1));
            string YEARMONTH3 = ForDateTime.DateTimeToTWYearMonth(t);


            List<SqlParameter> SqlPrams = new List<SqlParameter>();
            SqlPrams.Add(new SqlParameter(":MZ_EXAD", MZ_EXAD));
            SqlPrams.Add(new SqlParameter(":YEARMONTH1", YEARMONTH1));
            SqlPrams.Add(new SqlParameter(":YEARMONTH2", YEARMONTH2));
            SqlPrams.Add(new SqlParameter(":YEARMONTH3", YEARMONTH3));


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
            string SamplePath = Server.MapPath("~/3-forleave/report/C_OVERTIME_OVERWORK_OVER45HR_3MONTH_RPT.xls");
            using (FileStream fileStream = new FileStream(SamplePath, FileMode.Open, FileAccess.Read))
            {
                workbook = new HSSFWorkbook(fileStream);
            }
            //第一個分業
            HSSFSheet sheet = workbook.GetSheetAt(0);

            //抓取表頭文字
            //新北市政府警察局ＸＸ分局○年○月加班時數連續3個月45小時以上清冊
            //#ADName##UnitName##YEAR#年#MONTH#月加班時數連續3個月45小時以上清冊

            string title = "#ADName##UnitName##YEAR#年#MONTH#月加班時數連續3個月45小時以上清冊";
            title = title.Replace("#ADName#", ADName);
            title = title.Replace("#UnitName#", UnitName);
            title = title.Replace("#YEAR#", YEAR);
            title = title.Replace("#MONTH#", MONTH);
            //將資料綁到內容中
            sheet.GetRow(0).GetCell(0).SetCellValue(title);

            //設定表頭的年月
            // EX: 112年3月  112年4月  112年5月
            //將民國年月字串,轉換成DateTime物件,以當月一號為主
            DateTime t = ForDateTime.TWYearMonthToDateTime(YEAR + MONTH).Value;
            //取得前三個月的民國年月字串
            //將DateTime物件 轉換成 民國年月
            string YEARMONTH = ForDateTime.DateTimeToTWYearMonth(t.AddMonths(-2), "YYY年MM月");
            sheet.GetRow(2).GetCell(3).SetCellValue(YEARMONTH);
            YEARMONTH = ForDateTime.DateTimeToTWYearMonth(t.AddMonths(-1), "YYY年MM月");
            sheet.GetRow(2).GetCell(4).SetCellValue(YEARMONTH);
            YEARMONTH = ForDateTime.DateTimeToTWYearMonth(t, "YYY年MM月");
            sheet.GetRow(2).GetCell(5).SetCellValue(YEARMONTH);

            //一開始簽名區的index為3,也就是第4列
            int i_sheetRow = 3;

            //先把範本中的下半部往下搬動
            if (dt.Rows.Count > 0)
            {
                //從 index=3 到 index=4, DB抓到多少筆資料,就往下搬動多少筆
                sheet.ShiftRows(i_sheetRow, i_sheetRow + 1, dt.Rows.Count);
            }
            //備註這列比較高
            sheet.GetRow(i_sheetRow + dt.Rows.Count).Height = 2800;

            int i_MaxCell = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                //然後建立新的列
                var sheetRow = sheet.CreateRow(i_sheetRow);
                i_sheetRow++;


                //綁定數值
                DataRow dataRow = dt.Rows[i];
                int i_sheetCell = 0;//歸零

                //DataTable預計格式
                /*
    UNIT_NAME：後勤科
    MZ_NAME：李OO
    MZ_ID：P123456789
    MZ_OCCC：1491
    MZ_OCCC_NAME：警務正

     */

                //EXCEL預計格式
                /*
    單位名稱	職稱	姓名	112年3月  112年4月  112年5月  合計  備註
    人事室     	科員    王大明   65      65         65       195
                 */
                //單位名稱 
                var cell = sheetRow.CreateCell(i_sheetCell++);
                cell.SetCellValue(dataRow["UNIT_NAME"].ToString());
                //職稱  
                cell = sheetRow.CreateCell(i_sheetCell++);
                cell.SetCellValue(dataRow["MZ_OCCC_NAME"].ToString());
                //姓名  
                cell = sheetRow.CreateCell(i_sheetCell++);
                cell.SetCellValue(dataRow["MZ_NAME"].ToString());
                //112年3月  
                cell = sheetRow.CreateCell(i_sheetCell++);
                cell.SetCellValue(dataRow["TOTALHOUR1"].ToString().SafeToInt());
                //112年4月
                cell = sheetRow.CreateCell(i_sheetCell++);
                cell.SetCellValue(dataRow["TOTALHOUR2"].ToString().SafeToInt());
                //112年5月 
                cell = sheetRow.CreateCell(i_sheetCell++);
                cell.SetCellValue(dataRow["TOTALHOUR3"].ToString().SafeToInt());
                //合計  
                cell = sheetRow.CreateCell(i_sheetCell++);
                cell.SetCellValue(
                    dataRow["TOTALHOUR1"].ToString().SafeToInt() +
                    dataRow["TOTALHOUR2"].ToString().SafeToInt() +
                    dataRow["TOTALHOUR3"].ToString().SafeToInt()
                    );
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

        public bool TEST_GetDataTable()
        {
            var dt = GetDataTable("382130000C", "0065", "11106");
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            return false;
        }

        public bool TEST_GetWorkbook()
        {
            var dt = GetDataTable("382130000C", "0065", "11106");
            var workbook = GetWorkbook(dt, "新北市警察局OO分局", "XX派出所", "111", "06");
            //匯出
            OfficeHelpers.ExcelHelpers.Response_Workbook(workbook, "加班時數連續3個月45小時以上清冊");
            return false;
        }
    }
}