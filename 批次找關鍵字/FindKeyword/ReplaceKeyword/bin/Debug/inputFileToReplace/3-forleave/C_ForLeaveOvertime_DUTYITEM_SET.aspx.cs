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

namespace TPPDDB._3_forleave
{
    public partial class C_ForLeaveOvertime_DUTYITEM_SET : System.Web.UI.Page
    {



        public void group_control(string GROUP)
        {
            if (!string.IsNullOrEmpty(GROUP))
            {
                switch (GROUP)
                {
                    case "C":
                    case "D":
                        btInsert.Enabled = false;
                        btOK.Enabled = false;
                        break;
                    case "E":
                        btInsert.Enabled = false;
                        btOK.Enabled = false;
                        break;

                }
            }



        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                C.check_power();
            //by MQ 20100312---------   
            C.set_Panel_EnterToTAB(ref this.Panel1);
                C.controlEnable(ref this.Panel1, false);

                Label1.Text = "勤務項目設定";

                string strGID = o_a_Function.strGID(Session["ADPMZ_ID"].ToString());

                group_control(strGID);
            }
        }

        protected void btInsert_Click(object sender, EventArgs e)
        {
            ViewState["Mode"] = "INSERT";

            ViewState["sqlStr"] = "INSERT INTO C_DUTYITEM(DUTY_NO,DUTY_NAME) VALUES(@DUTY_NO,@DUTY_NAME) ";

            btOK.Enabled = true;
            btInsert.Enabled = false;
            //btDelete.Enabled = false;
            btCancel.Enabled = true;
            //btUpdate.Enabled = false;
            TextBox_DUTY_NO.Text = string.Empty;
            TextBox_DUTY_NAME.Text = string.Empty;
            C.controlEnable(ref this.Panel1, true);

        }

        protected void btUpdate_Click(object sender, EventArgs e)
        {

            ViewState["Mode"] = "UPDATE";

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
                ViewState["sqlStr"] = "UPDATE C_DUTYITEM SET DUTY_NO=@DUTY_NO,DUTY_NAME=@DUTY_NAME WHERE DUTY_NO='" + GridView1.SelectedRow.Cells[0].Text.Trim() + "'";

                HttpCookie Cookie1 = new HttpCookie("PKEY_DUTY_NO");
                Cookie1.Value = TextBox_DUTY_NO.Text;
                Response.Cookies.Add(Cookie1);

                //btUpdate.Enabled = false;
                //btDelete.Enabled = false;
                btOK.Enabled = true;
                C.controlEnable(ref this.Panel1, true);


            }
        }

        protected void execSQL()
        {
            //2010.06.07 by 伊珊
            string ErrorString = "";

            string old_DUTY_NO = "NULL";

            if (ViewState["Mode"].ToString() == "UPDATE")
            {
                HttpCookie Cookie1 = new HttpCookie("PKEY_DUTY_NO");
                Cookie1 = Request.Cookies["PKEY_DUTY_NO"];
                old_DUTY_NO = Cookie1.Value.ToString();
            }

            string pkey_check;

            if (old_DUTY_NO == TextBox_DUTY_NO.Text && ViewState["Mode"].ToString() == "UPDATE")
                pkey_check = "0";
            else
                pkey_check = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM C_DUTYITEM WHERE DUTY_NO='" + TextBox_DUTY_NO.Text.Trim() + "'");

            if (pkey_check != "0")
            {
                ErrorString += "勤務項目編號違反唯一值的條件" + "\\r\\n";
                TextBox_DUTY_NO.BackColor = Color.Orange;
            }
            else
            {
                TextBox_DUTY_NO.BackColor = Color.White;
            }
            //20141216 這應該是想做控制項檢核.但 DBDIC 又沒有資料
            //foreach (Object ob in Panel1.Controls)
            //{
            //    if (ob is TextBox)
            //    {
            //        TextBox tbox = (TextBox)ob;

            //        string result = o_Check_Dbdic.ErrorString(tbox.ID, "C_DUTYITEM", tbox.Text);

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
                SqlCommand cmd = new SqlCommand(ViewState["sqlStr"].ToString(), conn);
                cmd.CommandType = CommandType.Text;
                try
                {
                    cmd.Parameters.Add("DUTY_NO", SqlDbType.VarChar).Value = TextBox_DUTY_NO.Text.Trim();
                    cmd.Parameters.Add("DUTY_NAME", SqlDbType.VarChar).Value = TextBox_DUTY_NAME.Text.Trim();
                    cmd.ExecuteNonQuery();

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
                    GridView1.DataBind();
                    btInsert.Enabled = true;
                    //btUpdate.Enabled = false;
                    //btDelete.Enabled = false;
                    btCancel.Enabled = false;
                    btOK.Enabled = false;
                    GridView1.SelectedIndex = -1;
                    TextBox_DUTY_NO.Text = string.Empty;
                    TextBox_DUTY_NAME.Text = string.Empty;
                    ViewState.Remove("Mode");
                    C.controlEnable(ref this.Panel1, false);

                }
                catch (Exception)
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

        //protected void btDelete_Click(object sender, EventArgs e)
        //{
        //    if (GridView1.SelectedRow == null)
        //    {
        //        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請選取資料')", true);
        //        return;
        //    }
        //    else
        //    {
        //        string DeleteString = "DELETE FROM C_DUTYITEM WHERE DUTY_NO='" + GridView1.SelectedRow.Cells[0].Text.Trim() + "'";
        //        try
        //        {
        //            o_DBFactory.ABC_toTest.Edit_Data(DeleteString);
        //            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除成功')", true);

        //            //2010.06.04 LOG紀錄 by伊珊
        //            TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00004", TPMPermissions._boolTPM(), DeleteString);

        //            btDelete.Enabled = false;
        //            btOK.Enabled = false;
        //            btCancel.Enabled = false;
        //            btUpdate.Enabled = false;
        //            btInsert.Enabled = true;
        //            TextBox_DUTY_NO.Text = string.Empty;
        //            TextBox_DUTY_NAME.Text = string.Empty;
        //            GridView1.DataBind();
        //            GridView1.SelectedIndex = -1;
        //        }
        //        catch
        //        {
        //            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除失敗')", true);
        //        }
        //    }
        //}

        protected void btCancel_Click(object sender, EventArgs e)
        {
            GridView1.SelectedIndex = -1;
            btCancel.Enabled = false;
            //btUpdate.Enabled = false;
            btInsert.Enabled = true;
            //btDelete.Enabled = false;
            btOK.Enabled = false;
            TextBox_DUTY_NO.Text = string.Empty;
            TextBox_DUTY_NAME.Text = string.Empty;
            C.controlEnable(ref this.Panel1, false);

        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView1.SelectedIndex = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "Select")
            {
                TextBox_DUTY_NO.Text = GridView1.SelectedRow.Cells[0].Text.Trim();
                TextBox_DUTY_NAME.Text = GridView1.SelectedRow.Cells[1].Text.Trim();
                //btUpdate.Enabled = true;
                //btDelete.Enabled = true;
                btCancel.Enabled = true;
                btInsert.Enabled = false;
                btOK.Enabled = false;
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
