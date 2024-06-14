using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace TPPDDB._2_salary
{
    public partial class B_SearchYearSalary3 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           

            Label_MSG.Visible = false;

            if (!IsPostBack)
            { 
                SalaryPublic.checkPermission();
                SalaryPublic.fillDropDownList(ref this.DropDownList_PAY_AD);
                TextBox_YEAR.Text = SalaryPublic.strRepublicLastYear();
            }
            switch (Request.QueryString["chkType"])
            {
                case "1":
                    Label_TITLE.Text = "年終獎金明細表";
                    break;
                case "2":
                    Label_TITLE.Text = "考績獎金明細表";
                    break;
                case "3":
                    Label_TITLE.Text = "薪資年度總表";
                    break;
                default:
                    TPMPermissions._boolErrorData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "ADUXXX0001");
                    Response.Redirect("~/ErrorPage.aspx?erid=ADUXXX0001");
                    break;
            }
        }

        protected void Button_CANCEL_Click(object sender, EventArgs e)
        {
            Response.Redirect("B_SearchYearSalary3.aspx?chkType=" + Request.QueryString["chkType"] + "&TPM_FION=" + Request.QueryString["TPM_FION"]);
        }

        protected bool chkYearType()
        {
            if (TextBox_YEAR.Text.Length == 3)
            {
                return true;
            }
            return false;
        }

        protected void Button_SEARCH_Click(object sender, EventArgs e)
        {
            if (chkYearType())
            {
                switch (Request.QueryString["chkType"].ToString())
                {
                    case "1":
                        sendYearEndtoRPT();
                        break;
                    case "2":
                        sendYearEffecttoRPT();
                        break;
                    case "3":
                        sendYearSalaryRPT();
                        break;
                    default:
                        //strSQL = string.Format("SELECT MM_SNID FROM B_MONTHPAY_MAIN WHERE IDCARD='{0}' AND AMONTH='{1}'", dataTable.Rows[0]["MZ_ID"].ToString(), TextBox_YEAR.Text + DropDownList_MONTH.SelectedValue.PadLeft(2, '0'));
                        break;
                }
            }
            else
            {
                Label_MSG.Text = "輸入年份格式錯誤或未選擇月份";
                Label_MSG.Visible = true;
            }
        }

        //年終獎金明細表  VW_ALL_YEARPAY_DATA
        protected void sendYearEndtoRPT()
        {
            if (TextBox_YEAR.Text.Length == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('請輸入查詢的資料年度')", true);
                return;
            }


            List<SqlParameter> ops = new List<SqlParameter>();
            string sql = "SELECT * FROM VW_ALL_YEARPAY_DATA WHERE 1=1 AND PAY_AD=@PAY_AD AND AYEAR=@AYEAR";

            ops.Add(new SqlParameter("PAY_AD", DropDownList_PAY_AD.SelectedValue));
            ops.Add(new SqlParameter("AYEAR", TextBox_YEAR.Text));
            if (TextBox_MZ_ID.Text.Length > 0)
            {
                sql += " AND IDCARD=@IDCARD";
                ops.Add(new SqlParameter("IDCARD", TextBox_MZ_ID.Text.ToString().ToUpper()));
            }
            if (TextBox_MZ_NAME.Text.Length > 0)
            {
                sql += " AND NAME=@NAME";
                ops.Add(new SqlParameter("NAME", TextBox_MZ_NAME.Text.ToString()));
            }
            if (TextBox_MZ_POLNO.Text.Length > 0)
            {
                sql += " AND MZ_POLNO=@MZ_POLNO";
                ops.Add(new SqlParameter("MZ_POLNO", TextBox_MZ_POLNO.Text.ToString()));
            }

            string strSQL =SalaryPublic.RegixSQL(sql, ops);  

            DataTable rpt_dt = o_DBFactory.ABC_toTest.DataSelect(sql, ops);

            if (rpt_dt.Rows.Count == 0)
            {
                Label_MSG.Text = "此搜尋條件查無資料";
                Label_MSG.Visible = true;
                return;
            }
            Session["RPT_SQL_B"] = strSQL;
            //Session["rpt_dt"] = rpt_dt;
            string tmp_url = "B_rpt.aspx?fn=SalaryYear-Endbonus&TPM_FION=" + Request.QueryString["TPM_FION"];
            ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
        }

        
        //考績獎金明細表  
        protected void sendYearEffecttoRPT()
        {
            List<String> lsSQL = new List<string>();
            if (TextBox_MZ_ID.Text.Length > 0)
            {
                lsSQL.Add(" IDCARD ='" + TextBox_MZ_ID.Text.ToString().ToUpper() + "'");
            }
            if (TextBox_MZ_NAME.Text.Length > 0)
            {
                lsSQL.Add(" MZ_NAME ='" + TextBox_MZ_NAME.Text.ToString() + "'");
            }
            if (TextBox_MZ_POLNO.Text.Length > 0)
            {
                lsSQL.Add(" MZ_POLNO ='" + TextBox_MZ_POLNO.Text.ToString() + "'");
            }
            lsSQL.Add(String.Format(" AYEAR ='{0}'", TextBox_YEAR.Text));

            string strSQL = String.Format(@"SELECT MZ_NAME, IDCARD, MZ_POLNO, AYEAR, MZ_OCCC,AKC.MZ_KCHI MZ_OCCC_CH, PAY, BOSS_AMONTH, SALARYPAY1, PROFESS, BOSS,ELECTRICPAY , EXTRA01, WORKP, TECHNICS, FAR, TAX, NOTE, TOTAL 
                                            FROM B_EFFECT
                                            LEFT JOIN A_KTYPE AKC ON AKC.MZ_KCODE=MZ_OCCC AND AKC.MZ_KTYPE='26' 
                                            WHERE PAY_AD='{0}' {1}", DropDownList_PAY_AD.SelectedValue, " AND " + String.Join(" AND ", lsSQL.ToArray()));
            DataTable rpt_dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "YPInfo"); ;
            Session["RPT_SQL_B"] = strSQL;
            if (rpt_dt.Rows.Count == 0)
            {
                Label_MSG.Text = "此搜尋條件查無資料";
                Label_MSG.Visible = true;
            }
            else
            {
               
                
                string tmp_url = "B_rpt.aspx?fn=PersonalEffectDetail&TPM_FION=" + Request.QueryString["TPM_FION"];
                ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
            }
        }

        private static DataTable dt_TODATA = new DataTable();
        /*
        protected void sendYearSalaryRPT()
        {
            List<String> lsSQL = new List<string>();
            if (TextBox_MZ_ID.Text.Length > 0)
            {
                lsSQL.Add(" MZ_ID ='" + TextBox_MZ_ID.Text.ToString().ToUpper() + "'");
            }
            if (TextBox_MZ_NAME.Text.Length > 0)
            {
                lsSQL.Add(" MZ_NAME ='" + TextBox_MZ_NAME.Text.ToString() + "'");
            }
            if (TextBox_MZ_POLNO.Text.Length > 0)
            {
                lsSQL.Add(" MZ_POLNO ='" + TextBox_MZ_POLNO.Text.ToString() + "'");
            }

            string strIDSQL = String.Format("SELECT MZ_ID FROM A_DLBASE WHERE PAY_AD='{0}' {1}", DropDownList_PAY_AD.SelectedValue, lsSQL.Count > 0 ? " AND " + String.Join(" AND ", lsSQL.ToArray()) : String.Empty);
            DataTable dt = o_DBFactory.ABC_toTest.Create_Table(strIDSQL, "targetID");

            string strYEAR = TextBox_YEAR.Text.ToString();

            dt_TODATA.Rows.Clear();
            dt_TODATA.Columns.Clear();
            dt_TODATA.Columns.Add("LSNAME", typeof(String));
            dt_TODATA.Columns.Add("LSIDCARD", typeof(String));
            dt_TODATA.Columns.Add("LSPAY", typeof(String));
            dt_TODATA.Columns.Add("LSDA", typeof(String));
            dt_TODATA.Columns.Add("LSNOTE", typeof(String));

            for (int j = 0; j < dt.Rows.Count; j++)
            {
                string strIDCARD = dt.Rows[j]["MZ_ID"].ToString();
                for (int i = 1; i <= 12; i++)
                {
                    DataRow dr = dt_TODATA.NewRow();
                    dr["LSNAME"] = strRETURN_DATA(strIDCARD, strYEAR + i.ToString("00"), "MONTHPAY", "MZ_NAME");
                    dr["LSIDCARD"] = strIDCARD;
                    dr["LSPAY"] = (SalaryMonth_View.intSearch_SalaryMonth_Data_Increase(strYEAR + i.ToString("00"), strIDCARD) - SalaryMonth_View.intSearch_SalaryMonth_Data_Decrease(strYEAR + i.ToString("00"), strIDCARD)).ToString();
                    dr["LSDA"] = (strYEAR + i.ToString("00")).ToString();
                    dr["LSNOTE"] = strRETURN_DATA(strIDCARD, strYEAR + i.ToString("00"), "MONTHPAY", "NOTE");
                    dt_TODATA.Rows.Add(dr);
                }

                for (int i = 1; i <= 12; i++)
                {
                    DataRow dr = dt_TODATA.NewRow();
                    dr["LSNAME"] = strRETURN_DATA(strIDCARD, strYEAR + i.ToString("00"), "REPAIRPAY", "MZ_NAME");
                    dr["LSIDCARD"] = strIDCARD;
                    dr["LSPAY"] = (SalaryRepair.intSearch_SalaryRepair_Data_Increase(strYEAR + i.ToString("00"), strIDCARD) - SalaryRepair.intSearch_SalaryRepair_Data_Decrease(strYEAR + i.ToString("00"), strIDCARD)).ToString();
                    dr["LSDA"] = (strYEAR + i.ToString("00")).ToString();
                    dr["LSNOTE"] = strRETURN_DATA(strIDCARD, strYEAR + i.ToString("00"), "REPAIRPAY", "NOTE");
                    dt_TODATA.Rows.Add(dr);
                }

                using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
                {
                    Selectconn.Open();
                    try
                    {
                        string strSQL = "SELECT MZ_NAME, DA, DA_INOUT_GROUP, NOTE, SUM(PAY-(TAX+PAY1+PAY2+PAY3)) AS PAY_DATA FROM B_SOLE WHERE IDCARD = '" + strIDCARD + "' AND dbo.SUBSTR(DA, 1, 3) = '" + strYEAR + "' GROUP BY MZ_NAME, DA, DA_INOUT_GROUP, NOTE";
                        SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
                        DataTable dt_SOLE = new DataTable();
                        dt_SOLE.Load(Selectcmd.ExecuteReader());
                        if (dt_SOLE.Rows.Count > 0)
                        {
                            foreach (DataRow item in dt_SOLE.Rows)
                            {
                                DataRow dr = dt_TODATA.NewRow();
                                dr["LSNAME"] = item["MZ_NAME"].ToString();
                                dr["LSIDCARD"] = strIDCARD;
                                dr["LSPAY"] = item["PAY_DATA"].ToString();
                                dr["LSDA"] = item["DA"].ToString();
                                dr["LSNOTE"] = "(" + (item["DA_INOUT_GROUP"].ToString() == "IN" ? "入帳" : "沖銷") + ")" + item["NOTE"].ToString();
                                dt_TODATA.Rows.Add(dr);
                            }
                        }
                    }
                    catch (Exception) { throw; }
                    finally { Selectconn.Close(); }
                }

                for (int i = 0; i < 1; i++)
                {
                    DataRow dr = dt_TODATA.NewRow();
                    dr["LSNAME"] = strRETURN_DATA(strIDCARD, strYEAR + i.ToString("00"), "YEARPAY", "MZ_NAME");
                    dr["LSIDCARD"] = strIDCARD;
                    dr["LSPAY"] = strRETURN_DATA(strIDCARD, strYEAR + i.ToString("00"), "YEARPAY", "TOTAL");
                    dr["LSDA"] = strYEAR;
                    dr["LSNOTE"] = strRETURN_DATA(strIDCARD, strYEAR + i.ToString("00"), "YEARPAY", "NOTE");
                    dt_TODATA.Rows.Add(dr);
                }

                for (int i = 0; i < 1; i++)
                {
                    DataRow dr = dt_TODATA.NewRow();
                    dr["LSNAME"] = strRETURN_DATA(strIDCARD, strYEAR + i.ToString("00"), "EFFECT", "MZ_NAME");
                    dr["LSIDCARD"] = strIDCARD;
                    dr["LSPAY"] = strRETURN_DATA(strIDCARD, strYEAR + i.ToString("00"), "EFFECT", "TOTAL");
                    dr["LSDA"] = strYEAR;
                    dr["LSNOTE"] = strRETURN_DATA(strIDCARD, strYEAR + i.ToString("00"), "EFFECT", "NOTE");
                    dt_TODATA.Rows.Add(dr);
                }
            }

            Session["rpt_dt"] = dt_TODATA;
            string tmp_url = "B_rpt.aspx?fn=SearchYearSalary&TPM_FION=" + Request.QueryString["TPM_FION"];
            ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
        }
        */


        //薪資年度總表  VW_PERSONAL_YEARSALARY
        protected void sendYearSalaryRPT()
        {
            List<SqlParameter> ps = new List<SqlParameter>();
            //年份條件
            int intYear = int.Parse(TextBox_YEAR.Text.Trim());
            string strSQL = "SELECT * FROM dbo.VW_PERSONAL_YEARSALARY WHERE 1=1 AND PAY_AD=@PAYAD AND AYEAR=@AYEAR";

            ps.Add(new SqlParameter("PAYAD", DropDownList_PAY_AD.SelectedValue));
            ps.Add(new SqlParameter("AYEAR", TextBox_YEAR.Text.PadLeft(3, '0')));

            if (TextBox_MZ_ID.Text.Length > 0)
            {
                strSQL += " AND IDNO=@IDNO";
                ps.Add(new SqlParameter("IDNO", TextBox_MZ_ID.Text.ToString().ToUpper()));

            }
            if (TextBox_MZ_NAME.Text.Length > 0)
            {
                strSQL += " AND NAME LIKE @NAME";
                ps.Add(new SqlParameter("NAME", "%" + TextBox_MZ_NAME.Text + "%"));
            }
            if (TextBox_MZ_POLNO.Text.Length > 0)
            {
                strSQL += " AND MZ_POLNO=@POLNO";
                ps.Add(new SqlParameter("POLNO", TextBox_MZ_POLNO.Text));
            }
            strSQL += "  ORDER BY AMONTH";

            string sql = SalaryPublic.RegixSQL(strSQL, ps);  
            //DataTable dt_TODATA = o_DBFactory.ABC_toTest.DataSelect(strSQL, ps);
            Session["RPT_SQL_B"] = sql;
            //Session["rpt_dt"] = dt_TODATA;
            Session["payad"] = o_A_KTYPE.CODE_TO_NAME(DropDownList_PAY_AD.SelectedValue, "04");
            string tmp_url = "B_rpt.aspx?fn=SearchYearSalary&TPM_FION=" + Request.QueryString["TPM_FION"];
            ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
        }

        private string strRETURN_DATA(string strIDCARD, string strAMONTH, string strDB, string strDATA)
        {
            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn.Open();
                try
                {
                    string strSQL = String.Empty;
                    switch (strDB)
                    {
                        case "MONTHPAY":
                            strSQL = "SELECT " + strDATA + " FROM B_MONTHPAY_MAIN WHERE IDCARD = '" + strIDCARD + "' AND AMONTH = '" + strAMONTH + "'";
                            break;
                        case "REPAIRPAY":
                            strSQL = "SELECT " + strDATA + " FROM B_REPAIRPAY WHERE IDCARD = '" + strIDCARD + "' AND AMONTH = '" + strAMONTH + "'";
                            break;
                        case "YEARPAY":
                            strSQL = "SELECT " + strDATA + " FROM B_YEARPAY WHERE IDCARD = '" + strIDCARD + "' AND AYEAR = '" + strAMONTH + "'";
                            break;
                        case "EFFECT":
                            strSQL = "SELECT " + strDATA + " FROM B_EFFECT WHERE IDCARD = '" + strIDCARD + "' AND AYEAR = '" + strAMONTH + "'";
                            break;
                    }
                    DataTable dt = new DataTable();
                    SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
                    dt.Load(Selectcmd.ExecuteReader(CommandBehavior.CloseConnection));
                    if (dt.Rows.Count == 1)
                    { return dt.Rows[0][strDATA].ToString(); }
                    else
                    { return ""; }
                }
                catch { return ""; }
                finally
                {
                    Selectconn.Close();//XX2013/06/18 
                    Selectconn.Dispose();
                }
            }

        }
    }
}
