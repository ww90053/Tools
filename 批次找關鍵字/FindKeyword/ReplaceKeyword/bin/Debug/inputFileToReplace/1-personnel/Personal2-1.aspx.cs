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
using Microsoft.VisualBasic;
using System.Drawing;
using System.IO;
using System.Text;
using NPOI.HSSF.UserModel;
using TPPDDB.Helpers;

namespace TPPDDB._1_personnel
{
    public partial class personal2_1 : System.Web.UI.Page
    {

        List<string> PRKB_MZ_ID = new List<string>();
        List<string> PRKB_MZ_NO = new List<string>();
        List<string> PRKB_MZ_PRCT = new List<string>();
        bool checkPRRST = true;
        //bool checkPRK1 = true;

        PersonalService PersonalService = new PersonalService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                A.check_power();
                ///群組權限
                ViewState["A_strGID"] = o_a_Function.strGID(Session["ADPMZ_ID"].ToString());

                //移除下載資訊
                Session.Remove("msData");
                Session.Remove("fileName");
            }

            
           

            A.set_Panel_EnterToTAB(ref this.Panel_Personal2);
           
            ViewState["tbID"] = Session["PRKB_ID"];

            if (ViewState["tbID"] != null)
                ViewState["tbID2"] = Session["PRKB_ID"];

            Session.Remove("PRKB_ID");

            ViewState["tbNAME"] = Session["PRKB_NAME"];

            if (ViewState["tbNAME"] != null)
                ViewState["tbNAME2"] = Session["PRKB_NAME"];

            Session.Remove("PRKB_NAME");

            //HttpCookie PRKB_ID_Cookie = new HttpCookie("PRKB_ID");
            //PRKB_ID_Cookie = Request.Cookies["PRKB_ID"];

            //if (PRKB_ID_Cookie == null)
            //{
            //    ViewState["tbID"] = null;
            //    Response.Cookies["PRKB_ID"].Expires = DateTime.Now.AddYears(-1);
            //}
            //else
            //{
            //    ViewState["tbID"] = TPMPermissions._strDecod(PRKB_ID_Cookie.Value.ToString());
            //    Response.Cookies["PRKB_ID"].Expires = DateTime.Now.AddYears(-1);
            //}

            //HttpCookie PRKB_NAME_Cookie = new HttpCookie("PRKB_NAME");
            //PRKB_NAME_Cookie = Request.Cookies["PRKB_NAME"];

            //if (PRKB_NAME_Cookie == null)
            //{
            //    ViewState["tbNAME"] = null;
            //    Response.Cookies["PRKB_NAME"].Expires = DateTime.Now.AddYears(-1);
            //}
            //else
            //{
            //    ViewState["tbNAME"] = TPMPermissions._strDecod(PRKB_NAME_Cookie.Value.ToString());
            //    Response.Cookies["PRKB_NAME"].Expires = DateTime.Now.AddYears(-1);
            //}

            ////查詢ID
            //HttpCookie Personal_PRKB_Detail_ID_Cookie1 = new HttpCookie("Personal_PRKB_Detail_ID");
            //Personal_PRKB_Detail_ID_Cookie1 = Request.Cookies["Personal_PRKB_Detail_ID"];

            //if (Personal_PRKB_Detail_ID_Cookie1 == null)
            //{
            //    ViewState["MZ_ID1"] = null;
            //    Response.Cookies["Personal_PRKB_Detail_ID"].Expires = DateTime.Now.AddYears(-1);
            //}
            //else
            //{
            //    ViewState["MZ_ID1"] = TPMPermissions._strDecod(Personal_PRKB_Detail_ID_Cookie1.Value.ToString());
            //    Response.Cookies["Personal_PRKB_Detail_ID"].Expires = DateTime.Now.AddYears(-1);
            //}

            //HttpCookie Personal_PRKB_Detail_PRCT_Cookie1 = new HttpCookie("Personal_PRKB_Detail_PRCT");
            //Personal_PRKB_Detail_PRCT_Cookie1 = Request.Cookies["Personal_PRKB_Detail_PRCT"];

            //if (Personal_PRKB_Detail_PRCT_Cookie1 == null)
            //{
            //    ViewState["MZ_PRCT1"] = null;
            //    Response.Cookies["Personal_PRKB_Detail_PRCT"].Expires = DateTime.Now.AddYears(-1);
            //}
            //else
            //{
            //    ViewState["MZ_PRCT1"] = TPMPermissions._strDecod(Personal_PRKB_Detail_PRCT_Cookie1.Value.ToString());
            //    Response.Cookies["Personal_PRKB_Detail_PRCT"].Expires = DateTime.Now.AddYears(-1);
            //}

            ViewState["Detail_NO"] = Request["Detail_NO"];
            ViewState["cbAD"] = Request["AD"];
            ViewState["cbUNIT"] = Request["UNIT"];
            ViewState["tbNO"] = Request["NO"];
            ViewState["Personl_PRKB_SWT4_MZ_NO"] = Request["Personl_PRKB_SWT4_MZ_NO"];
            ViewState["MZ_ID"] = Session["PersonalSearchIDwithNAME_MZ_ID"];
            Session.Remove("PersonalSearchIDwithNAME_MZ_ID");
            ViewState["MZ_PRCT1"] = Session["Personal_PRKB_Detail_PRCT"];
            Session.Remove("Personal_PRKB_Detail_PRCT");
            ViewState["MZ_ID1"] = Session["Personal_PRKB_Detail_ID"];
            Session.Remove("Personal_PRKB_Detail_ID");

            //HttpCookie PRKB_MZ_ID_Cookie = new HttpCookie("PersonalSearchIDwithNAME_MZ_ID");
            //PRKB_MZ_ID_Cookie = Request.Cookies["PersonalSearchIDwithNAME_MZ_ID"];

            //if (PRKB_MZ_ID_Cookie == null)
            //{
            //    ViewState["MZ_ID"] = null;
            //    Response.Cookies["PersonalSearchIDwithNAME_MZ_ID"].Expires = DateTime.Now.AddYears(-1);
            //}
            //else
            //{
            //    ViewState["MZ_ID"] = TPMPermissions._strDecod(PRKB_MZ_ID_Cookie.Value.ToString());
            //    Response.Cookies["PersonalSearchIDwithNAME_MZ_ID"].Expires = DateTime.Now.AddYears(-1);
            //}

            ViewState["XCOUNT"] = Request["XCOUNT"];

