using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Web.Configuration;

namespace TPPDDB._2_salary
{
    public partial class B_InsuranceStatistics_rpt_2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {            
            if (!IsPostBack)
            {                
                SalaryPublic.checkPermission();
                TextBox_AMONTH.Text = (System.DateTime.Now.Year - 1911).ToString().PadLeft(3, '0') + DateTime.Now.Month.ToString().PadLeft(2, '0');;
                SalaryPublic.fillDropDownList(ref this.DropDownList_PAY_AD);
            }
        }

        protected void Button_Send_Click(object sender, EventArgs e)
        {
            string strPAY_AD = String.Empty;
            string strDATE = String.Empty;

            strPAY_AD = DropDownList_PAY_AD.SelectedValue.ToString();

            Label_MSG.Text = "";
            if (TextBox_AMONTH.Text.Length == 0)
            {
                Label_MSG.Text = "請輸入日期(年月)";
                Label_MSG.ForeColor = System.Drawing.Color.Red;
                return;
            }
            if (TextBox_AMONTH.Text.Length != 5)
            {
                Label_MSG.Text = "請輸入正確5碼年月格式(例:09811)";
                Label_MSG.ForeColor = System.Drawing.Color.Red;
                return;
            }
            strDATE = TextBox_AMONTH.Text.ToString();

            string tmp_url = String.Empty;
            if (boolRequest("TPM_FION"))
            {
                tmp_url = "B_rpt.aspx?fn=insuranceStat_2&TPM_FION=" + Request.QueryString["TPM_FION"].ToString() + "&AD=" + strPAY_AD + "&DATE=" + strDATE;
            }
            else
            {
                tmp_url = "B_rpt.aspx?fn=insuranceStat_2";
            }

          
            ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
        }

        #region 20140707 好像沒在用
        ////送至報表之DataTable
        //private static DataTable dt_TO_rpt = new DataTable();

       
        //初始化送至報表的DataTable欄位
        //private void initRPTDT()
        //{
        //    dt_TO_rpt.Clear();
        //    dt_TO_rpt.Columns.Clear();

        //    dt_TO_rpt.Columns.Add("MZ_UNIT", typeof(String));
        //    dt_TO_rpt.Columns.Add("MZ_ID", typeof(String));
        //    dt_TO_rpt.Columns.Add("MZ_NAME", typeof(String));
        //    dt_TO_rpt.Columns.Add("IPAY", typeof(int));
        //    dt_TO_rpt.Columns.Add("ISUM", typeof(int));
        //    dt_TO_rpt.Columns.Add("HCOUNT", typeof(int));
        //    dt_TO_rpt.Columns.Add("HPAY", typeof(int));
        //    dt_TO_rpt.Columns.Add("HSUM", typeof(int));
        //    dt_TO_rpt.Columns.Add("IREFUND", typeof(int));
        //    dt_TO_rpt.Columns.Add("HREFUND", typeof(int));
        //}


        
      
        ////公(勞)保費總計-依年度從1月到輸入月份
        //private int intInsurancePaySum(string strMZ_ID, string strDATE)
        //{
        //    using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
        //    {
        //        try
        //        {
        //            Selectconn.Open();
        //            string strSQL = string.Format("SELECT SUM(INSURANCEPAY) FROM B_MONTHPAY_MAIN WHERE IDCARD='{0}' AND AMONTH<='{1}' AND AMONTH>='{2}'", strMZ_ID, strDATE, strDATE.Substring(0, 3) + "01");
        //            SqlCommand cmd = new SqlCommand(strSQL,Selectconn);
        //            DataTable dt = new DataTable();
        //            dt.Load(cmd.ExecuteReader());
        //            if (dt.Rows.Count != 1)
        //            {
        //                return 0;
        //            }
        //            return Convert.ToInt32(dt.Rows[0]["INSURANCEPAY"]);
        //        }
        //        catch { return 0; }
        //        finally { Selectconn.Close();
        //        //XX2013/06/18 
        //        Selectconn.Dispose();
        //        }
        //    }
        //}

        ////健保費總計-依年度從1月到輸入月份
        //private int intHealthPaySum(string strMZ_ID, string strDATE)
        //{
        //    using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
        //    {
        //        try
        //        {
        //            Selectconn.Open();
        //            string strSQL = string.Format("SELECT SUM(HEALTHPAY) FROM B_MONTHPAY_MAIN WHERE IDCARD='{0}' AND AMONTH<='{1}' AND AMONTH>='{2}'", strMZ_ID, strDATE, strDATE.Substring(0, 3) + "01");
        //            SqlCommand cmd = new SqlCommand(strSQL, Selectconn);
        //            DataTable dt = new DataTable();
        //            dt.Load(cmd.ExecuteReader());
        //            if (dt.Rows.Count != 1)
        //            {
        //                return 0;
        //            }
        //            return Convert.ToInt32(dt.Rows[0]["INSURANCEPAY"]);
        //        }
        //        catch { return 0; }
        //        finally { Selectconn.Close();
        //        //XX2013/06/18 
        //        Selectconn.Dispose();
        //        }
        //    }
        //}

        ////退公(勞)保費-來自單一發放-入帳(依年度從1月1日到輸入月份最後一日)
        //private int intInsuranceRefundSum(string strMZ_ID, string strDATE)
        //{
        //    using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
        //    {
        //        try
        //        {
        //            Selectconn.Open();
        //            string strSQL = string.Format("SELECT SUM(PAY2) FROM B_SOLE WHERE IDCARD='{0}' AND AMONTH<='{1}' AND AMONTH>='{2}' AND DA_INOUT_GROUP='IN'", strMZ_ID, strDATE + "31", strDATE.Substring(0, 3) + "0101");
        //            SqlCommand cmd = new SqlCommand(strSQL, Selectconn);
        //            DataTable dt = new DataTable();
        //            dt.Load(cmd.ExecuteReader());
        //            if (dt.Rows.Count != 1)
        //            {
        //                return 0;
        //            }
        //            return Convert.ToInt32(dt.Rows[0]["PAY2"]);
        //        }
        //        catch { return 0; }
        //        finally { Selectconn.Close();
        //        //XX2013/06/18 
        //        Selectconn.Dispose();
        //        }
        //    }
        //}

        ////退健保費-來自單一發放-入帳(依年度從1月1日到輸入月份最後一日)
        //private int intHealthRefundSum(string strMZ_ID, string strDATE)
        //{
        //    using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
        //    {
        //        try
        //        {
        //            Selectconn.Open();
        //            string strSQL = string.Format("SELECT SUM(PAY1) FROM B_MONTHPAY_MAIN WHERE IDCARD='{0}' AND AMONTH<='{1}' AND AMONTH>='{2}' AND DA_INOUT_GROUP='IN'", strMZ_ID, strDATE + "31", strDATE.Substring(0, 3) + "0101");
        //            SqlCommand cmd = new SqlCommand(strSQL, Selectconn);
        //            DataTable dt = new DataTable();
        //            dt.Load(cmd.ExecuteReader());
        //            if (dt.Rows.Count != 1)
        //            {
        //                return 0;
        //            }
        //            return Convert.ToInt32(dt.Rows[0]["PAY1"]);
        //        }
        //        catch { return 0; }
        //        finally { Selectconn.Close();
        //        //XX2013/06/18 
        //        Selectconn.Dispose();
        //        }
        //    }
        //}

        #endregion 好像沒在用

        private bool boolRequest(string strRequest)
        {
            try
            {
                Request.QueryString[strRequest].ToString();
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
