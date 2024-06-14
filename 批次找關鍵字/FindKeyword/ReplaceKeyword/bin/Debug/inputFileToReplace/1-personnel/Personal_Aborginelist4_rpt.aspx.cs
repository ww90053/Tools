using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;


namespace TPPDDB._1_personnel
{
    public partial class Personal_Aborginelist4_rpt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                A.check_power();
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string strSQL_EXAD = "SELECT MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE='04' AND MZ_KCODE LIKE '38213%' ORDER BY MZ_KCODE ";

            DataTable EXAD_dt = new DataTable();

            EXAD_dt = o_DBFactory.ABC_toTest.Create_Table(strSQL_EXAD, "GET_EXAD");

            DataTable GCount_dt = new DataTable();
            //訂出DataTable欄位
            GCount_dt.Columns.Add("EXAD", typeof(string));
            GCount_dt.Columns.Add("G3c", typeof(int));
            GCount_dt.Columns.Add("B30c", typeof(int));
            GCount_dt.Columns.Add("G3B30", typeof(int));
            GCount_dt.Columns.Add("G21c", typeof(int));
            GCount_dt.Columns.Add("G22c", typeof(int));
            GCount_dt.Columns.Add("G23c", typeof(int));
            GCount_dt.Columns.Add("G24c", typeof(int));
            GCount_dt.Columns.Add("G25c", typeof(int));
            GCount_dt.Columns.Add("G2", typeof(int));
            GCount_dt.Columns.Add("G1c", typeof(int));
            GCount_dt.Columns.Add("G2c", typeof(int));
            GCount_dt.Columns.Add("G0c", typeof(int));
            GCount_dt.Columns.Add("G1", typeof(int));

