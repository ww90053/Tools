using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TPPDDB._2_salary
{
    public partial class _1_B_UnitCheckPage : System.Web.UI.Page
    {
        DataTable dtBase
        {
            get { return (DataTable)ViewState["dtBase"]; }
            set { ViewState["dtBase"] = value; }
        }
        DataTable dtAccount
        {
            get { return (DataTable)ViewState["dtAccount"]; }
            set { ViewState["dtAccount"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
           

            if (!IsPostBack)
            { SalaryPublic.checkPermission();
                SalaryPublic.fillDropDownList(ref ddl_AD);
                SalaryPublic.AddFirstSelection(ref ddl_AD);
            }
        }

        protected void ddl_AD_SelectedIndexChanged(object sender, EventArgs e)
        {
            DoSearch();
        }

        protected void gv_base_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_base.PageIndex = e.NewPageIndex;
            gv_base.DataSource = dtBase;
            gv_base.DataBind();
        }

        protected void gv_Account_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_Account.PageIndex = e.NewPageIndex;
            gv_Account.DataSource = dtAccount;
            gv_Account.DataBind();
        }

        private void DoSearch()
        {
            DataTable dt = new DataTable();
            string strSQL;

            strSQL = string.Format("SELECT * FROM A_DLBASE WHERE MZ_ID NOT IN ( SELECT IDCARD FROM B_BASE) AND PAY_AD='{0}'", ddl_AD.SelectedValue);
            dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "BASE");
            dtBase = dt;
            gv_base.DataSource = dtBase;
            gv_base.DataBind();

            strSQL = string.Format("SELECT MZ_KCHI PAY_AD, MZ_ID, MZ_POLNO, MZ_NAME FROM A_DLBASE JOIN B_BASE ON MZ_ID=IDCARD JOIN A_KTYPE ON PAY_AD=MZ_KCODE WHERE PAY_AD IS NOT NULL AND MZ_ID NOT IN (SELECT MZ_ID FROM B_BASE_STOCKPILE JOIN A_DLBASE ON MZ_ID=IDCARD AND B_BASE_STOCKPILE.PAY_AD=A_DLBASE.PAY_AD AND \"GROUP\"=2) AND PAY_AD='{0}'", ddl_AD.SelectedValue);
            dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "SLVC");
            dtAccount = dt;
            gv_Account.DataSource = dtAccount;
            gv_Account.DataBind();
        }
    }
}
