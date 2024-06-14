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
    public partial class B_SearchMonthSalary : System.Web.UI.Page
    {
        DataTable temp = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            
            Label_MSG.Visible = false;

            if (!IsPostBack)
            {SalaryPublic.checkPermission();


                TextBox_YEAR.Text = SalaryPublic.strRepublicYear();
            }

            switch (Request.QueryString["chkType"])
            {
                case "1":
                    Label_TITLE.Text = "每月薪資明細表";
                    break;
                case "2":
                    Label_TITLE.Text = "補發薪資明細表";
                    break;
                default:
                    TPMPermissions._boolErrorData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "ADUXXX0001");
                    Response.Redirect("~/ErrorPage.aspx?erid=ADUXXX0001");
                    break;
            }
        }

        protected bool chkYearType()
        {
            if (TextBox_YEAR.Text.Length == 3 && DropDownList_MONTH.SelectedValue != "-1")
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
                        sendMonthSalaryToRPT();
                        break;
                    case "2":
                        sendRepairToRPT();
                        break;
                    default:
                        //strSQL = string.Format("SELECT MM_SNID FROM B_MONTHPAY_MAIN WHERE IDCARD='{0}' AND AMONTH='{1}'", Session["ADPMZ_ID"].ToString(), TextBox_YEAR.Text + DropDownList_MONTH.SelectedValue.PadLeft(2, '0'));
                        break;
                }
            }
            else
            {
                Label_MSG.Text = "輸入年份格式錯誤或未選擇月份";
                Label_MSG.Visible = true;
            }
        }

        protected void Button_CANCEL_Click(object sender, EventArgs e)
        {
            Response.Redirect("B_SearchMonthSalary.aspx?chkType=" + Request.QueryString["chkType"] + "&TPM_FION=" + Request.QueryString["TPM_FION"]);
        }
        /// <summary>
        /// 補發新資
        /// </summary>
        protected void sendRepairToRPT()
        {
            List<String> lsSQL = new List<string>();
            lsSQL.Add(String.Format(" AMONTH ='{0}'", TextBox_YEAR.Text + DropDownList_MONTH.SelectedValue));

            //string strSQL = String.Format("SELECT IDCARD,AMONTH,MZ_POLNO,PAY_AD,MZ_NAME,MZ_OCCC,MZ_SRANK,MZ_SLVC,MZ_SPT,SALARYPAY1,WORKPPAY,PROFESSPAY,BOSSPAY,TECHNICSPAY,BONUSPAY,ADVENTIVEPAY,FARPAY,ELECTRICPAY,INSURANCEPAY,HEALTHPAY,HEALTHPAY1,MONTHPAY_TAX,MONTHPAY,CONCUR3PAY,TAX,EXTRA01,EXTRA02,EXTRA03,EXTRA04,EXTRA05,EXTRA06,EXTRA07,EXTRA08,EXTRA09,NOTE FROM B_REPAIRPAY WHERE PAY_AD ='{0}' {1}", DropDownList_PAY_AD.SelectedValue.ToString(), lsSQL.Count > 0 ? " AND " + String.Join(" AND ", lsSQL.ToArray()) : String.Empty);
            string strSQL = string.Format("SELECT * FROM VW_ALL_REPAIR_DATA WHERE IDNO ='{0}' {1}", SalaryPublic.strLoginID, lsSQL.Count > 0 ? " AND " + String.Join(" AND ", lsSQL.ToArray()) : String.Empty);

            DataTable dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "RSInfo");

            if (dt.Rows.Count == 0)
            {
                Label_MSG.Text = "此搜尋條件查無資料";
                Label_MSG.Visible = true;
            }
            else
            {
                Session["RPT_SQL_B"] = strSQL;
                Session["TITLE"] = dt.Rows[0]["CHIAD"];
               
                string tmp_url = "B_rpt.aspx?fn=RepairPay&TPM_FION=" + Request.QueryString["TPM_FION"];
                ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
            }
        }
        /// <summary>
        /// 每月薪資
        /// </summary>
        protected void sendMonthSalaryToRPT()
        {
            List<String> lsSQL = new List<string>();
            lsSQL.Add(String.Format(" AMONTH ='{0}'", TextBox_YEAR.Text + DropDownList_MONTH.SelectedValue));

            string strSQL = String.Format("SELECT * FROM VW_ALL_SALARY_DATA WHERE IDNO ='{0}' {1}", SalaryPublic.strLoginID, lsSQL.Count > 0 ? " AND " + String.Join(" AND ", lsSQL.ToArray()) : String.Empty);

            DataTable dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "MSInfo");

            if (dt.Rows.Count == 0)
            {
                Label_MSG.Text = "此搜尋條件查無資料";
                Label_MSG.Visible = true;
            }
            else
            {
                Session["RPT_SQL_B"] = strSQL;
                Session["TITLE"] = dt.Rows[0]["CHIAD"];
               // Session["rpt_dt"] = dt;
                string tmp_url = "B_rpt.aspx?fn=PersonalMonthSalary&TPM_FION=" + Request.QueryString["TPM_FION"];
                ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
            }
        }
    }
}
