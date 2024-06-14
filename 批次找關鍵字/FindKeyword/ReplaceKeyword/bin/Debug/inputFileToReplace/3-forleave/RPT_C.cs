using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using TPPDDB.App_Code;
using TPPDDB.Service;
using TPPDDB.Logic;
using TPPDDB.Helpers;
using TPPDDB.Models._3_ForLeave;

namespace TPPDDB._3_forleave
{
    public class RPT_C
    {
        /// <summary>
        /// 4.3.1印請假單
        /// </summary>
        public class C_offduty
        {

            public static DataTable doSearch(string MZ_ID, string MZ_NAME, string MZ_IDATE1, string MZ_ITIME1)
            {
                string strSQL = @"SELECT MZ_DLTB01_SN,MZ_ID,MZ_NAME, 
                            MZ_EXAD_CH MZ_EXAD,MZ_EXUNIT_CH  MZ_EXUNIT , MZ_OCCC_CH   MZ_OCCC , MZ_CODE_CH  MZ_CODE,   MZ_RANK1_CH   MZ_RANK1,
                            MZ_SYSDAY,MZ_IDATE1,MZ_ITIME1,MZ_ODATE,MZ_OTIME,MZ_TDAY,MZ_TTIME,MZ_CAUSE,MZ_MEMO,MZ_SWT,MZ_TADD,MZ_RNAME, 
                           MZ_ROCCC_CH  ROCCC,
                             MZ_CHK1,MZ_SYSTIME,MZ_FOREIGN,MZ_CHINA,MZ_CODE AS MZ_CODE1  
                             FROM VW_C_DLTB01  ";

                List<string> condition = new List<string>();



                if (MZ_ID != string.Empty)
                {
                    condition.Add("MZ_ID='" + MZ_ID + "'");
                }


                if (MZ_NAME != string.Empty)
                {
                    condition.Add("MZ_NAME='" + o_str.tosql(MZ_NAME) + "'");
                }

                if (MZ_IDATE1 != string.Empty)
                {
                    condition.Add("MZ_IDATE1='" + MZ_IDATE1.Replace("/", "").PadLeft(7, '0') + "'");
                }

                if (MZ_ITIME1 != string.Empty)
                {
                    condition.Add("MZ_ITIME1='" + MZ_ITIME1 + "'");
                }


                //以前是報表頁才有  現在通用看有沒有問題
                condition.Add("(MZ_TDAY>0 OR MZ_TTIME>0)");
                //
                string where = (condition.Count > 0 ? " WHERE " + string.Join(" AND ", condition.ToArray()) : string.Empty);

                strSQL += where;

                return o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");
            }



            public static string isHome(DataTable offduty)
            {
                string MZ_ID = offduty.Rows[0]["MZ_ID"].ToString();
                string is_home = "";

                if (o_A_DLBASE.CAD(MZ_ID) == "新北市政府警察局")
                    is_home = "Y";
                else
                    is_home = "N";


                return is_home;
            }

            public static string TITLE(DataTable offduty)
            {
                string MZ_ID = offduty.Rows[0]["MZ_ID"].ToString();


                return string.Format("{0}{1}　員工請假申請單", o_A_DLBASE.CAD(MZ_ID), o_A_DLBASE.CUNIT(MZ_ID.Trim()));

            }

            public static string PS(DataTable offduty)
            {
                string MZ_ID = offduty.Rows[0]["MZ_ID"].ToString();

                int CODE_DAY = 0;

                int CODE_TIME = 0;

                string sql = " SELECT NVL(SUM(MZ_TDAY),0) MZ_TDAY  ,NVL(SUM(MZ_TTIME),0) MZ_TTIME FROM C_DLTB01 WHERE MZ_CODE='" + offduty.Rows[0]["MZ_CODE1"].ToString() + "' AND MZ_ID='" + MZ_ID + "' AND dbo.SUBSTR(MZ_IDATE1,1,3)='" + (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0') + "' AND MZ_CHK1='Y'";
                DataTable temp = o_DBFactory.ABC_toTest.Create_Table(sql, "get");
                if (temp.Rows.Count > 0)
                {
                    CODE_DAY = int.Parse(temp.Rows[0]["MZ_TDAY"].ToString());
                    CODE_TIME = int.Parse(temp.Rows[0]["MZ_TTIME"].ToString());
                }
                int CODE_USE_DAY = (CODE_DAY * 8 + CODE_TIME) / 8;

                int CODE_USE_TIME = (CODE_DAY * 8 + CODE_TIME) % 8;


                return string.Format("附註：已請({0})  {1} 日  {2}時", offduty.Rows[0]["MZ_CODE"].ToString(), CODE_USE_DAY, CODE_USE_TIME);

            }

            public static string DOWN(DataTable offduty, string date)
            {
                string MZ_ID = offduty.Rows[0]["MZ_ID"].ToString();


                int have_day = 0;
                int have_time = 0;

                string sql = string.Format("SELECT MZ_HDAY,MZ_HTIME FROM C_DLTBB WHERE MZ_ID='{0}' AND MZ_YEAR='{1}'", MZ_ID, date.Replace("/", string.Empty).PadLeft(7, '0').Substring(0, 3));

                DataTable temp = o_DBFactory.ABC_toTest.Create_Table(sql, "GET");

                if (temp.Rows.Count > 0)
                {
                    have_day = int.Parse(temp.Rows[0]["MZ_HDAY"].ToString());
                    have_time = int.Parse(temp.Rows[0]["MZ_HTIME"].ToString());
                }
                int hday = 0;
                int htime = 0;

                sql = "SELECT NVL(SUM(MZ_TDAY),0 ) MZ_TDAY, NVL(SUM(MZ_TTIME),0 ) MZ_TTIME  FROM C_DLTB01 WHERE MZ_CODE='03' AND MZ_ID='" + MZ_ID + "' AND dbo.SUBSTR(MZ_IDATE1,1,3)='" + date.Replace("/", string.Empty).PadLeft(7, '0').Substring(0, 3) + "' AND MZ_CHK1='Y'";
                temp = o_DBFactory.ABC_toTest.Create_Table(sql, "GET");
                if (temp.Rows.Count > 0)
                {

                    hday = int.Parse(temp.Rows[0]["MZ_TDAY"].ToString());
                    htime = int.Parse(temp.Rows[0]["MZ_TTIME"].ToString());
                }
                int hday_used;

                int htime_used;

                if (htime != 0)
                {
                    int hday_Add = htime / 8;
                    hday_used = hday + hday_Add;
                    htime_used = htime % 8;
                }
                else
                {
                    hday_used = hday;
                    htime_used = 0;
                }

                return string.Format("應休假日數： {0} 日  {1} 時       已休假日數：  {2} 日  {3} 時", have_day, have_time, hday_used, htime_used);


            }
        }

        /// <summary>
        /// 4.3.2印出差單
        /// </summary>
        public class C_businessTrip
        {
            public static DataTable doSearch(string MZ_ID, string MZ_NAME, string MZ_IDATE1)
            {

                string strSQL = @"SELECT MZ_ID,MZ_NAME,MZ_DLTB01_SN, MZ_EXAD_CH MZ_EXAD,MZ_EXUNIT_CH  MZ_EXUNIT , MZ_OCCC_CH   MZ_OCCC , MZ_CODE_CH  MZ_CODE,   MZ_RANK1_CH   MZ_RANK1,
                                       MZ_SYSDAY,MZ_IDATE1,MZ_ITIME1,MZ_ODATE,MZ_OTIME,MZ_TDAY,MZ_TTIME,MZ_CAUSE,MZ_MEMO,MZ_SWT,MZ_TADD,MZ_RNAME,
                                       MZ_ROCCC_CH  ROCCC,
                                       MZ_CHK1,MZ_SYSTIME,MZ_FOREIGN,MZ_CHINA  
                                       FROM VW_C_DLTB01 
                        
                                       WHERE (MZ_CODE='07' OR MZ_CODE='06')  AND (MZ_TDAY>0 OR MZ_TTIME>0) ";



                List<string> condition = new List<string>();

                if (!string.IsNullOrEmpty(MZ_IDATE1))
                {
                    condition.Add("MZ_IDATE1 = '" + MZ_IDATE1.Replace("/", string.Empty).PadLeft(7, '0') + "'");
                }

                if (!string.IsNullOrEmpty(MZ_ID))
                {
                    condition.Add("MZ_ID='" + MZ_ID + "'");
                }

                if (!string.IsNullOrEmpty(MZ_NAME))
                {
                    //曾因名字有罕見字.用SQL句收詢有資料.但用o_DBFactory.ABC_toTest.Create_Table卻跑不出來.應該是編碼問題.目前沒有處理
                    condition.Add("MZ_NAME='" + MZ_NAME + "'");
                }

                string where = (condition.Count > 0 ? " AND " + string.Join(" AND ", condition.ToArray()) : string.Empty);

                strSQL += where;


                return o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");

            }
        }

        /// <summary>
        /// 4.3.3勤惰卡
        /// </summary>
        public class C_shift
        {
            public static DataTable doSearch_shift(string YEAR, string YEAR2, string MZ_ID)
            {
                List<string> condition = new List<string>();
                if (!string.IsNullOrEmpty(YEAR) && !string.IsNullOrEmpty(YEAR2))
                {
                    condition.Add("MZ_IDATE1>='" + YEAR.Trim().PadLeft(7, '0') + "' AND MZ_IDATE1<='" + YEAR2.Trim().PadLeft(7, '0') + "'");
                }

                if (MZ_ID != string.Empty)
                {
                    condition.Add("CD.MZ_ID='" + o_str.tosql(MZ_ID.Trim()) + "'");
                }

                string where = (condition.Count > 0 ? " AND " + string.Join(" AND ", condition.ToArray()) : string.Empty);

                string strSQL = string.Format(@"SELECT MZ_HTIME,
(SELECT SUM(MZ_TDAY) FROM C_DLTB01 WHERE MZ_CODE='03' AND MZ_ID=CD.MZ_ID AND MZ_IDATE1>='{0}' AND MZ_IDATE1<='{1}') DAY_USED,
(SELECT SUM(MZ_TTIME) FROM C_DLTB01 WHERE MZ_CODE='03' AND MZ_ID=CD.MZ_ID AND MZ_IDATE1>='{0}' AND MZ_IDATE1<='{1}') HOUR_USED,
MZ_SDAY,MZ_SDAY_HOUR,MZ_SDAY2,MZ_SDAY2_HOUR,MZ_HDAY,CD.MZ_ID,CD.MZ_NAME,
MZ_EXAD_CH MZ_EXAD,
MZ_EXUNIT_CH MZ_EXUNIT,
MZ_OCCC_CH MZ_OCCC ,
MZ_CODE_CH MZ_CODE,
MZ_IDATE1,MZ_ITIME1, CD.MZ_ODATE,MZ_OTIME,MZ_TDAY,MZ_TTIME,MZ_CAUSE, A_DLBASE.MZ_MEMO 
,MZ_FDATE,MZ_POLNO,MZ_TYEAR,MZ_TMONTH,MZ_RYEAR,MZ_RMONTH,MZ_OFFYY,MZ_OFFMM,' ' as DOWN 
FROM VW_C_DLTB01 CD,C_DLTBB CBB 
LEFT JOIN A_DLBASE ON CBB.MZ_ID = A_DLBASE.MZ_ID
WHERE (CD.MZ_TDAY>0 OR CD.MZ_TTIME>0) AND  CD.MZ_CHK1='Y' AND CD.MZ_ID = CBB.MZ_ID AND CBB.MZ_YEAR='{2}'   {3}
ORDER BY MZ_IDATE1 DESC", YEAR, YEAR2, YEAR2.Substring(0, 3), where);

                DataTable shift = new DataTable();
                shift = o_DBFactory.ABC_toTest.Create_Table(strSQL, "DETAIL");


                strSQL = "SELECT MZ_CODE,MZ_CNAME FROM C_DLCODE ORDER BY MZ_CODE";
                DataTable temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
                DataTable dt = new DataTable();




                foreach (DataRow dr1 in shift.Rows)
                {
                    List<string> S = new List<string>();

                    S.Insert(0, dr1["MZ_FDATE"].ToString());

                    S.Insert(1, dr1["MZ_TYEAR"].ToString());
                    S.Insert(2, dr1["MZ_TMONTH"].ToString());
                    S.Insert(3, YEAR2.PadLeft(7, '0'));
                    S.Insert(4, dr1["MZ_RYEAR"].ToString());
                    S.Insert(5, dr1["MZ_RMONTH"].ToString());

                    C_COUNT_HDAY COUNT = new C_COUNT_HDAY()
                    {
                        COUNT_OFF_RESULT = S,
                    };

                    dr1["MZ_OFFYY"] = COUNT.COUNT_OFF_RESULT[0];
                    dr1["MZ_OFFMM"] = COUNT.COUNT_OFF_RESULT[1];

                    int b_day = 0;
                    int b_hour = 0;
                    int b_total = 0;
                    int s_day = 0;
                    int s_hour = 0;
                    string result = string.Empty;
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {

                        strSQL = string.Format("SELECT NVL(SUM(MZ_TDAY),0) DAY,NVL(SUM(MZ_TTIME),0) TIME FROM C_DLTB01 WHERE  MZ_CHK1='Y' AND  MZ_IDATE1>='" + YEAR.Trim().PadLeft(7, '0') + "' AND MZ_IDATE1<='" + YEAR2.Trim().PadLeft(7, '0') + "' AND MZ_ID='{0}' AND MZ_CODE='{1}'", MZ_ID, temp.Rows[i]["MZ_CODE"].ToString());

                        dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");

                        if (dt.Rows.Count > 0)
                        {
                            b_day = int.Parse(dt.Rows[0]["DAY"].ToString());
                            b_hour = int.Parse(dt.Rows[0]["TIME"].ToString());
                            b_total = b_day * 8 + b_hour;
                            s_day = b_total / 8;
                            s_hour = b_total % 8;
                        }
                        /*
                         下面註腳的格式: 
                         病假 XX日X時  特休假 00日0時  ＯＯＯ假別 00日0時  陪產檢及陪產假 00日0時  Ｏ假別 00日0時
                         (每行顯示5組)                         
                         */
                        if ((i + 1) % 5 == 0)
                        {
                            result += temp.Rows[i]["MZ_CNAME"].ToString().Trim() + " " + s_day.ToString().PadLeft(2, ' ') + "日" + s_hour.ToString() + "時  &N";
                        }
                        else
                        {
                            result += temp.Rows[i]["MZ_CNAME"].ToString().Trim() + " " + s_day.ToString().PadLeft(2, ' ') + "日" + s_hour.ToString() + "時  ";

                        }
                    }
                    dr1["DOWN"] = result;

                    List<string> ss = DLTB_DAYS_COUNT(dr1["MZ_ID"].ToString());

                    dr1["MZ_OFFYY"] = ss[0];
                    dr1["MZ_OFFMM"] = ss[1];
                }


                return shift;


            }

            public static DataTable doSearch_shift1(string YEAR, string MZ_ID)
            {

                string strSQL = @" SELECT A.MZ_EXAD_CH  MZ_EXAD,A.MZ_EXUNIT_CH  MZ_EXUNIT , A.MZ_OCCC_CH  MZ_OCCC , A.MZ_NAME , A.MZ_ID , A.MZ_POLNO ,A.MZ_FDATE,A.MZ_TYEAR,A.MZ_TMONTH,A.MZ_RYEAR,A.MZ_RMONTH,MZ_OFFYY,MZ_OFFMM,
B.*,'' AS MZ_IDATE1,'' AS MZ_CAUSE,'' AS MZ_CODE,'' AS DOWN,0 AS HOUR_USED,0 AS DAY_USED 
FROM VW_A_DLBASE A,C_DLTBB B 
WHERE A.MZ_ID=B.MZ_ID AND B.MZ_ID='" + MZ_ID.ToUpper() + "' AND MZ_YEAR='" + YEAR.Substring(0, 3) + "'";
                DataTable shift = o_DBFactory.ABC_toTest.Create_Table(strSQL, "DETAIL");

                foreach (DataRow dr1 in shift.Rows)
                {
                    List<string> ss = DLTB_DAYS_COUNT(dr1["MZ_ID"].ToString());


                    dr1["MZ_OFFYY"] = ss[0];
                    dr1["MZ_OFFMM"] = ss[1];
                }

                return shift;


            }

            protected static List<string> DLTB_DAYS_COUNT(string MZ_ID)
            {
                List<string> result = new List<string>();

                string selectSQL = "SELECT MZ_FDATE,MZ_TYEAR,MZ_TMONTH,MZ_RYEAR,MZ_RMONTH,MZ_MEMO FROM A_DLBASE WHERE MZ_ID='" + MZ_ID + "'";

                DataTable temp = new DataTable();

                temp = o_DBFactory.ABC_toTest.Create_Table(selectSQL, "GET");

                int tyear = int.Parse(string.IsNullOrEmpty(temp.Rows[0]["MZ_TYEAR"].ToString()) ? "0" : temp.Rows[0]["MZ_TYEAR"].ToString());

                int tmonth = int.Parse(string.IsNullOrEmpty(temp.Rows[0]["MZ_TMONTH"].ToString()) ? "0" : temp.Rows[0]["MZ_TMONTH"].ToString());

                int ryear = int.Parse(string.IsNullOrEmpty(temp.Rows[0]["MZ_RYEAR"].ToString()) ? "0" : temp.Rows[0]["MZ_RYEAR"].ToString());

                int rmonth = int.Parse(string.IsNullOrEmpty(temp.Rows[0]["MZ_RMONTH"].ToString()) ? "0" : temp.Rows[0]["MZ_RMONTH"].ToString());


                string FDATE = temp.Rows[0]["MZ_FDATE"].ToString();

                if (string.IsNullOrEmpty(FDATE))
                {
                    FDATE = (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0') + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0');
                }

                //任公職年資月數
                System.DateTime dt1 = DateTime.Parse((int.Parse(FDATE.Substring(0, 3)) + 1911).ToString() + "-" + FDATE.Substring(3, 2) + "-" + FDATE.Substring(5, 2));

                System.DateTime dt2 = DateTime.Now;

                int monthDiff = Convert.ToInt32(Microsoft.VisualBasic.DateAndTime.DateDiff("M", dt1, dt2, Microsoft.VisualBasic.FirstDayOfWeek.Sunday, Microsoft.VisualBasic.FirstWeekOfYear.FirstFullWeek)) + tyear * 12 + tmonth - ryear * 12 - rmonth;

                if (monthDiff <= 0)
                {
                    result.Insert(0, "0");
                    result.Insert(1, "0");
                }
                else
                {
                    result.Insert(0, (monthDiff / 12).ToString());
                    result.Insert(1, (monthDiff % 12).ToString());
                }

                return result;
            }


        }

