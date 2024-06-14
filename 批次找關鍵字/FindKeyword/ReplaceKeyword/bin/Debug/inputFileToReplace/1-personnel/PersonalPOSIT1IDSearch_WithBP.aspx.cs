using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TPPDDB._1_personnel
{
    public partial class PersonalPOSIT1IDSearch_WithBP : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
           
           
//20140113
            Session["POSIT1_GV1"] = @"SELECT MZ_NO,MZ_ID,MZ_NAME,AKD.MZ_KCHI MZ_AD ,  AKU.MZ_KCHI MZ_UNIT ,  AKO.MZ_KCHI MZ_OCCC , AKS.MZ_KCHI MZ_SRANK                   
                                    FROM A_POSIT2 
                                    LEFT JOIN A_KTYPE AKD ON RTRIM(AKD.MZ_KCODE)=RTRIM(A_POSIT2.MZ_AD) AND RTRIM(AKD.MZ_KTYPE)='04' 
                                    LEFT JOIN A_KTYPE AKU ON RTRIM(AKU.MZ_KCODE)=RTRIM(A_POSIT2.MZ_UNIT) AND RTRIM(AKU.MZ_KTYPE)='25' 
                                    LEFT JOIN A_KTYPE AKO ON RTRIM(AKO.MZ_KCODE)=RTRIM(A_POSIT2.MZ_OCCC) AND RTRIM(AKO.MZ_KTYPE)='26' 
                                    LEFT JOIN A_KTYPE AKS ON RTRIM(AKS.MZ_KCODE)=RTRIM(A_POSIT2.MZ_SRANK) AND RTRIM(AKS.MZ_KTYPE)='09' 
                                   WHERE MZ_PRID='北縣警人' AND MZ_PRID1='" + o_str.tosql(TextBox_MZ_PRID1.Text.Trim().ToUpper()) + "' AND MZ_AD='" + Session["ADPMZ_EXAD"].ToString()+ "'";
           
            Session["BPSQL"] = "MZ_PRID='北縣警人' AND MZ_PRID1='" + o_str.tosql(TextBox_MZ_PRID1.Text.Trim().ToUpper()) + "' AND MZ_AD='" + Session["ADPMZ_EXAD"].ToString() + "'";

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.opener.document.getElementById('" + Session["POSIT1_BT"] + "').click();window.close();", true);
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.close();", true);
        }
    }
}
