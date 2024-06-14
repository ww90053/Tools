using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TPPDDB.Helpers;

namespace TPPDDB._3_forleave
{
    /// <summary>
    /// 邏輯模組-行事曆
    /// </summary>
    public class LogicService_DUTYHOLIDAY
    {
        /// <summary>
        /// 根據日期,判斷當天的行事曆資料
        /// </summary>
        /// <param name="MZ_DATE">民國年月日,YYYMMDD</param>
        /// <returns>行事曆資料</returns>
        public static C_DUTYHOLIDAY_V2 Create_Data(string MZ_DATE)
        {
            //將日期轉換成Date
            DateTime? dt = Helpers.ForDateTime.TWYearMonthDay_ToDateTime(MZ_DATE);
            //先檢查格是
            if (dt == null)
                return null;//日期錯誤,不處理了

            //抓取當天的假日資料
            var data_H = C_DUTYHOLIDAY_Service.Select(MZ_DATE);
            //抓取當天的補班資料
            var data_S = C_SPRINGDAY_Service.Select(MZ_DATE);
            //判斷當天六日
            DayOfWeek dayOfWeek = dt.Value.DayOfWeek;

            C_DUTYHOLIDAY_V2 data = new C_DUTYHOLIDAY_V2();
            data.MZ_DATE = MZ_DATE;

            //如果當天是國定假日,就放假
            if (data_H != null)
            {
                data.MZ_DAYTYPE = "H"; //放假
                data.MZ_DAYTYPE_NAME = "假日";
                data.MZ_HOLIDAY_NAME = data_H.MZ_HOLIDAY_NAME;
                return data;
            }
            //如果當天補班,則上班
            if (data_S != null)
            {
                data.MZ_DAYTYPE = "D";//工作日
                data.MZ_DAYTYPE_NAME = "工作日";
                data.MZ_HOLIDAY_NAME = data_S.MZ_SPRING_NAME;
                return data;
            }
            //如果六日
            if (dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday)
            {
                data.MZ_DAYTYPE = "H"; //放假
                data.MZ_DAYTYPE_NAME = "假日";
                data.MZ_HOLIDAY_NAME = "六日休假";
                return data;
            }
            //都沒有,就是工作日了
            data.MZ_DAYTYPE = "D"; //工作日
            data.MZ_DAYTYPE_NAME = "工作日";
            data.MZ_HOLIDAY_NAME = "平日";
            return data;
        }

        /// <summary>
        /// 抓取並判斷假期設定
        /// </summary>
        /// <param name="MZ_DATE">民國年月:YYYMMDD</param>
        /// <returns>H:假日,D:工作日</returns>
        public static string GetAndSet_DUTYHOLIDAY_Type(string MZ_DATE)
        {
            var data = C_DUTYHOLIDAY_V2_Service.Select(MZ_DATE);
            //沒資料?重新產生再重抓
            if (data == null)
            {
                CreateAndSave_InDB(MZ_DATE);
                data = C_DUTYHOLIDAY_V2_Service.Select(MZ_DATE);
            }
            //還是沒資料?算了...
            if (data == null)
            {
                return "";
            }
            //回傳設定值
            return data.MZ_DAYTYPE;
        }

        /// <summary>
        /// 根據日期,判斷當天的行事曆資料,並且寫到資料庫
        /// </summary>
        /// <param name="MZ_DATE"></param>
        /// <returns></returns>
        public static bool CreateAndSave_InDB(string MZ_DATE)
        {
            //根據日期,判斷當天的行事曆資料
            var data = Create_Data(MZ_DATE);
            //產生失敗就算了
            if (data == null)
            {
                return false;
            }
            //儲存當天資料
            return C_DUTYHOLIDAY_V2_Service.Save(data);
        }

        /// <summary>
        /// 根據起訖日期,批次判斷並更新行事曆資料到資料庫
        /// </summary>
        /// <param name="Beg"></param>
        /// <param name="End"></param>
        /// <returns></returns>
        public static bool CreateAndSave_InDB(DateTime Beg, DateTime End)
        {
            string msg = "";
            bool isOK = true;
            for (DateTime day = Beg; day <= End; day = day.AddDays(1))
            {
                //轉成民國年
                string MZ_DATE = ForDateTime.DateTime_To_TW_YYYMMDD(day);
                isOK = isOK && CreateAndSave_InDB(MZ_DATE);
                if (isOK == false)
                {
                    msg += ";此日期產生錯誤:" + MZ_DATE;
                }

            }
            return isOK;
        }

        /// <summary>
        /// 自動判斷並且補上行事曆資料,以目前日期為基準,往後一年,往前三年的資料都補上
        /// 如果當年已經有365筆資料,就不會異動,不滿365者會自動重算
        /// </summary>
        /// <returns></returns>
        public static bool CreateAndSave_InDB_AllYear()
        {
            try
            {
                //得知明年此時是幾年?
                int Year = DateTime.Now.AddYears(1).Year;
                int cnt_YearBefore = 4;
                //回推4年
                Year = Year - cnt_YearBefore;
                for (int i = 1; i <= cnt_YearBefore; i++)
                {
                    //取得當年,EX: 現在西元 2023 , 則從 2023-4+1 = 2020 開始,也就是 109年
                    int current_Y = Year + i;
                    //計算當年有多少筆資料產生?
                    bool isReady = C_DUTYHOLIDAY_V2_Service.isYearReady((current_Y - 1911).ToString("D3"));
                    //當年資料不齊全?重算一次
                    if (isReady == false)
                    {

                        CreateAndSave_InDB(
                             DateTime.Parse(current_Y + "-01-01"),
                             DateTime.Parse(current_Y + "-12-31")
                             );
                    }
                }
            }
            catch (Exception ex)
            {
                TPPDDB.App_Code.Log.SaveLog(ex);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 測試方法
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool TEST(ref string msg)
        {
            //1000405	清明節
            //1100911	中秋節補班
            //1121118   六日
            //1121011   平日

            //1000405	清明節
            var data = Create_Data("1000405");
            if (data.MZ_DAYTYPE != "H")
            {
                msg = "NG 1000405	清明節";
                return false;
            }
            //1100911	中秋節補班
            data = Create_Data("1100911");
            if (data.MZ_DAYTYPE != "D")
            {
                msg = "NG 1100911	中秋節補班";
                return false;
            }
            //1121118   六日
            data = Create_Data("1121118");
            if (data.MZ_DAYTYPE != "H")
            {
                msg = "NG 1121118   六日";
                return false;
            }
            //1121011   平日
            data = Create_Data("1121011");
            if (data.MZ_DAYTYPE != "D")
            {
                msg = "NG 1121011   平日";
                return false;
            }

            //測試通過
            msg = "OK";
            return true;
        }
    }
}