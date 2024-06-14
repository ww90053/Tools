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
    public partial class C_ForLeaveOvertime_DUTYTARGETSET : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                C.check_power();
            

            //MQ ---------------------20100331   
            C.set_Panel_EnterToTAB(ref this.Panel1);
            string sql = @"SELECT AKD.MZ_KCHI MZ_EXAD,  AKU.MZ_KCHI MZ_EXUNIT
                             FROM A_DLBASE
                            LEFT JOIN  A_KTYPE AKD ON AKD.MZ_KCODE=MZ_EXAD AND AKD.MZ_KTYPE='04' 
                            LEFT JOIN  A_KTYPE AKU ON AKU.MZ_KCODE=MZ_EXUNIT AND AKU.MZ_KTYPE='25' 
                            WHERE MZ_ID='" + Session["ADPMZ_ID"].ToString() + "'";

            DataTable ad_unit = o_DBFactory.ABC_toTest.Create_Table(sql, "get");
            if (ad_unit.Rows.Count > 0)
                Label1.Text = ad_unit.Rows[0]["MZ_EXAD"].ToString() + ad_unit.Rows[0]["MZ_EXUNIT"].ToString() + "任務目標設定";
            
            
                C.controlEnable(ref this.Panel1, false);
            }
        }

        protected void execSQL()
        {
            if (ViewState["Mode"].ToString() == "INSERT")
            {
                //2010.06.07 by 伊珊
                string ErrorString = "";

                string old_DUTYTARGET_NO = "NULL";

                if (ViewState["Mode"].ToString() == "UPDATE")
                {
                    HttpCookie Cookie1 = new HttpCookie("PKEY_MZ_DUTYTARGET_NO");
                    Cookie1 = Request.Cookies["PKEY_MZ_DUTYTARGET_NO"];
                    old_DUTYTARGET_NO = Cookie1.Value.ToString();
                }

                string pkey_check;

                if (old_DUTYTARGET_NO == TextBox_MZ_DUTYTARGET_NO.Text.Trim() && ViewState["Mode"].ToString() == "UPDATE")
                    pkey_check = "0";
                else
                    pkey_check = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM C_DUTYTARGET WHERE MZ_DUTYTARGET_NO='" + TextBox_MZ_DUTYTARGET_NO.Text.Trim() + "'");

                if (pkey_check != "0")
                {
                    ErrorString += "任務目標編號違反唯一值條件。" + "\\r\\n";
                    TextBox_MZ_DUTYTARGET_NO.BackColor = Color.Orange;
                }
                else
                {
                    TextBox_MZ_DUTYTARGET_NO.BackColor = Color.White;
                }
                ////20141216 這應該是想做控制項檢核.但 DBDIC 又沒有資料
                //foreach (Object ob in Panel1.Controls)
                //{
                //    if (ob is TextBox)
                //    {
                //        TextBox tbox = (TextBox)ob;

                //        string result = o_Check_Dbdic.ErrorString(tbox.ID, "C_DUTYTARGET", tbox.Text);

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
            }
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(ViewState["CMDSQL"].ToString(), conn);
                cmd.CommandType = CommandType.Text;
                try
                {
                    cmd.Parameters.Add("MZ_AD", SqlDbType.VarChar).Value = Session["ADPMZ_EXAD"].ToString();
                    cmd.Parameters.Add("MZ_UNIT", SqlDbType.VarChar).Value = Session["ADPMZ_EXUNIT"].ToString();
                    cmd.Parameters.Add("MZ_DUTYTARGET_NO", SqlDbType.VarChar).Value = TextBox_MZ_DUTYTARGET_NO.Text.Trim().ToUpper();
                    cmd.Parameters.Add("MZ_DUTYTARGET", SqlDbType.VarChar).Value = TextBox_MZ_DUTYTARGET.Text.Trim().ToUpper();
                    cmd.ExecuteNonQuery();

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
                    btCancel.Enabled = false;
                    btInsert.Enabled = true;
                    btUpdate.Enabled = false;
                    btDelete.Enabled = false;
                    btOK.Enabled = false;
                    ViewState.Remove("Mode");
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
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('修改失敗');", true);
                    }
                }
                finally
                {
                    conn.Close();

                    //XX2013/06/18 
                    conn.Dispose();
                }
                btCancel.Enabled = false;
            }
        }

        protected void btInsert_Click(object sender, EventArgs e)
        {
            ViewState["Mode"] = "INSERT";
            ViewState["CMDSQL"] = "INSERT INTO C_DUTYTARGET(MZ_AD,MZ_UNIT,MZ_DUTYTARGET_NO,MZ_DUTYTARGET) " +
                                                   " VALUES(@MZ_AD,@MZ_UNIT,@MZ_DUTYTARGET_NO,@MZ_DUTYTARGET)";

            btCancel.Enabled = true;
            btOK.Enabled = true;
            btDelete.Enabled = false;
            btInsert.Enabled = false;
            btUpdate.Enabled = false;
            C.controlEnable(ref this.Panel1, true);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_DUTYTARGET_NO.ClientID + "').focus();$get('" + TextBox_MZ_DUTYTARGET_NO.ClientID + "').focus();", true);
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
                ViewState["CMDSQL"] = "UPDATE C_DUTYTARGET SET MZ_AD=@MZ_AD,MZ_UNIT=@MZ_UNIT,MZ_DUTYTARGET_NO=@MZ_DUTYTARGET_NO," +
                                                            "  MZ_DUTYTARGET=@MZ_DUTYTARGET" +
                                                            "  WHERE " +
                                                            "  MZ_AD='" + Session["ADPMZ_EXAD"].ToString() +
                                                            "' AND MZ_UNIT='" + Session["ADPMZ_EXUNIT"].ToString() +
                                                            "' AND MZ_DUTYTARGET_NO='" + GridView1.SelectedRow.Cells[0].Text.Trim() + "'";

                HttpCookie Cookie1 = new HttpCookie("PKEY_MZ_DUTYTARGET_NO");
                Cookie1.Value = GridView1.SelectedRow.Cells[0].Text.Trim();
                Response.Cookies.Add(Cookie1);

                btDelete.Enabled = false;
                btUpdate.Enabled = false;
                btOK.Enabled = true;
                C.controlEnable(ref this.Panel1, true);
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_DUTYTARGET_NO.ClientID + "').focus();$get('" + TextBox_MZ_DUTYTARGET_NO.ClientID + "').focus();", true);
            }
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
                string DeleteString = "DELETE FROM C_DUTYTARGET WHERE " +
                                                                      "  MZ_AD='" + Session["ADPMZ_EXAD"].ToString() +
                                                                      "' AND MZ_UNIT='" + Session["ADPMZ_EXUNIT"].ToString() +
                                                                      "' AND MZ_DUTYTARGET_NO='" + GridView1.SelectedRow.Cells[0].Text.Trim() + "'";
                try
                {
                    o_DBFactory.ABC_toTest.Edit_Data(DeleteString);
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除成功')", true);

                    //2010.06.04 LOG紀錄 by伊珊
                    TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00004", TPMPermissions._boolTPM(), DeleteString);

                    btOK.Enabled = false;
                    btDelete.Enabled = false;
                    btCancel.Enabled = false;
                    btUpdate.Enabled = false;
                    btInsert.Enabled = true;
                    TextBox_MZ_DUTYTARGET.Text = string.Empty;
                    TextBox_MZ_DUTYTARGET_NO.Text = string.Empty;
                    GridView1.DataBind();
                    GridView1.SelectedIndex = -1;
                }
                catch
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除失敗')", true);
                }
            }
        }
        protected void btCancel_Click(object sender, EventArgs e)
        {
            GridView1.SelectedIndex = -1;
            btOK.Enabled = false;
            btDelete.Enabled = false;
            btCancel.Enabled = false;
            btUpdate.Enabled = false;
            btInsert.Enabled = true;
            TextBox_MZ_DUTYTARGET.Text = string.Empty;
            TextBox_MZ_DUTYTARGET_NO.Text = string.Empty;
            C.controlEnable(ref this.Panel1, false);

        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView1.SelectedIndex = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "Select")
            {
                btUpdate.Enabled = true;
                btDelete.Enabled = true;
                btCancel.Enabled = true;
                btOK.Enabled = false;
                btInsert.Enabled = false;

                TextBox_MZ_DUTYTARGET_NO.Text = GridView1.SelectedRow.Cells[0].Text.Trim();
                TextBox_MZ_DUTYTARGET.Text = GridView1.SelectedRow.Cells[1].Text.Trim();
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
