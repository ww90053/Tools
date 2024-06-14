using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Data;


namespace TPPDDB._1_personnel
{
    public partial class PersonalUpdatePOSITSWT4 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            A.set_Panel_EnterToTAB(ref this.Panel1);

            ViewState["MZ_NO"] = Request["MZ_NO"];

            if (!IsPostBack)
            {
                TextBox_MZ_NO.Text = ViewState["MZ_NO"].ToString();
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            GridView1.Visible = true;
            GridView1.DataBind();
            
            if (RadioButtonList1.SelectedIndex == 0)
            {
                if (GridView1.Rows.Count == 0)
                {
                    GridView1.Visible = false ;

                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('查無資料');window.close()", true);
                }
                else
                {
                    GridView1.Visible = true;
                    Panel1.Visible = false;
                    Button2.Visible = true;
                    Button4.Visible = true;
                }

            }
            else
            {
                if (string.IsNullOrEmpty(o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_POSIT WHERE RTRIM(MZ_NO)='" + o_str.tosql(TextBox_MZ_NO.Text.Trim()) + "'")))
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.alert('查無資料！');", true);
                    return;
                }

                using (SqlConnection UpdateSWT4conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
                {
                    UpdateSWT4conn.Open();
                    string UpdateSWT4sql = "";
                 
                    if (RadioButtonList2.SelectedIndex == 0)
                    {
                        UpdateSWT4sql = "UPDATE A_POSIT SET MZ_SWT4='Y' WHERE MZ_NO='" + TextBox_MZ_NO.Text.Trim()+ "' AND (MZ_SWT1!='Y' )";
                    }
                    else
                    {
                        UpdateSWT4sql = "UPDATE A_POSIT SET MZ_SWT4='N' WHERE  RTRIM(MZ_NO)='" +TextBox_MZ_NO.Text.Trim() + "' AND (MZ_SWT1!='Y' )";
                    }

                    SqlCommand UpdateSWT4cmd = new SqlCommand(UpdateSWT4sql, UpdateSWT4conn);
                    try
                    {
                        UpdateSWT4cmd.ExecuteNonQuery();
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('審核成功');window.opener.location.href='Personal3-1.aspx?Personl_POSIT_SWT4_MZ_NO=" + o_str.tosql(TextBox_MZ_NO.Text.ToString()) + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "';window.close()", true);

                    }
                    catch (Exception)
                    {

                        throw;
                    }
                    finally
                    {
                        UpdateSWT4conn.Close();

                        //XX2013/06/18 
                        UpdateSWT4conn.Dispose();
                    }

                }

            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            using (SqlConnection UpdateSWT4conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                UpdateSWT4conn.Open();
                SqlTransaction transaction = UpdateSWT4conn.BeginTransaction();
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    string UpdateSWT4sql="";

                    if (RadioButtonList2.SelectedIndex == 0)
                    {
                        UpdateSWT4sql = "UPDATE A_POSIT SET MZ_SWT4='Y' WHERE MZ_ID='" + GridView1.DataKeys[i].Value + "' AND RTRIM(MZ_NO)='" + o_str.tosql(TextBox_MZ_NO.Text.Trim()) + "' AND (MZ_SWT1!='Y' )";
                    }
                    else
                    {
                        UpdateSWT4sql = "UPDATE A_POSIT SET MZ_SWT4='N' WHERE MZ_ID='" + GridView1.DataKeys[i].Value + "' AND RTRIM(MZ_NO)='" + o_str.tosql(TextBox_MZ_NO.Text.Trim()) + "' AND (MZ_SWT1!='Y' )";
                    }

                    SqlCommand UpdateSWT4cmd = new SqlCommand(UpdateSWT4sql, UpdateSWT4conn);
                    UpdateSWT4cmd.Transaction = transaction;
                    try
                    {
                        if ((GridView1.Rows[i].FindControl("CheckBox_SWT4") as CheckBox).Checked)
                        {
                            UpdateSWT4cmd.ExecuteNonQuery();
                        }

                        if (i == GridView1.Rows.Count - 1)
                        {
                            transaction.Commit();
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.alert('審核成功！');window.opener.location.href='Personal3-1.aspx?Personl_POSIT_SWT4_MZ_NO=" + o_str.tosql(TextBox_MZ_NO.Text.ToString()) + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "';window.close();", true);
                        }
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                    finally
                    {
                        if (i == GridView1.Rows.Count - 1)
                        {
                            UpdateSWT4conn.Close();

                            //XX2013/06/18 
                            UpdateSWT4conn.Dispose();
                        }
                    }
                }
            }
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.close()", true);
        }
    }
}
