using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TPPDDB.Helpers;

namespace TPPDDB._1_personnel
{
    public partial class Personal_Card : System.Web.UI.Page
    {
        List<String> Personal_Card_SN = new List<string>();
       

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                A.check_power();
            }
           

            ViewState["MZ_AD"] = Request["MZ_AD"];
            ViewState["MZ_UNIT"] = Request["MZ_UNIT"];
            ViewState["MZ_OCCC"] = Request["MZ_OCCC"];
            ViewState["MZ_IDNO"] = Request["MZ_IDNO"];
            ViewState["PRINT"] = Request["PRINT"];
            ViewState["MZ_ID1"] = Session["PersonalSearchIDwithNAME_MZ_ID4"];
            Session.Remove("PersonalSearchIDwithNAME_MZ_ID4");
            ViewState["MZ_NAME"] = Session["Personal_Card_NAME"];
            Session.Remove("Personal_Card_NAME");

            A.set_Panel_EnterToTAB(ref this.Panel_Card);
            A.set_Panel_EnterToTAB(ref this.Panel1);

            ////查詢ID
            //HttpCookie DLBASE_ID_Cookie = new HttpCookie("PersonalSearch_ID");
            //DLBASE_ID_Cookie = Request.Cookies["PersonalSearch_ID"];

            //if (DLBASE_ID_Cookie == null)
            //{
            //    ViewState["MZ_ID"] = null;
            //    Response.Cookies["PersonalSearch_ID"].Expires = DateTime.Now.AddYears(-1);
            //}
            //else
            //{
            //    ViewState["MZ_ID"] = TPMPermissions._strDecod(DLBASE_ID_Cookie.Value.ToString());
            //    Response.Cookies["PersonalSearch_ID"].Expires = DateTime.Now.AddYears(-1);
            //}

            //查詢姓名
            //HttpCookie CARD_NAME_Cookie = new HttpCookie("Personal_Card_NAME");
            //CARD_NAME_Cookie = Request.Cookies["Personal_Card_NAME"];

            //if (CARD_NAME_Cookie == null)
            //{
            //    ViewState["MZ_NAME"] = null;
            //    Response.Cookies["Personal_Card_NAME"].Expires = DateTime.Now.AddYears(-1);
            //}
            //else
            //{
            //    ViewState["MZ_NAME"] = TPMPermissions._strDecod(CARD_NAME_Cookie.Value.ToString());
            //    Response.Cookies["Personal_Card_NAME"].Expires = DateTime.Now.AddYears(-1);
            //}

            //HttpCookie PRKB_MZ_ID_Cookie = new HttpCookie("PersonalSearchIDwithNAME_MZ_ID4");
            //PRKB_MZ_ID_Cookie = Request.Cookies["PersonalSearchIDwithNAME_MZ_ID4"];

            //if (PRKB_MZ_ID_Cookie == null)
            //{
            //    ViewState["MZ_ID1"] = null;
            //    Response.Cookies["PersonalSearchIDwithNAME_MZ_ID4"].Expires = DateTime.Now.AddYears(-1);
            //}
            //else
            //{
            //    ViewState["MZ_ID1"] = TPMPermissions._strDecod(PRKB_MZ_ID_Cookie.Value.ToString());
            //    Response.Cookies["PersonalSearchIDwithNAME_MZ_ID4"].Expires = DateTime.Now.AddYears(-1);
            //}

