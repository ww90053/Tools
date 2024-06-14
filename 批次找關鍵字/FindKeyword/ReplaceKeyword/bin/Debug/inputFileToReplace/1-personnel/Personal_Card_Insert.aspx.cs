using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.IO;
using System.Data.OleDb;

namespace TPPDDB._1_personnel
{
    public partial class Personal_Card_Insert : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            
            if (FileUpload1.HasFile)
            {

                string fileName = DateTime.Now.ToString("yyyyMMddhhmm") + ".xls";

                string savePath = Server.MapPath("./A_excel_upload/");

                savePath += fileName;
                //Label2.Text = savePath;
                FileUpload1.SaveAs(savePath);

                //FileUpload1.SaveAs("~/1-personnel/A_excel-upload");


                FileUpload1.SaveAs(savePath);
                string OLEDBConnStr = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source='" + savePath + "';" + "Extended Properties='Excel 8.0;HDR=YES;IMEX=1;Persist Security Info=False'";

                DataSet ds = new DataSet();
                OleDbConnection conn = new OleDbConnection();
                conn.ConnectionString = OLEDBConnStr;
                OleDbCommand com = new OleDbCommand();
                com.Connection = conn;
                DataTable dt = new DataTable();
                try
                {
                    com.CommandText = "select * from [Sheet1$]";  // SheetName = Sheet1 + $
                    conn.Open();
                    OleDbDataReader myData = com.ExecuteReader();
                    for (int i = 0; i < myData.FieldCount; i++)
                    {
                        DataColumn dc = new DataColumn();
                        dc.ColumnName = myData.GetName(i);
                        dc.DataType = myData.GetFieldType(i);
                        dt.Columns.Add(dc);
                    }
                    while (myData.Read())
                    {
                        DataRow dr = dt.NewRow();
                        for (int i = 0; i < myData.FieldCount; i++)
                        {

                            dr[i] = myData[i];
                        }
                        dt.Rows.Add(dr);
                    }
                    myData.Close();

                    //XX2013/06/18 
                    myData.Dispose();
                }
                catch { }
                finally
                {
                   
                    conn.Close();
                    //XX2013/06/18
                    conn.Dispose();
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Label2.Text += "<BR/>1";
                    Label2.Text += dt.Rows[i][0].ToString() + "   2";
                    Label2.Text += dt.Rows[i][1].ToString() + "   3";
                    Label2.Text += dt.Rows[i][2].ToString() + "   4";
                    Label2.Text += dt.Rows[i][3].ToString() + "   5";
                    Label2.Text += dt.Rows[i][4].ToString() + "   6";
                    Label2.Text += dt.Rows[i][5].ToString() + "   7";
                    Label2.Text += dt.Rows[i][6].ToString() + "   8";
                    Label2.Text += dt.Rows[i][7].ToString() + "   9";
                    Label2.Text += dt.Rows[i][8].ToString() + "   10";
                    Label2.Text += dt.Rows[i][9].ToString();
                }

            }

            //return dt;

        }
    }
}
