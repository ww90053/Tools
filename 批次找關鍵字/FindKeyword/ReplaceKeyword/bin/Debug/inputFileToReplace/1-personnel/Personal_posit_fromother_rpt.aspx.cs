using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TPPDDB._1_personnel
{
    public partial class Personal_posit_fromother_rpt : System.Web.UI.Page
    {
        int TPM_FION=0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                A.check_power();
            }
        }

        protected void btPrint_Click(object sender, EventArgs e)
        {
            DataTable temp = new DataTable();

            string strSQL = "SELECT A_POSIT2.*,'' AS MZ_BCHKAD_NAME FROM A_POSIT2 WHERE MZ_PRID='" + DropDownList_MZ_PRID.SelectedItem.Text + "' AND MZ_PRID1='" + TextBox_MZ_PRID1.Text + "'";

            temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GETVALUE");

            if (temp.Rows.Count == 0)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('無符合資料！')", true);
                return;
            }

            for (int i = 0; i < temp.Rows.Count; i++)
            {
                temp.Rows[i]["MZ_DATE"] = temp.Rows[i]["MZ_DATE"].ToString().Substring(0, 3) + "年" + temp.Rows[i]["MZ_DATE"].ToString().Substring(3, 2) + "月" + temp.Rows[i]["MZ_DATE"].ToString().Substring(5, 2) + "日";
                temp.Rows[i]["MZ_EXOAD"] = o_A_KTYPE.RAD(temp.Rows[i]["MZ_EXOAD"].ToString());
                temp.Rows[i]["MZ_AD"] = o_A_KTYPE.RAD(temp.Rows[i]["MZ_AD"].ToString());
                temp.Rows[i]["MZ_EXOOCC"] = o_A_KTYPE.CODE_TO_NAME(temp.Rows[i]["MZ_EXOOCC"].ToString(), "26");
                temp.Rows[i]["MZ_OCCC"] = o_A_KTYPE.CODE_TO_NAME(temp.Rows[i]["MZ_OCCC"].ToString(), "26");
                temp.Rows[i]["MZ_EXOUNIT"] = o_A_KTYPE.RUNIT(temp.Rows[i]["MZ_EXOUNIT"].ToString());
                temp.Rows[i]["MZ_BCHKAD_NAME"] = o_A_KTYPE.CODE_TO_NAME(temp.Rows[i]["MZ_BCHKAD"].ToString(), "04");
                temp.Rows[i]["MZ_BCHKAD"] = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_PRID FROM A_CHKAD WHERE MZ_AD='" + temp.Rows[i]["MZ_BCHKAD"].ToString() + "'");

            }

            Session["rpt_dt"] = temp;

            Session["TITLE"] = string.Format("{0}轉發文件通知書", o_A_KTYPE.CODE_TO_NAME(DropDownList_MZ_PRID.SelectedValue, "04"));

            Session["EXPLAN1"] = TextBox1.Text.Trim();

            string tmp_url = "A_rpt.aspx?fn=posit_fromother&TPM_FION=" + TPM_FION;

            ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            TextBox_MZ_PRID1.Text = string.Empty;
            TextBox1.Text = string.Empty;
        }

        protected void btPrint1_Click(object sender, EventArgs e)
        {
            DataTable temp = new DataTable();

            string strSQL = "SELECT A_POSIT2.*,'' AS MZ_BCHKAD_NAME FROM A_POSIT2 WHERE MZ_PRID='" + DropDownList_MZ_PRID.SelectedItem.Text + "' AND MZ_PRID1='" + TextBox_MZ_PRID1.Text + "'";

            temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GETVALUE");

            if (temp.Rows.Count == 0)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('無符合資料！')", true);
                return;
            }

            for (int i = 0; i < temp.Rows.Count; i++)
            {
                temp.Rows[i]["MZ_DATE"] = temp.Rows[i]["MZ_DATE"].ToString().Substring(0, 3) + "年" + temp.Rows[i]["MZ_DATE"].ToString().Substring(3, 2) + "月" + temp.Rows[i]["MZ_DATE"].ToString().Substring(5, 2) + "日";
                temp.Rows[i]["MZ_EXOAD"] = o_A_KTYPE.RAD(temp.Rows[i]["MZ_EXOAD"].ToString());
                temp.Rows[i]["MZ_AD"] = o_A_KTYPE.RAD(temp.Rows[i]["MZ_AD"].ToString());
                temp.Rows[i]["MZ_EXOOCC"] = o_A_KTYPE.CODE_TO_NAME(temp.Rows[i]["MZ_EXOOCC"].ToString(), "26");
                temp.Rows[i]["MZ_OCCC"] = o_A_KTYPE.CODE_TO_NAME(temp.Rows[i]["MZ_OCCC"].ToString(), "26");
                temp.Rows[i]["MZ_EXOUNIT"] = o_A_KTYPE.RUNIT(temp.Rows[i]["MZ_EXOUNIT"].ToString());
                temp.Rows[i]["MZ_BCHKAD_NAME"] = o_A_KTYPE.CODE_TO_NAME(temp.Rows[i]["MZ_BCHKAD"].ToString(), "04");
                temp.Rows[i]["MZ_BCHKAD"] = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_PRID FROM A_CHKAD WHERE MZ_AD='" + temp.Rows[i]["MZ_BCHKAD"].ToString() + "'");
            }

            Session["rpt_dt"] = temp;

            Session["TITLE"] = string.Format("{0}轉發文件通知書（稿）", o_A_KTYPE.CODE_TO_NAME(DropDownList_MZ_PRID.SelectedValue,"04"));

            Session["EXPLAN1"] = TextBox1.Text.Trim();

            string tmp_url = "A_rpt.aspx?fn=posit_fromother1&TPM_FION=" + TPM_FION;

            ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
        }
    }
}
