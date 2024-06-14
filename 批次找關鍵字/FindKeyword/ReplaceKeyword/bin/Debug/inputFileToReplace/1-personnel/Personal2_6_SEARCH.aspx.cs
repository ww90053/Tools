using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TPPDDB._1_personnel
{
    public partial class Personal2_6_SEARCH : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btOK_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.opener.location.href='Personal2-6.aspx?MZ_NO="+o_str.tosql(TextBox_MZ_NO.Text.Trim())+"&TPM_FION=" + Request.QueryString["TPM_FION"] + "';window.close();", true);
        }

        protected void btLEAVE_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.close();", true); 
        }
    }
}
