using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Drawing;

namespace TPPDDB._2_salary
{
    public partial class B_SalaryBasic4 : System.Web.UI.Page
    {
        //保費其實還有一個 INSURANCE_D,是用於舊制的保費設定,此功能無法設定
        //到職日在112.07.01以後的人員採新制
        string sql = "SELECT CASE NAME1 WHEN 1 THEN '公務人員、警察' WHEN 2 THEN '司機、工友、技工' END NAME1, ORIGIN1, ORIGIN2 , PAY1, PAY2, PAY3, INSURANCE_F, CONCUR3, GOVERNMENT FROM B_SALARY ORDER BY name1, origin1";

        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                SalaryPublic.checkPermission();
                DoSearch();
            }
        }

        private void DoSearch()
        {
            GridView_SALARY.DataSource = o_DBFactory.ABC_toTest.DataSelect(sql);
            GridView_SALARY.DataBind();
        }

        protected void btn_Export_Click(object sender, EventArgs e)
        {
            DataTable dt = o_DBFactory.ABC_toTest.DataSelect(sql);

            dt.Columns["NAME1"].ColumnName = "類別";
            dt.Columns["ORIGIN1"].ColumnName = "俸點";
            dt.Columns["ORIGIN2"].ColumnName = "俸級";
            dt.Columns["PAY1"].ColumnName = "薪俸";
            dt.Columns["PAY2"].ColumnName = "福利互助金";
            dt.Columns["PAY3"].ColumnName = "警察共濟金";
            dt.Columns["INSURANCE_F"].ColumnName = "公保費";
            dt.Columns["CONCUR3"].ColumnName = "退撫基金(自繳)";
            dt.Columns["GOVERNMENT"].ColumnName = "退撫基金(政府撥繳";

            App_Code.ToExcel.Dt2Excel(dt, "薪俸級距表");
        }
    }
}
