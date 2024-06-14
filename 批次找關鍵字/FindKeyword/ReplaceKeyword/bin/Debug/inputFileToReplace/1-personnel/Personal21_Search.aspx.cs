using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TPPDDB._1_personnel
{
    public partial class Personal21_Search : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //by MQ 20100311---------
            A.set_Panel_EnterToTAB(ref this.Panel_UNIT_AD);
        }

        protected void btOk_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.opener.location.href='Personal21.aspx?MZ_UNIT=" + TextBox_MZ_UNIT.Text.Trim() + "&MZ_AD=" + DropDownList_AD.SelectedValue.Trim() + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "';window.close();", true);
        }

        protected void btLeave_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.close();", true);
        }
        protected void TextBox_MZ_UNIT_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_UNIT.Text, "25");
            if (string.IsNullOrEmpty(CName))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('查無資料')", true);
            }
            //Ktype_Cname_Check(CName, TextBox_MZ_UNIT1, TextBox_MZ_UNIT, TextBox_MZ_EXAD);
        }
        protected void btUNIT_Click(object sender, EventArgs e)
        {
            Ktype_Search(TextBox_MZ_UNIT, TextBox_MZ_UNIT1, "25");
        }
        protected void Ktype_Search(TextBox tb1, TextBox tb2, string ktype)
        {
            //回傳值
            Session["KTYPE_CID"] = tb1.ClientID;
            Session["KTYPE_CID1"] = tb2.ClientID;
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_Ktype_Search.aspx?MZ_KTYPE=" + ktype + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin','top=190,left=200,width=450,height=450,toolbar=0,menubar=0,location=0,directories=0,status=0,scrollbars=yes');", true);
        }

        private void Ktype_Cname_Check(string Cname, TextBox tb1, TextBox tb2, object obj)
        {
            tb2.Text = tb2.Text.ToUpper();
            //檢查代碼中文名稱是否存在
            if (string.IsNullOrEmpty(Cname))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('查無資料')", true);
                tb1.Text = string.Empty;
                tb2.Text = string.Empty;
                tb2.Focus();
            }
            else
            {
                tb1.Text = Cname;
                if (obj is TextBox)
                {
                    (obj as TextBox).Focus();
                }
                else if (obj is RadioButtonList)
                {
                    (obj as RadioButtonList).Focus();
                }
            }
        }
    }
}