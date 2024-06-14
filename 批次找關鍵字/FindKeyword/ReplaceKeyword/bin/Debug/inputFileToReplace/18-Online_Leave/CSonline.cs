using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TPPDDB.Helpers;

namespace TPPDDB.Online_Leave
{
    public class CSonline
    {
        private string TableName;

        private string MZ_ID;

        private string strSQL = "";

        DataTable dt = new DataTable();

        public CSonline(string tablename, string mz_id)
        {
            TableName = tablename;
            MZ_ID = mz_id;
        }

        /// <summary>
        /// 找指定Table有多少筆個人資料
        /// </summary>
        /// <returns></returns>
        public int getDateCount()
        {
            int count = 0;

            switch (TableName)
            {
               
                case "C_LEAVE_HISTORY"://差勤線上簽和數量 irk改
                    strSQL = @"SELECT COUNT(*) FROM C_LEAVE_HISTORY
                    right join c_dltb01 on mz_dltb01_sn=dltb01_sn   
                    WHERE REVIEW_ID='" +
                       MZ_ID + "'AND PROCESS_DATE is null AND RETURN_FLAG IS NULL "
                  + " AND LEAVE_SCHEDULE_SN > '1' order by SN";
                   
                    break;
                case "C_HISTORY_AGENTS"://代理人線上簽和數量
                    strSQL = @"select COUNT(*) FROM C_LEAVE_HISTORY INNER JOIN 
                     C_DLTB01 ON C_LEAVE_HISTORY.DLTB01_SN= C_DLTB01.MZ_DLTB01_SN 
                     INNER JOIN C_DLCODE ON C_LEAVE_HISTORY.LEAVE_SN = C_DLCODE.MZ_CODE 
                     WHERE C_LEAVE_HISTORY.REVIEW_ID ='" + MZ_ID
                     + "'AND C_LEAVE_HISTORY.PROCESS_DATE IS NULL AND C_LEAVE_HISTORY.LEAVE_SCHEDULE_SN = '1' AND RETURN_FLAG IS NULL "
                     + " order by C_LEAVE_HISTORY.SN";
                 
                    break;
                case "reSearch"://差勤線上抽回數量
                    strSQL = @"select COUNT(*) FROM C_LEAVE_HISTORY INNER JOIN 
                    C_DLTB01 ON C_LEAVE_HISTORY.DLTB01_SN= C_DLTB01.MZ_DLTB01_SN 
                    INNER JOIN C_DLCODE ON C_DLTB01.MZ_CODE = C_DLCODE.MZ_CODE 
                    WHERE C_LEAVE_HISTORY.MZ_MID='" + MZ_ID
                    + "' AND C_LEAVE_HISTORY.LEAVE_SCHEDULE_SN !=1 AND C_LEAVE_HISTORY.PROCESS_DATE IS NULL AND RETURN_FLAG IS NULL order by MZ_IDATE1 ";
                   

                    break;
                case "bussiness_search"://搜尋給GV的差旅費資料
                    strSQL = @"select COUNT(*) FROM C_BUSSINESSTRIP_HISTORY INNER JOIN 
                         C_DLTB01 ON C_BUSSINESSTRIP_HISTORY.DLTB01_SN= C_DLTB01.MZ_DLTB01_SN 
                         WHERE C_BUSSINESSTRIP_HISTORY.REVIEW_ID='" + MZ_ID
                         + "'AND C_BUSSINESSTRIP_HISTORY.PROCESS_DATE IS NULL AND C_BUSSINESSTRIP_HISTORY.LEAVE_SCHEDULE_SN > '1' AND RETURN_FLAG IS NULL "
                         + " order by C_BUSSINESSTRIP_HISTORY.SN";
                    
                    break;
                case "bresearch": //差旅費可抽回數量
                    strSQL = @"select COUNT(*) FROM C_BUSSINESSTRIP_HISTORY INNER JOIN 
                         C_DLTB01 ON C_BUSSINESSTRIP_HISTORY.DLTB01_SN= C_DLTB01.MZ_DLTB01_SN 
                         INNER JOIN C_DLCODE ON C_BUSSINESSTRIP_HISTORY.LEAVE_SN = C_DLCODE.MZ_CODE 
                         WHERE C_BUSSINESSTRIP_HISTORY.MZ_MID='" + MZ_ID
                         + "' AND C_BUSSINESSTRIP_HISTORY.LEAVE_SCHEDULE_SN !=1 AND C_BUSSINESSTRIP_HISTORY.PROCESS_DATE IS NULL AND RETURN_FLAG IS NULL ";
                    
                    break;
                case "change_dltb01_search"://銷假簽核數量
                    strSQL = @"select COUNT(*) FROM C_CHANGE_DLTB01_HISTORY INNER JOIN 
                         C_DLTB01 ON C_CHANGE_DLTB01_HISTORY.DLTB01_SN= C_DLTB01.MZ_DLTB01_SN 
                         WHERE C_CHANGE_DLTB01_HISTORY.REVIEW_ID='" + MZ_ID
                         + "'AND C_CHANGE_DLTB01_HISTORY.PROCESS_DATE IS NULL AND C_CHANGE_DLTB01_HISTORY.LEAVE_SCHEDULE_SN > '1' AND RETURN_FLAG IS NULL "
                         + " order by C_CHANGE_DLTB01_HISTORY.SN";
                    
                    break;
                case "RECHANGE_DLTB01_search"://銷假抽回數量
                    strSQL = @"select COUNT(*) FROM C_CHANGE_DLTB01_HISTORY 
                                INNER JOIN C_DLTB01 ON C_CHANGE_DLTB01_HISTORY.DLTB01_SN= C_DLTB01.MZ_DLTB01_SN 
                                INNER JOIN C_DLCODE ON C_CHANGE_DLTB01_HISTORY.LEAVE_SN = C_DLCODE.MZ_CODE 
                                WHERE C_CHANGE_DLTB01_HISTORY.MZ_MID='" + MZ_ID 
                                + "' AND C_CHANGE_DLTB01_HISTORY.LEAVE_SCHEDULE_SN !=1 AND C_CHANGE_DLTB01_HISTORY.PROCESS_DATE IS NULL AND RETURN_FLAG IS NULL ";
                    break;
            }
            count = int.Parse(o_DBFactory.ABC_toTest.vExecSQL(strSQL));
            return count;
        }


