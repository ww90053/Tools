using System;
using System.Data;
using System.Web;
using TPPDDB.Service;
using TPPDDB.Helpers;
using System.Data.SqlClient;
using System.Collections.Generic;
using TPPDDB.Models._3_ForLeave;

namespace TPPDDB._3_forleave
{
    public partial class C_rpt : System.Web.UI.Page
    {
        DataTable rpt_dt = new DataTable();       

        CrystalDecisions.CrystalReports.Engine.ReportDocument report;
        protected void Page_Unload(object sender, EventArgs e)
        {
            report.Close();
            report.Dispose();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            string fkind = "";
            string filename = "";
            //string filename2 = "";
            string filepath = "report/";

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

            #region 報表查詢共用參數
            string MZ_ID = string.Empty;
            string DATE = string.Empty;
            string MZ_AD = string.Empty;
            string MZ_UNIT = string.Empty;
            string TPM_FION = Request.QueryString["TPM_FION"].ToStringNullSafe();
            #endregion

            //設定報表檔案
            switch (fkind)
            {
                case "ondutyhour":
                    filename = "C_ondutyour.rpt";
                    break;


                #region 4.3  差勤報表    包函請假輸入 1.2.10.11.13

                case "offduty"://4.3.1印請假單
                    filename = "C_offduty.rpt";
                    break;
                case "businessTrip_Sub"://4.3.2旅費報告表
                    filename = "C_business_Sub.rpt";
                    break;
                case "businessTrip"://4.3.2出差請示單
                    filename = "C_business.rpt";
                    break;
                case "shift"://4.3.3勤惰卡
                    filename = "C_shift.rpt";
                    break;
                case "shift1"://4.3.3勤惰卡
                    filename = "C_shift1.rpt";
                    break;
                case "holiday"://4.3.4個人差假明細表
                    filename = "C_holiday.rpt";
                    break;
                case "personalduty"://4.3.5各別假別統計表
                    filename = "C_personalduty.rpt";
                    break;
                case "ToBeOffDuty"://4.3.6 員警應休假查核表
                    filename = "C_ToBeOffDuty.rpt";
                    break;
                case "codecount":////4.3.7 員警差假統計表
                    filename = "C_codecount.rpt";
                    break;

                case "dutydetail": //4.3.8員警差假明細表
                    filename = "C_dutydetail.rpt";
                    break;
                case "chinaabroad"://4.3.9赴大陸假單
                    filename = "C_chinaabroad.rpt";
                    break;
                case "chinaabroad1"://4.3.9申請人赴大陸地區注意事項
                    filename = "C_chinaabroad_2nd.rpt";
                    break;
                case "abroad"://4.3.10 出國請假單 4.1也有
                    filename = "C_abroad.rpt";
                    break;
                case "abroad_CN"://4.3.10 赴大陸請假單 4.1也有
                    filename = "C_abroad_CN.rpt";
                    break;
                case "ondutyabroad"://4.3.11 職員非請假出國報備表 4.1也有
                    filename = "C_ondutyabroad.rpt";
                    break;
                case "ondutyabroad_CN"://4.3.11 職員非請假赴大陸報備表 4.1也有
                    filename = "C_ondutyabroad_CN.rpt";
                    break;
                case "offCFM"://4.3/12員警請假核定通知書
                    filename = "C_offCFM.rpt";
                    break;
                case "askleave"://4.3.13請假報告單 4.1也有
                    filename = "C_askleave.rpt";
                    break;

                case "busitrip50"://4.3.14出差前50名報表
                    filename = "C_busitrip50.rpt";
                    break;

                case "noplaylist"://4.3.15因公未休假改發加班費印領清冊
                    filename = "C_noplaylist.rpt";
                    break;
                case "paided"://4.3.16已請領強制休假補助費明細表
                    filename = "C_paided.rpt";
                    break;

                case "nopaid"://4.3.17未請領強制休假補助費明細表
                    filename = "C_nopaid.rpt";
                    break;
                case "diffdutydetail"://4.2.18刷卡(勤惰)明細表 異常
                    filename = "C_diffdutydetail.rpt";
                    break;
                case "diffdutydetail2"://4.2.18刷卡(勤惰)明細表 全部和加班
                    filename = "C_diffdutydetail2.rpt";
                    break;
                case "over_detail"://4.2.18刷卡(勤惰)明細表 差勤
                    filename = "C_over_detail.rpt";
                    break;
                case "day_detail"://4.2.18 刷卡(勤惰)明細表 值日
                    filename="C_day_detail.rpt";
                    break;
                case "carddetail":////4.3.19勤惰(刷卡)紀錄明細表
                    filename = "C_carddetail.rpt";
                    break;
                case "finger_out": //4.3.20外勤時間表
                    filename = "C_fingerdetail.rpt";
                    break;

                #endregion 4.3  差勤報表    包函請假輸入 1.2.10.11.13

                case "dutymonthhour":
                    filename = "C_dutymonthhour.rpt";
                    break;

                #region 5.加班管理作業 報表

                case "OvertimeInsideAsk"://5.1 加班請示單
                    filename = "C_OvertimeInsideAsk.rpt";
                    break;

                case "OvertimeInsideDetail"://5.2加班明細表
                    filename = "C_OvertimeInsideDetail.rpt";
                    break;
                case "OvertimeInsideTotal"://5.3員工加班明細表
                    filename = "C_OvertimeInsideTotal.rpt";
                    break;

                //TODO SKY 正式上線時替換
                case "OvertimeInsideAsk_New"://5.1 加班請示單
                    filename = "C_OvertimeInsideAsk.rpt";
                    break;
                case "OvertimeInsideDetail_New"://5.2加班明細表
                    filename = "C_OvertimeInsideDetail_New.rpt";
                    break;
                case "OvertimeInsideTotal_New"://5.3員工加班明細表
                    filename = "C_OvertimeInsideTotal_New.rpt";
                    break;
                case "OverTimeDutyDetail": //5.x新加班勤惰明細表
                    filename = "C_OverTimeDutyDetail.rpt";
                    break;
                #endregion

                #region 6.3 超勤報表

                case "Overtimepay_detail"://6.3.1超勤時數統計表_ㄧ般人員
                    filename = "C_Overtimepay_detail.rpt";
                    break;
                case "C_Overtimepay_detail_Criminal"://6.3.1超勤時數統計表_刑事、少年、婦幼
                    filename = "C_Overtimepay_detail_Criminal.rpt";
                    break;
                case "overtimerequestlistrpt"://6.3.2超勤印領清冊
                    filename = "C_overtimerequestlistrpt.rpt";
                    break;
                case "OvertimeOutSide_Unit"://6.3.3超勤支領情形分析表(小時) //如果不選單位 與6.3.4共用
                    filename = "C_OvertimeOutSide_Unit.rpt";
                    break;
                case "OvertimeOutSide_Hour"://6.3.3超勤支領情形分析表(小時) //有選單位
                    filename = "C_OvertimeOutSide_Hour.rpt";
                    break;
                case "OvertimeOutSide_Cost"://6.3.4超勤支領情形分析表(金額)
                    filename = "C_OvertimeOutSide_Cost.rpt";
                    break;
                #endregion 6.3 超勤報表

                case "dutyday6":
                    filename = "C_dutyday6.rpt";
                    break;
                case "dutyday7":
                    filename = "C_dutyday7.rpt";
                    break;
                case "dutyday8":
                    filename = "C_dutyday8.rpt";
                    break;
                case "dutyday9":
                    filename = "C_dutyday9.rpt";
                    break;
                case "monthturns":
                    filename = "C_monthturns.rpt";
                    break;


               
                case "OvertimePayMoneyList":
                    filename = "C_OvertimePayMoneyList.rpt";
                    break;
                case "noplaylistTotal":
                    filename = "C_noplaylistTotal.rpt";
                    break;


                #region  輪值報表

                case "fingerdetail": //11.9.4指紋明細表
                    filename = "C_fingerdetail.rpt";
                    break;


                #endregion

                case "ONDUTY_LIST":


                    filename = "C_ONDUTY_LIST.rpt";

                    break;
                /// Joy 新增 輪值表(新版)
                case "ONDUTY_LIST_NEW":

                    filename = "C_ONDUTY_LIST_NEW.rpt";
                    break;

                case "ONDUTY_PAY_LIST":
                    filename = "C_ONDUTY_PAY_LIST.rpt";
                    break;

                // Joy新增 值日報表更新版
                case "ONDUTY_PAY_LIST_NEW":


                    filename = "C_ONDUTY_PAY_LIST_NEW.rpt";

                    break;

            }

            report = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
            report.Load(Server.MapPath(filepath + filename));
            report.SetDatabaseLogon(db_UserID, db_Password, db_ServerName, db_DatabaseName);


            if (Session["RPT_SQL_C"] != null)
            {
                rpt_dt = o_DBFactory.ABC_toTest.Create_Table(Session["RPT_SQL_C"].ToString(), "getvalue");
            }

            //產生報表資料
            switch (fkind)
            {
                case "ondutyhour":
                    
                    report.SetDataSource(Session["rpt_dt"] as DataTable);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(Session["TITLE"].ToString().Trim());
                    CrystalReportViewer1.ReportSource = report;
                    break;

                    

                #region 4.3  差勤報表    包函請假輸入 1.2.10.11.13

                #region 4.3.1印請假單-每次一張
                case "offduty"://印請假單-每次一張
                    
                    rpt_dt=RPT_C.C_offduty.doSearch( Session["RPT_C_IDNO"]==null?"":Session["RPT_C_IDNO"].ToString() ,HttpUtility.UrlDecode(Request["MZ_NAME"]) ,Request["DATE"],Request["TIME"]);

                    
                  
                    if (rpt_dt.Rows.Count == 0)
                    {
                        lb_tip.Text = " 無此日期之相關資料";
                    
                    }

                    if (!string.IsNullOrEmpty(lb_tip.Text))
                    {
                        lb_tip.Visible = true;
                        return;
                    
                    }
                    
                    string dlcode_code = o_DBFactory.ABC_toTest.vExecSQL(string.Format(@"SELECT DISTINCT LEAVE_SN FROM C_LEAVE_HISTORY WHERE DLTB01_SN={0}", rpt_dt.Rows[0]["MZ_DLTB01_SN"].ToString()));

                    CSreportSign Sign = new CSreportSign(dlcode_code, rpt_dt.Rows[0]["MZ_DLTB01_SN"].ToString());

                    
                    report.SetDataSource(rpt_dt);
                    
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(RPT_C.C_offduty.TITLE(rpt_dt));
                    
                    report.ParameterFields["DOWN"].CurrentValues.AddValue(RPT_C.C_offduty.DOWN(rpt_dt,Request["date"] ));
                    
                    report.ParameterFields["PS"].CurrentValues.AddValue(RPT_C.C_offduty.PS(rpt_dt));
                
                    report.ParameterFields["SEND_TIME"].CurrentValues.AddValue(Sign.getSendTime());
                    report.ParameterFields["RNAME_SIGN"].CurrentValues.AddValue(Sign.getRnameSign());
                    report.ParameterFields["MANAGER_SIGN"].CurrentValues.AddValue(Sign.getMangerSign());
                    report.ParameterFields["Personnel_SIGN"].CurrentValues.AddValue(Sign.getPersonnelSign());
                    report.ParameterFields["DECISION_SIGN"].CurrentValues.AddValue(Sign.getDecisionSign());
                    report.ParameterFields["DECISION_MEMO"].CurrentValues.AddValue(Sign.getDecisionMeno());
                    report.ParameterFields["GET_LEVEL"].CurrentValues.AddValue(Sign.getLevel());                    
                    report.ParameterFields["isHome"].CurrentValues.AddValue(RPT_C.C_offduty.isHome(rpt_dt));
                    CrystalReportViewer1.ReportSource = report;

                    //2013/10/15
                    if (TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00005", TPMPermissions._boolTPM(), "") == "N")
                    {
                        TPMPermissions._boolErrorData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00005","");
                    }

                    break;

                #endregion 印請假單-每次一張

                #region 4.3.2出差請示單
                case "businessTrip":
                    rpt_dt=RPT_C.C_businessTrip.doSearch( Session["RPT_C_IDNO"]==null?"":Session["RPT_C_IDNO"].ToString() ,HttpUtility.UrlDecode(Request["MZ_NAME"]) ,Request["DATE"]) ;

                    if (rpt_dt.Rows.Count == 0)
                        lb_tip.Text = " 無此日期之相關資料";


                    if (!string.IsNullOrEmpty(lb_tip.Text))
                    {
                        lb_tip.Visible = true;
                        return;
                    }
                    report.SetDataSource(rpt_dt);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(string.Format("{0} 出差請示單", o_A_KTYPE.RAD(Session["ADPMZ_EXAD"].ToString())));
                    CrystalReportViewer1.ReportSource = report;
                    break;
                #endregion 出差請示單

                #region 4.3.2旅費報告表
                case "businessTrip_Sub":

                    rpt_dt = RPT_C.C_businessTrip.doSearch(Session["RPT_C_IDNO"] == null ? "" : Session["RPT_C_IDNO"].ToString(), HttpUtility.UrlDecode(Request["MZ_NAME"]), Request["DATE"]);


                    if (rpt_dt.Rows.Count == 0)
                    {
                        lb_tip.Text = " 無此日期之相關資料";

                    }

                    if (!string.IsNullOrEmpty(lb_tip.Text))
                    {
                        lb_tip.Visible = true;
                        return;

                    }
                    DataTable temp = new DataTable();

                    temp.Columns.Add("SN1", typeof(string));
                    temp.Columns.Add("SN2", typeof(string));
                    temp.Columns.Add("SN3", typeof(string));
                    temp.Columns.Add("SN4", typeof(string));
                    temp.Columns.Add("SN5", typeof(string));
                    temp.Columns.Add("SN6", typeof(string));

                    string strSQL = string.Format(@"SELECT * FROM C_BUSSINESSTRIP WHERE MZ_DLTB01_SN={0}", rpt_dt.Rows[0]["MZ_DLTB01_SN"].ToString());

                    DataTable tempDT = new DataTable();

                    tempDT = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GETVALUE");

                    if (tempDT.Rows.Count > 0)
                    {
                        int out_count = tempDT.Rows.Count % 6 == 0 ? tempDT.Rows.Count / 6 : (tempDT.Rows.Count / 6) + 1;
                        for (int i = 0; i < out_count; i++)
                        {
                            if (i + 1 != out_count)
                            {
                                DataRow dr = temp.NewRow();
                                for (int j = 6 * i + 1; j < 6 * i + 1 + 6; j++)
                                {
                                    int columns = j % 6 == 0 ? 6 : j % 6;
                                    dr["SN" + columns.ToString()] = tempDT.Rows[j - 1]["SN"].ToString();
                                }
                                temp.Rows.Add(dr);
                            }
                            else
                            {
                                DataRow dr = temp.NewRow();
                                for (int j = 3 * i + 1; j <= tempDT.Rows.Count; j++)
                                {
                                    int columns = j % 6 == 0 ? 6 : j % 6;
                                    dr["SN" + columns.ToString()] = tempDT.Rows[j - 1]["SN"].ToString();
                                }
                                temp.Rows.Add(dr);
                            }
                        }
                    }

                    foreach (DataRow dr in temp.Rows)
                    {
                        if (string.IsNullOrEmpty(dr["SN2"].ToString()))
                        {
                            dr["SN2"] = "NULL";
                        }
                        if (string.IsNullOrEmpty(dr["SN3"].ToString()))
                        {
                            dr["SN3"] = "NULL";
                        }
                        if (string.IsNullOrEmpty(dr["SN4"].ToString()))
                        {
                            dr["SN4"] = "NULL";
                        }
                        if (string.IsNullOrEmpty(dr["SN5"].ToString()))
                        {
                            dr["SN5"] = "NULL";
                        }
                        if (string.IsNullOrEmpty(dr["SN6"].ToString()))
                        {
                            dr["SN6"] = "NULL";
                        }
                    }


                    report.SetDataSource(rpt_dt);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(string.Format("{0} 國內出差旅費報告表", o_A_KTYPE.RAD(Session["ADPMZ_EXAD"].ToString())));
                    report.ParameterFields["NAME"].CurrentValues.AddValue(rpt_dt.Rows[0]["MZ_NAME"].ToString());
                    report.ParameterFields["RANK"].CurrentValues.AddValue(rpt_dt.Rows[0]["MZ_RANK1"].ToString());
                    report.ParameterFields["OCCC"].CurrentValues.AddValue(o_A_KTYPE.CODE_TO_NAME(rpt_dt.Rows[0]["MZ_OCCC"].ToString(), "26"));
                    report.ParameterFields["UNIT"].CurrentValues.AddValue(rpt_dt.Rows[0]["MZ_EXUNIT"].ToString());
                    report.ParameterFields["START"].CurrentValues.AddValue(rpt_dt.Rows[0]["MZ_IDATE1"].ToString().Substring(0, 3) + " 年 " + rpt_dt.Rows[0]["MZ_IDATE1"].ToString().Substring(3, 2) + " 月 " + rpt_dt.Rows[0]["MZ_IDATE1"].ToString().Substring(5, 2) + " 日 " + rpt_dt.Rows[0]["MZ_ITIME1"].ToString().Replace(":", " 時 ") + " 分 ");
                    report.ParameterFields["END"].CurrentValues.AddValue(rpt_dt.Rows[0]["MZ_ODATE"].ToString().Substring(0, 3) + " 年 " + rpt_dt.Rows[0]["MZ_ODATE"].ToString().Substring(3, 2) + " 月 " + rpt_dt.Rows[0]["MZ_ODATE"].ToString().Substring(5, 2) + " 日 " + rpt_dt.Rows[0]["MZ_OTIME"].ToString().Replace(":", " 時 ") + " 分 ");
                    report.ParameterFields["CAUSE"].CurrentValues.AddValue(rpt_dt.Rows[0]["MZ_CAUSE"].ToString());
                    report.ParameterFields["TDAY"].CurrentValues.AddValue(rpt_dt.Rows[0]["MZ_TDAY"].ToString());
                    report.ParameterFields["TTIME"].CurrentValues.AddValue(rpt_dt.Rows[0]["MZ_TTIME"].ToString());
                    
                    
                    CrystalReportViewer1.ReportSource = report;
                    break;
                #endregion 旅費報告表

                #region 4.3.3勤惰卡
                case "shift":

                    rpt_dt = RPT_C.C_shift.doSearch_shift(Request["YEAR"] == null ? "" : Request["YEAR"].ToString(), Request["YEAR2"] == null ? "" : Request["YEAR2"].ToString(), Session["RPT_C_IDNO"] == null ? "" : Session["RPT_C_IDNO"].ToString());


                    report.SetDataSource(rpt_dt);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(string.Format("{0}年度 勤惰記錄卡", int.Parse(Request["YEAR"].ToString().Substring(0, 3))));

                    CrystalReportViewer1.ReportSource = report;
                    break;

                case "shift1":

                    rpt_dt = RPT_C.C_shift.doSearch_shift1(Request["YEAR"] == null ? "" : Request["YEAR"].ToString(), Session["RPT_C_IDNO"] == null ? "" : Session["RPT_C_IDNO"].ToString());

                    report.SetDataSource(rpt_dt);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(string.Format("{0}年度 勤惰記錄卡", int.Parse(Request["YEAR"].ToString().Substring(0, 3))).Trim());
                    CrystalReportViewer1.ReportSource = report;
                    break;
                #endregion 4.3.3勤惰卡

                #region 4.3.4 個人差假明細表
                case "holiday":
                    int ff_day = 0;
                    int ff_hour = 0;
                    int th_day = 0;
                    int th_hour = 0;
                    foreach (DataRow dr in rpt_dt.Rows)
                    {
                        dr["MZ_EXUNIT"] = o_A_DLBASE.CUNIT(dr["MZ_ID"].ToString());

                        dr["MZ_OCCC"] = o_A_DLBASE.EXTPOS_OR_OCCC(dr["MZ_ID"].ToString(), dr["MZ_OCCC"].ToString());

                        if (dr["MZ_CODE"].ToString().Trim() == "補休")
                        {
                            ff_day += int.Parse(dr["MZ_TDAY"].ToString());
                            ff_hour += int.Parse(dr["MZ_TTIME"].ToString());
                        }
                        else if (dr["MZ_CODE"].ToString().Trim() == "休假")
                        {
                            th_day += int.Parse(dr["MZ_TDAY"].ToString());
                            th_hour += int.Parse(dr["MZ_TTIME"].ToString());
                        }
                        //請假事由最多顯示30字,避免破版
                        string MZ_CAUSE = dr["MZ_CAUSE"].ToString();
                        MZ_CAUSE = MZ_CAUSE.Replace("\n", "");
                        MZ_CAUSE = MZ_CAUSE.Replace("\r", "");
                        if (MZ_CAUSE.Length > 20)
                        {
                            MZ_CAUSE = MZ_CAUSE.Substring(0, 20) + "...";
                            dr["MZ_CAUSE"] = MZ_CAUSE;
                        }
                    }

                    th_day = th_day + th_hour / 8;
                    ff_day = ff_day + ff_hour / 8;

                    report.SetDataSource(rpt_dt);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(string.Format("{0}個人差假明細表", o_A_KTYPE.RAD(Session["ADPMZ_EXAD"].ToString())));
                    report.ParameterFields["DOWN"].CurrentValues.AddValue("共計：休假 " + th_day + " 日 " + th_hour % 8 + " 時   補休 " + ff_day + " 日 " + ff_hour % 8 + " 時 ");
                    report.ParameterFields["HOLIDAY_UP"].CurrentValues.AddValue(Session["HOLIDAY_UP"].ToString().Trim());
                    CrystalReportViewer1.ReportSource = report;
                    break;
                #endregion 4.3.4 個人差假明細表

                #region 4.3.5各別假別統計表
                case "personalduty":

                    report.SetDataSource(Session["rpt_dt"] as DataTable);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(Session["TITLE"].ToStringNullSafe().SafeTrim());

                    report.ParameterFields["personaldhtydate"].CurrentValues.AddValue(Session["personaldhtydate"].ToStringNullSafe().SafeTrim());
                    CrystalReportViewer1.ReportSource = report;
                    break;
                #endregion

                #region 4.3.6 員警應休假查核表
                case "ToBeOffDuty"://4.3.6 員警應休假查核表

                    for (int i = 0; i < rpt_dt.Rows.Count; i++)
                    {
                        //任公職年資月數
                        string FDATE = rpt_dt.Rows[i]["MZ_FDATE"].ToString();

                        int tyear = int.Parse(string.IsNullOrEmpty(rpt_dt.Rows[i]["MZ_TYEAR"].ToString()) ? "0" : rpt_dt.Rows[i]["MZ_TYEAR"].ToString());

                        int tmonth = int.Parse(string.IsNullOrEmpty(rpt_dt.Rows[i]["MZ_TMONTH"].ToString()) ? "0" : rpt_dt.Rows[i]["MZ_TMONTH"].ToString());

                        int ryear = int.Parse(string.IsNullOrEmpty(rpt_dt.Rows[i]["MZ_RYEAR"].ToString()) ? "0" : rpt_dt.Rows[i]["MZ_RYEAR"].ToString());

                        int rmonth = int.Parse(string.IsNullOrEmpty(rpt_dt.Rows[i]["MZ_RMONTH"].ToString()) ? "0" : rpt_dt.Rows[i]["MZ_RMONTH"].ToString());


                        if (!string.IsNullOrEmpty(FDATE))
                        {

                            System.DateTime dt1;

                            try
                            {
                                dt1 = DateTime.Parse((int.Parse(FDATE.Substring(0, 3)) + 1911).ToString() + "-" + FDATE.Substring(3, 2) + "-" + FDATE.Substring(5, 2));
                            }
                            catch
                            {
                                dt1 = DateTime.Now;
                            }

                            System.DateTime dt2 = DateTime.Now;

                            int monthDiff = Convert.ToInt32(Microsoft.VisualBasic.DateAndTime.DateDiff("M", dt1, dt2, Microsoft.VisualBasic.FirstDayOfWeek.Sunday, Microsoft.VisualBasic.FirstWeekOfYear.FirstFullWeek)) + tyear * 12 + tmonth - ryear * 12 - rmonth;

                            if (monthDiff > 0)
                            {
                                rpt_dt.Rows[i]["MZ_OFFYY"] = monthDiff / 12;
                                rpt_dt.Rows[i]["MZ_OFFMM"] = monthDiff % 12;
                            }
                            else
                            {
                                rpt_dt.Rows[i]["MZ_OFFYY"] = 0;
                                rpt_dt.Rows[i]["MZ_OFFMM"] = 0;
                            }
                        }
                        else
                        {
                            rpt_dt.Rows[i]["MZ_OFFYY"] = 0;
                            rpt_dt.Rows[i]["MZ_OFFMM"] = 0;
                        }
                    }


                    report.SetDataSource(rpt_dt);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(string.Format("{0}員警應休假查核表", o_A_KTYPE.RAD(Request["EXAD"].ToString())));
                    CrystalReportViewer1.ReportSource = report;
                    break;
                #endregion 4.3.6 員警應休假查核表

                #region 4.3.7 員警差假統計表
                case "codecount":

                    report.SetDataSource(Session["rpt_dt"] as DataTable);
                    report.ParameterFields["BEGINDATE"].CurrentValues.AddValue(Session["BEGINDATE"].ToString().Trim());
                    report.ParameterFields["ENDDATE"].CurrentValues.AddValue(Session["ENDDATE"].ToString().Trim());
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(Session["TITLE"].ToString().Trim());
                    CrystalReportViewer1.ReportSource = report;
                    break;
                #endregion

                #region 4.3.8員警差假明細表  報表的製表日期怪怪的.先用奇怪的方式補(因為只出現2,再report前寫死加10)

                case "dutydetail":
                    for (int i = 0; i < rpt_dt.Rows.Count; i++)
                    {
                        rpt_dt.Rows[i]["MZ_OCCC"] = o_A_DLBASE.EXTPOS_OR_OCCC(rpt_dt.Rows[i]["MZ_ID"].ToString(), rpt_dt.Rows[i]["MZ_OCCC"].ToString());
                        //請假事由最多顯示30字,避免破版
                        string MZ_CAUSE = rpt_dt.Rows[i]["MZ_CAUSE"].ToString();
                        if (MZ_CAUSE.Length > 30)
                        {
                            MZ_CAUSE = MZ_CAUSE.Substring(0, 30) + "...";
                            rpt_dt.Rows[i]["MZ_CAUSE"] = MZ_CAUSE;
                        }
                    }  
                    report.SetDataSource(rpt_dt);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(Session["TITLE"].ToString().Trim());
                    CrystalReportViewer1.ReportSource = report;
                    break;
                #endregion 4.3.8員警差假明細表

                #region 4.3.9赴大陸假單
                case "chinaabroad":
                    String LastTimeToChinaDateUglyString = String.Empty; //上次前往中國之日期
                    Int32 Amount_ThisYearToChina = 0;

                    rpt_dt = RPT_C.C_chinaabroad.doSearch(Session["RPT_C_IDNO"] == null ? "" : Session["RPT_C_IDNO"].ToString()
                                                          ,HttpUtility.UrlDecode(Request["MZ_NAME"])
                                                          ,Request["DATE"]
                                                          ,ref LastTimeToChinaDateUglyString
                                                          ,ref Amount_ThisYearToChina);

                    if (rpt_dt.Rows.Count == 0)
                        lb_tip.Text = " 無此日期之相關資料";


                    if (!string.IsNullOrEmpty(lb_tip.Text))
                    {
                        lb_tip.Visible = true;
                        return;
                    }

                    report.SetDataSource(rpt_dt);
                    report.ParameterFields["PLACE"].CurrentValues.AddValue(RPT_C.C_chinaabroad.place(rpt_dt));
                    report.ParameterFields["DURING"].CurrentValues.AddValue(RPT_C.C_chinaabroad.during(rpt_dt, Logic.LogicVacation.enumChinaDateRangeFormat.Standard));
                    report.ParameterFields["DURING_LastToChina"].CurrentValues.AddValue(RPT_C.C_chinaabroad.during(LastTimeToChinaDateUglyString, Logic.LogicVacation.enumChinaDateRangeFormat.Standard));
                    report.ParameterFields["AMOUNT_ThisYearToChina"].CurrentValues.AddValue(Amount_ThisYearToChina); //本年度赴大陸次數(含本次)

                    //report.ParameterFields["TITLE"].CurrentValues.AddValue(Session["TITLE"].ToString().Trim());
                    CrystalReportViewer1.ReportSource = report;
                    break;
                #endregion 4.3.9赴大陸假單

                #region 4.3.9申請人赴大陸地區注意事項
                case "chinaabroad1"://4.3.9申請人赴大陸地區注意事項                   
                    report.ParameterFields["TITLE"].CurrentValues.AddValue("申請人赴大陸地區注意事項");
                    CrystalReportViewer1.ReportSource = report;
                    break;
                #endregion 4.3.9申請人赴大陸地區注意事項

                #region 4.3.10 出國請假單 4.1也有
                case "abroad":
                    rpt_dt = RPT_C.C_abroad.doSearch(Session["RPT_C_IDNO"] == null ? "" : Session["RPT_C_IDNO"].ToString(), HttpUtility.UrlDecode(Request["MZ_NAME"]), Request["DATE"]);

                    if (rpt_dt.Rows.Count == 0)
                        lb_tip.Text = " 無此日期之相關資料";


                    if (!string.IsNullOrEmpty(lb_tip.Text))
                    {
                        lb_tip.Visible = true;
                        return;
                    }

                    report.SetDataSource(rpt_dt);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(string.Format("{0}職員出國請假報告單", o_A_KTYPE.RAD(Session["ADPMZ_EXAD"].ToString())));
                    //report.ParameterFields["DURING"].CurrentValues.AddValue(RPT_C.C_chinaabroad.during(rpt_dt, Logic.LogicVacation.enumChinaDateRangeFormat.StandardWithTime));

                    CrystalReportViewer1.ReportSource = report;
                    break;
                #endregion 4.3.10 出國請假單

                #region 4.3.10 赴大陸請假單 4.1也有
                case "abroad_CN":
                    rpt_dt = RPT_C.C_abroad.doSearch(Session["RPT_C_IDNO"] == null ? "" : Session["RPT_C_IDNO"].ToString(), HttpUtility.UrlDecode(Request["MZ_NAME"]), Request["DATE"]);

                    if (rpt_dt.Rows.Count == 0)
                        lb_tip.Text = " 無此日期之相關資料";


                    if (!string.IsNullOrEmpty(lb_tip.Text))
                    {
                        lb_tip.Visible = true;
                        return;
                    }

                    report.SetDataSource(rpt_dt);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(string.Format("{0}職員赴大陸地區請假報告單", o_A_KTYPE.RAD(Session["ADPMZ_EXAD"].ToString())));
                    //report.ParameterFields["DURING"].CurrentValues.AddValue(RPT_C.C_chinaabroad.during(rpt_dt, Logic.LogicVacation.enumChinaDateRangeFormat.StandardWithTime));
                    CrystalReportViewer1.ReportSource = report;
                    break;
                #endregion 4.3.10 赴大陸請假單

                #region 4.3.11 職員非請假出國報備表 4.1也有
                case "ondutyabroad":
                    rpt_dt = RPT_C.C_ondutyabroad.doSearch(Session["RPT_C_IDNO"] == null ? "" : Session["RPT_C_IDNO"].ToString(), HttpUtility.UrlDecode(Request["MZ_NAME"]), Request["DATE"]);

                    if (rpt_dt.Rows.Count == 0)
                        lb_tip.Text = " 無此日期之相關資料";


                    if (!string.IsNullOrEmpty(lb_tip.Text))
                    {
                        lb_tip.Visible = true;
                        return;
                    }
                    report.SetDataSource(rpt_dt);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue("新北市政府警察局 職員非請假期間出國報備表");
                    //report.ParameterFields["DURING"].CurrentValues.AddValue(RPT_C.C_chinaabroad.during(rpt_dt, Logic.LogicVacation.enumChinaDateRangeFormat.StandardWithTime));

                    CrystalReportViewer1.ReportSource = report;
                    break;
                #endregion 4.3.11 職員非請假出國報備表 4.1也有

                #region 4.3.11 職員非請假出國報備表 4.1也有
                case "ondutyabroad_CN":
                    rpt_dt = RPT_C.C_ondutyabroad.doSearch(Session["RPT_C_IDNO"] == null ? "" : Session["RPT_C_IDNO"].ToString(), HttpUtility.UrlDecode(Request["MZ_NAME"]), Request["DATE"]);

                    if (rpt_dt.Rows.Count == 0)
                        lb_tip.Text = " 無此日期之相關資料";


                    if (!string.IsNullOrEmpty(lb_tip.Text))
                    {
                        lb_tip.Visible = true;
                        return;
                    }
                    report.SetDataSource(rpt_dt);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue("新北市政府警察局 職員非請假期間赴大陸地區報備表");
                    //report.ParameterFields["DURING"].CurrentValues.AddValue(RPT_C.C_chinaabroad.during(rpt_dt, Logic.LogicVacation.enumChinaDateRangeFormat.StandardWithTime));

                    CrystalReportViewer1.ReportSource = report;
                    break;
                #endregion 4.3.11 職員非請假出國報備表 4.1也有

                #region 4.3.12員警請假核定通知書
                case "offCFM":

                    report.SetDataSource(Session["rpt_dt"] as DataTable);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(Session["TITLE"].ToString().Trim());
                    report.ParameterFields["offCM_DATE1"].CurrentValues.AddValue(Session["offCM_DATE1"].ToString().Trim());
                    report.ParameterFields["offCM_DATE2"].CurrentValues.AddValue(Session["offCM_DATE2"].ToString().Trim());
                    report.ParameterFields["offCM_PRID1"].CurrentValues.AddValue(Session["offCM_PRID1"].ToString().Trim());
                    report.ParameterFields["offCM_PRID2"].CurrentValues.AddValue(Session["offCM_PRID2"].ToString().Trim());
                    report.ParameterFields["CM_AD"].CurrentValues.AddValue(Session["CM_AD"].ToString().Trim());
                    CrystalReportViewer1.ReportSource = report;
                    break;
                #endregion

                #region 4.3.13請假報告單 4.1也有
                case "askleave":


                    report.SetDataSource(Session["rpt_dt"] as DataTable);
                    report.ParameterFields["TO"].CurrentValues.AddValue(Session["TO"].ToString().Trim());
                    report.ParameterFields["DATE"].CurrentValues.AddValue(Session["DATE"].ToString().Trim());
                    report.ParameterFields["PRID"].CurrentValues.AddValue(Session["PRID"].ToString().Trim());
                    report.ParameterFields["MEMO"].CurrentValues.AddValue(Session["MEMO"].ToString().Trim());

                    report.ParameterFields["printtime"].CurrentValues.AddValue(Convert.ToInt32(DateTime.Now.Year) - 1911 + " 年 " + DateTime.Now.Month.ToString().PadLeft(2, '0') + " 月 " + DateTime.Now.Day.ToString().PadLeft(2, '0') + " 日 ");
                    CrystalReportViewer1.ReportSource = report;
                    break;
                #endregion

                #region 4.3.14出差前50名報表
                case "busitrip50"://4.3.14出差前50名報表

                    rpt_dt = RPT_C.C_busitrip50.doSearch(Request["DATE1"] == null ? "" : Request["DATE1"].ToString(), Request["DATE2"] == null ? "" : Request["DATE2"].ToString(), Request["EXAD"] == null ? "" : Request["EXAD"].ToString());
                    report.SetDataSource(rpt_dt);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(string.Format("{0}出差前50名報表", o_A_KTYPE.RAD(Request["EXAD"] )));
                    report.ParameterFields["UP"].CurrentValues.AddValue(RPT_C.C_busitrip50.UP(Request["DATE1"] == null ? "" : Request["DATE1"].ToString(), Request["DATE2"] == null ? "" : Request["DATE2"].ToString().Trim()));
                    CrystalReportViewer1.ReportSource = report;
                    break;
                #endregion 4.3.14出差前50名報表

                #region 4.3.15因公未休假改發加班費印領清冊
                case "noplaylist":

                    report.SetDataSource(Session["rpt_dt"] as DataTable);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(Session["TITLE"].ToString().Trim());
                    report.ParameterFields["TITLE1"].CurrentValues.AddValue(Session["TITLE1"].ToString().Trim());
                    CrystalReportViewer1.ReportSource = report;
                    break;
                #endregion

                #region 4.3.16已請領強制休假補助費明細表
                case "paided":

                    report.SetDataSource(Session["rpt_dt"] as DataTable);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(Session["TITLE"].ToString().Trim());
                    CrystalReportViewer1.ReportSource = report;
                    break;
                #endregion

                #region 4.3.17未請領強制休假補助費明細表
                case "nopaid":

                    report.SetDataSource(Session["rpt_dt"] as DataTable);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(Session["TITLE"].ToString().Trim());
                    CrystalReportViewer1.ReportSource = report;
                    break;
                #endregion

                #region 4.3.18刷卡(勤惰)明細表
                case "diffdutydetail"://4.3.18刷卡(勤惰)明細表 異常   這兩張報表一筆一筆兜資料.很慢,有時間再修
                case "diffdutydetail2"://4.3.18刷卡(勤惰)明細表 全部和加班 這兩張報表一筆一筆兜資料.很慢,有時間再修
                case "over_detail":// 4.3.18刷卡(勤惰)明細表 差勤
                case "day_detail":// 4.3.18刷卡(勤惰)明細表 值日
                    rpt_dt = RPT_C.C_diffdutydetail.doSearch(Request["DATE1"], Request["DATE2"], Request["AD"], Request["UNIT"], Session["RPT_C_IDNO"] == null ? "" : Session["RPT_C_IDNO"].ToString(), HttpUtility.UrlDecode(Request["NAME"]), Request["TYPE"]);

                    //if (rpt_dt.Rows.Count == 0)
                    //{
                    //    lb_tip.Text = " 無此日期之相關資料";

                    //}

                    if (!string.IsNullOrEmpty(lb_tip.Text))
                    {
                        lb_tip.Visible = true;
                        return;

                    }

                    report.SetDataSource(rpt_dt);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(RPT_C.C_diffdutydetail.doTitle(Request["TYPE"], o_A_KTYPE.RAD(Request["AD"])));
                    
                    CrystalReportViewer1.ReportSource = report;
                    break;
                #endregion

                #region 4.3.19勤惰(刷卡)紀錄明細表
                case "carddetail":

                    report.SetDataSource(Session["rpt_dt"] as DataTable);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(RPT_C.C_diffdutydetail.doTitle(Request["TYPE"], o_A_KTYPE.RAD(Request["AD"])));
                    CrystalReportViewer1.ReportSource = report;
                    break;
                #endregion

                #region 4.3.20外勤時間表
                case "finger_out":
                    MZ_ID = Session["MZ_ID"].ToStringNullSafe();
                    Session.Remove(MZ_ID);
                    rpt_dt.Clear();
                    rpt_dt = RPT_C.C_time_out.doSearch(Request["DATE"], Request["DATE2"], Request["AD"], Request["UNIT"], MZ_ID);
                    report.SetDataSource(rpt_dt);

                    report.ParameterFields["TITLE"].CurrentValues.AddValue(o_A_KTYPE.RAD(Request["AD"]) + o_A_KTYPE.RUNIT(Request["UNIT"]) + int.Parse(Request["Date"].Substring(0, 3)) + "年" + Request["Date"].Substring(3, 2) + "月差勤時間明細表");
                    report.ParameterFields["MZ_DATE"].CurrentValues.AddValue(Convert.ToInt32(DateTime.Now.Year) - 1911 + " 年 " + DateTime.Now.Month.ToString().PadLeft(2, '0') + " 月 " + DateTime.Now.Day.ToString().PadLeft(2, '0') + " 日 ");

                    CrystalReportViewer1.ReportSource = report;

                    break;
                #endregion

                #endregion 4.3  差勤報表    包函請假輸入 1.2.10.11.13

                case "dutymonthhour":
                      
                    //report.ParameterFields["EXUNIT"].CurrentValues.AddValue(Session["C_MONTHHOUR_EXUNIT"].ToString().Trim());
                    //report.ParameterFields["EXAD"].CurrentValues.AddValue(Session["C_MONTHHOUR_EXAD"].ToString().Trim());
                    //report.ParameterFields["YEAR"].CurrentValues.AddValue(Session["C_MONTHHOUR_YEAR"].ToString().Trim());
                    //report.ParameterFields["MONTH"].CurrentValues.AddValue(Session["C_MONTHHOUR_MONTH"].ToString().Trim());
                    report.SetDataSource(Session["rpt_dt"] as DataTable);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(Session["TITLE"].ToString().Trim());
                    CrystalReportViewer1.ReportSource = report;
                    break;

                #region 5.加班管理作業 報表

                #region 5.1 加班請示單
                case "OvertimeInsideAsk":
                    rpt_dt = RPT_C.OvertimeInsideAsk.doSearch(Session["RPT_C_IDNO"] == null ? "" : Session["RPT_C_IDNO"].ToString(), Request["DATE"]);
                    report.SetDataSource(rpt_dt);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(string.Format("{0}", o_A_KTYPE.RAD(Session["ADPMZ_EXAD"].ToString())));
                    CrystalReportViewer1.ReportSource = report;
                    break;
                #endregion

                #region 5.2加班明細表
                case "OvertimeInsideDetail":
                    rpt_dt.Clear();
                    rpt_dt = RPT_C.OvertimeInsideDetail.doSearch(Request["DATE"], Request["AD"], Request["UNIT"], Session["RPT_C_IDNO"] == null ? "" : Session["RPT_C_IDNO"].ToString());
                    report.SetDataSource(rpt_dt);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(string.Format("{0}加班費管制記錄卡   {1}年度", o_A_KTYPE.RAD(Request["AD"]), Request["DATE"].Substring(0, 3)));
                    report.ParameterFields["UNIT"].CurrentValues.AddValue(o_A_KTYPE.RUNIT(Request["UNIT"]));
                    CrystalReportViewer1.ReportSource = report;
                    break;
                #endregion

                #region 5.3員工加班明細表
                case "OvertimeInsideTotal":

                    report.SetDataSource(Session["rpt_dt"] as DataTable);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(Session["TITLE"].ToString().Trim());
                    report.ParameterFields["TITLE2"].CurrentValues.AddValue(Session["TITLE2"].ToString().Trim());
                    CrystalReportViewer1.ReportSource = report;
                    break;
                #endregion


                //TODO SKY 正式上線時刪除
                #region 5.1 加班請示單
                case "OvertimeInsideAsk_New":
                    MZ_ID = Session["RPT_C_IDNO"].ToStringNullSafe();
                    DATE = Session["RPT_C_DATE"].ToStringNullSafe();

                    rpt_dt = RPT_C.OvertimeInsideAsk.doSearch_New(MZ_ID, DATE);
                    report.SetDataSource(rpt_dt);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(string.Format("{0}", o_A_KTYPE.RAD(Session["ADPMZ_EXAD"].ToString())));
                    CrystalReportViewer1.ReportSource = report;

                    LogModel.saveLog("CF", "S", "產生5.1 加班請示單", new List<SqlParameter>(), TPM_FION, "人事系統產生報表。");
                    break;
                #endregion

                #region 5.2加班明細表
                case "OvertimeInsideDetail_New":
                    MZ_ID = Session["RPT_C_IDNO"].ToStringNullSafe();
                    DATE = Session["RPT_C_DATE"].ToStringNullSafe();
                    MZ_AD = Request["AD"].ToStringNullSafe();
                    MZ_UNIT = Request["UNIT"].ToStringNullSafe();

                    rpt_dt.Clear();
                    rpt_dt = RPT_C.OvertimeInsideDetail.doSearch_New(DATE, MZ_AD, MZ_UNIT, MZ_ID);
                    report.SetDataSource(rpt_dt);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(string.Format("{0}加班費管制記錄卡   {1}年度", o_A_KTYPE.RAD(MZ_AD), DATE.SubstringOutToEmpty(0, 3)));
                    report.ParameterFields["UNIT"].CurrentValues.AddValue(o_A_KTYPE.RUNIT(MZ_UNIT));
                    CrystalReportViewer1.ReportSource = report;
                    break;
                #endregion

                #region 5.3員工加班明細表
                case "OvertimeInsideTotal_New":
                    MZ_AD = Request["AD"].ToStringNullSafe();
                    MZ_UNIT = Request["UNIT"].ToStringNullSafe();
                    DATE = Session["RPT_C_DATE"].ToStringNullSafe();

                    rpt_dt.Clear();
                    rpt_dt = RPT_C.OvertimeInsideTotal.doSearch(MZ_AD, MZ_UNIT, DATE);
                    report.SetDataSource(rpt_dt);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(Session["TITLE"].ToString().Trim());
                    report.ParameterFields["TITLE2"].CurrentValues.AddValue(Session["TITLE2"].ToString().Trim());
                    CrystalReportViewer1.ReportSource = report;
                    break;
                #endregion

                #region 5.x新加班勤惰明細表
                case "OverTimeDutyDetail":
                    C_OverTime_Detail_rpt_Query query = Session["PageQuery"] as C_OverTime_Detail_rpt_Query;

                    rpt_dt.Clear();
                    rpt_dt = RPT_C.OverTimeDutyDetail.doSearch(query);

                    string title = string.Format("{0}{1} 加班明細表"
                        , AccountService.getSysPara(query.Search_EXAD, AccountService.enumCategroy.AD)
                        , AccountService.getSysPara(query.Search_EXUNIT, AccountService.enumCategroy.UNIT));
                    Model.AccountModel aModel = null;
                    if (!string.IsNullOrEmpty(query.Search_ID))
                    {
                        aModel = AccountService.lookupAccount(query.Search_ID);
                    }
                    else if (!string.IsNullOrEmpty(query.Search_NAME))
                    {
                        aModel = AccountService.lookupAccountByName(query.Search_NAME);
                    }
                    string name_occc = aModel != null ? 
                        string.Format("{0}{1}", AccountService.getSysPara(aModel.Title, AccountService.enumCategroy.OCCC), aModel.Name) : "";

                    report.SetDataSource(rpt_dt);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(title);
                    report.ParameterFields["NAME_OCCC"].CurrentValues.AddValue(name_occc);
                    CrystalReportViewer1.ReportSource = report;
                    break;
                #endregion
                #endregion

                #region 6.3 超勤報表

                case "Overtimepay_detail"://6.3.1超勤時數統計表_ㄧ般人員
                      
                    report.SetDataSource(Session["rpt_dt"] as DataTable);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(Session["TITLE"].ToString().Trim());
                    //report.ParameterFields["DutyUserClassify"].CurrentValues.AddValue(Session["DutyUserClassify"].ToString().Trim());
                    CrystalReportViewer1.ReportSource = report;
                    break;
                case "C_Overtimepay_detail_Criminal"://6.3.1超勤時數統計表_刑事、少年、婦幼

                    report.SetDataSource(Session["rpt_dt"] as DataTable);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(Session["TITLE"].ToString().Trim());
                    //report.ParameterFields["DutyUserClassify"].CurrentValues.AddValue(Session["DutyUserClassify"].ToString().Trim());
                    CrystalReportViewer1.ReportSource = report;
                    break;

                #region 6.3.2超勤印領清冊
                case "overtimerequestlistrpt"://6.3.2超勤印領清冊
                    for (int i = 0; i < rpt_dt.Rows.Count; i++)
                    {
                        rpt_dt.Rows[i]["PAGE"] = i / 10;
                    }
                    report.SetDataSource(rpt_dt);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(Session["TITLE"].ToString().Trim());
                    CrystalReportViewer1.ReportSource = report;
                    break;
                #endregion 6.3.2超勤印領清冊

                case "OvertimeOutSide_Unit"://6.3.3超勤支領情形分析表(小時)   //如果不選單位  與6.3.4共用
                    DataTable dt= RPT_C.OVERTIME.OvertimeOutSide_Unit.doSearch(Request["DATE"], Request["MZ_AD"], Request["MZ_UNIT"]);
                    report.SetDataSource(dt);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue((string.Format("{0}{1}{2}超勤加班費支領情形分析表 ", o_A_KTYPE.RAD(Request["MZ_AD"]),o_A_KTYPE.RUNIT(Request["MZ_UNIT"]), Request["DATE"])));
                    //補上部分欄位,特殊:實際上是最後列列才有資料
                    int iRows = dt.Rows.Count-1;
                    if (dt.Rows.Count > 0)
                    {
                        report.ParameterFields["I_CASH18"].CurrentValues.AddValue(dt.Rows[iRows]["I_CASH18"].ToString());
                        report.ParameterFields["I_CASH19"].CurrentValues.AddValue(dt.Rows[iRows]["I_CASH19"].ToString());
                        report.ParameterFields["O_CASH18"].CurrentValues.AddValue(dt.Rows[iRows]["O_CASH18"].ToString());
                        report.ParameterFields["O_CASH19"].CurrentValues.AddValue(dt.Rows[iRows]["O_CASH19"].ToString());
                    }
                    else
                    {
                        report.ParameterFields["I_CASH18"].CurrentValues.AddValue(0);
                        report.ParameterFields["I_CASH19"].CurrentValues.AddValue(0);
                        report.ParameterFields["O_CASH18"].CurrentValues.AddValue(0);
                        report.ParameterFields["O_CASH19"].CurrentValues.AddValue(0);
                    }
                    TPPDDB.App_Code.Log.SaveLog(                        
                        " I_CASH18=" +dt.Rows[iRows]["I_CASH18"].ToString()
                       + " I_CASH19=" + dt.Rows[iRows]["I_CASH19"].ToString()
                       + " O_CASH18=" + dt.Rows[iRows]["O_CASH18"].ToString()
                       + " O_CASH19=" + dt.Rows[iRows]["O_CASH19"].ToString()
                        );
                    CrystalReportViewer1.ReportSource = report;
                    break;

                case "OvertimeOutSide_Hour"://6.3.3超勤支領情形分析表(小時) //有選單位

                    //取得機關名稱及單位名稱
                    String ADName = AccountService.getSysPara(Request["MZ_AD"], AccountService.enumCategroy.AD);

                    report.SetDataSource(RPT_C.OVERTIME.OvertimeOutSide_Hour.doSearch(Request["DATE"], Request["MZ_AD"]));
                    //report.ParameterFields["TITLE"].CurrentValues.AddValue(string.Format("{0}{1}超勤加班費支領情形分析表(小時) ",o_A_KTYPE.RAD(Request["MZ_AD"]), Request["DATE"]));
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(string.Format("{0}{1}",
                                                                           Request["DATE"],
                                                                           ADName));
                    CrystalReportViewer1.ReportSource = report;
                    break;

                case "OvertimeOutSide_Cost"://6.3.4超勤支領情形分析表(金額)

                    //取得機關名稱及單位名稱
                    String ADName2 = AccountService.getSysPara(Request["MZ_AD"], AccountService.enumCategroy.AD);

                    report.SetDataSource(RPT_C.OVERTIME.OvertimeOutSide_Cost.doSearch(Request["DATE"], Request["MZ_AD"]));
                    //report.ParameterFields["TITLE"].CurrentValues.AddValue(string.Format("{0}{1}超勤加班費支領情形分析表(金額) ", o_A_KTYPE.RAD(Request["MZ_AD"]), Request["DATE"]));
                    //report.ParameterFields["TITLE"].CurrentValues.AddValue(string.Format("{0}{1}",
                    //                                                       Request["DATE"],
                    //                                                       Request["MZ_AD"]));
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(string.Format("{0}{1}",
                                                                           Request["DATE"],
                                                                           ADName2));
                    CrystalReportViewer1.ReportSource = report;
                    break;

                #endregion 6.3 超勤報表

                case "dutyday6":
                case "dutyday7":
                case "dutyday8":
                case "dutyday9":
                      
                    report.SetDataSource(Session["rpt_dt"] as DataTable);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(Session["TITLE"].ToString().Trim());
                    CrystalReportViewer1.ReportSource = report;
                    break;
                case "monthturns":
                      
                    report.SetDataSource(Session["rpt_dt"] as DataTable);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(Session["TITLE"].ToString().Trim());
                    report.ParameterFields["YEAR"].CurrentValues.AddValue(Session["MZ_YEAR"].ToString().Trim());
                    report.ParameterFields["MONTH"].CurrentValues.AddValue(Session["MZ_MONTH"].ToString().Trim());
                    report.ParameterFields["WEEKDAY"].CurrentValues.AddValue(Session["WEEKDAY"].ToString().Trim());
                    report.ParameterFields["MONTHDAYS"].CurrentValues.AddValue(Session["MONTHDAYS"].ToString().Trim());
                    CrystalReportViewer1.ReportSource = report;
                    break;

               
               
               
                case "OvertimePayMoneyList":
                      
                    report.SetDataSource(Session["rpt_dt"] as DataTable);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(Session["TITLE"].ToString().Trim());
                    CrystalReportViewer1.ReportSource = report;
                    break;
                case "noplaylistTotal":
                      
                    report.SetDataSource(Session["rpt_dt"] as DataTable);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(Session["TITLE"].ToString().Trim());
                    report.ParameterFields["TITLE2"].CurrentValues.AddValue(Session["TITLE2"].ToString().Trim());
                    CrystalReportViewer1.ReportSource = report;
                    break;


                #region  輪值報表

                case "fingerdetail": //11.9.4指紋明細表
                     MZ_ID = string.Empty;
                    if (Session["MZ_ID"] != null)
                    {
                        MZ_ID = Session["MZ_ID"].ToString();
                    }
                    Session.Remove(MZ_ID);
                    rpt_dt.Clear();
                    rpt_dt = RPT_C.fingerdetail.doSearch(Request["DATE"],Request["DATE2"], Request["AD"], Request["UNIT"],MZ_ID);
                   // rpt_dt = RPT_C.fingerdetail.sortDataTable(rpt_dt);
                    report.SetDataSource(rpt_dt);

                    report.ParameterFields["TITLE"].CurrentValues.AddValue(o_A_KTYPE.RAD(Request["AD"]) + o_A_KTYPE.RUNIT(Request["UNIT"]) + int.Parse(Request["Date"].Substring(0, 3)) + "年" + Request["Date"].Substring(3, 2) + "月指紋時間表");
                    //report.ParameterFields["MZ_EXAD"].CurrentValues.AddValue(o_A_DLBASE.CMZAD(Request["ID"]));
                    //report.ParameterFields["MZ_UNIT"].CurrentValues.AddValue(o_A_KTYPE.RUNIT(Request["UNIT"]));
                    //report.ParameterFields["MZ_NAME"].CurrentValues.AddValue(o_A_DLBASE.CNAME(Request["ID"]));
                    //report.ParameterFields["MZ_OCCC"].CurrentValues.AddValue(o_DBFactory.ABC_toTest.name_OCCC(o_A_DLBASE.CNAME(Request["ID"])));
                    report.ParameterFields["MZ_DATE"].CurrentValues.AddValue(Convert.ToInt32(DateTime.Now.Year) - 1911 + " 年 " + DateTime.Now.Month.ToString().PadLeft(2, '0') + " 月 " + DateTime.Now.Day.ToString().PadLeft(2, '0') + " 日 ");

                    CrystalReportViewer1.ReportSource = report;
                    
                    break;


                #endregion


                case "ONDUTY_LIST":
                    rpt_dt = RPT_C.C_ONTYDUTY_UnitList.doSearch( Request["EXAD"], Request["EXUNIT"],Request["DATE"] );
                    report.SetDataSource(rpt_dt);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(o_A_KTYPE.RAD(Request["EXAD"]) + o_A_KTYPE.RUNIT(Request["EXUNIT"])  + int.Parse(Request["Date"].Substring(0, 3)) + "年" + Request["Date"].Substring(3, 2) + "月份輪值表");
                    CrystalReportViewer1.ReportSource = report;
                    
                    break;

                case "ONDUTY_LIST_NEW":
                    rpt_dt = RPT_C.C_ONTYDUTY_UnitList.doSearch_new( Request["EXAD"], Request["EXUNIT"],Request["DATE"] );
                    report.SetDataSource(rpt_dt);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(o_A_KTYPE.RAD(Request["EXAD"]) + o_A_KTYPE.RUNIT(Request["EXUNIT"])  + int.Parse(Request["Date"].Substring(0, 3)) + "年" + Request["Date"].Substring(3, 2) + "月份輪值表");
                    CrystalReportViewer1.ReportSource = report;
                    
                    break;

                case "ONDUTY_PAY_LIST":
                    rpt_dt = RPT_C.C_ONTYDUTY_UnitList_PAY.doSearch(Request["EXAD"], Request["EXUNIT"], Request["DATE"]);
                    report.SetDataSource(rpt_dt);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(o_A_KTYPE.RAD(Request["EXAD"]) + o_A_KTYPE.RUNIT(Request["EXUNIT"]) + int.Parse(Request["Date"].Substring(0, 3)) + "年" + Request["Date"].Substring(3, 2) + "月份值日費印領清冊");
                    CrystalReportViewer1.ReportSource = report;

                    break;
                // Joy 新增 值日報表更新
                case "ONDUTY_PAY_LIST_NEW":
                    rpt_dt = RPT_C.C_ONTYDUTY_UnitList_PAY.doSearch_New(Request["EXAD"], Request["EXUNIT"], Request["DATE"]);
                    report.SetDataSource(rpt_dt);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(o_A_KTYPE.RAD(Request["EXAD"]) + o_A_KTYPE.RUNIT(Request["EXUNIT"]) + int.Parse(Request["Date"].Substring(0, 3)) + "年" + Request["Date"].Substring(3, 2) + "月份值日費印領清冊");
                    CrystalReportViewer1.ReportSource = report;
                    
                    break;
            }
        }
    }
}
