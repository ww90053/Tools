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
    public partial class Personal4_1 : System.Web.UI.Page
    {
        List<String> CHKAD_AD = new List<string>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                A.check_power();
           
             A.set_Panel_EnterToTAB(ref this.Panel_CHKAD);
                A.controlEnable(ref this.Panel_CHKAD, false);
            }
        }

        protected void btInsert_Click(object sender, EventArgs e)
        {
            ViewState["CMDSQL"] = "INSERT INTO A_CHKAD(MZ_AD,MZ_MASTER_NAME,MZ_MASTER_POS,MZ_PRID," +//MZ_PRID1," +
                                                           " MZ_FAXNO,MZ_ADDRESS,PWD_NO,SPEED_NO,CC_NO,MZ_FILENO,MZ_YEARUSE)" +
                                              " VALUES(@MZ_AD,@MZ_MASTER_NAME,@MZ_MASTER_POS,@MZ_PRID," +//:MZ_PRID1," +
                                                           " @MZ_FAXNO,@MZ_ADDRESS,@PWD_NO,@SPEED_NO,@CC_NO,MZ_FILENO,MZ_YEARUSE)";
            ViewState["Mode"] = "INSERT";


            TextBox_CC_NO.Text = string.Empty;
            TextBox_MZ_AD.Text = string.Empty;
            TextBox_MZ_ADDRESS.Text = string.Empty;
            TextBox_MZ_MASTER_NAME.Text = string.Empty;
            TextBox_MZ_MASTER_POS.Text = string.Empty;
            TextBox_MZ_NAME.Text = string.Empty;
            TextBox_MZ_PRID.Text = string.Empty;
            TextBox_MZ_FAXNO.Text = string.Empty;
            TextBox_PWD_NO.Text = string.Empty;
            TextBox_SPEED_NO.Text = string.Empty;

            btInsert.Enabled = false;
            btDelete.Enabled = false;
            btUpdate.Enabled = false;
            btCancel.Enabled = true;
            btok.Enabled = true;

            A.controlEnable(ref this.Panel_CHKAD, true);
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_AD.ClientID + "').focus();$get('" + TextBox_MZ_AD.ClientID + "').focus();", true);
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
                ViewState["CMDSQL"] = "UPDATE A_CHKAD SET MZ_AD=@MZ_AD,MZ_MASTER_NAME=@MZ_MASTER_NAME," +
                                                               " MZ_MASTER_POS=@MZ_MASTER_POS,MZ_PRID=@MZ_PRID," +//MZ_PRID1=@MZ_PRID1," +
                                                               " MZ_FAXNO=@MZ_FAXNO,MZ_ADDRESS=@MZ_ADDRESS,PWD_NO=@PWD_NO,SPEED_NO=@SPEED_NO,CC_NO=@CC_NO,MZ_FILENO=@MZ_FILENO,MZ_YEARUSE=@MZ_YEARUSE" +
                                                               " WHERE MZ_AD='" + o_str.tosql(TextBox_MZ_AD.Text.Trim()) + "'";
                ViewState["Mode"] = "UPDATE";

                HttpCookie Cookie1 = new HttpCookie("PKEY_MZ_AD");
                Cookie1.Value = TextBox_MZ_AD.Text;
                Response.Cookies.Add(Cookie1);

                btUpdate.Enabled = false;
                btDelete.Enabled = false;
                btok.Enabled = true;

                A.controlEnable(ref this.Panel_CHKAD, true);
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_AD.ClientID + "').focus();$get('" + TextBox_MZ_AD.ClientID + "').focus();", true);
            }
        }

        protected void execSQL()
        {
            //2010.06.07 by 伊珊
            string ErrorString = "";

            string old_AD = "NULL";

            if (ViewState["Mode"].ToString() == "UPDATE")
            {
                HttpCookie Cookie1 = new HttpCookie("PKEY_MZ_AD");
                Cookie1 = Request.Cookies["PKEY_MZ_AD"];
                old_AD = Cookie1.Value.ToString();
            }

            string pkey_check;

            if (old_AD == TextBox_MZ_AD.Text && ViewState["Mode"].ToString() == "UPDATE")
                pkey_check = "0";
            else
                pkey_check = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_CHKAD WHERE MZ_AD='" + TextBox_MZ_AD.Text.Trim() + "'");

            if (pkey_check != "0")
            {
                ErrorString += "核定機關違反唯一值的條件" + "\\r\\n";
                TextBox_MZ_AD.BackColor = Color.Orange;
            }
            else
                TextBox_MZ_AD.BackColor = Color.White;


            //20141216 這應該是想做控制項檢核.但 DBDIC 又沒有資料
            //foreach (Object ob in Panel_CHKAD.Controls)
            //{
            //    if (ob is TextBox)
            //    {
            //        TextBox tbox = (TextBox)ob;

            //        string result = o_Check_Dbdic.ErrorString(tbox.ID, "A_CHKAD", tbox.Text);

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
            //    else if (ob is DropDownList)
            //    {
            //        DropDownList dlist = (DropDownList)ob;

            //        string result = o_Check_Dbdic.ErrorString(dlist.ID, "A_CHKAD", dlist.Text);

            //        if (!string.IsNullOrEmpty(result))
            //        {
            //            ErrorString += result + "\\r\\n";
            //            dlist.BackColor = Color.Orange;
            //        }
            //        else
            //        {
            //            dlist.BackColor = Color.White;
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


                cmd.Parameters.Add("MZ_AD", SqlDbType.VarChar).Value = TextBox_MZ_AD.Text.Trim();
                cmd.Parameters.Add("MZ_MASTER_NAME", SqlDbType.VarChar).Value = TextBox_MZ_MASTER_NAME.Text.Trim();
                cmd.Parameters.Add("MZ_MASTER_POS", SqlDbType.VarChar).Value = TextBox_MZ_MASTER_POS.Text.Trim();
                cmd.Parameters.Add("MZ_PRID", SqlDbType.VarChar).Value = TextBox_MZ_PRID.Text.Trim();
                cmd.Parameters.Add("MZ_FAXNO", SqlDbType.VarChar).Value = TextBox_MZ_FAXNO.Text.Trim();//unknowfield
                cmd.Parameters.Add("MZ_ADDRESS", SqlDbType.VarChar).Value = TextBox_MZ_ADDRESS.Text.Trim();
                cmd.Parameters.Add("PWD_NO", SqlDbType.VarChar).Value = TextBox_PWD_NO.Text.Trim();
                cmd.Parameters.Add("SPEED_NO", SqlDbType.VarChar).Value = TextBox_SPEED_NO.Text.Trim();
                cmd.Parameters.Add("CC_NO", SqlDbType.VarChar).Value = TextBox_CC_NO.Text.Trim();
                cmd.Parameters.Add("MZ_FILENO", SqlDbType.VarChar).Value = TextBox_MZ_FILENO.Text;
                cmd.Parameters.Add("MZ_YEARUSE", SqlDbType.VarChar).Value = TextBox_MZ_YEARUSE.Text.Trim();
                try
                {
                    cmd.ExecuteNonQuery();

                    Response.Cookies["PKEY_MZ_AD"].Expires = DateTime.Now.AddYears(-1);

                    if (ViewState["Mode"].ToString() == "INSERT")
                    {
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
                    GridView1.DataBind();
                    GridView1.SelectedIndex = -1;

                    TextBox_CC_NO.Text = string.Empty;
                    TextBox_MZ_AD.Text = string.Empty;
                    TextBox_MZ_ADDRESS.Text = string.Empty;
                    TextBox_MZ_MASTER_NAME.Text = string.Empty;
                    TextBox_MZ_MASTER_POS.Text = string.Empty;
                    TextBox_MZ_NAME.Text = string.Empty;
                    TextBox_MZ_PRID.Text = string.Empty;
                    TextBox_MZ_FAXNO.Text = string.Empty;
                    TextBox_PWD_NO.Text = string.Empty;
                    TextBox_SPEED_NO.Text = string.Empty;

                    btInsert.Enabled = true;
                    btCancel.Enabled = false;
                    btDelete.Enabled = false;
                    btUpdate.Enabled = false;
                    btok.Enabled = false;
                    ViewState.Remove("Mode");
                    A.controlEnable(ref this.Panel_CHKAD, false);
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
            btInsert.Enabled = true;
            btCancel.Enabled = false;
            btUpdate.Enabled = false;
            btDelete.Enabled = false;
            btok.Enabled = false;
            TextBox_CC_NO.Text = string.Empty;
            TextBox_MZ_AD.Text = string.Empty;
            TextBox_MZ_ADDRESS.Text = string.Empty;
            TextBox_MZ_MASTER_NAME.Text = string.Empty;
            TextBox_MZ_MASTER_POS.Text = string.Empty;
            TextBox_MZ_NAME.Text = string.Empty;
            TextBox_MZ_PRID.Text = string.Empty;
            TextBox_MZ_FAXNO.Text = string.Empty;
            TextBox_PWD_NO.Text = string.Empty;
            TextBox_SPEED_NO.Text = string.Empty;

            A.controlEnable(ref this.Panel_CHKAD, false);
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
                string DeleteString = "DELETE FROM A_CHKAD WHERE MZ_AD='" + TextBox_MZ_AD.Text + "'";
                try
                {
                    o_DBFactory.ABC_toTest.Edit_Data(DeleteString);
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除成功')", true);
                    //2010.06.04 LOG紀錄 by伊珊
                    TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00004", TPMPermissions._boolTPM(), DeleteString);
                    GridView1.DataBind();
                    GridView1.SelectedIndex = -1;
                    btInsert.Enabled = true;
                    btCancel.Enabled = false;
                    btUpdate.Enabled = false;
                    btDelete.Enabled = false;
                    btok.Enabled = false;
                    TextBox_CC_NO.Text = string.Empty;
                    TextBox_MZ_AD.Text = string.Empty;
                    TextBox_MZ_ADDRESS.Text = string.Empty;
                    TextBox_MZ_MASTER_NAME.Text = string.Empty;
                    TextBox_MZ_MASTER_POS.Text = string.Empty;
                    TextBox_MZ_NAME.Text = string.Empty;
                    TextBox_MZ_PRID.Text = string.Empty;
                    TextBox_MZ_FAXNO.Text = string.Empty;
                    TextBox_PWD_NO.Text = string.Empty;
                    TextBox_SPEED_NO.Text = string.Empty;
                }
                catch
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除失敗');", true);
                }
            }
        }

        protected void btSearch_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal4-1Search.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "','查詢','top=190,left=200,width=500,height=150,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GridView1, "Select$" + e.Row.RowIndex);
                e.Row.Cells[6].Attributes.Add("Style", "display:none");
            }
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView1.SelectedIndex = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "Select")
            {
                GridView1.SelectedIndex = Convert.ToInt32(e.CommandArgument);
                string strSQL = "SELECT * FROM A_CHKAD WHERE MZ_AD='" + GridView1.SelectedRow.Cells[0].Text.Trim() + "'";

                DataTable tempDT = new DataTable();

                tempDT = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GETVALUE");

                btInsert.Enabled = false;
                btUpdate.Enabled = true;
                btDelete.Enabled = true;
                btCancel.Enabled = true;
                btok.Enabled = false;

                TextBox_CC_NO.Text = tempDT.Rows[0]["CC_NO"].ToString();
                TextBox_MZ_AD.Text = tempDT.Rows[0]["MZ_AD"].ToString();
                TextBox_MZ_ADDRESS.Text = tempDT.Rows[0]["MZ_ADDRESS"].ToString();
                TextBox_MZ_MASTER_NAME.Text = tempDT.Rows[0]["MZ_MASTER_NAME"].ToString();
                TextBox_MZ_MASTER_POS.Text = tempDT.Rows[0]["MZ_MASTER_POS"].ToString();
                TextBox_MZ_NAME.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND MZ_KCODE='" + tempDT.Rows[0]["MZ_AD"].ToString() + "'");
                TextBox_MZ_PRID.Text = tempDT.Rows[0]["MZ_PRID"].ToString();
                TextBox_MZ_FAXNO.Text = tempDT.Rows[0]["MZ_FAXNO"].ToString();
                TextBox_PWD_NO.Text = tempDT.Rows[0]["PWD_NO"].ToString();
                TextBox_SPEED_NO.Text = tempDT.Rows[0]["SPEED_NO"].ToString();
                TextBox_MZ_FILENO.Text = tempDT.Rows[0]["MZ_FILENO"].ToString();
                TextBox_MZ_YEARUSE.Text = tempDT.Rows[0]["MZ_YEARUSE"].ToString();
            }
        }

        protected void TextBox_MZ_AD_TextChanged(object sender, EventArgs e)
        {
            TextBox_MZ_AD.Text = TextBox_MZ_AD.Text.ToUpper();
            TextBox_MZ_NAME.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE='" + o_str.tosql(TextBox_MZ_AD.Text.Trim()) + "' AND MZ_KTYPE='04' ");
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_AD.ClientID + "').focus();$get('" + TextBox_MZ_AD.ClientID + "').focus();", true);
        }

        protected void CV_AD_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (string.IsNullOrEmpty(args.Value))
            {
                args.IsValid = false;
                TextBox_MZ_AD.BackColor = Color.Orange;

            }
            else
            {
                args.IsValid = true;
                TextBox_MZ_AD.BackColor = Color.White;

            }
        }

        protected void btok_Click(object sender, EventArgs e)
        {
            execSQL();
        }
    }
}
