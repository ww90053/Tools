using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Data;


namespace TPPDDB._3_forleave
{
    public partial class C_ForLeaveOvertime_Leave_Check : System.Web.UI.Page
    {
       
        //2013/09/26
        string is_check = "";
        //

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                C.check_power();
            
            }


            //查詢ID
            HttpCookie ForLeaveBasic_ID_Cookie = new HttpCookie("ForLeaveBasicSearch_ID");
            ForLeaveBasic_ID_Cookie = Request.Cookies["ForLeaveBasicSearch_ID"];

            if (ForLeaveBasic_ID_Cookie == null)
            {
                ViewState["ID"] = null;
                Response.Cookies["ForLeaveBasicSearch_ID"].Expires = DateTime.Now.AddYears(-1);
            }
            else
            {
                ViewState["ID"] = TPMPermissions._strDecod(ForLeaveBasic_ID_Cookie.Value.ToString());
                Response.Cookies["ForLeaveBasicSearch_ID"].Expires = DateTime.Now.AddYears(-1);
            }

            //查詢姓名
            HttpCookie ForLeaveBasic_NAME_Cookie = new HttpCookie("ForLeaveBasicSearch_NAME");
            ForLeaveBasic_NAME_Cookie = Request.Cookies["ForLeaveBasicSearch_NAME"];

            if (ForLeaveBasic_NAME_Cookie == null)
            {
                ViewState["NAME"] = null;
                Response.Cookies["ForLeaveBasicSearch_NAME"].Expires = DateTime.Now.AddYears(-1);
            }
            else
            {
                ViewState["NAME"] = TPMPermissions._strDecod(ForLeaveBasic_NAME_Cookie.Value.ToString());
                Response.Cookies["ForLeaveBasicSearch_NAME"].Expires = DateTime.Now.AddYears(-1);
            }

            ViewState["MZ_EXAD"] = Request["MZ_EXAD"];
            ViewState["MZ_EXUNIT"] = Request["MZ_EXUNIT"];
            ViewState["MZ_IDATE1"] = Request["MZ_IDATE1"];
            ViewState["MZ_IDATE2"] = Request["MZ_IDATE2"];
            //2013/09/26
            is_check = Request["ISCHECK"];