        public DataTable getDate()
        {
            DataTable dt = new DataTable();

            switch (TableName)
            {
                //case "C_LEAVE_HISTORY"://差勤線上簽和數量 //原始
                //    strSQL = "SELECT * FROM C_LEAVE_HISTORY  WHERE REVIEW_ID='" +
                //       MZ_ID + "'AND PROCESS_DATE is null "
                //  + " AND LEAVE_SCHEDULE_SN > '1' order by SN";

                case "C_LEAVE_HISTORY"://差勤線上簽和數量 irk改
                    strSQL = @"SELECT * FROM C_LEAVE_HISTORY
                    right join c_dltb01 on mz_dltb01_sn=dltb01_sn   
                    WHERE REVIEW_ID='" +
                       MZ_ID + "'AND PROCESS_DATE is null AND RETURN_FLAG IS NULL "
                  + " AND LEAVE_SCHEDULE_SN > '1' order by SN";
                    dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "C_LEAVE_HISTORY");
                    break;
                case "C_HISTORY_AGENTS"://代理人線上簽和數量
                    strSQL = @"select C_DLTB01.*,C_LEAVE_HISTORY.*,C_DLCODE.MZ_CNAME FROM C_LEAVE_HISTORY INNER JOIN 
                     C_DLTB01 ON C_LEAVE_HISTORY.DLTB01_SN= C_DLTB01.MZ_DLTB01_SN 
                     INNER JOIN C_DLCODE ON C_LEAVE_HISTORY.LEAVE_SN = C_DLCODE.MZ_CODE 
                     WHERE C_LEAVE_HISTORY.REVIEW_ID ='" + MZ_ID
                     + "'AND C_LEAVE_HISTORY.PROCESS_DATE IS NULL AND C_LEAVE_HISTORY.LEAVE_SCHEDULE_SN = '1' AND RETURN_FLAG IS NULL "
                     + " order by C_LEAVE_HISTORY.SN";
                    dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "C_HISTORY_AGENTS");
                    break;
                case "reSearch"://差勤線上抽回數量
                    strSQL = @"select C_DLTB01.*,C_LEAVE_HISTORY.*,C_DLCODE.MZ_CNAME FROM C_LEAVE_HISTORY INNER JOIN 
                    C_DLTB01 ON C_LEAVE_HISTORY.DLTB01_SN= C_DLTB01.MZ_DLTB01_SN 
                    INNER JOIN C_DLCODE ON C_DLTB01.MZ_CODE = C_DLCODE.MZ_CODE 
                    WHERE C_LEAVE_HISTORY.MZ_MID='" + MZ_ID
                    + "' AND C_LEAVE_HISTORY.LEAVE_SCHEDULE_SN !=1 AND C_LEAVE_HISTORY.PROCESS_DATE IS NULL AND RETURN_FLAG IS NULL order by MZ_IDATE1 ";
                    dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "Show_gv");

