using NPOI.HSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using TPPDDB.Helpers;
using TPPDDB.Model.Const;

namespace TPPDDB._3_forleave
{
    public partial class C_OVERTIMEINSIDE_TIMEPAYTOTAL_rpt_V2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                C.check_power();

                ViewState["C_strGID"] = o_a_Function.strGID(Session["ADPMZ_ID"].ToString());

                chk_TPMGroup();

                //預設給予年月
                DateTime BEG = DateTime.Now.AddMonths(-6);
                DateTime END = DateTime.Now.AddMonths(-1);
                this.TextBox_DATE_S.Text = (BEG.Year - 1911).ToString() + BEG.Month.ToString("D2");
                this.TextBox_DATE_E.Text = (END.Year - 1911).ToString() + END.Month.ToString("D2");

                //設定單位下拉選單
                o_A_KTYPE.AddUNIT(DropDownList_MZ_AD.SelectedValue, this.DropDownList_MZ_UNIT);
                DropDownList_MZ_UNIT.Items.RemoveAt(0);
            }
        }

        /// <summary>
        /// 依群組權限建立機關選單內容
        /// </summary>
        protected void chk_TPMGroup()
        {
            string premission = ViewState["C_strGID"].ToString();
            bool IsNewZhonghe = false;
            switch (premission)
            {
                case "A":
                case "B":
                    GetMZ_AD_List(1, ref IsNewZhonghe);
                    break;
                case "C":
                case "D":
                    GetMZ_AD_List(2, ref IsNewZhonghe);
                    break;
                case "E":
                    GetMZ_AD_List(2, ref IsNewZhonghe);
                    if (!IsNewZhonghe)
                    {   
                        //機關單位都先鎖定住
                        DropDownList_MZ_AD.Enabled = false;
                        DropDownList_MZ_UNIT.Enabled = false;
                    }
                    break;
                default:
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click",
                        "alert('此人員查無超勤之權限資料，無法使用此功能'); window.location.href='/10-knowledge/J_Login_after.aspx?TPM_FION=-1';", true);
                    return;
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
        }

        /// <summary>
        /// 取得機關List內容
        /// </summary>
        /// <param name="Mode"></param>
        private void GetMZ_AD_List(int Mode, ref bool IsNewZhonghe)
        {
            DataTable dt;
            string SQL = "";
            string SQL_IN = "";

            //AB權限會帶入1
            if (Mode == 1)
            {
                dt = o_DBFactory.ABC_toTest.Create_Table("  (SELECT  MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE = '04' AND MZ_KCODE LIKE '38213%') ", "GET");
            }
            else
            {
                //帶入三個單位資料 編制機關 現服機關 發薪機關 
                List<String> Depts = new List<String>();
                if (Session[ConstSession.AD] != null)
                {
                    Depts.Add(Session[ConstSession.AD].ToString());
                }
                if (Session[ConstSession.EXAD] != null)
                {
                    Depts.Add(Session[ConstSession.EXAD].ToString());
                }
                if (Session[ConstSession.ADPPAY_AD] != null)
                {
                    Depts.Add(Session[ConstSession.ADPPAY_AD].ToString());
                }

                if (Depts.Count > 0)
                {
                    SQL_IN = String.Join("', '", Depts.ToArray());
                    SQL_IN = String.Format(@" AND (MZ_KCODE IN ('{0}'))", SQL_IN);
                }

                //matthew 如果三個單位裡面有中和分局 要帶出中和一&中和二
                if (Depts.Where(x => x.Contains("382133600C")).Count() > 0)
                {
                    Depts.AddRange(new List<string>() { "382133400C", "382133500C" });
                    string dept_List = String.Join("', '", Depts.ToArray());
                    SQL_IN = String.Format(@" AND (MZ_KCODE IN ('{0}'))", dept_List);
                    IsNewZhonghe = true;
                }
                SQL = @"SELECT MZ_KCHI, MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE='04' AND MZ_KCODE LIKE '38213%'  " + SQL_IN;
                dt = o_DBFactory.ABC_toTest.Create_Table(SQL, "GET");
            }

            DropDownList_MZ_AD.DataSource = dt;
            DropDownList_MZ_AD.DataBind();
        }

        /// <summary>
        /// 當機關 下拉選單變動時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DropDownList_AD_SelectedIndexChanged(object sender, EventArgs e)
        {
            //設定單位下拉選單
            o_A_KTYPE.AddUNIT(DropDownList_MZ_AD.SelectedValue, this.DropDownList_MZ_UNIT);
            DropDownList_MZ_UNIT.Items.RemoveAt(0);
        }


        /// <summary>
        /// 列印按鈕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btPrintExcel_Click(object sender, EventArgs e)
        {
            //判斷query是否正確
            if (DropDownList_MZ_AD.SelectedIndex == 0 || DropDownList_MZ_AD.SelectedValue == "")
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('機關未選擇');", true);
                return;
            }
            //判斷民國年日期正確
            if (this.TextBox_DATE_S.Text.Length != 5)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('年月格式輸入錯誤');", true);
                return;
            }

            if (this.TextBox_DATE_E.Text.Length != 5)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('年月格式輸入錯誤');", true);
                return;
            }

            switch (RadioButtonList_TYPE.SelectedValue)
            {
                case "1":
                    //業務報表
                    GetInsideData();
                    break;
                case "2":
                    //超勤報表
                    GetOverTimeData();
                    break;
                default:
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('未選擇列印項目');", true);
                    break;
            }
        }

        /// <summary>
        /// 取消按鈕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btCancel_Click(object sender, EventArgs e)
        {
            TextBox_DATE_S.Text = string.Empty;
            TextBox_DATE_E.Text = string.Empty;
            TextBox_MZ_ID.Text = string.Empty;
        }

        /// <summary>
        /// 取得業務加班時數、金額資料
        /// </summary>
        private void GetInsideData()
        {
            //從UI綁定資料查詢需要的物件
            QryData_InsideData qry = Create_QryData_FromUI();
            //根據參數,查詢 業務加班報表 回傳DataTable物件
            DataTable tempDT = GetDataTable_InsideData(qry);

            if (tempDT.Rows.Count > 0)
            {
                //tempDT.ImportRow(sumTempDT.Rows[0]);
                ExcelExtendData ex = new ExcelExtendData();
                //標題:新北市警察局板橋分局 業務加班費時數及金額統計表	
                ex.Title = this.DropDownList_MZ_AD.SelectedItem.Text + " 業務加班費時數及金額統計表";
                //起訖日期 EX: 11101  ~ 11106
                ex.TextBox_DATE_S = this.TextBox_DATE_S.Text;
                ex.TextBox_DATE_E = this.TextBox_DATE_E.Text;
                //匯出檔案名稱
                ex.FileName = "業務加班報表";
                //根據DataTable物件 回應 Excel檔
                Response_Excel(tempDT, ex);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "alert('無此日期之相關資料');", true);
            }
        }

        /// <summary>
        /// 取得超勤加班時數、金額資料
        /// </summary>
        private void GetOverTimeData()
        {
            //從UI綁定資料查詢需要的物件
            QryData_InsideData qry = new QryData_InsideData();
            qry.QryYearMonth_Beg = this.TextBox_DATE_S.Text;
            qry.QryYearMonth_End = this.TextBox_DATE_E.Text;
            qry.MZ_EXAD = this.DropDownList_MZ_AD.SelectedValue;
            qry.MZ_EXUNIT = this.DropDownList_MZ_UNIT.SelectedValue;
            qry.MZ_SRANK = this.DropDownList_MZ_SRANK.SelectedValue;
            qry.MZ_ID = this.TextBox_MZ_ID.Text;
            //根據參數,查詢 業務加班報表 回傳DataTable物件
            DataTable tempDT = GetDataTable_OverTimeData(qry);

            if (tempDT.Rows.Count > 0)
            {
                //tempDT.ImportRow(sumTempDT.Rows[0]);
                ExcelExtendData ex = new ExcelExtendData();
                //標題:新北市警察局板橋分局 業務加班費時數及金額統計表	
                ex.Title = this.DropDownList_MZ_AD.SelectedItem.Text + " 超勤加班費時數及金額統計表";
                //起訖日期 EX: 11101  ~ 11106
                ex.TextBox_DATE_S = this.TextBox_DATE_S.Text;
                ex.TextBox_DATE_E = this.TextBox_DATE_E.Text;
                //匯出檔案名稱
                ex.FileName = "超勤加班報表";
                //根據DataTable物件 回應 Excel檔
                Response_Excel(tempDT, ex);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "alert('無此日期之相關資料');", true);
            }
            return;
        }


        /// <summary>
        /// 從UI綁定資料查詢需要的物件
        /// </summary>
        /// <returns></returns>
        private QryData_InsideData Create_QryData_FromUI()
        {
            QryData_InsideData qry = new QryData_InsideData();
            qry.QryYearMonth_Beg = this.TextBox_DATE_S.Text;
            qry.QryYearMonth_End = this.TextBox_DATE_E.Text;
            qry.MZ_EXAD = this.DropDownList_MZ_AD.SelectedValue;
            qry.MZ_EXUNIT = this.DropDownList_MZ_UNIT.SelectedValue;
            qry.MZ_SRANK = this.DropDownList_MZ_SRANK.SelectedValue;
            qry.MZ_ID = this.TextBox_MZ_ID.Text;
            return qry;
        }

        /// <summary>
        /// 根據DataTable物件 回應 Excel檔
        /// </summary>
        /// <param name="tempDT"></param>
        public static void Response_Excel(DataTable tempDT, ExcelExtendData extendData)
        {
            //拼接出Excel,包含以下部分
            //標題:新北市警察局板橋分局 業務加班費時數及金額統計表	
            //期間: 111年1月至111年6月
            //表頭與資料:根據 DataTable tempDT
            var Option_Center_Border = new OfficeHelpers.ExcelHelpers.CellStyle()
            {
                CenterText = true   //文字靠中
                      ,
                UseBorder = true //有框線
            };
            //框線
            var Option_Border = new OfficeHelpers.ExcelHelpers.CellStyle()
            {
                UseBorder = true //有框線
            };
            //抓取輸入資料的欄位寬度
            int column_Count = tempDT.Columns.Count;
            //建立表格表頭
            List<OfficeHelpers.ExcelHelpers.FormatColumn> exceFormat = new List<OfficeHelpers.ExcelHelpers.FormatColumn>();
            var t1 = new OfficeHelpers.ExcelHelpers.FormatColumn(0, 0, 0, column_Count - 1, extendData.Title);
            //綁定樣式設定
            t1.Option = Option_Center_Border;
            exceFormat.Add(t1);

            //拼接 期間字串
            string printStr = "";
            if (extendData.TextBox_DATE_S != "")
            {
                printStr += o_str.RCDateConvert(extendData.TextBox_DATE_S, "{0}年{1}月", true);
            }
            printStr += "至";
            if (extendData.TextBox_DATE_E != "")
            {
                printStr += o_str.RCDateConvert(extendData.TextBox_DATE_E, "{0}年{1}月", true);
            }
            var t2 = new OfficeHelpers.ExcelHelpers.FormatColumn(1, 0, 1, column_Count - 1, string.Format("期間：{0}", printStr));
            //綁定樣式設定
            t2.Option = Option_Center_Border;
            exceFormat.Add(t2);

            //將資料塞入Excel,產生表頭和資料內容
            HSSFWorkbook workbook = new OfficeHelpers.ExcelHelpers().RenderDataTable_ToExcelWorkbook(tempDT, true, exceFormat.ToArray());
            //抓出工作表,等下有用
            var sheet = workbook.GetSheetAt(0);
            //中間的資料區塊原本的設計不會設定框線,統一設定框線等樣式,,需要額外套
            OfficeHelpers.ExcelHelpers.Set_CellStyle_Range(workbook, sheet, Option_Border, 2, tempDT.Rows.Count + 2, 0, tempDT.Columns.Count - 1);

            //產生下方的合計欄位
            exceFormat = new List<OfficeHelpers.ExcelHelpers.FormatColumn>();
            //下方的合計欄位,假設資料含欄位名稱有 6列,則 合併從[行,列] [8,0]~[8,4]
            // [XXX][XXX][XXX][XXX][XXX][XXX][XXX][XXX][XXX][XXX][XXX]
            // [        合計           ][XXX][XXX][XXX][XXX][XXX][XXX]

            //計算目前的格子位置
            int rowIndex = tempDT.Rows.Count + 3;
            int colIndex = 0;
            int mergeRow = rowIndex;
            int mergeCol = 4;

            t2 = new OfficeHelpers.ExcelHelpers.FormatColumn(rowIndex, colIndex, mergeRow, mergeCol, "總計");
            //綁定樣式設定
            t2.Option = Option_Center_Border;
            exceFormat.Add(t2);

            //設定加總欄位,
            //公式需要的截止欄位
            int Fomula_END = tempDT.Rows.Count + 3;

            //實際超勤時數等欄位的合計公式
            for (colIndex = 5; colIndex <= 10; colIndex++)
            {
                /* 這邊利用字元和數字轉換 拼接出公式需要的欄位字母
                 A+0=A
                 A+1=B
                 A+5=F  以此類推
                 */
                char Col_Eng = (char)((int)'A' + colIndex);
                t2 = new OfficeHelpers.ExcelHelpers.FormatColumn(rowIndex, colIndex, mergeRow, colIndex, "=SUM(" + Col_Eng + "4:" + Col_Eng + Fomula_END + ")");
                //綁定樣式設定
                t2.Option = Option_Border;
                exceFormat.Add(t2);
            }

            //套用 剛才設定的那些儲存格
            OfficeHelpers.ExcelHelpers.SetCoustomCell(exceFormat.ToArray(), workbook, sheet);




            //轉換成檔案串流,並且回應出來
            OfficeHelpers.ExcelHelpers.Response_Workbook(workbook, extendData.FileName);
            //new OfficeHelpers.ExcelHelpers().DtToExcelForXLS(tempDT, extendData.FileName, true, exceFormat.ToArray());
        }


        /// <summary>
        /// 查詢參數-業務加班
        /// </summary>
        public class QryData_InsideData
        {
            public string MZ_SRANK { get; set; } = "";
            public string QryYearMonth_Beg { get; set; } = "";
            public string QryYearMonth_End { get; set; } = "";
            public string MZ_EXAD { get; set; } = "";
            public string MZ_EXUNIT { get; set; } = "";
            public string MZ_ID { get; set; } = "";
        }

        /// <summary>
        /// Excel匯出檔的額外資訊,包括 標題/期間/檔名 等資訊
        /// </summary>
        public class ExcelExtendData
        {
            /// <summary>
            /// OOOO 標題
            /// </summary>
            public string Title { get; set; } = "";
            /// <summary>
            /// 期間-起: YYYMM
            /// </summary>
            public string TextBox_DATE_S { get; set; } = "";
            /// <summary>
            /// 期間-訖: YYYMM
            /// </summary>
            public string TextBox_DATE_E { get; set; } = "";
            /// <summary>
            /// 檔案名稱 不含副檔名
            /// </summary>
            public string FileName { get; set; } = "";
        }

        /// <summary>
        /// 根據參數,查詢 業務加班報表 回傳DataTable物件
        /// </summary>
        /// <param name="Qry"></param>
        /// <returns></returns>
        public static DataTable GetDataTable_InsideData(QryData_InsideData Qry)
        {
            string SQL;
            /*
            業務加班時數金額資料
            (1)	申請加班時數(C_OVERTIMEMONTH_HOUR)：查詢機關/單位內時間區間（MZ_YEAR+MZ_MONTH）的MZ_BUDGET_HOUR÷60取整數部分。
            (2)	支領加班費時數(C_OVERTIMEMONTH_HOUR)：查詢機關/單位時間區間(MZ_YEAR+MZ_MONTH）的MZ_REAL_HOUR÷60取整數部分。
            (3)	已補休時數(C_OVERTIME_BASE)：限業務加班(OVER_TYPE = ‘OTB’)，查詢機關/單位時間區間(OVER_DAY)的 REST_HOUR÷60取整數部分。
            (4)	已申請敘獎時數(C_OVERTIME_BASE)：限業務加班(OVER_TYPE = ‘OTB’)，查詢機關/單位時間區間(OVER_DAY)的 PRIZE_HOUR÷60取整數部分。
            (5)	累積剩餘時數(C_OVERTIME_BASE)：限業務加班(OVER_TYPE = ‘OTB’)，查詢機關/單位時間區間（OVER_DAY)的 SURPLUS_TOTAL÷60，呈現方式2（小時).59（60進位，分鐘)。
            (6)	支領金額(C_OVERTIMEMONTH_HOUR)：查詢機關/單位時間區間（MZ_YEAR+MZ_MONTH）的MZ_OVERTIME_PAY值。
            業務加班的核發是以現服單位為主:MZ_EXAD/MZ_EXUNIT

            日期參數範例(格式可能有點怪)
            111年上半年
            QryYearMonth_Beg：11101
            QryYearMonth_End：1110699

            111年下半年
            QryYearMonth_Beg：11107
            QryYearMonth_End：1111299
            */

            SQL = @"
--子查詢-加班統計
with SUM_C_OVERTIME_BASE as (
	select MZ_ID 
		,MZ_EXAD,MZ_EXUNIT/*機關單位*/
		--(3)	已補休時數(C_OVERTIME_BASE)：限業務加班(OVER_TYPE = ‘OTB’)，查詢機關/單位時間區間(OVER_DAY)的 REST_HOUR÷60取整數部分。
		,floor(sum(  REST_HOUR) / 60) as SUM_REST_HOUR
		--(4)	已申請敘獎時數(C_OVERTIME_BASE)：限業務加班(OVER_TYPE = ‘OTB’)，查詢機關/單位時間區間(OVER_DAY)的 PRIZE_HOUR÷60取整數部分。
		,floor(sum(  PRIZE_HOUR) / 60) as SUM_PRIZE_HOUR
		--(5)	累積剩餘時數(C_OVERTIME_BASE)：限業務加班(OVER_TYPE = ‘OTB’)，查詢機關/單位時間區間（OVER_DAY)的 SURPLUS_TOTAL÷60，呈現方式2（小時).59（60進位，分鐘)。
		,sum( SURPLUS_TOTAL) as SUM_SURPLUS_TOTAL
	from C_OVERTIME_BASE
	where 1=1
    AND OVERTIME_TYPE in ('OTB','OTT')
	--篩選年月 C_OVERTIME_BASE
	AND (OVER_DAY) >= @QryYearMonth_Beg
	AND (OVER_DAY) < @QryYearMonth_End
	--機關代碼
	AND MZ_EXAD= @MZ_EXAD
	--單位代碼
	AND MZ_EXUNIT= @MZ_EXUNIT
	group by MZ_ID,MZ_EXAD,MZ_EXUNIT/*機關單位*/
)
,
--子查詢-月加班統計
SUM_C_OVERTIMEMONTH_HOUR as(
	Select 
		COTHI.MZ_ID /*身分證號*/
		--(1)	申請加班時數(C_OVERTIMEMONTH_HOUR)：查詢機關/單位內時間區間（MZ_YEAR+MZ_MONTH）的MZ_BUDGET_HOUR÷60取整數部分。
		,SUM( floor(COTHI.MZ_BUDGET_HOUR / 60)) as SUM_MZ_BUDGET_HOUR
		--(2)	支領加班費時數(C_OVERTIMEMONTH_HOUR)：查詢機關/單位時間區間(MZ_YEAR+MZ_MONTH）的MZ_REAL_HOUR÷60取整數部分。
		,SUM( floor(COTHI.MZ_REAL_HOUR / 60)) as SUM_MZ_REAL_HOUR
		--(6)	支領金額(C_OVERTIMEMONTH_HOUR)：查詢機關/單位時間區間（MZ_YEAR+MZ_MONTH）的 MZ_OVERTIME_PAY 值。
		,SUM( MZ_OVERTIME_PAY ) as SUM_MZ_OVERTIME_PAY
		/*支領金額*/
	From C_OVERTIMEMONTH_HOUR COTHI
	Where 1=1 
	--篩選年月 C_OVERTIMEMONTH_HOUR
	AND (COTHI.MZ_YEAR + COTHI.MZ_MONTH) >= @QryYearMonth_Beg
	AND (COTHI.MZ_YEAR + COTHI.MZ_MONTH) < @QryYearMonth_End
	--機關代碼
	AND MZ_EXAD= @MZ_EXAD
	--單位代碼
	AND MZ_EXUNIT= @MZ_EXUNIT
	group by COTHI.MZ_ID /*身分證號*/
)

--新語法:取得業務加班時數、金額資料
Select 
	--OTB.MZ_EXAD /*現服機關*/
	AKAD.MZ_KCHI as 機關 /*現服機關名*/
	--,OTB.MZ_EXUNIT /*現服單位*/
	,AKK.MZ_KCHI as 單位   /*現服單位名*/
	--,AD.MZ_OCCC /*職稱*/
	,AKS.MZ_KCHI as 職稱 /*職稱*/
	,AD.MZ_ID as 身分證號 /*身分證號*/
	,AD.MZ_NAME as 姓名 /*姓名*/
	--(1)	申請加班時數(C_OVERTIMEMONTH_HOUR)：查詢機關/單位內時間區間（MZ_YEAR+MZ_MONTH）的MZ_BUDGET_HOUR÷60取整數部分。
	,NVL(COTHI.SUM_MZ_BUDGET_HOUR,0) as 申請加班時數
	--(2)	支領加班費時數(C_OVERTIMEMONTH_HOUR)：查詢機關/單位時間區間(MZ_YEAR+MZ_MONTH）的MZ_REAL_HOUR÷60取整數部分。
	,NVL(COTHI.SUM_MZ_REAL_HOUR,0) as 支領加班費時數
	--(3)	已補休時數(C_OVERTIME_BASE)：限業務加班(OVER_TYPE = ‘OTB’)，查詢機關/單位時間區間(OVER_DAY)的REST_HOUR÷60取整數部分。
	,NVL(OTB.SUM_REST_HOUR,0) as 已補休時數
	--(4)	已申請敘獎時數(C_OVERTIME_BASE)：限業務加班(OVER_TYPE = ‘OTB’)，查詢機關/單位時間區間(OVER_DAY)的 PRIZE_HOUR÷60取整數部分。
	,NVL(OTB.SUM_PRIZE_HOUR,0) as 已申請敘獎時數
	--(5)	累積剩餘時數(C_OVERTIME_BASE)：限業務加班(OVER_TYPE = ‘OTB’)，查詢機關/單位時間區間（OVER_DAY)的 SURPLUS_TOTAL÷60，呈現方式2（小時).59（60進位，分鐘)。
