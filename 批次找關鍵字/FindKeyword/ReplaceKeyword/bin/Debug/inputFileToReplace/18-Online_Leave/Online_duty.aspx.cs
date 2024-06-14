using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Data;


namespace TPPDDB._18_Online_Leave
{
    public partial class Online_duty : System.Web.UI.Page
    {
        string strSQL = string.Empty;
        DataTable temp = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //Session["ADPMZ_ID"] = "A225101494";
                //Session["ADPMZ_EXAD"] = "382130000C";//機關代碼
                //Session["ADPMZ_EXUNIT"] = "0133";//現服次單位
                anewAllSearch();
            }
        }
        protected void anewAllSearch()
        {
            int countDuty = int.Parse(o_DBFactory.ABC_toTest.vExecSQL(string.Format(@"select count(*) from C_OVERTIME_HOUR_INSIDE 
                                                                          where SC_ID is null and (OVERTIME_STATUS != 1 or OVERTIME_STATUS IS NULL)
                                                                          and MZ_EXAD='{0}' order by SN ", Session["ADPMZ_EXAD"].ToString())));
            if (countDuty > 0)
            {
                lbtn_duty.Text = "勤務中心線上簽核 ( " + countDuty + " ) ";
                lbtn_duty.Enabled = true;
            }
            else
            {
                lbtn_duty.Text = "勤務中心線上簽核 ( " + countDuty + " ) ";
                lbtn_duty.Enabled = false;
            }
        }
        /// <summary>
        /// sc_id未審核資料
        /// </summary>
        protected DataTable dutySginSearch()
        {
            strSQL = string.Format(@"select * from C_OVERTIME_HOUR_INSIDE where SC_ID is null 
                      and (OVERTIME_STATUS != 1 or OVERTIME_STATUS IS NULL) and MZ_EXAD='{0}'  order by MZ_DATE ", Session["ADPMZ_EXAD"].ToString());
            temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "dutySgin");
            return temp;
        }
        /// <summary>
        /// 歷程查詢
        /// </summary>
        protected DataTable dutyHistorySearch(string overtimeSN)
        {
            strSQL = "SELECT C_OVERTIME_CHECKFLOW.*,C_OVERTIME_HOUR_INSIDE.MZ_EXUNIT,"
                + " C_OVERTIME_HOUR_INSIDE.MZ_ID,C_OVERTIME_HOUR_INSIDE.MZ_OCCC,C_OVERTIME_HOUR_INSIDE.OTIME,"
                + " C_OVERTIME_HOUR_INSIDE.HOUR_PAY,C_OVERTIME_HOUR_INSIDE.PAY_SUM,C_OVERTIME_HOUR_INSIDE.OTREASON,"
                + " C_OVERTIME_HOUR_INSIDE.INS_ID,C_OVERTIME_HOUR_INSIDE.INS_DATE,C_OVERTIME_HOUR_INSIDE.MZ_EXAD,"
                + " C_OVERTIME_HOUR_INSIDE.MZ_DATE,C_OVERTIME_HOUR_INSIDE.MZ_RESTHOUR,C_OVERTIME_HOUR_INSIDE.OVERTIME_STATUS,"
                + " C_OVERTIME_HOUR_INSIDE.SC_ID,C_STATUS.C_STATUS_NAME FROM C_OVERTIME_CHECKFLOW INNER JOIN C_OVERTIME_HOUR_INSIDE ON "
                + " C_OVERTIME_CHECKFLOW.OVERTIME_SN=C_OVERTIME_HOUR_INSIDE.SN INNER JOIN C_STATUS ON "
                + " C_OVERTIME_CHECKFLOW.PROCESS_STATUS = C_STATUS.C_STATUS_SN WHERE C_OVERTIME_CHECKFLOW.OVERTIME_SN="
                + int.Parse(overtimeSN) + " AND C_OVERTIME_CHECKFLOW.PROCESS_DATE IS NOT NULL "
                + "ORDER BY C_OVERTIME_CHECKFLOW.O_SN";
            temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "dutyHistory");
            return temp;
        }
        protected void lbtn_duty_Click(object sender, EventArgs e)
        {
            gv_duty.DataSource = dutySginSearch();
            gv_duty.DataBind();
        }
        //加班審核gv
        protected void gv_duty_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label ot_date = (Label)e.Row.FindControl("lbl_date_ot");
                Label ot_name = (Label)e.Row.FindControl("lbl_name_ot");
                Label ot_sumhour = (Label)e.Row.FindControl("lbl_sumhour_ot");
                Button SN = (Button)e.Row.FindControl("btn_history_ot");
                Label ot_s = (Label)e.Row.FindControl("lbl_s");
                Label card = (Label)e.Row.FindControl("lbl_sumhour_card");

                strSQL = "select * from C_OVERTIME_HOUR_INSIDE WHERE SN=" + int.Parse(SN.CommandArgument);
                temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "ot");

                if (temp.Rows.Count > 0)
                {

                    //日期
                    ot_date.Text = temp.Rows[0]["MZ_DATE"].ToString().Substring(0, 3) + "/" +
                                   temp.Rows[0]["MZ_DATE"].ToString().Substring(3, 2) + "/" +
                                   temp.Rows[0]["MZ_DATE"].ToString().Substring(5, 2);

                    string LOGDATESTRING = (int.Parse(temp.Rows[0]["MZ_DATE"].ToString().Substring(0, 3)) + 1911).ToString() + "/" +
                                           temp.Rows[0]["MZ_DATE"].ToString().Substring(3, 2) + "/" +
                                           temp.Rows[0]["MZ_DATE"].ToString().Substring(5, 2);
                    //假單者
                    ot_name.Text = o_A_DLBASE.CNAME(temp.Rows[0]["MZ_ID"].ToString());

                    //已加班時數
                    string overtimesum = o_DBFactory.ABC_toTest.vExecSQL(" SELECT SUM(OTIME) FROM C_OVERTIME_HOUR_INSIDE " +
                                                             " WHERE MZ_ID='" + temp.Rows[0]["MZ_ID"].ToString()
                                                           + "'AND OVERTIME_STATUS = 2" +
                                                             " AND dbo.SUBSTR(MZ_DATE,1,5)='" + temp.Rows[0]["MZ_DATE"].ToString().Substring(0, 5) + "'").ToString();

                    string selectINTIME = o_DBFactory.ABC_toTest.vExecSQL("SELECT LOGTIME FROM (SELECT LOGTIME,ROW_NUMBER() OVER(ORDER BY LOGTIME) AS ROWCOUNT FROM C_CARDHISTORY_NEW WHERE dbo.SUBSTR(USERNAME,0,10)='" + temp.Rows[0]["MZ_ID"].ToString() + "' AND LOGDATE='" + LOGDATESTRING + "' AND VERIFY='IN') WHERE ROWCOUNT=1");

                    string selectOUTTIME = o_DBFactory.ABC_toTest.vExecSQL("SELECT LOGTIME FROM (SELECT LOGTIME,ROW_NUMBER() OVER(ORDER BY LOGTIME DESC) AS ROWCOUNT FROM C_CARDHISTORY_NEW WHERE dbo.SUBSTR(USERNAME,0,10)='" + temp.Rows[0]["MZ_ID"].ToString() + "' AND LOGDATE='" + LOGDATESTRING + "' AND VERIFY='IN') WHERE ROWCOUNT=1");


                    string searchNULL = string.Empty;
                    if (temp.Rows[0]["OVERTIME_STATUS"].ToString() == "")
                    {
                        searchNULL = "4";
                    }
                    else
                    {
                        searchNULL = temp.Rows[0]["OVERTIME_STATUS"].ToString();
                    }
                    ot_s.Text = o_DBFactory.ABC_toTest.vExecSQL("select C_STATUS_NAME from C_STATUS where C_STATUS_SN =" + int.Parse(searchNULL));


                    if (overtimesum == "" || overtimesum == null)
                        ot_sumhour.Text = "0";
                    else
                        ot_sumhour.Text = overtimesum;

                }
            }
        }
        //加班歷程gv
        protected void btn_history_ot_Command(object sender, CommandEventArgs e)
        {
            gv_history.DataSource = dutyHistorySearch(e.CommandArgument.ToString());
            gv_history.DataBind();
            gv_history.Visible = true;
        }
        /// <summary>
        /// 修改加班單C_OVERTIME_HOUR_INSIDE  SC_ID
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dutyUPDATE(string overtimeSN, string status)
        {
            strSQL = "UPDATE C_OVERTIME_HOUR_INSIDE SET "
                    + " SC_ID=@SC_ID,OVERTIME_STATUS=@OVERTIME_STATUS "
                    + " WHERE SN = " + int.Parse(overtimeSN);
            SqlParameter[] sp = {
                                    new SqlParameter("SC_ID",SqlDbType.VarChar ){Value = Session["ADPMZ_ID"].ToString ()},
                                    new SqlParameter ("OVERTIME_STATUS",SqlDbType.Float){Value = status}
                                   };
            o_DBFactory.ABC_toTest.ExecuteNonQuery( strSQL, sp);
        }
        /// <summary>
        /// 新增加班歷程C_OVERTIME_CHECKFLOW (加班單SN,狀態)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dutyADD(string overtimeSN, string status)
        {
            string today = (System.Convert.ToInt32(DateTime.Now.Year.ToString()) - 1911).ToString()
                            + DateTime.Now.ToString("MMdd");
            string now = DateTime.Now.ToString("HH:mm:ss");

            strSQL = "INSERT INTO C_OVERTIME_CHECKFLOW("
                 + "O_SN,OVERTIME_SN,REVIEW_ID,LETTER_DATE,PROCESS_DATE,"
                 + "REVIEW_MESSAGE,PROCESS_TIME,PROCESS_STATUS,SCHEDULE_SORT)"
                 + "VALUES("
                 + "C_OVERTIME_CHECKFLOW_SN.NEXTVAL,@OVERTIME_SN,@REVIEW_ID,@LETTER_DATE,"
                 + ":PROCESS_DATE,@REVIEW_MESSAGE,@PROCESS_TIME,"
                 + ":PROCESS_STATUS,@SCHEDULE_SORT)";
            SqlParameter[] sp = {
                                    new SqlParameter ("OVERTIME_SN",SqlDbType.Float )
                                        {Value = overtimeSN },
                                    new SqlParameter ("REVIEW_ID",SqlDbType .VarChar )
                                        {Value = Session["ADPMZ_ID"].ToString ()},
                                    new SqlParameter ("LETTER_DATE",SqlDbType .VarChar )
                                        {Value = o_Function .vExecSQL ("select INS_DATE from C_OVERTIME_HOUR_INSIDE where SN=" + overtimeSN)},
                                    new SqlParameter ("PROCESS_DATE",SqlDbType .VarChar )
                                        {Value = today},
                                    new SqlParameter ("REVIEW_MESSAGE",SqlDbType .VarChar )
                                        {Value = txt_message.Text },
                                    new SqlParameter ("PROCESS_TIME",SqlDbType .VarChar )
                                        {Value = now},
                                    new SqlParameter ("PROCESS_STATUS",SqlDbType .VarChar )
                                        {Value = o_Function .vExecSQL ("SELECT C_STATUS_SN FROM C_STATUS WHERE C_STATUS_NAME='" +status +"'")},
                                    new SqlParameter("SCHEDULE_SORT",SqlDbType.Float )
                                        {Value = 2} //2勤務
                                         };
            o_DBFactory.ABC_toTest.ExecuteNonQuery( strSQL, sp);

        }
        protected void gv_history_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label date = (Label)e.Row.FindControl("lbl_OTDATE");
                Label name = (Label)e.Row.FindControl("lbl_OTNAME");
                Label exad = (Label)e.Row.FindControl("lbl_overtime_exad");
                Label exunit = (Label)e.Row.FindControl("lbl_overtime_exunit");
                Label occc = (Label)e.Row.FindControl("lbl_occc_history");
                Button O_SN = (Button)e.Row.FindControl("btn_osn");

                temp = o_DBFactory.ABC_toTest.Create_Table("select * from C_OVERTIME_CHECKFLOW WHERE O_SN=" + int.Parse(O_SN.CommandArgument.ToString()), "a");
                string sort = temp.Rows[0]["SCHEDULE_SORT"].ToString();
                if (int.Parse(sort) > 0 && int.Parse(sort) < 3) //代表是第二層 1、2 = 2層 3=1層(承辦人)
                    sort = "2";
                else
                    sort = "1";

                //單位
                exunit.Text = o_DBFactory.ABC_toTest.vExecSQL("select MZ_KCHI from A_KTYPE where MZ_KTYPE = '25' and MZ_KCODE ='" +
                         (o_DBFactory.ABC_toTest.vExecSQL("select MZ_EXUNIT from C_REVIEW_MANAGEMENT where MZ_ID = '" + temp.Rows[0]["REVIEW_ID"].ToString()
                         + "' and REVIEW_LEVEL= '" + sort + "'")) + "'");
                //機關
                exad.Text = o_DBFactory.ABC_toTest.vExecSQL("select MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE = '04' AND  MZ_KCODE ='" +
                            (o_DBFactory.ABC_toTest.vExecSQL("select MZ_EXAD from C_REVIEW_MANAGEMENT where MZ_ID ='" + temp.Rows[0]["REVIEW_ID"].ToString()
                            + "' AND REVIEW_LEVEL= '" + sort + "'")) + "'");
                occc.Text = o_DBFactory.ABC_toTest.vExecSQL("select MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE = '26' AND  MZ_KCODE ='" +
                            (o_DBFactory.ABC_toTest.vExecSQL("select MZ_OCCC from C_REVIEW_MANAGEMENT where MZ_ID ='" + temp.Rows[0]["REVIEW_ID"].ToString()
                            + "' AND REVIEW_LEVEL= '" + sort + "'")) + "'");

                //審核者姓名
                name.Text = o_A_DLBASE.CNAME(temp.Rows[0]["REVIEW_ID"].ToString());
                date.Text = temp.Rows[0]["PROCESS_DATE"].ToString().Substring(0, 3) + "/" +
                        temp.Rows[0]["PROCESS_DATE"].ToString().Substring(3, 2) + "/" +
                        temp.Rows[0]["PROCESS_DATE"].ToString().Substring(5, 2) + " " +
                            temp.Rows[0]["PROCESS_TIME"].ToString();

            }
        }

        protected void btn_decision_Click(object sender, EventArgs e)
        {
            txt_message.Text = "";
            for (int i = 0; i < gv_duty.Rows.Count; i++)
            {
                CheckBox overtimeCK = (CheckBox)gv_duty.Rows[i].FindControl("ck_select_ot");
                if (overtimeCK.Checked == true)
                {
                    string COUNT_HISTORY = o_DBFactory.ABC_toTest.vExecSQL("select count(*) from C_OVERTIME_CHECKFLOW where " +
                        " OVERTIME_SN=" + int.Parse(gv_duty.DataKeys[i]["SN"].ToString()) + "and PROCESS_STATUS=2"); //2是決行的意思
                    //是否有決行  (歷程+2)
                    if (COUNT_HISTORY != "0")  //代表主管已審核且決行。
                    {
                        dutyUPDATE(gv_duty.DataKeys[i]["SN"].ToString(), "2"); //2=決行
                        //新增勤務的歷程
                        dutyADD(gv_duty.DataKeys[i]["SN"].ToString(), btn_decision.Text.ToString());
                        //新增給人事室的歷程
                        //dutyADD(gv_duty.DataKeys[i]["SN"].ToString(), btn_decision.Text.ToString(), 2);
                    }
                    else
                    {
                        dutyUPDATE(gv_duty.DataKeys[i]["SN"].ToString(), "4"); //4=待審中
                        dutyADD(gv_duty.DataKeys[i]["SN"].ToString(), btn_decision.Text.ToString());
                    }
                }
            }
            gv_history.Visible = false;
            gv_duty.DataSource = dutySginSearch();
            gv_duty.DataBind();
            anewAllSearch();
        }

        protected void btn_return_Click(object sender, EventArgs e)
        {
            int p = 0;
            int back = 0;
            for (int i = 0; i < gv_duty.Rows.Count; i++)
            {
                CheckBox overtimeCK = (CheckBox)gv_duty.Rows[i].FindControl("ck_select_ot");
                if (overtimeCK.Checked == true)
                {
                    ViewState["rows_count"] = p.ToString();
                    ViewState["cell" + p] = gv_duty.DataKeys[i]["SN"].ToString();
                    p++;

                    back = 1;
                }
            }
            if (back > 0)
            {
                txt_message.Text = "";
                pl_message.GroupingText = "退回原因";
                M_P_E_MESSAGE.Show();
            }
            gv_history.Visible = false;
        }

        protected void btn_message_yes(object sender, EventArgs e)
        {
            for (int i = 0; i < int.Parse(ViewState["rows_count"].ToString()) + 1; i++)
            {
                dutyUPDATE(ViewState["cell" + i].ToString(), "1");
                dutyADD(ViewState["cell" + i].ToString(), btn_return.Text.ToString());
            }
            anewAllSearch();
            gv_duty.DataSource = dutySginSearch();
            gv_duty.DataBind();

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "btn_message", "alert('確定修改！');", true);
        }
    }
}
