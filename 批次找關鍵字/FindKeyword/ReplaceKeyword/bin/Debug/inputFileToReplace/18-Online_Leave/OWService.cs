using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using TPPDDB.Helpers;
using TPPDDB.Models._18_Online_Work;

namespace TPPDDB._18_Online_Leave
{
    public class OWService
    {
        /// <summary>
        /// 統計需審核加班筆數
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public int GetOverTimeReViewCount(Online_work_Query query)
        {
            try
            {
                int result = 0;
                if (string.IsNullOrEmpty(query.PerformReviewID) || string.IsNullOrEmpty(query.OVERTIME_TYPE))
                {
                    throw new Exception("無審核人ID、加班單類型。");
                }

                string strSQL = @"Select Count(*) From C_OVERTIME_HISTORY coh
                                  INNER JOIN C_OVERTIME_BASE cob on cob.SN = coh.OVERTIME_SN
                                  Where coh.REVIEW_ID=@PerformReviewID And coh.OVERTIME_TYPE=@OVERTIME_TYPE And
                                        coh.PROCESS_STATUS='4' And (coh.PROCESS_DATE is null or coh.PROCESS_DATE='') ";
                List<SqlParameter> parameters = new List<SqlParameter>()
                {
                    new SqlParameter("PerformReviewID", query.PerformReviewID),
                    new SqlParameter("OVERTIME_TYPE", query.OVERTIME_TYPE)
                };


                result = Convert.ToInt32(o_DBFactory.ABC_toTest.Get_First_Field(strSQL, parameters));
                //LogModel.saveLog("OW", "S", strSQL, parameters, query.TPMFION, "統計需審核加班筆數。");

                return result;
            }
            catch (Exception ex)
            {
                //LogModel.saveLog("OW", "S", ex.Message, new List<SqlParameter>(), query.TPMFION, "統計需審核加班筆數異常。");
                throw ex;
            }
        }
        /// <summary>
        /// 取得需審核的加班資料
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DataTable GetReViewOverTimeBase(Online_work_Query query)
        {
            try
            {
                if (string.IsNullOrEmpty(query.PerformReviewID) || string.IsNullOrEmpty(query.OVERTIME_TYPE))
                {
                    return null;
                }

                string strSQL = @"Select TWDATE_FORMAT(OVER_DAY, 'yyy/mm/dd') OVER_DAY, ab.MZ_NAME, 
                                    --格式長這樣 理由/備註
                                    cob.REASON+'/'+cob.OVER_REMARK as REASON,
                                  (CASE WHEN cob.overtime_chg_total=0 THEN  CONCAT(floor(0),'時') + CONCAT(floor(0),'分') ELSE CONCAT(floor(overtime_chg_total/60),'時') + CONCAT(overtime_chg_total - (floor(overtime_chg_total/60)*60),'分') END ) AS PAYTIME,
                                  (Select MIN(LOGTIME) From C_CARDHISTORY_NEW Where USERNAME like (cob.MZ_ID+'%') And LOGDATE=RCDATE_FORMAT(cob.OVER_DAY,'yyyy/MM/dd')) FirstTime,
                                  (Select MAX(LOGTIME) From C_CARDHISTORY_NEW Where USERNAME like (cob.MZ_ID+'%') And LOGDATE=RCDATE_FORMAT(cob.OVER_DAY,'yyyy/MM/dd')) LastTime,
                                  coh.O_SN, coh.OVERTIME_SN, cob.MZ_ID, cob.OVER_DAY COB_OVER_DAY,cob.SN
                                  From C_OVERTIME_HISTORY coh
                                  left join C_OVERTIME_BASE cob on cob.SN = coh.OVERTIME_SN
                                  left join A_DLBASE ab on ab.MZ_ID = cob.MZ_ID 
                                  Where coh.REVIEW_ID=@PerformReviewID And coh.PROCESS_DATE is null And coh.PROCESS_STATUS='4' And coh.OVERTIME_TYPE=@OVERTIME_TYPE ";
                List<SqlParameter> parameters = new List<SqlParameter>()
                {
                    new SqlParameter("PerformReviewID", query.PerformReviewID),
                    new SqlParameter("OVERTIME_TYPE", query.OVERTIME_TYPE)
                };

                DataTable dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, parameters);
                //LogModel.saveLog("OW", "S", strSQL, parameters, query.TPMFION, "查詢需審核的加班資料。");


                return dt;
            }
            catch (Exception ex)
            {
                //LogModel.saveLog("OW", "S", ex.Message, new List<SqlParameter>(), query.TPMFION, "查詢需審核的加班資料異常。");
                return null;
            }
        }
        /// <summary>
        /// 取得加班簽核歷程資料
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DataTable GetOverTimeHistory(Online_work_Query query)
        {
            try
            {
                string strSQL = string.Empty;
                List<SqlParameter> parameters = new List<SqlParameter>();

                if (string.IsNullOrEmpty(query.OVERTIME_SN) || string.IsNullOrEmpty(query.OVERTIME_TYPE))
                {
                    return null;
                }

                strSQL = @"Select (TWDATE_FORMAT(coh.PROCESS_DATE, 'yyy/MM/dd') + ' ' + coh.PROCESS_TIME) PROCESS_DATE,
                                    CASE WHEN ad.MZ_KCHI='新北市政府警察局' THEN '警察局' ELSE replace(ad.MZ_KCHI,'新北市政府警察局') End REVIEW_AD, 
                                    unit.MZ_KCHI REVIEW_UNIT,oc.MZ_KCHI REVIEW_OCCC, adb.MZ_NAME REVIEW_NAME, cs.C_STATUS_NAME, coh.REVIEW_MESSAGE
                            From C_OVERTIME_HISTORY coh
                            left join A_DLBASE adb on adb.MZ_ID= coh.REVIEW_ID
                            left join A_KTYPE ad on ad.MZ_KTYPE='04' And ad.MZ_KCODE=adb.MZ_EXAD
                            left join A_KTYPE unit on unit.MZ_KTYPE='25' And unit.MZ_KCODE=adb.MZ_EXUNIT
                            left join A_KTYPE oc on oc.MZ_KTYPE='26' And oc.MZ_KCODE=adb.MZ_OCCC
                            INNER JOIN C_STATUS cs ON cs.C_STATUS_SN=coh.PROCESS_STATUS
                            WHERE coh.OVERTIME_SN=@OVERTIME_SN And coh.OVERTIME_TYPE=@OVERTIME_TYPE
                            Order by coh.O_SN";
                parameters = new List<SqlParameter>()
                {
                    new SqlParameter("OVERTIME_SN", query.OVERTIME_SN),
                    new SqlParameter("OVERTIME_TYPE", query.OVERTIME_TYPE)
                };

                DataTable dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, parameters);
                //LogModel.saveLog("OW", "S", strSQL, parameters, query.TPMFION, "查詢加班簽核歷程資料。");

                return dt;
            }
            catch (Exception ex)
            {
                //LogModel.saveLog("OW", "S", ex.Message, new List<SqlParameter>(), query.TPMFION, "查詢加班簽核歷程資料異常。");
                return null;
            }
        }
        /// <summary>
        /// 取得加班線上簽核人員資料
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DataTable GetOverTimeOnlineReview(Online_work_Query query)
        {
            try
            {
                if (string.IsNullOrEmpty(query.OVERTIME_TYPE) || string.IsNullOrEmpty(query.SCHEDULE_SORT))
                {
                    throw new Exception("無簽核單類型或流程層級資料。");
                }

                string strSQL = @"Select * From C_OVERTIME_SCHEDULE 
                                  INNER JOIN C_LEAVE_SCHEDULE_CODE clsc on clsc.CODE_VALUE=SCHEDULE_CODE 
                                  Where SCHEDULE_TYPE=@SCHEDULE_TYPE And SORT=@SORT ";
                List<SqlParameter> parameters = new List<SqlParameter>()
                {
                    new SqlParameter("SCHEDULE_TYPE", query.OVERTIME_TYPE),
                    new SqlParameter("SORT", query.SCHEDULE_SORT)
                };

                DataTable cosDt = o_DBFactory.ABC_toTest.Create_Table(strSQL, parameters);

                if (cosDt != null && cosDt.Rows.Count > 0)
                {
                    DataTable crmDt = new DataTable();
                    switch (cosDt.Rows[0]["CODE_VALUE"].ToStringNullSafe())
                    {
                        case "0001":
                            #region 代理人

                            break;
                        #endregion
                        case "0002":
                            #region 單位主管
                            strSQL = @"Select crm.*, akc.MZ_KCHI As MZ_OCCC_NM, adb.MZ_NAME From C_REVIEW_MANAGEMENT crm 
                                       INNER JOIN A_DLBASE adb on adb.MZ_ID = crm.MZ_ID 
                                       INNER JOIN A_KTYPE akc on akc.MZ_KTYPE='26' And akc.MZ_KCODE = crm.MZ_OCCC 
                                       Where crm.REVIEW_LEVEL=@REVIEW_LEVEL ";
                            parameters = new List<SqlParameter>() { new SqlParameter("REVIEW_LEVEL", query.REVIEW_LEVEL) };
                            if (!string.IsNullOrEmpty(query.MZ_AD))
                            {
                                strSQL += "And crm.MZ_EXAD=@MZ_AD ";
                                parameters.Add(new SqlParameter("MZ_AD", query.MZ_AD));
                            }
                            if (!string.IsNullOrEmpty(query.MZ_UNIT))
                            {
                                strSQL += "And crm.MZ_EXUNIT=@MZ_UNIT ";
                                parameters.Add(new SqlParameter("MZ_UNIT", query.MZ_UNIT));
                            }
                            //是否為最高管理者
                            if (query.isTOP_MANAGER == true)
                            {
                                strSQL += " and crm.TOP_MANAGER='Y' ";
                            }

                            crmDt = o_DBFactory.ABC_toTest.Create_Table(strSQL, parameters);

                            break;
                        #endregion
                        default:
                            #region 其他流程
                            strSQL = @"
                                    Select crm.*, cos.SN SCHEDULE_SN 
                                    ,akc.MZ_KCHI As MZ_OCCC_NM /*職稱*/
                                    , adb.MZ_NAME /*姓名*/
                                    From C_REVIEW_MANAGEMENT crm 
                                    left join C_OVERTIME_SCHEDULE cos on cos.SCHEDULE_TYPE=@SCHEDULE_TYPE And cos.SORT=@SORT
                                    left JOIN A_DLBASE adb on adb.MZ_ID = crm.MZ_ID 
                                    left JOIN A_KTYPE akc on akc.MZ_KTYPE='26' And akc.MZ_KCODE = crm.MZ_OCCC 
                                    Where crm.REVIEW_LEVEL=@REVIEW_LEVEL 
                                        ";
                            parameters = new List<SqlParameter>()
                            {
                                new SqlParameter("REVIEW_LEVEL", query.REVIEW_LEVEL),
                                new SqlParameter("SCHEDULE_TYPE", query.OVERTIME_TYPE),
                                new SqlParameter("SORT", query.SCHEDULE_SORT)
                            };

                            List<string> strWhere = new List<string>();
                            for (int i = 0; i < cosDt.Rows.Count; i++)
                            {
                                string tmp = "1=1 ";
                                if (string.IsNullOrEmpty(cosDt.Rows[i]["MZ_EXAD"].ToStringNullSafe()))
                                {
                                    tmp += string.Format("And crm.MZ_EXAD=@MZ_AD_{0} ", i);
                                    parameters.Add(new SqlParameter("MZ_AD_" + i, query.MZ_AD));
                                }
                                else
                                {
                                    tmp += string.Format("And crm.MZ_EXAD=@MZ_EXAD_{0} ", i);
                                    parameters.Add(new SqlParameter("MZ_EXAD_" + i, cosDt.Rows[i]["MZ_EXAD"].ToStringNullSafe()));
                                }
                                //應承辦人要求，警員間會互相支援故將此條件移除
                                //if (string.IsNullOrEmpty(cosDt.Rows[i]["MZ_EXUNIT"].ToStringNullSafe()))
                                //{
                                //    tmp += string.Format("And crm.MZ_EXUNIT=@MZ_UNIT_{0} ", i);
                                //    parameters.Add(new SqlParameter("MZ_UNIT_" + i, query.MZ_UNIT));
                                //}
                                //else
                                //{
                                //    tmp += string.Format("And crm.MZ_EXUNIT=@MZ_EXUNIT_{0} ", i);
                                //    parameters.Add(new SqlParameter("MZ_EXUNIT_" + i, cosDt.Rows[i]["MZ_EXUNIT"].ToStringNullSafe()));
                                //}
                                strWhere.Add(string.Format("({0})", tmp));
                            }

                            strSQL += string.Format("And ({0}) ", string.Join("Or", strWhere.ToArray()));

                            crmDt = o_DBFactory.ABC_toTest.Create_Table(strSQL, parameters);

                            break;
                            #endregion

                    }

                    //LogModel.saveLog("OW", "S", strSQL, parameters, query.TPMFION, "取得加班線上簽核人員資料。");

                    return crmDt;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                //LogModel.saveLog("OW", "S", ex.Message, new List<SqlParameter>(), query.TPMFION, "取得加班線上簽核人員資料異常。");
                throw ex;
            }
        }

        /// <summary>
        /// 根據ID,判斷此人是否在C_REVIEW_MANAGEMENT被設定為單位主管?
        /// </summary>
        /// <param name="MZ_ID"></param>
        /// <returns></returns>
        public static string Get_TOP_MANAGER(string MZ_ID)
        {
            MZ_ID = MZ_ID.Filter_SQLInjection();
            string SQL = @"
select TOP_MANAGER from C_REVIEW_MANAGEMENT where MZ_ID='" + MZ_ID + @"'
";
            return o_DBFactory.ABC_toTest.Get_First_Field(SQL, new List<SqlParameter>());
        }

        
        /// <summary>
        /// 取得線上簽核功能中,可以再陳核的人員資料,針對Online_work.aspx功能
        /// </summary>
        /// <param name="exad">機關</param>
        /// <param name="exunit">單位</param>
        /// <param name="REVIEW_LEVEL">限定要抓哪個等級的人員,0就不限定,
        /// 大部分都是0,只有加班原本設計為2,也就是抓出主管
        /// 感覺可能只是因為當初的設計上不統一導致的?應該不太需要此條件才是
        /// </param>
        /// <param name="MZ_ID">目前操作者</param>
        /// <param name="isToUpper">是否要抓取上層單位</param>
        /// <returns></returns>
        public static DataTable Get_REVIEW_MANAGEMENT_forOnline_work(string exad, string exunit, int REVIEW_LEVEL, string MZ_ID , bool isToUpper)
        {

            string SQL = @"
SELECT c_review_management.sn,
       CASE
         WHEN review_level = '1' THEN '承辦人'
         ELSE (SELECT mz_kchi
               FROM   a_ktype
               WHERE  mz_ktype = '26'
                      AND mz_kcode = c_review_management.mz_occc)
       END AS MZ_OCCC,
       a_dlbase.mz_name,a_dlbase.MZ_ID
FROM   c_review_management,
       a_dlbase
WHERE  a_dlbase.mz_id = c_review_management.mz_id
        --不要陳給自己
        AND c_review_management.MZ_ID<>'" + MZ_ID + @"'
        --一定是同分局
       AND c_review_management.mz_exad = '" + exad + @"'
        --這邊是附加的條件,
        #UNIT_OR_REVIEW_LEVEL#
";
            if (REVIEW_LEVEL > 0)
            {
                SQL = SQL + @"
        AND REVIEW_LEVEL=" + REVIEW_LEVEL + @"
";
            }

            //是否為單位最高主管?
            string TOP_MANAGER = Get_TOP_MANAGER(MZ_ID);
            //根據 機關代碼 是否為最高管理者 是否第一層陳核,串出需要的OR條件
            string SQL_OR = _Get_SQL_Upper_REVIEW_MANAGEMENT(exad, exunit, TOP_MANAGER, isToUpper);
            if (SQL_OR != "")
            {
                SQL_OR = @"
                AND ( 1=0 --為了方便後面拼接or指令
                " + SQL_OR + @"
                )
";
            }
            SQL = SQL.Replace("#UNIT_OR_REVIEW_LEVEL#", SQL_OR);

            return o_DBFactory.ABC_toTest.Create_Table(SQL, "get");
        }

        /// <summary>
        /// 根據 機關代碼 是否為最高管理者 是否第一層陳核,串出需要的OR條件
        /// </summary>
        /// <param name="exad"></param>
        /// <param name="TOP_MANAGER"></param>
        /// <param name="isToUpper"></param>
        /// <returns></returns>
        public static string _Get_SQL_Upper_REVIEW_MANAGEMENT(string exad, string exunit, string TOP_MANAGER, bool isToUpper)
        {
            /*
                -A:抓"第一層陳核"人員:
                     -一律陳核給各長官科室或差勤承辦人
                -B:抓"陳核"人員
                     -預設抓出同單位人員
                     -如果是大隊,追加原本的特殊抓法
                     -如果是單位最高主管,再追加可陳核給各長官科室或差勤承辦人
             */
            string SQL_OR_toUpper = @"
--新增對象為相同機關(MZ_EXAD)的0326(局長室)、0327(副局長室)、0002(主秘室)、0341(大隊長室)、0354(副大隊長室)、0328(分局長室)、0329(副分局長室)、0334(隊長室)、0335	副隊長室
  OR C_REVIEW_MANAGEMENT.mz_exunit in('0326','0327','0002','0341','0354','0328','0329','0334','0335')
--及相同MZ_EZAD內 REVIEW_LEVEL=4(差勤承辦人)的人
  OR C_REVIEW_MANAGEMENT.REVIEW_LEVEL='4'
";
            string SQL_OR = @"";
            //-A:抓第一層決行人員:
            if (isToUpper)
            {   //後面不動
                return SQL_OR_toUpper;
            }
            //-B:抓"陳核"人員
            SQL_OR += @"
                    --可以陳核給自己單位
                    OR c_review_management.mz_exunit = '" + exunit + @"'
                    ";

            if (TOP_MANAGER == "Y")
            {
                //單位最高主管,要追加陳給上層單位,不然同單位沒人可陳了
                SQL_OR += SQL_OR_toUpper;
            }
            //如果是人事單位,則兩者都抓,因為分局和大隊這邊不太統一
            //分局有 PAY0 人事室  大隊則是 PAZ0 人事管理員
            if (exunit == "PAY0"|| exunit == "PAZ0")
            {
                //PAY0 人事室
                //PAZ0 人事管理員
                SQL_OR += @"
                    OR C_REVIEW_MANAGEMENT.MZ_EXUNIT in ('PAY0', 'PAZ0')
                    ";
            }
            // sam 20200504 應北市警 資訊室要求 偉欣資訊  
            // exad 382130500C  新北市政府警察局婦幼警察隊
            // 如果是上述單位的申請,改用 '0335'隊長室,'0334'副隊長室,'0302'隊本部
            if (exad == "382130500C")
            {
                SQL_OR += @"
                    OR C_REVIEW_MANAGEMENT.MZ_EXUNIT in ('0335', '0334', '0302')
                    ";
            }
            /*
        * 少年隊(MZ_EXAD = ‘382135000C’)底下所有單位的請假單，
        * 電子流程的「第一層決行」、「陳核」，除既有對象以外，
        * 增加單位為人事管理員且身分為差勤承辦人(人事室)的對象
        * '0335'隊長室,'0334'副隊長室,'0302'隊本部,'PAZ0'人事室
       */
            if ((exad == "382135000C"))
            {
                SQL_OR += @"
                    --改用 '0335'隊長室,'0334'副隊長室,'0302'隊本部 的所有人員
                    OR C_REVIEW_MANAGEMENT.MZ_EXUNIT in ('0335', '0334', '0302')
                    --或者 'PAZ0'人事室 的 差勤承辦人(Review_LEVEL=4)
                    OR (C_REVIEW_MANAGEMENT.MZ_EXUNIT in ('PAZ0') and Review_LEVEL=4)
                    ";
            }
            if (exad == "382130100C" || exad == "382130300C")//20210521 保大新增人事管理-13.差勤線上簽核的流程 差勤承辦人(REVIEW_LEVEL='4')+副大隊長+大隊長('1403','1404') + 第二中隊所有單位主管(C_REVIEW_MANAGEMENT.mz_exunit='0174')
            {
                //20210618  交通警察大隊 新增 同 保大
                //temp = o_DBFactory.ABC_toTest.Create_Table("SELECT c_review_management.sn,CASE WHEN REVIEW_LEVEL='1' THEN '承辦人' WHEN REVIEW_LEVEL='2' THEN '單位主管' WHEN REVIEW_LEVEL='4' THEN '差勤承辦人'  ELSE (SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='26' AND MZ_KCODE=C_REVIEW_MANAGEMENT.MZ_OCCC) END AS MZ_OCCC,a_dlbase.mz_name FROM c_review_management,a_dlbase WHERE A_DLBASE.MZ_ID = C_REVIEW_MANAGEMENT.MZ_ID AND C_REVIEW_MANAGEMENT.MZ_EXAD = '" + exad + "' AND (C_REVIEW_MANAGEMENT.mz_exunit='0174'  OR  REVIEW_LEVEL='4' OR c_review_management.MZ_OCCC IN ('1403','1404') ) ORDER BY C_REVIEW_MANAGEMENT.MZ_ID", "get");
                SQL_OR += @"
                    OR  REVIEW_LEVEL='4' 
                    OR C_REVIEW_MANAGEMENT.mz_exunit='PAY0' 
                    OR c_review_management.MZ_OCCC IN ('1403','1404') 
                ";
            }
            return SQL_OR;
        }

        /// <summary>
        /// 取得目前流程設定的SN,加班簽核歷程資料需要紀錄之
        /// </summary>
        /// <param name="Sort">第幾關</param>
        /// <param name="SCHEDULE_TYPE">代碼</param>
        /// <returns></returns>
        public string Get_OVERTIME_SCHEDULE_SN(int Sort, string SCHEDULE_TYPE)
        {
            string SQL = @"
            Select SN 
            From C_OVERTIME_SCHEDULE 
            WHERE Sort=@Sort AND SCHEDULE_TYPE=@SCHEDULE_TYPE
";
            List<SqlParameter> parameters = new List<SqlParameter>()
                {
                    new SqlParameter(":SCHEDULE_TYPE", SCHEDULE_TYPE),
                    new SqlParameter(":SORT", Sort)
                };
            return o_DBFactory.ABC_toTest.Get_First_Field(SQL, parameters);
        }

        /// <summary>
        /// 檢查是否為線上簽核人員權限
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public bool CheckReviewManagement(Online_work_Query query)
        {
            try
            {
                bool result = false;
                if (string.IsNullOrEmpty(query.MZ_AD) || string.IsNullOrEmpty(query.REVIEW_LEVEL) || string.IsNullOrEmpty(query.PerformReviewID))
                {
                    throw new Exception("無機關、ID、簽核層級資料!");
                }

                string strSQL = @"Select COUNT(*) From C_REVIEW_MANAGEMENT Where MZ_EXAD=@MZ_AD And MZ_ID=@PerformReviewID And REVIEW_LEVEL in (" + query.REVIEW_LEVEL + ") ";
                List<SqlParameter> parameters = new List<SqlParameter>()
                {
                    new SqlParameter("MZ_AD", query.MZ_AD),
                    new SqlParameter("PerformReviewID", query.PerformReviewID),
                    //new SqlParameter("REVIEW_LEVEL", query.REVIEW_LEVEL)
                };

                if (o_DBFactory.ABC_toTest.Get_First_Field(strSQL, parameters) != "0")
                {
                    result = true;
                }
                //LogModel.saveLog("OW", "S", strSQL, parameters, query.TPMFION, "檢查線上簽核人員權限。");

                return result;
            }
            catch (Exception ex)
            {
                //LogModel.saveLog("OW", "S", ex.Message, new List<SqlParameter>(), query.TPMFION, "檢查線上簽核人員權限異常。");
                throw ex;
            }
        }

        /// <summary>
        /// 加班流程資料儲存
        /// </summary>
        /// <param name="models"></param>
        /// <param name="TPMFION"></param>
        /// <returns></returns>
        public bool C_OVERTIME_HISTORY_Save(List<C_OVERTIME_HISTORY_Model> models, string TPMFION = "")
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
                        throw new Exception("該流程資料無對應加班單編號。");
                    }

                    if (item.O_SN != 0)
                    {
                        //更新流程資料
                        strSQL = @"Update C_OVERTIME_HISTORY Set {0} 
                                    Where O_SN=@O_SN And OVERTIME_SN=@OVERTIME_SN ";
                        parameters = new List<SqlParameter>()
                        {
                            new SqlParameter("O_SN", item.O_SN),
                            new SqlParameter("OVERTIME_SN", item.OVERTIME_SN)
                        };
                        #region 需要更新欄位
                        List<string> upColumn = new List<string>();
                        if (!string.IsNullOrEmpty(item.REVIEW_ID))
                        {
                            upColumn.Add("REVIEW_ID=@REVIEW_ID ");
                            parameters.Add(new SqlParameter("REVIEW_ID", item.REVIEW_ID));
                        }
                        if (!string.IsNullOrEmpty(item.LETTER_DATE))
                        {
                            upColumn.Add("LETTER_DATE=@LETTER_DATE ");
                            parameters.Add(new SqlParameter("LETTER_DATE", item.LETTER_DATE));
                        }

                        if (!string.IsNullOrEmpty(item.OVERTIME_SCHEDULE_SN.ToString()))
                        {
                            upColumn.Add("OVERTIME_SCHEDULE_SN=@OVERTIME_SCHEDULE_SN ");
                            parameters.Add(new SqlParameter("OVERTIME_SCHEDULE_SN", item.OVERTIME_SCHEDULE_SN));
                        }

                        if (!string.IsNullOrEmpty(item.LETTER_TIME))
                        {
                            upColumn.Add("LETTER_TIME=@LETTER_TIME ");
                            parameters.Add(new SqlParameter("LETTER_TIME", item.LETTER_TIME));
                        }

                        if (!string.IsNullOrEmpty(item.OVERTIME_TYPE))
                        {
                            upColumn.Add("OVERTIME_TYPE=@OVERTIME_TYPE ");
                            parameters.Add(new SqlParameter("OVERTIME_TYPE", item.OVERTIME_TYPE));
                        }

                        if (!string.IsNullOrEmpty(item.SEND_ID))
                        {
                            upColumn.Add("SEND_ID=@SEND_ID ");
                            parameters.Add(new SqlParameter("SEND_ID", item.SEND_ID));
                        }

                        if (!string.IsNullOrEmpty(item.PROCESS_DATE))
                        {
                            upColumn.Add("PROCESS_DATE=@PROCESS_DATE ");
                            parameters.Add(new SqlParameter("PROCESS_DATE", item.PROCESS_DATE));
                        }
                        if (!string.IsNullOrEmpty(item.REVIEW_MESSAGE))
                        {
                            upColumn.Add("REVIEW_MESSAGE=@REVIEW_MESSAGE ");
                            parameters.Add(new SqlParameter("REVIEW_MESSAGE", item.REVIEW_MESSAGE));
                        }
                        if (!string.IsNullOrEmpty(item.PROCESS_TIME))
                        {
                            upColumn.Add("PROCESS_TIME=@PROCESS_TIME ");
                            parameters.Add(new SqlParameter("PROCESS_TIME", item.PROCESS_TIME));
                        }
                        if (!string.IsNullOrEmpty(item.PROCESS_STATUS))
                        {
                            upColumn.Add("PROCESS_STATUS=@PROCESS_STATUS ");
                            parameters.Add(new SqlParameter("PROCESS_STATUS", item.PROCESS_STATUS));
                        }
                        #endregion

                        strSQL = string.Format(strSQL, string.Join(",", upColumn.ToArray()));
                    }
                    else
                    {
                        //新增流程資料
                        strSQL = @"INSERT INTO C_OVERTIME_HISTORY 
                                    (O_SN, OVERTIME_SN, REVIEW_ID, LETTER_DATE, OVERTIME_SCHEDULE_SN, LETTER_TIME, PROCESS_STATUS, OVERTIME_TYPE, SEND_ID) 
                                    VALUES 
                                    ( NEXT VALUE FOR dbo.C_OVERTIME_HISTORY_SN, @OVERTIME_SN, @REVIEW_ID, @LETTER_DATE, @OVERTIME_SCHEDULE_SN, @LETTER_TIME, @PROCESS_STATUS, @OVERTIME_TYPE, @SEND_ID) ";
                        parameters = new List<SqlParameter>()
                        {
                            new SqlParameter("OVERTIME_SN", item.OVERTIME_SN),
                            new SqlParameter("REVIEW_ID", item.REVIEW_ID),
                            new SqlParameter("LETTER_DATE", item.LETTER_DATE),
                            new SqlParameter("OVERTIME_SCHEDULE_SN", item.OVERTIME_SCHEDULE_SN),
                            new SqlParameter("LETTER_TIME", item.LETTER_TIME),
                            new SqlParameter("PROCESS_STATUS", item.PROCESS_STATUS),
                            new SqlParameter("OVERTIME_TYPE", item.OVERTIME_TYPE),
                            new SqlParameter("SEND_ID", item.SEND_ID)
                        };
                    }

                    otf.MultiSql_Add(strSQL, parameters.ToArray());
                    //LogModel.saveLog("OW", strSQL.IndexOf("Update") > 0 ? "U" : "A", strSQL, parameters, TPMFION, "加班流程資料儲存。");
                }

                otf.MultiSQL_Exec();

                return true;
            }
            catch (Exception ex)
            {
                //LogModel.saveLog("OW", "UA", ex.Message, new List<SqlParameter>(), TPMFION, "加班流程資料儲存異常。");
                throw ex;
            }
        }
        /// <summary>
        /// 加班流程資料儲存
        /// </summary>
        /// <param name="model"></param>
        /// <param name="TPMFION"></param>
        /// <returns></returns>
        public bool C_OVERTIME_HISTORY_Save(C_OVERTIME_HISTORY_Model model, string TPMFION = "")
        {
            return C_OVERTIME_HISTORY_Save(new List<C_OVERTIME_HISTORY_Model>() { model }, TPMFION);
        }
        /// <summary>
        /// 加班流程資料刪除
        /// </summary>
        /// <param name="model"></param>
        /// <param name="TPMFION"></param>
        /// <returns></returns>
        public bool C_OVERTIME_HISTORY_Delete(C_OVERTIME_HISTORY_Model model, string TPMFION = "")
        {
            try
            {
                string strSQL = string.Empty;
                List<SqlParameter> parameters;

                if (model.O_SN == 0 && model.OVERTIME_SN == 0)
                {
                    return false;
                }

                if (model.O_SN != 0)
                {
                    //刪除單筆流程
                    strSQL = @"DELETE FROM C_OVERTIME_HISTORY Where O_SN=@O_SN ";
                    parameters = new List<SqlParameter>()
                    {
                        new SqlParameter("O_SN", model.O_SN)
                    };
                }
                else if (model.OVERTIME_SN != 0)
                {
                    //刪除整批加班流程
                    strSQL = @"DELETE FROM C_OVERTIME_HISTORY Where OVERTIME_SN=@OVERTIME_SN ";
                    parameters = new List<SqlParameter>()
                    {
                        new SqlParameter("OVERTIME_SN", model.OVERTIME_SN)
                    };
                }
                else
                {
                    return false;
                }

                if (o_DBFactory.ABC_toTest.DealCommandLog(strSQL, parameters))
                {
                    LogModel.saveLog("OW", "D", strSQL, parameters.ToArray(), TPMFION, "加班流程資料刪除。");
                }
                else
                {
                    LogModel.saveLog("OW", "D", strSQL, parameters.ToArray(), TPMFION, "加班流程資料刪除失敗。");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                //LogModel.saveLog("OW", "D", ex.Message, new List<SqlParameter>(), TPMFION, "加班流程資料刪除異常。");
                throw ex;
            }
        }


        #region 銷假相關
        /// <summary>
        /// 取得可抽回銷假資料
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DataTable GetReCHANGEH(Online_work_Query query)
        {
            try
            {
                string strSQL = string.Empty;
                List<SqlParameter> parameters = new List<SqlParameter>();

                if (string.IsNullOrEmpty(query.SendID))
                {
                    return null;
                }

                strSQL = @"Select CD01.*,CCDH.*,CDC.MZ_CNAME FROM C_CHANGE_DLTB01_HISTORY CCDH
                            INNER JOIN C_DLTB01 CD01 ON CCDH.DLTB01_SN= CD01.MZ_DLTB01_SN 
                            INNER JOIN C_DLCODE CDC ON CCDH.LEAVE_SN = CDC.MZ_CODE 
                            WHERE CCDH.MZ_MID=@MZ_MID AND CCDH.LEAVE_SCHEDULE_SN !=1 AND CCDH.PROCESS_DATE IS NULL AND CCDH.RETURN_FLAG IS NULL ";
                parameters = new List<SqlParameter>()
                {
                    new SqlParameter("MZ_MID", query.SendID)
                };

                DataTable dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, parameters);

                return dt;
            }
            catch (Exception ex)
            {
                LogModel.saveLog("OW", "S", ex.Message, new List<SqlParameter>(), query.TPMFION, "查詢可抽回銷假資料異常。");
                throw ex;
            }
        }
        /// <summary>
        /// 取得銷假簽核歷程資料
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DataTable GetCHANGEHistory(Online_work_Query query)
        {
            try
            {
                string strSQL = string.Empty;
                List<SqlParameter> parameters = new List<SqlParameter>();

                if (string.IsNullOrEmpty(query.DLTB01_SN))
                {
                    return null;
                }

                strSQL = @"SELECT CCDH.SN, CCDH.PROCESS_DATE, CCDH.PROCESS_TIME, CCDH.REVIEW_MESSAGE, CCDH.REVIEW_ID, 
                                CS.C_STATUS_NAME, CD01.MZ_FILE, ADB.MZ_NAME, ADB.MZ_EXAD, ADB.MZ_EXUNIT, ADB.MZ_OCCC 
                           FROM C_CHANGE_DLTB01_HISTORY CCDH
                           INNER JOIN C_STATUS CS ON CCDH.PROCESS_STATUS = CS.C_STATUS_SN 
                           INNER JOIN C_DLTB01 CD01 ON CCDH.DLTB01_SN = CD01.MZ_DLTB01_SN 
                           INNER JOIN A_DLBASE ADB ON CCDH.REVIEW_ID = ADB.MZ_ID 
                           WHERE CCDH.DLTB01_SN =@DLTB01_SN 
                           ORDER BY CCDH.SN ";
                parameters = new List<SqlParameter>()
                {
                    new SqlParameter("DLTB01_SN", query.DLTB01_SN)
                };

                DataTable dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, parameters);

                return dt;
            }
            catch (Exception ex)
            {
                LogModel.saveLog("OW", "S", ex.Message, new List<SqlParameter>(), query.TPMFION, "查詢銷假簽核歷程資料異常。");
                throw ex;
            }
        }
        #endregion
    }
}