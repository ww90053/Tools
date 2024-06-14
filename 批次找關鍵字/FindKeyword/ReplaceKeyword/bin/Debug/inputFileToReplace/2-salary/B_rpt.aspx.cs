using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using NPOI.HSSF.Util;
using NPOI.Util;
using NPOI.HSSF.UserModel;
using LinqToExcel;
using System.IO;




namespace TPPDDB._2_salary
{
    public partial class B_rpt : System.Web.UI.Page
    {
       
        int total = 0;
        DataTable rpt_dt = new DataTable();
        //CrystalDecisions.CrystalReports.Engine.ReportDocument report = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        CrystalDecisions.CrystalReports.Engine.ReportDocument report;
        protected void Page_Load(object sender, EventArgs e)
        {
              


            string strAD;
            string strTitle;
            string fkind = "";
            string filename = "";
            string filepath = "report/";

            if (Session["TITLE"] == null)
            {
                strAD = "";
                strTitle = "";
            }
            else
            {
                if (Session["SalaryReportAD"] != null)
                    strAD = Session["SalaryReportAD"].ToString();
                else
                    strAD = "";

                strTitle = Session["TITLE"].ToString();
            }

            //報表登入
            string db_ServerName;
            string db_UserID;
            string db_Password;
            string db_DatabaseName;

            db_ServerName = System.Configuration.ConfigurationManager.AppSettings["servername"];
            db_UserID = System.Configuration.ConfigurationManager.AppSettings["userid"];
            db_Password = System.Configuration.ConfigurationManager.AppSettings["pass"];
            db_DatabaseName = System.Configuration.ConfigurationManager.AppSettings["database"];

            if (Request["fn"] != null)
            {
                fkind = Request["fn"].ToString();
            }

            switch (fkind)
            {
             
                case "PersonalMonthSalary": //8.1每月薪資明細表
                    strTitle = strTitle + "每月薪資明細表";
                    filename = "B_PersonalMonthSalary.rpt";
                    break;
                case "RepairPay"://8.2薪資補發明細表
                    strTitle = strTitle + "薪資補發明細表";
                    filename = "B_PersonalMonthRepairSalary.rpt";
                    break;
                case "Sole"://8.3單一發放明細表
                    strTitle = "單一發放明細表";
                    filename = "B_SoleReport.rpt";
                    break;
                case "SalaryYear-Endbonus"://8.4年終獎金明細表
                    strTitle = "年終獎金明細表";
                    filename = "B_SalaryYear-Endbonus.rpt";
                    break;
                case "PersonalEffectDetail"://8.5考績獎金明細表
                    strTitle = "個人考績明細表";
                    filename = "B_PersonalEffectDetail.rpt";
                    break;
                case "SearchYearSalary"://8.6薪資年度總表
                    filename = "B_TotalSalary1.rpt";
                    break;
                case "insuranceStat"://????
                    filename = "B_InsuranceStatistics.rpt";
                    break;
                case "insuranceStat_2"://8.7保費統計表
                    filename = "B_InsuranceStatistics_2.rpt";
                    break;
                case "79"://8.8個人全年薪資明細表
                    filename = "B_PersonalDetail.rpt";
                    break;

                case "PersonalYearSalary":
                    filename = "B_PersonalYearSalary.rpt";
                    break;

                case "effect_detail":
                    strTitle = strTitle + "考績獎金明細表";
                    filename = "B_effectdetail.rpt";
                    break;
                case "effect_list":
                    strTitle = strTitle + "考績獎金印領清冊";
                    filename = "B_effectlist.rpt";
                    break;
                case "yearpay_detail":
                    strTitle += "年終獎金明細表";
                    filename = "B_yearendbonusdetail.rpt";
                    break;

                case "yearpay_list":
                    strTitle += "年終獎金總表";
                    filename = "B_yearendbonuslist.rpt";
                    break;

            

                #region 2.2 每月薪資作業

                case "monthpay_list"://每月薪資應發總表
                    //strTitle += "應發薪資總表";
                    filename = "B_monthpaylist.rpt";
                    break;

                case "monthpay_list2"://每月薪資應發總表()
                    //strTitle += "應發薪資總表";
                    filename = "B_monthpaylist.rpt";
                    break;

                case "monthpay_detail":
                    //strTitle += "應發薪資明細表";
                    filename = "B_monthpaydetail.rpt";
                    break;

                case "monthpaytakeoff_list"://每月薪資應扣總表
                    //strTitle += "實發薪資總表";
                    filename = "B_monthpaytakeofflist.rpt";
                    break;

                case "monthpaytakeoff_list2"://每月薪資應扣總表
                    //strTitle += "實發薪資總表";
                    filename = "B_monthpaytakeofflist.rpt";
                    break;

                case "monthpaytakeoff_detail": // 應扣
                    //strTitle += "實發薪資明細表";
                    filename = "B_monthpaytakeoffdetail.rpt";
                    break;

                #endregion 2.2 每月薪資作業

                #region 2.3 補發薪資作業

                
                case "repairpay_list"://2.3.2 補發薪資應發總表                     
                    filename = "B_monthpaylist.rpt";
                    break;

                case "repairpay_detail"://2.3.3 補發薪資明細表                   
                    filename = "B_monthpaydetail.rpt";
                    break;
                case "repairtakeoff_list":////2.3.4 補發薪資總表(應扣)                   
                    filename = "B_monthpaytakeofflist.rpt";
                    break;

                case "repairpaytakeoff_detail"://2.3.5 補發薪資應扣明細表                    
                    filename = "B_monthpaytakeoffdetail.rpt";
                    break;

                //應政府新預算編制分開職稱列印 20180410 by sky
                case "repairpay_list2"://2.3.2 補發薪資應發總表New
                    filename = "B_monthpaylist.rpt";
                    break;
                case "repairtakeoff_list2":////2.3.4 補發薪資總表(應扣)New
                    filename = "B_monthpaytakeofflist.rpt";
                    break;
                #endregion 2.3 補發薪資作業


                #region 每月薪資差異表

                case "monthpay_list_different"://每月薪資應發差異總表
                     strTitle += "應發薪資差異總表";
                    filename = "B_monthpaylist_different.rpt";
                    break;

                case "monthpay_detail_different":
                    strTitle += "薪資應發(加項)異動差異明細表";
                    filename = "B_monthpay_detail_different.rpt";
                    break;
                case "monthpaytakeoff_list_different"://每月薪資應扣差異總表
                    strTitle += "實發薪資差異總表";
                    filename = "B_monthpaytakeofflist_different.rpt";
                    break;

                case "monthpaytakeoff_detail_different": // 應扣
                    strTitle += "薪資實發(減項)異動差異明細表";
                    filename = "B_monthpaytakeoff_detail_different.rpt";
                    break;
                #endregion 每月薪資差異表

                case "salary_back_different"://轉帳回饋擋檢核 2014/06/18 BY青
                    strTitle += "轉帳回饋檔檢核差異明細表";
                    filename = "B_salary_back_different.rpt";
                    break;

                #region 2.9 金融機構轉存

                case "bank_list"://2.9金融機構轉存-機關團體戶存款單
                    //strTitle = "機關團體戶" + strTitle + "存款單";
                    filename = "B_banklist.rpt";
                    break;

                case "AmountDetail":
                    strTitle = strTitle + "薪資存款帳戶移送單";
                    filename = "B_AmountDetail.rpt";
                    break;

                case "AmountDetailAll":
                    strTitle = strTitle + "薪資存款帳戶移送單";
                    filename = "B_AmountDetailAll.rpt";
                    break;

                case "AmountDetail2":
                    strTitle = strTitle + "薪資存款帳戶移送單";
                    filename = "B_AmountDetail-2.rpt";
                    break;

                case "AmountDetail2All":
                    strTitle = strTitle + "薪資存款帳戶移送單";
                    filename = "B_AmountDetail-2All.rpt";
                    break;

                #endregion 2.9 金融機構轉存

                case "unit_list":
                    filename = "B_banklist.rpt";
                    break;

                case "help_list":
                    filename = "B_helplist.rpt";
                    break;

                case "group_list":
                    filename = "B_grouplist.rpt";
                    break;

             
                case "tax_list":
                    filename = "B_taxlist.rpt";
                    break;

                case "tax_detail":
                    strTitle += "員工各項稅額清冊";
                    filename = "B_taxdetail.rpt";
                    break;

                case "savelist":
                    filename = "B_savelist.rpt";
                    break;

                case "SalaryBasicDetail":
                    filename = "B_SalaryBasicDetail.rpt";
                    break;

     

                case "SalaryIncomeTax"://所得稅扣繳憑單
                    filename = "B_TaxDeductableReceipt2.rpt";
                    break;

              

                case "insuranceDetail":
                    filename = "B_InsuranceDetail.rpt";
                    break;


                //2013/03/20 
                case "SearchBonus" :
                    filename = "B_SearchBonus.rpt";
                    break;
                case "SearchBonus_AD":
                    filename = "B_SearchBonus_AD.rpt";
                    break;
            }

            //CrystalDecisions.CrystalReports.Engine.ReportDocument report = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
            report = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
            report.Load(Server.MapPath(filepath + filename));
            report.SetDatabaseLogon(db_UserID, db_Password, db_ServerName, db_DatabaseName);

            //CrystalDecisions.CrystalReports.Engine.ReportDocument report2 = new CrystalDecisions.CrystalReports.Engine.ReportDocument();


            if (Session["RPT_SQL_B"] != null)
            {
                rpt_dt = o_DBFactory.ABC_toTest.Create_Table(Session["RPT_SQL_B"].ToString(), "getvalue");
            }


            switch (fkind)
            {

                #region 2.9 金融機構轉存
                case "bank_list"://2.9金融機構轉存-機關團體戶存款單

                    report.SetDataSource(B_RPT.bank_list.doSearch(Request["PAYAD"], Request["BANK"], Request["TYPE"], Request["BNO"], Request["CASE"], Request["DATE"]));
                    report.ParameterFields["TITLE"].CurrentValues.AddValue("機關團體戶" +B_RPT.bank_list.doTitle(Request["TYPE"]) + "存款單");
                    report.ParameterFields["AGENCYNAME"].CurrentValues.AddValue(B_RPT.bank_list.doAGENCYNAME(Request["PAYAD"], Request["BANK"]));
                    report.ParameterFields["AGENCYID"].CurrentValues.AddValue(B_RPT.bank_list.doAGENCYID(Request["PAYAD"], Request["BANK"]));
                    report.ParameterFields["DATE"].CurrentValues.AddValue(B_RPT.bank_list.GetTransDate(Request["TRANSDATE"]));
                    report.ParameterFields["UNITACC"].CurrentValues.AddValue(HttpUtility.UrlDecode(Request["BUNITNO"]));
                    report.ParameterFields["toUnit"].CurrentValues.AddValue(HttpUtility.UrlDecode(Request["toUnit"]));
                    report.ParameterFields["item"].CurrentValues.AddValue(HttpUtility.UrlDecode(Request["item"]));
                    

                //report.SetDataSource(Session["rpt_dt"]);
                    //report.ParameterFields["TITLE"].CurrentValues.AddValue(strTitle);
                    //report.ParameterFields["AGENCYNAME"].CurrentValues.AddValue(Session["AGENCYNAME"].ToString().Trim());
                    //report.ParameterFields["AGENCYID"].CurrentValues.AddValue(Session["AGENCYID"].ToString().Trim());
                    //report.ParameterFields["DATE"].CurrentValues.AddValue(Session["DATE"].ToString().Trim());
                    //report.ParameterFields["toUnit"].CurrentValues.AddValue(Session["toUnit"].ToString().Trim());
                    //report.ParameterFields["UNITACC"].CurrentValues.AddValue(Session["UNITACC"].ToString().Trim());
                    //report.ParameterFields["item"].CurrentValues.AddValue(Session["item"].ToString().Trim());
                    CrystalReportViewer1.ReportSource = report;
                    break;
                case "AmountDetail":
                    report.SetDataSource(Salary.addPageNumber(B_RPT.AmountDetail.doSearch(Request["PAYAD"], Request["BANK"], Request["TYPE"], Request["BNO"], Request["CASE"], Request["DATE"], Request["SORT"],"1"), 30, "MZ_UNIT"));
                    //report.ParameterFields["TITLE"].CurrentValues.AddValue(strTitle);
                    
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(o_A_KTYPE.CODE_TO_NAME(SalaryPublic.strLoginEXAD, "04") + "薪資存款帳戶移送單");
                    report.ParameterFields["AGENCYNAME"].CurrentValues.AddValue(B_RPT.AmountDetail.doAGENCYNAME(Request["PAYAD"], Request["BANK"]));
                    report.ParameterFields["AGENCYID"].CurrentValues.AddValue(B_RPT.AmountDetail.doAGENCYID(Request["PAYAD"], Request["BANK"]));
                    report.ParameterFields["DATE"].CurrentValues.AddValue(B_RPT.AmountDetail.GetTransDate(Request["TRANSDATE"]));
                    report.ParameterFields["UNITACC"].CurrentValues.AddValue(HttpUtility.UrlDecode(Request["BUNITNO"]));
                    report.ParameterFields["ENTRUSTID"].CurrentValues.AddValue("1554");
                    report.ParameterFields["toUnit"].CurrentValues.AddValue(HttpUtility.UrlDecode(Request["toUnit"]));

                    CrystalReportViewer1.ReportSource = report;
                    break;
                case "AmountDetail2":

                    report.SetDataSource(Salary.addPageNumber(B_RPT.AmountDetail.doSearch(Request["PAYAD"], Request["BANK"], Request["TYPE"], Request["BNO"], Request["CASE"], Request["DATE"], Request["SORT"], "2"), 30, "MZ_UNIT"));
                    //report.ParameterFields["TITLE"].CurrentValues.AddValue(strTitle);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(o_A_KTYPE.CODE_TO_NAME(SalaryPublic.strLoginEXAD, "04") + "薪資存款帳戶移送單");
                    report.ParameterFields["UNITACC"].CurrentValues.AddValue(HttpUtility.UrlDecode(Request["BUNITNO"]));
                    report.ParameterFields["AGENCYNAME"].CurrentValues.AddValue(B_RPT.AmountDetail.doAGENCYNAME(Request["PAYAD"], Request["BANK"]));
                    report.ParameterFields["AGENCYID"].CurrentValues.AddValue(B_RPT.AmountDetail.doAGENCYID(Request["PAYAD"], Request["BANK"]));
                    report.ParameterFields["DATE"].CurrentValues.AddValue(B_RPT.AmountDetail.GetTransDate(Request["TRANSDATE"]));
                    //report.ParameterFields["ENTRUSTID"].CurrentValues.AddValue(Session["ENTRUSTID"].ToString().Trim());
                    report.ParameterFields["ENTRUSTID"].CurrentValues.AddValue("1554");
                    report.ParameterFields["toUnit"].CurrentValues.AddValue(HttpUtility.UrlDecode(Request["toUnit"]));
                    CrystalReportViewer1.ReportSource = report;
                    break;


                case "AmountDetailAll":
                    report.SetDataSource(Salary.addPageNumber(B_RPT.AmountDetail.doSearch(Request["PAYAD"], Request["BANK"], Request["TYPE"], Request["BNO"], Request["CASE"], Request["DATE"], Request["SORT"],"1"), 30, null));
                    //report.ParameterFields["TITLE"].CurrentValues.AddValue(strTitle);

                    report.ParameterFields["TITLE"].CurrentValues.AddValue(o_A_KTYPE.CODE_TO_NAME(SalaryPublic.strLoginEXAD, "04") + "薪資存款帳戶移送單");
                    report.ParameterFields["AGENCYNAME"].CurrentValues.AddValue(B_RPT.AmountDetail.doAGENCYNAME(Request["PAYAD"], Request["BANK"]));
                    report.ParameterFields["AGENCYID"].CurrentValues.AddValue(B_RPT.AmountDetail.doAGENCYID(Request["PAYAD"], Request["BANK"]));
                    report.ParameterFields["DATE"].CurrentValues.AddValue(B_RPT.AmountDetail.GetTransDate(Request["TRANSDATE"]));
                    report.ParameterFields["UNITACC"].CurrentValues.AddValue(HttpUtility.UrlDecode(Request["BUNITNO"]));
                    report.ParameterFields["ENTRUSTID"].CurrentValues.AddValue("1554");
                    report.ParameterFields["toUnit"].CurrentValues.AddValue(HttpUtility.UrlDecode(Request["toUnit"]));

                    CrystalReportViewer1.ReportSource = report;

                   

                   // report.SetDataSource(Session["rpt_dt"]);
                   // report.ParameterFields["TITLE"].CurrentValues.AddValue(strTitle);
                   // report.ParameterFields["AGENCYNAME"].CurrentValues.AddValue(Session["AGENCYNAME"].ToString().Trim());
                   // report.ParameterFields["AGENCYID"].CurrentValues.AddValue(Session["AGENCYID"].ToString().Trim());
                   // report.ParameterFields["UNITACC"].CurrentValues.AddValue(Session["UNITACC"].ToString().Trim());
                   // report.ParameterFields["DATE"].CurrentValues.AddValue(Session["DATE"].ToString().Trim());
                   // //report.ParameterFields["ENTRUSTID"].CurrentValues.AddValue(Session["ENTRUSTID"].ToString().Trim());
                   //report.ParameterFields["ENTRUSTID"].CurrentValues.AddValue("1554");
                   // report.ParameterFields["toUnit"].CurrentValues.AddValue(Session["toUnit"].ToString().Trim());
                   // CrystalReportViewer1.ReportSource = report;
                    break;

                //case "AmountDetail2":
                case "AmountDetail2All":

                    report.SetDataSource(Salary.addPageNumber(B_RPT.AmountDetail.doSearch(Request["PAYAD"], Request["BANK"], Request["TYPE"], Request["BNO"], Request["CASE"], Request["DATE"], Request["SORT"], "1"), 30, null));
                    //report.ParameterFields["TITLE"].CurrentValues.AddValue(strTitle);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(o_A_KTYPE.CODE_TO_NAME(SalaryPublic.strLoginEXAD, "04") + "薪資存款帳戶移送單");
                    report.ParameterFields["AGENCYNAME"].CurrentValues.AddValue(B_RPT.AmountDetail.doAGENCYNAME(Request["PAYAD"], Request["BANK"]));
                    report.ParameterFields["AGENCYID"].CurrentValues.AddValue(B_RPT.AmountDetail.doAGENCYID(Request["PAYAD"], Request["BANK"]));
                    report.ParameterFields["UNITACC"].CurrentValues.AddValue(HttpUtility.UrlDecode(Request["BUNITNO"]));
                    report.ParameterFields["DATE"].CurrentValues.AddValue(B_RPT.AmountDetail.GetTransDate(Request["TRANSDATE"]));
                    //report.ParameterFields["ENTRUSTID"].CurrentValues.AddValue(Session["ENTRUSTID"].ToString().Trim());
                    report.ParameterFields["ENTRUSTID"].CurrentValues.AddValue("1554");
                    report.ParameterFields["toUnit"].CurrentValues.AddValue(HttpUtility.UrlDecode(Request["toUnit"]));
                    CrystalReportViewer1.ReportSource = report;

                    //report.SetDataSource(Session["rpt_dt"]);
                    //report.ParameterFields["TITLE"].CurrentValues.AddValue(strTitle);
                    //report.ParameterFields["AGENCYNAME"].CurrentValues.AddValue(Session["AGENCYNAME"].ToString().Trim());
                    //report.ParameterFields["AGENCYID"].CurrentValues.AddValue(Session["AGENCYID"].ToString().Trim());
                    //report.ParameterFields["UNITACC"].CurrentValues.AddValue(Session["UNITACC"].ToString().Trim());
                    //report.ParameterFields["DATE"].CurrentValues.AddValue(Session["DATE"].ToString().Trim());
                    ////report.ParameterFields["ENTRUSTID"].CurrentValues.AddValue(Session["ENTRUSTID"].ToString().Trim());
                    //report.ParameterFields["ENTRUSTID"].CurrentValues.AddValue("1554");
                    //report.ParameterFields["toUnit"].CurrentValues.AddValue(Session["toUnit"].ToString().Trim());
                    //CrystalReportViewer1.ReportSource = report;
                    break;
                #endregion 2.9 金融機構轉存

                case "unit_list":
                    
                    report.SetDataSource(Session["rpt_dt"]);
                    report.ParameterFields["T_DATE"].CurrentValues.AddValue(Session["T_DATE"].ToString().Trim());
                    report.ParameterFields["ADNO"].CurrentValues.AddValue(Session["ADNO"].ToString().Trim());
                    report.ParameterFields["ADNAME"].CurrentValues.AddValue(Session["ADNAME"].ToString().Trim());
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(Session["TITLE"].ToString().Trim());
                    CrystalReportViewer1.ReportSource = report;
                    break;
                case "help_list":
                     
                    report.SetDataSource(Session["rpt_dt"]);
                    report.ParameterFields["ADNO"].CurrentValues.AddValue(Session["ADNO"].ToString().Trim());
                    report.ParameterFields["ADNAME"].CurrentValues.AddValue(Session["ADNAME"].ToString().Trim());
                    report.ParameterFields["ENO"].CurrentValues.AddValue(Session["ENO"].ToString().Trim());
                    report.ParameterFields["FNO"].CurrentValues.AddValue(Session["FNO"].ToString().Trim());
                    CrystalReportViewer1.ReportSource = report;
                    break;
                case "group_list":
                     
                    report.SetDataSource(Session["rpt_dt"]);
                    report.ParameterFields["T_DATE"].CurrentValues.AddValue(Session["T_DATE"].ToString().Trim());
                    report.ParameterFields["ADNO"].CurrentValues.AddValue(Session["ADNO"].ToString().Trim());
                    report.ParameterFields["ADNAME"].CurrentValues.AddValue(Session["ADNAME"].ToString().Trim());
                    report.ParameterFields["ENO"].CurrentValues.AddValue(Session["ENO"].ToString().Trim());
                    report.ParameterFields["FNO"].CurrentValues.AddValue(Session["FNO"].ToString().Trim());
                    CrystalReportViewer1.ReportSource = report;
                    break;
                case "SalaryBasicDetail":
                     
                    DataTable dt = (DataTable)Session["rpt_dt"];
                    report.SetDataSource(dt);
                    CrystalReportViewer1.ReportSource = report;
                    break;
              
                case "SalaryIncomeTax":
                     
                    report.SetDataSource((DataTable)Session["rpt_dt"]);
                    report.ParameterFields["taxinvoice"].CurrentValues.AddValue(Session["taxinvoice"].ToString().Trim());
                    report.ParameterFields["taxunit"].CurrentValues.AddValue(Session["taxunit"].ToString().Trim());
                    report.ParameterFields["taxname"].CurrentValues.AddValue(Session["taxname"].ToString().Trim());
                    report.ParameterFields["taxaddr"].CurrentValues.AddValue(Session["taxaddr"].ToString().Trim());
                    report.ParameterFields["taxpers"].CurrentValues.AddValue(Session["taxpers"].ToString().Trim());
                    CrystalReportViewer1.ReportSource = report;
                    break;

               
                case "insuranceDetail":
                     
                    //report.SetDataSource((DataTable)Session["rpt_dt"]);
                    //report.ParameterFields["PAY_AD"].CurrentValues.AddValue(Session["PAY_AD"].ToString().Trim());
                    //report.ParameterFields["YEAR"].CurrentValues.AddValue(Session["YEAR"].ToString().Trim());
                    //report.ParameterFields["MONTH"].CurrentValues.AddValue(Session["MONTH"].ToString().Trim());
                    CrystalReportViewer1.ReportSource = report;
                    break;

               


                #region 2.2 每月薪資作業

                case "monthpay_list"://2.2.3每月薪資應發總表

                    report.SetDataSource(B_RPT.monthpay_list.doSearch(Request["YEAR"], Request["MONTH"], Request["PAY_AD"]));                    
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(o_A_KTYPE.CODE_TO_NAME(Request["PAY_AD"], "04") + Request["YEAR"] + "年" + Request["MONTH"].PadLeft(2, '0') + "月" + "應發薪資總表");
                    CrystalReportViewer1.ReportSource = report;
                    break;

                case "monthpay_list2"://2.2.3每月薪資應發總表New

                    report.SetDataSource(B_RPT.monthpay_list.doSearch2(Request["YEAR"], Request["MONTH"], Request["PAY_AD"]));
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(o_A_KTYPE.CODE_TO_NAME(Request["PAY_AD"], "04") + Request["YEAR"] + "年" + Request["MONTH"].PadLeft(2, '0') + "月" + "應發薪資總表");
                    CrystalReportViewer1.ReportSource = report;
                    break;
                
                case "monthpaytakeoff_list"://2.2.5每月薪資應扣總表                       
                    report.SetDataSource(B_RPT.monthpaytakeoff_list.doSearch(Request["YEAR"], Request["MONTH"], Request["PAY_AD"]));   
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(o_A_KTYPE.CODE_TO_NAME(Request["PAY_AD"], "04") + Request["YEAR"] + "年" + Request["MONTH"].PadLeft(2, '0') + "月" + "實發薪資總表");                  
                    CrystalReportViewer1.ReportSource = report;
                    break;

                case "monthpaytakeoff_list2"://2.2.5每月薪資應扣總表New
                    report.SetDataSource(B_RPT.monthpaytakeoff_list.doSearch2(Request["YEAR"], Request["MONTH"], Request["PAY_AD"]));
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(o_A_KTYPE.CODE_TO_NAME(Request["PAY_AD"], "04") + Request["YEAR"] + "年" + Request["MONTH"].PadLeft(2, '0') + "月" + "實發薪資總表");
                    CrystalReportViewer1.ReportSource = report;
                    break;
               
                case "monthpay_detail": //2.2.4 每月薪資應發個人明細表
                    report.SetDataSource(B_RPT.monthpay_detail.doSearch(Request["YEAR"], Request["MONTH"], Request["PAY_AD"],Request["UNIT"]));
                    report.ParameterFields["AD"].CurrentValues.AddValue(o_A_KTYPE.CODE_TO_NAME(Request["PAY_AD"], "04"));
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(" " + Request["YEAR"] + "年" + Request["MONTH"].PadLeft(2, '0') + "月" + "應發薪資明細表");
                    CrystalReportViewer1.ReportSource = report;
                    break;


                case "monthpaytakeoff_detail": //2.2.6 每月薪資應扣個人明細表
                    report.SetDataSource(B_RPT.monthpaytakeoff_detail.doSearch(Request["YEAR"], Request["MONTH"], Request["PAY_AD"], Request["UNIT"]));
                    report.ParameterFields["AD"].CurrentValues.AddValue(o_A_KTYPE.CODE_TO_NAME(Request["PAY_AD"], "04"));
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(" " + Request["YEAR"] + "年" + Request["MONTH"].PadLeft(2, '0') + "月" + "實發薪資明細表");
                    CrystalReportViewer1.ReportSource = report;
                    break;

                #endregion 2.2 每月薪資作業


                #region 2.3 補發薪資作業

                case "repairpay_list": //2.3.2 補發薪資應發總表 
                    report.SetDataSource(B_RPT.repairpay_list.doSearch(Request["YEAR"], Request["MONTH"], Request["PAY_AD"], Request["BATCH_NUMBER"]));                    
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(o_A_KTYPE.CODE_TO_NAME(Request["PAY_AD"], "04") + Request["YEAR"] + "年" + Request["MONTH"].PadLeft(2, '0') + "月第" + Request["BATCH_NUMBER"] + "案補發薪資總表");
                    CrystalReportViewer1.ReportSource = report;
                    break;

                case "repairtakeoff_list": //2.3.4 補發薪資總表(應扣)
                    report.SetDataSource(B_RPT.repairtakeoff_list.doSearch(Request["YEAR"], Request["MONTH"], Request["PAY_AD"], Request["BATCH_NUMBER"]));                  
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(o_A_KTYPE.CODE_TO_NAME(Request["PAY_AD"], "04") + Request["YEAR"] + "年" + Request["MONTH"].PadLeft(2, '0') + "月第" + Request["BATCH_NUMBER"] + "案補發薪資總表(應扣)");
                    CrystalReportViewer1.ReportSource = report;
                    break;

                //應政府新預算編制分開職稱列印 20180410 by sky
                case "repairpay_list2"://2.3.2 補發薪資應發總表New
                    report.SetDataSource(B_RPT.repairpay_list.doSearchClassify(Request["YEAR"], Request["MONTH"], Request["PAY_AD"], Request["BATCH_NUMBER"]));
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(o_A_KTYPE.CODE_TO_NAME(Request["PAY_AD"], "04") + Request["YEAR"] + "年" + Request["MONTH"].PadLeft(2, '0') + "月第" + Request["BATCH_NUMBER"] + "案補發薪資總表");
                    CrystalReportViewer1.ReportSource = report;
                    break;
                case "repairtakeoff_list2":////2.3.4 補發薪資總表(應扣)New
                    report.SetDataSource(B_RPT.repairtakeoff_list.doSearchClassify(Request["YEAR"], Request["MONTH"], Request["PAY_AD"], Request["BATCH_NUMBER"]));
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(o_A_KTYPE.CODE_TO_NAME(Request["PAY_AD"], "04") + Request["YEAR"] + "年" + Request["MONTH"].PadLeft(2, '0') + "月第" + Request["BATCH_NUMBER"] + "案補發薪資總表(應扣)");
                    CrystalReportViewer1.ReportSource = report;
                    break;


                case "repairpay_detail":// 2.3.3 補發薪資明細表
                    report.SetDataSource(B_RPT.repairpay_detail.doSearch(Request["YEAR"], Request["MONTH"], Request["PAY_AD"],Request["UNIT"], Request["BATCH_NUMBER"]));
                    report.ParameterFields["AD"].CurrentValues.AddValue(o_A_KTYPE.CODE_TO_NAME(Request["PAY_AD"], "04"));
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(" " + Request["YEAR"] + "年" + Request["MONTH"].PadLeft(2, '0') + "月第" + Request["BATCH_NUMBER"] + "案補發薪資明細表");
                    CrystalReportViewer1.ReportSource = report;
                    break;

                case "repairpaytakeoff_detail"://2.3.5 補發薪資應扣明細表 
                    report.SetDataSource(B_RPT.repairpaytakeoff_detail.doSearch(Request["YEAR"], Request["MONTH"], Request["PAY_AD"], Request["UNIT"], Request["BATCH_NUMBER"]));
                    report.ParameterFields["AD"].CurrentValues.AddValue(o_A_KTYPE.CODE_TO_NAME(Request["PAY_AD"], "04"));
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(" " + Request["YEAR"] + "年" + Request["MONTH"].PadLeft(2, '0') + "月第" + Request["BATCH_NUMBER"] + "案補發薪資明細表(應扣)");
                    CrystalReportViewer1.ReportSource = report;
                    break;

                #endregion 2.3 補發薪資作業

                case "tax_detail":
                     
                    report.SetDataSource(Session["rpt_dt"]);
                    report.ParameterFields["AD"].CurrentValues.AddValue(strAD);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(strTitle);
                    CrystalReportViewer1.ReportSource = report;
                    break;


                #region 每月薪資差異表

                case "monthpay_list_different"://每月薪資應發差異總表
                    //2014/6/16 增加查無資料防呆 by 青
                    rpt_dt = B_RPT.monthpay_list_different.doSearch(Request["DATE1"], Request["DATE2"], Request["AD"]);

                    if (rpt_dt.Rows.Count == 0)
                    {
                        lb_tip.Text = "無相關資料";

                    }

                    if (!string.IsNullOrEmpty(lb_tip.Text))
                    {
                        lb_tip.Visible = true;
                        return;

                    }
                    report.SetDataSource(Salary.addPageNumber(rpt_dt, 22, "PAY_AD"));
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(o_A_KTYPE.CODE_TO_NAME(Request["AD"], "04") + " " + Request["DATE1"].Substring(0, 3) + "年" + Request["DATE1"].Substring(3, 2).PadLeft(2, '0') + "月" +
                        "與" + Request["DATE2"].Substring(0, 3) + "年" + Request["DATE2"].Substring(3, 2).PadLeft(2, '0') + "月" + strTitle);

                    CrystalReportViewer1.ReportSource = report;
                    break;

                case "monthpay_detail_different":
                    //2014/6/16 增加查無資料防呆 by 青
                    rpt_dt = B_RPT.monthpay_detail_different.doSearch(Request["DATE1"], Request["DATE2"], Request["AD"]);

                    if (rpt_dt.Rows.Count == 0)
                    {
                        lb_tip.Text = "無相關資料";

                    }

                    if (!string.IsNullOrEmpty(lb_tip.Text))
                    {
                        lb_tip.Visible = true;
                        return;

                    }

                    report.SetDataSource(Salary.addPageNumber(rpt_dt, 20, "PAY_UNIT"));
                     report.ParameterFields["TITLE"].CurrentValues.AddValue(o_A_KTYPE.CODE_TO_NAME(Request["AD"], "04") + " " + Request["DATE1"].Substring(0, 3) + "年" + Request["DATE1"].Substring(3, 2).PadLeft(2, '0') + "月" +
                      "與" + Request["DATE2"].Substring(0, 3) + "年" + Request["DATE2"].Substring(3, 2).PadLeft(2, '0') + "月" + strTitle);


                    report.ParameterFields["DATE1"].CurrentValues.AddValue(Request["DATE1"]);
                    report.ParameterFields["DATE2"].CurrentValues.AddValue(Request["DATE2"]);

                    CrystalReportViewer1.ReportSource = report;
                    break;



                case "monthpaytakeoff_list_different"://每月薪資應扣差異總表
                    //2014/6/16 增加查無資料防呆 by 青
                    rpt_dt = B_RPT.monthpaytakeoff_list_different.doSearch(Request["DATE1"], Request["DATE2"], Request["AD"], Request["TYPE"]);

                    if (rpt_dt.Rows.Count == 0)
                    {
                        lb_tip.Text = "無相關資料";

                    }

                    if (!string.IsNullOrEmpty(lb_tip.Text))
                    {
                        lb_tip.Visible = true;
                        return;

                    }
                    report.SetDataSource(Salary.addPageNumber(rpt_dt, 22, "PAY_AD"));
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(o_A_KTYPE.CODE_TO_NAME(Request["AD"], "04") + " " + Request["DATE1"].Substring(0, 3) + "年" + Request["DATE1"].Substring(3, 2).PadLeft(2, '0') + "月" +
                        "與" + Request["DATE2"].Substring(0, 3) + "年" + Request["DATE2"].Substring(3, 2).PadLeft(2, '0') + "月" + strTitle);

                    CrystalReportViewer1.ReportSource = report;
                    break;
                case "monthpaytakeoff_detail_different":
                    //2014/6/16 增加查無資料防呆 by 青
                    rpt_dt = B_RPT.monthpaytakeoff_detail_different.doSearch(Request["DATE1"], Request["DATE2"], Request["AD"], Request["TYPE"]);

                    if (rpt_dt.Rows.Count == 0)
                    {
                        lb_tip.Text = "無相關資料";

                    }

                    if (!string.IsNullOrEmpty(lb_tip.Text))
                    {
                        lb_tip.Visible = true;
                        return;

                    }


                    report.SetDataSource(Salary.addPageNumber(rpt_dt, 20, "PAY_UNIT"));
                  
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(o_A_KTYPE.CODE_TO_NAME(Request["AD"], "04") + " " + Request["DATE1"].Substring(0, 3) + "年" + Request["DATE1"].Substring(3, 2).PadLeft(2, '0') + "月" +
                      "與" + Request["DATE2"].Substring(0, 3) + "年" + Request["DATE2"].Substring(3, 2).PadLeft(2, '0') + "月" + strTitle);

                    report.ParameterFields["DATE1"].CurrentValues.AddValue(Request["DATE1"]);
                    report.ParameterFields["DATE2"].CurrentValues.AddValue(Request["DATE2"]);
                    CrystalReportViewer1.ReportSource = report;



                    break;

                #endregion 每月薪資差異表


                case "yearpay_list":
                case "effect_list":
                     
                    report.SetDataSource(Session["rpt_dt"]);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(strTitle);
                    report.ParameterFields["TITLE1"].CurrentValues.AddValue(Session["TITLE1"].ToString().Trim());
                    CrystalReportViewer1.ReportSource = report;
                    break;

                case "yearpay_detail":
                case "effect_detail":
                     
                    report.SetDataSource(Session["rpt_dt"]);
                    report.ParameterFields["AD"].CurrentValues.AddValue(strAD);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(strTitle);
                    report.ParameterFields["TITLE1"].CurrentValues.AddValue(Session["TITLE1"].ToString().Trim());
                    CrystalReportViewer1.ReportSource = report;
                    break;

                case "savelist":
                     
                    report.SetDataSource((DataTable)Session["rpt_dt"]);
                    report.ParameterFields["TOTALPEOPLE"].CurrentValues.AddValue(Session["TOTALPEOPLE"].ToString());
                    report.ParameterFields["TOTALPAY"].CurrentValues.AddValue(Session["TOTALPAY"].ToString());
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(strTitle);
                    CrystalReportViewer1.ReportSource = report;
                    break;
                //轉帳回饋擋檢核報表 2014/06/18 BY青
                case "salary_back_different":
                    rpt_dt = B_RPT.pay_compare.doSearch(Request["AD"], Request["TYPE"], Request["BANKID"], Request["CASEID"],Request["DATADATE"], Request["IN_ACCOUNT_DATE"]);

                    if (rpt_dt.Rows.Count == 0)
                    {
                        lb_tip.Text = "無差異資料";

                    }

                    if (!string.IsNullOrEmpty(lb_tip.Text))
                    {
                        lb_tip.Visible = true;
                        return;

                    }
                    report.SetDataSource(Salary.addPageNumber(rpt_dt, 22, "PAY_AD"));
                    report.ParameterFields["AD"].CurrentValues.AddValue(o_A_KTYPE.CODE_TO_NAME(Request["AD"], "04"));
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(strTitle);
   
                    CrystalReportViewer1.ReportSource = report;


                    break;

                    //2013/03/20 by 立廷
              
                #region 全部和個人共用-薪資系統
                case "Sole": //8.3單一發放明細表
                    rpt_dt.Columns.Add("PAGE", typeof(int));
                    int count = 1;
                    for (int i = 0; i < rpt_dt.Rows.Count; i++)//為了統計每頁小計而群組
                    {
                        if (i % 11 == 0)
                        {
                            count++;
                        }
                        rpt_dt.Rows[i]["PAGE"] = count;
                    }

                    report.SetDataSource(rpt_dt);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(strTitle);
                    CrystalReportViewer1.ReportSource = report;
                    break;
                case "PersonalEffectDetail"://8.5考績獎金明細表

                    //有兩個頁面使用這張報表.SQL句稍有不同
                    //B_SearchYearSalary.aspx 個人.如未關帳是查不到的 
                    //  B_SearchYearSalary3.aspx  分局薪資作業時用
                 
                    report.SetDataSource(rpt_dt);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(strTitle);
                    CrystalReportViewer1.ReportSource = report;
                    break;
                case "SearchYearSalary"://8.6薪資年度總表

                    report.SetDataSource(rpt_dt);
                    report.ParameterFields["PAY_AD"].CurrentValues.AddValue(Session["payad"].ToString().Trim());
                    CrystalReportViewer1.ReportSource = report;
                    break;

                case "79"://8.8個人全年薪資明細表

                    Police police = new Police(Session["RPT_A_IDNO"] == null ? "" : Session["RPT_A_IDNO"].ToString());
                     // 查無此員警
                     if (police.id == null)
                     {
                         lb_tip.Text ="該身份證字號查無資料";
                         
                     }                    

                     if (!string.IsNullOrEmpty(lb_tip.Text))
                     {
                         lb_tip.Visible = true;
                         return;

                     }


                    total = 0;




                    rpt_dt = B_RPT.B_79.doSearch(Request["YEAR"], Session["RPT_A_IDNO"] == null ? "" : Session["RPT_A_IDNO"].ToString());
                    foreach (DataRow row in rpt_dt.Rows)
                    {
                        total += int.Parse(row["AMOUNT"].ToString());
                    }

                   

                    report.SetDataSource(rpt_dt);                   
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(police.payadName);
                    report.ParameterFields["SUBTITLE"].CurrentValues.AddValue(Request["YEAR"] + "(全)年薪資明細表");
                    report.ParameterFields["POLNO"].CurrentValues.AddValue(police.polno);
                    report.ParameterFields["OCCC"].CurrentValues.AddValue(police.occcName);
                    report.ParameterFields["NAME"].CurrentValues.AddValue(police.name);                 
                    report.ParameterFields["TOTAL"].CurrentValues.AddValue(total);
                    CrystalReportViewer1.ReportSource = report;
                    break;

                case "insuranceStat"://??
                    //string strPAY_AD= Request["AD"];
                    //string strDATE = Request["DATE"];
                    //B.B_InsuranceStatistics insuranceStat = new B.B_InsuranceStatistics();
                    //rpt_dt = insuranceStat.doSearch(strPAY_AD, strDATE);


                    report.SetDataSource((DataTable)Session["rpt_dt"]);
                    report.ParameterFields["PAY_AD"].CurrentValues.AddValue(Session["PAY_AD"].ToString().Trim() );
                    report.ParameterFields["YEAR"].CurrentValues.AddValue(Session["YEAR"].ToString().Trim());
                    report.ParameterFields["MONTH"].CurrentValues.AddValue(Session["MONTH"].ToString().Trim());
                    CrystalReportViewer1.ReportSource = report;
                    break;

                case "insuranceStat_2"://8.7保費統計表
                     string strPAY_AD = Request["AD"];
                     string strDATE = Request["DATE"];                  
                       
                        B.B_InsuranceStatistics_2 insuranceStat = new B.B_InsuranceStatistics_2();
                        rpt_dt = insuranceStat.doSearch(strPAY_AD, strDATE);                    
                    
                    report.SetDataSource(rpt_dt);

                    report.ParameterFields["PAY_AD"].CurrentValues.AddValue(o_A_KTYPE.CODE_TO_NAME(strPAY_AD,"04").Trim());
                    report.ParameterFields["YEAR"].CurrentValues.AddValue(strDATE.Substring(0, 3).Trim());
                    report.ParameterFields["MONTH"].CurrentValues.AddValue(strDATE.Substring(3, 2).Trim());
                    CrystalReportViewer1.ReportSource = report;
                    break;


                case "SearchBonus":
                    report.SetDataSource((DataTable)Session["rpt_dt"]);
                    report.ParameterFields["AD"].CurrentValues.AddValue(strAD);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(strTitle);
                    CrystalReportViewer1.ReportSource = report;
                    break;

                case "SearchBonus_AD":
                    report.SetDataSource((DataTable)Session["rpt_dt"]);
                    report.ParameterFields["AD"].CurrentValues.AddValue(strAD);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(strTitle);
                    CrystalReportViewer1.ReportSource = report;
                    break;

                case "PersonalMonthSalary": //8.1每月薪資明細表                   
                case "RepairPay"://8.2薪資補發明細表                   
                case "SalaryYear-Endbonus"://8.4年終獎金明細表                   
                    report.SetDataSource(rpt_dt);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(strTitle);
                    CrystalReportViewer1.ReportSource = report;
                    break;
                #endregion 2薪資系統
                
                default:          
                    report.SetDataSource(Session["rpt_dt"]);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(strTitle);
                    CrystalReportViewer1.ReportSource = report;
                    break;
            }
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            report.Close();
            report.Dispose();
        }
    }
}
