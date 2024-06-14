using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
namespace TPPDDB._1_personnel
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                A.check_power();
            }
        }
        protected void TextBox_MZ_AD_TextChanged(object sender, EventArgs e)
        {
            string CName = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)='38213' UNION ALL SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)<>'38213') WHERE MZ_KCODE='" + TextBox_MZ_AD.Text.Trim().ToUpper() + "'");
            Ktype_Cname_Check(CName, TextBox_MZ_AD1, TextBox_MZ_AD, TextBox_MZ_UNIT);
        }
        protected void TextBox_MZ_UNIT_TextChanged(object sender, EventArgs e)
        {
            string CName = o_A_KTYPE.Find_Ktype_Cname(TextBox_MZ_UNIT.Text, "25");
            Ktype_Cname_Check(CName, TextBox_MZ_UNIT1, TextBox_MZ_UNIT, DropDownList_Year);
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
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + tb2.ClientID + "').focus();$get('" + tb2.ClientID + "').focus();", true);
            }
            else
            {
                tb1.Text = Cname;
                if (obj is TextBox)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + (obj as TextBox).ClientID + "').focus();$get('" + (obj as TextBox).ClientID + "').focus();", true);
                }
                else if (obj is DropDownList)
                {
                    (obj as DropDownList).Focus();
                }

            }
        }
        protected void btAD_Click(object sender, EventArgs e)
        {
            Session["KTYPE_CID"] = TextBox_MZ_AD.ClientID;
            Session["KTYPE_CID1"] = TextBox_MZ_AD1.ClientID;

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_Ktype_Search.aspx?CID_NAME=AD&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin','top=190,left=200,width=420,height=450,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
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
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_Ktype_Search.aspx?MZ_KTYPE=" + ktype + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin','top=190,left=200,width=420,height=450,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }

        protected void btInsert_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
                {
                    conn.Open();

                    string strSQL = "INSERT INTO A_EFFICIENCY (MZ_ID,T01,T25,T26) (SELECT ARB.MZ_ID,ARB.MZ_YEAR,MZ_NUM,MZ_GRADE FROM A_REV_BASE ARB,((SELECT MZ_ID,MZ_YEAR FROM A_REV_BASE WHERE MZ_AD = '" + TextBox_MZ_AD.Text + "' AND MZ_UNIT = '" + TextBox_MZ_UNIT.Text + "' AND MZ_YEAR = '" + DropDownList_Year.SelectedValue + "')MINUS (SELECT MZ_ID,T01 FROM A_EFFICIENCY WHERE T01 = '" + DropDownList_Year.SelectedValue + "')) MARB WHERE ARB.MZ_ID = MARB.MZ_ID AND ARB.MZ_YEAR = MARB.MZ_YEAR)";

                    SqlCommand cmd = new SqlCommand(strSQL, conn);
                    DataTable temp = o_DBFactory.ABC_toTest.Create_Table("(SELECT ARB.MZ_ID,ARB.MZ_YEAR,MZ_NUM,MZ_GRADE FROM A_REV_BASE ARB,((SELECT MZ_ID,MZ_YEAR FROM A_REV_BASE WHERE MZ_AD = '" + TextBox_MZ_AD.Text + "' AND MZ_UNIT = '" + TextBox_MZ_UNIT.Text + "' AND MZ_YEAR = '" + DropDownList_Year.SelectedValue + "') INTERSECT (SELECT MZ_ID,T01 FROM A_EFFICIENCY WHERE T01 = '" + DropDownList_Year.SelectedValue + "')) MARB WHERE ARB.MZ_ID = MARB.MZ_ID AND ARB.MZ_YEAR = MARB.MZ_YEAR)", "INTERSECT");

                    cmd.ExecuteNonQuery();
                    foreach (DataRow dr in temp.Rows)
                    {
                        string strsql = "UPDATE A_EFFICIENCY SET  T25 = '" + dr["MZ_NUM"] + "' , T26 = '" + dr["MZ_GRADE"] + "' WHERE MZ_ID = '" + dr["MZ_ID"] + "' AND T01 = '" + dr["MZ_YEAR"] + "'";
                        o_DBFactory.ABC_toTest.vExecSQL(strsql);
                    }

                }
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('轉入完成！')", true);
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('轉入失敗，身分證和年度必須唯一！')", true);
            }
        }
    }
}
