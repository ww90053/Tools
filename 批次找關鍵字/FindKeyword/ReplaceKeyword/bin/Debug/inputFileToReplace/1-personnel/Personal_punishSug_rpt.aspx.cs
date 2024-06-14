using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TPPDDB._1_personnel
{
    public partial class Personal_punishSug_rpt : System.Web.UI.Page
    {
        int TPM_FION=0;
        
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                A.check_power();    
                A.set_Panel_EnterToTAB(ref this.Panel1);           
               
       
                A.fill_MZ_PRID(DropDownList_MZ_PRID,2);
                DropDownList_MZ_PRID.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                ViewState["A_strGID"] = o_a_Function.strGID(Session["ADPMZ_ID"].ToString());

                chk_TPMGroup();
            }

        }

        /// <summary>
        /// 群組權限
        /// </summary>
        protected void chk_TPMGroup()
        {
            switch (ViewState["A_strGID"].ToString() )
            {
                case "A":
                case "B":
                    break;
                case "C":
                    string strSQL = "SELECT * FROM Z_CODE_ON WHERE NEW='" + Session["ADPMZ_EXAD"].ToString() + "'";
                    DataTable tempDT = new DataTable();
                    tempDT = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
                    
                    strSQL = "SELECT MZ_PRID,MZ_AD FROM A_CHKAD WHERE (MZ_AD='" + tempDT.Rows[0][0].ToString() + "' OR MZ_AD='" + tempDT.Rows[0][1].ToString() + "')";
                    //matthew 中和要可以看到中一&中二
                    if (Session["ADPMZ_EXAD"].ToString() == "382133600C")
                    {
                        strSQL = "SELECT MZ_PRID,MZ_AD FROM A_CHKAD where MZ_AD in ('382133400C','382133500C','382133600C')";
                    }
                    DataTable source = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
                    DropDownList_MZ_PRID.DataSource = source;
                    DropDownList_MZ_PRID.DataTextField = "MZ_PRID";
                    DropDownList_MZ_PRID.DataValueField = "MZ_AD";
                    //SqlDataSource1.SelectCommand = "SELECT MZ_PRID,MZ_AD FROM A_CHKAD WHERE (MZ_AD='" + tempDT.Rows[0][0].ToString() + "' OR MZ_AD='" + tempDT.Rows[0][1].ToString() + "')";
                    DropDownList_MZ_PRID.DataBind();
                    break;
                case "D":
                case "E":
                    //無權限
                    TPMPermissions._boolErrorData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "ADUXXX0001");
                    Response.Redirect("~/ErrorPage.aspx?erid=ADUXXX0001");
                    break;

            }
        }


        protected void btPrint_Click(object sender, EventArgs e)
        {           
            string tmp_url = "A_rpt.aspx?fn=punishSug&TPM_FION=" + TPM_FION + "&PRID=" + HttpUtility.UrlEncode(DropDownList_MZ_PRID.SelectedItem.Text.Trim()) + "&PRID1=" + TextBox_MZ_PRID1.Text.Trim() + "&MZ_SRANK=" + ListBox_MZ_SRANK.SelectedValue.Trim(); 

            ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);

        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            TextBox_MZ_PRID1.Text = string.Empty;
        }
    }
}
