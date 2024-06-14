using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TPPDDB._1_personnel
{
    public partial class Personal_Support_rpt : System.Web.UI.Page
    {
        int TPM_FION=0;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                A.check_power();
                A.set_Panel_EnterToTAB(ref this.Panel1);
            A.set_Panel_EnterToTAB(ref this.Panel3);
            }
          

           
        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            TextBox_MZ_NO1.Text = string.Empty;
            TextBox_MZ_NO2.Text = string.Empty;
        }

        protected void btPrint_Click(object sender, EventArgs e)
        {
           
            string strSQL = @"SELECT  AKD.MZ_KCHI MZ_AD ,AKU.MZ_KCHI MZ_UNIT ,  MZ_OCCC AS OCCC,dbo.to_number(CASE WHEN MZ_TBDV IS NULL THEN '999' WHEN MZ_TBDV='Z99' THEN '999' ELSE MZ_TBDV END) AS TBDV,
                            AKO.MZ_KCHI MZ_OCCC,MZ_NAME,MZ_ID,MZ_PRCT,   AP.MZ_PRONAME MZ_PROLNO    ,AKP.MZ_KCHI MZ_PRRST,MZ_MEMO 
                            FROM A_PRKB 
                                LEFT JOIN A_KTYPE AKD ON RTRIM(AKD.MZ_KCODE)=RTRIM(A_PRKB.MZ_AD) AND RTRIM(AKD.MZ_KTYPE)='04' 
                                LEFT JOIN A_KTYPE AKU ON RTRIM(AKU.MZ_KCODE)=RTRIM(A_PRKB.MZ_UNIT) AND RTRIM(AKU.MZ_KTYPE)='25' 
                                LEFT JOIN A_KTYPE AKO ON RTRIM(AKO.MZ_KCODE)=RTRIM(A_PRKB.MZ_OCCC) AND RTRIM(AKO.MZ_KTYPE)='26' 
                                LEFT JOIN A_PROLNO AP ON RTRIM(AP.MZ_PROLNO)=RTRIM(A_PRKB.MZ_PROLNO)  
                                LEFT JOIN A_KTYPE AKP ON RTRIM(AKP.MZ_KCODE)=RTRIM(A_PRKB.MZ_PRRST) AND RTRIM(AKP.MZ_KTYPE)='24' 
                            WHERE " +
                                     "MZ_SWT3='" + DropDownList_MZ_SWT3.SelectedValue.Trim() + "'";

            if (TextBox_MZ_NO1.Text.Trim() != "" && TextBox_MZ_NO2.Text.Trim() != "")
            {
                strSQL += " AND MZ_NO>='" + TextBox_MZ_NO1.Text.Trim() + "' AND MZ_NO<='" + TextBox_MZ_NO2.Text.Trim() + "'";
            }
            else  if (TextBox_MZ_NO1.Text.Trim() != "" )
            {
                strSQL += " AND MZ_NO='" + TextBox_MZ_NO1.Text.Trim() + "'";
            }

            strSQL += " ORDER BY TBDV,OCCC,MZ_UNIT";
            //2013/12/02
            Session["RPT_SQL_A"] = strSQL;
            
            

            Session["TITLE"] = string.Format("{0}獎懲建議名冊", o_A_KTYPE.RAD(Session["ADPMZ_EXAD"].ToString()));


            string tmp_url = "A_rpt.aspx?fn=support&TPM_FION=" + TPM_FION;

            ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);

        }

    }
}