            if (!IsPostBack)
            {
                bool Change = false;

                TextBox_MZ_AD1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_EXAD1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_EXUNIT1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_OCCC1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_RANK_1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_RANK1_1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_UNIT1.Attributes.Add("onkeydown", "javascript:return false;");

                if (ViewState["XCOUNT"] != null)
                {
                    finddata(int.Parse(ViewState["XCOUNT"].ToString()));
                    xcount.Text = ViewState["XCOUNT"].ToString();
                    if (int.Parse(ViewState["XCOUNT"].ToString()) == 0)
                    {
                        btNEXT.Enabled = true;
                        btUpper.Enabled = false;
                    }
                    else if (int.Parse(ViewState["XCOUNT"].ToString()) > 0 && int.Parse(ViewState["XCOUNT"].ToString()) < Personal_Card_SN.Count - 1)
                    {
                        btNEXT.Enabled = true;
                        btUpper.Enabled = true;
                    }
                    else if (int.Parse(ViewState["XCOUNT"].ToString()) == Personal_Card_SN.Count - 1)
                    {
                        btUpper.Enabled = true;
                        btNEXT.Enabled = false;
                    }
                    else
                    {
                        btNEXT.Enabled = false;
                        btUpper.Enabled = false;
                    }

                    if (Personal_Card_SN.Count == 0)
                    {
                        Label1.Visible = false;
                    }
                    else
                    {
                        Label1.Visible = true;
                        Label1.Text = "第" + (int.Parse(xcount.Text) + 1).ToString() + "筆共" + Personal_Card_SN.Count.ToString() + "筆";
                    }
                    btUpdate.Enabled = true;
                    btInsert.Enabled = true;
                    btOK.Enabled = false;
                    btCancel.Enabled = false;
                    Button2.Enabled = false;
                    btDelete.Enabled = true;
                    Change = false;
                }

                if (ViewState["MZ_AD"] != null)
                {
                    string strSQL = "SELECT A_POLICE.*,(SELECT dbo.to_number(CASE WHEN MZ_TBDV IS NULL THEN '999' WHEN MZ_TBDV='Z99' THEN '999' ELSE MZ_TBDV END) FROM A_DLBASE WHERE MZ_ID=A_POLICE.MZ_ID) AS TBDV,(SELECT MZ_EXUNIT FROM A_DLBASE WHERE MZ_ID=A_POLICE.MZ_ID) AS UNIT FROM A_POLICE WHERE 1=1";

                    if (ViewState["MZ_IDNO"].ToString() != "")
                    {
                        strSQL += " AND MZ_IDNO='" + ViewState["MZ_IDNO"].ToString() + "'";
                    }

                    if (ViewState["MZ_OCCC"].ToString() != "")
                    {
                        strSQL += " AND MZ_ID IN (SELECT MZ_ID FROM A_DLBASE WHERE MZ_STATUS2='Y' AND MZ_OCCC= '" + ViewState["MZ_OCCC"].ToString() + "')";
                    }

                    if (ViewState["MZ_AD"].ToString() != "")
                    {
                        strSQL += " AND MZ_ID IN (SELECT MZ_ID FROM A_DLBASE WHERE MZ_STATUS2='Y' AND MZ_EXAD= '" + ViewState["MZ_AD"].ToString() + "')";
                    }

                    if (ViewState["MZ_UNIT"].ToString() != "")
                    {
                        strSQL += " AND MZ_ID IN (SELECT MZ_ID FROM A_DLBASE WHERE MZ_STATUS2='Y' AND MZ_EXUNIT= '" + ViewState["MZ_UNIT"].ToString() + "')";
                    }

                    if (ViewState["MZ_NAME"].ToString() != "")
                    {
                        strSQL += " AND RTRIM(MZ_NAME) ='" + ViewState["MZ_NAME"].ToString() + "'";
                    }

                    if (ViewState["PRINT"].ToString() != "")
                    {
                        if (ViewState["PRINT"].ToString() == "N")
                            strSQL += " AND (MZ_MEMO1='N' OR MZ_MEMO1 IS NULL) ";
                        else
                            strSQL += " AND MZ_MEMO1='" + ViewState["PRINT"].ToString() + "'";
                    }

                    strSQL = strSQL + " ORDER BY TBDV,UNIT";

                    ViewState["strsql"] = strSQL;

                    Personal_Card_SN = o_DBFactory.ABC_toTest.DataListArray(strSQL, "SN");

                    Session["Personal_Card_SN"] = Personal_Card_SN;

                    DataTable dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "card");

                    Session["Personal_Card_GV_DT"] = dt;

                    GridView1.DataSource = dt;
                    GridView1.AllowPaging = true;
                    GridView1.PageSize = 4;
                    GridView1.DataBind();

                    if (Personal_Card_SN.Count == 0)
                    {
                        Response.Write(@"<script language=javascript>window.alert('查無資料！');location.href('Personal_Card.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "');</script>");
                    }
                    else if (Personal_Card_SN.Count == 1)
                    {
                        xcount.Text = "0";
                        finddata(int.Parse(xcount.Text));
                        GridView1.SelectedIndex = 0;
                    }
                    else
                    {
                        btNEXT.Enabled = true;
                        xcount.Text = "0";
                        finddata(int.Parse(xcount.Text));
                        GridView1.SelectedIndex = 0;
                    }

                    if (Personal_Card_SN.Count == 0)
                    {
                        Label1.Visible = false;
                    }
                    else
                    {
                        Label1.Visible = true;
                        Label1.Text = "第" + (int.Parse(xcount.Text) + 1).ToString() + "筆共" + Personal_Card_SN.Count.ToString() + "筆";
                    }
                    Change = false;
                }

                if (ViewState["MZ_ID1"] != null)
                {
                    preLoad(ViewState["MZ_ID1"].ToString());
                    Change = true;
                }

                A.controlEnable(ref this.Panel_Card, Change);
            }
        }
       

        protected void findPic()
        {
            string picPath = "SELECT PICTUREPATH FROM (SELECT PICTUREPATH FROM A_PICPATH WHERE MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "' ORDER BY BUDATE DESC) WHERE ROWNUM<=1 ";
            DataTable picPathTable = o_DBFactory.ABC_toTest.Create_Table(picPath, "picPathTable");

            if (picPathTable.Rows.Count == 0)
            {
                Image1.ImageUrl = "~/1-personnel/images/nopic.jpg";
            }
            else
            {
                Image1.ImageUrl = picPathTable.Rows[0]["PICTUREPATH"].ToString();
            }

        }

        protected void finddata(int dataCount)
        {
            Personal_Card_SN = Session["Personal_Card_SN"] as List<string>;

            string strSQL = "SELECT * FROM A_POLICE WHERE SN=" + Personal_Card_SN[dataCount] + "";

            DataTable dt = new DataTable();

            dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");

            if (dt.Rows.Count == 1)
            {
                TextBox_MZ_AD.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_AD FROM A_DLBASE WHERE MZ_ID = '" + dt.Rows[0]["MZ_ID"].ToString() + "'");
                if (dt.Rows[0]["MZ_DATE"].ToString().Length == 7)
                    TextBox_MZ_DATE1.Text = (dt.Rows[0]["MZ_DATE"].ToString().Substring(0, 1) == "0" ? dt.Rows[0]["MZ_DATE"].ToString().Substring(1, 2) + "/" + dt.Rows[0]["MZ_DATE"].ToString().Substring(3, 2) + "/" + dt.Rows[0]["MZ_DATE"].ToString().Substring(5, 2) : dt.Rows[0]["MZ_DATE"].ToString().Substring(0, 3) + "/" + dt.Rows[0]["MZ_DATE"].ToString().Substring(3, 2) + "/" + dt.Rows[0]["MZ_DATE"].ToString().Substring(5, 2));
                else
                    TextBox_MZ_DATE1.Text = dt.Rows[0]["MZ_DATE"].ToString();
                if (dt.Rows[0]["MZ_EDATE"].ToString().Length == 7)
                    TextBox_MZ_EDATE.Text = (dt.Rows[0]["MZ_EDATE"].ToString().Substring(0, 1) == "0" ? dt.Rows[0]["MZ_EDATE"].ToString().Substring(1, 2) + "/" + dt.Rows[0]["MZ_EDATE"].ToString().Substring(3, 2) + "/" + dt.Rows[0]["MZ_EDATE"].ToString().Substring(5, 2) : dt.Rows[0]["MZ_EDATE"].ToString().Substring(0, 3) + "/" + dt.Rows[0]["MZ_EDATE"].ToString().Substring(3, 2) + "/" + dt.Rows[0]["MZ_EDATE"].ToString().Substring(5, 2));
                else
                    TextBox_MZ_EDATE.Text = dt.Rows[0]["MZ_EDATE"].ToString();
                TextBox_MZ_EXAD.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_EXAD FROM A_DLBASE WHERE MZ_ID = '" + dt.Rows[0]["MZ_ID"].ToString() + "'");
                TextBox_MZ_EXUNIT.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_EXUNIT FROM A_DLBASE WHERE MZ_ID = '" + dt.Rows[0]["MZ_ID"].ToString() + "'");
                TextBox_MZ_ID.Text = dt.Rows[0]["MZ_ID"].ToString();
                TextBox_MZ_IDNO.Text = dt.Rows[0]["MZ_IDNO"].ToString();
                TextBox_MZ_MEMO.Text = dt.Rows[0]["MZ_MEMO"].ToString();
                DropDownList_MZ_MEMO1.SelectedValue = dt.Rows[0]["MZ_MEMO1"].ToString();
                TextBox_MZ_NAME.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_NAME FROM A_DLBASE WHERE MZ_ID = '" + dt.Rows[0]["MZ_ID"].ToString() + "'");
                TextBox_MZ_NO1.Text = dt.Rows[0]["MZ_NO1"].ToString();
                TextBox_MZ_OCCC.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_OCCC FROM A_DLBASE WHERE MZ_ID = '" + dt.Rows[0]["MZ_ID"].ToString() + "'");
                if (dt.Rows[0]["MZ_ODATE"].ToString().Length == 7)
                    TextBox_MZ_ODATE.Text = (dt.Rows[0]["MZ_ODATE"].ToString().Substring(0, 1) == "0" ? dt.Rows[0]["MZ_ODATE"].ToString().Substring(1, 2) + "/" + dt.Rows[0]["MZ_ODATE"].ToString().Substring(3, 2) + "/" + dt.Rows[0]["MZ_ODATE"].ToString().Substring(5, 2) : dt.Rows[0]["MZ_ODATE"].ToString().Substring(0, 3) + "/" + dt.Rows[0]["MZ_ODATE"].ToString().Substring(3, 2) + "/" + dt.Rows[0]["MZ_ODATE"].ToString().Substring(5, 2));
                else
                    TextBox_MZ_ODATE.Text = dt.Rows[0]["MZ_ODATE"].ToString();
                DropDownList_MZ_INO.SelectedValue = dt.Rows[0]["MZ_INO"].ToString();
                DropDownList_MZ_SWT.SelectedValue = dt.Rows[0]["MZ_SWT"].ToString();
                TextBox_MZ_RANK.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_RANK FROM A_DLBASE WHERE MZ_ID = '" + dt.Rows[0]["MZ_ID"].ToString() + "'");
                TextBox_MZ_RANK1.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_RANK1 FROM A_DLBASE WHERE MZ_ID = '" + dt.Rows[0]["MZ_ID"].ToString() + "'");
                TextBox_MZ_UNIT.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_UNIT FROM A_DLBASE WHERE MZ_ID = '" + dt.Rows[0]["MZ_ID"].ToString() + "'");

                TextBox_MZ_AD1.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)='38213' UNION ALL SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)<>'38213') WHERE MZ_KCODE='" + TextBox_MZ_AD.Text.Trim() + "'");
                TextBox_MZ_EXAD1.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)='38213' UNION ALL SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)<>'38213') WHERE MZ_KCODE='" + TextBox_MZ_EXAD.Text.Trim() + "'");
                TextBox_MZ_EXUNIT1.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='25' AND MZ_KCODE='" + TextBox_MZ_EXUNIT.Text.Trim() + "'");
                TextBox_MZ_OCCC1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_OCCC.Text, "26");
                TextBox_MZ_RANK_1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_RANK.Text, "09");
                TextBox_MZ_RANK1_1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_RANK1.Text, "09");
                TextBox_MZ_UNIT1.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='25' AND MZ_KCODE='" + TextBox_MZ_UNIT.Text.Trim() + "'");
                TextBox_OCCC.Text = dt.Rows[0]["MZ_OCCC"].ToString();
                btUpdate.Enabled = true;
                btDelete.Enabled = true;
                findPic();
            }

        }

        protected void preLoad(string MZ_ID)
        {
            btInsert.Enabled = false;
            btUpdate.Enabled = false;
            btOK.Enabled = true;
            Button2.Enabled = true;
            btCancel.Enabled = true;

            string FindString = "SELECT MZ_NAME,MZ_ID,MZ_AD,MZ_UNIT,MZ_EXAD,MZ_EXUNIT,MZ_RANK,MZ_RANK1,MZ_OCCC,MZ_SRANK,MZ_TBDV FROM A_DLBASE WHERE MZ_ID='" + MZ_ID + "'";

            DataTable FindDt = o_DBFactory.ABC_toTest.Create_Table(FindString, "123");

            if (FindDt.Rows.Count == 1)
            {

                TextBox_MZ_NAME.Text = FindDt.Rows[0]["MZ_NAME"].ToString().Trim();
                TextBox_MZ_ID.Text = FindDt.Rows[0]["MZ_ID"].ToString().Trim().ToUpper();
                findPic();
                TextBox_MZ_AD.Text = FindDt.Rows[0]["MZ_AD"].ToString().Trim().ToUpper();
                TextBox_MZ_UNIT.Text = FindDt.Rows[0]["MZ_UNIT"].ToString().Trim().ToUpper();
                TextBox_MZ_EXAD.Text = FindDt.Rows[0]["MZ_EXAD"].ToString().Trim().ToUpper();
                TextBox_MZ_EXUNIT.Text = FindDt.Rows[0]["MZ_EXUNIT"].ToString().Trim().ToUpper();
                TextBox_MZ_OCCC.Text = FindDt.Rows[0]["MZ_OCCC"].ToString().Trim().ToUpper();
                TextBox_MZ_RANK.Text = FindDt.Rows[0]["MZ_RANK"].ToString().Trim().ToUpper();
                TextBox_MZ_RANK1.Text = FindDt.Rows[0]["MZ_RANK1"].ToString().Trim().ToUpper();
                TextBox_MZ_AD1.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)='38213' UNION ALL SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)<>'38213') WHERE MZ_KCODE='" + TextBox_MZ_AD.Text.Trim() + "'");
                TextBox_MZ_EXAD1.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)='38213' UNION ALL SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)<>'38213') WHERE MZ_KCODE='" + TextBox_MZ_EXAD.Text.Trim() + "'");
                TextBox_MZ_EXUNIT1.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='25' AND MZ_KCODE='" + TextBox_MZ_EXUNIT.Text.Trim() + "'");
                TextBox_MZ_OCCC1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_OCCC.Text, "26");
                TextBox_MZ_RANK_1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_RANK.Text, "09");
                TextBox_MZ_RANK1_1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_RANK1.Text, "09");
                TextBox_MZ_UNIT1.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='25' AND MZ_KCODE='" + TextBox_MZ_UNIT.Text.Trim() + "'");
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_IDNO.ClientID + "').focus();$get('" + TextBox_MZ_IDNO.ClientID + "').focus();", true);
            }
        }

        protected void TextBox_MZ_ID_TextChanged(object sender, EventArgs e)
        {
            TextBox_MZ_ID.Text = TextBox_MZ_ID.Text.ToUpper();

            if (string.IsNullOrEmpty(o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_ID FROM A_DLBASE WHERE MZ_ID='" + TextBox_MZ_ID.Text + "'")))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('查無資料');", true);
            }
            else
            {
                preLoad(TextBox_MZ_ID.Text);
            }
        }

        protected void Ktype_Search(TextBox tb1, TextBox tb2, string ktype)
        {
            //回傳值
            Session["KTYPE_CID"] = tb1.ClientID;
            Session["KTYPE_CID1"] = tb2.ClientID;
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_Ktype_Search.aspx?MZ_KTYPE=" + ktype + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin','top=190,left=200,width=420,height=450,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }

        protected void Ktype_Cname_Check(string Cname, TextBox tb1, TextBox tb2, TextBox tb3)
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
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + tb3.ClientID + "').focus();$get('" + tb3.ClientID + "').focus();", true);
            }
        }

        protected void btEXAD_Click(object sender, EventArgs e)
        {
            Session["KTYPE_CID"] = TextBox_MZ_EXAD.ClientID;
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_Ktype_Search.aspx?CID_NAME=AD&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin','top=190,left=200,width=420,height=450,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }

        protected void btUNIT_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_MZ_UNIT, TextBox_MZ_UNIT1, "25");
        }

        protected void btEXUNIT_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_MZ_EXUNIT, TextBox_MZ_EXUNIT1, "25");
        }

        protected void btAD_Click(object sender, EventArgs e)
        {

            Session["KTYPE_CID"] = TextBox_MZ_AD.ClientID;
            Session["KTYPE_CID1"] = TextBox_MZ_AD1.ClientID;

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_Ktype_Search.aspx?CID_NAME=AD&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin','top=190,left=200,width=420,height=450,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }

        protected void btOCCC_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_MZ_OCCC, TextBox_MZ_OCCC1, "26");
        }

        protected void btRANK_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_MZ_RANK, TextBox_MZ_RANK_1, "09");
        }

        protected void btRANK1_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_MZ_RANK1, TextBox_MZ_RANK1_1, "09");
        }

        protected void btInsert_Click(object sender, EventArgs e)
        {
            Session["Card_Mode"] = "INSERT";

            Session["Card_CMDSQL"] = "INSERT INTO A_POLICE(SN,MZ_IDNO,MZ_SWT,MZ_DATE,MZ_MEMO1,MZ_ODATE,MZ_EDATE,MZ_MEMO,MZ_NO1,MZ_INO,MZ_ID,MZ_OCCC,MZ_NAME,MZ_BIR) VALUES ( NEXT VALUE FOR dbo.A_POLICE_SN,@MZ_IDNO,@MZ_SWT,@MZ_DATE,@MZ_MEMO1,@MZ_ODATE,@MZ_EDATE,@MZ_MEMO,@MZ_NO1,@MZ_INO,@MZ_ID,@MZ_OCCC,@MZ_NAME,@MZ_BIR) ";

            ViewState["strsql"] = null;

            foreach (object ob in Panel_Card.Controls)
            {
                if (ob is TextBox)
                {
                    TextBox tb = ob as TextBox;
                    tb.Text = string.Empty;
                }
                if (ob is DropDownList)
                {
                    DropDownList dl = ob as DropDownList;
                    dl.SelectedValue = string.Empty;
                }
            }

            GridView1.DataSource = null;
            GridView1.DataBind();


            btInsert.Enabled = false;
            btUpdate.Enabled = false;
            btCancel.Enabled = true;
            btOK.Enabled = true;
            Button2.Enabled = true;
            A.controlEnable(ref this.Panel_Card, true);
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_ID.ClientID + "').focus();$get('" + TextBox_MZ_ID.ClientID + "').focus();", true);
        }

        protected void btUpdate_Click(object sender, EventArgs e)
        {
            Personal_Card_SN = Session["Personal_Card_SN"] as List<string>;

            Session["Card_Mode"] = "UPDATE";
            Session["Card_CMDSQL"] = "UPDATE A_POLICE SET MZ_IDNO = @MZ_IDNO,MZ_SWT = @MZ_SWT,MZ_DATE = @MZ_DATE,MZ_MEMO1 = @MZ_MEMO1,MZ_ODATE = @MZ_ODATE,MZ_EDATE = @MZ_EDATE,MZ_MEMO = @MZ_MEMO,MZ_NO1 = @MZ_NO1,MZ_INO = @MZ_INO,MZ_ID = @MZ_ID,MZ_OCCC=@MZ_OCCC,MZ_NAME=@MZ_NAME,MZ_BIR=@MZ_BIR WHERE SN=" + Personal_Card_SN[int.Parse(xcount.Text)] + "";

            btDelete.Enabled = true;
            btInsert.Enabled = false;
            btUpdate.Enabled = false;
            btCancel.Enabled = true;
            btOK.Enabled = true;
            A.controlEnable(ref this.Panel_Card, true);
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_ID.ClientID + "').focus();$get('" + TextBox_MZ_ID.ClientID + "').focus();", true);
        }

        #region Excel匯出
        protected void btExcel_Click(object sender, EventArgs e)
        {
            btn_ModalPop.Show();
        }
  
        //合併列印
        protected void btExcel_Export_Click(object sender, EventArgs e)
        {
            DataTable excel_dt = new DataTable();

            string strSQL = string.Format(@"SELECT '新北市政府警察局' As 服務機關, A_POLICE.MZ_OCCC As 職別, A_POLICE.MZ_NAME As 姓名, 
                                                dbo.SUBSTR(A_POLICE.MZ_BIR, 0, 3) As 出生年, dbo.SUBSTR(A_POLICE.MZ_BIR, 4, 2) As 出生月, dbo.SUBSTR(A_POLICE.MZ_BIR, 6, 2) As 出生日, 
                                                '新北警' As ""證號-1"", A_POLICE.MZ_IDNO As ""證號-2"",  
                                                dbo.SUBSTR(A_POLICE.MZ_DATE, 0, 3) As 發證年, dbo.SUBSTR(A_POLICE.MZ_DATE, 4, 2) As 發證月, dbo.SUBSTR(A_POLICE.MZ_DATE, 6, 2) As 發證日,
                                                dbo.SUBSTR('{0}', 0, 3) As 有效年, dbo.SUBSTR('{0}', 4, 2) As 有效月, dbo.SUBSTR('{0}', 6, 2) As 有效日 
                                            FROM A_POLICE 
                                            LEFT JOIN A_DLBASE on A_DLBASE.MZ_ID=A_POLICE.MZ_ID 
                                            WHERE A_POLICE.MZ_MEMO1='N'", txt_Exp_DATE.Text);
            if (!string.IsNullOrEmpty(txt_Insert_YEAR.Text))
            {
                strSQL += string.Format(@" AND A_POLICE.MZ_DATE='{0}'", txt_Insert_YEAR.Text);
            }

            strSQL += @" ORDER BY A_DLBASE.MZ_AD,A_DLBASE.MZ_UNIT";

            excel_dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "excel");

            new OfficeHelpers.ExcelHelpers().DtToExcelForXLS(excel_dt, "合併列印名冊", true);

            btn_ModalPop.Show();
        }

        protected void btExcel_Leave_Click(object sender, EventArgs e)
        {
            btn_ModalPop.Hide();
        }
        //領用清冊
        protected void btExcel_Export1_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txt_Insert_YEAR1.Text))
            {
                if (!ForDateTime.Check_date(txt_Insert_YEAR1.Text))
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('日期格式錯誤')", true);
                    btn_ModalPop2.Show();
                    return;
                }
            }

            if (!String.IsNullOrEmpty(txt_Insert_YEAR2.Text))
            {
                if (!ForDateTime.Check_date(txt_Insert_YEAR2.Text))
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('日期格式錯誤')", true);
                    btn_ModalPop2.Show();
                    return;
                }
            }
            
           

            DataTable excel_dt = new DataTable();

            excel_dt.Columns.Add("服務機關", typeof(string));
            excel_dt.Columns.Add("服務單位", typeof(string));
            excel_dt.Columns.Add("職稱", typeof(string));
            excel_dt.Columns.Add("服務證號", typeof(string));
            excel_dt.Columns.Add("服務證類別", typeof(string));
            excel_dt.Columns.Add("發證日期", typeof(string));
            excel_dt.Columns.Add("是否列印", typeof(string));
            excel_dt.Columns.Add("繳回日期", typeof(string));
            excel_dt.Columns.Add("遺失日期", typeof(string));
            excel_dt.Columns.Add("備註", typeof(string));
            excel_dt.Columns.Add("徽號", typeof(string));
            excel_dt.Columns.Add("換發原因", typeof(string));
            excel_dt.Columns.Add("身分證號", typeof(string));
            excel_dt.Columns.Add("出生日期", typeof(string));
            excel_dt.Columns.Add("姓名", typeof(string));
            excel_dt.Columns.Add("職稱代碼", typeof(string));
            excel_dt.Columns.Add("是否在職", typeof(string));
            excel_dt.Columns.Add("序列", typeof(string));

            string strSQL = string.Format(@"SELECT  A_DLBASE.MZ_EXAD,
                                                    A_DLBASE.MZ_UNIT,
                                                    A_POLICE.MZ_OCCC,
                                                    A_POLICE.MZ_IDNO,
                                                    A_POLICE.MZ_SWT,
                                                    A_POLICE.MZ_DATE,
                                                    A_POLICE.MZ_MEMO1,
                                                    A_POLICE.MZ_ODATE,
                                                    A_POLICE.MZ_EDATE,
                                                    A_POLICE.MZ_MEMO,
                                                    A_POLICE.MZ_NO1,
                                                    A_POLICE.MZ_INO,
                                                    A_POLICE.MZ_ID,
                                                    A_POLICE.MZ_BIR,
                                                    A_POLICE.MZ_NAME,
                                                    A_DLBASE.MZ_OCCC as MZ_OCCC1,
                                                    A_DLBASE.MZ_STATUS2,
                                                    A_DLBASE.MZ_TBDV 
                                            FROM A_POLICE 
                                            LEFT JOIN A_DLBASE on A_DLBASE.MZ_ID=A_POLICE.MZ_ID 
                                            WHERE 1=1");

            if (!string.IsNullOrEmpty(DropDownList_Excel_AD.SelectedValue))
                strSQL += string.Format(@" AND A_DLBASE.MZ_EXAD='{0}'", DropDownList_Excel_AD.SelectedValue);
            //if (!string.IsNullOrEmpty(txt_Insert_YEAR1.Text))
            //    strSQL += string.Format(@" AND A_POLICE.MZ_DATE='{0}'", txt_Insert_YEAR.Text);

            strSQL += @" ORDER BY A_DLBASE.MZ_AD,A_DLBASE.MZ_UNIT";

            DataTable dt = new DataTable();

            dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "excel");


            foreach (DataRow dr in dt.Rows)
            {
                DataRow excel_dr = excel_dt.NewRow();

                if (!string.IsNullOrEmpty(dr["MZ_DATE"].ToString()))
                {
                        DateTime MZ_DATE = StringtoDate(dr["MZ_DATE"].ToString());

                        if (String.IsNullOrEmpty(txt_Insert_YEAR1.Text) && String.IsNullOrEmpty(txt_Insert_YEAR2.Text))
                        {
                                excel_dt.Rows.Add(DataTabletoExcel(excel_dt, dr));
                        }
                        else if (String.IsNullOrEmpty(txt_Insert_YEAR1.Text) && !String.IsNullOrEmpty(txt_Insert_YEAR2.Text))
                        {

                            DateTime dt2 = StringtoDate(txt_Insert_YEAR2.Text);
                            if (MZ_DATE < dt2.AddDays(1))
                            {
                                excel_dt.Rows.Add(DataTabletoExcel(excel_dt, dr));
                            }
                        }
                        else if (!String.IsNullOrEmpty(txt_Insert_YEAR1.Text) && String.IsNullOrEmpty(txt_Insert_YEAR2.Text))
                        {
                            DateTime dt1 = StringtoDate(txt_Insert_YEAR1.Text);
                            if (MZ_DATE >= dt1)
                            {
                                excel_dt.Rows.Add(DataTabletoExcel(excel_dt, dr));
                            }
                           
                        }
                        else if (!String.IsNullOrEmpty(txt_Insert_YEAR1.Text) && !String.IsNullOrEmpty(txt_Insert_YEAR2.Text))
                        {
                            DateTime dt1 = StringtoDate(txt_Insert_YEAR1.Text);
                            DateTime dt2 = StringtoDate(txt_Insert_YEAR2.Text);
                            if (MZ_DATE >= dt1 && MZ_DATE < dt2.AddDays(1))
                            {
                                excel_dt.Rows.Add(DataTabletoExcel(excel_dt, dr));
                            }
                        }
                      
                }
              

            }

            App_Code.ToExcel.Dt2Excel(excel_dt, HttpUtility.UrlEncode("領用清冊名冊", System.Text.Encoding.UTF8));

            btn_ModalPop2.Show();
        }

        protected void btExcel_Leave1_Click(object sender, EventArgs e)
        {
            btn_ModalPop2.Hide();
        }
        protected void btExcel1_Click(object sender, EventArgs e)
        {
            btn_ModalPop2.Show();
        }
        //設定已列印
        protected void btMEMO_1_Click(object sender, EventArgs e)
        {
            SqlParameter[] parameterList = {
            new SqlParameter("MZ_MEMO1",SqlDbType.VarChar){Value = "Y" },};

            try
            {
                Session["Card_Mode"] = "UPDATE";
                Session["Card_CMDSQL"] = "UPDATE A_POLICE SET MZ_MEMO1 = @MZ_MEMO1";
                o_DBFactory.ABC_toTest.ExecuteNonQuery( Session["Card_CMDSQL"].ToString(), parameterList);

                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('修改成功');", true);
                //    //2010.06.04 LOG紀錄 by伊珊
                TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00003", TPMPermissions._boolTPM(), o_DBFactory.ABC_toTest.RegexSQL(Session["Card_CMDSQL"].ToString(), parameterList));
            }
            catch
            {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('修改失敗');", true);
            }
        }

        //時間轉日期
        private DateTime StringtoDate(String Date)
        {
            DateTime dt;

            int ROC_index = 3;

            if (Date.Length > 7)//年超過3位數
            {
                ROC_index += Date.Length - 7;
            }
            else if (Date.Length<7)
            {
                ROC_index = 2;
            }

     
                int WYear = int.Parse(Date.Substring(0, ROC_index)) + 1911;//

                string DUTYDATE = WYear.ToString() + "/" + Date.Substring(ROC_index, 2) + "/" + Date.Substring(ROC_index + 2, 2);

                dt = Convert.ToDateTime(DUTYDATE);

                return dt;
        }
        
        //DataTabletoExcel
        private DataRow DataTabletoExcel(DataTable excel_dt, DataRow dr)
        {
            DataRow excel_dr = excel_dt.NewRow(); 
            
            excel_dr["服務機關"] = o_A_KTYPE.CODE_TO_NAME(dr["MZ_EXAD"].ToString(), "04");
            excel_dr["服務單位"] = o_A_KTYPE.CODE_TO_NAME(dr["MZ_UNIT"].ToString(), "25");
            excel_dr["職稱"] = dr["MZ_OCCC"].ToString();
            excel_dr["服務證號"] = dr["MZ_IDNO"].ToString();
            switch (dr["MZ_SWT"].ToString())
            {
                case "1":
                    excel_dr["服務證類別"] = "行政警察";
                    break;
                case "2":
                    excel_dr["服務證類別"] = "刑事警察";
                    break;
                case "3":
                    excel_dr["服務證類別"] = "外事警察";
                    break;
                case "4":
                    excel_dr["服務證類別"] = "一般警察";
                    break;
                default:
                    excel_dr["服務證類別"] = "";
                    break;
            }

            excel_dr["發證日期"] = dr["MZ_DATE"].ToString();
            excel_dr["是否列印"] = dr["MZ_MEMO1"].ToString();
            excel_dr["繳回日期"] = dr["MZ_ODATE"].ToString();
            excel_dr["遺失日期"] = dr["MZ_EDATE"].ToString();
            excel_dr["備註"] = dr["MZ_MEMO"].ToString();
            excel_dr["徽號"] = dr["MZ_NO1"].ToString();
            switch (dr["MZ_INO"].ToString())
            {
                case "1":
                    excel_dr["換發原因"] = "換發";
                    break;
                case "2":
                    excel_dr["換發原因"] = "補發";
                    break;
                case "3":
                    excel_dr["換發原因"] = "註銷";
                    break;
                case "4":
                    excel_dr["換發原因"] = "繳回";
                    break;
                default:
                    excel_dr["換發原因"] = "";
                    break;
            }
            excel_dr["身分證號"] = dr["MZ_ID"].ToString();
            excel_dr["出生日期"] = dr["MZ_BIR"].ToString();
            excel_dr["姓名"] = dr["MZ_NAME"].ToString();
            excel_dr["職稱代碼"] = dr["MZ_OCCC1"].ToString();
            excel_dr["是否在職"] = dr["MZ_STATUS2"].ToString();
            excel_dr["序列"] = dr["MZ_TBDV"].ToString();
          


            return excel_dr;
        }

        #endregion

        protected void btOK_Click(object sender, EventArgs e)
        {

            //2010.06.07 by 伊珊
            string ErrorString = "";
            //20141216 這應該是想做控制項檢核.但 DBDIC 又沒有資料
            //foreach (Object ob in Panel_Card.Controls)
            //{
            //    if (ob is TextBox)
            //    {
            //        TextBox tbox = (TextBox)ob;

            //        string result = o_Check_Dbdic.ErrorString(tbox.ID, "A_POLICE", tbox.Text);

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

            //        string result = o_Check_Dbdic.ErrorString(dlist.ID, "A_POLICE", dlist.Text);

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

            string BIR = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_BIR FROM A_POLICE WHERE MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "' AND MZ_IDNO='" + TextBox_MZ_IDNO.Text.Trim() + "'");
            string BIR1 = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_BIR FROM A_DLBASE WHERE MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "'");

            SqlParameter[] parameterList = {
            new SqlParameter("MZ_IDNO",SqlDbType.VarChar){Value = TextBox_MZ_IDNO.Text},
            new SqlParameter("MZ_SWT",SqlDbType.VarChar){Value = DropDownList_MZ_SWT .SelectedValue },//DropDownList_MZ_SWT.Text},
            new SqlParameter("MZ_DATE",SqlDbType.VarChar){Value = (string.IsNullOrEmpty (TextBox_MZ_DATE1.Text )?"":TextBox_MZ_DATE1 .Text.Trim().Replace ("/","").PadLeft (7,'0') )},//DropDownList_MZ_DATE.Text},
            new SqlParameter("MZ_MEMO1",SqlDbType.VarChar){Value = DropDownList_MZ_MEMO1.SelectedValue  },
            new SqlParameter("MZ_ODATE",SqlDbType.VarChar){Value = (string.IsNullOrEmpty (TextBox_MZ_ODATE.Text)?"": TextBox_MZ_ODATE.Text.Trim().Replace ("/","").PadLeft (7,'0'))},
            new SqlParameter("MZ_EDATE",SqlDbType.VarChar){Value = (string.IsNullOrEmpty (TextBox_MZ_EDATE.Text)?"": TextBox_MZ_EDATE.Text.Trim().Replace ("/","").PadLeft (7,'0'))},
            new SqlParameter("MZ_MEMO",SqlDbType.VarChar){Value = TextBox_MZ_MEMO.Text},
            new SqlParameter("MZ_NO1",SqlDbType.VarChar){Value = TextBox_MZ_NO1.Text},
            new SqlParameter("MZ_INO",SqlDbType.VarChar){Value = DropDownList_MZ_INO.SelectedValue.Trim()},
            new SqlParameter("MZ_ID",SqlDbType.VarChar){Value = TextBox_MZ_ID.Text},
            new SqlParameter("MZ_OCCC",SqlDbType.VarChar){Value = TextBox_OCCC.Text},
            //new SqlParameter("MZ_AD",SqlDbType.VarChar){Value = TextBox_MZ_AD.Text},
            //new SqlParameter("MZ_UNIT",SqlDbType.VarChar){Value = TextBox_MZ_UNIT.Text},
            //new SqlParameter("MZ_EXAD",SqlDbType.VarChar){Value = TextBox_MZ_EXAD.Text},
            //new SqlParameter("MZ_EXUNIT",SqlDbType.VarChar){Value = TextBox_MZ_EXUNIT.Text},
            //new SqlParameter("MZ_OCCC",SqlDbType.VarChar){Value = TextBox_MZ_OCCC.Text},
            //new SqlParameter("MZ_RANK",SqlDbType.VarChar){Value = TextBox_MZ_RANK.Text},
            //new SqlParameter("MZ_RANK1",SqlDbType.VarChar){Value = TextBox_MZ_RANK1.Text},
            new SqlParameter("MZ_BIR",SqlDbType.VarChar){Value = string.IsNullOrEmpty(BIR)?BIR1:BIR},
            new SqlParameter("MZ_NAME",SqlDbType.VarChar){Value = TextBox_MZ_NAME.Text}
            };

            try
            {
                o_DBFactory.ABC_toTest.ExecuteNonQuery( Session["Card_CMDSQL"].ToString(), parameterList);

                if (Session["Card_Mode"].ToString() == "INSERT")
                {
                    GridView1.DataSource = o_DBFactory.ABC_toTest.Create_Table("SELECT * FROM A_POLICE WHERE MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "' AND MZ_IDNO='" + TextBox_MZ_IDNO.Text.Trim() + "'", "card1");
                    GridView1.DataBind();
                    xcount.Text = "0";

                    string SN = o_DBFactory.ABC_toTest.vExecSQL("SELECT SN FROM A_POLICE WHERE ROWNUM=1 ORDER BY SN DESC");

                    Personal_Card_SN.Insert(0, SN);

                    Session["Personal_Card_SN"] = Personal_Card_SN;

                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('新增成功');", true);
                    //2010.06.04 LOG紀錄 by伊珊
                    TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00002", TPMPermissions._boolTPM(), o_DBFactory.ABC_toTest.RegexSQL(Session["Card_CMDSQL"].ToString(), parameterList));
                }
                else if (Session["Card_Mode"].ToString() == "UPDATE")
                {
                    GridView1.DataSource = o_DBFactory.ABC_toTest.Create_Table(ViewState["strsql"].ToString(), "card1");
                    GridView1.DataBind();
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('修改成功');", true);
                    //2010.06.04 LOG紀錄 by伊珊
                    TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00003", TPMPermissions._boolTPM(), o_DBFactory.ABC_toTest.RegexSQL(Session["Card_CMDSQL"].ToString(), parameterList));
                }
                btUpdate.Enabled = true;
                btInsert.Enabled = true;
                btOK.Enabled = false;
                btCancel.Enabled = false;
                btDelete.Enabled = false;
                Session.Remove("Card_Mode");
                A.controlEnable(ref this.Panel_Card, false);
            }
            catch
            {
                if (Session["Card_Mode"].ToString() == "INSERT")
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('新增失敗');", true);
                }
                else if (Session["Card_Mode"].ToString() == "UPDATE")
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('修改失敗');", true);
                }
            }
        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            if (Session["Card_Mode"].ToString() == "INSERT")
            {
                foreach (object dl in Panel_Card.Controls)
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
                btUpdate.Enabled = false;
            }
            else if (Session["Card_Mode"].ToString() == "UPDATE")
            {
                finddata(int.Parse(xcount.Text.Trim()));

                Personal_Card_SN = Session["Personal_Card_ID"] as List<string>;
                if (int.Parse(xcount.Text.Trim()) == 0 && Personal_Card_SN.Count == 1)
                {
                    btUpper.Enabled = false;
                    btNEXT.Enabled = false;
                }
                else if (int.Parse(xcount.Text.Trim()) == 0 && Personal_Card_SN.Count > 1)
                {
                    btUpper.Enabled = false;
                    btNEXT.Enabled = true;
                }
                else if (int.Parse(xcount.Text.Trim()) + 1 == Personal_Card_SN.Count)
                {
                    btUpper.Enabled = true;
                    btNEXT.Enabled = false;
                }
                else if (int.Parse(xcount.Text.Trim()) > 0 && int.Parse(xcount.Text.Trim()) + 1 < Personal_Card_SN.Count)
                {
                    btNEXT.Enabled = true;
                    btUpper.Enabled = true;
                }
                btUpdate.Enabled = true;
            }

            Session.Remove("PKEY_MZ_ID");
            Session.Remove("PKEY_MZ_IDNO");
            btInsert.Enabled = true;
            btCancel.Enabled = false;
            btOK.Enabled = false;
            btDelete.Enabled = false;
            A.controlEnable(ref this.Panel_Card, false);
        }

        protected void btDelete_Click(object sender, EventArgs e)
        {
            string DeleteString = "DELETE FROM A_POLICE WHERE MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "' AND MZ_IDNO = '" + TextBox_MZ_IDNO.Text.Trim() + "'";
            try
            {
                o_DBFactory.ABC_toTest.Edit_Data(DeleteString);

                Personal_Card_SN = Session["Personal_Card_ID"] as List<string>;

                Personal_Card_SN.RemoveAt(int.Parse(xcount.Text.Trim()));

                if (Personal_Card_SN.Count == 0)
                {

                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除成功');location.href('Personal_Card.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "');", true);
                    //2010.06.04 LOG紀錄 by伊珊
                    TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00004", TPMPermissions._boolTPM(), DeleteString);
                    btUpdate.Visible = false;
                }
                else
                {
                    xcount.Text = "0";
                    finddata(int.Parse(xcount.Text));
                    findPic();
                    if (Personal_Card_SN.Count > 1)
                    {
                        btNEXT.Enabled = true;
                    }
                    btUpdate.Enabled = true;
                    Label1.Text = "第" + (int.Parse(xcount.Text) + 1).ToString() + "筆共" + Personal_Card_SN.Count.ToString() + "筆";
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除成功')", true);
                    //2010.06.04 LOG紀錄 by伊珊
                    TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00004", TPMPermissions._boolTPM(), DeleteString);
                }
                DataTable tempDT = new DataTable();


                if (ViewState["strsql"].ToString() != null)
                {
                    GridView1.DataSource = o_DBFactory.ABC_toTest.Create_Table(ViewState["strsql"].ToString(), "card2");
                    GridView1.DataBind();
                }
                else
                {
                    GridView1.DataSource = null;
                    GridView1.DataBind();
                }
                btInsert.Enabled = true;
                btOK.Enabled = false;
                btCancel.Enabled = false;
                btDelete.Enabled = false;
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除失敗');", true);
            }
        }

        protected void btSearch_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_Card_Search.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "','查詢','top=190,left=200,width=420,height=250,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }

        protected void btUpper_Click(object sender, EventArgs e)
        {
            if (int.Parse(xcount.Text) != 0)
            {
                xcount.Text = (Convert.ToInt32(xcount.Text) - 1).ToString();
                finddata(int.Parse(xcount.Text));
                findPic();
                if (int.Parse(xcount.Text) != Personal_Card_SN.Count - 1)
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
                findPic();
                btUpper.Enabled = false;
            }

            Label1.Text = "第" + (int.Parse(xcount.Text) + 1).ToString() + "筆共" + Personal_Card_SN.Count.ToString() + "筆";
            GridView1.DataSource = Session["Personal_Card_GV_DT"] as DataTable;
            GridView1.PageIndex = int.Parse(xcount.Text) / 4;
            GridView1.DataBind();
            GridView1.SelectedIndex = int.Parse(xcount.Text) % 4;
        }

        protected void btNEXT_Click(object sender, EventArgs e)
        {
            if (int.Parse(xcount.Text) == 0)
            {
                xcount.Text = (Convert.ToInt32(xcount.Text) + 1).ToString();
                btUpper.Enabled = true;
                finddata(int.Parse(xcount.Text));
                findPic();

                if (int.Parse(xcount.Text) == Personal_Card_SN.Count - 1)
                {
                    btNEXT.Enabled = false;
                }
            }
            else
            {
                xcount.Text = (Convert.ToInt32(xcount.Text) + 1).ToString();
                finddata(int.Parse(xcount.Text));
                findPic();

                if (int.Parse(xcount.Text) == Personal_Card_SN.Count - 1)
                {
                    btNEXT.Enabled = false;
                }
            }

            Label1.Text = "第" + (int.Parse(xcount.Text) + 1).ToString() + "筆共" + Personal_Card_SN.Count.ToString() + "筆";
            GridView1.DataSource = Session["Personal_Card_GV_DT"] as DataTable;
            GridView1.PageIndex = int.Parse(xcount.Text) / 4;
            GridView1.DataBind();
            GridView1.SelectedIndex = int.Parse(xcount.Text) % 4;
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('PersonalSearchIDwithNAME.aspx?TableName=CARD&TPM_FION=" + Request.QueryString["TPM_FION"] + "','查詢','top=190,left=200,width=750,height=500,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }

        protected void TextBox_MZ_AD_TextChanged(object sender, EventArgs e)
        {
            string CName = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)='38213' UNION ALL SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)<>'38213') WHERE MZ_KCODE='" + TextBox_MZ_AD.Text.Trim().ToUpper() + "'");
            Ktype_Cname_Check(CName, TextBox_MZ_AD1, TextBox_MZ_AD, TextBox_MZ_UNIT);
        }

        protected void TextBox_MZ_UNIT_TextChanged(object sender, EventArgs e)
        {
            string CName = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='25' AND MZ_KCODE='" + TextBox_MZ_UNIT.Text.Trim().ToUpper() + "'");
            Ktype_Cname_Check(CName, TextBox_MZ_UNIT1, TextBox_MZ_UNIT, TextBox_MZ_EXAD);
        }

        protected void TextBox_MZ_EXAD_TextChanged(object sender, EventArgs e)
        {
            string CName = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)='38213' UNION ALL SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)<>'38213') WHERE MZ_KCODE='" + TextBox_MZ_EXAD.Text.Trim().ToUpper() + "'");

            Ktype_Cname_Check(CName, TextBox_MZ_EXAD1, TextBox_MZ_EXAD, TextBox_MZ_EXUNIT);
        }

        protected void TextBox_MZ_EXUNIT_TextChanged(object sender, EventArgs e)
        {
            string CName = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='25' AND MZ_KCODE='" + TextBox_MZ_EXUNIT.Text.Trim().ToUpper() + "'");

            Ktype_Cname_Check(CName, TextBox_MZ_EXUNIT1, TextBox_MZ_EXUNIT, TextBox_MZ_RANK);
        }

        protected void TextBox_MZ_RANK_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_RANK.Text, "09");

            Ktype_Cname_Check(CName, TextBox_MZ_RANK_1, TextBox_MZ_RANK, TextBox_MZ_RANK1);
        }

        protected void TextBox_MZ_RANK1_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_RANK1.Text, "09");

            Ktype_Cname_Check(CName, TextBox_MZ_RANK1_1, TextBox_MZ_RANK1, TextBox_MZ_OCCC);
        }

        protected void TextBox_MZ_OCCC_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_OCCC.Text, "26");

            Ktype_Cname_Check(CName, TextBox_MZ_OCCC1, TextBox_MZ_OCCC, TextBox_MZ_IDNO);
        }

        protected void TextBox_MZ_DATE1_TextChanged(object sender, EventArgs e)
        {
            returnSameDataType(TextBox_MZ_DATE1, TextBox_MZ_NO1);
        }

        protected void TextBox_MZ_ODATE_TextChanged(object sender, EventArgs e)
        {
            returnSameDataType(TextBox_MZ_ODATE, TextBox_MZ_EDATE);
        }

        protected void TextBox_MZ_EDATE_TextChanged(object sender, EventArgs e)
        {
            returnSameDataType(TextBox_MZ_EDATE, TextBox_MZ_MEMO);
        }

        protected void returnSameDataType(TextBox tb, TextBox tb1)
        {
            tb.Text = o_str.tosql(tb.Text.Trim().Replace("/", ""));

            if (tb.Text != "")
            {
                if (!ForDateTime.Check_date(tb.Text))
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('日期格式錯誤')", true);
                    tb.Text = string.Empty;
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + tb.ClientID + "').focus();$get('" + tb.ClientID + "').focus();", true);
                }
                else
                {
                    tb.Text = o_CommonService.Personal_ReturnDateString(tb.Text.Trim());
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + tb1.ClientID + "').focus();$get('" + tb1.ClientID + "').focus();", true);
                }
            }
        }

        protected void CV_ID_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (string.IsNullOrEmpty(args.Value))
            {
                args.IsValid = false;
                TextBox_MZ_ID.BackColor = Color.Orange;
            }
            else
            {
                args.IsValid = true;
                TextBox_MZ_ID.BackColor = Color.White;
            }
        }

        protected void CV_IDNO_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (string.IsNullOrEmpty(args.Value))
            {
                args.IsValid = false;
                TextBox_MZ_IDNO.BackColor = Color.Orange;
            }
            else
            {
                args.IsValid = true;
                TextBox_MZ_IDNO.BackColor = Color.White;
            }
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView1.SelectedIndex = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "Select")
            {
                btUpdate.Enabled = true;
                btDelete.Enabled = true;
                btInsert.Enabled = true;
                btOK.Enabled = false;
                btCancel.Enabled = false;
                xcount.Text = (GridView1.PageIndex * 4 + GridView1.SelectedIndex).ToString();
                finddata(int.Parse((GridView1.PageIndex * 4 + GridView1.SelectedIndex).ToString()));
                findPic();

                if (int.Parse(xcount.Text) == 0)
                {
                    btNEXT.Enabled = true;
                    btUpper.Enabled = false;
                }
                else if (int.Parse(xcount.Text) > 0 && int.Parse(xcount.Text) < Personal_Card_SN.Count - 1)
                {
                    btNEXT.Enabled = true;
                    btUpper.Enabled = true;
                }
                else if (int.Parse(xcount.Text) == Personal_Card_SN.Count - 1)
                {
                    btUpper.Enabled = true;
                    btNEXT.Enabled = false;
                }
                else
                {
                    btNEXT.Enabled = false;
                    btUpper.Enabled = false;
                }

                if (Personal_Card_SN.Count == 0)
                {
                    Label1.Visible = false;
                }
                else
                {
                    Label1.Visible = true;
                    Label1.Text = "第" + (int.Parse(xcount.Text) + 1).ToString() + "筆共" + Personal_Card_SN.Count.ToString() + "筆";
                }

            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GridView1, "Select$" + e.Row.RowIndex);
                e.Row.Cells[GridView1.Columns.Count - 1].Attributes.Add("Style", "display:none");
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[4].Text.Length == 7)
                    e.Row.Cells[4].Text = (e.Row.Cells[4].Text.Substring(0, 1) == "0" ? e.Row.Cells[4].Text.Substring(1, 2) + "/" + e.Row.Cells[4].Text.Substring(3, 2) + "/" + e.Row.Cells[4].Text.Substring(5, 2) : e.Row.Cells[4].Text.Substring(0, 3) + "/" + e.Row.Cells[4].Text.Substring(3, 2) + "/" + e.Row.Cells[4].Text.Substring(5, 2));
                if (e.Row.Cells[7].Text.Length == 7)
                    e.Row.Cells[7].Text = (e.Row.Cells[7].Text.Substring(0, 1) == "0" ? e.Row.Cells[7].Text.Substring(1, 2) + "/" + e.Row.Cells[7].Text.Substring(3, 2) + "/" + e.Row.Cells[7].Text.Substring(5, 2) : e.Row.Cells[7].Text.Substring(0, 3) + "/" + e.Row.Cells[7].Text.Substring(3, 2) + "/" + e.Row.Cells[7].Text.Substring(5, 2));
                if (e.Row.Cells[6].Text.Length == 7)
                    e.Row.Cells[6].Text = (e.Row.Cells[6].Text.Substring(0, 1) == "0" ? e.Row.Cells[6].Text.Substring(1, 2) + "/" + e.Row.Cells[6].Text.Substring(3, 2) + "/" + e.Row.Cells[6].Text.Substring(5, 2) : e.Row.Cells[6].Text.Substring(0, 3) + "/" + e.Row.Cells[6].Text.Substring(3, 2) + "/" + e.Row.Cells[6].Text.Substring(5, 2));
                switch (e.Row.Cells[3].Text)
                {
                    case "1":
                        e.Row.Cells[3].Text = "行政警察";
                        break;
                    case "2":
                        e.Row.Cells[3].Text = "刑事警察";
                        break;
                    case "3":
                        e.Row.Cells[3].Text = "外事警察";
                        break;
                    case "4":
                        e.Row.Cells[3].Text = "一般行政";
                        break;
                }
                switch (e.Row.Cells[4].Text)
                {
                    case "1":
                        e.Row.Cells[4].Text = "換發";
                        break;
                    case "2":
                        e.Row.Cells[4].Text = "補發";
                        break;
                    case "3":
                        e.Row.Cells[4].Text = "註銷";
                        break;
                    case "4":
                        e.Row.Cells[4].Text = "繳回";
                        break;
                }
                switch (e.Row.Cells[GridView1.Columns.Count - 2].Text)
                {
                    case "Y":
                        e.Row.Cells[GridView1.Columns.Count - 2].Text = "是";
                        break;
                    case "N":
                        e.Row.Cells[GridView1.Columns.Count - 2].Text = "否";
                        break;
                }

            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            GridView1.DataSource = Session["Personal_Card_GV_DT"] as DataTable;
            GridView1.DataBind();
        }
    }
}
