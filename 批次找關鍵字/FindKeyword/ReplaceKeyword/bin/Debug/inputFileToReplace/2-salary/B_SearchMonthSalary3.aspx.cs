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
    public partial class B_SearchMonthSalary3 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            

            Label_MSG.Visible = false;

            if (!IsPostBack)
            {
                SalaryPublic.checkPermission();
                SalaryPublic.fillDropDownList(ref this.DropDownList_PAY_AD);//只有權限為A.B能用下拉選單
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


        /// <summary>
        /// 判斷是否還停留在初始化設定
        /// </summary>
        /// <returns></returns>
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

        protected void Button_CANCEL_Click(object sender, EventArgs e)
        {
            Response.Redirect("B_SearchMonthSalary3.aspx?chkType=" + Request.QueryString["chkType"] + "&TPM_FION=" + Request.QueryString["TPM_FION"]);
        }


        //補發薪資明細表  VW_ALL_REPAIR_DATA
        protected void sendRepairToRPT()
        {
            List<String> lsSQL = new List<string>();
            if (TextBox_MZ_ID.Text.Length > 0)
            {
                lsSQL.Add(" IDNO ='" + TextBox_MZ_ID.Text.ToString() + "'");
            }
            if (TextBox_MZ_NAME.Text.Length > 0)
            {
                lsSQL.Add(" NAME ='" + TextBox_MZ_NAME.Text.ToString() + "'");
            }
            if (TextBox_MZ_POLNO.Text.Length > 0)
            {
                lsSQL.Add(" MZ_POLNO ='" + TextBox_MZ_POLNO.Text.ToString() + "'");
            }
            lsSQL.Add(String.Format(" AMONTH ='{0}'", TextBox_YEAR.Text + DropDownList_MONTH.SelectedValue));

            //string strSQL = String.Format("SELECT IDCARD,AMONTH,MZ_POLNO,PAY_AD,MZ_NAME,MZ_OCCC,MZ_SRANK,MZ_SLVC,MZ_SPT,SALARYPAY1,WORKPPAY,PROFESSPAY,BOSSPAY,TECHNICSPAY,BONUSPAY,ADVENTIVEPAY,FARPAY,ELECTRICPAY,INSURANCEPAY,HEALTHPAY,HEALTHPAY1,MONTHPAY_TAX,MONTHPAY,CONCUR3PAY,TAX,EXTRA01,EXTRA02,EXTRA03,EXTRA04,EXTRA05,EXTRA06,EXTRA07,EXTRA08,EXTRA09,NOTE FROM B_REPAIRPAY WHERE PAY_AD ='{0}' {1}", DropDownList_PAY_AD.SelectedValue.ToString(), lsSQL.Count > 0 ? " AND " + String.Join(" AND ", lsSQL.ToArray()) : String.Empty);
            string strSQL = string.Format("SELECT * FROM VW_ALL_REPAIR_DATA WHERE PAY_AD ='{0}' {1}", DropDownList_PAY_AD.SelectedValue.ToString(), lsSQL.Count > 0 ? " AND " + String.Join(" AND ", lsSQL.ToArray()) : String.Empty);

            DataTable dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "RSInfo");
            Session["RPT_SQL_B"] = strSQL;
            if (dt.Rows.Count == 0)
            {
                Label_MSG.Text = "此搜尋條件查無資料";
                Label_MSG.Visible = true;
            }
            else
            {
                Session["TITLE"] = dt.Rows[0]["CHIAD"];
               
                string tmp_url = "B_rpt.aspx?fn=RepairPay&TPM_FION=" + Request.QueryString["TPM_FION"];
                ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
            }
        }

        //每月薪資明細表   VW_ALL_SALARY_DATA
        protected void sendMonthSalaryToRPT()
        {
            List<String> lsSQL = new List<string>();
            if (TextBox_MZ_ID.Text.Length > 0)
            {
                lsSQL.Add(" IDNO ='" + TextBox_MZ_ID.Text.ToString().ToUpper() + "'");
            }
            if (TextBox_MZ_NAME.Text.Length > 0)
            {
                lsSQL.Add(" NAME ='" + TextBox_MZ_NAME.Text.ToString() + "'");
            }
            if (TextBox_MZ_POLNO.Text.Length > 0)
            {
                lsSQL.Add(" MZ_POLNO ='" + TextBox_MZ_POLNO.Text.ToString() + "'");
            }
            lsSQL.Add(String.Format(" AMONTH ='{0}'", TextBox_YEAR.Text + DropDownList_MONTH.SelectedValue));

            string strSQL = String.Format("SELECT * FROM VW_ALL_SALARY_DATA WHERE PAY_AD ='{0}' {1}", DropDownList_PAY_AD.SelectedValue.ToString(), lsSQL.Count > 0 ? " AND " + String.Join(" AND ", lsSQL.ToArray()) : String.Empty);

            DataTable dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "MSInfo");

            Session["RPT_SQL_B"] = strSQL;

            if (dt.Rows.Count == 0)
            {
                Label_MSG.Text = "此搜尋條件查無資料";
                Label_MSG.Visible = true;
            }
            else
            {
                Session["TITLE"] = dt.Rows[0]["CHIAD"];
               
                string tmp_url = "B_rpt.aspx?fn=PersonalMonthSalary&TPM_FION=" + Request.QueryString["TPM_FION"];
                ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
            }
        }
    }
}