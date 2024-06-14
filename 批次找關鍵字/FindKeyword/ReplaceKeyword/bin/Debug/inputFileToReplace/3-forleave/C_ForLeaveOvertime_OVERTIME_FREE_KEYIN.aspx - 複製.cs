using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Drawing;
using TPPDDB.App_Code;
using System.Collections.Generic;
using TPPDDB.Logic;
using TPPDDB.Service;
using TPPDDB.Model;
using TPPDDB.Model.Const;
using TPPDDB.Helpers;

namespace TPPDDB._3_forleave
{
    public partial class C_ForLeaveOvertime_OVERTIME_FREE_KEYIN : System.Web.UI.Page
    {
        /// <summary>使用者之系統權限身分</summary>
        string MZPower
        {
            get
            {
                return o_a_Function.strGID(Session["ADPMZ_ID"].ToString());
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //TODO 20150527 Neil 尋找整個專案後，無地方引用此參數，故先行註解移除。待上版後若有問題，再進行復原動作 
                //Session["DIRECTTIME_DUTYMONTH"] = (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0') + DateTime.Now.Month.ToString().PadLeft(2, '0');

                C.check_power();
                C.set_Panel_EnterToTAB(ref this.Panel3);
                C.set_Panel_EnterToTAB(ref this.Panel2);

                #region 此段Cookie應不必用到，確定修正完成後可刪除
                //查詢姓名
                HttpCookie ForLeaveOvertime_OVERTIME_FREE_KEYIN_NAME_Cookie = new HttpCookie("ForLeaveOvertime_OVERTIME_FREE_KEYIN_NAME");
                ForLeaveOvertime_OVERTIME_FREE_KEYIN_NAME_Cookie = Request.Cookies["ForLeaveOvertime_OVERTIME_FREE_KEYIN_NAME"];

                HttpCookie ForLeaveOvertime_OVERTIME_FREE_KEYIN_ID_Cookie = new HttpCookie("ForLeaveOvertime_OVERTIME_FREE_KEYIN_ID");
                ForLeaveOvertime_OVERTIME_FREE_KEYIN_ID_Cookie = Request.Cookies["ForLeaveOvertime_OVERTIME_FREE_KEYIN_ID"];

                if (ForLeaveOvertime_OVERTIME_FREE_KEYIN_ID_Cookie == null)
                {
                    ViewState["MZ_ID"] = null;
                    Response.Cookies["ForLeaveOvertime_OVERTIME_FREE_KEYIN_ID"].Expires = DateTime.Now.AddYears(-1);
                }
                else
                {
                    ViewState["MZ_ID"] = TPMPermissions._strDecod(ForLeaveOvertime_OVERTIME_FREE_KEYIN_ID_Cookie.Value.ToString());
                    Response.Cookies["ForLeaveOvertime_OVERTIME_FREE_KEYIN_ID"].Expires = DateTime.Now.AddYears(-1);
                }

                if (ForLeaveOvertime_OVERTIME_FREE_KEYIN_NAME_Cookie == null)
                {
                    ViewState["MZ_NAME"] = null;
                    Response.Cookies["ForLeaveOvertime_OVERTIME_FREE_KEYIN_NAME"].Expires = DateTime.Now.AddYears(-1);
                }
                else
                {
                    ViewState["MZ_NAME"] = TPMPermissions._strDecod(ForLeaveOvertime_OVERTIME_FREE_KEYIN_NAME_Cookie.Value.ToString());
                    Response.Cookies["ForLeaveOvertime_OVERTIME_FREE_KEYIN_NAME"].Expires = DateTime.Now.AddYears(-1);
                }
                #endregion 

                #region 人名(人員)下拉欄位之資料載入

                //使用者是否有進行查詢動作
                Boolean isSearchCallBack = (Request["type"] != null) ? true : false;

                String SES_ADPMZ_ID = Session["ADPMZ_ID"].ToString();
                String SES_Search_ID = "";
                String SES_Search_Name = "";
                String SES_Search_EXAD = "";
                String SES_Search_EXUNIT = "";
                List<String> WHEREs = new List<String>();
                if (isSearchCallBack)
                {
                    //人員下拉選單為查詢條件帶回的機關單位來進行查詢
                    //因查詢條件有可能是用人名，因此這邊在查詢條件需要多補上，否則人名下拉清單會為空
                    SES_Search_ID = Session["OverTime_Search_ID"] != null ? Session["OverTime_Search_ID"].ToString() : ""; //人名
                    SES_Search_Name = Session["OverTime_Search_Name"] != null ? Session["OverTime_Search_Name"].ToString() : ""; //身分證字號
                    SES_Search_EXAD = Session["OverTime_Search_EXAD"] != null ? Session["OverTime_Search_EXAD"].ToString() : ""; //現服機關
                    SES_Search_EXUNIT = Session["OverTime_Search_EXUNIT"] != null ? Session["OverTime_Search_EXUNIT"].ToString() : ""; //現服單位

                    ///特殊規則: 為了因應姓名查詢的問題，故若使用者只有輸入姓名，則協助其抓取身分證字號後用 OR 進行查詢
                    if (String.IsNullOrEmpty(SES_Search_ID) && !String.IsNullOrEmpty(SES_Search_Name))
                    {
                        ///TODO: 目前新寫的功能的SQL均採用字串直接組合的方式，但目前驗證後，發現特殊中文字無法以這樣的方式查詢
                        ///故這部分往後需統一改為 SqlParameter 的方式進行 SQL 撰寫
                        List<SqlParameter> lstParam = new List<SqlParameter>();
                        lstParam.Add(new SqlParameter("MZ_NAME", SqlDbType.NVarChar) { Value = SES_Search_Name });

                        String ID = o_DBFactory.ABC_toTest.GetValue(@" SELECT MZ_ID FROM A_DLBASE WHERE MZ_NAME like '%'+ @MZ_NAME+'%' ", lstParam);
                        if (!String.IsNullOrEmpty(ID))
                        {
                            SES_Search_ID = ID;
                            SES_Search_Name = ""; //原先姓名的部分移除 
                            Session["OverTime_Search_Name"] = ""; //Session 亦移除，否則 gv_show 也會撈不出資料
                        }
                    }

                }
                else
                {
                    //如果是第一次載入而非經過查詢，則直接帶入目前登入者的現服機關及單位
                    SES_Search_EXAD = Session["ADPMZ_EXAD"] != null ? Session["ADPMZ_EXAD"].ToString() : ""; //現服機關
                    SES_Search_EXUNIT = Session["ADPMZ_EXUNIT"] != null ? Session["ADPMZ_EXUNIT"].ToString() : ""; //現服單位
                    //預設先選自己
                    DropDownList_MZ_NAME.SelectedValue = SES_ADPMZ_ID;

                }

                if (!String.IsNullOrEmpty(SES_Search_ID))
                    WHEREs.Add(String.Format(@"MZ_ID='{0}'", SES_Search_ID));

                //if (!String.IsNullOrEmpty(SES_Search_Name))
                //    WHEREs.Add(String.Format(@"MZ_NAME like N'%{0}%'", SES_Search_Name));

                if (!String.IsNullOrEmpty(SES_Search_EXAD))
                    WHEREs.Add(String.Format(@"MZ_EXAD='{0}'", SES_Search_EXAD));

                if (!String.IsNullOrEmpty(SES_Search_EXUNIT))
                    WHEREs.Add(String.Format(@"MZ_EXUNIT='{0}'", SES_Search_EXUNIT));

                String SQL_Where = "";
                if (WHEREs.Count > 0)
                    SQL_Where = String.Join(@" AND ", WHEREs.ToArray());
                else
                    //此一小段為預防參數全部都沒有時，撈出全警局資料的異常情況；若程式會執行到這邊代表有問題
                    SQL_Where = @" MZ_EXAD = '382130000C' AND MZ_EXUNIT = '0287' ";

                //組合查詢字串
                SqlDataSource3.SelectCommand = @"SELECT MZ_ID,MZ_NAME FROM A_DLBASE WHERE " + SQL_Where + "AND MZ_STATUS2 = 'Y'";
                DropDownList_MZ_NAME.DataBind();

                //協助其載入內外勤人員選項
                RadioButtonList_WorkP.SelectedValue = AccountService.getDutyUserClassify_V1(DropDownList_MZ_NAME.SelectedValue).ToString();

                #endregion

                gv_show();

                C.controlEnable(ref this.Panel3, false);
                C.controlEnable(ref this.Panel2, false);

                btnJobRelocationInput.Enabled = true;

                DropDownList_MZ_NAME.Enabled = true;

            }
        }


        /// <summary>按鈕 : 新增</summary>
        /// 未直接執行SQL，確認鈕才真正執行
        protected void btInsert_Click(object sender, EventArgs e)
        {
            TextBox_DUTYDATE.Text = string.Empty;

            foreach (object obj in Panel2.Controls)
            {
                if (obj is DropDownList)
                {
                    DropDownList dd = obj as DropDownList;
                    dd.SelectedIndex = -1;
                }
            }

            ViewState["Mode"] = "INSERT";
            ViewState["CMDSQL"] = "INSERT INTO C_DUTYTABLE_PERSONAL (MZ_AD,MZ_UNIT, MZ_EXAD, MZ_EXUNIT, MZ_ID, DUTYDATE,TIME1,DUTYITEM1,TIME2,DUTYITEM2,TIME3,DUTYITEM3," +
                                                                    "TIME4,DUTYITEM4,TIME5,DUTYITEM5,TIME6,DUTYITEM6,TIME7,DUTYITEM7,TIME8,DUTYITEM8," +
                                                                    "TIME9,DUTYITEM9,TIME10,DUTYITEM10,TIME11,DUTYITEM11,TIME12,DUTYITEM12,TIME13,DUTYITEM13," +
                                                                    "TIME14,DUTYITEM14,TIME15,DUTYITEM15,TIME16,DUTYITEM16,TIME17,DUTYITEM17,TIME18,DUTYITEM18," +
                                                                    "TIME19,DUTYITEM19,TIME20,DUTYITEM20,TIME21,DUTYITEM21,TIME22,DUTYITEM22,TIME23,DUTYITEM23," +
                                                                    "TIME24,DUTYITEM24,TIME25,DUTYITEM25,TIME26,DUTYITEM26,TIME27,DUTYITEM27,TOTAL_HOURS,ISDIRECTTIME,MZ_MEMO, CONVERT_REST_HOURS," +
                                                                    "CREATEUSER, CREATEDATE, DUTYSTOPOFF, DUTYUSERCLASSIFY) " +
                                                          " VALUES (@MZ_AD,@MZ_UNIT, @MZ_EXAD, @MZ_EXUNIT, @MZ_ID,@DUTYDATE,@TIME1,@DUTYITEM1,@TIME2,@DUTYITEM2,@TIME3,@DUTYITEM3," +
                                                                   ":TIME4,@DUTYITEM4,@TIME5,@DUTYITEM5,@TIME6,@DUTYITEM6,@TIME7,@DUTYITEM7,@TIME8,@DUTYITEM8," +
                                                                   ":TIME9,@DUTYITEM9,@TIME10,@DUTYITEM10,@TIME11,@DUTYITEM11,@TIME12,@DUTYITEM12,@TIME13,@DUTYITEM13," +
                                                                   ":TIME14,@DUTYITEM14,@TIME15,@DUTYITEM15,@TIME16,@DUTYITEM16,@TIME17,@DUTYITEM17,@TIME18,@DUTYITEM18," +
                                                                   ":TIME19,@DUTYITEM19,@TIME20,@DUTYITEM20,@TIME21,@DUTYITEM21,@TIME22,@DUTYITEM22,@TIME23,@DUTYITEM23," +
                                                                   ":TIME24,@DUTYITEM24,@TIME25,@DUTYITEM25,@TIME26,@DUTYITEM26,@TIME27,@DUTYITEM27,@TOTAL_HOURS,@ISDIRECTTIME,@MZ_MEMO, @CONVERT_REST_HOURS," +
                                                                   ":CREATEUSER, @CREATEDATE, @DUTYSTOPOFF, @DUTYUSERCLASSIFY) ";


            #region 元件開關控制

            //整體Panel控制一定要放前面
            C.controlEnable(ref this.Panel3, true);
            C.controlEnable(ref this.Panel2, true);

            btnJobRelocationInput.Enabled = false;
            btInsert.Enabled = false;
            btUpdate.Enabled = false;
            btSearch.Enabled = false;
            btDelete.Enabled = false;
            btOK.Enabled = true; //打開確認鍵
            btCancel.Enabled = true; //打開取消鍵

            #endregion

            DropDownList_MZ_NAME.Focus();
            timeShow("insert");
        }

        /// <summary>按鈕 : 更改</summary>
        /// 未直接執行SQL，確認鈕才真正執行
        protected void btUpdate_Click(object sender, EventArgs e)
        {
            String DutyDate = o_str.tosql(TextBox_DUTYDATE.Text.Replace("/", "").PadLeft(7, '0')); //帶回原始日期
            String MZ_ID = (tbMZ_ID.Visible ? o_str.tosql(tbMZ_ID.Text) : DropDownList_MZ_NAME.SelectedValue); //帶回原始ID



            if (DutyService.isDutyTableVerify(MZ_ID, DutyDate))
            {
                //檢查是否已被審核，若已審核則跳出訊息並結束編輯
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('此超勤資料已審核，無法變更');", true);
                return;
            }

            try
            {
                String ID = GridView1.SelectedRow.Cells[1].Text;
                DropDownList_MZ_NAME.SelectedValue = ID;

            }
            catch (Exception ex)
            {
                //記錄錯誤
                Log.SaveLog("C_ForLeaveOvertime_OVERTIME_FREE_KEYIN", "1", ex.ToString() + "\r\n" + ex.StackTrace);
                tbMZ_ID.Text = GridView1.SelectedRow.Cells[1].Text;
                tbMZ_ID.Enabled = false;
                tbChange();
            }

            ViewState["Mode"] = "UPDATE";
            //ViewState["CMDSQL"] = "UPDATE C_DUTYTABLE_PERSONAL SET " +
            //                                "MZ_AD = @MZ_AD,MZ_UNIT = @MZ_UNIT, MZ_ID = @MZ_ID,DUTYDATE = @DUTYDATE,TIME1 = @TIME1," +
            //                                "DUTYITEM1 = @DUTYITEM1,TIME2 = @TIME2,DUTYITEM2 = @DUTYITEM2,TIME3 = @TIME3,DUTYITEM3 = @DUTYITEM3," +
            //                                "TIME4 = @TIME4,DUTYITEM4 = @DUTYITEM4,TIME5 = @TIME5,DUTYITEM5 = @DUTYITEM5,TIME6 = @TIME6," +
            //                                "DUTYITEM6 = @DUTYITEM6,TIME7 = @TIME7,DUTYITEM7 = @DUTYITEM7,TIME8 = @TIME8,DUTYITEM8 = @DUTYITEM8," +
            //                                "TIME9 = @TIME9,DUTYITEM9 = @DUTYITEM9,TIME10 = @TIME10,DUTYITEM10 = @DUTYITEM10,TIME11 = @TIME11," +
            //                                "DUTYITEM11 = @DUTYITEM11,TIME12 = @TIME12,DUTYITEM12 = @DUTYITEM12,TIME13 = @TIME13,DUTYITEM13 = @DUTYITEM13," +
            //                                "TIME14 = @TIME14,DUTYITEM14 = @DUTYITEM14,TIME15 = @TIME15,DUTYITEM15 = @DUTYITEM15,TIME16 = @TIME16," +
            //                                "DUTYITEM16 = @DUTYITEM16,TIME17 = @TIME17,DUTYITEM17 = @DUTYITEM17,TIME18 = @TIME18,DUTYITEM18 = @DUTYITEM18," +
            //                                "TIME19 = @TIME19,DUTYITEM19 = @DUTYITEM19,TIME20 = @TIME20,DUTYITEM20 = @DUTYITEM20,TIME21 = @TIME21," +
            //                                "DUTYITEM21 = @DUTYITEM21,TIME22 = @TIME22,DUTYITEM22 = @DUTYITEM22,TIME23 = @TIME23,DUTYITEM23 = @DUTYITEM23," +
            //                                "TIME24 = @TIME24,DUTYITEM24 = @DUTYITEM24,TIME25 = @TIME25,DUTYITEM25 = @DUTYITEM25,TIME26 = @TIME26," +
            //                                "DUTYITEM26 = @DUTYITEM26,TIME27 = @TIME27,DUTYITEM27 = @DUTYITEM27,TOTAL_HOURS = @TOTAL_HOURS,ISDIRECTTIME = @ISDIRECTTIME,MZ_MEMO=@MZ_MEMO, " +
            //                                "UPDATEUSER = @UPDATEUSER, UPDATEDATE= @UPDATEDATE, CONVERT_REST_HOURS = @CONVERT_REST_HOURS, DUTYSTOPOFF = @DUTYSTOPOFF " +
            //                      "WHERE " +
            //                      "MZ_ID='" + MZ_ID +
            //                          "' AND DUTYDATE='" + DutyDate + "'";
            // Joy 修改 修改功能不更新機關單位 後更改為可以修改機關單位(下拉選單自行選擇)
            ViewState["CMDSQL"] = "UPDATE C_DUTYTABLE_PERSONAL SET " +
                                            "MZ_AD = @MZ_AD,MZ_UNIT = @MZ_UNIT, MZ_ID = @MZ_ID,DUTYDATE = @DUTYDATE,TIME1 = @TIME1," +
                                            "DUTYITEM1 = @DUTYITEM1,TIME2 = @TIME2,DUTYITEM2 = @DUTYITEM2,TIME3 = @TIME3,DUTYITEM3 = @DUTYITEM3," +
                                            "TIME4 = @TIME4,DUTYITEM4 = @DUTYITEM4,TIME5 = @TIME5,DUTYITEM5 = @DUTYITEM5,TIME6 = @TIME6," +
                                            "DUTYITEM6 = @DUTYITEM6,TIME7 = @TIME7,DUTYITEM7 = @DUTYITEM7,TIME8 = @TIME8,DUTYITEM8 = @DUTYITEM8," +
                                            "TIME9 = @TIME9,DUTYITEM9 = @DUTYITEM9,TIME10 = @TIME10,DUTYITEM10 = @DUTYITEM10,TIME11 = @TIME11," +
                                            "DUTYITEM11 = @DUTYITEM11,TIME12 = @TIME12,DUTYITEM12 = @DUTYITEM12,TIME13 = @TIME13,DUTYITEM13 = @DUTYITEM13," +
                                            "TIME14 = @TIME14,DUTYITEM14 = @DUTYITEM14,TIME15 = @TIME15,DUTYITEM15 = @DUTYITEM15,TIME16 = @TIME16," +
                                            "DUTYITEM16 = @DUTYITEM16,TIME17 = @TIME17,DUTYITEM17 = @DUTYITEM17,TIME18 = @TIME18,DUTYITEM18 = @DUTYITEM18," +
                                            "TIME19 = @TIME19,DUTYITEM19 = @DUTYITEM19,TIME20 = @TIME20,DUTYITEM20 = @DUTYITEM20,TIME21 = @TIME21," +
                                            "DUTYITEM21 = @DUTYITEM21,TIME22 = @TIME22,DUTYITEM22 = @DUTYITEM22,TIME23 = @TIME23,DUTYITEM23 = @DUTYITEM23," +
                                            "TIME24 = @TIME24,DUTYITEM24 = @DUTYITEM24,TIME25 = @TIME25,DUTYITEM25 = @DUTYITEM25,TIME26 = @TIME26," +
                                            "DUTYITEM26 = @DUTYITEM26,TIME27 = @TIME27,DUTYITEM27 = @DUTYITEM27,TOTAL_HOURS = @TOTAL_HOURS,ISDIRECTTIME = @ISDIRECTTIME,MZ_MEMO=@MZ_MEMO, " +
                                            "UPDATEUSER = @UPDATEUSER, UPDATEDATE= @UPDATEDATE, CONVERT_REST_HOURS = @CONVERT_REST_HOURS, DUTYSTOPOFF = @DUTYSTOPOFF, DUTYUSERCLASSIFY = @DUTYUSERCLASSIFY " +
                                  "WHERE " +
                                  "MZ_ID='" + MZ_ID +
                                      "' AND DUTYDATE='" + DutyDate + "'";

            HttpCookie Cookie1 = new HttpCookie("PKEY_MZ_ID");
            Cookie1.Value = DropDownList_MZ_NAME.SelectedValue.Trim();
            Response.Cookies.Add(Cookie1);

            HttpCookie Cookie2 = new HttpCookie("PKEY_DUTYDATE");
            Cookie2.Value = TextBox_DUTYDATE.Text.Replace("/", "").PadLeft(7, '0');
            Response.Cookies.Add(Cookie2);


            ///帶回動作需先判斷是否已鎖定
            /// 1.若已鎖定(LOCK_FLAG)，則 Disable 此輸入框
            /// 2.若未鎖定(LOCK_FLAG)，則帶回來原值，按下確認後連動修改加班表(C_OVERTIME_HOUR_INSIDE)
            Boolean isLock = DutyService.isOvertimeLock(MZ_ID, DutyDate, DutyService.enumOverDutyType.YU);
            if (isLock)
                txtConvertRest.Enabled = false;
            else
                txtConvertRest.Enabled = true;

            #region 元件開關控制
            //整體Panel控制一定要放前面
            C.controlEnable(ref this.Panel3, true);
            C.controlEnable(ref this.Panel2, true);

            btnJobRelocationInput.Enabled = false;
            btUpdate.Enabled = false;
            btSearch.Enabled = false;
            btInsert.Enabled = false;
            btPrint.Enabled = false;

            btOK.Enabled = true; //開啟確認
            btCancel.Enabled = true; //開啟取消


            #endregion

            DropDownList_MZ_NAME.Focus();

        }

