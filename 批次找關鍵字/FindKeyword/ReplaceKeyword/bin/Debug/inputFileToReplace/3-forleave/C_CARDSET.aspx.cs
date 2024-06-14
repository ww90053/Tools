using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;

namespace TPPDDB._3_forleave
{
    public partial class C_CARDSET : System.Web.UI.Page
    {
        List<String> CARDSET_ID = new List<string>();
        string C_strGID = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                C.check_power();
            }

            //查詢ID
            HttpCookie ForLeaveBasic_ID_Cookie = new HttpCookie("ForLeaveBasicSearch_ID");
            ForLeaveBasic_ID_Cookie = Request.Cookies["ForLeaveBasicSearch_ID"];

            if (ForLeaveBasic_ID_Cookie == null)
            {
                ViewState["MZ_ID"] = null;
                Response.Cookies["ForLeaveBasicSearch_ID"].Expires = DateTime.Now.AddYears(-1);
            }
            else
            {
                ViewState["MZ_ID"] = TPMPermissions._strDecod(ForLeaveBasic_ID_Cookie.Value.ToString());
                Response.Cookies["ForLeaveBasicSearch_ID"].Expires = DateTime.Now.AddYears(-1);
            }

            //查詢姓名
            HttpCookie ForLeaveBasic_NAME_Cookie = new HttpCookie("ForLeaveBasicSearch_NAME");
            ForLeaveBasic_NAME_Cookie = Request.Cookies["ForLeaveBasicSearch_NAME"];

            if (ForLeaveBasic_NAME_Cookie == null)
            {
                ViewState["MZ_NAME"] = null;
                Response.Cookies["ForLeaveBasicSearch_NAME"].Expires = DateTime.Now.AddYears(-1);
            }
            else
            {
                ViewState["MZ_NAME"] = TPMPermissions._strDecod(ForLeaveBasic_NAME_Cookie.Value.ToString());
                Response.Cookies["ForLeaveBasicSearch_NAME"].Expires = DateTime.Now.AddYears(-1);
            }

            ViewState["MZ_EXAD"] = Request["MZ_EXAD"];
            ViewState["MZ_EXUNIT"] = Request["MZ_EXUNIT"];
            ViewState["XCOUNT"] = Request["XCOUNT"];

           

