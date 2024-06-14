using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Drawing;

namespace TPPDDB._2_salary
{
    public partial class B_SalaryBasic16 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
             if (IsPostBack != true)
            {
                SalaryPublic.checkPermission();
                intPAY = 0;
            }
            Label_MSG.Text = "";
            btUpdate.Enabled = false;
            btExit.Visible = false;
            btExit.Enabled = false;

           
        }

        private string strID
        {
            get
            {
                return TextBox_ID.Text;
            }
            set
            {
                TextBox_ID.Text = value;
            }
        }

        private int intPAY
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_PAY.Text);
            }
            set
            {
                TextBox_PAY.Text = String.Format("{0:$#,#0}", int.Parse(value.ToString()));
            }
        }

        private bool boolCHK_ID_Data(string strID_Data)
        {
            
                    string strSQL = "SELECT * FROM B_SALARY_2 WHERE \"ID\" = '" + strID_Data + "'";
                    
                    DataTable dt = new DataTable();
                    dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");
                    if (dt.Rows.Count == 1)
                    {
                        return true;
                    }
                    else
                    { return false; }

               
            
        }

        protected void btCreate_Click(object sender, EventArgs e)
        {
            TextBox_ID.BackColor = Color.White;
            Label_MSG.ForeColor = Color.Red;
            Label_MSG.Text = "";
            if (TextBox_ID.Text.Trim().Length == 0)
            {
                Label_MSG.Text = "代碼欄位不可為空白";
                TextBox_ID.BackColor = Color.Orange;
            }
            else
            {
                if (boolCHK_ID_Data(strID) == false)
                {
                    using (SqlConnection insertconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
                    {
                        insertconn.Open();
                        try
                        {
                            string insertString = "Insert Into B_SALARY_2(\"ID\",\"NAME\",PAY,MP,CREATEDATE) "
                            + "Values (:\"ID\",:\"NAME\",@PAY,@MP,@CREATEDATE)";

                            SqlCommand cmd = new SqlCommand(insertString, insertconn);
                            cmd.CommandType = CommandType.Text;

                            cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = TextBox_ID.Text.Trim();
                            cmd.Parameters.Add("NAME", SqlDbType.VarChar).Value = TextBox_NAME.Text;
                            cmd.Parameters.Add("PAY", SqlDbType.Float).Value = intPAY;
                            cmd.Parameters.Add("MP", SqlDbType.VarChar).Value = RadioButtonList_MP.SelectedValue;
                            cmd.Parameters.Add("CREATEDATE", SqlDbType.DateTime).Value = DateTime.Now;

                            cmd.Connection = insertconn;

                            cmd.ExecuteNonQuery();
                            GridView_SALARY_2.DataBind();
                            Label_MSG.Text = "新增完成";
                            Label_MSG.ForeColor = Color.Blue;
                        }
                        catch { Label_MSG.Text = "新增失敗"; }
                        finally { insertconn.Close();
                        //XX2013/06/18 
                        insertconn.Dispose();
                        }
                    }
                }
                else
                {
                    Label_MSG.Text = "新增失敗，ID代碼已重覆。";
                    TextBox_ID.BackColor = Color.Orange;
                }
            }
        }

        protected void btUpdate_Click(object sender, EventArgs e)
        {
           
        }

        protected void btExit_Click(object sender, EventArgs e)
        {
            //voidExit_Data();
        }

        protected void GridView_SALARY_2_RowCommand(object sender, GridViewCommandEventArgs e)
        {
           
        }

      

        protected void voidChanged(object sender, EventArgs e)
        {
            switch ((sender as TextBox).ID)
            {
                case "TextBox_PAY":
                    intPAY = SalaryPublic.intdelimiterChars(TextBox_PAY.Text);
                    break;
            }
        }
    }
}
