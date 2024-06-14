using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TPPDDB._1_personnel
{
    public partial class Personal_quit_rpt : System.Web.UI.Page
    {
        int TPM_FION=0;
       

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
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
            string strSQL = "SELECT  A_POSIT2.*,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=MZ_AD AND MZ_KTYPE='04') AS MZ_AD1,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=MZ_TBNREA AND MZ_KTYPE='11') AS MZ_TBNREA1 FROM A_POSIT2 WHERE MZ_PRID='" + DropDownList_MZ_PRID.SelectedItem.Text + "'";

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
                tempDT.Rows[i]["MZ_UNIT"] = o_A_DLBASE.CUNIT(tempDT.Rows[i]["MZ_ID"].ToString());
                tempDT.Rows[i]["MZ_OCCC"] = o_A_DLBASE.OCCC(tempDT.Rows[i]["MZ_ID"].ToString());
                tempDT.Rows[i]["MZ_EXOOCC"] = o_A_DLBASE.OCCC(tempDT.Rows[i]["MZ_ID"].ToString());
                tempDT.Rows[i]["MZ_EXOAD"] = o_A_KTYPE.RAD(tempDT.Rows[i]["MZ_EXOAD"].ToString());
                tempDT.Rows[i]["MZ_AD"] = o_A_KTYPE.RAD(tempDT.Rows[i]["MZ_AD"].ToString());
                tempDT.Rows[i]["MZ_EXOUNIT"] = o_A_KTYPE.RAD(tempDT.Rows[i]["MZ_EXOUNIT"].ToString());

            }

            Session["rpt_dt"] = tempDT;

            Session["TITLE"] = string.Format("{0}離職通知書", o_A_KTYPE.RAD(Session["ADPMZ_EXAD"].ToString()));

            string printkind;

            if (RadioButtonList1.SelectedValue == "1")
                printkind = "quit";
            else
            {
                printkind = "quit1";
                Session["TITLE1"] = "局長";
                Session["TITLE2"] = "副局長";
            }
            string tmp_url = "A_rpt.aspx?fn=" + printkind + "&TPM_FION=" + TPM_FION;
            ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            TextBox_MZ_PRID1.Text = string.Empty;
            TextBox_MZ_ID.Text = string.Empty;
        }
    }
}
