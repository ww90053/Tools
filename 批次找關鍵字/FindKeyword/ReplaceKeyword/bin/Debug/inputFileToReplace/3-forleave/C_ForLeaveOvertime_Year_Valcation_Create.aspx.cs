using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Data.SqlClient;
using TPPDDB.Helpers;

namespace TPPDDB._3_forleave
{
    public partial class C_ForLeaveOvertime_Year_Valcation_Create : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {


            if (!IsPostBack)
            {
                C.check_power();
                //MQ-----------------------20100331
                C.set_Panel_EnterToTAB(ref this.Panel1);

                TextBox_MZ_YEAR.Text = (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0');

                lblErrorMsg.Text = string.Empty;

                switch (o_a_Function.strGID(Session["ADPMZ_ID"].ToString()))
                {
                    case "A":
                        break;
                    default:
                        RadioButtonList1.Items[0].Enabled = false;
                        RadioButtonList1.Items[1].Enabled = false;
                        break;
                }
            }
        }

        /// <summary>
        /// 按鈕：計算年度休假日數
        /// 調整成共用函數 20190124 by sky
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button1_Click(object sender, EventArgs e)
        {
            C_ForLeave_YearVacation_Create.ComputeModel computeModel = new C_ForLeave_YearVacation_Create.ComputeModel();

            computeModel.StatisticsYear = TextBox_MZ_YEAR.Text;
            if (RadioButtonList1.SelectedValue == "新增全部人員")
            {
                computeModel.InsertData = true;
            }
            else if (RadioButtonList1.SelectedValue == "單人")
            {
                if (!string.IsNullOrEmpty(TextBox_MZ_ID.Text))
                {
                    computeModel.MZ_ID = TextBox_MZ_ID.Text;
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請輸入身分證號！')", true);
                    return;
                }
            }
            if (Request.QueryString["TPM_FION"] != null)
            {
                computeModel.TPM_FION = Request.QueryString["TPM_FION"].ToString();
            }

            string errorMsg = string.Empty;
            C_ForLeave_YearVacation_Create.YearVacation(computeModel, ref errorMsg);

            if (!string.IsNullOrEmpty(errorMsg))
            {
                //lblErrorMsg.Text = errorMsg.Replace("\n", "<br/>");
                errorMsg = errorMsg.Replace("\n", " ");
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('"+ errorMsg + "')", true);
            }
            else
            {
                //lblErrorMsg.Text = "計算完成！";
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('計算完成！')", true);
            }
        }

        #region 舊按鈕功能，因統一全系統計算，該程式棄置 20190124 by sky
        //protected void Button1_Click_(object sender, EventArgs e)
        //{
        //    //20140108
        //    //第1年                          0天
        //    // <3年                          7天
        //    //  3<=?< 6  (就是小於第7年)    14天
        //    //  7<=?< 9  (就是小於第10年)   21天
        //    // 10<=?<13  (就是小於第14年)   28天
        //    // 14<=?<15  (14年以上)         30天
        //    //可是有人反應15年才有30天 待確認

        //    //原初任職日改為合格實授日 20181120 by sky


        //    #region  全部人員
        //    if (RadioButtonList1.SelectedValue == "全部人員")
        //    {
        //        // string strSQL = "SELECT MZ_FDATE,MZ_TYEAR,MZ_TMONTH,A_DLBASE.MZ_ID,MZ_SDAY3,MZ_SDAY2 FROM A_DLBASE,C_DLTBB WHERE  A_DLBASE.MZ_ID = C_DLTBB.MZ_ID AND MZ_YEAR='" + (int.Parse (TextBox_MZ_YEAR.Text) -1 ).ToString ().PadLeft (3,'0')  + "'";

        //        //原初任職日改為合格實授日 20181120 by sky
        //        //string strSQL = "SELECT MZ_FDATE,MZ_TYEAR,MZ_TMONTH,MZ_RYEAR,MZ_RMONTH,MZ_ID,'' MZ_SDAY3,'' MZ_SDAY2,'' MZ_SDAY FROM A_DLBASE WHERE 1=1 AND MZ_STATUS2='Y' ";
        //        string strSQL = "SELECT MZ_QUA_DATE ,MZ_TYEAR,MZ_TMONTH,MZ_RYEAR,MZ_RMONTH,MZ_ID,'' MZ_SDAY3,'' MZ_SDAY2,'' MZ_SDAY FROM A_DLBASE WHERE 1=1 AND MZ_STATUS2='Y' ";

        //        DataTable dt = new DataTable();
        //        dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");

        //        string pday = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_DAY FROM C_DLCODE_SETDAY WHERE MZ_CODE = (SELECT MZ_CODE FROM C_DLCODE WHERE MZ_CNAME = '事假')");
        //        string sickday = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_DAY FROM C_DLCODE_SETDAY WHERE MZ_CODE = (SELECT MZ_CODE FROM C_DLCODE WHERE MZ_CNAME = '病假')");
        //        string hcareday = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_DAY FROM C_DLCODE_SETDAY WHERE MZ_CODE = (SELECT MZ_CODE FROM C_DLCODE WHERE MZ_CNAME = '家庭照顧')");

        //        pday = string.IsNullOrEmpty(pday) ? "0" : pday;

        //        sickday = string.IsNullOrEmpty(sickday) ? "0" : sickday;

        //        hcareday = string.IsNullOrEmpty(hcareday) ? "0" : hcareday;

        //        int sday3;

        //        string MZ_YEAR = TextBox_MZ_YEAR.Text.PadLeft(3, '0');

        //        string ERRORFDATE_ID = "";

        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            int tyear = int.Parse(string.IsNullOrEmpty(dt.Rows[i]["MZ_TYEAR"].ToString()) ? "0" : dt.Rows[i]["MZ_TYEAR"].ToString());

        //            int tmonth = int.Parse(string.IsNullOrEmpty(dt.Rows[i]["MZ_TMONTH"].ToString()) ? "0" : dt.Rows[i]["MZ_TMONTH"].ToString());

        //            int ryear = int.Parse(string.IsNullOrEmpty(dt.Rows[i]["MZ_RYEAR"].ToString()) ? "0" : dt.Rows[i]["MZ_RYEAR"].ToString());

        //            int rmonth = int.Parse(string.IsNullOrEmpty(dt.Rows[i]["MZ_RMONTH"].ToString()) ? "0" : dt.Rows[i]["MZ_RMONTH"].ToString());

        //            DateTime FDATE;

        //            string fdate = dt.Rows[i]["MZ_ID"].ToString();

        //            //刪除已計算之資料
        //            string DeleteString = "DELETE FROM C_DLTBB WHERE MZ_YEAR='" + TextBox_MZ_YEAR.Text + "' AND MZ_ID='" + fdate + "'";
        //            o_DBFactory.ABC_toTest.Edit_Data(DeleteString);

        //            if (string.IsNullOrEmpty(dt.Rows[i]["MZ_QUA_DATE"].ToString()))
        //            {
        //                ERRORFDATE_ID += dt.Rows[i]["MZ_ID"].ToString();
        //                continue;
        //            }

        //            try
        //            {
        //                FDATE = DateTime.Parse((int.Parse(dt.Rows[i]["MZ_QUA_DATE"].ToString().Substring(0, 3)) + 1911).ToString() + "-" + dt.Rows[i]["MZ_QUA_DATE"].ToString().Substring(3, 2) + "-01");
        //            }
        //            catch
        //            {
        //                FDATE = DateTime.Now;
        //            }

        //            //if (FDATE.Year == 2009 && FDATE.Month == 10)
        //            //{
        //            //    FDATE = FDATE;
        //            //}

        //            int monthDiff = Convert.ToInt32(Microsoft.VisualBasic.DateAndTime.DateDiff("M", FDATE, DateTime.Parse((int.Parse(TextBox_MZ_YEAR.Text.Trim()) + 1911).ToString() + "-01-01"), Microsoft.VisualBasic.FirstDayOfWeek.Sunday, Microsoft.VisualBasic.FirstWeekOfYear.FirstFullWeek));

        //            int MONTH = monthDiff;

        //            int sday3_hour = 0;

        //            int addMONTH = tyear * 12 + tmonth - ryear * 12 - rmonth;

        //            updateBasicOff(FDATE, dt.Rows[i]["MZ_ID"].ToString(), addMONTH);

        //            //20140114   扣除中間留職停薪的月份 立廷

        //            strSQL = "SELECT EDATE,SDATE FROM C_LVHISTORY WHERE MZ_ID=@MZ_ID AND BACK=1 ORDER BY EDATE";
        //            SqlParameter[] parameterList1 ={
        //            new SqlParameter("MZ_ID",SqlDbType.VarChar){Value=fdate},
        //            };
        //            DataTable stay_job = o_DBFactory.ABC_toTest.ExecuteDataset( strSQL, parameterList1).Tables[0];
        //            if (stay_job.Rows.Count > 0)
        //            {
        //                for (int k = 0; k < stay_job.Rows.Count; k++)
        //                {
        //                    TimeSpan month = DateTime.Parse(stay_job.Rows[k][0].ToString()) - DateTime.Parse(stay_job.Rows[k][1].ToString());
        //                    MONTH = MONTH - (month.Days / 30);

        //                }