            if (!IsPostBack)
            { 
                //by MQ 20100312---------   
            C.set_Panel_EnterToTAB(ref this.Panel1);
            
                C_strGID = o_a_Function.strGID(Session["ADPMZ_ID"].ToString());
                //by MQ 20100312---------
                C.controlEnable(ref this.Panel1, false);

                Label_MZ_AD.Text = "";
                Label_MZ_EXAD.Text = "";
                if (ViewState["MZ_ID"] != null)
                {

                    string strSQL = "SELECT MZ_ID FROM C_CARDSET WHERE 1=1";
                    string GVSQL = " SELECT MZ_ID,(SELECT MZ_NAME FROM A_DLBASE WHERE MZ_ID=C_CARDSET.MZ_ID) AS NAME,MZ_CLOCK,MZ_OPENDOOR,MZ_OVERTIME FROM C_CARDSET WHERE 1=1 ";

                    if (ViewState["MZ_ID"] != null && ViewState["MZ_ID"].ToString() != "")
                    {
                        strSQL += " AND MZ_ID='" + ViewState["MZ_ID"].ToString().Trim() + "'";
                        GVSQL += " AND MZ_ID='" + ViewState["MZ_ID"].ToString().Trim() + "'";
                    }

                    if (ViewState["MZ_NAME"] != null && ViewState["MZ_NAME"].ToString() != "")
                    {
                        strSQL += " AND MZ_ID IN (SELECT MZ_NAME FROM A_DLBASE WHERE MZ_NAME ='" + ViewState["MZ_NAME"].ToString().Trim() + "')";
                        GVSQL += " AND MZ_ID IN (SELECT MZ_NAME FROM A_DLBASE WHERE MZ_NAME ='" + ViewState["MZ_NAME"].ToString().Trim() + "')";
                    }

                    if (ViewState["MZ_EXAD"] != null && ViewState["MZ_EXAD"].ToString() != "")
                    {
                        strSQL += " AND MZ_ID IN(SELECT MZ_ID FROM A_DLBASE WHERE  MZ_EXAD='" + ViewState["MZ_EXAD"].ToString().Trim() + "')";
                        GVSQL += " AND MZ_ID IN(SELECT MZ_ID FROM A_DLBASE WHERE  MZ_EXAD='" + ViewState["MZ_EXAD"].ToString().Trim() + "')";
                    }

                    if (ViewState["MZ_EXUNIT"] != null && ViewState["MZ_EXUNIT"].ToString() != "")
                    {
                        strSQL += " AND MZ_ID IN (SELECT MZ_ID FROM A_DLBASE WHERE MZ_EXUNIT='" + ViewState["MZ_EXUNIT"].ToString().Trim() + "')";
                        GVSQL += " AND MZ_ID IN (SELECT MZ_ID FROM A_DLBASE WHERE MZ_EXUNIT='" + ViewState["MZ_EXUNIT"].ToString().Trim() + "')";
                    }

                    strSQL += " ORDER BY MZ_ID";
                    GVSQL += " ORDER BY MZ_ID";

                    CARDSET_ID = o_DBFactory.ABC_toTest.DataListArray(strSQL, "MZ_ID");
                    Session["CARDSET_ID"] = CARDSET_ID;
                    ViewState["CARD_SQL"] = strSQL;

                    //作 GRIDVIEW                  
                    DataTable gvDT = new DataTable();
                    gvDT = o_DBFactory.ABC_toTest.Create_Table(GVSQL, "GETVALUE");
                    Session["CARD_GVSQL"] = GVSQL;

                    if (CARDSET_ID.Count == 0)
                    {
                        Response.Write(@"<script language=javascript>window.alert('查無資料！');location.href('C_CARDSET.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "');</script>");
                    }
                    else if (CARDSET_ID.Count == 1)
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

                    GridView1.DataSource = gvDT;
                    GridView1.AllowPaging = true;
                    GridView1.PageSize = 10;
                    GridView1.DataBind();
                }
            }
        }

        protected void preLoad(string MZ_ID, int Mode)
        {

            TextBox_MZ_ID.Text = MZ_ID;

            //如果有建立人事代碼對應的View .這段可以修

            string strsql = @"SELECT MZ_NAME ,MZ_AD_CH,MZ_UNIT_CH,MZ_EXAD_CH, MZ_EXUNIT_CH,  MZ_OCCC_CH
                              FROM VW_A_DLBASE_S1  
                              
                              WHERE MZ_ID='" + MZ_ID + "'";

            DataTable person_data = o_DBFactory.ABC_toTest.Create_Table(strsql, "get");

            if (person_data.Rows.Count > 0)
            {
                TextBox_MZ_NAME.Text = person_data.Rows[0]["MZ_NAME"].ToString();

                Label_MZ_AD.Text = person_data.Rows[0]["MZ_AD_CH"].ToString() + "     " + person_data.Rows[0]["MZ_UNIT_CH"].ToString();

                Label_MZ_EXAD.Text = person_data.Rows[0]["MZ_EXAD_CH"].ToString() + "     " + person_data.Rows[0]["MZ_EXUNIT_CH"].ToString() + "     " + person_data.Rows[0]["MZ_OCCC_CH"].ToString();
            }


        }

        protected void finddata(int dataCount)
        {
            CARDSET_ID = Session["CARDSET_ID"] as List<string>;

            string selectString = "SELECT * FROM C_CARDSET WHERE MZ_ID='" + CARDSET_ID[dataCount] + "'";

            DataTable temp = new DataTable();

            temp = o_DBFactory.ABC_toTest.Create_Table(selectString, "GET");

            if (temp.Rows.Count == 1)
            {
                TextBox_MZ_ID.Text = temp.Rows[0]["MZ_ID"].ToString();

                TextBox_MZ_NAME.Text = o_A_DLBASE.CNAME(temp.Rows[0]["MZ_ID"].ToString());

                if (temp.Rows[0]["MZ_CLOCK"].ToString() == "Y")
                {
                    CheckBox_MZ_CLOCK.Checked = true;
                }
                else
                {
                    CheckBox_MZ_CLOCK.Checked = false;
                }

                if (temp.Rows[0]["MZ_OPENDOOR"].ToString() == "Y")
                {
                    CheckBox_MZ_OPENDOOR.Checked = true;
                }
                else
                {
                    CheckBox_MZ_OPENDOOR.Checked = false;
                }

                if (temp.Rows[0]["MZ_OVERTIME"].ToString() == "Y")
                {
                    CheckBox_MZ_OVERTIME.Checked = true;
                }
                else
                {
                    CheckBox_MZ_OVERTIME.Checked = false;
                }
            }

            preLoad(TextBox_MZ_ID.Text.Trim(), 2);

            btUpdate.Enabled = true;
            btDelete.Enabled = true;

            //by MQ 20100311
            //moveGV_IndexMark();
            Label1.Text = "第" + (int.Parse(xcount.Text) + 1).ToString() + "筆共" + CARDSET_ID.Count.ToString() + "筆";
        }

