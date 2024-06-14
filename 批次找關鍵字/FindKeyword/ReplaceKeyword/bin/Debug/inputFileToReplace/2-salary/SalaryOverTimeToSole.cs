using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Configuration;
using System.Data.SqlClient;

namespace TPPDDB._2_salary
{
    public class SalaryOverTimeToSole : SalarySole
    {

        /// <summary>
        /// 產生超勤檔至單一發放
        /// </summary>
        /// <param name="strPAY_AD">發薪機關</param>
        /// <param name="strYEAR">產生年份</param>
        /// <param name="strMONTH">產生月份</param>
        /// <returns></returns>
        public static bool boolSalaryOverTimeToSole_Create(string strPAY_AD, string strYEAR, string strMONTH)
        {
            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn.Open();
                try
                {
                    string strSQL = "SELECT C_OH.MZ_OVERTIME_PAY AS MZ_OVERTIME_PAY, C_OH.MZ_ID AS MZ_ID, C_OH.MZ_YEAR AS MZ_YEAR, C_OH.MZ_MONTH AS MZ_MONTH, A_DB.MZ_POLNO AS MZ_POLNO, A_DB.MZ_NAME AS MZ_NAME, A_DB.MZ_OCCC AS MZ_OCCC, A_DB.MZ_SRANK AS MZ_SRANK, A_DB.MZ_SLVC AS MZ_SLVC, A_DB.MZ_SPT AS MZ_SPT, A_DB.MZ_UNIT AS MZ_UNIT "
                    + " FROM C_DUTYMONTHOVERTIME_HOUR C_OH, A_DLBASE A_DB WHERE C_OH.MZ_ID = A_DB.MZ_ID AND A_DB.PAY_AD = '" + strPAY_AD + "' AND C_OH.MZ_YEAR = '" + strYEAR + "' AND C_OH.MZ_MONTH = '" + strMONTH + "'";
                    SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
                    DataTable dt = new DataTable();
                    dt.Load(Selectcmd.ExecuteReader(CommandBehavior.CloseConnection));
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow item in dt.Rows)
                        {
                            int intMZ_OVERTIME_PAY = int.Parse(item["MZ_OVERTIME_PAY"].ToString());
                            string strMZ_ID = item["MZ_ID"].ToString();
                            string strMZ_YEAR = item["MZ_YEAR"].ToString();
                            string strMZ_MONTH = item["MZ_MONTH"].ToString();
                            string strMZ_POLNO = item["MZ_POLNO"].ToString();
                            string strMZ_NAME = item["MZ_NAME"].ToString();
                            string strMZ_OCCC = item["MZ_OCCC"].ToString();
                            string strMZ_SRANK = item["MZ_SRANK"].ToString();
                            string strMZ_SLVC = item["MZ_SLVC"].ToString();
                            string strMZ_SPT = item["MZ_SPT"].ToString();
                            string strMZ_UNIT = item["MZ_UNIT"].ToString();

                            //if (boolSole_Create(strMZ_ID, strMZ_POLNO, strPAY_AD, strMZ_NAME, strMZ_OCCC, strMZ_SRANK, strMZ_SLVC, strMZ_SPT, strMZ_UNIT, "N", "IN", strYEAR + "/" + strMONTH + "/" + DateTime.Now.Day, "A1", "", intMZ_OVERTIME_PAY, "N", 0, 0, 0, 0, strYEAR + strMONTH + "超勤") == false)
                            //{
                            //    return false;
                            //}
                        }
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch { return false; }
                finally { Selectconn.Close();
                //XX2013/06/18 
                Selectconn.Dispose();
                }
            }
        }

        /// <summary>
        /// 產生超勤檔至單一發放 - 刪除
        /// </summary>
        /// <param name="strPAY_AD">發薪機關</param>
        /// <param name="strYEAR">產生年份</param>
        /// <param name="strMONTH">產生月份</param>
        /// <returns></returns>
        public static bool boolSalaryOverTimeToSole_Delete(string strPAY_AD, string strYEAR, string strMONTH)
        {
            using (SqlConnection DeleteConn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                DeleteConn.Open();

                string strDAY = DateTime.DaysInMonth(int.Parse(strYEAR) + 1911, int.Parse(strMONTH)).ToString();
                string DeleteString = "DELETE B_SOLE WHERE LOCKDB = 'N' AND CASEID = 'A1' AND DA BETWEEN dbo.TO_DATE('" + (int.Parse(strYEAR) + 1911) + "-" + strMONTH + "-01', 'YYYY-MM-DD') AND dbo.TO_DATE('" + (int.Parse(strYEAR) + 1911) + "-" + strMONTH + "-" + strDAY + "', 'YYYY-MM-DD')";
                SqlCommand cmd = new SqlCommand(DeleteString, DeleteConn);
                try
                {
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch { return false; }
                finally { DeleteConn.Close();
                //XX2013/06/18 
                DeleteConn.Dispose();
                }
            }
        }

        /// <summary>
        /// 產生超勤檔至單一發放 - 鎖定資料庫
        /// </summary>
        /// <param name="strPAY_AD">發薪機關</param>
        /// <param name="strYEAR">產生年份</param>
        /// <param name="strMONTH">產生月份</param>
        /// <returns></returns>
        public static bool boolSalaryOverTimeToSole_LOCKDB(string strPAY_AD, string strYEAR, string strMONTH)
        {
            using (SqlConnection UpdateConn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                UpdateConn.Open();
                try
                {
                    string UpdateString = "UPDATE B_SOLE SET LOCKDB = 'Y' WHERE CASEID = 'A1' AND DA < dbo.TO_DATE('" + (int.Parse(strYEAR) + 1911).ToString("0000") + "-" + strMONTH + "-01', 'YYYY-MM-DD') AND DA >= dbo.TO_DATE('" + SalaryPublic.strLOCKDB_YEAR(strYEAR, strMONTH) + "-" + SalaryPublic.strLOCKDB_MONTH(strMONTH) + "-01', 'YYYY-MM-DD')";
                    SqlCommand cmd = new SqlCommand(UpdateString, UpdateConn);
                    cmd.CommandType = CommandType.Text;

                    cmd.Connection = UpdateConn;
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch { return false; }
                finally { UpdateConn.Close();
                //XX2013/06/18 
                UpdateConn.Dispose();
                }
            }
        }


        
    }
}
