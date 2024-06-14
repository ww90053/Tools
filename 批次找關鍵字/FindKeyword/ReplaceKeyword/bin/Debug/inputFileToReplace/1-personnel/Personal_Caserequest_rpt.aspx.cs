using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TPPDDB._1_personnel
{
    public partial class Personal_Caserequest_rpt : System.Web.UI.Page
    {
        int TPM_FION=0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                A.check_power();
                 A.set_Panel_EnterToTAB(ref this.Panel1);          

            }
            
          
        }

        protected void btPrint_Click(object sender, EventArgs e)
        {
            
            string tmp_url = "A_rpt.aspx?fn=caserequest&TPM_FION=" + TPM_FION + "&NO1=" + TextBox_MZ_NO1.Text.Trim() + "&NO2=" + TextBox_MZ_NO2.Text.Trim();

            ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);

        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            TextBox_MZ_NO1.Text = string.Empty;
            TextBox_MZ_NO2.Text = string.Empty;
        }

    }
}
