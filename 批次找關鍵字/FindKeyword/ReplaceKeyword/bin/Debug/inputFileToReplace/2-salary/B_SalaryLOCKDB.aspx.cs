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
    public partial class B_SalaryLOCKDB : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack != true)
            {
                SalaryPublic.checkPermission();
                SalaryPublic.fillDropDownList(ref this.DropDownList_PAY_AD);
                DropDownList_GROUP_SelectedIndexChanged(sender, e);
            }
        }

        private string strPAY_AD
        {
            get
            {
                return DropDownList_PAY_AD.SelectedValue;
            }
            set
            {
                DropDownList_PAY_AD.SelectedValue = value;
            }
        }

        private string strGROUP
        {
            get
            {
                return DropDownList_GROUP.SelectedValue;
            }
            set
            {
                DropDownList_GROUP.SelectedValue = value;
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

        protected void DropDownList_GROUP_SelectedIndexChanged(object sender, EventArgs e)
        {
            batch.Visible = false;

            switch (strGROUP)
            {
                case "SALARY":
                    lb_da.Text = "年月";
                    strDA = (DateTime.Now.Year - 1911).ToString("000") + DateTime.Now.Month.ToString("00");
                    break;

                case "REPAIR":
                    lb_da.Text = "年月";
                    strDA = (DateTime.Now.Year - 1911).ToString("000") + DateTime.Now.Month.ToString("00");
                    batch.Visible = true;
                    break;

                case "SOLE":
                    lb_da.Text = "日期";
                    strDA = (DateTime.Now.Year - 1911).ToString("000") + DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00");
                    batch.Visible = true;
                    break;

                case "YEARPAY":
                case "EFFECT":
                case "TAXES":
                    lb_da.Text = "年度";
                    //減1 計算上年度
                    strDA = ((DateTime.Now.Year - 1911) - 1).ToString("000");
                    break;
            }
        }

        protected void btSave_Click(object sender, EventArgs e)
        {
            switch (strGROUP)
            {
                case "SALARY":
                    Salary.lockDB(strPAY_AD, strDA);
                    break;

                case "REPAIR":
                    if (txt_caseid.Text.Trim().Length == 0)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('批號不可空白')", true);
                        return;
                    }
                    Repair.lockDB(strPAY_AD, strDA, txt_caseid.Text);
                    break;

                case "SOLE":
                    if (txt_caseid.Text.Trim().Length == 0)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('批號不可空白')", true);
                        return;
                    }
                    Sole.lockDB(strPAY_AD, strDA, txt_caseid.Text);
                    break;

                case "YEARPAY":
                    YearEnd.lockDB(strPAY_AD, strDA);
                    break;

                case "EFFECT":
                    Effect.lockDB(strPAY_AD, strDA);
                    break;

                case "TAXES":
                    Tax.lockDB(strPAY_AD, strDA);
                    break;
            }

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('已完成關帳作業')", true);
        }
    }
}