        /// <summary>按鈕 : 刪除</summary>
        /// 直接執行SQL進行刪除
        protected void btDelete_Click(object sender, EventArgs e)
        {
            //tbMZ_ID.Text 可能為空值
            // 如果tbMZ_ID.Text 為空值 則抓取下拉選單之身分證號
            String MZ_ID = (tbMZ_ID.Visible ? o_str.tosql(tbMZ_ID.Text) : DropDownList_MZ_NAME.SelectedValue); //帶回原始ID
            //if (string.IsNullOrEmpty(MZ_ID))
            //{
            //    MZ_ID = DropDownList_MZ_NAME.SelectedValue;
            //}
            String DutyDate = TextBox_DUTYDATE.Text.Trim().Replace("/", "").PadLeft(7, '0');

            if (DutyService.isDutyTableVerify(MZ_ID, DutyDate))
            {
                //檢查是否已被審核，若已審核則跳出訊息並結束編輯
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('此超勤資料已審核，無法變更');", true);
                return;
            }

            /*
             維護單:1121201
             刪除時，如果超勤補休時數(C_DUTYTABLE_PERSONAL.CONVERT_REST_HOURS)>0，
             則查找C_OVERTIME_BASE同一天(OVER_DAY)的資料是否有OVERTIME_TYPE= ‘OTU’的資料，
             如有且C_OVERTIME_BASE.REST_HOUR>0，則禁止刪除(警語：本日已有超勤補休紀錄，資料禁止刪除！)；
             如果C_OVERTIME_BASE.REST_HOUR=0，則可以刪除但一併刪除C_OVERTIME_BASE的同一日OVERTIME_TYPE=OTU的資料。
             */

            //flag:要順便刪除 C_OVERTIME_BASE的同一日OTU的資料
            bool isOVERTIME_BASE_OTU_Need_DEL = false;
            //根據ID,上班日,找出當天的超勤補休時數,參考資料表:C_DUTYTABLE_PERSONAL
            int iCONVERT_REST_HOURS = Service_C_DUTYTABLE_PERSONAL.Get_iCONVERT_REST_HOURS(MZ_ID, DutyDate);
            //如果超勤補休時數(CONVERT_REST_HOURS)>0
            if (iCONVERT_REST_HOURS > 0)
            {
                //則查找C_OVERTIME_BASE同一天(OVER_DAY)的資料是否有OVERTIME_TYPE = ‘OTU’的資料，

                //根據ID,上班日,找出當天的超勤補休時數,參考資料表: C_OVERTIME_BASE
                int iREST_HOURS = Service_C_OVERTIME_BASE.Get_iREST_HOURS(MZ_ID, DutyDate);
                // 如有且REST_HOUR > 0，
                if (iREST_HOURS > 0)
                {
                    //則禁止刪除(警語：本日已有超勤補休紀錄，資料禁止刪除！)；
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('本日已有超勤補休紀錄，資料禁止刪除！');", true);
                    return;
                }
                else
                {
                    // 如果REST_HOUR = 0，則可以刪除但一併刪除C_OVERTIME_BASE的同一日OTU的資料。  
                    isOVERTIME_BASE_OTU_Need_DEL = true;
                }
            }

            /*
            string DeleteString = "DELETE " +
                                  "FROM " +
                                            "C_DUTYTABLE_PERSONAL " +
                                  "WHERE " +
                                            "MZ_ID='" + MZ_ID +
                                      "' AND DUTYDATE='" + DutyDate + "'";
            */
            try
            {
                //刪除資料
                string DeleteString = "";
                bool isOK = Delete(MZ_ID, DutyDate, isOVERTIME_BASE_OTU_Need_DEL, ref DeleteString);
                if (isOK == false)
                {   //失敗就拋錯去catch,這邊不return,讓後面的解除鎖定動作能繼續跑
                    throw new Exception("刪除失敗");
                }

                //o_DBFactory.ABC_toTest.Edit_Data(DeleteString);
                btCancel.Enabled = false;
                btUpdate.Enabled = false;
                btDelete.Enabled = false;
                btInsert.Enabled = true;
                btPrint.Enabled = false;
                btOK.Enabled = false;
                foreach (object obj in Panel2.Controls)
                {
                    if (obj is DropDownList)
                    {
                        DropDownList dd = obj as DropDownList;
                        dd.SelectedIndex = -1;
                    }
                }

                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除成功');", true);
                //2010.06.04 LOG紀錄 by伊珊
                TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00004", TPMPermissions._boolTPM(), DeleteString);
                timeShow("insert");
            }
            catch (Exception ex)
            {
                //記錄錯誤
                Log.SaveLog("C_ForLeaveOvertime_OVERTIME_FREE_KEYIN", "1", ex.ToString() + "\r\n" + ex.StackTrace);
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除失敗');", true);
            }

            gv_show();

            //20140305
            btSearch.Enabled = true;


            DropDownList_MZ_NAME.Visible = true;
            DropDownList_MZ_NAME.Enabled = true;

            tbMZ_ID.Visible = false;
            tbMZ_ID.Text = string.Empty;
            lbName.Visible = false;
            lbName.Text = string.Empty;

            lblIDNAME.Visible = false;

            TextBox_DUTYDATE.Text = string.Empty;

        }

        /// <summary>
        /// 刪除資料
        /// </summary>
        /// <param name="MZ_ID">員警ID</param>
        /// <param name="DutyDate">上班日</param>
        /// <param name="isOVERTIME_BASE_OTU_Need_DEL">是否要連加班的OTU超勤補休 也要一併刪除?</param>
        /// <returns></returns>
        public bool Delete(string MZ_ID, string DutyDate, bool isOVERTIME_BASE_OTU_Need_DEL, ref string DeleteString)
        {

            List<SqlCommand> listCMD = new List<SqlCommand>();
            //超勤資料
            SqlCommand cmd1 = Service_C_DUTYTABLE_PERSONAL.GetCommand_Delete(MZ_ID, DutyDate);
            listCMD.Add(cmd1);

            DeleteString = cmd1.CommandText;

            //如果 加班的OTU超勤補休 也要一併刪除
            if (isOVERTIME_BASE_OTU_Need_DEL)
            {
                //兩張表都刪除
                //加班的OTU超勤補休
                SqlCommand cmd2 = Service_C_OVERTIME_BASE.GetCommand_DeleteOTU(MZ_ID, DutyDate);
                listCMD.Add(cmd2);

                DeleteString += ";" + cmd2.CommandText;
            }
            return o_DBFactory.ABC_toTest.Transaction_ExecuteNonQuery(listCMD);
        }


        /// <summary>按鈕 : 取消</summary>
        protected void btCancel_Click(object sender, EventArgs e)
        {
            foreach (object obj in Panel2.Controls)
            {
                if (obj is DropDownList)
                {
                    DropDownList dd = obj as DropDownList;
                    dd.SelectedIndex = -1;
                }
            }

            #region 元件開關控制
            //整體Panel控制一定要放前面
            C.controlEnable(ref this.Panel3, false);
            C.controlEnable(ref this.Panel2, false);

            btnJobRelocationInput.Enabled = true;
            btDelete.Enabled = false;
            btCancel.Enabled = false;
            btUpdate.Enabled = false;
            btOK.Enabled = false;
            btPrint.Enabled = false;

            btInsert.Enabled = true; //開啟新增鈕
            btSearch.Enabled = true; //開啟搜尋鈕

            TextBox_DUTYDATE.Text = string.Empty;
            DropDownList_MZ_NAME.Visible = true;
            DropDownList_MZ_NAME.Enabled = true;
            tbMZ_ID.Visible = false;
            lbName.Visible = false;
            tbMZ_ID.Text = string.Empty;
            lbName.Text = string.Empty;
            lblIDNAME.Visible = false;
            TextBox_MZ_MEMO.Text = "";

            //協助其載入內外勤人員選項
            RadioButtonList_WorkP.SelectedValue = AccountService.getDutyUserClassify_V1(DropDownList_MZ_NAME.SelectedValue).ToString();

            #endregion
        }

        /// <summary>按鈕 : 列印</summary>
        /// TODO: 1040514 Neil 太多部分與原先重複，須兩邊一起進行重構動作
        protected void btPrint_Click(object sender, EventArgs e)
        {

            //C_diffdutydetail_rpt
            Response.Redirect("~/3-forleave/C_Overtimepay_detail_rpt.aspx?TPM_FION=" + Request.QueryString["TPM_FION"].ToString());


            //取得目前選擇之使用之現服機關等
            //String MZ_ID = tbMZ_ID.Text;
            //AccountModel mUser = AccountService.lookupAccount(MZ_ID);

            //String Name_AD = AccountService.getSysPara(mUser.ExAD, AccountService.enumCategroy.AD);
            //String Name_Unit = AccountService.getSysPara(mUser.ExUnit, AccountService.enumCategroy.UNIT);

            //String DutyMonth = TextBox_DUTYDATE.Text.Substring(0, 5);

            //            DataTable rpt = new DataTable();
            //            #region RPT表格欄位定義
            //            rpt.Columns.Add("MZ_ID", typeof(string));
            //            rpt.Columns.Add("MZ_NAME", typeof(string));
            //            rpt.Columns.Add("MZ_OCCC", typeof(string));
            //            rpt.Columns.Add("MZ_SRANK", typeof(string));
            //            rpt.Columns.Add("ITEMA", typeof(string));
            //            rpt.Columns.Add("ITEMB", typeof(string));
            //            rpt.Columns.Add("ITEMC", typeof(string));
            //            rpt.Columns.Add("ITEMD", typeof(string));
            //            rpt.Columns.Add("ITEME", typeof(string));
            //            rpt.Columns.Add("ITEMF", typeof(string));
            //            rpt.Columns.Add("ITEMG", typeof(string));
            //            rpt.Columns.Add("ITEMH", typeof(string));
            //            rpt.Columns.Add("ITEMI", typeof(string));
            //            rpt.Columns.Add("ITEMII", typeof(string));
            //            rpt.Columns.Add("TOTAL", typeof(string));
            //            rpt.Columns.Add("LIMIT", typeof(string));
            //            rpt.Columns.Add("OVERTIME", typeof(string));
            //            rpt.Columns.Add("ISHOLIDAY", typeof(string));
            //            rpt.Columns.Add("DATE", typeof(string));
            //            rpt.Columns.Add("MZ_MEMO", typeof(string));
            //            rpt.Columns.Add("ITEMM_ADDHOUR", typeof(string));
            //            #endregion 

            //            string IDSQL = "SELECT DISTINCT A.MZ_ID,CASE " +
            //                                " WHEN (SELECT MZ_HOUR_LIMIT FROM C_DUTYLIMIT WHERE MZ_AD=A.MZ_EXAD" +
            //                                                                             " AND MZ_UNIT=A.MZ_EXUNIT" +
            //                                                                             " AND MZ_ID=A.MZ_ID) IS NOT NULL " +
            //                                " THEN " +
            //                                      "(SELECT MZ_HOUR_LIMIT FROM C_DUTYLIMIT WHERE MZ_AD=A.MZ_EXAD" +
            //                                                                             " AND MZ_UNIT=A.MZ_EXUNIT" +
            //                                                                             " AND MZ_ID=A.MZ_ID) " +

            //                                " WHEN (SELECT MZ_HOUR_LIMIT FROM C_DUTYLIMIT WHERE MZ_AD=A.MZ_EXAD" +
            //                                                                             " AND MZ_UNIT=A.MZ_EXUNIT" +
            //                                                                             " AND MZ_ID ='NULL') IS NOT NULL" +
            //                                " THEN " +
            //                                      "(SELECT MZ_HOUR_LIMIT FROM C_DUTYLIMIT WHERE MZ_AD=A.MZ_EXAD" +
            //                                                                             " AND MZ_UNIT=A.MZ_EXUNIT" +
            //                                                                             " AND MZ_ID ='NULL')" +
            //                                " ELSE -1 END AS \"LIMIT\" FROM C_DUTYTABLE_PERSONAL A,A_DLBASE B WHERE A.MZ_ID=B.MZ_ID AND dbo.dbo.SUBSTR(A.DUTYDATE,1,5)='" + DutyMonth + "'";

            ////            string strSQL = @"SELECT A.DUTYDATE, A.MZ_ID, A.MZ_MEMO ,
            ////                            CASE WHEN CO.RESTFLAG ='N' THEN  '支領加班費' WHEN CO.RESTFLAG = 'YO' THEN '加班補休' WHEN CO.RESTFLAG = 'YU' THEN '超勤補休' WHEN CO.RESTFLAG = 'YD' THEN '值日補休' ELSE '' END ||
            ////                            CASE WHEN CD.MZ_ID IS  NULL THEN   ''  ELSE  '值日 ' END MEMO_NEW
            ////                            FROM  C_DUTYTABLE_PERSONAL A
            ////                            LEFT JOIN C_OVERTIME_HOUR_INSIDE  CO ON CO.MZ_ID = A.MZ_ID AND CO.MZ_DATE = A.DUTYDATE
            ////                            LEFT JOIN C_ONDUTY_DAY  CD ON CD.MZ_ID = A.MZ_ID AND CD.DATE_TAG = A.DUTYDATE 
            ////                            WHERE dbo.dbo.SUBSTR(A.DUTYDATE,1,5)='" + DutyMonth + "'";
            //            string strSQL = @"SELECT A.DUTYDATE, A.MZ_ID, A.MZ_MEMO ,
            //                              CASE 
            //                                WHEN CO.RESTFLAG ='N' THEN  '支領加班費'
            //                                WHEN CO.RESTFLAG = 'YO' THEN '加班補休'
            //                                WHEN CO.RESTFLAG = 'YU' THEN '超勤補休'
            //                                WHEN CO.RESTFLAG = 'YD' THEN '值日補休'
            //                              ELSE '' END ||
            //                              CASE
            //                                WHEN CD.MZ_ID IS  NULL THEN   ''
            //                                ELSE  '值日 ' END ||
            //                              C_DLCODE.MZ_CNAME AS MEMO_NEW
            //                              FROM  C_DUTYTABLE_PERSONAL A
            //                              LEFT JOIN C_OVERTIME_HOUR_INSIDE CO ON CO.MZ_ID = A.MZ_ID AND CO.MZ_DATE = A.DUTYDATE
            //                              LEFT JOIN C_ONDUTY_DAY  CD ON CD.MZ_ID = A.MZ_ID AND CD.DATE_TAG = A.DUTYDATE 
            //                              LEFT JOIN C_DLTB01 ON C_DLTB01.MZ_ID = A.MZ_ID
            //                                                    AND (A.DUTYDATE = C_DLTB01.MZ_IDATE1 OR (A.DUTYDATE >= C_DLTB01.MZ_IDATE1 AND A.DUTYDATE <= C_DLTB01.MZ_ODATE))
            //                              LEFT JOIN C_DLCODE ON C_DLTB01.MZ_CODE = C_DLCODE.MZ_CODE
            //                              WHERE dbo.dbo.SUBSTR(A.DUTYDATE,1,5)='" + DutyMonth + "'";

            //            if (!string.IsNullOrEmpty(mUser.AD))
            //            {
            //                // Joy 修改查詢條件為現服機關
            //                //strSQL += " AND A.MZ_AD='" + mUser.ExAD + "'";
            //                //IDSQL += " AND A.MZ_AD='" + mUser.ExAD + "'";
            //                strSQL += " AND A.MZ_EXAD='" + mUser.ExAD + "'";
            //                IDSQL += " AND A.MZ_EXAD='" + mUser.ExAD + "'";
            //            }

            //            if (!string.IsNullOrEmpty(mUser.Unit))
            //            {
            //                // Joy 修改查詢條件為現服單位
            //                //strSQL += " AND A.MZ_UNIT='" + mUser.ExUnit + "'";
            //                //IDSQL += " AND A.MZ_UNIT='" + mUser.ExUnit + "'";
            //                strSQL += " AND A.MZ_EXUNIT='" + mUser.ExUnit + "'";
            //                IDSQL += " AND A.MZ_EXUNIT='" + mUser.ExUnit + "'";
            //            }

            //            if (!string.IsNullOrEmpty(mUser.ID))
            //            {
            //                strSQL += " AND A.MZ_ID='" + mUser.ID + "'";
            //                IDSQL += " AND A.MZ_ID='" + mUser.ID + "'";
            //            }
            //            if (DropDownList_MZ_NAME.SelectedValue != "")
            //            {
            //                strSQL += " AND A.MZ_ID='" + DropDownList_MZ_NAME.SelectedValue + "'";
            //                IDSQL += " AND A.MZ_ID='" + DropDownList_MZ_NAME.SelectedValue + "'";
            //            }
            //            if (MZ_ID == "")
            //                MZ_ID = DropDownList_MZ_NAME.SelectedValue;
            //            strSQL += " ORDER BY A.DUTYDATE";

            //            DataTable tempDT = new DataTable();
            //            tempDT = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GETVALUE");

            //            DataTable idDT = new DataTable();
            //            idDT = o_DBFactory.ABC_toTest.Create_Table(IDSQL, "GETVALUE2");

            //            for (int j = 0; j < idDT.Rows.Count; j++)
            //            {
            //                //TODO 20141229 今天沒空.這之後要改掉,為什麼每次換代碼都要回資料庫撈.不會一開始就兜出來嗎
            //                string NAME = o_DBFactory.ABC_toTest.CNAME(idDT.Rows[j]["MZ_ID"].ToString());
            //                string OCCC = o_DBFactory.ABC_toTest.OCCC(idDT.Rows[j]["MZ_ID"].ToString());
            //                string SRANK = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=(SELECT MZ_SRANK FROM A_DLBASE WHERE MZ_ID='" + idDT.Rows[j]["MZ_ID"].ToString() + "') AND MZ_KTYPE='09' ");

            //                //塞取空白日期
            //                for (int i = 1; i <= 31; i++)
            //                {
            //                    DataRow dr = rpt.NewRow();

            //                    dr["MZ_ID"] = idDT.Rows[j]["MZ_ID"].ToString();
            //                    dr["MZ_NAME"] = NAME;
            //                    dr["MZ_OCCC"] = OCCC;
            //                    dr["MZ_SRANK"] = SRANK;
            //                    dr["ITEMA"] = "";
            //                    dr["ITEMB"] = "";
            //                    dr["ITEMC"] = "";
            //                    dr["ITEMD"] = "";
            //                    dr["ITEME"] = "";
            //                    dr["ITEMF"] = "";
            //                    dr["ITEMG"] = "";
            //                    dr["ITEMH"] = "";
            //                    dr["ITEMI"] = "";
            //                    dr["ITEMII"] = "";
            //                    dr["TOTAL"] = "";
            //                    dr["LIMIT"] = idDT.Rows[j]["LIMIT"].ToString();
            //                    dr["OVERTIME"] = "";
            //                    dr["ISHOLIDAY"] = "";
            //                    dr["DATE"] = i.ToString().PadLeft(2, '0');
            //                    dr["MZ_MEMO"] = "";
            //                    dr["ITEMM_ADDHOUR"] = "";
            //                    rpt.Rows.Add(dr);
            //                }
            //            }

            //            //處理每一天的迴圈
            //            for (int i = 0; i < tempDT.Rows.Count; i++)
            //            {
            //                //統計當天累積時數  //TODO 跑迴圈且一筆一筆回資料庫統計.想死人嗎
            //                //String MZ_ID = tempDT.Rows[i]["MZ_ID"].ToString();
            //                String FULLDATE = tempDT.Rows[i]["DUTYDATE"].ToString(); //包含日期
            //                String DATE = FULLDATE.Substring(5, 2); //僅有日數

            //                string ITEMA = Count(MZ_ID, "A", tempDT.Rows[i]["DUTYDATE"].ToString());
            //                string ITEMB = Count(MZ_ID, "B", tempDT.Rows[i]["DUTYDATE"].ToString());
            //                string ITEMC = Count(MZ_ID, "C", tempDT.Rows[i]["DUTYDATE"].ToString());
            //                string ITEMD = Count(MZ_ID, "D", tempDT.Rows[i]["DUTYDATE"].ToString());
            //                string ITEME = Count(MZ_ID, "E", tempDT.Rows[i]["DUTYDATE"].ToString()); //已改用廢棄函數，應廢掉
            //                string ITEMF = Count(MZ_ID, "F", tempDT.Rows[i]["DUTYDATE"].ToString());
            //                string ITEMG = Count(MZ_ID, "G", tempDT.Rows[i]["DUTYDATE"].ToString()); //已改用廢棄函數，應廢掉
            //                // Joy 新增 判斷是否有值宿
            //                string ITEMM = Count(MZ_ID, "M", tempDT.Rows[i]["DUTYDATE"].ToString());
            //                //Joy 20150408 常訓併計於其他勤務下

            //                //已改用廢棄函數，應廢掉
            //                string ITEMH = Convert.ToString(int.Parse(Count(tempDT.Rows[i]["MZ_ID"].ToString(), "H", tempDT.Rows[i]["DUTYDATE"].ToString())) + int.Parse(Count(tempDT.Rows[i]["MZ_ID"].ToString(), "J", tempDT.Rows[i]["DUTYDATE"].ToString())));
            //                string ITEMI = Count(MZ_ID, "I", tempDT.Rows[i]["DUTYDATE"].ToString());

            //                //G督勤 
            //                int ITEMGG = 0;
            //                if (ITEMG != "")
            //                {
            //                    string[] COUNTG = ITEMG.Split('.');
            //                    ITEMGG = COUNTG.Length;
            //                }

            //                //I專案情務
            //                int ITEMII = 0;
            //                if (ITEMI != "")
            //                {
            //                    string[] COUNTI = ITEMI.Split('.');
            //                    ITEMII = COUNTI.Length;
            //                }

            //                //判斷是否為國定假日
            //                String s = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_HOLIDAY_DATE FROM C_DUTYHOLIDAY WHERE MZ_HOLIDAY_DATE='" + tempDT.Rows[i]["DUTYDATE"].ToString() + "'");
            //                Boolean isHoliday = (!String.IsNullOrEmpty(s));

            //                //開始計算超勤當日
            //                Int32 DutyTime = 0;
            //                Int32 OverTime = 0;
            //                Int32 HAmount = 0;
            //                Int32 EAmount = 0;
            //                Int32 RestHour = GetSingleDayOvertimeRest(MZ_ID, FULLDATE);
            //                LogicOvertime Logic = new LogicOvertime();
            //                List<String> lstSingleDayInputs = GetSingleDayOverTimeList(MZ_ID, FULLDATE); //每日超勤狀況使用者輸入值
            //                ///判斷是否停休

            //                String LastValue = GetDutyStopOff(MZ_ID, FULLDATE) ;
            //                Boolean isDutyStopOff = (LastValue == "Y") ? true : false;

            //                Logic.calSingleDayOvertime(lstSingleDayInputs, ref DutyTime, ref OverTime, isDutyStopOff, isHoliday); //取得上班時數總時數及超勤時數總時數
            //                Logic.calSingleDayOverTime_HAmount(lstSingleDayInputs, ref HAmount); //取得其他勤務總時數
            //                Logic.calSingleDayOverTime_EAmount(lstSingleDayInputs, ref EAmount); //取得勤區查察總時數

            //                int ITEMM_ADDHOUR = 0;
            //                //Joy 判斷是否有值宿且超勤時數>12
            //                if (ITEMM != "0" && DutyTime > 12)
            //                {
            //                    ITEMM_ADDHOUR = DutyTime - 12;
            //                }

            //                for (int j = 0; j < rpt.Rows.Count; j++)
            //                {
            //                    if (rpt.Rows[j]["MZ_ID"].ToString() == tempDT.Rows[i]["MZ_ID"].ToString() &&
            //                        rpt.Rows[j]["DATE"].ToString() == DATE)
            //                    {
            //                        rpt.Rows[j]["ITEMA"] = ITEMA;
            //                        rpt.Rows[j]["ITEMB"] = ITEMB;
            //                        rpt.Rows[j]["ITEMC"] = ITEMC;
            //                        rpt.Rows[j]["ITEMD"] = ITEMD;
            //                        rpt.Rows[j]["ITEME"] = EAmount.ToString(); //勤區查察總時數
            //                        rpt.Rows[j]["ITEMF"] = ITEMF;
            //                        rpt.Rows[j]["ITEMG"] = ITEMG;
            //                        rpt.Rows[j]["ITEMH"] = HAmount.ToString(); //其他勤務總時數
            //                        rpt.Rows[j]["ITEMI"] = ITEMI;
            //                        rpt.Rows[j]["ITEMII"] = ITEMII;
            //                        rpt.Rows[j]["TOTAL"] = DutyTime.ToString(); //上班時數
            //                        rpt.Rows[j]["OVERTIME"] = (OverTime - RestHour).ToString(); //超勤時數
            //                        rpt.Rows[j]["ISHOLIDAY"] = (isHoliday) ? "Y" : "N"; //是否為國定假日
            //                        rpt.Rows[j]["MZ_MEMO"] = tempDT.Rows[i]["MZ_MEMO"].ToString() + "  " + tempDT.Rows[i]["MEMO_NEW"].ToString();
            //                        rpt.Rows[j]["ITEMM_ADDHOUR"] = ITEMM_ADDHOUR;
            //                    }
            //                }
            //            }

            //            if (rpt.Rows.Count > 0)
            //            {
            //                Session["rpt_dt"] = rpt;

            //                Session["TITLE"] = string.Format("{0}{1}{2}年{3}月超勤時數統計表",
            //                                                Name_AD,
            //                                                Name_Unit,
            //                                                int.Parse(TextBox_DUTYDATE.Text.Substring(0, 3)).ToString(),
            //                                                int.Parse(TextBox_DUTYDATE.Text.Substring(3, 2)).ToString());

            //                string tmp_url = "C_rpt.aspx?fn=Overtimepay_detail&TPM_FION=" + "0";

            //                ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
            //            }
            //            else
            //            {
            //                ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "alert('無此日期之相關資料');", true);

            //            }
        }

