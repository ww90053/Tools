using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TPPDDB.Helpers;
using TPPDDB.Models._3_ForLeave;

namespace TPPDDB._3_forleave
{
    public partial class C_OverTime_Detail_rpt : C_BasePage
    {
        C_OverTime_Detail_rpt_Query _query
        {
            get { return ViewState["query"] != null ? (C_OverTime_Detail_rpt_Query)ViewState["query"] : null; }
            set { ViewState["query"] = value; }
        }

        /// <summary>
        /// 初始化各頁面功能
        /// </summary>
        private void PageInitial()
        {
            //查詢機關選單
            string ddlAD_SQL = "SELECT RTRIM(MZ_KCHI) MZ_KCHI, RTRIM(MZ_KCODE) MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE='04' ";
            switch (_strGID)
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
            WebUIHelpers.DropDownList_DataBind(ddlAD_SQL, ddl_Search_EXAD, "MZ_KCHI", "MZ_KCODE", new WebUIHelpers.OtherListItem(0, new ListItem("請選擇", "")));
            ddl_Search_EXAD.SelectedValue = Session["ADPMZ_EXAD"].ToStringNullSafe();

            //查詢單位選單
            string ddlUNIT_SQL = string.Format(@"SELECT RTRIM(MZ_KCHI) MZ_KCHI,RTRIM(MZ_KCODE) MZ_KCODE FROM A_KTYPE 
                                                 WHERE MZ_KTYPE='25' AND MZ_KCODE IN (SELECT MZ_UNIT FROM A_UNIT_AD WHERE MZ_AD='{0}') ",
                                                 ddl_Search_EXAD.SelectedValue);
            switch (_strGID)
            {
                case "D": //一般使用者
                case "E": //所屬單位承辦人
                    ddlUNIT_SQL += string.Format(@"AND MZ_KCODE='{0}' ", Session["ADPMZ_EXUNIT"].ToStringNullSafe());
                    break;
            }
            WebUIHelpers.DropDownList_DataBind(ddlUNIT_SQL, ddl_Search_EXUNIT, "MZ_KCHI", "MZ_KCODE", new WebUIHelpers.OtherListItem(0, new ListItem("請選擇", "")));
            ddl_Search_EXUNIT.SelectedValue = Session["ADPMZ_EXUNIT"].ToStringNullSafe();

            txt_Search_ID.Text = Session["ADPMZ_ID"].ToStringNullSafe();
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            //一般權限檢查
            C.check_power();

            //取得群組權限，人事系統取得 MZ_POWER
            _strGID = strGID(Session["ADPMZ_ID"].ToStringNullSafe());

            //取得模組
            _TPM_FION = Request.QueryString["TPM_FION"].ToStringNullSafe();

            if (!IsPostBack)
            {
                PageInitial();
            }
        }
        /// <summary>
        /// 按鈕: 列印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Print_Click(object sender, EventArgs e)
        {
            //檢查必要欄位
            if (!string.IsNullOrEmpty(txt_DateS.Text) && !string.IsNullOrEmpty(txt_DateE.Text))
            {
                if (!(ForDateTime.Check_date(txt_DateS.Text) && ForDateTime.Check_date(txt_DateE.Text)))
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請檢查欲查詢之起迄日期');", true);
                    return;
                }

                if (!(ForDateTime.Check_date(txt_DateS.Text)))
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請檢查欲查詢之起始日期');", true);
                    return;
                }
            }
            if (string.IsNullOrEmpty(ddl_Search_EXAD.SelectedValue) && string.IsNullOrEmpty(ddl_Search_EXUNIT.SelectedValue)
                && string.IsNullOrEmpty(txt_Search_ID.Text) && string.IsNullOrEmpty(txt_Search_NAME.Text))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('機關、單位、身分證、姓名不可全空');", true);
                return;
            }

            _query = new C_OverTime_Detail_rpt_Query()
            {
                DateS = txt_DateS.Text,
                DateE = txt_DateE.Text,
                Search_EXAD = ddl_Search_EXAD.SelectedValue,
                Search_EXUNIT = ddl_Search_EXUNIT.SelectedValue,
                Search_ID = txt_Search_ID.Text,
                Search_NAME = txt_Search_NAME.Text,
                TPMFION = _TPM_FION
            };
            Session["PageQuery"] = _query;

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "go_print('C_rpt.aspx?fn=OverTimeDutyDetail');", true);
        }

        /// <summary>
        /// 下拉選單: 查詢單位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddl_Search_EXAD_SelectedIndexChanged(object sender, EventArgs e)
        {
            string ddlUNIT_SQL = string.Format(@"SELECT RTRIM(MZ_KCHI) MZ_KCHI,RTRIM(MZ_KCODE) MZ_KCODE FROM A_KTYPE 
                                                 WHERE MZ_KTYPE='25' AND MZ_KCODE IN (SELECT MZ_UNIT FROM A_UNIT_AD WHERE MZ_AD='{0}') ",
                                                 ddl_Search_EXAD.SelectedValue);

            WebUIHelpers.DropDownList_DataBind(ddlUNIT_SQL, ddl_Search_EXUNIT, "MZ_KCHI", "MZ_KCODE", new WebUIHelpers.OtherListItem(0, new ListItem("請選擇", "")));
        }
    }
}