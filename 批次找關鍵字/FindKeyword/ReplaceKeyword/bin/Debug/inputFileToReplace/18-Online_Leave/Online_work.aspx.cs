using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Data;
using TPPDDB.Helpers;
using TPPDDB._18_Online_Leave;
using TPPDDB.Models._18_Online_Work;
using TPPDDB.Models._3_ForLeave;
using TPPDDB._3_forleave;
using TPPDDB._18_Online_Leave.Service;

namespace TPPDDB.Online_Leave
{
    /// <summary>
    /// 差勤線上簽核功能
    /// 注意!有一部分程式碼整併到 Online_work.xxx.cs,
    /// 不然原本這包太大,五千多行很難維護
    /// </summary>
    public partial class Online_work : System.Web.UI.Page
    {
        #region 頁面初始值及權限

        protected void Page_Load(object sender, EventArgs e)
        {
            //取得模組
            _TPM_FION = Request.QueryString["TPM_FION"].ToStringNullSafe();

            if (!Page.IsPostBack)
            {
                _C_strGID = o_a_Function.strGID(SessionHelper.ADPMZ_ID);
                _query = new Online_work_Query()
                {
                    PerformReviewID = SessionHelper.ADPMZ_ID.ToStringNullSafe(),
                    SendID = SessionHelper.ADPMZ_ID.ToStringNullSafe(),
                    TPMFION = _TPM_FION
                };

                Label_ADData.Text = _strUserName;

                DataTable AD_TB = Service_A_KTYPE.GetDT_AD();

                //差假簽核用的機關
                DDL_AD.DataSource = AD_TB;
                DDL_AD.DataTextField = "MZ_KCHI";
                DDL_AD.DataValueField = "MZ_KCODE";
                DDL_AD.DataBind();
                ChangeUnit();

                //加班簽核用的機關
                DDL_EXAD.DataSource = AD_TB;
                DDL_EXAD.DataTextField = "MZ_KCHI";
                DDL_EXAD.DataValueField = "MZ_KCODE";
                DDL_EXAD.DataBind();
                ChangeExUnit();

                //依照權限建立頁面功能
                chk_TPMGroup();



                allclose();//所有控制項都關掉
                Page_update();
            }

        }


        /// <summary>
        /// 群組權限
        /// </summary>
        protected void chk_TPMGroup()
        {
            switch (_C_strGID)
            {
                case "A":
                case "B":
                    DDL_AD.SelectedValue = SessionHelper.ADPMZ_EXAD;
                    DDL_EXAD.SelectedValue = SessionHelper.ADPMZ_EXAD;
                    lbtn_flow_all.Visible = true;
                    break;
                case "C":
                    DDL_AD.SelectedValue = SessionHelper.ADPMZ_EXAD;
                    DDL_AD.Enabled = false;
                    ChangeUnit();
                    DDL_EXAD.SelectedValue = SessionHelper.ADPMZ_EXAD;
                    DDL_EXAD.Enabled = false;
                    ChangeExUnit();
                    lbtn_flow_all.Visible = true;
                    break;
                case "D":
                    lbtn_flow_all.Visible = false;
                    break;
                case "E":
                    DDL_AD.SelectedValue = SessionHelper.ADPMZ_EXAD;
                    DDL_AD.Enabled = false;
                    ChangeUnit();
                    DDL_UNIT.SelectedValue = SessionHelper.ADPMZ_EXUNIT;
                    DDL_UNIT.Enabled = false;
                    ChangeExUnit();
                    lbtn_flow_all.Visible = true;
                    break;
            }
        }
        /// <summary>
        /// 下拉選單:差假搜尋 單位
        /// </summary>
        protected void ChangeUnit()
        {
            string strSQL = string.Format(@"SELECT RTRIM(MZ_KCHI),RTRIM(MZ_KCODE) FROM A_KTYPE 
                                            WHERE MZ_KTYPE='25' AND MZ_KCODE IN (SELECT MZ_UNIT FROM A_UNIT_AD WHERE MZ_AD='{0}')",
                                            DDL_AD.SelectedValue);

            WebUIHelpers.DropDownList_DataBind(strSQL, DDL_UNIT, "RTRIM(MZ_KCHI)", "RTRIM(MZ_KCODE)", new WebUIHelpers.OtherListItem(0, new ListItem("請選擇", "請選擇")));
        }
        /// <summary>
        /// 下拉選單:加班搜尋 單位
        /// </summary>
        protected void ChangeExUnit()
        {
            string strSQL = string.Format(@"SELECT RTRIM(MZ_KCHI),RTRIM(MZ_KCODE) FROM A_KTYPE 
                                            WHERE MZ_KTYPE='25' AND MZ_KCODE IN (SELECT MZ_UNIT FROM A_UNIT_AD WHERE MZ_AD='{0}')",
                                            DDL_EXAD.SelectedValue);

            WebUIHelpers.DropDownList_DataBind(strSQL, DDL_EXUNIT, "RTRIM(MZ_KCHI)", "RTRIM(MZ_KCODE)", new WebUIHelpers.OtherListItem(0, new ListItem("請選擇", "")));
        }
        /// <summary>
        /// 所有控制項都關掉
        /// </summary>
        protected void allclose()
        {
            #region 右區塊按鈕區域
            //功能名稱標籤
            lbl_sign.Visible = false;
            //主管簽核按鈕
            pl_data.Visible = false;
            //代理人簽核按鈕
            pl_agents.Visible = false;
            //差假承辦人(人事)按鈕
            pl_person.Visible = false;
            //加班簽核按鈕
            pl_overtime.Visible = false;
            //加班承辦人按鈕
            pl_overtime_person.Visible = false;
            //差假簽核抽回按鈕
            Panel_re.Visible = false;
            //差旅費簽核按鈕
            Panel_bussiness_btn.Visible = false;
            //差旅費簽核抽回按鈕
            Panel_b_re.Visible = false;
            //銷假簽核按鈕
            Panel_change_dltb01_btn.Visible = false;
            //銷假簽核抽回按鈕
            Panel_change_dltb01_re.Visible = false;
            //個人簽核狀態之各紀錄連接按鈕
            pl_self_item.Visible = false;
            //個人簽核狀態之查詢日期區間
            drop.Visible = false;
            #endregion

            #region 右區塊查詢區域
            //差假搜尋條件
            Panel_search.Visible = false;
            //差假簽核紀錄
            pl_data2.Visible = false;
            //差假簽核紀錄的歷程
            pl_history.Visible = false;
            //差假詳細資料Dialog
            P_E_ModalPopupExtender.Hide();
            //差假意見填寫Dialog
            P_E_BACK.Hide();
            //差假陳核Dialog
            Panel_select_ModalPopupExtender.Hide();

            //加班搜尋條件
            pl_search.Visible = false;
            //加班簽核紀錄
            pl_overtime_gv.Visible = false;
            //加班簽核紀錄的歷程
            pl_overtime_history.Visible = false;
            //加班意見填寫Dialog
            M_P_E_Opinion.Hide();
            //加班陳核Dialog
            M_P_E_overtime_selcet.Hide();
            //加班搜尋同名人員選擇的Dialog
            M_P_E_Search.Hide();

            //差旅費簽核紀錄
            Panel_bussinesstrip_GV.Visible = false;
            //差旅費簽核紀錄的歷程
            pl_b_history.Visible = false;
            //差旅費意見填寫Dialog
            panel_b_message_pop.Hide();
            //差旅費陳核Dialog
            Panel_b_select_ModalPopupExtender.Hide();

            //銷假簽核紀錄
            pl_change_dltb01.Visible = false;
            //銷假簽核紀錄的歷程
            pl_change_dltb01_history.Visible = false;
            //銷假陳核Dialog
            btn_popup6_ModalPopupExtender.Hide();
            //銷假意見填寫Dialog
            btn_fake4_ModalPopupExtender.Hide();
            #endregion

            //差假簽核搜尋，類別條件
            ddl_Kind.Visible = false;
            //差假簽核歷程，退回上個流程按鈕
            process_bar.Visible = false;
        }
        #endregion

