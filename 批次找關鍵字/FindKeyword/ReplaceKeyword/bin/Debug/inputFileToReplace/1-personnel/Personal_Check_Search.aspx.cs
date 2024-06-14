using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace TPPDDB._1_personnel
{
    public partial class Personal_Check_Search : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            A.set_Panel_EnterToTAB(ref this.Panel1);

        }

        protected void btOK_Click(object sender, EventArgs e)
        {
            Session["Personal_Check_ID"] = TextBox_MZ_ID.Text.Trim();
            Session["Personal_Check_NAME"] = TextBox_MZ_NAME.Text.Trim();
            Session["Personal_Check_RETRIEVE"] = TextBox_MZ_RETRIEVE.Text.Trim();
            Session["Personal_CHeck_IDATE1"] = string.IsNullOrEmpty(TextBox_MZ_IDATE.Text.Trim()) ? "" : TextBox_MZ_IDATE.Text.Trim().Replace("/", "").PadLeft(7, '0');
            Session["Personal_CHeck_IDATE2"] = string.IsNullOrEmpty(TextBox_MZ_IDATE1.Text.Trim()) ? "" : TextBox_MZ_IDATE1.Text.Trim().Replace("/", "").PadLeft(7, '0');
            Session["Personal_Check_UNAD"] = DropDownList_MZ_UNAD.SelectedValue;
            Session["Personal_MZ_CONTENT"] = DropDownList_MZ_CONTENT.SelectedValue;
           
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.opener.location.href='Personal_Check.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "';window.close();", true);
        }

        protected void btLeave_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.close();", true);
        }
    }
}
