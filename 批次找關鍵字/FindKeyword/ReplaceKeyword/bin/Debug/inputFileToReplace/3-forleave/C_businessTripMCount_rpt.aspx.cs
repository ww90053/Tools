using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TPPDDB._3_forleave
{
    public partial class C_businessTripMCount_rpt : System.Web.UI.Page
    {
        int TPM_FION=0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                C.check_power();           
                //by MQ ------------------------------20100331            
                C.set_Panel_EnterToTAB(ref this.Panel1);
            }
            
        }

        static DataTable dt(string MZ_DATE, string MZ_ID, string MZ_NAME)
        {
            string strSQL = "SELECT MZ_ID,MZ_NAME,MZ_DLTB01_SN," +
                                      " (SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=MZ_EXAD AND MZ_KTYPE='04') MZ_EXAD," +
                                      " (SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=MZ_EXUNIT AND MZ_KTYPE='25') MZ_EXUNIT," +
                                      " MZ_OCCC ," +
                                      " (SELECT MZ_CNAME FROM C_DLCODE WHERE C_DLCODE.MZ_CODE=C_DLTB01.MZ_CODE) MZ_CODE," +
                                      " (SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=MZ_RANK1 AND MZ_KTYPE='09') MZ_RANK1," +
                                      " MZ_SYSDAY,MZ_IDATE1,MZ_ITIME1,MZ_ODATE,MZ_OTIME,MZ_TDAY,MZ_TTIME,MZ_CAUSE,MZ_MEMO,MZ_SWT,MZ_TADD,MZ_RNAME," +
                                      " (SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=MZ_ROCCC AND MZ_KTYPE='26') ROCCC," +
                                      " MZ_ROCCC,MZ_CHK1,MZ_SYSTIME,MZ_FOREIGN,MZ_CHINA  " +
                                      " FROM C_DLTB01 " +
                                      " WHERE (MZ_CODE='07' OR MZ_CODE='06') AND (MZ_TDAY>0 OR MZ_TTIME>0) ";

            List<string> condition = new List<string>();
            if (MZ_DATE != string.Empty)
            {
                condition.Add("MZ_IDATE1 = '" + MZ_DATE.Replace("/", string.Empty).PadLeft(7, '0') + "'");
            }

            if (MZ_ID != string.Empty)
            {
                condition.Add("MZ_ID='" + MZ_ID + "'");
            }

            if (MZ_NAME != string.Empty)
            {
                condition.Add("MZ_NAME='" + MZ_NAME + "'");
            }

            string where = (condition.Count > 0 ? " AND " + string.Join(" AND ", condition.ToArray()) : string.Empty);

            strSQL += where;
            DataTable businessTrip = new DataTable();

            businessTrip = o_DBFactory.ABC_toTest.Create_Table(strSQL, "business");

            return businessTrip;
        }

        protected void btPrint_Click(object sender, EventArgs e)
        {
            DataTable businessTrip = new DataTable();

            string strSQL = @"SELECT C_DLTB01.MZ_EXAD,C_DLTB01.MZ_EXUNIT,C_DLTB01.MZ_ID,SUM(C_BUSSINESSTRIP.TOTAL) AS TOTALPAY, 
                                     (SELECT MZ_NAME FROM A_DLBASE WHERE A_DLBASE.MZ_ID=C_DLTB01.MZ_ID) AS MZ_NAME,
                                     (SELECT MZ_POLNO FROM A_DLBASE WHERE A_DLBASE.MZ_ID=C_DLTB01.MZ_ID) AS MZ_POLNO 
                                FROM C_DLTB01,C_BUSSINESSTRIP 
                               WHERE C_BUSSINESSTRIP.MZ_DLTB01_SN=C_DLTB01.MZ_DLTB01_SN 
                                     AND dbo.SUBSTR(C_DLTB01.MZ_IDATE1,1,5)='{0}'
                                     AND MZ_EXAD='{1}'
                                     AND MZ_EXUNIT='{2}' 
                            GROUP BY C_DLTB01.MZ_EXAD,C_DLTB01.MZ_EXUNIT,C_DLTB01.MZ_ID
                            ORDER BY (SELECT dbo.to_number(CASE WHEN MZ_TBDV IS NULL THEN '999' WHEN MZ_TBDV='Z99' THEN '999' ELSE MZ_TBDV END) AS TBDV FROM A_DLBASE WHERE A_DLBASE.MZ_ID=C_DLTB01.MZ_ID),
                                     (SELECT MZ_OCCC FROM A_DLBASE WHERE A_DLBASE.MZ_ID=C_DLTB01.MZ_ID)";

            businessTrip = o_DBFactory.ABC_toTest.Create_Table(string.Format(strSQL, TextBox_MZ_DATE1.Text, DropDownList_AD.SelectedValue, DropDownList_UNIT.SelectedValue), "123");



            if (businessTrip.Rows.Count > 0)
            {
                Session["TITLE"] = DropDownList_AD.SelectedItem.Text;

                Session["TITLE2"] = TextBox_MZ_DATE1.Text.Substring(0, 3) + "年" + TextBox_MZ_DATE1.Text.Substring(3, 2) + "月";

                Session["rpt_dt"] = businessTrip;

                string tmp_url = "C_rpt.aspx?fn=businessTripMCount&TPM_FION=" + TPM_FION;

                ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "alert('無此日期之相關資料');", true);
            }

        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("C_businessTripMCount_rpt.aspx?TPM_FION=0");
        }

        protected void DropDownList_UNIT_DataBound(object sender, EventArgs e)
        {
            DropDownList_UNIT.Items.Insert(0, new ListItem(" ", ""));
        }

    }
}
