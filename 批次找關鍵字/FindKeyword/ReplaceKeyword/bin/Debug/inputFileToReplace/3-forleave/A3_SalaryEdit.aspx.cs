using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Drawing;

namespace TPPDDB._3_forleave
{
    public partial class A3_SalaryEdit : System.Web.UI.Page
    {
        string strSQL;
        DataTable temp = new DataTable();
        DataTable searchResult
        {
            get { return (DataTable)ViewState["searchResult"]; }
            set { ViewState["searchResult"] = value; }
        }

        #region 頁面變數

        string key
        {
            set { ViewState["SN"] = value; }
            get { return ViewState["SN"].ToString(); }
        }

        private string strIDCARD
        {
            get
            {
                return Label_IDCARD.Text.Trim().ToUpper();
            }
            set
            {
                Label_IDCARD.Text = value;
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

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                pageInit();
            }
        }

        #region 查詢

        protected void Button_SearchDone_Click(object sender, EventArgs e)
        {
            searchResult = getSearchResult();

            if (searchResult.Rows.Count == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('查無資料')", true);
                return;
            }
            GridView_MANUFACTURER_BASE.DataSource = searchResult;
            GridView_MANUFACTURER_BASE.DataBind();
        }

        protected void GridView_MANUFACTURER_BASE_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView_MANUFACTURER_BASE.PageIndex = e.NewPageIndex;
            GridView_MANUFACTURER_BASE.DataSource = searchResult;
            GridView_MANUFACTURER_BASE.DataBind();
        }

        DataTable getSearchResult()
        {
            DataTable dt = new DataTable();
            List<SqlParameter> listParameter = new List<SqlParameter>();

            strSQL = "SELECT MZ_ID,MZ_NAME,MZ_SPT,SALARYPAY1,PROFESS,WORKP,FAR,OTHERADD,BONUS,EXUNIT_NAME,BOSS,AMONTH,MZ_AD,TECHNICS,SN,ADVENTIVE FROM B_MONTHPAY_A3 WHERE 1=1";
            if (TextBox_SearchID.Text.Length != 0)
            {
                strSQL += " AND MZ_ID LIKE @MZ_ID";
                listParameter.Add(new SqlParameter("MZ_ID", "%" + TextBox_SearchID.Text + "%"));
            }
            if (ddlAd.SelectedValue != "請選擇")
            {
                strSQL += " AND MZ_AD LIKE @MZ_AD";
                listParameter.Add(new SqlParameter("MZ_AD", "%" + ddlAd.SelectedValue + "%"));
            }
            if (TextBox_SearchName.Text.Length != 0)
            {
                strSQL += " AND MZ_NAME LIKE @MZ_NAME";
                listParameter.Add(new SqlParameter("MZ_NAME", "%" + TextBox_SearchName.Text + "%"));
            }
            if (TextBox_YearMonth.Text.Length != 0)
            {
                strSQL += " AND AMONTH LIKE @AMONTH";
                listParameter.Add(new SqlParameter("AMONTH", "%" + TextBox_YearMonth.Text + "%"));
            }
            dt = o_DBFactory.ABC_toTest.GetDataTable(strSQL, listParameter);
            fatchADs(ref dt);
            return dt;
        }

        #endregion

        #region 選取資料列