        protected void btInsert_Click(object sender, EventArgs e)
        {
            ViewState["MODE"] = "INSERT";

            ViewState["CMDSQL"] = "INSERT INTO C_CARDSET (MZ_ID,MZ_CLOCK,MZ_OPENDOOR,MZ_OVERTIME) VALUES (@MZ_ID,@MZ_CLOCK,@MZ_OPENDOOR,@MZ_OVERTIME) ";
            //清空
            emptyMonitor(ref this.Panel1);

            btNEXT.Enabled = false;
            btUpper.Enabled = false;
            btInsert.Enabled = false;
            btUpdate.Enabled = false;
            btCancel.Enabled = true;
            btOK.Enabled = true;

            //by MQ 20100312---------
            C.controlEnable(ref this.Panel1, true);
        }

        private void emptyMonitor(ref Panel Pl)
        {
            foreach (Object ob in Pl.Controls)
            {
                if (ob is TextBox)
                {
                    TextBox tbox = (TextBox)ob;
                    tbox.Text = string.Empty;
                }
                else if (ob is DropDownList)
                {
                    DropDownList ddlist = (DropDownList)ob;

                    ddlist.SelectedIndex = -1;
                }
                else if (ob is CheckBox)
                {
                    CheckBox cb = (CheckBox)ob;
                    cb.Checked = false;
                }
                else if (ob is Label)
                {
                    Label cb = (Label)ob;
                    cb.Text = string.Empty;

                }
            }
        }

        protected void btUpdate_Click(object sender, EventArgs e)
        {
            ViewState["MODE"] = "UPDATE";
            ViewState["CMDSQL"] = "UPDATE C_CARDSET SET MZ_ID = @MZ_ID,MZ_CLOCK = @MZ_CLOCK,MZ_OPENDOOR = @MZ_OPENDOOR,MZ_OVERTIME = @MZ_OVERTIME WHERE MZ_ID='" + o_str.tosql(TextBox_MZ_ID.Text.Trim()) + "'";

            HttpCookie Cookie1 = new HttpCookie("PKEY_MZ_ID");
            Cookie1.Value = TextBox_MZ_ID.Text;
            Response.Cookies.Add(Cookie1);

            btNEXT.Enabled = false;
            btUpper.Enabled = false;
            btDelete.Enabled = true;
            btInsert.Enabled = false;
            btUpdate.Enabled = false;
            btCancel.Enabled = true;
            btOK.Enabled = true;

            //by MQ 20100312---------
            C.controlEnable(ref this.Panel1, true);
        }

        protected void btOK_Click(object sender, EventArgs e)
        {
            //2010.06.07 by 伊珊
            string ErrorString = "";

            string old_ID = "NULL";

            if (ViewState["MODE"].ToString() == "UPDATE")
            {
                HttpCookie Cookie1 = new HttpCookie("PKEY_MZ_ID");
                Cookie1 = Request.Cookies["PKEY_MZ_ID"];
                old_ID = Cookie1.Value.ToString();
            }

            string pkey_check;

            if (old_ID == TextBox_MZ_ID.Text && ViewState["MODE"].ToString() == "UPDATE")
                pkey_check = "0";
            else 
                pkey_check = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM C_CARDSET WHERE MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "'");

