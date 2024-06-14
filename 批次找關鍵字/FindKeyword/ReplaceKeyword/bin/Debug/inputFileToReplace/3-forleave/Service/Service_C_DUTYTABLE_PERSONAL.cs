using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TPPDDB.Helpers;

namespace TPPDDB._3_forleave
{
    public static class Service_C_DUTYTABLE_PERSONAL
    {
        /// <summary>
        /// 根據ID,上班日,找出當天的超勤補休時數,參考資料表:C_DUTYTABLE_PERSONAL
        /// </summary>
        /// <param name="MZ_ID"></param>
        /// <param name="DutyDate"></param>
        /// <returns></returns>
        public static int Get_iCONVERT_REST_HOURS(string MZ_ID, string DutyDate)
        {
            //SQL: 找出CONVERT_REST_HOURS
            string SQL_CONVERT_REST_HOURS = @"
            SELECT CONVERT_REST_HOURS FROM C_DUTYTABLE_PERSONAL WHERE MZ_ID =@MZ_ID AND DUTYDATE =@DUTYDATE
            ";
            List<SqlParameter> SqlPrams = new List<SqlParameter>() {
                new SqlParameter(":MZ_ID",MZ_ID),
                new SqlParameter(":DUTYDATE",DutyDate)
            };
            //超勤補休時數
            int iCONVERT_REST_HOURS = o_DBFactory.ABC_toTest.Get_First_Field(SQL_CONVERT_REST_HOURS, SqlPrams).SafeToInt();
            return iCONVERT_REST_HOURS;
        }

        /// <summary>
        /// 取得刪除資料的指令,用於後續交易一併執行
        /// </summary>
        /// <param name="MZ_ID"></param>
        /// <param name="DutyDate"></param>
        /// <returns></returns>
        public static SqlCommand GetCommand_Delete(string MZ_ID, string DutyDate)
        {
            string DeleteString = "DELETE " +
                                  "FROM " +
                                            "C_DUTYTABLE_PERSONAL " +
                                  @" WHERE MZ_ID=@MZ_ID AND  DUTYDATE=@DUTYDATE ";
            List<SqlParameter> SqlPrams = new List<SqlParameter>() {
                new SqlParameter(":MZ_ID",MZ_ID),
                new SqlParameter(":DUTYDATE",DutyDate)
            };

            return o_DBFactory.ABC_toTest.Get_Command(DeleteString, SqlPrams);
        }


        /// <summary>基礎函數 : 取得某一天之超勤紀錄資料列</summary>
        /// <param name="MZ_ID">身分證字號</param>
        /// <param name="DATE">超勤日期</param>
        /// 20150506 Neil
        public static DataRow getOvertimeDatarow(String MZ_ID, String DATE)
        {
            String strSQL = String.Format(@"SELECT * FROM C_DUTYTABLE_PERSONAL WHERE MZ_ID='{0}' AND DUTYDATE='{1}'",
                                          MZ_ID,
                                          DATE);

            DataTable dt = new DataTable();
            dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GETVALUE");

            if (dt.Rows.Count > 0)
            {
                DataRow r = dt.Rows[0]; //正常僅會有一筆資料
                return r;
            }
            else
                return null;
        }

        /// <summary>取得某特定日期之已轉換補休時數</summary>
        /// 20150506 Neil
        public static Int32 GetSingleDayOvertimeRest(String MZ_ID, String DATE)
        {
            Int32 result = 0;
            DataRow r = Service_C_DUTYTABLE_PERSONAL.getOvertimeDatarow(MZ_ID, DATE);

            if (r != null)
            {
                if (r["CONVERT_REST_HOURS"] != DBNull.Value)
                    result = Convert.ToInt32(r["CONVERT_REST_HOURS"]);
                else
                    result = 0;
            }
            return result;
        }

        /// <summary>取得某特定日期之超勤紀錄全部</summary>
        /// <returns>出勤紀錄</returns>
        /// 20150506 Neil
        public static List<String> GetSingleDayOverTimeList(String MZ_ID, String DATE)
        {
            List<String> result = new List<String>();
            DataRow r = Service_C_DUTYTABLE_PERSONAL.getOvertimeDatarow(MZ_ID, DATE);

            if (r != null)
            {
                // Joy 20150513 超勤時數增加08-09欄位，但時數統計表未合併計算，所以將i值範圍由原先27改為28
                for (int i = 1; i < 28; i++)
                {
                    String s = r["DUTYITEM" + i.ToString()].ToString();
                    result.Add(s);
                }
            }
            return result;
        }

        /// <summary>
        /// 特殊: 
        /// (1) 針對 非GI類工作項目,取得數量 例如:  "5" 
        /// (2) G或I類工作項目,取得時間字串,格式如 :  "0600-0700.0800-0900.0900-1000"
        /// 為何會那麼畸形?我也不知道...
        /// </summary>
        /// <param name="ID">員警證號</param>
        /// <param name="MODE">工作項目代碼</param>
        /// <param name="DATE">日期</param>
        /// <returns>
        /// 如果 MODE為 G 或 I,格式如 :  "0600-0700.0800-0900.0900-1000"
        /// 否則 會傳的只有數字,代表筆數,例如:  "5"
        /// </returns>
        public static string Count(string ID, string MODE, string DATE)
        {
            string result = "";

            string strSQL = "SELECT * FROM  C_DUTYTABLE_PERSONAL WHERE MZ_ID='" + ID + "' AND DUTYDATE='" + DATE + "'";


            DataTable tempDT = new DataTable();

            tempDT = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GETVALUE");

            int j = 0;//非 G I 類型的項目有多少筆
            int K = 0;//G I 類型的項目有多少筆,其實只是用來算誰是第一筆而已,避免拼接分隔符號錯誤
            string timestring = "";
            // Joy 20150513 超勤時數增加08-09欄位，但時數統計表未合併計算，所以將i值範圍由原先27改為28
            for (int i = 1; i < 28; i++)
            {
                if (MODE == "G" || MODE == "I")//G督勤 I專案情務
                {
                    if (tempDT.Rows[0]["DUTYITEM" + i.ToString()].ToString() == MODE)
                    {
                        K++;

                        if (K == 1)
                            timestring += tempDT.Rows[0]["TIME" + i.ToString()].ToString();
                        else
                            timestring += "." + tempDT.Rows[0]["TIME" + i.ToString()].ToString();
                    }
                }
                else
                {
                    if (tempDT.Rows[0]["DUTYITEM" + i.ToString()].ToString() == MODE)
                        j++;

                }
            }

            if (MODE == "G" || MODE == "I")
            {
                result = timestring;
            }
            else
            {
                result = j.ToString();

            }

            return result;
        }


        public static bool TEST(ref string msg)
        {
            msg = "OK";

            int value = Get_iCONVERT_REST_HOURS("F128165671", "1110901");
            if (value == 0)
            {
                msg = "失敗Get_iCONVERT_REST_HOURS";
                return false;
            }

            /*刪除測試資料*/
            var cmd = GetCommand_Delete("F128165671", "1110901");
            if (o_DBFactory.ABC_toTest.TEST_Command(cmd) == false)
            {
                msg = "失敗 GetCommand_Delete";
                return false;
            }

            return true;
        }
    }

}