        protected void GridView_MANUFACTURER_BASE_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "sel")
            {
                key = e.CommandArgument.ToString();
                dataShow(key);

                btn_showPopup_ModalPopupExtender.Show();
            }
        }

        //填入選取或查詢到的該筆資料至控制項
        protected void dataShow(string key)
        {
            strSQL = string.Format("SELECT MZ_ID,MZ_NAME,MZ_SPT,SALARYPAY1,PROFESS,WORKP,FAR,OTHERADD,BONUS,EXUNIT_NAME,BOSS,AMONTH,MZ_AD,TECHNICS,SN,ADVENTIVE FROM B_MONTHPAY_A3 WHERE SN ='{0}'", key); ;
            temp = new DataTable();
            temp.Load(OracleHelper.ExecuteReader(OracleHelper.connStr, CommandType.Text, strSQL));

            HiddenField_IDCARD.Value = key;
            strIDCARD = temp.Rows[0]["MZ_ID"].ToString();
            strNAME = temp.Rows[0]["MZ_NAME"].ToString();
        }
        #endregion

        #region popup視窗

        protected void btn_popupSave_Click(object sender, EventArgs e)
        {
            if (strNAME.Length == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('姓名不可空白！')", true);
                TextBox_NAME.BackColor = Color.Orange;
                return;
            }
            if (boolMANUFACTURER_BASE_Update())
            {
                GridView_MANUFACTURER_BASE.DataBind();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('修改完成')", true);

            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('修改失敗')", true);
            }

            searchResult = getSearchResult();
            GridView_MANUFACTURER_BASE.DataSource = searchResult;
            GridView_MANUFACTURER_BASE.DataBind();
        }
        private bool boolMANUFACTURER_BASE_Update()
        {
            //key = SN
            if (key == null || key.ToString() == "")
                return false;
            try
            {
                strSQL = "UPDATE B_MONTHPAY_A3 SET MZ_NAME =@MZ_NAME WHERE SN = @SN";
                SqlParameter[] parameterList = { 
                    new SqlParameter("SN",SqlDbType.VarChar){Value = key},
                    new SqlParameter("MZ_NAME",SqlDbType.VarChar){Value = strNAME},                 
                    };
                o_DBFactory.ABC_toTest.ExecuteNonQuery( strSQL, parameterList);

                return true;
            }
            catch
            {
                return false;
            }
        }


        #endregion

        protected void btn_different_Click(object sender, EventArgs e)
        {

            string error = "";
            if (ddlAd.SelectedValue == "請選擇")
            {
                error += "請選擇機關\n";
            }
            if (string.IsNullOrEmpty(TextBox_YearMonth.Text) || TextBox_YearMonth.Text.Count() != 5)
            {
                error += "年度月份不能為空白或打錯\n";
            }
            if (!string.IsNullOrEmpty(error))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('" + error + "');", true);
                return;
            }
            else
            {
                String SQL = String.Empty;
                DataTable dt_main = new DataTable();
                List<SqlParameter> ParmeterList = new List<SqlParameter>();
                //基本SQL語法
                SQL = "SELECT * FROM b_monthpay_main WHERE amonth ='" + TextBox_YearMonth.Text + "'" + "and mz_ad = '" + ddlAd.SelectedValue + "'";

                //SQL = "SELECT * FROM b_monthpay_main WHERE amonth ='" + TextBox_YearMonth.Text + "'" ;
                //SQL = "SELECT * FROM b_monthpay_main";
                //取得 DataTable
                dt_main = o_DBFactory.ABC_toTest.GetDataTable(SQL, ParmeterList);
                //select b_monthpay_A3
                String SQL_A3 = String.Empty;
                DataTable dt_A3 = new DataTable();
                //基本SQL語法
                SQL_A3 = "SELECT * FROM b_monthpay_A3 WHERE amonth ='" + TextBox_YearMonth.Text + "'" + "and mz_ad = '" + ddlAd.SelectedValue + "'";

                //SQL_A3 = "SELECT * FROM b_monthpay_A3 WHERE amonth ='" + TextBox_YearMonth.Text + "'";
                //SQL_A3 = "SELECT * FROM b_monthpay_A3";
                //取得 DataTable
                dt_A3 = o_DBFactory.ABC_toTest.GetDataTable(SQL_A3, ParmeterList);
                string sqlInSN_main = "";
                string sqlInSN_A3 = "";
                string sqlNotInA3 = "";
                for (int i = 0; i < dt_main.Rows.Count; i++)
                {
                    DataRow dr_main = dt_main.Rows[i];
                    string KeyID_main = dr_main["IDCARD"].ToString().Substring(0, 6) + dr_main["MZ_NAME"].ToString();

                    for (int j = 0; j < dt_A3.Rows.Count; j++)
                    {
                        DataRow dr_A3 = dt_A3.Rows[j];
                        //姓名加身分證字號前六碼來比對 如果A3裡面都沒有薪資的資料也要SHOW出來
                        string KeyID_A3 = dr_A3["MZ_ID"].ToString().Substring(0, 6) + dr_A3["MZ_NAME"].ToString();
                        if (KeyID_main == KeyID_A3)
                        {
                            //WORKP+ELECTRIC+ADVENTIVE
                            int workpTotal = int.Parse(dr_main["WORKP"].ToString()) + int.Parse(dr_main["ELECTRIC"].ToString());
                            if (dr_main["MZ_SPT"].ToString().Count() == 3)
                            {
                                if ("0" + dr_main["MZ_SPT"].ToString() != dr_A3["MZ_SPT"].ToString())
                                {
                                    sqlInSN_main = sqlInSN_main + "," + "'" + dr_main["MM_SNID"].ToString() + "'";
                                    sqlInSN_A3 = sqlInSN_A3 + "," + "'" + dr_A3["SN"].ToString() + "'";
                                }
                            }
                            else
                            {
                                if (dr_main["MZ_SPT"].ToString() != dr_A3["MZ_SPT"].ToString())
                                {
                                    sqlInSN_main = sqlInSN_main + "," + "'" + dr_main["MM_SNID"].ToString() + "'";
                                    sqlInSN_A3 = sqlInSN_A3 + "," + "'" + dr_A3["SN"].ToString() + "'";
                                }
                            }
                            if (dr_main["SALARYPAY1"].ToString() != dr_A3["SALARYPAY1"].ToString())
                            {
                                sqlInSN_main = sqlInSN_main + "," + "'" + dr_main["MM_SNID"].ToString() + "'";
                                sqlInSN_A3 = sqlInSN_A3 + "," + "'" + dr_A3["SN"].ToString() + "'";
                            }
                            else if (dr_main["PROFESS"].ToString() != dr_A3["PROFESS"].ToString())
                            {
                                sqlInSN_main = sqlInSN_main + "," + "'" + dr_main["MM_SNID"].ToString() + "'";
                                sqlInSN_A3 = sqlInSN_A3 + "," + "'" + dr_A3["SN"].ToString() + "'";
                            }
                            //else if (workpTotal.ToString() != dr_A3["WORKP"].ToString())
                            //{
                            //    sqlInSN = sqlInSN + "," + "'" + dr_main["MM_SNID"].ToString() + "'";
                            //}
                            else if (dr_main["FAR"].ToString() != dr_A3["FAR"].ToString())
                            {
                                sqlInSN_main = sqlInSN_main + "," + "'" + dr_main["MM_SNID"].ToString() + "'";
                                sqlInSN_A3 = sqlInSN_A3 + "," + "'" + dr_A3["SN"].ToString() + "'";
                            }
                            else if (dr_main["OTHERADD"].ToString() != dr_A3["OTHERADD"].ToString())
                            {
                                sqlInSN_main = sqlInSN_main + "," + "'" + dr_main["MM_SNID"].ToString() + "'";
                                sqlInSN_A3 = sqlInSN_A3 + "," + "'" + dr_A3["SN"].ToString() + "'";
                            }
                            else if (dr_main["BONUS"].ToString() != dr_A3["BONUS"].ToString())
                            {
                                sqlInSN_main = sqlInSN_main + "," + "'" + dr_main["MM_SNID"].ToString() + "'";
                                sqlInSN_A3 = sqlInSN_A3 + "," + "'" + dr_A3["SN"].ToString() + "'";
                            }
                            else if (dr_main["BOSS"].ToString() != dr_A3["BOSS"].ToString())
                            {
                                sqlInSN_main = sqlInSN_main + "," + "'" + dr_main["MM_SNID"].ToString() + "'";
                                sqlInSN_A3 = sqlInSN_A3 + "," + "'" + dr_A3["SN"].ToString() + "'";
                            }
                            else if (dr_main["AMONTH"].ToString() != dr_A3["AMONTH"].ToString())
                            {
                                sqlInSN_main = sqlInSN_main + "," + "'" + dr_main["MM_SNID"].ToString() + "'";
                                sqlInSN_A3 = sqlInSN_A3 + "," + "'" + dr_A3["SN"].ToString() + "'";
                            }
                            else if (dr_main["MZ_AD"].ToString() != dr_A3["MZ_AD"].ToString())
                            {
                                sqlInSN_main = sqlInSN_main + "," + "'" + dr_main["MM_SNID"].ToString() + "'";
                                sqlInSN_A3 = sqlInSN_A3 + "," + "'" + dr_A3["SN"].ToString() + "'";
                            }
                            else if (dr_main["TECHNICS"].ToString() != dr_A3["TECHNICS"].ToString())
                            {
                                sqlInSN_main = sqlInSN_main + "," + "'" + dr_main["MM_SNID"].ToString() + "'";
                                sqlInSN_A3 = sqlInSN_A3 + "," + "'" + dr_A3["SN"].ToString() + "'";
                            }
                            else if (dr_main["ADVENTIVE"].ToString() != dr_A3["ADVENTIVE"].ToString())
                            {
                                sqlInSN_main = sqlInSN_main + "," + "'" + dr_main["MM_SNID"].ToString() + "'";
                                sqlInSN_A3 = sqlInSN_A3 + "," + "'" + dr_A3["SN"].ToString() + "'";
                            }
                            break;
                        }
                        else if (j + 1 == dt_A3.Rows.Count)
                        {
                            sqlNotInA3 = sqlNotInA3 + "," + "'" + dr_main["IDCARD"].ToString().Substring(0, 6).ToString() + "****" + "'";
                            //sqlNotInA3 = sqlNotInA3 + "," + "'" + dr_A3["SN"].ToString() + "'";
                        }

                    }
                }
                if (sqlInSN_main != "" || sqlNotInA3 != "")
                {
                    //if (sqlInSN_main != "")
                    //{
                    //    sqlInSN_main = sqlInSN_main.Remove(0, 1);
                    //    sqlInSN_A3 = sqlInSN_A3.Remove(0, 1);
                    //    //有差異的資料
                    //    String SQLd_main = String.Empty;
                    //    String SQLd_A3 = String.Empty;
                    //    DataTable dt_difference_main = new DataTable();
                    //    DataTable dt_difference_A3 = new DataTable();
                    //    //基本SQL語法
                    //    SQLd_main = "SELECT * FROM b_monthpay_main WHERE MM_SNID in(" + sqlInSN_main + ") order by IDCARD";
                    //    SQLd_A3 = "SELECT * FROM b_monthpay_A3 WHERE SN in(" + sqlInSN_A3 + ") order by MZ_ID";
                    //    //取得 DataTable
                    //    dt_difference_main = o_DBFactory.ABC_toTest.GetDataTable(SQLd_main, ParmeterList);
                    //    dt_difference_A3 = o_DBFactory.ABC_toTest.GetDataTable(SQLd_A3, ParmeterList);
                    //    //重新組合單位轉成明碼
                    //    fatchUnits(ref dt_difference_main);
                    //    fatchADs(ref dt_difference_A3);
                    //    searchResult = dt_difference_A3;
                    //    if (searchResult.Rows.Count == 0)
                    //    {
                    //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('查無資料')", true);
                    //        return;
                    //    }
                    //    GridView_MANUFACTURER_BASE.DataSource = searchResult;
                    //    GridView_MANUFACTURER_BASE.DataBind();
                    //}
                    //薪資有A3沒有的資料
                    if (sqlNotInA3 != "")
                    {
                        sqlNotInA3 = sqlNotInA3.Remove(0, 1);
                        String SQLd_NotInA3 = String.Empty;
                        DataTable dt_difference_NotInA3 = new DataTable();
                        //SQL = "SELECT * FROM b_monthpay_main WHERE amonth ='" + TextBox_YearMonth.Text + "'" + "and mz_ad = '" + ddlAd.SelectedValue + "'";
                        SQLd_NotInA3 = "SELECT * FROM b_monthpay_A3 WHERE amonth ='" + TextBox_YearMonth.Text + "'" + "and mz_ad = '" + ddlAd.SelectedValue + "'" + "and MZ_ID in(" + sqlNotInA3 + ") order by MZ_ID";
                        //SQLd_NotInA3 = "SELECT * FROM b_monthpay_A3 WHERE SN in(" + sqlNotInA3 + ") order by MZ_ID";
                        dt_difference_NotInA3 = o_DBFactory.ABC_toTest.GetDataTable(SQLd_NotInA3, ParmeterList);
                        fatchADs(ref dt_difference_NotInA3);
                        searchResult = dt_difference_NotInA3;
                        if (searchResult.Rows.Count == 0)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('查無資料')", true);
                            return;
                        }
                        GridView_MANUFACTURER_BASE.DataSource = searchResult;
                        GridView_MANUFACTURER_BASE.DataBind();
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('查無資料')", true);
                }

            }
         
        }

        private void pageInit()
        {
            string strSQL = "";
            DataTable temp = new DataTable();
            //機關
            strSQL = "SELECT MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE='04' AND MZ_KCODE LIKE '38213%' and MZ_KCODE not in ('382133400C','382133500C')";
            temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
            if (temp.Rows.Count > 0)
            {
                ddlAd.DataSource = temp;
                ddlAd.DataTextField = "MZ_KCHI";
                ddlAd.DataValueField = "MZ_KCODE";
                ddlAd.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                ddlAd.DataBind();
                ddlAd.Items.Insert(0, new ListItem("請選擇", "請選擇"));
            }
        }

        private void fatchADs(ref DataTable dt)
        {
            //機關
            DataColumn workADName = dt.Columns.Add("ADName", typeof(String));
            //取得所有機關的名稱           
            string strSQL = "SELECT MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE='04' AND MZ_KCODE LIKE '38213%'";
            //取出所有機關名稱
            DataTable dt_AD = o_DBFactory.ABC_toTest.GetDataTable(strSQL);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //把機關名稱存起來
                List<String> lstAD = new List<String>();

                foreach (DataRow row_unit in dt_AD.Rows)
                {
                    if (dt.Rows[i]["MZ_AD"].ToString() == row_unit["MZ_KCODE"].ToString())
                    {
                        lstAD.Add(row_unit["MZ_KCHI"].ToString());
                        dt.Rows[i]["ADName"] = String.Join(",", lstAD.ToArray());
                    }
                }
            }
        }

        private void fatchUnits(ref DataTable dt)
        {
            //機關
            DataColumn workADName = dt.Columns.Add("ADName", typeof(String));
            //取得所有機關的名稱           
            string strSQL = "SELECT MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE='04' AND MZ_KCODE LIKE '38213%'";
            //取出所有機關名稱
            DataTable dt_AD = o_DBFactory.ABC_toTest.GetDataTable(strSQL);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //把機關名稱存起來
                List<String> lstAD = new List<String>();

                foreach (DataRow row_unit in dt_AD.Rows)
                {
                    if (dt.Rows[i]["MZ_AD"].ToString() == row_unit["MZ_KCODE"].ToString())
                    {
                        lstAD.Add(row_unit["MZ_KCHI"].ToString());
                        dt.Rows[i]["ADName"] = String.Join(",", lstAD.ToArray());
                    }
                }
            }
            //單位
            DataColumn workUNITName = dt.Columns.Add("UNITName", typeof(String));
            //取得所有單位
            strSQL = "SELECT AAA.* FROM (SELECT MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE='25' AND MZ_KCODE IN (SELECT MZ_UNIT FROM VW_A_UNIT_AD)) AAA ";
            DataTable dt_Unit = o_DBFactory.ABC_toTest.GetDataTable(strSQL);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //把單位名稱存起來
                List<String> lstUnit = new List<String>();
                foreach (DataRow row_unit in dt_Unit.Rows)
                {
                    if (dt.Rows[i]["MZ_UNIT"].ToString() == row_unit["MZ_KCODE"].ToString())
                    {
                        lstUnit.Add(row_unit["MZ_KCHI"].ToString());
                        dt.Rows[i]["UNITName"] = String.Join(",", lstUnit.ToArray());
                    }
                }
            }
        }

    }
}