        //                //TimeSpan month = DateTime.Parse(stay_job.Rows[stay_job.Rows.Count - 1][0].ToString()) - DateTime.Parse(stay_job.Rows[stay_job.Rows.Count - 1][1].ToString());
        //                //MONTH = MONTH - (month.Days / 30);

        //            }
        //            //20140114 


        //            if (MONTH < 0)
        //            {
        //                sday3 = 0;
        //            }
        //            else
        //            {
        //                if (MONTH < 12)
        //                {
        //                    //if (addMONTH == 0 && FDATE.Month == 1)
        //                    //{
        //                    //    sday3 = 7;
        //                    //}
        //                    //else
        //                    //{
        //                    if (addMONTH == 0)
        //                    {
        //                        double countsay = MathHelper.Round(7 * double.Parse(MONTH.ToString()) / 12, 1);

        //                        string[] s = countsay.ToString().Split('.');

        //                        sday3 = int.Parse(s[0]);

        //                        if (s.Length == 2)
        //                        {
        //                            if (int.Parse(s[1]) > 5)
        //                            {
        //                                sday3 = sday3 + 1;
        //                            }
        //                            else if (int.Parse(s[1]) > 0)
        //                            {
        //                                sday3_hour = 4;
        //                            }
        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (addMONTH + MONTH < 36)
        //                        {
        //                            double countsay = MathHelper.Round(7 * double.Parse(MONTH.ToString()) / 12, 1);

        //                            string[] s = countsay.ToString().Split('.');

        //                            sday3 = int.Parse(s[0]);
        //                            if (s.Length == 2)
        //                            {
        //                                if (int.Parse(s[1]) > 5)
        //                                {
        //                                    sday3 = sday3 + 1;
        //                                }
        //                                else if (int.Parse(s[1]) > 0)
        //                                {
        //                                    sday3_hour = 4;
        //                                }
        //                            }
        //                        }
        //                        else if (36 <= addMONTH + MONTH && addMONTH + MONTH < 72)
        //                        {
        //                            double countsay = MathHelper.Round(14 * double.Parse(MONTH.ToString()) / 12, 1);

        //                            string[] s = countsay.ToString().Split('.');

        //                            sday3 = int.Parse(s[0]);
        //                            if (s.Length == 2)
        //                            {
        //                                if (int.Parse(s[1]) > 5)
        //                                {
        //                                    sday3 = sday3 + 1;
        //                                }
        //                                else if (int.Parse(s[1]) > 0)
        //                                {
        //                                    sday3_hour = 4;
        //                                }
        //                            }
        //                        }
        //                        else if (72 <= addMONTH + MONTH && addMONTH + MONTH < 108)
        //                        {
        //                            double countsay = MathHelper.Round(21 * double.Parse(MONTH.ToString()) / 12, 1);

        //                            string[] s = countsay.ToString().Split('.');

        //                            sday3 = int.Parse(s[0]);

        //                            if (s.Length == 2)
        //                            {
        //                                if (int.Parse(s[1]) > 5)
        //                                {
        //                                    sday3 = sday3 + 1;
        //                                }
        //                                else if (int.Parse(s[1]) > 0)
        //                                {
        //                                    sday3_hour = 4;
        //                                }
        //                            }
        //                        }
        //                        else if (108 <= addMONTH + MONTH && addMONTH + MONTH < 168)
        //                        {
        //                            double countsay = MathHelper.Round(28 * double.Parse(MONTH.ToString()) / 12, 1);

        //                            string[] s = countsay.ToString().Split('.');

        //                            sday3 = int.Parse(s[0]);

        //                            if (s.Length == 2)
        //                            {
        //                                if (int.Parse(s[1]) > 5)
        //                                {
        //                                    sday3 = sday3 + 1;
        //                                }
        //                                else if (int.Parse(s[1]) > 0)
        //                                {
        //                                    sday3_hour = 4;
        //                                }
        //                            }
        //                        }
        //                        else
        //                        {
        //                            double countsay = MathHelper.Round(30 * double.Parse(MONTH.ToString()) / 12, 1);

        //                            string[] s = countsay.ToString().Split('.');

        //                            sday3 = int.Parse(s[0]);

        //                            if (s.Length == 2)
        //                            {
        //                                if (int.Parse(s[1]) > 5)
        //                                {
        //                                    sday3 = sday3 + 1;
        //                                }
        //                                else if (int.Parse(s[1]) > 0)
        //                                {
        //                                    sday3_hour = 4;
        //                                }
        //                            }
        //                        }
        //                    }
        //                    //}
        //                }
        //                else if (12 <= MONTH + addMONTH && MONTH + addMONTH < 36)
        //                {
        //                    sday3 = 7;
        //                }
        //                else if (36 <= MONTH + addMONTH && MONTH + addMONTH < 72)
        //                {
        //                    sday3 = 14;
        //                }
        //                else if (72 <= MONTH + addMONTH && MONTH + addMONTH < 108)
        //                {
        //                    sday3 = 21;
        //                }
        //                else if (108 <= MONTH + addMONTH && MONTH + addMONTH < 168)
        //                {
        //                    sday3 = 28;
        //                }
        //                else
        //                {
        //                    sday3 = 30;
        //                }
        //            }

        //            #region 留職停薪 2012/11/29 irk 新增
        //            //strSQL = "SELECT EDATE,SDATE FROM C_LVHISTORY WHERE MZ_ID=@MZ_ID AND BACK=1 ORDER BY SN DESC";
        //            //SqlParameter[] parameterList1 ={
        //            //new SqlParameter("MZ_ID",SqlDbType.VarChar){Value=fdate},
        //            //};
        //            //DataTable temp = o_DBFactory.ABC_toTest.ExecuteDataset( strSQL, parameterList1).Tables[0];
        //            if (stay_job.Rows.Count > 0)
        //            {
        //                double rday = 0;
        //                DateTime sdate = DateTime.Parse(stay_job.Rows[stay_job.Rows.Count - 1][1].ToString()).AddYears(1);
        //                //sdate = new DateTime(sdate.Year, sdate.Month, 1);
        //                DateTime edate = DateTime.Parse(stay_job.Rows[stay_job.Rows.Count - 1][0].ToString()).AddYears(1);
        //                //edate = new DateTime(edate.Year, edate.Month, 1); 

        //                //20150327
        //                if (int.Parse(MZ_YEAR) + 1 == edate.Year - 1911)
        //                {
        //                    sdate = sdate.AddYears(-1);
        //                    edate = edate.AddYears(-1);
        //                }

        //                if (int.Parse(MZ_YEAR) != edate.Year - 1911)///有跨年度留職停薪
        //                {
        //                    //2015 104 103 
        //                    if ((int.Parse(MZ_YEAR) >= (sdate.Year - 1911)) && (int.Parse(MZ_YEAR) <= (edate.Year - 1911)))
        //                    {
        //                        sday3 = 0;
        //                        sday3_hour = 0;
        //                    }
        //                }
        //                else if (int.Parse(MZ_YEAR) == edate.Year - 1911 && int.Parse(MZ_YEAR) == sdate.Year - 1911)//當年請.當年度復職
        //                {
        //                    // 20140107 韓毅 簡單修正留職停薪假期問題
        //                    int subMonth = 0;

        //                    //20140502

        //                    ////由天數去計
        //                    //double TotalDays = 0;
        //                    //DateTime ThisYear = new DateTime(DateTime.Now.Year, 1, 1);
        //                    //TotalDays = new TimeSpan(edate.Ticks - ThisYear.Ticks).Days;

        //                    //switch (edate.Month)
        //                    //{
        //                    //    case 1:
        //                    //    case 3:
        //                    //    case 5:
        //                    //    case 7:
        //                    //    case 8:
        //                    //    case 10:
        //                    //    case 12:
        //                    //        if (edate.Day == 31)
        //                    //            TotalDays=TotalDays + 1;
        //                    //        break;
        //                    //    case 4:
        //                    //    case 6:
        //                    //    case 9:
        //                    //    case 11:
        //                    //        if (edate.Day == 30)
        //                    //            TotalDays = TotalDays + 1;
        //                    //        break;
        //                    //    case 2:
        //                    //        if (DateTime.IsLeapYear( edate.Year)==true && edate.Day == 29)
        //                    //            TotalDays = TotalDays + 1;
        //                    //        else if (DateTime.IsLeapYear(edate.Year) == false && edate.Day == 28)
        //                    //            TotalDays = TotalDays + 1;
        //                    //        break;

        //                    //}

        //                    ////subMonth = TotalDays / 30;

        //                    //subMonth=Convert.ToInt16(Math.Ceiling( TotalDays / 3));


        //                    //20150114
        //                    //由月數去計
        //                    DateTime ThisYear = new DateTime(DateTime.Now.Year, 1, 1);
        //                    //subMonth = edate.Year * 12 + edate.Month - (ThisYear.Year * 12 + ThisYear.Month) + 1;
        //                    subMonth = edate.Year * 12 + edate.Month - (ThisYear.Year * 12 + sdate.Month);
        //                    switch (edate.Month)
        //                    {
        //                        case 1:
        //                        case 3:
        //                        case 5:
        //                        case 7:
        //                        case 8:
        //                        case 10:
        //                        case 12:
        //                            if (edate.Day == 31)
        //                                subMonth = Math.Abs(subMonth - 1);
        //                            break;
        //                        case 4:
        //                        case 6:
        //                        case 9:
        //                        case 11:
        //                            if (edate.Day == 30)
        //                                subMonth = Math.Abs(subMonth - 1);
        //                            break;
        //                        case 2:
        //                            if (DateTime.IsLeapYear(edate.Year) == true && edate.Day == 29)
        //                                subMonth = Math.Abs(subMonth - 1);
        //                            else if (DateTime.IsLeapYear(edate.Year) == false && edate.Day == 28)
        //                                subMonth = Math.Abs(subMonth - 1);
        //                            break;

