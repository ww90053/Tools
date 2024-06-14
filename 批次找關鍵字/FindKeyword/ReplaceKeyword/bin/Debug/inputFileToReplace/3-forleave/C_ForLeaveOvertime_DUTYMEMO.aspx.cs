using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Drawing;

namespace TPPDDB._3_forleave
{
    public partial class C_ForLeaveOvertime_DUTYMEMO : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (!IsPostBack)
            {

                C.check_power();

                string sql = @"SELECT AKD.MZ_KCHI MZ_EXAD,  AKU.MZ_KCHI MZ_EXUNIT
                             FROM A_DLBASE
                            LEFT JOIN  A_KTYPE AKD ON AKD.MZ_KCODE=MZ_EXAD AND AKD.MZ_KTYPE='04' 
                            LEFT JOIN  A_KTYPE AKU ON AKU.MZ_KCODE=MZ_EXUNIT AND AKU.MZ_KTYPE='25' 
                            WHERE MZ_ID='" + Session["ADPMZ_ID"].ToString() + "'";

                DataTable ad_unit = o_DBFactory.ABC_toTest.Create_Table(sql, "get");
                if (ad_unit.Rows.Count > 0)
                    Label1.Text = ad_unit.Rows[0]["MZ_EXAD"].ToString() + ad_unit.Rows[0]["MZ_EXUNIT"].ToString() + "重點記事";


