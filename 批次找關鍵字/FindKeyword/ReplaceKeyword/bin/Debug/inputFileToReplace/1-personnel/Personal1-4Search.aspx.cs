using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace TPPDDB._1_personnel
{
    public partial class Personal1_4Search : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btOK_Click(object sender, EventArgs e)
        {
            HttpCookie Cookie1 = new HttpCookie("Personal1_4Search_ID");
            Cookie1.Value = TPMPermissions._strEncood(TextBox_MZ_ID.Text.Trim());
            Response.Cookies.Add(Cookie1);

            string AD;
            string UNIT;

            if (!string.IsNullOrEmpty(DropDownList_AD.SelectedValue))
                AD = DropDownList_AD.SelectedValue;
            else
                AD = "";

            if (!string.IsNullOrEmpty(DropDownList_UNIT.SelectedValue))
                UNIT = DropDownList_UNIT.SelectedValue;
            else
                UNIT = "";

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.opener.location.href='Personal1-4.aspx?AD=" + AD + "&UNIT=" + UNIT + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "';window.close();", true);
        }

        protected void btLeave_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.close();", true);
        }

        protected void DropDownList_UNIT_DataBound(object sender, EventArgs e)
        {
            ListItem li = new ListItem(" ", "");
            DropDownList_UNIT.Items.Insert(0, li);
        }

        protected void DropDownList_AD_DataBound(object sender, EventArgs e)
        {
            DropDownList_AD.Items.Insert(0, new ListItem(" ", ""));
        }

        protected void DropDownList_AD_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownList_AD.SelectedValue == "")
            {
                DropDownList_UNIT.SelectedValue = "";
                DropDownList_UNIT.Enabled = false;
            }
        }
    }
}
