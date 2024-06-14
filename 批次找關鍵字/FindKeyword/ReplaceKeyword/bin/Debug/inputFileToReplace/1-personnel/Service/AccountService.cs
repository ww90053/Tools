using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TPPDDB.Model;

namespace TPPDDB.Service
{
    /// <summary>帳戶類別</summary>
    public static class AccountService
    {
        /// <summary>
        /// 取得單一使用者之資料模型 for 姓名
        /// </summary>
        /// <param name="Name">姓名</param>
        /// <returns></returns>
        public static AccountModel lookupAccountByName(string Name)
        {
            AccountModel m = new AccountModel();

            string SQL = string.Format(@"SELECT MZ_ID, MZ_NAME, MZ_AD, MZ_UNIT, MZ_EXAD, MZ_EXUNIT, 
                                        MZ_OCCC, MZ_RANK, MZ_RANK1, MZ_SLVC, MZ_SPT, MZ_SRANK, MZ_STATUS2, PAY_AD
                                        FROM A_DLBASE WHERE MZ_NAME like '{0}%'",
                                        Name);

            DataTable dt = o_DBFactory.ABC_toTest.Create_Table(SQL, "A_DLBASE");

            if (dt.Rows.Count <= 0) return new AccountModel();
            else
            {
                DataRow r = dt.Rows[0];
                m.ID        = r["MZ_ID"].ToString();
                m.Name      = r["MZ_NAME"].ToString();
                m.AD        = r["MZ_AD"].ToString();
                m.Unit      = r["MZ_UNIT"].ToString();
                m.ExAD      = r["MZ_EXAD"].ToString();
                m.ExUnit    = r["MZ_EXUNIT"].ToString();
                m.Title     = r["MZ_OCCC"].ToString();
                m.RankBegin = r["MZ_RANK"].ToString();
                m.RankEnd   = r["MZ_RANK1"].ToString();
                m.SLVC      = r["MZ_SLVC"].ToString();
                m.SPT       = r["MZ_SPT"].ToString();
                m.SRANK     = r["MZ_SRANK"].ToString();
                m.isWorking = (r["MZ_STATUS2"].ToString() == "Y") ? true : false;
                m.PayAD     = r["PAY_AD"].ToString();
                return m;
            }
        }

        /// <summary>
        /// 取得單一使用者之資料模型
        /// </summary>
        /// <param name="ID">身分證號</param>
        /// <returns></returns>
        public static AccountModel lookupAccount(string ID)
        {
            AccountModel m = new AccountModel();

            string SQL = string.Format(@"SELECT MZ_ID, MZ_NAME, MZ_AD, MZ_UNIT, MZ_EXAD, MZ_EXUNIT, 
                                        MZ_OCCC, MZ_RANK, MZ_RANK1, MZ_SLVC, MZ_SPT, MZ_SRANK, MZ_STATUS2, PAY_AD
                                        FROM A_DLBASE WHERE MZ_ID='{0}'",
                                        ID);

            DataTable dt = o_DBFactory.ABC_toTest.Create_Table(SQL, "A_DLBASE");
            
            if (dt.Rows.Count <= 0) return new AccountModel();
            else
            {
                DataRow r = dt.Rows[0];
                m.ID        = r["MZ_ID"].ToString();
                m.Name      = r["MZ_NAME"].ToString();
                m.AD        = r["MZ_AD"].ToString();
                m.Unit      = r["MZ_UNIT"].ToString();
                m.ExAD      = r["MZ_EXAD"].ToString();
                m.ExUnit    = r["MZ_EXUNIT"].ToString();
                m.Title     = r["MZ_OCCC"].ToString();
                m.RankBegin = r["MZ_RANK"].ToString();
                m.RankEnd   = r["MZ_RANK1"].ToString();
                m.SLVC      = r["MZ_SLVC"].ToString();
                m.SPT       = r["MZ_SPT"].ToString();
                m.SRANK     = r["MZ_SRANK"].ToString();
                m.isWorking = (r["MZ_STATUS2"].ToString() == "Y") ? true : false;
                m.PayAD     = r["PAY_AD"].ToString();
                return m;
            }
        }

