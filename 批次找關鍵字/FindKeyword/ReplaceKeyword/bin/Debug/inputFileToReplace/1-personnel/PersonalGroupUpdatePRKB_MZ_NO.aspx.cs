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
    public partial class PersonalGroupUpdatePRKB_MZ_NO : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                A.set_Panel_EnterToTAB(ref this.Panel1);
                if (Request["MZ_NO"] != null)
                {
                    TextBox_OLD_MZ_NO.Text = Request["MZ_NO"].ToString().Trim();
                }
            }
        }

        //protected void TextBox_OLD_MZ_NO_TextChanged(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrEmpty(o_DBFactory.ABC_toTest.vExecSQL("SELECT DISTINCT MZ_NO FROM A_PRKB WHERE MZ_NO='" + TextBox_OLD_MZ_NO.Text.Trim() + "'")))
        //    {
        //        //TextBox_OLD_MZ_NO.Focus();
        //        //20141210
        //        //ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_OLD_MZ_NO.ClientID + "').focus();$get('" + TextBox_OLD_MZ_NO.ClientID + "').focus();", true);
        //        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('無此案號');", true);
        //        return;
        //    }
        //}

        protected void Button1_Click(object sender, EventArgs e)
        {
            string strSQL2 = "";
            //權限
            switch (o_a_Function.strGID(Session["ADPMZ_ID"].ToString()))
            {
                case "A":
                case "B":
                    strSQL2 = "";
                    break;
                case "C":
                    strSQL2 = " AND MZ_AD='" + Session["ADPMZ_EXAD"].ToString() + "' AND MZ_SWT3='2' ";
                    break;
                case "D":
                case "E":
                    strSQL2 = " AND MUSER='" + Session["ADPMZ_ID"].ToString() + "'";
                    break;
            }

            string selectSQL = "SELECT COUNT(*) FROM A_PRKB WHERE MZ_NO='" + TextBox_OLD_MZ_NO.Text.Trim() + "'";

            string checkDT = o_DBFactory.ABC_toTest.vExecSQL(selectSQL);

            if (checkDT == "0")
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('無此案號或無權限！');", true);
            }
            else
            {
                //驗證轉換後是否有重複資料 20180522 by sky
                string checkSQL = string.Format(@"SELECT MZ_ID, MZ_PRCT, MZ_PRK FROM A_PRKB WHERE MZ_NO in ('{0}', '{1}')
                                                GROUP BY MZ_ID, MZ_PRCT, MZ_PRK
                                                HAVING count(*)>1", TextBox_OLD_MZ_NO.Text.Trim(), TextBox_NEW_MZ_NO.Text.Trim());
                DataTable dt = new DataTable();
                dt = o_DBFactory.ABC_toTest.Create_Table(checkSQL, "GET");
                if (dt.Rows.Count != 0)
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('有重複資料！無法進行修改。');", true);
                    return;
                }

                string UpdateString = "UPDATE A_PRKB SET MZ_NO='" + TextBox_NEW_MZ_NO.Text.Trim() + "' WHERE MZ_NO='" + TextBox_OLD_MZ_NO.Text.Trim() + "' AND (MZ_SWT1 !='Y' )" + strSQL2;

                try
                {
                    o_DBFactory.ABC_toTest.vExecSQL(UpdateString);
                    
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('修改成功!');window.opener.location.href='Personal2-1.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "';window.close();", true);
                }
                catch
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('修改失敗!有重複資料！');", true);

                    if (TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "P-GUPDATENO", TPMPermissions._boolTPM(), UpdateString) == "N")
                    {
                        TPMPermissions._boolErrorData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "P-GUPDATENO", UpdateString);
                    }
                }
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.close();", true);
        }

        protected void TextBox_NEW_MZ_NO_TextChanged(object sender, EventArgs e)
        {
            //if (!string.IsNullOrEmpty(o_DBFactory.ABC_toTest.vExecSQL("SELECT DISTINCT MZ_NO FROM A_PRKB WHERE MZ_NO='" + TextBox_NEW_MZ_NO.Text.Trim() + "'")))
            //{
            //    //TextBox_NEW_MZ_NO.Focus();
            //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_NEW_MZ_NO.ClientID + "').focus();$get('" + TextBox_NEW_MZ_NO.ClientID + "').focus();", true);
            //    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('已有此案號,請重新輸入');", true);
            //    return;
            //}
        }
    }
}
