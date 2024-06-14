using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TPPDDB._18_Online_Leave;
using TPPDDB.Helpers;
using TPPDDB.Models._18_Online_Work;
using TPPDDB.Models._3_ForLeave;
using TPPDDB.Service;

namespace TPPDDB._3_forleave
{
    // sam 20200723 新加班 最後一日 confirm 當日加班時數不足，是否為本月最後一筆加班資料？
    public partial class C_OverTime_Base_Input : System.Web.UI.Page
    {
        //issue_dave 增加顯示每月統計資料欄位 20200817
        private int gv_totmin = 0;
        private int MON_OVER_HOUR = 0;
        private int MON_OVER_MINUTE = 0;
        private int MON_PAY_HOUR = 0;
        private int MON_PAY_MIN = 0;
        private int MON_REST_HOUR = 0;
        private int MON_PRIZE_HOUR = 0;
        private int MON_SURPLUS_HOUR = 0;
        private int MON_SURPLUS_MINUTE = 0;

        CFService CFService = new CFService();
        OWService OWService = new OWService();
        public static string PageTitle = string.Empty;
        //模組代碼
        string _TPM_FION
        {
            get { return ViewState["TPM_FION"] != null ? ViewState["TPM_FION"].ToStringNullSafe() : string.Empty; }
            set { ViewState["TPM_FION"] = value; }
        }
        C_OverTime_Base_Input_Query _query
        {
            get { return ViewState["query"] != null ? (C_OverTime_Base_Input_Query)ViewState["query"] : null; }
            set { ViewState["query"] = value; }
        }

        /// <summary>
        /// 初始化各頁面功能
        /// </summary>
        private void PageInitial(string strGID)
        {
            //查詢機關選單
            string ddlAD_SQL = "SELECT RTRIM(MZ_KCHI) MZ_KCHI, RTRIM(MZ_KCODE) MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE='04' ";

            //if()

            //switch (strGID)
            //{
            //    case "A": //系統管理員
            //    case "TPMIDISAdmin": //系統開發管理員
            //    case "B": //局本部承辦人
            //        //如果是「中和分局」進來抓 中和第一、中和第二、中和
            //        if (Session["ADPMZ_EXAD"].ToStringNullSafe() == "382133600C")
            //        {
            //            ddlAD_SQL += "AND MZ_KCODE in ('382133400C','382133500C','382133600C') ";
            //        }
            //        else
            //        {
            //            ddlAD_SQL += "AND MZ_KCODE LIKE '38213%' ";
            //        }
            //        break;
            //    case "C": //分局、大隊、隊承辦人
            //    case "D": //一般使用者
            //    case "E": //所屬單位承辦人
            //        ddlAD_SQL += string.Format("AND MZ_KCODE ='{0}' ", Session["ADPMZ_EXAD"].ToStringNullSafe());
            //        break;
            //    default:
            //        Response.Redirect("~/ErrorPage.aspx?erid=TPFXXX0001");
            //        break;
            //}




            if (strGID == "A" || strGID == "B")
            {
                //如果是「中和分局」進來抓 中和第一、中和第二、中和
                if (Session["ADPMZ_EXAD"].ToStringNullSafe() == "382133600C")
                {
                    ddlAD_SQL += "AND MZ_KCODE in ('382133400C','382133500C','382133600C') ";
                }
                else
                {
                    ddlAD_SQL += "AND MZ_KCODE LIKE '38213%' ";
                }

                WebUIHelpers.DropDownList_DataBind(ddlAD_SQL, ddl_Search_AD, "MZ_KCHI", "MZ_KCODE", new WebUIHelpers.OtherListItem(0, new ListItem("全部", "")));
                ddl_Search_AD.SelectedValue = Session["ADPMZ_EXAD"].ToStringNullSafe();

                //查詢單位選單
                string ddlUNIT_SQL = string.Format(@"SELECT RTRIM(MZ_KCHI) MZ_KCHI,RTRIM(MZ_KCODE) MZ_KCODE FROM A_KTYPE 
                                                 WHERE MZ_KTYPE='25' AND MZ_KCODE IN (SELECT MZ_UNIT FROM A_UNIT_AD WHERE MZ_AD='{0}') ",
                                                     ddl_Search_AD.SelectedValue);


                WebUIHelpers.DropDownList_DataBind(ddlUNIT_SQL, ddl_Search_Unit, "MZ_KCHI", "MZ_KCODE", new WebUIHelpers.OtherListItem(0, new ListItem("全部", "")));
                ddl_Search_Unit.SelectedValue = Session["ADPMZ_EXUNIT"].ToStringNullSafe();
            }
            else if (strGID == "C")
            {
                ddlAD_SQL += string.Format("AND MZ_KCODE ='{0}' ", Session["ADPMZ_EXAD"].ToStringNullSafe());
                WebUIHelpers.DropDownList_DataBind(ddlAD_SQL, ddl_Search_AD, "MZ_KCHI", "MZ_KCODE", null);
                ddl_Search_AD.SelectedValue = Session["ADPMZ_EXAD"].ToStringNullSafe();

                //查詢單位選單
                string ddlUNIT_SQL = string.Format(@"SELECT RTRIM(MZ_KCHI) MZ_KCHI,RTRIM(MZ_KCODE) MZ_KCODE FROM A_KTYPE 
                                                 WHERE MZ_KTYPE='25' AND MZ_KCODE IN (SELECT MZ_UNIT FROM A_UNIT_AD WHERE MZ_AD='{0}') ",
                                                     Session["ADPMZ_EXAD"].ToStringNullSafe());

                WebUIHelpers.DropDownList_DataBind(ddlUNIT_SQL, ddl_Search_Unit, "MZ_KCHI", "MZ_KCODE", new WebUIHelpers.OtherListItem(0, new ListItem("全部", "")));
                ddl_Search_Unit.SelectedValue = Session["ADPMZ_EXUNIT"].ToStringNullSafe();
            }
            else if (strGID == "D" || strGID == "E")
            {
                ddlAD_SQL += string.Format("AND MZ_KCODE ='{0}' ", Session["ADPMZ_EXAD"].ToStringNullSafe());
                WebUIHelpers.DropDownList_DataBind(ddlAD_SQL, ddl_Search_AD, "MZ_KCHI", "MZ_KCODE", null);
                ddl_Search_AD.SelectedValue = Session["ADPMZ_EXAD"].ToStringNullSafe();

                //查詢單位選單
                string ddlUNIT_SQL = string.Format(@"SELECT RTRIM(MZ_KCHI) MZ_KCHI,RTRIM(MZ_KCODE) MZ_KCODE FROM A_KTYPE 
                                                 WHERE MZ_KTYPE='25' AND MZ_KCODE IN (SELECT MZ_UNIT FROM A_UNIT_AD WHERE MZ_AD='{0}') ",
                                                     Session["ADPMZ_EXAD"].ToStringNullSafe());

                ddlUNIT_SQL += string.Format(@"AND MZ_KCODE='{0}' ", Session["ADPMZ_EXUNIT"].ToStringNullSafe());
                WebUIHelpers.DropDownList_DataBind(ddlUNIT_SQL, ddl_Search_Unit, "MZ_KCHI", "MZ_KCODE", null);
                ddl_Search_Unit.SelectedValue = Session["ADPMZ_EXUNIT"].ToStringNullSafe();
            }
            else
            {
                Response.Redirect("~/ErrorPage.aspx?erid=TPFXXX0001");
            }

            //建立加班類型選項
            //string rblOT_SQL = string.Format(@"SELECT CODE_VALUE, CODE_DESC from C_OVERTIME_CODE WHERE CODE_TYPE='OT' and CODE_VALUE <> 'OTR' and CODE_VALUE <> 'OTU' And STATUS='Y' ");
            //string localip = System.Configuration.ConfigurationManager.AppSettings["ip"].ToString();
            //if(localip == "154")
            //{
            //    rblOT_SQL = string.Format(@"SELECT CODE_VALUE, CODE_DESC from C_OVERTIME_CODE WHERE CODE_TYPE='OT' and CODE_VALUE <> 'OTR'  and STATUS='Y' ");
            //}

            string rblOT_SQL = rblOT_SQL = string.Format(@"SELECT CODE_VALUE, CODE_DESC from C_OVERTIME_CODE WHERE CODE_TYPE='OT' and CODE_VALUE <> 'OTR'  and STATUS='Y' ");


            WebUIHelpers.DropDownList_DataBind(rblOT_SQL, ddl_OVERTIME_TYPE, "CODE_DESC", "CODE_VALUE", null);

        }
        /// <summary>
        /// 初始化Query區塊
        /// 1.更新姓名選單
        /// 2.抓取選單第一位人員的資料
        /// 如果抓不到人員,則回傳false
        /// </summary>
        /// <param name="query"></param>
        /// <returns>true:抓取成功,false:抓不到人 多半是切換單位的時候發生,某些單位可能無人編制</returns>
        private bool PageQueryInitial(C_OverTime_Base_Input_Query query)
        {
            //建立姓名選單
            string ddlQueryName_SQL = string.Format(@"SELECT MZ_NAME,MZ_ID FROM A_DLBASE WHERE MZ_EXAD='{0}' AND MZ_EXUNIT='{1}' ",
                query != null && !string.IsNullOrEmpty(query.SearchAD) ? query.SearchAD : Session["ADPMZ_EXAD"].ToStringNullSafe(),
                query != null && !string.IsNullOrEmpty(query.SearchUnit) ? query.SearchUnit : Session["ADPMZ_EXUNIT"].ToStringNullSafe());
            DataTable dt = o_DBFactory.ABC_toTest.Create_Table(ddlQueryName_SQL, new List<SqlParameter>());
            //若姓名選單無人
            if (dt.Rows.Count == 0)
            {
                //這邊跳出就好 等出去了再給錯誤訊息
                return false;
            }
            WebUIHelpers.DropDownList_DataBind(dt, ddl_Query_Name, "MZ_NAME", "MZ_ID", null);


            string ddl_selectValue = (query != null && !string.IsNullOrEmpty(query.MZ_ID)) ? query.MZ_ID : Session["ADPMZ_ID"].ToStringNullSafe();
            if (ddl_Query_Name.Items.FindByValue(ddl_selectValue) != null)
            {
                ddl_Query_Name.SelectedValue = ddl_selectValue;
            }
            else
            {
                ddl_Query_Name.SelectedIndex = 0;
            }

            //取得基本資料
            string aBase_SQL = string.Format(@"Select AKU.MZ_KCHI MZ_EXUNIT, AKO.MZ_KCHI MZ_OCCC, MZ_ID From A_DLBASE 
                                               left join A_KTYPE AKU ON AKU.MZ_KCODE=MZ_EXUNIT AND AKU.MZ_KTYPE='25' 
                                               left join A_KTYPE AKO ON AKO.MZ_KCODE=MZ_OCCC AND AKO.MZ_KTYPE='26' 
                                               Where MZ_ID='{0}' ", ddl_Query_Name.SelectedValue);
            DataTable dt_ADLBASE = o_DBFactory.ABC_toTest.Create_Table(aBase_SQL, "ABASE");
            if (dt_ADLBASE.Rows != null && dt_ADLBASE.Rows.Count > 0)
            {
                lbl_Unit.Text = dt_ADLBASE.Rows[0]["MZ_EXUNIT"].ToStringNullSafe(); //單位
                lbl_OCCC.Text = dt_ADLBASE.Rows[0]["MZ_OCCC"].ToStringNullSafe();   //職稱
                lbl_MZ_ID.Text = dt_ADLBASE.Rows[0]["MZ_ID"].ToStringNullSafe();    //身分證
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('查無人員資料!');", true);
            }

            //加班日期欄位
            txt_OVER_DAY.Text = ForDateTime.RCDateToTWDate(DateTime.Now.ToString("yyyy/MM/dd"), "yyy/MM/dd");
            txt_OVER_DAY.Enabled = true;
            //加班事由欄位
            txt_REASON.Text = "";
            txt_REASON.Enabled = true;
            //加班時
            txt_OVER_HOUR.Text = "0";
            txt_OVER_HOUR.Enabled = false;
            //加班分
            txt_OVER_MINUTE.Text = "0";
            txt_OVER_MINUTE.Enabled = false;
            //加班類型
            ddl_OVERTIME_TYPE.Enabled = true;
            ddl_OVERTIME_TYPE.SelectedIndex = -1;
            //每時金額
            lbl_PAY_UNIT.Text = CFService.GetPayUnit(ddl_Query_Name.SelectedValue).ToStringNullSafe();
            //申請業務加班 時數
            txt_PAY_HOUR.Text = "0";
            txt_PAY_HOUR.Enabled = false;
            //申請業務加班 分鐘數
            txt_PAY_MIN.Text = "0";
            txt_PAY_MIN.Enabled = false;
            //申請輪值加班 時數
            txt_SHIFT_HOUR.Text = "0";
            txt_SHIFT_HOUR.Enabled = false;
            //申請輪值加班 分鐘數
            txt_SHIFT_MIN.Text = "0";
            txt_SHIFT_MIN.Enabled = false;

            //備註
            txt_OVER_REMARK.Text = "";
            txt_OVER_REMARK.Enabled = false;
            //狀態
            txt_OVER_STATUS.Text = "";

            //按鈕區塊
            btn_Update.Enabled = false;
            btn_Delete.Enabled = false;
            btn_ReOnline.Enabled = false;
            btn_ReGetTime.Enabled = false;
            esgstr.Visible = false;

            return true;
        }
        /// <summary>
        /// 取得加班資料
        /// </summary>
        /// <param name="query"></param>
        private void GetOverTime_Bind_GridView(C_OverTime_Base_Input_Query query)
        {
            //issue_dave 增加顯示每月統計資料 20200806
            gv_totmin = 0;
            //issue_dave 增加顯示每月統計資料欄位 20200817
            MON_OVER_HOUR = 0;
            MON_OVER_MINUTE = 0;
            MON_PAY_HOUR = 0;
            MON_PAY_MIN = 0;
            MON_REST_HOUR = 0;
            MON_PRIZE_HOUR = 0;
            MON_SURPLUS_HOUR = 0;
            MON_SURPLUS_MINUTE = 0;

            gv_resultat.DataSource = CFService.GetOverTimeBase(query);
            gv_resultat.SelectedIndex = -1;
            gv_resultat.DataBind();

            PageQueryInitial(query);
        }
        /// <summary>
        /// 取得主管陳核名單
        /// </summary>
        /// <param name="query"></param>
        private void GetOnlineCheckManager_Bind_GridView(Online_work_Query query)
        {
            //特殊規則:
            /*
             如果新增的這個人(MZ_ID)的MZ_EXAD為382130000C且MZ_OCCC為1078或1050或1410，
             則這些人的陳核清單顯示要改成特別規則：
             顯示(C_REVIEW_MANAGEMENT)這張表的MZ_EXAD為382130000C且MZ_EXUNIT為0002且TOP_MANAGER為Y的人。
             */

            //抓申請人的 職稱代碼
            _2_salary.Police police = new _2_salary.Police(ddl_Query_Name.SelectedValue);
            //總局 的 1410	督察長  1078	科長  1050 主任
            if (query.MZ_AD == "382130000C" && (police.occc == "1078"|| police.occc == "1050" || police.occc == "1410"))
            {
                query.MZ_UNIT = "0002";
                query.isTOP_MANAGER = true;
            }

            gv_online_people.DataSource = OWService.GetOverTimeOnlineReview(query);
            gv_online_people.SelectedIndex = -1;
            gv_online_people.DataBind();

            hfd_MZ_ID.Value = ddl_Query_Name.SelectedValue;
            hfd_OVERDAY.Value = txt_OVER_DAY.Text.Replace("/", "").Replace("-", "");
        }
        /// <summary>
        /// 檢核上傳檔案
        /// </summary>
        /// <param name="fu"></param>
        /// <returns></returns>
        private bool CheckUpFile(FileUpload fu, ref string msg)
        {
            try
            {
                string fName = fu.FileName;
                HttpPostedFile fuPost = fu.PostedFile;
                int tFileLength = fuPost.ContentLength;
                byte[] tFileByte = new byte[tFileLength];
                fuPost.InputStream.Read(tFileByte, 0, tFileLength);
                if (tFileLength <= 10485760)
                {
                    if (!Directory.Exists(WebConfigHelpers.C_UPLOAD_FILEPATH_TO_SERVER_PATH))
                    {
                        Directory.CreateDirectory(WebConfigHelpers.C_UPLOAD_FILEPATH_TO_SERVER_PATH);
                    }
                    //取得副檔名
                    string Extension = Path.GetExtension(fName);
                    fName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + _query.TPMFION + Extension.ToString();
                    FileStream tNewfile = new FileStream(WebConfigHelpers.C_UPLOAD_FILEPATH_TO_SERVER_PATH + fName, FileMode.Create);
                    tNewfile.Write(tFileByte, 0, tFileByte.Length);
                    tNewfile.Close();

                    msg = fName;
                    return true;
                }
                else
                {
                    msg = "檔案大小超過10MB限制!";
                    return false;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            //一般權限檢查
            C.check_power();
            //取得群組權限，人事系統取得 MZ_POWER
            string strGID = o_a_Function.strGID(Session["ADPMZ_ID"].ToStringNullSafe());
            //取得模組
            _TPM_FION = Request.QueryString["TPM_FION"].ToStringNullSafe();

            if (!IsPostBack)
            {
                _query = new C_OverTime_Base_Input_Query()
                {
                    TPMFION = _TPM_FION,
                    MZ_ID = Session["ADPMZ_ID"].ToStringNullSafe(),
                    SearchYM = (DateTime.Now.Year - 1911).ToString() + DateTime.Now.ToString("MM")
                };

                //_query = new C_OverTime_Base_Input_Query()
                //{
                //    TPMFION = _TPM_FION,
                //    MZ_ID = Session["ADPMZ_ID"].ToStringNullSafe(),
                //    SearchYM = (DateTime.Now.Year - 1911).ToString() + DateTime.Now.AddMonths(-1).ToString("MM")
                //};

                PageInitial(strGID);
                //取得頁面標題
                PageTitle = new CommonService().GetPageTitleName(_TPM_FION);
                //預設查詢
                GetOverTime_Bind_GridView(_query);
                //讀取人事室人員_All
                讀取人事室人員_All();
            }
        }
        /// <summary>
        /// 按鈕: 打開查詢頁面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Search_Click(object sender, EventArgs e)
        {
            mpe_Search.Show();
        }
        /// <summary>
        /// 按鈕: 查詢加班資料 確定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Search_Ok_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddl_Search_AD.SelectedValue) || string.IsNullOrEmpty(ddl_Search_Unit.SelectedValue) || string.IsNullOrEmpty(txt_Search_YM.Text))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('機關、單位、年月未填!');", true);
                return;
            }

            //依照條件建立該單位人員選單
            _query = new C_OverTime_Base_Input_Query()
            {
                TPMFION = _TPM_FION,
                SearchAD = ddl_Search_AD.SelectedValue,
                SearchUnit = ddl_Search_Unit.SelectedValue,
                SearchYM = txt_Search_YM.Text
            };

            // 初始化Query區塊
            // 1.更新姓名選單
            // 2.抓取選單第一位人員的資料
            // 如果抓不到人員,則回傳false
            bool isOK = PageQueryInitial(_query);
            //若姓名選單無人
            if (isOK == false)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('查無人員資料!');", true);
                return;
            }
            //取得正確的預設查詢人員ID
            _query.MZ_ID = ddl_Query_Name.SelectedValue;
            //讀取人事室人員名單
            讀取人事室人員_All();
            //查詢加班資料
            GetOverTime_Bind_GridView(_query);
            UpdatePanel1.Update();
            mpe_Search.Hide();
        }


