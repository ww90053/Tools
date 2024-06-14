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
    public partial class B_SalaryIncomeTax5 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            { 
                SalaryPublic.checkPermission();

                strAYEAR = ((DateTime.Now.Year - 1911 - 1)).ToString("000");
                SalaryPublic.fillDropDownList(ref DropDownList_PAY_AD);
                payadChanged(sender, e);
            }

        }

        private string strAYEAR
        {
            get
            {
                return txt_year.Text;
            }
            set
            {
                txt_year.Text = value;
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

        protected void btPrint_Click(object sender, EventArgs e)
        {
            Branch bh = new Branch(DropDownList_PAY_AD.SelectedValue);
            Session["taxinvoice"] = bh.taxinvoice;
            Session["taxunit"] = bh.taxunit;
            Session["taxname"] = bh.taxname;
            Session["taxaddr"] = bh.taxaddr;
            Session["taxpers"] = bh.taxpers;
            Session["rpt_dt"] = Tax.getReceipt(bh, strAYEAR, ddl_unit.SelectedIndex > 0 ? ddl_unit.SelectedValue : null, tbID.Text.Length > 0 ? tbID.Text : null, txt_taxType.Text.Length > 0 ? txt_taxType.Text : null);
            string tmp_url = "B_rpt.aspx?fn=SalaryIncomeTax&TPM_FION=" + Request.QueryString["TPM_FION"];
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
        }

        //為了避免以後會用到租賃相關的，這個方法先留著可是沒用到
        private string strRENTNUM_DATA(string strIDCARD_Data, string strSELECT_DATA)
        {
            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn.Open();

                string strSQL = "SELECT " + strSELECT_DATA + " FROM B_MANUFACTURER_BASE WHERE IDCARD = '" + strIDCARD_Data + "' ";
                SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
                DataTable dt = new DataTable();
                dt.Load(Selectcmd.ExecuteReader(CommandBehavior.CloseConnection));
                if (dt.Rows.Count == 1)
                {
                    return dt.Rows[0][strSELECT_DATA].ToString();
                }
                else
                {
                    return "";
                }
            }
        }

        protected void payadChanged(object sender, EventArgs e)
        {
            SalaryPublic.fillUnitDropDownList(ref ddl_unit, strPAY_AD);
        }
    }
}
