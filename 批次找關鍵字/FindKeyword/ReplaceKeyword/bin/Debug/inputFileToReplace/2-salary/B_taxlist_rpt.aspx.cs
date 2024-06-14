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
    public partial class B_taxlist_rpt : System.Web.UI.Page
    {
        //string strSQL;
        DataTable temp = new DataTable();
        DataTable rpt_dt;
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                SalaryPublic.checkPermission();
                SalaryPublic.fillDropDownList(ref this.DropDownList_AD);
                TextBox_YEAR.Text = SalaryPublic.strRepublicLastYear();
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

        protected void Button_SEARCH_Click(object sender, EventArgs e)
        {
            List<SqlParameter> ps = new List<SqlParameter>();
            string strSQL = string.Format("SELECT * FROM VW_TAX_LIST WHERE 1=1 AND PAY_AD=@ad AND AYEAR=@year ",
                                        strAD, strAYEAR);

            ps.Add(new SqlParameter("ad", strAD));
            ps.Add(new SqlParameter("year", strAYEAR));
            

        
            switch(rblType.SelectedValue)
            {
                case "50":
                    strSQL += "AND TAX_TYPE=@type";
                    ps.Add(new SqlParameter("type", rblType.SelectedValue));
                    break;

                case "其他格式":
                    strSQL += "AND TAX_TYPE !='50'";
                    break;

                case "全部":
                    break;


            
            }
           
            strSQL += " ORDER BY UNITCODE";

            DataTable dt = o_DBFactory.ABC_toTest.DataSelect(strSQL, ps);

            Session["rpt_dt"] = Salary.addPageNumber(dt, 20, "PAY_AD");
            Session["TITLE"] = o_A_KTYPE.CODE_TO_NAME(strAD, "04") + "   " + strAYEAR + "年度 員工各項稅額清冊";
            string tmp_url = "B_rpt.aspx?fn=tax_list";
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
        }

        protected DataTable rpt_dt_init()
        {
            rpt_dt = new DataTable();
            rpt_dt.Columns.Add("MZ_UNIT", typeof(string));
            rpt_dt.Columns.Add("PEOPLE_ACCOUNT", typeof(int));
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
            rpt_dt.Columns.Add("page", typeof(string));
            return rpt_dt;
        }

        protected void Button_CANCEL_Click(object sender, EventArgs e)
        {
            Response.Redirect("B_taxlist_rpt.aspx");
        }
    }
}
