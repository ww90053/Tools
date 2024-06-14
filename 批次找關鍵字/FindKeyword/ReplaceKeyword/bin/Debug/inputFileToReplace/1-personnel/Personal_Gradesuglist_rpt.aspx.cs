using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TPPDDB._1_personnel
{
    public partial class Personal_Gradesuglist_rpt : System.Web.UI.Page
    {
        int TPM_FION=0;
          

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                A.check_power();
                A.set_Panel_EnterToTAB(ref this.Panel1);                       
                //A.fill_AD_POST(DropDownList_EXAD);
                A.fill_AD_POST_BOSS(DropDownList_EXAD, 1);
                DropDownList_EXAD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                A.fill_unit_code_text(DropDownList_EXUNIT, DropDownList_EXAD.SelectedValue);               
            }

        }

        protected void btPrint_Click(object sender, EventArgs e)
        {   

            string tmp_url = "A_rpt.aspx?fn=gradesuglist&TPM_FION=" + TPM_FION + "&AD=" + DropDownList_EXAD.SelectedValue.Trim() + "&UNIT=" + DropDownList_EXUNIT.SelectedValue.Trim()
                   + "&SWT3=" + DropDownList_MZ_SWT3.SelectedValue.Trim() + "&NO1=" + TextBox_MZ_NO1.Text.Trim() + "&NO2=" + TextBox_MZ_NO2.Text.Trim()+"&SORT=1";

            ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);

        }

        protected void btn_bysn_Click(object sender, EventArgs e)
        {   

            string tmp_url = "A_rpt.aspx?fn=gradesuglist&TPM_FION=" + TPM_FION + "&AD=" + DropDownList_EXAD.SelectedValue.Trim() + "&UNIT=" + DropDownList_EXUNIT.SelectedValue.Trim()
                  + "&SWT3=" + DropDownList_MZ_SWT3.SelectedValue.Trim() + "&NO1=" + TextBox_MZ_NO1.Text.Trim() + "&NO2=" + TextBox_MZ_NO2.Text.Trim() + "&SORT=2";


            ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);

        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            TextBox_MZ_NO1.Text = string.Empty;
            TextBox_MZ_NO2.Text = string.Empty;
            DropDownList_EXAD.SelectedValue = "";
            DropDownList_EXUNIT.SelectedValue = "";
            DropDownList_MZ_SWT3.Text = string.Empty;
        }

        protected void DropDownList_EXUNIT_DataBound(object sender, EventArgs e)
        {
            ListItem li = new ListItem(" ", "");
            DropDownList_EXUNIT.Items.Insert(0, li);
        }

        protected void DropDownList_EXAD_SelectedIndexChanged(object sender, EventArgs e)
        {
            A.fill_unit_code_text(DropDownList_EXUNIT, DropDownList_EXAD.SelectedValue);
        }

    }
}
