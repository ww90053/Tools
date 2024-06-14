using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace TPPDDB._2_salary
{
    public class DBClass
    {
        private static string strCon = WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString;

        public static DataTable DataSelect(string sql, List<SqlParameter> OPrams)
        {
            DataTable DT = new DataTable();
            SqlConnection conn = new SqlConnection(strCon);

            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                if (OPrams != null && OPrams.Count() > 0)
                {
                    foreach (SqlParameter para in OPrams)
                    {
                        cmd.Parameters.Add(para);
                    }
                }

                DT.Load(cmd.ExecuteReader());

                return DT;
            }
            catch
            {
                throw;
            }
            finally
            {
                //if (conn.State == ConnectionState.Open)
                //{
                //    //XX2013/06/18 
                    
                //    //conn.Close();
                //}
                conn.Close();
                //XX2013/06/18 
                conn.Dispose();
                cmd.Parameters.Clear();
            }
        }
        public static DataTable DataSelect(string SQLcmd, string TabName)
        {
            SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString);
            conn.Open();
            DataSet myDataSet = new DataSet();
            OracleDataAdapter myAdapter;
            myAdapter = new OracleDataAdapter(SQLcmd, conn);
            myAdapter.Fill(myDataSet, TabName);
            conn.Close();
            //XX2013/06/18 
            conn.Dispose();
            return myDataSet.Tables[TabName];

        }
        public static DataTable DataSelect(string sql)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(strCon))
            {
                SqlCommand cmd = new SqlCommand(string.Format(sql), conn);
                cmd.CommandType = CommandType.Text;
                conn.Open();
                dt.Load(cmd.ExecuteReader(CommandBehavior.CloseConnection));
                conn.Close();
                //XX2013/06/18 
                conn.Dispose();
            }
            return dt;
        }

        public static void Edit_Data(string SQLcmd)
        {
            SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand(SQLcmd, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
            //XX2013/06/18 
            conn.Dispose();
        }
        public static bool Edit_Data(string sql, List<SqlParameter> OPrams)
        {
            SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString);
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
                transation = conn.BeginTransaction();
            }
            bool isOk = false;
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Transaction = transation;
            try
            {
                if (OPrams != null && OPrams.Count() > 0)
                {
                    foreach (SqlParameter para in OPrams)
                    {
                        if (para.Value == null)
                            para.Value = DBNull.Value;
                        else if (string.IsNullOrEmpty(para.Value.ToString()))
                            para.Value = DBNull.Value;
                        cmd.Parameters.Add(para);
                    }
                }

                if (cmd.ExecuteNonQuery() > 0)
                {
                    transation.Commit();
                    isOk = true;
                }
                else
                {
                    isOk = false;
                }
            }
            catch(Exception ex)
            {

                //記錄拼接出來的SQL語法到LOG
                TPPDDB.App_Code.Log.SaveSQLError(sql, ex);

                transation.Rollback();
                isOk = false;
            }
            finally
            {
                //XX2013/06/18 
                //if (conn.State == ConnectionState.Open)
                //{
                //    conn.Close();
                //}
                conn.Close();
                //XX2013/06/18 
                conn.Dispose();
                cmd.Parameters.Clear();
            }


            return isOk;
        }

        public static string GetValueFromDB(string sqlstr)
        {
            string result;
            SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand(sqlstr, conn);
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader(CommandBehavior.CloseConnection));
            if (dt.Rows.Count > 0)
            {
                result = dt.Rows[0][0].ToString();
            }
            else
            {
                result = "";
            }

            return result;
        }
        public static string GetValueFromDB(string sql, List<SqlParameter> OPrams)
        {
            DataTable DT = new DataTable();
            SqlConnection conn = new SqlConnection(strCon);

            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                if (OPrams != null && OPrams.Count() > 0)
                {
                    foreach (SqlParameter para in OPrams)
                    {
                        cmd.Parameters.Add(para);
                    }
                }
                DT.Load(cmd.ExecuteReader());

                if (DT.Rows.Count > 0)
                    return DT.Rows[0][0].ToString();

                return "";
            }
            catch
            {
                throw;
            }
            finally
            {
                //XX2013/06/18 
                
                //if (conn.State == ConnectionState.Open)
                //{
                //    conn.Close();
                //}
                conn.Close();
                //XX2013/06/18 
                conn.Dispose();
                cmd.Parameters.Clear();
            }
        }

        public static string SequenceVal(string val)
        {
            DataTable dt = new DataTable();
            dt = DataSelect(string.Format("select {0}.nextval ", val), "GET");
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0][0].ToString();
            }
            else
                return "1";
        }

        private static string HandleSQL(string sql, List<SqlParameter> OPrams)
        {
            foreach (SqlParameter para in OPrams)
                sql = sql.Replace(":" + para.ParameterName, para.Value.ToString());
            return sql;
        }
        static SqlTransaction transation;
    }
}
