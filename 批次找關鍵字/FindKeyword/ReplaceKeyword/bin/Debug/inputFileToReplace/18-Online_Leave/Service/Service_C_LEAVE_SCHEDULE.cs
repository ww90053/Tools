using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace TPPDDB._18_Online_Leave.Service
{
    public class Service_C_LEAVE_SCHEDULE
    {
        public static string Get_SCHEDULE_CODE(int SORT_NO, string LEAVE_SN)
        {
            return o_DBFactory.ABC_toTest.vExecSQL("SELECT SCHEDULE_CODE FROM C_LEAVE_SCHEDULE WHERE DLCODE_CODE ='" + LEAVE_SN + "' AND SORT_NO = '" + SORT_NO + "'");
        }

        public static string Get_REVIEW_LEAVE(int SORT_NO, string LEAVE_SN)
        {
            return o_DBFactory.ABC_toTest.vExecSQL("SELECT REVIEW_LEAVE FROM C_LEAVE_SCHEDULE WHERE SORT_NO ='" + SORT_NO + "' AND DLCODE_CODE = '" + LEAVE_SN + "'");
        }
    }
}