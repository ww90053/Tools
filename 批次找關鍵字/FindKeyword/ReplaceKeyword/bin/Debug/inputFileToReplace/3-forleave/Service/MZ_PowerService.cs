using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TPPDDB.Service
{
    /// <summary>
    /// 幫忙判斷 MZ_POWER 欄位對應的權限
    /// </summary>
    public static class MZ_PowerService
    {
        /// <summary>
        /// 根據Session儲存內容,抓取MZ_Power
        /// </summary>
        /// <returns></returns>
        public static string GetMZ_Power_BySession()
        {
            //避免無Session
            if (HttpContext.Current.Session["ADPMZ_ID"] == null)
            {
                return "";
            }
            //抓員警ID
            string MZ_ID = HttpContext.Current.Session["ADPMZ_ID"].ToString();
            //根據ID查出 MZ_Power,其實會優先根據Session設定,其次才抓DB
            return o_a_Function.strGID(MZ_ID);
        }

        /// <summary>
        /// 判斷權限等級ABCD,可否抓取全部的機關資料?
        /// </summary>
        /// <param name="MZ_POWER"></param>
        /// <returns></returns>
        public static bool isAll_AD()
        {
            return isAll_AD(GetMZ_Power_BySession());
        }

        /// <summary>
        /// 判斷權限等級ABCD,可否抓取全部的機關資料?
        /// </summary>
        /// <param name="MZ_POWER"></param>
        /// <returns></returns>
        public static bool isAll_AD(string MZ_POWER)
        {
            //AB等級可以,其他等級不行
            switch (MZ_POWER)
            {
                case "A":
                case "B":
                    return true;
                case "C":
                case "D":
                case "E":
                default:
                    return false;
            }
        }

        /// <summary>
        /// 判斷MZ_POWER權限等級ABCD,可否抓取全部的單位資料?
        /// EX: 某人為 板橋分局 板橋派出所
        /// 若可抓取,則不會鎖定派出所選項
        /// 
        /// </summary>
        /// <returns></returns>

        public static bool isAll_UNIT()
        {
            return isAll_UNIT(GetMZ_Power_BySession());
        }

        /// <summary>
        /// 判斷MZ_POWER權限等級ABCD,可否抓取全部的單位資料?
        /// EX: 某人為 板橋分局 板橋派出所
        /// 若可抓取,則不會鎖定派出所選項
        /// 
        /// </summary>
        /// <param name="MZ_POWER"></param>
        /// <returns></returns>
        public static bool isAll_UNIT(string MZ_POWER)
        {
            //ABCD等級可以,E等級不行
            switch (MZ_POWER)
            {
                case "A":
                case "B":
                case "C":
                case "D":
                    return true;
                case "E":
                default:
                    return false;
            }
        }
        public static bool isOK()
        {
            return isOK(GetMZ_Power_BySession());
        }
        /// <summary>
        /// 判斷MZ_POWER權限等級ABCD,是否有效?
        /// </summary>
        /// <param name="MZ_POWER"></param>
        /// <returns></returns>
        public static bool isOK(string MZ_POWER)
        {
            //ABCDE等級可以,其他不認識的不行
            switch (MZ_POWER)
            {
                case "A":
                case "B":
                case "C":
                case "D":
                case "E":
                    return true;
                default:
                    return false;
            }
        }
    }
}