,NVL(floor(OTB.SUM_SURPLUS_TOTAL / 60),0) + NVL(mod(OTB.SUM_SURPLUS_TOTAL , 60),0)*0.01 as 累積剩餘時數
--(6)	支領金額(C_OVERTIMEMONTH_HOUR)：查詢機關/單位時間區間（MZ_YEAR+MZ_MONTH）的 MZ_OVERTIME_PAY 值。
,NVL(COTHI.SUM_MZ_OVERTIME_PAY,0) as 支領金額
/*支領金額*/
From 
--基本資料
A_DLBASE AD
--統計-每日加班資訊 以加班資料為基準,有資料的才出現在名單上
inner join SUM_C_OVERTIME_BASE OTB on OTB.MZ_ID=AD.MZ_ID
--統計-加班資訊月摘要
left join SUM_C_OVERTIMEMONTH_HOUR COTHI on AD.MZ_ID = COTHI.MZ_ID
--職稱
left join A_KTYPE AKS on AKS.MZ_KCODE = AD.MZ_OCCC AND AKS.MZ_KTYPE = '26'
--現服機關 以實際加班的現服機關為主
left join A_KTYPE AKAD on AKAD.MZ_KCODE = OTB.MZ_EXAD AND AKAD.MZ_KTYPE = '04'                            
--現服單位 以實際加班的現服單位為主
left join A_KTYPE AKK on AKK.MZ_KCODE = OTB.MZ_EXUNIT AND AKK.MZ_KTYPE = '25'
Where 1=1 
--排除掉特定職稱
AND AD.MZ_OCCC!='Z011' 
--職等判斷
";
            //職等判斷
            if (Qry.MZ_SRANK != "0")
            {
                switch (Qry.MZ_SRANK)
                {
                    case "1":
                        SQL += @"AND (AD.MZ_SRANK like 'G%') ";
                        break;
                    case "2":
                        SQL += @"AND (AKS.MZ_SRANK like 'P%' OR AKS.MZ_SRANK like 'B%') ";
                        break;
                    case "3":
                        SQL += @"AND (AKS.MZ_SRANK like 'J%' OR AKS.MZ_SRANK like 'M%') ";
                        break;
                    default:
                        break;
                }
            }
            SQL += @"