            if (!Page.IsPostBack)
            {
                TextBox_MZ_AD1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_EXAD1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_EXUNIT1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_OCCC1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_RANK_1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_RANK1_1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_SRANK1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_UNIT1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_PROLNO1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_PCODEM1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_PRK_1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_PRK1_1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_PRRST1.Attributes.Add("onkeydown", "javascript:return false;");

                bool Change = false;

                if (ViewState["XCOUNT"] != null)
                {
                    finddata(int.Parse(ViewState["XCOUNT"].ToString()));
                    xcount.Text = ViewState["XCOUNT"].ToString();
                    if (int.Parse(ViewState["XCOUNT"].ToString()) == 0 && PRKB_MZ_ID.Count > 1)
                    {
                        btNEXT.Enabled = true;
                        btUpper.Enabled = false;
                    }
                    else if (int.Parse(ViewState["XCOUNT"].ToString()) > 0 && int.Parse(ViewState["XCOUNT"].ToString()) < PRKB_MZ_ID.Count - 1)
                    {
                        btNEXT.Enabled = true;
                        btUpper.Enabled = true;
                    }
                    else if (int.Parse(ViewState["XCOUNT"].ToString()) == PRKB_MZ_ID.Count - 1)
                    {
                        btUpper.Enabled = true;
                        btNEXT.Enabled = false;
                    }
                    else
                    {
                        btNEXT.Enabled = false;
                        btUpper.Enabled = false;
                    }

                    if (PRKB_MZ_ID.Count == 0)
                    {
                        Label1.Visible = false;
                        btDetail.Visible = false;
                    }
                    else
                    {
                        Label1.Visible = true;
                        Label1.Text = "第" + (int.Parse(xcount.Text) + 1).ToString() + "筆共" + PRKB_MZ_ID.Count.ToString() + "筆";
                    }
                    btUpdate.Enabled = true;
                    btInsert.Enabled = true;
                    btOK.Enabled = false;
                    btCancel.Enabled = false;
                    Button2.Enabled = false;
                    btDelete.Enabled = true;
                    Change = false;
                }

                if (ViewState["Personl_PRKB_SWT4_MZ_NO"] != null)
                {
                    string strSQL = "SELECT MZ_ID,MZ_NO,MZ_PRCT FROM A_PRKB WHERE MZ_NO='" + ViewState["Personl_PRKB_SWT4_MZ_NO"].ToString() + "' ORDER BY MZ_ID";

                    PRKB_MZ_ID = o_DBFactory.ABC_toTest.DataListArray(strSQL, "MZ_ID");
                    PRKB_MZ_NO = o_DBFactory.ABC_toTest.DataListArray(strSQL, "MZ_NO");
                    PRKB_MZ_PRCT = o_DBFactory.ABC_toTest.DataListArray(strSQL, "MZ_PRCT");
                    Session["PRKB_MZ_ID"] = PRKB_MZ_ID;
                    Session["PRKB_MZ_NO"] = PRKB_MZ_NO;
                    Session["PRKB_MZ_PRCT"] = PRKB_MZ_PRCT;

                    if (PRKB_MZ_ID.Count == 0)
                    {
                        Response.Write(@"<script language=javascript>window.alert('查無資料！');location.href('Personal2-1.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "');</script>");
                    }
                    else if (PRKB_MZ_ID.Count == 1)
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

                    if (PRKB_MZ_ID.Count == 0)
                    {
                        Label1.Visible = false;
                        btDetail.Visible = false;
                    }
                    else
                    {//
                        Label1.Visible = true;
                        btDetail.Visible = true;
                        Label1.Text = "第" + (int.Parse(xcount.Text) + 1).ToString() + "筆共" + PRKB_MZ_ID.Count.ToString() + "筆";
                    }

                    Change = false;
                }
                //
                if (ViewState["MZ_ID1"] != null)
                {
                    PRKB_MZ_ID = Session["PRKB_MZ_ID"] as List<string>;
                    PRKB_MZ_NO = Session["PRKB_MZ_NO"] as List<string>;
                    PRKB_MZ_PRCT = Session["PRKB_MZ_PRCT"] as List<string>;

                    PRKB_MZ_ID.Clear();
                    PRKB_MZ_NO.Clear();
                    PRKB_MZ_PRCT.Clear();

                    PRKB_MZ_ID.Insert(0, ViewState["MZ_ID1"].ToString());
                    PRKB_MZ_NO.Insert(0, ViewState["Detail_NO"].ToString());
                    PRKB_MZ_PRCT.Insert(0, ViewState["MZ_PRCT1"].ToString());

                    Session["PRKB_MZ_ID"] = PRKB_MZ_ID;
                    Session["PRKB_MZ_NO"] = PRKB_MZ_NO;
                    Session["PRKB_MZ_PRCT"] = PRKB_MZ_PRCT;

                    xcount.Text = "0";
                    finddata(int.Parse(xcount.Text));
                    btInsert.Enabled = true;
                    btUpdate.Enabled = true;
                    btOK.Enabled = false;
                    btDelete.Enabled = true;
                    btCancel.Enabled = false;
                    btNEXT.Enabled = false;
                    btUpper.Enabled = false;

                }
                else if (ViewState["tbID"] != null)
                {
                    string strSQL = "SELECT MZ_ID,MZ_NO,MZ_PRCT,dbo.to_number(CASE WHEN MZ_TBDV IS NULL THEN '999' WHEN MZ_TBDV='Z99' THEN '999' ELSE MZ_TBDV END) AS TBDV FROM A_PRKB WHERE 1=1";
                    ////?
                    if (ViewState["tbID"].ToString() != "")
                    {
                        strSQL = strSQL + " AND MZ_ID='" + ViewState["tbID"].ToString() + "'";
                    }

                    if (ViewState["tbNAME"].ToString() != "")
                    {
                        strSQL = strSQL + " AND MZ_NAME='" + ViewState["tbNAME"].ToString() + "'";
                    }
                    ////?
                    if (ViewState["cbAD"].ToString() != "" && ViewState["cbUNIT"].ToString() == "")
                    {
                        strSQL = strSQL + " AND (MZ_AD='" + ViewState["cbAD"].ToString() + "' OR MZ_EXAD='" + ViewState["cbAD"].ToString() + "')";
                    }
                    else if (ViewState["cbAD"].ToString() != "" && ViewState["cbUNIT"].ToString() != "")
                    {
                        strSQL = strSQL + " AND ((MZ_EXAD ='" + ViewState["cbAD"].ToString() + "' AND MZ_EXUNIT='" + ViewState["cbUNIT"].ToString() + "') OR (MZ_AD ='" + ViewState["cbAD"].ToString() + "' AND MZ_UNIT='" + ViewState["cbUNIT"].ToString() + "'))";
                    }

                    if (ViewState["tbNO"].ToString() != "")
                    {
                        strSQL = strSQL + " AND MZ_NO='" + ViewState["tbNO"].ToString() + "'";
                    }
                    //20140407
                    //20140606
                    //switch (Session["Personal_strGID"].ToString())
                    switch (ViewState["A_strGID"].ToString())                   
                    {
                        case "A":
                        case "B":
                        case "C":
                            break;
                        case "D":
                        case "E":
                            strSQL += "AND MUSER='" + Session["ADPMZ_ID"].ToString() + "'";
                            break;
                    }

                    strSQL = strSQL + " ORDER BY MZ_AD,MZ_UNIT,MZ_ID ";

                    PRKB_MZ_ID = o_DBFactory.ABC_toTest.DataListArray(strSQL, "MZ_ID");
                    PRKB_MZ_NO = o_DBFactory.ABC_toTest.DataListArray(strSQL, "MZ_NO");
                    PRKB_MZ_PRCT = o_DBFactory.ABC_toTest.DataListArray(strSQL, "MZ_PRCT");
                    Session["PRKB_MZ_ID"] = PRKB_MZ_ID;
                    Session["PRKB_MZ_NO"] = PRKB_MZ_NO;
                    Session["PRKB_MZ_PRCT"] = PRKB_MZ_PRCT;

                    if (PRKB_MZ_ID.Count == 0)
                    {

                        Response.Write(@"<script language=javascript>window.alert('查無資料！');location.href('Personal2-1.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "');</script>");

                    }
                    else if (PRKB_MZ_ID.Count == 1)
                    {
                        xcount.Text = "0";
                        finddata(int.Parse(xcount.Text));
                        // ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_PRKB_Detail.aspx?NO=" + TextBox_MZ_NO.Text.Trim() + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "','查詢','');", true);
                    }
                    else
                    {
                        btNEXT.Enabled = true;
                        xcount.Text = "0";
                        finddata(int.Parse(xcount.Text));
                        //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_PRKB_Detail.aspx?NO=" + TextBox_MZ_NO.Text.Trim() + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "','查詢','');", true);
                    }

                    if (PRKB_MZ_ID.Count == 0)
                    {
                        Label1.Visible = false;
                        btDetail.Visible = false;
                    }
                    else
                    {
                        Label1.Visible = true;
                        btDetail.Visible = true;
                        Label1.Text = "第" + (int.Parse(xcount.Text) + 1).ToString() + "筆共" + PRKB_MZ_ID.Count.ToString() + "筆";
                    }

                    Change = false;
                }

                if (ViewState["MZ_ID"] != null)
                {
                    if (Session["PRKB_OLD_MZ_NO"] != null && Session["PRKB_OLD_MZ_ID"] != null)
                    {
                        TextBox_MZ_NO.Text = Session["PRKB_OLD_MZ_NO"].ToString();

                        string OldString = "SELECT * FROM A_PRKB WHERE MZ_NO='" + Session["PRKB_OLD_MZ_NO"].ToString() +
                                                                "' AND MZ_ID='" + Session["PRKB_OLD_MZ_ID"].ToString() + "'";
                        DataTable Olddt = new DataTable();
                        Olddt = o_DBFactory.ABC_toTest.Create_Table(OldString, "GET");

                        if (Olddt.Rows.Count > 0)
                        {
                            TextBox_MZ_PRK1.Text = Olddt.Rows[0]["MZ_PRK1"].ToString().Trim();
                            TextBox_MZ_PRK.Text = Olddt.Rows[0]["MZ_PRK"].ToString().Trim();
                            TextBox_MZ_PRRST.Text = Olddt.Rows[0]["MZ_PRRST"].ToString().Trim();
                            TextBox_MZ_PROLNO.Text = Olddt.Rows[0]["MZ_PROLNO"].ToString().Trim();
                            TextBox_MZ_PROLNO2.Text = Olddt.Rows[0]["MZ_PROLNO2"].ToString().Trim();
                            TextBox_MZ_AD.Text = Olddt.Rows[0]["MZ_AD"].ToString().Trim();
                            TextBox_MZ_UNIT.Text = Olddt.Rows[0]["MZ_UNIT"].ToString().Trim();
                            DropDownList_MZ_POLK.SelectedValue = string.IsNullOrEmpty(Olddt.Rows[0]["MZ_POLK"].ToString().Trim()) ? "" : Olddt.Rows[0]["MZ_POLK"].ToString().Trim();
                            DropDownList_MZ_PCODE.SelectedValue = string.IsNullOrEmpty(Olddt.Rows[0]["MZ_PCODE"].ToString().Trim()) ? "" : Olddt.Rows[0]["MZ_PCODE"].ToString().Trim();
                            TextBox_MZ_MEMO.Text = Olddt.Rows[0]["MZ_MEMO"].ToString().Trim();
                            DropDownList_MZ_SWT3.SelectedValue = string.IsNullOrEmpty(Olddt.Rows[0]["MZ_SWT3"].ToString().Trim()) ? "" : Olddt.Rows[0]["MZ_SWT3"].ToString().Trim();
                            TextBox_MZ_SWT4.Text = Olddt.Rows[0]["MZ_SWT4"].ToString().Trim();
                            TextBox_MZ_SWT1.Text = Olddt.Rows[0]["MZ_SWT1"].ToString().Trim();
                            TextBox_MZ_PCODEM.Text = Olddt.Rows[0]["MZ_PCODEM"].ToString().Trim();
                            TextBox_MZ_REMARK.Text = Olddt.Rows[0]["MZ_REMARK"].ToString().Trim();
                            TextBox_MZ_EXAD.Text = Olddt.Rows[0]["MZ_EXAD"].ToString().Trim();
                            TextBox_MZ_EXUNIT.Text = Olddt.Rows[0]["MZ_EXUNIT"].ToString().Trim();
                            TextBox_MZ_PRCT.Text = Olddt.Rows[0]["MZ_PRCT"].ToString().Trim();

                            TextBox_MZ_PROLNO1.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_PRONAME AS MZ_KCHI FROM A_PROLNO WHERE MZ_PROLNO='" + TextBox_MZ_PROLNO.Text + "'");
                            TextBox_MZ_PCODEM1.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_PNAME FROM A_POLNUM WHERE MZ_POLNO='" + TextBox_MZ_PCODEM.Text + "'");
                            TextBox_MZ_PRK_1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_PRK.Text, "22");
                        }
                    }
                    else
                    {
                        TextBox_MZ_NO.Text = Session["PRKB_MZ_NO"].ToString();
                    }

                    Change = true;
                    DropDownList_MZ_POLK.Enabled = false;

                    preLoad(ViewState["MZ_ID"].ToString());
                    //20141210
                    //TextBox_MZ_SWT1.Text = string.Empty;
                    //TextBox_MZ_SWT4.Text = string.Empty;
                }

                A.controlEnable(ref this.Panel_Personal2, Change);
                //20141115
                TextBox_MZ_NAME.Enabled = false;
                chk_TPMGroup();
            }
        }
        /// <summary>
        /// 群組權限
        /// </summary>
        protected void chk_TPMGroup()
        {
            switch (ViewState["A_strGID"].ToString())
            {
                case "A":
                case "B":
                case "C":

                    break;
               
                case "D":                   
                case "E":
                default:
                  
                    Button_MZ_SWT4.Visible = false;
                    btReplace.Visible = false;
                    btAddNote.Visible = false;
                    break;
            }
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
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + (obj as RadioButtonList).ClientID + "').focus();$get('" + (obj as RadioButtonList).ClientID + "').focus();", true);
                }
            }
        }

        protected void preLoad(string ID)
        {
            btInsert.Enabled = false;
            btUpdate.Enabled = false;
            btOK.Enabled = true;
            Button2.Enabled = true;
            btCancel.Enabled = true;
            string FindString = @"SELECT MZ_NAME,MZ_ID,MZ_AD,MZ_AD_CH,MZ_UNIT,MZ_UNIT_CH,MZ_EXAD,MZ_EXAD_CH,MZ_EXUNIT,MZ_EXUNIT_CH,
                                  MZ_RANK,MZ_RANK_CH,MZ_RANK1,MZ_RANK1_CH,MZ_OCCC,MZ_OCCC_CH,MZ_SRANK,MZ_SRANK_CH,MZ_TBDV,MZ_TBDV_CH 
                                  FROM VW_A_DLBASE_S2 WHERE RTRIM(MZ_ID)='" + ID + "'";

            DataTable FindDt = o_DBFactory.ABC_toTest.Create_Table(FindString, "123");

            


            TextBox_MZ_NAME.Text = FindDt.Rows[0]["MZ_NAME"].ToString().Trim();
            TextBox_MZ_ID.Text = FindDt.Rows[0]["MZ_ID"].ToString().Trim().ToUpper();
            TextBox_MZ_AD.Text = FindDt.Rows[0]["MZ_AD"].ToString().Trim().ToUpper();
            TextBox_MZ_UNIT.Text = FindDt.Rows[0]["MZ_UNIT"].ToString().Trim().ToUpper();
            TextBox_MZ_EXAD.Text = FindDt.Rows[0]["MZ_EXAD"].ToString().Trim().ToUpper();
            TextBox_MZ_EXUNIT.Text = FindDt.Rows[0]["MZ_EXUNIT"].ToString().Trim().ToUpper();
            TextBox_MZ_OCCC.Text = FindDt.Rows[0]["MZ_OCCC"].ToString().Trim().ToUpper();
            TextBox_MZ_RANK.Text = FindDt.Rows[0]["MZ_RANK"].ToString().Trim().ToUpper();
            TextBox_MZ_RANK1.Text = FindDt.Rows[0]["MZ_RANK1"].ToString().Trim().ToUpper();
            TextBox_MZ_SRANK.Text = FindDt.Rows[0]["MZ_SRANK"].ToString().Trim().ToUpper();
            TextBox_MZ_TBDV.Text = FindDt.Rows[0]["MZ_TBDV"].ToString().Trim().ToUpper();



            TextBox_MZ_AD1.Text = FindDt.Rows[0]["MZ_AD_CH"].ToString().Trim();
            TextBox_MZ_UNIT1.Text = FindDt.Rows[0]["MZ_UNIT_CH"].ToString().Trim();
           
            TextBox_MZ_EXAD1.Text = FindDt.Rows[0]["MZ_EXAD_CH"].ToString().Trim();
            TextBox_MZ_EXUNIT1.Text = FindDt.Rows[0]["MZ_EXUNIT_CH"].ToString().Trim();
            TextBox_MZ_OCCC1.Text = FindDt.Rows[0]["MZ_OCCC_CH"].ToString().Trim();
            TextBox_MZ_RANK_1.Text = FindDt.Rows[0]["MZ_RANK_CH"].ToString().Trim();
            TextBox_MZ_RANK1_1.Text = FindDt.Rows[0]["MZ_RANK1_CH"].ToString().Trim();
            TextBox_MZ_SRANK1.Text = FindDt.Rows[0]["MZ_SRANK_CH"].ToString().Trim();

            TextBox_MZ_TBDV1.Text = FindDt.Rows[0]["MZ_TBDV_CH"].ToString().Trim();

            



            //20141210
            TextBox_MZ_SWT1.Text = "N";
            TextBox_MZ_SWT4.Text = "N";

            if (FindDt.Rows[0]["MZ_UNIT"].ToString().Trim() != FindDt.Rows[0]["MZ_EXUNIT"].ToString().Trim())
            {
                TextBox_MZ_MEMO.Text = "支援" + o_A_KTYPE.RUNIT(FindDt.Rows[0]["MZ_EXUNIT"].ToString().Trim());
            }
            else
            {
                TextBox_MZ_MEMO.Text = string.Empty;
            }

            //特殊:如果已經離職,則備註標記一下
            string MZ_STATUS2String = @"SELECT MZ_STATUS2 FROM A_DLBASE WHERE RTRIM(MZ_ID)='" + ID + "'";
            DataTable MZ_STATUSDt = o_DBFactory.ABC_toTest.Create_Table(MZ_STATUS2String, "123");
            if (MZ_STATUSDt.Rows[0]["MZ_STATUS2"].ToString().Trim() == "N"
                && TextBox_MZ_MEMO.Text.Contains("已調離新北或離職")==false)
            {
                TextBox_MZ_MEMO.Text += "已調離新北或離職";
            }


            Check_SWT3();

            chk_TPMGroup();

           
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_PRK.ClientID + "').focus();$get('" + TextBox_MZ_PRK.ClientID + "').focus();", true);
        }

        /// <summary>
        /// 依條件查出獎懲資料後Bind至畫面欄位
        /// </summary>
        /// <param name="Datacount"></param>
        protected void finddata(int Datacount)
        {
            PRKB_MZ_ID = Session["PRKB_MZ_ID"] as List<string>;
            PRKB_MZ_NO = Session["PRKB_MZ_NO"] as List<string>;
            PRKB_MZ_PRCT = Session["PRKB_MZ_PRCT"] as List<string>;
            string strSQL = "SELECT * FROM A_PRKB WHERE MZ_ID='" + PRKB_MZ_ID[Datacount] + "' AND MZ_NO='" + PRKB_MZ_NO[Datacount] + "' AND MZ_PRCT=@MZ_PRCT";

            SqlParameter[] ParamaterList ={
            new SqlParameter("MZ_PRCT",SqlDbType.NVarChar){Value = PRKB_MZ_PRCT[Datacount]}
            };

            SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString);
            conn.Open();
            SqlCommand oracmd = new SqlCommand(strSQL, conn);

            if (ParamaterList != null && ParamaterList.Count() > 0)
            {
                foreach (SqlParameter para in ParamaterList)
                {
                    oracmd.Parameters.Add(para);
                }
            }

            SqlDataReader read = oracmd.ExecuteReader();


            if (read.Read())
            {
                TextBox_MZ_NO.Text = read["MZ_NO"].ToString().Trim();
                TextBox_MZ_ID.Text = read["MZ_ID"].ToString().Trim();
                TextBox_MZ_NAME.Text = read["MZ_NAME"].ToString().Trim();
                TextBox_MZ_AD.Text = read["MZ_AD"].ToString().Trim();
                TextBox_MZ_UNIT.Text = read["MZ_UNIT"].ToString().Trim();
                TextBox_MZ_OCCC.Text = read["MZ_OCCC"].ToString().Trim();
                TextBox_MZ_RANK.Text = read["MZ_RANK"].ToString().Trim();
                TextBox_MZ_RANK1.Text = read["MZ_RANK1"].ToString().Trim();
                TextBox_MZ_SRANK.Text = read["MZ_SRANK"].ToString().Trim();
                TextBox_MZ_TBDV.Text = read["MZ_TBDV"].ToString().Trim();
                TextBox_MZ_PRK1.Text = read["MZ_PRK1"].ToString().Trim();
                TextBox_MZ_PRK.Text = read["MZ_PRK"].ToString().Trim();
                TextBox_MZ_PRRST.Text = read["MZ_PRRST"].ToString().Trim();
                TextBox_MZ_PROLNO.Text = read["MZ_PROLNO"].ToString().Trim();
                TextBox_MZ_AD.Text = read["MZ_AD"].ToString().Trim();
                TextBox_MZ_UNIT.Text = read["MZ_UNIT"].ToString().Trim();
                DropDownList_MZ_POLK.SelectedValue = string.IsNullOrEmpty(read["MZ_POLK"].ToString().Trim()) ? "" : read["MZ_POLK"].ToString().Trim();
                DropDownList_MZ_PCODE.SelectedValue = string.IsNullOrEmpty(read["MZ_PCODE"].ToString().Trim()) ? "" : read["MZ_PCODE"].ToString().Trim();
                TextBox_MZ_MEMO.Text = read["MZ_MEMO"].ToString().Trim();
                DropDownList_MZ_SWT3.SelectedValue = string.IsNullOrEmpty(read["MZ_SWT3"].ToString().Trim()) ? "" : read["MZ_SWT3"].ToString().Trim();
                TextBox_MZ_SWT4.Text = read["MZ_SWT4"].ToString().Trim();
                TextBox_MZ_SWT1.Text = read["MZ_SWT1"].ToString().Trim();
                //TextBox_MZ_RDATE.Text = read[23].ToString().Trim();
                TextBox_MZ_PCODEM.Text = read["MZ_PCODEM"].ToString().Trim();
                TextBox_MZ_REMARK.Text = read["MZ_REMARK"].ToString().Trim();
                TextBox_MZ_EXAD.Text = read["MZ_EXAD"].ToString().Trim();
                TextBox_MZ_EXUNIT.Text = read["MZ_EXUNIT"].ToString().Trim();
                TextBox_MZ_PRCT.Text = read["MZ_PRCT"].ToString().Trim();
                TextBox_MZ_PROLNO2.Text = read["MZ_PROLNO2"].ToString().Trim();
                TextBox_MZ_MEMO.Text = read["MZ_MEMO"].ToString().Trim();

                TextBox_MZ_AD1.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)='38213' UNION ALL SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)<>'38213') WHERE MZ_KCODE='" + TextBox_MZ_AD.Text.Trim() + "'");
                TextBox_MZ_EXAD1.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)='38213' UNION ALL SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)<>'38213') WHERE MZ_KCODE='" + TextBox_MZ_EXAD.Text.Trim() + "'");
                TextBox_MZ_EXUNIT1.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='25' AND MZ_KCODE IN (SELECT MZ_UNIT FROM A_UNIT_AD WHERE MZ_AD='" + TextBox_MZ_EXAD.Text.Trim() + "') AND MZ_KCODE='" + TextBox_MZ_EXUNIT.Text.Trim() + "'");
                TextBox_MZ_OCCC1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_OCCC.Text, "26");
                TextBox_MZ_RANK_1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_RANK.Text, "09");
                TextBox_MZ_RANK1_1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_RANK1.Text, "09");
                TextBox_MZ_SRANK1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_SRANK.Text, "09");
                TextBox_MZ_UNIT1.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='25' AND MZ_KCODE IN (SELECT MZ_UNIT FROM A_UNIT_AD WHERE MZ_AD='" + TextBox_MZ_AD.Text.Trim() + "') AND MZ_KCODE='" + TextBox_MZ_UNIT.Text.Trim() + "'");
                TextBox_MZ_PROLNO1.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_PRONAME AS MZ_KCHI FROM A_PROLNO WHERE MZ_PROLNO='" + TextBox_MZ_PROLNO.Text + "'");
                TextBox_MZ_PCODEM1.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_PNAME FROM A_POLNUM WHERE MZ_POLNO='" + TextBox_MZ_PCODEM.Text + "'");
                TextBox_MZ_PRK_1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_PRK.Text, "22");
                TextBox_MZ_TBDV1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_TBDV.Text, "43");
                TextBox_MZ_PRRST1.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='24' AND MZ_KCODE='" + TextBox_MZ_PRRST.Text.Trim().ToUpper() + "' ");
                TextBox_MZ_PRK1_1.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT STATIC_NAME FROM A_STATIC WHERE STATIC_NO='" + TextBox_MZ_PRK1.Text.Trim().ToUpper() + "'");

                //2012/12/26 irk 新增
                ViewState["A_PRKB_SN"] = read["SN"].ToString();
                //switch (Session["Personal_strGID"].ToString())
                //{
                //    case "A":
                //        btUpdate.Visible = true;
                //        btDelete.Visible = true;
                //        break;
                //    case "B":
                //        btUpdate.Visible = true;
                //        btDelete.Visible = true;
                //        break;
                //    case "C":
                //        //if (o_DBFactory.ABC_toTest.vExecSQL("SELECT DISTINCT MUSER FROM A_PRKB WHERE MZ_NO='" + dt.Rows[0]["MZ_NO"].ToString() + "' AND MZ_ID='" + dt.Rows[0]["MZ_ID"].ToString() + "'") != Session["ADPMZ_ID"].ToString())
                //        //{
                //        //    btUpdate.Visible = false;
                //        //    btDelete.Visible = false;
                //        //}
                //        //else
                //        //{
                //        //    btUpdate.Visible = true;
                //        //    btDelete.Visible = true;
                //        //}
                //        break;
                //    case "D":
                //        //if (o_DBFactory.ABC_toTest.vExecSQL("SELECT DISTINCT MUSER FROM A_PRKB WHERE MZ_NO='" + dt.Rows[0]["MZ_NO"].ToString() + "'") != Session["ADPMZ_ID"].ToString())
                //        //{
                //        //    btUpdate.Visible = false;
                //        //    btDelete.Visible = false;
                //        //}
                //        //else
                //        //{
                //        //    btUpdate.Visible = true;
                //        //    btDelete.Visible = true;
                //        //}
                //        break;
                //    case "E":
                //        //if (o_DBFactory.ABC_toTest.vExecSQL("SELECT DISTINCT MUSER FROM A_PRKB WHERE MZ_NO='" + dt.Rows[0]["MZ_NO"].ToString() + "'") != Session["ADPMZ_ID"].ToString())
                //        //{
                //        //    btUpdate.Visible = false;
                //        //    btDelete.Visible = false;
                //        //}
                //        //else
                //        //{
                //        //    btUpdate.Visible = true;
                //        //    btDelete.Visible = true;
                //        //}
                //        break;
                //}
            }
            if (TextBox_MZ_SWT4.Text.Trim() == "Y")
            {
                btUpdate.Enabled = false;
                btDelete.Enabled = false;
            }
            else
            {
                btUpdate.Enabled = true;
                btDelete.Enabled = true;
            }
            conn.Close();
            //XX2013/06/18 
            conn.Dispose();

            chk_TPMGroup();
        }

        /// <summary>
        /// 按鈕：新增
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btInsert_Click(object sender, EventArgs e)
        {
            if (Session["PRKB_Mode"] != null)
            {
                if (Session["PRKB_Mode"].ToString() == "SEARCH")
                {
                    Session["PRKB_OLD_MZ_NO"] = TextBox_MZ_NO.Text.Trim();
                    Session["PRKB_OLD_MZ_ID"] = TextBox_MZ_ID.Text.Trim();
                    TextBox_MZ_ID.Text = string.Empty;
                    TextBox_MZ_NAME.Text = string.Empty;
                    TextBox_MZ_AD.Text = string.Empty;
                    TextBox_MZ_EXAD.Text = string.Empty;
                    TextBox_MZ_EXUNIT.Text = string.Empty;
                    TextBox_MZ_OCCC.Text = string.Empty;
                    TextBox_MZ_UNIT.Text = string.Empty;
                    TextBox_MZ_RANK.Text = string.Empty;
                    TextBox_MZ_RANK1.Text = string.Empty;
                    TextBox_MZ_SRANK.Text = string.Empty;
                    TextBox_MZ_TBDV.Text = string.Empty;
                    TextBox_MZ_AD1.Text = string.Empty;
                    TextBox_MZ_EXAD1.Text = string.Empty;
                    TextBox_MZ_EXUNIT1.Text = string.Empty;
                    TextBox_MZ_OCCC1.Text = string.Empty;
                    TextBox_MZ_UNIT1.Text = string.Empty;
                    TextBox_MZ_RANK_1.Text = string.Empty;
                    TextBox_MZ_RANK1_1.Text = string.Empty;
                    TextBox_MZ_SRANK1.Text = string.Empty;
                    TextBox_MZ_TBDV1.Text = string.Empty;

                    TextBox_MZ_REMARK.Text = string.Empty;
                    DropDownList_MZ_SWT3.SelectedValue = "";

                    TextBox_MZ_MEMO.Text = string.Empty;
                }

                //TextBox_MZ_SWT4.Text = string.Empty;
                //20141210
                TextBox_MZ_SWT1.Text = "N";
                TextBox_MZ_SWT4.Text = "N";
            }
            else
            {
                foreach (object dl in Panel_Personal2.Controls)
                {
                    if (dl is DropDownList)
                    {
                        DropDownList dl1 = dl as DropDownList;
                        if (dl1.ID != "DropDownList_MZ_PCODE")
                        {
                            dl1.SelectedValue = "";
                        }
                    }


                    if (dl is TextBox)
                    {
                        TextBox tbox = dl as TextBox;

                        if (tbox.ID == "TextBox_MZ_MEMO" || tbox.ID == "TextBox_MZ_REMARK" || tbox.ID == "TextBox_MZ_SWT1" || tbox.ID == "TextBox_MZ_SWT4")
                        {
                            tbox.Text = string.Empty;
                        }
                      

                    }
                }

                if (string.IsNullOrEmpty(o_DBFactory.ABC_toTest.vExecSQL("SELECT DISTINCT MZ_NO FROM A_PRKB WHERE MZ_NO='" + TextBox_MZ_NO.Text.Trim() + "'")))
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_NO.ClientID + "').focus();$get('" + TextBox_MZ_NO.ClientID + "').focus();", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_ID.ClientID + "').focus();$get('" + TextBox_MZ_ID.ClientID + "').focus();", true);
                }



                //20141210
                TextBox_MZ_SWT1.Text = "N";
                TextBox_MZ_SWT4.Text = "N";



            }

            if (string.IsNullOrEmpty(TextBox_MZ_NO.Text))
            {
                //2023-03-23 這段先不用帶了
                //if (Session["ADPMZ_EXUNIT"] != null)
                //{
                //    TextBox_MZ_NO.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_NO1 FROM A_UNIT_AD WHERE MZ_AD='" + Session["ADPMZ_EXAD"].ToString() + "' AND MZ_UNIT='" + Session["ADPMZ_EXUNIT"].ToString() + "'");
                //}
            }

            Session["PRKB_CMDSQL"] = "INSERT INTO " +
                                     "A_PRKB" +
                                           "(MZ_NO,MZ_ID,MZ_NAME,MZ_AD,MZ_UNIT,MZ_OCCC,MZ_RANK,MZ_RANK1,MZ_SRANK,MZ_TBDV,MZ_PRCT, " +
                                           " MZ_PRK1,MZ_PRK,MZ_PRRST,MZ_PROLNO,MZ_POLK,MZ_PCODE,MZ_MEMO,MZ_SWT3," +
                                           " MZ_SWT4,MZ_SWT1,MZ_RDATE,MZ_PCODEM,MZ_REMARK,MZ_EXAD,MZ_EXUNIT,MZ_PROLNO2,MUSER,MDATE)" +
                                     " VALUES" +
                                           "(@MZ_NO,@MZ_ID,@MZ_NAME,@MZ_AD,@MZ_UNIT,@MZ_OCCC,@MZ_RANK,@MZ_RANK1,@MZ_SRANK,@MZ_TBDV,@MZ_PRCT, " +
                                           " @MZ_PRK1,@MZ_PRK,@MZ_PRRST,@MZ_PROLNO,@MZ_POLK,@MZ_PCODE,@MZ_MEMO,@MZ_SWT3," +
                                           " @MZ_SWT4,@MZ_SWT1,@MZ_RDATE,@MZ_PCODEM,@MZ_REMARK,@MZ_EXAD,@MZ_EXUNIT,@MZ_PROLNO2,@MUSER,@MDATE)";

            Session["PRKB_Mode"] = "INSERT";

            btInsert.Enabled = false;
            btUpdate.Enabled = false;
            btOK.Enabled = true;
            Button2.Enabled = true;
            btCancel.Enabled = true;
            btDelete.Enabled = false;
            btUpper.Enabled = false;
            btNEXT.Enabled = false;
            Label1.Visible = false;
            btSearch.Enabled = false;

            A.controlEnable(ref this.Panel_Personal2, true);
           //20141115
                TextBox_MZ_NAME.Enabled = false;

            DropDownList_MZ_POLK.Enabled = false;
            DropDownList_MZ_POLK.Enabled = false;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_NO.ClientID + "').focus();$get('" + TextBox_MZ_NO.ClientID + "').focus();", true);
        }

        /// <summary>
        /// 按鈕：刪除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btDelete_Click(object sender, EventArgs e)
        {
            if (TextBox_MZ_SWT4.Text == "Y")
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('已審核！不可刪除！');", true);
                return;
            }

            //2012/12/26 irk 改用sn刪除
            //string DeleteString = "DELETE " +
            //                      "FROM " +
            //                              "A_PRKB " +
            //                      "WHERE " +
            //                              "  MZ_ID= '" + TextBox_MZ_ID.Text.Trim().ToUpper() +
            //                              "' AND MZ_NO='" + TextBox_MZ_NO.Text.Trim() +
            //                              "' AND MZ_PRCT like '" + TextBox_MZ_PRCT.Text.Trim() + "%'";

            string delstr = string.Format("Delete from A_PRKB WHERE SN={0}", ViewState["A_PRKB_SN"].ToString());
            try
            {
                //o_DBFactory.ABC_toTest.Edit_Data(DeleteString);
                o_DBFactory.ABC_toTest.Edit_Data(delstr);
                PRKB_MZ_ID = Session["PRKB_MZ_ID"] as List<string>;
                PRKB_MZ_NO = Session["PRKB_MZ_NO"] as List<string>;
                PRKB_MZ_PRCT = Session["PRKB_MZ_PRCT"] as List<string>;
                PRKB_MZ_ID.RemoveAt(int.Parse(xcount.Text.Trim()));
                PRKB_MZ_NO.RemoveAt(int.Parse(xcount.Text.Trim()));
                PRKB_MZ_PRCT.RemoveAt(int.Parse(xcount.Text.Trim()));

                if (PRKB_MZ_ID.Count == 0)
                {

                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除成功');location.href('Personal2-1.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "');", true);
                    btUpdate.Enabled = false;
                    btDelete.Enabled = false;
                }
                else
                {
                    xcount.Text = "0";
                    finddata(int.Parse(xcount.Text));
                    if (PRKB_MZ_ID.Count > 1)
                    {
                        btNEXT.Enabled = true;
                    }

                    Label1.Text = "第" + (int.Parse(xcount.Text) + 1).ToString() + "筆共" + PRKB_MZ_ID.Count.ToString() + "筆";
                    btUpdate.Enabled = true;
                    btDelete.Enabled = true;
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除成功')", true);

                    //2010.06.04 LOG紀錄 by伊珊
                    TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00004", TPMPermissions._boolTPM(), delstr);
                }
                btInsert.Enabled = true;
                btOK.Enabled = false;
                btCancel.Enabled = false;
                Button2.Enabled = false;
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除失敗');", true);
            }
        }

        /// <summary>
        /// 按鈕：修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btUpdate_Click(object sender, EventArgs e)
        {

            Session["PRKB_CMDSQL"] = "UPDATE " +
                                                "A_PRKB " +
                                     "SET " +
                                                "MZ_NO=@MZ_NO,MZ_ID=@MZ_ID,MZ_NAME=@MZ_NAME,MZ_AD=@MZ_AD,MZ_UNIT=@MZ_UNIT," +
                                                " MZ_OCCC=@MZ_OCCC,MZ_RANK=@MZ_RANK,MZ_RANK1=@MZ_RANK1,MZ_SRANK=@MZ_SRANK," +
                                                " MZ_TBDV=@MZ_TBDV,MZ_PRCT=@MZ_PRCT,MZ_PRK1=@MZ_PRK1,MZ_PRK=@MZ_PRK,MZ_PRRST=@MZ_PRRST," +
                                                " MZ_PROLNO=@MZ_PROLNO,MZ_POLK=@MZ_POLK,MZ_PCODE=@MZ_PCODE,MZ_MEMO=@MZ_MEMO,MZ_SWT3=@MZ_SWT3," +
                                                " MZ_SWT4=@MZ_SWT4,MZ_SWT1=@MZ_SWT1,MZ_RDATE=@MZ_RDATE,MZ_PCODEM=@MZ_PCODEM,MZ_REMARK=@MZ_REMARK," +
                                                " MZ_EXAD=@MZ_EXAD,MZ_EXUNIT=@MZ_EXUNIT,MZ_PROLNO2=@MZ_PROLNO2,MUSER=@MUSER,MDATE=@MDATE " +
                                     "WHERE " +
                                                "MZ_ID='" + TextBox_MZ_ID.Text.Trim().ToUpper() +
                                                "' AND MZ_NO='" + TextBox_MZ_NO.Text.Trim() +
                                                "' AND MZ_PRCT=@MZ_PRCT_A";

            Session["PRKB_Mode"] = "UPDATE";

            Session["PKEY_MZ_ID"] = TextBox_MZ_ID.Text;

            Session["PKEY_MZ_NO"] = TextBox_MZ_NO.Text;

            Session["PKEY_MZ_PRCT"] = TextBox_MZ_PRCT.Text;


            btUpdate.Enabled = false;
            btCancel.Enabled = true;
            btInsert.Enabled = false;
            btOK.Enabled = true;
            Button2.Enabled = false;
            btNEXT.Enabled = false;
            btUpper.Enabled = false;
            btSearch.Enabled = false;

            A.controlEnable(ref this.Panel_Personal2, true);
            //20141115
            TextBox_MZ_NAME.Enabled = false;
            DropDownList_MZ_POLK.Enabled = false;
            DropDownList_MZ_PCODE.SelectedValue = DropDownList_MZ_PCODE.SelectedItem.Text.Substring(0, 1);
            Button2.Enabled = false;
        }

        protected void btUpper_Click(object sender, EventArgs e)
        {
            if (int.Parse(xcount.Text) != 0)
            {
                xcount.Text = (Convert.ToInt32(xcount.Text) - 1).ToString();
                finddata(int.Parse(xcount.Text));
                if (int.Parse(xcount.Text) != PRKB_MZ_ID.Count - 1)
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
            Label1.Text = "第" + (int.Parse(xcount.Text) + 1).ToString() + "筆共" + PRKB_MZ_ID.Count.ToString() + "筆";
        }

        /// <summary>
        /// 下一筆按鈕操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btNEXT_Click(object sender, EventArgs e)
        {
            if (int.Parse(xcount.Text) == 0)
            {
                xcount.Text = (Convert.ToInt32(xcount.Text) + 1).ToString();

                btUpper.Enabled = true;

                finddata(int.Parse(xcount.Text));

                if (int.Parse(xcount.Text) == PRKB_MZ_ID.Count - 1)
                {
                    btNEXT.Enabled = false;
                }
            }
            else
            {
                xcount.Text = (Convert.ToInt32(xcount.Text) + 1).ToString();

                finddata(int.Parse(xcount.Text));

                if (int.Parse(xcount.Text) == PRKB_MZ_ID.Count - 1)
                {
                    btNEXT.Enabled = false;
                }
            }

            Label1.Text = "第" + (int.Parse(xcount.Text) + 1).ToString() + "筆共" + PRKB_MZ_ID.Count.ToString() + "筆";
        }

        protected void btSearch_Click(object sender, EventArgs e)
        {
            Session["PRKB_Mode"] = "SEARCH";

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('PersonalSearch1.aspx?TableName=PRKB&TPM_FION=" + Request.QueryString["TPM_FION"] + "','查詢','top=190,left=200,width=500,height=230,toolbar=0,menubar=0,location=0,directories=0,status=0,scrollbars=yes');", true);
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            Session["PRKB_MZ_NO"] = TextBox_MZ_NO.Text.Trim();

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('PersonalSearchIDwithNAME.aspx?TableName=PRKB&TPM_FION=" + Request.QueryString["TPM_FION"] + "','查詢','top=190,left=200,width=750,height=500,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }

        protected void Button_MZ_SWT4_Click(object sender, EventArgs e)
        {
            string SWT4_DATE = (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0') + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0');

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('PersonalUpdateSWT4.aspx?MZ_NO=" + TextBox_MZ_NO.Text.Trim() +
                                                                                           "&SWT4_DATE=" + SWT4_DATE +
                                                                                           "&TPM_FION=" + Request.QueryString["TPM_FION"] + "','查詢'," +
                                                                                           "'top=190,left=200,width=650,height=400,toolbar=0,menubar=0,location=0,directories=0,status=0,scrollbars=yes');", true);
        }

        protected void btGroupUpdate_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('PersonalPRKB_GroupUpdate.aspx?MZ_NO=" + TextBox_MZ_NO.Text.Trim() + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "','查詢','top=190,left=200,width=800,height=400,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }

        protected void DropDownList_MZ_PCODE_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownList_MZ_PCODE.SelectedIndex == 1)
            {
                TextBox_MZ_PCODEM.Enabled = true;
                btPCODEM.Enabled = true;
                TextBox_MZ_PCODEM1.Enabled = true;
            }
            else
            {
                TextBox_MZ_PCODEM.Enabled = false;
                btPCODEM.Enabled = false;
                TextBox_MZ_PCODEM1.Enabled = false;
            }
        }

        /// <summary>
        /// 按鈕：確認
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btok_Click(object sender, EventArgs e)
        {
            string ErrorString = "";
            string old_ID = "NULL";
            string old_NO = "NULL";
            string old_PRCT = "NULL";

            A_PRKB_Model model = new A_PRKB_Model()
            {
                MZ_ID = TextBox_MZ_ID.Text.Trim(),
                MZ_PRCT = TextBox_MZ_PRCT.Text.Trim(),
                MZ_PRK = TextBox_MZ_PRK.Text.Trim(),
                MZ_PRRST = TextBox_MZ_PRRST.Text.Trim()
            };

            if (Session["PRKB_Mode"].ToString() == "UPDATE")
            {
                old_ID = Session["PKEY_MZ_ID"].ToString();
                old_NO = Session["PKEY_MZ_NO"].ToString();
                old_PRCT = Session["PKEY_MZ_PRCT"].ToString();
            }

            //新增獎懲類別重複限制 20180329 by sky
            //重新改寫共用Function檢查 20181218 by sky
            //修改檢核條件，更新程式寫法 20191002 by sky
            if (old_ID == model.MZ_ID && old_PRCT == model.MZ_PRCT && Session["PRKB_Mode"].ToString() == "UPDATE")
            {
                //TextBox_MZ_NO.BackColor = Color.White;
                TextBox_MZ_ID.BackColor = Color.White;
                TextBox_MZ_PRCT.BackColor = Color.White;
                //TextBox_MZ_PRK.BackColor = Color.White;
            }
            else
            {
                //新增檢核
                if (PersonalService.DuplicateCheckFor_APRKB(model))
                {
                    //TextBox_MZ_NO.BackColor = Color.White;
                    TextBox_MZ_ID.BackColor = Color.White;
                    TextBox_MZ_PRCT.BackColor = Color.White;
                    //TextBox_MZ_PRK.BackColor = Color.White;
                }
                else
                {
                    ErrorString += "身分證號與獎懲內容違反唯一值的條件" + "\\r\\n";
                    //TextBox_MZ_NO.BackColor = Color.Orange;
                    TextBox_MZ_ID.BackColor = Color.Orange;
                    TextBox_MZ_PRCT.BackColor = Color.Orange;
                    //TextBox_MZ_PRK.BackColor = Color.Orange;
                }
            }

            if (string.IsNullOrEmpty(DropDownList_MZ_POLK.SelectedValue))
            {
                ErrorString += "依據類別不可為空白" + "\\r\\n";
            }
            if (string.IsNullOrEmpty(TextBox_MZ_PRK1.Text))
            {
                ErrorString += "獎懲統計分類不可為空白" + "\\r\\n";
            }
            if (TextBox_MZ_PRCT.Text.Length > 50)
            {
                ErrorString += "獎懲內容不能超過50個字" + "\\r\\n";
            }
            if (string.IsNullOrEmpty(TextBox_MZ_PRCT.Text))
            {
                ErrorString += "獎懲內容不可為空白" + "\\r\\n";
            }

            if (string.IsNullOrEmpty(TextBox_MZ_PROLNO.Text))
            {
                ErrorString += "獎懲依據不可為空白" + "\\r\\n";
            }
            else
            {
                string check_PROLNO = o_DBFactory.ABC_toTest.vExecSQL(@"SELECT MZ_PROLNO  FROM A_PROLNO WHERE MZ_PROLNO='" + TextBox_MZ_PROLNO.Text + "'");
                if (string.IsNullOrEmpty(check_PROLNO))
                {
                    ErrorString += "獎懲依據錯誤，請重新選擇" + "\\r\\n";
                }
            }
            if (string.IsNullOrEmpty(DropDownList_MZ_PCODE.SelectedValue))
            {
                ErrorString += "是否配分不可為空白" + "\\r\\n";
            }
            if (!string.IsNullOrEmpty(ErrorString))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('" + ErrorString + "請更改不可重複欄位與輸入變色之欄位');", true);
                return;
            }

            //確認獎懲相關條文是否相符
            if (!check_PRRST(2))
            {
                return;
            }

            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(Session["PRKB_CMDSQL"].ToString(), conn);
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.Add("MZ_NO", SqlDbType.VarChar).Value = TextBox_MZ_NO.Text.Trim().ToUpper();
                cmd.Parameters.Add("MZ_ID", SqlDbType.VarChar).Value = TextBox_MZ_ID.Text.Trim().ToUpper();
                cmd.Parameters.Add("MZ_NAME", SqlDbType.VarChar).Value = TextBox_MZ_NAME.Text.Trim().ToUpper();
                cmd.Parameters.Add("MZ_AD", SqlDbType.VarChar).Value = TextBox_MZ_AD.Text.Trim().ToUpper();
                cmd.Parameters.Add("MZ_UNIT", SqlDbType.VarChar).Value = TextBox_MZ_UNIT.Text.Trim().ToUpper();
                cmd.Parameters.Add("MZ_OCCC", SqlDbType.VarChar).Value = TextBox_MZ_OCCC.Text.Trim().ToUpper();
                cmd.Parameters.Add("MZ_RANK", SqlDbType.VarChar).Value = TextBox_MZ_RANK.Text.Trim().ToUpper();
                cmd.Parameters.Add("MZ_RANK1", SqlDbType.VarChar).Value = TextBox_MZ_RANK1.Text.Trim().ToUpper();
                cmd.Parameters.Add("MZ_SRANK", SqlDbType.VarChar).Value = TextBox_MZ_SRANK.Text.Trim().ToUpper();
                cmd.Parameters.Add("MZ_TBDV", SqlDbType.VarChar).Value = TextBox_MZ_TBDV.Text.Trim().ToUpper();
                cmd.Parameters.Add("MZ_PRK1", SqlDbType.VarChar).Value = TextBox_MZ_PRK1.Text.Trim().ToUpper();
                cmd.Parameters.Add("MZ_PRK", SqlDbType.VarChar).Value = TextBox_MZ_PRK.Text.Trim().ToUpper();
                cmd.Parameters.Add("MZ_PRRST", SqlDbType.VarChar).Value = TextBox_MZ_PRRST.Text.Trim().ToUpper();
                //2013/08/22
                ////////////////////////
                
                //舊的
                cmd.Parameters.Add("MZ_PROLNO", SqlDbType.VarChar).Value = TextBox_MZ_PROLNO.Text.Trim().ToUpper();
                //如果獎逞依據有空白下面要改寫
                //cmd.Parameters.Add("MZ_PROLNO", SqlDbType.VarChar).Value = string.IsNullOrEmpty(TextBox_MZ_PROLNO.Text.Trim().ToUpper());

                ////////////////////////
                
                //cmd.Parameters.Add("MZ_POLK", SqlDbType.VarChar).Value = string.IsNullOrEmpty(DropDownList_MZ_POLK.SelectedValue.Trim().ToUpper()) ? Convert.DBNull : DropDownList_MZ_POLK.SelectedValue.Trim().ToUpper();
                
                cmd.Parameters.Add("MZ_POLK", SqlDbType.VarChar).Value = string.IsNullOrEmpty(DropDownList_MZ_POLK.SelectedValue.Trim()) ? "H" : DropDownList_MZ_POLK.SelectedValue.Trim().ToUpper();
                
                //cmd.Parameters.Add("MZ_PCODE", SqlDbType.VarChar).Value = string.IsNullOrEmpty(DropDownList_MZ_PCODE.SelectedValue.Trim().ToUpper()) ? Convert.DBNull : DropDownList_MZ_PCODE.SelectedValue.Trim().ToUpper();
                
                cmd.Parameters.Add("MZ_PCODE", SqlDbType.VarChar).Value = string.IsNullOrEmpty(DropDownList_MZ_PCODE.SelectedValue.Trim()) ? "9" : DropDownList_MZ_PCODE.SelectedValue.Trim().ToUpper();
               //2013/08/22
                
                cmd.Parameters.Add("MZ_MEMO", SqlDbType.VarChar).Value = TextBox_MZ_MEMO.Text.Trim().ToUpper();
                cmd.Parameters.Add("MZ_SWT3", SqlDbType.VarChar).Value = string.IsNullOrEmpty(DropDownList_MZ_SWT3.SelectedValue.Trim().ToUpper()) ? Convert.DBNull : DropDownList_MZ_SWT3.SelectedValue.Trim().ToUpper();
                cmd.Parameters.Add("MZ_SWT4", SqlDbType.VarChar).Value = TextBox_MZ_SWT4.Text.Trim().ToUpper();
                cmd.Parameters.Add("MZ_SWT1", SqlDbType.VarChar).Value = TextBox_MZ_SWT1.Text.Trim().ToUpper();
                cmd.Parameters.Add("MZ_RDATE", SqlDbType.VarChar).Value = Convert.DBNull;//TextBox_MZ_RDATE.Text;
                cmd.Parameters.Add("MZ_PCODEM", SqlDbType.VarChar).Value = TextBox_MZ_PCODEM.Text.Trim().ToUpper();
                cmd.Parameters.Add("MZ_REMARK", SqlDbType.VarChar).Value = TextBox_MZ_REMARK.Text.Trim().ToUpper();
                cmd.Parameters.Add("MZ_EXAD", SqlDbType.VarChar).Value = TextBox_MZ_EXAD.Text.Trim().ToUpper();
                cmd.Parameters.Add("MZ_EXUNIT", SqlDbType.VarChar).Value = TextBox_MZ_EXUNIT.Text.Trim().ToUpper();
                cmd.Parameters.Add("MZ_PROLNO2", SqlDbType.VarChar).Value = TextBox_MZ_PROLNO2.Text.Trim().ToUpper();
                cmd.Parameters.Add("MZ_PRCT", SqlDbType.VarChar).Value = TextBox_MZ_PRCT.Text.Trim().ToUpper();//Strings.StrConv(TextBox_MZ_PRCT.Text.Trim().ToUpper(),VbStrConv.Wide,0); //全部轉全形輸入
                cmd.Parameters.Add("MUSER", SqlDbType.VarChar).Value = Session["ADPMZ_ID"].ToString();
                cmd.Parameters.Add("MDATE", SqlDbType.VarChar).Value = (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0') + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0');

                if (Session["PRKB_Mode"].ToString() == "UPDATE")
                    cmd.Parameters.Add("MZ_PRCT_A", SqlDbType.VarChar).Value = old_PRCT;

                try
                {
                    cmd.ExecuteNonQuery();

                    if (Session["PRKB_Mode"].ToString() == "INSERT")
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('新增成功');", true);

                        //2010.06.04 LOG紀錄 by伊珊
                        TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00002", TPMPermissions._boolTPM(), o_DBFactory.ABC_toTest.RegexSQL(cmd));

                        Session["PRKB_OLD_MZ_NO"] = TextBox_MZ_NO.Text.Trim();
                        Session["PRKB_OLD_MZ_ID"] = TextBox_MZ_ID.Text.Trim();
                        TextBox_MZ_ID.Text = string.Empty;
                        TextBox_MZ_NAME.Text = string.Empty;
                        TextBox_MZ_AD.Text = string.Empty;
                        TextBox_MZ_EXAD.Text = string.Empty;
                        TextBox_MZ_EXUNIT.Text = string.Empty;
                        TextBox_MZ_OCCC.Text = string.Empty;
                        TextBox_MZ_UNIT.Text = string.Empty;
                        TextBox_MZ_RANK.Text = string.Empty;
                        TextBox_MZ_RANK1.Text = string.Empty;
                        TextBox_MZ_SRANK.Text = string.Empty;
                        TextBox_MZ_TBDV.Text = string.Empty;
                        DropDownList_MZ_SWT3.SelectedValue = "";
                        TextBox_MZ_MEMO.Text = string.Empty;

                        TextBox_MZ_AD1.Text = string.Empty;
                        TextBox_MZ_EXAD1.Text = string.Empty;
                        TextBox_MZ_EXUNIT1.Text = string.Empty;
                        TextBox_MZ_OCCC1.Text = string.Empty;
                        TextBox_MZ_UNIT1.Text = string.Empty;
                        TextBox_MZ_RANK_1.Text = string.Empty;
                        TextBox_MZ_RANK1_1.Text = string.Empty;
                        TextBox_MZ_SRANK1.Text = string.Empty;
                        TextBox_MZ_TBDV1.Text = string.Empty;
                        TextBox_MZ_REMARK.Text = string.Empty;

                        //20141210
                        Button2.Enabled = false;
                        btInsert.Enabled = true;
                        btOK.Enabled = false;
                        btCancel.Enabled = false;
                        btUpdate.Enabled = false;
                        btDelete.Enabled = false;
                        
                        //Button2.Enabled = true;
                        //btInsert.Enabled = false;
                        //btCancel.Enabled = true;
                        //btOK.Enabled = true;
                        //20141210

                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_ID.ClientID + "').focus();$get('" + TextBox_MZ_ID.ClientID + "').focus();", true);
                    }
                    else if (Session["PRKB_Mode"].ToString() == "UPDATE")
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('修改成功')", true);

                        //2010.06.04 LOG紀錄 by伊珊
                        TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00003", TPMPermissions._boolTPM(), o_DBFactory.ABC_toTest.RegexSQL(cmd));

                        PRKB_MZ_ID = Session["PRKB_MZ_ID"] as List<string>;
                        PRKB_MZ_NO = Session["PRKB_MZ_NO"] as List<string>;
                        PRKB_MZ_PRCT = Session["PRKB_MZ_PRCT"] as List<string>;

                        PRKB_MZ_NO[int.Parse(xcount.Text)] = TextBox_MZ_NO.Text.Trim();
                        PRKB_MZ_PRCT[int.Parse(xcount.Text)] = TextBox_MZ_PRCT.Text.Trim();

                        if (int.Parse(xcount.Text.Trim()) == 0 && PRKB_MZ_ID.Count == 1)
                        {
                            btUpper.Enabled = false;
                            btNEXT.Enabled = false;
                        }
                        else if (int.Parse(xcount.Text.Trim()) == 0 && PRKB_MZ_ID.Count > 1)
                        {
                            btUpper.Enabled = false;
                            btNEXT.Enabled = true;
                        }
                        else if (int.Parse(xcount.Text.Trim()) + 1 == PRKB_MZ_ID.Count)
                        {
                            btUpper.Enabled = true;
                            btNEXT.Enabled = false;
                        }
                        else if (int.Parse(xcount.Text.Trim()) > 0 && int.Parse(xcount.Text.Trim()) + 1 < PRKB_MZ_ID.Count)
                        {
                            btNEXT.Enabled = true;
                            btUpper.Enabled = true;
                        }

                        Button2.Enabled = false;
                        btInsert.Enabled = true;
                        btOK.Enabled = false;
                        btCancel.Enabled = false;
                        btUpdate.Enabled = true;
                        btDelete.Enabled = true;

                        Session.Remove("PKEY_MZ_ID");
                        Session.Remove("PKEY_MZ_NO");
                        Session.Remove("PKEY_MZ_PRCT");
                        A.controlEnable(ref this.Panel_Personal2, false);
                        Button2.Enabled = false;
                    }
                  
                    btSearch.Enabled = true;
                }
                catch (Exception)
                {
                    if (Session["PRKB_Mode"].ToString() == "INSERT")
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('新增失敗');", true);
                    }
                    else if (Session["PRKB_Mode"].ToString() == "UPDATE")
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('修改失敗');location.href('Personal2-1.aspx?XCOUNT=" + xcount.Text.Trim() + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "');", true);
                    }

                    throw;
                }
                finally
                {
                
                    //2013/09/10

                    conn.Close();

                    //XX2013/06/18 

                    conn.Dispose();

                }
            }
        }

        /// <summary>
        /// 按鈕：取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btCancel_Click(object sender, EventArgs e)
        {
            if (Session["PRKB_Mode"].ToString() == "INSERT")
            {
                foreach (object dl in Panel_Personal2.Controls)
                {
                    if (dl is DropDownList)
                    {
                        DropDownList dl1 = dl as DropDownList;
                        if (dl1.ID != "DropDownList_MZ_PCODE")
                        {
                            dl1.SelectedValue = "";
                        }
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
                btUpdate.Enabled = false;
                btDelete.Enabled = false;
                Label1.Visible = false;
            }
            else if (Session["PRKB_Mode"].ToString() == "UPDATE")
            {
                finddata(int.Parse(xcount.Text.Trim()));

                PRKB_MZ_ID = Session["PRKB_MZ_ID"] as List<string>;
                if (int.Parse(xcount.Text.Trim()) == 0 && PRKB_MZ_ID.Count == 1)
                {
                    btUpper.Enabled = false;
                    btNEXT.Enabled = false;
                }
                else if (int.Parse(xcount.Text.Trim()) == 0 && PRKB_MZ_ID.Count > 1)
                {
                    btUpper.Enabled = false;
                    btNEXT.Enabled = true;
                }
                else if (int.Parse(xcount.Text.Trim()) + 1 == PRKB_MZ_ID.Count)
                {
                    btUpper.Enabled = true;
                    btNEXT.Enabled = false;
                }
                else if (int.Parse(xcount.Text.Trim()) > 0 && int.Parse(xcount.Text.Trim()) + 1 < PRKB_MZ_ID.Count)
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
            Button2.Enabled = false;
            Session.Remove("PKEY_MZ_ID");
            Session.Remove("PKEY_MZ_NO");
            Session.Remove("PKEY_MZ_ID");
            Session.Remove("PRKB_OLD_MZ_NO");
            Session.Remove("PRKB_OLD_MZ_ID");
            A.controlEnable(ref this.Panel_Personal2, false);
            Button2.Enabled = false;
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
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_Ktype_Search.aspx?MZ_KTYPE=" + ktype + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin','top=190,left=200,width=450,height=500,toolbar=0,menubar=0,location=0,directories=0,status=0,scrollbars=yes');", true);
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
            //Session["KTYPE_CID1"] = TextBox_MZ_EXAD1.ClientID;
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_Ktype_Search.aspx?CID_NAME=AD&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin','top=190,left=200,width=450,height=500,toolbar=0,menubar=0,location=0,directories=0,status=0,scrollbars=yes');", true);
        }

        protected void btUNIT_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_MZ_UNIT, TextBox_MZ_UNIT1, "25");
            //if (string.IsNullOrEmpty(o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCODE FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,6)='38213' UNION ALL SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,6)<>'38213') WHERE MZ_KCODE='" + TextBox_MZ_AD.Text.Trim() + "'")))
            //{
            //    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請先輸入編制機關')", true);
            //    return;
            //}
            //Session["KTYPE_CID"] = TextBox_MZ_UNIT.ClientID;
            //Session["KTYPE_CID1"] = TextBox_MZ_UNIT1.ClientID;
            //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_Ktype_Search.aspx?CID_NAME=UNIT&AD=" + TextBox_MZ_AD.Text.Trim() + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin');", true);
        }

        protected void btEXUNIT_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_MZ_EXUNIT, TextBox_MZ_EXUNIT1, "25");
            //if (string.IsNullOrEmpty(o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCODE FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,6)='38213' UNION ALL SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,6)<>'38213') WHERE MZ_KCODE='" + TextBox_MZ_EXAD.Text.Trim() + "'")))
            //{
            //    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請先輸入現服機關')", true);
            //}
            //Session["KTYPE_CID"] = TextBox_MZ_EXUNIT.ClientID;
            //Session["KTYPE_CID1"] = TextBox_MZ_EXUNIT1.ClientID;
            //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_Ktype_Search.aspx?CID_NAME=UNIT&AD=" + TextBox_MZ_EXAD.Text.Trim() + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin');", true);
        }

        protected void btAD_Click(object sender, EventArgs e)
        {

            Session["KTYPE_CID"] = TextBox_MZ_AD.ClientID;
            Session["KTYPE_CID1"] = TextBox_MZ_AD1.ClientID;

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_Ktype_Search.aspx?CID_NAME=AD&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin','top=190,left=200,width=450,height=500,toolbar=0,menubar=0,location=0,directories=0,status=0,scrollbars=yes');", true);
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

        protected void btSRANK_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_MZ_SRANK, TextBox_MZ_SRANK1, "09");
        }

        protected void btPRK_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_MZ_PRK, TextBox_MZ_PRK_1, "22");
        }

        protected void btPRCT1_Click(object sender, EventArgs e)
        {
            Session["KTYPE_CID"] = TextBox_MZ_PRCT.ClientID;
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_Ktype_Search.aspx?MZ_KTYPE=NOTE&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin');", true);
        }

        protected void btTBDV_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_MZ_TBDV, TextBox_MZ_TBDV1, "43");
        }

        protected void TextBox_MZ_PRK_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_PRK.Text, "22");

            Ktype_Cname_Check(CName, TextBox_MZ_PRK_1, TextBox_MZ_PRK, TextBox_MZ_PRK1);

            DropDownList_MZ_POLK.SelectedValue = "H";//TextBox_MZ_PRK.Text.Substring(0, 1).ToString();

            //if (TextBox_MZ_PRK.Text.Substring(0, 1).ToString() == "A")
            //{
            //    DropDownList_MZ_PRK1.Items.Clear();
            //    DropDownList_MZ_PRK1.AppendDataBoundItems = false;
            //    SqlDataSource1.SelectCommand = "SELECT RTRIM(STATIC_NO)+RTRIM(STATIC_NAME),RTRIM(STATIC_NO) FROM A_STATIC WHERE dbo.SUBSTR(STATIC_NO,1,1)='A'";
            //    DropDownList_MZ_PRK1.DataTextField="RTRIM(STATIC_NO)+RTRIM(STATIC_NAME)";
            //    DropDownList_MZ_PRK1.DataValueField="RTRIM(STATIC_NO)";
            //    DropDownList_MZ_PRK1.DataBind();

            //    DropDownList_MZ_PRRST.Items.Clear();
            //    DropDownList_MZ_PRRST.AppendDataBoundItems = false;
            //    SqlDataSource2.SelectCommand = "SELECT RTRIM(MZ_KCODE)+RTRIM(MZ_KCHI),RTRIM(MZ_KCODE) FROM A_KTYPE WHERE MZ_KTYPE='24' "+
            //                                   " AND (dbo.SUBSTR(MZ_KCODE,1,1)='4' OR dbo.SUBSTR(MZ_KCODE,1,1)='7' OR dbo.SUBSTR(MZ_KCODE,1,1)='8'"+ 
            //                                   " OR dbo.SUBSTR(MZ_KCODE,1,1)='9')";
            //    DropDownList_MZ_PRRST.DataTextField="RTRIM(MZ_KCODE)+RTRIM(MZ_KCHI)";
            //    DropDownList_MZ_PRRST.DataValueField="RTRIM(MZ_KCODE)";
            //    DropDownList_MZ_PRRST.DataBind();
            //}
            //else if (TextBox_MZ_PRK.Text.Substring(0, 1).ToString() == "B" || TextBox_MZ_PRK.Text.Substring(0, 1).ToString() == "C" || TextBox_MZ_PRK.Text.Substring(0, 1).ToString() == "D")
            //{
            //    DropDownList_MZ_PRK1.Items.Clear();
            //    DropDownList_MZ_PRK1.AppendDataBoundItems = false;
            //    SqlDataSource1.SelectCommand = "SELECT RTRIM(STATIC_NO)+RTRIM(STATIC_NAME),RTRIM(STATIC_NO) FROM A_STATIC WHERE dbo.SUBSTR(STATIC_NO,1,1)='B'";
            //    DropDownList_MZ_PRK1.DataTextField = "RTRIM(STATIC_NO)+RTRIM(STATIC_NAME)";
            //    DropDownList_MZ_PRK1.DataValueField = "RTRIM(STATIC_NO)";
            //    DropDownList_MZ_PRK1.DataBind();

            //    DropDownList_MZ_PRRST.Items.Clear();
            //    DropDownList_MZ_PRRST.AppendDataBoundItems = false;
            //    SqlDataSource2.SelectCommand = "SELECT RTRIM(MZ_KCODE)+RTRIM(MZ_KCHI),RTRIM(MZ_KCODE) FROM A_KTYPE WHERE MZ_KTYPE='24' " +
            //                                   " AND (dbo.SUBSTR(MZ_KCODE,1,1)='2' OR dbo.SUBSTR(MZ_KCODE,1,1)='5' OR dbo.SUBSTR(MZ_KCODE,1,1)='6' " +
            //                                   " OR dbo.SUBSTR(MZ_KCODE,1,1)='A' OR dbo.SUBSTR(MZ_KCODE,1,1)='B')";
            //    DropDownList_MZ_PRRST.DataTextField = "RTRIM(MZ_KCODE)+RTRIM(MZ_KCHI)";
            //    DropDownList_MZ_PRRST.DataValueField = "RTRIM(MZ_KCODE)";
            //    DropDownList_MZ_PRRST.DataBind();
            //}
        }

        protected void btPCODEM_Click(object sender, EventArgs e)
        {
            Session["KTYPE_CID"] = TextBox_MZ_PCODEM.ClientID;
            Session["KTYPE_CID1"] = TextBox_MZ_PCODEM1.ClientID;
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_Ktype_Search.aspx?MZ_KTYPE=PCODEM&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin','top=190,left=200,width=450,height=500,toolbar=0,menubar=0,location=0,directories=0,status=0,scrollbars=yes');", true);
        }

        protected void btPROLNO_Click(object sender, EventArgs e)
        {
            Session["KTYPE_CID"] = TextBox_MZ_PROLNO.ClientID;
            Session["KTYPE_CID1"] = TextBox_MZ_PROLNO1.ClientID;
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_Ktype_Search.aspx?MZ_KTYPE=PROLNO&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin','top=190,left=200,width=450,height=500,toolbar=0,menubar=0,location=0,directories=0,status=0,scrollbars=yes');", true);
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

            Ktype_Cname_Check(CName, TextBox_MZ_OCCC1, TextBox_MZ_OCCC, TextBox_MZ_TBDV);
        }

        protected void TextBox_MZ_SRANK_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_SRANK.Text, "09");

            Ktype_Cname_Check(CName, TextBox_MZ_SRANK1, TextBox_MZ_SRANK, TextBox_MZ_PRK);
        }

        protected void TextBox_MZ_PROLNO_TextChanged(object sender, EventArgs e)
        {
            string CName = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_PRONAME  FROM A_PROLNO WHERE MZ_PROLNO='" + TextBox_MZ_PROLNO.Text.Trim().ToUpper() + "'");

            Ktype_Cname_Check(CName, TextBox_MZ_PROLNO1, TextBox_MZ_PROLNO, TextBox_MZ_PROLNO2);
        }

        protected void TextBox_MZ_PCODEM_TextChanged(object sender, EventArgs e)
        {
            string CName = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_PNAME FROM A_POLNUM WHERE MZ_POLNO='" + TextBox_MZ_PCODEM.Text.Trim().ToUpper() + "'");

            Ktype_Cname_Check(CName, TextBox_MZ_PCODEM1, TextBox_MZ_PCODEM, TextBox_MZ_MEMO);
        }

        protected void TextBox_MZ_TBDV_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_TBDV.Text, "43");

            Ktype_Cname_Check(CName, TextBox_MZ_TBDV1, TextBox_MZ_TBDV, TextBox_MZ_SRANK);
        }

        protected void check_PRK1(int mode)
        {
            TextBox_MZ_PRK1.Text = TextBox_MZ_PRK1.Text.Trim().ToUpper();

            if (string.IsNullOrEmpty(TextBox_MZ_PRK.Text.Trim()))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請先輸入獎懲類別')", true);
                return;
            }

            string CName = o_DBFactory.ABC_toTest.vExecSQL("SELECT STATIC_NAME FROM A_STATIC WHERE dbo.SUBSTR(STATIC_NO,1,1)='" + TextBox_MZ_PRK.Text.Trim().ToUpper().Substring(0, 1) + "' AND STATIC_NO='" + TextBox_MZ_PRK1.Text.Trim().ToUpper() + "'");

            if (mode == 1)
            {
                if (string.IsNullOrEmpty(CName))
                {
                    TextBox_MZ_PRK1.Text = string.Empty;
                    TextBox_MZ_PRK1_1.Text = string.Empty;
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('獎懲類別與統計分類不相符')", true);
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_PRK1.ClientID + "').focus();$get('" + TextBox_MZ_PRK1.ClientID + "').focus();", true);
                }
                else
                {
                    TextBox_MZ_PRK1_1.Text = CName;
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_PRCT.ClientID + "').focus();$get('" + TextBox_MZ_PRCT.ClientID + "').focus();", true);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(CName))
                {
                    TextBox_MZ_PRK1.Text = string.Empty;
                    TextBox_MZ_PRK1_1.Text = string.Empty;
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('獎懲類別與統計分類不相符')", true);
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_PRK1.ClientID + "').focus();$get('" + TextBox_MZ_PRK1.ClientID + "').focus();", true);
                    //checkPRK1 = false;
                }
            }
        }

        protected void TextBox_MZ_PRK1_TextChanged(object sender, EventArgs e)
        {
            check_PRK1(1);
        }

        /// <summary>
        /// 檢核獎懲類別與獎懲結果欄位
        /// </summary>
        /// <param name="mode">1:欄位檢查 other:新修檢查</param>
        /// <returns></returns>
        private bool check_PRRST(int mode)
        {
            TextBox_MZ_PRRST.Text = TextBox_MZ_PRRST.Text.Trim().ToUpper();

            if (string.IsNullOrEmpty(TextBox_MZ_PRK.Text.Trim()))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請先輸入獎懲類別')", true);
                return false;
            }

            A_PRKB_Model a_PRKB_Model = new A_PRKB_Model()
            {
                MZ_PRRST = TextBox_MZ_PRRST.Text.Trim().ToUpper(),
                MZ_PRK = TextBox_MZ_PRK.Text.Trim().ToUpper()
            };
            string CName = PersonalService.ConsistencyCheckFor_PRRST(a_PRKB_Model);

            if (mode == 1)
            {
                if (string.IsNullOrEmpty(CName))
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('獎懲類別與獎懲結果不相符')", true);
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_PRRST.ClientID + "').focus();$get('" + TextBox_MZ_PRRST.ClientID + "').focus();", true);
                }
                else
                {
                    TextBox_MZ_PRRST1.Text = CName;
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_PROLNO.ClientID + "').focus();$get('" + TextBox_MZ_PROLNO.ClientID + "').focus();", true);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(CName))
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('獎懲類別與獎懲結果不相符')", true);
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_PRRST.ClientID + "').focus();$get('" + TextBox_MZ_PRRST.ClientID + "').focus();", true);
                    return false;
                }
            }

            return true;
        }

        protected void TextBox_MZ_PRRST_TextChanged(object sender, EventArgs e)
        {
            check_PRRST(1);
            Check_SWT3();
        }

        protected void Check_SWT3()
        {
            if (int.Parse(o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_SWT3 WHERE MZ_AD='" + TextBox_MZ_AD.Text.Trim().ToUpper() + "'")) > 0
               || int.Parse(o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_SWT3 WHERE MZ_UNIT='" + TextBox_MZ_UNIT.Text.Trim().ToUpper() + "'")) > 0
               || int.Parse(o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_SWT3 WHERE MZ_TBDV='" + TextBox_MZ_TBDV.Text.Trim() + "'")) > 0
               || int.Parse(o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_SWT3 WHERE MZ_OCCC='" + TextBox_MZ_OCCC.Text.Trim().ToUpper() + "' AND MZ_PRRST='" + TextBox_MZ_PRRST.Text.Trim() + "'")) > 0
               || int.Parse(o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_SWT3 WHERE MZ_TBDV='" + TextBox_MZ_TBDV.Text.Trim() + "' AND MZ_PRRST='" + TextBox_MZ_PRRST.Text.Trim() + "'")) > 0)
            {
                DropDownList_MZ_SWT3.SelectedValue = "1";
            }
            else
            {
                DropDownList_MZ_SWT3.SelectedValue = "2";
            }
        }

        protected void btPRK1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBox_MZ_PRK.Text.Trim()))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請先輸入獎懲類別')", true);
                return;
            }

            Session["KTYPE_CID"] = TextBox_MZ_PRK1.ClientID;
            Session["KTYPE_CID1"] = TextBox_MZ_PRK1_1.ClientID;
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_Ktype_Search.aspx?MZ_KTYPE=STATIC&FIRST=" + TextBox_MZ_PRK.Text.Trim().Substring(0, 1) + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin','top=190,left=200,width=450,height=500,toolbar=0,menubar=0,location=0,directories=0,status=0,scrollbars=yes');", true);
        }

        protected void btPRRST_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBox_MZ_PRK.Text.Trim()))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請先輸入獎懲類別')", true);
                return;
            }

            Session["KTYPE_CID"] = TextBox_MZ_PRRST.ClientID;
            Session["KTYPE_CID1"] = TextBox_MZ_PRRST1.ClientID;
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_Ktype_Search.aspx?MZ_KTYPE=24&FIRST=" + TextBox_MZ_PRK.Text.Trim().Substring(0, 1) + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin','top=190,left=200,width=450,height=500,toolbar=0,menubar=0,location=0,directories=0,status=0,scrollbars=yes');", true);
        }

        protected void TextBox_MZ_NO_TextChanged(object sender, EventArgs e)
        {
            Can_not_empty(TextBox_MZ_NO, TextBox_MZ_ID, "案號");
        }

        protected void TextBox_MZ_NAME_TextChanged(object sender, EventArgs e)
        {
            Can_not_empty(TextBox_MZ_NAME, TextBox_MZ_AD, "姓名");
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

        protected void CV_NAME_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (string.IsNullOrEmpty(args.Value))
            {
                args.IsValid = false;
                TextBox_MZ_NAME.BackColor = Color.Orange;
            }
            else
            {
                args.IsValid = true;
                TextBox_MZ_NAME.BackColor = Color.White;
            }
        }

        protected void CV_AD_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (string.IsNullOrEmpty(args.Value))
            {
                args.IsValid = false;
                TextBox_MZ_AD.BackColor = Color.Orange;
            }
            else
            {
                args.IsValid = true;
                TextBox_MZ_AD.BackColor = Color.White;
            }
        }

        protected void CV_UNIT_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (string.IsNullOrEmpty(args.Value))
            {
                args.IsValid = false;
                TextBox_MZ_UNIT.BackColor = Color.Orange;
            }
            else
            {
                args.IsValid = true;
                TextBox_MZ_UNIT.BackColor = Color.White;
            }
        }

        protected void CV_EXAD_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (string.IsNullOrEmpty(args.Value))
            {
                args.IsValid = false;
                TextBox_MZ_EXAD.BackColor = Color.Orange;
            }
            else
            {
                args.IsValid = true;
                TextBox_MZ_EXAD.BackColor = Color.White;
            }
        }

        protected void CV_EXUNIT_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (string.IsNullOrEmpty(args.Value))
            {
                args.IsValid = false;
                TextBox_MZ_EXUNIT.BackColor = Color.Orange;
            }
            else
            {
                args.IsValid = true;
                TextBox_MZ_EXUNIT.BackColor = Color.White;
            }
        }

        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (DropDownList_MZ_PCODE.SelectedValue == "9")
            {
                if (string.IsNullOrEmpty(args.Value))
                {
                    args.IsValid = false;
                    TextBox_MZ_PCODEM.BackColor = Color.Orange;
                }
                else
                {
                    args.IsValid = true;
                    TextBox_MZ_PCODEM.BackColor = Color.White;
                }
            }
        }

        protected void CV_RANK1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (string.IsNullOrEmpty(args.Value))
            {
                args.IsValid = false;
                TextBox_MZ_RANK1.BackColor = Color.Orange;
            }
            else
            {
                args.IsValid = true;
                TextBox_MZ_RANK1.BackColor = Color.White;
            }
        }

        protected void CV_SRANK_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (string.IsNullOrEmpty(args.Value))
            {
                args.IsValid = false;
                TextBox_MZ_SRANK.BackColor = Color.Orange;
            }
            else
            {
                args.IsValid = true;
                TextBox_MZ_SRANK.BackColor = Color.White;
            }
        }

        protected void CV_TBDV_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (string.IsNullOrEmpty(args.Value))
            {
                args.IsValid = false;
                TextBox_MZ_TBDV.BackColor = Color.Orange;
            }
            else
            {
                args.IsValid = true;
                TextBox_MZ_TBDV.BackColor = Color.White;
            }
        }

        protected void CV_OCCC_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (string.IsNullOrEmpty(args.Value))
            {
                args.IsValid = false;
                TextBox_MZ_OCCC.BackColor = Color.Orange;
            }
            else
            {
                args.IsValid = true;
                TextBox_MZ_OCCC.BackColor = Color.White;
            }

        }

        protected void CV_PRK1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (string.IsNullOrEmpty(args.Value))
            {
                args.IsValid = false;
                TextBox_MZ_PRK1.BackColor = Color.Orange;
            }
            else
            {
                args.IsValid = true;
                TextBox_MZ_PRK1.BackColor = Color.White;
            }
        }

        protected void CV_PRCT_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (string.IsNullOrEmpty(args.Value))
            {
                args.IsValid = false;
                TextBox_MZ_PRCT.BackColor = Color.Orange;
            }
            else
            {
                args.IsValid = true;
                TextBox_MZ_PRCT.BackColor = Color.White;
            }
        }

        protected void CV_PRRST_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (string.IsNullOrEmpty(args.Value))
            {
                args.IsValid = false;
                TextBox_MZ_PRRST.BackColor = Color.Orange;
            }
            else
            {
                args.IsValid = true;
                TextBox_MZ_PRRST.BackColor = Color.White;
            }
        }

        protected void CV_PROLNO_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (string.IsNullOrEmpty(args.Value))
            {
                args.IsValid = false;
                TextBox_MZ_PROLNO.BackColor = Color.Orange;
            }
            else
            {
                args.IsValid = true;
                TextBox_MZ_PROLNO.BackColor = Color.White;
            }
        }

        protected void CV_SWT3_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (string.IsNullOrEmpty(args.Value))
            {
                args.IsValid = false;
                DropDownList_MZ_SWT3.BackColor = Color.Orange;
            }
            else
            {
                args.IsValid = true;
                DropDownList_MZ_SWT3.BackColor = Color.White;
            }
        }

        protected void CV_PRK_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (string.IsNullOrEmpty(args.Value))
            {
                args.IsValid = false;
                TextBox_MZ_PRK.BackColor = Color.Orange;
            }
            else
            {
                args.IsValid = true;
                TextBox_MZ_PRK.BackColor = Color.White;
            }
        }

        protected void RequiredFieldValidator1_ValidatorCalloutExtender_Disposed(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_NO.ClientID + "').focus();$get('" + TextBox_MZ_NO.ClientID + "').focus();", true);
        }

        protected void btGroupMZ_NOUpdate_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('PersonalGroupUpdatePRKB_MZ_NO.aspx?MZ_NO=" + TextBox_MZ_NO.Text.Trim() + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "','查詢','top=190,left=200,width=300,height=150,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }

        protected void btPROLNO_DETAIL_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('images/A_PROLNO_DETAIL.htm');", true);

            // Response.Redirect("~/1-personnel/report/A_PROLNO_DETAIL.PDF"); 
        }

        protected void btReplace_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_PRKBUPDATE.aspx?MZ_NO=" + TextBox_MZ_NO.Text.Trim() + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "','查詢','top=190,left=200,width=300,height=300,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }

        int TPM_FION = 0;
        /// <summary>
        /// 列印建議名冊
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button5_Click(object sender, EventArgs e)
        {
            if (TextBox_MZ_NO.Text != string.Empty)
            {               

                string tmp_url = "A_rpt.aspx?fn=gradesuglist&TPM_FION=" + TPM_FION + "&AD=&UNIT=&SWT3=&NO1=" + TextBox_MZ_NO.Text.Trim() + "&NO2=&SORT=1" ;
                switch (ViewState["A_strGID"].ToString())
                {
                    case "A":
                    case "B":
                    case "C":
                        break;
                    case "D":
                    case "E":
                        tmp_url += "&MUSER=" + Session["ADPMZ_ID"].ToString() + "";
                        break;
                }
                ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "location.href('Personal2-1.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "');", true);
            }
        }

        /// <summary>
        /// 列印建議名冊(依流水號排序)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_bysn_Click(object sender, EventArgs e)
        {
            if (TextBox_MZ_NO.Text != string.Empty)
            {       
                string tmp_url = "A_rpt.aspx?fn=gradesuglist&TPM_FION=" + TPM_FION + "&AD=&UNIT=&SWT3=&NO1=" + TextBox_MZ_NO.Text.Trim() + "&NO2=&SORT=2";
                switch (ViewState["A_strGID"].ToString())
                {
                    case "A":
                    case "B":
                    case "C":
                        break;
                    case "D":
                    case "E":
                        tmp_url += "&MUSER=" + Session["ADPMZ_ID"].ToString() + "";
                        break;
                }

                ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "location.href('Personal2-1.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "');", true);
            }
        }

        protected void btAddNote_Click(object sender, EventArgs e)
        {
            string InsertString = "INSERT INTO A_NOTE (MZ_NOTE,MZ_NOTE_NAME) VALUES('" + Session["ADPMZ_ID"].ToString() + "','" + TextBox_MZ_PRCT.Text.Trim() + "')";

            try
            {
                o_DBFactory.ABC_toTest.Edit_Data(InsertString);
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('新增失敗！')", true);
            }
        }

        protected void btDetail_Click(object sender, EventArgs e)
        {
            if (Session["PRKB_Mode"].ToString() == "SEARCH")
            {
                Session["PRKB_DETAIL_ID"] = (ViewState["tbID2"] == null ? "" : ViewState["tbID2"]);
                Session["PRKB_DETAIL_NAME"] = (ViewState["tbNAME2"] == null ? "" : ViewState["tbNAME2"]);
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_PRKB_Detail.aspx?NO=" + (ViewState["tbNO"] == null ? "" : ViewState["tbNO"].ToString()) + "&MZ_AD=" + (ViewState["cbAD"] == null ? "" : ViewState["cbAD"].ToString()) + "&MZ_UNIT=" + (ViewState["cbUNIT"] == null ? "" : ViewState["cbUNIT"].ToString()) + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "','查詢','');", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_PRKB_Detail.aspx?NO=" + TextBox_MZ_NO.Text.Trim() + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "','查詢','');", true);
            }
        }
        /// <summary>
        /// 匯出資料
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btExport_Click(object sender, EventArgs e)
        {
            PRKB_MZ_ID = Session["PRKB_MZ_ID"] as List<string>;
            PRKB_MZ_NO = Session["PRKB_MZ_NO"] as List<string>;
            PRKB_MZ_PRCT = Session["PRKB_MZ_PRCT"] as List<string>;
            string filename = DateTime.Now.Year.ToString() + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second;

            //判斷是否已查明細
            if (ViewState["tbNO"] == null)
            {
                //產生空白表格
                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[] { new DataColumn("案號"), new DataColumn("身分證號"), new DataColumn("姓名"), new DataColumn("職稱"),
                                                        new DataColumn("薪俸職等"), new DataColumn("獎懲內容"), new DataColumn("統計分類代碼"), new DataColumn("統計分類"),
                                                        new DataColumn("獎懲類別號"), new DataColumn("獎懲類別"), new DataColumn("獎懲結果代碼"), new DataColumn("獎懲結果"),
                                                        new DataColumn("獎懲依據編號"), new DataColumn("獎懲依據"), new DataColumn("說明"), new DataColumn("資料序號") });
                for (int i = 0; i < 100; i++)
                {
                    dt.Rows.Add(new object[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" });
                }

                Session["msData"] = new OfficeHelpers.ExcelHelpers().RenderDataTable_ToExcelFileStream(dt, true);
            }
            else
            {
                string strSQL = @"SELECT MZ_NO 案號, MZ_ID 身分證號, MZ_NAME 姓名, RTRIM(ak_OCCC.MZ_KCHI) 職稱, 
                                         RTRIM(ak_SRANK.MZ_KCHI) 薪俸職等, MZ_PRCT 獎懲內容, apb.MZ_PRK1 統計分類代碼, RTRIM(as_PRK1.STATIC_NAME) 統計分類, 
                                         apb.MZ_PRK 獎懲類別號, RTRIM(ak_PRK.MZ_KCHI) 獎懲類別, apb.MZ_PRRST 獎懲結果代碼, RTRIM(ak_PRRST.MZ_KCHI) 獎懲結果, 
                                         apb.MZ_PROLNO 獎懲依據編號, RTRIM(ap_PROLNO.MZ_PRONAME) 獎懲依據, MZ_MEMO 說明, apb.SN 資料序號
                                    FROM A_PRKB apb
                                    left join A_KTYPE ak_OCCC on RTRIM(ak_OCCC.MZ_KTYPE) = '26' And RTRIM(ak_OCCC.MZ_KCODE) = apb.MZ_OCCC
                                    left join A_KTYPE ak_SRANK on RTRIM(ak_SRANK.MZ_KTYPE) = '09' And RTRIM(ak_SRANK.MZ_KCODE) = apb.MZ_SRANK
                                    left join A_STATIC as_PRK1 on as_PRK1.STATIC_NO = apb.MZ_PRK1
                                    left join A_KTYPE ak_PRK on RTRIM(ak_PRK.MZ_KTYPE) = '22' And RTRIM(ak_PRK.MZ_KCODE) = apb.MZ_PRK
                                    left join A_KTYPE ak_PRRST on RTRIM(ak_PRRST.MZ_KTYPE) = '24' And RTRIM(ak_PRRST.MZ_KCODE) = apb.MZ_PRRST
                                    left join A_PROLNO ap_PROLNO on RTRIM(ap_PROLNO.MZ_PROLNO) = apb.MZ_PROLNO
                                    WHERE 1=1 ";
                strSQL += ViewState["tbNO"] == null ? "" : ViewState["tbNO"].ToString() != "" ? string.Format("And MZ_NO = '{0}' ", ViewState["tbNO"].ToString()) : "";
                strSQL += ViewState["tbID2"] == null ? "" : ViewState["tbID2"].ToString() != "" ? string.Format("And MZ_ID = '{0}' ", ViewState["tbID2"].ToString()) : "";
                strSQL += ViewState["tbNAME2"] == null ? "" : ViewState["tbNAME2"].ToString() != "" ? string.Format("And MZ_NAME = '{0}' ", ViewState["tbNAME2"].ToString()) : "";
                if ((ViewState["cbAD"] != null && ViewState["cbAD"].ToString() != "") && (ViewState["cbUNIT"] != null && ViewState["cbUNIT"].ToString() == ""))
                {
                    strSQL += string.Format("And (MZ_AD='{0}' Or MZ_EXAD='{0}') ", ViewState["cbAD"].ToString());
                }
                else if ((ViewState["cbAD"] != null && ViewState["cbAD"].ToString() != "") && (ViewState["cbUNIT"] != null && ViewState["cbUNIT"].ToString() != ""))
                {
                    strSQL += string.Format("And ((MZ_EXAD='{0}' And MZ_EXUNIT='{1}') OR (MZ_AD='{0}' And MZ_UNIT='{1}')) ", ViewState["cbAD"].ToString(), ViewState["cbUNIT"].ToString());
                }
                strSQL += "ORDER BY MZ_NO,SN,MZ_AD,MZ_UNIT,MZ_ID ";
                DataTable dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");

                Session["msData"] = new OfficeHelpers.ExcelHelpers().RenderDataTable_ToExcelFileStream(dt, true);
            }
            Session["fileName"] = filename;

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('ExcelDowload.ashx');", true);
        }

        /// <summary>
        /// 匯入資料
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btImport_Click(object sender, EventArgs e)
        {
            string error = string.Empty;
            if (!fuImport.HasFile)
            {
                error += "請上傳檔案\\n";
            }
            else if (!new List<string>() { ".xls" }.Exists(tmpStr => tmpStr.EndsWith(Path.GetExtension(fuImport.FileName).ToLower())))
            {
                error += "請上傳Excel檔(.xls)\\n";
            }
            if (!string.IsNullOrEmpty(error))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('" + error + "');", true);
            }
            else
            {
                string msg = string.Empty;
                try
                {
                    PersonalService personalService = new PersonalService();
                    HSSFWorkbook workbook = new HSSFWorkbook(fuImport.FileContent);
                    HSSFSheet sheet = workbook.GetSheetAt(0);

                    //建立無法匯入的資料回饋檔
                    DataTable dt = new DataTable();
                    foreach (HSSFCell item in sheet.GetRow(0))
                    {
                        dt.Columns.Add(o_str.NPOIGetCellValue(item));
                    }
                    dt.Columns.Add("錯誤原因");

                    #region 處理資料
                    int okResult = 0, errorResult = 0;
                    for (int i = 1; i <= sheet.LastRowNum; i++)
                    {
                        HSSFRow row = sheet.GetRow(i);
                        A_PRKB_Model model = new A_PRKB_Model();

                        string MZ_ID = o_str.NPOIGetCellValue(row.GetCell(1));
                        //透過ID取回人員資料
                        if (!string.IsNullOrEmpty(MZ_ID))
                        {
                            //取得A_DLBASE資料
                            string strSQL = string.Format("Select * From A_DLBASE WHERE MZ_ID = '{0}' ", MZ_ID.Trim());
                            DataTable tmpDt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "A_DLBASE");
                            if (tmpDt.Rows.Count == 1)
                            {
                                //A_DLBASE資料
                                model.MZ_NAME = tmpDt.Rows[0]["MZ_NAME"].ToString();
                                model.MZ_AD = tmpDt.Rows[0]["MZ_AD"].ToString();
                                model.MZ_UNIT = tmpDt.Rows[0]["MZ_UNIT"].ToString();
                                model.MZ_OCCC = tmpDt.Rows[0]["MZ_OCCC"].ToString();
                                model.MZ_RANK = tmpDt.Rows[0]["MZ_RANK"].ToString();
                                model.MZ_RANK1 = tmpDt.Rows[0]["MZ_RANK1"].ToString();
                                model.MZ_SRANK = tmpDt.Rows[0]["MZ_SRANK"].ToString();
                                model.MZ_TBDV = tmpDt.Rows[0]["MZ_TBDV"].ToString();
                                model.MZ_EXAD = tmpDt.Rows[0]["MZ_EXAD"].ToString();
                                model.MZ_EXUNIT = tmpDt.Rows[0]["MZ_EXUNIT"].ToString();

                                //Excel資料
                                model.MZ_NO = o_str.NPOIGetCellValue(row.GetCell(0));
                                model.MZ_ID = o_str.NPOIGetCellValue(row.GetCell(1));
                                model.MZ_PRCT = o_str.NPOIGetCellValue(row.GetCell(5));//獎懲內容
                                model.MZ_PRK1 = o_str.NPOIGetCellValue(row.GetCell(6)).ToUpperInvariant();//獎懲統計分類
                                model.MZ_PRK = o_str.NPOIGetCellValue(row.GetCell(8)).ToUpperInvariant();//獎懲類別
                                model.MZ_PRRST = o_str.NPOIGetCellValue(row.GetCell(10));//獎懲結果
                                model.MZ_PROLNO = o_str.NPOIGetCellValue(row.GetCell(12));//獎懲依據
                                model.MZ_MEMO = o_str.NPOIGetCellValue(row.GetCell(14));
                                model.SN = o_str.NPOIGetCellValue(row.GetCell(15));//資料序號

                                model.MZ_POLK = "H";
                                model.MZ_PCODE = "9";

                                #region 檢查資料內容規則
                                //獎懲類別
                                strSQL = string.Format("Select * From A_KTYPE WHERE MZ_KTYPE = '22' And MZ_KCODE = '{0}' And rownum < 2 ", model.MZ_PRK);
                                tmpDt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "A_KTYPE");
                                if (tmpDt.Rows.Count == 0)
                                {
                                    errorResult++;
                                    DataRow dr = dt.NewRow();
                                    for (int r = 0; r <= row.LastCellNum; r++)
                                    {
                                        if (r == row.LastCellNum)
                                        {
                                            dr[r] = "查無獎懲類別資料。";
                                        }
                                        else
                                        {
                                            dr[r] = o_str.NPOIGetCellValue(row.GetCell(r));
                                        }
                                    }
                                    dt.Rows.Add(dr);
                                    continue;
                                }
                                //檢查獎懲類別及獎懲統計分類是否一致
                                if (model.MZ_PRK.Substring(0, 1) != model.MZ_PRK1.Substring(0, 1))
                                {
                                    errorResult++;
                                    DataRow dr = dt.NewRow();
                                    for (int r = 0; r <= row.LastCellNum; r++)
                                    {
                                        if (r == row.LastCellNum)
                                        {
                                            dr[r] = "獎懲類別與獎懲統計分類不一致。";
                                        }
                                        else
                                        {
                                            dr[r] = o_str.NPOIGetCellValue(row.GetCell(r));
                                        }
                                    }
                                    dt.Rows.Add(dr);
                                    continue;
                                }

                                //獎懲統計分類
                                strSQL = string.Format("Select * From A_STATIC WHERE STATIC_NO = '{0}' And rownum < 2 ", model.MZ_PRK1);
                                tmpDt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "A_STATIC");
                                if (tmpDt.Rows.Count == 0)
                                {
                                    errorResult++;
                                    DataRow dr = dt.NewRow();
                                    for (int r = 0; r <= row.LastCellNum; r++)
                                    {
                                        if (r == row.LastCellNum)
                                        {
                                            dr[r] = "查無獎懲統計分類資料。";
                                        }
                                        else
                                        {
                                            dr[r] = o_str.NPOIGetCellValue(row.GetCell(r));
                                        }
                                    }
                                    dt.Rows.Add(dr);
                                    continue;
                                }

                                //檢查獎懲類別及獎懲結果是否一致
                                if (model.MZ_PRK.Substring(0, 1) == "A")
                                {
                                    if (model.MZ_PRRST.Substring(0, 1) != "4" && model.MZ_PRRST.Substring(0, 1) != "7" 
                                        && model.MZ_PRRST.Substring(0, 1) != "8" && model.MZ_PRRST.Substring(0, 1) != "9")
                                    {
                                        errorResult++;
                                        DataRow dr = dt.NewRow();
                                        for (int r = 0; r <= row.LastCellNum; r++)
                                        {
                                            if (r == row.LastCellNum)
                                            {
                                                dr[r] = "獎懲類別與獎懲結果不一致。";
                                            }
                                            else
                                            {
                                                dr[r] = o_str.NPOIGetCellValue(row.GetCell(r));
                                            }
                                        }
                                        dt.Rows.Add(dr);
                                        continue;
                                    }
                                }
                                else
                                {
                                    if (model.MZ_PRRST.Substring(0, 1) != "2" && model.MZ_PRRST.Substring(0, 1) != "5" && model.MZ_PRRST.Substring(0, 1) != "6" 
                                        && model.MZ_PRRST.Substring(0, 1) != "A" && model.MZ_PRRST.Substring(0, 1) != "B")
                                    {
                                        errorResult++;
                                        DataRow dr = dt.NewRow();
                                        for (int r = 0; r <= row.LastCellNum; r++)
                                        {
                                            if (r == row.LastCellNum)
                                            {
                                                dr[r] = "獎懲類別與獎懲結果不一致。";
                                            }
                                            else
                                            {
                                                dr[r] = o_str.NPOIGetCellValue(row.GetCell(r));
                                            }
                                        }
                                        dt.Rows.Add(dr);
                                        continue;
                                    }
                                }

                                //獎懲結果
                                strSQL = string.Format("Select * From A_KTYPE WHERE MZ_KTYPE = '24' And MZ_KCODE = '{0}' And rownum < 2 ", model.MZ_PRRST);
                                tmpDt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "A_KTYPE");
                                if (tmpDt.Rows.Count == 0)
                                {
                                    errorResult++;
                                    DataRow dr = dt.NewRow();
                                    for (int r = 0; r <= row.LastCellNum; r++)
                                    {
                                        if (r == row.LastCellNum)
                                        {
                                            dr[r] = "查無獎懲結果資料。";
                                        }
                                        else
                                        {
                                            dr[r] = o_str.NPOIGetCellValue(row.GetCell(r));
                                        }
                                    }
                                    dt.Rows.Add(dr);
                                    continue;
                                }

                                //獎懲依據
                                strSQL = string.Format("Select * From A_PROLNO WHERE MZ_PROLNO = '{0}' And rownum < 2 ", model.MZ_PROLNO);
                                tmpDt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "A_PROLNO");
                                if (tmpDt.Rows.Count == 0)
                                {
                                    errorResult++;
                                    DataRow dr = dt.NewRow();
                                    for (int r = 0; r <= row.LastCellNum; r++)
                                    {
                                        if (r == row.LastCellNum)
                                        {
                                            dr[r] = "查無獎懲依據資料。";
                                        }
                                        else
                                        {
                                            dr[r] = o_str.NPOIGetCellValue(row.GetCell(r));
                                        }
                                    }
                                    dt.Rows.Add(dr);
                                    continue;
                                }
                                #endregion

                                //權責資料判斷
                                if (int.Parse(o_DBFactory.ABC_toTest.vExecSQL(string.Format("SELECT COUNT(*) FROM A_SWT3 WHERE MZ_AD='{0}'", model.MZ_AD.Trim().ToUpperInvariant()))) > 0
                                    || int.Parse(o_DBFactory.ABC_toTest.vExecSQL(string.Format("SELECT COUNT(*) FROM A_SWT3 WHERE MZ_UNIT='{0}'", model.MZ_UNIT.Trim().ToUpperInvariant()))) > 0
                                    || int.Parse(o_DBFactory.ABC_toTest.vExecSQL(string.Format("SELECT COUNT(*) FROM A_SWT3 WHERE MZ_TBDV='{0}'", model.MZ_TBDV.Trim()))) > 0
                                    || int.Parse(o_DBFactory.ABC_toTest.vExecSQL(string.Format("SELECT COUNT(*) FROM A_SWT3 WHERE MZ_OCCC='{0}' AND MZ_PRRST='{1}'", model.MZ_OCCC.Trim().ToUpperInvariant(), model.MZ_PRRST.Trim()))) > 0
                                    || int.Parse(o_DBFactory.ABC_toTest.vExecSQL(string.Format("SELECT COUNT(*) FROM A_SWT3 WHERE MZ_TBDV='{0}' AND MZ_PRRST='{1}'", model.MZ_TBDV.Trim(), model.MZ_PRRST.Trim()))) > 0)
                                {
                                    model.MZ_SWT3 = "1";
                                }
                                else
                                {
                                    model.MZ_SWT3 = "2";
                                }

                                model.MUSER = Session["ADPMZ_ID"].ToString();
                                model.MDATE = o_str.TWDateConvert(DateTime.Now, "{0}{1}{2}");

                                //檢查重複資料
                                if (!personalService.DuplicateCheckFor_APRKB(model))
                                {
                                    errorResult++;
                                    DataRow dr = dt.NewRow();
                                    for (int r = 0; r <= row.LastCellNum; r++)
                                    {
                                        if (r == row.LastCellNum)
                                        {
                                            //dr[r] = "身分證號、案號、獎懲類別、獎懲內容重複。";
                                            dr[r] = "身分證號、獎懲類別、獎懲內容重複。";
                                        }
                                        else
                                        {
                                            dr[r] = o_str.NPOIGetCellValue(row.GetCell(r));
                                        }
                                    }
                                    dt.Rows.Add(dr);
                                    continue;
                                }

                                //新增資料
                                if (Import_savedate(model))
                                {
                                    okResult++;
                                }
                                else
                                {
                                    errorResult++;
                                    DataRow dr = dt.NewRow();
                                    for (int r = 0; r <= row.LastCellNum; r++)
                                    {
                                        if (r == row.LastCellNum)
                                        {
                                            dr[r] = "新增或更新該筆資料失敗。";
                                        }
                                        else
                                        {
                                            dr[r] = o_str.NPOIGetCellValue(row.GetCell(r));
                                        }
                                    }
                                    dt.Rows.Add(dr);
                                }
                            }
                            else if (tmpDt.Rows.Count == 0)
                            {
                                errorResult++;
                                DataRow dr = dt.NewRow();
                                for (int r = 0; r <= row.LastCellNum; r++)
                                {
                                    if (r == row.LastCellNum)
                                    {
                                        dr[r] = "找不到該身分證人員。";
                                    }
                                    else
                                    {
                                        dr[r] = o_str.NPOIGetCellValue(row.GetCell(r));
                                    }
                                }
                                dt.Rows.Add(dr);
                            }
                            else
                            {
                                errorResult++;
                                DataRow dr = dt.NewRow();
                                for (int r = 0; r <= row.LastCellNum; r++)
                                {
                                    if (r == row.LastCellNum)
                                    {
                                        dr[r] = "無法特定該身分證人員。";
                                    }
                                    else
                                    {
                                        dr[r] = o_str.NPOIGetCellValue(row.GetCell(r));
                                    }
                                }
                                dt.Rows.Add(dr);
                            }
                        }
                        else
                        {
                            errorResult++;
                            DataRow dr = dt.NewRow();
                            for (int r = 0; r <= row.LastCellNum; r++)
                            {
                                if (r == row.LastCellNum)
                                {
                                    dr[r] = "身分證欄位空白。";
                                }
                                else
                                {
                                    dr[r] = o_str.NPOIGetCellValue(row.GetCell(r));
                                }
                            }
                            dt.Rows.Add(dr);
                        }
                    }
                    #endregion

                    //處理完畢
                    sheet = null;
                    workbook = null;

                    if (dt.Rows.Count > 0)
                    {
                        msg = string.Format("匯入完成，成功{0}筆,失敗{1}筆。", okResult, errorResult);
                        Session["msData"] = new OfficeHelpers.ExcelHelpers().RenderDataTable_ToExcelFileStream(dt, true);
                        Session["fileName"] = string.Format("ErrorData_{0}", DateTime.Now.ToString("yyyy-MM-dd"));
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('" + msg + "'); window.open('ExcelDowload.ashx');", true);
                    }
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('資料匯入失敗');", true);
                }
            }
        }
        /// <summary>
        /// 開啟上傳Dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Open_Import_Click(object sender, EventArgs e)
        {
            Panel_Excel_ModalPopupExtender.Show();
        }
        /// <summary>
        /// Excel匯入 新增SQL執行
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private bool Import_savedate(A_PRKB_Model model)
        {
            List<SqlParameter> paramerte = new List<SqlParameter>() {
                new SqlParameter("MZ_NAME", model.MZ_NAME),
                new SqlParameter("MZ_AD", model.MZ_AD),
                new SqlParameter("MZ_UNIT", model.MZ_UNIT),
                new SqlParameter("MZ_OCCC", model.MZ_OCCC),
                new SqlParameter("MZ_RANK", model.MZ_RANK),
                new SqlParameter("MZ_RANK1", model.MZ_RANK1),
                new SqlParameter("MZ_SRANK", model.MZ_SRANK),
                new SqlParameter("MZ_TBDV", model.MZ_TBDV),
                new SqlParameter("MZ_EXAD", model.MZ_EXAD),
                new SqlParameter("MZ_EXUNIT", model.MZ_EXUNIT),
                new SqlParameter("MZ_NO", model.MZ_NO),
                new SqlParameter("MZ_ID", model.MZ_ID),
                new SqlParameter("MZ_PRCT", model.MZ_PRCT),
                new SqlParameter("MZ_PRK1", model.MZ_PRK1),
                new SqlParameter("MZ_PRK", model.MZ_PRK),
                new SqlParameter("MZ_PRRST", model.MZ_PRRST),
                new SqlParameter("MZ_PROLNO", model.MZ_PROLNO),
                new SqlParameter("MZ_MEMO", model.MZ_MEMO),
                new SqlParameter("MZ_POLK", model.MZ_POLK),
                new SqlParameter("MZ_PCODE", model.MZ_PCODE),
                new SqlParameter("MZ_SWT3", model.MZ_SWT3),
                new SqlParameter("MUSER", model.MUSER),
                new SqlParameter("MDATE", model.MDATE),
            };

            try
            {
                if (string.IsNullOrEmpty(model.SN))
                {
                    //不用手動寫A_PRKB_SN.NEXTVAL，已有觸發程序A_PRKB_INSERT_SM_copy自動新增編號
                    string strSQL = @"INSERT INTO A_PRKB (MZ_NO,MZ_ID,MZ_NAME,MZ_AD,MZ_UNIT,MZ_OCCC,MZ_RANK,MZ_RANK1,MZ_SRANK,MZ_TBDV,MZ_PRCT,
                                                      MZ_PRK1,MZ_PRK,MZ_PRRST,MZ_PROLNO,MZ_POLK,MZ_PCODE,MZ_MEMO,MZ_SWT3,MZ_EXAD,MZ_EXUNIT,MUSER,MDATE)
                                              VALUES (@MZ_NO,@MZ_ID,@MZ_NAME,@MZ_AD,@MZ_UNIT,@MZ_OCCC,@MZ_RANK,@MZ_RANK1,@MZ_SRANK,@MZ_TBDV,@MZ_PRCT,
                                                      @MZ_PRK1,@MZ_PRK,@MZ_PRRST,@MZ_PROLNO,@MZ_POLK,@MZ_PCODE,@MZ_MEMO,@MZ_SWT3,@MZ_EXAD,@MZ_EXUNIT,@MUSER,@MDATE)";
                    o_DBFactory.ABC_toTest.ExecuteNonQuery( strSQL, paramerte.ToArray());
                }
                else
                {
                    paramerte.Add(new SqlParameter("SN", model.SN));
                    string strSQL = @"UPDATE A_PRKB SET MZ_NO=@MZ_NO, MZ_ID=@MZ_ID, MZ_NAME=@MZ_NAME, MZ_AD=@MZ_AD, MZ_UNIT=@MZ_UNIT, MZ_OCCC=@MZ_OCCC, MZ_RANK=@MZ_RANK, MZ_RANK1=@MZ_RANK1, 
                                                        MZ_SRANK=@MZ_SRANK, MZ_TBDV=@MZ_TBDV, MZ_PRCT=@MZ_PRCT, MZ_PRK1=@MZ_PRK1, MZ_PRK=@MZ_PRK, MZ_PRRST=@MZ_PRRST, MZ_PROLNO=@MZ_PROLNO, 
                                                        MZ_POLK=@MZ_POLK, MZ_PCODE=@MZ_PCODE, MZ_MEMO=@MZ_MEMO, MZ_SWT3=@MZ_SWT3, MZ_EXAD=@MZ_EXAD, MZ_EXUNIT=@MZ_EXUNIT, MUSER=@MUSER, MDATE=@MDATE 
                                        WHERE SN=@SN ";
                    o_DBFactory.ABC_toTest.ExecuteNonQuery( strSQL, paramerte.ToArray());
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