        /// <summary>
        /// 4.3.9員警赴大陸地區請假單
        /// </summary>
        public class C_chinaabroad
        {
            /// <summary>取得假單內容</summary>
            /// <param name="MZ_ID">身分證字號</param>
            /// <param name="MZ_NAME">姓名</param>
            /// <param name="MZ_DATE">出國日期</param>
            /// <param name="LastTimeToChinaDateUglyString">ref: 上次赴大陸日期(未拆解之醜字串)</param>
            /// <param name="Amount_ThisYearToChina">ref: 今年前往大陸次數(含本次)</param>
            /// <returns></returns>
            public static DataTable doSearch(string MZ_ID, string MZ_NAME, string MZ_DATE, ref String LastTimeToChinaDateUglyString, ref Int32 Amount_ThisYearToChina)
            {
                //本次請假之天數及時數
                Int32 ThisDays = 0;
                Int32 ThisHours = 0;

                List<string> condition = new List<string>();
                if (MZ_ID != string.Empty)
                {
                    condition.Add("CC.MZ_ID='" + o_str.tosql(MZ_ID) + "'");
                }

                if (MZ_NAME != string.Empty)
                {
                    condition.Add("CC.MZ_NAME='" + o_str.tosql(MZ_NAME) + "'");
                }


                string where = (condition.Count > 0 ? " AND " + string.Join(" AND ", condition.ToArray()) : string.Empty);

                //這一段SQL會取出該名使用者歷史以來的出國紀錄(中國)
                string strSQL = string.Format(
                                @"SELECT ROWNUM AS NUM,AAA.*,'' as MAX_C,'' as B_IDATE,'' as B_ITIME,'' as B_ODATE,'' as B_OTIME ,'' as B_TDAY,'' as B_TIME,'' as HDAY,'' as USEHDAY
                             FROM 
                           ( SELECT CC.MZ_ID, CC.MZ_NAME, MZ_EXAD_CH MZ_EXAD,MZ_EXUNIT_CH  MZ_EXUNIT , MZ_OCCC_CH   MZ_OCCC , MZ_CODE_CH  MZ_CODE,   MZ_RANK1_CH   MZ_RANK1,
                            MZ_SYSDAY, CC.MZ_IDATE1,MZ_ITIME1, CC.MZ_ODATE,MZ_OTIME,MZ_TDAY,MZ_TTIME,MZ_CAUSE, CC.MZ_MEMO,CC.MZ_SWT,MZ_TADD, 
                           MZ_ROCCC_CH  ROCCC, 
                           CC.MZ_ROCCC,MZ_CHK1,MZ_SYSTIME,MZ_FOREIGN,MZ_CHINA,MZ_RNAME   ,AB.MZ_BIR  BIR   ,AB.MZ_FDATE  FDATE, CC.MZ_LASTYEARJOBLOCATION, 
                           AB.MZ_MOVETEL, AB.MZ_ENAME
                           FROM VW_C_DLTB01 CC                        
                           LEFT JOIN  A_DLBASE AB ON AB.MZ_ID=CC.MZ_ID

                           WHERE  MZ_FOREIGN='Y' AND MZ_CHINA='Y' AND (MZ_TDAY>0 OR MZ_TTIME>0)      {0}" +
                                " ORDER BY MZ_IDATE1 DESC" +
                                " ) AAA"
                                , where);

                DataTable temp = new DataTable();
                DataTable chinaabroad = new DataTable();
                temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");


                DataRow copy;
                if (temp.Rows.Count >= 1) //20151119 Neil 由 > 1 修正為 >=1 (SQL內已包含 MZ_CHINA='Y'，故只要有值即為中國的出國紀錄)
                {
                    DataRow[] dr = temp.Select("MZ_IDATE1='" + o_str.tosql(MZ_DATE) + "'"); //撈出查詢日期，故為本次請假紀錄
                    int num = 0;
                    if (dr.Count() > 0)
                    {
                        #region 本次請假紀錄
                        num = int.Parse(dr[0]["NUM"].ToString()) - 1; //20151014 Neil : 應是減1後才是陣列的索引值? 未來若有錯再進行修感

                        if (num <= temp.Rows.Count)
                        {
                            //copy = temp.Rows[num]; //20151014 Neil : copy 不知用途，暫時移除

                            IEnumerable<DataRow> qry = from d in temp.AsEnumerable()
                                                       where d.Field<string>("MZ_IDATE1").Substring(0, 3) == (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0')
                                                       select d;

                            dr[0]["MAX_C"] = qry.Count<DataRow>();
                            dr[0]["B_IDATE"] = temp.Rows[num]["MZ_IDATE1"].ToString();
                            dr[0]["B_ITIME"] = temp.Rows[num]["MZ_ITIME1"].ToString();
                            dr[0]["B_ODATE"] = temp.Rows[num]["MZ_ODATE"].ToString();
                            dr[0]["B_OTIME"] = temp.Rows[num]["MZ_OTIME"].ToString();
                            dr[0]["B_TDAY"] = temp.Rows[num]["MZ_TDAY"].ToString();
                            dr[0]["B_TIME"] = temp.Rows[num]["MZ_TTIME"].ToString();

                        }
                        #endregion 

                        #region 取得前一次赴大陸地區資料
                        if (temp.Rows.Count >= 2) //若有兩筆以上資料才代表存在以前出國之紀錄
                        {
                            //檢驗此筆資料是否於今年度內，若是則表示為前一年度資料，故回傳此Data
                            if (temp.Rows[1]["MZ_IDATE1"].ToString().Substring(0, 3) == MZ_DATE.Substring(0, 3))
                                LastTimeToChinaDateUglyString = temp.Rows[1]["MZ_TADD"].ToString();
                        }
                        #endregion 

                        #region 取得本年度赴大陸次數
                        if (temp.Rows.Count >= 1) //若有兩筆以上資料才代表存在以前出國之紀錄
                        {
                            Amount_ThisYearToChina = 0;
                            foreach (DataRow item in temp.Rows)
                                if (item["MZ_IDATE1"].ToString().Substring(0, 3) == MZ_DATE.Substring(0, 3)) //若為相同年度才+1
                                    Amount_ThisYearToChina++;
                        }
                        #endregion 
                    }

                    //取出表頭欄位
                    DataColumn[] cols = new DataColumn[temp.Columns.Count];
                    for (int i = 0; i < temp.Columns.Count; i++)
                    {
                        cols[i] = new DataColumn(temp.Columns[i].ColumnName, temp.Columns[i].DataType);
                    }

                    //儲存本次請假之天數及時數
                    Int32.TryParse(dr[0]["MZ_TDAY"].ToString(), out ThisDays);
                    Int32.TryParse(dr[0]["MZ_TTIME"].ToString(), out ThisHours);

                    chinaabroad.Columns.AddRange(cols);
                    chinaabroad.BeginLoadData();
                    chinaabroad.LoadDataRow(dr[0].ItemArray, true);
                }
                else
                { //若無任何中國出差紀錄

                    if (temp.Rows.Count > 0)
                    {
                        DataColumn[] cols = new DataColumn[temp.Columns.Count];
                        for (int i = 0; i < temp.Columns.Count; i++)
                        {
                            cols[i] = new DataColumn(temp.Columns[i].ColumnName, temp.Columns[i].DataType);
                        }
                        chinaabroad.Columns.AddRange(cols);
                        chinaabroad.BeginLoadData();
                        chinaabroad.LoadDataRow(temp.Rows[0].ItemArray, true);
                    }
                }


                if (chinaabroad.Rows.Count > 0)
                {
                    string year = MZ_DATE.Substring(0, 3);


                    C_COUNT_HDAY COUNT = new C_COUNT_HDAY()
                    {
                        r = MZ_ID

                    };

                    List<int> count_result = COUNT.COUNT_HDAY;
                    string hday = count_result[2].ToString();
                    string htime = count_result[3].ToString();
                    int hday_use = count_result[0]; //本年度已使用天數(已審核才算)
                    int htime_use = count_result[1]; //本年度已使用時數(已審核才算)
                                                     //時數、天數之轉換已透過 C_COUNT_HDAY() 進行轉換，故這邊的時數可以不用再轉天

                    //應休假日數
                    chinaabroad.Rows[0]["HDAY"] = hday + "日" + (htime == "0" ? "" : htime + "時");

                    //加上本次請假的資料後，重新計算總日數及天數
                    hday_use += ThisDays;
                    htime_use += ThisHours;
                    LogicVacation logic = new LogicVacation();
                    logic.transformDays(ref hday_use, ref htime_use);

                    //本年度已請假日數重新加總
                    //chinaabroad.Rows[0]["USEHDAY"] = (hday_use + htime_use / 8).ToString() + "日" + ((htime_use % 8).ToString() == "0" ? "" : (htime_use % 8).ToString() + "時");
                    chinaabroad.Rows[0]["USEHDAY"] = hday_use.ToString() + "日" + ((htime_use == 0) ? "" : htime_use.ToString() + "時");

                }

                return chinaabroad;

            }

            public static string place(DataTable chinaabroad)
            {
                string[] ABDays = chinaabroad.Rows[0]["MZ_TADD"].ToString().Trim().ToUpper().Split('$');
                string result = "";
                if (chinaabroad.Rows[0]["MZ_TADD"].ToString().Contains("$"))
                {


                    result = ABDays[0].ToString().Trim();

                }
                else
                {
                    result = chinaabroad.Rows[0]["MZ_TADD"].ToString();

                }



                return result;
            }

            public static string during(DataTable chinaabroad, LogicVacation.enumChinaDateRangeFormat type)
            {
                string[] ABDays = chinaabroad.Rows[0]["MZ_TADD"].ToString().Trim().ToUpper().Split('$');
                string result = "";
                ////20140620 增加日期防呆 BY青
                ///TODO : 20151022 Neil : 原始建構方法如下，建議報表穩定後即可移除
                //if (ABDays.Length > 1)
                //{
                //    result =
                //        ABDays[1].ToString().Trim().Substring(0, 3) + "年" + ABDays[1].ToString().Trim().Substring(3, 2) + "月" + ABDays[1].ToString().Trim().Substring(5, 2) + "日" +
                //        "  起\r\n" +
                //         ABDays[3].ToString().Trim().Substring(0, 3) + "年" + ABDays[3].ToString().Trim().Substring(3, 2) + "月" + ABDays[3].ToString().Trim().Substring(5, 2) + "日" +
                //        "  止\r\n" +
                //        "共計：" + ABDays[5].ToString().Trim() + "日";

                //}

                //20151014 Neil : 以新的邏輯層取代
                LogicVacation logic = new LogicVacation();
                result = logic.parseChinaDateRange(chinaabroad.Rows[0]["MZ_TADD"].ToString(), type);

                return result;

            }

            public static String during(String UglyString, LogicVacation.enumChinaDateRangeFormat type)
            {
                LogicVacation logic = new LogicVacation();
                String result = logic.parseChinaDateRange(UglyString, type);

                return result;
            }
        }

        /// <summary>
        /// 4.3.10出國請假報告單
        /// </summary>
        public class C_abroad
        {
            public static DataTable doSearch(string MZ_ID, string MZ_NAME, string MZ_DATE)
            {
                //特殊:如果有指定身分證,就不要把姓名當查詢條件了,某些姓名似乎會導致查不到資料
                if (string.IsNullOrEmpty(MZ_ID) == false)
                {
                    MZ_NAME = "";
                }

                string strSQL = @"
                    SELECT 
                        CC.MZ_ID, CC.MZ_NAME, MZ_EXAD_CH MZ_EXAD, MZ_EXUNIT_CH MZ_EXUNIT, MZ_OCCC_CH MZ_OCCC, MZ_CODE_CH MZ_CODE,
                        MZ_RANK1_CH   MZ_RANK1,
                        MZ_SYSDAY,CC.MZ_IDATE1,MZ_ITIME1,CC.MZ_ODATE,MZ_OTIME,MZ_TDAY,MZ_TTIME,MZ_CAUSE,CC.MZ_MEMO,CC.MZ_SWT,MZ_TADD,MZ_RNAME,
                        MZ_ROCCC_CH  ROCCC, MZ_CHK1,MZ_SYSTIME,
                        MZ_FOREIGN,MZ_CHINA,AB.MZ_FDATE,'' AS MZ_HDAY,'' AS HDAY_USE,'' AS HTIME_USE  , CC.MZ_LASTYEARJOBLOCATION 
                    FROM  VW_C_DLTB01 CC  
                    LEFT JOIN  A_DLBASE AB ON AB.MZ_ID=CC.MZ_ID
                    WHERE MZ_FOREIGN='Y'  AND CC.MZ_IDATE1='" + o_str.tosql(MZ_DATE) + "' AND (MZ_TDAY>0 OR MZ_TTIME>0)";

                if (MZ_NAME != "")
                {
                    strSQL += " AND CC.MZ_NAME=N'" + o_str.tosql(MZ_NAME) + "' ";
                }
                if (MZ_ID != "")
                {
                    strSQL += " AND CC.MZ_ID='" + o_str.tosql(MZ_ID) + "' ";
                }

                DataTable abroad = new DataTable();

                abroad = o_DBFactory.ABC_toTest.Create_Table(strSQL, "punT");
                for (int i = 0; i < abroad.Rows.Count; i++)
                {
                    if (abroad.Rows[0]["MZ_TADD"].ToString().Contains("$"))
                    {

                        string[] ABDays = abroad.Rows[i]["MZ_TADD"].ToString().Trim().ToUpper().Split('$');

                        // Joy 新增欄位存取外出地點
                        abroad.Columns.Add("Place");
                        abroad.Rows[i]["Place"] = ABDays[0].ToString();

                        abroad.Rows[i]["MZ_TADD"] = //ABDays[0].ToString().Trim() + "\r\n" +
                            ABDays[1].ToString().Trim().Substring(0, 3) + "年" + ABDays[1].ToString().Trim().Substring(3, 2) + "月" + ABDays[1].ToString().Trim().Substring(5, 2) + "日" +
                            ABDays[2].ToString().Trim().Substring(0, 2) + "時" + ABDays[2].ToString().Trim().Substring(2, 2) + "分  起\r\n" +
                             ABDays[3].ToString().Trim().Substring(0, 3) + "年" + ABDays[3].ToString().Trim().Substring(3, 2) + "月" + ABDays[3].ToString().Trim().Substring(5, 2) + "日" +
                            ABDays[4].ToString().Trim().Substring(0, 2) + "時" + ABDays[4].ToString().Trim().Substring(2, 2) + "分  止\r\n" +
                            "共計：" + ABDays[5].ToString().Trim() + "日" + ABDays[6].ToString().Trim() + "時";
                    }
                    else
                    {
                        abroad.Rows[i]["MZ_TADD"] = "\r\n" + " \r\n" + abroad.Rows[i]["MZ_TADD"].ToString();
                    }

                    C_COUNT_HDAY COUNT = new C_COUNT_HDAY()
                    {
                        r = abroad.Rows[i]["MZ_ID"].ToString()

                    };

                    //本次請假總天數/時數
                    Int32 ThisDays = 0;
                    Int32 ThisHours = 0;
                    if (abroad.Rows[i]["MZ_CODE"].ToString().Trim() == "休假")
                    {
                        Int32.TryParse(abroad.Rows[i]["MZ_TDAY"].ToString(), out ThisDays);
                        Int32.TryParse(abroad.Rows[i]["MZ_TTIME"].ToString(), out ThisHours);
                    }

                    //加上本次請假的資料後，重新計算總日數及天數
                    List<Int32> count_result = COUNT.COUNT_HDAY;
                    Int32 hday_use = count_result[0]; hday_use += ThisDays;
                    Int32 htime_use = count_result[1]; htime_use += ThisHours;

                    LogicVacation logic = new LogicVacation();
                    logic.transformDays(ref hday_use, ref htime_use);

                    abroad.Rows[i]["HDAY_USE"] = hday_use.ToString();
                    abroad.Rows[i]["HTIME_USE"] = htime_use.ToString();
                    abroad.Rows[i]["MZ_HDAY"] = count_result[2].ToString();
                    abroad.Rows[i]["MZ_FDATE"] = abroad.Rows[i]["MZ_FDATE"].ToString().Substring(0, 3) + "年" + abroad.Rows[i]["MZ_FDATE"].ToString().Substring(3, 2) + "月" + abroad.Rows[i]["MZ_FDATE"].ToString().Substring(5, 2) + "日";

                }

                return abroad;

            }
        }


        /// <summary>
        /// 4.3.11職員非請假出國報備表
        /// </summary>
        public class C_ondutyabroad
        {
            public static DataTable doSearch(string MZ_ID, string MZ_NAME, string MZ_DATE)
            {
                string strSQL = "";


                string sqlpart = "";

                if (MZ_NAME != "")
                {
                    sqlpart += " AND MZ_NAME='" + o_str.tosql(MZ_NAME) + "'";
                }


                if (!string.IsNullOrEmpty(MZ_DATE))
                {
                    strSQL = @"SELECT MZ_CODE,MZ_ID,MZ_NAME, MZ_EXAD_CH MZ_EXAD,MZ_EXUNIT_CH  MZ_EXUNIT,
                                MZ_OCCC_CH MZ_OCCC,MZ_CODE,MZ_RANK1_CH MZ_RANK1,
                                MZ_SYSDAY,MZ_IDATE1,MZ_ITIME1,MZ_ODATE,MZ_OTIME,MZ_TDAY,MZ_TTIME,MZ_CAUSE,MZ_MEMO,MZ_SWT,MZ_TADD,MZ_RNAME,MZ_ROCCC,MZ_CHK1,MZ_SYSTIME,MZ_FOREIGN,MZ_CHINA ,
                                MZ_LASTYEARJOBLOCATION 
                                FROM VW_C_DLTB01 
                                WHERE MZ_CODE IN (91,92,93) 
                                AND MZ_ID='" + o_str.tosql(MZ_ID) + "' AND MZ_IDATE1='" + o_str.tosql(MZ_DATE) + "' " + sqlpart;
                }
                else
                {
                    strSQL = "SELECT 'no_data' as MZ_CODE,MZ_ID,MZ_NAME,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=MZ_EXAD AND MZ_KTYPE='04') MZ_EXAD,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=MZ_EXUNIT AND MZ_KTYPE='25') MZ_EXUNIT,MZ_OCCC,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=MZ_RANK1 AND MZ_KTYPE='09') MZ_RANK1,MZ_ID,'' AS MZ_IDATE1,'' AS MZ_ODATE,'' AS MZ_TADD,'' AS MZ_TDAY,'' AS MZ_TTIME,'' AS MZ_ITIME1,'' AS MZ_OTIME FROM A_DLBASE WHERE MZ_ID='" + MZ_ID + "' " + sqlpart;
                }


                DataTable ondutyabroad = o_DBFactory.ABC_toTest.Create_Table(strSQL, "punT");

                for (int i = 0; i < ondutyabroad.Rows.Count; i++)
                {
                    if (ondutyabroad.Rows[0]["MZ_TADD"].ToString().Contains("$"))
                    {

                        string[] ABDays = ondutyabroad.Rows[i]["MZ_TADD"].ToString().Trim().ToUpper().Split('$');
                        ondutyabroad.Rows[i]["MZ_TADD"] = ABDays[0].ToString().Trim() + "\r\n" +
                            ABDays[1].ToString().Trim().Substring(0, 3) + "年" + ABDays[1].ToString().Trim().Substring(3, 2) + "月" + ABDays[1].ToString().Trim().Substring(5, 2) + "日" +
                            ABDays[2].ToString().Trim().Substring(0, 2) + "時" + ABDays[2].ToString().Trim().Substring(2, 2) + "分  起\r\n" +
                             ABDays[3].ToString().Trim().Substring(0, 3) + "年" + ABDays[3].ToString().Trim().Substring(3, 2) + "月" + ABDays[3].ToString().Trim().Substring(5, 2) + "日" +
                            ABDays[4].ToString().Trim().Substring(0, 2) + "時" + ABDays[4].ToString().Trim().Substring(2, 2) + "分  止\r\n" +
                            "共計：" + ABDays[5].ToString().Trim() + "日" + ABDays[6].ToString().Trim() + "時";
                    }
                    else
                    {
                        ondutyabroad.Rows[i]["MZ_TADD"] = "\r\n" + " \r\n" + ondutyabroad.Rows[i]["MZ_TADD"].ToString();


                    }

                }

                return ondutyabroad;
            }

        }

        /// <summary>
        /// 4.3.14出差前50名報表
        /// </summary>
        public class C_busitrip50
        {
            public static DataTable doSearch(string DATE1, string DATE2, string MZ_EXAD)
            {
                List<string> condition = new List<string>();
                if (DATE1.Trim() != string.Empty)
                {
                    DATE1 = DATE1.PadLeft(7, '0');
                    DATE2 = DATE2.PadLeft(7, '0');

                    condition.Add("MZ_IDATE1 >= '" + o_str.tosql(DATE1.Replace("/", string.Empty).PadLeft(7, '0')) + "'");
                    if (DATE2.Trim() != string.Empty)
                    {
                        condition.Add("MZ_IDATE1 <= '" + o_str.tosql(DATE2.Replace("/", string.Empty).PadLeft(7, '0')) + "'");

                    }
                    else
                    {
                        condition.Add("MZ_IDATE1 <= '" + o_str.tosql(DATE1.Replace("/", string.Empty).PadLeft(7, '0')) + "'");

                    }


                }

                if (MZ_EXAD != string.Empty)
                {
                    condition.Add("MZ_EXAD='" + MZ_EXAD + "'");
                }

                condition.Add("MZ_CODE IN ('07')");

                string where = (condition.Count > 0 ? " WHERE " + string.Join(" AND ", condition.ToArray()) : string.Empty);

                string strSQL = string.Format(
                               "SELECT ROWNUM AS NUM,AAA.*" +
                               " FROM" +
                               "(" +
                               " SELECT MZ_ID,MZ_NAME,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=MZ_EXAD AND MZ_KTYPE='04') MZ_EXAD,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=MZ_EXUNIT AND MZ_KTYPE='25') MZ_EXUNIT,MZ_OCCC,SUM(MZ_TDAY*8 + MZ_TTIME) TOTAL" +
                               " FROM C_DLTB01 {0} AND (MZ_TDAY>0 OR MZ_TTIME>0)" +
                               " GROUP BY MZ_ID,MZ_NAME,MZ_EXAD,MZ_EXUNIT,MZ_OCCC" +
                               " ORDER BY TOTAL DESC" +
                               ") AAA " +
                               " WHERE ROWNUM <=50   "
                               , where);

                DataTable busitrip50 = new DataTable();
                busitrip50 = o_DBFactory.ABC_toTest.Create_Table(strSQL, "busitrip50");
                foreach (DataRow dr in busitrip50.Rows)
                {
                    dr["MZ_OCCC"] = o_A_DLBASE.EXTPOS_OR_OCCC(dr["MZ_ID"].ToString(), dr["MZ_OCCC"].ToString());
                }

                return busitrip50;

            }


            public static string UP(string DATE1, string DATE2)
            {
                string up = string.Empty;
                if (DATE1.Trim() != string.Empty)
                {
                    DATE1 = DATE1.PadLeft(7, '0');
                    DATE2 = DATE2.PadLeft(7, '0');


                    up += "統計日期： " + DATE1.Substring(0, 3) + "年" + DATE1.Substring(3, 2) + "月" + DATE1.Substring(5, 2) + "日";
                    if (DATE2.Trim() != string.Empty)
                    {

                        up += "至" + DATE2.Substring(0, 3) + "年" + DATE2.Substring(3, 2) + "月" + DATE2.Substring(5, 2) + "日";
                    }
                    else
                    {

                        up += "至" + DATE1.Substring(0, 3) + "年" + DATE1.Substring(3, 2) + "月" + DATE1.Substring(5, 2) + "日";
                    }


                }
                return up;

            }
        }






        /// <summary>
        /// 4.3.18 刷卡(勤惰)明細表
        /// </summary>
        public class C_diffdutydetail
        {
            public static DataTable diffdutydetail = new DataTable();


            // sam
            public static DataTable doSearch(string DATE1, string DATE2, string MZ_EXAD, string MZ_EXUNIT, string MZ_ID, string MZ_NAME, string type)
            {
                try
                {

                    // DataTable diffdutydetail = new DataTable();
                    //diffdutydetail.Columns.Clear();
                    diffdutydetail.Clear();
                    diffdutydetail.Columns.Clear();

                    diffdutydetail.Columns.Add("UNIT", typeof(string));
                    diffdutydetail.Columns.Add("OCCC", typeof(string));
                    diffdutydetail.Columns.Add("NAME", typeof(string));
                    diffdutydetail.Columns.Add("LOGDATE", typeof(string));
                    diffdutydetail.Columns.Add("INTIME", typeof(string));
                    diffdutydetail.Columns.Add("OUTTIME", typeof(string));
                    diffdutydetail.Columns.Add("KIND", typeof(string));
                    diffdutydetail.Columns.Add("MEMO", typeof(string));

                    if (int.Parse(type) == 4)
                    {
                        diffdutydetail.Columns.Add("DAYSECOND", typeof(string));
                    }
                    string SQLPART = "";

                    string SQLDATE = "";

                    string date1 = "";

                    string date2 = "";

                    string begindate = "";
                    string enddate = "";



                    if (DATE1 != string.Empty && DATE2 != string.Empty)
                    {

                        date1 = o_str.tosql(DATE1.Replace("/", string.Empty).PadLeft(7, '0'));
                        date2 = o_str.tosql(DATE2.Replace("/", string.Empty).PadLeft(7, '0'));
                        begindate = (int.Parse(date1.Substring(0, 3)) + 1911).ToString() + "/" + date1.Substring(3, 2) + "/" + date1.Substring(5, 2);
                        enddate = (int.Parse(date2.Substring(0, 3)) + 1911).ToString() + "/" + date2.Substring(3, 2) + "/" + date2.Substring(5, 2);

                        SQLDATE = " AND LOGDATE>='" + begindate + "' AND LOGDATE<='" + enddate + "'";


                    }
                    else if (DATE1 != string.Empty)
                    {

                        date1 = o_str.tosql(DATE1.Replace("/", string.Empty).PadLeft(7, '0'));
                        begindate = (int.Parse(date1.Substring(0, 3)) + 1911).ToString() + "/" + date1.Substring(3, 2) + "/" + date1.Substring(5, 2);
                        SQLDATE = " AND LOGDATE='" + begindate + "'";


                    }

                    if (MZ_EXUNIT != string.Empty)
                    {
                        SQLPART = " AND MZ_EXUNIT='" + MZ_EXUNIT + "'";
                    }

                    if (!string.IsNullOrEmpty(MZ_ID))
                    {
                        SQLPART += " AND MZ_ID='" + MZ_ID + "'";
                    }

                    if (!string.IsNullOrEmpty(MZ_NAME))
                    {
                        SQLPART += " AND MZ_NAME='" + MZ_NAME + "'";
                    }
                    if (!string.IsNullOrEmpty(MZ_EXAD))
                    {
                        SQLPART += " AND MZ_EXAD='" + MZ_EXAD + "'";
                    }

                    // sam 20200630 將已離職的先Filter
                    SQLPART += " AND mz_status2 = 'Y' ";


                    string strSQL = @"SELECT MZ_ID,RTRIM(A_DLBASE.MZ_NAME) NAME, A_DLBASE.MZ_EXUNIT ,nvl(AKO.MZ_KCHI,AKEX.MZ_KCHI) OCCC,dbo.to_number(CASE WHEN MZ_TBDV IS NULL THEN '999' WHEN MZ_TBDV='Z99' THEN '999' ELSE MZ_TBDV END) AS TBDV 
FROM A_DLBASE
LEFT JOIN A_KTYPE AKO ON AKO.MZ_KCODE =A_DLBASE.MZ_OCCC AND AKO.MZ_KTYPE='26' 
 LEFT JOIN A_KTYPE AKEX ON AKEX.MZ_KCODE =A_DLBASE.MZ_EXTPOS AND AKEX.MZ_KTYPE='@91'
WHERE 1=1" + SQLPART + " AND MZ_ID IN (SELECT MZ_ID FROM C_CARDSET WHERE (MZ_CLOCK='Y' OR MZ_OVERTIME='Y')) ORDER BY MZ_EXUNIT,TBDV,OCCC";

                    DataTable tempDT = new DataTable();

                    tempDT = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GETID");



                    if (DATE1 != string.Empty && DATE2 != string.Empty)
                    {
                        //DateTime TS1 = DateTime.Parse(begindate);
                        //DateTime TS2 = DateTime.Parse(enddate);

                        DateTime TS1;
                        DateTime TS2;
                        DateTime.TryParse(begindate, out TS1);
                        DateTime.TryParse(enddate, out TS2);

                        TimeSpan TS = TS2 - TS1;

                        for (int i = 0; i < tempDT.Rows.Count; i++)
                        {
                            if (TS1 == DateTime.MinValue || TS2 == DateTime.MinValue)
                            {
                                continue;
                            }

                            for (int j = 0; j <= TS.Days; j++)
                            {
                                //去抓刷卡紀錄
                                DataRow drr = Count_Card_Record(tempDT.Rows[i]["MZ_ID"].ToString(), TS1.AddDays(j), type, tempDT.Rows[i]["MZ_EXUNIT"].ToString(), tempDT.Rows[i]["NAME"].ToString(), tempDT.Rows[i]["OCCC"].ToString());

                                if (!string.IsNullOrEmpty(drr[0].ToString()))
                                {

                                    diffdutydetail.Rows.Add(drr);


                                }


                            }
                        }
                    }
                    else if (DATE1 != string.Empty)
                    {
                        for (int i = 0; i < tempDT.Rows.Count; i++)
                        {
                            DateTime TS1 = DateTime.Parse(begindate);

                            diffdutydetail.Rows.Add(Count_Card_Record(tempDT.Rows[i]["MZ_ID"].ToString(), TS1, type, tempDT.Rows[i]["MZ_EXUNIT"].ToString(), tempDT.Rows[i]["NAME"].ToString(), tempDT.Rows[i]["OCCC"].ToString()));
                        }
                    }

                    for (int i = 0; i < diffdutydetail.Rows.Count; i++)
                    {
                        diffdutydetail.Rows[i]["UNIT"] = o_A_KTYPE.RUNIT(diffdutydetail.Rows[i]["UNIT"].ToString());
                    }



                    return diffdutydetail;


                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    var result = MZ_ID + " || " + MZ_NAME + " || " + MZ_EXUNIT;
                    throw;
                }

            }

            /// <summary>
            /// 產生單筆資料
            /// </summary>
            /// <param name="MZ_ID">身分證號</param>
            /// <param name="DATE">查詢日期</param>
            /// <param name="type">報表類型</param>
            /// <param name="punit">列印服務單位</param>
            /// <param name="name">列印姓名</param>
            /// <param name="occc">列印職稱</param>
            /// <returns></returns>
            protected static DataRow Count_Card_Record(string MZ_ID, DateTime DATE, string type, string punit, string name, string occc)
            {
                try
                {

                    string LOGDATE = DATE.Year.ToString() + "/" + DATE.Month.ToString().PadLeft(2, '0') + "/" + DATE.Day.ToString().PadLeft(2, '0');
                    string LOGDATE1 = (DATE.Year - 1911).ToString().PadLeft(3, '0') + DATE.Month.ToString().PadLeft(2, '0') + DATE.Day.ToString().PadLeft(2, '0');

                    string KIND = "";
                    string MEMO = "";
                    string CODE = "";
                    string day_second = "";

                    //請假/國定休假日紀錄清單
                    // 陣列意義 > 一般請假：[0]假別, [1]請假起日, [2]起時, [3]訖日, [4]迄時, [5]日數, [6]時數；國定假日：只回傳假日名稱
                    List<String> lstDayOff = new List<String>();
                    //這是在找有休假的的資料 
                    lstDayOff = C_CountCardRecord.list_Abnormal(MZ_ID, LOGDATE);

                    string selectINTIME = "";
                    string selectOUTTIME = "";
                    //值日
                    string selectINTIME_DAY = "";
                    string selectOUTTIME_DAY = "";
                    //超勤
                    string selectINTIME_over = "";
                    string selectOUTTIME_over = "";
                    //matthew 因公未刷卡
                    string C_CARD_EDIT = "";

                    #region 加班

                    if (type == "2")//加班 CheckBox2.Checked
                    {
                        //雖然說為什麼要進出資料庫找兩次.但找起來還算快.就先不改
                        selectINTIME = o_DBFactory.ABC_toTest.vExecSQL("SELECT LOGTIME FROM (SELECT LOGTIME,ROW_NUMBER() OVER(ORDER BY LOGTIME) AS ROWCOUNT FROM C_CARDHISTORY_NEW WHERE dbo.SUBSTR(USERNAME,0,10)='" + MZ_ID + "' AND LOGDATE='" + LOGDATE + "' AND (FKEY='NONE' OR FKEY='F3') AND VERIFY='IN') WHERE ROWCOUNT=1");
                        if (selectINTIME.Length > 0 && selectINTIME.Substring(0, 3) == "24:")
                        {
                            selectINTIME = "00:" + selectINTIME.Substring(3, 5);
                        }
                        selectOUTTIME = o_DBFactory.ABC_toTest.vExecSQL("SELECT LOGTIME FROM (SELECT LOGTIME,ROW_NUMBER() OVER(ORDER BY LOGTIME DESC) AS ROWCOUNT FROM C_CARDHISTORY_NEW WHERE dbo.SUBSTR(USERNAME,0,10)='" + MZ_ID + "' AND LOGDATE='" + LOGDATE + "' AND FKEY='F3' AND VERIFY='IN') WHERE ROWCOUNT=1");
                        if (selectOUTTIME.Length > 0 && selectOUTTIME.Substring(0, 3) == "24:")
                        {
                            selectOUTTIME = "00:" + selectOUTTIME.Substring(3, 5);
                        }
                        //雖然說為什麼要進出資料庫找兩次.但找起來還算快.就先不改

                        KIND = "加班";
                    }
                    #endregion 加班
                    #region 超勤

                    else if (type == "3")
                    {
                        DateTime nowDate = DateTime.Parse(LOGDATE);
                        string fDATE = nowDate.AddDays(-1).ToString("yyyy/MM/dd"); //前一天日期
                        string lDATE = nowDate.AddDays(1).ToString("yyyy/MM/dd");  //後一天日期

                        //前一天
                        string fMonth = fDATE.Substring(5, 2);
                        string fDate = fDATE.Substring(8, 2);

                        //後一天
                        string lMonth = lDATE.Substring(5, 2);
                        string lDate = lDATE.Substring(8, 2);


                        string OverINTIME = "";
                        string OverOUTTIME = "";
                        string ftime = "";
                        string ltime = "";

                        List<string> Overdata = new List<string>();

                        ftime = o_DBFactory.ABC_toTest.vExecSQL("SELECT LOGTIME FROM (SELECT LOGTIME,ROW_NUMBER() OVER(ORDER BY LOGTIME DESC) AS ROWCOUNT FROM C_CARDHISTORY_NEW WHERE dbo.SUBSTR(USERNAME,0,10)='" + MZ_ID + "' AND LOGDATE='" + fDATE + "' AND FKEY='F4' AND VERIFY='IN') WHERE ROWCOUNT=1"); //計算前天最晚刷卡時間


                        OverINTIME = o_DBFactory.ABC_toTest.vExecSQL("SELECT LOGTIME FROM (SELECT LOGTIME,ROW_NUMBER() OVER(ORDER BY LOGTIME) AS ROWCOUNT FROM C_CARDHISTORY_NEW WHERE dbo.SUBSTR(USERNAME,0,10)='" + MZ_ID + "' AND LOGDATE='" + LOGDATE + "' AND FKEY='F4' AND VERIFY='IN') WHERE ROWCOUNT=1"); //計算當天最早刷卡時間

                        OverOUTTIME = o_DBFactory.ABC_toTest.vExecSQL("SELECT LOGTIME FROM (SELECT LOGTIME,ROW_NUMBER() OVER(ORDER BY LOGTIME DESC) AS ROWCOUNT FROM C_CARDHISTORY_NEW WHERE dbo.SUBSTR(USERNAME,0,10)='" + MZ_ID + "' AND LOGDATE='" + LOGDATE + "' AND FKEY='F4' AND VERIFY='IN') WHERE ROWCOUNT=1"); //計算當天最晚刷卡時間

                        ltime = o_DBFactory.ABC_toTest.vExecSQL("SELECT LOGTIME FROM (SELECT LOGTIME,ROW_NUMBER() OVER(ORDER BY LOGTIME) AS ROWCOUNT FROM C_CARDHISTORY_NEW WHERE dbo.SUBSTR(USERNAME,0,10)='" + MZ_ID + "' AND LOGDATE='" + lDATE + "' AND FKEY='F4' AND VERIFY='IN') WHERE ROWCOUNT=1");//計算隔天最早刷卡時間

                        TimeSpan T_allday;
                        DateTime TOverOUTTIME, TOverIntime;

                        //處理時間顯示

                        if (!string.IsNullOrEmpty(OverOUTTIME))
                        {
                            OverOUTTIME = OverOUTTIME.Substring(0, 5);

                        }

                        if (!string.IsNullOrEmpty(OverINTIME))
                        {
                            OverINTIME = OverINTIME.Substring(0, 5);

                        }

                        if (!string.IsNullOrEmpty(ltime))
                        {
                            ltime = ltime.Substring(0, 5);

                        }

                        if (!string.IsNullOrEmpty(ftime))
                        {
                            ftime = ftime.Substring(0, 5);
                        }


                        //判斷是否有異常刷卡
                        if (OverOUTTIME != "" && OverINTIME != "")
                        {
                            TOverOUTTIME = DateTime.Parse(LOGDATE + " " + OverOUTTIME);
                            TOverIntime = DateTime.Parse(LOGDATE + " " + OverINTIME);
                            T_allday = TOverOUTTIME - TOverIntime;
                            if (T_allday.TotalMinutes < 10)
                            {
                                OverOUTTIME = OverINTIME;
                            }
                        }




                        if (ftime == "" && ltime == "" && OverINTIME != "" && OverOUTTIME != "" && OverINTIME != OverOUTTIME) //只有當天有資料
                        {
                            Overdata.Insert(0, OverINTIME);
                            Overdata.Insert(1, OverOUTTIME);
                        }

                        else if (OverINTIME == OverOUTTIME && OverOUTTIME != "" && ltime != "") //當天只有一筆資料 隔天有資料
                        {
                            Overdata.Insert(0, OverOUTTIME);
                            Overdata.Insert(1, lDate + "日" + ltime);

                        }
                        else if (ftime == "" && ltime != "" && OverINTIME != "" && OverOUTTIME != "" && OverINTIME != OverOUTTIME) //當天有兩筆資料隔天也有
                        {
                            Overdata.Insert(0, OverINTIME);
                            Overdata.Insert(1, OverOUTTIME);
                        }
                        else if (OverINTIME == OverOUTTIME && OverOUTTIME != "" && ftime != "" && diffdutydetail.Rows.Count == 0) //當天只有一筆資料 前一天有資料 只在第一天作用
                        {
                            Overdata.Insert(0, fDate + "日" + ftime);
                            Overdata.Insert(1, OverINTIME);
                        }
                        else if (OverOUTTIME != "" && OverINTIME != "" && ftime != "" && ltime != "" && OverOUTTIME != OverINTIME)//四筆都有資料
                        {
                            Overdata.Insert(0, OverOUTTIME);
                            Overdata.Insert(1, lDate + "日" + ltime);

                        }
                        else if (OverOUTTIME == OverINTIME && OverINTIME != "")
                        {
                            Overdata.Insert(0, OverINTIME);
                            Overdata.Insert(1, "");
                        }





                        string againtime_over = "";
                        bool kind_over = false;
                        string fday = "";

                        string ITEMA = Count(MZ_ID, LOGDATE1);
                        string[] COUNTA = { "" };
                        int ITEMAA = 0;
                        //計算超勤時間和系統時間比對
                        if (ITEMA != "")
                        {
                            COUNTA = ITEMA.Split('.');
                            ITEMAA = COUNTA.Length;
                            string over_in = ""; string over_out = "";
                            if (COUNTA.Length > 1)
                            {
                                int numA = COUNTA.Length - 1;
                                over_in = COUNTA[0].Substring(0, 2) + ":" + COUNTA[0].Substring(2, 2);

                                over_out = COUNTA[numA].Substring(5, 2) + ":" + COUNTA[numA].Substring(7, 2);
                                if (COUNTA.Length >= 19)
                                    over_out = lDATE.Substring(5, 2) + "日" + COUNTA[numA].Substring(5, 2) + ":" + COUNTA[numA].Substring(7, 2);

                                if (Overdata.Count == 0)
                                {
                                    KIND += " 超勤異常";
                                }
                                else if (Overdata.Count == 2)
                                {
                                    if (Overdata[0] != "")
                                    {
                                        TimeSpan TS;
                                        DateTime TS1, TS2;
                                        if (Overdata[0].Contains("日")) //有包含日的話則為前一天
                                        {
                                            TS1 = DateTime.Parse(fDATE + " " + Overdata[0]);//指紋起

                                            TS2 = DateTime.Parse(fDATE + " " + over_in); //   系統起
                                            TS = TS1 - TS2;

                                            if (TS.TotalHours > 1)
                                            {
                                                Overdata[0] = "";
                                            }
                                        }
                                        else
                                        {
                                            TS1 = DateTime.Parse(LOGDATE + " " + Overdata[0]);//起

                                            TS2 = DateTime.Parse(LOGDATE + " " + over_in); //   系統起
                                            TS = TS1 - TS2;

                                            if (TS.TotalHours > 1)
                                            {
                                                Overdata[0] = "";
                                            }

                                        }

                                    }

                                    if (Overdata[1] != "")
                                    {
                                        TimeSpan TS;
                                        DateTime TS1, TS2;
                                        if (Overdata[1].Contains("日")) //有包含日的話則為後一天
                                        {
                                            TS1 = DateTime.Parse(lDATE + " " + Overdata[1]);//指紋迄

                                            TS2 = DateTime.Parse(lDATE + " " + over_out); //   系統迄
                                            TS = TS1 - TS2;

                                            if (TS.TotalHours > 1)
                                            {
                                                Overdata[1] = "";
                                            }
                                        }
                                        else
                                        {
                                            TS1 = DateTime.Parse(LOGDATE + " " + Overdata[1]);//迄

                                            TS2 = DateTime.Parse(LOGDATE + " " + over_out); //   系統迄
                                            TS = TS1 - TS2;

                                            if (TS.TotalHours > 1)
                                            {
                                                Overdata[1] = "";
                                            }

                                        }

                                    }

                                    if (diffdutydetail.Rows.Count != 0)
                                    {
                                        againtime_over = startTime(Overdata[0], diffdutydetail, "超勤");//判斷前一天的迄和今天的起是否相同
                                        kind_over = adkind(diffdutydetail, "超勤"); //判斷前一天是否有刷卡異常
                                        fday = fday_time(diffdutydetail);
                                        if (fday != "")
                                        {
                                            fday = int.Parse(fday.Substring(0, 3)) + 1911 + fday.Substring(3, 6);
                                        }
                                    }
                                    if (againtime_over == Overdata[0] && kind_over == false && fday == fDATE)
                                    {
                                        selectINTIME_over = "";
                                        selectOUTTIME_over = "";

                                    }
                                    else if (againtime_over != Overdata[0] && kind_over == false)
                                    {
                                        selectINTIME_over = Overdata[0];
                                        selectOUTTIME_over = Overdata[1];
                                    }
                                    else
                                    {
                                        selectINTIME_over = Overdata[0];
                                        selectOUTTIME_over = Overdata[1];

                                    }

                                    if (Overdata[1].Length > 5 || Overdata[0].Length > 5)
                                    {
                                        KIND += " 超勤跨日";

                                    }
                                    if (selectINTIME_over != "" && selectOUTTIME_over == "")
                                    {
                                        KIND += " 超勤異常";
                                    }



                                }

                            }

                        }
                        else
                        {
                            if (Overdata.Count == 2)
                            {
                                if (diffdutydetail.Rows.Count != 0)
                                {
                                    againtime_over = startTime(Overdata[0], diffdutydetail, "超勤");//判斷前一天的迄和今天的起是否相同
                                    kind_over = adkind(diffdutydetail, "超勤"); //判斷前一天是否有刷卡異常
                                    fday = fday_time(diffdutydetail);
                                    if (fday != "")
                                    {
                                        fday = int.Parse(fday.Substring(0, 3)) + 1911 + fday.Substring(3, 6);
                                    }
                                }
                                if (againtime_over == Overdata[0] && kind_over == false && fday == fDATE)
                                {
                                    selectINTIME_over = "";
                                    selectOUTTIME_over = "";

                                }
                                else if (againtime_over != Overdata[0] && kind_over == false)
                                {
                                    selectINTIME_over = Overdata[0];
                                    selectOUTTIME_over = Overdata[1];
                                }
                                else
                                {
                                    selectINTIME_over = Overdata[0];
                                    selectOUTTIME_over = Overdata[1];

                                }

                                if (Overdata[1].Length > 5 || Overdata[0].Length > 5)
                                {
                                    KIND += " 超勤跨日";

                                }
                                if (selectINTIME_over != "" && selectOUTTIME_over == "")
                                {
                                    KIND += " 超勤異常";
                                }
                            }

                        }



                        //if (Overdata.Count == 2)
                        //{
                        //    if (diffdutydetail.Rows.Count != 0)
                        //    {
                        //        againtime_over = startTime(Overdata[0], diffdutydetail, "超勤");//判斷前一天的迄和今天的起是否相同
                        //        kind_over = adkind(diffdutydetail, "超勤"); //判斷前一天是否有刷卡異常
                        //        fday = fday_time(diffdutydetail);
                        //        if (fday != "")
                        //        {
                        //            fday = int.Parse(fday.Substring(0, 3)) + 1911 + fday.Substring(3, 6);
                        //        }
                        //    }
                        //    if (againtime_over == Overdata[0] && kind_over == false && fday == fDATE)
                        //    {
                        //        selectINTIME_over = "";
                        //        selectOUTTIME_over = "";

                        //    }
                        //    else if (againtime_over != Overdata[0] && kind_over == false)
                        //    {
                        //        selectINTIME_over = Overdata[0];
                        //        selectOUTTIME_over = Overdata[1];
                        //    }
                        //    else
                        //    {
                        //        selectINTIME_over = Overdata[0];
                        //        selectOUTTIME_over = Overdata[1];

                        //    }

                        //    if (Overdata[1].Length > 5 || Overdata[0].Length > 5)
                        //    {
                        //        KIND += " 超勤跨日";

                        //    }
                        //    if (selectINTIME_over != "" && selectOUTTIME_over == "")
                        //    {
                        //        KIND += " 超勤異常";
                        //    }

                        //}

                    }
                    #endregion 超勤
                    #region 值日

                    else if (type == "4")
                    {

                        DateTime nowDate = DateTime.Parse(LOGDATE);
                        string fDATE = nowDate.AddDays(-1).ToString("yyyy/MM/dd"); //前一天日期
                        string lDATE = nowDate.AddDays(1).ToString("yyyy/MM/dd");  //後一天日期

                        //前一天
                        string fMonth = fDATE.Substring(5, 2);
                        string fDate = fDATE.Substring(8, 2);

                        //後一天
                        string lMonth = lDATE.Substring(5, 2);
                        string lDate = lDATE.Substring(8, 2);

                        string ftime = "";
                        //string ltime = "";
                        string ftime_day = "";
                        string ltime_day = "";
                        string OverINTIME_day = "";
                        string OverOUTTIME_day = "";


                        List<string> Overdata_day = new List<string>();

                        ftime_day = o_DBFactory.ABC_toTest.vExecSQL("SELECT LOGTIME FROM (SELECT LOGTIME,ROW_NUMBER() OVER(ORDER BY LOGTIME DESC) AS ROWCOUNT FROM C_CARDHISTORY_NEW WHERE dbo.SUBSTR(USERNAME,0,10)='" + MZ_ID + "' AND LOGDATE='" + fDATE + "' AND  FKEY='F2' AND VERIFY='IN') WHERE ROWCOUNT=1"); //計算前天最晚刷卡時間
                        OverINTIME_day = o_DBFactory.ABC_toTest.vExecSQL("SELECT LOGTIME FROM (SELECT LOGTIME,ROW_NUMBER() OVER(ORDER BY LOGTIME) AS ROWCOUNT FROM C_CARDHISTORY_NEW WHERE dbo.SUBSTR(USERNAME,0,10)='" + MZ_ID + "' AND LOGDATE='" + LOGDATE + "' AND  FKEY='F2' AND VERIFY='IN') WHERE ROWCOUNT=1"); //計算當天最早刷卡時間


                        OverOUTTIME_day = o_DBFactory.ABC_toTest.vExecSQL("SELECT LOGTIME FROM (SELECT LOGTIME,ROW_NUMBER() OVER(ORDER BY LOGTIME DESC) AS ROWCOUNT FROM C_CARDHISTORY_NEW WHERE dbo.SUBSTR(USERNAME,0,10)='" + MZ_ID + "' AND LOGDATE='" + LOGDATE + "' AND FKEY='F2' AND VERIFY='IN') WHERE ROWCOUNT=1"); //計算當天最晚刷卡時間

                        ltime_day = o_DBFactory.ABC_toTest.vExecSQL("SELECT LOGTIME FROM (SELECT LOGTIME,ROW_NUMBER() OVER(ORDER BY LOGTIME) AS ROWCOUNT FROM C_CARDHISTORY_NEW WHERE dbo.SUBSTR(USERNAME,0,10)='" + MZ_ID + "' AND LOGDATE='" + lDATE + "' AND  FKEY='F2' AND VERIFY='IN') WHERE ROWCOUNT=1");//計算隔天最早刷卡時間



                        //if (!string.IsNullOrEmpty(OverOUTTIME_day))
                        //{
                        //    OverOUTTIME_day = OverOUTTIME_day.Substring(0, 5);
                        //}

                        //if (!string.IsNullOrEmpty(OverINTIME_day))
                        //{
                        //    OverINTIME_day = OverINTIME_day.Substring(0, 5);
                        //}

                        //if (!string.IsNullOrEmpty(ltime_day))
                        //{
                        //    ltime_day = ltime_day.Substring(0, 5);

                        //}

                        //if (!string.IsNullOrEmpty(ftime_day))
                        //{
                        //    ftime_day = ftime_day.Substring(0, 5);
                        //}

                        //判斷當天是否有異常刷卡
                        if (OverOUTTIME_day != "" && OverINTIME_day != "")
                        {
                            DateTime TOverOUTTIME_day = DateTime.Parse(LOGDATE + " " + OverOUTTIME_day);
                            DateTime TOverIntime_day = DateTime.Parse(LOGDATE + " " + OverINTIME_day);
                            TimeSpan T_allday_day = TOverOUTTIME_day - TOverIntime_day;
                            if (T_allday_day.TotalMinutes < 10)
                            {
                                OverOUTTIME_day = OverINTIME_day;
                            }
                        }
                        if (ftime_day == "" && ltime_day == "" && OverINTIME_day != "" && OverOUTTIME_day != "" && OverINTIME_day != OverOUTTIME_day) //只有當天有資料
                        {
                            Overdata_day.Insert(0, OverINTIME_day);
                            Overdata_day.Insert(1, OverOUTTIME_day);
                        }
                        else if (OverINTIME_day == OverOUTTIME_day && OverOUTTIME_day != "" && ltime_day != "") //當天只有一筆資料 隔天有資料
                        {
                            Overdata_day.Insert(0, OverOUTTIME_day);
                            Overdata_day.Insert(1, lDate + "日" + ltime_day);
                        }
                        else if (OverINTIME_day == OverOUTTIME_day && OverOUTTIME_day != "" && ftime_day != "" && diffdutydetail.Rows.Count == 0) //當天只有一筆資料 前一天有資料 只在第一天作用
                        {
                            Overdata_day.Insert(0, fDate + "日" + ftime);
                            Overdata_day.Insert(1, OverINTIME_day);
                        }
                        else if (ftime_day == "" && ltime_day != "" && OverINTIME_day != "" && OverOUTTIME_day != "" && OverINTIME_day != OverOUTTIME_day) //當天有兩筆資料，隔天也有
                        {
                            Overdata_day.Insert(0, OverINTIME_day);
                            Overdata_day.Insert(1, OverOUTTIME_day);
                        }
                        else if (ftime_day != "" && ltime_day == "" && OverINTIME_day != "" && OverOUTTIME_day != "" && OverINTIME_day != OverOUTTIME_day) //當天有兩筆資料，前天也有
                        {
                            Overdata_day.Insert(0, OverINTIME_day);
                            Overdata_day.Insert(1, OverOUTTIME_day);
                        }
                        else if (OverOUTTIME_day != "" && OverINTIME_day != "" && ftime_day != "" && ltime_day != "" && OverOUTTIME_day != OverINTIME_day)//四筆都有資料
                        {
                            if (diffdutydetail.Rows.Count > 0)
                            {
                                string Time_compare = startTime(OverOUTTIME_day, diffdutydetail, "值日"); //先判斷最後一筆資料有沒有重複
                                DateTime Time_compare_t = DateTime.Parse(Time_compare);
                                DateTime OverOUTTIME_day_t = DateTime.Parse(OverOUTTIME_day);
                                TimeSpan t = Time_compare_t - OverOUTTIME_day_t;

                                if (t.TotalHours < 1)
                                {
                                    Overdata_day.Insert(0, OverINTIME_day);
                                    Overdata_day.Insert(1, OverOUTTIME_day);
                                }
                                else
                                {
                                    Overdata_day.Insert(0, OverOUTTIME_day);
                                    Overdata_day.Insert(1, lDate + "日" + ltime_day);
                                }
                            }
                            else
                            {
                                Overdata_day.Insert(0, OverOUTTIME_day);
                                Overdata_day.Insert(1, lDate + "日" + ltime_day);
                            }

                        }



                        else if (OverOUTTIME_day == OverINTIME_day && OverINTIME_day != "")
                        {
                            Overdata_day.Insert(0, OverINTIME_day);
                            Overdata_day.Insert(1, "");
                        }

                        string againtime = "";
                        //bool kind_day = false;
                        //string fday="";

                        //判斷當天起迄的值班
                        string sql_day = @"select TIME_SATRT , TIME_END from  C_ONDUTY_DAY where  IS_CHK='Y' and
 MZ_ID='" + MZ_ID + "' and TIME_SATRT>=dbo.TO_CHAR(dbo.TO_DATE('" + LOGDATE + "','YYYY/MM/DD'))and TIME_SATRT<=dbo.TO_CHAR(dbo.TO_DATE('" + lDATE + "','YYYY/MM/DD'))";

                        string dt_start_OverINTIME_day = "", dt_end_OverINTIME_day = "";//上下班系統統計時間

                        DateTime INTIME, OUTTIME;
                        DataTable dt_day = new DataTable();
                        dt_day = o_DBFactory.ABC_toTest.Create_Table(sql_day, "get");
                        if (dt_day.Rows.Count > 0)
                        {
                            //dt_start_OverINTIME_day = dt_day.Rows[0]["TIME_SATRT"].ToString();
                            //dt_end_OverINTIME_day = dt_day.Rows[0]["TIME_END"].ToString();
                            INTIME = DateTime.Parse(dt_day.Rows[0]["TIME_SATRT"].ToString());
                            dt_start_OverINTIME_day = INTIME.ToString("HH:mm");
                            OUTTIME = DateTime.Parse(dt_day.Rows[0]["TIME_END"].ToString());

                            if (OUTTIME.ToString("yyyy/MM/dd") == LOGDATE)
                            {
                                dt_end_OverINTIME_day = OUTTIME.ToString("HH:mm");
                            }
                            else if (OUTTIME.ToString("yyyy/MM/dd") == lDATE)
                            {
                                dt_end_OverINTIME_day = lDATE.Substring(8, 2) + "日" + OUTTIME.ToString("HH:mm");
                            }

                            //指紋時間與系統時間比對
                            if (dt_start_OverINTIME_day != "" && dt_end_OverINTIME_day != "")
                            {
                                if (Overdata_day.Count == 2)
                                {
                                    if (Overdata_day[0] != "")
                                    {
                                        TimeSpan TS;
                                        DateTime TS1;
                                        if (Overdata_day[0].Contains("日")) //有包含日的話則為前一天
                                        {
                                            TS1 = DateTime.Parse(fDATE + " " + Overdata_day[0]);//起

                                            TS = TS1 - INTIME;

                                            if (TS.TotalHours > 1)
                                            {
                                                Overdata_day[0] = "";
                                            }
                                        }
                                        else
                                        {
                                            TS1 = DateTime.Parse(LOGDATE + " " + Overdata_day[0]);//起

                                            TS = TS1 - INTIME;

                                            if (TS.TotalHours > 1)
                                            {
                                                Overdata_day[0] = "";
                                            }

                                        }


                                    }
                                    if (Overdata_day[1] != "")
                                    {
                                        DateTime TS2; TimeSpan TS;
                                        if (Overdata_day[1].Contains("日")) //有包含日的話則為後一天
                                        {
                                            TS2 = DateTime.Parse(lDATE + " " + Overdata_day[1]);//迄
                                            TS = TS2 - OUTTIME;
                                            if (TS.TotalHours > 1)
                                            {
                                                Overdata_day[1] = "";

                                            }
                                        }
                                        else
                                        {
                                            TS2 = DateTime.Parse(LOGDATE + " " + Overdata_day[1]);//迄
                                            TS = TS2 - OUTTIME;
                                            if (TS.TotalHours > 1)
                                            {
                                                Overdata_day[1] = "";

                                            }

                                        }



                                    }

                                    if (diffdutydetail.Rows.Count > 0)
                                    {
                                        againtime = startTime(Overdata_day[0], diffdutydetail, "值日");//判斷前一天的迄和今天的起是否相同
                                    }
                                    if (againtime == Overdata_day[0])
                                    {
                                        selectINTIME_DAY = "";
                                        selectOUTTIME_DAY = "";

                                    }
                                    else
                                    {
                                        if (Overdata_day[0].Length != 0)
                                        {
                                            selectINTIME_DAY = Overdata_day[0].Substring(0, 5);
                                        }
                                        selectOUTTIME_DAY = Overdata_day[1];

                                        if (Overdata_day[0].Length == 8)
                                        {
                                            selectINTIME_DAY = Left(Overdata_day[0], 5);

                                        }
                                        else if (Overdata_day[0].Length == 11)
                                        {
                                            selectINTIME_DAY = Left(Overdata_day[0], 8);
                                            KIND += " 值日跨日";

                                        }

                                        if (Overdata_day[1].Length == 8)
                                        {
                                            selectOUTTIME_DAY = Left(Overdata_day[1], 5);
                                            day_second = Right(Overdata_day[1], 3);
                                        }
                                        else if (Overdata_day[1].Length == 11)
                                        {
                                            selectOUTTIME_DAY = Left(Overdata_day[1], 8);
                                            day_second = Right(Overdata_day[1], 3);
                                            KIND += " 值日跨日";

                                        }

                                        //if (Overdata_day[1].Length > 5 || Overdata_day[0].Length > 5)
                                        //{
                                        //    KIND += " 值日跨日";
                                        //}
                                        if (Overdata_day[0] != "" && Overdata_day[1] == "")
                                        {
                                            KIND += " 值日異常";
                                        }
                                    }

                                }
                                else if (Overdata_day.Count == 0)
                                {
                                    KIND += " 值日異常";
                                }

                            }
                        }
                        else
                        {
                            if (Overdata_day.Count == 2)
                            {
                                if (diffdutydetail.Rows.Count > 0)
                                {
                                    againtime = startTime(Overdata_day[0], diffdutydetail, "值日");//判斷前一天的迄和今天的起是否相同
                                }
                                if (againtime == Overdata_day[0])
                                {
                                    selectINTIME_DAY = "";
                                    selectOUTTIME_DAY = "";

                                }
                                else
                                {
                                    if (Overdata_day[0].Length != 0)
                                    {
                                        selectINTIME_DAY = Overdata_day[0].Substring(0, 5);
                                    }
                                    selectOUTTIME_DAY = Overdata_day[1];

                                    if (Overdata_day[0].Length == 8)
                                    {
                                        selectINTIME_DAY = Left(Overdata_day[0], 5);

                                    }
                                    else if (Overdata_day[0].Length == 11)
                                    {
                                        selectINTIME_DAY = Left(Overdata_day[0], 8);
                                        KIND += " 值日跨日";

                                    }

                                    if (Overdata_day[1].Length == 8)
                                    {
                                        selectOUTTIME_DAY = Left(Overdata_day[1], 5);
                                        day_second = Right(Overdata_day[1], 3);
                                    }
                                    else if (Overdata_day[1].Length == 11)
                                    {
                                        selectOUTTIME_DAY = Left(Overdata_day[1], 8);
                                        day_second = Right(Overdata_day[1], 3);
                                        KIND += " 值日跨日";

                                    }
                                    if (Overdata_day[0] != "" && Overdata_day[1] == "")
                                    {
                                        KIND += " 值日異常";
                                    }
                                }
                            }
                        }

                        //沒有比對系統時間
                        //if (Overdata_day.Count == 2)
                        //{
                        //    if (diffdutydetail.Rows.Count != 0)
                        //    {
                        //        againtime = startTime(Overdata_day[0], diffdutydetail, "值日");//判斷前一天的迄和今天的起是否相同
                        //        kind_day = adkind(diffdutydetail, "值日"); //判斷前一天是否有刷卡異常
                        //        fday = fday_time(diffdutydetail);
                        //        if (fday != "")
                        //        {
                        //            fday = int.Parse(fday.Substring(0, 3)) + 1911 + fday.Substring(3, 6);
                        //        }
                        //    }
                        //    if (againtime == Overdata_day[0] && kind_day == false && fday == fDATE) //againtime->前一天的迄 Overdata_day[0]->當天的起
                        //    {
                        //        selectINTIME_DAY = "";
                        //        selectOUTTIME_DAY = "";

                        //    }
                        //    else if (againtime != Overdata_day[0] && kind_day == false)
                        //    {
                        //        selectINTIME_DAY = Overdata_day[0];
                        //        selectOUTTIME_DAY = Overdata_day[1];
                        //    }
                        //    else
                        //    {
                        //        selectINTIME_DAY = Overdata_day[0];
                        //        selectOUTTIME_DAY = Overdata_day[1];
                        //    }

                        //    if (Overdata_day[1].Length > 5 || Overdata_day[0].Length > 5)
                        //    {
                        //        KIND += " 值日跨日";
                        //    }
                        //    if (selectINTIME_DAY != "" && selectOUTTIME_DAY == "")
                        //    {
                        //        KIND += "值日異常";
                        //    }
                        //}


                    }
                    #endregion 值日
                    #region 其他 含請假 上班異常 遲到...等

                    else//全部
                    {
                        int INTIME = 90100, OUTTIME = 170000;

                        if (DATE >= Convert.ToDateTime("2021/05/17") && DATE <= Convert.ToDateTime("2021/07/26"))
                        {
                            INTIME = 100100;
                            OUTTIME = 160000;
                        }

                        if (DATE >= Convert.ToDateTime("2021/07/27") && DATE <= Convert.ToDateTime("2022/12/31"))
                        {
                            INTIME = 093100;
                            OUTTIME = 163000;
                        }

                        //matthew 新增 因公未刷卡 如果TID = 99 就是因公 還要看是上班還是下班目前沒判斷 警局也沒要求 所以先不改
                        C_CARD_EDIT = o_DBFactory.ABC_toTest.vExecSQL("SELECT TID FROM C_CARDHISTORY_NEW WHERE dbo.SUBSTR(USERNAME,0,10)='" + MZ_ID + "'" + "AND LOGDATE='" + LOGDATE + "'" + "AND TID ='99'");
                        //C_CARD_EDIT = o_DBFactory.ABC_toTest.vExecSQL("SELECT TID FROM (SELECT TID,LOGTIME,ROW_NUMBER() OVER(ORDER BY LOGTIME) AS ROWCOUNT FROM C_CARDHISTORY_NEW WHERE dbo.SUBSTR(USERNAME,0,10)='" + MZ_ID + "' AND LOGDATE='" + LOGDATE + "' AND VERIFY='IN') WHERE ROWCOUNT=1");
                        //雖然說為什麼要進出資料庫找兩次.但找起來還算快.就先不改
                        selectINTIME = o_DBFactory.ABC_toTest.vExecSQL("SELECT LOGTIME FROM (SELECT LOGTIME,ROW_NUMBER() OVER(ORDER BY LOGTIME) AS ROWCOUNT FROM C_CARDHISTORY_NEW WHERE dbo.SUBSTR(USERNAME,0,10)='" + MZ_ID + "' AND LOGDATE='" + LOGDATE + "' AND VERIFY='IN') WHERE ROWCOUNT=1");
                        if (selectINTIME.Length > 0 && selectINTIME.Substring(0, 3) == "24:")
                        {
                            selectINTIME = "00:" + selectINTIME.Substring(3, 5);
                        }

                        selectOUTTIME = o_DBFactory.ABC_toTest.vExecSQL("SELECT LOGTIME FROM (SELECT LOGTIME,ROW_NUMBER() OVER(ORDER BY LOGTIME DESC) AS ROWCOUNT FROM C_CARDHISTORY_NEW WHERE dbo.SUBSTR(USERNAME,0,10)='" + MZ_ID.Trim() + "' AND LOGDATE='" + LOGDATE + "' AND VERIFY='IN') WHERE ROWCOUNT=1");
                        if (selectOUTTIME.Length > 0 && selectOUTTIME.Substring(0, 3) == "24:")
                        {
                            selectOUTTIME = "00:" + selectOUTTIME.Substring(3, 5);
                        }
                        //雖然說為什麼要進出資料庫找兩次找.但找起來還算快.就先不改

                        //取得免刷卡人員做判斷 20190718 by sky
                        string noSwipe = o_DBFactory.ABC_toTest.vExecSQL(string.Format("SELECT MZ_CLOCK FROM C_CARDSET WHERE MZ_ID = '{0}' ", MZ_ID.Trim()));

                        string qdate = (DATE.Year - 1911).ToString().PadLeft(3, '0') + DATE.Month.ToString().PadLeft(2, '0') + DATE.Day.ToString().PadLeft(2, '0');
                        //抓取補班日的資料?
                        string C_SPRINGDAY = o_DBFactory.ABC_toTest.vExecSQL(string.Format("SELECT mz_spring_name FROM C_SPRINGDAY WHERE mz_spring_date = '{0}' ", qdate));

                        if (string.IsNullOrEmpty(selectINTIME) && string.IsNullOrEmpty(selectOUTTIME))//如果當天都沒刷卡紀錄
                        {
                            //如果 當天是六日 且 不是補班
                            if ((DateTime.Parse(LOGDATE).DayOfWeek == DayOfWeek.Saturday || DateTime.Parse(LOGDATE).DayOfWeek == DayOfWeek.Sunday) && C_SPRINGDAY == "")
                            {
                                MEMO = "例假日";
                            }
                            else if (noSwipe == "N")
                            {
                                //免刷卡人員不進行判斷
                            }
                            else if (lstDayOff.Count == 0) ///無刷卡狀態
                            {
                                if (DATE.Year.ToString() + DATE.Month.ToString() + DATE.Day.ToString() == DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString())
                                {
                                    int NOWTIME = int.Parse(DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0'));

                                    if (NOWTIME <= INTIME)///本日九點前
                                    {
                                        KIND = "";
                                        MEMO = "";
                                    }
                                    else if (NOWTIME > INTIME)///本日九點後
                                    {
                                        KIND = "上班未刷卡";
                                        MEMO = "上班異常";
                                    }
                                }
                                else///過本日後
                                {
                                    KIND = "未刷卡";
                                    MEMO = "上班異常";
                                }
                            }
                            else
                            {
                                if (lstDayOff.Count > 1)///有請假
                                {
                                    CODE = lstDayOff[0];

                                    if (int.Parse(LOGDATE1) >= int.Parse(lstDayOff[1]) && int.Parse(LOGDATE1) <= int.Parse(lstDayOff[3]) && int.Parse(lstDayOff[6]) > 0)///正常
                                    {
                                        KIND = "";
                                        MEMO = "請假";
                                    }
                                    else
                                    {
                                        KIND = "未刷卡";
                                        MEMO = "上班異常";
                                    }
                                }
                                else if (lstDayOff.Count == 1)
                                {
                                    KIND = "";
                                    MEMO = "國定假日";
                                }
                            }
                            if (C_CARD_EDIT == "99")
                            {
                                MEMO = "因公疏未辦理刷卡`";
                            }
                        }
                        else//如果當天有刷卡紀錄
                        {
                            if (DateTime.Parse(LOGDATE).DayOfWeek == DayOfWeek.Saturday || DateTime.Parse(LOGDATE).DayOfWeek == DayOfWeek.Sunday)
                            {
                                MEMO = "例假日";
                            }
                            else if (noSwipe == "N")
                            {
                                //免刷卡人員不進行判斷
                            }
                            else if (DATE.Year.ToString() + DATE.Month.ToString() + DATE.Day.ToString() == DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString())///如果是執行此動作當天當日
                            {
                                if (int.Parse(selectINTIME.Replace(":", string.Empty)) <= INTIME)///當日刷9點前的卡
                                {
                                    if (selectINTIME == selectOUTTIME && lstDayOff.Count == 0)///只有一筆刷卡資料而且無請假
                                    {
                                        KIND = "";
                                        MEMO = "";
                                        selectOUTTIME = "";
                                    }
                                    else if (selectINTIME == selectOUTTIME && lstDayOff.Count > 1)///只有一筆刷卡資料而且有請假
                                    {
                                        selectOUTTIME = "";

                                        CODE = lstDayOff[0];

                                        DateTime TS1 = DateTime.Parse(LOGDATE + " " + lstDayOff[2] + ":00");///跟請假起時比

                                        if (DateTime.Now > TS1)///超過就算下班未刷卡
                                        {
                                            selectOUTTIME = "";
                                            KIND = "下班未刷卡";
                                            MEMO = "上班異常";
                                        }

                                    }
                                    else if (int.Parse(selectOUTTIME.Replace(":", string.Empty)) < INTIME)
                                    {
                                        KIND = "";
                                        MEMO = "";
                                        selectOUTTIME = "";
                                    }
                                    else if (int.Parse(selectOUTTIME.Replace(":", string.Empty)) < OUTTIME)///下班時間少於17點
                                    {
                                        if (lstDayOff.Count > 1)///有請假
                                        {
                                            CODE = lstDayOff[0];

                                            DateTime TS1 = DateTime.Parse(LOGDATE + " " + selectINTIME);

                                            DateTime TS2 = DateTime.Parse(LOGDATE + " " + selectOUTTIME);

                                            TimeSpan TS = TS2 - TS1;

                                            if (TS.Hours + int.Parse(lstDayOff[6]) >= 8)
                                            {
                                                MEMO = "請假";
                                            }
                                            else
                                            {
                                                KIND = "早退";
                                                MEMO = "請假早退";
                                            }
                                        }
                                        else if (lstDayOff.Count == 1)
                                        {
                                            KIND = "";
                                            MEMO = "國定假日";
                                        }
                                        else
                                        {
                                            KIND = "早退";
                                            MEMO = "上班異常";
                                        }
                                    }
                                    else ///下班時間大於17點
                                    {
                                        if (lstDayOff.Count > 1)
                                        {
                                            CODE = lstDayOff[0];
                                        }

                                        DateTime TS1 = DateTime.Parse(LOGDATE + " " + selectINTIME);

                                        DateTime TS2 = DateTime.Parse(LOGDATE + " " + selectOUTTIME);

                                        TimeSpan TS = TS2 - TS1;

                                        if (TS.Hours > 8)
                                        {
                                            MEMO = "";
                                        }
                                        else if (lstDayOff.Count == 1)
                                        {
                                            KIND = "";
                                            MEMO = "國定假日";
                                        }
                                        else
                                        {
                                            KIND = "早退";
                                            MEMO = "上班異常";
                                        }
                                    }
                                }
                                else if (int.Parse(selectINTIME.Replace(":", string.Empty)) > INTIME)///當天刷卡時間大於9點1分
                                {
                                    if (selectINTIME == selectOUTTIME && lstDayOff.Count == 0)///只有一筆刷卡紀錄
                                    {
                                        if (int.Parse(selectINTIME.Replace(":", string.Empty)) > OUTTIME)///記錄大於17點
                                        {
                                            KIND = "上班未刷卡";
                                            MEMO = "上班異常";
                                            selectINTIME = "";
                                        }
                                        else
                                        {
                                            KIND = "遲到";
                                            MEMO = "上班異常";
                                            selectOUTTIME = "";
                                        }
                                    }
                                    else if (selectINTIME == selectOUTTIME && lstDayOff.Count > 1)///只有一筆刷卡紀錄有請假
                                    {
                                        CODE = lstDayOff[0];

                                        if (int.Parse(lstDayOff[2].Replace(":", "")) <= 900 && int.Parse(lstDayOff[4].Replace(":", "")) <= 1330)
                                        {
                                            if (int.Parse(selectINTIME.Replace(":", string.Empty)) < 133000)///未超過1330刷卡
                                            {
                                                KIND = "";
                                                MEMO = "";
                                                selectOUTTIME = "";
                                            }
                                            else///超過1330
                                            {
                                                KIND = "遲到";
                                                MEMO = "上班異常";
                                                selectOUTTIME = "";
                                            }
                                        }
                                        else if (int.Parse(lstDayOff[2].Replace(":", "")) <= 900)
                                        {
                                            if (int.Parse(selectINTIME.Replace(":", string.Empty)) < int.Parse(lstDayOff[4].Replace(":", string.Empty) + "00"))
                                            {
                                                if (int.Parse(selectINTIME.Replace(":", string.Empty)) > 173000)
                                                {
                                                    KIND = "上班未刷卡";
                                                    MEMO = "上班異常";
                                                    selectINTIME = "";
                                                }
                                                else
                                                {
                                                    KIND = "下班未刷卡";
                                                    MEMO = "上班異常";
                                                    selectOUTTIME = "";
                                                }
                                            }
                                            else
                                            {
                                                KIND = "遲到";
                                                MEMO = "上班異常";
                                                selectOUTTIME = "";
                                            }
                                        }
                                        else
                                        {
                                            KIND = "遲到";
                                            MEMO = "上班異常";
                                            selectOUTTIME = "";
                                        }
                                    }
                                    else if (int.Parse(selectOUTTIME.Replace(":", string.Empty)) < 180000)
                                    {
                                        if (lstDayOff.Count > 1)
                                        {
                                            CODE = lstDayOff[0];

                                            DateTime TS1 = DateTime.Parse(LOGDATE + " " + lstDayOff[2] + ":00");

                                            DateTime TS2 = DateTime.Parse(LOGDATE + " " + selectOUTTIME);

                                            TimeSpan TS = TS1 - TS2;

                                            if (TS.Hours + int.Parse(lstDayOff[6]) < 8)
                                            {
                                                KIND = "遲到";
                                                MEMO = "上班異常";
                                            }
                                            else
                                            {
                                                KIND = "遲到早退";
                                                MEMO = "上班異常";
                                            }
                                        }
                                        else
                                        {
                                            KIND = "遲到早退";
                                            MEMO = "上班異常";
                                        }
                                    }
                                    else
                                    {
                                        if (lstDayOff.Count > 1)
                                        {
                                            CODE = lstDayOff[0];

                                            MEMO = "";
                                        }
                                        else
                                        {
                                            KIND = "遲到";
                                            MEMO = "上班異常";
                                        }
                                    }

                                }
                            }
                            else
                            {
                                //取得時間差
                                DateTime dt1 = DateTime.Parse(LOGDATE + " " + selectINTIME); //刷卡時間(上班)
                                DateTime dt2 = DateTime.Parse(LOGDATE + " " + selectOUTTIME); //刷卡時間(下班)，若當日無下班刷卡紀錄則會跟上班時間相同
                                TimeSpan dt_diff = dt2 - dt1; //時間差


                                if (int.Parse(selectINTIME.Replace(":", string.Empty)) <= INTIME)///非當日刷卡時間未超過9點1分
                                {
                                    if (selectINTIME == selectOUTTIME && lstDayOff.Count == 0)///只有一筆刷卡資料且未請假
                                    {
                                        KIND = "下班未刷卡";
                                        //MEMO = ""; //20151209 Neil: 小隊長建議移除備註欄即可
                                        MEMO = "上班異常";//20160112 JACK: 1041218 江杰昊 異常報表查不到資料 memo為空的問題
                                        selectOUTTIME = "";
                                    }
                                    else if (selectINTIME == selectOUTTIME && lstDayOff.Count > 0) ///只有一筆刷卡資料且有請假
                                    {
                                        KIND = "下班未刷卡"; //事實上仍未刷卡故顯示
                                        MEMO = "請假"; //但備註欄補上請假資訊
                                        selectOUTTIME = "";
                                    }
                                    ///20151209 Neil : 此部分原先的設計共有兩種情況會進入下面的 else if :
                                    /// 1. 有人下午請假，但忘記刷下班卡
                                    /// 2. 有人平時忘記打下班卡，只好用補請假的方式去補回
                                    ///但此兩種情況均不符合原先 if 裡面的設計(下班未刷、上班異常)
                                    /// 
                                    /// 因此補上條件 selectINTIME != selectOUTTIME，用於判斷短時間內上下班，且早於 09:01
                                    /// 此 Else 適用情況 : 
                                    /// 若當日有正常上下班刷卡，但下班刷卡時間早於 09:01
                                    else if (selectINTIME != selectOUTTIME && int.Parse(selectOUTTIME.Replace(":", string.Empty)) < INTIME)///多筆資料都在九點前
                                    {
                                        if (dt_diff.Hours >= 1)
                                        {
                                            if (lstDayOff.Count == 0) //無請假
                                            {
                                                KIND = "早退";
                                                MEMO = "上班異常";
                                                selectOUTTIME = "";
                                            }
                                            else if (lstDayOff.Count == 1)
                                            {
                                                KIND = "";
                                                MEMO = "國定假日";
                                            }
                                            else
                                            {
                                                if (lstDayOff[4].ToString().IsNormalized())
                                                {
                                                    KIND = "";
                                                    MEMO = lstDayOff[0].ToString();
                                                }
                                                else
                                                {
                                                    string strETime = lstDayOff[4].ToString().Substring(0, 2) + lstDayOff[4].ToString().Substring(3, 2);
                                                    int iETime = int.Parse(strETime);

                                                    if (iETime < 1330)
                                                    {
                                                        KIND = "";
                                                        MEMO = "早上請假";
                                                    }
                                                    else
                                                    {
                                                        KIND = "";
                                                        MEMO = lstDayOff[0].ToString();
                                                    }
                                                }

                                            }
                                        }
                                        else
                                        {
                                            KIND = "早退";
                                            MEMO = "上班異常";
                                            selectOUTTIME = "";
                                        }


                                    }
                                    else if (int.Parse(selectOUTTIME.Replace(":", string.Empty)) < OUTTIME)///下班刷卡時間早於5點
                                    {
                                        if (lstDayOff.Count > 1) ///有請假
                                        {
                                            CODE = lstDayOff[0];

                                            DateTime TS1 = DateTime.Parse(LOGDATE + " " + selectINTIME);

                                            DateTime TS2 = DateTime.Parse(LOGDATE + " " + selectOUTTIME);

                                            TimeSpan TS = TS2 - TS1;

                                            if (TS.Hours + int.Parse(lstDayOff[6]) >= 8)
                                            {
                                                MEMO = "下午請假";
                                            }
                                            else
                                            {
                                                KIND = "早退";
                                                MEMO = "上班異常";
                                            }
                                        }
                                        else if (lstDayOff.Count == 1)
                                        {
                                            KIND = "";
                                            MEMO = "國定假日";
                                        }
                                        else
                                        {
                                            KIND = "早退";
                                            MEMO = "上班異常";
                                        }
                                    }
                                    else
                                    {
                                        DateTime TS1 = DateTime.Parse(LOGDATE + " " + selectINTIME);
                                        DateTime TS2 = DateTime.Parse(LOGDATE + " " + selectOUTTIME);
                                        TimeSpan TS = TS2 - TS1;

                                        if (lstDayOff.Count > 1) ///有請假
                                        {
                                            if (TS.Hours + int.Parse(lstDayOff[6]) > 8)
                                            {
                                                KIND = "";
                                            }
                                            //else if (TS.Hours + int.Parse(temp[6]) == 8)
                                            //{
                                            //    KIND = "早退";
                                            //    MEMO = "請假早退";
                                            //}
                                            else
                                            {
                                                KIND = "早退";
                                                MEMO = "請假早退";
                                            }
                                        }
                                        else
                                        {
                                            if (TS.Hours > 8)
                                            {
                                                MEMO = "";
                                            }
                                            else
                                            {
                                                KIND = "早退";
                                                MEMO = "上班異常";
                                            }
                                        }

                                    }
                                }
                                else if (int.Parse(selectINTIME.Replace(":", string.Empty)) > INTIME)///上班刷卡時間大於9點1分
                                {
                                    if (selectINTIME == selectOUTTIME && lstDayOff.Count == 0)///只一筆刷卡資料
                                    {
                                        if (int.Parse(selectINTIME.Replace(":", string.Empty)) > OUTTIME)
                                        {
                                            KIND = "上班未刷卡";
                                            MEMO = "上班異常";
                                            selectINTIME = "";
                                        }
                                        else
                                        {
                                            KIND = "遲到下班未刷卡";
                                            MEMO = "上班異常";
                                            selectINTIME = "";
                                        }
                                    }
                                    else if (selectINTIME == selectOUTTIME && lstDayOff.Count > 1)///只一筆有請假
                                    {
                                        CODE = lstDayOff[0];

                                        if (lstDayOff[2] == "08:30" && lstDayOff[4] == "12:30")
                                        {

                                            if (int.Parse(selectINTIME.Replace(":", string.Empty)) < 133000)
                                            {
                                                KIND = "下班未刷卡";
                                                MEMO = "上班異常";
                                                selectOUTTIME = "";
                                            }
                                            else if (int.Parse(selectINTIME.Replace(":", string.Empty)) >= 173000)
                                            {
                                                KIND = "上班未刷卡";
                                                MEMO = "請假";
                                                selectINTIME = "";
                                            }
                                            else
                                            {
                                                KIND = "遲到下班未刷卡";
                                                MEMO = "上班異常";
                                                selectOUTTIME = "";
                                            }

                                        }
                                        else if (int.Parse(lstDayOff[2].Replace(":", "")) <= 900)
                                        {
                                            if (int.Parse(selectINTIME.Replace(":", string.Empty)) < int.Parse(lstDayOff[4].Replace(":", string.Empty) + "00"))
                                            {
                                                KIND = "下班未刷卡";
                                                MEMO = "上班異常";
                                                selectOUTTIME = "";
                                            }
                                            else
                                            {
                                                KIND = "遲到下班未刷卡";
                                                MEMO = "上班異常";
                                                selectOUTTIME = "";
                                            }
                                        }
                                        else
                                        {
                                            KIND = "遲到下班未刷卡";
                                            MEMO = "上班異常";
                                            selectOUTTIME = "";
                                        }
                                    }
                                    else if (int.Parse(selectOUTTIME.Replace(":", string.Empty)) < 180000)
                                    {
                                        if (lstDayOff.Count > 1)
                                        {
                                            CODE = lstDayOff[0];

                                            DateTime TS1 = DateTime.Parse(LOGDATE + " " + selectINTIME);

                                            DateTime TS2 = DateTime.Parse(LOGDATE + " " + selectOUTTIME);

                                            TimeSpan TS = TS2 - TS1;

                                            if (TS.Hours + int.Parse(lstDayOff[6]) >= 8)
                                            {
                                                KIND = "";
                                                MEMO = "早上請假";
                                            }
                                            else
                                            {
                                                KIND = "遲到早退";
                                                // Joy 當天有請假紀錄，則不出現上班異常備註，新需求要加註請假類別。
                                                // MEMO = "上班異常";  
                                                if (!string.IsNullOrEmpty(CODE))
                                                {
                                                    MEMO = CODE;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            KIND = "遲到早退";
                                            MEMO = "上班異常";
                                        }
                                    }
                                    else
                                    {
                                        if (lstDayOff.Count > 1)
                                        {
                                            CODE = lstDayOff[0];

                                            MEMO = "正常";
                                        }
                                        else
                                        {
                                            KIND = "遲到";
                                            MEMO = "上班異常";
                                        }
                                    }
                                }
                                else if (string.IsNullOrEmpty(selectINTIME))
                                {
                                    if (int.Parse(selectOUTTIME.Replace(":", string.Empty)) < OUTTIME)
                                    {
                                        KIND = "遲到早退上班未刷卡";

                                        MEMO = "上班異常";
                                    }
                                    else
                                    {
                                        KIND = "遲到上班未刷卡";

                                        MEMO = "上班異常";
                                    }
                                }
                            }
                            if (C_CARD_EDIT == "99")
                            {
                                MEMO = "因公疏未辦理刷卡";
                            }
                        }
                    }
                    #endregion 其他 含請假 上班異常 遲到...等

                    DataRow dr = diffdutydetail.NewRow();
                    if (type == "1")//CheckBox1.Checked//異常報表
                    {
                        if (MEMO == "上班異常")
                        {
                            dr["UNIT"] = punit;
                            dr["OCCC"] = occc;
                            dr["NAME"] = name;
                            dr["LOGDATE"] = int.Parse(DateManange.datetostr(LOGDATE).Substring(0, 3)).ToString() + "/" + DateManange.datetostr(LOGDATE).Substring(3, 2) + "/" + DateManange.datetostr(LOGDATE).Substring(5, 2);
                            dr["INTIME"] = selectINTIME;
                            dr["OUTTIME"] = selectOUTTIME;
                            dr["KIND"] = KIND;
                            dr["MEMO"] = MEMO;

                            return dr;
                        }
                        else
                        {
                            return dr;
                        }
                    }
                    else if (type == "2")//CheckBox2.Checked//加班報表
                    {
                        if (string.IsNullOrEmpty(selectINTIME))
                        {
                            return dr;
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(selectOUTTIME))
                            {
                                return dr;
                            }
                            else
                            {

                                dr["UNIT"] = punit;
                                dr["OCCC"] = occc;
                                dr["NAME"] = name;
                                dr["LOGDATE"] = int.Parse(DateManange.datetostr(LOGDATE).Substring(0, 3)).ToString() + "/" + DateManange.datetostr(LOGDATE).Substring(3, 2) + "/" + DateManange.datetostr(LOGDATE).Substring(5, 2); ;
                                dr["INTIME"] = selectINTIME;
                                dr["OUTTIME"] = selectOUTTIME;
                                dr["KIND"] = KIND;
                                dr["MEMO"] = MEMO;
                                return dr;
                            }
                        }
                    }
                    else if (type == "3") //超勤部分
                    {
                        if (string.IsNullOrEmpty(selectINTIME_over) && KIND.Contains("超勤異常") != true)
                        {
                            return dr;
                        }

                        else
                        {
                            dr["UNIT"] = punit;
                            dr["OCCC"] = occc;
                            dr["NAME"] = name;
                            dr["LOGDATE"] = int.Parse(DateManange.datetostr(LOGDATE).Substring(0, 3)).ToString() + "/" + DateManange.datetostr(LOGDATE).Substring(3, 2) + "/" + DateManange.datetostr(LOGDATE).Substring(5, 2); ;
                            dr["INTIME"] = selectINTIME_over;
                            dr["OUTTIME"] = selectOUTTIME_over;
                            dr["KIND"] = KIND;
                            dr["MEMO"] = MEMO;
                            return dr;
                        }
                    }
                    else if (type == "4") //值日部分
                    {
                        if (string.IsNullOrEmpty(selectINTIME_DAY) && KIND.Contains("值日異常") != true)
                        {
                            return dr;
                        }
                        else
                        {
                            dr["UNIT"] = punit;
                            dr["OCCC"] = occc;
                            dr["NAME"] = name;
                            dr["LOGDATE"] = int.Parse(DateManange.datetostr(LOGDATE).Substring(0, 3)).ToString() + "/" + DateManange.datetostr(LOGDATE).Substring(3, 2) + "/" + DateManange.datetostr(LOGDATE).Substring(5, 2); ;
                            dr["INTIME"] = selectINTIME_DAY;
                            dr["OUTTIME"] = selectOUTTIME_DAY;
                            dr["KIND"] = KIND;
                            dr["MEMO"] = MEMO;
                            dr["DAYSECOND"] = day_second;
                            return dr;
                        }
                    }
                    else
                    {

                        dr["UNIT"] = punit;
                        dr["OCCC"] = occc;
                        dr["NAME"] = name;
                        dr["LOGDATE"] = int.Parse(DateManange.datetostr(LOGDATE).Substring(0, 3)).ToString() + "/" + DateManange.datetostr(LOGDATE).Substring(3, 2) + "/" + DateManange.datetostr(LOGDATE).Substring(5, 2); ;
                        dr["INTIME"] = selectINTIME;
                        dr["OUTTIME"] = selectOUTTIME;
                        dr["KIND"] = KIND;
                        if (C_CARD_EDIT == "99")
                        {
                            dr["MEMO"] = "因公疏未辦理刷卡";
                        }
                        else
                        {
                            dr["MEMO"] = MEMO;
                        }
                        return dr;
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    var result = MZ_ID + " || " + DATE + " || " + name;
                    throw;
                }
            }

            public static string doTitle(string TYPE, string EXAD)
            {
                string title = "";
                if (TYPE == "0")//CheckBox3.Checked
                {
                    title = string.Format("{0}刷卡(勤惰)明細表", EXAD);

                }
                else if (TYPE == "1")//CheckBox1.Checked//異常報表
                {
                    title = string.Format("{0}刷卡(勤惰)異常報表", EXAD);

                }
                else if (TYPE == "2")//CheckBox2.Checked//加班報表
                {
                    title = string.Format("{0}刷卡(勤惰)加班報表", EXAD);

                }
                else if (TYPE == "3")
                {
                    title = string.Format("{0}刷卡(勤惰)差勤報表", EXAD);
                }
                else if (TYPE == "4")
                {
                    title = string.Format("{0}刷卡(勤惰)值日報表", EXAD);
                }

                return title;

            }
            /// <summary>
            /// 判斷起訖時間是否有重複
            /// </summary>

            public static string startTime(string outtime, DataTable temp, string type_A)
            {
                if (type_A == "值日")
                {
                    int Num = int.Parse(temp.Rows.Count.ToString());
                    outtime = temp.Rows[Num - 1]["OUTTIME"].ToString();
                    string second_day = temp.Rows[Num - 1]["DAYSECOND"].ToString();
                    outtime = Right(outtime, 5) + second_day;
                }
                else if (type_A == "超勤")
                {
                    int Num = int.Parse(temp.Rows.Count.ToString());
                    outtime = temp.Rows[Num - 1]["OUTTIME"].ToString();
                    outtime = Right(outtime, 5);

                }

                return outtime;
            }


            /// <summary>
            /// 計算超勤系統的時間
            /// </summary>
            /// <param name="ID"></param>
            /// <param name="MODE"></param>
            /// <param name="DATE"></param>
            /// <returns></returns>
            protected static string Count(string ID, string DATE)
            {
                string result = "";

                string strSQL = "SELECT * FROM  C_DUTYTABLE_PERSONAL WHERE MZ_ID='" + ID + "' AND DUTYDATE='" + DATE + "'";

                DataTable temp_DT_OVER = new DataTable();

                temp_DT_OVER = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GETVALUE");

                int j = 0;
                int K = 0;
                string timestring = "";
                if (temp_DT_OVER.Rows.Count > 0)
                {
                    for (int i = 1; i < 27; i++)
                    {
                        j++;


                        if (temp_DT_OVER.Rows[0]["DUTYITEM" + i.ToString()].ToString() != string.Empty)
                        {
                            K++;
                            if (K == 1)
                                timestring += temp_DT_OVER.Rows[0]["TIME" + i.ToString()].ToString();
                            else
                                timestring += "." + temp_DT_OVER.Rows[0]["TIME" + i.ToString()].ToString();

                        }

                        result = timestring;



                    }
                }
                return result;
            }

            /// <summary>
            /// 判斷狀態
            /// </summary>

            public static bool adkind(DataTable temp, string type_A)
            {
                string kind = "";
                bool kindY = false;
                if (type_A == "值日")
                {
                    int Num = int.Parse(temp.Rows.Count.ToString());
                    kind = temp.Rows[Num - 1]["KIND"].ToString();
                    if (kind == "值日異常")
                    {
                        kindY = true;
                    }
                }
                else if (type_A == "超勤")
                {
                    int Num = int.Parse(temp.Rows.Count.ToString());
                    kind = temp.Rows[Num - 1]["KIND"].ToString();
                    if (kind == "超勤異常")
                    {
                        kindY = true;
                    }
                }

                return kindY;
            }

            /// <summary>
            /// 判斷是否為前一天
            /// </summary>

            public static string fday_time(DataTable temp)
            {
                string fday = string.Empty;

                int Num = int.Parse(temp.Rows.Count.ToString());
                fday = temp.Rows[Num - 1]["LOGDATE"].ToString();

                return fday;
            }


            // 取得由左數來第N位字串
            public static string Left(string sSource, int iLength)
            {
                if (sSource.Trim().Length > 0)
                    return sSource.Substring(0, iLength > sSource.Length ? sSource.Length : iLength);
                else
                    return "";

            }

            // 取得由右數來第N位字串

            public static string Right(string sSource, int iLength)
            {

                if (sSource.Trim().Length > 0)

                    return sSource.Substring(iLength > sSource.Length ? 0 : sSource.Length - iLength);

                else

                    return "";

            }

        }





        #region 加班報表

        /// <summary>
        /// 5.1 加班請示單
        /// </summary>
        public class OvertimeInsideAsk
        {
            public static DataTable doSearch(string MZ_ID, string MZ_DATE)
            {
                DataTable rptDT = new DataTable();

                rptDT.Columns.Add("BEGINTIME", typeof(string));
                rptDT.Columns.Add("ENDTIME", typeof(string));
                rptDT.Columns.Add("MZ_ID", typeof(string));
                rptDT.Columns.Add("MZ_NAME", typeof(string));
                rptDT.Columns.Add("MZ_OCCC", typeof(string));
                rptDT.Columns.Add("MZ_EXAD", typeof(string));
                rptDT.Columns.Add("MZ_EXUNIT", typeof(string));
                rptDT.Columns.Add("OTIME", typeof(string));
                rptDT.Columns.Add("HOUR_PAY", typeof(string));
                rptDT.Columns.Add("PAY_SUM", typeof(string));
                rptDT.Columns.Add("OTREASON", typeof(string));
                rptDT.Columns.Add("MZ_DATE", typeof(string));
                rptDT.Columns.Add("C1000", typeof(string));
                rptDT.Columns.Add("C100", typeof(string));
                rptDT.Columns.Add("C10", typeof(string));
                rptDT.Columns.Add("C1", typeof(string));

                string strSQL = @"
SELECT CC.MZ_ID,AA.MZ_NAME,AKO.MZ_KCHI MZ_OCCC,CC.MZ_EXAD,AKEU.MZ_KCHI MZ_EXUNIT ,OTIME,OTREASON,CC.MZ_DATE,HOUR_PAY,PAY_SUM,RESTFLAG
 FROM C_OVERTIME_HOUR_INSIDE CC
LEFT JOIN A_DLBASE AA ON CC.MZ_ID=AA.MZ_ID
LEFT JOIN A_KTYPE AKO ON AKO.MZ_KCODE=CC.MZ_OCCC AND  AKO.MZ_KTYPE='26'
LEFT JOIN  A_KTYPE AKEU ON AKEU.MZ_KCODE=CC.MZ_EXUNIT AND AKEU.MZ_KTYPE='25'

WHERE CC.MZ_ID='" + MZ_ID + "'";

                if (!string.IsNullOrEmpty(MZ_ID))
                {
                    strSQL += " AND CC.MZ_DATE='" + MZ_DATE + "'";
                }
                //if (TYPE == "1")
                //{
                //    strSQL += " AND (RESTFLAG='N' OR RESTFLAG IS NULL) ";
                //}
                DataTable tempDT = new DataTable();

                tempDT = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GETVALUE");

                for (int i = 0; i < tempDT.Rows.Count; i++)
                {
                    DataRow dr = rptDT.NewRow();
                    dr["BEGINTIME"] = ""; //o_DBFactory.ABC_toTest.vExecSQL("SELECT LOGTIME FROM (SELECT LOGTIME,ROW_NUMBER() OVER(ORDER BY LOGTIME) AS ROWCOUNT FROM C_CARDHISTORY_NEW WHERE USERID='" + tempDT.Rows[i]["MZ_ID"].ToString() + "' AND LOGDATE='" + tempDT.Rows[i]["MZ_DATE"].ToString() + "' AND FKEY='F3' ) WHERE ROWCOUNT=1"); ;
                    dr["ENDTIME"] = ""; //o_DBFactory.ABC_toTest.vExecSQL("SELECT LOGTIME FROM (SELECT LOGTIME,ROW_NUMBER() OVER(ORDER BY LOGTIME DESC) AS ROWCOUNT FROM C_CARDHISTORY_NEW WHERE USERID='" + tempDT.Rows[i]["MZ_ID"].ToString() + "' AND LOGDATE='" + tempDT.Rows[i]["MZ_DATE"].ToString() + "' AND FKEY='F3' ) WHERE ROWCOUNT=1");
                    dr["MZ_ID"] = tempDT.Rows[i]["MZ_ID"].ToString();
                    dr["MZ_NAME"] = tempDT.Rows[i]["MZ_NAME"].ToString();
                    dr["MZ_OCCC"] = tempDT.Rows[i]["MZ_OCCC"].ToString();
                    dr["MZ_EXAD"] = tempDT.Rows[i]["MZ_EXAD"].ToString();
                    dr["MZ_EXUNIT"] = tempDT.Rows[i]["MZ_EXUNIT"].ToString();
                    dr["OTIME"] = tempDT.Rows[i]["OTIME"].ToString();
                    dr["OTREASON"] = tempDT.Rows[i]["OTREASON"].ToString();
                    dr["MZ_DATE"] = tempDT.Rows[i]["MZ_DATE"].ToString();
                    dr["HOUR_PAY"] = tempDT.Rows[i]["HOUR_PAY"].ToString();
                    dr["PAY_SUM"] = tempDT.Rows[i]["PAY_SUM"].ToString();

                    if (tempDT.Rows[i]["RESTFLAG"].ToString() == "Y")
                    {

                        dr["C1000"] = "";
                        dr["C100"] = "";
                        dr["C10"] = "";
                        dr["C1"] = "";
                    }
                    else
                    {

                        if (tempDT.Rows[i]["PAY_SUM"].ToString().Length >= 3)//TODO 20140423  兩位數就不會顯示金額.不合理???
                        {
                            if (tempDT.Rows[i]["PAY_SUM"].ToString().Substring(0, (tempDT.Rows[i]["PAY_SUM"]).ToString().Length - 3).Length == 4)
                            {
                                dr["C1000"] = DateManange.toChinese(tempDT.Rows[i]["PAY_SUM"].ToString().Substring(0, 1), 2);
                                dr["C100"] = DateManange.toChinese(tempDT.Rows[i]["PAY_SUM"].ToString().Substring(1, 1), 2);
                                dr["C10"] = DateManange.toChinese(tempDT.Rows[i]["PAY_SUM"].ToString().Substring(2, 1), 2);
                                dr["C1"] = DateManange.toChinese(tempDT.Rows[i]["PAY_SUM"].ToString().Substring(3, 1), 2);

                            }
                            else if (tempDT.Rows[i]["PAY_SUM"].ToString().Substring(0, (tempDT.Rows[i]["PAY_SUM"]).ToString().Length - 3).Length == 3)
                            {


                                dr["C1000"] = "";
                                dr["C100"] = DateManange.toChinese(tempDT.Rows[i]["PAY_SUM"].ToString().Substring(0, 1), 2);
                                dr["C10"] = DateManange.toChinese(tempDT.Rows[i]["PAY_SUM"].ToString().Substring(1, 1), 2);
                                dr["C1"] = DateManange.toChinese(tempDT.Rows[i]["PAY_SUM"].ToString().Substring(2, 1), 2);
                            }
                            else if (tempDT.Rows[i]["PAY_SUM"].ToString().Substring(0, (tempDT.Rows[i]["PAY_SUM"]).ToString().Length - 3).Length == 2)
                            {

                                dr["C1000"] = "";
                                dr["C100"] = "";
                                dr["C10"] = DateManange.toChinese(tempDT.Rows[i]["PAY_SUM"].ToString().Substring(0, 1), 2);
                                dr["C1"] = DateManange.toChinese(tempDT.Rows[i]["PAY_SUM"].ToString().Substring(1, 1), 2);
                            }
                            else
                            {
                                dr["C1000"] = "";
                                dr["C100"] = "";
                                dr["C10"] = "";
                                dr["C1"] = "";
                            }
                        }
                        else
                        {
                            dr["C1000"] = "";
                            dr["C100"] = "";
                            dr["C10"] = "";
                            dr["C1"] = "";

                        }
                    }
                    rptDT.Rows.Add(dr);
                }


                return rptDT;



            }
            /// <summary>
            /// 新加班功能調整
            /// </summary>
            /// <param name="MZ_ID"></param>
            /// <param name="MZ_DATE"></param>
            /// <returns></returns>
            public static DataTable doSearch_New(string MZ_ID, string MZ_DATE)
            {
                DataTable rptDT = new DataTable();
                rptDT.Columns.Add("BEGINTIME", typeof(string));
                rptDT.Columns.Add("ENDTIME", typeof(string));
                rptDT.Columns.Add("MZ_ID", typeof(string));
                rptDT.Columns.Add("MZ_NAME", typeof(string));
                rptDT.Columns.Add("MZ_OCCC", typeof(string));
                rptDT.Columns.Add("MZ_EXAD", typeof(string));
                rptDT.Columns.Add("MZ_EXUNIT", typeof(string));
                rptDT.Columns.Add("OTIME", typeof(string));
                rptDT.Columns.Add("HOUR_PAY", typeof(string));
                rptDT.Columns.Add("PAY_SUM", typeof(string));
                rptDT.Columns.Add("OTREASON", typeof(string));
                rptDT.Columns.Add("MZ_DATE", typeof(string));
                rptDT.Columns.Add("C1000", typeof(string));
                rptDT.Columns.Add("C100", typeof(string));
                rptDT.Columns.Add("C10", typeof(string));
                rptDT.Columns.Add("C1", typeof(string));

                string strSQL = @"SELECT cob.MZ_ID, ad.MZ_NAME, ako.MZ_KCHI MZ_OCCC, cob.MZ_EXAD, akeu.MZ_KCHI MZ_EXUNIT,
                                    FLOOR(cob.PAY_HOUR / 60) OTIME, cob.REASON OTREASON, cob.OVER_DAY MZ_DATE, cob.PAY_UNIT HOUR_PAY, cob.PAY_SUM
                                  FROM C_OVERTIME_BASE cob
                                  LEFT JOIN A_DLBASE ad on ad.MZ_ID = cob.MZ_ID
                                  LEFT JOIN A_KTYPE ako on ako.MZ_KCODE = cob.MZ_OCCC And ako.MZ_KTYPE='26'
                                  LEFT JOIN A_KTYPE akeu on akeu.MZ_KCODE=cob.MZ_EXUNIT And akeu.MZ_KTYPE='25' 
                                  WHERE 1=1 ";
                List<SqlParameter> parameters = new List<SqlParameter>();
                if (!string.IsNullOrEmpty(MZ_ID))
                {
                    strSQL += "And cob.MZ_ID=@MZ_ID ";
                    parameters.Add(new SqlParameter("MZ_ID", MZ_ID));
                }
                if (!string.IsNullOrEmpty(MZ_DATE))
                {
                    strSQL += "And cob.OVER_DAY=@MZ_DATE ";
                    parameters.Add(new SqlParameter("MZ_DATE", MZ_DATE));
                }

                DataTable cobDt = o_DBFactory.ABC_toTest.Create_Table(strSQL, parameters);

                for (int i = 0; i < cobDt.Rows.Count; i++)
                {
                    DataRow dr = rptDT.NewRow();
                    dr["BEGINTIME"] = "";
                    dr["ENDTIME"] = "";
                    dr["MZ_ID"] = cobDt.Rows[i]["MZ_ID"].ToStringNullSafe();
                    dr["MZ_NAME"] = cobDt.Rows[i]["MZ_NAME"].ToStringNullSafe();
                    dr["MZ_OCCC"] = cobDt.Rows[i]["MZ_OCCC"].ToStringNullSafe();
                    dr["MZ_EXAD"] = cobDt.Rows[i]["MZ_EXAD"].ToStringNullSafe();
                    dr["MZ_EXUNIT"] = cobDt.Rows[i]["MZ_EXUNIT"].ToStringNullSafe();
                    dr["OTIME"] = cobDt.Rows[i]["OTIME"].ToStringNullSafe();
                    dr["OTREASON"] = cobDt.Rows[i]["OTREASON"].ToStringNullSafe();
                    dr["MZ_DATE"] = cobDt.Rows[i]["MZ_DATE"].ToStringNullSafe();
                    dr["HOUR_PAY"] = cobDt.Rows[i]["HOUR_PAY"].ToStringNullSafe();
                    dr["PAY_SUM"] = cobDt.Rows[i]["PAY_SUM"].ToStringNullSafe();

                    //兩位數以下不顯示金額
                    string paysum = cobDt.Rows[i]["PAY_SUM"].ToStringNullSafe();
                    dr["C1"] = paysum.SubstringOutToEmpty(paysum.Length - 1, 1).NumberToChinese(2);
                    dr["C10"] = paysum.SubstringOutToEmpty(paysum.Length - 2, 1).NumberToChinese(2);
                    dr["C100"] = paysum.SubstringOutToEmpty(paysum.Length - 3, 1).NumberToChinese(2);
                    dr["C1000"] = paysum.SubstringOutToEmpty(paysum.Length - 4, 1).NumberToChinese(2);

                    rptDT.Rows.Add(dr);
                }

                return rptDT;
            }
        }

        /// <summary>
        /// 5.2 加班費明細表
        /// </summary>
        public class OvertimeInsideDetail
        {
            public static DataTable doSearch(string DATE, string MZ_EXAD, string MZ_EXUNIT, string MZ_ID)
            {

                //因為原先SQL 句就只用('N','YO')當WHERE,所以就不拔他條件寫進去
                //                string strSQL = @"SELECT CC.MZ_ID, AKO.MZ_KCHI MZ_OCCC, AB.MZ_NAME, CC.MZ_DATE,OTREASON,CC.OTIME,HOUR_PAY,
                //case RESTFLAG  WHEN 'YO'  THEN '加班補休'   ELSE '支領加班費' END MEMO ,
                //case RESTFLAG  WHEN 'YO'  THEN 0            ELSE CC.PAY_SUM END PAY_SUM,
                //dbo.to_number(CASE WHEN AB.MZ_TBDV IS NULL THEN '999' WHEN AB.MZ_TBDV='Z99' THEN '999' ELSE AB.MZ_TBDV END) AS TBDV,
                // CC.MZ_OCCC OCCC ,  RESTFLAG ,
                //AKR.MZ_KCHI MZ_SRANK,
                //nvl(AB.MZ_SPT1,AB.MZ_SPT) MZ_SPT,
                //BB.SALARYPAY PAY1 ,NVL(BB.BOSSPAY,0) BOSS , BB.PROFESSPAY PROFESS
                //FROM C_OVERTIME_HOUR_INSIDE  CC
                //LEFT JOIN A_DLBASE AB ON AB.MZ_ID=CC.MZ_ID
                //LEFT JOIN A_KTYPE AKO ON AKO.MZ_KCODE=CC.MZ_OCCC AND AKO.MZ_KTYPE = '26'
                //LEFT JOIN B_BASE BB ON BB.IDCARD= AB.MZ_ID 
                //LEFT JOIN A_KTYPE AKR ON AKR.MZ_KCODE=AB.MZ_SRANK AND AKR.MZ_KTYPE = '09'
                //WHERE RESTFLAG IN ('N','YO') AND CC.OTIME>0 AND CC.MZ_EXAD='" + MZ_EXAD + "'";



                string strSQL = @"SELECT CC.MZ_ID, AKO.MZ_KCHI MZ_OCCC, AB.MZ_NAME, CC.MZ_DATE,OTREASON,CC.OTIME,HOUR_PAY,CD.MZ_ID,CO.MZ_ID,
case RESTFLAG  WHEN 'YO'  THEN '加班補休'   ELSE '支領加班費' END +
CASE  WHEN CD.MZ_ID IS NULL THEN '' ELSE  ',超勤'  END +
CASE  WHEN CO.MZ_ID IS NULL THEN '' ELSE ',值日' END  MEMO ,
case RESTFLAG  WHEN 'YO'  THEN 0            ELSE CC.PAY_SUM END PAY_SUM,
dbo.to_number(CASE WHEN AB.MZ_TBDV IS NULL THEN '999' WHEN AB.MZ_TBDV='Z99' THEN '999' ELSE AB.MZ_TBDV END) AS TBDV,
 CC.MZ_OCCC OCCC ,  RESTFLAG ,
AKR.MZ_KCHI MZ_SRANK,
nvl(AB.MZ_SPT1,AB.MZ_SPT) MZ_SPT,
BB.SALARYPAY PAY1 ,NVL(BB.BOSSPAY,0) BOSS , BB.PROFESSPAY PROFESS
FROM C_OVERTIME_HOUR_INSIDE  CC
LEFT JOIN A_DLBASE AB ON AB.MZ_ID=CC.MZ_ID
LEFT JOIN A_KTYPE AKO ON AKO.MZ_KCODE=CC.MZ_OCCC AND AKO.MZ_KTYPE = '26'
LEFT JOIN B_BASE BB ON BB.IDCARD= AB.MZ_ID 
LEFT JOIN A_KTYPE AKR ON AKR.MZ_KCODE=AB.MZ_SRANK AND AKR.MZ_KTYPE = '09'
LEFT JOIN C_DUTYTABLE_PERSONAL CD ON CD.MZ_ID = CC.MZ_ID AND CD.DUTYDATE = CC.MZ_DATE
LEFT JOIN C_ONDUTY_DAY  CO ON CO.MZ_ID = CC.MZ_ID AND CO.DATE_TAG = CC.MZ_DATE

WHERE RESTFLAG IN ('N','YO') AND CC.MZ_EXAD='" + MZ_EXAD + "'";



                if (!string.IsNullOrEmpty(MZ_EXUNIT))
                {
                    strSQL += " AND CC.MZ_EXUNIT='" + MZ_EXUNIT + "'";

                }

                if (!string.IsNullOrEmpty(DATE))
                {
                    strSQL += " AND dbo.SUBSTR(CC.MZ_DATE,1,5)='" + o_str.tosql(DATE) + "'";
                }

                if (!string.IsNullOrEmpty(MZ_ID))
                {
                    strSQL += " AND CC.MZ_ID='" + MZ_ID + "'";
                }
                // Joy 修改 小時數=0 則不出現該資料
                strSQL += " and CC.OTIME > 0 ";
                strSQL += " ORDER BY AB.MZ_TBDV,CC.MZ_OCCC,CC.MZ_DATE";

                DataTable tempDT = new DataTable();

                tempDT = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");




                return tempDT;

            }
            /// <summary>
            /// 新加班明細表
            /// </summary>
            /// <param name="OVER_DAY"></param>
            /// <param name="MZ_EXAD"></param>
            /// <param name="MZ_EXUNIT"></param>
            /// <param name="MZ_ID"></param>
            /// <returns></returns>
            public static DataTable doSearch_New(string OVER_DAY, string MZ_EXAD, string MZ_EXUNIT, string MZ_ID)
            {

                string strSQL = @"
with C_OVERTIME_BASE_V2 as (
    --要將業務加班 和 輪值加班 分開列出
    --業務加班資料
    select '1' as SubOrder 
    --小時/分鐘
    , FLOOR((PAY_HOUR) / 60) OTIME
    , MOD((PAY_HOUR) , 60) OTIME1
    --ID/加班日/機關/單位
    ,A.MZ_ID,A.OVER_DAY,A.MZ_EXAD,A.MZ_EXUNIT
    --事由/每小時金額
    ,A.REASON,A.PAY_UNIT
    ,A.PAY_HOUR/*只顯示業務加班時數*/,0 as SHIFT_HOUR /*排除掉輪值加班的時數*/
    ,A.PAY_SUM,A.MZ_OCCC,A.OVERTIME_TYPE
    from C_OVERTIME_BASE A
    --有提出業務加班費才抓取
    where PAY_HOUR>0
    union all    
    --輪值加班資料
    select '2' as SubOrder
    --小時/分鐘
    , FLOOR((SHIFT_HOUR) / 60) OTIME
    , MOD((SHIFT_HOUR) , 60) OTIME1
    --ID/加班日/機關/單位
    ,A.MZ_ID,A.OVER_DAY,A.MZ_EXAD,A.MZ_EXUNIT
    --事由/每小時金額
    ,A.REASON,A.PAY_UNIT
    ,0 as PAY_HOUR/*排除掉業務加班時數*/,A.SHIFT_HOUR /*顯示輪值加班的時數*/
    ,A.PAY_SUM,A.MZ_OCCC,A.OVERTIME_TYPE
    from C_OVERTIME_BASE A
    --有提出輪值加班費才抓取
    where SHIFT_HOUR>0
) 
Select cob.MZ_ID, AKO.MZ_KCHI MZ_OCCC, AB.MZ_NAME, cob.OVER_DAY MZ_DATE, cob.REASON OTREASON
--原本每小時金額使用的是C_OVERTIME_BASE.PAY_UNIT
--後來的計算規則改成:B_BASE的（基本薪資+專業加給）÷240所得值四捨五入
--注意,勞工(MZ_OCCC='Z015')要 *1.33倍
,dbo.to_number(ROUND( ((BB.SALARYPAY+BB.PROFESSPAY+BB.BOSSPAY)/240)*(case when AB.MZ_OCCC in('Z011','Z015','Z014','Z016','4769') then 1.33 else 1 end) )) as HOUR_PAY
--標記是否為勞工
,case when AB.MZ_OCCC in('Z011','Z015','Z014','Z016','4769') then 'Y' else 'N' end as isWorker
, CDP.MZ_ID CDP_ID, COD.MZ_ID COD_ID, 
--申請加班費:小時/分鐘
cob.OTIME,cob.OTIME1,
                                    (CASE WHEN cob.PAY_HOUR>0 THEN '支領加班費' 
                                          WHEN cob.PAY_HOUR=0 and cob.SHIFT_HOUR>0  THEN '支領輪值加班費' END +
                                        CASE WHEN CDP.MZ_ID IS NULL THEN '' ELSE ',超勤' END +
                                        CASE WHEN COD.MZ_ID IS NULL THEN '' ELSE ',值日' END 
                                    ) MEMO,
                                    cob.PAY_SUM,
                                    dbo.to_number(CASE WHEN AB.MZ_TBDV IS NULL THEN '999' WHEN AB.MZ_TBDV='Z99' THEN '999' ELSE AB.MZ_TBDV END) AS TBDV,
                                    cob.MZ_OCCC OCCC, AKR.MZ_KCHI MZ_SRANK, NVL(AB.MZ_SPT1,AB.MZ_SPT) MZ_SPT,
                                    BB.SALARYPAY PAY1, NVL(BB.BOSSPAY,0) BOSS, BB.PROFESSPAY PROFESS,
                                    --業務加班申請分鐘數
                                    cob.PAY_HOUR,
                                    --輪值加班申請分鐘數
                                    cob.SHIFT_HOUR
                                    From C_OVERTIME_BASE_V2 cob
                                    LEFT JOIN A_DLBASE AB ON AB.MZ_ID=cob.MZ_ID
                                    LEFT JOIN A_KTYPE AKO ON AKO.MZ_KCODE=cob.MZ_OCCC AND AKO.MZ_KTYPE = '26'
                                    LEFT JOIN C_DUTYTABLE_PERSONAL CDP ON CDP.MZ_ID=cob.MZ_ID AND CDP.DUTYDATE = cob.OVER_DAY And cob.OVERTIME_TYPE='OTU'
                                    LEFT JOIN C_ONDUTY_DAY COD ON COD.MZ_ID=cob.MZ_ID AND COD.DATE_TAG=cob.OVER_DAY And cob.OVERTIME_TYPE='OTD'
                                    LEFT JOIN A_KTYPE AKR ON AKR.MZ_KCODE=AB.MZ_SRANK AND AKR.MZ_KTYPE = '09'
                                    LEFT JOIN B_BASE BB ON BB.IDCARD=cob.MZ_ID      
                                    WHERE 1=1
                                    ";

                string strWhere = "And cob.MZ_EXAD=@MZ_EXAD ";
                List<SqlParameter> parameters = new List<SqlParameter>() { new SqlParameter("MZ_EXAD", MZ_EXAD) };
                if (!string.IsNullOrEmpty(MZ_EXUNIT))
                {
                    strWhere += "And cob.MZ_EXUNIT=@MZ_EXUNIT ";
                    parameters.Add(new SqlParameter("MZ_EXUNIT", MZ_EXUNIT));
                }
                if (!string.IsNullOrEmpty(OVER_DAY))
                {
                    strWhere += "And Substr(cob.OVER_DAY,1,5)=@OVER_DAY ";
                    parameters.Add(new SqlParameter("OVER_DAY", OVER_DAY));
                }
                if (!string.IsNullOrEmpty(MZ_ID))
                {
                    strWhere += "And cob.MZ_ID=@MZ_ID ";
                    parameters.Add(new SqlParameter("MZ_ID", MZ_ID));
                }

                DataTable dt = o_DBFactory.ABC_toTest.Create_Table(string.Format("{0} {1}  ORDER BY TBDV,MZ_OCCC,MZ_DATE,cob.MZ_ID,cob.SubOrder ", strSQL, strWhere), parameters);
                //      DataTable dt = o_DBFactory.ABC_toTest.Create_Table(string.Format("{0} {2} ORDER BY TBDV,MZ_OCCC,MZ_DATE ", strSQL, strSQL2, strWhere), parameters);

                return dt;
            }

            //public static string GetSQL(int mode)
            //{
            //    string SQL = @"
                    
            //    ";
            //}
        }

        /// <summary>
        /// 5.3.加班費彙總表 
        /// </summary>
        public class OvertimeInsideTotal
        {
            public static DataTable doSearch(string MZ_EXAD, string MZ_EXUNIT, string OVER_DAY)
            {
                //string strSQL = string.Format(@"Select COHI.MZ_ID, AD.MZ_POLNO, RTRIM(AK.MZ_KCHI) MZ_OCCC, RTRIM(AD.MZ_NAME) MZ_NAME, 
                //                                PAY_UNIT HOUR_PAY, COHI.TOTALTIME, COHI.TOTALPAY, UNIT.MZ_KCHI MZ_EXUNIT, '{0}' MZ_EXAD, 
                //                                dbo.to_number(CASE AD.MZ_TBDV WHEN NULL THEN '999' WHEN 'Z99' THEN '999' ELSE AD.MZ_TBDV END) As TBDV 
                //                                From (SELECT MZ_ID, MZ_OCCC, MZ_EXUNIT, PAY_UNIT, FLOOR(SUM(PAY_HOUR) / 60) TOTALTIME,SUM(PAY_SUM) TOTALPAY 
                //                                FROM C_OVERTIME_BASE
                //                                      WHERE MZ_OCCC!='Z011' AND MZ_EXAD=@MZ_EXAD {1} {2}
                //                                      GROUP BY MZ_ID,MZ_OCCC,MZ_EXUNIT,PAY_UNIT
                //                                ) COHI
                //                                left join A_DLBASE AD on AD.MZ_ID = COHI.MZ_ID 
                //                                left join A_KTYPE AK on AK.MZ_KTYPE = '26' And AK.MZ_KCODE = COHI.MZ_OCCC 
                //                                left join A_KTYPE UNIT on UNIT.MZ_KTYPE = '25' And UNIT.MZ_KCODE = COHI.MZ_EXUNIT 
                //                                ORDER BY TBDV,AD.MZ_PCHIEF,COHI.MZ_OCCC ",
                //                                MZ_EXAD,
                //                                string.IsNullOrEmpty(MZ_EXUNIT) ? "" : "AND MZ_EXUNIT=@MZ_EXUNIT ",
                //                                string.IsNullOrEmpty(OVER_DAY) ? "" : "AND dbo.SUBSTR(OVER_DAY,1,5)=@OVER_DAY ");
                string strSQL = string.Format(@"Select COHI.MZ_ID, AD.MZ_POLNO, RTRIM(AK.MZ_KCHI) MZ_OCCC, RTRIM(AD.MZ_NAME) MZ_NAME, 
                                                /*單價(每小時加班費)*/MZ_HOUR_PAY HOUR_PAY, 
                                                /*總時數(有申請加班費的)*/COHI.TOTALTIME, 
                                                /*金額*/COHI.TOTALPAY, 
                                                /*機關單位*/UNIT.MZ_KCHI MZ_EXUNIT, '{0}' MZ_EXAD, 
                                                /*業務加班費*/MZ_OVERTIME_PAY_JOB,
                                                /*輪值加班費*/MZ_OVERTIME_PAY_SHIFT,
                                                /*業務加班小時數*/TOTAL_JOB,
                                                /*輪值加班小時數*/TOTAL_SHIFT,
                                                dbo.to_number(CASE AD.MZ_TBDV WHEN NULL THEN '999' WHEN 'Z99' THEN '999' ELSE AD.MZ_TBDV END) As TBDV                                                         
                                                From (SELECT  MZ_ID, MZ_EXUNIT,(SELECT MZ_OCCC from A_DLBASE WHERE MZ_ID = coh.MZ_ID) MZ_OCCC, MZ_HOUR_PAY, FLOOR(SUM(MZ_REAL_HOUR) / 60) TOTALTIME,MZ_OVERTIME_PAY  TOTALPAY 
                                                       
                                                        ,coh.MZ_OVERTIME_PAY_JOB/*當月的業務加班費*/
                                                        ,coh.MZ_OVERTIME_PAY_SHIFT/*當月的輪值加班費*/
                                                        ,coh.TOTAL_JOB/*當月的業務加班時數*/
                                                        ,coh.TOTAL_SHIFT/*當月的輪值加班時數*/
                                                        FROM C_OVERTIMEMONTH_HOUR coh
                                                      WHERE MZ_EXAD=@MZ_EXAD {1} {2} {3}
                                                      GROUP BY MZ_ID,MZ_EXUNIT,MZ_HOUR_PAY,MZ_OVERTIME_PAY
                                                      ,coh.MZ_OVERTIME_PAY_JOB/*當月的業務加班費*/
                                                        ,coh.MZ_OVERTIME_PAY_SHIFT/*當月的輪值加班費*/
                                                        ,coh.TOTAL_JOB/*當月的業務加班時數*/
                                                        ,coh.TOTAL_SHIFT/*當月的輪值加班時數*/
                                                ) COHI
                                                left join A_DLBASE AD on AD.MZ_ID = COHI.MZ_ID 
                                                left join A_KTYPE AK on AK.MZ_KTYPE = '26' And AK.MZ_KCODE = COHI.MZ_OCCC 
                                                left join A_KTYPE UNIT on UNIT.MZ_KTYPE = '25' And UNIT.MZ_KCODE = COHI.MZ_EXUNIT 
                                                ORDER BY TBDV,AD.MZ_PCHIEF,COHI.MZ_OCCC  ",
                                                MZ_EXAD,
                                                string.IsNullOrEmpty(MZ_EXUNIT) ? "" : "AND MZ_EXUNIT=@MZ_EXUNIT ",
                                                string.IsNullOrEmpty(OVER_DAY) ? "" : "AND MZ_YEAR = dbo.SUBSTR(@OVER_DAY,1,3)",
                                                string.IsNullOrEmpty(OVER_DAY) ? "" : "AND MZ_MONTH = dbo.SUBSTR(@OVER_DAY,4,2)");
                List<SqlParameter> parameters = new List<SqlParameter>() { new SqlParameter("MZ_EXAD", MZ_EXAD) };
                if (!string.IsNullOrEmpty(MZ_EXUNIT))
                {
                    parameters.Add(new SqlParameter("MZ_EXUNIT", MZ_EXUNIT));
                }
                if (!string.IsNullOrEmpty(OVER_DAY))
                {
                    parameters.Add(new SqlParameter("OVER_DAY", OVER_DAY));
                }

                DataTable dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, parameters);

                return dt;
            }
        }

        /// <summary>
        /// 5.x 新加班勤惰明細表
        /// </summary>
        public class OverTimeDutyDetail
        {
            public static DataTable doSearch(C_OverTime_Detail_rpt_Query query)
            {
                //設定報表DataTable
                DataTable OverTimeDutyDetail = new DataTable();
                OverTimeDutyDetail.Columns.Add("OVER_DAY", typeof(string));
                OverTimeDutyDetail.Columns.Add("MEMO", typeof(string));
                OverTimeDutyDetail.Columns.Add("INTIME", typeof(string));
                OverTimeDutyDetail.Columns.Add("OUTTIME", typeof(string));
                OverTimeDutyDetail.Columns.Add("OVER_HOUR", typeof(int));
                OverTimeDutyDetail.Columns.Add("OVER_MIN", typeof(int));
                OverTimeDutyDetail.Columns.Add("KIND", typeof(string));
                OverTimeDutyDetail.Columns.Add("OVER_REMARK", typeof(string));
                OverTimeDutyDetail.Columns.Add("REVIEW", typeof(string));

                #region 取得查詢人資料
                string strSQL = @"Select * From A_DLBASE Where 1=1 ";
                List<SqlParameter> parameters = new List<SqlParameter>();
                if (!string.IsNullOrEmpty(query.Search_EXAD))
                {
                    strSQL += "And MZ_EXAD=@Search_EXAD ";
                    parameters.Add(new SqlParameter("Search_EXAD", query.Search_EXAD));
                }
                if (!string.IsNullOrEmpty(query.Search_EXUNIT))
                {
                    strSQL += "And MZ_EXUNIT=@Search_EXUNIT ";
                    parameters.Add(new SqlParameter("Search_EXUNIT", query.Search_EXUNIT));
                }
                if (!string.IsNullOrEmpty(query.Search_ID))
                {
                    strSQL += "And MZ_ID=@Search_ID ";
                    parameters.Add(new SqlParameter("Search_ID", query.Search_ID));
                }
                if (!string.IsNullOrEmpty(query.Search_NAME))
                {
                    strSQL += "And MZ_NAME=@Search_NAME ";
                    parameters.Add(new SqlParameter("Search_NAME", query.Search_NAME));
                }

                DataTable adb = o_DBFactory.ABC_toTest.Create_Table(strSQL, parameters);
                #endregion

                if (!string.IsNullOrEmpty(query.DateS) && !string.IsNullOrEmpty(query.DateE))
                {
                    DateTime timeS = DateTime.Parse(ForDateTime.TWDateToRCDate(query.DateS, "yyyy/MM/dd"));
                    DateTime timeE = DateTime.Parse(ForDateTime.TWDateToRCDate(query.DateE, "yyyy/MM/dd"));
                    TimeSpan timeSpan = timeE - timeS;

                    foreach (DataRow adbRow in adb.Rows)
                    {
                        for (int i = 0; i <= timeSpan.Days; i++)
                        {
                            DateTime t = timeS.AddDays(i);
                            //取得刷卡紀錄
                            DataRow CardRow = Count_Card_Record(adbRow["MZ_ID"].ToStringNullSafe(), t, "0", "", "", "", OverTimeDutyDetail);

                            if (!string.IsNullOrEmpty(CardRow[0].ToStringNullSafe()))
                            {
                                //建立刷卡紀錄資料
                                DataRow dr = OverTimeDutyDetail.NewRow();
                                dr["OVER_DAY"] = CardRow["OVER_DAY"].ToStringNullSafe();
                                dr["MEMO"] = CardRow["MEMO"].ToStringNullSafe();
                                dr["INTIME"] = CardRow["INTIME"].ToStringNullSafe();
                                dr["OUTTIME"] = CardRow["OUTTIME"].ToStringNullSafe();
                                dr["KIND"] = CardRow["KIND"].ToStringNullSafe();

                                #region 建立加班資料
                                strSQL = @"Select cob.OVER_REMARK,
                                            FLOOR(case when cob.OVERTIME_CHG='Y' then cob.OVERTIME_CHG_TOTAL else cob.OVER_TOTAL end / 60) OVER_HOUR, 
                                            MOD(case when cob.OVERTIME_CHG='Y' then cob.OVERTIME_CHG_TOTAL else cob.OVER_TOTAL end, 60) OVER_MINUTE,
                                            (Select C_STATUS_NAME From 
                                                (Select cs.C_STATUS_NAME, coh.OVERTIME_SN, ROW_NUMBER() OVER (partition by OVERTIME_SN ORDER BY coh.O_SN desc) AS RN 
                                                From C_OVERTIME_HISTORY coh 
                                                INNER JOIN C_STATUS cs ON cs.C_STATUS_SN=coh.PROCESS_STATUS)
                                                Where RN=1 And OVERTIME_SN=cob.SN
                                            ) REVIEW
                                            From C_OVERTIME_BASE cob 
                                            Where cob.OVER_DAY=@OVER_DAY And cob.MZ_ID=@MZ_ID ";
                                parameters = new List<SqlParameter>()
                                {
                                    new SqlParameter("OVER_DAY", ForDateTime.RCDateToTWDate(t.ToString("yyyyMMdd"), "yyyMMdd")),
                                    new SqlParameter("MZ_ID", adbRow["MZ_ID"].ToStringNullSafe())
                                };
                                DataTable overtimeDt = o_DBFactory.ABC_toTest.Create_Table(strSQL, parameters);
                                if (overtimeDt != null && overtimeDt.Rows.Count > 0)
                                {
                                    dr["OVER_HOUR"] = overtimeDt.Rows[0]["OVER_HOUR"].ToStringNullSafe();
                                    dr["OVER_MIN"] = overtimeDt.Rows[0]["OVER_MINUTE"].ToStringNullSafe();
                                    dr["OVER_REMARK"] = overtimeDt.Rows[0]["OVER_REMARK"].ToStringNullSafe();
                                    dr["REVIEW"] = overtimeDt.Rows[0]["REVIEW"].ToStringNullSafe();
                                }
                                #endregion

                                OverTimeDutyDetail.Rows.Add(dr);
                            }
                        }
                    }
                }

                return OverTimeDutyDetail;
            }

            // TODO SKY 暫時完整Copy，需調整內容為該表專用
            public static DataRow Count_Card_Record(string MZ_ID, DateTime DATE, string type, string punit, string name, string occc, DataTable dt)
            {
                int INTIME = 90100, OUTTIME = 170000;

                if (DATE >= Convert.ToDateTime("2021/05/17") && DATE <= Convert.ToDateTime("2021/07/26"))
                {
                    INTIME = 100100;
                    OUTTIME = 160000;
                }

                if (DATE >= Convert.ToDateTime("2021/07/27") && DATE <= Convert.ToDateTime("2022/12/31"))
                {
                    INTIME = 093100;
                    OUTTIME = 163000;
                }

                string LOGDATE = DATE.Year.ToString() + "/" + DATE.Month.ToString().PadLeft(2, '0') + "/" + DATE.Day.ToString().PadLeft(2, '0');
                string LOGDATE1 = (DATE.Year - 1911).ToString().PadLeft(3, '0') + DATE.Month.ToString().PadLeft(2, '0') + DATE.Day.ToString().PadLeft(2, '0');

                string KIND = "";
                string MEMO = "";
                string CODE = "";

                //請假/國定休假日紀錄清單
                // 陣列意義 > 一般請假：[0]假別, [1]請假起日, [2]起時, [3]訖日, [4]迄時, [5]日數, [6]時數；國定假日：只回傳假日名稱
                List<String> lstDayOff = new List<String>();
                //這是在找有休假的的資料 
                lstDayOff = C_CountCardRecord.list_Abnormal(MZ_ID, LOGDATE);

                string selectINTIME = "";
                string selectOUTTIME = "";
                //matthew 因公未刷卡
                string C_CARD_EDIT = "";

                #region 全部

                //全部
                {
                    //matthew 新增 因公未刷卡 如果TID = 99 就是因公 還要看是上班還是下班目前沒判斷 警局也沒要求 所以先不改
                    C_CARD_EDIT = o_DBFactory.ABC_toTest.vExecSQL("SELECT TID FROM C_CARDHISTORY_NEW WHERE dbo.SUBSTR(USERNAME,0,10)='" + MZ_ID + "'" + "AND LOGDATE='" + LOGDATE + "'" + "AND TID ='99'");
                    //C_CARD_EDIT = o_DBFactory.ABC_toTest.vExecSQL("SELECT TID FROM (SELECT TID,LOGTIME,ROW_NUMBER() OVER(ORDER BY LOGTIME) AS ROWCOUNT FROM C_CARDHISTORY_NEW WHERE dbo.SUBSTR(USERNAME,0,10)='" + MZ_ID + "' AND LOGDATE='" + LOGDATE + "' AND VERIFY='IN') WHERE ROWCOUNT=1");
                    //雖然說為什麼要進出資料庫找兩次.但找起來還算快.就先不改
                    selectINTIME = o_DBFactory.ABC_toTest.vExecSQL("SELECT LOGTIME FROM (SELECT LOGTIME,ROW_NUMBER() OVER(ORDER BY LOGTIME) AS ROWCOUNT FROM C_CARDHISTORY_NEW WHERE dbo.SUBSTR(USERNAME,0,10)='" + MZ_ID + "' AND LOGDATE='" + LOGDATE + "' AND VERIFY='IN') WHERE ROWCOUNT=1");
                    if (selectINTIME.Length > 0 && selectINTIME.Substring(0, 3) == "24:")
                    {
                        selectINTIME = "00:" + selectINTIME.Substring(3, 5);
                    }

                    selectOUTTIME = o_DBFactory.ABC_toTest.vExecSQL("SELECT LOGTIME FROM (SELECT LOGTIME,ROW_NUMBER() OVER(ORDER BY LOGTIME DESC) AS ROWCOUNT FROM C_CARDHISTORY_NEW WHERE dbo.SUBSTR(USERNAME,0,10)='" + MZ_ID.Trim() + "' AND LOGDATE='" + LOGDATE + "' AND VERIFY='IN') WHERE ROWCOUNT=1");
                    if (selectOUTTIME.Length > 0 && selectOUTTIME.Substring(0, 3) == "24:")
                    {
                        selectOUTTIME = "00:" + selectOUTTIME.Substring(3, 5);
                    }
                    //雖然說為什麼要進出資料庫找兩次找.但找起來還算快.就先不改

                    //取得免刷卡人員做判斷 20190718 by sky
                    string noSwipe = o_DBFactory.ABC_toTest.vExecSQL(string.Format("SELECT MZ_CLOCK FROM C_CARDSET WHERE MZ_ID = '{0}' ", MZ_ID.Trim()));

                    string qdate = (DATE.Year - 1911).ToString().PadLeft(3, '0') + DATE.Month.ToString().PadLeft(2, '0') + DATE.Day.ToString().PadLeft(2, '0');
                    //抓取補班日的資料?
                    string C_SPRINGDAY = o_DBFactory.ABC_toTest.vExecSQL(string.Format("SELECT mz_spring_name FROM C_SPRINGDAY WHERE mz_spring_date = '{0}' ", qdate));

                    if (string.IsNullOrEmpty(selectINTIME) && string.IsNullOrEmpty(selectOUTTIME))//如果當天都沒刷卡紀錄
                    {
                        //如果 當天是六日 且 不是補班
                        if ((DateTime.Parse(LOGDATE).DayOfWeek == DayOfWeek.Saturday || DateTime.Parse(LOGDATE).DayOfWeek == DayOfWeek.Sunday) && C_SPRINGDAY == "")
                        {
                            MEMO = "例假日";
                        }
                        else if (noSwipe == "N")
                        {
                            //免刷卡人員不進行判斷
                        }
                        else if (lstDayOff.Count == 0) ///無刷卡狀態
                        {
                            if (DATE.Year.ToString() + DATE.Month.ToString() + DATE.Day.ToString() == DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString())
                            {
                                int NOWTIME = int.Parse(DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0'));

                                if (NOWTIME <= INTIME)///本日九點前
                                {
                                    KIND = "";
                                    MEMO = "";
                                }
                                else if (NOWTIME > INTIME)///本日九點後
                                {
                                    KIND = "上班未刷卡";
                                    MEMO = "上班異常";
                                }
                            }
                            else///過本日後
                            {
                                KIND = "未刷卡";
                                MEMO = "上班異常";
                            }
                        }
                        else
                        {
                            if (lstDayOff.Count > 1)///有請假
                            {
                                CODE = lstDayOff[0];

                                if (int.Parse(LOGDATE1) >= int.Parse(lstDayOff[1]) && int.Parse(LOGDATE1) <= int.Parse(lstDayOff[3]) && int.Parse(lstDayOff[6]) > 0)///正常
                                {
                                    KIND = "";
                                    MEMO = "請假";
                                }
                                else
                                {
                                    KIND = "未刷卡";
                                    MEMO = "上班異常";
                                }
                            }
                            else if (lstDayOff.Count == 1)
                            {
                                KIND = "";
                                MEMO = "國定假日";
                            }
                        }
                        if (C_CARD_EDIT == "99")
                        {
                            MEMO = "因公疏未辦理刷卡`";
                        }
                    }
                    else//如果當天有刷卡紀錄
                    {
                        if (DateTime.Parse(LOGDATE).DayOfWeek == DayOfWeek.Saturday || DateTime.Parse(LOGDATE).DayOfWeek == DayOfWeek.Sunday)
                        {
                            MEMO = "例假日";
                        }
                        else if (noSwipe == "N")
                        {
                            //免刷卡人員不進行判斷
                        }
                        else if (DATE.Year.ToString() + DATE.Month.ToString() + DATE.Day.ToString() == DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString())///如果是執行此動作當天當日
                        {
                            if (int.Parse(selectINTIME.Replace(":", string.Empty)) <= INTIME)///當日刷9點前的卡
                            {
                                if (selectINTIME == selectOUTTIME && lstDayOff.Count == 0)///只有一筆刷卡資料而且無請假
                                {
                                    KIND = "";
                                    MEMO = "";
                                    selectOUTTIME = "";
                                }
                                else if (selectINTIME == selectOUTTIME && lstDayOff.Count > 1)///只有一筆刷卡資料而且有請假
                                {
                                    selectOUTTIME = "";

                                    CODE = lstDayOff[0];

                                    DateTime TS1 = DateTime.Parse(LOGDATE + " " + lstDayOff[2] + ":00");///跟請假起時比

                                    if (DateTime.Now > TS1)///超過就算下班未刷卡
                                    {
                                        selectOUTTIME = "";
                                        KIND = "下班未刷卡";
                                        MEMO = "上班異常";
                                    }

                                }
                                else if (int.Parse(selectOUTTIME.Replace(":", string.Empty)) < INTIME)
                                {
                                    KIND = "";
                                    MEMO = "";
                                    selectOUTTIME = "";
                                }
                                else if (int.Parse(selectOUTTIME.Replace(":", string.Empty)) < OUTTIME)///下班時間少於17點
                                {
                                    if (lstDayOff.Count > 1)///有請假
                                    {
                                        CODE = lstDayOff[0];

                                        DateTime TS1 = DateTime.Parse(LOGDATE + " " + selectINTIME);

                                        DateTime TS2 = DateTime.Parse(LOGDATE + " " + selectOUTTIME);

                                        TimeSpan TS = TS2 - TS1;

                                        if (TS.Hours + int.Parse(lstDayOff[6]) >= 8)
                                        {
                                            MEMO = "請假";
                                        }
                                        else
                                        {
                                            KIND = "早退";
                                            MEMO = "請假早退";
                                        }
                                    }
                                    else
                                    {
                                        KIND = "早退";
                                        MEMO = "上班異常";
                                    }
                                }
                                else ///下班時間大於17點
                                {
                                    if (lstDayOff.Count > 1)
                                    {
                                        CODE = lstDayOff[0];
                                    }

                                    DateTime TS1 = DateTime.Parse(LOGDATE + " " + selectINTIME);

                                    DateTime TS2 = DateTime.Parse(LOGDATE + " " + selectOUTTIME);

                                    TimeSpan TS = TS2 - TS1;

                                    if (TS.Hours > 8)
                                    {
                                        MEMO = "";
                                    }
                                    else
                                    {
                                        KIND = "早退";
                                        MEMO = "上班異常";
                                    }
                                }
                            }
                            else if (int.Parse(selectINTIME.Replace(":", string.Empty)) > INTIME)///當天刷卡時間大於9點1分
                            {
                                if (selectINTIME == selectOUTTIME && lstDayOff.Count == 0)///只有一筆刷卡紀錄
                                {
                                    if (int.Parse(selectINTIME.Replace(":", string.Empty)) > OUTTIME)///記錄大於17點
                                    {
                                        KIND = "上班未刷卡";
                                        MEMO = "上班異常";
                                        selectINTIME = "";
                                    }
                                    else
                                    {
                                        KIND = "遲到";
                                        MEMO = "上班異常";
                                        selectOUTTIME = "";
                                    }
                                }
                                else if (selectINTIME == selectOUTTIME && lstDayOff.Count > 1)///只有一筆刷卡紀錄有請假
                                {
                                    CODE = lstDayOff[0];

                                    if (int.Parse(lstDayOff[2].Replace(":", "")) <= 900 && int.Parse(lstDayOff[4].Replace(":", "")) <= 1330)
                                    {
                                        if (int.Parse(selectINTIME.Replace(":", string.Empty)) < 133000)///未超過1330刷卡
                                        {
                                            KIND = "";
                                            MEMO = "";
                                            selectOUTTIME = "";
                                        }
                                        else///超過1330
                                        {
                                            KIND = "遲到";
                                            MEMO = "上班異常";
                                            selectOUTTIME = "";
                                        }
                                    }
                                    else if (int.Parse(lstDayOff[2].Replace(":", "")) <= 900)
                                    {
                                        if (int.Parse(selectINTIME.Replace(":", string.Empty)) < int.Parse(lstDayOff[4].Replace(":", string.Empty) + "00"))
                                        {
                                            if (int.Parse(selectINTIME.Replace(":", string.Empty)) > 173000)
                                            {
                                                KIND = "上班未刷卡";
                                                MEMO = "上班異常";
                                                selectINTIME = "";
                                            }
                                            else
                                            {
                                                KIND = "下班未刷卡";
                                                MEMO = "上班異常";
                                                selectOUTTIME = "";
                                            }
                                        }
                                        else
                                        {
                                            KIND = "遲到";
                                            MEMO = "上班異常";
                                            selectOUTTIME = "";
                                        }
                                    }
                                    else
                                    {
                                        KIND = "遲到";
                                        MEMO = "上班異常";
                                        selectOUTTIME = "";
                                    }
                                }
                                else if (int.Parse(selectOUTTIME.Replace(":", string.Empty)) < 180000)
                                {
                                    if (lstDayOff.Count > 1)
                                    {
                                        CODE = lstDayOff[0];

                                        DateTime TS1 = DateTime.Parse(LOGDATE + " " + lstDayOff[2] + ":00");

                                        DateTime TS2 = DateTime.Parse(LOGDATE + " " + selectOUTTIME);

                                        TimeSpan TS = TS1 - TS2;

                                        if (TS.Hours + int.Parse(lstDayOff[6]) < 8)
                                        {
                                            KIND = "遲到";
                                            MEMO = "上班異常";
                                        }
                                        else
                                        {
                                            KIND = "遲到早退";
                                            MEMO = "上班異常";
                                        }
                                    }
                                    else
                                    {
                                        KIND = "遲到早退";
                                        MEMO = "上班異常";
                                    }
                                }
                                else
                                {
                                    if (lstDayOff.Count > 1)
                                    {
                                        CODE = lstDayOff[0];

                                        MEMO = "";
                                    }
                                    else
                                    {
                                        KIND = "遲到";
                                        MEMO = "上班異常";
                                    }
                                }

                            }
                        }
                        else
                        {
                            //取得時間差
                            DateTime dt1 = DateTime.Parse(LOGDATE + " " + selectINTIME); //刷卡時間(上班)
                            DateTime dt2 = DateTime.Parse(LOGDATE + " " + selectOUTTIME); //刷卡時間(下班)，若當日無下班刷卡紀錄則會跟上班時間相同
                            TimeSpan dt_diff = dt2 - dt1; //時間差


                            if (int.Parse(selectINTIME.Replace(":", string.Empty)) <= INTIME)///非當日刷卡時間未超過9點1分
                            {
                                if (selectINTIME == selectOUTTIME && lstDayOff.Count == 0)///只有一筆刷卡資料且未請假
                                {
                                    KIND = "下班未刷卡";
                                    //MEMO = ""; //20151209 Neil: 小隊長建議移除備註欄即可
                                    MEMO = "上班異常";//20160112 JACK: 1041218 江杰昊 異常報表查不到資料 memo為空的問題
                                    selectOUTTIME = "";
                                }
                                else if (selectINTIME == selectOUTTIME && lstDayOff.Count > 0) ///只有一筆刷卡資料且有請假
                                {
                                    KIND = "下班未刷卡"; //事實上仍未刷卡故顯示
                                    MEMO = "請假"; //但備註欄補上請假資訊
                                    selectOUTTIME = "";
                                }
                                ///20151209 Neil : 此部分原先的設計共有兩種情況會進入下面的 else if :
                                /// 1. 有人下午請假，但忘記刷下班卡
                                /// 2. 有人平時忘記打下班卡，只好用補請假的方式去補回
                                ///但此兩種情況均不符合原先 if 裡面的設計(下班未刷、上班異常)
                                /// 
                                /// 因此補上條件 selectINTIME != selectOUTTIME，用於判斷短時間內上下班，且早於 09:01
                                /// 此 Else 適用情況 : 
                                /// 若當日有正常上下班刷卡，但下班刷卡時間早於 09:01
                                else if (selectINTIME != selectOUTTIME && int.Parse(selectOUTTIME.Replace(":", string.Empty)) < INTIME)///多筆資料都在九點前
                                {


                                    if (dt_diff.Hours >= 1)
                                    {
                                        if (lstDayOff.Count == 0) //無請假
                                        {
                                            KIND = "早退";
                                            MEMO = "上班異常";
                                            selectOUTTIME = "";
                                        }
                                        else
                                        {
                                            if (lstDayOff[4].ToString().IsNormalized())
                                            {
                                                KIND = "";
                                                MEMO = lstDayOff[0].ToString();
                                            }
                                            else
                                            {
                                                string strETime = lstDayOff[4].ToString().Substring(0, 2) + lstDayOff[4].ToString().Substring(3, 2);
                                                int iETime = int.Parse(strETime);

                                                if (iETime < 1330)
                                                {
                                                    KIND = "";
                                                    MEMO = "早上請假";
                                                }
                                                else
                                                {
                                                    KIND = "";
                                                    MEMO = lstDayOff[0].ToString();
                                                }
                                            }

                                        }
                                    }
                                    else
                                    {
                                        KIND = "早退";
                                        MEMO = "上班異常";
                                        selectOUTTIME = "";
                                    }


                                }
                                else if (int.Parse(selectOUTTIME.Replace(":", string.Empty)) < OUTTIME)///下班刷卡時間早於5點
                                {
                                    if (lstDayOff.Count > 1) ///有請假
                                    {
                                        CODE = lstDayOff[0];

                                        DateTime TS1 = DateTime.Parse(LOGDATE + " " + selectINTIME);

                                        DateTime TS2 = DateTime.Parse(LOGDATE + " " + selectOUTTIME);

                                        TimeSpan TS = TS2 - TS1;

                                        if (TS.Hours + int.Parse(lstDayOff[6]) >= 8)
                                        {
                                            MEMO = "下午請假";
                                        }
                                        else
                                        {
                                            KIND = "早退";
                                            MEMO = "上班異常";
                                        }
                                    }
                                    else
                                    {
                                        KIND = "早退";
                                        MEMO = "上班異常";
                                    }
                                }
                                else
                                {
                                    DateTime TS1 = DateTime.Parse(LOGDATE + " " + selectINTIME);
                                    DateTime TS2 = DateTime.Parse(LOGDATE + " " + selectOUTTIME);
                                    TimeSpan TS = TS2 - TS1;

                                    if (lstDayOff.Count > 1) ///有請假
                                    {
                                        if (TS.Hours + int.Parse(lstDayOff[6]) > 8)
                                        {
                                            KIND = "";
                                        }
                                        //else if (TS.Hours + int.Parse(temp[6]) == 8)
                                        //{
                                        //    KIND = "早退";
                                        //    MEMO = "請假早退";
                                        //}
                                        else
                                        {
                                            KIND = "早退";
                                            MEMO = "請假早退";
                                        }
                                    }
                                    else
                                    {
                                        if (TS.Hours > 8)
                                        {
                                            MEMO = "";
                                        }
                                        else
                                        {
                                            KIND = "早退";
                                            MEMO = "上班異常";
                                        }
                                    }

                                }
                            }
                            else if (int.Parse(selectINTIME.Replace(":", string.Empty)) > INTIME)///上班刷卡時間大於9點1分
                            {
                                if (selectINTIME == selectOUTTIME && lstDayOff.Count == 0)///只一筆刷卡資料
                                {
                                    if (int.Parse(selectINTIME.Replace(":", string.Empty)) > OUTTIME)
                                    {
                                        KIND = "上班未刷卡";
                                        MEMO = "上班異常";
                                        selectINTIME = "";
                                    }
                                    else
                                    {
                                        KIND = "遲到下班未刷卡";
                                        MEMO = "上班異常";
                                        selectINTIME = "";
                                    }
                                }
                                else if (selectINTIME == selectOUTTIME && lstDayOff.Count > 1)///只一筆有請假
                                {
                                    CODE = lstDayOff[0];

                                    if (lstDayOff[2] == "08:30" && lstDayOff[4] == "12:30")
                                    {

                                        if (int.Parse(selectINTIME.Replace(":", string.Empty)) < 133000)
                                        {
                                            KIND = "下班未刷卡";
                                            MEMO = "上班異常";
                                            selectOUTTIME = "";
                                        }
                                        else if (int.Parse(selectINTIME.Replace(":", string.Empty)) >= 173000)
                                        {
                                            KIND = "上班未刷卡";
                                            MEMO = "請假";
                                            selectINTIME = "";
                                        }
                                        else
                                        {
                                            KIND = "遲到下班未刷卡";
                                            MEMO = "上班異常";
                                            selectOUTTIME = "";
                                        }

                                    }
                                    else if (int.Parse(lstDayOff[2].Replace(":", "")) <= 900)
                                    {
                                        if (int.Parse(selectINTIME.Replace(":", string.Empty)) < int.Parse(lstDayOff[4].Replace(":", string.Empty) + "00"))
                                        {
                                            KIND = "下班未刷卡";
                                            MEMO = "上班異常";
                                            selectOUTTIME = "";
                                        }
                                        else
                                        {
                                            KIND = "遲到下班未刷卡";
                                            MEMO = "上班異常";
                                            selectOUTTIME = "";
                                        }
                                    }
                                    else
                                    {
                                        KIND = "遲到下班未刷卡";
                                        MEMO = "上班異常";
                                        selectOUTTIME = "";
                                    }
                                }
                                else if (int.Parse(selectOUTTIME.Replace(":", string.Empty)) < 180000)
                                {
                                    if (lstDayOff.Count > 1)
                                    {
                                        CODE = lstDayOff[0];

                                        DateTime TS1 = DateTime.Parse(LOGDATE + " " + selectINTIME);

                                        DateTime TS2 = DateTime.Parse(LOGDATE + " " + selectOUTTIME);

                                        TimeSpan TS = TS2 - TS1;

                                        if (TS.Hours + int.Parse(lstDayOff[6]) >= 8)
                                        {
                                            KIND = "";
                                            MEMO = "早上請假";
                                        }
                                        else
                                        {
                                            KIND = "遲到早退";
                                            // Joy 當天有請假紀錄，則不出現上班異常備註，新需求要加註請假類別。
                                            // MEMO = "上班異常";  
                                            if (!string.IsNullOrEmpty(CODE))
                                            {
                                                MEMO = CODE;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        KIND = "遲到早退";
                                        MEMO = "上班異常";
                                    }
                                }
                                else
                                {
                                    if (lstDayOff.Count > 1)
                                    {
                                        CODE = lstDayOff[0];

                                        MEMO = "正常";
                                    }
                                    else
                                    {
                                        KIND = "遲到";
                                        MEMO = "上班異常";
                                    }
                                }
                            }
                            else if (string.IsNullOrEmpty(selectINTIME))
                            {
                                if (int.Parse(selectOUTTIME.Replace(":", string.Empty)) < OUTTIME)
                                {
                                    KIND = "遲到早退上班未刷卡";

                                    MEMO = "上班異常";
                                }
                                else
                                {
                                    KIND = "遲到上班未刷卡";

                                    MEMO = "上班異常";
                                }
                            }
                        }
                        if (C_CARD_EDIT == "99")
                        {
                            MEMO = "因公疏未辦理刷卡";
                        }
                    }
                }
                #endregion 其他 含請假 上班異常 遲到...等

                DataRow dr = dt.NewRow();
                {
                    dr["OVER_DAY"] = int.Parse(DateManange.datetostr(LOGDATE).Substring(0, 3)).ToString() + "/" + DateManange.datetostr(LOGDATE).Substring(3, 2) + "/" + DateManange.datetostr(LOGDATE).Substring(5, 2); ;
                    dr["INTIME"] = selectINTIME;
                    dr["OUTTIME"] = selectOUTTIME;
                    dr["KIND"] = KIND;
                    if (C_CARD_EDIT == "99")
                    {
                        dr["MEMO"] = "因公疏未辦理刷卡";
                    }
                    else
                    {
                        dr["MEMO"] = MEMO;
                    }
                    return dr;
                }
            }
        }


        #endregion

        #region 超勤報表
        /// <summary>
        /// 超勤報表
        /// </summary>
        public class OVERTIME
        {

            /// <summary>
            ///  6.3.3 OR 4 超勤支領情形分析表 如果下拉單位未選擇時,同時顯示金額及時數  這沒辦法用SQL兜出來嗎?????? 
            /// </summary>
            public class OvertimeOutSide_Unit
            {
                public static DataTable doSearch(string DATE, string MZ_AD, string MZ_UNIT)
                {
                    DataTable rpt_dt = new DataTable();

                    int I_HCOUNT = 0;
                    int O_HCOUNT = 0;
                    int I_HOUR01 = 0;
                    int I_HOUR31 = 0;
                    int I_HOUR41 = 0;
                    int I_HOUR51 = 0;
                    int I_HOUR61 = 0;
                    int I_HOUR71 = 0;
                    int I_HOUR81 = 0;
                    int I_HOUR91 = 0;
                    int O_HOUR01 = 0;
                    int O_HOUR31 = 0;
                    int O_HOUR41 = 0;
                    int O_HOUR51 = 0;
                    int O_HOUR61 = 0;
                    int O_HOUR71 = 0;
                    int O_HOUR81 = 0;
                    int O_HOUR91 = 0;
                    int I_CCOUNT = 0;
                    int O_CCOUNT = 0;
                    int I_CASH08 = 0;
                    int I_CASH09 = 0;
                    int I_CASH10 = 0;
                    int I_CASH11 = 0;
                    int I_CASH12 = 0;
                    int I_CASH13 = 0;
                    int I_CASH14 = 0;
                    int I_CASH15 = 0;
                    int I_CASH16 = 0;
                    int I_CASH17 = 0;
                    int I_CASH18 = 0;
                    int I_CASH19 = 0;
                    int I_CTOTAL = 0;
                    int O_CASH08 = 0;
                    int O_CASH09 = 0;
                    int O_CASH10 = 0;
                    int O_CASH11 = 0;
                    int O_CASH12 = 0;
                    int O_CASH13 = 0;
                    int O_CASH14 = 0;
                    int O_CASH15 = 0;
                    int O_CASH16 = 0;
                    int O_CASH17 = 0;
                    int O_CASH18 = 0;
                    int O_CASH19 = 0;
                    int O_CTOTAL = 0;

                    rpt_dt.Columns.Add("I_HCOUNT", typeof(string)); //本月支領人數
                    rpt_dt.Columns.Add("I_HOUR01", typeof(string));
                    rpt_dt.Columns.Add("I_HOUR31", typeof(string));
                    rpt_dt.Columns.Add("I_HOUR41", typeof(string));
                    rpt_dt.Columns.Add("I_HOUR51", typeof(string));
                    rpt_dt.Columns.Add("I_HOUR61", typeof(string));
                    rpt_dt.Columns.Add("I_HOUR71", typeof(string));
                    rpt_dt.Columns.Add("I_HOUR81", typeof(string));
                    rpt_dt.Columns.Add("I_HOUR91", typeof(string));
                    rpt_dt.Columns.Add("O_HCOUNT", typeof(string));
                    rpt_dt.Columns.Add("O_HOUR01", typeof(string));
                    rpt_dt.Columns.Add("O_HOUR31", typeof(string));
                    rpt_dt.Columns.Add("O_HOUR41", typeof(string));
                    rpt_dt.Columns.Add("O_HOUR51", typeof(string));
                    rpt_dt.Columns.Add("O_HOUR61", typeof(string));
                    rpt_dt.Columns.Add("O_HOUR71", typeof(string));
                    rpt_dt.Columns.Add("O_HOUR81", typeof(string));
                    rpt_dt.Columns.Add("O_HOUR91", typeof(string));
                    rpt_dt.Columns.Add("I_CCOUNT", typeof(string));
                    rpt_dt.Columns.Add("I_CASH08", typeof(string));
                    rpt_dt.Columns.Add("I_CASH09", typeof(string));
                    rpt_dt.Columns.Add("I_CASH10", typeof(string));
                    rpt_dt.Columns.Add("I_CASH11", typeof(string));
                    rpt_dt.Columns.Add("I_CASH12", typeof(string));
                    rpt_dt.Columns.Add("I_CASH13", typeof(string));
                    rpt_dt.Columns.Add("I_CASH14", typeof(string));
                    rpt_dt.Columns.Add("I_CASH15", typeof(string));
                    rpt_dt.Columns.Add("I_CASH16", typeof(string));
                    rpt_dt.Columns.Add("I_CASH17", typeof(string));
                    rpt_dt.Columns.Add("I_CASH18", typeof(string));
                    rpt_dt.Columns.Add("I_CASH19", typeof(string));
                    rpt_dt.Columns.Add("I_CTOTAL", typeof(string));
                    rpt_dt.Columns.Add("O_CCOUNT", typeof(string));
                    rpt_dt.Columns.Add("O_CASH08", typeof(string));
                    rpt_dt.Columns.Add("O_CASH09", typeof(string));
                    rpt_dt.Columns.Add("O_CASH10", typeof(string));
                    rpt_dt.Columns.Add("O_CASH11", typeof(string));
                    rpt_dt.Columns.Add("O_CASH12", typeof(string));
                    rpt_dt.Columns.Add("O_CASH13", typeof(string));
                    rpt_dt.Columns.Add("O_CASH14", typeof(string));
                    rpt_dt.Columns.Add("O_CASH15", typeof(string));
                    rpt_dt.Columns.Add("O_CASH16", typeof(string));
                    rpt_dt.Columns.Add("O_CASH17", typeof(string));
                    rpt_dt.Columns.Add("O_CASH18", typeof(string));
                    rpt_dt.Columns.Add("O_CASH19", typeof(string));
                    rpt_dt.Columns.Add("O_CTOTAL", typeof(string));

                    rpt_dt.Columns.Add("UNIT", typeof(string));

                    //20151203 Neil 原先WHERE是從A_DLBASE撈資料，但應以超勤記錄當下為主，故改為WHERE C_DUTYMONTHOVERTIME_HOUR 內的條件
                    //因應刑大提出未編制的單位分類出現，原服務單位條件改成編制單位條件。 20180615 by sky
                    string strSQL = String.Format(@"SELECT MZ_REAL_HOUR, MZ_OVERTIME_PAY, MZ_ID, DUTYUSERCLASSIFY
                                                      FROM C_DUTYMONTHOVERTIME_HOUR
                                                     WHERE MZ_YEAR='{0}' 
                                                           AND MZ_MONTH='{1}' 
                                                           AND MZ_AD='{2}'
                                                           AND MZ_UNIT='{3}'"
                                                    , DATE.Substring(0, 3)
                                                    , DATE.Substring(3, 2)
                                                    , MZ_AD
                                                    , MZ_UNIT);

                    string MZ_UNIT_CH = o_A_KTYPE.RUNIT(MZ_UNIT);
                    DataTable tempDT2 = new DataTable();

                    tempDT2 = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GETVALUE");

                    for (int j = 0; j < tempDT2.Rows.Count; j++)
                    {
                        //取得內外勤種類
                        String DutyUserClassify = tempDT2.Rows[j]["DUTYUSERCLASSIFY"].ToString();

                        //目前不是抓現職，而是抓產生印領清冊當下的
                        //string WORKP = o_DBFactory.ABC_toTest.vExecSQL("SELECT WORKP FROM B_BASE WHERE IDCARD='" + tempDT2.Rows[j]["MZ_ID"].ToString() + "'");

                        int REAL_HOUR = Convert.ToInt32(tempDT2.Rows[j]["MZ_REAL_HOUR"]); //實際時數
                        int OVERTIME_PAY = Convert.ToInt32(tempDT2.Rows[j]["MZ_OVERTIME_PAY"]); //總支付

                        if (DutyUserClassify == Enum_C_DUTYTABLE_PERSONAL_DUTYUSERCLASSIFY.Outter.ToString())
                        {
                            O_HCOUNT++; //本月支領人數；外勤；小時
                            O_CCOUNT++; //本月支領人數；外勤；金額

                            if (OVERTIME_PAY <= 8000) O_CASH08++;
                            else if (OVERTIME_PAY > 8000 && OVERTIME_PAY <= 9000) O_CASH09++;
                            else if (OVERTIME_PAY > 9000 && OVERTIME_PAY <= 10000) O_CASH10++;
                            else if (OVERTIME_PAY > 10000 && OVERTIME_PAY <= 11000) O_CASH11++;
                            else if (OVERTIME_PAY > 11000 && OVERTIME_PAY <= 12000) O_CASH12++;
                            else if (OVERTIME_PAY > 12000 && OVERTIME_PAY <= 13000) O_CASH13++;
                            else if (OVERTIME_PAY > 13000 && OVERTIME_PAY <= 14000) O_CASH14++;
                            else if (OVERTIME_PAY > 14000 && OVERTIME_PAY <= 15000) O_CASH15++;
                            else if (OVERTIME_PAY > 15000 && OVERTIME_PAY <= 16000) O_CASH16++;
                            else if (OVERTIME_PAY > 16000 && OVERTIME_PAY <= 17000) O_CASH17++;
                            else if (OVERTIME_PAY > 17000 && OVERTIME_PAY <= 18000) O_CASH18++;
                            else if (OVERTIME_PAY > 18000 && OVERTIME_PAY <= 19000) O_CASH19++;

                            O_CTOTAL = O_CTOTAL + OVERTIME_PAY;

                            if (REAL_HOUR < 31) O_HOUR01++;
                            else if (REAL_HOUR >= 31 && REAL_HOUR < 41) O_HOUR31++;
                            else if (REAL_HOUR >= 41 && REAL_HOUR < 51) O_HOUR41++;
                            else if (REAL_HOUR >= 51 && REAL_HOUR < 61) O_HOUR51++;
                            else if (REAL_HOUR >= 61 && REAL_HOUR < 71) O_HOUR61++;
                            else if (REAL_HOUR >= 71 && REAL_HOUR < 81) O_HOUR71++;
                            else if (REAL_HOUR >= 81 && REAL_HOUR < 91) O_HOUR81++;
                            else if (REAL_HOUR >= 91 && REAL_HOUR < 101) O_HOUR91++;

                        }
                        else
                        {
                            I_HCOUNT++;
                            I_CCOUNT++;

                            if (OVERTIME_PAY <= 8000) I_CASH08++;
                            else if (OVERTIME_PAY > 8000 && OVERTIME_PAY <= 9000) I_CASH09++;
                            else if (OVERTIME_PAY > 9000 && OVERTIME_PAY <= 10000) I_CASH10++;
                            else if (OVERTIME_PAY > 10000 && OVERTIME_PAY <= 11000) I_CASH11++;
                            else if (OVERTIME_PAY > 11000 && OVERTIME_PAY <= 12000) I_CASH12++;
                            else if (OVERTIME_PAY > 12000 && OVERTIME_PAY <= 13000) I_CASH13++;
                            else if (OVERTIME_PAY > 13000 && OVERTIME_PAY <= 14000) I_CASH14++;
                            else if (OVERTIME_PAY > 14000 && OVERTIME_PAY <= 15000) I_CASH15++;
                            else if (OVERTIME_PAY > 15000 && OVERTIME_PAY <= 16000) I_CASH16++;
                            else if (OVERTIME_PAY > 16000 && OVERTIME_PAY <= 17000) I_CASH17++;
                            else if (OVERTIME_PAY > 17000 && OVERTIME_PAY <= 18000) I_CASH18++;
                            else if (OVERTIME_PAY > 18000 && OVERTIME_PAY <= 19000) I_CASH19++;
                            else
                            {
                                TPPDDB.App_Code.Log.SaveLog(
                                     "超過19000 OVERTIME_PAY=" + OVERTIME_PAY.ToString()
                       );
                            }

                            I_CTOTAL = I_CTOTAL + OVERTIME_PAY;

                            if (REAL_HOUR < 31) I_HOUR01++;
                            else if (REAL_HOUR >= 31 && REAL_HOUR < 41) I_HOUR31++;
                            else if (REAL_HOUR >= 41 && REAL_HOUR < 51) I_HOUR41++;
                            else if (REAL_HOUR >= 51 && REAL_HOUR < 61) I_HOUR51++;
                            else if (REAL_HOUR >= 61 && REAL_HOUR < 71) I_HOUR61++;
                            else if (REAL_HOUR >= 71 && REAL_HOUR < 81) I_HOUR71++;
                            else if (REAL_HOUR >= 81 && REAL_HOUR < 91) I_HOUR81++;
                            else if (REAL_HOUR >= 91 && REAL_HOUR < 101) I_HOUR91++;
                        }
                        TPPDDB.App_Code.Log.SaveLog(
                           " I_CASH18=" + I_CASH18.ToString()
                      + " I_CASH19=" + I_CASH19.ToString()
                      + " O_CASH18=" + O_CASH18.ToString()
                      + " O_CASH19=" + O_CASH19.ToString()
                       );

                        /// 內外勤人員資料相反
                        DataRow dr = rpt_dt.NewRow();
                        dr["I_HCOUNT"] = I_HCOUNT;
                        dr["I_HOUR01"] = I_HOUR01;
                        dr["I_HOUR31"] = I_HOUR31;
                        dr["I_HOUR41"] = I_HOUR41;
                        dr["I_HOUR51"] = I_HOUR51;
                        dr["I_HOUR61"] = I_HOUR61;
                        dr["I_HOUR71"] = I_HOUR71;
                        dr["I_HOUR81"] = I_HOUR81;
                        dr["I_HOUR91"] = I_HOUR91;
                        dr["O_HCOUNT"] = O_HCOUNT;
                        dr["O_HOUR01"] = O_HOUR01;
                        dr["O_HOUR31"] = O_HOUR31;
                        dr["O_HOUR41"] = O_HOUR41;
                        dr["O_HOUR51"] = O_HOUR51;
                        dr["O_HOUR61"] = O_HOUR61;
                        dr["O_HOUR71"] = O_HOUR71;
                        dr["O_HOUR81"] = O_HOUR81;
                        dr["O_HOUR91"] = O_HOUR91;
                        dr["I_CCOUNT"] = I_CCOUNT;
                        dr["I_CASH08"] = I_CASH08;
                        dr["I_CASH09"] = I_CASH09;
                        dr["I_CASH10"] = I_CASH10;
                        dr["I_CASH11"] = I_CASH11;
                        dr["I_CASH12"] = I_CASH12;
                        dr["I_CASH13"] = I_CASH13;
                        dr["I_CASH14"] = I_CASH14;
                        dr["I_CASH15"] = I_CASH15;
                        dr["I_CASH16"] = I_CASH16;
                        dr["I_CASH17"] = I_CASH17;
                        dr["I_CASH18"] = I_CASH18;
                        dr["I_CASH19"] = I_CASH19;
                        dr["I_CTOTAL"] = I_CTOTAL;
                        dr["O_CCOUNT"] = O_CCOUNT;
                        dr["O_CASH08"] = O_CASH08;
                        dr["O_CASH09"] = O_CASH09;
                        dr["O_CASH10"] = O_CASH10;
                        dr["O_CASH11"] = O_CASH11;
                        dr["O_CASH12"] = O_CASH12;
                        dr["O_CASH13"] = O_CASH13;
                        dr["O_CASH14"] = O_CASH14;
                        dr["O_CASH15"] = O_CASH15;
                        dr["O_CASH16"] = O_CASH16;
                        dr["O_CASH17"] = O_CASH17;
                        dr["O_CASH18"] = O_CASH18;
                        dr["O_CASH19"] = O_CASH19;
                        dr["O_CTOTAL"] = O_CTOTAL;

                        //dr["I_HCOUNT"] = O_HCOUNT;
                        //dr["I_HOUR01"] = O_HOUR01;
                        //dr["I_HOUR31"] = O_HOUR31;
                        //dr["I_HOUR41"] = O_HOUR41;
                        //dr["I_HOUR51"] = O_HOUR51;
                        //dr["I_HOUR61"] = O_HOUR61;
                        //dr["I_HOUR71"] = O_HOUR71;
                        //dr["I_HOUR81"] = O_HOUR81;
                        //dr["I_HOUR91"] = O_HOUR91;
                        //dr["O_HCOUNT"] = I_HCOUNT;
                        //dr["O_HOUR01"] = I_HOUR01;
                        //dr["O_HOUR31"] = I_HOUR31;
                        //dr["O_HOUR41"] = I_HOUR41;
                        //dr["O_HOUR51"] = I_HOUR51;
                        //dr["O_HOUR61"] = I_HOUR61;
                        //dr["O_HOUR71"] = I_HOUR71;
                        //dr["O_HOUR81"] = I_HOUR81;
                        //dr["O_HOUR91"] = I_HOUR91;
                        //dr["I_CCOUNT"] = O_CCOUNT;
                        //dr["I_CASH08"] = O_CASH08;
                        //dr["I_CASH09"] = O_CASH09;
                        //dr["I_CASH10"] = O_CASH10;
                        //dr["I_CASH11"] = O_CASH11;
                        //dr["I_CASH12"] = O_CASH12;
                        //dr["I_CASH13"] = O_CASH13;
                        //dr["I_CASH14"] = O_CASH14;
                        //dr["I_CASH15"] = O_CASH15;
                        //dr["I_CASH16"] = O_CASH16;
                        //dr["I_CASH17"] = O_CASH17;
                        //dr["I_CTOTAL"] = O_CTOTAL;
                        //dr["O_CCOUNT"] = I_CCOUNT;
                        //dr["O_CASH08"] = I_CASH08;
                        //dr["O_CASH09"] = I_CASH09;
                        //dr["O_CASH10"] = I_CASH10;
                        //dr["O_CASH11"] = I_CASH11;
                        //dr["O_CASH12"] = I_CASH12;
                        //dr["O_CASH13"] = I_CASH13;
                        //dr["O_CASH14"] = I_CASH14;
                        //dr["O_CASH15"] = I_CASH15;
                        //dr["O_CASH16"] = I_CASH16;
                        //dr["O_CASH17"] = I_CASH17;
                        //dr["O_CTOTAL"] = I_CTOTAL;

                        dr["UNIT"] = MZ_UNIT_CH;
                        rpt_dt.Rows.Add(dr);

                    }

                    return rpt_dt;
                }

            }


            /// <summary>
            ///  6.3.3超勤支領情形分析表(小時) 這沒辦法用SQL兜出來嗎?????? 
            /// </summary>
            public class OvertimeOutSide_Hour
            {


                public static DataTable doSearch(string DATE, string MZ_AD)
                {

                    DataTable rpt_dt = new DataTable();

                    int I_HCOUNT = 0;
                    int O_HCOUNT = 0;
                    int I_HOUR01 = 0;
                    int I_HOUR31 = 0;
                    int I_HOUR41 = 0;
                    int I_HOUR51 = 0;
                    int I_HOUR61 = 0;
                    int I_HOUR71 = 0;
                    int I_HOUR81 = 0;
                    int I_HOUR91 = 0;
                    int O_HOUR01 = 0;
                    int O_HOUR31 = 0;
                    int O_HOUR41 = 0;
                    int O_HOUR51 = 0;
                    int O_HOUR61 = 0;
                    int O_HOUR71 = 0;
                    int O_HOUR81 = 0;
                    int O_HOUR91 = 0;
                    //int I_CCOUNT = 0;
                    //int O_CCOUNT = 0;
                    //int I_CASH08 = 0;
                    //int I_CASH09 = 0;
                    //int I_CASH10 = 0;
                    //int I_CASH11 = 0;
                    //int I_CASH12 = 0;
                    //int I_CASH13 = 0;
                    //int I_CASH14 = 0;
                    //int I_CASH15 = 0;
                    //int I_CASH16 = 0;
                    //int I_CASH17 = 0;
                    //int O_CASH08 = 0;
                    //int O_CASH09 = 0;
                    //int O_CASH10 = 0;
                    //int O_CASH11 = 0;
                    //int O_CASH12 = 0;
                    //int O_CASH13 = 0;
                    //int O_CASH14 = 0;
                    //int O_CASH15 = 0;
                    //int O_CASH16 = 0;
                    //int O_CASH17 = 0;

                    rpt_dt.Columns.Add("UNIT", typeof(string));
                    rpt_dt.Columns.Add("I_HCOUNT", typeof(string));
                    rpt_dt.Columns.Add("I_HOUR01", typeof(string));
                    rpt_dt.Columns.Add("I_HOUR31", typeof(string));
                    rpt_dt.Columns.Add("I_HOUR41", typeof(string));
                    rpt_dt.Columns.Add("I_HOUR51", typeof(string));
                    rpt_dt.Columns.Add("I_HOUR61", typeof(string));
                    rpt_dt.Columns.Add("I_HOUR71", typeof(string));
                    rpt_dt.Columns.Add("I_HOUR81", typeof(string));
                    rpt_dt.Columns.Add("I_HOUR91", typeof(string));
                    rpt_dt.Columns.Add("O_HCOUNT", typeof(string));
                    rpt_dt.Columns.Add("O_HOUR01", typeof(string));
                    rpt_dt.Columns.Add("O_HOUR31", typeof(string));
                    rpt_dt.Columns.Add("O_HOUR41", typeof(string));
                    rpt_dt.Columns.Add("O_HOUR51", typeof(string));
                    rpt_dt.Columns.Add("O_HOUR61", typeof(string));
                    rpt_dt.Columns.Add("O_HOUR71", typeof(string));
                    rpt_dt.Columns.Add("O_HOUR81", typeof(string));
                    rpt_dt.Columns.Add("O_HOUR91", typeof(string));


                    string selectSQL = @"SELECT MZ_UNIT, MZ_KCHI MZ_UNIT_CH FROM A_UNIT_AD 
                                     LEFT JOIN A_KTYPE ON MZ_KCODE = MZ_UNIT AND MZ_KTYPE='25'
                                     WHERE MZ_AD='" + MZ_AD + "' ORDER BY MZ_UNIT";

                    DataTable dtUnitList = new DataTable();
                    dtUnitList = o_DBFactory.ABC_toTest.Create_Table(selectSQL, "GETVALUE_AD");

                    if (MZ_AD == "382130000C")
                    {
                        TPPDDB._2_salary.Salary.movetoFirst(ref dtUnitList, 1, "副局長室");//副分局長室0327
                        TPPDDB._2_salary.Salary.movetoFirst(ref dtUnitList, 1, "局長室");//分局長室0326

                    }
                    else
                    {
                        TPPDDB._2_salary.Salary.movetoFirst(ref dtUnitList, 1, "副分局長室");//副分局長室0329
                        TPPDDB._2_salary.Salary.movetoFirst(ref dtUnitList, 1, "分局長室");//分局長室0328
                    }

                    //迴圈 : 依照某一機關下，逐一跑每一個單位
                    for (int iUnit = 0; iUnit < dtUnitList.Rows.Count; iUnit++)
                    {
                        String Unit = dtUnitList.Rows[iUnit]["MZ_UNIT"].ToString();

                        //抓取印領清冊資料表
                        //20151203 Neil 原先WHERE是從A_DLBASE撈資料，但應以超勤記錄當下為主，故改為WHERE C_DUTYMONTHOVERTIME_HOUR 內的條件
                        //因應刑大提出未編制的單位分類出現，原服務單位條件改成編制單位條件。 20180615 by sky
                        string strSQL = String.Format(@"SELECT MZ_REAL_HOUR, MZ_ID, DUTYUSERCLASSIFY
                                                        FROM C_DUTYMONTHOVERTIME_HOUR
                                                        WHERE MZ_YEAR='{0}'
                                                              AND MZ_MONTH='{1}' 
                                                              AND MZ_AD='{2}'
                                                              AND MZ_UNIT='{3}'",
                                                        DATE.Substring(0, 3),
                                                        DATE.Substring(3, 2),
                                                        MZ_AD, //函數帶入機關編號
                                                        Unit); //迴圈帶出此機關底下的單位

                        //使用者當月超勤資料
                        DataTable dtUserData = new DataTable();
                        dtUserData = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GETVALUE");

                        //迴圈 : 抓出該單位底下，每個人的印領清冊
                        for (int j = 0; j < dtUserData.Rows.Count; j++)
                        {
                            String DutyUserClassify = dtUserData.Rows[j]["DUTYUSERCLASSIFY"].ToString();
                            String ID = dtUserData.Rows[j]["MZ_ID"].ToString(); //身分證字號
                            Int32 REAL_HOUR = Convert.ToInt32(dtUserData.Rows[j]["MZ_REAL_HOUR"]); //實際時數
                            //String WORKP = o_DBFactory.ABC_toTest.vExecSQL(String.Format(@"SELECT WORKP FROM B_BASE WHERE IDCARD='{0}'", ID)); //取得個人薪資資料(B_BASE)內的警勤加給

                            //若為外勤
                            if (DutyUserClassify == Enum_C_DUTYTABLE_PERSONAL_DUTYUSERCLASSIFY.Outter.ToString())
                            {
                                O_HCOUNT++;
                                if (REAL_HOUR < 31)
                                    O_HOUR01++;
                                else if (REAL_HOUR >= 31 && REAL_HOUR < 41)
                                    O_HOUR31++;
                                else if (REAL_HOUR >= 41 && REAL_HOUR < 51)
                                    O_HOUR41++;
                                else if (REAL_HOUR >= 51 && REAL_HOUR < 61)
                                    O_HOUR51++;
                                else if (REAL_HOUR >= 61 && REAL_HOUR < 71)
                                    O_HOUR61++;
                                else if (REAL_HOUR >= 71 && REAL_HOUR < 81)
                                    O_HOUR71++;
                                else if (REAL_HOUR >= 81 && REAL_HOUR < 91)
                                    O_HOUR81++;
                                else if (REAL_HOUR >= 91 && REAL_HOUR < 101)
                                    O_HOUR91++;
                            }
                            else
                            {
                                I_HCOUNT++;
                                if (REAL_HOUR < 31)
                                    I_HOUR01++;
                                else if (REAL_HOUR >= 31 && REAL_HOUR < 41)
                                    I_HOUR31++;
                                else if (REAL_HOUR >= 41 && REAL_HOUR < 51)
                                    I_HOUR41++;
                                else if (REAL_HOUR >= 51 && REAL_HOUR < 61)
                                    I_HOUR51++;
                                else if (REAL_HOUR >= 61 && REAL_HOUR < 71)
                                    I_HOUR61++;
                                else if (REAL_HOUR >= 71 && REAL_HOUR < 81)
                                    I_HOUR71++;
                                else if (REAL_HOUR >= 81 && REAL_HOUR < 91)
                                    I_HOUR81++;
                                else if (REAL_HOUR >= 91 && REAL_HOUR < 101)
                                    I_HOUR91++;
                            }
                        }

                        DataRow dr = rpt_dt.NewRow();
                        /// Joy 如果內勤、外勤人員皆為0則不顯示該欄
                        if (I_HCOUNT == 0 && O_HCOUNT == 0)
                        { }
                        else
                        {
                            //dr["UNIT"] = tempDT.Rows[i]["MZ_UNIT_CH"].ToString();
                            dr["I_HCOUNT"] = I_HCOUNT;
                            dr["I_HOUR01"] = I_HOUR01;
                            dr["I_HOUR31"] = I_HOUR31;
                            dr["I_HOUR41"] = I_HOUR41;
                            dr["I_HOUR51"] = I_HOUR51;
                            dr["I_HOUR61"] = I_HOUR61;
                            dr["I_HOUR71"] = I_HOUR71;
                            dr["I_HOUR81"] = I_HOUR81;
                            dr["I_HOUR91"] = I_HOUR91;
                            dr["O_HCOUNT"] = O_HCOUNT;
                            dr["O_HOUR01"] = O_HOUR01;
                            dr["O_HOUR31"] = O_HOUR31;
                            dr["O_HOUR41"] = O_HOUR41;
                            dr["O_HOUR51"] = O_HOUR51;
                            dr["O_HOUR61"] = O_HOUR61;
                            dr["O_HOUR71"] = O_HOUR71;
                            dr["O_HOUR81"] = O_HOUR81;
                            dr["O_HOUR91"] = O_HOUR91;

                            dr["UNIT"] = dtUnitList.Rows[iUnit]["MZ_UNIT_CH"].ToString();
                            //dr["I_HCOUNT"] = O_HCOUNT;
                            //dr["I_HOUR01"] = O_HOUR01;
                            //dr["I_HOUR31"] = O_HOUR31;
                            //dr["I_HOUR41"] = O_HOUR41;
                            //dr["I_HOUR51"] = O_HOUR51;
                            //dr["I_HOUR61"] = O_HOUR61;
                            //dr["I_HOUR71"] = O_HOUR71;
                            //dr["I_HOUR81"] = O_HOUR81;
                            //dr["I_HOUR91"] = O_HOUR91;

                            //dr["O_HCOUNT"] = I_HCOUNT;
                            //dr["O_HOUR01"] = I_HOUR01;
                            //dr["O_HOUR31"] = I_HOUR31;
                            //dr["O_HOUR41"] = I_HOUR41;
                            //dr["O_HOUR51"] = I_HOUR51;
                            //dr["O_HOUR61"] = I_HOUR61;
                            //dr["O_HOUR71"] = I_HOUR71;
                            //dr["O_HOUR81"] = I_HOUR81;
                            //dr["O_HOUR91"] = I_HOUR91;

                            rpt_dt.Rows.Add(dr);

                        }

                        I_HCOUNT = 0;
                        I_HOUR01 = 0;
                        I_HOUR31 = 0;
                        I_HOUR41 = 0;
                        I_HOUR51 = 0;
                        I_HOUR61 = 0;
                        I_HOUR71 = 0;
                        I_HOUR81 = 0;
                        I_HOUR91 = 0;
                        O_HCOUNT = 0;
                        O_HOUR01 = 0;
                        O_HOUR31 = 0;
                        O_HOUR41 = 0;
                        O_HOUR51 = 0;
                        O_HOUR61 = 0;
                        O_HOUR71 = 0;
                        O_HOUR81 = 0;
                        O_HOUR91 = 0;
                    }



                    return rpt_dt;
                }

            }

            /// <summary>
            ///  6.3.4超勤支領情形分析表(金額) 這沒辦法用SQL兜出來嗎?????? 
            /// </summary>
            public class OvertimeOutSide_Cost
            {
                public static DataTable doSearch(string DATE, string MZ_AD)
                {
                    DataTable rpt_dt = new DataTable();


                    int I_CCOUNT = 0;
                    int O_CCOUNT = 0;
                    int I_CASH08 = 0;
                    int I_CASH09 = 0;
                    int I_CASH10 = 0;
                    int I_CASH11 = 0;
                    int I_CASH12 = 0;
                    int I_CASH13 = 0;
                    int I_CASH14 = 0;
                    int I_CASH15 = 0;
                    int I_CASH16 = 0;
                    int I_CASH17 = 0;
                    int I_CTOTAL = 0;
                    int O_CASH08 = 0;
                    int O_CASH09 = 0;
                    int O_CASH10 = 0;
                    int O_CASH11 = 0;
                    int O_CASH12 = 0;
                    int O_CASH13 = 0;
                    int O_CASH14 = 0;
                    int O_CASH15 = 0;
                    int O_CASH16 = 0;
                    int O_CASH17 = 0;
                    int O_CTOTAL = 0;


                    rpt_dt.Columns.Add("UNIT", typeof(string));
                    rpt_dt.Columns.Add("I_CCOUNT", typeof(string));
                    rpt_dt.Columns.Add("I_CASH08", typeof(string));
                    rpt_dt.Columns.Add("I_CASH09", typeof(string));
                    rpt_dt.Columns.Add("I_CASH10", typeof(string));
                    rpt_dt.Columns.Add("I_CASH11", typeof(string));
                    rpt_dt.Columns.Add("I_CASH12", typeof(string));
                    rpt_dt.Columns.Add("I_CASH13", typeof(string));
                    rpt_dt.Columns.Add("I_CASH14", typeof(string));
                    rpt_dt.Columns.Add("I_CASH15", typeof(string));
                    rpt_dt.Columns.Add("I_CASH16", typeof(string));
                    rpt_dt.Columns.Add("I_CASH17", typeof(string));
                    rpt_dt.Columns.Add("I_CTOTAL", typeof(string));
                    rpt_dt.Columns.Add("O_CCOUNT", typeof(string));
                    rpt_dt.Columns.Add("O_CASH08", typeof(string));
                    rpt_dt.Columns.Add("O_CASH09", typeof(string));
                    rpt_dt.Columns.Add("O_CASH10", typeof(string));
                    rpt_dt.Columns.Add("O_CASH11", typeof(string));
                    rpt_dt.Columns.Add("O_CASH12", typeof(string));
                    rpt_dt.Columns.Add("O_CASH13", typeof(string));
                    rpt_dt.Columns.Add("O_CASH14", typeof(string));
                    rpt_dt.Columns.Add("O_CASH15", typeof(string));
                    rpt_dt.Columns.Add("O_CASH16", typeof(string));
                    rpt_dt.Columns.Add("O_CASH17", typeof(string));
                    rpt_dt.Columns.Add("O_CTOTAL", typeof(string));

                    string selectSQL = @"SELECT MZ_UNIT, MZ_KCHI MZ_UNIT_CH FROM A_UNIT_AD 
                                     LEFT JOIN A_KTYPE ON MZ_KCODE = MZ_UNIT AND MZ_KTYPE='25'
                                     WHERE MZ_AD='" + MZ_AD + "' ORDER BY MZ_UNIT";

                    DataTable tempDT = new DataTable();

                    tempDT = o_DBFactory.ABC_toTest.Create_Table(selectSQL, "GETVALUE_AD");


                    if (MZ_AD == "382130000C")
                    {
                        TPPDDB._2_salary.Salary.movetoFirst(ref tempDT, 1, "副局長室");//副分局長室0327
                        TPPDDB._2_salary.Salary.movetoFirst(ref tempDT, 1, "局長室");//分局長室0326

                    }
                    else
                    {
                        TPPDDB._2_salary.Salary.movetoFirst(ref tempDT, 1, "副分局長室");//副分局長室0329
                        TPPDDB._2_salary.Salary.movetoFirst(ref tempDT, 1, "分局長室");//分局長室0328
                    }

                    for (int i = 0; i < tempDT.Rows.Count; i++)
                    {
                        //20151203 Neil 原先WHERE是從A_DLBASE撈資料，但應以超勤記錄當下為主，故改為WHERE C_DUTYMONTHOVERTIME_HOUR 內的條件
                        //因應刑大提出未編制的單位分類出現，原服務單位條件改成編制單位條件。 20180615 by sky
                        string strSQL = String.Format(@"SELECT MZ_REAL_HOUR
                                                             , MZ_ID
                                                             , MZ_OVERTIME_PAY
                                                             , DutyUserClassify
                                                          FROM C_DUTYMONTHOVERTIME_HOUR
                                                         WHERE MZ_YEAR='{0}'
                                                               AND MZ_MONTH='{1}' 
                                                               AND MZ_AD='{2}' 
                                                               AND MZ_UNIT='{3}'"
                                                        , DATE.Substring(0, 3)
                                                        , DATE.Substring(3, 2)
                                                        , MZ_AD
                                                        , tempDT.Rows[i]["MZ_UNIT"].ToString());

                        DataTable tempDT2 = new DataTable();

                        tempDT2 = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GETVALUE");

                        for (int j = 0; j < tempDT2.Rows.Count; j++)
                        {
                            String DutyUserClassify = tempDT2.Rows[j]["DUTYUSERCLASSIFY"].ToString();
                            string WORKP = o_DBFactory.ABC_toTest.vExecSQL("SELECT WORKP FROM B_BASE WHERE IDCARD='" + tempDT2.Rows[j]["MZ_ID"].ToString() + "'");

                            int REAL_HOUR = Convert.ToInt32(tempDT2.Rows[j]["MZ_REAL_HOUR"]);

                            int OVERTIME_PAY = Convert.ToInt32(tempDT2.Rows[j]["MZ_OVERTIME_PAY"]);

                            if (DutyUserClassify == Enum_C_DUTYTABLE_PERSONAL_DUTYUSERCLASSIFY.Outter.ToString())
                            {
                                O_CCOUNT++;

                                if (OVERTIME_PAY <= 8000) O_CASH08++;
                                else if (OVERTIME_PAY > 8000 && OVERTIME_PAY <= 9000) O_CASH09++;
                                else if (OVERTIME_PAY > 9000 && OVERTIME_PAY <= 10000) O_CASH10++;
                                else if (OVERTIME_PAY > 10000 && OVERTIME_PAY <= 11000) O_CASH11++;
                                else if (OVERTIME_PAY > 11000 && OVERTIME_PAY <= 12000) O_CASH12++;
                                else if (OVERTIME_PAY > 12000 && OVERTIME_PAY <= 13000) O_CASH13++;
                                else if (OVERTIME_PAY > 13000 && OVERTIME_PAY <= 14000) O_CASH14++;
                                else if (OVERTIME_PAY > 14000 && OVERTIME_PAY <= 15000) O_CASH15++;
                                else if (OVERTIME_PAY > 15000 && OVERTIME_PAY <= 16000) O_CASH16++;
                                else if (OVERTIME_PAY > 16000 && OVERTIME_PAY <= 17000) O_CASH17++;

                                O_CTOTAL = O_CTOTAL + OVERTIME_PAY;
                            }
                            else
                            {
                                I_CCOUNT++;

                                if (OVERTIME_PAY <= 8000) I_CASH08++;
                                else if (OVERTIME_PAY > 8000 && OVERTIME_PAY <= 9000) I_CASH09++;
                                else if (OVERTIME_PAY > 9000 && OVERTIME_PAY <= 10000) I_CASH10++;
                                else if (OVERTIME_PAY > 10000 && OVERTIME_PAY <= 11000) I_CASH11++;
                                else if (OVERTIME_PAY > 11000 && OVERTIME_PAY <= 12000) I_CASH12++;
                                else if (OVERTIME_PAY > 12000 && OVERTIME_PAY <= 13000) I_CASH13++;
                                else if (OVERTIME_PAY > 13000 && OVERTIME_PAY <= 14000) I_CASH14++;
                                else if (OVERTIME_PAY > 14000 && OVERTIME_PAY <= 15000) I_CASH15++;
                                else if (OVERTIME_PAY > 15000 && OVERTIME_PAY <= 16000) I_CASH16++;
                                else if (OVERTIME_PAY > 16000 && OVERTIME_PAY <= 17000) I_CASH17++;


                                I_CTOTAL = I_CTOTAL + OVERTIME_PAY;
                            }
                        }

                        DataRow dr = rpt_dt.NewRow();
                        /// Joy 若外勤、內勤人員數目為0則該欄不顯示
                        if (I_CCOUNT == 0 && O_CCOUNT == 0)
                        { }
                        else
                        {
                            dr["UNIT"] = tempDT.Rows[i]["MZ_UNIT_CH"].ToString();
                            dr["I_CCOUNT"] = I_CCOUNT;
                            dr["I_CASH08"] = I_CASH08;
                            dr["I_CASH09"] = I_CASH09;
                            dr["I_CASH10"] = I_CASH10;
                            dr["I_CASH11"] = I_CASH11;
                            dr["I_CASH12"] = I_CASH12;
                            dr["I_CASH13"] = I_CASH13;
                            dr["I_CASH14"] = I_CASH14;
                            dr["I_CASH15"] = I_CASH15;
                            dr["I_CASH16"] = I_CASH16;
                            dr["I_CASH17"] = I_CASH17;
                            dr["I_CTOTAL"] = I_CTOTAL;
                            dr["O_CCOUNT"] = O_CCOUNT;
                            dr["O_CASH08"] = O_CASH08;
                            dr["O_CASH09"] = O_CASH09;
                            dr["O_CASH10"] = O_CASH10;
                            dr["O_CASH11"] = O_CASH11;
                            dr["O_CASH12"] = O_CASH12;
                            dr["O_CASH13"] = O_CASH13;
                            dr["O_CASH14"] = O_CASH14;
                            dr["O_CASH15"] = O_CASH15;
                            dr["O_CASH16"] = O_CASH16;
                            dr["O_CASH17"] = O_CASH17;
                            dr["O_CTOTAL"] = O_CTOTAL;
                            rpt_dt.Rows.Add(dr);
                        }

                        I_CCOUNT = 0;
                        O_CCOUNT = 0;
                        I_CASH08 = 0;
                        I_CASH09 = 0;
                        I_CASH10 = 0;
                        I_CASH11 = 0;
                        I_CASH12 = 0;
                        I_CASH13 = 0;
                        I_CASH14 = 0;
                        I_CASH15 = 0;
                        I_CASH16 = 0;
                        I_CASH17 = 0;
                        I_CTOTAL = 0;
                        O_CASH08 = 0;
                        O_CASH09 = 0;
                        O_CASH10 = 0;
                        O_CASH11 = 0;
                        O_CASH12 = 0;
                        O_CASH13 = 0;
                        O_CASH14 = 0;
                        O_CASH15 = 0;
                        O_CASH16 = 0;
                        O_CASH17 = 0;
                        O_CTOTAL = 0;

                    }
                    return rpt_dt;
                }
            }

        }

        #endregion 超勤報表

        /// <summary>
        /// 11.9.4指紋明細表
        /// </summary>       
        public class fingerdetail
        {

            //static DataTable temp_DT = new DataTable();

            public static DataTable doSearch(string DATE, string DATE2, string MZ_EXAD, string MZ_EXUNIT, string MZ_ID)
            {
                DataTable temp_DT = new DataTable();


                temp_DT.Columns.Clear();

                temp_DT.Columns.Add("OCCC", typeof(string));
                temp_DT.Columns.Add("MZ_NAME", typeof(string));
                temp_DT.Columns.Add("LOGDATE", typeof(string));
                //temp_DT.Columns.Add("WORKINTIME", typeof(string));
                //temp_DT.Columns.Add("WORKOUTTIME", typeof(string));

                //請假
                temp_DT.Columns.Add("HolidayLOGDATE", typeof(string));
                temp_DT.Columns.Add("HolidayINTIME", typeof(string));
                temp_DT.Columns.Add("HolidayOUTTIME", typeof(string));

                //超勤
                //temp_DT.Columns.Add("OVERITEM", typeof(string));
                temp_DT.Columns.Add("OVERINTIME", typeof(string));
                temp_DT.Columns.Add("OVEROUTTIME", typeof(string));

                //值日
                //temp_DT.Columns.Add("DAYITEM", typeof(string));
                temp_DT.Columns.Add("DAYINTIME", typeof(string));
                temp_DT.Columns.Add("DAYOUTTIME", typeof(string));
                temp_DT.Columns.Add("DAYSECOND", typeof(string));

                //加班
                temp_DT.Columns.Add("ADDINTIME", typeof(string));
                temp_DT.Columns.Add("ADDOUTTIME", typeof(string));

                temp_DT.Columns.Add("Memo", typeof(string));
                temp_DT.Columns.Add("KIND", typeof(string));

                temp_DT.Columns.Add("MZ_TBDV", typeof(string));


                string strSQL = "";
                string SQLUNIT = "";
                string SQLDATE = "";

                string date1 = "";
                string date2 = "";

                string begindate = "";
                string enddate = "";

                temp_DT.Rows.Clear();

                if (DATE != string.Empty && DATE2 != string.Empty)
                {
                    date1 = o_str.tosql(DATE.Replace("/", string.Empty).PadLeft(5, '0'));
                    begindate = (int.Parse(date1.Substring(0, 3)) + 1911).ToString() + "/" + date1.Substring(3, 2) + "/" + date1.Substring(5, 2);
                    date2 = o_str.tosql(DATE2.Replace("/", string.Empty).PadLeft(5, '0'));
                    enddate = (int.Parse(date2.Substring(0, 3)) + 1911).ToString() + "/" + date2.Substring(3, 2) + "/" + date2.Substring(5, 2);

                    SQLDATE = " AND LOGDATE>='" + begindate + "' AND LOGDATE<='" + enddate + "'";
                }
                if (MZ_EXUNIT.Trim() != string.Empty)
                {
                    SQLUNIT = " AND MZ_EXUNIT='" + MZ_EXUNIT + "'";
                }

                //strSQL = "SELECT MZ_ID,MZ_OCCC AS OCCC,dbo.to_number(CASE WHEN MZ_TBDV IS NULL THEN '999' WHEN MZ_TBDV='Z99' THEN '999' ELSE MZ_TBDV END) AS TBDV FROM A_DLBASE WHERE MZ_EXAD='" + MZ_EXAD + "'" + SQLUNIT + " AND MZ_ID IN (SELECT MZ_ID FROM C_CARDSET WHERE MZ_CLOCK='Y') ORDER BY TBDV,OCCC";
                if (MZ_ID != "")
                {
                    strSQL = "SELECT MZ_ID,MZ_OCCC AS OCCC,dbo.to_number(CASE WHEN MZ_TBDV IS NULL THEN '999' WHEN MZ_TBDV='Z99' THEN '999' ELSE MZ_TBDV END) AS TBDV FROM A_DLBASE WHERE MZ_EXAD='" + MZ_EXAD + "'" + SQLUNIT + " AND MZ_ID IN (SELECT MZ_ID FROM C_CARDSET WHERE MZ_CLOCK='Y') AND MZ_ID='" + MZ_ID + "'" + " ORDER BY TBDV,OCCC";
                }
                else
                {
                    strSQL = "SELECT MZ_ID,MZ_OCCC AS OCCC,dbo.to_number(CASE WHEN MZ_TBDV IS NULL THEN '999' WHEN MZ_TBDV='Z99' THEN '999' ELSE MZ_TBDV END) AS TBDV FROM A_DLBASE WHERE MZ_EXAD='" + MZ_EXAD + "'" + SQLUNIT + " AND MZ_ID IN (SELECT MZ_ID FROM C_CARDSET WHERE MZ_CLOCK='Y') ORDER BY TBDV,OCCC";
                }
                DataTable tempDT = new DataTable();

                tempDT = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GETID");

                DateTime TS1 = DateTime.Parse(begindate);
                DateTime TS2 = DateTime.Parse(enddate);

                TimeSpan TS = TS2 - TS1;

                // int personum = 0;
                for (int i = 0; i < tempDT.Rows.Count; i++)
                {
                    for (int j = 0; j <= TS.Days; j++)
                    {
                        fingerdetail a = new fingerdetail();
                        DataRow drrr = a.Count_Card_Record(tempDT.Rows[i]["MZ_ID"].ToString(), TS1.AddDays(j), temp_DT);
                        temp_DT.Rows.Add(drrr);
                    }

                }

                DataTable dtCopy = temp_DT.Copy();

                DataView dv = temp_DT.DefaultView;
                dv.Sort = "LOGDATE,MZ_TBDV";
                dtCopy = dv.ToTable();


                return dtCopy;
            }


            protected DataRow Count_Card_Record(string MZ_ID, DateTime DATE, DataTable temp_DT)
            {
                int qINTIME = 90100, qOUTTIME = 170000;

                if (DATE >= Convert.ToDateTime("2021/05/17") && DATE <= Convert.ToDateTime("2021/07/26"))
                {
                    qINTIME = 100100;
                    qOUTTIME = 160000;
                }

                if (DATE >= Convert.ToDateTime("2021/07/27") && DATE <= Convert.ToDateTime("2022/12/31"))
                {
                    qINTIME = 093100;
                    qOUTTIME = 163000;
                }

                string LOGDATE = DATE.Year.ToString() + "/" + DATE.Month.ToString().PadLeft(2, '0') + "/" + DATE.Day.ToString().PadLeft(2, '0');
                string LOGDATE1 = (DATE.Year - 1911).ToString().PadLeft(3, '0') + DATE.Month.ToString().PadLeft(2, '0') + DATE.Day.ToString().PadLeft(2, '0');

                List<String> temp = new List<string>();

                temp = C_CountCardRecord.list_Abnormal(MZ_ID, LOGDATE);

                string selectINTIME = o_DBFactory.ABC_toTest.vExecSQL("SELECT LOGTIME FROM (SELECT LOGTIME,ROW_NUMBER() OVER(ORDER BY LOGTIME) AS ROWCOUNT FROM C_CARDHISTORY_NEW WHERE dbo.SUBSTR(USERNAME,0,10)='" + o_str.tosql(MZ_ID.Trim()) + "' AND LOGDATE='" + LOGDATE + "' AND VERIFY='IN') WHERE ROWCOUNT=1");
                string selectOUTTIME = o_DBFactory.ABC_toTest.vExecSQL("SELECT LOGTIME FROM (SELECT LOGTIME,ROW_NUMBER() OVER(ORDER BY LOGTIME DESC) AS ROWCOUNT FROM C_CARDHISTORY_NEW WHERE dbo.SUBSTR(USERNAME,0,10)='" + o_str.tosql(MZ_ID.Trim()) + "' AND LOGDATE='" + LOGDATE + "' AND VERIFY='IN') WHERE ROWCOUNT=1");
                //string selectINOVERTIME = o_DBFactory.ABC_toTest.vExecSQL("SELECT LOGTIME FROM (SELECT LOGTIME,ROW_NUMBER() OVER(ORDER BY LOGTIME) AS ROWCOUNT FROM C_CARDHISTORY_NEW WHERE USERID='" + o_str.tosql(MZ_ID.Trim()) + "' AND LOGDATE='" + LOGDATE + "' AND FKEY='F3' ) WHERE ROWCOUNT=1");
                //string selectOUTOVERTIME = o_DBFactory.ABC_toTest.vExecSQL("SELECT LOGTIME FROM (SELECT LOGTIME,ROW_NUMBER() OVER(ORDER BY LOGTIME DESC) AS ROWCOUNT FROM C_CARDHISTORY_NEW WHERE USERID='" + o_str.tosql(MZ_ID.Trim()) + "' AND LOGDATE='" + LOGDATE + "' AND FKEY='F3' ) WHERE ROWCOUNT=1");

                string KIND = "";
                string MEMO = "";
                string CODE = "";
                string AddINTIME = "";
                string AddOUTTIME = "";
                string OverINTIME = "";
                string OverOUTTIME = "";
                #region 正常上下班和請假

                //正常上下班打卡
                if (string.IsNullOrEmpty(selectINTIME) && string.IsNullOrEmpty(selectOUTTIME))
                {
                    if (DateTime.Parse(LOGDATE).DayOfWeek == DayOfWeek.Saturday || DateTime.Parse(LOGDATE).DayOfWeek == DayOfWeek.Sunday)
                    {


                        MEMO = "例假日";
                    }
                    //else if (temp.Count == 0) ///無刷卡狀態
                    //{
                    //    if (DATE.Year.ToString() + DATE.Month.ToString() + DATE.Day.ToString() == DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString())
                    //    {
                    //        int NOWTIME = int.Parse(DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString().PadLeft(2, '0'));//+ DateTime.Now.Second.ToString().PadLeft(2, '0'));

                    //        if (NOWTIME <= 90100)///本日九點前
                    //        {
                    //            KIND = "";
                    //            MEMO = "";
                    //        }
                    //        else if (NOWTIME > 90100)///本日九點後
                    //        {
                    //            KIND = "上班未刷卡";
                    //            MEMO = "上班異常";
                    //        }
                    //    }
                    //    else///過本日後
                    //    {
                    //        KIND = "未刷卡";
                    //        MEMO = "上班異常";
                    //    }
                    //}
                    else
                    {
                        if (temp.Count > 1)///有請假
                        {
                            CODE = temp[0];

                            if (int.Parse(LOGDATE1) >= int.Parse(temp[1]) && int.Parse(LOGDATE1) <= int.Parse(temp[3]) && int.Parse(temp[6]) > 0)///正常
                            {
                                KIND = "";
                                MEMO = "請假";
                            }
                            //else
                            //{
                            //    KIND = "未刷卡";
                            //    MEMO = "上班異常";
                            //}
                        }
                        else if (temp.Count == 1)
                        {
                            KIND = "";
                            MEMO = "國定假日";
                        }
                    }
                }
                else
                {
                    if (DateTime.Parse(LOGDATE).DayOfWeek == DayOfWeek.Saturday || DateTime.Parse(LOGDATE).DayOfWeek == DayOfWeek.Sunday)
                    {


                        MEMO = "例假日";
                    }
                    else if (DATE.Year.ToString() + DATE.Month.ToString() + DATE.Day.ToString() == DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString())///當日
                    {
                        if (int.Parse(selectINTIME.Replace(":", string.Empty)) <= qINTIME)///當日刷9點前的卡
                        {
                            if (selectINTIME == selectOUTTIME && temp.Count == 0)///只有一筆刷卡資料而且無請假
                            {
                                KIND = "";
                                MEMO = "";
                                selectOUTTIME = "";
                            }
                            else if (selectINTIME == selectOUTTIME && temp.Count > 1)///只有一筆刷卡資料而且有請假
                            {
                                selectOUTTIME = "";

                                CODE = temp[0];

                                DateTime TS1 = DateTime.Parse(LOGDATE + " " + temp[2] + ":00");///跟請假起時比

                                if (DateTime.Now > TS1)///超過就算下班未刷卡
                                {
                                    selectOUTTIME = "";
                                    //KIND = "下班未刷卡";
                                    //MEMO = "上班異常";
                                }

                            }
                            else if (int.Parse(selectOUTTIME.Replace(":", string.Empty)) < qINTIME)
                            {
                                KIND = "";
                                MEMO = "";
                                selectOUTTIME = "";
                            }
                            else if (int.Parse(selectOUTTIME.Replace(":", string.Empty)) < qOUTTIME)///下班時間少於17點
                            {
                                if (temp.Count > 1)///有請假
                                {
                                    CODE = temp[0];

                                    DateTime TS1 = DateTime.Parse(LOGDATE + " " + selectINTIME);

                                    DateTime TS2 = DateTime.Parse(LOGDATE + " " + selectOUTTIME);

                                    TimeSpan TS = TS2 - TS1;

                                    if (TS.Hours + int.Parse(temp[6]) >= 8)
                                    {
                                        MEMO = "請假";
                                    }
                                    //else
                                    //{
                                    //    KIND = "早退";
                                    //    MEMO = "上班異常";
                                    //}
                                }
                                //else
                                //{
                                //    KIND = "早退";
                                //    MEMO = "上班異常";
                                //}
                            }
                            else ///下班時間大於17點
                            {
                                if (temp.Count > 1)
                                {
                                    CODE = temp[0];
                                }

                                DateTime TS1 = DateTime.Parse(LOGDATE + " " + selectINTIME);

                                DateTime TS2 = DateTime.Parse(LOGDATE + " " + selectOUTTIME);

                                TimeSpan TS = TS2 - TS1;

                                if (TS.Hours > 8)
                                {
                                    MEMO = "";
                                }
                                //else
                                //{
                                //    KIND = "早退";
                                //    MEMO = "上班異常";
                                //}
                            }
                        }
                        else if (int.Parse(selectINTIME.Replace(":", string.Empty)) > qINTIME)///當天刷卡時間大於9點1分
                        {
                            if (selectINTIME == selectOUTTIME && temp.Count == 0)///只有一筆刷卡紀錄
                            {
                                if (int.Parse(selectINTIME.Replace(":", string.Empty)) > qOUTTIME)///記錄大於17點
                                {
                                    //KIND = "上班未刷卡";
                                    //MEMO = "上班異常";
                                    selectINTIME = "";
                                }
                                else
                                {
                                    //KIND = "遲到";
                                    //MEMO = "上班異常";
                                    selectOUTTIME = "";
                                }
                            }
                            else if (selectINTIME == selectOUTTIME && temp.Count > 1)///只有一筆刷卡紀錄有請假
                            {
                                CODE = temp[0];

                                if (int.Parse(temp[2].Replace(":", "")) <= 900 && int.Parse(temp[4].Replace(":", "")) <= 1330)
                                {
                                    if (int.Parse(selectINTIME.Replace(":", string.Empty)) < 133000)///未超過1330刷卡
                                    {
                                        KIND = "";
                                        MEMO = "";
                                        selectOUTTIME = "";
                                    }
                                    else///超過1330
                                    {
                                        // KIND = "遲到";
                                        // MEMO = "上班異常";
                                        selectOUTTIME = "";
                                    }
                                }
                                else if (int.Parse(temp[2].Replace(":", "")) <= 900)
                                {
                                    if (int.Parse(selectINTIME.Replace(":", string.Empty)) < int.Parse(temp[4].Replace(":", string.Empty) + "00"))
                                    {
                                        if (int.Parse(selectINTIME.Replace(":", string.Empty)) > 173000)
                                        {
                                            // KIND = "上班未刷卡";
                                            // MEMO = "上班異常";
                                            selectINTIME = "";
                                        }
                                        else
                                        {
                                            // KIND = "下班未刷卡";
                                            // MEMO = "上班異常";
                                            selectOUTTIME = "";
                                        }
                                    }
                                    else
                                    {
                                        // KIND = "遲到";
                                        // MEMO = "上班異常";
                                        selectOUTTIME = "";
                                    }
                                }
                                else
                                {
                                    //KIND = "遲到";
                                    // MEMO = "上班異常";
                                    selectOUTTIME = "";
                                }
                            }
                            else if (int.Parse(selectOUTTIME.Replace(":", string.Empty)) < 180000)
                            {
                                if (temp.Count > 1)
                                {
                                    CODE = temp[0];

                                    DateTime TS1 = DateTime.Parse(LOGDATE + " " + temp[2] + ":00");

                                    DateTime TS2 = DateTime.Parse(LOGDATE + " " + selectOUTTIME);

                                    TimeSpan TS = TS1 - TS2;

                                    if (TS.Hours + int.Parse(temp[6]) < 8)
                                    {
                                        // KIND = "遲到";
                                        //MEMO = "上班異常";
                                    }
                                    else
                                    {
                                        // KIND = "遲到早退";
                                        //MEMO = "上班異常";
                                    }
                                }
                                else
                                {
                                    //KIND = "遲到早退";
                                    // MEMO = "上班異常";
                                }
                            }
                            else
                            {
                                if (temp.Count > 1)
                                {
                                    CODE = temp[0];

                                    MEMO = "";
                                }
                                else
                                {
                                    //   KIND = "遲到";
                                    //  MEMO = "上班異常";
                                }
                            }

                        }
                    }
                    else
                    {
                        if (int.Parse(selectINTIME.Replace(":", string.Empty)) <= qINTIME)///非當日刷卡時間未超過9點1分
                        {
                            if (selectINTIME == selectOUTTIME && temp.Count == 0)///只有一筆刷卡資料
                            {
                                // KIND = "下班未刷卡";
                                // MEMO = "上班異常";
                                selectOUTTIME = "";
                            }
                            else if (int.Parse(selectOUTTIME.Replace(":", string.Empty)) < qINTIME)///多筆資料都在九點前
                            {
                                // KIND = "下班未刷卡";
                                // MEMO = "上班異常";
                                selectOUTTIME = "";
                            }
                            else if (int.Parse(selectOUTTIME.Replace(":", string.Empty)) < qOUTTIME)///下班刷卡時間早於5點
                            {
                                if (temp.Count > 1) ///有請假
                                {
                                    CODE = temp[0];

                                    DateTime TS1 = DateTime.Parse(LOGDATE + " " + selectINTIME);

                                    DateTime TS2 = DateTime.Parse(LOGDATE + " " + selectOUTTIME);

                                    TimeSpan TS = TS2 - TS1;

                                    if (TS.Hours + int.Parse(temp[6]) >= 8)
                                    {
                                        MEMO = "下午請假";
                                    }
                                    else
                                    {
                                        KIND = "早退";
                                        // MEMO = "上班異常";
                                    }
                                }
                                else
                                {
                                    // KIND = "早退";
                                    // MEMO = "上班異常";
                                }
                            }
                            else
                            {
                                DateTime TS1 = DateTime.Parse(LOGDATE + " " + selectINTIME);

                                DateTime TS2 = DateTime.Parse(LOGDATE + " " + selectOUTTIME);

                                TimeSpan TS = TS2 - TS1;

                                if (TS.Hours > 8)
                                {
                                    MEMO = "";
                                }
                                else
                                {
                                    //  KIND = "早退";
                                    //  MEMO = "上班異常";
                                }
                            }
                        }
                        else if (int.Parse(selectINTIME.Replace(":", string.Empty)) > qINTIME)///上班刷卡時間大於9點1分
                        {
                            if (selectINTIME == selectOUTTIME && temp.Count == 0)///只一筆刷卡資料
                            {
                                if (int.Parse(selectINTIME.Replace(":", string.Empty)) > qOUTTIME)
                                {
                                    //  KIND = "上班未刷卡";
                                    // MEMO = "上班異常";
                                    selectINTIME = "";
                                }
                                else
                                {
                                    // KIND = "遲到下班未刷卡";
                                    // MEMO = "上班異常";
                                    selectINTIME = "";
                                }
                            }
                            else if (selectINTIME == selectOUTTIME && temp.Count > 1)///只一筆有請假
                            {
                                CODE = temp[0];

                                if (temp[2] == "08:30" && temp[4] == "12:30")
                                {

                                    if (int.Parse(selectINTIME.Replace(":", string.Empty)) < 133000)
                                    {
                                        // KIND = "下班未刷卡";
                                        // MEMO = "上班異常";
                                        selectOUTTIME = "";
                                    }
                                    else if (int.Parse(selectINTIME.Replace(":", string.Empty)) >= 173000)
                                    {
                                        // KIND = "上班未刷卡";
                                        // MEMO = "上班異常";
                                        selectINTIME = "";
                                    }
                                    else
                                    {
                                        //KIND = "遲到下班未刷卡";
                                        // MEMO = "上班異常";
                                        selectOUTTIME = "";
                                    }

                                }
                                else if (int.Parse(temp[2].Replace(":", "")) <= 900)
                                {
                                    if (int.Parse(selectINTIME.Replace(":", string.Empty)) < int.Parse(temp[4].Replace(":", string.Empty) + "00"))
                                    {
                                        // KIND = "下班未刷卡";
                                        // MEMO = "上班異常";
                                        selectOUTTIME = "";
                                    }
                                    else
                                    {
                                        // KIND = "遲到下班未刷卡";
                                        // MEMO = "上班異常";
                                        selectOUTTIME = "";
                                    }
                                }
                                //else
                                //{
                                //    KIND = "遲到下班未刷卡";
                                //    MEMO = "上班異常";
                                //    selectOUTTIME = "";
                                //}
                            }
                            else if (int.Parse(selectOUTTIME.Replace(":", string.Empty)) < 180000)
                            {
                                if (temp.Count > 1)
                                {
                                    CODE = temp[0];

                                    DateTime TS1 = DateTime.Parse(LOGDATE + " " + selectINTIME);

                                    DateTime TS2 = DateTime.Parse(LOGDATE + " " + selectOUTTIME);

                                    TimeSpan TS = TS2 - TS1;

                                    if (TS.Hours + int.Parse(temp[6]) >= 8)
                                    {
                                        KIND = "";
                                        MEMO = "早上請假";

                                    }
                                    //else
                                    //{
                                    //    KIND = "遲到早退";
                                    //    MEMO = "上班異常";
                                    //}
                                }

                            }
                            else
                            {
                                if (temp.Count > 1)
                                {
                                    CODE = temp[0];

                                    // MEMO = "正常";
                                }
                                else
                                {
                                    //KIND = "遲到";
                                    //MEMO = "上班異常";
                                }
                            }
                        }
                        //else if (string.IsNullOrEmpty(selectINTIME))
                        //{
                        //    if (int.Parse(selectOUTTIME.Replace(":", string.Empty)) < 170000)
                        //    {
                        //        KIND = "遲到早退上班未刷卡";

                        //        MEMO = "上班異常";
                        //    }
                        //    else
                        //    {
                        //        KIND = "遲到上班未刷卡";

                        //        MEMO = "上班異常";
                        //    }
                        //}
                    }
                }

                #endregion 正常上下班和請假

                #region 加班
                //加班

                AddINTIME = o_DBFactory.ABC_toTest.vExecSQL("SELECT LOGTIME FROM (SELECT LOGTIME,ROW_NUMBER() OVER(ORDER BY LOGTIME) AS ROWCOUNT FROM C_CARDHISTORY_NEW WHERE dbo.SUBSTR(USERNAME,0,10)='" + MZ_ID + "' AND LOGDATE='" + LOGDATE + "' AND (FKEY='NONE' OR FKEY='F3') AND VERIFY='IN') WHERE ROWCOUNT=1");

                if (AddINTIME.Length > 0 && AddINTIME.Substring(0, 3) == "24:")
                {
                    AddINTIME = "00:" + AddINTIME.Substring(3, 7);
                    AddINTIME = AddINTIME.Substring(0, 5);
                }
                AddOUTTIME = o_DBFactory.ABC_toTest.vExecSQL("SELECT LOGTIME FROM (SELECT LOGTIME,ROW_NUMBER() OVER(ORDER BY LOGTIME DESC) AS ROWCOUNT FROM C_CARDHISTORY_NEW WHERE dbo.SUBSTR(USERNAME,0,10)='" + MZ_ID + "' AND LOGDATE='" + LOGDATE + "' AND FKEY='F3' AND VERIFY='IN') WHERE ROWCOUNT=1");
                if (AddOUTTIME.Length > 0 && AddOUTTIME.Substring(0, 3) == "24:")
                {
                    AddOUTTIME = "00:" + AddOUTTIME.Substring(3, 7);
                    AddOUTTIME = AddOUTTIME.Substring(0, 5);
                }
                if (AddINTIME != string.Empty && AddOUTTIME != string.Empty)
                {
                    AddINTIME = AddINTIME.Substring(0, 5);
                    AddOUTTIME = AddOUTTIME.Substring(0, 5);
                    MEMO += " 加班";


                }


                if (string.IsNullOrEmpty(AddOUTTIME))
                {
                    AddINTIME = "";

                }

                #endregion 加班

                #region 超勤
                //超勤

                DateTime nowDate = DateTime.Parse(LOGDATE);
                string fDATE = nowDate.AddDays(-1).ToString("yyyy/MM/dd"); //前一天日期
                string lDATE = nowDate.AddDays(1).ToString("yyyy/MM/dd");  //後一天日期

                //前一天
                string fDate = fDATE.Substring(8, 2);

                //後一天
                string lDate = lDATE.Substring(8, 2);

                string ftime = "";
                string ltime = "";




                List<string> Overdata = new List<string>();

                ftime = o_DBFactory.ABC_toTest.vExecSQL("SELECT LOGTIME FROM (SELECT LOGTIME,ROW_NUMBER() OVER(ORDER BY LOGTIME DESC) AS ROWCOUNT FROM C_CARDHISTORY_NEW WHERE dbo.SUBSTR(USERNAME,0,10)='" + MZ_ID + "' AND LOGDATE='" + fDATE + "' AND FKEY='F4' AND VERIFY='IN') WHERE ROWCOUNT=1"); //計算前天最晚刷卡時間


                OverINTIME = o_DBFactory.ABC_toTest.vExecSQL("SELECT LOGTIME FROM (SELECT LOGTIME,ROW_NUMBER() OVER(ORDER BY LOGTIME) AS ROWCOUNT FROM C_CARDHISTORY_NEW WHERE dbo.SUBSTR(USERNAME,0,10)='" + MZ_ID + "' AND LOGDATE='" + LOGDATE + "' AND FKEY='F4' AND VERIFY='IN') WHERE ROWCOUNT=1"); //計算當天最早刷卡時間

                OverOUTTIME = o_DBFactory.ABC_toTest.vExecSQL("SELECT LOGTIME FROM (SELECT LOGTIME,ROW_NUMBER() OVER(ORDER BY LOGTIME DESC) AS ROWCOUNT FROM C_CARDHISTORY_NEW WHERE dbo.SUBSTR(USERNAME,0,10)='" + MZ_ID + "' AND LOGDATE='" + LOGDATE + "' AND FKEY='F4' AND VERIFY='IN') WHERE ROWCOUNT=1"); //計算當天最晚刷卡時間

                ltime = o_DBFactory.ABC_toTest.vExecSQL("SELECT LOGTIME FROM (SELECT LOGTIME,ROW_NUMBER() OVER(ORDER BY LOGTIME) AS ROWCOUNT FROM C_CARDHISTORY_NEW WHERE dbo.SUBSTR(USERNAME,0,10)='" + MZ_ID + "' AND LOGDATE='" + lDATE + "' AND FKEY='F4' AND VERIFY='IN') WHERE ROWCOUNT=1");//計算隔天最早刷卡時間



                //處理時間顯示

                if (!string.IsNullOrEmpty(OverOUTTIME))
                {
                    OverOUTTIME = OverOUTTIME.Substring(0, 5);

                }

                if (!string.IsNullOrEmpty(OverINTIME))
                {
                    OverINTIME = OverINTIME.Substring(0, 5);

                }

                if (!string.IsNullOrEmpty(ltime))
                {
                    ltime = ltime.Substring(0, 5);

                }

                if (!string.IsNullOrEmpty(ftime))
                {
                    ftime = ftime.Substring(0, 5);
                }


                //判斷是否有異常刷卡
                if (OverOUTTIME != "" && OverINTIME != "")
                {
                    TimeSpan T_allday;
                    DateTime TOverOUTTIME, TOverIntime;

                    TOverOUTTIME = DateTime.Parse(LOGDATE + " " + OverOUTTIME);
                    TOverIntime = DateTime.Parse(LOGDATE + " " + OverINTIME);
                    T_allday = TOverOUTTIME - TOverIntime;
                    if (T_allday.TotalMinutes < 10)
                    {
                        OverOUTTIME = OverINTIME;
                    }
                }




                if (ftime == "" && ltime == "" && OverINTIME != "" && OverOUTTIME != "" && OverINTIME != OverOUTTIME) //只有當天有資料
                {
                    Overdata.Insert(0, OverINTIME);
                    Overdata.Insert(1, OverOUTTIME);
                }

                else if (OverINTIME == OverOUTTIME && OverOUTTIME != "" && ltime != "") //當天只有一筆資料 隔天有資料
                {
                    Overdata.Insert(0, OverOUTTIME);
                    Overdata.Insert(1, lDate + "日" + ltime);

                }
                else if (ftime == "" && ltime != "" && OverINTIME != "" && OverOUTTIME != "" && OverINTIME != OverOUTTIME) //當天有兩筆資料隔天也有
                {
                    Overdata.Insert(0, OverINTIME);
                    Overdata.Insert(1, OverOUTTIME);
                }
                else if (ftime != "" && ltime == "" && OverINTIME != "" && OverOUTTIME != "" && OverINTIME != OverOUTTIME) //當天有兩筆資料前天也有
                {
                    Overdata.Insert(0, OverINTIME);
                    Overdata.Insert(1, OverOUTTIME);
                }
                else if (OverINTIME == OverOUTTIME && OverOUTTIME != "" && ftime != "" && temp_DT.Rows.Count == 0) //當天只有一筆資料 前一天有資料 只在第一天作用
                {
                    Overdata.Insert(0, fDate + "日" + ftime);
                    Overdata.Insert(1, OverINTIME);
                }
                else if (OverOUTTIME != "" && OverINTIME != "" && ftime != "" && ltime != "" && OverOUTTIME != OverINTIME)//四筆都有資料
                {
                    Overdata.Insert(0, OverOUTTIME);
                    Overdata.Insert(1, lDate + "日" + ltime);

                }
                else if (OverOUTTIME == OverINTIME && OverINTIME != "")
                {
                    Overdata.Insert(0, OverINTIME);
                    Overdata.Insert(1, "");
                }

                #endregion 超勤






                #region 值日

                //值日

                string ftime_day = "";
                string ltime_day = "";
                string OverINTIME_day = "";
                string OverOUTTIME_day = "";


                List<string> Overdata_day = new List<string>();

                ftime_day = o_DBFactory.ABC_toTest.vExecSQL("SELECT LOGTIME FROM (SELECT LOGTIME,ROW_NUMBER() OVER(ORDER BY LOGTIME DESC) AS ROWCOUNT FROM C_CARDHISTORY_NEW WHERE dbo.SUBSTR(USERNAME,0,10)='" + MZ_ID + "' AND LOGDATE='" + fDATE + "' AND  FKEY='F2' AND VERIFY='IN') WHERE ROWCOUNT=1"); //計算前天最晚刷卡時間


                OverINTIME_day = o_DBFactory.ABC_toTest.vExecSQL("SELECT LOGTIME FROM (SELECT LOGTIME,ROW_NUMBER() OVER(ORDER BY LOGTIME) AS ROWCOUNT FROM C_CARDHISTORY_NEW WHERE dbo.SUBSTR(USERNAME,0,10)='" + MZ_ID + "' AND LOGDATE='" + LOGDATE + "' AND  FKEY='F2' AND VERIFY='IN') WHERE ROWCOUNT=1"); //計算當天最早刷卡時間


                OverOUTTIME_day = o_DBFactory.ABC_toTest.vExecSQL("SELECT LOGTIME FROM (SELECT LOGTIME,ROW_NUMBER() OVER(ORDER BY LOGTIME DESC) AS ROWCOUNT FROM C_CARDHISTORY_NEW WHERE dbo.SUBSTR(USERNAME,0,10)='" + MZ_ID + "' AND LOGDATE='" + LOGDATE + "' AND FKEY='F2' AND VERIFY='IN') WHERE ROWCOUNT=1"); //計算當天最晚刷卡時間

                ltime_day = o_DBFactory.ABC_toTest.vExecSQL("SELECT LOGTIME FROM (SELECT LOGTIME,ROW_NUMBER() OVER(ORDER BY LOGTIME) AS ROWCOUNT FROM C_CARDHISTORY_NEW WHERE dbo.SUBSTR(USERNAME,0,10)='" + MZ_ID + "' AND LOGDATE='" + lDATE + "' AND  FKEY='F2' AND VERIFY='IN') WHERE ROWCOUNT=1");//計算隔天最早刷卡時間






                //判斷當天是否有異常刷卡
                if (OverOUTTIME_day != "" && OverINTIME_day != "")
                {
                    DateTime TOverOUTTIME_day = DateTime.Parse(LOGDATE + " " + OverOUTTIME_day);
                    DateTime TOverIntime_day = DateTime.Parse(LOGDATE + " " + OverINTIME_day);
                    TimeSpan T_allday_day = TOverOUTTIME_day - TOverIntime_day;
                    if (T_allday_day.TotalMinutes < 10)
                    {
                        OverOUTTIME_day = OverINTIME_day;
                    }
                }






                //if (!string.IsNullOrEmpty(OverOUTTIME_day))
                //{
                //    OverOUTTIME_day = OverOUTTIME_day.Substring(0, 5);
                //}

                //if (!string.IsNullOrEmpty(OverINTIME_day))
                //{
                //    day_alltime = OverINTIME_day;
                //    OverINTIME_day = OverINTIME_day.Substring(0, 5);
                //}

                //if (!string.IsNullOrEmpty(ltime_day))
                //{
                //    ltime_day = ltime_day.Substring(0, 5);

                //}

                //if (!string.IsNullOrEmpty(ftime_day))
                //{
                //    ftime_day = ftime_day.Substring(0, 5);
                //}




                if (ftime_day == "" && ltime_day == "" && OverINTIME_day != "" && OverOUTTIME_day != "" && OverINTIME_day != OverOUTTIME_day) //只有當天有資料
                {
                    Overdata_day.Insert(0, OverINTIME_day);
                    Overdata_day.Insert(1, OverOUTTIME_day);
                }
                else if (OverINTIME_day == OverOUTTIME_day && OverOUTTIME_day != "" && ltime_day != "") //當天只有一筆資料 隔天有資料
                {
                    Overdata_day.Insert(0, OverOUTTIME_day);
                    Overdata_day.Insert(1, lDate + "日" + ltime_day);
                }
                else if (OverINTIME_day == OverOUTTIME_day && OverOUTTIME_day != "" && ftime_day != "" && temp_DT.Rows.Count == 0) //當天只有一筆資料 前一天有資料 只在第一列起作用
                {
                    Overdata_day.Insert(0, fDate + "日" + ftime_day);
                    Overdata_day.Insert(1, OverINTIME_day);
                }
                else if (ftime_day == "" && ltime_day != "" && OverINTIME_day != "" && OverOUTTIME_day != "" && OverINTIME_day != OverOUTTIME_day) //當天有兩筆資料隔天也有資料
                {
                    Overdata_day.Insert(0, OverINTIME_day);
                    Overdata_day.Insert(1, OverOUTTIME_day);
                }
                else if (ftime_day != "" && ltime_day == "" && OverINTIME_day != "" && OverOUTTIME_day != "" && OverINTIME_day != OverOUTTIME_day) //當天有兩筆資料前天也有資料
                {
                    Overdata_day.Insert(0, OverINTIME_day);
                    Overdata_day.Insert(1, OverOUTTIME_day);
                }
                else if (OverOUTTIME_day != "" && OverINTIME_day != "" && ftime_day != "" && ltime_day != "" && OverOUTTIME_day != OverINTIME_day)//四筆都有資料
                {
                    if (temp_DT.Rows.Count > 0)
                    {
                        string Time_compare = startTime(OverOUTTIME_day, temp_DT, "值日"); //先判斷最後一筆資料有沒有重複
                        DateTime Time_compare_t;
                        TimeSpan t;

                        if (Time_compare != "")
                        {
                            Time_compare_t = DateTime.Parse(Time_compare);
                            DateTime OverOUTTIME_day_t = DateTime.Parse(OverOUTTIME_day);
                            t = Time_compare_t - OverOUTTIME_day_t;
                            if (t.TotalHours < 1)
                            {
                                Overdata_day.Insert(0, OverINTIME_day);
                                Overdata_day.Insert(1, OverOUTTIME_day);
                            }
                            else
                            {
                                Overdata_day.Insert(0, OverOUTTIME_day);
                                Overdata_day.Insert(1, lDate + "日" + ltime_day);
                            }
                        }


                    }
                    else
                    {
                        Overdata_day.Insert(0, OverOUTTIME_day);
                        Overdata_day.Insert(1, lDate + "日" + ltime_day);
                    }

                }
                else if (OverOUTTIME_day == OverINTIME_day && OverINTIME_day != "")
                {
                    Overdata_day.Insert(0, OverINTIME_day);
                    Overdata_day.Insert(1, "");
                }

                #endregion 值日



                string sqlTBDV = "select MZ_TBDV from A_DLBASE where MZ_ID='" + MZ_ID + "'";
                string resultTBDV = o_DBFactory.ABC_toTest.vExecSQL(sqlTBDV);


                DataRow dr = temp_DT.NewRow();
                dr["OCCC"] = OCCC(MZ_ID);
                dr["MZ_NAME"] = o_A_DLBASE.CNAME(MZ_ID);

                dr["LOGDATE"] = int.Parse(DateManange.datetostr(LOGDATE).Substring(0, 3)).ToString() + "/" + DateManange.datetostr(LOGDATE).Substring(3, 2) + "/" + DateManange.datetostr(LOGDATE).Substring(5, 2);
                //dr["WORKINTIME"] = selectINTIME;
                //dr["WORKOUTTIME"] = selectOUTTIME;
                dr["HolidayLOGDATE"] = "";
                dr["HolidayINTIME"] = "";
                dr["HolidayOUTTIME"] = "";
                dr["OVEROUTTIME"] = "";
                dr["OVERINTIME"] = "";

                dr["DAYINTIME"] = "";
                dr["DAYOUTTIME"] = "";
                dr["ADDINTIME"] = AddINTIME;
                dr["ADDOUTTIME"] = AddOUTTIME;

                dr["MZ_TBDV"] = resultTBDV;

                //超勤
                string againtime_over = "";
                string ITEMA = Count(MZ_ID, LOGDATE1);
                string[] COUNTA = { "" };
                int ITEMAA = 0;
                //計算超勤時間和系統時間比對
                if (ITEMA != "")
                {
                    COUNTA = ITEMA.Split('.');
                    ITEMAA = COUNTA.Length;
                    string over_in = ""; string over_out = "";
                    if (COUNTA.Length > 1)
                    {
                        int numA = COUNTA.Length - 1;
                        over_in = COUNTA[0].Substring(0, 2) + ":" + COUNTA[0].Substring(2, 2);

                        over_out = COUNTA[numA].Substring(5, 2) + ":" + COUNTA[numA].Substring(7, 2);
                        if (COUNTA.Length >= 19)
                            over_out = lDATE.Substring(5, 2) + "日" + COUNTA[numA].Substring(5, 2) + ":" + COUNTA[numA].Substring(7, 2);


                        if (Overdata.Count == 2)
                        {
                            if (Overdata[0] != "")
                            {
                                TimeSpan TS;
                                DateTime TS1, TS2;
                                if (Overdata[0].Contains("日")) //有包含日的話則為前一天
                                {
                                    TS1 = DateTime.Parse(fDATE + " " + Overdata[0]);//指紋起

                                    TS2 = DateTime.Parse(fDATE + " " + over_in); //   系統起
                                    TS = TS1 - TS2;

                                    if (TS.TotalHours > 1)
                                    {
                                        Overdata[0] = "";
                                    }
                                }
                                else
                                {
                                    TS1 = DateTime.Parse(LOGDATE + " " + Overdata[0]);//起

                                    TS2 = DateTime.Parse(LOGDATE + " " + over_in); //   系統起
                                    TS = TS1 - TS2;

                                    if (TS.TotalHours > 1)
                                    {
                                        Overdata[0] = "";
                                    }

                                }

                            }

                            if (Overdata[1] != "")
                            {
                                TimeSpan TS;
                                DateTime TS1, TS2;
                                if (Overdata[1].Contains("日")) //有包含日的話則為後一天
                                {
                                    TS1 = DateTime.Parse(lDATE + " " + Overdata[1]);//指紋迄

                                    TS2 = DateTime.Parse(lDATE + " " + over_out); //   系統迄
                                    TS = TS1 - TS2;

                                    if (TS.TotalHours > 1)
                                    {
                                        Overdata[1] = "";
                                    }
                                }
                                else
                                {
                                    TS1 = DateTime.Parse(LOGDATE + " " + Overdata[1]);//迄

                                    TS2 = DateTime.Parse(LOGDATE + " " + over_out); //   系統迄
                                    TS = TS1 - TS2;

                                    if (TS.TotalHours > 1)
                                    {
                                        Overdata[1] = "";
                                    }

                                }

                            }

                            if (temp_DT.Rows.Count != 0)
                            {
                                againtime_over = startTime(Overdata[0], temp_DT, "超勤");//判斷前一天的迄和今天的起是否相同
                            }
                            if (againtime_over == Overdata[0])
                            {
                                dr["OVERINTIME"] = "";
                                dr["OVEROUTTIME"] = "";
                            }
                            else
                            {
                                dr["OVERINTIME"] = Overdata[0];
                                dr["OVEROUTTIME"] = Overdata[1];
                                if (Overdata[1].Length > 5 || Overdata[0].Length > 5)
                                {
                                    MEMO += " 超勤跨日";

                                }
                                if (Overdata[0] != "" && Overdata[1] == "")
                                {
                                    MEMO += " 超勤異常";
                                }
                            }

                        }
                        else if (Overdata.Count == 0)
                        {
                            MEMO += " 超勤異常";
                        }


                    }

                }
                else
                {
                    if (Overdata.Count == 2)
                    {
                        if (temp_DT.Rows.Count != 0)
                        {
                            againtime_over = startTime(Overdata[0], temp_DT, "超勤");//判斷前一天的迄和今天的起是否相同
                        }
                        if (againtime_over == Overdata[0])
                        {
                            Overdata[0] = "";
                            Overdata[1] = "";
                        }
                        else
                        {
                            dr["OVERINTIME"] = Overdata[0];
                            dr["OVEROUTTIME"] = Overdata[1];
                            if (Overdata[1].Length > 5 || Overdata[0].Length > 5)
                            {
                                MEMO += " 超勤跨日";

                            }
                            if (Overdata[0] != "" && Overdata[1] == "")
                            {
                                MEMO += " 超勤異常";
                            }
                        }
                    }

                }

                //if (Overdata.Count == 2)
                //{
                //    if (temp_DT.Rows.Count != 0)
                //    {
                //        againtime_over = startTime(Overdata[0], temp_DT, "超勤");//判斷前一天的迄和今天的起是否相同
                //    }
                //    if (againtime_over == Overdata[0])
                //    {
                //        dr["OVERINTIME"] = "";
                //        dr["OVEROUTTIME"] = "";

                //    }
                //    else
                //    {
                //        dr["OVERINTIME"] = Overdata[0];
                //        dr["OVEROUTTIME"] = Overdata[1];
                //        if (Overdata[1].Length > 5 || Overdata[0].Length > 5)
                //        {
                //            MEMO += " 超勤跨日";

                //        }
                //        if (Overdata[0] != "" && Overdata[1] == "")
                //        {
                //            MEMO += " 超勤異常";
                //        }
                //    }

                //}

                //值日
                string againtime = "";

                //判斷當天起迄的值班
                string sql_day = @"select TIME_SATRT , TIME_END from  C_ONDUTY_DAY where  IS_CHK='Y' and
 MZ_ID='" + MZ_ID + "' and TIME_SATRT>=dbo.TO_CHAR(dbo.TO_DATE('" + LOGDATE + "','YYYY/MM/DD'))and TIME_SATRT<=dbo.TO_CHAR(dbo.TO_DATE('" + lDATE + "','YYYY/MM/DD'))";

                string dt_start_OverINTIME_day = "", dt_end_OverINTIME_day = "";//上下班系統統計時間

                DateTime INTIME, OUTTIME;
                DataTable dt_day = new DataTable();
                dt_day = o_DBFactory.ABC_toTest.Create_Table(sql_day, "get");
                if (dt_day.Rows.Count > 0)
                {
                    //dt_start_OverINTIME_day = dt_day.Rows[0]["TIME_SATRT"].ToString();
                    //dt_end_OverINTIME_day = dt_day.Rows[0]["TIME_END"].ToString();
                    INTIME = DateTime.Parse(dt_day.Rows[0]["TIME_SATRT"].ToString());
                    dt_start_OverINTIME_day = INTIME.ToString("HH:mm");
                    OUTTIME = DateTime.Parse(dt_day.Rows[0]["TIME_END"].ToString());

                    if (OUTTIME.ToString("yyyy/MM/dd") == LOGDATE)
                    {
                        dt_end_OverINTIME_day = OUTTIME.ToString("HH:mm");
                    }
                    else if (OUTTIME.ToString("yyyy/MM/dd") == lDATE)
                    {
                        dt_end_OverINTIME_day = lDATE.Substring(8, 2) + "日" + OUTTIME.ToString("HH:mm");
                    }

                    //指紋時間與系統時間比對
                    if (dt_start_OverINTIME_day != "" && dt_end_OverINTIME_day != "")
                    {
                        if (Overdata_day.Count == 2)
                        {
                            if (Overdata_day[0] != "")
                            {
                                TimeSpan TS;
                                DateTime TS1;
                                if (Overdata_day[0].Contains("日")) //有包含日的話則為前一天
                                {
                                    TS1 = DateTime.Parse(fDATE + " " + Overdata_day[0]);//起

                                    TS = TS1 - INTIME;

                                    if (TS.TotalHours > 1)
                                    {
                                        Overdata_day[0] = "";
                                    }
                                }
                                else
                                {
                                    TS1 = DateTime.Parse(LOGDATE + " " + Overdata_day[0]);//起

                                    TS = TS1 - INTIME;

                                    if (TS.TotalHours > 1)
                                    {
                                        Overdata_day[0] = "";
                                    }

                                }

                            }
                            if (Overdata_day[1] != "")
                            {
                                DateTime TS2; TimeSpan TS;
                                if (Overdata_day[1].Contains("日")) //有包含日的話則為後一天
                                {
                                    TS2 = DateTime.Parse(lDATE + " " + Overdata_day[1]);//迄
                                    TS = TS2 - OUTTIME;
                                    if (TS.TotalHours > 1)
                                    {
                                        Overdata_day[1] = "";

                                    }
                                }
                                else
                                {
                                    TS2 = DateTime.Parse(LOGDATE + " " + Overdata_day[1]);//迄
                                    TS = TS2 - OUTTIME;
                                    if (TS.TotalHours > 1)
                                    {
                                        Overdata_day[1] = "";

                                    }

                                }



                            }

                            if (temp_DT.Rows.Count != 0)
                            {
                                againtime = startTime(Overdata_day[0], temp_DT, "值日");//判斷前一天的迄和今天的起是否相同
                            }
                            if (againtime == Overdata_day[0])
                            {
                                Overdata_day[0] = "";
                                Overdata_day[1] = "";

                            }
                            else
                            {
                                //if (Overdata_day[0].Length != 0)
                                //{
                                //    dr["DAYINTIME"] = Overdata_day[0].Substring(0, 5);
                                //}
                                //dr["DAYINTIME"] = Left(Overdata_day[0],5);
                                //dr["DAYINTIME"] = Overdata_day[0];
                                //dr["DAYOUTTIME"] = Overdata_day[1];
                                if (Overdata_day[0].Length == 8)
                                {
                                    dr["DAYINTIME"] = Left(Overdata_day[0], 5);

                                }
                                else if (Overdata_day[0].Length == 11)
                                {
                                    dr["DAYINTIME"] = Left(Overdata_day[0], 8);
                                    KIND += " 值日跨日";

                                }

                                if (Overdata_day[1].Length == 8)
                                {
                                    dr["DAYOUTTIME"] = Left(Overdata_day[1], 5);
                                    dr["DAYSECOND"] = Right(Overdata_day[1], 3);
                                }
                                else if (Overdata_day[1].Length == 11)
                                {
                                    dr["DAYOUTTIME"] = Left(Overdata_day[1], 8);
                                    dr["DAYSECOND"] = Right(Overdata_day[1], 3);
                                    KIND += " 值日跨日";

                                }


                                //if (Overdata_day[1].Length > 5 || Overdata_day[0].Length > 5)
                                //{
                                //    KIND += " 值日跨日";
                                //}
                                if (Overdata_day[0] != "" && Overdata_day[1] == "")
                                {
                                    KIND += " 值日異常";
                                }
                            }

                        }
                        else if (Overdata_day.Count == 0)
                        {
                            KIND += " 值日異常";

                        }

                    }

                }
                else
                {
                    if (Overdata_day.Count == 2)
                    {
                        if (temp_DT.Rows.Count != 0)
                        {
                            againtime = startTime(Overdata_day[0], temp_DT, "值日");//判斷前一天的迄和今天的起是否相同
                        }
                        if (againtime == Overdata_day[0])
                        {
                            Overdata_day[0] = "";
                            Overdata_day[1] = "";

                        }
                        if (Overdata_day[0].Length == 8)
                        {
                            dr["DAYINTIME"] = Left(Overdata_day[0], 5);
                            dr["DAYSECOND"] = Right(Overdata_day[1], 3);

                        }
                        else if (Overdata_day[0].Length == 11)
                        {
                            dr["DAYINTIME"] = Left(Overdata_day[0], 8);
                            dr["DAYSECOND"] = Right(Overdata_day[1], 3);
                            KIND += " 值日跨日";

                        }

                        if (Overdata_day[1].Length == 8)
                        {
                            dr["DAYOUTTIME"] = Left(Overdata_day[1], 5);
                            dr["DAYSECOND"] = Right(Overdata_day[1], 3);
                        }
                        else if (Overdata_day[1].Length == 11)
                        {
                            dr["DAYOUTTIME"] = Left(Overdata_day[1], 8);
                            dr["DAYSECOND"] = Right(Overdata_day[1], 3);
                            KIND += " 值日跨日";

                        }

                        if (Overdata_day[0] != "" && Overdata_day[1] == "")
                        {
                            KIND += " 值日異常";
                        }

                    }
                }



                //if (Overdata_day.Count == 2)
                //{

                //    if (temp_DT.Rows.Count != 0)
                //    {
                //        againtime = startTime(Overdata_day[0], temp_DT, "值日");//判斷前一天的迄和今天的起是否相同
                //    }
                //    if (againtime == Overdata_day[0])
                //    {
                //        dr["DAYINTIME"] = "";
                //        dr["DAYOUTTIME"] = "";

                //    }
                //    else
                //    {
                //        if (Overdata_day[0].Length != 0)
                //        {
                //            dr["DAYINTIME"] = Overdata_day[0].Substring(0, 5);
                //        }
                //        dr["DAYOUTTIME"] = Overdata_day[1];

                //        if (Overdata_day[1].Length > 5 || Overdata_day[0].Length > 5)
                //        {
                //            KIND += " 值日跨日";
                //        }
                //        if (Overdata_day[0] != "" && Overdata_day[1] == "")
                //        {
                //            KIND += " 值日異常";
                //        }
                //    }




                //}


                if (string.IsNullOrEmpty(CODE))
                {
                    dr["HolidayLOGDATE"] = "";
                    dr["HolidayINTIME"] = "";
                    dr["HolidayOUTTIME"] = "";
                }
                else
                {
                    if (temp.Count > 1)
                    {
                        dr["HolidayLOGDATE"] = CODE;
                        //dr["HolidayINTIME"]=temp[1];
                        //dr["HolidayOUTTIME"] = temp[3];

                        dr["HolidayINTIME"] = temp[2].Substring(0, 5);
                        dr["HolidayOUTTIME"] = temp[4].Substring(0, 5);

                        if (temp[6] == "0")
                        {
                            //dr["TDAY"] = "1日0時";
                            dr["HolidayINTIME"] = "08:30";
                            dr["HolidayOUTTIME"] = "17:30";

                        }

                        else if (temp[6] == "8")
                        {
                            //dr["TDAY"] = "1日0時";
                            dr["HolidayINTIME"] = "08:30";
                            dr["HolidayOUTTIME"] = "17:30";
                        }




                        else
                        {
                            //dr["TDAY"] = "0日" + temp[6] + "時";
                            //dr["PDAY"] = "";
                            int hour = (int.Parse(Left(temp[2], 2))) + (int.Parse(temp[6]));
                            string min = Right(temp[4], 2);
                            dr["HolidayINTIME"] = temp[2];
                            dr["HolidayOUTTIME"] = hour.ToString() + ":" + min;
                        }


                    }
                    else
                    {
                        dr["HolidayINTIME"] = "08:30";
                        dr["HolidayOUTTIME"] = "17:30";
                    }
                }

                dr["Memo"] = MEMO;
                dr["KIND"] = KIND;


                return dr;
            }

            /// <summary>
            /// 判斷起訖時間是否有重複
            /// </summary>

            public static string startTime(string outtime, DataTable temp, string type_A)
            {
                string day_second = "";
                if (type_A == "值日")
                {
                    int Num = int.Parse(temp.Rows.Count.ToString());
                    outtime = temp.Rows[Num - 1]["DAYOUTTIME"].ToString();
                    day_second = temp.Rows[Num - 1]["DAYSECOND"].ToString();
                    outtime = Right(outtime, 5) + day_second;
                }
                else if (type_A == "超勤")
                {
                    int Num = int.Parse(temp.Rows.Count.ToString());
                    outtime = temp.Rows[Num - 1]["OVEROUTTIME"].ToString();
                    outtime = Right(outtime, 5);

                }

                return outtime;
            }


            /// <summary>
            /// 身分證字號抓取職稱(回傳中文職稱)
            /// </summary>
            /// <param name="IDNO">身分證字號</param>
            /// <returns></returns>
            public static string OCCC(string IDNO)
            {
                string strSQL;
                DataTable temp = new DataTable();
                strSQL = @"select RTRIM(b.MZ_KCHI) from A_DLBASE A
LEFT JOIN    A_KTYPE b ON  RTRIM(B.MZ_KTYPE) = '26'   AND RTRIM(B.MZ_KCODE) =  RTRIM(A.MZ_OCCC)
WHERE  RTRIM(A.MZ_ID) ='" + IDNO + "'";
                System.Data.DataTable dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");
                string rs = (dt.Rows.Count > 0) ? rs = dt.Rows[0][0].ToString() : "";
                return rs;
            }


            // 取得由左數來第N位字串
            public static string Left(string sSource, int iLength)
            {
                if (sSource.Trim().Length > 0)
                    return sSource.Substring(0, iLength > sSource.Length ? sSource.Length : iLength);
                else
                    return "";

            }

            // 取得由右數來第N位字串

            public static string Right(string sSource, int iLength)
            {

                if (sSource.Trim().Length > 0)

                    return sSource.Substring(iLength > sSource.Length ? 0 : sSource.Length - iLength);

                else

                    return "";

            }

            /// <summary>
            /// 計算超勤系統的時間
            /// </summary>
            /// <param name="ID"></param>
            /// <param name="MODE"></param>
            /// <param name="DATE"></param>
            /// <returns></returns>
            protected static string Count(string ID, string DATE)
            {
                string result = "";

                string strSQL = "SELECT * FROM  C_DUTYTABLE_PERSONAL WHERE MZ_ID='" + ID + "' AND DUTYDATE='" + DATE + "'";

                DataTable temp_DT_OVER = new DataTable();

                temp_DT_OVER = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GETVALUE");

                int j = 0;
                int K = 0;
                string timestring = "";
                if (temp_DT_OVER.Rows.Count > 0)
                {
                    for (int i = 1; i < 27; i++)
                    {
                        j++;


                        if (temp_DT_OVER.Rows[0]["DUTYITEM" + i.ToString()].ToString() != string.Empty)
                        {
                            K++;
                            if (K == 1)
                                timestring += temp_DT_OVER.Rows[0]["TIME" + i.ToString()].ToString();
                            else
                                timestring += "." + temp_DT_OVER.Rows[0]["TIME" + i.ToString()].ToString();

                        }

                        result = timestring;



                    }
                }
                return result;
            }

        }

        /// <summary>
        /// 輪值表各單位明細
        /// </summary>
        public class C_ONTYDUTY_UnitList
        {
            /// <summary> Joy 新增 輪值表 (新版)
            /// 
            /// </summary>
            /// <param name="MZ_EXAD"></param>
            /// <param name="MZ_EXUNIT"></param>
            /// <param name="DATE"></param>
            /// <returns></returns>
            public static DataTable doSearch_new(string MZ_EXAD, string MZ_EXUNIT, string DATE)
            {
                string strSQL = string.Format(@"SELECT MZ_NAME,DATE_TAG,kind
                                                FROM C_ONDUTY_DAY  CC
                                                LEFT JOIN A_DLBASE ON A_DLBASE.MZ_ID= CC.MZ_ID   
                                                LEFT JOIN C_DUTYTABLE_PERSONAL CD ON CD.MZ_ID = CC.MZ_ID AND CD.DUTYDATE = CC.DATE_TAG
                                                LEFT JOIN C_OVERTIME_HOUR_INSIDE  CO  ON CO.MZ_ID = CC.MZ_ID AND CO.MZ_DATE = CC.DATE_TAG 
                                                WHERE  CC.MZ_EXAD='{0}' AND CC.MZ_EXUNIT='{1}' AND   substr(CC.DATE_TAG , 0,5 )='{2}' ORDER BY  DATE_TAG ", MZ_EXAD, MZ_EXUNIT, DATE);


                DataTable Data_dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");

                DateTime date = DateTime.Parse((int.Parse(DATE.Substring(0, 3)) + 1911) + "/" + DATE.Substring(3, 2) + "/01");
                int days_count = DateTime.DaysInMonth(date.Year, date.Month);

                DataTable rpt_dt = new DataTable();

                /// 新增資料欄位
                rpt_dt.Columns.Add("DAY");
                rpt_dt.Columns.Add("WEEK");
                /// Joy 修正 人員分為三種類型(24小時、12小時、8小時)
                rpt_dt.Columns.Add("Group1Name");
                rpt_dt.Columns.Add("Group2Name");
                rpt_dt.Columns.Add("Group3Name");

                for (int i = 1; i <= days_count; i++)
                {
                    /// 新增資料
                    /// 
                    DataRow dr = rpt_dt.NewRow();
                    /// 處理每日資料
                    DataRow[] adddr = Data_dt.Select("DATE_TAG='" + DATE + i.ToString().PadLeft(2, '0') + "'");

                    DateTime WeekName = DateTime.Parse((int.Parse(DATE.Substring(0, 3)) + 1911) + "/" + DATE.Substring(3, 2) + "/" + i.ToString().PadLeft(2, '0'));

                    if (adddr.Length > 0)
                    {
                        /// 人員分為三種類別
                        List<string> NAME_LISE1 = new List<string>();       // 24小時
                        List<string> NAME_LISE2 = new List<string>();       // 12小時
                        List<string> NAME_LISE3 = new List<string>();       // 8小時

                        foreach (DataRow dr_NAME in adddr)
                        {
                            /// 類別
                            string GroupType = dr_NAME["KIND"].ToString().Substring(1, 1);
                            if (GroupType == "1")
                            {
                                NAME_LISE1.Add(dr_NAME["MZ_NAME"].ToString());
                            }
                            else if (GroupType == "2")
                            {
                                NAME_LISE2.Add(dr_NAME["MZ_NAME"].ToString());
                            }
                            else if (GroupType == "3")
                            {
                                NAME_LISE3.Add(dr_NAME["MZ_NAME"].ToString());
                            }
                        }

                        dr["DAY"] = i.ToString().PadLeft(2, '0');
                        dr["WEEK"] = DateManange.WeekToShortName(WeekName);

                        dr["Group1Name"] = string.Join("、", NAME_LISE1.ToArray());
                        dr["Group2Name"] = string.Join("、", NAME_LISE2.ToArray());
                        dr["Group3Name"] = string.Join("、", NAME_LISE3.ToArray());

                        rpt_dt.Rows.Add(dr);
                    }
                    else
                    {
                        dr["DAY"] = i.ToString().PadLeft(2, '0');
                        dr["WEEK"] = DateManange.WeekToShortName(WeekName);
                        dr["NAME"] = "";

                        dr["MEMO"] = "";

                        rpt_dt.Rows.Add(dr);
                    }
                }

                //值勤人員每日上午9時交接，負責工作日誌之總整理及維護本局電腦機房各項系統正常運作，另協助各單位設備故障處理。


                return rpt_dt;
            }

            public static DataTable doSearch(string MZ_EXAD, string MZ_EXUNIT, string DATE)
            {

                string strSQL = string.Format(@"SELECT MZ_NAME,DATE_TAG,
CASE WHEN CD.MZ_ID IS NULL THEN '' ELSE '超勤 ' END +
CASE WHEN CO.MZ_ID IS NULL THEN ''ELSE '加班' END MEMO
FROM C_ONDUTY_DAY  CC
LEFT JOIN A_DLBASE ON A_DLBASE.MZ_ID= CC.MZ_ID   
LEFT JOIN C_DUTYTABLE_PERSONAL CD ON CD.MZ_ID = CC.MZ_ID AND CD.DUTYDATE = CC.DATE_TAG
LEFT JOIN C_OVERTIME_HOUR_INSIDE  CO  ON CO.MZ_ID = CC.MZ_ID AND CO.MZ_DATE = CC.DATE_TAG 
WHERE  CC.MZ_EXAD='{0}' AND CC.MZ_EXUNIT='{1}' AND   substr(CC.DATE_TAG , 0,5 )='{2}' ORDER BY  DATE_TAG ", MZ_EXAD, MZ_EXUNIT, DATE);


                DataTable Data_dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");

                DateTime date = DateTime.Parse((int.Parse(DATE.Substring(0, 3)) + 1911) + "/" + DATE.Substring(3, 2) + "/01");
                int days_count = DateTime.DaysInMonth(date.Year, date.Month);

                DataTable rpt_dt = new DataTable();

                rpt_dt.Columns.Add("DAY");
                rpt_dt.Columns.Add("WEEK");
                rpt_dt.Columns.Add("NAME");

                rpt_dt.Columns.Add("MEMO");

                for (int i = 1; i <= days_count; i++)
                {
                    DataRow dr = rpt_dt.NewRow();

                    DataRow[] adddr = Data_dt.Select("DATE_TAG='" + DATE + i.ToString().PadLeft(2, '0') + "'");

                    DateTime WeekName = DateTime.Parse((int.Parse(DATE.Substring(0, 3)) + 1911) + "/" + DATE.Substring(3, 2) + "/" + i.ToString().PadLeft(2, '0'));

                    if (adddr.Length > 0)
                    {

                        List<string> NAME_LISE = new List<string>();

                        List<string> MEMO_LISE = new List<string>();

                        foreach (DataRow dr_NAME in adddr)
                        {
                            NAME_LISE.Add(dr_NAME["MZ_NAME"].ToString());
                            if (!string.IsNullOrEmpty(dr_NAME["MEMO"].ToString()))
                                MEMO_LISE.Add(dr_NAME["MZ_NAME"].ToString() + "：" + dr_NAME["MEMO"].ToString());
                        }



                        dr["DAY"] = i.ToString().PadLeft(2, '0');
                        dr["WEEK"] = DateManange.WeekToShortName(WeekName);
                        dr["NAME"] = string.Join("、", NAME_LISE.ToArray());

                        if (MEMO_LISE.Count > 0)
                            dr["MEMO"] = string.Join(";", MEMO_LISE.ToArray());

                        rpt_dt.Rows.Add(dr);
                    }
                    else
                    {
                        dr["DAY"] = i.ToString().PadLeft(2, '0');
                        dr["WEEK"] = DateManange.WeekToShortName(WeekName);
                        dr["NAME"] = "";

                        dr["MEMO"] = "";

                        rpt_dt.Rows.Add(dr);
                    }



                }

                //值勤人員每日上午9時交接，負責工作日誌之總整理及維護本局電腦機房各項系統正常運作，另協助各單位設備故障處理。




                return rpt_dt;
            }


        }

        /// <summary>
        /// 輪值費印領清冊
        /// </summary>
        public class C_ONTYDUTY_UnitList_PAY
        {
            public static DataTable doSearch(string MZ_EXAD, string MZ_EXUNIT, string DATE)
            {

                string strSQL = string.Format(@"SELECT MZ_NAME,MZ_OCCC_CH, DATE_TAG  ,Holiday.DUTY_KINDNAME HOLIDAY,KIND.DUTY_KINDNAME KIND,PAY,MEMO FROM C_ONDUTY_DAY  CC
                                  LEFT JOIN VW_A_DLBASE_S1 AA ON AA.MZ_ID= CC.MZ_ID   
                                  LEFT JOIN C_ONDUTY_CODE Holiday ON Holiday.DUTY_KIND=substr(CC.KIND , 0,1 )
                                  LEFT JOIN C_ONDUTY_CODE KIND ON KIND.DUTY_KIND=CC.KIND
                                  WHERE  CC.MZ_EXAD='{0}' AND CC.MZ_EXUNIT='{1}' AND   substr(CC.DATE_TAG , 0,5 )='{2}'  ORDER BY  DATE_TAG ", MZ_EXAD, MZ_EXUNIT, DATE);


                DataTable Data_dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");



                return Data_dt;
            }
            // Joy 值日費印領清冊更新
            public static DataTable doSearch_New(string MZ_EXAD, string MZ_EXUNIT, string DATE)
            {
                /// 抓取當月有紀錄者
                string strSQL = string.Format(@"SELECT CC.MZ_ID,MZ_NAME,MZ_OCCC_CH  FROM C_ONDUTY_DAY  CC
                                  LEFT JOIN VW_A_DLBASE_S1 AA ON AA.MZ_ID= CC.MZ_ID   
                                  LEFT JOIN C_ONDUTY_CODE Holiday ON Holiday.DUTY_KIND=substr(CC.KIND , 0,1 )
                                  LEFT JOIN C_ONDUTY_CODE KIND ON KIND.DUTY_KIND=CC.KIND
                                  WHERE  CC.MZ_EXAD='{0}' AND CC.MZ_EXUNIT='{1}' AND   substr(CC.DATE_TAG , 0,5 )='{2}' group by CC.MZ_ID,MZ_NAME,MZ_OCCC_CH  ", MZ_EXAD, MZ_EXUNIT, DATE);


                DataTable Data_dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");
                // 增加欄位紀錄 平日、假日、三節 資訊
                Data_dt.Columns.Add(new DataColumn("NormalA1Date", typeof(string)));            // 平日滿24小時日期
                Data_dt.Columns.Add(new DataColumn("NormalA1Day", typeof(string)));             // 平日滿24小時天數
                Data_dt.Columns.Add(new DataColumn("NormalA2Date", typeof(string)));            // 平日滿12小時日期
                Data_dt.Columns.Add(new DataColumn("NormalA2Day", typeof(string)));             // 平日滿12小時天數
                Data_dt.Columns.Add(new DataColumn("NormalA3Date", typeof(string)));            // 平日滿8小時日期
                Data_dt.Columns.Add(new DataColumn("NormalA3Day", typeof(string)));             // 平日滿8小時天數
                Data_dt.Columns.Add(new DataColumn("VacationB1Date", typeof(string)));          // 假日滿24小時日期
                Data_dt.Columns.Add(new DataColumn("VacationB1Day", typeof(string)));           // 假日滿24小時天數
                Data_dt.Columns.Add(new DataColumn("VacationB2Date", typeof(string)));          // 假日滿12小時日期
                Data_dt.Columns.Add(new DataColumn("VacationB2Day", typeof(string)));           // 假日滿12小時天數
                Data_dt.Columns.Add(new DataColumn("VacationB3Date", typeof(string)));          // 假日滿8小時日期
                Data_dt.Columns.Add(new DataColumn("VacationB3Day", typeof(string)));           // 假日滿8小時天數
                Data_dt.Columns.Add(new DataColumn("VacationThC1Date", typeof(string)));        // 三節滿24小時日期
                Data_dt.Columns.Add(new DataColumn("VacationThC1Day", typeof(string)));         // 三節滿24小時天數
                Data_dt.Columns.Add(new DataColumn("VacationThC2Date", typeof(string)));        // 三節滿12小時日期
                Data_dt.Columns.Add(new DataColumn("VacationThC2Day", typeof(string)));         // 三節滿12小時天數
                Data_dt.Columns.Add(new DataColumn("VacationThC3Date", typeof(string)));        // 三節滿8小時日期
                Data_dt.Columns.Add(new DataColumn("VacationThC3Day", typeof(string)));        // 三節滿8小時天數
                Data_dt.Columns.Add(new DataColumn("A1sum", typeof(string)));
                Data_dt.Columns.Add(new DataColumn("A2sum", typeof(string)));
                Data_dt.Columns.Add(new DataColumn("A3sum", typeof(string)));
                Data_dt.Columns.Add(new DataColumn("B1sum", typeof(string)));
                Data_dt.Columns.Add(new DataColumn("B2sum", typeof(string)));
                Data_dt.Columns.Add(new DataColumn("B3sum", typeof(string)));
                Data_dt.Columns.Add(new DataColumn("C1sum", typeof(string)));
                Data_dt.Columns.Add(new DataColumn("C2sum", typeof(string)));
                Data_dt.Columns.Add(new DataColumn("C3sum", typeof(string)));
                Data_dt.Columns.Add(new DataColumn("SUM", typeof(string)));
                Data_dt.Columns.Add(new DataColumn("MEMO", typeof(string)));

                for (int i = 0; i < Data_dt.Rows.Count; i++)
                {
                    /// 當月有紀錄者 抓取天數
                    string SQL_New = string.Empty;
                    SQL_New = @"SELECT CC.KIND, substr(DATE_TAG , 6,2 ) Ndate,
                                CASE WHEN CD.MZ_ID IS NULL THEN '' ELSE '超勤 ' END +
                                CASE 
                                WHEN CO.RESTFLAG = 'N' THEN '支領加班費' 
                                WHEN CO.RESTFLAG = 'YO' THEN '加班補休'
                                WHEN CO.RESTFLAG = 'YU' THEN '超勤補休'
                                ELSE '' END MEMO 
                                FROM C_ONDUTY_DAY  CC
                                LEFT JOIN A_DLBASE ON A_DLBASE.MZ_ID= CC.MZ_ID   
                                LEFT JOIN C_DUTYTABLE_PERSONAL CD ON CD.MZ_ID = CC.MZ_ID AND CD.DUTYDATE = CC.DATE_TAG
                                LEFT JOIN C_OVERTIME_HOUR_INSIDE  CO  ON CO.MZ_ID = CC.MZ_ID AND CO.MZ_DATE = CC.DATE_TAG 
                                WHERE CC.MZ_EXAD='" + MZ_EXAD + "' AND CC.MZ_EXUNIT='" + MZ_EXUNIT + "' AND   substr(DATE_TAG , 0,5 )='" + DATE + "'"
                                + "and CC.mz_id = '" + Data_dt.Rows[i]["MZ_ID"] + "'";

                    // SQL_New = @"select KIND, substr(DATE_TAG , 6,2 ) Ndate,MEMO from C_ONDUTY_DAY WHERE MZ_EXAD='" + MZ_EXAD + "' AND MZ_EXUNIT='" + MZ_EXUNIT + "' AND   substr(DATE_TAG , 0,5 )='" + DATE + "'"
                    //             + "and mz_id = '" + Data_dt.Rows[i]["MZ_ID"] + "'";

                    DataTable Data_dt_New = o_DBFactory.ABC_toTest.Create_Table(SQL_New, "get");

                    int A1 = 0;
                    int A2 = 0;
                    int A3 = 0;
                    int B1 = 0;
                    int B2 = 0;
                    int B3 = 0;
                    int C1 = 0;
                    int C2 = 0;
                    int C3 = 0;
                    string MEMO = string.Empty;
                    for (int y = 0; y < Data_dt_New.Rows.Count; y++)
                    {
                        string type = Data_dt_New.Rows[y]["KIND"].ToString();
                        string dateN = Data_dt_New.Rows[y]["Ndate"].ToString();
                        /// Joy 新增 備註欄位
                        /// 
                        if (!string.IsNullOrEmpty(Data_dt_New.Rows[y]["MEMO"].ToString()))
                        {
                            MEMO = MEMO + dateN + Data_dt_New.Rows[y]["MEMO"].ToString() + ".";
                        }
                        switch (type)
                        {
                            // 平日金額資訊
                            case "A1":
                                Data_dt.Rows[i]["NormalA1Date"] = Data_dt.Rows[i]["NormalA1Date"] + dateN + ".";
                                A1 += 1;
                                break;
                            case "A2":
                                Data_dt.Rows[i]["NormalA2Date"] = Data_dt.Rows[i]["NormalA2Date"] + dateN + ".";
                                A2 += 1;
                                break;
                            case "A3":
                                Data_dt.Rows[i]["NormalA3Date"] = Data_dt.Rows[i]["NormalA3Date"] + dateN + ".";
                                A3 += 1;
                                break;
                            // 假日金額資料
                            case "B1":
                                Data_dt.Rows[i]["VacationB1Date"] = Data_dt.Rows[i]["VacationB1Date"] + dateN + ".";
                                B1 += 1;
                                break;
                            case "B2":
                                Data_dt.Rows[i]["VacationB2Date"] = Data_dt.Rows[i]["VacationB2Date"] + dateN + ".";
                                B2 += 1;
                                break;
                            case "B3":
                                Data_dt.Rows[i]["VacationB3Date"] = Data_dt.Rows[i]["VacationB3Date"] + dateN + ".";
                                B3 += 1;
                                break;
                            // 三節金額資訊
                            case "C1":
                                Data_dt.Rows[i]["VacationThC1Date"] = Data_dt.Rows[i]["VacationThC1Date"] + dateN + ".";
                                C1 += 1;
                                break;
                            case "C2":
                                Data_dt.Rows[i]["VacationThC2Date"] = Data_dt.Rows[i]["VacationThC2Date"] + dateN + ".";
                                C2 += 1;
                                break;
                            case "C3":
                                Data_dt.Rows[i]["VacationThC3Date"] = Data_dt.Rows[i]["VacationThC3Date"] + dateN + ".";
                                C3 += 1;
                                break;
                        }
                    }
                    // 值班天數
                    Data_dt.Rows[i]["NormalA1Day"] = A1;
                    Data_dt.Rows[i]["NormalA2Day"] = A2;
                    Data_dt.Rows[i]["NormalA3Day"] = A3;
                    Data_dt.Rows[i]["VacationB1Day"] = B1;
                    Data_dt.Rows[i]["VacationB2Day"] = B2;
                    Data_dt.Rows[i]["VacationB3Day"] = B3;
                    Data_dt.Rows[i]["VacationThC1Day"] = C1;
                    Data_dt.Rows[i]["VacationThC2Day"] = C2;
                    Data_dt.Rows[i]["VacationThC3Day"] = C3;

                    // 值班類別總金額
                    int A1sum = A1 * 300;
                    int A2sum = A2 * 200;
                    int A3sum = A3 * 100;
                    int B1sum = B1 * 500;
                    int B2sum = B2 * 340;
                    int B3sum = B3 * 170;
                    int C1sum = C1 * 1000;
                    int C2sum = C2 * 680;
                    int C3sum = C3 * 340;
                    int SUM = A1sum + A2sum + A3sum + B1sum + B2sum + B3sum + C1sum + C2sum + C3sum;

                    Data_dt.Rows[i]["A1sum"] = A1sum;
                    Data_dt.Rows[i]["A2sum"] = A2sum;
                    Data_dt.Rows[i]["A3sum"] = A3sum;
                    Data_dt.Rows[i]["B1sum"] = B1sum;
                    Data_dt.Rows[i]["B2sum"] = B2sum;
                    Data_dt.Rows[i]["B3sum"] = B3sum;
                    Data_dt.Rows[i]["C1sum"] = C1sum;
                    Data_dt.Rows[i]["C2sum"] = C2sum;
                    Data_dt.Rows[i]["C3sum"] = C3sum;
                    Data_dt.Rows[i]["SUM"] = SUM;
                    Data_dt.Rows[i]["MEMO"] = MEMO;
                }
                return Data_dt;
            }
        }

        /// <summary>
        /// 外勤使用值班報表
        /// </summary>

        public class C_time_out
        {
            public static DataTable doSearch(string DATE, string DATE2, string MZ_EXAD, string MZ_EXUNIT, string MZ_ID)
            {
                DataTable temp_DT = new DataTable();


                temp_DT.Columns.Clear();

                temp_DT.Columns.Add("OCCC", typeof(string));
                temp_DT.Columns.Add("MZ_NAME", typeof(string));
                temp_DT.Columns.Add("LOGDATE", typeof(string));
                //temp_DT.Columns.Add("WORKINTIME", typeof(string));
                //temp_DT.Columns.Add("WORKOUTTIME", typeof(string));

                //請假
                temp_DT.Columns.Add("HolidayLOGDATE", typeof(string));
                temp_DT.Columns.Add("HolidayINTIME", typeof(string));
                temp_DT.Columns.Add("HolidayOUTTIME", typeof(string));

                //超勤
                //temp_DT.Columns.Add("OVERITEM", typeof(string));
                temp_DT.Columns.Add("OVERINTIME", typeof(string));
                temp_DT.Columns.Add("OVEROUTTIME", typeof(string));

                //值日
                //temp_DT.Columns.Add("DAYITEM", typeof(string));
                temp_DT.Columns.Add("DAYINTIME", typeof(string));
                temp_DT.Columns.Add("DAYOUTTIME", typeof(string));

                //加班
                temp_DT.Columns.Add("ADDINTIME", typeof(string));
                temp_DT.Columns.Add("ADDOUTTIME", typeof(string));

                temp_DT.Columns.Add("Memo", typeof(string));
                temp_DT.Columns.Add("KIND", typeof(string));

                temp_DT.Columns.Add("MZ_TBDV", typeof(string));


                string strSQL = "";
                string SQLUNIT = "";
                string SQLDATE = "";

                string date1 = "";
                string date2 = "";

                string begindate = "";
                string enddate = "";

                temp_DT.Rows.Clear();

                if (DATE != string.Empty && DATE2 != string.Empty)
                {
                    date1 = o_str.tosql(DATE.Replace("/", string.Empty).PadLeft(5, '0'));
                    begindate = (int.Parse(date1.Substring(0, 3)) + 1911).ToString() + "/" + date1.Substring(3, 2) + "/" + date1.Substring(5, 2);
                    date2 = o_str.tosql(DATE2.Replace("/", string.Empty).PadLeft(5, '0'));
                    enddate = (int.Parse(date2.Substring(0, 3)) + 1911).ToString() + "/" + date2.Substring(3, 2) + "/" + date2.Substring(5, 2);
                    //begindate = date1 + "01";

                    SQLDATE = " AND LOGDATE>='" + begindate + "' AND LOGDATE<='" + enddate + "'";
                }
                if (MZ_EXUNIT.Trim() != string.Empty)
                {
                    SQLUNIT = " AND MZ_EXUNIT='" + MZ_EXUNIT + "'";
                }

                //strSQL = "SELECT MZ_ID,MZ_OCCC AS OCCC,dbo.to_number(CASE WHEN MZ_TBDV IS NULL THEN '999' WHEN MZ_TBDV='Z99' THEN '999' ELSE MZ_TBDV END) AS TBDV FROM A_DLBASE WHERE MZ_EXAD='" + MZ_EXAD + "'" + SQLUNIT + " AND MZ_ID IN (SELECT MZ_ID FROM C_CARDSET WHERE MZ_CLOCK='Y') ORDER BY TBDV,OCCC";
                if (MZ_ID != "")
                {
                    //strSQL = "SELECT MZ_ID,MZ_OCCC AS OCCC,dbo.to_number(CASE WHEN MZ_TBDV IS NULL THEN '999' WHEN MZ_TBDV='Z99' THEN '999' ELSE MZ_TBDV END) AS TBDV FROM A_DLBASE WHERE MZ_EXAD='" + MZ_EXAD + "'" + SQLUNIT + " AND MZ_ID IN (SELECT MZ_ID FROM C_CARDSET WHERE MZ_CLOCK='N') AND MZ_ID='" + MZ_ID + "'" + " ORDER BY TBDV,OCCC";
                    strSQL = "SELECT MZ_ID,MZ_OCCC AS OCCC,dbo.to_number(CASE WHEN MZ_TBDV IS NULL THEN '999' WHEN MZ_TBDV='Z99' THEN '999' ELSE MZ_TBDV END) AS TBDV FROM A_DLBASE WHERE MZ_EXAD='" + MZ_EXAD + "'" + SQLUNIT + " AND MZ_ID='" + MZ_ID + "'" + " ORDER BY TBDV,OCCC";

                }
                else
                {
                    strSQL = "SELECT MZ_ID,MZ_OCCC AS OCCC,dbo.to_number(CASE WHEN MZ_TBDV IS NULL THEN '999' WHEN MZ_TBDV='Z99' THEN '999' ELSE MZ_TBDV END) AS TBDV FROM A_DLBASE WHERE MZ_EXAD='" + MZ_EXAD + "'" + SQLUNIT + "  ORDER BY TBDV,OCCC";
                }
                DataTable tempDT = new DataTable();

                tempDT = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GETID");

                DateTime TS1 = DateTime.Parse(begindate);
                DateTime TS2 = DateTime.Parse(enddate);

                TimeSpan TS = TS2 - TS1;

                for (int i = 0; i < tempDT.Rows.Count; i++)
                {
                    for (int j = 0; j <= TS.Days; j++)
                    {
                        C_time_out a = new C_time_out();
                        DataRow drrr = a.Count_Card_Record(tempDT.Rows[i]["MZ_ID"].ToString(), TS1.AddDays(j), temp_DT);
                        temp_DT.Rows.Add(drrr);
                    }

                }

                DataTable dtCopy = temp_DT.Copy();

                DataView dv = temp_DT.DefaultView;
                dv.Sort = "LOGDATE,MZ_TBDV";
                dtCopy = dv.ToTable();


                return dtCopy;
            }




            protected DataRow Count_Card_Record(string MZ_ID, DateTime DATE, DataTable temp_DT)
            {

                string LOGDATE = DATE.Year.ToString() + "/" + DATE.Month.ToString().PadLeft(2, '0') + "/" + DATE.Day.ToString().PadLeft(2, '0'); //西元日期
                string LOGDATE1 = (DATE.Year - 1911).ToString().PadLeft(3, '0') + DATE.Month.ToString().PadLeft(2, '0') + DATE.Day.ToString().PadLeft(2, '0'); //民國日期

                List<String> temp = new List<string>();

                temp = C_CountCardRecord.list_Abnormal(MZ_ID, LOGDATE);

                string selectINTIME = o_DBFactory.ABC_toTest.vExecSQL("SELECT LOGTIME FROM (SELECT LOGTIME,ROW_NUMBER() OVER(ORDER BY LOGTIME) AS ROWCOUNT FROM C_CARDHISTORY_NEW WHERE dbo.SUBSTR(USERNAME,0,10)='" + o_str.tosql(MZ_ID.Trim()) + "' AND LOGDATE='" + LOGDATE + "' AND VERIFY='IN') WHERE ROWCOUNT=1"); //紀錄當天刷卡時間
                string selectOUTTIME = o_DBFactory.ABC_toTest.vExecSQL("SELECT LOGTIME FROM (SELECT LOGTIME,ROW_NUMBER() OVER(ORDER BY LOGTIME DESC) AS ROWCOUNT FROM C_CARDHISTORY_NEW WHERE dbo.SUBSTR(USERNAME,0,10)='" + o_str.tosql(MZ_ID.Trim()) + "' AND LOGDATE='" + LOGDATE + "' AND VERIFY='IN') WHERE ROWCOUNT=1");
                //string selectINOVERTIME = o_DBFactory.ABC_toTest.vExecSQL("SELECT LOGTIME FROM (SELECT LOGTIME,ROW_NUMBER() OVER(ORDER BY LOGTIME) AS ROWCOUNT FROM C_CARDHISTORY_NEW WHERE USERID='" + o_str.tosql(MZ_ID.Trim()) + "' AND LOGDATE='" + LOGDATE + "' AND FKEY='F3' ) WHERE ROWCOUNT=1");
                //string selectOUTOVERTIME = o_DBFactory.ABC_toTest.vExecSQL("SELECT LOGTIME FROM (SELECT LOGTIME,ROW_NUMBER() OVER(ORDER BY LOGTIME DESC) AS ROWCOUNT FROM C_CARDHISTORY_NEW WHERE USERID='" + o_str.tosql(MZ_ID.Trim()) + "' AND LOGDATE='" + LOGDATE + "' AND FKEY='F3' ) WHERE ROWCOUNT=1");

                string KIND = "";
                string MEMO = "";
                string CODE = "";
                string AddINTIME = "";
                string AddOUTTIME = "";

                #region 正常上下班和請假

                //請假

                if (temp.Count > 1)//有請假
                {
                    CODE = temp[0];

                    if (int.Parse(LOGDATE1) >= int.Parse(temp[1]) && int.Parse(LOGDATE1) <= int.Parse(temp[3]) && int.Parse(temp[6]) > 0)///正常
                    {
                        KIND = "";
                        MEMO = "請假";
                    }

                }



                #endregion 正常上下班和請假

                #region 加班
                //加班

                //AddINTIME = o_DBFactory.ABC_toTest.vExecSQL("SELECT LOGTIME FROM (SELECT LOGTIME,ROW_NUMBER() OVER(ORDER BY LOGTIME) AS ROWCOUNT FROM C_CARDHISTORY_NEW WHERE dbo.SUBSTR(USERNAME,0,10)='" + MZ_ID + "' AND LOGDATE='" + LOGDATE + "' AND (FKEY='NONE' OR FKEY='F3') AND VERIFY='IN') WHERE ROWCOUNT=1");

                //if (AddINTIME.Length > 0 && AddINTIME.Substring(0, 3) == "24:")
                //{
                //    AddINTIME = "00:" + AddINTIME.Substring(3, 7);
                //    AddINTIME = AddINTIME.Substring(0, 5);
                //}
                //AddOUTTIME = o_DBFactory.ABC_toTest.vExecSQL("SELECT LOGTIME FROM (SELECT LOGTIME,ROW_NUMBER() OVER(ORDER BY LOGTIME DESC) AS ROWCOUNT FROM C_CARDHISTORY_NEW WHERE dbo.SUBSTR(USERNAME,0,10)='" + MZ_ID + "' AND LOGDATE='" + LOGDATE + "' AND FKEY='F3' AND VERIFY='IN') WHERE ROWCOUNT=1");
                //if (AddOUTTIME.Length > 0 && AddOUTTIME.Substring(0, 3) == "24:")
                //{
                //    AddOUTTIME = "00:" + AddOUTTIME.Substring(3, 7);
                //    AddOUTTIME = AddOUTTIME.Substring(0, 5);
                //}
                //if (AddINTIME != string.Empty && AddOUTTIME != string.Empty)
                //{
                //    AddINTIME = AddINTIME.Substring(0, 5);
                //    AddOUTTIME = AddOUTTIME.Substring(0, 5);
                //    MEMO += " 加班";


                //}


                //if (string.IsNullOrEmpty(AddOUTTIME))
                //{
                //    AddINTIME = "";

                //}

                #endregion 加班

                #region 超勤
                DateTime nowDate = DateTime.Parse(LOGDATE);
                string lDATE = nowDate.AddDays(1).ToString("yyyy/MM/dd");  //後一天日期

                //超勤
                string ITEMA = Count(MZ_ID, LOGDATE1);
                string[] COUNTA = { "" };
                int ITEMAA = 0;
                if (ITEMA != "")
                {
                    COUNTA = ITEMA.Split('.');
                    ITEMAA = COUNTA.Length;
                }


                #endregion 超勤






                #region 值日

                //值日
                string sql_day = @"select TIME_SATRT , TIME_END from  C_ONDUTY_DAY where IS_CHK='Y' 
                and MZ_ID='" + MZ_ID + "' and TIME_SATRT>=dbo.TO_CHAR(dbo.TO_DATE('" + LOGDATE + "','YYYY/MM/DD'))and TIME_SATRT<=dbo.TO_CHAR(dbo.TO_DATE('" + lDATE + "','YYYY/MM/DD'))";  //有審核

                //                string sql_day = @"select TIME_SATRT , TIME_END from  C_ONDUTY_DAY where 
                // MZ_ID='" + MZ_ID + "' and TIME_SATRT>=dbo.TO_CHAR(dbo.TO_DATE('" + LOGDATE + "','YYYY/MM/DD'))and TIME_SATRT<=dbo.TO_CHAR(dbo.TO_DATE('" + lDATE + "','YYYY/MM/DD'))";  //沒有審核


                DataTable dt_day = o_DBFactory.ABC_toTest.Create_Table(sql_day, "getDay");


                #endregion 值日



                string sqlTBDV = "select MZ_TBDV from A_DLBASE where MZ_ID='" + MZ_ID + "'";
                string resultTBDV = o_DBFactory.ABC_toTest.vExecSQL(sqlTBDV);


                DataRow dr = temp_DT.NewRow();
                dr["OCCC"] = OCCC(MZ_ID);
                dr["MZ_NAME"] = o_A_DLBASE.CNAME(MZ_ID);

                dr["LOGDATE"] = int.Parse(DateManange.datetostr(LOGDATE).Substring(0, 3)).ToString() + "/" + DateManange.datetostr(LOGDATE).Substring(3, 2) + "/" + DateManange.datetostr(LOGDATE).Substring(5, 2);
                //dr["WORKINTIME"] = selectINTIME;
                //dr["WORKOUTTIME"] = selectOUTTIME;
                dr["HolidayLOGDATE"] = "";
                dr["HolidayINTIME"] = "";
                dr["HolidayOUTTIME"] = "";

                dr["OVEROUTTIME"] = "";
                dr["OVERINTIME"] = "";

                dr["DAYINTIME"] = "";
                dr["DAYOUTTIME"] = "";
                dr["ADDINTIME"] = AddINTIME;
                dr["ADDOUTTIME"] = AddOUTTIME;

                dr["MZ_TBDV"] = resultTBDV;



                //超勤
                if (COUNTA.Length > 1)
                {
                    int numA = COUNTA.Length - 1;
                    dr["OVERINTIME"] = COUNTA[0].Substring(0, 2) + ":" + COUNTA[0].Substring(2, 2);

                    dr["OVEROUTTIME"] = COUNTA[numA].Substring(5, 2) + ":" + COUNTA[numA].Substring(7, 2);
                    if (COUNTA.Length >= 19)
                        dr["OVEROUTTIME"] = lDATE.Substring(5, 2) + "日" + COUNTA[numA].Substring(5, 2) + ":" + COUNTA[numA].Substring(7, 2);

                }


                //值日
                if (dt_day.Rows.Count > 0)
                {
                    DateTime INTIME = DateTime.Parse(dt_day.Rows[0]["TIME_SATRT"].ToString());
                    dr["DAYINTIME"] = INTIME.ToString("HH:mm");
                    DateTime OUTTIME = DateTime.Parse(dt_day.Rows[0]["TIME_END"].ToString());
                    if (OUTTIME.ToString("yyyy/MM/dd") == LOGDATE)
                    {
                        dr["DAYOUTTIME"] = OUTTIME.ToString("HH:mm");
                    }
                    else if (OUTTIME.ToString("yyyy/MM/dd") == lDATE)
                    {
                        dr["DAYOUTTIME"] = lDATE.Substring(8, 2) + "日" + OUTTIME.ToString("HH:mm");
                    }

                }




                if (string.IsNullOrEmpty(CODE))
                {
                    dr["HolidayLOGDATE"] = "";
                    dr["HolidayINTIME"] = "";
                    dr["HolidayOUTTIME"] = "";
                }
                else
                {
                    if (temp.Count > 1)
                    {
                        dr["HolidayLOGDATE"] = CODE;
                        //dr["HolidayINTIME"]=temp[1];
                        //dr["HolidayOUTTIME"] = temp[3];

                        dr["HolidayINTIME"] = temp[2].Substring(0, 5);
                        dr["HolidayOUTTIME"] = temp[4].Substring(0, 5);

                        if (temp[6] == "0")
                        {
                            //dr["TDAY"] = "1日0時";
                            dr["HolidayINTIME"] = "08:30";
                            dr["HolidayOUTTIME"] = "17:30";

                        }

                        else if (temp[6] == "8")
                        {
                            //dr["TDAY"] = "1日0時";
                            dr["HolidayINTIME"] = "08:30";
                            dr["HolidayOUTTIME"] = "17:30";
                        }




                        else
                        {
                            //dr["TDAY"] = "0日" + temp[6] + "時";
                            //dr["PDAY"] = "";
                            int hour = (int.Parse(Left(temp[2], 2))) + (int.Parse(temp[6]));
                            string min = Right(temp[4], 2);
                            dr["HolidayINTIME"] = temp[2];
                            dr["HolidayOUTTIME"] = hour.ToString() + ":" + min;
                        }


                    }
                    else
                    {
                        dr["HolidayINTIME"] = "08:30";
                        dr["HolidayOUTTIME"] = "17:30";
                    }
                }

                dr["Memo"] = MEMO;
                dr["KIND"] = KIND;


                return dr;
            }

            /// <summary>
            /// 判斷起訖時間是否有重複
            /// </summary>

            public static string startTime(string outtime, DataTable temp, string type_A)
            {
                // string day_second = "";
                if (type_A == "值日")
                {
                    int Num = int.Parse(temp.Rows.Count.ToString());
                    outtime = temp.Rows[Num - 1]["DAYOUTTIME"].ToString();
                    outtime = Right(outtime, 5);
                }
                else if (type_A == "超勤")
                {
                    int Num = int.Parse(temp.Rows.Count.ToString());
                    outtime = temp.Rows[Num - 1]["OVEROUTTIME"].ToString();
                    outtime = Right(outtime, 5);

                }

                return outtime;
            }


            /// <summary>
            /// 身分證字號抓取職稱(回傳中文職稱)
            /// </summary>
            /// <param name="IDNO">身分證字號</param>
            /// <returns></returns>
            public static string OCCC(string IDNO)
            {
                string strSQL;
                DataTable temp = new DataTable();
                strSQL = @"select RTRIM(b.MZ_KCHI) from A_DLBASE A
LEFT JOIN    A_KTYPE b ON  RTRIM(B.MZ_KTYPE) = '26'   AND RTRIM(B.MZ_KCODE) =  RTRIM(A.MZ_OCCC)
WHERE  RTRIM(A.MZ_ID) ='" + IDNO + "'";
                System.Data.DataTable dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");
                string rs = (dt.Rows.Count > 0) ? rs = dt.Rows[0][0].ToString() : "";
                return rs;
            }


            // 取得由左數來第N位字串
            public static string Left(string sSource, int iLength)
            {
                if (sSource.Trim().Length > 0)
                    return sSource.Substring(0, iLength > sSource.Length ? sSource.Length : iLength);
                else
                    return "";

            }

            // 取得由右數來第N位字串

            public static string Right(string sSource, int iLength)
            {

                if (sSource.Trim().Length > 0)

                    return sSource.Substring(iLength > sSource.Length ? 0 : sSource.Length - iLength);

                else

                    return "";

            }


            /// <summary>
            /// 計算超勤系統的時間
            /// </summary>
            /// <param name="ID"></param>
            /// <param name="MODE"></param>
            /// <param name="DATE"></param>
            /// <returns></returns>
            protected static string Count(string ID, string DATE)
            {
                string result = "";

                string strSQL = "SELECT * FROM  C_DUTYTABLE_PERSONAL WHERE MZ_ID='" + ID + "' AND DUTYDATE='" + DATE + "'";

                DataTable temp_DT_OVER = new DataTable();

                temp_DT_OVER = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GETVALUE");

                int j = 0;
                int K = 0;
                string timestring = "";
                if (temp_DT_OVER.Rows.Count > 0)
                {
                    for (int i = 1; i < 27; i++)
                    {
                        j++;


                        if (temp_DT_OVER.Rows[0]["DUTYITEM" + i.ToString()].ToString() != string.Empty)
                        {
                            K++;
                            if (K == 1)
                                timestring += temp_DT_OVER.Rows[0]["TIME" + i.ToString()].ToString();
                            else
                                timestring += "." + temp_DT_OVER.Rows[0]["TIME" + i.ToString()].ToString();

                        }

                        result = timestring;



                    }
                }
                return result;
            }
        }

    }
}

