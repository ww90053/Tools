using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TPPDDB.Helpers;

namespace TPPDDB._3_forleave
{
    public static class Service_C_DUTYPEOPLE
    {

        public static string get_pno(string ad, string unit, string idno, string dutydate)
        {
            string cno = string.Empty;
            string strSQL = string.Format("SELECT MZ_PNO FROM C_DUTYPEOPLE WHERE MZ_ID='{0}' AND MZ_AD='{1}' AND MZ_UNIT='{2}' AND DUTYDATE='{3}'", idno, ad, unit, dutydate);
            DataTable temp = new DataTable();
            temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
            if (temp.Rows.Count > 0)
            {
                return temp.Rows[0][0].ToString();
            }
            else
            {
                return "";
            }
        }

        public static string get_pno_ym(string ad, string unit, string idno, string year, string month)
        {
            string cno = string.Empty;
            string strSQL = string.Format("SELECT DISTINCT MZ_PNO FROM C_DUTYPEOPLE WHERE MZ_ID='{0}' AND MZ_AD='{1}' AND MZ_UNIT='{2}' AND DUTYDATE LIKE '{3}{4}%'", idno, ad, unit, year.PadLeft(3, '0'), month.PadLeft(2, '0'));
            DataTable temp = new DataTable();
            temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
            if (temp.Rows.Count > 0)
            {
                return temp.Rows[0][0].ToString();
            }
            else
            {
                return "";
            }

        }
    }

}