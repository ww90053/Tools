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
    public partial class B_yearendbonuslist_rpt : System.Web.UI.Page
    {
        
        DataTable temp = new DataTable();
        DataTable rpt_dt;
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                SalaryPublic.checkPermission();
                SalaryPublic.fillDropDownList(ref this.DropDownList_AD);
                TextBox_YEAR.Text = SalaryPublic.strRepublicLastYear();
            }
        }

        private String strAD
        {
            get
            {
                return DropDownList_AD.SelectedValue;
            }
        }

        private string strAYEAR
        {
            get
            {
                return TextBox_YEAR.Text;
            }
        }

        protected void Button_SEARCH_Click(object sender, EventArgs e)
        {
            string strSQL = string.Format("SELECT * FROM VW_YEAREND_BONUS_LIST WHERE PAY_AD = '{0}' AND AYEAR = '{1}' ORDER BY UNITCODE", strAD, strAYEAR);

            Session["rpt_dt"] = o_DBFactory.ABC_toTest.Create_Table(strSQL, "VW");
            Session["TITLE"] = o_A_KTYPE.CODE_TO_NAME(strAD, "04") + strAYEAR + "年";
            Session["TITLE1"] = "發給年終工作獎金1.5月";
            string tmp_url = "B_rpt.aspx?fn=yearpay_list";
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
        }

        protected DataTable rpt_dt_init()
        {
            rpt_dt = new DataTable();
            rpt_dt.Columns.Add("UNIT", typeof(string));
            rpt_dt.Columns.Add("PEOPLE_ACCOUNT", typeof(int));
            rpt_dt.Columns.Add("SALARYPAY1", typeof(int));
            rpt_dt.Columns.Add("BOSS", typeof(int));
            rpt_dt.Columns.Add("PROFESS", typeof(int));
            rpt_dt.Columns.Add("SSUM", typeof(int));
            rpt_dt.Columns.Add("TAX", typeof(int));
            rpt_dt.Columns.Add("EXTRA01", typeof(int));
            rpt_dt.Columns.Add("TOTAL", typeof(int));
            return rpt_dt;
        }

        protected void Button_CANCEL_Click(object sender, EventArgs e)
        {
            Response.Redirect("B_yearendbonuslist_rpt.aspx?TPM_FION=" + Request.QueryString["TPM_FION"]);
        }
    }
}