        //                    }


        //                    //int subMonth = edate.Month;
        //                    //if (subMonth < 6)
        //                    //{
        //                    subMonth = 12 - subMonth;
        //                    //}
        //                    if (subMonth != 0)
        //                    {
        //                        rday = MathHelper.Round(sday3 * subMonth / float.Parse("12.0"), 1);
        //                        string[] k = rday.ToString().Split('.');
        //                        sday3 = int.Parse(k[0]);
        //                        if (k.Count() == 2)
        //                        {
        //                            if (int.Parse(k[1]) > 5)
        //                            {
        //                                sday3 += 1;
        //                            }
        //                            else if (int.Parse(k[1]) > 0)
        //                            {
        //                                sday3_hour = 4;
        //                            }
        //                        }
        //                    }
        //                }
        //                else if (int.Parse(MZ_YEAR) == edate.Year - 1911)//當年度復職,以年底減復職日計算
        //                {
        //                    // 20140107 韓毅 簡單修正留職停薪假期問題
        //                    int subMonth = 0;

        //                    //20140502

        //                    ////由天數去計
        //                    //double TotalDays = 0;
        //                    //DateTime ThisYear = new DateTime(DateTime.Now.Year, 1, 1);
        //                    //TotalDays = new TimeSpan(edate.Ticks - ThisYear.Ticks).Days;

        //                    //switch (edate.Month)
        //                    //{
        //                    //    case 1:
        //                    //    case 3:
        //                    //    case 5:
        //                    //    case 7:
        //                    //    case 8:
        //                    //    case 10:
        //                    //    case 12:
        //                    //        if (edate.Day == 31)
        //                    //            TotalDays=TotalDays + 1;
        //                    //        break;
        //                    //    case 4:
        //                    //    case 6:
        //                    //    case 9:
        //                    //    case 11:
        //                    //        if (edate.Day == 30)
        //                    //            TotalDays = TotalDays + 1;
        //                    //        break;
        //                    //    case 2:
        //                    //        if (DateTime.IsLeapYear( edate.Year)==true && edate.Day == 29)
        //                    //            TotalDays = TotalDays + 1;
        //                    //        else if (DateTime.IsLeapYear(edate.Year) == false && edate.Day == 28)
        //                    //            TotalDays = TotalDays + 1;
        //                    //        break;

        //                    //}

        //                    ////subMonth = TotalDays / 30;

        //                    //subMonth=Convert.ToInt16(Math.Ceiling( TotalDays / 3));


        //                    //20150114
        //                    //由月數去計
        //                    DateTime ThisYear = new DateTime(DateTime.Now.Year, 1, 1);
        //                    subMonth = edate.Year * 12 + edate.Month - (ThisYear.Year * 12 + ThisYear.Month) + 1;




        //                    //int subMonth = edate.Month;
        //                    //if (subMonth < 6)
        //                    //{
        //                    subMonth = 12 - subMonth;
        //                    //}
        //                    if (subMonth != 0)
        //                    {
        //                        rday = MathHelper.Round(sday3 * subMonth / float.Parse("12.0"), 1);
        //                        string[] k = rday.ToString().Split('.');
        //                        sday3 = int.Parse(k[0]);
        //                        if (k.Count() == 2)
        //                        {
        //                            if (int.Parse(k[1]) > 5)
        //                            {
        //                                sday3 += 1;
        //                            }
        //                            else if (int.Parse(k[1]) > 0)
        //                            {
        //                                sday3_hour = 4;
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //            #endregion

        //            string InsertSQL = "INSERT INTO C_DLTBB(MZ_YEAR,MZ_ID,MZ_HDAY,MZ_HTIME,MZ_SDAY,MZ_SDAY2,MZ_PDAY,MZ_SICKDAY,MZ_HCAREDAY) VALUES(@MZ_YEAR,@MZ_ID,@MZ_HDAY,@MZ_HTIME,@MZ_SDAY,@MZ_SDAY2,@MZ_PDAY,@MZ_SICKDAY,@MZ_HCAREDAY)";

        //            SqlParameter[] parameterList ={
        //            new SqlParameter("MZ_YEAR",SqlDbType.VarChar){Value=MZ_YEAR},
        //            new SqlParameter("MZ_ID",SqlDbType.VarChar){Value=dt.Rows[i]["MZ_ID"].ToString()},
        //            new SqlParameter("MZ_HDAY",SqlDbType.Float){Value=sday3},
        //            new SqlParameter("MZ_HTIME",SqlDbType.Float){Value=sday3_hour},
        //            new SqlParameter("MZ_SDAY",SqlDbType.Float){Value=string.IsNullOrEmpty(dt.Rows[i]["MZ_SDAY3"].ToString())?"0":dt.Rows[i]["MZ_SDAY3"].ToString()},
        //            new SqlParameter("MZ_SDAY2",SqlDbType.Float){Value=string.IsNullOrEmpty(dt.Rows[i]["MZ_SDAY"].ToString())?"0":dt.Rows[i]["MZ_SDAY"].ToString()},
        //            new SqlParameter("MZ_PDAY",SqlDbType.Float){Value=pday },
        //            new SqlParameter("MZ_SICKDAY",SqlDbType.Float){Value=sickday },
        //            new SqlParameter("MZ_HCAREDAY",SqlDbType.Float){Value=hcareday },

        //            };
        //            try
        //            {
        //                o_DBFactory.ABC_toTest.ExecuteNonQuery( InsertSQL, parameterList);
        //                if (i == dt.Rows.Count - 1)
        //                {
        //                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('計算完成');", true);
        //                }
        //            }
        //            catch (Exception)
        //            {
        //                throw;
        //            }
        //        }
        //    }

        //    #endregion 全部人員

        //    #region 新進人員

        //    else if (RadioButtonList1.SelectedValue == "新進人員")
        //    {
        //        //原初任職日改為合格實授日 20181120 by sky
        //        //string strSQL = "SELECT MZ_FDATE,MZ_ID,MZ_TYEAR,MZ_TMONTH,MZ_RYEAR,MZ_RMONTH,,'' MZ_SDAY3,'' MZ_SDAY2,'' MZ_SDAY  FROM A_DLBASE WHERE MZ_ID NOT IN (SELECT MZ_ID FROM C_DLTBB1) ";
        //        string strSQL = "SELECT MZ_QUA_DATE ,MZ_ID,MZ_TYEAR,MZ_TMONTH,MZ_RYEAR,MZ_RMONTH,,'' MZ_SDAY3,'' MZ_SDAY2,'' MZ_SDAY  FROM A_DLBASE WHERE MZ_ID NOT IN (SELECT MZ_ID FROM C_DLTBB1) ";

        //        DataTable dt = new DataTable();
        //        dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");

        //        string pday = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_DAY FROM C_DLCODE_SETDAY WHERE MZ_CODE = (SELECT MZ_CODE FROM C_DLCODE WHERE MZ_CNAME = '事假')");
        //        string sickday = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_DAY FROM C_DLCODE_SETDAY WHERE MZ_CODE = (SELECT MZ_CODE FROM C_DLCODE WHERE MZ_CNAME = '病假')");
        //        string hcareday = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_DAY FROM C_DLCODE_SETDAY WHERE MZ_CODE = (SELECT MZ_CODE FROM C_DLCODE WHERE MZ_CNAME = '家庭照顧')");

        //        pday = string.IsNullOrEmpty(pday) ? "0" : pday;

        //        sickday = string.IsNullOrEmpty(sickday) ? "0" : sickday;

        //        hcareday = string.IsNullOrEmpty(hcareday) ? "0" : hcareday;

        //        int sday3;

        //        string MZ_YEAR = TextBox_MZ_YEAR.Text.PadLeft(3, '0');

        //        string ERRORFDATE_ID = "";

        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            int tyear = int.Parse(string.IsNullOrEmpty(dt.Rows[i]["MZ_TYEAR"].ToString()) ? "0" : dt.Rows[i]["MZ_TYEAR"].ToString());

        //            int tmonth = int.Parse(string.IsNullOrEmpty(dt.Rows[i]["MZ_TMONTH"].ToString()) ? "0" : dt.Rows[i]["MZ_TMONTH"].ToString());

        //            int ryear = int.Parse(string.IsNullOrEmpty(dt.Rows[i]["MZ_RYEAR"].ToString()) ? "0" : dt.Rows[i]["MZ_RYEAR"].ToString());

        //            int rmonth = int.Parse(string.IsNullOrEmpty(dt.Rows[i]["MZ_RMONTH"].ToString()) ? "0" : dt.Rows[i]["MZ_RMONTH"].ToString());

        //            DateTime FDATE;

        //            string fdate = dt.Rows[i]["MZ_ID"].ToString();

