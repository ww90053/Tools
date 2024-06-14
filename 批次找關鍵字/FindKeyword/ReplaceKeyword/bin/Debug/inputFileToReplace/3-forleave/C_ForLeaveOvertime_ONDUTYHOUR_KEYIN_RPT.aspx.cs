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

namespace TPPDDB._3_forleave
{
    public partial class C_ForLeaveOvertime_ONDUTYHOUR_KEYIN_RPT : System.Web.UI.Page
    {
        string strSQL;
        DataTable temp = new DataTable();//暫存dt
        DataTable onduty_hour; //報表dt
        int TPM_FION=0;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                C.check_power();
                //MQ-----------------------20100331
                C.set_Panel_EnterToTAB(ref this.Panel1);
                int j = 0;
                for (int i = 100; i <= DateTime.Now.Year - 1911; i++)
                {

                    DropDownList_YEAR.Items.Insert(j, new ListItem(i.ToString(), i.ToString()));

                    j++;
                }
            }
        }

        /// <summary>
        /// 報表dt初始化
        /// </summary>
        /// <returns></returns>
        protected DataTable onduty_hour_init()
        {
            onduty_hour = new DataTable();
            onduty_hour.Columns.Add("MZ_OCCC", typeof(string));
            onduty_hour.Columns.Add("MZ_NAME", typeof(string));
            onduty_hour.Columns.Add("DUTY_DATE", typeof(string));
            onduty_hour.Columns.Add("MZ_TOTAL_HOUR", typeof(int));
            onduty_hour.Columns.Add("MZ_ONDUTY_PAY", typeof(string));
            onduty_hour.Columns.Add("MZ_TOTAL_PAY", typeof(int));
            onduty_hour.Columns.Add("MZ_POLNO", typeof(string));
            onduty_hour.Columns.Add("MZ_REMARK", typeof(string));
            return onduty_hour;
        }

        /// <summary>
        /// 報表產生dt
        /// </summary>
        protected void make_rpt()
        {
            onduty_hour = onduty_hour_init();
            strSQL = string.Format("SELECT MZ_ID,MZ_YEAR,MZ_MONTH,MZ_ONDUTY_PAY,MZ_TOTAL_PAY,MZ_TOTAL_HOUR,MZ_REMARK FROM C_ONDUTY_HOUR WHERE MZ_YEAR='{2}' AND MZ_MONTH='{3}' AND MZ_ID IN (SELECT MZ_ID FROM A_DLBASE WHERE MZ_EXAD='{0}' AND MZ_EXUNIT='{1}')", DropDownList_AD.SelectedValue
                                   , DropDownList_UNIT.SelectedValue, DropDownList_YEAR.SelectedValue, DropDownList_MONTH.SelectedValue.PadLeft(2, '0'));
            temp = new DataTable();
            temp.Load(OracleHelper.ExecuteReader(OracleHelper.connStr, CommandType.Text, strSQL));

            //塞報表所需資料及組日期01,02,03,04
            foreach (DataRow dr in temp.Rows)
            {

                strSQL = string.Format("SELECT \"1\",\"2\",\"3\",\"4\",\"5\",\"6\",\"7\",\"8\",\"9\",\"10\",\"11\",\"12\"," +
                                              " \"13\",\"14\",\"15\",\"16\",\"17\",\"18\",\"19\",\"20\"," +
                                              " \"21\",\"22\",\"23\",\"24\",\"25\",\"26\",\"27\",\"28\"," +
                                              " \"29\",\"30\",\"31\" FROM C_ONDUTY_HOUR WHERE MZ_ID='{0}' AND MZ_YEAR='{1}' AND MZ_MONTH='{2}'", dr["MZ_ID"], dr["MZ_YEAR"], dr["MZ_MONTH"]);
                DataTable date = new DataTable();
                date.Load(OracleHelper.ExecuteReader(OracleHelper.connStr, CommandType.Text, strSQL));
                string date_merge = string.Empty;//日期字串
                for (int i = 0; i < 31; i++)
                {
                    string col = i.ToString();
                    if (date.Rows[0][i].ToString() == "1" || date.Rows[0][i].ToString() == "2")
                        date_merge += (i + 1).ToString().PadLeft(2, '0') + ",";
                }
                DataRow rpt_dr = onduty_hour.NewRow();
                rpt_dr["MZ_OCCC"] = o_A_DLBASE.OCCC(dr["MZ_ID"].ToString());
                rpt_dr["MZ_NAME"] = o_A_DLBASE.CNAME(dr["MZ_ID"].ToString());
                rpt_dr["DUTY_DATE"] = date_merge.Substring(0, date_merge.Length - 1);
                rpt_dr["MZ_TOTAL_HOUR"] = dr["MZ_TOTAL_HOUR"];
                rpt_dr["MZ_ONDUTY_PAY"] = dr["MZ_ONDUTY_PAY"];
                rpt_dr["MZ_TOTAL_PAY"] = dr["MZ_TOTAL_PAY"];
                rpt_dr["MZ_POLNO"] = o_A_DLBASE.POLNO(dr["MZ_ID"].ToString());
                rpt_dr["MZ_REMARK"] = dr["MZ_REMARK"].ToString();
                onduty_hour.Rows.Add(rpt_dr);
            }
            ViewState["TITLE"] = string.Format("{0}{1}{2}年{3}月份值日費印領清冊", o_A_KTYPE.RAD(DropDownList_AD.SelectedValue), o_A_KTYPE.RUNIT(DropDownList_UNIT.SelectedValue), int.Parse(DropDownList_YEAR.SelectedValue), DropDownList_MONTH.SelectedValue.PadLeft(2, '0'));
            ViewState["rpt_dt"] = onduty_hour;
        }

        protected void Button_MAKE_RPT_Click(object sender, EventArgs e)
        {
            make_rpt();//產生報表
            Session["TITLE"] = ViewState["TITLE"];
            Session["rpt_dt"] = ViewState["rpt_dt"] as DataTable;
            string tmp_url = "C_rpt.aspx?fn=ondutyhour&TPM_FION=" + TPM_FION;
            ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
        }
    }
}
