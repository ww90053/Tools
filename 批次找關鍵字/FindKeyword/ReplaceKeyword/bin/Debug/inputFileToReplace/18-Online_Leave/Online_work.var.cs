using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Data;
using TPPDDB.Helpers;
using TPPDDB._18_Online_Leave;
using TPPDDB.Models._18_Online_Work;
using TPPDDB.Models._3_ForLeave;
using TPPDDB._3_forleave;

namespace TPPDDB.Online_Leave
{
    /// <summary>
    /// 針對線上簽核功能,把變數擴充拔出來,不然原本太大一包了...
    /// </summary>
    public partial class Online_work
    {
        /*
        下面這堆string int 等變數,
        如果沒有用到ViewSatae Session等
        照理說應該是沒有暫存的效果
        建議還是宣告成區域變數比較好,否則後面會很難追蹤
        就像全家人都穿同一條褲子一樣
            */

        CSonline CSonline;
        /// <summary>
        /// 暫存用-建議做成區域變數較佳
        /// </summary>
        string strSQL = string.Empty;
        DataTable temp_SharedDT = new DataTable();
        DataTable temp2 = new DataTable();
        public string mz_date;     //簽核日期
        public string agents_date; //代理人日期
        public string person_date; //呈辦人日期(人事室)
        public string history_date; //歷程
        public string overtime_date;//加班日期
        public string overtime_histyry_date;//加班歷程日期
        public int iOverTimeHistory = 0; //gv_overtime_history
        public int iOverTime = 0; //gv_overtime
        public int i = 0;  //gv_data
        public int iAgents = 0; //gv_agents
        public int iPerson = 0; //gv_person
        public int iHistory = 0; //gv_history
        //public int iSelect = 0;
        //public int iFLOW = 0;
        public int iSelf = 0;
        public string EXAD
        {
            get { return ViewState["EXADS"].ToStringNullSafe(); }
            set { ViewState["EXADS"] = value; }
        }
        public string EXUNIT
        {
            get { return ViewState["EXUNITS"].ToStringNullSafe(); }
            set { ViewState["EXUNITS"] = value; }
        }
        /// <summary>
        /// 目前操作,屬於哪個關卡代碼?
        /// </summary>
        public string SCHEDULE
        {
            get { return ViewState["SCHEDULE"].ToStringNullSafe(); }
            set { ViewState["SCHEDULE"] = value; }
        }
        /// <summary>
        /// 目前操作,屬於第幾關?
        /// </summary>
        public int SORT
        {
            get
            {
                if (ViewState["SORT"] != null)
                {
                    return int.Parse(ViewState["SORT"].ToString());
                }
                else
                {
                    return 0;
                }
            }
            set { ViewState["SORT"] = value; }
        }
        /// <summary>
        /// 現在是否在查詢 第一層陳核 ?
        /// </summary>
        public bool isUpper
        {
            get
            {
                if (ViewState["isUpper"] != null)
                {
                    return bool.Parse(ViewState["isUpper"].ToString());
                }
                else
                {
                    return false;
                }
            }
            set
            {
                ViewState["isUpper"] = value.ToStringNullSafe();
            }
        }

        /// <summary>
        /// 目前的審核模式? 對應到左側點了哪個超連結按鈕
        /// temp:差假線上簽核
        /// AGENTS:代理人簽核
        /// </summary>
        public string MODE
        {
            get { return ViewState["MODE"].ToStringNullSafe(); }
            set { ViewState["MODE"] = value; }
        }
        /// <summary>
        /// 沒意義的變數, 目前是否點了需要開啟陳核人員的視窗? y:是
        /// 但是程式碼裡面只有給值,卻沒有將此值作為判斷用途,應可移除
        /// </summary>
        public string s
        {
            get { return ViewState["s"].ToStringNullSafe(); }
            set { ViewState["s"] = value; }
        }

        /// <summary>
        /// 判斷有無承辦人或主管? y:有找到可陳核的人
        /// </summary>
        public string np
        {
            get { return ViewState["np"].ToStringNullSafe(); }
            set { ViewState["np"] = value; }
        }
        /// <summary>
        /// 出現"沒有下一層，同意請按決行，不同意請按退回"的時候,的一個標記
        /// 出現的時候標記為y
        /// </summary>
        public string re
        {
            get { return ViewState["re"].ToStringNullSafe(); }
            set { ViewState["re"] = value; }
        }
        /// <summary>
        /// 現在點的是哪個按鈕? 上面記錄了按鈕的中文名稱,也就是  btn_xxxx.Text
        /// 需要注意的是,上面記載的文字很重要,因為它會跟資料表上的設定做對照
        /// 也就是 C_STATUS.C_STATUS_NAME ,會影響到 C_LEAVE_HISTORY.PROCESS_STATUS 的寫入
        /// 如果按鈕文字要改,資料也要跟著改
        /// </summary>
        public string btn
        {
            get { return ViewState["btn"].ToStringNullSafe(); }
            set { ViewState["btn"] = value; }
        }

        /// <summary>
        /// Y:代表目前並沒有查詢到差勤紀錄
        /// 如果有上述Y值,隨後會把頁面導去重新載入,然後此值會再被清除
        /// 變相來說就是不顯示資料的概念
        /// </summary>
        public string type
        {
            get { return ViewState["type"].ToStringNullSafe(); }
            set { ViewState["type"] = value; }
        }