        //            if (string.IsNullOrEmpty(dt.Rows[i]["MZ_QUA_DATE"].ToString()))
        //            {
        //                ERRORFDATE_ID += dt.Rows[i]["MZ_ID"].ToString();
        //                continue;
        //            }

        //            try
        //            {
        //                //irk 2013/1/2 修正
        //                //FDATE = DateTime.Parse((int.Parse(dt.Rows[i]["MZ_FDATE"].ToString().Substring(0, 3)) + 1911).ToString() + "-" + dt.Rows[i]["MZ_FDATE"].ToString().Substring(3, 2) + "-" + dt.Rows[i]["MZ_FDATE"].ToString().Substring(5, 2));
        //                FDATE = DateTime.Parse((int.Parse(dt.Rows[i]["MZ_QUA_DATE"].ToString().Substring(0, 3)) + 1911).ToString() + "-" + dt.Rows[i]["MZ_QUA_DATE"].ToString().Substring(3, 2) + "-01");
        //            }
        //            catch
        //            {
        //                FDATE = DateTime.Now;
        //            }

        //            int monthDiff = Convert.ToInt32(Microsoft.VisualBasic.DateAndTime.DateDiff("M", FDATE, DateTime.Parse((int.Parse(TextBox_MZ_YEAR.Text.Trim()) + 1911).ToString() + "-01-01").AddDays(-1), Microsoft.VisualBasic.FirstDayOfWeek.Sunday, Microsoft.VisualBasic.FirstWeekOfYear.FirstFullWeek));

        //            int MONTH = monthDiff;

        //            int sday3_hour = 0;

        //            int addMONTH = tyear * 12 + tmonth - ryear * 12 - rmonth;

        //            updateBasicOff(FDATE, dt.Rows[i]["MZ_ID"].ToString(), addMONTH);

        //            //20140114   扣除中間留職停薪的月份 立廷

        //            strSQL = "SELECT EDATE,SDATE FROM C_LVHISTORY WHERE MZ_ID=@MZ_ID AND BACK=1 ORDER BY EDATE";
        //            SqlParameter[] parameterList1 ={
        //            new SqlParameter("MZ_ID",SqlDbType.VarChar){Value=fdate},
        //            };
        //            DataTable stay_job = o_DBFactory.ABC_toTest.ExecuteDataset( strSQL, parameterList1).Tables[0];
        //            if (stay_job.Rows.Count > 0)
        //            {

        //                for (int k = 0; k < stay_job.Rows.Count; k++)
        //                {
        //                    TimeSpan month = DateTime.Parse(stay_job.Rows[k][0].ToString()) - DateTime.Parse(stay_job.Rows[k][1].ToString());
        //                    MONTH = MONTH - (month.Days / 30);

        //                }

        //                //TimeSpan month = DateTime.Parse(stay_job.Rows[stay_job.Rows.Count - 1][0].ToString()) - DateTime.Parse(stay_job.Rows[stay_job.Rows.Count - 1][1].ToString());
        //                //MONTH = MONTH - (month.Days / 30);

        //            }
        //            //20140114 



        //            if (MONTH < 0)
        //            {
        //                sday3 = 0;
        //            }
        //            else
        //            {
        //                if (MONTH < 12)
        //                {
        //                    //if (FDATE.Month == 1)
        //                    //{
        //                    //    sday3 = 7;
        //                    //}
        //                    //else
        //                    //{
        //                    if (addMONTH == 0)
        //                    {
        //                        double countsay = MathHelper.Round(7 * double.Parse(MONTH.ToString()) / 12, 1);

        //                        string[] s = countsay.ToString().Split('.');

        //                        sday3 = int.Parse(s[0]);

        //                        if (s.Length == 2)
        //                        {
        //                            if (int.Parse(s[1]) > 5)
        //                            {
        //                                sday3 = sday3 + 1;
        //                            }
        //                            else if (int.Parse(s[1]) > 0)
        //                            {
        //                                sday3_hour = 4;
        //                            }
        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (addMONTH < 36)
        //                        {
        //                            double countsay = MathHelper.Round(7 * double.Parse(MONTH.ToString()) / 12, 1);

        //                            string[] s = countsay.ToString().Split('.');

        //                            sday3 = int.Parse(s[0]);
        //                            if (s.Length == 2)
        //                            {
        //                                if (int.Parse(s[1]) > 5)
        //                                {
        //                                    sday3 = sday3 + 1;
        //                                }
        //                                else if (int.Parse(s[1]) > 0)
        //                                {
        //                                    sday3_hour = 4;
        //                                }
        //                            }
        //                        }
        //                        else if (36 <= addMONTH && addMONTH < 72)
        //                        {
        //                            double countsay = MathHelper.Round(14 * double.Parse(MONTH.ToString()) / 12, 1);

        //                            string[] s = countsay.ToString().Split('.');

        //                            sday3 = int.Parse(s[0]);
        //                            if (s.Length == 2)
        //                            {
        //                                if (int.Parse(s[1]) > 5)
        //                                {
        //                                    sday3 = sday3 + 1;
        //                                }
        //                                else if (int.Parse(s[1]) > 0)
        //                                {
        //                                    sday3_hour = 4;
        //                                }
        //                            }
        //                        }
        //                        else if (72 <= addMONTH && addMONTH < 108)
        //                        {
        //                            double countsay = MathHelper.Round(21 * double.Parse(MONTH.ToString()) / 12, 1);

        //                            string[] s = countsay.ToString().Split('.');

        //                            sday3 = int.Parse(s[0]);

        //                            if (s.Length == 2)
        //                            {
        //                                if (int.Parse(s[1]) > 5)
        //                                {
        //                                    sday3 = sday3 + 1;
        //                                }
        //                                else if (int.Parse(s[1]) > 0)
        //                                {
        //                                    sday3_hour = 4;
        //                                }
        //                            }
        //                        }
        //                        else if (108 <= addMONTH && addMONTH < 168)
        //                        {
        //                            double countsay = MathHelper.Round(28 * double.Parse(MONTH.ToString()) / 12, 1);

        //                            string[] s = countsay.ToString().Split('.');

        //                            sday3 = int.Parse(s[0]);

        //                            if (s.Length == 2)
        //                            {
        //                                if (int.Parse(s[1]) > 5)
        //                                {
        //                                    sday3 = sday3 + 1;
        //                                }
        //                                else if (int.Parse(s[1]) > 0)
        //                                {
        //                                    sday3_hour = 4;
        //                                }
        //                            }
        //                        }
        //                        else
        //                        {
        //                            double countsay = MathHelper.Round(30 * double.Parse(MONTH.ToString()) / 12, 1);

        //                            string[] s = countsay.ToString().Split('.');

        //                            sday3 = int.Parse(s[0]);

        //                            if (s.Length == 2)
        //                            {
        //                                if (int.Parse(s[1]) > 5)
        //                                {
        //                                    sday3 = sday3 + 1;
        //                                }
        //                                else if (int.Parse(s[1]) > 0)
        //                                {
        //                                    sday3_hour = 4;
        //                                }
        //                            }
        //                        }
        //                    }
        //                    //}
        //                }
        //                else if (12 <= MONTH + addMONTH && MONTH + addMONTH < 36)
        //                {
        //                    sday3 = 7;
        //                }
        //                else if (36 <= MONTH + addMONTH && MONTH + addMONTH < 72)
        //                {
        //                    sday3 = 14;
        //                }
        //                else if (72 <= MONTH + addMONTH && MONTH + addMONTH < 108)
        //                {
        //                    sday3 = 21;
        //                }
        //                else if (108 <= MONTH + addMONTH && MONTH + addMONTH < 168)
        //                {
        //                    sday3 = 28;
        //                }
        //                else
        //                {
        //                    sday3 = 30;
        //                }
        //            }

        //            #region 留職停薪 2012/11/29 irk 新增
        //            //strSQL = "SELECT EDATE,SDATE FROM C_LVHISTORY WHERE MZ_ID=@MZ_ID AND BACK=1 ORDER BY SN DESC";
        //            //SqlParameter[] parameterList1 ={
        //            //new SqlParameter("MZ_ID",SqlDbType.VarChar){Value=fdate},
        //            //};
        //            //DataTable temp = o_DBFactory.ABC_toTest.ExecuteDataset( strSQL, parameterList1).Tables[0];
        //            if (stay_job.Rows.Count > 0)
        //            {
        //                double rday = 0;
        //                DateTime sdate = DateTime.Parse(stay_job.Rows[stay_job.Rows.Count - 1][1].ToString()).AddYears(1);
        //                //sdate = new DateTime(sdate.Year, sdate.Month, 1);
        //                DateTime edate = DateTime.Parse(stay_job.Rows[stay_job.Rows.Count - 1][0].ToString()).AddYears(1);
        //                //edate = new DateTime(edate.Year, edate.Month, 1);

        //                //20150327
        //                if (int.Parse(MZ_YEAR) + 1 == edate.Year - 1911)
        //                {
        //                    sdate = sdate.AddYears(-1);
        //                    edate = edate.AddYears(-1);
        //                }

