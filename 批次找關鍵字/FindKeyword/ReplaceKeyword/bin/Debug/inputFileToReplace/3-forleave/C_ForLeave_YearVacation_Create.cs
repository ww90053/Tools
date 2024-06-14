using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TPPDDB.Helpers;

namespace TPPDDB._3_forleave
{
    /// <summary>
    /// 計算年度應休天數
    /// </summary>
    public static class C_ForLeave_YearVacation_Create
    {
        //20140108
        //第1年                          0天
        // <3年                          7天
        //  3<=?< 6  (就是小於第7年)    14天
        //  7<=?< 9  (就是小於第10年)   21天
        // 10<=?<13  (就是小於第14年)   28天
        // 14<=?<15  (14年以上)         30天
        //可是有人反應15年才有30天 待確認


        /// <summary>
        /// 計算用Model
        /// </summary>
        public class ComputeModel
        {
            /// <summary>
            /// 單人計算的人員ID
            /// </summary>
            public string MZ_ID { get; set; }
            /// <summary>
            /// 是否為 新增全部人員?
            /// </summary>
            public bool InsertData { get; set; }
            /// <summary>
            /// 計算年度
            /// </summary>
            public string StatisticsYear { get; set; }
            /// <summary>
            /// 功能代碼(Log使用)
            /// </summary>
            public string TPM_FION { get; set; }
        }

