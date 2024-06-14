using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace TPPDDB._18_Online_Leave.Service
{
    public class Service_C_LEAVE_SCHEDULE_CODE
    {
        /// <summary>
        /// 可能沒有意義 ,根據目前的關卡代碼, 抓取對應的機關單位
        /// </summary>
        /// <param name="sCHEDULE"></param>
        /// <returns></returns>
        public static DataTable Get_AD_UNIT(string sCHEDULE)
        {
            return o_DBFactory.ABC_toTest.Create_Table("SELECT MZ_EXAD,MZ_EXUNIT FROM C_LEAVE_SCHEDULE_CODE WHERE CODE_VALUE = '" + sCHEDULE + "'", "get");
        }
    }
}