using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using TPPDDB.App_Code;

namespace TPPDDB._3_forleave
{
    public partial class C_OvertimeInsideAsk_rpt : System.Web.UI.Page
    {
        int TPM_FION=0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                C.check_power();

                //MQ-----------------------20100331
                C.set_Panel_EnterToTAB(ref this.Panel1);
            }
        }

        protected void btPrint_Click(object sender, EventArgs e)
        {
            

            Session["RPT_C_IDNO"] = TextBox_MZ_ID.Text;

            string tmp_url = "C_rpt.aspx?fn=OvertimeInsideAsk&TPM_FION=" + TPM_FION
                    + "&DATE=" + TextBox_MZ_DATE.Text.Trim().Replace("/", "").PadLeft(7, '0');
                    //+"&TYPE="+2;
            ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            TextBox_MZ_DATE.Text = string.Empty;
            TextBox_MZ_ID.Text = string.Empty;
        }
    }
}
