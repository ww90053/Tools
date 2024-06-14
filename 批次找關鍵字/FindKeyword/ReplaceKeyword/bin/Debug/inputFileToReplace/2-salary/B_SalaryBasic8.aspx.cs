using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Drawing;

namespace TPPDDB._2_salary
{
    public partial class B_SalaryBasic8 : System.Web.UI.Page
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
            using (SqlConnection SelectConn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                SelectConn.Open();
                try
                {
                    string strSQL = "SELECT * FROM B_ADVENTIVE WHERE \"ID\" = '" + strID_Data + "'";
                    SqlCommand SelectCmd = new SqlCommand(strSQL, SelectConn);
                    DataTable dt = new DataTable();
                    dt.Load(SelectCmd.ExecuteReader());
                    if (dt.Rows.Count == 1)
                    {
                        return true;
                    }
                    else
                    { return false; }

                }
                catch { return true; }
                finally { SelectConn.Close();
                //XX2013/06/18 
                SelectConn.Dispose();
                }
            }
        }

        protected void btCreate_Click(object sender, EventArgs e)
        {
            TextBox_ID.BackColor = Color.White;
            Label_MSG.ForeColor = Color.Red;
            Label_MSG.Text = "";
            if (TextBox_ID.Text.Trim().Length == 0)
            {
                Label_MSG.Text = "代碼不可為空白";
                TextBox_ID.BackColor = Color.Orange;
                return;
            }
            if (boolCHK_ID_Data(strID) == false)
            {
                using (SqlConnection insertconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
                {
                    insertconn.Open();
                    try
                    {
                        string insertString = "Insert Into B_ADVENTIVE (\"ID\",\"NAME\",PAY,CREATEDATE) "
                        + "Values (:\"ID\",:\"NAME\",@PAY,@CREATEDATE)";

                        SqlCommand cmd = new SqlCommand(insertString, insertconn);
                        cmd.CommandType = CommandType.Text;

                        cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = TextBox_ID.Text.Trim().PadLeft(2, '0');
                        cmd.Parameters.Add("NAME", SqlDbType.VarChar).Value = TextBox_NAME.Text;
                        cmd.Parameters.Add("PAY", SqlDbType.Float).Value = intPAY;
                        cmd.Parameters.Add("CREATEDATE", SqlDbType.DateTime).Value = DateTime.Now;

                        cmd.Connection = insertconn;
                        cmd.ExecuteNonQuery();
                        GridView_ADVENTIVE.DataBind();
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

        protected void btUpdate_Click(object sender, EventArgs e)
        {
            //using (SqlConnection Updateconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            //{
            //    Updateconn.Open();

            //    string UpdateString = "UPDATE B_ADVENTIVE SET OLD_DATA=@OLD_DATA "
            //        + " WHERE A_SNID = '" + Session["B_ADVENTIVESelectKeyword"] + "' ";

            //    SqlCommand cmd = new SqlCommand(UpdateString, Updateconn);

            //    cmd.Parameters.Add("OLD_DATA", SqlDbType.VarChar).Value = RadioButtonList_OLD_DATA.SelectedValue;

            //    try
            //    {
            //        cmd.ExecuteNonQuery();
            //        voidExit_Data();
            //        Label_MSG.Text = "更新成功！";
            //    }
            //    catch
            //    {
            //        Label_MSG.Text = "更新失敗！";
            //    }

            //    Updateconn.Close();
            //}
        }

        protected void btExit_Click(object sender, EventArgs e)
        {
            //voidExit_Data();
        }

        protected void GridView_ADVENTIVE_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //if (e.CommandName == "btSelect")
            //{
            //    btCreate.Enabled = false;
            //    btUpdate.Enabled = true;
            //    btExit.Visible = true;
            //    btExit.Enabled = true;
            //    TextBox_ID.Enabled = false;
            //    TextBox_NAME.Enabled = false;
            //    TextBox_PAY.Enabled = false;
            //    RadioButtonList_OLD_DATA.Enabled = true;

            //    int index = Convert.ToInt32(e.CommandArgument);
            //    string strKeyword = GridView_ADVENTIVE.DataKeys[index].Value.ToString();

            //    Session["B_ADVENTIVESelectKeyword"] = strKeyword;

            //    using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            //    {
            //        Selectconn.Open();

            //        string strSQL = "SELECT * FROM B_ADVENTIVE WHERE A_SNID = '" + Session["B_ADVENTIVESelectKeyword"] + "' ";
            //        SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);

            //        Selectcmd.ExecuteNonQuery();

            //        SqlDataReader dr = Selectcmd.ExecuteReader(CommandBehavior.SingleRow);

            //        if (dr.Read())
            //        {
            //            TextBox_ID.Text = dr["ID"].ToString();
            //            TextBox_NAME.Text = dr["NAME"].ToString();
            //            TextBox_PAY.Text = dr["PAY"].ToString();
            //            RadioButtonList_OLD_DATA.SelectedValue = dr["OLD_DATA"].ToString();
            //        }
            //    }
            //}
        }

        private void voidExit_Data()
        {
            //TextBox_ID.Text = string.Empty;
            //TextBox_NAME.Text = string.Empty;
            //TextBox_PAY.Text = string.Empty;
            //RadioButtonList_OLD_DATA.SelectedValue = "N";
            //btCreate.Enabled = true;
            //btUpdate.Enabled = false;
            //btExit.Visible = false;
            //btExit.Enabled = false;
            //TextBox_ID.Enabled = true;
            //TextBox_NAME.Enabled = true;
            //TextBox_PAY.Enabled = true;
            //RadioButtonList_OLD_DATA.Enabled = false;
            //GridView_ADVENTIVE.DataBind();
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