            for (int i = 0; i < EXAD_dt.Rows.Count; i++)
            {
                //警監
                string G3 = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_DLBASE WHERE MZ_STATUS2='Y' AND MZ_ABORIGINE = 'Y' AND MZ_SRANK IN (SELECT MZ_KCODE FROM A_KTYPE WHERE MZ_KCHI LIKE '警監%' AND MZ_KTYPE = '09') AND MZ_EXAD = '" + EXAD_dt.Rows[i]["MZ_KCODE"].ToString() + "' ");
                int G3c = int.Parse(string.IsNullOrEmpty(G3) ? "0" : G3);
                //簡任
                string B30 = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_DLBASE WHERE MZ_STATUS2='Y' AND  MZ_ABORIGINE = 'Y' AND MZ_SRANK = (SELECT MZ_KCODE FROM A_KTYPE WHERE MZ_KCHI = '簡任' AND MZ_KTYPE = '09') AND MZ_EXAD = '" + EXAD_dt.Rows[i]["MZ_KCODE"].ToString() + "' ");
                int B30c = int.Parse(string.IsNullOrEmpty(B30) ? "0" : B30);
                //警監+簡任
                int G3B30 = G3c + B30c;

                //警正一階
                string G21 = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_DLBASE WHERE MZ_STATUS2='Y' AND  MZ_ABORIGINE = 'Y' AND  MZ_SRANK = (SELECT MZ_KCODE FROM A_KTYPE WHERE MZ_KCHI = '警正一階' AND MZ_KTYPE = '09') AND MZ_EXAD = '" + EXAD_dt.Rows[i]["MZ_KCODE"].ToString() + "' ");
                int G21c = int.Parse(string.IsNullOrEmpty(G21) ? "0" : G21);
                //警正二階
                string G22 = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_DLBASE WHERE MZ_STATUS2='Y' AND  MZ_ABORIGINE = 'Y' AND MZ_SRANK = (SELECT MZ_KCODE FROM A_KTYPE WHERE MZ_KCHI = '警正二階' AND MZ_KTYPE = '09') AND MZ_EXAD = '" + EXAD_dt.Rows[i]["MZ_KCODE"].ToString() + "' ");
                int G22c = int.Parse(string.IsNullOrEmpty(G22) ? "0" : G22);
                //警正三階
                string G23 = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_DLBASE WHERE MZ_STATUS2='Y' AND  MZ_ABORIGINE = 'Y' AND MZ_SRANK = (SELECT MZ_KCODE FROM A_KTYPE WHERE MZ_KCHI = '警正三階' AND MZ_KTYPE = '09') AND MZ_EXAD = '" + EXAD_dt.Rows[i]["MZ_KCODE"].ToString() + "' ");
                int G23c = int.Parse(string.IsNullOrEmpty(G23) ? "0" : G23);
                //警正巡佐
                string G24 = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_DLBASE WHERE MZ_STATUS2='Y' AND  MZ_ABORIGINE = 'Y' AND MZ_SRANK = (SELECT MZ_KCODE FROM A_KTYPE WHERE MZ_KCHI = '警正四階' AND MZ_KTYPE = '09') AND MZ_TBDV = '010' AND  MZ_EXAD = '" + EXAD_dt.Rows[i]["MZ_KCODE"].ToString() + "' ");
                int G24c = int.Parse(string.IsNullOrEmpty(G24) ? "0" : G24);

                //警正警員
                string G25 = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_DLBASE WHERE MZ_STATUS2='Y' AND  MZ_ABORIGINE = 'Y' AND MZ_SRANK = (SELECT MZ_KCODE FROM A_KTYPE WHERE MZ_KCHI = '警正四階' AND MZ_KTYPE = '09') AND MZ_TBDV = '011' AND  MZ_EXAD = '" + EXAD_dt.Rows[i]["MZ_KCODE"].ToString() + "' ");
                int G25c = int.Parse(string.IsNullOrEmpty(G25) ? "0" : G25);

                //警正小計
                int G2 = G21c + G22c + G23c + G24c + G25c;

                //警佐巡佐
                string G1s = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_DLBASE WHERE MZ_STATUS2='Y' AND  MZ_ABORIGINE = 'Y' AND MZ_SRANK LIKE 'G1%' AND MZ_TBDV = '010' AND MZ_EXAD = '" + EXAD_dt.Rows[i]["MZ_KCODE"].ToString() + "' ");
                int G1c = int.Parse(string.IsNullOrEmpty(G1s) ? "0" : G1s);

                //警佐警員
                string G2s = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_DLBASE WHERE MZ_STATUS2='Y' AND  MZ_ABORIGINE = 'Y' AND MZ_SRANK LIKE 'G1%' AND MZ_TBDV = '011' AND MZ_EXAD = '" + EXAD_dt.Rows[i]["MZ_KCODE"].ToString() + "' ");
                int G2c = int.Parse(string.IsNullOrEmpty(G2s) ? "0" : G2s);
                //比照警佐
                string G0 = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_DLBASE WHERE MZ_STATUS2='Y' AND  MZ_ABORIGINE = 'Y' AND MZ_SRANK IN (SELECT MZ_KCODE FROM A_KTYPE WHERE MZ_KCHI LIKE '比照警佐%' AND MZ_KTYPE = '09') AND MZ_EXAD = '" + EXAD_dt.Rows[i]["MZ_KCODE"].ToString() + "' ");
                int G0c = int.Parse(string.IsNullOrEmpty(G0) ? "0" : G0);
                //警佐小計
                int G1 = G1c + G2c + G0c;

                string EXAD = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND MZ_KCODE='" + EXAD_dt.Rows[i][0].ToString() + "'").Replace("新北市政府警察局", "");

                //新增一筆DataRow資料, 從畫面上取得DataRow的值
                DataRow dr = GCount_dt.NewRow();
                dr["EXAD"] = string.IsNullOrEmpty(EXAD) ? "新北市政府警察局" : EXAD;
                dr["G3c"] = G3c;
                dr["B30c"] = B30c;
                dr["G3B30"] = G3B30;
                dr["G21c"] = G21c;
                dr["G22c"] = G22c;
                dr["G23c"] = G23c;
                dr["G24c"] = G24c;
                dr["G25c"] = G25c;
                dr["G2"] = G2;
                dr["G1c"] = G1c;
                dr["G2c"] = G2c;
                dr["G0c"] = G0c;
                dr["G1"] = G1;
                GCount_dt.Rows.Add(dr);//將資料新增到DataTable
            }
            Session["rpt_dt"] = GCount_dt;
            string tmp_url = "A_rpt.aspx?fn=Aborginelist";
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
        }
    }
}
