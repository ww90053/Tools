using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TPPDDB._1_personnel
{
    public partial class PersonalSearch1 : System.Web.UI.Page
    {
        string TableName;
        
        protected void Page_Load(object sender, EventArgs e)
        {
           
            TableName = Request["TableName"];

            if (!IsPostBack)
            { 
                A.set_Panel_EnterToTAB(ref this.Panel1);
                A.fill_AD(DropDownList_MZ_AD );

                //if (Request["TableName"].ToString() == "POSIT")
                //{
                    switch (o_a_Function.strGID(Session["ADPMZ_ID"].ToString()))
                    {
                        case "A":
                        case "B":
                            break;
                        case "C":
                            //if (TableName == "POSIT")
                            //{
                                DropDownList_MZ_AD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                                //DropDownList_MZ_AD.Enabled = false;
                                A.fill_unit(DropDownList_MZ_UNIT, DropDownList_MZ_AD.SelectedValue);
                            //}
                            //else if (TableName == "PRKB")
                            //{
                            //    Panel1.Visible = false;
                            //}
                            break;
                        case "D":
                        default:
                            Panel1.Visible = false;
                           
                            break;
                        
                        case "E":
                            if (TableName == "POSIT")
                            {
                                DropDownList_MZ_AD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                                DropDownList_MZ_AD.Enabled = false;
                                A.fill_unit(DropDownList_MZ_UNIT, DropDownList_MZ_AD.SelectedValue);
                                DropDownList_MZ_UNIT.Text = Session["ADPMZ_EXUNIT"].ToString();
                                DropDownList_MZ_UNIT.Enabled = false;
                            }
                            else if (TableName == "PRKB")
                            {
                                Panel1.Visible = false;
                            }
                            break;
                    }
                //}
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string AD;
            string UNIT;


            AD = DropDownList_MZ_AD.SelectedValue;


            UNIT = DropDownList_MZ_UNIT.SelectedValue;

            if (TableName == "PRKB")
            {
                Session["PRKB_ID"] = TextBox_ID.Text.Trim().ToUpper();

                Session["PRKB_NAME"] = TextBox_NAME.Text.Trim();

                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.opener.location.href='Personal2-1.aspx?AD=" + AD + "&UNIT=" + UNIT + "&NO=" + TextBox_NO.Text.Trim() + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "';window.close();", true);
            }
            else if (TableName == "POSIT")
            {
                Session["POSIT_ID"] = TextBox_ID.Text.Trim().ToUpper();

                Session["POSIT_NAME"] = TextBox_NAME.Text.Trim();

                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.opener.location.href='Personal3-1.aspx?AD=" + AD + "&UNIT=" + UNIT + "&NO=" + TextBox_NO.Text.Trim() + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "';window.close();", true);
            }
        }

        //protected void ChangeUnit()
        //{
        //    DataTable temp = new DataTable();
        //    string strSQL = "SELECT RTRIM(MZ_KCHI),RTRIM(MZ_KCODE) FROM A_KTYPE WHERE MZ_KTYPE='25' AND MZ_KCODE IN (SELECT MZ_UNIT FROM A_UNIT_AD WHERE MZ_AD='" + DropDownList_MZ_AD.SelectedValue + "')";
        //    temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");
        //    DropDownList_MZ_UNIT.DataSource = temp;
        //    DropDownList_MZ_UNIT.DataTextField = "RTRIM(MZ_KCHI)";
        //    DropDownList_MZ_UNIT.DataValueField = "RTRIM(MZ_KCODE)";
        //    DropDownList_MZ_UNIT.DataBind();
        //    DropDownList_MZ_UNIT.Items.Insert(0, "");
        //}

        protected void Button2_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.close();", true);
        }

        protected void DropDownList_MZ_EXAD_SelectedIndexChanged(object sender, EventArgs e)
        {
            A.fill_unit(DropDownList_MZ_UNIT, DropDownList_MZ_AD.SelectedValue);

        }

        protected void DropDownList_MZ_UNIT_DataBound(object sender, EventArgs e)
        {
            DropDownList_MZ_UNIT.Items.Insert(0, "");
        }
    }
}