        /// <summary>
        /// 填寫意見,可能從以下來源
        /// txt_message
        /// txt_b_message
        /// txt_c_message
        /// </summary>
        public string txt_message_Text
        {
            get { return ViewState["txt_message"].ToStringNullSafe(); }
            set { ViewState["txt_message"] = value; }
        }

        /// <summary>
        /// 此變數只會給值 PERSON 否則不給
        /// 給PERSON代表目前操作的功能屬於個人表單
        /// </summary>
        public string WHAT
        {
            get { return ViewState["WHAT"].ToStringNullSafe(); }
            set { ViewState["WHAT"] = value; }
        }

        /// <summary>
        /// 會給兩種值
        /// flow:好像沒有實質用途
        /// overtime:選擇月份的時候,會再多抓個人加班歷程
        /// </summary>
        public string WHATS
        {
            get { return ViewState["WHATS"].ToStringNullSafe(); }
            set { ViewState["WHATS"] = value; }
        }
        /// <summary>
        /// 假單的 DLTB01_SN ,會把 e.CommandArgument 對應值記錄下來 
        /// </summary>
        public object History
        {
            get { return ViewState["History"]; }
            set { ViewState["History"] = value; }
        }
        

        /// <summary>
        /// 設值 UI上方的 待簽核表單 個表單的勾選狀態 有標y的代表打勾
        /// </summary>
        /// <param name="index"></param>
        /// <param name="input"></param>
        public void Set_upd(int index, object input)
        {
            ViewState["upd" + index] = input.ToStringNullSafe();
        }
        /// <summary>
        /// 取值
        /// UI上方的 待簽核表單 個表單的勾選狀態
        /// 有標y的代表打勾
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string Get_upd(int index)
        {
            return ViewState["upd" + index].ToStringNullSafe();
        }
        /// <summary>
        /// UI上方的 待簽核表單中 給人家勾了多少筆? 會影響到某些迴圈的上限值
        /// </summary>
        public int RSN_LENTH
        {
            get { return ViewState["RSN_LENTH"].ToString().SafeToInt(); }
            set { ViewState["RSN_LENTH"] = value; }
        }

        /// <summary>
        /// 設值 目前勾選的簽核表單,通常會搭配上面的RSN_LENTH跑回圈
        /// </summary>
        /// <param name="index"></param>
        /// <param name="input"></param>
        public void Set_RSN(int index, object input)
        {
            ViewState["RSN" + index] = input.ToStringNullSafe();
        }
        /// <summary>
        /// 取值 目前勾選的簽核表單,通常會搭配上面的RSN_LENTH跑回圈
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string Get_RSN(int index)
        {
            return ViewState["RSN" + index].ToStringNullSafe();
        }

        /// <summary>
        /// 已決行案件(人事室呈辦人)
        /// </summary>
        public DataTable gv_data_Person
        {
            get { return ViewState["gv_data_Person"] as DataTable; }
            set { ViewState["gv_data_Person"] = value; }
        }
        /// <summary>
        /// 清除所有ViewState變數內容
        /// </summary>
        void ViewStateClear()
        {
            ViewState.Clear();
        }

        //int count;

        //20140616
        string _C_strGID
        {
            get { return ViewState["C_strGID"] != null ? ViewState["C_strGID"].ToString() : string.Empty; }
            set { ViewState["C_strGID"] = value; }
        }

        /// <summary>
        /// 登入者資訊
        /// </summary>
        private string _strUserName
        {
            get
            {

                DataTable ADtoAdt = o_DBFactory.ABC_toTest.Create_Table(@"SELECT MZ_KCHI MZ_OCCC,MZ_NAME FROM A_DLBASE 
                                LEFT JOIN A_KTYPE ON A_KTYPE.MZ_KCODE =A_DLBASE.MZ_OCCC AND A_KTYPE.MZ_KTYPE='26'
                                WHERE RTRIM(MZ_ID) = '" + SessionHelper.ADPMZ_ID + "' ", "get");

                if (ADtoAdt.Rows.Count == 1)
                {
                    string MZ_KCHI = ADtoAdt.Rows[0]["MZ_OCCC"].ToString().Trim();
                    string MZ_NAME = ADtoAdt.Rows[0]["MZ_NAME"].ToString().Trim();
                    return "使用者：" + MZ_KCHI + " " + MZ_NAME;
                }
                else
                {
                    DataTable UNITNOdt = o_DBFactory.ABC_toTest.Create_Table("SELECT OCCC, \"NAME\" FROM H_VPBASE WHERE RTRIM(IDNO) = '" + SessionHelper.ADPMZ_ID + "' ", "get");

                    if (UNITNOdt.Rows.Count == 1)
                    {
                        string OCCC = ADtoAdt.Rows[0]["OCCC"].ToString().Trim();
                        string NAME = ADtoAdt.Rows[0]["NAME"].ToString().Trim();
                        return "使用者：" + OCCC + " " + NAME;
                    }
                    else
                    { return ""; }

                }




            }
        }

        OWService OWService = new OWService();
        /// <summary>
        /// 假勤相關模組
        /// </summary>
        CFService CFService = new CFService();
        string _TPM_FION
        {
            get { return ViewState["TPM_FION"] != null ? ViewState["TPM_FION"].ToStringNullSafe() : string.Empty; }
            set { ViewState["TPM_FION"] = value; }
        }
        Online_work_Query _query
        {
            get { return ViewState["query"] != null ? (Online_work_Query)ViewState["query"] : null; }
            set { ViewState["query"] = value; }
        }
    }
}