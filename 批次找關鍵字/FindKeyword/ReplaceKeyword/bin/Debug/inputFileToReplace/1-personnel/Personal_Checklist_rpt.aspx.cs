using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TPPDDB._1_personnel
{
    public partial class Personal_Checklist_rpt : System.Web.UI.Page
    {
        int TPM_FION=0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                A.check_power();                
            }
        }

        protected void btPrint_Click(object sender, EventArgs e)
        {
            if ((sender as Button).Text == "全部")
            {
                string tmp_url = "A_rpt.aspx?fn=checklist1&TPM_FION=" + TPM_FION + "&MZ_SWT3=" + DropDownList_MZ_SWT3.Text.Trim() + "&NO=" + TextBox_MZ_NO1.Text.Trim() + "&NO2=" + TextBox_MZ_NO2.Text.Trim();
                ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
            }
            else
            {
                string tmp_url = "A_rpt.aspx?fn=checklist&TPM_FION=" + TPM_FION + "&MZ_SWT3=" + DropDownList_MZ_SWT3.Text.Trim() + "&NO=" + TextBox_MZ_NO1.Text.Trim() + "&NO2=" + TextBox_MZ_NO2.Text.Trim();
                ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
            }

        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            TextBox_MZ_NO1.Text = string.Empty;
            TextBox_MZ_NO2.Text = string.Empty;
        }

        /// <summary>
        /// 按鈕: 依機關列印ODF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_PrintODF_Click(object sender, EventArgs e)
        {
            string tmp_url = "A_rpt.aspx?fn=checklist_ODF&TPM_FION=" + TPM_FION + "&MZ_SWT3=" + DropDownList_MZ_SWT3.Text.Trim() + "&NO=" + TextBox_MZ_NO1.Text.Trim() + "&NO2=" + TextBox_MZ_NO2.Text.Trim();
            ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
        }
    }
}
