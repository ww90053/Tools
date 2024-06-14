using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TPPDDB.Helpers;

namespace TPPDDB._3_forleave
{
    public static class Service_C_DUTYMONTHOVERTIME_HOUR
    {
        /// <summary>
        /// 檢查是否已經有核定過的資料存在於當天?
        /// </summary>
        /// <param name="MZ_ID">人員ID</param>
        /// <param name="MZ_YEAR">民國年</param>
        /// <param name="MZ_MONTH">月份,要補0</param>
        /// <param name="MZ_AD">編制機關,這邊基本上是根據UI上輸入的機關為準</param>
        /// <param name="MZ_EXUNIT">現服單位代碼</param>
        /// <returns></returns>
        public static bool CheckExist(string MZ_ID
               , string MZ_YEAR, string MZ_MONTH
            , string MZ_AD, string MZ_EXUNIT
            )
        {
            string SQL = $@"
            --
            select count(*) as cnt
            from C_DUTYMONTHOVERTIME_HOUR 
            where 1=1
            and MZ_YEAR='{MZ_YEAR}'
            and MZ_MONTH='{MZ_MONTH}'
            and MZ_AD='{MZ_AD}'
            and MZ_EXUNIT='{MZ_EXUNIT}'
            and MZ_VERIFY='Y'
            ";
            string cnt = o_DBFactory.ABC_toTest.Get_First_Field(SQL, new List<SqlParameter>());
            //
            return (int.Parse(cnt) > 0);
        }
    }
}