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
    public partial class Personal4_4 : System.Web.UI.Page
    {
        List<String> PROLNO_PROLNO = new List<string>();
        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (!Page.IsPostBack)
            { 
                A.check_power();
                A.controlEnable(ref this.Panel_PROLNO, false);
                A.set_Panel_EnterToTAB(ref this.Panel_PROLNO);
            }
           

          
        }

      

        protected void btInsert_Click(object sender, EventArgs e)
        {
            ViewState["CMDSQL"] = "INSERT INTO A_PROLNO(MZ_PROLNO,MZ_PRONAME) VALUES(@MZ_PROLNO,@MZ_PRONAME)";

            ViewState["Mode"] = "INSERT";

            TextBox_MZ_PROLNO.Text = string.Empty;
            TextBox_MZ_PRONAME.Text = string.Empty;

            btDelete.Enabled = false;
            btInsert.Enabled = false;
            btUpdate.Enabled = false;
            btCancel.Enabled = true;
            btOK.Enabled = true;
            A.controlEnable(ref this.Panel_PROLNO, true);

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
                ViewState["CMDSQL"] = "UPDATE A_PROLNO SET MZ_PROLNO=@MZ_PROLNO,MZ_PRONAME=@MZ_PRONAME WHERE MZ_PROLNO='" + TextBox_MZ_PROLNO.Text + "'";

                HttpCookie Cookie1 = new HttpCookie("PKEY_MZ_PROLNO");
                Cookie1.Value = TextBox_MZ_PROLNO.Text;
                Response.Cookies.Add(Cookie1);

                ViewState["Mode"] = "UPDATE";
                btOK.Enabled = true;
                btUpdate.Enabled = false;
                btDelete.Enabled = false;
            }
            A.controlEnable(ref this.Panel_PROLNO, true);

        }

        protected void btOK_Click(object sender, EventArgs e)
        {

            //2010.06.07 by 伊珊
            string ErrorString = "";

            string old_PROLNO = "NULL";

            if (ViewState["Mode"].ToString() == "UPDATE")
            {
                HttpCookie Cookie1 = new HttpCookie("PKEY_MZ_PROLNO");
                Cookie1 = Request.Cookies["PKEY_MZ_PROLNO"];
                old_PROLNO = Cookie1.Value.ToString();
            }

            string pkey_check;

            if (old_PROLNO == TextBox_MZ_PROLNO.Text && ViewState["Mode"].ToString() == "UPDATE")
                pkey_check = "0";
            else
                pkey_check = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_PROLNO WHERE MZ_PROLNO='" + TextBox_MZ_PROLNO.Text.Trim() + "'");

            if (pkey_check != "0")
            {
                ErrorString += "獎懲依據代碼違反唯一值的條件" + "\\r\\n";
                TextBox_MZ_PROLNO.BackColor = Color.Orange;
            }
            else
                TextBox_MZ_PROLNO.BackColor = Color.White;
            //20141216 這應該是想做控制項檢核.但 DBDIC 又沒有資料
            //foreach (Object ob in Panel_PROLNO.Controls)
            //{
            //    if (ob is TextBox)
            //    {
            //        TextBox tbox = (TextBox)ob;

            //        string result = o_Check_Dbdic.ErrorString(tbox.ID, "A_PROLNO", tbox.Text);

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
                cmd.Parameters.Add("MZ_PROLNO", SqlDbType.VarChar).Value = TextBox_MZ_PROLNO.Text.Trim();
                cmd.Parameters.Add("MZ_PRONAME", SqlDbType.VarChar).Value = TextBox_MZ_PRONAME.Text.Trim();

                try
                {
                    cmd.ExecuteNonQuery();

                    Response.Cookies["PKEY_MZ_PROLNO"].Expires = DateTime.Now.AddYears(-1);

                    if (ViewState["Mode"].ToString() == "INSERT")
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('新增成功');location.href('Personal4-4.aspx?MZ_PROLNO=" + TextBox_MZ_PROLNO.Text.Trim() + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "');", true);
                        //2010.06.04 LOG紀錄 by伊珊
                        TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00002", TPMPermissions._boolTPM(), o_DBFactory.ABC_toTest.RegexSQL(cmd));
                    }
                    else if (ViewState["Mode"].ToString() == "UPDATE")
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('修改成功');", true);
                        //2010.06.04 LOG紀錄 by伊珊
                        TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00003", TPMPermissions._boolTPM(), o_DBFactory.ABC_toTest.RegexSQL(cmd));
                        
                    }
                    TextBox_MZ_PROLNO.Text = string.Empty;
                    TextBox_MZ_PRONAME.Text = string.Empty;
                    GridView1.DataBind();
                    GridView1.SelectedIndex = -1;
                    btInsert.Enabled = true;
                    btCancel.Enabled = false;
                    btOK.Enabled = false;
                    btDelete.Enabled = false;
                    btUpdate.Enabled = false;
                    ViewState.Remove("Mode");
                    A.controlEnable(ref this.Panel_PROLNO, false);
                }
                catch
                {
                    if (ViewState["Mode"].ToString() == "INSERT")
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('新增失敗');", true);
                    }
                    else if (ViewState["Mode"].ToString() == "UPDATE")
                    {
                        //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('修改失敗');location.href('Personal4-4.aspx?XCOUNT=" + xcount.Text.Trim() + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "');", true);
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
            TextBox_MZ_PROLNO.Text = string.Empty;
            TextBox_MZ_PRONAME.Text = string.Empty;
            btInsert.Enabled = true;
            btCancel.Enabled = false;
            btOK.Enabled = false;
            btDelete.Enabled = false;
            btUpdate.Enabled = false;
            A.controlEnable(ref this.Panel_PROLNO, false);

        }

        protected void btDelete_Click(object sender, EventArgs e)
        {
            string sqlString = "DELETE FROM A_PROLNO WHERE MZ_PROLNO='" + TextBox_MZ_PROLNO.Text.Trim() + "'";
            try
            {
                o_DBFactory.ABC_toTest.Edit_Data(sqlString);
                
                TextBox_MZ_PROLNO.Text = string.Empty;
                TextBox_MZ_PRONAME.Text = string.Empty;
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除成功')", true);
                //2010.06.04 LOG紀錄 by伊珊
                TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00004", TPMPermissions._boolTPM(), sqlString);
                //}

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

        protected void btSearch_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal4-4Search.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "','查詢','top=190,left=200,width=500,height=150,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
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
            GridView1.SelectedIndex = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "Select")
            {
                


                TextBox_MZ_PROLNO.Text = GridView1.SelectedRow.Cells[0].Text.Trim();
                TextBox_MZ_PRONAME.Text = GridView1.SelectedRow.Cells[1].Text.Trim();

                btUpdate.Enabled = true;
                btDelete.Enabled = true;
                btCancel.Enabled = true;
                btOK.Enabled = false;
                btInsert.Enabled = false;
            }
        }
    }
}
