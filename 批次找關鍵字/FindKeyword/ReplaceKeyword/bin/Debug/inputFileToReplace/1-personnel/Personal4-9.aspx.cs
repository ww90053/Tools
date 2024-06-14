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
    public partial class Personal4_9 : System.Web.UI.Page
    {
        //List<String> UNIT_AD_UNIT;
        //List<String> UNIT_AD_AD;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                A.check_power();
            }

            ViewState["MZ_UNIT"] = Request["MZ_UNIT"];
            ViewState["MZ_AD"] = Request["MZ_AD"];
            //ViewState["XCOUNT"] = Request["XCOUNT"];

         
          

            if (!IsPostBack)
            { 
                //by MQ 20100311---------
            A.set_Panel_EnterToTAB(ref this.Panel_UNIT_AD);
                //by MQ 20100311---------
                A.controlEnable(ref this.Panel_UNIT_AD, false);

                
                string strSQL = "";

                if (ViewState["MZ_UNIT"] != null || ViewState["MZ_AD"] != null)
                {
                    strSQL = "SELECT MZ_AD+(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=MZ_AD AND MZ_KTYPE='04') AS MZ_AD,MZ_UNIT+(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=MZ_UNIT AND MZ_KTYPE='25') AS MZ_UNIT,MZ_NO1 FROM A_UNIT_AD WHERE 1=1";

                    if (ViewState["MZ_UNIT"].ToString() != "")
                    {
                        strSQL += " AND MZ_UNIT='" + ViewState["MZ_UNIT"].ToString().Trim() + "'";
                    }

                    if (ViewState["MZ_AD"].ToString() != "")
                    {
                        strSQL += " AND MZ_AD='" + ViewState["MZ_AD"].ToString().Trim() + "'";
                    }
                    strSQL += " ORDER BY MZ_AD";
                }

                SqlDataSource1.SelectCommand = strSQL;
                ViewState["SQL"] = strSQL;
                GridView1.DataBind();

                btUpdate.Enabled = false;
                btInsert.Enabled = true;
                btOK.Enabled = false;
                btCancel.Enabled = false;
                btDelete.Enabled = false;


                
            }
        }

        

        protected void btInsert_Click(object sender, EventArgs e)
        {
            ViewState["CMDSQL"] = "INSERT INTO A_UNIT_AD(MZ_UNIT,MZ_AD,MZ_NO1) VALUES(@MZ_UNIT,@MZ_AD,@MZ_NO1)";

            ViewState["Mode"] = "INSERT";

            TextBox_MZ_AD.Text = string.Empty;
            TextBox_MZ_AD1.Text = string.Empty;
            TextBox_MZ_UNIT.Text = string.Empty;
            TextBox_MZ_UNIT1.Text = string.Empty;
            TextBox_MZ_NO1.Text = string.Empty;
            btDelete.Enabled = false;
            btInsert.Enabled = false;
            btUpdate.Enabled = false;
            btCancel.Enabled = true;
            btOK.Enabled = true;

            //by MQ 20100311---------
            A.controlEnable(ref this.Panel_UNIT_AD, true);
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_UNIT.ClientID + "').focus();$get('" + TextBox_MZ_UNIT.ClientID + "').focus();", true);
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
                ViewState["CMDSQL"] = "UPDATE A_UNIT_AD SET MZ_UNIT=@MZ_UNIT,MZ_AD=@MZ_AD,MZ_NO1=@MZ_NO1 WHERE MZ_UNIT='" + o_str.tosql(TextBox_MZ_UNIT.Text.Trim()) + "' AND MZ_AD='" + o_str.tosql(TextBox_MZ_AD.Text.Trim()) + "'";

                ViewState["Mode"] = "UPDATE";

                HttpCookie Cookie1 = new HttpCookie("PKEY_MZ_AD");
                Cookie1.Value = TextBox_MZ_AD.Text;
                Response.Cookies.Add(Cookie1);

                HttpCookie Cookie2 = new HttpCookie("PKEY_MZ_UNIT");
                Cookie2.Value = TextBox_MZ_UNIT.Text;
                Response.Cookies.Add(Cookie2);

                btDelete.Enabled = false;
                btUpdate.Enabled = false;
                btOK.Enabled = true;

                //by MQ 20100311---------
                A.controlEnable(ref this.Panel_UNIT_AD, true);
            }
        }

        protected void btOK_Click(object sender, EventArgs e)
        {
            //2010.06.07 by 伊珊
            string ErrorString = "";

            string old_AD = "NULL";

            string old_UNIT = "NULL";

            if (ViewState["Mode"].ToString() == "UPDATE")
            {
                HttpCookie Cookie1 = new HttpCookie("PKEY_MZ_AD");
                Cookie1 = Request.Cookies["PKEY_MZ_AD"];
                old_AD = Cookie1.Value.ToString();

                HttpCookie Cookie2 = new HttpCookie("PKEY_MZ_UNIT");
                Cookie2 = Request.Cookies["PKEY_MZ_UNIT"];
                old_UNIT = Cookie1.Value.ToString();
            }

            string pkey_check;

            if (old_AD == TextBox_MZ_AD.Text && old_UNIT == TextBox_MZ_UNIT.Text && ViewState["Mode"].ToString() == "UPDATE")
                pkey_check = "0";
            else
                pkey_check = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_UNIT_AD WHERE MZ_AD='" + TextBox_MZ_AD.Text.Trim() + "' AND MZ_UNIT='" + TextBox_MZ_UNIT.Text.Trim() + "'");

            if (pkey_check != "0")
            {
                ErrorString += "單位與機關違反唯一值的條件" + "\\r\\n";
                TextBox_MZ_AD.BackColor = Color.Orange;
                TextBox_MZ_UNIT.BackColor = Color.Orange;
            }
            else
            {
                TextBox_MZ_AD.BackColor = Color.White;
                TextBox_MZ_UNIT.BackColor = Color.White;
            }
            //20141216 這應該是想做控制項檢核.但 DBDIC 又沒有資料
            //foreach (Object ob in Panel_UNIT_AD.Controls)
            //{
            //    if (ob is TextBox)
            //    {
            //        TextBox tbox = (TextBox)ob;

            //        string result = o_Check_Dbdic.ErrorString(tbox.ID, "A_UNIT_AD", tbox.Text);

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

                cmd.Parameters.Add("MZ_UNIT", SqlDbType.VarChar).Value = TextBox_MZ_UNIT.Text.Trim();
                cmd.Parameters.Add("MZ_AD", SqlDbType.VarChar).Value = TextBox_MZ_AD.Text.Trim();
                cmd.Parameters.Add("MZ_NO1", SqlDbType.VarChar).Value = TextBox_MZ_NO1.Text.Trim();

                try
                {
                    cmd.ExecuteNonQuery();

                    Response.Cookies["PKEY_MZ_AD"].Expires = DateTime.Now.AddYears(-1);
                    Response.Cookies["PKEY_MZ_UNIT"].Expires = DateTime.Now.AddYears(-1);

                    if (ViewState["Mode"].ToString() == "INSERT")
                    {


                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('新增成功');location.href('Personal4-9.aspx?MZ_AD=" + TextBox_MZ_AD.Text.Trim() + "&MZ_UNIT=" + TextBox_MZ_UNIT.Text.Trim() + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "');", true);
                        //2010.06.04 LOG紀錄 by伊珊
                        TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00002", TPMPermissions._boolTPM(), o_DBFactory.ABC_toTest.RegexSQL(cmd));
                    }
                    else if (ViewState["Mode"].ToString() == "UPDATE")
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('修改成功');", true);
                        //2010.06.04 LOG紀錄 by伊珊
                        TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00003", TPMPermissions._boolTPM(), o_DBFactory.ABC_toTest.RegexSQL(cmd));
                        //UNIT_AD_AD = Session["UNIT_AD_AD"] as List<string>;

                        

                        SqlDataSource1.SelectCommand = ViewState["SQL"].ToString();
                        GridView1.DataBind();
                    }
                    btUpdate.Enabled = false;
                    btInsert.Enabled = true;
                    btCancel.Enabled = false;
                    btOK.Enabled = false;
                    btDelete.Enabled = false;

                    GridView1.SelectedIndex = -1;
                    TextBox_MZ_AD.Text = string.Empty;
                    TextBox_MZ_AD1.Text = string.Empty;
                    TextBox_MZ_UNIT.Text = string.Empty;
                    TextBox_MZ_UNIT1.Text = string.Empty;
                    TextBox_MZ_NO1.Text = string.Empty;
                    ViewState.Remove("Mode");
                    //by MQ 20100311---------
                    A.controlEnable(ref this.Panel_UNIT_AD, false);
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

        protected void btCancel_Click(object sender, EventArgs e)
        {
            
            GridView1.SelectedIndex = -1;
            TextBox_MZ_AD.Text = string.Empty;
            TextBox_MZ_AD1.Text = string.Empty;
            TextBox_MZ_UNIT.Text = string.Empty;
            TextBox_MZ_UNIT1.Text = string.Empty;
            TextBox_MZ_NO1.Text = string.Empty;
            btInsert.Enabled = true;
            btCancel.Enabled = false;
            btOK.Enabled = false;
            btDelete.Enabled = false;
            btUpdate.Enabled = false;

            //by MQ 20100311---------
            A.controlEnable(ref this.Panel_UNIT_AD, false);
        }

        protected void btDelete_Click(object sender, EventArgs e)
        {
            string sqlString = "DELETE FROM A_UNIT_AD WHERE MZ_UNIT='" + o_str.tosql(TextBox_MZ_UNIT.Text.Trim()) + "' AND MZ_AD='" + o_str.tosql(TextBox_MZ_AD.Text.Trim()) + "'  AND MZ_NO1='" + o_str.tosql(TextBox_MZ_NO1.Text.Trim()) + "'";
            try
            {
                o_DBFactory.ABC_toTest.Edit_Data(sqlString);
                
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除成功')", true);
                //2010.06.04 LOG紀錄 by伊珊
                TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00004", TPMPermissions._boolTPM(), sqlString);
                // }
                TextBox_MZ_AD.Text = string.Empty;
                TextBox_MZ_AD1.Text = string.Empty;
                TextBox_MZ_UNIT.Text = string.Empty;
                TextBox_MZ_UNIT1.Text = string.Empty;
                TextBox_MZ_NO1.Text = string.Empty;
                btInsert.Enabled = true;
                btCancel.Enabled = false;
                btOK.Enabled = false;
                btDelete.Enabled = false;
                btUpdate.Enabled = false;

                SqlDataSource1.SelectCommand = ViewState["SQL"].ToString();
                GridView1.DataBind();

                GridView1.SelectedIndex = -1;
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除失敗');", true);
            }
        }

        protected void btSearch_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal4-9Search.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "','查詢','top=190,left=200,width=500,height=150,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
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
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + (obj as TextBox).ClientID + "').focus();get('" + (obj as TextBox).ClientID + "').focus();", true);
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

            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_UNIT.Text.Trim(), "25");
            Ktype_Cname_Check(CName, TextBox_MZ_UNIT1, TextBox_MZ_UNIT, TextBox_MZ_NO1);
        }

        protected void btUNIT_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_MZ_UNIT, TextBox_MZ_UNIT1, "25");
        }

        protected void btAD_Click(object sender, EventArgs e)
        {
            Session["KTYPE_CID"] = TextBox_MZ_AD.ClientID;
            Session["KTYPE_CID1"] = TextBox_MZ_AD1.ClientID;

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_Ktype_Search.aspx?CID_NAME=AD&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin','top=190,left=200,width=420,height=450,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                GridView1.SelectedIndex = Convert.ToInt32(e.CommandArgument);
                TextBox_MZ_UNIT.Text = GridView1.SelectedRow.Cells[0].Text.Trim().Substring(0, 4);
                TextBox_MZ_AD.Text = GridView1.SelectedRow.Cells[1].Text.Trim().Substring(0, 10);
                TextBox_MZ_NO1.Text = GridView1.SelectedRow.Cells[2].Text.Trim();

                TextBox_MZ_UNIT1.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='25' AND MZ_KCODE='" + o_str.tosql(TextBox_MZ_UNIT.Text.Trim()) + "'");
                TextBox_MZ_AD1.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)='38213' UNION ALL SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)<>'38213') WHERE MZ_KCODE='" + o_str.tosql(TextBox_MZ_AD.Text.Trim()) + "'");

                btUpdate.Enabled = true;
                btDelete.Enabled = true;
                btCancel.Enabled = true;
                btOK.Enabled = false;
                btInsert.Enabled = false;
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GridView1, "Select$" + e.Row.RowIndex);
                e.Row.Cells[3].Attributes.Add("Style", "display:none");
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            SqlDataSource1.SelectCommand = ViewState["SQL"].ToString();

            GridView1.DataBind();
        }
    }
}