        /// <summary>
        /// 計算年度休假日數
        /// 原初任職日改為初任公職日 20181120 by sky
        /// 更新執行流程，不再進行刪除重建，增加保留假計算 20200304 by sky
        /// </summary>
        public static void YearVacation(ComputeModel model, ref string ErrorMsg)
        {
            List<SqlParameter> parameterList;
            //取得全部人員資料
            string strSQL = "SELECT MZ_QUA_DATE, MZ_TYEAR, MZ_TMONTH, MZ_RYEAR, MZ_RMONTH, MZ_ID, MZ_FDATE FROM A_DLBASE WHERE 1=1 ";
            //針對全部人員計算
            if (string.IsNullOrEmpty(model.MZ_ID))
            {
                //表示計算全部人員
                //if (model.NewPersonnel)
                //{
                //    //針對新進人員
                //    strSQL += " AND MZ_ID NOT IN (SELECT MZ_ID FROM C_DLTBB1) ";
                //}
                //else
                //{
                //    strSQL += " AND MZ_STATUS2='Y' order by MZ_ID";
                //}
                //20211228暫時將新進與全部人員組一樣SQL
                strSQL += " AND MZ_STATUS2='Y' order by MZ_ID";
            }
            else
            {
                //針對單人計算
                strSQL += string.Format(" AND MZ_ID='{0}' ", model.MZ_ID);
            }
            DataTable dt = new DataTable();
            dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");

            //取得各假別基本天數
            string pday = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_DAY FROM C_DLCODE_SETDAY WHERE MZ_CODE = (SELECT MZ_CODE FROM C_DLCODE WHERE MZ_CNAME = '事假')");
            pday = string.IsNullOrEmpty(pday) ? "0" : pday;

            string sickday = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_DAY FROM C_DLCODE_SETDAY WHERE MZ_CODE = (SELECT MZ_CODE FROM C_DLCODE WHERE MZ_CNAME = '病假')");
            sickday = string.IsNullOrEmpty(sickday) ? "0" : sickday;

            string hcareday = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_DAY FROM C_DLCODE_SETDAY WHERE MZ_CODE = (SELECT MZ_CODE FROM C_DLCODE WHERE MZ_CNAME = '家庭照顧')");
            hcareday = string.IsNullOrEmpty(hcareday) ? "0" : hcareday;

            int sday3;
            string MZ_YEAR = model.StatisticsYear.PadLeft(3, '0');

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //取得併計年資
                int tyear = int.Parse(string.IsNullOrEmpty(dt.Rows[i]["MZ_TYEAR"].ToString()) ? "0" : dt.Rows[i]["MZ_TYEAR"].ToString());
                int tmonth = int.Parse(string.IsNullOrEmpty(dt.Rows[i]["MZ_TMONTH"].ToString()) ? "0" : dt.Rows[i]["MZ_TMONTH"].ToString());
                //取得減併計年資
                int ryear = int.Parse(string.IsNullOrEmpty(dt.Rows[i]["MZ_RYEAR"].ToString()) ? "0" : dt.Rows[i]["MZ_RYEAR"].ToString());
                int rmonth = int.Parse(string.IsNullOrEmpty(dt.Rows[i]["MZ_RMONTH"].ToString()) ? "0" : dt.Rows[i]["MZ_RMONTH"].ToString());

                ////非新進人員進行刪除舊資料
                //if (!model.NewPersonnel)
                //{
                //    //刪除已建立資料
                //    string deleteStrSQL = string.Format("DELETE FROM C_DLTBB WHERE MZ_YEAR='{0}' AND MZ_ID='{1}' ", MZ_YEAR, dt.Rows[i]["MZ_ID"].ToString());
                //    o_DBFactory.ABC_toTest.Edit_Data(deleteStrSQL);
                //}

                //判斷初任公職日是否正常
                if (string.IsNullOrEmpty(dt.Rows[i]["MZ_QUA_DATE"].ToString()))
                {
                    if (!string.IsNullOrEmpty(model.MZ_ID))
                    {
                        ErrorMsg += "此人初任公職日有誤，請查明後再算！\n";
                        return;
                    }
                    else
                    {
                        ErrorMsg += string.Format("{0}初任公職日異常。\n", dt.Rows[i]["MZ_ID"].ToString());
                        continue;
                    }
                }

                DateTime FDATE = DateTime.Now;

                try
                {
                    FDATE = DateTime.Parse((int.Parse(dt.Rows[i]["MZ_FDATE"].ToString().Substring(0, 3)) + 1911).ToString() + "-" + dt.Rows[i]["MZ_FDATE"].ToString().Substring(3, 2) + "-01");
                }
                catch (Exception)
                {
                    FDATE = DateTime.Now;
                }


                int monthDiff = 0;
                //計算從到職日到指定年度的元旦,經過了幾個月?
                monthDiff = Convert.ToInt32(Microsoft.VisualBasic.DateAndTime.DateDiff("M", FDATE, DateTime.Parse((int.Parse(MZ_YEAR) + 1911).ToString() + "-01-01"), Microsoft.VisualBasic.FirstDayOfWeek.Sunday, Microsoft.VisualBasic.FirstWeekOfYear.FirstFullWeek));

                int MONTH = monthDiff;

                int sday3_hour = 0;
                //計算 核定年資加加減減後,折算幾個月?
                int addMONTH = tyear * 12 + tmonth - ryear * 12 - rmonth;
                //更新基本資料的任職日期
                updateBasicOff(FDATE, dt.Rows[i]["MZ_ID"].ToString(), addMONTH);

                #region 扣除中間留職停薪的月份 20140114 by 立廷
                strSQL = string.Format(@"SELECT EDATE,SDATE FROM C_LVHISTORY WHERE MZ_ID='{0}' AND BACK=1 ORDER BY EDATE", dt.Rows[i]["MZ_ID"].ToString());
                DataTable stay_job = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
                if (stay_job.Rows.Count > 0)
                {
                    for (int k = 0; k < stay_job.Rows.Count; k++)
                    {
                        //時間差
                        DateTime EDATE = DateTime.Parse(stay_job.Rows[k][0].ToString());
                        DateTime SDATE = DateTime.Parse(stay_job.Rows[k][1].ToString());
                        //特殊:如果已經超過產生年度的元旦,則應該以當年元旦為截止,這樣年資扣除才不會超過
                        DateTime Limit = DateTime.Parse((int.Parse(MZ_YEAR) + 1911).ToString() + "-01-01");
                        if (EDATE> Limit)
                        {
                            EDATE = Limit;
                        }
                        //應該避免開始大於結束.否則年資會增加,正常不該發生
                        if (SDATE > EDATE)
                        {   //這回合不處理了,這筆資料有異常
                            continue;
                        }
                        TimeSpan month = EDATE - SDATE;
                        MONTH = MONTH - (month.Days / 30);
                    }
                }
                #endregion

                #region 計算年度休假日數

                sday3 = 計算特休(model, MONTH, ref sday3_hour, addMONTH);
                #endregion

                #region 留職停薪處理 20121129 by irk

                /*
                 如果有留職停薪的資料,這邊僅取最後一筆資料
                 case 不是當年副值

                 */

                //如果有留職停薪的資料
                if (stay_job.Rows.Count > 0)
                {
                    double rday = 0;

                    //這邊僅取最後一筆資料
                    DataRow rowLastVaction = stay_job.Rows[stay_job.Rows.Count - 1];
                    DateTime sdate = DateTime.Parse(rowLastVaction[1].ToString());
                    DateTime edate = DateTime.Parse(rowLastVaction[0].ToString());

                    //換算目標年度, EX:112年的特休,以111年的任職日期作為計算基準,也就是 112-1+1911= 2022年
                    int TargetYear = (int.Parse(MZ_YEAR) - 1) + 1911;

                    //用修正後的計算模組,計算有效年月
                    int iEffectMonthCount = subGetEffectMonthAmount_V2(TargetYear, sdate, edate);
                    //依照有效月份數，折算本年度特休
                    if (iEffectMonthCount >= 0 && iEffectMonthCount<=12)
                    {
                        //計算特休天數
                        subParserResult(iEffectMonthCount, ref sday3, ref sday3_hour);
                    }
                    #region DEL CODE
                    ////修改 20150327 by ???
                    ////如果 要產生的年度+1 等於 留職停薪結束 的年度 ,EX: 112年+1 ,留職停薪 113年結束
                    ////這段邏輯似乎沒有任何意義
                    //if (int.Parse(MZ_YEAR) + 1 == edate.Year - 1911)
                    //{
                    //    sdate = sdate.AddYears(-1);
                    //    edate = edate.AddYears(-1);
                    //}

                    ////若輸入年度不等於結束日期之年度別
                    ////留職停薪的復職年度不在今年
                    ///*
                    // |    |
                    // EX:今年111,要產生112年的特休
                    // 2022年以前復職,不影響計算
                    // 2022當年復職,
                    // 0: 111/1/1,以前復職,單純計算停職多久即可
                    // A: 112年1/1,以前結束留職停薪,代表有剩餘的月份要計算上班
                    // B: 111/1/1,以前開始停職,112/1/1以後才復職,因為沒有上班,不給特休
                    // */
                    ////EX:要產生112年的特休,留職停薪結束不在112年
                    //if (int.Parse(MZ_YEAR) != edate.Year - 1911)
                    //{
                    //    /*
                    //     * A:去年以前復職
                    //     * B:明年以後復職
                    //     */
                    //    //如果 輸入年度 >= 留職停薪開始 且 輸入年度 <= 留職停薪結束年度 (加上上面那個條件, 其實相當於 "<" )
                    //    //相當於:留職停薪中,明年以後才復職
                    //    //則不給特休
                    //    if ((int.Parse(MZ_YEAR) >= (sdate.Year - 1911)) && (int.Parse(MZ_YEAR) <= (edate.Year - 1911)))
                    //    {
                    //        sday3 = 0;
                    //        sday3_hour = 0;
                    //    }
                    //}
                    ////當年度留職停薪復職,以年底減復職日計算
                    //else if (int.Parse(MZ_YEAR) == edate.Year - 1911)
                    //{
                    //    //新算法，這邊的計算主要是進行月份的相減
                    //    int iEffectMonthCount = subGetEffectMonthAmount(sdate, edate);

                    //    //依照有效月份數，折算本年度特休
                    //    if (iEffectMonthCount != 0)
                    //    {
                    //        subParserResult(iEffectMonthCount, ref sday3, ref sday3_hour);
                    //    }
                    //} 
                    #endregion
                }
                #endregion

                //如果是 "新增全部人員"
                if (model.InsertData)
                {
                    strSQL = @"INSERT INTO C_DLTBB(MZ_YEAR,MZ_ID,MZ_HDAY,MZ_HTIME,MZ_SDAY,MZ_SDAY_HOUR,MZ_SDAY2,MZ_SDAY2_HOUR,MZ_PDAY,MZ_SICKDAY,MZ_HCAREDAY) 
                                VALUES(@MZ_YEAR,@MZ_ID,@MZ_HDAY,@MZ_HTIME,@MZ_SDAY,@MZ_SDAY_HOUR,@MZ_SDAY2,@MZ_SDAY2_HOUR,@MZ_PDAY,@MZ_SICKDAY,@MZ_HCAREDAY) ";
                }
                else
                {
                    //如果有指定人員ID,代表是針對單人的
                    if (!string.IsNullOrEmpty(model.MZ_ID))
                    {
                        //先檢查有沒有重複
                        bool isExist = checkExist_C_DLTBB(MZ_YEAR, model.MZ_ID);
                        if (isExist)
                        {
                            ErrorMsg += "本年度應休假天數已計算完成，請至10.20功能自行修改！\n";
                            return;
                        }

                        strSQL = @"INSERT INTO C_DLTBB(MZ_YEAR,MZ_ID,MZ_HDAY,MZ_HTIME,MZ_SDAY,MZ_SDAY_HOUR,MZ_SDAY2,MZ_SDAY2_HOUR,MZ_PDAY,MZ_SICKDAY,MZ_HCAREDAY) 
                                VALUES(@MZ_YEAR,@MZ_ID,@MZ_HDAY,@MZ_HTIME,@MZ_SDAY,@MZ_SDAY_HOUR,@MZ_SDAY2,@MZ_SDAY2_HOUR,@MZ_PDAY,@MZ_SICKDAY,@MZ_HCAREDAY) ";
                    }
                    //"更新全部人員"的版本
                    else
                    {
                        strSQL = @"UPDATE C_DLTBB SET MZ_HDAY=@MZ_HDAY, MZ_HTIME=@MZ_HTIME, MZ_SDAY=@MZ_SDAY, MZ_SDAY_HOUR=@MZ_SDAY_HOUR, MZ_SDAY2=@MZ_SDAY2, MZ_SDAY2_HOUR=@MZ_SDAY2_HOUR,
                                                  MZ_PDAY=@MZ_PDAY, MZ_SICKDAY=@MZ_SICKDAY, MZ_HCAREDAY=@MZ_HCAREDAY
                                Where MZ_YEAR=@MZ_YEAR And MZ_ID=@MZ_ID ";
                    }
                }

                parameterList = new List<SqlParameter>()
                {
                    new SqlParameter("MZ_YEAR",SqlDbType.VarChar) { Value = MZ_YEAR },
                    new SqlParameter("MZ_ID",SqlDbType.VarChar) { Value = dt.Rows[i]["MZ_ID"].ToString() },
                    new SqlParameter("MZ_HDAY",SqlDbType.Float) { Value = sday3 >= 0 ? sday3 : 0 },
                    new SqlParameter("MZ_HTIME",SqlDbType.Float) { Value = sday3_hour >= 0 ? sday3_hour : 0 },
                    //new SqlParameter("MZ_SDAY",SqlDbType.Float) { Value = SDAY },
                    //new SqlParameter("MZ_SDAY_HOUR", SqlDbType.Float) { Value = SDAY_HOUR },
                    //new SqlParameter("MZ_SDAY2",SqlDbType.Float) { Value = SDAY2 },
                    //new SqlParameter("MZ_SDAY2_HOUR", SqlDbType.Float) { Value = SDAY2_HOUR },
                    new SqlParameter("MZ_SDAY",SqlDbType.Float) { Value = 0 },
                    new SqlParameter("MZ_SDAY_HOUR", SqlDbType.Float) { Value = 0 },
                    new SqlParameter("MZ_SDAY2",SqlDbType.Float) { Value = 0 },
                    new SqlParameter("MZ_SDAY2_HOUR", SqlDbType.Float) { Value = 0 },
                    new SqlParameter("MZ_PDAY",SqlDbType.Float) { Value = pday },
                    new SqlParameter("MZ_SICKDAY",SqlDbType.Float) { Value = sickday },
                    new SqlParameter("MZ_HCAREDAY",SqlDbType.Float) { Value = hcareday }
                };
                try
                {
                    //Log
                    o_DBFactory.ABC_toTest.DealCommandLog(strSQL, parameterList);

                    LogModel.saveLog("C", "A", strSQL, parameterList, model.TPM_FION, "新增可休假日");
                }
                catch (Exception ex)
                {
                    ErrorMsg += string.Format("{0} 資料新增異常！\n", dt.Rows[i]["MZ_ID"].ToString());
                }
            }//for (int i = 0; i < dt.Rows.Count; i++)
        }

