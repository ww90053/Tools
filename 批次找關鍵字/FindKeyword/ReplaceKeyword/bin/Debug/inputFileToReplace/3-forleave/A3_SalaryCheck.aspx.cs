using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Aspose.Cells;
using System.Reflection;
using System.IO;

namespace TPPDDB._3_forleave
{
    public partial class A3_SalaryCheck : o_score_Function
    {
        string _strGID
        {
            get { return ViewState["O_strGID"] != null ? ViewState["O_strGID"].ToString() : string.Empty; }
            set { ViewState["O_strGID"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // 一般權限檢查
                chk_TPMPermissions();

                // 檢查群組權限
                //_strGID = chk_TPMGroup();
                ViewState["C_strGID"] = o_a_Function.strGID(Session["ADPMZ_ID"].ToString());
            }
            //O.fill_AD(DropDownList_AD);
            if (!Page.IsPostBack)
            {
                pageInit();
                //group_control(_strGID);
                group_control();
            }
        }

        protected void btn_Out_Click(object sender, EventArgs e)
        {
            string error = "";
            if (ddlAd.SelectedValue == "請選擇")
            {
                error += "請選擇機關\n";
            }
            if (string.IsNullOrEmpty(txt_Year.Text) || txt_Year.Text.Count() != 5)
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
                string path = Server.MapPath(@"report\A3_Salary.xls");
                string name;
                if (File.Exists(path))
                {
                    name = Server.MapPath(DateTime.Now.Ticks.ToString());
                    File.Copy(path, name);
                    Workbook work = GetExcel(name);
                    Worksheet sheet = work.Worksheets[0];
                    Cells cells = sheet.Cells;
                    //比對資料差異 先把主表 b_monthpay_main select 出來
                        String SQL = String.Empty;
                        DataTable dt_main = new DataTable();
                        List<SqlParameter> ParmeterList = new List<SqlParameter>();
                        //基本SQL語法
                        //if (ddlAd.SelectedValue == "全部")
                        //{
                        //    SQL = "SELECT * FROM b_monthpay_main WHERE amonth ='" + txt_Year.Text + "'";
                        //}
                        //else
                        //{
                        //    SQL = "SELECT * FROM b_monthpay_main WHERE amonth ='" + txt_Year.Text + "'" + "and mz_ad = '" + ddlAd.SelectedValue + "'";
                        //}
                        SQL = "SELECT * FROM b_monthpay_main WHERE amonth ='" + txt_Year.Text + "'" + "and mz_ad = '" + ddlAd.SelectedValue + "'";

                        //取得 DataTable
                        dt_main = o_DBFactory.ABC_toTest.GetDataTable(SQL + " order by IDCARD", ParmeterList);
                        //select b_monthpay_A3
                        String SQL_A3 = String.Empty;
                        DataTable dt_A3 = new DataTable();
                        //基本SQL語法
                        //if (ddlAd.SelectedValue == "全部")
                        //{
                        //    SQL_A3 = "SELECT * FROM b_monthpay_A3 WHERE amonth ='" + txt_Year.Text + "'";
                        //}
                        //else
                        //{
                        //    SQL_A3 = "SELECT * FROM b_monthpay_A3 WHERE amonth ='" + txt_Year.Text + "'" + "and mz_ad = '" + ddlAd.SelectedValue + "'";

                        //}
                        SQL_A3 = "SELECT * FROM b_monthpay_A3 WHERE amonth ='" + txt_Year.Text + "'" + "and mz_ad = '" + ddlAd.SelectedValue + "'";
                        //取得 DataTable
                        dt_A3 = o_DBFactory.ABC_toTest.GetDataTable(SQL_A3 + " order by mz_id", ParmeterList);                     
                        string sqlInSN_main = "";
                        string sqlInSN_A3 = "";
                        string sqlNotInA3 = "";
                        string sqlNotInMain = "";
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
                                    //int workpTotal = int.Parse(dr_main["WORKP"].ToString()) + int.Parse(dr_main["ELECTRIC"].ToString()) + int.Parse(dr_main["ADVENTIVE"].ToString());
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
                                    else if (workpTotal.ToString() != dr_A3["WORKP"].ToString())///
                                    {
                                        sqlInSN_main = sqlInSN_main + "," + "'" + dr_main["MM_SNID"].ToString() + "'";
                                        sqlInSN_A3 = sqlInSN_A3 + "," + "'" + dr_A3["SN"].ToString() + "'";
                                    }
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
                                //如果已經比到最後一筆都還沒比到人就代表A3裡面都沒有薪資的資料
                                else if (j + 1 == dt_A3.Rows.Count)
                                {
                                    sqlNotInA3 = sqlNotInA3 + "," + "'" + dr_main["MM_SNID"].ToString() + "'";
                                }

                            }
                        }
                        //薪資裡面都沒有A3的資料
                        for (int i = 0; i < dt_A3.Rows.Count; i++)
                        {
                            DataRow dr_A3 = dt_A3.Rows[i];
                            string KeyID_A3 = dr_A3["MZ_ID"].ToString().Substring(0, 6) + dr_A3["MZ_NAME"].ToString();

                            for (int j = 0; j < dt_main.Rows.Count; j++)
                            {
                                DataRow dr_main = dt_main.Rows[j];
                                //姓名加身分證字號前六碼來比對 如果A3裡面都沒有薪資的資料也要SHOW出來
                                string KeyID_main = dr_main["IDCARD"].ToString().Substring(0, 6) + dr_main["MZ_NAME"].ToString();
                                if (KeyID_A3 == KeyID_main)
                                {
                                    break;
                                }
                                //如果已經比到最後一筆都還沒比到人就代表薪資裡面都沒有A3的資料
                                else if (j + 1 == dt_main.Rows.Count)
                                {
                                    sqlNotInMain = sqlNotInMain + "," + "'" + dr_A3["SN"].ToString() + "'";
                                }

                            }
                        }
                        if (sqlInSN_main != "" || sqlNotInA3 != "" || sqlNotInA3 != "" || sqlNotInMain != "")
                        {
                            //用來計算差異資料最後一筆跑到哪
                            int main = 0;
                            int A3 = 0;
                            int NotInA3 = 0;
                            if (sqlInSN_main != "" && sqlNotInA3 != "")
                            {
                                sqlInSN_main = sqlInSN_main.Remove(0, 1);
                                sqlInSN_A3 = sqlInSN_A3.Remove(0, 1);
                                //有差異的資料
                                String SQLd_main = String.Empty;
                                String SQLd_A3 = String.Empty;
                                DataTable dt_difference_main = new DataTable();
                                DataTable dt_difference_A3 = new DataTable();
                                //基本SQL語法
                                SQLd_main = "SELECT * FROM b_monthpay_main WHERE MM_SNID in(" + sqlInSN_main + ") order by IDCARD";
                                SQLd_A3 = "SELECT * FROM b_monthpay_A3 WHERE SN in(" + sqlInSN_A3 + ") order by MZ_ID";
                                //取得 DataTable
                                dt_difference_main = o_DBFactory.ABC_toTest.GetDataTable(SQLd_main, ParmeterList);
                                dt_difference_A3 = o_DBFactory.ABC_toTest.GetDataTable(SQLd_A3, ParmeterList);
                                //重新組合單位轉成明碼
                                fatchUnits(ref dt_difference_main);
                                fatchADs(ref dt_difference_A3);
                                main = dt_difference_main.Rows.Count;
                                A3 = dt_difference_A3.Rows.Count;
                                for (int i = 0; i < dt_difference_main.Rows.Count; i++)
                                {
                                    DataRow dr_main = dt_difference_main.Rows[i];
                                    DataRow dr_A3 = dt_difference_A3.Rows[i];
                                    int j = 0;
                                    if (i != 0)
                                    {
                                        j = i + 1;
                                    }
                                    else
                                    {
                                        j = 1;
                                    }
                                    //main
                                    cells[i + j, 0].PutValue(dr_main["IDCARD"].ToString());
                                    cells[i + j, 1].PutValue(dr_main["MZ_NAME"].ToString());
                                    cells[i + j, 2].PutValue(dr_main["MZ_SPT"].ToString());
                                    cells[i + j, 3].PutValue(dr_main["SALARYPAY1"].ToString());
                                    cells[i + j, 4].PutValue(dr_main["PROFESS"].ToString());
                                    cells[i + j, 5].PutValue(dr_main["WORKP"].ToString());
                                    cells[i + j, 6].PutValue(dr_main["ELECTRIC"].ToString());
                                    cells[i + j, 7].PutValue(dr_main["FAR"].ToString());
                                    cells[i + j, 8].PutValue(dr_main["OTHERADD"].ToString());
                                    cells[i + j, 9].PutValue(dr_main["BONUS"].ToString());
                                    cells[i + j, 10].PutValue(dr_main["UNITName"].ToString());
                                    cells[i + j, 11].PutValue(dr_main["BOSS"].ToString());
                                    cells[i + j, 12].PutValue(dr_main["TECHNICS"].ToString());
                                    cells[i + j, 13].PutValue(dr_main["ADVENTIVE"].ToString());
                                    cells[i + j, 14].PutValue(dr_main["AMONTH"].ToString());
                                    cells[i + j, 15].PutValue(dr_main["ADName"].ToString());
                                    //A3
                                    cells[i + j + 1, 0].PutValue(dr_A3["MZ_ID"].ToString());
                                    cells[i + j + 1, 1].PutValue(dr_A3["MZ_NAME"].ToString());
                                    cells[i + j + 1, 2].PutValue(dr_A3["MZ_SPT"].ToString());
                                    cells[i + j + 1, 3].PutValue(dr_A3["SALARYPAY1"].ToString());
                                    cells[i + j + 1, 4].PutValue(dr_A3["PROFESS"].ToString());
                                    cells[i + j + 1, 5].PutValue(dr_A3["WORKP"].ToString());
                                    cells[i + j + 1, 6].PutValue("");
                                    cells[i + j + 1, 7].PutValue(dr_A3["FAR"].ToString());
                                    cells[i + j + 1, 8].PutValue(dr_A3["OTHERADD"].ToString());
                                    cells[i + j + 1, 9].PutValue(dr_A3["BONUS"].ToString());
                                    cells[i + j + 1, 10].PutValue(dr_A3["EXUNIT_NAME"].ToString());
                                    cells[i + j + 1, 11].PutValue(dr_A3["BOSS"].ToString());
                                    cells[i + j + 1, 12].PutValue(dr_A3["TECHNICS"].ToString());
                                    cells[i + j + 1, 13].PutValue(dr_A3["ADVENTIVE"].ToString());
                                    cells[i + j + 1, 14].PutValue(dr_A3["AMONTH"].ToString());
                                    cells[i + j + 1, 15].PutValue(dr_A3["ADName"].ToString());
                                }
                            }
                            //薪資有A3沒有的資料
                            if (sqlNotInA3 != "")
                            {
                                sqlNotInA3 = sqlNotInA3.Remove(0, 1);
                                String SQLd_NotInA3 = String.Empty;
                                DataTable dt_difference_NotInA3 = new DataTable();
                                SQLd_NotInA3 = "SELECT * FROM b_monthpay_main WHERE MM_SNID in(" + sqlNotInA3 + ") order by IDCARD";
                                dt_difference_NotInA3 = o_DBFactory.ABC_toTest.GetDataTable(SQLd_NotInA3, ParmeterList);
                                fatchUnits(ref dt_difference_NotInA3);
                                NotInA3 = dt_difference_NotInA3.Rows.Count;
                                for (int k = 0; k < dt_difference_NotInA3.Rows.Count; k++)
                                {
                                    DataRow dr_NotInA3 = dt_difference_NotInA3.Rows[k];
                                    cells[main + A3 + 1 + k, 0].PutValue(dr_NotInA3["IDCARD"].ToString());
                                    cells[main + A3 + 1 + k, 1].PutValue(dr_NotInA3["MZ_NAME"].ToString());
                                    cells[main + A3 + 1 + k, 2].PutValue(dr_NotInA3["MZ_SPT"].ToString());
                                    cells[main + A3 + 1 + k, 3].PutValue(dr_NotInA3["SALARYPAY1"].ToString());
                                    cells[main + A3 + 1 + k, 4].PutValue(dr_NotInA3["PROFESS"].ToString());
                                    cells[main + A3 + 1 + k, 5].PutValue(dr_NotInA3["WORKP"].ToString());
                                    cells[main + A3 + 1 + k, 6].PutValue(dr_NotInA3["ELECTRIC"].ToString());
                                    cells[main + A3 + 1 + k, 7].PutValue(dr_NotInA3["FAR"].ToString());
                                    cells[main + A3 + 1 + k, 8].PutValue(dr_NotInA3["OTHERADD"].ToString());
                                    cells[main + A3 + 1 + k, 9].PutValue(dr_NotInA3["BONUS"].ToString());
                                    cells[main + A3 + 1 + k, 10].PutValue(dr_NotInA3["UNITName"].ToString());
                                    cells[main + A3 + 1 + k, 11].PutValue(dr_NotInA3["BOSS"].ToString());
                                    cells[main + A3 + 1 + k, 12].PutValue(dr_NotInA3["TECHNICS"].ToString());
                                    cells[main + A3 + 1 + k, 13].PutValue(dr_NotInA3["ADVENTIVE"].ToString());
                                    cells[main + A3 + 1 + k, 14].PutValue(dr_NotInA3["AMONTH"].ToString());
                                    cells[main + A3 + 1 + k, 15].PutValue(dr_NotInA3["ADName"].ToString());
                                }
                            }
                            //A3有薪資沒有
                            if (sqlNotInMain != "")
                            {
                                sqlNotInMain = sqlNotInMain.Remove(0, 1);
                                String SQLd_NotInMain = String.Empty;
                                DataTable dt_difference_NotInMain = new DataTable();
                                SQLd_NotInMain = "SELECT * FROM b_monthpay_A3 WHERE SN in(" + sqlNotInMain + ") order by MZ_ID";
                                dt_difference_NotInMain = o_DBFactory.ABC_toTest.GetDataTable(SQLd_NotInMain, ParmeterList);
                                fatchADs(ref dt_difference_NotInMain);

                                for (int k = 0; k < dt_difference_NotInMain.Rows.Count; k++)
                                {
                                    DataRow dr_NotInMain = dt_difference_NotInMain.Rows[k];
                                    cells[main + A3 + NotInA3 + 1 + k, 0].PutValue(dr_NotInMain["MZ_ID"].ToString());
                                    cells[main + A3 + NotInA3 + 1 + k, 1].PutValue(dr_NotInMain["MZ_NAME"].ToString());
                                    cells[main + A3 + NotInA3 + 1 + k, 2].PutValue(dr_NotInMain["MZ_SPT"].ToString());
                                    cells[main + A3 + NotInA3 + 1 + k, 3].PutValue(dr_NotInMain["SALARYPAY1"].ToString());
                                    cells[main + A3 + NotInA3 + 1 + k, 4].PutValue(dr_NotInMain["PROFESS"].ToString());
                                    cells[main + A3 + NotInA3 + 1 + k, 5].PutValue(dr_NotInMain["WORKP"].ToString());
                                    cells[main + A3 + NotInA3 + 1 + k, 6].PutValue("");
                                    cells[main + A3 + NotInA3 + 1 + k, 7].PutValue(dr_NotInMain["FAR"].ToString());
                                    cells[main + A3 + NotInA3 + 1 + k, 8].PutValue(dr_NotInMain["OTHERADD"].ToString());
                                    cells[main + A3 + NotInA3 + 1 + k, 9].PutValue(dr_NotInMain["BONUS"].ToString());
                                    cells[main + A3 + NotInA3 + 1 + k, 10].PutValue(dr_NotInMain["EXUNIT_NAME"].ToString());
                                    cells[main + A3 + NotInA3 + 1 + k, 11].PutValue(dr_NotInMain["BOSS"].ToString());
                                    cells[main + A3 + NotInA3 + 1 + k, 12].PutValue(dr_NotInMain["TECHNICS"].ToString());
                                    cells[main + A3 + NotInA3 + 1 + k, 13].PutValue(dr_NotInMain["ADVENTIVE"].ToString());
                                    cells[main + A3 + NotInA3 + 1 + k, 14].PutValue(dr_NotInMain["AMONTH"].ToString());
                                    cells[main + A3 + NotInA3 + 1 + k, 15].PutValue(dr_NotInMain["ADName"].ToString());                                  
                                }
                            }
                            Response.AddHeader("Content-Disposition", "attachment;filename=" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
                            work.Save(HttpContext.Current.Response.OutputStream, new XlsSaveOptions(SaveFormat.Excel97To2003));

                            try
                            {
                                File.Delete(name);
                            }
                            catch { }
                            HttpContext.Current.Response.End();
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('" + "無資料" + "');", true);
                            return;

                        }                      
                }
            }
        }
        private void group_control()
        {
            switch (ViewState["C_strGID"].ToString())
            {
                case "TPMIDISADMIN":
                case "A":
                case "B":
                case "C":
                    break;
                default:
                    Response.Redirect("~/ErrorPage.aspx?erid=TPFXXX0001");
                    break;

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
        public Workbook GetExcel(string FullFileName)
        {
            AsposeLicense();
            Workbook work = new Workbook(FullFileName);

            return work;

        }
        public void AsposeLicense()
        {
            Aspose.Cells.License license = new Aspose.Cells.License();
            license.SetLicense("Aspose.Cells.lic");
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

    }
}