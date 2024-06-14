using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LinqToExcel;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.Util;
using NPOI;
using NPOI.HSSF;
using System.Collections;


namespace TPPDDB._2_salary.rpt
{
    public partial class B_monthpay_different_rpt : System.Web.UI.Page
    {

        string strGroup_Data_Function
        {
            get { return ViewState["B_strGID"] != null ? ViewState["B_strGID"].ToString() : string.Empty; }
            set { ViewState["B_strGID"] = value; }

        }

        string strType
        {
            get { return ViewState["strType"] != null ? ViewState["strType"].ToString() : string.Empty; }
            set { ViewState["strType"] = value; }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SalaryPublic.checkPermission();
                strType = Request["TYPE"];

                switch (strType)
                {
                    case "DD"://個人應發(加項)差異表
                        lb_Title.Text = "每月薪資個人應發差異明細表";
                        break;
                    case "LD"://機關應發(加項)差異表
                        lb_Title.Text = "每月薪資機關應發差異總表";
                        break;
                    case "DTOD"://個人應扣(減項)差異表
                        Tr_Dtype.Visible = true;
                        lb_Title.Text = "每月薪資個人應扣差異明細表";
                        break;
                    case "LTOD"://機關應扣(減項)差異表
                        Tr_Dtype.Visible = true;
                        lb_Title.Text = "每月薪資機關應扣差異總表";
                        break;

                }

                SalaryPublic.fillDropDownList(ref this.DropDownList_AD);

                DropDownList_AD.SelectedValue = Session["ADPPAY_AD"].ToString();//Session["ADPMZ_EXAD"]

                strGroup_Data_Function = TPMPermissions._strGroupData_ID(int.Parse(Session["TPM_MID"].ToString()), int.Parse(Request.QueryString["TPM_FION"].ToString()));

                switch (strGroup_Data_Function)
                {

                    case "C":
                    case "D":
                    case "E":
                        //matthew 如果發薪單位在中和分局 下拉不鎖 SalaryPublic.fillDropDownList(ref this.DropDownList_AD); 這個方法已經把中和一跟中和二加進來
                        if (Session["ADPPAY_AD"].ToString() != "382133600C")
                        {
                            DropDownList_AD.Enabled = false;
                        }                     
                        break;


                }


                DateTime DateTime_Date = DateTime.Now;


                TextBox_YEAR.Text = DateTime_Date.Year - 1911 + DateTime_Date.Month.ToString().PadLeft(2, '0');
                TextBox_YEAR2.Text = DateTime_Date.AddMonths(-1).Year - 1911 + DateTime_Date.AddMonths(-1).Month.ToString().PadLeft(2, '0');


            }
        }

        private String strAD
        {
            get
            {
                return DropDownList_AD.SelectedValue;
            }
        }

        private string strAYEAR
        {
            get
            {
                return TextBox_YEAR.Text.PadLeft(3, '0');
            }
        }


