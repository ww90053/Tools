using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

namespace TPPDDB._3_forleave
{
    public partial class C_ForLeaveOvertime_ONDUTY_CHECK_UNIT : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                Label1.Text = "單位核定";


                C.check_power();              
                C.fill_AD(ddl_EXAD);
                chk_TPMGroup();
                //ddl_EXAD.Items.Insert(0, new ListItem("", ""));
                ddl_EXAD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                C.fill_unit(ddl_EXUNIT, ddl_EXAD.SelectedValue);
                //ddl_EXUNIT.Items.Insert(0, new ListItem("", ""));
                ddl_EXUNIT.SelectedValue = Session["ADPMZ_EXUNIT"].ToString();

                

              

            }
            //if (GridView1.Rows.Count > 0)
            //{
            //    Button_Check.Enabled = true;

            //}
            //else
            //{
            //    Button_Check.Enabled = false;
            //}


        }

        /// <summary>
        /// 群組權限
        /// </summary>
        protected void chk_TPMGroup()
        {

            switch (o_a_Function.strGID(Session["ADPMZ_ID"].ToString()))
            {
                case "TPMIDISAdmin":
                case "A":
                case "B":
                    break;
                case "C":
                    //matthew 中和分局進來要可以選中一中二
                    if (Session["ADPMZ_EXAD"].ToString() == "382133600C")
                    {
                        C.fill_DLL_New(ddl_EXAD);
                    }
                    else
                    {
                        ddl_EXAD.Enabled = false;
                    }
                    //ddl_EXAD.Enabled = false;
                    break;
                case "E":
                    //matthew 中和分局進來要可以選中一中二
                    if (Session["ADPMZ_EXAD"].ToString() == "382133600C")
                    {
                        C.fill_DLL_New(ddl_EXAD);
                    }
                    else
                    {
                        ddl_EXAD.Enabled = false;
                    }
                    //ddl_EXAD.Enabled = false;
                    ddl_EXUNIT.Enabled = false;
                    break;
                case "D":
                default:
                    HttpContext.Current.Response.Redirect("~/ErrorPage.aspx?erid=ADUXXX0001");
                    break;



            }


        }


        public void doSearch(string EXAD, string EXUNIT, string DATE1, string DATE2, string MZ_NAME, string MZ_ID, string IS_CHK)
        {



            string gvSelectCommand = @"SELECT CC.SN,CC.MZ_ID ,MZ_NAME,MZ_OCCC_CH, DATE_TAG ,C1.DUTY_KINDNAME ,  C2.DUTY_KINDNAME HOURTYPE, PAY , dbo.TO_CHAR(TIME_SATRT,'yyyy/MM/dd HH24:mi' )+ '~ '+ dbo.TO_CHAR(TIME_END,'yyyy/MM/dd HH24:mi' ) REAL_DATE ,
                                           CASE IS_OVERTIME_HOUR_INSIDE WHEN 'Y' THEN '捕休' ELSE '' END IS_OVERTIME_HOUR_INSIDE,
                                           CASE IS_POSITIVE WHEN '1' THEN '正值' ELSE '副值' END IS_POSITIVE,
                                           IS_CHK_UNIT ,MEMO
                                           FROM C_ONDUTY_DAY CC
                                           LEFT JOIN VW_A_DLBASE_S1 AD ON AD.MZ_ID=CC.MZ_ID
                                           LEFT JOIN C_ONDUTY_CODE C1  ON substr(CC.KIND,0,1)=C1.DUTY_KIND
                                           LEFT JOIN C_ONDUTY_CODE C2  ON CC.KIND=C2.DUTY_KIND
                                           WHERE 1=1";


            if (!string.IsNullOrEmpty(DATE2) && !string.IsNullOrEmpty(DATE1))
            {
                gvSelectCommand += "AND DATE_TAG>='" + DATE1 + "' AND DATE_TAG<='" + DATE2 + "'";
            }
            else if (!string.IsNullOrEmpty(DATE1) && !string.IsNullOrEmpty(DATE2))
            {
                gvSelectCommand += " AND DATE_TAG='" + DATE1 + "'";
            }


            if (!string.IsNullOrEmpty(EXAD))
            {
                gvSelectCommand += " AND CC.MZ_EXAD='" + EXAD + "'";
            }

            if (!string.IsNullOrEmpty(EXUNIT))
            {
                gvSelectCommand += " AND CC.MZ_EXUNIT='" + EXUNIT + "'";
            }

            if (!string.IsNullOrEmpty(MZ_ID))
            {
                gvSelectCommand += " AND CC.MZ_ID='" + MZ_ID + "'";
            }


            if (!string.IsNullOrEmpty(MZ_NAME))
            {
                gvSelectCommand += " AND MZ_NAME LIKE'" + MZ_NAME + "%'";
            }
            //2013/09/26
            if (!string.IsNullOrEmpty(IS_CHK))
            {
                gvSelectCommand += "AND IS_CHK_UNIT='" + IS_CHK + "' ";

            }
            //2013/09/26
            gvSelectCommand += " ORDER BY DATE_TAG DESC,CC.MZ_ID";

            DataTable Search_dt = o_DBFactory.ABC_toTest.Create_Table(gvSelectCommand, "get");
            GridView1.DataSource = Search_dt;
            GridView1.DataBind();

        }

        protected void Button_Check_Click(object sender, EventArgs e)
        {
            //string SN_Y = "";
            //string SN_N = "";

            List<string> SN_Y = new List<string>();
            List<string> SN_N = new List<string>();

            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                Label lb_SN = (Label)GridView1.Rows[i].Cells[0].FindControl("lb_SN");
                Label lb_CHK = (Label)GridView1.Rows[i].Cells[0].FindControl("lb_CHK");
                CheckBox CheckBox_CHK1 = GridView1.Rows[i].FindControl("CheckBox_CHK1") as CheckBox;
                if (CheckBox_CHK1.Checked)
                {
                    //SN_Y += lb_SN.Text + ",";
                    if (lb_CHK.Text == "N")
                        SN_Y.Add(lb_SN.Text);

                }
                else
                {
                    if (lb_CHK.Text == "Y")
                        SN_N.Add(lb_SN.Text);
                    //SN_N += lb_SN.Text + ",";
                }

               

            }


            string Updatesql_Y = "UPDATE C_ONDUTY_DAY SET IS_CHK_UNIT=@IS_CHK_UNIT,CHKUSER_UNIT=@CHKUSER_UNIT , CHKDATE_UNIT=@CHKDATE_UNIT WHERE SN IN ({0})";
            string Updatesql_N = "UPDATE C_ONDUTY_DAY SET IS_CHK_UNIT=@IS_CHK_UNIT,CHKUSER_UNIT=@CHKUSER_UNIT , CHKDATE_UNIT=@CHKDATE_UNIT WHERE SN IN ({0})";

            Updatesql_Y = string.Format(Updatesql_Y, string.Join(",", SN_Y.ToArray()));
            Updatesql_N = string.Format(Updatesql_N, string.Join(",", SN_N.ToArray()));



            
            List<SqlParameter> parY = new List<SqlParameter>();
            //parY.Add(new SqlParameter("SN", SqlDbType.Float) { Value = YY});
            parY.Add(new SqlParameter("IS_CHK_UNIT", SqlDbType.VarChar) { Value = "Y" });
            parY.Add(new SqlParameter("CHKDATE_UNIT", SqlDbType.DateTime) { Value = DateTime.Now });
            parY.Add(new SqlParameter("CHKUSER_UNIT", SqlDbType.VarChar) { Value = Session["ADPMZ_ID"].ToString() });

            List<SqlParameter> parN = new List<SqlParameter>();
            //parN.Add(new SqlParameter("SN", SqlDbType.Float) { Value = NN });
            parN.Add(new SqlParameter("IS_CHK_UNIT", SqlDbType.VarChar) { Value = "N" });
            parN.Add(new SqlParameter("CHKDATE_UNIT", SqlDbType.DateTime) { Value = DateTime.Now });
            parN.Add(new SqlParameter("CHKUSER_UNIT", SqlDbType.VarChar) { Value = Session["ADPMZ_ID"].ToString() });

            try
            {
                if (SN_Y.Count > 0)
                    o_DBFactory.ABC_toTest.SQLExecute(Updatesql_Y, parY);
                if (SN_N.Count > 0)
                    o_DBFactory.ABC_toTest.SQLExecute(Updatesql_N, parN);

                //if (i == GridView1.Rows.Count - 1)
                //{
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('核定成功！');", true);
                //doSearch(ddl_EXAD.SelectedValue, ddl_EXUNIT.SelectedValue, txt_DATE1.Text, txt_DATE2.Text, txt_NAME.Text, txt_ID.Text, rbl_chk.SelectedValue);

                //}
            }
            catch (Exception)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('核定失敗！');", true);

                throw;
            }
            //}




        }

        protected void btSearch_Show_Click(object sender, EventArgs e)
        {

            txt_ID.Text = "";
            txt_NAME.Text = "";

            txt_DATE1.Text = (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0') + (DateTime.Now.Month).ToString().PadLeft(2, '0') + "01";
            txt_DATE2.Text = (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0') + (DateTime.Now.Month).ToString().PadLeft(2, '0') + DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            //txt_DATE1.Text = "";
            //txt_DATE2.Text = "";
            pl_Search_ModalPopupExtender.Show();
        }



        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }



        protected void ddl_EXAD_SelectedIndexChanged(object sender, EventArgs e)
        {
            C.fill_unit(ddl_EXUNIT, ddl_EXAD.SelectedValue);
            pl_Search_ModalPopupExtender.Show();
        }

        protected void btn_Search_Click(object sender, EventArgs e)
        {

            string error = "";
            if (string.IsNullOrEmpty(txt_DATE1.Text) || string.IsNullOrEmpty(txt_DATE2.Text))
                error += @"請輸入起迄日期\r\n";
            else
            {
                if (!TPPDDB.App_Code.DateManange.Check_date(txt_DATE1.Text) || !TPPDDB.App_Code.DateManange.Check_date(txt_DATE2.Text))
                {
                    error += @"日期格式錯誤\r\n";
                }
            }

            if (!string.IsNullOrEmpty(error))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('" + error + "');", true);
                pl_Search_ModalPopupExtender.Show();
                return;
            }


            doSearch(ddl_EXAD.SelectedValue, ddl_EXUNIT.SelectedValue, txt_DATE1.Text, txt_DATE2.Text, txt_NAME.Text, txt_ID.Text, rbl_chk.SelectedValue);
        }
    }
}
