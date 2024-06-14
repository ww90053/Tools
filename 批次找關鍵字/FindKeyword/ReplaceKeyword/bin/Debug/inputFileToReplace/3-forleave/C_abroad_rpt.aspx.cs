using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TPPDDB._3_forleave
{
    public partial class C_abroad_rpt : System.Web.UI.Page
    {
        int TPM_FION=0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                C.check_power();            
            
                //by MQ ------------------------------20100331
            
                C.set_Panel_EnterToTAB(ref this.Panel1);
           
                TextBox_MZ_ID.Text = Session["ADPMZ_ID"].ToString();
            }
        }
        // 出國請假報告單
        protected void btPrint_Click(object sender, EventArgs e)
        {
            
            Session["RPT_C_IDNO"] = TextBox_MZ_ID.Text.Trim();

            string tmp_url = "C_rpt.aspx?fn=abroad" +
                "&DATE=" + TextBox_MZ_DATE.Text.Trim().Replace("/", string.Empty).PadLeft(7, '0') +
                "&MZ_NAME=" + HttpUtility.UrlEncode(TextBox_MZ_NAME.Text.Trim()) +
                "&TPM_FION=" + TPM_FION; ;

                ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);

        }
        // 赴大陸請假報告單
        protected void btPrint_Click_CN(object sender, EventArgs e)
        {

            Session["RPT_C_IDNO"] = TextBox_MZ_ID.Text.Trim();

            string tmp_url = "C_rpt.aspx?fn=abroad_CN" +
                "&DATE=" + TextBox_MZ_DATE.Text.Trim().Replace("/", string.Empty).PadLeft(7, '0') +
                "&MZ_NAME=" + HttpUtility.UrlEncode(TextBox_MZ_NAME.Text.Trim()) +
                "&TPM_FION=" + TPM_FION; ;

            ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);

        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            TextBox_MZ_DATE.Text = string.Empty;
            TextBox_MZ_ID.Text = string.Empty;
            TextBox_MZ_NAME.Text = string.Empty;
        }
    }
}
