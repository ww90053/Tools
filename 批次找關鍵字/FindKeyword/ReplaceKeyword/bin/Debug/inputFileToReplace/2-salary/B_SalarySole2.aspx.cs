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
    public partial class B_SalarySole2 : System.Web.UI.Page
    {
        string group
        {
            get { return ViewState["group"].ToString(); }
            set { ViewState["group"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
           

            if (!IsPostBack)
            { //檢查權限
            SalaryPublic.checkPermission();
                Label_MSG.Text = "";
                group = SalaryPublic.GetGroupPermission();

                //C權限只能瀏覽資料
                if (group == "C")
                {
                    btCreate.Enabled = false;
                    btUpdate.Enabled = false;
                    btDelete.Enabled = false;
                    btExit.Enabled = false;
                }
                else
                {
                    btCreate.Enabled = true;
                    btUpdate.Enabled = false;
                    btDelete.Enabled = false;
                    btExit.Enabled = false;
                }

                //依權限裝填所得下拉控制項
                SalaryPublic.fillTaxTypeDropDownList(ref DropDownListTAXES_TYPE);
                ListItem li = new ListItem("全部", "-1");
                DropDownListTAXES_TYPE.Items.Insert(0, li);

                DoSearch();
            }
        }

        //單一發放代碼
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

        //單一發放名稱
        private string strNAME
        {
            get
            {
                return TextBox_NAME.Text;
            }
            set
            {
                TextBox_NAME.Text = value;
            }
        }

        //是否納入所得
        private string strTAXES_YESNO
        {
            get
            {
                return RadioButtonList_TAXES_YESNO.SelectedValue;
            }
            set
            {
                RadioButtonList_TAXES_YESNO.SelectedValue = value;
            }
        }

        //所得格式代碼
        private string strTAXES_ID
        {
            get
            {
                return DropDownListTAXES_TYPE.SelectedValue;
            }
            set
            {
                if (value != "")
                    DropDownListTAXES_TYPE.SelectedValue = value;
            }
        }

        //所得機關
        private string strTAXES_AD
        {
            get
            {
                return RadioButtonList_TAXES_AD.SelectedValue;
            }
            set
            {
                RadioButtonList_TAXES_AD.SelectedValue = value;
            }
        }

        //所得單位
        private string strTAXES_UNIT
        {
            get
            {
                return RadioButtonList_TAXES_UNIT.SelectedValue;
            }
            set
            {
                RadioButtonList_TAXES_UNIT.SelectedValue = value;
            }
        }

        private static List<String> listStrData = new List<string>();

        //查詢
        protected void btTable_Click(object sender, EventArgs e)
        {
            DoSearch();
            Label_MSG.Text = "查詢完成";
        }

        //新增
        protected void btCreate_Click(object sender, EventArgs e)
        {
            if (strID.Length > 0 && strNAME.Length > 0)
            {
                if (bool_CHK_ID_Data(strID) == false)
                {
                    if (strTAXES_YESNO.Equals("Y"))
                    {
                        if (strTAXES_ID.Length > 0)
                        {
                            voidCreate();
                        }
                        else
                        {
                            Label_MSG.Text = "請選擇所得格式";
                        }
                    }
                    else
                    {
                        voidCreate();
                    }
                }
                else
                {
                    Label_MSG.Text = "代碼已重覆";
                }
            }
            else
            {
                Label_MSG.Text = "請輸入代碼及名稱。";
            }
        }

        private void voidCreate()
        {
            using (SqlConnection insertconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                insertconn.Open();
                try
                {
                    string insertString;
                    SqlCommand cmd;

                    //納入所得才有所得格式資料
                    if (RadioButtonList_TAXES_YESNO.SelectedValue == "Y")
                    {
                        insertString = "Insert Into B_SOLEITEM (\"ID\",\"NAME\",TAXES_YESNO,TAXES_ID,TAXES_AD,TAXES_UNIT,LASTDA) "
                        + "Values (:\"ID\",:\"NAME\",@TAXES_YESNO,@TAXES_ID,@TAXES_AD,@TAXES_UNIT,@LASTDA)";

                        cmd = new SqlCommand(insertString, insertconn);
                        cmd.CommandType = CommandType.Text;

                        cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = strID;
                        cmd.Parameters.Add("NAME", SqlDbType.VarChar).Value = strNAME;
                        cmd.Parameters.Add("TAXES_YESNO", SqlDbType.VarChar).Value = strTAXES_YESNO;
                        cmd.Parameters.Add("TAXES_ID", SqlDbType.VarChar).Value = strTAXES_ID;
                        cmd.Parameters.Add("TAXES_AD", SqlDbType.Float).Value = strTAXES_AD;
                        cmd.Parameters.Add("TAXES_UNIT", SqlDbType.Float).Value = strTAXES_UNIT;
                        cmd.Parameters.Add("LASTDA", SqlDbType.DateTime).Value = DateTime.Now;
                    }
                    else//不納入所得，所得格式為空值
                    {
                        insertString = "Insert Into B_SOLEITEM (\"ID\",\"NAME\",TAXES_YESNO,TAXES_AD,TAXES_UNIT,LASTDA) "
                        + "Values (:\"ID\",:\"NAME\",@TAXES_YESNO,@TAXES_AD,@TAXES_UNIT,@LASTDA)";

                        cmd = new SqlCommand(insertString, insertconn);
                        cmd.CommandType = CommandType.Text;

                        cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = strID;
                        cmd.Parameters.Add("NAME", SqlDbType.VarChar).Value = strNAME;
                        cmd.Parameters.Add("TAXES_YESNO", SqlDbType.VarChar).Value = strTAXES_YESNO;
                        //cmd.Parameters.Add("TAXES_ID", SqlDbType.VarChar).Value = strTAXES_ID;
                        cmd.Parameters.Add("TAXES_AD", SqlDbType.Float).Value = strTAXES_AD;
                        cmd.Parameters.Add("TAXES_UNIT", SqlDbType.Float).Value = strTAXES_UNIT;
                        cmd.Parameters.Add("LASTDA", SqlDbType.DateTime).Value = DateTime.Now;
                    }

                    cmd.Connection = insertconn;
                    cmd.ExecuteNonQuery();

                    Label_MSG.Text = "新增完成";
                    DoSearch();
                }
                catch { Label_MSG.Text = "新增失敗"; }
                finally { insertconn.Close();
                //XX2013/06/18 
                insertconn.Dispose();
                }
            }
        }

        //修改
        protected void btUpdate_Click(object sender, EventArgs e)
        {
            using (SqlConnection Updateconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Updateconn.Open();

                string UpdateString;
                SqlCommand cmd;

                //納入所得才有所得格式資料
                if (RadioButtonList_TAXES_YESNO.SelectedValue == "Y")
                {
                    UpdateString = "UPDATE B_SOLEITEM SET \"NAME\"=:\"NAME\",TAXES_YESNO=@TAXES_YESNO,TAXES_ID=@TAXES_ID,TAXES_AD=@TAXES_AD,TAXES_UNIT=@TAXES_UNIT,LASTDA=@LASTDA "
                        + " WHERE \"ID\" = '" + Session["B_SOLEITEMSelectKeyword"] + "' ";

                    cmd = new SqlCommand(UpdateString, Updateconn);

                    cmd.Parameters.Add("NAME", SqlDbType.VarChar).Value = strNAME;
                    cmd.Parameters.Add("TAXES_YESNO", SqlDbType.VarChar).Value = strTAXES_YESNO;
                    cmd.Parameters.Add("TAXES_ID", SqlDbType.VarChar).Value = strTAXES_ID;
                    cmd.Parameters.Add("TAXES_AD", SqlDbType.Float).Value = strTAXES_AD;
                    cmd.Parameters.Add("TAXES_UNIT", SqlDbType.Float).Value = strTAXES_UNIT;
                    cmd.Parameters.Add("LASTDA", SqlDbType.DateTime).Value = DateTime.Now;
                }
                else//不納入所得，所得格式為空值
                {
                    UpdateString = "UPDATE B_SOLEITEM SET \"NAME\"=:\"NAME\",TAXES_YESNO=@TAXES_YESNO,TAXES_ID=@TAXES_ID,TAXES_AD=@TAXES_AD,TAXES_UNIT=@TAXES_UNIT,LASTDA=@LASTDA "
                        + " WHERE \"ID\" = '" + Session["B_SOLEITEMSelectKeyword"] + "' ";

                    cmd = new SqlCommand(UpdateString, Updateconn);

                    cmd.Parameters.Add("NAME", SqlDbType.VarChar).Value = strNAME;
                    cmd.Parameters.Add("TAXES_YESNO", SqlDbType.VarChar).Value = strTAXES_YESNO;
                    cmd.Parameters.Add("TAXES_ID", SqlDbType.VarChar).Value = string.Empty;
                    cmd.Parameters.Add("TAXES_AD", SqlDbType.Float).Value = strTAXES_AD;
                    cmd.Parameters.Add("TAXES_UNIT", SqlDbType.Float).Value = strTAXES_UNIT;
                    cmd.Parameters.Add("LASTDA", SqlDbType.DateTime).Value = DateTime.Now;
                }

                try
                {
                    cmd.ExecuteNonQuery();
                    voidExit_Data();
                    Label_MSG.Text = "更新成功！";
                    DoSearch();
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

        //刪除
        protected void btDelete_Click(object sender, EventArgs e)
        {
            using (SqlConnection Deleteconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Deleteconn.Open();
                string DeleteString = "DELETE B_SOLEITEM WHERE \"ID\" = '" + Session["B_SOLEITEMSelectKeyword"] + "'";
                SqlCommand cmd = new SqlCommand(DeleteString, Deleteconn);

                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                try
                {
                    cmd.ExecuteNonQuery();
                    voidExit_Data();
                    Label_MSG.Text = "刪除成功！";
                    DoSearch();
                }
                catch
                {
                    Label_MSG.Text = "刪除失敗！";
                    btUpdate.Enabled = true;
                    btDelete.Enabled = true;
                }
                finally
                {
                    Deleteconn.Close();

                    //XX2013/06/18 
                    Deleteconn.Dispose();
                }
            }
        }

        //行為取消
        protected void btExit_Click(object sender, EventArgs e)
        {
            voidExit_Data();
        }

        //初始化控制項
        private void voidExit_Data()
        {
            Label_MSG.Text = "";
            strID = "";
            strNAME = "";
            strTAXES_YESNO = "Y";
            strTAXES_ID = "50";
            TextBox_ID.Enabled = true;

            btTable.Enabled = true;
            btCreate.Enabled = true;
            btUpdate.Enabled = false;
            btDelete.Enabled = false;
            btExit.Enabled = false;

            string strData = string.Format("SELECT \"ID\", \"NAME\", TAXES_YESNO, TAXES_ID, CASE WHEN (TAXES_AD) = '1' THEN '發薪機關' WHEN (TAXES_AD) = '2' THEN '編制機關' ELSE '現服機關' END AS TAXES_AD, CASE WHEN (TAXES_UNIT) = '1' THEN '編制單位' ELSE '現服單位' END AS TAXES_UNIT FROM B_SOLEITEM {0}", listStrData.Count > 0 ? " WHERE " + string.Join(" AND ", listStrData.ToArray()) : string.Empty);
            SqlDataSource_SINGLEITEM.SelectCommand = strData;
            GridView_SINGLEITEM.DataBind();
        }

        //選取GridView中某一筆資料，進入修改模式
        protected void GridView_SINGLEITEM_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "btSelect")
            {
                btTable.Enabled = false;
                btCreate.Enabled = false;
                btUpdate.Enabled = true;
                btDelete.Enabled = true;
                btExit.Enabled = true;
                TextBox_ID.Enabled = false;

                int index = Convert.ToInt32(e.CommandArgument);
                string strKeyword = GridView_SINGLEITEM.DataKeys[index].Value.ToString();
                Session["B_SOLEITEMSelectKeyword"] = strKeyword;

                using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
                {
                    Selectconn.Open();

                    //取得選取的資料
                    string strSQL = "SELECT * FROM B_SOLEITEM WHERE \"ID\" = '" + Session["B_SOLEITEMSelectKeyword"] + "' ";
                    SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);

                    Selectcmd.ExecuteNonQuery();

                    SqlDataReader dr = Selectcmd.ExecuteReader(CommandBehavior.SingleRow);

                    if (dr.Read())
                    {
                        strID = dr["ID"].ToString();
                        strNAME = dr["NAME"].ToString();
                        strTAXES_YESNO = dr["TAXES_YESNO"].ToString();
                        strTAXES_ID = dr["TAXES_ID"].ToString();
                        strTAXES_AD = dr["TAXES_AD"].ToString();
                        strTAXES_UNIT = dr["TAXES_UNIT"].ToString();
                    }
                }
            }
        }

        //該ID是否已存在
        private bool bool_CHK_ID_Data(string strID)
        {
            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn.Open();
                try
                {
                    string strSQL = "SELECT * FROM B_SOLEITEM WHERE \"ID\" = '" + strID + "'";
                    SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
                    DataTable dt = new DataTable();
                    dt.Load(Selectcmd.ExecuteReader(CommandBehavior.CloseConnection));
                    if (dt.Rows.Count == 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch { return false; }
                finally { Selectconn.Close();
                //XX2013/06/18 
                Selectconn.Dispose();
                }
            }
        }

        protected void RadioButtonList_TAXES_YESNO_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (strTAXES_YESNO == "N")
            {
                //strTAXES_ID = strTAXES_ID;
                DropDownListTAXES_TYPE.Enabled = false;
            }
            else
            {
                DropDownListTAXES_TYPE.Enabled = true;
            }
        }

        private void DoSearch()
        {
            listStrData.Clear();
            if (strID.Length > 0)
            {
                listStrData.Add("\"ID\" = '" + strID + "'");
            }
            if (strNAME.Length > 0)
            {
                listStrData.Add("\"NAME\" = '" + strNAME + "'");
            }
            //if (strTAXES_YESNO.Length > 0)
            //{
            //    listStrData.Add("TAXES_YESNO = '" + strTAXES_YESNO + "'");
            //}
            if (strTAXES_ID.Length > 0 && strTAXES_ID != "-1")
            {
                listStrData.Add("TAXES_ID = '" + strTAXES_ID + "'");
            }
            listStrData.Add("TAXES_AD = '" + strTAXES_AD + "'");
            listStrData.Add("TAXES_UNIT = '" + strTAXES_AD + "'");

            string strData = string.Format("SELECT \"ID\", \"NAME\", TAXES_YESNO, TAXES_ID, CASE WHEN (TAXES_AD) = '1' THEN '發薪機關' WHEN (TAXES_AD) = '2' THEN '編制機關' ELSE '現服機關' END AS TAXES_AD, CASE WHEN (TAXES_UNIT) = '1' THEN '編制單位' ELSE '現服單位' END AS TAXES_UNIT FROM B_SOLEITEM {0} ORDER BY \"ID\"", listStrData.Count > 0 ? " WHERE " + string.Join(" AND ", listStrData.ToArray()) : string.Empty);
            SqlDataSource_SINGLEITEM.SelectCommand = strData;
            GridView_SINGLEITEM.DataBind();
        }

        protected void GridView_SINGLEITEM_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
                return;

            LinkButton ctrl = (LinkButton)e.Row.Cells[4].Controls[0];
            if (group == "C")
                ctrl.Enabled = false;
            else
                ctrl.Enabled = true;
        }
    }
}
