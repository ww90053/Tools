using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace TPPDDB._18_Online_Leave.Service
{
    public class Service_C_REVIEW_MANAGEMENT
    {

        public static DataTable GetDT_REVIEW_LEVEL()
        {
            //GetDT_REVIEW_LEVEL
            DataTable dt = new DataTable();
            //dt = o_DBFactory.ABC_toTest.Create_Table("SELECT REVIEW_LEVEL FROM C_REVIEW_MANAGEMENT WHERE MZ_EXAD='"
            //   + SessionHelper.ADPMZ_EXAD + "'AND MZ_EXUNIT='0001' AND MZ_ID='" + SessionHelper.ADPMZ_ID + "'", "getvalue");
            dt = o_DBFactory.ABC_toTest.Create_Table("SELECT REVIEW_LEVEL FROM C_REVIEW_MANAGEMENT WHERE MZ_EXAD='"
                + SessionHelper.ADPMZ_EXAD + "'AND (MZ_EXUNIT='0001' OR MZ_EXUNIT='PAY0' OR MZ_EXUNIT='PAZ0') AND MZ_ID='" + SessionHelper.ADPMZ_ID + "'", "getvalue");
            // MZ_EXUNIT='" +   o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCODE FROM A_KTYPE WHERE  MZ_KCHI='人事室' AND MZ_KTYPE = '25'") +s
            return dt;
        }

        ///// <summary>
        ///// 取得
        ///// </summary>
        ///// <param name="EXAD"></param>
        ///// <param name="EXUNIT"></param>
        ///// <returns></returns>
        //public static string Get_MZ_ID_LEVEL_1_4(string EXAD, string EXUNIT)
        //{
        //    //Get_MZ_ID
        //    return o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_ID FROM c_review_management WHERE " +
        //                        " MZ_EXAD = '" + EXAD + "' AND MZ_EXUNIT = '" + EXUNIT +
        //                        "' AND REVIEW_LEVEL IN ('1','4')");
        //}
    }
}