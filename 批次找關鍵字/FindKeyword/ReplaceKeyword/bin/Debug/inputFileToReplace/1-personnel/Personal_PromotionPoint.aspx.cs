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
    public partial class Personal_PromotionPoint : System.Web.UI.Page
    {
        List<String> TP_PER_MZ_ID = new List<string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                A.check_power();
            }

            ViewState["MZ_AD"] = Request["MZ_AD"];
            ViewState["MZ_UNIT"] = Request["MZ_UNIT"];

            ViewState["MZ_ID"] = Session["PersonalSearch_ID"];
            Session.Remove("PersonalSearch_ID");

            ViewState["MZ_YEAR"] = Request["MZ_YEAR"];

            ViewState["MZ_NAME"] = Session["PersonalSearch_NAME"];
            Session.Remove("PersonalSearch_NAME");

            ViewState["MZ_ID1"] = Session["PersonalSearchIDwithNAME_MZ_ID3"];
            Session.Remove("PersonalSearchIDwithNAME_MZ_ID3");

            A.set_Panel_EnterToTAB(ref this.Panel_PromotionPoint);
            A.set_Panel_EnterToTAB(ref this.Panel3);

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

            ////查詢姓名
            //HttpCookie DLBASE_NAME_Cookie = new HttpCookie("PersonalSearch_NAME");
            //DLBASE_NAME_Cookie = Request.Cookies["PersonalSearch_NAME"];

            //if (DLBASE_NAME_Cookie == null)
            //{
            //    ViewState["MZ_NAME"] = null;
            //    Response.Cookies["PersonalSearch_NAME"].Expires = DateTime.Now.AddYears(-1);
            //}
            //else
            //{
            //    ViewState["MZ_NAME"] = TPMPermissions._strDecod(DLBASE_NAME_Cookie.Value.ToString());
            //    Response.Cookies["PersonalSearch_NAME"].Expires = DateTime.Now.AddYears(-1);
            //}

            //HttpCookie PER_CHECK_ID_Cookie = new HttpCookie("PersonalSearchIDwithNAME_MZ_ID3");
            //PER_CHECK_ID_Cookie = Request.Cookies["PersonalSearchIDwithNAME_MZ_ID3"];

            //if (PER_CHECK_ID_Cookie == null)
            //{
            //    ViewState["MZ_ID1"] = null;
            //    Response.Cookies["PersonalSearchIDwithNAME_MZ_ID3"].Expires = DateTime.Now.AddYears(-1);
            //}
            //else
            //{
            //    ViewState["MZ_ID1"] = TPMPermissions._strDecod(PER_CHECK_ID_Cookie.Value.ToString());
            //    Response.Cookies["PersonalSearchIDwithNAME_MZ_ID3"].Expires = DateTime.Now.AddYears(-1);
            //}

            if (!IsPostBack)
            {
                bool CHAGE = false;

                txt_Report_Year.Text = (DateTime.Now.Year - 1911).ToString();

                Button_Excel.Attributes.Add("OnClick", Page.ClientScript.GetPostBackEventReference(Button_PBExcel, ""));
                if (ViewState["XCOUNT"] != null)
                {
                    finddata(int.Parse(ViewState["XCOUNT"].ToString()));
                    xcount.Text = ViewState["XCOUNT"].ToString();
                    if (int.Parse(ViewState["XCOUNT"].ToString()) == 0 && TP_PER_MZ_ID.Count > 1)
                    {
                        btNEXT.Enabled = true;
                        btUpper.Enabled = false;
                    }
                    else if (int.Parse(ViewState["XCOUNT"].ToString()) > 0 && int.Parse(ViewState["XCOUNT"].ToString()) < TP_PER_MZ_ID.Count - 1)
                    {
                        btNEXT.Enabled = true;
                        btUpper.Enabled = true;
                    }
                    else if (int.Parse(ViewState["XCOUNT"].ToString()) == TP_PER_MZ_ID.Count - 1)
                    {
                        btUpper.Enabled = true;
                        btNEXT.Enabled = false;
                    }
                    else
                    {
                        btNEXT.Enabled = false;
                        btUpper.Enabled = false;
                    }

                    if (TP_PER_MZ_ID.Count == 0)
                    {
                        Label1.Visible = false;
                    }
                    else
                    {
                        Label1.Visible = true;
                        Label1.Text = "第" + (int.Parse(xcount.Text) + 1).ToString() + "筆共" + TP_PER_MZ_ID.Count.ToString() + "筆";
                    }

                    btUpdate.Enabled = true;
                    btInsert.Enabled = true;
                    btOK.Enabled = false;
                    btCancel.Enabled = false;
                    btDelete.Enabled = true;
                    CHAGE = false;
                }

                if (ViewState["MZ_ID"] != null)
                {
                    string strSQL = "SELECT MZ_ID FROM A_TP_PER WHERE 1=1";

                    if (ViewState["MZ_ID"].ToString() != "")
                    {
                        strSQL = strSQL + " AND MZ_ID='" + ViewState["MZ_ID"].ToString() + "'";
                    }
                    if (ViewState["MZ_NAME"].ToString() != "")
                    {
                        strSQL = strSQL + " AND MZ_NAME='" + ViewState["MZ_NAME"].ToString() + "'";
                    }
                    if (ViewState["MZ_AD"].ToString() != "")
                    {
                        strSQL = strSQL + " AND MZ_AD='" + ViewState["MZ_AD"].ToString() + "'";
                    }
                    if (ViewState["MZ_UNIT"].ToString() != "")
                    {
                        strSQL = strSQL + " AND MZ_UNIT='" + ViewState["MZ_UNIT"].ToString() + "'";
                    }
                    if (ViewState["MZ_YEAR"].ToString() != "")
                    {
                        strSQL = strSQL + " AND MZ_YEAR='" + ViewState["MZ_YEAR"].ToString() + "'";
                    }

                    strSQL = strSQL + " ORDER BY MZ_ID";

                    TP_PER_MZ_ID = o_DBFactory.ABC_toTest.DataListArray(strSQL, "MZ_ID");

                    Session["TP_PER_MZ_ID"] = TP_PER_MZ_ID;

                    if (TP_PER_MZ_ID.Count == 0)
                    {
                        Response.Write(@"<script language=javascript>window.alert('查無資料！');location.href('Personal_PromotionPoint.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "');</script>");
                    }
                    else if (TP_PER_MZ_ID.Count == 1)
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

                    if (TP_PER_MZ_ID.Count == 0)
                    {
                        Label1.Visible = false;
                    }
                    else
                    {
                        Label1.Visible = true;
                        Label1.Text = "第" + (int.Parse(xcount.Text) + 1).ToString() + "筆共" + TP_PER_MZ_ID.Count.ToString() + "筆";
                    }
                    CHAGE = false;
                }
                if (ViewState["MZ_ID1"] != null)
                {
                    preLoad(ViewState["MZ_ID1"].ToString());
                    CHAGE = true;
                }

                A.controlEnable(ref this.Panel_PromotionPoint, CHAGE);
            }
        }

        protected void preLoad(string ID)
        {
            btInsert.Enabled = false;
            btUpdate.Enabled = false;
            btOK.Enabled = true;
            btCancel.Enabled = true;

            string FindString = @"SELECT MZ_NAME,MZ_ID,MZ_AD,MZ_AD_CH ,MZ_UNIT,MZ_UNIT_CH,MZ_EXAD,MZ_EXAD_CH,MZ_EXUNIT,MZ_EXUNIT_CH,
            MZ_OCCC,MZ_OCCC_CH,MZ_SRANK,MZ_SRANK_CH,MZ_TBDV ,MZ_TBDV_CH  FROM VW_A_DLBASE WHERE RTRIM(MZ_ID)='" + ID + "'";

            DataTable FindDt = o_DBFactory.ABC_toTest.Create_Table(FindString, "123");

            if (FindDt.Rows.Count == 1)
            {
                TextBox_MZ_YEAR.Text = (DateTime.Now.Year - 1911).ToString();

                TextBox_MZ_NAME.Text = FindDt.Rows[0]["MZ_NAME"].ToString().Trim();
                TextBox_MZ_ID.Text = FindDt.Rows[0]["MZ_ID"].ToString().Trim().ToUpper();
                TextBox_MZ_AD.Text = FindDt.Rows[0]["MZ_AD"].ToString().Trim().ToUpper();
                TextBox_MZ_UNIT.Text = FindDt.Rows[0]["MZ_UNIT"].ToString().Trim().ToUpper();
                TextBox_MZ_EXAD.Text = FindDt.Rows[0]["MZ_EXAD"].ToString().Trim().ToUpper();
                TextBox_MZ_EXUNIT.Text = FindDt.Rows[0]["MZ_EXUNIT"].ToString().Trim().ToUpper();
                TextBox_MZ_OCCC.Text = FindDt.Rows[0]["MZ_OCCC"].ToString().Trim().ToUpper();
                TextBox_MZ_SRANK.Text = FindDt.Rows[0]["MZ_SRANK"].ToString().Trim().ToUpper();
                TextBox_MZ_TBDV.Text = FindDt.Rows[0]["MZ_TBDV"].ToString().Trim().ToUpper();

                //20140407
                TextBox_MZ_AD1.Text = FindDt.Rows[0]["MZ_AD_CH"].ToString().Trim();
                TextBox_MZ_EXAD1.Text = FindDt.Rows[0]["MZ_EXAD_CH"].ToString().Trim();
                TextBox_MZ_EXUNIT1.Text = FindDt.Rows[0]["MZ_EXUNIT_CH"].ToString().Trim();
                TextBox_MZ_OCCC1.Text = FindDt.Rows[0]["MZ_OCCC_CH"].ToString().Trim();
                TextBox_MZ_SRANK1.Text = FindDt.Rows[0]["MZ_SRANK_CH"].ToString().Trim();
                TextBox_MZ_UNIT1.Text = FindDt.Rows[0]["MZ_UNIT_CH"].ToString().Trim();
                TextBox_MZ_TBDV1.Text = FindDt.Rows[0]["MZ_TBDV_CH"].ToString().Trim();

                //TextBox_MZ_AD1.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)='38213' UNION ALL SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,15)<>'38213') WHERE MZ_KCODE='" + TextBox_MZ_AD.Text.Trim() + "'");
                //TextBox_MZ_EXAD1.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)='38213' UNION ALL SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)<>'38213') WHERE MZ_KCODE='" + TextBox_MZ_EXAD.Text.Trim() + "'");
                //TextBox_MZ_EXUNIT1.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='25' AND MZ_KCODE='" + TextBox_MZ_EXUNIT.Text.Trim() + "'");
                //TextBox_MZ_OCCC1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_OCCC.Text, "26");
                //TextBox_MZ_SRANK1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_SRANK.Text, "09");
                //TextBox_MZ_UNIT1.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='25' AND MZ_KCODE='" + TextBox_MZ_UNIT.Text.Trim() + "'");
                //TextBox_MZ_TBDV1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_TBDV.Text, "43");
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_TOT.ClientID + "').focus();$get('" + TextBox_MZ_TOT.ClientID + "').focus();", true);
            }
        }

        protected void finddata(int DataCount)
        {
            TP_PER_MZ_ID = Session["TP_PER_MZ_ID"] as List<String>;
            //irk 101/6/4 增加 年度
            string year = ViewState["MZ_YEAR"].ToString() == "" ? (DateTime.Now.Year - 1911).ToString() : ViewState["MZ_YEAR"].ToString();
            string SelectString = "SELECT * FROM A_TP_PER WHERE MZ_ID='" + TP_PER_MZ_ID[DataCount] + "' and MZ_YEAR=" + year;

            DataTable temp = new DataTable();

            temp = o_DBFactory.ABC_toTest.Create_Table(SelectString, "GET");

            TextBox_MZ_AD.Text = temp.Rows[0]["MZ_AD"].ToString();
            TextBox_MZ_EXAD.Text = temp.Rows[0]["MZ_EXAD"].ToString();
            TextBox_MZ_EXUNIT.Text = temp.Rows[0]["MZ_EXUNIT"].ToString();
            TextBox_MZ_ID.Text = temp.Rows[0]["MZ_ID"].ToString();
            TextBox_MZ_NAME.Text = temp.Rows[0]["MZ_NAME"].ToString();
            TextBox_MZ_OCCC.Text = temp.Rows[0]["MZ_OCCC"].ToString();
            TextBox_MZ_SCEA.Text = temp.Rows[0]["MZ_SCEA"].ToString();
            TextBox_MZ_SCMA.Text = temp.Rows[0]["MZ_SCMA"].ToString();
            TextBox_MZ_SCPA.Text = temp.Rows[0]["MZ_SCPA"].ToString();
            TextBox_MZ_SCPH.Text = temp.Rows[0]["MZ_SCPH"].ToString();
            TextBox_MZ_SCSP.Text = temp.Rows[0]["MZ_SCSP"].ToString();
            TextBox_MZ_SCTA.Text = temp.Rows[0]["MZ_SCTA"].ToString();
            TextBox_MZ_SCTF1.Text = temp.Rows[0]["MZ_SCTF1"].ToString();
            TextBox_MZ_SCTF2.Text = temp.Rows[0]["MZ_SCTF2"].ToString();
            TextBox_MZ_SCTF3.Text = temp.Rows[0]["MZ_SCTF3"].ToString();
            TextBox_MZ_SCTF4.Text = temp.Rows[0]["MZ_SCTF4"].ToString();
            TextBox_MZ_SCTF6.Text = temp.Rows[0]["MZ_SCTF6"].ToString();
            TextBox_MZ_SCTN1.Text = temp.Rows[0]["MZ_SCTN1"].ToString();
            TextBox_MZ_SCTN2.Text = temp.Rows[0]["MZ_SCTN2"].ToString();
            TextBox_MZ_SCYA.Text = temp.Rows[0]["MZ_SCYA"].ToString();
            TextBox_MZ_SRANK.Text = temp.Rows[0]["MZ_SRANK"].ToString();
            TextBox_MZ_SCR1.Text = temp.Rows[0]["MZ_SCR1"].ToString();
            TextBox_MZ_TBDV.Text = temp.Rows[0]["MZ_TBDV"].ToString();
            TextBox_MZ_TOT.Text = temp.Rows[0]["MZ_TOT"].ToString();
            TextBox_MZ_TOT1.Text = temp.Rows[0]["MZ_TOT1"].ToString();
            TextBox_MZ_TOT2.Text = temp.Rows[0]["MZ_TOT2"].ToString();
            TextBox_MZ_TOT3.Text = temp.Rows[0]["MZ_TOT3"].ToString();
            TextBox_MZ_UNIT.Text = temp.Rows[0]["MZ_UNIT"].ToString();
            TextBox_MZ_YEAR.Text = temp.Rows[0]["MZ_YEAR"].ToString();

            TextBox_MZ_UNIT1.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='25' AND MZ_KCODE='" + o_str.tosql(TextBox_MZ_UNIT.Text.Trim()) + "'");
            TextBox_MZ_TBDV1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_TBDV.Text, "43");
            TextBox_MZ_SRANK1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_SRANK.Text, "09");
            TextBox_MZ_OCCC1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_OCCC.Text, "26");
            TextBox_MZ_EXUNIT1.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='25' AND MZ_KCODE='" + o_str.tosql(TextBox_MZ_EXUNIT.Text.Trim()) + "'");
            TextBox_MZ_EXAD1.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)='38213' UNION ALL SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)<>'38213') WHERE MZ_KCODE='" + o_str.tosql(TextBox_MZ_EXAD.Text.Trim()) + "'");
            TextBox_MZ_AD1.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)='38213' UNION ALL SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)<>'38213') WHERE MZ_KCODE='" + o_str.tosql(TextBox_MZ_AD.Text.Trim()) + "'");

            btUpdate.Enabled = true;
            btDelete.Enabled = true;

        }

        protected void btInsert_Click(object sender, EventArgs e)
        {
            Session["Mode"] = "INSERT";

            foreach (object dl in Panel_PromotionPoint.Controls)
            {

                if (dl is TextBox)
                {
                    TextBox tbox = dl as TextBox;
                    tbox.Text = "";
                }
            }

            Session["TP_PER_CMDSQL"] = "INSERT INTO A_TP_PER(MZ_ID,MZ_NAME,MZ_AD,MZ_UNIT,MZ_EXAD,MZ_EXUNIT,MZ_OCCC,MZ_SRANK,MZ_TBDV,MZ_SCEA," +
                                                    " MZ_SCTA,MZ_SCYA,MZ_SCMA,MZ_SCPA,MZ_SCPH,MZ_SCSP,MZ_SCTF1,MZ_SCTF2,MZ_SCTF3,MZ_SCTF4," +
                                                    " MZ_SCTN1,MZ_SCTN2,MZ_SCTN3,MZ_SCTN4,MZ_SCR1,MZ_TOT,MZ_TOT1,MZ_TOT2,MZ_TOT3,MZ_SCTF5," +
                                                    " MZ_SCTF6,MZ_YEAR,MDATE,MUSER) " +
                                             " VALUES(@MZ_ID,@MZ_NAME,@MZ_AD,@MZ_UNIT,@MZ_EXAD,@MZ_EXUNIT,@MZ_OCCC,@MZ_SRANK,@MZ_TBDV,@MZ_SCEA," +
                                                    " @MZ_SCTA,@MZ_SCYA,@MZ_SCMA,@MZ_SCPA,@MZ_SCPH,@MZ_SCSP,@MZ_SCTF1,@MZ_SCTF2,@MZ_SCTF3,@MZ_SCTF4," +
                                                    " @MZ_SCTN1,@MZ_SCTN2,@MZ_SCTN3,@MZ_SCTN4,@MZ_SCR1,@MZ_TOT,@MZ_TOT1,@MZ_TOT2,@MZ_TOT3,@MZ_SCTF5," +
                                                    " @MZ_SCTF6,@MZ_YEAR,@MDATE,@MUSER)";


            btNEXT.Enabled = false;
            btUpper.Enabled = false;
            btInsert.Enabled = false;
            btUpdate.Enabled = false;
            btCancel.Enabled = true;
            btOK.Enabled = true;
            
            A.controlEnable(ref this.Panel_PromotionPoint, true);

            TextBox_MZ_YEAR.Text = (DateTime.Now.Year - 1911).ToString();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_ID.ClientID + "').focus();$get('" + TextBox_MZ_ID.ClientID + "').focus();", true);
        }

        protected void btUpdate_Click(object sender, EventArgs e)
        {
            Session["Mode"] = "UPDATE";
            Session["TP_PER_CMDSQL"] = "UPDATE A_TP_PER SET MZ_ID=@MZ_ID,MZ_NAME=@MZ_NAME,MZ_AD=@MZ_AD,MZ_UNIT=@MZ_UNIT,MZ_EXAD=@MZ_EXAD,MZ_EXUNIT=@MZ_EXUNIT," +
                                                   " MZ_OCCC=@MZ_OCCC,MZ_SRANK=@MZ_SRANK,MZ_TBDV=@MZ_TBDV,MZ_SCEA=@MZ_SCEA," +
                                                   " MZ_SCTA=@MZ_SCTA,MZ_SCYA=@MZ_SCYA,MZ_SCMA=@MZ_SCMA,MZ_SCPA=@MZ_SCPA,MZ_SCPH=@MZ_SCPH,MZ_SCSP=@MZ_SCSP," +
                                                   " MZ_SCTF1=@MZ_SCTF1,MZ_SCTF2=@MZ_SCTF2,MZ_SCTF3=@MZ_SCTF3,MZ_SCTF4=@MZ_SCTF4," +
                                                   " MZ_SCTN1=@MZ_SCTN1,MZ_SCTN2=@MZ_SCTN2,MZ_SCTN3=@MZ_SCTN3,MZ_SCTN4=@MZ_SCTN4,MZ_SCR1=@MZ_SCR1,MZ_TOT=@MZ_TOT," +
                                                   " MZ_TOT1=@MZ_TOT1,MZ_TOT2=@MZ_TOT2,MZ_TOT3=@MZ_TOT3,MZ_SCTF5=@MZ_SCTF5," +
                                                   " MZ_SCTF6=@MZ_SCTF6,MZ_YEAR=@MZ_YEAR,MDATE=@MDATE,MUSER=@MUSER WHERE MZ_ID='" + o_str.tosql(TextBox_MZ_ID.Text) + "' AND MZ_YEAR='" + TextBox_MZ_YEAR.Text + "'";

            Session["PKEY_MZ_ID"] = TextBox_MZ_ID.Text;
            Session["PKEY_MZ_YEAR"] = TextBox_MZ_YEAR.Text;

            btNEXT.Enabled = false;
            btUpper.Enabled = false;
            btDelete.Enabled = true;
            btInsert.Enabled = false;
            btUpdate.Enabled = false;
            btCancel.Enabled = true;
            btOK.Enabled = true;
            A.controlEnable(ref this.Panel_PromotionPoint, true);
            btNameSearch.Enabled = false;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_ID.ClientID + "').focus();$get('" + TextBox_MZ_ID.ClientID + "').focus();", true);

        }

        protected void btOK_Click(object sender, EventArgs e)
        {
            //2010.06.07 by 伊珊
            string ErrorString = "";

            string old_ID = "NULL";

            string old_Year = "NULL";

            if (Session["Mode"].ToString() == "UPDATE")
            {
                old_ID = Session["PKEY_MZ_ID"].ToString();

                old_Year = Session["PKEY_MZ_YEAR"].ToString();
            }
            string pkey_check;

            if (old_ID == TextBox_MZ_ID.Text.Trim() && old_Year == TextBox_MZ_YEAR.Text.Trim() && Session["Mode"].ToString() == "UPDATE")
                pkey_check = "0";
            else
                pkey_check = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_TP_PER WHERE MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "' AND MZ_YEAR='" + TextBox_MZ_YEAR.Text.Trim() + "'");

            if (pkey_check != "0")
            {
                ErrorString += "年度及人員重複" + "\\r\\n";
                TextBox_MZ_ID.BackColor = Color.Orange;
            }
            else
            {
                TextBox_MZ_ID.BackColor = Color.White;
            }
            //20141216 這應該是想做控制項檢核.但 DBDIC 又沒有資料
            //foreach (Object ob in Panel_PromotionPoint.Controls)
            //{
            //    if (ob is TextBox)
            //    {
            //        TextBox tbox = (TextBox)ob;

            //        string result = o_Check_Dbdic.ErrorString(tbox.ID, "A_TP_PER", tbox.Text);

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

            //        string result = o_Check_Dbdic.ErrorString(dlist.ID, "A_TP_PER", dlist.Text);

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
            new SqlParameter("MZ_ID",SqlDbType.NVarChar){Value = TextBox_MZ_ID.Text},
            new SqlParameter("MZ_NAME",SqlDbType.NVarChar){Value = TextBox_MZ_NAME.Text},
            new SqlParameter("MZ_AD",SqlDbType.NVarChar){Value = TextBox_MZ_AD.Text},
            new SqlParameter("MZ_UNIT",SqlDbType.NVarChar){Value = TextBox_MZ_UNIT.Text},
            new SqlParameter("MZ_EXAD",SqlDbType.NVarChar){Value = TextBox_MZ_EXAD.Text},
            new SqlParameter("MZ_EXUNIT",SqlDbType.NVarChar){Value = TextBox_MZ_EXUNIT.Text},
            new SqlParameter("MZ_OCCC",SqlDbType.NVarChar){Value = TextBox_MZ_OCCC.Text},
            new SqlParameter("MZ_SRANK",SqlDbType.NVarChar){Value = TextBox_MZ_SRANK.Text},
            new SqlParameter("MZ_TBDV",SqlDbType.NVarChar){Value = TextBox_MZ_TBDV.Text},
            new SqlParameter("MZ_SCEA",SqlDbType.Float){Value = string.IsNullOrEmpty(TextBox_MZ_SCEA.Text)?"0":TextBox_MZ_SCEA.Text},
            new SqlParameter("MZ_SCTA",SqlDbType.Float){Value = string.IsNullOrEmpty(TextBox_MZ_SCTA.Text)?"0":TextBox_MZ_SCTA.Text},
            new SqlParameter("MZ_SCYA",SqlDbType.Float){Value = string.IsNullOrEmpty(TextBox_MZ_SCYA.Text)?"0":TextBox_MZ_SCYA.Text},
            new SqlParameter("MZ_SCMA",SqlDbType.Float){Value = string.IsNullOrEmpty(TextBox_MZ_SCMA.Text)?"0":TextBox_MZ_SCMA.Text},
            new SqlParameter("MZ_SCPA",SqlDbType.Float){Value = string.IsNullOrEmpty(TextBox_MZ_SCPA.Text)?"0":TextBox_MZ_SCPA.Text},
            new SqlParameter("MZ_SCPH",SqlDbType.Float){Value = string.IsNullOrEmpty(TextBox_MZ_SCPH.Text)?"0":TextBox_MZ_SCPH.Text},
            new SqlParameter("MZ_SCSP",SqlDbType.Float){Value = string.IsNullOrEmpty(TextBox_MZ_SCSP.Text)?"0":TextBox_MZ_SCSP.Text},
            new SqlParameter("MZ_SCTF1",SqlDbType.Float){Value = string.IsNullOrEmpty(TextBox_MZ_SCTF1.Text)?"0":TextBox_MZ_SCTF1.Text},
            new SqlParameter("MZ_SCTF2",SqlDbType.Float){Value = string.IsNullOrEmpty(TextBox_MZ_SCTF2.Text)?"0":TextBox_MZ_SCTF2.Text},
            new SqlParameter("MZ_SCTF3",SqlDbType.Float){Value = string.IsNullOrEmpty(TextBox_MZ_SCTF3.Text)?"0":TextBox_MZ_SCTF3.Text},
            new SqlParameter("MZ_SCTF4",SqlDbType.Float){Value = string.IsNullOrEmpty(TextBox_MZ_SCTF4.Text)?"0":TextBox_MZ_SCTF4.Text},
            new SqlParameter("MZ_SCTN1",SqlDbType.Float){Value = string.IsNullOrEmpty(TextBox_MZ_SCTN1.Text)?"0":TextBox_MZ_SCTN1.Text},
            new SqlParameter("MZ_SCTN2",SqlDbType.Float){Value = string.IsNullOrEmpty(TextBox_MZ_SCTN2.Text)?"0":TextBox_MZ_SCTN2.Text},
            new SqlParameter("MZ_SCTN3",SqlDbType.Float){Value = Convert.DBNull},//TextBox_MZ_SCTN3.Text},
            new SqlParameter("MZ_SCTN4",SqlDbType.Float){Value = Convert.DBNull},//TextBox_MZ_SCTN4.Text},
            new SqlParameter("MZ_SCR1",SqlDbType.Float){Value = Convert.DBNull},//TextBox_MZ_SCR1.Text},
            new SqlParameter("MZ_TOT",SqlDbType.Float){Value = string.IsNullOrEmpty(TextBox_MZ_TOT.Text)?"0":TextBox_MZ_TOT.Text},
            new SqlParameter("MZ_TOT1",SqlDbType.Float){Value = string.IsNullOrEmpty(TextBox_MZ_TOT1.Text)?"0":TextBox_MZ_TOT1.Text},
            new SqlParameter("MZ_TOT2",SqlDbType.Float){Value = string.IsNullOrEmpty(TextBox_MZ_TOT2.Text)?"0":TextBox_MZ_TOT2.Text},
            new SqlParameter("MZ_TOT3",SqlDbType.Float){Value = string.IsNullOrEmpty(TextBox_MZ_TOT3.Text)?"0":TextBox_MZ_TOT3.Text},
            new SqlParameter("MZ_SCTF5",SqlDbType.Float){Value = Convert.DBNull},//TextBox_MZ_SCTF5.Text},
            new SqlParameter("MZ_SCTF6",SqlDbType.Float){Value = string.IsNullOrEmpty(TextBox_MZ_SCTF6.Text)?"0":TextBox_MZ_SCTF6.Text},
            new SqlParameter("MZ_YEAR",SqlDbType.NVarChar){Value = TextBox_MZ_YEAR.Text.Trim() },
            new SqlParameter("MDATE",SqlDbType.DateTime){Value = DateTime.Now},//TextBox_MZ_SCTF5.Text},
            new SqlParameter("MUSER",SqlDbType.NVarChar){Value = Session["ADPMZ_ID"].ToString()}
            };
            try
            {

                o_DBFactory.ABC_toTest.ExecuteNonQuery( Session["TP_PER_CMDSQL"].ToString(), parameterList);

                Response.Cookies["PKEY_MZ_ID"].Expires = DateTime.Now.AddYears(-1);

                if (Session["Mode"].ToString() == "INSERT")
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('新增成功');", true);
                    //2010.06.04 LOG紀錄 by伊珊
                    TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00002", TPMPermissions._boolTPM(), o_DBFactory.ABC_toTest.RegexSQL(Session["TP_PER_CMDSQL"].ToString(), parameterList));
                }
                else if (Session["Mode"].ToString() == "UPDATE")
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('修改成功')", true);
                    //2010.06.04 LOG紀錄 by伊珊
                    TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00003", TPMPermissions._boolTPM(), o_DBFactory.ABC_toTest.RegexSQL(Session["TP_PER_CMDSQL"].ToString(), parameterList));

                    TP_PER_MZ_ID = Session["TP_PER_MZ_ID"] as List<string>;
                    if (int.Parse(xcount.Text.Trim()) == 0 && TP_PER_MZ_ID.Count == 1)
                    {
                        btUpper.Enabled = false;
                        btNEXT.Enabled = false;
                    }
                    else if (int.Parse(xcount.Text.Trim()) == 0 && TP_PER_MZ_ID.Count > 1)
                    {
                        btUpper.Enabled = false;
                        btNEXT.Enabled = true;
                    }
                    else if (int.Parse(xcount.Text.Trim()) + 1 == TP_PER_MZ_ID.Count)
                    {
                        btUpper.Enabled = true;
                        btNEXT.Enabled = false;
                    }
                    else if (int.Parse(xcount.Text.Trim()) > 0 && int.Parse(xcount.Text.Trim()) + 1 < TP_PER_MZ_ID.Count)
                    {
                        btNEXT.Enabled = true;
                        btUpper.Enabled = true;
                    }
                }
                btUpdate.Enabled = true;
                btInsert.Enabled = true;
                btOK.Enabled = false;
                btCancel.Enabled = false;
                btDelete.Enabled = true;
                Session.Remove("Mode");
                A.controlEnable(ref this.Panel_PromotionPoint, false);
                btNameSearch.Enabled = false;
            }
            catch
            {
                if (Session["Mode"].ToString() == "INSERT")
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('新增失敗');", true);
                }
                else if (Session["Mode"].ToString() == "UPDATE")
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('修改失敗');location.href('Personal_PromotionPoint.aspx?XCOUNT=" + xcount.Text.Trim() + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "');", true);
                }
            }
        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            if (Session["Mode"].ToString() == "INSERT")
            {
                foreach (object dl in Panel_PromotionPoint.Controls)
                {

                    if (dl is TextBox)
                    {
                        TextBox tbox = dl as TextBox;
                        tbox.Text = "";
                    }
                }
                btUpdate.Enabled = false;
                btDelete.Enabled = false;
            }
            else if (Session["Mode"].ToString() == "UPDATE")
            {
                finddata(int.Parse(xcount.Text.Trim()));

                TP_PER_MZ_ID = Session["TP_PER_MZ_ID"] as List<string>;
                if (int.Parse(xcount.Text.Trim()) == 0 && TP_PER_MZ_ID.Count == 1)
                {
                    btUpper.Enabled = false;
                    btNEXT.Enabled = false;
                }
                else if (int.Parse(xcount.Text.Trim()) == 0 && TP_PER_MZ_ID.Count > 1)
                {
                    btUpper.Enabled = false;
                    btNEXT.Enabled = true;
                }
                else if (int.Parse(xcount.Text.Trim()) + 1 == TP_PER_MZ_ID.Count)
                {
                    btUpper.Enabled = true;
                    btNEXT.Enabled = false;
                }
                else if (int.Parse(xcount.Text.Trim()) > 0 && int.Parse(xcount.Text.Trim()) + 1 < TP_PER_MZ_ID.Count)
                {
                    btNEXT.Enabled = true;
                    btUpper.Enabled = true;
                }
                btUpdate.Enabled = true;
                btDelete.Enabled = true;
            }
            btInsert.Enabled = true;
            btOK.Enabled = false;
            btCancel.Enabled = false;
            A.controlEnable(ref this.Panel_PromotionPoint, false);
            btNameSearch.Enabled = false;

        }

        protected void btDelete_Click(object sender, EventArgs e)
        {
            string DeleteString = "DELETE FROM A_TP_PER WHERE MZ_ID = '" + TextBox_MZ_ID.Text.Trim() + "' AND MZ_YEAR='" + TextBox_MZ_YEAR.Text + "'";
            try
            {
                o_DBFactory.ABC_toTest.Edit_Data(DeleteString);
                TP_PER_MZ_ID = Session["TP_PER_MZ_ID"] as List<string>;
                TP_PER_MZ_ID.RemoveAt(int.Parse(xcount.Text.Trim()));

                if (TP_PER_MZ_ID.Count == 0)
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除成功！');location.href('Personal_PromotionPoint.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "');", true);
                    //2010.06.04 LOG紀錄 by伊珊
                    TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00004", TPMPermissions._boolTPM(), DeleteString);

                    btUpdate.Enabled = false;
                }
                else
                {
                    xcount.Text = "0";
                    finddata(int.Parse(xcount.Text));
                    if (TP_PER_MZ_ID.Count > 1)
                    {
                        btNEXT.Enabled = true;
                    }
                    btUpdate.Enabled = true;
                    Label1.Text = "第" + (int.Parse(xcount.Text) + 1).ToString() + "筆共" + TP_PER_MZ_ID.Count.ToString() + "筆";

                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除成功')", true);
                    //2010.06.04 LOG紀錄 by伊珊
                    TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00004", TPMPermissions._boolTPM(), DeleteString);

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
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('PersonalSearch_PromotionPoint.aspx?TableName=TP_PER&TPM_FION=" + Request.QueryString["TPM_FION"] + "','查詢','top=190,left=200,width=420,height=250,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }

        protected void btUpper_Click(object sender, EventArgs e)
        {
            if (int.Parse(xcount.Text) != 0)
            {
                xcount.Text = (Convert.ToInt32(xcount.Text) - 1).ToString();
                finddata(int.Parse(xcount.Text));
                if (int.Parse(xcount.Text) != TP_PER_MZ_ID.Count - 1)
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
            Label1.Text = "第" + (int.Parse(xcount.Text) + 1).ToString() + "筆共" + TP_PER_MZ_ID.Count.ToString() + "筆";
        }

        protected void btNEXT_Click(object sender, EventArgs e)
        {
            if (int.Parse(xcount.Text) == 0)
            {
                xcount.Text = (Convert.ToInt32(xcount.Text) + 1).ToString();
                btUpper.Enabled = true;
                finddata(int.Parse(xcount.Text));

                if (int.Parse(xcount.Text) == TP_PER_MZ_ID.Count - 1)
                {
                    btNEXT.Enabled = false;
                }
            }
            else
            {
                xcount.Text = (Convert.ToInt32(xcount.Text) + 1).ToString();
                finddata(int.Parse(xcount.Text));

                if (int.Parse(xcount.Text) == TP_PER_MZ_ID.Count - 1)
                {
                    btNEXT.Enabled = false;
                }
            }
            Label1.Text = "第" + (int.Parse(xcount.Text) + 1).ToString() + "筆共" + TP_PER_MZ_ID.Count.ToString() + "筆";
        }

        protected void Can_not_empty(TextBox tb1, object obj, string fieldname)
        {
            if (string.IsNullOrEmpty(tb1.Text.Trim()))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + tb1.ClientID + "').focus();$get('" + tb1.ClientID + "').focus();", true);
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('" + fieldname + "不可空白')", true);
            }
            else
            {
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

        private void Ktype_Cname_Check(string Cname, TextBox tb1, TextBox tb2, object obj)
        {

            tb2.Text = tb2.Text.ToUpper();
            //檢查代碼中文名稱是否存在
            if (string.IsNullOrEmpty(Cname))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('查無資料')", true);
                tb1.Text = string.Empty;
                tb2.Text = string.Empty;
             //   ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + tb2.ClientID + "').focus();$get('" + tb2.ClientID + "').focus();", true);
            }
            else
            {
                tb1.Text = Cname;
                if (obj is TextBox)
                {
               //     ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + (obj as TextBox).ClientID + "').focus();$get('" + (obj as TextBox).ClientID + "').focus();", true);
                }
                else if (obj is RadioButtonList)
                {
                //    (obj as RadioButtonList).Focus();
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
            string CName = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)='38213' UNION ALL SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)<>'38213') WHERE MZ_KCODE='" + TextBox_MZ_AD.Text.Trim().ToUpper() + "'");
            Ktype_Cname_Check(CName, TextBox_MZ_AD1, TextBox_MZ_AD, TextBox_MZ_UNIT);
        }

        protected void TextBox_MZ_UNIT_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_UNIT.Text, "25");
            Ktype_Cname_Check(CName, TextBox_MZ_UNIT1, TextBox_MZ_UNIT, TextBox_MZ_EXAD);
        }

        protected void TextBox_MZ_EXAD_TextChanged(object sender, EventArgs e)
        {
            string CName = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)='38213' UNION ALL SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)<>'38213') WHERE MZ_KCODE='" + TextBox_MZ_EXAD.Text.Trim().ToUpper() + "'");

            Ktype_Cname_Check(CName, TextBox_MZ_EXAD1, TextBox_MZ_EXAD, TextBox_MZ_EXUNIT);
        }

        protected void TextBox_MZ_EXUNIT_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_EXUNIT.Text, "25");

            Ktype_Cname_Check(CName, TextBox_MZ_EXUNIT1, TextBox_MZ_EXUNIT, TextBox_MZ_OCCC);
        }

        protected void TextBox_MZ_OCCC_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_OCCC.Text, "26");

            Ktype_Cname_Check(CName, TextBox_MZ_OCCC1, TextBox_MZ_OCCC, TextBox_MZ_SRANK);
        }

        protected void TextBox_MZ_SRANK_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_SRANK.Text, "09");

            Ktype_Cname_Check(CName, TextBox_MZ_SRANK1, TextBox_MZ_SRANK, TextBox_MZ_TBDV);
        }

        protected void TextBox_MZ_TBDV_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_TBDV.Text, "43");

            Ktype_Cname_Check(CName, TextBox_MZ_TBDV1, TextBox_MZ_TBDV, TextBox_MZ_TOT);
        }

        protected void btAD_Click(object sender, EventArgs e)
        {
            Session["KTYPE_CID"] = TextBox_MZ_AD.ClientID;
            Session["KTYPE_CID1"] = TextBox_MZ_AD1.ClientID;

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_Ktype_Search.aspx?CID_NAME=AD&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin','top=190,left=200,width=420,height=450,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }

        protected void btUNIT_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_MZ_EXUNIT, TextBox_MZ_EXUNIT1, "25");
        }

        protected void btEXAD_Click(object sender, EventArgs e)
        {
            Session["KTYPE_CID"] = TextBox_MZ_EXAD.ClientID;
            Session["KTYPE_CID1"] = TextBox_MZ_EXAD1.ClientID;

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_Ktype_Search.aspx?CID_NAME=AD&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin','top=190,left=200,width=420,height=450,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }

        protected void btEXUNIT_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_MZ_EXUNIT, TextBox_MZ_EXUNIT1, "25");
        }

        protected void btOCCC_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_MZ_OCCC, TextBox_MZ_OCCC1, "26");
        }

        protected void btSRANK_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_MZ_SRANK, TextBox_MZ_SRANK1, "09");
        }

        protected void btTBDV_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_MZ_TBDV, TextBox_MZ_TBDV1, "43");
        }

        protected void CV_AD_ServerValidate(object source, ServerValidateEventArgs args)
        {

        }

        protected void CV_EXAD_ServerValidate(object source, ServerValidateEventArgs args)
        {

        }

        protected void CV_UNIT_ServerValidate(object source, ServerValidateEventArgs args)
        {

        }

        protected void CV_EXUNIT_ServerValidate(object source, ServerValidateEventArgs args)
        {

        }

        protected void CV_OCCC_ServerValidate(object source, ServerValidateEventArgs args)
        {

        }

        protected void CV_TBDV_ServerValidate(object source, ServerValidateEventArgs args)
        {

        }

        protected void CV_SRANK_ServerValidate(object source, ServerValidateEventArgs args)
        {

        }

        protected void btNameSearch_Click(object sender, EventArgs e)
        {

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('PersonalSearchIDwithNAME.aspx?TableName=TP_PER&TPM_FION=" + Request.QueryString["TPM_FION"] + "','查詢','top=190,left=200,width=750,height=500,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }

        protected void TextBox_MZ_ID_TextChanged(object sender, EventArgs e)
        {
            TextBox_MZ_ID.Text = TextBox_MZ_ID.Text.Trim().ToUpper();
            preLoad(TextBox_MZ_ID.Text.Trim());
        }

        protected void TextBox_MZ_TOT_TextChanged(object sender, EventArgs e)
        {
            string MZ_TBDV = o_A_DLBASE.return_Rank(TextBox_MZ_ID.Text);

            MZ_TBDV = string.IsNullOrEmpty(MZ_TBDV) ? "0" : MZ_TBDV;

            if (int.Parse(MZ_TBDV) < 7) //第六序列以上
            {
                string TOT2 = (float.Parse(string.IsNullOrEmpty(TextBox_MZ_TOT.Text) ? "0" : TextBox_MZ_TOT.Text) * 0.4).ToString();

                TextBox_MZ_TOT2.Text = float.Parse(TOT2) > 40 ? "40" : TOT2;
            }
            ////2013/05/20 立廷
            else if (int.Parse(MZ_TBDV) == 11)//第十一序列
            {
                string TOT2 = (float.Parse(string.IsNullOrEmpty(TextBox_MZ_TOT.Text) ? "0" : TextBox_MZ_TOT.Text) * 0.8).ToString();

                TextBox_MZ_TOT2.Text = TOT2;

            }

                ////2013/05/20

            else//第7~10序列以上
            {
                string TOT2 = (float.Parse(string.IsNullOrEmpty(TextBox_MZ_TOT.Text) ? "0" : TextBox_MZ_TOT.Text) * 0.8).ToString();

                TextBox_MZ_TOT2.Text = float.Parse(TOT2) > 80 ? "80" : TOT2;
            }
        }

        protected void COUNT_TOT3()
        {
            TextBox_MZ_TOT3.Text = string.Empty;
            TextBox_MZ_TOT3.Text = (float.Parse(string.IsNullOrEmpty(TextBox_MZ_SCTF4.Text) ? "0" : TextBox_MZ_SCTF4.Text) + float.Parse(string.IsNullOrEmpty(TextBox_MZ_SCTF2.Text) ? "0" : TextBox_MZ_SCTF2.Text) + float.Parse(string.IsNullOrEmpty(TextBox_MZ_SCTN1.Text) ? "0" : TextBox_MZ_SCTN1.Text) + float.Parse(string.IsNullOrEmpty(TextBox_MZ_SCTN2.Text) ? "0" : TextBox_MZ_SCTN2.Text) + float.Parse(string.IsNullOrEmpty(TextBox_MZ_SCTF6.Text) ? "0" : TextBox_MZ_SCTF6.Text)).ToString();
        }

        protected void COUNT_SRC1()
        {
            TextBox_MZ_SCR1.Text = string.Empty;
            TextBox_MZ_SCR1.Text = (float.Parse(string.IsNullOrEmpty(TextBox_MZ_SCEA.Text) ? "0" : TextBox_MZ_SCEA.Text) + float.Parse(string.IsNullOrEmpty(TextBox_MZ_SCTA.Text) ? "0" : TextBox_MZ_SCTA.Text) + float.Parse(string.IsNullOrEmpty(TextBox_MZ_SCYA.Text) ? "0" : TextBox_MZ_SCYA.Text) + float.Parse(string.IsNullOrEmpty(TextBox_MZ_SCMA.Text) ? "0" : TextBox_MZ_SCMA.Text) + float.Parse(string.IsNullOrEmpty(TextBox_MZ_SCPA.Text) ? "0" : TextBox_MZ_SCPA.Text) + float.Parse(string.IsNullOrEmpty(TextBox_MZ_SCPH.Text) ? "0" : TextBox_MZ_SCPH.Text) + float.Parse(string.IsNullOrEmpty(TextBox_MZ_SCSP.Text) ? "0" : TextBox_MZ_SCSP.Text) + float.Parse(string.IsNullOrEmpty(TextBox_MZ_SCTF1.Text) ? "0" : TextBox_MZ_SCTF1.Text) + float.Parse(string.IsNullOrEmpty(TextBox_MZ_SCTF3.Text) ? "0" : TextBox_MZ_SCTF3.Text)).ToString();
        }

        protected void COUNT_TOT1()
        {
            TextBox_MZ_TOT1.Text = string.Empty;
            TextBox_MZ_TOT1.Text = (float.Parse(string.IsNullOrEmpty(TextBox_MZ_TOT3.Text) ? "0" : TextBox_MZ_TOT3.Text) + float.Parse(string.IsNullOrEmpty(TextBox_MZ_TOT2.Text) ? "0" : TextBox_MZ_TOT2.Text) + float.Parse(string.IsNullOrEmpty(TextBox_MZ_SCR1.Text) ? "0" : TextBox_MZ_SCR1.Text)).ToString();
        }

        protected void TextBox_MZ_SCTF4_TextChanged(object sender, EventArgs e)
        {
            COUNT_TOT3();
            COUNT_TOT1();
            TextBox_MZ_SCTF2.Focus();
        }

        protected void TextBox_MZ_SCTF2_TextChanged(object sender, EventArgs e)
        {
            COUNT_TOT3();
            COUNT_TOT1();
            TextBox_MZ_SCTN1.Focus();
        }

        protected void TextBox_MZ_SCTN1_TextChanged(object sender, EventArgs e)
        {
            COUNT_TOT3();
            COUNT_TOT1();
            TextBox_MZ_SCTN2.Focus();
        }

        protected void TextBox_MZ_SCTN2_TextChanged(object sender, EventArgs e)
        {
            COUNT_TOT3();
            COUNT_TOT1();
            TextBox_MZ_SCTF6.Focus();
        }

        protected void TextBox_MZ_SCTF6_TextChanged(object sender, EventArgs e)
        {
            COUNT_TOT3();
            COUNT_TOT1();
            TextBox_MZ_SCEA.Focus();
        }

        protected void TextBox_MZ_SCEA_TextChanged(object sender, EventArgs e)
        {
            COUNT_SRC1();
            COUNT_TOT1();
            TextBox_MZ_SCTA.Focus();
        }

        protected void TextBox_MZ_SCTA_TextChanged(object sender, EventArgs e)
        {
            COUNT_SRC1();
            COUNT_TOT1();
            TextBox_MZ_SCYA.Focus();
        }

        protected void TextBox_MZ_SCYA_TextChanged(object sender, EventArgs e)
        {
            COUNT_SRC1();
            COUNT_TOT1();
            TextBox_MZ_SCMA.Focus();
        }

        protected void TextBox_MZ_SCMA_TextChanged(object sender, EventArgs e)
        {
            COUNT_SRC1();
            COUNT_TOT1();
            TextBox_MZ_SCPA.Focus();
        }

        protected void TextBox_MZ_SCPA_TextChanged(object sender, EventArgs e)
        {
            COUNT_SRC1();
            COUNT_TOT1();
            TextBox_MZ_SCPH.Focus();
        }

        protected void TextBox_MZ_SCPH_TextChanged(object sender, EventArgs e)
        {
            COUNT_SRC1();
            COUNT_TOT1();
            TextBox_MZ_SCSP.Focus();
        }

        protected void TextBox_MZ_SCSP_TextChanged(object sender, EventArgs e)
        {
            COUNT_SRC1();
            COUNT_TOT1();
            TextBox_MZ_SCTF1.Focus();
        }

        protected void TextBox_MZ_SCTF1_TextChanged(object sender, EventArgs e)
        {
            COUNT_SRC1();
            COUNT_TOT1();
            TextBox_MZ_SCTF3.Focus();
        }

        protected void TextBox_MZ_SCTF3_TextChanged(object sender, EventArgs e)
        {
            COUNT_SRC1();
            COUNT_TOT1();
            TextBox_MZ_SCTF3.Focus();
        }

        protected void DropDownList_AD_SelectedIndexChanged(object sender, EventArgs e)
        {
            Panel_Excel_ModalPopupExtender.Show();
            txt_Report_Year.Text = (DateTime.Now.Year - 1911).ToString();
        }

        protected void Button_Excel_Click(object sender, EventArgs e)
        {
            //JUMP Button_PBExcel_Click
        }
        //EXCEL
        public override void VerifyRenderingInServerForm(Control control)
        {
            // '處理'GridView' 的控制項 'GridView' 必須置於有 runat=server 的表單標記之中  
        }

        protected void Button_PBExcel_Click(object sender, EventArgs e)
        {
            string Year = string.IsNullOrEmpty(txt_Report_Year.Text) ? (DateTime.Now.Year - 1911).ToString() : txt_Report_Year.Text;

            if (RadioButtonList_Type.SelectedValue == "1")
            {
                string strSQL = " SELECT MZ_ID,dbo.to_number(CASE WHEN MZ_TBDV IS NULL THEN '999' WHEN MZ_TBDV='Z99' THEN '999' ELSE MZ_TBDV END) AS TBDV,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE = MZ_AD AND MZ_KTYPE = '04') MZ_AD,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE = MZ_UNIT AND MZ_KTYPE = '25') MZ_UNIT,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE = dbo.LPAD(MZ_TBDV,3,'0') AND MZ_KTYPE = '43')  MZ_TBDV,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE = MZ_SRANK AND MZ_KTYPE = '09')MZ_SRANK,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE = MZ_OCCC AND MZ_KTYPE = '26')MZ_OCCC,MZ_NAME,MZ_SCTF1,MZ_SCTF3,MZ_SCTF4,MZ_SCTF2,MZ_SCTN1,MZ_SCTN2,MZ_TOT3,MZ_SCTF6,MZ_SCR1,MZ_TOT,MZ_TOT2,MZ_TOT1 FROM A_TP_PER WHERE 1=1 ";

                if (!string.IsNullOrEmpty(DropDownList_AD.SelectedValue))
                {
                    strSQL += "  AND MZ_AD = '" + DropDownList_AD.SelectedValue + "'";
                }


                if (!string.IsNullOrEmpty(DropDownList_UNIT.SelectedValue))
                {
                    strSQL += "  AND MZ_UNIT = '" + DropDownList_UNIT.SelectedValue + "'";
                }

                if (!string.IsNullOrEmpty(txt_Report_TBDV.Text))
                {
                    strSQL += string.Format(" AND MZ_TBDV='{0}'", txt_Report_TBDV.Text);
                }

                strSQL += " AND MZ_YEAR='" + Year + "'";

                strSQL += " ORDER BY TBDV ASC,MZ_TOT1 DESC";

                DataTable dt = new DataTable();
                dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "E1");
                if (dt.Rows.Count == 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "alert('該條件下無資料！')", true);
                }
                else
                {
                    GridView_Excel1.DataSource = dt;
                    GridView_Excel1.DataBind();
                    o_str.toexcel(this, GridView_Excel1);
                }
            }
            else if (RadioButtonList_Type.SelectedValue == "2")
            {
                string strSQL = " SELECT MZ_ID,dbo.to_number(CASE WHEN MZ_TBDV IS NULL THEN '999' WHEN MZ_TBDV='Z99' THEN '999' ELSE MZ_TBDV END) AS TBDV,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE = dbo.LPAD(MZ_TBDV,3,'0') AND MZ_KTYPE = '43')  MZ_TBDV,MZ_SCEA,MZ_SCTA,MZ_SCYA,MZ_SCMA,MZ_SCPA,MZ_SCTF4,MZ_SCTF2,MZ_SCTN1,MZ_SCTN2,MZ_SCTF6,MZ_SCR1 FROM A_TP_PER WHERE 1=1 ";

                if (!string.IsNullOrEmpty(DropDownList_AD.SelectedValue))
                {
                    strSQL += "  AND MZ_AD = '" + DropDownList_AD.SelectedValue + "'";
                }


                if (!string.IsNullOrEmpty(DropDownList_UNIT.SelectedValue))
                {
                    strSQL += "  AND MZ_UNIT = '" + DropDownList_UNIT.SelectedValue + "'";
                }

                if (!string.IsNullOrEmpty(txt_Report_TBDV.Text))
                {
                    strSQL += string.Format(" AND MZ_TBDV='{0}'", txt_Report_TBDV.Text);
                }

                strSQL += " AND MZ_YEAR='" + Year + "'";

                strSQL += " ORDER BY TBDV ASC,MZ_TOT1 DESC";

                DataTable dt = new DataTable();
                dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "E2");
                if (dt.Rows.Count == 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "alert('該單位無資料')", true);
                }
                else
                {
                    GridView_Excel2.DataSource = dt;
                    GridView_Excel2.DataBind();
                    o_str.toexcel(this, GridView_Excel2);
                }
            }
        }

        protected void DropDownList_UNIT_DataBound(object sender, EventArgs e)
        {
            DropDownList_UNIT.Items.Insert(0, new ListItem(" ", ""));
        }

        protected void DropDownList_AD_DataBound(object sender, EventArgs e)
        {
            DropDownList_AD.Items.Insert(0, new ListItem(" ", ""));
        }

        
    }
}
