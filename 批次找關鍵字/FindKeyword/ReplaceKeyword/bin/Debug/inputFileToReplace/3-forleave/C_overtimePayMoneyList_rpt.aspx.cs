using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TPPDDB._3_forleave
{
    public partial class C_overtimePayMoneyList_rpt : System.Web.UI.Page
    {
        int TPM_FION=0;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btPrint_Click(object sender, EventArgs e)
        {
            DataTable rpt_dt = new DataTable();

            string strSQL1 = " AND C_DUTYMONTHOVERTIME_HOUR.MZ_AD='" + DropDownList_AD.SelectedValue + "' AND MZ_YEAR='" + TextBox_MZ_DATE1.Text.Substring(0, 3) + "' AND MZ_MONTH='" + TextBox_MZ_DATE1.Text.Substring(3, 2) + "'";

            if (!string.IsNullOrEmpty(DropDownList_UNIT.SelectedValue))
            {
                strSQL1 += " AND C_DUTYMONTHOVERTIME_HOUR.MZ_UNIT='" + DropDownList_UNIT.SelectedValue + "'";
            }

            string strSQL = @"SELECT C_DUTYMONTHOVERTIME_HOUR.*,MZ_OCCC_CH MZ_OCCC,MZ_NAME FROM C_DUTYMONTHOVERTIME_HOUR
                             LEFT JOIN VW_A_DLBASE_S1 ON C_DUTYMONTHOVERTIME_HOUR.MZ_ID = VW_A_DLBASE_S1.MZ_ID 
                             WHERE 1=1 " + strSQL1;

            rpt_dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "VALUEGET");

            //if (rpt_dt.Rows.Count > 0)
            //{
            //    for (int i = 0; i < rpt_dt.Rows.Count; i++)
            //    {
            //        rpt_dt.Rows[i]["MZ_OCCC"] = o_A_DLBASE.OCCC(rpt_dt.Rows[i]["MZ_ID"].ToString());
            //        rpt_dt.Rows[i]["MZ_NAME"] = o_A_DLBASE.CNAME(rpt_dt.Rows[i]["MZ_ID"].ToString());
            //    }
            //}

            Session["rpt_dt"] = rpt_dt;

            Session["TITLE"] = string.Format("{0}{1} {2}年{3}月警察人員超勤印領表", DropDownList_AD.SelectedItem.Text, DropDownList_UNIT.SelectedItem.Text, int.Parse(TextBox_MZ_DATE1.Text.Substring(0, 3)), TextBox_MZ_DATE1.Text.Substring(3, 2));

            string tmp_url = "C_rpt.aspx?fn=OvertimePayMoneyList&TPM_FION=" + TPM_FION;

            ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            TextBox_MZ_DATE1.Text = string.Empty;
        }

        protected void DropDownList_UNIT_DataBound(object sender, EventArgs e)
        {
            DropDownList_UNIT.Items.Insert(0, new ListItem(" ", ""));
        }
    }
}
