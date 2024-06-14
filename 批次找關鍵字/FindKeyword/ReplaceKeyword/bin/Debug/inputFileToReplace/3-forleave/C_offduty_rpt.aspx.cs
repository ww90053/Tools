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
    public partial class C_offduty_rpt : System.Web.UI.Page
    {
        
        int TPM_FION=0;

        protected void Page_Load(object sender, EventArgs e)
        {
            
            
            if (!IsPostBack)
            {
                C.check_power(); 

            //MQ-----------------------20100331
            C.set_Panel_EnterToTAB(ref this.Panel1);
            
                TextBox_MZ_ID.Text = Session["ADPMZ_ID"].ToString();
            }
        }

        protected void btPrint_Click(object sender, EventArgs e)
        {

            if (DateManange.Check_date(TextBox_MZ_DATE.Text))
            {



                Session["RPT_C_IDNO"] = o_str.tosql(TextBox_MZ_ID.Text);

                string tmp_url = "C_rpt.aspx?fn=offduty&DATE=" + TextBox_MZ_DATE.Text +
                    "&TIME=&MZ_NAME=" + HttpUtility.UrlEncode(o_str.tosql(TextBox_MZ_NAME.Text).Trim()) +
                    "&TPM_FION=" + TPM_FION;

                ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "alert('請檢查欲查詢之日期');", true);
            }
            
        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            TextBox_MZ_DATE.Text = string.Empty;
            TextBox_MZ_ID.Text = string.Empty;
            TextBox_MZ_NAME.Text = string.Empty;
        }

        protected void TextBox_MZ_DATE_TextChanged(object sender, EventArgs e)
        {
            returnSameDataType(TextBox_MZ_DATE, TextBox_MZ_DATE);
        }

        protected void returnSameDataType(TextBox tb, object obj1)
        {
            tb.Text = o_str.tosql(tb.Text.Trim().Replace("/", ""));

            if (tb.Text != "")
            {
                if (!DateManange.Check_date(tb.Text))
                {
                    ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "alert('日期格式錯誤')", true);
                    tb.Focus();
                }
                else
                {
                    tb.Text = o_CommonService.Personal_ReturnDateString(tb.Text.Trim());

                    if (obj1 is TextBox)
                    {
                        TextBox tb1 = obj1 as TextBox;
                        tb1.Focus();
                    }
                    else if (obj1 is DropDownList)
                    {
                        DropDownList dl1 = obj1 as DropDownList;
                        dl1.Focus();
                    }
                }
            }
        }
    }
}
