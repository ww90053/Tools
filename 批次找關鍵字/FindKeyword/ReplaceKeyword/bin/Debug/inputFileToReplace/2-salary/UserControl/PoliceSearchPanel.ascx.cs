using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TPPDDB._2_salary.UserControl
{
    public partial class PoliceSearchPanel : System.Web.UI.UserControl
    {
        public delegate void Postbacked();
        public event Postbacked postbacked;

        public delegate void SearchClicked();
        public event SearchClicked searchClicked;

        public DropDownList adControl
        {
            get { return ddl_ad; }
        }

        public string ad
        {
            get { return ddl_ad.SelectedValue; }
        }

        public string unit
        {
            get { return ddl_unit.SelectedValue; }
        }

        public string polno
        {
            get { return txt_polno.Text; }
        }

        public string idcard
        {
            get { return txt_idcard.Text; }
        }

        public string name
        {
            get { return txt_name.Text; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SalaryPublic.fillDropDownList(ref ddl_ad);
                SalaryPublic.AddFirstSelection(ref ddl_ad);
                ddl_ad.SelectedValue = SalaryPublic.strLoginEXAD;
                SalaryPublic.fillUnitDropDownList(ref ddl_unit, ddl_ad.SelectedValue);
            }
        }

        protected void ddl_ad_SelectedIndexChanged(object sender, EventArgs e)
        {
            SalaryPublic.fillUnitDropDownList(ref ddl_unit, ddl_ad.SelectedValue);

            if (postbacked != null)
                postbacked();
        }

        protected void btn_search_Click(object sender, EventArgs e)
        {
            if (searchClicked != null)
                searchClicked();
        }
    }
}