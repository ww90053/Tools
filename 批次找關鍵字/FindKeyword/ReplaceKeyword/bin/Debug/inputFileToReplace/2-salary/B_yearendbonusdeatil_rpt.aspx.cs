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

namespace TPPDDB._2_salary
{
    public partial class B_yearendbonusdeatil_rpt : System.Web.UI.Page
    {
        string strSQL;
        DataTable temp = new DataTable();
        DataTable rpt_dt;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SalaryPublic.checkPermission();

                SalaryPublic.fillDropDownList(ref this.DropDownList_AD);
                TextBox_YEAR.Text = SalaryPublic.strRepublicLastYear();
                strSQL = string.Format("SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='25' AND MZ_KCODE IN (SELECT MZ_UNIT FROM A_UNIT_AD WHERE MZ_AD='{0}')", strAD);
                temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
                DropDownList_UNIT.DataSource = temp;
                DropDownList_UNIT.DataTextField = "MZ_KCHI";
                DropDownList_UNIT.DataValueField = "MZ_KCODE";
                DropDownList_UNIT.DataBind();
                DropDownList_UNIT.Items.Insert(0, "請選擇");
            }
        }

        private String strAD
        {
            get
            {
                return DropDownList_AD.SelectedValue;
            }
        }

        private string strAYEAR
        {
            get
            {
                return TextBox_YEAR.Text;
            }
        }

        private string strUNIT
        {
            get
            {
                return DropDownList_UNIT.SelectedValue;
            }
        }

        protected void Button_SEARCH_Click(object sender, EventArgs e)
        {
            string strSQL;

            if (DropDownList_UNIT.SelectedIndex == 0)
                strSQL = string.Format("SELECT ROWNUM NUM, VW.* FROM (SELECT * FROM VW_YEAREND_BONUS_DETAIL WHERE PAY_AD = '{0}' AND AYEAR = '{1}'ORDER BY MZ_UNIT, MZ_POLNO) VW  ORDER BY NUM", strAD, strAYEAR);
            else
                strSQL = string.Format("SELECT ROWNUM NUM, VW.* FROM (SELECT * FROM VW_YEAREND_BONUS_DETAIL WHERE PAY_AD = '{0}' AND MZ_UNIT = '{1}' AND AYEAR = '{2}' ORDER BY MZ_UNIT, MZ_POLNO) VW  ORDER BY NUM", strAD, strUNIT, strAYEAR);

            //2013/01/29 修改考績獎金報表格式
            Session["rpt_dt"] = Salary.addPageNumber(o_DBFactory.ABC_toTest.Create_Table(strSQL, "VW"), 13, "PAY_UNIT");
            //Session["rpt_dt"] = o_DBFactory.ABC_toTest.Create_Table(strSQL, "VW");

            Session["SalaryReportAD"] = o_A_KTYPE.CODE_TO_NAME(strAD, "04");
            Session["TITLE"] = strAYEAR + "年";
            Session["TITLE1"] = "發給年終工作獎金1.5月";
            string tmp_url = "B_rpt.aspx?fn=yearpay_detail";
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
        }

        protected DataTable rpt_dt_init()
        {
            rpt_dt = new DataTable();
            rpt_dt.Columns.Add("NUM", typeof(string));
            rpt_dt.Columns.Add("MZ_SRANK", typeof(string));
            rpt_dt.Columns.Add("SLVC_SPT", typeof(string));
            rpt_dt.Columns.Add("MZ_OCCC", typeof(string));
            rpt_dt.Columns.Add("MZ_NAME", typeof(string));
            rpt_dt.Columns.Add("MZ_ADATE", typeof(string));
            rpt_dt.Columns.Add("SALARYPAY1", typeof(int));
            rpt_dt.Columns.Add("BOSS", typeof(int));
            rpt_dt.Columns.Add("PROFESS", typeof(int));
            rpt_dt.Columns.Add("PAY", typeof(string));
            rpt_dt.Columns.Add("SSUM", typeof(int));
            rpt_dt.Columns.Add("TAX", typeof(int));
            rpt_dt.Columns.Add("EXTRA01", typeof(int));
            rpt_dt.Columns.Add("TOTAL", typeof(int));
            rpt_dt.Columns.Add("NOTE", typeof(string));
            rpt_dt.Columns.Add("IDCARD", typeof(string));
            rpt_dt.Columns.Add("MZ_POLNO", typeof(string));
            rpt_dt.Columns.Add("ACCOUNT", typeof(string));
            return rpt_dt;
        }

        protected void DropDownList_AD_SelectedIndexChanged(object sender, EventArgs e)
        {
            strSQL = string.Format("SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='25' AND MZ_KCODE IN (SELECT MZ_UNIT FROM A_UNIT_AD WHERE MZ_AD='{0}')", strAD);
            temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
            DropDownList_UNIT.DataSource = temp;
            DropDownList_UNIT.DataTextField = "MZ_KCHI";
            DropDownList_UNIT.DataValueField = "MZ_KCODE";
            DropDownList_UNIT.DataBind();
            DropDownList_UNIT.Items.Insert(0, "請選擇");
        }

        protected void Button_CANCEL_Click(object sender, EventArgs e)
        {
            Response.Redirect("B_yearendbonusdetail_rpt.aspx?TPM_FION=" + Request.QueryString["TPM_FION"]);
        }

        //2013/02/04新增年終獎金匯出EXCEL資料表
        protected void btn_Export_Click(object sender, EventArgs e)
        {
            string excelTitle = strAYEAR + "年";
            excelTitle += o_A_KTYPE.RAD(strAD);
            rpt_dt = rpt_dt_init();

            if (DropDownList_UNIT.SelectedIndex == 0)
            {
                strSQL = string.Format(@"SELECT ROWNUM 編號,MZ_SRANK 等級,SLVC_SPT 俸階,MZ_OCCC 職務名稱,MZ_NAME 姓名,MZ_ADATE 初任日期,SALARYPAY1 薪俸,BOSS 主管加給,PROFESS 專業加給,PAY 在職月數,SSUM 合計,TAX 所得稅稅率,EXTRA01 法院扣款,TOTAL 實領金額, NOTE 備註,IDCARD 身分證號,ACCOUNT 帳號 FROM (SELECT  * FROM VW_YEAREND_BONUS_DETAIL WHERE PAY_AD = '{0}' AND AYEAR = '{1}' ORDER BY  MZ_UNIT,MZ_POLNO) VW  ORDER BY 編號", strAD, strAYEAR);
            }
            else
            {
                excelTitle += o_A_KTYPE.RUNIT(strUNIT);
                strSQL = string.Format(@"SELECT ROWNUM 編號,MZ_SRANK 等級,SLVC_SPT 俸階,MZ_OCCC 職務名稱,MZ_NAME 姓名,MZ_ADATE 初任日期,SALARYPAY1 薪俸,BOSS 主管加給,PROFESS 專業加給,PAY 在職月數,SSUM 合計,TAX 所得稅稅率,EXTRA01 法院扣款,TOTAL 實領金額, NOTE 備註,IDCARD 身分證號,ACCOUNT 帳號 FROM (SELECT  * FROM VW_YEAREND_BONUS_DETAIL WHERE PAY_AD = '{0}' AND MZ_UNIT = '{1}' AND AYEAR = '{2}'  ORDER BY  MZ_UNIT,MZ_POLNO) VW  ORDER BY 編號", strAD, strUNIT, strAYEAR);
            }

            DataTable dt = new DataTable();
            dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");

            excelTitle += "年終獎金明細表";
            Excel.Dt2Excel(dt, excelTitle);
        }
    }
}
