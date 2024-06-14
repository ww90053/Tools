using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TPPDDB._3_forleave
{
    public partial class ForLeaveBasicSearch : System.Web.UI.Page
    {
        string C_strGID = ""; 

        protected void Page_Load(object sender, EventArgs e)
        {
            ViewState["TableName"] = Request["TableName"];

          
            

            if (!IsPostBack)
            {  
                //MQ-----------------------20100331
            C.set_Panel_EnterToTAB(ref this.Panel2);
            C.set_Panel_EnterToTAB(ref this.Panel1);
                if (ViewState["TableName"].ToString() == "BASIC")
                {
                    Panel1.Visible = true;
                    C_strGID = o_a_Function.strGID(Session["ADPMZ_ID"].ToString());
                    switch (C_strGID)
                    {
                        case "A":
                        case "B":

                            break;
                        case "C":
                            if (Session["ADPMZ_EXAD"].ToString() == "382130300C" || Session["ADPMZ_EXAD"].ToString() == "382130200C")
                            {
                                DropDownList_MZ_EXAD.AppendDataBoundItems = false;
                                DropDownList_MZ_EXAD.DataBind();
                                DropDownList_MZ_EXAD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                                ChangeUnit();
                            }
                            else
                            {

                                DropDownList_MZ_EXAD.DataBind();
                                DropDownList_MZ_EXAD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                                DropDownList_MZ_EXAD.Enabled = false;
                                ChangeUnit();
                            }
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
                }
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.close();", true);
        }

        protected void Button_Click(object sender, EventArgs e)
        {
            string IDATE1 = string.IsNullOrEmpty(TextBox_MZ_IDATE1.Text.Trim()) ? string.Empty : TextBox_MZ_IDATE1.Text.Trim().Replace("/", string.Empty).PadLeft(7, '0');

            string EXUNIT = "";
            string EXAD = "";

            if (DropDownList_MZ_EXAD.SelectedValue == "")
            {
                EXAD = "";
            }
            else
            {
                EXAD = DropDownList_MZ_EXAD.SelectedValue;
            }

            if (DropDownList_MZ_EXUNIT.SelectedValue == "")
            {
                EXUNIT = "";
            }
            else
            {
                EXUNIT = DropDownList_MZ_EXUNIT.SelectedValue;
            }

            if ((sender as Button).ID == "bt_DLBASE")
            {
                HttpCookie Cookie1 = new HttpCookie("ForLeaveBasicSearch_ID");
                Cookie1.Value = TPMPermissions._strEncood(TextBox_ID.Text.Trim());
                Response.Cookies.Add(Cookie1);

                HttpCookie Cookie2 = new HttpCookie("ForLeaveBasicSearch_NAME");
                Cookie2.Value = TPMPermissions._strEncood(TextBox_NAME.Text.Trim());
                Response.Cookies.Add(Cookie2);

                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.opener.location.href='C_ForLeaveBasic.aspx?SEARCHMODE=DLBASE&MZ_EXAD=" + EXAD + "&MZ_EXUNIT=" + EXUNIT + "&MZ_IDATE1=" + IDATE1 + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "';window.close();", true);
            }
            else
            {
                if (C_strGID == "D")
                {
                    HttpCookie Cookie1 = new HttpCookie("ForLeaveBasicSearch_ID");
                    Cookie1.Value = TPMPermissions._strEncood(Session["ADPMZ_ID"].ToString());
                    Response.Cookies.Add(Cookie1);

                    HttpCookie Cookie2 = new HttpCookie("ForLeaveBasicSearch_NAME");
                    Cookie2.Value = TPMPermissions._strEncood(o_A_DLBASE.CNAME(Session["ADPMZ_ID"].ToString()));
                    Response.Cookies.Add(Cookie2);
                }
                else
                {
                    HttpCookie Cookie1 = new HttpCookie("ForLeaveBasicSearch_ID");
                    Cookie1.Value = TPMPermissions._strEncood(TextBox_ID.Text.Trim());
                    Response.Cookies.Add(Cookie1);

                    HttpCookie Cookie2 = new HttpCookie("ForLeaveBasicSearch_NAME");
                    Cookie2.Value = TPMPermissions._strEncood(TextBox_NAME.Text.Trim());
                    Response.Cookies.Add(Cookie2);
                }
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.opener.location.href='C_bussinesstrip.aspx?MZ_EXAD=" + EXAD + "&MZ_EXUNIT=" + EXUNIT + "&MZ_IDATE1=" + IDATE1 + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "';window.close();", true);
            }
        }

        protected void ChangeUnit()
        {
            if (Session["ADPMZ_EXAD"].ToString() == "382130300C" || Session["ADPMZ_EXAD"].ToString() == "382130200C")
            {
                if (Session["ADPMZ_EXAD"].ToString() == DropDownList_MZ_EXAD.SelectedValue)
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
                else
                {
                    if (Session["ADPMZ_EXAD"].ToString() == "382130300C")
                    {
                        DataTable temp = new DataTable();
                        string strSQL = "SELECT RTRIM(MZ_KCHI),RTRIM(MZ_KCODE) FROM A_KTYPE WHERE MZ_KTYPE='25' AND dbo.SUBSTR(MZ_KCODE,1,2)='2T' AND MZ_KCODE IN (SELECT MZ_UNIT FROM A_UNIT_AD WHERE MZ_AD='" + DropDownList_MZ_EXAD.SelectedValue + "')";
                        temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");
                        DropDownList_MZ_EXUNIT.DataSource = temp;
                        DropDownList_MZ_EXUNIT.DataTextField = "RTRIM(MZ_KCHI)";
                        DropDownList_MZ_EXUNIT.DataValueField = "RTRIM(MZ_KCODE)";
                        DropDownList_MZ_EXUNIT.DataBind();

                        if (DropDownList_MZ_EXUNIT.Items.Count == 0)
                        {

                            DropDownList_MZ_EXAD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                            DropDownList_MZ_EXAD.DataBind();
                        }
                    }
                    else
                    {
                        DataTable temp = new DataTable();
                        string strSQL = "SELECT RTRIM(MZ_KCHI),RTRIM(MZ_KCODE) FROM A_KTYPE WHERE MZ_KTYPE='25' AND dbo.SUBSTR(MZ_KCODE,1,2)='2S' AND MZ_KCODE IN (SELECT MZ_UNIT FROM A_UNIT_AD WHERE MZ_AD='" + DropDownList_MZ_EXAD.SelectedValue + "')";
                        temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");
                        DropDownList_MZ_EXUNIT.DataSource = temp;
                        DropDownList_MZ_EXUNIT.DataTextField = "RTRIM(MZ_KCHI)";
                        DropDownList_MZ_EXUNIT.DataValueField = "RTRIM(MZ_KCODE)";
                        DropDownList_MZ_EXUNIT.DataBind();

                        if (DropDownList_MZ_EXUNIT.Items.Count == 0)
                        {

                            DropDownList_MZ_EXAD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                            DropDownList_MZ_EXAD.DataBind();
                        }
                    }
                }
            }
            else
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
        }

        protected void DropDownList_MZ_EXAD_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeUnit();
        }
    }
}