        /// <summary>
        /// 計算特休
        /// </summary>
        /// <param name="model">計算參數</param>
        /// <param name="MONTH">在這邊任職幾個月</param>
        /// <param name="sday3_hour">輸出 特休小時 扣掉特休日後</param>
        /// <param name="addMONTH">在以前的單位任職幾個月? 併計年資扣掉減併計年資後 </param>
        /// <returns>輸出 特休日</returns>
        public static int 計算特休(ComputeModel model, int MONTH, ref int sday3_hour, int addMONTH)
        {
            int sday3;
            if (MONTH < 0)
            {
                sday3 = 0;
            }
            else
            {
                if (MONTH < 12)
                {
                    if (addMONTH == 0)
                    {
                        double countsay = MathHelper.Round(7 * double.Parse(MONTH.ToString()) / 12, 1);

                        string[] s = countsay.ToString().Split('.');

                        sday3 = int.Parse(s[0]);

                        if (s.Length == 2)
                        {
                            if (int.Parse(s[1]) > 5)
                            {
                                sday3 = sday3 + 1;
                            }
                            else if (int.Parse(s[1]) > 0)
                            {
                                sday3_hour = 4;
                            }
                        }
                    }
                    else
                    {
                        //任職的月份數,先以 併計年資算出的月份數 為基準
                        int aMTotle = addMONTH;
                        //將目前在警局任職的月份數也納入計算
                        aMTotle += MONTH;

                        if (aMTotle < 36)
                        {
                            double countsay = MathHelper.Round(7 * double.Parse(MONTH.ToString()) / 12, 1);

                            string[] s = countsay.ToString().Split('.');

                            sday3 = int.Parse(s[0]);
                            if (s.Length == 2)
                            {
                                if (int.Parse(s[1]) > 5)
                                {
                                    sday3 = sday3 + 1;
                                }
                                else if (int.Parse(s[1]) > 0)
                                {
                                    sday3_hour = 4;
                                }
                            }
                        }
                        else if (36 <= aMTotle && aMTotle < 72)
                        {
                            double countsay = MathHelper.Round(14 * double.Parse(MONTH.ToString()) / 12, 1);

                            string[] s = countsay.ToString().Split('.');

                            sday3 = int.Parse(s[0]);
                            if (s.Length == 2)
                            {
                                if (int.Parse(s[1]) > 5)
                                {
                                    sday3 = sday3 + 1;
                                }
                                else if (int.Parse(s[1]) > 0)
                                {
                                    sday3_hour = 4;
                                }
                            }
                        }
                        else if (72 <= aMTotle && aMTotle < 108)
                        {
                            double countsay = MathHelper.Round(21 * double.Parse(MONTH.ToString()) / 12, 1);

                            string[] s = countsay.ToString().Split('.');

                            sday3 = int.Parse(s[0]);

                            if (s.Length == 2)
                            {
                                if (int.Parse(s[1]) > 5)
                                {
                                    sday3 = sday3 + 1;
                                }
                                else if (int.Parse(s[1]) > 0)
                                {
                                    sday3_hour = 4;
                                }
                            }
                        }
                        else if (108 <= aMTotle && aMTotle < 168)
                        {
                            double countsay = MathHelper.Round(28 * double.Parse(MONTH.ToString()) / 12, 1);

                            string[] s = countsay.ToString().Split('.');

                            sday3 = int.Parse(s[0]);

                            if (s.Length == 2)
                            {
                                if (int.Parse(s[1]) > 5)
                                {
                                    sday3 = sday3 + 1;
                                }
                                else if (int.Parse(s[1]) > 0)
                                {
                                    sday3_hour = 4;
                                }
                            }
                        }
                        else
                        {
                            double countsay = MathHelper.Round(30 * double.Parse(MONTH.ToString()) / 12, 1);

                            string[] s = countsay.ToString().Split('.');

                            sday3 = int.Parse(s[0]);

                            if (s.Length == 2)
                            {
                                if (int.Parse(s[1]) > 5)
                                {
                                    sday3 = sday3 + 1;
                                }
                                else if (int.Parse(s[1]) > 0)
                                {
                                    sday3_hour = 4;
                                }
                            }
                        }
                    }
                }
                else if (12 <= MONTH + addMONTH && MONTH + addMONTH < 36)
                {
                    sday3 = 7;
                }
                else if (36 <= MONTH + addMONTH && MONTH + addMONTH < 72)
                {
                    sday3 = 14;
                }
                else if (72 <= MONTH + addMONTH && MONTH + addMONTH < 108)
                {
                    sday3 = 21;
                }
                else if (108 <= MONTH + addMONTH && MONTH + addMONTH < 168)
                {
                    sday3 = 28;
                }
                else
                {
                    sday3 = 30;
                }
            }

            return sday3;
        }

