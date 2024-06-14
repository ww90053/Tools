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

using TPPDDB.App_Code; 

namespace TPPDDB._1_personnel
{
    public partial class Personal2_6 : System.Web.UI.Page
    {
       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                A.check_power();
            }
           
            ViewState["MZ_NO"] = Request["MZ_NO"];

            A.set_Panel_EnterToTAB(ref this.Panel_PRK3);
           

            if (!IsPostBack)
            {
                string selectCHKAD = "SELECT * FROM A_CHKAD WHERE MZ_AD='" + Session["ADPMZ_AD"].ToString() + "'";

                DataTable tempCHKAD = new DataTable();

                tempCHKAD = o_DBFactory.ABC_toTest.Create_Table(selectCHKAD, "GET");

                DropDownList_MZ_OAD.SelectedValue = Session["ADPMZ_AD"].ToString();

                if (tempCHKAD.Rows.Count == 1)
                {
                    TextBox_MZ_MASTER_POS.Text = tempCHKAD.Rows[0]["MZ_MASTER_POS"].ToString();
                    TextBox_MZ_MASTER_NAME.Text = tempCHKAD.Rows[0]["MZ_MASTER_NAME"].ToString();
                    TextBox_MZ_PRID.Text = tempCHKAD.Rows[0]["MZ_PRID"].ToString();
                }

                if (ViewState["XCOUNT"] != null)
                {
                    finddata(ViewState["MZ_NO"].ToString());

                    btUpdate.Enabled = true;
                    btInsert.Enabled = true;
                    btOK.Enabled = false;
                    btCancel.Enabled = false;
                    btDelete.Enabled = false;
                }

                if (ViewState["MZ_NO"] != null)
                {
                    string strSQL = "SELECT MZ_NO FROM A_PRK3 WHERE 1=1";

                    if (ViewState["MZ_NO"].ToString() != "")
                    {
                        strSQL += " AND MZ_NO='" + ViewState["MZ_NO"].ToString().Trim() + "'";
                    }
                    strSQL += " ORDER BY MZ_NO";

                    if (string.IsNullOrEmpty(o_DBFactory.ABC_toTest.vExecSQL(strSQL)))
                    {
                        Response.Write(@"<script language=javascript>window.alert('查無資料！');location.href('Personal2-1.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "');</script>");
                    }
                    else
                    {
                        finddata(ViewState["MZ_NO"].ToString());
                    }
                }

                A.controlEnable(ref this.Panel_PRK3, false);
            }
        }

      

        protected void finddata(string MZ_NO)
        {
            string strSQL = "SELECT * FROM A_PRK3 WHERE MZ_NO='" + MZ_NO + "'";

            DataTable temp = new DataTable();

            temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");

            TextBox_MZ_NO.Text = temp.Rows[0]["MZ_NO"].ToString();
            TextBox_MZ_DATE.Text = o_CommonService.Personal_ReturnDateString(temp.Rows[0]["MZ_DATE"].ToString());
            TextBox_MZ_PRID.Text = temp.Rows[0]["MZ_PRID"].ToString();
            TextBox_MZ_PRID1.Text = temp.Rows[0]["MZ_PRID1"].ToString();
            TextBox_MZ_MASTER_NAME.Text = temp.Rows[0]["MZ_MASTER_NAME"].ToString();
            TextBox_MZ_MASTER_POS.Text = temp.Rows[0]["MZ_MASTER_POS"].ToString();
            TextBox_MZ_CAUSE.Text = temp.Rows[0]["MZ_CAUSE"].ToString();
            TextBox_MZ_DESC.Text = temp.Rows[0]["MZ_DESC"].ToString();
            TextBox_MZ_MAX.Text = temp.Rows[0]["MZ_MAX"].ToString();
            TextBox_MZ_PRE.Text = temp.Rows[0]["MZ_PRE"].ToString();
            DropDownList_MZ_OAD.Text = temp.Rows[0]["MZ_OAD"].ToString();
            DropDownList_MZ_USER.Text = temp.Rows[0]["MZ_USER"].ToString();

            btDelete.Enabled = true;
            btUpdate.Enabled = true;


        }