        //                if (int.Parse(MZ_YEAR) != edate.Year - 1911)////有跨年度留職停薪
        //                {
        //                    //2015 104 103 
        //                    if ((int.Parse(MZ_YEAR) >= (sdate.Year - 1911)) && (int.Parse(MZ_YEAR) <= (edate.Year - 1911)))
        //                    {
        //                        sday3 = 0;
        //                        sday3_hour = 0;
        //                    }
        //                }
        //                else if (int.Parse(MZ_YEAR) == edate.Year - 1911 && int.Parse(MZ_YEAR) == sdate.Year - 1911)//當年度請.當年度復職
        //                {
        //                    // 20140107 韓毅 簡單修正留職停薪假期問題
        //                    int subMonth = 0;



        //                    //20140502
        //                    ////由天數去計
        //                    //int TotalDays = 0;
        //                    //    DateTime ThisYear = new DateTime(DateTime.Now.Year, 1, 1);
        //                    //    TotalDays = new TimeSpan(edate.Ticks - ThisYear.Ticks).Days;


        //                        //switch (edate.Month)
        //                        //{
        //                        //    case 1:
        //                        //    case 3:
        //                        //    case 5:
        //                        //    case 7:
        //                        //    case 8:
        //                        //    case 10:
        //                        //    case 12:
        //                        //        if (edate.Day == 31)
        //                        //            TotalDays=TotalDays + 1;
        //                        //        break;
        //                        //    case 4:
        //                        //    case 6:
        //                        //    case 9:
        //                        //    case 11:
        //                        //        if (edate.Day == 30)
        //                        //            TotalDays = TotalDays + 1;
        //                        //        break;
        //                        //    case 2:
        //                        //        if (DateTime.IsLeapYear( edate.Year)==true && edate.Day == 29)
        //                        //            TotalDays = TotalDays + 1;
        //                        //        else if (DateTime.IsLeapYear(edate.Year) == false && edate.Day == 28)
        //                        //            TotalDays = TotalDays + 1;
        //                        //        break;

        //                        //}

        //                        ////subMonth = TotalDays / 30;

        //                        //subMonth=Convert.ToInt16(Math.Ceiling( TotalDays / 3));

        //                    //20150114
        //                    //由月數去計
        //                    DateTime ThisYear = new DateTime(DateTime.Now.Year, 1, 1);
        //                    //subMonth = edate.Year * 12 + edate.Month - (ThisYear.Year * 12 + ThisYear.Month) + 1;
        //                    subMonth = edate.Year * 12 + edate.Month - (ThisYear.Year * 12 + sdate.Month);



        //                    //int subMonth = edate.Month;
        //                    //if (subMonth < 6)
        //                    //{
        //                    subMonth = 12 - subMonth;
        //                    //}
        //                    if (subMonth != 0)
        //                    {
        //                        rday = MathHelper.Round(sday3 * subMonth / float.Parse("12.0"), 1);
        //                        string[] k = rday.ToString().Split('.');
        //                        sday3 = int.Parse(k[0]);
        //                        if (k.Count() == 2)
        //                        {
        //                            if (int.Parse(k[1]) > 5)
        //                            {
        //                                sday3 += 1;
        //                            }
        //                            else if (int.Parse(k[1]) > 0)
        //                            {
        //                                sday3_hour = 4;
        //                            }
        //                        }
        //                    }
        //                }
        //                else if (int.Parse(MZ_YEAR) == edate.Year - 1911)//當年度復職,以年底減復職日計算
        //                {
        //                    // 20140107 韓毅 簡單修正留職停薪假期問題
        //                    int subMonth = 0;



        //                    //20140502
        //                    ////由天數去計
        //                    //int TotalDays = 0;
        //                    //    DateTime ThisYear = new DateTime(DateTime.Now.Year, 1, 1);
        //                    //    TotalDays = new TimeSpan(edate.Ticks - ThisYear.Ticks).Days;


        //                    //switch (edate.Month)
        //                    //{
        //                    //    case 1:
        //                    //    case 3:
        //                    //    case 5:
        //                    //    case 7:
        //                    //    case 8:
        //                    //    case 10:
        //                    //    case 12:
        //                    //        if (edate.Day == 31)
        //                    //            TotalDays=TotalDays + 1;
        //                    //        break;
        //                    //    case 4:
        //                    //    case 6:
        //                    //    case 9:
        //                    //    case 11:
        //                    //        if (edate.Day == 30)
        //                    //            TotalDays = TotalDays + 1;
        //                    //        break;
        //                    //    case 2:
        //                    //        if (DateTime.IsLeapYear( edate.Year)==true && edate.Day == 29)
        //                    //            TotalDays = TotalDays + 1;
        //                    //        else if (DateTime.IsLeapYear(edate.Year) == false && edate.Day == 28)
        //                    //            TotalDays = TotalDays + 1;
        //                    //        break;

        //                    //}

        //                    ////subMonth = TotalDays / 30;

        //                    //subMonth=Convert.ToInt16(Math.Ceiling( TotalDays / 3));

        //                    //20150114
        //                    //由月數去計
        //                    DateTime ThisYear = new DateTime(DateTime.Now.Year, 1, 1);
        //                    subMonth = edate.Year * 12 + edate.Month - (ThisYear.Year * 12 + ThisYear.Month) ;





        //                    //int subMonth = edate.Month;
        //                    //if (subMonth < 6)
        //                    //{
        //                    subMonth = 12 - subMonth;
        //                    //}
        //                    if (subMonth != 0)
        //                    {
        //                        rday = MathHelper.Round(sday3 * subMonth / float.Parse("12.0"), 1);
        //                        string[] k = rday.ToString().Split('.');
        //                        sday3 = int.Parse(k[0]);
        //                        if (k.Count() == 2)
        //                        {
        //                            if (int.Parse(k[1]) > 5)
        //                            {
        //                                sday3 += 1;
        //                            }
        //                            else if (int.Parse(k[1]) > 0)
        //                            {
        //                                sday3_hour = 4;
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //            #endregion

        //            string InsertSQL = "INSERT INTO C_DLTBB(MZ_YEAR,MZ_ID,MZ_HDAY,MZ_HTIME,MZ_SDAY,MZ_SDAY2,MZ_PDAY,MZ_SICKDAY,MZ_HCAREDAY) VALUES(@MZ_YEAR,@MZ_ID,@MZ_HDAY,@MZ_HTIME,@MZ_SDAY,@MZ_SDAY2,@MZ_PDAY,@MZ_SICKDAY,@MZ_HCAREDAY)";

        //            SqlParameter[] parameterList ={
        //            new SqlParameter("MZ_YEAR",SqlDbType.VarChar){Value=MZ_YEAR},
        //            new SqlParameter("MZ_ID",SqlDbType.VarChar){Value=dt.Rows[i]["MZ_ID"].ToString()},
        //            new SqlParameter("MZ_HDAY",SqlDbType.Float){Value=sday3},
        //            new SqlParameter("MZ_HTIME",SqlDbType.Float){Value=sday3_hour},
        //            new SqlParameter("MZ_SDAY",SqlDbType.Float){Value=string.IsNullOrEmpty(dt.Rows[i]["MZ_SDAY3"].ToString())?"0":dt.Rows[i]["MZ_SDAY3"].ToString()},
        //            new SqlParameter("MZ_SDAY2",SqlDbType.Float){Value=string.IsNullOrEmpty(dt.Rows[i]["MZ_SDAY"].ToString())?"0":dt.Rows[i]["MZ_SDAY"].ToString()},
        //            new SqlParameter("MZ_PDAY",SqlDbType.Float){Value=pday },
        //            new SqlParameter("MZ_SICKDAY",SqlDbType.Float){Value=sickday },
        //            new SqlParameter("MZ_HCAREDAY",SqlDbType.Float){Value=hcareday },

        //            };
        //            try
        //            {
        //                o_DBFactory.ABC_toTest.ExecuteNonQuery( InsertSQL, parameterList);
        //                if (i == dt.Rows.Count - 1)
        //                {
        //                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('計算完成');", true);
        //                }
        //            }
        //            catch (Exception)
        //            {
        //                throw;
        //            }
        //        }
        //    }

        //    #endregion 新進人員

        //    #region 單人
        //    else
        //    {
        //        string personal_ID = "";

        //        if (!string.IsNullOrEmpty(TextBox_MZ_ID.Text))
        //        {
        //            personal_ID = " AND MZ_ID='" + TextBox_MZ_ID.Text + "'";
        //        }
        //        else
        //        {
        //            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請輸入身分證號！')", true);
        //            return;
        //        }

        //        //原初任職日改為合格實授日 20181120 by sky
        //        //string strSQL = "SELECT MZ_FDATE,MZ_TYEAR,MZ_TMONTH,MZ_RYEAR,MZ_RMONTH,MZ_ID,'' MZ_SDAY3,'' MZ_SDAY2,'' MZ_SDAY FROM A_DLBASE WHERE 1=1 " + personal_ID;
        //        string strSQL = "SELECT MZ_QUA_DATE,MZ_TYEAR,MZ_TMONTH,MZ_RYEAR,MZ_RMONTH,MZ_ID,'' MZ_SDAY3,'' MZ_SDAY2,'' MZ_SDAY FROM A_DLBASE WHERE 1=1 " + personal_ID;

        //        DataTable dt = new DataTable();
        //        dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");

