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
    public partial class C_DLCODE_SETDAY : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                C.check_power();
            

            //by MQ 20100312---------   
            C.set_Panel_EnterToTAB(ref this.Panel1);
            
                //by MQ 20100312---------
                C.controlEnable(ref this.Panel1, false);

                Label1.Text = "假別日數設定";
            }
        }

        protected void TextBox_MZ_CODE_TextChanged(object sender, EventArgs e)
        {
            TextBox_MZ_CODE.Text = TextBox_MZ_CODE.Text.ToUpper();

            string CName = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_CNAME FROM C_DLCODE WHERE MZ_CODE='" + TextBox_MZ_CODE.Text.Trim() + "'");


            //檢查代碼中文名稱是否存在
            if (string.IsNullOrEmpty(CName))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('查無資料')", true);
                TextBox_MZ_CODE1.Text = string.Empty;
                TextBox_MZ_CODE.Text = string.Empty;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_CODE.ClientID + "').focus();", true);
            }
            else
            {
                TextBox_MZ_CODE1.Text = CName;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_CODE.ClientID + "').focus();", true);

            }
        }

        protected void btCODE_Click(object sender, EventArgs e)
        {
            Session["KTYPE_CID"] = TextBox_MZ_CODE.ClientID;
            Session["KTYPE_CID1"] = TextBox_MZ_CODE1.ClientID;
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('../1-personnel/Personal_Ktype_Search.aspx?CID_NAME=MZ_CODE&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin','top=190,left=200,width=420,height=450,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }

        protected void execSQL()
        {
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
            else
                pkey_check = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM C_DLCODE_SETDAY WHERE MZ_CODE='" + TextBox_MZ_CODE.Text.Trim() + "'");

            if (pkey_check != "0")
            {
                ErrorString += "假別違反唯一值的條件" + "\\r\\n";
                TextBox_MZ_CODE.BackColor = Color.Orange;
            }
            else
            {
                TextBox_MZ_CODE.BackColor = Color.White;
            }
            //20141216 這應該是想做控制項檢核.但 DBDIC 又沒有資料
            //foreach (Object ob in Panel1.Controls)
            //{
            //    if (ob is TextBox)
            //    {
            //        TextBox tbox = (TextBox)ob;

            //        string result = o_Check_Dbdic.ErrorString(tbox.ID, "C_DLCODE_SETDAY", tbox.Text);
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


            try
            {
                SqlParameter[] parameterList = {
                new SqlParameter("MZ_CODE",SqlDbType.VarChar){Value = TextBox_MZ_CODE.Text},
                new SqlParameter("MZ_DAY",SqlDbType.Float){Value = TextBox_MZ_DAY.Text}
                };
       
                o_DBFactory.ABC_toTest.ExecuteNonQuery( ViewState["CMDSQL"].ToString(), parameterList);

                Response.Cookies["PKEY_MZ_CODE"].Expires = DateTime.Now.AddYears(-1);

                if (ViewState["Mode"].ToString() == "INSERT")
                {

                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('新增成功');", true);

                    //2010.06.04 LOG紀錄 by伊珊
                    TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00002", TPMPermissions._boolTPM(), o_DBFactory.ABC_toTest.RegexSQL(ViewState["CMDSQL"].ToString(), parameterList));
                }
                else if (ViewState["Mode"].ToString() == "UPDATE")
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('更正成功');", true);

                    //2010.06.04 LOG紀錄 by伊珊
                    TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00003", TPMPermissions._boolTPM(), o_DBFactory.ABC_toTest.RegexSQL(ViewState["CMDSQL"].ToString(), parameterList));

                }
                GridView1.DataBind();

                TextBox_MZ_CODE.Text = string.Empty;
                TextBox_MZ_CODE1.Text = string.Empty;
                TextBox_MZ_DAY.Text = string.Empty;

                btInsert.Enabled = true;
                btUpdate.Enabled = false;
                btDelete.Enabled = false;
                btOK.Enabled = false;
                btCancel.Enabled = false;
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
            btCancel.Enabled = false;
        }

        protected void btInsert_Click(object sender, EventArgs e)
        {

            ViewState["Mode"] = "INSERT";
            ViewState["CMDSQL"] = "INSERT INTO C_DLCODE_SETDAY(MZ_CODE,MZ_DAY) " +
                                                     " VALUES(@MZ_CODE,@MZ_DAY)";

            TextBox_MZ_CODE.Text = string.Empty;
            TextBox_MZ_CODE1.Text = string.Empty;
            TextBox_MZ_DAY.Text = string.Empty;

            btInsert.Enabled = false;
            btUpdate.Enabled = false;
            btDelete.Enabled = false;
            btCancel.Enabled = true;
            btOK.Enabled = true;

            //by MQ 20100312---------
            C.controlEnable(ref this.Panel1, true);
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_CODE.ClientID + "').focus();", true);
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
                ViewState["Mode"] = "UPDATE";

                ViewState["CMDSQL"] = "UPDATE C_DLCODE_SETDAY SET MZ_CODE = @MZ_CODE,MZ_DAY = @MZ_DAY WHERE MZ_CODE='" + GridView1.SelectedRow.Cells[0].Text.Trim() + "'";

                HttpCookie Cookie1 = new HttpCookie("PKEY_MZ_CODE");
                Cookie1.Value = TextBox_MZ_CODE.Text;
                Response.Cookies.Add(Cookie1);

                btUpdate.Enabled = false;
                btDelete.Enabled = false;
                btOK.Enabled = true;

                //by MQ 20100312---------
                C.controlEnable(ref this.Panel1, true);
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_CODE.ClientID + "').focus();", true);
            }
        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            GridView1.SelectedIndex = -1;
            btCancel.Enabled = false;
            btDelete.Enabled = false;
            btUpdate.Enabled = false;
            btInsert.Enabled = true;
            btOK.Enabled = false;
            TextBox_MZ_CODE.Text = string.Empty;
            TextBox_MZ_CODE1.Text = string.Empty;
            TextBox_MZ_DAY.Text = string.Empty;

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
                string DeleteString = "DELETE FROM C_DLCODE_SETDAY WHERE MZ_CODE='" + TextBox_MZ_CODE.Text.Trim() + "'";
                try
                {
                    o_DBFactory.ABC_toTest.Edit_Data(DeleteString);
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除成功')", true);

                    //2010.06.04 LOG紀錄 by伊珊
                    TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00004", TPMPermissions._boolTPM(), DeleteString);

                    GridView1.SelectedIndex = -1;
                    btDelete.Enabled = false;
                    btCancel.Enabled = false;
                    btUpdate.Enabled = false;
                    btInsert.Enabled = true;
                    btOK.Enabled = false;
                    TextBox_MZ_CODE.Text = string.Empty;
                    TextBox_MZ_CODE1.Text = string.Empty;
                    TextBox_MZ_DAY.Text = string.Empty;
                    GridView1.DataBind();
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
                btDelete.Enabled = true;
                btCancel.Enabled = true;
                btInsert.Enabled = false;
                btOK.Enabled = false;

                TextBox_MZ_CODE.Text = GridView1.SelectedRow.Cells[0].Text.Trim();
                TextBox_MZ_CODE1.Text = GridView1.SelectedRow.Cells[1].Text.Trim();
                TextBox_MZ_DAY.Text = GridView1.SelectedRow.Cells[2].Text.Trim();
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GridView1, "Select$" + e.Row.RowIndex);
                e.Row.Cells[0].Attributes.Add("Style", "display:none");
                e.Row.Cells[3].Attributes.Add("Style", "display:none");
            }
        }

        protected void CV_CODE_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (string.IsNullOrEmpty(args.Value))
            {
                args.IsValid = false;
                TextBox_MZ_CODE.BackColor = Color.Orange;
            }
            else
            {
                args.IsValid = true;
                TextBox_MZ_CODE.BackColor = Color.White;
            }
        }

        protected void btOK_Click(object sender, EventArgs e)
        {
            execSQL();
        }
    }
}
