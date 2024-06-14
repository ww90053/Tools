using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TPPDDB._1_personnel
{
    public partial class Personal4_2Search : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            A.set_Panel_EnterToTAB(ref this.Panel_KTYPE);
            TextBox_MZ_KTYPE.Focus();
        }

        protected void btOk_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.opener.location.href='Personal4-2.aspx?MZ_KTYPE=" + TextBox_MZ_KTYPE.Text.Trim() + "&MZ_KCODE=" + TextBox_MZ_KCODE.Text.Trim() + "&MZ_KCHI=" + Server.UrlEncode (TextBox_MZ_KCHI.Text.Trim()) + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "';window.close();", true);
        }

        protected void btLeave_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.close();", true); 
        }

        protected void TextBox_MZ_KCODE_TextChanged(object sender, EventArgs e)
        {
            TextBox_MZ_KCODE.Text = TextBox_MZ_KCODE.Text.ToUpper().Trim();
            TextBox_MZ_KCODE.Focus();
        }
    }
}