        //        string pday = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_DAY FROM C_DLCODE_SETDAY WHERE MZ_CODE = (SELECT MZ_CODE FROM C_DLCODE WHERE MZ_CNAME = '事假')");
        //        string sickday = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_DAY FROM C_DLCODE_SETDAY WHERE MZ_CODE = (SELECT MZ_CODE FROM C_DLCODE WHERE MZ_CNAME = '病假')");
        //        string hcareday = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_DAY FROM C_DLCODE_SETDAY WHERE MZ_CODE = (SELECT MZ_CODE FROM C_DLCODE WHERE MZ_CNAME = '家庭照顧')");

        //        pday = string.IsNullOrEmpty(pday) ? "0" : pday;

        //        sickday = string.IsNullOrEmpty(sickday) ? "0" : sickday;

        //        hcareday = string.IsNullOrEmpty(hcareday) ? "0" : hcareday;

        //        int sday3;

        //        string txtYear = TextBox_MZ_YEAR.Text.PadLeft(3, '0');

        //        string ERRORFDATE_ID = "";

        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            int tyear = int.Parse(string.IsNullOrEmpty(dt.Rows[i]["MZ_TYEAR"].ToString()) ? "0" : dt.Rows[i]["MZ_TYEAR"].ToString());

        //            int tmonth = int.Parse(string.IsNullOrEmpty(dt.Rows[i]["MZ_TMONTH"].ToString()) ? "0" : dt.Rows[i]["MZ_TMONTH"].ToString());

        //            int ryear = int.Parse(string.IsNullOrEmpty(dt.Rows[i]["MZ_RYEAR"].ToString()) ? "0" : dt.Rows[i]["MZ_RYEAR"].ToString());

        //            int rmonth = int.Parse(string.IsNullOrEmpty(dt.Rows[i]["MZ_RMONTH"].ToString()) ? "0" : dt.Rows[i]["MZ_RMONTH"].ToString());

        //            DateTime FDATE;

        //            string fdate = dt.Rows[i]["MZ_ID"].ToString();

        //            string DeleteString = "DELETE FROM C_DLTBB WHERE MZ_YEAR='" + TextBox_MZ_YEAR.Text + "' AND MZ_ID='" + fdate + "'";

        //            o_DBFactory.ABC_toTest.Edit_Data(DeleteString);

        //            if (string.IsNullOrEmpty(dt.Rows[i]["MZ_QUA_DATE"].ToString()))
        //            {
        //                ERRORFDATE_ID += dt.Rows[i]["MZ_ID"].ToString();
        //                //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('此人初任公職日有誤，請查明後再算！')", true);
        //                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('此人合格實授日有誤，請查明後再算！')", true);
        //                return;
        //            }

        //            try
        //            {
        //                FDATE = DateTime.Parse((int.Parse(dt.Rows[i]["MZ_QUA_DATE"].ToString().Substring(0, 3)) + 1911).ToString() + "-" + dt.Rows[i]["MZ_QUA_DATE"].ToString().Substring(3, 2) + "-01");
        //            }
        //            catch
        //            {
        //                FDATE = DateTime.Now;
        //            }

        //            //if (FDATE.Year == 2009 && FDATE.Month == 10)
        //            //{
        //            //    FDATE = FDATE;
        //            //}

        //            //到職日到現在經過幾月
        //            int monthDiff = Convert.ToInt32(Microsoft.VisualBasic.DateAndTime.DateDiff("M", FDATE, DateTime.Parse((int.Parse(TextBox_MZ_YEAR.Text.Trim()) + 1911).ToString() + "-01-01"), Microsoft.VisualBasic.FirstDayOfWeek.Sunday, Microsoft.VisualBasic.FirstWeekOfYear.FirstFullWeek));

        //            int MONTH = monthDiff;

        //            int sday3_hour = 0;

        //            int addMONTH = tyear * 12 + tmonth - ryear * 12 - rmonth;

        //            updateBasicOff(FDATE, dt.Rows[i]["MZ_ID"].ToString(), addMONTH);

        //            //20140114   扣除中間留職停薪的月份 立廷
        //            #region 計算應有特休天數

        //            strSQL = "SELECT EDATE,SDATE FROM C_LVHISTORY WHERE MZ_ID=@MZ_ID AND BACK=1 ORDER BY EDATE ";
        //            //strSQL = "SELECT EDATE,SDATE FROM C_LVHISTORY WHERE MZ_ID=@MZ_ID AND SN = 744 ";
        //            SqlParameter[] parameterList1 = {
        //                new SqlParameter("MZ_ID",SqlDbType.VarChar){Value=fdate},
        //            };

        //            DataTable stay_job = o_DBFactory.ABC_toTest.ExecuteDataset( strSQL, parameterList1).Tables[0];
        //            if (stay_job.Rows.Count > 0)
        //            {
        //                for (int k = 0; k < stay_job.Rows.Count; k++)
        //                {
        //                    //時間差
        //                    TimeSpan month = DateTime.Parse(stay_job.Rows[k][0].ToString()) - DateTime.Parse(stay_job.Rows[k][1].ToString());
        //                    MONTH = MONTH - (month.Days / 30);
        //                }

        //                //TimeSpan month =  DateTime.Parse(stay_job.Rows[stay_job.Rows.Count - 1][0].ToString()) - DateTime.Parse(stay_job.Rows[stay_job.Rows.Count - 1][1].ToString());
        //                //MONTH = MONTH - (month.Days / 30);

        //            }
        //            //20140114 

        //            if (MONTH < 0)
        //            {
        //                sday3 = 0;
        //            }
        //            else
        //            {
        //                if (MONTH < 12)
        //                {
        //                    //if (addMONTH == 0 && FDATE.Month == 1)
        //                    //{
        //                    //    sday3 = 7;
        //                    //}
        //                    //else
        //                    //{
        //                    if (addMONTH == 0)
        //                    {
        //                        double countsay = MathHelper.Round(7 * double.Parse(MONTH.ToString()) / 12, 1);

        //                        string[] s = countsay.ToString().Split('.');

        //                        sday3 = int.Parse(s[0]);

        //                        if (s.Length == 2)
        //                        {
        //                            if (int.Parse(s[1]) > 5)
        //                            {
        //                                sday3 = sday3 + 1;
        //                            }
        //                            else if (int.Parse(s[1]) > 0)
        //                            {
        //                                sday3_hour = 4;
        //                            }
        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (addMONTH + MONTH < 36)
        //                        {
        //                            double countsay = MathHelper.Round(7 * double.Parse(MONTH.ToString()) / 12, 1);

        //                            string[] s = countsay.ToString().Split('.');

        //                            sday3 = int.Parse(s[0]);
        //                            if (s.Length == 2)
        //                            {
        //                                if (int.Parse(s[1]) > 5)
        //                                {
        //                                    sday3 = sday3 + 1;
        //                                }
        //                                else if (int.Parse(s[1]) > 0)
        //                                {
        //                                    sday3_hour = 4;
        //                                }
        //                            }
        //                        }
        //                        else if (36 <= addMONTH + MONTH && addMONTH + MONTH < 72)
        //                        {
        //                            double countsay = MathHelper.Round(14 * double.Parse(MONTH.ToString()) / 12, 1);

        //                            string[] s = countsay.ToString().Split('.');

        //                            sday3 = int.Parse(s[0]);
        //                            if (s.Length == 2)
        //                            {
        //                                if (int.Parse(s[1]) > 5)
        //                                {
        //                                    sday3 = sday3 + 1;
        //                                }
        //                                else if (int.Parse(s[1]) > 0)
        //                                {
        //                                    sday3_hour = 4;
        //                                }
        //                            }
        //                        }
        //                        else if (72 <= addMONTH + MONTH && addMONTH + MONTH < 108)
        //                        {
        //                            double countsay = MathHelper.Round(21 * double.Parse(MONTH.ToString()) / 12, 1);

        //                            string[] s = countsay.ToString().Split('.');

        //                            sday3 = int.Parse(s[0]);

        //                            if (s.Length == 2)
        //                            {
        //                                if (int.Parse(s[1]) > 5)
        //                                {
        //                                    sday3 = sday3 + 1;
        //                                }
        //                                else if (int.Parse(s[1]) > 0)
        //                                {
        //                                    sday3_hour = 4;
        //                                }
        //                            }
        //                        }
        //                        else if (108 <= addMONTH + MONTH && addMONTH + MONTH < 168)
        //                        {
        //                            double countsay = MathHelper.Round(28 * double.Parse(MONTH.ToString()) / 12, 1);

        //                            string[] s = countsay.ToString().Split('.');

        //                            sday3 = int.Parse(s[0]);

        //                            if (s.Length == 2)
        //                            {
        //                                if (int.Parse(s[1]) > 5)
        //                                {
        //                                    sday3 = sday3 + 1;
        //                                }
        //                                else if (int.Parse(s[1]) > 0)
        //                                {
        //                                    sday3_hour = 4;
        //                                }
        //                            }
        //                        }
        //                        else
        //                        {
        //                            double countsay = MathHelper.Round(30 * double.Parse(MONTH.ToString()) / 12, 1);

        //                            string[] s = countsay.ToString().Split('.');

        //                            sday3 = int.Parse(s[0]);

