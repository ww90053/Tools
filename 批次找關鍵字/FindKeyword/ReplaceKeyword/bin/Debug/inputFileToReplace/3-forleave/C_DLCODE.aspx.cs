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

namespace TPPDDB._3_forleave
{
    public partial class C_DLCODE : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                C.check_power();
            

            
                //by MQ 20100312---------
                C.set_Panel_EnterToTAB(ref this.Panel1);    
                C.set_Panel_EnterToTAB(ref this.Panel2);
                //by MQ 20100312---------
                C.controlEnable(ref this.Panel1, false);

                btInsert.Enabled = true;
                btUpdate.Enabled = false;
                btDelete.Enabled = false;
                btCancel.Enabled = false;
                btOK.Enabled = false;
            }
        }

        protected void btInsert_Click(object sender, EventArgs e)
        {
            ViewState["CMDSQL"] = "INSERT INTO C_DLCODE(MZ_CODE,MZ_CNAME) VALUES(@MZ_CODE,@MZ_CNAME)";

            ViewState["Mode"] = "INSERT";

            TextBox_MZ_CNAME.Text = string.Empty;
            TextBox_MZ_CODE.Text = string.Empty;

            btDelete.Enabled = false;
            btInsert.Enabled = false;
            btUpdate.Enabled = false;
            btCancel.Enabled = true;
            btOK.Enabled = true;

            //by MQ 20100312---------
            C.controlEnable(ref this.Panel1, true);
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_CODE.ClientID + "').focus();$get('" + TextBox_MZ_CODE.ClientID + "').focus();", true);
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
                ViewState["CMDSQL"] = "UPDATE C_DLCODE SET MZ_CODE=@MZ_CODE,MZ_CNAME=@MZ_CNAME WHERE MZ_CODE='" + TextBox_MZ_CODE.Text + "'";

                ViewState["Mode"] = "UPDATE";

                HttpCookie Cookie1 = new HttpCookie("PKEY_MZ_CODE");
                Cookie1.Value = TextBox_MZ_CODE.Text;
                Response.Cookies.Add(Cookie1);

                btUpdate.Enabled = false;
                btDelete.Enabled = false;
                btOK.Enabled = true;

                //by MQ 20100312---------
                C.controlEnable(ref this.Panel1, true);
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_CODE.ClientID + "').focus();$get('" + TextBox_MZ_CODE.ClientID + "').focus();", true);
            }
        }

        protected void execSQL()
        {
            //2010.06.07 by 伊珊

            string ErrorString = "";

            string old_CODE = "NULL";

            if (ViewState["Mode"].ToString() == "UPDATE")
            {
                HttpCookie Cookie1 = new HttpCookie("PKEY_MZ_CODE");
                Cookie1 = Request.Cookies["PKEY_MZ_CODE"];
                old_CODE = Cookie1.Value.ToString();
            }

            string pkey_check;

            if (old_CODE == TextBox_MZ_CODE.Text && ViewState["Mode"].ToString() == "UPDATE")
                pkey_check = "0";
            else pkey_check = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM C_DLCODE WHERE MZ_CODE='" + TextBox_MZ_CODE.Text.Trim() + "'");

            if (pkey_check != "0")
            {
                ErrorString += "差假別代號違反唯一值的條件" + "\\r\\n";
                TextBox_MZ_CODE.BackColor = Color.Orange;
            }
            else
                TextBox_MZ_CODE.BackColor = Color.White;

            //20141216 這應該是想做控制項檢核.但 DBDIC 又沒有資料
            //foreach (Object ob in Panel1.Controls)
            //{
            //    if (ob is TextBox)
            //    {
            //        TextBox tbox = (TextBox)ob;

            //        string result = o_Check_Dbdic.ErrorString(tbox.ID, "C_DLCODE", tbox.Text);

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
                try
                {
                    cmd.Parameters.Add("MZ_CODE", SqlDbType.VarChar).Value = TextBox_MZ_CODE.Text.Trim();
                    cmd.Parameters.Add("MZ_CNAME", SqlDbType.VarChar).Value = TextBox_MZ_CNAME.Text.Trim();
                    cmd.ExecuteNonQuery();

                    Response.Cookies["PKEY_MZ_CODE"].Expires = DateTime.Now.AddYears(-1);

                    if (ViewState["Mode"].ToString() == "INSERT")
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('新增成功');", true);

                        //2010.06.04 LOG紀錄 by伊珊
                        TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00002", TPMPermissions._boolTPM(), o_DBFactory.ABC_toTest.RegexSQL(cmd));
                    }
                    else if (ViewState["Mode"].ToString() == "UPDATE")
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('編輯成功');", true);

                        //2010.06.04 LOG紀錄 by伊珊
                        TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00003", TPMPermissions._boolTPM(), o_DBFactory.ABC_toTest.RegexSQL(cmd));
                    }

                    TextBox_MZ_CNAME.Text = string.Empty;
                    TextBox_MZ_CODE.Text = string.Empty;

                    GridView1.DataBind();
                    GridView1.SelectedIndex = -1;

                    btInsert.Enabled = true;
                    btUpdate.Enabled = false;
                    btDelete.Enabled = false;
                    btCancel.Enabled = false;
                    btOK.Enabled = false;
                    ViewState.Remove("Mode");
                    //by MQ 20100312---------
                    C.controlEnable(ref this.Panel1, false);
                }
                catch
                {
                    if (ViewState["Mode"].ToString() == "INSERT")
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('新增失敗');", true);
                    }
                    else if (ViewState["Mode"].ToString() == "UPDATE")
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('編輯失敗');", true);
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
            btDelete.Enabled = false;
            btCancel.Enabled = false;
            btUpdate.Enabled = false;
            btInsert.Enabled = true;
            btOK.Enabled = false;
            TextBox_MZ_CODE.Text = string.Empty;
            TextBox_MZ_CNAME.Text = string.Empty;

            //by MQ 20100312---------
            C.controlEnable(ref this.Panel1, false);
        }

        protected void btDelete_Click(object sender, EventArgs e)
        {
            if (GridView1.SelectedRow == null)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請選取資料')", true);
                return;
            }
            else
            {
                string DeleteString = "DELETE FROM C_DLCODE WHERE MZ_CODE='" + GridView1.SelectedRow.Cells[0].Text.Trim() + "'";
                try
                {
                    o_DBFactory.ABC_toTest.Edit_Data(DeleteString);
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除成功')", true);

                    //2010.06.04 LOG紀錄 by伊珊
                    TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00004", TPMPermissions._boolTPM(), DeleteString);

                    TextBox_MZ_CODE.Text = string.Empty;
                    TextBox_MZ_CNAME.Text = string.Empty;

                    btCancel.Enabled = false;
                    btDelete.Enabled = false;
                    btUpdate.Enabled = false;
                    btInsert.Enabled = true;
                    btOK.Enabled = false;
                    GridView1.DataBind();
                    GridView1.SelectedIndex = -1;
                }
                catch
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除失敗')", true);
                }
            }
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView1.SelectedIndex = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "Select")
            {
                btUpdate.Enabled = true;
                //依照 維護單-1120313 要求,刪除按鈕永久停用
                btDelete.Enabled = false;
                btCancel.Enabled = true;
                btOK.Enabled = false;
                btInsert.Enabled = false;

                TextBox_MZ_CODE.Text = GridView1.SelectedRow.Cells[0].Text.Trim();
                TextBox_MZ_CNAME.Text = GridView1.SelectedRow.Cells[1].Text.Trim();
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

        protected void btOK_Click(object sender, EventArgs e)
        {
            execSQL();
        }
    }
}
