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
    public partial class B_SalaryBasic19 : System.Web.UI.Page
    {
        private static string _strGroup_Data_Function;
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (IsPostBack != true)
            {
                SalaryPublic.checkPermission();
                Label_MSG.Text = "";
                Label_MSG.ForeColor = System.Drawing.Color.Black;

                btExit.Visible = false;
                btExit.Enabled = false;

                Panel_GR.Visible = true;
                Panel_con.Visible = false;

                btCreate.Enabled = false;
                btUpdate.Enabled = false;

                _strGroup_Data_Function = TPMPermissions._strGroupData_ID(int.Parse(Session["TPM_MID"].ToString()), int.Parse(Request.QueryString["TPM_FION"].ToString()));
                switch (_strGroup_Data_Function)
                {
                    case "TPMIDISAdmin":
                    case "A":
                    case "B":
                        GridView_ER.Columns[0].Visible = true;
                        GridView_ER.Columns[1].Visible = true;
                        btC.Enabled = true;
                        break;
                    default:
                        GridView_ER.Columns[0].Visible = false;
                        GridView_ER.Columns[1].Visible = false;
                        btC.Enabled = false;
                        break;
                }
            }
        }

        private string strDAs
        {
            get
            {
                return TextBox_DAs.Text;
            }
            set
            {
                TextBox_DAs.Text = value;
            }
        }

        private string strNOTEs
        {
            get
            {
                return TextBox_NOTEs.Text;
            }
            set
            {
                TextBox_NOTEs.Text = value;
            }
        }

        private string strPAY_ID
        {
            get
            {
                return TextBox_PAY_ID.Text;
            }
            set
            {
                TextBox_PAY_ID.Text = value;
            }
        }

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

        private string strBANK_ID
        {
            get
            {
                return TextBox_BANK_ID.Text;
            }
            set
            {
                TextBox_BANK_ID.Text = value;
            }
        }

        private string strDA
        {
            get
            {
                return TextBox_DA.Text;
            }
            set
            {
                TextBox_DA.Text = value;
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

        private string strNOTE
        {
            get
            {
                return TextBox_NOTE.Text;
            }
            set
            {
                TextBox_NOTE.Text = value;
            }
        }

        private string strPAY_NO
        {
            get
            {
                return TextBox_PAY_NO.Text;
            }
            set
            {
                TextBox_PAY_NO.Text = value;
            }
        }

        private static List<String> lsSQL_PAY_ID = new List<string>();

        protected void btCreate_Click(object sender, EventArgs e)
        {
            if (boolCreate(strPAY_ID, strDA, intPAY, strNOTE, strPAY_NO))
            {
                Label_MSG.Text = "新增完成";
                Label_MSG.ForeColor = System.Drawing.Color.Blue;

                string strSQL = "";
                int intCount = 1;
                foreach (String item in lsSQL_PAY_ID)
                {
                    if (lsSQL_PAY_ID.Count == 1)
                    {
                        strSQL += " AND ER_SNID = '" + item.ToString() + "'";
                    }
                    else
                    {
                        if (intCount == 1)
                        {
                            strSQL += " AND (ER_SNID = '" + item.ToString() + "'";
                        }
                        else if (lsSQL_PAY_ID.Count == intCount)
                        {
                            strSQL += " OR ER_SNID = '" + item.ToString() + "')";
                        }
                        else
                        {
                            strSQL += " OR ER_SNID = '" + item.ToString() + "'";
                        }
                    }
                    intCount++;
                }

                _strER_V = strSQL;
                voidER_V();
            }
            else
            {
                Label_MSG.Text = "新增失敗";
                Label_MSG.ForeColor = System.Drawing.Color.Red;
            }

            foreach (Object item in Panel_con.Controls)
            {
                if (item is TextBox)
                {
                    TextBox tb = ((TextBox)item);
                    switch (tb.ID)
                    {
                        case "TextBox_PAY_ID":
                        case "TextBox_NAME":
                        case "TextBox_BANK_ID":
                        case "TextBox_PAY":
                        case "TextBox_PAY_NO":
                            tb.Text = String.Empty;
                            break;
                    }
                }
            }
            voidSelectControl(TextBox_PAY_ID);
        }

        private static string _strER_V;

        private void voidER_V()
        {
            SqlDataSource_ER_V.SelectCommand = String.Format("SELECT ER_SNID, NAME, DA, PAY, NOTE, PAY_NO FROM B_EXCHANGE_RATE ER, B_EXCHANGE_RATE_BRANCH ERB WHERE ER.PAY_ID = ERB.PAY_ID {0}", _strER_V);
            GridView_ER_V.DataBind();
        }

        protected void btUpdate_Click(object sender, EventArgs e)
        {
            if (boolUpdate(int.Parse(strER_SNID), strPAY_ID, strDA, intPAY, strNOTE, strPAY_NO))
            {
                Label_MSG.Text = "修改成功";
                Label_MSG.ForeColor = System.Drawing.Color.Blue;
                voidER_V();
            }
            else
            {
                Label_MSG.Text = "修改失敗";
                Label_MSG.ForeColor = System.Drawing.Color.Red;
            }
        }

        private static string strER_SNID;

        protected void GridView_ER_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int intKeyword = Convert.ToInt32(e.CommandArgument);
            string strKeyword = GridView_ER.DataKeys[intKeyword].Value.ToString();

            if (e.CommandName == "btUpdate")
            {
                strER_SNID = strKeyword;
                voidSelect(int.Parse(strKeyword));
                Panel_GR.Visible = false;
                Panel_con.Visible = true;
                btCreate.Enabled = false;
                btUpdate.Enabled = true;
                btExit.Enabled = true;
            }

            if (e.CommandName == "btDelete")
            {
                if (boolDelete(int.Parse(strKeyword)))
                {
                    Label_MSG.Text = "刪除成功";
                    Label_MSG.ForeColor = System.Drawing.Color.Blue;
                }
                else
                {
                    Label_MSG.Text = "刪除失敗";
                    Label_MSG.ForeColor = System.Drawing.Color.Red;
                }
                GridView_ER.DataBind();
                GridView_ER_V.DataBind();
            }

        }

        protected void GridView_ER_V_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int intKeyword = Convert.ToInt32(e.CommandArgument);
            string strKeyword = GridView_ER_V.DataKeys[intKeyword].Value.ToString();

            if (e.CommandName == "btUpdate")
            {
                strER_SNID = strKeyword;
                btCreate.Enabled = false;
                btUpdate.Enabled = true;
                btExit.Enabled = true;
                voidSelect(int.Parse(strKeyword));
            }
        }

        private void voidSelect(int intER_SNID)
        {
            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn.Open();

                string strSQL = "SELECT ER.PAY_ID AS PAY_ID, \"NAME\", BANK_ID, DA, PAY, NOTE, PAY_NO FROM B_EXCHANGE_RATE ER, B_EXCHANGE_RATE_BRANCH ERB WHERE ER.PAY_ID = ERB.PAY_ID AND ER_SNID = '" + intER_SNID + "' ";
                SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
                Selectcmd.ExecuteNonQuery();
                SqlDataReader dr = Selectcmd.ExecuteReader(CommandBehavior.SingleRow);

                if (dr.Read())
                {
                    btExit.Visible = true;

                    strPAY_ID = dr["PAY_ID"].ToString();
                    strNAME = dr["NAME"].ToString();
                    strBANK_ID = dr["BANK_ID"].ToString();
                    strDA = dr["DA"].ToString();
                    intPAY = int.Parse(dr["PAY"].ToString());
                    strNOTE = dr["NOTE"].ToString();
                    strPAY_NO = dr["PAY_NO"].ToString();
                }
            }
        }

        private bool boolCreate(string strPAY_ID_Data, string strDA_Data, int intPAY_Data, string strNOTE_Data, string strPAY_NO_Data)
        {
            int intNEXTsVAL_Data = intNEXTsVAL();
            using (SqlConnection insertconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                insertconn.Open();
                try
                {
                    string insertString = "Insert Into B_EXCHANGE_RATE (ER_SNID, PAY_ID, DA, PAY, NOTE, PAY_NO)"
                    + " Values ('" + intNEXTsVAL_Data + "', '" + strPAY_ID_Data + "', '" + strDA_Data + "', '" + intPAY_Data + "', '" + strNOTE_Data + "', '" + strPAY_NO_Data + "')";

                    SqlCommand cmd = new SqlCommand(insertString, insertconn);
                    cmd.CommandType = CommandType.Text;

                    cmd.Connection = insertconn;
                    cmd.ExecuteNonQuery();

                    lsSQL_PAY_ID.Add(intNEXTsVAL_Data.ToString());
                    return true;
                }
                catch { return false; }
                finally { insertconn.Close();
                //XX2013/06/18 
                insertconn.Dispose();
                }
            }
        }

        private bool boolUpdate(int intER_SNID, string strPAY_ID_Data, string strDA_Data, int intPAY_Data, string strNOTE_Data, string strPAY_NO_Data)
        {
            using (SqlConnection UpdateConn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                UpdateConn.Open();
                try
                {
                    string UpdateString = "UPDATE B_EXCHANGE_RATE SET PAY_ID = '" + strPAY_ID_Data + "', DA = '" + strDA + "', PAY = '" + intPAY_Data + "', NOTE = '" + strNOTE_Data + "', PAY_NO = '" + strPAY_NO_Data + "' WHERE ER_SNID = '" + intER_SNID + "'";

                    SqlCommand cmd = new SqlCommand(UpdateString, UpdateConn);
                    cmd.CommandType = CommandType.Text;

                    cmd.Connection = UpdateConn;
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch { return false; }
                finally { UpdateConn.Close();
                //XX2013/06/18 
                UpdateConn.Dispose();
                }
            }
        }

        private bool boolDelete(int intER_SNID)
        {
            using (SqlConnection deleteconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                deleteconn.Open();
                try
                {
                    string strSQL_Del = "DELETE B_EXCHANGE_RATE WHERE ER_SNID = '" + intER_SNID + "'";
                    SqlCommand Selectcmd_Del = new SqlCommand(strSQL_Del, deleteconn);
                    DataTable dt_Del = new DataTable();
                    dt_Del.Load(Selectcmd_Del.ExecuteReader());
                    return true;
                }
                catch { return false; }
                finally { deleteconn.Close();
                //XX2013/06/18 
                deleteconn.Dispose();
                }
            }
        }

        protected void CheckBox_JOIN_CheckedChanged(object sender, EventArgs e)
        {
            voidCheckBox_JOIN();
        }

        protected void TextBox_PAY_ID_TextChanged(object sender, EventArgs e)
        {
            voidChanged(sender, e);
            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn.Open();

                string strSQL = "SELECT \"NAME\", BANK_ID FROM B_EXCHANGE_RATE_BRANCH WHERE PAY_ID = '" + strPAY_ID + "'";
                SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
                Selectcmd.ExecuteNonQuery();
                SqlDataReader dr = Selectcmd.ExecuteReader(CommandBehavior.SingleRow);
                if (dr.Read())
                {
                    strNAME = dr["NAME"].ToString();
                    strBANK_ID = dr["BANK_ID"].ToString();
                }
            }
        }

        protected void btC_Click(object sender, EventArgs e)
        {
            if (Panel_GR.Visible)
            {
                Panel_GR.Visible = false;
                Panel_con.Visible = true;

                if (String.IsNullOrEmpty(strER_SNID))
                {
                    btCreate.Enabled = true;
                    strDAs = (DateTime.Now.Year - 1911).ToString("000") + DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00");
                }
                else
                {
                    btUpdate.Enabled = true;
                }

            }
            else
            {
                Panel_GR.Visible = true;
                Panel_con.Visible = false;

                btCreate.Enabled = false;
                btUpdate.Enabled = false;

                GridView_ER.DataBind();
            }
        }

        private int intNEXTsVAL()
        {
            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn.Open();
                try
                {
                    string strSQL = "SELECT  NEXT VALUE FOR dbo.B_EXCHANGE_RATE_SN  NEXT VALUE FOR dbo.AS ";
                    SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
                    DataTable dt = new DataTable();
                    dt.Load(Selectcmd.ExecuteReader());
                    if (dt.Rows.Count == 1)
                    {
                        return int.Parse(dt.Rows[0]["NEXTVAL"].ToString());
                    }
                    else
                    { return 0; }
                }
                catch (Exception) { throw; }
                finally { Selectconn.Close();
                //XX2013/06/18 
                Selectconn.Dispose();
                }
            }
        }

        protected void voidChanged(object sender, EventArgs e)
        {
            switch (((TextBox)sender).ID)
            {
                case "TextBox_DAs":
                    voidSelectControl(TextBox_NOTEs);
                    break;
                case "TextBox_NOTEs":
                    voidSelectControl(TextBox_PAY_ID);
                    break;
                case "TextBox_PAY_ID":
                    voidSelectControl(TextBox_PAY);
                    break;
                case "TextBox_PAY":
                    voidSelectControl(TextBox_PAY_NO);
                    break;
                case "TextBox_PAY_NO":
                    voidSelectControl(btCreate);
                    break;
            }
        }

        private void voidSelectControl(object ojNext)
        {
            if (ojNext is Button)
            {
                ((Button)ojNext).Focus();
            }
            else if (ojNext is TextBox)
            {
                ((TextBox)ojNext).Focus();
            }
            else if (ojNext is DropDownList)
            {
                ((DropDownList)ojNext).Focus();
            }
            else if (ojNext is RadioButtonList)
            {
                ((RadioButtonList)ojNext).Focus();
            }
        }

        protected void btExit_Click(object sender, EventArgs e)
        {
            strER_SNID = String.Empty;
            if (Panel_GR.Visible)
            {
                btCreate.Enabled = false;
            }
            else
            {
                btCreate.Enabled = true;
            }
            btUpdate.Enabled = false;
            btExit.Visible = false;
            btExit.Enabled = false;
            foreach (Object item in Panel_con.Controls)
            {
                if (item is TextBox)
                {
                    TextBox tb = (TextBox)item;
                    switch (tb.ID)
                    {
                        case "TextBox_DAs":
                        case "TextBox_NOTEs":
                            break;
                        case "TextBox_DA":
                        case "TextBox_NOTE":
                            if (CheckBox_JOIN.Checked == false)
                            {
                                tb.Text = String.Empty;
                            }
                            else
                            {
                                strDA = strDAs;
                                strNOTE = strNOTEs;
                            }
                            break;
                        default:
                            tb.Text = String.Empty;
                            break;
                    }
                }
            }
            voidSelectControl(TextBox_PAY_ID);
            //strDAs = (DateTime.Now.Year - 1911).ToString("000") + DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00");
        }

        protected void TextBox_DAs_TextChanged(object sender, EventArgs e)
        {
            voidCheckBox_JOIN();
            voidChanged(sender, e);
        }

        private void voidCheckBox_JOIN()
        {
            if (CheckBox_JOIN.Checked)
            {
                strDA = strDAs;
                strNOTE = strNOTEs;
            }
            else
            {
                strDA = String.Empty;
                strNOTE = String.Empty;
            }
        }







    }
}
