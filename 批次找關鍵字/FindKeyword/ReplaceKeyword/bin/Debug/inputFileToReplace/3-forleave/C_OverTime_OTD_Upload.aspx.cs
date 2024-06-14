using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TPPDDB.Helpers;
using TPPDDB.Models._3_ForLeave;
using TPPDDB.Service;

namespace TPPDDB._3_forleave
{
    public partial class C_OverTime_OTD_Upload : C_BasePage
    {
        C_OverTime_OTD_Upload_Query _query
        {
            get { return ViewState["query"] != null ? (C_OverTime_OTD_Upload_Query)ViewState["query"] : null; }
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
            WebUIHelpers.DropDownList_DataBind(ddlAD_SQL, ddl_Upload_AD, "MZ_KCHI", "MZ_KCODE", new WebUIHelpers.OtherListItem(0, new ListItem("請選擇", "")));
            ddl_Upload_AD.SelectedValue = Session["ADPMZ_EXAD"].ToStringNullSafe();

            //查詢單位選單
            string ddlUNIT_SQL = string.Format(@"SELECT RTRIM(MZ_KCHI) MZ_KCHI,RTRIM(MZ_KCODE) MZ_KCODE FROM A_KTYPE 
                                                 WHERE MZ_KTYPE='25' AND MZ_KCODE IN (SELECT MZ_UNIT FROM A_UNIT_AD WHERE MZ_AD='{0}') ",
                                                 ddl_Upload_AD.SelectedValue);
            switch (_strGID)
            {
                case "D": //一般使用者
                case "E": //所屬單位承辦人
                    ddlUNIT_SQL += string.Format(@"AND MZ_KCODE='{0}' ", Session["ADPMZ_EXUNIT"].ToStringNullSafe());
                    break;
            }
            WebUIHelpers.DropDownList_DataBind(ddlUNIT_SQL, ddl_Upload_Unit, "MZ_KCHI", "MZ_KCODE", new WebUIHelpers.OtherListItem(0, new ListItem("請選擇", "")));
            ddl_Upload_Unit.SelectedValue = Session["ADPMZ_EXUNIT"].ToStringNullSafe();
        }
        /// <summary>
        /// 初始化Query區塊
        /// </summary>
        /// <param name="query"></param>
        private void PageQueryInitial(C_OverTime_OTD_Upload_Query query)
        {
            switch (_strGID)
            {
                case "C": //分局、大隊、隊承辦人
                    ddl_Upload_AD.Enabled = false;
                    break;
                case "E": //所屬單位承辦人
                    ddl_Upload_AD.Enabled = false;
                    ddl_Upload_Unit.Enabled = false;
                    break;
            }

            txt_UploadYM.Text = ForDateTime.RCDateToTWDate(DateTime.Now.ToString("yyyy/MM/dd"), "yyyMM");
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
                _query = new C_OverTime_OTD_Upload_Query()
                {
                    TPMFION = _TPM_FION
                };

                PageInitial();

                PageQueryInitial(_query);
            }
        }

        /// <summary>
        /// 按鈕: 上傳檔案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Upload_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddl_Upload_AD.SelectedValue) || string.IsNullOrEmpty(ddl_Upload_Unit.SelectedValue) || string.IsNullOrEmpty(txt_UploadYM.Text))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('請選擇機關、單位、年月!');", true);
                return;
            }

            C_OVERTIME_ONDUTY_FILE_Model model = new C_OVERTIME_ONDUTY_FILE_Model()
            {
                MZ_AD = ddl_Upload_AD.SelectedValue,
                MZ_UNIT = ddl_Upload_Unit.SelectedValue,
                FILE_DATE = txt_UploadYM.Text,
                ADD_ID = Session["ADPMZ_ID"].ToStringNullSafe(),
                ADD_DATE = DateTime.Now
            };

            //檢查上傳檔案
            if (FileUpload1.HasFile)
            {
                string msg = string.Empty;
                if (new CommonService().CheckUpFile(FileUpload1, WebConfigHelpers.C_UPLOAD_FILEPATH_TO_SERVER_PATH, ref msg))
                {
                    model.FILE_URL = msg;
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", string.Format("alert('上傳檔案失敗，{0}!');", msg), true);
                    return;
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('請選擇上傳檔案!');", true);
                return;
            }

            //儲存資料
            if (CFService.C_OVERTIME_ONDUTY_FILE_Delete(model, _query.TPMFION) && CFService.C_OVERTIME_ONDUTY_FILE_Save(model, _query.TPMFION))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('上傳檔案完成。');", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('上傳檔案失敗!');", true);
            }
        }

        /// <summary>
        /// 下拉選單: 查詢單位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddl_Upload_AD_SelectedIndexChanged(object sender, EventArgs e)
        {
            string ddlUNIT_SQL = string.Format(@"SELECT RTRIM(MZ_KCHI) MZ_KCHI,RTRIM(MZ_KCODE) MZ_KCODE FROM A_KTYPE 
                                                 WHERE MZ_KTYPE='25' AND MZ_KCODE IN (SELECT MZ_UNIT FROM A_UNIT_AD WHERE MZ_AD='{0}') ",
                                                 ddl_Upload_AD.SelectedValue);

            WebUIHelpers.DropDownList_DataBind(ddlUNIT_SQL, ddl_Upload_Unit, "MZ_KCHI", "MZ_KCODE", new WebUIHelpers.OtherListItem(0, new ListItem("請選擇", "")));
        }
    }
}