using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace TPPDDB._2_salary
{
    public class SalarySole
    {
       

        /// <summary>
        /// 查詢項目 - 所得格式代碼
        /// </summary>
        /// <param name="strID">項目主編號</param>
        /// <returns></returns>
        public static string strTAXES_ID(string strID)
        {
            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn.Open();
                try
                {
                    string strSQL = "SELECT TAXES_ID FROM B_SOLEITEM WHERE ID = '" + strID + "' AND TAXES_YESNO = 'Y'";
                    SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
                    DataTable dt = new DataTable();
                    dt.Load(Selectcmd.ExecuteReader());
                    if (dt.Rows.Count == 1)
                    {
                        return dt.Rows[0]["TAXES_ID"].ToString();
                    }
                    else
                    { return ""; }
                }
                catch { return ""; }
                finally { Selectconn.Close();
                //XX2013/06/18 
                Selectconn.Dispose();
                }
            }
        }

        
        /// <summary>
        /// 查詢是否納入所得
        /// </summary>
        /// <param name="strID">所得格式主編號</param>
        /// <returns></returns>
        public static bool boolTAXES_YESNO(string strID)
        {
            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn.Open();
                try
                {
                    string strSQL = "SELECT TAXES_YESNO FROM B_SOLEITEM WHERE ID = '" + strID + "' AND TAXES_YESNO = 'Y'";
                    SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
                    DataTable dt = new DataTable();
                    dt.Load(Selectcmd.ExecuteReader());
                    if (dt.Rows.Count == 1)
                    {
                        return true;
                    }
                    else
                    { return false; }
                }
                catch { return false; }
                finally { Selectconn.Close();
                //XX2013/06/18 
                Selectconn.Dispose();
                }
            }
        }

        

        /// <summary>
        /// 查詢單一發放主編號
        /// </summary>
        /// <param name="strIDCARD">身分證號</param>
        /// <param name="strMZ_NAME">姓名</param>
        /// <param name="strMZ_POLNO">員工編號</param>
        /// <param name="strDA_INOUT_GROUP">IN入帳日期，OUT退回日期</param>
        /// <param name="strDA">日期</param>
        /// <param name="strCASEID">案號</param>
        /// <param name="strNUM">項目代碼</param>
        /// <param name="strTAXES_ID1">所得附加格式</param>
        /// <returns></returns>
        public static int intSole_S_SIND_Data(string strIDCARD, string strMZ_NAME, string strMZ_POLNO, string strDA_INOUT_GROUP, string strDA, string strCASEID, string strNUM, string strTAXES_ID1)
        {
            string strTAXES_ID1_Data = "";
            if (strTAXES_ID1 == "")
            {
                strTAXES_ID1_Data = "AND TAXES_ID1 IS NULL";
            }
            else
            {
                strTAXES_ID1_Data = "AND TAXES_ID1 = '" + strTAXES_ID1 + "'";
            }
            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn.Open();
                try
                {
                    string strSQL = "SELECT S_SNID FROM B_SOLE WHERE IDCARD = '" + strIDCARD + "' AND MZ_NAME = '" + strMZ_NAME + "' AND MZ_POLNO = '" + strMZ_POLNO + "' AND DA_INOUT_GROUP = '" + strDA_INOUT_GROUP + "' AND DA = '" + strDA + "' AND CASEID = '" + strCASEID + "' AND NUM = '" + strNUM + "' " + strTAXES_ID1_Data + "";
                    SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
                    DataTable dt = new DataTable();
                    dt.Load(Selectcmd.ExecuteReader());
                    if (dt.Rows.Count == 1)
                    {
                        return int.Parse(dt.Rows[0]["S_SNID"].ToString());
                    }
                    else
                    { return 0; }
                }
                catch { throw; }
                finally { Selectconn.Close();
                //XX2013/06/18 
                Selectconn.Dispose();
                }
            }
        }

        

        public static bool boolSole_Create(string strIDCARD, string strMZ_POLNO, string strPAY_AD, string strMZ_NAME, string strMZ_OCCC, string strMZ_SRANK, string strMZ_SLVC, string strMZ_SPT, string strMZ_UNIT, string strLOCKDB, string strDA_INOUT_GROUP, string strDA, string strCASEID, string strNUM, string strTAXES_ID, string strTAXES_ID1, int intPAY, string strTAXFLAG, int intTAX, int intPAY1, int intPAY2, int intPAY3, string strNOTE)
        {
            using (SqlConnection InsertConn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                InsertConn.Open();
                try
                {
                    string InsertString = "INSERT INTO B_SOLE (S_SNID, IDCARD, MZ_POLNO, PAY_AD, MZ_NAME, MZ_OCCC, MZ_SRANK, MZ_SLVC, MZ_SPT, MZ_UNIT, LOCKDB, DA_INOUT_GROUP, DA, CASEID, NUM, TAXES_ID, TAXES_ID1, PAY, TAXFLAG, TAX, PAY1, PAY2, PAY3, NOTE, CREATEDATE) "
                    + " VALUES ( NEXT VALUE FOR dbo.B_SOLE_SN, '" + strIDCARD + "', '" + strMZ_POLNO + "', '" + strPAY_AD + "', '" + strMZ_NAME + "', '" + strMZ_OCCC + "', '" + strMZ_SRANK + "', '" + strMZ_SLVC + "', '" + strMZ_SPT + "', '" + strMZ_UNIT + "', '" + strLOCKDB + "', '" + strDA_INOUT_GROUP + "', '" + strDA + "', '" + strCASEID + "', '" + strNUM + "', '" + strTAXES_ID + "', '" + strTAXES_ID1 + "', '" + intPAY + "', '" + strTAXFLAG + "', '" + intTAX + "', '" + intPAY1 + "', '" + intPAY2 + "', '" + intPAY3 + "', '" + strNOTE + "', @CREATEDATE) ";

                    SqlCommand cmd = new SqlCommand(InsertString, InsertConn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("CREATEDATE", SqlDbType.DateTime).Value = DateTime.Now;
                    cmd.ExecuteNonQuery();

                    return true;
                }
                catch { return false; }
                finally
                { InsertConn.Close();
                //XX2013/06/18 
                InsertConn.Dispose();
                }
            }
        }

       

        public static bool boolSole_Update(int intS_SNID, string strIDCARD, string strMZ_POLNO, string strPAY_AD, string strMZ_NAME, string strMZ_OCCC, string strMZ_SRANK, string strMZ_SLVC, string strMZ_SPT, string strMZ_UNIT, string strLOCKDB, string strDA_INOUT_GROUP, string strDA, string strCASEID, string strNUM, string strTAXES_ID, string strTAXES_ID1, int intPAY, string strTAXFLAG, int intTAX, int intPAY1, int intPAY2, int intPAY3, string strNOTE)
        {
            string strTAXES_ID1_Data = "";
            if (strTAXES_ID1 == "")
            {
                strTAXES_ID1_Data = "AND TAXES_ID1 IS NULL";
            }
            else
            {
                strTAXES_ID1_Data = "AND TAXES_ID1 = '" + strTAXES_ID1 + "'";
            }
            using (SqlConnection UpdateConn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                UpdateConn.Open();
                try
                {
                    string UpdateString = "UPDATE B_SOLE SET IDCARD = '" + strIDCARD + "', MZ_POLNO = '" + strMZ_POLNO + "', PAY_AD = '" + strPAY_AD + "', MZ_NAME = '" + strMZ_NAME + "', MZ_OCCC = '" + strMZ_OCCC + "', MZ_SRANK = '" + strMZ_SRANK + "', MZ_SLVC = '" + strMZ_SLVC + "', MZ_UNIT = '" + strMZ_UNIT + "', LOCKDB = '" + strLOCKDB + "', DA_INOUT_GROUP = '" + strDA_INOUT_GROUP + "', DA = '" + strDA + "', CASEID = '" + strCASEID + "', NUM = '" + strNUM + "', PAY = '" + intPAY + "', TAXFLAG = '" + strTAXFLAG + "', TAX = '" + intTAX + "', PAY1 = '" + intPAY1 + "', PAY2 = '" + intPAY2 + "', PAY3 = '" + intPAY3 + "', NOTE = '" + strNOTE + "' WHERE S_SNID = '" + intS_SNID + "'";
                    SqlCommand cmd = new SqlCommand(UpdateString, UpdateConn);
                    cmd.CommandType = CommandType.Text;

                    cmd.Connection = UpdateConn;
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch { return false; }
                finally { UpdateConn.Close();
                //XX2013/06/18 
                UpdateConn.Dispose();
                }
            }
        }

        

        /// <summary>
        /// 查詢單一發放 - 結卡旗標
        /// </summary>
        /// <param name="intS_SNID">單一發放主編號</param>
        /// <returns></returns>
        public static string str_LOCKDB_Data_Serach(int intS_SNID)
        {
            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn.Open();
                try
                {
                    string strSQL = "SELECT LOCKDB FROM B_SOLE WHERE S_SNID = '" + intS_SNID + "'";
                    SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
                    DataTable dt = new DataTable();
                    dt.Load(Selectcmd.ExecuteReader(CommandBehavior.CloseConnection));
                    if (dt.Rows.Count == 1)
                    {
                        return dt.Rows[0]["LOCKDB"].ToString();
                    }
                    else
                    {
                        return "Y";
                    }
                }
                catch { return "Y"; }
                finally { Selectconn.Close();
                //XX2013/06/18 
                Selectconn.Dispose();
                }
            }
        }

     

        




    }
}
