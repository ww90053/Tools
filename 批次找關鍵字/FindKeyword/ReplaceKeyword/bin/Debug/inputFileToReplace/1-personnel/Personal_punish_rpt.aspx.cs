using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TPPDDB._1_personnel
{
    public partial class Personal_punish_rpt : System.Web.UI.Page
    {
        int TPM_FION=0;
        

        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                A.check_power(); 
   
                A.set_Panel_EnterToTAB(ref this.Panel1);     

                DropDownList_MZ_PRID.DataBind(); 
                ///群組權限
            
                ViewState["A_strGID"] = o_a_Function.strGID(Session["ADPMZ_ID"].ToString());
                A.fill_MZ_PRID(DropDownList_MZ_PRID,1);
                chk_TPMGroup();
            }
        }

        protected void chk_TPMGroup()
        {
            switch (ViewState["A_strGID"].ToString())
            {
                case "A":
                case "B":

                    break;
                case "C":
                    string strSQL = "SELECT * FROM Z_CODE_ON WHERE NEW='" + Session["ADPMZ_EXAD"].ToString() + "'";
                    DataTable tempDT = new DataTable();
                    tempDT = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");

                    strSQL  = "SELECT MZ_PRID,MZ_AD FROM A_CHKAD WHERE (MZ_AD='" + tempDT.Rows[0][0].ToString() + "' OR MZ_AD='" + tempDT.Rows[0][1].ToString() + "')";
                    DataTable source = new DataTable();
                    source = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
                    DropDownList_MZ_PRID.DataTextField="MZ_PRID";
                    DropDownList_MZ_PRID.DataValueField = "MZ_AD";
                    DropDownList_MZ_PRID.DataBind();
                    break;
                case "D":                  
                case "E":
                    //無權限
                    TPMPermissions._boolErrorData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "ADUXXX0001");
                    Response.Redirect("~/ErrorPage.aspx?erid=ADUXXX0001");
                    break;

            }

            DropDownList_MZ_PRID.SelectedValue = Session["ADPMZ_EXAD"].ToString();
        }




        /// <summary>
        /// 按鈕: 列印獎懲令
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btPrint_Click(object sender, EventArgs e)
        {
            string tmp_url = "A_rpt.aspx?fn=punish&TPM_FION=" + TPM_FION + "&PRID=" + HttpUtility.UrlEncode(DropDownList_MZ_PRID.SelectedItem.Text.Trim()) + "&PRID1=" + TextBox_MZ_PRID1.Text.Trim() + "&MZ_SRANK=" + ListBox_MZ_SRANK.SelectedValue.Trim() +"&DATAFROM=";

            ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);

        }
        /// <summary>
        /// 按鈕: 列印獎懲令（稿）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btPrint1_Click(object sender, EventArgs e)
        {
            string tmp_url = "A_rpt.aspx?fn=punish1&TPM_FION=" + TPM_FION + "&PRID=" + HttpUtility.UrlEncode(DropDownList_MZ_PRID.SelectedItem.Text.Trim()) + "&PRID1=" + TextBox_MZ_PRID1.Text.Trim() + "&MZ_SRANK=" + ListBox_MZ_SRANK.SelectedValue.Trim(); 

            ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);

        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            TextBox_MZ_PRID1.Text = string.Empty;
        }

        /// <summary>
        /// 按鈕: 印獎懲令(ODF)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_PrintODF_Click(object sender, EventArgs e)
        {
            string tmp_url = "A_rpt.aspx?fn=punishODF&TPM_FION=" + TPM_FION + "&PRID=" + HttpUtility.UrlEncode(DropDownList_MZ_PRID.SelectedItem.Text.Trim()) + "&PRID1=" + TextBox_MZ_PRID1.Text.Trim() + "&MZ_SRANK=" + ListBox_MZ_SRANK.SelectedValue.Trim() + "&DATAFROM=";

            ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
        }

        /// <summary>
        /// 按鈕: 印獎懲令（稿）(ODF) 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Print1ODF_Click(object sender, EventArgs e)
        {
            string tmp_url = "A_rpt.aspx?fn=punish1ODF&TPM_FION=" + TPM_FION + "&PRID=" + HttpUtility.UrlEncode(DropDownList_MZ_PRID.SelectedItem.Text.Trim()) + "&PRID1=" + TextBox_MZ_PRID1.Text.Trim() + "&MZ_SRANK=" + ListBox_MZ_SRANK.SelectedValue.Trim();

            ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
        }
    }
}
