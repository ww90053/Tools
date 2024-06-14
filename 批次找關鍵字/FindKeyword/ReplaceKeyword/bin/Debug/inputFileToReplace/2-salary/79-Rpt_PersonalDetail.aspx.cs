using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

namespace TPPDDB._2_salary
{
    public partial class _9_Rpt_PersonalDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SalaryPublic.checkPermission();
                txt_idcard.Text = SalaryPublic.strLoginID;
            }

            // 一般員警只能查詢自己的資料
            if (SalaryPublic.GetGroupPermission() == "")
                txt_idcard.Enabled = false;
            else
                txt_idcard.Enabled = true;
        }

        protected void btn_report_Click(object sender, EventArgs e)
        {


            Session["RPT_A_IDNO"] = txt_idcard.Text;

            string tmp_url = "B_rpt.aspx?fn=79&YEAR=" + txt_year.Text ;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
        }
    }
}
