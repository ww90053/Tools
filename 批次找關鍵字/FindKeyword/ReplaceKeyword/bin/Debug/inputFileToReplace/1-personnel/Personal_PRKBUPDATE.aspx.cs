using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace TPPDDB._1_personnel
{
    public partial class Personal_PRKBUPDATE : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                A.check_power();
                TextBox_MZ_NO.Text = Request["MZ_NO"].ToString();
            }

        }

        protected void btOK_Click(object sender, EventArgs e)
        {
            //因獎懲案號有特殊符號需求，不進行字串處理。 20190829 by sky
            string selectString = string.Format("SELECT MZ_ID FROM A_PRKB WHERE MZ_NO='{0}'", TextBox_MZ_NO.Text.Trim());

            DataTable dt = new DataTable();

            dt = o_DBFactory.ABC_toTest.Create_Table(selectString, "GET");

            if (dt.Rows.Count == 0)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('無資料')", true);
            }
            else
            {
                //using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
                //{
                //    conn.Open();

                //    SqlTransaction oraTran = conn.BeginTransaction();

                try
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string strSQL = @"
                                SELECT MZ_AD,MZ_UNIT,MZ_EXAD,MZ_EXUNIT
                                ,MZ_OCCC --職稱代碼 
                                ,MZ_RANK --官職等起 
                                ,MZ_RANK1 --官職等迄
                                ,MZ_SRANK --薪俸職等 
                                ,MZ_TBDV  --職序 
                                FROM A_DLBASE
                                WHERE MZ_ID='" + dt.Rows[i][0].ToString() + "' ";

                        DataTable tempDT = new DataTable();

                        tempDT = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET2");

                        string AD = tempDT.Rows[0]["MZ_AD"].ToString();
                        string UNIT = tempDT.Rows[0]["MZ_UNIT"].ToString();
                        string EXAD = tempDT.Rows[0]["MZ_EXAD"].ToString();
                        string EXUNIT = tempDT.Rows[0]["MZ_EXUNIT"].ToString();
                        string OCCC = tempDT.Rows[0]["MZ_OCCC"].ToString();
                        string MZ_RANK = tempDT.Rows[0]["MZ_RANK"].ToString();
                        string MZ_RANK1 = tempDT.Rows[0]["MZ_RANK1"].ToString();
                        string MZ_SRANK = tempDT.Rows[0]["MZ_SRANK"].ToString();
                        string MZ_TBDV = tempDT.Rows[0]["MZ_TBDV"].ToString();




                        //string updateString = "UPDATE A_PRKB SET MZ_AD='" + AD + "',MZ_UNIT='" + UNIT + "',MZ_EXAD='" + EXAD + "',MZ_EXUNIT='" + EXUNIT + 
                        //     "' WHERE MZ_ID='" + dt.Rows[i][0].ToString() + "' AND MZ_NO='" + o_str.tosql(TextBox_MZ_NO.Text.Trim()) + "'";

                        string updateString = @"
UPDATE A_PRKB 
SET MZ_AD=@MZ_AD ,MZ_UNIT=@MZ_UNIT,MZ_EXAD=@MZ_EXAD,MZ_EXUNIT=@MZ_EXUNIT 
,MZ_OCCC=@MZ_OCCC --職稱代碼 
,MZ_RANK=@MZ_RANK --官職等起 
,MZ_RANK1=@MZ_RANK1 --官職等迄
,MZ_SRANK=@MZ_SRANK --薪俸職等 
,MZ_TBDV=@MZ_TBDV  --職序 
WHERE MZ_ID=@MZ_ID AND MZ_NO=@MZ_NO
";
                        List<SqlParameter> para = new List<SqlParameter>();

                        para.Add(new SqlParameter("MZ_AD", SqlDbType.VarChar) { Value = AD });
                        para.Add(new SqlParameter("MZ_UNIT", SqlDbType.VarChar) { Value = UNIT });
                        para.Add(new SqlParameter("MZ_EXAD", SqlDbType.VarChar) { Value = EXAD });
                        para.Add(new SqlParameter("MZ_EXUNIT", SqlDbType.VarChar) { Value = EXUNIT });
                        para.Add(new SqlParameter("MZ_OCCC", SqlDbType.VarChar) { Value = OCCC });

                        para.Add(new SqlParameter("MZ_RANK", SqlDbType.VarChar) { Value = MZ_RANK  });
                        para.Add(new SqlParameter("MZ_RANK1", SqlDbType.VarChar) { Value = MZ_RANK1 });
                        para.Add(new SqlParameter("MZ_SRANK", SqlDbType.VarChar) { Value = MZ_SRANK });
                        para.Add(new SqlParameter("MZ_TBDV", SqlDbType.VarChar) { Value = MZ_TBDV });

                        para.Add(new SqlParameter("MZ_ID", SqlDbType.VarChar) { Value = dt.Rows[i][0].ToString() });
                        para.Add(new SqlParameter("MZ_NO", SqlDbType.VarChar) { Value = TextBox_MZ_NO.Text.Trim() });

                        o_DBFactory.ABC_toTest.SQLExecute(updateString, para);

                        //SqlCommand cmd = new SqlCommand(updateString, conn);

                        //cmd.Transaction = oraTran;

                        //cmd.ExecuteNonQuery();

                        if (i == dt.Rows.Count - 1)
                        {
                            //oraTran.Commit();

                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('重整成功');window.close();", true);
                        }
                    }

                }
                catch
                {
                    //oraTran.Rollback();
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('重整失敗')", true);
                }
                finally
                {
                    //conn.Close();
                    ////XX2013/06/18 
                    //conn.Dispose();
                }
                //}
            }
        }

        protected void btLEAVE_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.close();", true);
        }
    }
}
