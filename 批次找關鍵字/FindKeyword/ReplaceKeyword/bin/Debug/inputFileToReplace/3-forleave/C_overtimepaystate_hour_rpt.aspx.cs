using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TPPDDB._3_forleave
{
    public partial class C_overtimepaystate_hour_rpt : System.Web.UI.Page
    {
        int TPM_FION=0;

        protected void Page_Load(object sender, EventArgs e)
        {            

            if (!IsPostBack)
            {

                //C.fill_AD_POST(DropDownList_AD);
                C.fill_AD(DropDownList_AD);
                TextBox_MZ_DATE1.Text = (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0') + DateTime.Now.Month.ToString().PadLeft(2, '0');
                DropDownList_AD.SelectedValue = Session["ADPMZ_EXAD"].ToString();

                C.fill_unit(DropDownList_UNIT, DropDownList_AD.SelectedValue);
                DropDownList_UNIT.SelectedValue = Session["ADPMZ_EXUNIT"].ToString();

                // C、D權限要鎖定發薪機關
                switch (o_a_Function.strGID(Session["ADPMZ_ID"].ToString()))
                {
                    case "C":
                        DropDownList_AD.Enabled = false;
                        break;
                    case "D":
                        DropDownList_AD.Enabled = false;
                        break;
                }
            }
        }

        protected void btPrint_Click(object sender, EventArgs e)
        {


            if (!string.IsNullOrEmpty(DropDownList_UNIT.SelectedValue))
            {
 
                string tmp_url = "C_rpt.aspx?fn=OvertimeOutSide_Unit&TPM_FION=" + TPM_FION +
                    "&DATE=" + TextBox_MZ_DATE1.Text + "&MZ_AD=" + DropDownList_AD.SelectedValue + "&MZ_UNIT=" + DropDownList_UNIT.SelectedValue;

                ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
            }
            else
            {


                string tmp_url = "C_rpt.aspx?fn=OvertimeOutSide_Hour&TPM_FION=" + TPM_FION +
                    "&DATE=" + TextBox_MZ_DATE1.Text + "&MZ_AD=" + DropDownList_AD.SelectedValue ; 

                ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
            }
        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            DropDownList_UNIT.SelectedValue = "";
            TextBox_MZ_DATE1.Text = string.Empty;
        }

        protected void DropDownList_UNIT_DataBound(object sender, EventArgs e)
        {
            DropDownList_UNIT.Items.Insert(0, new ListItem(" ", ""));
        }

        protected void DropDownList_AD_SelectedIndexChanged(object sender, EventArgs e)
        {
            C.fill_unit(DropDownList_UNIT, DropDownList_AD.SelectedValue);
        }
    }
}
