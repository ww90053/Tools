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
    public partial class B_savelist_rpt : System.Web.UI.Page
    {
        string strSQL;
        DataTable temp = new DataTable();
        DataTable rpt_dt;
        int TPM_FION=0;
        protected void Page_Load(object sender, EventArgs e)
        {
          
            if (!IsPostBack)
            { 
                SalaryPublic.checkPermission();

                SalaryPublic.fillDropDownList(ref this.DropDownList_AD);
                TextBox_YEAR.Text = SalaryPublic.strRepublicDate().Substring(0, 3);
            }
        }

        private String strAD
        {
            get
            {
                return DropDownList_AD.SelectedValue;
            }
        }

        private string strAYEAR
        {
            get
            {
                return TextBox_YEAR.Text;
            }
        }

        private string strMONTH
        {
            get
            {
                return DropDownList_MONTH.SelectedValue;
            }
        }

        protected void Button_SEARCH_Click(object sender, EventArgs e)
        {
            rpt_dt = rpt_dt_init();
            //發薪機關及編制單位下的所有人。
            string date = strAYEAR + strMONTH.PadLeft(2, '0');
            strSQL = string.Format("SELECT * FROM VW_SAVELIST WHERE PAY_AD='{0}' AND AMONTH='{1}' ORDER BY MZ_POLNO",
                                    strAD, date);
            DataTable dt = new DataTable();
            dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
            int count = 1;
            int totalPay = 0;

            if (dt.Rows.Count > 0)
            {
                int page = 1;
                int rownum = 0;
                int index = 0;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    totalPay += int.Parse(dt.Rows[i]["PAY"].ToString());

                    index = (count - (90 * (page - 1)));

                    if (index <= 30)
                    {
                        DataRow dr1 = rpt_dt.NewRow();
                        dr1["PAGE1"] = page;
                        dr1["NUM"] = 30 * (page - 1) + rownum;
                        rownum++;
                        dr1["NAME1"] = dt.Rows[i]["NAME"];
                        dr1["ACCOUNT1"] = dt.Rows[i]["STOCKPILE_BANKID"];
                        dr1["PAY1"] = dt.Rows[i]["PAY"];
                        dr1["paysum"] = dt.Rows[i]["PAY"];
                        rpt_dt.Rows.Add(dr1);
                        if (count == 30)
                        {
                            rownum = 0;
                        }
                    }
                    else if (index > 30 && index <= 60)
                    {
                        rpt_dt.Rows[30 * (page - 1) + rownum]["NAME2"] = dt.Rows[i]["NAME"];
                        rpt_dt.Rows[30 * (page - 1) + rownum]["ACCOUNT2"] = dt.Rows[i]["STOCKPILE_BANKID"];
                        rpt_dt.Rows[30 * (page - 1) + rownum]["PAY2"] = dt.Rows[i]["PAY"];
                        rpt_dt.Rows[30 * (page - 1) + rownum]["paysum"] = int.Parse(dt.Rows[i]["PAY"].ToString()) + int.Parse(rpt_dt.Rows[30 * (page - 1) + rownum]["paysum"].ToString());

                        rownum++;
                        if (count == 60)
                        {
                            rownum = 0;
                        }
                    }
                    else
                    {
                        rpt_dt.Rows[30 * (page - 1) + rownum]["NAME3"] = dt.Rows[i]["NAME"];
                        rpt_dt.Rows[30 * (page - 1) + rownum]["ACCOUNT3"] = dt.Rows[i]["STOCKPILE_BANKID"];
                        rpt_dt.Rows[30 * (page - 1) + rownum]["PAY3"] = dt.Rows[i]["PAY"];
                        rpt_dt.Rows[30 * (page - 1) + rownum]["paysum"] = int.Parse(dt.Rows[i]["PAY"].ToString()) + int.Parse(rpt_dt.Rows[30 * (page - 1) + rownum]["paysum"].ToString());

                        rownum++;
                        if (count == 90)
                        {
                            rownum = 0;
                        }
                    }

                    if (count % 90 == 0)
                        page++;

                    count++;
                }
            }
            Session["TOTALPEOPLE"] = count - 1;
            Session["TOTALPAY"] = totalPay;
            Session["rpt_dt"] = rpt_dt;
            Session["TITLE"] = o_A_KTYPE.CODE_TO_NAME(strAD, "04") + " " + strAYEAR + "年" + strMONTH.PadLeft(2, '0') + "月 職員優惠存款名冊";
            string tmp_url = "B_rpt.aspx?fn=savelist&TPM_FION=" + TPM_FION;
            ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
        }

        protected DataTable rpt_dt_init()
        {
            rpt_dt = new DataTable();
            rpt_dt.Columns.Add("NUM", typeof(string));
            rpt_dt.Columns.Add("PAGE1", typeof(string));
            rpt_dt.Columns.Add("NAME1", typeof(string));
            rpt_dt.Columns.Add("ACCOUNT1", typeof(string));
            rpt_dt.Columns.Add("PAY1", typeof(int));
            rpt_dt.Columns.Add("NAME2", typeof(string));
            rpt_dt.Columns.Add("ACCOUNT2", typeof(string));
            rpt_dt.Columns.Add("PAY2", typeof(int));
            rpt_dt.Columns.Add("NAME3", typeof(string));
            rpt_dt.Columns.Add("ACCOUNT3", typeof(string));
            rpt_dt.Columns.Add("PAY3", typeof(int));
            rpt_dt.Columns.Add("paysum", typeof(int));
            return rpt_dt;
        }

        protected void Button_CANCEL_Click(object sender, EventArgs e)
        {
            Response.Redirect("B_savelist_rpt.aspx");
        }
    }
}
