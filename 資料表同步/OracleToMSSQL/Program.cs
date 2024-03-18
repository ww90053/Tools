using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OracleToMSSQL
{
    class Program
    {
        static OracleDB oracleDB = new OracleDB();
        static MSSQLDB mssql = new MSSQLDB();

        static void Main(string[] args)
        {
            //資料表無腦對抄,注意兩邊的結構要相同,建議使用SSMA工具同步結構較好
            CopyDBTable_OracleToMSSQL("A_DLBASE","TPDBUSER.A_DLBASE");
            //CopyDBTable_OracleToMSSQL("B_BASE", "TPDBUSER.B_BASE");            
        }

        /// <summary>
        /// 資料表無腦對抄,注意兩邊的結構要相同,建議使用SSMA工具同步結構較好
        /// </summary>
        /// <param name="fromTableName">來源資料表名稱</param>
        /// <param name="ToTableName">目標資料表名稱</param>
        /// <returns></returns>
        public static bool CopyDBTable_OracleToMSSQL(string fromTableName,string ToTableName)
        {
            //根據資料表名稱,或SQL,連線到Oracle資料庫,將資料全抓,回傳DT
            //抓取來源資料表
            DataTable dt = oracleDB.GetDataTable($"select * from {fromTableName} ", fromTableName);
            //修改名稱為目標資料表,此舉不確定是否有用?
            //dt.TableName = ToTableName;
            //根據DT & SQL ,連線到MSSQL,並把資料寫入對應的表
            bool isOK = mssql.Write_DataTable_To_DBTable(dt, ToTableName);
            return isOK;
        }
    }
}
