using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Data;
using AjaxControlToolkit;
using System.Drawing;
using TPPDDB.Logic;

namespace TPPDDB._3_forleave
{
    public partial class C_ForLeaveOvertime_DUTY_Limit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                C.check_power();
            
            //by MQ 20100312---------   
            C.set_Panel_EnterToTAB(ref this.Panel1);

            
                Label1.Text = "超勤金額小時上限設定";
                C.controlEnable(ref this.Panel1, false);
            }
        }

        protected void TextBox_MZ_AD_TextChanged(object sender, EventArgs e)
        {
            string CName = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)='38213' UNION ALL SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)<>'38213') WHERE MZ_KCODE='" + TextBox_MZ_AD.Text.Trim().ToUpper() + "'");
            //Ktype_Cname_Check(CName, TextBox_MZ_AD1, TextBox_MZ_AD, TextBox_MZ_UNIT);
            TextBox_MZ_AD.Text = TextBox_MZ_AD.Text.ToUpper();
            //檢查代碼中文名稱是否存在
            if (string.IsNullOrEmpty(CName))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('查無資料')", true);
                TextBox_MZ_AD1.Text = string.Empty;
                TextBox_MZ_AD.Text = string.Empty;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_AD.ClientID + "').focus();$get('" + TextBox_MZ_AD.ClientID + "').focus();", true);
            }
            else
            {
                TextBox_MZ_AD1.Text = CName;

            }
        }

        protected void TextBox_MZ_UNIT_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_UNIT.Text, "25");
                       
            TextBox_MZ_UNIT.Text = TextBox_MZ_UNIT.Text.ToUpper();
            //檢查代碼中文名稱是否存在
            if (string.IsNullOrEmpty(CName))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('查無資料')", true);
                TextBox_MZ_UNIT1.Text = string.Empty;
                TextBox_MZ_UNIT.Text = string.Empty;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_UNIT.ClientID + "').focus();$get('" + TextBox_MZ_UNIT.ClientID + "').focus();", true);
            }
            else
            {
                TextBox_MZ_UNIT1.Text = CName;

            }
        }

        protected void btUNIT_Click(object sender, EventArgs e)
        {
            Session["KTYPE_CID"] = TextBox_MZ_UNIT.ClientID;
            Session["KTYPE_CID1"] = TextBox_MZ_UNIT1.ClientID;
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('../1-personnel/Personal_Ktype_Search.aspx?CID_NAME=UNIT&AD=" + TextBox_MZ_AD.Text.Trim().ToUpper() + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin','top=190,left=200,width=420,height=450,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }

        protected void btAD_Click(object sender, EventArgs e)
        {
            Session["KTYPE_CID"] = TextBox_MZ_AD.ClientID;
            Session["KTYPE_CID1"] = TextBox_MZ_AD1.ClientID;

            ///20150429 Neil : 修正CID_NAME為 ADUD (僅撈取新北市警察局 代碼為38213開頭之資料)
            ///此部分代碼，在 Personal_Ktype_Search 有針對各種 CID_NAME 做撈取SQL的判斷
            ///其可撈出不同群組情況下的使用者清單資料
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('../1-personnel/Personal_Ktype_Search.aspx?CID_NAME=ADAU&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin','top=190,left=200,width=420,height=450,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }

        protected void TextBox_MZ_ID_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_ID FROM A_DLBASE WHERE MZ_ID='" + TextBox_MZ_ID.Text + "'")))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('查無資料');", true);
            }
            else
            {
                string strSQL = @"SELECT MZ_NAME+'   '+(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=A_DLBASE.MZ_OCCC AND MZ_KTYPE='26') 
                                    FROM A_DLBASE WHERE 1=1 ";
                if (!string.IsNullOrEmpty(TextBox_MZ_AD.Text))
                {
                    strSQL += string.Format(" And MZ_AD = '{0}' ", TextBox_MZ_AD.Text.Trim().ToUpper());
                }
                if (!string.IsNullOrEmpty(TextBox_MZ_UNIT.Text))
                {
                    strSQL += string.Format(" And MZ_UNIT = '{0}' ", TextBox_MZ_UNIT.Text.Trim());
                }
                strSQL += string.Format(" And MZ_ID = '{0}' ", TextBox_MZ_ID.Text.Trim().ToUpper());
                string S = o_DBFactory.ABC_toTest.vExecSQL(strSQL);

                if (string.IsNullOrEmpty(S))
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('該員不在此單位');", true);
                }
                else
                {
                    Label2.Text = S;
                }
            }
        }

        protected void execSQL()
        {
            //2010.06.07 by 伊珊
            string ErrorString = "";

            string pkey_check = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM C_DUTYLIMIT WHERE MZ_AD='" + TextBox_MZ_AD.Text.Trim() + "' AND MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "' AND MZ_UNIT='" + TextBox_MZ_UNIT.Text.Trim() + "'");
            string statuString = ViewState["Mode"].ToString();

            if (pkey_check != "0")
            {
                if (statuString != "UPDATE")
                {
                    ErrorString += "機關與單位與身分證號違反唯一值的條件" + "\\r\\n";
                    TextBox_MZ_AD.BackColor = Color.Orange;
                    TextBox_MZ_UNIT.BackColor = Color.Orange;
                    TextBox_MZ_ID.BackColor = Color.Orange;
                }
            }
            else
            {
                TextBox_MZ_AD.BackColor = Color.White;
                TextBox_MZ_UNIT.BackColor = Color.White;
                TextBox_MZ_ID.BackColor = Color.White;
            }

            if (!string.IsNullOrEmpty(ErrorString))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('" + ErrorString + "請更改不可重複欄位與輸入變色之欄位');", true);
                return;
            }

            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                if (!string.IsNullOrEmpty(TextBox_MZ_HOUR_LIMIT.Text.Trim()) || !string.IsNullOrEmpty(TextBox_MZ_MONEY_LIMIT.Text.Trim()))
                {
                    //if (!string.IsNullOrEmpty(TextBox_MZ_HOUR_LIMIT.Text.Trim()) && !string.IsNullOrEmpty(TextBox_MZ_MONEY_LIMIT.Text.Trim()))
                    //{
                    //    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('時間與金額上限只能設定一個!');", true);
                    //}
                    //else
                    //{
                        conn.Open();
                        SqlCommand cmd = new SqlCommand(ViewState["CMDSQL"].ToString(), conn);

                        //Joy 判斷資料是否有重複
                        string selectDouble = "SELECT COUNT(MZ_AD) FROM C_DUTYLIMIT WHERE MZ_AD = @MZ_AD AND MZ_UNIT = @MZ_UNIT AND MZ_ID = @MZ_ID";

                        List<SqlParameter> para = new List<SqlParameter>();
                        para.Add(new SqlParameter("MZ_AD", SqlDbType.NVarChar) { Value = TextBox_MZ_AD.Text.Trim().ToUpper() });
                        para.Add(new SqlParameter("MZ_UNIT", SqlDbType.NVarChar) { Value = TextBox_MZ_UNIT.Text.Trim().ToUpper() });
                        para.Add(new SqlParameter("MZ_ID", SqlDbType.NVarChar) { Value = string.IsNullOrEmpty(TextBox_MZ_ID.Text.Trim().ToUpper()) ? "NULL" : TextBox_MZ_ID.Text.Trim().ToUpper() });


                        string countNum = o_DBFactory.ABC_toTest.GetValue(selectDouble, para);
                        int doubleNum = int.Parse(countNum);

                        if (doubleNum > 0 && ViewState["Mode"].ToString() != "UPDATE")
                        {
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('資料重複！');", true);
                            conn.Close();

                            //XX2013/06/18 
                            conn.Dispose();
                        }
                        else
                        {
                            try
                            {

                                cmd.Parameters.Add("MZ_AD", SqlDbType.VarChar).Value = TextBox_MZ_AD.Text.Trim().ToUpper(); ;
                                cmd.Parameters.Add("MZ_UNIT", SqlDbType.VarChar).Value = TextBox_MZ_UNIT.Text.Trim().ToUpper();
                                cmd.Parameters.Add("MZ_ID", SqlDbType.VarChar).Value = string.IsNullOrEmpty(TextBox_MZ_ID.Text.Trim().ToUpper()) ? "NULL" : TextBox_MZ_ID.Text.Trim().ToUpper();
                                cmd.Parameters.Add("MZ_HOUR_LIMIT", SqlDbType.Float).Value = string.IsNullOrEmpty(TextBox_MZ_HOUR_LIMIT.Text) ? Convert.DBNull : int.Parse(TextBox_MZ_HOUR_LIMIT.Text.Trim());
                                cmd.Parameters.Add("MZ_MONEY_LIMIT", SqlDbType.Float).Value = string.IsNullOrEmpty(TextBox_MZ_MONEY_LIMIT.Text) ? Convert.DBNull : int.Parse(TextBox_MZ_MONEY_LIMIT.Text.Trim());
                                cmd.ExecuteNonQuery();
                                if (ViewState["Mode"].ToString() == "INSERT")
                                {
                                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('新增成功');", true);
                                    //2010.06.04 LOG紀錄 by伊珊
                                    TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00002", TPMPermissions._boolTPM(), o_DBFactory.ABC_toTest.RegexSQL(cmd));
                                }
                                else if (ViewState["Mode"].ToString() == "UPDATE")
                                {
                                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('編輯成功');", true);
                                    TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00003", TPMPermissions._boolTPM(), o_DBFactory.ABC_toTest.RegexSQL(cmd));
                                }

                                GridView1.DataBind();
                                GridView1.SelectedIndex = -1;
                                btInsert.Enabled = true;
                                btUpdate.Enabled = false;
                                btDelete.Enabled = false;
                                btCancel.Enabled = false;
                                btOK.Enabled = false;
                                TextBox_MZ_UNIT.Text = string.Empty;
                                TextBox_MZ_UNIT1.Text = string.Empty;
                                TextBox_MZ_ID.Text = string.Empty;
                                TextBox_MZ_HOUR_LIMIT.Text = string.Empty;
                                TextBox_MZ_MONEY_LIMIT.Text = string.Empty;
                                ViewState.Remove("Mode");
                                C.controlEnable(ref this.Panel1, false);

                            }
                            catch
                            {
                                if (ViewState["Mode"].ToString() == "INSERT")
                                {
                                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('新增失敗');", true);
                                }
                                else if (ViewState["Mode"].ToString() == "UPDATE")
                                {
                                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('編輯失敗');", true);
                                }
                            }
                            finally
                            {
                                conn.Close();

                                //XX2013/06/18 
                                conn.Dispose();
                            }
                            //btCancel.Enabled = false;
                        }
                    //}
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請輸入金額或時間上限，並少選一個');", true);
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            ViewState["Mode"] = "search";
            ViewState["CMDSQL"] = "SELECT * FROM C_DUTYLIMIT";

            //僅開啟勾選按鈕跟身分證欄位提供查詢
            btAD.Enabled = true;
            btUNIT.Enabled = true;
            TextBox_MZ_ID.Enabled = true;
            btOK.Enabled = true;
            btCancel.Enabled = true;

            //清空選單值
            TextBox_MZ_AD.Text = string.Empty;
            TextBox_MZ_AD1.Text = string.Empty;
            TextBox_MZ_UNIT.Text = string.Empty;
            TextBox_MZ_UNIT1.Text = string.Empty;
            TextBox_MZ_ID.Text = string.Empty;
            TextBox_MZ_HOUR_LIMIT.Text = string.Empty;
            TextBox_MZ_MONEY_LIMIT.Text = string.Empty;
        }

        protected void btInsert_Click(object sender, EventArgs e)
        {
            ViewState["Mode"] = "INSERT";
            ViewState["CMDSQL"] = "INSERT INTO C_DUTYLIMIT(MZ_AD,MZ_UNIT,MZ_ID,MZ_HOUR_LIMIT,MZ_MONEY_LIMIT) " +
                                                  " VALUES(@MZ_AD,@MZ_UNIT,@MZ_ID,@MZ_HOUR_LIMIT,@MZ_MONEY_LIMIT)";

            btOK.Enabled = true;
            btCancel.Enabled = true;
            btDelete.Enabled = false;
            btInsert.Enabled = false;
            btUpdate.Enabled = false;
            TextBox_MZ_UNIT.Text = string.Empty;
            TextBox_MZ_UNIT1.Text = string.Empty;
            TextBox_MZ_ID.Text = string.Empty;
            TextBox_MZ_HOUR_LIMIT.Text = string.Empty;
            TextBox_MZ_MONEY_LIMIT.Text = string.Empty;
            C.controlEnable(ref this.Panel1, true);
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_AD.ClientID + "').focus();$get('" + TextBox_MZ_AD.ClientID + "').focus();", true);
        }

        protected void btUpdate_Click(object sender, EventArgs e)
        {
            if (GridView1.Rows.Count == 0)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請先新增資料')", true);
                return;
            }
            else if (GridView1.SelectedRow == null)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請選取資料')", true);
                return;
            }
            else
            {
                ViewState["Mode"] = "UPDATE";

                string sqlPart = "";

                if (string.IsNullOrEmpty(TextBox_MZ_ID.Text.Trim()))
                {
                    sqlPart = " = 'NULL' ";
                }
                else
                {
                    sqlPart = " = '" + TextBox_MZ_ID.Text.Trim() + "'";
                }

                ViewState["CMDSQL"] = "UPDATE C_DUTYLIMIT  SET MZ_AD=@MZ_AD,MZ_UNIT=@MZ_UNIT,MZ_ID=@MZ_ID," +
                                                            "  MZ_HOUR_LIMIT=@MZ_HOUR_LIMIT,MZ_MONEY_LIMIT=@MZ_MONEY_LIMIT" +
                                                            "  WHERE " +
                                                            "  MZ_AD='" + TextBox_MZ_AD.Text.Trim().ToUpper() +
                                                            "' AND MZ_UNIT='" + TextBox_MZ_UNIT.Text.Trim().ToUpper() +
                                                            "' AND MZ_ID " + sqlPart;



                btUpdate.Enabled = false;
                btDelete.Enabled = false;
                btOK.Enabled = true;
                C.controlEnable(ref this.Panel1, true);
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_AD.ClientID + "').focus();$get('" + TextBox_MZ_AD.ClientID + "').focus();", true);

            }



        }

        protected void btDelete_Click(object sender, EventArgs e)
        {
            if (GridView1.SelectedRow == null)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請選取資料')", true);
                return;
            }
            else
            {
                string sqlPart = "";

                if (string.IsNullOrEmpty(TextBox_MZ_ID.Text.Trim()))
                {
                    sqlPart = " is null ";
                }
                else
                {
                    sqlPart = " = '" + TextBox_MZ_ID.Text.Trim() + "'";
                }

                string DeleteString = "DELETE FROM C_DUTYLIMIT WHERE " +
                                                                     "  MZ_AD='" + TextBox_MZ_AD.Text.Trim().ToUpper() +
                                                                     "' AND MZ_UNIT='" + TextBox_MZ_UNIT.Text.Trim().ToUpper() +
                                                                     "' AND MZ_ID " + sqlPart;

                try
                {
                    o_DBFactory.ABC_toTest.Edit_Data(DeleteString);
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除成功')", true);
                    TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00004", TPMPermissions._boolTPM(), DeleteString);

                    btOK.Enabled = false;
                    btDelete.Enabled = false;
                    btCancel.Enabled = false;
                    btUpdate.Enabled = false;
                    btInsert.Enabled = true;
                    TextBox_MZ_UNIT.Text = string.Empty;
                    TextBox_MZ_UNIT1.Text = string.Empty;
                    TextBox_MZ_ID.Text = string.Empty;
                    TextBox_MZ_HOUR_LIMIT.Text = string.Empty;
                    TextBox_MZ_MONEY_LIMIT.Text = string.Empty;
                    GridView1.DataBind();
                    GridView1.SelectedIndex = -1;
                }
                catch
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除失敗')", true);
                }
            }
        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            GridView1.SelectedIndex = -1;
            btDelete.Enabled = false;
            btOK.Enabled = false;
            btCancel.Enabled = false;
            btUpdate.Enabled = false;
            btInsert.Enabled = true;
            TextBox_MZ_AD.Text = string.Empty;
            TextBox_MZ_AD1.Text = string.Empty;
            TextBox_MZ_UNIT.Text = string.Empty;
            TextBox_MZ_UNIT1.Text = string.Empty;
            TextBox_MZ_ID.Text = string.Empty;
            TextBox_MZ_HOUR_LIMIT.Text = string.Empty;
            TextBox_MZ_MONEY_LIMIT.Text = string.Empty;
            C.controlEnable(ref this.Panel1, false);
        }

        /// <summary>
        /// 按鈕: 開啟自動計算上限視窗
        /// 依照設定金額自動計算上限時數
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btCaluLimit_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click",
            "window.open('C_ForLeaveOvertime_DUTY_Limit_HourCalc.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "','超勤時數上限計算','top=190,left=200,width=670,height=300,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }

        protected void searchResult()
        {
            string searchString = "";
            searchString = "SELECT (SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=MZ_AD AND MZ_KTYPE='04') MZ_AD,MZ_AD as AD,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=MZ_UNIT AND MZ_KTYPE='25' ) MZ_UNIT,MZ_ID,MZ_HOUR_LIMIT,MZ_MONEY_LIMIT,MZ_UNIT AS MZ_UNIT1 FROM C_DUTYLIMIT WHERE 1 = 1";
            if (!string.IsNullOrEmpty(TextBox_MZ_AD.Text))
            {
                searchString += " AND  MZ_AD='" + TextBox_MZ_AD.Text.Trim().ToUpper() + "'";
            }
            if (!string.IsNullOrEmpty(TextBox_MZ_UNIT.Text))
            {
                searchString += " AND MZ_UNIT='" + TextBox_MZ_UNIT.Text.Trim().ToUpper() + "'";
            }
            if (!string.IsNullOrEmpty(TextBox_MZ_ID.Text))
            {
                searchString +=  " AND MZ_ID =  '" + TextBox_MZ_ID.Text + "'";
            }
            
            string tabName = "DT";
            DataTable DTT = new DataTable();
            DTT = o_DBFactory.ABC_toTest.Create_Table(searchString, tabName);
            GridView1.DataSource = DTT;
            //GridView1.DataSourceID = aaaaaa ;
            GridView1.DataSourceID = "";
            GridView1.DataBind();
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView1.SelectedIndex = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "Select")
            {
                btUpdate.Enabled = true;
                btDelete.Enabled = true;
                btInsert.Enabled = false;
                btOK.Enabled = false;
                btCancel.Enabled = true;

                TextBox_MZ_AD.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE='04' AND MZ_KCODE='" + GridView1.DataKeys[int.Parse(e.CommandArgument.ToString())]["AD"].ToString() + "'");
                TextBox_MZ_AD1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_AD.Text, "04");
                TextBox_MZ_UNIT.Text = GridView1.DataKeys[GridView1.SelectedIndex].Values[3].ToString();
                TextBox_MZ_UNIT1.Text = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_UNIT.Text, "25");
                TextBox_MZ_ID.Text = GridView1.SelectedRow.Cells[2].Text.Trim() == "&nbsp;" ? string.Empty : GridView1.DataKeys[int.Parse(e.CommandArgument.ToString())]["MZ_ID"].ToString();
                Label2.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_NAME+'   '+(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=A_DLBASE.MZ_OCCC AND MZ_KTYPE='26') FROM A_DLBASE WHERE MZ_ID='" + TextBox_MZ_ID.Text.Trim().ToUpper() + "' AND MZ_UNIT='" + TextBox_MZ_UNIT.Text.Trim() + "' AND MZ_AD='" + TextBox_MZ_AD.Text.Trim().ToUpper() + "'");
                TextBox_MZ_HOUR_LIMIT.Text = GridView1.SelectedRow.Cells[3].Text.Trim() == "&nbsp;" ? string.Empty : GridView1.SelectedRow.Cells[3].Text.Trim();
                TextBox_MZ_MONEY_LIMIT.Text = GridView1.SelectedRow.Cells[4].Text.Trim() == "&nbsp;" ? string.Empty : GridView1.SelectedRow.Cells[4].Text.Trim();
            }
        }

        protected void btOK_Click(object sender, EventArgs e)
        {
            string a = ViewState["Mode"].ToString();
            if (ViewState["Mode"].ToString() != "search")
            {
                execSQL();
            }
            else
            {
                searchResult();
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GridView1, "Select$" + e.Row.RowIndex);
                e.Row.Cells[GridView1.Columns.Count - 1].Attributes.Add("Style", "display:none");
                if (e.Row.Cells[2].Text == "NULL")
                    e.Row.Cells[2].Text = string.Empty;
                else
                    e.Row.Cells[2].Text = o_A_DLBASE.CNAME(e.Row.Cells[2].Text);
                //e.Row.Attributes ["onclick"] = ClientScript .GetPostBackClientHyperlink (this.GridView1 ,"SELECT$"+e.Row .RowIndex );
            }
            else if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[GridView1.Columns.Count - 1].Attributes.Add("Style", "display:none");
            }
        }

        

        
    }
}
