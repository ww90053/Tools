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
    public partial class B_SalaryBasic2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           

            if (!IsPostBack)
            {
                //檢核頁面存取權限
            SalaryPublic.checkPermission();
                Session.Remove("B_BRANCHSelectKeyword");
                SalaryPublic.fillDropDownList(ref this.DropDownList_PAY_AD);
                btCreate.Enabled = true;
                btUpdate.Enabled = false;
                btExit.Visible = false;
                btExit.Enabled = false;

                divBank.Visible = false;
                GridView_BANK_DATA.Visible = false;

                DropDownList_BANK.DataBind();
            }
            else if (Session["B_BRANCHSelectKeyword"] == null || Session["B_BRANCHSelectKeyword"].ToString() == "")
            {
                //若沒選擇任何一筆分局資料，則為新增模式(修改按鈕disable)
                btCreate.Enabled = true;
                btUpdate.Enabled = false;
            }

            string strGroup_Data_Function;
            //群組權限
            if (Session["ADPMZ_ID"] != null)
            {
                strGroup_Data_Function = SalaryPublic.GetGroupPermission();
            }
            else
            {
                strGroup_Data_Function = "";
            }

            //除TPMIDISAdmin、A、B群組可出現完整資料外，其餘只呈現登入者現服機關資料
            switch (strGroup_Data_Function)
            {
                case "TPMIDISAdmin":
                case "A":
                case "B":
                    SqlDataSource_BRANCH.SelectCommand = "SELECT B_SNID, (SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=ID) ID, IP, TAXUNIT, TAXINVOICE, TAXNAME, TAXADDR, TAXPERS, TICKETNUM, LASTDA FROM B_BRANCH";
                    break;
                default:
                    SqlDataSource_BRANCH.SelectCommand = "SELECT B_SNID, (SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=ID) ID, IP, TAXUNIT, TAXINVOICE, TAXNAME, TAXADDR, TAXPERS, TICKETNUM, LASTDA FROM B_BRANCH WHERE \"ID\"='" + SalaryPublic.strLoginEXAD + "'";
                    break;
            }
            Label_MSG.Text = "";
        }

        private string strPAY_AD
        {
            get
            {
                return DropDownList_PAY_AD.SelectedValue;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    try
                    {
                        DropDownList_PAY_AD.SelectedValue = value;
                    }
                    catch (Exception ex) { Label_MSG.Text = "選取分局時發生錯誤：" + ex.Message; }
                }
            }
        }

        private void fillDropDownList_BankBranch()
        {
            DropDownList_BankBranch.Items.Clear();
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                conn.Open();

                string strSQL = "SELECT B_SNID, NAME FROM B_BANK WHERE ID='" + DropDownList_BANK.SelectedValue + "'";
                DataTable dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "BankBranchNAME");
                foreach (DataRow dr in dt.Rows)
                {
                    ListItem li = new ListItem(dr["NAME"].ToString(), dr["B_SNID"].ToString());
                    DropDownList_BankBranch.Items.Add(li);
                }
            }
        }

        private bool boolCHK_PAY_AD_Data(string strPAY_AD_Data)
        {
            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn.Open();

                string strSQL = "SELECT B_SNID FROM B_BRANCH WHERE \"ID\" = '" + strPAY_AD_Data + "' ";
                SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
                DataTable dt = new DataTable();
                dt.Load(Selectcmd.ExecuteReader(CommandBehavior.CloseConnection));
                if (dt.Rows.Count == 1)
                {
                    if (dt.Rows[0]["B_SNID"] != null && dt.Rows[0]["B_SNID"].ToString() != "")
                    {
                        Session["B_SNID"] = dt.Rows[0]["B_SNID"].ToString();
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        protected void btCreate_Click(object sender, EventArgs e)
        {
            if (Session["B_BRANCHSelectKeyword"] != null && (string)Session["B_BRANCHSelectKeyword"] != "")
            {
                //if (boolCHK_BANK_NAME(Session["B_BRANCHSelectKeyword"].ToString()))
                //{
                // 內層畫面
                if (!IsDataComplete())
                {
                    Label_MSG.Text = "銀行分行必須選擇，如選單中無資料，請到「銀行資料設定」頁面新增";
                    return;
                }

                using (SqlConnection insertconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
                {
                    insertconn.Open();
                    try
                    {
                        string insertString = "Insert Into B_BRANCH_BANK (BB_SNID, B_SNID, BANK_ID, BANK_NAME, FIANCENO, FIANCENO1,MAIL, \"GROUP\") "
                        + "Values ( NEXT VALUE FOR dbo.B_SET_SN, @B_SNID, @BANK_ID, @BANK_NAME, @FIANCENO, @FIANCENO1, @MAIL,@gp)";

                        SqlCommand cmd = new SqlCommand(insertString, insertconn);
                        cmd.CommandType = CommandType.Text;

                        cmd.Parameters.Add("B_SNID", SqlDbType.Float).Value = Session["B_BRANCHSelectKeyword"].ToString();
                        cmd.Parameters.Add("BANK_ID", SqlDbType.VarChar).Value = DropDownList_BankBranch.SelectedValue;
                        cmd.Parameters.Add("BANK_NAME", SqlDbType.VarChar).Value = DropDownList_BankBranch.SelectedItem.Text;
                        cmd.Parameters.Add("FIANCENO", SqlDbType.VarChar).Value = TextBox_FIANCENO.Text;
                        cmd.Parameters.Add("FIANCENO1", SqlDbType.VarChar).Value = TextBox_FIANCENO1.Text;

                        cmd.Parameters.Add("MAIL", SqlDbType.VarChar).Value = txt_email.Text;
                        cmd.Parameters.Add("gp", SqlDbType.VarChar).Value = ddl_group.SelectedValue;

                        cmd.Connection = insertconn;
                        cmd.ExecuteNonQuery();

                        //SqlDataSource_BANK_DATA.SelectCommand = "SELECT * FROM B_BRANCH_BANK WHERE B_SNID = '" + Session["B_BRANCHSelectKeyword"].ToString() + "'";

                        //GridView_BANK_DATA.DataBind();
                        SearchBranchBank();
                        Label_MSG.Text = "新增完成";
                    }
                    catch { Label_MSG.Text = "新增失敗"; }
                    finally { insertconn.Close();
                    //XX2013/06/18 
                    insertconn.Dispose();
                    }
                }
                //}
                //else
                //{
                //    Label_MSG.Text = "新增資料已重覆";
                //}

                divBank.Visible = true;
                GridView_BANK_DATA.Visible = true;

            }
            else// 外層畫面
            {
                if (boolCHK_PAY_AD_Data(strPAY_AD) == false)
                {
                    using (SqlConnection insertconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
                    {
                        insertconn.Open();
                        try
                        {
                            string insertString = "Insert Into B_BRANCH (B_SNID,\"ID\",IP,TAXUNIT,TAXINVOICE,TAXNAME,TAXADDR,TAXPERS,TICKETNUM,LASTDA, \"GROUP\") "
                            + "Values ( NEXT VALUE FOR dbo.B_SET_SN,:\"ID\",@IP,@TAXUNIT,@TAXINVOICE,@TAXNAME,@TAXADDR,@TAXPERS,@TICKETNUM,@LASTDA, @GROUP)";

                            SqlCommand cmd = new SqlCommand(insertString, insertconn);
                            cmd.CommandType = CommandType.Text;

                            //cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = TextBox_ID.Text;
                            //cmd.Parameters.Add("NAME", SqlDbType.VarChar).Value = DropDownList_BankBranch.SelectedValue;
                            cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = DropDownList_PAY_AD.SelectedValue;
                            cmd.Parameters.Add("IP", SqlDbType.VarChar).Value = TextBox_IP.Text;
                            //cmd.Parameters.Add("FIANCENO", SqlDbType.VarChar).Value = TextBox_FIANCENO.Text;
                            //cmd.Parameters.Add("FIANCENO1", SqlDbType.VarChar).Value = TextBox_FIANCENO1.Text;
                            //cmd.Parameters.Add("FIANCENO2", SqlDbType.VarChar).Value = TextBox_FIANCENO2.Text;
                            cmd.Parameters.Add("TAXUNIT", SqlDbType.VarChar).Value = TextBox_TAXUNIT.Text;
                            cmd.Parameters.Add("TAXINVOICE", SqlDbType.VarChar).Value = TextBox_TAXINVOICE.Text;
                            cmd.Parameters.Add("TAXNAME", SqlDbType.VarChar).Value = TextBox_TAXNAME.Text;
                            cmd.Parameters.Add("TAXADDR", SqlDbType.VarChar).Value = TextBox_TAXADDR.Text;
                            cmd.Parameters.Add("TAXPERS", SqlDbType.VarChar).Value = TextBox_TAXPERS.Text;
                            cmd.Parameters.Add("TICKETNUM", SqlDbType.VarChar).Value = TextBox_TICKETNUM.Text;
                            cmd.Parameters.Add("LASTDA", SqlDbType.DateTime).Value = DateTime.Now;
                            cmd.Parameters.Add("GROUP", SqlDbType.VarChar).Value = ddl_group.SelectedValue;

                            cmd.Connection = insertconn;
                            cmd.ExecuteNonQuery();
                            GridView_BRANCH.DataBind();
                            Label_MSG.Text = "新增完成";
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
                    Label_MSG.Text = "新增失敗，分局資料已輸入。";
                }
            }
        }

        protected void btUpdate_Click(object sender, EventArgs e)
        {
            if (Session["B_BRANCH_BANKSelectKeyword"] != null && (string)Session["B_BRANCH_BANKSelectKeyword"] != "")// 內層畫面
            {
                using (SqlConnection Updateconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
                {
                    Updateconn.Open();

                    string UpdateString = "UPDATE B_BRANCH_BANK SET BANK_ID = @BANK_ID, BANK_NAME = @BANK_NAME, FIANCENO = @FIANCENO, FIANCENO1 = @FIANCENO1 ,MAIL=@MAIL"
                        + " WHERE BB_SNID = '" + Session["B_BRANCH_BANKSelectKeyword"] + "' ";

                    SqlCommand cmd = new SqlCommand(UpdateString, Updateconn);

                    cmd.Parameters.Add("BANK_ID", SqlDbType.VarChar).Value = DropDownList_BankBranch.SelectedValue;
                    cmd.Parameters.Add("BANK_NAME", SqlDbType.VarChar).Value = DropDownList_BankBranch.SelectedItem.Text;
                    cmd.Parameters.Add("FIANCENO", SqlDbType.VarChar).Value = TextBox_FIANCENO.Text;
                    cmd.Parameters.Add("FIANCENO1", SqlDbType.VarChar).Value = TextBox_FIANCENO1.Text;
                    cmd.Parameters.Add("MAIL", SqlDbType.VarChar).Value = txt_email.Text;

                    try
                    {
                        cmd.ExecuteNonQuery();

                        Label_MSG.Text = "分局銀行資料更新成功！";
                    }
                    catch
                    {
                        Label_MSG.Text = "分局銀行資料更新失敗！";
                    }
                    finally
                    {
                        Updateconn.Close();
                        //XX2013/06/18 
                        Updateconn.Dispose();
                    }

                    DropDownList_PAY_AD.Enabled = true;
                    TextBox_IP.Enabled = true;
                    TextBox_TAXUNIT.Enabled = true;
                    TextBox_TAXINVOICE.Enabled = true;
                    TextBox_TAXNAME.Enabled = true;
                    TextBox_TAXADDR.Enabled = true;
                    TextBox_TAXPERS.Enabled = true;
                    TextBox_TICKETNUM.Enabled = true;

                    divBank.Visible = true;
                    GridView_BANK_DATA.Visible = true;

                    btUpdate.Enabled = true;
                    btExit.Visible = true;
                    btExit.Enabled = true;

                    //SqlDataSource_BANK_DATA.SelectCommand = "SELECT * FROM B_BRANCH_BANK WHERE B_SNID = '" + Session["B_BRANCHSelectKeyword"].ToString() + "'";
                    //GridView_BANK_DATA.DataBind();
                    SearchBranchBank();
                }

            }
            else if (Session["B_BRANCHSelectKeyword"] != null && (string)Session["B_BRANCHSelectKeyword"] != "")
            {
                using (SqlConnection Updateconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
                {
                    Updateconn.Open();

                    string UpdateString = "UPDATE B_BRANCH SET \"ID\"=:\"ID\",IP=@IP"
                        + " ,TAXUNIT=@TAXUNIT,TAXINVOICE=@TAXINVOICE,TAXNAME=@TAXNAME,TAXADDR=@TAXADDR,TAXPERS=@TAXPERS "
                        + " ,TICKETNUM=@TICKETNUM,LASTDA=@LASTDA "
                        + " WHERE B_SNID = '" + Session["B_BRANCHSelectKeyword"] + "' ";

                    SqlCommand cmd = new SqlCommand(UpdateString, Updateconn);

                    cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = DropDownList_PAY_AD.SelectedValue;
                    cmd.Parameters.Add("IP", SqlDbType.VarChar).Value = TextBox_IP.Text;
                    cmd.Parameters.Add("TAXUNIT", SqlDbType.VarChar).Value = TextBox_TAXUNIT.Text;
                    cmd.Parameters.Add("TAXINVOICE", SqlDbType.VarChar).Value = TextBox_TAXINVOICE.Text;
                    cmd.Parameters.Add("TAXNAME", SqlDbType.VarChar).Value = TextBox_TAXNAME.Text;
                    cmd.Parameters.Add("TAXADDR", SqlDbType.VarChar).Value = TextBox_TAXADDR.Text;
                    cmd.Parameters.Add("TAXPERS", SqlDbType.VarChar).Value = TextBox_TAXPERS.Text;
                    cmd.Parameters.Add("TICKETNUM", SqlDbType.VarChar).Value = TextBox_TICKETNUM.Text;
                    cmd.Parameters.Add("LASTDA", SqlDbType.DateTime).Value = DateTime.Now;

                    try
                    {
                        cmd.ExecuteNonQuery();
                        voidExit_Data();
                        Label_MSG.Text = "分局資料更新成功！";
                    }
                    catch
                    {
                        Label_MSG.Text = "分局資料更新失敗！";
                    }
                    finally
                    {
                        GridView_BRANCH.DataBind();
                        Updateconn.Close();

                        //XX2013/06/18 
                        Updateconn.Dispose();
                    }
                }

            }


        }

        protected void btExit_Click(object sender, EventArgs e)
        {
            if (Session["B_BRANCH_BANKSelectKeyword"] != null && (string)Session["B_BRANCH_BANKSelectKeyword"] != "")
            {
                btCreate.Enabled = true;
                btUpdate.Enabled = true;
                btExit.Visible = true;
                btExit.Enabled = true;

                DropDownList_PAY_AD.Enabled = true;
                TextBox_IP.Enabled = true;
                TextBox_TAXUNIT.Enabled = true;
                TextBox_TAXINVOICE.Enabled = true;
                TextBox_TAXNAME.Enabled = true;
                TextBox_TAXADDR.Enabled = true;
                TextBox_TAXPERS.Enabled = true;
                TextBox_TICKETNUM.Enabled = true;

                divBank.Visible = true;
                GridView_BANK_DATA.Visible = true;

                DropDownList_BANK.DataBind();
                fillDropDownList_BankBranch();
                TextBox_FIANCENO.Text = String.Empty;
                TextBox_FIANCENO1.Text = String.Empty;

                SqlDataSource_BANK_DATA.SelectCommand = "SELECT * FROM B_BRANCH_BANK WHERE B_SNID = '" + Session["B_BRANCHSelectKeyword"].ToString() + "'";
                GridView_BANK_DATA.DataBind();

                Session.Remove("B_BRANCH_BANKSelectKeyword");
            }
            else
            {
                voidExit_Data();
            }

            SetButtonText();
        }

        //選取分局事件
        protected void GridView_BRANCH_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "btSelect")
            {
                //可新增分局銀行資料，可修改分局資料
                btCreate.Enabled = true;
                btUpdate.Enabled = true;
                btExit.Visible = true;
                btExit.Enabled = true;

                divBank.Visible = true;
                GridView_BANK_DATA.Visible = true;
                GridView_BRANCH.Visible = false;

                int index = Convert.ToInt32(e.CommandArgument);
                string strKeyword = GridView_BRANCH.DataKeys[index].Value.ToString();

                Session["B_BRANCHSelectKeyword"] = strKeyword;

                using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
                {
                    Selectconn.Open();

                    string strSQL = "SELECT * FROM B_BRANCH WHERE B_SNID = '" + Session["B_BRANCHSelectKeyword"] + "' ";
                    SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);

                    Selectcmd.ExecuteNonQuery();

                    SqlDataReader dr = Selectcmd.ExecuteReader(CommandBehavior.SingleRow);

                    if (dr.Read())
                    {
                        //TextBox_ID.Text = dr["ID"].ToString();
                        strPAY_AD = dr["ID"].ToString();
                        //DropDownList_BankBranch.SelectedValue = dr["NAME"].ToString();
                        TextBox_IP.Text = dr["IP"].ToString();
                        TextBox_TAXUNIT.Text = dr["TAXUNIT"].ToString();
                        TextBox_TAXINVOICE.Text = dr["TAXINVOICE"].ToString();
                        TextBox_TAXNAME.Text = dr["TAXNAME"].ToString();
                        TextBox_TAXADDR.Text = dr["TAXADDR"].ToString();
                        TextBox_TAXPERS.Text = dr["TAXPERS"].ToString();
                        TextBox_TICKETNUM.Text = dr["TICKETNUM"].ToString();
                    }
                }

                //SqlDataSource_BANK_DATA.SelectCommand = "SELECT * FROM B_BRANCH_BANK WHERE B_SNID = '" + Session["B_BRANCHSelectKeyword"].ToString() + "'";
                SearchBranchBank();
                fillDropDownList_BankBranch();
                DropDownList_BankBranch.SelectedValue = DropDownList_BankBranch.SelectedValue;
            }

            SetButtonText();
        }

        private void voidExit_Data()
        {
            //TextBox_ID.Text = string.Empty;
            //DropDownList_BankBranch.SelectedValue = string.Empty;
            DropDownList_PAY_AD.DataBind();
            TextBox_IP.Text = string.Empty;
            //TextBox_FIANCENO.Text = string.Empty;
            //TextBox_FIANCENO1.Text = string.Empty;
            //TextBox_FIANCENO2.Text = string.Empty;
            TextBox_TAXUNIT.Text = string.Empty;
            TextBox_TAXINVOICE.Text = string.Empty;
            TextBox_TAXNAME.Text = string.Empty;
            TextBox_TAXADDR.Text = string.Empty;
            TextBox_TAXPERS.Text = string.Empty;
            TextBox_TICKETNUM.Text = string.Empty;
            btCreate.Enabled = true;
            //btUpdate.Enabled = false;
            btExit.Visible = false;
            btExit.Enabled = false;

            divBank.Visible = false;
            GridView_BANK_DATA.Visible = false;
            GridView_BRANCH.Visible = true;

            GridView_BRANCH.DataBind();

            Session.Remove("B_BRANCHSelectKeyword");
        }

        private bool boolCHK_BANK_NAME(string strB_SNID)
        {
            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn.Open();

                string strSQL = "SELECT B_SNID FROM B_BRANCH_BANK WHERE B_SNID = '" + strB_SNID + "' ";
                SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
                DataTable dt = new DataTable();
                dt.Load(Selectcmd.ExecuteReader(CommandBehavior.CloseConnection));
                if (dt.Rows.Count == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }

        //選取分局銀行事件
        protected void GridView_BANK_DATA_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "btSelect")
            {
                //可修改分局銀行資料
                btCreate.Enabled = false;
                btUpdate.Enabled = true;
                btExit.Visible = true;
                btExit.Enabled = true;

                DropDownList_PAY_AD.Enabled = false;
                TextBox_IP.Enabled = false;
                TextBox_TAXUNIT.Enabled = false;
                TextBox_TAXINVOICE.Enabled = false;
                TextBox_TAXNAME.Enabled = false;
                TextBox_TAXADDR.Enabled = false;
                TextBox_TAXPERS.Enabled = false;
                TextBox_TICKETNUM.Enabled = false;

                divBank.Visible = true;
                GridView_BANK_DATA.Visible = true;

                Session["B_BRANCH_BANKSelectKeyword"] = e.CommandArgument;

                using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
                {
                    Selectconn.Open();

                    string strSQL = string.Format("SELECT * FROM B_BRANCH_BANK JOIN B_BANK ON B_BANK.B_SNID = B_BRANCH_BANK.BANK_ID WHERE BB_SNID = {0}", Session["B_BRANCH_BANKSelectKeyword"]);

                    SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);

                    Selectcmd.ExecuteNonQuery();

                    SqlDataReader dr = Selectcmd.ExecuteReader(CommandBehavior.SingleRow);

                    if (dr.Read())
                    {
                        DropDownList_BANK.SelectedValue = dr["ID"].ToString();
                        fillDropDownList_BankBranch();
                        DropDownList_BankBranch.SelectedValue = dr["BANK_ID"].ToString();
                        TextBox_FIANCENO.Text = dr["FIANCENO"].ToString();
                        TextBox_FIANCENO1.Text = dr["FIANCENO1"].ToString();
                        ddl_group.SelectedValue = dr["GROUP"].ToString();
                    }
                }
            }
            else if (e.CommandName == "del")
            {
                string sql = "DELETE B_BRANCH_BANK WHERE BB_SNID=@BB_SNID";
                List<SqlParameter> ops = new List<SqlParameter>();

                ops.Add(new SqlParameter("BB_SNID", e.CommandArgument));

                o_DBFactory.ABC_toTest.Edit_Data(sql, ops);
                SearchBranchBank();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('刪除成功');", true);
            }

            SetButtonText();
        }

        protected void DropDownList_BANK_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillDropDownList_BankBranch();
        }

        protected void DropDownList_BankBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList_BankBranch.SelectedValue = DropDownList_BankBranch.SelectedValue;
        }

        // 驗證資料是否齊全
        private bool IsDataComplete()
        {
            if (DropDownList_BankBranch.SelectedIndex == -1)
                return false;
            return true;
        }

        // 開關下方按鈕
        private void SetButtonText()
        {
            // 分局新增
            if (Session["B_BRANCHSelectKeyword"] == null)
            {
                btUpdate.Enabled = false;
                btCreate.Text = "建立分局資料";
                return;
            }

            btUpdate.Enabled = true;
            btCreate.Text = "建立銀行資料";
            // 分局修改和銀行新增
            if (Session["B_BRANCH_BANKSelectKeyword"] == null)
            {
                btUpdate.Text = "修改分局資料";
                return;
            }

            // 銀行修改
            btUpdate.Text = "修改銀行資料";
            return;
        }

        private void SearchBranchBank()
        {
            SqlDataSource_BANK_DATA.SelectCommand = "SELECT BB_SNID, B_BANK_LIST.BANK_NAME BANK_ID, B_BANK.NAME BANK_NAME,"+
"FIANCENO, FIANCENO1, CASE \"GROUP\" WHEN 1 THEN '優惠存款' WHEN 2 THEN '薪資轉帳' WHEN 3 THEN '國宅貸款' WHEN 4 THEN '退撫金貸款' WHEN 5 THEN '分期付款' WHEN 6 THEN '銀行貸款' WHEN 7 THEN '法院扣款' END GROUPNAME ,B_BRANCH_BANK.MAIL " +
            "FROM B_BRANCH_BANK JOIN B_BANK ON B_BANK.B_SNID = B_BRANCH_BANK.BANK_ID JOIN B_BANK_LIST ON ID = B_BANK_LIST.BANK_ID WHERE B_BRANCH_BANK.B_SNID = '" + Session["B_BRANCHSelectKeyword"].ToString() + "'";
            GridView_BANK_DATA.DataBind();
        }
    }
}