        #region 左邊選單link_Button click事件
        /// <summary>
        /// 頁面更新
        /// 0827更新差假相關
        /// </summary>
        protected void Page_update()
        {
            //0815→Dean差勤線上簽和數量
            CSonline = new CSonline("C_LEAVE_HISTORY", SessionHelper.ADPMZ_ID);
            int inttemp = CSonline.getDateCount();
            lbtn_sign.Text = "差假線上簽核 (" + inttemp + ") ";
            lbtn_sign.Enabled = false;
            if (inttemp > 0)
            {
                lbtn_sign.Enabled = true;
            }


            //代理人線上簽和數量           
            CSonline = new CSonline("C_HISTORY_AGENTS", SessionHelper.ADPMZ_ID);
            int intAgents = CSonline.getDateCount();
            lbtn_agents.Text = "代理人線上簽核(" + intAgents + ")";
            lbtn_agents.Enabled = false;
            if (intAgents > 0)
            {
                lbtn_agents.Enabled = true;
            }


            //可抽回數量          
            CSonline = new CSonline("reSearch", SessionHelper.ADPMZ_ID);
            int rec = CSonline.getDateCount();
            lbtn_re.Text = "抽回差假簽核(" + rec + ")";
            lbtn_re.Enabled = false;
            if (rec > 0)
            {
                lbtn_re.Enabled = true;
            }


            //差旅費數量           
            CSonline = new CSonline("bussiness_search", SessionHelper.ADPMZ_ID);
            int b_count = CSonline.getDateCount();
            lbtn_bussinesstrip.Text = "差旅費線上簽核(" + b_count + ")";
            lbtn_bussinesstrip.Enabled = false;
            if (b_count > 0)
            {
                lbtn_bussinesstrip.Enabled = true;
            }


            //差旅費可抽回數量           
            CSonline = new CSonline("bresearch", SessionHelper.ADPMZ_ID);
            int bre_count = CSonline.getDateCount();
            lbtn_bussiness_back.Text = "抽回差旅費簽核(" + bre_count + ")";
            lbtn_bussiness_back.Enabled = false;
            if (bre_count > 0)
            {
                lbtn_bussiness_back.Enabled = true;
            }


            //銷假數量            
            CSonline = new CSonline("change_dltb01_search", SessionHelper.ADPMZ_ID);
            int c_count = CSonline.getDateCount();
            lbtn_change_dltb01.Text = "銷假線上簽核(" + c_count + ")";
            lbtn_change_dltb01.Enabled = false;
            if (c_count > 0)
            {
                lbtn_change_dltb01.Enabled = true;
            }


            //抽回銷假數量            
            CSonline = new CSonline("RECHANGE_DLTB01_search", SessionHelper.ADPMZ_ID);
            lbtn_change_dltb01_back.Text = "抽回銷假簽核(" + CSonline.checkDecision() + ")";
            lbtn_change_dltb01_back.Enabled = false;
            if (CSonline.getDateCount() > 0)
            {
                lbtn_change_dltb01_back.Enabled = true;
            }

            //是否有加班費需審核 (單位主管1、勤務中心2，4、5是人室事承辦人)
            //加班線上簽核數量
            _query.OVERTIME_TYPE = "OLWA";
            int ovrCount = OWService.GetOverTimeReViewCount(_query);
            btn_left_overtime.Text = "加班線上簽核(" + ovrCount + ")";
            btn_left_overtime.Enabled = ovrCount > 0 ? true : false;


            lbtn_flow.Text = "個人簽核狀態";

            DataTable dt = Service_C_REVIEW_MANAGEMENT.GetDT_REVIEW_LEVEL();

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["REVIEW_LEVEL"].ToString() == "4")
                    {

                        string cnt = Service_C_LEAVE_HISTORY.GetCount_待人事室審核中();
                        lbtn_person.Text = $"已決行案件({cnt})";
                        int y = Service_C_LEAVE_HISTORY.GetCount_人事室審核();

                        if (y > 0)
                        {
                            lbtn_person.Enabled = true;
                        }
                        else
                        {
                            lbtn_person.Enabled = false;
                        }

                        lbtn_person.Visible = true;
                        btn_allOK.Visible = true;
                        btn_all_b_OK.Visible = true;
                        btn_all_c_OK.Visible = true;
                        break;
                    }
                    else
                    {
                        lbtn_person.Visible = false;
                        btn_allOK.Visible = false;
                        btn_all_b_OK.Visible = false;
                        btn_all_c_OK.Visible = false;

                    }
                }
            }
            else
            {
                lbtn_person.Visible = false;
                btn_allOK.Visible = false;
                btn_all_b_OK.Visible = false;
                btn_all_c_OK.Visible = false;
            }

            //判斷是否為加班承辦人(人事室)
            //已決行加班案件
            btn_left_overtime_decision.Visible = false;
            _query.MZ_AD = SessionHelper.ADPMZ_EXAD.ToStringNullSafe();
            _query.REVIEW_LEVEL = "'4','5'";
            _query.OVERTIME_TYPE = "OLWU";
            if (OWService.CheckReviewManagement(_query))
            {
                btn_left_overtime_decision.Visible = true;
                int ovruCount = OWService.GetOverTimeReViewCount(_query);
                btn_left_overtime_decision.Text = "已決行加班案件(" + ovruCount + ")";
                btn_left_overtime_decision.Enabled = ovruCount > 0 ? true : false;
            }
        }



        protected void back_Click(object sender, EventArgs e)
        {
            //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click",
            //        "window.opener.location.href='C_ForLeaveBasic.aspx?SEARCHMODE=DLBASE&MZ_EXAD=" + EXAD + "&MZ_EXUNIT=" + EXUNIT + "&MZ_IDATE1=" + IDATE1 + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "';window.close();", true);
            Response.Redirect("~/10-knowledge/Default.aspx");
        }

        //左邊 差假線上簽核 ;btn = gv_data
        protected void lbtn_sign_Click(object sender, EventArgs e)
        {
            if (lbtn_sign.Enabled == true)
            {
                this.MODE = Const_Mode.差假線上簽核;
                DataTable tempMuch = new DataTable();
                CSonline = new CSonline("doSearch", SessionHelper.ADPMZ_ID);
                tempMuch = CSonline.getSearch();
                gv_data.DataSource = tempMuch;
                gv_data.DataBind();
                lbl_sign.Text = "假單審核";

                //顯示判定

                allclose();
                lbl_sign.Visible = true;
                gv_data.Visible = true;
                gv_data.Columns[7].Visible = true;
                pl_data2.Visible = true;
                pl_data.Visible = true;
                Page_update();
            }
        }

        //左邊 代理人簽核 lbtn = gv_agents
        protected void lbtn_agents_Click(object sender, EventArgs e)
        {
            //0810→Dean
            //20140620

            if (lbtn_agents.Enabled == true)
            {
                this.MODE = Const_Mode.代理人簽核;
                DataTable agentsMuch = new DataTable();
                CSonline = new CSonline("agentsSearch", SessionHelper.ADPMZ_ID);
                agentsMuch = CSonline.getSearch();
                gv_data.DataSource = agentsMuch;
                gv_data.DataBind();
                lbl_sign.Text = "代理人假單審核";

                //顯示判定
                allclose();
                lbl_sign.Visible = true;
                gv_data.Visible = true;
                gv_data.Columns[7].Visible = true;
                pl_agents.Visible = true;
                pl_data2.Visible = true;
                Page_update();
            }
        }

        //左邊選單 差旅費線上簽核
        protected void lbtn_bussinesstrip_Click(object sender, EventArgs e)
        {
            this.MODE = Const_Mode.差旅費線上簽核;
            DataTable tempMuch = new DataTable();
            CSonline = new CSonline("bussiness_search", SessionHelper.ADPMZ_ID);
            tempMuch = CSonline.getDate();
            GV_bussinesstrip.DataSource = tempMuch;
            GV_bussinesstrip.DataBind();
            GV_bussinesstrip.Columns[6].Visible = true;
            //顯示判定
            lbl_sign.Text = "差旅費審核";

            allclose();
            lbl_sign.Visible = true;
            Page_update();
            Panel_bussiness_btn.Visible = true;
            Panel_bussinesstrip_GV.Visible = true;
        }

        //左邊選單  銷假線上簽核
        protected void lbtn_change_dltb01_Click(object sender, EventArgs e)
        {
            try
            {

                this.MODE = Const_Mode.差旅費線上簽核;

                DataTable tempMuch = new DataTable();

                CSonline = new CSonline("change_dltb01_search", SessionHelper.ADPMZ_ID);

                tempMuch = CSonline.getDate();

                gv_change_dltb01.DataSource = tempMuch;
                gv_change_dltb01.DataBind();
                gv_change_dltb01.Columns[6].Visible = true;
                //顯示判定
                lbl_sign.Text = "銷假審核";

                allclose();
                Page_update();

                lbl_sign.Visible = true;
                Panel_change_dltb01_btn.Visible = true;
                pl_change_dltb01.Visible = true;
                gv_change_dltb01.Visible = true;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }



        //左邊 抽回差假簽核 0812→Dean
        protected void lbtn_re_Click(object sender, EventArgs e)
        {
            this.MODE = Const_Mode.抽回差假簽核;
            DataTable temp = new DataTable();

            CSonline = new CSonline("reSearch", SessionHelper.ADPMZ_ID);
            temp = CSonline.getDate();
            gv_data.DataSource = temp;
            gv_data.DataBind();
            gv_data.Columns[7].Visible = false;
            lbl_sign.Text = "假單簽核歷程抽回";
            allclose();
            Panel_re.Visible = true;
            pl_data2.Visible = true;
            lbl_sign.Visible = true;
            gv_data.Visible = true;
            //此頁面用途為給承辦人看已經核可之假單，如果需要按鈕則取消註解
            //pl_data.Visible = true;
            Page_update();
        }

        //左邊選單 抽回差旅費簽核
        protected void lbtn_bussiness_back_Click(object sender, EventArgs e)
        {
            this.MODE = Const_Mode.抽回差旅費簽核;
            DataTable tempMuch = new DataTable();
            CSonline = new CSonline("bresearch", SessionHelper.ADPMZ_ID);
            tempMuch = CSonline.getDate();
            GV_bussinesstrip.DataSource = tempMuch;
            GV_bussinesstrip.DataBind();
            GV_bussinesstrip.Columns[6].Visible = false;
            //顯示判定
            lbl_sign.Text = "差旅費抽回";
            allclose();
            lbl_sign.Visible = true;
            Page_update();
            Panel_b_re.Visible = true;
            Panel_bussinesstrip_GV.Visible = true;
        }

        //左邊選單 抽回銷假簽核
        protected void lbtn_change_dltb01_back_Click(object sender, EventArgs e)
        {
            this.MODE = Const_Mode.抽回銷假簽核;
            DataTable tempMuch = new DataTable();
            CSonline = new CSonline("RECHANGE_DLTB01_search", SessionHelper.ADPMZ_ID);
            tempMuch = CSonline.getDate();
            gv_change_dltb01.DataSource = tempMuch;
            gv_change_dltb01.DataBind();
            gv_change_dltb01.Columns[6].Visible = false;
            //顯示判定
            lbl_sign.Text = "抽回";
            allclose();
            lbl_sign.Visible = true;
            Page_update();
            Panel_change_dltb01_re.Visible = true;
            pl_change_dltb01.Visible = true;
            gv_change_dltb01.Visible = true;
        }



        //左邊選單 個人簽核狀態
        protected void lbtn_flow_Click(object sender, EventArgs e)
        {
            this.WHATS = "flow";
            flow_Click("", "", "C_LEAVE_HISTORY");
        }

        //左邊選單 查詢簽核狀態
        protected void lbtn_flow_all_Click(object sender, EventArgs e)
        {
            allclose();
            Panel_search.Visible = true;
            ddl_Kind.Visible = true;
            ddl_Kind.SelectedValue = "差假";
            pl_data2.Visible = true;
            gv_data.Visible = true;
            lbl_sign.Text = "假單簽核歷程查詢";
            this.MODE = Const_Mode.查詢簽核狀態;
        }



        //左邊 已決行案件(人事室呈辦人)
        protected void lbtn_person_Click(object sender, EventArgs e)
        {
            //0812→Dean
            //20140620

            // if ((int)ViewState["tempPerson"] > 0)
            if (lbtn_person.Enabled == true)
            {
                this.MODE = Const_Mode.已決行案件;
                DataTable tempMuch = new DataTable();
                tempMuch = PersonSearch("C_LEAVE_HISTORY");
                gv_data.DataSource = tempMuch;
                this.gv_data_Person = tempMuch;
                gv_data.DataBind();
                gv_data.Visible = true;
                lbl_sign.Text = "已決行案件";
                allclose();
                lbl_sign.Visible = true;
                pl_data2.Visible = true;
                pl_person.Visible = true;
                Panel_search.Visible = true;

                Page_update();
            }

        }

        //左邊 已核定假單
        protected void btn_allOK_Click(object sender, EventArgs e)
        {
            //0812→DEAN
            this.MODE = Const_Mode.已核定假單;
            DataTable temp = new DataTable();
            temp = PersonSearch("C_LEAVE_HISTORY");
            gv_data.DataSource = temp;
            gv_data.DataBind();
            gv_data.Columns[7].Visible = false;
            lbl_sign.Text = "已核定假單";
            allclose();
            lbl_sign.Visible = true;
            Panel_search.GroupingText = "已核定假單搜尋";
            Panel_search.Visible = true;

            pl_data2.Visible = true;
            gv_data.Visible = true;
            //此頁面用途為給承辦人看已經核可之假單，如果需要按鈕則取消註解
            //pl_data.Visible = true;
            Page_update();
        }

        //左邊 已核定差旅費
        protected void btn_all_b_OK_Click(object sender, EventArgs e)
        {
            this.MODE = Const_Mode.已核定假單;
            DataTable temp = new DataTable();
            temp = PersonSearch("C_BUSSINESSTRIP_HISTORY");
            GV_bussinesstrip.DataSource = temp;
            GV_bussinesstrip.DataBind();
            GV_bussinesstrip.Columns[6].Visible = false;
            lbl_sign.Text = "已核定差旅費";
            allclose();
            Panel_search.GroupingText = "已核定差旅費搜尋";
            Panel_search.Visible = true;


            Panel_bussinesstrip_GV.Visible = true;
            GV_bussinesstrip.Visible = true;
            lbl_sign.Visible = true;
            //此頁面用途為給承辦人看已經核可之假單，如果需要按鈕則取消註解
            //pl_data.Visible = true;
            Page_update();
        }

        //左邊 已核定銷假單
        protected void btn_all_c_OK_Click(object sender, EventArgs e)
        {
            this.MODE = Const_Mode.已核定假單;
            DataTable temp = new DataTable();
            temp = PersonSearch("C_CHANGE_DLTB01_HISTORY");
            gv_change_dltb01.DataSource = temp;
            gv_change_dltb01.DataBind();
            gv_change_dltb01.Columns[6].Visible = false;
            lbl_sign.Text = "已核定銷假";
            allclose();
            Panel_search.GroupingText = "已核定銷假搜尋";
            Panel_search.Visible = true;

            pl_change_dltb01.Visible = true;
            gv_change_dltb01.Visible = true;
            lbl_sign.Visible = true;
            //此頁面用途為給承辦人看已經核可之假單，如果需要按鈕則取消註解
            //pl_data.Visible = true;
            Page_update();
        }

        #endregion


        #region 一般差假
        // sam 差假決行層級規定
        //代理人同意
        protected void btn_agree_Click(object sender, EventArgs e)
        {
            //0810→DEAN
            string agreeSQL = string.Empty;
            int count = 0;
            for (int a = 0; a < gv_data.Rows.Count; a++)
            {
                CheckBox agreeCK = (CheckBox)gv_data.Rows[a].FindControl("ckbox_select");
                if (agreeCK.Checked == true)
                {
                    SQL_UPDATE_XXX_HISTORY(a, btn_agree.Text.ToString(), gv_data, "C_LEAVE_HISTORY", true);
                    SQL_INSERT(a);
                    count++;
                }
            }
            if (this.MODE == Const_Mode.差假線上簽核)//差假核定 同意
            {
                DataTable tempMuch = new DataTable();
                CSonline = new CSonline("doSearch", SessionHelper.ADPMZ_ID);
                tempMuch = CSonline.getSearch();
                gv_data.DataSource = tempMuch;
                gv_data.DataBind();
                gv_data.Visible = true;
            }
            else if (this.MODE == Const_Mode.代理人簽核)//代理人同意btn
            {
                DataTable agentsMuch = new DataTable();
                CSonline = new CSonline("agentsSearch", SessionHelper.ADPMZ_ID);
                agentsMuch = CSonline.getSearch();
                gv_data.DataSource = agentsMuch;
                gv_data.DataBind();
                gv_data.Visible = true;
            }

            this.RSN_LENTH = count;
            Page_update();
        }//差勤同意

        //差勤不同意
        protected void btn_notagree_Click(object sender, EventArgs e)
        {
            //0810→DEAN
            string agreeSQL = string.Empty;
            int count = 0;
            for (int a = 0; a < gv_data.Rows.Count; a++)
            {
                CheckBox agreeCK = (CheckBox)gv_data.Rows[a].FindControl("ckbox_select");
                if (agreeCK.Checked == true)
                {
                    SQL_UPDATE_XXX_HISTORY(a, btn_notagree.Text.ToString(), gv_data, "C_LEAVE_HISTORY", false);
                    //SQL_INSERT(a);

                    count++;
                }
            }
            if (this.MODE == Const_Mode.差假線上簽核)
            {
                DataTable tempMuch = new DataTable();
                CSonline = new CSonline("doSearch", SessionHelper.ADPMZ_ID);
                tempMuch = CSonline.getSearch();
                gv_data.DataSource = tempMuch;
                gv_data.DataBind();
                gv_data.Visible = true;
            }
            else if (this.MODE == Const_Mode.代理人簽核)
            {
                DataTable agentsMuch = new DataTable();
                CSonline = new CSonline("agentsSearch", SessionHelper.ADPMZ_ID);
                agentsMuch = CSonline.getSearch();
                gv_data.DataSource = agentsMuch;
                gv_data.DataBind();
                gv_data.Visible = true;
            }

            Page_update();

            if (this.type != "")
            {
                this.type = "";
                Response.Redirect("~/18-Online_Leave/Online_work.aspx?TPM_FION=" + _TPM_FION);
                this.ViewStateClear();
            }

            this.RSN_LENTH = count;
        }

        //差勤決行
        protected void btn_check_Click(object sender, EventArgs e)
        {
            //0810→Dean
            int count = gv_data.Rows.Count;
            for (int n = 0; n < gv_data.Rows.Count; n++)
            {
                CheckBox myCheckbox = (CheckBox)gv_data.Rows[n].FindControl("ckbox_select");
                if (myCheckbox.Checked == true)
                {
                    SQL_UPDATE_XXX_HISTORY(n, btn_check.Text.ToString(), gv_data, "C_LEAVE_HISTORY", true);
                    SQL_INSERTOK(n);
                }
            }

            DataTable tempMuch = new DataTable();
            CSonline = new CSonline("doSearch", SessionHelper.ADPMZ_ID);
            tempMuch = CSonline.getSearch();
            if (tempMuch.Rows.Count == 0)
            {
                this.type = "Y";
            }

            gv_data.DataSource = tempMuch;
            gv_data.DataBind();
            gv_data.Visible = true;
            Page_update();

            if (this.type != "")
            {
                this.type = "";
                Response.Redirect("~/18-Online_Leave/Online_work.aspx?TPM_FION=" + _TPM_FION);
                this.ViewStateClear();
            }
        }

        //同層呈核
        protected void btn_sign_Click(object sender, EventArgs e)
        {
            _btn_sign_Click(false);
        }

        //上層呈核
        protected void btn_upper_Click(object sender, EventArgs e)
        {
            _btn_sign_Click(true);
        }
        /// <summary>
        /// 陳核 與 第一層陳核
        /// </summary>
        /// <param name="_isUpper">t:第一層陳核 f:陳核</param>
        private void _btn_sign_Click(bool _isUpper)
        {
            //0810→Dean
            string agreeSQL = string.Empty;
            //從 上方的等待簽核資料中 勾了多少筆?
            int count_CheckedInGV = 0;

            //UI上的差假記錄
            for (int a = 0; a < gv_data.Rows.Count; a++)
            {
                //勾選方塊
                CheckBox agreeCK = (CheckBox)gv_data.Rows[a].FindControl("ckbox_select");
                //沒打勾? pass
                if (agreeCK.Checked != true)
                {
                    continue;
                }
                //下面就是有打勾的

                //紀錄目前打勾的簽核資料的SN,對應到 C_LEAVE_HISTORY.SN
                this.Set_RSN(count_CheckedInGV, gv_data.DataKeys[a]["SN"].ToStringNullSafe());
                //這段幹什麼的待查

                //根據 當前勾選的 歷程記錄SN 和 歷程記錄資料表名稱 ,找到假卡資料 ,綁定來源機關單位
                string History_SN = gv_data.DataKeys[a]["SN"].ToString();
                Reviewer_V2(History_SN, "C_LEAVE_HISTORY");

                //設值 UI上方的 待簽核表單 個表單的勾選狀態 有標y的代表打勾
                this.Set_upd(a, "y");

                //把目前按鈕上的文字,記錄起來

                if (_isUpper)
                    this.btn = btn_upper.Text.ToString();//第一層陳核
                else
                    this.btn = btn_sign.Text.ToString();//陳核

                //勾選多少,再追加
                count_CheckedInGV++;

            }//for (int a = 0; a < gv_data.Rows.Count; a++)


            //如果有勾到表單
            if (count_CheckedInGV > 0)
            {
                //設定目前為陳核(非第一層陳核),後面選人之後,需要知道現在是跑哪一種
                this.isUpper = _isUpper;
                bool isFindAnyOne = GV_CHECK_show( GV_CHECKER, _isUpper);
                //顧名思義判斷有無承辦人或主管
                if (isFindAnyOne == false)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "alert('找不到該單位承辦人或單位主管')", true);

                    return;
                }
                //打開陳核人員的選單
                Panel_select_ModalPopupExtender.Show();
            }
            //把簽名歷程區塊關掉,不確定有沒有用?
            pl_history.Visible = false;
            Page_update();

            this.RSN_LENTH = count_CheckedInGV;
        }

        /// <summary>
        /// 推動進度,根據 tablename UI上的陳核人員勾選,更新審核歷程資料
        /// </summary>
        /// <param name="number">從GridView上抓取目前選到的資料的索引</param>
        /// <param name="status">目前的動作按鈕名稱,注意這名稱會影響動作代碼</param>
        /// <param name="gv">陳核人員的GridView物件</param>
        /// <param name="tablename">歷程資料的表名稱</param>
        /// <param name="check">進還是退? true:陳核(或決行) false:退回 </param>
        protected void SQL_UPDATE_XXX_HISTORY(int number, string status, GridView gv, string tablename, bool check)
        {
            //從GridView上抓取目前選到的資料
            DataKey dataKey = gv.DataKeys[number];


            string year = (System.Convert.ToInt32(DateTime.Now.Year.ToString()) - 1911).ToString();
            string today = year + DateTime.Now.ToString("MMdd");
            string now = DateTime.Now.ToString("HH:mm:ss");

            string agreeSQL = "UPDATE " + tablename + " SET "
                    + "PROCESS_DATE = @PROCESS_DATE,PROCESS_STATUS = @PROCESS_STATUS,REVIEW_MESSAGE=@REVIEW_MESSAGE,"
                    + "PROCESS_TIME = @PROCESS_TIME WHERE SN=" + dataKey["SN"];

            SqlParameter[] sp = {
                                    new SqlParameter("PROCESS_DATE",SqlDbType.VarChar){Value =  today  },
                                    new SqlParameter("PROCESS_STATUS",SqlDbType.VarChar){Value = Service_C_STATUS.Get_C_STATUS_SN(status)},
                                    new SqlParameter("PROCESS_TIME",SqlDbType.VarChar){Value =  now },
                                    new SqlParameter("REVIEW_MESSAGE",SqlDbType.VarChar){Value =  this.txt_message_Text==""?(check?"同意":"退回"):this.txt_message_Text  }
                                    };

            try
            {

                o_DBFactory.ABC_toTest.ExecuteNonQuery( agreeSQL, sp);
                //新增新的排程
                //irk 2012/11/6 LOG增加
                LogModel lm = new LogModel("C");
                lm.sqlType = "U";
                string sql = lm.RegexSQL(agreeSQL, sp);
                lm.SqlHistory("修改歷程", sql, Request.QueryString["TPM_FION"]);

                if (!check)//退回或不同意就會標記 RETURN_FLAG='Y' 線上作業就會找不到該假單,除非 "退回上個流程"
                {
                    string editSQL = "";
                    try
                    {
                        editSQL = string.Format("UPDATE  " + tablename + " SET RETURN_FLAG='Y' WHERE DLTB01_SN='{0}'", dataKey["MZ_DLTB01_SN"]);
                        o_DBFactory.ABC_toTest.Edit_Data(editSQL);
                    }
                    catch
                    {
                        lm.SqlHistory("error修改歷程-0", editSQL, Request.QueryString["TPM_FION"]);

                    }

                    //增加回應紀錄用以人員登入時通知。 Add 20191018 by sky
                    try
                    {
                        editSQL = string.Format(@"INSERT INTO C_LEAVE_REPLY (SN, APPLICANT_ID, MZ_DLTB01_SN, RETURN_MESSAGE, READ_FALG, RETURN_ID, RETURN_DATE) 
                                                    Select  NEXT VALUE FOR dbo.C_LEAVE_REPLY_SN, b.MZ_ID, a.DLTB01_SN, a.REVIEW_MESSAGE, 'N', a.REVIEW_ID, GETDATE() From C_LEAVE_HISTORY a
                                                    left join C_DLTB01 b on a.DLTB01_SN = b.MZ_DLTB01_SN 
                                                    Where a.SN='{0}' ", dataKey["SN"]);
                        o_DBFactory.ABC_toTest.Edit_Data(editSQL);
                    }
                    catch (Exception ex)
                    {
                        LogModel.saveLog("C", "U", editSQL, new List<SqlParameter>(), Request.QueryString["TPM_FION"], ex.Message.PadRight(500, ' ').Substring(0, 499));
                    }
                }
                this.txt_message_Text = "";
            }
            catch (Exception ex)
            {
                LogModel lm = new LogModel("C");
                lm.sqlType = "U";
                string sql = lm.RegexSQL(agreeSQL, sp);
                lm.SqlHistory("error修改歷程-9", sql + ";--" + ex.Message, Request.QueryString["TPM_FION"]);


            }
        }


        /// <summary>
        /// 修改假單狀態(人事室呈辦人)
        /// </summary>
        /// <param name="DLTB01_NUMBER"></param>
        /// <param name="status">DLTB01 MZ_STATUS</param>
        /// <param name="chk1">DLTB01 MZ_CHK1</param>
        protected void SQL_UPDATE_DLTB01(int DLTB01_NUMBER, string status, string chk1)
        {
            string personSQL = @"UPDATE C_DLTB01 SET 
                                    MZ_STATUS =@MZ_STATUS,MZ_CHK1=@MZ_CHK1 WHERE MZ_DLTB01_SN = " + DLTB01_NUMBER;
            string MZ_STATUS = Service_C_STATUS.Get_C_STATUS_SN(status);
            List<SqlParameter> op = new List<SqlParameter>()
            {
                new SqlParameter("MZ_STATUS", SqlDbType.Float) { Value=MZ_STATUS },
                new SqlParameter("MZ_CHK1", chk1)
            };
            o_DBFactory.ABC_toTest.ExecuteNonQuery( personSQL, op.ToArray());

            //irk 2012/11/6 LOG增加
            LogModel lm = new LogModel("C");
            lm.sqlType = "U";
            string sql = lm.RegexSQL(personSQL, op.ToArray());
            lm.SqlHistory("修改假單資訊-核定", sql, Request.QueryString["TPM_FION"]);
        }

        /// <summary>
        /// 選定下一個呈核者HyperLink1
        /// 這邊的查詢結果都是寫到ViewState (上面再透過get set 讓他們變成可以留存的成員)
        /// SORT
        /// </summary>
        /// <param name="number">目前處理的是GridView上方選擇的第幾個簽核紀錄?</param>
        /// <param name="type"> 
        /// 0:抓當前的關卡代碼 SCHEDULE_CODE 
        /// 1:抓下一關的關卡代碼SCHEDULE_CODE , 且 SORT會+1 </param>
        /// <param name="gv">簽核紀錄的GridView物件</param>
        /// <param name="tablename">簽核資料表名稱</param>
        protected void Reviewer(int number, int type, GridView gv, string tablename)
        {
            /*
             會寫入以下ViewState
             this.SORT : 第幾關
             this.SCHEDULE : 關卡代碼
             this.EXAD : 機關,正常應該是根據假卡上的現服機關為主
             this.EXUNIT : 單位,比較奇怪的是,某些條件下會變
             */
            //從GridView上抓取目前選到的資料
            DataKey dataKey = gv.DataKeys[number];
            //20141022
            //string insertSQL = "SELECT * FROM " + tablename + " WHERE SN=" + DataKeys["SN"];

            //抓取當前簽核資料,的關卡代碼/假別
            string SQL = "SELECT LEAVE_SCHEDULE_SN,DLTB01_SN FROM " + tablename + " WHERE SN=" + dataKey["SN"];
            DataTable dt = new DataTable();
            dt = o_DBFactory.ABC_toTest.Create_Table(SQL, "INSERT");
            //抓當前的流程序號欄位? 第幾關?
            this.SORT = System.Convert.ToInt32(dt.Rows[0]["LEAVE_SCHEDULE_SN"]);

            //現在的關卡代碼
            int SORT_NO = (this.SORT + type);
            string LEAVE_SN = dataKey["LEAVE_SN"].ToStringNullSafe();
            this.SCHEDULE = Service_C_LEAVE_SCHEDULE.Get_SCHEDULE_CODE(SORT_NO, LEAVE_SN);


            if (this.SCHEDULE == null || this.SCHEDULE == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "alert('沒有下一層，同意請按決行，不同意請按退回')", true);
                this.re = "y";
                return;
            }

            //判斷目前在哪個關卡,應該查詢哪些機關單位,一些特殊邏輯

            // sam 婦幼隊 在 第一層決行時 人事應 拋給 副隊長 然後 副隊 再第一層決行 拋回 人事 此段待查 少年隊是用pazo
            if (SessionHelper.ADPMZ_EXAD == ConstAD.新北市政府警察局婦幼警察隊 && this.SCHEDULE.Equals("0015"))
            {
                this.SCHEDULE = "0021";
            }

            if (SessionHelper.ADPMZ_EXAD == ConstAD.新北市政府警察局婦幼警察隊 && this.SCHEDULE.Equals("0016"))
            {
                this.SCHEDULE = "0020";
            }
            DataTable AD_UNIT = Service_C_LEAVE_SCHEDULE_CODE.Get_AD_UNIT(this.SCHEDULE);
            //如果在流程設定中有抓到資料
            if (AD_UNIT.Rows.Count > 1)
            {

                this.EXAD = AD_UNIT.Rows[0]["MZ_EXAD"].ToString();
                foreach (DataRow dr in AD_UNIT.Rows)
                {
                    //20210521 保大新增人事管理-13.差勤線上簽核的流程
                    //20210618  交通警察大隊 新增人事管理-13.差勤線上簽核的流程
                    if (SessionHelper.ADPMZ_EXAD == ConstAD.新北市政府警察局保安警察大隊 || SessionHelper.ADPMZ_EXAD == ConstAD.新北市政府警察局交通警察大隊)
                    {
                        if (dr["MZ_EXUNIT"].ToString() == ConstUNIT.人事室)
                        {
                            this.EXUNIT = dr["MZ_EXUNIT"].ToString();
                        }
                    }
                    else
                    {
                        if (dr["MZ_EXUNIT"].ToString() == ConstUNIT.人事管理員)
                        {
                            this.EXUNIT = dr["MZ_EXUNIT"].ToString();
                        }
                    }
                }
            }
            else
            {
                //382130100C	新北市政府警察局保安警察大隊
                if (SessionHelper.ADPMZ_EXAD == ConstAD.新北市政府警察局保安警察大隊)
                {
                    this.EXAD = ConstAD.新北市政府警察局保安警察大隊;
                    this.EXUNIT = "";
                }
                //382130300C	新北市政府警察局交通警察大隊
                else if (SessionHelper.ADPMZ_EXAD == ConstAD.新北市政府警察局交通警察大隊)
                {
                    this.EXAD = ConstAD.新北市政府警察局交通警察大隊;
                    this.EXUNIT = "";
                }
                else
                {
                    this.EXAD = AD_UNIT.Rows[0]["MZ_EXAD"].ToString();
                    this.EXUNIT = AD_UNIT.Rows[0]["MZ_EXUNIT"].ToString();
                }

            }




            string DLTB01_SN = dt.Rows[0]["DLTB01_SN"].ToStringNullSafe();
            DataTable temp_dt = Service_C_DLTB01.Get_MZ_ID_AD_UNIT(DLTB01_SN);
            string id = temp_dt.Rows[0]["MZ_ID"].ToString();
            //如果上面都沒抓到 機關單位,則改以假卡上的機關單位為主
            if (this.EXAD == "") this.EXAD = temp_dt.Rows[0]["MZ_EXAD"].ToString();
            if (this.EXUNIT == "") this.EXUNIT = temp_dt.Rows[0]["MZ_EXUNIT"].ToString();
            //根據type 可能是 +0 or +1
            //+0 代表現在的陳核並非第一層陳核
            //+1 則是第一層陳核
            this.SORT = this.SORT + type;
            //這個欄位抓了卻沒有實質用途?
            SORT_NO = (this.SORT);
            LEAVE_SN = dataKey["LEAVE_SN"].ToStringNullSafe();
            base.ViewState["Reviewlevel"] = Service_C_LEAVE_SCHEDULE.Get_REVIEW_LEAVE(SORT_NO, LEAVE_SN);
        }






        /// <summary>
        /// 根據 當前勾選的 歷程記錄SN 和 歷程記錄資料表名稱 ,找到假卡資料 ,綁定來源機關單位
        /// </summary>
        /// <param name="number">目前處理的是GridView上方選擇的第幾個簽核紀錄?</param>
        /// <param name="type"> 
        /// 0:抓當前的關卡代碼 SCHEDULE_CODE 
        /// 1:抓下一關的關卡代碼SCHEDULE_CODE , 且 SORT會+1 </param>
        /// <param name="gv">簽核紀錄的GridView物件</param>
        /// <param name="tablename">簽核資料表名稱</param>
        protected void Reviewer_V2(string History_SN, string tablename)
        {
            /*
             會寫入以下ViewState
             this.EXAD : 機關,正常應該是根據假卡上的現服機關為主
             this.EXUNIT : 單位,比較奇怪的是,某些條件下會變
             */
            //從GridView上抓取目前選到的資料
            //抓取當前簽核資料,取得 假卡的 SN
            string SQL = "SELECT LEAVE_SCHEDULE_SN,DLTB01_SN FROM " + tablename + " WHERE SN=" + History_SN.SafeToInt();
            DataTable dt = new DataTable();
            dt = o_DBFactory.ABC_toTest.Create_Table(SQL, "INSERT");

            //根據 假卡的 SN 抓取 請假資料,目的是要抓取申請人當時所屬的現服分局 現服機關
            string DLTB01_SN = dt.Rows[0]["DLTB01_SN"].ToStringNullSafe();
            DataTable temp_dt = Service_C_DLTB01.Get_MZ_ID_AD_UNIT(DLTB01_SN);
            this.EXAD = temp_dt.Rows[0]["MZ_EXAD"].ToString();
            this.EXUNIT = temp_dt.Rows[0]["MZ_EXUNIT"].ToString();
        }

        //退回btn  
        protected void btn_back_Click(object sender, EventArgs e)
        {
            string myMsg = string.Empty;
            int count = gv_data.Rows.Count;
            for (int n = 0; n < gv_data.Rows.Count; n++)
            {
                CheckBox myCheckbox = (CheckBox)gv_data.Rows[n].FindControl("ckbox_select");
                if (myCheckbox.Checked == true)
                {
                    //strSQL = @"UPDATE C_LEAVE_HISTORY SET 
                    //    PROCESS_DATE=@PROCESS_DATE,PROCESS_STATUS=@PROCESS_STATUS,
                    //    REVIEW_MESSAGE=@REVIEW_MESSAGE,PROCESS_TIME=@PROCESS_TIME 
                    //    WHERE SN =" + gv_data.DataKeys[n]["SN"];
                    SQL_UPDATE_XXX_HISTORY(n, btn_back.Text.ToString(), gv_data, "C_LEAVE_HISTORY", false);
                }
            }

            DataTable tempMuch = new DataTable();
            CSonline = new CSonline("doSearch", SessionHelper.ADPMZ_ID);
            tempMuch = CSonline.getSearch();
            if (tempMuch.Rows.Count == 0)
            {
                this.type = "Y";
            }
            gv_data.DataSource = tempMuch;
            gv_data.DataBind();
            gv_data.Visible = true;
            Page_update();

            if (this.type != "")
            {
                this.type = "";
                Response.Redirect("~/18-Online_Leave/Online_work.aspx?TPM_FION=" + _TPM_FION);
                this.ViewStateClear();
            }

            this.RSN_LENTH = count;
            //   P_E_BACK.Show();
            //Label1.Text = "測試SN : " + ViewState["SN0"];
        }

        //跳出意見回覆的確定btn
        protected void btn_ck_Click(object sender, EventArgs e)
        {
            this.txt_message_Text = txt_message.Text;

        }

        protected void message_update(int index, string message)
        {
            strSQL = @"UPDATE C_LEAVE_HISTORY SET 
                        REVIEW_MESSAGE=@REVIEW_MESSAGE 
                        WHERE SN =" + index;
            SqlParameter[] sp = {
                                new SqlParameter("REVIEW_MESSAGE",SqlDbType.VarChar){Value = message},
                                };
            o_DBFactory.ABC_toTest.ExecuteNonQuery( strSQL, sp);

            //irk 2012/11/6 LOG增加
            LogModel lm = new LogModel("C");
            lm.sqlType = "U";
            string sql = lm.RegexSQL(strSQL, sp);
            lm.SqlHistory("歷程意見修改", sql, Request.QueryString["TPM_FION"]);
        }



        //所有已決行待核定的假單
        protected DataTable PersonSearch(string tablename)
        {
            //0812→Dean
            int year = DateTime.Now.Year - 1911;
            if (this.MODE == Const_Mode.已核定假單)
            {
                int process = 0;

                if (tablename == "C_LEAVE_HISTORY")
                {
                    process = 7;
                }
                else
                {
                    process = 2;
                }

                strSQL = "select C_DLTB01.*," + tablename + ".*,C_DLCODE.MZ_CNAME FROM " + tablename + " INNER JOIN "
                 + "C_DLTB01 ON " + tablename + ".DLTB01_SN= C_DLTB01.MZ_DLTB01_SN "
                 + "INNER JOIN C_DLCODE ON C_DLTB01.MZ_CODE = C_DLCODE.MZ_CODE "
                 + "WHERE " + tablename + ".PROCESS_STATUS ='" + process + "' ";
            }
            else
            {
                strSQL = "select C_DLTB01.*," + tablename + ".*,C_DLCODE.MZ_CNAME FROM " + tablename + " INNER JOIN "
                     + "C_DLTB01 ON " + tablename + ".DLTB01_SN= C_DLTB01.MZ_DLTB01_SN "
                     + "INNER JOIN C_DLCODE ON C_DLTB01.MZ_CODE = C_DLCODE.MZ_CODE "
                     + "WHERE 1=1 AND " + tablename + ".REVIEW_ID='" + SessionHelper.ADPMZ_ID
                     + "'AND " + tablename + ".LEAVE_SCHEDULE_SN = '0' AND " + tablename + ".PROCESS_STATUS ='4' ";
            }
            if (ViewState["search"] == null)
            {
                strSQL = strSQL + " AND C_DLTB01.MZ_EXAD = '" + SessionHelper.ADPMZ_EXAD + "' ";
                ViewState.Remove("search");
            }
            else
            {
                if (txt_name.Text != "")
                {
                    strSQL = strSQL + " AND C_DLTB01.MZ_NAME = '" + txt_name.Text + "' ";
                }
                if (txt_idno.Text != "")
                {
                    strSQL = strSQL + " AND C_DLTB01.MZ_ID = '" + txt_idno.Text + "' ";
                }
                if (txt_date.Text != "" && txt_date1.Text != "")
                {
                    strSQL = strSQL + " AND C_DLTB01.MZ_IDATE1 >= '" + txt_date.Text + "' AND C_DLTB01.MZ_IDATE1<='" + txt_date1.Text + "'";
                }
                else if (txt_date.Text != "")
                {
                    strSQL = strSQL + " AND C_DLTB01.MZ_IDATE1 = '" + txt_date.Text + "'";
                }
                if (DDL_AD.SelectedItem.Text != "請選擇")
                {
                    strSQL = strSQL + " AND C_DLTB01.MZ_EXAD = '" + DDL_AD.SelectedValue + "' ";
                }
                if (DDL_UNIT.SelectedItem.Text != "請選擇")
                {
                    strSQL = strSQL + " AND C_DLTB01.MZ_EXUNIT = '" + DDL_UNIT.SelectedValue + "' ";
                }
                ViewState.Remove("search");
            }
            strSQL = strSQL + "order by " + tablename + ".SN";
            temp_SharedDT = o_DBFactory.ABC_toTest.Create_Table(strSQL, "Person_gv");

            //Log
            LogModel lm = new LogModel("C");
            lm.sqlType = "S";
            string sql = lm.RegexSQL(strSQL, new List<SqlParameter>());
            lm.SqlHistory("查詢線上簽核", sql, Request.QueryString["TPM_FION"]);
            return temp_SharedDT;
        }

        //歷程 
        protected DataTable HistorySearch()
        {
            //0808→DEAN
            var qHistory = this.History.ToStringNullSafe();
            string cHistory = "'" + string.Join("','", qHistory.Split(',').ToArray()) + "'";
            //string historySQL = @"SELECT C_LEAVE_HISTORY.SN ,C_LEAVE_HISTORY.PROCESS_DATE,
            //       C_LEAVE_HISTORY.PROCESS_TIME,C_LEAVE_HISTORY.REVIEW_MESSAGE,C_LEAVE_HISTORY.REVIEW_ID,
            //       C_STATUS.C_STATUS_NAME,C_DLTB01.MZ_FILE,A_DLBASE.MZ_NAME,A_DLBASE.MZ_EXAD,A_DLBASE.MZ_EXUNIT,A_DLBASE.MZ_OCCC FROM C_LEAVE_HISTORY 
            //       INNER JOIN C_STATUS ON C_LEAVE_HISTORY.PROCESS_STATUS = C_STATUS.C_STATUS_SN 
            //       INNER JOIN C_DLTB01 ON C_LEAVE_HISTORY.DLTB01_SN = C_DLTB01.MZ_DLTB01_SN 
            //       INNER JOIN A_DLBASE ON C_LEAVE_HISTORY.REVIEW_ID = A_DLBASE.MZ_ID 
            //       WHERE C_LEAVE_HISTORY.DLTB01_SN =" + int.Parse(this.History) +
            //      " ORDER BY C_LEAVE_HISTORY.SN";
            string historySQL = @"SELECT C_LEAVE_HISTORY.SN ,C_LEAVE_HISTORY.PROCESS_DATE,
                   C_LEAVE_HISTORY.PROCESS_TIME,C_LEAVE_HISTORY.REVIEW_MESSAGE,C_LEAVE_HISTORY.REVIEW_ID,
                   C_STATUS.C_STATUS_NAME,C_DLTB01.MZ_FILE,A_DLBASE.MZ_NAME,A_DLBASE.MZ_EXAD,A_DLBASE.MZ_EXUNIT,A_DLBASE.MZ_OCCC FROM C_LEAVE_HISTORY 
                   INNER JOIN C_STATUS ON C_LEAVE_HISTORY.PROCESS_STATUS = C_STATUS.C_STATUS_SN 
                   INNER JOIN C_DLTB01 ON C_LEAVE_HISTORY.DLTB01_SN = C_DLTB01.MZ_DLTB01_SN 
                   INNER JOIN A_DLBASE ON C_LEAVE_HISTORY.REVIEW_ID = A_DLBASE.MZ_ID 
                   WHERE C_LEAVE_HISTORY.DLTB01_SN  in (" + cHistory + ")" +
                  " ORDER BY C_LEAVE_HISTORY.SN";
            temp_SharedDT = o_DBFactory.ABC_toTest.Create_Table(historySQL, "history");

            return temp_SharedDT;
        }

        //把要請假的時間結合起來MZ_IDATE1、MZ_ITIME1、MZ_ODATE、MZ_OTIME
        protected string DateAndTime(DataTable temp)
        {
            string date;
            date = temp.Rows[0]["MZ_IDATE1"].ToString().Substring(0, 3) + "年"
                 + temp.Rows[0]["MZ_IDATE1"].ToString().Substring(3, 2) + "月"
                 + temp.Rows[0]["MZ_IDATE1"].ToString().Substring(5, 2) + "日  ";
            date += temp.Rows[0]["MZ_ITIME1"].ToString().Substring(0, 2) + "點"
                  + temp.Rows[0]["MZ_ITIME1"].ToString().Substring(3, 2) + "分 至 ";
            date += temp.Rows[0]["MZ_ODATE"].ToString().Substring(0, 3) + "年"
                    + temp.Rows[0]["MZ_ODATE"].ToString().Substring(3, 2) + "月"
                    + temp.Rows[0]["MZ_ODATE"].ToString().Substring(5, 2) + "日";
            date += temp.Rows[0]["MZ_OTIME"].ToString().Substring(0, 2) + "點"
                     + temp.Rows[0]["MZ_OTIME"].ToString().Substring(3, 2) + "分";
            date += "  共計" + temp.Rows[0]["MZ_TDAY"].ToString() + "日" + temp.Rows[0]["MZ_TTIME"].ToString() + "時";
            return date;
        }

        /// <summary>
        /// 選下一個呈核者GV
        /// </summary>
        /// <param name="exad"></param>
        /// <param name="exunit"></param>
        /// <param name="gv"></param>
        /// <param name="isToUpper">是否為往上承</param>
        /// <returns>是否有抓到可陳核人?</returns>
        protected bool GV_CHECK_show(GridView gv, bool isToUpper)
        {
            //取得線上簽核功能中,可以再陳核的人員資料,針對Online_work.aspx功能
            DataTable temp = OWService.Get_REVIEW_MANAGEMENT_forOnline_work(
                 SessionHelper.ADPMZ_EXAD
                 , SessionHelper.ADPMZ_EXUNIT
                , 0, SessionHelper.ADPMZ_ID, isToUpper);

            //抓到的資料數
            int x = temp.Rows.Count;
            GV_CHECKER.DataSource = temp;
            GV_CHECKER.DataBind();
            this.s = "y";
            if (x == 0)
            {   //標記為 沒抓到陳核人資料
                this.np = "y";
                return false;
            }
            //有抓到陳核人資料
            return true;
        }
        /// <summary>
        /// 當點選了陳核人員
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GV_CHECKER_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //0815→Dean
            if (e.CommandName == "checker")
            {
                //找出上方 差假記錄Gridview 勾選的
                for (int a = 0; a < gv_data.Rows.Count; a++)
                {
                    if (this.Get_upd(a) != "")
                    {
                        //推動進度,根據 tablename UI上的陳核人員勾選,更新審核歷程資料
                        SQL_UPDATE_XXX_HISTORY(a, this.btn, gv_data, "C_LEAVE_HISTORY", true);                        
                        this.Set_upd(a, "");
                    }
                }
                this.btn = "";

                int count = this.RSN_LENTH;

                for (int i = 0; i < count; ++i)
                {
                    //寫入下一關歷程的起始資料,for請假紀錄
                    SQL_INSERT_NEXT_C_LEAVE_HISTORY(e.CommandArgument.ToString(), this.Get_RSN(i));
                }
                this.s = "";
                DataTable tempMuch = new DataTable();
                //所有此人未簽核的內容(不包含代理人)
                CSonline = new CSonline("doSearch", SessionHelper.ADPMZ_ID);
                tempMuch = CSonline.getSearch();
                gv_data.DataSource = tempMuch;
                gv_data.DataBind();
                gv_data.Visible = true;

                if (this.type != "")
                {
                    this.type = "";
                    Response.Redirect("~/18-Online_Leave/Online_work.aspx?TPM_FION=" + _TPM_FION);
                    this.ViewStateClear();
                }

                Panel_select_ModalPopupExtender.Hide();
                Page_update();
            }
        }

        /// <summary>
        /// 寫入下一關歷程的起始資料,for請假紀錄
        /// </summary>
        /// <param name="C_REVIEW_MANAGEMENT_SN">選定的陳核人員的設定資料的SN,依此紀錄被陳核人的ID</param>
        /// <param name="C_LEAVE_HISTORY_SN">這關的歷程資料的SN,為了抓取一些資訊往後抄錄</param>
        private void SQL_INSERT_NEXT_C_LEAVE_HISTORY(string C_REVIEW_MANAGEMENT_SN, string C_LEAVE_HISTORY_SN)
        {

            //抓取目前選中的陳核人員ID
            string REVIEW_ID = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_ID FROM C_REVIEW_MANAGEMENT WHERE SN = '" + C_REVIEW_MANAGEMENT_SN + "'");
            string year = (System.Convert.ToInt32(DateTime.Now.Year.ToString()) - 1911).ToString();
            string today = year + DateTime.Now.ToString("MMdd");
            string now = DateTime.Now.ToString("HH:mm:ss");
            //新增下一關的起始資料
            string strSQL = @"INSERT INTO C_LEAVE_HISTORY
                                                                   (SN,LEAVE_SN,REVIEW_ID,LETTER_DATE,LETTER_TIME,LEAVE_SCHEDULE_SN,DLTB01_SN,PROCESS_STATUS,MZ_MID)
                                                              VALUES
                                                                   ( NEXT VALUE FOR dbo.C_LEAVE_HISTORY_SN,@LEAVE_SN,@REVIEW_ID,@LETTER_DATE,@LETTER_TIME,@LEAVE_SCHEDULE_SN,@DLTB01_SN,@PROCESS_STATUS,@MZ_MID)";




            //抓這一關的資料,做為參考
            DataTable temp_dt = o_DBFactory.ABC_toTest.Create_Table("SELECT LEAVE_SN , DLTB01_SN,LEAVE_SCHEDULE_SN FROM C_LEAVE_HISTORY WHERE SN = '" + C_LEAVE_HISTORY_SN + "'", "get");
            //以下參數從當前的歷史紀錄照抄
            string LEAVE_SN = temp_dt.Rows[0]["LEAVE_SN"].ToString();
            string DLTB01_SN = temp_dt.Rows[0]["DLTB01_SN"].ToString();
            string LEAVE_SCHEDULE_SN = temp_dt.Rows[0]["LEAVE_SCHEDULE_SN"].ToString();
            // string SN = o_DBFactory.ABC_toTest.vExecSQL("select max(dbo.to_number(SN)) + 1  from C_LEAVE_HISTORY");


            SqlParameter[] parameterList = {
                    //new SqlParameter("SN",SqlDbType.VarChar){Value = SN},
                    new SqlParameter("LEAVE_SN",SqlDbType.VarChar){Value = LEAVE_SN},
                    new SqlParameter("REVIEW_ID",SqlDbType.VarChar){Value = REVIEW_ID},
                    new SqlParameter("LETTER_DATE",SqlDbType.VarChar){Value = today},
                    new SqlParameter("LEAVE_SCHEDULE_SN",SqlDbType.VarChar){Value = LEAVE_SCHEDULE_SN},
                    new SqlParameter("LETTER_TIME",SqlDbType.VarChar){Value = now},
                    new SqlParameter("DLTB01_SN",SqlDbType.Float){Value = DLTB01_SN},
                    new SqlParameter("PROCESS_STATUS",SqlDbType.Char){Value = '4'},
                    new SqlParameter("MZ_MID",SqlDbType.VarChar){Value =  SessionHelper.ADPMZ_ID}
                                                  };
            //20141022
            try
            {
                o_DBFactory.ABC_toTest.ExecuteNonQuery( strSQL, parameterList);
            }
            catch (Exception ex)
            {
                LogModel lm = new LogModel("C");
                lm.sqlType = "A";
                string sql = lm.RegexSQL(strSQL, parameterList);
                lm.SqlHistory("error新增核定歷程", sql + ";--" + ex.Message, Request.QueryString["TPM_FION"]);
            }
            //irk 2012/11/6 LOG增加
            //LogModel lm = new LogModel("C");
            //lm.sqlType = "UP";
            //string sql = lm.RegexSQL(strSQL, parameterList);
            //lm.SqlHistory("新增核定歷程", sql, Request.QueryString["TPM_FION"]);
        }

        protected void GV_CHECKER_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GV_CHECKER.PageIndex = e.NewPageIndex;
            GV_CHECK_show( GV_CHECKER, this.isUpper);
            Panel_select_ModalPopupExtender.Show();
        }

        protected void btn_exit_Click(object sender, EventArgs e)
        {
            this.s = "";
            Panel_select_ModalPopupExtender.Hide();
        }

        /// <summary>
        /// 按鈕:差假抽回
        /// 0815→Dean 抽回簽核
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_re_Click(object sender, EventArgs e)
        {
            for (int a = 0; a < gv_data.Rows.Count; a++)
            {
                CheckBox agreeCK = (CheckBox)gv_data.Rows[a].FindControl("ckbox_select");
                if (agreeCK.Checked == true)
                {
                    string sn = gv_data.DataKeys[a]["MZ_DLTB01_SN"].ToString();
                    int s = System.Convert.ToInt32(gv_data.DataKeys[a]["LEAVE_SCHEDULE_SN"].ToString());

                    string sn2 = o_DBFactory.ABC_toTest.vExecSQL(string.Format(@"select sn from 
                                                    (select rownum as row_id ,sn  from 
                                                    (select * from c_leave_history where dltb01_sn='{0}'  order by sn desc)) 
                                                    where row_id=2", gv_data.DataKeys[a]["MZ_DLTB01_SN"].ToString()));

                    string upd = string.Format(@"UPDATE C_LEAVE_HISTORY SET  PROCESS_DATE = '' ,PROCESS_STATUS = '{0}',PROCESS_TIME = ''  WHERE SN={1}", 4, sn2);
                    strSQL = "Delete FROM C_LEAVE_HISTORY WHERE SN = " + gv_data.DataKeys[a]["SN"].ToString();
                    o_DBFactory.ABC_toTest.ExecuteNonQuery( upd);
                    o_DBFactory.ABC_toTest.ExecuteNonQuery( strSQL);

                    //irk 2012/11/6 LOG增加
                    LogModel lm = new LogModel("C");
                    lm.sqlType = "U";
                    try
                    {
                        string sql = lm.RegexSQL(upd, new List<SqlParameter>());
                        lm.SqlHistory("抽回簽核", sql, Request.QueryString["TPM_FION"]);
                    }
                    catch
                    {
                    }

                    //irk 2012/11/6 LOG增加
                    lm = new LogModel("C");
                    lm.sqlType = "U";
                    try
                    {
                        string sql = lm.RegexSQL(strSQL, new List<SqlParameter>());
                        lm.SqlHistory("刪除歷程", sql, Request.QueryString["TPM_FION"]);
                    }
                    catch
                    {
                    }
                }
            }

            DataTable temp = new DataTable();
            CSonline = new CSonline("reSearch", SessionHelper.ADPMZ_ID);
            temp = CSonline.getDate();
            gv_data.DataSource = temp;
            gv_data.DataBind();
            gv_data.Visible = true;
            pl_history.Visible = false;
            Page_update();
        }

        //呈辦人核定 (退回=no、核定=ok) irk
        protected void person_command(object sender, CommandEventArgs e)
        {
            //0812→Dean
            int count = 0;
            string agreeSQL = string.Empty;
            for (int p = 0; p < gv_data.Rows.Count; p++)
            {
                CheckBox personCK = (CheckBox)gv_data.Rows[p].FindControl("ckbox_select");
                if (personCK.Checked == true)
                {
                    if (e.CommandName == "ok")
                    {
                        SQL_UPDATE_XXX_HISTORY(p, btn_ok.Text.ToString(), gv_data, "C_LEAVE_HISTORY", true);
                        SQL_UPDATE_DLTB01(int.Parse(gv_data.DataKeys[p]["MZ_DLTB01_SN"].ToString()), btn_ok.Text.ToString(), "Y");
                    }
                    else if (e.CommandName == "no")
                    {
                        SQL_UPDATE_XXX_HISTORY(p, btn_no.Text.ToString(), gv_data, "C_LEAVE_HISTORY", false);
                        SQL_UPDATE_DLTB01(int.Parse(gv_data.DataKeys[p]["MZ_DLTB01_SN"].ToString()), btn_no.Text.ToString(), "N");
                    }
                    count++;
                }
            }

            if (this.type != "")
            {
                this.type = "";
                Response.Redirect("~/18-Online_Leave/Online_work.aspx?TPM_FION=" + _TPM_FION);
                this.ViewStateClear();
            }

            //DataTable tempMuch = new DataTable();
            //tempMuch = PersonSearch("C_LEAVE_HISTORY");
            //gv_data.DataSource = tempMuch;
            //gv_data.DataBind();
            btn_search_Click(sender, e);
            gv_data.Visible = true;
            pl_history.Visible = false;
            this.RSN_LENTH = count;
            Page_update();

        }

        //gv_data歷程btn
        protected void btn_history_Command(object sender, CommandEventArgs e)
        {
            btn_bef_process.CommandArgument = e.CommandArgument.ToString();

            this.History = e.CommandArgument;



            DataTable tempHistory = new DataTable();
            tempHistory = HistorySearch();
            gv_history.DataSource = tempHistory;
            gv_history.DataBind();
            //顯示判定//顯示判定//顯示判定//顯示判定
            pl_history.Visible = true;
            pl_overtime_history.Visible = false;
        }

        protected void btn_message_Command(object sender, CommandEventArgs e)
        {
            txt_message.Text = string.Empty;
            P_E_BACK.Show();
        }
        #endregion

        #region 加班
        //加班-審核者要呈核的時後，沒有同層的人的確定btn
        protected void Button3_Click(object sender, EventArgs e)
        {
            M_P_E_overtime_selcet.Hide();
        }

        /*無用程式碼
        //個人加班歷程GV
        protected void gv_overtime_self_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataTable tempOTS = new DataTable();
            tempOTS = OverTime_Self_Search();
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label date = (Label)e.Row.FindControl("lbl_self_date");
                Label s = (Label)e.Row.FindControl("lbl_self_s");
                Label exunit = (Label)e.Row.FindControl("lbl_exunit_self");
                if (tempOTS.Rows[iSelf]["OVERTIME_STATUS"].ToString() == null || tempOTS.Rows[iSelf]["OVERTIME_STATUS"].ToString() == "")
                {
                    s.Text = "尚未審核";
                }
                else
                {
                    s.Text = o_DBFactory.ABC_toTest.vExecSQL("select C_STATUS_NAME FROM C_STATUS WHERE C_STATUS_SN = " +
                       int.Parse(tempOTS.Rows[iSelf]["OVERTIME_STATUS"].ToString()));
                }
                exunit.Text = o_DBFactory.ABC_toTest.vExecSQL("select MZ_KCHI from A_KTYPE where MZ_KTYPE = 25 and MZ_EXUNIT ='" +
                    (o_DBFactory.ABC_toTest.vExecSQL("select MZ_EXUNIT from C_REVIEW_MANAGEMENT where MZ_ID = '" + tempOTS.Rows[iSelf]["REVIEW_ID"].ToString() + "'") + "'"));//o_DBFactory.ABC_toTest.vExecSQL("select MZ_KCHI from A_KTYPE where MZ_KTYPE='25' and MZ_KCODE='" + tempOTS.Rows[iSelf ]["MZ_EXUNIT"].ToString () + "'");
                overtime_histyry_date = tempOTS.Rows[iSelf]["INS_DATE"].ToString().Substring(0, 3) + "/" +
                        tempOTS.Rows[iSelf]["INS_DATE"].ToString().Substring(3, 2) + "/" +
                        tempOTS.Rows[iSelf]["INS_DATE"].ToString().Substring(5, 2);
                date.Text = overtime_histyry_date.ToString();
                iSelf++;
            }
        }
        */

        protected void gv_data_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_data.PageIndex = e.NewPageIndex;
            if (this.MODE == Const_Mode.差假線上簽核)  //差假線上審核
            {
                CSonline = new CSonline("doSearch", SessionHelper.ADPMZ_ID);
                temp_SharedDT = CSonline.getSearch();
                gv_data.DataSource = temp_SharedDT;
                gv_data.DataBind();
                gv_data.Visible = true;
            }
            else if (this.MODE == Const_Mode.簽核狀態)
            {

                //2014/01/08  直接拿下拉時的function來用,不過裡面一些顯示的切換沒有細查,有人反映有問題再說
                flow_Click(txt_m.Text, DropDownList1.SelectedValue, "C_LEAVE_HISTORY");


            }
            else if (this.MODE == Const_Mode.已決行案件 || this.MODE == Const_Mode.查詢簽核狀態)
            {
                if (this.gv_data_Person != null)
                {
                    gv_data.DataSource = this.gv_data_Person;
                    gv_data.DataBind();
                    gv_data.Visible = true;
                }
            }
            //20140218
            else if (this.MODE == Const_Mode.抽回差假簽核)//抽回線上簽核
            {

                DataTable temp = new DataTable();

                CSonline = new CSonline("reSearch", SessionHelper.ADPMZ_ID);
                temp = CSonline.getDate();
                gv_data.DataSource = temp;
                gv_data.DataBind();
                gv_data.Visible = true;
            }
        }
        //加班 承辦人搜尋
        protected void btn_search1_Click(object sender, EventArgs e)
        {
            if (txt_MZ_ID.Text == "" && txt_MZ_NAME.Text != "")
            {
                int repeat = int.Parse(o_DBFactory.ABC_toTest.vExecSQL("select count(*) from A_DLBASE where "
                      + " MZ_NAME ='" + txt_MZ_NAME.Text + "' and MZ_EXAD = '" + DDL_EXAD.SelectedValue + "'"));

                if (repeat > 1)
                {
                    gv_search.DataSource = o_DBFactory.ABC_toTest.Create_Table("select * from A_DLBASE where "
                        + "MZ_NAME = '" + txt_MZ_NAME.Text + "' and MZ_EXAD = '" + DDL_EXAD.SelectedValue + "'", "NAME");
                    gv_search.DataBind();
                    M_P_E_Search.Show();
                }
                else
                {
                    ViewState["overtime_search"] = "NAME";
                    gv_OT.DataSource = OverTime_Search(DDL_EXAD.SelectedValue, DDL_EXUNIT.SelectedValue, txt_MZ_ID.Text, txt_MZ_NAME.Text, txt_MZ_DATE.Text, txt_MZ_DATE1.Text);
                    gv_OT.DataBind();
                }
            }
            else
            {
                ViewState["overtime_search"] = "NAME";
                gv_OT.DataSource = OverTime_Search(DDL_EXAD.SelectedValue, DDL_EXUNIT.SelectedValue, txt_MZ_ID.Text, txt_MZ_NAME.Text, txt_MZ_DATE.Text, txt_MZ_DATE1.Text);
                gv_OT.DataBind();
            }

            allclose();
            lbl_sign.Visible = true;
            // pl_data2.Visible = true;
            pl_search.Visible = true;
            pl_overtime_gv.Visible = true;

        }

        /// <summary>
        /// 搜尋條件(機關、身份證、名字、日期)
        /// </summary>
        /// <param name="DDL"></param>
        /// <param name="ID"></param>
        /// <param name="NAME"></param>
        /// <param name="EXAD"></param>
        /// <returns></returns>
        protected DataTable OverTime_Search(string AD, string UNIT, string ID, string NAME, string DATE, string DATE1)
        {
            strSQL = @"select C_OVERTIME_CHECKFLOW.*,C_OVERTIME_HOUR_INSIDE.SN,C_OVERTIME_HOUR_INSIDE.MZ_EXUNIT,
                C_OVERTIME_HOUR_INSIDE.MZ_ID,C_OVERTIME_HOUR_INSIDE.MZ_OCCC,C_OVERTIME_HOUR_INSIDE.OTIME,
                C_OVERTIME_HOUR_INSIDE.HOUR_PAY,C_OVERTIME_HOUR_INSIDE.PAY_SUM,C_OVERTIME_HOUR_INSIDE.OTREASON,
                C_OVERTIME_HOUR_INSIDE.INS_DATE,C_OVERTIME_HOUR_INSIDE.MZ_EXAD,C_OVERTIME_HOUR_INSIDE.MZ_DATE,
                C_OVERTIME_HOUR_INSIDE.MZ_RESTHOUR FROM C_OVERTIME_CHECKFLOW
                INNER JOIN C_OVERTIME_HOUR_INSIDE ON C_OVERTIME_CHECKFLOW.OVERTIME_SN = C_OVERTIME_HOUR_INSIDE.SN 
                WHERE C_OVERTIME_HOUR_INSIDE.OVERTIME_STATUS='2' AND C_OVERTIME_CHECKFLOW.PROCESS_STATUS='2'  ";

            //都有條件
            if (AD != "" && ID != "" && NAME != "" && DATE != "" && AD != "請選擇" && UNIT != "請選擇" && UNIT != "" && DATE1 != "")
            {
                strSQL = strSQL + " AND MZ_EXAD ='" + AD
                        + "' and MZ_ID='" + o_DBFactory.ABC_toTest.vExecSQL("select MZ_ID from A_DLBASE where MZ_NAME ='"
                            + NAME + "' and MZ_EXAD ='" + AD.ToString() + "' and MZ_ID='" + ID + "'")
                        + "' and  MZ_ID ='" + ID + "' AND dbo.SUBSTR(C_OVERTIME_HOUR_INSIDE.MZ_DATE,1,5) >='" + DATE.ToString() + "' AND dbo.SUBSTR(C_OVERTIME_HOUR_INSIDE.MZ_DATE,1,5) <='" + DATE1.ToString() + "'";
            }
            else
            {
                //判斷是否有輸入，有的話 加判斷式

                if (AD != "請選擇" && AD != "")
                    strSQL = strSQL + "AND MZ_EXAD = '" + AD.ToString() + "'";

                if (UNIT != "請選擇")
                    strSQL = strSQL + "AND MZ_EXUNIT = '" + UNIT.ToString() + "'";
                //看是哪邊來的姓名判斷 overtime_search=ID 代表傳來的是身份證 NAME=姓名
                switch (ViewState["overtime_search"].ToString())
                {
                    case "ID":
                        strSQL = strSQL + " and MZ_ID = '" + NAME + "'";
                        break;
                    case "NAME":
                        if (NAME != "")
                            strSQL = strSQL + " AND MZ_ID = '" +
                                  o_DBFactory.ABC_toTest.vExecSQL("select MZ_ID from A_DLBASE where MZ_NAME ='"
                                  + NAME + "' and MZ_EXAD = '" + AD.ToString() + "'") + "'"; //

                        break;
                }

                //身份證
                if (ID != "")
                    strSQL = strSQL + " AND MZ_ID = '" + ID.ToString() + "'";
                //日期
                if (DATE1 != "" && DATE != "")
                    strSQL = strSQL + " AND dbo.SUBSTR(C_OVERTIME_HOUR_INSIDE.MZ_DATE,1,5) >='" + DATE.ToString() + "'  AND dbo.SUBSTR(C_OVERTIME_HOUR_INSIDE.MZ_DATE,1,5) <='" + DATE1.ToString() + "'";
                if (DATE != "")
                    strSQL = strSQL + " AND dbo.SUBSTR(C_OVERTIME_HOUR_INSIDE.MZ_DATE,1,5) ='" + DATE.ToString() + "'";
                //+ " dbo.SUBSTR(C_OVERTIME_HOUR_INSIDE.MZ_DATE,4,2)='" + DropDownList1.SelectedValue.ToString()
                //+ "' AND dbo.SUBSTR(C_OVERTIME_HOUR_INSIDE.MZ_DATE,1,3)='" + txt_m.Text.ToString()
                //+ "' ORDER BY C_OVERTIME_HOUR_INSIDE.MZ_DATE DESC";
            }
            temp_SharedDT = o_DBFactory.ABC_toTest.Create_Table(strSQL, "allOverTime");
            return temp_SharedDT;
        }

        //加班 承辦人search時 多個人名重覆 選擇的人 btn_search
        protected void btn_search_Command(object sender, CommandEventArgs e)
        {
            ViewState["overtime_search"] = "ID";
            gv_OT.DataSource = OverTime_Search(DDL_EXAD.SelectedValue, DDL_EXUNIT.SelectedValue, txt_MZ_ID.Text,
               e.CommandArgument.ToString(), txt_MZ_DATE.Text, txt_MZ_DATE1.Text);
            gv_OT.DataBind();
            allclose();
            //pl_data2.Visible = true;
            pl_search.Visible = true;
            pl_overtime_gv.Visible = true;
        }

        //加班 承辦人search時 多個人名重覆
        protected void gv_search_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label exad = (Label)e.Row.FindControl("lbl_search_exad");
                Label exunit = (Label)e.Row.FindControl("lbl_search_exunit");
                Button ID = (Button)e.Row.FindControl("btn_search");

                DataTable ad_UNIT = o_DBFactory.ABC_toTest.Create_Table("select MZ_EXAD,MZ_EXUNIT from A_DLBASE where MZ_ID = '" + ID.CommandArgument + "'", "get");
                exad.Text = ad_UNIT.Rows[0]["MZ_EXAD"].ToString();
                exunit.Text = ad_UNIT.Rows[0]["MZ_EXUNIT"].ToString();

                //exad.Text = o_DBFactory.ABC_toTest.vExecSQL("select MZ_KCHI from A_KTYPE where MZ_KCODE = '"
                //     + o_DBFactory.ABC_toTest.vExecSQL("select MZ_EXAD from A_DLBASE where MZ_ID = '" + ID.CommandArgument + "'") + "'");
                //exunit.Text = o_DBFactory.ABC_toTest.vExecSQL("select MZ_KCHI from A_KTYPE where MZ_KCODE= '"
                //    + o_DBFactory.ABC_toTest.vExecSQL("select MZ_EXUNIT from A_DLBASE where MZ_ID = '" + ID.CommandArgument + "'") + "'");

            }
        }

        //選擇月份
        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (txt_m.Text == "")
            {
                txt_m.Text = (System.Convert.ToInt32(DateTime.Now.Year.ToString()) - 1911).ToString();
            }
            if (txt_m.Text.Length > 3)
            {
                txt_m.Text = "請輸入正確民國年";
                return;
            }
            else if (txt_m.Text.Length == 2)
            {
                txt_m.Text = "0" + txt_m.Text;
            }
            else if (this.WHAT == "PERSON")
            {
                if (this.WHATS == "overtime")
                {
                    gv_OT.DataSource = OverTime_Self_Search();
                    gv_OT.DataBind();
                }
                else
                {
                    if (gv_data.Visible)
                        flow_Click(txt_m.Text, DropDownList1.SelectedValue, "C_LEAVE_HISTORY");//差(請)假
                    else if (GV_bussinesstrip.Visible)
                        flow_Click(txt_m.Text, DropDownList1.SelectedValue, "C_BUSSINESSTRIP_HISTORY");
                    else
                        flow_Click(txt_m.Text, DropDownList1.SelectedValue, "C_CHANGE_DLTB01_HISTORY");
                }
            }

            pl_overtime_history.Visible = false;

        }

        //選擇月份前先顯示現在的月份
        protected void DropDownList1_Init(object sender, EventArgs e)
        {
            if (txt_m.Text == "")
                txt_m.Text = (System.Convert.ToInt32(DateTime.Now.Year.ToString()) - 1911).ToString();

            string today = DateTime.Now.ToString("MM");
            DropDownList1.SelectedValue = today;
        }

        //個人加班lbtn
        protected void lbtn_overtime_self_Click(object sender, EventArgs e)
        {
            gv_OT.DataSource = OverTime_Self_Search();
            gv_OT.DataBind();
            lbl_sign.Text = "個人加班歷程";
            this.WHATS = "overtime";
            this.WHAT = "PERSON";
            allclose();
            pl_overtime_gv.Visible = true;
            drop.Visible = true;
            pl_self_item.Visible = true;
            //pl_data2.Visible = true;
            lbl_sign.Visible = true;
            gv_data.Visible = false;
            Page_update();
        }

        /*無用程式碼
        protected DataTable OverTimeAllSearch()
        {
            string strSQL = @"select C_OVERTIME_CHECKFLOW.*,C_OVERTIME_HOUR_INSIDE.SN,C_OVERTIME_HOUR_INSIDE.MZ_EXUNIT,
             C_OVERTIME_HOUR_INSIDE.MZ_ID,C_OVERTIME_HOUR_INSIDE.MZ_OCCC,C_OVERTIME_HOUR_INSIDE.OTIME,
             C_OVERTIME_HOUR_INSIDE.HOUR_PAY,C_OVERTIME_HOUR_INSIDE.PAY_SUM,C_OVERTIME_HOUR_INSIDE.OTREASON,
             C_OVERTIME_HOUR_INSIDE.INS_DATE,C_OVERTIME_HOUR_INSIDE.MZ_EXAD,C_OVERTIME_HOUR_INSIDE.MZ_DATE,
             C_OVERTIME_HOUR_INSIDE.MZ_RESTHOUR FROM C_OVERTIME_CHECKFLOW
             INNER JOIN C_OVERTIME_HOUR_INSIDE ON C_OVERTIME_CHECKFLOW.OVERTIME_SN = C_OVERTIME_HOUR_INSIDE.SN 
             WHERE  (C_OVERTIME_CHECKFLOW.PROCESS_STATUS ='2' OR C_OVERTIME_CHECKFLOW.PROCESS_STATUS ='1'OR C_OVERTIME_CHECKFLOW.PROCESS_STATUS ='4') AND
             dbo.SUBSTR(C_OVERTIME_HOUR_INSIDE.MZ_DATE,4,2)='" + DropDownList1.SelectedValue.ToString()
            + "' AND dbo.SUBSTR(C_OVERTIME_HOUR_INSIDE.MZ_DATE,1,3)='" + txt_m.Text.ToString()
            + "' ORDER BY C_OVERTIME_HOUR_INSIDE.MZ_DATE DESC";
            temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "allOverTime");
            return temp;
        }
        */

        /// <summary>
        /// 個人加班歷程
        /// </summary>
        protected DataTable OverTime_Self_Search()
        {
            string strSQL = @"select C_OVERTIME_HOUR_INSIDE.*,C_OVERTIME_HOUR_INSIDE.MZ_EXUNIT,
          C_OVERTIME_HOUR_INSIDE.MZ_ID,C_OVERTIME_HOUR_INSIDE.MZ_OCCC,C_OVERTIME_HOUR_INSIDE.OTIME,
          C_OVERTIME_HOUR_INSIDE.HOUR_PAY,C_OVERTIME_HOUR_INSIDE.PAY_SUM,C_OVERTIME_HOUR_INSIDE.OTREASON,
          C_OVERTIME_HOUR_INSIDE.INS_DATE,C_OVERTIME_HOUR_INSIDE.MZ_EXAD,C_OVERTIME_HOUR_INSIDE.MZ_DATE,
          C_OVERTIME_HOUR_INSIDE.MZ_RESTHOUR,C_OVERTIME.* FROM C_OVERTIME_HOUR_INSIDE
          INNER JOIN (SELECT OVERTIME_SN,MIN( SCHEDULE_SORT) AS SCHEDULE_SORT,
          MIN(O_SN) AS O_SN FROM C_OVERTIME_CHECKFLOW  GROUP BY OVERTIME_SN)  C_OVERTIME 
          ON C_OVERTIME_HOUR_INSIDE.SN = C_OVERTIME.OVERTIME_SN 
          WHERE 
          C_OVERTIME_HOUR_INSIDE.MZ_ID ='" + SessionHelper.ADPMZ_ID
         + "' AND dbo.SUBSTR(C_OVERTIME_HOUR_INSIDE.MZ_DATE,4,2)='" + DropDownList1.SelectedValue.ToString()
         + "' AND dbo.SUBSTR(C_OVERTIME_HOUR_INSIDE.MZ_DATE,1,3)='" + txt_m.Text.ToString()
         + "' ORDER BY C_OVERTIME_HOUR_INSIDE.MZ_DATE DESC";
            temp_SharedDT = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GJ");

            return temp_SharedDT;
        }

        /// <summary>
        /// 此人需審核加班費
        /// </summary>
        protected DataTable OverTime_Search()
        {
            string OverTimeSQL = @"select C_OVERTIME_CHECKFLOW.*,C_OVERTIME_HOUR_INSIDE.SN,C_OVERTIME_HOUR_INSIDE.MZ_EXUNIT,
             C_OVERTIME_HOUR_INSIDE.MZ_ID,C_OVERTIME_HOUR_INSIDE.MZ_OCCC,C_OVERTIME_HOUR_INSIDE.OTIME,
             C_OVERTIME_HOUR_INSIDE.HOUR_PAY,C_OVERTIME_HOUR_INSIDE.PAY_SUM,C_OVERTIME_HOUR_INSIDE.OTREASON,
             C_OVERTIME_HOUR_INSIDE.INS_DATE,C_OVERTIME_HOUR_INSIDE.MZ_EXAD,C_OVERTIME_HOUR_INSIDE.MZ_DATE,
             C_OVERTIME_HOUR_INSIDE.MZ_RESTHOUR FROM C_OVERTIME_CHECKFLOW 
             INNER JOIN C_OVERTIME_HOUR_INSIDE ON 
             C_OVERTIME_CHECKFLOW.OVERTIME_SN = C_OVERTIME_HOUR_INSIDE.SN 
             WHERE C_OVERTIME_CHECKFLOW.REVIEW_ID='" + SessionHelper.ADPMZ_ID
            + "' AND C_OVERTIME_CHECKFLOW. PROCESS_DATE IS NULL "
            + "AND SCHEDULE_SORT = 1 AND "
            + "(C_OVERTIME_HOUR_INSIDE.OVERTIME_STATUS != 1 OR C_OVERTIME_HOUR_INSIDE.OVERTIME_STATUS IS NULL ) "
            + "ORDER BY O_SN";

            temp_SharedDT = o_DBFactory.ABC_toTest.Create_Table(OverTimeSQL, "OverTime");
            return temp_SharedDT;
        }

        /// <summary>
        /// 此人需審核加班費筆數
        /// </summary>
        protected int OverTime_Search_Count()
        {
            string OverTimeSQL = @"select COUNT(*) FROM C_OVERTIME_CHECKFLOW 
             INNER JOIN C_OVERTIME_HOUR_INSIDE ON 
             C_OVERTIME_CHECKFLOW.OVERTIME_SN = C_OVERTIME_HOUR_INSIDE.SN 
             WHERE C_OVERTIME_CHECKFLOW.REVIEW_ID='" + SessionHelper.ADPMZ_ID
            + "' AND C_OVERTIME_CHECKFLOW. PROCESS_DATE IS NULL "
            + "AND SCHEDULE_SORT = 1 AND "
            + "(C_OVERTIME_HOUR_INSIDE.OVERTIME_STATUS != 1 OR C_OVERTIME_HOUR_INSIDE.OVERTIME_STATUS IS NULL ) "
            + "ORDER BY O_SN";


            return int.Parse(o_DBFactory.ABC_toTest.vExecSQL(OverTimeSQL));
        }

        /// <summary>
        /// 所有已決行待核定的加班費
        /// </summary>
        protected DataTable OverTime_PersonSearch()
        {
            strSQL = @"select C_OVERTIME_CHECKFLOW.*,C_OVERTIME_HOUR_INSIDE.SN,C_OVERTIME_HOUR_INSIDE.MZ_EXUNIT,C_OVERTIME_HOUR_INSIDE.MZ_ID,
                 C_OVERTIME_HOUR_INSIDE.MZ_OCCC,C_OVERTIME_HOUR_INSIDE.OTIME,C_OVERTIME_HOUR_INSIDE.HOUR_PAY,
                 C_OVERTIME_HOUR_INSIDE.PAY_SUM,C_OVERTIME_HOUR_INSIDE.OTREASON,C_OVERTIME_HOUR_INSIDE.INS_ID,
                 C_OVERTIME_HOUR_INSIDE.INS_DATE,C_OVERTIME_HOUR_INSIDE.MZ_EXAD,C_OVERTIME_HOUR_INSIDE.MZ_DATE,
                 C_OVERTIME_HOUR_INSIDE.MZ_RESTHOUR FROM C_OVERTIME_CHECKFLOW INNER JOIN C_OVERTIME_HOUR_INSIDE ON 
                 C_OVERTIME_CHECKFLOW.OVERTIME_SN = C_OVERTIME_HOUR_INSIDE.SN WHERE 
                 C_OVERTIME_HOUR_INSIDE.OVERTIME_STATUS = '2' AND C_OVERTIME_CHECKFLOW.PROCESS_STATUS='2' AND
                 C_OVERTIME_HOUR_INSIDE.MZ_EXAD='" + SessionHelper.ADPMZ_EXAD + "' AND "
                + " dbo.SUBSTR(C_OVERTIME_HOUR_INSIDE.MZ_IDATE,1,5)='" + (DateTime.Now.Year - 1911).ToString() + (DateTime.Now.Month).ToString().PadLeft(2, '0') + "' "
                + " ORDER BY C_OVERTIME_CHECKFLOW.O_SN DESC";


            temp_SharedDT = o_DBFactory.ABC_toTest.Create_Table(strSQL, "OverTime_Person_gv");
            return temp_SharedDT;
        }
        /// <summary>
        /// 修改加班歷程(O_SN,狀態(中文))
        /// </summary>
        protected void SQL_OverTime_UPDATE(int number, string status)
        {
            string year = (System.Convert.ToInt32(DateTime.Now.Year.ToString()) - 1911).ToString();
            string today = year + DateTime.Now.ToString("MMdd");
            string now = DateTime.Now.ToString("HH:mm:ss");

            string agreeSQL = @"UPDATE C_OVERTIME_CHECKFLOW SET 
                    PROCESS_DATE = @PROCESS_DATE,PROCESS_STATUS = @PROCESS_STATUS,
                    PROCESS_TIME = @PROCESS_TIME,REVIEW_MESSAGE = @REVIEW_MESSAGE 
                    WHERE O_SN=" + number;

            SqlParameter[] sp = {
                                    new SqlParameter("PROCESS_DATE",SqlDbType.VarChar){Value =  today  },
                                    new SqlParameter("PROCESS_STATUS",SqlDbType.VarChar){Value = Service_C_STATUS.Get_C_STATUS_SN(status)},
                                    new SqlParameter("PROCESS_TIME",SqlDbType.VarChar){Value =  now },
                                    new SqlParameter("REVIEW_MESSAGE",SqlDbType .VarChar ){Value = txt_opinion.Text.ToString()}
                                   };
            o_DBFactory.ABC_toTest.ExecuteNonQuery( agreeSQL, sp);

            //irk 2012/11/6 LOG增加
            LogModel lm = new LogModel("C");
            lm.sqlType = "U";
            string sql = lm.RegexSQL(agreeSQL, sp);
            lm.SqlHistory("修改加班歷程", sql, Request.QueryString["TPM_FION"]);

        }

        /// <summary>
        /// 修改此加班費的資料表狀態：退回or決行 (int假單SN，string狀態中文)
        /// </summary>
        protected void SQL_OverTime_Edit(int number, string status)
        {
            string OverTimeSQL = "UPDATE C_OVERTIME_HOUR_INSIDE SET "
                    + "OVERTIME_STATUS = @OVERTIME_STATUS WHERE SN =" + number;
            SqlParameter[] SP = {
                                   new SqlParameter ("OVERTIME_STATUS",SqlDbType.Float ){Value=Service_C_STATUS.Get_C_STATUS_SN( status )}
                                   };
            o_DBFactory.ABC_toTest.ExecuteNonQuery( OverTimeSQL, SP);

            //irk 2012/11/6 LOG增加
            LogModel lm = new LogModel("C");
            lm.sqlType = "U";
            string sql = lm.RegexSQL(OverTimeSQL, SP);
            lm.SqlHistory("修改此加班費的資料表狀態", sql, Request.QueryString["TPM_FION"]);
        }

        /// <summary>
        /// 加班-尋找同層的審核者
        /// </summary>
        protected DataTable select_name()
        {

            return OWService.Get_REVIEW_MANAGEMENT_forOnline_work(
                 SessionHelper.ADPMZ_EXAD
                 , SessionHelper.ADPMZ_EXUNIT
                 , 2
                 , SessionHelper.ADPMZ_ID
                 , false);
        }

        /// <summary>
        /// 修改加班歷程 //呈核的
        /// </summary>
        protected void SQL_OverTime_UPDATEa(int number, string status)
        {
            string year = (System.Convert.ToInt32(DateTime.Now.Year.ToString()) - 1911).ToString();
            string today = year + DateTime.Now.ToString("MMdd");
            string now = DateTime.Now.ToString("HH:mm:ss");

            string agreeSQL = @"UPDATE C_OVERTIME_CHECKFLOW SET 
                    PROCESS_DATE = @PROCESS_DATE,PROCESS_STATUS = @PROCESS_STATUS,
                    PROCESS_TIME = @PROCESS_TIME , REVIEW_MESSAGE = @REVIEW_MESSAGE 
                    WHERE O_SN=" + number;

            SqlParameter[] sp = {
                                    new SqlParameter("PROCESS_DATE",SqlDbType.VarChar){Value =  today  },
                                    new SqlParameter("PROCESS_STATUS",SqlDbType.VarChar){Value = Service_C_STATUS.Get_C_STATUS_SN(status)},
                                    new SqlParameter("PROCESS_TIME",SqlDbType.VarChar){Value =  now },
                                    new SqlParameter("REVIEW_MESSAGE",SqlDbType .VarChar ){Value = txt_opinion.Text.ToString()}

                                   };
            o_DBFactory.ABC_toTest.ExecuteNonQuery( agreeSQL, sp);

            //irk 2012/11/6 LOG增加
            LogModel lm = new LogModel("C");
            lm.sqlType = "U";
            string sql = lm.RegexSQL(agreeSQL, sp);
            lm.SqlHistory("修改加班歷程呈核", sql, Request.QueryString["TPM_FION"]);
        }

        /// <summary>
        /// 新增加班歷程(同層) (int歷程sn,string同層人ID)
        /// </summary>
        protected void SQL_OverTime_select_inster(int number, string reviewID)
        {
            string year = (System.Convert.ToInt32(DateTime.Now.Year.ToString()) - 1911).ToString();
            string today = year + DateTime.Now.ToString("MMdd");
            string now = DateTime.Now.ToString("HH:mm:ss");
            string insertSQL = "SELECT * FROM C_OVERTIME_CHECKFLOW WHERE O_SN=" + number;
            DataTable tempINSERT = new DataTable();

            tempINSERT = o_DBFactory.ABC_toTest.Create_Table(insertSQL, "INSERT");

            string reviewid = reviewID;

            string agreeSQL = @"INSERT INTO C_OVERTIME_CHECKFLOW (O_SN,OVERTIME_SN,REVIEW_ID,LETTER_DATE,
                LETTER_TIME,PROCESS_STATUS,SCHEDULE_SORT)VALUES( NEXT VALUE FOR dbo.C_OVERTIME_CHECKFLOW_SN,@OVERTIME_SN,@REVIEW_ID,
                @LETTER_DATE,@LETTER_TIME,@PROCESS_STATUS,@SCHEDULE_SORT)";

            SqlParameter[] sp = {
                                     new SqlParameter("OVERTIME_SN",SqlDbType.Float ){Value = int.Parse (tempINSERT.Rows[0]["OVERTIME_SN"].ToString ())},
                                     new SqlParameter("REVIEW_ID",SqlDbType.VarChar){Value =  reviewid},
                                     new SqlParameter("LETTER_DATE",SqlDbType.VarChar){Value = today  },
                                     new SqlParameter("LETTER_TIME",SqlDbType.VarChar){Value = now },
                                     new SqlParameter("PROCESS_STATUS",SqlDbType.VarChar){Value = Service_C_STATUS.Get_C_STATUS_SN("待審中")},
                                     new SqlParameter ("SCHEDULE_SORT",SqlDbType.Float){Value=int.Parse (tempINSERT.Rows[0]["SCHEDULE_SORT"].ToString ())}
                                    };
            o_DBFactory.ABC_toTest.ExecuteNonQuery( agreeSQL, sp);

            //irk 2012/11/6 LOG增加
            LogModel lm = new LogModel("C");
            lm.sqlType = "A";
            string sql = lm.RegexSQL(agreeSQL, sp);
            lm.SqlHistory("新增加班歷程", sql, Request.QueryString["TPM_FION"]);
        }
        /*
        /// <summary>
        /// 歷程 加班費
        /// </summary>
        protected DataTable OverTimeHistorySearch()
        {
            string OverTimeHistorySQL = @"SELECT C_OVERTIME_CHECKFLOW.*,C_OVERTIME_HOUR_INSIDE.MZ_EXUNIT,
                 C_OVERTIME_HOUR_INSIDE.MZ_ID,C_OVERTIME_HOUR_INSIDE.MZ_OCCC,C_OVERTIME_HOUR_INSIDE.OTIME,
                 C_OVERTIME_HOUR_INSIDE.HOUR_PAY,C_OVERTIME_HOUR_INSIDE.PAY_SUM,C_OVERTIME_HOUR_INSIDE.OTREASON,
                 C_OVERTIME_HOUR_INSIDE.INS_ID,C_OVERTIME_HOUR_INSIDE.INS_DATE,C_OVERTIME_HOUR_INSIDE.MZ_EXAD,
                 C_OVERTIME_HOUR_INSIDE.MZ_DATE,C_OVERTIME_HOUR_INSIDE.MZ_RESTHOUR,C_OVERTIME_HOUR_INSIDE.OVERTIME_STATUS,
                 C_OVERTIME_HOUR_INSIDE.SC_ID,C_STATUS.C_STATUS_NAME FROM C_OVERTIME_CHECKFLOW INNER JOIN C_OVERTIME_HOUR_INSIDE ON 
                 C_OVERTIME_CHECKFLOW.OVERTIME_SN=C_OVERTIME_HOUR_INSIDE.SN INNER JOIN C_STATUS ON 
                 C_OVERTIME_CHECKFLOW.PROCESS_STATUS = C_STATUS.C_STATUS_SN WHERE C_OVERTIME_CHECKFLOW.OVERTIME_SN="
                + int.Parse(ViewState["OverTime_History"].ToString()) + " AND C_OVERTIME_CHECKFLOW.PROCESS_DATE IS NOT NULL "
                + "ORDER BY C_OVERTIME_CHECKFLOW.O_SN";
            temp = o_DBFactory.ABC_toTest.Create_Table(OverTimeHistorySQL, "OverTimeHistory");
            return temp;
        }
        */

        //呈核
        protected void btn_overtime_sign_Click(object sender, EventArgs e)
        {
            DataTable tempSelect = new DataTable();
            tempSelect = select_name();


            int aCOUNT = 0;
            for (int p = 0; p < gv_OT.Rows.Count; p++)
            {
                CheckBox overtimeCK = (CheckBox)gv_OT.Rows[p].FindControl("ckb_Select_OT");
                if (overtimeCK.Checked == true)
                {

                    if (tempSelect.Rows.Count > 0)
                    {

                        gv_overtime_select.Visible = true;
                        btn_overtime_clear.Visible = true;
                        lbl_no_overtime.Visible = false;
                        Button3.Visible = false;
                        //ViewState["overtime_opinion"] = "sing";

                        gv_overtime_select.DataSource = tempSelect;
                        gv_overtime_select.DataBind();
                        aCOUNT = aCOUNT + 1;

                    }
                    else
                    {
                        lbl_no_overtime.Visible = true;
                        Button3.Visible = true;
                        gv_overtime_select.Visible = false;
                        btn_overtime_clear.Visible = false;
                        lbl_no_overtime.Text = "沒有同層主管，請審核。";
                        M_P_E_overtime_selcet.Show();
                        break;
                    }
                }

            }
            if (aCOUNT == 1)
            {
                M_P_E_overtime_selcet.Show();
            }
            else if (aCOUNT > 1)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "alert('陳核無法同時處理超過1筆加班資料')", true);
                return;
            }
        }

        //加班-審核者要呈核的時後，同層的審核者gv
        protected void gv_overtime_select_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Button MZ_ID = (Button)e.Row.FindControl("btn_overtime_select");
                Label mylbl = (Label)e.Row.FindControl("lbl_mz_name");
                Label occc = (Label)e.Row.FindControl("lbl_man");
                Label OCCC_ID = (Label)e.Row.FindControl("lbl_occc");
                // 取得目前資料列的資料
                DataRowView rowView = (DataRowView)e.Row.DataItem;
                mylbl.Text = rowView["MZ_NAME"].ToString();
                occc.Text = rowView["MZ_OCCC"].ToString();
                //mylbl.Text = o_A_DLBASE.CNAME(o_DBFactory.ABC_toTest.vExecSQL("select MZ_ID from C_REVIEW_MANAGEMENT where SN=" + int.Parse(SN.CommandArgument.ToString())));
                //mylbl.Text = o_A_DLBASE.CNAME(MZ_ID.CommandArgument.ToString());
                //occc.Text = o_DBFactory.ABC_toTest.vExecSQL("select RTRIM(MZ_KCHI) FROM A_KTYPE WHERE MZ_KTYPE ='26' AND MZ_KCODE ='" + OCCC_ID.Text.ToString().Trim() + "'");
            }

        }

        //呈核人選擇
        protected void btn_overtime_select_Command(object sender, CommandEventArgs e)
        {
            for (int i = 0; i < gv_OT.Rows.Count; i++)
            {
                #region 更新歷程
                #endregion

                CheckBox ckb_Select_OT = (CheckBox)gv_OT.Rows[i].FindControl("ckb_Select_OT");
                if (ckb_Select_OT.Checked)
                {
                    string MZID = e.CommandArgument.ToString();

                    DateTime nowDT = DateTime.Now;
                    //更新陳核單
                    string overtime_type = o_DBFactory.ABC_toTest.vExecSQL("select OVERTIME_TYPE from C_OVERTIME_BASE where SN = '" + gv_OT.DataKeys[i]["OVERTIME_SN"].ToStringNullSafe() + "'");
                    C_OVERTIME_HISTORY_Model upModel = new C_OVERTIME_HISTORY_Model()
                    {
                        O_SN = Convert.ToInt32(gv_OT.DataKeys[i]["O_SN"].ToStringNullSafe()),
                        OVERTIME_SN = Convert.ToInt32(gv_OT.DataKeys[i]["OVERTIME_SN"].ToStringNullSafe()),
                        PROCESS_DATE = ForDateTime.RCDateToTWDate(nowDT.ToString("yyyy/MM/dd"), "yyyMMdd"),
                        REVIEW_MESSAGE = "陳核",
                        PROCESS_TIME = nowDT.ToString("HH:mm:ss"),
                        PROCESS_STATUS = Service_C_STATUS.Get_C_STATUS_SN("陳核")
                    };

                    if (OWService.C_OVERTIME_HISTORY_Save(new List<C_OVERTIME_HISTORY_Model> { upModel }, _query.TPMFION))
                    {

                        DataTable temp_dt = o_DBFactory.ABC_toTest.Create_Table("SELECT LETTER_DATE,OVERTIME_SCHEDULE_SN FROM C_OVERTIME_HISTORY WHERE O_SN = '" + gv_OT.DataKeys[i]["O_SN"].ToStringNullSafe() + "'", "get");

                        string LETTER_DATE = temp_dt.Rows[0]["LETTER_DATE"].ToString();
                        string OVERTIME_SCHEDULE_SN = temp_dt.Rows[0]["OVERTIME_SCHEDULE_SN"].ToString();

                        C_OVERTIME_HISTORY_Model addModel = new C_OVERTIME_HISTORY_Model()
                        {
                            OVERTIME_SN = Convert.ToInt32(gv_OT.DataKeys[i]["OVERTIME_SN"].ToStringNullSafe()),
                            REVIEW_ID = MZID,
                            LETTER_DATE = LETTER_DATE,
                            OVERTIME_SCHEDULE_SN = Convert.ToInt32(OVERTIME_SCHEDULE_SN),
                            LETTER_TIME = DateTime.Now.ToString("HH:mm:ss"),
                            PROCESS_STATUS = Service_C_STATUS.Get_C_STATUS_SN("待審中"),
                            OVERTIME_TYPE = "OLWA",
                            SEND_ID = SessionHelper.ADPMZ_ID

                        };

                        if (OWService.C_OVERTIME_HISTORY_Save(addModel, _query.TPMFION))
                        {

                            C_OVERTIME_BASE_Model upCOBModel = new C_OVERTIME_BASE_Model()
                            {
                                OVERTIME_TYPE = overtime_type,
                                MZ_ID = gv_OT.DataKeys[i]["MZ_ID"].ToStringNullSafe(),
                                OVER_DAY = gv_OT.DataKeys[i]["COB_OVER_DAY"].ToStringNullSafe(),
                                REVIEW_ID = MZID,
                                LOCK_FLAG = "N",
                                IS_SIGN_RETURN = "N"
                            };

                            //取得加班時間(加班時間起訖、加班總時數)
                            CFService.GetOverTimeRange(ref upCOBModel);
                            //更新加班單
                            CFService.C_OVERTIME_BASE_Save(new List<C_OVERTIME_BASE_Model> { upCOBModel }, _query.TPMFION);
                            CFService.SynchronizeOverTime(upCOBModel, _query.TPMFION);

                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('已送出資料!');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('新增線上簽核失敗!');", true);
                        }



                    }
                }

            }
            //ViewState["rows_count"] = null;
            M_P_E_overtime_selcet.Hide();

            //重新回頁面
            btn_left_overtime_Click(sender, e);
            Page_update();
        }

        protected void btn_overtime_clear_Click(object sender, EventArgs e)
        {
            gv_overtime_select.DataSource = null;
            M_P_E_overtime_selcet.Hide();

        }

        protected void DDL_AD_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeUnit();
        }

        protected void btn_search_Click(object sender, EventArgs e)
        {
            ViewState["search"] = "y";
            DataTable tempMuch = new DataTable();
            if (gv_data.Visible)
            {
                if (this.MODE == Const_Mode.查詢簽核狀態)
                    flow_all_Click("C_LEAVE_HISTORY");
                else
                {
                    tempMuch = PersonSearch("C_LEAVE_HISTORY");
                    gv_data.DataSource = tempMuch;
                    this.gv_data_Person = tempMuch;
                    gv_data.DataBind();
                    gv_data.Visible = true;
                }

            }
            else if (GV_bussinesstrip.Visible)
            {
                if (this.MODE == Const_Mode.查詢簽核狀態)
                    flow_all_Click("C_BUSSINESSTRIP_HISTORY");
                else
                {
                    temp_SharedDT = PersonSearch("C_BUSSINESSTRIP_HISTORY");
                    GV_bussinesstrip.DataSource = temp_SharedDT;
                    GV_bussinesstrip.DataBind();
                    GV_bussinesstrip.Visible = true;
                }
            }
            else
            {
                if (this.MODE == Const_Mode.查詢簽核狀態)
                    flow_all_Click("C_CHANGE_DLTB01_HISTORY");
                else
                {
                    temp_SharedDT = PersonSearch("C_CHANGE_DLTB01_HISTORY");
                    gv_change_dltb01.DataSource = temp_SharedDT;
                    gv_change_dltb01.DataBind();
                    gv_change_dltb01.Visible = true;
                }
            }


            Page_update();
        }

        #endregion

        #region 新加班審核
        /// <summary>
        /// 連接按鈕: 左邊選單 加班線上簽核
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_left_overtime_Click(object sender, EventArgs e)
        {
            allclose();
            //開啟GridView區塊
            pl_overtime_gv.Visible = true;
            //開啟按鈕區塊
            pl_overtime.Visible = true;
            //設定資料
            _query.OVERTIME_TYPE = "OLWA";
            gv_OT.DataSource = OWService.GetReViewOverTimeBase(_query);
            gv_OT.DataBind();

            lbl_sign.Text = "加班線上簽核";
            lbl_sign.Visible = true;
        }
        /// <summary>
        /// 按鈕: 加班陳核單 退回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_overtime_no_Click(object sender, EventArgs e)
        {
            int c = 0;
            for (int i = 0; i < gv_OT.Rows.Count; i++)
            {
                CheckBox ckb_Select_OT = (CheckBox)gv_OT.Rows[i].FindControl("ckb_Select_OT");
                if (ckb_Select_OT.Checked)
                {
                    c++;
                }
            }

            if (c > 0)
            {
                //呼叫Dialog視窗
                pl_opinion.GroupingText = "退回原因";
                M_P_E_Opinion.Show();
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "alert('未選擇退回項目。')", true);
            }
        }
        /// <summary>
        /// 按鈕: 加班陳核單 決行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_overtime_yes_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gv_OT.Rows.Count; i++)
            {
                CheckBox ckb_Select_OT = (CheckBox)gv_OT.Rows[i].FindControl("ckb_Select_OT");
                if (ckb_Select_OT.Checked)
                {
                    DateTime nowDT = DateTime.Now;
                    //更新陳核單
                    string overtime_type = o_DBFactory.ABC_toTest.vExecSQL("select OVERTIME_TYPE from C_OVERTIME_BASE where SN = '" + gv_OT.DataKeys[i]["OVERTIME_SN"].ToStringNullSafe() + "'");
                    C_OVERTIME_HISTORY_Model upModel = new C_OVERTIME_HISTORY_Model()
                    {
                        O_SN = Convert.ToInt32(gv_OT.DataKeys[i]["O_SN"].ToStringNullSafe()),
                        OVERTIME_SN = Convert.ToInt32(gv_OT.DataKeys[i]["OVERTIME_SN"].ToStringNullSafe()),
                        PROCESS_DATE = ForDateTime.RCDateToTWDate(nowDT.ToString("yyyy/MM/dd"), "yyyMMdd"),
                        REVIEW_MESSAGE = "同意",
                        PROCESS_TIME = nowDT.ToString("HH:mm:ss"),
                        PROCESS_STATUS = Service_C_STATUS.Get_C_STATUS_SN("決行")
                    };

                    if (OWService.C_OVERTIME_HISTORY_Save(new List<C_OVERTIME_HISTORY_Model> { upModel }, _query.TPMFION))
                    {
                        C_OVERTIME_BASE_Model upCOBModel = new C_OVERTIME_BASE_Model()
                        {
                            OVERTIME_TYPE = overtime_type,
                            MZ_ID = gv_OT.DataKeys[i]["MZ_ID"].ToStringNullSafe(),
                            OVER_DAY = gv_OT.DataKeys[i]["COB_OVER_DAY"].ToStringNullSafe(),
                            REVIEW_ID = _query.PerformReviewID,
                            LOCK_FLAG = "N",
                            IS_SIGN_RETURN = "N"
                        };

                        //取得加班時間(加班時間起訖、加班總時數)
                        CFService.GetOverTimeRange(ref upCOBModel);
                        //更新加班單
                        CFService.C_OVERTIME_BASE_Save(new List<C_OVERTIME_BASE_Model> { upCOBModel }, _query.TPMFION);
                        CFService.SynchronizeOverTime(upCOBModel, _query.TPMFION);
                    }
                }
            }

            //重新回頁面
            btn_left_overtime_Click(sender, e);
            Page_update();
        }
        /// <summary>
        /// 連接按鈕: 左邊選單 已決行加班案件 加班承辦人(人事室)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_left_overtime_decision_Click(object sender, EventArgs e)
        {
            allclose();
            //開啟GridView區塊
            pl_overtime_gv.Visible = true;
            //開啟核定按鈕區塊
            pl_overtime_person.Visible = true;
            //設定資料
            _query.OVERTIME_TYPE = "OLWU";
            gv_OT.DataSource = OWService.GetReViewOverTimeBase(_query);
            gv_OT.DataBind();
            //開啟搜尋區塊
            pl_search.Visible = true;

            lbl_sign.Text = "已決行加班案件";
            lbl_sign.Visible = true;
        }
        /// <summary>
        /// 按鈕: 加班修改單 承辦人退回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_overtime_person_no_Click(object sender, EventArgs e)
        {
            int c = 0;
            for (int i = 0; i < gv_OT.Rows.Count; i++)
            {
                CheckBox ckb_Select_OT = (CheckBox)gv_OT.Rows[i].FindControl("ckb_Select_OT");
                if (ckb_Select_OT.Checked)
                {
                    c++;
                }
            }

            if (c > 0)
            {
                //呼叫Dialog視窗
                pl_opinion.GroupingText = "退回原因";
                M_P_E_Opinion.Show();
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "alert('未選擇退回項目。')", true);
            }
        }
        /// <summary>
        /// 按鈕: 加班修改單 承辦人核定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_overtime_person_yes_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gv_OT.Rows.Count; i++)
            {
                CheckBox ckb_Select_OT = (CheckBox)gv_OT.Rows[i].FindControl("ckb_Select_OT");
                if (ckb_Select_OT.Checked)
                {
                    DateTime nowDT = DateTime.Now;
                    //建立更新修改單資料
                    C_OVERTIME_HISTORY_Model upModel = new C_OVERTIME_HISTORY_Model()
                    {
                        O_SN = Convert.ToInt32(gv_OT.DataKeys[i]["O_SN"].ToStringNullSafe()),
                        OVERTIME_SN = Convert.ToInt32(gv_OT.DataKeys[i]["OVERTIME_SN"].ToStringNullSafe()),
                        PROCESS_DATE = ForDateTime.RCDateToTWDate(nowDT.ToString("yyyy/MM/dd"), "yyyMMdd"),
                        REVIEW_MESSAGE = "同意",
                        PROCESS_TIME = nowDT.ToString("HH:mm:ss"),
                        PROCESS_STATUS = Service_C_STATUS.Get_C_STATUS_SN("核定")
                    };

                    //更新修改單歷程
                    if (OWService.C_OVERTIME_HISTORY_Save(upModel, _query.TPMFION))
                    {   //加班類型
                        string strsql = "select OVERTIME_TYPE from C_OVERTIME_BASE where MZ_ID = '"
                            + gv_OT.DataKeys[i]["MZ_ID"].ToStringNullSafe()
                            + "' and OVER_DAY = '"
                            + gv_OT.DataKeys[i]["COB_OVER_DAY"].ToStringNullSafe()
                            + "'";
                        string strtype = o_DBFactory.ABC_toTest.vExecSQL(strsql);
                        //修改後的時數overtime_chg_total
                        strsql = "select overtime_chg_total from C_OVERTIME_BASE where MZ_ID = '"
                            + gv_OT.DataKeys[i]["MZ_ID"].ToStringNullSafe()
                            + "' and OVER_DAY = '"
                            + gv_OT.DataKeys[i]["COB_OVER_DAY"].ToStringNullSafe()
                            + "'";
                        string strchgtotal = o_DBFactory.ABC_toTest.vExecSQL(strsql);
                        int ichgtotal = 0;
                        if (strchgtotal.ToStringNullSafe() != "")
                        { ichgtotal = Convert.ToInt32(strchgtotal); }
                        //建立更新加班主檔資料
                        C_OVERTIME_BASE_Model upCOBModel = new C_OVERTIME_BASE_Model()
                        {
                            MZ_ID = gv_OT.DataKeys[i]["MZ_ID"].ToStringNullSafe(),
                            OVER_DAY = gv_OT.DataKeys[i]["COB_OVER_DAY"].ToStringNullSafe(),
                            OVERTIME_TYPE = strtype,
                            OVERTIME_CHG_ID = _query.PerformReviewID,
                            OVERTIME_CHG = "Y",
                            OVER_TOTAL = ichgtotal,
                            LOCK_FLAG = "N",
                            IS_SIGN_RETURN = "N"
                        };

                        //更新加班單
                        CFService.C_OVERTIME_BASE_Save(new List<C_OVERTIME_BASE_Model> { upCOBModel }, _query.TPMFION);
                        CFService.SynchronizeOverTime(upCOBModel, _query.TPMFION);
                    }
                }
            }

            //重新回頁面
            btn_left_overtime_decision_Click(sender, e);
            Page_update();
        }
        /// <summary>
        /// 按鈕: 退回填寫意見Dialog 確定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_opinion_yes_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gv_OT.Rows.Count; i++)
            {
                CheckBox ckb_Select_OT = (CheckBox)gv_OT.Rows[i].FindControl("ckb_Select_OT");
                if (ckb_Select_OT.Checked) //處理已選資料
                {
                    DateTime nowDT = DateTime.Now;
                    //更新陳核單
                    C_OVERTIME_HISTORY_Model upModel = new C_OVERTIME_HISTORY_Model()
                    {
                        O_SN = Convert.ToInt32(gv_OT.DataKeys[i]["O_SN"].ToStringNullSafe()),
                        OVERTIME_SN = Convert.ToInt32(gv_OT.DataKeys[i]["OVERTIME_SN"].ToStringNullSafe()),
                        PROCESS_DATE = ForDateTime.RCDateToTWDate(nowDT.ToString("yyyy/MM/dd"), "yyyMMdd"),
                        REVIEW_MESSAGE = string.IsNullOrEmpty(txt_opinion.Text) ? "退回" : txt_opinion.Text.ToStringNullSafe(),
                        PROCESS_TIME = nowDT.ToString("HH:mm:ss"),
                        PROCESS_STATUS = Service_C_STATUS.Get_C_STATUS_SN("退回")
                    };

                    if (OWService.C_OVERTIME_HISTORY_Save(new List<C_OVERTIME_HISTORY_Model> { upModel }, _query.TPMFION))
                    {
                        C_OVERTIME_BASE_Model upCOBModel = new C_OVERTIME_BASE_Model();
                        //加班申請單
                        if (_query.OVERTIME_TYPE == "OLWA")
                        {
                            upCOBModel.MZ_ID = gv_OT.DataKeys[i]["MZ_ID"].ToStringNullSafe();
                            upCOBModel.OVER_DAY = gv_OT.DataKeys[i]["COB_OVER_DAY"].ToStringNullSafe();
                            upCOBModel.IS_SIGN_RETURN = "Y";
                            upCOBModel.LOCK_FLAG = "N";
                        }
                        //加班修改單
                        else
                        {
                            upCOBModel.MZ_ID = gv_OT.DataKeys[i]["MZ_ID"].ToStringNullSafe();
                            upCOBModel.OVER_DAY = gv_OT.DataKeys[i]["COB_OVER_DAY"].ToStringNullSafe();
                            upCOBModel.OVERTIME_CHG = "N";
                            upCOBModel.OVERTIME_CHG_ID = _query.PerformReviewID;
                            upCOBModel.LOCK_FLAG = "N";
                            upCOBModel.IS_SIGN_RETURN = "Y";
                        }

                        //更新加班單狀態
                        CFService.C_OVERTIME_BASE_Save(upCOBModel, _query.TPMFION);
                        CFService.SynchronizeOverTime(upCOBModel, _query.TPMFION);
                    }
                }
            }
            M_P_E_Opinion.Hide();
            //重新回頁面
            if (_query.OVERTIME_TYPE == "OLWA")
            {
                btn_left_overtime_Click(sender, e);
            }
            else
            {
                btn_left_overtime_decision_Click(sender, e);
            }
            Page_update();
        }


        /// <summary>
        /// GridView: 各列加班簽核資料綁定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gv_OT_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView dr = (DataRowView)e.Row.DataItem;
                //取得附件數量
                C_OverTime_Base_Input_Query fQuery = new C_OverTime_Base_Input_Query() { SN = Convert.ToInt32(dr["OVERTIME_SN"].ToStringNullSafe()), TPMFION = _query.TPMFION };
                DataTable dt = CFService.GetOverTimeContextFile(fQuery);
                if (dt != null && dt.Rows.Count > 0)
                {
                    //動態新增連結按鈕
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        HyperLink link = new HyperLink();
                        link.NavigateUrl = WebConfigHelpers.C_UPLOAD_SHOWPATH + dt.Rows[i]["FILE_URL"].ToStringNullSafe();
                        link.Text = (i + 1).ToString() + ".";
                        link.Target = "_blank";    //線上簽核附件檔案分頁開啟
                        e.Row.Cells[8].Controls.Add(link);
                    }
                }
            }
        }
        /// <summary>
        /// GridView: 換頁操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gv_OT_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_OT.PageIndex = e.NewPageIndex;
            gv_OT.DataSource = OWService.GetReViewOverTimeBase(_query);
            gv_OT.DataBind();
            Page_update();

            if (pl_overtime_history.Visible)
            {
                pl_overtime_history.Visible = false;
            }
        }
        /// <summary>
        /// GridView: 列表中歷程按鈕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_history_ot_Command(object sender, CommandEventArgs e)
        {
            _query.OVERTIME_SN = e.CommandArgument.ToStringNullSafe();

            //設定歷程資料
            gv_OT_History.DataSource = OWService.GetOverTimeHistory(_query);
            gv_OT_History.DataBind();
            //因附件連結會消失，重新再Bind
            gv_OT.DataSource = OWService.GetReViewOverTimeBase(_query);
            gv_OT.DataBind();

            //開啟GridView區塊
            pl_overtime_history.Visible = true;
        }
        #endregion

        #region 差旅費
        protected void gv_bessiness_show()//產生GV
        {
            DataTable temp = new DataTable();
            CSonline = new CSonline("bussiness_search", SessionHelper.ADPMZ_ID);
            temp = CSonline.getDate();
            GV_bussinesstrip.DataSource = temp;
            GV_bussinesstrip.DataBind();
        }

        protected void GV_bussinesstrip_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GV_bussinesstrip.PageIndex = e.NewPageIndex;
            gv_bessiness_show();
        }

        protected void GV_bussinesstrip_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (this.MODE == Const_Mode.簽核狀態)
            {
                //DataTable tempPerson = new DataTable();
                //string strSQL = "select C_DLTB01.*,C_BUSSINESSTRIP_HISTORY.*,C_DLCODE.MZ_CNAME FROM C_DLTB01 INNER JOIN "
                // + "C_BUSSINESSTRIP_HISTORY ON C_DLTB01.MZ_DLTB01_SN = C_BUSSINESSTRIP_HISTORY.DLTB01_SN "
                // + "INNER JOIN C_DLCODE ON C_DLTB01.MZ_CODE = C_DLCODE.MZ_CODE "
                // + "WHERE C_DLTB01.MZ_ID='" + SessionHelper.ADPMZ_ID
                // + "'AND (C_BUSSINESSTRIP_HISTORY.PROCESS_STATUS ='7' OR C_BUSSINESSTRIP_HISTORY.PROCESS_STATUS ='1' OR C_BUSSINESSTRIP_HISTORY.PROCESS_STATUS ='4' OR C_BUSSINESSTRIP_HISTORY.PROCESS_STATUS ='6') order by C_BUSSINESSTRIP_HISTORY.SN";
                //tempPerson = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GJ");

                if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[0].Attributes.Add("style", "display:none");
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    LinkButton myhlink1 = (LinkButton)e.Row.FindControl("lbtn_time");

                    DataTable gv_bessiness_temp = o_DBFactory.ABC_toTest.Create_Table("select * from C_DLTB01 WHERE MZ_DLTB01_SN =" + myhlink1.CommandArgument, "A");

                    //請假的日期顯示
                    person_date = gv_bessiness_temp.Rows[0]["MZ_IDATE1"].ToString().Substring(0, 3) + "/" + gv_bessiness_temp.Rows[0]["MZ_IDATE1"].ToString().Substring(3, 2) + "/" + gv_bessiness_temp.Rows[0]["MZ_IDATE1"].ToString().Substring(5, 2);
                    string exad = o_A_KTYPE.RAD(e.Row.Cells[2].Text);
                    e.Row.Cells[2].Text = exad == "新北市政府警察局" ? "警察局" : exad.Replace("新北市政府警察局", "");
                    e.Row.Cells[3].Text = o_A_KTYPE.RUNIT(e.Row.Cells[3].Text);
                    e.Row.Cells[4].Text = gv_bessiness_temp.Rows[0]["MZ_NAME"].ToString();
                    myhlink1.Text = person_date.ToString();
                }

            }
            else if (this.MODE == Const_Mode.已核定假單)
            {
                DataTable temp = new DataTable();
                string strSQL = @"select C_DLTB01.*,C_BUSSINESSTRIP_HISTORY.*,C_DLCODE.MZ_CNAME FROM C_BUSSINESSTRIP_HISTORY 
                     INNER JOIN  C_DLTB01 ON C_BUSSINESSTRIP_HISTORY.DLTB01_SN= C_DLTB01.MZ_DLTB01_SN 
                     INNER JOIN C_DLCODE ON C_DLTB01.MZ_CODE = C_DLCODE.MZ_CODE 
                     WHERE C_BUSSINESSTRIP_HISTORY.PROCESS_STATUS ='7' order by C_BUSSINESSTRIP_HISTORY.SN DESC";
                temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GJ");
                if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[0].Attributes.Add("style", "display:none");
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    LinkButton myhlink1 = (LinkButton)e.Row.FindControl("lbtn_time");

                    DataTable gv_bessiness_temp = o_DBFactory.ABC_toTest.Create_Table("select * from C_DLTB01 WHERE MZ_DLTB01_SN =" + myhlink1.CommandArgument, "A");

                    //請假的日期顯示
                    person_date = gv_bessiness_temp.Rows[0]["MZ_IDATE1"].ToString().Substring(0, 3) + "/" + gv_bessiness_temp.Rows[0]["MZ_IDATE1"].ToString().Substring(3, 2) + "/" + gv_bessiness_temp.Rows[0]["MZ_IDATE1"].ToString().Substring(5, 2);
                    string exad = o_A_KTYPE.RAD(e.Row.Cells[2].Text);
                    e.Row.Cells[2].Text = exad == "新北市政府警察局" ? "警察局" : exad.Replace("新北市政府警察局", "");
                    e.Row.Cells[3].Text = o_A_KTYPE.RUNIT(e.Row.Cells[3].Text);
                    e.Row.Cells[4].Text = gv_bessiness_temp.Rows[0]["MZ_NAME"].ToString();
                    myhlink1.Text = person_date.ToString();
                }

            }
            else
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    LinkButton myhlink1 = (LinkButton)e.Row.FindControl("lbtn_time");//lbtn的內容與顯示
                    DataTable gv_bessiness_temp = o_DBFactory.ABC_toTest.Create_Table("select * from C_DLTB01 WHERE MZ_DLTB01_SN =" + myhlink1.CommandArgument, "A");
                    //請假的日期，顯示為 年/月/日
                    string exad = o_A_KTYPE.RAD(e.Row.Cells[2].Text);
                    e.Row.Cells[2].Text = exad == "新北市政府警察局" ? "警察局" : exad.Replace("新北市政府警察局", "");
                    e.Row.Cells[3].Text = o_A_KTYPE.RUNIT(e.Row.Cells[3].Text);
                    mz_date = gv_bessiness_temp.Rows[0]["MZ_IDATE1"].ToString().Substring(0, 3) + "/" + gv_bessiness_temp.Rows[0]["MZ_IDATE1"].ToString().Substring(3, 2) + "/" + gv_bessiness_temp.Rows[0]["MZ_IDATE1"].ToString().Substring(5, 2);
                    e.Row.Cells[4].Text = gv_bessiness_temp.Rows[0]["MZ_NAME"].ToString();
                    myhlink1.Text = mz_date.ToString();
                }
            }
        }

        protected DataTable content_Search(string MZ_DLTB01_SN, string tablename)//產生內容table
        {
            strSQL = "select C_DLTB01.*," + tablename + ".*,C_DLCODE.MZ_CNAME FROM " + tablename + " INNER JOIN "
                     + "C_DLTB01 ON " + tablename + ".DLTB01_SN= C_DLTB01.MZ_DLTB01_SN "
                     + "INNER JOIN C_DLCODE ON C_DLTB01.MZ_CODE = C_DLCODE.MZ_CODE "
                     + "WHERE"// C_BUSSINESSTRIP_HISTORY.REVIEW_ID='" + SessionHelper.ADPMZ_ID  AND'
                     + " C_DLTB01.MZ_DLTB01_SN ='" + MZ_DLTB01_SN + "'";
            temp_SharedDT = o_DBFactory.ABC_toTest.Create_Table(strSQL, "Show_gv");
            return temp_SharedDT;
        }

        protected void lbtn_b_content(object sender, CommandEventArgs e)//觀看差假內容的設定
        {
            pl_Show.GroupingText = "差旅費內容";
            DataTable tempShow = new DataTable();
            tempShow = content_Search((String)e.CommandArgument, "C_BUSSINESSTRIP_HISTORY");
            lbl_MZ_NAME.Text = tempShow.Rows[0]["MZ_NAME"].ToString();
            lbl_MZ_CNAME.Text = tempShow.Rows[0]["MZ_CNAME"].ToString();
            lbl_MZ_EXAD.Text = o_A_KTYPE.RAD(tempShow.Rows[0]["MZ_EXAD"].ToString());
            lbl_MZ_EXUNIT.Text = o_A_KTYPE.RUNIT(tempShow.Rows[0]["MZ_EXUNIT"].ToString());
            string Show_Date = temp_SharedDT.Rows[0]["MZ_IDATE1"].ToString() + temp_SharedDT.Rows[0]["MZ_ITIME1"] +
                    temp_SharedDT.Rows[0]["MZ_ODATE"].ToString() + temp_SharedDT.Rows[0]["MZ_OTIME"].ToString() +
                    temp_SharedDT.Rows[0]["MZ_TDAY"].ToString() + temp_SharedDT.Rows[0]["MZ_TTIME"].ToString();
            lbl_Date.Text = DateAndTime(temp_SharedDT);
            lbl_MZ_CAUSE.Text = temp_SharedDT.Rows[0]["MZ_CAUSE"].ToString();
            //0812→Dean
            string MZ_ID = tempShow.Rows[0]["MZ_ID"].ToString();

            DataTable temp_dt = o_DBFactory.ABC_toTest.Create_Table(" SELECT SUM(MZ_TDAY) MZ_TDAY,SUM(MZ_TTIME) MZ_TTIME FROM C_DLTB01 WHERE MZ_CODE='03' AND MZ_ID='" + MZ_ID + "' AND dbo.SUBSTR(MZ_IDATE1,1,3)='" + (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0') + "'", "get");
            int hday = 0;
            int htime = 0;
            if (temp_dt.Rows.Count > 0)
            {
                hday = string.IsNullOrEmpty(temp_dt.Rows[0]["MZ_TDAY"].ToString()) ? 0 : int.Parse(temp_dt.Rows[0]["MZ_TDAY"].ToString());

                htime = string.IsNullOrEmpty(temp_dt.Rows[0]["MZ_TTIME"].ToString()) ? 0 : int.Parse(temp_dt.Rows[0]["MZ_TTIME"].ToString());
            }


            int hday_used;

            int htime_used;

            if (htime != 0)
            {
                int hday_Add = htime / 8;
                hday_used = hday + hday_Add;
                htime_used = htime % 8;
            }
            else
            {
                hday_used = hday;
                htime_used = 0;
            }
            lbl_QKDay.Text = hday_used.ToString() + "天" + htime_used.ToString() + "時";//已休假天數
            lbl_agent.Text = tempShow.Rows[0]["MZ_RNAME"].ToString();//代理人
            lbl_location.Text = tempShow.Rows[0]["MZ_TADD"].ToString();//休假地點
            lbl_allowance.Text = temp_SharedDT.Rows[0]["MZ_SWT"].ToString() == "Y" ? "是" : "否";//是否有申請補助
            lbl_abroad.Text = temp_SharedDT.Rows[0]["MZ_FOREIGN"].ToString() == "Y" ? "出國、" : "" +
                              temp_SharedDT.Rows[0]["MZ_CHINA"].ToString() == "Y" ? "大陸、" : "";//是否為出國、赴大陸
            lbl_abroad.Text = lbl_abroad.Text == "" ? "否" : lbl_abroad.Text.Substring(0, lbl_abroad.Text.Length - 1);
            //顯示判定
            pl_history.Visible = false;
            pl_overtime_history.Visible = false;
            P_E_ModalPopupExtender.Show();

            gv_Business_Detail.Visible = true;
            pl_change_dltb01_show.Visible = false;

            string strSQL = string.Format(@"SELECT C_BUSSINESSTRIP.* FROM C_BUSSINESSTRIP WHERE MZ_DLTB01_SN={0} ORDER BY SN ", (String)e.CommandArgument);

            Session["gv_Business_Detail_dt"] = o_DBFactory.ABC_toTest.Create_Table(strSQL, "strSQL");

            gv_Business_Detail.DataSource = Session["gv_Business_Detail_dt"] as DataTable;

            gv_Business_Detail.DataBind();


        }

        protected void btn_b_back_Click(object sender, EventArgs e)//退回按鈕
        {
            //this.ViewStateClear();
            int count = GV_bussinesstrip.Rows.Count;
            for (int n = 0; n < GV_bussinesstrip.Rows.Count; n++)
            {
                CheckBox myCheckbox = (CheckBox)gv_data.Rows[n].FindControl("CB_select");
                if (myCheckbox.Checked == true)
                {
                    strSQL = @"UPDATE C_BUSSINESSTRIP_HISTORY SET 
                        PROCESS_DATE=@PROCESS_DATE,PROCESS_STATUS=@PROCESS_STATUS,
                        REVIEW_MESSAGE=@REVIEW_MESSAGE,PROCESS_TIME=@PROCESS_TIME 
                        WHERE SN =" + GV_bussinesstrip.DataKeys[n]["SN"];
                    SQL_UPDATE(n, btn_b_back.Text.ToString(), strSQL, false);
                }
            }
            DataTable tempMuch = new DataTable();
            CSonline = new CSonline("bussiness_search", SessionHelper.ADPMZ_ID);
            tempMuch = CSonline.getDate();


            if (tempMuch.Rows.Count == 0)
            {
                this.type = "Y";
            }
            GV_bussinesstrip.DataSource = tempMuch;
            GV_bussinesstrip.DataBind();
            GV_bussinesstrip.Visible = true;
            Page_update();
            if (this.type != "")
            {
                this.type = "";
                Response.Redirect("~/18-Online_Leave/Online_work.aspx?TPM_FION=" + _TPM_FION);
                this.ViewStateClear();
            }

            this.RSN_LENTH = count;
        }

        protected void btn_b_check_Click(object sender, EventArgs e)//決行按鈕
        {
            int count = GV_bussinesstrip.Rows.Count;

            for (int n = 0; n < GV_bussinesstrip.Rows.Count; n++)
            {
                CheckBox myCheckbox = (CheckBox)GV_bussinesstrip.Rows[n].FindControl("CB_select");
                if (myCheckbox.Checked == true)
                {
                    //20140616
                    //ViewState["SN" + n] = GV_bussinesstrip.DataKeys[n]["SN"];
                    strSQL = @"UPDATE C_BUSSINESSTRIP_HISTORY SET 
                    PROCESS_DATE = @PROCESS_DATE,PROCESS_STATUS = @PROCESS_STATUS,REVIEW_MESSAGE=@REVIEW_MESSAGE,
                    PROCESS_TIME = @PROCESS_TIME WHERE SN=" + GV_bussinesstrip.DataKeys[n]["SN"];
                    SQL_UPDATE(n, btn_b_check.Text.ToString(), strSQL, true);
                    BUSSINESSTRIP_STATUS(GV_bussinesstrip.DataKeys[n]["MZ_DLTB01_SN"].ToString());
                }
            }

            DataTable tempMuch = new DataTable();

            CSonline = new CSonline("bussiness_search", SessionHelper.ADPMZ_ID);
            tempMuch = CSonline.getDate();

            if (tempMuch.Rows.Count == 0)
            {
                this.type = "Y";
            }

            GV_bussinesstrip.DataSource = tempMuch;
            GV_bussinesstrip.DataBind();
            GV_bussinesstrip.Visible = true;
            Page_update();
            if (this.type != "")
            {
                this.type = "";
                Response.Redirect("~/18-Online_Leave/Online_work.aspx?TPM_FION=" + _TPM_FION);
                this.ViewStateClear();
            }

            this.RSN_LENTH = count;
        }

        void BUSSINESSTRIP_STATUS(string DLTB01_SN)
        {
            string updateString = "UPDATE C_BUSSINESSTRIP SET BUSSINESSTRIP_STATUS='Y' WHERE MZ_DLTB01_SN=@MZ_DLTB01_SN";

            SqlParameter[] paraList ={
                                         new SqlParameter("MZ_DLTB01_SN",SqlDbType.VarChar){Value =  DLTB01_SN }
                                        };
            o_DBFactory.ABC_toTest.ExecuteNonQuery( updateString, paraList);

            //irk 2012/11/6 LOG增加
            LogModel lm = new LogModel("C");
            lm.sqlType = "U";
            try
            {
                string sql = lm.RegexSQL(updateString, paraList);
                lm.SqlHistory("更改差旅狀態", sql, Request.QueryString["TPM_FION"]);
            }
            catch
            {
            }
        }

        /// <summary>
        /// 差旅費 陳核 按鈕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_b_sign_Click(object sender, EventArgs e)
        {
            //標記,目前需要的參數
            this.isUpper = false;


            string agreeSQL = string.Empty;
            int count = 0;
            string content = string.Empty;

            //20140613
            for (int a = 0; a < GV_bussinesstrip.Rows.Count; a++)
            {
                CheckBox agreeCK = (CheckBox)GV_bussinesstrip.Rows[a].FindControl("CB_select");
                if (agreeCK.Checked == true)
                {
                    //20140616
                    this.Set_RSN(count, GV_bussinesstrip.DataKeys[a]["SN"].ToStringNullSafe());

                    Reviewer(a, 0, GV_bussinesstrip, "C_BUSSINESSTRIP_HISTORY");

                    //設值 UI上方的 待簽核表單 個表單的勾選狀態 有標y的代表打勾
                    this.Set_upd(a, "y");
                    this.btn = btn_b_sign.Text.ToString();
                    count++;

                    if (content == null || content == "")
                    {
                        content = this.SCHEDULE;
                    }
                    if (content != this.SCHEDULE)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "alert('請確定下一筆流程都往同單位')", true);
                        return;
                    }
                }
            }



            if (count > 0)
            {
                GV_b_CHECK_show(EXAD, EXUNIT, false);
                //顧名思義判斷有無承辦人或主管
                if (this.np != "")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "alert('找不到該單位承辦人或單位主管')", true);
                    this.np = "";
                    return;
                }
                Panel_b_select_ModalPopupExtender.Show();//選人的panel
            }

            this.RSN_LENTH = count;
            Page_update();
        }

        //上層呈核
        protected void btn_b_upper_Click(object sender, EventArgs e)
        {
            //標記,目前需要的參數
            this.isUpper = true;

            string agreeSQL = string.Empty;
            int count = 0;
            string content = string.Empty;

            //20140613
            for (int a = 0; a < GV_bussinesstrip.Rows.Count; a++)
            {
                CheckBox agreeCK = (CheckBox)GV_bussinesstrip.Rows[a].FindControl("CB_select");
                if (agreeCK.Checked == true)
                {
                    //20140616
                    this.Set_RSN(count, GV_bussinesstrip.DataKeys[a]["SN"].ToStringNullSafe());

                    Reviewer(a, 1, GV_bussinesstrip, "C_BUSSINESSTRIP_HISTORY");

                    //設值 UI上方的 待簽核表單 個表單的勾選狀態 有標y的代表打勾
                    this.Set_upd(a, "y");
                    this.btn = btn_b_upper.Text.ToString();

                    count++;

                    if (content == null || content == "")
                    {
                        content = this.SCHEDULE;
                    }
                    if (content != this.SCHEDULE)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "alert('請確定下一筆流程都往同單位')", true);
                        return;
                    }
                }
            }



            GV_b_CHECK_show(EXAD, EXUNIT, true);
            //顧名思義判斷有無承辦人或主管
            if (this.np != "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "alert('找不到該單位承辦人或單位主管')", true);
                this.np = "";
                return;
            }
            //如果不能繼續呈核給上層、跳出
            if (this.re != "")
            {
                this.re = "";
                return;
            }

            if (count > 0)
            {
                GV_b_CHECK_show(EXAD, EXUNIT, true);
                //顧名思義判斷有無承辦人或主管
                int y = System.Convert.ToInt32(o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM c_review_management,a_dlbase WHERE A_DLBASE.MZ_ID = C_REVIEW_MANAGEMENT.MZ_ID" +
                                            " AND C_REVIEW_MANAGEMENT.MZ_EXAD = '" + EXAD + "' AND C_REVIEW_MANAGEMENT.MZ_EXUNIT = '" + EXUNIT +
                                            "' AND C_REVIEW_MANAGEMENT.REVIEW_LEVEL = '1'"));
                if (this.np != "")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "alert('找不到該單位承辦人或單位主管')", true);
                    this.np = "";
                    return;
                }
                if (y == 1)
                {
                    for (int a = 0; a < GV_bussinesstrip.Rows.Count; a++)
                    {
                        if (this.Get_upd(a) != "")
                        {
                            strSQL = @"UPDATE C_BUSSINESSTRIP_HISTORY SET 
                                 PROCESS_DATE = @PROCESS_DATE,PROCESS_STATUS = @PROCESS_STATUS,
                                 PROCESS_TIME = @PROCESS_TIME WHERE SN=" + GV_bussinesstrip.DataKeys[a]["SN"];
                            SQL_UPDATE(a, this.btn, strSQL, true);
                            this.Set_upd(a, "");
                        }
                    }
                    this.btn = "";//清除目前記錄按了哪顆按鈕
                    for (int i = 0; i < count; ++i)
                    {
                        string temp = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_ID FROM c_review_management WHERE " +
                                            " MZ_EXAD = '" + EXAD + "' AND MZ_EXUNIT = '" + EXUNIT +
                                            "' AND REVIEW_LEVEL = '1'");
                        string year = (System.Convert.ToInt32(DateTime.Now.Year.ToString()) - 1911).ToString();
                        string today = year + DateTime.Now.ToString("MMdd");
                        string now = DateTime.Now.ToString("HH:mm:ss");

                        string strSQL = @"INSERT INTO C_BUSSINESSTRIP_HISTORY
                                                                       (SN,LEAVE_SN,REVIEW_ID,LETTER_DATE,LETTER_TIME,LEAVE_SCHEDULE_SN,DLTB01_SN,PROCESS_STATUS,MZ_MID)
                                                                  VALUES
                                                                       ( NEXT VALUE FOR dbo.C_BUSSINESSTRIP_HISTORY_SN,@LEAVE_SN,@REVIEW_ID,@LETTER_DATE,@LETTER_TIME,@LEAVE_SCHEDULE_SN,@DLTB01_SN,@PROCESS_STATUS,@MZ_MID)";

                        DataTable temp_dt = o_DBFactory.ABC_toTest.Create_Table("SELECT LEAVE_SN , DLTB01_SN FROM C_BUSSINESSTRIP_HISTORY WHERE SN = '" + this.Get_RSN(i) + "'", "get");

                        string LEAVE_SN = temp_dt.Rows[0]["LEAVE_SN"].ToString();
                        string DLTB01_SN = temp_dt.Rows[0]["DLTB01_SN"].ToString();

                        SqlParameter[] parameterList = {
                    new SqlParameter("LEAVE_SN",SqlDbType.VarChar){Value = LEAVE_SN},
                    new SqlParameter("REVIEW_ID",SqlDbType.VarChar){Value = temp},
                    new SqlParameter("LETTER_DATE",SqlDbType.VarChar){Value = today},
                    new SqlParameter("LEAVE_SCHEDULE_SN",SqlDbType.VarChar){Value =this.SORT},
                    new SqlParameter("LETTER_TIME",SqlDbType.VarChar){Value = now},
                    new SqlParameter("DLTB01_SN",SqlDbType.Float){Value = DLTB01_SN},
                    new SqlParameter("PROCESS_STATUS",SqlDbType.Char){Value = '4'},
                    new SqlParameter("MZ_MID",SqlDbType.VarChar){Value =  SessionHelper.ADPMZ_ID}
                                                  };

                        o_DBFactory.ABC_toTest.ExecuteNonQuery( strSQL, parameterList);

                        //irk 2012/11/6 LOG增加
                        LogModel lm = new LogModel("C");
                        lm.sqlType = "A";
                        string sql = lm.RegexSQL(strSQL, parameterList);
                        lm.SqlHistory("新增差旅歷程", sql, Request.QueryString["TPM_FION"]);
                    }
                    //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('資料已送出')", true);
                    this.s = "";
                    DataTable tempMuch = new DataTable();
                    CSonline = new CSonline("bussiness_search", SessionHelper.ADPMZ_ID);
                    tempMuch = CSonline.getDate();
                    GV_bussinesstrip.DataSource = tempMuch;
                    GV_bussinesstrip.DataBind();
                    GV_bussinesstrip.Visible = true;
                    if (this.type != "")
                    {
                        this.type = "";
                        Response.Redirect("~/18-Online_Leave/Online_work.aspx?TPM_FION=" + _TPM_FION);
                        this.ViewStateClear();
                    }
                }
                else
                {
                    Panel_b_select_ModalPopupExtender.Show();//選人的panel
                }
            }
            this.RSN_LENTH = count;
            pl_b_history.Visible = false;
            Page_update();
        }

        // 抽回按鈕
        protected void btn_b_re_Click(object sender, EventArgs e)
        {
            for (int a = 0; a < GV_bussinesstrip.Rows.Count; a++)
            {
                CheckBox agreeCK = (CheckBox)GV_bussinesstrip.Rows[a].FindControl("CB_select");
                if (agreeCK.Checked == true)
                {
                    string sn = GV_bussinesstrip.DataKeys[a]["MZ_DLTB01_SN"].ToString();
                    int s = System.Convert.ToInt32(GV_bussinesstrip.DataKeys[a]["LEAVE_SCHEDULE_SN"].ToString());

                    string sn2 = o_DBFactory.ABC_toTest.vExecSQL(string.Format(@"select sn from 
                                                    (select rownum as row_id ,sn  from 
                                                    (select * from c_bussinesstrip_history where dltb01_sn='{0}'  order by sn desc)) 
                                                    where row_id=2", GV_bussinesstrip.DataKeys[a]["MZ_DLTB01_SN"].ToString()));

                    string upd = string.Format(@"UPDATE C_BUSSINESSTRIP_HISTORY SET  PROCESS_DATE = '' ,PROCESS_STATUS = '{0}',PROCESS_TIME = ''  WHERE SN={1}", 4, sn2);
                    strSQL = "Delete FROM C_BUSSINESSTRIP_HISTORY WHERE SN = " + GV_bussinesstrip.DataKeys[a]["SN"].ToString();
                    o_DBFactory.ABC_toTest.ExecuteNonQuery( upd);
                    o_DBFactory.ABC_toTest.ExecuteNonQuery( strSQL);

                    //irk 2012/11/6 LOG增加
                    LogModel lm = new LogModel("C");
                    lm.sqlType = "U";
                    try
                    {
                        string sql = lm.RegexSQL(upd, new List<SqlParameter>());
                        lm.SqlHistory("抽回差旅簽核", sql, Request.QueryString["TPM_FION"]);
                    }
                    catch
                    {
                    }

                    //irk 2012/11/6 LOG增加
                    lm = new LogModel("C");
                    lm.sqlType = "U";
                    try
                    {
                        string sql = lm.RegexSQL(strSQL, new List<SqlParameter>());
                        lm.SqlHistory("刪除差旅歷程", sql, Request.QueryString["TPM_FION"]);
                    }
                    catch
                    {
                    }
                }
            }
            DataTable temp = new DataTable();

            DataTable bre_table = new DataTable();

            CSonline = new CSonline("bresearch", SessionHelper.ADPMZ_ID);

            temp = CSonline.getDate();

            GV_bussinesstrip.DataSource = temp;
            GV_bussinesstrip.DataBind();
            GV_bussinesstrip.Visible = true;
            Page_update();
        }

        protected void SQL_UPDATE(int number, string status, string strSQL, bool check)
        {
            string year = (System.Convert.ToInt32(DateTime.Now.Year.ToString()) - 1911).ToString();
            string today = year + DateTime.Now.ToString("MMdd");
            string now = DateTime.Now.ToString("HH:mm:ss");

            SqlParameter[] sp = {
                                    new SqlParameter("PROCESS_DATE",SqlDbType.VarChar){Value =  today  },
                                    new SqlParameter("PROCESS_STATUS",SqlDbType.VarChar){Value = Service_C_STATUS.Get_C_STATUS_SN(status)},
                                    new SqlParameter("PROCESS_TIME",SqlDbType.VarChar){Value =  now },
                                    new SqlParameter("REVIEW_MESSAGE",SqlDbType.VarChar){Value =  this.txt_message_Text==""?(check?"同意":"退回"):this.txt_message_Text  }
                                    };
            o_DBFactory.ABC_toTest.ExecuteNonQuery( strSQL, sp);
            //新增新的排程

            //irk 2012/11/6 LOG增加
            LogModel lm = new LogModel("C");
            lm.sqlType = "A";
            string sql = lm.RegexSQL(strSQL, sp);
            lm.SqlHistory("新增歷程", sql, Request.QueryString["TPM_FION"]);
        }



        /// <summary>
        /// 往通過下一層 新增歷程
        /// </summary>
        /// <param name="number">GV的第幾個</param>
        protected void SQL_INSERT(int number)
        {
            //0815→Dean
            string year = (System.Convert.ToInt32(DateTime.Now.Year.ToString()) - 1911).ToString();
            string today = year + DateTime.Now.ToString("MMdd");
            string now = DateTime.Now.ToString("HH:mm:ss");
            string insertSQL = "SELECT SN,MZ_MID,LEAVE_SN,DLTB01_SN FROM C_LEAVE_HISTORY WHERE SN=" + gv_data.DataKeys[number]["SN"];
            DataTable tempINSERT = new DataTable();
            try
            {
                tempINSERT = o_DBFactory.ABC_toTest.Create_Table(insertSQL, "INSERT");
            }
            catch (Exception ex)
            {
                LogModel lm = new LogModel("C");
                lm.SqlHistory("error新增歷程-0", insertSQL + ";--" + ex.Message, Request.QueryString["TPM_FION"]);

            }

            //string x = (System.Convert.ToInt32(o_DBFactory.ABC_toTest.vExecSQL("select C_LEAVE_SCHEDULE.SORT_NO FROM C_LEAVE_HISTORY INNER JOIN C_LEAVE_SCHEDULE ON C_LEAVE_SCHEDULE.SORT_NO = C_LEAVE_HISTORY.LEAVE_SCHEDULE_SN WHERE C_LEAVE_HISTORY.SN =" + tempINSERT.Rows[0]["SN"])) + 1).ToString();
            string SELECT_SORTNO_SQL = "select C_LEAVE_SCHEDULE.SORT_NO+1 FROM C_LEAVE_HISTORY INNER JOIN C_LEAVE_SCHEDULE ON C_LEAVE_SCHEDULE.SORT_NO = C_LEAVE_HISTORY.LEAVE_SCHEDULE_SN WHERE C_LEAVE_HISTORY.SN =" + tempINSERT.Rows[0]["SN"];
            string x = "";
            try
            {
                x = o_DBFactory.ABC_toTest.vExecSQL(SELECT_SORTNO_SQL);
            }
            catch (Exception ex)
            {
                LogModel lm = new LogModel("C");
                lm.SqlHistory("error新增歷程-0", insertSQL + ";--" + ex.Message, Request.QueryString["TPM_FION"]);

            }
            string reviewid = tempINSERT.Rows[0]["MZ_MID"].ToString();

            string agreeSQL = @"INSERT INTO C_LEAVE_HISTORY ( 
                        SN,LEAVE_SN,REVIEW_ID,LETTER_DATE,PROCESS_STATUS,LEAVE_SCHEDULE_SN,
                        DLTB01_SN,LETTER_TIME,MZ_MID)
                        VALUES(
                         NEXT VALUE FOR dbo.C_LEAVE_HISTORY_SN,@LEAVE_SN,@REVIEW_ID,@LETTER_DATE,@PROCESS_STATUS,@LEAVE_SCHEDULE_SN,
                        @DLTB01_SN,@LETTER_TIME,@MZ_MID)";



            SqlParameter[] sp = {
                                     //new SqlParameter("SN",SqlDbType.VarChar){Value = SN},
                                     new SqlParameter("LEAVE_SN",SqlDbType.VarChar){Value =tempINSERT.Rows[0]["LEAVE_SN"].ToString ()},
                                     new SqlParameter("REVIEW_ID",SqlDbType.VarChar){Value =  reviewid},
                                     new SqlParameter("LETTER_DATE",SqlDbType.VarChar){Value = today  },
                                     new SqlParameter("PROCESS_STATUS",SqlDbType.Char){Value = Service_C_STATUS.Get_C_STATUS_SN("待審中") },
                                     new SqlParameter("LEAVE_SCHEDULE_SN",SqlDbType.VarChar){Value = x },
                                     new SqlParameter("DLTB01_SN",SqlDbType.Float){Value =tempINSERT.Rows[0]["DLTB01_SN"]},
                                     new SqlParameter("LETTER_TIME",SqlDbType.VarChar){Value = now },
                                     new SqlParameter("MZ_MID",SqlDbType.VarChar){Value =  SessionHelper.ADPMZ_ID}
                                    };
            try
            {
                o_DBFactory.ABC_toTest.ExecuteNonQuery( agreeSQL, sp);

                //irk 2012/11/6 LOG增加
                LogModel lm = new LogModel("C");
                lm.sqlType = "A";
                string sql = lm.RegexSQL(agreeSQL, sp);
                lm.SqlHistory("新增歷程", sql, Request.QueryString["TPM_FION"]);
            }
            catch (Exception ex)
            {
                LogModel lm = new LogModel("C");
                lm.sqlType = "A";
                string sql = lm.RegexSQL(agreeSQL, sp);
                lm.SqlHistory("error新增歷程-9", sql + ";--" + ex.Message, Request.QueryString["TPM_FION"]);

            }


        }


        protected void SQL_INSERT(int number, string tablename)
        {
            //0815→Dean
            string year = (System.Convert.ToInt32(DateTime.Now.Year.ToString()) - 1911).ToString();
            string today = year + DateTime.Now.ToString("MMdd");
            string now = DateTime.Now.ToString("HH:mm:ss");
            string insertSQL = "SELECT SN,MZ_MID,LEAVE_SN,DLTB01_SN FROM " + tablename + " WHERE SN=" + gv_data.DataKeys[number]["SN"];
            DataTable tempINSERT = new DataTable();
            try
            {
                tempINSERT = o_DBFactory.ABC_toTest.Create_Table(insertSQL, "INSERT");
            }
            catch (Exception ex)
            {
                LogModel lm = new LogModel("C");

                lm.SqlHistory("error新增歷程X--0", insertSQL + ";--" + ex.Message, Request.QueryString["TPM_FION"]);

            }

            string x = "";
            string SORT_NO_SQL = "select C_LEAVE_SCHEDULE.SORT_NO+1 FROM " + tablename + " INNER JOIN C_LEAVE_SCHEDULE ON C_LEAVE_SCHEDULE.SORT_NO = " + tablename + ".LEAVE_SCHEDULE_SN WHERE C_LEAVE_HISTORY.SN =" + tempINSERT.Rows[0]["SN"];
            try
            {
                x = o_DBFactory.ABC_toTest.vExecSQL(SORT_NO_SQL);
            }
            catch (Exception ex)
            {
                LogModel lm = new LogModel("C");

                lm.SqlHistory("error新增歷程X--1", SORT_NO_SQL + ";--" + ex.Message, Request.QueryString["TPM_FION"]);

            }
            string reviewid = tempINSERT.Rows[0]["MZ_MID"].ToString();

            string agreeSQL = "INSERT INTO " + tablename + @" ( 
                        SN,LEAVE_SN,REVIEW_ID,LETTER_DATE,PROCESS_STATUS,LEAVE_SCHEDULE_SN,
                        DLTB01_SN,LETTER_TIME,MZ_MID)
                        VALUES(
                         NEXT VALUE FOR dbo.C_BUSSINESSTRIP_HISTORY_SN,@LEAVE_SN,@REVIEW_ID,@LETTER_DATE,@PROCESS_STATUS,@LEAVE_SCHEDULE_SN,
                        @DLTB01_SN,@LETTER_TIME,@MZ_MID)";

            SqlParameter[] sp = {
                                     new SqlParameter("LEAVE_SN",SqlDbType.VarChar){Value =tempINSERT.Rows[0]["LEAVE_SN"].ToString ()},
                                     new SqlParameter("REVIEW_ID",SqlDbType.VarChar){Value =  reviewid},
                                     new SqlParameter("LETTER_DATE",SqlDbType.VarChar){Value = today  },
                                     new SqlParameter("PROCESS_STATUS",SqlDbType.Char){Value = Service_C_STATUS.Get_C_STATUS_SN("待審中") },
                                     new SqlParameter("LEAVE_SCHEDULE_SN",SqlDbType.VarChar){Value = x },
                                     new SqlParameter("DLTB01_SN",SqlDbType.Float){Value =tempINSERT.Rows[0]["DLTB01_SN"]},
                                     new SqlParameter("LETTER_TIME",SqlDbType.VarChar){Value = now },
                                     new SqlParameter("MZ_MID",SqlDbType.VarChar){Value =  SessionHelper.ADPMZ_ID}
                                    };
            //20141022
            try
            {
                o_DBFactory.ABC_toTest.ExecuteNonQuery( agreeSQL, sp);

                //irk 2012/11/6 LOG增加
                LogModel lm = new LogModel("C");
                lm.sqlType = "A";
                string sql = lm.RegexSQL(agreeSQL, sp);
                lm.SqlHistory("新增歷程", sql, Request.QueryString["TPM_FION"]);
            }
            catch (Exception ex)
            {
                LogModel lm = new LogModel("C");
                lm.sqlType = "A";
                string sql = lm.RegexSQL(agreeSQL, sp);
                lm.SqlHistory("error新增歷程X1", sql + ";--" + ex.Message, Request.QueryString["TPM_FION"]);

            }
        }

        protected void SQL_INSERTOK(int number)
        {
            //0815→Dean
            string year = (System.Convert.ToInt32(DateTime.Now.Year.ToString()) - 1911).ToString();
            string today = year + DateTime.Now.ToString("MMdd");
            string now = DateTime.Now.ToString("HH:mm:ss");
            string insertSQL = "SELECT SN,LEAVE_SN,DLTB01_SN FROM C_LEAVE_HISTORY WHERE SN=" + gv_data.DataKeys[number]["SN"];
            DataTable tempINSERT = new DataTable();
            tempINSERT = o_DBFactory.ABC_toTest.Create_Table(insertSQL, "INSERT");
            string x = "";
            string SORT_NO_SQL = @"select C_LEAVE_SCHEDULE.SORT_NO+1 FROM C_LEAVE_HISTORY 
                                INNER JOIN C_LEAVE_SCHEDULE ON C_LEAVE_SCHEDULE.SORT_NO = C_LEAVE_HISTORY.LEAVE_SCHEDULE_SN 
                                WHERE C_LEAVE_HISTORY.SN =" + tempINSERT.Rows[0]["SN"];
            try
            {
                x = o_DBFactory.ABC_toTest.vExecSQL(SORT_NO_SQL);
            }

            catch (Exception ex)
            {
                LogModel lm = new LogModel("C");

                lm.SqlHistory("error新增歷程X2--1", SORT_NO_SQL + ";--" + ex.Message, Request.QueryString["TPM_FION"]);

            }
            string ad = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_EXAD FROM A_DLBASE WHERE MZ_ID = '" + SessionHelper.ADPMZ_ID + "'");
            //string reviewid = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_ID FROM C_REVIEW_MANAGEMENT WHERE MZ_EXAD = '" + ad + "' AND MZ_EXUNIT = '0001' AND REVIEW_LEVEL = '4'");
            string reviewid = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_ID FROM C_REVIEW_MANAGEMENT WHERE MZ_EXAD = '" + ad + "' AND (MZ_EXUNIT = '0001' OR MZ_EXUNIT = 'PAY0' OR MZ_EXUNIT='PAZ0') AND REVIEW_LEVEL = '4'");

            string agreeSQL = @"INSERT INTO C_LEAVE_HISTORY ( 
                        SN,LEAVE_SN,REVIEW_ID,LETTER_DATE,PROCESS_STATUS,LEAVE_SCHEDULE_SN,
                        DLTB01_SN,LETTER_TIME,MZ_MID)
                        VALUES(
                         NEXT VALUE FOR dbo.C_LEAVE_HISTORY_SN,@LEAVE_SN,@REVIEW_ID,@LETTER_DATE,@PROCESS_STATUS,@LEAVE_SCHEDULE_SN,
                        @DLTB01_SN,@LETTER_TIME,@MZ_MID)";



            SqlParameter[] sp = {
                                     //new SqlParameter("SN",SqlDbType.VarChar){Value = SN},
                                     new SqlParameter("LEAVE_SN",SqlDbType.VarChar){Value =tempINSERT.Rows[0]["LEAVE_SN"].ToString ()},
                                     new SqlParameter("REVIEW_ID",SqlDbType.VarChar){Value =  reviewid},
                                     new SqlParameter("LETTER_DATE",SqlDbType.VarChar){Value = today  },
                                     new SqlParameter("PROCESS_STATUS",SqlDbType.Char){Value = Service_C_STATUS.Get_C_STATUS_SN("待審中") },
                                     new SqlParameter("LEAVE_SCHEDULE_SN",SqlDbType.Float){Value = 0 },
                                     new SqlParameter("DLTB01_SN",SqlDbType.Float){Value =tempINSERT.Rows[0]["DLTB01_SN"]},
                                     new SqlParameter("LETTER_TIME",SqlDbType.VarChar){Value = now },
                                     new SqlParameter("MZ_MID",SqlDbType.VarChar){Value =  SessionHelper.ADPMZ_ID}
                                    };
            try
            {
                o_DBFactory.ABC_toTest.ExecuteNonQuery( agreeSQL, sp);

                //irk 2012/11/6 LOG增加
                LogModel lm = new LogModel("C");
                lm.sqlType = "A";
                string sql = lm.RegexSQL(agreeSQL, sp);
                lm.SqlHistory("新增歷程", sql, Request.QueryString["TPM_FION"]);
            }
            catch (Exception ex)
            {
                LogModel lm = new LogModel("C");
                lm.sqlType = "A";
                string sql = lm.RegexSQL(agreeSQL, sp);
                lm.SqlHistory("error新增歷程X2", sql + ";--" + ex.Message, Request.QueryString["TPM_FION"]);
            }
        }


        protected void btn_b_ck_Click(object sender, EventArgs e)//內容確定按鈕
        {
            this.txt_message_Text = txt_b_message.Text;


            panel_b_message_pop.Hide();
        }


        //下一個簽核者的GV
        protected void GV_b_CHECK_show(string exad, string exunit, bool isToUpper)
        {
            DataTable temp = new DataTable();
            temp = OWService.Get_REVIEW_MANAGEMENT_forOnline_work(exad, exunit, 0, SessionHelper.ADPMZ_ID, isToUpper);
            // o_DBFactory.ABC_toTest.Create_Table("SELECT c_review_management.sn,CASE WHEN REVIEW_LEVEL='1' THEN '承辦人' ELSE (SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='26' AND MZ_KCODE=C_REVIEW_MANAGEMENT.MZ_OCCC) END AS MZ_OCCC,a_dlbase.mz_name FROM c_review_management,a_dlbase WHERE A_DLBASE.MZ_ID = C_REVIEW_MANAGEMENT.MZ_ID AND C_REVIEW_MANAGEMENT.MZ_EXAD = '" + exad + "' AND C_REVIEW_MANAGEMENT.MZ_EXUNIT = '" + exunit + "'", "get");
            int x = temp.Rows.Count;
            GV_b_CHECKER.DataSource = temp;
            GV_b_CHECKER.DataBind();
            this.s = "y";
            if (x == 0)
            {
                this.np = "y";
            }
        }

        protected void GV_b_CHECKER_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //0815→Dean
            if (e.CommandName == "checker")
            {
                for (int a = 0; a < GV_bussinesstrip.Rows.Count; a++)
                {
                    if (this.Get_upd(a) != "")
                    {
                        SQL_UPDATE_XXX_HISTORY(a, this.btn, GV_bussinesstrip, "C_BUSSINESSTRIP_HISTORY", true);
                        this.Set_upd(a, "");
                    }
                }
                this.btn = "";//清除目前記錄按了哪顆按鈕

                int count = this.RSN_LENTH;

                for (int i = 0; i < count; ++i)
                {
                    string temp = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_ID FROM C_REVIEW_MANAGEMENT WHERE SN = '" + e.CommandArgument.ToString() + "'");
                    string year = (System.Convert.ToInt32(DateTime.Now.Year.ToString()) - 1911).ToString();
                    string today = year + DateTime.Now.ToString("MMdd");
                    string now = DateTime.Now.ToString("HH:mm:ss");

                    string strSQL = @"INSERT INTO C_BUSSINESSTRIP_HISTORY
                                                                   (SN,LEAVE_SN,REVIEW_ID,LETTER_DATE,LETTER_TIME,LEAVE_SCHEDULE_SN,DLTB01_SN,PROCESS_STATUS,MZ_MID)
                                                              VALUES
                                                                   ( NEXT VALUE FOR dbo.C_BUSSINESSTRIP_HISTORY_SN,@LEAVE_SN,@REVIEW_ID,@LETTER_DATE,@LETTER_TIME,@LEAVE_SCHEDULE_SN,@DLTB01_SN,@PROCESS_STATUS,@MZ_MID)";
                    DataTable temp_dt = o_DBFactory.ABC_toTest.Create_Table("SELECT LEAVE_SN , DLTB01_SN FROM C_BUSSINESSTRIP_HISTORY WHERE SN = '" + this.Get_RSN(i) + "'", "get");

                    string LEAVE_SN = temp_dt.Rows[0]["LEAVE_SN"].ToString();
                    string DLTB01_SN = temp_dt.Rows[0]["DLTB01_SN"].ToString();

                    SqlParameter[] parameterList = {
                    new SqlParameter("LEAVE_SN",SqlDbType.VarChar){Value = LEAVE_SN},
                    new SqlParameter("REVIEW_ID",SqlDbType.VarChar){Value = temp},
                    new SqlParameter("LETTER_DATE",SqlDbType.VarChar){Value = today},
                    new SqlParameter("LEAVE_SCHEDULE_SN",SqlDbType.VarChar){Value = this.SORT},
                    new SqlParameter("LETTER_TIME",SqlDbType.VarChar){Value = now},
                    new SqlParameter("DLTB01_SN",SqlDbType.Float){Value = DLTB01_SN},
                    new SqlParameter("PROCESS_STATUS",SqlDbType.Char){Value = '4'},
                    new SqlParameter("MZ_MID",SqlDbType.VarChar){Value =  SessionHelper.ADPMZ_ID}
                                                  };

                    o_DBFactory.ABC_toTest.ExecuteNonQuery( strSQL, parameterList);

                    //irk 2012/11/6 LOG增加
                    LogModel lm = new LogModel("C");
                    lm.sqlType = "A";
                    string sql = lm.RegexSQL(strSQL, parameterList);
                    lm.SqlHistory("新增差旅歷程", sql, Request.QueryString["TPM_FION"]);
                }

                this.s = "";
                DataTable tempMuch = new DataTable();
                CSonline = new CSonline("bussiness_search", SessionHelper.ADPMZ_ID);
                tempMuch = CSonline.getDate();
                GV_bussinesstrip.DataSource = tempMuch;
                GV_bussinesstrip.DataBind();
                GV_bussinesstrip.Visible = true;
                if (this.type != "")
                {
                    this.type = "";
                    Response.Redirect("~/18-Online_Leave/Online_work.aspx?TPM_FION=" + _TPM_FION);
                    this.ViewStateClear();
                }
                Panel_b_select_ModalPopupExtender.Hide();
                Page_update();
            }
        }

        protected void GV_b_CHECKER_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GV_b_CHECKER.PageIndex = e.NewPageIndex;
            GV_b_CHECK_show(EXAD, EXUNIT, this.isUpper);
            Panel_b_select_ModalPopupExtender.Show();
        }

        protected void btn_b_history_Command(object sender, CommandEventArgs e)  //歷程相關
        {
            //對應到MZ_DLTB01_SN欄位
            this.History = e.CommandArgument;
            DataTable tempHistory = new DataTable();
            tempHistory = bHistorySearch();
            gv_b_history.DataSource = tempHistory;
            gv_b_history.DataBind();
            //顯示判定//顯示判定//顯示判定//顯示判定
            pl_b_history.Visible = true;

        }

        protected DataTable bHistorySearch()  //歷程用的SQL句
        {
            //0808→DEAN
            int DLTB01_SN = int.Parse(this.History.ToStringNullSafe());
            string historySQL = @"SELECT C_BUSSINESSTRIP_HISTORY.SN ,C_BUSSINESSTRIP_HISTORY.PROCESS_DATE,
                   C_BUSSINESSTRIP_HISTORY.PROCESS_TIME,C_BUSSINESSTRIP_HISTORY.REVIEW_MESSAGE,C_BUSSINESSTRIP_HISTORY.REVIEW_ID,
                   C_STATUS.C_STATUS_NAME,C_DLTB01.MZ_FILE,A_DLBASE.MZ_NAME,A_DLBASE.MZ_EXAD,A_DLBASE.MZ_EXUNIT,A_DLBASE.MZ_OCCC FROM C_BUSSINESSTRIP_HISTORY 
                   INNER JOIN C_STATUS ON C_BUSSINESSTRIP_HISTORY.PROCESS_STATUS = C_STATUS.C_STATUS_SN 
                   INNER JOIN C_DLTB01 ON C_BUSSINESSTRIP_HISTORY.DLTB01_SN = C_DLTB01.MZ_DLTB01_SN 
                   INNER JOIN A_DLBASE ON C_BUSSINESSTRIP_HISTORY.REVIEW_ID = A_DLBASE.MZ_ID 
                   WHERE C_BUSSINESSTRIP_HISTORY.DLTB01_SN =" + DLTB01_SN +
                  " ORDER BY C_BUSSINESSTRIP_HISTORY.SN";
            temp_SharedDT = o_DBFactory.ABC_toTest.Create_Table(historySQL, "history");

            return temp_SharedDT;
        }

        //核定相關SQL
        protected void gv_b_history_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //0808→DEAN
            DataTable tempHistory = new DataTable();
            tempHistory = bHistorySearch();
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label date = (Label)e.Row.FindControl("lbl_history_date");
                if (temp_SharedDT.Rows[iHistory]["PROCESS_DATE"].ToString() == "")
                {
                    date.Text = "尚未批";
                }
                else
                {
                    history_date = tempHistory.Rows[iHistory]["PROCESS_DATE"].ToString().Substring(0, 3) + "/" +
                                   tempHistory.Rows[iHistory]["PROCESS_DATE"].ToString().Substring(3, 2) + "/" +
                                   tempHistory.Rows[iHistory]["PROCESS_DATE"].ToString().Substring(5, 2) + " " +
                                   tempHistory.Rows[iHistory]["PROCESS_TIME"].ToString();
                    date.Text = history_date.ToString();

                }
                iHistory++;

                string exad = o_A_KTYPE.RAD(e.Row.Cells[1].Text);
                e.Row.Cells[1].Text = exad == "新北市政府警察局" ? "警察局" : exad.Replace("新北市政府警察局", "");
                e.Row.Cells[2].Text = o_A_KTYPE.RUNIT(e.Row.Cells[2].Text);
                e.Row.Cells[3].Text = o_A_KTYPE.CODE_TO_NAME(e.Row.Cells[3].Text, "26");
            }
        }


        protected void lbtn_self_businesstrip_Click(object sender, EventArgs e)
        {
            this.WHATS = "flow";
            flow_Click("", "", "C_BUSSINESSTRIP_HISTORY");
        }

        protected void gv_Business_Detail_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_Business_Detail.PageIndex = e.NewPageIndex;

            gv_Business_Detail.DataSource = Session["gv_Business_Detail_dt"] as DataTable;

            gv_Business_Detail.DataBind();

            P_E_ModalPopupExtender.Show();
        }

        protected void DDL_EXAD_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeExUnit();
        }

        protected void btn_b_message_Command(object sender, CommandEventArgs e)
        {
            txt_b_message.Text = string.Empty;
            panel_b_message_pop.Show();
        }

        #endregion

        #region 銷假


        protected void gv_change_dltb01_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_change_dltb01.PageIndex = e.NewPageIndex;
            change_dltb01_show();
        }

        private void change_dltb01_show()
        {
            DataTable temp = new DataTable();

            CSonline = new CSonline("change_dltb01_search", SessionHelper.ADPMZ_ID);

            temp = CSonline.getDate();

            gv_change_dltb01.DataSource = temp;
            gv_change_dltb01.DataBind();
        }

        protected void gv_change_dltb01_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (this.MODE == Const_Mode.簽核狀態)
            {

                if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[0].Attributes.Add("style", "display:none");
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    LinkButton myhlink1 = (LinkButton)e.Row.FindControl("lbtn_time");

                    DataTable gv_change_dltb01 = o_DBFactory.ABC_toTest.Create_Table("select * from C_DLTB01 WHERE MZ_DLTB01_SN =" + myhlink1.CommandArgument, "A");

                    //請假的日期顯示
                    person_date = gv_change_dltb01.Rows[0]["MZ_IDATE1"].ToString().Substring(0, 3) + "/" + gv_change_dltb01.Rows[0]["MZ_IDATE1"].ToString().Substring(3, 2) + "/" + gv_change_dltb01.Rows[0]["MZ_IDATE1"].ToString().Substring(5, 2);
                    string exad = o_A_KTYPE.RAD(e.Row.Cells[2].Text);
                    e.Row.Cells[2].Text = exad == "新北市政府警察局" ? "警察局" : exad.Replace("新北市政府警察局", "");
                    e.Row.Cells[3].Text = o_A_KTYPE.RUNIT(e.Row.Cells[3].Text);
                    e.Row.Cells[4].Text = gv_change_dltb01.Rows[0]["MZ_NAME"].ToString();
                    myhlink1.Text = person_date.ToString();
                }

            }
            else if (this.MODE == Const_Mode.已核定假單)
            {
                DataTable temp = new DataTable();
                string strSQL = @"select C_DLTB01.*,C_CHANGE_DLTB01_HISTORY.*,C_DLCODE.MZ_CNAME FROM C_CHANGE_DLTB01_HISTORY INNER JOIN 
                     C_DLTB01 ON C_CHANGE_DLTB01_HISTORY.DLTB01_SN= C_DLTB01.MZ_DLTB01_SN
                     INNER JOIN C_DLCODE ON C_DLTB01.MZ_CODE = C_DLCODE.MZ_CODE 
                     WHERE C_CHANGE_DLTB01_HISTORY.PROCESS_STATUS ='7' order by C_CHANGE_DLTB01_HISTORY.SN DESC";
                temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GJ");
                if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[0].Attributes.Add("style", "display:none");
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    LinkButton myhlink1 = (LinkButton)e.Row.FindControl("lbtn_time");

                    DataTable gv_change_dltb01 = o_DBFactory.ABC_toTest.Create_Table("select * from C_DLTB01 WHERE MZ_DLTB01_SN =" + myhlink1.CommandArgument, "A");

                    //請假的日期顯示
                    person_date = gv_change_dltb01.Rows[0]["MZ_IDATE1"].ToString().Substring(0, 3) + "/" + gv_change_dltb01.Rows[0]["MZ_IDATE1"].ToString().Substring(3, 2) + "/" + gv_change_dltb01.Rows[0]["MZ_IDATE1"].ToString().Substring(5, 2);
                    string exad = o_A_KTYPE.RAD(e.Row.Cells[2].Text);
                    e.Row.Cells[2].Text = exad == "新北市政府警察局" ? "警察局" : exad.Replace("新北市政府警察局", "");
                    e.Row.Cells[3].Text = o_A_KTYPE.RUNIT(e.Row.Cells[3].Text);
                    e.Row.Cells[4].Text = gv_change_dltb01.Rows[0]["MZ_NAME"].ToString();
                    myhlink1.Text = person_date.ToString();
                }

            }
            else
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    LinkButton myhlink1 = (LinkButton)e.Row.FindControl("lbtn_time");//lbtn的內容與顯示
                    DataTable gv_change_dltb01 = o_DBFactory.ABC_toTest.Create_Table("select * from C_DLTB01 WHERE MZ_DLTB01_SN =" + myhlink1.CommandArgument, "A");
                    //請假的日期，顯示為 年/月/日
                    string exad = o_A_KTYPE.RAD(e.Row.Cells[2].Text);
                    e.Row.Cells[2].Text = exad == "新北市政府警察局" ? "警察局" : exad.Replace("新北市政府警察局", "");
                    e.Row.Cells[3].Text = o_A_KTYPE.RUNIT(e.Row.Cells[3].Text);
                    mz_date = gv_change_dltb01.Rows[0]["MZ_IDATE1"].ToString().Substring(0, 3) + "/" + gv_change_dltb01.Rows[0]["MZ_IDATE1"].ToString().Substring(3, 2) + "/" + gv_change_dltb01.Rows[0]["MZ_IDATE1"].ToString().Substring(5, 2);
                    e.Row.Cells[4].Text = gv_change_dltb01.Rows[0]["MZ_NAME"].ToString();
                    myhlink1.Text = mz_date.ToString();
                }
            }
        }
        /// <summary>
        /// GridView: 銷假歷程資料綁定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gv_change_dltb01_history_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView dr = (DataRowView)e.Row.DataItem;
                Label lbl_history_date = (Label)e.Row.FindControl("lbl_history_date");
                if (dr["PROCESS_DATE"].ToStringNullSafe() == "")
                {
                    lbl_history_date.Text = "尚未批";
                }
                else
                {
                    string strDate = string.Format("{0}/{1}/{2} {3}"
                        , dr["PROCESS_DATE"].ToStringNullSafe().SubstringOutToEmpty(0, 3)
                        , dr["PROCESS_DATE"].ToStringNullSafe().SubstringOutToEmpty(3, 2)
                        , dr["PROCESS_DATE"].ToStringNullSafe().SubstringOutToEmpty(5, 2)
                        , dr["PROCESS_TIME"].ToStringNullSafe());

                    lbl_history_date.Text = strDate;
                }

                string exad = o_A_KTYPE.RAD(e.Row.Cells[1].Text);
                e.Row.Cells[1].Text = exad == "新北市政府警察局" ? "警察局" : exad.Replace("新北市政府警察局", "");
                e.Row.Cells[2].Text = o_A_KTYPE.RUNIT(e.Row.Cells[2].Text);
                e.Row.Cells[3].Text = o_A_KTYPE.CODE_TO_NAME(e.Row.Cells[3].Text, "26");
            }
        }
        /// <summary>
        /// 按鈕: 銷假抽回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_change_dltb01_re_Click(object sender, EventArgs e)
        {
            //處理選擇要抽回的單據
            for (int a = 0; a < gv_change_dltb01.Rows.Count; a++)
            {
                CheckBox agreeCK = (CheckBox)gv_change_dltb01.Rows[a].FindControl("CB_select");
                if (agreeCK.Checked == true)
                {
                    //取得上一層陳核單據編號
                    string lowerSN = o_DBFactory.ABC_toTest.vExecSQL(string.Format(@"Select sn From 
                                                                        (select rownum as row_id ,sn from (select * from C_CHANGE_DLTB01_HISTORY where DLTB01_SN='{0}'  order by sn desc)) 
                                                                     Where row_id=2",
                                                                     gv_change_dltb01.DataKeys[a]["MZ_DLTB01_SN"].ToString()));

                    //若有上一層單據編號則更新為待審核狀態
                    if (!string.IsNullOrEmpty(lowerSN))
                    {
                        string upSQL = string.Format(@"UPDATE C_CHANGE_DLTB01_HISTORY SET PROCESS_DATE='', PROCESS_STATUS='4', PROCESS_TIME='' 
                                                        WHERE SN='{0}' ", lowerSN);
                        o_DBFactory.ABC_toTest.Edit_Data(upSQL);
                        LogModel.saveLog("C", "U", upSQL, new List<SqlParameter>(), _query.TPMFION, "抽回銷假簽核_更新上層");
                    }

                    strSQL = string.Format(@"Delete FROM C_CHANGE_DLTB01_HISTORY WHERE SN='{0}' ", gv_change_dltb01.DataKeys[a]["SN"].ToStringNullSafe());
                    o_DBFactory.ABC_toTest.Edit_Data(strSQL);
                    LogModel.saveLog("C", "D", strSQL, new List<SqlParameter>(), _query.TPMFION, "抽回銷假簽核_刪除單據");
                }
            }

            _query.SendID = SessionHelper.ADPMZ_ID.ToStringNullSafe();
            gv_change_dltb01.DataSource = OWService.GetReCHANGEH(_query);
            gv_change_dltb01.DataBind();
            gv_change_dltb01.Visible = true;
            Page_update();
        }

        protected void btn_change_dltb01_back_Click(object sender, EventArgs e)
        {
            int count = gv_change_dltb01.Rows.Count;
            for (int n = 0; n < gv_change_dltb01.Rows.Count; n++)
            {
                CheckBox myCheckbox = (CheckBox)gv_data.Rows[n].FindControl("CB_select");
                if (myCheckbox.Checked == true)
                {
                    //20140616
                    //ViewState["SN" + n] = gv_change_dltb01.DataKeys[n]["SN"];
                    strSQL = @"UPDATE C_CHANGE_DLTB01_HISTORY SET 
                        PROCESS_DATE=@PROCESS_DATE,PROCESS_STATUS=@PROCESS_STATUS,
                        REVIEW_MESSAGE=@REVIEW_MESSAGE,PROCESS_TIME=@PROCESS_TIME 
                        WHERE SN =" + gv_change_dltb01.DataKeys[n]["SN"];
                    SQL_UPDATE(n, btn_change_dltb01_back.Text.ToString(), strSQL, false);
                }
            }
            DataTable tempMuch = new DataTable();

            CSonline = new CSonline("change_dltb01_search", SessionHelper.ADPMZ_ID);

            tempMuch = CSonline.getDate();

            txt_message.Text = string.Empty;
            if (tempMuch.Rows.Count == 0)
            {
                this.type = "Y";
            }
            gv_change_dltb01.DataSource = tempMuch;
            gv_change_dltb01.DataBind();
            gv_change_dltb01.Visible = true;
            this.RSN_LENTH = count;
            Page_update();
            btn_popup6_ModalPopupExtender.Show();
        }

        protected void btn_change_dltb01_sign_Click(object sender, EventArgs e)
        {
            //標記,目前需要的參數
            this.isUpper = false;

            string agreeSQL = string.Empty;
            int count = 0;
            string content = string.Empty;
            //20140613
            for (int a = 0; a < gv_change_dltb01.Rows.Count; a++)
            {
                CheckBox agreeCK = (CheckBox)gv_change_dltb01.Rows[a].FindControl("CB_select");
                if (agreeCK.Checked == true)
                {
                    //20140616
                    //ViewState["SN" + count] = gv_change_dltb01.DataKeys[a]["SN"];
                    this.Set_RSN(count, gv_change_dltb01.DataKeys[a]["SN"]);
                    Reviewer(a, 0, gv_change_dltb01, "C_CHANGE_DLTB01_HISTORY");
                    //設值 UI上方的 待簽核表單 個表單的勾選狀態 有標y的代表打勾
                    this.Set_upd(a, "y");
                    this.btn = btn_b_sign.Text.ToString();
                    count++;

                    if (content == null || content == "")
                    {
                        content = this.SCHEDULE;
                    }
                    if (content != this.SCHEDULE)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "alert('請確定下一筆流程都往同單位')", true);
                        return;
                    }
                }
            }



            if (count > 0)
            {
                GV_C_CHECK_show(EXAD, EXUNIT, this.isUpper);
                //顧名思義判斷有無承辦人或主管
                if (this.np != "")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "alert('找不到該單位承辦人或單位主管')", true);
                    this.np = "";
                    return;
                }

                btn_popup6_ModalPopupExtender.Show();
                // Panel_b_select_ModalPopupExtender.Show();//選人的panel
            }

            this.RSN_LENTH = count;
            Page_update();
        }

        private void CHANGE_DLTB01_STATUS(string DLTB01_SN)
        {
            string updateString = "UPDATE C_CHANGE_DLTB01 SET CHANGE_DLTB01_STATUS='Y' WHERE MZ_DLTB01_SN=@MZ_DLTB01_SN";

            SqlParameter[] paraList ={
                                         new SqlParameter("MZ_DLTB01_SN",SqlDbType.VarChar){Value =  DLTB01_SN }
                                        };
            o_DBFactory.ABC_toTest.ExecuteNonQuery( updateString, paraList);

            DataTable dt = o_DBFactory.ABC_toTest.Create_Table("SELECT * FROM C_CHANGE_DLTB01 WHERE  MZ_DLTB01_SN= " + DLTB01_SN, "12344");

            if (dt.Rows.Count > 0)
            {
                //2013/07/18
                //if (dt.Rows[0]["NEW_MZ_TDAY"].ToString() == "0" && dt.Rows[0]["NEW_MZ_TDAY"].ToString() == "0")
                //{
                //    string DeleteString = "DELETE FROM C_DLTB01 WHERE MZ_DLTB01_SN=" + DLTB01_SN + "";

                //    o_DBFactory.ABC_toTest.Edit_Data(DeleteString);
                //}
                if (dt.Rows[0]["NEW_MZ_TDAY"].ToString() == "0" && dt.Rows[0]["NEW_MZ_TTIME"].ToString() == "0")
                {
                    //20140108 若刪除假單,銷假記錄就查不到,嚴重bug
                    string DeleteString = "DELETE FROM C_DLTB01 WHERE MZ_DLTB01_SN=" + DLTB01_SN + "";

                    o_DBFactory.ABC_toTest.Edit_Data(DeleteString);
                }
                else
                {
                    updateString = "UPDATE C_DLTB01 SET MZ_IDATE1=@MZ_IDATE1,MZ_ITIME1=@MZ_ITIME1,MZ_ODATE=@MZ_ODATE,MZ_OTIME=@MZ_OTIME,MZ_TDAY=@MZ_TDAY,MZ_TTIME=@MZ_TTIME  WHERE MZ_DLTB01_SN=@MZ_DLTB01_SN";

                    SqlParameter[] paraList1 ={
                                         new SqlParameter("MZ_DLTB01_SN",SqlDbType.VarChar){Value =  DLTB01_SN },
                                         new SqlParameter("MZ_IDATE1",SqlDbType.VarChar){Value =  dt.Rows[0]["NEW_MZ_IDATE1"].ToString() },
                                         new SqlParameter("MZ_ITIME1",SqlDbType.VarChar){Value =  dt.Rows[0]["NEW_MZ_ITIME1"].ToString() },
                                         new SqlParameter("MZ_ODATE",SqlDbType.VarChar){Value =  dt.Rows[0]["NEW_MZ_ODATE"].ToString() },
                                         new SqlParameter("MZ_OTIME",SqlDbType.VarChar){Value =  dt.Rows[0]["NEW_MZ_OTIME"].ToString() },
                                         new SqlParameter("MZ_TDAY",SqlDbType.Float){Value =  dt.Rows[0]["NEW_MZ_TDAY"].ToString() },
                                         new SqlParameter("MZ_TTIME",SqlDbType.Float){Value =  dt.Rows[0]["NEW_MZ_TTIME"].ToString() },
                                        };
                    o_DBFactory.ABC_toTest.ExecuteNonQuery( updateString, paraList1);

                    //irk 2012/11/6 LOG增加
                    LogModel lm = new LogModel("C");
                    lm.sqlType = "U";
                    string sql = lm.RegexSQL(updateString, paraList1);
                    lm.SqlHistory("銷假修改日期", sql, Request.QueryString["TPM_FION"]);
                }
            }
        }
        /// <summary>
        /// 按鈕: 銷假決行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_change_dltb01_check_Click(object sender, EventArgs e)
        {
            int count = gv_change_dltb01.Rows.Count;

            for (int n = 0; n < gv_change_dltb01.Rows.Count; n++)
            {
                CheckBox myCheckbox = (CheckBox)gv_change_dltb01.Rows[n].FindControl("CB_select");
                if (myCheckbox.Checked == true)
                {
                    strSQL = @"UPDATE C_CHANGE_DLTB01_HISTORY SET 
                    PROCESS_DATE = @PROCESS_DATE,PROCESS_STATUS = @PROCESS_STATUS,REVIEW_MESSAGE=@REVIEW_MESSAGE,
                    PROCESS_TIME = @PROCESS_TIME WHERE SN=" + gv_change_dltb01.DataKeys[n]["SN"];
                    SQL_UPDATE(n, btn_change_dltb01_check.Text.ToString(), strSQL, true);
                    // SQL_INSERTOK(n, "C_CHANGE_DLTB01_HISTORY", gv_change_dltb01);
                    CHANGE_DLTB01_STATUS(gv_change_dltb01.DataKeys[n]["MZ_DLTB01_SN"].ToString());

                    //2013/09/16加入insert狀態為7
                    try
                    {
                        //string temp = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_ID FROM C_REVIEW_MANAGEMENT WHERE SN = '" + gv_change_dltb01.DataKeys[n]["SN"] + "'");
                        string year = (System.Convert.ToInt32(DateTime.Now.Year.ToString()) - 1911).ToString();
                        string today = year + DateTime.Now.ToString("MMdd");
                        string now = DateTime.Now.ToString("HH:mm:ss");

                        string SQL_TEMPPP = " SELECT LEAVE_SN,DLTB01_SN FROM C_CHANGE_DLTB01_HISTORY WHERE SN = '" + gv_change_dltb01.DataKeys[n]["SN"] + "'";
                        DataTable temppp = o_DBFactory.ABC_toTest.Create_Table(SQL_TEMPPP, "get");

                        //////
                        string LEAVE_SN = "";
                        string DLTB01_SN = "";
                        if (temppp.Rows.Count > 0)
                        {
                            LEAVE_SN = temppp.Rows[0]["LEAVE_SN"].ToString();
                            DLTB01_SN = temppp.Rows[0]["DLTB01_SN"].ToString();
                        }
                        //////


                        strSQL = @"INSERT INTO C_CHANGE_DLTB01_HISTORY 
                             (SN,LEAVE_SN,LETTER_DATE,LETTER_TIME,LEAVE_SCHEDULE_SN,DLTB01_SN,PROCESS_STATUS,MZ_MID) 
                             VALUES
                            ( NEXT VALUE FOR dbo.C_CHANGE_DLTB01_HISTORY_SN,@LEAVE_SN,@LETTER_DATE,@LETTER_TIME,@LEAVE_SCHEDULE_SN,@DLTB01_SN,@PROCESS_STATUS,@MZ_MID)";


                        SqlParameter[] parameterList = {
                  new SqlParameter("LEAVE_SN",SqlDbType.VarChar){Value =  LEAVE_SN},///////////////////
                   // new SqlParameter("REVIEW_ID",SqlDbType.VarChar){Value = SessionHelper.ADPMZ_ID},
                    new SqlParameter("LETTER_DATE",SqlDbType.VarChar){Value = today},


                    new SqlParameter("LETTER_TIME",SqlDbType.VarChar){Value = now},
  //new SqlParameter("LEAVE_SCHEDULE_SN",SqlDbType.VarChar){Value = this.SORT},
                    new SqlParameter("LEAVE_SCHEDULE_SN",SqlDbType.VarChar){Value = "999"},
                    new SqlParameter("DLTB01_SN",SqlDbType.Float){Value = DLTB01_SN},
                    new SqlParameter("PROCESS_STATUS",SqlDbType.Char){Value = '7'},
                    new SqlParameter("MZ_MID",SqlDbType.VarChar){Value =  SessionHelper.ADPMZ_ID}
                                                  };


                        o_DBFactory.ABC_toTest.ExecuteNonQuery( strSQL, parameterList);
                    }
                    catch
                    {

                    }
                    //2013/09/16加入insert狀態為7




                }
            }




            DataTable tempMuch = new DataTable();

            CSonline = new CSonline("change_dltb01_search", SessionHelper.ADPMZ_ID);

            tempMuch = CSonline.getDate();

            if (tempMuch.Rows.Count == 0)
            {
                this.type = "Y";
            }

            gv_change_dltb01.DataSource = tempMuch;
            gv_change_dltb01.DataBind();
            gv_change_dltb01.Visible = true;
            Page_update();
            if (this.type != "")
            {
                this.type = "";
                Response.Redirect("~/18-Online_Leave/Online_work.aspx?TPM_FION=" + _TPM_FION);
                this.ViewStateClear();
            }
            this.RSN_LENTH = count;
        }

        protected void btn_c_ck_Click(object sender, EventArgs e)
        {
            this.txt_message_Text = txt_c_message.Text;

            btn_fake4_ModalPopupExtender.Hide();
        }

        protected void btn_c_exit_Click(object sender, EventArgs e)
        {
            this.s = "";
            btn_popup6_ModalPopupExtender.Hide();
        }

        protected void GV_C_CHECKER_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GV_C_CHECKER.PageIndex = e.NewPageIndex;
            GV_C_CHECK_show(EXAD, EXUNIT, this.isUpper);
            btn_popup6_ModalPopupExtender.Show();
        }

        private void GV_C_CHECK_show(string EXAD, string EXUNIT, bool isToUpper)
        {
            DataTable temp = new DataTable();
            //取得線上簽核功能中,可以再陳核的人員資料,針對Online_work.aspx功能
            temp = OWService.Get_REVIEW_MANAGEMENT_forOnline_work(EXAD, EXUNIT, 0, SessionHelper.ADPMZ_ID, isToUpper);// o_DBFactory.ABC_toTest.Create_Table("SELECT c_review_management.sn,CASE WHEN REVIEW_LEVEL='1' THEN '承辦人' ELSE (SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='26' AND MZ_KCODE=C_REVIEW_MANAGEMENT.MZ_OCCC) END AS MZ_OCCC,a_dlbase.mz_name FROM c_review_management,a_dlbase WHERE A_DLBASE.MZ_ID = C_REVIEW_MANAGEMENT.MZ_ID AND C_REVIEW_MANAGEMENT.MZ_EXAD = '" + EXAD + "' AND C_REVIEW_MANAGEMENT.MZ_EXUNIT = '" + EXUNIT + "'", "get");
            int x = temp.Rows.Count;
            GV_C_CHECKER.DataSource = temp;
            GV_C_CHECKER.DataBind();
            this.s = "y";
            if (x == 0)
            {
                this.np = "y";
            }
        }

        protected void GV_C_CHECKER_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //0815→Dean
            if (e.CommandName == "checker")
            {
                for (int a = 0; a < gv_change_dltb01.Rows.Count; a++)
                {
                    if (this.Get_upd(a) != "")
                    {
                        SQL_UPDATE_XXX_HISTORY(a, this.btn, gv_change_dltb01, "C_CHANGE_DLTB01_HISTORY", true);
                        this.Set_upd(a, "");
                    }
                }
                this.btn = "";//清除目前記錄按了哪顆按鈕

                int count = this.RSN_LENTH;

                for (int i = 0; i < count; ++i)
                {
                    string temp = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_ID FROM C_REVIEW_MANAGEMENT WHERE SN = '" + e.CommandArgument.ToString() + "'");
                    string year = (System.Convert.ToInt32(DateTime.Now.Year.ToString()) - 1911).ToString();
                    string today = year + DateTime.Now.ToString("MMdd");
                    string now = DateTime.Now.ToString("HH:mm:ss");

                    string strSQL = @"INSERT INTO C_CHANGE_DLTB01_HISTORY
                                                                   (SN,LEAVE_SN,REVIEW_ID,LETTER_DATE,LETTER_TIME,LEAVE_SCHEDULE_SN,DLTB01_SN,PROCESS_STATUS,MZ_MID)
                                                              VALUES
                                                                   ( NEXT VALUE FOR dbo.C_CHANGE_DLTB01_HISTORY_SN,@LEAVE_SN,@REVIEW_ID,@LETTER_DATE,@LETTER_TIME,@LEAVE_SCHEDULE_SN,@DLTB01_SN,@PROCESS_STATUS,@MZ_MID)";

                    DataTable temp_dt = o_DBFactory.ABC_toTest.Create_Table("SELECT LEAVE_SN , DLTB01_SN FROM C_CHANGE_DLTB01_HISTORY WHERE SN = '" + this.Get_RSN(i) + "'", "get");

                    string LEAVE_SN = temp_dt.Rows[0]["LEAVE_SN"].ToString();
                    string DLTB01_SN = temp_dt.Rows[0]["DLTB01_SN"].ToString();

                    SqlParameter[] parameterList = {
                    new SqlParameter("LEAVE_SN",SqlDbType.VarChar){Value = LEAVE_SN},
                    new SqlParameter("REVIEW_ID",SqlDbType.VarChar){Value = temp},
                    new SqlParameter("LETTER_DATE",SqlDbType.VarChar){Value = today},
                    new SqlParameter("LEAVE_SCHEDULE_SN",SqlDbType.VarChar){Value = this.SORT},
                    new SqlParameter("LETTER_TIME",SqlDbType.VarChar){Value = now},
                    new SqlParameter("DLTB01_SN",SqlDbType.Float){Value = DLTB01_SN},
                    new SqlParameter("PROCESS_STATUS",SqlDbType.Char){Value = '4'},
                    new SqlParameter("MZ_MID",SqlDbType.VarChar){Value =  SessionHelper.ADPMZ_ID}
                                                  };

                    o_DBFactory.ABC_toTest.ExecuteNonQuery( strSQL, parameterList);

                    //irk 2012/11/6 LOG增加
                    LogModel lm = new LogModel("C");
                    lm.sqlType = "U";
                    string sql = lm.RegexSQL(strSQL, parameterList);
                    lm.SqlHistory("新增銷假記錄", sql, Request.QueryString["TPM_FION"]);

                }
                //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('資料已送出')", true);
                this.s = "";
                DataTable tempMuch = new DataTable();
                CSonline = new CSonline("change_dltb01_search", SessionHelper.ADPMZ_ID);
                tempMuch = CSonline.getDate();
                gv_change_dltb01.DataSource = tempMuch;
                gv_change_dltb01.DataBind();
                if (this.type != "")
                {
                    this.type = "";
                    Response.Redirect("~/18-Online_Leave/Online_work.aspx?TPM_FION=" + _TPM_FION);
                    this.ViewStateClear();
                }
                gv_change_dltb01.Visible = true;
                btn_popup6_ModalPopupExtender.Hide();
                Page_update();
            }
        }

        protected void btn_exit1_Click(object sender, EventArgs e)
        {
            this.s = "";
            Panel_b_select_ModalPopupExtender.Hide();
        }


        /// <summary>
        /// GridView: 銷假歷程按鈕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_c_history_Command(object sender, CommandEventArgs e)
        {
            //設定Query條件
            _query.DLTB01_SN = e.CommandArgument.ToStringNullSafe();


            gv_change_dltb01_history.DataSource = OWService.GetCHANGEHistory(_query);
            gv_change_dltb01_history.DataBind();
            pl_change_dltb01_history.Visible = true;
        }

        protected void lbtn_c_content(object sender, CommandEventArgs e)//觀看銷假內容的設定
        {
            pl_Show.GroupingText = "銷假申請內容";
            DataTable tempShow = new DataTable();
            tempShow = content_Search((String)e.CommandArgument, "C_CHANGE_DLTB01_HISTORY");
            lbl_MZ_NAME.Text = tempShow.Rows[0]["MZ_NAME"].ToString();
            lbl_MZ_CNAME.Text = tempShow.Rows[0]["MZ_CNAME"].ToString();
            lbl_MZ_EXAD.Text = o_A_KTYPE.RAD(tempShow.Rows[0]["MZ_EXAD"].ToString());
            lbl_MZ_EXUNIT.Text = o_A_KTYPE.RUNIT(tempShow.Rows[0]["MZ_EXUNIT"].ToString());
            string Show_Date = temp_SharedDT.Rows[0]["MZ_IDATE1"].ToString() + temp_SharedDT.Rows[0]["MZ_ITIME1"] +
                    temp_SharedDT.Rows[0]["MZ_ODATE"].ToString() + temp_SharedDT.Rows[0]["MZ_OTIME"].ToString() +
                    temp_SharedDT.Rows[0]["MZ_TDAY"].ToString() + temp_SharedDT.Rows[0]["MZ_TTIME"].ToString();
            lbl_Date.Text = DateAndTime(temp_SharedDT);
            lbl_MZ_CAUSE.Text = temp_SharedDT.Rows[0]["MZ_CAUSE"].ToString();
            lblMZ_ID.Text = tempShow.Rows[0]["MZ_ID"].ToString();
            lblOccc.Text = o_A_KTYPE.CODE_TO_NAME(tempShow.Rows[0]["MZ_OCCC"].ToString(), "26");

            //0812→Dean
            string MZ_ID = tempShow.Rows[0]["MZ_ID"].ToString();

            var _SQL = @"SELECT 
            SUM(MZ_TDAY)-SUM(ORGINAL_MZ_TDAY-NEW_MZ_TDAY) MZ_TDAY,
            SUM(MZ_TTIME)-SUM(ORGINAL_MZ_TTIME-NEW_MZ_TTIME) MZ_TTIME
            FROM C_DLTB01 
            left join C_CHANGE_DLTB01
            on C_DLTB01.MZ_DLTB01_SN=C_CHANGE_DLTB01.MZ_DLTB01_SN 
            and CHANGE_DLTB01_STATUS='Y'
            WHERE MZ_CODE='03' 
            AND MZ_ID='" + MZ_ID + "' AND dbo.SUBSTR(MZ_IDATE1,1,3)='" + (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0') + "'";

            DataTable temp_dt = o_DBFactory.ABC_toTest.Create_Table(_SQL.ToString(), "get");
            int hday = 0;
            int htime = 0;
            if (temp_dt.Rows.Count > 0)
            {
                hday = string.IsNullOrEmpty(temp_dt.Rows[0]["MZ_TDAY"].ToString()) ? 0 : int.Parse(temp_dt.Rows[0]["MZ_TDAY"].ToString());

                htime = string.IsNullOrEmpty(temp_dt.Rows[0]["MZ_TTIME"].ToString()) ? 0 : int.Parse(temp_dt.Rows[0]["MZ_TTIME"].ToString());
            }
            int hday_used;

            int htime_used;

            if (htime != 0)
            {
                int hday_Add = htime / 8;
                hday_used = hday + hday_Add;
                htime_used = htime % 8;
            }
            else
            {
                hday_used = hday;
                htime_used = 0;
            }
            lbl_QKDay.Text = hday_used.ToString() + "天" + htime_used.ToString() + "時";//已休假天數
            lbl_agent.Text = tempShow.Rows[0]["MZ_RNAME"].ToString();//代理人
            lbl_location.Text = tempShow.Rows[0]["MZ_TADD"].ToString();//休假地點
            lbl_allowance.Text = temp_SharedDT.Rows[0]["MZ_SWT"].ToString() == "Y" ? "是" : "否";//是否有申請補助
            lbl_abroad.Text = temp_SharedDT.Rows[0]["MZ_FOREIGN"].ToString() == "Y" ? "出國、" : "" +
                              temp_SharedDT.Rows[0]["MZ_CHINA"].ToString() == "Y" ? "大陸、" : "";//是否為出國、赴大陸
            lbl_abroad.Text = lbl_abroad.Text == "" ? "否" : lbl_abroad.Text.Substring(0, lbl_abroad.Text.Length - 1);
            //顯示判定
            pl_history.Visible = false;
            pl_overtime_history.Visible = false;
            P_E_ModalPopupExtender.Show();

            gv_Business_Detail.Visible = false;
            pl_change_dltb01_show.Visible = true;

            string strSQL = string.Format(@"SELECT C_CHANGE_DLTB01.* FROM C_CHANGE_DLTB01 WHERE MZ_DLTB01_SN={0} ", (String)e.CommandArgument);

            DataTable dt = new DataTable();

            dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET12345");

            if (dt.Rows.Count > 0)
            {
                ORIGINAL_DLTB01.Text = dt.Rows[0]["ORGINAL_MZ_IDATE1"].ToString() + " " + dt.Rows[0]["ORGINAL_MZ_ITIME1"].ToString() +
                             " 至 " + dt.Rows[0]["ORGINAL_MZ_ODATE"].ToString() + " " + dt.Rows[0]["ORGINAL_MZ_OTIME"].ToString() +
                             " 共 " + dt.Rows[0]["ORGINAL_MZ_TDAY"].ToString() + " 日 " + dt.Rows[0]["ORGINAL_MZ_TTIME"].ToString() + " 時";

                NEW_DLTB01.Text = dt.Rows[0]["NEW_MZ_IDATE1"].ToString() + " " + dt.Rows[0]["NEW_MZ_ITIME1"].ToString() +
                        " 至 " + dt.Rows[0]["NEW_MZ_ODATE"].ToString() + " " + dt.Rows[0]["NEW_MZ_OTIME"].ToString() +
                        " 共 " + dt.Rows[0]["NEW_MZ_TDAY"].ToString() + " 日 " + dt.Rows[0]["NEW_MZ_TTIME"].ToString() + " 時";

            }
        }


        protected void lbtn_self_change_dltb01_Click(object sender, EventArgs e)
        {
            this.WHATS = "flow";//2013/09/16 為什麼不帶選的時間
            flow_cancel_Click(txt_m.Text, DropDownList1.SelectedValue, "C_CHANGE_DLTB01_HISTORY");
            //flow_Click("", "", "C_CHANGE_DLTB01_HISTORY");
        }

        protected void btn_c_message_Command(object sender, CommandEventArgs e)
        {
            txt_c_message.Text = string.Empty;
            btn_fake4_ModalPopupExtender.Show();
        }



        protected void btn_change_dltb01_upper_Click(object sender, EventArgs e)
        {
            string agreeSQL = string.Empty;
            int count = 0;
            string content = string.Empty;
            //20140613
            for (int a = 0; a < gv_change_dltb01.Rows.Count; a++)
            {
                CheckBox agreeCK = (CheckBox)gv_change_dltb01.Rows[a].FindControl("CB_select");
                if (agreeCK.Checked == true)
                {
                    //20140616
                    //ViewState["SN" + count] = gv_change_dltb01.DataKeys[a]["SN"];
                    this.Set_RSN(count, gv_change_dltb01.DataKeys[a]["SN"]);

                    Reviewer(a, 1, gv_change_dltb01, "C_CHANGE_DLTB01_HISTORY");

                    //設值 UI上方的 待簽核表單 個表單的勾選狀態 有標y的代表打勾
                    this.Set_upd(a, "y");
                    this.btn = btn_change_dltb01_upper.Text.ToString();
                    count++;

                    if (content == null || content == "")
                    {
                        content = this.SCHEDULE;
                    }
                    if (content != this.SCHEDULE)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "alert('請確定下一筆流程都往同單位')", true);
                        return;
                    }
                }
            }



            GV_C_CHECK_show(EXAD, EXUNIT, this.isUpper);
            //顧名思義判斷有無承辦人或主管
            if (this.np != "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "alert('找不到該單位承辦人或單位主管')", true);
                this.np = "";
                return;
            }
            //如果不能繼續呈核給上層、跳出
            if (this.re != "")
            {
                this.re = "";
                return;
            }

            if (count > 0)
            {
                GV_C_CHECK_show(EXAD, EXUNIT, this.isUpper);
                //顧名思義判斷有無承辦人或主管
                int y = System.Convert.ToInt32(o_DBFactory.ABC_toTest.vExecSQL(@"SELECT COUNT(*) FROM c_review_management,a_dlbase WHERE A_DLBASE.MZ_ID = C_REVIEW_MANAGEMENT.MZ_ID
                                             AND C_REVIEW_MANAGEMENT.MZ_EXAD = '" + EXAD + "' AND C_REVIEW_MANAGEMENT.MZ_EXUNIT = '" + EXUNIT +
                                            "' AND C_REVIEW_MANAGEMENT.REVIEW_LEVEL = '1'"));
                if (this.np != "")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "alert('找不到該單位承辦人或單位主管')", true);
                    this.np = "";
                    return;
                }
                if (y == 1)
                {
                    for (int a = 0; a < gv_change_dltb01.Rows.Count; a++)
                    {
                        if (this.Get_upd(a) != "")
                        {
                            strSQL = @"UPDATE C_CHANGE_DLTB01_HISTORY SET 
                                 PROCESS_DATE = @PROCESS_DATE,PROCESS_STATUS = @PROCESS_STATUS,
                                 PROCESS_TIME = @PROCESS_TIME,REVIEW_MESSAGE=@REVIEW_MESSAGE WHERE SN=" + gv_change_dltb01.DataKeys[a]["SN"];
                            SQL_UPDATE(a, this.btn, strSQL, true);
                            this.Set_upd(a, "");
                        }
                    }
                    this.btn = "";//清除目前記錄按了哪顆按鈕
                    for (int i = 0; i < count; ++i)
                    {
                        string temp = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_ID FROM c_review_management WHERE " +
                                            " MZ_EXAD = '" + EXAD + "' AND MZ_EXUNIT = '" + EXUNIT +
                                            "' AND REVIEW_LEVEL = '1'");
                        string year = (System.Convert.ToInt32(DateTime.Now.Year.ToString()) - 1911).ToString();
                        string today = year + DateTime.Now.ToString("MMdd");
                        string now = DateTime.Now.ToString("HH:mm:ss");

                        string strSQL = @"INSERT INTO C_CHANGE_DLTB01_HISTORY
                                                                       (SN,LEAVE_SN,REVIEW_ID,LETTER_DATE,LETTER_TIME,LEAVE_SCHEDULE_SN,DLTB01_SN,PROCESS_STATUS,MZ_MID)
                                                                  VALUES
                                                                       ( NEXT VALUE FOR dbo.C_CHANGE_DLTB01_HISTORY_SN,@LEAVE_SN,@REVIEW_ID,@LETTER_DATE,@LETTER_TIME,@LEAVE_SCHEDULE_SN,@DLTB01_SN,@PROCESS_STATUS,@MZ_MID)";

                        DataTable temp_dt = o_DBFactory.ABC_toTest.Create_Table("SELECT LEAVE_SN,DLTB01_SN FROM C_CHANGE_DLTB01_HISTORY WHERE SN = '" + this.Get_RSN(i) + "'", "get");

                        string LEAVE_SN = temp_dt.Rows[0]["LEAVE_SN"].ToString();
                        string DLTB01_SN = temp_dt.Rows[0]["DLTB01_SN"].ToString();
                        SqlParameter[] parameterList = {
                    new SqlParameter("LEAVE_SN",SqlDbType.VarChar){Value = LEAVE_SN},
                    new SqlParameter("REVIEW_ID",SqlDbType.VarChar){Value = temp},
                    new SqlParameter("LETTER_DATE",SqlDbType.VarChar){Value = today},
                    new SqlParameter("LEAVE_SCHEDULE_SN",SqlDbType.VarChar){Value = this.SORT},
                    new SqlParameter("LETTER_TIME",SqlDbType.VarChar){Value = now},
                    new SqlParameter("DLTB01_SN",SqlDbType.Float){Value = DLTB01_SN},
                    new SqlParameter("PROCESS_STATUS",SqlDbType.Char){Value = '4'},
                    new SqlParameter("MZ_MID",SqlDbType.VarChar){Value =  SessionHelper.ADPMZ_ID}
                                                  };

                        o_DBFactory.ABC_toTest.ExecuteNonQuery( strSQL, parameterList);
                    }
                    //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('資料已送出')", true);
                    this.s = "";
                    DataTable tempMuch = new DataTable();
                    CSonline = new CSonline("change_dltb01_search", SessionHelper.ADPMZ_ID);
                    tempMuch = CSonline.getDate();
                    gv_change_dltb01.DataSource = tempMuch;
                    gv_change_dltb01.DataBind();
                    gv_change_dltb01.Visible = true;
                    if (this.type != "")
                    {
                        this.type = "";
                        Response.Redirect("~/18-Online_Leave/Online_work.aspx?TPM_FION=" + _TPM_FION);
                        this.ViewStateClear();
                    }
                }
                else
                {
                    btn_popup6_ModalPopupExtender.Show();//選人的panel
                }
            }
            this.RSN_LENTH = count;
            pl_change_dltb01_history.Visible = false;
            Page_update();
        }

        #endregion

        protected void ddl_Kind_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_Kind.SelectedValue == "差假")
            {
                Panel_search.Visible = true;
                pl_data2.Visible = true;
                gv_data.Visible = true;
                pl_change_dltb01.Visible = false;
                gv_change_dltb01.Visible = false;
                lbl_sign.Text = "假單簽核歷程查詢";
            }
            else
            {
                Panel_search.GroupingText = "銷假簽核歷程搜尋";
                Panel_search.Visible = true;

                pl_change_dltb01.Visible = true;
                gv_change_dltb01.Visible = true;
                pl_data2.Visible = false;
                gv_data.Visible = false;
                lbl_sign.Visible = true;
                lbl_sign.Text = "銷假簽核歷程查詢";
                //此頁面用途為給承辦人看已經核可之假單，如果需要按鈕則取消註解
                //pl_data.Visible = true;
                Page_update();
            }
        }

        //退回上個流程
        protected void btn_bef_process_Command(object sender, CommandEventArgs e)
        {
            string sn = e.CommandArgument.ToString();

            string[] snarr = sn.Split(',');


            if (string.IsNullOrEmpty(o_DBFactory.ABC_toTest.vExecSQL(string.Format(@"(select PROCESS_DATE from 
                                                    (select rownum as row_id ,sn,PROCESS_DATE  from 
                                                    (select * from c_leave_history where dltb01_sn='{0}'  order by sn desc)) 
                                                    where row_id=1)", snarr[0]))))
            {
                string sn2 = o_DBFactory.ABC_toTest.vExecSQL(string.Format(@"select sn from 
                                                    (select rownum as row_id ,sn  from 
                                                    (select * from c_leave_history where dltb01_sn='{0}'  order by sn desc)) 
                                                    where row_id=2", snarr[0]));
                if (!string.IsNullOrEmpty(sn2))
                {
                    //20140620 加上  ,REVIEW_MESSAGE='',RETURN_FLAG='' 
                    string upd = string.Format(@"UPDATE C_LEAVE_HISTORY SET  PROCESS_DATE = '' ,PROCESS_STATUS = '{0}',PROCESS_TIME = ''  ,REVIEW_MESSAGE='',RETURN_FLAG=''   WHERE SN={1}", 4, sn2);
                    strSQL = @"Delete FROM C_LEAVE_HISTORY WHERE SN = " + string.Format(@"(select sn from 
                                                    (select rownum as row_id ,sn  from 
                                                    (select * from c_leave_history where dltb01_sn='{0}'  order by sn desc)) 
                                                    where row_id=1)", snarr[0]);
                    o_DBFactory.ABC_toTest.ExecuteNonQuery( upd);
                    o_DBFactory.ABC_toTest.ExecuteNonQuery( strSQL);
                }
            }
            else
            {
                //20140620 加上  ,REVIEW_MESSAGE='',RETURN_FLAG='' 
                string upd = string.Format(@"UPDATE C_LEAVE_HISTORY SET  PROCESS_DATE = '' ,PROCESS_STATUS = '{0}',PROCESS_TIME = ''   ,REVIEW_MESSAGE='',RETURN_FLAG=''  WHERE SN=(select sn from 
                                                    (select rownum as row_id ,sn  from 
                                                    (select * from c_leave_history where dltb01_sn='{1}'  order by sn desc)) 
                                                    where row_id=1)", 4, snarr[0]);
                try
                {
                    o_DBFactory.ABC_toTest.ExecuteNonQuery( upd);
                }
                catch (Exception ex)
                {

                    LogModel lm = new LogModel("C");
                    lm.SqlHistory("error修改歷程X3", upd + ";--" + ex.Message, Request.QueryString["TPM_FION"]);

                }
            }

            this.History = e.CommandArgument;
            DataTable tempHistory = new DataTable();
            tempHistory = HistorySearch();
            gv_history.DataSource = tempHistory;
            gv_history.DataBind();
            //顯示判定//顯示判定//顯示判定//顯示判定
            pl_history.Visible = true;
            pl_overtime_history.Visible = false;
        }

        //簽核狀態
        void flow_Click(string Year, string Month, string tablename)
        {
            this.MODE = Const_Mode.簽核狀態;
            this.WHAT = "PERSON";
            var a = SessionHelper.ADPMZ_ID;
            DataTable agentsMuch = new DataTable();
            //TODO 不需要所有欄位
            string strSQL = "select C_DLTB01.*," + tablename + ".*,C_DLCODE.MZ_CNAME FROM " + tablename + " INNER JOIN "
                 + "C_DLTB01 ON " + tablename + ".DLTB01_SN= C_DLTB01.MZ_DLTB01_SN "
                 + "INNER JOIN C_DLCODE ON C_DLTB01.MZ_CODE = C_DLCODE.MZ_CODE "
                 + "WHERE C_DLTB01.MZ_ID='" + SessionHelper.ADPMZ_ID
                 + "' -- 2020.04.09 cancel off --AND " + tablename + ".RETURN_FLAG IS NULL  " + Environment.NewLine
                 + "  AND  (" + tablename + ".PROCESS_STATUS ='1' OR " + tablename + ".PROCESS_STATUS ='4' OR " + tablename + ".PROCESS_STATUS ='6' OR " + tablename + ".PROCESS_STATUS ='7') ";

            if (Year != "" && Month != "")
            {
                strSQL += " AND dbo.SUBSTR(C_DLTB01.MZ_IDATE1,1,5)='" + Year + Month + "'";
            }
            else
            {
                strSQL += " AND dbo.SUBSTR(C_DLTB01.MZ_IDATE1,1,5)='" + (DateTime.Now.Year - 1911).ToString() + (DateTime.Now.Month).ToString().PadLeft(2, '0') + "'";
            }

            strSQL += " order by " + tablename + ".SN";
            agentsMuch = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GJ");

            if (tablename == "C_LEAVE_HISTORY")//差(請)假
            {
                gv_data.DataSource = agentsMuch;
                gv_data.DataBind();
                gv_data.Columns[7].Visible = false;
                lbl_sign.Text = "個人假單簽核歷程";
                //顯示判定
                allclose();
                pl_data2.Visible = true;
                gv_data.Visible = true;
            }
            else if (tablename == "C_BUSSINESSTRIP_HISTORY")
            {
                GV_bussinesstrip.DataSource = agentsMuch;
                GV_bussinesstrip.DataBind();
                GV_bussinesstrip.Columns[6].Visible = false;
                lbl_sign.Text = "個人差旅費歷程";
                allclose();
                Panel_bussinesstrip_GV.Visible = true;
                GV_bussinesstrip.Visible = true;
            }
            else
            {
                gv_change_dltb01.DataSource = agentsMuch;
                gv_change_dltb01.DataBind();
                gv_change_dltb01.Columns[6].Visible = false;
                lbl_sign.Text = "個人銷假歷程";
                allclose();
                pl_change_dltb01.Visible = true;
                gv_change_dltb01.Visible = true;
            }

            lbl_sign.Visible = true;
            drop.Visible = true;
            pl_self_item.Visible = true;
            Page_update();
        }

        //銷假簽核狀態
        void flow_cancel_Click(string Year, string Month, string tablename)
        {
            this.MODE = Const_Mode.簽核狀態;
            this.WHAT = "PERSON";
            var a = SessionHelper.ADPMZ_ID;
            DataTable agentsMuch = new DataTable();
            //TODO 不需要所有欄位
            string strSQL = "select C_DLTB01.*," + tablename + ".*,C_DLCODE.MZ_CNAME FROM " + tablename + " INNER JOIN "
                 + "C_DLTB01 ON " + tablename + ".DLTB01_SN= C_DLTB01.MZ_DLTB01_SN "
                 + "INNER JOIN C_DLCODE ON C_DLTB01.MZ_CODE = C_DLCODE.MZ_CODE "
                 + "WHERE C_DLTB01.MZ_ID='" + SessionHelper.ADPMZ_ID
                 + "' -- 2020.04.09 cancel off -- AND " + tablename + ".RETURN_FLAG IS NULL  " + Environment.NewLine
                 + "  AND  (" + tablename + ".PROCESS_STATUS ='1' OR " + tablename + ".PROCESS_STATUS ='4' OR " + tablename + ".PROCESS_STATUS ='6' OR " + tablename + ".PROCESS_STATUS ='2') ";

            if (Year != "" && Month != "")
            {
                strSQL += " AND dbo.SUBSTR(C_DLTB01.MZ_IDATE1,1,5)='" + Year + Month + "'";
            }
            else
            {
                strSQL += " AND dbo.SUBSTR(C_DLTB01.MZ_IDATE1,1,5)='" + (DateTime.Now.Year - 1911).ToString() + (DateTime.Now.Month).ToString().PadLeft(2, '0') + "'";
            }

            strSQL += " order by " + tablename + ".SN";
            agentsMuch = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GJ");

            if (tablename == "C_LEAVE_HISTORY")//差(請)假
            {
                gv_data.DataSource = agentsMuch;
                gv_data.DataBind();
                gv_data.Columns[7].Visible = false;
                lbl_sign.Text = "個人假單簽核歷程";
                //顯示判定
                allclose();
                pl_data2.Visible = true;
                gv_data.Visible = true;
            }
            else if (tablename == "C_BUSSINESSTRIP_HISTORY")
            {
                GV_bussinesstrip.DataSource = agentsMuch;
                GV_bussinesstrip.DataBind();
                GV_bussinesstrip.Columns[6].Visible = false;
                lbl_sign.Text = "個人差旅費歷程";
                allclose();
                Panel_bussinesstrip_GV.Visible = true;
                GV_bussinesstrip.Visible = true;
            }
            else
            {
                gv_change_dltb01.DataSource = agentsMuch;
                gv_change_dltb01.DataBind();
                gv_change_dltb01.Columns[6].Visible = false;
                lbl_sign.Text = "個人銷假歷程";
                allclose();
                pl_change_dltb01.Visible = true;
                gv_change_dltb01.Visible = true;
            }

            lbl_sign.Visible = true;
            drop.Visible = true;
            pl_self_item.Visible = true;
            Page_update();
        }

        void flow_all_Click(string tablename)
        {
            this.MODE = Const_Mode.查詢簽核狀態;
            this.WHAT = "PERSON";
            DataTable agentsMuch = new DataTable();
            string strSQL = "select C_DLTB01.*,'' SN,'' LEAVE_SN,'' LEAVE_SCHEDULE_SN,C_DLCODE.MZ_CNAME FROM C_DLTB01,C_DLCODE WHERE C_DLTB01.MZ_CODE = C_DLCODE.MZ_CODE AND SIGN_KIND='2' ";

            if (txt_name.Text != "")
            {
                strSQL = strSQL + " AND C_DLTB01.MZ_NAME = '" + txt_name.Text + "' ";
            }
            if (txt_idno.Text != "")
            {
                strSQL = strSQL + " AND C_DLTB01.MZ_ID = '" + txt_idno.Text + "' ";
            }
            if (txt_date.Text != "" && txt_date1.Text != "")
            {
                strSQL = strSQL + " AND C_DLTB01.MZ_IDATE1 >= '" + txt_date.Text + "' AND C_DLTB01.MZ_IDATE1<='" + txt_date1.Text + "'";
            }
            else if (txt_date.Text != "")
            {
                strSQL = strSQL + " AND C_DLTB01.MZ_IDATE1 = '" + txt_date.Text + "'";
            }
            if (DDL_AD.SelectedItem.Text != "請選擇")
            {
                strSQL = strSQL + " AND C_DLTB01.MZ_EXAD = '" + DDL_AD.SelectedValue + "' ";
            }
            if (DDL_UNIT.SelectedItem.Text != "請選擇")
            {
                strSQL = strSQL + " AND C_DLTB01.MZ_EXUNIT = '" + DDL_UNIT.SelectedValue + "' ";
            }

            if (tablename == "C_CHANGE_DLTB01_HISTORY")
            {
                strSQL = strSQL + " AND MZ_DLTB01_SN IN (SELECT DISTINCT DLTB01_SN FROM C_CHANGE_DLTB01_HISTORY)";
            }

            strSQL += " order by C_DLTB01.MZ_IDATE1 DESC";
            agentsMuch = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GJ");
            //Log
            LogModel lm = new LogModel("C");
            lm.sqlType = "S";
            string sql = lm.RegexSQL(strSQL, new List<SqlParameter>());
            lm.SqlHistory("查詢線上簽核", sql, Request.QueryString["TPM_FION"]);

            ViewState.Remove("search");

            if (tablename == "C_LEAVE_HISTORY")
            {
                pl_change_dltb01.Visible = false;
                gv_change_dltb01.Visible = false;

                gv_data.DataSource = agentsMuch;
                this.gv_data_Person = agentsMuch;
                gv_data.DataBind();
                gv_data.Columns[7].Visible = false;

                //顯示判定
                allclose();
                process_bar.Visible = true;
                ddl_Kind.Visible = true;
                pl_data2.Visible = true;
                gv_data.Visible = true;
            }
            else
            {
                pl_data2.Visible = false;
                gv_data.Visible = false;
                gv_change_dltb01.DataSource = agentsMuch;
                this.gv_data_Person = agentsMuch;
                gv_change_dltb01.DataBind();
                gv_change_dltb01.Columns[6].Visible = false;

                //顯示判定
                allclose();
                process_bar.Visible = true;
                ddl_Kind.Visible = true;
                pl_change_dltb01.Visible = true;
                gv_change_dltb01.Visible = true;
            }

            Panel_search.Visible = true;
            lbl_sign.Visible = true;
            //drop.Visible = true;
            //pl_self_item.Visible = true;
            Page_update();
        }

        // sam 差假決行層級規定
        //簽核的gv
        protected void gv_data_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //0808 DEAN
            DataTable gv_data_temp = new DataTable();
            if (this.MODE == Const_Mode.差假線上簽核)//差假簽核
            {
                //20140620

                if (!(lbtn_sign.Enabled == false))
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        LinkButton myhlink1 = (LinkButton)e.Row.FindControl("hlink1");
                        gv_data_temp = o_DBFactory.ABC_toTest.Create_Table("select * from C_DLTB01 WHERE MZ_DLTB01_SN =" + myhlink1.CommandArgument, "A");
                        mz_date = gv_data_temp.Rows[i]["MZ_IDATE1"].ToString().Substring(0, 3) + "/" + gv_data_temp.Rows[i]["MZ_IDATE1"].ToString().Substring(3, 2) + "/" + gv_data_temp.Rows[i]["MZ_IDATE1"].ToString().Substring(5, 2);

                        string exad = o_A_KTYPE.RAD(e.Row.Cells[3].Text);
                        e.Row.Cells[3].Text = exad == "新北市政府警察局" ? "警察局" : exad.Replace("新北市政府警察局", "");
                        e.Row.Cells[4].Text = o_A_KTYPE.RUNIT(e.Row.Cells[4].Text);

                        Session.Add("SAM20200325", e.Row.Cells[3].Text);

                        myhlink1.Text = mz_date.ToString();
                        ((System.Web.UI.WebControls.HyperLink)e.Row.Cells[7].FindControl("HyperLink1")).Visible = (!string.IsNullOrEmpty(gv_data_temp.Rows[i]["MZ_FILE"].ToString()));
                        ((System.Web.UI.WebControls.HyperLink)e.Row.Cells[7].FindControl("HyperLink2")).Visible = (!string.IsNullOrEmpty(gv_data_temp.Rows[i]["MZ_FILE1"].ToString()));
                        ((System.Web.UI.WebControls.HyperLink)e.Row.Cells[7].FindControl("HyperLink3")).Visible = (!string.IsNullOrEmpty(gv_data_temp.Rows[i]["MZ_FILE2"].ToString()));


                    }
                }
            }
            else if (this.MODE == Const_Mode.代理人簽核)//代理人
            {

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    LinkButton myhlink1 = (LinkButton)e.Row.FindControl("hlink1");
                    gv_data_temp = o_DBFactory.ABC_toTest.Create_Table("select * from C_DLTB01 WHERE MZ_DLTB01_SN =" + myhlink1.CommandArgument, "A");

                    //請假的日期，顯示為 年/月/日
                    agents_date = gv_data_temp.Rows[iAgents]["MZ_IDATE1"].ToString().Substring(0, 3) + "/" +
                        gv_data_temp.Rows[iAgents]["MZ_IDATE1"].ToString().Substring(3, 2) + "/" +
                        gv_data_temp.Rows[iAgents]["MZ_IDATE1"].ToString().Substring(5, 2);

                    string exad = o_A_KTYPE.RAD(e.Row.Cells[3].Text);
                    e.Row.Cells[3].Text = exad == "新北市政府警察局" ? "警察局" : exad.Replace("新北市政府警察局", "");
                    e.Row.Cells[4].Text = o_A_KTYPE.RUNIT(e.Row.Cells[4].Text);

                    myhlink1.Text = agents_date.ToString();
                    ((System.Web.UI.WebControls.HyperLink)e.Row.Cells[7].FindControl("HyperLink1")).Visible = (!string.IsNullOrEmpty(gv_data_temp.Rows[iAgents]["MZ_FILE"].ToString()));
                    ((System.Web.UI.WebControls.HyperLink)e.Row.Cells[7].FindControl("HyperLink2")).Visible = (!string.IsNullOrEmpty(gv_data_temp.Rows[iAgents]["MZ_FILE1"].ToString()));
                    ((System.Web.UI.WebControls.HyperLink)e.Row.Cells[7].FindControl("HyperLink3")).Visible = (!string.IsNullOrEmpty(gv_data_temp.Rows[iAgents]["MZ_FILE2"].ToString()));


                }
            }
            else if (this.MODE == Const_Mode.已決行案件)//已決行案件
            {

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    LinkButton myhlink1 = (LinkButton)e.Row.FindControl("hlink1");
                    gv_data_temp = o_DBFactory.ABC_toTest.Create_Table("select * from C_DLTB01 WHERE MZ_DLTB01_SN =" + myhlink1.CommandArgument, "A");

                    //請假的日期顯示
                    person_date = gv_data_temp.Rows[iPerson]["MZ_IDATE1"].ToString().Substring(0, 3) + "/" +
                        gv_data_temp.Rows[iPerson]["MZ_IDATE1"].ToString().Substring(3, 2) + "/" +
                       gv_data_temp.Rows[iPerson]["MZ_IDATE1"].ToString().Substring(5, 2);

                    string exad = o_A_KTYPE.RAD(e.Row.Cells[3].Text);
                    e.Row.Cells[3].Text = exad == "新北市政府警察局" ? "警察局" : exad.Replace("新北市政府警察局", "");
                    e.Row.Cells[4].Text = o_A_KTYPE.RUNIT(e.Row.Cells[4].Text);

                    myhlink1.Text = person_date.ToString();
                    ((System.Web.UI.WebControls.HyperLink)e.Row.Cells[7].FindControl("HyperLink1")).Visible = (!string.IsNullOrEmpty(gv_data_temp.Rows[iPerson]["MZ_FILE"].ToString()));
                    ((System.Web.UI.WebControls.HyperLink)e.Row.Cells[7].FindControl("HyperLink2")).Visible = (!string.IsNullOrEmpty(gv_data_temp.Rows[iPerson]["MZ_FILE1"].ToString()));
                    ((System.Web.UI.WebControls.HyperLink)e.Row.Cells[7].FindControl("HyperLink3")).Visible = (!string.IsNullOrEmpty(gv_data_temp.Rows[iPerson]["MZ_FILE2"].ToString()));


                }
            }
            else if (this.MODE == Const_Mode.簽核狀態 || this.MODE == Const_Mode.查詢簽核狀態 || this.MODE == Const_Mode.已核定假單)//查詢簽核狀態 ||  || //已核定 系列
            {
                if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[0].Attributes.Add("style", "display:none");
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    LinkButton myhlink1 = (LinkButton)e.Row.FindControl("hlink1");

                    gv_data_temp = o_DBFactory.ABC_toTest.Create_Table("select * from C_DLTB01 WHERE MZ_DLTB01_SN =" + myhlink1.CommandArgument, "A");

                    //請假的日期顯示
                    person_date = gv_data_temp.Rows[0]["MZ_IDATE1"].ToString().Substring(0, 3) + "/" +
                        gv_data_temp.Rows[0]["MZ_IDATE1"].ToString().Substring(3, 2) + "/" +
                        gv_data_temp.Rows[0]["MZ_IDATE1"].ToString().Substring(5, 2);

                    myhlink1.Text = person_date.ToString();
                    ((System.Web.UI.WebControls.HyperLink)e.Row.Cells[7].FindControl("HyperLink1")).Visible = (!string.IsNullOrEmpty(gv_data_temp.Rows[0]["MZ_FILE"].ToString()));
                    ((System.Web.UI.WebControls.HyperLink)e.Row.Cells[7].FindControl("HyperLink2")).Visible = (!string.IsNullOrEmpty(gv_data_temp.Rows[0]["MZ_FILE1"].ToString()));
                    ((System.Web.UI.WebControls.HyperLink)e.Row.Cells[7].FindControl("HyperLink3")).Visible = (!string.IsNullOrEmpty(gv_data_temp.Rows[0]["MZ_FILE2"].ToString()));


                    string exad = o_A_KTYPE.RAD(e.Row.Cells[3].Text);
                    e.Row.Cells[3].Text = exad == "新北市政府警察局" ? "警察局" : exad.Replace("新北市政府警察局", "");
                    e.Row.Cells[4].Text = o_A_KTYPE.RUNIT(e.Row.Cells[4].Text);

                }

            }
            else if (this.MODE == Const_Mode.抽回差假簽核)//假單簽核歷程抽回
            {
                DataTable temp = new DataTable();

                CSonline = new CSonline("reSearch", SessionHelper.ADPMZ_ID);
                temp = CSonline.getDate();

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    LinkButton myhlink1 = (LinkButton)e.Row.FindControl("hlink1");

                    gv_data_temp = o_DBFactory.ABC_toTest.Create_Table("select * from C_DLTB01 WHERE MZ_DLTB01_SN =" + myhlink1.CommandArgument, "A");

                    //請假的日期顯示
                    person_date = gv_data_temp.Rows[0]["MZ_IDATE1"].ToString().Substring(0, 3) + "/" +
                        gv_data_temp.Rows[0]["MZ_IDATE1"].ToString().Substring(3, 2) + "/" +
                        gv_data_temp.Rows[0]["MZ_IDATE1"].ToString().Substring(5, 2);

                    myhlink1.Text = person_date.ToString();
                    ((System.Web.UI.WebControls.HyperLink)e.Row.Cells[7].FindControl("HyperLink1")).Visible = (!string.IsNullOrEmpty(gv_data_temp.Rows[0]["MZ_FILE"].ToString()));
                    ((System.Web.UI.WebControls.HyperLink)e.Row.Cells[7].FindControl("HyperLink2")).Visible = (!string.IsNullOrEmpty(gv_data_temp.Rows[0]["MZ_FILE1"].ToString()));
                    ((System.Web.UI.WebControls.HyperLink)e.Row.Cells[7].FindControl("HyperLink3")).Visible = (!string.IsNullOrEmpty(gv_data_temp.Rows[0]["MZ_FILE2"].ToString()));
                    //myhlink1.Enabled = false;
                    //iFLOW++;
                    string exad = o_A_KTYPE.RAD(e.Row.Cells[3].Text);
                    e.Row.Cells[3].Text = exad == "新北市政府警察局" ? "警察局" : exad.Replace("新北市政府警察局", "");
                    e.Row.Cells[4].Text = o_A_KTYPE.RUNIT(e.Row.Cells[4].Text);
                }
            }
        }

        //歷程的gv
        protected void gv_history_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //0808→DEAN
            DataTable tempHistory = new DataTable();
            tempHistory = HistorySearch();
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label date = (Label)e.Row.FindControl("lbl_history_date");
                if (temp_SharedDT.Rows[iHistory]["PROCESS_DATE"].ToString() == "")
                {
                    date.Text = "尚未批";
                }
                else
                {
                    history_date = tempHistory.Rows[iHistory]["PROCESS_DATE"].ToString().Substring(0, 3) + "/" +
                                   tempHistory.Rows[iHistory]["PROCESS_DATE"].ToString().Substring(3, 2) + "/" +
                                   tempHistory.Rows[iHistory]["PROCESS_DATE"].ToString().Substring(5, 2) + " " +
                                   tempHistory.Rows[iHistory]["PROCESS_TIME"].ToString();
                    date.Text = history_date.ToString();

                }
                iHistory++;
                string exad = o_A_KTYPE.RAD(e.Row.Cells[1].Text);
                e.Row.Cells[1].Text = exad == "新北市政府警察局" ? "警察局" : exad.Replace("新北市政府警察局", "");
                e.Row.Cells[2].Text = o_A_KTYPE.RUNIT(e.Row.Cells[2].Text);
                e.Row.Cells[3].Text = o_A_KTYPE.CODE_TO_NAME(e.Row.Cells[3].Text, "26");
            }
        }

        //gv_data的差假內容
        protected void lbtn_content(object sender, CommandEventArgs e)
        {
            pl_Show.GroupingText = "差假內容";
            DataTable tempShow = new DataTable();
            //這筆未簽核的詳細假單內容(含代理人，利用SN查詢)
            tempShow = Service_C_LEAVE_HISTORY.AllSearch(this.MODE, (String)e.CommandArgument);
            if (tempShow.Rows.Count > 0)
            {
                lbl_MZ_NAME.Text = tempShow.Rows[0]["MZ_NAME"].ToString();
                lbl_MZ_CNAME.Text = tempShow.Rows[0]["MZ_CNAME"].ToString();
                lbl_MZ_EXAD.Text = o_A_KTYPE.RAD(tempShow.Rows[0]["MZ_EXAD"].ToString());
                lbl_MZ_EXUNIT.Text = o_A_KTYPE.RUNIT(tempShow.Rows[0]["MZ_EXUNIT"].ToString());
                string Show_Date = tempShow.Rows[0]["MZ_IDATE1"].ToString() + tempShow.Rows[0]["MZ_ITIME1"] +
                        tempShow.Rows[0]["MZ_ODATE"].ToString() + tempShow.Rows[0]["MZ_OTIME"].ToString() +
                        tempShow.Rows[0]["MZ_TDAY"].ToString() + tempShow.Rows[0]["MZ_TTIME"].ToString();
                lbl_Date.Text = DateAndTime(tempShow);
                lbl_MZ_CAUSE.Text = tempShow.Rows[0]["MZ_CAUSE"].ToString();
                //0812→Dean
                string MZ_ID = tempShow.Rows[0]["MZ_ID"].ToString();

                var h_SQL = "SELECT SUM(MZ_TDAY)-SUM(ORGINAL_MZ_TDAY-NEW_MZ_TDAY) FROM C_DLTB01 left join C_CHANGE_DLTB01 on C_DLTB01.MZ_DLTB01_SN=C_CHANGE_DLTB01.MZ_DLTB01_SN and CHANGE_DLTB01_STATUS='Y' WHERE MZ_CODE='03' AND MZ_ID='" + MZ_ID + "' AND dbo.SUBSTR(MZ_IDATE1,1,3)='" + (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0') + "'";
                int hday = int.Parse(string.IsNullOrEmpty(o_DBFactory.ABC_toTest.vExecSQL(h_SQL)) ? "0" : o_DBFactory.ABC_toTest.vExecSQL(h_SQL));

                //var t_SQL = "SELECT SUM(MZ_TTIME)-SUM(ORGINAL_MZ_TTIME-NEW_MZ_TTIME) FROM C_DLTB01 left join C_CHANGE_DLTB01 on C_DLTB01.MZ_DLTB01_SN=C_CHANGE_DLTB01.MZ_DLTB01_SN and CHANGE_DLTB01_STATUS='Y' WHERE MZ_CODE='03' AND MZ_ID='" + MZ_ID + "' AND dbo.SUBSTR(MZ_IDATE1,1,3)='" + (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0') + "'";
                var t_SQL = "SELECT SUM(MZ_TTIME) FROM C_DLTB01 WHERE MZ_CODE='03' AND MZ_ID='" + MZ_ID + "' AND dbo.SUBSTR(MZ_IDATE1,1,3)='" + (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0') + "'";
                int htime = int.Parse(string.IsNullOrEmpty(o_DBFactory.ABC_toTest.vExecSQL(t_SQL)) ? "0" : o_DBFactory.ABC_toTest.vExecSQL(t_SQL));
                string year = tempShow.Rows[0]["MZ_SYSDAY"].ToString().Substring(0, 3);
                string HdayInfo = o_DBFactory.ABC_toTest.vExecSQL(string.Format("Select MZ_HDAY + '日' + MZ_HTIME + '時' from C_DLTBB WHERE MZ_ID='{0}' and  MZ_YEAR={1}", MZ_ID, year));

                //irk 2012/10/26 新增
                lblMZ__HDAY.Text = HdayInfo;
                lblMZ_ID.Text = MZ_ID;
                lblADDDATE.Text = string.Format("{0} {1}", tempShow.Rows[0]["MZ_SYSDAY"].ToString(), tempShow.Rows[0]["MZ_SYSTIME"].ToString());
                lblOccc.Text = o_A_KTYPE.CODE_TO_NAME(tempShow.Rows[0]["MZ_OCCC"].ToString(), "26");
                lblPox.Text = tempShow.Rows[0]["MZ_CNAME"].ToString();
                DataTable dt = o_DBFactory.ABC_toTest.Create_Table(string.Format("SELECT NVL(SUM(MZ_TDAY),0) DAY,NVL(SUM(MZ_TTIME),0) TIME FROM C_DLTB01 WHERE MZ_CHK1='Y' AND MZ_IDATE1>='{0}0101' AND MZ_IDATE1<='{0}1231' AND MZ_ID='{1}' AND MZ_CODE='{2}'", year, MZ_ID, tempShow.Rows[0]["MZ_CODE"].ToString()), "GET");
                int b_day = 0;
                int b_hour = 0;
                int b_total = 0;
                int s_day = 0;
                int s_hour = 0;
                if (dt.Rows.Count > 0)
                {
                    b_day = int.Parse(dt.Rows[0]["DAY"].ToString());
                    b_hour = int.Parse(dt.Rows[0]["TIME"].ToString());
                    b_total = b_day * 8 + b_hour;
                    s_day = b_total / 8;
                    s_hour = b_total % 8;
                }
                lbOtherDate.Text = string.Format("{0}日{1}時", s_day, s_hour);


                int hday_used;

                int htime_used;

                if (htime != 0)
                {
                    int hday_Add = htime / 8;
                    hday_used = hday + hday_Add;
                    htime_used = htime % 8;
                }
                else
                {
                    hday_used = hday;
                    htime_used = 0;
                }
                lbl_QKDay.Text = hday_used.ToString() + "天" + htime_used.ToString() + "時";//已休假天數
                lbl_agent.Text = tempShow.Rows[0]["MZ_RNAME"].ToString();//代理人
                lbl_location.Text = tempShow.Rows[0]["MZ_TADD"].ToString();//休假地點
                lbl_allowance.Text = tempShow.Rows[0]["MZ_SWT"].ToString() == "Y" ? "是" : "否";//是否有申請補助
                lbl_abroad.Text = tempShow.Rows[0]["MZ_FOREIGN"].ToString() == "Y" ? "出國、" : "" +
                                  tempShow.Rows[0]["MZ_CHINA"].ToString() == "Y" ? "大陸、" : "";//是否為出國、赴大陸
                lbl_abroad.Text = lbl_abroad.Text == "" ? "否" : lbl_abroad.Text.Substring(0, lbl_abroad.Text.Length - 1);
                //顯示判定
                pl_history.Visible = false;
                pl_overtime_history.Visible = false;
                P_E_ModalPopupExtender.Show();
                gv_Business_Detail.Visible = false;
                pl_change_dltb01_show.Visible = false;
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "alert('該假單尚未進行線上簽核，請送至審核')", true);
            }
        }


    }
}
