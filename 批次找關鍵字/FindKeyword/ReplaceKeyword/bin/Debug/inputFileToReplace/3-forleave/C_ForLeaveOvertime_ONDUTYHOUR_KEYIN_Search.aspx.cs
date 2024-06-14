using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TPPDDB._3_forleave
{
    public partial class C_ForLeaveOvertime_ONDUTYHOUR_KEYIN_Search : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ViewState["TableName"] = Request["TableName"];
            if (!IsPostBack)
            {
                
                Label_MZ_MONTH.Text = "輪值月份範例：" + (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0') + (DateTime.Now.Month).ToString().PadLeft(2, '0');

               
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.close();", true);
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            HttpCookie Cookie1 = new HttpCookie("ONDUTYHOUR_KEYIN_Search_ID");
            Cookie1.Value = TPMPermissions._strEncood(TextBox_ID.Text.Trim());
            Response.Cookies.Add(Cookie1);

            HttpCookie Cookie2 = new HttpCookie("ONDUTYHOUR_KEYIN_Search_NAME");
            Cookie2.Value = TPMPermissions._strEncood(TextBox_NAME.Text.Trim());
            Response.Cookies.Add(Cookie2);

            string EXUNIT = "";
            string EXAD = "";
            string MZ_MONTH = "";
            if (DropDownList_MZ_EXAD.SelectedValue.Trim() == "")
            {
                EXAD = "";
            }
            else
            {
                EXAD = DropDownList_MZ_EXAD.SelectedValue.Trim();
            }
            if (DropDownList_MZ_EXUNIT.SelectedValue.Trim() == "")
            {
                EXUNIT = "";
            }
            else
            {
                EXUNIT = DropDownList_MZ_EXUNIT.SelectedValue.Trim();
            }
            if (string.IsNullOrEmpty(TextBox_MZ_MONTH.Text))
            {
                MZ_MONTH = "";
            }
            else
            {
                MZ_MONTH = TextBox_MZ_MONTH.Text;
            }
            if (ViewState["TableName"].ToString() == "ONDUTYHOUR")
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.opener.location.href='C_ForLeaveOvertime_ONDUTYHOUR_KEYIN.aspx?MZ_EXAD=" + EXAD + "&MZ_EXUNIT=" + EXUNIT + "&MZ_MONTH=" + MZ_MONTH + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "';window.close();", true);
            }
            //else if (ViewState["TableName"].ToString() == "CARDSET")
            //{
            //    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.opener.location.href='C_CARDSET.aspx?MZ_EXAD=" + EXAD + "&MZ_EXUNIT=" + EXUNIT + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "';window.close();", true);
            //}
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
            DropDownList_MZ_EXUNIT.Items.Insert(0, "請選擇");
        }

        protected void DropDownList_MZ_EXAD_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeUnit();
        }

    }
}
