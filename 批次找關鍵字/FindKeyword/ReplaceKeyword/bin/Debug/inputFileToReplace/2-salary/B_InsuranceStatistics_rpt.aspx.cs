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
    public partial class 已經廢止_B_InsuranceStatistics_rpt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (!IsPostBack)
            { 
                SalaryPublic.checkPermission();  
                TextBox_AMONTH.Text = (System.DateTime.Now.Year - 1911).ToString().PadLeft(3, '0') + DateTime.Now.Month.ToString().PadLeft(2, '0');
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

            initRPTDT();

            voidNAME1_3();
            voidNAME2();

            string strRANK = String.Empty;

            string strSQL = String.Format("SELECT MZ_SPT,dbo.SUBSTR(MZ_SRANK,0,1) MZ_SRANK FROM B_MONTHPAY_MAIN WHERE MZ_SRANK IS NOT NULL AND PAY_AD='{0}' AND AMONTH='{1}'", strPAY_AD, strDATE);
            DataTable dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "SRANK");
            foreach (DataRow drSRANK in dt.Rows)
            {
                strRANK = drSRANK["MZ_SRANK"].ToString();
                switch (strRANK)
                {
                    //員警-對應俸點(元)
                    case "G":
                    case "P":
                        for (int i = 0; i < dt1.Rows.Count; i++)
                        {
                            if (dt1.Rows[i]["ORIGIN2"].Equals(drSRANK["MZ_SPT"]))
                            {
                                if (dt_TO_rpt.Rows[i]["ICOUNT"].ToString() == "")
                                {
                                    dt_TO_rpt.Rows[i]["ICOUNT"] = 1;
                                    dt_TO_rpt.Rows[i]["HCOUNT"] = 1;
                                    dt_TO_rpt.Rows[i]["CCOUNT"] = 1;
                                }
                                else
                                {
                                    dt_TO_rpt.Rows[i]["ICOUNT"] = Convert.ToInt32(dt_TO_rpt.Rows[i]["ICOUNT"]) + 1;
                                    dt_TO_rpt.Rows[i]["HCOUNT"] = Convert.ToInt32(dt_TO_rpt.Rows[i]["HCOUNT"]) + 1;
                                    dt_TO_rpt.Rows[i]["CCOUNT"] = Convert.ToInt32(dt_TO_rpt.Rows[i]["CCOUNT"]) + 1;
                                }
                                break;
                            }
                        }
                        break;
                    //雇員-對應俸點(點)
                    case "M":
                    case "J":
                        for (int i = 0; i < dt2.Rows.Count; i++)
                        {
                            if (dt2.Rows[i]["ORIGIN1"].Equals(drSRANK["MZ_SPT"]))
                            {
                                if (dt_TO_rpt.Rows[i + 50]["ICOUNT"].ToString() == "")
                                {
                                    dt_TO_rpt.Rows[i + 50]["ICOUNT"] = 1;
                                    dt_TO_rpt.Rows[i + 50]["HCOUNT"] = 1;
                                    dt_TO_rpt.Rows[i + 50]["CCOUNT"] = 1;
                                }
                                else
                                {
                                    dt_TO_rpt.Rows[i + 50]["ICOUNT"] = Convert.ToInt32(dt_TO_rpt.Rows[i + 50]["ICOUNT"]) + 1;
                                    dt_TO_rpt.Rows[i + 50]["HCOUNT"] = Convert.ToInt32(dt_TO_rpt.Rows[i + 50]["HCOUNT"]) + 1;
                                    dt_TO_rpt.Rows[i + 50]["CCOUNT"] = Convert.ToInt32(dt_TO_rpt.Rows[i + 50]["CCOUNT"]) + 1;
                                }
                                break;
                            }
                        }
                        break;
                    //技工等-對應俸點(點)
                    case "B":
                        for (int i = 0; i < dt1.Rows.Count; i++)
                        {
                            if (dt1.Rows[i]["ORIGIN1"].Equals(drSRANK["MZ_SPT"]))
                            {
                                if (dt_TO_rpt.Rows[i]["ICOUNT"].ToString() == "")
                                {
                                    dt_TO_rpt.Rows[i]["ICOUNT"] = 1;
                                    dt_TO_rpt.Rows[i]["HCOUNT"] = 1;
                                    dt_TO_rpt.Rows[i]["CCOUNT"] = 1;
                                }
                                else
                                {
                                    dt_TO_rpt.Rows[i]["ICOUNT"] = Convert.ToInt32(dt_TO_rpt.Rows[i]["ICOUNT"]) + 1;
                                    dt_TO_rpt.Rows[i]["HCOUNT"] = Convert.ToInt32(dt_TO_rpt.Rows[i]["HCOUNT"]) + 1;
                                    dt_TO_rpt.Rows[i]["CCOUNT"] = Convert.ToInt32(dt_TO_rpt.Rows[i]["CCOUNT"]) + 1;
                                }
                                break;
                            }
                        }
                        break;
                    default:
                        break;
                }
            }

            strSQL = "SELECT B_SALARY.PAY1, INSURANCE FROM B_SALARY JOIN B_HEALTH_INSURANCE ON B_SALARY.PAY1>=B_HEALTH_INSURANCE.PAY1 AND B_SALARY.PAY1<=B_HEALTH_INSURANCE.PAY2 ORDER BY B_SALARY.PAY1 DESC";
            DataTable dtHealth = o_DBFactory.ABC_toTest.Create_Table(strSQL, "HPAY");

            //將費用及費用合計資料填入準備送至報表之DataTable
            for (int i = 0; i < dt_TO_rpt.Rows.Count; i++)
            {
                foreach (DataRow dr in dtHealth.Rows)
                {
                    if (Convert.ToInt32(dt_TO_rpt.Rows[i]["PAY1"]).Equals(Convert.ToInt32(dr["PAY1"])))
                    {
                        dt_TO_rpt.Rows[i]["HPAY"] = dr["INSURANCE"];
                    }
                }
                if (dt_TO_rpt.Rows[i]["ICOUNT"].ToString() == "")
                    dt_TO_rpt.Rows[i]["ICOUNT"] = 0;
                if (dt_TO_rpt.Rows[i]["HCOUNT"].ToString() == "")
                    dt_TO_rpt.Rows[i]["HCOUNT"] = 0;
                if (dt_TO_rpt.Rows[i]["CCOUNT"].ToString() == "")
                    dt_TO_rpt.Rows[i]["CCOUNT"] = 0;
                if (dt_TO_rpt.Rows[i]["IPAY"].ToString() == "")
                    dt_TO_rpt.Rows[i]["IPAY"] = 0;
                if (dt_TO_rpt.Rows[i]["HPAY"].ToString() == "")
                    dt_TO_rpt.Rows[i]["HPAY"] = 0;
                if (dt_TO_rpt.Rows[i]["CPAY"].ToString() == "")
                    dt_TO_rpt.Rows[i]["CPAY"] = 0;
                dt_TO_rpt.Rows[i]["ISUM"] = Convert.ToInt32(dt_TO_rpt.Rows[i]["ICOUNT"]) * Convert.ToInt32(dt_TO_rpt.Rows[i]["IPAY"]);
                dt_TO_rpt.Rows[i]["HSUM"] = Convert.ToInt32(dt_TO_rpt.Rows[i]["HCOUNT"]) * Convert.ToInt32(dt_TO_rpt.Rows[i]["HPAY"]);
                dt_TO_rpt.Rows[i]["CSUM"] = Convert.ToInt32(dt_TO_rpt.Rows[i]["CCOUNT"]) * Convert.ToInt32(dt_TO_rpt.Rows[i]["CPAY"]);
            }
            string tmp_url = String.Empty;
            if (boolRequest("TPM_FION"))
            {
                tmp_url = "B_rpt.aspx?fn=insuranceStat&TPM_FION=" + Request.QueryString["TPM_FION"].ToString();
            }
            else
            {
                tmp_url = "B_rpt.aspx?fn=insuranceStat";
            }

            Session["PAY_AD"] = o_A_KTYPE.CODE_TO_NAME(strPAY_AD,"04");
            Session["YEAR"] = strDATE.Substring(0, 3);
            Session["MONTH"] = strDATE.Substring(3, 2);
            Session["rpt_dt"] = dt_TO_rpt;
            ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
        }

        //員警及雇員之級距表
        private static DataTable dt1 = new DataTable();
        //技工駕駛工友之級距表
        private static DataTable dt2 = new DataTable();

        //送至報表之DataTable
        private static DataTable dt_TO_rpt = new DataTable();

        //初始化送至報表的DataTable欄位
        private void initRPTDT()
        {
            dt_TO_rpt.Clear();
            dt_TO_rpt.Columns.Clear();

            dt_TO_rpt.Columns.Add("ORIGIN1", typeof(String));
            dt_TO_rpt.Columns.Add("ORIGIN2", typeof(String));
            dt_TO_rpt.Columns.Add("PAY1", typeof(int));
            dt_TO_rpt.Columns.Add("ICOUNT", typeof(int));
            dt_TO_rpt.Columns.Add("IPAY", typeof(int));
            dt_TO_rpt.Columns.Add("ISUM", typeof(int));
            dt_TO_rpt.Columns.Add("HCOUNT", typeof(int));
            dt_TO_rpt.Columns.Add("HPAY", typeof(int));
            dt_TO_rpt.Columns.Add("HSUM", typeof(int));
            dt_TO_rpt.Columns.Add("CCOUNT", typeof(int));
            dt_TO_rpt.Columns.Add("CPAY", typeof(int));
            dt_TO_rpt.Columns.Add("CSUM", typeof(int));
        }

        private void voidNAME2()
        {
            dt2.Clear();
            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn.Open();
                try
                {
                    string strSQL = "SELECT ORIGIN1, ORIGIN2, PAY1, GOV_INSURANCE, CONCUR3 FROM B_SALARY WHERE NAME1 = '2' ORDER BY ORIGIN1 DESC";
                    SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
                    dt2.Clear();
                    dt2.Load(Selectcmd.ExecuteReader());
                    foreach (DataRow item in dt2.Rows)
                    {
                        DataRow dr_new = dt_TO_rpt.NewRow();

                        dr_new["ORIGIN1"] = item["ORIGIN1"].ToString();
                        dr_new["ORIGIN2"] = item["ORIGIN2"].ToString();
                        dr_new["PAY1"] = Convert.ToInt32(item["PAY1"].ToString());
                        dr_new["IPAY"] = Convert.ToInt32(item["GOV_INSURANCE"].ToString());
                        dr_new["CPAY"] = Convert.ToInt32(item["CONCUR3"].ToString());

                        dt_TO_rpt.Rows.Add(dr_new);
                    }
                }
                catch (Exception) { throw; }
                finally { Selectconn.Close();
                //XX2013/06/18 
                Selectconn.Dispose();
                }
            }
        }


        private void voidNAME1_3()
        {
            dt1.Clear();
            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn.Open();
                try
                {
                    string strSQL = "SELECT ORIGIN1, ORIGIN2, PAY1, GOV_INSURANCE, CONCUR3 FROM B_SALARY WHERE NAME1 ='1' OR (NAME1 = '3' AND ORIGIN1 < '0160') ORDER BY ORIGIN1 DESC";
                    SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
                    dt1.Load(Selectcmd.ExecuteReader());
                    foreach (DataRow item in dt1.Rows)
                    {
                        DataRow dr_new = dt_TO_rpt.NewRow();

                        dr_new["ORIGIN1"] = item["ORIGIN1"].ToString();
                        dr_new["ORIGIN2"] = item["ORIGIN2"].ToString();
                        dr_new["PAY1"] = Convert.ToInt32(item["PAY1"].ToString());
                        dr_new["IPAY"] = Convert.ToInt32(item["GOV_INSURANCE"].ToString());
                        dr_new["CPAY"] = Convert.ToInt32(item["CONCUR3"].ToString());

                        dt_TO_rpt.Rows.Add(dr_new);
                    }

                }
                catch (Exception) { throw; }
                finally { Selectconn.Close();
                //XX2013/06/18 
                Selectconn.Dispose();
                }
            }
        }

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
