using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace TPPDDB._18_Online_Leave.Service
{
    public class Service_C_STATUS
    {
        //Service_C_STATUS.Get_C_STATUS_SN(
        public static string Get_C_STATUS_SN(string status)
        {
            return o_DBFactory.ABC_toTest.vExecSQL("SELECT C_STATUS_SN FROM C_STATUS WHERE C_STATUS_NAME='" + status + "'");
        }
    }
}