        /// <summary>按鈕 : 確認</summary>
        protected void btok_Click(object sender, EventArgs e)
        {
            //2010.06.07 by 伊珊
            string ErrorString = "";
            string old_ID = "NULL";
            string old_DUTYDATE = "NULL";
            String DutyDate = TextBox_DUTYDATE.Text;

            if (ViewState["Mode"].ToString() == "UPDATE")
            {
                HttpCookie Cookie1 = new HttpCookie("PKEY_MZ_ID");
                Cookie1 = Request.Cookies["PKEY_MZ_ID"];
                old_ID = Cookie1.Value.ToString();

                HttpCookie Cookie2 = new HttpCookie("PKEY_DUTYDATE");
                Cookie2 = Request.Cookies["PKEY_DUTYDATE"];
                old_DUTYDATE = Cookie2.Value.ToString();
            }

            //避免資料重複寫入,同一人
            string pkey_check;
            //如果目前是更新
            if (old_ID == DropDownList_MZ_NAME.SelectedValue
                && old_DUTYDATE == TextBox_DUTYDATE.Text.Trim().Replace("/", string.Empty).PadLeft(7, '0')
                && ViewState["Mode"].ToString() == "UPDATE")
            {
                pkey_check = "0";
            }
            //如果目前是 調離單位申請
            else if (ViewState["Mode"].ToString() == "JobRelocation")
            {
                pkey_check = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM C_DUTYTABLE_PERSONAL WHERE MZ_ID='" + tbMZ_ID.Text + "' AND DUTYDATE='" + o_str.tosql(TextBox_DUTYDATE.Text.Replace("/", "").PadLeft(7, '0')) + "'");
            }
            //其他模式 抓取下拉選單選到的員警
            else
            {
                pkey_check = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM C_DUTYTABLE_PERSONAL WHERE MZ_ID='" + DropDownList_MZ_NAME.SelectedValue.Trim() + "' AND DUTYDATE='" + o_str.tosql(TextBox_DUTYDATE.Text.Replace("/", "").PadLeft(7, '0')) + "'");
            }

            if (pkey_check != "0")
            {
                ErrorString += "該日期已有輸入資料，請重新確認日期資料是否輸入正確!" + "\\r\\n"; //人員與超勤日期違反唯一值的條件
                DropDownList_MZ_NAME.BackColor = Color.Orange;
                TextBox_DUTYDATE.BackColor = Color.Orange;
            }
            else
            {
                DropDownList_MZ_NAME.BackColor = Color.White;
                TextBox_DUTYDATE.BackColor = Color.White;
            }


            if (!string.IsNullOrEmpty(ErrorString))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('" + ErrorString + "請更改不可重複欄位與輸入變色之欄位');", true);
                return;
            }


            //如果C_OVERTEIM_BASE已經有相同日期(DUTYDATEàOVER_DAY)超勤補休資料，則跳出視窗阻擋(訊息：請先行移除5.1.加班系統之超勤補休時數！)。
            String input_MZ_ID = tbMZ_ID.Visible ? tbMZ_ID.Text : DropDownList_MZ_NAME.SelectedValue;
            String input_Date = string.IsNullOrEmpty(TextBox_DUTYDATE.Text.Trim()) ? "" : TextBox_DUTYDATE.Text.Trim().Replace("/", string.Empty).PadLeft(7, '0');
            bool isOverTimeExist = DutyService.isOvertimeExist(input_MZ_ID, input_Date);
            //查詢 : 加班表 C_OVERTIME_BASE 超勤補休 是否已存在? 且 已經使用了超勤補休來請假
            bool isOvertime_And_RestHour_Exist = DutyService.isOvertime_And_RestHour_Exist(input_MZ_ID, input_Date);
            //判斷超勤補休時數是否有異動
            //bool isChange = (txtConvertRest.Text != txtConvertRest_Back);
            //如果 超勤補休時數有改變 且 超勤補休時數已經存在,且 本次的加班時數>0
            if (isOvertime_And_RestHour_Exist)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請先行移除5.1.加班系統之超勤補休時數！');", true);
                return;
            }

