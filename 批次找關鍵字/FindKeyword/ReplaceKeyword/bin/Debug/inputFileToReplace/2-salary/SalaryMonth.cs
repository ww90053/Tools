using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace TPPDDB._2_salary
{
    public class SalaryMonth : SalaryBasic
    {
        // 每月薪資產生-所得稅 還沒寫
        // 每月薪資產生-所得稅 還沒寫
        // 每月薪資產生-所得稅 還沒寫
        // 每月薪資產生-所得稅 還沒寫
        // 每月薪資產生-所得稅 還沒寫
        // 每月薪資產生-所得稅 還沒寫
        // 每月薪資產生-所得稅 還沒寫


       

       

        /// <summary>
        /// 讀取新增每月薪資時的主編號
        /// </summary>
        /// <param name="strMZ_ID">身分證號</param>
        /// <param name="strAmonth">產生月份</param>
        /// <returns></returns>
        public static int intMonthPay_Data_MM_SNID(string strMZ_ID, string strAmonth)
        {
            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn.Open();
                try
                {
                    string strSQL = "SELECT MM_SNID, IDCARD, AMONTH FROM B_MONTHPAY_MAIN WHERE IDCARD = '" + strMZ_ID + "' AND AMONTH = '" + strAmonth + "' ";
                    SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
                    DataTable dt = new DataTable();
                    dt.Load(Selectcmd.ExecuteReader(CommandBehavior.CloseConnection));
                    if (dt.Rows.Count == 1)
                    {
                        return int.Parse(dt.Rows[0]["MM_SNID"].ToString());
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch { return 0; }
                finally { Selectconn.Close();
                //XX2013/06/18 
                Selectconn.Dispose();
                }
            }
        }

        

     

        

        

    }
}
