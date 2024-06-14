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
using System.Text;

namespace TPPDDB._2_salary
{
    public class SalaryTaxes
    {
        

        protected static string two00(string COL, string IDCARD)
        {
            string strSQL = string.Format("SELECT {0} FROM B_TAXES BT,B_BRANCH BB WHERE IDCARD='{1}' AND \"ID\"=PAY_AD", COL, IDCARD);
            DataTable temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
            if (temp.Rows.Count > 0)
            {
                if (COL == "BT.TICKETNUM")
                { COL = "TICKETNUM"; }
                return temp.Rows[0][COL].ToString();
            }
            else
            {
                return "";
            }
            //TAXUNIT,TAXINVOICE,BT.TICKETNUM,TAX_TYPE,MONTHPAY,\"REPAIR\",SOLE,YEARPAY,EFFECT,MONTHPAYTAX,REPAIRTAX,SOLETAX,YEARPAYTAX,EFFECTTAX,LABORPAY
        }

        
    }
}
