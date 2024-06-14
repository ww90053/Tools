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


namespace TPPDDB._1_personnel
{
    public partial class Personal4_3 : System.Web.UI.Page
    {
        List<String> SWT3_AD = new List<string>();
        List<String> SWT3_UNIT = new List<string>();
        List<String> SWT3_OCCC = new List<string>();
        List<String> SWT3_TBDV = new List<string>();
        List<String> SWT3_PRRST = new List<string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                A.check_power();            
                //by MQ 20100311---------
            A.set_Panel_EnterToTAB(ref this.Panel_SWT3);
                //by MQ 20100311---------
                A.controlEnable(ref this.Panel_SWT3, false);

                
            }

        }

       

        protected void btInsert_Click(object sender, EventArgs e)
        {
            //by MQ 20100311---------
            A.controlEnable(ref this.Panel_SWT3, true);
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_AD.ClientID + "').focus();$get('" + TextBox_MZ_AD.ClientID + "').focus();", true);


            TextBox_MZ_AD.Text = string.Empty;
            TextBox_MZ_AD1.Text = string.Empty;
            TextBox_MZ_OCCC.Text = string.Empty;
            TextBox_MZ_OCCC1.Text = string.Empty;
            TextBox_MZ_PRRST.Text = string.Empty;
            TextBox_MZ_PRRST1.Text = string.Empty;
            TextBox_MZ_TBDV.Text = string.Empty;
            TextBox_MZ_TBDV1.Text = string.Empty;
            TextBox_MZ_UNIT.Text = string.Empty;
            TextBox_MZ_UNIT1.Text = string.Empty;

            ViewState["CMDSQL"] = "INSERT INTO A_SWT3(MZ_AD,MZ_UNIT,MZ_OCCC,MZ_TBDV,MZ_PRRST) VALUES(@MZ_AD,@MZ_UNIT,@MZ_OCCC,@MZ_TBDV,@MZ_PRRST)";

            ViewState["Mode"] = "INSERT";


            btOK.Enabled = true;
            btDelete.Enabled = false;
            btUpdate.Enabled = false;
            btInsert.Enabled = false;
            btCancel.Enabled = true;

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
                ViewState["CMDSQL"] = "UPDATE A_SWT3 SET MZ_AD=@MZ_AD,MZ_UNIT=@MZ_UNIT,MZ_OCCC=@MZ_OCCC,MZ_TBDV=@MZ_TBDV,MZ_PRRST=@MZ_PRRST WHERE MZ_AD='" + GridView1.SelectedRow.Cells[0].Text + "' AND MZ_UNIT='" + GridView1.SelectedRow.Cells[1].Text + "' AND MZ_OCCC='" + GridView1.SelectedRow.Cells[2].Text + "' AND MZ_TBDV='" + GridView1.SelectedRow.Cells[3].Text + "' AND MZ_PRRST='" + GridView1.SelectedRow.Cells[4].Text + "'";

                ViewState["Mode"] = "UPDATE";

                HttpCookie Cookie1 = new HttpCookie("PKEY_MZ_AD");
                Cookie1.Value = TextBox_MZ_AD.Text;
                Response.Cookies.Add(Cookie1);

                HttpCookie Cookie2 = new HttpCookie("PKEY_MZ_UNIT");
                Cookie2.Value = TextBox_MZ_UNIT.Text;
                Response.Cookies.Add(Cookie2);

                HttpCookie Cookie3 = new HttpCookie("PKEY_MZ_OCCC");
                Cookie3.Value = TextBox_MZ_OCCC.Text;
                Response.Cookies.Add(Cookie3);

                HttpCookie Cookie4 = new HttpCookie("PKEY_MZ_TBDV");
                Cookie4.Value = TextBox_MZ_TBDV.Text;
                Response.Cookies.Add(Cookie4);

                HttpCookie Cookie5 = new HttpCookie("PKEY_MZ_PRRST");
                Cookie5.Value = TextBox_MZ_PRRST.Text;
                Response.Cookies.Add(Cookie5);

                btUpdate.Enabled = false;
                btDelete.Enabled = false;
                btOK.Enabled = true;

                //by MQ 20100311---------
                A.controlEnable(ref this.Panel_SWT3, true);
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_AD.ClientID + "').focus();$get('" + TextBox_MZ_AD.ClientID + "').focus();", true);

            }
        }

        protected void execSQL()
        {
            //2010.06.07 by 伊珊
            string ErrorString = "";

            string old_AD = "NULL";

            string old_UNIT = "NULL";

            string old_OCCC = "NULL";

            string old_PRRST = "NULL";

            string old_TBDV = "NULL";

            if (ViewState["Mode"].ToString() == "UPDATE")
            {
                HttpCookie Cookie1 = new HttpCookie("PKEY_MZ_AD");
                Cookie1 = Request.Cookies["PKEY_MZ_AD"];
                old_AD = Cookie1.Value.ToString();

                HttpCookie Cookie2 = new HttpCookie("PKEY_MZ_UNIT");
                Cookie2 = Request.Cookies["PKEY_MZ_UNIT"];
                old_UNIT = Cookie2.Value.ToString();

                HttpCookie Cookie3 = new HttpCookie("PKEY_MZ_OCCC");
                Cookie3 = Request.Cookies["PKEY_MZ_OCCC"];
                old_OCCC = Cookie3.Value.ToString();

                HttpCookie Cookie4 = new HttpCookie("PKEY_MZ_TBDV");
                Cookie4 = Request.Cookies["PKEY_MZ_TBDV"];
                old_TBDV = Cookie4.Value.ToString();

                HttpCookie Cookie5 = new HttpCookie("PKEY_MZ_PRRST");
                Cookie5 = Request.Cookies["PKEY_MZ_PRRST"];
                old_PRRST = Cookie5.Value.ToString();
            }

            string pkey_check;

            if (old_AD == TextBox_MZ_AD.Text && old_UNIT == TextBox_MZ_UNIT.Text && old_OCCC == TextBox_MZ_OCCC.Text && old_TBDV == TextBox_MZ_TBDV.Text && old_PRRST == TextBox_MZ_PRRST.Text && ViewState["Mode"].ToString() == "UPDATE")
                pkey_check = "0";
            else
                pkey_check = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_SWT3 WHERE MZ_OCCC='" + TextBox_MZ_OCCC.Text.Trim() + "'AND MZ_TBDV='" + TextBox_MZ_TBDV.Text.Trim() + "'AND MZ_PRRST='" + TextBox_MZ_PRRST.Text.Trim() + "'AND MZ_AD='" + TextBox_MZ_AD.Text.Trim() + "'AND MZ_UNIT='" + TextBox_MZ_UNIT.Text.Trim() + "'");

            if (pkey_check != "0")
                ErrorString += "編制機關與編制單位與職稱與職序與獎懲結果違反唯一值的條件" + "\\r\\n";
            //20141216 這應該是想做控制項檢核.但 DBDIC 又沒有資料
            //foreach (Object ob in Panel_SWT3.Controls)
            //{
            //    if (ob is TextBox)
            //    {
            //        TextBox tbox = (TextBox)ob;

            //        string result = o_Check_Dbdic.ErrorString(tbox.ID, "A_SWT3", tbox.Text);

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

                cmd.Parameters.Add("MZ_AD", SqlDbType.VarChar).Value = string.IsNullOrEmpty(TextBox_MZ_AD.Text.Trim()) ? " " : TextBox_MZ_AD.Text.Trim();
                cmd.Parameters.Add("MZ_UNIT", SqlDbType.VarChar).Value = string.IsNullOrEmpty(TextBox_MZ_UNIT.Text.Trim()) ? " " : TextBox_MZ_UNIT.Text.Trim();
                cmd.Parameters.Add("MZ_OCCC", SqlDbType.VarChar).Value = string.IsNullOrEmpty(TextBox_MZ_OCCC.Text.Trim()) ? " " : TextBox_MZ_OCCC.Text.Trim();
                cmd.Parameters.Add("MZ_TBDV", SqlDbType.VarChar).Value = string.IsNullOrEmpty(TextBox_MZ_TBDV.Text.Trim()) ? " " : TextBox_MZ_TBDV.Text.Trim();
                cmd.Parameters.Add("MZ_PRRST", SqlDbType.VarChar).Value = string.IsNullOrEmpty(TextBox_MZ_PRRST.Text.Trim()) ? " " : TextBox_MZ_PRRST.Text.Trim();

                try
                {
                    cmd.ExecuteNonQuery();

                    Response.Cookies["PKEY_MZ_AD"].Expires = DateTime.Now.AddYears(-1);
                    Response.Cookies["PKEY_MZ_UNIT"].Expires = DateTime.Now.AddYears(-1);
                    Response.Cookies["PKEY_MZ_OCCC"].Expires = DateTime.Now.AddYears(-1);
                    Response.Cookies["PKEY_MZ_PRRST"].Expires = DateTime.Now.AddYears(-1);
                    Response.Cookies["PKEY_MZ_TBDV"].Expires = DateTime.Now.AddYears(-1);

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
                    TextBox_MZ_AD.Text = string.Empty;
                    TextBox_MZ_AD1.Text = string.Empty;
                    TextBox_MZ_OCCC.Text = string.Empty;
                    TextBox_MZ_OCCC1.Text = string.Empty;
                    TextBox_MZ_PRRST.Text = string.Empty;
                    TextBox_MZ_PRRST1.Text = string.Empty;
                    TextBox_MZ_TBDV.Text = string.Empty;
                    TextBox_MZ_TBDV1.Text = string.Empty;
                    TextBox_MZ_UNIT.Text = string.Empty;
                    TextBox_MZ_UNIT1.Text = string.Empty;
                    GridView1.DataBind();
                    GridView1.SelectedIndex = -1;
                    btUpdate.Enabled = false;
                    btInsert.Enabled = true;
                    btCancel.Enabled = false;
                    btDelete.Enabled = false;
                    btOK.Enabled = false;
                    ViewState.Remove("Mode");
                    //by MQ 20100311---------
                    A.controlEnable(ref this.Panel_SWT3, false);
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
            //by MQ 20100311---------
            A.controlEnable(ref this.Panel_SWT3, false);

            GridView1.SelectedIndex = -1;
            btInsert.Enabled = true;
            btCancel.Enabled = false;
            btUpdate.Enabled = false;
            btDelete.Enabled = false;
            btOK.Enabled = false;
            TextBox_MZ_AD.Text = string.Empty;
            TextBox_MZ_AD1.Text = string.Empty;
            TextBox_MZ_OCCC.Text = string.Empty;
            TextBox_MZ_OCCC1.Text = string.Empty;
            TextBox_MZ_PRRST.Text = string.Empty;
            TextBox_MZ_PRRST1.Text = string.Empty;
            TextBox_MZ_TBDV.Text = string.Empty;
            TextBox_MZ_TBDV1.Text = string.Empty;
            TextBox_MZ_UNIT.Text = string.Empty;
            TextBox_MZ_UNIT1.Text = string.Empty;
           
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
                string OCCC = string.IsNullOrEmpty(TextBox_MZ_OCCC.Text.Trim()) ? " " : TextBox_MZ_OCCC.Text.Trim();
                string TBDV = string.IsNullOrEmpty(TextBox_MZ_TBDV.Text.Trim()) ? " " : TextBox_MZ_TBDV.Text.Trim();
                string PRRST = string.IsNullOrEmpty(TextBox_MZ_PRRST.Text.Trim()) ? " " : TextBox_MZ_PRRST.Text.Trim();
                string UNIT = string.IsNullOrEmpty(TextBox_MZ_UNIT.Text.Trim()) ? " " : TextBox_MZ_UNIT.Text.Trim();
                string AD = string.IsNullOrEmpty(TextBox_MZ_AD.Text.Trim()) ? " " : TextBox_MZ_AD.Text.Trim();


                string DeleteString = "DELETE FROM A_SWT3 WHERE     MZ_OCCC='" + o_str.tosql(OCCC) +
                                                             "' AND MZ_TBDV='" + o_str.tosql(TBDV) +
                                                            "' AND MZ_PRRST='" + o_str.tosql(PRRST) +
                                                             "' AND MZ_UNIT='" + o_str.tosql(UNIT) +
                                                               "' AND MZ_AD='" + o_str.tosql(AD) + "'";

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
                    btOK.Enabled = false;
                    TextBox_MZ_AD.Text = string.Empty;
                    TextBox_MZ_AD1.Text = string.Empty;
                    TextBox_MZ_OCCC.Text = string.Empty;
                    TextBox_MZ_OCCC1.Text = string.Empty;
                    TextBox_MZ_PRRST.Text = string.Empty;
                    TextBox_MZ_PRRST1.Text = string.Empty;
                    TextBox_MZ_TBDV.Text = string.Empty;
                    TextBox_MZ_TBDV1.Text = string.Empty;
                    TextBox_MZ_UNIT.Text = string.Empty;
                    TextBox_MZ_UNIT1.Text = string.Empty;
                    
                }
                catch
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除失敗');", true);
                }
            }
        }

       

        private void Ktype_Cname_Check(string Cname, TextBox tb1, TextBox tb2, object obj)
        {
            tb2.Text = tb2.Text.ToUpper();
            //檢查代碼中文名稱是否存在
            if (string.IsNullOrEmpty(Cname))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('查無資料')", true);
                tb1.Text = string.Empty;
                tb2.Text = string.Empty;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + tb2.ClientID + "').focus();$get('" + tb2.ClientID + "').focus();", true);
            }
            else
            {
                tb1.Text = Cname;
                if (obj is TextBox)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + (obj as TextBox).ClientID + "').focus();$get('" + (obj as TextBox).ClientID + "').focus();", true);
                }
                else if (obj is RadioButtonList)
                {
                    (obj as RadioButtonList).Focus();
                }
            }
        }

        protected void Ktype_Search(TextBox tb1, TextBox tb2, string ktype)
        {
            //回傳值
            Session["KTYPE_CID"] = tb1.ClientID;
            Session["KTYPE_CID1"] = tb2.ClientID;
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_Ktype_Search.aspx?MZ_KTYPE=" + ktype + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin','top=190,left=200,width=420,height=450,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }

        protected void TextBox_MZ_AD_TextChanged(object sender, EventArgs e)
        {
            string CName = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)='38213' UNION ALL SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)<>'38213') WHERE MZ_KCODE='" + o_str.tosql(TextBox_MZ_AD.Text.Trim().ToUpper()) + "'");
            Ktype_Cname_Check(CName, TextBox_MZ_AD1, TextBox_MZ_AD, TextBox_MZ_UNIT);
        }

        protected void TextBox_MZ_UNIT_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBox_MZ_AD.Text.Trim()))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "alert('請先輸入機關!')", true);
                return;
            }
            string CName = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='25' AND MZ_KCODE IN (SELECT MZ_UNIT FROM A_UNIT_AD WHERE MZ_AD='" + TextBox_MZ_AD.Text.Trim().ToUpper() + "') AND MZ_KCODE='" + TextBox_MZ_UNIT.Text.Trim().ToUpper() + "'");
            Ktype_Cname_Check(CName, TextBox_MZ_UNIT1, TextBox_MZ_UNIT, TextBox_MZ_OCCC);
        }

        protected void TextBox_MZ_OCCC_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_OCCC.Text, "26");

            Ktype_Cname_Check(CName, TextBox_MZ_OCCC1, TextBox_MZ_OCCC, TextBox_MZ_TBDV);
        }

        protected void TextBox_MZ_TBDV_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_TBDV.Text, "43");

            Ktype_Cname_Check(CName, TextBox_MZ_TBDV1, TextBox_MZ_TBDV, TextBox_MZ_PRRST);
        }

        protected void TextBox_MZ_PRRST_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_PRRST.Text, "24");

            Ktype_Cname_Check(CName, TextBox_MZ_PRRST1, TextBox_MZ_PRRST, TextBox_MZ_PRRST);
        }

        protected void btUNIT_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(TextBox_MZ_AD.Text))
            {
                Session["KTYPE_CID"] = TextBox_MZ_UNIT.ClientID;
                Session["KTYPE_CID1"] = TextBox_MZ_UNIT1.ClientID;
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('../1-personnel/Personal_Ktype_Search.aspx?CID_NAME=UNIT&AD=" + TextBox_MZ_AD.Text.Trim() + "TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin','top=190,left=200,width=420,height=450,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "alert('請先輸入機關!')", true);
            }
        }

        protected void btOCCC_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_MZ_OCCC, TextBox_MZ_OCCC1, "26");
        }

        protected void btTBDV_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_MZ_TBDV, TextBox_MZ_TBDV1, "43");
        }

        protected void btAD_Click(object sender, EventArgs e)
        {
            Session["KTYPE_CID"] = TextBox_MZ_AD.ClientID;
            Session["KTYPE_CID1"] = TextBox_MZ_AD1.ClientID;

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_Ktype_Search.aspx?CID_NAME=AD&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin','top=190,left=200,width=420,height=450,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }

        protected void btPRRST_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_MZ_PRRST, TextBox_MZ_PRRST1, "24");
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GridView1, "Select$" + e.Row.RowIndex);
                e.Row.Cells[5].Attributes.Add("Style", "display:none");
            }
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                GridView1.SelectedIndex = Convert.ToInt32(e.CommandArgument);

                btUpdate.Enabled = true;
                btDelete.Enabled = true;
                btCancel.Enabled = true;
                btOK.Enabled = false;
                btInsert.Enabled = false;

                TextBox_MZ_AD.Text = GridView1.SelectedRow.Cells[0].Text.Trim();
                TextBox_MZ_UNIT.Text = GridView1.SelectedRow.Cells[1].Text.Trim();
                TextBox_MZ_OCCC.Text = GridView1.SelectedRow.Cells[2].Text.Trim();
                TextBox_MZ_TBDV.Text = GridView1.SelectedRow.Cells[3].Text.Trim();
                TextBox_MZ_PRRST.Text = GridView1.SelectedRow.Cells[4].Text.Trim();

                TextBox_MZ_AD1.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)='38213' UNION ALL SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)<>'38213') WHERE MZ_KCODE='" + o_str.tosql(TextBox_MZ_AD.Text.Trim()) + "'");
                TextBox_MZ_OCCC1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_OCCC.Text, "26");
                TextBox_MZ_UNIT1.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='25' AND MZ_KCODE='" + o_str.tosql(TextBox_MZ_UNIT.Text.Trim()) + "'");
                TextBox_MZ_TBDV1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_TBDV.Text, "43");
                TextBox_MZ_PRRST1.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='24' AND MZ_KCODE='" + o_str.tosql(TextBox_MZ_PRRST.Text.Trim().ToUpper()) + "' ");


            }
        }

        protected void btOK_Click(object sender, EventArgs e)
        {
            execSQL();
        }
    }
}
