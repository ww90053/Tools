using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TPPDDB._1_personnel
{
    public partial class Personal_ABORIGINE2_rpt : System.Web.UI.Page
    {
        int TPM_FION=0;
        

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                A.check_power();            
                chk_TPMGroup();
            }

           

           
        }

        /// <summary>
        /// 群組權限
        /// </summary>
        protected void chk_TPMGroup()
        {
            switch (o_a_Function.strGID(Session["ADPMZ_ID"].ToString()))
            {
                case "A":
                case "B":

                    break;
                case "C":
                    DropDownList_AD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                    DropDownList_AD.Enabled = false;
                    break;
            }
        }
       
        protected void btABORIGINE1_Click(object sender, EventArgs e)
        {
            DataTable ABORIGINE2 = new DataTable();

            ABORIGINE2.Columns.Clear();

            ABORIGINE2.Columns.Add("EXAD", typeof(string));
            ABORIGINE2.Columns.Add("TOTAL_COUNT", typeof(int));
            ABORIGINE2.Columns.Add("COUNT_M_1455", typeof(int));
            ABORIGINE2.Columns.Add("COUNT_M_1460", typeof(int));
            ABORIGINE2.Columns.Add("COUNT_M_1450", typeof(int));
            ABORIGINE2.Columns.Add("COUNT_M_1453", typeof(int));
            ABORIGINE2.Columns.Add("COUNT_F_1455", typeof(int));
            ABORIGINE2.Columns.Add("COUNT_F_1460", typeof(int));
            ABORIGINE2.Columns.Add("COUNT_F_1450", typeof(int));
            ABORIGINE2.Columns.Add("COUNT_F_1453", typeof(int));


            string selectEXAD_Sting = "SELECT MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE='04' AND MZ_KCODE LIKE '38213%' ORDER BY MZ_KCODE ";

            DataTable tempEXAD_dt = new DataTable();

            tempEXAD_dt = o_DBFactory.ABC_toTest.Create_Table(selectEXAD_Sting, "GET_EXAD");

            for (int i = 0; i < tempEXAD_dt.Rows.Count; i++)
            {
                //警務員
                string M_1455 = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_DLBASE WHERE MZ_STATUS2='Y' AND  MZ_OCCC='1455' AND MZ_SM='1' AND MZ_ABORIGINE='Y' AND MZ_EXAD='" + tempEXAD_dt.Rows[i][0].ToString() + "'");
                string F_1455 = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_DLBASE WHERE MZ_STATUS2='Y' AND  MZ_OCCC='1455' AND MZ_SM='2' AND MZ_ABORIGINE='Y' AND MZ_EXAD='" + tempEXAD_dt.Rows[i][0].ToString() + "'");
                int count_m_1455 = int.Parse(string.IsNullOrEmpty(M_1455) ? "0" : M_1455);
                int count_f_1455 = int.Parse(string.IsNullOrEmpty(F_1455) ? "0" : F_1455);
                //巡佐
                string M_1460 = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_DLBASE WHERE MZ_STATUS2='Y' AND  MZ_OCCC='1460' AND MZ_SM='1' AND MZ_ABORIGINE='Y' AND MZ_EXAD='" + tempEXAD_dt.Rows[i][0].ToString() + "'");
                string F_1460 = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_DLBASE WHERE MZ_STATUS2='Y' AND  MZ_OCCC='1460' AND MZ_SM='2' AND MZ_ABORIGINE='Y' AND MZ_EXAD='" + tempEXAD_dt.Rows[i][0].ToString() + "'");
                int count_m_1460 = int.Parse(string.IsNullOrEmpty(M_1460) ? "0" : M_1460);
                int count_f_1460 = int.Parse(string.IsNullOrEmpty(F_1460) ? "0" : F_1460);

                //巡官
                string M_1450 = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_DLBASE WHERE MZ_STATUS2='Y' AND  MZ_OCCC='1450' AND MZ_SM='1' AND MZ_ABORIGINE='Y' AND MZ_EXAD='" + tempEXAD_dt.Rows[i][0].ToString() + "'");
                string F_1450 = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_DLBASE WHERE MZ_STATUS2='Y' AND  MZ_OCCC='1450' AND MZ_SM='2' AND MZ_ABORIGINE='Y' AND MZ_EXAD='" + tempEXAD_dt.Rows[i][0].ToString() + "'");
                int count_m_1450 = int.Parse(string.IsNullOrEmpty(M_1450) ? "0" : M_1450);
                int count_f_1450 = int.Parse(string.IsNullOrEmpty(F_1450) ? "0" : F_1450);
                //警員
                string M_1453 = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_DLBASE WHERE MZ_STATUS2='Y' AND  MZ_OCCC='1453' AND MZ_SM='1' AND MZ_ABORIGINE='Y' AND MZ_EXAD='" + tempEXAD_dt.Rows[i][0].ToString() + "'");
                string F_1453 = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_DLBASE WHERE MZ_STATUS2='Y' AND  MZ_OCCC='1453' AND MZ_SM='2' AND MZ_ABORIGINE='Y' AND MZ_EXAD='" + tempEXAD_dt.Rows[i][0].ToString() + "'");
                int count_m_1453 = int.Parse(string.IsNullOrEmpty(M_1453) ? "0" : M_1453);
                int count_f_1453 = int.Parse(string.IsNullOrEmpty(F_1453) ? "0" : F_1453);

                DataRow dr = ABORIGINE2.NewRow();

                string EXAD=o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND MZ_KCODE='" + tempEXAD_dt.Rows[i][0].ToString() + "'").Replace("新北市政府警察局","");

                dr["EXAD"] = string.IsNullOrEmpty(EXAD) ? "新北市政府警察局" : EXAD;
                dr["TOTAL_COUNT"] = count_m_1455 + count_m_1460 + count_m_1450 + count_m_1453 + count_f_1455 + count_f_1460 + count_f_1450 + count_f_1453;
                dr["COUNT_M_1455"] = count_m_1455;
                dr["COUNT_M_1460"] = count_m_1460;
                dr["COUNT_M_1450"] = count_m_1450;
                dr["COUNT_M_1453"] = count_m_1453;
                dr["COUNT_F_1455"] = count_f_1455;
                dr["COUNT_F_1460"] = count_f_1460;
                dr["COUNT_F_1450"] = count_f_1450;
                dr["COUNT_F_1453"] = count_f_1453;

                ABORIGINE2.Rows.Add(dr);
            }

            Session["rpt_dt"] = ABORIGINE2;

            Session["TITLE"] = string.Format("新北市政府警察局{0}年{1}月原住民人數統計管制表", (DateTime.Now.Year - 1911).ToString(), DateTime.Now.Month.ToString());

            string tmp_url = "A_rpt.aspx?fn=aborginelist2&TPM_FION=" + TPM_FION;

            ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
        }

        protected void btABORIGINE3_Click(object sender, EventArgs e)
        {
            Session["TITLE"] = string.Format("{0}原住民員警名冊", DropDownList_AD.SelectedItem.Text);
            Session["MZ_EXAD"] = DropDownList_AD.SelectedValue;

            string tmp_url = "A_rpt.aspx?fn=aborginelist3&TPM_FION=" + TPM_FION;

            ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
        }
    }
}
