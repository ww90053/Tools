using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TPPDDB.Helpers;
using TPPDDB.Models._3_ForLeave;

namespace TPPDDB._3_forleave
{
    public partial class C_OverTime_EXCARD_Input : C_BasePage
    {
        C_OverTime_EXCARD_Input_Query _query
        {
            get { return ViewState["query"] != null ? (C_OverTime_EXCARD_Input_Query)ViewState["query"] : null; }
            set { ViewState["query"] = value; }
        }

        /// <summary>
        /// 初始化各頁面功能
        /// </summary>
        private void PageInitial()
        {
            //查詢機關選單
            string ddlAD_SQL = "SELECT RTRIM(MZ_KCHI) MZ_KCHI, RTRIM(MZ_KCODE) MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE='04' ";
            //如果是「中和分局」進來抓 中和第一、中和第二、中和
            if (Session["ADPMZ_EXAD"].ToStringNullSafe() == "382133600C")
            {
                ddlAD_SQL += "AND MZ_KCODE in ('382133400C','382133500C','382133600C') ";
            }
            else
            {
                ddlAD_SQL += "AND MZ_KCODE LIKE '38213%' ";
            }
            WebUIHelpers.DropDownList_DataBind(ddlAD_SQL, ddl_Search_MZ_EXAD, "MZ_KCHI", "MZ_KCODE", null);
            ddl_Search_MZ_EXAD.SelectedValue = Session["ADPMZ_EXAD"].ToStringNullSafe();

            //查詢單位選單
            string ddlUNIT_SQL = string.Format(@"SELECT RTRIM(MZ_KCHI) MZ_KCHI,RTRIM(MZ_KCODE) MZ_KCODE FROM A_KTYPE 
                                                 WHERE MZ_KTYPE='25' AND MZ_KCODE IN (SELECT MZ_UNIT FROM A_UNIT_AD WHERE MZ_AD='{0}') ",
                                                 ddl_Search_MZ_EXAD.SelectedValue);
            WebUIHelpers.DropDownList_DataBind(ddlUNIT_SQL, ddl_Search_MZ_EXUNIT, "MZ_KCHI", "MZ_KCODE", new WebUIHelpers.OtherListItem(0, new ListItem("全部", "")));
            ddl_Search_MZ_EXUNIT.SelectedValue = Session["ADPMZ_EXUNIT"].ToStringNullSafe();

            PageQueryInitial();
        }
        /// <summary>
        /// 初始化Query區塊
        /// </summary>
        private void PageQueryInitial()
        {
            txt_MZ_ID.BackColor = Color.White;
            txt_MZ_ID.Text = string.Empty;
            txt_MZ_NAME.Text = string.Empty;
            lbl_MZ_EXAD.Text = string.Empty;
            lbl_MZ_EXUNIT.Text = string.Empty;
            lbl_OCCC.Text = string.Empty;
            rbl_IS_MANAGER.SelectedIndex = -1;
        }
        /// <summary>
        /// 取得人員基本資料
        /// </summary>
        /// <param name="query"></param>
        private void GetPersonnel(C_OverTime_EXCARD_Input_Query query)
        {
            DataTable dt = CFService.GetPersonnelData(query.MZ_ID, query.TPMFION);

            if (dt != null && dt.Rows.Count > 0)
            {
                txt_MZ_NAME.Text = dt.Rows[0]["MZ_NAME"].ToStringNullSafe();
                lbl_MZ_EXAD.Text = dt.Rows[0]["MZ_EXAD_CH"].ToStringNullSafe();
                lbl_MZ_EXUNIT.Text = dt.Rows[0]["MZ_EXUNIT_CH"].ToStringNullSafe();
                lbl_OCCC.Text = dt.Rows[0]["MZ_OCCC_CH"].ToStringNullSafe();
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('無法取得人員基本資料');", true);
            }
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
                _query = new C_OverTime_EXCARD_Input_Query()
                {
                    TPMFION = _TPM_FION
                };

                C.set_Panel_EnterToTAB(ref pnl_Query);
                //關閉Query區域操作
                C.controlEnable(ref pnl_Query, false);

                //初始化查詢區塊
                PageInitial();
            }
        }
        /// <summary>
        /// 按鈕: 開啟查詢
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Search_Click(object sender, EventArgs e)
        {
            ddl_Search_MZ_EXAD.SelectedValue = Session["ADPMZ_EXAD"].ToStringNullSafe();
            mpe_Search.Show();
        }
        /// <summary>
        /// 按鈕: 新增
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Insert_Click(object sender, EventArgs e)
        {
            _query.OperationMode = OperationMode.INSERT;
            PageQueryInitial();

            btn_Insert.Enabled = false;
            btn_Update.Enabled = false;
            btn_Cancel.Enabled = true;
            btn_OK.Enabled = true;
            //開啟Query區域操作
            C.controlEnable(ref pnl_Query, true);
        }
        /// <summary>
        /// 按鈕: 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Update_Click(object sender, EventArgs e)
        {
            _query.OperationMode = OperationMode.UPDATE;

            btn_Delete.Enabled = true;
            btn_Insert.Enabled = false;
            btn_Update.Enabled = false;
            btn_Cancel.Enabled = true;
            btn_OK.Enabled = true;
            //開啟Query區域操作
            C.controlEnable(ref pnl_Query, true);
        }
        /// <summary>
        /// 按鈕: 確定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_OK_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 按鈕: 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Cancel_Click(object sender, EventArgs e)
        {
            PageQueryInitial();

            btn_Insert.Enabled = true;
            btn_OK.Enabled = false;
            btn_Cancel.Enabled = false;

            //關閉Query區域操作
            C.controlEnable(ref pnl_Query, false);
        }
        /// <summary>
        /// 按鈕: 刪除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Delete_Click(object sender, EventArgs e)
        {
            if (gv_resultat.SelectedIndex != -1)
            {
                C_OVERTIME_EXCARD_Model model = new C_OVERTIME_EXCARD_Model()
                {
                    MZ_ID = gv_resultat.DataKeys[gv_resultat.SelectedIndex]["MZ_ID"].ToStringNullSafe()
                };


            }
            
        }
        /// <summary>
        /// 按鈕: 查詢確定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Search_OK_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 輸入框: 身分證號變更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txt_MZ_ID_TextChanged(object sender, EventArgs e)
        {
            txt_MZ_ID.Text = txt_MZ_ID.Text.SafeTrim().ToUpper();
            _query.MZ_ID = txt_MZ_ID.Text;
            GetPersonnel(_query);
        }
        /// <summary>
        /// 下拉選單: 查詢現服機關
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddl_Search_MZ_EXAD_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gv_resultat_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }




    }
}