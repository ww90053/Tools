using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TPPDDB.Logic;

namespace TPPDDB._3_forleave
{
    public class C_COUNT_HDAY
    {
        //private string _k;
        //public string k
        //{
        //    get
        //    {
        //        List<int> r = COUNT_HDAY(_k);
        //        return r[0].ToString();
        //    }
        //    set
        //    {
        //        _k = value;
        //    }
        //}

        private string _r;
        public string r
        {
            get
            {
                //List<int> r = COUNT_HDAY(_r);
                //return r[1].ToString();
                
                return _r;
            }
            set
            {
                _r = value;
            }
        }

        private List<int> _COUNT_HDAY;
        public List<int> COUNT_HDAY    
        {
        get
        {
            List<int> r = COUNT_HDAY1(_r);
            return r;
        }
        set
        {
            _COUNT_HDAY = value;
        }
    }


        private List<int> COUNT_HDAY1(string MZ_ID)
        {
            List<int> CAN_USE = new List<int>();

            //string hdayString = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_HDAY FROM C_DLTBB WHERE MZ_ID='" + MZ_ID.Trim() + "' AND MZ_YEAR='" + (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0') + "'");
            //int hday_canuse = int.Parse(hdayString);

            #region 取得本年度可休假天數

            //從差假基本檔取得
            string Hday_SQL = "SELECT nvl(MZ_HDAY,0) MZ_HDAY,nvl(MZ_HTIME,0) MZ_HTIME FROM C_DLTBB WHERE MZ_ID='" + MZ_ID.Trim() + "' AND MZ_YEAR='" + (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0') + "'";
            DataTable HDAY = o_DBFactory.ABC_toTest.Create_Table(Hday_SQL, "get");
            string hdayString = "0";
            string htimeString = "0";

            hdayString = HDAY.Rows.Count == 0 ? "0" : HDAY.Rows[0]["MZ_HDAY"].ToString();
            htimeString = HDAY.Rows.Count == 0 ? "0" : HDAY.Rows[0]["MZ_HTIME"].ToString();

            int hday_canuse = int.Parse(hdayString);
            int htime_canuse = int.Parse(htimeString);

            #endregion 

            //20140116
            #region 取得本年度已使用 & 已審核之總天數、時數

            int hday = 0;
            int htime = 0;

            string strHDAY_Count = "SELECT nvl(SUM(MZ_TDAY),0  )  MZ_TDAY ,nvl(SUM(MZ_TTIME),0) MZ_TTIME  FROM C_DLTB01 WHERE MZ_CODE='03' AND MZ_CHK1='Y' AND MZ_ID='" + MZ_ID + "' AND dbo.SUBSTR(MZ_IDATE1,1,3)='" + (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0') + "' ";
            DataTable HDAY_Count = o_DBFactory.ABC_toTest.Create_Table(strHDAY_Count, "get");

            if (HDAY_Count.Rows.Count > 0)
            {
                hday = int.Parse(HDAY_Count.Rows[0]["MZ_TDAY"].ToString());

                htime = int.Parse(HDAY_Count.Rows[0]["MZ_TTIME"].ToString());
            }

            #endregion 

            #region 計算天數
            //時數、天數已於此進行轉換
            //TODO 20151014 應可抽出為邏輯層

            //int hday = int.Parse(string.IsNullOrEmpty(o_DBFactory.ABC_toTest.vExecSQL("SELECT SUM(MZ_TDAY) FROM C_DLTB01 WHERE MZ_CODE='03' AND MZ_ID='" + MZ_ID + "' AND dbo.SUBSTR(MZ_IDATE1,1,3)='" + (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0') + "' AND MZ_CHK1='Y' ")) ? "0" : o_DBFactory.ABC_toTest.vExecSQL("SELECT SUM(MZ_TDAY) FROM C_DLTB01 WHERE MZ_CODE='03' AND MZ_ID='" + MZ_ID + "' AND dbo.SUBSTR(MZ_IDATE1,1,3)='" + (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0') + "' AND MZ_CHK1='Y' "));
            //int htime = int.Parse(string.IsNullOrEmpty(o_DBFactory.ABC_toTest.vExecSQL("SELECT SUM(MZ_TTIME) FROM C_DLTB01 WHERE MZ_CODE='03' AND MZ_ID='" + MZ_ID + "' AND dbo.SUBSTR(MZ_IDATE1,1,3)='" + (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0') + "' AND MZ_CHK1='Y' ")) ? "0" : o_DBFactory.ABC_toTest.vExecSQL("SELECT SUM(MZ_TTIME) FROM C_DLTB01 WHERE MZ_CODE='03' AND MZ_ID='" + MZ_ID + "' AND dbo.SUBSTR(MZ_IDATE1,1,3)='" + (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0') + "' AND MZ_CHK1='Y' "));
            //int hday_used;
            //int htime_used;

            //if (htime != 0)
            //{
            //    int hday_Add = htime / 8;
            //    hday_used = hday + hday_Add;
            //    htime_used = htime % 8;
            //}
            //else
            //{
            //    hday_used = hday;
            //    htime_used = 0;
            //}

            //20151014 Neil 改由邏輯層運算，並直接透過 ref 原參數來進行調整，而不另開變數儲存(ex: 104行被註解的部分)
            //TODO 若兩個月後無反映任何問題，請移除上方已註解之舊程式碼及此行
            LogicVacation logic = new LogicVacation();
                          logic.transformDays(ref hday, ref htime);

            #endregion 

            CAN_USE.Insert(0, hday);
            CAN_USE.Insert(1, htime);
            CAN_USE.Insert(2, hday_canuse);
            CAN_USE.Insert(3, htime_canuse);
            return CAN_USE;
        }

        private List<string> _COUNT_OFF_RESULT;

        public List<string> COUNT_OFF_RESULT
        {
            get
            {
                List<string> COUNT_RESULT = COUNT_OFF(_COUNT_OFF_RESULT[0], _COUNT_OFF_RESULT[1], _COUNT_OFF_RESULT[2], _COUNT_OFF_RESULT[3], _COUNT_OFF_RESULT[4], _COUNT_OFF_RESULT[5]);
                return COUNT_RESULT;
            }
            set
            {
                _COUNT_OFF_RESULT = value;
            }
        }

        private List<string> COUNT_OFF(string FDATE, string TYEAR, string TMONTH, string DATE, string RYEAR, string RMONTH)
        {
            List<string> OFF = new List<string>();

            //任公職年資月數


            int tyear = int.Parse(string.IsNullOrEmpty(TYEAR) ? "0" : TYEAR);

            int tmonth = int.Parse(string.IsNullOrEmpty(TMONTH) ? "0" : TMONTH);

            int ryear = int.Parse(string.IsNullOrEmpty(RYEAR) ? "0" : RYEAR);

            int rmonth = int.Parse(string.IsNullOrEmpty(RMONTH) ? "0" : RMONTH);

            int year;

            int month;

            if (!string.IsNullOrEmpty(FDATE))
            {
                System.DateTime dt1 = DateTime.Parse((int.Parse(FDATE.Substring(0, 3)) + 1911).ToString() + "-" + FDATE.Substring(3, 2) + "-" + FDATE.Substring(5, 2));

                System.DateTime dt2 = DateTime.Parse((int.Parse(DATE.Substring(0, 3)) + 1911).ToString() + "-" + DATE.Substring(3, 2) + "-" + DATE.Substring(5, 2));

                int monthDiff = Convert.ToInt32(Microsoft.VisualBasic.DateAndTime.DateDiff("M", dt1, dt2, Microsoft.VisualBasic.FirstDayOfWeek.Sunday, Microsoft.VisualBasic.FirstWeekOfYear.FirstFullWeek)) + tyear * 12 + tmonth - ryear * 12 - rmonth;

                if (monthDiff > 0)
                {
                    year = monthDiff / 12;
                    month = monthDiff % 12;
                }
                else
                {
                    year = 0;
                    month = 0;
                }
            }
            else
            {
                year = 0;
                month = 0;
            }

            OFF.Insert(0, year.ToString());
            OFF.Insert(1, month.ToString());

            return OFF;
        }
    }
}
