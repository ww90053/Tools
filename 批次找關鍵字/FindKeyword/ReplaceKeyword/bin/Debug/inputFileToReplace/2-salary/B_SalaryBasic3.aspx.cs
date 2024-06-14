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
    public partial class B_SalaryBasic3 : System.Web.UI.Page
    {
        string strPayAD;

        protected void Page_Load(object sender, EventArgs e)
        {
            //檢核頁面存取權限
            
            strPayAD = SalaryPublic.strLoginEXAD;

            if (!IsPostBack)
            {
                SalaryPublic.checkPermission();
                ViewState["GroupPermission"] = SalaryPublic.GetGroupPermission();
            }
            DoSearch();

            Label_MSG.Text = "";
            btUpdate.Enabled = false;
            btExit.Visible = false;
            btExit.Enabled = false;
        }

        private string strBank_ID
        {
            get
            {
                return DropDownList_BANK.SelectedValue.ToString();
            }
            set
            {
                if (value != "")
                {
                    DropDownList_BANK.SelectedValue = value;
                }
                else
                {
                    ListItem li = new ListItem("無資料", "0");
                    DropDownList_BANK.Items.Insert(0, li);
                    DropDownList_BANK.SelectedValue = "0";
                }
            }
        }

        protected void btCreate_Click(object sender, EventArgs e)
        {
            TextBox_BANKNO.BackColor = Color.White;
            TextBox_NAME.BackColor = Color.White;
            Label_MSG.Text = "";
            if (TextBox_NAME.Text.Trim().Length == 0 || TextBox_BANKNO.Text.Trim().Length == 0)
            {
                Label_MSG.Text = "分行名稱和轉帳局號不可為空白";
                Label_MSG.ForeColor = Color.Red;
                if (TextBox_BANKNO.Text.Trim().Length == 0)
                    TextBox_BANKNO.BackColor = Color.Orange;
                if (TextBox_NAME.Text.Trim().Length == 0)
                    TextBox_NAME.BackColor = Color.Orange;
            }
            else
            {
                Label_MSG.ForeColor = Color.Red;
                using (SqlConnection insertconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
                {
                    insertconn.Open();
                    try
                    {
                        string insertString = "Insert Into B_BANK (B_SNID,\"ID\",\"NAME\",TEL,FAX,TOUCH,BANKNO,LASTDA,MZ_AD) "
                        + "Values ( NEXT VALUE FOR dbo.B_SET_SN,:\"ID\",:\"NAME\",@TEL,@FAX,@TOUCH,@BANKNO,@LASTDA,@MZ_AD)";
                        //if (!Session["ADPMZ_AD"].Equals(null) && Session["ADPMZ_AD"].ToString() != "")
                        if (strPayAD != "")
                        {
                            SqlCommand cmd = new SqlCommand(insertString, insertconn);
                            cmd.CommandType = CommandType.Text;

                            //cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = TextBox_ID.Text;

                            cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = strBank_ID;
                            cmd.Parameters.Add("NAME", SqlDbType.VarChar).Value = TextBox_NAME.Text;
                            cmd.Parameters.Add("TEL", SqlDbType.VarChar).Value = TextBox_TEL.Text;
                            cmd.Parameters.Add("FAX", SqlDbType.VarChar).Value = TextBox_FAX.Text;
                            cmd.Parameters.Add("TOUCH", SqlDbType.VarChar).Value = TextBox_TOUCH.Text;
                            cmd.Parameters.Add("BANKNO", SqlDbType.VarChar).Value = TextBox_BANKNO.Text;
                            cmd.Parameters.Add("LASTDA", SqlDbType.DateTime).Value = DateTime.Now;
                            cmd.Parameters.Add("MZ_AD", SqlDbType.VarChar).Value = strPayAD;

                            cmd.Connection = insertconn;
                            cmd.ExecuteNonQuery();
                            GridView_BANK.DataBind();
                            Label_MSG.Text = "新增完成";
                            Label_MSG.ForeColor = Color.Blue;
                        }
                        else
                        {
                            Label_MSG.Text = "無權限進行此操作";
                            return;
                        }
                    }
                    catch { Label_MSG.Text = "新增失敗"; }
                    finally { insertconn.Close();
                    //XX2013/06/18 
                    insertconn.Dispose();
                    }
                }
            }
        }

        protected void btUpdate_Click(object sender, EventArgs e)
        {
            using (SqlConnection Updateconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Updateconn.Open();

                string UpdateString = "UPDATE B_BANK SET \"ID\"=:\"ID\",\"NAME\"=:\"NAME\",TEL=@TEL,FAX=@FAX,TOUCH=@TOUCH "
                    + " ,BANKNO=@BANKNO,LASTDA=@LASTDA,MZ_AD=@MZ_AD "
                    + " WHERE B_SNID = '" + Session["B_BANKSelectKeyword"] + "' ";

                SqlCommand cmd = new SqlCommand(UpdateString, Updateconn);

                //cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = TextBox_ID.Text;
                cmd.Parameters.Add("NAME", SqlDbType.VarChar).Value = TextBox_NAME.Text;
                cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = strBank_ID;
                cmd.Parameters.Add("TEL", SqlDbType.VarChar).Value = TextBox_TEL.Text;
                cmd.Parameters.Add("FAX", SqlDbType.VarChar).Value = TextBox_FAX.Text;
                cmd.Parameters.Add("TOUCH", SqlDbType.VarChar).Value = TextBox_TOUCH.Text;
                cmd.Parameters.Add("BANKNO", SqlDbType.VarChar).Value = TextBox_BANKNO.Text;
                cmd.Parameters.Add("LASTDA", SqlDbType.DateTime).Value = DateTime.Now;
                cmd.Parameters.Add("MZ_AD", SqlDbType.VarChar).Value = strPayAD;

                try
                {
                    cmd.ExecuteNonQuery();
                    voidExit_Data();
                    Label_MSG.Text = "更新成功！";
                }
                catch
                {
                    Label_MSG.Text = "更新失敗！";
                }

                Updateconn.Close();
                //XX2013/06/18 
                Updateconn.Dispose();
            }
        }

        protected void btExit_Click(object sender, EventArgs e)
        {
            voidExit_Data();
        }

        protected void GridView_BANK_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "btSelect")
            {
                btCreate.Enabled = false;
                btUpdate.Enabled = true;
                btExit.Visible = true;
                btExit.Enabled = true;

                int index = Convert.ToInt32(e.CommandArgument);
                string strKeyword = GridView_BANK.DataKeys[index].Value.ToString();

                Session["B_BANKSelectKeyword"] = strKeyword;

                using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
                {
                    Selectconn.Open();

                    string strSQL = "SELECT * FROM B_BANK WHERE B_SNID = '" + Session["B_BANKSelectKeyword"] + "' ";
                    SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);

                    Selectcmd.ExecuteNonQuery();

                    SqlDataReader dr = Selectcmd.ExecuteReader(CommandBehavior.SingleRow);

                    if (dr.Read())
                    {
                        //TextBox_ID.Text = dr["ID"].ToString();
                        TextBox_NAME.Text = dr["NAME"].ToString();
                        strBank_ID = dr["ID"].ToString();
                        TextBox_TEL.Text = dr["TEL"].ToString();
                        TextBox_FAX.Text = dr["FAX"].ToString();
                        TextBox_TOUCH.Text = dr["TOUCH"].ToString();
                        TextBox_BANKNO.Text = dr["BANKNO"].ToString();
                    }
                }
            }
        }

        private void voidExit_Data()
        {
            //TextBox_ID.Text = string.Empty;
            //TextBox_NAME.Text = string.Empty;
            strBank_ID = "001";
            TextBox_TEL.Text = string.Empty;
            TextBox_FAX.Text = string.Empty;
            TextBox_TOUCH.Text = string.Empty;
            TextBox_BANKNO.Text = string.Empty;
            btCreate.Enabled = true;
            btUpdate.Enabled = false;
            btExit.Visible = false;
            btExit.Enabled = false;
            GridView_BANK.DataBind();
        }

        protected void GridView_BANK_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DoSearch();
            GridView_BANK.PageIndex = e.NewPageIndex;
        }

        private void DoSearch()
        {
            //群組權限
            string strGroup_Data_Function = (string)ViewState["GroupPermission"];

            //除TPMIDISAdmin、A、B群組可出現完整資料外，其餘只呈現登入者(出納)現服機關資料
            switch (strGroup_Data_Function)
            {
                case "TPMIDISAdmin":
                case "A":
                case "B":
                    SqlDataSource_BANK.SelectCommand = "SELECT B_SNID, BANK_NAME, NAME, TEL, FAX, TOUCH, BANKNO, LASTDA, (SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=MZ_AD) MZ_AD FROM B_BANK JOIN B_BANK_LIST ON ID = BANK_ID ORDER BY ID";
                    break;
                default:
                    SqlDataSource_BANK.SelectCommand = String.Format("SELECT B_SNID, BANK_NAME, NAME, TEL, FAX, TOUCH, BANKNO, LASTDA, (SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=MZ_AD) MZ_AD FROM B_BANK JOIN B_BANK_LIST ON ID = BANK_ID WHERE MZ_AD ='{0}' ORDER BY ID", strPayAD);
                    break;
            }
        }
    }
}
