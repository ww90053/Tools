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
    public partial class Personal3_2 : System.Web.UI.Page
    {
        List<string> POSIT1_MZ_PRID1 = new List<string>();
        List<String> POSIT1_MZ_PRID = new List<string>();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                A.check_power();
            }

         
            ViewState["POSIT1_PRID"] = Session["POSIT1_PRID"];
            Session.Remove("POSIT1_PRID");

            //HttpCookie POSIT1_PRID_Cookie = new HttpCookie("POSIT1_PRID");

            //POSIT1_PRID_Cookie = Request.Cookies["POSIT1_PRID"];

            /////發文文號用cookie作,用session 會造成下方程式無限迴圈
            //if (POSIT1_PRID_Cookie == null)
            //{
            //    ViewState["POSIT1_PRID"] = null;
            //    Response.Cookies["POSIT1_PRID"].Expires = DateTime.Now.AddYears(-1);
            //}
            //else
            //{
            //    ViewState["POSIT1_PRID"] = TPMPermissions._strDecod(POSIT1_PRID_Cookie.Value.ToString());
            //    Response.Cookies["POSIT1_PRID"].Expires = DateTime.Now.AddYears(-1);
            //}

            ViewState["POSIT1_PRID1"] = Request["POSIT1_PRID1"];
            ViewState["POSIT1_DATE"] = Request["DATE"];
            ViewState["XCOUNT"] = Request["XCOUNT"];

            A.set_Panel_EnterToTAB(ref this.Panel_POSIT1);
            //A.set_Panel_EnterToTAB(ref this.Panel1);
            A.set_Panel_EnterToTAB(ref this.Panel2);
            A.set_Panel_EnterToTAB(ref this.Panel3);

            GridView1.Visible = true;

            GridView2.Visible = false;

            if (!IsPostBack)
            {
                TextBox_MZ_PRID.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_PRID FROM A_CHKAD WHERE MZ_AD='" + Session["ADPMZ_EXAD"].ToString() + "'");
                TextBox_MZ_EXPLAIN0.Text = "受文機關及各受令人";
                TextBox_MZ_EXPLAIN1.Text = "本局人事室";

                if (ViewState["XCOUNT"] != null)
                {
                    finddata(int.Parse(ViewState["XCOUNT"].ToString()));
                    xcount.Text = ViewState["XCOUNT"].ToString();
                    if (int.Parse(ViewState["XCOUNT"].ToString()) == 0 && POSIT1_MZ_PRID1.Count > 1)
                    {
                        btNEXT.Enabled = true;
                        btUpper.Enabled = false;
                    }
                    else if (int.Parse(ViewState["XCOUNT"].ToString()) > 0 && int.Parse(ViewState["XCOUNT"].ToString()) < POSIT1_MZ_PRID1.Count - 1)
                    {
                        btNEXT.Enabled = true;
                        btUpper.Enabled = true;
                    }
                    else if (int.Parse(ViewState["XCOUNT"].ToString()) == POSIT1_MZ_PRID1.Count - 1)
                    {
                        btUpper.Enabled = true;
                        btNEXT.Enabled = false;
                    }
                    else
                    {
                        btNEXT.Enabled = false;
                        btUpper.Enabled = false;
                    }

                    btUpper.Enabled = true;
                    btOK.Enabled = false;
                    btInsert.Enabled = true;
                    btDelete.Enabled = false;
                    btUpdate.Enabled = true;
                }
                if (ViewState["POSIT1_PRID"] != null)
                {
                    string strSQL = "SELECT MZ_PRID,MZ_PRID1 FROM A_POSIT1 WHERE 1=1";

                    if (ViewState["POSIT1_PRID"].ToString() != "")
                    {
                        strSQL += " AND MZ_PRID='" + ViewState["POSIT1_PRID"].ToString() + "'";
                    }
                    if (ViewState["POSIT1_PRID1"].ToString() != "")
                    {
                        strSQL += " AND MZ_PRID1='" + ViewState["POSIT1_PRID1"].ToString() + "'";
                    }

                    strSQL += " ORDER BY MZ_PRID,MZ_PRID1";

                    POSIT1_MZ_PRID = o_DBFactory.ABC_toTest.DataListArray(strSQL, "MZ_PRID");
                    POSIT1_MZ_PRID1 = o_DBFactory.ABC_toTest.DataListArray(strSQL, "MZ_PRID1");

                    Session["POSIT1_MZ_PRID1"] = POSIT1_MZ_PRID1;
                    Session["POSIT1_MZ_PRID"] = POSIT1_MZ_PRID;

                    if (POSIT1_MZ_PRID1.Count == 0)
                    {
                        Response.Write(@"<script language=javascript>window.alert('查無資料！');location.href('Personal3-2.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "');</script>");
                    }
                    else if (POSIT1_MZ_PRID1.Count == 1)
                    {

                        xcount.Text = "0";
                        finddata(int.Parse(xcount.Text));
                    }
                    else
                    {
                        btNEXT.Enabled = true;
                        xcount.Text = "0";
                        finddata(int.Parse(xcount.Text));
                    }
                }
                A.controlEnable(ref this.Panel_POSIT1, false);
            }

            if (ViewState["Mode"] != null)
            {
                if (ViewState["Mode"].ToString() == "INSERT" && int.Parse(o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_POSIT2 WHERE MZ_PRID='" + TextBox_MZ_PRID.Text + "' AND MZ_PRID1='" + TextBox_MZ_PRID1.Text + "'")) > 0)
                {
                    ViewState.Remove("GV2_DB");

                    GridView2.Visible = false;

                    GridView1.Visible = true;
                }
            }

            if (ViewState["GV2_DB"] != null)
            {
                GridView2.DataSource = ViewState["GV2_DB"] as DataTable;

                GridView2.AllowPaging = true;

                GridView2.PageSize = 6;

                GridView2.DataBind();

                TextBox_GrivewCount.Text = (ViewState["GV2_DB"] as DataTable).Rows.Count.ToString();

                GridView2.Visible = true;

                GridView1.Visible = false;
            }
            else
            {

                Session.Remove("PRK1_TEMP_DT2");

                GridView2.Visible = false;

                GridView1.Visible = true;
            }
        }
       

        protected void COUNT_GV_ROWS(string PRID, string PRID1)
        {
            TextBox_GrivewCount.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_POSIT2 WHERE MZ_PRID='" + PRID + "' AND MZ_PRID1='" + PRID1 + "'");
        }

        protected void UpdateDLBASE(string strSQL)
        {
            DataTable dt = new DataTable();
            dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                conn.Open();
                SqlTransaction connTran = conn.BeginTransaction();
                try
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string UpdateSQL = "UPDATE A_DLBASE SET  WHERE MZ_ID='" + dt.Rows[i]["MZ_ID"].ToString().Trim() + "'";
                        SqlCommand cmd = new SqlCommand(UpdateSQL, conn);
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add("MZ_AD", SqlDbType.VarChar).Value = dt.Rows[i]["MZ_AD"].ToString().Trim();

                        cmd.ExecuteNonQuery();

                    }
                    connTran.Commit();
                }
                catch
                {
                    connTran.Rollback();
                }
                finally
                {
                    conn.Close();
                    //XX2013/06/18 
                    conn.Dispose();
                }
            }
        }

        protected void finddata(int Datacount)
        {
            POSIT1_MZ_PRID1 = Session["POSIT1_MZ_PRID1"] as List<string>;
            POSIT1_MZ_PRID = Session["POSIT1_MZ_PRID"] as List<string>;

            string Findsql = "SELECT * FROM A_POSIT1 WHERE MZ_PRID='" + POSIT1_MZ_PRID[Datacount] + "' AND MZ_PRID1='" + POSIT1_MZ_PRID1[Datacount].ToString() + "'";

            DataTable Finddt = o_DBFactory.ABC_toTest.Create_Table(Findsql, "123");

            if (Finddt.Rows.Count == 1)
            {
                TextBox_MZ_PRID.Text = Finddt.Rows[0]["MZ_PRID"].ToString();
                TextBox_MZ_EXPLAIN.Text = Finddt.Rows[0]["MZ_EXPLAIN"].ToString();
                TextBox_MZ_EXPLAIN0.Text = Finddt.Rows[0]["MZ_EXPLAIN0"].ToString();
                TextBox_MZ_EXPLAIN1.Text = Finddt.Rows[0]["MZ_EXPLAIN1"].ToString();
                TextBox_MZ_DATE.Text = o_CommonService.Personal_ReturnDateString(Finddt.Rows[0]["MZ_DATE"].ToString());
                TextBox_MZ_IDATE.Text = o_CommonService.Personal_ReturnDateString(Finddt.Rows[0]["MZ_IDATE"].ToString());

                TextBox_MZ_PRID1.Text = Finddt.Rows[0]["MZ_PRID1"].ToString();
                TextBox_MZ_ADATE.Text = o_CommonService.Personal_ReturnDateString(Finddt.Rows[0]["MZ_ADATE"].ToString());

                COUNT_GV_ROWS(TextBox_MZ_PRID.Text, TextBox_MZ_PRID1.Text);
            }
            btUpdate.Enabled = true;
            btDelete.Enabled = true;
        }

        protected void btInsert_Click(object sender, EventArgs e)
        {
            foreach (object dl in Panel_POSIT1.Controls)
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

            ViewState["Mode"] = "INSERT";

            ViewState["CMDSQL"] = "INSERT INTO A_POSIT1(MZ_PRID,MZ_PRID1,MZ_CHKAD,MZ_DATE,MZ_IDATE,MZ_ADATE," +
                                             " MZ_EXPLAIN0,MZ_EXPLAIN1,MZ_EXPLAIN,MZ_BP)" +
                                      " VALUES(@MZ_PRID,@MZ_PRID1,@MZ_CHKAD,@MZ_DATE,@MZ_IDATE,@MZ_ADATE," +
                                             " @MZ_EXPLAIN0,@MZ_EXPLAIN1,@MZ_EXPLAIN,@MZ_BP)";

            ViewState.Remove("GV2_DB");

            Session.Remove("POSIT1_TEMP_DT2");

            GridView2.DataSource = null;
            GridView2.DataBind();

            GridView1.DataSource = null;
            GridView1.DataBind();


            btCancel.Enabled = true;
            btNEXT.Enabled = false;
            btUpper.Enabled = false;
            btOK.Enabled = true;
            btInsert.Enabled = false;
            btDelete.Enabled = false;
            btUpdate.Enabled = false;
            Button4.Enabled = true;
            Button5.Enabled = true;

            A.controlEnable(ref this.Panel_POSIT1, true);
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_PRID1.ClientID + "').focus();$get('" + TextBox_MZ_PRID1.ClientID + "').focus();", true);
        }

        protected void btSearch_Click(object sender, EventArgs e)
        {
            Session["POSIT1_PRID"] = TextBox_MZ_PRID.Text;
            Session["POSIT1_PRID1"] = TextBox_MZ_PRID1.Text;
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('PersonalSerch2.aspx?TableName=POSIT1&TPM_FION=" + Request.QueryString["TPM_FION"] + "','查詢','top=190,left=200,width=380,height=200,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }

        protected void btUpdate_Click(object sender, EventArgs e)
        {
            ViewState["Mode"] = "UPDATE";

            ViewState["CMDSQL"] = "UPDATE A_POSIT1 SET MZ_PRID=@MZ_PRID,MZ_PRID1=@MZ_PRID1,MZ_CHKAD=@MZ_CHKAD,MZ_DATE=@MZ_DATE," +
                                                     " MZ_IDATE=@MZ_IDATE,MZ_ADATE=@MZ_ADATE,MZ_EXPLAIN0=@MZ_EXPLAIN0,MZ_EXPLAIN1=@MZ_EXPLAIN1," +
                                                     " MZ_EXPLAIN=@MZ_EXPLAIN,MZ_BP=@MZ_BP WHERE MZ_PRID='" + TextBox_MZ_PRID.Text +
                                                "' AND MZ_PRID1='" + TextBox_MZ_PRID1.Text.Trim() + "'";

            Session["PKEY_MZ_PRID"] = TextBox_MZ_PRID.Text;

            Session["PKEY_MZ_PRID1"] = TextBox_MZ_PRID1.Text;

            btCancel.Enabled = true;
            btNEXT.Enabled = false;
            btUpper.Enabled = false;
            btOK.Enabled = true;
            btInsert.Enabled = false;
            btDelete.Enabled = true;
            btUpdate.Enabled = false;
            Button4.Enabled = true;
            Button5.Enabled = true;
            A.controlEnable(ref this.Panel_POSIT1, true);
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_PRID1.ClientID + "').focus();$get('" + TextBox_MZ_PRID1.ClientID + "').focus();", true);
        }

        protected void btDelete_Click(object sender, EventArgs e)
        {
            string DeleteString = "DELETE FROM A_POSIT1 WHERE MZ_PRID='" + o_str.tosql(TextBox_MZ_PRID.Text) + "' AND MZ_PRID1='" + o_str.tosql(TextBox_MZ_PRID1.Text.Trim()) + "'";

            string DelectPOSIT2String = "DELETE FROM A_POSIT2 WHERE MZ_PRID='" + TextBox_MZ_PRID.Text + "' AND MZ_PRID1='" + o_str.tosql(TextBox_MZ_PRID1.Text.Trim()) + "'"; ;

            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                conn.Open();
                SqlTransaction oraTran = conn.BeginTransaction();

                SqlCommand cmd1 = new SqlCommand(DeleteString, conn);
                cmd1.Transaction = oraTran;

                SqlCommand cmd2 = new SqlCommand(DelectPOSIT2String, conn);
                cmd2.Transaction = oraTran;

                try
                {
                    cmd1.ExecuteNonQuery();

                    cmd2.ExecuteNonQuery();

                    string strSQL = "SELECT DISTINCT MZ_NO FROM A_POSIT2 WHERE MZ_PRID='" + TextBox_MZ_PRID.Text + "' AND MZ_PRID1='" + o_str.tosql(TextBox_MZ_PRID1.Text.Trim()) + "'";

                    DataTable tempDT = new DataTable();

                    tempDT = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");

                    for (int i = 0; i < tempDT.Rows.Count; i++)
                    {
                        string UpdatePRKBString = "UPDATE A_POSIT SET MZ_SWT1='N' WHERE MZ_NO='" + tempDT.Rows[i]["MZ_NO"].ToString() + "'";

                        SqlCommand cmd3 = new SqlCommand(UpdatePRKBString, conn);

                        cmd3.Transaction = oraTran;

                        cmd3.ExecuteNonQuery();
                    }

                    POSIT1_MZ_PRID = Session["PRK1_MZ_PRID"] as List<string>;
                    POSIT1_MZ_PRID1 = Session["POSIT1_MZ_PRID1"] as List<string>;
                    POSIT1_MZ_PRID.RemoveAt(int.Parse(xcount.Text.Trim()));
                    POSIT1_MZ_PRID1.RemoveAt(int.Parse(xcount.Text.Trim()));

                    if (POSIT1_MZ_PRID.Count == 0)
                    {
                        btUpdate.Enabled = false;
                        btDelete.Enabled = false;
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除成功');location.href('Personal2-1.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "');", true);

                        //2010.06.04 LOG紀錄 by伊珊
                        TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00004", TPMPermissions._boolTPM(), o_DBFactory.ABC_toTest.RegexSQL(cmd1));
                        TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00004", TPMPermissions._boolTPM(), o_DBFactory.ABC_toTest.RegexSQL(cmd2));
                    }
                    else
                    {
                        xcount.Text = "0";
                        finddata(int.Parse(xcount.Text));
                        if (POSIT1_MZ_PRID1.Count > 1)
                        {
                            btNEXT.Enabled = true;
                        }
                        btUpdate.Enabled = true;
                        btDelete.Enabled = true;
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除成功')", true);
                        //2010.06.04 LOG紀錄 by伊珊
                        TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00004", TPMPermissions._boolTPM(), o_DBFactory.ABC_toTest.RegexSQL(cmd1));
                        TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00004", TPMPermissions._boolTPM(), o_DBFactory.ABC_toTest.RegexSQL(cmd2));
                    }

                    btInsert.Enabled = true;
                    btCancel.Enabled = false;
                    btOK.Enabled = false;

                }
                catch
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除失敗');", true);
                }
            }
        }

        protected void btok_Click(object sender, EventArgs e)
        {
            //2010.06.07 by 伊珊
            string ErrorString = "";

            string old_PRID = "NULL";

            string old_PRID1 = "NULL";

            if (ViewState["Mode"].ToString() == "UPDATE")
            {
                old_PRID = Session["PKEY_MZ_PRID"].ToString();
          
                old_PRID1 = Session["PKEY_MZ_PRID1"].ToString();
            }

            string pkey_check;

            if (old_PRID == TextBox_MZ_PRID.Text && old_PRID1 == TextBox_MZ_PRID1.Text && ViewState["Mode"].ToString() == "UPDATE")
                pkey_check = "0";
            else
                pkey_check = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_POSIT1 WHERE MZ_PRID='" + TextBox_MZ_PRID.Text.Trim() + "' AND MZ_PRID1='" + TextBox_MZ_PRID1.Text.Trim() + "'");

            if (pkey_check != "0")
            {
                ErrorString += "發文文號與發文字號違反唯一值的條件" + "\\r\\n";
                TextBox_MZ_PRID.BackColor = Color.Orange;
                TextBox_MZ_PRID1.BackColor = Color.Orange;
            }
            else
            {
                TextBox_MZ_PRID.BackColor = Color.White;
                TextBox_MZ_PRID1.BackColor = Color.White;
            }

            bool check_MZ_DATE = DateManange.check_Date(TextBox_MZ_DATE.Text.Replace("/", "").PadLeft(7, '0'));

            bool check_MZ_IDATE = DateManange.check_Date(TextBox_MZ_IDATE.Text.Trim().Replace("/", "").PadLeft(7, '0'));

            if (!check_MZ_DATE)
            {
                ErrorString += "發文日期日期有誤" + "\\r\\n";
                TextBox_MZ_DATE.BackColor = Color.Orange;
            }
            else
            {
                TextBox_MZ_DATE.BackColor = Color.White;
            }

            if (!check_MZ_IDATE)
            {
                ErrorString += "生效日期日期有誤" + "\\r\\n";
                TextBox_MZ_IDATE.BackColor = Color.Orange;
            }
            else
            {
                TextBox_MZ_IDATE.BackColor = Color.White;
            }

            //20141216 這應該是想做控制項檢核.但 DBDIC 又沒有資料
            //foreach (Object ob in Panel_POSIT1.Controls)
            //{
            //    if (ob is TextBox)
            //    {
            //        TextBox tbox = (TextBox)ob;

            //        string result = o_Check_Dbdic.ErrorString(tbox.ID, "A_POSIT1", tbox.Text);

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

            //        string result = o_Check_Dbdic.ErrorString(dlist.ID, "A_POSIT1", dlist.Text);

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

            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(ViewState["CMDSQL"].ToString(), conn);

                cmd.CommandType = CommandType.Text;

                SqlTransaction oraTran = conn.BeginTransaction();

                cmd.Transaction = oraTran;

                SqlCommand cmd1;

                SqlCommand cmd2;

                cmd.Parameters.Add("MZ_PRID", SqlDbType.VarChar).Value = TextBox_MZ_PRID.Text.Trim();
                cmd.Parameters.Add("MZ_PRID1", SqlDbType.VarChar).Value = TextBox_MZ_PRID1.Text.Trim().ToUpper();
                cmd.Parameters.Add("MZ_CHKAD", SqlDbType.VarChar).Value = Session["ADPMZ_EXAD"].ToString();
                cmd.Parameters.Add("MZ_DATE", SqlDbType.VarChar).Value = string.IsNullOrEmpty(TextBox_MZ_DATE.Text.Trim()) ? Convert.DBNull : TextBox_MZ_DATE.Text.Trim().Replace("/", "").PadLeft(7, '0');
                cmd.Parameters.Add("MZ_IDATE", SqlDbType.VarChar).Value = string.IsNullOrEmpty(TextBox_MZ_IDATE.Text.Trim()) ? Convert.DBNull : TextBox_MZ_IDATE.Text.Trim().Replace("/", "").PadLeft(7, '0');
                cmd.Parameters.Add("MZ_ADATE", SqlDbType.VarChar).Value = string.IsNullOrEmpty(TextBox_MZ_ADATE.Text.Trim()) ? Convert.DBNull : TextBox_MZ_ADATE.Text.Trim().Replace("/", "").PadLeft(7, '0');
                cmd.Parameters.Add("MZ_EXPLAIN0", SqlDbType.VarChar).Value = TextBox_MZ_EXPLAIN0.Text.Trim().ToUpper();
                cmd.Parameters.Add("MZ_EXPLAIN1", SqlDbType.VarChar).Value = TextBox_MZ_EXPLAIN1.Text.Trim().ToUpper();
                cmd.Parameters.Add("MZ_EXPLAIN", SqlDbType.NVarChar).Value = TextBox_MZ_EXPLAIN.Text.Trim().ToUpper();
                cmd.Parameters.Add("MZ_BP", SqlDbType.VarChar).Value = Session["ADPMZ_EXAD"].ToString() == "382130000C" ? "Y" : "N";

                try
                {
                    DataTable tempDT = new DataTable();
                    tempDT = Session["POSIT1_tempDT"] as DataTable;

                    if (GridView1.Rows.Count == 0 && tempDT.Rows.Count == 0)
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請先謄稿');", true);
                        return;
                    }
                    else
                    {
                        cmd.ExecuteNonQuery();
                    }

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


                        POSIT1_MZ_PRID1 = Session["POSIT1_MZ_PRID1"] as List<string>;

                        if (int.Parse(xcount.Text.Trim()) == 0 && POSIT1_MZ_PRID1.Count == 1)
                        {
                            btUpper.Enabled = false;
                            btNEXT.Enabled = false;
                        }
                        else if (int.Parse(xcount.Text.Trim()) == 0 && POSIT1_MZ_PRID1.Count > 1)
                        {
                            btUpper.Enabled = false;
                            btNEXT.Enabled = true;
                        }
                        else if (int.Parse(xcount.Text.Trim()) + 1 == POSIT1_MZ_PRID1.Count)
                        {
                            btUpper.Enabled = true;
                            btNEXT.Enabled = false;
                        }
                        else if (int.Parse(xcount.Text.Trim()) > 0 && int.Parse(xcount.Text.Trim()) + 1 < POSIT1_MZ_PRID1.Count)
                        {
                            btNEXT.Enabled = true;
                            btUpper.Enabled = true;
                        }
                    }

                    if (tempDT != null)
                    {
                        if (tempDT.Rows.Count > 0)
                        {
                            if (Session["ISBP"].ToString() == "NO")
                            {
                                for (int i = 0; i < tempDT.Rows.Count; i++)
                                {
                                    string strPartSQL1 = "";

                                    //權限
                                    switch (o_a_Function.strGID(Session["ADPMZ_ID"].ToString()))
                                    {
                                        case "A":
                                        case "B":
                                        case "D":
                                        case "E":
                                            break;
                                        
                                        case "C":
                                            strPartSQL1 = " AND MZ_CHKAD='" + Session["ADPMZ_EXAD"].ToString() + "'";
                                            break;
                                       
                                    }

                                    string sysday = (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0') + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0');

                                    string strPartSQL = " MZ_NO='" + tempDT.Rows[i]["MZ_NO"].ToString() + "' AND MZ_ID='" + tempDT.Rows[i]["MZ_ID"].ToString() + "'";

                                    string strSQL;

                                   
                                    strSQL = "INSERT INTO A_POSIT2(MZ_NO,MZ_ID,MZ_NAME,MZ_CHKAD,MZ_EXOAD,MZ_EXOUNIT,MZ_EXOOCC," +
                                                                           "  MZ_EXORANK,MZ_EXRANK1,MZ_EXOPOS,MZ_EXOPCHIEF,MZ_TBNREA," +
                                                                           "  MZ_TBDATE,MZ_TBID,MZ_EXOPNO1,MZ_EXOPNO,MZ_AD,MZ_UNIT,MZ_OCCC,MZ_RANK,MZ_RANK1," +
                                                                           "  MZ_CHISI,MZ_POSIND,MZ_NIN,MZ_NREA,MZ_TBDV,MZ_PCHIEF,MZ_PESN," +
                                                                           "  MZ_PNO,MZ_TNO,MZ_WDATE,MZ_SRANK,MZ_SLVC,MZ_SPT,MZ_SPT1,MZ_MEMO," +
                                                                           "  MZ_RET,MZ_REMARK,MZ_NRT,OTH_THING,CONDITION," +
                                                                           "  MZ_PNO1,MUSER,MDATE,MZ_EXTPOS,MZ_EXTPOS2,MZ_ISEXTPOS,MZ_ISEXTPOS2," +
                                                                           "  MZ_PRID,MZ_PRID1,MZ_DATE,MZ_IDATE,MZ_ADATE) ";
                                    

                                    string MZ_DATE = string.IsNullOrEmpty(TextBox_MZ_DATE.Text.Trim().Replace("/", string.Empty)) ? string.Empty : TextBox_MZ_DATE.Text.Trim().Replace("/", string.Empty).PadLeft(7, '0');

                                    string MZ_IDATE = string.IsNullOrEmpty(TextBox_MZ_IDATE.Text.Trim().Replace("/", string.Empty)) ? string.Empty : TextBox_MZ_IDATE.Text.Trim().Replace("/", string.Empty).PadLeft(7, '0');

                                   
                                    strSQL = strSQL + "  SELECT MZ_NO,MZ_ID,MZ_NAME,MZ_CHKAD,MZ_EXOAD,MZ_EXOUNIT,MZ_EXOOCC," +
                                                                            "  MZ_EXORANK,MZ_EXRANK1,MZ_EXOPOS,MZ_EXOPCHIEF,MZ_TBNREA," +
                                                                            "  MZ_TBDATE,MZ_TBID,MZ_EXOPNO1,MZ_EXOPNO,MZ_AD,MZ_UNIT,MZ_OCCC,MZ_RANK,MZ_RANK1," +
                                                                            "  MZ_CHISI,MZ_POSIND,MZ_NIN,MZ_NREA,MZ_TBDV,MZ_PCHIEF,MZ_PESN," +
                                                                            "  MZ_PNO,MZ_TNO,MZ_WDATE,MZ_SRANK,MZ_SLVC,MZ_SPT,MZ_SPT1,MZ_MEMO," +
                                                                            "  MZ_RET,MZ_REMARK,MZ_NRT,OTH_THING,CONDITION," +
                                                                            "  MZ_PNO1,'" + Session["ADPMZ_ID"].ToString() + "','" + sysday + "',MZ_EXTPOS,MZ_EXTPOS2,MZ_ISEXTPOS,MZ_ISEXTPOS2, '"
                                                                             + TextBox_MZ_PRID.Text + "','" + TextBox_MZ_PRID1.Text.Trim() + "','" + MZ_DATE + "','"
                                                                             + MZ_IDATE + "','" + TextBox_MZ_ADATE.Text.Trim().Replace("/", string.Empty).PadLeft(7, '0') + "'  FROM A_POSIT " +
                                                                            " WHERE " + strPartSQL + strPartSQL1;
                                    


                                    string UpdateSWT1sql = "UPDATE A_POSIT SET MZ_SWT1='Y' WHERE " + strPartSQL;

                                    cmd1 = new SqlCommand(strSQL, conn);

                                    cmd1.Transaction = oraTran;

                                    cmd2 = new SqlCommand(UpdateSWT1sql, conn);

                                    cmd2.Transaction = oraTran;

                                    cmd1.ExecuteNonQuery();

                                    cmd2.ExecuteNonQuery();

                                    //2010.06.04 LOG紀錄 by伊珊
                                    TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00003", TPMPermissions._boolTPM(), o_DBFactory.ABC_toTest.RegexSQL(cmd2));
                                }
                            }
                            else if (Session["ISBP"].ToString() == "YES")
                            {
                                string sysday = (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0') + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0');

                                string MZ_DATE = string.IsNullOrEmpty(TextBox_MZ_DATE.Text.Trim().Replace("/", string.Empty)) ? string.Empty : TextBox_MZ_DATE.Text.Trim().Replace("/", string.Empty).PadLeft(7, '0');

                                string MZ_IDATE = string.IsNullOrEmpty(TextBox_MZ_IDATE.Text.Trim().Replace("/", string.Empty)) ? string.Empty : TextBox_MZ_IDATE.Text.Trim().Replace("/", string.Empty).PadLeft(7, '0');

                                string strSQL;

                                strSQL = "INSERT INTO A_POSIT2(MZ_NO,MZ_ID,MZ_NAME,MZ_CHKAD,MZ_EXOAD,MZ_EXOUNIT,MZ_EXOOCC," +
                                                                          "  MZ_EXORANK,MZ_EXRANK1,MZ_EXOPOS,MZ_EXOPCHIEF,MZ_TBNREA," +
                                                                          "  MZ_TBDATE,MZ_TBID,MZ_EXOPNO1,MZ_EXOPNO,MZ_AD,MZ_UNIT,MZ_OCCC,MZ_RANK,MZ_RANK1," +
                                                                          "  MZ_CHISI,MZ_POSIND,MZ_NIN,MZ_NREA,MZ_TBDV,MZ_PCHIEF,MZ_PESN," +
                                                                          "  MZ_PNO,MZ_TNO,MZ_WDATE,MZ_SRANK,MZ_SLVC,MZ_SPT,MZ_SPT1,MZ_MEMO," +
                                                                          "  MZ_RET,MZ_REMARK,MZ_NRT,OTH_THING,CONDITION," +
                                                                          "  MZ_PNO1,MUSER,MDATE,MZ_EXTPOS,MZ_EXTPOS2,MZ_ISEXTPOS,MZ_ISEXTPOS2," +
                                                                          "  MZ_PRID,MZ_PRID1,MZ_DATE,MZ_IDATE,MZ_ADATE) ";

                                strSQL += "  SELECT MZ_NO,MZ_ID,MZ_NAME,MZ_CHKAD,MZ_EXOAD,MZ_EXOUNIT,MZ_EXOOCC," +
                                                                          "  MZ_EXORANK,MZ_EXRANK1,MZ_EXOPOS,MZ_EXOPCHIEF,MZ_TBNREA," +
                                                                          "  MZ_TBDATE,MZ_TBID,MZ_EXOPNO1,MZ_EXOPNO,MZ_AD,MZ_UNIT,MZ_OCCC,MZ_RANK,MZ_RANK1," +
                                                                          "  MZ_CHISI,MZ_POSIND,MZ_NIN,MZ_NREA,MZ_TBDV,MZ_PCHIEF,MZ_PESN," +
                                                                          "  MZ_PNO,MZ_TNO,MZ_WDATE,MZ_SRANK,MZ_SLVC,MZ_SPT,MZ_SPT1,MZ_MEMO," +
                                                                          "  MZ_RET,MZ_REMARK,MZ_NRT,OTH_THING,CONDITION," +
                                                                          "  MZ_PNO1,'" + Session["ADPMZ_ID"].ToString() + "','" + sysday + "',MZ_EXTPOS,MZ_EXTPOS2,MZ_ISEXTPOS,MZ_ISEXTPOS2, '"
                                                                           + TextBox_MZ_PRID.Text + "','" + TextBox_MZ_PRID1.Text.Trim() + "','" + MZ_DATE + "','"
                                                                           + MZ_IDATE + "','" + TextBox_MZ_ADATE.Text.Trim().Replace("/", string.Empty).PadLeft(7, '0') + "'  FROM A_POSIT2 " +
                                                                          " WHERE " + Session["BPSQL"].ToString();


                                cmd1 = new SqlCommand(strSQL, conn);

                                cmd1.Transaction = oraTran;

                                cmd1.ExecuteNonQuery();

                                //2010.06.04 LOG紀錄 by伊珊
                                TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00003", TPMPermissions._boolTPM(), o_DBFactory.ABC_toTest.RegexSQL(cmd1));
                            }
                        }
                    }
                    oraTran.Commit();

                    GridView1.DataBind();

                    GridView1.Visible = true;

                    GridView2.Visible = false;

                    Session.Remove("PKEY_MZ_PRID");

                    Session.Remove("PKEY_MZ_PRID1");

                    btCancel.Enabled = false;
                    btOK.Enabled = false;
                    btInsert.Enabled = true;
                    btDelete.Enabled = true;
                    btUpdate.Enabled = true;
                    Button4.Enabled = false;
                    Button5.Enabled = false;
                    ViewState.Remove("Mode");
                    A.controlEnable(ref this.Panel_POSIT1, false);

                }
                catch
                {
                    oraTran.Rollback();
                    if (ViewState["Mode"].ToString() == "INSERT")
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('新增失敗');", true);
                    }
                    else if (ViewState["Mode"].ToString() == "UPDATE")
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('編輯失敗');location.href('Personal3-2.aspx?XCOUNT=" + xcount.Text.Trim() + "TPM_FION=" + Request.QueryString["TPM_FION"] + "');", true);
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

        protected void btUpper_Click(object sender, EventArgs e)
        {
            if (int.Parse(xcount.Text) != 0)
            {
                xcount.Text = (Convert.ToInt32(xcount.Text) - 1).ToString();
                finddata(int.Parse(xcount.Text));
                if (int.Parse(xcount.Text) != POSIT1_MZ_PRID1.Count - 1)
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
                btUpper.Enabled = true;
                finddata(int.Parse(xcount.Text));

                if (int.Parse(xcount.Text) == POSIT1_MZ_PRID1.Count - 1)
                {

                    btNEXT.Enabled = false;
                }
            }
            else
            {
                xcount.Text = (Convert.ToInt32(xcount.Text) + 1).ToString();
                finddata(int.Parse(xcount.Text));

                if (int.Parse(xcount.Text) == POSIT1_MZ_PRID1.Count - 1)
                {

                    btNEXT.Enabled = false;
                }
            }
        }

        protected void GridView1_DataBound(object sender, EventArgs e)
        {
            if (GridView1.Rows.Count > 0)
            {
                TextBox_GrivewCount.Text = GridView1.Rows.Count.ToString();
            }
        }

        protected void Button5_Click(object sender, EventArgs e)
        {
            Session["POSIT1_BT"] = GV2_BT.ClientID;
            Session["ISBP"] = "NO";
            //20140731
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('PersonalPOSITIDSearch.aspx?MZ_PRID=" + TextBox_MZ_PRID.Text + "&MZ_PRID1=" + TextBox_MZ_PRID1.Text.Trim() + "&MZ_ADATE=" + TextBox_MZ_ADATE.Text.Trim().Replace("/", "").PadLeft(7, '0')
                                                                                           + "&MZ_DATE=" + TextBox_MZ_DATE.Text.Trim().Replace("/", "").PadLeft(7, '0') + "&MZ_IDATE=" + TextBox_MZ_IDATE.Text.Trim().Replace("/", "").PadLeft(7, '0')
                                                                                           + "&MZ_AD=" + o_A_DLBASE.PMZAD(Session["ADPMZ_ID"].ToString()) + "&TPM_FION=" + Request.QueryString["TPM_FION"] + ");','查詢','top=190,left=200,width=450,height=200,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            if (ViewState["Mode"].ToString() == "INSERT")
            {
                foreach (object dl in Panel_POSIT1.Controls)
                {
                    if (dl is TextBox)
                    {


                        TextBox tbox = dl as TextBox;
                        if (tbox.ID == "TextBox_MZ_PRID")
                        {

                        }
                        else
                        {
                            tbox.Text = "";
                            tbox.BackColor = Color.White;
                        }
                    }
                }
            
                btDelete.Enabled = false;
                btUpdate.Enabled = false;
            }
            else if (ViewState["Mode"].ToString() == "UPDATE")
            {
                finddata(int.Parse(xcount.Text.Trim()));

                POSIT1_MZ_PRID1 = Session["POSIT1_MZ_PRID1"] as List<string>;
                if (int.Parse(xcount.Text.Trim()) == 0 && POSIT1_MZ_PRID1.Count == 1)
                {
                    btUpper.Enabled = false;
                    btNEXT.Enabled = false;
                }
                else if (int.Parse(xcount.Text.Trim()) == 0 && POSIT1_MZ_PRID1.Count > 1)
                {
                    btUpper.Enabled = false;
                    btNEXT.Enabled = true;
                }
                else if (int.Parse(xcount.Text.Trim()) + 1 == POSIT1_MZ_PRID1.Count)
                {
                    btUpper.Enabled = true;
                    btNEXT.Enabled = false;
                }
                else if (int.Parse(xcount.Text.Trim()) > 0 && int.Parse(xcount.Text.Trim()) + 1 < POSIT1_MZ_PRID1.Count)
                {
                    btNEXT.Enabled = true;
                    btUpper.Enabled = true;
                }
                btDelete.Enabled = true;
                btUpdate.Enabled = true;
            }


            Button4.Enabled = false;
            Button5.Enabled = false;
            btInsert.Enabled = true;
            btCancel.Enabled = false;
            btOK.Enabled = false;

            GridView2.DataSource = null;
            GridView2.DataBind();

            GridView1.DataSource = null;
            GridView1.DataBind();

            Session.Remove("PKEY_MZ_PRID");
            Session.Remove("PKEY_MZ_PRID1");
            ViewState.Remove("GV2_DB");
            COUNT_GV_ROWS(TextBox_MZ_PRID.Text, TextBox_MZ_PRID1.Text);
          
            A.controlEnable(ref this.Panel_POSIT1, false);
        }

        protected void Button4_Click(object sender, EventArgs e)
        {
            Session["ISBP"] = "YES";
            Session["POSIT1_BT"] = GV2_BT.ClientID;
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('PersonalPOSIT1IDSearch_WithBP.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + ");','查詢','top=190,left=200,width=610,height=400,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }

        protected void TextBox_MZ_IDATE_TextChanged(object sender, EventArgs e)
        {

            returnSameDataType(TextBox_MZ_IDATE, TextBox_MZ_ADATE);
        }

        protected void TextBox_MZ_ADATE_TextChanged(object sender, EventArgs e)
        {
            returnSameDataType(TextBox_MZ_ADATE, TextBox_MZ_EXPLAIN0);
        }

        protected void TextBox_MZ_DATE_TextChanged(object sender, EventArgs e)
        {
            returnSameDataType(TextBox_MZ_DATE, TextBox_MZ_IDATE);

            string sysday = (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0') + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0');

            if (int.Parse(sysday) > int.Parse(TextBox_MZ_DATE.Text.Trim().Replace("/", "").PadLeft(7, '0')))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('發文日期不得小於系統日期！')", true);
                TextBox_MZ_DATE.Text = string.Empty;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_DATE.ClientID + "').focus();$get('" + TextBox_MZ_DATE.ClientID + "').focus();", true);
            }

            TextBox_MZ_IDATE.Text = TextBox_MZ_DATE.Text;
        }

        protected void TextBox_TEMP_EXPLAIN_TextChanged(object sender, EventArgs e)
        {
            TextBox_MZ_EXPLAIN.Text += Environment.NewLine + TextBox_TEMP_EXPLAIN.Text.Trim();
        }

        protected void btEXPLAIN_Click(object sender, EventArgs e)
        {
            Session["KTYPE_CID"] = TextBox_TEMP_EXPLAIN.ClientID;
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_Ktype_Search.aspx?MZ_KTYPE=NOTE&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin');", true);
        }

        protected void CV_PRID_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (string.IsNullOrEmpty(args.Value))
            {
                args.IsValid = false;
                TextBox_MZ_PRID.BackColor = Color.Orange;
            }
            else
            {
                args.IsValid = true;
                TextBox_MZ_PRID.BackColor = Color.White;
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

        protected void CV_IDATE_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (string.IsNullOrEmpty(args.Value))
            {
                args.IsValid = false;
                TextBox_MZ_IDATE.BackColor = Color.Orange;
            }
            else
            {
                args.IsValid = true;
                TextBox_MZ_IDATE.BackColor = Color.White;
            }
        }

        protected void returnSameDataType(TextBox tb, TextBox tb1)
        {
            tb.Text = o_str.tosql(tb.Text.Trim().Replace("/", ""));

            if (tb.Text != "")
            {
                if (!DateManange.Check_date(tb.Text))
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('日期格式錯誤')", true);
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + tb.ClientID + "').focus();$get('" + tb.ClientID + "').focus();", true);
                }
                else
                {
                    tb.Text = o_CommonService.Personal_ReturnDateString(tb.Text.Trim());
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + tb1.ClientID + "').focus();$get('" + tb1.ClientID + "').focus();", true);
                }
            }
        }

        protected void GV2_BT_Click(object sender, EventArgs e)
        {
            GridView1.Visible = false;

            GridView2.Visible = true;

//            string strSQL = @" SELECT MZ_NO,MZ_ID,MZ_NAME,
//                             (SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE = '04' AND MZ_KCODE = MZ_AD) AS MZ_AD,
//                             (SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE = '25' AND MZ_KCODE = MZ_UNIT ) AS MZ_UNIT,
//                             (SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE = '26' AND MZ_KCODE = MZ_OCCC) AS MZ_OCCC,
//                             (SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE = '09' AND MZ_KCODE = MZ_SRANK) AS MZ_SRANK FROM A_POSIT2 
//                             WHERE MZ_PRID='" + o_str.tosql(TextBox_MZ_PRID.Text.Trim()) + "' AND MZ_PRID1='" + o_str.tosql(TextBox_MZ_PRID1.Text.Trim()) + "' ORDER BY MZ_PRID,MZ_PRID1";


            string strSQL = @" SELECT MZ_NO,MZ_ID,MZ_NAME,
                            AKD.MZ_KCHI     MZ_AD ,  AKU.MZ_KCHI   MZ_UNIT   , 
                            AKO.MZ_KCHI   MZ_OCCC ,   AKR.MZ_KCHI   
                            FROM A_POSIT2 
                            LEFT JOIN A_KTYPE AKD ON RTRIM(AKD.MZ_KCODE)=RTRIM(A_POSIT2.MZ_AD) AND RTRIM(AKD.MZ_KTYPE)='04' 
                            LEFT JOIN A_KTYPE AKU ON RTRIM(AKU.MZ_KCODE)=RTRIM(A_POSIT2.MZ_UNIT) AND RTRIM(AKU.MZ_KTYPE)='25' 
                            LEFT JOIN A_KTYPE AKO ON RTRIM(AKO.MZ_KCODE)=RTRIM(A_POSIT2.MZ_OCCC) AND RTRIM(AKO.MZ_KTYPE)='26' 
                            LEFT JOIN A_KTYPE AKR ON RTRIM(AKR.MZ_KCODE)=RTRIM(A_POSIT2.MZ_SRANK) AND RTRIM(AKR.MZ_KTYPE)='09' 
                             WHERE MZ_PRID='" + o_str.tosql(TextBox_MZ_PRID.Text.Trim()) + "' AND MZ_PRID1='" + o_str.tosql(TextBox_MZ_PRID1.Text.Trim()) + "' ORDER BY MZ_PRID,MZ_PRID1";

            DataTable GV2_DT = new DataTable();

            GV2_DT = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET_DATA");

            DataTable tempDT1 = new DataTable();

            tempDT1 = o_DBFactory.ABC_toTest.Create_Table(Session["POSIT1_GV1"].ToString(), "GETDATA");

            DataTable dt = new DataTable();

            if (Session["POSIT1_TEMP_DT2"] != null)
                dt = Session["POSIT1_TEMP_DT2"] as DataTable;

            Session["POSIT1_TEMP_DT2"] = o_CommonService.Union(tempDT1, dt);

            DataTable tempDT = new DataTable();

            tempDT = Session["POSIT1_TEMP_DT2"] as DataTable;

            Session["POSIT1_tempDT"] = tempDT;

            if (tempDT.Rows.Count > 0)
            {
                GV2_DT = o_CommonService.Union(GV2_DT, tempDT);

                ViewState["GV2_DB"] = GV2_DT;

                GridView2.DataSource = GV2_DT;

                GridView2.AllowPaging = true;

                GridView2.PageSize = 6;

                GridView2.DataBind();

                TextBox_GrivewCount.Text = GV2_DT.Rows.Count.ToString();
            }

            if (GV2_DT.Rows.Count == 0)
            {
                GridView1.Visible = true;

                GridView2.Visible = false;
            }
        }

        protected void GridView2_DataBound(object sender, EventArgs e)
        {
            if (GridView2.Rows.Count > 0)
            {
                TextBox_GrivewCount.Text = (ViewState["GV2_DB"] as DataTable).Rows.Count.ToString();
            }
        }

        protected void GridView2_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataTable tempDT = new DataTable();

            tempDT = ViewState["GV2_DB"] as DataTable;

            GridView2.PageIndex = e.NewPageIndex;

            GridView2.DataSource = tempDT;

            GridView2.AllowPaging = true;

            GridView2.PageSize = 6;

            GridView2.DataBind();

            TextBox_GrivewCount.Text = tempDT.Rows.Count.ToString();
        }

        protected void btAddNote_Click(object sender, EventArgs e)
        {
            string values = "";

            if ((sender as Button).ID == "btAddNote")
            {
                values = TextBox_MZ_EXPLAIN1.Text;
            }
            else
            {
                values = TextBox_TEMP_EXPLAIN.Text;
            }

            if (string.IsNullOrEmpty(values))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('新增片語內容不可空白！')", true);
            }

            string InsertString = "INSERT INTO A_NOTE (MZ_NOTE,MZ_NOTE_NAME) VALUES('" + Session["ADPMZ_ID"].ToString() + "','" + values + "')";

            try
            {
                o_DBFactory.ABC_toTest.Edit_Data(InsertString);
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('新增失敗！')", true);
            }
        }

        protected void bt_EXPLAIN1_Click(object sender, EventArgs e)
        {
            Session["KTYPE_CID"] = TextBox_MZ_EXPLAIN1.ClientID;
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_Ktype_Search.aspx?MZ_KTYPE=NOTE&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin');", true);
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBox_MZ_PRID.Text.Trim()) && string.IsNullOrEmpty(TextBox_MZ_PRID1.Text.Trim()))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "location.href('Personal_posit_rpt.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "');", true);
            }
            else
            {
                //string strSQL = "SELECT MZ_ID,MZ_NAME,MZ_PRID,MZ_PRID1,MZ_EXOPOS," +
                //               "MZ_NREA,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='11' AND MZ_KCODE=MZ_NREA) AS MZ_NREA_NAME," +
                //               "MZ_EXOAD,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND MZ_KCODE=MZ_EXOAD) AS MZ_EXOAD_NAME," +
                //               "MZ_EXOUNIT,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='25' AND MZ_KCODE=MZ_EXOUNIT) AS MZ_EXOUNIT_NAME," +
                //               "MZ_EXOOCC,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='26' AND MZ_KCODE=MZ_EXOOCC) AS MZ_EXOOCC_NAME," +
                //               "MZ_EXORANK,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='09' AND MZ_KCODE=MZ_EXORANK) AS MZ_EXORANK_NAME," +
                //               "MZ_EXRANK1,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='09' AND MZ_KCODE=MZ_EXRANK1) AS MZ_EXRANK1_NAME," +
                //               "MZ_SPT,MZ_REMARK,OTH_THING,CONDITION,MZ_POSIND,MZ_PNO,MZ_PNO1,MZ_EXOPNO,MZ_EXOPNO1," +
                //               "MZ_AD,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND MZ_KCODE=MZ_AD) AS MZ_AD_NAME," +
                //               "MZ_UNIT,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='25' AND MZ_KCODE=MZ_EXOUNIT) AS MZ_UNIT_NAME," +
                //               "MZ_OCCC,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='26' AND MZ_KCODE=MZ_OCCC) AS MZ_OCCC_NAME," +
                //               "MZ_RANK,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='09' AND MZ_KCODE=MZ_RANK) AS MZ_RANK_NAME," +
                //               "MZ_RANK1,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='09' AND MZ_KCODE=MZ_RANK1) AS MZ_RANK1_NAME," +
                //               "MZ_SRANK,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='09' AND MZ_KCODE=MZ_RANK1) AS MZ_SRANK_NAME," +
                //               "MZ_CHISI,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='23' AND MZ_KCODE=MZ_CHISI) AS MZ_CHISI_NAME, " +
                //               "MZ_SLVC,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='64' AND MZ_KCODE=MZ_SLVC) AS MZ_SLVC_NAME, " +
                //               "(CONVERT(VARCHAR(200),dbo.to_number(dbo.SUBSTR(MZ_DATE,0,3)))+'年'+dbo.SUBSTR(MZ_DATE,4,2)+'月'+dbo.SUBSTR(MZ_DATE,6,2)+'日') as MZ_DATE," +
                //               "(CONVERT(VARCHAR(200),dbo.to_number(dbo.SUBSTR(MZ_IDATE,0,3)))+'年'+dbo.SUBSTR(MZ_IDATE,4,2)+'月'+dbo.SUBSTR(MZ_IDATE,6,2)+'日') as MZ_IDATE," +
                //               "(Select SPEED_NO From A_CHKAD Where MZ_PRID=MZ_PRID and MZ_AD=MZ_CHKAD) AS SPEED_NO, " +
                //               "(Select PWD_NO From A_CHKAD Where MZ_PRID=MZ_PRID and MZ_AD=MZ_CHKAD) AS PWD_NO " +
                //               "FROM A_POSIT2 " +
                //               "WHERE MZ_PRID='" + TextBox_MZ_PRID.Text.Trim() + "' AND MZ_PRID1='" + o_str.tosql(TextBox_MZ_PRID1.Text.Trim()) + "'";


                string strSQL = @"SELECT MZ_ID,MZ_NAME,A_POSIT2.MZ_PRID,MZ_PRID1,MZ_EXOPOS, 
                               MZ_NREA,  AKNR.MZ_KCHI  MZ_NREA_NAME,  MZ_EXOAD, AKEXOD.MZ_KCHI   MZ_EXOAD_NAME, 
                               MZ_EXOUNIT, AKEXOU.MZ_KCHI   MZ_EXOUNIT_NAME  ,   MZ_EXOOCC,  AKEXOO.MZ_KCHI  MZ_EXOOCC_NAME , 
                                MZ_EXORANK,  AKEXOR.MZ_KCHI    MZ_EXORANK_NAME,   MZ_EXRANK1,   AKEXOR1.MZ_KCHI    MZ_EXORANK1_NAME, 
                               MZ_SPT,MZ_REMARK,OTH_THING,CONDITION,MZ_POSIND,MZ_PNO,MZ_PNO1,MZ_EXOPNO,MZ_EXOPNO1,
                              
                               A_POSIT2.MZ_AD, AKD.MZ_KCHI      MZ_AD_NAME ,  MZ_UNIT,  AKU.MZ_KCHI   MZ_UNIT_NAME   , 
                                MZ_OCCC,  AKO.MZ_KCHI   MZ_OCCC_NAME  ,  MZ_RANK, AKR.MZ_KCHI   MZ_RANK_NAME  ,  
                               MZ_RANK1,  AKR1.MZ_KCHI   MZ_RANK1_NAME ,MZ_SRANK,AKSR.MZ_KCHI  MZ_SRANK_NAME, 
                               MZ_CHISI, AKCH.MZ_KCHI  MZ_CHISI_NAME,  MZ_SLVC,   AKSL.MZ_KCHI   MZ_SLVC_NAME ,  
                               (CONVERT(VARCHAR(200),dbo.to_number(dbo.SUBSTR(MZ_DATE,0,3)))+'年'+dbo.SUBSTR(MZ_DATE,4,2)+'月'+dbo.SUBSTR(MZ_DATE,6,2)+'日') as MZ_DATE,
                               (CONVERT(VARCHAR(200),dbo.to_number(dbo.SUBSTR(MZ_IDATE,0,3)))+'年'+dbo.SUBSTR(MZ_IDATE,4,2)+'月'+dbo.SUBSTR(MZ_IDATE,6,2)+'日') as MZ_IDATE,
                               ACK.SPEED_NO      SPEED_NO  ,  ACK. PWD_NO    PWD_NO          
                               FROM A_POSIT2 
                               LEFT JOIN A_KTYPE AKNR ON RTRIM(AKNR.MZ_KCODE)=RTRIM(A_POSIT2.MZ_NREA) AND RTRIM(AKNR.MZ_KTYPE)='11' 
                                  LEFT JOIN A_KTYPE AKEXOD ON RTRIM(AKEXOD.MZ_KCODE)=RTRIM(A_POSIT2.MZ_EXOAD) AND RTRIM(AKEXOD.MZ_KTYPE)='04' 
                                  LEFT JOIN A_KTYPE AKEXOU ON RTRIM(AKEXOU.MZ_KCODE)=RTRIM(A_POSIT2.MZ_EXOUNIT) AND RTRIM(AKEXOU.MZ_KTYPE)='25' 
                                  LEFT JOIN A_KTYPE AKEXOO ON RTRIM(AKEXOO.MZ_KCODE)=RTRIM(A_POSIT2.MZ_EXOOCC) AND RTRIM(AKEXOO.MZ_KTYPE)='26' 
                                  LEFT JOIN A_KTYPE AKEXOR ON RTRIM(AKEXOR.MZ_KCODE)=RTRIM(A_POSIT2.MZ_EXORANK) AND RTRIM(AKEXOR.MZ_KTYPE)='09' 
                                  LEFT JOIN A_KTYPE AKEXOR1 ON RTRIM(AKEXOR1.MZ_KCODE)=RTRIM(A_POSIT2.MZ_EXRANK1) AND RTRIM(AKEXOR1.MZ_KTYPE)='09' 

                               LEFT JOIN A_KTYPE AKD ON RTRIM(AKD.MZ_KCODE)=RTRIM(A_POSIT2.MZ_AD) AND RTRIM(AKD.MZ_KTYPE)='04' 
                               LEFT JOIN A_KTYPE AKU ON RTRIM(AKU.MZ_KCODE)=RTRIM(A_POSIT2.MZ_UNIT) AND RTRIM(AKU.MZ_KTYPE)='25' 
                               LEFT JOIN A_KTYPE AKO ON RTRIM(AKO.MZ_KCODE)=RTRIM(A_POSIT2.MZ_OCCC) AND RTRIM(AKO.MZ_KTYPE)='26' 
                               LEFT JOIN A_KTYPE AKR ON RTRIM(AKR.MZ_KCODE)=RTRIM(A_POSIT2.MZ_RANK) AND RTRIM(AKR.MZ_KTYPE)='09' 
                               LEFT JOIN A_KTYPE AKR1 ON RTRIM(AKR1.MZ_KCODE)=RTRIM(A_POSIT2.MZ_RANK1) AND RTRIM(AKR1.MZ_KTYPE)='09' 
                               LEFT JOIN A_KTYPE AKSR ON RTRIM(AKSR.MZ_KCODE)=RTRIM(A_POSIT2.MZ_RANK1) AND RTRIM(AKSR.MZ_KTYPE)='09' 
                               LEFT JOIN A_KTYPE AKCH ON RTRIM(AKCH.MZ_KCODE)=RTRIM(A_POSIT2.MZ_CHISI) AND RTRIM(AKCH.MZ_KTYPE)='23' 
                               LEFT JOIN A_KTYPE AKSL ON RTRIM(AKSL.MZ_KCODE)=RTRIM(A_POSIT2.MZ_SLVC) AND RTRIM(AKSL.MZ_KTYPE)='64' 

                               LEFT JOIN A_CHKAD ACK ON  ACK.MZ_PRID=A_POSIT2.MZ_PRID and ACK.MZ_AD=A_POSIT2.MZ_CHKAD

                               WHERE A_POSIT2.MZ_PRID='" + TextBox_MZ_PRID.Text.Trim() + "' AND A_POSIT2.MZ_PRID1='" + o_str.tosql(TextBox_MZ_PRID1.Text.Trim()) + "'";

                strSQL += " ORDER BY A_POSIT2.MZ_AD";

                DataTable tempDT = new DataTable();

                tempDT = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");

                DataTable rpt = new DataTable();

                rpt.Columns.Add("TOJ", typeof(string));
                rpt.Columns.Add("MZ_ID", typeof(string));
                rpt.Columns.Add("MZ_NAME", typeof(string));
                rpt.Columns.Add("MZ_PRID", typeof(string));
                rpt.Columns.Add("MZ_PRID1", typeof(string));
                rpt.Columns.Add("MZ_NREA", typeof(string));
                rpt.Columns.Add("MZ_NREA_NAME", typeof(string));
                rpt.Columns.Add("OTH_THING", typeof(string));
                rpt.Columns.Add("MZ_REMARK", typeof(string));
                rpt.Columns.Add("CONDITION", typeof(string));
                rpt.Columns.Add("MZ_DATE", typeof(string));
                rpt.Columns.Add("MZ_IDATE", typeof(string));
                rpt.Columns.Add("SPEED_NO", typeof(string));
                rpt.Columns.Add("PWD_NO", typeof(string));
                rpt.Columns.Add("PRID3", typeof(string));
                rpt.Columns.Add("PRINTGROUP", typeof(string));
                rpt.Columns.Add("COUNTPAGEMAN", typeof(string));
                rpt.Columns.Add("NOW", typeof(string));
                rpt.Columns.Add("AFTER", typeof(string));

                for (int i = 0; i < tempDT.Rows.Count; i++)
                {
                    DataRow newdr = rpt.NewRow();
                    newdr["TOJ"] = string.Empty;
                    newdr["MZ_ID"] = tempDT.Rows[i]["MZ_ID"];
                    newdr["MZ_NAME"] = tempDT.Rows[i]["MZ_NAME"];
                    newdr["MZ_PRID"] = tempDT.Rows[i]["MZ_PRID"];
                    newdr["MZ_PRID1"] = tempDT.Rows[i]["MZ_PRID1"];
                    newdr["MZ_NREA"] = tempDT.Rows[i]["MZ_NREA"];
                    newdr["MZ_NREA_NAME"] = tempDT.Rows[i]["MZ_NREA_NAME"];
                    newdr["OTH_THING"] = tempDT.Rows[i]["OTH_THING"];
                    newdr["MZ_REMARK"] = tempDT.Rows[i]["MZ_REMARK"];
                    newdr["CONDITION"] = tempDT.Rows[i]["CONDITION"];
                    newdr["MZ_DATE"] = tempDT.Rows[i]["MZ_DATE"];
                    newdr["MZ_IDATE"] = tempDT.Rows[i]["MZ_IDATE"];
                    newdr["SPEED_NO"] = tempDT.Rows[i]["SPEED_NO"];
                    newdr["PWD_NO"] = tempDT.Rows[i]["PWD_NO"];
                    newdr["PRID3"] = (i / 2).ToString();
                    newdr["PRINTGROUP"] = "3";
                    newdr["COUNTPAGEMAN"] = "２";
                    newdr["NOW"] = NOW(tempDT, i);
                    newdr["AFTER"] = AFTER(tempDT, i);
                    rpt.Rows.Add(newdr);
                }

                if (tempDT.Rows.Count % 2 == 1)
                {
                    rpt.Rows[rpt.Rows.Count - 1]["COUNTPAGEMAN"] = "１";
                }

                Session["rpt_dt"] = rpt;

                Session["TITLE"] = string.Format("{0}   令(稿)", o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND MZ_KCODE='" + Session["ADPMZ_EXAD"].ToString() + "'"));

                strSQL = string.Format("SELECT MZ_EXPLAIN,MZ_EXPLAIN0,MZ_EXPLAIN1 FROM A_POSIT1 WHERE MZ_PRID='{0}' AND MZ_PRID1='{1}'", TextBox_MZ_PRID.Text, TextBox_MZ_PRID1.Text.Trim());
                DataTable temp = new DataTable();
                temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");

                string memo = string.Empty;
                string memo1 = string.Empty;
                if (temp.Rows.Count > 0)
                {
                    string exp = temp.Rows[0]["MZ_EXPLAIN"].ToString();
                    string exp0 = temp.Rows[0]["MZ_EXPLAIN0"].ToString();
                    string exp1 = temp.Rows[0]["MZ_EXPLAIN1"].ToString();
                    if (exp.IndexOf("一、") > -1 || exp.IndexOf("1.") > -1)
                    {
                        memo += "&N";
                        string[] exps = exp.Split(new char[] { '。' }, System.StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < exps.Count(); i++)
                        {
                            memo += exps[i] + "。";
                        }
                        memo += "&N";
                        memo1 += "正本：" + exp0 + "&N";
                        memo1 += "副本：" + exp1 + "&N";
                    }
                    else
                    {
                        string[] exps = exp.Split(new char[] { '。' }, System.StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < exps.Count(); i++)
                        {
                            memo += exps[i] + "。";
                        }
                        memo += "&N";
                        memo1 += "正本：" + exp0 + "&N";
                        memo1 += "副本：" + exp1 + "&N";
                    }
                    Session["EXP"] = memo;
                    Session["EXP1"] = memo1;
                }
                else
                {
                    Session["EXP"] = memo;
                    Session["EXP1"] = memo1;
                }

                strSQL = string.Format("SELECT MZ_MASTER_NAME,MZ_MASTER_POS FROM A_CHKAD WHERE MZ_PRID='{0}'", TextBox_MZ_PRID.Text.Trim());
                DataTable dt1 = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");

                string name = dt1.Rows[0]["MZ_MASTER_NAME"].ToString();
                string result_name = string.Empty;
                string result_namemark = string.Empty;

                if (dt1.Rows.Count > 0)
                {
                    for (int i = 0; i < name.Length; i++)
                    {
                        result_name += name.Substring(i, 1) + "  ";
                        if (i == 0)
                            result_namemark += name.Substring(i, 1) + "  ";
                        else
                            result_namemark += "○" + "  ";
                    }
                    Session["MAN"] = dt1.Rows[0][1].ToString() + "  " + result_namemark;
                }
                else
                {
                    Session["MAN"] = string.Empty;
                }

                strSQL = string.Format("SELECT AC.MZ_ADDRESS,ACC.MZ_ID,MZ_TELNO,MZ_EMAIL FROM A_CHKAD_CONTRACTORS ACC,A_CHKAD AC WHERE AC.MZ_AD = ACC.MZ_CHKAD");
                temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
                string result = string.Empty;
                if (temp.Rows.Count > 0)
                {
                    result += "通訊地址：" + temp.Rows[0]["MZ_ADDRESS"].ToString() + "&N";
                    result += "聯絡方式：" + o_A_DLBASE.OCCC(temp.Rows[0]["MZ_ID"].ToString()) + o_A_DLBASE.CNAME(temp.Rows[0]["MZ_ID"].ToString()) + "警用" + temp.Rows[0]["MZ_TELNO"].ToString() + "&N";
                    result += "電子信箱：" + temp.Rows[0]["MZ_EMAIL"].ToString() + "&N";
                    Session["CONN"] = result;
                }
                else
                {
                    Session["CONN"] = result;
                }

                string tmp_url = "A_rpt.aspx?fn=posit1&TPM_FION=" + TPM_FION;

                ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBox_MZ_PRID.Text.Trim()) && string.IsNullOrEmpty(TextBox_MZ_PRID1.Text.Trim()))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "location.href('Personal_posit_sug_rpt.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "');", true);
            }
            else
            {
                //string strSQL = "SELECT MZ_ID,MZ_NAME,MZ_PRID,MZ_PRID1,MZ_EXOPOS," +
                //               "MZ_NREA,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='11' AND MZ_KCODE=MZ_NREA) AS MZ_NREA_NAME," +
                //               "MZ_EXOAD,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND MZ_KCODE=MZ_EXOAD) AS MZ_EXOAD_NAME," +
                //               "MZ_EXOUNIT,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='25' AND MZ_KCODE=MZ_EXOUNIT) AS MZ_EXOUNIT_NAME," +
                //               "MZ_EXOOCC,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='26' AND MZ_KCODE=MZ_EXOOCC) AS MZ_EXOOCC_NAME," +
                //               "MZ_EXORANK,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='09' AND MZ_KCODE=MZ_EXORANK) AS MZ_EXORANK_NAME," +
                //               "MZ_EXRANK1,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='09' AND MZ_KCODE=MZ_EXRANK1) AS MZ_EXRANK1_NAME," +
                //               "MZ_SPT,MZ_REMARK,OTH_THING,CONDITION,MZ_POSIND,MZ_PNO,MZ_PNO1,MZ_EXOPNO,MZ_EXOPNO1," +
                //               "MZ_AD,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND MZ_KCODE=MZ_AD) AS MZ_AD_NAME," +
                //               "MZ_UNIT,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='25' AND MZ_KCODE=MZ_EXOUNIT) AS MZ_UNIT_NAME," +
                //               "MZ_OCCC,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='26' AND MZ_KCODE=MZ_OCCC) AS MZ_OCCC_NAME," +
                //               "MZ_RANK,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='09' AND MZ_KCODE=MZ_RANK) AS MZ_RANK_NAME," +
                //               "MZ_RANK1,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='09' AND MZ_KCODE=MZ_RANK1) AS MZ_RANK1_NAME," +
                //               "MZ_SRANK,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='09' AND MZ_KCODE=MZ_RANK1) AS MZ_SRANK_NAME," +
                //               "MZ_CHISI,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='23' AND MZ_KCODE=MZ_CHISI) AS MZ_CHISI_NAME, " +
                //               "MZ_SLVC,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='64' AND MZ_KCODE=MZ_SLVC) AS MZ_SLVC_NAME, " +
                //               "(CONVERT(VARCHAR(200),dbo.to_number(dbo.SUBSTR(MZ_DATE,0,3)))+'年'+dbo.SUBSTR(MZ_DATE,4,2)+'月'+dbo.SUBSTR(MZ_DATE,6,2)+'日') as MZ_DATE," +
                //               "(CONVERT(VARCHAR(200),dbo.to_number(dbo.SUBSTR(MZ_IDATE,0,3)))+'年'+dbo.SUBSTR(MZ_IDATE,4,2)+'月'+dbo.SUBSTR(MZ_IDATE,6,2)+'日') as MZ_IDATE," +
                //               "(Select SPEED_NO From A_CHKAD Where MZ_PRID=MZ_PRID and MZ_AD=MZ_CHKAD) AS SPEED_NO, " +
                //               "(Select PWD_NO From A_CHKAD Where MZ_PRID=MZ_PRID and MZ_AD=MZ_CHKAD) AS PWD_NO " +
                //               "FROM A_POSIT2 " +
                //               "WHERE MZ_PRID='" + TextBox_MZ_PRID.Text.Trim() + "' AND MZ_PRID1='" + o_str.tosql(TextBox_MZ_PRID1.Text.Trim()) + "'";

                //strSQL += " ORDER BY MZ_AD";

                string strSQL = @"SELECT MZ_ID,MZ_NAME,A_POSIT2.MZ_PRID,A_POSIT2.MZ_PRID1,MZ_EXOPOS,
                                 MZ_NREA,  AKNR.MZ_KCHI  MZ_NREA_NAME,  MZ_EXOAD, AKEXOD.MZ_KCHI   MZ_EXOAD_NAME, 
                               MZ_EXOUNIT, AKEXOU.MZ_KCHI   MZ_EXOUNIT_NAME  ,   MZ_EXOOCC,  AKEXOO.MZ_KCHI  MZ_EXOOCC_NAME , 
                                MZ_EXORANK,  AKEXOR.MZ_KCHI    MZ_EXORANK_NAME,   MZ_EXRANK1,   AKEXOR1.MZ_KCHI    MZ_EXORANK1_NAME, 
                               MZ_SPT,MZ_REMARK,OTH_THING,CONDITION,MZ_POSIND,MZ_PNO,MZ_PNO1,MZ_EXOPNO,MZ_EXOPNO1,
                                A_POSIT2.MZ_AD, AKD.MZ_KCHI      MZ_AD_NAME ,  MZ_UNIT,  AKU.MZ_KCHI   MZ_UNIT_NAME   , 
                                MZ_OCCC,  AKO.MZ_KCHI   MZ_OCCC_NAME  ,  MZ_RANK, AKR.MZ_KCHI   MZ_RANK_NAME  ,  
                               MZ_RANK1,  AKR1.MZ_KCHI   MZ_RANK1_NAME ,MZ_SRANK,AKSR.MZ_KCHI  MZ_SRANK_NAME, 
                               MZ_CHISI, AKCH.MZ_KCHI  MZ_CHISI_NAME,  MZ_SLVC,   AKSL.MZ_KCHI   MZ_SLVC_NAME ,  
                               (CONVERT(VARCHAR(200),dbo.to_number(dbo.SUBSTR(MZ_DATE,0,3)))+'年'+dbo.SUBSTR(MZ_DATE,4,2)+'月'+dbo.SUBSTR(MZ_DATE,6,2)+'日') as MZ_DATE,
                               (CONVERT(VARCHAR(200),dbo.to_number(dbo.SUBSTR(MZ_IDATE,0,3)))+'年'+dbo.SUBSTR(MZ_IDATE,4,2)+'月'+dbo.SUBSTR(MZ_IDATE,6,2)+'日') as MZ_IDATE,
                                ACK.SPEED_NO      SPEED_NO  ,  ACK. PWD_NO    PWD_NO       
                               FROM A_POSIT2 
                                   LEFT JOIN A_KTYPE AKNR ON RTRIM(AKNR.MZ_KCODE)=RTRIM(A_POSIT2.MZ_NREA) AND RTRIM(AKNR.MZ_KTYPE)='11' 
                                  LEFT JOIN A_KTYPE AKEXOD ON RTRIM(AKEXOD.MZ_KCODE)=RTRIM(A_POSIT2.MZ_EXOAD) AND RTRIM(AKEXOD.MZ_KTYPE)='04' 
                                  LEFT JOIN A_KTYPE AKEXOU ON RTRIM(AKEXOU.MZ_KCODE)=RTRIM(A_POSIT2.MZ_EXOUNIT) AND RTRIM(AKEXOU.MZ_KTYPE)='25' 
                                  LEFT JOIN A_KTYPE AKEXOO ON RTRIM(AKEXOO.MZ_KCODE)=RTRIM(A_POSIT2.MZ_EXOOCC) AND RTRIM(AKEXOO.MZ_KTYPE)='26' 
                                  LEFT JOIN A_KTYPE AKEXOR ON RTRIM(AKEXOR.MZ_KCODE)=RTRIM(A_POSIT2.MZ_EXORANK) AND RTRIM(AKEXOR.MZ_KTYPE)='09' 
                                  LEFT JOIN A_KTYPE AKEXOR1 ON RTRIM(AKEXOR1.MZ_KCODE)=RTRIM(A_POSIT2.MZ_EXRANK1) AND RTRIM(AKEXOR1.MZ_KTYPE)='09' 

                               LEFT JOIN A_KTYPE AKD ON RTRIM(AKD.MZ_KCODE)=RTRIM(A_POSIT2.MZ_AD) AND RTRIM(AKD.MZ_KTYPE)='04' 
                               LEFT JOIN A_KTYPE AKU ON RTRIM(AKU.MZ_KCODE)=RTRIM(A_POSIT2.MZ_UNIT) AND RTRIM(AKU.MZ_KTYPE)='25' 
                               LEFT JOIN A_KTYPE AKO ON RTRIM(AKO.MZ_KCODE)=RTRIM(A_POSIT2.MZ_OCCC) AND RTRIM(AKO.MZ_KTYPE)='26' 
                               LEFT JOIN A_KTYPE AKR ON RTRIM(AKR.MZ_KCODE)=RTRIM(A_POSIT2.MZ_RANK) AND RTRIM(AKR.MZ_KTYPE)='09' 
                               LEFT JOIN A_KTYPE AKR1 ON RTRIM(AKR1.MZ_KCODE)=RTRIM(A_POSIT2.MZ_RANK1) AND RTRIM(AKR1.MZ_KTYPE)='09' 
                               LEFT JOIN A_KTYPE AKSR ON RTRIM(AKSR.MZ_KCODE)=RTRIM(A_POSIT2.MZ_RANK1) AND RTRIM(AKSR.MZ_KTYPE)='09' 
                               LEFT JOIN A_KTYPE AKCH ON RTRIM(AKCH.MZ_KCODE)=RTRIM(A_POSIT2.MZ_CHISI) AND RTRIM(AKCH.MZ_KTYPE)='23' 
                               LEFT JOIN A_KTYPE AKSL ON RTRIM(AKSL.MZ_KCODE)=RTRIM(A_POSIT2.MZ_SLVC) AND RTRIM(AKSL.MZ_KTYPE)='64' 

                               LEFT JOIN A_CHKAD ACK ON  ACK.MZ_PRID=A_POSIT2.MZ_PRID and ACK.MZ_AD=A_POSIT2.MZ_CHKAD
                               WHERE A_POSIT2.MZ_PRID='" + TextBox_MZ_PRID.Text.Trim() + "' AND A_POSIT2.MZ_PRID1='" + o_str.tosql(TextBox_MZ_PRID1.Text.Trim()) + "'";

                strSQL += " ORDER BY A_POSIT2.MZ_AD";
                DataTable tempDT = new DataTable();

                tempDT = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");

                DataTable rpt = new DataTable();

                rpt.Columns.Add("TOJ", typeof(string));
                rpt.Columns.Add("MZ_ID", typeof(string));
                rpt.Columns.Add("MZ_NAME", typeof(string));
                rpt.Columns.Add("MZ_PRID", typeof(string));
                rpt.Columns.Add("MZ_PRID1", typeof(string));
                rpt.Columns.Add("MZ_NREA", typeof(string));
                rpt.Columns.Add("MZ_NREA_NAME", typeof(string));
                rpt.Columns.Add("OTH_THING", typeof(string));
                rpt.Columns.Add("MZ_REMARK", typeof(string));
                rpt.Columns.Add("CONDITION", typeof(string));
                rpt.Columns.Add("MZ_POSIND", typeof(string));
                rpt.Columns.Add("MZ_DATE", typeof(string));
                rpt.Columns.Add("MZ_IDATE", typeof(string));
                rpt.Columns.Add("SPEED_NO", typeof(string));
                rpt.Columns.Add("PWD_NO", typeof(string));
                rpt.Columns.Add("COUNTMAN", typeof(string));
                rpt.Columns.Add("NOW", typeof(string));
                rpt.Columns.Add("AFTER", typeof(string));

                for (int i = 0; i < tempDT.Rows.Count; i++)
                {
                    DataRow newdr = rpt.NewRow();
                    newdr["TOJ"] = string.Empty;
                    newdr["MZ_ID"] = tempDT.Rows[i]["MZ_ID"];
                    newdr["MZ_NAME"] = tempDT.Rows[i]["MZ_NAME"];
                    newdr["MZ_PRID"] = tempDT.Rows[i]["MZ_PRID"];
                    newdr["MZ_PRID1"] = tempDT.Rows[i]["MZ_PRID1"];
                    newdr["MZ_NREA"] = tempDT.Rows[i]["MZ_NREA"];
                    newdr["MZ_NREA_NAME"] = tempDT.Rows[i]["MZ_NREA_NAME"];
                    newdr["OTH_THING"] = tempDT.Rows[i]["OTH_THING"];
                    newdr["MZ_REMARK"] = tempDT.Rows[i]["MZ_REMARK"];
                    newdr["CONDITION"] = tempDT.Rows[i]["CONDITION"];
                    newdr["MZ_DATE"] = tempDT.Rows[i]["MZ_DATE"];
                    newdr["MZ_IDATE"] = tempDT.Rows[i]["MZ_IDATE"];
                    newdr["SPEED_NO"] = tempDT.Rows[i]["SPEED_NO"];
                    newdr["PWD_NO"] = tempDT.Rows[i]["PWD_NO"];
                    //20140123
                    newdr["COUNTMAN"] = DateManange.toChinese((i + 1).ToString(), 1) + "、";
                    //newdr["COUNTMAN"] = o_crystal.toChinese((i + 1).ToString(), 1) + "、";
                    newdr["NOW"] = NOW(tempDT, i);
                    newdr["AFTER"] = AFTER(tempDT, i);
                    rpt.Rows.Add(newdr);
                }

                if (rpt.Rows.Count == 1)
                {
                    rpt.Rows[0]["COUNTMAN"] = "";
                }

                Session["rpt_dt"] = rpt;

                Session["TITLE"] = string.Format("{0}   派免建議函", o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND MZ_KCODE='" + Session["ADPMZ_EXAD"].ToString() + "'"));

                strSQL = string.Format("SELECT MZ_EXPLAIN,MZ_EXPLAIN0,MZ_EXPLAIN1 FROM A_POSIT1 WHERE MZ_PRID='{0}' AND MZ_PRID1='{1}'", TextBox_MZ_PRID.Text, TextBox_MZ_PRID1.Text);
                DataTable temp = new DataTable();
                temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");

                string memo = string.Empty;
                string memo1 = string.Empty;
                if (temp.Rows.Count > 0)
                {
                    string exp = temp.Rows[0]["MZ_EXPLAIN"].ToString();
                    string exp0 = temp.Rows[0]["MZ_EXPLAIN0"].ToString();
                    string exp1 = temp.Rows[0]["MZ_EXPLAIN1"].ToString();
                    if (exp.IndexOf("一、") > -1 || exp.IndexOf("1.") > -1)
                    {
                        memo += "&N";
                        string[] exps = exp.Split(new char[] { '。' }, System.StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < exps.Count(); i++)
                        {
                            memo += exps[i] + "。";
                        }
                        memo += "&N";
                        memo1 += "正本：" + exp0 + "&N";
                        memo1 += "副本：" + exp1 + "&N";
                    }
                    else
                    {
                        string[] exps = exp.Split(new char[] { '。' }, System.StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < exps.Count(); i++)
                        {
                            memo += exps[i] + "。";
                        }
                        memo += "&N";
                        memo1 += "正本：" + exp0 + "&N";
                        memo1 += "副本：" + exp1 + "&N";
                    }
                    Session["EXP"] = memo;
                    Session["EXP1"] = memo1;
                }
                else
                {
                    Session["EXP"] = memo;
                    Session["EXP1"] = memo1;
                }

                strSQL = string.Format("SELECT AC.MZ_ADDRESS,ACC.MZ_ID,MZ_TELNO,MZ_EMAIL FROM A_CHKAD_CONTRACTORS ACC,A_CHKAD AC WHERE AC.MZ_AD = ACC.MZ_CHKAD AND ACC.MZ_CHKAD='" + Session["ADPMZ_EXAD"].ToString() + "'");
                temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
                string result = string.Empty;
                if (temp.Rows.Count > 0)
                {
                    result += "通訊地址：" + temp.Rows[0]["MZ_ADDRESS"].ToString() + "&N";
                    result += "聯絡方式：" + o_A_DLBASE.OCCC(temp.Rows[0]["MZ_ID"].ToString()) + o_A_DLBASE.CNAME(temp.Rows[0]["MZ_ID"].ToString()) + "警用" + temp.Rows[0]["MZ_TELNO"].ToString() + "&N";
                    result += "電子信箱：" + temp.Rows[0]["MZ_EMAIL"].ToString() + "&N";
                    Session["CONN"] = result;
                }
                else
                {
                    Session["CONN"] = result;
                }

                strSQL = string.Format("SELECT MZ_MASTER_NAME,MZ_MASTER_POS FROM A_CHKAD WHERE MZ_PRID='{0}'", TextBox_MZ_PRID.Text);
                DataTable dt1 = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
                if (dt1.Rows.Count > 0)
                {
                    Session["MAN"] = dt1.Rows[0][1].ToString() + "   " + dt1.Rows[0][0].ToString();
                }
                else
                {
                    Session["MAN"] = string.Empty;
                }
                string tmp_url = "A_rpt.aspx?fn=posit_sug&TPM_FION=" + TPM_FION;

                ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
            }
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBox_MZ_PRID.Text.Trim()) && string.IsNullOrEmpty(TextBox_MZ_PRID1.Text.Trim()))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "location.href('Personal_posit_rpt.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "');", true);
            }
            else
            {
                string strSQL = @"SELECT MZ_ID,MZ_NAME,A_POSIT2.MZ_PRID,A_POSIT2.MZ_PRID1,MZ_EXOPOS,
                                 MZ_NREA,  AKNR.MZ_KCHI  MZ_NREA_NAME,  MZ_EXOAD, AKEXOD.MZ_KCHI   MZ_EXOAD_NAME, 
                               MZ_EXOUNIT, AKEXOU.MZ_KCHI   MZ_EXOUNIT_NAME  ,   MZ_EXOOCC,  AKEXOO.MZ_KCHI  MZ_EXOOCC_NAME , 
                                MZ_EXORANK,  AKEXOR.MZ_KCHI    MZ_EXORANK_NAME,   MZ_EXRANK1,   AKEXOR1.MZ_KCHI    MZ_EXORANK1_NAME, 
                               MZ_SPT,MZ_REMARK,OTH_THING,CONDITION,MZ_POSIND,MZ_PNO,MZ_PNO1,MZ_EXOPNO,MZ_EXOPNO1,
                               A_POSIT2.MZ_AD, AKD.MZ_KCHI      MZ_AD_NAME ,  MZ_UNIT,  AKU.MZ_KCHI   MZ_UNIT_NAME   , 
                                MZ_OCCC,  AKO.MZ_KCHI   MZ_OCCC_NAME  ,  MZ_RANK, AKR.MZ_KCHI   MZ_RANK_NAME  ,  
                               MZ_RANK1,  AKR1.MZ_KCHI   MZ_RANK1_NAME ,MZ_SRANK,AKSR.MZ_KCHI  MZ_SRANK_NAME, 
                               MZ_CHISI, AKCH.MZ_KCHI  MZ_CHISI_NAME,  MZ_SLVC,   AKSL.MZ_KCHI   MZ_SLVC_NAME ,  
                               (CONVERT(VARCHAR(200),dbo.to_number(dbo.SUBSTR(MZ_DATE,0,3)))+'年'+dbo.SUBSTR(MZ_DATE,4,2)+'月'+dbo.SUBSTR(MZ_DATE,6,2)+'日') as MZ_DATE,
                               (CONVERT(VARCHAR(200),dbo.to_number(dbo.SUBSTR(MZ_IDATE,0,3)))+'年'+dbo.SUBSTR(MZ_IDATE,4,2)+'月'+dbo.SUBSTR(MZ_IDATE,6,2)+'日') as MZ_IDATE,
                                ACK.SPEED_NO      SPEED_NO  ,  ACK. PWD_NO    PWD_NO       
                               FROM A_POSIT2 
                                  LEFT JOIN A_KTYPE AKNR ON RTRIM(AKNR.MZ_KCODE)=RTRIM(A_POSIT2.MZ_NREA) AND RTRIM(AKNR.MZ_KTYPE)='11' 
                                  LEFT JOIN A_KTYPE AKEXOD ON RTRIM(AKEXOD.MZ_KCODE)=RTRIM(A_POSIT2.MZ_EXOAD) AND RTRIM(AKEXOD.MZ_KTYPE)='04' 
                                  LEFT JOIN A_KTYPE AKEXOU ON RTRIM(AKEXOU.MZ_KCODE)=RTRIM(A_POSIT2.MZ_EXOUNIT) AND RTRIM(AKEXOU.MZ_KTYPE)='25' 
                                  LEFT JOIN A_KTYPE AKEXOO ON RTRIM(AKEXOO.MZ_KCODE)=RTRIM(A_POSIT2.MZ_EXOOCC) AND RTRIM(AKEXOO.MZ_KTYPE)='26' 
                                  LEFT JOIN A_KTYPE AKEXOR ON RTRIM(AKEXOR.MZ_KCODE)=RTRIM(A_POSIT2.MZ_EXORANK) AND RTRIM(AKEXOR.MZ_KTYPE)='09' 
                                  LEFT JOIN A_KTYPE AKEXOR1 ON RTRIM(AKEXOR1.MZ_KCODE)=RTRIM(A_POSIT2.MZ_EXRANK1) AND RTRIM(AKEXOR1.MZ_KTYPE)='09' 

                               LEFT JOIN A_KTYPE AKD ON RTRIM(AKD.MZ_KCODE)=RTRIM(A_POSIT2.MZ_AD) AND RTRIM(AKD.MZ_KTYPE)='04' 
                               LEFT JOIN A_KTYPE AKU ON RTRIM(AKU.MZ_KCODE)=RTRIM(A_POSIT2.MZ_UNIT) AND RTRIM(AKU.MZ_KTYPE)='25' 
                               LEFT JOIN A_KTYPE AKO ON RTRIM(AKO.MZ_KCODE)=RTRIM(A_POSIT2.MZ_OCCC) AND RTRIM(AKO.MZ_KTYPE)='26' 
                               LEFT JOIN A_KTYPE AKR ON RTRIM(AKR.MZ_KCODE)=RTRIM(A_POSIT2.MZ_RANK) AND RTRIM(AKR.MZ_KTYPE)='09' 
                               LEFT JOIN A_KTYPE AKR1 ON RTRIM(AKR1.MZ_KCODE)=RTRIM(A_POSIT2.MZ_RANK1) AND RTRIM(AKR1.MZ_KTYPE)='09' 
                               LEFT JOIN A_KTYPE AKSR ON RTRIM(AKSR.MZ_KCODE)=RTRIM(A_POSIT2.MZ_RANK1) AND RTRIM(AKSR.MZ_KTYPE)='09' 
                               LEFT JOIN A_KTYPE AKCH ON RTRIM(AKCH.MZ_KCODE)=RTRIM(A_POSIT2.MZ_CHISI) AND RTRIM(AKCH.MZ_KTYPE)='23' 
                               LEFT JOIN A_KTYPE AKSL ON RTRIM(AKSL.MZ_KCODE)=RTRIM(A_POSIT2.MZ_SLVC) AND RTRIM(AKSL.MZ_KTYPE)='64' 

                               LEFT JOIN A_CHKAD ACK ON  ACK.MZ_PRID=A_POSIT2.MZ_PRID and ACK.MZ_AD=A_POSIT2.MZ_CHKAD
                               WHERE A_POSIT2.MZ_PRID='" + TextBox_MZ_PRID.Text.Trim() + "' AND A_POSIT2.MZ_PRID1='" + o_str.tosql(TextBox_MZ_PRID1.Text.Trim()) + "'";

                strSQL += " ORDER BY A_POSIT2.MZ_AD";





                DataTable tempDT = new DataTable();

                tempDT = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");

                DataTable rpt = new DataTable();

                rpt.Columns.Add("TOJ", typeof(string));
                rpt.Columns.Add("MZ_ID", typeof(string));
                rpt.Columns.Add("MZ_NAME", typeof(string));
                rpt.Columns.Add("MZ_PRID", typeof(string));
                rpt.Columns.Add("MZ_PRID1", typeof(string));
                rpt.Columns.Add("MZ_NREA", typeof(string));
                rpt.Columns.Add("MZ_NREA_NAME", typeof(string));
                rpt.Columns.Add("OTH_THING", typeof(string));
                rpt.Columns.Add("MZ_REMARK", typeof(string));
                rpt.Columns.Add("CONDITION", typeof(string));
                rpt.Columns.Add("MZ_POSIND", typeof(string));
                rpt.Columns.Add("MZ_DATE", typeof(string));
                rpt.Columns.Add("MZ_IDATE", typeof(string));
                rpt.Columns.Add("SPEED_NO", typeof(string));
                rpt.Columns.Add("PWD_NO", typeof(string));
                rpt.Columns.Add("PRID3", typeof(string));
                rpt.Columns.Add("PRINTGROUP", typeof(string));
                rpt.Columns.Add("COUNTPAGEMAN", typeof(string));
                rpt.Columns.Add("NOW", typeof(string));
                rpt.Columns.Add("AFTER", typeof(string));
                int side1 = 0;

                int count = 0;

                for (int i = 0; i < tempDT.Rows.Count; i++)
                {
                    if (tempDT.Rows.Count - i >= 2)
                    {
                        for (int k = side1; k <= side1 + 1; k++)
                        {
                            DataRow newdr = rpt.NewRow();
                            newdr["TOJ"] = string.Empty;
                            newdr["MZ_ID"] = tempDT.Rows[k]["MZ_ID"];
                            newdr["MZ_NAME"] = tempDT.Rows[k]["MZ_NAME"];
                            newdr["MZ_PRID"] = tempDT.Rows[k]["MZ_PRID"];
                            newdr["MZ_PRID1"] = tempDT.Rows[k]["MZ_PRID1"];
                            newdr["MZ_NREA"] = tempDT.Rows[k]["MZ_NREA"];
                            newdr["MZ_NREA_NAME"] = tempDT.Rows[k]["MZ_NREA_NAME"];
                            newdr["OTH_THING"] = tempDT.Rows[k]["OTH_THING"];
                            newdr["MZ_REMARK"] = tempDT.Rows[k]["MZ_REMARK"];
                            newdr["CONDITION"] = tempDT.Rows[k]["CONDITION"];
                            newdr["MZ_DATE"] = tempDT.Rows[k]["MZ_DATE"];
                            newdr["MZ_IDATE"] = tempDT.Rows[k]["MZ_IDATE"];
                            newdr["SPEED_NO"] = tempDT.Rows[k]["SPEED_NO"];
                            newdr["PWD_NO"] = tempDT.Rows[k]["PWD_NO"];
                            newdr["PRID3"] = (i / 2).ToString();
                            newdr["PRINTGROUP"] = "1";
                            newdr["COUNTPAGEMAN"] = "２";
                            newdr["NOW"] = NOW(tempDT, k);
                            newdr["AFTER"] = AFTER(tempDT, k);
                            rpt.Rows.Add(newdr);
                        }
                    }
                    else
                    {
                        if (tempDT.Rows.Count % 2 == 0)
                        {
                            DataRow newdr = rpt.NewRow();
                            newdr["TOJ"] = string.Empty;
                            newdr["MZ_ID"] = tempDT.Rows[side1]["MZ_ID"];
                            newdr["MZ_NAME"] = tempDT.Rows[side1]["MZ_NAME"];
                            newdr["MZ_PRID"] = tempDT.Rows[side1]["MZ_PRID"];
                            newdr["MZ_PRID1"] = tempDT.Rows[side1]["MZ_PRID1"];
                            newdr["MZ_NREA"] = tempDT.Rows[side1]["MZ_NREA"];
                            newdr["MZ_NREA_NAME"] = tempDT.Rows[side1]["MZ_NREA_NAME"];
                            newdr["OTH_THING"] = tempDT.Rows[side1]["OTH_THING"];
                            newdr["MZ_REMARK"] = tempDT.Rows[side1]["MZ_REMARK"];
                            newdr["CONDITION"] = tempDT.Rows[side1]["CONDITION"];
                            newdr["MZ_DATE"] = tempDT.Rows[side1]["MZ_DATE"];
                            newdr["MZ_IDATE"] = tempDT.Rows[side1]["MZ_IDATE"];
                            newdr["SPEED_NO"] = tempDT.Rows[side1]["SPEED_NO"];
                            newdr["PWD_NO"] = tempDT.Rows[side1]["PWD_NO"];
                            newdr["PRID3"] = (i / 2).ToString();
                            newdr["PRINTGROUP"] = "1";
                            newdr["COUNTPAGEMAN"] = "２";
                            newdr["NOW"] = NOW(tempDT, side1);
                            newdr["AFTER"] = AFTER(tempDT, side1);
                            rpt.Rows.Add(newdr);

                            newdr = rpt.NewRow();
                            newdr["TOJ"] = string.Empty;
                            newdr["MZ_ID"] = tempDT.Rows[side1 + 1]["MZ_ID"];
                            newdr["MZ_NAME"] = tempDT.Rows[side1 + 1]["MZ_NAME"];
                            newdr["MZ_PRID"] = tempDT.Rows[side1 + 1]["MZ_PRID"];
                            newdr["MZ_PRID1"] = tempDT.Rows[side1 + 1]["MZ_PRID1"];
                            newdr["MZ_NREA"] = tempDT.Rows[side1 + 1]["MZ_NREA"];
                            newdr["MZ_NREA_NAME"] = tempDT.Rows[side1 + 1]["MZ_NREA_NAME"];
                            newdr["OTH_THING"] = tempDT.Rows[side1 + 1]["OTH_THING"];
                            newdr["MZ_REMARK"] = tempDT.Rows[side1 + 1]["MZ_REMARK"];
                            newdr["CONDITION"] = tempDT.Rows[side1 + 1]["CONDITION"];
                            newdr["MZ_DATE"] = tempDT.Rows[side1 + 1]["MZ_DATE"];
                            newdr["MZ_IDATE"] = tempDT.Rows[side1 + 1]["MZ_IDATE"];
                            newdr["SPEED_NO"] = tempDT.Rows[side1 + 1]["SPEED_NO"];
                            newdr["PWD_NO"] = tempDT.Rows[side1 + 1]["PWD_NO"];
                            newdr["PRID3"] = (i / 2).ToString();
                            newdr["PRINTGROUP"] = "1";
                            newdr["COUNTPAGEMAN"] = "２";
                            newdr["NOW"] = NOW(tempDT, side1 + 1);
                            newdr["AFTER"] = AFTER(tempDT, side1 + 1);
                            rpt.Rows.Add(newdr);
                        }
                        else
                        {

                            DataRow newdr = rpt.NewRow();
                            newdr["TOJ"] = string.Empty;
                            newdr["MZ_ID"] = tempDT.Rows[side1]["MZ_ID"];
                            newdr["MZ_NAME"] = tempDT.Rows[side1]["MZ_NAME"];
                            newdr["MZ_PRID"] = tempDT.Rows[side1]["MZ_PRID"];
                            newdr["MZ_PRID1"] = tempDT.Rows[side1]["MZ_PRID1"];
                            newdr["MZ_NREA"] = tempDT.Rows[side1]["MZ_NREA"];
                            newdr["MZ_NREA_NAME"] = tempDT.Rows[side1]["MZ_NREA_NAME"];
                            newdr["OTH_THING"] = tempDT.Rows[side1]["OTH_THING"];
                            newdr["MZ_REMARK"] = tempDT.Rows[side1]["MZ_REMARK"];
                            newdr["CONDITION"] = tempDT.Rows[side1]["CONDITION"];
                            newdr["MZ_DATE"] = tempDT.Rows[side1]["MZ_DATE"];
                            newdr["MZ_IDATE"] = tempDT.Rows[side1]["MZ_IDATE"];
                            newdr["SPEED_NO"] = tempDT.Rows[side1]["SPEED_NO"];
                            newdr["PWD_NO"] = tempDT.Rows[side1]["PWD_NO"];
                            newdr["PRID3"] = (i / 2).ToString();
                            newdr["PRINTGROUP"] = "1";
                            newdr["COUNTPAGEMAN"] = "２";
                            newdr["NOW"] = NOW(tempDT, side1);
                            newdr["AFTER"] = AFTER(tempDT, side1);
                            rpt.Rows.Add(newdr);

                        }
                    }
                    count++;

                    if (count == 2)
                    {
                        side1 = side1 + 2;
                        count = 0;
                    }
                }

                int side = 0;

                count = 0;

                for (int i = 0; i < rpt.Rows.Count; i++)
                {
                    if (i % 2 == 0)
                    {
                        rpt.Rows[i]["TOJ"] = tempDT.Rows[side]["MZ_NAME"].ToString();
                        side++;
                    }
                }

                string oldAD = "";

                for (int i = 0; i < tempDT.Rows.Count; i++)
                {
                    if (oldAD != "" && oldAD != tempDT.Rows[i]["MZ_AD"].ToString() && i % 2 == 1)
                    {
                        DataRow newdr = rpt.NewRow();
                        newdr["TOJ"] = string.Empty;
                        newdr["MZ_ID"] = tempDT.Rows[i]["MZ_ID"];
                        newdr["MZ_NAME"] = tempDT.Rows[i]["MZ_NAME"];
                        newdr["MZ_PRID"] = tempDT.Rows[i]["MZ_PRID"];
                        newdr["MZ_PRID1"] = tempDT.Rows[i]["MZ_PRID1"];
                        newdr["MZ_NREA"] = tempDT.Rows[i]["MZ_NREA"];
                        newdr["MZ_NREA_NAME"] = tempDT.Rows[i]["MZ_NREA_NAME"];
                        newdr["OTH_THING"] = tempDT.Rows[i]["OTH_THING"];
                        newdr["MZ_REMARK"] = tempDT.Rows[i]["MZ_REMARK"];
                        newdr["CONDITION"] = tempDT.Rows[i]["CONDITION"];
                        newdr["MZ_POSIND"] = tempDT.Rows[i]["MZ_POSIND"];
                        newdr["MZ_DATE"] = tempDT.Rows[i]["MZ_DATE"];
                        newdr["MZ_IDATE"] = tempDT.Rows[i]["MZ_IDATE"];
                        newdr["SPEED_NO"] = tempDT.Rows[i]["SPEED_NO"];
                        newdr["PWD_NO"] = tempDT.Rows[i]["PWD_NO"];
                        newdr["PRID3"] = (i / 2).ToString();
                        newdr["PRINTGROUP"] = "2";
                        newdr["COUNTPAGEMAN"] = "２";
                        newdr["NOW"] = NOW(tempDT, i);
                        newdr["AFTER"] = AFTER(tempDT, i);

                        rpt.Rows.Add(newdr);

                        newdr = rpt.NewRow();
                        newdr["TOJ"] = tempDT.Rows[i - 1]["MZ_AD_NAME"];
                        newdr["MZ_ID"] = tempDT.Rows[i - 1]["MZ_ID"];
                        newdr["MZ_NAME"] = tempDT.Rows[i - 1]["MZ_NAME"];
                        newdr["MZ_PRID"] = tempDT.Rows[i - 1]["MZ_PRID"];
                        newdr["MZ_PRID1"] = tempDT.Rows[i - 1]["MZ_PRID1"];
                        newdr["MZ_NREA"] = tempDT.Rows[i - 1]["MZ_NREA"];
                        newdr["MZ_NREA_NAME"] = tempDT.Rows[i - 1]["MZ_NREA_NAME"];
                        newdr["OTH_THING"] = tempDT.Rows[i - 1]["OTH_THING"];
                        newdr["MZ_REMARK"] = tempDT.Rows[i - 1]["MZ_REMARK"];
                        newdr["CONDITION"] = tempDT.Rows[i - 1]["CONDITION"];
                        newdr["MZ_POSIND"] = tempDT.Rows[i - 1]["MZ_POSIND"];
                        newdr["MZ_DATE"] = tempDT.Rows[i - 1]["MZ_DATE"];
                        newdr["MZ_IDATE"] = tempDT.Rows[i - 1]["MZ_IDATE"];
                        newdr["SPEED_NO"] = tempDT.Rows[i - 1]["SPEED_NO"];
                        newdr["PWD_NO"] = tempDT.Rows[i - 1]["PWD_NO"];
                        newdr["PRID3"] = (i / 2).ToString();
                        newdr["PRINTGROUP"] = "2";
                        newdr["COUNTPAGEMAN"] = "２";
                        newdr["NOW"] = NOW(tempDT, i);
                        newdr["AFTER"] = AFTER(tempDT, i);
                        rpt.Rows.Add(newdr);

                        newdr = rpt.NewRow();
                        newdr["TOJ"] = string.Empty;
                        newdr["MZ_ID"] = tempDT.Rows[i]["MZ_ID"];
                        newdr["MZ_NAME"] = tempDT.Rows[i]["MZ_NAME"];
                        newdr["MZ_PRID"] = tempDT.Rows[i]["MZ_PRID"];
                        newdr["MZ_PRID1"] = tempDT.Rows[i]["MZ_PRID1"];
                        newdr["MZ_NREA"] = tempDT.Rows[i]["MZ_NREA"];
                        newdr["MZ_NREA_NAME"] = tempDT.Rows[i]["MZ_NREA_NAME"];
                        newdr["OTH_THING"] = tempDT.Rows[i]["OTH_THING"];
                        newdr["MZ_REMARK"] = tempDT.Rows[i]["MZ_REMARK"];
                        newdr["CONDITION"] = tempDT.Rows[i]["CONDITION"];
                        newdr["MZ_DATE"] = tempDT.Rows[i]["MZ_DATE"];
                        newdr["MZ_IDATE"] = tempDT.Rows[i]["MZ_IDATE"];
                        newdr["SPEED_NO"] = tempDT.Rows[i]["SPEED_NO"];
                        newdr["PWD_NO"] = tempDT.Rows[i]["PWD_NO"];
                        newdr["PRID3"] = (i / 2).ToString();
                        newdr["PRINTGROUP"] = "2";
                        newdr["COUNTPAGEMAN"] = "２";
                        newdr["NOW"] = NOW(tempDT, i);
                        newdr["AFTER"] = AFTER(tempDT, i);
                        rpt.Rows.Add(newdr);
                    }
                    else if (i % 2 == 0)
                    {
                        DataRow newdr = rpt.NewRow();
                        newdr["TOJ"] = tempDT.Rows[i]["MZ_AD_NAME"];
                        newdr["MZ_ID"] = tempDT.Rows[i]["MZ_ID"];
                        newdr["MZ_NAME"] = tempDT.Rows[i]["MZ_NAME"];
                        newdr["MZ_PRID"] = tempDT.Rows[i]["MZ_PRID"];
                        newdr["MZ_PRID1"] = tempDT.Rows[i]["MZ_PRID1"];
                        newdr["MZ_NREA"] = tempDT.Rows[i]["MZ_NREA"];
                        newdr["MZ_NREA_NAME"] = tempDT.Rows[i]["MZ_NREA_NAME"];
                        newdr["OTH_THING"] = tempDT.Rows[i]["OTH_THING"];
                        newdr["MZ_REMARK"] = tempDT.Rows[i]["MZ_REMARK"];
                        newdr["CONDITION"] = tempDT.Rows[i]["CONDITION"];
                        newdr["MZ_DATE"] = tempDT.Rows[i]["MZ_DATE"];
                        newdr["MZ_IDATE"] = tempDT.Rows[i]["MZ_IDATE"];
                        newdr["SPEED_NO"] = tempDT.Rows[i]["SPEED_NO"];
                        newdr["PWD_NO"] = tempDT.Rows[i]["PWD_NO"];
                        newdr["PRID3"] = (i / 2).ToString();
                        newdr["PRINTGROUP"] = "2";
                        newdr["COUNTPAGEMAN"] = "２";
                        newdr["NOW"] = NOW(tempDT, i);
                        newdr["AFTER"] = AFTER(tempDT, i);
                        rpt.Rows.Add(newdr);
                    }
                    else
                    {
                        DataRow newdr = rpt.NewRow();
                        newdr["TOJ"] = string.Empty;
                        newdr["MZ_ID"] = tempDT.Rows[i]["MZ_ID"];
                        newdr["MZ_NAME"] = tempDT.Rows[i]["MZ_NAME"];
                        newdr["MZ_PRID"] = tempDT.Rows[i]["MZ_PRID"];
                        newdr["MZ_PRID1"] = tempDT.Rows[i]["MZ_PRID1"];
                        newdr["MZ_NREA"] = tempDT.Rows[i]["MZ_NREA"];
                        newdr["MZ_NREA_NAME"] = tempDT.Rows[i]["MZ_NREA_NAME"];
                        newdr["OTH_THING"] = tempDT.Rows[i]["OTH_THING"];
                        newdr["MZ_REMARK"] = tempDT.Rows[i]["MZ_REMARK"];
                        newdr["CONDITION"] = tempDT.Rows[i]["CONDITION"];
                        newdr["MZ_DATE"] = tempDT.Rows[i]["MZ_DATE"];
                        newdr["MZ_IDATE"] = tempDT.Rows[i]["MZ_IDATE"];
                        newdr["SPEED_NO"] = tempDT.Rows[i]["SPEED_NO"];
                        newdr["PWD_NO"] = tempDT.Rows[i]["PWD_NO"];
                        newdr["PRID3"] = (i / 2).ToString();
                        newdr["PRINTGROUP"] = "2";
                        newdr["COUNTPAGEMAN"] = "２";
                        newdr["NOW"] = NOW(tempDT, i);
                        newdr["AFTER"] = AFTER(tempDT, i);
                        rpt.Rows.Add(newdr);
                    }

                    oldAD = tempDT.Rows[i]["MZ_AD"].ToString();
                }

                if (tempDT.Rows.Count % 2 == 1)
                {
                    rpt.Rows[rpt.Rows.Count - 1]["COUNTPAGEMAN"] = "１";
                }

                string[] explain = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_EXPLAIN1 FROM A_POSIT1 WHERE MZ_PRID='" + TextBox_MZ_PRID.Text + "' AND MZ_PRID1='" + TextBox_MZ_PRID1.Text.Trim() + "'").Split(new char[] { '、' });

                for (int j = 0; j < explain.Length; j++)
                {
                    for (int i = 0; i < tempDT.Rows.Count; i++)
                    {
                        if (i % 2 == 0)
                        {
                            DataRow newdr = rpt.NewRow();
                            newdr["TOJ"] = explain[j];
                            newdr["MZ_ID"] = tempDT.Rows[i]["MZ_ID"];
                            newdr["MZ_NAME"] = tempDT.Rows[i]["MZ_NAME"];
                            newdr["MZ_PRID"] = tempDT.Rows[i]["MZ_PRID"];
                            newdr["MZ_PRID1"] = tempDT.Rows[i]["MZ_PRID1"];
                            newdr["MZ_NREA"] = tempDT.Rows[i]["MZ_NREA"];
                            newdr["MZ_NREA_NAME"] = tempDT.Rows[i]["MZ_NREA_NAME"];
                            newdr["OTH_THING"] = tempDT.Rows[i]["OTH_THING"];
                            newdr["MZ_REMARK"] = tempDT.Rows[i]["MZ_REMARK"];
                            newdr["CONDITION"] = tempDT.Rows[i]["CONDITION"];
                            newdr["MZ_POSIND"] = tempDT.Rows[i]["MZ_POSIND"];
                            newdr["MZ_DATE"] = tempDT.Rows[i]["MZ_DATE"];
                            newdr["MZ_IDATE"] = tempDT.Rows[i]["MZ_IDATE"];
                            newdr["SPEED_NO"] = tempDT.Rows[i]["SPEED_NO"];
                            newdr["PWD_NO"] = tempDT.Rows[i]["PWD_NO"];
                            newdr["PRID3"] = (i / 2).ToString();
                            newdr["PRINTGROUP"] = (3 + j).ToString();
                            newdr["COUNTPAGEMAN"] = "２";
                            newdr["NOW"] = NOW(tempDT, i);
                            newdr["AFTER"] = AFTER(tempDT, i);
                            rpt.Rows.Add(newdr);
                        }
                        else
                        {
                            DataRow newdr = rpt.NewRow();
                            newdr["TOJ"] = string.Empty;
                            newdr["MZ_ID"] = tempDT.Rows[i]["MZ_ID"];
                            newdr["MZ_NAME"] = tempDT.Rows[i]["MZ_NAME"];
                            newdr["MZ_PRID"] = tempDT.Rows[i]["MZ_PRID"];
                            newdr["MZ_PRID1"] = tempDT.Rows[i]["MZ_PRID1"];
                            newdr["MZ_NREA"] = tempDT.Rows[i]["MZ_NREA"];
                            newdr["MZ_NREA_NAME"] = tempDT.Rows[i]["MZ_NREA_NAME"];
                            newdr["OTH_THING"] = tempDT.Rows[i]["OTH_THING"];
                            newdr["MZ_REMARK"] = tempDT.Rows[i]["MZ_REMARK"];
                            newdr["CONDITION"] = tempDT.Rows[i]["CONDITION"];
                            newdr["MZ_DATE"] = tempDT.Rows[i]["MZ_DATE"];
                            newdr["MZ_IDATE"] = tempDT.Rows[i]["MZ_IDATE"];
                            newdr["SPEED_NO"] = tempDT.Rows[i]["SPEED_NO"];
                            newdr["PWD_NO"] = tempDT.Rows[i]["PWD_NO"];
                            newdr["PRID3"] = (i / 2).ToString();
                            newdr["PRINTGROUP"] = (3 + j).ToString();
                            newdr["COUNTPAGEMAN"] = "２";
                            newdr["NOW"] = NOW(tempDT, i);
                            newdr["AFTER"] = AFTER(tempDT, i);
                            rpt.Rows.Add(newdr);
                        }
                    }

                    if (tempDT.Rows.Count % 2 == 1)
                    {
                        rpt.Rows[rpt.Rows.Count - 1]["COUNTPAGEMAN"] = "１";
                    }
                }

                Session["rpt_dt"] = rpt;

                Session["TITLE"] = string.Format("{0}  令", o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND MZ_KCODE='" + Session["ADPMZ_EXAD"].ToString() + "'"));

                strSQL = string.Format("SELECT MZ_EXPLAIN,MZ_EXPLAIN0,MZ_EXPLAIN1 FROM A_POSIT1 WHERE MZ_PRID='{0}' AND MZ_PRID1='{1}'", TextBox_MZ_PRID.Text, TextBox_MZ_PRID1.Text.Trim());
                DataTable temp = new DataTable();
                temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");

                string memo = string.Empty;
                string memo1 = string.Empty;
                if (temp.Rows.Count > 0)
                {
                    string exp = temp.Rows[0]["MZ_EXPLAIN"].ToString();
                    string exp0 = temp.Rows[0]["MZ_EXPLAIN0"].ToString();
                    string exp1 = temp.Rows[0]["MZ_EXPLAIN1"].ToString();
                    if (exp.IndexOf("一、") > -1 || exp.IndexOf("1.") > -1)
                    {
                        memo += "&N";
                        string[] exps = exp.Split(new char[] { '。' }, System.StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < exps.Count(); i++)
                        {
                            memo += exps[i] + "。";
                        }
                        memo += "&N";
                        memo1 += "正本：" + exp0 + "&N";
                        memo1 += "副本：" + exp1 + "&N";
                    }
                    else
                    {
                        string[] exps = exp.Split(new char[] { '。' }, System.StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < exps.Count(); i++)
                        {
                            memo += exps[i] + "。";
                        }
                        memo += "&N";
                        memo1 += "正本：" + exp0 + "&N";
                        memo1 += "副本：" + exp1 + "&N";
                    }
                    Session["EXP"] = memo;
                    Session["EXP1"] = memo1;
                }
                else
                {
                    Session["EXP"] = memo;
                    Session["EXP1"] = memo1;
                }

                strSQL = string.Format("SELECT MZ_MASTER_NAME,MZ_MASTER_POS FROM A_CHKAD WHERE MZ_PRID='{0}'", TextBox_MZ_PRID.Text.Trim());
                DataTable dt1 = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
                if (dt1.Rows.Count > 0)
                {
                    Session["MAN"] = dt1.Rows[0][1].ToString() + "   " + dt1.Rows[0][0].ToString();
                }
                else
                {
                    Session["MAN"] = string.Empty;
                }

                strSQL = string.Format("SELECT AC.MZ_ADDRESS,ACC.MZ_ID,MZ_TELNO,MZ_EMAIL FROM A_CHKAD_CONTRACTORS ACC,A_CHKAD AC WHERE AC.MZ_AD = ACC.MZ_CHKAD");
                temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
                string result = string.Empty;
                if (temp.Rows.Count > 0)
                {
                    result += "通訊地址：" + temp.Rows[0]["MZ_ADDRESS"].ToString() + "&N";
                    result += "聯絡方式：" + o_A_DLBASE.OCCC(temp.Rows[0]["MZ_ID"].ToString()) + o_A_DLBASE.CNAME(temp.Rows[0]["MZ_ID"].ToString()) + "警用" + temp.Rows[0]["MZ_TELNO"].ToString() + "&N";
                    result += "電子信箱：" + temp.Rows[0]["MZ_EMAIL"].ToString() + "&N";
                    Session["CONN"] = result;
                }
                else
                {
                    Session["CONN"] = result;
                }


                string tmp_url = "A_rpt.aspx?fn=posit&TPM_FION=" + TPM_FION;

                ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
            }

        }
        private string NOW(DataTable dt, int row)
        {
            string result = "";

            if (!string.IsNullOrEmpty(dt.Rows[row]["MZ_EXOAD"].ToString()))
            {
                result += dt.Rows[row]["MZ_EXOAD_NAME"].ToString() + "(" + dt.Rows[row]["MZ_EXOAD"].ToString() + ")";
            }

            if (!string.IsNullOrEmpty(dt.Rows[row]["MZ_EXOUNIT"].ToString()))
            {
                result += "，" + dt.Rows[row]["MZ_EXOUNIT_NAME"].ToString() + "(" + dt.Rows[row]["MZ_EXOUNIT"].ToString() + ")";
            }

            if (!string.IsNullOrEmpty(dt.Rows[row]["MZ_EXOPNO"].ToString()))
            {
                result += "，" + dt.Rows[row]["MZ_EXOPNO"].ToString() + "(" + dt.Rows[row]["MZ_EXOPNO1"].ToString() + ")";
            }

            if (!string.IsNullOrEmpty(dt.Rows[row]["MZ_EXOOCC"].ToString()))
            {
                result += "，" + dt.Rows[row]["MZ_EXOOCC_NAME"].ToString() + "(" + dt.Rows[row]["MZ_EXOOCC"].ToString() + ")";
            }

            if (!string.IsNullOrEmpty(dt.Rows[row]["MZ_EXORANK"].ToString()) && !string.IsNullOrEmpty(dt.Rows[row]["MZ_EXRANK1"].ToString()))
            {
                result += "，" + dt.Rows[row]["MZ_EXORANK_NAME"].ToString() + "至" + dt.Rows[row]["MZ_EXRANK1_NAME"].ToString() + "(" + dt.Rows[row]["MZ_EXORANK"].ToString() + "-" + dt.Rows[row]["MZ_EXRANK1"].ToString() + ")";
            }
            else if (!string.IsNullOrEmpty(dt.Rows[row]["MZ_EXRANK1"].ToString()))
            {
                result += "，" + dt.Rows[row]["MZ_EXRANK1_NAME"].ToString() + "(" + dt.Rows[row]["MZ_EXRANK1"].ToString() + ")";
            }

            if (!string.IsNullOrEmpty(dt.Rows[row]["MZ_POSIND"].ToString()))
            {
                result += "，" + dt.Rows[row]["MZ_POSIND"].ToString() + "'";
            }

            if (!string.IsNullOrEmpty(dt.Rows[row]["MZ_CHISI"].ToString()))
            {
                result += "，" + dt.Rows[row]["MZ_CHISI_NAME"].ToString() + "(" + dt.Rows[row]["MZ_CHISI"].ToString() + ")";
            }

            result += "。";

            result = o_CommonService.d_report_break_line(result, 60, "&N");

            return result;
        }

        int TPM_FION=0;

        private string AFTER(DataTable dt, int row)
        {
            string result = "";

            if (!string.IsNullOrEmpty(dt.Rows[row]["MZ_AD"].ToString()))
            {
                result += dt.Rows[row]["MZ_AD_NAME"].ToString() + "(" + dt.Rows[row]["MZ_AD"].ToString() + ")";
            }

            if (!string.IsNullOrEmpty(dt.Rows[row]["MZ_UNIT"].ToString()))
            {
                result += "，" + dt.Rows[row]["MZ_UNIT_NAME"].ToString() + "(" + dt.Rows[row]["MZ_UNIT"].ToString() + ")";
            }

            if (!string.IsNullOrEmpty(dt.Rows[row]["MZ_PNO"].ToString()))
            {
                result += "，" + dt.Rows[row]["MZ_PNO"].ToString() + "(" + dt.Rows[row]["MZ_PNO1"].ToString() + ")";
            }

            if (!string.IsNullOrEmpty(dt.Rows[row]["MZ_OCCC"].ToString()))
            {
                result += "，" + dt.Rows[row]["MZ_OCCC_NAME"].ToString() + "(" + dt.Rows[row]["MZ_OCCC"].ToString() + ")";
            }

            if (!string.IsNullOrEmpty(dt.Rows[row]["MZ_RANK"].ToString()) && !string.IsNullOrEmpty(dt.Rows[row]["MZ_RANK1"].ToString()))
            {
                result += "，" + dt.Rows[row]["MZ_RANK_NAME"].ToString() + "至" + dt.Rows[row]["MZ_RANK1_NAME"].ToString() + "(" + dt.Rows[row]["MZ_RANK"].ToString() + "-" + dt.Rows[row]["MZ_RANK1"].ToString() + ")";
            }
            else if (!string.IsNullOrEmpty(dt.Rows[row]["MZ_RANK1"].ToString()))
            {
                result += "，" + dt.Rows[row]["MZ_RANK1_NAME"].ToString() + "(" + dt.Rows[row]["MZ_RANK1"].ToString() + ")";
            }

            if (!string.IsNullOrEmpty(dt.Rows[row]["MZ_POSIND"].ToString()))
            {
                result += "，" + dt.Rows[row]["MZ_POSIND"].ToString() + "'";
            }

            if (!string.IsNullOrEmpty(dt.Rows[row]["MZ_CHISI"].ToString()))
            {
                result += "，" + dt.Rows[row]["MZ_CHISI_NAME"].ToString() + "(" + dt.Rows[row]["MZ_CHISI"].ToString() + ")";
            }

            if (!string.IsNullOrEmpty(dt.Rows[row]["MZ_SRANK"].ToString()))
            {
                result += "，暫支" + dt.Rows[row]["MZ_SRANK_NAME"].ToString() + dt.Rows[row]["MZ_SLVC_NAME"].ToString() + dt.Rows[row]["MZ_SPT"].ToString() + "元";
            }

            result += "。";

            result = o_CommonService.d_report_break_line(result, 60, "&N");

            return result;
        }

        protected void TextBox_MZ_EXPLAIN_TextChanged(object sender, EventArgs e)
        {
            TextBox_MZ_EXPLAIN.Attributes.Add("onkeydown", "javascript:if(event.keyCode == 13)event.keyCode =13;");
        }

    }
}
