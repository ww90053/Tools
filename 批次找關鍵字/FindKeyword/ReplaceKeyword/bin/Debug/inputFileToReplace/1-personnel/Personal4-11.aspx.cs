using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;

namespace TPPDDB._1_personnel
{
    public partial class Personal4_11 : System.Web.UI.Page
    {
        List<String> STATIC_NO = new List<string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                A.check_power();
            //by MQ 20100311---------
            A.set_Panel_EnterToTAB(ref this.Panel_STATIC);
                //by MQ 20100311---------
                A.controlEnable(ref this.Panel_STATIC, false);
            }
        }

       

        protected void btSearch_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal4-11Search.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "','查詢','top=190,left=200,width=500,height=150,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }

        protected void btInsert_Click(object sender, EventArgs e)
        {

            ViewState["CMDSQL"] = "INSERT INTO A_STATIC(STATIC_NO,STATIC_NAME) VALUES(@STATIC_NO,@STATIC_NAME)";

            ViewState["Mode"] = "INSERT";

            TextBox_STATIC_NAME.Text = string.Empty;
            TextBox_STATIC_NO.Text = string.Empty;
            btInsert.Enabled = false;
            btDelete.Enabled = false;
            btUpdate.Enabled = false;
            btCancel.Enabled = true;
            btOK.Enabled = true;

            //by MQ 20100311---------
            A.controlEnable(ref this.Panel_STATIC, true);

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
                ViewState["CMDSQL"] = "UPDATE A_STATIC SET STATIC_NO=@STATIC_NO,STATIC_NAME=@STATIC_NAME WHERE STATIC_NO='" + TextBox_STATIC_NO.Text + "'";

                ViewState["Mode"] = "UPDATE";

                HttpCookie Cookie1 = new HttpCookie("PKEY_STATIC_NO");
                Cookie1.Value = TextBox_STATIC_NO.Text;
                Response.Cookies.Add(Cookie1);

                btDelete.Enabled = false;

                btUpdate.Enabled = false;

                btOK.Enabled = true;

                //by MQ 20100311---------
                A.controlEnable(ref this.Panel_STATIC, true);
            }
        }

        protected void btOK_Click(object sender, EventArgs e)
        {

            //2010.06.07 by 伊珊
            string ErrorString = "";

            string old_STATIC_NO = "NULL";

            if (ViewState["Mode"].ToString() == "UPDATE")
            {
                HttpCookie Cookie1 = new HttpCookie("PKEY_STATIC_NO");
                Cookie1 = Request.Cookies["PKEY_STATIC_NO"];
                old_STATIC_NO = Cookie1.Value.ToString();
            }

            string pkey_check;

            if (old_STATIC_NO == TextBox_STATIC_NO.Text && ViewState["Mode"].ToString() == "UPDATE")
                pkey_check = "0";
            else
                pkey_check = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_STATIC WHERE STATIC_NO='" + TextBox_STATIC_NO.Text.Trim() + "'");

            if (pkey_check != "0")
            {
                ErrorString += "獎懲統計分類代碼違反唯一值的條件" + "\\r\\n";
                TextBox_STATIC_NAME.BackColor = Color.Orange;
            }
            else
                TextBox_STATIC_NAME.BackColor = Color.White;
            //20141216 這應該是想做控制項檢核.但 DBDIC 又沒有資料
            //foreach (Object ob in Panel_STATIC.Controls)
            //{
            //    if (ob is TextBox)
            //    {
            //        TextBox tbox = (TextBox)ob;

            //        string result = o_Check_Dbdic.ErrorString(tbox.ID, "A_STATIC", tbox.Text);

            //        if (!string.IsNullOrEmpty(result))
            //        {
            //            ErrorString += result + "\\r\\n";
            //            tbox.BackColor = Color.Orange;
            //        }
            //        else
            //        {
            //            tbox.BackColor = Color.White;
            //        }
            //    }
            //}
            //20141216 這應該是想做控制項檢核.但 DBDIC 又沒有資料
            if (!string.IsNullOrEmpty(ErrorString))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('" + ErrorString + "請更改不可重複欄位與輸入變色之欄位');", true);
                return;
            }

            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(ViewState["CMDSQL"].ToString(), conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("STATIC_NO", SqlDbType.VarChar).Value = TextBox_STATIC_NO.Text.Trim();
                cmd.Parameters.Add("STATIC_NAME", SqlDbType.VarChar).Value = TextBox_STATIC_NAME.Text.Trim();

                try
                {
                    cmd.ExecuteNonQuery();

                    Response.Cookies["PKEY_STATIC_NO"].Expires = DateTime.Now.AddYears(-1);

                    if (ViewState["Mode"].ToString() == "INSERT")
                    {

                        // ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('新增成功');location.href('Personal4-11.aspx?STATIC_NO=" + TextBox_STATIC_NO.Text.Trim() + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "');", true);
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('新增成功');", true);
                        //2010.06.04 LOG紀錄 by伊珊
                        TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00002", TPMPermissions._boolTPM(), o_DBFactory.ABC_toTest.RegexSQL(cmd));
                    }
                    else if (ViewState["Mode"].ToString() == "UPDATE")
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('修改成功');", true);
                        //2010.06.04 LOG紀錄 by伊珊
                        TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00003", TPMPermissions._boolTPM(), o_DBFactory.ABC_toTest.RegexSQL(cmd));
                        
                    }
                    btUpdate.Enabled = false;
                    btInsert.Enabled = true;
                    btCancel.Enabled = false;
                    btOK.Enabled = false;
                    btDelete.Enabled = true;
                    GridView1.DataBind();
                    GridView1.SelectedIndex = -1;
                    ViewState.Remove("Mode");
                    //by MQ 20100311---------
                    A.controlEnable(ref this.Panel_STATIC, false);
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
            TextBox_STATIC_NAME.Text = string.Empty;
            TextBox_STATIC_NO.Text = string.Empty;

            btInsert.Enabled = true;
            btCancel.Enabled = false;
            btUpdate.Enabled = false;
            btDelete.Enabled = false;
            btOK.Enabled = false;

            //by MQ 20100311---------
            A.controlEnable(ref this.Panel_STATIC, false);
        }

        protected void btDelete_Click(object sender, EventArgs e)
        {
            string sqlString = "DELETE FROM A_STATIC WHERE STATIC_NO='" + o_str.tosql(TextBox_STATIC_NO.Text.Trim()) + "'";
            try
            {
                o_DBFactory.ABC_toTest.Edit_Data(sqlString);
                
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除成功')", true);
                //2010.06.04 LOG紀錄 by伊珊
                TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00004", TPMPermissions._boolTPM(), sqlString);
                //}
                GridView1.DataBind();
                TextBox_STATIC_NAME.Text = string.Empty;
                TextBox_STATIC_NO.Text = string.Empty;
                btInsert.Enabled = true;
                btCancel.Enabled = false;
                btOK.Enabled = false;
                btDelete.Enabled = false;
                btUpdate.Enabled = false;
                GridView1.SelectedIndex = -1;
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除失敗');", true);
            }
        }

        

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                GridView1.SelectedIndex = Convert.ToInt32(e.CommandArgument);
                TextBox_STATIC_NO.Text = GridView1.SelectedRow.Cells[0].Text.Trim();
                TextBox_STATIC_NAME.Text = GridView1.SelectedRow.Cells[1].Text.Trim();

                btInsert.Enabled = false;
                btOK.Enabled = false;
                btCancel.Enabled = true;
                btUpdate.Enabled = true;
                btDelete.Enabled = true;
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GridView1, "Select$" + e.Row.RowIndex);
                e.Row.Cells[2].Attributes.Add("Style", "display:none");
            }
        }
    }
}