        /// <summary>取得某特定機關(與特定單位)底下所有帳號資料</summary>
        /// <param name="ID">身分證字號</param>
        /// <param name="AD">單位</param>
        /// <param name="UNIT">機關</param>
        /// <param name="Type">體制 : 編制機關單位(Build), 現服機關單位(Now)</param>
        /// <param name="AdditionalWHERE">額外的 WHERE 條件式。須直接組串，範例如 : 「A = 10 AND B = 'TEST'」</param>
        /// <returns>使用者模型清單</returns>
        public static List<AccountModel> lookupAccount(string ID, string AD, string UNIT, string Type, string AdditionalWHERE = "")
        {
            String SQL = @"SELECT {0} FROM A_DLBASE";
            List<String> WHEREs = new List<String>();
            List<AccountModel> results = new List<AccountModel>();

            //依照類型，撈取不同的資料欄位
            String AD_Column = "";
            String UNIT_Column = "";
            switch (Type)
            {
                case "Build"://編制機關單位
                    AD_Column = @"MZ_AD";
                    UNIT_Column = @"MZ_UNIT";
                    SQL = string.Format(SQL, "MZ_ID, MZ_NAME, MZ_AD, MZ_UNIT As MZ_EXUNIT, MZ_EXAD, MZ_EXUNIT As MZ_UNIT, MZ_OCCC, MZ_RANK, MZ_RANK1, MZ_SLVC, MZ_SPT, MZ_SRANK, MZ_STATUS2, PAY_AD");
                    break;
                case "Now" : //現服機關單位
                    AD_Column = @"MZ_EXAD";
                    UNIT_Column = @"MZ_EXUNIT";
                    SQL = string.Format(SQL, "MZ_ID, MZ_NAME, MZ_AD, MZ_UNIT, MZ_EXAD, MZ_EXUNIT, MZ_OCCC, MZ_RANK, MZ_RANK1, MZ_SLVC, MZ_SPT, MZ_SRANK, MZ_STATUS2, PAY_AD");
                    break;
                case "Pay" : //僅依照發薪單位
                    AD_Column = @"PAY_AD"; //機關改為發薪機關
                    UNIT_Column = @"MZ_UNIT"; //單位照舊。小隊長說不會有編制機關跟發薪機關底下單位不對等的情況發生
                    SQL = string.Format(SQL, "MZ_ID, MZ_NAME, MZ_AD, MZ_UNIT As MZ_EXUNIT, PAY_AD As MZ_EXAD, MZ_EXUNIT As MZ_UNIT, MZ_OCCC, MZ_RANK, MZ_RANK1, MZ_SLVC, MZ_SPT, MZ_SRANK, MZ_STATUS2, PAY_AD");
                    break;
            }

            //若有輸入 AD，則 WHERE 條件加入 AD 欄位
            //目前 AD 應設為必填，否則會撈出過多資料
            if (!String.IsNullOrEmpty(AD))
                WHEREs.Add(String.Format(@" {0} = '{1}' ", AD_Column, AD));

            if (!String.IsNullOrEmpty(UNIT))
                WHEREs.Add(String.Format(@" {0} = '{1}' ", UNIT_Column, UNIT));

            //身分證字號
            if (!String.IsNullOrEmpty(ID))
                WHEREs.Add(String.Format(@" {0} = '{1}' ", "MZ_ID", ID));

            //組合WHERE子句
            SQL += (WHEREs.Count > 0) ? @" WHERE " : @"";
            SQL += String.Join(@" AND ", WHEREs.ToArray());
            SQL += (AdditionalWHERE != "") ? @" AND " + AdditionalWHERE : @""; 

            DataTable dt = o_DBFactory.ABC_toTest.Create_Table(SQL, "A_DLBASE");

            if (dt.Rows.Count <= 0)
            {
                return new List<AccountModel>();
            }
            else
            {
                foreach (DataRow r in dt.Rows)
                {
                    AccountModel m = new AccountModel();
                    m.ID = r["MZ_ID"].ToString();
                    m.Name = r["MZ_NAME"].ToString();
                    m.AD = r["MZ_AD"].ToString();
                    m.Unit = r["MZ_UNIT"].ToString();
                    m.ExAD = r["MZ_EXAD"].ToString();
                    m.ExUnit = r["MZ_EXUNIT"].ToString();
                    m.Title = r["MZ_OCCC"].ToString();
                    m.RankBegin = r["MZ_RANK"].ToString();
                    m.RankEnd = r["MZ_RANK1"].ToString();
                    m.SLVC = r["MZ_SLVC"].ToString();
                    m.SPT = r["MZ_SPT"].ToString();
                    m.SRANK = r["MZ_SRANK"].ToString();
                    m.isWorking = (r["MZ_STATUS2"].ToString() == "Y") ? true : false;
                    m.PayAD = r["PAY_AD"].ToString();
                    results.Add(m);
                }
            }
            return results;
        }

        /// <summary>
        /// 檢查身分證字號是否正確
        /// </summary>
        /// <param name="ID">身分證號</param>
        /// <returns></returns>
        public static bool isIDRight(string ID)
        {
            if (String.IsNullOrEmpty(ID)) return false;

            Boolean isValid = false;
            //arg_Identify = arg_Identify.ToUpper(); //20151006 取消大小寫代為轉換部分，因大部分程式可能未檢查，或需要進行錯誤資料校正。故需顯示錯誤之身分證字號為何

            if (ID.Length == 10)
            {
                if (ID[0] >= 0x41 && ID[0] <= 0x5A)
                {
                    var a = new[] { 10, 11, 12, 13, 14, 15, 16, 17, 34, 18, 19, 20, 21, 22, 35, 23, 24, 25, 26, 27, 28, 29, 32, 30, 31, 33 };
                    var b = new int[11];
                        b[1] = a[(ID[0]) - 65] % 10;
                    var c = b[0] = a[(ID[0]) - 65] / 10;
                    for (var i = 1; i <= 9; i++)
                    {
                        b[i + 1] = ID[i] - 48;
                        c += b[i] * (10 - i);
                    }
                    if (((c % 10) + b[10]) % 10 == 0)
                    {
                        isValid = true;
                    }
                }
            }
            return isValid;
        }

