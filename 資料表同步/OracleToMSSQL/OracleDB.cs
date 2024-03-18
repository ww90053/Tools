using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OracleToMSSQL
{
    public class OracleDB
    {
        /// <summary>
        /// 連線字串
        /// </summary>
        string _connectionString = ConfigurationManager.ConnectionStrings["ORDBConnectionString"].ConnectionString;


        public DataTable GetDataTable(string SQLcmd, string TableName)
        {
            return GetDataSet(SQLcmd, TableName).Tables[TableName];
        }

        /// <summary>
        /// 執行SQL 回傳一個DataTable
        /// </summary>
        /// <param name="SQLcmd"></param>
        /// <param name="TabName"></param>
        /// <returns></returns>
        public DataSet GetDataSet(string SQLcmd,string TableName)
        {

            using (OracleConnection conn = new OracleConnection(_connectionString))
            {
                conn.Open();

                try
                {
                    DataSet myDataSet = new DataSet();
                    OracleDataAdapter myAdapter = new OracleDataAdapter(SQLcmd, conn);
                    myAdapter.Fill(myDataSet, TableName);
                    //記錄拼接出來的SQL語法到LOG
                    string msg
                        = DateTime.Now.ToString() + " 執行成功 \r\n"
                        + SQLcmd;
                    LogHelper.SaveLog(msg);

                    return myDataSet;
                }
                catch (Exception e)
                {

                    //記錄拼接出來的SQL語法到LOG
                    string msg
                        = DateTime.Now.ToString() + " \r\n"
                        + e.Message + " \r\n"
                        + SQLcmd;
                    LogHelper.SaveLog(msg);

                    throw e;
                }
            }
        }

    }
}
