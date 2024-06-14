using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using TPPDDB._2_salary;
using TPPDDB.Helpers;
using TPPDDB.Models._3_ForLeave;
using TPPDDB._18_Online_Leave;
using TPPDDB.Models._18_Online_Work;
using System.Linq;

namespace TPPDDB._3_forleave
{
    /// <summary>
    /// 假勤與加班相關模組,大部分是加班
    /// </summary>
    public class CFService
    {
        /// <summary>
        /// 取得保留假及併計年資資料
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DataTable GetC_DLTBB(C_ForLeaveOvertime_KeepDay_Query query)
        {
            try
            {
                string strSQL = @"SELECT cdb.MZ_YEAR, adb.MZ_ID, adb.MZ_NAME, MZ_SDAY, MZ_SDAY_HOUR, MZ_SDAY2, MZ_SDAY2_HOUR, MZ_SDAY3, MZ_SDAY3_HOUR, NVL(MZ_TYEAR,'0') MZ_TYEAR, NVL(MZ_TMONTH,'0') MZ_TMONTH, MZ_MEMO, 
                                        NVL(MZ_RYEAR,'0') MZ_RYEAR, NVL(MZ_RMONTH,'0') MZ_RMONTH, dbo.to_number(CASE WHEN MZ_TBDV IS NULL THEN '999' WHEN MZ_TBDV='Z99' THEN '999' ELSE MZ_TBDV END) AS TBDV  
                                  FROM C_DLTBB cdb, A_DLBASE adb 
                                  WHERE cdb.MZ_ID = adb.MZ_ID ";
                List<SqlParameter> parameters = new List<SqlParameter>();
                if (!string.IsNullOrEmpty(query.MZ_YEAR))
                {
                    strSQL += "And cdb.MZ_YEAR=@MZ_YEAR ";
                    parameters.Add(new SqlParameter("MZ_YEAR", query.MZ_YEAR));
                }
                if (!string.IsNullOrEmpty(query.MZ_ID))
                {
                    strSQL += "And adb.MZ_ID=@MZ_ID ";
                    parameters.Add(new SqlParameter("MZ_ID", query.MZ_ID));
                }
                if (!string.IsNullOrEmpty(query.MZ_EXAD))
                {
                    strSQL += "And MZ_EXAD=@MZ_EXAD ";
                    parameters.Add(new SqlParameter("MZ_EXAD", query.MZ_EXAD));
                }
                if (!string.IsNullOrEmpty(query.MZ_EXUNIT))
                {
                    strSQL += "And MZ_EXUNIT=@MZ_EXUNIT ";
                    parameters.Add(new SqlParameter("MZ_EXUNIT", query.MZ_EXUNIT));
                }

                //資料排序
                strSQL += @" Order by TBDV,adb.MZ_EXUNIT,adb.MZ_ID,cdb.MZ_YEAR desc ";

                DataTable dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, parameters);
                LogModel.saveLog("CF", "S", strSQL, parameters, query.TPMFION, "查詢保留假及併計年資資料。");

                return dt;
            }
            catch (Exception ex)
            {
                LogModel.saveLog("CF", "S", ex.Message, new List<SqlParameter>(), query.TPMFION, "查詢保留假及併計年資異常。");
                throw ex;
            }
        }