        //                            if (s.Length == 2)
        //                            {
        //                                if (int.Parse(s[1]) > 5)
        //                                {
        //                                    sday3 = sday3 + 1;
        //                                }
        //                                else if (int.Parse(s[1]) > 0)
        //                                {
        //                                    sday3_hour = 4;
        //                                }
        //                            }
        //                        }
        //                    }
        //                    //}
        //                }
        //                else if (12 <= MONTH + addMONTH && MONTH + addMONTH < 36)
        //                {
        //                    sday3 = 7;
        //                }                       
        //                else if (36 <= MONTH + addMONTH && MONTH + addMONTH < 72)
        //                {
        //                    sday3 = 14;
        //                }
        //                else if (72 <= MONTH + addMONTH && MONTH + addMONTH < 108)                           
        //                {
        //                    sday3 = 21;
        //                }
        //                else if (108 <= MONTH + addMONTH && MONTH + addMONTH < 168)
        //                {
        //                    sday3 = 28;
        //                }
        //                else
        //                {
        //                    sday3 = 30;
        //                }
        //            }

        //            #endregion

        //            #region 留職停薪 
        //            //strSQL = "SELECT EDATE,SDATE FROM C_LVHISTORY WHERE MZ_ID=@MZ_ID AND BACK=1 ORDER BY SN DESC";
        //            //SqlParameter[] parameterList1 ={
        //            //new SqlParameter("MZ_ID",SqlDbType.VarChar){Value=fdate},
        //            //};
        //            //DataTable temp = o_DBFactory.ABC_toTest.ExecuteDataset( strSQL, parameterList1).Tables[0];

        //            //若本年度有請假，才會進入此部分做計算
        //            if (stay_job.Rows.Count > 0)
        //            {
        //                double rday = 0;

        //                //這邊僅取最後一筆資料
        //                DataRow rowLastVaction = stay_job.Rows[stay_job.Rows.Count - 1] ;

        //                DateTime sdate = DateTime.Parse(rowLastVaction[1].ToString()).AddYears(1);
        //                DateTime edate = DateTime.Parse(rowLastVaction[0].ToString()).AddYears(1);

        //                //DateTime sdate = DateTime.Parse(temp.Rows[0][1].ToString()).AddYears(1);
        //                //sdate = new DateTime(sdate.Year, sdate.Month, 1);
        //                //DateTime edate = DateTime.Parse(temp.Rows[0][0].ToString()).AddYears(1);
        //                //edate = new DateTime(edate.Year, edate.Month, 1);

        //                //if (int.Parse(MZ_YEAR) + 1 == edate.Year - 1911)
        //                //{

        //                //}
        //                //else //迄的時間是去年
        //                //{

        //                    //20150327
        //                    //若結束日期為
        //                    if (int.Parse(txtYear) + 1 == edate.Year - 1911)
        //                    {
        //                       sdate= sdate.AddYears(-1);
        //                       edate = edate.AddYears(-1);
        //                    }

        //                    //若輸入年度不等於結束日期之年度別
        //                    if (int.Parse(txtYear) != edate.Year - 1911)////有跨年度留職停薪
        //                    {
        //                        //2015 104 103 
        //                        if((int.Parse(txtYear) >= (sdate.Year - 1911)) &&
        //                           (int.Parse(txtYear) <= (edate.Year - 1911)))
        //                        {
        //                            sday3 = 0;
        //                            sday3_hour = 0;
        //                        }
        //                    }

        //                    //當年度請，且當年度復職
        //                    else if (int.Parse(txtYear) == edate.Year - 1911
        //                             && int.Parse(txtYear) == sdate.Year - 1911)
        //                    {
        //                        // 20140107 韓毅 簡單修正留職停薪假期問題
        //                        int subMonth = 0;

        //                        ////由天數去計
        //                        //double TotalDays = 0;
        //                        //DateTime ThisYear = new DateTime(DateTime.Now.Year, 1, 1);
        //                        //TotalDays = new TimeSpan(edate.Ticks - ThisYear.Ticks).Days;

        //                        //switch (edate.Month)
        //                        //{
        //                        //    case 1:
        //                        //    case 3:
        //                        //    case 5:
        //                        //    case 7:
        //                        //    case 8:
        //                        //    case 10:
        //                        //    case 12:
        //                        //        if (edate.Day == 31)
        //                        //            TotalDays=TotalDays + 1;
        //                        //        break;
        //                        //    case 4:
        //                        //    case 6:
        //                        //    case 9:
        //                        //    case 11:
        //                        //        if (edate.Day == 30)
        //                        //            TotalDays = TotalDays + 1;
        //                        //        break;
        //                        //    case 2:
        //                        //        if (DateTime.IsLeapYear( edate.Year)==true && edate.Day == 29)
        //                        //            TotalDays = TotalDays + 1;
        //                        //        else if (DateTime.IsLeapYear(edate.Year) == false && edate.Day == 28)
        //                        //            TotalDays = TotalDays + 1;
        //                        //        break;

        //                        //}

        //                        ////subMonth = TotalDays / 30;

        //                        //subMonth=Convert.ToInt16(Math.Ceiling( TotalDays / 3));

        //                        //20150114
        //                        //由月數去計
        //                        DateTime ThisYear = new DateTime(DateTime.Now.Year, 1, 1);
        //                        //subMonth = edate.Year * 12 + edate.Month - (ThisYear.Year * 12 + ThisYear.Month) + 1;
        //                        subMonth = edate.Year * 12 + edate.Month - (ThisYear.Year * 12 + sdate.Month);


        //                        //switch (edate.Month)
        //                        //{
        //                        //    case 1:
        //                        //    case 3:
        //                        //    case 5:
        //                        //    case 7:
        //                        //    case 8:
        //                        //    case 10:
        //                        //    case 12:
        //                        //        if (edate.Day == 31)
        //                        //            subMonth = Math.Abs(subMonth - 1);
        //                        //        break;
        //                        //    case 4:
        //                        //    case 6:
        //                        //    case 9:
        //                        //    case 11:
        //                        //        if (edate.Day == 30)
        //                        //            subMonth = Math.Abs(subMonth - 1);
        //                        //        break;
        //                        //    case 2:
        //                        //        if (DateTime.IsLeapYear(edate.Year) == true && edate.Day == 29)
        //                        //            subMonth = Math.Abs(subMonth - 1);
        //                        //        else if (DateTime.IsLeapYear(edate.Year) == false && edate.Day == 28)
        //                        //            subMonth = Math.Abs(subMonth - 1);
        //                        //        break;

        //                        //}


        //                        //int subMonth = edate.Month;
        //                        //if (subMonth < 6)
        //                        //{

        //                        //}

        //                        //新算法
        //                        Int32 iEffectMonthCount = subGetEffectMonthAmount(sdate, edate);

        //                        //依照有效月份數，折算本年度特休
        //                        if (iEffectMonthCount != 0)
        //                            subParserResult(iEffectMonthCount, ref sday3, ref sday3_hour);
        //                    }

        //                    //當年度復職,以年底減復職日計算
        //                    else if (int.Parse(txtYear) == edate.Year - 1911)
        //                    {
        //                        #region 由天數去計(備份保留)
        //                        //double TotalDays = 0;
        //                        //DateTime ThisYear = new DateTime(DateTime.Now.Year, 1, 1);
        //                        //TotalDays = new TimeSpan(edate.Ticks - ThisYear.Ticks).Days;

        //                        //switch (edate.Month)
        //                        //{
        //                        //    case 1:
        //                        //    case 3:
        //                        //    case 5:
        //                        //    case 7:
        //                        //    case 8:
        //                        //    case 10:
        //                        //    case 12:
        //                        //        if (edate.Day == 31)
        //                        //            TotalDays=TotalDays + 1;
        //                        //        break;
        //                        //    case 4:
        //                        //    case 6:
        //                        //    case 9:
        //                        //    case 11:
        //                        //        if (edate.Day == 30)
        //                        //            TotalDays = TotalDays + 1;
        //                        //        break;
        //                        //    case 2:
        //                        //        if (DateTime.IsLeapYear( edate.Year)==true && edate.Day == 29)
        //                        //            TotalDays = TotalDays + 1;
        //                        //        else if (DateTime.IsLeapYear(edate.Year) == false && edate.Day == 28)
        //                        //            TotalDays = TotalDays + 1;
        //                        //        break;

        //                        //}

        //                        ////subMonth = TotalDays / 30;

        //                        //subMonth=Convert.ToInt16(Math.Ceiling( TotalDays / 3));
        //                        #endregion 

        //                        //本年度無效月份總數(扣掉其他長假後)
        //                        //此值會被拿來做為特休天數的計算用
        //                        int iNoEffectMonthCount = 0;

        //                        //原始算法(未刪除，直接以新算法蓋過值，以方便進行偵測還原用)
        //                        DateTime ThisYear = new DateTime(DateTime.Now.Year, 1, 1); //為什麼是用Now?
        //                        iNoEffectMonthCount = edate.Year * 12 + edate.Month - (ThisYear.Year * 12 + ThisYear.Month);
        //                                              //前面曾經有對 edate + 1 year 過，故若為去年(2014)請的假，會變成 2015(2014+1)
        //                                              //若有年份差(例如2014、2015)，則經過上述公式後就會變成有 12 的差
        //                                              //但這部分應可透過月份的相加減即可算出，應不用透過上述計算式

