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
    public partial class WebForm4 : System.Web.UI.Page
    {
        DataTable PAGE_DT = new DataTable();

        List<String> KTYPE_KTYPE = new List<string>();
        List<String> KTYPE_KCODE = new List<string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                A.check_power();
            }

            ViewState["MZ_KTYPE"] = Request["MZ_KTYPE"];
            ViewState["MZ_KCODE"] = Request["MZ_KCODE"];
            if (Request["MZ_KCHI"] != null)
                ViewState["MZ_KCHI"] = Server.UrlDecode(Request["MZ_KCHI"].ToString());
            ViewState["XCOUNT"] = Request["XCOUNT"];

           
            if (!IsPostBack)
            {
                //by MQ 20100311---------
                A.set_Panel_EnterToTAB(ref this.Panel_KTYPE);
                //by MQ 20100311---------
                A.controlEnable(ref this.Panel_KTYPE, false);

                if (ViewState["XCOUNT"] != null)
                {
                    finddata(int.Parse(ViewState["XCOUNT"].ToString()));
                    xcount.Text = ViewState["XCOUNT"].ToString();
                    if (int.Parse(ViewState["XCOUNT"].ToString()) == 0 && KTYPE_KTYPE.Count > 1)
                    {
                        btNEXT.Enabled = true;
                        btUpper.Enabled = false;
                    }
                    else if (int.Parse(ViewState["XCOUNT"].ToString()) > 0 && int.Parse(ViewState["XCOUNT"].ToString()) < KTYPE_KTYPE.Count - 1)
                    {
                        btNEXT.Enabled = true;
                        btUpper.Enabled = true;
                    }
                    else if (int.Parse(ViewState["XCOUNT"].ToString()) == KTYPE_KTYPE.Count - 1)
                    {
                        btUpper.Enabled = true;
                        btNEXT.Enabled = false;
                    }
                    else
                    {
                        btNEXT.Enabled = false;
                        btUpper.Enabled = false;
                    }
                    btUpdate.Enabled = true;
                    btInsert.Enabled = true;
                    btOK.Enabled = false;
                    btCancel.Enabled = false;
                    btDelete.Enabled = false;

                }

                if (ViewState["MZ_KTYPE"] != null)
                {
                    string strSQL = "SELECT * FROM A_KTYPE WHERE 1=1";
                    if (ViewState["MZ_KTYPE"].ToString() != "")
                    {
                        strSQL += " AND MZ_KTYPE LIKE '%" + ViewState["MZ_KTYPE"].ToString().Trim() + "%' ";
                    }
                    if (ViewState["MZ_KCODE"].ToString() != "")
                    {
                        strSQL += " AND MZ_KCODE LIKE '%" + ViewState["MZ_KCODE"].ToString().Trim() + "%' ";
                    }
                    if (ViewState["MZ_KCHI"].ToString() != "")
                    {
                        strSQL += " AND MZ_KCHI LIKE '%" + ViewState["MZ_KCHI"].ToString().Trim() + "%' ";
                    }
                    strSQL += " ORDER BY MZ_KTYPE,MZ_KCODE";

                    KTYPE_KTYPE = o_DBFactory.ABC_toTest.DataListArray(strSQL, "MZ_KTYPE");
                    KTYPE_KCODE = o_DBFactory.ABC_toTest.DataListArray(strSQL, "MZ_KCODE");

                    Session["KTYPE_KTYPE"] = KTYPE_KTYPE;
                    Session["KTYPE_KCODE"] = KTYPE_KCODE;

                    if (KTYPE_KTYPE.Count == 0)
                    {
                        Response.Write(@"<script language=javascript>window.alert('查無資料！');location.href('Personal4-2.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "');</script>");

                    }
                    else if (KTYPE_KTYPE.Count == 1)
                    {
                        xcount.Text = "0";
                        finddata(int.Parse(xcount.Text));
                    }
                    else
                    {
                        xcount.Text = "0";
                        finddata(int.Parse(xcount.Text));

                        btNEXT.Enabled = true;
                    }

                    PAGE_DT = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GETPAGE");

                    ViewState["strSQL"] = strSQL;

                    ViewState["PAGE_DT"] = PAGE_DT;

                    GridView1.DataSource = PAGE_DT;
                    GridView1.AllowPaging = true;
                    GridView1.PageSize = 10;
                    GridView1.DataBind();
                }

                //預設操作模式為空值
                hfd_Mode.Value = "";
            }
        }

        protected void finddata(int Datacount)
        {
            KTYPE_KTYPE = Session["KTYPE_KTYPE"] as List<String>;
            KTYPE_KCODE = Session["KTYPE_KCODE"] as List<String>;

            string strSQL = "SELECT * FROM A_KTYPE WHERE MZ_KTYPE='" + KTYPE_KTYPE[Datacount].Trim() + "' AND MZ_KCODE='" + KTYPE_KCODE[Datacount].Trim() + "'";

            DataTable dt = new DataTable();
            dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");

            if (dt.Rows.Count == 1)
            {
                TextBox_MZ_KCHI.Text = dt.Rows[0]["MZ_KCHI"].ToString().Trim();
                TextBox_MZ_KCODE.Text = dt.Rows[0]["MZ_KCODE"].ToString().Trim();
                TextBox_MZ_KFIL.Text = dt.Rows[0]["MZ_KFIL"].ToString().Trim();
                TextBox_MZ_KTYPE.Text = dt.Rows[0]["MZ_KTYPE"].ToString().Trim();
            }

            btUpdate.Enabled = true;
            btDelete.Enabled = true;

            //by MQ 20100311
            moveGV_IndexMark();
            this.Label1.Text = "第" + (int.Parse(xcount.Text) + 1).ToString() + "筆共" + KTYPE_KTYPE.Count.ToString() + "筆";
            //
        }

        protected void btInsert_Click(object sender, EventArgs e)
        {
            ViewState["CMDSQL"] = "INSERT INTO A_KTYPE(MZ_KTYPE,MZ_KCODE,MZ_KCHI,MZ_KFIL)" +
                                                      " VALUES(@MZ_KTYPE,@MZ_KCODE,@MZ_KCHI,@MZ_KFIL) ";
            ViewState["Mode"] = "INSERT";
            hfd_Mode.Value = "INSERT";

            btInsert.Enabled = false;
            btUpdate.Enabled = false;
            btCancel.Enabled = true;
            btOK.Enabled = true;

            TextBox_MZ_KCHI.Text = string.Empty;
            TextBox_MZ_KCODE.Text = string.Empty;
            TextBox_MZ_KFIL.Text = string.Empty;
            TextBox_MZ_KTYPE.Text = string.Empty;

            //by MQ 20100311---------
            A.controlEnable(ref this.Panel_KTYPE, true);
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_KTYPE.ClientID + "').focus();$get('" + TextBox_MZ_KTYPE.ClientID + "').focus();", true);
        }

        protected void btUpdate_Click(object sender, EventArgs e)
        {
            ViewState["CMDSQL"] = "UPDATE A_KTYPE SET MZ_KTYPE=@MZ_KTYPE,MZ_KCODE=@MZ_KCODE,MZ_KCHI=@MZ_KCHI,MZ_KFIL=@MZ_KFIL" +
                                                            " WHERE MZ_KTYPE='" + TextBox_MZ_KTYPE.Text + "' AND MZ_KCODE='" + TextBox_MZ_KCODE.Text + "'";
            ViewState["Mode"] = "UPDATE";
            hfd_Mode.Value = "UPDATE";

            HttpCookie Cookie1 = new HttpCookie("PKEY_MZ_KTYPE");
            Cookie1.Value = TextBox_MZ_KTYPE.Text;
            Response.Cookies.Add(Cookie1);

            HttpCookie Cookie2 = new HttpCookie("PKEY_MZ_KCODE");
            Cookie2.Value = TextBox_MZ_KCODE.Text;
            Response.Cookies.Add(Cookie2);

            btDelete.Enabled = true;
            btInsert.Enabled = false;
            btUpdate.Enabled = false;
            btCancel.Enabled = true;
            btOK.Enabled = true;

            //by MQ 20100311---------
            A.controlEnable(ref this.Panel_KTYPE, true);
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_KTYPE.ClientID + "').focus();$get('" + TextBox_MZ_KTYPE.ClientID + "').focus();", true);
            
        }

        protected void btOK_Click(object sender, EventArgs e)
        {
            //2010.06.07 by 伊珊
            string ErrorString = "";

            string old_KTYPE = "NULL";

            string old_KCODE = "NULL";

            if (ViewState["Mode"].ToString() == "UPDATE")
            {
                HttpCookie Cookie1 = new HttpCookie("PKEY_MZ_KTYPE");
                Cookie1 = Request.Cookies["PKEY_MZ_KTYPE"];
                old_KTYPE = Cookie1.Value.ToString();

                HttpCookie Cookie2 = new HttpCookie("PKEY_MZ_KCODE");
                Cookie2 = Request.Cookies["PKEY_MZ_KCODE"];
                old_KCODE = Cookie2.Value.ToString();
            }

            string pkey_check;

            if (old_KTYPE == TextBox_MZ_KTYPE.Text && old_KCODE == TextBox_MZ_KCODE.Text && ViewState["Mode"].ToString() == "UPDATE")
                pkey_check = "0";
            else
                pkey_check = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_KTYPE WHERE MZ_KTYPE='" + TextBox_MZ_KTYPE.Text.Trim() + "'AND MZ_KCODE='" + TextBox_MZ_KCODE.Text.Trim() + "'");

            if (pkey_check != "0")
            {
                ErrorString += "序號與代碼違反唯一值的條件" + "\\r\\n";
                TextBox_MZ_KTYPE.BackColor = Color.Orange;
                TextBox_MZ_KCODE.BackColor = Color.Orange;
            }
            else
            {
                TextBox_MZ_KTYPE.BackColor = Color.White;
                TextBox_MZ_KCODE.BackColor = Color.White;
            }
            //20141216 這應該是想做控制項檢核.但 DBDIC 又沒有資料
            //foreach (Object ob in Panel_KTYPE.Controls)
            //{
            //    if (ob is TextBox)
            //    {
            //        TextBox tbox = (TextBox)ob;

            //        string result = o_Check_Dbdic.ErrorString(tbox.ID, "A_KTYPE", tbox.Text);

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

                cmd.Parameters.Add("MZ_KTYPE", SqlDbType.VarChar).Value = TextBox_MZ_KTYPE.Text.Trim();
                cmd.Parameters.Add("MZ_KCODE", SqlDbType.VarChar).Value = TextBox_MZ_KCODE.Text.Trim();
                cmd.Parameters.Add("MZ_KCHI", SqlDbType.VarChar).Value = TextBox_MZ_KCHI.Text.Trim();
                cmd.Parameters.Add("MZ_KFIL", SqlDbType.VarChar).Value = TextBox_MZ_KFIL.Text.Trim();

                try
                {
                    cmd.ExecuteNonQuery();

                    Response.Cookies["PKEY_MZ_KCODE"].Expires = DateTime.Now.AddYears(-1);
                    Response.Cookies["PKEY_MZ_KTYPE"].Expires = DateTime.Now.AddYears(-1);

                    if (ViewState["Mode"].ToString() == "INSERT")
                    {


                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('新增成功');location.href('Personal4-2.aspx?MZ_KTYPE=" + TextBox_MZ_KTYPE.Text.Trim() + "&MZ_KCODE=" + TextBox_MZ_KCODE.Text.Trim() + "&MZ_KCHI=&TPM_FION=" + Request.QueryString["TPM_FION"] + "');", true);
                        //2010.06.04 LOG紀錄 by伊珊
                        TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00002", TPMPermissions._boolTPM(), o_DBFactory.ABC_toTest.RegexSQL(cmd));
                    }
                    else if (ViewState["Mode"].ToString() == "UPDATE")
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('修改成功');", true);
                        //2010.06.04 LOG紀錄 by伊珊
                        TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00003", TPMPermissions._boolTPM(), o_DBFactory.ABC_toTest.RegexSQL(cmd));

                        KTYPE_KTYPE = Session["KTYPE_KTYPE"] as List<string>;

                        if (int.Parse(xcount.Text.Trim()) == 0 && KTYPE_KTYPE.Count == 1)
                        {
                            btUpper.Enabled = false;
                            btNEXT.Enabled = false;
                        }
                        else if (int.Parse(xcount.Text.Trim()) == 0 && KTYPE_KTYPE.Count > 1)
                        {
                            btUpper.Enabled = false;
                            btNEXT.Enabled = true;
                        }
                        else if (int.Parse(xcount.Text.Trim()) + 1 == KTYPE_KTYPE.Count)
                        {
                            btUpper.Enabled = true;
                            btNEXT.Enabled = false;
                        }
                        else if (int.Parse(xcount.Text.Trim()) > 0 && int.Parse(xcount.Text.Trim()) + 1 < KTYPE_KTYPE.Count)
                        {
                            btNEXT.Enabled = true;
                            btUpper.Enabled = true;
                        }
                    }

                    //by MQ 20100311
                    PAGE_DT = o_DBFactory.ABC_toTest.Create_Table(ViewState["strSQL"].ToString(), "GETPAGE");
                    ViewState["PAGE_DT"] = PAGE_DT;
                    GridView1.DataSource = PAGE_DT;
                    GridView1.DataBind();
                    //GridView1.DataBind();
                    //GridView1.SelectedIndex = -1;

                    //TextBox_MZ_KCHI.Text = string.Empty;
                    //TextBox_MZ_KCODE.Text = string.Empty;
                    //TextBox_MZ_KFIL.Text = string.Empty;
                    //TextBox_MZ_KTYPE.Text = string.Empty;

                    btInsert.Enabled = true;
                    btCancel.Enabled = false;
                    btOK.Enabled = false;
                    btDelete.Enabled = false;
                    ViewState.Remove("Mode");
                    //by MQ 20100311---------
                    A.controlEnable(ref this.Panel_KTYPE, false);
                }
                catch
                {
                    if (ViewState["Mode"].ToString() == "INSERT")
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('新增失敗');", true);
                    }
                    else if (ViewState["Mode"].ToString() == "UPDATE")
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('修改失敗');location.href('Personal4-2.aspx?XCOUNT=" + xcount.Text.Trim() + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "');", true);
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
            A.controlEnable(ref this.Panel_KTYPE, false);
            //

            if (ViewState["Mode"].ToString() == "INSERT")
            {
                foreach (object dl in Panel_KTYPE.Controls)
                {
                    if (dl is DropDownList)
                    {
                        DropDownList dl1 = dl as DropDownList;
                        dl1.SelectedValue = "";
                    }

                    if (dl is ComboBox)
                    {
                        ComboBox cm1 = dl as ComboBox;
                        cm1.SelectedValue = "";

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
                finddata(int.Parse(xcount.Text.Trim()));

                KTYPE_KTYPE = Session["KTYPE_KTYPE"] as List<string>;
                if (int.Parse(xcount.Text.Trim()) == 0 && KTYPE_KTYPE.Count == 1)
                {
                    btUpper.Enabled = false;
                    btNEXT.Enabled = false;
                }
                else if (int.Parse(xcount.Text.Trim()) == 0 && KTYPE_KTYPE.Count > 1)
                {
                    btUpper.Enabled = false;
                    btNEXT.Enabled = true;
                }
                else if (int.Parse(xcount.Text.Trim()) + 1 == KTYPE_KTYPE.Count)
                {
                    btUpper.Enabled = true;
                    btNEXT.Enabled = false;
                }
                else if (int.Parse(xcount.Text.Trim()) > 0 && int.Parse(xcount.Text.Trim()) + 1 < KTYPE_KTYPE.Count)
                {
                    btNEXT.Enabled = true;
                    btUpper.Enabled = true;
                }
            }

            btInsert.Enabled = true;
            btCancel.Enabled = false;
            btOK.Enabled = false;
            btDelete.Enabled = false;
        }

        protected void btDelete_Click(object sender, EventArgs e)
        {
            string sqlString = "DELETE FROM A_KTYPE WHERE MZ_KTYPE='" + TextBox_MZ_KTYPE.Text + "' AND MZ_KCODE='" + TextBox_MZ_KCODE.Text + "'";
            try
            {
                o_DBFactory.ABC_toTest.Edit_Data(sqlString);
                KTYPE_KTYPE = Session["KTYPE_KTYPE"] as List<String>;
                KTYPE_KCODE = Session["KTYPE_KCODE"] as List<String>;
                KTYPE_KTYPE.RemoveAt(int.Parse(xcount.Text));
                KTYPE_KCODE.RemoveAt(int.Parse(xcount.Text));

                //by MQ 20100311
                PAGE_DT = o_DBFactory.ABC_toTest.Create_Table(ViewState["strSQL"].ToString(), "GETPAGE");
                ViewState["PAGE_DT"] = PAGE_DT;
                GridView1.DataSource = PAGE_DT;
                GridView1.DataBind();//

                if (KTYPE_KTYPE.Count == 0)
                {
                    btUpdate.Enabled = false;
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除成功');location.href('Personal4-2.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "');", true);
                    //2010.06.04 LOG紀錄 by伊珊
                    TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00004", TPMPermissions._boolTPM(), sqlString);
                }
                else
                {
                    xcount.Text = "0";
                    finddata(int.Parse(xcount.Text));

                    if (KTYPE_KTYPE.Count > 1)
                    {
                        btNEXT.Enabled = true;
                    }
                    btUpdate.Enabled = true;

                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除成功')", true);
                }

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

        protected void btSearch_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal4-2Search.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "','查詢','top=190,left=200,width=500,height=150,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }

        protected void btUpper_Click(object sender, EventArgs e)
        {
            if (int.Parse(xcount.Text) != 0)
            {
                xcount.Text = (Convert.ToInt32(xcount.Text) - 1).ToString();
                finddata(int.Parse(xcount.Text));
                if (int.Parse(xcount.Text) != KTYPE_KTYPE.Count - 1)
                {
                    btNEXT.Enabled = true;
                }
                if (int.Parse(xcount.Text) == 0)
                {
                    btUpper.Enabled = false;
                }
            }
            else if (int.Parse(xcount.Text) == 0)
            {
                finddata(int.Parse(xcount.Text));
                btUpper.Enabled = false;
            }
        }

        protected void btNEXT_Click(object sender, EventArgs e)
        {
            if (int.Parse(xcount.Text) == 0)
            {
                xcount.Text = (Convert.ToInt32(xcount.Text) + 1).ToString();

                finddata(int.Parse(xcount.Text));

                if (int.Parse(xcount.Text) == KTYPE_KTYPE.Count - 1)
                {
                    btNEXT.Enabled = false;
                }

                btUpper.Enabled = true;
            }
            else
            {
                xcount.Text = (Convert.ToInt32(xcount.Text) + 1).ToString();
                finddata(int.Parse(xcount.Text));

                if (int.Parse(xcount.Text) == KTYPE_KTYPE.Count - 1)
                {
                    btNEXT.Enabled = false;
                }
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            PAGE_DT = ViewState["PAGE_DT"] as DataTable;
            GridView1.DataSource = PAGE_DT;
            GridView1.PageIndex = e.NewPageIndex;
            GridView1.DataBind();

            GridView1.SelectedIndex = -1;
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GridView1, "Select$" + e.Row.RowIndex);
                e.Row.Cells[4].Attributes.Add("Style", "display:none");
            }
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                GridView1.SelectedIndex = Convert.ToInt32(e.CommandArgument);

                //by MQ 20100311---------
                int a = (GridView1.PageIndex) * (GridView1.PageSize) + Convert.ToInt32(e.CommandArgument);
                xcount.Text = a.ToString();
                finddata(int.Parse(xcount.Text));

                if (int.Parse(xcount.Text) == 0)
                {
                    btUpper.Enabled = false;
                }
                else
                {
                    btUpper.Enabled = true;
                }
                if (int.Parse(xcount.Text) == KTYPE_KTYPE.Count - 1)
                {
                    btNEXT.Enabled = false;
                }
                else
                {
                    btNEXT.Enabled = true;
                }
                //TextBox_MZ_KCHI.Text = GridView1.SelectedRow.Cells[2].Text;
                //TextBox_MZ_KCODE.Text = GridView1.SelectedRow.Cells[1].Text;
                //TextBox_MZ_KFIL.Text = GridView1.SelectedRow.Cells[4].Text;
                //TextBox_MZ_KTYPE.Text = GridView1.SelectedRow.Cells[0].Text;
                //

                btUpdate.Enabled = true;
                btDelete.Enabled = true;
                btCancel.Enabled = true;
            }
        }

        protected void TextBox_MZ_KCODE_TextChanged(object sender, EventArgs e)
        {
            TextBox_MZ_KCODE.Text = TextBox_MZ_KCODE.Text.ToUpper().Trim();
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_KCODE.ClientID + "').focus();$get('" + TextBox_MZ_KCODE.ClientID + "').focus();", true);
        }

        protected void moveGV_IndexMark()
        {
            //by MQ 20100311

            int n = int.Parse(xcount.Text);

            PAGE_DT = ViewState["PAGE_DT"] as DataTable;
            GridView1.DataSource = PAGE_DT;
            GridView1.PageIndex = n / GridView1.PageSize;

            if ((n + 1) <= GridView1.PageSize)
            {
                GridView1.SelectedIndex = n;
            }
            else
            {
                GridView1.SelectedIndex = n % GridView1.PageSize;
            }

            GridView1.DataBind();
            //
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_Ktype_detail.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "','查詢','top=190,left=200,width=650,height=500,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }
    }
}
