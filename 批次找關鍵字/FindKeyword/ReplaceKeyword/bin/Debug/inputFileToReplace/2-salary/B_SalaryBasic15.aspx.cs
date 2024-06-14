using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Configuration;
using System.Data.SqlClient;

namespace TPPDDB._2_salary
{
    public partial class B_SalaryBasic15 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
           

            if (IsPostBack != true)
            {
                SalaryPublic.checkPermission();
                intINSURANCE = 0;
                intPAY1 = 0;
                intPAY2 = 0;
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

        private int intINSURANCE
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_INSURANCE.Text);
            }
            set
            {
                TextBox_INSURANCE.Text = String.Format("{0:$#,#0}", int.Parse(value.ToString()));
            }
        }

        private int intPAY1
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_PAY1.Text);
            }
            set
            {
                TextBox_PAY1.Text = String.Format("{0:$#,#0}", int.Parse(value.ToString()));
            }
        }

        private int intPAY2
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_PAY2.Text);
            }
            set
            {
                TextBox_PAY2.Text = String.Format("{0:$#,#0}", int.Parse(value.ToString()));
            }
        }

        private bool boolCHK_ID_Data(string strID_Data)
        {
            using (SqlConnection SelectConn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                SelectConn.Open();
                try
                {
                    string strSQL = "SELECT * FROM B_GOV_INSURANCE WHERE \"ID\" = '" + strID_Data + "'";
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
            if (boolCHK_ID_Data(strID) == false)
            {

                if (boolGOV_INSURANCE_Create(TextBox_ID.Text, intINSURANCE, intPAY1, intPAY2, "B_GOV_INSURANCE"))
                {
                    GridView_GOV_INSURANCE.DataBind();
                    Label_MSG.Text = "新增完成";
                }
                else
                {
                    Label_MSG.Text = "新增失敗"; 
                }
            }
            else
            {
                Label_MSG.Text = "新增失敗，ID代碼已重覆。";
            }
        }

        private bool boolGOV_INSURANCE_Create(string strID_Data, int intINSURANCE_Data, int intPAY1_Data, int intPAY2_Data, string strDB_Table)
        {
            using (SqlConnection insertconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                insertconn.Open();
                try
                {
                    string insertString = "";
                    if (strDB_Table != "B_GOV_INSURANCE")
                    {
                        insertString = "Insert Into B_GOV_INSURANCE_OLD (GIO_SNID,\"ID\",INSURANCE,PAY1,PAY2,CREATEDATE) "
                        + "Values ( NEXT VALUE FOR dbo.B_SET_SN, '" + strID_Data + "', '" + intINSURANCE_Data + "', '" + intPAY1_Data + "', '" + intPAY2_Data + "', @CREATEDATE)";
                    }
                    else
                    {
                        insertString = "Insert Into B_GOV_INSURANCE (\"ID\",INSURANCE,PAY1,PAY2,CREATEDATE) "
                        + "Values ('" + strID_Data + "', '" + intINSURANCE_Data + "', '" + intPAY1_Data + "', '" + intPAY2_Data + "', @CREATEDATE)";

                    }

                    SqlCommand cmd = new SqlCommand(insertString, insertconn);
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.Add("CREATEDATE", SqlDbType.DateTime).Value = DateTime.Now;

                    cmd.Connection = insertconn;
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch
                {
                    return false;
                }
                finally { insertconn.Close();
                //XX2013/06/18 
                insertconn.Dispose();
                }
            }
        }

        private string strGOV_INSURANCE_Select()
        {
            using (SqlConnection SelectConn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                SelectConn.Open();
                try
                {
                    string strSQL = "SELECT * FROM B_GOV_INSURANCE";
                    SqlCommand SelectCmd = new SqlCommand(strSQL, SelectConn);
                    DataTable dt = new DataTable();
                    dt.Load(SelectCmd.ExecuteReader());
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow item in dt.Rows)
                        {
                            string strID = item["ID"].ToString();
                            int intINSURANCE_Data = int.Parse(item["INSURANCE"].ToString());
                            int intPAY1_Data = int.Parse(item["PAY1"].ToString());
                            int intPAY2_Data = int.Parse(item["PAY2"].ToString());

                            if (boolGOV_INSURANCE_Create(strID, intINSURANCE_Data, intPAY1_Data, intPAY2_Data, "B_GOV_INSURANCE_OLD") == false)
                            {
                                return "轉入歷史時，有部份資料失敗，請確認轉入資料。";
                            }
                            else
                            {
                                if (strGOV_INSURANCE_Delete(strID) == false)
                                {
                                    return "轉入歷史時，進行刪資料時，發生錯誤。";
                                }
                            }
                        }

                        return "轉入歷史完成。";
                    }
                    else
                    { return "無轉入歷史檔資料。"; }

                }
                catch { return "資料庫連線錯誤。"; }
                finally { SelectConn.Close();
                //XX2013/06/18 
                SelectConn.Dispose();
                }
            }
        }

        private bool strGOV_INSURANCE_Delete(string strID_Data)
        {
            using (SqlConnection DeleteConn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                DeleteConn.Open();
                string TPFDeleteString = "DELETE B_GOV_INSURANCE WHERE \"ID\" = '" + strID_Data + "'";
                SqlCommand DeleteCmd = new SqlCommand(TPFDeleteString, DeleteConn);
                DataTable dt1 = new DataTable();
                dt1.Load(DeleteCmd.ExecuteReader());
                try
                {
                    DeleteCmd.ExecuteNonQuery();
                    return true;
                }
                catch
                {
                    return false;
                }
                finally
                {
                    DeleteConn.Close();

                    //XX2013/06/18 
                    DeleteConn.Dispose();
                }

            }
        }

        protected void btUpdate_Click(object sender, EventArgs e)
        {
            //using (SqlConnection Updateconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            //{
            //    Updateconn.Open();

            //    string UpdateString = "UPDATE B_GOV_INSURANCE SET OLD_DATA=@OLD_DATA "
            //        + " WHERE GEI_SNID = '" + Session["B_GOV_INSURANCESelectKeyword"] + "' ";

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

        protected void GridView_GOV_INSURANCE_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //if (e.CommandName == "btSelect")
            //{
            //    btCreate.Enabled = false;
            //    btUpdate.Enabled = true;
            //    btExit.Visible = true;
            //    btExit.Enabled = true;
            //    TextBox_ID.Enabled = false;
            //    TextBox_INSURANCE.Enabled = false;
            //    TextBox_PAY1.Enabled = false;
            //    TextBox_PAY2.Enabled = false;
            //    RadioButtonList_OLD_DATA.Enabled = true;

            //    int index = Convert.ToInt32(e.CommandArgument);
            //    string strKeyword = GridView_GOV_INSURANCE.DataKeys[index].Value.ToString();

            //    Session["B_GOV_INSURANCESelectKeyword"] = strKeyword;

            //    using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            //    {
            //        Selectconn.Open();

            //        string strSQL = "SELECT * FROM B_GOV_INSURANCE WHERE GEI_SNID = '" + Session["B_GOV_INSURANCESelectKeyword"] + "' ";
            //        SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);

            //        Selectcmd.ExecuteNonQuery();

            //        SqlDataReader dr = Selectcmd.ExecuteReader(CommandBehavior.SingleRow);

            //        if (dr.Read())
            //        {
            //            TextBox_ID.Text = dr["ID"].ToString();
            //            TextBox_INSURANCE.Text = dr["INSURANCE"].ToString();
            //            TextBox_PAY1.Text = dr["PAY1"].ToString();
            //            TextBox_PAY2.Text = dr["PAY2"].ToString();
            //            RadioButtonList_OLD_DATA.SelectedValue = dr["OLD_DATA"].ToString();
            //        }
            //    }
            //}
        }

        private void voidExit_Data()
        {
            //TextBox_ID.Text = string.Empty;
            //TextBox_INSURANCE.Text = string.Empty;
            //TextBox_PAY1.Text = string.Empty;
            //TextBox_PAY2.Text = string.Empty;
            //RadioButtonList_OLD_DATA.SelectedValue = "N";
            //btCreate.Enabled = true;
            //btUpdate.Enabled = false;
            //btExit.Visible = false;
            //btExit.Enabled = false;
            //TextBox_ID.Enabled = true;
            //TextBox_INSURANCE.Enabled = true;
            //TextBox_PAY1.Enabled = true;
            //TextBox_PAY2.Enabled = true;
            //RadioButtonList_OLD_DATA.Enabled = false;
            //GridView_GOV_INSURANCE.DataBind();
        }

        protected void btOLD_DATA_Click(object sender, EventArgs e)
        {
            Label_MSG.Text = strGOV_INSURANCE_Select();
            GridView_GOV_INSURANCE.DataBind();
        }

        protected void voidChanged(object sender, EventArgs e)
        {
            switch ((sender as TextBox).ID)
            {
                case "TextBox_PAY1":
                    intPAY1 = SalaryPublic.intdelimiterChars(TextBox_PAY1.Text);
                    break;
                case "TextBox_PAY2":
                    intPAY2 = SalaryPublic.intdelimiterChars(TextBox_PAY2.Text);
                    break;
                case "TextBox_INSURANCE":
                    intINSURANCE = SalaryPublic.intdelimiterChars(TextBox_INSURANCE.Text);
                    break;
            }
        }

    }
}