        protected void Button_SEARCH_Click(object sender, EventArgs e)
        {
            string error = "";
            if (TextBox_YEAR.Text.Length < 5 || TextBox_YEAR2.Text.Length < 5)
            {
                error += @"請輸入限定日期格式！ \n";


            }

            if (error != "")
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), "msg1", "alert('" + error + "');", true);

                return;
            }


            string tmp_url = string.Empty;


            switch (strType)
            {
                case "DD"://個人應發(加項)差異表
                    tmp_url = "../B_rpt.aspx?fn=monthpay_detail_different&TPM_FION=" + Request["TPM_FION"] +
                "&DATE1=" + TextBox_YEAR.Text + "&DATE2=" + TextBox_YEAR2.Text + "&AD=" + strAD;
                    break;
                case "LD"://機關應發(加項)差異表
                    tmp_url = "../B_rpt.aspx?fn=monthpay_list_different&TPM_FION=" + Request["TPM_FION"] +
               "&DATE1=" + TextBox_YEAR.Text + "&DATE2=" + TextBox_YEAR2.Text + "&AD=" + strAD;
                    break;
                case "DTOD"://個人應扣(減項)差異表
                    tmp_url = "../B_rpt.aspx?fn=monthpaytakeoff_detail_different&TPM_FION=" + Request["TPM_FION"] +
                "&DATE1=" + TextBox_YEAR.Text + "&DATE2=" + TextBox_YEAR2.Text + "&AD=" + strAD + "&TYPE=" + ddl_Dtype.SelectedValue;
                    break;
                case "LTOD"://機關應扣(減項)差異表
                    tmp_url = "../B_rpt.aspx?fn=monthpaytakeoff_list_different&TPM_FION=" + Request["TPM_FION"] +
               "&DATE1=" + TextBox_YEAR.Text + "&DATE2=" + TextBox_YEAR2.Text + "&AD=" + strAD + "&TYPE=" + ddl_Dtype.SelectedValue;
                    break;



            }

            if (!string.IsNullOrEmpty(tmp_url))
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
        }





        #region //20140414 明細表撈來,舊的
        //protected void Button_CANCEL_Click(object sender, EventArgs e)
        //{
        //    Response.Redirect("B_monthpaydetail_rpt.aspx");
        //}

        //EXCEL匯出2014/06/17 BY青
        protected void btn_Export_Click(object sender, EventArgs e)
        {
            //20140414 明細表撈來,舊的
            
            DataTable tempDT = new DataTable();
            DataTable dt = new DataTable();

            switch (strType)
            {
                case "DD"://個人應發(加項)差異表
                    dt = B_RPT.monthpay_detail_different.doSearch(TextBox_YEAR.Text, TextBox_YEAR2.Text, DropDownList_AD.SelectedValue);
                    dt.Columns.Remove("MZ_UNIT");
                    dt.Columns.Remove("MZ_UNIT1");
                   
                   
                    string[] DataDD = { "NUM", "PAY_AD", "PAY_UNIT", "AMONTH", "IDCARD", "MZ_SRANK", "SLVC", "SLVC_SPT", "MZ_OCCC", "MZ_NAME",
                    "ACCOUNT", "MZ_OFF_DATA", "SALARYPAY1", "BOSS", "PROFESS", "WORKP", "TECHNICS", "BONUS", "ADVENTIVE", "FAR", "ELECTRIC",
                    "OTHERADD", "TOTAL", "MZ_POLNO", "NOTE", "PAY_AD1", "PAY_UNIT1", "AMONTH1", "IDCARD1",
                    "MZ_SRANK1", "SLVC1", "SLVC_SPT1", "MZ_OCCC1", "MZ_NAME1", "ACCOUNT1", "MZ_OFF_DATA1", "SALARYPAY11", "BOSS1",
                    "PROFESS1", "WORKP1", "TECHNICS1", "BONUS1", "ADVENTIVE1", "FAR1", "ELECTRIC1", "OTHERADD1", "TOTAL1", "MZ_POLNO1", "NOTE1"};
               
                    ReOrderTableColumn(dt,DataDD);


                    RenderDataTableToExcel(dt);
                    //Excel.Dt2Excel(dt, "個人應發(加項)差異表");
                    break;
                case "LD"://機關應發(加項)差異表
                    dt = B_RPT.monthpay_list_different.doSearch(TextBox_YEAR.Text, TextBox_YEAR2.Text, DropDownList_AD.SelectedValue);
                    dt.Columns.Remove("UNITCODE");
                    dt.Columns.Remove("UNITCODE1");

                    string LD = @"NUM PAY_AD UNIT AMONTH PEOPLE_ACCOUNT SALARYPAY1 BOSS PROFESS WORKP TECHNICS BONUS ADVENTIVE FAR ELECTRIC OTHERADD TOTAL PAY_AD1 UNIT1 AMONTH1 PEOPLE_ACCOUNT1 SALARYPAY11 BOSS1 PROFESS1 WORKP1 TECHNICS1 BONUS1 ADVENTIVE1 FAR1 ELECTRIC1 OTHERADD1 TOTAL1";
                    string[] DataLD;
                    DataLD = LD.Split(' ');
                    ReOrderTableColumn(dt, DataLD);
                    RenderDataTableToExcel(dt);    
                   // Excel.Dt2Excel(dt, "機關應發(加項)差異表");
                    break;
                case "DTOD"://個人應扣(減項)差異表
                    dt = B_RPT.monthpaytakeoff_detail_different.doSearch(TextBox_YEAR.Text, TextBox_YEAR2.Text, DropDownList_AD.SelectedValue, ddl_Dtype.SelectedValue);
                    dt.Columns.Remove("MZ_UNIT");
                    dt.Columns.Remove("MZ_UNIT1");
                    string DTOD = @"NUM PAY_AD PAY_UNIT AMONTH IDCARD MZ_SRANK SLVC SLVC_SPT MZ_SPT MZ_OCCC MZ_NAME ACCOUNT MZ_OFF_DATA INSURANCEPAY HEALTHPAY HEALTHPAY1 CONCUR3PAY MONTHPAY_TAX MONTHPAY TAX EXTRA01 EXTRA02 EXTRA03 EXTRA04 EXTRA05 EXTRA06 EXTRA07 EXTRA08 EXTRA09 OTHERMINUS ADD_SUM DES_SUM TOTAL MZ_POLNO NOTE PAY_AD1 PAY_UNIT1 AMONTH1 IDCARD1 MZ_SRANK1 SLVC1 SLVC_SPT1 MZ_SPT1 MZ_OCCC1 MZ_NAME1 ACCOUNT1 MZ_OFF_DATA1 INSURANCEPAY1 HEALTHPAY2 HEALTHPAY11 CONCUR3PAY1 MONTHPAY_TAX1 MONTHPAY1 TAX1 EXTRA011 EXTRA021 EXTRA031 EXTRA041 EXTRA051 EXTRA061 EXTRA071 EXTRA081 EXTRA091 OTHERMINUS1 ADD_SUM1 DES_SUM1 TOTAL1 MZ_POLNO1 NOTE1";
                    string[] DTODdata;

                    DTODdata = DTOD.Split(' ');
                    ReOrderTableColumn(dt, DTODdata);
                    RenderDataTableToExcel(dt);     
                    //Excel.Dt2Excel(dt, "個人應扣(減項)差異表");
                    break;
                case "LTOD"://機關應扣(減項)差異表
                    dt = B_RPT.monthpaytakeoff_list_different.doSearch(TextBox_YEAR.Text, TextBox_YEAR2.Text, DropDownList_AD.SelectedValue, ddl_Dtype.SelectedValue);
                    dt.Columns.Remove("UNITCODE");
                    dt.Columns.Remove("UNITCODE1");
                    string LTOD = @"NUM PAY_AD UNIT AMONTH PEOPLE_ACCOUNT INSURANCEPAY HEALTHPAY HEALTHPAY1 CONCUR3PAY MONTHPAY_TAX MONTHPAY TAX EXTRA01 EXTRA02 EXTRA03 EXTRA04 EXTRA05 EXTRA06 EXTRA07 EXTRA08 EXTRA09 OTHERMINUS ADD_SUM DES_SUM TOTAL PAY_AD1 UNIT1 AMONTH1 PEOPLE_ACCOUNT1 INSURANCEPAY1 HEALTHPAY2 HEALTHPAY11 CONCUR3PAY1 MONTHPAY_TAX1 MONTHPAY1 TAX1 EXTRA011 EXTRA021 EXTRA031 EXTRA041 EXTRA051 EXTRA061 EXTRA071 EXTRA081 EXTRA091 OTHERMINUS1 ADD_SUM1 DES_SUM1 TOTAL1";
                    string[] LTODdata;
                    LTODdata = LTOD.Split(' ');
                    ReOrderTableColumn(dt, LTODdata);
                    RenderDataTableToExcel(dt);    
                    //Excel.Dt2Excel(dt, "機關應扣(減項)差異表");
                    break;

            }

        }


        //判斷欄位名稱 2014/06/17 By青
        public Stream RenderDataTableToExcel(DataTable SourceTable)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            MemoryStream ms = new MemoryStream();
            HSSFSheet sheet = workbook.CreateSheet();
            HSSFRow headerRow = sheet.CreateRow(0);
            headerRow.HeightInPoints = 20;
    

            HSSFCellStyle borderStyle = workbook.CreateCellStyle();
            borderStyle.Alignment = HSSFCellStyle.ALIGN_CENTER;//水平置中
            borderStyle.VerticalAlignment = HSSFCellStyle.VERTICAL_CENTER;
            borderStyle.WrapText = true;
            borderStyle.BorderTop = HSSFCellStyle.BORDER_THIN;
            borderStyle.BorderLeft = HSSFCellStyle.BORDER_THIN;
            borderStyle.BorderBottom = HSSFCellStyle.BORDER_THIN;
            borderStyle.BorderRight = HSSFCellStyle.BORDER_THIN;


           // 個人應發(加項)差異表
            string[] DD = new string[] { "編號", "機關", "單位","當前年月", "身分證字號",
                "薪俸職等", "俸階","俸點","職稱","姓名","銀行帳戶","在職天數","月支數額","主管加給",
                "專業加給","警勤加給","技術加給","工作獎助金","外事加給","偏遠加給","繁重加給",
                "其他應發","應發總計","員工編號","備註","機關","單位","比較年月","身分證字號",
                "薪俸職等","俸階","俸點","職稱","姓名","銀行帳戶","在職天數","月支數額","主管加給",
                "專業加給","警勤加給","技術加給","工作獎助金","外事加給","偏遠加給","繁重加給",
                "其他應發","應發總計","員工編號","備註"};




            //機關應發(加項)差異表
            string[] LD = new string[]{"編號","機關","單位","當前年月","機關人數","月支數額","主管加給",
                "專業加給","警勤加給","技術加給","工作獎助金","外事加給","偏遠加給","繁重加給","其他應發",
                "應發總計","機關","單位","比較年月","機關人數","月支數額","主管加給","專業加給",
                "警勤加給","技術加給","工作獎助金","外事加給","偏遠加給","繁重加給","其他應發","應發總計"};



            //個人應扣(減項)差異表
            string[] DTOD = new string[] { "編號", "機關", "單位", "當前年月", "身分證字號", 
                "薪俸職等", "俸階", "俸等", "俸點", "職稱", "姓名", "銀行帳戶", "在職天數", "公(勞)保費",
                "健保費", "健保費補差扣款", "退撫金費", "薪資扣款1", "薪資扣款2", "薪津所得稅", "法院扣款", 
                "國宅貸款", "銀行貸款", "分期付款", "優惠存款", "員工宿舍費", "福利互助金費", "伙食費",
                "退撫金貸款", "其他應扣", "應發總計", "應扣總計", "實發金額", "員工編號", "備註", "機關",
                "單位","比較年月", "身分證字號", "薪俸職等", "俸階", "俸等", "俸點", "職稱", 
                "姓名", "銀行帳戶", "	在職天數", "	公(勞)保費", "健保費", "健保費補差扣款 ", "退撫金費",
                "薪資扣款1", "薪資扣款2", "薪津所得稅", "法院扣款", "國宅貸款", "銀行貸款", "分期付款", "優惠存款",
                "員工宿舍費", "福利互助金費", "伙食費", "退撫金貸款", "其他應扣", "應發總計", "應扣總計", 
                "實發金額", "員工編號", "備註" };

            //機關應扣(減項)差異表
            string[] LTOD = new string[] { "編號", "機關","單位", "當前年月", "機關人數", 
                "公(勞)保費", "健保費", "健保費補差扣款", "退撫金費", "薪資扣款1", "薪資扣款2", "薪津所得稅", 
                "法院扣款", "國宅貸款", "銀行貸款", "分期付款", "優惠存款", "員工宿舍費", "福利互助金費", "伙食費",
                "退撫金貸款", "其他應扣", "應發總計", "應扣總計", "實發金額", "機關", "單位","比較年月", 
                 "機關人數", "公(勞)保費", "健保費", "健保費補差扣款", "退撫金費", "薪資扣款1", "薪資扣款2",
                "薪津所得稅", "法院扣款", "國宅貸款", "銀行貸款", "分期付款", "優惠存款", "員工宿舍費",
                "福利互助金費", "伙食費", "退撫金貸款", "其他應扣", "應發總計", "應扣總計", "實發金額" };
            

            string FileName = "";
            switch (strType)
            {
                case "DD"://個人應發(加項)差異表 

                    CellRangeAddress regionDD = new CellRangeAddress(0, 0, 0, DD.Length / 2);
                    CellRangeAddress DDright = new CellRangeAddress(0, 0, DD.Length / 2 + 1, DD.Length - 1);
                    sheet.AddMergedRegion(regionDD);
                    sheet.AddMergedRegion(DDright);
                    headerRow.CreateCell(0).SetCellValue("當前年月");
                    //headerRow.GetCell(0).CellStyle = styleRow1;
                    headerRow.GetCell(0).CellStyle = borderStyle;
                    headerRow.CreateCell(DD.Length / 2 + 1).SetCellValue("比較年月");
                    headerRow.GetCell(DD.Length / 2 + 1).CellStyle = borderStyle;
                    

                    FileName = "個人應發(加項)差異表";
                    // handling header.

                    for (int i = 0; i < DD.Length; i++)
                    {
                        HSSFCell cell = sheet.CreateRow(1).CreateCell(i);
                        cell.SetCellValue(DD[i]);
                        //  cell.CellStyle = styleRow1;
                        cell.CellStyle = borderStyle;
                        sheet.SetColumnWidth(i, 50 * 256);
                    }

                    break;

                case "LD"://機關應發(加項)差異表

                    CellRangeAddress regionLD = new CellRangeAddress(0, 0, 0, LD.Length / 2);
                    sheet.AddMergedRegion(regionLD);
                    CellRangeAddress LDright = new CellRangeAddress(0, 0, LD.Length / 2 + 1, LD.Length - 1);
                    sheet.AddMergedRegion(LDright);
                    headerRow.CreateCell(0).SetCellValue("當前年月");
                    headerRow.GetCell(0).CellStyle = borderStyle;
                    headerRow.CreateCell(LD.Length / 2 + 1).SetCellValue("比較年月");
                    headerRow.GetCell(LD.Length / 2 + 1).CellStyle = borderStyle;

                    FileName = "個人應發(加項)差異表";
                    // handling header.

                    for (int i = 0; i < LD.Length; i++)
                    {
                        HSSFCell cell = sheet.CreateRow(1).CreateCell(i);
                        cell.SetCellValue(LD[i]);
                        cell.CellStyle = borderStyle;
                        sheet.SetColumnWidth(i, 20 * 256);
                    }

                    FileName = "機關應發(加項)差異表";
                    break;

                case "DTOD"://個人應扣(減項)差異表
                    CellRangeAddress regionDTOD = new CellRangeAddress(0, 0, 0, DTOD.Length / 2);
                    sheet.AddMergedRegion(regionDTOD);
                    CellRangeAddress DTODDright = new CellRangeAddress(0, 0, DTOD.Length / 2 + 1, DTOD.Length - 1);
                    sheet.AddMergedRegion(DTODDright);

                    headerRow.CreateCell(0).SetCellValue("當前年月");
                    headerRow.GetCell(0).CellStyle = borderStyle;
                    headerRow.CreateCell(DTOD.Length / 2 + 1).SetCellValue("比較年月");
                    headerRow.GetCell(DTOD.Length / 2 + 1).CellStyle = borderStyle;

                    FileName = "個人應發(減項)差異表";
                    for (int i = 0; i < DTOD.Length; i++)
                    {
                        HSSFCell cell = sheet.CreateRow(1).CreateCell(i);
                        cell.SetCellValue(DTOD[i]);
                        cell.CellStyle = borderStyle;
                        sheet.SetColumnWidth(i, 50 * 256);
                    }

                    break;
                // handling header.

                case "LTOD"://"機關應扣(減項)差異表"
                    CellRangeAddress regionLTOD = new CellRangeAddress(0, 0, 0, LTOD.Length / 2);
                    sheet.AddMergedRegion(regionLTOD);
                    CellRangeAddress LTODright = new CellRangeAddress(0, 0, LTOD.Length / 2 + 1, LTOD.Length - 1);
                    sheet.AddMergedRegion(LTODright);
                    headerRow.CreateCell(0).SetCellValue("當前年月");
                    headerRow.GetCell(0).CellStyle = borderStyle;
                    
                    headerRow.CreateCell(LTOD.Length / 2 + 1).SetCellValue("比較年月");
                    headerRow.GetCell(LTOD.Length / 2 + 1).CellStyle = borderStyle;

                    for (int i = 0; i < LTOD.Length; i++)
                    {
                        HSSFCell cell = sheet.CreateRow(1).CreateCell(i);
                        cell.SetCellValue(LTOD[i]);
                        cell.CellStyle = borderStyle;
                        sheet.SetColumnWidth(i, 50 * 256);
                    }

                    FileName = "機關應扣(減項)差異表";
                    break;


            }


            // handling value.
            int rowIndex = 2;

            foreach (DataRow row in SourceTable.Rows)
            {

                HSSFRow dataRow = sheet.CreateRow(rowIndex);
                String PAY_AD = o_A_KTYPE.CODE_TO_NAME(row["PAY_AD"].ToString(), "04");
                String PAY_AD1 = o_A_KTYPE.CODE_TO_NAME(row["PAY_AD1"].ToString(), "04");


                if (PAY_AD != "新北市政府警察局")
                {
                    PAY_AD = PAY_AD.Replace("新北市政府警察局", "");
                }

                if (PAY_AD1 != "新北市政府警察局")
                {
                    PAY_AD1 = PAY_AD1.Replace("新北市政府警察局", "");
                }
                row["PAY_AD"] = PAY_AD;
                row["PAY_AD1"] = PAY_AD1;



                foreach (DataColumn column in SourceTable.Columns)
                {
                    dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                    dataRow.GetCell(column.Ordinal).CellStyle = borderStyle;
                    sheet.SetColumnWidth(column.Ordinal, 20 * 256);
                    dataRow.HeightInPoints = 30;
                }


                rowIndex++;
            }





            workbook.Write(ms);

            ms.Flush();
            ms.Position = 0;


            if (HttpContext.Current.Request.Browser.Browser == "IE")
                FileName = HttpContext.Current.Server.UrlPathEncode(FileName);

            HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xls", FileName));
            // 設定強制下載標頭。
            FileName = HttpUtility.UrlEncode(FileName, System.Text.Encoding.UTF8);
            // 輸出檔案。
            HttpContext.Current.Response.BinaryWrite(ms.ToArray());

            sheet = null;
            headerRow = null;
            workbook = null;

            return ms;
        }




        // string date = strAYEAR + strMONTH.PadLeft(2, '0');

        //if (DropDownList_UNIT.SelectedIndex == 0)
        //{
        //    strSQL = string.Format(@"SELECT PAY_UNIT 單位, MZ_POLNO 員工編號, IDCARD 身份證字號, MZ_SRANK 職等, SLVC_SPT 俸階, MZ_OCCC 職稱, MZ_NAME 姓名, ACCOUNT 帳戶, ADD_SUM 應發金額, INSURANCEPAY 保險費, HEALTHPAY 健保費, CONCUR3PAY 退撫金, TAX 薪津所得稅, HEALTHPAY1 健保費補扣款, MONTHPAY_TAX 薪資扣款1, MONTHPAY 薪資扣款2, EXTRA01 法院扣款, EXTRA02 國宅貸款, EXTRA03 銀行貸款, EXTRA04 分期付款, EXTRA05 優惠存款, EXTRA06 員工宿舍費, EXTRA07 福利互助金, EXTRA08 伙食費, EXTRA09 退撫金扣款, OTHERMINUS 其他應扣, DES_SUM 應扣金額, TOTAL 實發金額, NOTE 備註 FROM  VW_MONTHPAY_TAKEOFF_DETAIL_POL WHERE AMONTH='{0}' AND PAY_AD='{1}' ORDER BY MZ_UNIT, MZ_POLNO ", date, strAD);
        //}
        //else
        //{
        //    strSQL = string.Format(@"SELECT PAY_UNIT 單位, MZ_POLNO 員工編號, IDCARD 身份證字號, MZ_SRANK 職等, SLVC_SPT 俸階, MZ_OCCC 職稱, MZ_NAME 姓名, ACCOUNT 帳戶, ADD_SUM 應發金額, INSURANCEPAY 保險費, HEALTHPAY 健保費, CONCUR3PAY 退撫金, TAX 薪津所得稅, HEALTHPAY1 健保費補扣款, MONTHPAY_TAX 薪資扣款1, MONTHPAY 薪資扣款2, EXTRA01 法院扣款, EXTRA02 國宅貸款, EXTRA03 銀行貸款, EXTRA04 分期付款, EXTRA05 優惠存款, EXTRA06 員工宿舍費, EXTRA07 福利互助金, EXTRA08 伙食費, EXTRA09 退撫金扣款, OTHERMINUS 其他應扣, DES_SUM 應扣金額, TOTAL 實發金額, NOTE 備註 FROM  VW_MONTHPAY_TAKEOFF_DETAIL_POL WHERE AMONTH='{0}' AND PAY_AD='{1}' AND MZ_UNIT = '{2}' ORDER BY MZ_UNIT, MZ_POLNO ", date, strAD, strUNIT);
        //}

        //if (DropDownList_U.SelectedIndex == 0)
        //{
        //    strSQL = string.Format(@"SELECT PAY_UNIT 單位, MZ_POLNO 員工編號, IDCARD 身份證字號, MZ_SRANK 職等, SLVC_SPT 俸階, MZ_OCCC 職稱, MZ_NAME 姓名, ACCOUNT 帳戶, ADD_SUM 應發金額, INSURANCEPAY 保險費, HEALTHPAY 健保費, CONCUR3PAY 退撫金, TAX 薪津所得稅, HEALTHPAY1 健保費補扣款, MONTHPAY_TAX 薪資扣款1, MONTHPAY 薪資扣款2, EXTRA01 法院扣款, EXTRA02 國宅貸款, EXTRA03 銀行貸款, EXTRA04 分期付款, EXTRA05 優惠存款, EXTRA06 員工宿舍費, EXTRA07 福利互助金, EXTRA08 伙食費, EXTRA09 退撫金扣款, OTHERMINUS 其他應扣, DES_SUM 應扣金額, TOTAL 實發金額, NOTE 備註 FROM  VW_MONTHPAY_TAKEOFF_DETAIL_POL WHERE AMONTH='{0}' AND PAY_AD='{1}' ORDER BY MZ_UNIT, MZ_POLNO ", date, strAD);
        //}
        //else
        //{
        //    strSQL = string.Format(@"SELECT PAY_UNIT 單位, MZ_POLNO 員工編號, IDCARD 身份證字號, MZ_SRANK 職等, SLVC_SPT 俸階, MZ_OCCC 職稱, MZ_NAME 姓名, ACCOUNT 帳戶, ADD_SUM 應發金額, INSURANCEPAY 保險費, HEALTHPAY 健保費, CONCUR3PAY 退撫金, TAX 薪津所得稅, HEALTHPAY1 健保費補扣款, MONTHPAY_TAX 薪資扣款1, MONTHPAY 薪資扣款2, EXTRA01 法院扣款, EXTRA02 國宅貸款, EXTRA03 銀行貸款, EXTRA04 分期付款, EXTRA05 優惠存款, EXTRA06 員工宿舍費, EXTRA07 福利互助金, EXTRA08 伙食費, EXTRA09 退撫金扣款, OTHERMINUS 其他應扣, DES_SUM 應扣金額, TOTAL 實發金額, NOTE 備註 FROM  VW_MONTHPAY_TAKEOFF_DETAIL_POL WHERE AMONTH='{0}' AND PAY_AD='{1}' AND MZ_UNIT = '{2}' ORDER BY MZ_UNIT, MZ_POLNO ", date, strAD, strUNIT);
        //}

        public static void ReOrderTableColumn(DataTable objDt, string[] strNewColumnsOrder)
        {

            objDt.PrimaryKey = null;



            for (int i = 0; i < strNewColumnsOrder.Length; i++)

                objDt.Columns[strNewColumnsOrder[i]].SetOrdinal(i);



            int intCount = objDt.Columns.Count;

            for (int i = strNewColumnsOrder.Length; i < intCount; i++)

                objDt.Columns.RemoveAt(strNewColumnsOrder.Length);

        }


        //DataView轉DataTable
        //public DataTable CreateTable(DataView obDataView)
        //{
        //    if (null == obDataView)
        //    {
        //        throw new ArgumentNullException
        //        ("DataView", "Invalid DataView object specified");
        //    }

        //    DataTable obNewDt = obDataView.Table.Clone();
        //    int idx = 0;
        //    string[] strColNames = new string[obNewDt.Columns.Count];

        //    foreach (DataColumn col in obNewDt.Columns)
        //    {
        //        strColNames[idx++] = col.ColumnName;
        //    }

        //    IEnumerator viewEnumerator = obDataView.GetEnumerator();
        //    while (viewEnumerator.MoveNext())
        //    {
        //        DataRowView drv = (DataRowView)viewEnumerator.Current;
        //        DataRow dr = obNewDt.NewRow();
        //        try
        //        {
        //            foreach (string strName in strColNames)
        //            {
        //                dr[strName] = drv[strName];
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine(ex.Message);
        //        }
        //        obNewDt.Rows.Add(dr);
        //    }

        //    return obNewDt;

        //}




        #endregion
    }

}
