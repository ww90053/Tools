using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using TPPDDB.App_Code; 

namespace TPPDDB._3_forleave
{
    public partial class C_ForLeaveOvertime_DUTYTABLE_SEARCH : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
      //MQ ---------------------20100331   
            C.set_Panel_EnterToTAB(ref this.Panel1);         
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.close();", true);
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.opener.location.href='C_ForLeaveOvertime_DUTYTABLE_SET.aspx?DUTYDATE=" + TextBox_DUTYDATE.Text.Trim().Replace("/", "").PadLeft(7, '0') + "&DUTYMODE=" + TextBox_DUTYMODE.Text + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "';window.close();", true);
        }

        protected void TextBox_DUTYMODE_TextChanged(object sender, EventArgs e)
        {
            TextBox_DUTYMODE.Text = TextBox_DUTYMODE.Text.ToUpper();
          
            string Cname = o_DBFactory.ABC_toTest.vExecSQL("SELECT DUTY_NAME FROM C_DUTYITEM WHERE DUTY_NO='" + TextBox_DUTYMODE.Text.Trim() + "'");
          
            if (string.IsNullOrEmpty(Cname))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('查無資料');", true);
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_DUTYMODE.ClientID + "').focus();$get('" + TextBox_DUTYMODE.ClientID + "').focus();", true); 
            }
            else
            {
                TextBox_DUTYMODE1.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT DUTY_NAME FROM C_DUTYITEM WHERE DUTY_NO='" + TextBox_DUTYMODE.Text.Trim().Substring(0, 1) + "'") + "   " + Cname;
            }
        }

        protected void btDUTYMODE_Click(object sender, EventArgs e)
        {
            Session["KTYPE_CID"] = TextBox_DUTYMODE.ClientID;
            Session["KTYPE_CID1"] = TextBox_DUTYMODE1.ClientID;
            //20140731
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", 
                "window.open('../1-personnel/Personal_Ktype_Search.aspx?CID_NAME=DUTYMODE&AD=" + o_A_DLBASE.PMZAD(Session["ADPMZ_ID"].ToString())
                + "&UNIT=" + o_A_DLBASE.PMZUNIT(Session["ADPMZ_ID"].ToString())
                + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin','top=190,left=200,width=420,height=450,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }

        protected void TextBox_DUTYDATE_TextChanged(object sender, EventArgs e)
        {
            TextBox_DUTYDATE.Text = o_str.tosql(TextBox_DUTYDATE.Text.Trim().Replace("/", ""));

            if (TextBox_DUTYDATE.Text != "")
            {
                if (!DateManange.Check_date(TextBox_DUTYDATE.Text))
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('日期格式錯誤')", true);
                    TextBox_DUTYDATE.Text = string.Empty;
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_DUTYDATE.ClientID + "').focus();$get('" + TextBox_DUTYDATE.ClientID + "').focus();", true);
                }
                else
                {
                    TextBox_DUTYDATE.Text = o_CommonService.Personal_ReturnDateString(TextBox_DUTYDATE.Text.Trim());
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_DUTYMODE.ClientID + "').focus();$get('" + TextBox_DUTYMODE.ClientID + "').focus();", true);
                }
            }
        }
    }
}
