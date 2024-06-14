using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TPPDDB._1_personnel
{
    public partial class A_Leavejob_rpt : System.Web.UI.Page
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
            string strSQL = "SELECT A_POSIT2.*,'' AS MZ_BIR,'' AS PAY1 FROM A_POSIT2 WHERE MZ_PRID='" + DropDownList_MZ_PRID.SelectedItem.Text + "' AND MZ_PRID1='" + TextBox_MZ_PRID1.Text.Trim() + "'";

            DataTable Leavejob = new DataTable();

            Leavejob = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GETVALUE");

            for (int i = 0; i < Leavejob.Rows.Count; i++)
            {
                string BIR = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_BIR FROM A_DLBASE WHERE MZ_ID='" + Leavejob.Rows[i]["MZ_ID"].ToString() + "'");
                string ADATE = Leavejob.Rows[i]["MZ_ADATE"].ToString().PadLeft(7,'0');
                Leavejob.Rows[i]["PAY1"] = o_DBFactory.ABC_toTest.vExecSQL("SELECT PAY1 FROM B_SALARY WHERE ORIGIN2=(SELECT MZ_SPT FROM A_DLBASE WHERE MZ_ID='" + Leavejob.Rows[i]["MZ_ID"].ToString() + "')");
                Leavejob.Rows[i]["MZ_BIR"] = string.IsNullOrEmpty(BIR) ? string.Empty : BIR.Substring(0, 3) + "." + BIR.Substring(3, 2) + "." + BIR.Substring(5, 2);
                Leavejob.Rows[i]["MZ_ADATE"] = string.IsNullOrEmpty(ADATE) ? string.Empty : ADATE.Substring(0, 3) + "." + ADATE.Substring(3, 2) + "." + ADATE.Substring(5, 2);
                Leavejob.Rows[i]["MZ_EXOAD"] = o_A_KTYPE.RAD(Leavejob.Rows[i]["MZ_EXOAD"].ToString());
                Leavejob.Rows[i]["MZ_AD"] = o_A_KTYPE.RAD(Leavejob.Rows[i]["MZ_AD"].ToString());
                Leavejob.Rows[i]["MZ_EXOOCC"] = o_A_KTYPE.CODE_TO_NAME(Leavejob.Rows[i]["MZ_EXOOCC"].ToString(),"26");
                Leavejob.Rows[i]["MZ_OCCC"] = o_A_DLBASE.OCCC(Leavejob.Rows[i]["MZ_ID"].ToString());
                Leavejob.Rows[i]["MZ_EXOOCC"] = Leavejob.Rows[i]["MZ_EXOOCC"].ToString();
                Leavejob.Rows[i]["MZ_OCCC"] = o_A_DLBASE.OCCC(Leavejob.Rows[i]["MZ_OCCC"].ToString());
            }

            Session["TITLE"] = string.Format("{0}員警離職傳知名冊",o_A_KTYPE.RAD(Session["ADPMZ_EXAD"].ToString()));

            string tmp_url = "A_rpt.aspx?fn=Leavejob&TPM_FION=" + TPM_FION;

            Session["rpt_dt"] = Leavejob;

            ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            TextBox_MZ_PRID1.Text = string.Empty;
        }
    }
}