        protected void btSearch_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal2_6_SEARCH.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "','查詢','top=190,left=200,width=350,height=300,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }

        protected void btInsert_Click(object sender, EventArgs e)
        {
            ViewState["Mode"] = "INSERT";

            ViewState["CMDSQL"] = "INSERT INTO " +
                                                "A_PRK3 (MZ_NO,MZ_DATE,MZ_PRID,MZ_PRID1,MZ_AD,MZ_MASTER_NAME,MZ_MASTER_POS," +
                                                        "MZ_CAUSE,MZ_DESC,MZ_MAX,MZ_PRE,MZ_OAD,MZ_USER) " +//MZ_SWT4,
                                                "VALUES (@MZ_NO,@MZ_DATE,@MZ_PRID,@MZ_PRID1,@MZ_AD,@MZ_MASTER_NAME,@MZ_MASTER_POS," +
                                                        ":MZ_CAUSE,@MZ_DESC,@MZ_MAX,@MZ_PRE,@MZ_OAD,@MZ_USER) ";//:MZ_SWT4,
            btInsert.Enabled = false;
            btUpdate.Enabled = false;
            btCancel.Enabled = true;
            btOK.Enabled = true;
            A.controlEnable(ref this.Panel_PRK3, true);
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_NO.ClientID + "').focus();$get('" + TextBox_MZ_NO.ClientID + "').focus();", true);
        }

        protected void btUpdate_Click(object sender, EventArgs e)
        {
            ViewState["Mode"] = "UPDATE";

            ViewState["CMDSQL"] = "UPDATE " +
                                            "A_PRK3 " +
                                  "SET " +
                                            "MZ_NO = @MZ_NO,MZ_DATE = @MZ_DATE,MZ_PRID = @MZ_PRID,MZ_PRID1 = @MZ_PRID1,MZ_AD = @MZ_AD," +
                                            "MZ_MASTER_NAME = @MZ_MASTER_NAME,MZ_MASTER_POS = @MZ_MASTER_POS," +
                                            "MZ_CAUSE = @MZ_CAUSE,MZ_DESC = @MZ_DESC,MZ_MAX = @MZ_MAX,MZ_PRE = @MZ_PRE," +//MZ_SWT4 = @MZ_SWT4,"+
                                            "MZ_OAD = @MZ_OAD,MZ_USER = @MZ_USER WHERE MZ_NO='" + o_str.tosql(TextBox_MZ_NO.Text.Trim()) + "'";

            HttpCookie Cookie1 = new HttpCookie("PKEY_MZ_NO");
            Cookie1.Value = TextBox_MZ_NO.Text;
            Response.Cookies.Add(Cookie1);

            btDelete.Enabled = true;
            btInsert.Enabled = false;
            btUpdate.Enabled = false;
            btCancel.Enabled = true;
            btOK.Enabled = true;

            A.controlEnable(ref this.Panel_PRK3, true);
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_NO.ClientID + "').focus();$get('" + TextBox_MZ_NO.ClientID + "').focus();", true);
        }

        protected void btOK_Click(object sender, EventArgs e)
        {

            string ErrorString = "";

            string old_NO = "NULL";

            if (ViewState["Mode"].ToString() == "UPDATE")
            {
                HttpCookie Cookie1 = new HttpCookie("PKEY_MZ_NO");
                Cookie1 = Request.Cookies["PKEY_MZ_NO"];
                old_NO = Cookie1.Value.ToString();
                Response.Cookies["PKEY_MZ_NO"].Expires = DateTime.Now.AddYears(-1);
            }
            string pkey_check;

            if (old_NO == TextBox_MZ_NO.Text && ViewState["Mode"].ToString() == "UPDATE")
                pkey_check = "0";
            else
                pkey_check = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_PRK2 WHERE MZ_NO='" + TextBox_MZ_NO.Text.Trim() + "'");

            if (pkey_check != "0")
            {
                ErrorString += "案號違反唯一值的條件" + "\\r\\n";
                TextBox_MZ_NO.BackColor = Color.Orange;
            }
            else
            {
                TextBox_MZ_NO.BackColor = Color.White;
            }
            //20141216 這應該是想做控制項檢核.但 DBDIC 又沒有資料
            //foreach (Object ob in Panel_PRK3.Controls)
            //{
            //    if (ob is TextBox)
            //    {
            //        TextBox tbox = (TextBox)ob;

            //        string result = o_Check_Dbdic.ErrorString(tbox.ID, "A_PRK3", tbox.Text);

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

            //        string result = o_Check_Dbdic.ErrorString(dlist.ID, "A_PRK3", dlist.Text);

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

            SqlParameter[] parameterList = {
                new SqlParameter("MZ_NO",SqlDbType.VarChar){Value = TextBox_MZ_NO.Text},
                new SqlParameter("MZ_DATE",SqlDbType.VarChar){Value =string.IsNullOrEmpty(TextBox_MZ_DATE.Text)?Convert.DBNull:TextBox_MZ_DATE.Text.Trim().Replace("/","").PadLeft(7,'0')},
                new SqlParameter("MZ_PRID",SqlDbType.VarChar){Value = TextBox_MZ_PRID.Text},
                new SqlParameter("MZ_PRID1",SqlDbType.VarChar){Value = TextBox_MZ_PRID1.Text},
                new SqlParameter("MZ_AD",SqlDbType.VarChar){Value = Session["ADPMZ_AD"].ToString()},
                new SqlParameter("MZ_MASTER_NAME",SqlDbType.VarChar){Value = TextBox_MZ_MASTER_NAME.Text},
                new SqlParameter("MZ_MASTER_POS",SqlDbType.VarChar){Value = TextBox_MZ_MASTER_POS.Text},
                new SqlParameter("MZ_CAUSE",SqlDbType.VarChar){Value = TextBox_MZ_CAUSE.Text},
                new SqlParameter("MZ_DESC",SqlDbType.VarChar){Value = TextBox_MZ_DESC.Text},
                new SqlParameter("MZ_MAX",SqlDbType.VarChar){Value = TextBox_MZ_MAX.Text},
                new SqlParameter("MZ_PRE",SqlDbType.VarChar){Value = TextBox_MZ_PRE.Text},
                new SqlParameter("MZ_OAD",SqlDbType.VarChar){Value = DropDownList_MZ_OAD.Text},
                new SqlParameter("MZ_USER",SqlDbType.VarChar){Value =  DropDownList_MZ_USER.Text}
                };
            try
            {
                o_DBFactory.ABC_toTest.ExecuteNonQuery( ViewState["CMDSQL"].ToString(), parameterList);

                if (ViewState["Mode"].ToString() == "INSERT")
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('新增成功');", true);
                }
                else if (ViewState["Mode"].ToString() == "UPDATE")
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('修改成功');", true);
                }
                btUpdate.Enabled = true;
                btInsert.Enabled = true;
                btOK.Enabled = false;
                btCancel.Enabled = false;
                btDelete.Enabled = false;
                ViewState.Remove("Mode");
                A.controlEnable(ref this.Panel_PRK3, false);
            }
            catch
            {
                if (ViewState["Mode"].ToString() == "INSERT")
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('新增失敗');", true);
                }
                else if (ViewState["Mode"].ToString() == "UPDATE")
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('修改失敗');location.href('Personal2-6.aspx?MZ_NO=" + TextBox_MZ_NO.ToString() + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "');", true);
                }
            }
        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            if (ViewState["Mode"].ToString() == "INSERT")
            {
                foreach (object dl in Panel_PRK3.Controls)
                {
                    if (dl is DropDownList)
                    {
                        DropDownList dl1 = dl as DropDownList;
                        dl1.SelectedValue = "";
                    }

                    if (dl is TextBox)
                    {
                        TextBox tbox = dl as TextBox;
                        tbox.Text = "";
                    }
                }
            }
            else if (ViewState["Mode"].ToString() == "UPDATE")
            {
                finddata(ViewState["MZ_NO"].ToString());
            }

            btInsert.Enabled = true;
            btCancel.Enabled = false;
            btOK.Enabled = false;
            btDelete.Enabled = false;
            A.controlEnable(ref this.Panel_PRK3, false);

        }

        protected void btDelete_Click(object sender, EventArgs e)
        {
            string DeleteString = "DELETE FROM A_PRK3 WHERE MZ_NO='" + o_str.tosql(TextBox_MZ_NO.Text.Trim()) + "'";
            try
            {
                o_DBFactory.ABC_toTest.Edit_Data(DeleteString);

                btUpdate.Enabled = true;
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除成功')", true);


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

        protected void btDESC_Click(object sender, EventArgs e)
        {
            Session["KTYPE_CID"] = TextBox_MZ_DESC1.ClientID;
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_Ktype_Search.aspx?MZ_KTYPE=NOTE&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin');", true);
        }

        protected void btCAUSE_Click(object sender, EventArgs e)
        {
            Session["KTYPE_CID"] = TextBox_MZ_CAUSE1.ClientID;
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_Ktype_Search.aspx?MZ_KTYPE=NOTE&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin');", true);
        }

        protected void TextBox_MZ_CAUSE1_TextChanged(object sender, EventArgs e)
        {
            TextBox_MZ_CAUSE.Text += Environment.NewLine + TextBox_MZ_CAUSE1.Text;
        }

        protected void TextBox_MZ_DESC1_TextChanged(object sender, EventArgs e)
        {
            TextBox_MZ_DESC.Text += Environment.NewLine + TextBox_MZ_DESC1.Text;
        }

        protected void TextBox_MZ_DATE_TextChanged(object sender, EventArgs e)
        {

            TextBox_MZ_DATE.Text = o_str.tosql(TextBox_MZ_DATE.Text.Trim().Replace("/", ""));

            if (TextBox_MZ_DATE.Text != "")
            {
                if (!DateManange.Check_date(TextBox_MZ_DATE.Text))
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('日期格式錯誤')", true);
                    TextBox_MZ_DATE.Text = o_CommonService.Personal_ReturnDateString(TextBox_MZ_DATE.Text.Trim());
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_DATE.ClientID + "').focus();$get('" + TextBox_MZ_DATE.ClientID + "').focus();", true);
                }
                else
                {
                    TextBox_MZ_DATE.Text = o_CommonService.Personal_ReturnDateString(TextBox_MZ_DATE.Text.Trim());
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_PRID.ClientID + "').focus();$get('" + TextBox_MZ_PRID.ClientID + "').focus();", true);
                }
            }
        }

        protected void CV_NO_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (string.IsNullOrEmpty(args.Value))
            {
                args.IsValid = false;
                TextBox_MZ_NO.BackColor = Color.Orange;
            }
            else
            {
                args.IsValid = true;
                TextBox_MZ_NO.BackColor = Color.White;
            }
        }

        protected void CV_DATE_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (string.IsNullOrEmpty(args.Value))
            {
                args.IsValid = false;
                TextBox_MZ_DATE.BackColor = Color.Orange;
            }
            else
            {
                args.IsValid = true;
                TextBox_MZ_DATE.BackColor = Color.White;
            }
        }

        protected void CV_PRID1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (string.IsNullOrEmpty(args.Value))
            {
                args.IsValid = false;
                TextBox_MZ_PRID1.BackColor = Color.Orange;
            }
            else
            {
                args.IsValid = true;
                TextBox_MZ_PRID1.BackColor = Color.White;
            }
        }

        int TPM_FION=0;

        protected void btPRINT_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBox_MZ_NO.Text.Trim()))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "location.href('Personal_Caserequest_rpt.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "');", true);
            }
            else
            {               

                string tmp_url = "A_rpt.aspx?fn=caserequest&TPM_FION=" + TPM_FION + "&NO1=" + o_str.tosql(TextBox_MZ_NO.Text.Trim());

                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
            }
        }
    }
}
