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
    public partial class C_ForLeaveOvertime_HolydaySet : System.Web.UI.Page
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

                Label1.Text = "國定假日設定";
            }
        }

        protected void execSQL()
        {
            //2010.06.07 by 伊珊
            string ErrorString = "";

            string old_HOLIDAY_DATE = "NULL";

            if (ViewState["Mode"].ToString() == "UPDATE")
            {
                HttpCookie Cookie1 = new HttpCookie("PKEY_MZ_HOLIDAY_DATE");
                Cookie1 = Request.Cookies["PKEY_MZ_HOLIDAY_DATE"];
                old_HOLIDAY_DATE = Cookie1.Value.ToString();
            }

            string pkey_check;

            if (old_HOLIDAY_DATE == TextBox_MZ_HOLIDAY_DATE.Text.Replace("/", string.Empty).PadLeft(7, '0').Trim() && ViewState["Mode"].ToString() == "UPDATE")
                pkey_check = "0";
            else
                pkey_check = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM C_DUTYHOLIDAY WHERE MZ_HOLIDAY_DATE='" + TextBox_MZ_HOLIDAY_DATE.Text.Trim() + "'");

            if (pkey_check != "0")
            {
                ErrorString += "國定假日日期違反唯一值的條件" + "\\r\\n";
                TextBox_MZ_HOLIDAY_DATE.BackColor = Color.Orange;
            }
            else
            {
                TextBox_MZ_HOLIDAY_DATE.BackColor = Color.White;
            }
            //20141216 這應該是想做控制項檢核.但 DBDIC 又沒有資料
            //foreach (Object ob in Panel1.Controls)
            //{
            //    if (ob is TextBox)
            //    {
            //        TextBox tbox = (TextBox)ob;

            //        string result = o_Check_Dbdic.ErrorString(tbox.ID, "C_DUTYHOLIDAY", tbox.Text);

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
                    cmd.Parameters.Add("MZ_HOLIDAY_DATE", SqlDbType.VarChar).Value = TextBox_MZ_HOLIDAY_DATE.Text.Trim().Replace("/", "").PadLeft(7, '0');
                    cmd.Parameters.Add("MZ_HOLIDAY_NAME", SqlDbType.VarChar).Value = TextBox_MZ_HOLIDAY_NAME.Text.Trim().ToUpper();
                    cmd.ExecuteNonQuery();

                    Response.Cookies["PKEY_MZ_HOLIDAY_DATE"].Expires = DateTime.Now.AddYears(-1);


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

                    TextBox_MZ_HOLIDAY_DATE.Text = string.Empty;
                    TextBox_MZ_HOLIDAY_NAME.Text = string.Empty;

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

        protected void btInsert_Click(object sender, EventArgs e)
        {
            ViewState["Mode"] = "INSERT";
            ViewState["CMDSQL"] = "INSERT INTO C_DUTYHOLIDAY(MZ_HOLIDAY_DATE,MZ_HOLIDAY_NAME) " +
                                                   " VALUES(@MZ_HOLIDAY_DATE,@MZ_HOLIDAY_NAME)";

            TextBox_MZ_HOLIDAY_DATE.Text = string.Empty;
            TextBox_MZ_HOLIDAY_NAME.Text = string.Empty;

            btCancel.Enabled = true;
            btOK.Enabled = true;
            btInsert.Enabled = false;
            btUpdate.Enabled = false;
            btDelete.Enabled = false;

            //by MQ 20100312---------
            C.controlEnable(ref this.Panel1, true);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_HOLIDAY_DATE.ClientID + "').focus();$get('" + TextBox_MZ_HOLIDAY_DATE.ClientID + "').focus();", true);
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
                ViewState["CMDSQL"] = "UPDATE C_DUTYHOLIDAY SET MZ_HOLIDAY_DATE=@MZ_HOLIDAY_DATE,MZ_HOLIDAY_NAME=@MZ_HOLIDAY_NAME " +
                                                         "  WHERE MZ_HOLIDAY_DATE='" + GridView1.SelectedRow.Cells[0].Text.Trim() + "'";

                HttpCookie Cookie1 = new HttpCookie("PKEY_MZ_HOLIDAY_DATE");
                Cookie1.Value = GridView1.SelectedRow.Cells[0].Text.Trim();
                Response.Cookies.Add(Cookie1);

                btDelete.Enabled = false;
                btUpdate.Enabled = false;
                btOK.Enabled = true;

                //by MQ 20100312---------
                C.controlEnable(ref this.Panel1, true);
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_HOLIDAY_DATE.ClientID + "').focus();$get('" + TextBox_MZ_HOLIDAY_DATE.ClientID + "').focus();", true);
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
                string DeleteString = "DELETE FROM C_DUTYHOLIDAY WHERE MZ_HOLIDAY_DATE='" + GridView1.SelectedRow.Cells[0].Text.Trim() + "'";
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
                    TextBox_MZ_HOLIDAY_DATE.Text = string.Empty;
                    TextBox_MZ_HOLIDAY_NAME.Text = string.Empty;
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
            btDelete.Enabled = false;
            btOK.Enabled = false;
            btCancel.Enabled = false;
            btUpdate.Enabled = false;
            btInsert.Enabled = true;
            TextBox_MZ_HOLIDAY_DATE.Text = string.Empty;
            TextBox_MZ_HOLIDAY_NAME.Text = string.Empty;

            //by MQ 20100312---------
            C.controlEnable(ref this.Panel1, false);
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView1.SelectedIndex = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "Select")
            {
                btOK.Enabled = false;
                btCancel.Enabled = true;
                btUpdate.Enabled = true;
                btDelete.Enabled = true;
                btInsert.Enabled = false;

                TextBox_MZ_HOLIDAY_DATE.Text = o_CommonService.Personal_ReturnDateString(GridView1.SelectedRow.Cells[0].Text.Trim());
                TextBox_MZ_HOLIDAY_NAME.Text = GridView1.SelectedRow.Cells[1].Text.Trim();
            }
        }

        protected void btOK_Click(object sender, EventArgs e)
        {
            //執行新增或更新的SQL語法
            execSQL();
            string MZ_DATE = TextBox_MZ_HOLIDAY_DATE.Text.Trim().Replace("/", "").PadLeft(7, '0');
            //刷新行事曆資料表
            //指定日期,刷新當天
            LogicService_DUTYHOLIDAY.CreateAndSave_InDB(MZ_DATE);
            //自動判斷並且補上行事曆資料,以目前日期為基準,往後一年,往前三年的資料都補上
            //如果當年已經有365筆資料,就不會異動,不滿365者會自動重算
            LogicService_DUTYHOLIDAY.CreateAndSave_InDB_AllYear();
        }
    }
}
