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
    public partial class B_taxdetail_rpt : System.Web.UI.Page
    {
        
        DataTable temp = new DataTable();
        DataTable rpt_dt;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            { 
                SalaryPublic.checkPermission();
                SalaryPublic.fillDropDownList(ref this.DropDownList_AD);
                TextBox_YEAR.Text = SalaryPublic.strRepublicLastYear();
                DropDownList_AD_SelectedIndexChanged(sender, e);
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

        private string strUNIT
        {
            get
            {
                return DropDownList_UNIT.SelectedValue;
            }
        }

        protected void Button_SEARCH_Click(object sender, EventArgs e)
        {
            string sql;
            string subsql;
            List<SqlParameter> ps = new List<SqlParameter>();

            subsql = "SELECT VW_TAX_DETAIL.* FROM VW_TAX_DETAIL WHERE LEN(RTRIM(TAX_TYPE)) > 0 AND AYEAR=@year AND PAY_AD=@payad ";
            if (DropDownList_UNIT.SelectedIndex != 0)
            {
                subsql += " AND MZ_UNIT=@unit ";
                ps.Add(new SqlParameter("unit", strUNIT));
            }

            switch (rblType.SelectedValue)
            {
                case "50":
                    subsql += "AND TAX_TYPE=@type";
                    ps.Add(new SqlParameter("type", rblType.SelectedValue));
                    break;

                case "其他格式":
                    subsql += "AND TAX_TYPE !='50'";
                    break;

                case "全部":
                    break;
            }
            
            sql = string.Format("SELECT ROWNUM NUM, VW.* FROM ({0} ORDER BY MZ_UNIT, MZ_POLNO) VW  order by NUM", subsql);

            ps.Add(new SqlParameter("year", strAYEAR));
            ps.Add(new SqlParameter("payad", strAD));

            Session["rpt_dt"] = Salary.addPageNumber(o_DBFactory.ABC_toTest.DataSelect(sql, ps), 20, "PAY_UNIT");
            Session["SalaryReportAD"] = o_A_KTYPE.CODE_TO_NAME(strAD, "04");
            Session["TITLE"] = " " + strAYEAR + "年";
            string tmp_url = "B_rpt.aspx?fn=tax_detail&TPM_FION=" + Request.QueryString["TPM_FION"];
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
        }

        protected DataTable rpt_dt_init()
        {
            rpt_dt = new DataTable();
            rpt_dt.Columns.Add("NUM", typeof(string));
            rpt_dt.Columns.Add("MZ_OCCC", typeof(string));
            rpt_dt.Columns.Add("MZ_NAME", typeof(string));
            rpt_dt.Columns.Add("MONTHPAY", typeof(int));
            rpt_dt.Columns.Add("REPAIR", typeof(int));
            rpt_dt.Columns.Add("YEARPAY", typeof(int));
            rpt_dt.Columns.Add("EFFECT", typeof(int));
            rpt_dt.Columns.Add("SOLE", typeof(int));
            rpt_dt.Columns.Add("ADD_SUM", typeof(int));
            rpt_dt.Columns.Add("MONTHPAYTAX", typeof(int));
            rpt_dt.Columns.Add("REPAIRTAX", typeof(int));
            rpt_dt.Columns.Add("YEARPAYTAX", typeof(int));
            rpt_dt.Columns.Add("EFFECTTAX", typeof(int));
            rpt_dt.Columns.Add("SOLETAX", typeof(int));
            rpt_dt.Columns.Add("DES_SUM", typeof(int));
            rpt_dt.Columns.Add("PAGE", typeof(int));
            return rpt_dt;
        }

        protected void DropDownList_AD_SelectedIndexChanged(object sender, EventArgs e)
        {
            SalaryPublic.fillUnitDropDownList(ref DropDownList_UNIT, DropDownList_AD.SelectedValue);
        }

        protected void Button_CANCEL_Click(object sender, EventArgs e)
        {
            Response.Redirect("B_taxlist_rpt.aspx");
        }
    }
}

