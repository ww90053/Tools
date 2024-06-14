using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TPPDDB._1_personnel
{
    public partial class Personal1_1_Salary : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //檢查權限                          
                A.check_power();

                ShowData();
            }
        }

        private void ShowData()
        {
            string sql = @"SELECT professpay,
                                       workppay,
                                       electricpay,
                                       bosspay
                                FROM   b_base
                                WHERE  idcard = '" + Session["Search_ID"].ToString() + "' ";

            DataTable dt = o_DBFactory.ABC_toTest.Create_Table(sql, "get");
            if (dt != null && dt.Rows.Count > 0)
            {
                this.txtPro.Text = dt.Rows[0]["professpay"].ToString();
                this.txtWork.Text = dt.Rows[0]["workppay"].ToString();
                this.txtElectric.Text = dt.Rows[0]["electricpay"].ToString();
                this.txtBoss.Text = dt.Rows[0]["bosspay"].ToString();
            }
        }

        protected void btUpdate_Click(object sender, EventArgs e)
        {
            string sql = @"UPDATE b_base
                            SET    professpay = @professpay,
                                   workppay = @workppay,
                                   electricpay = @electricpay,
                                   bosspay = @bosspay
                            WHERE  idcard = @idcard";

            List<SqlParameter> paras = new List<SqlParameter>();

            int professpay = 0;
            int workppay = 0;
            int electricpay = 0;
            int bosspay = 0;
            string idcard = Session["Search_ID"].ToString();

            int.TryParse(txtPro.Text.Trim(), out professpay);
            int.TryParse(txtWork.Text.Trim(), out workppay);
            int.TryParse(txtElectric.Text.Trim(), out electricpay);
            int.TryParse(txtBoss.Text.Trim(), out bosspay);

            paras.Add(new SqlParameter("professpay", SqlDbType.Int) { Value = professpay });;
            paras.Add(new SqlParameter("workppay", SqlDbType.Int) { Value = workppay });;
            paras.Add(new SqlParameter("electricpay", SqlDbType.Int) { Value = electricpay });;
            paras.Add(new SqlParameter("bosspay", SqlDbType.Int) { Value = bosspay });;
            paras.Add(new SqlParameter("idcard", SqlDbType.VarChar) { Value = idcard });;

            o_DBFactory.ABC_toTest.Create_Table(sql, paras);
            ShowData();

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('修改完成！');", true);

        }


    }
}