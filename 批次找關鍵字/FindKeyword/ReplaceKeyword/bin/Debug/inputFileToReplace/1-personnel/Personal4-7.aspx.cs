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
using System.Drawing;

namespace TPPDDB._1_personnel
{
    public partial class Personal4_7 : System.Web.UI.Page
    {
        //List<String> POLNUM_POLNO;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                A.check_power();
            
            //by MQ 20100310
            A.set_Panel_EnterToTAB(ref this.Panel_POLNUM);
                //by MQ 20100310
                A.controlEnable(ref this.Panel_POLNUM, false);
            }
        }

       

        protected void btInsert_Click(object sender, EventArgs e)
        {
            ViewState["CMDSQL"] = "INSERT INTO A_POLNUM(MZ_POLNO,MZ_PNAME) VALUES(@MZ_POLNO,@MZ_PNAME)";

            ViewState["Mode"] = "INSERT";

            TextBox_MZ_PNAME.Text = string.Empty;
            TextBox_MZ_POLNO.Text = string.Empty;

            btDelete.Enabled = false;
            btInsert.Enabled = false;
            btUpdate.Enabled = false;
            btCancel.Enabled = true;
            btOK.Enabled = true;

            //by MQ 20100310
            A.controlEnable(ref this.Panel_POLNUM, true);
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_POLNO.ClientID + "').focus();$get('" + TextBox_MZ_POLNO.ClientID + "').focus();", true);
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

                ViewState["CMDSQL"] = "UPDATE A_POLNUM SET MZ_POLNO=@MZ_POLNO,MZ_PNAME=@MZ_PNAME WHERE MZ_POLNO='" + o_str.tosql(TextBox_MZ_POLNO.Text.Trim()) + "'";

                ViewState["Mode"] = "UPDATE";

                HttpCookie Cookie1 = new HttpCookie("PKEY_MZ_POLNO");
                Cookie1.Value = TextBox_MZ_POLNO.Text;
                Response.Cookies.Add(Cookie1);

                btDelete.Enabled = false;
                btUpdate.Enabled = false;
                btOK.Enabled = true;

                //by MQ 20100310
                A.controlEnable(ref this.Panel_POLNUM, true);
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_POLNO.ClientID + "').focus();$get('" + TextBox_MZ_POLNO.ClientID + "').focus();", true);
            }
        }

        protected void btOK_Click(object sender, EventArgs e)
        {
            //2010.06.07 by 伊珊
            string ErrorString = "";

            string old_POLNO = "NULL";

            if (ViewState["Mode"].ToString() == "UPDATE")
            {
                HttpCookie Cookie1 = new HttpCookie("PKEY_MZ_POLNO");
                Cookie1 = Request.Cookies["PKEY_MZ_POLNO"];
                old_POLNO = Cookie1.Value.ToString();
            }

            string pkey_check;

            if (old_POLNO == TextBox_MZ_POLNO.Text && ViewState["Mode"].ToString() == "UPDATE")
                pkey_check = "0";
            else pkey_check = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_POLNUM WHERE MZ_POLNO='" + TextBox_MZ_POLNO.Text.Trim() + "'");

            if (pkey_check != "0")
            {
                ErrorString += "配分代碼違反唯一值的條件" + "\\r\\n";
                TextBox_MZ_POLNO.BackColor = Color.Orange;
            }
            else
                TextBox_MZ_POLNO.BackColor = Color.White;
            //20141216 這應該是想做控制項檢核.但 DBDIC 又沒有資料
            //foreach (Object ob in Panel_POLNUM.Controls)
            //{
            //    if (ob is TextBox)
            //    {
            //        TextBox tbox = (TextBox)ob;

            //        string result = o_Check_Dbdic.ErrorString(tbox.ID, "A_POLNUM", tbox.Text);
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
                cmd.Parameters.Add("MZ_POLNO", SqlDbType.VarChar).Value = TextBox_MZ_POLNO.Text.Trim();
                cmd.Parameters.Add("MZ_PNAME", SqlDbType.VarChar).Value = TextBox_MZ_PNAME.Text.Trim();
                try
                {
                    cmd.ExecuteNonQuery();

                    Response.Cookies["PKEY_MZ_POLNO"].Expires = DateTime.Now.AddYears(-1);

                    if (ViewState["Mode"].ToString() == "INSERT")
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('新增成功');location.href('Personal4-7.aspx?MZ_POLNO=" + TextBox_MZ_POLNO.Text.Trim() + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "');", true);
                        //2010.06.04 LOG紀錄 by伊珊
                        TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00002", TPMPermissions._boolTPM(), o_DBFactory.ABC_toTest.RegexSQL(cmd));

                    }

                    else if (ViewState["Mode"].ToString() == "UPDATE")
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('修改成功');", true);
                        //2010.06.04 LOG紀錄 by伊珊
                        TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00003", TPMPermissions._boolTPM(), o_DBFactory.ABC_toTest.RegexSQL(cmd));

                       
                    }
                    TextBox_MZ_PNAME.Text = string.Empty;
                    TextBox_MZ_POLNO.Text = string.Empty;
                    GridView1.DataBind();
                    GridView1.SelectedIndex = -1;
                    btInsert.Enabled = true;
                    btCancel.Enabled = false;
                    btOK.Enabled = false;
                    btDelete.Enabled = false;
                    ViewState.Remove("Mode");
                    //by MQ 20100310
                    A.controlEnable(ref this.Panel_POLNUM, false);

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
            TextBox_MZ_PNAME.Text = string.Empty;
            TextBox_MZ_POLNO.Text = string.Empty;
            btInsert.Enabled = true;
            btCancel.Enabled = false;
            btOK.Enabled = false;
            btDelete.Enabled = false;


            //by MQ 20100310
            A.controlEnable(ref this.Panel_POLNUM, false);

        }

        protected void btDelete_Click(object sender, EventArgs e)
        {
            string sqlString = "DELETE FROM A_POLNUM WHERE MZ_POLNO='" + TextBox_MZ_POLNO.Text.Trim() + "'";

            try
            {
                o_DBFactory.ABC_toTest.Edit_Data(sqlString);

               
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除成功')", true);
                //2010.06.04 LOG紀錄 by伊珊
                TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00004", TPMPermissions._boolTPM(), sqlString);
                //    }
                TextBox_MZ_PNAME.Text = string.Empty;
                TextBox_MZ_POLNO.Text = string.Empty;
                GridView1.DataBind();
                GridView1.SelectedIndex = -1;
                btInsert.Enabled = true;
                btCancel.Enabled = false;
                btOK.Enabled = false;
                btDelete.Enabled = false;
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除失敗');", true);
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

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                GridView1.SelectedIndex = Convert.ToInt32(e.CommandArgument);
                TextBox_MZ_POLNO.Text = GridView1.SelectedRow.Cells[0].Text.Trim();
                TextBox_MZ_PNAME.Text = GridView1.SelectedRow.Cells[1].Text.Trim();

                btInsert.Enabled = false;
                btUpdate.Enabled = true;
                btDelete.Enabled = true;
                btOK.Enabled = false;
                btCancel.Enabled = true;
            }
        }
    }
}
