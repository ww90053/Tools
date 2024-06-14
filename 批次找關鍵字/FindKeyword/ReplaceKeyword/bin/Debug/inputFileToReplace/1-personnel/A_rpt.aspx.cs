using System;
using System.Data;
using System.Linq;
using System.Web;
using TPPDDB.Helpers;

namespace TPPDDB._1_personnel
{
    public partial class A_rpt : System.Web.UI.Page
    {
        //string strSQL;
        DataTable rpt_dt = new DataTable();
        DataTable temp = new DataTable();
        DataTable temp1 = new DataTable();

        DataTable SOURCE = new DataTable();
        CrystalDecisions.CrystalReports.Engine.ReportDocument report;

        //報表登入
        string db_ServerName;
        string db_UserID;
        string db_Password;
        string db_DatabaseName;

        string fkind = "";
        string filename = "";
        //string filename2 = "";
        string filepath = "report/";

        protected void Page_Init(object sender, EventArgs e)
        {
            if (report != null)
            {
                report.Close();
                report.Dispose();
            }
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            report.Close();
            report.Dispose();

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            db_ServerName = System.Configuration.ConfigurationManager.AppSettings["servername"];
            db_UserID = System.Configuration.ConfigurationManager.AppSettings["userid"];
            db_Password = System.Configuration.ConfigurationManager.AppSettings["pass"];
            db_DatabaseName = System.Configuration.ConfigurationManager.AppSettings["database"];
            //if (db_DatabaseName == "")
            //{
            //    db_DatabaseName = "TPDBUSER";
            //}
            if (Request["fn"] != null)
            {
                fkind = Request["fn"].ToString();
            }

            switch (fkind)
            {
                case "onthejob"://在職證明
                    filename = "A_onthejob.rpt";
                    break;
                case "experience":
                    filename = "A_experience.rpt";
                    break;
                case "basic_all"://1.2.3全員基本資料名冊
                    filename = "A_basic.rpt";
                    break;

                #region 獎懲報表
                case "gradesuglist"://獎懲建議名冊
                    filename = "A_gradesuglist.rpt";
                    break;
                case "caserequest"://獎懲案件請示單
                    filename = "A_caserequest.rpt";
                    break;
                case "punish"://獎懲令
                case "punishODF"://獎懲令_產出ODF
                    filename = "A_punish.rpt";
                    break;
                case "punish1"://獎懲令(稿)
                case "punish1ODF"://獎懲令(稿)_產出ODF
                    filename = "A_punish1.rpt";
                    break;
                case "punishSug"://獎 懲 建 議 函
                    filename = "A_punishSug.rpt";
                    break;
                case "checklist"://獎懲核定名冊_依機關
                    filename = "A_checklist.rpt";
                    break;
                case "checklist_ODF"://獎懲核定名冊_依機關_產出ODF
                    filename = "A_checklist_odt.rpt";
                    break;
                case "checklist1"://獎懲核定名冊)_全部
                    filename = "A_checklist1.rpt";
                    break;
                case "gradedetail"://個人獎懲明細
                    filename = "A_gradedetail.rpt";
                    break;
                case "othergrade"://獎懲-調他機關名冊
                    filename = "A_othergrade.rpt";
                    break;
                case "gradeSum": //獎懲個案件統計表 
                    filename = "A_gradesum.rpt";
                    break;
                case "goodtobad"://功過相抵申誡以上報表
                    filename = "A_goodtobad.rpt";
                    break;
                case "gradenotice"://功過相抵申誡以上通知書
                    filename = "A_gradenotice.rpt";
                    break;
                case "yearofgradelist"://各機關年度獎懲統計表
                    filename = "A_yearofgradelist.rpt";
                    break;
                case "gradeSumList"://每月獎懲統計表
                    filename = "A_gradesumlist.rpt";
                    break;
                case "support"://陳報警政署獎懲建議名冊
                    filename = "A_support.rpt";
                    break;
                case "biggradelist": //記功過以上獎懲明細報表
                    filename = "A_biggradelist.rpt";
                    break;
                case "PrkGoodList"://獎勵統計表 
                    filename = "A_PrkGoodList.rpt";
                    break;
                case "PrkBadList":// 懲戒統計表                    
                    filename = "A_PrkBadList.rpt";
                    break;
                #endregion 獎懲報表

                case "aborginelist2":
                    filename = "A_aborginelist2.rpt";
                    break;
                case "aborginelist3":
                    filename = "A_aborginelist3.rpt";
                    break;
                case "basic_communications"://員警通訊名冊
                    filename = "A_communications.rpt";
                    break;
                case "check":
                    filename = "A_check.rpt";
                    break;
                case "Getjobnotify":
                    filename = "A_Getjobnotify.rpt";
                    break;
                case "quit":
                    filename = "A_quit.rpt";
                    break;
                case "quit1":
                    filename = "A_quit1.rpt";
                    break;
                case "posit":
                    filename = "A_posit.rpt";
                    break;
                case "posit1":
                    filename = "A_posit1.rpt";
                    break;
                case "posit_sug":
                    filename = "A_posit_sug.rpt";
                    break;
                case "basic1":
                    filename = "A_basic2.rpt";
                    break;
                case "Aborginelist":
                    filename = "A_aborginelist4.rpt";
                    break;
                case "Leavejob":
                    filename = "A_Leavejob.rpt";
                    break;
                case "Onjob":
                    filename = "A_Onjob.rpt";
                    break;
                case "changejob":
                    filename = "A_Changejob.rpt";
                    break;
                case "changejoblist":
                    filename = "A_Changejoblist.rpt";
                    break;
                case "posit_fromother":
                    filename = "A_posit_fromother.rpt";
                    break;
                case "posit_fromother1":
                    filename = "A_posit_fromother1.rpt";
                    break;
                #region 服務證
                case "Criminal_police"://服務證名冊--刑事警察
                    filename = "A_Criminal_police.rpt";
                    break;
                case "Normal_police"://服務證名冊--行政警察與一般行政
                    filename = "A_Normal_police.rpt";
                    break;
                case "Criminal_police_2"://服務證印領清冊--刑事警察
                    filename = "A_Criminal_police_2.rpt";
                    break;
                case "Normal_police_2"://服務證印領清冊--行政警察與一般行政
                    filename = "A_Normal_police_2.rpt";
                    break;
                #endregion 服務證
                case "Card_All_1"://舊的刑事
                    filename = "A_Card_All_1.rpt";
                    break;
                case "Card_All_Criminal_Horizontal"://服務證_刑事警察_整批_橫式
                    filename = "A_Card_All_Criminal_Horizontal.rpt";
                    break;
                case "Card_All_Criminal_Vertical"://服務證_刑事警察_整批_直式
                    filename = "A_Card_All_Criminal_Vertical.rpt";
                    break;
                //2013/05/15
                case "Card_All_Horizontal"://服務證_行政警察_整批_橫式
                    filename = "A_Card_All_Horizontal.rpt";
                    break;

                // case "Card_All":
                //filename = "A_Card_All.rpt";
                //break;
                case "Card_All_Vertical"://服務證_行政警察_整批_直式
                    filename = "A_Card_All_Vertical.rpt";
                    break;

                case "Card_One"://舊的行政
                    filename = "A_Card_One.rpt";
                    break;

                case "Card_One_1": //舊的刑事
                    filename = "A_Card_One_1.rpt";
                    break;
                case "Card_One_Criminal_Horizontal": //舊的刑事
                    filename = "A_Card_One_Criminal_Horizontal.rpt";
                    break;
                case "Card_One_Criminal_Vertical"://服務證_刑事警察_單批_直式
                    filename = "A_Card_One_Criminal_Vertical.rpt";
                    break;
                //2013/05/28
                case "Card_One_Horizontal"://服務證_行政警察_單批_橫式
                    filename = "A_Card_One_Horizontal.rpt";
                    break;
                case "Card_One_Vertical"://服務證_行政警察_單批_直式
                    filename = "A_Card_One_Vertical.rpt";
                    break;
                case "Effective":
                    filename = "A_Effective.rpt";
                    break;
                case "EffectiveAll":
                    filename = "A_EffectiveAll.rpt";
                    break;
                case "C_CARDHISTORY_edit": //因公疏未辦理按卡紀錄報告單
                    filename = "C_CARDHISTORY.rpt";
                    break;


            }

            report = new CrystalDecisions.CrystalReports.Engine.ReportDocument();

            report.Load(Server.MapPath(filepath + filename));

            CrystalDecisions.CrystalReports.Engine.ReportDocument report2 = new CrystalDecisions.CrystalReports.Engine.ReportDocument();

            report.SetDatabaseLogon(db_UserID, db_Password, db_ServerName, db_DatabaseName);

            //2013
            if (Session["RPT_SQL_A"] != null)
            {
                rpt_dt = o_DBFactory.ABC_toTest.Create_Table(Session["RPT_SQL_A"].ToString(), "getvalue");
            }

            switch (fkind)
            {
                case "onthejob"://在職證明

                    report.ParameterFields["MZ_ID"].CurrentValues.AddValue(Session["MZ_ID"].ToString().Trim());
                    report.ParameterFields["SMALL"].CurrentValues.AddValue(Session["SMALL"].ToString().Trim());
                    report.ParameterFields["BIG"].CurrentValues.AddValue(Session["BIG"].ToString());
                    report.ParameterFields["BOSS"].CurrentValues.AddValue(Session["BOSS"].ToString().Trim());
                    report.ParameterFields["BOSS1"].CurrentValues.AddValue(Session["BOSS1"].ToString().Trim());
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(Session["TITLE"].ToString().Trim());
                    CrystalReportViewer1.ReportSource = report;
                    break;
                case "experience":

                    report.ParameterFields["MZ_ID"].CurrentValues.AddValue(Session["MZ_ID"].ToString().Trim());
                    report.ParameterFields["SMALLDay"].CurrentValues.AddValue(Session["SMALLDay"].ToString().Trim());
                    report.ParameterFields["SMALLNid"].CurrentValues.AddValue(Session["SMALLNid"].ToString().Trim());
                    report.ParameterFields["OPENING"].CurrentValues.AddValue(Session["OPENING"].ToString().Trim());
                    report.ParameterFields["BIG"].CurrentValues.AddValue(Session["BIG"].ToString());
                    report.ParameterFields["BOSS"].CurrentValues.AddValue(Session["BOSS"].ToString().Trim());
                    report.ParameterFields["BOSS1"].CurrentValues.AddValue(Session["BOSS1"].ToString().Trim());
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(Session["TITLE"].ToString().Trim());
                    CrystalReportViewer1.ReportSource = report;
                    break;
                case "basic_all"://1.2.3全員基本資料名冊

                    rpt_dt = RPT.basic_all.doSearch(Request["AD"] == null ? "" : Request["AD"].ToString(), Request["UNIT"] == null ? "" : Request["UNIT"].ToString());

                    report.SetDataSource(rpt_dt);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(string.Format("{0}{1}基本資料名冊", o_A_KTYPE.RAD(Request["AD"] == null ? "" : Request["AD"].ToString()), o_A_KTYPE.RUNIT(Request["UNIT"] == null ? "" : Request["UNIT"].ToString())));
                    string now = string.Format("製表日期：{0}年{1}月{2}日", (int.Parse(DateTime.Now.Year.ToString()) - 1911).ToString(), DateTime.Now.Month.ToString().PadLeft(2), DateTime.Now.Day.ToString().PadLeft(2)); ;
                    report.ParameterFields["NOW"].CurrentValues.AddValue(now);
                    CrystalReportViewer1.ReportSource = report;
                    break;


                #region 獎懲報表

                #region 獎懲建議名冊

                case "gradesuglist":

                    DataTable gradesuglist = RPT.punish.A_gradesuglist.doSearch(Request["AD"] == null ? "" : Request["AD"], Request["UNIT"] == null ? "" : Request["UNIT"],
                        Request["SWT3"] == null ? "" : Request["SWT3"], Request["NO1"] == null ? "" : Request["NO1"], Request["NO2"] == null ? "" : Request["NO2"], Request["SORT"], Request["MUSER"] == null ? "" : Request["MUSER"]);
                    string gradesuglist_SUM = RPT.punish.A_gradesuglist.doSUM(Request["AD"] == null ? "" : Request["AD"], Request["UNIT"] == null ? "" : Request["UNIT"],
                        Request["SWT3"] == null ? "" : Request["SWT3"], Request["NO1"] == null ? "" : Request["NO1"], Request["NO2"] == null ? "" : Request["NO2"], gradesuglist, Request["MUSER"] == null ? "" : Request["MUSER"]);
                    report.SetDataSource(gradesuglist);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(string.Format("{0}獎懲建議名冊", o_A_KTYPE.RAD(Session["ADPMZ_EXAD"].ToString())));
                    report.ParameterFields["SUM"].CurrentValues.AddValue(gradesuglist_SUM.Trim());
                    CrystalReportViewer1.ReportSource = report;
                    break;
                #endregion 獎懲建議名冊

                #region 獎懲案件請示單

                case "caserequest":
                    DataTable caserequest = RPT.punish.A_caserequest.doSearch(Request["NO1"] == null ? "" : Request["NO1"], Request["NO2"] == null ? "" : Request["NO2"]);
                    report.SetDataSource(caserequest);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(string.Format("{0}警察人員獎懲案件請示單", o_A_KTYPE.RAD(Session["ADPMZ_EXAD"].ToString())).Trim());
                    CrystalReportViewer1.ReportSource = report;
                    break;

                #endregion 獎懲案件請示單

                #region 獎 懲 令
                case "punish":
                case "punishODF":
                    string punish_PRID = HttpUtility.UrlDecode(Request["PRID"]);
                    string punish_PRID1 = Request["PRID1"];

                    rpt_dt = RPT.punish.A_punish.doSearch(punish_PRID, punish_PRID1, Request["MZ_SRANK"]);


                    if (rpt_dt.Rows.Count == 0)
                    {
                        lb_tip.Text = " 本案無懲戒或一次記一大功以上資料可列印";
                    }

                    if (!string.IsNullOrEmpty(lb_tip.Text))
                    {
                        lb_tip.Visible = true;
                        return;

                    }

                    int punish_count = rpt_dt.Rows.Count;

                    //20150327
                    //string punish_CHKAD = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_CHKAD FROM A_PRK2 WHERE MZ_PRID='" + punish_PRID + "' AND MZ_PRID1='" + punish_PRID1 + "' AND ROWNUM=1");
                    string punish_CHKAD = "";

                    if (punish_PRID == "北警人" || punish_PRID == "新北警人")
                    {
                        punish_CHKAD = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_CHKAD FROM A_PRK2 WHERE MZ_PRID in ('北警人','新北警人') AND MZ_PRID1='" + punish_PRID1 + "' AND ROWNUM=1");
                    }
                    else
                    {
                        punish_CHKAD = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_CHKAD FROM A_PRK2 WHERE MZ_PRID='" + punish_PRID + "' AND MZ_PRID1='" + punish_PRID1 + "' AND ROWNUM=1");

                    }

                    SOURCE = RPT.punish.A_punish.doTable(punish_PRID, punish_PRID1, punish_CHKAD, rpt_dt);


                    rpt_dt.DefaultView.RowFilter = "MZ_SRANK IN ('P10','P11','P12','P13','P14','G31','G32','G33','G34') AND SUBSTRING(MZ_PRK,1,1)<>'A'";

                    temp1 = rpt_dt.DefaultView.ToTable();



                    string punish_memo = string.Empty;
                    string punish_memoString = "";

                    string punish_EXP1 = "";
                    string punish_EXP2 = "";
                    string punish_MAN = "";

                    #region 說明欄位排版
                    //20150327 
                    //string strSQL = string.Format("SELECT MZ_EXPLAIN,MZ_EXPLAIN0,MZ_EXPLAIN1 FROM A_PRK1 WHERE MZ_PRID='{0}' AND MZ_PRID1='{1}'", punish_PRID, punish_PRID1);

                    string strSQL = "";

                    if (punish_PRID == "北警人" || punish_PRID == "新北警人")
                    {
                        strSQL = string.Format("SELECT MZ_EXPLAIN,MZ_EXPLAIN0,MZ_EXPLAIN1 FROM A_PRK1 WHERE MZ_PRID IN ('北警人','新北警人') AND MZ_PRID1='{0}'", punish_PRID1);

                    }
                    else
                    {
                        strSQL = string.Format("SELECT MZ_EXPLAIN,MZ_EXPLAIN0,MZ_EXPLAIN1 FROM A_PRK1 WHERE MZ_PRID='{0}' AND MZ_PRID1='{1}'", punish_PRID, punish_PRID1);
                    }

                    temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");


                    if (temp.Rows.Count > 0)
                    {
                        string exp = temp.Rows[0]["MZ_EXPLAIN"].ToString();
                        punish_EXP1 = temp.Rows[0]["MZ_EXPLAIN0"].ToString();
                        string exp1 = (temp1.Rows.Count > 0 ? "監察院、" : "") + temp.Rows[0]["MZ_EXPLAIN1"].ToString();

                        if (exp.IndexOf("一、") > -1 || exp.IndexOf("1.") > -1)
                        {
                            string[] exps = exp.Split(new char[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
                            for (int i = 0; i < exps.Count(); i++)
                            {
                                punish_memo = exps[i];
                                punish_memo = o_CommonService.d_report_break_line(punish_memo, 62, 58, "&N　　");
                                punish_memoString += punish_memo;
                                punish_memoString += "&N";
                            }
                        }
                        else
                        {
                            punish_memoString = o_CommonService.d_report_break_line(exp, 62, 62, "&N");
                        }

                        punish_EXP2 = o_CommonService.d_report_break_line(exp1, 62, 62, "&N");
                    }
                    else
                    {
                        string exp1 = (temp1.Rows.Count > 0 ? "監察院、" : "") + "";

                        punish_EXP2 = o_CommonService.d_report_break_line(exp1, 62, 62, "&N");
                    }
                    #endregion 說明欄位排版

                    if (punish_PRID == "北警人" || punish_PRID == "新北警人")
                    {
                        strSQL = "SELECT MZ_MASTER_NAME,MZ_MASTER_POS FROM A_CHKAD WHERE MZ_PRID in ('北警人','新北警人')";
                    }
                    else
                    {

                        strSQL = string.Format("SELECT MZ_MASTER_NAME,MZ_MASTER_POS FROM A_CHKAD WHERE MZ_PRID='{0}'", punish_PRID);
                    }
                    DataTable dt2 = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
                    if (dt2.Rows.Count > 0)
                    {
                        punish_MAN = dt2.Rows[0][1].ToString() + "   " + dt2.Rows[0][0].ToString();
                    }
                    else
                    {
                        punish_MAN = string.Empty;
                    }


                    report.SetDataSource(SOURCE);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(string.Format("{0}   令", o_A_KTYPE.RAD(punish_CHKAD)));
                    report.ParameterFields["EXP"].CurrentValues.AddValue(punish_memoString.Trim());
                    report.ParameterFields["EXP1"].CurrentValues.AddValue(punish_EXP1.Trim());
                    report.ParameterFields["EXP2"].CurrentValues.AddValue(punish_EXP2.Trim());
                    report.ParameterFields["MAN"].CurrentValues.AddValue(punish_MAN.Trim());
                    report.ParameterFields["COUNTS"].CurrentValues.AddValue(punish_count);

                    //增加產生ODF格式檔案 20200210 by sky
                    if (fkind == "punishODF")
                    {
                        new OfficeHelpers.ODFHelpers().CrystalReportToOdt(report, string.Format("{0}   令_{1}", o_A_KTYPE.RAD(punish_CHKAD), DateTime.Now.ToString("yyyyMMdd")));
                    }
                    else
                    {
                        CrystalReportViewer1.ReportSource = report;
                    }

                    string updateString = "";
                    try
                    {//2013/12/02
                        if (!IsPostBack)
                        {
                            //因為原先2-2才有此動作.而報表沒有.保留原先設定
                            if (Request["DATAFROM"] == "2")
                            {
                                updateString = "UPDATE A_PRK2 SET MZ_SWT2='Y' WHERE MZ_PRID='" + rpt_dt.Rows[0]["MZ_PRID"].ToString() + "' AND MZ_PRID1='" + rpt_dt.Rows[0]["MZ_PRID1"].ToString() + "'";

                                o_DBFactory.ABC_toTest.Edit_Data(updateString);
                            }
                        }
                    }
                    catch
                    {

                    }

                    break;

                #endregion 獎 懲 令

                #region 獎 懲 令 (稿)
                case "punish1":
                case "punish1ODF":

                    string punish1_PRID = HttpUtility.UrlDecode(Request["PRID"]);
                    string punish1_PRID1 = Request["PRID1"];


                    rpt_dt = RPT.punish.A_punish1.doSearch(punish1_PRID, punish1_PRID1, Request["MZ_SRANK"]);


                    if (rpt_dt.Rows.Count == 0)
                    {
                        lb_tip.Text = " 無資料";
                    }

                    if (!string.IsNullOrEmpty(lb_tip.Text))
                    {
                        lb_tip.Visible = true;
                        return;

                    }

                    int punish1_count = rpt_dt.Rows.Count;
                    string punish1_CHKAD = o_DBFactory.ABC_toTest.vExecSQL("SELECT  MZ_CHKAD FROM A_PRK2 WHERE MZ_PRID='" + punish1_PRID + "' AND MZ_PRID1='" + punish1_PRID1 + "' AND ROWNUM=1");

                    SOURCE = RPT.punish.A_punish1.doTable(punish1_PRID, punish1_PRID1, punish1_CHKAD, rpt_dt);


                    string punish1_memo = string.Empty;
                    string punish1_memoString = "";

                    string punish1_EXP1 = "";
                    string punish1_EXP2 = "";
                    string punish1_MAN = "";
                    string punish1_CONN = "";

                    #region 說明欄位排版
                    strSQL = string.Format("SELECT MZ_EXPLAIN,MZ_EXPLAIN0,MZ_EXPLAIN1 FROM A_PRK1 WHERE MZ_PRID='{0}' AND MZ_PRID1='{1}'", punish1_PRID, punish1_PRID1);
                    temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");

                    if (temp.Rows.Count > 0)
                    {
                        string exp = temp.Rows[0]["MZ_EXPLAIN"].ToString();
                        punish1_EXP1 = temp.Rows[0]["MZ_EXPLAIN0"].ToString();
                        string exp1 = temp.Rows[0]["MZ_EXPLAIN1"].ToString();

                        if (exp.IndexOf("一、") > -1 || exp.IndexOf("1.") > -1)
                        {
                            string[] exps = exp.Split(new char[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
                            for (int i = 0; i < exps.Count(); i++)
                            {
                                //punish1_memo = exps[i] + "。";
                                punish1_memo = exps[i];
                                punish1_memo = o_CommonService.d_report_break_line(punish1_memo, 62, 58, "&N　　");
                                punish1_memoString += punish1_memo;
                                punish1_memoString += "&N";
                            }
                        }
                        else
                        {
                            punish1_memoString = o_CommonService.d_report_break_line(exp, 62, 62, "&N");
                        }


                        punish1_EXP2 = o_CommonService.d_report_break_line(exp1, 62, 62, "&N");
                    }
                    #endregion 說明欄位排版


                    strSQL = string.Format("SELECT AC.MZ_ADDRESS,ACC.MZ_ID,MZ_TELNO,MZ_EMAIL FROM A_CHKAD_CONTRACTORS ACC,A_CHKAD AC WHERE AC.MZ_AD = ACC.MZ_CHKAD AND ACC.MZ_ID='" + Session["ADPMZ_ID"].ToString() + "'");
                    temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");

                    if (temp.Rows.Count > 0)
                    {
                        punish1_CONN += "通訊地址：" + temp.Rows[0]["MZ_ADDRESS"].ToString() + "&N";
                        punish1_CONN += "聯絡方式：" + o_A_DLBASE.OCCC(temp.Rows[0]["MZ_ID"].ToString()) + o_A_DLBASE.CNAME(temp.Rows[0]["MZ_ID"].ToString()) + "警用" + temp.Rows[0]["MZ_TELNO"].ToString() + "&N";
                        punish1_CONN += "電子信箱：" + temp.Rows[0]["MZ_EMAIL"].ToString() + "&N";

                    }


                    strSQL = string.Format("SELECT MZ_MASTER_NAME,MZ_MASTER_POS FROM A_CHKAD WHERE MZ_PRID='{0}'", punish1_PRID);
                    temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");



                    if (temp.Rows.Count > 0)
                    {
                        string name = temp.Rows[0]["MZ_MASTER_NAME"].ToString();
                        string result_name = string.Empty;
                        string result_namemark = string.Empty;
                        for (int i = 0; i < name.Length; i++)
                        {
                            result_name += name.Substring(i, 1) + "  ";
                            if (i == 0)
                                result_namemark += name.Substring(i, 1) + "  ";
                            else
                                result_namemark += "○" + "  ";
                        }
                        punish1_MAN = temp.Rows[0][1].ToString() + "  " + result_namemark;
                    }


                    report.SetDataSource(SOURCE);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(string.Format("{0}   令(稿)", o_A_KTYPE.RAD(punish1_CHKAD)));
                    report.ParameterFields["EXP"].CurrentValues.AddValue(punish1_memoString.Trim());
                    report.ParameterFields["EXP1"].CurrentValues.AddValue(punish1_EXP1.Trim());
                    report.ParameterFields["EXP2"].CurrentValues.AddValue(punish1_EXP2.Trim());
                    report.ParameterFields["MAN"].CurrentValues.AddValue(punish1_MAN.Trim());
                    report.ParameterFields["COUNTS"].CurrentValues.AddValue(punish1_count.ToString().Trim());
                    report.ParameterFields["CONN"].CurrentValues.AddValue(punish1_CONN.Trim());

                    //增加產生ODF格式檔案 20200210 by sky
                    if (fkind == "punish1ODF")
                    {
                        new OfficeHelpers.ODFHelpers().CrystalReportToOdt(report, string.Format("{0}   令(稿)_{1}", o_A_KTYPE.RAD(punish1_CHKAD), DateTime.Now.ToString("yyyyMMdd")));
                    }
                    else
                    {
                        CrystalReportViewer1.ReportSource = report;
                    }

                    break;

                #endregion 獎 懲 令 (稿)

                #region  獎 懲 建 議 函
                case "punishSug":
                    string punishSug_PRID = HttpUtility.UrlDecode(Request["PRID"]);
                    string punishSug_PRID1 = Request["PRID1"];



                    rpt_dt = RPT.punish.A_punish1.doSearch(punishSug_PRID, punishSug_PRID1, Request["MZ_SRANK"]);


                    if (rpt_dt.Rows.Count == 0)
                    {
                        lb_tip.Text = " 無資料";
                    }

                    if (!string.IsNullOrEmpty(lb_tip.Text))
                    {
                        lb_tip.Visible = true;
                        return;

                    }

                    int punishSug_count = rpt_dt.Rows.Count;
                    string punishSug_CHKAD = o_DBFactory.ABC_toTest.vExecSQL("SELECT  MZ_CHKAD FROM A_PRK2 WHERE MZ_PRID='" + punishSug_PRID + "' AND MZ_PRID1='" + punishSug_PRID1 + "' AND ROWNUM=1");

                    SOURCE = RPT.punish.A_punishSug.doTable(punishSug_PRID, punishSug_PRID1, punishSug_CHKAD, rpt_dt);


                    string punishSug_memo = string.Empty;
                    string punishSug_memoString = "";

                    string punishSug_EXP1 = "";
                    string punishSug_EXP2 = "";
                    string punishSug_MAN = "";
                    string punishSug_CONN = "";



                    #region 說明欄位排版
                    strSQL = string.Format("SELECT MZ_EXPLAIN,MZ_EXPLAIN0,MZ_EXPLAIN1 FROM A_PRK1 WHERE MZ_PRID='{0}' AND MZ_PRID1='{1}'", punishSug_PRID.Trim(), punishSug_PRID1.Trim());
                    temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");

                    if (temp.Rows.Count > 0)
                    {
                        string exp = temp.Rows[0]["MZ_EXPLAIN"].ToString();
                        punish1_EXP1 = temp.Rows[0]["MZ_EXPLAIN0"].ToString();
                        string exp1 = temp.Rows[0]["MZ_EXPLAIN1"].ToString();

                        if (exp.IndexOf("一、") > -1 || exp.IndexOf("1.") > -1)
                        {
                            string[] exps = exp.Split(new char[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
                            for (int i = 0; i < exps.Count(); i++)
                            {
                                punishSug_memo = exps[i] + "。";
                                punishSug_memo = o_CommonService.d_report_break_line(punishSug_memo, 62, 58, "&N　　");
                                punishSug_memoString += punishSug_memo;
                                punishSug_memoString += "&N";
                            }
                        }
                        else
                        {
                            punishSug_memoString = o_CommonService.d_report_break_line(exp, 62, 62, "&N");
                        }


                        punishSug_EXP2 = o_CommonService.d_report_break_line(exp1, 62, 62, "&N");
                    }
                    #endregion 說明欄位排版


                    strSQL = string.Format("SELECT AC.MZ_ADDRESS,ACC.MZ_ID,MZ_TELNO,MZ_EMAIL FROM A_CHKAD_CONTRACTORS ACC,A_CHKAD AC WHERE AC.MZ_AD = ACC.MZ_CHKAD AND MZ_ID='" + Session["ADPMZ_ID"].ToString() + "'");
                    temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
                    if (temp.Rows.Count > 0)
                    {
                        punishSug_CONN += "通訊地址：" + temp.Rows[0]["MZ_ADDRESS"].ToString() + "&N";
                        punishSug_CONN += "聯絡方式：" + o_A_DLBASE.OCCC(temp.Rows[0]["MZ_ID"].ToString()) + o_A_DLBASE.CNAME(temp.Rows[0]["MZ_ID"].ToString()) + "警用" + temp.Rows[0]["MZ_TELNO"].ToString() + "&N";
                        punishSug_CONN += "電子信箱：" + temp.Rows[0]["MZ_EMAIL"].ToString() + "&N";

                    }


                    strSQL = string.Format("SELECT MZ_MASTER_NAME,MZ_MASTER_POS FROM A_CHKAD WHERE MZ_PRID='{0}'", punishSug_PRID.Trim());
                    temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
                    if (temp.Rows.Count > 0)
                    {
                        punishSug_MAN = temp.Rows[0][1].ToString() + "   " + temp.Rows[0][0].ToString().Substring(0, 1) + "○○";
                    }
                    else
                    {
                        punishSug_MAN = string.Empty;
                    }

                    report.SetDataSource(SOURCE);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(string.Format("{0}  獎懲建議函", o_A_KTYPE.RAD(punishSug_CHKAD)));
                    report.ParameterFields["EXP"].CurrentValues.AddValue(punishSug_memoString.Trim());
                    report.ParameterFields["EXP1"].CurrentValues.AddValue(punishSug_EXP1.Trim());
                    report.ParameterFields["EXP2"].CurrentValues.AddValue(punishSug_EXP2.Trim());
                    report.ParameterFields["MAN"].CurrentValues.AddValue(punishSug_MAN.Trim());
                    report.ParameterFields["COUNTS"].CurrentValues.AddValue(punishSug_count.ToString().Trim());
                    report.ParameterFields["CONN"].CurrentValues.AddValue(punishSug_CONN.Trim());
                    CrystalReportViewer1.ReportSource = report;



                    break;
                #endregion  獎 懲 建 議 函

                #region 獎懲核定名冊_依機關

                case "checklist"://_依機關
                case "checklist_ODF":

                    rpt_dt = RPT.punish.A_checklist.doSearch(Request["MZ_SWT3"], Request["NO"], Request["NO2"]);

                    //for (int i = 0; i < rpt_dt.Rows.Count; i++)
                    //{
                    //    rpt_dt.Rows[i]["MZ_PRCT"] = o_CommonService.d_report_break_line(rpt_dt.Rows[i]["MZ_PRCT"].ToString(), 32, "&N");
                    //}
                    report.SetDataSource(rpt_dt);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(string.Format("{0}獎懲核定名冊", o_A_KTYPE.RAD(Session["ADPMZ_EXAD"].ToString())));

                    //增加產生ODF格式檔案 20200210 by sky
                    if (fkind == "checklist_ODF")
                    {
                        new OfficeHelpers.ODFHelpers().CrystalReportToOdt(report, 
                            string.Format("{0}獎懲核定名冊_{1}"
                            , o_A_KTYPE.RAD(Session["ADPMZ_EXAD"].ToString())
                            , DateTime.Now.ToString("yyyyMMdd")));
                    }
                    else
                    {
                        CrystalReportViewer1.ReportSource = report;
                    }
                    break;
                #endregion 獎懲核定名冊_依機關

                #region 獎懲核定名冊_全部

                case "checklist1":
                    rpt_dt = RPT.punish.A_checklist.doSearch(Request["MZ_SWT3"], Request["NO"], Request["NO2"]);

                    //非空資料串時強制塞一個"以下空白"資料
                    if (rpt_dt != null && rpt_dt.Rows.Count > 0 && (rpt_dt.Rows.Count % 13) > 0)
                    {
                        DataRow dr = rpt_dt.NewRow();
                        dr["MZ_AD"] = "以下空白";
                        rpt_dt.Rows.Add(dr);
                    }
                    report.SetDataSource(rpt_dt);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(string.Format("{0}獎懲核定名冊", o_A_KTYPE.RAD(Session["ADPMZ_EXAD"].ToString())));
                    CrystalReportViewer1.ReportSource = report;
                    break;
                #endregion 獎懲核定名冊_全部

                #region 個人獎懲明細



                case "gradedetail":


                    string pgradedetail_NAME = HttpUtility.UrlDecode(Request["MZ_NAME"]);

                    rpt_dt = RPT.punish.A_gradedetail.doSearch(Session["RPT_A_IDNO"].ToString(), pgradedetail_NAME, Request["MZ_IDATE1"].ToString(), Request["MZ_IDATE2"].ToString());


                    if (rpt_dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < rpt_dt.Rows.Count; i++)
                        {
                            rpt_dt.Rows[i]["MZ_PRCT"] = o_CommonService.d_report_break_line(rpt_dt.Rows[i]["MZ_PRCT"].ToString(), 50, "&N");
                        }
                    }
                    report.SetDataSource(rpt_dt);
                    report.ParameterFields["TITLE1"].CurrentValues.AddValue(RPT.punish.A_gradedetail.COUNTDATE(Request["MZ_IDATE1"].ToString(), Request["MZ_IDATE2"].ToString()));
                    CrystalReportViewer1.ReportSource = report;
                    break;
                #endregion 個人獎懲明細

                #region 獎懲-調他機關名冊
                case "othergrade":
                    rpt_dt = RPT.punish.A_othergrade.doSearch(Request["MZ_SWT3"], Request["NO"], Request["NO2"]);

                    report.SetDataSource(rpt_dt);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(string.Format("{0}外調人員獎懲建議名冊", o_A_KTYPE.RAD(Session["ADPMZ_EXAD"].ToString())));
                    CrystalReportViewer1.ReportSource = report;
                    break;
                #endregion 獎懲-調他機關名冊

                case "gradeSum": //獎懲個案件統計表                 
                    report.SetDataSource(Session["rpt_dt"] as DataTable);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(Session["TITLE"].ToString().Trim());
                    report.ParameterFields["TOTAL"].CurrentValues.AddValue(Session["TOTAL"].ToString().Trim());
                    report.ParameterFields["PRID"].CurrentValues.AddValue(Session["PRID"].ToString().Trim());
                    report.ParameterFields["NO"].CurrentValues.AddValue(Session["NO"].ToString().Trim());
                    CrystalReportViewer1.ReportSource = report;
                    break;

                #region 功過相抵申誡以上報表
                case "goodtobad":


                    rpt_dt = RPT.punish.A_goodtobad.doSearch(Request["MZ_AD"], Request["MZ_DATE1"], Request["MZ_DATE2"]);

                    //2013/12/02                    
                    report.SetDataSource(rpt_dt);

                    report.ParameterFields["TITLE"].CurrentValues.AddValue(string.Format("{0}{1}年度功過相抵申誡以上報表", o_A_KTYPE.RAD(Request["MZ_AD"]), Request["MZ_DATE1"].ToString().Trim().Replace("/", string.Empty).PadLeft(7, '0').Substring(0, 3)));
                    CrystalReportViewer1.ReportSource = report;
                    break;

                #endregion 功過相抵申誡以上報表

                #region X功過相抵申誡以上通知書  鬼SQL

                case "gradenotice":
                    report.SetDataSource(rpt_dt);
                    report.ParameterFields["DATE"].CurrentValues.AddValue(Session["DATE"].ToString().Trim());
                    report.ParameterFields["PRID"].CurrentValues.AddValue(Session["PRID"].ToString().Trim());
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(Session["TITLE"].ToString().Trim());
                    report.ParameterFields["MEMO"].CurrentValues.AddValue(Session["MEMO"].ToString().Trim());
                    report.ParameterFields["DATE1"].CurrentValues.AddValue(Session["DATE1"].ToString().Trim());
                    CrystalReportViewer1.ReportSource = report;
                    break;
                #endregion X功過相抵申誡以上通知書

                #region 各機關年度獎懲統計表
                case "yearofgradelist":

                    rpt_dt = RPT.punish.A_yearofgradelist.doSearch(Request["MZ_AD"], Request["MZ_DATE1"], Request["MZ_DATE2"], 2);


                    report.SetDataSource(rpt_dt);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(string.Format("{0}{1}年度獎懲統計表", o_A_KTYPE.RAD(Request["MZ_AD"]), (DateTime.Now.Year - 1911).ToString()));
                    CrystalReportViewer1.ReportSource = report;
                    break;
                #endregion 各機關年度獎懲統計表

                case "gradeSumList"://每月獎懲統計表
                    report.SetDataSource(Session["rpt_dt"] as DataTable);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(Session["TITLE"].ToString().Trim());
                    CrystalReportViewer1.ReportSource = report;
                    break;

                #region X陳報警政署獎懲建議名冊

                case "support":
                    for (int i = 0; i < rpt_dt.Rows.Count; i++)
                    {
                        rpt_dt.Rows[i]["MZ_PRCT"] = o_CommonService.d_report_break_line(rpt_dt.Rows[i]["MZ_PRCT"].ToString(), 27, "&N");
                    }
                    report.SetDataSource(rpt_dt);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(Session["TITLE"].ToString().Trim());
                    CrystalReportViewer1.ReportSource = report;
                    break;
                #endregion X陳報警政署獎懲建議名冊

                #region 記功過以上獎懲明細報表

                case "biggradelist":
                    report.SetDataSource(RPT.punish.biggradelist.doSearch(Request["MZ_PRRST"], Request["DATE1"], Request["DATE2"]));
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(string.Format("{0}記功過以上獎懲明細表", o_A_KTYPE.RAD(Session["ADPMZ_EXAD"].ToString())));
                    CrystalReportViewer1.ReportSource = report;
                    break;

                #endregion X記功過以上獎懲明細報表

                case "PrkGoodList"://獎勵統計表                    
                    report.SetDataSource(Session["rpt_dt"] as DataTable);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(Session["TITLE"].ToString().Trim());
                    CrystalReportViewer1.ReportSource = report;
                    break;
                case "PrkBadList"://懲戒統計表                     
                    report.SetDataSource(Session["rpt_dt"] as DataTable);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(Session["TITLE"].ToString().Trim());
                    CrystalReportViewer1.ReportSource = report;
                    break;

                #endregion 獎懲報表

                case "aborginelist2":

                    report.SetDataSource(Session["rpt_dt"] as DataTable);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(Session["TITLE"].ToString().Trim());
                    CrystalReportViewer1.ReportSource = report;
                    break;
                case "aborginelist3":

                    report.ParameterFields["MZ_EXAD"].CurrentValues.AddValue(Session["MZ_EXAD"].ToString().Trim());
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(Session["TITLE"].ToString().Trim());
                    CrystalReportViewer1.ReportSource = report;
                    break;
                case "basic_communications"://員警通訊名冊

                    //TODO 這要改
                    foreach (DataRow dr in rpt_dt.Rows)
                    {
                        dr["MZ_CAD"] = o_A_KTYPE.RAD(dr["MZ_AD"].ToString());
                        dr["MZ_ADD2"] = o_CommonService.d_report_break_line(dr["MZ_ADD2"].ToString(), 28, "&N");
                    }


                    report.SetDataSource(rpt_dt);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(Session["TITLE"].ToString().Trim());
                    CrystalReportViewer1.ReportSource = report;
                    break;
                case "check":

                    report.SetDataSource(Session["rpt_dt"] as DataTable);
                    CrystalReportViewer1.ReportSource = report;
                    break;
                case "Getjobnotify":

                    report.SetDataSource(Session["rpt_dt"] as DataTable);
                    CrystalReportViewer1.ReportSource = report;
                    break;
                case "quit":

                    report.SetDataSource(Session["rpt_dt"] as DataTable);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(Session["TITLE"].ToString().Trim());
                    CrystalReportViewer1.ReportSource = report;
                    break;
                case "quit1":

                    report.SetDataSource(Session["rpt_dt"] as DataTable);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(Session["TITLE"].ToString().Trim());
                    report.ParameterFields["副機關首長"].CurrentValues.AddValue(Session["TITLE2"].ToString().Trim());
                    report.ParameterFields["機關首長"].CurrentValues.AddValue(Session["TITLE1"].ToString().Trim());
                    CrystalReportViewer1.ReportSource = report;
                    break;
                case "posit":

                    report.SetDataSource(Session["rpt_dt"] as DataTable);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(Session["TITLE"].ToString().Trim());
                    report.ParameterFields["EXP"].CurrentValues.AddValue(Session["EXP"].ToString().Trim());
                    report.ParameterFields["EXP1"].CurrentValues.AddValue(Session["EXP1"].ToString().Trim());
                    report.ParameterFields["EXP2"].CurrentValues.AddValue(Session["EXP2"].ToString().Trim());
                    report.ParameterFields["MAN"].CurrentValues.AddValue(Session["MAN"].ToString().Trim());
                    report.ParameterFields["CONN"].CurrentValues.AddValue(Session["CONN"].ToString().Trim());
                    CrystalReportViewer1.ReportSource = report;
                    break;
                case "posit1":

                    report.SetDataSource(Session["rpt_dt"] as DataTable);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(Session["TITLE"].ToString().Trim());
                    report.ParameterFields["EXP"].CurrentValues.AddValue(Session["EXP"].ToString().Trim());
                    report.ParameterFields["EXP1"].CurrentValues.AddValue(Session["EXP1"].ToString().Trim());
                    report.ParameterFields["EXP2"].CurrentValues.AddValue(Session["EXP2"].ToString().Trim());
                    report.ParameterFields["MAN"].CurrentValues.AddValue(Session["MAN"].ToString().Trim());
                    report.ParameterFields["CONN"].CurrentValues.AddValue(Session["CONN"].ToString().Trim());
                    CrystalReportViewer1.ReportSource = report;
                    break;
                case "posit_sug":

                    report.SetDataSource(Session["rpt_dt"] as DataTable);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(Session["TITLE"].ToString().Trim());
                    report.ParameterFields["EXP"].CurrentValues.AddValue(Session["EXP"].ToString().Trim());
                    report.ParameterFields["EXP1"].CurrentValues.AddValue(Session["EXP1"].ToString().Trim());
                    report.ParameterFields["EXP2"].CurrentValues.AddValue(Session["EXP2"].ToString().Trim());
                    report.ParameterFields["MAN"].CurrentValues.AddValue(Session["MAN"].ToString().Trim());
                    report.ParameterFields["CONN"].CurrentValues.AddValue(Session["CONN"].ToString().Trim());
                    CrystalReportViewer1.ReportSource = report;
                    break;
                case "basic1":

                    report.SetDataSource(Session["rpt_dt"] as DataTable);
                    report.ParameterFields["title_ad"].CurrentValues.AddValue(Session["title_ad"].ToString().Trim());
                    report.ParameterFields["title_unit"].CurrentValues.AddValue(Session["title_unit"].ToString().Trim());
                    CrystalReportViewer1.ReportSource = report;
                    break;
                case "Aborginelist":
                    report.SetDataSource(Session["rpt_dt"] as DataTable);
                    CrystalReportViewer1.ReportSource = report;
                    break;
                case "Leavejob":
                    report.SetDataSource(Session["rpt_dt"] as DataTable);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(Session["TITLE"].ToString().Trim());
                    CrystalReportViewer1.ReportSource = report;
                    break;
                case "Onjob":

                    report.SetDataSource(Session["rpt_dt"] as DataTable);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(Session["TITLE"].ToString().Trim());
                    CrystalReportViewer1.ReportSource = report;
                    break;
                case "changejob":

                    report.SetDataSource(Session["rpt_dt"] as DataTable);
                    CrystalReportViewer1.ReportSource = report;
                    break;
                case "changejoblist":

                    report.SetDataSource(Session["rpt_dt"] as DataTable);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(Session["TITLE"].ToString().Trim());
                    CrystalReportViewer1.ReportSource = report;
                    break;
                case "posit_fromother":

                    report.SetDataSource(Session["rpt_dt"] as DataTable);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(Session["TITLE"].ToString().Trim());
                    report.ParameterFields["EXPLAN1"].CurrentValues.AddValue(Session["EXPLAN1"].ToString().Trim());
                    CrystalReportViewer1.ReportSource = report;
                    break;
                case "posit_fromother1":

                    report.SetDataSource(Session["rpt_dt"] as DataTable);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(Session["TITLE"].ToString().Trim());
                    report.ParameterFields["EXPLAN1"].CurrentValues.AddValue(Session["EXPLAN1"].ToString().Trim());
                    CrystalReportViewer1.ReportSource = report;
                    break;

                #region 服務證
                case "Criminal_police"://服務證名冊--刑事警察

                    for (int i = 0; i < rpt_dt.Rows.Count; i++)
                    {
                        rpt_dt.Rows[i]["MZ_NAME"] = o_A_DLBASE.CNAME(rpt_dt.Rows[i]["MZ_ID"].ToString());
                        rpt_dt.Rows[i]["MZ_UNIT"] = o_A_DLBASE.CMZUNIT(rpt_dt.Rows[i]["MZ_ID"].ToString());
                    }

                    report.SetDataSource(rpt_dt);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(Session["TITLE"].ToString().Trim());
                    CrystalReportViewer1.ReportSource = report;
                    break;
                case "Normal_police"://服務證名冊--行政警察與一般行政
                    for (int i = 0; i < rpt_dt.Rows.Count; i++)
                    {
                        rpt_dt.Rows[i]["MZ_NAME"] = o_A_DLBASE.CNAME(rpt_dt.Rows[i]["MZ_ID"].ToString());
                        rpt_dt.Rows[i]["MZ_UNIT"] = o_A_DLBASE.CMZUNIT(rpt_dt.Rows[i]["MZ_ID"].ToString());
                    }
                    report.SetDataSource(rpt_dt);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(Session["TITLE"].ToString().Trim());
                    CrystalReportViewer1.ReportSource = report;
                    break;

                case "Criminal_police_2"://服務證印領清冊--刑事警察
                    SOURCE = RPT.Criminal_police_2.doSearch(rpt_dt);
                    report.SetDataSource(SOURCE);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(Session["TITLE"].ToString().Trim());
                    CrystalReportViewer1.ReportSource = report;
                    break;

                case "Normal_police_2"://服務證印領清冊--行政警察與一般行政
                    SOURCE = RPT.Normal_police_2.doSearch(rpt_dt);
                    report.SetDataSource(SOURCE);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(Session["TITLE"].ToString().Trim());
                    CrystalReportViewer1.ReportSource = report;
                    break;

                #endregion 服務證

                case "Card_One_Horizontal"://服務證_行政警察_單張_橫式
                    report.SetDataSource(Session["rpt_dt"] as DataTable);
                    CrystalReportViewer1.ReportSource = report;
                    break;
                case "Card_One_Vertical"://服務證_行政警察_單張_直式


                    report.SetDataSource(Session["rpt_dt"] as DataTable);
                    CrystalReportViewer1.ReportSource = report;
                    break;
                //服務證_刑事警察_單批_直式
                case "Card_One_Criminal_Vertical":

                    report.SetDataSource(Session["rpt_dt"] as DataTable);
                    CrystalReportViewer1.ReportSource = report;
                    break;

                case "Card_One_Criminal_Horizontal"://服務證_刑事警察_單張_橫式

                    report.SetDataSource(Session["rpt_dt"] as DataTable);
                    CrystalReportViewer1.ReportSource = report;
                    break;

                //case "Card_One"://舊的行政服務證_行政警察_單張_直式
                //    report.SetDatabaseLogon(db_UserID, db_Password, db_ServerName, db_DatabaseName);
                //    //if (Session["rpt_dt"] != null)
                //    //{
                //    //    dt = o_DBFactory.ABC_toTest.report_cache(Session["rpt_dt"] as DataTable, "rpt_dt");
                //    //    Session.Remove("rpt_dt");
                //    //}
                //    //else
                //    //{
                //    //    dt = (DataTable)HttpContext.Current.Cache["rpt_dt"];
                //    //}
                //    report.SetDataSource(Session["rpt_dt"] as DataTable);
                //    CrystalReportViewer1.ReportSource = report;
                //    break;
                //case "Card_All_1"://舊的刑事
                //    report.SetDatabaseLogon(db_UserID, db_Password, db_ServerName, db_DatabaseName);
                //    //if (Session["rpt_dt"] != null)
                //    //{
                //    //    dt = o_DBFactory.ABC_toTest.report_cache(Session["rpt_dt"] as DataTable, "rpt_dt");
                //    //    Session.Remove("rpt_dt");
                //    //}
                //    //else
                //    //{
                //    //    dt = (DataTable)HttpContext.Current.Cache["rpt_dt"];
                //    //}
                //    report.SetDataSource(Session["rpt_dt"] as DataTable);
                //    CrystalReportViewer1.ReportSource = report;
                //    break;

                //2013/05/15 立廷
                case "Card_All_Horizontal"://服務證_行政警察_整批_橫式

                    report.SetDataSource(Session["rpt_dt"] as DataTable);
                    CrystalReportViewer1.ReportSource = report;
                    break;

                //case "Card_All":
                case "Card_All_Vertical"://服務證_行政警察_整批_直式

                    report.SetDataSource(Session["rpt_dt"] as DataTable);
                    CrystalReportViewer1.ReportSource = report;
                    break;

                case "Card_All_Criminal_Horizontal"://服務證_刑事警察_整批_橫式

                    report.SetDataSource(Session["rpt_dt"] as DataTable);
                    CrystalReportViewer1.ReportSource = report;
                    break;
                case "Card_All_Criminal_Vertical"://服務證_刑事警察_整批_直式

                    report.SetDataSource(Session["rpt_dt"] as DataTable);
                    CrystalReportViewer1.ReportSource = report;
                    break;


                //case "Card_One_1":
                //    report.SetDatabaseLogon(db_UserID, db_Password, db_ServerName, db_DatabaseName);
                //    report.SetDataSource(Session["rpt_dt"] as DataTable);
                //    CrystalReportViewer1.ReportSource = report;
                //    break;
                case "Effective":

                    report.SetDataSource(Session["rpt_dt"] as DataTable);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(Session["TITLE"].ToString().Trim());
                    CrystalReportViewer1.ReportSource = report;
                    break;
                case "EffectiveAll":

                    report.SetDataSource(Session["rpt_dt"] as DataTable);
                    report.ParameterFields["TITLE"].CurrentValues.AddValue(Session["TITLE"].ToString().Trim());
                    CrystalReportViewer1.ReportSource = report;
                    break;
                case "C_CARDHISTORY_edit": // 因公疏未辦理按卡紀錄報告單
                                           //report.SetDataSource(Session["rpt_dt"] as DataTable);
                                           //report.ParameterFields["TITLE"].CurrentValues.AddValue(Session["TITLE"].ToString().Trim());
                                           //CrystalReportViewer1.ReportSource = report;

                    report.ParameterFields["SN"].CurrentValues.AddValue(Session["C_CARDHISTORY_SN"].ToString().Trim());
                    //report.ParameterFields["TITLE"].CurrentValues.AddValue(Session["TITLE"].ToString().Trim());
                    CrystalReportViewer1.ReportSource = report;

                    break;

            }

            //report.VerifyDatabase();
        }
    }
}