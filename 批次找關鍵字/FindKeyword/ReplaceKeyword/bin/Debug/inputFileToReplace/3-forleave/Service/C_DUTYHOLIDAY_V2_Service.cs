using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TPPDDB.Helpers;
namespace TPPDDB._3_forleave
{
    public class C_DUTYHOLIDAY_V2_Service
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool Insert(C_DUTYHOLIDAY_V2 data)
        {
            string SQL = @"
Insert INTO C_DUTYHOLIDAY_V2
(MZ_DATE, MZ_HOLIDAY_NAME, MZ_DAYTYPE, MZ_DAYTYPE_NAME)
VALUES
(@MZ_DATE, @MZ_HOLIDAY_NAME, @MZ_DAYTYPE, @MZ_DAYTYPE_NAME)

";
            List<SqlParameter> OPrams = new List<SqlParameter>();
            OPrams.Add(new SqlParameter(":MZ_DATE", data.MZ_DATE));
            OPrams.Add(new SqlParameter(":MZ_HOLIDAY_NAME", data.MZ_HOLIDAY_NAME));
            OPrams.Add(new SqlParameter(":MZ_DAYTYPE", data.MZ_DAYTYPE));
            OPrams.Add(new SqlParameter(":MZ_DAYTYPE_NAME", data.MZ_DAYTYPE_NAME));

            return o_DBFactory.ABC_toTest.DealCommandLog(SQL, OPrams);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool Update(C_DUTYHOLIDAY_V2 data)
        {
            string SQL = @"
UPDATE C_DUTYHOLIDAY_V2
SET MZ_HOLIDAY_NAME=@MZ_HOLIDAY_NAME
, MZ_DAYTYPE=@MZ_DAYTYPE
, MZ_DAYTYPE_NAME=@MZ_DAYTYPE_NAME
WHERE MZ_DATE=@MZ_DATE
";
            List<SqlParameter> OPrams = new List<SqlParameter>();
            OPrams.Add(new SqlParameter(":MZ_DATE", data.MZ_DATE));
            OPrams.Add(new SqlParameter(":MZ_HOLIDAY_NAME", data.MZ_HOLIDAY_NAME));
            OPrams.Add(new SqlParameter(":MZ_DAYTYPE", data.MZ_DAYTYPE));
            OPrams.Add(new SqlParameter(":MZ_DAYTYPE_NAME", data.MZ_DAYTYPE_NAME));

            return o_DBFactory.ABC_toTest.DealCommandLog(SQL, OPrams);

        }
        /// <summary>
        /// 儲存,自動判斷新增修改
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool Save(C_DUTYHOLIDAY_V2 data)
        {
            var data_Old = Select(data.MZ_DATE);
            if (data_Old == null)
            {
                return Insert(data);
            }
            //特殊:如果兩邊都長一樣,就不要動作了

            bool isSame = DataHelpers.CompareObjects<C_DUTYHOLIDAY_V2>(data, data_Old);
            if (isSame)
            {
                return true;
            }

            return Update(data);
        }
        /// <summary>
        /// 查詢 回傳 DataTable
        /// </summary>
        /// <param name="MZ_DATE"></param>
        /// <returns></returns>
        public static DataTable Select_DataSet(string MZ_DATE)
        {
            string SQL = @"
SELECT *
FROM C_DUTYHOLIDAY_V2
WHERE MZ_DATE=@MZ_DATE
";
            List<SqlParameter> OPrams = new List<SqlParameter>();
            OPrams.Add(new SqlParameter(":MZ_DATE", MZ_DATE));
            var dt = o_DBFactory.ABC_toTest.Create_Table(SQL, OPrams);
            return dt;
        }

        /// <summary>
        /// 查詢 回傳資料物件
        /// </summary>
        /// <param name="MZ_DATE"></param>
        /// <returns></returns>
        public static C_DUTYHOLIDAY_V2 Select(string MZ_DATE)
        {
            var dt = Select_DataSet(MZ_DATE);
            if (dt.Rows.Count == 0)
                return null;
            return new C_DUTYHOLIDAY_V2(dt.Rows[0]);
        }

        /// <summary>
        /// 根據輸入的民國年,判斷當年有多少筆資料,用於確認是否資料已經備齊?
        /// </summary>
        /// <param name="TWYear"></param>
        /// <returns></returns>
        public static int Count_Year(string TWYear)
        {
            string SQL = @"
--判斷當年度的資料是否已經備齊?
select count(*) as cnt
from C_DUTYHOLIDAY_V2
WHERE MZ_DATE like @TWYear+'%'
";

            List<SqlParameter> OPrams = new List<SqlParameter>();
            OPrams.Add(new SqlParameter(":TWYear", TWYear));
            string cnt = o_DBFactory.ABC_toTest.Get_First_Field(SQL, OPrams);
            return cnt.SafeToInt();
        }

        /// <summary>
        /// 判斷當年資料是否已經備齊?
        /// </summary>
        /// <param name="TWYear"></param>
        /// <returns></returns>
        public static bool isYearReady(string TWYear)
        {
            int cnt = Count_Year(TWYear);
            return (cnt >= 365);//應該要大於等於365天
        }

        //刪除
        public static bool Delete(string MZ_DATE)
        {
            string SQL = @"
            Delete C_DUTYHOLIDAY_V2
            WHERE MZ_DATE=@MZ_DATE
            ";

            List<SqlParameter> OPrams = new List<SqlParameter>();
            OPrams.Add(new SqlParameter(":MZ_DATE", MZ_DATE));

            return o_DBFactory.ABC_toTest.DealCommandLog(SQL, OPrams);
        }

        /// <summary>
        /// 測試方法
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool TEST(ref string msg)
        {

            C_DUTYHOLIDAY_V2 data = new C_DUTYHOLIDAY_V2();
            data.MZ_DATE = "0770411";
            data.MZ_HOLIDAY_NAME = "今天我生日";
            data.MZ_DAYTYPE = "H";
            data.MZ_DAYTYPE_NAME = "假日";


            //先砍掉目標
            Delete(data.MZ_DATE);
            //新增
            bool isOK = Insert(data);
            if (isOK == false)
            {
                msg = "Insert NG";
                return false;
            }
            //重新抓
            var data2 = Select(data.MZ_DATE);
            if (data2 == null)
            {

                msg = "Get NG";
                return false;
            }
            //改文字
            data2.MZ_HOLIDAY_NAME = "今天我生日喔";
            isOK = Update(data2);
            if (isOK == false)
            {
                msg = "Update NG";
                return false;
            }
            //重新抓
            data2 = Select(data.MZ_DATE);
            if (data2 == null)
            {

                msg = "Get NG";
                return false;
            }
            if (data2.MZ_HOLIDAY_NAME != "今天我生日喔")
            {

                msg = "Update NG Col";
                return false;
            }
            //砍掉目標
            isOK = Delete(data.MZ_DATE);
            if (isOK == false)
            {
                msg = "Delete NG Error";
                return false;
            }
            //重新抓
            data2 = Select(data.MZ_DATE);
            if (data2 != null)
            {

                msg = "Delete NG Not Delete";
                return false;
            }

            //儲存測試
            isOK = Save(data);
            if (isOK == false)
            {
                msg = "Save NG Error";
                return false;
            }
            //重新抓
            data2 = Select(data.MZ_DATE);
            if (data2 == null)
            {

                msg = "Save NG Not Save";
                return false;
            }

            //砍掉目標
            isOK = Delete(data.MZ_DATE);
            if (isOK == false)
            {
                msg = "Delete NG Error";
                return false;
            }
            //重新抓
            data2 = Select(data.MZ_DATE);
            if (data2 != null)
            {

                msg = "Delete NG Not Delete";
                return false;
            }

            //測試通過
            msg = "OK";
            return true;
        }
    }

    public class C_DUTYHOLIDAY_V2
    {
        public C_DUTYHOLIDAY_V2() { }

        public C_DUTYHOLIDAY_V2(DataRow dr)
        {
            this.MZ_DATE = dr["MZ_DATE"].ToStringNullSafe();
            this.MZ_HOLIDAY_NAME = dr["MZ_HOLIDAY_NAME"].ToStringNullSafe();
            this.MZ_DAYTYPE = dr["MZ_DAYTYPE"].ToStringNullSafe();
            this.MZ_DAYTYPE_NAME = dr["MZ_DAYTYPE_NAME"].ToStringNullSafe();
        }

        /// <summary>
        /// 民國年月日,YYYMMDD
        /// </summary>
        public string MZ_DATE { get; set; } = "";
        /// <summary>
        /// 假日名稱
        /// </summary>
        public string MZ_HOLIDAY_NAME { get; set; } = "";
        /// <summary>
        /// H:假日 D:平日
        /// </summary>
        public string MZ_DAYTYPE { get; set; } = "";
        /// <summary>
        /// 假日/平日
        /// </summary>
        public string MZ_DAYTYPE_NAME { get; set; } = "";
    }
}