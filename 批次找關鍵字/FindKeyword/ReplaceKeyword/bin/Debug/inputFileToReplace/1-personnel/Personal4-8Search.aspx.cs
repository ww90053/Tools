using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TPPDDB._1_personnel
{
    public partial class Personal4_8Search : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //by MQ 20100311---------
            A.set_Panel_EnterToTAB(ref this.Panel_NOTE);
            TextBox_MZ_NOTE.Focus();
        }

        protected void btOk_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.opener.location.href='Personal4-8.aspx?MZ_NOTE=" + TextBox_MZ_NOTE.Text.Trim() + "&MZ_NOTE_NAME=" + TextBox_MZ_NOTE_NAME.Text.Trim() + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "';window.close();", true);
        }

        protected void btLeave_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.close();", true); 
        }
    }
}
