using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TPPDDB.Helpers;
namespace TPPDDB._3_forleave
{
    public class C_SPRINGDAY_Service
    {
        /// <summary>
        /// 查詢 回傳 DataTable
        /// </summary>
        /// <param name="MZ_DATE"></param>
        /// <returns></returns>
        public static DataTable Select_DataSet(string MZ_SPRING_DATE)
        {
            string SQL = @"
SELECT *
FROM C_SPRINGDAY
WHERE MZ_SPRING_DATE=@MZ_SPRING_DATE
";
            List<SqlParameter> OPrams = new List<SqlParameter>();
            OPrams.Add(new SqlParameter(":MZ_SPRING_DATE", MZ_SPRING_DATE));
            var dt = o_DBFactory.ABC_toTest.Create_Table(SQL, OPrams);
            return dt;
        }

        /// <summary>
        /// 查詢 回傳資料物件
        /// </summary>
        /// <param name="MZ_DATE"></param>
        /// <returns></returns>
        public static C_SPRINGDAY Select(string MZ_SPRING_DATE)
        {
            var dt = Select_DataSet(MZ_SPRING_DATE);
            if (dt.Rows.Count == 0)
                return null;
            return new C_SPRINGDAY(dt.Rows[0]);
        }


        /// <summary>
        /// 測試方法
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool TEST(ref string msg)
        {

            var data = Select("0990922");
            if (data == null)
            {
                msg = "NG";
                return false;
            }

            //測試通過
            msg = "OK";
            return true;
        }
    }
    /// <summary>
    /// 補班日設定資料
    /// </summary>
    public class C_SPRINGDAY
    {
        public C_SPRINGDAY() { }

        public C_SPRINGDAY(DataRow dr)
        {
            this.MZ_SPRING_DATE = dr["MZ_SPRING_DATE"].ToStringNullSafe();
            this.MZ_SPRING_NAME = dr["MZ_SPRING_NAME"].ToStringNullSafe();
        }
        /// <summary>
        /// 民國年月日,YYYMMDD
        /// </summary>
        public string MZ_SPRING_DATE { get; set; } = "";
        /// <summary>
        /// 假日名稱
        /// </summary>
        public string MZ_SPRING_NAME { get; set; } = "";
    }
}