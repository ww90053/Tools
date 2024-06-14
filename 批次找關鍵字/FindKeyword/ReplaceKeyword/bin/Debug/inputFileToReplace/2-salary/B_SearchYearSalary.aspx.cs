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

namespace TPPDDB._2_salary
{
    public partial class B_SearchYearSalary : System.Web.UI.Page
    {
        DataTable temp = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (!IsPostBack)
            { 
                SalaryPublic.checkPermission();
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
                    Label_TITLE.Text = "個人薪資年度總表";
                    break;
                default:
                    TPMPermissions._boolErrorData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "ADUXXX0001");
                    Response.Redirect("~/ErrorPage.aspx?erid=ADUXXX0001");
                    break;
            }
        }

        protected void Button_SEARCH_Click(object sender, EventArgs e)
        {
            if (TextBox_YEAR.Text.Length != 3)
            {
                Label_MSG.Text = "日期格式錯誤";
            }
            else
            {
                Session["MZ_IDforSalary"] = Session["ADPMZ_ID"];
                switch (Request.QueryString["chkType"])
                {
                    case "1":
                        if (!SendYearEndToReport())
                        { return; }
                        break;
                    case "2":
                        if (!SendEffectToReport())
                        { return; }
                        break;
                    case "3":
                        sendYearSalaryRPT();
                        break;
                    default:
                        break;
                }
            }
        }

        protected void Button_CANCEL_Click(object sender, EventArgs e)
        {
            Response.Redirect("B_SearchYearSalary.aspx?chkType=" + Request.QueryString["chkType"] + "&TPM_FION=" + Request.QueryString["TPM_FION"]);
        }

        /// <summary>
        /// 年度新資明細表
        /// </summary>
        protected void sendYearSalaryRPT()
        {
            List<String> lsSQL = new List<string>();
            //年份條件
            int intYear = int.Parse(TextBox_YEAR.Text.Trim());

            //string strSQL = string.Format("SELECT * FROM dbo.VW_SEARCH_YEAR_SALARY WHERE 1=1 AND PAY_AD='{0}' AND LSDA LIKE '{1}' ", DropDownList_PAY_AD.SelectedValue, intYear.ToString().PadLeft(3, '0') + "%");
            string strSQL = string.Format("SELECT * FROM dbo.VW_PERSONAL_YEARSALARY WHERE 1=1 AND IDNO='{0}' AND AYEAR='{1}'", SalaryPublic.strLoginID, TextBox_YEAR.Text.PadLeft(3, '0'));
            strSQL += "  ORDER BY AMONTH";
            Session["RPT_SQL_B"] = strSQL;
            //DataTable dt_TODATA = o_DBFactory.ABC_toTest.Create_Table(strSQL, "SalaryDetail");

            //Session["rpt_dt"] = dt_TODATA;
            //20140701
            Session["payad"] = o_A_KTYPE.CODE_TO_NAME(Session["ADPPAY_AD"].ToString(), "04");
            string tmp_url = "B_rpt.aspx?fn=SearchYearSalary&TPM_FION=" + Request.QueryString["TPM_FION"];
            ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
        }

        
        
        
        private string strAYEAR
        {
            get
            {
                return TextBox_YEAR.Text.ToString();
                //return "098";
            }
        }
        private string strIDCARD
        {
            get
            {
                return Session["ADPMZ_ID"].ToString();
                //return "T222222222";
            }
        }

        /// <summary>
        /// 考績獎金明細表
        /// </summary>
        /// <returns></returns>
        protected bool SendEffectToReport()
        {
            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn.Open();
                try
                {
                    string strSQL = @"SELECT MZ_NAME, IDCARD, MZ_POLNO, AYEAR,  MZ_OCCC,AKC.MZ_KCHI MZ_OCCC_CH, PAY, BOSS_AMONTH, SALARYPAY1, PROFESS, BOSS,ELECTRICPAY
                                      , EXTRA01, WORKP, TECHNICS, FAR, TAX, NOTE, TOTAL 
                                      FROM B_EFFECT 
                                      LEFT JOIN A_KTYPE AKC ON AKC.MZ_KCODE=MZ_OCCC AND AKC.MZ_KTYPE='26' 
                                      WHERE IDCARD = '" + strIDCARD + "' AND AYEAR = '" + strAYEAR + "' AND LOCKDB='Y'";
                    SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
                    DataTable rpt_dt = new DataTable();
                    rpt_dt.Load(Selectcmd.ExecuteReader());
                    if (rpt_dt.Rows.Count == 0)
                    {
                        Label_MSG.Text = "輸入年份查無資料";
                    }
                    else
                    {
                        Session["RPT_SQL_B"] = strSQL;


                       
                        string tmp_url = "B_rpt.aspx?fn=PersonalEffectDetail&TPM_FION=" + Request.QueryString["TPM_FION"];
                        ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
                    }
                    return true;
                }
                catch (Exception) { return false; }
                finally
                {
                    Selectconn.Close(); //XX2013/06/18 
                    Selectconn.Dispose();
                }
            }
        }

        /// <summary>
        /// 年終獎金
        /// 增加只查已關帳資料 20190131 by sky
        /// </summary>
        /// <returns></returns>
        protected bool SendYearEndToReport()
        {
            if (TextBox_YEAR.Text.Length == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('請輸入查詢的資料年度')", true);
                return false;
            }


            List<SqlParameter> ops = new List<SqlParameter>();
            string sql = string.Format("SELECT * FROM VW_ALL_YEARPAY_DATA WHERE 1=1 AND IDNO='{0}' AND AYEAR='{1}' AND LOCKDB='Y'", SalaryPublic.strLoginID, TextBox_YEAR.Text);
            Session["RPT_SQL_B"] = sql;
            //ops.Add(new SqlParameter("IDCARD", SalaryPublic.strLoginID));
            //ops.Add(new SqlParameter("AYEAR", TextBox_YEAR.Text));

            //DataTable rpt_dt = o_DBFactory.ABC_toTest.DataSelect(sql, ops);
            DataTable rpt_dt = o_DBFactory.ABC_toTest.Create_Table (sql, "get");
            if (rpt_dt.Rows.Count == 0)
            {
                Label_MSG.Text = "此搜尋條件查無資料";
                Label_MSG.Visible = true;
                return false;
            }

            //Session["rpt_dt"] = rpt_dt;
            string tmp_url = "B_rpt.aspx?fn=SalaryYear-Endbonus&TPM_FION=" + Request.QueryString["TPM_FION"];
            ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
            return true;
        }
    }
}
