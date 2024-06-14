using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TPPDDB._1_personnel
{
    public partial class Personal_Changejob_rpt : System.Web.UI.Page
    {
        int TPM_FION=0;
       

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                A.check_power();

            A.set_Panel_EnterToTAB(ref this.Panel1);
            
                DropDownList_MZ_PRID.DataBind();
                DropDownList_MZ_PRID.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                chk_TPMGroup();
            }
        }

        protected void chk_TPMGroup()
        {
            switch (o_a_Function.strGID(Session["ADPMZ_ID"].ToString()))
            {
                case "A":
                case "B":
                    break;
                case "C":
                    DropDownList_MZ_PRID.Enabled = false;
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
            string strSQL = "SELECT A_POSIT2.*,'' AS PAY1 FROM A_POSIT2 WHERE MZ_PRID='" + DropDownList_MZ_PRID.SelectedItem.Text + "'";

            if (!string.IsNullOrEmpty(TextBox_MZ_PRID1.Text.Trim()))
            {
                strSQL += " AND MZ_PRID1='" + TextBox_MZ_PRID1.Text.Trim() + "'";
            }

            if (!string.IsNullOrEmpty(TextBox_MZ_ID.Text.Trim()))
            {
                strSQL += " AND MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "'";
            }

            DataTable tempDT = new DataTable();

            tempDT = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GETVALUE");

            for (int i = 0; i < tempDT.Rows.Count; i++)
            {
                tempDT.Rows[i]["PAY1"] = o_DBFactory.ABC_toTest.vExecSQL("SELECT PAY1 FROM B_SALARY WHERE ORIGIN2=(SELECT MZ_SPT FROM A_DLBASE WHERE MZ_ID='" + tempDT.Rows[i]["MZ_ID"].ToString() + "')");
                tempDT.Rows[i]["MZ_UNIT"] = o_A_DLBASE.CUNIT(tempDT.Rows[i]["MZ_ID"].ToString());
                tempDT.Rows[i]["MZ_OCCC"] = o_A_DLBASE.OCCC(tempDT.Rows[i]["MZ_ID"].ToString());
                tempDT.Rows[i]["MZ_DATE"] = ChangeDate(tempDT.Rows[i]["MZ_DATE"].ToString());
                tempDT.Rows[i]["MZ_IDATE"] = ChangeDate(tempDT.Rows[i]["MZ_IDATE"].ToString());
                tempDT.Rows[i]["MZ_ADATE"] = ChangeDate(tempDT.Rows[i]["MZ_ADATE"].ToString());
                tempDT.Rows[i]["MZ_TBDATE"] = ChangeDate(tempDT.Rows[i]["MZ_TBDATE"].ToString());
                tempDT.Rows[i]["MZ_WDATE"] = ChangeDate(tempDT.Rows[i]["MZ_WDATE"].ToString());
                tempDT.Rows[i]["MZ_EXOUNIT"] = o_A_DLBASE.CUNIT(tempDT.Rows[i]["MZ_ID"].ToString());
                tempDT.Rows[i]["MZ_EXOOCC"] = o_A_DLBASE.OCCC(tempDT.Rows[i]["MZ_ID"].ToString());
                tempDT.Rows[i]["MZ_AD"] = o_A_DLBASE.CAD(tempDT.Rows[i]["MZ_ID"].ToString());
                tempDT.Rows[i]["MZ_EXOAD"] = o_A_DLBASE.CAD(tempDT.Rows[i]["MZ_ID"].ToString());
            }

            Session["rpt_dt"] = tempDT;

            string tmp_url = "A_rpt.aspx?fn=changejob&TPM_FION=" + TPM_FION;
            ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
        }

        private string ChangeDate(string date)
        {
            string result = "";
            if (!string.IsNullOrEmpty(date))
            {
                result = null;
            }
            else
            {
                if (date.Length == 7)
                {
                    result = (date.Substring(0, 1) == "0" ? date.Substring(1, 2) : date.Substring(0, 3)) + "年" + date.Substring(3, 2) + "月" + date.Substring(5, 2) + "日";
                }
                else
                {
                    result = null;
                }
            }
            return result;
        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            TextBox_MZ_PRID1.Text = string.Empty;
            TextBox_MZ_ID.Text = string.Empty;
        }
    }
}