--身分證號判斷
AND AD.MZ_ID=@MZ_ID
order by AD.MZ_ID
";

            //擷取參數
            List<SqlParameter> list_sqlPrams = new List<SqlParameter>();
            //如果 年月 起 有輸入
            if (string.IsNullOrEmpty(Qry.QryYearMonth_Beg) == false)
            {
                list_sqlPrams.Add(new SqlParameter("QryYearMonth_Beg", Qry.QryYearMonth_Beg));
            }
            //如果 年月 訖 有輸入
            if (string.IsNullOrEmpty(Qry.QryYearMonth_End) == false)
            {
                //要做一點特殊處理
                // EX: 11106 => 1110699 ,這樣比大小的時候,六月底的日期也可以納入
                string tmp = Qry.QryYearMonth_End + "99";
                list_sqlPrams.Add(new SqlParameter("QryYearMonth_End", tmp));
            }
            //如果 機關 有輸入
            if (string.IsNullOrEmpty(Qry.MZ_EXAD) == false)
            {
                list_sqlPrams.Add(new SqlParameter("MZ_EXAD", Qry.MZ_EXAD));
            }
            //如果 單位 有輸入
            if (string.IsNullOrEmpty(Qry.MZ_EXUNIT) == false)
            {
                list_sqlPrams.Add(new SqlParameter("MZ_EXUNIT", Qry.MZ_EXUNIT));
            }
            //如果 身分證號 有輸入
            if (string.IsNullOrEmpty(Qry.MZ_ID) == false)
            {
                list_sqlPrams.Add(new SqlParameter("MZ_ID", Qry.MZ_ID));
            }

            //判斷參數是否存在,如果不存在,則將之移除,不做判定
            if (list_sqlPrams.Exists(x => x.ParameterName == "QryYearMonth_Beg") == false)
            {
                SQL = SQL.Replace("AND (OVER_DAY) >= @QryYearMonth_Beg", "");
                SQL = SQL.Replace("AND (COTHI.MZ_YEAR + COTHI.MZ_MONTH) >= @QryYearMonth_Beg", "");
            }
            if (list_sqlPrams.Exists(x => x.ParameterName == "QryYearMonth_End") == false)
            {
                SQL = SQL.Replace("AND (OVER_DAY) < @QryYearMonth_End", "");
                SQL = SQL.Replace("AND (COTHI.MZ_YEAR + COTHI.MZ_MONTH) < @QryYearMonth_End", "");
            }
            if (list_sqlPrams.Exists(x => x.ParameterName == "MZ_EXAD") == false)
            {
                SQL = SQL.Replace("AND MZ_EXAD= @MZ_EXAD", "");
            }
            if (list_sqlPrams.Exists(x => x.ParameterName == "MZ_EXUNIT") == false)
            {
                SQL = SQL.Replace("AND MZ_EXUNIT= @MZ_EXUNIT", "");
            }
            if (list_sqlPrams.Exists(x => x.ParameterName == "MZ_ID") == false)
            {
                SQL = SQL.Replace("AND AD.MZ_ID=@MZ_ID", "");
            }

            DataTable tempDT = o_DBFactory.ABC_toTest.Create_Table(SQL, list_sqlPrams);
            //特殊 累積剩餘時數要做一下格式處理,小數位後面要補0
            // EX:  12.1 => 12.01
            for (int i = 0; i < tempDT.Rows.Count; i++)
            {
                string value = tempDT.Rows[i]["累積剩餘時數"].ToString();
                double DValue = Math.Round(double.Parse(value), 2);
                tempDT.Rows[i]["累積剩餘時數"] = DValue;
            }

            return tempDT;
        }



        /// <summary>
        /// 根據參數,取得超勤報表資料 回傳DataTable物件
        /// </summary>
        /// <param name="Qry"></param>
        /// <returns></returns>
        public static DataTable GetDataTable_OverTimeData(QryData_InsideData Qry)
        {
            string SQL;
            /*
--取得超勤報表資料
--(1)實際超勤時數(C_DUTYMONTHOVERTIME_HOUR +  C_OVERTIME_BASE)：查詢機關/單位內時間區間（MZ_YEAR+MZ_MONTH）的MZ_BUDGET_HOUR + 超勤補休(OVER_TYPE = ‘OTU’)，查詢機關/單位時間區間(OVER_DAY)的OVER_TOTAL÷60取整數部分。
--(2)實際支領超勤加班費時數(C_DUTYMONTHOVERTIME_HOUR)：查詢機關/單位時間區間(MZ_YEAR+MZ_MONTH）的MZ_REAL_HOUR。
--(3)申請超勤補休時數(C_OVERTIME_BASE)：限超勤補休(OVER_TYPE = ‘OTU’)，查詢機關/單位時間區間(OVER_DAY)的REST_HOUR÷60取整數部分。
--(4)已申請敘獎時數(C_DUTYTABLE_REWARD)：僅提供查詢條件為上、下半年度的查詢才有值，查詢機關/單位時間區間（MZ_YEAR+MZ_YEARRANGE）的MZ_HOUT_AWARD值
--(5)累積剩餘時數(C_DUTYTABLE_REWARD)：。僅提供查詢條件為上、下半年度的查詢才有值，查詢機關/單位時間區間（MZ_YEAR+MZ_YEARRANGE）的MZ_HOUR_H1      值
--(6)支領金額(C_DUTYMONTHOVERTIME_HOUR)：查詢機關/單位時間區間（MZ_YEAR+MZ_MONTH）的MZ_OVERTIME_PAY值。
超勤加班費的核發是以編制單位為主:MZ_AD/MZ_UNIT

日期參數範例(格式可能有點怪)
111年上半年
QryYearMonth_Beg：11101
QryYearMonth_End：1110699
QryYear_AND_RANGE：111U

111年下半年
QryYearMonth_Beg：11107
QryYearMonth_End：1111299
QryYear_AND_RANGE：111D


*/

            SQL = @"
--子查詢-月加班統計
with SUM_C_DUTYMONTHOVERTIME_HOUR as (
	select MZ_ID ,MZ_AD,MZ_UNIT
	--(1)實際超勤時數(C_DUTYMONTHOVERTIME_HOUR +  C_OVERTIME_BASE)：查詢機關/單位內時間區間（MZ_YEAR+MZ_MONTH）的 MZ_BUDGET_HOUR + 超勤補休(OVER_TYPE = ‘OTU’)，查詢機關/單位時間區間(OVER_DAY)的OVER_TOTAL÷60取整數部分。
	,SUM( MZ_BUDGET_HOUR) as SUM_MZ_BUDGET_HOUR
	--(2)實際支領超勤加班費時數(C_DUTYMONTHOVERTIME_HOUR)：查詢機關/單位時間區間(MZ_YEAR+MZ_MONTH）的 MZ_REAL_HOUR。
	,SUM(MZ_REAL_HOUR) as SUM_MZ_REAL_HOUR
	--(6)支領金額(C_DUTYMONTHOVERTIME_HOUR)：查詢機關/單位時間區間（MZ_YEAR+MZ_MONTH）的 MZ_OVERTIME_PAY 值。
	,SUM(MZ_OVERTIME_PAY) as SUM_MZ_OVERTIME_PAY
	from C_DUTYMONTHOVERTIME_HOUR
	where 1=1
	--篩選年月 C_OVERTIME_BASE
	AND (MZ_YEAR+MZ_MONTH) >= @QryYearMonth_Beg
	AND (MZ_YEAR+MZ_MONTH) < @QryYearMonth_End
	--加班的現服單位
	--機關代碼 EX:382133200C	新北市政府警察局海山分局
	AND MZ_AD= @MZ_AD
	--單位代碼
	AND MZ_UNIT= @MZ_UNIT
	group by MZ_ID ,MZ_AD,MZ_UNIT
)
,
--子查詢-日加班統計
SUM_C_OVERTIME_BASE as(
	Select 
	MZ_ID /*身分證號*/
	,MZ_AD,MZ_UNIT/*機關單位*/
	--(1)實際超勤時數(C_DUTYMONTHOVERTIME_HOUR +  C_OVERTIME_BASE)：查詢機關/單位內時間區間（MZ_YEAR+MZ_MONTH）的MZ_BUDGET_HOUR + 超勤補休(OVER_TYPE = ‘OTU’)，查詢機關/單位時間區間(OVER_DAY)的OVER_TOTAL÷60取整數部分。
	,SUM( case when  OVERTIME_TYPE='OTU' then floor(OVER_TOTAL/60) else 0 end ) as SUM_OVER_TOTAL
	--(3)申請超勤補休時數(C_OVERTIME_BASE)：限超勤補休(OVER_TYPE = ‘OTU’)，查詢機關/單位時間區間(OVER_DAY)的 REST_HOUR÷60取整數部分。
	,SUM( case when  OVERTIME_TYPE='OTU' then floor(REST_HOUR/60) else 0 end ) as SUM_REST_HOUR
	/*支領金額*/
	From C_OVERTIME_BASE 
	Where 1=1 
	--篩選年月 C_OVERTIMEMONTH_HOUR
	AND OVER_DAY >= @QryYearMonth_Beg
	AND OVER_DAY < @QryYearMonth_End
	--加班的現服單位
	--機關代碼 EX:382133200C	新北市政府警察局海山分局
	AND MZ_AD= @MZ_AD
	--單位代碼
	AND MZ_UNIT= @MZ_UNIT
	group by MZ_ID /*身分證號*/,MZ_AD,MZ_UNIT/*機關單位*/
),
--子查詢-敘獎統計
SUM_c_dutytable_personal as(
	Select 
	MZ_ID /*身分證號*/
	,MZ_AD,MZ_UNIT/*機關單位*/
	--實際超勤時數
	,SUM( NVL( convert_rest_hours,0) ) as SUM_convert_rest_hours
	/*支領金額*/
	From c_dutytable_personal 
	Where 1=1 
	--篩選年月 
	AND dutyDATE >= @QryYearMonth_Beg
	AND dutyDATE < @QryYearMonth_End
	--加班的現服單位
	--機關代碼 EX:382133200C	新北市政府警察局海山分局
	AND MZ_AD= @MZ_AD
	--單位代碼
	AND MZ_UNIT= @MZ_UNIT
	group by MZ_ID /*身分證號*/,MZ_AD,MZ_UNIT/*機關單位*/
)
,
--子查詢-敘獎統計
SUM_C_DUTYTABLE_REWARD as(
	Select 
	MZ_ID /*身分證號*/
	--(4)已申請敘獎時數(C_DUTYTABLE_REWARD)：僅提供查詢條件為上、下半年度的查詢才有值，查詢機關/單位時間區間（MZ_YEAR+MZ_YEARRANGE）的 MZ_HOUT_AWARD 值
	,SUM(MZ_HOUT_AWARD) as SUM_MZ_HOUT_AWARD
	--(5)累積剩餘時數(C_DUTYTABLE_REWARD)：。僅提供查詢條件為上、下半年度的查詢才有值，查詢機關/單位時間區間（MZ_YEAR+MZ_YEARRANGE）的 MZ_HOUR_H1 值
	,SUM(mz_hour_h1_left) as sum_mz_hour_h1_left
	/*支領金額*/
	From C_DUTYTABLE_REWARD 
	Where 1=1 
	--篩選年月 C_OVERTIMEMONTH_HOUR
	--MZ_YEARRANGE 上半年為U 下班年為D
	--EX: 111U  111D
	AND (MZ_YEAR+MZ_YEARRANGE) = @QryYear_AND_RANGE
	--加班的現服單位,這邊可能會有點問題,敘獎應該是以編制單位為主,可是目前沒有編制單位的紀錄欄位
	--機關代碼 EX:382133200C	新北市政府警察局海山分局
	AND MZ_EXAD= @MZ_AD
	--單位代碼
	AND MZ_EXUNIT= @MZ_UNIT
	group by MZ_ID /*身分證號*/
)

--新語法:取得業務加班時數、金額資料
Select NVL(AKAD.MZ_KCHI,AKAD2.MZ_KCHI) as 機關 /*機關名,先以C_OVERTIME_BASE為主,C_DUTYMONTHOVERTIME_HOUR次之*/ 
	, NVL(AKK.MZ_KCHI,AKK2.MZ_KCHI) as 單位   /*單位名,先以C_OVERTIME_BASE為主,C_DUTYMONTHOVERTIME_HOUR次之*/   
	,AKS.MZ_KCHI as 職稱 /*職稱*/
	,AD.MZ_ID as 身分證號 /*身分證號*/
	,AD.MZ_NAME as 姓名/*姓名*/
	--(1)實際超勤時數(C_DUTYMONTHOVERTIME_HOUR +  C_OVERTIME_BASE)：查詢機關/單位內時間區間（MZ_YEAR+MZ_MONTH）的MZ_BUDGET_HOUR + 超勤補休(OVER_TYPE = ‘OTU’)，查詢機關/單位時間區間(OVER_DAY)的OVER_TOTAL÷60取整數部分。	
	,NVL(a.sum_mz_budget_hour,0) as 實際超勤時數
	--(2)實際支領超勤加班費時數(C_DUTYMONTHOVERTIME_HOUR)：查詢機關/單位時間區間(MZ_YEAR+MZ_MONTH）的MZ_REAL_HOUR。
	,NVL(a.sum_mz_real_hour,0)  as 實際支領超勤
	--(3)申請超勤補休時數(C_OVERTIME_BASE)：限超勤補休(OVER_TYPE = ‘OTU’)，查詢機關/單位時間區間(OVER_DAY)的 REST_HOUR÷60取整數部分。
	,NVL(b.sum_rest_hour,0)  as 申請超勤補休時數
	--(4)已申請敘獎時數(C_DUTYTABLE_REWARD)：僅提供查詢條件為上、下半年度的查詢才有值，查詢機關/單位時間區間（MZ_YEAR+MZ_YEARRANGE）的MZ_HOUT_AWARD值
	,NVL(c.sum_mz_hout_award,0)  as 已申請敘獎時數
	--(5)累積剩餘時數(C_DUTYTABLE_REWARD)：。僅提供查詢條件為上、下半年度的查詢才有值，查詢機關/單位時間區間（MZ_YEAR+MZ_YEARRANGE）的MZ_HOUR_H1 值
	,NVL(c.sum_mz_hour_h1_left,0)  as 累積剩餘時數
	--(6)支領金額(C_DUTYMONTHOVERTIME_HOUR)：查詢機關/單位時間區間（MZ_YEAR+MZ_MONTH）的MZ_OVERTIME_PAY值。
	,NVL(a.sum_mz_overtime_pay,0)  as 支領金額
From 
--基本資料
A_DLBASE AD
--統計-每日加班資訊 以加班資料為基準,有資料的才出現在名單上
left join SUM_C_OVERTIME_BASE B on B.MZ_ID=AD.MZ_ID
--統計-加班資訊月摘要
left join SUM_C_DUTYMONTHOVERTIME_HOUR A on A.MZ_ID=AD.MZ_ID
left join SUM_C_DUTYTABLE_REWARD C on C.MZ_ID=AD.MZ_ID
left join SUM_c_dutytable_personal d on d.MZ_ID=AD.MZ_ID
--職稱
left join A_KTYPE AKS on AKS.MZ_KCODE = AD.MZ_OCCC AND AKS.MZ_KTYPE = '26'
----兩種資料來源各自有自己的機關單位,但理論上應該都要一樣,
--機關 1 
left join A_KTYPE AKAD on AKAD.MZ_KCODE = B.MZ_AD AND AKAD.MZ_KTYPE = '04'
--單位 1
left join A_KTYPE AKK on AKK.MZ_KCODE = B.MZ_UNIT AND AKK.MZ_KTYPE = '25' 
--機關  2
left join A_KTYPE AKAD2 on AKAD2.MZ_KCODE = A.MZ_AD AND AKAD2.MZ_KTYPE = '04'
--單位 2
left join A_KTYPE AKK2 on AKK2.MZ_KCODE = A.MZ_UNIT AND AKK2.MZ_KTYPE = '25' 
Where 1=1 
--C_OVERTIME_BASE, C_DUTYMONTHOVERTIME_HOUR 兩種統計來源,只要其中一種出現即可
AND (B.MZ_ID is not null OR A.MZ_ID is not null)
--排除掉特定職稱
AND AD.MZ_OCCC!='Z011' 
";
            //職等判斷
            if (Qry.MZ_SRANK != "0")
            {
                switch (Qry.MZ_SRANK)
                {
                    case "1":
                        SQL += @"AND (AD.MZ_SRANK like 'G%') ";
                        break;
                    case "2":
                        SQL += @"AND (AKS.MZ_SRANK like 'P%' OR AKS.MZ_SRANK like 'B%') ";
                        break;
                    case "3":
                        SQL += @"AND (AKS.MZ_SRANK like 'J%' OR AKS.MZ_SRANK like 'M%') ";
                        break;
                    default:
                        break;
                }
            }
            SQL += @"
--身分證號判斷
AND AD.MZ_ID=@MZ_ID
order by AD.MZ_ID
";

            //擷取參數
            List<SqlParameter> list_sqlPrams = new List<SqlParameter>();
            //如果 年月 起 有輸入
            if (string.IsNullOrEmpty(Qry.QryYearMonth_Beg) == false)
            {
                list_sqlPrams.Add(new SqlParameter("QryYearMonth_Beg", Qry.QryYearMonth_Beg));
            }
            //如果 年月 訖 有輸入
            if (string.IsNullOrEmpty(Qry.QryYearMonth_End) == false)
            {
                //要做一點特殊處理
                // EX: 11106 => 1110699 ,這樣比大小的時候,六月底的日期也可以納入
                string tmp = Qry.QryYearMonth_End + "99";
                list_sqlPrams.Add(new SqlParameter("QryYearMonth_End", tmp));
            }
            //特殊 C_DUTYTABLE_REWARD 資料表的 上下半年呈現比較特殊: 111年上半年 111U / 111年下半年 111D
            string YearRange = GetYearRange(Qry.QryYearMonth_Beg, Qry.QryYearMonth_End);
            if (string.IsNullOrEmpty(YearRange) == false)
            {
                list_sqlPrams.Add(new SqlParameter("QryYear_AND_RANGE", YearRange));
            }
            else
            {   //如果判定失敗還是要下條件,故意給一個不會成立的內容,讓他查不到即可
                list_sqlPrams.Add(new SqlParameter("QryYear_AND_RANGE", "9999"));
            }

            //如果 機關 有輸入
            if (string.IsNullOrEmpty(Qry.MZ_EXAD) == false)
            {
                list_sqlPrams.Add(new SqlParameter("MZ_AD", Qry.MZ_EXAD));
            }
            //如果 單位 有輸入
            if (string.IsNullOrEmpty(Qry.MZ_EXUNIT) == false)
            {
                list_sqlPrams.Add(new SqlParameter("MZ_UNIT", Qry.MZ_EXUNIT));
            }
            //如果 身分證號 有輸入
            if (string.IsNullOrEmpty(Qry.MZ_ID) == false)
            {
                list_sqlPrams.Add(new SqlParameter("MZ_ID", Qry.MZ_ID));
            }

            //判斷參數是否存在,如果不存在,則將之移除,不做判定
            if (list_sqlPrams.Exists(x => x.ParameterName == "QryYearMonth_Beg") == false)
            {
                SQL = SQL.Replace("AND (MZ_YEAR+MZ_MONTH) >= @QryYearMonth_Beg", "");
                SQL = SQL.Replace("AND OVER_DAY >= @QryYearMonth_Beg", "");
            }
            if (list_sqlPrams.Exists(x => x.ParameterName == "QryYearMonth_End") == false)
            {
                SQL = SQL.Replace("AND (MZ_YEAR+MZ_MONTH) < @QryYearMonth_End", "");
                SQL = SQL.Replace("AND OVER_DAY < @QryYearMonth_End", "");
            }
            if (list_sqlPrams.Exists(x => x.ParameterName == "MZ_AD") == false)
            {
                SQL = SQL.Replace("AND MZ_AD= @MZ_AD", "");
                SQL = SQL.Replace("AND MZ_EXAD= @MZ_AD", "");
            }
            if (list_sqlPrams.Exists(x => x.ParameterName == "MZ_UNIT") == false)
            {
                SQL = SQL.Replace("AND MZ_UNIT= @MZ_UNIT", "");
                SQL = SQL.Replace("AND MZ_EXUNIT= @MZ_UNIT", "");
            }
            if (list_sqlPrams.Exists(x => x.ParameterName == "MZ_ID") == false)
            {
                SQL = SQL.Replace("AND AD.MZ_ID=@MZ_ID", "");
            }

            DataTable tempDT = o_DBFactory.ABC_toTest.Create_Table(SQL, list_sqlPrams);
            //特殊:某個欄位因為太長會在Oracle中噴錯,所以抓出來之後還要再改
            tempDT.Columns["實際支領超勤"].ColumnName = "實際支領超勤加班費時數";
            return tempDT;
        }

        /// <summary>
        /// 判斷輸入的年月字串組合 是上半年還是 下半年?
        /// EX:
        /// 11101,11106 => 111U
        /// 11007,11012 => 110D  
        /// </summary>
        /// <param name="beg">年月字串 起,EX:11101</param>
        /// <param name="end">年月字串 訖,EX:11106</param>
        /// <returns>回傳 U:上半年 D:下半年 "":無法判定</returns>
        public static string GetYearRange(string beg, string end)
        {
            //過濾無值 和 長度不正確的情境
            beg = beg ?? "";
            if (beg.Length != 5)
                return "";
            end = end ?? "";
            if (end.Length != 5)
                return "";
            //抓取年度,需要是同一個年度,不然沒有意義
            if (beg.Substring(0, 3) != end.Substring(0, 3))
                return "";

            //抓取 月份 判斷上下半年
            string temp = beg.Substring(3, 2) + end.Substring(3, 2);
            string UD = "";
            if (temp == "0106")
                UD = "U";
            else if (temp == "0712")
                UD = "D";
            else
                return "";//月份格式也不對

            //回傳結果,EX: 111U
            return beg.Substring(0, 3) + UD;
        }

    }
}