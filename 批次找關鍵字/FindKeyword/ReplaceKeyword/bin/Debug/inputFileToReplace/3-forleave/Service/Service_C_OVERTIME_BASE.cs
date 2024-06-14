using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TPPDDB.Helpers;

namespace TPPDDB._3_forleave
{
    public static class Service_C_OVERTIME_BASE
    {

        /// <summary>
        /// 根據ID,上班日,找出當天的超勤補休時數,參考資料表:C_OVERTIME_BASE
        /// 
        /// </summary>
        /// <param name="MZ_ID"></param>
        /// <param name="OVER_DAY"></param>
        /// <returns></returns>
        public static int Get_iREST_HOURS(string MZ_ID, string OVER_DAY)
        {
            //則查找C_OVERTIME_BASE同一天(OVER_DAY)的資料是否有OVERTIME_TYPE = ‘OTU’的資料，
            string SQL_OTU = @"
                        SELECT REST_HOUR
                         FROM  C_OVERTIME_BASE
                         WHERE MZ_ID =@MZ_ID
                          AND OVER_DAY =@OVER_DAY
                           AND OVERTIME_TYPE = 'OTU'
";

            List<SqlParameter> SqlPrams = new List<SqlParameter>() {
                new SqlParameter(":MZ_ID",MZ_ID),
                new SqlParameter(":OVER_DAY",OVER_DAY)
            };
            //超勤補休時數
            int data = o_DBFactory.ABC_toTest.Get_First_Field(SQL_OTU, SqlPrams).SafeToInt();
            return data;
        }

        public static SqlCommand GetCommand_DeleteOTU(string MZ_ID, string OVER_DAY)
        {
            string DeleteString = @"
                        DELETE FROM C_OVERTIME_BASE
                         WHERE MZ_ID =@MZ_ID
                          AND OVER_DAY =@OVER_DAY
                            --加班類型:輪值補休
                           AND OVERTIME_TYPE = 'OTU'
                    ";
            List<SqlParameter> SqlPrams = new List<SqlParameter>() {
                new SqlParameter(":MZ_ID",MZ_ID),
                new SqlParameter(":OVER_DAY",OVER_DAY)
            };
            return o_DBFactory.ABC_toTest.Get_Command(DeleteString, SqlPrams);
        }

        public static bool TEST(ref string msg)
        {
            msg = "OK";

            int value = Get_iREST_HOURS("F128165671", "1100630");
            if (value == 0)
            {
                msg = "失敗Get_iCONVERT_REST_HOURS";
                return false;
            }

            /*刪除測試資料*/
            var cmd = GetCommand_DeleteOTU("F128165671", "1100630");
            if (o_DBFactory.ABC_toTest.TEST_Command(cmd) == false)
            {
                msg = "失敗 GetCommand_Delete";
                return false;
            }

            return true;
        }
    }
}