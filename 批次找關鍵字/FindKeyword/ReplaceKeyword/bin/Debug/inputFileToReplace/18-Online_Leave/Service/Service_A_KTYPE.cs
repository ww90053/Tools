using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace TPPDDB._18_Online_Leave.Service
{
    public class Service_A_KTYPE
    {
        /// <summary>
        /// 取得機關代碼
        /// </summary>
        /// <returns></returns>
        public static DataTable GetDT_AD()
        {
            //初始化葉面選單
            return o_DBFactory.ABC_toTest.Create_Table("(SELECT  MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE = '04' AND MZ_KCODE LIKE '38213%') UNION ALL (SELECT  MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE = '04' AND MZ_KCODE LIKE '376411%')", "get");
        }
    }
}