        /// <summary>
        /// 檢查C_DLTBB資料是否重複
        /// </summary>
        /// <param name="Year">年度</param>
        /// <param name="MZ_ID">員警ID</param>
        /// <returns></returns>
        public static bool checkExist_C_DLTBB(string Year, string MZ_ID)
        {
            //下面這段程式不知為何會噴錯
            try
            {
                string strSQL = string.Format(@"SELECT count(*) as cnt FROM C_DLTBB where MZ_ID='{0}' AND MZ_YEAR='{1}' ", MZ_ID, Year);
                string count = o_DBFactory.ABC_toTest.vExecSQL(strSQL);
                if (count == "0")
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 更新基本資料的任職日期
        /// </summary>
        /// <param name="FDATE"></param>
        /// <param name="ID"></param>
        /// <param name="addMonth"></param>
        private static void updateBasicOff(DateTime FDATE, string ID, int addMonth)
        {
            int monthDiff = Convert.ToInt32(Microsoft.VisualBasic.DateAndTime.DateDiff("M", FDATE, DateTime.Now, Microsoft.VisualBasic.FirstDayOfWeek.Sunday, Microsoft.VisualBasic.FirstWeekOfYear.FirstFullWeek));
            int totalMonth = monthDiff + addMonth;

            string strSQL = string.Format(@"UPDATE A_DLBASE SET MZ_OFFYY={0}, MZ_OFFMM={1} WHERE MZ_ID='{2}' "
                                            , totalMonth / 12
                                            , totalMonth % 12
                                            , ID);

            o_DBFactory.ABC_toTest.Edit_Data(strSQL);
        }

        /// <summary>
        /// 有效月份數計算 V2版本
        /// 以前的計算方法沒有比較基準的目標年度,會導致計算錯誤
        /// 這邊先不考慮一年留停兩次的極端情境,遇到再說
        /// </summary>
        /// <param name="Year_OnJob">
        /// 任職年度西元年,為產生特休年度的前一年(MZ_YEAR - 1 + 1911)
        /// EX: 計算2023年要核發的的特休,應以2022年作為任職年度
        /// </param>
        /// <param name="dtStartDate">留職停薪起始日</param>
        /// <param name="dtEndDate">留職停薪結束日</param>
        /// <returns>有效任職月份數</returns>
        public static int subGetEffectMonthAmount_V2(int Year_OnJob, DateTime input_dtStartDate, DateTime input_dtEndDate)
        {
            //先做換算,得到上班換算基準的日期
            //留停開始前一天上班
            DateTime beforeStopJobDate = input_dtStartDate.AddDays(-1);
            //留停結束後一天上班
            DateTime ReJobDate = input_dtEndDate.AddDays(1);

            //以下幾種情況,直接回傳12,代表今年12個月都有上班,後面計算上相當於不會減免
            //根本就不在當年度留職停薪
            //代表整年都有任職到
            //以後才留停
            if (beforeStopJobDate.Year > Year_OnJob && ReJobDate.Year > Year_OnJob)
                return 12;
            if (beforeStopJobDate.Year < Year_OnJob && ReJobDate.Year < Year_OnJob)
                return 12;
            //排除掉起訖錯誤的狀況,正常不該發生,留停之前的最後上班日,晚於留停復職日
            if (beforeStopJobDate > ReJobDate)
                return 12;
            //排除掉起訖都在同一個月的情況,代表留停不到一個月
            if (beforeStopJobDate.ToString("yyyy-MM") == ReJobDate.ToString("yyyy-MM"))
                return 12;
            //計算留停的有效月份,原則上,只要當月有一天是上班日,就應該計算為有效月份
            int monthCount_A = 0;
            int monthCount_B = 0;
            //如果是當年開始留停
            if (beforeStopJobDate.Year == Year_OnJob)
            {
                //判斷 留停開始前一天上班在哪個月份? 以那個月份作開始
                //EX: 2022.03.02留停 2022.03.01最後上班日 則視同上班到三月,所以將1,2,3都納入有效月份
                //EX: 2022.05.01留停 2022.04.30最後上班日 則視同上班到四月,所以將1,2,3,4都納入有效月份
                monthCount_A = beforeStopJobDate.Month;
            }

            if (ReJobDate.Year == Year_OnJob)
            {
                //判斷 留停開始前一天上班在哪個月份? 以那個月份作開始上班日
                //EX: 2022.10.01結束留停 2022.10.02上班日 所以將10,11,12都納入有效月份
                //EX: 2022.09.30結束留停 2022.10.01上班日 所以將10,11,12都納入有效月份
                //EX: 2022.09.29結束留停 2022.09.30上班日 所以將9,10,11,12都納入有效月份
                monthCount_B = 12 - ReJobDate.Month + 1;
            }
            return monthCount_A + monthCount_B;
        }

        /// <summary>
        /// 有效月份數計算
        /// 20150408 Neil 建立
        /// </summary>
        /// <param name="dtStartDate">留職停薪起始日</param>
        /// <param name="dtEndDate">留職停薪結束日</param>
        /// <returns>有效月份數</returns>
        public static int subGetEffectMonthAmount(DateTime dtStartDate, DateTime dtEndDate)
        {
            DateTime dtClearStartDate = dtStartDate.AddMonths(1); //此月天數均略過，故+1
            DateTime dtClearEndDate = dtEndDate.AddMonths(-1); //此月天數均略過，故-1
            int iDiffDays = new TimeSpan(dtEndDate.Ticks - dtStartDate.Ticks).Days + 1; //兩個日期的差異天數

            //判斷年份是否跨年，若為跨年度復職則該年度計算休假時，扣掉包含去年度的部分
            //例如 102.08.05 ~ 103.04.05，無效月份數為 9, 10, 11 ,12, 1, 2, 3，故104年特休公式為 : 7 / 12
            //例外 : 當結束日期為該月最後一天時，該月份同樣也要算入無效月份數中
            Boolean isCrossYear = (dtEndDate.Year - dtStartDate.Year) == 0 ? false : true;
            Boolean isCrossTooMuchYear = (dtEndDate.Year - dtStartDate.Year >= 2) ? true : false;

            int iNoEffectMonthCount = 0;

            //無效月份數
            if (!isCrossYear)
                iNoEffectMonthCount = (dtClearEndDate.Month - dtClearStartDate.Month) + 1; //(後-前) + 1 為個數
            else
            {
                //若為跨年度則分為兩部分運算:
                //第一部份先判斷是否有年度跨越。用於跨超過兩年以上，例如 101.08 請到 103.02。若有則有效月份直接歸 0 (給12)
                //第二部分則拆算兩年之零散月份。例如 102.06 請到 103.06，則拆分成 102.06 ~ 102.12 及 103.01 ~ 103.06 兩部份計算
                if (isCrossTooMuchYear) iNoEffectMonthCount = 12;
                else
                {
                    //先計算跨日期之上半部
                    int iFrontPart = 0;
                    if (dtStartDate.Month == 12)
                        iFrontPart = 0; //若起始日期為12月，上半部直接給 0
                    else
                        iFrontPart = (12 - dtClearStartDate.Month) + 1;

                    //後計算跨日期之下半部
                    int iAfterPart = 0;
                    if (dtEndDate.Month == 1)
                        iAfterPart = 0; //若結束日期為1月，下半部直接給 0
                    else
                        iAfterPart = dtClearEndDate.Month;

                    iNoEffectMonthCount = iFrontPart + iAfterPart;
                }
            }

            //↑以上為無效月份原始數字計算
            //↓以下針對特殊情況進行調整

            //當為同月份時(8/__~8/__) (跨年度不用做同月份調整)
            if (!isCrossYear && dtStartDate.Month == dtEndDate.Month)
            {
                iNoEffectMonthCount = (iNoEffectMonthCount <= 0) ? 0 : iNoEffectMonthCount; //若為同月份相減則會為負數，故在此歸零
                int iThisMonthDayTotal = DateTime.DaysInMonth(dtStartDate.Year, dtStartDate.Month); ;

                //判斷日期差異數是否 >= 當月總天數(正常來講只會有 = 成立，不會有 > 成立)
                if (iDiffDays >= iThisMonthDayTotal)
                {
                    //差異超過當月日數，故當月份一整個月都有請假，故+1
                    iNoEffectMonthCount += 1;
                }
                else
                {
                    //日期差異未滿當月時，即為少於一個月
                    //原算式相減會為-1，故直接調整為 0
                    iNoEffectMonthCount = 0;
                }
            }
            //若為不同月份(就算跨年度也是要做月份調整)
            else
            {
                //判斷初始日期是否為當月1號
                //因此判斷式前提為月份不相等(else)，因此僅需判斷是否為1號開始，若是則把整月算入休假
                if (dtStartDate.Day == 1)
                    iNoEffectMonthCount += 1;
            }

            Int32 iEffectMonthCount = 12 - iNoEffectMonthCount; //進行減法後，即為今年特休計算上的有效月數
            return iEffectMonthCount;
        }

        /// <summary>
        /// 計算特休天數
        /// 20150408 Neil 建立
        /// </summary>
        /// <param name="iEffectMonthCount">當年度有效月份數</param>
        /// <param name="iOriginalDay">原始特休天數</param>
        /// <param name="iOriginalHours">原始特休小時數</param>
        public static void subParserResult(int iEffectMonthCount, ref int iOriginalDay, ref int iOriginalHours)
        {
            //主要公式
            Double tmp = MathHelper.Round(iOriginalDay * iEffectMonthCount / float.Parse("12.0"), 1);

            String[] k = tmp.ToString().Split('.');
            //重新設定當年度特休天數
            iOriginalDay = int.Parse(k[0]);

            //若有小數點
            if (k.Count() >= 2)
                if (int.Parse(k[1]) > 5) //若小數第一位大於5則多給一天
                    iOriginalDay += 1;
                else if (int.Parse(k[1]) > 0) //倘若沒大於5，但是有小數點，則多給 0.5 天 (一天 =8 Hour)
                    iOriginalHours = 4;
        }
    }
}