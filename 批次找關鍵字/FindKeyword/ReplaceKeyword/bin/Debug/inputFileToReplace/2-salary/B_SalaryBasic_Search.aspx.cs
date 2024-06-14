using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace TPPDDB._2_salary
{
    public partial class B_SalaryBasic_Search : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           if (IsPostBack != true)
            { 
                SalaryPublic.checkPermission();
                SalaryPublic.fillDropDownList(ref DropDownList_PAY_AD);
                SalaryPublic.fillUnitDropDownList(ref DropDownList_MZ_UNIT, DropDownList_PAY_AD.SelectedValue);

                PanelAdmin.Visible = false;// 不讓人用礙眼的PanelAdmin
                TextBox_AMONTH.Text = (DateTime.Now.Year - 1911).ToString("000") + DateTime.Now.Month.ToString("00");
                RadioButtonList_ADType.Visible = false;

                switch (Request["reID"])
                {
                    case "BASIC":
                        tr_SOLE_DA.Visible = true;
                        tr_AYEAR.Visible = false;
                        tr_AMONTH.Visible = false;
                        tr_SOLE_DA.Visible = false;
                        SalaryPublic.AddFirstSelection(ref DropDownList_PAY_AD);// 只有基本資料頁的選項有"全部"

                        // 如果是基本資料頁呼叫此頁面，發薪機關預設帶出納的現服機關
                        DropDownList_PAY_AD.SelectedValue = SalaryPublic.strLoginEXAD;
                        break;

                    case "MONTHPAY":
                        tr_AYEAR.Visible = false;
                        tr_AMONTH.Visible = true;
                        tr_SOLE_DA.Visible = false;
                        int intDate_Now_Month = (DateTime.Now.Month + 1);
                        int intDate_Now_Month_Update;
                        int intDate_Now_Year_Update;
                        if (intDate_Now_Month > 12)
                        {
                            intDate_Now_Year_Update = ((DateTime.Now.Year - 1911) + 1);
                            intDate_Now_Month_Update = 1;
                        }
                        else
                        {
                            intDate_Now_Year_Update = (DateTime.Now.Year - 1911);
                            intDate_Now_Month_Update = intDate_Now_Month;
                        }
                        TextBox_AMONTH.Text = intDate_Now_Year_Update.ToString("000") + intDate_Now_Month_Update.ToString("00");
                        break;

                    default:
                        //PanelAdmin.Visible = false;

                        tr_AYEAR.Visible = false;
                        tr_AMONTH.Visible = false;
                        tr_SOLE_DA.Visible = false;
                        if (Request["reID"] == "MONTHPAY" || Request["reID"] == "REPAIRPAY")
                        {
                            tr_AMONTH.Visible = true;
                        }
                        if (Request["reID"] == "SOLE")
                        {
                            tr_SOLE_DA.Visible = true;
                        }

                        break;
                }

                TextBox_AYEAR.Text = (DateTime.Now.Year - 1911).ToString("000");
                TextBox_PAY_AD.Text = SalaryPublic.strLoginEXAD;//原本使用Session["ADPMZ_EXAD"]
            }
        }

        protected void btOK_Click(object sender, EventArgs e)
        {
            DateTime dtNOW = DateTime.Now;
            switch (Request["reID"])
            {
                case "BASIC":
                    HttpCookie Cookie1 = new HttpCookie("B_SalaryBasic_ID");
                    //Cookie1.Value = TPMPermissions._strEncood(TextBox_ID.Text.Trim());


                    Cookie1.Values.Add("strMZ_ID", "");
                    Cookie1.Values.Add("strMZ_POLNO", "");
                    Cookie1.Values.Add("strMZ_NAME", "");
                    Cookie1.Values.Add("strPAY_AD", "");
                    Cookie1.Values.Add("strMZ_AD", "");
                    Cookie1.Values.Add("strMZ_EXAD", "");
                    Cookie1.Values.Add("strPAY_UNIT", "");
                    Cookie1.Values.Add("strMZ_UNIT", "");
                    Cookie1.Values.Add("strMZ_EXUNIT", "");

                    Cookie1.Values["strMZ_ID"] = TPMPermissions._strEncood(TextBox_MZ_ID.Text.Trim().ToUpper());
                    Cookie1.Values["strMZ_POLNO"] = TextBox_MZ_POLNO.Text.Trim();
                    Cookie1.Values["strMZ_NAME"] = TPMPermissions._strEncood(TextBox_MZ_NAME.Text.Trim());

                    if (DropDownList_MZ_UNIT.SelectedIndex != 0)
                        Cookie1.Values["strPAY_UNIT"] = DropDownList_MZ_UNIT.SelectedValue;

                    if (RadioButtonList_ADType.SelectedValue == "0")// 發薪機關
                    {
                        Cookie1.Values["strPAY_AD"] = DropDownList_PAY_AD.SelectedValue.Trim();
                    }
                    else if (RadioButtonList_ADType.SelectedValue == "1")// 編制機關
                    {
                        Cookie1.Values["strMZ_AD"] = DropDownList_PAY_AD.SelectedValue.Trim();
                    }
                    else// 現服機關
                    {
                        Cookie1.Values["strMZ_EXAD"] = DropDownList_PAY_AD.SelectedValue.Trim();
                    }

                    Cookie1.Values["strMZ_UNIT"] = TextBox_MZ_UNIT.Text.Trim();
                    Cookie1.Values["strMZ_EXUNIT"] = TextBox_MZ_EXUNIT.Text.Trim();

                    Cookie1.Expires = dtNOW.AddMinutes(20);
                    Response.Cookies.Add(Cookie1);

                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.opener.location.href='B_SalaryBasic1.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "';window.close();", true);
                    break;
                case "MONTHPAY":
                    HttpCookie Cookie_MONTHPAY = new HttpCookie("B_SalaryMONTHPAY_ID");
                    Cookie_MONTHPAY.Values.Add("strMZ_ID", TPMPermissions._strEncood(TextBox_MZ_ID.Text.Trim()));
                    Cookie_MONTHPAY.Values.Add("strMZ_POLNO", TextBox_MZ_POLNO.Text.Trim());
                    Cookie_MONTHPAY.Values.Add("strMZ_NAME", TPMPermissions._strEncood(TextBox_MZ_NAME.Text.Trim()));
                    Cookie_MONTHPAY.Values.Add("strPAY_AD", DropDownList_PAY_AD.SelectedValue.Trim());
                    Cookie_MONTHPAY.Values.Add("strPAY_UNIT", DropDownList_MZ_UNIT.SelectedValue);
                    Cookie_MONTHPAY.Values.Add("strAMONTH", TextBox_AMONTH.Text.Trim());
                    Cookie_MONTHPAY.Expires = dtNOW.AddMinutes(20);
                    Response.Cookies.Add(Cookie_MONTHPAY);

                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.opener.location.href='B_SalaryMonth3.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "';window.close();", true);
                    break;
                case "REPAIRPAY":
                    HttpCookie Cookie_REPAIRPAY = new HttpCookie("B_SalaryREPAIRPAY_ID");
                    Cookie_REPAIRPAY.Values.Add("strMZ_ID", TPMPermissions._strEncood(TextBox_MZ_ID.Text.Trim()));
                    Cookie_REPAIRPAY.Values.Add("strMZ_POLNO", TextBox_MZ_POLNO.Text.Trim());
                    Cookie_REPAIRPAY.Values.Add("strMZ_NAME", TPMPermissions._strEncood(TextBox_MZ_NAME.Text.Trim()));
                    Cookie_REPAIRPAY.Values.Add("strPAY_AD", DropDownList_PAY_AD.SelectedValue.Trim());
                    Cookie_REPAIRPAY.Values.Add("strPAY_UNIT", DropDownList_MZ_UNIT.SelectedValue);
                    Cookie_REPAIRPAY.Values.Add("strAMONTH", TextBox_AMONTH.Text.Trim());
                    Cookie_REPAIRPAY.Expires = dtNOW.AddMinutes(20);
                    Response.Cookies.Add(Cookie_REPAIRPAY);

                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.opener.location.href='B_SalaryRepair1.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "';window.close();", true);
                    break;
                case "SOLE":
                    HttpCookie Cookie_SOLE = new HttpCookie("B_SalarySOLE_ID");
                    Cookie_SOLE.Values.Add("strMZ_ID", TPMPermissions._strEncood(TextBox_MZ_ID.Text.Trim()));
                    Cookie_SOLE.Values.Add("strMZ_POLNO", TextBox_MZ_POLNO.Text.Trim());
                    Cookie_SOLE.Values.Add("strMZ_NAME", TPMPermissions._strEncood(TextBox_MZ_NAME.Text.Trim()));
                    Cookie_SOLE.Values.Add("strPAY_AD", DropDownList_PAY_AD.SelectedValue.Trim());
                    Cookie_SOLE.Values.Add("strPAY_UNIT", DropDownList_MZ_UNIT.SelectedValue);
                    Cookie_SOLE.Values.Add("strDA", TextBox_DA.Text.Trim());
                    Cookie_SOLE.Expires = dtNOW.AddMinutes(20);
                    Response.Cookies.Add(Cookie_SOLE);

                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.opener.location.href='B_SalarySole1.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "';window.close();", true);
                    break;
            }
        }

        protected void btExit_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.close();", true);
        }

        private void Ktype_Cname_Check(string Cname, Label tb1, TextBox tb2, object obj)
        {

            tb2.Text = tb2.Text.ToUpper();
            //檢查代碼中文名稱是否存在
            if (string.IsNullOrEmpty(Cname))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('查無資料')", true);
                tb1.Text = string.Empty;
                tb2.Text = string.Empty;
                tb2.Focus();
            }
            else
            {
                tb1.Text = Cname;
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

        protected void Ktype_Search(TextBox tb1, Label tb2, string ktype)
        {
            //回傳值
            Session["KTYPE_CID"] = tb1.ClientID;
            Session["KTYPE_CID1"] = tb2.ClientID;
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('/1-personnel/Personal_Ktype_Search.aspx?MZ_KTYPE=" + ktype + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin','top=190,left=200,width=420,height=450,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }

        protected void TextBox_PAY_AD_TextChanged(object sender, EventArgs e)
        {
            string CName = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND MZ_KCODE LIKE '38213%' UNION ALL SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND MZ_KCODE NOT LIKE '38213%') WHERE MZ_KCODE='" + TextBox_PAY_AD.Text.Trim().ToUpper() + "'");
            Ktype_Cname_Check(CName, Label_PAY_AD, TextBox_PAY_AD, TextBox_MZ_AD);
        }

        protected void btPAY_AD_Click(object sender, EventArgs e)
        {
            Session["KTYPE_CID"] = TextBox_PAY_AD.ClientID;
            Session["KTYPE_CID1"] = Label_PAY_AD.ClientID;
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('/1-personnel/Personal_Ktype_Search.aspx?CID_NAME=AD&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin','top=190,left=200,width=420,height=450,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }

        protected void TextBox_MZ_AD_TextChanged(object sender, EventArgs e)
        {
            string CName = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND MZ_KCODE LIKE '38213%' UNION ALL SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND MZ_KCODE NOT LIKE '38213%') WHERE MZ_KCODE='" + TextBox_MZ_AD.Text.Trim().ToUpper() + "'");
            Ktype_Cname_Check(CName, Label_MZ_AD, TextBox_MZ_AD, TextBox_MZ_UNIT);
        }

        protected void btMZ_AD_Click(object sender, EventArgs e)
        {
            Session["KTYPE_CID"] = TextBox_MZ_AD.ClientID;
            Session["KTYPE_CID1"] = Label_MZ_AD.ClientID;
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('/1-personnel/Personal_Ktype_Search.aspx?CID_NAME=AD&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin','top=190,left=200,width=420,height=450,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }

        protected void TextBox_MZ_UNIT_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_UNIT.Text, "25");
            Ktype_Cname_Check(CName, Label_MZ_UNIT, TextBox_MZ_UNIT, TextBox_MZ_EXAD);
        }

        protected void btMZ_UNIT_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_MZ_UNIT, Label_MZ_UNIT, "25");
        }

        protected void TextBox_MZ_EXAD_TextChanged(object sender, EventArgs e)
        {
            string CName = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND MZ_KCODE LIKE '38213%' UNION ALL SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND MZ_KCODE NOT LIKE '38213%') WHERE MZ_KCODE='" + TextBox_MZ_EXAD.Text.Trim().ToUpper() + "'");
            Ktype_Cname_Check(CName, Label_MZ_EXAD, TextBox_MZ_EXAD, TextBox_MZ_EXUNIT);
        }

        protected void btMZ_EXAD_Click(object sender, EventArgs e)
        {
            Session["KTYPE_CID"] = TextBox_MZ_EXAD.ClientID;
            Session["KTYPE_CID1"] = Label_MZ_EXAD.ClientID;
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('/1-personnel/Personal_Ktype_Search.aspx?CID_NAME=AD&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin','top=190,left=200,width=420,height=450,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }

        protected void TextBox_MZ_EXUNIT_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_EXUNIT.Text, "25");
            Ktype_Cname_Check(CName, Label_MZ_EXUNIT, TextBox_MZ_EXUNIT, btOK);
        }

        protected void btMZ_EXUNIT_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_MZ_EXUNIT, Label_MZ_EXUNIT, "25");
        }

        protected void DropDownList_PAY_AD_SelectedIndexChanged(object sender, EventArgs e)
        {
            SalaryPublic.fillUnitDropDownList(ref DropDownList_MZ_UNIT, DropDownList_PAY_AD.SelectedValue);
        }
    }
}
