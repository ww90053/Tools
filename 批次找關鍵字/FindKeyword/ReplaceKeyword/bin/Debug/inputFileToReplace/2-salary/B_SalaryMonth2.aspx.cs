using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TPPDDB._2_salary
{
    public partial class B_SalaryMonth2 : System.Web.UI.Page
    {
        DataTable dtPayadCounts
        {
            set { ViewState["dtPayadCounts"] = value; }
            get { return (DataTable)ViewState["dtPayadCounts"]; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SalaryPublic.checkPermission();
                dtPayadCounts = Salary.getPayadCounts();// 取得人事資料中各發薪機關現有人數
                DateTime dt = DateTime.Now.AddMonths(1);
                //txt_amonth.Text = (dt.Year - 1911).ToString("000") + (dt.Month + 1).ToString("00");
                txt_amonth.Text = (dt.Year - 1911).ToString("000") + (dt.Month ).ToString("00");


                //matthew 中和合併之後不用在顯示中一中二的下拉選單
                SalaryPublic.fillDropDownList_Chang(ref this.DropDownList_PAY_AD);
                //SalaryPublic.fillDropDownList(ref this.DropDownList_PAY_AD);
                lb_Counts.Text = dtPayadCounts.Select("PAY_AD='" + DropDownList_PAY_AD.SelectedValue + "'").First()["Count"].ToString();
                TextBox_Group_MZ_ID_Data.Text = "";
            }
        }

        private string strPAY_AD
        {
            get
            {
                return DropDownList_PAY_AD.SelectedValue;
            }
        }

        private string strAMONTH
        {
            get
            {
                return txt_amonth.Text;
            }
            set
            {
                txt_amonth.Text = value;
            }
        }

        private string strRadioButtonList_ALLORPersonnel
        {
            get
            {
                return RadioButtonList_ALLORPersonnel.SelectedValue;
            }
            set
            {
                RadioButtonList_ALLORPersonnel.SelectedValue = value;
            }
        }

        private string strGroup_MZ_ID_Data
        {
            get
            {
                return TextBox_Group_MZ_ID_Data.Text;
            }
        }

        protected void btCreate_Click(object sender, EventArgs e)
        {
            int total;
            DataTable dtError = new DataTable();

            ListBox_MonthPay_Data.Items.Clear();
            //如果人員資料選擇 全部
            if (RadioButtonList_ALLORPersonnel.SelectedValue == "ALL")
            {
                int.TryParse(lb_Counts.Text, out total);
                dtError = Monthpay.createMonthpay(strAMONTH, DropDownList_PAY_AD.SelectedValue);
            }
            else
            {//如果指定單一人的身分證
                total = 1;
                dtError = Monthpay.createMonthpay(strAMONTH, DropDownList_PAY_AD.SelectedValue, TextBox_Group_MZ_ID_Data.Text);
            }

            foreach (DataRow row in dtError.Rows)
            {
                ListItem li = new ListItem(row["Name"].ToString() + ":" + row["Polno"].ToString());
                ListBox_MonthPay_Data.Items.Add(li);
            }

            lb_success.Text = (total - dtError.Rows.Count).ToString();
            lb_fail.Text = dtError.Rows.Count.ToString();
        }

        protected void RadioButtonList_ALLORPersonnel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (strRadioButtonList_ALLORPersonnel == "ALL")
            {
                trIDCARD.Visible = false;
            }
            else
            {
                trIDCARD.Visible = true;
            }
        }

        protected void DropDownList_PAY_AD_SelectedIndexChanged(object sender, EventArgs e)
        {
            lb_Counts.Text = dtPayadCounts.Select("PAY_AD='" + DropDownList_PAY_AD.SelectedValue + "'").First()["Count"].ToString();
            change();
        }

        protected void txt_amonth_TextChanged(object sender, EventArgs e)
        {
            change();
        }

        void change()
        {
            string sql = string.Format("select * from B_MONTHPAY_MAIN where pay_ad='{0}' and amonth='{1}'", strPAY_AD, strAMONTH);
            DataTable temp = new DataTable();
            temp = o_DBFactory.ABC_toTest.Create_Table(sql, "GET");
            if (temp.Rows.Count > 0)
            {
                CommonCS.ShowMessage(lb_Counts, string.Format("{0} 月份，已有資料，若要覆蓋，請按產生", txt_amonth.Text));
            }
        }

    }
}
