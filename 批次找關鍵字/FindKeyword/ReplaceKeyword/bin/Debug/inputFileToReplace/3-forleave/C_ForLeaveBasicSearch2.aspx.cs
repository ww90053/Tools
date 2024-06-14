using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TPPDDB._3_forleave
{
    public partial class C_ForLeaveBasicSearch2 : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
             
            ViewState["TableName"] = Request["TableName"];

           

            if (!IsPostBack)
            {
 
                C.set_Panel_EnterToTAB(ref this.Panel1);

                switch (o_a_Function.strGID(Session["ADPMZ_ID"].ToString()))
                {
                    case "A":
                    case "B":
                        DropDownList_MZ_EXAD.DataBind();
                        DropDownList_MZ_EXAD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                        ChangeUnit();
                        break;
                    case "C":
                        DropDownList_MZ_EXAD.DataBind();
                        DropDownList_MZ_EXAD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                        DropDownList_MZ_EXAD.Enabled = false;
                        ChangeUnit();
                        break;
                    case "D":                       
                    case "E":
                        DropDownList_MZ_EXAD.DataBind();
                        DropDownList_MZ_EXAD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                        DropDownList_MZ_EXAD.Enabled = false;
                        ChangeUnit();
                        DropDownList_MZ_EXUNIT.Text = Session["ADPMZ_EXUNIT"].ToString();
                        DropDownList_MZ_EXUNIT.Enabled = false;
                        break;
                }

                if (ViewState["TableName"].ToString() == "CHECK")
                {
                    Panel1.Visible = true;
                }
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.close();", true);
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            HttpCookie Cookie1 = new HttpCookie("ForLeaveBasicSearch_ID");
            Cookie1.Value = TPMPermissions._strEncood(TextBox_ID.Text.Trim());
            Response.Cookies.Add(Cookie1);

            HttpCookie Cookie2 = new HttpCookie("ForLeaveBasicSearch_NAME");
            Cookie2.Value = TPMPermissions._strEncood(TextBox_NAME.Text.Trim());
            Response.Cookies.Add(Cookie2);

            string EXUNIT = "";
            string EXAD = "";

            if (DropDownList_MZ_EXAD.SelectedValue == "請選擇")
            {
                EXAD = "";
            }
            else
            {
                EXAD = DropDownList_MZ_EXAD.SelectedValue;
            }

            if (DropDownList_MZ_EXUNIT.SelectedValue == "請選擇")
            {
                EXUNIT = "";
            }
            else
            {
                EXUNIT = DropDownList_MZ_EXUNIT.SelectedValue;
            }

            if (ViewState["TableName"].ToString() == "CHECK")
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.opener.location.href='C_ForLeaveOvertime_Leave_Check.aspx?MZ_EXAD=" + EXAD + "&MZ_EXUNIT=" + EXUNIT + "';window.close();", true);
            }
            else if (ViewState["TableName"].ToString() == "ReviewManagement")
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.opener.location.href='C_ReviewManagement.aspx?MZ_EXAD=" + EXAD + "&MZ_EXUNIT=" + EXUNIT + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "';window.close();", true);
            }
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
            DropDownList_MZ_EXUNIT.Items.Insert(0, "請選擇");
        }
    }
}
