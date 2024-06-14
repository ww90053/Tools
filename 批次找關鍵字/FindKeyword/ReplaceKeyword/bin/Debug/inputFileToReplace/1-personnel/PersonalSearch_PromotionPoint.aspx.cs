using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Data;
using AjaxControlToolkit;


namespace TPPDDB._1_personnel
{
    public partial class PersonalSearch_PromotionPoint : System.Web.UI.Page
    {
       

        protected void Page_Load(object sender, EventArgs e)
        {
            
           
            if (!IsPostBack)
            {
                A.set_Panel_EnterToTAB(ref this.Panel1);

                txt_Year.Text = (DateTime.Now.Year - 1911).ToString();

                if (Request["TableName"].ToString() == "DLBASE")
                {
                    switch (o_a_Function.strGID(Session["ADPMZ_ID"].ToString()))
                    {
                        case "A":
                        case "B":

                            break;
                        case "C":
                            if (Session["ADPMZ_EXAD"].ToString() == "382130300C" || Session["ADPMZ_EXAD"].ToString() == "382130100C")
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
                if (Request["TableName"].ToString() == "TP_PER")
                {
                    Label_AD.Text = "編制機關";
                    Label_UNIT.Text = "編制單位";
                }
                else if (Request["TableName"].ToString() == "POWER")
                {
                    t1.Attributes.Add("style", "width: 400px; display : ;");
                }
                else if (Request["TableName"].ToString() == "REV_BASE")
                {
                    t2.Attributes.Add("style", "width: 400px; display : ;");
                    RequiredFieldValidator1.Enabled = true;
                    TextBox_MZ_YEAR.Text = (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0');
                }

            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.close();", true);
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string AD;
            string UNIT;
            string YEAR = txt_Year.Text;
            if (DropDownList_MZ_EXAD.SelectedValue.Trim() == "")
            {
                AD = "";
            }
            else
            {
                AD = DropDownList_MZ_EXAD.SelectedValue.Trim();
            }
            if (DropDownList_MZ_EXUNIT.SelectedValue.Trim() == "")
            {
                UNIT = "";
            }
            else
            {
                UNIT = DropDownList_MZ_EXUNIT.SelectedValue.Trim();
            }


            Session["PersonalSearch_ID"] = TextBox_ID.Text.Trim().ToUpper();

            Session["PersonalSearch_NAME"] = TextBox_NAME.Text.Trim();

            HttpCookie Cookie1 = new HttpCookie("PersonalSearch_ID");
            Cookie1.Value = TPMPermissions._strEncood(TextBox_ID.Text.Trim());
            Response.Cookies.Add(Cookie1);

            HttpCookie Cookie2 = new HttpCookie("PersonalSearch_NAME");
            Cookie2.Value = TPMPermissions._strEncood(TextBox_NAME.Text.Trim());
            Response.Cookies.Add(Cookie2);

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.opener.location.href='Personal_PromotionPoint.aspx?MZ_AD=" + AD + "&MZ_UNIT=" + UNIT + "&MZ_YEAR=" + YEAR + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "';window.close();", true);
        }

        protected void DropDownList_MZ_EXAD_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeUnit();
        }

        protected void ChangeUnit()
        {
            if (Session["ADPMZ_EXAD"].ToString() == "382130300C" || Session["ADPMZ_EXAD"].ToString() == "382130100C")
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
                        string strSQL = "SELECT RTRIM(MZ_KCHI),RTRIM(MZ_KCODE) FROM A_KTYPE WHERE MZ_KTYPE='25' AND dbo.SUBSTR(MZ_KCODE,1,2)='DG' AND MZ_KCODE IN (SELECT MZ_UNIT FROM A_UNIT_AD WHERE MZ_AD='" + DropDownList_MZ_EXAD.SelectedValue + "')";
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
                        string strSQL = "SELECT RTRIM(MZ_KCHI),RTRIM(MZ_KCODE) FROM A_KTYPE WHERE MZ_KTYPE='25' AND dbo.SUBSTR(MZ_KCODE,1,2)='DH' AND MZ_KCODE IN (SELECT MZ_UNIT FROM A_UNIT_AD WHERE MZ_AD='" + DropDownList_MZ_EXAD.SelectedValue + "')";
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
    }
}
