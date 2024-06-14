using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TPPDDB._1_personnel
{
    public partial class Personal4_7Search : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btOk_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.opener.location.href='Personal4-7.aspx?MZ_POLNO=" + TextBox_MZ_POLNO.Text.Trim() + "&MZ_PNAME=" + TextBox_MZ_PNAME.Text.Trim() + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "';window.close();", true);
        }

        protected void btLeave_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.close();", true); 
        }
    }
}