            if (pkey_check != "0")
            {
                ErrorString += "身分證號違反唯一值的條件" + "\\r\\n";
                TextBox_MZ_ID.BackColor = Color.Orange;
            }
            else
            {
                TextBox_MZ_ID.BackColor = Color.White;
            }
            //20141216 這應該是想做控制項檢核.但 DBDIC 又沒有資料
            //foreach (Object ob in Panel1.Controls)
            //{
            //    if (ob is TextBox)
            //    {
            //        TextBox tbox = (TextBox)ob;

            //        string result = o_Check_Dbdic.ErrorString(tbox.ID, "C_CARDSET", tbox.Text);

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

            SqlParameter[] parameterList = {
            new SqlParameter("MZ_ID",SqlDbType.VarChar){Value = TextBox_MZ_ID.Text},
            new SqlParameter("MZ_CLOCK",SqlDbType.VarChar){Value = CheckBox_MZ_CLOCK.Checked?"Y":"N"},
            new SqlParameter("MZ_OPENDOOR",SqlDbType.VarChar){Value = CheckBox_MZ_OPENDOOR.Checked?"Y":"N"},
            new SqlParameter("MZ_OVERTIME",SqlDbType.VarChar){Value = CheckBox_MZ_OVERTIME.Checked?"Y":"N"}
            };

            try
            {
                o_DBFactory.ABC_toTest.ExecuteNonQuery( ViewState["CMDSQL"].ToString(), parameterList);

                Response.Cookies["PKEY_MZ_ID"].Expires = DateTime.Now.AddYears(-1);

                if (ViewState["MODE"].ToString() == "INSERT")
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('新增成功');", true);

                    //2010.06.04 LOG紀錄 by伊珊
                    TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00002", TPMPermissions._boolTPM(), o_DBFactory.ABC_toTest.RegexSQL(ViewState["CMDSQL"].ToString(), parameterList));

                    CARDSET_ID = o_DBFactory.ABC_toTest.DataListArray(ViewState["CARD_SQL"].ToString(), "MZ_ID");
                    Session["CARDSET_ID"] = CARDSET_ID;

                    xcount.Text = "0";
                    finddata(int.Parse(xcount.Text));
                    btUpper.Enabled = false;

                    if (CARDSET_ID.Count > 1)
                    {
                        btNEXT.Enabled = true;
                    }
                    else
                    {
                        btNEXT.Enabled = false;
                    }
                }
                else if (ViewState["MODE"].ToString() == "UPDATE")
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('修改成功');", true);

                    //2010.06.04 LOG紀錄 by伊珊
                    TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00003", TPMPermissions._boolTPM(), o_DBFactory.ABC_toTest.RegexSQL(ViewState["CMDSQL"].ToString(), parameterList));
                    CARDSET_ID = Session["CARDSET_ID"] as List<string>;

