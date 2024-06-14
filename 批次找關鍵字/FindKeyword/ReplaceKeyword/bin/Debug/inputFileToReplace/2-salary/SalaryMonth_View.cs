using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace TPPDDB._2_salary
{
    public class SalaryMonth_View : SalaryMonth
    {
            

        /// <summary>
        /// 查詢每月薪資 - 應發金額總計
        /// </summary>
        /// <param name="strAMONTH">查詢月份</param>
        /// <param name="strIDCARD">查詢人員身分證號</param>
        /// <returns></returns>
        public static int intSearch_SalaryMonth_Data_Increase(string strAMONTH, string strIDCARD)
        {
            int Salarypay1 = intSearch_SalaryMonth_Data_Salarypay1(strAMONTH, strIDCARD);
            int intBoss = intSearch_SalaryMonth_Data_Boss(strAMONTH, strIDCARD);
            int intProfess = intSearch_SalaryMonth_Data_Profess(strAMONTH, strIDCARD);
            int intWorkp = intSearch_SalaryMonth_Data_Workp(strAMONTH, strIDCARD);
            int intTechnics = intSearch_SalaryMonth_Data_Technics(strAMONTH, strIDCARD);
            int intBouns = intSearch_SalaryMonth_Data_Bonus(strAMONTH, strIDCARD);
            int intAdventive = intSearch_SalaryMonth_Data_Adventive(strAMONTH, strIDCARD);
            int intFar = intSearch_SalaryMonth_Data_Far(strAMONTH, strIDCARD);
            int intElectric = intSearch_SalaryMonth_Data_Electric(strAMONTH, strIDCARD);
            int intOther_Increase = intSearch_SalaryMonth_Data_Other_Increase(strIDCARD, strAMONTH);

            return Salarypay1 + intBoss + intProfess + intWorkp + intTechnics + intBouns + intAdventive + intFar + intElectric + intOther_Increase;
        }

        /// <summary>
        /// 查詢每月薪資 - 應扣金額總計
        /// </summary>
        /// <param name="strAMONTH">查詢月份</param>
        /// <param name="strIDCARD">查詢人員身分證號</param>
        /// <returns></returns>
        public static int intSearch_SalaryMonth_Data_Decrease(string strAMONTH, string strIDCARD)
        {

            int intHEALTHPAY = intSearch_SalaryMonth_Data_Healthpay(strAMONTH, strIDCARD);
            int intHEALTHPAY1 = intSearch_SalaryMonth_Data_Healthpay1(strAMONTH, strIDCARD);
            int intMONTHPAY_TAX = intSearch_SalaryMonth_Data_MonthpayTax(strAMONTH, strIDCARD);
            int intMONTHPAY = intSearch_SalaryMonth_Data_Monthpay(strAMONTH, strIDCARD);
            int intINSURANCEPAY = intSearch_SalaryMonth_Data_Insurancepay(strAMONTH, strIDCARD);
            int intTAX = intSearch_SalaryMonth_Data_Tax(strAMONTH, strIDCARD);
            int intCONCUR3PAY = intSearch_SalaryMonth_Data_Concur3pay(strAMONTH, strIDCARD);
            int intOther_Decrease = intSearch_SalaryMonth_Data_Other_Decrease(strIDCARD, strAMONTH);

            int intEXTRA01 = intSearch_SalaryMonth_Data_EXTRA01(strAMONTH, strIDCARD);
            int intEXTRA02 = intSearch_SalaryMonth_Data_EXTRA02(strAMONTH, strIDCARD);
            int intEXTRA03 = intSearch_SalaryMonth_Data_EXTRA03(strAMONTH, strIDCARD);
            int intEXTRA04 = intSearch_SalaryMonth_Data_EXTRA04(strAMONTH, strIDCARD);
            int intEXTRA05 = intSearch_SalaryMonth_Data_EXTRA05(strAMONTH, strIDCARD);
            int intEXTRA06 = intSearch_SalaryMonth_Data_EXTRA06(strAMONTH, strIDCARD);
            int intEXTRA07 = intSearch_SalaryMonth_Data_EXTRA07(strAMONTH, strIDCARD);
            int intEXTRA08 = intSearch_SalaryMonth_Data_EXTRA08(strAMONTH, strIDCARD);
            int intEXTRA09 = intSearch_SalaryMonth_Data_EXTRA09(strAMONTH, strIDCARD);

            return intHEALTHPAY + intHEALTHPAY1 + intMONTHPAY_TAX + intMONTHPAY + intINSURANCEPAY + intTAX + intCONCUR3PAY + intOther_Decrease
                + intEXTRA01 + intEXTRA02 + intEXTRA03 + intEXTRA04 + intEXTRA05 + intEXTRA06 + intEXTRA07 + intEXTRA08 + intEXTRA09;
        }

       

        /// <summary>
        /// 查詢每月薪資 - 月支數額
        /// </summary>
        /// <param name="strAMONTH">查詢月份</param>
        /// <param name="strIDCARD">查詢人員身分證號</param>
        /// <returns></returns>
        public static int intSearch_SalaryMonth_Data_Salarypay1(string strAMONTH, string strIDCARD)
        {
            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn.Open();
                try
                {
                    string strSQL = "SELECT AMONTH, IDCARD, SALARYPAY1 FROM B_MONTHPAY_MAIN WHERE AMONTH = '" + strAMONTH + "' AND IDCARD = '" + strIDCARD + "'";
                    SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
                    DataTable dt = new DataTable();
                    dt.Load(Selectcmd.ExecuteReader(CommandBehavior.CloseConnection));
                    if (dt.Rows.Count == 1)
                    {
                        return int.Parse(dt.Rows[0]["SALARYPAY1"].ToString());
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

        /// <summary>
        /// 查詢每月薪資 - 主管加給
        /// </summary>
        /// <param name="strAMONTH">查詢月份</param>
        /// <param name="strIDCARD">查詢人員身分證號</param>
        /// <returns></returns>
        public static int intSearch_SalaryMonth_Data_Boss(string strAMONTH, string strIDCARD)
        {
            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn.Open();
                try
                {
                    string strSQL = "SELECT AMONTH, IDCARD, BOSS FROM B_MONTHPAY_MAIN WHERE AMONTH = '" + strAMONTH + "' AND IDCARD = '" + strIDCARD + "'";
                    SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
                    DataTable dt = new DataTable();
                    dt.Load(Selectcmd.ExecuteReader(CommandBehavior.CloseConnection));
                    if (dt.Rows.Count == 1)
                    {
                        return int.Parse(dt.Rows[0]["BOSS"].ToString());
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

        /// <summary>
        /// 查詢每月薪資 - 專業加給
        /// </summary>
        /// <param name="strAMONTH">查詢月份</param>
        /// <param name="strIDCARD">查詢人員身分證號</param>
        /// <returns></returns>
        public static int intSearch_SalaryMonth_Data_Profess(string strAMONTH, string strIDCARD)
        {
            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn.Open();
                try
                {
                    string strSQL = "SELECT AMONTH, IDCARD, PROFESS FROM B_MONTHPAY_MAIN WHERE AMONTH = '" + strAMONTH + "' AND IDCARD = '" + strIDCARD + "'";
                    SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
                    DataTable dt = new DataTable();
                    dt.Load(Selectcmd.ExecuteReader(CommandBehavior.CloseConnection));
                    if (dt.Rows.Count == 1)
                    {
                        return int.Parse(dt.Rows[0]["PROFESS"].ToString());
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

        /// <summary>
        /// 查詢每月薪資 - 警勤加給
        /// </summary>
        /// <param name="strAMONTH">查詢月份</param>
        /// <param name="strIDCARD">查詢人員身分證號</param>
        /// <returns></returns>
        public static int intSearch_SalaryMonth_Data_Workp(string strAMONTH, string strIDCARD)
        {
            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn.Open();
                try
                {
                    string strSQL = "SELECT AMONTH, IDCARD, WORKP FROM B_MONTHPAY_MAIN WHERE AMONTH = '" + strAMONTH + "' AND IDCARD = '" + strIDCARD + "'";
                    SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
                    DataTable dt = new DataTable();
                    dt.Load(Selectcmd.ExecuteReader(CommandBehavior.CloseConnection));
                    if (dt.Rows.Count == 1)
                    {
                        return int.Parse(dt.Rows[0]["WORKP"].ToString());
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

        /// <summary>
        /// 查詢每月薪資 - 工作獎助金
        /// </summary>
        /// <param name="strAMONTH">查詢月份</param>
        /// <param name="strIDCARD">查詢人員身分證號</param>
        /// <returns></returns>
        public static int intSearch_SalaryMonth_Data_Bonus(string strAMONTH, string strIDCARD)
        {
            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn.Open();
                try
                {
                    string strSQL = "SELECT AMONTH, IDCARD, BONUS FROM B_MONTHPAY_MAIN WHERE AMONTH = '" + strAMONTH + "' AND IDCARD = '" + strIDCARD + "'";
                    SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
                    DataTable dt = new DataTable();
                    dt.Load(Selectcmd.ExecuteReader(CommandBehavior.CloseConnection));
                    if (dt.Rows.Count == 1)
                    {
                        return int.Parse(dt.Rows[0]["BONUS"].ToString());
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

        /// <summary>
        /// 查詢每月薪資 - 技術加給
        /// </summary>
        /// <param name="strAMONTH">查詢月份</param>
        /// <param name="strIDCARD">查詢人員身分證號</param>
        /// <returns></returns>
        public static int intSearch_SalaryMonth_Data_Technics(string strAMONTH, string strIDCARD)
        {
            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn.Open();
                try
                {
                    string strSQL = "SELECT AMONTH, IDCARD, TECHNICS FROM B_MONTHPAY_MAIN WHERE AMONTH = '" + strAMONTH + "' AND IDCARD = '" + strIDCARD + "'";
                    SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
                    DataTable dt = new DataTable();
                    dt.Load(Selectcmd.ExecuteReader(CommandBehavior.CloseConnection));
                    if (dt.Rows.Count == 1)
                    {
                        return int.Parse(dt.Rows[0]["TECHNICS"].ToString());
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

        /// <summary>
        /// 查詢每月薪資 - 外事加給
        /// </summary>
        /// <param name="strAMONTH">查詢月份</param>
        /// <param name="strIDCARD">查詢人員身分證號</param>
        /// <returns></returns>
        public static int intSearch_SalaryMonth_Data_Adventive(string strAMONTH, string strIDCARD)
        {
            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn.Open();
                try
                {
                    string strSQL = "SELECT AMONTH, IDCARD, ADVENTIVE FROM B_MONTHPAY_MAIN WHERE AMONTH = '" + strAMONTH + "' AND IDCARD = '" + strIDCARD + "'";
                    SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
                    DataTable dt = new DataTable();
                    dt.Load(Selectcmd.ExecuteReader(CommandBehavior.CloseConnection));
                    if (dt.Rows.Count == 1)
                    {
                        return int.Parse(dt.Rows[0]["ADVENTIVE"].ToString());
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

        /// <summary>
        /// 查詢每月薪資 - 偏遠加給
        /// </summary>
        /// <param name="strAMONTH">查詢月份</param>
        /// <param name="strIDCARD">查詢人員身分證號</param>
        /// <returns></returns>
        public static int intSearch_SalaryMonth_Data_Far(string strAMONTH, string strIDCARD)
        {
            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn.Open();
                try
                {
                    string strSQL = "SELECT AMONTH, IDCARD, FAR FROM B_MONTHPAY_MAIN WHERE AMONTH = '" + strAMONTH + "' AND IDCARD = '" + strIDCARD + "'";
                    SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
                    DataTable dt = new DataTable();
                    dt.Load(Selectcmd.ExecuteReader(CommandBehavior.CloseConnection));
                    if (dt.Rows.Count == 1)
                    {
                        return int.Parse(dt.Rows[0]["FAR"].ToString());
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

        /// <summary>
        /// 查詢每月薪資 - 水電補助
        /// </summary>
        /// <param name="strAMONTH">查詢月份</param>
        /// <param name="strIDCARD">查詢人員身分證號</param>
        /// <returns></returns>
        public static int intSearch_SalaryMonth_Data_Electric(string strAMONTH, string strIDCARD)
        {
            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn.Open();
                try
                {
                    string strSQL = "SELECT AMONTH, IDCARD, ELECTRIC FROM B_MONTHPAY_MAIN WHERE AMONTH = '" + strAMONTH + "' AND IDCARD = '" + strIDCARD + "'";
                    SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
                    DataTable dt = new DataTable();
                    dt.Load(Selectcmd.ExecuteReader(CommandBehavior.CloseConnection));
                    if (dt.Rows.Count == 1)
                    {
                        return int.Parse(dt.Rows[0]["ELECTRIC"].ToString());
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

        /// <summary>
        /// 統計應發總金額
        /// </summary>
        private static int intSearch_SalaryMonth_Data_Other_Increase_Statistics = 0;

        /// <summary>
        /// 計算其他應發金額
        /// </summary>
        /// <param name="strMZ_ID">身分證號</param>
        /// <param name="strAmonth">產生月份</param>
        /// <returns></returns>
        public static int intSearch_SalaryMonth_Data_Other_Increase(string strMZ_ID, string strAmonth)
        {
            intSearch_SalaryMonth_Data_Other_Increase_Statistics = 0;
            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn.Open();
                try
                {
                    string strSQL = "SELECT PAY FROM B_MONTHPAY_OTHER_PAY WHERE MM_SNID = '" + intMonthPay_Data_MM_SNID(strMZ_ID, strAmonth) + "' AND MP = 'P'";
                    SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
                    DataTable dt = new DataTable();
                    dt.Load(Selectcmd.ExecuteReader(CommandBehavior.CloseConnection));
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow item_S_SNID in dt.Rows)
                        {
                            intSearch_SalaryMonth_Data_Other_Increase_Statistics = intSearch_SalaryMonth_Data_Other_Increase_Statistics + int.Parse(item_S_SNID["PAY"].ToString());
                        }
                        return intSearch_SalaryMonth_Data_Other_Increase_Statistics;
                    }
                    else
                    {
                        return intSearch_SalaryMonth_Data_Other_Increase_Statistics;
                    }
                }
                //catch { return intSearch_SalaryMonth_Data_Other_Increase_Statistics + intOtherMinus; }
                finally { Selectconn.Close();
                //XX2013/06/18 
                Selectconn.Dispose();
                }
            }
        }

        ///// <summary>
        ///// 查詢每月薪資 - 健保年功俸
        ///// </summary>
        ///// <param name="strAMONTH">查詢月份</param>
        ///// <param name="strIDCARD">查詢人員身分證號</param>
        ///// <returns></returns>
        //public static int intSearch_SalaryMonth_Data_Healthid(string strAMONTH, string strIDCARD)
        //{
        //    using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
        //    {
        //        Selectconn.Open();
        //        try
        //        {
        //            string strSQL = "SELECT AMONTH, IDCARD, HEALTHID FROM B_MONTHPAY_MAIN WHERE AMONTH = '" + strAMONTH + "' AND IDCARD = '" + strIDCARD + "'";
        //            SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
        //            DataTable dt = new DataTable();
        //            dt.Load(Selectcmd.ExecuteReader(CommandBehavior.CloseConnection));
        //            if (dt.Rows.Count == 1)
        //            {
        //                return int.Parse(dt.Rows[0]["HEALTHID"].ToString());
        //            }
        //            else
        //            {
        //                return 0;
        //            }
        //        }
        //        catch { return 0; }
        //        finally { Selectconn.Close();
        //        //XX2013/06/18 
        //        Selectconn.Dispose();
        //        }
        //    }
        //}

        ///// <summary>
        ///// 查詢每月薪資 - 健保眷口數
        ///// </summary>
        ///// <param name="strAMONTH">查詢月份</param>
        ///// <param name="strIDCARD">查詢人員身分證號</param>
        ///// <returns></returns>
        //public static int intSearch_SalaryMonth_Data_Healthman(string strAMONTH, string strIDCARD)
        //{
        //    using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
        //    {
        //        Selectconn.Open();
        //        try
        //        {
        //            string strSQL = "SELECT AMONTH, IDCARD, HEALTHMAN FROM B_MONTHPAY_MAIN WHERE AMONTH = '" + strAMONTH + "' AND IDCARD = '" + strIDCARD + "'";
        //            SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
        //            DataTable dt = new DataTable();
        //            dt.Load(Selectcmd.ExecuteReader(CommandBehavior.CloseConnection));
        //            if (dt.Rows.Count == 1)
        //            {
        //                return int.Parse(dt.Rows[0]["HEALTHMAN"].ToString());
        //            }
        //            else
        //            {
        //                return 0;
        //            }
        //        }
        //        catch { return 0; }
        //        finally { Selectconn.Close();
        //        //XX2013/06/18 
        //        Selectconn.Dispose();
        //        }
        //    }
        //}

        /// <summary>
        /// 查詢每月薪資 - 健保費
        /// </summary>
        /// <param name="strAMONTH">查詢月份</param>
        /// <param name="strIDCARD">查詢人員身分證號</param>
        /// <returns></returns>
        public static int intSearch_SalaryMonth_Data_Healthpay(string strAMONTH, string strIDCARD)
        {
            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn.Open();
                try
                {
                    string strSQL = "SELECT AMONTH, IDCARD, HEALTHPAY FROM B_MONTHPAY_MAIN WHERE AMONTH = '" + strAMONTH + "' AND IDCARD = '" + strIDCARD + "'";
                    SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
                    DataTable dt = new DataTable();
                    dt.Load(Selectcmd.ExecuteReader(CommandBehavior.CloseConnection));
                    if (dt.Rows.Count == 1)
                    {
                        return int.Parse(dt.Rows[0]["HEALTHPAY"].ToString());
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

        /// <summary>
        /// 查詢每月薪資 - 健保費補差扣款
        /// </summary>
        /// <param name="strAMONTH">查詢月份</param>
        /// <param name="strIDCARD">查詢人員身分證號</param>
        /// <returns></returns>
        public static int intSearch_SalaryMonth_Data_Healthpay1(string strAMONTH, string strIDCARD)
        {
            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn.Open();
                try
                {
                    string strSQL = "SELECT AMONTH, IDCARD, HEALTHPAY1 FROM B_MONTHPAY_MAIN WHERE AMONTH = '" + strAMONTH + "' AND IDCARD = '" + strIDCARD + "'";
                    SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
                    DataTable dt = new DataTable();
                    dt.Load(Selectcmd.ExecuteReader(CommandBehavior.CloseConnection));
                    if (dt.Rows.Count == 1)
                    {
                        return int.Parse(dt.Rows[0]["HEALTHPAY1"].ToString());
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

        /// <summary>
        /// 查詢每月薪資 - 薪資扣款(列入所得)
        /// </summary>
        /// <param name="strAMONTH">查詢月份</param>
        /// <param name="strIDCARD">查詢人員身分證號</param>
        /// <returns></returns>
        public static int intSearch_SalaryMonth_Data_MonthpayTax(string strAMONTH, string strIDCARD)
        {
            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn.Open();
                try
                {
                    string strSQL = "SELECT AMONTH, IDCARD, MONTHPAY_TAX FROM B_MONTHPAY_MAIN WHERE AMONTH = '" + strAMONTH + "' AND IDCARD = '" + strIDCARD + "'";
                    SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
                    DataTable dt = new DataTable();
                    dt.Load(Selectcmd.ExecuteReader(CommandBehavior.CloseConnection));
                    if (dt.Rows.Count == 1)
                    {
                        return int.Parse(dt.Rows[0]["MONTHPAY_TAX"].ToString());
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

        /// <summary>
        /// 查詢每月薪資 - 薪資扣款(不列入所得)
        /// </summary>
        /// <param name="strAMONTH">查詢月份</param>
        /// <param name="strIDCARD">查詢人員身分證號</param>
        /// <returns></returns>
        public static int intSearch_SalaryMonth_Data_Monthpay(string strAMONTH, string strIDCARD)
        {
            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn.Open();
                try
                {
                    string strSQL = "SELECT AMONTH, IDCARD, MONTHPAY FROM B_MONTHPAY_MAIN WHERE AMONTH = '" + strAMONTH + "' AND IDCARD = '" + strIDCARD + "'";
                    SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
                    DataTable dt = new DataTable();
                    dt.Load(Selectcmd.ExecuteReader(CommandBehavior.CloseConnection));
                    if (dt.Rows.Count == 1)
                    {
                        return int.Parse(dt.Rows[0]["MONTHPAY"].ToString());
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

        /// <summary>
        /// 查詢每月薪資 - 公(勞)保費
        /// </summary>
        /// <param name="strAMONTH">查詢月份</param>
        /// <param name="strIDCARD">查詢人員身分證號</param>
        /// <returns></returns>
        public static int intSearch_SalaryMonth_Data_Insurancepay(string strAMONTH, string strIDCARD)
        {
            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn.Open();
                try
                {
                    string strSQL = "SELECT AMONTH, IDCARD, INSURANCEPAY FROM B_MONTHPAY_MAIN WHERE AMONTH = '" + strAMONTH + "' AND IDCARD = '" + strIDCARD + "'";
                    SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
                    DataTable dt = new DataTable();
                    dt.Load(Selectcmd.ExecuteReader(CommandBehavior.CloseConnection));
                    if (dt.Rows.Count == 1)
                    {
                        return int.Parse(dt.Rows[0]["INSURANCEPAY"].ToString());
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

        /// <summary>
        /// 查詢每月薪資 - 薪津所得稅
        /// </summary>
        /// <param name="strAMONTH">查詢月份</param>
        /// <param name="strIDCARD">查詢人員身分證號</param>
        /// <returns></returns>
        public static int intSearch_SalaryMonth_Data_Tax(string strAMONTH, string strIDCARD)
        {
            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn.Open();
                try
                {
                    string strSQL = "SELECT AMONTH, IDCARD, TAX FROM B_MONTHPAY_MAIN WHERE AMONTH = '" + strAMONTH + "' AND IDCARD = '" + strIDCARD + "'";
                    SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
                    DataTable dt = new DataTable();
                    dt.Load(Selectcmd.ExecuteReader(CommandBehavior.CloseConnection));
                    if (dt.Rows.Count == 1)
                    {
                        return int.Parse(dt.Rows[0]["TAX"].ToString());
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

        /// <summary>
        /// 查詢每月薪資 - 退撫金費
        /// </summary>
        /// <param name="strAMONTH">查詢月份</param>
        /// <param name="strIDCARD">查詢人員身分證號</param>
        /// <returns></returns>
        public static int intSearch_SalaryMonth_Data_Concur3pay(string strAMONTH, string strIDCARD)
        {
            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn.Open();
                try
                {
                    string strSQL = "SELECT AMONTH, IDCARD, CONCUR3PAY FROM B_MONTHPAY_MAIN WHERE AMONTH = '" + strAMONTH + "' AND IDCARD = '" + strIDCARD + "'";
                    SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
                    DataTable dt = new DataTable();
                    dt.Load(Selectcmd.ExecuteReader(CommandBehavior.CloseConnection));
                    if (dt.Rows.Count == 1)
                    {
                        return int.Parse(dt.Rows[0]["CONCUR3PAY"].ToString());
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

        /// <summary>
        /// 統計應扣總金額
        /// </summary>
        private static int intSearch_SalaryMonth_Data_Other_Decrease_Statistics = 0;

        /// <summary>
        /// 計算其他應扣金額
        /// </summary>
        /// <param name="strMZ_ID">身分證號</param>
        /// <param name="strAmonth">產生月份</param>
        /// <returns></returns>
        public static int intSearch_SalaryMonth_Data_Other_Decrease(string strMZ_ID, string strAmonth)
        {
            intSearch_SalaryMonth_Data_Other_Decrease_Statistics = 0;
            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn.Open();
                try
                {
                    // 不知道是耍什麼酣加的，根本沒有OTHERMINUS這個欄位
                    //int intOtherMinus;
                    //int.TryParse(o_DBFactory.ABC_toTest.vExecSQL(string.Format("select OTHERMINUS from B_MONTHPAY_MAIN where IDCARD = '{0}' AND AMONTH = '{1}'", strMZ_ID, strAmonth)), out intOtherMinus);

                    string strSQL = "SELECT * FROM B_MONTHPAY_OTHER_PAY WHERE MM_SNID = '" + intMonthPay_Data_MM_SNID(strMZ_ID, strAmonth) + "' AND MP = 'M'";
                    SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
                    DataTable dt = new DataTable();
                    dt.Load(Selectcmd.ExecuteReader(CommandBehavior.CloseConnection));
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow item_S_SNID in dt.Rows)
                        {
                            intSearch_SalaryMonth_Data_Other_Decrease_Statistics = intSearch_SalaryMonth_Data_Other_Decrease_Statistics + int.Parse(item_S_SNID["PAY"].ToString());
                        }
                        return intSearch_SalaryMonth_Data_Other_Decrease_Statistics;// +intOtherMinus;
                    }
                    else
                    {
                        return intSearch_SalaryMonth_Data_Other_Decrease_Statistics;// +intOtherMinus;
                    }
                }
                //catch { return intSearch_SalaryMonth_Data_Other_Decrease_Statistics; }
                finally { Selectconn.Close();
                //XX2013/06/18 
                Selectconn.Dispose();
                }
            }
        }

        /// <summary>
        /// 查詢每月薪資 - 法院扣款金額
        /// </summary>
        /// <param name="strAMONTH">查詢月份</param>
        /// <param name="strIDCARD">查詢人員身分證號</param>
        /// <returns></returns>
        public static int intSearch_SalaryMonth_Data_EXTRA01(string strAMONTH, string strIDCARD)
        {
            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn.Open();
                try
                {
                    string strSQL = "SELECT AMONTH, IDCARD, EXTRA01 FROM B_MONTHPAY_MAIN WHERE AMONTH = '" + strAMONTH + "' AND IDCARD = '" + strIDCARD + "'";
                    SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
                    DataTable dt = new DataTable();
                    dt.Load(Selectcmd.ExecuteReader(CommandBehavior.CloseConnection));
                    if (dt.Rows.Count == 1)
                    {
                        return int.Parse(dt.Rows[0]["EXTRA01"].ToString());
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

        /// <summary>
        /// 查詢每月薪資 - 國宅貸款金額
        /// </summary>
        /// <param name="strAMONTH">查詢月份</param>
        /// <param name="strIDCARD">查詢人員身分證號</param>
        /// <returns></returns>
        public static int intSearch_SalaryMonth_Data_EXTRA02(string strAMONTH, string strIDCARD)
        {
            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn.Open();
                try
                {
                    string strSQL = "SELECT AMONTH, IDCARD, EXTRA02 FROM B_MONTHPAY_MAIN WHERE AMONTH = '" + strAMONTH + "' AND IDCARD = '" + strIDCARD + "'";
                    SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
                    DataTable dt = new DataTable();
                    dt.Load(Selectcmd.ExecuteReader(CommandBehavior.CloseConnection));
                    if (dt.Rows.Count == 1)
                    {
                        return int.Parse(dt.Rows[0]["EXTRA02"].ToString());
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

        /// <summary>
        /// 查詢每月薪資 - 銀行貸款金額
        /// </summary>
        /// <param name="strAMONTH">查詢月份</param>
        /// <param name="strIDCARD">查詢人員身分證號</param>
        /// <returns></returns>
        public static int intSearch_SalaryMonth_Data_EXTRA03(string strAMONTH, string strIDCARD)
        {
            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn.Open();
                try
                {
                    string strSQL = "SELECT AMONTH, IDCARD, EXTRA03 FROM B_MONTHPAY_MAIN WHERE AMONTH = '" + strAMONTH + "' AND IDCARD = '" + strIDCARD + "'";
                    SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
                    DataTable dt = new DataTable();
                    dt.Load(Selectcmd.ExecuteReader(CommandBehavior.CloseConnection));
                    if (dt.Rows.Count == 1)
                    {
                        return int.Parse(dt.Rows[0]["EXTRA03"].ToString());
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

        /// <summary>
        /// 查詢每月薪資 - 分期付款金額
        /// </summary>
        /// <param name="strAMONTH">查詢月份</param>
        /// <param name="strIDCARD">查詢人員身分證號</param>
        /// <returns></returns>
        public static int intSearch_SalaryMonth_Data_EXTRA04(string strAMONTH, string strIDCARD)
        {
            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn.Open();
                try
                {
                    string strSQL = "SELECT AMONTH, IDCARD, EXTRA04 FROM B_MONTHPAY_MAIN WHERE AMONTH = '" + strAMONTH + "' AND IDCARD = '" + strIDCARD + "'";
                    SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
                    DataTable dt = new DataTable();
                    dt.Load(Selectcmd.ExecuteReader(CommandBehavior.CloseConnection));
                    if (dt.Rows.Count == 1)
                    {
                        return int.Parse(dt.Rows[0]["EXTRA04"].ToString());
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

        /// <summary>
        /// 查詢每月薪資 - 優惠存款金額
        /// </summary>
        /// <param name="strAMONTH">查詢月份</param>
        /// <param name="strIDCARD">查詢人員身分證號</param>
        /// <returns></returns>
        public static int intSearch_SalaryMonth_Data_EXTRA05(string strAMONTH, string strIDCARD)
        {
            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn.Open();
                try
                {
                    string strSQL = "SELECT AMONTH, IDCARD, EXTRA05 FROM B_MONTHPAY_MAIN WHERE AMONTH = '" + strAMONTH + "' AND IDCARD = '" + strIDCARD + "'";
                    SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
                    DataTable dt = new DataTable();
                    dt.Load(Selectcmd.ExecuteReader(CommandBehavior.CloseConnection));
                    if (dt.Rows.Count == 1)
                    {
                        return int.Parse(dt.Rows[0]["EXTRA05"].ToString());
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

        /// <summary>
        /// 查詢每月薪資 - 員工宿舍費金額
        /// </summary>
        /// <param name="strAMONTH">查詢月份</param>
        /// <param name="strIDCARD">查詢人員身分證號</param>
        /// <returns></returns>
        public static int intSearch_SalaryMonth_Data_EXTRA06(string strAMONTH, string strIDCARD)
        {
            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn.Open();
                try
                {
                    string strSQL = "SELECT AMONTH, IDCARD, EXTRA06 FROM B_MONTHPAY_MAIN WHERE AMONTH = '" + strAMONTH + "' AND IDCARD = '" + strIDCARD + "'";
                    SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
                    DataTable dt = new DataTable();
                    dt.Load(Selectcmd.ExecuteReader(CommandBehavior.CloseConnection));
                    if (dt.Rows.Count == 1)
                    {
                        return int.Parse(dt.Rows[0]["EXTRA06"].ToString());
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

        /// <summary>
        /// 查詢每月薪資 - 伙食費金額
        /// </summary>
        /// <param name="strAMONTH">查詢月份</param>
        /// <param name="strIDCARD">查詢人員身分證號</param>
        /// <returns></returns>
        public static int intSearch_SalaryMonth_Data_EXTRA07(string strAMONTH, string strIDCARD)
        {
            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn.Open();
                try
                {
                    string strSQL = "SELECT AMONTH, IDCARD, EXTRA07 FROM B_MONTHPAY_MAIN WHERE AMONTH = '" + strAMONTH + "' AND IDCARD = '" + strIDCARD + "'";
                    SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
                    DataTable dt = new DataTable();
                    dt.Load(Selectcmd.ExecuteReader(CommandBehavior.CloseConnection));
                    if (dt.Rows.Count == 1)
                    {
                        return int.Parse(dt.Rows[0]["EXTRA07"].ToString());
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

        /// <summary>
        /// 查詢每月薪資 - 福利互助金金額
        /// </summary>
        /// <param name="strAMONTH">查詢月份</param>
        /// <param name="strIDCARD">查詢人員身分證號</param>
        /// <returns></returns>
        public static int intSearch_SalaryMonth_Data_EXTRA08(string strAMONTH, string strIDCARD)
        {
            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn.Open();
                try
                {
                    string strSQL = "SELECT AMONTH, IDCARD, EXTRA08 FROM B_MONTHPAY_MAIN WHERE AMONTH = '" + strAMONTH + "' AND IDCARD = '" + strIDCARD + "'";
                    SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
                    DataTable dt = new DataTable();
                    dt.Load(Selectcmd.ExecuteReader(CommandBehavior.CloseConnection));
                    if (dt.Rows.Count == 1)
                    {
                        return int.Parse(dt.Rows[0]["EXTRA08"].ToString());
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

        /// <summary>
        /// 查詢每月薪資 - 退撫金貸款金額
        /// </summary>
        /// <param name="strAMONTH">查詢月份</param>
        /// <param name="strIDCARD">查詢人員身分證號</param>
        /// <returns></returns>
        public static int intSearch_SalaryMonth_Data_EXTRA09(string strAMONTH, string strIDCARD)
        {
            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn.Open();
                try
                {
                    string strSQL = "SELECT AMONTH, IDCARD, EXTRA09 FROM B_MONTHPAY_MAIN WHERE AMONTH = '" + strAMONTH + "' AND IDCARD = '" + strIDCARD + "'";
                    SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
                    DataTable dt = new DataTable();
                    dt.Load(Selectcmd.ExecuteReader(CommandBehavior.CloseConnection));
                    if (dt.Rows.Count == 1)
                    {
                        return int.Parse(dt.Rows[0]["EXTRA09"].ToString());
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