        /// <summary>
        /// 按鈕: 新增加班資料
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Insert_Click(object sender, EventArgs e)
        {
            //檢查必要欄位


            if (string.IsNullOrEmpty(ddl_Query_Name.SelectedValue))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('請選擇申請人!');", true);
                return;
            }
            //如果沒寫加班日期
            if (string.IsNullOrEmpty(txt_OVER_DAY.Text))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('請填加班日期!');", true);
                return;
            }
            //將加班日期轉換成民國年
            string tmpOverDay = ForDateTime.TWDateToRCDate(txt_OVER_DAY.Text);
            //檢查日期格式
            if (!(bool)ForDateTime.CheckDateTime(tmpOverDay))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('加班日期格式錯誤!');", true);
                return;
            }
            //抓取今天時間
            DateTime nowdate = DateTime.Now;
            //加班日期轉換成民國年
            DateTime dtOverDay = Convert.ToDateTime(ForDateTime.TWDateToRCDate(txt_OVER_DAY.Text));

            /*         
             特殊規則:
             維護單1121201(多個問題之一)
             問題： 配合輪值加班功能，原本新增和修改資料有設定檢核機制，
             現在針對加班事由(REASON)首6個字為「輪值加班補登」的資料移除檢核讓它通過(未來會在關起來就是了) "

            12/19 以下條件開放
            17:25 If 
            #業務加班&其他業務加班 
            -加班日期申請不可以小於兩個月!!
            -該月份之加班費申請資料已審核確認,不可新增!!
            17:27 If "修改"按鈕前端會反灰，讓這些資料前端可以點
             */
            //判斷是否符合首6個字為「輪值加班補登」
            bool is輪值加班補登 = this.txt_REASON.Text.Contains("輪值加班補登");
            //非輪值加班補登,這樣宣告純粹是為了提升可讀性(懶得思考)
            bool isNot_輪值加班補登 = !is輪值加班補登;


            //針對申請時間起訖做一些限制 但是 值日補休(OTD)不在此限
            if (ddl_OVERTIME_TYPE.SelectedItem.Text != "值日補休")
            {

                //檢查是否審核確認過
                string sql = "select NVL(count(*),0) as count1 from  C_OVERTIMEMONTH_HOUR WHERE MZ_ID = '" + _query.MZ_ID + "' and MZ_YEAR='" + txt_OVER_DAY.Text.Substring(0, 3) + "' AND MZ_MONTH='" + txt_OVER_DAY.Text.Substring(4, 2) + "' and (MZ_VERIFY = 'Y')";
                string count1 = o_DBFactory.ABC_toTest.vExecSQL(sql);
                //如果這個月已經審核,而且不是為了輪值加班補登
                if (int.Parse(count1) > 0 && isNot_輪值加班補登)
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('該月份之加班費申請資料已審核確認,不可新增!!');", true);
                    return;
                }

                //不得早於期限??
                DateTime dttmp = Convert.ToDateTime("2021-12-01");
                if (dtOverDay < dttmp)
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('不得新增110/12/01前的日期!');", true);
                    return;
                }
                //檢查加班日是否可以申請?
                bool isOK = Check_OverDay_inLimit(dtOverDay, nowdate, isNot_輪值加班補登);
                if (isOK == false)
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('申請加班日期超過時效！（自每月11日起不得提出上個月之加班申請且不得提出未來第8日起之加班申請）');", true);
                    return;
                }

            }
            //// sam test new add
            //檢查補申請限制一天內 , 暫時mark掉
            //if (Convert.ToDateTime(tmpOverDay) < Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyy/MM/dd")))
            //{
            //    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('無法申請超過一天的加班!');", true);
            //    return;
            //}
            //}

            //針對 值日補休 的檢核如下
            //檢核加班時數
            //加班類型:值日補休 的時候,時數合計不得為0
            bool isOVER_HOUR_Zero = (txt_OVER_HOUR.Text == "0") || (txt_OVER_HOUR.Text == "");
            //加班小時 轉INT
            int intOVER_HOUR = 0;
            int.TryParse(txt_OVER_HOUR.Text, out intOVER_HOUR);
            //加班分鐘 轉INT
            int intOVER_MINUTE = 0;
            int.TryParse(txt_OVER_MINUTE.Text, out intOVER_MINUTE);
            //如果 新增的加班類別為 值日補休 且 加班的時分都是0
            if (ddl_OVERTIME_TYPE.SelectedItem.Text == "值日補休" && intOVER_HOUR == 0 && intOVER_MINUTE == 0)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('申請值日補休時數合計不得為0！');", true);
                return;
            }


            _query.MZ_ID = ddl_Query_Name.SelectedValue;
            _query.OVER_DAY = txt_OVER_DAY.Text.Replace("/", "").Replace("-", "");
            _query.SearchYM = _query.OVER_DAY.SubstringOutToEmpty(0, 5); //查詢加班資料用
                                                                         //加班資料重複檢查
                                                                         //檢查資料重複 true:未重複 false:重複資料
            if (CFService.Check_OverTimeBase_Repeat(_query) == false)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('已有該筆加班資料!');", true);
                return;
            }

            DataTable dt = o_DBFactory.ABC_toTest.Create_Table(string.Format("SELECT MZ_EXAD,MZ_EXUNIT,MZ_OCCC FROM A_DLBASE WHERE MZ_ID='{0}'", _query.MZ_ID), "A_DLBASE");

            var qMZ_AD = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_AD FROM VW_A_DLBASE_S1 WHERE MZ_ID='" + _query.MZ_ID + "'");
            var qMZ_UNIT = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_UNIT FROM VW_A_DLBASE_S1 WHERE MZ_ID='" + _query.MZ_ID + "'");
            //MARK 其他業務加班 , 加班時數可以自己輸
            int allhour = int.Parse(txt_OVER_HOUR.Text) * 60 + int.Parse(txt_OVER_MINUTE.Text);
            //新增資料
            C_OVERTIME_BASE_Model addModel = new C_OVERTIME_BASE_Model()
            {
                MZ_ID = _query.MZ_ID,
                OVER_DAY = _query.OVER_DAY,
                MZ_EXAD = dt.Rows != null ? dt.Rows[0]["MZ_EXAD"].ToStringNullSafe() : "",
                MZ_EXUNIT = dt.Rows != null ? dt.Rows[0]["MZ_EXUNIT"].ToStringNullSafe() : "",
                MZ_OCCC = dt.Rows != null ? dt.Rows[0]["MZ_OCCC"].ToStringNullSafe() : "",
                REASON = txt_REASON.Text.SafeTrim(),
                PAY_UNIT = CFService.GetPayUnit(_query.MZ_ID),
                LOCK_FLAG = "Y", //新增因送簽核，預設鎖定
                OVERTIME_TYPE = ddl_OVERTIME_TYPE.SelectedValue,
                IS_FREESIGN = "N",
                OPTYPE = rdoptype.SelectedValue,
                MZ_AD = qMZ_AD,
                MZ_UNIT = qMZ_UNIT,
                OVER_TOTAL = allhour,
                //增加新增人員 , 新增日期欄位        
                INS_ID = Session["ADPMZ_ID"].ToString(), //新增者ID
                INS_DATE = (DateTime.Today.Year - 1911).ToString() + DateTime.Today.ToString("MMdd")
            };

            //依照申請類型調整流程
            switch (addModel.OVERTIME_TYPE)
            {
                case "OTB": //業務加班
                    #region 檢核是否為免簽核人員
                    DataTable excardDt = CFService.GetOVERTIME_EXCARD(_query);
                    if (excardDt.Rows.Count > 0)
                    {
                        addModel.IS_FREESIGN = "Y";
                        addModel.LOCK_FLAG = "N";
                    }
                    #endregion
                    break;
                case "OTD": //值日補休
                    addModel.IS_FREESIGN = "Y";

                    //值日補休時數目前聽小隊長提出會有特例，且目前系統無法正確定義假日資料
                    //由申請者直接輸入申請時數並帶入修改時數中
                    addModel.OVER_TOTAL = 0;
                    addModel.OVERTIME_CHG_TOTAL = allhour;
                    break;
                case "OTR": //特殊輪值
                case "OTT": //其他業務加班
                    #region 檢核是否為免簽核人員
                    DataTable excardDt1 = CFService.GetOVERTIME_EXCARD(_query);
                    if (excardDt1.Rows.Count > 0)
                    {
                        addModel.IS_FREESIGN = "Y";
                        addModel.LOCK_FLAG = "N";
                    }
                    #endregion
                    break;
                //addModel.IS_FREESIGN = "Y";
                //addModel.LOCK_FLAG = "N";
                //break;
                case "OTU": //超勤補休
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('無法直接申請超勤補休，請透過「6.1 超勤時數輸入」新增！');", true);

                    //更新頁面
                    GetOverTime_Bind_GridView(_query);
                    UpdatePanel1.Update();
                    return;
            }

            if (CFService.C_OVERTIME_BASE_Save(addModel, _query.TPMFION))
            {
                if (addModel.LOCK_FLAG == "Y")
                {
                    switch (addModel.OVERTIME_TYPE)
                    {
                        case "OTD": //值日補休
                            #region 值日補休送人事室審核
                            //取得人事室加班審核人
                            DataTable pay0Dt = OWService.GetOverTimeOnlineReview(
                                new Online_work_Query()
                                {
                                    OVERTIME_TYPE = "OLWU",
                                    SCHEDULE_SORT = "1",//申請流程的第1關,在本功能應該都是第1關,後面的關卡要去Online_work.aspx處理
                                    REVIEW_LEVEL = "4", //線上簽核-加班承辦人(人事室)
                                    MZ_AD = addModel.MZ_EXAD,
                                    MZ_UNIT = addModel.MZ_EXUNIT
                                });
                            //取得加班資料序列號
                            int seqNo = CFService.GetOverTimeBase(addModel.MZ_ID, addModel.OVER_DAY, _query.TPMFION).SN;
                            //判斷是否有加班承辦人 與 加班流水號?
                            if (pay0Dt != null && pay0Dt.Rows.Count > 0 && seqNo > 0)
                            {
                                #region 陳核人事室加班承辦人
                                //C_OVERTIME_HISTORY_Model forPAY0 = new C_OVERTIME_HISTORY_Model()
                                //{
                                //    OVERTIME_SN = seqNo,
                                //    REVIEW_ID = pay0Dt.Rows[0]["MZ_ID"].ToStringNullSafe(),
                                //    LETTER_DATE = ForDateTime.RCDateToTWDate(DateTime.Now.ToString("yyyy/MM/dd"), "yyyMMdd"),
                                //    OVERTIME_SCHEDULE_SN = Convert.ToInt32(pay0Dt.Rows[0]["SCHEDULE_SN"].ToStringNullSafe()),
                                //    LETTER_TIME = DateTime.Now.ToString("HH:mm:ss"),
                                //    PROCESS_STATUS = o_DBFactory.ABC_toTest.Get_First_Field("Select C_STATUS_SN From C_STATUS Where C_STATUS_NAME='待審中'", null),
                                //    OVERTIME_TYPE = "OLWU",
                                //    SEND_ID = addModel.MZ_ID
                                //};
                                //if (OWService.C_OVERTIME_HISTORY_Save(forPAY0, _query.TPMFION))
                                //{
                                //    //20210803
                                //    Getfileup_GridView(seqNo.ToString());
                                //    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('已送出資料!');", true);
                                //}
                                //else
                                //{
                                //    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('新增線上簽核失敗!');", true);
                                //}
                                #endregion
                                //20210803
                                Getfileup_GridView(seqNo.ToString());
                                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('已送出資料，請上傳檔案!');", true);
                            }
                            else
                            {
                                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('無加班承辦人或無加班流水號!');", true);
                            }
                            #endregion
                            break;
                        default:
                            #region 預設送主管審核
                            //開啟簽核選單
                            mpe_OnlienPeople.Show();
                            //設定可選主管列表
                            Online_work_Query query = new Online_work_Query()
                            {
                                OVERTIME_TYPE = "OLWA",
                                SCHEDULE_SORT = "1",//申請流程的第1關,在本功能應該都是第1關,後面的關卡要去Online_work.aspx處理
                                REVIEW_LEVEL = "2",
                                MZ_AD = addModel.MZ_EXAD,
                                MZ_UNIT = addModel.MZ_EXUNIT
                            };
                            GetOnlineCheckManager_Bind_GridView(query);
                            #endregion
                            return;
                    }
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('新增加班資料失敗!');", true);
            }


            //更新頁面
            GetOverTime_Bind_GridView(_query);
            UpdatePanel1.Update();
        }

        /// <summary>
        /// 檢查加班日是否可以申請?
        /// </summary>
        /// <param name="dtOverDay">哪一天加班?</param>
        /// <param name="nowdate">今天哪一天?</param>
        /// <param name="isNot_輪值加班補登"></param>
        /// <returns></returns>
        public static bool Check_OverDay_inLimit(DateTime dtOverDay, DateTime nowdate, bool isNot_輪值加班補登)
        {
            //不得早於期限??
            DateTime dttmp = Convert.ToDateTime("2021-12-01");
            if (dtOverDay < dttmp)
            {
                return false;
            }
            /*
規則:自每月11日起不得提出上個月之加班申請且不得提出未來第8日起之加班申請
EX:
今天幾號	可加班起日	可加班迄日
2024/1/31	2024/1/1	2024/2/7
2024/2/1	2024/1/1	2024/2/8
2024/2/2	2024/1/1	2024/2/9
2024/2/9	2024/1/1	2024/2/16
2024/2/10	2024/1/1	2024/2/17
2024/2/11	2024/2/1	2024/2/18
2024/2/12	2024/2/1	2024/2/19
2024/2/13	2024/2/1	2024/2/20
             */
            //先判斷不超過8天以後
            if (dtOverDay >= nowdate.AddDays(8).Date)
            {
                return false;
            }

            //上月1日
            DateTime LastMonth_day1 = DateTime.Parse(nowdate.AddMonths(-1).ToString("yyyy-MM-01"));
            //如果是 1~10號,不可超過上個月 1/1
            //取得上個月 1/1
            if (nowdate.Day >= 1 && nowdate.Day <= 10 && dtOverDay < LastMonth_day1 && isNot_輪值加班補登)
            {
                return false;
            }
            //本月1日
            DateTime thisMonth_day1 = DateTime.Parse(nowdate.ToString("yyyy-MM-01"));
            if (nowdate.Day >= 11 && nowdate.Day <= 31 && dtOverDay < thisMonth_day1 && isNot_輪值加班補登)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 按鈕: 確定簽核人員
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Online_OK_Click(object sender, EventArgs e)
        {
            string OVERTIME_TYPE = ddl_OVERTIME_TYPE.SelectedValue.ToStringNullSafe(); //加班類型
            string MZ_ID = lbl_MZ_ID.Text.ToStringNullSafe();
            string OVER_DAY = txt_OVER_DAY.Text.ToStringNullSafe().Replace("/", "");
            string strSQL = "";
            var otf=o_DBFactory.ABC_toTest;
            List<SqlParameter> parameters;
            //簽核人員流水號
            if (gv_online_people.SelectedIndex < 0)
            {
                strSQL = @"Update C_OVERTIME_BASE Set LOCK_FLAG = 'N'
                                    Where MZ_ID=@MZ_ID And OVER_DAY=@OVER_DAY And OVERTIME_TYPE=@OVERTIME_TYPE ";
                parameters = new List<SqlParameter>()
                        {
                            new SqlParameter("MZ_ID",MZ_ID ),
                            new SqlParameter("OVER_DAY", OVER_DAY),
                            new SqlParameter("OVERTIME_TYPE", OVERTIME_TYPE),
                        };
                otf.MultiSql_Add(strSQL, parameters.ToArray());
                otf.MultiSQL_Exec();

                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('加班日: " + OVER_DAY + " 無挑選簽核人員，請重新簽核!');", true);

                GetOverTime_Bind_GridView(_query);
                UpdatePanel1.Update();
                mpe_OnlienPeople.Hide();

                return;
            }
            string review_id = gv_online_people.DataKeys[gv_online_people.SelectedIndex]["MZ_ID"].ToStringNullSafe();


            if (string.IsNullOrEmpty(review_id) || string.IsNullOrEmpty(hfd_MZ_ID.Value) || string.IsNullOrEmpty(hfd_OVERDAY.Value))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('簽核人員資料異常，無法進行線上簽核!');", true);
                return;
            }

            //取得簽核資訊
            strSQL = string.Format(@"Select C_OVERTIME_HISTORY.o_sn,cob.SN, cs.C_STATUS_SN, coc.CODE_VALUE, cos.SN SCHEDULE_SN From C_OVERTIME_BASE cob
                                            left join C_STATUS cs on cs.C_STATUS_NAME='待審中'
                                            left join C_OVERTIME_CODE coc on coc.CODE_TYPE='OLW' And coc.CODE_VALUE='OLWA'
                                            left join C_OVERTIME_SCHEDULE cos on cos.SCHEDULE_TYPE='OLWA' And cos.SORT=1
                                            left join C_OVERTIME_HISTORY  on C_OVERTIME_HISTORY.OVERTIME_SN=cob.sn And C_OVERTIME_HISTORY.PROCESS_DATE is null And C_OVERTIME_HISTORY.PROCESS_STATUS='4'
                                            Where cob.MZ_ID='{0}' And cob.OVER_DAY='{1}' ", hfd_MZ_ID.Value, hfd_OVERDAY.Value);
            DataTable baseDt = o_DBFactory.ABC_toTest.Create_Table(strSQL, new List<SqlParameter>());

            int covertimehsn = 0;
            if (!string.IsNullOrEmpty(baseDt.Rows[0]["o_sn"].ToString()))
            {
                covertimehsn = Convert.ToInt32(baseDt.Rows[0]["o_sn"].ToString());
            }

            if (baseDt.Rows != null && baseDt.Rows.Count > 0)
            {
                C_OVERTIME_HISTORY_Model addModel = new C_OVERTIME_HISTORY_Model()
                {
                    O_SN = covertimehsn,
                    OVERTIME_SN = Convert.ToInt32(baseDt.Rows[0]["SN"]),
                    REVIEW_ID = review_id,
                    LETTER_DATE = ForDateTime.RCDateToTWDate(DateTime.Now.ToString("yyyy/MM/dd"), "yyyMMdd"),
                    OVERTIME_SCHEDULE_SN = Convert.ToInt32(baseDt.Rows[0]["SCHEDULE_SN"]),
                    LETTER_TIME = DateTime.Now.ToString("HH:mm:ss"),
                    PROCESS_STATUS = baseDt.Rows[0]["C_STATUS_SN"].ToStringNullSafe(),
                    OVERTIME_TYPE = baseDt.Rows[0]["CODE_VALUE"].ToStringNullSafe(),
                    SEND_ID = hfd_MZ_ID.Value
                };

                if (OWService.C_OVERTIME_HISTORY_Save(new List<C_OVERTIME_HISTORY_Model>() { addModel }, _query.TPMFION))
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('已送出資料!');", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('新增線上簽核失敗!');", true);
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('查無已申請加班單，無法送出線上簽核!');", true);
            }

            //更新查詢頁面
            GetOverTime_Bind_GridView(_query);
            UpdatePanel1.Update();
            mpe_OnlienPeople.Hide();
        }

        /// <summary>
        /// 按鈕: 取消簽核人員
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>   
        protected void btn_Online_Exit_Click1(object sender, EventArgs e)
        {
            string OVERTIME_TYPE = ddl_OVERTIME_TYPE.SelectedValue.ToStringNullSafe(); //加班類型
            string MZ_ID = lbl_MZ_ID.Text.ToStringNullSafe();
            string OVER_DAY = txt_OVER_DAY.Text.ToStringNullSafe().Replace("/", "");
            string strSQL = "";
            var otf=o_DBFactory.ABC_toTest;
            List<SqlParameter> parameters;

            strSQL = @"Update C_OVERTIME_BASE Set LOCK_FLAG = 'N'
                                    Where MZ_ID=@MZ_ID And OVER_DAY=@OVER_DAY And OVERTIME_TYPE=@OVERTIME_TYPE ";
            parameters = new List<SqlParameter>()
                        {
                            new SqlParameter("MZ_ID",MZ_ID ),
                            new SqlParameter("OVER_DAY", OVER_DAY),
                            new SqlParameter("OVERTIME_TYPE", OVERTIME_TYPE),
                        };
            otf.MultiSql_Add(strSQL, parameters.ToArray());
            otf.MultiSQL_Exec();

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('加班日: " + OVER_DAY + " 無挑選簽核人員，請重新簽核!');", true);

            GetOverTime_Bind_GridView(_query);
            UpdatePanel1.Update();
            mpe_OnlienPeople.Hide();

            return;

        }
        /// <summary>
        /// 按鈕: 更新刷卡時間，僅業務加班需要
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_ReGetTime_Click(object sender, EventArgs e)
        {
            if (gv_resultat.SelectedIndex < 0)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('未選擇需要更新的資料!');", true);
                return;
            }
            if (ddl_OVERTIME_TYPE.SelectedValue != "OTB" && ddl_OVERTIME_TYPE.SelectedValue != "OTT" && ddl_OVERTIME_TYPE.SelectedValue != "OTD" && ddl_OVERTIME_TYPE.SelectedValue != "OTU")
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('僅業務加班、其他業務加班、超勤補休、值日補休可更新刷卡時間!');", true);
                return;
            }

            C_OVERTIME_BASE_Model model = new C_OVERTIME_BASE_Model()
            {
                MZ_ID = gv_resultat.DataKeys[gv_resultat.SelectedIndex]["MZ_ID"].ToStringNullSafe(),
                OVER_DAY = gv_resultat.DataKeys[gv_resultat.SelectedIndex]["OVER_DAY"].ToStringNullSafe(),
                OVERTIME_TYPE = gv_resultat.DataKeys[gv_resultat.SelectedIndex]["OVERTIME_TYPE"].ToStringNullSafe(),
                LOCK_FLAG = "N"
            };

            //取得加班時間(加班時間起訖、加班總時數)
            CFService.GetOverTimeRange(ref model);
            if (model.OVER_STIME == null && model.OVER_ETIME == null)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('當日無刷卡時間!');", true);
                return;
            }
            else if (model.OVER_STIME == null && model.OVER_ETIME != null)
            {
                model.OVER_STIME = model.OVER_ETIME;
            }
            else if (model.OVER_ETIME == null && model.OVER_STIME != null)
            {
                model.OVER_ETIME = model.OVER_STIME;
            }
            //職稱與每時金額
            if (txt_PAY_HOUR.Text.Trim() != "0"
                || txt_PAY_MIN.Text.Trim() != "0"
                || txt_SHIFT_HOUR.Text != "0"
                || txt_SHIFT_MIN.Text != "0"
                )
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('請將申請加班費時數歸零後再更新時間!');", true);
                return;
            }
            model.PAY_UNIT = CFService.GetPayUnit(model.MZ_ID);
            model.MZ_OCCC = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_OCCC FROM A_DLBASE WHERE MZ_ID = '" + model.MZ_ID + "'");

            //特殊:如果該筆加班資料的 OVERTIME_CHG:Y,代表加班時數被異動過,如果是這樣的話,只能夠異動PAY_UNIT欄位,其他都不準改
            //這個做法算是應急,建議還是給予獨立的按鈕來進行
            string OVERTIME_CHG = o_DBFactory.ABC_toTest.vExecSQL("SELECT OVERTIME_CHG FROM C_OVERTIME_BASE Where MZ_ID='" + model.MZ_ID + "' And OVER_DAY='" + model.OVER_DAY + "' And OVERTIME_TYPE='" + model.OVERTIME_TYPE + "' ");
            if (OVERTIME_CHG == "Y")
            {
                var otf=o_DBFactory.ABC_toTest;
                string SQL = @"
                Update C_OVERTIME_BASE 
                Set PAY_UNIT=" + model.PAY_UNIT + @" Where MZ_ID='" + model.MZ_ID + "' And OVER_DAY='" + model.OVER_DAY + "' And OVERTIME_TYPE='" + model.OVERTIME_TYPE + "' ";
                List<SqlParameter> parameters = new List<SqlParameter>();
                int result_coount = otf.ExecuteNonQuery(SQL, parameters.ToArray());

                if (result_coount >= 1)
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('更新完成(僅更新每小時金額)!');", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('更新失敗!');", true);
                }

            }
            //更新加班單
            else if (CFService.C_OVERTIME_BASE_Save(model, _query.TPMFION))
            {
                CFService.SynchronizeOverTime(model, _query.TPMFION);
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('更新完成!');", true);
            }

            //更新查詢頁面
            GetOverTime_Bind_GridView(_query);
            UpdatePanel1.Update();
        }
        /// <summary>
        /// 按鈕: 更新加班資料
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Update_Click(object sender, EventArgs e)
        {
            var last = this.LastDay.Value;
            string source_over_day = "";
            int acc_hour = 0, acc_min = 0;
            if (gv_resultat.SelectedIndex != -1)
            {
                // sam 2020709 應判斷所選的日期之日班時數
                //檢查申請加班費時數
                int rest_min = Convert.ToInt32(gv_resultat.SelectedRow.Cells[8].Text); //補休
                int surplus_hour = Convert.ToInt32(gv_resultat.SelectedRow.Cells[10].Text); //取得累積剩餘時
                int surplus_min = Convert.ToInt32(gv_resultat.SelectedRow.Cells[11].Text); //取得累積剩餘分
                int goalhour = Convert.ToInt32(gv_resultat.SelectedRow.Cells[9].Text); //申請敘獎時數
                int pay_hour = Convert.ToInt32(txt_PAY_HOUR.Text);                     //申請業務加班時數
                int pay_min = Convert.ToInt32(txt_PAY_MIN.Text);                       //申請業務加班分鐘數
                int shift_hour = txt_SHIFT_HOUR.Text.SafeToInt();                     //申請輪值加班時數
                int shift_min = txt_SHIFT_MIN.Text.SafeToInt();                      //申請輪值加班分鐘數
                int over_hour = Convert.ToInt32(txt_OVER_HOUR.Text);                    //加班時數
                int over_min = Convert.ToInt32(txt_OVER_MINUTE.Text);                   //加班分鐘數


                if (last == "last")
                {
                    // 當日加班時數不足，是否為本月最後一筆加班資料？
                    // 應取總累積時數 與 欲申請時數相比
                    //先更新掉其他的本月已經結餘的部分
                    //string sql = "update C_Overtime_Base set where ";

                    surplus_hour = Convert.ToInt32(gv_resultat.FooterRow.Cells[8].Text); //取得累積剩餘時
                    if (surplus_hour > 0)
                    {
                        #region
                        //DataView view1 = new DataView();
                        //view1 = gv_resultat.DataSource as DataView;
                        // int allcount = gv_resultat.Rows.Count;// (資料總筆數)
                        // int oldhour = 0, delhour = 0;
                        // string tmpdate = "";
                        //for (int i = 0; i < allcount; i++)
                        // {
                        //     oldhour = int.Parse(gv_resultat.Rows[i].Cells[3].Text);
                        //     delhour = int.Parse(gv_resultat.Rows[i].Cells[6].Text);
                        //     delhour += int.Parse(gv_resultat.Rows[i].Cells[7].Text);
                        //     delhour += int.Parse(gv_resultat.Rows[i].Cells[8].Text);
                        //     if(oldhour> delhour)
                        //     {
                        //         acc_hour += oldhour - delhour;
                        //         tmpdate = gv_resultat.Rows[i].Cells[1].Text.Substring(0, 9);                           }

                        //     if(int.Parse(gv_resultat.Rows[i].Cells[4].Text)>0|| tmpdate=="")
                        //     {
                        //         tmpdate = gv_resultat.Rows[i].Cells[1].Text.Substring(0, 9) ;
                        //     }
                        //     if (tmpdate != "") { 
                        //     source_over_day += tmpdate + ";";
                        //     }
                        //     acc_min += int.Parse(gv_resultat.Rows[i].Cells[4].Text);

                        // }

                        // if (acc_min >= 60)
                        // {
                        //     acc_min = (int)(acc_min / 60) * 60;
                        // }
                        #endregion
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('剩餘時數不足，無法申請加班費!');", true);
                        return;
                    }
                }

                this.LastDay.Value = string.Empty;

                //Mark 20210413 取消
                //if (pay_hour > surplus_hour)
                //{
                //    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('剩餘時數不足，無法申請加班費!');", true);
                //    return;
                //}

                C_OVERTIME_BASE_Model upModel = new C_OVERTIME_BASE_Model()
                {
                    MZ_ID = gv_resultat.DataKeys[gv_resultat.SelectedIndex]["MZ_ID"].ToStringNullSafe(),
                    OVER_DAY = gv_resultat.DataKeys[gv_resultat.SelectedIndex]["OVER_DAY"].ToStringNullSafe(),
                    OVERTIME_TYPE = gv_resultat.DataKeys[gv_resultat.SelectedIndex]["OVERTIME_TYPE"].ToStringNullSafe()
                };
                string overTime_Type = gv_resultat.DataKeys[gv_resultat.SelectedIndex]["OVERTIME_TYPE"].ToStringNullSafe(); //取得加班類型

                //if(overTime_Type == "OTB" || overTime_Type == "OTT")
                //{
                //    int toalo = (Convert.ToInt32(txt_PAY_HOUR.Text) * 60) + Convert.ToInt32(txt_PAY_MIN.Text);
                //    if (rdoptype.SelectedValue == "0")
                //    {
                //        string strDay = txt_OVER_DAY.Text;
                //        if (toalo > 240)
                //        {
                //            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('除專案加班外，平日加班費申請不得超過4小時！');", true);
                //            return;
                //        }
                //    }
                //}

                //判斷是否為修改時數(為避免需修改事由，修改時數必須變更備註內容)
                string over_remark = gv_resultat.SelectedRow.Cells[5].Text.Replace("&nbsp;", "");
                upModel.OPTYPE = rdoptype.SelectedValue;
                // upModel.ACC_TYPE = rdacc_type.SelectedValue.ToString();
                // upModel.ACC_OTHERHOUR = acc_hour * 60 + acc_min;
                // upModel.ACC_OTHERDAY = source_over_day;

                string overTimeSN = gv_resultat.DataKeys[gv_resultat.SelectedIndex]["SN"].ToStringNullSafe();

                //介面填寫的值
                int allhour = int.Parse(txt_OVER_HOUR.Text) * 60 + int.Parse(txt_OVER_MINUTE.Text);

                //資料目前存的值
                int OVER_TOTAL = 0;
                int Payhour = 0;
                string OVER_TOTALstr = o_DBFactory.ABC_toTest.Get_First_Field("Select OVER_TOTAL From C_OVERTIME_BASE Where SN='" + overTimeSN + "'", null);
                string tmppay = o_DBFactory.ABC_toTest.Get_First_Field("Select PAY_HOUR From C_OVERTIME_BASE Where SN='" + overTimeSN + "'", null);
                if (!string.IsNullOrEmpty(OVER_TOTALstr))
                {
                    OVER_TOTAL = int.Parse(OVER_TOTALstr);
                }
                if (!string.IsNullOrEmpty(tmppay))
                {
                    Payhour = int.Parse(tmppay);
                }

                //職稱
                string stroccc = o_DBFactory.ABC_toTest.vExecSQL("select MZ_OCCC from C_OVERTIME_BASE where MZ_ID = '" + gv_resultat.DataKeys[gv_resultat.SelectedIndex]["MZ_ID"].ToStringNullSafe() + "'");
                //如果介面填寫的加班時數,不等於資料目前儲存的加班時數
                if (allhour != OVER_TOTAL)
                {
                    //如果 申請加班費有值,先提示使用者歸0,避免當日核算的加班時數與加班費有異常
                    if (pay_hour != 0 || pay_min != 0 || shift_hour != 0 || shift_min != 0)
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('因為異動【加班時數】，請先將【申請加班費時數】歸0!');", true);
                    }

                    //如果 備註 欄位有寫東西的話,沒有會跳訊息
                    if (string.IsNullOrEmpty(txt_OVER_REMARK.Text))
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('有異動加班時數，備註必填!');", true);
                        return;
                    }

                    //如果 還沒有選擇預定要送審的人員
                    if (this.HiddenField_REVIEW_ID_LEVEL_5.Value.IsNullOrEmpty())
                    {
                        //先讀取並且開啟人事室人員視窗
                        //開啟簽核選單
                        mpe_OnlienPeople_Level5.Show();
                        return;
                    }

                    //如果 備註 欄位有寫東西的話,沒有會跳訊息
                    //if (!string.IsNullOrEmpty(txt_OVER_REMARK.Text))
                    //{
                    //申請修改時數
                    upModel.OVER_REMARK = txt_OVER_REMARK.Text;
                    upModel.OVERTIME_CHG_TOTAL = (Convert.ToInt32(txt_OVER_HOUR.Text) * 60) + Convert.ToInt32(txt_OVER_MINUTE.Text);
                    upModel.REASON = txt_REASON.Text;
                    upModel.LOCK_FLAG = "Y";
                    //備註才能修改時數
                    //upModel.PAY_HOUR = pay_hour;
                    //upModel.PAY_HOUR = (Convert.ToInt32(txt_PAY_HOUR.Text) * 60) + Convert.ToInt32(txt_PAY_MIN.Text); ;
                    upModel.PAY_HOUR = 0;//申請的加業務班費時數先歸0
                    upModel.SHIFT_HOUR = 0;//申請的輪值加班費時數先歸0
                                           //根據員警身分證號 重新計算每小時金額
                                           //upModel.PAY_UNIT = CFService.GetPayUnit(upModel.MZ_ID);
                                           //依照申請類型調整流程
                    switch (overTime_Type)
                    {
                        case "OTB": //業務加班
                            #region 檢核是否為免簽主管
                            _query.MZ_ID = upModel.MZ_ID;
                            DataTable excardDt = CFService.GetOVERTIME_EXCARD(_query);
                            if (excardDt.Rows.Count > 0 && excardDt.Rows[0]["IS_MANAGER"].ToStringNullSafe() == "Y")
                            {
                                upModel.LOCK_FLAG = "N";
                            }
                            #endregion
                            break;
                        case "OTD": //值日補休
                        case "OTR": //特殊輪值
                        case "OTT": //其他
                                    //目前無特殊處理
                            break;
                        case "OTU": //超勤補休
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('無法修改超勤補休，請透過「6.1 超勤時數輸入」修改！');", true);

                            //更新頁面
                            GetOverTime_Bind_GridView(_query);
                            UpdatePanel1.Update();
                            return;
                    }

                    //更新加班資料並開啟人事送簽
                    if (CFService.C_OVERTIME_BASE_Save(upModel, _query.TPMFION))
                    {
                        //開啟附件上傳視窗
                        mpe_OnlienUp.Show();
                        return;
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('更新加班資料錯誤!');", true);
                    }
                    //}
                    //else
                    //{
                    //    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('有異動加班時數，備註必填!');", true);
                    //    return;
                    //}
                }
                else
                {   //加班時數沒有異動 只是修改其他欄位內容

                    //申請加班費、修改加班事由
                    int pay_unit = Convert.ToInt32(lbl_PAY_UNIT.Text);
                    upModel.REASON = txt_REASON.Text;

                    switch (overTime_Type)
                    {
                        case "OTB": //業務加班
                        case "OTR": //特殊輪值
                        case "OTT": //其他
                                    //限制單日最多4H，單月20H , 假日可以報 8 小時
                            string holiday = "";
                            int hour1 = 0;
                            //判斷假日
                            string sql1 = "select MZ_Holiday_Date from C_DUTYHOLIDAY where MZ_HOLIDAY_DATE = '" + upModel.OVER_DAY + "'";
                            //判斷補班課
                            string strSPRINGDAY = o_DBFactory.ABC_toTest.vExecSQL("select MZ_SPRING_NAME FROM C_SPRINGDAY where MZ_SPRING_DATE = '" + upModel.OVER_DAY + "'");
                            holiday = o_DBFactory.ABC_toTest.vExecSQL(sql1);
                            if (holiday != "")
                            {
                                holiday = "例假日";
                            }
                            //如果 加班時分數 小於 (剩餘時數 + 申請敘獎時數 + 申請業務加班費時數 + 申請輪值加班時數 ) 
                            //也就是你的加班費申請超過了
                            if ((over_hour * 60 + over_min) < (rest_min + goalhour * 60 + pay_hour * 60 + pay_min + shift_hour * 60 + shift_min))
                            {
                                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('超過可申請時數或分鐘數!');", true);
                                return;
                            }


                            string tmpdate = (int.Parse(upModel.OVER_DAY.Substring(0, 3)) + 1911).ToString() + "/" + upModel.OVER_DAY.Substring(3, 2) + "/" + upModel.OVER_DAY.Substring(5, 2);
                            DateTime dt = DateTime.Parse(tmpdate);
                            //holiday = tmpdate.DayOfWeek.ToString();
                            if (dt.DayOfWeek.ToString() == "Saturday" || dt.DayOfWeek.ToString() == "Sunday")
                            {
                                if (strSPRINGDAY == "")
                                {
                                    holiday = "例假日";
                                }
                            }
                            //申請加班費的總分鐘數,業務加班+輪值加班
                            int ipaytotal = pay_hour * 60 + pay_min + shift_hour * 60 + shift_min;

                            if (holiday == "例假日")
                            {
                                hour1 = 480;
                                //如果假日申請的加班費超過480分鐘,也就是8小時,則阻擋之
                                if (ipaytotal > hour1 && rdoptype.SelectedValue == "0")
                                {
                                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('除專案加班外，假日加班費申請不得超過8小時');", true);
                                    return;
                                }
                            }
                            else
                            {
                                hour1 = 240;
                                //如果平日申請的加班費超過240分鐘,也就是4小時,則阻擋之
                                if (ipaytotal > hour1 && rdoptype.SelectedValue == "0")
                                {
                                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('除專案加班外，平日加班費申請不得超過4小時！');", true);
                                    return;
                                }
                            }

                            DataTable payMonth = CFService.GetMonthPay(_query);
                            if (payMonth != null && payMonth.Rows.Count > 0)
                            {
                                //if (int.Parse(payMonth.Rows[0]["PAY_HOUR_TOTAL"].ToStringNullSafe()) > 1200)
                                //{
                                //    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('單月最多申請20小時!');", true);
                                //    return;
                                //}
                            }
                            //計算預計要寫入DB欄位的實際數字,這張表以分鐘數為主
                            //業務加班費申請分鐘數
                            upModel.PAY_HOUR = pay_hour * 60 + int.Parse(txt_PAY_MIN.Text);
                            upModel.SHIFT_HOUR = shift_hour * 60 + shift_min;
                            //總共申請的加班費=申請業務加班費時數*每小時金額+申請輪值加班費時數*每小時金額
                            /*
                            其實PAY_SUM這個DB欄位並不會實際被使用到
                            因為這樣計算等於是不考慮畸零未滿小時的分鐘數
                            後續月結算加班費的時候,申請的時分數才會進行加總計算真正的加班費
                            EX: 今天加班1.5小時,明天加班1.5小時 ,該欄位每天只算出1小時加班費,但月結算之後卻是3小時加班費
                            屆時請參考C_OVERTIMEMONTH_HOUR資料表
                             */
                            upModel.PAY_SUM = (pay_hour + shift_hour) * pay_unit;

                            upModel.OPTYPE = rdoptype.SelectedValue;

                            //   upModel.ACC_TYPE = rdacc_type.SelectedValue.ToString();
                            //upModel.ACC_OTHERHOUR = acc_hour * 60 + acc_min;
                            //   upModel.ACC_OTHERDAY = source_over_day;

                            break;
                        case "OTD": //值日補休
                        case "OTU": //超勤補休
                            if (pay_unit > 0)
                            {
                                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('該類型無法申請加班費！');", true);
                            }
                            break;
                    }


                    //更新加班資料及同步剩餘時數
                    if (CFService.C_OVERTIME_BASE_Save(new List<C_OVERTIME_BASE_Model>() { upModel }, _query.TPMFION) &&
                        CFService.SynchronizeOverTime(upModel, _query.TPMFION))
                    {

                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('已送出資料!');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('更新加班資料錯誤!');", true);
                    }
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('請先選擇加班單!');", true);
            }

            //更新查詢頁面
            GetOverTime_Bind_GridView(_query);
            UpdatePanel1.Update();
        }

        //20210803
        //新增加般單     加班類型:假日補休  要上傳檔案 傳SN用
        private void Getfileup_GridView(string seqNo)
        {
            HiddenOVERSN.Value = seqNo;
            mpe_OnlienUp.Show();
        }

        /// <summary>
        /// 按鈕: 確定上傳附件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_OnlineUp_OK_Click(object sender, EventArgs e)
        {
            //20210803

            //加班資料的SN,值日補休才會有值
            string OVERSN = HiddenOVERSN.Value;
            string OVER_SN = "", SEND_MZ_ID = "";
            //預定的審核者
            string REVIEW_ID = "";
            string REVIEWER_NAME = "";
            //判斷現在這段是要申請變更加班時數? 還是申請值日補休?
            //當初設計有點障礙,兩種申請都會跑來這邊上傳附件,只能透過HiddenOVERSN有沒有數值來判斷是何者?
            bool isChangeOverTime = (string.IsNullOrEmpty(OVERSN));
            //如果 沒有指定 OVERSN,代表非值日補休
            if (isChangeOverTime)
            {
                //清單上面也沒有點選嗎?
                if (gv_resultat.SelectedIndex == -1)
                {   //沒有選擇加班單
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('請先選擇加班單!');", true);
                    return;
                }
                else
                {
                    //根據清單上面點選的資料,帶入OVER_SN
                    OVER_SN = gv_resultat.DataKeys[gv_resultat.SelectedIndex]["SN"].ToStringNullSafe();
                    SEND_MZ_ID = gv_resultat.DataKeys[gv_resultat.SelectedIndex]["MZ_ID"].ToStringNullSafe();
                }

                //綁定審核者
                REVIEW_ID = this.HiddenField_REVIEW_ID_LEVEL_5.Value.ToStringNullSafe();
                REVIEWER_NAME = this.Label_Reviewer.Text;
            }
            else
            {   //反之,可能是 值日補休
                OVER_SN = OVERSN;
                //從加班單反查是誰提的申請單?
                SEND_MZ_ID = o_DBFactory.ABC_toTest.Get_First_Field(string.Format("SELECT INS_ID FROM C_OVERTIME_BASE WHERE SN='{0}'", OVERSN), null);
                //尋找簽核者的層級4,代表人事室人員
                //REVIEW_LEVEL = "4";
                //綁定審核者
                REVIEW_ID = this.HiddenField_REVIEW_ID_LEVEL_4.Value.ToStringNullSafe();
                REVIEWER_NAME = this.Label_Reviewer_OTD.Text;
            }

            //找出此次流程設定資料,所屬的SN,其實這邊固定會拿到4
            string SCHEDULE_SN = OWService.Get_OVERTIME_SCHEDULE_SN(1, "OLWU");

            #region 如果REVIEW_ID沒有抓到審核人員,但是正常不該發生
            if (REVIEW_ID.IsNullOrEmpty())
            {
                string REVIEW_LEVEL = "5";/*層級預設為5,代表*/
                if (isChangeOverTime == false)
                {   //不是更改加班時數,就是申請值日補休
                    REVIEW_LEVEL = "4";
                }
                //取得 C_LEAVE_SCHEDULE_CODE設定之單位人員
                Online_work_Query query = new Online_work_Query()
                {
                    OVERTIME_TYPE = "OLWU",//加班資料更新
                    SCHEDULE_SORT = "1",//申請流程的第1關,在本功能應該都是第1關,後面的關卡要去Online_work.aspx處理
                    REVIEW_LEVEL = REVIEW_LEVEL, //線上簽核-加班承辦人(人事室)
                    MZ_AD = o_DBFactory.ABC_toTest.Get_First_Field(string.Format("SELECT MZ_EXAD FROM A_DLBASE WHERE MZ_ID='{0}'", _query.MZ_ID), null),
                    MZ_UNIT = o_DBFactory.ABC_toTest.Get_First_Field(string.Format("SELECT MZ_EXUNIT FROM A_DLBASE WHERE MZ_ID='{0}'", _query.MZ_ID), null)
                };
                //如果是 因為更改加班時數,需要設定審核者

                //取得加班線上簽核人員資料
                DataTable dt = OWService.GetOverTimeOnlineReview(query);
                REVIEW_ID = dt.Rows[0]["MZ_ID"].ToStringNullSafe();
            }
            #endregion


            //如果抓得到簽核人員
            if (REVIEW_ID.IsNullOrEmpty() == false)
            {

                List<C_OVERTIME_CHANGE_FILE_Model> models = new List<C_OVERTIME_CHANGE_FILE_Model>();
                string msg = string.Empty;
                string errorMsg = string.Empty;

                #region 處理附件上傳
                List<FileUpload> fus = new List<FileUpload>() { FileUpload1, FileUpload2, FileUpload3 };
                foreach (FileUpload item in fus)
                {
                    if (item.HasFile)
                    {
                        if (CheckUpFile(item, ref msg))
                        {
                            models.Add(new C_OVERTIME_CHANGE_FILE_Model()
                            {
                                OVERTIME_SN = Convert.ToInt32(OVER_SN),
                                FILE_URL = msg,
                                ADD_ID = _query.MZ_ID
                            });
                        }
                        else
                        {
                            errorMsg += string.Format("附件{0}上傳失敗，{1}。", item.ID.SubstringOutToEmpty(item.ID.Length), msg);
                        }
                    }
                }
                #endregion

                if (!string.IsNullOrEmpty(errorMsg))
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", string.Format("alert('{0}!');", errorMsg), true);
                }

                if (models.Count > 0)
                {
                    #region 更新 新加班單 簽核決行主管


                    string UstrSQL = @"Update C_OVERTIME_BASE Set REVIEW_ID=@REVIEW_ID
                                    Where SN=@SN ";
                    List<SqlParameter> parameters = new List<SqlParameter>()
                        {
                            new SqlParameter("REVIEW_ID", REVIEW_ID),
                            new SqlParameter("SN", OVER_SN)
                        };

                    var otf=o_DBFactory.ABC_toTest;
                    otf.MultiSql_Add(UstrSQL, parameters.ToArray());
                    otf.MultiSQL_Exec();

                    #endregion

                    //刪除舊附件，儲存異動附件資料
                    if (CFService.C_OVERTIME_CHANGE_FILE_Delete(models.FirstOrDefault(), _query.TPMFION) && CFService.C_OVERTIME_CHANGE_FILE_Save(models, _query.TPMFION))
                    {
                        C_OVERTIME_HISTORY_Model addModel = new C_OVERTIME_HISTORY_Model()
                        {
                            OVERTIME_SN = Convert.ToInt32(OVER_SN),
                            REVIEW_ID = REVIEW_ID,
                            LETTER_DATE = ForDateTime.RCDateToTWDate(DateTime.Now.ToString("yyyy/MM/dd"), "yyyMMdd"),
                            OVERTIME_SCHEDULE_SN = Convert.ToInt32(SCHEDULE_SN),
                            LETTER_TIME = DateTime.Now.ToString("HH:mm:ss"),
                            PROCESS_STATUS = o_DBFactory.ABC_toTest.Get_First_Field("Select C_STATUS_SN From C_STATUS Where C_STATUS_NAME='待審中'", null),
                            OVERTIME_TYPE = "OLWU",
                            SEND_ID = SEND_MZ_ID
                        };

                        if (OWService.C_OVERTIME_HISTORY_Save(addModel, _query.TPMFION))
                        {
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('已送出資料!陳核人員:" + REVIEWER_NAME + "');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('新增線上簽核失敗!');", true);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('儲存上傳資訊失敗!');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('請選擇上傳附件!');", true);
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('無加班承辦人!');", true);
            }

            HiddenOVERSN.Value = "";
            //更新查詢頁面
            GetOverTime_Bind_GridView(_query);
            UpdatePanel1.Update();
            mpe_OnlienUp.Hide();
        }
        /// <summary>
        /// 按鈕: 刪除加班資料
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Delete_Click(object sender, EventArgs e)
        {
            if (gv_resultat.SelectedIndex != -1)
            {
                C_OVERTIME_BASE_Model delModel = new C_OVERTIME_BASE_Model()
                {
                    MZ_ID = gv_resultat.DataKeys[gv_resultat.SelectedIndex]["MZ_ID"].ToStringNullSafe(),
                    OVER_DAY = gv_resultat.DataKeys[gv_resultat.SelectedIndex]["OVER_DAY"].ToStringNullSafe(),
                    SN = Convert.ToInt32(gv_resultat.DataKeys[gv_resultat.SelectedIndex]["SN"].ToStringNullSafe())
                };

                string msg = CFService.C_OVERTIME_BASE_ALL_Delete(delModel, _query.TPMFION);
                if (!string.IsNullOrEmpty(msg))
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", string.Format("alert('{0}');", msg), true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('加班資料刪除完成!');", true);
                }

                //更新查詢頁面
                GetOverTime_Bind_GridView(_query);
                UpdatePanel1.Update();
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('請先選擇加班單!');", true);
            }
        }
        /// <summary>
        /// 按鈕: 重新發送簽核
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_ReOnline_Click(object sender, EventArgs e)
        {
            if (gv_resultat.SelectedIndex != -1)
            {
                //未選陳核主管、遭主管退回
                string lock_flag = gv_resultat.DataKeys[gv_resultat.SelectedIndex]["LOCK_FLAG"].ToStringNullSafe(); //資料鎖定
                string over_status = gv_resultat.DataKeys[gv_resultat.SelectedIndex]["OVER_STATUS"].ToStringNullSafe(); //線上審核最新狀態
                string is_sign_return = gv_resultat.DataKeys[gv_resultat.SelectedIndex]["IS_SIGN_RETURN"].ToStringNullSafe(); //是否簽核退回
                string OVERTIME_TYPE = gv_resultat.DataKeys[gv_resultat.SelectedIndex]["OVERTIME_TYPE"].ToStringNullSafe(); //加班類型
                string MZ_ID = gv_resultat.DataKeys[gv_resultat.SelectedIndex]["MZ_ID"].ToStringNullSafe();
                string OVER_DAY = gv_resultat.DataKeys[gv_resultat.SelectedIndex]["OVER_DAY"].ToStringNullSafe();
                string strSQL = "";
                var otf=o_DBFactory.ABC_toTest;
                List<SqlParameter> parameters;

                //備註欄清空
                strSQL = @"Update C_OVERTIME_BASE Set OVER_REMARK = '' , OVER_TOTAL = '0',SURPLUS_TOTAL = '0', OVERTIME_CHG = 'N', OVERTIME_CHG_TOTAL = '0' 
                                    ,PAY_HOUR = '0',PAY_SUM = '0'
                                    Where MZ_ID=@MZ_ID And OVER_DAY=@OVER_DAY And OVERTIME_TYPE=@OVERTIME_TYPE ";
                parameters = new List<SqlParameter>()
                        {
                            new SqlParameter("MZ_ID",MZ_ID ),
                            new SqlParameter("OVER_DAY", OVER_DAY),
                            new SqlParameter("OVERTIME_TYPE", OVERTIME_TYPE),
                        };
                otf.MultiSql_Add(strSQL, parameters.ToArray());
                otf.MultiSQL_Exec();

                switch (OVERTIME_TYPE)
                {
                    case "OTD": //值日補休

                        Getfileup_GridView(gv_resultat.DataKeys[gv_resultat.SelectedIndex]["SN"].ToStringNullSafe());

                        break;
                    default:
                        #region 申請單流程重送
                        //開啟簽核選單
                        mpe_OnlienPeople.Show();
                        //設定query
                        Online_work_Query query = new Online_work_Query()
                        {
                            OVERTIME_TYPE = "OLWA",
                            SCHEDULE_SORT = "1",//申請流程的第1關,在本功能應該都是第1關,後面的關卡要去Online_work.aspx處理
                            REVIEW_LEVEL = "2",
                            MZ_AD = gv_resultat.DataKeys[gv_resultat.SelectedIndex]["MZ_EXAD"].ToStringNullSafe(),
                            MZ_UNIT = gv_resultat.DataKeys[gv_resultat.SelectedIndex]["MZ_EXUNIT"].ToStringNullSafe()
                        };
                        //設定可選主管列表
                        GetOnlineCheckManager_Bind_GridView(query);
                        #endregion

                        break;
                }

                #region
                //if ((lock_flag == "N" && string.IsNullOrEmpty(over_status)) || (lock_flag == "N" && is_sign_return == "Y"))
                //{       //備註欄清空
                //    strSQL = @"Update C_OVERTIME_BASE Set OVER_REMARK = ''
                //                    Where MZ_ID=@MZ_ID And OVER_DAY=@OVER_DAY And OVERTIME_TYPE=@OVERTIME_TYPE ";
                //    parameters = new List<SqlParameter>()
                //        {
                //            new SqlParameter("MZ_ID",MZ_ID ),
                //            new SqlParameter("OVER_DAY", OVER_DAY),
                //            new SqlParameter("OVERTIME_TYPE", OVERTIME_TYPE),
                //        };
                //    otf.MultiSql_Add(strSQL, parameters.ToArray());
                //    otf.MultiSQL_Exec();

                //    switch (OVERTIME_TYPE)
                //    {
                //        case "OTD": //值日補休

                //            Getfileup_GridView(gv_resultat.DataKeys[gv_resultat.SelectedIndex]["SN"].ToStringNullSafe());

                //            break;
                //        default:
                //            #region 申請單流程重送
                //            //開啟簽核選單
                //            mpe_OnlienPeople.Show();
                //            //設定query
                //            Online_work_Query query = new Online_work_Query()
                //            {
                //                OVERTIME_TYPE = "OLWA",
                //                SCHEDULE_SORT = "1",
                //                REVIEW_LEVEL = "2",
                //                MZ_AD = gv_resultat.DataKeys[gv_resultat.SelectedIndex]["MZ_EXAD"].ToStringNullSafe(),
                //                MZ_UNIT = gv_resultat.DataKeys[gv_resultat.SelectedIndex]["MZ_EXUNIT"].ToStringNullSafe()
                //            };
                //            //設定可選主管列表
                //            GetOnlineCheckManager_Bind_GridView(query);
                //            #endregion

                //            break;
                //    }




                //}
                //else if (lock_flag == "Y")  //待審中未選到簽核人員
                //{   //備註欄清空
                //    strSQL = @"Update C_OVERTIME_BASE Set OVER_REMARK = ''
                //                    Where MZ_ID=@MZ_ID And OVER_DAY=@OVER_DAY And OVERTIME_TYPE=@OVERTIME_TYPE ";
                //    parameters = new List<SqlParameter>()
                //        {
                //            new SqlParameter("MZ_ID",MZ_ID ),
                //            new SqlParameter("OVER_DAY", OVER_DAY),
                //            new SqlParameter("OVERTIME_TYPE", OVERTIME_TYPE),
                //        };
                //    otf.MultiSql_Add(strSQL, parameters.ToArray());
                //    otf.MultiSQL_Exec();

                //    #region 重新開啟上傳檔案
                //    txt_OVER_REMARK.Text = "";
                //    switch (OVERTIME_TYPE)
                //    {
                //        case "OTD": //值日補休

                //            Getfileup_GridView(gv_resultat.DataKeys[gv_resultat.SelectedIndex]["SN"].ToStringNullSafe());

                //            break;
                //        default:
                //            #region 申請單流程重送
                //            //開啟簽核選單
                //            mpe_OnlienPeople.Show();
                //            //設定query
                //            Online_work_Query query = new Online_work_Query()
                //            {
                //                OVERTIME_TYPE = "OLWA",
                //                SCHEDULE_SORT = "1",
                //                REVIEW_LEVEL = "2",
                //                MZ_AD = gv_resultat.DataKeys[gv_resultat.SelectedIndex]["MZ_EXAD"].ToStringNullSafe(),
                //                MZ_UNIT = gv_resultat.DataKeys[gv_resultat.SelectedIndex]["MZ_EXUNIT"].ToStringNullSafe()
                //            };
                //            //設定可選主管列表
                //            GetOnlineCheckManager_Bind_GridView(query);
                //            #endregion

                //            break;
                //    }
                //    #endregion
                //}
                //else
                //{
                //    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('無法重新發送簽核!');", true);
                //}
                #endregion
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('請先選擇加班單!');", true);
            }

        }
        /// <summary>
        /// 按鈕: 列印加班請示單
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btOvertimeAsk_Click(object sender, EventArgs e)
        {
            if (ddl_Query_Name.SelectedIndex != -1 && !string.IsNullOrEmpty(txt_OVER_DAY.Text))
            {
                Session["RPT_C_IDNO"] = ddl_Query_Name.SelectedValue;
                Session["RPT_C_DATE"] = txt_OVER_DAY.Text.Replace("/", "").PadLeft(7, '0');
                string tmp_url = string.Format("C_rpt.aspx?fn=OvertimeInsideAsk_New&TPM_FION={0}", _TPM_FION);

                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "location.href('C_OvertimeInsideAsk_rpt_New.aspx?TPM_FION=" + _TPM_FION + "');", true);
            }
        }
        /// <summary>
        /// 按鈕: 加班費管制卡
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btOvertimeDetail_Click(object sender, EventArgs e)
        {
            Response.Redirect("C_OvertimeInsideDetail_rpt_New.aspx?TPM_FION=" + _TPM_FION);
        }
        /// <summary>
        /// 按鈕: 加班費總表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btOvertimeTotal_Click(object sender, EventArgs e)
        {
            Response.Redirect("C_OvertimeInsideTotal_rpt_New.aspx?TPM_FION=" + _TPM_FION);
        }


        /// <summary>
        /// 下拉選單: 查詢加班資料 機關
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
        /// 下拉選單: 查詢人員資料
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddl_Query_Name_SelectedIndexChanged(object sender, EventArgs e)
        {
            //選擇查詢人員ID
            _query.MZ_ID = ddl_Query_Name.SelectedValue;

            //重讀取人事室人員()
            讀取人事室人員_All();

            //查詢加班資料
            GetOverTime_Bind_GridView(_query);
        }
        /// <summary>
        /// 下拉選單: 加班類型切換
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddl_OVERTIME_TYPE_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_OVERTIME_TYPE.SelectedItem.Text == "值日補休")
            {
                //值日補休申請直接輸入時數
                //加 班 時 數(時)(分) - 啟用
                txt_OVER_HOUR.Enabled = true;
                txt_OVER_MINUTE.Enabled = true;
            }
            else if (ddl_OVERTIME_TYPE.SelectedItem.Text == "超勤補休")
            {
                //新增按鈕 - 鎖定
                btn_Insert.Enabled = false;
            }
            else
            {
                //如果 目前尚未選擇任何 加班類型
                if (gv_resultat.SelectedIndex == -1)
                {
                    //加班時數(時)(分) - 鎖定
                    txt_OVER_HOUR.Enabled = false;
                    txt_OVER_MINUTE.Enabled = false;
                }
            }

        }
        /// <summary>
        /// 輸入框: 加班日期格式變更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txt_OVER_DAY_TextChanged(object sender, EventArgs e)
        {
            string strRCDate = ForDateTime.TWDateToRCDate(txt_OVER_DAY.Text, "yyyy/MM/dd");

            if ((bool)ForDateTime.CheckDateTime(strRCDate))
            {
                txt_OVER_DAY.Text = ForDateTime.RCDateToTWDate(strRCDate, "yyy/MM/dd");
            }
            else
            {
                txt_OVER_DAY.Text = string.Empty;
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('日期格式錯誤!')", true);
            }
        }



        /// <summary>
        /// 數字輸入框變更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txt_IntNumTextChanged(object sender, EventArgs e)
        {
            //把觸發的物件轉型成 TextBox
            TextBox s = (TextBox)sender;
            //判斷文字框內輸入的是否為整數?否則將強制把裡面的文字轉換成0
            s.Text = s.Text.SafeToInt().ToString();
        }


        /// <summary>
        /// GridView: 加班資料Row建立觸發
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gv_resultat_RowCreated(object sender, GridViewRowEventArgs e)
        {
            GridView gv = sender as GridView;

            switch (e.Row.RowType)
            {
                case DataControlRowType.Header:
                    TableCellCollection tcHeader = e.Row.Cells;
                    tcHeader.Clear();

                    tcHeader.Add(new TableHeaderCell() { RowSpan = 2, Text = "加班日期", CssClass = "GV_Header" });
                    tcHeader.Add(new TableHeaderCell() { RowSpan = 2, Text = "加班事由", CssClass = "GV_Header" });
                    tcHeader.Add(new TableHeaderCell() { ColumnSpan = 3, Text = "加班時數", CssClass = "GV_Header" });
                    tcHeader.Add(new TableHeaderCell() { RowSpan = 2, Text = "加班費申請時", CssClass = "GV_Header" });
                    tcHeader.Add(new TableHeaderCell() { RowSpan = 2, Text = "加班費申請分", CssClass = "GV_Header" });
                    tcHeader.Add(new TableHeaderCell() { RowSpan = 2, Text = "已補休時數", CssClass = "GV_Header" });
                    tcHeader.Add(new TableHeaderCell() { RowSpan = 2, Text = "已申請敘獎時數", CssClass = "GV_Header" });
                    tcHeader.Add(new TableHeaderCell() { ColumnSpan = 2, Text = "累積剩餘時數", CssClass = "GV_Header" });
                    //tcHeader.Add(new TableHeaderCell() { RowSpan = 2, Text = "結算餘時", CssClass = "GV_Header" });
                    tcHeader.Add(new TableHeaderCell() { RowSpan = 2, Text = "狀態", CssClass = "GV_Header" });
                    tcHeader.Add(new TableHeaderCell() { RowSpan = 2, Text = "專案加班</th></tr><tr>", CssClass = "GV_Header" });
                    tcHeader.Add(new TableHeaderCell() { Text = "時", CssClass = "GV_Header" });
                    tcHeader.Add(new TableHeaderCell() { Text = "分", CssClass = "GV_Header" });
                    tcHeader.Add(new TableHeaderCell() { Text = "備註(修改原因)", CssClass = "GV_Header" });
                    tcHeader.Add(new TableHeaderCell() { Text = "時", CssClass = "GV_Header" });
                    tcHeader.Add(new TableHeaderCell() { Text = "分</th></tr><tr>", CssClass = "GV_Header" });

                    break;
                case DataControlRowType.Footer:
                    break;
                case DataControlRowType.DataRow:
                    for (int i = 0; i < e.Row.Cells.Count; i++)
                    {
                        TableCell cell = e.Row.Cells[i];

                        cell.Attributes["onclick"] = string.Format("{0}", Page.ClientScript.GetPostBackClientHyperlink(gv, string.Format("Select${0}", e.Row.RowIndex)));
                    }
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
        /// <summary>
        /// GridView: 各列加班資料綁定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gv_resultat_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridView gv = sender as GridView;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //for (int i = 0; i < e.Row.Cells.Count; i++)
                //{
                //    if (i == 0 || i == 10)
                //    {
                //        e.Row.Cells[i].Attributes.Add("Style", "display:none");
                //    }
                //} 
                //issue_dave 增加顯示每月統計資料 20200806
                if (!string.IsNullOrEmpty(e.Row.Cells[4].Text))
                {
                    int cell_hour = Convert.ToInt32(e.Row.Cells[3].Text);
                    int cell_min = Convert.ToInt32(e.Row.Cells[4].Text);
                    gv_totmin += (cell_hour * 60) + cell_min;
                }
                if (!string.IsNullOrEmpty(e.Row.Cells[6].Text))
                {
                    MON_PAY_HOUR += Convert.ToInt32(e.Row.Cells[6].Text);
                }
                if (!string.IsNullOrEmpty(e.Row.Cells[7].Text))
                {
                    MON_PAY_MIN += Convert.ToInt32(e.Row.Cells[7].Text);
                }
                if (!string.IsNullOrEmpty(e.Row.Cells[8].Text))
                {
                    MON_REST_HOUR += Convert.ToInt32(e.Row.Cells[8].Text);
                }
                if (!string.IsNullOrEmpty(e.Row.Cells[9].Text))
                {
                    MON_PRIZE_HOUR += Convert.ToInt32(e.Row.Cells[9].Text);
                }
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                var month = txt_Search_YM.Text;
                e.Row.Cells[0].Text = month + "- 當月統計";
                //e.Row.Cells[1].Text = "當月統計";

                //issue_dave 增加顯示每月統計資料 20200817
                MON_OVER_HOUR = gv_totmin / 60;
                MON_OVER_MINUTE = gv_totmin % 60;
                e.Row.Cells[2].Text = MON_OVER_HOUR.ToString();         //統計加班時
                e.Row.Cells[3].Text = MON_OVER_MINUTE.ToString();       //統計加班分

                if (MON_PAY_MIN / 60 > 0) //判斷加班費分鐘數總和是否超過一小時
                {
                    e.Row.Cells[5].Text = (MON_PAY_HOUR + (MON_PAY_MIN / 60)).ToString();          //加班費
                    e.Row.Cells[6].Text = (MON_PAY_MIN % 60).ToString();          //加班費
                }
                else
                {
                    e.Row.Cells[5].Text = MON_PAY_HOUR.ToString();          //加班費
                    e.Row.Cells[6].Text = MON_PAY_MIN.ToString();          //加班費
                }
                e.Row.Cells[7].Text = MON_REST_HOUR.ToString();         //已補休
                e.Row.Cells[8].Text = MON_PRIZE_HOUR.ToString();        //敍奬

                // sam wellsince 20201027 取得 累積剩餘時數、分數
                var data = gv_resultat.DataSource;

                int 累積剩餘時數 = 0;
                int 累積剩餘分數 = 0;

                int 時數 = 0;
                int 分數 = 0;

                foreach (DataRow r in (data as DataTable).Rows)
                {
                    //int.TryParse(r["SURPLUS_TOTAL"].ToString(), out 分數);
                    int.TryParse(r["LEFT_HOUR"].ToString(), out 時數);
                    int.TryParse(r["LEFT_MIN"].ToString(), out 分數);

                    累積剩餘分數 += 時數 * 60 + 分數;
                }

                累積剩餘時數 = 累積剩餘分數 / 60;
                累積剩餘分數 = 累積剩餘分數 % 60;

                e.Row.Cells[9].Text = 累積剩餘時數.ToString();         //累積剩餘時數
                e.Row.Cells[10].Text = 累積剩餘分數.ToString();        //累積剩餘分數
            }
        }
        /// <summary>
        /// GridView: 加班資料列點擊
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gv_resultat_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView gv = sender as GridView;

            if (e.CommandName == "Select")
            {
                if (gv.SelectedIndex == Convert.ToInt32(e.CommandArgument))
                {
                    //點選同項目，重新取得加班資料來取消選擇
                    GetOverTime_Bind_GridView(_query);
                }
                else
                {
                    gv.SelectedIndex = Convert.ToInt32(e.CommandArgument);

                    //取得鎖定標籤
                    string lock_flag = gv.DataKeys[gv.SelectedIndex]["LOCK_FLAG"].ToStringNullSafe();
                    string tmpdate = gv.DataKeys[gv.SelectedIndex]["FORMAT_DAY"].ToStringNullSafe();
                    string overtype = gv.DataKeys[gv.SelectedIndex]["OVERTIME_TYPE"].ToStringNullSafe();
                    string status = o_DBFactory.ABC_toTest.vExecSQL("select MZ_VERIFY from C_OVERTIMEMONTH_HOUR where MZ_ID = '" + gv.DataKeys[gv.SelectedIndex]["MZ_ID"] + "' and MZ_YEAR = '" + tmpdate.Split('/')[0] + "' and MZ_MONTH = '" + tmpdate.Split('/')[1] + "'");
                    string paychk = o_DBFactory.ABC_toTest.vExecSQL("select PAY_CHK from C_OVERTIME_BASE where MZ_ID = '" + gv.DataKeys[gv.SelectedIndex]["MZ_ID"] + "' and OVER_DAY = '" + gv.DataKeys[gv.SelectedIndex]["OVER_DAY"].ToStringNullSafe() + "'");
                    if (!string.IsNullOrEmpty(lock_flag))
                    {
                        string over_status = gv.DataKeys[gv.SelectedIndex]["OVER_STATUS"].ToStringNullSafe(); //線上審核最新狀態
                        txt_OVER_STATUS.Text = over_status;
                        //將所有欄位開放
                        allOpen();

                        if (overtype == "OTB" || overtype == "OTT")  //業務加班、其他業務加班
                        {
                            /*         
                             特殊規則:
                             維護單1121201(多個問題之一)
                             問題： 配合輪值加班功能，原本新增和修改資料有設定檢核機制，
                             現在針對加班事由(REASON)首6個字為「輪值加班補登」的資料移除檢核讓它通過(未來會在關起來就是了) "

                            12/19 以下條件開放
                            17:27 If "修改"按鈕前端會反灰，讓這些資料前端可以點
                            */
                            //判斷是否符合首6個字為「輪值加班補登」
                            bool is輪值加班補登 = this.txt_REASON.Text.Contains("輪值加班補登");
                            //如果這筆資料已經被審核並產生加班費 且 不是輪值加班補登
                            if (paychk == "Y" && is輪值加班補登 == false)
                            { allClose(); }
                            else
                            {
                                switch (over_status)
                                {
                                    case "待審中":
                                        //加班日期
                                        txt_OVER_DAY.Enabled = false;
                                        //加班類型
                                        ddl_OVERTIME_TYPE.Enabled = false;
                                        //加班時、分
                                        txt_OVER_HOUR.Enabled = false;
                                        txt_OVER_MINUTE.Enabled = false;
                                        //鎖定加班費申請相關欄位
                                        SetEnabled_加班費申請相關欄位(false);
                                        //備註欄
                                        txt_OVER_REMARK.Enabled = false;
                                        //修改
                                        btn_Update.Enabled = false;
                                        //更新刷卡時間
                                        btn_ReGetTime.Enabled = false;
                                        esgstr.Visible = false;//退 回 原 因
                                        break;
                                    case "決行":
                                        //加班日期、加班類型
                                        txt_OVER_DAY.Enabled = false;
                                        ddl_OVERTIME_TYPE.Enabled = false;
                                        esgstr.Visible = false;//退 回 原 因
                                        break;
                                    case "退回":
                                        //加班日期、加班類型、加班時數時分、備註欄、修改
                                        txt_OVER_DAY.Enabled = false;
                                        ddl_OVERTIME_TYPE.Enabled = false;
                                        txt_OVER_HOUR.Enabled = false;
                                        txt_OVER_MINUTE.Enabled = false;
                                        txt_OVER_REMARK.Enabled = false;
                                        btn_Update.Enabled = false;
                                        esgstr.Visible = true;//退 回 原 因
                                        break;
                                    case "核定":
                                        //加班日期、加班事由、加班類型、加班時數(時)、加班時數(分)、備註欄、更新刷卡時間
                                        txt_OVER_DAY.Enabled = false;
                                        txt_REASON.Enabled = false;
                                        ddl_OVERTIME_TYPE.Enabled = false;
                                        txt_OVER_HOUR.Enabled = false;
                                        txt_OVER_MINUTE.Enabled = false;
                                        txt_OVER_REMARK.Enabled = false;
                                        esgstr.Visible = false;//退 回 原 因
                                        break;
                                    default:
                                        //加班類型、加班時分數、申請加班費時分數、備註欄、一般/專案加班、修改、更新刷卡時間
                                        ddl_OVERTIME_TYPE.Enabled = false;
                                        txt_OVER_HOUR.Enabled = false;
                                        txt_OVER_MINUTE.Enabled = false;
                                        //鎖定加班費申請相關欄位
                                        SetEnabled_加班費申請相關欄位(false);
                                        txt_OVER_REMARK.Enabled = false;
                                        rdoptype.Enabled = false;
                                        btn_Update.Enabled = false;
                                        btn_ReGetTime.Enabled = false;
                                        esgstr.Visible = false;//退 回 原 因
                                        break;
                                }
                            }


                        }
                        else if (overtype == "OTD")  //值日補休
                        {
                            if (paychk == "Y")
                            { allClose(); }
                            else
                            {
                                //鎖定加班費申請相關欄位
                                SetEnabled_加班費申請相關欄位(false);
                                //重新簽核按鈕 在 值日補休的時候 一律鎖定
                                btn_ReOnline.Enabled = false;
                                switch (over_status)
                                {
                                    case "待審中":
                                        //加班日期、加班類型、申請加班費時分、備註欄、修改、重新簽核、更新刷卡時間
                                        txt_OVER_DAY.Enabled = false;
                                        ddl_OVERTIME_TYPE.Enabled = false;
                                        //鎖定加班費申請相關欄位
                                        SetEnabled_加班費申請相關欄位(false);
                                        txt_OVER_REMARK.Enabled = false;
                                        btn_Update.Enabled = false;
                                        btn_ReGetTime.Enabled = false;
                                        esgstr.Visible = false;//退 回 原 因
                                        break;
                                    case "決行":
                                        //加班日期、加班類型、申請加班費時分、重新簽核
                                        txt_OVER_DAY.Enabled = false;
                                        ddl_OVERTIME_TYPE.Enabled = false;
                                        //鎖定加班費申請相關欄位
                                        SetEnabled_加班費申請相關欄位(false);
                                        esgstr.Visible = false;//退 回 原 因
                                        break;
                                    case "退回":
                                        //加班日期、加班類型、申請加班費時分數、備註欄、修改、更新刷卡時間
                                        txt_OVER_DAY.Enabled = false;
                                        ddl_OVERTIME_TYPE.Enabled = false;
                                        //鎖定加班費申請相關欄位
                                        SetEnabled_加班費申請相關欄位(false);
                                        txt_OVER_REMARK.Enabled = false;
                                        btn_Update.Enabled = false;
                                        btn_ReGetTime.Enabled = false;
                                        esgstr.Visible = true;//退 回 原 因
                                        break;
                                    case "核定":
                                        //加班日期、加班事由、加班類型、加班時分數、申請加班費時分數、備註欄
                                        txt_OVER_DAY.Enabled = false;
                                        txt_REASON.Enabled = false;
                                        ddl_OVERTIME_TYPE.Enabled = false;
                                        txt_OVER_HOUR.Enabled = false;
                                        txt_OVER_MINUTE.Enabled = false;
                                        //鎖定加班費申請相關欄位
                                        SetEnabled_加班費申請相關欄位(false);
                                        txt_OVER_REMARK.Enabled = false;
                                        esgstr.Visible = false;//退 回 原 因
                                        break;
                                    default:
                                        //加班類型、申請加班費時分數、備註欄、一般/專案加班、修改、更新刷卡時間
                                        ddl_OVERTIME_TYPE.Enabled = false;
                                        //鎖定加班費申請相關欄位
                                        SetEnabled_加班費申請相關欄位(false);
                                        txt_OVER_REMARK.Enabled = false;
                                        rdoptype.Enabled = false;
                                        btn_Update.Enabled = false;
                                        btn_ReGetTime.Enabled = false;
                                        esgstr.Visible = false;//退 回 原 因
                                        break;
                                }
                            }

                        }
                        else if (overtype == "OTU")  //超勤補休
                        {
                            //加班時、分
                            txt_OVER_HOUR.Enabled = false;
                            txt_OVER_MINUTE.Enabled = false;
                            //鎖定加班費申請相關欄位
                            SetEnabled_加班費申請相關欄位(false);
                            //備註欄
                            txt_OVER_REMARK.Enabled = false;
                            //一般專案
                            rdoptype.Enabled = false;
                            //修改
                            btn_Update.Enabled = false;
                            //更新刷卡
                            btn_ReGetTime.Enabled = false;
                            //加班類型
                            ddl_OVERTIME_TYPE.Enabled = false;
                            //重新簽核
                            btn_ReOnline.Enabled = false;
                            esgstr.Visible = false;//退 回 原 因

                        }

                        //加班日期
                        txt_OVER_DAY.Text = gv.DataKeys[gv.SelectedIndex]["FORMAT_DAY"].ToStringNullSafe();

                        //加班事由
                        txt_REASON.Text = gv.SelectedRow.Cells[2].Text.Replace("&nbsp;", "");

                        //加班時
                        txt_OVER_HOUR.Text = gv.SelectedRow.Cells[3].Text;

                        //加班分
                        txt_OVER_MINUTE.Text = gv.SelectedRow.Cells[4].Text;

                        //每時金額
                        string pay_unit = gv.DataKeys[gv.SelectedIndex]["PAY_UNIT"].ToStringNullSafe();
                        lbl_PAY_UNIT.Text = pay_unit ?? "0";

                        //申請加班費時數
                        //string pay_chk = gv.DataKeys[gv.SelectedIndex]["PAY_CHK"].ToStringNullSafe();
                        //txt_PAY_HOUR.Text = gv.SelectedRow.Cells[6].Text;
                        //txt_PAY_MIN.Text = gv.SelectedRow.Cells[7].Text;

                        //申請業務加班費時數
                        txt_PAY_HOUR.Text = (gv.SelectedRow.FindControl("PAY_HOUR") as HiddenField).Value;
                        txt_PAY_MIN.Text = (gv.SelectedRow.FindControl("PAY_MIN") as HiddenField).Value;
                        //申請輪值加班費時數
                        txt_SHIFT_HOUR.Text = (gv.SelectedRow.FindControl("SHIFT_HOUR") as HiddenField).Value;
                        txt_SHIFT_MIN.Text = (gv.SelectedRow.FindControl("SHIFT_MIN") as HiddenField).Value;

                        //備註
                        txt_OVER_REMARK.Text = gv.SelectedRow.Cells[5].Text.Replace("&nbsp;", "");
                        //txt_OVER_REMARK.Enabled = lock_flag == "N" ? true : false;

                        //加班類型
                        string overtime_type = gv.DataKeys[gv.SelectedIndex]["OVERTIME_TYPE"].ToStringNullSafe();
                        ddl_OVERTIME_TYPE.SelectedValue = overtime_type ?? "OTB"; //預設業務加班(OTB)
                        //ddl_OVERTIME_TYPE.Enabled = false;
                        //一般專案
                        rdoptype.SelectedValue = "0";
                        string optype = gv.SelectedRow.Cells[13].Text;

                        if (optype == "是")
                        {
                            rdoptype.SelectedValue = "1";
                        }
                        else
                        {
                            rdoptype.SelectedValue = "0";
                        }

                        string is_sign_return = gv.DataKeys[gv.SelectedIndex]["IS_SIGN_RETURN"].ToStringNullSafe(); //是否簽核退回
                        string over_remark = gv.SelectedRow.Cells[5].Text.Replace("&nbsp;", "");

                        _query.SearchYM = txt_OVER_DAY.Text.Replace("/", "").Replace("-", "").SubstringOutToEmpty(0, 5);

                        //退 回 原 因
                        string messagestr = o_DBFactory.ABC_toTest.vExecSQL("select review_message from C_OVERTIME_HISTORY where overtime_sn = '" + gv.DataKeys[gv.SelectedIndex]["SN"].ToStringNullSafe() + "' ORDER BY o_sn desc ");
                        review_message.Text = messagestr;

                    }
                    else
                    {
                        //無法判斷，關閉修改功能
                        btn_Update.Enabled = false;
                        btn_Delete.Enabled = false;
                        btn_ReOnline.Enabled = false;
                        btn_ReGetTime.Enabled = false;
                    }
                }
            }
        }

        /// <summary>
        /// 設定是否啟用加班費申請相關欄位,具體包括
        /// 申請業務加班費(時/分)
        /// 申請輪值加班費(時/分)
        /// </summary>
        /// <param name="Enabled">是否啟用</param>
        protected void SetEnabled_加班費申請相關欄位(bool Enabled)
        {
            txt_OVER_HOUR.Enabled = Enabled;
            txt_OVER_MINUTE.Enabled = Enabled;
            txt_SHIFT_HOUR.Enabled = Enabled;
            txt_SHIFT_MIN.Enabled = Enabled;
        }


        /// <summary>
        /// 將所有欄位開放
        /// </summary>
        protected void allOpen()
        {   //姓名
            ddl_Query_Name.Enabled = true;
            //加班日期
            txt_OVER_DAY.Enabled = true;
            //加班事由
            txt_REASON.Enabled = true;
            //加班類型
            ddl_OVERTIME_TYPE.Enabled = true;
            //加班時、分
            txt_OVER_HOUR.Enabled = true;
            txt_OVER_MINUTE.Enabled = true;
            //申請業務加班費時、分
            txt_PAY_HOUR.Enabled = true;
            txt_PAY_MIN.Enabled = true;
            //申請輪值加班費時分
            txt_SHIFT_HOUR.Enabled = true;
            txt_SHIFT_MIN.Enabled = true;
            //備註
            txt_OVER_REMARK.Enabled = true;
            //一般專案
            rdoptype.Enabled = true;
            //修改紐
            btn_Update.Enabled = true;
            //刪除紐
            btn_Delete.Enabled = true;
            //重新簽核
            btn_ReOnline.Enabled = true;
            //更新刷卡
            btn_ReGetTime.Enabled = true;
        }/// <summary>
         /// 將所有欄位鎖定
         /// </summary>
        protected void allClose()
        {
            //加班日期
            txt_OVER_DAY.Enabled = false;
            //加班事由
            txt_REASON.Enabled = false;
            //加班類型
            ddl_OVERTIME_TYPE.Enabled = false;
            //加班時、分
            txt_OVER_HOUR.Enabled = false;
            txt_OVER_MINUTE.Enabled = false;
            //申請加班費時、分
            txt_PAY_HOUR.Enabled = false;
            txt_PAY_MIN.Enabled = false;
            //申請輪值加班費時分
            txt_SHIFT_HOUR.Enabled = false;
            txt_SHIFT_MIN.Enabled = false;
            //備註
            txt_OVER_REMARK.Enabled = false;
            //一般專案
            rdoptype.Enabled = false;
            //修改紐
            btn_Update.Enabled = false;
            //刪除紐
            btn_Delete.Enabled = false;
            //重新簽核
            btn_ReOnline.Enabled = false;
        }

        /// <summary>
        /// GridView: 加班資料SelectedIndex變更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gv_resultat_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            if (e.NewSelectedIndex != gv_resultat.SelectedIndex)
            {
                //取消同選項
                e.NewSelectedIndex = -1;

                _query.TPMFION = _TPM_FION;
                _query.MZ_ID = ddl_Query_Name.SelectedValue;

                PageQueryInitial(_query);
            }
        }
        /// <summary>
        /// GridView: 陳核資料Row建立觸發
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gv_online_people_RowCreated(object sender, GridViewRowEventArgs e)
        {
            GridView gv = sender as GridView;
            switch (e.Row.RowType)
            {
                case DataControlRowType.Header:
                    e.Row.Cells[0].Attributes.Add("Style", "display:none");
                    e.Row.Cells[3].Attributes.Add("Style", "display:none");
                    break;
                case DataControlRowType.Footer:
                    break;
                case DataControlRowType.DataRow:
                    for (int i = 0; i < e.Row.Cells.Count; i++)
                    {
                        TableCell cell = e.Row.Cells[i];

                        cell.Attributes["onclick"] = string.Format("{0}", Page.ClientScript.GetPostBackClientHyperlink(gv, string.Format("Select${0}", e.Row.RowIndex)));
                    }
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
        /// <summary>
        /// GridView: 陳核資料列點擊
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gv_online_people_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView gv = sender as GridView;

            if (e.CommandName == "Select")
            {
                gv.SelectedIndex = Convert.ToInt32(e.CommandArgument);
            }
        }


        public void OnClick_設定人事承辦_變更加班(object sender, EventArgs e)
        {
            //讀取名單資料
            讀取人事室人員(5);

            //開啟簽核選單
            mpe_OnlienPeople_Level5.Show();
        }

        public void OnClick_顯示人事室人員_值日補休(object sender, EventArgs e)
        {
            //讀取名單資料
            讀取人事室人員(4);
            //開啟簽核選單
            mpe_OnlienPeople_Level5.Show();
        }


        public void OnClick_人事室人員_確定(object sender, EventArgs e)
        {
            //檢查目前選取者
            if (this.gv_online_people_Level5.SelectedIndex == -1)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel4, GetType()
                    , "click", "alert('未選擇人員!');", true);
                return;
            }
            //綁定目前選中的人事室承辦人
            綁定目前選中的人事室承辦人();

            //關閉簽核選單
            mpe_OnlienPeople_Level5.Hide();
        }

        public void OnClick_人事室人員_關閉(object sender, EventArgs e)
        {
            //開啟簽核選單
            mpe_OnlienPeople_Level5.Hide();
        }


        public void 讀取人事室人員_All()
        {
            讀取人事室人員(4);
            讀取人事室人員(5);
        }
        /// <summary>
        /// 1.根據等級代碼,讀取人事室人員
        /// 2.如果原本已經有選定的人員,會重新綁定之,否則預設第一位
        /// 3.最後再把當前選中的人員,帶到UI上
        /// </summary>
        /// <param name="REVIEW_LEVEL">人員等級,5:加班時數異動審核的承辦 , 4:值日補休的審核承辦 </param>
        public void 讀取人事室人員(int REVIEW_LEVEL)
        {
            //綁定到Hidden
            HiddenField_REVIEW_LEVEL.Value = REVIEW_LEVEL.ToString();

            //設定可選主管列表
            Online_work_Query query = new Online_work_Query()
            {
                OVERTIME_TYPE = "OLWU",//加班修改單(包括:改時數&值日補休申請)
                SCHEDULE_SORT = "1",//申請流程的第1關,在本功能應該都是第1關,後面的關卡要去Online_work.aspx處理
                REVIEW_LEVEL = REVIEW_LEVEL.ToString(),/*5:人事室承辦人,負責審核加班時數異動*/
                /*依照使用者目前所屬MZ_AD*/
                MZ_AD = o_DBFactory.ABC_toTest.Get_First_Field(string.Format("SELECT MZ_EXAD FROM A_DLBASE WHERE MZ_ID='{0}'", _query.MZ_ID), null),
                MZ_UNIT = o_DBFactory.ABC_toTest.Get_First_Field(string.Format("SELECT MZ_EXUNIT FROM A_DLBASE WHERE MZ_ID='{0}'", _query.MZ_ID), null)
            };

            //如果沒有選到人員,或者AD無效,後面不處理
            if (_query.MZ_ID.IsNullOrEmpty())
                return;
            if (query.MZ_AD.IsNullOrEmpty())
                return;


            //讀取
            DataTable dt = OWService.GetOverTimeOnlineReview(query);
            //如果原本有設定人員,則綁定回去
            string targetID = "";
            if (REVIEW_LEVEL == 4)
            {
                targetID = this.HiddenField_REVIEW_ID_LEVEL_4.Value;
            }
            if (REVIEW_LEVEL == 5)
            {
                targetID = this.HiddenField_REVIEW_ID_LEVEL_5.Value;
            }
            DataRow[] foundRows = dt.Select("MZ_ID='" + targetID + "'");
            int rowIndex = 0;//預設選擇第一位
            //有找到資料?
            if (foundRows.Length > 0)
            {   //看資料在第幾筆
                rowIndex = dt.Rows.IndexOf(foundRows[0]);
            }
            //綁定抓到的名單資料,以及當前暫存的人員
            gv_online_people_Level5.DataSource = dt;
            gv_online_people_Level5.SelectedIndex = rowIndex;
            gv_online_people_Level5.DataBind();

            //綁定目前選中的人事室承辦人
            綁定目前選中的人事室承辦人();
        }
        public void 綁定目前選中的人事室承辦人()
        {
            if (this.gv_online_people_Level5.Rows.Count == 0)
            {   //沒資料,不處理
                return;
            }

            int index = this.gv_online_people_Level5.SelectedIndex;
            if (this.HiddenField_REVIEW_LEVEL.Value == "4")
            {
                //綁定目前選中的人事室承辦人
                this.HiddenField_REVIEW_ID_LEVEL_4.Value =
                     gv_online_people_Level5.DataKeys[index]["MZ_ID"].ToStringNullSafe();
                //顯示選中的人,以便除錯
                this.Label_Reviewer_OTD.Text = gv_online_people_Level5.DataKeys[index]["MZ_NAME"].ToStringNullSafe();
            }
            if (this.HiddenField_REVIEW_LEVEL.Value == "5")
            {
                //綁定目前選中的人事室承辦人
                this.HiddenField_REVIEW_ID_LEVEL_5.Value =
                     gv_online_people_Level5.DataKeys[index]["MZ_ID"].ToStringNullSafe();
                //顯示選中的人,以便除錯
                this.Label_Reviewer.Text = gv_online_people_Level5.DataKeys[index]["MZ_NAME"].ToStringNullSafe();
            }
        }
    }

}