                    break;
                case "bussiness_search"://搜尋給GV的差旅費資料
                    strSQL = @"select C_DLTB01.*,C_BUSSINESSTRIP_HISTORY.* FROM C_BUSSINESSTRIP_HISTORY INNER JOIN 
                         C_DLTB01 ON C_BUSSINESSTRIP_HISTORY.DLTB01_SN= C_DLTB01.MZ_DLTB01_SN 
                         WHERE C_BUSSINESSTRIP_HISTORY.REVIEW_ID='" + MZ_ID
                         + "'AND C_BUSSINESSTRIP_HISTORY.PROCESS_DATE IS NULL AND C_BUSSINESSTRIP_HISTORY.LEAVE_SCHEDULE_SN > '1' AND RETURN_FLAG IS NULL "
                         + " order by C_BUSSINESSTRIP_HISTORY.SN";
                    dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "Select_gv");
                    break;
                case "bresearch": //差旅費可抽回數量
                    strSQL = @"select C_DLTB01.*,C_BUSSINESSTRIP_HISTORY.*,C_DLCODE.MZ_CNAME FROM C_BUSSINESSTRIP_HISTORY INNER JOIN 
                         C_DLTB01 ON C_BUSSINESSTRIP_HISTORY.DLTB01_SN= C_DLTB01.MZ_DLTB01_SN 
                         INNER JOIN C_DLCODE ON C_BUSSINESSTRIP_HISTORY.LEAVE_SN = C_DLCODE.MZ_CODE 
                         WHERE C_BUSSINESSTRIP_HISTORY.MZ_MID='" + MZ_ID
                         + "' AND C_BUSSINESSTRIP_HISTORY.LEAVE_SCHEDULE_SN !=1 AND C_BUSSINESSTRIP_HISTORY.PROCESS_DATE IS NULL AND RETURN_FLAG IS NULL ";
                    dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "Show_gv");
                    break;
                case "change_dltb01_search"://銷假簽核數量
                    strSQL = @"select C_DLTB01.*,C_CHANGE_DLTB01_HISTORY.* FROM C_CHANGE_DLTB01_HISTORY INNER JOIN 
                         C_DLTB01 ON C_CHANGE_DLTB01_HISTORY.DLTB01_SN= C_DLTB01.MZ_DLTB01_SN 
                         WHERE C_CHANGE_DLTB01_HISTORY.REVIEW_ID='" + MZ_ID
                         + "'AND C_CHANGE_DLTB01_HISTORY.PROCESS_DATE IS NULL AND C_CHANGE_DLTB01_HISTORY.LEAVE_SCHEDULE_SN > '1' AND RETURN_FLAG IS NULL "
                         + " order by C_CHANGE_DLTB01_HISTORY.SN";
                    dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "Select_gv");
                    break;
                case "RECHANGE_DLTB01_search"://銷假抽回數量
                    strSQL = @"select C_DLTB01.*,C_CHANGE_DLTB01_HISTORY.*,C_DLCODE.MZ_CNAME FROM C_CHANGE_DLTB01_HISTORY INNER JOIN 
                         C_DLTB01 ON C_CHANGE_DLTB01_HISTORY.DLTB01_SN= C_DLTB01.MZ_DLTB01_SN 
                         INNER JOIN C_DLCODE ON C_CHANGE_DLTB01_HISTORY.LEAVE_SN = C_DLCODE.MZ_CODE 
                         WHERE C_CHANGE_DLTB01_HISTORY.MZ_MID='" + MZ_ID
                         + "' AND C_CHANGE_DLTB01_HISTORY.LEAVE_SCHEDULE_SN !=1 AND C_CHANGE_DLTB01_HISTORY.PROCESS_DATE IS NULL AND RETURN_FLAG IS NULL ";
                    dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "Show_gv");
                    break;
            }

            return dt;
        }

        public DataTable getSearch()
        {
            switch (TableName)
            {
                    //TODO.不需要撈所有欄位出來
                case "agentsSearch": //search是此人為代理人的假單
                    strSQL = "select C_DLTB01.*,C_LEAVE_HISTORY.*,C_DLCODE.MZ_CNAME FROM C_LEAVE_HISTORY INNER JOIN "
                         + "C_DLTB01 ON C_LEAVE_HISTORY.DLTB01_SN= C_DLTB01.MZ_DLTB01_SN "
                         + "INNER JOIN C_DLCODE ON C_DLTB01.MZ_CODE = C_DLCODE.MZ_CODE "
                         + "WHERE C_LEAVE_HISTORY.REVIEW_ID='" + MZ_ID
                         + "'AND C_LEAVE_HISTORY.PROCESS_DATE IS NULL AND C_LEAVE_HISTORY.LEAVE_SCHEDULE_SN = '1' AND RETURN_FLAG IS NULL "
                         + " order by C_LEAVE_HISTORY.SN";
                    dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "Agents_gv");
                    break;
                case "doSearch"://所有此人未簽核的內容(不包含代理人)
                    strSQL = "select C_DLTB01.*,C_LEAVE_HISTORY.*,C_DLCODE.MZ_CNAME FROM C_LEAVE_HISTORY INNER JOIN "
                         + "C_DLTB01 ON C_LEAVE_HISTORY.DLTB01_SN= C_DLTB01.MZ_DLTB01_SN "
                         + "INNER JOIN C_DLCODE ON C_DLTB01.MZ_CODE = C_DLCODE.MZ_CODE "
                         + "WHERE C_LEAVE_HISTORY.REVIEW_ID='" + MZ_ID
                         + "'AND C_LEAVE_HISTORY.PROCESS_DATE IS NULL AND C_LEAVE_HISTORY.LEAVE_SCHEDULE_SN > '1' AND RETURN_FLAG IS NULL "
                         + " order by C_LEAVE_HISTORY.SN";
                    dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "Select_gv");
                    break;
                case "bussiness_check_search"://搜尋給GV的差旅費資料
                    strSQL = "select C_DLTB01.*,C_BUSSINESSTRIP_HISTORY.* FROM C_BUSSINESSTRIP_HISTORY INNER JOIN "
                         + "C_DLTB01 ON C_BUSSINESSTRIP_HISTORY.DLTB01_SN= C_DLTB01.MZ_DLTB01_SN "
                         + "WHERE C_BUSSINESSTRIP_HISTORY.REVIEW_ID='" + MZ_ID
                         + "'AND C_BUSSINESSTRIP_HISTORY.PROCESS_DATE IS NULL AND C_BUSSINESSTRIP_HISTORY.PROCESS_STATUS ='4' AND  C_BUSSINESSTRIP_HISTORY.LEAVE_SCHEDULE_SN='0' AND RETURN_FLAG IS NULL "
                         + " order by C_BUSSINESSTRIP_HISTORY.SN";
                    dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "Select_gv");

                    break;
            }

            return dt;
        }

        /// <summary>
        /// 確認簽核是否已主管決行，決定Menu不再出現數字
        /// </summary>
        /// <returns></returns>
        public int checkDecision()
        {
            int c = 0;
            DataTable dt = new DataTable();

            switch (TableName)
            {
                case "RECHANGE_DLTB01_search":
                    //取得主檔流水號
                    strSQL = string.Format(@"select DLTB01_SN FROM C_CHANGE_DLTB01_HISTORY 
                                                INNER JOIN C_DLTB01 ON C_CHANGE_DLTB01_HISTORY.DLTB01_SN= C_DLTB01.MZ_DLTB01_SN 
                                                INNER JOIN C_DLCODE ON C_CHANGE_DLTB01_HISTORY.LEAVE_SN = C_DLCODE.MZ_CODE 
                                                WHERE C_CHANGE_DLTB01_HISTORY.MZ_MID='{0}' AND C_CHANGE_DLTB01_HISTORY.LEAVE_SCHEDULE_SN !=1 AND C_CHANGE_DLTB01_HISTORY.PROCESS_DATE IS NULL AND RETURN_FLAG IS NULL ", MZ_ID);
                    dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");

                    //判斷每批歷程是否主管已決行
                    foreach (DataRow item in dt.Rows)
                    {
                        strSQL = string.Format(@"SELECT COUNT(*) FROM C_CHANGE_DLTB01_HISTORY 
                                                WHERE DLTB01_SN ='{0}' AND PROCESS_STATUS = '2'
                                                ORDER BY SN", item["DLTB01_SN"].ToStringNullSafe());
                        if (int.Parse(o_DBFactory.ABC_toTest.vExecSQL(strSQL)) == 0)
                        {
                            c++;
                        }
                    }
                    break;
            }

            return c;
        }
    }
}