        /// <summary>
        /// 取得參數對照設定，主要用於撈取目前的機關單位名稱
        /// </summary>
        /// <param name="target"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string getSysPara(string target, enumCategroy type)
        {
            String SQL = String.Format(@"SELECT MZ_KCODE, MZ_KCHI, MZ_KFIL FROM A_KTYPE WHERE MZ_KTYPE='{0}' and MZ_KCODE = '{1}'",
                                        enumCategroyParser(type),
                                        target);

            DataTable dt = o_DBFactory.ABC_toTest.Create_Table(SQL, "get");

            if (dt.Rows.Count <= 0) return "";
            else
            {
                DataRow r = dt.Rows[0];
                return r["MZ_KCHI"].ToString();
            }
        }

        #region 已經廢止
        /// <summary>
        /// (原始的抓法,尚待討論是否廢止)取得內外勤分別(透過 B_BASE)
        /// </summary>
        /// <param name="MZ_ID"></param>
        /// <returns></returns>
        public static enumDutyUserClassify getDutyUserClassify_V1(string MZ_ID)
        {
            enumDutyUserClassify result;

            //20210217 弈夫說 WORKP這個欄位資料現在沒有在維護了,改從WORKPPAY欄位進行判斷
            //String SQL = @"SELECT WORKP FROM B_BASE WHERE IDCARD='{0}'" ;
            String SQL = @"SELECT WORKPPAY FROM B_BASE WHERE IDCARD='{0}'";

            String WorkP = o_DBFactory.ABC_toTest.vExecSQL(String.Format(SQL, MZ_ID));
            //switch (WorkP)
            //{
            //    //case "03":
            //    //case "04":
            //    //    result = enumDutyUserClassify.Outter; break;
            //    //case "01":
            //    //case "02":
            //    //case "05":
            //    //case "06":
            //    //case "00":
            //    //default:
            //    //    result = enumDutyUserClassify.Inner; break;
            //}

            if (WorkP == "8435" || WorkP == "13496" 
                || WorkP == TPPDDB._2_salary.Salary.WorkP_8435.ToString()
                || WorkP == TPPDDB._2_salary.Salary.WorkP_13496.ToString()
                )
            { result = enumDutyUserClassify.Outter; }
            else
            { result = enumDutyUserClassify.Inner; }

            return result;
        }
        #endregion

        /// <summary>
        /// 取得內外勤分別(透過 A_DLBASE),這個版本先等資訊室確認之後再套用
        /// </summary>
        /// <param name="MZ_ID"></param>
        /// <returns></returns>
        public static enumDutyUserClassify getDutyUserClassify_V2(string MZ_ID)
        {
            enumDutyUserClassify result ;

            //20210217 弈夫說 WORKP這個欄位資料現在沒有在維護了,改從WORKPPAY欄位進行判斷
            //String SQL = @"SELECT WORKP FROM B_BASE WHERE IDCARD='{0}'" ;
            String SQL = @"SELECT DUTYUSERCLASSIFY FROM A_DLBASE WHERE MZ_ID='{0}'";

            String DUTYUSERCLASSIFY = o_DBFactory.ABC_toTest.vExecSQL(String.Format(SQL, MZ_ID));
            DUTYUSERCLASSIFY = DUTYUSERCLASSIFY ?? "";
            DUTYUSERCLASSIFY = DUTYUSERCLASSIFY.ToLower();

            if (DUTYUSERCLASSIFY == "inner")
            {
                return enumDutyUserClassify.Inner;
            }
            else
            {
                return enumDutyUserClassify.Outter;
            }
        }

        public enum enumCategroy
        {
            /// <summary>機關名稱</summary>
            AD = 4,
            /// <summary>單位名稱</summary>
            UNIT = 25,
            /// <summary>
            /// 職稱
            /// </summary>
            OCCC = 26,
        }

        /// <summary>內外勤人員分別</summary>
        public enum enumDutyUserClassify
        {
            /// <summary>外勤</summary>
            Outter = 1,
            /// <summary>內勤</summary>
            Inner = 2
        }

        private static string enumCategroyParser(enumCategroy type)
        {
            switch (type)
            {
                case enumCategroy.AD: //機關名稱
                    return "04";
                case enumCategroy.UNIT: //單位名稱
                    return "25";
                case enumCategroy.OCCC: //職稱
                    return "26";
                default:
                    return "";
            }
        }
    }
}