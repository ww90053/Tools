using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace TPPDDB._2_salary
{
    /// <summary>
    /// 轉帳文字檔部分(給銀行)
    /// </summary>
    public class SalaryToBank
    {
        //------------------------------------Function----------------------------------


        //------------------------------------------------------------------------------

        /// <summary>
        /// 金融轉存
        /// </summary>
        /// <param name="strPAY_AD">發薪機關</param>
        /// <param name="strType">發放類別</param>
        /// <param name="strDataDate">資料日期</param>
        /// <param name="strT_Date">入帳日期</param>
        /// <param name="strBank_id">存款銀行</param>
        /// <param name="strMemo">摘要代碼（淡水第一信用合作社使用）</param>
        /// <param name="strCaseID">案號</param>
        /// <param name="old">1未加密 2加密</param>
        /// <returns></returns>
        public static StringBuilder sb_Is_Bank(string strPAY_AD, string strType, string strDataDate, string strT_Date, string strBank_id, string strMemo, string strCaseID, string sort, string item, int old, int pwd)
        {
            StringBuilder sbData = new StringBuilder();
            string encryption = "";
            switch (strBank_id)
            {
                case "005"://土地銀行
                    if (old == 2)
                    {
                          if (pwd == 2)
                            encryption = o_DBFactory.ABC_toTest.vExecSQL("SELECT PWD FROM B_BANK_LIST WHERE BANK_ID='" + strBank_id + "'");
                    }

                    sbData = sb_LandBank_ToBank(GetTxtData(strPAY_AD, strType, strDataDate, strCaseID, strBank_id, strT_Date, sort), strT_Date, encryption);
                    break;
                case "008"://華南銀行
                    if (old == 2)
                    {
                          if (pwd == 2)
                            encryption = o_DBFactory.ABC_toTest.vExecSQL("SELECT PWD FROM B_BANK_LIST WHERE BANK_ID='" + strBank_id + "'");
                    }

                    sbData = sb_SouthBank_ToBank(GetTxtData(strPAY_AD, strType, strDataDate, strCaseID, strBank_id, strT_Date, sort), strT_Date, encryption);
                    break;
                case "004"://臺灣銀行
                    if (old == 1 && pwd!=3)
                    {
                        sbData = sb_TaiwanBank_ToBank(GetTxtData(strPAY_AD, strType, strDataDate, strCaseID, strBank_id, strT_Date, sort), strT_Date, "");
                    }
                   // else if (old == 2)
                    else
                    {

                        if (pwd == 2 )
                            encryption = o_DBFactory.ABC_toTest.vExecSQL("SELECT PWD FROM B_BANK_LIST WHERE BANK_ID='" + strBank_id + "'");
                        sbData = sb_TaiwanBank_ToBank_NEW(GetTxtData(strPAY_AD, strType, strDataDate, strCaseID, strBank_id, strT_Date, sort), strT_Date, encryption, strType, pwd, item);
                    }
                    break;
                case "700"://中華郵政
                    if (old == 2)
                    {
                          if (pwd == 2)
                            encryption = o_DBFactory.ABC_toTest.vExecSQL("SELECT PWD FROM B_BANK_LIST WHERE BANK_ID='" + strBank_id + "'");
                    }

                    sbData = sb_PostOffice_ToBank(GetTxtData(strPAY_AD, strType, strDataDate, strCaseID, strBank_id, strT_Date, sort), strT_Date, encryption);
                    


                         break;
                case "951"://農會
                         if (old == 1 && pwd != 3)
                         {
                             sbData = sb_Farmers_Cooperative_ToBank(GetTxtData(strPAY_AD, strType, strDataDate, strCaseID, strBank_id, strT_Date, sort), strT_Date, encryption);

                         }
                         else
                         {

                             if (old == 2)
                             {
                                 if (pwd == 2)
                                     encryption = o_DBFactory.ABC_toTest.vExecSQL("SELECT PWD FROM B_BANK_LIST WHERE BANK_ID='" + strBank_id + "'");
                             }

                        sbData = sb_Farmers_Cooperative_ToBank_NEW(GetTxtData(strPAY_AD, strType, strDataDate, strCaseID, strBank_id, strT_Date, sort), strT_Date, encryption, strType);
                        //暫時先不套用新版 20181129 by sky
                        //sbData = sb_Farmers_Cooperative_ToBank_NEW150(GetTxtData(strPAY_AD, strType, strDataDate, strCaseID, strBank_id, strT_Date, sort), strT_Date, encryption, strType, item);
                         }
                         break;
                case "119"://淡水一信
                         if (old == 1 && pwd != 3)
                         {
                             sbData = sb_One_ToBank(GetTxtData(strPAY_AD, strType, strDataDate, strCaseID, strBank_id, strT_Date, sort), strT_Date, strMemo, encryption);

                         }
                         else
                         {
                             if (old == 2)
                             {
                                 if (pwd == 2)
                                     encryption = o_DBFactory.ABC_toTest.vExecSQL("SELECT PWD FROM B_BANK_LIST WHERE BANK_ID='" + strBank_id + "'");

                             }
                             sbData = sb_One_ToBank_NEW(GetTxtData(strPAY_AD, strType, strDataDate, strCaseID, strBank_id, strT_Date, sort), strT_Date, strType, encryption);
                         }
                         break;
                case "013": //國泰世華
                    sbData = sb_CathayUnitedBank_ToBank(GetTxtData(strPAY_AD, strType, strDataDate, strCaseID, strBank_id, strT_Date, sort), GetFIANCENO(strPAY_AD, strBank_id), strT_Date, encryption, strType, item);
                    break;
                default:
                    return new StringBuilder("不支援此銀行格式！！");
            }
            return sbData;
        }

        /// <summary>
        /// 電子檔產生測試用
        /// 20181204 by sky
        /// </summary>
        /// <returns></returns>
        public static StringBuilder sb_Is_Bank_Test(string strPAY_AD, string strType, string strDataDate, string strT_Date, string strBank_id, string strMemo, string strCaseID, string sort, string item, int old, int pwd)
        {
            StringBuilder sbData = new StringBuilder();
            string encryption = "";

            switch (strBank_id)
            {
                case "951"://農會
                    if (old == 1 && pwd != 3)
                    {
                        sbData = sb_Farmers_Cooperative_ToBank(GetTxtData(strPAY_AD, strType, strDataDate, strCaseID, strBank_id, strT_Date, sort), strT_Date, encryption);
                    }
                    else
                    {

                        if (old == 2 && pwd == 2)
                        {
                            encryption = o_DBFactory.ABC_toTest.vExecSQL("SELECT PWD FROM B_BANK_LIST WHERE BANK_ID='" + strBank_id + "'");
                        }
                        
                        sbData = sb_Farmers_Cooperative_ToBank_NEW150(GetTxtData(strPAY_AD, strType, strDataDate, strCaseID, strBank_id, strT_Date, sort), strT_Date, encryption, strType, item);
                    }
                    break;
                case "013": //國泰世華
                    sbData = sb_CathayUnitedBank_ToBank(GetTxtData(strPAY_AD, strType, strDataDate, strCaseID, strBank_id, strT_Date, sort), GetFIANCENO(strPAY_AD, strBank_id), strT_Date, encryption, strType, item);
                    break;
                default:
                    return new StringBuilder("不支援此銀行格式！！");
            }

            return sbData;
        }





        //------------------------------------------------------



        /// <summary>
        /// 取得薪資資料
        /// </summary>
        /// <param name="strPAY_AD">發薪機關</param>
        /// <param name="strType">薪資種類</param>
        /// <param name="strT_Date">薪資日期</param>
        /// <param name="strCaseID">批號</param>
        /// <param name="strBankID">銀行代號</param>
       
        /// <param name="strT_Date">入帳日期</param>
        /// <returns></returns>

        private static DataTable GetTxtData(string strPAY_AD, string strType, string strT_Date, string strCaseID, string strBankID ,string IN_ACCOUNT_Date ,string sort)
        {
            string strSQL = string.Empty;

            switch (strType)
            {
                case "MONTH":
                    //20140513 寫入入帳日期
                    o_DBFactory.ABC_toTest.vExecSQL(string.Format("UPDATE B_MONTHPAY_MAIN SET IN_ACCOUNT_DATE='{2}' WHERE PAY_AD = '{0}' AND AMONTH = '{1}' ", strPAY_AD, FORMAT_DATE(strT_Date, 4), IN_ACCOUNT_Date));


                    strSQL = string.Format("SELECT * FROM VW_SALARYTOTXT WHERE PAY_AD = '{0}' AND AMONTH = '{1}' AND BANKID='{2}' {3} ", strPAY_AD, FORMAT_DATE(strT_Date, 4), strBankID, sort == "1" ? "ORDER BY MZ_POLNO" : "ORDER BY MM_SNID ASC");
                    break;
                case "REPAIR":
                    //20140513 寫入入帳日期
                    o_DBFactory.ABC_toTest.vExecSQL(string.Format("UPDATE B_REPAIRPAY SET IN_ACCOUNT_DATE='{3}' WHERE PAY_AD = '{0}' AND AMONTH = '{1}'  {2} ", strPAY_AD, FORMAT_DATE(strT_Date, 4), strCaseID.Length == 0 ? null : "AND BATCH_NUMBER = '" + strCaseID + "'", IN_ACCOUNT_Date));

                    strSQL = string.Format("SELECT * FROM VW_REPAIRTOTXT WHERE PAY_AD = '{0}' AND AMONTH = '{1}' AND BANKID='{2}' {3} {4}", strPAY_AD, FORMAT_DATE(strT_Date, 4), strBankID, strCaseID.Length == 0 ? null : "AND BATCH_NUMBER = '" + strCaseID + "'", sort == "1" ? "ORDER BY MZ_POLNO" : "ORDER BY R_SNID ASC");
                    break;
                case "YEAR":
                    //20140513 寫入入帳日期
                    o_DBFactory.ABC_toTest.vExecSQL(string.Format("UPDATE B_YEARPAY SET IN_ACCOUNT_DATE='{2}' WHERE PAY_AD = '{0}' AND AYEAR = '{1}' ", strPAY_AD, FORMAT_DATE(strT_Date, 5), IN_ACCOUNT_Date));

                    strSQL = string.Format("SELECT * FROM VW_YEARBONUSTOTXT WHERE PAY_AD = '{0}' AND AYEAR = '{1}' AND BANKID='{2}' {3} ", strPAY_AD, FORMAT_DATE(strT_Date, 5), strBankID, sort == "1" ? "ORDER BY MZ_POLNO" : "ORDER BY Y_SNID ASC");
                    break;
                case "EFFECT":
                    //20140513 寫入入帳日期
                    o_DBFactory.ABC_toTest.vExecSQL(string.Format("UPDATE B_EFFECT SET IN_ACCOUNT_DATE='{2}' WHERE PAY_AD = '{0}' AND AYEAR = '{1}' ", strPAY_AD, FORMAT_DATE(strT_Date, 5), IN_ACCOUNT_Date));

                    strSQL = string.Format("SELECT * FROM VW_EFFECTBONUSTOTXT WHERE PAY_AD = '{0}' AND AYEAR = '{1}' AND BANKID='{2}' {3} ", strPAY_AD, FORMAT_DATE(strT_Date, 5), strBankID, sort == "1" ? "ORDER BY MZ_POLNO" : "ORDER BY E_SNID ASC");
                    break;
                case "SOLE":
                    //20140513 寫入入帳日期
                    o_DBFactory.ABC_toTest.vExecSQL(string.Format("UPDATE B_SOLE SET IN_ACCOUNT_DATE='{3}' WHERE PAY_AD = '{0}' AND DA = '{1}'  {2} ", strPAY_AD, strT_Date, strCaseID == "" ? null : "AND CASEID = '" + strCaseID + "'", IN_ACCOUNT_Date));

                    strSQL = string.Format("SELECT * FROM VW_SOLETOTXT WHERE PAY_AD = '{0}' AND DA = '{1}'  {2} AND BANKID='{3}' {4}", strPAY_AD, strT_Date, strCaseID == "" ? null : "AND CASEID = '" + strCaseID + "'", strBankID, sort == "1" ? "ORDER BY MZ_POLNO" : "ORDER BY S_SNID ASC");
                    break;
                case "OFFER":

                    //o_DBFactory.ABC_toTest.vExecSQL(string.Format("UPDATE B_MONTHPAY_MAIN SET IN_ACCOUNT_DATE='{2}' WHERE PAY_AD = '{0}' AND AMONTH = '{1}' AND  EXTRA05 <> 0", strPAY_AD, FORMAT_DATE(strT_Date, 4), IN_ACCOUNT_Date));


                    strSQL = string.Format("SELECT * FROM VW_SAVELIST WHERE PAY_AD = '{0}' AND AMONTH = '{1}' AND BANKID='{2}' ORDER BY MZ_POLNO", strPAY_AD, FORMAT_DATE(strT_Date, 4), strBankID);
                    break;
            }

            DataTable dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "Data");
            //補發及單一方放因為匯入回饋檔時,可能同一批案號有相同身分證但不同金額的情況.所以只好拿身分證+流水號當key
            if (strType == "SOLE")
            {
                for (int i = 1; i <= dt.Rows.Count; i++)
                {

                    o_DBFactory.ABC_toTest.vExecSQL(string.Format("UPDATE B_SOLE SET TO_BANK_SN='{3}' WHERE PAY_AD = '{0}' AND DA = '{1}' AND S_SNID='{4}'  {2} "
                        , strPAY_AD
                        , strT_Date
                        , strCaseID == "" ? null : string.Format(@"AND CASEID = '{0}'", strCaseID)
                        , i
                        , dt.Rows[i - 1]["S_SNID"].ToString()));
                }
            }
            else if (strType == "REPAIR")
            {
                 for (int i = 1; i <= dt.Rows.Count; i++)
                {
                    o_DBFactory.ABC_toTest.vExecSQL(string.Format("UPDATE B_REPAIRPAY SET TO_BANK_SN='{3}' WHERE PAY_AD = '{0}' AND AMONTH = '{1}' AND R_SNID='{4}'  {2} "
                        , strPAY_AD
                        , FORMAT_DATE(strT_Date, 4)
                        , strCaseID.Length == 0 ? null : string.Format("AND BATCH_NUMBER = '{0}'", strCaseID)
                        , i
                        , dt.Rows[i - 1]["R_SNID"].ToString()));
                }
            
            }

            return dt;
        }
        /// <summary>
        /// 取得銀行委託代號
        /// </summary>
        /// <param name="strPAY_AD">分局代碼</param>
        /// <param name="strBANK_ID">銀行代碼</param>
        /// <returns></returns>
        private static string GetFIANCENO(string strPAY_AD, string strBANK_ID)
        {
            string strSQL = @"SELECT bbb.* FROM B_BRANCH_BANK bbb
                                left join B_BRANCH bb on bb.B_SNID = bbb.B_SNID
                                left join B_BANK bbk on bbk.B_SNID = bbb.BANK_ID
                                WHERE bb.ID = @strPAY_AD And bbk.ID = @strBANK_ID ";
            List<SqlParameter> paramerte = new List<SqlParameter>()
            {
                new SqlParameter("strPAY_AD", strPAY_AD),
                new SqlParameter("strBANK_ID", strBANK_ID)
            };

            DataTable dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, paramerte);
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["FIANCENO"].ToString();
            }
            else
            {
                throw new Exception("無法取得委託代號。");
            }
        }
        public static int GetTxtDataCount(string strPAY_AD, string strType, string strT_Date, string strCaseID, string strBankID, string IN_ACCOUNT_Date)
        {
            string strSQL = string.Empty;

            switch (strType)
            {
                case "MONTH":
                   
                    strSQL = string.Format("SELECT Count(*) FROM VW_SALARYTOTXT WHERE PAY_AD = '{0}' AND AMONTH = '{1}' AND BANKID='{2}'  ", strPAY_AD, FORMAT_DATE(strT_Date, 4), strBankID);
                    break;
                case "REPAIR":

                    strSQL = string.Format("SELECT Count(*) FROM VW_REPAIRTOTXT WHERE PAY_AD = '{0}' AND AMONTH = '{1}' AND BANKID='{2}' {3} ", strPAY_AD, FORMAT_DATE(strT_Date, 4), strBankID, strCaseID.Length == 0 ? null : "AND BATCH_NUMBER = '" + strCaseID + "'");
                    break;
                case "YEAR":

                    strSQL = string.Format("SELECT Count(*) FROM VW_YEARBONUSTOTXT WHERE PAY_AD = '{0}' AND AYEAR = '{1}' AND BANKID='{2}' ", strPAY_AD, FORMAT_DATE(strT_Date, 5), strBankID);
                    break;
                case "EFFECT":

                    strSQL = string.Format("SELECT Count(*) FROM VW_EFFECTBONUSTOTXT WHERE PAY_AD = '{0}' AND AYEAR = '{1}' AND BANKID='{2}'  ", strPAY_AD, FORMAT_DATE(strT_Date, 5), strBankID);
                    break;
                case "SOLE":

                    strSQL = string.Format("SELECT Count(*) FROM VW_SOLETOTXT WHERE PAY_AD = '{0}' AND DA = '{1}'  {2} AND BANKID='{3}' ", strPAY_AD, strT_Date, strCaseID == "" ? null : "AND CASEID = '" + strCaseID + "'", strBankID);
                    break;
                case "OFFER":

                    strSQL = string.Format("SELECT Count(*) FROM VW_SAVELIST WHERE PAY_AD = '{0}' AND AMONTH = '{1}' AND BANKID='{2}' ORDER BY MZ_POLNO", strPAY_AD, FORMAT_DATE(strT_Date, 4), strBankID);
                    break;
            }

            return int.Parse(o_DBFactory.ABC_toTest.vExecSQL(strSQL));

           


            
        }

        public static  string changetype(string strType)
        {
            string type = "";
            switch (strType)
            {
                case "MONTH":
                    type = "每月薪資";
                    break;
                case "REPAIR":
                    type = "補發薪資";
                    break;
                case "YEAR":
                    type = "年終獎金";
                    break;
                case "EFFECT":
                    type = "考績獎金";
                    break;
                case "SOLE":
                    type = "單一發放";
                    break;
                case "OFFER":
                    type = "優惠存款";
                    break;
            }

            return type;
        
        }

        #region 金融機構

        #region      土地銀行
        private static StringBuilder sb_LandBank_ToBank(DataTable temp, string strT_Date, string encryption_type)
        {
            if (temp.Rows.Count == 0)
                return new StringBuilder();

            string strSQL = String.Empty;
            string result = string.Empty;
            int count = 0;
            int total = 0;
            StringBuilder tobank_result = new StringBuilder();
            StringBuilder second = new StringBuilder();
            string type = "in";

            string append = "";

            //第二筆至最後第二筆
            foreach (DataRow dr in temp.Rows)
            {
                string strMZ_ID = dr["IDCARD"].ToString();

                string str001 = "2".PadRight(5, '0').PadRight(9, ' ');//錄別
                string str003 = dr["FIANCENO"].ToString().PadRight(8, ' ');//收件單位，新莊0050865                
                string str004 = type == "in" ? "20000" : "10000";//轉帳類別                
                string str005 = strT_Date.PadLeft(8, '0');//轉帳日(國曆4碼右靠左補0)ex:民國101年2月3日=01010203                
                string str006 = dr["STOCKPILE_BANKID"].ToString().PadLeft(20, '0');//帳號                
                string str007 = dr["PAY"].ToString().PadLeft(12, '0') + "00";//交易金額(12位加小數點2位)
                string str008 = "0".PadRight(8, '0');//營利事業編號，新莊00000000
                string str009 = "9999";//狀況代號                
                string str010 = "".PadLeft(90, ' ');//專用資料區                
                string str011 = "".PadRight(10, ' ');//帳戶ID                
                string str0012 = "".PadRight(2, ' ');//幣別                
                string str0013 = "".PadRight(22, ' ');//保留欄  

                //2013/01/31 金額為0的不列出
                int tmpAmount = int.Parse(dr["PAY"].ToString());

                //2013/02/04 金額為0的仍列出
                //if (tmpAmount == 0)
                //    continue;

                total += tmpAmount;//總計加總  

                str007 = str007.PadLeft(14, '0');

                //總筆數加總
                count++;
                
                second.Append(str001 + str003 + str004 + str005 + str006 + str007 + str008 + str009 + str010 + str011 + str0012 + str0013 + "\r\n");
            
            }

            //首筆(第一筆資料            
            string str01 = "1".PadRight(5, '0').PadRight(9, ' ');//錄別
            string str03 = temp.Rows[0]["FIANCENO"].ToString().PadRight(8, ' ');//收件單位，新莊0050865            
            string str04 = type == "in" ? "20000" : "10000";//轉帳類別            
            string str05 = strT_Date.PadLeft(8, '0');//轉帳日            
            string str06 = "1";//資料性質別            
            string str07 = "".PadRight(169, ' ');//保留欄
            string first = str01 + str03 + str04 + str05 + str06 + str07 + "\r\n";

            //尾錄欄位格式(最後一筆)

            string str012 = "3".PadRight(5, '0').PadRight(9, ' ');//錄別
            string str014 = temp.Rows[0]["FIANCENO"].ToString().PadRight(8, ' ');//收件單位，新莊0050865            
            string str015 = type == "in" ? "20000" : "10000";//轉帳類別            
            string str016 = strT_Date.PadLeft(8, '0');//轉帳日            
            string str017 = total.ToString().PadLeft(14, '0') + "00";//成交總金額            
            string str018 = count.ToString().PadLeft(10, '0');//成交總筆數            
            string str019 = "".PadLeft(14, '0') + "00";//未成交總金額            
            string str020 = "".PadLeft(10, '0');//未成交總筆數            
            string str021 = "".PadLeft(39, ' ');//保留欄
            string str022 = "\r\n";//尾檔記號
            string third = str012 + str014 + str015 + str016 + str017 + str018 + str019 + str020 + str021 + str022;
            append += (first + second + third);
            //tobank_result.Append(first + second + third);

            if (string.IsNullOrEmpty(encryption_type))
            {
                tobank_result.Append(append);
            }
            else
            {
                //20140417 測試加密用
                //tobank_result.Append(SalaryToBank.SalaryToBank_encryption.TaiwanBank.DESDecrypt("2F2A23G2C22&2E72", SalaryToBank.SalaryToBank_encryption.TaiwanBank.DESEncrypt("2F2A23G2C22&2E72", first + second)));
                tobank_result.Append(SalaryToBank_encryption.TaiwanBank.DESEncrypt(encryption_type, append));

            }

            return tobank_result;
        }
        #endregion      土地銀行

        #region     華南銀行
        private static StringBuilder sb_SouthBank_ToBank(DataTable temp, string strT_Date, string encryption_type)
        {
            //010308168039551017  192200384799T1223315741010000008176000

            //irk 3/26改 
            
            //20120308168039551017  0000192200384799T1223315741010000008176000
            if (temp.Rows.Count == 0)
                return new StringBuilder();

            string strSQL = String.Empty;
            string result = string.Empty;
            StringBuilder tobank_result = new StringBuilder();
            int count = 0;
            int sum = 0;
            int year = int.Parse(strT_Date.Substring(0, 3)) + 1911;

            string append = "";
            foreach (DataRow dr in temp.Rows)
            {
                string str01 = year.ToString().PadLeft(4, '0') + strT_Date.Substring(3);//6碼民國年月日(6)
                string str02 = temp.Rows[0]["FIANCENO1"].ToString();//委存帳號(12)
                string str03 = "".PadLeft(2, ' ');//空2格不知道是什麼(2)
                string str04 = dr["STOCKPILE_BANKID"].ToString().PadLeft(16, '0');//入帳帳號(12)
                string str05 = dr["IDCARD"].ToString();//身份證字號(10)
                string str06 = "101";//轉帳別(薪資轉帳)
                string str07 = dr["PAY"].ToString().PadLeft(11, '0') + "00";//入帳金額(11)+小數點(2)

                //2013/01/31 金額為0的不列出
                int tmpAmount = int.Parse(dr["PAY"].ToString());

                //2013/02/04 金額為0的仍列出
                //if (tmpAmount == 0)
                //    continue;

                sum += tmpAmount;//總計加總  
                count++;

                //20140610
                //string str08 = "".PadRight(10, ' ');
                //string str09 = "".PadRight(12, ' ');
                //string str10 = "".PadRight(1, ' ');
                //string str11 = dr["NAME"].ToString();
                //tobank_result.Append(str01 + str02 + str03 + str04 + str05 + str06 + str07 +
                //    str08 + str09 + str10+ str11 +"\r\n");
                    
                    //20140610
                //tobank_result.Append(str01 + str02 + str03 + str04 + str05 + str06 + str07 + "\r\n");
                append +=str01 + str02 + str03 + str04 + str05 + str06 + str07 + "\r\n";
            }
            //首筆
            //string str01 = strT_Date.Substring(1);//6
            //string str02 = temp.Rows[0]["BANKNO"].ToString();// "1680";//4 汐止分行
            //string str03 = temp.Rows[0]["TAXINVOICE"].ToString();//8
            //string str04 = "1";//1
            //string str05 = "01";//2
            //tobank_result.Append(string.Format("{0},{1},{2},{3},{4},{5},{6}\r\n", str01, str02, str03, str04, str05, count, sum));

            if (string.IsNullOrEmpty(encryption_type))
            {
                tobank_result.Append(append);
            }
            else
            {
                //20140417 測試加密用
                //tobank_result.Append(SalaryToBank.SalaryToBank_encryption.TaiwanBank.DESDecrypt("2F2A23G2C22&2E72", SalaryToBank.SalaryToBank_encryption.TaiwanBank.DESEncrypt("2F2A23G2C22&2E72", first + second)));
                tobank_result.Append(SalaryToBank_encryption.TaiwanBank.DESEncrypt(encryption_type, append));

            }

            return tobank_result;
        }
        #endregion     華南銀行

        #region    台灣銀行-舊
        private static StringBuilder sb_TaiwanBank_ToBank(DataTable temp, string strT_Date, string encryption)
        {
            if (temp.Rows.Count == 0)
                return new StringBuilder();

            string bankID;
            string strSQL = String.Empty;
            string result = string.Empty;
            int count = 0;
            int total = 0;
            StringBuilder tobank_result = new StringBuilder();
            StringBuilder second = new StringBuilder();
            bankID = temp.Rows[0]["BANKNO"].ToString();

            //第二筆(含)以後
            foreach (DataRow dr in temp.Rows)
            {
                //irk 3/21增加 判別 無帳號 不列出
                if (!string.IsNullOrEmpty(dr["STOCKPILE_BANKID"].ToString()))
                {
                    string strMZ_ID = dr["IDCARD"].ToString();
                    string str001 = "".PadLeft(3, ' ');
                    string str002 = dr["STOCKPILE_BANKID"].ToString().PadLeft(14, '0');
                    string str003 = dr["PAY"].ToString().PadLeft(12, '0');
                    string str004 = "".PadLeft(30, ' ');
                    string str005 = "9";

                    //2013/01/31 金額為0的不列出
                    int tmpAmount=int.Parse(str003);

                    //2013/02/04 金額為0的仍列出
                    //if (tmpAmount == 0)
                    //    continue;

                    total += tmpAmount;//總計加總  
                    count++;//總筆數加總
                    second.Append(str001 + str002 + str003 + str004 + str005 + "\r\n");
                }
            }
            //首筆
            string str01 = string.Format("{0}{1}{2}", (int.Parse(DateTime.Now.Year.ToString()) - 1911).ToString().PadLeft(3, '0').Substring(0), DateTime.Now.Month.ToString().PadLeft(2, '0'), DateTime.Now.Day.ToString().PadLeft(2, '0'));
            string str02 = strT_Date.Substring(1);
            string str03 = bankID.PadLeft(3, ' ');// 分行代號(B_BANK.BANKNO)
            string str04 = temp.Rows[0]["TAXINVOICE"].ToString().PadLeft(8, ' ');//營利事業統一編號(8碼)
            string str05 = temp.Rows[0]["FIANCENO"].ToString().PadLeft(3, '0');//客戶代號(3碼)
            string str06 = "00000000000000";//14碼0
            string str07 = total.ToString().PadLeft(12, '0');//總金額(12碼不含角分)
            string str08 = count.ToString().PadLeft(4, '0');//總筆數(4碼)
            string str09 = "".PadLeft(3, ' ');//3碼英文空白
            string str10 = "9";//結束記號9(1碼)
            string first = str01 + str02 + str03 + str04 + str05 + str06 + str07 + str08 + str09 + str10 + "\r\n";

            if (string.IsNullOrEmpty(encryption))
            {
                tobank_result.Append(first + second);
            }
            else 
            {    
                //20140417 測試加密用
                //tobank_result.Append(SalaryToBank.SalaryToBank_encryption.TaiwanBank.DESDecrypt("2F2A23G2C22&2E72", SalaryToBank.SalaryToBank_encryption.TaiwanBank.DESEncrypt("2F2A23G2C22&2E72", first + second)));
                tobank_result.Append(SalaryToBank_encryption.TaiwanBank.DESEncrypt(encryption, first + second));
           
            }
            return tobank_result;
        }
        #endregion    台灣銀行-舊

        #region    台灣銀行-新
        private static StringBuilder sb_TaiwanBank_ToBank_NEW(DataTable temp, string strT_Date, string encryption_type, string strType, int IsPwd, string item)
        {
            if (temp.Rows.Count == 0)
                return new StringBuilder();

          
            string strSQL = String.Empty;
            string result = string.Empty;
           
            StringBuilder tobank_result = new StringBuilder();
         
            string append ="";
            
            int SN = 1;
                   int total = 0;    
            foreach (DataRow dr in temp.Rows)
            {
                
                //irk 3/21增加 判別 無帳號 不列出
                if (!string.IsNullOrEmpty(dr["STOCKPILE_BANKID"].ToString()))
                {

                  
                    //交易序號	起始位置1	總長度16
                    string str001 = SN.ToString().PadLeft(16, '0');

                    //付款日期	起始位置17	總長度8
                    string str002 = strT_Date.PadRight(8, ' ');

                    //應付薪資金額	起始位置25	總長度12
                    string str003 = dr["PAY"].ToString().PadRight(12, ' ');
                    total +=  int.Parse(dr["PAY"].ToString());

                    //實付薪資金額	37	總長度12
                    string str004 = dr["PAY"].ToString().PadRight(12, ' ');

                    //手續費試算	起始位置49	總長度5
                    string str005 = "".PadRight(5, ' '); 

                    int NAME_LENGTH = Encoding.Default.GetByteCount(dr["NAME"].ToString());

                    //員工戶名	起始位置54	總長度80
                    string str006 = dr["NAME"].ToString() + "".PadRight(80 - NAME_LENGTH, ' ');
                    
                    //銀行代碼	起始位置134	總長度7
                    string str007 = dr["FIANCENO"].ToString().PadRight(7, ' ');

                    //員工帳號	起始位置141	總長度16
                    string str008 = dr["STOCKPILE_BANKID"].ToString().PadRight(16, ' ');

                    //手續費分攤方式	起始位置157	總長度1
                    string str009 = "".PadRight(1, ' ');                                    
                    
                    string type=changetype(strType); 
                   

                    //備註	起始位置158	總長度80

                    int MEMO_LENGTH = 0; 
                    string str010 = "";
                    if (strType == "SOLE")
                    {
                        MEMO_LENGTH = Encoding.Default.GetByteCount(dr["ITEM"].ToString());

                        str010 = dr["ITEM"].ToString() + "".PadRight(80 - MEMO_LENGTH, ' ');
                    }
                    else
                    {
                        MEMO_LENGTH = Encoding.Default.GetByteCount(item);
                        str010 = item + "".PadRight(80 - MEMO_LENGTH, ' ');
                    }
                    //員工身分證號	起始位置238	總長度10
                    string str011 = dr["IDCARD"].ToString().PadRight(10, ' ');

                    //員工e-mail	起始位置248	總長度50
                    string str012 = "".PadRight(50, ' ');

                    //空白	起始位置298	總長度32            
                    string str013 = "".PadRight(32, ' ');                                  

                    append += str001 + str002 + str003 + str004 + str005 +
                        str006 + str007 + str008 + str009 + str010 +
                        str011 + str012 + str013 + "\r\n";
                   
                    


                    SN++;
                }
            }

            if (IsPwd == 1)
            {
                append += " 共 "+ temp.Rows.Count+ "筆 總金額為:" + total.ToString() + "\r\n";
            
            }

            ////首筆
            
            //string str03 = bankID.PadLeft(3, ' ');// 分行代號(B_BANK.BANKNO)
            //string str04 = temp.Rows[0]["TAXINVOICE"].ToString().PadLeft(8, ' ');//營利事業統一編號(8碼)
            //string str05 = temp.Rows[0]["FIANCENO"].ToString().PadLeft(3, '0');//客戶代號(3碼)

            if (string.IsNullOrEmpty(encryption_type))
            {
                tobank_result.Append(append);
            }
            else 
            {
                //20140417 測試加密用
                //tobank_result.Append(SalaryToBank.SalaryToBank_encryption.TaiwanBank.DESDecrypt("2F2A23G2C22&2E72", SalaryToBank.SalaryToBank_encryption.TaiwanBank.DESEncrypt("2F2A23G2C22&2E72", first + second)));
                tobank_result.Append(SalaryToBank_encryption.TaiwanBank.DESEncrypt(encryption_type, append));

            }
            return tobank_result;
        }
        #endregion    台灣銀行

        #region   中華郵政
        private static StringBuilder sb_PostOffice_ToBank(DataTable temp, string strT_Date, string encryption_type)
        {
            string strSQL = String.Empty;
            string result = string.Empty;
            StringBuilder tobank_result = new StringBuilder();
            string append = "";
            foreach (DataRow dr in temp.Rows)
            {
                string str001 = "033";//卡別(3)
                string str002 = dr["FIANCENO"].ToString().PadLeft(7, ' ');//經辦局(7)總局0311083
                string str003 = "".PadLeft(6, ' ');//空白(6)
                string str004 = dr["FIANCENO1"].ToString().PadLeft(8, ' ');//委存帳號(8)總局14153018
                string str005 = FORMAT_DATE(strT_Date, 3);//轉存日期(6)
                string str007;
                //若在人事找不到，則去其他基本資料找??
                str007 = dr["STOCKPILE_BANKID"].ToString().PadLeft(7, ' ');//立帳帳號(7) //2013/06/10 (14碼)
                string str008 = dr["IDCARD"].ToString();//身分證字號(10) 
                //string str009 = TRANSFER_PAY(mz_id, strPAY_AD, strT_Date).ToString().PadRight(8, '0');
                //存款金額-含角分(8+2)
                string str009 = dr["PAY"].ToString().PadLeft(8, '0') + "00";// 存款金額
                string str010 = "".PadLeft(15, ' ');//保留欄(15)
                append += str001 + str002 + str003 + str004 + str005 + str007 + str008 + str009 + str010 + "\r\n";
                //tobank_result.Append(str001 + str002 + str003 + str004 + str005 + str007 + str008 + str009 + str010 + "\r\n");
            }

            if (string.IsNullOrEmpty(encryption_type))
            {
                tobank_result.Append(append);
            }
            else
            {
                //20140417 測試加密用
                //tobank_result.Append(SalaryToBank.SalaryToBank_encryption.TaiwanBank.DESDecrypt("2F2A23G2C22&2E72", SalaryToBank.SalaryToBank_encryption.TaiwanBank.DESEncrypt("2F2A23G2C22&2E72", first + second)));
                tobank_result.Append(SalaryToBank_encryption.TaiwanBank.DESEncrypt(encryption_type, append));

            }


            return tobank_result;
        }
        #endregion   中華郵政

        #region  農會 - 舊

        private static StringBuilder sb_Farmers_Cooperative_ToBank(DataTable temp, string strT_Date, string encryption_type)
        {
            // 格式： " " + 05121727467000 + 114000 + 00001000 + 00051 + 20110310 + 20110311 + "           " + 00000000
            //               個人薪資帳號               金額             轉帳日期   入帳日期
            string strSQL = String.Empty;
            string result = string.Empty;
            StringBuilder tobank_result = new StringBuilder();
            string append = "";

            foreach (DataRow dr in temp.Rows)
            {
                #region 舊的

                //string strMZ_ID = dr["IDCARD"].ToString();

                //string str001 = dr["BANKID"].ToString();//個人薪資帳號(14)
                //string str002 = "1";//1
                //string str003 = "114";//3
                //string str004 = dr["PAY"].ToString();
                //string str005 = dr["BANKID"].ToString().PadLeft(4, ' ').Substring(0, 4);//4
                //string str006 = FORMAT_DATE(DateTime.Now.ToString(), 2);//8
                //string str007 = FORMAT_DATE(strT_Date, 2);//8
                //string str008 = "".PadLeft(2, ' ');//2
                //string str009 = "1";//1
                //string str010 = "".PadLeft(10, ' ');//10
                //string str011 = "000000";//6
                //string str012 = "0";//0

                //tobank_result.Append(str001 + str002 + str003 + str004 + str005 + str006 + str007 + str008 + str009 + str010 + str011 + str012 + "\r\n");

                #endregion

                string strMZ_ID = dr["IDCARD"].ToString();

                string str000 = " " + dr["FIANCENO"].ToString();//委託機關代碼
                string str001 = dr["STOCKPILE_BANKID"].ToString().PadRight(12, '0').Substring(0, 12);//個人薪資帳號(12)
                string str002 = "114000";// 未知(6)
                string str003 = dr["PAY"].ToString().PadLeft(8, '0');// 金額(8)
                string str004 = "00051";// 未知(5)
                string str005 = SalaryPublic.strADDate();// 轉帳日期(8)
                string str006 = SalaryPublic.ToADDate(strT_Date);// 入帳日期(8)
                string str007 = "".PadLeft(11, ' ');// 未知(11)
                string str008 = "00000000";// 未知(8)

                //tobank_result.Append(str000 + str001 + str002 + str003 + str004 + str005 + str006 + str007 + str008 + "\r\n");
                append += str000 + str001 + str002 + str003 + str004 + str005 + str006 + str007 + str008 + "\r\n";
            }

            if (string.IsNullOrEmpty(encryption_type))
            {
                tobank_result.Append(append);
            }
            else
            {
                //20140417 測試加密用
                //tobank_result.Append(SalaryToBank.SalaryToBank_encryption.TaiwanBank.DESDecrypt("2F2A23G2C22&2E72", SalaryToBank.SalaryToBank_encryption.TaiwanBank.DESEncrypt("2F2A23G2C22&2E72", first + second)));
                tobank_result.Append(SalaryToBank_encryption.TaiwanBank.DESEncrypt(encryption_type, append));

            }

            return tobank_result;
        }
        #endregion  農會 - 舊

        #region  農會 - 新
        
        private static StringBuilder sb_Farmers_Cooperative_ToBank_NEW(DataTable temp, string strT_Date, string encryption_type , string strType)
        {
            // 格式： " " + 05121727467000 + 114000 + 00001000 + 00051 + 20110310 + 20110311 + "           " + 00000000
            //               個人薪資帳號               金額             轉帳日期   入帳日期
            string strSQL = String.Empty;
            string result = string.Empty;
            StringBuilder tobank_result = new StringBuilder();
            string append = "";

            foreach (DataRow dr in temp.Rows)
            {
                //string strMZ_ID = dr["IDCARD"].ToString();

                //string str000 = " " + dr["FIANCENO"].ToString();//委託機關代碼
                //string str001 = dr["STOCKPILE_BANKID"].ToString().PadRight(12, '0').Substring(0, 12);//個人薪資帳號(12)
                //string str002 = "114000";// 未知(6)
                //string str003 = dr["PAY"].ToString().PadLeft(8, '0');// 金額(8)
                //string str004 = "00051";// 未知(5)
                //string str005 = SalaryPublic.strADDate();// 轉帳日期(8)
                //string str006 = SalaryPublic.ToADDate(strT_Date);// 入帳日期(8)
                //string str007 = "".PadLeft(11, ' ');// 未知(11)
                //string str008 = "00000000";// 未知(8)
                
                string strMZ_ID = dr["IDCARD"].ToString();

                //string str000 = dr["FIANCENO"].ToString() +  dr["STOCKPILE_BANKID"].ToString().PadRight(12, '0').Substring(0, 12);//委託機關代碼(02)+//個人薪資帳號(12)=存摺帳號(14)
                string str000 = dr["FIANCENO"].ToString() +"0"+ dr["STOCKPILE_BANKID"].ToString().PadRight(11, '0').Substring(0, 11);//委託機關代碼(02)+//個人薪資帳號(12)=存摺帳號(14)
                
                string str001 = "1";//存提區分(1)
                string str002 = "014";// 摘要(3) 沿用舊的有錯更正  014薪資 015獎金 
                
                switch(strType)
                {                                         
                    case "EFFECT":                       
                    case "YEAR":                       
                    case "SOLE":
                        str002 = "015";
                        break;
                }
                string str003 = dr["PAY"].ToString().PadLeft(11, '0')+"00";// 金額(13)
                string str004 = str000.Substring(0, 4);  //"0512";// 農會代號(2)+分會代號(2)
                string str005 = SalaryPublic.strADDate();// 轉帳日期(登入日)(8)
                string str006 = SalaryPublic.ToADDate(strT_Date);// 入帳日期(決算日)(8)

                //13個空白
                string str007 = "  ";//空白(2)
                string str008 = " ";// 未知(1)
                string str009 = "".PadLeft(10,' ');// 空白(10)

                //7個零
                string str010 = "00";// 補零(2)
                string str011 = "000";// 企業別(3) 若無企業別代號時可為空白或補零
                string str012 = "0";// 補零(1)
                string str013 = "0";// 備註(1)


                //tobank_result.Append(str000 + str001 + str002 + str003 + str004 + str005 + str006 + str007 + str008 + "\r\n");
                append += str000 + str001 + str002 + str003 + str004 + str005 + str006 + str007 + str008 +
                   str009 + str010 + str011 + str012 + str013 + "\r\n";
            }

            if (string.IsNullOrEmpty(encryption_type))
            {
                tobank_result.Append(append);
            }
            else
            {
                //20140417 測試加密用
                //tobank_result.Append(SalaryToBank.SalaryToBank_encryption.TaiwanBank.DESDecrypt("2F2A23G2C22&2E72", SalaryToBank.SalaryToBank_encryption.TaiwanBank.DESEncrypt("2F2A23G2C22&2E72", first + second)));
                tobank_result.Append(SalaryToBank_encryption.TaiwanBank.DESEncrypt(encryption_type, append));

            }

            return tobank_result;
        }
        #endregion  農會 - 新

        #region 農會 - 新150byte

        /// <summary>
        /// 產生電子入扣帳文字
        /// </summary>
        /// <param name="temp">明細資料物件</param>
        /// <param name="strT_Date">入帳日期</param>
        /// <param name="encryption_type">加密密碼</param>
        /// <param name="strType">發放類別</param>
        /// <param name="item">入帳項目</param>
        /// <returns></returns>
        private static StringBuilder sb_Farmers_Cooperative_ToBank_NEW150(DataTable temp, string strT_Date, string encryption_type, string strType, string item)
        {
            //新格式分成三段:首筆*1、明細*N、尾筆*1
            //首筆： 1   +  781XX001  +  "600     "  +   78101       +   1071109     +   1     +  00    +  " "(68)       +   " "(50)
            //     固定1    發件單位  +   收件單位    +   本分會代號   +  實際入扣帳日  + 性質別  +  固定  +  空白欄(68碼)   +  左靠右補空白
            //明細： 2   +  同首筆    +   同首筆      +   同首筆      +   1071109       +     2     +   253       +   78101111022103   +  00000000130000              +   000000000000  +    99
            //     固定2 +  發件單位  +   收件單位    +   本分會代號   +  實際入扣帳日   +  存提區分  +  摘要代號    +      銀行帳號       +  交易金額(右靠左補0含2小數)    +   證券扣款不足   +  固定99
            //明細接續前：   ""(14)          +   ""(10)     +      ""(1)       +        ""(10)              +     00001(12)            +     " "(28)
            //         交易註記1(入帳項目)   +  交易註記2   +  身份證檢核記號   +  身份證字號(前項非空才填)    +  專用資料區(明細項目數)   +     保留欄(填空白)
            //尾筆： 3    +  同首筆    +   同首筆     +   同首筆      +   1071109     +  0000000022         +   0000000000000000            +    0000000008696800           +  0000000000   +  0000000000000000  +  0000000000000000  +  ""(37)
            //     固定3  +  發件單位  +   收件單位   +  本分會代號   +  實際入扣帳日   +  總筆數(靠右左補0)   +  借方總金額(右靠左補0含2小數)   +  貸方總金額(右靠左補0含2小數)   +  未成交總筆數  +  未成交借方總金額    +  未成交貸方總金額    +  待確認交易總筆數(5)、待確認交易總金額(16)、空白欄(16)

            StringBuilder tobank_result = new StringBuilder();
            StringBuilder topResult = new StringBuilder();
            topResult.Append("1");//區別碼(1)
            topResult.Append("781XX001");//發件單位(8)
            topResult.Append("600".PadRight(8, ' '));//收件單位(8)
            topResult.Append("78101");//本分會代號(5)
            topResult.Append(strT_Date);//實際入扣帳日(7)
            topResult.Append("1");//性質別(1)
            topResult.Append("00");//證券扣款區分(2)
            topResult.Append("".PadLeft(68, ' '));//空白欄(68)
            topResult.Append("".PadLeft(50, ' '));//放置71轉150時的檔案名(50)

            StringBuilder detail = new StringBuilder();
            int total = 0;
            int sumPay = 0;
            foreach (DataRow dr in temp.Rows)
            {
                string data = "";
                data += "2";//區別碼(1)
                data += "781XX001";//發件單位(8)
                data += "600".PadRight(8, ' ');//收件單位(8)
                data += "78101";//本分會代號(5)
                data += strT_Date;//實際入扣帳日(7)
                data += "2";//存提區分(1)
                data += Get_Farmers_Cooperative_Detail7(item).PadLeft(3, '0');//摘要代號(3)
                data += dr["FIANCENO"].ToString() + "0" + dr["STOCKPILE_BANKID"].ToString().PadRight(11, '0').Substring(0, 11);//委託機關代碼(02)+個人薪資帳號(12)=銀行帳號(14)
                data += dr["PAY"].ToString().PadLeft(12, '0') + "00";//交易金額(14) 整數12位 小數2位0
                data += "".PadLeft(12, '0');//證券扣款不足(12)
                data += "99";//狀況代號(2)  送來的檔案一律放99
                //處理入帳項目文字
                if (SalaryPublic.IsBig5Code(item))
                {
                    item = item.PadRight(6).Substring(0, 6);
                    int bc = Encoding.Default.GetByteCount(item);
                    item = item + "".PadRight(14 - bc, ' ');
                }
                else
                {
                    int bc = Encoding.Default.GetByteCount(item);
                    item = (item + "".PadRight(14 - bc, ' ')).Substring(0, 14);
                }
                data += item;//交易註記1(14)
                data += "".PadLeft(10, ' ');//交易註記2(10)
                data += " ";//身份證檢核記號(1)
                data += "".PadLeft(10, ' ');//身份證字號(1) 前項非空才填
                data += "".PadLeft(12, ' ');//專用資料區(12) 目前無用填空白
                data += "".PadLeft(28, ' ');//保留欄(28)

                total++;
                int.TryParse(dr["PAY"].ToString(), out int pay);
                sumPay += pay;

                detail.Append(data);
                if (temp.Rows.Count != total)
                {
                    detail.AppendLine();
                }
            }
            

            StringBuilder bottomResult = new StringBuilder();
            bottomResult.Append("3");//區別碼(1)
            bottomResult.Append("781XX001");//發件單位(8)
            bottomResult.Append("600".PadRight(8, ' '));//收件單位(8)
            bottomResult.Append("78101");//本分會代號(5)
            bottomResult.Append(strT_Date);//實際入扣帳日(7)
            bottomResult.Append(total.ToString().PadLeft(10, '0'));//總筆數(10)
            bottomResult.Append("".PadLeft(16, '0'));//借方總金額(16)
            bottomResult.Append(sumPay.ToString().PadLeft(14, '0') + "00");//貸方總金額(16)
            bottomResult.Append("".PadLeft(10, '0'));//未成交總筆數(10)
            bottomResult.Append("".PadLeft(16, '0'));//未成交借方總金額(16)
            bottomResult.Append("".PadLeft(16, '0'));//未成交貸方總金額(16)
            bottomResult.Append("".PadLeft(5, ' '));//待確認交易總筆數(5)
            bottomResult.Append("".PadLeft(16, ' '));//待確認交易總金額(16)
            bottomResult.Append("".PadLeft(16, ' '));//空白欄(16)


            tobank_result.AppendLine(topResult.ToString());
            tobank_result.AppendLine(detail.ToString());
            tobank_result.AppendLine(bottomResult.ToString());
            if (!string.IsNullOrEmpty(encryption_type))
            {
                string desStr = SalaryToBank_encryption.TaiwanBank.DESEncrypt(encryption_type, tobank_result.ToString());
                tobank_result = new StringBuilder();
                tobank_result.Append(desStr);
            }

            return tobank_result;
        }
        /// <summary>
        /// 取得農會明細「摘要代號」欄位
        /// </summary>
        /// <param name="item">入賬項目</param>
        /// <returns>3碼代碼</returns>
        private static string Get_Farmers_Cooperative_Detail7(string item)
        {
            string summaryCode = "000";

            switch (item.Trim())
            {
                case "每月薪資":
                    summaryCode = "051";
                    break;
                case "超勤加班費":
                    summaryCode = "251";
                    break;
                case "值日費":
                    summaryCode = "382";
                    break;
                case "不休假獎金":
                    summaryCode = "268";
                    break;
                case "年終獎金":
                    summaryCode = "380";
                    break;
                case "考績獎金":
                    summaryCode = "379";
                    break;
                case "獎金":
                    summaryCode = "261";
                    break;
                case "慰問金":
                    summaryCode = "322";
                    break;
                case "誤餐費":
                    summaryCode = "252";
                    break;
                case "鐘點費":
                    summaryCode = "253";
                    break;
                case "差旅費":
                    summaryCode = "254";
                    break;
                case "結婚補助":
                    summaryCode = "255";
                    break;
                case "喪葬補助":
                    summaryCode = "256";
                    break;
                case "兼職費":
                    summaryCode = "256";
                    break;
                case "教育補助":
                    summaryCode = "264";
                    break;
                case "旅遊補助":
                    summaryCode = "265";
                    break;
                case "出席費":
                    summaryCode = "258";
                    break;
                case "退休金":
                    summaryCode = "262";
                    break;
                case "酬勞金":
                    summaryCode = "271";
                    break;
                case "獎學金":
                    summaryCode = "272";
                    break;
                case "退保費":
                    summaryCode = "309";
                    break;
            }

            return summaryCode;
        }
        #endregion

        #region  淡水一信 - 舊

        private static StringBuilder sb_One_ToBank(DataTable temp, string strT_Date, string strMemo, string encryption_type)
        {
            string strSQL = String.Empty;
            //string str008;
            string result = string.Empty;
            StringBuilder tobank_result = new StringBuilder();
            string append = "";
            foreach (DataRow dr in temp.Rows)
            {
                string strMZ_ID = dr["IDCARD"].ToString();

                string str001 = dr["BANKNO"].ToString().PadLeft(7, ' ');//受理單位代號(7)
                string str002 = strT_Date.Substring(1);//薪資入帳日期(6)
                string str003 = "01010";//企業代號(5) 淡水分局



                string str004 = strMemo; //摘要(3)150=薪水   
                string str005 = dr["STOCKPILE_BANKID"].ToString().Length == 16 ? dr["STOCKPILE_BANKID"].ToString() : dr["BANKID"].ToString() + dr["STOCKPILE_BANKID"].ToString();// 受款人帳號
                string str006 = dr["PAY"].ToString().PadLeft(11, '0') + "00";
                string str007 = "1";//存提區分 1表示存款
               

                append += str001 + str002 + str003 + str004 + str005 + str006 + str007 + "\r\n";

                
            }
            if (string.IsNullOrEmpty(encryption_type))
            {
                tobank_result.Append(append);
            }
            else
            {
                //20140417 測試加密用
                //tobank_result.Append(SalaryToBank.SalaryToBank_encryption.TaiwanBank.DESDecrypt("2F2A23G2C22&2E72", SalaryToBank.SalaryToBank_encryption.TaiwanBank.DESEncrypt("2F2A23G2C22&2E72", first + second)));
                tobank_result.Append(SalaryToBank_encryption.TaiwanBank.DESEncrypt(encryption_type, append));

            }

            return tobank_result;


        }
        #endregion  淡水一信 - 舊

        #region  淡水一信 - 新
        private static StringBuilder sb_One_ToBank_NEW(DataTable temp, string strT_Date, string strType, string encryption_type)
        {
            string strSQL = String.Empty;
            //string str008;
            string result = string.Empty;
            StringBuilder tobank_result = new StringBuilder();
            string append = "";
            foreach (DataRow dr in temp.Rows)
            {
                string strMZ_ID = dr["IDCARD"].ToString();

                string str001 = dr["BANKNO"].ToString().PadLeft(7, ' ');//受理單位代號(7)
                string str002 = strT_Date.Substring(1);//薪資入帳日期(6)
                string str003 = "01010";//企業代號(5) 淡水分局

                string str004 = "";//摘要

                switch (strType)
                {
                    case "MONTH":
                    case "REPAIR":
                    case "OFFER":
                        str004 = "071";
                        break;
                    case "EFFECT":
                        str004 = "150";
                        break;
                    case "YEAR":
                        str004 = "174";
                        break;
                    case "SOLE":
                        str004 = dr["NOTE"].ToString().Trim().Substring(0,3);
                        break;


                }
               
                //string str004 = strMemo; //摘要(3)150=薪水   
                string str005 = dr["STOCKPILE_BANKID"].ToString().Length == 16 ? dr["STOCKPILE_BANKID"].ToString() : dr["BANKID"].ToString() + dr["STOCKPILE_BANKID"].ToString();// 受款人帳號
                string str006 = dr["PAY"].ToString().PadLeft(11, '0') + "00";
                string str007 = "1";//存提區分 1表示存款
               

                append += str001 + str002 + str003 + str004 + str005 + str006 + str007 + "\r\n";

            }

            if (string.IsNullOrEmpty(encryption_type))
            {
                tobank_result.Append(append);
            }
            else
            {
                //20140417 測試加密用
                //tobank_result.Append(SalaryToBank.SalaryToBank_encryption.TaiwanBank.DESDecrypt("2F2A23G2C22&2E72", SalaryToBank.SalaryToBank_encryption.TaiwanBank.DESEncrypt("2F2A23G2C22&2E72", first + second)));
                tobank_result.Append(SalaryToBank_encryption.TaiwanBank.DESEncrypt(encryption_type, append));

            }

            return tobank_result;
        }
        #endregion  淡水一信 - 新

        #region 國泰世華
        /// <summary>
        /// 國泰世華 產生電子入扣帳文字
        /// 20181204 by sky
        /// </summary>
        /// <param name="temp"></param>
        /// <param name="strFIANCENO">銀行委託代碼</param>
        /// <param name="strT_Date"></param>
        /// <param name="encryption_type"></param>
        /// <param name="strType"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        private static StringBuilder sb_CathayUnitedBank_ToBank(DataTable temp, string strFIANCENO, string strT_Date, string encryption_type, string strType, string item)
        {
            //格式分成三段:首筆*1、明細*N、尾筆*1
            //首筆： 1   +  00018529  +  01300000  +  20181109   +   "900  "    +    1     +   99   +   " "(167)
            //     固定  +  發件單位  +   收件單位  + 指定入扣帳日 + 媒體轉帳種型  +  性質別  +  固定  +   固定空白
            //明細： 2   +  00018529  +  01300000  +  20181109   +   "9002 "    +     0990000001       +      0000003456700        +       " "(8)       +  0130000    +      0000000123456789
            //     固定  +  發件單位  +   收件單位  + 指定入扣帳日 + 媒體轉帳種型  + 交易序號(099+流水號)  + 交易金額(右靠左補0含2小數) +  委託單位(固定空白) +  轉帳行代號  +  轉帳帳號(0000+帳號(12)右靠左補零)
            //明細接續前： A123456789  +      ""(12)" "(4)       +     9999          +  " "(12)帳號檢碼(2)金額檢碼(2)" "(14)  +   " "(56)
            //            轉帳帳號ID   +  客戶自訂備註(入帳項目)  +  轉帳後之回應代碼  +            銀行專用區                  +  客戶自定區
            //尾筆： 3   +  00018529  +  01300000  +  20181109   +   "900  "    +   "+"  +    000000003456700     +   0000012345  +  "+"   +  000000000000000  +  0000000000  + "+"  +  000000000000000   +  0000000000  + " "(92)
            //     固定  +  發件單位  +   收件單位  + 指定入扣帳日 + 媒體轉帳種型  +  固定  +  交易總金額(末兩位小數) +    交易總筆數  +  固定  +        固定        +    固定      + 固定 +        固定         +     固定     + 保留欄位

            StringBuilder tobank_result = new StringBuilder();
            StringBuilder topResult = new StringBuilder();
            topResult.Append("1");//區別碼(1)
            topResult.Append(strFIANCENO);//發件單位(3)(5)
            topResult.Append("01300000");//收件單位(3)(5)
            topResult.Append(o_str.RCDateConvert(strT_Date, "yyyyMMdd", false));//指定入/扣帳日期(8)YYYYMMDD
            topResult.Append("900".PadRight(5, ' '));//媒體轉帳種型(3)(1)(1)
            topResult.Append("1");//性質別(1)
            topResult.Append("99");//批號(2)
            topResult.Append("".PadLeft(167, ' '));//保留欄(167)

            StringBuilder detail = new StringBuilder();
            int total = 0; //統計筆數
            int sumPay = 0; //統計總金額
            foreach (DataRow dr in temp.Rows)
            {
                string strData = "";
                strData += "2";//區別碼(1)
                strData += strFIANCENO;//發件單位(3)(5)
                strData += "01300000";//收件單位(3)(5)
                strData += o_str.RCDateConvert(strT_Date, "yyyyMMdd", false);//指定入/扣帳日期(8)YYYYMMDD
                strData += "9002".PadRight(5, ' ');//媒體轉帳種型(3)(1)(1)
                strData += "099" + (total + 1).ToString().PadLeft(7, '0');//交易序號(1)(2)(7)

                //金額
                string am = dr["PAY"].ToString().PadLeft(11, '0') + "00";
                strData += am;//交易金額(11)(V99)

                strData += "".PadLeft(8, ' ');//委託單位(8)
                strData += "0130000";//轉帳行代號(3)(4)

                //帳號
                string ac = dr["STOCKPILE_BANKID"].ToString().PadLeft(12, '0');
                strData += "0000" + ac;//轉帳帳號(4)(12)

                strData += dr["IDCARD"].ToString().PadRight(10, ' ');//轉帳帳號ID(10)
                ////處理入帳項目文字
                if (SalaryPublic.IsBig5Code(item))
                {
                    item = item.PadRight(6).Substring(0, 6);
                    int bc = Encoding.Default.GetByteCount(item);
                    item = item + "".PadRight(12 - bc, ' ');
                }
                else
                {
                    int bc = Encoding.Default.GetByteCount(item);
                    item = (item + "".PadRight(12 - bc, ' ')).Substring(0, 12);
                }
                strData += item + "".PadRight(4, ' ');//客戶自訂備註(12)(4)
                strData += "9999";//轉帳後之回應代碼(4)
                strData += "".PadLeft(12, ' ') + Get_CheckAccount(ac) + Get_CheckAmount(am) + "".PadLeft(14, ' ');//銀行專用區(12)(2)(2)(14)
                strData += "".PadLeft(56, ' ');//客戶自定區(56)

                total++;
                int.TryParse(dr["PAY"].ToString(), out int pay);
                sumPay += pay;

                detail.Append(strData);
                if (temp.Rows.Count != total)
                {
                    detail.AppendLine();
                }
            }

            StringBuilder bottomResult = new StringBuilder();
            bottomResult.Append("3");//區別碼(3)
            bottomResult.Append(strFIANCENO);//發件單位(3)(5)
            bottomResult.Append("01300000");//收件單位(3)(5)
            bottomResult.Append(o_str.RCDateConvert(strT_Date, "yyyyMMdd", false));//指定入/扣帳日期(8)YYYYMMDD
            bottomResult.Append("900".PadRight(5, ' '));//媒體轉帳種型(3)(1)(1)
            bottomResult.Append("+");//檢核欄位(1)
            bottomResult.Append(sumPay.ToString().PadLeft(13,'0') + "00");//交易總金額(13)(V99)
            bottomResult.Append(total.ToString().PadLeft(10, '0'));//交易總筆數(10)
            bottomResult.Append("+");//檢核欄位(1)
            bottomResult.Append("000000000000000");//成功總金額(13)(V99)
            bottomResult.Append("0000000000");//成功總筆數(10)
            bottomResult.Append("+");//檢核欄位(1)
            bottomResult.Append("000000000000000");//不成功總金額(13)(V99)
            bottomResult.Append("0000000000");//不成功總筆數(10)
            bottomResult.Append("".PadLeft(92, ' '));//保留欄位(92)


            tobank_result.AppendLine(topResult.ToString());
            tobank_result.AppendLine(detail.ToString());
            tobank_result.AppendLine(bottomResult.ToString());

            return tobank_result;
        }
        /// <summary>
        /// 取得帳號檢碼
        /// </summary>
        /// <param name="account">12位帳號碼</param>
        /// <returns></returns>
        private static string Get_CheckAccount(string account)
        {
            string ca = string.Empty;

            int A1 = Convert.ToInt32(account.Substring(1, 1)) * 9;
            int A2 = Convert.ToInt32(account.Substring(2, 1)) * 2;
            int A3 = Convert.ToInt32(account.Substring(5, 1)) * 5;
            int A4 = Convert.ToInt32(account.Substring(6, 1)) * 4;
            int A5 = Convert.ToInt32(account.Substring(9, 1)) * 3;
            int A6 = Convert.ToInt32(account.Substring(10, 1)) * 6;
            int x = A1 + A2 + A3 + A4 + A5 + A6;
            int S1 = 11 - (x % 11);

            A1 = Convert.ToInt32(account.Substring(0, 1)) * 8;
            A2 = Convert.ToInt32(account.Substring(3, 1)) * 2;
            A3 = Convert.ToInt32(account.Substring(4, 1)) * 7;
            A4 = Convert.ToInt32(account.Substring(7, 1)) * 5;
            A5 = Convert.ToInt32(account.Substring(8, 1)) * 6;
            A6 = Convert.ToInt32(account.Substring(11, 1)) * 9;
            x = A1 + A2 + A3 + A4 + A5 + A6;
            int S2 = 11 - (x % 11);

            string y = (S1 - S1 / 10 * 10).ToString() + (S2 - S2 / 10 * 10).ToString();
            int r = Convert.ToInt32(y) + 88;
            ca = r.ToString().Substring(r.ToString().Length - 2, 2);

            return ca;
        }
        /// <summary>
        /// 取得金額檢碼
        /// </summary>
        /// <param name="amount">13位金額</param>
        /// <returns></returns>
        private static string Get_CheckAmount(string amount)
        {
            string ca = string.Empty;

            int A1 = Convert.ToInt32(amount.Substring(0, 1)) * 1;
            int A2 = Convert.ToInt32(amount.Substring(1, 1)) * 3;
            int A3 = Convert.ToInt32(amount.Substring(2, 1)) * 2;
            int A4 = Convert.ToInt32(amount.Substring(6, 1)) * 7;
            int A5 = Convert.ToInt32(amount.Substring(7, 1)) * 9;
            int A6 = Convert.ToInt32(amount.Substring(8, 1)) * 5;
            int A7 = Convert.ToInt32(amount.Substring(12, 1)) * 4;
            int x = A1 + A2 + A3 + A4 + A5 + A6 + A7;
            int S3 = 11 - (x % 11);

            A1 = Convert.ToInt32(amount.Substring(3, 1)) * 1;
            A2 = Convert.ToInt32(amount.Substring(4, 1)) * 7;
            A3 = Convert.ToInt32(amount.Substring(5, 1)) * 2;
            A4 = Convert.ToInt32(amount.Substring(9, 1)) * 6;
            A5 = Convert.ToInt32(amount.Substring(10, 1)) * 8;
            A6 = Convert.ToInt32(amount.Substring(11, 1)) * 9;
            x = A1 + A2 + A3 + A4 + A5 + A6;
            int S4 = 11 - (x % 11);

            string z = (S3 - S3 / 10 * 10).ToString() + (S4 - S4 / 10 * 10).ToString();
            int r = Convert.ToInt32(z) + 88;
            ca = r.ToString().Substring(r.ToString().Length - 2, 2);

            return ca;
        }
        #endregion

        #endregion 金融機構








        /// <summary>
        /// 轉帳日期
        /// </summary>
        /// <param name="pay_ad">發薪機關</param>
        /// <param name="t_date">入帳日期</param>
        /// <param name="type">類型(1=民國6碼；2=西元8碼；3=民國7碼；4=民國年月5碼；5=民國年3碼)</param>
        /// <returns></returns>
        public static string FORMAT_DATE(string t_date, int type)
        {
            // 已經強迫user輸入正確字串
            // 懶得改 先直接回傳原本的字串
            return t_date;
            /*
            if (type == 1)
            {
                DateTime dt = DateTime.Parse(t_date);
                string date = string.Format("{0}{1}{2}", int.Parse(dt.Year.ToString()) - 1911, dt.Month.ToString().PadLeft(2, '0'), dt.Day.ToString().PadLeft(2, '0'));
                return date;
            }
            else if (type == 3)
            {
                DateTime dt = DateTime.Parse(t_date);
                string date = string.Format("{0}{1}{2}", (int.Parse(dt.Year.ToString()) - 1911).ToString().PadLeft(3, '0'), dt.Month.ToString().PadLeft(2, '0'), dt.Day.ToString().PadLeft(2, '0'));
                return date;
            }
            else if (type == 2)
            {
                DateTime dt = DateTime.Parse(t_date);
                string date = string.Format("{0}{1}{2}", dt.Year.ToString().PadLeft(4, '0'), dt.Month.ToString().PadLeft(2, '0'), dt.Day.ToString().PadLeft(2, '0'));
                return date;
            }
            else if (type == 4)
            {
                DateTime dt = DateTime.Parse(t_date);
                string date = string.Format("{0}{1}", (int.Parse(dt.Year.ToString()) - 1911).ToString().PadLeft(3, '0'), dt.Month.ToString().PadLeft(2, '0'));
                return date;
            }
            else
            {
                DateTime dt = DateTime.Parse(t_date);
                string date = string.Format("{0}", (int.Parse(dt.Year.ToString()) - 1911).ToString().PadLeft(3, '0'));
                return date;
            }*/
        }
    }


    
    /// <summary>
    /// 轉帳回饋檔部分(銀行回來)
    /// </summary>
    public class Salary_BACK_BANK
    {
        /// <summary>
        /// 金融機構回饋檔匯入
        /// </summary>
        /// <param name="strPAY_AD"></param>
        /// <param name="strType"></param>
        /// <param name="strDataDate">資料日期</param>
        /// <param name="strT_Date">入帳日期</param>
        /// <param name="strBank_id"></param>
        /// <param name="strMemo"></param>
        /// <param name="strCaseID"></param>
        /// <param name="strBatchNumber"></param>
        /// <param name="encryption_type"></param>
        /// <param name="old"></param>
        public static void sb_Is_Bank_export(   Stream oSR, string strPAY_AD, string strType, string strDataDate, string strT_Date, string strBank_id,  string strCaseID, int encryption_type)
        {
            //StringBuilder sbData = new StringBuilder();

            switch (strBank_id)
            {
                case "005"://土地銀行
                    sr_LandBank_BackBank(oSR, strPAY_AD, strT_Date, strType, strCaseID, strBank_id);
                    break;
                case "008"://華南銀行
                   sr_SouthBank_BackBank(oSR, strPAY_AD, strT_Date, strType, strCaseID, strBank_id);
                   
                    break;
                case "004"://臺灣銀行
                    sr_TaiwanBank_BackBank(oSR, strPAY_AD, strT_Date, strType, strCaseID, strBank_id);
                    break;
                case "700"://中華郵政
                    sr_PostOffice_BackBank(oSR, strPAY_AD, strT_Date, strType, strCaseID, strBank_id);
                    break;
                case "951"://農會
                    sr_Farmers_Cooperative_BackBank(oSR, strPAY_AD, strT_Date, strType, strCaseID, strBank_id);
                    break;
                case "119"://淡水一信
                    sr_One_BackBank(oSR, strPAY_AD, strT_Date, strType, strCaseID, strBank_id);
                    break;
                default:

                    // return new StringBuilder("不支援此銀行格式！！");
                    break;

            }
            //return sbData;
        }


        #region 台灣銀行 004
        
        private static void sr_TaiwanBank_BackBank( Stream oS ,string strPAY_AD, string strT_Date, string strType,string strCaseID, string BANK_ID)
        {
            oS.Position = 0;//因為檢查入帳日期是否有與輸入的符合已經使用過一次.要把記憶體的位置重置

            StreamReader oSR = new StreamReader(oS ,System.Text.Encoding.Default );
            //StreamReader oSR = new StreamReader(oS, System.Text.Encoding.UTF8);

            int delete_no = 0;//只執行一次判斷
            string line = "";
            while ((line =  oSR.ReadLine()) != null)
            {
                if (string.IsNullOrEmpty(line.Trim()))
                    continue;

                //姓名碰到罕見字就會照字數來數

                //交易序號	起始位置1	總長度16
                string str001 = line.Substring(0, 16);

                //付款日期	起始位置17	總長度8
                string str002 = line.Substring(16, 8);

                //應付薪資金額	起始位置25	總長度12
                string str003 = line.Substring(24, 12);    

                //實付薪資金額	37	總長度12
                string str004 = line.Substring(36, 12);

                //手續費試算	起始位置49	總長度5
                string str005 = line.Substring(48, 5);


                //員工戶名	起始位置54	總長度80
                string str006 = o_str.SubString(line, 53, 80);

                //銀行代碼	起始位置134	總長度7
                string str007 = o_str.SubString(line, 133, 7);

                //員工帳號	起始位置141	總長度16
                string str008 = o_str.SubString(line, 140, 16);
                if (str008.Trim().Length != 12)// 姓名碰到罕見字就會照字數來數抓估計範圍值
                {

                    str008 = line.Substring(137, 15).Trim();

                }



                //手續費分攤方式	起始位置157	總長度1
                string str009 = o_str.SubString(line, 156, 1);


                //備註	起始位置158	總長度80
                string str010 = o_str.SubString(line, 157, 80);

                //員工身分證號	起始位置238	總長度10
                string str011 = o_str.SubString(line, 237, 10);
                if (str011.Trim().Length != 10)// // 姓名碰到罕見字就會照字數來數抓估計範圍值
                {

                    str011 = line.Substring(227, 20).Trim();
                
                }

                //員工e-mail	起始位置248	總長度50
                string str012 = o_str.SubString(line, 247, 50);

                //空白	起始位置298	總長度32            
                string str013 = o_str.SubString(line, 297, 32);

                /////////回饋之後


                string str014 = o_str.SubString(line, 329, 1);

                string str015 = o_str.SubString(line, 330, 4);

                string str016 = o_str.SubString(line, 334, 80);

                string str017 = o_str.SubString(line, 414, 8);


                // //刪除舊有的匯入資料

                if (delete_no == 0)//只執行一次
                {
                    delete_old_data(strPAY_AD, strType, str011.Trim(), str002.Trim(), str006.Trim(), str008.Trim(), int.Parse(str004.Trim()), strCaseID, BANK_ID, str010.Trim());
                    //delete_no = 1;
                }

                delete_no= delete_no+1;

                //薪資項目.先以備註代替  OR strType
                insert_table(strPAY_AD,strType, str011.Trim(), str002.Trim(), str006.Trim(), str008.Trim(), int.Parse(str004.Trim()), strCaseID, BANK_ID, str010.Trim(), delete_no);
                
                    
                
                string stop = "";
            }

        }

        #endregion 台灣銀行

        #region 郵局 700
        
        private static void sr_PostOffice_BackBank(Stream oS, string strPAY_AD, string strT_Date, string strType, string strCaseID, string BANK_ID)
        {
            oS.Position = 0;//因為檢查入帳日期是否有與輸入的符合已經使用過一次.要把記憶體的位置重置

            //StreamReader oSR = new StreamReader(oS, System.Text.Encoding.UTF8);
            StreamReader oSR = new StreamReader(oS, System.Text.Encoding.Default);
            int delete_no = 0;//只執行一次判斷

            string line = "";
            int i = 0;
            string IN_ACCOUNT_DATE = "";
            string ACCOUNT = "";
            string IDCARD = "";
            string NAME = "";
            int PAY = 0;
            while ((line = oSR.ReadLine()) != null)
            {

                if (i == 0)
                {


                }
                else if (i == 2)
                {
                    IN_ACCOUNT_DATE = o_str.SubString(line, 25, 9).Trim();

                }
                else if (i == 3)
                {

                }
                else if (i > 6)
                {
                    if (string.IsNullOrEmpty(line.Trim()))
                    {
                        break;

                        // oSR.ReadToEnd();
                        //continue;
                    }

                    ACCOUNT = o_str.SubString(line, 0, 7) + o_str.SubString(line, 9, 7);
                    IDCARD = o_str.SubString(line, 18, 10);
                    PAY = int.Parse(o_str.SubString(line, 30, 8));
                    NAME = o_A_DLBASE.CNAME(IDCARD);//用回去找姓名的.怕會有誤



                    // //刪除舊有的匯入資料

                    if (delete_no == 0)//只執行一次
                    {
                        delete_old_data(strPAY_AD, strType, IDCARD, IN_ACCOUNT_DATE, NAME, ACCOUNT, PAY, strCaseID, BANK_ID, "");
                        //delete_no = 1;
                    }
                    delete_no = delete_no + 1;

                    insert_table(strPAY_AD, strType, IDCARD, IN_ACCOUNT_DATE, NAME, ACCOUNT,  PAY,strCaseID,BANK_ID, "", delete_no);



                }
                else
                {

                }

                i++;
            }


        }
        #endregion 郵局

        #region 華南銀行 008
        
        private static void sr_SouthBank_BackBank(Stream oS, string strPAY_AD, string strT_Date, string strType, string strCaseID, string BANK_ID)
        {
            oS.Position = 0;//因為檢查入帳日期是否有與輸入的符合已經使用過一次.要把記憶體的位置重置

            StreamReader oSR = new StreamReader(oS, System.Text.Encoding.Default);

            int delete_no = 0;//只執行一次判斷

            string line = "";

            string IN_ACCOUNT_DATE = "";
            string ACCOUNT = "";

            string IDCARD = "";

            int PAY = 0;

            string ACCOUNT_NAME = "";//戶名要看轉存檔給不給家

            while ((line = oSR.ReadLine()) != null)
            {

                string date = o_str.SubString(line, 0, 8);
                IN_ACCOUNT_DATE = (int.Parse(date.Substring(0, 4)) - 1911).ToString() + date.Substring(4, 4);
                ACCOUNT = o_str.SubString(line, 22, 16).TrimStart('0');//這裡不能戶名是有0開頭的
                IDCARD = o_str.SubString(line, 38, 10);
                PAY = int.Parse(o_str.SubString(line, 51, 11));
                ACCOUNT_NAME = o_str.SubString(line, 87, 40).Trim();//戶名要看轉存檔給不給家
                //NAME = o_A_DLBASE.CNAME(IDCARD);//否則就要回去找姓名的.怕會有誤



                if (delete_no == 0)//只執行一次
                {
                    delete_old_data(strPAY_AD, strType, IDCARD, IN_ACCOUNT_DATE, ACCOUNT_NAME, ACCOUNT, PAY, strCaseID, BANK_ID, "");
                    //delete_no = 1;
                }
                delete_no = delete_no + 1;

                insert_table(strPAY_AD, strType, IDCARD, IN_ACCOUNT_DATE, ACCOUNT_NAME, ACCOUNT, PAY, strCaseID, BANK_ID, "", delete_no);

            }

        }

        #endregion 華南銀行

        #region 土地銀行 005
        
        private static void sr_LandBank_BackBank(Stream oS, string strPAY_AD, string strT_Date, string strType, string strCaseID, string BANK_ID)
        {
            oS.Position = 0;//因為檢查入帳日期是否有與輸入的符合已經使用過一次.要把記憶體的位置重置

            StreamReader oSR = new StreamReader(oS, System.Text.Encoding.Default);
            int delete_no = 0;//只執行一次判斷

            string line = "";

            string IN_ACCOUNT_DATE = "";
            string ACCOUNT = "";

            string IDCARD = "";

            int PAY = 0;

            string ACCOUNT_NAME = "";

            while ((line = oSR.ReadLine()) != null)
            {
                if (string.IsNullOrEmpty(line.Trim()))
                    continue;

                if (line.Substring(0, 1) == "2")
                {
                   

                    ACCOUNT = o_str.SubString(line, 38, 12);//目前數過應該全為12碼; 有0做開頭的



                    IN_ACCOUNT_DATE = o_str.SubString(line, 23, 7);

                    PAY = int.Parse(o_str.SubString(line, 50, 12));


                    DataTable NAME_ID= o_DBFactory.ABC_toTest.Create_Table(@" SELECT  DISTINCT IDCARD  FROM  B_BASE_STOCKPILE
                                                                  WHERE B_BASE_STOCKPILE.STOCKPILE_BANKID='" + ACCOUNT  + "' ", "get");

                    if (NAME_ID.Rows.Count > 0)
                    {

                        IDCARD = NAME_ID.Rows[0][0].ToString(); //不知會不會帳號有不同筆身分證情況


                        ACCOUNT_NAME = o_A_DLBASE.CNAME(IDCARD);//否則就要回去找姓名的.怕會有誤
                    }
                    else
                    {
                        NAME_ID.Clear();
                        NAME_ID = new DataTable();

                        NAME_ID = o_DBFactory.ABC_toTest.Create_Table(@" SELECT  DISTINCT IDCARD  FROM B_MANUFACTURER_BASE
                                  WHERE B_MANUFACTURER_BASE.BANK_ID='" + ACCOUNT + "' ", "get");

                        if (NAME_ID.Rows.Count > 0)
                        {
                            IDCARD = NAME_ID.Rows[0][0].ToString(); //不知會不會帳號有不同筆身分證情況


                            ACCOUNT_NAME = o_A_DLBASE.CNAME(IDCARD);//否則就要回去找姓名的.怕會有誤

                        }
                        else
                        {


                            IDCARD = ""; //不知會不會帳號有不同筆身分證情況


                            ACCOUNT_NAME = "";//否則就要回去找姓名的.怕會有誤
                        }
                    }


                    if (delete_no == 0)//只執行一次
                    {
                        delete_old_data(strPAY_AD, strType, IDCARD, IN_ACCOUNT_DATE, ACCOUNT_NAME, ACCOUNT, PAY, strCaseID, BANK_ID, "");
                        //delete_no = 1;
                    }
                    delete_no = delete_no + 1;

                    insert_table(strPAY_AD, strType, IDCARD, IN_ACCOUNT_DATE, ACCOUNT_NAME, ACCOUNT, PAY, strCaseID, BANK_ID, "", delete_no);

                }


            }

        }
        #endregion 土地銀行

        #region 淡水一信 119
        
        private static void sr_One_BackBank(Stream oS, string strPAY_AD, string strT_Date, string strType, string strCaseID, string BANK_ID)
        {
            oS.Position = 0;//因為檢查入帳日期是否有與輸入的符合已經使用過一次.要把記憶體的位置重置

            StreamReader oSR = new StreamReader(oS, System.Text.Encoding.Default);
            int delete_no = 0;//只執行一次判斷

            string line = "";

            string IN_ACCOUNT_DATE = "";
            string ACCOUNT = "";

            string IDCARD = "";

            int PAY = 0;

            string ACCOUNT_NAME = "";

            while ((line = oSR.ReadLine()) != null)
            {
                if (string.IsNullOrEmpty(line.Trim()))
                    continue;

                if (!string.IsNullOrEmpty(line))
                {
                    ACCOUNT = o_str.SubString(line, 35, 13);

                    IN_ACCOUNT_DATE = o_str.SubString(line, 0, 8);

                    IN_ACCOUNT_DATE = (int.Parse(IN_ACCOUNT_DATE.Substring(0, 4)) - 1911).ToString().PadLeft(3, '0') + IN_ACCOUNT_DATE.Substring(4, 4);

                    PAY = int.Parse(o_str.SubString(line, 58, 12));

                    IDCARD = o_str.SubString(line, 48, 10);

                    ACCOUNT_NAME = o_A_DLBASE.CNAME(IDCARD);
                }





                if (delete_no == 0)//只執行一次
                {
                    delete_old_data(strPAY_AD, strType, IDCARD, IN_ACCOUNT_DATE, ACCOUNT_NAME, ACCOUNT, PAY, strCaseID, BANK_ID, "");
                    
                }
                delete_no = delete_no + 1;

               insert_table(strPAY_AD, strType, IDCARD, IN_ACCOUNT_DATE, ACCOUNT_NAME, ACCOUNT, PAY, strCaseID, BANK_ID, "", delete_no);




            }

        }
        #endregion 淡水一信

        #region 農會 951
        
        private static void sr_Farmers_Cooperative_BackBank(Stream oS, string strPAY_AD, string strT_Date, string strType, string strCaseID, string BANK_ID)
        {
            oS.Position = 0;//因為檢查入帳日期是否有與輸入的符合已經使用過一次.要把記憶體的位置重置

            StreamReader oSR = new StreamReader(oS, System.Text.Encoding.Default);
            int delete_no = 0;//只執行一次判斷

            string line = "";

            string IN_ACCOUNT_DATE = "";
            string ACCOUNT = "";

            string IDCARD = "";

            int PAY = 0;

            string ACCOUNT_NAME = "";

            while ((line = oSR.ReadLine()) != null)
            {
                if (string.IsNullOrEmpty(line.Trim()))
                    continue;

                if (!string.IsNullOrEmpty(line))
                {
                    //20141217
                    IN_ACCOUNT_DATE = o_str.SubString(line, 0, 8);
                    //IN_ACCOUNT_DATE = o_str.SubStr(line, 43, 8);
                    IN_ACCOUNT_DATE = (int.Parse(IN_ACCOUNT_DATE.Substring(0, 4)) - 1911).ToString().PadLeft(3, '0') + IN_ACCOUNT_DATE.Substring(4, 4);

                    PAY = int.Parse(o_str.SubString(line, 58, 12));
                    //回饋檔             實際帳號
                    //05012173365500    121733655
                    //05012101176301    121011763010
                    //05012170584800    121705848

                    //05012171836100    12171836100
                    //05012172204000    12172204



                    DataTable NAME_ID = o_DBFactory.ABC_toTest.Create_Table(@"  SELECT  DISTINCT IDCARD,B_BASE_STOCKPILE.STOCKPILE_BANKID  FROM  B_BASE_STOCKPILE
                                                                  WHERE '050' + RPAD(B_BASE_STOCKPILE.STOCKPILE_BANKID,'12','0')='" + o_str.SubString(line, 34, 14) + "' + '0'", "get");

                    if (NAME_ID.Rows.Count > 0)
                    {
                        ACCOUNT = NAME_ID.Rows[0][1].ToString();
                        IDCARD = NAME_ID.Rows[0][0].ToString(); //不知會不會帳號有不同筆身分證情況


                        ACCOUNT_NAME = o_A_DLBASE.CNAME(IDCARD);//否則就要回去找姓名的.怕會有誤
                    }
                    else
                    {
                        NAME_ID.Clear();
                        NAME_ID = new DataTable();

                        NAME_ID = o_DBFactory.ABC_toTest.Create_Table(@" SELECT  DISTINCT IDCARD,BANK_ID  FROM B_MANUFACTURER_BASE
                                  WHERE '05' + RPAD(B_MANUFACTURER_BASE.BANK_ID,'12','0')='" + o_str.SubString(line, 34, 14) + "' ", "get");

                        if (NAME_ID.Rows.Count > 0)
                        {
                            IDCARD = NAME_ID.Rows[0][0].ToString(); //不知會不會帳號有不同筆身分證情況
                            ACCOUNT = NAME_ID.Rows[0][1].ToString();

                            ACCOUNT_NAME = o_A_DLBASE.CNAME(IDCARD);//否則就要回去找姓名的.怕會有誤

                        }
                        else
                        {


                            IDCARD = ""; //不知會不會帳號有不同筆身分證情況


                            ACCOUNT_NAME = "";//否則就要回去找姓名的.怕會有誤
                        }

                    }

                    
//                    IN_ACCOUNT_DATE = o_str.SubStr(line, 43, 8);
//                    IN_ACCOUNT_DATE = (int.Parse(IN_ACCOUNT_DATE.Substring(0, 4)) - 1911).ToString().PadLeft(3, '0') + IN_ACCOUNT_DATE.Substring(4, 4);

//                    PAY = int.Parse(o_str.SubStr(line, 59, 11));



//                    DataTable NAME_ID = o_DBFactory.ABC_toTest.Create_Table(@"  SELECT  DISTINCT IDCARD,B_BASE_STOCKPILE.STOCKPILE_BANKID  FROM  B_BASE_STOCKPILE
//                                                                  WHERE '050' + RPAD(B_BASE_STOCKPILE.STOCKPILE_BANKID,'11','0')='" + o_str.SubStr(line, 0, 14) + "'  ", "get");

//                    if (NAME_ID.Rows.Count > 0)
//                    {
//                        ACCOUNT = NAME_ID.Rows[0][1].ToString();
//                        IDCARD = NAME_ID.Rows[0][0].ToString(); //不知會不會帳號有不同筆身分證情況


//                        ACCOUNT_NAME = o_A_DLBASE.CNAME(IDCARD);//否則就要回去找姓名的.怕會有誤
//                    }
//                    else
//                    {
//                        NAME_ID.Clear();
//                        NAME_ID = new DataTable();

//                        NAME_ID = o_DBFactory.ABC_toTest.Create_Table(@" SELECT  DISTINCT IDCARD,BANK_ID  FROM B_MANUFACTURER_BASE
//                                  WHERE '05' + RPAD(B_MANUFACTURER_BASE.BANK_ID,'12','0')='" + o_str.SubStr(line, 0, 14) + "' ", "get");

//                        if (NAME_ID.Rows.Count > 0)
//                        {
//                            IDCARD = NAME_ID.Rows[0][0].ToString(); //不知會不會帳號有不同筆身分證情況
//                            ACCOUNT = NAME_ID.Rows[0][1].ToString();

//                            ACCOUNT_NAME = o_A_DLBASE.CNAME(IDCARD);//否則就要回去找姓名的.怕會有誤

//                        }
//                        else
//                        {


//                            IDCARD = ""; //不知會不會帳號有不同筆身分證情況


//                            ACCOUNT_NAME = "";//否則就要回去找姓名的.怕會有誤
//                        }

//                    }




                }





                if (delete_no == 0)//只執行一次
                {
                    delete_old_data(strPAY_AD, strType, IDCARD, IN_ACCOUNT_DATE, ACCOUNT_NAME, ACCOUNT, PAY, strCaseID, BANK_ID, "");
                    //delete_no = 1;
                }
                delete_no = delete_no + 1;

                insert_table(strPAY_AD, strType, IDCARD, IN_ACCOUNT_DATE, ACCOUNT_NAME, ACCOUNT, PAY, strCaseID, BANK_ID, "", delete_no);




            }

        }
        #endregion 農會

        //刪除舊有的匯入資料
        private static void delete_old_data(string strPAY_AD, string SALARY_TYPE, string IDCARD, string IN_ACCOUNT_DATE, string ACCOUNT_NAME, string ACCOUNT_NO, int PAY, string strCaseID, string BANK_ID, string MEMO)
        {
            //這樣比對一定漏洞很多.因為一定沒辦法刪得很徹底.待強者修復
            string d_sql = @"DELETE B_BANK_BACK_TXT WHERE  SALARY_TYPE=@SALARY_TYPE AND 
                             IN_ACCOUNT_DATE=@IN_ACCOUNT_DATE AND     BANKID=@BANKID AND PAY_AD=@PAY_AD ";

            List<SqlParameter> d_Para = new List<SqlParameter>();

            d_Para.Add(new SqlParameter("SALARY_TYPE", SqlDbType.VarChar) { Value = SALARY_TYPE });
            //d_Para.Add(new SqlParameter("IDCARD", SqlDbType.VarChar) { Value = IDCARD });
            d_Para.Add(new SqlParameter("IN_ACCOUNT_DATE", SqlDbType.VarChar) { Value = IN_ACCOUNT_DATE });
            d_Para.Add(new SqlParameter("BANKID", SqlDbType.VarChar) { Value = BANK_ID });
            d_Para.Add(new SqlParameter("PAY_AD", SqlDbType.VarChar) { Value = strPAY_AD });

            if (SALARY_TYPE == "SOLE" || SALARY_TYPE == "REPAIR")//單一發放因為有同一批號 同一人有兩筆的情形.要特殊處理
            {
                d_sql = @"DELETE B_BANK_BACK_TXT WHERE  SALARY_TYPE=@SALARY_TYPE  AND 
                             IN_ACCOUNT_DATE=@IN_ACCOUNT_DATE AND     BANKID=@BANKID AND PAY_AD=@PAY_AD AND CASEID=@CASEID   ";

                //d_Para.Add(new SqlParameter("PAY", SqlDbType.Float) { Value = PAY });
                d_Para.Add(new SqlParameter("CASEID", SqlDbType.VarChar) { Value = strCaseID });
            }
            
            o_DBFactory.ABC_toTest.SQLExecute(d_sql, d_Para);
        }


        //寫入資料
        private static void insert_table(string strPAY_AD, string SALARY_TYPE, string IDCARD, string IN_ACCOUNT_DATE, string ACCOUNT_NAME, string ACCOUNT_NO, int PAY, string strCaseID, string BANK_ID, string MEMO, int IMPORT_SN)
        {



            string I_sql = @"INSERT INTO B_BANK_BACK_TXT (SN, SALARY_TYPE,IDCARD, IN_ACCOUNT_DATE , ACCOUNT_NAME , ACCOUNT_NO , PAY ,BANKID, MEMO ,PAY_AD,ADD_DATE ,CASEID ,IMPORT_SN ,ADDUSER) VALUES 
                                              (  NEXT VALUE FOR dbo.B_BANK_BACK_TXT_SN , @SALARY_TYPE,@IDCARD, @IN_ACCOUNT_DATE , @ACCOUNT_NAME , @ACCOUNT_NO , @PAY ,@BANKID, @MEMO ,@PAY_AD,@ADD_DATE ,@CASEID ,@IMPORT_SN ,@ADDUSER)";

                            List<SqlParameter> _Para = new List<SqlParameter>();

                            _Para.Add(new SqlParameter("SALARY_TYPE", SqlDbType.VarChar) { Value = SALARY_TYPE });
                            _Para.Add(new SqlParameter("IDCARD", SqlDbType.VarChar) { Value = IDCARD });
                            _Para.Add(new SqlParameter("IN_ACCOUNT_DATE", SqlDbType.VarChar) { Value = IN_ACCOUNT_DATE });
                            _Para.Add(new SqlParameter("ACCOUNT_NAME", SqlDbType.VarChar) { Value = ACCOUNT_NAME });
                            _Para.Add(new SqlParameter("ACCOUNT_NO", SqlDbType.VarChar) { Value = ACCOUNT_NO });
                            _Para.Add(new SqlParameter("PAY", SqlDbType.Float) { Value = PAY });
                            _Para.Add(new SqlParameter("CASEID", SqlDbType.VarChar) { Value = strCaseID });
                            _Para.Add(new SqlParameter("BANKID", SqlDbType.VarChar) { Value = BANK_ID });
                            _Para.Add(new SqlParameter("MEMO", SqlDbType.VarChar) { Value = MEMO });
                            _Para.Add(new SqlParameter("PAY_AD", SqlDbType.VarChar) { Value = strPAY_AD });
                            _Para.Add(new SqlParameter("ADD_DATE", SqlDbType.DateTime) { Value = DateTime.Now  });
                            _Para.Add(new SqlParameter("IMPORT_SN", SqlDbType.Float) { Value = IMPORT_SN });
                            _Para.Add(new SqlParameter("ADDUSER", SqlDbType.VarChar) { Value = HttpContext.Current.Session["ADPMZ_ID"].ToString() });
                            


                            o_DBFactory.ABC_toTest.SQLExecute(I_sql, _Para);
        
        }


    }


    
    /// <summary>
    /// 台灣銀行文字檔加密
    /// </summary>
    public class SalaryToBank_encryption
    {
        /// <summary>
        /// 台灣銀行
        /// </summary>
        public class TaiwanBank
        {
            /// <summary>
            /// 資料加密處理
            /// </summary>
            /// <param name="sKey">private key</param>
            /// <param name="sData">encrypeted data</param>
            /// <returns></returns>
            public static string DESEncrypt(string sKey, string sData)
            {
                byte[] byteKey = null;
                string sResult = "";

                byte[] byteData = System.Text.Encoding.Default.GetBytes(sData);
                byteKey = System.Text.Encoding.ASCII.GetBytes(sKey);

                if (byteKey.Length != 16 && byteKey.Length != 24)
                {
                    //黃頁顯示錯誤
                    throw new Exception("密碼長度為" + byteKey.Length + "字，非此演算法的有效大小，密碼長度需為16或24個字元");


                }

                TripleDESCryptoServiceProvider oDES = new TripleDESCryptoServiceProvider();
                oDES.Mode = CipherMode.ECB;//請指定 CipherMode 
                oDES.Key = byteKey;
                //oDES.IV = byteKey;

                MemoryStream msStream = new MemoryStream();

                ICryptoTransform olCryptoTransform = oDES.CreateEncryptor();

                CryptoStream csStream = new CryptoStream(msStream, olCryptoTransform, CryptoStreamMode.Write);

                csStream.Write(byteData, 0, byteData.Length);
                csStream.FlushFinalBlock();
                sResult = Convert.ToBase64String(msStream.ToArray());

                msStream.Close();
                csStream.Close();
                byteKey = null;

                return sResult;


            }


            /// <summary>
            /// 資料解密處理
            /// </summary>
            /// <param name="sKey">private key</param>
            /// <param name="sData">decrypted data</param>
            /// <returns></returns>
            public static string DESDecrypt(string sKey, string sData)
            {
                byte[] byteKey = null;
                string sResult = "";


                byteKey = System.Text.Encoding.ASCII.GetBytes(sKey);
                byte[] byteIV = byteKey;
                byte[] byteData = Convert.FromBase64String(sData);

                if (byteKey.Length != 16 && byteKey.Length != 24)
                {
                    //黃頁顯示錯誤
                    throw new Exception("密碼長度為" + byteKey.Length + "自，非此演算法的有效大小，密碼長度需為16或24個字元");


                }

                MemoryStream msStream = new MemoryStream();

                TripleDESCryptoServiceProvider oDES = new TripleDESCryptoServiceProvider();
                //oDES.Padding = PaddingMode.None;  
                oDES.Mode = CipherMode.ECB;//請指定 CipherMode 



                CryptoStream csStream = new CryptoStream(msStream, oDES.CreateDecryptor(byteKey, byteIV), CryptoStreamMode.Write);

                csStream.Write(byteData, 0, byteData.Length);
                csStream.FlushFinalBlock();

                sResult = System.Text.Encoding.Default.GetString(msStream.ToArray());

                msStream.Close();
                csStream.Close();
                byteKey = null;

                return sResult;


            }


            /// <summary>
            /// 是否為弱勢憑證
            /// </summary>
            /// <param name="sKey">例:00000000000000 或 1234567891234567890 等等..</param>
            /// <returns></returns>
            public static bool IsWeakKey(string sKey)
            {
                return TripleDES.IsWeakKey(Encoding.Default.GetBytes(sKey));

            }
        }

    }
    

}