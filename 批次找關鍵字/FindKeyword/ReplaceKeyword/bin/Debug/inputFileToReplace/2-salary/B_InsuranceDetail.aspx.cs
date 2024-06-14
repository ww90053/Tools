using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TPPDDB._2_salary
{
    public partial class B_InsuranceDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                SalaryPublic.checkPermission();
                TextBox_AMONTH.Text = "09811";
                SalaryPublic.fillDropDownList(ref this.DropDownList_PAY_AD);
            }

        }

        protected void Button_Send_Click(object sender, EventArgs e)
        {
            string tmp_url = "B_rpt.aspx?fn=insuranceDetail";
            //Session["PAY_AD"] = SalaryBasic.strMZ_AD_Data(strPAY_AD);
            //Session["YEAR"] = strDATE.Substring(0, 3);
            //Session["MONTH"] = strDATE.Substring(3, 2);
            //Session["rpt_dt"] = dt_TO_rpt;
            ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
        }
    }
}
