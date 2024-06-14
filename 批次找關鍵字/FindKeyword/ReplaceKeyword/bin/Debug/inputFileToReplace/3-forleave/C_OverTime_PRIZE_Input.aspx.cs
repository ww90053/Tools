using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TPPDDB.Helpers;
using TPPDDB.Models._3_ForLeave;
using TPPDDB.Service;

namespace TPPDDB._3_forleave
{
    public partial class C_OverTime_PRIZE_Input : C_BasePage
    {
        C_OverTime_PRIZE_Input_Query _query
        {
            get { return ViewState["query"] != null ? (C_OverTime_PRIZE_Input_Query)ViewState["query"] : null; }
            set { ViewState["query"] = value; }
        }
        /// <summary>
        /// 嘉獎單位
        /// </summary>
        int onePrizeUnit = Convert.ToInt32(new CFService().GetOVERTIMECODE("CODE_VALUE", "OTPH", "", "嘉獎").Rows[0][0].ToStringNullSafe());
      //  int onePrizeUnit = 10;
        /// <summary>
        /// 初始化各頁面功能
        /// </summary>
        private void PageInitial(string strGID)
        {
            //查詢機關選單
            string ddlAD_SQL = "SELECT RTRIM(MZ_KCHI) MZ_KCHI, RTRIM(MZ_KCODE) MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE='04' ";
            switch (strGID)
            {
                case "A": //系統管理員
                    
                case "TPMIDISAdmin": //系統開發管理員
                   
                case "B": //局本部承辦人
                    //如果是「中和分局」進來抓 中和第一、中和第二、中和
                    if (Session["ADPMZ_EXAD"].ToStringNullSafe() == "382133600C")
                    {
                        ddlAD_SQL += "AND MZ_KCODE in ('382133400C','382133500C','382133600C') ";
                    }
                    else
                    {
                        ddlAD_SQL += "AND MZ_KCODE LIKE '38213%' ";
                    }
                    break;
                case "C": //分局、大隊、隊承辦人
                    ddlAD_SQL += string.Format("AND MZ_KCODE ='{0}' ", Session["ADPMZ_EXAD"].ToStringNullSafe());
                    break;
                case "D": //一般使用者
                    ddlAD_SQL += string.Format("AND MZ_KCODE ='{0}' ", Session["ADPMZ_EXAD"].ToStringNullSafe());
                    btn_Cancel.Visible = false;
                    break;
                case "E": //所屬單位承辦人
                    ddlAD_SQL += string.Format("AND MZ_KCODE ='{0}' ", Session["ADPMZ_EXAD"].ToStringNullSafe());
                    btn_Cancel.Visible = false;
                    break;
                default:
                    Response.Redirect("~/ErrorPage.aspx?erid=TPFXXX0001");
                    break;
            }
            WebUIHelpers.DropDownList_DataBind(ddlAD_SQL, ddl_Search_AD, "MZ_KCHI", "MZ_KCODE", new WebUIHelpers.OtherListItem(0, new ListItem("全部", "")));
            ddl_Search_AD.SelectedValue = Session["ADPMZ_EXAD"].ToStringNullSafe();

            //查詢單位選單
            string ddlUNIT_SQL = string.Format(@"SELECT RTRIM(MZ_KCHI) MZ_KCHI,RTRIM(MZ_KCODE) MZ_KCODE FROM A_KTYPE 
                                                 WHERE MZ_KTYPE='25' AND MZ_KCODE IN (SELECT MZ_UNIT FROM A_UNIT_AD WHERE MZ_AD='{0}') ",
                                                 ddl_Search_AD.SelectedValue);
            switch (strGID)
            {
                case "D": //一般使用者
                case "E": //所屬單位承辦人
                    ddlUNIT_SQL += string.Format(@"AND MZ_KCODE='{0}' ", Session["ADPMZ_EXUNIT"].ToStringNullSafe());
                    break;
            }
            WebUIHelpers.DropDownList_DataBind(ddlUNIT_SQL, ddl_Search_Unit, "MZ_KCHI", "MZ_KCODE", new WebUIHelpers.OtherListItem(0, new ListItem("全部", "")));
            ddl_Search_Unit.SelectedValue = Session["ADPMZ_EXUNIT"].ToStringNullSafe();
        }
        /// <summary>
        /// 初始化Query區塊
        /// </summary>
        /// <param name="query"></param>
        private void PageQueryInitial(C_OverTime_PRIZE_Input_Query query, string strGID)
        {
            switch (strGID)
            {
                case "C": //分局、大隊、隊承辦人
                    ddl_Search_AD.Enabled = false;
                    break;
                case "E": //所屬單位承辦人
                    ddl_Search_AD.Enabled = false;
                    ddl_Search_Unit.Enabled = false;
                    break;
                case "D": //一般使用者
                    ddl_Search_AD.Enabled = false;
                    ddl_Search_Unit.Enabled = false;
                    txt_MZ_ID.Enabled = false;
                    btn_Cancel.Enabled = false;
                    break;
            }

            txt_Year.Text = (DateTime.Now.Year - 1911).ToStringNullSafe();
            txt_MZ_ID.Text = query.MZ_ID;
        }
        /// <summary>
        /// 取得敘獎資料
        /// </summary>
        /// <param name="query"></param>
        private void GetPRIZE_Bind_GridView(C_OverTime_PRIZE_Input_Query query)
        {
            DataTable gvDt = CFService.GetOverTimeBaseByPrize(query);
            if (string.IsNullOrEmpty(query.MZ_ID))
            {
                //查詢機關單位全部資料
                gv_resultat.Visible = false;
                gv_manage_resultat.Visible = true;

                gv_manage_resultat.DataSource = gvDt;
                gv_manage_resultat.DataBind();

                pnl_apply.Visible = false;
            }
            else
            {
                //查詢單人各月份資料
                gv_resultat.Visible = true;
                gv_manage_resultat.Visible = false;

                gv_resultat.DataSource = gvDt;
                gv_resultat.DataBind();

                //取得已申請數量
                if (gvDt != null && gvDt.Rows.Count > 0)
                {
                    pnl_apply.Visible = true;
                    int c = CFService.GetApplyPrizeCount(query);
                    txt_apply_prize.Text = c.ToString();

                    //已申請不能修改，除透過管理者退回
                    txt_apply_prize.Enabled = c > 0 ? false : true;
                }
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            //一般權限檢查
            C.check_power();
            //取得群組權限，人事系統取得 MZ_POWER
            _strGID = o_a_Function.strGID(Session["ADPMZ_ID"].ToStringNullSafe());
            //取得模組
            _TPM_FION = Request.QueryString["TPM_FION"].ToStringNullSafe();

            if (!IsPostBack)
            {
                _query = new C_OverTime_PRIZE_Input_Query()
                {
                    TPMFION = _TPM_FION,
                    MZ_ID = Session["ADPMZ_ID"].ToStringNullSafe()
                };

                PageInitial(_strGID);
                //取得頁面標題
                PageTitle = new CommonService().GetPageTitleName(_TPM_FION);
                //建立查詢區塊
                PageQueryInitial(_query, _strGID);
            }
        }
        /// <summary>
        /// 按鈕: 查詢已加班資料
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Search_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt_Year.Text))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('查詢年度未填!');", true);
                return;
            }

            _query = new C_OverTime_PRIZE_Input_Query()
            {
                Year = txt_Year.Text,
                Year_Interval = rbl_Year_Interval.SelectedValue,
                MZ_ID = txt_MZ_ID.Text,
                Search_AD = ddl_Search_AD.SelectedValue,
                Search_Unit = ddl_Search_Unit.SelectedValue,
                TPMFION = _TPM_FION
            };

            //查詢資料
            GetPRIZE_Bind_GridView(_query);
            UpdatePanel1.Update();
        }
        /// <summary>
        /// 按鈕: 申請敘獎
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Apply_Click(object sender, EventArgs e)
        {
            if (!txt_apply_prize.Enabled)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('已申請，不能重複申請!');", true);
                return;
            }
            if (string.IsNullOrEmpty(txt_Year.Text) || string.IsNullOrEmpty(txt_MZ_ID.Text))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('申請年度、身分證號未填!');", true);
                return;
            }
            if (txt_apply_prize.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('請先查詢加班剩餘時數!');", true);
                return;
            }
            if (Convert.ToInt32(txt_apply_prize.Text) < 1)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('請填寫嘉獎數量!');", true);
                return;
            }

            C_OVERTIME_PRIZE_Model addModel = new C_OVERTIME_PRIZE_Model()
            {
                MZ_ID = txt_MZ_ID.Text,
                APPLY_YEAR = txt_Year.Text,
                APPLY_INTERVAL = rbl_Year_Interval.SelectedValue,
                PRIZE_AMOUNT = Convert.ToInt32(txt_apply_prize.Text)
            };

            //新增敘獎資料及更新加班剩餘時數
            if (CFService.C_OVERTIME_PRIZE_Save(new List<C_OVERTIME_PRIZE_Model>() { addModel }, _query.TPMFION))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('申請完成。');", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('新增敘獎資料錯誤!');", true);
            }

            //查詢資料
            GetPRIZE_Bind_GridView(_query);
            UpdatePanel1.Update();
        }
        /// <summary>
        /// 按鈕: 退回申請敘獎
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Cancel_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt_Year.Text) || string.IsNullOrEmpty(txt_MZ_ID.Text))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('退回申請年度、身分證號未填!');", true);
                return;
            }
            if (txt_apply_prize.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('請先查詢加班敘獎時數!');", true);
                return;
            }
            if (Convert.ToInt32(txt_apply_prize.Text) < 1)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('未申請敘獎無法退回!');", true);
                return;
            }

            C_OVERTIME_PRIZE_Model delModel = new C_OVERTIME_PRIZE_Model()
            {
                MZ_ID = txt_MZ_ID.Text,
                APPLY_YEAR = txt_Year.Text,
                APPLY_INTERVAL = rbl_Year_Interval.SelectedValue
            };

            //刪除敘獎資料及更新加班時數
            if (CFService.C_OVERTIME_PRIZE_Delete(new List<C_OVERTIME_PRIZE_Model>() { delModel }, _query.TPMFION))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('退回申請完成。');", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('退回申請錯誤!');", true);
            }

            //查詢資料
            GetPRIZE_Bind_GridView(_query);
            UpdatePanel1.Update();
        }


        /// <summary>
        /// 下拉選單: 查詢單位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddl_Search_AD_SelectedIndexChanged(object sender, EventArgs e)
        {
            string ddlUNIT_SQL = string.Format(@"SELECT RTRIM(MZ_KCHI) MZ_KCHI,RTRIM(MZ_KCODE) MZ_KCODE FROM A_KTYPE 
                                                 WHERE MZ_KTYPE='25' AND MZ_KCODE IN (SELECT MZ_UNIT FROM A_UNIT_AD WHERE MZ_AD='{0}') ",
                                     ddl_Search_AD.SelectedValue);

            WebUIHelpers.DropDownList_DataBind(ddlUNIT_SQL, ddl_Search_Unit, "MZ_KCHI", "MZ_KCODE", new WebUIHelpers.OtherListItem(0, new ListItem("全部", "")));
        }


        /// <summary>
        /// 輸入框: 申請時數輸入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txt_apply_prize_TextChanged(object sender, EventArgs e)
        {
            //檢查是否足時可申請
            int surplus_Sum = 0;
            for (int i = 0; i < gv_resultat.Rows.Count; i++)
            {
                surplus_Sum += Convert.ToInt32(gv_resultat.Rows[i].Cells[1].Text) + Convert.ToInt32(gv_resultat.Rows[i].Cells[2].Text);
            }

            //40小時/嘉獎
            if (Convert.ToInt32(txt_apply_prize.Text) > surplus_Sum)
            {
                txt_apply_prize.Text = "0";
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('可申請時數不足!');", true);
            }
        }
    }
}