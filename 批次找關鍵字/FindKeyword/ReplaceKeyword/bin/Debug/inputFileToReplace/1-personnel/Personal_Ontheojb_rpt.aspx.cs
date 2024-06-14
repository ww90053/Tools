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
    public partial class Personal_ontheojb_rpt : System.Web.UI.Page
    {
        int TPM_FION=0;
        string strSQL;
        DataTable temp = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                A.check_power();
            }

            A.set_Panel_EnterToTAB(ref this.Panel1);
            A.set_Panel_EnterToTAB(ref this.Panel2);
          
            ViewState["SMALL"] = string.Empty;
            ViewState["BIG"] = string.Empty;
        }

        protected void Button_MAKE_RPT_Click(object sender, EventArgs e)
        {
            rpt_data_job(TextBox_MZ_ID.Text);
            Session["SMALL"] = ViewState["SMALL"];
            Session["BIG"] = ViewState["BIG"];
            Session["BOSS"] = ViewState["BOSS"];
            Session["BOSS1"] = ViewState["BOSS1"];
            Session["MZ_ID"] = TextBox_MZ_ID.Text;
            string tmp_url = "A_rpt.aspx?fn=onthejob&TPM_FION=" + TPM_FION;
            ScriptManager.RegisterClientScriptBlock(TextBox_MZ_ID, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
        }

        /// <summary>
        /// 產生在職證明書 報表大小標題
        /// </summary>
        /// <param name="MZ_ID">身分證字號</param>
        protected void rpt_data_job(string MZ_ID)
        {
            //組日期
            strSQL = string.Format("SELECT MZ_ADATE,MZ_NID,MZ_AD FROM A_DLBASE WHERE MZ_ID='{0}'", MZ_ID);
            temp = new DataTable();
            temp.Load(OracleHelper.ExecuteReader(OracleHelper.connStr, CommandType.Text, strSQL));
            if (temp.Rows.Count == 0)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('查無資料');", true);
                return;
            }
            string adate = temp.Rows[0]["MZ_ADATE"].ToString();
            if (!string.IsNullOrEmpty(adate))
            {
                string year = adate.Substring(0, 3);
                string month = adate.Substring(3, 2);
                string day = adate.Substring(5, 2);
                //string nid = temp.Rows[0]["MZ_NID"].ToString();
                string nid = ListBox_MZ_PRID.SelectedItem.Text + "字第" + TextBox_NO.Text + "號";

                ViewState["SMALL"] = string.Format("中華民國{0}年{1}月{2}日 {3}", int.Parse(year), month, day, nid);
                ViewState["BIG"] = string.Format("中  華  民  國   {0} 年   {1} 月   {2} 日", int.Parse(DateTime.Now.Year.ToString()) - 1911, DateTime.Now.Month.ToString().PadLeft(2, '0'), DateTime.Now.Day.ToString().PadLeft(2, '0'));
            }
            
            //組報表 職稱   姓名
            strSQL = string.Format("SELECT MZ_MASTER_NAME,MZ_MASTER_POS FROM A_CHKAD WHERE MZ_AD='{0}'", temp.Rows[0]["MZ_AD"].ToString());
            temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");

            string name = temp.Rows[0]["MZ_MASTER_NAME"].ToString();
            string result_name = string.Empty;
            string result_namemark = string.Empty;
            for (int i = 0; i < name.Length; i++)
            {
                result_name += name.Substring(i, 1) + "  ";
                if (i == 0)
                    result_namemark += name.Substring(i, 1) + "  ";
                else
                    result_namemark += "○" + "  ";
            }
            ViewState["BOSS"] = string.Format("{0}    {1}", temp.Rows[0]["MZ_MASTER_POS"].ToString(), result_name);
            ViewState["BOSS1"] = string.Format("{0}    {1}", temp.Rows[0]["MZ_MASTER_POS"].ToString(), result_namemark);
            Session["TITLE"] = string.Format("{0}在職證明書", o_A_DLBASE.CMZAD(MZ_ID));
        }

        protected void Button_MAKE_RPT1_Click(object sender, EventArgs e)
        {
            rpt_data_job1(TextBox_MZ_ID.Text.ToUpper());
            //Session["SMALL"] = ViewState["SMALL"];
            Session["SMALLDay"] = ViewState["SMALLDay"];
            Session["SMALLNid"] = ViewState["SMALLNid"];
            Session["OPENING"] = ViewState["OPENING"];
            Session["BIG"] = ViewState["BIG"];
            Session["BOSS"] = ViewState["BOSS"];
            Session["BOSS1"] = ViewState["BOSS1"];
            Session["MZ_ID"] = TextBox_MZ_ID.Text;
            string tmp_url = "A_rpt.aspx?fn=experience&TPM_FION=" + TPM_FION;
            ScriptManager.RegisterClientScriptBlock(TextBox_MZ_ID, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
        }

        /// 產生經驗證明書 報表大小標題
        protected void rpt_data_job1(string MZ_ID)
        {
            //組日期
            strSQL = string.Format("SELECT MZ_ADATE,MZ_NID,MZ_AD,MZ_NAME,MZ_BIR FROM A_DLBASE WHERE MZ_ID='{0}'", MZ_ID);
            temp = new DataTable();
            temp.Load(OracleHelper.ExecuteReader(OracleHelper.connStr, CommandType.Text, strSQL));
            string adate = temp.Rows[0]["MZ_ADATE"].ToString();
            string name1 = temp.Rows[0]["MZ_NAME"].ToString();
            string bir = temp.Rows[0]["MZ_BIR"].ToString();

            if (!string.IsNullOrEmpty(adate))
            {
                string year = adate.Substring(0, 3);
                string month = adate.Substring(3, 2);
                string day = adate.Substring(5, 2);
                string nid = ListBox_MZ_PRID.SelectedItem.Text + "字第" + TextBox_NO.Text + "號";

                ViewState["SMALLDay"] = string.Format("中華民國{0}年{1}月{2}日", int.Parse(year), month, day);
                ViewState["SMALLNid"] = string.Format("{0}", nid);

                ViewState["BIG"] = string.Format("中  華  民  國   {0} 年   {1} 月   {2} 日", int.Parse(DateTime.Now.Year.ToString()) - 1911, DateTime.Now.Month.ToString().PadLeft(2, '0'), DateTime.Now.Day.ToString().PadLeft(2, '0'));
            }
            string year1 = string.Empty;
            string month1 = string.Empty;
            string day1 = string.Empty;
            try
            {

                year1 = (bir.Substring(0, 1) == "0" ? bir.Substring(1, 2) : bir.Substring(0, 3));
                month1 = bir.Substring(3, 2);
                day1 = bir.Substring(5, 2);

            }
            catch { }
            ViewState["OPENING"] = "查　" + name1 + "君，民國 " + year1 + " 年 " + month1 + " 月 " + day1 + " 日生，曾任本局所屬單位職務證明如下：";
            //組報表 職稱   姓名
            strSQL = string.Format("SELECT MZ_MASTER_NAME,MZ_MASTER_POS FROM A_CHKAD WHERE MZ_AD='{0}'", temp.Rows[0]["MZ_AD"].ToString());
            temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");

            string name = temp.Rows[0]["MZ_MASTER_NAME"].ToString();
            string result_name = string.Empty;
            string result_namemark = string.Empty;

            for (int i = 0; i < name.Length; i++)
            {
                result_name += name.Substring(i, 1) + "  ";
                if (i == 0)
                    result_namemark += name.Substring(i, 1) + "  ";
                else
                    result_namemark += "○" + "  ";
            }
            ViewState["BOSS"] = string.Format("{0}    {1}", temp.Rows[0]["MZ_MASTER_POS"].ToString(), result_name);
            ViewState["BOSS1"] = string.Format("{0}    {1}", temp.Rows[0]["MZ_MASTER_POS"].ToString(), result_namemark);
            Session["TITLE"] = string.Format("{0}經歷證明書", o_A_DLBASE.CMZAD(MZ_ID));
        }

        protected void TextBox_MZ_ID_TextChanged(object sender, EventArgs e)
        {
            TextBox_MZ_ID.Text = TextBox_MZ_ID.Text.ToUpper();
        }
    }
}
