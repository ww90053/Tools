using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TPPDDB._2_salary.rpt
{
    public partial class B_monthpay_detail_rpt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {


            if (!IsPostBack)
            {
                SalaryPublic.checkPermission();
                SalaryPublic.fillDropDownList(ref this.DropDownList_AD);
                SalaryPublic.fillUnitDropDownList(ref this.DropDownList_UNIT, DropDownList_AD.SelectedValue);

                TextBox_YEAR.Text = SalaryPublic.strRepublicDate().Substring(0, 3);
                DropDownList_MONTH.SelectedValue = System.DateTime.Now.Month.ToString();
                switch (Request["TYPE"].ToString())
                {
                    case "PAY":
                        lb_Title.Text = "每月薪資應發明細表";

                        break;
                    case "TAKE_OFF":
                        lb_Title.Text = "每月薪資應扣明細表";

                        break;
                    case "REPAIR_PAY":
                        lb_Title.Text = "補發薪資明細表";
                        tr_batch.Visible = true;
                        break;
                    case "REPAIR_TAKE_OFF":
                        lb_Title.Text = "補發薪資明細表(應扣)";
                        tr_batch.Visible = true;
                        break;
                }
            
            
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
                return TextBox_YEAR.Text.PadLeft(3, '0');
            }
        }

        private string strUNIT
        {
            get
            {
                return DropDownList_UNIT.SelectedValue;
            }
        }

        private string strMONTH
        {
            get
            {
                return DropDownList_MONTH.SelectedValue;
            }
        }

        protected void Button_SEARCH_Click(object sender, EventArgs e)
        {
            string tmp_url = "";
            switch (Request["TYPE"].ToString())
            {
                case "PAY":
                    tmp_url = "../B_rpt.aspx?fn=monthpay_detail" + "&YEAR=" + strAYEAR + "&MONTH=" + strMONTH + "&PAY_AD=" + strAD +"&UNIT="+strUNIT ;

                    break;
                case "TAKE_OFF":
                    tmp_url = "../B_rpt.aspx?fn=monthpaytakeoff_detail" + "&YEAR=" + strAYEAR + "&MONTH=" + strMONTH + "&PAY_AD=" + strAD + "&UNIT=" + strUNIT;

                    break;

                case "REPAIR_PAY":
                    tmp_url = "../B_rpt.aspx?fn=repairpay_detail" + "&YEAR=" + strAYEAR + "&MONTH=" + strMONTH + "&PAY_AD=" + strAD + "&UNIT=" + strUNIT + "&BATCH_NUMBER=" + txt_batch.Text;

                    break;
                case "REPAIR_TAKE_OFF":
                    tmp_url = "../B_rpt.aspx?fn=repairpaytakeoff_detail" + "&YEAR=" + strAYEAR + "&MONTH=" + strMONTH + "&PAY_AD=" + strAD + "&UNIT=" + strUNIT + "&BATCH_NUMBER=" + txt_batch.Text;

                    break;


            }


            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
        }



        protected void DropDownList_AD_SelectedIndexChanged(object sender, EventArgs e)
        {
            SalaryPublic.fillUnitDropDownList(ref this.DropDownList_UNIT, DropDownList_AD.SelectedValue);
        }

        protected void Button_CANCEL_Click(object sender, EventArgs e)
        {
            Response.Redirect("B_monthpay_detail_rpt.aspx?TPM_FION=" + Request["TPM_FION"] + "&TYPE=" + Request["TYPE"]);
        }

        protected void btn_Export_Click(object sender, EventArgs e)
        {
            string strSQL = "";
            string date = strAYEAR + strMONTH.PadLeft(2, '0');
            string filename = "";

            switch (Request["TYPE"].ToString())
            {
                case "PAY":
                    if (DropDownList_UNIT.SelectedIndex == 0)
                    {
                        strSQL = string.Format(@"SELECT PAY_UNIT 單位, MZ_POLNO 員工編號, IDCARD 身份證字號, MZ_SRANK 職等, SLVC_SPT 俸階, MZ_OCCC 職稱, MZ_NAME 姓名, ACCOUNT 帳戶, SALARYPAY1 薪俸, BOSS 主管加給, PROFESS 專業加給, WORKP 警勤加給, TECHNICS 技術加給, BONUS 工作獎助金, ADVENTIVE 外事加給, FAR 偏遠加給, ELECTRIC 繁重加給, OTHERADD 其他應發, TOTAL 應發金額, NOTE 備註 FROM (SELECT * FROM VW_MONTHPAY_DETAIL_POLNO WHERE AMONTH='{0}' AND PAY_AD='{1}' ORDER BY MZ_UNIT, MZ_POLNO) VW", date, strAD);
                    }
                    else
                    {
                        strSQL = string.Format(@"SELECT PAY_UNIT 單位, MZ_POLNO 員工編號, IDCARD 身份證字號, MZ_SRANK 職等, SLVC_SPT 俸階, MZ_OCCC 職稱, MZ_NAME 姓名, ACCOUNT 帳戶, SALARYPAY1 薪俸, BOSS 主管加給, PROFESS 專業加給, WORKP 警勤加給, TECHNICS 技術加給, BONUS 工作獎助金, ADVENTIVE 外事加給, FAR 偏遠加給, ELECTRIC 繁重加給, OTHERADD 其他應發, TOTAL 應發金額, NOTE 備註 FROM (SELECT * FROM VW_MONTHPAY_DETAIL_POLNO WHERE AMONTH='{0}' AND PAY_AD='{1}' AND MZ_UNIT = '{2}' ORDER BY MZ_UNIT, MZ_POLNO) VW", date, strAD, strUNIT);
                    }

                    filename = "每月薪資應發明細";

                    break;
                case "TAKE_OFF":
                    if (DropDownList_UNIT.SelectedIndex == 0)
                    {
                        strSQL = string.Format(@"SELECT PAY_UNIT 單位, MZ_POLNO 員工編號, IDCARD 身份證字號, MZ_SRANK 職等, SLVC_SPT 俸階, MZ_OCCC 職稱, MZ_NAME 姓名, ACCOUNT 帳戶, ADD_SUM 應發金額, INSURANCEPAY 保險費, HEALTHPAY 健保費, CONCUR3PAY 退撫金, TAX 薪津所得稅, HEALTHPAY1 健保費補扣款, MONTHPAY_TAX 薪資扣款1, MONTHPAY 薪資扣款2, EXTRA01 法院扣款, EXTRA02 國宅貸款, EXTRA03 銀行貸款, EXTRA04 分期付款, EXTRA05 優惠存款, EXTRA06 員工宿舍費, EXTRA07 福利互助金, EXTRA08 伙食費, EXTRA09 退撫金扣款, OTHERMINUS 其他應扣, DES_SUM 應扣金額, TOTAL 實發金額, NOTE 備註 FROM  VW_MONTHPAY_TAKEOFF_DETAIL_POL WHERE AMONTH='{0}' AND PAY_AD='{1}' ORDER BY MZ_UNIT, MZ_POLNO ", date, strAD);
                    }
                    else
                    {
                        strSQL = string.Format(@"SELECT PAY_UNIT 單位, MZ_POLNO 員工編號, IDCARD 身份證字號, MZ_SRANK 職等, SLVC_SPT 俸階, MZ_OCCC 職稱, MZ_NAME 姓名, ACCOUNT 帳戶, ADD_SUM 應發金額, INSURANCEPAY 保險費, HEALTHPAY 健保費, CONCUR3PAY 退撫金, TAX 薪津所得稅, HEALTHPAY1 健保費補扣款, MONTHPAY_TAX 薪資扣款1, MONTHPAY 薪資扣款2, EXTRA01 法院扣款, EXTRA02 國宅貸款, EXTRA03 銀行貸款, EXTRA04 分期付款, EXTRA05 優惠存款, EXTRA06 員工宿舍費, EXTRA07 福利互助金, EXTRA08 伙食費, EXTRA09 退撫金扣款, OTHERMINUS 其他應扣, DES_SUM 應扣金額, TOTAL 實發金額, NOTE 備註 FROM  VW_MONTHPAY_TAKEOFF_DETAIL_POL WHERE AMONTH='{0}' AND PAY_AD='{1}' AND MZ_UNIT = '{2}' ORDER BY MZ_UNIT, MZ_POLNO ", date, strAD, strUNIT);
                    }
                    filename = "每月薪資應扣明細";
                    break;
                case "REPAIR_PAY":
                    if (DropDownList_UNIT.SelectedIndex == 0)
                    {
                        strSQL = string.Format(@"SELECT PAY_UNIT 單位, MZ_POLNO 員工編號, IDCARD 身份證字號, MZ_SRANK 職等, SLVC_SPT 俸階, MZ_OCCC 職稱, MZ_NAME 姓名, ACCOUNT 帳戶, SALARYPAY1 薪俸, BOSS 主管加給, PROFESS 專業加給, WORKP 警勤加給, TECHNICS 技術加給, BONUS 工作獎助金, ADVENTIVE 外事加給, FAR 偏遠加給, ELECTRIC 繁重加給, OTHERADD 其他應發, TOTAL 應發金額, NOTE 備註 FROM (SELECT * FROM VW_REPAIRPAY_DETAIL WHERE AMONTH='{0}' AND PAY_AD='{1}' AND BATCH_NUMBER='{2}' ORDER BY MZ_UNIT, MZ_POLNO) VW", date, strAD, txt_batch.Text);
                    }
                    else
                    {
                        strSQL = string.Format(@"SELECT PAY_UNIT 單位, MZ_POLNO 員工編號, IDCARD 身份證字號, MZ_SRANK 職等, SLVC_SPT 俸階, MZ_OCCC 職稱, MZ_NAME 姓名, ACCOUNT 帳戶, SALARYPAY1 薪俸, BOSS 主管加給, PROFESS 專業加給, WORKP 警勤加給, TECHNICS 技術加給, BONUS 工作獎助金, ADVENTIVE 外事加給, FAR 偏遠加給, ELECTRIC 繁重加給, OTHERADD 其他應發, TOTAL 應發金額, NOTE 備註 FROM (SELECT * FROM VW_REPAIRPAY_DETAIL WHERE AMONTH='{0}' AND PAY_AD='{1}' AND MZ_UNIT = '{2}' AND BATCH_NUMBER='{3}' ORDER BY MZ_UNIT, MZ_POLNO) VW", date, strAD, strUNIT, txt_batch.Text);
                    }
                    filename = "補發薪資應發明細";
                    break;
                case "REPAIR_TAKE_OFF":
                    if (DropDownList_UNIT.SelectedIndex == 0)
                    {
                        strSQL = string.Format(@"SELECT PAY_UNIT 單位, MZ_POLNO 員工編號, IDCARD 身份證字號, MZ_SRANK 職等, SLVC_SPT 俸階, MZ_OCCC 職稱, MZ_NAME 姓名, ACCOUNT 帳戶, ADD_SUM 應發金額, INSURANCEPAY 保險費, HEALTHPAY 健保費, CONCUR3PAY 退撫金, TAX 薪津所得稅, HEALTHPAY1 健保費補扣款, MONTHPAY_TAX 薪資扣款1, MONTHPAY 薪資扣款2, EXTRA01 法院扣款, EXTRA02 國宅貸款, EXTRA03 銀行貸款, EXTRA04 分期付款, EXTRA05 優惠存款, EXTRA06 員工宿舍費, EXTRA07 福利互助金, EXTRA08 伙食費, EXTRA09 退撫金扣款, OTHERMINUS 其他應扣, DES_SUM 應扣金額, TOTAL 實發金額, NOTE 備註 FROM (SELECT * FROM VW_REPAIRPAY_TAKEOFF_DETAIL WHERE AMONTH='{0}' AND PAY_AD='{1}' AND BATCH_NUMBER='{2}' ORDER BY MZ_UNIT, MZ_POLNO) VW", date, strAD, txt_batch.Text);
                    }
                    else
                    {
                        strSQL = string.Format(@"SELECT PAY_UNIT 單位, MZ_POLNO 員工編號, IDCARD 身份證字號, MZ_SRANK 職等, SLVC_SPT 俸階, MZ_OCCC 職稱, MZ_NAME 姓名, ACCOUNT 帳戶, ADD_SUM 應發金額, INSURANCEPAY 保險費, HEALTHPAY 健保費, CONCUR3PAY 退撫金, TAX 薪津所得稅, HEALTHPAY1 健保費補扣款, MONTHPAY_TAX 薪資扣款1, MONTHPAY 薪資扣款2, EXTRA01 法院扣款, EXTRA02 國宅貸款, EXTRA03 銀行貸款, EXTRA04 分期付款, EXTRA05 優惠存款, EXTRA06 員工宿舍費, EXTRA07 福利互助金, EXTRA08 伙食費, EXTRA09 退撫金扣款, OTHERMINUS 其他應扣, DES_SUM 應扣金額, TOTAL 實發金額, NOTE 備註 FROM (SELECT * FROM VW_REPAIRPAY_TAKEOFF_DETAIL WHERE AMONTH='{0}' AND PAY_AD='{1}' AND MZ_UNIT = '{2}' AND BATCH_NUMBER='{3}' ORDER BY MZ_UNIT, MZ_POLNO) VW", date, strAD, strUNIT, txt_batch.Text);
                    }
                    filename = "補發薪資應扣明細";
                    break;


            }



           

            DataTable dt = new DataTable();
            dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");

            Excel.Dt2Excel(dt, filename);
        }
    }
}
