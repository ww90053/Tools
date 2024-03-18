using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OracleToMSSQL
{
    public class MSSQLDB
    {
        string _connectionString = ConfigurationManager.ConnectionStrings["MSDBConnectionString"].ConnectionString;

        /// <summary>
        /// 根據DataTable 直接對抄資料表
        /// 具體來說有兩步驟,1 表刪除 2 SqlBulkCopy寫入
        /// </summary>
        /// <param name="dataTable">DataTable物件</param>
        /// <param name="tableName">目標資料表名稱</param>
        /// <returns></returns>
        public bool Write_DataTable_To_DBTable(DataTable dataTable, string tableName)
        {
            bool isOK = true;
            try
            {
                // 創建 DataTable 並填充數據（這裡假設數據已填充到 dataTable 中）
                //string connectionString = ConfigurationManager.ConnectionStrings["MSDBConnectionString"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    SqlTransaction sqlTransaction = connection.BeginTransaction();
                    //TRUNCATE TABLE指令會刪除掉原本的資料,所以需要搭配交易,避免失敗
                    string SQL = @" TRUNCATE TABLE " + tableName + @"  ";
                    using (SqlCommand command = new SqlCommand(SQL, connection))
                    {
                        //綁定交易
                        command.Transaction = sqlTransaction;

                        try
                        {
                            // 執行 SQL 語句
                            command.ExecuteNonQuery();
                            LogHelper.SaveLog($"清除{tableName} 資料成功");
                        }
                        catch (Exception ex)
                        {
                            LogHelper.SaveLog("Error: " + ex.Message);
                            sqlTransaction.Rollback();
                            isOK = false;//標記出錯
                            return isOK;//如果這節就錯了,那後面不要跑了
                        }

                    }// using (SqlCommand command = new SqlCommand(SQL, connection))
                     // 創建 SqlBulkCopy 對象，設置目標表名稱和連接

                    // 設置處理現有資料的選項（保留現有識別欄位值）

                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.CheckConstraints, sqlTransaction))
                    {
                        //綁定目標資料表名稱
                        bulkCopy.DestinationTableName = tableName;

                        // 將 DataTable 中的資料批次複製到目標資料庫表中
                        try
                        {
                            bulkCopy.WriteToServer(dataTable);
                            LogHelper.SaveLog($"重新寫入{tableName} 資料成功");
                        }
                        catch (Exception ex)
                        {
                            LogHelper.SaveLog("Error: " + ex.Message);
                            sqlTransaction.Rollback();
                            LogHelper.SaveLog($"{tableName} 資料同步失敗,回溯");
                            isOK = false;//標記出錯
                        }
                    }
                    //都成功才提交
                    if (isOK)
                    {
                        sqlTransaction.Commit();
                        LogHelper.SaveLog($"同步{tableName} 資料提交成功");
                    }

                }

            }
            catch (Exception ex)
            {
                LogHelper.SaveLog("Error: " + ex.Message);
                isOK = false;//標記出錯
            }
            return isOK;
        }
    }
}
