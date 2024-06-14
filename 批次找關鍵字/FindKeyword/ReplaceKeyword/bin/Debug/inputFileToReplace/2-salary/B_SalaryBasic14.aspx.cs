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
    public partial class B_SalaryBasic14 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (IsPostBack != true)
            {
                SalaryPublic.checkPermission();
                if (ViewState["ViewState_OLD"] == null)
                {
                    Session.Remove("Session_OLD_OK");
                }
                Label_MSG.Text = "";
                btUpdate.Enabled = false;
                btExit.Visible = false;
                btExit.Enabled = false;

                intINSURANCE = 0;
                intPAY1 = 0;

                ViewState["ViewState_OLD"] = "NOW";
            }

            if (ViewState["ViewState_OLD"].Equals("NOW"))
            {
                SqlDataSource_LABOR_INSURANCE.SelectCommand = "SELECT ID, INSURANCE, PAY1, CREATEDATE FROM B_LABOR_INSURANCE ORDER BY ID ASC";
            }
            else
            {
                if (Session["Session_OLD_OK"] != null && Session["Session_OLD_OK"].Equals("OLD_OK"))
                {
                    GridView_LABOR_INSURANCE.Columns[3].Visible = true;
                    GridView_LABOR_INSURANCE.Columns[4].Visible = true;
                    SqlDataSource_LABOR_INSURANCE.SelectCommand = "SELECT ID, INSURANCE, PAY1, CREATEDATE FROM B_LABOR_INSURANCE ORDER BY ID ASC";
                }
                else
                {
                    GridView_LABOR_INSURANCE.Columns[3].Visible = false;
                    GridView_LABOR_INSURANCE.Columns[4].Visible = false;
                    SqlDataSource_LABOR_INSURANCE.SelectCommand = "SELECT ID, INSURANCE, PAY1, CREATEDATE FROM B_LABOR_INSURANCE_OLD ORDER BY CREATEDATE DESC, ID ASC";
                }
            }
            GridView_LABOR_INSURANCE.DataBind();
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

        private bool boolCHK_ID_Data(string strID_Data)
        {
            using (SqlConnection SelectConn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                SelectConn.Open();
                try
                {
                    string strSQL = "SELECT * FROM B_LABOR_INSURANCE WHERE \"ID\" = '" + strID_Data + "'";
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
                Label_MSG.Text = "代碼欄位不可為空白";
                TextBox_ID.BackColor = Color.Orange;
            }
            else
            {
                if (boolCHK_ID_Data(strID) == false)
                {
                    if (boolLABOR_INSURANCE_Create(TextBox_ID.Text.Trim(), intINSURANCE, intPAY1, "B_LABOR_INSURANCE"))
                    {
                        GridView_LABOR_INSURANCE.DataBind();
                        Label_MSG.Text = "新增完成";
                        Label_MSG.ForeColor = Color.Blue;
                    }
                    else
                    {
                        Label_MSG.Text = "新增失敗";
                    }
                }
                else
                {
                    Label_MSG.Text = "新增失敗，ID代碼已重覆。";
                    TextBox_ID.BackColor = Color.Orange;
                }
            }
        }

        private bool boolLABOR_INSURANCE_Create(string strID_Data, int intINSURANCE_Data, int intPAY1_Data, string strDB_Table)
        {
            using (SqlConnection insertconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                insertconn.Open();
                try
                {
                    string insertString = "";
                    if (strDB_Table != "B_LABOR_INSURANCE")
                    {
                        insertString = "Insert Into B_LABOR_INSURANCE_OLD (LIO_SNID,\"ID\",INSURANCE,PAY1,CREATEDATE) "
                        + "Values ( NEXT VALUE FOR dbo.B_SET_SN, '" + strID_Data + "', '" + intINSURANCE_Data + "', '" + intPAY1_Data + "', @CREATEDATE)";
                    }
                    else
                    {
                        insertString = "Insert Into B_LABOR_INSURANCE (\"ID\",INSURANCE,PAY1,CREATEDATE) "
                        + "Values ('" + strID_Data + "', '" + intINSURANCE_Data + "', '" + intPAY1_Data + "', @CREATEDATE)";

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

        private bool boolLABOR_INSURANCE_Update(string strID_Data, int intINSURANCE_Data, int intPAY1_Data)
        {
            using (SqlConnection updateconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                updateconn.Open();
                try
                {
                    string updateString = "UPDATE B_LABOR_INSURANCE SET PAY1 = '" + intPAY1_Data + "', INSURANCE = '" + intINSURANCE_Data + "' WHERE \"ID\" = '" + strID_Data + "'";

                    SqlCommand cmd = new SqlCommand(updateString, updateconn);
                    cmd.CommandType = CommandType.Text;

                    cmd.Connection = updateconn;
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch { return false; }
                finally { updateconn.Close();
                //XX2013/06/18 
                updateconn.Dispose();
                }
            }
        }

        private string strLABOR_INSURANCE_Select()
        {
            using (SqlConnection SelectConn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                SelectConn.Open();
                try
                {
                    string strSQL = "SELECT * FROM B_LABOR_INSURANCE";
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

                            //if (boolLABOR_INSURANCE_Create(strID, intINSURANCE_Data, intPAY1_Data, "B_LABOR_INSURANCE_OLD") == false)
                            //{
                            //    return "轉入歷史時，有部份資料失敗，請確認轉入資料。";
                            //}
                            //else
                            //{
                            //    if (strLABOR_INSURANCE_Delete(strID) == false)
                            //    {
                            //        return "轉入歷史時，進行刪資料時，發生錯誤。";
                            //    }
                            //}

                            if (boolLABOR_INSURANCE_Create(strID, intINSURANCE_Data, intPAY1_Data, "B_LABOR_INSURANCE_OLD") == false)
                            {
                                return  "轉入歷史時，有部份資料失敗，請確認轉入資料。";
                            }
                        }

                        GridView_LABOR_INSURANCE.Columns[3].Visible = true;
                        GridView_LABOR_INSURANCE.Columns[4].Visible = true;
                        Session["Session_OLD_OK"] = "OLD_OK";
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

        private bool strLABOR_INSURANCE_Delete(string strID_Data)
        {
            using (SqlConnection DeleteConn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                DeleteConn.Open();
                string TPFDeleteString = "DELETE B_LABOR_INSURANCE WHERE \"ID\" = '" + strID_Data + "'";
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

            //    string UpdateString = "UPDATE B_LABOR_INSURANCE SET OLD_DATA=@OLD_DATA "
            //        + " WHERE LI_SNID = '" + Session["B_LABOR_INSURANCESelectKeyword"] + "' ";

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

            if (boolLABOR_INSURANCE_Update(strID, intINSURANCE, intPAY1))
            {
                Label_MSG.Text = "修改成功";
            }
            else
            {
                Label_MSG.Text = "修改失敗";
            }

            btUpdate.Enabled = true;
            btExit.Enabled = true;
            GridView_LABOR_INSURANCE.DataBind();
        }

        protected void btExit_Click(object sender, EventArgs e)
        {
            //voidExit_Data();
            TextBox_ID.Enabled = true;
            btCreate.Enabled = true;
            btUpdate.Enabled = false;
            btExit.Visible = false;
            btExit.Enabled = false;
        }

        protected void GridView_HEALTH_INSURANCE_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            int index = Convert.ToInt32(e.CommandArgument);
            string strKeyword = GridView_LABOR_INSURANCE.DataKeys[index].Value.ToString();

            if (e.CommandName == "btSelect")
            {
                btCreate.Enabled = false;
                btUpdate.Enabled = true;
                btExit.Visible = true;
                btExit.Enabled = true;
                TextBox_ID.Enabled = false;

                //Session["B_LABOR_INSURANCESelectKeyword"] = strKeyword;

                using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
                {
                    Selectconn.Open();

                    string strSQL = "SELECT * FROM B_LABOR_INSURANCE WHERE ID = '" + strKeyword + "' ";
                    SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);

                    Selectcmd.ExecuteNonQuery();

                    SqlDataReader dr = Selectcmd.ExecuteReader(CommandBehavior.SingleRow);

                    if (dr.Read())
                    {
                        strID = dr["ID"].ToString();
                        intINSURANCE = int.Parse(dr["INSURANCE"].ToString());
                        intPAY1 = int.Parse(dr["PAY1"].ToString());
                    }
                }
            }

            if (e.CommandName == "btDelete")
            {
                using (SqlConnection DeleteConn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
                {
                    DeleteConn.Open();
                    string DeleteString = "DELETE B_LABOR_INSURANCE WHERE \"ID\" = '" + strKeyword + "'";
                    SqlCommand cmd = new SqlCommand(DeleteString, DeleteConn);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        strID = "";
                        intINSURANCE = 0;
                        intPAY1 = 0;
                        btCreate.Enabled = true;
                        btUpdate.Enabled = false;
                        btExit.Enabled = false;
                        GridView_LABOR_INSURANCE.DataBind();
                    }
                    catch { Label_MSG.Text = "刪除錯誤"; }
                    finally { DeleteConn.Close();
                    //XX2013/06/18 
                    DeleteConn.Dispose();
                    }
                }
            }

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
            //GridView_LABOR_INSURANCE.DataBind();
        }

        protected void btOLD_DATA_Click(object sender, EventArgs e)
        {
            Label_MSG.Text = strLABOR_INSURANCE_Select();
        }

        protected void voidChanged(object sender, EventArgs e)
        {
            switch (((TextBox)sender).ID)
            {
                case "TextBox_PAY1":
                    intPAY1 = SalaryPublic.intdelimiterChars(TextBox_PAY1.Text);
                    break;
                case "TextBox_INSURANCE":
                    intINSURANCE = SalaryPublic.intdelimiterChars(TextBox_INSURANCE.Text);
                    break;
            }
        }


        protected void DropDownList_OLD_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownList_OLD.SelectedValue == "NEW")
            {
                ViewState["ViewState_OLD"] = "NOW";
                if (Session["Session_OLD_OK"] != null && Session["Session_OLD_OK"].Equals("OLD_OK"))
                {
                    GridView_LABOR_INSURANCE.Columns[3].Visible = true;
                    GridView_LABOR_INSURANCE.Columns[4].Visible = true;
                }
                SqlDataSource_LABOR_INSURANCE.SelectCommand = "SELECT ID, INSURANCE, PAY1, CREATEDATE FROM B_LABOR_INSURANCE ORDER BY ID ASC";
                GridView_LABOR_INSURANCE.DataBind();
            }
            else
            {
                ViewState["ViewState_OLD"] = "OLD";
                SqlDataSource_LABOR_INSURANCE.SelectCommand = "SELECT ID, INSURANCE, PAY1, CREATEDATE FROM B_LABOR_INSURANCE_OLD ORDER BY CREATEDATE DESC, ID ASC";
                GridView_LABOR_INSURANCE.DataBind();
                GridView_LABOR_INSURANCE.Columns[3].Visible = false;
                GridView_LABOR_INSURANCE.Columns[4].Visible = false;
            }
        }

    }
}