                C.controlEnable(ref this.Panel1, false);
            }
        }

        protected void execSQL()
        {
            if (ViewState["Mode"].ToString() == "INSERT")
            {
                //2010.06.07 by 伊珊
                string ErrorString = "";

                string old_DATE = "NULL";

                if (ViewState["Mode"].ToString() == "UPDATE")
                {
                    HttpCookie Cookie1 = new HttpCookie("PKEY_MZ_DATE");
                    Cookie1 = Request.Cookies["PKEY_MZ_DATE"];
                    old_DATE = Cookie1.Value.ToString();
                }

                string pkey_check;

                if (old_DATE == TextBox_MZ_DATE.Text.Trim().Replace("/", string.Empty).PadLeft(7, '0') && ViewState["Mode"].ToString() == "UPDATE")
                    pkey_check = "0";
                else 
                    pkey_check = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM C_DUTYMEMO WHERE MZ_DATE='" + TextBox_MZ_DATE.Text.Trim() + "' AND MZ_AD='" + Session["ADPMZ_EXAD"].ToString() + "'AND MZ_UNIT='" + Session["ADPMZ_EXUNIT"].ToString() + "'");

                if (pkey_check != "0")
                {
                    ErrorString += "每一單位之每一部門一天只能新增一筆" + "\\r\\n";
                    TextBox_MZ_DATE.BackColor = Color.Orange;
                }
                //20141216 這應該是想做控制項檢核.但 DBDIC 又沒有資料
                //foreach (Object ob in Panel1.Controls)
                //{
                //    if (ob is TextBox)
                //    {
                //        TextBox tbox = (TextBox)ob;

                //        string result = o_Check_Dbdic.ErrorString(tbox.ID, "C_DUTYMEMO", tbox.Text);

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
                    cmd.Parameters.Add("MZ_DATE", SqlDbType.VarChar).Value = TextBox_MZ_DATE.Text.Trim().Replace("/", "").PadLeft(7, '0');
                    cmd.Parameters.Add("MZ_MEMO", SqlDbType.VarChar).Value = TextBox_MZ_MEMO.Text.Trim();
                    cmd.ExecuteNonQuery();
                    
                    Response.Cookies["PKEY_MZ_DATE"].Expires = DateTime.Now.AddYears(-1);
                    
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
                        TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00002", TPMPermissions._boolTPM(), o_DBFactory.ABC_toTest.RegexSQL(cmd));
                    }
                    GridView1.DataBind();
                    GridView1.SelectedIndex = -1;
                    btInsert.Enabled = true;
                    btUpdate.Enabled = false;
                    btDelete.Enabled = false;
                    btCancel.Enabled = false;
                    btOK.Enabled = false;
                    ViewState.Remove("Mode");
                    C.controlEnable(ref this.Panel1, false);

                }
                catch
                {
                    if (ViewState["Mode"].ToString() == "INSERT")
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('新增失敗，日期重覆');", true);
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


        protected void btInsert_Click(object sender, EventArgs e)
        {
            ViewState["Mode"] = "INSERT";
            ViewState["CMDSQL"] = "INSERT INTO " +
                                               "C_DUTYMEMO(MZ_AD,MZ_UNIT,MZ_DATE,MZ_MEMO) " +
                                  " VALUES " +
                                               "(@MZ_AD,@MZ_UNIT,@MZ_DATE,@MZ_MEMO)";

            TextBox_MZ_DATE.Text = string.Empty;
            //TextBox_MZ_MEMO.Text = string.Empty;
            btInsert.Enabled = false;
            btUpdate.Enabled = false;
            btDelete.Enabled = false;
            btCancel.Enabled = true;
            btOK.Enabled = true;
            C.controlEnable(ref this.Panel1, true);
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_DATE.ClientID + "').focus();$get('" + TextBox_MZ_DATE.ClientID + "').focus();", true);

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

                ViewState["CMDSQL"] = "UPDATE " +
                                                 "C_DUTYMEMO " +
                                      "SET " +
                                                 "MZ_AD=@MZ_AD,MZ_UNIT=@MZ_UNIT,MZ_DATE=@MZ_DATE,MZ_MEMO=@MZ_MEMO" +
                                      " WHERE " +
                                                 "  MZ_AD='" + Session["ADPMZ_EXAD"].ToString() +
                                                 "' AND MZ_UNIT='" + Session["ADPMZ_EXUNIT"].ToString() +
                                                 "' AND MZ_DATE='" + GridView1.SelectedRow.Cells[0].Text.Trim().Replace("/", "").PadLeft(7, '0') + "'";

                HttpCookie Cookie1 = new HttpCookie("PKEY_MZ_DATE");
                Cookie1.Value = GridView1.SelectedRow.Cells[0].Text.Trim().Replace("/", "").PadLeft(7, '0');
                Response.Cookies.Add(Cookie1);

                btUpdate.Enabled = false;
                btDelete.Enabled = false;
                btOK.Enabled = true;
                C.controlEnable(ref this.Panel1, true);
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_DATE.ClientID + "').focus();$get('" + TextBox_MZ_DATE.ClientID + "').focus();", true);

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
                string DeleteString = "DELETE " +
                                      "FROM " +
                                                "C_DUTYMEMO " +
                                      "WHERE " +
                                                "  MZ_AD='" + Session["ADPMZ_EXAD"].ToString() +
                                                "' AND MZ_UNIT='" + Session["ADPMZ_EXUNIT"].ToString() +
                                                "' AND MZ_DATE='" + GridView1.SelectedRow.Cells[0].Text.Trim().Replace("/", "").PadLeft(7, '0') + "'";
                try
                {
                    o_DBFactory.ABC_toTest.Edit_Data(DeleteString);
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除成功')", true);

                    //2010.06.04 LOG紀錄 by伊珊
                    TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00004", TPMPermissions._boolTPM(), DeleteString);

                    btCancel.Enabled = false;
                    btUpdate.Enabled = false;
                    btInsert.Enabled = true;
                    btDelete.Enabled = false;
                    btOK.Enabled = false;
                    TextBox_MZ_DATE.Text = string.Empty;
                    TextBox_MZ_MEMO.Text = string.Empty;
                    GridView1.DataBind();
                    GridView1.SelectedIndex = -1;
                }
                catch
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除失敗')", true);
                }
            }
        }

        protected void btOK_Click(object sender, EventArgs e)
        {
            execSQL();
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView1.SelectedIndex = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "Select")
            {
                btUpdate.Enabled = true;
                btDelete.Enabled = true;
                btInsert.Enabled = false;
                btOK.Enabled = false;
                btCancel.Enabled = true;

                TextBox_MZ_DATE.Text = GridView1.SelectedRow.Cells[0].Text.Trim();
                TextBox_MZ_MEMO.Text = GridView1.SelectedRow.Cells[1].Text.Trim();
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GridView1, "Select$" + e.Row.RowIndex);
                e.Row.Cells[2].Attributes.Add("Style", "display:none");
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Text = (e.Row.Cells[0].Text.Substring(0, 1) == "0" ? e.Row.Cells[0].Text.Substring(1, 2) + "/" + e.Row.Cells[0].Text.Substring(3, 2) + "/" + e.Row.Cells[0].Text.Substring(5, 2) : e.Row.Cells[0].Text.Substring(0, 3) + "/" + e.Row.Cells[0].Text.Substring(3, 2) + "/" + e.Row.Cells[0].Text.Substring(5, 2));
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
            TextBox_MZ_DATE.Text = string.Empty;
            TextBox_MZ_MEMO.Text = string.Empty;
            C.controlEnable(ref this.Panel1, false);
        }
    }
}
