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
    public partial class Personal3_3 : System.Web.UI.Page
    {
        List<string> POSIT2_MZ_ID;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                A.check_power();
            }
           

            ViewState["POSIT2_PRID1"] = Request["POSIT2_PRID1"];

            ViewState["POSIT2_ID"] = Session["POSIT2_ID"];
            Session.Remove("POSIT2_ID");

            ViewState["POSIT2_NAME"] = Session["POSIT2_NAME"];
            Session.Remove("POSIT2_NAME");

            ////查詢ID
            //HttpCookie POSIT2_ID_Cookie = new HttpCookie("POSIT2_ID");
            //POSIT2_ID_Cookie = Request.Cookies["POSIT2_ID"];

            //if (POSIT2_ID_Cookie == null)
            //{
            //    ViewState["POSIT2_ID"] = "";
            //    Response.Cookies["POSIT2_ID"].Expires = DateTime.Now.AddYears(-1);
            //}
            //else
            //{
            //    ViewState["POSIT2_ID"] = TPMPermissions._strDecod(POSIT2_ID_Cookie.Value.ToString());
            //    Response.Cookies["POSIT2_ID"].Expires = DateTime.Now.AddYears(-1);
            //}

            ////查詢姓名
            //HttpCookie POSIT2_NAME_Cookie = new HttpCookie("POSIT2_NAME");
            //POSIT2_NAME_Cookie = Request.Cookies["POSIT2_NAME"];

            //if (POSIT2_NAME_Cookie == null)
            //{
            //    ViewState["POSIT2_NAME"] = "";
            //    Response.Cookies["POSIT2_NAME"].Expires = DateTime.Now.AddYears(-1);
            //}
            //else
            //{
            //    ViewState["POSIT2_NAME"] = TPMPermissions._strDecod(POSIT2_NAME_Cookie.Value.ToString());
            //    Response.Cookies["POSIT2_NAME"].Expires = DateTime.Now.AddYears(-1);
            //}

            if (Session["SEARCH"] != null)
            {

                ViewState["POSIT2_PRID"] = Session["POSIT2_PRID"];
                Session.Remove("POSIT2_PRID");
                //HttpCookie POSIT2_PRID_Cookie = new HttpCookie("POSIT2_PRID");

                //POSIT2_PRID_Cookie = Request.Cookies["POSIT2_PRID"];

                /////發文文號用cookie作,用session 會造成下方程式無限迴圈
                //if (POSIT2_PRID_Cookie == null)
                //{
                //    ViewState["POSIT2_PRID"] = null;
                //    Response.Cookies["POSIT2_PRID"].Expires = DateTime.Now.AddYears(-1);
                //}
                //else
                //{
                //    ViewState["POSIT2_PRID"] = TPMPermissions._strDecod(POSIT2_PRID_Cookie.Value.ToString());
                //    Response.Cookies["POSIT2_PRID"].Expires = DateTime.Now.AddYears(-1);
                //}

                Session.Remove("SEARCH");
            }

            ViewState["POSIT2_DATE"] = Request["DATE"];
            ViewState["XCOUNT"] = Request["XCOUNT"];

            A.set_Panel_EnterToTAB(ref this.Panel1);
            A.set_Panel_EnterToTAB(ref this.Panel2);

            if (Session["POSIT2_Mode"] != null)
            {
                ViewState["Mode"] = Session["POSIT2_Mode"];
                ViewState["SqlString"] = Session["POSIT2_CMDSQL"];
                Session.Remove("POSIT2_Mode");
                Session.Remove("POSIT2_CMDSQL");
            }

            ViewState["MZ_ID"] = Session["PersonalSearchIDwithNAME_MZ_ID5"];
            Session.Remove("PersonalSearchIDwithNAME_MZ_ID5");

            //HttpCookie POSIT2_MZ_ID_Cookie = new HttpCookie("PersonalSearchIDwithNAME_MZ_ID1");
            //POSIT2_MZ_ID_Cookie = Request.Cookies["PersonalSearchIDwithNAME_MZ_ID1"];

            //if (POSIT2_MZ_ID_Cookie == null)
            //{
            //    ViewState["MZ_ID"] = null;
            //    Response.Cookies["PersonalSearchIDwithNAME_MZ_ID1"].Expires = DateTime.Now.AddYears(-1);
            //}
            //else
            //{
            //    ViewState["MZ_ID"] = TPMPermissions._strDecod(POSIT2_MZ_ID_Cookie.Value.ToString());
            //    Response.Cookies["PersonalSearchIDwithNAME_MZ_ID1"].Expires = DateTime.Now.AddYears(-1);
            //}

            TextBox_MZ_NREA.Attributes.Add("onKeyDown", "if (event.keyCode==9){__doPostBack('" + TextBox_MZ_NREA.ClientID + "','')}");

            if (Request.Form["__EVENTTARGET"] == TextBox_MZ_NREA.ClientID)
                Can_not_empty(TextBox_MZ_NREA, TextBox_MZ_AD, "任職原因");

            if (!Page.IsPostBack)
            {
                bool Change = false;

                TextBox_MZ_AD1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_EXOAD1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_CHISI1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_EXOUNIT1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_NREA1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_OCCC1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_RANK_1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_RANK1_1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_SLVC1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_SRANK1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_TBDV1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_UNIT1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_EXOOCC1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_EXORANK_1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_EXORANK_1.Attributes.Add("onkeydown", "javascript:return false;");
                //TextBox_MZ_TBNREA1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_NIN1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_NRT1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_PCHIEF1.Attributes.Add("onkeydown", "javascript:return false;");
                TextBox_MZ_PESN1.Attributes.Add("onkeydown", "javascript:return false;");

                if (ViewState["MZ_ID"] != null)
                {

                    preLoad(ViewState["MZ_ID"].ToString());
                    btCancel.Enabled = true;
                    btOK.Enabled = true;
                    btUpdate.Enabled = false;
                    btDelete.Enabled = false;
                    btUpper.Enabled = false;
                    btNEXT.Enabled = false;
                    btInsert.Enabled = false;
                    Change = true;
                }

                if (ViewState["XCOUNT"] != null)
                {
                    finddata(int.Parse(ViewState["XCOUNT"].ToString()));
                    xcount.Text = ViewState["XCOUNT"].ToString();
                    if (int.Parse(ViewState["XCOUNT"].ToString()) == 0 && POSIT2_MZ_ID.Count > 1)
                    {
                        btNEXT.Enabled = true;
                        btUpper.Enabled = false;
                    }
                    else if (int.Parse(ViewState["XCOUNT"].ToString()) > 0 && int.Parse(ViewState["XCOUNT"].ToString()) < POSIT2_MZ_ID.Count - 1)
                    {
                        btNEXT.Enabled = true;
                        btUpper.Enabled = true;
                    }
                    else if (int.Parse(ViewState["XCOUNT"].ToString()) == POSIT2_MZ_ID.Count - 1)
                    {
                        btUpper.Enabled = true;
                        btNEXT.Enabled = false;
                    }
                    else
                    {
                        btNEXT.Enabled = false;
                        btUpper.Enabled = false;
                    }

                    if (POSIT2_MZ_ID.Count == 0)
                    {
                        Label1.Visible = false;
                    }
                    else
                    {
                        Label1.Visible = true;
                        Label1.Text = "第" + (int.Parse(xcount.Text) + 1).ToString() + "筆共" + POSIT2_MZ_ID.Count.ToString() + "筆";
                    }

                    btUpdate.Enabled = true;
                    btOK.Enabled = false;
                    btCancel.Enabled = false;
                    btDelete.Enabled = false;
                    ViewState["SEARCH"] = null;
                    Change = false;
                }

                if (ViewState["POSIT2_PRID"] != null)
                {
                    string strSQL = "SELECT MZ_ID FROM A_POSIT2 WHERE 1=1";

                    strSQL += " AND MZ_PRID='" + ViewState["POSIT2_PRID"].ToString() + "' AND MZ_PRID1='" + ViewState["POSIT2_PRID1"].ToString() + "'";

                    if (!string.IsNullOrEmpty(ViewState["POSIT2_ID"].ToString()))
                    {
                        strSQL += " AND MZ_ID='" + ViewState["POSIT2_ID"].ToString() + "'";
                    }

                    if (!string.IsNullOrEmpty(ViewState["POSIT2_NAME"].ToString()))
                    {
                        strSQL += " AND MZ_NAME='" + ViewState["POSIT2_NAME"].ToString() + "'";
                    }


                    strSQL += " ORDER BY MZ_PRID,MZ_PRID1";

                    POSIT2_MZ_ID = o_DBFactory.ABC_toTest.DataListArray(strSQL, "MZ_ID");


                    Session["POSIT2_MZ_ID"] = POSIT2_MZ_ID;


                    if (POSIT2_MZ_ID.Count == 0)
                    {
                        Response.Write(@"<script language=javascript>window.alert('查無資料！');location.href('Personal3-3.aspx?TPM_FION=" + Request["TPM_FION"].ToString() + "');</script>");
                    }
                    else if (POSIT2_MZ_ID.Count == 1)
                    {
                        btUpdate.Enabled = true;
                        xcount.Text = "0";
                        finddata(int.Parse(xcount.Text));
                    }
                    else
                    {
                        btUpdate.Enabled = true;
                        btNEXT.Enabled = true;
                        xcount.Text = "0";
                        finddata(int.Parse(xcount.Text));
                    }

                    if (POSIT2_MZ_ID.Count == 0)
                    {
                        Label1.Visible = false;
                    }
                    else
                    {
                        Label1.Visible = true;
                        Label1.Text = "第" + (int.Parse(xcount.Text) + 1).ToString() + "筆共" + POSIT2_MZ_ID.Count.ToString() + "筆";
                    }
                    ViewState["SEARCH"] = null;
                    Change = false;
                }
                A.controlEnable(ref this.Panel2, Change);

                if (Change)
                {
                    EXTPOS();
                    EXTPOS2();
                }
            }
        }

        protected void preLoad(string MZ_ID)
        {
            string FindString = @"SELECT MZ_NAME,MZ_ID,MZ_AD,MZ_AD_CH,MZ_UNIT,MZ_UNIT_CH,MZ_EXAD,MZ_EXAD_CH,MZ_EXUNIT,MZ_EXUNIT_CH,
MZ_RANK,MZ_RANK_CH,MZ_RANK1,MZ_RANK1_CH,MZ_OCCC,MZ_OCCC_CH,MZ_SRANK,MZ_SRANK_CH,
MZ_EXTPOS,MZ_ISEXTPOS,MZ_SRANK,MZ_SPT,MZ_SLVC ,MZ_SLVC_CH FROM VW_A_DLBASE_S2 WHERE MZ_ID='" + MZ_ID + "'";

            DataTable FindDt = o_DBFactory.ABC_toTest.Create_Table(FindString, "123");
            if (FindDt.Rows.Count == 1)
            {
                TextBox_MZ_NAME.Text = FindDt.Rows[0]["MZ_NAME"].ToString().Trim();
                TextBox_MZ_ID.Text = FindDt.Rows[0]["MZ_ID"].ToString().Trim();
                TextBox_MZ_EXOAD.Text = FindDt.Rows[0]["MZ_AD"].ToString().Trim();
                TextBox_MZ_EXOUNIT.Text = FindDt.Rows[0]["MZ_UNIT"].ToString().Trim();
                TextBox_MZ_EXOOCC.Text = FindDt.Rows[0]["MZ_OCCC"].ToString().Trim();
                TextBox_MZ_EXORANK.Text = FindDt.Rows[0]["MZ_RANK"].ToString().Trim();
                TextBox_MZ_EXRANK1.Text = FindDt.Rows[0]["MZ_RANK1"].ToString().Trim();
                TextBox_MZ_SLVC.Text = FindDt.Rows[0]["MZ_SLVC"].ToString().Trim();
                TextBox_MZ_SPT.Text = FindDt.Rows[0]["MZ_SPT"].ToString().Trim();
                TextBox_MZ_SRANK.Text = FindDt.Rows[0]["MZ_SRANK"].ToString().Trim();

                RadioButtonList_MZ_ISEXTPOS.SelectedValue = string.IsNullOrEmpty(FindDt.Rows[0]["MZ_ISEXTPOS"].ToString().Trim()) ? "N" : FindDt.Rows[0]["MZ_ISEXTPOS"].ToString().Trim();

                if (RadioButtonList_MZ_ISEXTPOS.SelectedValue == "Y")
                {
                    DropDownList_MZ_EXTPOS.DataBind();
                    DropDownList_MZ_EXTPOS.SelectedValue = FindDt.Rows[0]["MZ_EXTPOS"].ToString().Trim();
                }
                else
                {
                    DropDownList_MZ_EXTPOS.DataBind();
                    DropDownList_MZ_EXTPOS.SelectedValue = "";
                }

                TextBox_MZ_EXOAD1.Text = FindDt.Rows[0]["MZ_AD_CH"].ToString().Trim();
                TextBox_MZ_EXOUNIT1.Text = FindDt.Rows[0]["MZ_UNIT_CH"].ToString().Trim();
                TextBox_MZ_EXOOCC1.Text = FindDt.Rows[0]["MZ_OCCC_CH"].ToString().Trim();
                TextBox_MZ_EXORANK_1.Text = FindDt.Rows[0]["MZ_RANK_CH"].ToString().Trim();
                TextBox_MZ_EXRANK1_1.Text = FindDt.Rows[0]["MZ_RANK1_CH"].ToString().Trim();
                TextBox_MZ_SLVC1.Text = FindDt.Rows[0]["MZ_SLVC_CH"].ToString().Trim();
                TextBox_MZ_SRANK1.Text = FindDt.Rows[0]["MZ_SRANK_CH"].ToString().Trim();

                //TextBox_MZ_EXOAD1.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)='38213' UNION ALL SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)<>'38213') WHERE MZ_KCODE='" + TextBox_MZ_EXOAD.Text.Trim() + "'");
                //TextBox_MZ_EXOUNIT1.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='25' AND MZ_KCODE IN (SELECT MZ_UNIT FROM A_UNIT_AD WHERE MZ_AD='" + o_str.tosql(TextBox_MZ_EXOAD.Text.Trim()) + "') AND MZ_KCODE='" + o_str.tosql(TextBox_MZ_EXOUNIT.Text.Trim()) + "'");
                //TextBox_MZ_EXOOCC1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_EXOOCC.Text, "26");
                //TextBox_MZ_EXORANK_1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_EXORANK.Text, "09");
                //TextBox_MZ_EXRANK1_1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_EXRANK1.Text, "09");
                //TextBox_MZ_SLVC1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_SLVC.Text, "64");
                //TextBox_MZ_SRANK1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_SRANK.Text, "09");

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_AD.ClientID + "').focus();$get('" + TextBox_MZ_AD.ClientID + "').focus();", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('無資料!')", true);

            }
        }

       

        protected void Can_not_empty(TextBox tb1, object obj, string fieldname)
        {
            if (string.IsNullOrEmpty(tb1.Text.Trim()))
            {
                tb1.Focus();
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('" + fieldname + "不可空白')", true);
            }
            else
            {
                if (obj is TextBox)
                {
                    (obj as TextBox).Focus();
                }
                else if (obj is RadioButtonList)
                {
                    (obj as RadioButtonList).Focus();
                }
            }
        }

        protected void finddata(int Datacount)
        {

            POSIT2_MZ_ID = Session["POSIT2_MZ_ID"] as List<string>;

            string strSQL = "SELECT * FROM  A_POSIT2  WHERE MZ_ID='" + POSIT2_MZ_ID[Datacount] + "' AND MZ_PRID='" + ViewState["POSIT2_PRID"].ToString() + "' AND MZ_PRID1='" + ViewState["POSIT2_PRID1"].ToString() + "'";
            DataTable dt = new DataTable();
            dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");

            if (dt.Rows.Count == 1)
            {

                TextBox_MZ_PRID.Text = dt.Rows[0]["MZ_PRID"].ToString().Trim();
                TextBox_MZ_PRID1.Text = dt.Rows[0]["MZ_PRID1"].ToString().Trim();

                TextBox_MZ_ID.Text = dt.Rows[0]["MZ_ID"].ToString().Trim();
                TextBox_MZ_NAME.Text = dt.Rows[0]["MZ_NAME"].ToString().Trim();
                TextBox_MZ_EXOAD.Text = dt.Rows[0]["MZ_EXOAD"].ToString().Trim();
                TextBox_MZ_EXOUNIT.Text = dt.Rows[0]["MZ_EXOUNIT"].ToString().Trim();
                TextBox_MZ_EXOOCC.Text = dt.Rows[0]["MZ_EXOOCC"].ToString().Trim();
                TextBox_MZ_EXORANK.Text = dt.Rows[0]["MZ_EXORANK"].ToString().Trim();
                TextBox_MZ_EXRANK1.Text = dt.Rows[0]["MZ_EXRANK1"].ToString().Trim();
                TextBox_MZ_EXOPOS.Text = dt.Rows[0]["MZ_EXOPOS"].ToString().Trim();
                //TextBox_MZ_TBNREA.Text = dt.Rows[0]["MZ_TBNREA"].ToString().Trim();
                //TextBox_MZ_TBDATE.Text = o_CommonService.Personal_ReturnDateString(dt.Rows[0]["MZ_TBDATE"].ToString().Trim());
                //TextBox_MZ_TBID.Text = dt.Rows[0]["MZ_TBID"].ToString().Trim();
                TextBox_MZ_EXOPNO.Text = dt.Rows[0]["MZ_EXOPNO"].ToString().Trim();
                TextBox_MZ_AD.Text = dt.Rows[0]["MZ_AD"].ToString().Trim();
                TextBox_MZ_UNIT.Text = dt.Rows[0]["MZ_UNIT"].ToString().Trim();
                TextBox_MZ_OCCC.Text = dt.Rows[0]["MZ_OCCC"].ToString().Trim();
                TextBox_MZ_RANK.Text = dt.Rows[0]["MZ_RANK"].ToString().Trim();
                TextBox_MZ_RANK1.Text = dt.Rows[0]["MZ_RANK1"].ToString().Trim();
                TextBox_MZ_CHISI.Text = dt.Rows[0]["MZ_CHISI"].ToString().Trim();
                TextBox_MZ_POSIND.Text = dt.Rows[0]["MZ_POSIND"].ToString().Trim();
                TextBox_MZ_NIN.Text = dt.Rows[0]["MZ_NIN"].ToString().Trim();
                TextBox_MZ_NREA.Text = dt.Rows[0]["MZ_NREA"].ToString().Trim();
                TextBox_MZ_TBDV.Text = dt.Rows[0]["MZ_TBDV"].ToString().Trim();
                TextBox_MZ_PCHIEF.Text = dt.Rows[0]["MZ_PCHIEF"].ToString().Trim();
                TextBox_MZ_PESN.Text = dt.Rows[0]["MZ_PESN"].ToString().Trim();
                TextBox_MZ_PNO.Text = dt.Rows[0]["MZ_PNO"].ToString().Trim();
                TextBox_MZ_TNO.Text = dt.Rows[0]["MZ_TNO"].ToString().Trim();
                TextBox_MZ_WDATE.Text = o_CommonService.Personal_ReturnDateString(dt.Rows[0]["MZ_WDATE"].ToString().Trim());
                TextBox_MZ_SRANK.Text = dt.Rows[0]["MZ_SRANK"].ToString().Trim();
                TextBox_MZ_SLVC.Text = dt.Rows[0]["MZ_SLVC"].ToString().Trim();
                TextBox_MZ_SPT.Text = dt.Rows[0]["MZ_SPT"].ToString().Trim();
                TextBox_MZ_SPT1.Text = dt.Rows[0]["MZ_SPT1"].ToString().Trim();
                TextBox_MZ_MEMO.Text = dt.Rows[0]["MZ_MEMO"].ToString().Trim();
                TextBox_MZ_RET.Text = dt.Rows[0]["MZ_RET"].ToString().Trim();
                TextBox_MZ_REMARK.Text = dt.Rows[0]["MZ_REMARK"].ToString().Trim();
                TextBox_MZ_SWT.Text = dt.Rows[0]["MZ_SWT"].ToString().Trim();

                RadioButtonList_MZ_ISEXTPOS.SelectedValue = string.IsNullOrEmpty(dt.Rows[0]["MZ_ISEXTPOS"].ToString().Trim()) ? "N" : dt.Rows[0]["MZ_ISEXTPOS"].ToString().Trim();
                RadioButtonList_MZ_ISEXTPOS2.SelectedValue = string.IsNullOrEmpty(dt.Rows[0]["MZ_ISEXTPOS2"].ToString().Trim()) ? "N" : dt.Rows[0]["MZ_ISEXTPOS2"].ToString().Trim();

                if (RadioButtonList_MZ_ISEXTPOS.SelectedValue == "Y")
                {
                    DropDownList_MZ_EXTPOS.DataBind();
                    DropDownList_MZ_EXTPOS.SelectedValue = dt.Rows[0]["MZ_EXTPOS"].ToString().Trim();
                }
                else
                {
                    DropDownList_MZ_EXTPOS.DataBind();
                    DropDownList_MZ_EXTPOS.SelectedValue = "";
                }

                if (RadioButtonList_MZ_ISEXTPOS2.SelectedValue == "Y")
                {
                    DropDownList_MZ_EXTPOS2.DataBind();
                    DropDownList_MZ_EXTPOS2.SelectedValue = dt.Rows[0]["MZ_EXTPOS2"].ToString().Trim();
                }
                else
                {
                    DropDownList_MZ_EXTPOS2.DataBind();
                    DropDownList_MZ_EXTPOS2.SelectedValue = "";
                }

                TextBox_MZ_NRT.Text = dt.Rows[0]["MZ_NRT"].ToString().Trim();
                TextBox_CONDITION.Text = dt.Rows[0]["CONDITION"].ToString().Trim();
                TextBox_OTH_THING.Text = dt.Rows[0]["OTH_THING"].ToString().Trim();
                DropDownList_MZ_PNO1.Text = dt.Rows[0]["MZ_PNO1"].ToString().Trim();
                DropDownList_MZ_EXPNO1.Text = dt.Rows[0]["MZ_EXOPNO1"].ToString().Trim();
                DropDownList_MZ_BPRID.Text = string.IsNullOrEmpty(dt.Rows[0]["MZ_BCHKAD"].ToString().Trim()) ? "" : dt.Rows[0]["MZ_BCHKAD"].ToString().Trim();
                TextBox_MZ_BPRID1.Text = dt.Rows[0]["MZ_BPRID1"].ToString().Trim();
                TextBox_MZ_BPRID2.Text = dt.Rows[0]["MZ_BPRID2"].ToString().Trim();

                TextBox_MZ_BDATE.Text = o_CommonService.Personal_ReturnDateString(dt.Rows[0]["MZ_BDATE"].ToString().Trim());
                TextBox_MZ_DATE.Text = o_CommonService.Personal_ReturnDateString(dt.Rows[0]["MZ_DATE"].ToString().Trim());
                TextBox_MZ_IDATE.Text = o_CommonService.Personal_ReturnDateString(dt.Rows[0]["MZ_IDATE"].ToString().Trim());
                TextBox_MZ_ADATE.Text = o_CommonService.Personal_ReturnDateString(dt.Rows[0]["MZ_ADATE"].ToString().Trim());

                TextBox_MZ_AD1.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)='38213' UNION ALL SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)<>'38213') WHERE MZ_KCODE='" + o_str.tosql(TextBox_MZ_AD.Text.Trim()) + "'");
                TextBox_MZ_EXOAD1.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)='38213' UNION ALL SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)<>'38213') WHERE MZ_KCODE='" + o_str.tosql(TextBox_MZ_EXOAD.Text.Trim()) + "'");
                TextBox_MZ_CHISI1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_CHISI.Text, "23");
                TextBox_MZ_EXOUNIT1.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='25' AND MZ_KCODE IN (SELECT MZ_UNIT FROM A_UNIT_AD WHERE MZ_AD='" + o_str.tosql(TextBox_MZ_EXOAD.Text.Trim()) + "') AND MZ_KCODE='" + o_str.tosql(TextBox_MZ_EXOUNIT.Text.Trim()) + "'");
                TextBox_MZ_NREA1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_NREA.Text, "11");
                TextBox_MZ_OCCC1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_OCCC.Text, "26");
                TextBox_MZ_RANK_1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_RANK.Text, "09");
                TextBox_MZ_RANK1_1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_RANK1.Text, "09");
                TextBox_MZ_SLVC1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_SLVC.Text, "64");
                TextBox_MZ_SRANK1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_SRANK.Text, "09");
                TextBox_MZ_TBDV1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_TBDV.Text, "43");
                TextBox_MZ_UNIT1.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='25' AND MZ_KCODE IN (SELECT MZ_UNIT FROM A_UNIT_AD WHERE MZ_AD='" + o_str.tosql(TextBox_MZ_AD.Text.Trim()) + "') AND MZ_KCODE='" + o_str.tosql(TextBox_MZ_UNIT.Text.Trim()) + "'");
                TextBox_MZ_EXOOCC1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_EXOOCC.Text, "26");
                TextBox_MZ_EXORANK_1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_EXORANK.Text, "09");
                TextBox_MZ_EXRANK1_1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_EXRANK1.Text, "09");


                //TextBox_MZ_TBNREA1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_TBNREA.Text, "11");
            }
            btDelete.Enabled = true;
            btUpdate.Enabled = true;
        }
        protected void btSearch_Click(object sender, EventArgs e)
        {
            Session["SEARCH"] = "true";
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('PersonalSerch2.aspx?TableName=POSIT2&TPM_FION=" + Request.QueryString["TPM_FION"] + "','查詢','top=190,left=200,width=380,height=200,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }


        protected void btInsert_Click(object sender, EventArgs e)
        {
            TextBox_MZ_NIN.Text = "1";

            TextBox_MZ_NIN1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_NIN.Text, "54");

            TextBox_MZ_NRT.Text = "1";

            TextBox_MZ_NRT1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_NRT.Text, "53");

            TextBox_MZ_PESN.Text = "03";

            TextBox_MZ_PESN1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_NRT.Text, "05");

            ViewState["Mode"] = "Insert";

            ViewState["SqlString"] = "INSERT INTO A_POSIT2(MZ_ID,MZ_PRID,MZ_PRID1,MZ_NAME,MZ_EXOAD,MZ_EXOUNIT,MZ_EXOOCC,MZ_EXORANK,MZ_EXRANK1,MZ_EXOPOS,MZ_BPRID,MZ_BPRID1"/*MZ_TBNREA,MZ_TBDATE"*/ +
                /*",MZ_TBID*/",MZ_EXOPNO,MZ_AD,MZ_UNIT,MZ_OCCC,MZ_RANK,MZ_RANK1,MZ_CHISI,MZ_POSIND,MZ_DATE,MZ_IDATE,MZ_ADATE" +
                                                     ",MZ_NIN,MZ_NREA,MZ_TBDV,MZ_PCHIEF,MZ_PESN,MZ_PNO,MZ_TNO,MZ_WDATE,MZ_SRANK,MZ_SLVC,MZ_SPT,MZ_SPT1,MZ_MEMO" +
                                                     ",MZ_RET,MZ_REMARK,MZ_SWT,MZ_EXTPOS,MZ_EXTPOS2,MUSER,MDATE,MZ_ISEXTPOS,MZ_ISEXTPOS2,OTH_THING,CONDITION,MZ_NRT,MZ_PNO1,MZ_EXOPNO1,MZ_BPRID2,MZ_BDATE,MZ_BCHKAD)" +
                                             " VALUES(@MZ_ID,@MZ_PRID,@MZ_PRID1,@MZ_NAME,@MZ_EXOAD,@MZ_EXOUNIT,@MZ_EXOOCC,@MZ_EXORANK,@MZ_EXRANK1,@MZ_EXOPOS,@MZ_BPRID,@MZ_BPRID1," +
                /*":MZ_TBNREA,@MZ_TBDATE,@MZ_TBID,*/":MZ_EXOPNO,@MZ_AD,@MZ_UNIT,@MZ_OCCC,@MZ_RANK,@MZ_RANK1,@MZ_CHISI,@MZ_POSIND,@MZ_DATE," +
                                                     ":MZ_IDATE,@MZ_ADATE,@MZ_NIN,@MZ_NREA,@MZ_TBDV,@MZ_PCHIEF,@MZ_PESN,@MZ_PNO,@MZ_TNO,@MZ_WDATE,@MZ_SRANK,@MZ_SLVC," +
                                                     ":MZ_SPT,@MZ_SPT1,@MZ_MEMO,@MZ_RET,@MZ_REMARK,@MZ_SWT,@MZ_EXTPOS,@MZ_EXTPOS2,@MUSER,@MDATE,@MZ_ISEXTPOS,@MZ_ISEXTPOS2," +
                                                     ":OTH_THING,@CONDITION,@MZ_NRT,@MZ_PNO1,@MZ_EXOPNO1,@MZ_BPRID2,@MZ_BDATE,@MZ_BCHKAD)";

            btCancel.Enabled = true;
            btOK.Enabled = true;
            btUpdate.Enabled = false;
            btDelete.Enabled = false;
            btUpper.Enabled = false;
            btNEXT.Enabled = false;
            btInsert.Enabled = false;
            A.controlEnable(ref this.Panel2, true);
            EXTPOS();
            EXTPOS2();
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_ID.ClientID + "').focus();$get('" + TextBox_MZ_ID.ClientID + "').focus();", true);
        }

        protected void btUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(o_DBFactory.ABC_toTest.vExecSQL("select MZ_ID from A_POSIT2 WHERE MZ_ID='" + o_str.tosql(TextBox_MZ_ID.Text.Trim()) + "'AND MZ_PRID='" + TextBox_MZ_PRID.Text.Trim() + "' AND MZ_PRID1='" + TextBox_MZ_PRID1.Text.Trim() + "'")))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請先查詢資料');", true);
                return;
            }

            ViewState["Mode"] = "Update";
            ViewState["SqlString"] = "UPDATE A_POSIT2 SET MZ_ID=@MZ_ID,MZ_PRID = @MZ_PRID,MZ_PRID1 = @MZ_PRID1,MZ_NAME = @MZ_NAME," +
                                                         "MZ_EXOAD = @MZ_EXOAD,MZ_EXOUNIT = @MZ_EXOUNIT,MZ_EXOOCC = @MZ_EXOOCC,MZ_EXORANK = @MZ_EXORANK,MZ_EXRANK1 = @MZ_EXRANK1,MZ_EXOPOS = @MZ_EXOPOS," +
                /*MZ_TBNREA = @MZ_TBNREA,MZ_TBDATE = @MZ_TBDATE,MZ_TBID = @MZ_TBID,*/"MZ_EXOPNO = @MZ_EXOPNO," +
                                                         "MZ_AD = @MZ_AD,MZ_UNIT = @MZ_UNIT,MZ_OCCC = @MZ_OCCC,MZ_RANK = @MZ_RANK,MZ_RANK1 = @MZ_RANK1,MZ_CHISI = @MZ_CHISI,MZ_POSIND = @MZ_POSIND,MZ_DATE = @MZ_DATE," +
                                                         "MZ_IDATE = @MZ_IDATE,MZ_ADATE = @MZ_ADATE,MZ_NIN = @MZ_NIN,MZ_NREA = @MZ_NREA,MZ_TBDV = @MZ_TBDV,MZ_PCHIEF = @MZ_PCHIEF,MZ_PESN = @MZ_PESN,MZ_PNO = @MZ_PNO," +
                                                         "MZ_TNO = @MZ_TNO,MZ_WDATE = @MZ_WDATE,MZ_SRANK = @MZ_SRANK,MZ_SLVC = @MZ_SLVC,MZ_SPT = @MZ_SPT,MZ_SPT1 = @MZ_SPT1,MZ_MEMO = @MZ_MEMO,MZ_RET = @MZ_RET,MZ_REMARK = @MZ_REMARK," +
                                                         "MZ_SWT = @MZ_SWT,MZ_EXTPOS = @MZ_EXTPOS,MZ_EXTPOS2 = @MZ_EXTPOS2,MUSER = @MUSER,MDATE = @MDATE,MZ_ISEXTPOS = @MZ_ISEXTPOS,MZ_ISEXTPOS2 = @MZ_ISEXTPOS2," +
                                                         "OTH_THING = @OTH_THING,CONDITION = @CONDITION,MZ_NRT = @MZ_NRT,MZ_PNO1 = @MZ_PNO1,MZ_EXOPNO1 = @MZ_EXOPNO1,MZ_BPRID = @MZ_BPRID,MZ_BPRID1 = @MZ_BPRID1,MZ_BPRID2=@MZ_BPRID2,MZ_BDATE=@MZ_BDATE,MZ_BCHKAD=@MZ_BCHKAD " +
                                                         " WHERE MZ_ID='" + o_str.tosql(TextBox_MZ_ID.Text.Trim()) + "' AND MZ_PRID='" + o_str.tosql(TextBox_MZ_PRID.Text.Trim()) + "' AND MZ_PRID1='" + o_str.tosql(TextBox_MZ_PRID1.Text.Trim()) + "'";

            HttpCookie Cookie1 = new HttpCookie("PKEY_MZ_PRID");
            Cookie1.Value = TextBox_MZ_PRID.Text;
            Response.Cookies.Add(Cookie1);

            HttpCookie Cookie2 = new HttpCookie("PKEY_MZ_PRID1");
            Cookie2.Value = TextBox_MZ_PRID1.Text;
            Response.Cookies.Add(Cookie2);

            HttpCookie Cookie3 = new HttpCookie("PKEY_MZ_ID");
            Cookie1.Value = TextBox_MZ_PRID.Text;
            Response.Cookies.Add(Cookie3);

            btInsert.Enabled = false;
            btCancel.Enabled = true;
            btNEXT.Enabled = false;
            btUpper.Enabled = false;
            btOK.Enabled = true;
            btDelete.Enabled = true;
            btUpdate.Enabled = false;
            A.controlEnable(ref this.Panel2, true);
            EXTPOS();
            EXTPOS2();
            Button1.Enabled = false;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_AD.ClientID + "').focus();$get('" + TextBox_MZ_AD.ClientID + "').focus();", true);
        }

        protected void btDelete_Click(object sender, EventArgs e)
        {

            string DeleteString = "DELETE FROM A_POSIT2 WHERE MZ_ID='" + o_str.tosql(TextBox_MZ_ID.Text.Trim()) + "' AND MZ_PRID='" + o_str.tosql(TextBox_MZ_PRID.Text.Trim()) + "' AND MZ_PRID1='" + o_str.tosql(TextBox_MZ_PRID1.Text.Trim()) + "'";
            try
            {
                o_DBFactory.ABC_toTest.Edit_Data(DeleteString);

                POSIT2_MZ_ID = Session["POSIT2_MZ_ID"] as List<string>;

                POSIT2_MZ_ID.RemoveAt(int.Parse(xcount.Text.Trim()));


                if (POSIT2_MZ_ID.Count == 0)
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除成功');location.href('Personal3-3.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "');", true);
                    //2010.06.04 LOG紀錄 by伊珊
                    TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00004", TPMPermissions._boolTPM(), DeleteString);

                    btUpdate.Enabled = false;
                }
                else
                {
                    xcount.Text = "0";
                    finddata(int.Parse(xcount.Text));
                    if (POSIT2_MZ_ID.Count > 1)
                    {
                        btNEXT.Enabled = true;
                    }
                    btUpdate.Enabled = true;
                    Label1.Text = "第" + (int.Parse(xcount.Text) + 1).ToString() + "筆共" + POSIT2_MZ_ID.Count.ToString() + "筆";
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除成功')", true);
                    //2010.06.04 LOG紀錄 by伊珊
                    TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00004", TPMPermissions._boolTPM(), DeleteString);
                }


                btDelete.Enabled = false;
                btOK.Enabled = true;
                btCancel.Enabled = false;
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
                if (int.Parse(xcount.Text) != POSIT2_MZ_ID.Count - 1)
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
            Label1.Text = "第" + (int.Parse(xcount.Text) + 1).ToString() + "筆共" + POSIT2_MZ_ID.Count.ToString() + "筆";
        }

        protected void btNEXT_Click(object sender, EventArgs e)
        {
            if (int.Parse(xcount.Text) == 0)
            {
                xcount.Text = (Convert.ToInt32(xcount.Text) + 1).ToString();
                btUpper.Enabled = true;
                finddata(int.Parse(xcount.Text));

                if (int.Parse(xcount.Text) == POSIT2_MZ_ID.Count - 1)
                {
                    btNEXT.Enabled = false;
                }
            }
            else
            {
                xcount.Text = (Convert.ToInt32(xcount.Text) + 1).ToString();
                finddata(int.Parse(xcount.Text));

                if (int.Parse(xcount.Text) == POSIT2_MZ_ID.Count - 1)
                {
                    btNEXT.Enabled = false;
                }
            }
            Label1.Text = "第" + (int.Parse(xcount.Text) + 1).ToString() + "筆共" + POSIT2_MZ_ID.Count.ToString() + "筆";
        }

        protected void btok_Click(object sender, EventArgs e)
        {
            //2010.06.07 by 伊珊
            string ErrorString = "";

            string old_PRID = "NULL";

            string old_PRID1 = "NULL";

            string old_ID = "NULL";

            if (ViewState["Mode"].ToString() == "Update")
            {
                HttpCookie Cookie4 = new HttpCookie("PKEY_MZ_PRID");
                Cookie4 = Request.Cookies["PKEY_MZ_PRID"];
                old_PRID = Cookie4.Value.ToString();

                HttpCookie Cookie2 = new HttpCookie("PKEY_MZ_PRID1");
                Cookie2 = Request.Cookies["PKEY_MZ_PRID1"];
                old_PRID1 = Cookie2.Value.ToString();

                HttpCookie Cookie3 = new HttpCookie("PKEY_MZ_ID");
                Cookie3 = Request.Cookies["PKEY_MZ_ID"];
                old_ID = Cookie3.Value.ToString();
            }

            string pkey_check;

            if (old_ID == TextBox_MZ_ID.Text && old_PRID == TextBox_MZ_PRID.Text && old_PRID1 == TextBox_MZ_PRID1.Text && ViewState["Mode"].ToString() == "Update")
                pkey_check = "0";
            else
                pkey_check = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_POSIT2 WHERE MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "' AND MZ_PRID='" + TextBox_MZ_PRID.Text.Trim() + "' AND MZ_PRID1='" + TextBox_MZ_PRID1.Text.Trim() + "'");

            if (pkey_check != "0")
            {
                ErrorString += "身分證號與發文文號與發文字號違反唯一值的條件" + "\\r\\n";
                TextBox_MZ_PRID.BackColor = Color.Orange;
                TextBox_MZ_PRID1.BackColor = Color.Orange;
                TextBox_MZ_ID.BackColor = Color.Orange;
            }
            else
            {
                TextBox_MZ_PRID.BackColor = Color.White;
                TextBox_MZ_PRID1.BackColor = Color.White;
                TextBox_MZ_ID.BackColor = Color.White;
            }

            if (RadioButtonList_MZ_ISEXTPOS.SelectedValue == "Y")
                if (string.IsNullOrEmpty(DropDownList_MZ_EXTPOS.SelectedValue))
                {
                    ErrorString += "原任兼代職名稱不可空白" + "\\r\\n";
                    DropDownList_MZ_EXTPOS.BackColor = Color.Orange;
                }
                else
                    DropDownList_MZ_EXTPOS.BackColor = Color.White;

            if (RadioButtonList_MZ_ISEXTPOS2.SelectedValue == "Y")
                if (string.IsNullOrEmpty(DropDownList_MZ_EXTPOS2.SelectedValue))
                {
                    ErrorString += "兼代職名稱不可空白" + "\\r\\n";
                    DropDownList_MZ_EXTPOS2.BackColor = Color.Orange;
                }
                else
                    DropDownList_MZ_EXTPOS2.BackColor = Color.White;


            //20141216 這應該是想做控制項檢核.但 DBDIC 又沒有資料
            //foreach (Object ob in Panel2.Controls)
            //{
            //    if (ob is TextBox)
            //    {
            //        TextBox tbox = (TextBox)ob;

            //        string result = o_Check_Dbdic.ErrorString(tbox.ID, "A_POSIT2", tbox.Text);

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

            //        string result = o_Check_Dbdic.ErrorString(dlist.ID, "A_POSIT2", dlist.Text);

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

            using (SqlConnection Updateconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Updateconn.Open();
                SqlCommand cmd = new SqlCommand(ViewState["SqlString"].ToString(), Updateconn);
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.Add("MZ_ID", SqlDbType.VarChar).Value = TextBox_MZ_ID.Text.Trim();
                cmd.Parameters.Add("MZ_PRID", SqlDbType.VarChar).Value = TextBox_MZ_PRID.Text.Trim();
                cmd.Parameters.Add("MZ_NAME", SqlDbType.VarChar).Value = TextBox_MZ_NAME.Text.Trim();
                cmd.Parameters.Add("MZ_PRID1", SqlDbType.VarChar).Value = TextBox_MZ_PRID1.Text.Trim();
                cmd.Parameters.Add("MZ_PRID", SqlDbType.VarChar).Value = TextBox_MZ_PRID.Text.Trim();
                cmd.Parameters.Add("MZ_DATE", SqlDbType.VarChar).Value = TextBox_MZ_DATE.Text.Trim();
                cmd.Parameters.Add("MZ_IDATE", SqlDbType.VarChar).Value = TextBox_MZ_IDATE.Text.Trim();
                cmd.Parameters.Add("MZ_ADATE", SqlDbType.VarChar).Value = TextBox_MZ_ADATE.Text.Trim();
                cmd.Parameters.Add("MZ_EXOAD", SqlDbType.VarChar).Value = TextBox_MZ_EXOAD.Text.Trim();
                cmd.Parameters.Add("MZ_EXOUNIT", SqlDbType.VarChar).Value = TextBox_MZ_EXOUNIT.Text.Trim();
                cmd.Parameters.Add("MZ_EXORANK", SqlDbType.VarChar).Value = TextBox_MZ_EXORANK.Text.Trim();
                cmd.Parameters.Add("MZ_EXRANK1", SqlDbType.VarChar).Value = TextBox_MZ_EXRANK1.Text.Trim();
                cmd.Parameters.Add("MZ_EXOOCC", SqlDbType.VarChar).Value = TextBox_MZ_EXOOCC.Text.Trim();
                cmd.Parameters.Add("MZ_EXOPOS", SqlDbType.VarChar).Value = TextBox_MZ_EXOPOS.Text.Trim();
                cmd.Parameters.Add("MZ_EXOPNO", SqlDbType.VarChar).Value = TextBox_MZ_EXOPNO.Text.Trim();
                //cmd.Parameters.Add("MZ_TBDATE", SqlDbType.VarChar).Value = TextBox_MZ_TBDATE.Text.Trim();
                //cmd.Parameters.Add("MZ_TBNREA", SqlDbType.VarChar).Value = TextBox_MZ_TBNREA.Text.Trim();
                //cmd.Parameters.Add("MZ_TBID", SqlDbType.VarChar).Value = TextBox_MZ_TBID.Text.Trim();
                cmd.Parameters.Add("MZ_AD", SqlDbType.VarChar).Value = TextBox_MZ_AD.Text.Trim();
                cmd.Parameters.Add("MZ_UNIT", SqlDbType.VarChar).Value = TextBox_MZ_UNIT.Text.Trim();
                cmd.Parameters.Add("MZ_RANK", SqlDbType.VarChar).Value = TextBox_MZ_RANK.Text.Trim();
                cmd.Parameters.Add("MZ_RANK1", SqlDbType.VarChar).Value = TextBox_MZ_RANK1.Text.Trim();
                cmd.Parameters.Add("MZ_OCCC", SqlDbType.VarChar).Value = TextBox_MZ_OCCC.Text.Trim();
                cmd.Parameters.Add("MZ_CHISI", SqlDbType.VarChar).Value = TextBox_MZ_CHISI.Text.Trim();
                cmd.Parameters.Add("MZ_TBDV", SqlDbType.VarChar).Value = TextBox_MZ_TBDV.Text.Trim();
                cmd.Parameters.Add("MZ_POSIND", SqlDbType.VarChar).Value = TextBox_MZ_POSIND.Text.Trim();
                cmd.Parameters.Add("MZ_NIN", SqlDbType.VarChar).Value = TextBox_MZ_NIN.Text.Trim();
                cmd.Parameters.Add("MZ_PESN", SqlDbType.VarChar).Value = TextBox_MZ_PESN.Text.Trim();
                cmd.Parameters.Add("MZ_PCHIEF", SqlDbType.VarChar).Value = TextBox_MZ_PCHIEF.Text.Trim();
                cmd.Parameters.Add("MZ_PNO", SqlDbType.VarChar).Value = TextBox_MZ_PNO.Text.Trim();
                cmd.Parameters.Add("MZ_TNO", SqlDbType.VarChar).Value = TextBox_MZ_TNO.Text.Trim();
                cmd.Parameters.Add("MZ_NREA", SqlDbType.VarChar).Value = TextBox_MZ_NREA.Text.Trim();
                cmd.Parameters.Add("MZ_SRANK", SqlDbType.VarChar).Value = TextBox_MZ_SRANK.Text.Trim();
                cmd.Parameters.Add("MZ_SLVC", SqlDbType.VarChar).Value = TextBox_MZ_SLVC.Text.Trim();
                cmd.Parameters.Add("MZ_SPT", SqlDbType.VarChar).Value = TextBox_MZ_SPT.Text.Trim();
                cmd.Parameters.Add("MZ_SPT1", SqlDbType.VarChar).Value = TextBox_MZ_SPT1.Text.Trim();
                cmd.Parameters.Add("MZ_REMARK", SqlDbType.VarChar).Value = TextBox_MZ_REMARK.Text.Trim();
                cmd.Parameters.Add("MZ_RET", SqlDbType.VarChar).Value = TextBox_MZ_RET.Text.Trim();
                cmd.Parameters.Add("MZ_SWT", SqlDbType.VarChar).Value = TextBox_MZ_SWT.Text.Trim();
                cmd.Parameters.Add("MZ_NRT", SqlDbType.VarChar).Value = TextBox_MZ_NRT.Text.Trim();
                cmd.Parameters.Add("OTH_THING", SqlDbType.VarChar).Value = TextBox_OTH_THING.Text.Trim();
                cmd.Parameters.Add("CONDITION", SqlDbType.VarChar).Value = TextBox_CONDITION.Text.Trim();
                cmd.Parameters.Add("MZ_EXOPNO1", SqlDbType.VarChar).Value = DropDownList_MZ_EXPNO1.Text.Trim();
                cmd.Parameters.Add("MZ_EXTPOS", SqlDbType.VarChar).Value = DropDownList_MZ_EXTPOS.SelectedValue.Trim();
                cmd.Parameters.Add("MZ_PNO1", SqlDbType.VarChar).Value = DropDownList_MZ_PNO1.Text.Trim();
                cmd.Parameters.Add("MZ_EXTPOS2", SqlDbType.VarChar).Value = DropDownList_MZ_EXTPOS2.SelectedValue.Trim();
                cmd.Parameters.Add("MZ_ISEXTPOS", SqlDbType.VarChar).Value = RadioButtonList_MZ_ISEXTPOS.SelectedValue.Trim();
                cmd.Parameters.Add("MZ_ISEXTPOS2", SqlDbType.VarChar).Value = RadioButtonList_MZ_ISEXTPOS2.SelectedValue.Trim();
                cmd.Parameters.Add("MZ_MEMO", SqlDbType.VarChar).Value = TextBox_MZ_MEMO.Text.Trim();
                cmd.Parameters.Add("MZ_IDATE", SqlDbType.VarChar).Value = string.IsNullOrEmpty(TextBox_MZ_IDATE.Text.Trim()) ? Convert.DBNull : TextBox_MZ_IDATE.Text.Trim().Replace("/", "").PadLeft(7, '0');
                cmd.Parameters.Add("MZ_DATE", SqlDbType.VarChar).Value = string.IsNullOrEmpty(TextBox_MZ_DATE.Text.Trim()) ? Convert.DBNull : TextBox_MZ_DATE.Text.Trim().Replace("/", "").PadLeft(7, '0');
                cmd.Parameters.Add("MZ_ADATE", SqlDbType.VarChar).Value = string.IsNullOrEmpty(TextBox_MZ_ADATE.Text.Trim()) ? Convert.DBNull : TextBox_MZ_ADATE.Text.Trim().Replace("/", "").PadLeft(7, '0');
                //cmd.Parameters.Add("MZ_TBDATE", SqlDbType.VarChar).Value = string.IsNullOrEmpty(TextBox_MZ_TBDATE.Text.Trim()) ? Convert.DBNull : TextBox_MZ_TBDATE.Text.Replace("/", "").Trim().PadLeft(7, '0');
                cmd.Parameters.Add("MZ_WDATE", SqlDbType.VarChar).Value = string.IsNullOrEmpty(TextBox_MZ_WDATE.Text.Trim()) ? Convert.DBNull : TextBox_MZ_WDATE.Text.Replace("/", "").Trim().PadLeft(7, '0');
                cmd.Parameters.Add("MZ_BPRID", SqlDbType.VarChar).Value = DropDownList_MZ_BPRID.SelectedItem.Text;
                cmd.Parameters.Add("MZ_BCHKAD", SqlDbType.VarChar).Value = DropDownList_MZ_BPRID.SelectedValue;
                cmd.Parameters.Add("MZ_BPRID1", SqlDbType.VarChar).Value = TextBox_MZ_BPRID1.Text.Trim();
                cmd.Parameters.Add("MZ_BPRID2", SqlDbType.VarChar).Value = TextBox_MZ_BPRID2.Text.Trim();
                cmd.Parameters.Add("MUSER", SqlDbType.VarChar).Value = Session["ADPMZ_ID"].ToString();
                cmd.Parameters.Add("MDATE", SqlDbType.VarChar).Value = (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0') + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0');
                cmd.Parameters.Add("MZ_BDATE", SqlDbType.VarChar).Value = string.IsNullOrEmpty(TextBox_MZ_BDATE.Text.Trim()) ? Convert.DBNull : TextBox_MZ_BDATE.Text.Trim().Replace("/", "").PadLeft(7, '0');

                try
                {
                    cmd.ExecuteNonQuery();

                    Response.Cookies["PKEY_MZ_PRID"].Expires = DateTime.Now.AddYears(-1);
                    Response.Cookies["PKEY_MZ_PRID1"].Expires = DateTime.Now.AddYears(-1);
                    Response.Cookies["PKEY_MZ_ID"].Expires = DateTime.Now.AddYears(-1);

                    if (ViewState["Mode"].ToString() == "Insert")
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('新增成功');", true);
                        //2010.06.04 LOG紀錄 by伊珊
                        TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00002", TPMPermissions._boolTPM(), o_DBFactory.ABC_toTest.RegexSQL(cmd));
                    }
                    else if (ViewState["Mode"].ToString() == "Update")
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('修改成功');", true);

                        //2010.06.04 LOG紀錄 by伊珊
                        TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00003", TPMPermissions._boolTPM(), o_DBFactory.ABC_toTest.RegexSQL(cmd));

                        POSIT2_MZ_ID = Session["POSIT2_MZ_ID"] as List<string>;

                        if (int.Parse(xcount.Text.Trim()) == 0 && POSIT2_MZ_ID.Count == 1)
                        {
                            btUpper.Enabled = false;
                            btNEXT.Enabled = false;
                        }
                        else if (int.Parse(xcount.Text.Trim()) == 0 && POSIT2_MZ_ID.Count > 1)
                        {
                            btUpper.Enabled = false;
                            btNEXT.Enabled = true;
                        }
                        else if (int.Parse(xcount.Text.Trim()) + 1 == POSIT2_MZ_ID.Count)
                        {
                            btUpper.Enabled = true;
                            btNEXT.Enabled = false;
                        }
                        else if (int.Parse(xcount.Text.Trim()) > 0 && int.Parse(xcount.Text.Trim()) + 1 < POSIT2_MZ_ID.Count)
                        {
                            btNEXT.Enabled = true;
                            btUpper.Enabled = true;
                        }

                    }
                    btCancel.Enabled = false;
                    btOK.Enabled = false;
                    btDelete.Enabled = false;
                    btInsert.Enabled = true;
                    btUpdate.Enabled = true;
                    ViewState.Remove("Mode");
                    A.controlEnable(ref this.Panel2, false);
                }
                catch
                {
                    if (ViewState["Mode"].ToString() == "Insert")
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('新增失敗');", true);
                    }
                    else if (ViewState["Mode"].ToString() == "Update")
                    {
                        HttpCookie Cookie1 = new HttpCookie("POSIT2_PRID");
                        Cookie1.Value = TPMPermissions._strEncood(TextBox_MZ_PRID.Text.Trim());
                        Response.Cookies.Add(Cookie1);
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('修改失敗');location.href('Personal3-3.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "&XCOUNT=" + xcount.Text.Trim() + "');", true);
                    }
                }
                finally
                {
                    Updateconn.Close();

                    //XX2013/06/18 
                    Updateconn.Dispose();
                }
            }


        }

        protected void btCancel_Click(object sender, EventArgs e)
        {

            if (ViewState["Mode"].ToString() == "Insert")
            {
                foreach (object dl in Panel2.Controls)
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
                btDelete.Enabled = false;
            }
            else if (ViewState["Mode"].ToString() == "Update")
            {
                finddata(int.Parse(xcount.Text.Trim()));

                POSIT2_MZ_ID = Session["POSIT2_MZ_ID"] as List<string>;
                if (int.Parse(xcount.Text.Trim()) == 0 && POSIT2_MZ_ID.Count == 1)
                {
                    btUpper.Enabled = false;
                    btNEXT.Enabled = false;
                }
                else if (int.Parse(xcount.Text.Trim()) == 0 && POSIT2_MZ_ID.Count > 1)
                {
                    btUpper.Enabled = false;
                    btNEXT.Enabled = true;
                }
                else if (int.Parse(xcount.Text.Trim()) + 1 == POSIT2_MZ_ID.Count)
                {
                    btUpper.Enabled = true;
                    btNEXT.Enabled = false;
                }
                else if (int.Parse(xcount.Text.Trim()) > 0 && int.Parse(xcount.Text.Trim()) + 1 < POSIT2_MZ_ID.Count)
                {
                    btNEXT.Enabled = true;
                    btUpper.Enabled = true;
                }

                btUpdate.Enabled = true;
                btDelete.Enabled = true;
            }
            btInsert.Enabled = true;
            btCancel.Enabled = false;
            btOK.Enabled = false;

            A.controlEnable(ref this.Panel2, false);

        }

        protected void Ktype_Search(TextBox tb1, TextBox tb2, string ktype)
        {
            //回傳值
            Session["KTYPE_CID"] = tb1.ClientID;
            Session["KTYPE_CID1"] = tb2.ClientID;
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_Ktype_Search.aspx?MZ_KTYPE=" + ktype + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin','top=190,left=200,width=420,height=450,toolbar=0,menubar=0,location=0,directories=0,status=0,scrollbar=yes');", true);
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

        protected void returnSameDataType(TextBox tb, TextBox tb1)
        {
            tb.Text = o_str.tosql(tb.Text.Trim().Replace("/", ""));

            if (tb.Text != "")
            {
                if (!DateManange.Check_date(tb.Text))
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('日期格式錯誤')", true);
                    tb.Text = string.Empty;
                    // tb.Focus();
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + tb.ClientID + "').focus();$get('" + tb.ClientID + "').focus();", true);
                }
                else
                {
                    tb.Text = o_CommonService.Personal_ReturnDateString(tb.Text.Trim());
                    //tb1.Focus();
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + tb1.ClientID + "').focus();$get('" + tb1.ClientID + "').focus();", true);
                }
            }
        }

        protected void btEXOAD_Click(object sender, EventArgs e)
        {
            Session["KTYPE_CID"] = TextBox_MZ_EXOAD.ClientID;
            Session["KTYPE_CID1"] = TextBox_MZ_EXOAD1.ClientID;
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_Ktype_Search.aspx?CID_NAME=AD&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin');", true);
        }

        protected void btUNIT_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_MZ_UNIT, TextBox_MZ_UNIT1, "25");
            //if (string.IsNullOrEmpty(o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCODE FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,6)='38213' UNION ALL SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,6)<>'38213') WHERE MZ_KCODE='" + TextBox_MZ_AD.Text.Trim() + "'")))
            //{
            //    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請先輸入新任服務機關')", true);
            //}
            //Session["KTYPE_CID"] = TextBox_MZ_UNIT.ClientID;
            //Session["KTYPE_CID1"] = TextBox_MZ_UNIT1.ClientID;
            //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_Ktype_Search.aspx?CID_NAME=UNIT&AD=" + TextBox_MZ_AD.Text.Trim() + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin');", true);
        }

        protected void btEXOUNIT_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_MZ_EXOUNIT, TextBox_MZ_EXOUNIT1, "25");
            //if (string.IsNullOrEmpty(o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCODE FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,6)='38213' UNION ALL SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,6)<>'38213') WHERE MZ_KCODE='" + TextBox_MZ_EXOAD.Text.Trim() + "'")))
            //{
            //    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請先輸入服務機關')", true);
            //}
            //Session["KTYPE_CID"] = TextBox_MZ_EXOUNIT.ClientID;
            //Session["KTYPE_CID1"] = TextBox_MZ_EXOUNIT1.ClientID;
            //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_Ktype_Search.aspx?CID_NAME=UNIT&AD=" + TextBox_MZ_EXOAD.Text.Trim() + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin');", true);
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

        protected void btSLVC_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_MZ_SLVC, TextBox_MZ_SLVC1, "64");
        }

        protected void btCHISI_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_MZ_CHISI, TextBox_MZ_CHISI1, "23");
        }

        protected void btTBDV_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_MZ_TBDV, TextBox_MZ_TBDV1, "43");
        }

        protected void btNREA_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_MZ_NREA, TextBox_MZ_NREA1, "11");
        }

        protected void btSRANK_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_MZ_SRANK, TextBox_MZ_SRANK1, "09");
        }

        protected void TextBox_MZ_AD_TextChanged(object sender, EventArgs e)
        {
            TextBox_MZ_AD.Text = TextBox_MZ_AD.Text.ToUpper();
            string CName = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,6)='38213' UNION ALL SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)<>'38213') WHERE MZ_KCODE='" + o_str.tosql(TextBox_MZ_AD.Text.Trim()) + "'");
            Ktype_Cname_Check(CName, TextBox_MZ_AD1, TextBox_MZ_AD, TextBox_MZ_UNIT);
        }

        protected void TextBox_MZ_UNIT_TextChanged(object sender, EventArgs e)
        {
            TextBox_MZ_UNIT.Text = TextBox_MZ_UNIT.Text.ToUpper();
            // string CName = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='25' AND MZ_KCODE IN (SELECT MZ_UNIT FROM A_UNIT_AD WHERE MZ_AD='" + o_str.tosql(TextBox_MZ_AD.Text.Trim()) + "') AND MZ_KCODE='" + o_str.tosql(TextBox_MZ_UNIT.Text.Trim()) + "'");
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_UNIT.Text, "25");
            Ktype_Cname_Check(CName, TextBox_MZ_UNIT1, TextBox_MZ_UNIT, TextBox_MZ_RANK);
        }

        protected void TextBox_MZ_EXOAD_TextChanged(object sender, EventArgs e)
        {
            TextBox_MZ_EXOAD.Text = TextBox_MZ_EXOAD.Text.ToUpper();

            string CName = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)='38213' UNION ALL SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)<>'38213') WHERE MZ_KCODE='" + o_str.tosql(TextBox_MZ_EXOAD.Text.Trim()) + "'");

            Ktype_Cname_Check(CName, TextBox_MZ_EXOAD1, TextBox_MZ_EXOAD, TextBox_MZ_EXOUNIT);
        }

        protected void TextBox_MZ_EXOUNIT_TextChanged(object sender, EventArgs e)
        {

            TextBox_MZ_EXOUNIT.Text = TextBox_MZ_EXOUNIT.Text.ToUpper();

            string CName = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='25' AND MZ_KCODE IN (SELECT MZ_UNIT FROM A_UNIT_AD WHERE MZ_AD='" + TextBox_MZ_EXOAD.Text.Trim() + "') AND MZ_KCODE='" + TextBox_MZ_EXOUNIT.Text.Trim() + "'");

            Ktype_Cname_Check(CName, TextBox_MZ_EXOUNIT1, TextBox_MZ_EXOUNIT, TextBox_MZ_EXORANK);
        }

        protected void TextBox_MZ_OCCC_TextChanged(object sender, EventArgs e)
        {
            TextBox_MZ_OCCC.Text = TextBox_MZ_OCCC.Text.ToUpper();

            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_OCCC.Text, "26");

            Ktype_Cname_Check(CName, TextBox_MZ_OCCC1, TextBox_MZ_OCCC, RadioButtonList_MZ_ISEXTPOS2);
        }

        protected void TextBox_MZ_RANK_TextChanged(object sender, EventArgs e)
        {
            TextBox_MZ_RANK.Text = TextBox_MZ_RANK.Text.ToUpper();

            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_RANK.Text, "09");

            Ktype_Cname_Check(CName, TextBox_MZ_RANK_1, TextBox_MZ_RANK, TextBox_MZ_RANK1);
        }

        protected void TextBox_MZ_RANK1_TextChanged(object sender, EventArgs e)
        {

            TextBox_MZ_RANK1.Text = TextBox_MZ_RANK1.Text.ToUpper();

            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_RANK1.Text, "09");

            Ktype_Cname_Check(CName, TextBox_MZ_RANK1_1, TextBox_MZ_RANK1, TextBox_MZ_CHISI);
        }

        protected void TextBox_MZ_SRANK_TextChanged(object sender, EventArgs e)
        {
            TextBox_MZ_SRANK.Text = TextBox_MZ_SRANK.Text.ToUpper();

            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_SRANK.Text, "09");

            Ktype_Cname_Check(CName, TextBox_MZ_SRANK1, TextBox_MZ_SRANK, TextBox_MZ_SLVC);
        }

        protected void TextBox_MZ_SLVC_TextChanged(object sender, EventArgs e)
        {
            TextBox_MZ_SLVC.Text = TextBox_MZ_SLVC.Text.ToUpper();

            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_SLVC.Text, "64");

            Ktype_Cname_Check(CName, TextBox_MZ_SLVC1, TextBox_MZ_SLVC, TextBox_MZ_SPT);
        }

        protected void TextBox_MZ_CHISI_TextChanged(object sender, EventArgs e)
        {
            TextBox_MZ_CHISI.Text = TextBox_MZ_CHISI.Text.ToUpper();

            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_CHISI.Text, "23");

            Ktype_Cname_Check(CName, TextBox_MZ_CHISI1, TextBox_MZ_CHISI, TextBox_MZ_TBDV);
        }

        protected void TextBox_MZ_TBDV_TextChanged(object sender, EventArgs e)
        {
            TextBox_MZ_TBDV.Text = TextBox_MZ_TBDV.Text.ToUpper();

            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_TBDV.Text, "43");

            Ktype_Cname_Check(CName, TextBox_MZ_TBDV1, TextBox_MZ_TBDV, TextBox_MZ_OCCC);
        }

        protected void TextBox_MZ_NREA_TextChanged(object sender, EventArgs e)
        {
            TextBox_MZ_NREA.Text = TextBox_MZ_NREA.Text.ToUpper();

            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_NREA.Text, "11");

            Ktype_Cname_Check(CName, TextBox_MZ_NREA1, TextBox_MZ_NREA, TextBox_MZ_NRT);
        }

        protected void btEXORANK_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_MZ_EXORANK, TextBox_MZ_EXORANK_1, "09");
        }

        protected void btEXRANK1_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_MZ_EXRANK1, TextBox_MZ_EXRANK1_1, "09");
        }

        protected void btEXOOCC_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_MZ_EXOOCC, TextBox_MZ_EXOOCC1, "26");
        }

        //protected void btTBNREA_Click(object sender, EventArgs e)
        //{
        //    Ktype_Search(TextBox_MZ_TBNREA, TextBox_MZ_TBNREA1, "11");
        //}

        protected void btOTH_THING_Click(object sender, EventArgs e)
        {
            Session["KTYPE_CID"] = TextBox_OTH_THING.ClientID;

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_Ktype_Search.aspx?MZ_KTYPE=NOTE&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin');", true);
        }

        protected void btCONDITION_Click(object sender, EventArgs e)
        {
            Session["KTYPE_CID"] = TextBox_CONDITION.ClientID;

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_Ktype_Search.aspx?MZ_KTYPE=NOTE&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin');", true);
        }

        protected void TextBox_MZ_EXORANK_TextChanged(object sender, EventArgs e)
        {
            TextBox_MZ_EXORANK.Text = TextBox_MZ_EXORANK.Text.ToUpper();

            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_EXORANK.Text, "09");

            Ktype_Cname_Check(CName, TextBox_MZ_EXORANK_1, TextBox_MZ_EXORANK, TextBox_MZ_EXRANK1);
        }

        protected void TextBox_MZ_EXRANK1_TextChanged(object sender, EventArgs e)
        {
            TextBox_MZ_EXRANK1.Text = TextBox_MZ_EXRANK1.Text.ToUpper();

            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_EXRANK1.Text, "09");

            Ktype_Cname_Check(CName, TextBox_MZ_EXRANK1_1, TextBox_MZ_EXRANK1, TextBox_MZ_EXOPOS);
        }

        //protected void TextBox_MZ_TBNREA_TextChanged(object sender, EventArgs e)
        //{
        //    TextBox_MZ_TBNREA.Text = TextBox_MZ_TBNREA.Text.ToUpper();

        //    string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_TBNREA.Text, "11");

        //    Ktype_Cname_Check(CName, TextBox_MZ_TBNREA1, TextBox_MZ_TBNREA, TextBox_MZ_TBID);
        //}

        protected void TextBox_MZ_EXOOCC_TextChanged(object sender, EventArgs e)
        {
            TextBox_MZ_EXOOCC.Text = TextBox_MZ_EXOOCC.Text.ToUpper();

            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_EXOOCC.Text, "26");

            Ktype_Cname_Check(CName, TextBox_MZ_EXOOCC1, TextBox_MZ_EXOOCC, RadioButtonList_MZ_ISEXTPOS);
        }

        protected void btAD_Click(object sender, EventArgs e)
        {
            Session["KTYPE_CID"] = TextBox_MZ_AD.ClientID;
            Session["KTYPE_CID1"] = TextBox_MZ_AD1.ClientID;

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_Ktype_Search.aspx?CID_NAME=AD&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin','top=190,left=200,width=420,height=450,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }

        protected void btPESN_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_MZ_PESN, TextBox_MZ_PESN1, "05");
        }

        protected void btNRT_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_MZ_NRT, TextBox_MZ_NRT1, "53");
        }

        protected void btNIN_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_MZ_NIN, TextBox_MZ_NIN1, "54");
        }

        protected void TextBox_MZ_PESN_TextChanged(object sender, EventArgs e)
        {
            TextBox_MZ_PESN.Text = TextBox_MZ_PESN.Text.ToUpper();

            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_PESN.Text, "05");

            Ktype_Cname_Check(CName, TextBox_MZ_PESN1, TextBox_MZ_PESN, TextBox_MZ_SRANK);
        }

        protected void TextBox_MZ_NRT_TextChanged(object sender, EventArgs e)
        {
            TextBox_MZ_NRT.Text = TextBox_MZ_NRT.Text.ToUpper();

            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_NRT.Text, "53");

            Ktype_Cname_Check(CName, TextBox_MZ_NRT1, TextBox_MZ_NRT, TextBox_MZ_POSIND);
        }

        protected void TextBox_MZ_NIN_TextChanged(object sender, EventArgs e)
        {
            TextBox_MZ_NIN.Text = TextBox_MZ_NIN.Text.ToUpper();

            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_NIN.Text, "54");

            Ktype_Cname_Check(CName, TextBox_MZ_NIN1, TextBox_MZ_NIN, TextBox_MZ_PCHIEF);
        }

        protected void btPCHIEF_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_MZ_PCHIEF, TextBox_MZ_PCHIEF1, "53");
        }

        protected void TextBox_MZ_PCHIEF_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_PCHIEF.Text, "54");

            Ktype_Cname_Check(CName, TextBox_MZ_PCHIEF1, TextBox_MZ_PCHIEF, TextBox_MZ_NIN);
        }

        protected void CV_NREA_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (string.IsNullOrEmpty(args.Value))
            {
                args.IsValid = false;
                TextBox_MZ_NREA.BackColor = Color.Orange;
            }
            else
            {
                args.IsValid = true;
                TextBox_MZ_NREA.BackColor = Color.White;
            }
        }

        protected void Button_MZ_SWT4_Click(object sender, EventArgs e)
        {

        }

        private void EXTPOS()
        {
            if (RadioButtonList_MZ_ISEXTPOS.SelectedValue == "Y")
            {
                DropDownList_MZ_EXTPOS.Enabled = true;
            }
            else
            {
                DropDownList_MZ_EXTPOS.Enabled = false;
            }
        }

        private void EXTPOS2()
        {
            if (RadioButtonList_MZ_ISEXTPOS2.SelectedValue == "Y")
            {
                DropDownList_MZ_EXTPOS2.Enabled = true;
            }
            else
            {
                DropDownList_MZ_EXTPOS2.Enabled = false;
            }
        }

        protected void RadioButtonList_MZ_ISEXTPOS_SelectedIndexChanged(object sender, EventArgs e)
        {
            EXTPOS();
        }

        protected void RadioButtonList_MZ_ISEXTPOS2_SelectedIndexChanged(object sender, EventArgs e)
        {
            EXTPOS2();
        }

        protected void TextBox_MZ_DATE_TextChanged(object sender, EventArgs e)
        {
            returnSameDataType(TextBox_MZ_DATE, TextBox_MZ_IDATE);
        }

        protected void TextBox_MZ_IDATE_TextChanged(object sender, EventArgs e)
        {
            returnSameDataType(TextBox_MZ_IDATE, TextBox_MZ_PRID);
        }

        protected void TextBox_MZ_ADATE_TextChanged(object sender, EventArgs e)
        {
            returnSameDataType(TextBox_MZ_ADATE, TextBox_MZ_DATE);
        }

        //protected void TextBox_MZ_TBDATE_TextChanged(object sender, EventArgs e)
        //{
        //    returnSameDataType(TextBox_MZ_TBDATE, TextBox_MZ_AD);
        //}

        protected void TextBox_MZ_WDATE_TextChanged(object sender, EventArgs e)
        {
            returnSameDataType(TextBox_MZ_WDATE, TextBox_OTH_THING);
        }

        protected void TextBox_MZ_ID_TextChanged(object sender, EventArgs e)
        {
            TextBox_MZ_ID.Text = TextBox_MZ_ID.Text.ToUpper();

            preLoad(TextBox_MZ_ID.Text.Trim());
        }

        protected void btPRID_Click(object sender, EventArgs e)
        {
            Session["KTYPE_CID"] = TextBox_MZ_PRID.ClientID;

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_Ktype_Search.aspx?MZ_KTYPE=PRID&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin','top=190,left=200,width=500,height=450,toolbar=0,menubar=0,location=0,directories=0,status=0,scrollbar=yes');", true);
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Session["POSIT2_Mode"] = ViewState["Mode"].ToString();

            Session["POSIT2_CMDSQL"] = ViewState["SqlString"].ToString();

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('PersonalSearchIDwithNAME.aspx?TableName=POSIT2&TPM_FION=" + Request.QueryString["TPM_FION"] + "','查詢','top=190,left=200,width=750,height=400,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }

        protected void TextBox_MZ_BDATE_TextChanged(object sender, EventArgs e)
        {
            returnSameDataType(TextBox_MZ_BDATE, TextBox_MZ_EXOAD);
        }


    }
}
