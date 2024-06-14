using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using TPPDDB.Helpers;
using TPPDDB.Models._3_ForLeave;

namespace TPPDDB._3_forleave
{
    //TODO SKY 正式上線時刪除
    public partial class C_OvertimeInsideDetail_rpt_New : C_BasePage
    {
        C_OvertimeInsideDetail_rpt_Query _query
        {
            get { return ViewState["query"] != null ? (C_OvertimeInsideDetail_rpt_Query)ViewState["query"] : null; }
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
                    ddlAD_SQL += string.Format("AND MZ_KCODE ='{0}' ", Session["ADPMZ_EXAD"].ToStringNullSafe());
                    break;
                case "E": //所屬單位承辦人
                    ddlAD_SQL += string.Format("AND MZ_KCODE ='{0}' ", Session["ADPMZ_EXAD"].ToStringNullSafe());
                    break;
                case "D": //一般使用者
                    ddlAD_SQL += string.Format("AND MZ_KCODE ='{0}' ", Session["ADPMZ_EXAD"].ToStringNullSafe());
                    break;
                default:
                  //  Response.Redirect("~/ErrorPage.aspx?erid=TPFXXX0001");
                    break;
            }
            WebUIHelpers.DropDownList_DataBind(ddlAD_SQL, DropDownList_AD, "MZ_KCHI", "MZ_KCODE", new WebUIHelpers.OtherListItem(0, new ListItem("全部", "")));
            DropDownList_AD.SelectedValue = Session["ADPMZ_EXAD"].ToStringNullSafe();

            //查詢單位選單
            string ddlUNIT_SQL = string.Format(@"SELECT RTRIM(MZ_KCHI) MZ_KCHI,RTRIM(MZ_KCODE) MZ_KCODE FROM A_KTYPE 
                                                 WHERE MZ_KTYPE='25' AND MZ_KCODE IN (SELECT MZ_UNIT FROM A_UNIT_AD WHERE MZ_AD='{0}') ",
                                                 DropDownList_AD.SelectedValue);
            switch (strGID)
            {
                case "D": //一般使用者
                case "E": //所屬單位承辦人
                    ddlUNIT_SQL += string.Format(@"AND MZ_KCODE='{0}' ", Session["ADPMZ_EXUNIT"].ToStringNullSafe());
                    break;
            }
            WebUIHelpers.DropDownList_DataBind(ddlUNIT_SQL, DropDownList_UNIT, "MZ_KCHI", "MZ_KCODE", new WebUIHelpers.OtherListItem(0, new ListItem("全部", "")));
            DropDownList_UNIT.SelectedValue = Session["ADPMZ_EXUNIT"].ToStringNullSafe();
        }
        /// <summary>
        /// 初始化Query區塊
        /// </summary>
        /// <param name="query"></param>
        private void PageQueryInitial(C_OvertimeInsideDetail_rpt_Query query, string strGID)
        {
            TextBox_MZ_ID.Text = query.Search_ID;

            switch (strGID)
            {
                case "D":
                    DropDownList_UNIT.Enabled = false;
                    TextBox_MZ_ID.Enabled = false;
                    break;
                case "E":
                    //如果是非中和分局鎖定機關選單
                    if (Session["ADPMZ_EXAD"].ToString() != "382133600C")
                    {
                        DropDownList_AD.Enabled = false;
                    }
                    DropDownList_UNIT.Enabled = false;
                    break;
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
                C.set_Panel_EnterToTAB(ref this.Panel1);

                _query = new C_OvertimeInsideDetail_rpt_Query()
                {
                    Search_ID = Session["ADPMZ_ID"].ToStringNullSafe()
                };

                PageInitial(_strGID);
                PageQueryInitial(_query, _strGID);
            }
        }
        /// <summary>
        /// 按鈕: 確定，產生加班費明細表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btPrint_Click(object sender, EventArgs e)
        {
            Session["RPT_C_IDNO"] = TextBox_MZ_ID.Text.Trim();
            Session["RPT_C_DATE"] = TextBox_MZ_DATE.Text.Trim();

            string tmp_url = string.Format("C_rpt.aspx?fn=OvertimeInsideDetail_New&TPM_FION={0}&AD={1}&UNIT={2}",
                _TPM_FION, DropDownList_AD.SelectedValue, DropDownList_UNIT.SelectedValue);

            ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
        }
        /// <summary>
        /// 按鈕: 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btCancel_Click(object sender, EventArgs e)
        {
            TextBox_MZ_DATE.Text = string.Empty;
        }


        /// <summary>
        /// 下拉選單: 查詢單位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DropDownList_AD_SelectedIndexChanged(object sender, EventArgs e)
        {
            string ddlUNIT_SQL = string.Format(@"SELECT RTRIM(MZ_KCHI) MZ_KCHI,RTRIM(MZ_KCODE) MZ_KCODE FROM A_KTYPE 
                                                 WHERE MZ_KTYPE='25' AND MZ_KCODE IN (SELECT MZ_UNIT FROM A_UNIT_AD WHERE MZ_AD='{0}') ",
                                                 DropDownList_AD.SelectedValue);

            WebUIHelpers.DropDownList_DataBind(ddlUNIT_SQL, DropDownList_UNIT, "MZ_KCHI", "MZ_KCODE", new WebUIHelpers.OtherListItem(0, new ListItem("全部", "")));
        }
    }
}