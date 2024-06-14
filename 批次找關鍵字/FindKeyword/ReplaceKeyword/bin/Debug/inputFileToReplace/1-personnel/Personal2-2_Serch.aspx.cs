using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TPPDDB._1_personnel
{
    public partial class Personal2_2_Serch : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //A.set_Panel_EnterToTAB(ref this.Panel1);
                A.fill_MZ_PRID(DropDownList_MZ_PRID, 1);


                switch (o_a_Function.strGID(Session["ADPMZ_ID"].ToString()))
                {
                    case "A":
                    case "B":

                        break;
                    case "C":
                        //DropDownList_MZ_PRID.DataBind();
                        DropDownList_MZ_PRID.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                        //matthew 中和分局進來要可以看到中一&中二
                        if (Session["ADPMZ_EXAD"].ToString() == "382133600C")
                        {
                            DropDownList_MZ_PRID.Items.Clear();
                            string strSQL = "";
                            strSQL = "SELECT MZ_PRID,MZ_AD FROM A_CHKAD where MZ_AD in ('382133400C','382133500C','382133600C')";
                            DataTable temp = new DataTable();
                            temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");

                            DropDownList_MZ_PRID.DataSource = temp;

                            DropDownList_MZ_PRID.DataTextField = "MZ_PRID";
                            DropDownList_MZ_PRID.DataValueField = "MZ_AD";
                            DropDownList_MZ_PRID.DataBind();
                        }
                        else
                        {
                            DropDownList_MZ_PRID.Enabled = false;
                        }
                        break;
                }



            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
                      
                string PRID = HttpUtility.UrlEncode(DropDownList_MZ_PRID.SelectedItem.Text.Trim());

                //Session["PRK1_PRID"] = DropDownList_MZ_PRID.SelectedItem.Text.Trim();

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "window.opener.location.href='Personal2-2.aspx?PRK1_PRID=" + PRID + "&PRK1_PRID1=" + o_str.tosql(TextBox_MZ_PRID1.Text.Trim()) + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "';window.close();", true);

                //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.opener.location.href='Personal2-2.aspx?PRK1_PRID=" + PRID + "&PRK1_PRID1=" + o_str.tosql(TextBox_MZ_PRID1.Text.Trim()) + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "';window.close();", true);
           
        }


    }
}