        /// <summary>
        /// 取得加班系統代碼資料
        /// </summary>
        /// <param name="codeType"></param>
        /// <returns></returns>
        public DataTable GetOVERTIMECODE(string reColumns, string codeType = "", string codeValue = "", string codeDesc = "", string status = "Y")
        {
            try
            {
                string strSQL = string.Format(@"Select {0} From C_OVERTIME_CODE Where STATUS='{1}' ", reColumns, status);
                List<SqlParameter> parameters = new List<SqlParameter>();

                if (!string.IsNullOrEmpty(codeType))
                {
                    strSQL += "And CODE_TYPE=@codeType ";
                    parameters.Add(new SqlParameter("codeType", codeType));
                }
                if (!string.IsNullOrEmpty(codeValue))
                {
                    strSQL += "And CODE_VALUE=@codeValue ";
                    parameters.Add(new SqlParameter("codeValue", codeValue));
                }
                if (!string.IsNullOrEmpty(codeDesc))
                {
                    strSQL += "And CODE_DESC=@codeDesc ";
                    parameters.Add(new SqlParameter("codeDesc", codeDesc));
                }

                return o_DBFactory.ABC_toTest.Create_Table(strSQL, parameters);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #region 系統GridView Data
        /// <summary>
        /// 取得加班資料
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DataTable GetOverTimeBase(C_OverTime_Base_Input_Query query)
        {
            try
            {
                //檢查必要條件
                if (string.IsNullOrEmpty(query.SearchYM) || string.IsNullOrEmpty(query.MZ_ID))
                {
                    return null;
                }

                DateTime tmpD = new DateTime();
                //檢查資料格式
                if (ForDateTime.CheckDateTime(ForDateTime.TWDateToRCDate(query.SearchYM + "01", "yyyy/MM/dd"), "3") is null)
                {
                    return null;
                }
                else
                {
                    tmpD = (DateTime)ForDateTime.CheckDateTime(ForDateTime.TWDateToRCDate(query.SearchYM + "01", "yyyy/MM/dd"), "3");
                }

                //string strSQL = string.Format(@"
                //        Select (TWDATE_FORMAT(cob.OVER_DAY, 'yyy/mm/dd') + ' ' + cob.OVER_STIME + ' ~ ' + TWDATE_FORMAT(cob.OVER_DAY, 'yyy/mm/dd') + ' ' + cob.OVER_ETIME) OVER_TIME, 
                //                FLOOR(case when cob.OVERTIME_CHG='Y' then cob.OVERTIME_CHG_TOTAL else cob.OVER_TOTAL end / 60) OVER_HOUR, 
                //                MOD(case when cob.OVERTIME_CHG='Y' then cob.OVERTIME_CHG_TOTAL else cob.OVER_TOTAL end, 60) OVER_MINUTE, 
                //                FLOOR(PAY_HOUR / 60) PAY_HOUR , MOD(PAY_HOUR , 60) PAY_MIN, REST_HOUR , FLOOR(PRIZE_HOUR / 60) PRIZE_HOUR,  
                //                FLOOR((case when cob.OVERTIME_CHG='Y' then cob.OVERTIME_CHG_TOTAL else cob.OVER_TOTAL end - REST_HOUR - PRIZE_HOUR - PAY_HOUR)/60) LEFT_HOUR , MOD(case when cob.OVERTIME_CHG='Y' then cob.OVERTIME_CHG_TOTAL else cob.OVER_TOTAL end - REST_HOUR - PRIZE_HOUR - PAY_HOUR,60) LEFT_MIN,
                //                '' SURPLUS_HOUR, '' SURPLUS_MINUTE, 
                //                (Select C_STATUS_NAME From 
                //                  (Select cs.C_STATUS_NAME, coh.OVERTIME_SN, ROW_NUMBER() OVER (partition by OVERTIME_SN ORDER BY coh.O_SN desc) AS RN 
                //                    From C_OVERTIME_HISTORY coh 
                //                    INNER JOIN C_STATUS cs ON cs.C_STATUS_SN=coh.PROCESS_STATUS)
                //                  Where RN=1 And OVERTIME_SN=cob.SN
                //                ) OVER_STATUS, 
                //                TWDATE_FORMAT(OVER_DAY, 'YYY/MM/DD') FORMAT_DAY,
                //                cob.* 
                //                From C_OVERTIME_BASE cob
                //                Where 1=1 And OVER_DAY >='{0}' And OVER_DAY <'{1}' And MZ_ID='{2}' 
                //                Order by cob.OVER_DAY ",
                //                ForDateTime.RCDateToTWDate(tmpD.ToString("yyyy/MM/dd"), "yyyMMdd"),
                //                ForDateTime.RCDateToTWDate(tmpD.AddMonths(1).ToString("yyyy/MM/dd"), "yyyMMdd"),
                //                query.MZ_ID
                //                );

                string strSQL = string.Format(@"
                        Select (TWDATE_FORMAT(cob.OVER_DAY, 'yyy/mm/dd') + ' ' + cob.OVER_STIME + ' ~ ' + TWDATE_FORMAT(cob.OVER_DAY, 'yyy/mm/dd') + ' ' + cob.OVER_ETIME) OVER_TIME, 
                                FLOOR(case when cob.LOCK_FLAG='Y' then (case when cob.OVERTIME_TYPE='OTD' then cob.OVER_TOTAL else (case when cob.OVERTIME_CHG='Y' then cob.OVERTIME_CHG_TOTAL else cob.over_total END)END)  else (case when cob.OVERTIME_TYPE='OTD' then (case when cob.OVERTIME_CHG='Y' then cob.OVERTIME_CHG_TOTAL else cob.over_total END) else (case when cob.OVERTIME_CHG='Y' then cob.OVERTIME_CHG_TOTAL else cob.OVER_TOTAL END)END)end / 60) OVER_HOUR,  
                                MOD(case when cob.LOCK_FLAG='Y' then (case when cob.OVERTIME_TYPE='OTD' then cob.OVER_TOTAL else (case when cob.OVERTIME_CHG='Y' then cob.OVERTIME_CHG_TOTAL else cob.OVER_TOTAL END)END)  else (case when cob.OVERTIME_TYPE='OTD' then (case when cob.OVERTIME_CHG='Y' then cob.OVERTIME_CHG_TOTAL else cob.OVER_TOTAL END) else (case when cob.OVERTIME_CHG='Y' then cob.OVERTIME_CHG_TOTAL else cob.OVER_TOTAL END)END) end, 60) OVER_MINUTE, 
                                FLOOR(CASE WHEN lock_flag = 'Y' THEN 0 ELSE (case WHEN is_sign_return = 'Y' then 0 ELSE pay_hour END) END / 60) PAY_HOUR , 
                                MOD(CASE WHEN lock_flag = 'Y' THEN 0 ELSE (CASE WHEN is_sign_return = 'Y' then 0 ELSE pay_hour END) END, 60) PAY_MIN, 
                                --申請輪值加班費時數-小時
                                FLOOR(CASE WHEN lock_flag = 'Y' THEN 0 ELSE (case WHEN is_sign_return = 'Y' then 0 ELSE SHIFT_HOUR END) END / 60) SHIFT_HOUR , 
                                 --申請輪值加班費時數-分鐘
                                MOD(CASE WHEN lock_flag = 'Y' THEN 0 ELSE (CASE WHEN is_sign_return = 'Y' then 0 ELSE SHIFT_HOUR END) END, 60) SHIFT_MIN,
                                
                                --申請加班費時數加總(業務+輪值)-小時
                                FLOOR(CASE WHEN lock_flag = 'Y' THEN 0 ELSE (case WHEN is_sign_return = 'Y' then 0 ELSE pay_hour+SHIFT_HOUR END) END / 60) PAY_AND_SHIFT_HOUR , 
                                --申請加班費時數加總(業務+輪值)-分鐘
                                MOD(CASE WHEN lock_flag = 'Y' THEN 0 ELSE (CASE WHEN is_sign_return = 'Y' then 0 ELSE pay_hour+SHIFT_HOUR END) END, 60) PAY_AND_SHIFT_MIN , 
                                
                                REST_HOUR , PRIZE_HOUR, 
                                FLOOR((case when cob.LOCK_FLAG='Y' then (case when cob.OVERTIME_TYPE='OTD' then cob.OVER_TOTAL else (case when cob.OVERTIME_CHG='Y' then cob.OVERTIME_CHG_TOTAL else cob.over_total END)END)  else (case when cob.OVERTIME_TYPE='OTD' then (case when cob.OVERTIME_CHG='Y' then cob.OVERTIME_CHG_TOTAL else cob.over_total END) else (case when cob.OVERTIME_CHG='Y' then cob.OVERTIME_CHG_TOTAL else cob.OVER_TOTAL END)END)end - (CASE WHEN lock_flag = 'Y' THEN 0 ELSE (case WHEN is_sign_return = 'Y' THEN 0 ELSE rest_hour END) END) - (CASE WHEN lock_flag = 'Y' THEN 0 ELSE (case when is_sign_return = 'Y' THEN prize_hour ELSE 0 END) END) - (CASE WHEN lock_flag = 'Y' THEN 0 ELSE (CASE WHEN is_sign_return = 'Y' THEN 0 ELSE PAY_HOUR+SHIFT_HOUR END) END))/60) LEFT_HOUR ,
                                MOD(case when cob.LOCK_FLAG='Y' then (case when cob.OVERTIME_TYPE='OTD' then cob.OVER_TOTAL else (case when cob.OVERTIME_CHG='Y' then cob.OVERTIME_CHG_TOTAL else cob.over_total END)END)  else (case when cob.OVERTIME_TYPE='OTD' then (case when cob.OVERTIME_CHG='Y' then cob.OVERTIME_CHG_TOTAL else cob.over_total END) else (case when cob.OVERTIME_CHG='Y' then cob.OVERTIME_CHG_TOTAL else cob.OVER_TOTAL END)END)end - (CASE WHEN lock_flag = 'Y' THEN 0 ELSE (case when is_sign_return = 'Y' THEN 0 ELSE rest_hour END) END) - (CASE WHEN lock_flag = 'Y' THEN 0 ELSE (case WHEN is_sign_return = 'Y' THEN prize_hour ELSE 0 END) END) - (CASE WHEN lock_flag = 'Y' THEN 0 ELSE (case WHEN is_sign_return = 'Y' THEN 0 ELSE PAY_HOUR+SHIFT_HOUR END) END),60) LEFT_MIN,
                                '' SURPLUS_HOUR, '' SURPLUS_MINUTE, 
                                (Select C_STATUS_NAME From 
                                  (Select cs.C_STATUS_NAME, coh.OVERTIME_SN, ROW_NUMBER() OVER (partition by OVERTIME_SN ORDER BY coh.O_SN desc) AS RN 
                                    From C_OVERTIME_HISTORY coh 
                                    INNER JOIN C_STATUS cs ON cs.C_STATUS_SN=coh.PROCESS_STATUS)
                                  Where RN=1 And OVERTIME_SN=cob.SN
                                ) OVER_STATUS, 
                                TWDATE_FORMAT(OVER_DAY, 'YYY/MM/DD') FORMAT_DAY,
                                cob.* 
                                From C_OVERTIME_BASE cob
                                Where 1=1 And OVER_DAY >='{0}' And OVER_DAY <'{1}' And MZ_ID='{2}' 
                                Order by cob.OVER_DAY ",
                                ForDateTime.RCDateToTWDate(tmpD.ToString("yyyy/MM/dd"), "yyyMMdd"),
                                ForDateTime.RCDateToTWDate(tmpD.AddMonths(1).ToString("yyyy/MM/dd"), "yyyMMdd"),
                                query.MZ_ID
                                );

                DataTable dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "C_OVERTIME_BASE");

                // sam test new add
                //計算累計剩餘時分
                int sup_Total = 0;
                for (int i = dt.Rows.Count - 1; i >= 0; i--)
                {
                    sup_Total += Convert.ToInt32(dt.Rows[i]["SURPLUS_TOTAL"]);

                    string OPTYPE = dt.Rows[i]["OPTYPE"].ToString();
                    if (OPTYPE == "0")
                    {
                        dt.Rows[i]["OPTYPE"] = "否";
                    }
                    else if (OPTYPE == "1")
                    {
                        dt.Rows[i]["OPTYPE"] = "是";
                    }
                    else
                    {
                        dt.Rows[i]["OPTYPE"] = OPTYPE;
                    }

                    dt.Rows[i]["SURPLUS_HOUR"] = sup_Total / 60;
                    dt.Rows[i]["SURPLUS_MINUTE"] = sup_Total % 60;
                }

                return dt;
            }
            catch (Exception ex)
            {
                LogModel.saveLog("CF", "S", ex.Message, new List<SqlParameter>(), query.TPMFION, "查詢加班資料異常。");
                throw ex;
            }
        }


        /// <summary>
        /// 取得加班資料
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DataTable GetOverTimeBase_data(C_OverTime_Base_Input_Query query)
        {
            try
            {
                //檢查必要條件
                //if (string.IsNullOrEmpty(query.SearchYM) || string.IsNullOrEmpty(query.MZ_ID))
                if (string.IsNullOrEmpty(query.MZ_ID))
                {
                    return null;
                }

                string strSQL = "";
                //西元轉民國年
                var tmp = new System.Globalization.TaiwanCalendar();
                string today = DateTime.Now.ToString("yyyyMMdd");
                //去年的今天
                //string today_LastYear = string.Format("{0}{1}{2}", tmp.GetYear(DateTime.Now.AddYears(-1)), today.Substring(4, 2), today.Substring(6, 2));
                DataTable dt;
                //OVER_DAY不是5碼?那就是7碼
                if (query.OVER_DAY.Length != 5)  //假別是否11(OTB 加班補休)
                {
                    strSQL = string.Format(@" 
                             select OVERTIME_TYPE,OVER_DAY, floor(SURPLUS_TOTAL / 60)  as SURPLUS_HOUR , floor(SURPLUS_TOTAL / 60)  as SURPLUS_TOTAL From C_OVERTIME_BASE
                                  Where MZ_ID='{0}' And OVER_DAY = '{1}' And SURPLUS_TOTAL > 0 And OVERTIME_TYPE is not null "
                                 , query.MZ_ID, query.OVER_DAY);
                    dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "C_OVERTIME_BASE");
                }
                //OVER_DAY 5碼
                else
                {
                    //如果After_OVER_DAY沒值,代表沒指定,正常不該發生
                    if (query.OVERDAY_LIMIT.IsNullOrEmpty())
                    {
                        //那就以當月一號為主,也就是當月全撈
                        query.OVERDAY_LIMIT = query.OVER_DAY + "01";
                    }

                    strSQL = string.Format(@" 
                                 select  substr(OVER_DAY,0,5) As OVER_TIME , substr(OVER_DAY,0,5) as OVER_DAY, floor(SUM(SURPLUS_TOTAL) / 60)  as SURPLUS_HOUR , floor(SUM(SURPLUS_TOTAL) / 60)  as SURPLUS_TOTAL From C_OVERTIME_BASE
                                    Where MZ_ID='{0}' And substr(OVER_DAY,0,5) = '{1}' And OVER_DAY >= '" + query.OVERDAY_LIMIT + "' And SURPLUS_TOTAL > 0 And  OVERTIME_TYPE in( 'OTB', 'OTT' )   group by  substr(OVER_DAY,0,5) having sum(SURPLUS_TOTAL) >= 60 order by  substr(OVER_DAY,0,5) "
                                    , query.MZ_ID, query.OVER_DAY);
                    dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "C_OVERTIME_BASE");
                    dt.Columns.Add("OVERTIME_TYPE", typeof(String));
                }



                //DataTable dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "C_OVERTIME_BASE");

                // sam test new add
                //計算累計剩餘時分
                int sup_Total = 0;
                for (int i = dt.Rows.Count - 1; i >= 0; i--)
                {
                    sup_Total += Convert.ToInt32(dt.Rows[i]["SURPLUS_TOTAL"]);

                    string tmptype = dt.Rows[i]["OVERTIME_TYPE"].ToString();
                    if (tmptype == "")
                    {   //如果沒有加班類型的話,一律視同 OTB業務加班
                        dt.Rows[i]["OVERTIME_TYPE"] = "OTB"; //加班補休
                    }
                    #region
                    //string OPTYPE = dt.Rows[i]["OPTYPE"].ToString();
                    //if (OPTYPE == "0")
                    //{
                    //    dt.Rows[i]["OPTYPE"] = "否";
                    //}
                    //else if (OPTYPE == "1")
                    //{
                    //    dt.Rows[i]["OPTYPE"] = "是";
                    //}
                    //else
                    //{
                    //    dt.Rows[i]["OPTYPE"] = OPTYPE;
                    //}
                    #endregion
                    dt.Rows[i]["SURPLUS_HOUR"] = sup_Total;
                    // dt.Rows[i]["SURPLUS_MINUTE"] = sup_Total % 60;
                }

                return dt;
            }
            catch (Exception ex)
            {
                LogModel.saveLog("CF", "S", ex.Message, new List<SqlParameter>(), query.TPMFION, "查詢加班資料異常。");
                throw ex;
            }
        }
        /// <summary>
        /// 取得可休加班日期區間
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DataTable GetOverTimeBaseForRESTDATE_SEARCH(C_ForLeaveOvertime_RESTDATE_SEARCH_Query query)
        {
            try
            {
                if (string.IsNullOrEmpty(query.MZ_ID))
                {
                    return null;
                }

                //string strSQL = @"Select (MIN_DAY + '~' + MAX_DAY) As OVER_TIME, SURPLUS_HOUR From 
                //                    (Select MIN(OVER_DAY) MIN_DAY From C_OVERTIME_BASE 
                //                        Where MZ_ID=@MZ_ID And SURPLUS_TOTAL>0 And OVER_DAY>:OVER_DAY And OVERTIME_TYPE is not null), 
                //                    (Select MAX(OVER_DAY) MAX_DAY From C_OVERTIME_BASE 
                //                        Where MZ_ID=@MZ_ID And SURPLUS_TOTAL>0 And OVER_DAY>:OVER_DAY And OVERTIME_TYPE is not null), 
                //                    (Select SUM(FLOOR(SUM(SURPLUS_TOTAL / 60))) SURPLUS_HOUR From C_OVERTIME_BASE 
                //                        Where MZ_ID=@MZ_ID And OVER_DAY>:OVER_DAY And OVERTIME_TYPE is not null 
                //                    Group by substr(OVER_DAY,0,5)) ";
                //20210326 - 原來用的 FLOOR 負數 時有問題 (如 FLOOR(-1.23) 會變 -2)
                //string strSQL = @"Select (MIN_DAY + '~' + MAX_DAY) As OVER_TIME, SURPLUS_HOUR From 
                //                    (Select MIN(OVER_DAY) MIN_DAY From C_OVERTIME_BASE 
                //                        Where MZ_ID=@MZ_ID And SURPLUS_TOTAL>0 And OVER_DAY>:OVER_DAY And OVERTIME_TYPE is not null), 
                //                    (Select MAX(OVER_DAY) MAX_DAY From C_OVERTIME_BASE 
                //                s        Where MZ_ID=@MZ_ID And SURPLUS_TOTAL>0 And OVER_DAY>:OVER_DAY And OVERTIME_TYPE is not null), 
                //                    (Select floor(SUM(SURPLUS_TOTAL / 60)) SURPLUS_HOUR From C_OVERTIME_BASE 
                //                        Where MZ_ID=@MZ_ID And OVER_DAY>:OVER_DAY And OVERTIME_TYPE is not null 
                //                    ) ";
                string strSQL = "";
                string over_type = "";
                if (query.MZ_CODE != "")
                {
                    switch (query.MZ_CODE)
                    {
                        case "11"://加班補休
                            over_type = "OTB";
                            strSQL = @"Select substr(OVER_DAY,0,5) As OVER_TIME , substr(OVER_DAY,0,5) as OVER_DAY,  floor(SUM(SURPLUS_TOTAL) / 60)  as SURPLUS_HOUR  From C_OVERTIME_BASE
                                      Where ( OVERTIME_TYPE = @over_type OR OVERTIME_TYPE = 'OTT') and MZ_ID=@MZ_ID And OVER_DAY>=@OVER_DAY And OVER_DAY < @MZ_IDATE1 And SURPLUS_TOTAL > 0 And OVERTIME_TYPE is not null group by substr(OVER_DAY,0,5) having sum(SURPLUS_TOTAL) >= 60 order by substr(OVER_DAY,0,5) 
                                     ";
                            break;
                        case "22"://值日補休
                            over_type = "OTD";
                            strSQL = @"Select OVER_DAY,  floor(SUM(SURPLUS_TOTAL / 60))  as SURPLUS_HOUR  From C_OVERTIME_BASE
                                  Where OVERTIME_TYPE = @over_type and MZ_ID=@MZ_ID And OVER_DAY>=@OVER_DAY And OVER_DAY < @MZ_IDATE1 And SURPLUS_TOTAL > 0 And OVERTIME_TYPE is not null group by OVER_DAY having sum(SURPLUS_TOTAL) >= 60 order by OVER_DAY 
                                     ";
                            break;
                        case "16"://超勤補休
                            over_type = "OTU";
                            strSQL = @"Select OVER_DAY,  floor(SUM(SURPLUS_TOTAL / 60))  as SURPLUS_HOUR  From C_OVERTIME_BASE
                                  Where OVERTIME_TYPE = @over_type and MZ_ID=@MZ_ID And OVER_DAY>=@OVER_DAY And OVER_DAY < @MZ_IDATE1  And SURPLUS_TOTAL > 0 And OVERTIME_TYPE is not null group by OVER_DAY having sum(SURPLUS_TOTAL) >= 60 order by OVER_DAY 
                                     ";
                            break;
                    }

                    //if (query.MZ_CODE == "11")
                    //{
                    //    over_type = "OTB";
                    //}
                    //if (query.MZ_CODE == "22")
                    //{
                    //    over_type = "OTD";
                    //}
                    //if (query.MZ_CODE == "16")
                    //{
                    //    over_type = "OTU";

                    //}
                    ////strSQL = @"Select substr(OVER_DAY,0,5) As OVER_TIME , substr(OVER_DAY,0,5) as OVER_DAY,  floor(SUM(SURPLUS_TOTAL / 60))  as SURPLUS_HOUR  From C_OVERTIME_BASE
                    ////              Where OVERTIME_TYPE = @over_type and MZ_ID=@MZ_ID And OVER_DAY>:OVER_DAY And SURPLUS_TOTAL > 0 And OVERTIME_TYPE is not null group by substr(OVER_DAY,0,5) having sum(SURPLUS_TOTAL) >= 60 order by substr(OVER_DAY,0,5) 
                    ////                 ";
                    //strSQL = @"Select OVER_DAY,  floor(SUM(SURPLUS_TOTAL / 60))  as SURPLUS_HOUR  From C_OVERTIME_BASE
                    //              Where OVERTIME_TYPE = @over_type and MZ_ID=@MZ_ID And OVER_DAY>:OVER_DAY And SURPLUS_TOTAL > 0 And OVERTIME_TYPE is not null group by OVER_DAY having sum(SURPLUS_TOTAL) >= 60 order by OVER_DAY 
                    //                 ";
                }
                else
                {
                    strSQL = @"Select substr(OVER_DAY,0,5) As OVER_TIME , substr(OVER_DAY,0,5) as OVER_DAY,  floor(SUM(SURPLUS_TOTAL) / 60)  as SURPLUS_HOUR  From C_OVERTIME_BASE
                                  Where MZ_ID=@MZ_ID And OVER_DAY>=@OVER_DAY And OVER_DAY < @MZ_IDATE1 And SURPLUS_TOTAL > 0 And OVERTIME_TYPE is not null group by substr(OVER_DAY,0,5) having sum(SURPLUS_TOTAL) >= 60 order by substr(OVER_DAY,0,5) 
                                     ";
                    //strSQL = @"Select substr(OVER_DAY,0,5) as OVER_DAY,  floor(SUM(SURPLUS_TOTAL / 60))  as SURPLUS_HOUR  From C_OVERTIME_BASE
                    //                  Where MZ_ID=@MZ_ID And OVER_DAY>:OVER_DAY And SURPLUS_TOTAL > 0 And OVERTIME_TYPE is not null group by OVER_DAY having sum(SURPLUS_TOTAL) >= 60 order by OVER_DAY
                    //                     ";
                }
                //如果沒有定義申請補休日,則取一個最大值
                if (query.MZ_IDATE1.IsNullOrEmpty())
                {
                    query.MZ_IDATE1 = "9991231";
                }

                List<SqlParameter> parameters = new List<SqlParameter>()
                {
                    new SqlParameter("over_type", over_type),
                    new SqlParameter("MZ_ID", query.MZ_ID),
                    new SqlParameter("OVER_DAY", query.KeepDay),
                    new SqlParameter("MZ_IDATE1", query.MZ_IDATE1)
                };

                DataTable dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, parameters);

                return dt;
            }
            catch (Exception ex)
            {
                LogModel.saveLog("CF", "S", ex.Message, new List<SqlParameter>(), query.TPMFION, "取得可休加班日期區間異常。");
                throw ex;
            }
        }
        /// <summary>
        /// 取得主管簽核人員資料
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DataTable GetOnlineCheckManager(C_OverTime_Base_Input_Query query)
        {
            try
            {
                if (string.IsNullOrEmpty(query.SearchAD) && string.IsNullOrEmpty(query.SearchUnit))
                {
                    return null;
                }

                string strSQL = string.Format(@"SELECT crm.MZ_ID, akc.MZ_KCHI As MZ_OCCC, adb.mz_name 
                                                FROM C_REVIEW_MANAGEMENT crm
                                                left join A_DLBASE adb on adb.MZ_ID = crm.MZ_ID
                                                left join A_KTYPE akc on akc.MZ_KTYPE='26' And akc.MZ_KCODE = crm.MZ_OCCC 
                                                Where 1=1 AND crm.REVIEW_LEVEL='2' ");
                List<SqlParameter> parameters = new List<SqlParameter>();
                if (!string.IsNullOrEmpty(query.SearchAD))
                {
                    strSQL += "And crm.MZ_EXAD=@SearchAD ";
                    parameters.Add(new SqlParameter("SearchAD", query.SearchAD));
                }
                if (!string.IsNullOrEmpty(query.SearchUnit))
                {
                    strSQL += "And crm.MZ_EXUNIT=@SearchUnit ";
                    parameters.Add(new SqlParameter("SearchUnit", query.SearchUnit));
                }

                DataTable dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, parameters);

                return dt;
            }
            catch (Exception ex)
            {
                LogModel.saveLog("CF", "S", ex.Message, new List<SqlParameter>(), query.TPMFION, "查詢主管簽核人員異常。");
                throw ex;
            }
        }
        /// <summary>
        /// 取得申請加班費資料
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DataTable GetOverTimeBaseByPay(C_OverTime_PAY_Check_Query query)
        {
            try
            {
                string strSQL = @"Select Case when cob.PAY_CHK='Y' then '已審核' else '未審核' end PAY_CHK, adb.MZ_NAME,
                                    TWDATE_FORMAT(cob.OVER_DAY, 'yyy/mm/dd') OVER_TIME, cob.REASON, 
                                    FLOOR(cob.PAY_HOUR / 60) PAY_HOUR, cob.PAY_UNIT, cob.PAY_SUM, cob.MZ_ID, cob.OVER_DAY
                                  From C_OVERTIME_BASE cob
                                  left join A_DLBASE adb on adb.MZ_ID = cob.MZ_ID 
                                  Where cob.PAY_HOUR>:PAY_HOUR ";
                List<SqlParameter> parameters = new List<SqlParameter>()
                {
                    new SqlParameter("PAY_HOUR", value: 0)
                };

                if (!string.IsNullOrEmpty(query.Search_AD))
                {
                    strSQL += "And cob.MZ_EXAD=@Search_AD ";
                    parameters.Add(new SqlParameter("Search_AD", query.Search_AD));
                }
                if (!string.IsNullOrEmpty(query.Search_UNIT))
                {
                    strSQL += "And cob.MZ_EXUNIT=@Search_UNIT ";
                    parameters.Add(new SqlParameter("Search_UNIT", query.Search_UNIT));
                }
                if (!string.IsNullOrEmpty(query.Search_YM))
                {
                    if (ForDateTime.CheckDateTime(ForDateTime.TWDateToRCDate(query.Search_YM + "01", "yyyy/MM/dd"), "3") is null)
                    {
                        throw new Exception("查詢年月資料異常。");
                    }
                    else
                    {
                        string intervalS = query.Search_YM + "01";
                        string intervalE = query.Search_YM + "31";
                        strSQL += "And cob.OVER_DAY>=@intervalS And cob.OVER_DAY<=@intervalE ";
                        parameters.Add(new SqlParameter("intervalS", intervalS));
                        parameters.Add(new SqlParameter("intervalE", intervalE));
                    }
                }
                if (!string.IsNullOrEmpty(query.Search_ID))
                {
                    strSQL += "And cob.MZ_ID=@Search_ID ";
                    parameters.Add(new SqlParameter("Search_ID", query.Search_ID));
                }
                if (!string.IsNullOrEmpty(query.Search_CHK))
                {
                    strSQL += "And cob.PAY_CHK=@Search_CHK ";
                    parameters.Add(new SqlParameter("Search_CHK", query.Search_CHK));
                }


                DataTable dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, parameters);

                return dt;
            }
            catch (Exception ex)
            {
                LogModel.saveLog("CF", "S", ex.Message, new List<SqlParameter>(), query.TPMFION, "查詢申請加班費資料異常。");
                throw ex;
            }
        }
        /// <summary>
        /// 取得申請敘獎加班資料
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DataTable GetOverTimeBaseByPrize(C_OverTime_PRIZE_Input_Query query)
        {
            try
            {
                if (string.IsNullOrEmpty(query.Year))
                {
                    return null;
                }

                //處理年度條件
                string halfYearS = string.Empty;
                string halfYearE = string.Empty;
                switch (query.Year_Interval)
                {
                    case "0": //上半年
                        halfYearS = query.Year + "0101";
                        halfYearE = query.Year + "0630";
                        break;
                    case "1": //下半年
                        halfYearS = query.Year + "0701";
                        halfYearE = query.Year + "1231";
                        break;
                }

                string strSQL = string.Empty;
                List<SqlParameter> parameters;
                if (!string.IsNullOrEmpty(query.MZ_ID))
                {
                    //取得單人各月份資料
                    //strSQL = @"Select substr(OVER_DAY,0,5) OVER_MONTH, FLOOR(SUM(SURPLUS_TOTAL / 60)) SURPLUS_HOUR, FLOOR(SUM(PRIZE_HOUR / 60)) PRIZE_HOUR 
                    //            From C_OVERTIME_BASE 
                    //            Where MZ_ID=@MZ_ID And OVER_DAY>=@halfYearS And OVER_DAY<=@halfYearE 
                    //            Group by substr(OVER_DAY,0,5) ";
                    //20210326 - 原來用的 FLOOR 負數 時有問題 (如 FLOOR(-1.23) 會變 -2)
                    //strSQL = @"Select substr(OVER_DAY,0,5) OVER_MONTH, floor(SUM(SURPLUS_TOTAL / 60)) SURPLUS_HOUR, floor(SUM(PRIZE_HOUR / 60)) PRIZE_HOUR 
                    //            From C_OVERTIME_BASE 
                    //            Where MZ_ID=@MZ_ID And OVER_DAY>=@halfYearS And OVER_DAY<=@halfYearE 
                    //             ";
                    strSQL = @"Select substr(OVER_DAY,0,5) OVER_MONTH, floor(SUM(SURPLUS_TOTAL / 60)) SURPLUS_HOUR, floor(SUM(PRIZE_HOUR) / 60) PRIZE_HOUR 
                                From C_OVERTIME_BASE 
                                Where OVERTIME_Type in ('OTB','OTT','OTD') and MZ_ID=@MZ_ID And OVER_DAY>=@halfYearS And OVER_DAY<=@halfYearE  group by substr(OVER_DAY,0,5) order by substr(OVER_DAY,0,5)
                                 ";
                    parameters = new List<SqlParameter>()
                    {
                        new SqlParameter("MZ_ID", query.MZ_ID),
                        new SqlParameter("halfYearS", halfYearS),
                        new SqlParameter("halfYearE", halfYearE)
                    };
                }
                else
                {
                    //取得機關單位全部資料
                    //strSQL = @"Select cob.*, NVL(cop.PRIZE_AMOUNT, 0) PRIZE_AMOUNT, 
                    //            CASE WHEN akAD.MZ_KCHI='新北市政府警察局' THEN '警察局' ELSE replace(akAD.MZ_KCHI,'新北市政府警察局') End AD_NAME,
                    //            akUNIT.MZ_KCHI UNIT_NAME,
                    //            (Select SUM(FLOOR(SUM(SURPLUS_TOTAL / 60))) From C_OVERTIME_BASE 
                    //             Where MZ_ID=cob.MZ_ID And OVER_DAY>=@halfYearS And OVER_DAY<=@halfYearE Group by substr(OVER_DAY,0,5)
                    //            ) SURPLUS_HOUR 
                    //           From (Select tmp.MZ_ID, ad.MZ_NAME, tmp.MZ_EXAD, tmp.MZ_EXUNIT From C_OVERTIME_BASE tmp
                    //                 left join A_DLBASE ad on ad.MZ_ID=tmp.MZ_ID
                    //                 Where tmp.OVER_DAY>=@halfYearS And tmp.OVER_DAY<=@halfYearE 
                    //                 Group by tmp.MZ_ID, ad.MZ_NAME, tmp.MZ_EXAD, tmp.MZ_EXUNIT
                    //                ) cob
                    //           left join C_OVERTIME_PRIZE cop on cop.MZ_ID=cob.MZ_ID And cop.APPLY_YEAR=@Year And cop.APPLY_INTERVAL=@Year_Interval 
                    //           left join A_KTYPE akAD on akAD.MZ_KTYPE='04' And akAD.MZ_KCODE=cob.MZ_EXAD 
                    //           left join A_KTYPE akUNIT on akUNIT.MZ_KTYPE='25' And akUNIT.MZ_KCODE=cob.MZ_EXUNIT 
                    //           Where 1=1 ";
                    //20210326 - 原來用的 FLOOR 負數 時有問題 (如 FLOOR(-1.23) 會變 -2)
                    strSQL = @"Select cob.*, NVL(cop.PRIZE_AMOUNT, 0) PRIZE_AMOUNT, 
                                CASE WHEN akAD.MZ_KCHI='新北市政府警察局' THEN '警察局' ELSE replace(akAD.MZ_KCHI,'新北市政府警察局') End AD_NAME,
                                akUNIT.MZ_KCHI UNIT_NAME,
                                (Select floor(SUM(SURPLUS_TOTAL / 60)) From C_OVERTIME_BASE 
                                 Where MZ_ID=cob.MZ_ID And OVER_DAY>=@halfYearS And OVER_DAY<=@halfYearE 
                                ) SURPLUS_HOUR 
                               From (Select tmp.MZ_ID, ad.MZ_NAME, tmp.MZ_EXAD, tmp.MZ_EXUNIT From C_OVERTIME_BASE tmp
                                     left join A_DLBASE ad on ad.MZ_ID=tmp.MZ_ID
                                     Where tmp.OVER_DAY>=@halfYearS And tmp.OVER_DAY<=@halfYearE 
                                     Group by tmp.MZ_ID, ad.MZ_NAME, tmp.MZ_EXAD, tmp.MZ_EXUNIT
                                    ) cob
                               left join C_OVERTIME_PRIZE cop on cop.MZ_ID=cob.MZ_ID And cop.APPLY_YEAR=@Year And cop.APPLY_INTERVAL=@Year_Interval 
                               left join A_KTYPE akAD on akAD.MZ_KTYPE='04' And akAD.MZ_KCODE=cob.MZ_EXAD 
                               left join A_KTYPE akUNIT on akUNIT.MZ_KTYPE='25' And akUNIT.MZ_KCODE=cob.MZ_EXUNIT 
                               Where 1=1 ";
                    parameters = new List<SqlParameter>()
                    {
                        new SqlParameter("halfYearS", halfYearS),
                        new SqlParameter("halfYearE", halfYearE),
                        new SqlParameter("Year", query.Year),
                        new SqlParameter("Year_Interval", query.Year_Interval)
                    };
                    if (!string.IsNullOrEmpty(query.Search_AD))
                    {
                        strSQL += "And cob.MZ_EXAD=@Search_AD ";
                        parameters.Add(new SqlParameter("Search_AD", query.Search_AD));
                    }
                    if (!string.IsNullOrEmpty(query.Search_Unit))
                    {
                        strSQL += "And cob.MZ_EXUNIT=@Search_Unit ";
                        parameters.Add(new SqlParameter("Search_Unit", query.Search_Unit));
                    }
                }

                DataTable dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, parameters);

                return dt;
            }
            catch (Exception ex)
            {
                LogModel.saveLog("CF", "S", ex.Message, new List<SqlParameter>(), query.TPMFION, "查詢申請敘獎加班資料異常。");
                throw ex;
            }
        }
        #endregion

        /// <summary>
        /// 取得單一天加班資料
        /// </summary>
        /// <param name="MZ_ID"></param>
        /// <param name="OVER_DAY"></param>
        /// <returns></returns>
        public C_OVERTIME_BASE_Model GetOverTimeBase(string MZ_ID, string OVER_DAY, string TPMFION = "")
        {
            try
            {
                if (string.IsNullOrEmpty(MZ_ID) || string.IsNullOrEmpty(OVER_DAY))
                {
                    throw new Exception("查詢條件為空，無法查詢。");
                }

                string strSQL = @"Select * From C_OVERTIME_BASE Where MZ_ID=@MZ_ID And OVER_DAY=@OVER_DAY ";
                List<SqlParameter> parameters = new List<SqlParameter>()
                {
                    new SqlParameter("MZ_ID", MZ_ID),
                    new SqlParameter("OVER_DAY", OVER_DAY)
                };

                DataTable dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, parameters);

                return DataHelpers.GetEntities<C_OVERTIME_BASE_Model>(dt).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogModel.saveLog("CF", "S", ex.Message, new List<SqlParameter>(), TPMFION, "取得單一天加班資料異常。");
                throw ex;
            }
        }
        /// <summary>
        /// 加班資料重複檢查
        /// </summary>
        /// <param name="query"></param>
        /// <returns>true:未重複 false:重複資料</returns>
        public bool Check_OverTimeBase_Repeat(C_OverTime_Base_Input_Query query)
        {
            try
            {
                bool result = false;
                //檢查資料
                if (string.IsNullOrEmpty(query.MZ_ID) || string.IsNullOrEmpty(query.OVER_DAY))
                {
                    throw new Exception("檢核資料為空值。");
                }

                string strSQL = @"
Select Count(0) From C_OVERTIME_BASE 
Where 1=1 
And MZ_ID=@MZ_ID 
And OVER_DAY=@OVER_DAY 
";
                List<SqlParameter> parameters = new List<SqlParameter>()
                {
                    new SqlParameter("MZ_ID", query.MZ_ID),
                    new SqlParameter("OVER_DAY", query.OVER_DAY)
                };

                if (o_DBFactory.ABC_toTest.Get_First_Field(strSQL, parameters) == "0")
                {
                    result = true;
                }

                return result;
            }
            catch (Exception ex)
            {
                LogModel.saveLog("CF", "S", ex.Message, new List<SqlParameter>(), query.TPMFION, "檢核加班資料重複異常。");

                throw ex;
            }
        }
        /// <summary>
        /// 取得例外人員資料
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DataTable GetOVERTIME_EXCARD(C_OverTime_Base_Input_Query query)
        {
            try
            {
                if (string.IsNullOrEmpty(query.MZ_ID))
                {
                    return null;
                }

                string strSQL = @"Select * From C_OVERTIME_EXCARD Where MZ_ID=@MZ_ID ";
                List<SqlParameter> parameters = new List<SqlParameter>()
                {
                    new SqlParameter("MZ_ID", query.MZ_ID)
                };

                DataTable dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, parameters);

                return dt;
            }
            catch (Exception ex)
            {
                LogModel.saveLog("CF", "S", ex.Message, new List<SqlParameter>(), query.TPMFION, "查詢加班指紋例外人員異常。");
                throw ex;
            }
        }

        /// <summary>
        /// 取得每小時薪資金額,用於計算加班費 回傳雙經度浮點數
        /// </summary>
        /// <param name="police">員警資料</param>
        /// <returns></returns>
        public Double GetPayUnit(_2_salary.Police police)
        {
            /*
計算邏輯:            
A非員警: (薪資+專業加給+主管加給)/240 *1.33    (先除後乘)
B約僱約聘:兩者之一取大
　1.MZ_SPT乘上135除240
　2.(薪資+專業加給+主管加給)/240
C其他員警: (薪資+專業加給+主管加給)/240

職稱代碼MZ_OCCC與身分定義
#約僱約聘如下
1179	約僱
I100	聘用

#非員警如下
4769    臨時人員
Z011	臨時人員
Z014	駕駛
Z015	工友
Z016	技工
Z050	義警
Z051	民防
Z052	義交
Z053	義刑
Z054	替代役
Z055	實習生
Z065	管理員
             */

            if (police == null)
            {
                return 0;
            }

            if (police.occc.Substring(0, 2) == "Z0")
            {
                //計算每小時薪資,並做四捨五入,使用AwayFromZero才是真正的四捨五入
                return Math.Round(Convert.ToDouble(police.salary + police.profess + police.boss) / 240 * 1.33, 0, MidpointRounding.AwayFromZero);
            }
            else
            {
                //1179	約僱  I100	聘用
                bool is約僱聘用 = (police.occc == "1179") || (police.occc == "I100");
                if (is約僱聘用)
                {
                    //特殊:約僱聘用的計算條件有兩段,兩者之一取大
                    // 1.MZ_SPT乘上135除240
                    double temp1 = Math.Round(Convert.ToDouble(int.Parse(police.spt) * 135) / 240, 0, MidpointRounding.AwayFromZero);
                    // 2.「B_BASE的(SALARYPAY+PROFESSPAY+BOSSPAY)」
                    double temp2 = Math.Round(Convert.ToDouble(police.salary + police.profess + police.boss) / 240 , 0, MidpointRounding.AwayFromZero);
                    return Math.Max(temp1, temp2);

                }                
                //臨時人員
                else if (police.occc == "4769")
                {
                    return Math.Round(Convert.ToDouble(police.salary + police.profess + police.boss) * 1.33 / 240, 0, MidpointRounding.AwayFromZero);
                }
                else
                {
                    return Math.Round(Convert.ToDouble(police.salary + police.profess + police.boss) / 240, 0, MidpointRounding.AwayFromZero);
                }
            }
        }

        /// <summary>
        /// 取得每小時薪資金額,用於計算加班費 回傳雙經度浮點數
        /// </summary>
        /// <param name="MZ_ID">人員身分證</param>
        /// <returns></returns>
        public Double GetPayUnit_Double(String MZ_ID)
        {
            _2_salary.Police police = new TPPDDB._2_salary.Police(MZ_ID);

            return GetPayUnit(police);
        }

        /// <summary>
        /// 取得每小時金額(薪資) 回傳十進位數字
        /// </summary>
        /// <param name="MZ_ID">人員身分證</param>
        /// <returns></returns>
        public decimal GetPayUnit(string MZ_ID)
        {
            Double PayUnit = GetPayUnit_Double(MZ_ID);
            return Convert.ToDecimal(PayUnit);
        }

        /// <summary>
        /// 根據員警資料 計算 每小時薪資金額,用於計算加班費
        /// </summary>
        /// <param name="police"></param>
        /// <returns></returns>
        public int GetPayUnit_int(_2_salary.Police police)
        {
            Double PayUnit = GetPayUnit(police);
            return Convert.ToInt32(PayUnit);
        }

        /// <summary>
        /// 取得每小時薪資金額,用於計算加班費 回傳整數
        /// </summary>
        /// <param name="MZ_ID">人員身分證</param>
        /// <returns></returns>
        public int GetPayUnit_int(string MZ_ID)
        {
            Double PayUnit = GetPayUnit_Double(MZ_ID);
            return Convert.ToInt32(PayUnit);
        }

        /// <summary>
        /// 取得當月加班費資料
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DataTable GetMonthPay(C_OverTime_Base_Input_Query query)
        {
            try
            {
                if (string.IsNullOrEmpty(query.MZ_ID) || string.IsNullOrEmpty(query.SearchYM))
                {
                    return null;
                }
                DateTime tmpD = new DateTime();
                //檢查資料格式
                if (ForDateTime.CheckDateTime(ForDateTime.TWDateToRCDate(query.SearchYM + "01", "yyyy/MM/dd"), "3") is null)
                {
                    return null;
                }
                else
                {
                    tmpD = (DateTime)ForDateTime.CheckDateTime(ForDateTime.TWDateToRCDate(query.SearchYM + "01", "yyyy/MM/dd"), "3");
                }

                string strSQL = string.Format(@"Select NVL(SUM(PAY_HOUR), 0) PAY_HOUR_TOTAL, NVL(SUM(PAY_SUM), 0) PAY_SUM_TOTAL From C_OVERTIME_BASE 
                                                Where MZ_ID = @MZ_ID And OVER_DAY >= @OVER_DAY_S And OVER_DAY < @OVER_DAY_E And IS_SIGN_RETURN='N' And LOCK_FLAG='N' ");
                List<SqlParameter> parameters = new List<SqlParameter>()
                {
                    new SqlParameter("MZ_ID", query.MZ_ID),
                    new SqlParameter("OVER_DAY_S", ForDateTime.RCDateToTWDate(tmpD.ToString("yyyy/MM/dd"), "yyyMMdd")),
                    new SqlParameter("OVER_DAY_E", ForDateTime.RCDateToTWDate(tmpD.AddMonths(1).ToString("yyyy/MM/dd"), "yyyMMdd"))
                };

                DataTable dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, parameters);
                LogModel.saveLog("CF", "S", strSQL, parameters, query.TPMFION, "查詢當月加班費資料。");

                return dt;
            }
            catch (Exception ex)
            {
                LogModel.saveLog("CF", "S", ex.Message, new List<SqlParameter>(), query.TPMFION, "查詢當月加班費資料異常。");
                throw ex;
            }
        }
        /// <summary>
        /// 取得加班時間起訖
        /// </summary>
        /// <param name="model">必要MZ_ID、OVER_DAY</param>
        public void GetOverTimeRange(ref C_OVERTIME_BASE_Model model)
        {
            try
            {
                //檢查ID、日期
                if (string.IsNullOrEmpty(model.MZ_ID) || string.IsNullOrEmpty(model.OVER_DAY))
                {
                    return;
                }

                string stroccc = o_DBFactory.ABC_toTest.vExecSQL("select MZ_OCCC from C_OVERTIME_BASE where MZ_ID = '" + model.MZ_ID + "'");

                string strSQL = @"SELECT LOGDATE, LOGTIME FROM C_CARDHISTORY_NEW 
                                  WHERE USERNAME like (@MZ_ID+'%') AND LOGDATE=RCDATE_FORMAT(@OVER_DAY,'yyyy/MM/dd')
                                        AND LOGDATE is not null AND LOGTIME is not null
                                        AND VERIFY = 'IN'
                                  ORDER BY LOGDATE, LOGTIME ";
                List<SqlParameter> parameters = new List<SqlParameter>()
                {
                    new SqlParameter("MZ_ID", model.MZ_ID),
                    new SqlParameter("OVER_DAY", model.OVER_DAY)
                };
                DataTable dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, parameters);

                if (dt.Rows.Count > 0)
                {
                    //取得當日第一卡時間
                    DataRow firstRow = dt.Rows[0];
                    DateTime firstTime = Convert.ToDateTime(string.Format("{0} {1}", firstRow["LOGDATE"].ToStringNullSafe(), firstRow["LOGTIME"].ToStringNullSafe()));
                    //取得當日最後卡時間
                    DataRow lastRow = dt.Rows[dt.Rows.Count - 1];
                    DateTime lastTime = Convert.ToDateTime(string.Format("{0} {1}", lastRow["LOGDATE"].ToStringNullSafe(), lastRow["LOGTIME"].ToStringNullSafe()));

                    //有兩卡時間才作分析
                    //if (firstTime != lastTime)
                    //{
                    //判斷當日是否為假日(系統定義國定假日及周六日)
                    string holidayCount = o_DBFactory.ABC_toTest.Get_First_Field(string.Format(@"Select COUNT(0) From C_DUTYHOLIDAY Where MZ_HOLIDAY_DATE='{0}'", model.OVER_DAY), null);
                    bool isHoliday = false;
                    if (holidayCount != "0" || firstTime.DayOfWeek == DayOfWeek.Saturday || firstTime.DayOfWeek == DayOfWeek.Sunday)
                    {
                        //為假日
                        isHoliday = true;
                    }

                    string SPRINGDAYstr = o_DBFactory.ABC_toTest.Get_First_Field(string.Format(@"Select COUNT(0) From C_SPRINGDAY Where mz_spring_date='{0}'", model.OVER_DAY), null);
                    if (SPRINGDAYstr != "0" && (firstTime.DayOfWeek == DayOfWeek.Saturday || firstTime.DayOfWeek == DayOfWeek.Sunday))
                    {
                        //補班
                        isHoliday = false;
                    }

                    // sam 北市府可能調時間段從07:00到16:00
                    //建立判斷基準時間段
                    DateTime standardSTime = Convert.ToDateTime(string.Format("{0} {1}", firstRow["LOGDATE"].ToStringNullSafe(), "08:00:00"));
                    DateTime standardSTimeadd = standardSTime.AddHours(1);//已超過正常上班時間(8~17、9~18)，不統計

                    DateTime standardETime = Convert.ToDateTime(string.Format("{0} {1}", lastRow["LOGDATE"].ToStringNullSafe(), "17:00:00"));
                    DateTime OCCCETime = Convert.ToDateTime(string.Format("{0} {1}", lastRow["LOGDATE"].ToStringNullSafe(), "18:00:00"));

                    DateTime LOGDATE = DateTime.Parse(firstRow["LOGDATE"].ToStringNullSafe());
                    if (LOGDATE >= Convert.ToDateTime("2021/08/01") && LOGDATE <= Convert.ToDateTime("2022/12/31"))
                    {
                        standardSTime = Convert.ToDateTime(string.Format("{0} {1}", firstRow["LOGDATE"].ToStringNullSafe(), "07:30:00"));
                        standardSTimeadd = standardSTime.AddHours(2);
                        standardETime = Convert.ToDateTime(string.Format("{0} {1}", lastRow["LOGDATE"].ToStringNullSafe(), "16:30:00"));
                        OCCCETime = Convert.ToDateTime(string.Format("{0} {1}", lastRow["LOGDATE"].ToStringNullSafe(), "18:00:00"));
                    }


                    //不分析刷卡秒數
                    DateTime fTime = firstTime.AddSeconds(-firstTime.Second);
                    DateTime lTime = lastTime.AddSeconds(-lastTime.Second);

                    //如果是其他業務加班單純以兩筆時間相減 , 以輸入的為主 Mark 20210603
                    TimeSpan ts;
                    if (model.OVERTIME_TYPE != "OTT")
                    {

                        if (isHoliday)
                        {
                            //假日計算
                            ts = new TimeSpan(lTime.Ticks - fTime.Ticks);
                            model.OVER_TOTAL = (ts.Hours * 60) + ts.Minutes;
                        }
                        else
                        {
                            //非假日計算              
                            // sam 北市府可能調時間段從07:00到16:00
                            //已超過正常上班時間(8~17、9~18)，不統計
                            if (fTime > standardSTime && fTime > standardSTimeadd)
                            {
                                model.OVER_TOTAL = 0;
                            }
                            //上班時間(8前)
                            else if (fTime < standardSTime)
                            {      //主任、科長、督察加班時間從18:00開始計算
                                   //if (stroccc == "1050" || stroccc == "1078" || stroccc == "5107")
                                   //{
                                   //    ts = new TimeSpan(lTime.Ticks - OCCCETime.Ticks);
                                   //}
                                   //else
                                   //{
                                   //    ts = new TimeSpan(lTime.Ticks - standardETime.Ticks);
                                   //}
                                ts = new TimeSpan(lTime.Ticks - standardETime.Ticks);
                                model.OVER_TOTAL = (ts.Hours * 60) + ts.Minutes;
                            }
                            //上班時間(8整點、後)
                            else
                            {
                                //主任、科長、督察加班時間從18:00開始計算
                                //if (stroccc == "1050" || stroccc == "1078" || stroccc == "5107")
                                //{
                                //    ts = new TimeSpan(lTime.Ticks - OCCCETime.Add(new TimeSpan(fTime.Ticks - standardSTime.Ticks)).Ticks);
                                //}
                                //else
                                //{
                                //    ts = new TimeSpan(lTime.Ticks - standardETime.Add(new TimeSpan(fTime.Ticks - standardSTime.Ticks)).Ticks);
                                //}
                                ts = new TimeSpan(lTime.Ticks - standardETime.Add(new TimeSpan(fTime.Ticks - standardSTime.Ticks)).Ticks);
                                model.OVER_TOTAL = (ts.Hours * 60) + ts.Minutes;
                            }

                        }
                    }
                    else
                    {
                        //小隊長如果值日隔日上班都是計算時 , 以上下班打卡時間直接相減計算 mark
                        ts = new TimeSpan(lTime.Ticks - fTime.Ticks);
                        model.OVER_TOTAL = (ts.Hours * 60) + ts.Minutes;
                    }


                    model.OVER_STIME = firstTime.ToString("HH:mm:ss");
                    model.OVER_ETIME = lastTime.ToString("HH:mm:ss");
                    //加班時數、剩餘總數
                    if (model.OVER_TOTAL < 0) { model.OVER_TOTAL = 0; }

                    //特殊:設定平日上限為240分鐘，假日上限720分鐘
                    if (isHoliday)
                    {
                        //特殊:假日加班最多720分鐘
                        if (model.OVER_TOTAL > 720)
                        {
                            model.OVER_TOTAL = 720;
                        }
                    }
                    else
                    {
                        //特殊:平日加班最多240分鐘
                        if (model.OVER_TOTAL > 240)
                        {
                            model.OVER_TOTAL = 240;
                        }
                    }
                }
                //}
            }
            catch (Exception)
            {

            }
        }
        /// <summary>
        /// 取得加班相關附件資料
        /// (加班異動附件、值日補休附件)
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DataTable GetOverTimeContextFile(C_OverTime_Base_Input_Query query)
        {
            try
            {
                if (query.SN == 0)
                {
                    return null;
                }

                string strSQL = @"Select FILE_URL From C_OVERTIME_CHANGE_FILE Where OVERTIME_SN=@SN 
                                  UNION ALL
                                  Select coof.FILE_URL From C_OVERTIME_BASE cob
                                  Inner join C_OVERTIME_ONDUTY_FILE coof on coof.MZ_AD = cob.MZ_EXAD And coof.MZ_UNIT = cob.MZ_EXUNIT And coof.FILE_DATE = substr(cob.OVER_DAY,0,5)
                                  Where cob.SN=@SN ";
                List<SqlParameter> parameters = new List<SqlParameter>()
                {
                    new SqlParameter("SN", query.SN)
                };

                DataTable dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, parameters);

                return dt;
            }
            catch (Exception ex)
            {
                LogModel.saveLog("CF", "S", ex.Message, new List<SqlParameter>(), query.TPMFION, "查詢加班異動附件資料異常。");
                throw ex;
            }
        }
        /// <summary>
        /// 取得已申請敘獎數量
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public int GetApplyPrizeCount(C_OverTime_PRIZE_Input_Query query)
        {
            try
            {
                if (string.IsNullOrEmpty(query.MZ_ID) || string.IsNullOrEmpty(query.Year) || string.IsNullOrEmpty(query.Year_Interval))
                {
                    return 0;
                }

                string strSQL = @"Select NVL(SUM(PRIZE_AMOUNT), 0) PRIZE_AMOUNT From C_OVERTIME_PRIZE 
                                  Where MZ_ID=@MZ_ID And APPLY_YEAR=@APPLY_YEAR And APPLY_INTERVAL=@APPLY_INTERVAL ";
                List<SqlParameter> parameters = new List<SqlParameter>()
                {
                    new SqlParameter("MZ_ID", query.MZ_ID),
                    new SqlParameter("APPLY_YEAR", query.Year),
                    new SqlParameter("APPLY_INTERVAL", query.Year_Interval)
                };

                int c = Convert.ToInt32(o_DBFactory.ABC_toTest.Get_First_Field(strSQL, parameters));
                LogModel.saveLog("CF", "S", strSQL, parameters, query.TPMFION, "查詢已申請敘獎數量。");

                return c;
            }
            catch (Exception ex)
            {
                LogModel.saveLog("CF", "S", ex.Message, new List<SqlParameter>(), query.TPMFION, "查詢已申請敘獎數量異常。");
                throw ex;
            }
        }
        /// <summary>
        /// 取得人員基本資料
        /// </summary>
        /// <param name="MZ_ID"></param>
        /// <returns></returns>
        public DataTable GetPersonnelData(string MZ_ID, string TPMFION = "")
        {
            try
            {
                if (string.IsNullOrEmpty(MZ_ID))
                {
                    throw new Exception("查詢條件為空，無法查詢。");
                }

                string strSQL = @"Select * From VW_A_DLBASE Where MZ_ID=@MZ_ID ";
                List<SqlParameter> parameters = new List<SqlParameter>()
                {
                    new SqlParameter("MZ_ID", MZ_ID)
                };

                DataTable dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, parameters);

                return dt;
            }
            catch (Exception ex)
            {
                LogModel.saveLog("CF", "S", ex.Message, new List<SqlParameter>(), TPMFION, "查詢人員基本資料異常。");
                return null;
            }
        }

        /// <summary>
        /// 加班基本資料儲存
        /// </summary>
        /// <param name="models"></param>
        /// <param name="TPMFION"></param>
        /// <returns></returns>
        public bool C_OVERTIME_BASE_Save(List<C_OVERTIME_BASE_Model> models, string TPMFION = "")
        {
            try
            {
                var otf=o_DBFactory.ABC_toTest;
                string strSQL = string.Empty;
                List<SqlParameter> parameters;

                foreach (var item in models)
                {
                    if (string.IsNullOrEmpty(item.MZ_ID) || string.IsNullOrEmpty(item.OVER_DAY))
                    {
                        throw new Exception(string.Format("ID:{0}，加班基本資料儲存主Key異常。", item.MZ_ID));
                    }

                    //檢查是否為新增 or 修改
                    strSQL = string.Format(@"Select count(*) from C_OVERTIME_BASE Where MZ_ID='{0}' And OVER_DAY='{1}' ",
                        item.MZ_ID, item.OVER_DAY);

                    if (o_DBFactory.ABC_toTest.vExecSQL(strSQL) != "0")
                    {
                        //修改
                        strSQL = @"Update C_OVERTIME_BASE Set {0}
                                    Where MZ_ID=@MZ_ID And OVER_DAY=@OVER_DAY And OVERTIME_TYPE=@OVERTIME_TYPE ";
                        string strtype = "";
                        if (item.OVERTIME_TYPE == null)
                        {
                            string strsql = "select OVERTIME_TYPE from C_OVERTIME_BASE where MZ_ID = '" + item.MZ_ID + "' and OVER_DAY = '" + item.OVER_DAY + "'";
                            strtype = o_DBFactory.ABC_toTest.vExecSQL(strsql);
                        }
                        else
                        {
                            strtype = item.OVERTIME_TYPE;
                        }
                        parameters = new List<SqlParameter>()
                        {
                            new SqlParameter("MZ_ID", item.MZ_ID),
                            new SqlParameter("OVER_DAY", item.OVER_DAY),
                            new SqlParameter("OVERTIME_TYPE", strtype)
                        };
                        #region 需更新欄位
                        List<string> upColumn = new List<string>();
                        if (!string.IsNullOrEmpty(item.MZ_OCCC))
                        {
                            upColumn.Add("MZ_OCCC=@MZ_OCCC ");
                            parameters.Add(new SqlParameter("MZ_OCCC", item.MZ_OCCC));
                        }
                        if (!string.IsNullOrEmpty(item.REVIEW_ID))
                        {
                            upColumn.Add("REVIEW_ID=@REVIEW_ID ");
                            parameters.Add(new SqlParameter("REVIEW_ID", item.REVIEW_ID));
                        }
                        if (!string.IsNullOrEmpty(item.REASON))
                        {
                            upColumn.Add("REASON=@REASON ");
                            parameters.Add(new SqlParameter("REASON", item.REASON));
                        }
                        if (!string.IsNullOrEmpty(item.OVER_STIME))
                        {
                            upColumn.Add("OVER_STIME=@OVER_STIME ");
                            parameters.Add(new SqlParameter("OVER_STIME", item.OVER_STIME));
                        }
                        if (!string.IsNullOrEmpty(item.OVER_ETIME))
                        {
                            upColumn.Add("OVER_ETIME=@OVER_ETIME ");
                            parameters.Add(new SqlParameter("OVER_ETIME", item.OVER_ETIME));
                        }
                        if (item.OVER_TOTAL.HasValue)
                        {
                            upColumn.Add("OVER_TOTAL=@OVER_TOTAL ");
                            parameters.Add(new SqlParameter("OVER_TOTAL", item.OVER_TOTAL.Value));
                        }
                        if (!string.IsNullOrEmpty(item.OVER_REMARK))
                        {
                            upColumn.Add("OVER_REMARK=@OVER_REMARK ");
                            parameters.Add(new SqlParameter("OVER_REMARK", item.OVER_REMARK));
                        }
                        if (!string.IsNullOrEmpty(item.OVERTIME_CHG))
                        {
                            upColumn.Add("OVERTIME_CHG=@OVERTIME_CHG ");
                            parameters.Add(new SqlParameter("OVERTIME_CHG", item.OVERTIME_CHG));
                        }
                        if (!string.IsNullOrEmpty(item.OVERTIME_CHG_ID))
                        {
                            upColumn.Add("OVERTIME_CHG_ID=@OVERTIME_CHG_ID ");
                            parameters.Add(new SqlParameter("OVERTIME_CHG_ID", item.OVERTIME_CHG_ID));
                        }
                        if (item.SURPLUS_TOTAL.HasValue)
                        {
                            upColumn.Add("SURPLUS_TOTAL=@SURPLUS_TOTAL ");
                            parameters.Add(new SqlParameter("SURPLUS_TOTAL", item.SURPLUS_TOTAL.Value));
                        }
                        if (item.PAY_HOUR.HasValue)
                        {
                            upColumn.Add("PAY_HOUR=@PAY_HOUR ");
                            parameters.Add(new SqlParameter("PAY_HOUR", item.PAY_HOUR.Value));
                        }
                        if (item.SHIFT_HOUR.HasValue)
                        {
                            upColumn.Add("SHIFT_HOUR=@SHIFT_HOUR ");
                            parameters.Add(new SqlParameter("SHIFT_HOUR", item.SHIFT_HOUR.Value));
                        }
                        if (item.PAY_UNIT.HasValue)
                        {
                            upColumn.Add("PAY_UNIT=@PAY_UNIT ");
                            parameters.Add(new SqlParameter("PAY_UNIT", item.PAY_UNIT.Value));
                        }
                        if (item.PAY_SUM.HasValue)
                        {
                            upColumn.Add("PAY_SUM=@PAY_SUM ");
                            parameters.Add(new SqlParameter("PAY_SUM", item.PAY_SUM.Value));
                        }
                        if (!string.IsNullOrEmpty(item.PAY_CHK))
                        {
                            upColumn.Add("PAY_CHK=@PAY_CHK ");
                            parameters.Add(new SqlParameter("PAY_CHK", item.PAY_CHK));
                        }
                        if (!string.IsNullOrEmpty(item.PAY_CHK_ID))
                        {
                            upColumn.Add("PAY_CHK_ID=@PAY_CHK_ID ");
                            parameters.Add(new SqlParameter("PAY_CHK_ID", item.PAY_CHK_ID));
                        }
                        if (item.PAY_CHK_DATE.HasValue)
                        {
                            upColumn.Add("PAY_CHK_DATE=@PAY_CHK_DATE ");
                            parameters.Add(new SqlParameter("PAY_CHK_DATE", item.PAY_CHK_DATE.Value));
                        }
                        if (item.REST_HOUR.HasValue)
                        {
                            upColumn.Add("REST_HOUR=@REST_HOUR ");
                            parameters.Add(new SqlParameter("REST_HOUR", item.REST_HOUR.Value));
                        }
                        if (!string.IsNullOrEmpty(item.REST_ID))
                        {
                            upColumn.Add("REST_ID=@REST_ID ");
                            parameters.Add(new SqlParameter("REST_ID", item.REST_ID));
                        }
                        if (item.REST_DATE.HasValue)
                        {
                            upColumn.Add("REST_DATE=@REST_DATE ");
                            parameters.Add(new SqlParameter("REST_DATE", item.REST_DATE.Value));
                        }
                        if (item.PRIZE_HOUR.HasValue)
                        {
                            upColumn.Add("PRIZE_HOUR=@PRIZE_HOUR ");
                            parameters.Add(new SqlParameter("PRIZE_HOUR", item.PRIZE_HOUR.Value));
                        }
                        if (!string.IsNullOrEmpty(item.PRIZE_ID))
                        {
                            upColumn.Add("PRIZE_ID=@PRIZE_ID ");
                            parameters.Add(new SqlParameter("PRIZE_ID", item.PRIZE_ID));
                        }
                        if (item.PRIZE_DATE.HasValue)
                        {
                            upColumn.Add("PRIZE_DATE=@PRIZE_DATE ");
                            parameters.Add(new SqlParameter("PRIZE_DATE", item.PRIZE_DATE.Value));
                        }
                        if (!string.IsNullOrEmpty(item.LOCK_FLAG))
                        {
                            upColumn.Add("LOCK_FLAG=@LOCK_FLAG ");
                            parameters.Add(new SqlParameter("LOCK_FLAG", item.LOCK_FLAG));
                        }
                        if (item.OVERTIME_CHG_TOTAL.HasValue)
                        {
                            upColumn.Add("OVERTIME_CHG_TOTAL=@OVERTIME_CHG_TOTAL ");
                            parameters.Add(new SqlParameter("OVERTIME_CHG_TOTAL", item.OVERTIME_CHG_TOTAL.Value));
                        }
                        if (!string.IsNullOrEmpty(item.IS_FREESIGN))
                        {
                            upColumn.Add("IS_FREESIGN=@IS_FREESIGN ");
                            parameters.Add(new SqlParameter("IS_FREESIGN", item.IS_FREESIGN));
                        }
                        if (!string.IsNullOrEmpty(item.IS_SIGN_RETURN))
                        {
                            upColumn.Add("IS_SIGN_RETURN=@IS_SIGN_RETURN ");
                            parameters.Add(new SqlParameter("IS_SIGN_RETURN", item.IS_SIGN_RETURN));
                        }
                        #endregion
                        //if (item.ACC_OTHERHOUR.HasValue)
                        //{
                        //    upColumn.Add("ACC_OTHERHOUR=@ACC_OTHERHOUR ");
                        //    parameters.Add(new SqlParameter("ACC_OTHERHOUR", item.ACC_OTHERHOUR.Value));
                        //}
                        //if (!string.IsNullOrEmpty(item.ACC_OTHERDAY))
                        //{
                        //    upColumn.Add("ACC_OTHERDAY=@ACC_OTHERDAY ");
                        //    parameters.Add(new SqlParameter("ACC_OTHERDAY", item.ACC_OTHERDAY));
                        //}
                        //if (!string.IsNullOrEmpty(item.ACC_TYPE))
                        //{
                        //    upColumn.Add("ACC_TYPE=@ACC_TYPE ");
                        //    parameters.Add(new SqlParameter("ACC_TYPE", item.ACC_TYPE));
                        //}
                        if (!string.IsNullOrEmpty(item.OPTYPE))
                        {
                            upColumn.Add("OPTYPE=@OPTYPE ");
                            parameters.Add(new SqlParameter("OPTYPE", item.OPTYPE));
                        }

                        if (!string.IsNullOrEmpty(item.INS_ID))
                        {
                            upColumn.Add("INS_ID=@INS_ID ");
                            parameters.Add(new SqlParameter("INS_ID", item.INS_ID));
                        }

                        if (!string.IsNullOrEmpty(item.INS_DATE))
                        {
                            upColumn.Add("INS_DATE=@INS_DATE ");
                            parameters.Add(new SqlParameter("INS_DATE", item.INS_DATE));
                        }

                        strSQL = string.Format(strSQL, string.Join(",", upColumn.ToArray()));
                    }
                    else
                    {
                        //新增
                        strSQL = @"INSERT INTO C_OVERTIME_BASE (MZ_ID, OVER_DAY, MZ_EXAD, MZ_EXUNIT, MZ_OCCC, SN, REASON, PAY_UNIT, LOCK_FLAG, OVERTIME_TYPE, IS_FREESIGN, OVER_TOTAL,OVERTIME_CHG_TOTAL,MZ_AD,MZ_UNIT,INS_ID,INS_DATE,OPTYPE) 
                                    VALUES 
                                    (@MZ_ID, @OVER_DAY, @MZ_EXAD, @MZ_EXUNIT, @MZ_OCCC,  NEXT VALUE FOR dbo.C_OVERTIME_BASE_SN, @REASON, @PAY_UNIT, @LOCK_FLAG, @OVERTIME_TYPE, @IS_FREESIGN, @OVER_TOTAL,@OVERTIME_CHG_TOTAL,@MZ_AD,@MZ_UNIT,@INS_ID,@INS_DATE,@OPTYPE) ";

                        parameters = new List<SqlParameter>()
                        {
                            new SqlParameter("MZ_ID", item.MZ_ID),
                            new SqlParameter("OVER_DAY", item.OVER_DAY),
                            new SqlParameter("MZ_EXAD", item.MZ_EXAD),
                            new SqlParameter("MZ_EXUNIT", item.MZ_EXUNIT),
                            new SqlParameter("MZ_OCCC", item.MZ_OCCC),
                            new SqlParameter("REASON", item.REASON),
                            new SqlParameter("PAY_UNIT", item.PAY_UNIT.HasValue ? item.PAY_UNIT.Value : 0),
                            new SqlParameter("LOCK_FLAG", item.LOCK_FLAG),
                            new SqlParameter("OVERTIME_TYPE", item.OVERTIME_TYPE),
                            new SqlParameter("IS_FREESIGN", item.IS_FREESIGN),
                            new SqlParameter("OVER_TOTAL", item.OVER_TOTAL.HasValue ? item.OVER_TOTAL.Value : 0),
                            new SqlParameter("OVERTIME_CHG_TOTAL", item.OVERTIME_CHG_TOTAL.HasValue ? item.OVERTIME_CHG_TOTAL.Value : 0),
                            new SqlParameter("MZ_AD", item.MZ_AD),
                            new SqlParameter("MZ_UNIT", item.MZ_UNIT),
                            new SqlParameter("INS_ID", item.INS_ID),
                            new SqlParameter("INS_DATE", item.INS_DATE),
                            new SqlParameter("OPTYPE", item.OPTYPE)

                        };
                    }

                    otf.MultiSql_Add(strSQL, parameters.ToArray());
                    LogModel.saveLog("CF", strSQL.IndexOf("Update") > 0 ? "U" : "A", strSQL, parameters, TPMFION, "加班基本資料儲存。");
                }

                otf.MultiSQL_Exec();

                return true;
            }
            catch (Exception ex)
            {
                LogModel.saveLog("CF", "UA", ex.Message, new List<SqlParameter>(), TPMFION, "加班基本資料儲存異常。");
                throw ex;
            }
        }
        /// <summary>
        /// 加班基本資料儲存
        /// </summary>
        /// <param name="model"></param>
        /// <param name="TPMFION"></param>
        /// <returns></returns>
        public bool C_OVERTIME_BASE_Save(C_OVERTIME_BASE_Model model, string TPMFION = "")
        {
            return C_OVERTIME_BASE_Save(new List<C_OVERTIME_BASE_Model>() { model }, TPMFION);
        }
        /// <summary>
        /// 重新同步(計算)加班時數,會更新C_OVERTIME_BASE的SURPLUS_TOTAL
        /// </summary>
        /// <param name="model"></param>
        /// <param name="TPMFION"></param>
        /// <returns></returns>
        public bool SynchronizeOverTime(C_OVERTIME_BASE_Model model, string TPMFION = "")
        {
            try
            {
                bool result = true;
                if ((string.IsNullOrEmpty(model.MZ_ID) || string.IsNullOrEmpty(model.OVER_DAY)) && model.SN == 0)
                {
                    throw new Exception("必要欄位遺失，無法同步加班時數。");
                }

                string strSQL = string.Format(@"UPDATE C_OVERTIME_BASE Set SURPLUS_TOTAL = 
                                                CASE OVERTIME_CHG WHEN 'Y' THEN NVL(OVERTIME_CHG_TOTAL-PAY_HOUR-SHIFT_HOUR-REST_HOUR-PRIZE_HOUR, 0) 
                                                                  WHEN 'N' THEN NVL(OVER_TOTAL-PAY_HOUR-SHIFT_HOUR-REST_HOUR-PRIZE_HOUR, 0) END 
                                                WHERE LOCK_FLAG='N' And IS_SIGN_RETURN='N' {0} ",
                                                model.SN == 0 ? "AND MZ_ID=@MZ_ID AND OVER_DAY=@OVER_DAY" : "AND SN=@SN");
                List<SqlParameter> parameters = new List<SqlParameter>();

                if (model.SN == 0)
                {
                    parameters.Add(new SqlParameter("MZ_ID", model.MZ_ID));
                    parameters.Add(new SqlParameter("OVER_DAY", model.OVER_DAY));
                }
                else
                {
                    parameters.Add(new SqlParameter("SN", model.SN));
                }

                if (!o_DBFactory.ABC_toTest.DealCommandLog(strSQL, parameters))
                {
                    result = false;
                }

                LogModel.saveLog("CF", "U", strSQL, parameters, TPMFION, "同步(計算)加班時數。");
                return result;
            }
            catch (Exception ex)
            {
                LogModel.saveLog("CF", "U", ex.Message, new List<SqlParameter>(), TPMFION, "同步(計算)加班時數異常。");
                return false;
            }
        }
        /// <summary>
        /// 加班基本資料及關連資料刪除
        /// </summary>
        /// <param name="model"></param>
        /// <param name="TPMFION"></param>
        /// <returns>空白=完成刪除</returns>
        public string C_OVERTIME_BASE_ALL_Delete(C_OVERTIME_BASE_Model model, string TPMFION = "")
        {
            try
            {
                string strSQL = string.Empty;
                List<SqlParameter> parameters;

                if (string.IsNullOrEmpty(model.MZ_ID) || string.IsNullOrEmpty(model.OVER_DAY) || model.SN == 0)
                {
                    throw new Exception(string.Format("ID:{0}，執行刪除的條件主Key異常。", model.MZ_ID));
                }

                strSQL = "SELECT COUNT(0) FROM C_OVERTIME_BASE WHERE MZ_ID=@MZ_ID And OVER_DAY=@OVER_DAY And SN=@SN ";
                parameters = new List<SqlParameter>()
                {
                    new SqlParameter("MZ_ID", model.MZ_ID),
                    new SqlParameter("OVER_DAY", model.OVER_DAY),
                    new SqlParameter("SN", model.SN)
                };

                //檢查資料
                if (o_DBFactory.ABC_toTest.Get_First_Field(strSQL, parameters) != "0")
                {
                    #region 檢查是否可刪除、執行刪除操作
                    //檢核是否已申請補休
                    strSQL = @"Select COUNT(0) COUNT_DLTB01 from C_OVERTIME_BASE cob
                            Inner join C_OVERTIME_FOR_DLTB01 cofd on cofd.OVERTIME_SN = cob.SN
                            Where cob.MZ_ID=@MZ_ID And cob.OVER_DAY=@OVER_DAY And cob.SN=@SN ";
                    if (o_DBFactory.ABC_toTest.Get_First_Field(strSQL, parameters) != "0")
                    {
                        return "已使用補休，無法刪除加班資料。";
                    }
                    //檢查是否已申請加班費
                    strSQL = @"Select PAY_HOUR from C_OVERTIME_BASE Where MZ_ID=@MZ_ID And OVER_DAY=@OVER_DAY And SN=@SN ";
                    if (o_DBFactory.ABC_toTest.Get_First_Field(strSQL, parameters) != "0")
                    {
                        return "已申請加班費，無法刪除加班資料。";
                    }
                    //檢查是否已申請敘獎
                    strSQL = @"Select PRIZE_HOUR from C_OVERTIME_BASE Where MZ_ID=@MZ_ID And OVER_DAY=@OVER_DAY And SN=@SN ";
                    if (o_DBFactory.ABC_toTest.Get_First_Field(strSQL, parameters) != "0")
                    {
                        return "已申請敘獎，無法刪除加班資料。";
                    }

                    //20210805 改成 任何狀態都能刪除資料 
                    //送簽中無法刪除
                    //strSQL = string.Format(@"SELECT COUNT(0) FROM C_OVERTIME_HISTORY WHERE OVERTIME_SN='{0}' And PROCESS_DATE is null", model.SN);
                    //if (o_DBFactory.ABC_toTest.Get_First_Field(strSQL, null) != "0")
                    //{
                    //    return "正在線上陳核中，無法刪除加班資料。";
                    //}

                    //檢查加班費審核 是否已審核
                    strSQL = @"Select PAY_CHK  from C_OVERTIME_BASE Where MZ_ID=@MZ_ID And OVER_DAY=@OVER_DAY And SN=@SN ";
                    if (o_DBFactory.ABC_toTest.Get_First_Field(strSQL, parameters) == "Y")
                    {
                        return "加班費已審核審核，無法刪除加班資料。";
                    }

                    //開始刪除資料
                    C_OVERTIME_HISTORY_Model OWModel = new C_OVERTIME_HISTORY_Model() { OVERTIME_SN = model.SN };
                    if (!new OWService().C_OVERTIME_HISTORY_Delete(OWModel, TPMFION))
                    {
                        throw new Exception("加班基本資料刪除操作，刪除簽核關聯資料失敗。");
                    }
                    C_OVERTIME_CHANGE_FILE_Model cofModel = new C_OVERTIME_CHANGE_FILE_Model() { OVERTIME_SN = model.SN };
                    if (!C_OVERTIME_CHANGE_FILE_Delete(cofModel, TPMFION))
                    {
                        throw new Exception("加班基本資料刪除操作，刪除附件資料失敗。");
                    }

                    //刪除加班基本資料
                    strSQL = @"DELETE FROM C_OVERTIME_BASE Where MZ_ID=@MZ_ID And OVER_DAY=@OVER_DAY And SN=@SN ";
                    if (!o_DBFactory.ABC_toTest.DealCommandLog(strSQL, parameters))
                    {
                        LogModel.saveLog("COHI", "D", strSQL, parameters.ToArray(), TPMFION, "加班基本資料刪除失敗。");
                        return "刪除加班基本資料失敗。";
                    }
                    else
                    {
                        LogModel.saveLog("COHI", "D", strSQL, parameters, TPMFION, "加班基本資料刪除。");
                    }
                    #endregion
                }
                else
                {
                    return "查無加班資料，無法刪除。";
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                LogModel.saveLog("CF", "D", ex.Message, new List<SqlParameter>(), TPMFION, "加班基本資料刪除異常。");
                throw ex;
            }
        }
        /// <summary>
        /// 加班異動審核附件儲存
        /// </summary>
        /// <param name="model"></param>
        /// <param name="TPMFION"></param>
        /// <returns></returns>
        public bool C_OVERTIME_CHANGE_FILE_Save(List<C_OVERTIME_CHANGE_FILE_Model> models, string TPMFION = "")
        {
            try
            {
                var otf=o_DBFactory.ABC_toTest;
                string strSQL = string.Empty;
                List<SqlParameter> parameters;

                foreach (var item in models)
                {
                    if (item.OVERTIME_SN == 0)
                    {
                        throw new Exception(string.Format("OVERTIME_SN:{0}，加班異動審核附件儲存主Key異常。", item.OVERTIME_SN));
                    }

                    //僅新增資料
                    strSQL = @"INSERT INTO C_OVERTIME_CHANGE_FILE(OVERTIME_SN, SN, FILE_URL, ADD_ID, ADD_DATE)
                                VALUES(@OVERTIME_SN,  NEXT VALUE FOR dbo.C_OVERTIME_FILE_SN, @FILE_URL, @ADD_ID, GETDATE()) ";
                    parameters = new List<SqlParameter>()
                    {
                        new SqlParameter("OVERTIME_SN", item.OVERTIME_SN),
                        new SqlParameter("FILE_URL", item.FILE_URL),
                        new SqlParameter("ADD_ID", item.ADD_ID)
                    };

                    otf.MultiSql_Add(strSQL, parameters.ToArray());
                    LogModel.saveLog("CF", strSQL.IndexOf("Update") > 0 ? "U" : "A", strSQL, parameters, TPMFION, "加班異動審核附件儲存。");
                }

                otf.MultiSQL_Exec();

                return true;
            }
            catch (Exception ex)
            {
                LogModel.saveLog("CF", "UA", ex.Message, new List<SqlParameter>(), TPMFION, "加班異動審核附件儲存異常。");
                throw ex;
            }
        }
        /// <summary>
        /// 加班異動審核附件刪除
        /// </summary>
        /// <param name="model"></param>
        /// <param name="TPMFION"></param>
        /// <returns></returns>
        public bool C_OVERTIME_CHANGE_FILE_Delete(C_OVERTIME_CHANGE_FILE_Model model, string TPMFION = "")
        {
            try
            {
                string strSQL = string.Empty;
                List<SqlParameter> parameters;

                if (model.OVERTIME_SN == 0)
                {
                    return false;
                }

                //取回檔名
                strSQL = @"Select FILE_URL From C_OVERTIME_CHANGE_FILE Where OVERTIME_SN=@OSN ";
                parameters = new List<SqlParameter>() { new SqlParameter("OSN", model.OVERTIME_SN) };

                DataTable coFiles = o_DBFactory.ABC_toTest.Create_Table(strSQL, parameters);
                foreach (DataRow item in coFiles.Rows)
                {
                    string filePath = WebConfigHelpers.C_UPLOAD_FILEPATH_TO_SERVER_PATH + item["FILE_URL"].ToStringNullSafe();
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                strSQL = @"DELETE FROM C_OVERTIME_CHANGE_FILE Where OVERTIME_SN=@OSN ";
                if (o_DBFactory.ABC_toTest.DealCommandLog(strSQL, parameters))
                {
                    LogModel.saveLog("CF", "D", strSQL, parameters, TPMFION, "加班異動審核附件刪除。");
                }
                else
                {
                    LogModel.saveLog("CF", "D", strSQL, parameters, TPMFION, "加班異動審核附件刪除失敗。");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                LogModel.saveLog("CF", "D", ex.Message, new List<SqlParameter>(), TPMFION, "加班異動審核附件刪除異常。");
                throw ex;
            }
        }
        /// <summary>
        /// 加班敘獎資料儲存
        /// </summary>
        /// <param name="models"></param>
        /// <param name="TPMFION"></param>
        /// <returns></returns>
        public bool C_OVERTIME_PRIZE_Save(List<C_OVERTIME_PRIZE_Model> models, string TPMFION = "")
        {
            try
            {
                var otf=o_DBFactory.ABC_toTest;
                string strSQL = string.Empty;
                List<SqlParameter> parameters;

                foreach (var item in models)
                {
                    if (string.IsNullOrEmpty(item.MZ_ID) || string.IsNullOrEmpty(item.APPLY_YEAR) || string.IsNullOrEmpty(item.APPLY_INTERVAL))
                    {
                        throw new Exception(string.Format("ID:{0} APPLY_YEAR:{1} APPLY_INTERVAL:{2}，加班敘獎資料儲存主Key異常。", item.MZ_ID, item.APPLY_YEAR, item.APPLY_INTERVAL));
                    }

                    //新增敘獎資料
                    strSQL = @"INSERT INTO C_OVERTIME_PRIZE (MZ_ID, APPLY_YEAR, APPLY_INTERVAL, PRIZE_AMOUNT)
                                Values(@MZ_ID, @APPLY_YEAR, @APPLY_INTERVAL, @PRIZE_AMOUNT) ";
                    parameters = new List<SqlParameter>()
                    {
                        new SqlParameter("MZ_ID", item.MZ_ID),
                        new SqlParameter("APPLY_YEAR", item.APPLY_YEAR),
                        new SqlParameter("APPLY_INTERVAL", item.APPLY_INTERVAL),
                        new SqlParameter("PRIZE_AMOUNT", item.PRIZE_AMOUNT)
                    };

                    otf.MultiSql_Add(strSQL, parameters.ToArray());
                    LogModel.saveLog("CF", strSQL.IndexOf("Update") > 0 ? "U" : "A", strSQL, parameters, TPMFION, "加班敘獎資料儲存。");

                    #region 更新加班剩餘時數
                    //嘉獎所需時數(分)
                    //int prizeHour = item.PRIZE_AMOUNT * Convert.ToInt32(GetOVERTIMECODE("CODE_VALUE", "OTPH", codeDesc: "嘉獎").Rows[0][0]) * 60;
                    int min_ToPrize = item.PRIZE_AMOUNT * 60;
                    //取得各月份可扣剩餘時數
                    C_OverTime_PRIZE_Input_Query copQuery = new C_OverTime_PRIZE_Input_Query() { MZ_ID = item.MZ_ID, Year = item.APPLY_YEAR, Year_Interval = item.APPLY_INTERVAL };
                    DataTable dt = GetOverTimeBaseByPrize(copQuery);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        //當月可申請時數
                        int min_CanBePrize = Convert.ToInt32(dt.Rows[i]["SURPLUS_HOUR"]) * 60;
                        if (min_CanBePrize == 0) { continue; }
                        string monthS = dt.Rows[i]["OVER_MONTH"].ToStringNullSafe() + "01";
                        string monthE = dt.Rows[i]["OVER_MONTH"].ToStringNullSafe() + "31";
                        //取得當月所有加班資料
                        strSQL = string.Format(@"Select OVER_DAY, SURPLUS_TOTAL From C_OVERTIME_BASE Where OVERTIME_type in ('OTB','OTT','OTD') and MZ_ID = '{0}' And OVER_DAY >= '{1}' And OVER_DAY <= '{2}'",
                            item.MZ_ID, monthS, monthE);
                        DataTable cDt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "C_OVERTIME_BASE");
                        for (int y = 0; y < cDt.Rows.Count; y++)
                        {
                            //當日未被申請的加班時數(實際上是分鐘數)
                            int SURPLUS_TOTAL = Convert.ToInt32(cDt.Rows[y]["SURPLUS_TOTAL"]);
                            strSQL = @"UPDATE C_OVERTIME_BASE Set SURPLUS_TOTAL=@SURPLUS_TOTAL, PRIZE_HOUR=@PRIZE_HOUR, PRIZE_ID=@MZ_ID, PRIZE_DATE=GETDATE() 
                                        Where OVERTIME_type in ('OTB','OTT','OTD') and MZ_ID=@MZ_ID And OVER_DAY=@OVER_DAY ";
                            parameters = new List<SqlParameter>() { new SqlParameter("MZ_ID", item.MZ_ID), new SqlParameter("OVER_DAY", cDt.Rows[y]["OVER_DAY"].ToStringNullSafe()) };
                            //申請時數小於當月可申請時數
                            if (min_ToPrize < min_CanBePrize)
                            {
                                if (min_ToPrize < SURPLUS_TOTAL)
                                { //申請時數小於當日剩餘時數
                                    parameters.Add(new SqlParameter("SURPLUS_TOTAL", SURPLUS_TOTAL - min_ToPrize));
                                    parameters.Add(new SqlParameter("PRIZE_HOUR", min_ToPrize));
                                    min_ToPrize = 0;
                                }
                                else
                                { //申請時數大等於當日剩餘時數
                                    parameters.Add(new SqlParameter("SURPLUS_TOTAL", value: 0));
                                    parameters.Add(new SqlParameter("PRIZE_HOUR", SURPLUS_TOTAL));
                                    min_ToPrize = min_ToPrize - SURPLUS_TOTAL;
                                    min_CanBePrize = min_CanBePrize - SURPLUS_TOTAL;
                                }
                            }
                            else
                            { //申請時數大等於當月可申請時數

                                //若 當月可申請時數(轉換成分鐘數) 小於 當日剩餘加班時數(轉換成分鐘數)
                                if (min_CanBePrize < SURPLUS_TOTAL)
                                {
                                    //當日剩餘加班時數(轉換成分鐘數) 減去 當月可申請時數(轉換成分鐘數)
                                    parameters.Add(new SqlParameter("SURPLUS_TOTAL", SURPLUS_TOTAL - min_CanBePrize));
                                    //剩下的分鐘數拿去申請 
                                    parameters.Add(new SqlParameter("PRIZE_HOUR", min_CanBePrize));
                                    min_ToPrize = min_ToPrize - min_CanBePrize;
                                    min_CanBePrize = 0;
                                }
                                else
                                { //若 當月可申請時數(轉換成分鐘數) 大等於 當日剩餘加班時數(轉換成分鐘數)

                                    //當日剩餘加班時數(轉換成分鐘數) 為0 就是用光了
                                    parameters.Add(new SqlParameter("SURPLUS_TOTAL", value: 0));
                                    //當日剩餘加班時數(轉換成分鐘數) 都拿去申請 
                                    parameters.Add(new SqlParameter("PRIZE_HOUR", SURPLUS_TOTAL));
                                    //扣除掉已經申請的,繼續下一回合
                                    min_ToPrize = min_ToPrize - SURPLUS_TOTAL;
                                    min_CanBePrize = min_CanBePrize - SURPLUS_TOTAL;
                                }
                            }
                            otf.MultiSql_Add(strSQL, parameters.ToArray());
                            if (min_ToPrize == 0 || min_CanBePrize == 0) { break; }
                        }
                        if (min_ToPrize == 0) { break; }
                    }
                    LogModel.saveLog("CF", "U", strSQL, parameters, TPMFION, "加班敘獎-更新加班剩餘時數。");
                    #endregion
                }

                otf.MultiSQL_Exec();

                return true;
            }
            catch (Exception ex)
            {
                LogModel.saveLog("CF", "UA", ex.Message, new List<SqlParameter>(), TPMFION, "加班敘獎資料儲存異常。");
                throw ex;
            }
        }
        /// <summary>
        /// 加班敘獎資料刪除
        /// </summary>
        /// <param name="models"></param>
        /// <param name="TPMFION"></param>
        /// <returns></returns>
        public bool C_OVERTIME_PRIZE_Delete(List<C_OVERTIME_PRIZE_Model> models, string TPMFION = "")
        {
            try
            {
                var otf=o_DBFactory.ABC_toTest;
                string strSQL = string.Empty;
                List<SqlParameter> parameters;

                foreach (var item in models)
                {
                    if (string.IsNullOrEmpty(item.MZ_ID) || string.IsNullOrEmpty(item.APPLY_YEAR) || string.IsNullOrEmpty(item.APPLY_INTERVAL))
                    {
                        throw new Exception(string.Format("ID:{0} APPLY_YEAR:{1} APPLY_INTERVAL:{2}，加班敘獎資料刪除主Key異常。", item.MZ_ID, item.APPLY_YEAR, item.APPLY_INTERVAL));
                    }

                    strSQL = @"DELETE FROM C_OVERTIME_PRIZE Where MZ_ID=@MZ_ID And APPLY_YEAR=@APPLY_YEAR And APPLY_INTERVAL=@APPLY_INTERVAL ";
                    parameters = new List<SqlParameter>()
                    {
                        new SqlParameter("MZ_ID", item.MZ_ID),
                        new SqlParameter("APPLY_YEAR", item.APPLY_YEAR),
                        new SqlParameter("APPLY_INTERVAL", item.APPLY_INTERVAL)
                    };

                    otf.MultiSql_Add(strSQL, parameters.ToArray());
                    LogModel.saveLog("CF", "D", strSQL, parameters, TPMFION, "加班敘獎資料刪除。");

                    #region 更新加班剩餘時數
                    string halfYearS = string.Empty;
                    string halfYearE = string.Empty;
                    switch (item.APPLY_INTERVAL)
                    {
                        case "0": //上半年
                            halfYearS = item.APPLY_YEAR + "0101";
                            halfYearE = item.APPLY_YEAR + "0630";
                            break;
                        case "1": //下半年
                            halfYearS = item.APPLY_YEAR + "0701";
                            halfYearE = item.APPLY_YEAR + "1231";
                            break;
                    }
                    strSQL = @"UPDATE C_OVERTIME_BASE Set SURPLUS_TOTAL=(SURPLUS_TOTAL + PRIZE_HOUR), PRIZE_HOUR=@prizehour, PRIZE_ID=@prizeid, PRIZE_DATE=GETDATE() 
                                Where MZ_ID=@MZ_ID And OVER_DAY>=@halfYearS And OVER_DAY<=@halfYearE ";
                    parameters = new List<SqlParameter>()
                    {
                        new SqlParameter("MZ_ID", item.MZ_ID),
                        new SqlParameter("halfYearS", halfYearS),
                        new SqlParameter("halfYearE", halfYearE),
                        new SqlParameter("prizehour", value: 0),
                        new SqlParameter("prizeid", DBNull.Value)
                    };
                    otf.MultiSql_Add(strSQL, parameters.ToArray());
                    LogModel.saveLog("CF", "U", strSQL, parameters, TPMFION, "加班敘獎資料刪除。");
                    #endregion
                }

                otf.MultiSQL_Exec();

                return true;
            }
            catch (Exception ex)
            {
                LogModel.saveLog("CF", "D", ex.Message, new List<SqlParameter>(), TPMFION, "加班敘獎資料刪除異常。");
                throw ex;
            }
        }
        /// <summary>
        /// 值日補休附件儲存
        /// </summary>
        /// <param name="models"></param>
        /// <param name="TPMFION"></param>
        /// <returns></returns>
        public bool C_OVERTIME_ONDUTY_FILE_Save(List<C_OVERTIME_ONDUTY_FILE_Model> models, string TPMFION = "")
        {
            try
            {
                var otf=o_DBFactory.ABC_toTest;
                string strSQL = string.Empty;
                List<SqlParameter> parameters;

                foreach (var item in models)
                {
                    if (string.IsNullOrEmpty(item.MZ_AD) || string.IsNullOrEmpty(item.MZ_UNIT) || string.IsNullOrEmpty(item.FILE_DATE))
                    {
                        throw new Exception("值日補休附件儲存主Key異常。");
                    }

                    //確認重複資料決定新增或更新
                    strSQL = @"Select Count(*) From C_OVERTIME_ONDUTY_FILE Where MZ_AD=@MZ_AD And MZ_UNIT=@MZ_UNIT And FILE_DATE=@FILE_DATE ";
                    parameters = new List<SqlParameter>()
                    {
                        new SqlParameter("MZ_AD", item.MZ_AD),
                        new SqlParameter("MZ_UNIT", item.MZ_UNIT),
                        new SqlParameter("FILE_DATE", item.FILE_DATE)
                    };

                    if (o_DBFactory.ABC_toTest.Get_First_Field(strSQL, parameters) != "0")
                    {
                        //更新
                        strSQL = @"Update C_OVERTIME_ONDUTY_FILE Set FILE_URL=@FILE_URL, ADD_ID=@ADD_ID, ADD_DATE=GETDATE() 
                                    Where MZ_AD=@MZ_AD And MZ_UNIT=@MZ_UNIT And FILE_DATE=@FILE_DATE ";
                        parameters.Clear();
                        parameters = new List<SqlParameter>()
                        {
                            new SqlParameter("FILE_URL", item.FILE_URL),
                            new SqlParameter("ADD_ID", item.ADD_ID),
                            new SqlParameter("MZ_AD", item.MZ_AD),
                            new SqlParameter("MZ_UNIT", item.MZ_UNIT),
                            new SqlParameter("FILE_DATE", item.FILE_DATE)
                        };
                    }
                    else
                    {
                        //新增
                        strSQL = @"INSERT INTO C_OVERTIME_ONDUTY_FILE(MZ_AD, MZ_UNIT, FILE_DATE, FILE_URL, ADD_ID, ADD_DATE) 
                                    VALUES(@MZ_AD, @MZ_UNIT, @FILE_DATE, @FILE_URL, @ADD_ID, GETDATE()) ";
                        parameters.Clear();
                        parameters = new List<SqlParameter>()
                        {
                            new SqlParameter("MZ_AD", item.MZ_AD),
                            new SqlParameter("MZ_UNIT", item.MZ_UNIT),
                            new SqlParameter("FILE_DATE", item.FILE_DATE),
                            new SqlParameter("FILE_URL", item.FILE_URL),
                            new SqlParameter("ADD_ID", item.ADD_ID)
                        };
                    }

                    otf.MultiSql_Add(strSQL, parameters.ToArray());
                    LogModel.saveLog("CF", strSQL.IndexOf("Update") > 0 ? "U" : "A", strSQL, parameters, TPMFION, "值日補休附件儲存。");
                }

                otf.MultiSQL_Exec();

                return true;
            }
            catch (Exception ex)
            {
                LogModel.saveLog("CF", "UA", ex.Message, new List<SqlParameter>(), TPMFION, "值日補休附件儲存異常。");
                throw ex;
            }
        }
        /// <summary>
        /// 值日補休附件儲存
        /// </summary>
        /// <param name="model"></param>
        /// <param name="TPMFION"></param>
        /// <returns></returns>
        public bool C_OVERTIME_ONDUTY_FILE_Save(C_OVERTIME_ONDUTY_FILE_Model model, string TPMFION = "")
        {
            return C_OVERTIME_ONDUTY_FILE_Save(new List<C_OVERTIME_ONDUTY_FILE_Model>() { model }, TPMFION);
        }
        /// <summary>
        /// 值日補休附件刪除
        /// </summary>
        /// <param name="model"></param>
        /// <param name="TPMFION"></param>
        /// <returns></returns>
        public bool C_OVERTIME_ONDUTY_FILE_Delete(C_OVERTIME_ONDUTY_FILE_Model model, string TPMFION = "")
        {
            try
            {
                string strSQL = string.Empty;
                List<SqlParameter> parameters;

                if (string.IsNullOrEmpty(model.MZ_AD) || string.IsNullOrEmpty(model.MZ_UNIT) || string.IsNullOrEmpty(model.FILE_DATE))
                {
                    return false;
                }

                //取回刪除路徑
                strSQL = @"Select FILE_URL From C_OVERTIME_ONDUTY_FILE Where MZ_AD=@MZ_AD And MZ_UNIT=@MZ_UNIT And FILE_DATE=@FILE_DATE ";
                parameters = new List<SqlParameter>()
                {
                    new SqlParameter("MZ_AD", model.MZ_AD),
                    new SqlParameter("MZ_UNIT", model.MZ_UNIT),
                    new SqlParameter("FILE_DATE", model.FILE_DATE)
                };

                DataTable onDutyFile = o_DBFactory.ABC_toTest.Create_Table(strSQL, parameters);
                foreach (DataRow item in onDutyFile.Rows)
                {
                    string filePath = WebConfigHelpers.C_UPLOAD_FILEPATH_TO_SERVER_PATH + item["FILE_URL"].ToStringNullSafe();
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                strSQL = @"DELETE FROM C_OVERTIME_ONDUTY_FILE Where MZ_AD=@MZ_AD And MZ_UNIT=@MZ_UNIT And FILE_DATE=@FILE_DATE ";
                if (o_DBFactory.ABC_toTest.DealCommandLog(strSQL, parameters))
                {
                    LogModel.saveLog("CF", "D", strSQL, parameters, TPMFION, "值日補休附件刪除。");
                }
                else
                {
                    LogModel.saveLog("CF", "D", strSQL, parameters, TPMFION, "值日補休附件刪除失敗。");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                LogModel.saveLog("CF", "D", ex.Message, new List<SqlParameter>(), TPMFION, "值日補休附件刪除異常。");
                throw ex;
            }
        }
        /// <summary>
        /// 加班指紋例外人員表刪除
        /// </summary>
        /// <param name="model"></param>
        /// <param name="TPMFION"></param>
        /// <returns></returns>
        public bool C_OVERTIME_EXCARD_Delete(C_OVERTIME_EXCARD_Model model, string TPMFION = "")
        {
            try
            {
                return false;
            }
            catch (Exception ex)
            {
                LogModel.saveLog("CF", "D", ex.Message, new List<SqlParameter>(), TPMFION, "加班指紋例外人員表刪除異常。");
                throw ex;
            }
        }

        /// <summary>
        /// 根據補休申請日,計算"最早加班日",需要考慮到新舊制的問題
        /// 
        /// </summary>
        /// <param name="date_MZ_IDATE1">補休申請日</param>
        /// <returns>最早加班日,代表該次補休申請,最早可以溯及的加班日是哪一天開始? </returns>
        public static DateTime Caculate_OVERDAY_LIMIT(DateTime date_MZ_IDATE1)
        {
            /*
             特殊:加班補休制度
             如果加班日早於'1120101',採用舊制,申請補休的期限只有一年
             如果加班日晚於等於 '1120101',採用新制,申請補休的期限長達兩年(730天)

             先判斷情境:
             A-申請補休日回推最多一年前的加班日,仍在舊制
             B-申請補休日回推最多一年前的加班日,已在新制，但是回推兩年前,仍在舊制
             C-申請補休日回推最多兩年前的加班日,都在新制

            計算"最早加班日"
             情境A:計算出補休日的365天以前是哪一天？為"最早加班日"
             情境B:1120101為"最早加班日"
                -舊制的加班補休申請此時都已經過期
                -但是新制的因為多了一年期限,所以都還沒過期
             情境C:計算出補休日的730天以前是哪一天？為"最早加班日"
             (1140101以後應該都是情境C了)
        
             */
            DateTime 新制度開始日 = Convert.ToDateTime("2023-01-01");
            //判斷當天採用的制度
            string Mode = "";
            //A-補休日一年前以內仍在舊制
            //A-申請補休日回推最多一年前的加班日,仍在舊制
            if (date_MZ_IDATE1.AddDays(-365) < 新制度開始日)
            {
                Mode = "A";
            }
            //B-申請補休日回推最多一年前的加班日,已在新制，但是回推兩年前,仍在舊制                
            if (新制度開始日 <= date_MZ_IDATE1.AddDays(-365)
                &&
                date_MZ_IDATE1.AddDays(-730) < 新制度開始日
                )
            {
                Mode = "B";
            }
            //C-申請補休日回推最多兩年前的加班日,都在新制
            if (新制度開始日 <= date_MZ_IDATE1.AddDays(-730)
                )
            {
                Mode = "C";
            }

            //最早加班日
            DateTime dateTarget = date_MZ_IDATE1.AddDays(-730);
            switch (Mode)
            {
                case "A":
                    //A: 計算出補休日的365天以前是哪一天？為"最早加班日"
                    dateTarget = date_MZ_IDATE1.AddDays(-365);
                    break;

                case "B":
                    //B: 1120101為"最早加班日"
                    dateTarget = 新制度開始日;
                    break;

                case "C":
                    //C:計算出補休日的730天以前是哪一天？為"最早加班日"
                    dateTarget = date_MZ_IDATE1.AddDays(-730);
                    break;
            }

            return dateTarget;
        }

        /// <summary>
        /// 根據補休申請日,計算"最早加班日",需要考慮到新舊制的問題
        /// 
        /// </summary>
        /// <param name="date_MZ_IDATE1">補休申請日</param>
        /// <returns>最早加班日,代表該次補休申請,最早可以溯及的加班日是哪一天開始? </returns>
        public static string Caculate_OVERDAY_LIMIT(string _MZ_IDATE1)
        {
            //先把民國年轉回日期物件
            DateTime date_MZ_IDATE1 =
                TPPDDB.Helpers.ForDateTime.TW_YYYMMDD_To_DateTime(_MZ_IDATE1)
                ?? DateTime.Now.Date;

            //根據補休申請日,計算"最早加班日",需要考慮到新舊制的問題
            DateTime OVERDAY_LIMIT = CFService.Caculate_OVERDAY_LIMIT(date_MZ_IDATE1);

            return TPPDDB.Helpers.ForDateTime.DateTime_To_TW_YYYMMDD(OVERDAY_LIMIT);
        }

        /// <summary>
        /// 加班費當月上限
        /// </summary>
        public const int Limit_MZ_OVERTIME_PAY = 19000;
    }
}