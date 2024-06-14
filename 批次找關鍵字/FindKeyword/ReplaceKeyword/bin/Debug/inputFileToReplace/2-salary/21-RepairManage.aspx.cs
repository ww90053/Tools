using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

namespace TPPDDB._2_salary
{
    public partial class _1_RepairManage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SalaryPublic.fillDropDownList(ref ddl_payad);
            }
        }

        protected void btn_search_Click(object sender, EventArgs e)
        {
            string sql;
            List<SqlParameter> ops = new List<SqlParameter>();

            sql = "SELECT R_SNID, AMONTH, BATCH_NUMBER, MZ_POLNO, MZ_NAME FROM B_REPAIRPAY WHERE PAY_AD=@PAY_AD";
            ops.Add(new SqlParameter("PAY_AD", ddl_payad.SelectedValue));

            if (txt_amonth.Text.Length > 0)
            {
                sql += " AND AMONTH=@AMONTH";
                ops.Add(new SqlParameter("AMONTH", txt_amonth.Text));
            }
            if (txt_batch.Text.Length > 0)
            {
                sql += " AND BATCH_NUMBER=@BATCH_NUMBER";
                ops.Add(new SqlParameter("BATCH_NUMBER", txt_batch.Text));
            }
            if (txt_polno.Text.Length > 0)
            {
                sql += " AND MZ_POLNO LIKE @MZ_POLNO";
                ops.Add(new SqlParameter("MZ_POLNO", "%" + txt_polno.Text + "%"));
            }
            if (txt_name.Text.Length > 0)
            {
                sql += " AND MZ_NAME LIKE @MZ_NAME";
                ops.Add(new SqlParameter("MZ_NAME", "%" + txt_name.Text + "%"));
            }

            gv_search.DataSource = o_DBFactory.ABC_toTest.DataSelect(sql, ops);
            gv_search.DataBind();
        }

        protected void gv_search_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }
    }
}
