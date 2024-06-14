using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace TPPDDB._2_salary
{
    public class SalaryBasic : System.Web.UI.Page
    {
        ///// <summary>
        ///// 民國年月日7碼轉DateTime
        ///// </summary>
        ///// <param name="strDate"></param>
        ///// <returns></returns>
        //public static DateTime ToDate(string strDate)
        //{
        //    int y;
        //    int m;
        //    int d;

        //    if (strDate.Length != 7)
        //        return System.DateTime.Now;

        //    y = int.Parse(strDate.Substring(0, 3));
        //    m = int.Parse(strDate.Substring(3, 2));
        //    d = int.Parse(strDate.Substring(5, 2));

        //    y += 1911;// 年份轉西元

        //    if (m <= 0 || m > 12)
        //        return System.DateTime.Now;

        //    int maxD = new DateTime(y, m + 1, 1).AddDays(-1).Day;

        //    if (d <= 0 || d > maxD)
        //        return System.DateTime.Now;

        //    return new DateTime(y, m, d);
        //}

//        /// <summary>
//        /// 查詢每月薪資總計
//        /// </summary>
//        /// <param name="strIDCARD">身分證號</param>
//        /// <param name="AMONTH">查詢月份</param>
//        /// <returns></returns>
//        public static int intTotalMonthPay(string strIDCARD, string strAMONTH)
//        {
//            string strMM_SNID = string.Empty;
            

//            string strSQL = @"SELECT 
//                            ((SELECT DISTINCT (SUM(SALARYPAY1+WORKP+PROFESS+BOSS+TECHNICS+BONUS+ADVENTIVE+FAR+ELECTRIC-INSURANCEPAY-HEALTHPAY-HEALTHPAY1-MONTHPAY_TAX-MONTHPAY-CONCUR3PAY-TAX-EXTRA01-EXTRA02-EXTRA03-EXTRA04-EXTRA05-EXTRA06-EXTRA07-EXTRA08-EXTRA09) FROM B_MONTHPAY_MAIN WHERE IDCARD=BMM.IDCARD)+
//                            (CASE WHEN (SELECT SUM(PAY) FROM B_MONTHPAY_OTHER_PAY WHERE MM_SNID=BMOP.MM_SNID AND MP='P')IS NULL THEN 0 ELSE (SELECT SUM(PAY) FROM B_MONTHPAY_OTHER_PAY WHERE MM_SNID=BMOP.MM_SNID AND MP='P') END)-
//                            (CASE WHEN (SELECT SUM(PAY) FROM B_MONTHPAY_OTHER_PAY WHERE MM_SNID=BMOP.MM_SNID AND MP='M')IS NULL THEN 0 ELSE (SELECT SUM(PAY) FROM B_MONTHPAY_OTHER_PAY WHERE MM_SNID=BMOP.MM_SNID AND MP='M') END)) MONTHPAY 
//                            FROM B_MONTHPAY_MAIN BMM JOIN B_MONTHPAY_OTHER_PAY BMOP 
//                            ON BMM.MM_SNID=BMOP.MM_SNID WHERE IDCARD='" + strIDCARD + "' AND AMONTH='" + strAMONTH + "' "
//                            + "GROUP BY IDCARD,AMONTH,BMOP.MM_SNID";
//            DataTable dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "TotalMonthPay");
//            if (dt.Rows.Count == 0)
//            {
//                return 0;
//            }
            
//            return (int)dt.Rows[0]["MONTHPAY"];
//        }

//        /// <summary>
//        /// 查詢補發薪資總計
//        /// </summary>
//        /// <param name="strIDCARD">身分證號</param>
//        /// <param name="AMONTH">查詢月份</param>
//        /// <returns></returns>
//        public static int intTotalRepairPay(string strIDCARD, string strAMONTH)
//        {
//            string strR_SNID = string.Empty;
//            int intMPAY = 0;
//            int intPPAY = 0;
//            int intSum = 0;

//            string strSQL = @"SELECT 
//                            R_SNID,SALARYPAY1,WORKPPAY,PROFESSPAY,BOSSPAY,TECHNICSPAY,BONUSPAY,ADVENTIVEPAY,FARPAY,ELECTRICPAY,INSURANCEPAY,HEALTHPAY,HEALTHPAY1,MONTHPAY_TAX,MONTHPAY,CONCUR3PAY,TAX,
//                            EXTRA01,EXTRA02,EXTRA03,EXTRA04,EXTRA05,EXTRA06,EXTRA07,EXTRA08,EXTRA09 FROM B_REPAIRPAY 
//                            WHERE IDCARD='" + strIDCARD + "' AND AMONTH='" + strAMONTH + "'";
//            DataTable dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "TotalRepairPay");
//            if (dt.Rows.Count == 0)
//            {
//                return 0;
//            }
//            intSum = Convert.ToInt32(dt.Rows[0]["SALARYPAY1"]) + Convert.ToInt32(dt.Rows[0]["WORKPPAY"]) + Convert.ToInt32(dt.Rows[0]["PROFESSPAY"]) +
//                Convert.ToInt32(dt.Rows[0]["BOSSPAY"]) + Convert.ToInt32(dt.Rows[0]["TECHNICSPAY"]) + Convert.ToInt32(dt.Rows[0]["BONUSPAY"]) + Convert.ToInt32(dt.Rows[0]["ADVENTIVEPAY"]) + 
//                Convert.ToInt32(dt.Rows[0]["FARPAY"]) + Convert.ToInt32(dt.Rows[0]["ELECTRICPAY"]) - Convert.ToInt32(dt.Rows[0]["INSURANCEPAY"]) - Convert.ToInt32(dt.Rows[0]["HEALTHPAY"]) -
//                Convert.ToInt32(dt.Rows[0]["HEALTHPAY1"]) - Convert.ToInt32(dt.Rows[0]["MONTHPAY_TAX"]) - Convert.ToInt32(dt.Rows[0]["MONTHPAY"]) - Convert.ToInt32(dt.Rows[0]["CONCUR3PAY"]) - Convert.ToInt32(dt.Rows[0]["TAX"]);
//            intSum = intSum - Convert.ToInt32(dt.Rows[0]["EXTRA01"]) - Convert.ToInt32(dt.Rows[0]["EXTRA02"]) - Convert.ToInt32(dt.Rows[0]["EXTRA03"])
//                - Convert.ToInt32(dt.Rows[0]["EXTRA04"]) - Convert.ToInt32(dt.Rows[0]["EXTRA05"]) - Convert.ToInt32(dt.Rows[0]["EXTRA06"]) - Convert.ToInt32(dt.Rows[0]["EXTRA07"]) 
//                - Convert.ToInt32(dt.Rows[0]["EXTRA08"]) - Convert.ToInt32(dt.Rows[0]["EXTRA09"]);

//            strR_SNID = dt.Rows[0]["R_SNID"].ToString();
//            strSQL = string.Format("SELECT (SELECT SUM(PAY) FROM B_REPAIRPAY_OTHER_PAY WHERE MP='M' AND R_SNID='{0}') MPAY,(SELECT SUM(PAY) FROM B_REPAIRPAY_OTHER_PAY WHERE MP='P' AND R_SNID='{0}') PPAY ", strR_SNID);

//            dt.Clear();
//            dt.Columns.Clear();

//            dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "OtherMonthPay");
//            if (dt.Rows[0]["MPAY"].ToString() != "")
//            {
//                intMPAY = Convert.ToInt32(dt.Rows[0]["MPAY"]);
//            }
//            if (dt.Rows[0]["PPAY"].ToString() != "")
//            {
//                intPPAY = Convert.ToInt32(dt.Rows[0]["PPAY"]);
//            }
//            intSum = intSum - intMPAY + intMPAY;

//            return intSum;
//        }

        ///// <summary>
        ///// 查詢年終獎金總計
        ///// </summary>
        ///// <param name="strIDCARD">身分證號</param>
        ///// <param name="AMONTH">查詢年分</param>
        ///// <returns></returns>
        //public static int intTotalYearPay(string strIDCARD, string strYEAR)
        //{
        //    string strSQL = "SELECT TOTAL FROM B_YEARPAY WHERE IDCARD='" + strIDCARD + "' AND AYEAR='" + strYEAR + "'";
        //    DataTable dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "TotalYearPay");
        //    if (dt.Rows.Count == 0)
        //    {
        //        return 0;
        //    }
        //    return Convert.ToInt32(dt.Rows[0]["TOTAL"]);
        //}

        ///// <summary>
        ///// 查詢考績獎金總計
        ///// </summary>
        ///// <param name="strIDCARD">身分證號</param>
        ///// <param name="AMONTH">查詢年分</param>
        ///// <returns></returns>
        //public static int intTotalEffectPay(string strIDCARD, string strYEAR)
        //{
        //    string strSQL = "SELECT TOTAL FROM B_EFFECT WHERE IDCARD='" + strIDCARD + "' AND AYEAR='" + strYEAR + "'";
        //    DataTable dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "TotalEffectPay");
        //    if (dt.Rows.Count == 0)
        //    {
        //        return 0;
        //    }
        //    return Convert.ToInt32(dt.Rows[0]["TOTAL"]);
        //}

        ///// <summary>
        ///// 修改人事發薪機關、員工編號
        ///// </summary>
        ///// <param name="strMZ_ID">身分證號</param>
        ///// <param name="strPAY_AD">發薪單位</param>
        ///// <param name="strMZ_POLNO">員工編號</param>
        ///// <returns></returns>
        //public static bool boolPersonnel_Set_Update(string strMZ_ID, string strPAY_AD, string strMZ_POLNO)
        //{
        //    string strPAY_AD_Data = "";
        //    if (strPAY_AD == "-1")
        //    {
        //        strPAY_AD_Data = "PAY_AD = NULL";
        //    }
        //    else
        //    {
        //        strPAY_AD_Data = "PAY_AD = '" + strPAY_AD + "'";
        //    }
        //    using (SqlConnection UpdateConn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
        //    {
        //        UpdateConn.Open();
        //        try
        //        {
        //            string UpdateString = "UPDATE A_DLBASE SET " + strPAY_AD_Data + ", MZ_POLNO = '" + strMZ_POLNO + "' WHERE MZ_ID = '" + strMZ_ID + "'";

        //            SqlCommand cmd = new SqlCommand(UpdateString, UpdateConn);
        //            cmd.CommandType = CommandType.Text;

        //            cmd.Connection = UpdateConn;
        //            cmd.ExecuteNonQuery();
        //            return true;
        //        }
        //        catch { return false; }
        //        finally { UpdateConn.Close();
        //        //XX2013/06/18 
        //        UpdateConn.Dispose();
        //        }
        //    }
        //}

        //public static bool UpdatePersonnalData(string strMZ_ID, string strPAY_AD, string strMZ_POLNO, string strMZ_ADATE, string strMZ_LDATE, string strGrade, string strMZ_TDATE, string strMZ_ODATE, string strMZ_SRANK, string strMZ_SPT, string strMZ_SLVC)
        //{
        //    string strPAY_AD_Data = "";
        //    if (strPAY_AD == "-1")
        //    {
        //        strPAY_AD_Data = "PAY_AD = NULL";
        //    }
        //    else
        //    {
        //        strPAY_AD_Data = "PAY_AD = '" + strPAY_AD + "'";
        //    }
        //    using (SqlConnection UpdateConn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
        //    {
        //        UpdateConn.Open();
        //        try
        //        {
        //            string strBaseSQL;
        //            string strGradeSQL;
        //            int intYear = DateTime.Now.Year - 1911 - 1;

        //            strBaseSQL = string.Format("UPDATE A_DLBASE SET {0}, MZ_POLNO = '{1}', MZ_SRANK='{2}', MZ_SPT='{3}', MZ_SLVC='{4}' ",
        //                                                 strPAY_AD_Data, strMZ_POLNO, strMZ_SRANK, strMZ_SPT, strMZ_SLVC);

        //            if (strMZ_ADATE.Length > 0)
        //                strBaseSQL += string.Format(", MZ_ADATE = '{0}'", strMZ_ADATE);
        //            if (strMZ_LDATE.Length > 0)
        //                strBaseSQL += string.Format(", MZ_LDATE = '{0}'", strMZ_LDATE);
        //            if (strMZ_TDATE.Length > 0)
        //                strBaseSQL += string.Format(", MZ_TDATE = '{0}'", strMZ_TDATE);
        //            if (strMZ_ODATE.Length > 0)
        //                strBaseSQL += string.Format(", MZ_ODATE = '{0}'", strMZ_ODATE);

        //            strBaseSQL += string.Format(" WHERE MZ_ID = '{0}'", strMZ_ID);


        //            SqlCommand cmd = new SqlCommand(strBaseSQL, UpdateConn);
        //            cmd.CommandType = CommandType.Text;

        //            cmd.Connection = UpdateConn;
        //            cmd.ExecuteNonQuery();

        //            SalaryBasic.UpdateSalaryData(strMZ_ID);

        //            if (strGrade == "")
        //                return true;

        //            strGradeSQL = string.Format("SELECT COUNT(*) FROM A_REV_BASE WHERE MZ_ID = '{0}' AND MZ_YEAR = '{1}'", strMZ_ID, intYear.ToString().PadLeft(3, '0'));
        //            if (o_DBFactory.ABC_toTest.vExecSQL(strGradeSQL) == "0")// 尚無此人去年考績資料，新增
        //            {
        //                strGradeSQL = string.Format("INSERT INTO A_REV_BASE( MZ_YEAR, MZ_ID, MZ_GRADE) VALUES( '{0}', '{1}', {2})", intYear.ToString().PadLeft(3, '0'), strMZ_ID, strGrade == "" ? "NULL" : strGrade);
        //            }
        //            else
        //            {
        //                strGradeSQL = string.Format("UPDATE A_REV_BASE SET MZ_GRADE = '{2}' WHERE MZ_ID = '{0}' AND MZ_YEAR = '{1}' ", strMZ_ID, intYear.ToString().PadLeft(3, '0'), strGrade == "" ? "NULL" : strGrade);
        //            }
        //            o_DBFactory.ABC_toTest.Edit_Data(strGradeSQL);

        //            return true;
        //        }
        //        catch { return false; }
        //        finally { UpdateConn.Close();
        //        //XX2013/06/18 
        //        UpdateConn.Dispose();
        //        }
        //    }
        //}

        

//        /// <summary>
//        /// 個人薪資設定 - 新增
//        /// </summary>
//        /// <param name="strIDCARD_Data">身分證號</param>
//        /// <param name="intCONCUR3_Data">是否扣退撫金</param>
//        /// <param name="strTECHNICS_Data">技術加給</param>
//        /// <param name="strBONUS_Data">工作獎助金</param>
//        /// <param name="strWORKP_Data">警勤加給</param>
//        /// <param name="strADVENTIVE_Data">外事加給</param>
//        /// <param name="strELECTRIC_Data">水電補助</param>
//        /// <param name="strHEALPER_INSURANCE">健保費級距</param>
//        /// <param name="strLABOR_INSURANCE">勞保費級距</param>
//        /// <param name="intTAXPER_Data">所得稅扣款金額</param>
//        /// <param name="intDE_HEALTH">健保費扣款</param>
//        /// <param name="intMONTHPAY_TAX">薪資扣款(列入所得)</param>
//        /// <param name="intMONTHPAY">薪資扣款(不列入所得)</param>
//        /// <param name="intTAXCHILD">所得稅撫養人</param>
//        /// <param name="intEXTRA01_Data">法院扣款</param>
//        /// <param name="intEXTRA02_Data">國宅貸款</param>
//        /// <param name="intEXTRA03_Data">銀行貸款</param>
//        /// <param name="intEXTRA04_Data">分期付款</param>
//        /// <param name="intEXTRA05_Data">優惠存款</param>
//        /// <param name="intEXTRA06_Data">員工宿舍費</param>
//        /// <param name="intEXTRA07_Data">伙食費</param>
//        /// <param name="intEXTRA08_Data">福利互助金費</param>
//        /// <param name="intEXTRA09_Data">退撫金貸款</param>
//        /// <param name="strNOTE_Data">薪資備註</param>
//        /// <returns></returns>
//        public static bool boolSalaryPay_Set_Create(string strIDCARD_Data, int intCONCUR3_Data,
//            string strTECHNICS_Data, string strBONUS_Data, string strWORKP_Data,
//            string strADVENTIVE_Data, string strELECTRIC_Data,
//            int intTAXPER_Data, int intDE_HEALTH, int intMONTHPAY_TAX, int intMONTHPAY, int intTAXCHILD, string strHEALPER_INSURANCE, string strLABOR_INSURANCE,
//            int intEXTRA01_Data, int intEXTRA02_Data, int intEXTRA03_Data, int intEXTRA04_Data, int intEXTRA05_Data,
//            int intEXTRA06_Data, int intEXTRA07_Data, int intEXTRA08_Data, int intEXTRA09_Data,
//            string strNOTE_Data, int intBoss, int intProfess, int intTechnics, int intBonus, int intWorkp, int intAdventive, int intFar, int intElectric, int intSalary, int intConcur3Pay, string strIsOffDuty, int intOtherAdd)
//        {
//            using (SqlConnection InsertConn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
//            {
//                InsertConn.Open();
//                try
//                {
//                    string strSQL;

//                    strSQL = @"insert into B_BASE (B_SNID, IDCARD, CONCUR3, TECHNICS, BONUS, WORKP, ADVENTIVE, ELECTRIC, 
//                                TAXPER, DE_HEALTH, MONTHPAY_TAX, MONTHPAY, TAXCHILD, HEALPER_INSURANCE, HEALTHPAY, INSURANCEPAY,
//                                EXTRA01, EXTRA02, EXTRA03, EXTRA04, EXTRA05, 
//                                EXTRA06, EXTRA07, EXTRA08, EXTRA09, NOTE, LASTDA, 
//                                BOSSPAY, PROFESSPAY, TECHNICSPAY, BONUSPAY, WORKPPAY, ADVENTIVEPAY, FARPAY, ELECTRICPAY, SALARYPAY, CONCUR3PAY, ISOFFDUTY, OTHERADD)
//                                values( NEXT VALUE FOR dbo.B_BASE_SN, @IDCARD, @CONCUR3, @TECHNICS, @BONUS, @WORKP, @ADVENTIVE, @ELECTRIC, 
//                                @TAXPER, @DE_HEALTH, @MONTHPAY_TAX, @MONTHPAY, @TAXCHILD, @HEALPER_INSURANCE, @HEALTHPAY, @INSURANCEPAY,
//                                @EXTRA01, @EXTRA02, @EXTRA03, @EXTRA04, @EXTRA05, 
//                                @EXTRA06, @EXTRA07, @EXTRA08, @EXTRA09, @NOTE, @LASTDA, 
//                                @BOSSPAY, @PROFESSPAY, @TECHNICSPAY, @BONUSPAY, @WORKPPAY, @ADVENTIVEPAY, @FARPAY, @ELECTRICPAY, @SALARYPAY, @CONCUR3PAY, @ISOFFDUTY, @OTHERADD)";

//                    SqlCommand cmd = new SqlCommand(strSQL, InsertConn);
//                    cmd.CommandType = CommandType.Text;

//                    cmd.Parameters.Add("IDCARD", strIDCARD_Data);
//                    cmd.Parameters.Add("CONCUR3", intCONCUR3_Data);
//                    cmd.Parameters.Add("TECHNICS", strTECHNICS_Data);
//                    cmd.Parameters.Add("BONUS", strBONUS_Data);
//                    cmd.Parameters.Add("WORKP", strWORKP_Data);
//                    cmd.Parameters.Add("ADVENTIVE", strADVENTIVE_Data);
//                    //cmd.Parameters.Add("FAR", strFAR_Data);
//                    cmd.Parameters.Add("ELECTRIC", strELECTRIC_Data);
//                    cmd.Parameters.Add("TAXPER", intTAXPER_Data);
//                    cmd.Parameters.Add("DE_HEALTH", intDE_HEALTH);
//                    cmd.Parameters.Add("MONTHPAY_TAX", intMONTHPAY_TAX);
//                    cmd.Parameters.Add("MONTHPAY", intMONTHPAY);
//                    cmd.Parameters.Add("TAXCHILD", intTAXCHILD);
//                    cmd.Parameters.Add("HEALPER_INSURANCE", 100);
//                    cmd.Parameters.Add("HEALTHPAY", strHEALPER_INSURANCE);
//                    cmd.Parameters.Add("INSURANCEPAY", strLABOR_INSURANCE);
//                    cmd.Parameters.Add("EXTRA01", intEXTRA01_Data);
//                    cmd.Parameters.Add("EXTRA02", intEXTRA02_Data);
//                    cmd.Parameters.Add("EXTRA03", intEXTRA03_Data);
//                    cmd.Parameters.Add("EXTRA04", intEXTRA04_Data);
//                    cmd.Parameters.Add("EXTRA05", intEXTRA05_Data);
//                    cmd.Parameters.Add("EXTRA06", intEXTRA06_Data);
//                    cmd.Parameters.Add("EXTRA07", intEXTRA07_Data);
//                    cmd.Parameters.Add("EXTRA08", intEXTRA08_Data);
//                    cmd.Parameters.Add("EXTRA09", intEXTRA09_Data);
//                    cmd.Parameters.Add("NOTE", strNOTE_Data);
//                    cmd.Parameters.Add("LASTDA", DateTime.Now);
//                    cmd.Parameters.Add("BOSSPAY", intBoss);
//                    cmd.Parameters.Add("PROFESSPAY", intProfess);
//                    cmd.Parameters.Add("TECHNICSPAY", intTechnics);
//                    cmd.Parameters.Add("BONUSPAY", intBonus);
//                    cmd.Parameters.Add("WORKPPAY", intWorkp);
//                    cmd.Parameters.Add("ADVENTIVEPAY", intAdventive);
//                    cmd.Parameters.Add("FARPAY", intFar);
//                    cmd.Parameters.Add("ELECTRICPAY", intElectric);
//                    cmd.Parameters.Add("SALARYPAY", intSalary);
//                    cmd.Parameters.Add("CONCUR3PAY", intConcur3Pay);
//                    cmd.Parameters.Add("ISOFFDUTY", strIsOffDuty);
//                    cmd.Parameters.Add("OTHERADD", intOtherAdd);

//                    cmd.Connection = InsertConn;
//                    cmd.ExecuteNonQuery();
//                    return true;
//                }
//                catch { return false; }
//                finally { InsertConn.Close();
//                //XX2013/06/18 
//                InsertConn.Dispose();
//                }
//            }
//        }

//        /// <summary>
//        /// 個人薪資設定 - 修改
//        /// </summary>
//        /// <param name="strIDCARD_Data">身分證號</param>
//        /// <param name="intCONCUR3_Data">是否扣退撫金</param>
//        /// <param name="strTECHNICS_Data">技術加給</param>
//        /// <param name="strBONUS_Data">工作獎助金</param>
//        /// <param name="strWORKP_Data">警勤加給</param>
//        /// <param name="strADVENTIVE_Data">外事加給</param>
//        /// <param name="strELECTRIC_Data">水電補助</param>
//        /// <param name="strHEALPER_INSURANCE">健保費級距</param>
//        /// <param name="strLABOR_INSURANCE">勞保費級距</param>
//        /// <param name="intTAXPER_Data">所得稅扣款金額</param>
//        /// <param name="intDE_HEALTH">健保費扣款</param>
//        /// <param name="intMONTHPAY_TAX">薪資扣款(列入所得)</param>
//        /// <param name="intMONTHPAY">薪資扣款(不列入所得)</param>
//        /// <param name="intTAXCHILD">所得稅撫養人</param>
//        /// <param name="intEXTRA01_Data">法院扣款</param>
//        /// <param name="intEXTRA02_Data">國宅貸款</param>
//        /// <param name="intEXTRA03_Data">銀行貸款</param>
//        /// <param name="intEXTRA04_Data">分期付款</param>
//        /// <param name="intEXTRA05_Data">優惠存款</param>
//        /// <param name="intEXTRA06_Data">員工宿舍費</param>
//        /// <param name="intEXTRA07_Data">伙食費</param>
//        /// <param name="intEXTRA08_Data">福利互助金費</param>
//        /// <param name="intEXTRA09_Data">退撫金貸款</param>
//        /// <param name="strNOTE_Data">薪資備註</param>
//        /// <returns></returns>
//        public static bool boolSalaryPay_Set_Update(string strIDCARD_Data, int intCONCUR3_Data,
//            string strTECHNICS_Data, string strBONUS_Data, string strWORKP_Data,
//            string strADVENTIVE_Data, string strELECTRIC_Data,
//            int intTAXPER_Data, int intDE_HEALTH, int intMONTHPAY_TAX, int intMONTHPAY, int intTAXCHILD, string strHEALPER_INSURANCE, string strLABOR_INSURANCE,
//            int intEXTRA01_Data, int intEXTRA02_Data, int intEXTRA03_Data, int intEXTRA04_Data, int intEXTRA05_Data,
//            int intEXTRA06_Data, int intEXTRA07_Data, int intEXTRA08_Data, int intEXTRA09_Data,
//            string strNOTE_Data, int intBoss, int intProfess, int intTechnics, int intBonus, int intWorkp, int intAdventive, int intFar, int intElectric, int intSalary, int intConcur3Pay, string strIsOffduty, int intOtherAdd)
//        {
//            using (SqlConnection UpdateConn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
//            {
//                UpdateConn.Open();
//                try
//                {
//                    string strSQL;

//                    strSQL = @"update B_BASE set CONCUR3 = @CONCUR3, TECHNICS = @TECHNICS, BONUS = @BONUS, WORKP = @WORKP, ADVENTIVE = @ADVENTIVE, ELECTRIC = @ELECTRIC, 
//                                TAXPER = @TAXPER, DE_HEALTH = @DE_HEALTH, MONTHPAY_TAX = @MONTHPAY_TAX, MONTHPAY = @MONTHPAY, TAXCHILD = @TAXCHILD, HEALTHPAY = @HEALTHPAY, INSURANCEPAY = @INSURANCEPAY, 
//                                EXTRA01 = @EXTRA01, EXTRA02 = @EXTRA02, EXTRA03 = @EXTRA03, EXTRA04 = @EXTRA04, EXTRA05 = @EXTRA05, 
//                                EXTRA06 = @EXTRA06, EXTRA07 = @EXTRA07, EXTRA08 = @EXTRA08, EXTRA09 = @EXTRA09, NOTE = @NOTE, LASTDA = @LASTDA, 
//                                BOSSPAY = @BOSSPAY, PROFESSPAY = @PROFESSPAY, TECHNICSPAY = @TECHNICSPAY, BONUSPAY = @BONUSPAY, WORKPPAY = @WORKPPAY, ADVENTIVEPAY = @ADVENTIVEPAY, FARPAY = @FARPAY, ELECTRICPAY = @ELECTRICPAY, SALARYPAY = @SALARYPAY, CONCUR3PAY = @CONCUR3PAY, ISOFFDUTY=@ISOFFDUTY, OTHERADD=@OTHERADD
//                                where IDCARD = @IDCARD ";

//                    SqlCommand cmd = new SqlCommand(strSQL, UpdateConn);
//                    cmd.CommandType = CommandType.Text;

//                    cmd.Parameters.Add("CONCUR3", intCONCUR3_Data);
//                    cmd.Parameters.Add("TECHNICS", strTECHNICS_Data);
//                    cmd.Parameters.Add("BONUS", strBONUS_Data);
//                    cmd.Parameters.Add("WORKP", strWORKP_Data);
//                    cmd.Parameters.Add("ADVENTIVE", strADVENTIVE_Data);
//                    //cmd.Parameters.Add("FAR", strFAR_Data);
//                    cmd.Parameters.Add("ELECTRIC", strELECTRIC_Data);
//                    cmd.Parameters.Add("TAXPER", intTAXPER_Data);
//                    cmd.Parameters.Add("DE_HEALTH", intDE_HEALTH);
//                    cmd.Parameters.Add("MONTHPAY_TAX", intMONTHPAY_TAX);
//                    cmd.Parameters.Add("MONTHPAY", intMONTHPAY);
//                    cmd.Parameters.Add("TAXCHILD", intTAXCHILD);
//                    cmd.Parameters.Add("HEALTHPAY", strHEALPER_INSURANCE);
//                    cmd.Parameters.Add("INSURANCEPAY", strLABOR_INSURANCE);
//                    cmd.Parameters.Add("EXTRA01", intEXTRA01_Data);
//                    cmd.Parameters.Add("EXTRA02", intEXTRA02_Data);
//                    cmd.Parameters.Add("EXTRA03", intEXTRA03_Data);
//                    cmd.Parameters.Add("EXTRA04", intEXTRA04_Data);
//                    cmd.Parameters.Add("EXTRA05", intEXTRA05_Data);
//                    cmd.Parameters.Add("EXTRA06", intEXTRA06_Data);
//                    cmd.Parameters.Add("EXTRA07", intEXTRA07_Data);
//                    cmd.Parameters.Add("EXTRA08", intEXTRA08_Data);
//                    cmd.Parameters.Add("EXTRA09", intEXTRA09_Data);
//                    cmd.Parameters.Add("NOTE", strNOTE_Data);
//                    cmd.Parameters.Add("LASTDA", DateTime.Now);
//                    cmd.Parameters.Add("BOSSPAY", intBoss);
//                    cmd.Parameters.Add("PROFESSPAY", intProfess);
//                    cmd.Parameters.Add("TECHNICSPAY", intTechnics);
//                    cmd.Parameters.Add("BONUSPAY", intBonus);
//                    cmd.Parameters.Add("WORKPPAY", intWorkp);
//                    cmd.Parameters.Add("ADVENTIVEPAY", intAdventive);
//                    cmd.Parameters.Add("FARPAY", intFar);
//                    cmd.Parameters.Add("ELECTRICPAY", intElectric);
//                    cmd.Parameters.Add("SALARYPAY", intSalary);
//                    cmd.Parameters.Add("IDCARD", strIDCARD_Data);
//                    cmd.Parameters.Add("CONCUR3PAY", intConcur3Pay);
//                    cmd.Parameters.Add("ISOFFDUTY", strIsOffduty);
//                    cmd.Parameters.Add("OTHERADD", intOtherAdd);

//                    cmd.Connection = UpdateConn;
//                    cmd.ExecuteNonQuery();
//                    return true;
//                }
//                catch { return false; }
//                finally { UpdateConn.Close();
//                //XX2013/06/18 
//                UpdateConn.Dispose();
//                }
//            }
//        }

        ///// <summary>
        ///// 個人薪資設定 - 其他金額新增
        ///// </summary>
        ///// <param name="strIDCARD_Data">身分證號</param>
        ///// <param name="strS_SNID_Data">其他金額ID</param>
        ///// <returns></returns>
        //public static bool boolSalaryPay_Othe_Set_Create(string strIDCARD_Data, string strS_SNID_Data)
        //{
        //    using (SqlConnection InsertConn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
        //    {
        //        InsertConn.Open();
        //        try
        //        {
        //            string InsertString = "INSERT INTO B_BASE_OTHER (BO_SNID, IDCARD, S_SNID, LASTDA) "
        //            + "VALUES ( NEXT VALUE FOR dbo.B_BASE_SN, '" + strIDCARD_Data + "', '" + strS_SNID_Data + "', @LASTDA)";

        //            SqlCommand cmd = new SqlCommand(InsertString, InsertConn);
        //            cmd.CommandType = CommandType.Text;

        //            cmd.Parameters.Add("LASTDA", SqlDbType.DateTime).Value = DateTime.Now;
        //            cmd.Connection = InsertConn;
        //            cmd.ExecuteNonQuery();

        //            //新增成功後寫入LOG
        //            TPMPermissions._strEventData(HttpContext.Current.Session["ADServerID"].ToString(), "0", "EBXXX00002", TPMPermissions._boolTPM(), o_DBFactory.ABC_toTest.RegexSQL(cmd));

        //            return true;
        //        }
        //        catch { return false; }
        //        finally { InsertConn.Close();
        //        //XX2013/06/18 
        //        InsertConn.Dispose();
        //        }
        //    }
        //}

        ///// <summary>
        ///// 個人薪資設定 - 其他金額修改
        ///// </summary>
        ///// <param name="strBO_SNID_Data">個人薪資設定其他金額主編號</param>
        ///// <param name="strS_SNID_Data">其他金額ID</param>
        ///// <returns></returns>
        //public static bool boolSalaryPay_Othe_Set_Update(string strBO_SNID_Data, string strS_SNID_Data)
        //{
        //    using (SqlConnection UpdateConn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
        //    {
        //        UpdateConn.Open();
        //        try
        //        {
        //            string UpdateString = "UPDATE B_BASE_OTHER SET S_SNID = '" + strS_SNID_Data + "', LASTDA = @LASTDA "
        //            + " WHERE BO_SNID = '" + strBO_SNID_Data + "' ";

        //            SqlCommand cmd = new SqlCommand(UpdateString, UpdateConn);
        //            cmd.CommandType = CommandType.Text;

        //            cmd.Parameters.Add("LASTDA", SqlDbType.DateTime).Value = DateTime.Now;

        //            cmd.Connection = UpdateConn;
        //            cmd.ExecuteNonQuery();
        //            return true;
        //        }
        //        catch { return false; }
        //        finally { UpdateConn.Close();
        //        //XX2013/06/18 
        //        UpdateConn.Dispose();
        //        }
        //    }
        //}

        ///// <summary>
        ///// 個人薪資設定 - 其他金額刪除
        ///// </summary>
        ///// <param name="strBO_SNID_Data">個人薪資設定其他金額主編號</param>
        ///// <returns></returns>
        //public static bool boolSalaryPay_Othe_Set_Delete(string strBO_SNID_Data)
        //{
        //    using (SqlConnection DeleteConn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
        //    {
        //        DeleteConn.Open();
        //        string DeleteString = "DELETE B_BASE_OTHER WHERE BO_SNID = '" + strBO_SNID_Data + "'";
        //        SqlCommand cmd = new SqlCommand(DeleteString, DeleteConn);
        //        try
        //        {
        //            cmd.ExecuteNonQuery();
        //            return true;
        //        }
        //        catch { return false; }
        //        finally { DeleteConn.Close();
        //        //XX2013/06/18 
        //        DeleteConn.Dispose();
        //        }
        //    }
        //}

        /// <summary>
        /// 個人薪資設定 - 銀行帳號新增
        /// </summary>
        /// <param name="strIDCARD_Data">身分證號</param>
        /// <param name="strBANKID_Data">銀行代號</param>
        /// <param name="strSTOCKPILE_BANKID_Data">銀行帳號</param>
        /// <param name="intGROUP_Data">群組種類</param>
        /// <param name="strPAY_AD">發薪機關</param>
        /// <returns></returns>
        public static bool boolSalaryPay_Stockpile_Set_Create(string strIDCARD_Data, string strBANKID_Data, string strSTOCKPILE_BANKID_Data, int intGROUP_Data, string strPAY_AD)
        {
            using (SqlConnection InsertConn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                InsertConn.Open();
                try
                {
                    string InsertString = "INSERT INTO B_BASE_STOCKPILE (BS_SNID, IDCARD, BANKID, STOCKPILE_BANKID, \"GROUP\", PAY_AD, LASTDA) "
                    + "VALUES ( NEXT VALUE FOR dbo.B_BASE_STOCKPILE_SN, '" + strIDCARD_Data + "', '" + strBANKID_Data + "', '" + strSTOCKPILE_BANKID_Data + "', '" + intGROUP_Data + "', '" + strPAY_AD + "', @LASTDA)";

                    SqlCommand cmd = new SqlCommand(InsertString, InsertConn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("LASTDA", SqlDbType.DateTime).Value = DateTime.Now;

                    cmd.Connection = InsertConn;
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch { return false; }
                finally { InsertConn.Close();
                //XX2013/06/18 
                InsertConn.Dispose();
                }
            }
        }

        /// <summary>
        /// 個人薪資設定 - 銀行帳號修改
        /// </summary>
        /// <param name="strBS_SNID_Data">個人薪資設定銀行帳號主編號</param>
        /// <param name="strBANKID_Data">銀行代號</param>
        /// <param name="strSTOCKPILE_BANKID_Data">銀行帳號</param>
        /// <param name="intGROUP_Data">群組種類</param>
        /// <param name="strPAY_AD">發薪機關</param>
        /// <returns></returns>
        public static bool boolSalaryPay_Stockpile_Set_Update(string strBS_SNID_Data, string strBANKID_Data, string strSTOCKPILE_BANKID_Data, int intGROUP_Data, string strPAY_AD)
        {
            using (SqlConnection UpdateConn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                UpdateConn.Open();

                string UpdateString = "UPDATE B_BASE_STOCKPILE SET BANKID = '" + strBANKID_Data + "', "
                + " STOCKPILE_BANKID = '" + strSTOCKPILE_BANKID_Data + "', \"GROUP\" = '" + intGROUP_Data + "', PAY_AD = '" + strPAY_AD + "', LASTDA=@LASTDA"
                + " WHERE BS_SNID = '" + strBS_SNID_Data + "'";

                SqlCommand cmd = new SqlCommand(UpdateString, UpdateConn);
                cmd.Parameters.Add("LASTDA", SqlDbType.DateTime).Value = DateTime.Now;

                try
                {
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

        /// <summary>
        /// 個人薪資設定 - 銀行帳號刪除
        /// </summary>
        /// <param name="strBS_SNID_Data">個人薪資設定銀行帳號主編號</param>
        /// <returns></returns>
        public static bool boolSalaryPay_Stockpile_Set_Delete(string strBS_SNID_Data)
        {
            using (SqlConnection DeleteConn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                DeleteConn.Open();
                string DeleteString = "DELETE B_BASE_STOCKPILE WHERE BS_SNID = '" + strBS_SNID_Data + "'";
                SqlCommand cmd = new SqlCommand(DeleteString, DeleteConn);

                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
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
        /// 查詢薪俸年供俸 - 金額
        /// </summary>
        /// <param name="strS_SNID_Data">薪俸年供俸主編號</param>
        /// <returns></returns>
        public static int intOrigin_PAY_Data_Serach(int intS_SNID_Data)
        {
            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn.Open();
                try
                {
                    string strSQL = "SELECT PAY1 FROM B_SALARY WHERE S_SNID = '" + intS_SNID_Data + "'";
                    SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
                    DataTable dt = new DataTable();
                    dt.Load(Selectcmd.ExecuteReader(CommandBehavior.CloseConnection));
                    if (dt.Rows.Count == 1)
                    {
                        return int.Parse(dt.Rows[0]["PAY1"].ToString());
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

        ///// <summary>
        ///// 查詢退撫金費
        ///// </summary>
        ///// <param name="strS_SNID_Data">薪俸主編號</param>
        ///// <returns></returns>
        //public static int intConcr3_PAY_Data_Serach(int intS_SNID_Data)
        //{
        //    using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
        //    {
        //        Selectconn.Open();
        //        try
        //        {
        //            string strSQL = "SELECT CONCUR3 FROM B_SALARY WHERE S_SNID = '" + intS_SNID_Data + "'";
        //            SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
        //            DataTable dt = new DataTable();
        //            dt.Load(Selectcmd.ExecuteReader(CommandBehavior.CloseConnection));
        //            if (dt.Rows.Count == 1)
        //            {
        //                return int.Parse(dt.Rows[0]["CONCUR3"].ToString());
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
        ///// 查詢公保費
        ///// </summary>
        ///// <param name="intS_SNID_Data">薪俸主編號</param>
        ///// <returns></returns>
        //public static int intGOV_INSURANCE_PAY_Data_Serach(int intS_SNID_Data)
        //{
        //    using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
        //    {
        //        Selectconn.Open();
        //        try
        //        {
        //            string strSQL = "SELECT GOV_INSURANCE FROM B_SALARY WHERE S_SNID = '" + intS_SNID_Data + "'";
        //            SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
        //            DataTable dt = new DataTable();
        //            dt.Load(Selectcmd.ExecuteReader(CommandBehavior.CloseConnection));
        //            if (dt.Rows.Count == 1)
        //            {
        //                return int.Parse(dt.Rows[0]["GOV_INSURANCE"].ToString());
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
        /// 查詢警勤加給金額
        /// </summary>
        /// <param name="strWorkp_Data">警勤加給ID</param>
        /// <returns></returns>
        public static int intWorkp_PAY_Data_Serach(string strWorkp_Data)
        {
            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn.Open();
                try
                {
                    string strSQL = "SELECT \"ID\", PAY FROM B_WORKP WHERE \"ID\" = '" + strWorkp_Data + "'";
                    SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
                    DataTable dt = new DataTable();
                    dt.Load(Selectcmd.ExecuteReader(CommandBehavior.CloseConnection));
                    if (dt.Rows.Count == 1)
                    {
                        return int.Parse(dt.Rows[0]["PAY"].ToString());
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
        /// 查詢專業加給金額
        /// </summary>
        /// <param name="strProfess_Data">專業加給ID</param>
        /// <returns></returns>
        public static int intProfess_PAY_Data_Serach(string strProfess_Data)
        {
            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn.Open();
                try
                {
                    string strSQL = "SELECT \"ID\", PAY FROM B_PROFESS WHERE \"ID\" = '" + strProfess_Data + "01D" + "'";
                    SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
                    DataTable dt = new DataTable();
                    dt.Load(Selectcmd.ExecuteReader(CommandBehavior.CloseConnection));
                    if (dt.Rows.Count == 1)
                    {
                        return int.Parse(dt.Rows[0]["PAY"].ToString());
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
        /// 查詢鑑識人員專業加給金額
        /// </summary>
        /// <param name="strProfess_Data">專業加給ID</param>
        /// <returns></returns>
        public static int GetForensicPay(string strProfess_Data)
        {
            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn.Open();
                try
                {
                    string strSQL = "SELECT PAY FROM B_PROFESS WHERE \"ID\" = '" + strProfess_Data + "01D" + "'";
                    SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
                    DataTable dt = new DataTable();
                    dt.Load(Selectcmd.ExecuteReader(CommandBehavior.CloseConnection));
                    if (dt.Rows.Count == 1)
                    {
                        return int.Parse(dt.Rows[0]["PAY"].ToString());
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
        /// 查詢主管加給金額
        /// </summary>
        /// <param name="strBoss_Data">主管加給ID</param>
        /// <param name="boolChk_isBoss">主管加給(true為任主管，false為不任主管)</param>
        /// <returns></returns>
        public static int intBoss_PAY_Data_Serach(string strBoss_Data, bool boolChk_isBoss)
        {
            if (boolChk_isBoss)
            {
                using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
                {
                    Selectconn.Open();
                    try
                    {
                        string strSQL = "SELECT \"ID\", PAY FROM B_BOSS WHERE \"ID\" = '" + strBoss_Data + "01D" + "'";
                        SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
                        DataTable dt = new DataTable();
                        dt.Load(Selectcmd.ExecuteReader(CommandBehavior.CloseConnection));
                        if (dt.Rows.Count == 1)
                        {
                            return int.Parse(dt.Rows[0]["PAY"].ToString());
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
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 身分證查詢主管加給金額
        /// </summary>
        /// <param name="strMZ_ID">身分證號</param>
        /// <returns></returns>
        public static int intBossPAY_byID(string strMZ_ID)
        {
            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn.Open();
                try
                {
                    string strSQL = "SELECT \"ID\", PAY,MZ_PB2 FROM B_BOSS JOIN A_DLBASE ON B_BOSS.\"ID\" = A_DLBASE.MZ_SRANK+'01D' WHERE MZ_ID = '" + strMZ_ID + "'";
                    SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
                    DataTable dt = new DataTable();
                    dt.Load(Selectcmd.ExecuteReader(CommandBehavior.CloseConnection));
                    if (dt.Rows.Count == 1)
                    {
                        //主管加給(主管加給 / 12 * 任主管職期間)
                        return Convert.ToInt32(dt.Rows[0]["PAY"]) / 12 * Convert.ToInt16(dt.Rows[0]["MZ_PB2"]);//MZ_PB2為任主管期間
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
        /// 查詢工作獎助金金額
        /// </summary>
        /// <param name="strBonus_Data">工作獎助金ID</param>
        /// <returns></returns>
        public static int intBonus_PAY_Data_Serach(string strBonus_Data)
        {
            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn.Open();
                try
                {
                    string strSQL = "SELECT \"ID\", PAY FROM B_BONUS WHERE \"ID\" = '" + strBonus_Data + "'";
                    SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
                    DataTable dt = new DataTable();
                    dt.Load(Selectcmd.ExecuteReader(CommandBehavior.CloseConnection));
                    if (dt.Rows.Count == 1)
                    {
                        return int.Parse(dt.Rows[0]["PAY"].ToString());
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
        /// 查詢技術加給金額
        /// </summary>
        /// <param name="strTechnics_Data">技術加給ID</param>
        /// <returns></returns>
        public static int intTechnics_PAY_Data_Serach(string strTechnics_Data)
        {
            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn.Open();
                try
                {
                    string strSQL = "SELECT \"ID\", PAY FROM B_TECHNICS WHERE \"ID\" = '" + strTechnics_Data + "'";
                    SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
                    DataTable dt = new DataTable();
                    dt.Load(Selectcmd.ExecuteReader(CommandBehavior.CloseConnection));
                    if (dt.Rows.Count == 1)
                    {
                        return int.Parse(dt.Rows[0]["PAY"].ToString());
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
        /// 查詢外事加給金額
        /// </summary>
        /// <param name="strAdventivet_Data">外事加給ID</param>
        /// <returns></returns>
        public static int intAdventivet_PAY_Data_Serach(string strAdventivet_Data)
        {
            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn.Open();
                try
                {
                    string strSQL = "SELECT \"ID\", PAY FROM B_ADVENTIVE WHERE \"ID\" = '" + strAdventivet_Data + "'";
                    SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
                    DataTable dt = new DataTable();
                    dt.Load(Selectcmd.ExecuteReader(CommandBehavior.CloseConnection));
                    if (dt.Rows.Count == 1)
                    {
                        return int.Parse(dt.Rows[0]["PAY"].ToString());
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

        ///// <summary>
        ///// 查詢偏遠加給與山地加給金額
        ///// </summary>
        ///// <param name="strFar_Data">偏遠加給ID</param>
        ///// <returns></returns>
        //public static int intFar_PAY_Data_Serach(string strFar_Data)
        //{
        //    using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
        //    {
        //        Selectconn.Open();
        //        try
        //        {
        //            string strSQL = "SELECT \"ID\", PAY FROM B_FAR WHERE \"ID\" = '" + strFar_Data + "'";
        //            SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
        //            DataTable dt = new DataTable();
        //            dt.Load(Selectcmd.ExecuteReader(CommandBehavior.CloseConnection));
        //            if (dt.Rows.Count == 1)
        //            {
        //                return int.Parse(dt.Rows[0]["PAY"].ToString());
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
        /// 查詢水電補助金額
        /// </summary>
        /// <param name="strElectric_Data">水電補助ID</param>
        /// <returns></returns>
        public static int intElectric_PAY_Data_Serach(string strElectric_Data)
        {
            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn.Open();
                try
                {
                    string strSQL = "SELECT \"ID\", PAY FROM B_ELECTRIC WHERE \"ID\" = '" + strElectric_Data + "'";
                    SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
                    DataTable dt = new DataTable();
                    dt.Load(Selectcmd.ExecuteReader(CommandBehavior.CloseConnection));
                    if (dt.Rows.Count == 1)
                    {
                        return int.Parse(dt.Rows[0]["PAY"].ToString());
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
        /// 計算其他金額總計
        /// </summary>
        public static int intOrthe_Increase_Data = 0;

        /// <summary>
        /// 查詢個人其他金額 - 應發金額
        /// </summary>
        /// <param name="strIDCARD">身分證號</param>
        /// <returns></returns>
        public static bool boolCHK_ORTHE_Increase_Data(string strIDCARD)
        {
            intOrthe_Increase_Data = 0;
            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn.Open();
                try
                {
                    string strSQL = "SELECT PAY FROM B_BASE_OTHER,B_SALARY_2 WHERE B_BASE_OTHER.S_SNID = B_SALARY_2.\"ID\" AND IDCARD = '" + strIDCARD + "' AND MP = 'P'";
                    SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
                    DataTable dt = new DataTable();
                    dt.Load(Selectcmd.ExecuteReader(CommandBehavior.CloseConnection));
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow item in dt.Rows)
                        {
                            intOrthe_Increase_Data = intOrthe_Increase_Data + int.Parse(item["PAY"].ToString());
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
        /// 判斷銀行資料不得重複
        /// </summary>
        /// <param name="strMZ_ID">身分證號</param>
        /// <param name="strGROUP_ID">群組代碼</param>
        /// <param name="strPAY_AD">發薪機關</param>
        /// <returns></returns>
        public static bool boolCHK_STOCKPILE_Data(string strMZ_ID, string strGROUP_ID, string strPAY_AD)
        {
            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn.Open();
                try
                {
                    string strSQL = string.Format("SELECT IDCARD FROM B_BASE_STOCKPILE WHERE IDCARD = '{0}' AND \"GROUP\" = '{1}' AND PAY_AD = '{2}'", strMZ_ID, strGROUP_ID, strPAY_AD);
                    SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
                    DataTable dt = new DataTable();
                    dt.Load(Selectcmd.ExecuteReader(CommandBehavior.CloseConnection));
                    if (dt.Rows.Count <= 0)
                    {
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
        /// 判斷職務資料是否為公務人員、警察人員
        /// </summary>
        /// <param name="strMZ_ID">身分證號</param>
        /// <returns></returns>
        public static bool boolCHK_MZ_OCCC_Data_Serach(string strMZ_ID)
        {
            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn.Open();
                try
                {
                    string strSQL = "SELECT MZ_OCCC FROM A_DLBASE WHERE MZ_ID = '" + strMZ_ID + "'";
                    SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
                    DataTable dt = new DataTable();
                    dt.Load(Selectcmd.ExecuteReader());
                    string strMZ_OCCC = dt.Rows[0]["MZ_OCCC"].ToString();
                    switch (strMZ_OCCC)
                    {
                        case "0011":
                        case "0013":
                        case "0014":
                        case "0015":
                        case "0016":
                            return false;
                        default:
                            return true;
                    }
                }
                catch { throw; }
                finally { Selectconn.Close();

                //XX2013/06/18 
                Selectconn.Dispose();
                }
            }
        }

        ///// <summary>
        ///// 查詢是否扣款，法院扣款
        ///// </summary>
        ///// <param name="strMZ_ID">身分證號</param>
        ///// <returns></returns>
        //public static bool boolCHK_EXTRA01_Data_Data_Serach(string strMZ_ID)
        //{
        //    using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
        //    {
        //        Selectconn.Open();
        //        try
        //        {
        //            string strSQL = "SELECT EXTRA01 FROM B_BASE WHERE IDCARD = '" + strMZ_ID + "' AND EXTRA01 > 0";
        //            SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
        //            DataTable dt = new DataTable();
        //            dt.Load(Selectcmd.ExecuteReader(CommandBehavior.CloseConnection));
        //            if (dt.Rows.Count == 1)
        //            {
        //                return true;
        //            }
        //            else
        //            {
        //                return false;
        //            }
        //        }
        //        catch { throw; }
        //        finally { Selectconn.Close();

        //        //XX2013/06/18 
        //        Selectconn.Dispose();
        //        }
        //    }
        //}

        // 取得初任公職日
        public static DateTime GetFirstGovDate(string idno)
        {
            string strSQL;

            strSQL = string.Format("SELECT CASE WHEN MZ_FDATE IS NULL THEN '1000101' ELSE MZ_FDATE END MZ_FDATE FROM A_DLBASE WHERE MZ_ID='{0}'", idno);

            return DateTime.Parse(SalaryPublic.ToADDateWithDash(o_DBFactory.ABC_toTest.vExecSQL(strSQL)));
        }

        // 建立薪資資料(員警第一次)
        public static void CreateSalaryData(string idno)
        {
            string strSQL;

            strSQL = string.Format("INSERT INTO B_BASE (B_SNID, IDCARD) VALUES( NEXT VALUE FOR dbo.B_BASE_SN, '{0}')", idno);

            o_DBFactory.ABC_toTest.Edit_Data(strSQL);
        }

        // 是否已有薪資資料
        public static bool HasSalary(string idno)
        {
            string strSQL;

            strSQL = string.Format("SELECT COUNT(*) FROM B_BASE WHERE IDCARD='{0}'", idno);

            if (o_DBFactory.ABC_toTest.vExecSQL(strSQL) == "")
                return false;

            return true;
        }

        //// 取得其他應發
        //public static int GetOtherAdd(string strIDCard)
        //{
        //    string strSQL;
        //    int result;

        //    strSQL = string.Format("SELECT OTHERADD FROM B_BASE WHERE IDCARD='{0}'", strIDCard);

        //    int.TryParse(o_DBFactory.ABC_toTest.vExecSQL(strSQL), out result);

        //    return result;
        //}

        //// 是否停職
        //public static string GetIsOffDuty(string strIDCard)
        //{
        //    string strSQL;

        //    strSQL = string.Format("SELECT ISOFFDUTY FROM B_BASE WHERE IDCARD='{0}'", strIDCard);

        //    return o_DBFactory.ABC_toTest.vExecSQL(strSQL);
        //}

        // 計算主管加給金額
        public static int CaculateBossPay(string strIDCard)
        {
            DataTable dtDLBASE = new DataTable();
            string strSQL = string.Format("select MZ_EXTPOS_SRANK, MZ_PCHIEF, MZ_SRANK from A_DLBASE where MZ_ID = '{0}'", strIDCard);

            dtDLBASE = o_DBFactory.ABC_toTest.Create_Table(strSQL, "DLBASE");

            // 有兼任主管以兼任的職等對照主管加給表，否則以本身職等對照主管加給表
            if (dtDLBASE.Rows[0]["MZ_EXTPOS_SRANK"] != null && dtDLBASE.Rows[0]["MZ_EXTPOS_SRANK"].ToString() != "")
            {
                return SalaryBasic.intBoss_PAY_Data_Serach(dtDLBASE.Rows[0]["MZ_EXTPOS_SRANK"].ToString(), true);
            }
            else if (dtDLBASE.Rows[0]["MZ_PCHIEF"] != null && dtDLBASE.Rows[0]["MZ_PCHIEF"].ToString() != "")
            {
                return SalaryBasic.intBoss_PAY_Data_Serach(dtDLBASE.Rows[0]["MZ_SRANK"].ToString(), true);
            }

            return 0;
        }
        //// 依身份證字號取得主管加給金額
        //public static int GetBossbyIDCard(string strIDCard)
        //{
        //    string strSQL;
        //    int intBoss;
        //    DataTable dt = new DataTable();

        //    strSQL = string.Format("select ISBOSS, BOSSPAY from B_BASE where IDCard = '{0}'", strIDCard);
        //    dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "B_BASECount");

        //    // B_BASE中沒有資料的話，預設為非主管
        //    if (dt.Rows.Count == 0)
        //        return 0;

        //    // 非主管
        //    if (dt.Rows[0]["ISBOSS"] == null || dt.Rows[0]["ISBOSS"].ToString() == "0")
        //        return 0;

        //    // 是主管，卻沒有主管加給資料，再計算一次
        //    if (dt.Rows[0]["BOSSPAY"] == null || dt.Rows[0]["BOSSPAY"].ToString() == "")
        //        return CaculateBossPay(strIDCard);

        //    int.TryParse(dt.Rows[0]["BOSSPAY"].ToString(), out intBoss);

        //    return intBoss;
        //}

        // 計算專業加給
        public static int CaculateProfessPay(string strIDCard)
        {
            DataTable dtDLBASE = new DataTable();
            DataTable dt = new DataTable();

            string strSQL = string.Format("select MZ_SRANK from A_DLBASE where MZ_ID = '{0}'", strIDCard);
            dtDLBASE = o_DBFactory.ABC_toTest.Create_Table(strSQL, "DLBASE");

            strSQL = string.Format("select TECHNICSPAY from B_BASE where IDCARD = '{0}'", strIDCard); ;
            dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "TECHNICSPAY");

            if (dt.Rows.Count == 0)
                return SalaryBasic.intProfess_PAY_Data_Serach(dtDLBASE.Rows[0]["MZ_SRANK"].ToString());

            // 鑑識人員的專業加給對照[B_PROFESS]資料表中的[PAY]欄位
            // 如果有技術加給，就是鑑識人員
            if (dt.Rows[0]["TECHNICSPAY"] == null || dt.Rows[0]["TECHNICSPAY"].ToString() == "" || dt.Rows[0]["TECHNICSPAY"].ToString() == "0")
            {
                return SalaryBasic.intProfess_PAY_Data_Serach(dtDLBASE.Rows[0]["MZ_SRANK"].ToString());
            }
            return SalaryBasic.GetForensicPay(dtDLBASE.Rows[0]["MZ_SRANK"].ToString());
        }
        // 依身份證字號取得專業加給金額
        public static int GetProfessbyIDCard(string strIDCard)
        {
            string strSQL;
            int intBoss;
            DataTable dt = new DataTable();

            strSQL = string.Format("select TECHNICSPAY, PROFESSPAY from B_BASE where IDCard = '{0}'", strIDCard);
            dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "B_BASECount");

            // B_BASE中沒有資料的話，就用計算的；否則直接取B_BASE中的資料
            if (dt.Rows.Count == 0 || dt.Rows[0]["PROFESSPAY"] == null || dt.Rows[0]["PROFESSPAY"].ToString() == "")
            {
                return CaculateProfessPay(strIDCard);
            }

            int.TryParse(dt.Rows[0]["PROFESSPAY"].ToString(), out intBoss);

            return intBoss;
        }

        // 計算公(勞)保費
        /* 這段已經被廢止了
        public static int CaculateInsurancePay(string strIDCard, bool Is30)
        {
            string strSQL;
            int intHealthPay;
            int intSALARY_int1_2;
            string strINSURANCE_GROUP;
            string strSALARY_NAME1;
            string strSPT;
            DataTable dtDLBASE = new DataTable();
            DataTable dtInsurance = new DataTable();

            // 任公職滿30年不用公保費
            if (Is30)
                return 0;

            strSQL = string.Format("select MZ_OCCC, MZ_SRANK, MZ_SPT from A_DLBASE where MZ_ID = '{0}'", strIDCard);
            dtDLBASE = o_DBFactory.ABC_toTest.Create_Table(strSQL, "DLBASE");

            if (dtDLBASE.Rows.Count == 0)
            {
                return 0;
            }

            //依職稱取得對照B_SALARY中的NAME1群組
            strSALARY_NAME1 = SalaryPublic.getSalaryName1(dtDLBASE.Rows[0]["MZ_OCCC"].ToString());
            //依職稱取得保險種類(公保/勞保)
            strINSURANCE_GROUP = SalaryPublic.getInsuranceGroup(dtDLBASE.Rows[0]["MZ_OCCC"].ToString());
            intSALARY_int1_2 = SalaryPublic.getSalaryOriginType(dtDLBASE.Rows[0]["MZ_SRANK"].ToString());

            strSPT = dtDLBASE.Rows[0]["MZ_SPT"].ToString().PadLeft(4, '0');

            //若MZ_SRANK職等為G開頭，表示警察人員，採用B_SALARY中的ORIGIN2薪級比對級距金額
            //其餘採用B_SALARY中的ORIGIN1俸點比對級距金額
            if (intSALARY_int1_2 == 2)
            {
                strSQL = string.Format("SELECT INSURANCE FROM B_SALARY WHERE NAME1 = '{0}' AND ORIGIN2 = '{1}'", strSALARY_NAME1, strSPT);
            }
            else
            {
                strSQL = string.Format("SELECT INSURANCE FROM B_SALARY WHERE NAME1 = '{0}' AND ORIGIN1 = '{1}'", strSALARY_NAME1, strSPT);
            }

            dtInsurance = o_DBFactory.ABC_toTest.Create_Table(strSQL, "InsurancePay");

            if (dtInsurance.Rows.Count == 0)
                return 0;

            int.TryParse(dtInsurance.Rows[0]["INSURANCE"].ToString(), out intHealthPay);

            return intHealthPay;
        }
        */

        //public static int CaculateInsurancePay(string occc, string srank, string spt)
        //{
        //    DataTable dtInsurance = new DataTable();
        //    string strSQL;
        //    int intSALARY_int1_2;
        //    string strINSURANCE_GROUP;
        //    string strSALARY_NAME1;
        //    int intResult;

        //    //依職稱取得對照B_SALARY中的NAME1群組
        //    strSALARY_NAME1 = SalaryPublic.getSalaryName1(occc);
        //    //依職稱取得保險種類(公保/勞保)
        //    strINSURANCE_GROUP = SalaryPublic.getInsuranceGroup(occc);
        //    intSALARY_int1_2 = SalaryPublic.getSalaryOriginType(srank);

        //    spt = spt.PadLeft(4, '0');

        //    //若MZ_SRANK職等為G開頭，表示警察人員，採用B_SALARY中的ORIGIN2薪級比對級距金額
        //    //其餘採用B_SALARY中的ORIGIN1俸點比對級距金額
        //    if (intSALARY_int1_2 == 2)
        //    {
        //        strSQL = string.Format("SELECT INSURANCE FROM B_SALARY WHERE NAME1 = '{0}' AND ORIGIN2 = '{1}'", strSALARY_NAME1, spt);
        //    }
        //    else
        //    {
        //        strSQL = string.Format("SELECT INSURANCE FROM B_SALARY WHERE NAME1 = '{0}' AND ORIGIN1 = '{1}'", strSALARY_NAME1, spt);
        //    }

        //    dtInsurance = o_DBFactory.ABC_toTest.Create_Table(strSQL, "InsurancePay");

        //    if (dtInsurance.Rows.Count == 0)
        //        return 0;

        //    int.TryParse(dtInsurance.Rows[0]["INSURANCE"].ToString(), out intResult);

        //    return intResult;
        //}
        //// 依身份證字號取得公(勞)保費
        //public static int GetInsurancePaybyIDCard(string strIDCard)
        //{
        //    string strSQL;
        //    int intHealthPay;

        //    DataTable dt = new DataTable();

        //    strSQL = string.Format("select INSURANCEPAY, IS30YEAR from B_BASE where IDCard = '{0}'", strIDCard);
        //    dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "INSURANCEPAY");

        //    if (dt.Rows.Count == 0)
        //        return 0;

        //    // B_BASE中沒有資料的話，就用計算的；否則直接取B_BASE中的資料
        //    if (dt.Rows[0]["INSURANCEPAY"] == null || dt.Rows[0]["INSURANCEPAY"].ToString() == "")
        //    {
        //        return CaculateInsurancePay(strIDCard, dt.Rows[0]["IS30YEAR"].ToString() == "是" ? true : false);
        //    }

        //    int.TryParse(dt.Rows[0]["INSURANCEPAY"].ToString(), out intHealthPay);

        //    return intHealthPay;
        //}

        // 計算健保年功俸(不含眷屬)
        public static int CaculateHealthID(int intSalary, int intBoss, int intProfess, int intWorkp, int intTechnics, int intBonus, int intAdventive, int intFar, int intElectric, bool isGov)
        {
            string strSQL;
            string strResult;
            int intResult;
            int intTotal = 0;

            intTotal = intSalary + intBoss + intProfess + intWorkp + intTechnics + intBonus + intAdventive + intFar + intElectric;

            if (isGov)
                intTotal = (int)Math.Floor(intTotal * 0.9352);

            strSQL = string.Format("SELECT INSURANCE FROM B_HEALTH_INSURANCE WHERE PAY1 <= {0} AND {0} <= PAY2", intTotal);

            strResult = o_DBFactory.ABC_toTest.vExecSQL(strSQL);
            int.TryParse(strResult, out intResult);

            return intResult;
        }
        // 取得各健保狀態之人數並加入至參考之List中
        public static void GetInsuranceMemberCount(ref List<int> lstMember, string strMZ_ID, int intInsuranceMode)
        {
            //健保模式為0則跳出此函式
            if (intInsuranceMode == 0)
                return;

            int intMemberCount;

            //取得InsuranceMode的加保人數
            intMemberCount = int.Parse(o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_FAMILY WHERE MZ_ID='" + strMZ_ID + "' AND MZ_ISINSURANCE='Y' AND MZ_INSURANCEMODE='" + intInsuranceMode + "'"));

            //將加保人數加入至lstMember中
            lstMember.Add(intMemberCount);

            //遞迴，每次將健保模式-1
            GetInsuranceMemberCount(ref lstMember, strMZ_ID, intInsuranceMode - 1);
        }
        //// 取得眷屬加保清單
        //public static List<int> GetInsuranceList(string strMZ_ID)
        //{
        //    //存放4種健保狀態人數之List，lstMember[3]為健保狀態1、lstMember[2]為健保狀態2、lstMember[1]為健保狀態3、lstMember[0]為健保狀態4
        //    List<int> lstMember = new List<int>();

        //    //個人健保狀態
        //    string strInsuranceSelf;

        //    //取得健保狀態，lstMember[3]為100%、lstMember[2]為75%、lstMember[1]為50%、lstMember[0]為0%
        //    GetInsuranceMemberCount(ref lstMember, strMZ_ID, 4);
        //    //取得個人健保狀態
        //    strInsuranceSelf = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_INSURANCEMODE FROM A_DLBASE WHERE MZ_ID='" + strMZ_ID + "'");
        //    //加入至對應的lstMember位置中，1為100%、2為75%、3為50%、4為0%
        //    switch (strInsuranceSelf)
        //    {
        //        case "1":
        //            lstMember[3]++;
        //            break;
        //        case "2":
        //            lstMember[2]++;
        //            break;
        //        case "3":
        //            lstMember[1]++;
        //            break;
        //        case "4":
        //            lstMember[0]++;
        //            break;
        //        default:
        //            lstMember[3]++;
        //            break;
        //    }

        //    return lstMember;
        //}
        //// 取得健保費(個人)
        //public static int GetHealthID(string strIDCard)
        //{
        //    string strSQL;
        //    string strResult;
        //    int intResult;

        //    strSQL = string.Format("SELECT PERSONALHEALTHPAY FROM B_BASE WHERE IDCARD='{0}'", strIDCard);

        //    strResult = o_DBFactory.ABC_toTest.vExecSQL(strSQL);
        //    int.TryParse(strResult, out intResult);

        //    return intResult;
        //}
        //// 計算健保費(含眷屬加保)
        //public static int CaculateHealthPay(int intHealthID, List<int> lstMember)
        //{
        //    // 4種健保狀態對應之計算百分比
        //    double[] arrPercent = { 0.0, 0.5, 0.75, 1.0 };
        //    // 最多計算至4人，多於4人取金額最少之4人算法
        //    int intMaxMemberCount = 4;

        //    int intTotalInsurance = 0;

        //    //取總金額最少之算法
        //    for (int i = 0; i < 4; i++)
        //    {
        //        //已計算人數達4人則跳出迴圈
        //        if (intMaxMemberCount <= 0)
        //            break;

        //        //如果人數大於目前需要之人數以目前需要之人數計算
        //        if (lstMember[i] > intMaxMemberCount)
        //        {
        //            intTotalInsurance += Convert.ToInt32(Math.Floor(intMaxMemberCount * arrPercent[i] * intHealthID));
        //        }
        //        else//否則以lstMember[i]中之人數計算
        //        {
        //            intTotalInsurance += Convert.ToInt32(Math.Floor(lstMember[i] * arrPercent[i]) * intHealthID);
        //        }

        //        //扣除已計算之人數
        //        intMaxMemberCount -= lstMember[i];
        //    }

        //    return intTotalInsurance;
        //}
        public static int CaculateHealthPay(string strIDCard, int intHealthID, bool Is30)
        {
            DataTable dt;
            string sql;
            int result;

            sql = string.Format("SELECT * FROM B_HEALTH_INFORMATION WHERE IDCARD='{0}' ORDER BY COST", strIDCard);
            dt = o_DBFactory.ABC_toTest.Create_Table(sql, "tag");
            result = 0;

            int i = 0;
            foreach (DataRow item in dt.Rows)
            {
                if (i == 3)
                    break;

                int cost;

                int.TryParse(item["COST"].ToString(), out cost);
                result += cost;
                i++;
            }

            result += intHealthID;

            // 任公職滿30年不用算本人的健保費
            if (Is30)
                result -= intHealthID;

            return result;
        }
        //public static int CaculateHealthPay(DataTable healthInfo, int intHealthID, bool Is30)
        //{
        //    int result;

        //    result = 0;

        //    int i = 0;
        //    foreach (DataRow item in healthInfo.Rows)
        //    {
        //        if (i == 3)
        //            break;

        //        int cost;

        //        int.TryParse(item["COST"].ToString(), out cost);
        //        result += cost;
        //        i++;
        //    }

        //    result += intHealthID;

        //    // 任公職滿30年不用算本人的健保費
        //    if (Is30)
        //        result -= intHealthID;

        //    return result;
        //}
        // 計算健保費(給人事更新資料時重新計算健保費用的)
        public static int CaculateHealthPay(string strIDCard, int intSalary, int intBoss, int intProfess)
        {
            //SalaryPublic SalaryPublic = new SalaryPublic();

            //return SalaryPublic.getInsurancebyMZ_ID(strIDCard);

            bool isGov;
            int intHealthID;
            int intTotalHealth;
            string strSQL;
            DataTable dt = new DataTable();

            strSQL = string.Format("SELECT LABOR_INSURANCE, WORKPPAY, TECHNICSPAY, BONUSPAY, ADVENTIVEPAY, FARPAY, ELECTRICPAY, IS30YEAR FROM B_BASE WHERE IDCARD = '{0}'", strIDCard);
            dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "Base");

            if (dt.Rows[0]["LABOR_INSURANCE"].ToString() == "1")
                isGov = true;
            else
                isGov = false;

            intHealthID = SalaryBasic.CaculateHealthID(intSalary, intBoss, intProfess, Convert.ToInt32(dt.Rows[0]["WORKPPAY"]), Convert.ToInt32(dt.Rows[0]["TECHNICSPAY"]), Convert.ToInt32(dt.Rows[0]["BONUSPAY"]), Convert.ToInt32(dt.Rows[0]["ADVENTIVEPAY"]), Convert.ToInt32(dt.Rows[0]["FARPAY"]), Convert.ToInt32(dt.Rows[0]["ELECTRICPAY"]), isGov);
            intTotalHealth = SalaryBasic.CaculateHealthPay(strIDCard, intHealthID, dt.Rows[0]["IS30YEAR"].ToString() == "1" ? true : false);

            return intTotalHealth;
        }
        //// 依身份證字號取得健保費
        //public static int GetHealthPaybyIDCard(string strIDCard)
        //{
        //    string strSQL;
        //    int intHealthPay;
        //    DataTable dt = new DataTable();

        //    strSQL = string.Format("select HEALTHPAY from B_BASE where IDCard = '{0}'", strIDCard);
        //    dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "HEALTHPAY");

        //    // B_BASE中沒有資料的話，就用計算的；否則直接取B_BASE中的資料
        //    if (dt.Rows.Count == 0 || dt.Rows[0]["HEALTHPAY"] == null || dt.Rows[0]["HEALTHPAY"].ToString() == "")
        //    {
        //        SalaryPublic SalaryPublic = new SalaryPublic();

        //        return CaculateHealthPay(GetHealthID(strIDCard), GetInsuranceList(strIDCard));
        //    }

        //    int.TryParse(dt.Rows[0]["HEALTHPAY"].ToString(), out intHealthPay);

        //    return intHealthPay;
        //}
        //// 取得健保加保人數
        //public static int GetHealthMan(string strIDCard)
        //{
        //    int result;
        //    string strSQL;

        //    strSQL = string.Format("SELECT COUNT(*) FROM B_HEALTH_INFORMATION WHERE IDCARD='{0}'", strIDCard);

        //    int.TryParse(o_DBFactory.ABC_toTest.vExecSQL(strSQL), out result);

        //    return result;
        //}

        //// 計算底薪
        //public static int CaculateSalaryPay(string occc, string srank, string spt)
        //{
        //    //依職稱取得對照B_SALARY中的NAME1群組
        //    int intSALARY_NAME1 = int.Parse(SalaryPublic.getSalaryName1(occc));

        //    //若MZ_SRANK職等為G開頭，表示警察人員，採用B_SALARY中的ORIGIN2薪級比對級距金額
        //    //其餘採用B_SALARY中的ORIGIN1俸點比對級距金額
        //    int intSALARY_int1_2 = SalaryPublic.getSalaryOriginType(srank);

        //    int intS_SNID = SalaryPublic.intSALARY_ID_Data_Serach(intSALARY_NAME1, spt, intSALARY_int1_2);
        //    int intSalaryPay = SalaryMonth.intOrigin_PAY_Data_Serach(intS_SNID);

        //    return intSalaryPay;
        //}


        public static int CaculateSalaryPay(string strIDCard)
        {
            //取得職稱、現俸職等、俸點資料
            DataTable dtBase = o_DBFactory.ABC_toTest.Create_Table(string.Format("SELECT mz_occc, MZ_SRANK, MZ_SPT, MZ_SPT1, mz_pchief FROM A_DLBASE where MZ_ID = '{0}'", strIDCard), "B_Base");

            //如果有資料才計算底薪，否則傳回0
            if (dtBase.Rows.Count > 0)
            {
                string strMZ_OCCC = dtBase.Rows[0]["MZ_OCCC"].ToString().Trim();
                string strMZ_SRANK = dtBase.Rows[0]["MZ_SRANK"].ToString().Trim();
                string strMZ_SPT;

                // 有暫之俸點的話以暫之俸點(MZ_SPT1)計算；沒有的話才用俸點(MZ_SPT)
                if (dtBase.Rows[0]["MZ_SPT1"] == null || dtBase.Rows[0]["MZ_SPT1"].ToString() == "")
                    strMZ_SPT = dtBase.Rows[0]["MZ_SPT"].ToString().Trim();
                else
                    strMZ_SPT = dtBase.Rows[0]["MZ_SPT1"].ToString().Trim();

                //依職稱取得對照B_SALARY中的NAME1群組
                int intSALARY_NAME1 = int.Parse(SalaryPublic.getSalaryName1(strMZ_OCCC));

                //若MZ_SRANK職等為G開頭，表示警察人員，採用B_SALARY中的ORIGIN2薪級比對級距金額
                //其餘採用B_SALARY中的ORIGIN1俸點比對級距金額
                int intSALARY_int1_2 = SalaryPublic.getSalaryOriginType(strMZ_SRANK);

                int intS_SNID = SalaryPublic.intSALARY_ID_Data_Serach(intSALARY_NAME1, strMZ_SPT, intSALARY_int1_2);
                int intSalaryPay = SalaryMonth.intOrigin_PAY_Data_Serach(intS_SNID);

                return intSalaryPay;
            }

            return 0;
        }
        // 依身分證字號取得底薪
        public static int GetSalaryPaybyIDCard(string strIDCard)
        {
            string strSQL;
            int intPay;
            DataTable dt = new DataTable();

            strSQL = string.Format("select SALARYPAY from B_BASE where IDCard = '{0}'", strIDCard);
            dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "B_BASECount");

            // B_BASE中沒有資料的話，就用計算的；否則直接取B_BASE中的資料
            if (dt.Rows.Count == 0 || dt.Rows[0]["SALARYPAY"] == null || dt.Rows[0]["SALARYPAY"].ToString() == "")
            {
                return CaculateSalaryPay(strIDCard);
            }

            int.TryParse(dt.Rows[0]["SALARYPAY"].ToString(), out intPay);

            return intPay;
        }

        //// 計算退撫金費
        //public static int CaculateConcur3Pay(string occc, string srank, string spt)
        //{
        //    //依職稱取得對照B_SALARY中的NAME1群組
        //    int intSALARY_NAME1 = int.Parse(SalaryPublic.getSalaryName1(occc));

        //    //若MZ_SRANK職等為G開頭，表示警察人員，採用B_SALARY中的ORIGIN2薪級比對級距金額
        //    //其餘採用B_SALARY中的ORIGIN1俸點比對級距金額
        //    int intSALARY_int1_2 = SalaryPublic.getSalaryOriginType(srank);

        //    int intS_SNID = SalaryPublic.intSALARY_ID_Data_Serach(intSALARY_NAME1, spt, intSALARY_int1_2);
        //    int intPay = SalaryBasic.intConcr3_PAY_Data_Serach(intS_SNID);

        //    return intPay;
        //}

        //public static int CaculateConcur3Pay(string strIDCard)
        //{
        //    //取得職稱、現俸職等、俸點資料
        //    DataTable dtBase = o_DBFactory.ABC_toTest.Create_Table(string.Format("SELECT mz_occc, MZ_SRANK, MZ_SPT, MZ_SPT1, mz_pchief FROM A_DLBASE where MZ_ID = '{0}'", strIDCard), "B_Base");

        //    //如果有資料才計算底薪，否則傳回0
        //    if (dtBase.Rows.Count > 0)
        //    {
        //        string strMZ_OCCC = dtBase.Rows[0]["MZ_OCCC"].ToString().Trim();
        //        string strMZ_SRANK = dtBase.Rows[0]["MZ_SRANK"].ToString().Trim();
        //        string strMZ_SPT;

        //        // 有暫之俸點的話以暫之俸點(MZ_SPT1)計算；沒有的話才用俸點(MZ_SPT)
        //        if (dtBase.Rows[0]["MZ_SPT1"] == null || dtBase.Rows[0]["MZ_SPT1"].ToString() == "")
        //            strMZ_SPT = dtBase.Rows[0]["MZ_SPT"].ToString().Trim();
        //        else
        //            strMZ_SPT = dtBase.Rows[0]["MZ_SPT1"].ToString().Trim();

        //        //依職稱取得對照B_SALARY中的NAME1群組
        //        int intSALARY_NAME1 = int.Parse(SalaryPublic.getSalaryName1(strMZ_OCCC));

        //        //若MZ_SRANK職等為G開頭，表示警察人員，採用B_SALARY中的ORIGIN2薪級比對級距金額
        //        //其餘採用B_SALARY中的ORIGIN1俸點比對級距金額
        //        int intSALARY_int1_2 = SalaryPublic.getSalaryOriginType(strMZ_SRANK);

        //        int intS_SNID = SalaryPublic.intSALARY_ID_Data_Serach(intSALARY_NAME1, strMZ_SPT, intSALARY_int1_2);
        //        int intPay = SalaryBasic.intConcr3_PAY_Data_Serach(intS_SNID);

        //        return intPay;
        //    }

        //    return 0;
        //}

        // 依身分證字號取得退撫金費
        //public static int GetConcur3PaybyIDCard(string strIDCard)
        //{
        //    string strSQL;
        //    int intPay;
        //    DataTable dt = new DataTable();

        //    strSQL = string.Format("select CONCUR3PAY from B_BASE where IDCard = '{0}'", strIDCard);
        //    dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "B_BASECount");

        //    // B_BASE中沒有資料的話，就用計算的；否則直接取B_BASE中的資料
        //    if (dt.Rows.Count == 0 || dt.Rows[0]["CONCUR3PAY"] == null || dt.Rows[0]["CONCUR3PAY"].ToString() == "")
        //    {
        //        return CaculateSalaryPay(strIDCard);
        //    }

        //    int.TryParse(dt.Rows[0]["CONCUR3PAY"].ToString(), out intPay);

        //    return intPay;
        //}
        /* 這段已經被廢止了
        /// <summary>
        /// 傳入身分證字號，重新計算底薪、主管加給、專業加給、健保費、勞保費並存入[B_BASE]資料表
        /// </summary>
        /// <param name="strIDCard">要修改對象的身分證字號</param>
        /// <returns>是否成功</returns>
        public static bool UpdateSalaryData(string strIDCard)
        {
            string strSQL;
            DataTable dt = new DataTable();

            int intBoss = CaculateBossPay(strIDCard);
            int intProfess = CaculateProfessPay(strIDCard);
            int intSalary = CaculateSalaryPay(strIDCard);
            int intHealth = CaculateHealthPay(strIDCard, intSalary, intBoss, intProfess);

            strSQL = string.Format("select IS30YEAR from B_BASE where IDCard = '{0}'", strIDCard);
            dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "INSURANCEPAY");

            int intInsurance = CaculateInsurancePay(strIDCard, dt.Rows[0]["IS30YEAR"].ToString() == "1" ? true : false);

            strSQL = string.Format("UPDATE B_BASE SET PROFESSPAY = {1}, BOSSPAY = {2}, SALARYPAY = {3}, HEALTHPAY = {4}, INSURANCEPAY = {5} WHERE IDCARD = '{0}'", strIDCard, intProfess, intBoss, intSalary, intHealth, intInsurance);

            try
            {
                o_DBFactory.ABC_toTest.Edit_Data(strSQL);
            }
            catch
            {
                return false;
            }

            return true;
        }
        */

    }
}