            //檢查日期格式是否正確
            String ErrMsg = String.Empty;
            if (!LogicCommon.isUnfromatDateStringVaild(TextBox_DUTYDATE.Text, ref ErrMsg))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('日期格式錯誤');", true);
                return;
            }

            //若身分證字號欄位為可輸入時，檢查身分證字號是否正確
            if (tbMZ_ID.Visible && !LogicCommon.isIDFormatVaild(tbMZ_ID.Text))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('身分證字號格式錯誤');", true);
                return;
            }


            //20150429 Neil : 此部分補入超勤時數上限之判斷
            Int32 DutyTime = 0;
            Int32 OverTime = 0;
            Boolean isNeedInputComment = false; //是否需要輸入備註欄位
            LogicOvertime Logic = new LogicOvertime();

            List<String> lstSingleDayInputs = new List<String>(); //每日超勤狀況使用者輸入值
            foreach (object obj in Panel2.Controls)
            {
                if (obj is DropDownList)
                {
                    DropDownList dd = obj as DropDownList;

                    //加入輸入 lst
                    if (!String.IsNullOrEmpty(dd.SelectedValue))
                        lstSingleDayInputs.Add(dd.SelectedValue);

                    //若有選擇專案勤務，則需要輸入備註
                    if (dd.SelectedValue == "I")
                        isNeedInputComment = true;
                }
            }

            //檢查備註欄位是否有輸入
            if (isNeedInputComment && String.IsNullOrEmpty(TextBox_MZ_MEMO.Text))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('選取專案勤務請輸入備註欄！');", true);
                return;
            }

            //判斷該日期是否是國定假日
            //Dictionary<String, String> dicHolidays = DutyService.Lookup_Holidays(); //撈出所有國定假日
            //string strname = "補假";
            //string strname2 = "彈性";
            //Boolean isHoliday = false;
            //if (dicHolidays.ContainsKey(strname) && dicHolidays.ContainsKey(strname2))
            //{
            //    isHoliday = true;
            //}
            //補假或彈性放假不算國定假日，要以平日計算
            string strHoliday = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_HOLIDAY_NAME FROM C_DUTYHOLIDAY WHERE  MZ_HOLIDAY_DATE = '" + DutyDate + "'");
            string strname = "補假";
            string strname2 = "彈性";
            Boolean isHoliday = false;
            if (!String.IsNullOrEmpty(strHoliday))
            {
                //如果不是補假或彈性放假
                if (!strHoliday.Contains(strname) && !strHoliday.Contains(strname2))
                {   //當天放假
                    isHoliday = true;
                }
            }
            //特殊:如果是外勤,則取消掉假日判定,都視為平常日
            if (RadioButtonList_WorkP.SelectedValue == "Outter")
            {
                isHoliday = false;
            }

            //判斷是否有勾選停休(指原本排休但後來來上班，計算規則如同國定假日，全部實報實銷沒有12小時上限)
            Boolean isDutyStopOff = (RadioButtonList_StopOff.SelectedValue == "Y") ? true : false;

            //進行每日工時計算，判斷是否有任何錯誤            
            Boolean isCalSuccess = Logic.calSingleDayOvertime(lstSingleDayInputs, ref DutyTime, ref OverTime, isDutyStopOff, isHoliday);
            if (!isCalSuccess)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('發生計算錯誤，請與資訊室聯絡');", true);
                return; //正常情況下應不會有 Excetiopn
            }

            //判斷是否有輸入補休轉換
            //針對國定假日及輪休排班修改判斷條件 20181004 by sky
            Int32 iConvertRestAmount = 0;
            int VYZ_SumTime = 0; //統計休假、輪休、補休時數
            if (!String.IsNullOrEmpty(txtConvertRest.Text))
            {
                Int32.TryParse(txtConvertRest.Text, out iConvertRestAmount);
                VYZ_SumTime = lstSingleDayInputs.Where(x => (x.ToString() == "Y" || x.ToString() == "V" || x.ToString() == "X")).Count();
                int dayinputSum = lstSingleDayInputs.Where(x => x.Length == 1).Count();
                if (iConvertRestAmount != 0)
                {
                    if (isHoliday)
                    { //國定假日+非全部輪休
                        if (VYZ_SumTime != dayinputSum)
                        {
                            if (iConvertRestAmount > OverTime)
                            {
                                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('補休時數大於總上班時數，請調整補休時數');", true);
                                return; //正常情況下應不會有 Excetiopn
                            }

                            OverTime -= iConvertRestAmount;
                        }
                    }
                    else
                    {
                        if (VYZ_SumTime == dayinputSum)
                        { //非國定假日+全部輪休
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('補休時數大於總上班時數，請調整補休時數');", true);
                            return;
                        }
                        else
                        { //非國定假日+非全部輪休
                            if (iConvertRestAmount > OverTime)
                            {
                                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('補休時數大於總上班時數，請調整補休時數');", true);
                                return; //正常情況下應不會有 Excetiopn
                            }

                            OverTime -= iConvertRestAmount;
                        }
                    }
                }
            }

            //判斷超勤時數加上當月的超勤時數
            //如果是內勤人員,當月若超過60小時則應給予提醒
            bool isOverHour = Check_isOver60Hour_forInnerDuty(input_MZ_ID, input_Date, OverTime, RadioButtonList_WorkP.SelectedValue);
            if (isOverHour)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('延長辦公時數每月不得超過60小時(含同時申報業務加班及超勤之時數)。\\r\\n如遇特殊專案報府同意後，另開放權限申請。');", true);

            }

            isOverHour = Check_isOver80Hour_forOuterDuty(input_MZ_ID, input_Date, OverTime, RadioButtonList_WorkP.SelectedValue);
            if (isOverHour)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('輪班輪休人員延長服勤時數每月不得超過80小時。\\r\\n如遇特殊緊急情況(勤休實施要點第5.6點之情形)\\r\\n以每3個月不得超過240小時為限。');", true);

            }

            //抓取當月時數累計

            #region  組合SQL參數集合
            //String input_MZ_ID = tbMZ_ID.Visible ? tbMZ_ID.Text : DropDownList_MZ_NAME.SelectedValue;
            //input_Date = string.IsNullOrEmpty(TextBox_DUTYDATE.Text.Trim()) ? "" : TextBox_DUTYDATE.Text.Trim().Replace("/", string.Empty).PadLeft(7, '0');

            String DT_Now = LogicCommon.getChineseDateTime();
            List<SqlParameter> lstParameter = new List<SqlParameter>();
            AccountModel m = AccountService.lookupAccount(input_MZ_ID);

            //機關單位填寫
            String AD = "";
            String Unit = "";
            String ExAD = "";
            String ExUnit = "";

            ///isCurrentDuty = true;
            ///   此部分登打共分為兩種情況。第一種情況為使用者自行輸入，因此在表內即存入該名使用者之編制機關單位、現服機關單位
            ///isCurrentDuty = false;
            ///   若使用者已調職，則新單位的承辦人員會來替這名使用者進行登打
            ///   此時登打所存入的編制機關單位、現服機關單位，均是存入「登打者」(目前Login的)的資料，而非使用者的資料
            if (ViewState["Mode"].ToString() != "JobRelocation")
            {
                //AD = m.AD;
                AD = m.PayAD; //20150625 修改為儲存發薪單位，而非編制單位
                Unit = m.Unit;
                ExAD = m.ExAD;
                ExUnit = m.ExUnit;

                // sam wellsince 待查
                //// Joy 20150806 機關單位依下拉選單所選修改
                //if (ViewState["Mode"].ToString() == "UPDATE")
                //{
                //    AD = DropDownList_AD.SelectedValue;
                //    Unit = DropDownList_UNIT.SelectedValue;
                //}
            }
            else
            {
                //AD = Session[ConstSession.AD].ToString();
                AD = Session[ConstSession.ADPPAY_AD].ToString(); //20150625 修改為儲存發薪單位，而非編制單位
                Unit = Session[ConstSession.UNIT].ToString();
                ExAD = Session[ConstSession.EXAD].ToString();
                ExUnit = Session[ConstSession.EXUNIT].ToString();
            }

            lstParameter.Add(new SqlParameter("MZ_AD", SqlDbType.Char) { Value = AD });
            lstParameter.Add(new SqlParameter("MZ_UNIT", SqlDbType.Char) { Value = Unit });
            lstParameter.Add(new SqlParameter("MZ_ID", SqlDbType.VarChar) { Value = input_MZ_ID });
            lstParameter.Add(new SqlParameter("DUTYDATE", SqlDbType.VarChar) { Value = input_Date });
            lstParameter.Add(new SqlParameter("TIME1", SqlDbType.VarChar) { Value = "06" + TextBox_DUTYITEM1_1.Text + "-07" + TextBox_DUTYITEM1_2.Text });
            lstParameter.Add(new SqlParameter("DUTYITEM1", SqlDbType.VarChar) { Value = DropDownList_DUTYITEM1.SelectedValue });
            lstParameter.Add(new SqlParameter("TIME2", SqlDbType.VarChar) { Value = "07" + TextBox_DUTYITEM2_1.Text + "-08" + TextBox_DUTYITEM2_2.Text });
            lstParameter.Add(new SqlParameter("DUTYITEM2", SqlDbType.VarChar) { Value = DropDownList_DUTYITEM2.SelectedValue });
            lstParameter.Add(new SqlParameter("TIME3", SqlDbType.VarChar) { Value = "08" + TextBox_DUTYITEM3_1.Text + "-09" + TextBox_DUTYITEM3_2.Text });
            lstParameter.Add(new SqlParameter("DUTYITEM3", SqlDbType.VarChar) { Value = DropDownList_DUTYITEM3.SelectedValue });
            lstParameter.Add(new SqlParameter("TIME4", SqlDbType.VarChar) { Value = "09" + TextBox_DUTYITEM4_1.Text + "-10" + TextBox_DUTYITEM4_2.Text });
            lstParameter.Add(new SqlParameter("DUTYITEM4", SqlDbType.VarChar) { Value = DropDownList_DUTYITEM4.SelectedValue });
            lstParameter.Add(new SqlParameter("TIME5", SqlDbType.VarChar) { Value = "10" + TextBox_DUTYITEM5_1.Text + "-11" + TextBox_DUTYITEM5_2.Text });
            lstParameter.Add(new SqlParameter("DUTYITEM5", SqlDbType.VarChar) { Value = DropDownList_DUTYITEM5.SelectedValue });
            lstParameter.Add(new SqlParameter("TIME6", SqlDbType.VarChar) { Value = "11" + TextBox_DUTYITEM6_1.Text + "-12" + TextBox_DUTYITEM6_2.Text });
            lstParameter.Add(new SqlParameter("DUTYITEM6", SqlDbType.VarChar) { Value = DropDownList_DUTYITEM6.SelectedValue });
            lstParameter.Add(new SqlParameter("TIME7", SqlDbType.VarChar) { Value = "12" + TextBox_DUTYITEM7_1.Text + "-13" + TextBox_DUTYITEM7_2.Text });
            lstParameter.Add(new SqlParameter("DUTYITEM7", SqlDbType.VarChar) { Value = DropDownList_DUTYITEM7.SelectedValue });
            lstParameter.Add(new SqlParameter("TIME8", SqlDbType.VarChar) { Value = "13" + TextBox_DUTYITEM8_1.Text + "-14" + TextBox_DUTYITEM8_2.Text });
            lstParameter.Add(new SqlParameter("DUTYITEM8", SqlDbType.VarChar) { Value = DropDownList_DUTYITEM8.SelectedValue });
            lstParameter.Add(new SqlParameter("TIME9", SqlDbType.VarChar) { Value = "14" + TextBox_DUTYITEM9_1.Text + "-15" + TextBox_DUTYITEM9_2.Text });
            lstParameter.Add(new SqlParameter("DUTYITEM9", SqlDbType.VarChar) { Value = DropDownList_DUTYITEM9.SelectedValue });
            lstParameter.Add(new SqlParameter("TIME10", SqlDbType.VarChar) { Value = "15" + TextBox_DUTYITEM10_1.Text + "-16" + TextBox_DUTYITEM10_2.Text });
            lstParameter.Add(new SqlParameter("DUTYITEM10", SqlDbType.VarChar) { Value = DropDownList_DUTYITEM10.SelectedValue });
            lstParameter.Add(new SqlParameter("TIME11", SqlDbType.VarChar) { Value = "16" + TextBox_DUTYITEM11_1.Text + "-17" + TextBox_DUTYITEM11_2.Text });
            lstParameter.Add(new SqlParameter("DUTYITEM11", SqlDbType.VarChar) { Value = DropDownList_DUTYITEM11.SelectedValue });
            lstParameter.Add(new SqlParameter("TIME12", SqlDbType.VarChar) { Value = "17" + TextBox_DUTYITEM12_1.Text + "-18" + TextBox_DUTYITEM12_2.Text });
            lstParameter.Add(new SqlParameter("DUTYITEM12", SqlDbType.VarChar) { Value = DropDownList_DUTYITEM12.SelectedValue });
            lstParameter.Add(new SqlParameter("TIME13", SqlDbType.VarChar) { Value = "18" + TextBox_DUTYITEM13_1.Text + "-19" + TextBox_DUTYITEM13_2.Text });
            lstParameter.Add(new SqlParameter("DUTYITEM13", SqlDbType.VarChar) { Value = DropDownList_DUTYITEM13.SelectedValue });
            lstParameter.Add(new SqlParameter("TIME14", SqlDbType.VarChar) { Value = "19" + TextBox_DUTYITEM14_1.Text + "-20" + TextBox_DUTYITEM14_2.Text });
            lstParameter.Add(new SqlParameter("DUTYITEM14", SqlDbType.VarChar) { Value = DropDownList_DUTYITEM14.SelectedValue });
            lstParameter.Add(new SqlParameter("TIME15", SqlDbType.VarChar) { Value = "20" + TextBox_DUTYITEM15_1.Text + "-21" + TextBox_DUTYITEM15_2.Text });
            lstParameter.Add(new SqlParameter("DUTYITEM15", SqlDbType.VarChar) { Value = DropDownList_DUTYITEM15.SelectedValue });
            lstParameter.Add(new SqlParameter("TIME16", SqlDbType.VarChar) { Value = "21" + TextBox_DUTYITEM16_1.Text + "-22" + TextBox_DUTYITEM16_2.Text });
            lstParameter.Add(new SqlParameter("DUTYITEM16", SqlDbType.VarChar) { Value = DropDownList_DUTYITEM16.SelectedValue });
            lstParameter.Add(new SqlParameter("TIME17", SqlDbType.VarChar) { Value = "22" + TextBox_DUTYITEM17_1.Text + "-23" + TextBox_DUTYITEM17_2.Text });
            lstParameter.Add(new SqlParameter("DUTYITEM17", SqlDbType.VarChar) { Value = DropDownList_DUTYITEM17.SelectedValue });
            lstParameter.Add(new SqlParameter("TIME18", SqlDbType.VarChar) { Value = "23" + TextBox_DUTYITEM18_1.Text + "-24" + TextBox_DUTYITEM18_2.Text });
            lstParameter.Add(new SqlParameter("DUTYITEM18", SqlDbType.VarChar) { Value = DropDownList_DUTYITEM18.SelectedValue });
            lstParameter.Add(new SqlParameter("TIME19", SqlDbType.VarChar) { Value = "24" + TextBox_DUTYITEM19_1.Text + "-01" + TextBox_DUTYITEM19_2.Text });
            lstParameter.Add(new SqlParameter("DUTYITEM19", SqlDbType.VarChar) { Value = DropDownList_DUTYITEM19.SelectedValue });
            lstParameter.Add(new SqlParameter("TIME20", SqlDbType.VarChar) { Value = "01" + TextBox_DUTYITEM20_1.Text + "-02" + TextBox_DUTYITEM20_2.Text });
            lstParameter.Add(new SqlParameter("DUTYITEM20", SqlDbType.VarChar) { Value = DropDownList_DUTYITEM20.SelectedValue });
            lstParameter.Add(new SqlParameter("TIME21", SqlDbType.VarChar) { Value = "02" + TextBox_DUTYITEM21_1.Text + "-03" + TextBox_DUTYITEM21_2.Text });
            lstParameter.Add(new SqlParameter("DUTYITEM21", SqlDbType.VarChar) { Value = DropDownList_DUTYITEM21.SelectedValue });
            lstParameter.Add(new SqlParameter("TIME22", SqlDbType.VarChar) { Value = "03" + TextBox_DUTYITEM22_1.Text + "-04" + TextBox_DUTYITEM22_2.Text });
            lstParameter.Add(new SqlParameter("DUTYITEM22", SqlDbType.VarChar) { Value = DropDownList_DUTYITEM22.SelectedValue });
            lstParameter.Add(new SqlParameter("TIME23", SqlDbType.VarChar) { Value = "04" + TextBox_DUTYITEM23_1.Text + "-05" + TextBox_DUTYITEM23_2.Text });
            lstParameter.Add(new SqlParameter("DUTYITEM23", SqlDbType.VarChar) { Value = DropDownList_DUTYITEM23.SelectedValue });
            lstParameter.Add(new SqlParameter("TIME24", SqlDbType.VarChar) { Value = "05" + TextBox_DUTYITEM24_1.Text + "-06" + TextBox_DUTYITEM24_2.Text });
            lstParameter.Add(new SqlParameter("DUTYITEM24", SqlDbType.VarChar) { Value = DropDownList_DUTYITEM24.SelectedValue });
            lstParameter.Add(new SqlParameter("TIME25", SqlDbType.VarChar) { Value = "06" + TextBox_DUTYITEM25_1.Text + "-07" + TextBox_DUTYITEM25_2.Text });
            lstParameter.Add(new SqlParameter("DUTYITEM25", SqlDbType.VarChar) { Value = DropDownList_DUTYITEM25.SelectedValue });
            lstParameter.Add(new SqlParameter("TIME26", SqlDbType.VarChar) { Value = "07" + TextBox_DUTYITEM26_1.Text + "-08" + TextBox_DUTYITEM26_2.Text });
            lstParameter.Add(new SqlParameter("DUTYITEM26", SqlDbType.VarChar) { Value = DropDownList_DUTYITEM26.SelectedValue });
            lstParameter.Add(new SqlParameter("TIME27", SqlDbType.VarChar) { Value = "08" + TextBox_DUTYITEM27_1.Text + "-09" + TextBox_DUTYITEM27_2.Text });
            lstParameter.Add(new SqlParameter("DUTYITEM27", SqlDbType.VarChar) { Value = DropDownList_DUTYITEM27.SelectedValue });
            lstParameter.Add(new SqlParameter("TOTAL_HOURS", SqlDbType.Float) { Value = DutyTime });
            lstParameter.Add(new SqlParameter("ISDIRECTTIME", SqlDbType.Float) { Value = OverTime });
            lstParameter.Add(new SqlParameter("MZ_MEMO", SqlDbType.NVarChar) { Value = TextBox_MZ_MEMO.Text });
            lstParameter.Add(new SqlParameter("DUTYSTOPOFF", SqlDbType.NVarChar) { Value = RadioButtonList_StopOff.SelectedValue });
            lstParameter.Add(new SqlParameter("DUTYUSERCLASSIFY", SqlDbType.NVarChar) { Value = RadioButtonList_WorkP.SelectedValue }); //內外勤資料

            //若為一般新增或調離單位申請，則採用Create
            if (ViewState["Mode"].ToString() == "INSERT" ||
                ViewState["Mode"].ToString() == "JobRelocation")
            {
                lstParameter.Add(new SqlParameter("MZ_EXAD", SqlDbType.Char) { Value = ExAD });
                lstParameter.Add(new SqlParameter("MZ_EXUNIT", SqlDbType.Char) { Value = ExUnit });
                lstParameter.Add(new SqlParameter("CONVERT_REST_HOURS", SqlDbType.Float) { Value = iConvertRestAmount });
                lstParameter.Add(new SqlParameter("CREATEUSER", SqlDbType.Char) { Value = Session["ADPMZ_ID"].ToString() });
                lstParameter.Add(new SqlParameter("CREATEDATE", SqlDbType.NVarChar) { Value = DT_Now });
            }

            if (ViewState["Mode"].ToString() == "UPDATE")
            {
                // Joy 移除機關單位
                //lstParameter.RemoveAt(0);
                //lstParameter.RemoveAt(0);

                lstParameter.Add(new SqlParameter("CONVERT_REST_HOURS", SqlDbType.Float) { Value = iConvertRestAmount });
                lstParameter.Add(new SqlParameter("UPDATEUSER", SqlDbType.Char) { Value = Session["ADPMZ_ID"].ToString() });
                lstParameter.Add(new SqlParameter("UPDATEDATE", SqlDbType.NVarChar) { Value = DT_Now });
            }
            #endregion 

            try
            {
                String sql = ViewState["CMDSQL"].ToString();

                //o_DBFactory.ABC_toTest.ExecuteNonQuery( sql, lstParameter.ToArray());
                o_DBFactory.ABC_toTest.SQLExecute(sql, lstParameter);

                #region 若有補休，則同時新增補休單

                AccountModel mAccount = AccountService.lookupAccount(input_MZ_ID);
                //isOverTimeExist = DutyService.isOvertimeExist(input_MZ_ID, input_Date);

                string OVER_STIME = "", OVER_ETIME = "";
                //根據UI上輸入的時間,找出加班時間起迄
                Set_OVER_STIME_ETIME(ref OVER_STIME, ref OVER_ETIME);
                //若當日"超勤補休"類型的加班資料不存在
                if (!isOverTimeExist)
                {
                    if (iConvertRestAmount > 0)
                    {
                        Double dHourPay = MathHelper.Round(Logic.getHourPay(mAccount.ID), 2);
                        Double dRestAmount = Convert.ToDouble(iConvertRestAmount);
                        Double iTotalPay = MathHelper.Round(dHourPay * dRestAmount);
                        String Comment = input_Date + "超勤補休"; //20150514 Neil 修正為固定式之註解(注意此部分為加班日期，非新增日期)
                        var qMZ_AD = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_AD FROM VW_A_DLBASE_S1 WHERE MZ_ID='" + input_MZ_ID + "'");
                        var qMZ_UNIT = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_UNIT FROM VW_A_DLBASE_S1 WHERE MZ_ID='" + input_MZ_ID + "'");

                        iConvertRestAmount = iConvertRestAmount * 60;

                        lstParameter.Clear();
                        lstParameter.Add(new SqlParameter("MZ_ID", SqlDbType.NVarChar) { Value = mAccount.ID }); //身分證號
                        lstParameter.Add(new SqlParameter("OVER_DAY", SqlDbType.VarChar) { Value = input_Date });//加班日期
                        lstParameter.Add(new SqlParameter("MZ_EXAD", SqlDbType.NVarChar) { Value = mAccount.ExAD }); //現服機關
                        lstParameter.Add(new SqlParameter("MZ_EXUNIT", SqlDbType.NVarChar) { Value = mAccount.ExUnit }); //現服機關
                        lstParameter.Add(new SqlParameter("MZ_OCCC", SqlDbType.NVarChar) { Value = mAccount.Title }); //職稱
                        lstParameter.Add(new SqlParameter("REASON", SqlDbType.VarChar) { Value = Comment });//加班事由

                        lstParameter.Add(new SqlParameter("OVER_STIME", SqlDbType.VarChar) { Value = OVER_STIME });//加班時間起
                        lstParameter.Add(new SqlParameter("OVER_ETIME", SqlDbType.VarChar) { Value = OVER_ETIME });//加班時間迄


                        lstParameter.Add(new SqlParameter("OVER_TOTAL", SqlDbType.Float) { Value = iConvertRestAmount }); //加班總時數
                        lstParameter.Add(new SqlParameter("SURPLUS_TOTAL", SqlDbType.Float) { Value = iConvertRestAmount }); //加班剩餘總時數
                        lstParameter.Add(new SqlParameter("LOCK_FLAG", SqlDbType.VarChar) { Value = "N" });//資料鎖定
                        lstParameter.Add(new SqlParameter("OVERTIME_TYPE", SqlDbType.NVarChar) { Value = "OTU" }); //加班類型
                        lstParameter.Add(new SqlParameter("IS_FREESIGN", SqlDbType.VarChar) { Value = "Y" });//是否免線上簽核
                        lstParameter.Add(new SqlParameter("OPTYPE", SqlDbType.VarChar) { Value = "0" });//一般(0)專案(1)加班
                        lstParameter.Add(new SqlParameter("MZ_AD", SqlDbType.NVarChar) { Value = qMZ_AD }); //編制機關
                        lstParameter.Add(new SqlParameter("MZ_UNIT", SqlDbType.NVarChar) { Value = qMZ_UNIT }); //編制單位
                        lstParameter.Add(new SqlParameter("INS_ID", SqlDbType.NVarChar) { Value = Session["ADPMZ_ID"].ToString() }); //新增者
                        lstParameter.Add(new SqlParameter("INS_DATE", SqlDbType.NVarChar) { Value = DT_Now }); //新增日期

                        //插入資料列
                        DutyService.Insert_OverTime(lstParameter);
                    }
                }
                else
                {
                    if (iConvertRestAmount > 0)
                    {
                        iConvertRestAmount = iConvertRestAmount * 60;

                        lstParameter.Clear();
                        lstParameter = new List<SqlParameter>();
                        lstParameter.Add(new SqlParameter("MZ_ID", SqlDbType.NVarChar) { Value = mAccount.ID }); //身分證字號
                        lstParameter.Add(new SqlParameter("OVER_DAY", SqlDbType.NVarChar) { Value = input_Date }); //加班日期
                        lstParameter.Add(new SqlParameter("OVER_TOTAL", SqlDbType.Float) { Value = iConvertRestAmount }); //加班小時數
                        lstParameter.Add(new SqlParameter("SURPLUS_TOTAL", SqlDbType.Float) { Value = iConvertRestAmount }); //加班剩餘小時數

                        lstParameter.Add(new SqlParameter("OVER_STIME", SqlDbType.VarChar) { Value = OVER_STIME });//加班時間起
                        lstParameter.Add(new SqlParameter("OVER_ETIME", SqlDbType.VarChar) { Value = OVER_ETIME });//加班時間迄

                        //更新資料列
                        DutyService.Update_OverTime(lstParameter);
                    }
                    else if (iConvertRestAmount == 0)
                    {
                        //若補休時數更新為0，刪除已建加班費資料
                        lstParameter.Clear();
                        lstParameter.Add(new SqlParameter("MZ_ID", SqlDbType.NVarChar) { Value = mAccount.ID }); //身分證字號
                        lstParameter.Add(new SqlParameter("OVER_DAY", SqlDbType.NVarChar) { Value = input_Date }); //加班日期

                        //刪除資料並記錄Log
                        if (!string.IsNullOrEmpty(mAccount.ID) && !string.IsNullOrEmpty(input_Date))
                        {
                            DutyService.Delete_OverTime(lstParameter, Request.QueryString["TPM_FION"].ToString());
                        }
                    }
                }
                #endregion

                ///主要執行"新增"之功能點
                if (ViewState["Mode"].ToString() == "INSERT" ||
                    ViewState["Mode"].ToString() == "JobRelocation")
                {

                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('新增成功');", true);
                    //2010.06.04 LOG紀錄 by伊珊
                    TPMPermissions._strEventData(Session["ADServerID"].ToString(),
                                                 Request.QueryString["TPM_FION"].ToString(),
                                                 "EBXXX00002",
                                                 TPMPermissions._boolTPM(),
                                                 o_DBFactory.ABC_toTest.RegexSQL(ViewState["CMDSQL"].ToString(), lstParameter.ToArray())
                                                 );

                    foreach (object obj in Panel2.Controls)
                    {
                        if (obj is DropDownList)
                        {
                            DropDownList dd = obj as DropDownList;
                            dd.SelectedIndex = -1;
                        }
                    }
                }

                ///主要執行"更新"之功能點
                else if (ViewState["Mode"].ToString() == "UPDATE")
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('修改成功');", true);
                    //2010.06.04 LOG紀錄 by伊珊
                    TPMPermissions._strEventData(Session["ADServerID"].ToString(),
                                                 Request.QueryString["TPM_FION"].ToString(),
                                                 "EBXXX00003",
                                                 TPMPermissions._boolTPM(),
                                                 o_DBFactory.ABC_toTest.RegexSQL(ViewState["CMDSQL"].ToString(), lstParameter.ToArray())
                                                 );
                }

                ViewState.Remove("Mode");

                #region 元件開關控制
                //整體Panel控制一定要放前面
                C.controlEnable(ref this.Panel3, false);
                C.controlEnable(ref this.Panel2, false);

                btOK.Enabled = false;
                btCancel.Enabled = false;
                btDelete.Enabled = false;
                btUpdate.Enabled = false;
                btnJobRelocationInput.Enabled = true; //開啟調離單位申請鈕
                btnJobRelocationInput.Visible = true;

                btnJobRelocationCancel.Visible = false;
                btnJobRelocationConfrm.Visible = false;

                btInsert.Enabled = true; //開啟新增按鈕
                btSearch.Enabled = true; //開啟搜尋按鈕

                Label_ADUN.Text = "";
                tbMZ_ID.Visible = false;
                lbName.Visible = false;
                tbMZ_ID.Text = string.Empty;
                lbName.Text = string.Empty;
                tbMZ_ID.Enabled = true;
                DropDownList_MZ_NAME.Visible = true;
                DropDownList_MZ_NAME.Enabled = true;
                TextBox_MZ_MEMO.Text = "";
                TextBox_DUTYDATE.Text = "";

                #endregion 

            }
            catch (Exception ex)
            {
                //記錄錯誤
                Log.SaveLog("C_ForLeaveOvertime_OVERTIME_FREE_KEYIN", "1", ex.ToString() + "\r\n" + ex.StackTrace);

                if (ViewState["Mode"].ToString() == "INSERT")
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('新增失敗');", true);
                }
                else if (ViewState["Mode"].ToString() == "UPDATE")
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('編輯失敗');", true);
                }
                else if (ViewState["Mode"].ToString() == "JobRelocation")
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('編輯失敗');", true);
                }
            }
            gv_show();
        }

        /// <summary>
        /// 根據UI上輸入的時間,找出加班時間起迄
        /// </summary>
        /// <param name="OVER_STIME">加班時間起</param>
        /// <param name="OVER_ETIME">加班時間迄</param>
        public void Set_OVER_STIME_ETIME(ref string OVER_STIME, ref string OVER_ETIME)
        {

            #region 找 加班時間起迄

            string sele1 = DropDownList_DUTYITEM1.SelectedValue;

            if (sele1 != "")
            {
                if (OVER_STIME == "")
                {
                    OVER_STIME = "06:" + TextBox_DUTYITEM1_1.Text;
                }

                OVER_ETIME = "07" + TextBox_DUTYITEM1_2.Text;
            }

            string sele2 = DropDownList_DUTYITEM2.SelectedValue;

            if (sele2 != "")
            {
                if (OVER_STIME == "")
                {
                    OVER_STIME = "07:" + TextBox_DUTYITEM2_1.Text;
                }

                OVER_ETIME = "08:" + TextBox_DUTYITEM2_2.Text;
            }

            string sele3 = DropDownList_DUTYITEM3.SelectedValue;

            if (sele3 != "")
            {
                if (OVER_STIME == "")
                {
                    OVER_STIME = "08:" + TextBox_DUTYITEM3_1.Text;
                }

                OVER_ETIME = "09:" + TextBox_DUTYITEM3_2.Text;
            }

            string sele4 = DropDownList_DUTYITEM4.SelectedValue;

            if (sele4 != "")
            {
                if (OVER_STIME == "")
                {
                    OVER_STIME = "09:" + TextBox_DUTYITEM4_1.Text;
                }

                OVER_ETIME = "10:" + TextBox_DUTYITEM4_2.Text;
            }

            string sele5 = DropDownList_DUTYITEM5.SelectedValue;

            if (sele5 != "")
            {
                if (OVER_STIME == "")
                {
                    OVER_STIME = "10:" + TextBox_DUTYITEM5_1.Text;
                }

                OVER_ETIME = "11:" + TextBox_DUTYITEM5_2.Text;
            }

            string sele6 = DropDownList_DUTYITEM6.SelectedValue;

            if (sele6 != "")
            {
                if (OVER_STIME == "")
                {
                    OVER_STIME = "11:" + TextBox_DUTYITEM6_1.Text;
                }

                OVER_ETIME = "12:" + TextBox_DUTYITEM6_2.Text;
            }

            string sele7 = DropDownList_DUTYITEM7.SelectedValue;

            if (sele7 != "")
            {
                if (OVER_STIME == "")
                {
                    OVER_STIME = "12:" + TextBox_DUTYITEM7_1.Text;
                }

                OVER_ETIME = "13:" + TextBox_DUTYITEM7_2.Text;
            }

            string sele8 = DropDownList_DUTYITEM8.SelectedValue;

            if (sele8 != "")
            {
                if (OVER_STIME == "")
                {
                    OVER_STIME = "13:" + TextBox_DUTYITEM8_1.Text;
                }

                OVER_ETIME = "14:" + TextBox_DUTYITEM8_2.Text;
            }

            string sele9 = DropDownList_DUTYITEM9.SelectedValue;

            if (sele9 != "")
            {
                if (OVER_STIME == "")
                {
                    OVER_STIME = "14:" + TextBox_DUTYITEM9_1.Text;
                }

                OVER_ETIME = "15:" + TextBox_DUTYITEM9_2.Text;
            }

            string sele10 = DropDownList_DUTYITEM10.SelectedValue;

            if (sele10 != "")
            {
                if (OVER_STIME == "")
                {
                    OVER_STIME = "15:" + TextBox_DUTYITEM10_1.Text;
                }

                OVER_ETIME = "16:" + TextBox_DUTYITEM10_2.Text;
            }

            string sele11 = DropDownList_DUTYITEM11.SelectedValue;

            if (sele11 != "")
            {
                if (OVER_STIME == "")
                {
                    OVER_STIME = "16:" + TextBox_DUTYITEM11_1.Text;
                }

                OVER_ETIME = "17:" + TextBox_DUTYITEM11_2.Text;
            }

            string sele12 = DropDownList_DUTYITEM12.SelectedValue;

            if (sele12 != "")
            {
                if (OVER_STIME == "")
                {
                    OVER_STIME = "17:" + TextBox_DUTYITEM12_1.Text;
                }

                OVER_ETIME = "18:" + TextBox_DUTYITEM12_2.Text;
            }

            string sele13 = DropDownList_DUTYITEM13.SelectedValue;

            if (sele13 != "")
            {
                if (OVER_STIME == "")
                {
                    OVER_STIME = "18:" + TextBox_DUTYITEM13_1.Text;
                }

                OVER_ETIME = "19:" + TextBox_DUTYITEM13_2.Text;
            }

            string sele14 = DropDownList_DUTYITEM14.SelectedValue;

            if (sele14 != "")
            {
                if (OVER_STIME == "")
                {
                    OVER_STIME = "19:" + TextBox_DUTYITEM14_1.Text;
                }

                OVER_ETIME = "20:" + TextBox_DUTYITEM14_2.Text;
            }

            string sele15 = DropDownList_DUTYITEM15.SelectedValue;

            if (sele15 != "")
            {
                if (OVER_STIME == "")
                {
                    OVER_STIME = "20:" + TextBox_DUTYITEM15_1.Text;
                }

                OVER_ETIME = "21:" + TextBox_DUTYITEM15_2.Text;
            }

            string sele16 = DropDownList_DUTYITEM16.SelectedValue;

            if (sele16 != "")
            {
                if (OVER_STIME == "")
                {
                    OVER_STIME = "21:" + TextBox_DUTYITEM16_1.Text;
                }

                OVER_ETIME = "22:" + TextBox_DUTYITEM16_2.Text;
            }

            string sele17 = DropDownList_DUTYITEM17.SelectedValue;

            if (sele17 != "")
            {
                if (OVER_STIME == "")
                {
                    OVER_STIME = "22:" + TextBox_DUTYITEM17_1.Text;
                }

                OVER_ETIME = "23:" + TextBox_DUTYITEM17_2.Text;
            }

            string sele18 = DropDownList_DUTYITEM18.SelectedValue;

            if (sele18 != "")
            {
                if (OVER_STIME == "")
                {
                    OVER_STIME = "23:" + TextBox_DUTYITEM18_1.Text;
                }

                OVER_ETIME = "24:" + TextBox_DUTYITEM18_2.Text;
            }

            string sele19 = DropDownList_DUTYITEM19.SelectedValue;

            if (sele19 != "")
            {
                if (OVER_STIME == "")
                {
                    OVER_STIME = "24:" + TextBox_DUTYITEM19_1.Text;
                }

                OVER_ETIME = "01:" + TextBox_DUTYITEM19_2.Text;
            }

            string sele20 = DropDownList_DUTYITEM20.SelectedValue;

            if (sele20 != "")
            {
                if (OVER_STIME == "")
                {
                    OVER_STIME = "01:" + TextBox_DUTYITEM20_1.Text;
                }

                OVER_ETIME = "02:" + TextBox_DUTYITEM20_2.Text;
            }

            string sele21 = DropDownList_DUTYITEM21.SelectedValue;

            if (sele21 != "")
            {
                if (OVER_STIME == "")
                {
                    OVER_STIME = "02:" + TextBox_DUTYITEM21_1.Text;
                }

                OVER_ETIME = "03:" + TextBox_DUTYITEM21_2.Text;
            }

            string sele22 = DropDownList_DUTYITEM22.SelectedValue;

            if (sele22 != "")
            {
                if (OVER_STIME == "")
                {
                    OVER_STIME = "03:" + TextBox_DUTYITEM22_1.Text;
                }

                OVER_ETIME = "04:" + TextBox_DUTYITEM22_2.Text;
            }

            string sele23 = DropDownList_DUTYITEM23.SelectedValue;

            if (sele23 != "")
            {
                if (OVER_STIME == "")
                {
                    OVER_STIME = "04:" + TextBox_DUTYITEM23_1.Text;
                }

                OVER_ETIME = "05:" + TextBox_DUTYITEM23_2.Text;
            }

            string sele24 = DropDownList_DUTYITEM24.SelectedValue;

            if (sele24 != "")
            {
                if (OVER_STIME == "")
                {
                    OVER_STIME = "05:" + TextBox_DUTYITEM24_1.Text;
                }

                OVER_ETIME = "06:" + TextBox_DUTYITEM24_2.Text;
            }

            string sele25 = DropDownList_DUTYITEM25.SelectedValue;

            if (sele25 != "")
            {
                if (OVER_STIME == "")
                {
                    OVER_STIME = "06:" + TextBox_DUTYITEM25_1.Text;
                }

                OVER_ETIME = "07:" + TextBox_DUTYITEM25_2.Text;
            }

            string sele26 = DropDownList_DUTYITEM26.SelectedValue;

            if (sele26 != "")
            {
                if (OVER_STIME == "")
                {
                    OVER_STIME = "07:" + TextBox_DUTYITEM26_1.Text;
                }

                OVER_ETIME = "08:" + TextBox_DUTYITEM26_2.Text;
            }

            string sele27 = DropDownList_DUTYITEM27.SelectedValue;

            if (sele27 != "")
            {
                if (OVER_STIME == "")
                {
                    OVER_STIME = "08:" + TextBox_DUTYITEM27_1.Text;
                }

                OVER_ETIME = "09:" + TextBox_DUTYITEM27_2.Text;
            }

            #endregion
        }

        /// <summary>
        /// 檢查 內勤人員,本次申請的超勤時數,是否會導致本月加班超過60小時
        /// </summary>
        /// <param name="input_MZ_ID">人員ID</param>
        /// <param name="input_Date">加班日 </param>
        /// <param name="OverTime">當天的超勤時數</param>
        /// <param name="DutyUserClassify">Outter:外勤人員,Inner:內勤人員</param>
        /// <returns>此次申請超勤,是否會讓當月超過60小時? </returns>
        public static bool Check_isOver60Hour_forInnerDuty(string input_MZ_ID, string input_Date, int OverTime, string DutyUserClassify)
        {
            int iSum = Get_ISDIRECTIME_withoutToday(input_MZ_ID, input_Date, OverTime);

            //內勤超過60小時給予警訊
            if (DutyUserClassify == "Inner" && iSum > 60)
            {
                return true;
            }
            //沒事
            return false;
        }

        /// <summary>
        /// 檢查 外勤人員,本次申請的超勤時數,是否會導致本月加班超過80小時
        /// </summary>
        /// <param name="input_MZ_ID"></param>
        /// <param name="input_Date"></param>
        /// <param name="OverTime"></param>
        /// <param name="DutyUserClassify"></param>
        /// <returns></returns>
        public static bool Check_isOver80Hour_forOuterDuty(string input_MZ_ID, string input_Date, int OverTime, string DutyUserClassify)
        {
            int iSum = Get_ISDIRECTIME_withoutToday(input_MZ_ID, input_Date, OverTime);
            //外勤超過80小時給予警訊
            if (DutyUserClassify == "Outter" && iSum > 80)
            {
                return true;
            }
            //沒事
            return false;
        }

        /// <summary>
        /// 取得本月的超勤時數,加上今天的時數,回傳總時數
        /// </summary>
        /// <param name="input_MZ_ID">人員ID</param>
        /// <param name="input_Date">加班日 </param>
        /// <param name="OverTime">當天的超勤時數</param>
        /// <returns></returns>
        private static int Get_ISDIRECTIME_withoutToday(string input_MZ_ID, string input_Date, int OverTime)
        {
            //判斷超勤時數加上當月的超勤時數
            //如果是內勤人員,當月若超過60小時則應給予提醒
            string sum = o_DBFactory.ABC_toTest.vExecSQL(@"SELECT SUM(ISDIRECTTIME) as SUM_ISDIRECTTIME   FROM C_DUTYTABLE_PERSONAL 
                                WHERE MZ_ID='" + input_MZ_ID + @"' 
                                AND DUTYDATE like '" + input_Date.Substring(0, 5) + @"%'
                                AND DUTYDATE<>'" + input_Date + @"'
                                ");
            int iSum = 0;
            int.TryParse(sum, out iSum);
            //本月已申請+本次核算的時數
            iSum = iSum + OverTime;
            return iSum;
        }

        /// <summary>按鈕 : 查詢</summary>
        protected void btSearch_Click(object sender, EventArgs e)
        {
            //20150330 不懂查詢為何要重找一次gridview
            //gv_show();
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('C_ForLeaveOvertime_OVERTIME_FREE_KEYIN_SEARCH.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "','查詢','top=190,left=200,width=500,height=250,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }

        /// <summary>按鈕 : 批次修改</summary>
        protected void btnBatchModify_Click(object sender, EventArgs e)
        {
            //目前僅判斷人名的下拉清單有值才能進行修改
            String SelectedMZ_ID = DropDownList_MZ_NAME.SelectedValue;
            String SelectedMZ_NAME = DropDownList_MZ_NAME.SelectedItem.Text;

            if (String.IsNullOrEmpty(SelectedMZ_ID))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('未選擇人員!');", true);
                return;
            }

            //開啟批次修改視窗
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('C_ForLeaveOvertime_OVERTIME_FREE_KEYIN_BatchModify.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "&id=" + SelectedMZ_ID + "','查詢','top=190,left=200,width=970,height=400,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }

        #region 未有任何地方使用到此func，故直接改名試著觸發錯誤；若確認無誤後可刪除
        protected void returnSameDataType22222(TextBox tb, TextBox tb1)
        {
            tb.Text = o_str.tosql(tb.Text.Trim().Replace("/", ""));

            if (tb.Text != "")
            {
                if (!DateManange.Check_date(tb.Text))
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('日期格式錯誤')", true);
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + tb.ClientID + "').focus();$get('" + tb.ClientID + "').focus();", true);
                }
                else
                {
                    tb.Text = o_DBFactory.ABC_toTest.Personal_ReturnDateString(tb.Text.Trim());
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + tb1.ClientID + "').focus();$get('" + tb1.ClientID + "').focus();", true);
                }
            }
        }

        protected void TextBox_DUTYDATE_TextChanged2222(object sender, EventArgs e)
        {
            //returnSameDataType(TextBox_DUTYDATE, TextBox_DUTYDATE);
        }
        #endregion

        /// <summary>GridView : 點擊列</summary>
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            DropDownList_MZ_NAME.Enabled = true;

            if (e.CommandName == "Select")
            {
                try
                {
                    string strSQL = string.Empty;
                    DataTable temp = new DataTable();

                    String MZ_ID = GridView1.Rows[int.Parse(e.CommandArgument.ToString())].Cells[1].Text;
                    String DutyDate = GridView1.Rows[int.Parse(e.CommandArgument.ToString())].Cells[3].Text;

                    #region 取得轉換之補休時數並回填

                    //取得轉換之補休時數、新增時填寫的內外勤資料，並回填
                    String CovertHourSQL = String.Format(@"SELECT CONVERT_REST_HOURS, DUTYUSERCLASSIFY FROM C_DUTYTABLE_PERSONAL WHERE MZ_ID = '{0}' AND DutyDate = '{1}'"
                                                         , MZ_ID
                                                         , DutyDate);

                    DataTable dt = o_DBFactory.ABC_toTest.Create_Table(CovertHourSQL, "GET");

                    String Hours = String.Empty;
                    String Classify = String.Empty;
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        Hours = dt.Rows[0]["CONVERT_REST_HOURS"].ToString();
                        Classify = dt.Rows[0]["DUTYUSERCLASSIFY"].ToString();
                    }

                    txtConvertRest.Text = Hours;
                    RadioButtonList_WorkP.SelectedValue = Classify;
                    #endregion

                    #region 取得並回填

                    //String DutyUserClassifySQL = String.Format(@"SELECT DUTYUSERCLASSIFY FROM C_DUTYTABLE_PERSONAL WHERE MZ_ID = @MZ_ID AND DutyDate = @DutyDate"
                    //                                           ,MZ_ID

                    //List<SqlParameter> lstParam2 = new List<SqlParameter>();
                    //                      lstParam2.Add(new SqlParameter("MZ_ID", SqlDbType.NVarChar) { Value = MZ_ID }); //身分證字號
                    //                      lstParam2.Add(new SqlParameter("DutyDate", SqlDbType.NVarChar) { Value = DutyDate }); //加班類型

                    //String Classify = o_DBFactory.ABC_toTest.GetValue(DutyUserClassifySQL, lstParam2);

                    //協助其載入內外勤人員選項


                    #endregion

                    /// 是否停休
                    string dutyStopOff = GetDutyStopOff(MZ_ID, DutyDate);
                    RadioButtonList_StopOff.SelectedValue = dutyStopOff;

                    //需要問小隊長是否調單位後  還能不能編輯以前的超勤資料?
                    strSQL = string.Format("SELECT MZ_ID FROM A_DLBASE WHERE MZ_ID='{0}' AND MZ_EXAD='{1}' AND MZ_EXUNIT='{2}'",
                                            MZ_ID,
                                            Session["ADPMZ_EXAD"].ToString(),
                                            Session["ADPMZ_EXUNIT"].ToString());

                    if ((o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET").Rows.Count > 0))
                    {
                        //若是經由查詢帶回來的，從網址列帶參數
                        if (Request["UNIT"] != null || Request["UNIT"] != "")
                        {
                            if (Request["UNIT"].ToString().Trim().Length > 0)
                            {
                                strSQL = "SELECT MZ_ID,MZ_NAME FROM A_DLBASE WHERE MZ_EXAD='" + Request["AD"] + "' AND MZ_EXUNIT='" + Request["UNIT"] + "'";
                                SqlDataSource3.SelectParameters.Clear();
                                SqlDataSource3.SelectCommand = strSQL;
                                DropDownList_MZ_NAME.DataBind();
                                if (DropDownList_MZ_NAME.Items.Count > 0)
                                    DropDownList_MZ_NAME.SelectedValue = MZ_ID;
                            }
                            else
                            {
                                strSQL = "SELECT MZ_ID,MZ_NAME FROM A_DLBASE WHERE MZ_EXAD='" + Request["AD"] + "'";
                                SqlDataSource3.SelectParameters.Clear();
                                SqlDataSource3.SelectCommand = strSQL;
                                DropDownList_MZ_NAME.DataBind();
                                if (DropDownList_MZ_NAME.Items.Count > 0)
                                    DropDownList_MZ_NAME.SelectedValue = MZ_ID;
                            }
                        }
                        else
                        {
                            if (!tbMZ_ID.Visible) //irk 增加 調離單位修改
                            {
                                strSQL = "SELECT MZ_ID,MZ_NAME FROM A_DLBASE WHERE MZ_EXAD='" + Request["AD"] + "'";
                                SqlDataSource3.SelectParameters.Clear();
                                SqlDataSource3.SelectCommand = strSQL;
                                DropDownList_MZ_NAME.DataBind();
                                if (DropDownList_MZ_NAME.Items.Count > 0)
                                    DropDownList_MZ_NAME.SelectedValue = MZ_ID;
                            }
                        }
                    }
                    else //若ID不存在則無法編輯
                    {
                        tbMZ_ID.Visible = true;
                        DropDownList_MZ_NAME.Visible = false;
                        tbMZ_ID.Text = MZ_ID;
                        tbChange(true);
                    }
                }
                catch (Exception ex)
                {
                    //記錄錯誤
                    Log.SaveLog("C_ForLeaveOvertime_OVERTIME_FREE_KEYIN", "1", ex.ToString() + "\r\n" + ex.StackTrace);
                }



                //gv_show();

                GridView1.SelectedIndex = Convert.ToInt32(e.CommandArgument);

                if (GridView1.Rows.Count > 0)
                {
                    TextBox_DUTYDATE.Text = GridView1.SelectedRow.Cells[3].Text;

                    DropDownList_DUTYITEM1.SelectedValue = GridView1.SelectedRow.Cells[4].Text == "&nbsp;" ? "" : GridView1.SelectedRow.Cells[4].Text;
                    DropDownList_DUTYITEM2.SelectedValue = GridView1.SelectedRow.Cells[5].Text == "&nbsp;" ? "" : GridView1.SelectedRow.Cells[5].Text;
                    DropDownList_DUTYITEM3.SelectedValue = GridView1.SelectedRow.Cells[6].Text == "&nbsp;" ? "" : GridView1.SelectedRow.Cells[6].Text;
                    DropDownList_DUTYITEM4.SelectedValue = GridView1.SelectedRow.Cells[7].Text == "&nbsp;" ? "" : GridView1.SelectedRow.Cells[7].Text;
                    DropDownList_DUTYITEM5.SelectedValue = GridView1.SelectedRow.Cells[8].Text == "&nbsp;" ? "" : GridView1.SelectedRow.Cells[8].Text;
                    DropDownList_DUTYITEM6.SelectedValue = GridView1.SelectedRow.Cells[9].Text == "&nbsp;" ? "" : GridView1.SelectedRow.Cells[9].Text;
                    DropDownList_DUTYITEM7.SelectedValue = GridView1.SelectedRow.Cells[10].Text == "&nbsp;" ? "" : GridView1.SelectedRow.Cells[10].Text;
                    DropDownList_DUTYITEM8.SelectedValue = GridView1.SelectedRow.Cells[11].Text == "&nbsp;" ? "" : GridView1.SelectedRow.Cells[11].Text;
                    DropDownList_DUTYITEM9.SelectedValue = GridView1.SelectedRow.Cells[12].Text == "&nbsp;" ? "" : GridView1.SelectedRow.Cells[12].Text;
                    DropDownList_DUTYITEM10.SelectedValue = GridView1.SelectedRow.Cells[13].Text == "&nbsp;" ? "" : GridView1.SelectedRow.Cells[13].Text;
                    DropDownList_DUTYITEM11.SelectedValue = GridView1.SelectedRow.Cells[14].Text == "&nbsp;" ? "" : GridView1.SelectedRow.Cells[14].Text;
                    DropDownList_DUTYITEM12.SelectedValue = GridView1.SelectedRow.Cells[15].Text == "&nbsp;" ? "" : GridView1.SelectedRow.Cells[15].Text;
                    DropDownList_DUTYITEM13.SelectedValue = GridView1.SelectedRow.Cells[16].Text == "&nbsp;" ? "" : GridView1.SelectedRow.Cells[16].Text;
                    DropDownList_DUTYITEM14.SelectedValue = GridView1.SelectedRow.Cells[17].Text == "&nbsp;" ? "" : GridView1.SelectedRow.Cells[17].Text;
                    DropDownList_DUTYITEM15.SelectedValue = GridView1.SelectedRow.Cells[18].Text == "&nbsp;" ? "" : GridView1.SelectedRow.Cells[18].Text;
                    DropDownList_DUTYITEM16.SelectedValue = GridView1.SelectedRow.Cells[19].Text == "&nbsp;" ? "" : GridView1.SelectedRow.Cells[19].Text;
                    DropDownList_DUTYITEM17.SelectedValue = GridView1.SelectedRow.Cells[20].Text == "&nbsp;" ? "" : GridView1.SelectedRow.Cells[20].Text;
                    DropDownList_DUTYITEM18.SelectedValue = GridView1.SelectedRow.Cells[21].Text == "&nbsp;" ? "" : GridView1.SelectedRow.Cells[21].Text;
                    DropDownList_DUTYITEM19.SelectedValue = GridView1.SelectedRow.Cells[22].Text == "&nbsp;" ? "" : GridView1.SelectedRow.Cells[22].Text;
                    DropDownList_DUTYITEM20.SelectedValue = GridView1.SelectedRow.Cells[23].Text == "&nbsp;" ? "" : GridView1.SelectedRow.Cells[23].Text;
                    DropDownList_DUTYITEM21.SelectedValue = GridView1.SelectedRow.Cells[24].Text == "&nbsp;" ? "" : GridView1.SelectedRow.Cells[24].Text;
                    DropDownList_DUTYITEM22.SelectedValue = GridView1.SelectedRow.Cells[25].Text == "&nbsp;" ? "" : GridView1.SelectedRow.Cells[25].Text;
                    DropDownList_DUTYITEM23.SelectedValue = GridView1.SelectedRow.Cells[26].Text == "&nbsp;" ? "" : GridView1.SelectedRow.Cells[26].Text;
                    DropDownList_DUTYITEM24.SelectedValue = GridView1.SelectedRow.Cells[27].Text == "&nbsp;" ? "" : GridView1.SelectedRow.Cells[27].Text;
                    DropDownList_DUTYITEM25.SelectedValue = GridView1.SelectedRow.Cells[28].Text == "&nbsp;" ? "" : GridView1.SelectedRow.Cells[28].Text;
                    DropDownList_DUTYITEM26.SelectedValue = GridView1.SelectedRow.Cells[29].Text == "&nbsp;" ? "" : GridView1.SelectedRow.Cells[29].Text;
                    DropDownList_DUTYITEM27.SelectedValue = GridView1.SelectedRow.Cells[30].Text == "&nbsp;" ? "" : GridView1.SelectedRow.Cells[30].Text;

                    timeShow("get");

                    // Joy 20150806 顯示該資料所屬機關單位
                    string MZ_ID = GridView1.SelectedRow.Cells[1].Text;
                    string MZ_Date = GridView1.SelectedRow.Cells[3].Text;
                    string sql_FindAD = "select MZ_AD, MZ_UNIT from C_DUTYTABLE_PERSONAL where MZ_ID ='" + MZ_ID + "' and DUTYDATE = '" + MZ_Date + "' ";
                    DataTable ADUNIT = o_DBFactory.ABC_toTest.Create_Table(sql_FindAD, "GET");

                    String AD_finded = ADUNIT.Rows[0]["MZ_AD"].ToString();
                    String UNIT_finded = ADUNIT.Rows[0]["MZ_UNIT"].ToString();

                    //機關下拉選單值及設定
                    ListItem li = new ListItem(" ", "");
                    C.fill_AD_POST(DropDownList_AD);
                    C.fill_unit(DropDownList_UNIT, AD_finded);

                    //sam wellsince 20201015 發薪單位為 內政部警政署 故找無
                    //取得該筆紀錄之機關資料
                    //DropDownList_AD.SelectedValue = AD_finded;

                    if (DropDownList_AD.Items.FindByValue("") != null)
                        DropDownList_AD.Items.Insert(0, li);
                    if (DropDownList_UNIT.Items.FindByValue("") != null)
                        DropDownList_UNIT.Items.Insert(0, li);

                    //取得該筆超勤紀錄之單位；若使用者曾經調過單位，則此下拉維持預設值即可
                    if (DropDownList_UNIT.Items.FindByValue(UNIT_finded) != null)
                        DropDownList_UNIT.SelectedValue = UNIT_finded;
                }

                btInsert.Enabled = false;
                btUpdate.Enabled = true;
                btDelete.Enabled = true;
                btCancel.Enabled = true;
                btPrint.Enabled = true;
                btOK.Enabled = false;
                C.controlEnable(ref this.Panel3, false);
                C.controlEnable(ref this.Panel2, false);
            }
        }
        // 機關變更時 單位選項連帶變更
        protected void DropDownList_AD_SelectedIndexChanged(object sender, EventArgs e)
        {
            C.fill_unit(DropDownList_UNIT, DropDownList_AD.SelectedValue);
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GridView1, "Select$" + e.Row.RowIndex);
                e.Row.Cells[0].Attributes.Add("Style", "display:none");
            }
        }

        protected void DropDownList_MZ_NAME_SelectedIndexChanged(object sender, EventArgs e)
        {
            Label_ADUN.Text = o_DBFactory.ABC_toTest.CAD(DropDownList_MZ_NAME.SelectedValue) + " " + o_DBFactory.ABC_toTest.CUNIT(DropDownList_MZ_NAME.SelectedValue);

            //協助其載入內外勤人員選項
            RadioButtonList_WorkP.SelectedValue = AccountService.getDutyUserClassify_V1(DropDownList_MZ_NAME.SelectedValue).ToString();

            gv_show();

            //sam clear
            txtConvertRest.Text = string.Empty;
        }

        protected void timeShow(string mode)
        {
            if (mode == "get")
            {
                #region 取值回去
                string selectString = "SELECT * FROM C_DUTYTABLE_PERSONAL WHERE MZ_ID='" + (tbMZ_ID.Visible ? tbMZ_ID.Text : DropDownList_MZ_NAME.SelectedValue) + "' AND DUTYDATE='" + TextBox_DUTYDATE.Text + "'";

                DataTable dt = new DataTable();

                dt = o_DBFactory.ABC_toTest.Create_Table(selectString, "get1");

                if (dt.Rows.Count == 1)
                {
                    if (dt.Rows[0]["TIME1"].ToString().Length == 9)
                    {
                        TextBox_DUTYITEM1_1.Text = dt.Rows[0]["TIME1"].ToString().Substring(2, 2);
                        TextBox_DUTYITEM1_2.Text = dt.Rows[0]["TIME1"].ToString().Substring(7, 2);
                    }
                    if (dt.Rows[0]["TIME2"].ToString().Length == 9)
                    {
                        TextBox_DUTYITEM2_1.Text = dt.Rows[0]["TIME2"].ToString().Substring(2, 2);
                        TextBox_DUTYITEM2_2.Text = dt.Rows[0]["TIME2"].ToString().Substring(7, 2);
                    }
                    if (dt.Rows[0]["TIME3"].ToString().Length == 9)
                    {
                        TextBox_DUTYITEM3_1.Text = dt.Rows[0]["TIME3"].ToString().Substring(2, 2);
                        TextBox_DUTYITEM3_2.Text = dt.Rows[0]["TIME3"].ToString().Substring(7, 2);
                    }
                    if (dt.Rows[0]["TIME4"].ToString().Length == 9)
                    {
                        TextBox_DUTYITEM4_1.Text = dt.Rows[0]["TIME4"].ToString().Substring(2, 2);
                        TextBox_DUTYITEM4_2.Text = dt.Rows[0]["TIME4"].ToString().Substring(7, 2);
                    }
                    if (dt.Rows[0]["TIME5"].ToString().Length == 9)
                    {
                        TextBox_DUTYITEM5_1.Text = dt.Rows[0]["TIME5"].ToString().Substring(2, 2);
                        TextBox_DUTYITEM5_2.Text = dt.Rows[0]["TIME5"].ToString().Substring(7, 2);
                    }
                    if (dt.Rows[0]["TIME6"].ToString().Length == 9)
                    {
                        TextBox_DUTYITEM6_1.Text = dt.Rows[0]["TIME6"].ToString().Substring(2, 2);
                        TextBox_DUTYITEM6_2.Text = dt.Rows[0]["TIME6"].ToString().Substring(7, 2);
                    }
                    if (dt.Rows[0]["TIME7"].ToString().Length == 9)
                    {
                        TextBox_DUTYITEM7_1.Text = dt.Rows[0]["TIME7"].ToString().Substring(2, 2);
                        TextBox_DUTYITEM7_2.Text = dt.Rows[0]["TIME7"].ToString().Substring(7, 2);
                    }
                    if (dt.Rows[0]["TIME8"].ToString().Length == 9)
                    {
                        TextBox_DUTYITEM8_1.Text = dt.Rows[0]["TIME8"].ToString().Substring(2, 2);
                        TextBox_DUTYITEM8_2.Text = dt.Rows[0]["TIME8"].ToString().Substring(7, 2);
                    }
                    if (dt.Rows[0]["TIME9"].ToString().Length == 9)
                    {
                        TextBox_DUTYITEM9_1.Text = dt.Rows[0]["TIME9"].ToString().Substring(2, 2);
                        TextBox_DUTYITEM9_2.Text = dt.Rows[0]["TIME9"].ToString().Substring(7, 2);
                    }
                    if (dt.Rows[0]["TIME10"].ToString().Length == 9)
                    {
                        TextBox_DUTYITEM10_1.Text = dt.Rows[0]["TIME10"].ToString().Substring(2, 2);
                        TextBox_DUTYITEM10_2.Text = dt.Rows[0]["TIME10"].ToString().Substring(7, 2);
                    }
                    if (dt.Rows[0]["TIME11"].ToString().Length == 9)
                    {
                        TextBox_DUTYITEM11_1.Text = dt.Rows[0]["TIME11"].ToString().Substring(2, 2);
                        TextBox_DUTYITEM11_2.Text = dt.Rows[0]["TIME11"].ToString().Substring(7, 2);
                    }
                    if (dt.Rows[0]["TIME12"].ToString().Length == 9)
                    {
                        TextBox_DUTYITEM12_1.Text = dt.Rows[0]["TIME12"].ToString().Substring(2, 2);
                        TextBox_DUTYITEM12_2.Text = dt.Rows[0]["TIME12"].ToString().Substring(7, 2);
                    }
                    if (dt.Rows[0]["TIME13"].ToString().Length == 9)
                    {
                        TextBox_DUTYITEM13_1.Text = dt.Rows[0]["TIME13"].ToString().Substring(2, 2);
                        TextBox_DUTYITEM13_2.Text = dt.Rows[0]["TIME13"].ToString().Substring(7, 2);
                    }
                    if (dt.Rows[0]["TIME14"].ToString().Length == 9)
                    {
                        TextBox_DUTYITEM14_1.Text = dt.Rows[0]["TIME14"].ToString().Substring(2, 2);
                        TextBox_DUTYITEM14_2.Text = dt.Rows[0]["TIME14"].ToString().Substring(7, 2);
                    }
                    if (dt.Rows[0]["TIME15"].ToString().Length == 9)
                    {
                        TextBox_DUTYITEM15_1.Text = dt.Rows[0]["TIME15"].ToString().Substring(2, 2);
                        TextBox_DUTYITEM15_2.Text = dt.Rows[0]["TIME15"].ToString().Substring(7, 2);
                    }
                    if (dt.Rows[0]["TIME16"].ToString().Length == 9)
                    {
                        TextBox_DUTYITEM16_1.Text = dt.Rows[0]["TIME16"].ToString().Substring(2, 2);
                        TextBox_DUTYITEM16_2.Text = dt.Rows[0]["TIME16"].ToString().Substring(7, 2);
                    }
                    if (dt.Rows[0]["TIME17"].ToString().Length == 9)
                    {
                        TextBox_DUTYITEM17_1.Text = dt.Rows[0]["TIME17"].ToString().Substring(2, 2);
                        TextBox_DUTYITEM17_2.Text = dt.Rows[0]["TIME17"].ToString().Substring(7, 2);
                    }
                    if (dt.Rows[0]["TIME18"].ToString().Length == 9)
                    {
                        TextBox_DUTYITEM18_1.Text = dt.Rows[0]["TIME18"].ToString().Substring(2, 2);
                        TextBox_DUTYITEM18_2.Text = dt.Rows[0]["TIME18"].ToString().Substring(7, 2);
                    }
                    if (dt.Rows[0]["TIME19"].ToString().Length == 9)
                    {
                        TextBox_DUTYITEM19_1.Text = dt.Rows[0]["TIME19"].ToString().Substring(2, 2);
                        TextBox_DUTYITEM19_2.Text = dt.Rows[0]["TIME19"].ToString().Substring(7, 2);
                    }
                    if (dt.Rows[0]["TIME20"].ToString().Length == 9)
                    {
                        TextBox_DUTYITEM20_1.Text = dt.Rows[0]["TIME20"].ToString().Substring(2, 2);
                        TextBox_DUTYITEM20_2.Text = dt.Rows[0]["TIME20"].ToString().Substring(7, 2);
                    }
                    if (dt.Rows[0]["TIME21"].ToString().Length == 9)
                    {
                        TextBox_DUTYITEM21_1.Text = dt.Rows[0]["TIME21"].ToString().Substring(2, 2);
                        TextBox_DUTYITEM21_2.Text = dt.Rows[0]["TIME21"].ToString().Substring(7, 2);
                    }
                    if (dt.Rows[0]["TIME22"].ToString().Length == 9)
                    {
                        TextBox_DUTYITEM22_1.Text = dt.Rows[0]["TIME22"].ToString().Substring(2, 2);
                        TextBox_DUTYITEM22_2.Text = dt.Rows[0]["TIME22"].ToString().Substring(7, 2);
                    }
                    if (dt.Rows[0]["TIME23"].ToString().Length == 9)
                    {
                        TextBox_DUTYITEM23_1.Text = dt.Rows[0]["TIME23"].ToString().Substring(2, 2);
                        TextBox_DUTYITEM23_2.Text = dt.Rows[0]["TIME23"].ToString().Substring(7, 2);
                    }
                    if (dt.Rows[0]["TIME24"].ToString().Length == 9)
                    {
                        TextBox_DUTYITEM24_1.Text = dt.Rows[0]["TIME24"].ToString().Substring(2, 2);
                        TextBox_DUTYITEM24_2.Text = dt.Rows[0]["TIME24"].ToString().Substring(7, 2);
                    }
                    if (dt.Rows[0]["TIME25"].ToString().Length == 9)
                    {
                        TextBox_DUTYITEM25_1.Text = dt.Rows[0]["TIME25"].ToString().Substring(2, 2);
                        TextBox_DUTYITEM25_2.Text = dt.Rows[0]["TIME25"].ToString().Substring(7, 2);
                    }
                    if (dt.Rows[0]["TIME26"].ToString().Length == 9)
                    {
                        TextBox_DUTYITEM26_1.Text = dt.Rows[0]["TIME26"].ToString().Substring(2, 2);
                        TextBox_DUTYITEM26_2.Text = dt.Rows[0]["TIME26"].ToString().Substring(7, 2);
                    }
                    if (dt.Rows[0]["TIME27"].ToString().Length == 9)
                    {
                        TextBox_DUTYITEM27_1.Text = dt.Rows[0]["TIME27"].ToString().Substring(2, 2);
                        TextBox_DUTYITEM27_2.Text = dt.Rows[0]["TIME27"].ToString().Substring(7, 2);
                    }
                    TextBox_MZ_MEMO.Text = dt.Rows[0]["MZ_MEMO"].ToString();
                }
                #endregion 
            }
            else if (mode == "insert")
            {
                #region 插入值
                TextBox_DUTYITEM1_1.Text = "00";
                TextBox_DUTYITEM1_2.Text = "00";
                TextBox_DUTYITEM2_1.Text = "00";
                TextBox_DUTYITEM2_2.Text = "00";
                TextBox_DUTYITEM3_1.Text = "00";
                TextBox_DUTYITEM3_2.Text = "00";
                TextBox_DUTYITEM4_1.Text = "00";
                TextBox_DUTYITEM4_2.Text = "00";
                TextBox_DUTYITEM5_1.Text = "00";
                TextBox_DUTYITEM5_2.Text = "00";
                TextBox_DUTYITEM6_1.Text = "00";
                TextBox_DUTYITEM6_2.Text = "00";
                TextBox_DUTYITEM7_1.Text = "00";
                TextBox_DUTYITEM7_2.Text = "00";
                TextBox_DUTYITEM8_1.Text = "00";
                TextBox_DUTYITEM8_2.Text = "00";
                TextBox_DUTYITEM9_1.Text = "00";
                TextBox_DUTYITEM9_2.Text = "00";
                TextBox_DUTYITEM10_1.Text = "00";
                TextBox_DUTYITEM10_2.Text = "00";
                TextBox_DUTYITEM11_1.Text = "00";
                TextBox_DUTYITEM11_2.Text = "00";
                TextBox_DUTYITEM12_1.Text = "00";
                TextBox_DUTYITEM12_2.Text = "00";
                TextBox_DUTYITEM13_1.Text = "00";
                TextBox_DUTYITEM13_2.Text = "00";
                TextBox_DUTYITEM14_1.Text = "00";
                TextBox_DUTYITEM14_2.Text = "00";
                TextBox_DUTYITEM15_1.Text = "00";
                TextBox_DUTYITEM15_2.Text = "00";
                TextBox_DUTYITEM16_1.Text = "00";
                TextBox_DUTYITEM16_2.Text = "00";
                TextBox_DUTYITEM17_1.Text = "00";
                TextBox_DUTYITEM17_2.Text = "00";
                TextBox_DUTYITEM18_1.Text = "00";
                TextBox_DUTYITEM18_2.Text = "00";
                TextBox_DUTYITEM19_1.Text = "00";
                TextBox_DUTYITEM19_2.Text = "00";
                TextBox_DUTYITEM20_1.Text = "00";
                TextBox_DUTYITEM20_2.Text = "00";
                TextBox_DUTYITEM21_1.Text = "00";
                TextBox_DUTYITEM21_2.Text = "00";
                TextBox_DUTYITEM22_1.Text = "00";
                TextBox_DUTYITEM22_2.Text = "00";
                TextBox_DUTYITEM23_1.Text = "00";
                TextBox_DUTYITEM23_2.Text = "00";
                TextBox_DUTYITEM24_1.Text = "00";
                TextBox_DUTYITEM24_2.Text = "00";
                TextBox_DUTYITEM25_1.Text = "00";
                TextBox_DUTYITEM25_2.Text = "00";
                TextBox_DUTYITEM26_1.Text = "00";
                TextBox_DUTYITEM26_2.Text = "00";
                TextBox_DUTYITEM27_1.Text = "00";
                TextBox_DUTYITEM27_2.Text = "00";
                TextBox_MZ_MEMO.Text = "";
                #endregion 
            }
        }//end timeshow()

        /// <summary>按鈕 : 調離單位申請</summary>
        protected void btnJobRelocationInput_Click(object sender, EventArgs e)
        {
            #region 表單元件控制
            C.controlEnable(ref this.Panel3, true);
            C.controlEnable(ref this.Panel2, true);

            ViewState["Mode"] = "JobRelocation";
            ViewState["CMDSQL"] = "INSERT INTO C_DUTYTABLE_PERSONAL (MZ_AD,MZ_UNIT, MZ_EXAD, MZ_EXUNIT, MZ_ID, DUTYDATE,TIME1,DUTYITEM1,TIME2,DUTYITEM2,TIME3,DUTYITEM3," +
                                                                     "TIME4,DUTYITEM4,TIME5,DUTYITEM5,TIME6,DUTYITEM6,TIME7,DUTYITEM7,TIME8,DUTYITEM8," +
                                                                     "TIME9,DUTYITEM9,TIME10,DUTYITEM10,TIME11,DUTYITEM11,TIME12,DUTYITEM12,TIME13,DUTYITEM13," +
                                                                     "TIME14,DUTYITEM14,TIME15,DUTYITEM15,TIME16,DUTYITEM16,TIME17,DUTYITEM17,TIME18,DUTYITEM18," +
                                                                     "TIME19,DUTYITEM19,TIME20,DUTYITEM20,TIME21,DUTYITEM21,TIME22,DUTYITEM22,TIME23,DUTYITEM23," +
                                                                     "TIME24,DUTYITEM24,TIME25,DUTYITEM25,TIME26,DUTYITEM26,TIME27,DUTYITEM27,TOTAL_HOURS,ISDIRECTTIME,MZ_MEMO, CONVERT_REST_HOURS," +
                                                                     "CREATEUSER, CREATEDATE, DUTYSTOPOFF, DUTYUSERCLASSIFY) " +
                                                           " VALUES (@MZ_AD,@MZ_UNIT, @MZ_EXAD, @MZ_EXUNIT, @MZ_ID,@DUTYDATE,@TIME1,@DUTYITEM1,@TIME2,@DUTYITEM2,@TIME3,@DUTYITEM3," +
                                                                    ":TIME4,@DUTYITEM4,@TIME5,@DUTYITEM5,@TIME6,@DUTYITEM6,@TIME7,@DUTYITEM7,@TIME8,@DUTYITEM8," +
                                                                    ":TIME9,@DUTYITEM9,@TIME10,@DUTYITEM10,@TIME11,@DUTYITEM11,@TIME12,@DUTYITEM12,@TIME13,@DUTYITEM13," +
                                                                    ":TIME14,@DUTYITEM14,@TIME15,@DUTYITEM15,@TIME16,@DUTYITEM16,@TIME17,@DUTYITEM17,@TIME18,@DUTYITEM18," +
                                                                    ":TIME19,@DUTYITEM19,@TIME20,@DUTYITEM20,@TIME21,@DUTYITEM21,@TIME22,@DUTYITEM22,@TIME23,@DUTYITEM23," +
                                                                    ":TIME24,@DUTYITEM24,@TIME25,@DUTYITEM25,@TIME26,@DUTYITEM26,@TIME27,@DUTYITEM27,@TOTAL_HOURS,@ISDIRECTTIME,@MZ_MEMO, @CONVERT_REST_HOURS," +
                                                                    ":CREATEUSER, @CREATEDATE, @DUTYSTOPOFF, @DUTYUSERCLASSIFY) ";
            lbName.Visible = true;  //打開人員姓名
            tbMZ_ID.Visible = true; //打開身分證字號
            DropDownList_MZ_NAME.Visible = false; //人員下拉選單關閉
            lblIDNAME.Visible = true; //說明文字 : 請輸入身分證字號

            btnJobRelocationInput.Visible = false;
            btnJobRelocationConfrm.Visible = true;
            btnJobRelocationCancel.Visible = true;

            timeShow("insert");
            #endregion 

            gv_show();
        }

        /// <summary>按鈕: 取消調離單位申請</summary>
        protected void btnJobRelocationCancel_Click(object sender, EventArgs e)
        {
            #region 表單元件控制
            foreach (object obj in Panel2.Controls)
            {
                if (obj is DropDownList)
                {
                    DropDownList dd = obj as DropDownList;
                    dd.SelectedIndex = -1;
                }
            }

            C.controlEnable(ref this.Panel3, false);
            C.controlEnable(ref this.Panel2, false);

            btnJobRelocationInput.Enabled = true;
            btnJobRelocationInput.Visible = true; //開啟主按鈕
            btnJobRelocationCancel.Visible = false;
            btnJobRelocationConfrm.Visible = false;

            TextBox_DUTYDATE.Text = ""; //清空輸入藍
            lbName.Visible = false;  //關閉人員姓名
            lbName.Text = ""; //清空人員姓名

            tbMZ_ID.Visible = false; //關閉身分證字號
            tbMZ_ID.Text = ""; //清空身分證字號
            DropDownList_MZ_NAME.Visible = true; //開啟下拉選單關閉
            DropDownList_MZ_NAME.Enabled = true;
            lblIDNAME.Visible = false; //不顯示說明文字 : 請輸入身分證字號
            Label_ADUN.Text = ""; //清空單位顯示
            TextBox_MZ_MEMO.Text = ""; //清空註解欄位

            #endregion
        }

        /// <summary>按鈕 : 確認調離單位申請</summary>
        protected void btnJobRelocationConfrm_Click(object sender, EventArgs e)
        {
            btok_Click(this, e);

        }

        protected void tbMZ_ID_TextChanged(object sender, EventArgs e)
        {
            tbChange();
        }

        private void tbChange(Boolean isUpdate = false)
        {
            string strSQL = string.Format("Select mz_name from vw_dlbase_basic where mz_id='{0}'", tbMZ_ID.Text);
            DataTable temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
            if (temp.Rows.Count > 0)
            {
                lbName.Visible = true;
                tbMZ_ID.Visible = true;
                lbName.Text = temp.Rows[0][0].ToString();
                Label_ADUN.Text = o_DBFactory.ABC_toTest.CAD(tbMZ_ID.Visible ? tbMZ_ID.Text : DropDownList_MZ_NAME.SelectedValue) + " " + o_DBFactory.ABC_toTest.CUNIT(tbMZ_ID.Visible ? tbMZ_ID.Text : DropDownList_MZ_NAME.SelectedValue);
                lblIDNAME.Visible = false;

                //協助其載入內外勤人員選項(人員之原始值)
                //編輯時因為要帶入新增時輸入的值，故需判斷是否避免重撈預設值
                if (!isUpdate)
                    RadioButtonList_WorkP.SelectedValue = AccountService.getDutyUserClassify_V1(tbMZ_ID.Text).ToString();
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(lbName, this.GetType(), "click", "alert('查無此人');", true);
            }
        }

        /// <summary>舊的，確認無誤後可以整個刪除</summary>
        private void gv_show1111()
        {
            //使用者是否有進行查詢動作
            Boolean isSearchCallBack = (Request["type"] != null) ? true : false;

            if (isSearchCallBack)
            {

            }
            else
            {

            }

            if (ViewState["MZ_ID"] != null)
            {
                string strSQL = "SELECT C_DUTYTABLE_PERSONAL.*,MZ_NAME FROM C_DUTYTABLE_PERSONAL LEFT JOIN A_DLBASE ON C_DUTYTABLE_PERSONAL.MZ_ID = A_DLBASE.MZ_ID WHERE 1=1 ";
                if (!(Session["ADPMZ_ID"].ToString() == "K220886357"))//20141104先開給鳳美姊 之後再釐清邱課是怎麼開權限給他用超勤
                {
                    switch (MZPower)
                    {
                        case "D":
                            strSQL += " AND C_DUTYTABLE_PERSONAL.MZ_ID = '" + Session["ADPMZ_ID"].ToString() + "'";
                            break;
                    }
                }
                if (ViewState["MZ_ID"].ToString() != "")
                {
                    strSQL += " AND C_DUTYTABLE_PERSONAL.MZ_ID='" + ViewState["MZ_ID"].ToString() + "'";
                }
                if (ViewState["MZ_NAME"].ToString() != "")
                {
                    strSQL += " AND C_DUTYTABLE_PERSONAL.MZ_ID in (SELECT MZ_ID FROM A_DLBASE WHERE MZ_NAME LIKE '%" + ViewState["MZ_NAME"].ToString() + "%')";
                }
                if (Request["AD"] != "")
                {
                    strSQL += " AND  C_DUTYTABLE_PERSONAL.MZ_AD='" + Request["AD"] + "'";
                }
                if (Request["UNIT"] != "")
                {
                    strSQL += " AND C_DUTYTABLE_PERSONAL.MZ_UNIT='" + Request["UNIT"] + "'";
                }

                if (Session["ForLeaveOvertime_OVERTIME_FREE_KEYIN_DATE"].ToString() != "")
                {
                    strSQL += " AND DUTYDATE like '" + Session["ForLeaveOvertime_OVERTIME_FREE_KEYIN_DATE"].ToString() + "%' ";
                }

                strSQL += " ORDER BY C_DUTYTABLE_PERSONAL.MZ_ID";
                SqlDataSource1.SelectCommand = strSQL;
                SqlDataSource1.SelectParameters.Clear();
                GridView1.DataBind();
            }
            else
            {
                String MZ_ID = tbMZ_ID.Visible ? tbMZ_ID.Text : DropDownList_MZ_NAME.SelectedValue;
                String SES_ADPMZ_EXAD = Session["ADPMZ_EXAD"].ToString();
                string strSQL = String.Format(
                                @"SELECT
                                    C_DUTYTABLE_PERSONAL.MZ_ID,
                                    MZ_NAME,DUTYDATE,
                                    DUTYITEM1, DUTYITEM2, DUTYITEM3, DUTYITEM4, DUTYITEM5, DUTYITEM6, DUTYITEM7, DUTYITEM8, DUTYITEM9, DUTYITEM10, DUTYITEM11, DUTYITEM12,
                                    DUTYITEM13, DUTYITEM14, DUTYITEM15, DUTYITEM16, DUTYITEM17, DUTYITEM18, DUTYITEM19, DUTYITEM20, DUTYITEM21, DUTYITEM22, DUTYITEM23, DUTYITEM24, DUTYITEM25, DUTYITEM26,DUTYITEM27,
                                    TOTAL_HOURS, ISDIRECTTIME
                                  FROM C_DUTYTABLE_PERSONAL
                                  LEFT JOIN A_DLBASE ON C_DUTYTABLE_PERSONAL.MZ_ID = A_DLBASE.MZ_ID
                                  WHERE 1=1 AND C_DUTYTABLE_PERSONAL.MZ_ID = '{0}' AND C_DUTYTABLE_PERSONAL.MZ_AD='{1}'"
                                , MZ_ID
                                , SES_ADPMZ_EXAD);


                switch (MZPower)
                {
                    //20140128

                    case "D":
                        break;
                    default:

                        strSQL += " AND C_DUTYTABLE_PERSONAL.MZ_UNIT='" + Session["ADPMZ_EXUNIT"].ToString() + "'";
                        break;

                }
                //20150330
                if (TextBox_DUTYDATE.Text.Trim() == "")
                {
                    strSQL += "  AND dbo.dbo.SUBSTR(DUTYDATE,0,5)='" + (DateTime.Now.Year - 1911).ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0') + "'";
                }
                else
                {
                    if (TextBox_DUTYDATE.Text.Trim().Length >= 5)
                        strSQL += "  AND dbo.dbo.SUBSTR(DUTYDATE,0,5)='" + TextBox_DUTYDATE.Text.Trim().Substring(0, 5) + "'";
                    else
                        strSQL += "  AND dbo.dbo.SUBSTR(DUTYDATE,0,5)='" + (DateTime.Now.Year - 1911).ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0') + "'";

                }

                SqlDataSource1.SelectCommand = strSQL;
                SqlDataSource1.SelectParameters.Clear();
                GridView1.DataBind();
            }
        }

        private void gv_show()
        {
            //SESSION統一管理
            String SES_ADPMZ_ID = Session["ADPMZ_ID"].ToString();
            String SES_Search_DutyDate = Session["OverTime_Search_DutyDate"] != null ? Session["OverTime_Search_DutyDate"].ToString() : ""; //日期
            String SES_Search_ID = Session["OverTime_Search_ID"] != null ? Session["OverTime_Search_ID"].ToString() : "";  //身分證
            String SES_Search_Name = Session["OverTime_Search_Name"] != null ? Session["OverTime_Search_Name"].ToString() : "";  //姓名
            String SES_Search_EXAD = Session["OverTime_Search_EXAD"] != null ? Session["OverTime_Search_EXAD"].ToString() : ""; //現服機關
            String SES_Search_EXUNIT = Session["OverTime_Search_EXUNIT"] != null ? Session["OverTime_Search_EXUNIT"].ToString() : ""; //現服單位
            //使用者是否有進行查詢動作
            Boolean isSearchCallBack = (Request["type"] != null) ? true : false;

            //SQL : 超勤資料主要撈取條件
            String SQL_DutyTable = @"SELECT C_DUTYTABLE_PERSONAL.MZ_ID, MZ_NAME, DUTYDATE, 
                                                    DUTYITEM1, DUTYITEM2, DUTYITEM3, DUTYITEM4, DUTYITEM5, DUTYITEM6, DUTYITEM7, DUTYITEM8, DUTYITEM9,
                                                    DUTYITEM10, DUTYITEM11, DUTYITEM12, DUTYITEM13, DUTYITEM14, DUTYITEM15, DUTYITEM16, DUTYITEM17,
                                                    DUTYITEM18, DUTYITEM19, DUTYITEM20, DUTYITEM21, DUTYITEM22, DUTYITEM23, DUTYITEM24, DUTYITEM25,
                                                    DUTYITEM26,  DUTYITEM27,
                                                TOTAL_HOURS, ISDIRECTTIME
                                         FROM C_DUTYTABLE_PERSONAL
                                         LEFT JOIN A_DLBASE ON C_DUTYTABLE_PERSONAL.MZ_ID = A_DLBASE.MZ_ID ";

            //若有進行查詢動作，則加上查詢後之撈取條件
            List<String> WHEREs = new List<String>();
            List<String> ORDERs = new List<String>();
            if (isSearchCallBack)
            {
                //2.列出超勤輸入清單，改為查詢條件帶回的條件來進行查詢
                //從查詢結果帶回查詢條件，並組串SQL

                //判斷下拉選單是否有選擇，若有則優先帶入下拉選單之值
                if (String.IsNullOrEmpty(DropDownList_MZ_NAME.SelectedValue))
                {
                    if (!String.IsNullOrEmpty(SES_Search_ID))
                        WHEREs.Add(String.Format(@"C_DUTYTABLE_PERSONAL.MZ_ID='{0}'", SES_Search_ID));
                }
                else
                    WHEREs.Add(String.Format(@"C_DUTYTABLE_PERSONAL.MZ_ID='{0}'", DropDownList_MZ_NAME.SelectedValue));

                //if (!String.IsNullOrEmpty(SES_Search_Name))
                //    WHEREs.Add(String.Format(@"C_DUTYTABLE_PERSONAL.MZ_ID in (SELECT MZ_ID FROM A_DLBASE WHERE MZ_NAME like '{0}%')", SES_Search_Name));

                if (!String.IsNullOrEmpty(SES_Search_EXAD))
                    // Joy 修改查詢條件為現服機關
                    //WHEREs.Add(String.Format(@"C_DUTYTABLE_PERSONAL.MZ_AD='{0}'", SES_Search_EXAD));
                    WHEREs.Add(String.Format(@"C_DUTYTABLE_PERSONAL.MZ_EXAD='{0}'", SES_Search_EXAD));

                if (!String.IsNullOrEmpty(SES_Search_EXUNIT))
                    // Joy 修改查詢條件為現服單位
                    //WHEREs.Add(String.Format(@"C_DUTYTABLE_PERSONAL.MZ_UNIT='{0}'", SES_Search_EXUNIT));
                    WHEREs.Add(String.Format(@"C_DUTYTABLE_PERSONAL.MZ_EXUNIT='{0}'", SES_Search_EXUNIT));

                if (!String.IsNullOrEmpty(SES_Search_DutyDate))
                    WHEREs.Add(String.Format(@"DUTYDATE like '{0}%' ", SES_Search_DutyDate));

                ORDERs.Add(@"C_DUTYTABLE_PERSONAL.MZ_ID, C_DUTYTABLE_PERSONAL.DUTYDATE ");
            }
            else
            {   //若無進行查詢動作，則直接帶入目前登入者之資料
                if (String.IsNullOrEmpty(DropDownList_MZ_NAME.SelectedValue))
                    DropDownList_MZ_NAME.SelectedValue = SES_ADPMZ_ID;

                String DT_now = LogicCommon.getChineseDateTime();

                String MZ_ID = "";
                String MZ_EXAD = "";
                String MZ_EXUNIT = "";

                //依照目前選擇的人員，進行
                if (String.IsNullOrEmpty(DropDownList_MZ_NAME.SelectedValue))
                {
                    MZ_ID = Session["ADPMZ_ID"].ToString();
                    MZ_EXAD = Session["ADPMZ_EXAD"].ToString();
                    MZ_EXUNIT = Session["ADPMZ_EXUNIT"].ToString();
                }
                else
                {
                    AccountModel m = AccountService.lookupAccount(DropDownList_MZ_NAME.SelectedValue);
                    MZ_ID = m.ID;
                    MZ_EXAD = m.ExAD;
                    MZ_EXUNIT = m.ExUnit;
                }

                WHEREs.Add(String.Format(@"DUTYDATE like '{0}%' ", DT_now.Substring(0, 5)));
                WHEREs.Add(String.Format(@"C_DUTYTABLE_PERSONAL.MZ_ID='{0}'", MZ_ID));
                // Joy 修改查詢條件為現服機關
                //WHEREs.Add(String.Format(@"C_DUTYTABLE_PERSONAL.MZ_AD='{0}'", MZ_EXAD));
                WHEREs.Add(String.Format(@"C_DUTYTABLE_PERSONAL.MZ_EXAD='{0}'", MZ_EXAD));

                if (MZPower != "D")
                    // Joy 修改查詢條件為現服單位
                    //WHEREs.Add(String.Format(@"C_DUTYTABLE_PERSONAL.MZ_UNIT='{0}'", MZ_EXUNIT));
                    WHEREs.Add(String.Format(@"C_DUTYTABLE_PERSONAL.MZ_EXUNIT='{0}'", MZ_EXUNIT));
            }

            //實際資料綁定
            SQL_DutyTable += (WHEREs.Count > 0) ? " WHERE " : " WHERE 1 = 1 ";
            SQL_DutyTable += String.Join(" AND ", WHEREs.ToArray());
            SQL_DutyTable += (ORDERs.Count > 0) ? " ORDER BY " : "";
            SQL_DutyTable += String.Join(" , ", ORDERs.ToArray());

            SqlDataSource1.SelectCommand = SQL_DutyTable;
            SqlDataSource1.SelectParameters.Clear();
            GridView1.DataBind();

            //若有資料則選擇第一行
            if (GridView1.Rows.Count > 0)
            {
                GridView1.SelectedIndex = 0;
                //finddate(0); //TODO: 預先選擇第一行? 不太懂原因為何
            }
        }

        /// <summary>目前已沒有任何地方參考這一支函數，確認無誤後可以整個刪除</summary>
        private void finddate(int dateCount)
        {
            try
            {
                string strSQL = string.Empty;
                DataTable temp = new DataTable();
                strSQL = string.Format("SELECT MZ_ID FROM A_DLBASE WHERE MZ_ID='{0}' AND MZ_EXAD='{1}' AND MZ_EXUNIT='{2}'", GridView1.Rows[dateCount].Cells[1].Text, Session["ADPMZ_EXAD"].ToString(), Session["ADPMZ_EXUNIT"].ToString());
                if ((o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET").Rows.Count > 0))
                {
                    if (Request["UNIT"] != null || Request["UNIT"] != "")
                    {
                        strSQL = "SELECT MZ_ID,MZ_NAME FROM A_DLBASE WHERE MZ_EXAD='" + Request["AD"] + "' AND MZ_EXUNIT='" + Request["UNIT"] + "'";
                        temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
                        if (temp.Rows.Count > 0)
                        {
                            DropDownList_MZ_NAME.DataSourceID = null;
                            DropDownList_MZ_NAME.DataSource = temp;
                            DropDownList_MZ_NAME.DataTextField = "MZ_NAME";
                            DropDownList_MZ_NAME.DataValueField = "MZ_ID";
                            DropDownList_MZ_NAME.DataBind();
                            DropDownList_MZ_NAME.SelectedValue = GridView1.Rows[dateCount].Cells[1].Text;
                        }
                    }
                    else
                    {
                        strSQL = "SELECT MZ_ID,MZ_NAME FROM A_DLBASE WHERE MZ_EXAD='" + Request["AD"] + "'";
                        temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
                        if (temp.Rows.Count > 0)
                        {
                            DropDownList_MZ_NAME.DataSourceID = null;
                            DropDownList_MZ_NAME.DataSource = temp;
                            DropDownList_MZ_NAME.DataTextField = "MZ_NAME";
                            DropDownList_MZ_NAME.DataValueField = "MZ_ID";
                            DropDownList_MZ_NAME.DataBind();
                            DropDownList_MZ_NAME.SelectedValue = GridView1.Rows[dateCount].Cells[1].Text;
                        }
                    }
                }
                else
                {
                    tbMZ_ID.Visible = true;
                    DropDownList_MZ_NAME.Visible = false;
                    tbMZ_ID.Text = GridView1.Rows[dateCount].Cells[1].Text;
                    //tbChange(); 20150730 偵錯用 故移除
                }
            }
            catch
            {

            }
            TextBox_DUTYDATE.Text = GridView1.SelectedRow.Cells[3].Text;
            DropDownList_DUTYITEM1.SelectedValue = GridView1.SelectedRow.Cells[4].Text == "&nbsp;" ? "" : GridView1.SelectedRow.Cells[4].Text;
            DropDownList_DUTYITEM2.SelectedValue = GridView1.SelectedRow.Cells[5].Text == "&nbsp;" ? "" : GridView1.SelectedRow.Cells[5].Text;
            DropDownList_DUTYITEM3.SelectedValue = GridView1.SelectedRow.Cells[6].Text == "&nbsp;" ? "" : GridView1.SelectedRow.Cells[6].Text;
            DropDownList_DUTYITEM4.SelectedValue = GridView1.SelectedRow.Cells[7].Text == "&nbsp;" ? "" : GridView1.SelectedRow.Cells[7].Text;
            DropDownList_DUTYITEM5.SelectedValue = GridView1.SelectedRow.Cells[8].Text == "&nbsp;" ? "" : GridView1.SelectedRow.Cells[8].Text;
            DropDownList_DUTYITEM6.SelectedValue = GridView1.SelectedRow.Cells[9].Text == "&nbsp;" ? "" : GridView1.SelectedRow.Cells[9].Text;
            DropDownList_DUTYITEM7.SelectedValue = GridView1.SelectedRow.Cells[10].Text == "&nbsp;" ? "" : GridView1.SelectedRow.Cells[10].Text;
            DropDownList_DUTYITEM8.SelectedValue = GridView1.SelectedRow.Cells[11].Text == "&nbsp;" ? "" : GridView1.SelectedRow.Cells[11].Text;
            DropDownList_DUTYITEM9.SelectedValue = GridView1.SelectedRow.Cells[12].Text == "&nbsp;" ? "" : GridView1.SelectedRow.Cells[12].Text;
            DropDownList_DUTYITEM10.SelectedValue = GridView1.SelectedRow.Cells[13].Text == "&nbsp;" ? "" : GridView1.SelectedRow.Cells[13].Text;
            DropDownList_DUTYITEM11.SelectedValue = GridView1.SelectedRow.Cells[14].Text == "&nbsp;" ? "" : GridView1.SelectedRow.Cells[14].Text;
            DropDownList_DUTYITEM12.SelectedValue = GridView1.SelectedRow.Cells[15].Text == "&nbsp;" ? "" : GridView1.SelectedRow.Cells[15].Text;
            DropDownList_DUTYITEM13.SelectedValue = GridView1.SelectedRow.Cells[16].Text == "&nbsp;" ? "" : GridView1.SelectedRow.Cells[16].Text;
            DropDownList_DUTYITEM14.SelectedValue = GridView1.SelectedRow.Cells[17].Text == "&nbsp;" ? "" : GridView1.SelectedRow.Cells[17].Text;
            DropDownList_DUTYITEM15.SelectedValue = GridView1.SelectedRow.Cells[18].Text == "&nbsp;" ? "" : GridView1.SelectedRow.Cells[18].Text;
            DropDownList_DUTYITEM16.SelectedValue = GridView1.SelectedRow.Cells[19].Text == "&nbsp;" ? "" : GridView1.SelectedRow.Cells[19].Text;
            DropDownList_DUTYITEM17.SelectedValue = GridView1.SelectedRow.Cells[20].Text == "&nbsp;" ? "" : GridView1.SelectedRow.Cells[20].Text;
            DropDownList_DUTYITEM18.SelectedValue = GridView1.SelectedRow.Cells[21].Text == "&nbsp;" ? "" : GridView1.SelectedRow.Cells[21].Text;
            DropDownList_DUTYITEM19.SelectedValue = GridView1.SelectedRow.Cells[22].Text == "&nbsp;" ? "" : GridView1.SelectedRow.Cells[22].Text;
            DropDownList_DUTYITEM20.SelectedValue = GridView1.SelectedRow.Cells[23].Text == "&nbsp;" ? "" : GridView1.SelectedRow.Cells[23].Text;
            DropDownList_DUTYITEM21.SelectedValue = GridView1.SelectedRow.Cells[24].Text == "&nbsp;" ? "" : GridView1.SelectedRow.Cells[24].Text;
            DropDownList_DUTYITEM22.SelectedValue = GridView1.SelectedRow.Cells[25].Text == "&nbsp;" ? "" : GridView1.SelectedRow.Cells[25].Text;
            DropDownList_DUTYITEM23.SelectedValue = GridView1.SelectedRow.Cells[26].Text == "&nbsp;" ? "" : GridView1.SelectedRow.Cells[26].Text;
            DropDownList_DUTYITEM24.SelectedValue = GridView1.SelectedRow.Cells[27].Text == "&nbsp;" ? "" : GridView1.SelectedRow.Cells[27].Text;
            DropDownList_DUTYITEM25.SelectedValue = GridView1.SelectedRow.Cells[28].Text == "&nbsp;" ? "" : GridView1.SelectedRow.Cells[28].Text;
            DropDownList_DUTYITEM26.SelectedValue = GridView1.SelectedRow.Cells[29].Text == "&nbsp;" ? "" : GridView1.SelectedRow.Cells[29].Text;
            DropDownList_DUTYITEM27.SelectedValue = GridView1.SelectedRow.Cells[30].Text == "&nbsp;" ? "" : GridView1.SelectedRow.Cells[30].Text;
        }

        //TODO: 此函數須擇日廢棄
        private static string Count(string ID, string MODE, string DATE)
        {
            string result = "";

            string strSQL = "SELECT * FROM  C_DUTYTABLE_PERSONAL WHERE MZ_ID='" + ID + "' AND DUTYDATE='" + DATE + "'";


            DataTable tempDT = new DataTable();

            tempDT = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GETVALUE");

            int j = 0;
            int K = 0;
            string timestring = "";
            // Joy 20150513 超勤時數增加08-09欄位，但時數統計表未合併計算，所以將i值範圍由原先27改為28
            for (int i = 1; i < 28; i++)
            {
                if (MODE == "G" || MODE == "I")//G督勤 I專案情務
                {
                    if (tempDT.Rows[0]["DUTYITEM" + i.ToString()].ToString() == MODE)
                    {
                        K++;

                        if (K == 1)
                            timestring += tempDT.Rows[0]["TIME" + i.ToString()].ToString();
                        else
                            timestring += "." + tempDT.Rows[0]["TIME" + i.ToString()].ToString();
                    }
                }
                else
                {
                    if (tempDT.Rows[0]["DUTYITEM" + i.ToString()].ToString() == MODE)
                        j++;

                }
            }

            if (MODE == "G" || MODE == "I")
            {
                result = timestring;
            }
            else
            {
                result = j.ToString();

            }

            return result;
        }

        //TODO: 此 Region 內跟 C_OVERTIMEPAY_DETAIL_RPT.aspx.cs 內的函式完全相同，須擇日抽出至Logic層
        #region 應重構
        /// <summary>取得某特定日期之已轉換補休時數</summary>
        /// 20150506 Neil
        private Int32 GetSingleDayOvertimeRest(String MZ_ID, String DATE)
        {
            Int32 result = 0;
            DataRow r = getOvertimeDatarow(MZ_ID, DATE);

            if (r != null)
            {
                if (r["CONVERT_REST_HOURS"] != DBNull.Value)
                    result = Convert.ToInt32(r["CONVERT_REST_HOURS"]);
                else
                    result = 0;
            }
            return result;
        }

        /// <summary>取得某特定日期之超勤紀錄全部</summary>
        /// <returns>出勤紀錄</returns>
        /// 20150506 Neil
        private List<String> GetSingleDayOverTimeList(String MZ_ID, String DATE)
        {
            List<String> result = new List<String>();
            DataRow r = getOvertimeDatarow(MZ_ID, DATE);

            if (r != null)
            {
                // Joy 20150513 超勤時數增加08-09欄位，但時數統計表未合併計算，所以將i值範圍由原先27改為28
                for (int i = 1; i < 28; i++)
                {
                    String s = r["DUTYITEM" + i.ToString()].ToString();
                    result.Add(s);
                }
            }
            return result;
        }

        /// <summary>取得某特定日期之是否停休</summary>
        /// <returns>是否停休</returns>
        /// 20150602 Joy
        private string GetDutyStopOff(String MZ_ID, String DATE)
        {
            List<String> result = new List<String>();
            DataRow r = getOvertimeDatarow(MZ_ID, DATE);
            string s = "";
            if (r != null)
            {
                // Joy 20150513 超勤時數增加08-09欄位，但時數統計表未合併計算，所以將i值範圍由原先27改為28
                s = r["DUTYSTOPOFF"].ToString();
            }
            return s;
        }

        /// <summary>取得某特定日期之督勤時間區段</summary>
        /// <returns>督勤時間區段</returns>
        /// 20150506 Neil
        /// 20150506 Neil : 經詢問林小隊長，確認督勤於業務面上應不會有"區段斷開"的問題，故應為連續區段
        private String getSingleDayOverTimeGType(String MZ_ID, String DATE)
        {
            String TYPE = "G"; //定義督勤之代碼
            DataRow r = getOvertimeDatarow(MZ_ID, DATE);

            List<String> lst = new List<String>();
            if (r != null)
            {
                // Joy 20150513 超勤時數增加08-09欄位，但時數統計表未合併計算，所以將i值範圍由原先27改為28
                for (int i = 1; i < 28; i++)
                {
                    String s = r["DUTYITEM" + i.ToString()].ToString();
                    if (s == TYPE) //僅轉換督勤
                        lst.Add(r["TIME" + i.ToString()].ToString());
                }
            }
            return String.Join(".", lst.ToArray());
        }

        /// <summary>基礎函數 : 取得某一天之超勤紀錄資料列</summary>
        /// <param name="MZ_ID">身分證字號</param>
        /// <param name="DATE">超勤日期</param>
        /// 20150506 Neil
        private DataRow getOvertimeDatarow(String MZ_ID, String DATE)
        {
            String strSQL = String.Format(@"SELECT * FROM C_DUTYTABLE_PERSONAL WHERE MZ_ID='{0}' AND DUTYDATE='{1}'",
                                          MZ_ID,
                                          DATE);

            DataTable dt = new DataTable();
            dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GETVALUE");

            if (dt.Rows.Count > 0)
            {
                DataRow r = dt.Rows[0]; //正常僅會有一筆資料
                return r;
            }
            else
                return null;
        }
        #endregion






    }
}
