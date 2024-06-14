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
    public partial class C_OVERTIMEINSIDE_SUMMARY_rpt : System.Web.UI.Page
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
            //======查詢資料
            var dt = GetDataTable(AD, UNIT, YEARMONTH);

            //轉換報表需要的參數
            string ADName = DropDownList_MZ_AD.SelectedItem.Text;
            string UnitName = DropDownList_MZ_UNIT.SelectedItem.Text;
            //特殊:單位名稱如果是全部,則不顯示名稱
            UnitName = UnitName.Replace("全部", "");

            string YEAR = YEARMONTH.Substring(0, 3);
            string MONTH = YEARMONTH.Substring(3, 2);
            //產生Excel報表
            var workbook = GetWorkbook(dt, ADName, UnitName, YEAR, MONTH);
            //匯出
            OfficeHelpers.ExcelHelpers.Response_Workbook(workbook, "超勤及業務加班情形一覽表");
        }
        /// <summary>
        /// 取消按鈕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btCancel_Click(object sender, EventArgs e)
        {
            TextBox_YEARMONTH.Text = string.Empty;
            TextBox_MZ_ID.Text = string.Empty;
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
            //預期結果欄位
            //MZ_NAME, MZ_ID, MZ_OCCC, MZ_TBDV, MZ_OCCC_NAME, A_BUDGET_HOUR, B_BUDGET_HOUR, A_OVERTIME_PAY, B_OVERTIME_PAY
            //鄭ＯＯ N12*******  1492    003 警政監 0   23  0   8165
            //陳ＯＯ F12*******  1078    004 科長  15  8   4770    2544
            //徐ＯＯ R12*******  1086    005 專員  18  12  5724    3816
            string SQL = @"
   with
                CTE01 AS(
                    --業務加班時數與支領金額
                    select 
                    --員警ID,機關單位
                    A.MZ_ID,A.MZ_EXAD,A.MZ_EXUNIT
                    --年月
                    ,A.MZ_YEAR,A.MZ_MONTH
                     --申請業務加班時數,但應該用不到
                    , TOTAL_JOB 
                    --特殊:報表上要呈現的業務加班時數,這邊必須以 [實際加班] 減去[輪值加班申報時數],無論是否申請業務加班費
                    , floor(MZ_BUDGET_HOUR / 60) - TOTAL_SHIFT as BUDGET_HOUR
                    --業務加班費用
                    ,A. MZ_OVERTIME_PAY_JOB as MZ_OVERTIME_PAY
                    --輪值加班時數
                    , TOTAL_SHIFT
                    --輪值加班費用
                    ,A. MZ_OVERTIME_PAY_SHIFT
                    from C_OVERTIMEMONTH_HOUR A
                    where 1=1
                    --邏輯條件:機關代碼 
                    and MZ_EXAD=@MZ_EXAD
                    --邏輯條件:單位代碼 and MZ_EXUNIT=@MZ_EXUNIT
                    AND (MZ_YEAR+MZ_MONTH)=@YEARMONTH
                    --不限定審核通過的資料
                    --AND MZ_VERIFY='Y'
                ),
                --超勤加班
                CTE02 AS(
                    select
                    --員警ID, 機關單位
                    A.MZ_ID, A.MZ_EXAD, A.MZ_EXUNIT
                    --年月
                    , A.MZ_YEAR, A.MZ_MONTH
                    --超勤時數
                    , A.MZ_BUDGET_HOUR
                    --超勤支領金額
                    , A.MZ_OVERTIME_PAY
                    from c_dutymonthovertime_hour A
                    where 1 = 1
                    --邏輯條件:機關代碼 
                    and MZ_EXAD =@MZ_EXAD
                    --邏輯條件:單位代碼 and MZ_EXUNIT =@MZ_EXUNIT
                    AND(MZ_YEAR + MZ_MONTH) =@YEARMONTH
                    --不限定審核通過的資料
                    --AND MZ_VERIFY='Y'
                )

                select
                --姓名,員警ID,職稱
                AD.MZ_NAME, AD.MZ_ID,AD.MZ_OCCC,AD.MZ_TBDV, AKS.MZ_KCHI as MZ_OCCC_NAME
                --業務加班時數
                ,NVL(CTE01.BUDGET_HOUR, 0) as A_BUDGET_HOUR
                --超勤加班時數
                ,NVL(CTE02.MZ_BUDGET_HOUR, 0) as B_BUDGET_HOUR
                --業務加班金額
                ,NVL(CTE01.MZ_OVERTIME_PAY, 0) as A_OVERTIME_PAY
                --超勤加班金額
                ,NVL(CTE02.MZ_OVERTIME_PAY, 0) as B_OVERTIME_PAY                
                --輪值加班時數
                ,NVL(CTE01.TOTAL_SHIFT, 0) as A_TOTAL_SHIFT
                --輪值加班費用
                ,NVL(CTE01.MZ_OVERTIME_PAY_SHIFT, 0) as A_MZ_OVERTIME_PAY_SHIFT
                FROM
                --基本資料
                A_DLBASE AD
                --業務加班資料
                left join CTE01 CTE01 on CTE01.MZ_ID = AD.MZ_ID
                --超勤加班資料
                left join CTE02 CTE02 on CTE02.MZ_ID = AD.MZ_ID
                --職稱
                left join A_KTYPE AKS on AKS.MZ_KCODE = AD.MZ_OCCC AND AKS.MZ_KTYPE = '26'
                where 1 = 1
                --業務和超勤,其中一邊有資料則抓取出該人員
                and(CTE01.MZ_ID is not null OR CTE02.MZ_ID is not NULL)
                --排除掉特定職稱
                AND AD.MZ_OCCC != 'Z011'
                --依照MZ_TBDV(職稱順位)排序,官階高者在上面
                order by AD.MZ_TBDV, AKS.MZ_KCHI
";

            List<SqlParameter> SqlPrams = new List<SqlParameter>();
            SqlPrams.Add(new SqlParameter(":MZ_EXAD", MZ_EXAD));
            SqlPrams.Add(new SqlParameter(":YEARMONTH", YEARMONTH));
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
            string SamplePath = Server.MapPath("~/3-forleave/report/C_OVERTIMEINSIDE_SUMMARY_rpt.xls");
            using (FileStream fileStream = new FileStream(SamplePath, FileMode.Open, FileAccess.Read))
            {
                workbook = new HSSFWorkbook(fileStream);
            }
            //第一個分業
            HSSFSheet sheet = workbook.GetSheetAt(0);

            //抓取表頭文字
            //新北市政府警察局(機關)○○科、室、中心(單位)112年○月份(年月)超勤及業務加班情形一覽表
            //#ADName##UnitName##YEAR#年#MONTH#月份超勤及業務加班情形一覽表

            string title = "#ADName##UnitName#\r\n#YEAR#年#MONTH#月份超勤及業務加班情形一覽表";
            title = title.Replace("#ADName#", ADName);
            title = title.Replace("#UnitName#", UnitName);
            title = title.Replace("#YEAR#", YEAR);
            title = title.Replace("#MONTH#", MONTH);
            //將資料綁到內容中
            sheet.GetRow(0).GetCell(0).SetCellValue(title);


            //一般欄位樣式
            HSSFCellStyle CellStyle_Common = GetStyle_A(workbook);
            //金額欄位樣式
            HSSFCellStyle CellStyle_Money = GetStyle_B(workbook);

            //一開始簽名區的index為3,也就是第4列
            int i_sheetRow = 3;
            int i_MaxCell = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //把範本中的簽名區往下搬動
                sheet.ShiftRows(i_sheetRow, i_sheetRow, 1);

                //然後建立新的列
                var sheetRow = sheet.CreateRow(i_sheetRow);
                i_sheetRow++;


                //綁定數值
                DataRow dataRow = dt.Rows[i];
                int i_sheetCell = 0;//歸零
                                    //預期結果欄位
                                    /*
                                                        MZ_NAME				鄭ＯＯ			鄭ＯＯ
                                                        MZ_ID				N12*******			N12*******
                                                        MZ_OCCC				1492			1078
                                                        MZ_TBDV				3			4
                                                        MZ_OCCC_NAME		警政監			科長
                                                        A_BUDGET_HOUR		0			15
                                                        B_BUDGET_HOUR		23			8
                                                        A_OVERTIME_PAY		0			4770
                                                        B_OVERTIME_PAY		8165			2544
                                                                         */

                //編號
                var cell = sheetRow.CreateCell(i_sheetCell++);
                cell.SetCellValue((i + 1).ToString().SafeToInt());
                cell.CellStyle = CellStyle_Common;
                //職稱
                cell = sheetRow.CreateCell(i_sheetCell++);
                cell.SetCellValue(dataRow["MZ_OCCC_NAME"].ToString());
                cell.CellStyle = CellStyle_Common;
                //姓名
                cell = sheetRow.CreateCell(i_sheetCell++);
                cell.SetCellValue(dataRow["MZ_NAME"].ToString());
                cell.CellStyle = CellStyle_Common;
                //加班總時數(小時)-業務加班 時數
                cell = sheetRow.CreateCell(i_sheetCell++);
                cell.SetCellValue(dataRow["A_BUDGET_HOUR"].ToString().SafeToInt());
                cell.CellStyle = CellStyle_Common;
                string CellFormulaName_A_BUDGET_HOUR = OfficeHelpers.ExcelHelpers.GetCellFormulaName(i_sheetCell, i_sheetRow);
                //加班總時數(小時)-輪值加班時數
                cell = sheetRow.CreateCell(i_sheetCell++);
                cell.SetCellValue(dataRow["A_TOTAL_SHIFT"].ToString().SafeToInt());
                cell.CellStyle = CellStyle_Common;
                string CellFormulaName_A_TOTAL_SHIFT = OfficeHelpers.ExcelHelpers.GetCellFormulaName(i_sheetCell, i_sheetRow);
                //加班總時數(小時)-超勤加班 時數
                cell = sheetRow.CreateCell(i_sheetCell++);
                cell.SetCellValue(dataRow["B_BUDGET_HOUR"].ToString().SafeToInt());
                cell.CellStyle = CellStyle_Common;
                string CellFormulaName_B_BUDGET_HOUR = OfficeHelpers.ExcelHelpers.GetCellFormulaName(i_sheetCell, i_sheetRow);
                //加班總時數(小時)-合計 時數
                cell = sheetRow.CreateCell(i_sheetCell++);
                cell.SetCellFormula(CellFormulaName_A_BUDGET_HOUR + "+" + CellFormulaName_A_TOTAL_SHIFT + "+" + CellFormulaName_B_BUDGET_HOUR);
                cell.CellStyle = CellStyle_Common;
                //支領金額(元)-業務加班 金額
                cell = sheetRow.CreateCell(i_sheetCell++);
                cell.SetCellValue(dataRow["A_OVERTIME_PAY"].ToString().SafeToInt());
                cell.CellStyle = CellStyle_Money;
                string CellFormulaName_A_OVERTIME_PAY = OfficeHelpers.ExcelHelpers.GetCellFormulaName(i_sheetCell, i_sheetRow);
                //支領金額(元)-輪值加班 金額
                cell = sheetRow.CreateCell(i_sheetCell++);
                cell.SetCellValue(dataRow["A_MZ_OVERTIME_PAY_SHIFT"].ToString().SafeToInt());
                cell.CellStyle = CellStyle_Money;
                string CellFormulaName_A_MZ_OVERTIME_PAY_SHIFT = OfficeHelpers.ExcelHelpers.GetCellFormulaName(i_sheetCell, i_sheetRow);
                //支領金額(元)-超勤加班 金額
                cell = sheetRow.CreateCell(i_sheetCell++);
                cell.SetCellValue(dataRow["B_OVERTIME_PAY"].ToString().SafeToInt());
                cell.CellStyle = CellStyle_Money;
                string CellFormulaName_B_OVERTIME_PAY = OfficeHelpers.ExcelHelpers.GetCellFormulaName(i_sheetCell, i_sheetRow);
                //支領金額(元)-合計 金額
                cell = sheetRow.CreateCell(i_sheetCell++);
                cell.SetCellFormula(CellFormulaName_A_OVERTIME_PAY + "+" + CellFormulaName_A_MZ_OVERTIME_PAY_SHIFT + "+" + CellFormulaName_B_OVERTIME_PAY);
                cell.CellStyle = CellStyle_Money;
                //備註
                cell = sheetRow.CreateCell(i_sheetCell++);
                cell.CellStyle = CellStyle_Common;


                //紀錄Cell的index最大值,等一下有用
                i_MaxCell = i_sheetCell - 1;
            }



            /*
            //統一設定樣式 
            OfficeHelpers.ExcelHelpers.CellStyle style = new OfficeHelpers.ExcelHelpers.CellStyle();
            style.CenterText = true;//文字靠中
            style.UseBorder = true;//有框線
            style.FontName = "標楷體";//標楷體

            //批次範圍設定樣式
            OfficeHelpers.ExcelHelpers.Set_HSSFCellStyle_Range(sheet, CellStyle_Common
                , 3, i_sheetRow - 1
                , 0, i_MaxCell);


            //批次範圍設定樣式
            OfficeHelpers.ExcelHelpers.Set_HSSFCellStyle_Range(sheet, CellStyle_Money
                , 3, i_sheetRow - 1
                , 7, 10);
                */

            //強制設定欄寬
            int i_co = 0;
            //編號
            sheet.SetColumnWidth(i_co++, 4 * 256);
            //職稱
            sheet.SetColumnWidth(i_co++, 10 * 256);
            //姓名
            sheet.SetColumnWidth(i_co++, 9 * 256);
            /*
             加班總時數(小時)								
		    "業務加班"	"輪值加班"	"超勤加班"	合計	
             */
            sheet.SetColumnWidth(i_co++, 6 * 256);
            sheet.SetColumnWidth(i_co++, 6 * 256);
            sheet.SetColumnWidth(i_co++, 6 * 256);
            sheet.SetColumnWidth(i_co++, 6 * 256);
            /*
            支領金額(元)
            "業務加班"	"輪值加班"	"超勤加班"	合計
             */
            sheet.SetColumnWidth(i_co++, 8 * 256);
            sheet.SetColumnWidth(i_co++, 8 * 256);
            sheet.SetColumnWidth(i_co++, 8 * 256);
            sheet.SetColumnWidth(i_co++, 8 * 256);
            //備註
            sheet.SetColumnWidth(i_co++, 13 * 256);

            return workbook;
        }

        private static HSSFCellStyle GetStyle_B(HSSFWorkbook workbook)
        {

            //針對金額欄位,設定樣式
            HSSFCellStyle CellStyle = workbook.CreateCellStyle();
            //設定樣式-邊框
            OfficeHelpers.ExcelHelpers.SetStyle_UseBorder(workbook, CellStyle);
            //設定樣式-置中
            OfficeHelpers.ExcelHelpers.SetStyle_RIGHTText(CellStyle);
            //標楷體
            OfficeHelpers.ExcelHelpers.SetStyle_數字千分位(workbook, CellStyle);
            return CellStyle;
        }

        private static HSSFCellStyle GetStyle_A(HSSFWorkbook workbook)
        {

            //建立樣式
            HSSFCellStyle CellStyle = workbook.CreateCellStyle();
            //設定樣式-邊框
            OfficeHelpers.ExcelHelpers.SetStyle_UseBorder(workbook, CellStyle);
            //設定樣式-置中
            OfficeHelpers.ExcelHelpers.SetStyle_CenterText(CellStyle);
            //標楷體
            OfficeHelpers.ExcelHelpers.SetStyle_標楷體(workbook, CellStyle);
            return CellStyle;
        }

        public void TEST_GetWorkbook()
        {
            var dt = GetDataTable("382130000C", "", "11009");
            HSSFWorkbook book = GetWorkbook(dt, "板橋分局", "", "110", "09");

            OfficeHelpers.ExcelHelpers.Response_Workbook(book, "系統測試");
        }

        public bool UnitTest(ref string msg)
        {
            try
            {

                //	0065	11005
                var dt = GetDataTable("382130000C", "0065", "11005");

                if (dt.Rows.Count == 0)
                {
                    msg = "查無資料,可能SQL錯誤";
                    return false;
                }
            }
            catch (Exception ex)
            {

                msg = ex.ToString();
                return false;
            }
            msg = "OK";
            return true;
        }
    }
}