                    if (int.Parse(xcount.Text.Trim()) == 0 && CARDSET_ID.Count == 1)
                    {
                        btUpper.Enabled = false;
                        btNEXT.Enabled = false;
                    }
                    else if (int.Parse(xcount.Text.Trim()) == 0 && CARDSET_ID.Count > 1)
                    {
                        btUpper.Enabled = false;
                        btNEXT.Enabled = true;
                    }
                    else if (int.Parse(xcount.Text.Trim()) + 1 == CARDSET_ID.Count)
                    {
                        btUpper.Enabled = true;
                        btNEXT.Enabled = false;
                    }
                    else if (int.Parse(xcount.Text.Trim()) > 0 && int.Parse(xcount.Text.Trim()) + 1 < CARDSET_ID.Count)
                    {
                        btNEXT.Enabled = true;
                        btUpper.Enabled = true;
                    }
                }

                //by MQ 20100312---------
                DataTable PAGE_DT = new DataTable();
                PAGE_DT = o_DBFactory.ABC_toTest.Create_Table(Session["CARD_GVSQL"].ToString(), "GETVALUE");
                GridView1.DataSource = PAGE_DT;
                GridView1.DataBind();

                btUpdate.Enabled = true;
                btInsert.Enabled = true;
                btOK.Enabled = false;
                btCancel.Enabled = false;
                btDelete.Enabled = true;
                ViewState.Remove("MODE");
                //by MQ 20100312---------
                C.controlEnable(ref this.Panel1, false);
            }
            catch
            {
                if (ViewState["MODE"].ToString() == "INSERT")
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('新增失敗');", true);
                }
                else if (ViewState["MODE"].ToString() == "UPDATE")
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('修改失敗');", true);
                }
            }
        }

        protected void btSearch_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('C_ForLeaveBasicSearch1.aspx?TableName=CARDSET&TPM_FION=" + Request.QueryString["TPM_FION"] + "','查詢','top=190,left=200,width=500,height=250,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            TextBox_MZ_ID.Text = string.Empty;
            TextBox_MZ_NAME.Text = string.Empty;
            CheckBox_MZ_CLOCK.Checked = false;
            CheckBox_MZ_OPENDOOR.Checked = false;
            CheckBox_MZ_OVERTIME.Checked = false;

            CARDSET_ID = Session["CARDSET_ID"] as List<string>;

            if (CARDSET_ID != null && CARDSET_ID.Count != 0)
            {
                finddata(int.Parse(xcount.Text.Trim()));

                if (int.Parse(xcount.Text.Trim()) == 0 && CARDSET_ID.Count == 1)
                {
                    btUpper.Enabled = false;
                    btNEXT.Enabled = false;
                }
                else if (int.Parse(xcount.Text.Trim()) == 0 && CARDSET_ID.Count > 1)
                {
                    btUpper.Enabled = false;
                    btNEXT.Enabled = true;
                }
                else if (int.Parse(xcount.Text.Trim()) + 1 == CARDSET_ID.Count)
                {
                    btUpper.Enabled = true;
                    btNEXT.Enabled = false;
                }
                else if (int.Parse(xcount.Text.Trim()) > 0 && int.Parse(xcount.Text.Trim()) + 1 < CARDSET_ID.Count)
                {
                    btNEXT.Enabled = true;
                    btUpper.Enabled = true;
                }
            }

            btInsert.Enabled = true;
            btOK.Enabled = false;
            btCancel.Enabled = false;

            //by MQ 20100312---------
            C.controlEnable(ref this.Panel1, false);

        }

        protected void btDelete_Click(object sender, EventArgs e)
        {
            string DeleteString = "DELETE FROM C_CARDSET WHERE MZ_ID='" + o_str.tosql(TextBox_MZ_ID.Text.Trim()) + "'";

            try
            {
                o_DBFactory.ABC_toTest.Edit_Data(DeleteString);

                CARDSET_ID = Session["CARDSET_ID"] as List<string>;
                CARDSET_ID.RemoveAt(int.Parse(xcount.Text));

                if (CARDSET_ID.Count == 0)
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除成功');location.href('C_CARDSET.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "');", true);
                    //2010.06.04 LOG紀錄 by伊珊
                    TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00004", TPMPermissions._boolTPM(), DeleteString);
                }
                else
                {
                    xcount.Text = "0";
                    finddata(int.Parse(xcount.Text));
                    btUpper.Enabled = false;

                    if (CARDSET_ID.Count > 1)
                    {
                        btNEXT.Enabled = true;
                    }
                    else
                    {
                        btNEXT.Enabled = false;
                    }

                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除成功')", true);
                    //2010.06.04 LOG紀錄 by伊珊
                    TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00004", TPMPermissions._boolTPM(), DeleteString);

                    DataTable PAGE_DT = new DataTable();
                    PAGE_DT = o_DBFactory.ABC_toTest.Create_Table(Session["CARD_GVSQL"].ToString(), "GETVALUE");
                    GridView1.DataSource = PAGE_DT;
                    GridView1.DataBind();
                }
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除失敗');", true);
            }

        }

        protected void btUpper_Click(object sender, EventArgs e)
        {
            if (int.Parse(xcount.Text) != 0)
            {
                xcount.Text = (Convert.ToInt32(xcount.Text) - 1).ToString();
                finddata(int.Parse(xcount.Text));
                if (int.Parse(xcount.Text) != CARDSET_ID.Count - 1)
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
            int n = int.Parse(xcount.Text);

           
            if (n < (GridView1.PageIndex) * GridView1.PageSize)
            {
                DataTable gvDT = new DataTable();

                string GVSQL = Session["CARD_GVSQL"].ToString();

                gvDT = o_DBFactory.ABC_toTest.Create_Table(GVSQL, "GETVALUE1");

                GridView1.DataSource = gvDT;
                GridView1.PageIndex = GridView1.PageIndex - 1;
                GridView1.DataBind();
                finddata(int.Parse(xcount.Text));
                GridView1.SelectedIndex = n % GridView1.PageSize;
            }
            else
            {
                GridView1.SelectedIndex = n % GridView1.PageSize;
            }

    }

        protected void btNEXT_Click(object sender, EventArgs e)
        {
            if (int.Parse(xcount.Text) == 0)
            {
                xcount.Text = (Convert.ToInt32(xcount.Text) + 1).ToString();
                btUpper.Enabled = true;
                finddata(int.Parse(xcount.Text));

                if (int.Parse(xcount.Text) == CARDSET_ID.Count - 1)
                {
                    btNEXT.Enabled = false;
                }
            }
            else
            {
                xcount.Text = (Convert.ToInt32(xcount.Text) + 1).ToString();
                finddata(int.Parse(xcount.Text));

                if (int.Parse(xcount.Text) == CARDSET_ID.Count - 1)
                {

                    btNEXT.Enabled = false;
                }
            }
            int n = int.Parse(xcount.Text);

            
            if (n >= (GridView1.PageIndex + 1) * GridView1.PageSize)
            {
                DataTable gvDT = new DataTable();

                string GVSQL = Session["CARD_GVSQL"].ToString();

                gvDT = o_DBFactory.ABC_toTest.Create_Table(GVSQL, "GETVALUE1");

                GridView1.DataSource = gvDT;
                GridView1.PageIndex = GridView1.PageIndex + 1;
                GridView1.DataBind();
                finddata(int.Parse(xcount.Text));
                GridView1.SelectedIndex = n % GridView1.PageSize;
            }
            else
            {
                GridView1.SelectedIndex = n % GridView1.PageSize;
            }



        }

        protected void TextBox_MZ_ID_TextChanged(object sender, EventArgs e)
        {
            TextBox_MZ_ID.Text = TextBox_MZ_ID.Text.Trim().ToUpper();

            preLoad(TextBox_MZ_ID.Text, 1);
        }

        protected void btGroup_Update_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('C_CARDSET_GROUPUPDATE.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin','top=190,left=200,width=550,height=300,toolbar=0,menubar=0,location=0,directories=0,status=0,scrollbar=yes');", true);
        }

        protected void CheckBox_GV_CLOCK_CheckedChanged(object sender, EventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((CheckBox)sender).NamingContainer;
            GridView gv = (GridView)gvr.NamingContainer;
            int index = gvr.RowIndex;

            gv.SelectedIndex = index;

            CheckBox GV_CLOCK = gv.Rows[index].Cells[2].Controls[1] as CheckBox;

            if (GV_CLOCK.Checked)
            {
                string UpdateSQL = "UPDATE C_CARDSET SET MZ_CLOCK='Y' WHERE MZ_ID='" + gv.Rows[index].Cells[0].Text + "'";

                o_DBFactory.ABC_toTest.Edit_Data(UpdateSQL);
            }
            else
            {
                string UpdateSQL = "UPDATE C_CARDSET SET MZ_CLOCK='N' WHERE MZ_ID='" + gv.Rows[index].Cells[0].Text + "'";
                o_DBFactory.ABC_toTest.Edit_Data(UpdateSQL);
            }

            //by MQ 20100311---------
            int a = (GridView1.PageIndex) * (GridView1.PageSize) + index;
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
            if (int.Parse(xcount.Text) == CARDSET_ID.Count - 1)
            {
                btNEXT.Enabled = false;
            }
            else
            {
                btNEXT.Enabled = true;
            }

            DataTable gvDT = new DataTable();

            string GVSQL = Session["CARD_GVSQL"].ToString();

            gvDT = o_DBFactory.ABC_toTest.Create_Table(GVSQL, "GETVALUE1");

            GridView1.DataSource = gvDT;

            GridView1.DataBind();
        }

        protected void CheckBox_GV_OPENDOOR_CheckedChanged(object sender, EventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((CheckBox)sender).NamingContainer;
            GridView gv = (GridView)gvr.NamingContainer;
            int index = gvr.RowIndex;

            gv.SelectedIndex = index;

            CheckBox GV_OPENDOOR = gv.Rows[index].Cells[3].Controls[1] as CheckBox;

            if (GV_OPENDOOR.Checked)
            {
                string UpdateSQL = "UPDATE C_CARDSET SET MZ_OPENDOOR='Y' WHERE MZ_ID='" + gv.Rows[index].Cells[0].Text + "'";

                o_DBFactory.ABC_toTest.Edit_Data(UpdateSQL);
            }
            else
            {
                string UpdateSQL = "UPDATE C_CARDSET SET MZ_OPENDOOR='N' WHERE MZ_ID='" + gv.Rows[index].Cells[0].Text + "'";
                o_DBFactory.ABC_toTest.Edit_Data(UpdateSQL);
            }
            //by MQ 20100311---------
            int a = (GridView1.PageIndex) * (GridView1.PageSize) + index;
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
            if (int.Parse(xcount.Text) == CARDSET_ID.Count - 1)
            {
                btNEXT.Enabled = false;
            }
            else
            {
                btNEXT.Enabled = true;
            }

            DataTable gvDT = new DataTable();

            string GVSQL = Session["CARD_GVSQL"].ToString();

            gvDT = o_DBFactory.ABC_toTest.Create_Table(GVSQL, "GETVALUE1");

            GridView1.DataSource = gvDT;

            GridView1.DataBind();
        }

        protected void CheckBox_GV_OVERTIME_CheckedChanged(object sender, EventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((CheckBox)sender).NamingContainer;
            GridView gv = (GridView)gvr.NamingContainer;
            int index = gvr.RowIndex;

            gv.SelectedIndex = index;

            CheckBox GV_OVERTIME = gv.Rows[index].Cells[4].Controls[1] as CheckBox;

            if (GV_OVERTIME.Checked)
            {
                string UpdateSQL = "UPDATE C_CARDSET SET MZ_OVERTIME='Y' WHERE MZ_ID='" + gv.Rows[index].Cells[0].Text + "'";

                o_DBFactory.ABC_toTest.Edit_Data(UpdateSQL);
            }
            else
            {
                string UpdateSQL = "UPDATE C_CARDSET SET MZ_OVERTIME='N' WHERE MZ_ID='" + gv.Rows[index].Cells[0].Text + "'";
                o_DBFactory.ABC_toTest.Edit_Data(UpdateSQL);
            }
            //by MQ 20100311---------
            int a = (GridView1.PageIndex) * (GridView1.PageSize) + index;
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
            if (int.Parse(xcount.Text) == CARDSET_ID.Count - 1)
            {
                btNEXT.Enabled = false;
            }
            else
            {
                btNEXT.Enabled = true;
            }

            DataTable gvDT = new DataTable();
            gvDT = o_DBFactory.ABC_toTest.Create_Table(Session["CARD_GVSQL"].ToString(), "GETVALUE1");

            GridView1.DataSource = gvDT;
            GridView1.DataBind();
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataTable gvDT = new DataTable();

            string GVSQL = Session["CARD_GVSQL"].ToString();

            gvDT = o_DBFactory.ABC_toTest.Create_Table(GVSQL, "GETVALUE1");

            GridView1.DataSource = gvDT;
            GridView1.PageIndex = e.NewPageIndex;
            GridView1.DataBind();
            xcount.Text = (GridView1.PageSize * GridView1.PageIndex).ToString();
            finddata(int.Parse(xcount.Text));
            GridView1.SelectedIndex = 0;

        }

        protected void moveGV_IndexMark()
        {
            //by MQ 20100312

            int n = int.Parse(xcount.Text);

            DataTable PAGE_DT = new DataTable();
            PAGE_DT = o_DBFactory.ABC_toTest.Create_Table(Session["CARD_GVSQL"].ToString(), "GETVALUE");

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

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GridView1, "SELECT$" + e.Row.RowIndex);
                e.Row.Cells[GridView1.Columns.Count - 1].Attributes.Add("Style", "display:none");
            }
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "SELECT")
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
                if (int.Parse(xcount.Text) == CARDSET_ID.Count - 1)
                {
                    btNEXT.Enabled = false;
                }
                else
                {
                    btNEXT.Enabled = true;
                }
                btUpdate.Enabled = true;
                btDelete.Enabled = true;
                btCancel.Enabled = true;
            }

        }
    }
}
