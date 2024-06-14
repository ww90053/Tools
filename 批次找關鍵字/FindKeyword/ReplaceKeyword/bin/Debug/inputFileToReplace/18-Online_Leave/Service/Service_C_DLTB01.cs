using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace TPPDDB._18_Online_Leave.Service
{
    public class Service_C_DLTB01
    {
        /// <summary>
        /// 取得請假人的 ID 機關 單位
        /// </summary>
        /// <param name="DLTB01_SN"></param>
        /// <returns></returns>
        public static DataTable Get_MZ_ID_AD_UNIT(string DLTB01_SN)
        {
            return o_DBFactory.ABC_toTest.Create_Table(@"SELECT C_DLTB01 .MZ_ID,MZ_EXAD,MZ_EXUNIT 
                                              FROM C_DLTB01 
                                              
                                              WHERE MZ_DLTB01_SN ='" + DLTB01_SN + "'", "get");
        }
    }
}