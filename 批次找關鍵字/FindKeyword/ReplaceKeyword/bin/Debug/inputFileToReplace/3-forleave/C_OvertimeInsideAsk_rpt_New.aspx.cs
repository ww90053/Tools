using System;
using System.Web.UI;

namespace TPPDDB._3_forleave
{
    //TODO SKY 正式上線時刪除
    public partial class C_OvertimeInsideAsk_rpt_New : System.Web.UI.Page
    {
        int TPM_FION = 0;
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
            if (!string.IsNullOrEmpty(TextBox_MZ_ID.Text) && !string.IsNullOrEmpty(TextBox_MZ_DATE.Text))
            {
                Session["RPT_C_IDNO"] = TextBox_MZ_ID.Text;
                Session["RPT_C_DATE"] = TextBox_MZ_DATE.Text.Trim().Replace("/", "").PadLeft(7, '0');
                string tmp_url = string.Format("C_rpt.aspx?fn=OvertimeInsideAsk_New&TPM_FION={0}", TPM_FION);

                ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Panel1, GetType(), "click", "alert('身分證號、加班日期不能為空白!');", true);
            }
        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            TextBox_MZ_DATE.Text = string.Empty;
            TextBox_MZ_ID.Text = string.Empty;
        }
    }
}