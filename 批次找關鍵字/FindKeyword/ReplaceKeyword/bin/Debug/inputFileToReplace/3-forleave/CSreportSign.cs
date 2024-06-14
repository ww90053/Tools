using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace TPPDDB._3_forleave
{
    [Serializable]
    public class CSreportSign
    {
        private string DLCODE_CODE;

        private string MZ_DLTB01_SN;

        private string strSQL;

        private DataTable dt = new DataTable();

        public CSreportSign(string dlcode_code, string mz_dltb01_sn)
        {
            DLCODE_CODE = dlcode_code;
            MZ_DLTB01_SN = mz_dltb01_sn;
        }
        //請假人寄送時間
        public string getSendTime()
        {
            string reasult = "";

            strSQL = string.Format(@"SELECT LETTER_DATE+' '+LETTER_TIME FROM C_LEAVE_HISTORY,C_LEAVE_SCHEDULE WHERE C_LEAVE_HISTORY.LEAVE_SCHEDULE_SN=C_LEAVE_SCHEDULE.SN AND DLTB01_SN={0} AND C_LEAVE_SCHEDULE.SCHEDULE_CODE='0001' ORDER BY C_LEAVE_HISTORY.SN", MZ_DLTB01_SN);

            reasult = o_DBFactory.ABC_toTest.vExecSQL(strSQL);

            return reasult;
        }
        //代理人簽核時間
        public string getRnameSign()
        {
            string reasult = "";

            strSQL = string.Format(@"SELECT PROCESS_DATE+' '+PROCESS_TIME FROM C_LEAVE_HISTORY,C_LEAVE_SCHEDULE WHERE C_LEAVE_HISTORY.LEAVE_SCHEDULE_SN=C_LEAVE_SCHEDULE.SN AND DLTB01_SN={0} AND C_LEAVE_SCHEDULE.SCHEDULE_CODE='0001' ORDER BY C_LEAVE_HISTORY.SN", MZ_DLTB01_SN);

            reasult = o_DBFactory.ABC_toTest.vExecSQL(strSQL);

            return reasult;
        }
        //主管簽核時間
        public string getMangerSign()
        {
            string reasult = "";

            strSQL = string.Format(@"SELECT (SELECT MZ_NAME FROM A_DLBASE WHERE MZ_ID=REVIEW_ID)+' '+PROCESS_DATE+' '+PROCESS_TIME FROM C_LEAVE_HISTORY,C_LEAVE_SCHEDULE WHERE C_LEAVE_HISTORY.LEAVE_SCHEDULE_SN=C_LEAVE_SCHEDULE.SN AND DLTB01_SN={0} AND C_LEAVE_SCHEDULE.SCHEDULE_CODE='0002' ORDER BY C_LEAVE_HISTORY.SN", MZ_DLTB01_SN);

            dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i == dt.Rows.Count - 1)
                    reasult += dt.Rows[i][0].ToString();
                else
                    reasult += dt.Rows[i][0].ToString() + "$N";
            }

            return reasult;
        }
        //政風室簽核時間 0006
        public string getEthicsSign()
        {
            string reasult = "";

            strSQL = string.Format(@"SELECT (SELECT MZ_NAME FROM A_DLBASE WHERE MZ_ID=REVIEW_ID)+' '+PROCESS_DATE+' '+PROCESS_TIME FROM C_LEAVE_HISTORY,C_LEAVE_SCHEDULE WHERE C_LEAVE_HISTORY.LEAVE_SCHEDULE_SN=C_LEAVE_SCHEDULE.SN AND DLTB01_SN={0} AND C_LEAVE_SCHEDULE.SCHEDULE_CODE='0006' ORDER BY C_LEAVE_HISTORY.SN", MZ_DLTB01_SN);

            dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i == dt.Rows.Count - 1)
                    reasult += dt.Rows[i][0].ToString();
                else
                    reasult += dt.Rows[i][0].ToString() + "$N";
            }

            return reasult;
        }
        //督察室簽核時間 0007 0010
        public string getInspectorsSign()
        {
            string reasult = "";

            strSQL = string.Format(@"SELECT (SELECT MZ_NAME FROM A_DLBASE WHERE MZ_ID=REVIEW_ID)+' '+PROCESS_DATE+' '+PROCESS_TIME FROM C_LEAVE_HISTORY,C_LEAVE_SCHEDULE WHERE C_LEAVE_HISTORY.LEAVE_SCHEDULE_SN=C_LEAVE_SCHEDULE.SN AND DLTB01_SN={0} AND C_LEAVE_SCHEDULE.SCHEDULE_CODE IN'(0007','0010') ORDER BY C_LEAVE_HISTORY.SN", MZ_DLTB01_SN);

            dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i == dt.Rows.Count - 1)
                    reasult += dt.Rows[i][0].ToString();
                else
                    reasult += dt.Rows[i][0].ToString() + "$N";
            }

            return reasult;
        }
        //保防室簽核時間0008 0011
        public string getSafeguardsSing()
        {
            string reasult = "";

            strSQL = string.Format(@"SELECT (SELECT MZ_NAME FROM A_DLBASE WHERE MZ_ID=REVIEW_ID)+' '+PROCESS_DATE+' '+PROCESS_TIME FROM C_LEAVE_HISTORY,C_LEAVE_SCHEDULE WHERE C_LEAVE_HISTORY.LEAVE_SCHEDULE_SN=C_LEAVE_SCHEDULE.SN AND DLTB01_SN={0} AND C_LEAVE_SCHEDULE.SCHEDULE_CODE IN ('0008','0011') ORDER BY C_LEAVE_HISTORY.SN", MZ_DLTB01_SN);

            dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i == dt.Rows.Count - 1)
                    reasult += dt.Rows[i][0].ToString();
                else
                    reasult += dt.Rows[i][0].ToString() + "$N";
            }

            return reasult;
        }
        //會計室簽核時間
        public string getAccountSing()
        {
            string reasult = "";

            strSQL = string.Format(@"SELECT (SELECT MZ_NAME FROM A_DLBASE WHERE MZ_ID=REVIEW_ID)+' '+PROCESS_DATE+' '+PROCESS_TIME FROM C_LEAVE_HISTORY,C_LEAVE_SCHEDULE WHERE C_LEAVE_HISTORY.LEAVE_SCHEDULE_SN=C_LEAVE_SCHEDULE.SN AND DLTB01_SN={0} AND C_LEAVE_SCHEDULE.SCHEDULE_CODE='0009' ORDER BY C_LEAVE_HISTORY.SN", MZ_DLTB01_SN);

            dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i == dt.Rows.Count - 1)
                    reasult += dt.Rows[i][0].ToString();
                else
                    reasult += dt.Rows[i][0].ToString() + "$N";
            }

            return reasult;
        }
        //主官簽核時間
        public string getHighLevelSign()
        {
            string reasult = "";

            return reasult;
        }
        //人事室簽核時間
        public string getPersonnelSign()
        {
            string reasult = "";

            strSQL = string.Format(@"SELECT (SELECT MZ_NAME FROM A_DLBASE WHERE MZ_ID=REVIEW_ID)+' '+PROCESS_DATE+' '+PROCESS_TIME FROM C_LEAVE_HISTORY,C_LEAVE_SCHEDULE  WHERE DLTB01_SN={0} AND C_LEAVE_HISTORY.LEAVE_SCHEDULE_SN=C_LEAVE_SCHEDULE.SN  AND C_LEAVE_SCHEDULE.SCHEDULE_CODE IN ('0003','0004') ORDER BY C_LEAVE_HISTORY.SN", MZ_DLTB01_SN);

            dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i == dt.Rows.Count - 1)
                    reasult += dt.Rows[i][0].ToString();
                else
                    reasult += dt.Rows[i][0].ToString() + "$N";
            }

            return reasult;
        }
        //檢查假別層級
        public string getLevel()
        {
            string result = "";

            strSQL = string.Format(@"SELECT COUNT(*)  FROM C_LEAVE_HISTORY WHERE LEAVE_SCHEDULE_SN<>'2' AND PROCESS_STATUS='2' AND DLTB01_SN={0} ", MZ_DLTB01_SN);

            int count;

            int.TryParse(o_DBFactory.ABC_toTest.vExecSQL(strSQL), out count);

            if (count > 0)
            {
                result = "一";
            }

            strSQL = string.Format(@"SELECT COUNT(*)  FROM C_LEAVE_HISTORY WHERE LEAVE_SCHEDULE_SN='2' AND PROCESS_STATUS='2' AND DLTB01_SN={0} ", MZ_DLTB01_SN);

            int.TryParse(o_DBFactory.ABC_toTest.vExecSQL(strSQL), out count);

            if (count > 0)
            {
                result = "二";
            }

            return result;
        }

        public string getDecisionSign()
        {
            string reasult = "";

            strSQL = string.Format(@"SELECT (SELECT MZ_NAME FROM A_DLBASE WHERE MZ_ID=REVIEW_ID)+' '+PROCESS_DATE+' '+PROCESS_TIME FROM C_LEAVE_HISTORY,C_LEAVE_SCHEDULE WHERE C_LEAVE_HISTORY.LEAVE_SCHEDULE_SN=C_LEAVE_SCHEDULE.SN AND DLTB01_SN={0} AND PROCESS_STATUS='2' ORDER BY C_LEAVE_HISTORY.SN", MZ_DLTB01_SN);

            dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i == dt.Rows.Count - 1)
                    reasult += dt.Rows[i][0].ToString();
                else
                    reasult += dt.Rows[i][0].ToString() + "$N";
            }

            return reasult;
        }

        public string getDecisionMeno()
        {
            string reasult = "";

            strSQL = string.Format(@"SELECT REVIEW_MESSAGE FROM C_LEAVE_HISTORY,C_LEAVE_SCHEDULE WHERE C_LEAVE_HISTORY.LEAVE_SCHEDULE_SN=C_LEAVE_SCHEDULE.SN AND DLTB01_SN={0} AND PROCESS_STATUS='2' ORDER BY C_LEAVE_HISTORY.SN", MZ_DLTB01_SN);

            dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i == dt.Rows.Count - 1)
                    reasult += dt.Rows[i][0].ToString();
                else
                    reasult += dt.Rows[i][0].ToString() + "$N";
            }

            return reasult;
        }

        //檢查是否已決行
        public bool getReturn()
        {
            bool reasult = false;

            strSQL = string.Format(@"SELECT COUNT(*) FROM C_LEAVE_HISTORY WHERE PROCESS_STATUS='2' AND DLTB01_SN={0} ", MZ_DLTB01_SN);

            int count;

            int.TryParse(o_DBFactory.ABC_toTest.vExecSQL(strSQL), out count);

            if (count > 0)
            {
                reasult = true;
            }

            return reasult;
        }
    }
}
