using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Data;
using AjaxControlToolkit;



namespace TPPDDB._1_personnel
{
    public partial class Personal_Code : System.Web.UI.Page
    {
        //List<String> PROLNO_PROLNO = new List<string>();
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!Page.IsPostBack)
            {
                A.check_power();
                A.controlEnable(ref this.Panel_ID, false);
                A.set_Panel_EnterToTAB(ref this.Panel_ID);
            }
        }

        protected void btInsert_Click(object sender, EventArgs e)
        {
            ViewState["CMDSQL"] = "INSERT INTO A_CODE(ID,CODE,TABLE_NAME,MEMO) VALUES(@ID,@CODE,@TABLE_NAME,@MEMO)";
            ViewState["Mode"] = "INSERT";
            TextBox_ID.Text = string.Empty;
            TextBox_CODE.Text = string.Empty;
            TextBox_TABLE_NAME.Text = string.Empty;
            TextBox_MEMO.Text = string.Empty;
            btDelete.Enabled = false;
            btInsert.Enabled = false;
            btUpdate.Enabled = false;
            btCancel.Enabled = true;
            btOK.Enabled = true;
            A.controlEnable(ref this.Panel_ID, true);

        }

        protected void btUpdate_Click(object sender, EventArgs e)
        {
            if (GridView1.Rows.Count == 0)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請先新增資料')", true);
                return;
            }
            else if (GridView1.SelectedRow == null)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請選取資料')", true);
                return;
            }
            else
            {
                ViewState["CMDSQL"] = "UPDATE A_CODE SET ID=@ID,CODE=@CODE,TABLE_NAME=@TABLE_NAME,MEMO=@MEMO WHERE ID='" + TextBox_ID.Text + "'";
                ViewState["Mode"] = "UPDATE";
                btOK.Enabled = true;
                btUpdate.Enabled = false;
                btDelete.Enabled = false;
                ViewState["OLD_KTYPE"]=TextBox_ID.Text;
            }
            A.controlEnable(ref this.Panel_ID, true);
        }

        protected void btOK_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(ViewState["CMDSQL"].ToString(), conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = TextBox_ID.Text.Trim();
                cmd.Parameters.Add("CODE", SqlDbType.VarChar).Value = TextBox_CODE.Text.Trim();
                cmd.Parameters.Add("TABLE_NAME", SqlDbType.VarChar).Value = TextBox_TABLE_NAME.Text.Trim();
                cmd.Parameters.Add("MEMO", SqlDbType.VarChar).Value = TextBox_MEMO.Text.Trim();
                
                try
                {
                    cmd.ExecuteNonQuery();

                    if (ViewState["Mode"].ToString() == "INSERT")
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('新增成功');location.href('Personal_Code.aspx?ID=" + TextBox_ID.Text.Trim() + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "');", true);
                        TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00002", TPMPermissions._boolTPM(), o_DBFactory.ABC_toTest.RegexSQL(cmd));
                    }
                    else if (ViewState["Mode"].ToString() == "UPDATE")
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('修改成功');", true);
                        TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00003", TPMPermissions._boolTPM(), o_DBFactory.ABC_toTest.RegexSQL(cmd));
                        string UpateString = "UPDATE " + TextBox_TABLE_NAME.Text + " SET MZ_KTYPE='" + TextBox_ID.Text + "' WHERE MZ_KTYPE='" + ViewState["OLD_KTYPE"].ToString() + "'";
                        o_DBFactory.ABC_toTest.Edit_Data(UpateString);
                    }
                    TextBox_ID.Text = string.Empty;
                    TextBox_CODE.Text = string.Empty;
                    TextBox_TABLE_NAME.Text = string.Empty;
                    TextBox_MEMO.Text = string.Empty;
                    GridView1.DataBind();
                    GridView1.SelectedIndex = -1;
                    btInsert.Enabled = true;
                    btCancel.Enabled = false;
                    btOK.Enabled = false;
                    btDelete.Enabled = false;
                    btUpdate.Enabled = false;
                    ViewState.Remove("Mode");
                }
                catch
                {
                    if (ViewState["Mode"].ToString() == "INSERT")
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('新增失敗');", true);
                    }
                    else if (ViewState["Mode"].ToString() == "UPDATE")
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('修改失敗');", true);
                    }
                }
                finally
                {
                    conn.Close();
                    //XX2013/06/18 
                    conn.Dispose();
                }

            }
        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            GridView1.SelectedIndex = -1;
            TextBox_ID.Text = string.Empty;
            TextBox_CODE.Text = string.Empty;
            TextBox_TABLE_NAME.Text = string.Empty;
            TextBox_MEMO.Text = string.Empty;
            btInsert.Enabled = true;
            btCancel.Enabled = false;
            btOK.Enabled = false;
            btDelete.Enabled = false;
            btUpdate.Enabled = false;
            A.controlEnable(ref this.Panel_ID, false);

        }

        protected void btDelete_Click(object sender, EventArgs e)
        {
            string sqlString = "DELETE FROM A_CODE WHERE ID='" + TextBox_ID.Text.Trim() + "'";
            try
            {
                o_DBFactory.ABC_toTest.Edit_Data(sqlString);
                TextBox_ID.Text = string.Empty;
                TextBox_CODE.Text = string.Empty;
                TextBox_TABLE_NAME.Text = string.Empty;
                TextBox_MEMO.Text = string.Empty;
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除成功')", true);
                TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00004", TPMPermissions._boolTPM(), sqlString);
                btInsert.Enabled = true;
                btCancel.Enabled = false;
                btOK.Enabled = false;
                btDelete.Enabled = false;
                btUpdate.Enabled = false;
                GridView1.DataBind();
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除失敗');", true);
            }
        }

        /*protected void btSearch_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal4-4Search.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "','查詢','top=190,left=200,width=500,height=150,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }*/

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GridView1, "Select$" + e.Row.RowIndex);
                //e.Row.Cells[2].Attributes.Add("Style", "display:none");

            }
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView1.SelectedIndex = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "Select")
            {
                TextBox_ID.Text = GridView1.SelectedRow.Cells[0].Text.Trim();
                TextBox_CODE.Text = GridView1.SelectedRow.Cells[1].Text.Trim();
                TextBox_TABLE_NAME.Text = GridView1.SelectedRow.Cells[2].Text.Trim();
                TextBox_MEMO.Text = GridView1.SelectedRow.Cells[3].Text.Trim();

                btUpdate.Enabled = true;
                btDelete.Enabled = true;
                btCancel.Enabled = true;
                btOK.Enabled = false;
                btInsert.Enabled = false;
            }
        }

    }
}
