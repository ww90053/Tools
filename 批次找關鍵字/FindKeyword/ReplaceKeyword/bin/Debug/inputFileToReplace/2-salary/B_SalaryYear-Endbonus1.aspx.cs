using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace TPPDDB._2_salary
{
    public partial class B_SalaryYear_Endbonus1 : System.Web.UI.Page
    {
       
        DataTable temp = new DataTable();
        DataTable dtPayadCounts
        {
            set { ViewState["dtPayadCounts"] = value; }
            get { return (DataTable)ViewState["dtPayadCounts"]; }
        }
        private string strYear
        {
            get
            {
                return txt_year.Text;
            }
        }

        private string strUnit
        {
            get
            {
                return DropDownList_MZ_AD.SelectedValue;
            }
        }

        private string strRadioButtonList_ALLORPersonnel
        {
            get
            {
                return RadioButtonList_ALLORPersonnel.SelectedValue;
            }
        }

        private string strGroup_MZ_ID_Data
        {
            get
            {
                return TextBox_Group_MZ_ID_Data.Text;
            }
        }

        private double _doubleAMONTH;
        private double doubleAMONTH
        {
            get
            {
                if (TextBox_AMONTH.Text != "")
                {
                    return double.Parse(TextBox_AMONTH.Text);
                }
                else { return 0; }
            }
            set
            {
                _doubleAMONTH = value;
                if (string.IsNullOrEmpty(_doubleAMONTH.ToString()))
                {
                    TextBox_AMONTH.Text = "0";
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
          
            if (!Page.IsPostBack)
            {  SalaryPublic.checkPermission();

                dtPayadCounts = Salary.getPayadOffDutyCounts();
                TextBox_AMONTH.Text = "1.5";

                txt_year.Text = (DateTime.Now.Year - 1911).ToString("000");

                SalaryPublic.fillDropDownList(ref this.DropDownList_MZ_AD);
                if (DropDownList_MZ_AD.SelectedValue == "382133400C")
                {
                    lb_Counts.Text = "0";
                }
                else if (DropDownList_MZ_AD.SelectedValue == "382133500C")
                {
                    lb_Counts.Text = "0";
                }
                else
                {
                    lb_Counts.Text = dtPayadCounts.Select("PAY_AD='" + DropDownList_MZ_AD.SelectedValue + "'").First()["Count"].ToString();
                }
            }

            if (Session["ADPPAY_AD"] == null)
                //20140701Session["ADPPAY_AD"] = SalaryPublic.strMZ_IDtoPAY_AD(Session["ADPMZ_ID"].ToString());
                Session["ADPPAY_AD"] = SalaryPublic.str_A_Column("PAY_AD", 1, Session["ADPMZ_ID"].ToString());
        }

        protected void btEFFECTTable_Click(object sender, EventArgs e)
        {

        }

        protected void btCreate_Click(object sender, EventArgs e)
        {
            double plus;
            int total;
            DataTable dtError = new DataTable();

            double.TryParse(TextBox_AMONTH.Text, out plus);
            ListBox_Effect_Data.Items.Clear();

            if (RadioButtonList_ALLORPersonnel.SelectedValue == "ALL")
            {
                int.TryParse(lb_Counts.Text, out total);
                dtError = YearEnd.createYearEnd(strYear.ToString(), DropDownList_MZ_AD.SelectedValue, plus, null);
            }
            else
            {
                total = 1;
                dtError = YearEnd.createYearEnd(strYear.ToString(), DropDownList_MZ_AD.SelectedValue, plus, TextBox_Group_MZ_ID_Data.Text);
            }

            foreach (DataRow row in dtError.Rows)
            {
                ListItem li = new ListItem(row["Name"].ToString() + ":" + row["Polno"].ToString());
                ListBox_Effect_Data.Items.Add(li);
            }

            lb_success.Text = (total - dtError.Rows.Count).ToString();
            lb_fail.Text = dtError.Rows.Count.ToString();
        }

        protected void RadioButtonList_ALLORPersonnel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((sender) as RadioButtonList).SelectedValue != "ALL")
            {
                trIDCARD.Visible = true;
            }
            else
            {
                trIDCARD.Visible = false;
            }
        }

        protected void DropDownList_MZ_AD_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownList_MZ_AD.SelectedValue == "382133400C")
            {
                lb_Counts.Text = "0";
            }
            else if (DropDownList_MZ_AD.SelectedValue == "382133500C")
            {
                lb_Counts.Text = "0";
            }
            else
            {
                lb_Counts.Text = dtPayadCounts.Select("PAY_AD='" + DropDownList_MZ_AD.SelectedValue + "'").First()["Count"].ToString();
            }

            //lb_Counts.Text = dtPayadCounts.Select("PAY_AD='" + DropDownList_MZ_AD.SelectedValue + "'").First()["Count"].ToString();
        }
    }
}