            if (!IsPostBack)
            {
                if (ViewState["MZ_EXAD"] != null)
                {
                    Label1.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE='" + ViewState["MZ_EXAD"].ToString() + "' AND MZ_KTYPE='04' ") + "差假核定";


                    string gvSelectCommand = "SELECT  (CASE WHEN SIGN_KIND=1 THEN '紙本' ELSE '線上' END) SIGN_KIND,MZ_CODE," +
                                                      "MZ_ID,MZ_CHK1," +
                                                      "(SELECT MZ_NAME FROM A_DLBASE WHERE MZ_ID=C_DLTB01.MZ_ID) AS MZ_NAME," +
                                                      "(SELECT MZ_CNAME FROM C_DLCODE WHERE MZ_CODE=C_DLTB01.MZ_CODE) AS MZ_CODE_NAME,MZ_CAUSE,MZ_RNAME," +
                                                      "CONVERT(VARCHAR(200),dbo.to_number(dbo.SUBSTR(MZ_IDATE1,1,3)))+'/'+dbo.SUBSTR(MZ_IDATE1,4,2)+'/'+dbo.SUBSTR(MZ_IDATE1,6,2) AS MZ_IDATE1,MZ_ITIME1," +
                                                      "CONVERT(VARCHAR(200),dbo.to_number(dbo.SUBSTR(MZ_ODATE,1,3)))+'/'+dbo.SUBSTR(MZ_ODATE,4,2)+'/'+dbo.SUBSTR(MZ_ODATE,6,2) AS MZ_ODATE,MZ_OTIME,MZ_TDAY,MZ_TTIME " +
                                             "FROM " +
                                                      "C_DLTB01 " +
                                             "WHERE 1=1";


                    if (ViewState["MZ_IDATE2"].ToString() == "0000000")
                    {
                        gvSelectCommand += " AND MZ_IDATE1='" + ViewState["MZ_IDATE1"].ToString() + "'";
                    }
                        //20150226
                    else if (ViewState["MZ_IDATE1"].ToString() == ViewState["MZ_IDATE2"].ToString())
                    {
                        gvSelectCommand += "AND MZ_IDATE1>='" + ViewState["MZ_IDATE1"].ToString() + "' AND MZ_IDATE1<='" + (int.Parse(ViewState["MZ_IDATE2"].ToString()) + 1).ToString() + "'";
                  
                    }
                    else
                    {
                        gvSelectCommand += "AND MZ_IDATE1>='" + ViewState["MZ_IDATE1"].ToString() + "' AND MZ_IDATE1<='" + ViewState["MZ_IDATE2"].ToString() + "'";
                    }

                    if (ViewState["MZ_EXAD"].ToString() != "")
                    {
                        gvSelectCommand += " AND MZ_EXAD='" + ViewState["MZ_EXAD"].ToString() + "'";
                    }

                    if (ViewState["MZ_EXUNIT"].ToString() != "")
                    {
                        gvSelectCommand += " AND MZ_EXUNIT='" + ViewState["MZ_EXUNIT"].ToString() + "'";
                    }

                    if (ViewState["ID"].ToString() != "")
                    {
                        gvSelectCommand += " AND MZ_ID='" + ViewState["ID"].ToString() + "'";
                    }


                    if (ViewState["NAME"].ToString() != "")
                    {
                        gvSelectCommand += " AND MZ_NAME LIKE'" + ViewState["NAME"].ToString() + "%'";
                    }
                    //2013/09/26
                    if (!string.IsNullOrEmpty(is_check))
                    {
                        //2013/10/03
                        if (is_check == "N")
                        {
                            gvSelectCommand += "AND (MZ_CHK1='N' OR MZ_CHK1 IS NULL)";
                        }
                        else
                            //2013/10/03
                        gvSelectCommand += "AND MZ_CHK1='" + is_check + "' ";
                    
                    }
                    //2013/09/26
                    gvSelectCommand += " ORDER BY MZ_IDATE1 DESC,MZ_ID";

                    SqlDataSource2.SelectParameters.Clear();
                    SqlDataSource2.SelectCommand = gvSelectCommand;

                    GridView1.DataBind();
                }
            }
        }

       

        protected void Button_Check_Click(object sender, EventArgs e)
        {
            using (SqlConnection Updateconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Updateconn.Open();
                SqlTransaction Trans1 = Updateconn.BeginTransaction();

                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    if ((GridView1.Rows[i].FindControl("CheckBox_CHK1") as CheckBox).Checked)
                    {
                        string Updatesql = "UPDATE C_DLTB01 SET MZ_CHK1='Y',MUSER='" + Session["ADPMZ_ID"] + "' , MDATE='" + (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0') + (DateTime.Now.Month).ToString().PadLeft(2, '0') + (DateTime.Now.Day).ToString().PadLeft(2, '0') + "' WHERE MZ_CODE='" + GridView1.Rows[i].Cells[0].Text.Trim() + "' AND MZ_ID='" + GridView1.Rows[i].Cells[3].Text.Trim() + "' AND MZ_IDATE1='" + GridView1.Rows[i].Cells[8].Text.Trim().Replace("/", "").PadLeft(7, '0') + "' AND MZ_ITIME1='" + GridView1.Rows[i].Cells[9].Text.Trim() + "'";

                        string Update_Lock_Flag = "";

                        if (GridView1.Rows[i].Cells[0].Text == "15")
                            Update_Lock_Flag = "UPDATE C_OVERTIME_HOUR_INSIDE SET LOCK_FLAG='Y' WHERE MZ_ID='" + GridView1.Rows[i].Cells[3].Text + "' AND MZ_IDATE='" + GridView1.Rows[i].Cells[7].Text.Replace("/", string.Empty).PadLeft(7, '0') + "'";

                        SqlCommand Updatecmd = new SqlCommand(Updatesql, Updateconn);

                        SqlCommand Update_Lock_Flag_cmd = new SqlCommand(Update_Lock_Flag, Updateconn);

                        Updatecmd.Transaction = Trans1;

                        Update_Lock_Flag_cmd.Transaction = Trans1;

                        try
                        {
                            Updatecmd.ExecuteNonQuery();

                            if (!string.IsNullOrEmpty(Update_Lock_Flag))
                                Update_Lock_Flag_cmd.ExecuteNonQuery();

                            if (i == GridView1.Rows.Count - 1)
                            {
                                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('核定成功！');location.href('C_ForLeaveOvertime_Leave_Check.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "')", true);

                                Trans1.Commit();
                            }
                        }
                        catch (Exception)
                        {
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('核定失敗！');location.href('C_ForLeaveOvertime_Leave_Check.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "')", true);
                            Trans1.Rollback();
                            throw;
                        }
                    }
                    else
                    {
                        string Updatesql = "UPDATE C_DLTB01 SET MZ_CHK1='N',MUSER='" + Session["ADPMZ_ID"].ToString() + "' , MDATE='" + (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0') + (DateTime.Now.Month).ToString().PadLeft(2, '0') + (DateTime.Now.Day).ToString().PadLeft(2, '0') + "' WHERE MZ_CODE='" + GridView1.Rows[i].Cells[0].Text.Trim() + "' AND MZ_ID='" + GridView1.Rows[i].Cells[3].Text.Trim() + "' AND MZ_IDATE1='" + GridView1.Rows[i].Cells[8].Text.Trim().Replace("/", "").PadLeft(7, '0') + "' AND MZ_ITIME1='" + GridView1.Rows[i].Cells[9].Text.Trim() + "'";

                        string Update_Lock_Flag = "";

                        if (GridView1.Rows[i].Cells[0].Text == "15")
                            Update_Lock_Flag = "UPDATE C_OVERTIME_HOUR_INSIDE SET LOCK_FLAG='N' WHERE MZ_ID='" + GridView1.Rows[i].Cells[3].Text + "' AND MZ_IDATE='" + GridView1.Rows[i].Cells[7].Text.Replace("/", string.Empty).PadLeft(7, '0') + "'";

                        SqlCommand Updatecmd = new SqlCommand(Updatesql, Updateconn);

                        SqlCommand Update_Lock_Flag_cmd = new SqlCommand(Update_Lock_Flag, Updateconn);

                        Updatecmd.Transaction = Trans1;

                        Update_Lock_Flag_cmd.Transaction = Trans1;

                        try
                        {
                            Updatecmd.ExecuteNonQuery();

                            if (!string.IsNullOrEmpty(Update_Lock_Flag))
                                Update_Lock_Flag_cmd.ExecuteNonQuery();

                            if (i == GridView1.Rows.Count - 1)
                            {
                                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('核定成功！');location.href('C_ForLeaveOvertime_Leave_Check.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "')", true);

                                Trans1.Commit();
                            }
                        }
                        catch (Exception)
                        {
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('核定失敗！');location.href('C_ForLeaveOvertime_Leave_Check.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "')", true);
                            Trans1.Rollback();
                            throw;
                        }
                    }
                }
            }
        }

        protected void btSearch_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('C_ForLeaveBasicSearch1.aspx?TableName=CHECK&TPM_FION=" + Request.QueryString["TPM_FION"] + "','查詢','top=190,left=200,width=420,height=250,toolbar=0,menubar=0,location=0,directories=0,status=0,scrollbars=yes');", true);
        }

        protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((CheckBox)sender).NamingContainer;

            GridView gv = (GridView)gvr.NamingContainer;

            if ((gv.HeaderRow.FindControl("CheckBox1") as CheckBox).Checked)
            {
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    (GridView1.Rows[i].FindControl("CheckBox_CHK1") as CheckBox).Checked = true;
                }
            }
            else
            {
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    (GridView1.Rows[i].FindControl("CheckBox_CHK1") as CheckBox).Checked = false;
                }
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
                e.Row.Cells[0].Attributes.Add("style", "display:none");

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[6].ToolTip = e.Row.Cells[6].Text;
                try
                {
                    e.Row.Cells[6].Text = e.Row.Cells[6].Text.Substring(0, 25);
                }
                catch { }
            }
        }

        protected void btnException_Click(object sender, EventArgs e)
        {
            string strSQL = "update c_dltb01 set mz_chk1='Y' where mz_dltb01_sn in (select mz_dltb01_sn from C_DLTB01 where  (MZ_CHK1<>'Y' OR MZ_CHK1 IS NULL) and sign_kind=2 and (select process_status  from (select * from C_LEAVE_HISTORY order by sn desc) where dltb01_sn=mz_dltb01_sn and rownum=1) =7 )";
            o_DBFactory.ABC_toTest.Edit_Data(strSQL);

            Response.Redirect("C_ForLeaveOvertime_Leave_Check.aspx?TPM_FION=" + Request["TPM_FION"]);

        }
    }
}
