using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace TPPDDB._2_salary
{
    public partial class _9_EffectImport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Form.Attributes.Add("enctype", "multipart/form-data");
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string fileName = fl_import.FileName;
            string up_extension = System.IO.Path.GetExtension(fileName);// 取得上傳檔副檔名
            string savePath = "\\Files\\" + fileName;
            savePath = System.Web.HttpContext.Current.Server.MapPath(savePath);
            fl_import.SaveAs(savePath);

            int success = 0;
            int fail = 0;

            DataTable dt = Excel.getDataTable(savePath, "Sheet1");
            System.IO.File.Delete(savePath);
            string eng = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            ////SqlTransaction transation;
            //SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString);
            //List<string> condition = new List<string>();
            //if (conn.State == ConnectionState.Closed)
            //{
            //    conn.Open();
               
                try
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        //transation = conn.BeginTransaction();

                        //SqlCommand cmd = new SqlCommand();

                        //20150216
                        //ID 那欄為上下合併儲存格 
                        //而不是上下兩行

                        //2018.1.23 by andy 格式更改
                        //if (row[2].ToString().Length == 0)
                        //    continue;
                        //if (!row[2].ToString().Contains('\n'))
                        //    continue;

                        //2018.1.23 by andy 格式更改
                        //string id = row[2].ToString().Split('\n')[1];
                        string id = row[1].ToString();
                        if (id == "")
                        {
                            continue;
                        }

                        //if (row[3].ToString().Trim().Length != 10)
                        //    continue;

                        //string id = row[3].ToString().Trim();
                        int grade;
                        string gradeLevel;
                        string sql = "";

                        if (id.Length != 10)
                            continue;
                        if (eng.IndexOf(id[0]) < 0)
                            continue;

                        //2018.1.23 by andy 格式更改
                        //int.TryParse(row[8].ToString(), out grade);
                        int.TryParse(row[30].ToString(), out grade);

                        Police police = new Police(id);

                        if (grade >= 80)
                            gradeLevel = "甲";
                        else if (grade >= 70)
                            gradeLevel = "乙";
                        else if (grade >= 60)
                            gradeLevel = "丙";
                        else
                            gradeLevel = "丁";

                        ////string B_sql = string.Format("UPDATE B_BASE SET GRADE='{0}' WHERE IDCARD='{1}'", gradeLevel, id);


                        ///////////
                        sql = "SELECT COUNT(*) FROM A_EFFICIENCY WHERE T01=@T01 AND MZ_ID=@MZ_ID ";
                        List<SqlParameter> para = new List<SqlParameter>();
                        para.Add(new SqlParameter("T01", SqlDbType.VarChar) { Value = txt_Year.Text });
                        para.Add(new SqlParameter("MZ_ID", SqlDbType.VarChar) { Value = id });
                        if (o_DBFactory.ABC_toTest.GetValue(sql, para) == "0")
                        {

                            sql = "INSERT INTO A_EFFICIENCY (T01,MZ_ID,T17,T25,T26) VALUES (@T01,@MZ_ID,@T17,@T25,@T26)";
                            para = new List<SqlParameter>();
                            para.Add(new SqlParameter("T01", SqlDbType.VarChar) { Value = txt_Year.Text });
                            para.Add(new SqlParameter("MZ_ID", SqlDbType.VarChar) { Value = id });
                            para.Add(new SqlParameter("T17", SqlDbType.VarChar) { Value = grade });
                            para.Add(new SqlParameter("T25", SqlDbType.VarChar) { Value = "年終考績" });
                            para.Add(new SqlParameter("T26", SqlDbType.VarChar) { Value = gradeLevel });



                        }
                        else
                        {
                            sql = "UPDATE A_EFFICIENCY SET T17=@T17 ,T26=@T26 WHERE T01=@T01 AND MZ_ID=@MZ_ID";
                            para = new List<SqlParameter>();
                            para.Add(new SqlParameter("T01", SqlDbType.VarChar) { Value = txt_Year.Text });
                            para.Add(new SqlParameter("MZ_ID", SqlDbType.VarChar) { Value = id });
                            para.Add(new SqlParameter("T17", SqlDbType.VarChar) { Value = grade });
                            para.Add(new SqlParameter("T26", SqlDbType.VarChar) { Value = gradeLevel });




                        }
                        ///////////////

                        //cmd.CommandText = sql;
                        //cmd.Connection = conn;
                        //cmd.Transaction = transation;

                        try
                        {


                            o_DBFactory.ABC_toTest.SQLExecute(sql, para);

                            ////20150216
                            ////保留舊有填回B_BASE的 GRADE
                            ////因為不想再薪資考績作業或薪資基本資料作業時.又要回頭判斷年度並抓人事的考績,
                            ////而且也怕修正原本考績的公式被打亂或遺漏.所以考績薪資公式都沒有修正
                            //o_DBFactory.ABC_toTest.SQLExecute(B_sql);


                            //o_DBFactory.ABC_toTest.Edit_Data(sql);
                            //cmd.ExecuteNonQuery();
                            //cmd.Transaction.Commit();
                            success++;
                        }
                        catch
                        {
                            //transation.Rollback();
                            fail++;
                        }
                    }
                }
                catch
                {
                   
                }
                finally
                {
                    //if (conn.State == ConnectionState.Open)
                    //{
                    //    //XX2013/06/18 
                        
                    //    //conn.Close();
                    //}
                    //conn.Close();
                    //conn.Dispose();
                }
            //}
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", string.Format("alert('匯入完成，成功{0}筆、失敗{1}筆、共{2}筆')", success, fail, success + fail), true);
        }
    }
}
