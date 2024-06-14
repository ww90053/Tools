using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using TPPDDB.Logic;
using TPPDDB.Model;
using TPPDDB.App_Code;

namespace TPPDDB.Service
{
    /// <summary>
    /// 超勤與加班之相關資料庫操作均放這
    /// </summary>
    public static class DutyService
    {
        /// <summary>插入 : 印領清冊</summary>
        public static Boolean Insert_OverTimeStatistics(List<SqlParameter> lstParam, out string sqlStr, ref String ErrMsg)
        {
            String SQL = String.Format(@"INSERT INTO C_DUTYMONTHOVERTIME_HOUR 
                                            (MZ_ID, MZ_YEAR, MZ_MONTH, " 
                                             + "\"1\",  \"2\",  \"3\",  \"4\",  \"5\",  \"6\",  \"7\",  \"8\",  \"9\",  \"10\"," 
                                             + "\"11\", \"12\", \"13\", \"14\", \"15\" ,\"16\" ,\"17\", \"18\", \"19\", \"20\"," 
                                             + "\"21\", \"22\", \"23\", \"24\", \"25\" ,\"26\", \"27\", \"28\", \"29\", \"30\", \"31\"," +
                                            @"MZ_BUDGET_HOUR, --上班時數
                                             MZ_REAL_HOUR, --實際支領超勤時數
                                             MZ_BALANCE_HOUR, --結餘時數
                                             PAY1, --月俸
                                             PROFESS, --專業加給
                                             BOSS, --長官加給
                                             MZ_HOUR_PAY, --每小時超勤金額
                                             MZ_OVERTIME_PAY, --總支付超勤金額
                                             MZ_AD, MZ_EXAD, MZ_UNIT, MZ_EXUNIT, DUTYUSERCLASSIFY)
                                         VALUES
                                            (@MZ_ID, @MZ_YEAR, @MZ_MONTH, 
                                             @D1,  @D2,  @D3,  @D4,  @D5,  @D6,  @D7,  @D8,  @D9,  @D10,
                                             @D11, @D12, @D13, @D14, @D15 ,@D16 ,@D17, @D18, @D19, @D20,
                                             @D21, @D22, @D23, @D24, @D25 ,@D26, @D27, @D28, @D29, @D30, @D31,
                                             @MZ_BUDGET_HOUR,
                                             @MZ_REAL_HOUR,
                                             @MZ_BALANCE_HOUR,
                                             @PAY1,
                                             @PROFESS,
                                             @BOSS,
                                             @MZ_HOUR_PAY,
                                             @MZ_OVERTIME_PAY,
                                             @MZ_AD, @MZ_EXAD, @MZ_UNIT, @MZ_EXUNIT, @DUTYUSERCLASSIFY) ");
            sqlStr = SQL;
            try
            {
                o_DBFactory.ABC_toTest.ExecuteNonQuery( SQL, lstParam.ToArray());
                return true;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return false;
            }
        }

        /// <summary>插入 : 印領清冊</summary>
        public static Boolean Insert_OverTimeStatistics_Hour(List<SqlParameter> lstParam, out string sqlStr, ref String ErrMsg)
        {
            String SQL = String.Format(@"INSERT INTO C_OVERTIMEMONTH_HOUR 
                                            (MZ_ID, MZ_YEAR, MZ_MONTH, "
                                             + "\"1\",  \"2\",  \"3\",  \"4\",  \"5\",  \"6\",  \"7\",  \"8\",  \"9\",  \"10\","
                                             + "\"11\", \"12\", \"13\", \"14\", \"15\" ,\"16\" ,\"17\", \"18\", \"19\", \"20\","
                                             + "\"21\", \"22\", \"23\", \"24\", \"25\" ,\"26\", \"27\", \"28\", \"29\", \"30\", \"31\"," +
                                            @"MZ_BUDGET_HOUR, --上班時數
                                             MZ_REAL_HOUR, --實際支領超勤時數
                                             MZ_BALANCE_HOUR, --結餘時數
                                             TOTAL, --結餘時數
                                             PAY1, --月俸
                                             PROFESS, --專業加給
                                             BOSS, --長官加給
                                             MZ_HOUR_PAY, --每小時超勤金額
                                             MZ_OVERTIME_PAY, --總支付超勤金額
                                             MZ_AD, MZ_EXAD, MZ_UNIT, MZ_EXUNIT,
                                            MZ_OVERTIME_PAY_JOB   ,
                                            MZ_OVERTIME_PAY_SHIFT ,
                                            TOTAL_JOB             ,
                                            TOTAL_SHIFT            )
                                         VALUES
                                            (@MZ_ID, @MZ_YEAR, @MZ_MONTH, 
                                             @D1,  @D2,  @D3,  @D4,  @D5,  @D6,  @D7,  @D8,  @D9,  @D10,
                                             @D11, @D12, @D13, @D14, @D15 ,@D16 ,@D17, @D18, @D19, @D20,
                                             @D21, @D22, @D23, @D24, @D25 ,@D26, @D27, @D28, @D29, @D30, @D31,
                                             @MZ_BUDGET_HOUR,
                                             @MZ_REAL_HOUR,
                                             @MZ_BALANCE_HOUR,
                                             @TOTAL,
                                             @PAY1,
                                             @PROFESS,
                                             @BOSS,
                                             @MZ_HOUR_PAY,
                                             @MZ_OVERTIME_PAY,
                                             @MZ_AD, @MZ_EXAD, @MZ_UNIT, @MZ_EXUNIT,
                                             @MZ_OVERTIME_PAY_JOB   ,
                                             @MZ_OVERTIME_PAY_SHIFT ,
                                             @TOTAL_JOB             ,
                                             @TOTAL_SHIFT            )

                                            ");
            sqlStr = SQL;
            try
            {
                o_DBFactory.ABC_toTest.ExecuteNonQuery( SQL, lstParam.ToArray());
                return true;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return false;
            }
        }

        /// <summary>插入 : 加班</summary>
        public static Boolean Insert_OverTime(List<SqlParameter> lstParam)
        {
            String SQL = String.Format(@"INSERT INTO C_OVERTIME_BASE 
                                            (MZ_ID,OVER_DAY,MZ_EXAD,MZ_EXUNIT,MZ_OCCC,SN,REASON,OVER_STIME,OVER_ETIME,OVER_TOTAL,SURPLUS_TOTAL,LOCK_FLAG,OVERTIME_TYPE,
                                            IS_FREESIGN,OPTYPE,MZ_AD,MZ_UNIT,INS_ID,INS_DATE)
                                         VALUES
                                            (@MZ_ID,@OVER_DAY,@MZ_EXAD,@MZ_EXUNIT,@MZ_OCCC,  NEXT VALUE FOR dbo.C_OVERTIME_BASE_SN,@REASON,@OVER_STIME,@OVER_ETIME,@OVER_TOTAL,@SURPLUS_TOTAL,@LOCK_FLAG,@OVERTIME_TYPE,
                                            @IS_FREESIGN,@OPTYPE,@MZ_AD,@MZ_UNIT,@INS_ID,@INS_DATE) ");

            try
            {
                o_DBFactory.ABC_toTest.ExecuteNonQuery( SQL, lstParam.ToArray());
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>插入 : 超勤時數上限設定檔</summary>
        public static Boolean Insert_OverTime_Limit(List<SqlParameter> lstParam, ref String ErrMsg)
        {
            String SQL = @"INSERT INTO C_DUTYLIMIT(MZ_AD,MZ_UNIT,MZ_ID,MZ_HOUR_LIMIT,MZ_MONEY_LIMIT) 
                                            VALUES(@MZ_AD,@MZ_UNIT,@MZ_ID,@MZ_HOUR_LIMIT,@MZ_MONEY_LIMIT)";
            try
            {
                o_DBFactory.ABC_toTest.ExecuteNonQuery( SQL, lstParam.ToArray());
                return true;
            }
            catch (Exception ex)
            {
                //避免使用者直接看到無法理解的訊息
                //ErrMsg = ex.Message;
                ErrMsg = "超勤時數上限資料新增失敗。";
                return false;
            }
        }

        /// <summary>更新 : 超勤時數上限設定檔</summary>
        public static Boolean Update_OverTime_Limit(List<SqlParameter> lstParam, ref String ErrMsg)
        {
            /*
             * 原條件只有MZ_ID且更新PK值，導致人員多筆資料時造成PK重複異常。
             * 現修改為針對PK值更新上限時數及金額。
             * 20180517 by sky
             */
            String SQL = @"UPDATE C_DUTYLIMIT SET MZ_HOUR_LIMIT = @MZ_HOUR_LIMIT,
                                                  MZ_MONEY_LIMIT = @MZ_MONEY_LIMIT
                                            WHERE MZ_AD = @MZ_AD
                                                  AND MZ_UNIT = @MZ_UNIT 
                                                  AND MZ_ID = @MZ_ID";
            try
            {
                o_DBFactory.ABC_toTest.ExecuteNonQuery( SQL, lstParam.ToArray());
                return true;
            }
            catch (Exception ex)
            {
                //避免使用者直接看到無法理解的訊息
                //ErrMsg = ex.Message;
                ErrMsg = "超勤時數上限資料更新失敗";
                return false;
            }
        }

        /// <summary>更新 : 加班時數   </summary>
        public static Boolean Update_OverTime(List<SqlParameter> lstParam)
        {
            String SQL = String.Format(@"UPDATE C_OVERTIME_BASE SET OVER_TOTAL = @OVER_TOTAL, SURPLUS_TOTAL=@SURPLUS_TOTAL, OVER_STIME=@OVER_STIME, OVER_ETIME=@OVER_ETIME  
                                         WHERE MZ_ID = @MZ_ID and OVER_DAY = @OVER_DAY and OVERTIME_TYPE='OTU' ");

            try
            {
                o_DBFactory.ABC_toTest.ExecuteNonQuery( SQL, lstParam.ToArray());
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 刪除補休時數為零的加班資料
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static bool Delete_OverTime(List<SqlParameter> parameters, string TPMFION = "")
        {
            string strSQL = string.Format(@"Delete From C_OVERTIME_BASE Where MZ_ID = @MZ_ID and OVER_DAY = @OVER_DAY and OVERTIME_TYPE='OTU' ");

            try
            {
                //紀錄Log
                LogModel.saveLog("COHI", "D", strSQL, parameters, TPMFION, "刪除補休時數為0的加班資料。");

                //執行刪除
                o_DBFactory.ABC_toTest.ExecuteNonQuery( strSQL, parameters.ToArray());

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>取得 : 個人超勤時數上限</summary>
        /// <param name="ID"></param>
        /// <param name="AD"></param>
        /// <param name="Unit"></param>
        /// <returns></returns>
        public static DataTable lookupOverTime_Limit(String ID, String AD, String Unit)
        {
            //增加發薪機關及編制單位條件來因應人員多筆超勤時數上限 20180615 by sky
            String SQL = String.Format(@"select * from C_DUTYLIMIT WHERE MZ_ID = '{0}' And MZ_AD = '{1}' And MZ_UNIT = '{2}'",
                                       ID, AD, Unit);

            try
            {
                DataTable dt = o_DBFactory.ABC_toTest.Create_Table(SQL, "C_DUTYLIMIT");

                if (dt.Rows.Count <= 0)
                    return new DataTable();
                else
                    return dt;
            }
            catch (Exception ex)
            {
                String a = ex.Message;
                return new DataTable();
            }
        }

        /// <summary>取得某特定機關(與特定單位)底下所有帳號資料</summary>
        /// <param name="ID">身分證字號</param>
        /// <param name="DutyDate">值勤時間</param>
        /// <param name="AD">發薪機關</param>
        /// <param name="Unit">編制單位</param>
        /// <returns></returns>
        public static DataTable lookupOverTime(String ID, String DutyDate, String AD, String Unit, String ExAD, String ExUnit)
        {
            String SQL = String.Format(@"SELECT *
                                          FROM C_DUTYTABLE_PERSONAL
                                          WHERE MZ_ID = '{0}' AND DUTYDATE like '{1}%' 
                                                and MZ_AD = '{2}' and MZ_UNIT = '{3}'
                                                and MZ_EXAD = '{4}' and MZ_EXUNIT = '{5}'"
                                        ,ID
                                        ,DutyDate
                                        ,AD
                                        ,Unit
                                        ,ExAD
                                        ,ExUnit);

            try
            {
                DataTable dt = o_DBFactory.ABC_toTest.Create_Table(SQL, "C_DUTYTABLE_PERSONAL");

                if (dt.Rows.Count <= 0)
                    return new DataTable();
                else
                    return dt;
            }
            catch (Exception ex)
            {
                String a = ex.Message;
                return new DataTable();
            }
        }

        /// <summary>取得某特定機關(與特定單位)底下所有帳號資料</summary>
        /// <param name="ID">身分證字號</param>
        /// <param name="DutyDate">值勤時間</param>
        /// <param name="AD">發薪機關</param>
        /// <param name="Unit">編制單位</param>
        /// <returns></returns>
        public static DataTable lookupOverTime_hour(String ID, String DutyDate, String AD, String Unit, String ExAD, String ExUnit)
        {
            String SQL = String.Format(@"SELECT *
                                          FROM C_OVERTIME_BASE
                                          WHERE ( OVERTIME_TYPE = 'OTB' OR OVERTIME_TYPE = 'OTT' ) AND MZ_ID = '{0}' AND OVER_DAY like '{1}%' 
                                                and MZ_EXAD = '{2}' and MZ_EXUNIT = '{3}'
                                                and MZ_EXAD = '{4}' and MZ_EXUNIT = '{5}'"
                                        , ID
                                        , DutyDate
                                        , ExAD
                                        , ExUnit
                                        , ExAD
                                        , ExUnit);

            try
            {
                DataTable dt = o_DBFactory.ABC_toTest.Create_Table(SQL, "C_OVERTIME_BASE");

                if (dt.Rows.Count <= 0)
                    return new DataTable();
                else
                    return dt;
            }
            catch (Exception ex)
            {
                String a = ex.Message;
                return new DataTable();
            }
        }

        public static DataTable lookupOverTimeByBuild(String ID, String DutyDate, String AD, String Unit)
        {
            String SQL = String.Format(@"SELECT *
                                          FROM C_DUTYTABLE_PERSONAL
                                          WHERE MZ_ID = '{0}'
                                                AND DUTYDATE like '{1}%' 
                                                AND MZ_AD = '{2}'
                                                AND MZ_UNIT = '{3}'"
                                        , ID
                                        , DutyDate
                                        , AD
                                        , Unit);

            try
            {
                DataTable dt = o_DBFactory.ABC_toTest.Create_Table(SQL, "C_DUTYTABLE_PERSONAL");

                if (dt.Rows.Count <= 0)
                    return new DataTable();
                else
                    return dt;
            }
            catch (Exception ex)
            {
                String a = ex.Message;
                return new DataTable();
            }
        }

        /// <summary>判斷某使用者是否已經有設定超勤金額上限</summary>
        public static Boolean isUserSetOvertTimeLimit(String MZ_ID, String AD, String Unit)
        {
            String SQL = String.Format(@"SELECT case Count(*) when 0 then 'N' else 'Y' end AS isExist
                                           FROM C_DUTYLIMIT
                                          WHERE MZ_ID = @MZ_ID --身分證字號 
                                                And MZ_AD = @MZ_AD 
                                                And MZ_UNIT = @MZ_UNIT ");
            try
            {
                List<SqlParameter> lstParameter = new List<SqlParameter>();
                lstParameter.Add(new SqlParameter("MZ_ID", SqlDbType.NVarChar) { Value = MZ_ID }); //身分證字號
                lstParameter.Add(new SqlParameter("MZ_AD", SqlDbType.NVarChar) { Value = AD });
                lstParameter.Add(new SqlParameter("MZ_UNIT", SqlDbType.NVarChar) { Value = Unit });

                String result = o_DBFactory.ABC_toTest.GetValue( SQL, lstParameter.ToArray()).ToString();

                return (result == "Y") ? true : false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>判斷超勤表是否已經審核</summary>
        public static Boolean isDutyTableVerify(String MZ_ID, String DutyDate)
        {
            String SQL = String.Format(@"SELECT NVL(MZ_VERIFY, 'N')
                                         FROM C_DUTYTABLE_PERSONAL
                                         WHERE MZ_ID = '{0}' AND DUTYDATE = '{1}'"
                                        , MZ_ID
                                        ,DutyDate);
            try
            {
                //20151119 Neil Parameter 有問題，改用String直接執行
                //List<SqlParameter> lstParameter = new List<SqlParameter>();
                //                      lstParameter.Add(new SqlParameter("MZ_ID", SqlDbType.NVarChar) { Value = MZ_ID }); //身分證字號
                //                      lstParameter.Add(new SqlParameter("DUTYDATE", SqlDbType.NVarChar) { Value = DutyDate }); //身分證字號

                String result = o_DBFactory.ABC_toTest.vExecSQL(SQL);
                //String result = o_DBFactory.ABC_toTest.GetValue(SQL, lstParameter);

                return (result == "Y") ? true : false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>查詢 : 加班表 C_OVERTIME_HOUR_INSIDE 是否已鎖定</summary>
        /// <param name="MZ_ID">身份證字號</param>
        /// <param name="DutyDate">加班日期</param>
        /// 20150514 Neil
        public static Boolean isOvertimeLock(String MZ_ID, String DutyDate, enumOverDutyType Type)
        {
            String SQL = String.Format(@"SELECT LOCK_FLAG
                                           FROM C_OVERTIME_HOUR_INSIDE
                                          WHERE MZ_ID = @MZ_ID --身分證字號
                                                and RESTFLAG = @RESTFLAG --補休類型
                                                and MZ_DATE = @MZ_DATE --補休日期 ");
            try
            {
                List<SqlParameter> lstParameter = new List<SqlParameter>();
                lstParameter.Add(new SqlParameter("MZ_ID", SqlDbType.NVarChar)      { Value = MZ_ID }); //身分證字號
                lstParameter.Add(new SqlParameter("RESTFLAG", SqlDbType.NVarChar)   { Value = getEnumOverDutyType(Type) }); //加班類型
                lstParameter.Add(new SqlParameter("MZ_DATE", SqlDbType.NVarChar)    { Value = DutyDate }); //加班日期

                String result = o_DBFactory.ABC_toTest.GetValue(SQL, lstParameter);

                return (result == "Y") ? true : false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>查詢 : 加班表 C_OVERTIME_HOUR_INSIDE 是否已存在</summary>
        /// <param name="MZ_ID">身份證字號</param>
        /// <param name="DutyDate">加班日期</param>
        /// 20150514 Neil
        public static Boolean isOvertimeExist(String MZ_ID, String DutyDate)
        {
            String SQL = String.Format(@"SELECT CASE COUNT(*) WHEN 0 THEN 'N' ELSE 'Y' END AS isExist 
                                           FROM C_OVERTIME_BASE
                                          WHERE MZ_ID = @MZ_ID --身分證字號
                                                and OVER_DAY = @MZ_DATE --加班日期
                                                and OVERTIME_TYPE = 'OTU' --加班日期");

            try
            {
                List<SqlParameter> lstParameter = new List<SqlParameter>();
                                      lstParameter.Add(new SqlParameter("MZ_ID", SqlDbType.NVarChar) { Value = MZ_ID }); //身分證字號
                                      lstParameter.Add(new SqlParameter("MZ_DATE", SqlDbType.NVarChar) { Value = DutyDate }); //加班日期

                String result = o_DBFactory.ABC_toTest.GetValue(SQL, lstParameter);

                return (result == "Y") ? true : false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>查詢 : 加班表 C_OVERTIME_BASE 超勤補休 是否已存在? 且 已經使用了超勤補休來請假
        /// 加班類型為 OTU超勤補休
        /// </summary>
        /// <param name="MZ_ID">身份證字號</param>
        /// <param name="DutyDate">加班日期</param>
        /// 20150514 Neil
        public static Boolean isOvertime_And_RestHour_Exist(String MZ_ID, String DutyDate)
        {
            String SQL = String.Format(@"SELECT CASE COUNT(*) WHEN 0 THEN 'N' ELSE 'Y' END AS isExist 
                                           FROM C_OVERTIME_BASE
                                          WHERE MZ_ID = @MZ_ID --身分證字號
                                                and OVER_DAY = @MZ_DATE --加班日期
                                                and OVERTIME_TYPE = 'OTU' --加班日期
                                                and REST_HOUR > 0 --當天加班已經被申請補休了
");

            try
            {
                List<SqlParameter> lstParameter = new List<SqlParameter>();
                lstParameter.Add(new SqlParameter("MZ_ID", SqlDbType.NVarChar) { Value = MZ_ID }); //身分證字號
                lstParameter.Add(new SqlParameter("MZ_DATE", SqlDbType.NVarChar) { Value = DutyDate }); //加班日期

                String result = o_DBFactory.ABC_toTest.GetValue(SQL, lstParameter);

                return (result == "Y") ? true : false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>取得全部的國定假日之字典檔</summary>
        /// 20150514 Neil
        public static Dictionary<String, String> Lookup_Holidays()
        {
            Dictionary<String, String> result = new Dictionary<String, String>();

            DataSet ds = o_DBFactory.ABC_toTest.ExecuteDataset( @"select MZ_HOLIDAY_DATE, MZ_HOLIDAY_NAME  from C_DUTYHOLIDAY");
            DataTable dt = ds.Tables[0];

            foreach (DataRow dr in dt.Rows)
                result.Add(dr["MZ_HOLIDAY_DATE"].ToString(), dr["MZ_HOLIDAY_NAME"].ToString());

            return result;
        }
        

        //當利用編制機關單位查找時，取得該員現服的每日超勤表字典檔
        public static List<StateDepartmentModel> Lookup_ExDept(String ID, String DutyDate)
        {
            List<StateDepartmentModel> lstResult = new List<StateDepartmentModel>();
            String SQL = String.Format(@"Select MZ_AD, MZ_UNIT, MZ_EXAD, MZ_EXUNIT
                                         from C_dutytable_personal WHERE MZ_ID = '{0}' AND DUTYDATE like '{1}%'
                                         GROUP BY MZ_AD, MZ_UNIT, MZ_EXAD, MZ_EXUNIT",
                                       ID,
                                       DutyDate);

            try
            {
                DataTable dt = o_DBFactory.ABC_toTest.Create_Table(SQL, "C_DUTYLIMIT");

                if (dt.Rows.Count <= 0)
                    return new List<StateDepartmentModel>();
                else
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        if (!String.IsNullOrEmpty(item["MZ_AD"].ToString()) && !String.IsNullOrEmpty(item["MZ_UNIT"].ToString()))
                        {
                            StateDepartmentModel m = new StateDepartmentModel();
                                                 m.AD = item["MZ_AD"].ToString() ;
                                                 m.UNIT = item["MZ_UNIT"].ToString() ;
                                                 m.EX_AD = item["MZ_EXAD"].ToString();
                                                 m.EX_UNIT = item["MZ_EXUNIT"].ToString();
                            lstResult.Add(m);
                        }
                    }

                    return lstResult;
                }
            }
            catch (Exception ex)
            {
                String a = ex.Message;
                return new List<StateDepartmentModel>();
            }
        }

        //當利用編制機關單位查找時，取得該員現服的每日超勤表字典檔
        public static List<StateDepartmentModel> Lookup_ExDept_hour(String ID, String DutyDate)
        {
            List<StateDepartmentModel> lstResult = new List<StateDepartmentModel>();
            String SQL = String.Format(@"Select MZ_EXAD, MZ_EXUNIT
                                         from C_OverTime_Base WHERE MZ_ID = '{0}' AND Over_day like '{1}%'
                                         GROUP BY  MZ_EXAD, MZ_EXUNIT",
                                       ID,
                                       DutyDate);
            DataTable dt = o_DBFactory.ABC_toTest.Create_Table(SQL, "C_DUTYLIMIT");
            foreach (DataRow item in dt.Rows)
            {
                if (!String.IsNullOrEmpty(item["MZ_EXAD"].ToString()) && !String.IsNullOrEmpty(item["MZ_EXUNIT"].ToString()))
                {
                    StateDepartmentModel m = new StateDepartmentModel();
                    m.AD = item["MZ_EXAD"].ToString();
                    m.UNIT = item["MZ_EXUNIT"].ToString();
                    m.EX_AD = item["MZ_EXAD"].ToString();
                    m.EX_UNIT = item["MZ_EXUNIT"].ToString();
                    lstResult.Add(m);
                }
            }

            return lstResult;

            //try
            //{
            //    DataTable dt = o_DBFactory.ABC_toTest.Create_Table(SQL, "C_DUTYLIMIT");

            //    if (dt.Rows.Count <= 0)
            //        return new List<StateDepartmentModel>();
            //    else
            //    {
            //        foreach (DataRow item in dt.Rows)
            //        {
            //            if (!String.IsNullOrEmpty(item["MZ_AD"].ToString()) && !String.IsNullOrEmpty(item["MZ_UNIT"].ToString()))
            //            {
            //                StateDepartmentModel m = new StateDepartmentModel();
            //                m.AD = item["MZ_AD"].ToString();
            //                m.UNIT = item["MZ_UNIT"].ToString();
            //                m.EX_AD = item["MZ_EXAD"].ToString();
            //                m.EX_UNIT = item["MZ_EXUNIT"].ToString();
            //                lstResult.Add(m);
            //            }
            //        }

            //        return lstResult;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    String a = ex.Message;
            //    return new List<StateDepartmentModel>();
            //}
        }

        /// <summary>取得特定查詢條件下，當月份的所有員工 ID</summary>
        /// <param name="AD">發薪機關</param>
        /// <param name="Unit">編制單位</param>
        /// <param name="DutyDate">超勤年月(ex 10405)</param>
        /// <param name="ID">身分證字號(非必填)</param>
        /// <returns></returns>
        public static List<String> Lookup_Personals(String AD, String Unit, String DutyDate, String ID = "")
        {
            String WHERE = "";
            if (!String.IsNullOrEmpty(ID))
                WHERE = String.Format(@" AND MZ_ID = '{0}' ", ID);

            List<String> lstResult = new List<String>();
            String SQL = String.Format(@"SELECT MZ_ID
                                         FROM C_dutytable_personal WHERE MZ_AD = '{0}' AND MZ_UNIT = '{1}' AND DUTYDATE like '{2}%' {3}
                                         GROUP BY MZ_ID",
                                         AD,
                                         Unit,
                                         DutyDate,
                                         WHERE);

            try
            {
                DataTable dt = o_DBFactory.ABC_toTest.Create_Table(SQL, "C_DUTYLIMIT");

                if (dt.Rows.Count <= 0)
                    return lstResult;
                else
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        String d = item["MZ_ID"].ToString();
                        if (!String.IsNullOrEmpty(d))
                            lstResult.Add(d);
                    }

                    return lstResult;
                }
            }
            catch (Exception ex)
            {
                String a = ex.Message;
                return lstResult;
            }
        }

        /// <summary>取得特定查詢條件下，當月份的所有員工 ID</summary>
        /// <param name="AD">發薪機關</param>
        /// <param name="Unit">編制單位</param>
        /// <param name="DutyDate">加班年月(ex 10405)</param>
        /// <param name="ID">身分證字號(非必填)</param>
        /// <returns></returns>
        public static List<String> Lookup_Personals_hour(String AD, String Unit, String DutyDate, String ID = "")
        {
            String WHERE = "";
            if (!String.IsNullOrEmpty(ID))
                WHERE = String.Format(@" AND MZ_ID = '{0}' ", ID);

            List<String> lstResult = new List<String>();
            String SQL = String.Format(@"SELECT MZ_ID
                                         FROM C_OverTime_Base WHERE MZ_EXAD = '{0}' AND MZ_EXUNIT = '{1}' AND OVER_DAY like '{2}%' {3}
                                         GROUP BY MZ_ID",
                                         AD,
                                         Unit,
                                         DutyDate,
                                         WHERE);

            DataTable dt = o_DBFactory.ABC_toTest.Create_Table(SQL, "C_OverTime_Base");
            foreach (DataRow item in dt.Rows)
            {
                String d = item["MZ_ID"].ToString();
                if (!String.IsNullOrEmpty(d))
                    lstResult.Add(d);
            }

            return lstResult;

            //try
            //{
            //    DataTable dt = o_DBFactory.ABC_toTest.Create_Table(SQL, "C_DUTYLIMIT");

            //    if (dt.Rows.Count <= 0)
            //        return lstResult;
            //    else
            //    {
            //        foreach (DataRow item in dt.Rows)
            //        {
            //            String d = item["MZ_ID"].ToString();
            //            if (!String.IsNullOrEmpty(d))
            //                lstResult.Add(d);
            //        }

            //        return lstResult;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    String a = ex.Message;
            //    return lstResult;
            //}
        }

        private static String getEnumOverDutyType(enumOverDutyType e)
        {
            switch (e)
            {
                case enumOverDutyType.N: return "N";
                case enumOverDutyType.YD: return "YD";
                case enumOverDutyType.YO: return "YO";
                case enumOverDutyType.YU: return "YU";
                default: return "";
            }
        }

        /// <summary>加班種類</summary>
        public enum enumOverDutyType
        {
            /// <summary>
            /// 廢棄狀態，目前暫時無用
            /// </summary>
            Y,
            /// <summary>
            /// 支領加班費
            /// </summary>
            N,
            /// <summary>
            /// 加班補休
            /// </summary>
            YO,
            /// <summary>
            /// 超勤補修
            /// </summary>
            YU,
            /// <summary>
            /// 值日補休
            /// </summary>
            YD
        }
    }
}