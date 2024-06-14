using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using TPPDDB.App_Code; 

namespace TPPDDB._3_forleave
{
    public partial class C_chinaabroad_rpt : System.Web.UI.Page
    {
        int TPM_FION=0;
        

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                C.check_power();
             ///群組權限
                ViewState["C_strGID"] = o_a_Function.strGID(Session["ADPMZ_ID"].ToString());
                //C_strGID = o_a_Function.strGID(Session["ADPMZ_ID"].ToString());
            //by MQ ------------------------------20100331
            C.set_Panel_EnterToTAB(ref this.Panel1);
           
                TextBox_MZ_ID.Text = Session["ADPMZ_ID"].ToString();  //身分證號的default;

                chk_TPMGroup();
            }
        }

        /// <summary>
        /// 群組權限
        /// </summary>
        protected void chk_TPMGroup()
        {
            switch (ViewState["C_strGID"].ToString())
            {
                case "A":
                case "B":
                case "C": 
                case "E":
                    break;
                case "D":
                    TextBox_MZ_ID.ReadOnly = true;
                    TextBox_MZ_NAME.ReadOnly = true;
                    break;
               

            }
        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            TextBox_MZ_DATE.Text = string.Empty;
            TextBox_MZ_ID.Text = string.Empty;
            TextBox_MZ_NAME.Text = string.Empty;
        }

        protected void btPrint_Click(object sender, EventArgs e)
        {
           
            if (TextBox_MZ_DATE.Text.Trim() != string.Empty)
            {
                if (DateManange.Check_date(TextBox_MZ_DATE.Text))
                {
                    
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "alert('請檢查欲查詢之日期');", true);
                    return;
                }
            }

            Session["RPT_C_IDNO"] = TextBox_MZ_ID.Text.Trim();

            string tmp_url = "C_rpt.aspx?fn=chinaabroad" +
            "&DATE=" + TextBox_MZ_DATE.Text.Trim().Replace("/", string.Empty).PadLeft(7, '0') +
            "&MZ_NAME=" + HttpUtility.UrlEncode(TextBox_MZ_NAME.Text.Trim()) +
            "&TPM_FION=" + TPM_FION; ;

                ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
            

        }

        protected void btPrint1_Click(object sender, EventArgs e)        {
            
            string tmp_url = "C_rpt.aspx?fn=chinaabroad1&TPM_FION=" + TPM_FION;
            ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
        }
    }
}
