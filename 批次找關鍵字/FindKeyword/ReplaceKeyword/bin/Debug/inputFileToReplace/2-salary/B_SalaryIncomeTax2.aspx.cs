using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TPPDDB._2_salary
{
    public partial class B_SalaryIncomeTax2 : System.Web.UI.Page
    {

        private string strAYEAR
        {
            get
            {
                return txt_year.Text.PadLeft(3, '0');
            }
            set
            {
                txt_year.Text = value;
            }
        }

        private string strPAY_AD
        {
            get
            {
                return DropDownList_PAY_AD.SelectedValue;
            }
            set
            {
                DropDownList_PAY_AD.SelectedValue = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
           

            if (IsPostBack != true)
            { //檢驗權限
            SalaryPublic.checkPermission();
                if (Session["ADPMZ_ID"].ToString() == "R122311721")
                    r11.Visible = true;
                else
                    r11.Visible = false;
                string strYEAR = (DateTime.Now.Year - 1911).ToString("000");
                SalaryPublic.fillDropDownList(ref this.DropDownList_PAY_AD);
            }
        }

        protected void btCreateTAX_Click(object sender, EventArgs e)
        {
            if (rblType.SelectedValue == "全部")
            {
                Tax.createTax(strPAY_AD, strAYEAR, "", "");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('產生完成');", true);
            }
            else
            {
                Tax.createTax(strPAY_AD, strAYEAR, "", tbIDNO.Text.Trim().Length > 0 ? tbIDNO.Text : "9999999999");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('產生完成');", true);
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Tax.createTax(strPAY_AD, strAYEAR, TextBox1.Text, "");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('重跑完成');", true);
        }
    }
}
