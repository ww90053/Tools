using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TPPDDB._1_personnel
{
    public partial class PersoanlSerch2 : System.Web.UI.Page
    {
        string TableName;
        

        protected void Page_Load(object sender, EventArgs e)
        {
             TableName = Request["TableName"];
           
           

           

           

            if (!IsPostBack)
            { A.set_Panel_EnterToTAB(ref this.Panel1);
                A.fill_MZ_PRID(DropDownList_MZ_PRID, 1);
               
                if (Request["TableName"].ToString() == "PRK1" || Request["TableName"].ToString() == "PRK2")
                {
                    switch (o_a_Function.strGID(Session["ADPMZ_ID"].ToString()))
                    {
                        case "A":
                        case "B":

                            break;
                        case "C":
                            DropDownList_MZ_PRID.DataBind();
                            DropDownList_MZ_PRID.SelectedValue = Session["ADPMZ_EXAD"].ToString();

                            if (Request["TableName"].ToString() == "PRK2")
                            {
                                ChangeDropDownList_AD();
                            }

                            DropDownList_MZ_PRID.Enabled = false;
                            break;
                    }
                }

                ////
                if (Request["TableName"].ToString() == "POSIT2")
                {
                    Panel2.Visible = true;
                    TextBox_MZ_PRID.Visible = true;
                    btPRID.Visible = true;
                    DropDownList_MZ_PRID.Visible = false;
                }
                else if (Request["TableName"].ToString() == "PRK2")
                {
                    Panel2.Visible = true;
                    TextBox_MZ_PRID.Visible = false;
                    btPRID.Visible = false;
                    DropDownList_MZ_PRID.Visible = true;
                }
                else
                {
                    TextBox_MZ_PRID.Visible = false;
                    btPRID.Visible = false;
                    DropDownList_MZ_PRID.Visible = true;
                }

            }
        }

        protected void ChangeDropDownList_AD()
        {
            string strSQL = "SELECT * FROM Z_CODE_ON WHERE NEW='" + Session["ADPMZ_EXAD"].ToString() + "'";
            DataTable tempDT = new DataTable();
            tempDT = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
             DataTable source = o_DBFactory.ABC_toTest.Create_Table("SELECT MZ_PRID,MZ_AD FROM A_CHKAD WHERE (MZ_AD='" + tempDT.Rows[0][0].ToString() + "' OR MZ_AD='" + tempDT.Rows[0][1].ToString() + "')","get");
            //SqlDataSource1.SelectCommand = "SELECT MZ_PRID,MZ_AD FROM A_CHKAD WHERE (MZ_AD='" + tempDT.Rows[0][0].ToString() + "' OR MZ_AD='" + tempDT.Rows[0][1].ToString() + "')";

            DropDownList_MZ_PRID.DataSource = source;
            DropDownList_MZ_PRID.DataTextField = "MZ_PRID";
            DropDownList_MZ_PRID.DataValueField = "MZ_AD";
            
            DropDownList_MZ_PRID.DataBind();

            DropDownList_MZ_PRID.SelectedValue = Session["ADPMZ_EXAD"].ToString();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (TableName == "PRK2")//檢查2-3是否會到這
            {
                Session["PRK2_PRID"] = DropDownList_MZ_PRID.SelectedItem.Text.Trim();

                Session["PRK2_ID"] = TextBox_MZ_ID.Text.Trim();

                Session["PRK2_NAME"] = TextBox_MZ_NAME.Text.Trim();

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "window.opener.location.href='Personal2-3.aspx?PRK2_PRID1=" + TextBox_MZ_PRID1.Text.Trim() + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "';window.close();", true);
           

                //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.opener.location.href='Personal2-3.aspx?PRK2_PRID1=" + TextBox_MZ_PRID1.Text.Trim() + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "';window.close();", true);
            }
            else if (TableName == "PRK1")//p2-2查詢
            {
                string PRID = HttpUtility.UrlEncode(DropDownList_MZ_PRID.SelectedItem.Text.Trim());

                //Session["PRK1_PRID"] = DropDownList_MZ_PRID.SelectedItem.Text.Trim();

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "window.opener.location.href='Personal2-2.aspx?PRK1_PRID=" + PRID + "&PRK1_PRID1=" + o_str.tosql(TextBox_MZ_PRID1.Text.Trim()) + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "';window.close();", true);
            
                //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.opener.location.href='Personal2-2.aspx?PRK1_PRID=" + PRID + "&PRK1_PRID1=" + o_str.tosql(TextBox_MZ_PRID1.Text.Trim()) + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "';window.close();", true);
            }
            else if (TableName == "POSIT1")
            {
                Session["POSIT1_PRID"] = DropDownList_MZ_PRID.SelectedItem.Text.Trim();

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "window.opener.location.href='Personal3-2.aspx?POSIT1_PRID1=" + o_str.tosql(TextBox_MZ_PRID1.Text.Trim()) + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "';window.close();", true);
            

                //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.opener.location.href='Personal3-2.aspx?POSIT1_PRID1=" + o_str.tosql(TextBox_MZ_PRID1.Text.Trim()) + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "';window.close();", true);
            }
            else if (TableName == "POSIT2")
            {
                Session["POSIT2_PRID"] = TextBox_MZ_PRID.Text.Trim();

                Session["POSIT2_ID"] = TextBox_MZ_ID.Text.Trim();

                Session["POSIT2_NAME"] = TextBox_MZ_NAME.Text.Trim();

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "window.opener.location.href='Personal3-3.aspx?POSIT2_PRID1=" + o_str.tosql(TextBox_MZ_PRID1.Text.Trim()) + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "';window.close();", true);
            

                //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.opener.location.href='Personal3-3.aspx?POSIT2_PRID1=" + o_str.tosql(TextBox_MZ_PRID1.Text.Trim()) + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "';window.close();", true);
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "window.close();", true);
     
            //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.close();", true);
        }

        protected void btPRID_Click(object sender, EventArgs e)
        {
            Session["KTYPE_CID"] = TextBox_MZ_PRID.ClientID;


            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "window.open('Personal_Ktype_Search.aspx?MZ_KTYPE=PRID&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin','top=190,left=200,width=500,height=450,toolbar=0,menubar=0,location=0,directories=0,status=0,scrollbar=yes');", true);
        
            //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_Ktype_Search.aspx?MZ_KTYPE=PRID&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin','top=190,left=200,width=500,height=450,toolbar=0,menubar=0,location=0,directories=0,status=0,scrollbar=yes');", true);
        }
    }
}
