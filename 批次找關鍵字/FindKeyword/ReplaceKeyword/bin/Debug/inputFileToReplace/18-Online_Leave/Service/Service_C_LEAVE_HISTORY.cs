using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using TPPDDB.Online_Leave;

namespace TPPDDB._18_Online_Leave.Service
{
    public class Service_C_LEAVE_HISTORY
    {

        /// <summary>
        /// 這筆未簽核的詳細假單內容(含代理人，利用SN查詢)
        /// </summary>
        /// <param name="MZ_DLTB01_SN"></param>
        /// <returns></returns>
        public static DataTable AllSearch(string MODE, string MZ_DLTB01_SN)
        {
            string strSQL = "";
            DataTable temp = null;
            //TODO 不需要所有欄位都出來
            //0812→Dean
            if (MODE == Const_Mode.抽回差假簽核)
            {
                strSQL = @"select C_DLTB01.*,C_LEAVE_HISTORY.*,C_DLCODE.MZ_CNAME FROM C_LEAVE_HISTORY INNER JOIN 
                     C_DLTB01 ON C_LEAVE_HISTORY.DLTB01_SN= C_DLTB01.MZ_DLTB01_SN 
                     INNER JOIN C_DLCODE ON C_DLTB01.MZ_CODE = C_DLCODE.MZ_CODE 
                     WHERE  C_DLTB01.MZ_DLTB01_SN ='"
                     + MZ_DLTB01_SN + "'";
                temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "Show_gv");
                return temp;
            }
            if (MODE == Const_Mode.已核定假單)
            {
                strSQL = @"select C_DLTB01.*,C_LEAVE_HISTORY.*,C_DLCODE.MZ_CNAME FROM C_LEAVE_HISTORY INNER JOIN 
                     C_DLTB01 ON C_LEAVE_HISTORY.DLTB01_SN= C_DLTB01.MZ_DLTB01_SN 
                     INNER JOIN C_DLCODE ON C_DLTB01.MZ_CODE = C_DLCODE.MZ_CODE 
                     WHERE  C_DLTB01.MZ_DLTB01_SN ='"
                     + MZ_DLTB01_SN + "'";
                temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "Show_gv");
                return temp;
            }
            if (MODE == Const_Mode.簽核狀態)
            {
                strSQL = @"select C_DLTB01.*,C_LEAVE_HISTORY.*,C_DLCODE.MZ_CNAME FROM C_LEAVE_HISTORY INNER JOIN 
                     C_DLTB01 ON C_LEAVE_HISTORY.DLTB01_SN= C_DLTB01.MZ_DLTB01_SN 
                     INNER JOIN C_DLCODE ON C_DLTB01.MZ_CODE = C_DLCODE.MZ_CODE 
                     WHERE C_DLTB01.MZ_ID='" + SessionHelper.ADPMZ_ID
                     + "'AND C_DLTB01.MZ_DLTB01_SN ='"
                     + MZ_DLTB01_SN + "'";
                temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "Show_gv");
                return temp;
            }
            else if (MODE == Const_Mode.查詢簽核狀態)
            {
                strSQL = @"select C_DLTB01.*,C_LEAVE_HISTORY.*,C_DLCODE.MZ_CNAME FROM C_LEAVE_HISTORY INNER JOIN 
                    C_DLTB01 ON C_LEAVE_HISTORY.DLTB01_SN= C_DLTB01.MZ_DLTB01_SN 
                    INNER JOIN C_DLCODE ON C_DLTB01.MZ_CODE = C_DLCODE.MZ_CODE 
                    WHERE  C_DLTB01.MZ_DLTB01_SN ='"
                    + MZ_DLTB01_SN + "'";
                temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "Show_gv");
                return temp;
            }
            else
            {
                strSQL = @"select C_DLTB01.*,C_LEAVE_HISTORY.*,C_DLCODE.MZ_CNAME FROM C_LEAVE_HISTORY INNER JOIN 
                     C_DLTB01 ON C_LEAVE_HISTORY.DLTB01_SN= C_DLTB01.MZ_DLTB01_SN 
                     INNER JOIN C_DLCODE ON C_DLTB01.MZ_CODE = C_DLCODE.MZ_CODE 
                     WHERE C_LEAVE_HISTORY.REVIEW_ID='" + SessionHelper.ADPMZ_ID
                     + "'AND C_LEAVE_HISTORY.PROCESS_DATE IS NULL AND C_DLTB01.MZ_DLTB01_SN ='"
                     + MZ_DLTB01_SN + "'";
                temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "Show_gv");
                return temp;
            }
        }

        public static string GetCount_待人事室審核中()
        {
            //PROCESS_STATUS=4 代表審核中   LEAVE_SCHEDULE_SN=0 代表已送到人事室承辦人
            return o_DBFactory.ABC_toTest.vExecSQL(@"select count(*) from C_LEAVE_HISTORY 
                                        JOIN C_DLTB01 ON C_LEAVE_HISTORY.DLTB01_SN= C_DLTB01.MZ_DLTB01_SN 
                                        JOIN C_DLCODE ON C_DLTB01.MZ_CODE = C_DLCODE.MZ_CODE WHERE PROCESS_STATUS ='4' 
                             AND LEAVE_SCHEDULE_SN='0' AND REVIEW_ID='" + SessionHelper.ADPMZ_ID + "' AND C_DLTB01.MZ_EXAD = '" + SessionHelper.ADPMZ_EXAD + "'");

        }

        public static int GetCount_人事室審核()
        {
            //GetCount_人事室審核
            //把歷程取出 放入ViewState["Person"]裡面 (屬於人事室承辦人的)
            string SQLPerson = "select * from C_LEAVE_HISTORY WHERE PROCESS_STATUS ='4' " +
                " AND LEAVE_SCHEDULE_SN=0 AND REVIEW_ID='" + SessionHelper.ADPMZ_ID + "'";

            DataTable tempPerson = new DataTable();
            tempPerson = o_DBFactory.ABC_toTest.Create_Table(SQLPerson, "C_HISTORY_Person");


            int y = tempPerson.Rows.Count;
            return y;
        }
    }
}