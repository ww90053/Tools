using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using TPPDDB.Helpers;
using TPPDDB.Models._3_ForLeave;
using TPPDDB.Service;

namespace TPPDDB._3_forleave
{
    public partial class C_OverTime_PAY_Check : C_BasePage
    {
        C_OverTime_PAY_Check_Query _query
        {
            get { return ViewState["query"] != null ? (C_OverTime_PAY_Check_Query)ViewState["query"] : null; }
            set { ViewState["query"] = value; }
        }

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
                case "E": //所屬單位承辦人
                    ddlAD_SQL += string.Format("AND MZ_KCODE ='{0}' ", Session["ADPMZ_EXAD"].ToStringNullSafe());
                    break;
                case "D": //一般使用者
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
        private void PageQueryInitial(C_OverTime_PAY_Check_Query query, string strGID)
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
            }

            txt_Year.Text = ForDateTime.RCDateToTWDate(DateTime.Now.ToString("yyyy/MM/dd"), "yyyMM");
        }
        /// <summary>
        /// 取得申請加班費資料
        /// </summary>
        /// <param name="query"></param>
        private void GetPAY_Bind_GridView(C_OverTime_PAY_Check_Query query)
        {
            gv_resultat.Visible = true;
            gv_resultat.DataSource = CFService.GetOverTimeBaseByPay(query);
            gv_resultat.DataBind();
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
                _query = new C_OverTime_PAY_Check_Query()
                {
                    TPMFION = _TPM_FION
                };

                PageInitial(_strGID);
                //取得頁面標題
                PageTitle = new CommonService().GetPageTitleName(_TPM_FION);
                //建立查詢區塊
                PageQueryInitial(_query, _strGID);
            }
        }
        /// <summary>
        /// 按鈕: 查詢加班費申請
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Search_Click(object sender, EventArgs e)
        {
            _query = new C_OverTime_PAY_Check_Query()
            {
                Search_AD = ddl_Search_AD.SelectedValue,
                Search_UNIT = ddl_Search_Unit.SelectedValue,
                Search_YM = txt_Year.Text,
                Search_ID = txt_MZ_ID.Text,
                Search_CHK = rbl_PAY_CHK.SelectedValue,
                TPMFION = _TPM_FION
            };

            //查詢資料
            GetPAY_Bind_GridView(_query);
            UpdatePanel1.Update();
        }
        /// <summary>
        /// 按鈕: 核准加班費申請
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Check_Click(object sender, EventArgs e)
        {
            if (gv_resultat.Visible && gv_resultat.Rows.Count > 0)
            {
                for (int i = 0; i < gv_resultat.Rows.Count; i++)
                {
                    CheckBox ckb_PAY_CHK = (CheckBox)gv_resultat.Rows[i].FindControl("ckb_PAY_CHK");
                    if (ckb_PAY_CHK.Checked)
                    {
                        //更新加班資料
                        C_OVERTIME_BASE_Model upModel = new C_OVERTIME_BASE_Model()
                        {
                            MZ_ID = gv_resultat.DataKeys[i]["MZ_ID"].ToStringNullSafe(),
                            OVER_DAY = gv_resultat.DataKeys[i]["OVER_DAY"].ToStringNullSafe(),
                            PAY_CHK = "Y",
                            PAY_CHK_ID = Session["ADPMZ_ID"].ToStringNullSafe(),
                            PAY_CHK_DATE = DateTime.Now
                        };

                        if (!CFService.C_OVERTIME_BASE_Save(new List<C_OVERTIME_BASE_Model>() { upModel }, _query.TPMFION))
                        {
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('加班資料更新異常!');", true);
                            break;
                        }
                    }
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('無申請資料!');", true);
            }

            //更新查詢頁面
            GetPAY_Bind_GridView(_query);
            UpdatePanel1.Update();
        }
        /// <summary>
        /// 按鈕: 退回加班費申請
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Cancel_Click(object sender, EventArgs e)
        {
            if (gv_resultat.Visible && gv_resultat.Rows.Count > 0)
            {
                for (int i = 0; i < gv_resultat.Rows.Count; i++)
                {
                    CheckBox ckb_PAY_CHK = (CheckBox)gv_resultat.Rows[i].FindControl("ckb_PAY_CHK");
                    if (ckb_PAY_CHK.Checked)
                    {
                        //更新加班資料
                        C_OVERTIME_BASE_Model upModel = new C_OVERTIME_BASE_Model()
                        {
                            MZ_ID = gv_resultat.DataKeys[i]["MZ_ID"].ToStringNullSafe(),
                            OVER_DAY = gv_resultat.DataKeys[i]["OVER_DAY"].ToStringNullSafe(),
                            PAY_CHK = "N",
                            PAY_HOUR = 0,
                            SHIFT_HOUR = 0,
                            PAY_SUM = 0,
                            PAY_CHK_ID = Session["ADPMZ_ID"].ToStringNullSafe(),
                            PAY_CHK_DATE = DateTime.Now
                        };

                        if (!CFService.C_OVERTIME_BASE_Save(new List<C_OVERTIME_BASE_Model>() { upModel }, _query.TPMFION) || !CFService.SynchronizeOverTime(upModel, _query.TPMFION))
                        {
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", string.Format("alert('ID:{0}，加班資料更新異常!');", upModel.MZ_ID), true);
                            break;
                        }
                    }
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('無資料退回!');", true);
            }

            //更新查詢頁面
            GetPAY_Bind_GridView(_query);
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
        /// GridView: Row建立觸發
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gv_resultat_RowCreated(object sender, GridViewRowEventArgs e)
        {
            GridView gv = sender as GridView;

            switch (e.Row.RowType)
            {
                case DataControlRowType.Header:
                    break;
                case DataControlRowType.Footer:
                    break;
                case DataControlRowType.DataRow:
                    break;
                case DataControlRowType.Separator:
                    break;
                case DataControlRowType.Pager:
                    break;
                case DataControlRowType.EmptyDataRow:
                    break;
                default:
                    break;
            }
        }
    }
}