        //                        /* 新算法
        //                         * 這邊的計算主要是進行月份的相減 */
        //                        Int32 iEffectMonthCount = subGetEffectMonthAmount(sdate, edate);

        //                        //依照有效月份數，折算本年度特休
        //                        if (iEffectMonthCount != 0)
        //                            subParserResult(iEffectMonthCount, ref sday3, ref sday3_hour);
        //                    }

        //                }

        //            //}
        //            #endregion

        //            string InsertSQL = "INSERT INTO C_DLTBB(MZ_YEAR,MZ_ID,MZ_HDAY,MZ_HTIME,MZ_SDAY,MZ_SDAY2,MZ_PDAY,MZ_SICKDAY,MZ_HCAREDAY) VALUES(@MZ_YEAR,@MZ_ID,@MZ_HDAY,@MZ_HTIME,@MZ_SDAY,@MZ_SDAY2,@MZ_PDAY,@MZ_SICKDAY,@MZ_HCAREDAY)";

        //            SqlParameter[] parameterList ={
        //            new SqlParameter("MZ_YEAR",SqlDbType.VarChar){Value=txtYear},
        //            new SqlParameter("MZ_ID",SqlDbType.VarChar){Value=dt.Rows[i]["MZ_ID"].ToString()},
        //            new SqlParameter("MZ_HDAY",SqlDbType.Float){Value=sday3},
        //            new SqlParameter("MZ_HTIME",SqlDbType.Float){Value=sday3_hour},
        //            new SqlParameter("MZ_SDAY",SqlDbType.Float){Value=string.IsNullOrEmpty(dt.Rows[i]["MZ_SDAY3"].ToString())?"0":dt.Rows[i]["MZ_SDAY3"].ToString()},
        //            new SqlParameter("MZ_SDAY2",SqlDbType.Float){Value=string.IsNullOrEmpty(dt.Rows[i]["MZ_SDAY"].ToString())?"0":dt.Rows[i]["MZ_SDAY"].ToString()},
        //            new SqlParameter("MZ_PDAY",SqlDbType.Float){Value=pday },
        //            new SqlParameter("MZ_SICKDAY",SqlDbType.Float){Value=sickday },
        //            new SqlParameter("MZ_HCAREDAY",SqlDbType.Float){Value=hcareday },

        //            };
        //            try
        //            {
        //                o_DBFactory.ABC_toTest.ExecuteNonQuery( InsertSQL, parameterList);
        //                if (i == dt.Rows.Count - 1)
        //                {
        //                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('計算完成');", true);
        //                }
        //            }
        //            catch (Exception)
        //            {
        //                throw;
        //            }
        //        }


        //    }
        //        #endregion 單人
        //}

        ///// <summary>有效月份數計算</summary>
        ///// <param name="dtStartDate">留職停薪起始日</param>
        ///// <param name="dtEndDate">留職停薪結束日</param>
        ///// <returns>有效月份數</returns>
        ///// 20150408 Neil 建立
        //private Int32 subGetEffectMonthAmount(DateTime dtStartDate, DateTime dtEndDate)
        //{
        //    DateTime dtClearStartDate = dtStartDate.AddMonths(1); //此月天數均略過，故+1
        //    DateTime dtClearEndDate = dtEndDate.AddMonths(-1); //此月天數均略過，故-1
        //    int iDiffDays = new TimeSpan(dtEndDate.Ticks - dtStartDate.Ticks).Days + 1; //兩個日期的差異天數

        //    //判斷年份是否跨年，若為跨年度復職則該年度計算休假時，扣掉包含去年度的部分
        //    //例如 102.08.05 ~ 103.04.05，無效月份數為 9, 10, 11 ,12, 1, 2, 3，故104年特休公式為 : 7 / 12
        //    //例外 : 當結束日期為該月最後一天時，該月份同樣也要算入無效月份數中
        //    Boolean isCrossYear = (dtEndDate.Year - dtStartDate.Year) == 0 ? false : true;
        //    Boolean isCrossTooMuchYear = (dtEndDate.Year - dtStartDate.Year >= 2) ? true : false;

        //    int iNoEffectMonthCount = 0;

        //    //無效月份數
        //    if (!isCrossYear)
        //        iNoEffectMonthCount = (dtClearEndDate.Month - dtClearStartDate.Month) + 1; //(後-前) + 1 為個數
        //    else {
        //        //若為跨年度則分為兩部分運算:
        //        //第一部份先判斷是否有年度跨越。用於跨超過兩年以上，例如 101.08 請到 103.02。若有則有效月份直接歸 0 (給12)
        //        //第二部分則拆算兩年之零散月份。例如 102.06 請到 103.06，則拆分成 102.06 ~ 102.12 及 103.01 ~ 103.06 兩部份計算
        //        if (isCrossTooMuchYear) iNoEffectMonthCount = 12;
        //        else
        //        {
        //            //先計算跨日期之上半部
        //            int iFrontPart = 0 ;
        //            if (dtStartDate.Month == 12)
        //                iFrontPart = 0; //若起始日期為12月，上半部直接給 0
        //            else
        //                iFrontPart = (12 - dtClearStartDate.Month) + 1;

        //            //後計算跨日期之下半部
        //            int iAfterPart = 0;
        //            if (dtEndDate.Month == 1)
        //                iAfterPart = 0; //若結束日期為1月，下半部直接給 0
        //            else
        //                iAfterPart = dtClearEndDate.Month; 

        //            iNoEffectMonthCount = iFrontPart + iAfterPart;
        //        }
        //    }

        //    //↑以上為無效月份原始數字計算
        //    //↓以下針對特殊情況進行調整

        //    //當為同月份時(8/__~8/__) (跨年度不用做同月份調整)
        //    if (!isCrossYear && dtStartDate.Month == dtEndDate.Month)
        //    {
        //        iNoEffectMonthCount = (iNoEffectMonthCount <= 0) ? 0 : iNoEffectMonthCount; //若為同月份相減則會為負數，故在此歸零
        //        int iThisMonthDayTotal = DateTime.DaysInMonth(dtStartDate.Year, dtStartDate.Month); ;

        //        //判斷日期差異數是否 >= 當月總天數(正常來講只會有 = 成立，不會有 > 成立)
        //        if (iDiffDays >= iThisMonthDayTotal)
        //        {
        //            //差異超過當月日數，故當月份一整個月都有請假，故+1
        //            iNoEffectMonthCount += 1;
        //        }
        //        else
        //        {
        //            //日期差異未滿當月時，即為少於一個月
        //            //原算式相減會為-1，故直接調整為 0
        //            iNoEffectMonthCount = 0; 
        //        }
        //    }
        //    //若為不同月份(就算跨年度也是要做月份調整)
        //    else
        //    {
        //        //判斷初始日期是否為當月1號
        //        //因此判斷式前提為月份不相等(else)，因此僅需判斷是否為1號開始，若是則把整月算入休假
        //        if (dtStartDate.Day == 1)
        //            iNoEffectMonthCount += 1;
        //    }

        //    Int32 iEffectMonthCount = 12 - iNoEffectMonthCount; //進行減法後，即為今年特休計算上的有效月數
        //    return iEffectMonthCount;
        //}

        ///// <summary>計算特休天數</summary>
        ///// <param name="iEffectMonthCount">當年度有效月份數</param>
        ///// <param name="iOriginalDay">原始特休天數</param>
        ///// <param name="iOriginalHours">原始特休小時數</param>
        ///// 20150408 Neil 建立
        //private void subParserResult(int iEffectMonthCount, ref int iOriginalDay, ref int iOriginalHours)
        //{
        //    //主要公式
        //    Double tmp = MathHelper.Round(iOriginalDay * iEffectMonthCount / float.Parse("12.0"), 1);

        //    String[] k = tmp.ToString().Split('.');
        //    iOriginalDay = int.Parse(k[0]);

        //    //若有小數點
        //    if (k.Count() >= 2)
        //        if (int.Parse(k[1]) > 5) //若小數第一位大於5則多給一天
        //            iOriginalDay += 1;
        //        else if (int.Parse(k[1]) > 0) //倘若沒大於5，但是有小數點，則多給 0.5 天 (一天 =8 Hour)
        //            iOriginalHours = 4;
        //}

        ///// <summary>
        ///// 更新基本資料的任職日期
        ///// </summary>
        ///// <param name="FDATE"></param>
        ///// <param name="ID"></param>
        ///// <param name="addMonth"></param>
        //protected void updateBasicOff(DateTime FDATE, string ID, int addMonth)
        //{
        //    int monthDiff = Convert.ToInt32(Microsoft.VisualBasic.DateAndTime.DateDiff("M", FDATE, DateTime.Now, Microsoft.VisualBasic.FirstDayOfWeek.Sunday, Microsoft.VisualBasic.FirstWeekOfYear.FirstFullWeek));

        //    int totalMonth = monthDiff + addMonth;

        //    string UpdateString = "UPDATE A_DLBASE SET MZ_OFFYY=" + (totalMonth / 12) + ",MZ_OFFMM=" + (totalMonth % 12) + " WHERE MZ_ID='" + ID + "'";

        //    try
        //    {
        //        o_DBFactory.ABC_toTest.Edit_Data(UpdateString);
        //    }
        //    catch
        //    {
        //    }

        //}
        #endregion
    }
}
