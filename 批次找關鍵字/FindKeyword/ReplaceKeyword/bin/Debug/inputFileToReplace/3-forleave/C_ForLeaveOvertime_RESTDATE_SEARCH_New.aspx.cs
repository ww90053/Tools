using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using TPPDDB.Models._3_ForLeave;
using TPPDDB.Helpers;

namespace TPPDDB._3_forleave
{
    //TODO SKY 正式上線時替換
    public partial class C_ForLeaveOvertime_RESTDATE_SEARCH_New : C_BasePage
    {
        C_ForLeaveOvertime_RESTDATE_SEARCH_Query _query
        {
            get { return ViewState["query"] != null ? (C_ForLeaveOvertime_RESTDATE_SEARCH_Query)ViewState["query"] : null; }
            set { ViewState["query"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //取得模組
            _TPM_FION = Request.QueryString["TPM_FION"].ToStringNullSafe();

            if (!IsPostBack)
            {

                //設定保留日期365天
                DateTime date365 = DateTime.Now.AddDays(-365);
                string datestring365 = (date365.Year - 1911).ToString().PadLeft(3, '0') + date365.Month.ToString().PadLeft(2, '0') + date365.Day.ToString().PadLeft(2, '0');

                _query = new C_ForLeaveOvertime_RESTDATE_SEARCH_Query()
                {
                    MZ_ID = Session["ForLaeveBasic_RESTDATE_ID"].ToStringNullSafe(),
                    KeepDay = datestring365,
                    ApplyTime = Convert.ToInt32(Session["ForLeaveBasic_CID1"].ToString()) * 60,
                    TPMFION = _TPM_FION
                };

                gv_resultat.DataSource = CFService.GetOverTimeBaseForRESTDATE_SEARCH(_query);
                gv_resultat.DataBind();
            }
        }
        /// <summary>
        /// 按鈕: 確定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Ok_Click(object sender, EventArgs e)
        {
            if (_query.ApplyTime == 0)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('請先填寫請假時數。')", true);
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("OVER_DAY", typeof(string));
            dt.Columns.Add("MZ_RESTHOUR", typeof(string));
            dt.Columns.Add("OVER_TYPE", typeof(string));

            for (int i = 0; i < gv_resultat.Rows.Count; i++)
            {
                CheckBox cb = (CheckBox)gv_resultat.Rows[i].FindControl("CheckBox1");
                if (cb.Checked)
                {
                    //取得加班資料
                    //20210326 - 原來用的 FLOOR 負數 時有問題 (如 FLOOR(-1.23) 會變 -2)
                    string strSQL = string.Format(@"Select OVER_DAY, SURPLUS_HOUR From 
                                                    (Select substr(OVER_DAY,0,5) OVER_DAY, floor(SUM(SURPLUS_TOTAL / 60)) SURPLUS_HOUR From C_OVERTIME_BASE 
                                                        Where MZ_ID='{0}' 
                                                        Group by substr(OVER_DAY,0,5)) 
                                                    Where SURPLUS_HOUR > 0 
                                                    Order by OVER_DAY ",
                                                    _query.MZ_ID);
                    DataTable tmp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "C_OVERTIME_BASE");
                    for (int y = 0; y < tmp.Rows.Count; y++)
                    {
                        //取得當月加班資料
                        C_OverTime_Base_Input_Query model = new C_OverTime_Base_Input_Query()
                        {
                            MZ_ID = _query.MZ_ID,
                            SearchYM = tmp.Rows[y]["OVER_DAY"].ToStringNullSafe(),
                            TPMFION = _query.TPMFION
                        };
                        DataTable dtMonth = CFService.GetOverTimeBase_data(model);
                        for (int j = dtMonth.Rows.Count - 1; j >= 0; j--)
                        {
                            if (_query.ApplyTime == 0) { break; }

                            int surplusTotal = Convert.ToInt32(dtMonth.Rows[j]["SURPLUS_TOTAL"]);
                            if (surplusTotal > 0)
                            {
                                DataRow dr = dt.NewRow();
                                dr["OVER_DAY"] = dtMonth.Rows[j]["OVER_DAY"].ToStringNullSafe();
                                dr["OVER_TYPE"] = dtMonth.Rows[j]["OVERTIME_TYPE"].ToStringNullSafe();
                                if (_query.ApplyTime < surplusTotal)
                                {
                                    dr["MZ_RESTHOUR"] = _query.ApplyTime;
                                    _query.ApplyTime = 0;
                                }
                                else
                                {
                                    dr["MZ_RESTHOUR"] = surplusTotal;
                                    _query.ApplyTime -= surplusTotal;
                                }
                                dt.Rows.Add(dr);
                            }
                        }

                        //已無需申請時數，直接跳出
                        if (_query.ApplyTime == 0) { break; }
                    }
                }
            }

            //最舊日期 至 最新日期資料
            Session["MZ_RESTHOUR_DT"] = dt;

            //觸發自動填寫請假事由
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click",
                "window.opener.document.getElementById('" + Session["ForLeaveBasic_BT"] + "').click();window.close();", true);
        }
        /// <summary>
        /// 按鈕: 離開
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Leave_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "window.close();", true);
        }
    }
}