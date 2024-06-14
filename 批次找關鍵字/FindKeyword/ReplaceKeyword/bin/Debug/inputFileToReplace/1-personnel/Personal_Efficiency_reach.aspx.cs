using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TPPDDB._1_personnel
{
    public partial class Personal_Efficiency_reach : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            A.set_Panel_EnterToTAB(ref this.Panel1);
            TextBox_MZ_YEAR.Text = (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0');
        }

        protected void DropDownList_MZ_EXAD_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeUnit();
        }

        protected void ChangeUnit()
        {
            DataTable temp = new DataTable();
            string strSQL = "SELECT RTRIM(MZ_KCHI),RTRIM(MZ_KCODE) FROM A_KTYPE WHERE MZ_KTYPE='25' AND MZ_KCODE IN (SELECT MZ_UNIT FROM A_UNIT_AD WHERE MZ_AD='" + DropDownList_MZ_EXAD.SelectedValue + "')";
            temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");
            DropDownList_MZ_EXUNIT.DataSource = temp;
            DropDownList_MZ_EXUNIT.DataTextField = "RTRIM(MZ_KCHI)";
            DropDownList_MZ_EXUNIT.DataValueField = "RTRIM(MZ_KCODE)";
            DropDownList_MZ_EXUNIT.DataBind();
            DropDownList_MZ_EXUNIT.Items.Insert(0, "");
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.close();", true);
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Session["Personal_Effiency1_MZ_AD"] = DropDownList_MZ_EXAD.SelectedValue;
            Session["Personal_Effiency1_MZ_UNIT"] = DropDownList_MZ_EXUNIT.SelectedValue;
            Session["Personal_Effiency1_MZ_YEAR"] = TextBox_MZ_YEAR.Text.Trim();
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.opener.location.href='Personal_Efficiency1.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "';window.close();", true);
        }
    }
}
