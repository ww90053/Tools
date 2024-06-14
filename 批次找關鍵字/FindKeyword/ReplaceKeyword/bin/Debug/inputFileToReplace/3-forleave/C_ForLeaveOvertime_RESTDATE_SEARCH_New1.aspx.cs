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
    public partial class C_ForLeaveOvertime_RESTDATE_SEARCH_New1 : C_BasePage
    {
        C_ForLeaveOvertime_RESTDATE_SEARCH_Query _query
        {
            get { return ViewState["query"] != null ? (C_ForLeaveOvertime_RESTDATE_SEARCH_Query)ViewState["query"] : null; }
            set { ViewState["query"] = value; }
        }

        public string _MZ_IDATE1 { get; set; } = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            //取得模組
            _TPM_FION = Request.QueryString["TPM_FION"].ToStringNullSafe();

            string Param = Request.QueryString["Param"].ToStringNullSafe();
            EncryptParam ep = new EncryptParam();
            ep.Import(Param);

            if (!IsPostBack)
            {
                //取得請假日
                this._MZ_IDATE1 = ep.dataDictionary["MZ_IDATE1"].ToStringNullSafe();

                //根據補休申請日,計算"最早加班日",需要考慮到新舊制的問題
                string OVERDAY_LIMIT = CFService.Caculate_OVERDAY_LIMIT(this._MZ_IDATE1);

                _query = new C_ForLeaveOvertime_RESTDATE_SEARCH_Query()
                {
                    MZ_ID = ep.dataDictionary["ForLaeveBasic_RESTDATE_ID"].ToStringNullSafe(),
                    KeepDay = OVERDAY_LIMIT,
                    MZ_IDATE1 = this._MZ_IDATE1,
                    //預計申請補休的小時數
                    ApplyTime = Convert.ToInt32(ep.dataDictionary["ForLeaveBasic_CID1"].ToString()),
                    MZ_CODE = Request["mz_code"].ToString(),
                    TPMFION = _TPM_FION
                };

                int atime = Convert.ToInt32(ep.dataDictionary["ForLeaveBasic_CID1"].ToString());
                gv_resultat.DataSource = CFService.GetOverTimeBaseForRESTDATE_SEARCH(_query);
                gv_resultat.DataBind();
                for (int i = 0; i < gv_resultat.Rows.Count; i++)
                {
                    CheckBox cb = (CheckBox)gv_resultat.Rows[i].FindControl("CheckBox1");

                    TextBox tmp1 = (TextBox)gv_resultat.Rows[i].FindControl("txt_SURPLUS_HOUR");
                    string tmpval = tmp1.Text.Trim();
                    if (atime > 0)
                    {
                        if (atime >= int.Parse(tmpval))
                        {
                            cb.Checked = true;
                            atime -= int.Parse(tmpval);
                        }
                        else
                        {
                            cb.Checked = true;
                            atime = 0;
                        }
                    }
                }
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
            dt.Columns.Add("MZ_ID", typeof(string));
            List<int> tmp = new List<int>();
            int totaltime = 0;  //總勾選時數
            for (int i = 0; i < gv_resultat.Rows.Count; i++)
            {
                CheckBox cb = (CheckBox)gv_resultat.Rows[i].FindControl("CheckBox1");
                if (cb.Checked)
                {
                    TextBox tmp1 = (TextBox)gv_resultat.Rows[i].FindControl("txt_OVER_TIME");
                    //加班日期字串
                    string tmpval = tmp1.Text.Trim();

                    #region 取得加班資料
                    //20210326 - 原來用的 FLOOR 負數 時有問題 (如 FLOOR(-1.23) 會變 -2)
                    //string strSQL = string.Format(@"Select OVER_DAY, SURPLUS_HOUR From 
                    //                                (Select substr(OVER_DAY,0,5) OVER_DAY, floor(SUM(SURPLUS_TOTAL / 60)) SURPLUS_HOUR From C_OVERTIME_BASE 
                    //                                    Where MZ_ID='{0}' 
                    //                                    Group by substr(OVER_DAY,0,5)) 
                    //                                Where SURPLUS_HOUR > 0 
                    //                                Order by OVER_DAY ",
                    //                                _query.MZ_ID);
                    //string strSQL = string.Format(@"Select OVER_DAY, SURPLUS_TOTAL as SURPLUS_HOUR From 
                    //                                C_OVERTIME_BASE 
                    //                                Where MZ_ID='{0}' and OVER_DAY = '{1}'  and
                    //                                SURPLUS_TOTAL > 0 
                    //                                Order by OVER_DAY ",
                    //                                _query.MZ_ID, tmpval);
                    //DataTable tmp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "C_OVERTIME_BASE");
                    //for (int y = 0; y < tmp.Rows.Count; y++)
                    //{
                    #endregion
                    //取得當月加班資料

                    //根據補休申請日,計算"最早加班日",需要考慮到新舊制的問題
                    string OVERDAY_LIMIT = CFService.Caculate_OVERDAY_LIMIT(this._MZ_IDATE1);
                    C_OverTime_Base_Input_Query model = new C_OverTime_Base_Input_Query()
                    {
                        MZ_ID = _query.MZ_ID,
                        OVER_DAY = tmpval,
                        //   SearchYM = tmp.Rows[y]["OVER_DAY"].ToStringNullSafe(),
                        TPMFION = _query.TPMFION
                        ,
                        OVERDAY_LIMIT = OVERDAY_LIMIT
                    };
                    //取得加班資料
                    DataTable dtMonth = CFService.GetOverTimeBase_data(model);
                    for (int j = dtMonth.Rows.Count - 1; j >= 0; j--)
                    {
                        if (_query.ApplyTime == 0) { break; }

                        int surplusTotal = Convert.ToInt32(dtMonth.Rows[j]["SURPLUS_TOTAL"]);
                        totaltime += surplusTotal;
                        //如果那天有加班可以申請補休
                        if (surplusTotal > 0)
                        {
                            DataRow dr = dt.NewRow();
                            dr["OVER_DAY"] = dtMonth.Rows[j]["OVER_DAY"].ToStringNullSafe();
                            //這邊會將要申請的時數寫入,但是此舉沒有什麼意義,後面也會被刷掉,給0亦可
                            dr["MZ_RESTHOUR"] = _query.ApplyTime;
                            dr["OVER_TYPE"] = dtMonth.Rows[j]["OVERTIME_TYPE"].ToStringNullSafe();
                            //Mark增加ID
                            dr["MZ_ID"] = _query.MZ_ID;
                            #region
                            //  dr["OVER_TYPE"] = dtMonth.Rows[j]["OVERTIME_TYPE"].ToStringNullSafe();
                            //if (_query.ApplyTime < surplusTotal)
                            //{
                            //    dr["MZ_RESTHOUR"] = _query.ApplyTime;
                            //    _query.ApplyTime = 0;
                            //}
                            //else
                            //{
                            //    dr["MZ_RESTHOUR"] = surplusTotal;
                            //    _query.ApplyTime -= surplusTotal;
                            //}
                            #endregion
                            dt.Rows.Add(dr);
                        }
                    }

                    //已無需申請時數，直接跳出
                    if (_query.ApplyTime == 0)
                    { break; }
                    //}
                }
            }



            //最舊日期 至 最新日期資料
            Session["MZ_RESTHOUR_DT"] = dt;
            //特殊標記Session["CHECK"]: 
            //true - 代表Session["MZ_RESTHOUR_DT"]裡面的資料,是已經經過check_15_Hour()計算過產生的
            //       dr["MZ_RESTHOUR"]裡面紀錄的資訊是:每個加班日預計申請補休時間,單位為分鐘數
            //false - 代表Session["MZ_RESTHOUR_DT"]裡面的資料,是從\3-forleave\C_ForLeaveOvertime_RESTDATE_SEARCH_New1.aspx.cs產生的
            //      dr["MZ_RESTHOUR"]裡面紀錄的資訊是:本次預計申請的加班時數,單位為小時數,所有資料的數字都長一樣
            //上述兩者的定義落差還蠻大的
            Session["CHECK"] = "false";

            //觸發自動填寫請假事由

            //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click",
            //    "alert('" + Session["ForLeaveBasic_BT"] + "');", true);

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