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
    public partial class B_SalaryYear_Endbonus3 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            

            Page.MaintainScrollPositionOnPostBack = true;
            if (!Page.IsPostBack)
            {
                SalaryPublic.checkPermission();
                SalaryPublic.fillDropDownList(ref DropDownList_PAY_AD);
                SalaryPublic.fillUnitDropDownList(ref DropDownList_MZ_UNIT, DropDownList_PAY_AD.SelectedValue);
                //SalaryYear_Endbonus.boolLockDB_Update();
            }
        }

        protected void btEFFECTTable_Click(object sender, EventArgs e)
        {
            string sql = "SELECT * FROM VW_ALL_YEARPAY_DATA WHERE PAY_AD=@PAY_AD";
            List<SqlParameter> ops = new List<SqlParameter>();

            ops.Add(new SqlParameter("PAY_AD", DropDownList_PAY_AD.SelectedValue));

            List<string> condition = new List<string>();
            if (DropDownList_AYEARt.SelectedValue != "全部")
            {
                sql += " AND AYEAR=@AYEAR";
                ops.Add(new SqlParameter("AYEAR", DropDownList_AYEARt.SelectedValue));
            }
            if (!string.IsNullOrEmpty(TextBox_VIEWIDt.Text))
            {
                sql += " AND MZ_POLNO=@MZ_POLNO";
                ops.Add(new SqlParameter("MZ_POLNO", TextBox_VIEWIDt.Text));
            }
            if (DropDownList_MZ_UNIT.SelectedIndex > 0)
            {
                sql += " AND MZ_UNIT=@MZ_UNIT";
                ops.Add(new SqlParameter("MZ_UNIT", DropDownList_MZ_UNIT.SelectedValue));
            }

            sql += " ORDER BY AYEAR, MZ_POLNO";

            GridView_YEARENDBONUS.DataSource = o_DBFactory.ABC_toTest.DataSelect(sql, ops);
            GridView_YEARENDBONUS.DataBind();
        }

        protected void GridView_EFFECT_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                Response.Redirect("B_SalaryYear-Endbonus2.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "&y_snid=" + e.CommandArgument);
            }
        }

        protected void DropDownList_PAY_AD_SelectedIndexChanged(object sender, EventArgs e)
        {
            SalaryPublic.fillUnitDropDownList(ref DropDownList_MZ_UNIT, DropDownList_PAY_AD.SelectedValue);
        }
    }
}
