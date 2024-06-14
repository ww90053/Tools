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
    public partial class C_busitrip50_rpt : System.Web.UI.Page
    {
        int TPM_FION=0;
        

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                C.check_power();
           

                //by MQ ------------------------------20100331            
                C.set_Panel_EnterToTAB(ref this.Panel1);
                //C.fill_AD(DropDownList_EXAD);
                //如果是中和分局進來改成只抓中和一&中和二&中和 matthew
                if (Session["ADPMZ_EXAD"].ToString() == "382133600C")
                {
                    C.fill_DLL_ONE_TWO(DropDownList_EXAD);
                }
                else
                {
                    //把所有機關撈出來包含台北縣
                    C.fill_AD_POST(DropDownList_EXAD);
                }
                chk_TPMGroup();
            }
        }

        /// <summary>
        /// 群組權限
        /// </summary>
        protected void chk_TPMGroup()
        {
            switch (o_a_Function.strGID(Session["ADPMZ_ID"].ToString()))
            {
                case "A":
                case "B":
                    if (Session["ADPMZ_EXAD"].ToString() != "382133600C")
                    {
                        DropDownList_EXAD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                    }
                    break;
                case "C":
                    //如果是中和分局進來要可以多選中和一&中和二 matthew
                    if (Session["ADPMZ_EXAD"].ToString() != "382133600C")
                    {
                        DropDownList_EXAD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                        DropDownList_EXAD.Enabled = false;
                    }
                    break;
                case "D":
                case "E":
                    TPMPermissions._boolErrorData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "ADUXXX0001");
                    Response.Redirect("~/ErrorPage.aspx?erid=ADUXXX0001");
                    break;
            }
        }

        

        protected void btCancel_Click(object sender, EventArgs e)
        {
            TextBox_MZ_DATE1.Text = string.Empty;
            TextBox_MZ_DATE2.Text = string.Empty;
        }

        protected void btPrint_Click(object sender, EventArgs e)
        {
            if (!(DateManange.Check_date(TextBox_MZ_DATE1.Text) && DateManange.Check_date(TextBox_MZ_DATE2.Text)))
            {
                ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "alert('請檢查輸入的起迄日期');", true);
                return;
            }



            string tmp_url = "C_rpt.aspx?fn=busitrip50&EXAD=" + DropDownList_EXAD.SelectedValue + "&DATE1=" + TextBox_MZ_DATE1.Text + "&DATE2=" + TextBox_MZ_DATE2.Text + "&TPM_FION=" + TPM_FION;

            ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);

        }
    }
}
