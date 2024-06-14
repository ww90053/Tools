using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TPPDDB.Service
{
    public static class Salary
    {
        /// <summary>
        /// 取得每小時支領金額(至整數第一位之四捨五入)
        /// </summary>
        /// <param name="MZ_ID"></param>
        /// <returns></returns>
        public static String getHourPay(String MZ_ID)
        {
            _2_salary.Police Police = new TPPDDB._2_salary.Police(MZ_ID);

            if (Police.occc.Substring(0, 2) == "Z0")
                return MathHelper.Round(Convert.ToDouble(Police.salary + Police.profess + Police.boss) / 240 * 1.33).ToString();
            else
                return MathHelper.Round(Convert.ToDouble(Police.salary + Police.profess + Police.boss) / 240).ToString();
        }

        /// <summary>
        /// 重新計算每小時支領金額(部分客製化)
        /// </summary>
        /// <param name="MZ_ID"></param>
        /// <param name="CustomerBoss"></param>
        /// <returns></returns>
        public static String reduceHourPay(String MZ_ID, Int32 CustomerBoss = 0)
        {
            _2_salary.Police Police = new TPPDDB._2_salary.Police(MZ_ID);

            //0照樣計算
            //CustomerBoss = (CustomerBoss == 0) ? Police.boss : CustomerBoss;

            if (Police.occc.Substring(0, 2) == "Z0")
                return MathHelper.Round(Convert.ToDouble(Police.salary + Police.profess + CustomerBoss) / 240 * 1.33).ToString();
            else
                return MathHelper.Round(Convert.ToDouble(Police.salary + Police.profess + CustomerBoss) / 240).ToString();
        }
    }
}