using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Configuration;
using System.Data.SqlClient;

namespace TPPDDB._2_salary
{
    public partial class B_SalaryIncomeTax1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack != true)
            { 
                           SalaryPublic.checkPermission();
                Label_MSG.Text = "";

                txt_taxpercent.Text = Tax.getTaxPercent().ToString();
                txt_taxstart.Text = Tax.getTaxStart().ToString();
            }
        }

        protected void btOK_Click(object sender, EventArgs e)
        {
            string sql;
            List<SqlParameter> ps = new List<SqlParameter>();

            sql = "UPDATE B_TAX_SET SET TAX_PERCENTAGE = @PER, TAX_START = @STT";

            ps.Add(new SqlParameter("PER", txt_taxpercent.Text));
            ps.Add(new SqlParameter("STT", txt_taxstart.Text));

            try
            {
                o_DBFactory.ABC_toTest.Edit_Data(sql, ps);
                Label_MSG.Text = "設定完成";
                Label_MSG.ForeColor = System.Drawing.Color.Blue;
            }
            catch
            {
                Label_MSG.Text = "設定失敗";
                Label_MSG.ForeColor = System.Drawing.Color.Red;
            }
        }
    }
}
