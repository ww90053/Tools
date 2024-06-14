using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
namespace TPPDDB._1_personnel
{
    public partial class Personal_Basic3_rpt : System.Web.UI.Page
    {
        
        DataTable temp = new DataTable();
        
        int TPM_FION=0;
       

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                A.check_power();

                A.set_Panel_EnterToTAB(ref this.Panel1);


                A.fill_AD(DropDownList_AD);
                DropDownList_AD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                A.fill_unit(DropDownList_UNIT, DropDownList_AD.SelectedValue);

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
                    DropDownList_AD.Enabled = false;
                    break;
                case "D":
                    //無權限
                    TPMPermissions._boolErrorData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "ADUXXX0001");
                    Response.Redirect("~/ErrorPage.aspx?erid=ADUXXX0001");
                    break;
                case "E":
                    DropDownList_AD.Enabled = false;
                    try
                    {
                        DropDownList_UNIT.SelectedValue = Session["ADPMZ_EXUNIT"].ToString();
                    }
                    catch
                    {
                        DropDownList_UNIT.SelectedValue = "";
                    }
                    DropDownList_UNIT.Enabled = false;
                    break;
            }
        }

        protected void Button_MAKE_COMMUNICATIONS_Click(object sender, EventArgs e)
        {

            string strSQL = "SELECT (SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=MZ_EXUNIT AND MZ_KTYPE='25') AS MZ_UNIT,MZ_ID,MZ_NAME,((SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=MZ_OCCC AND MZ_KTYPE='26') + '&N' + (SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=MZ_EXTPOS AND MZ_KTYPE='@91')) AS MZ_OCCC ,MZ_BIR,MZ_ADD2,MZ_PHONE,MZ_MOVETEL,MZ_AD,MZ_EXAD,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=MZ_EXUNIT AND MZ_KTYPE='25')MZ_EXUNIT,MZ_OCCC AS OCCC,dbo.to_number(CASE WHEN MZ_TBDV IS NULL THEN '999' WHEN MZ_TBDV='Z99' THEN '999' ELSE MZ_TBDV END) AS TBDV,'' AS MZ_CAD FROM A_DLBASE WHERE MZ_STATUS2='Y' AND MZ_EXAD='" + DropDownList_AD.SelectedValue + "'";

            if (!string.IsNullOrEmpty(DropDownList_UNIT.SelectedValue))
            {
                strSQL += " AND MZ_EXUNIT='" + DropDownList_UNIT.SelectedValue + "'";
            }

            strSQL += "ORDER BY MZ_EXUNIT,TBDV,OCCC";

            DataTable rpt_dt = new DataTable();

            Session["RPT_SQL_A"] = strSQL;
            //有部分丟到r去處理pt
            //rpt_dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "getvalue");

            //foreach (DataRow dr in rpt_dt.Rows)
            //{
            //    dr["MZ_CAD"] = o_A_KTYPE.RAD(dr["MZ_AD"].ToString());
            //    dr["MZ_ADD2"] = o_CommonService.d_report_break_line(dr["MZ_ADD2"].ToString(), 28, "&N");
            //}

          

            Session["TITLE"] = string.Format("{0}{1}通訊名冊", DropDownList_AD.SelectedItem.Text, string.IsNullOrEmpty(DropDownList_UNIT.SelectedItem.Text) ? string.Empty : DropDownList_UNIT.SelectedItem.Text);

            string tmp_url = "A_rpt.aspx?fn=basic_communications&TPM_FION=" + TPM_FION;

            ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
        }

        protected void btCancel_Click(object sender, EventArgs e)
        {

        }

        protected void DropDownList_UNIT_DataBound(object sender, EventArgs e)
        {
            ListItem li = new ListItem(" ", "");
            DropDownList_UNIT.Items.Insert(0, li);
        }

        protected void DropDownList_AD_SelectedIndexChanged(object sender, EventArgs e)
        {
            A.fill_unit(DropDownList_UNIT, DropDownList_AD.SelectedValue); 

           
        }
    }
}
