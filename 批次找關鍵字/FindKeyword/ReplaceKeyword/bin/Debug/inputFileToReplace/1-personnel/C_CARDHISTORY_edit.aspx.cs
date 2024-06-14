using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace TPPDDB._15_score
{
    /// <summary>
    /// 教師管理
    /// </summary>
    public partial class C_CARDHISTORY_edit : o_score_Function
    {
        string _strGID
        {
            //根本不能用 這個功能權限是C_strGID 可能是抄錯
            get { return ViewState["O_strGID"] != null ? ViewState["O_strGID"].ToString() : string.Empty; }
            set { ViewState["O_strGID"] = value; }
        }
        string isAdmin = "N";
        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (!IsPostBack)
            {
                // 一般權限檢查
                chk_TPMPermissions();

                // 不用檢查群組權限
                //_strGID = chk_TPMGroup();
                //人事的功能要抓MZ_POWER
                _strGID = o_a_Function.strGID(Session["ADPMZ_ID"].ToString());
            }
            //O.fill_AD(DropDownList_AD);
            if (!Page.IsPostBack)
            {
                txt_Date.Text = DateTime.Now.Year.ToString().PadLeft(3, '0') + "/" + DateTime.Now.Month.ToString("00") + "/" + DateTime.Now.Day.ToString("00");
                group_control(_strGID);
                DropDownList_MZ_AD.DataBind();
                DropDownList_MZ_AD_SelectedIndexChanged(sender, e);
                Time_Changed();
                string strSQL = "SELECT * FROM TP_MODEL_MEMBER where tpmn_gid in ('1901','1902','1909') and TPMID='" + Session["TPM_MID"] + "'";
                DataTable dttemp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
                dttemp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "REPORT");
                if (dttemp.Rows.Count > 0)
                {
                    isAdmin = "Y";
                }
                if (Request.QueryString["SN"] != null && Request.QueryString["SN"].ToString() != "")
                {
                    btInsert.Text = "修改";
                    preLoad(Request.QueryString["SN"].ToString());
                }
                else
                {
                    string NowYear = DateTime.Now.Year.ToString();
                    string SQL = "SELECT * FROM C_CARDHISTORY_EDIT WHERE  MZ_ID='" + Session["ADPMZ_ID"].ToString() + "' and DATETIME like '" + NowYear + "%' order by DATETIME desc";
                    DataTable dt_tmp = o_DBFactory.ABC_toTest.Create_Table(SQL, "GET");
                    gv_C_CARDHISTORY_EDIT.DataSource = dt_tmp;
                    gv_C_CARDHISTORY_EDIT.DataBind();
                }
            }

            string sql = "select count(*) as count1 from C_CARDHISTORY_EDIT where MZ_ID = '" + DropDownList_NAME.SelectedValue.ToString() + "' and CTYPE = '1' and (TYPE = '上班' or TYPE = '下班') and STATUS <> '0' and Datetime like '" + DateTime.Now.Year.ToString() + "%'";
            string count1 = o_DBFactory.ABC_toTest.vExecSQL(sql);
            sql = "select count(*) as count2 from C_CARDHISTORY_EDIT where MZ_ID = '" + DropDownList_NAME.SelectedValue.ToString() + "' and CTYPE = '1' and TYPE = '上下班' and STATUS <> '0' and Datetime like '" + DateTime.Now.Year.ToString() + "%'";
            string count2 = o_DBFactory.ABC_toTest.vExecSQL(sql);
            if (rd_CTYPE.SelectedValue == "1")
            { aacount1.Visible = true; }
            else
            {
                aacount1.Visible = false;
            }

            aacount1.InnerHtml = "<font color=red>本年度剩" + (12 - (int.Parse(count1) + int.Parse(count2) * 2)).ToString() + "次申請忘記刷卡</font>";
            //string sql1 = "select count(INTIME) as count1 from C_CARDHISTORY_EDIT where MZ_ID = '" + DropDownList_NAME.SelectedValue.ToString() + "' and CTYPE = '1'  and Datetime like '" + DateTime.Now.Year.ToString() + "%'";
        }

        public void get_usecount()
        {

            string NowYear = DateTime.Now.Year.ToString();
            string SQL = "SELECT * FROM C_CARDHISTORY_EDIT WHERE  MZ_ID='" + Session["ADPMZ_ID"].ToString() + "' and DATETIME like '" + NowYear + "%' order by DATETIME desc";
            DataTable dt_tmp = o_DBFactory.ABC_toTest.Create_Table(SQL, "GET");
            gv_C_CARDHISTORY_EDIT.DataSource = dt_tmp;
            gv_C_CARDHISTORY_EDIT.DataBind();

            string sql = "select count(*) as count1 from C_CARDHISTORY_EDIT where MZ_ID = '" + DropDownList_NAME.SelectedValue.ToString() + "' and CTYPE = '1' and (TYPE = '上班' or TYPE = '下班') and STATUS <> '0' and Datetime like '" + DateTime.Now.Year.ToString() + "%'";
            string count1 = o_DBFactory.ABC_toTest.vExecSQL(sql);
            sql = "select count(*) as count2 from C_CARDHISTORY_EDIT where MZ_ID = '" + DropDownList_NAME.SelectedValue.ToString() + "' and CTYPE = '1' and TYPE = '上下班' and STATUS <> '0' and Datetime like '" + DateTime.Now.Year.ToString() + "%'";
            string count2 = o_DBFactory.ABC_toTest.vExecSQL(sql);
            if (rd_CTYPE.SelectedValue == "1")
            { aacount1.Visible = true; }
            else
            {
                aacount1.Visible = false;
            }

            aacount1.InnerHtml = "<font color=red>本年度剩" + (12 - (int.Parse(count1) + int.Parse(count2) * 2)).ToString() + "次申請忘記刷卡</font>";
        }

        protected void txt_Date_TextChanged(object sender, EventArgs e)
        {
            Time_Changed();
        }
        private void Time_Changed()
        {
            string MZ_ID = DropDownList_NAME.SelectedValue;
            string LOGDATE = txt_Date.Text.Trim().ToString();
            string MZ_OCCC = "";
            string SQL = "SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=(select MZ_OCCC from a_dlbase where MZ_ID='" + MZ_ID + "') AND MZ_KTYPE='26'";
            DataTable temp = o_DBFactory.ABC_toTest.Create_Table(SQL, "GET");
            if (temp.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(temp.Rows[0][0].ToString()))
                {
                    TextBox_MZ_OCCC.Text = temp.Rows[0][0].ToString();
                }
            }
            if (MZ_ID != "" && LOGDATE != "")
            {
                if (DropDownList_Type.SelectedValue != "上班" && DropDownList_Type.SelectedValue != "上下班")
                {
                    string OverINTIME = o_DBFactory.ABC_toTest.vExecSQL("SELECT LOGTIME FROM (SELECT LOGTIME,ROW_NUMBER() OVER(ORDER BY LOGTIME) AS ROWCOUNT FROM C_CARDHISTORY_NEW WHERE dbo.SUBSTR(USERNAME,0,10)='" + MZ_ID + "' AND LOGDATE='" + LOGDATE + "' AND VERIFY='IN') WHERE ROWCOUNT=1"); //計算當天最早刷卡時間
                    Text_INTIME.Text = OverINTIME;
                }
                else
                {
                    Text_INTIME.Text = "09:00:00";
                }
                if (DropDownList_Type.SelectedValue != "下班" && DropDownList_Type.SelectedValue != "上下班")
                {
                    string OverOUTTIME = o_DBFactory.ABC_toTest.vExecSQL("SELECT LOGTIME FROM (SELECT LOGTIME,ROW_NUMBER() OVER(ORDER BY LOGTIME DESC) AS ROWCOUNT FROM C_CARDHISTORY_NEW WHERE dbo.SUBSTR(USERNAME,0,10)='" + MZ_ID + "' AND LOGDATE='" + LOGDATE + "' AND VERIFY='IN') WHERE ROWCOUNT=1"); //計算當天最晚刷卡時間
                    Text_OUTTIME.Text = OverOUTTIME;
                }
                else
                {
                    Text_OUTTIME.Text = "18:00:00";
                }
            }
        }
        private void group_control(string strGID)
        {
            switch (strGID)
            {
                case "A":
                    break;
                case "B":
                    //DropDownList_MZ_AD.DataBind();
                    //DropDownList_MZ_AD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                    //DropDownList_MZ_AD.Enabled = false;
                    break;
                case "C":
                    DropDownList_MZ_AD.DataBind();
                    DropDownList_MZ_AD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                    DropDownList_MZ_AD.Enabled = false;
                    break;
                case "D":
                    DropDownList_MZ_AD.DataBind();
                    DropDownList_MZ_AD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                    DropDownList_MZ_AD.Enabled = false;
                    Select_UNIT();
                    DropDownList_MZ_UNIT.SelectedValue = Session["ADPMZ_EXUNIT"].ToString();
                    DropDownList_MZ_UNIT.Enabled = false;
                    Select_Name();
                    DropDownList_NAME.SelectedValue = Session["ADPMZ_ID"].ToString();
                    Time_Changed();
                    GV_CHECK_init();
                    break;
                case "E":
                    DropDownList_MZ_AD.DataBind();
                    DropDownList_MZ_AD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                    DropDownList_MZ_AD.Enabled = false;
                    Select_UNIT();
                    DropDownList_MZ_UNIT.SelectedValue = Session["ADPMZ_EXUNIT"].ToString();
                    DropDownList_MZ_UNIT.Enabled = false;
                    Select_Name();
                    DropDownList_NAME.SelectedValue = Session["ADPMZ_ID"].ToString();
                    //DropDownList_NAME.Enabled = false;
                    Time_Changed();
                    GV_CHECK_init();
                    break;
                default:
                    DropDownList_MZ_AD.DataBind();
                    DropDownList_MZ_AD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                    DropDownList_MZ_AD.Enabled = false;
                    Select_UNIT();
                    DropDownList_MZ_UNIT.SelectedValue = Session["ADPMZ_EXUNIT"].ToString();
                    DropDownList_MZ_UNIT.Enabled = false;
                    Select_Name();
                    DropDownList_NAME.SelectedValue = Session["ADPMZ_ID"].ToString();
                    DropDownList_NAME.Enabled = false;
                    Time_Changed();
                    GV_CHECK_init();
                    break;
            }
        }

        //儲存結果
        protected void btn_Save_Click(object sender, EventArgs e)
        {
            string error = "";
            if (string.IsNullOrEmpty(txt_Date.Text))
            {
                error += "日期不能為空白\n";
            }

            if (!string.IsNullOrEmpty(error))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('" + error + "');", true);
            }
            else
            {
                string MZ_OCCC = "";
                string SQL = "SELECT MZ_KCODE FROM A_KTYPE WHERE MZ_KCHI='" + TextBox_MZ_OCCC.Text + "' AND MZ_KTYPE='26'";
                DataTable temp = o_DBFactory.ABC_toTest.Create_Table(SQL, "GET");
                if (temp.Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(temp.Rows[0][0].ToString()))
                    {
                        MZ_OCCC = temp.Rows[0][0].ToString();
                    }
                }
                SqlParameter[] parameterList = { 
                            new SqlParameter("MZ_AD", SqlDbType.NVarChar){Value = DropDownList_MZ_AD.SelectedValue},
                            new SqlParameter("MZ_OCCC", SqlDbType.NVarChar){Value = MZ_OCCC},
                            new SqlParameter("MZ_UNIT", SqlDbType.NVarChar){Value = DropDownList_MZ_UNIT.SelectedValue},
                            new SqlParameter("YEAR", SqlDbType.NVarChar){Value = txt_Date.Text},
                            };
                try
                {
                    //                    string sn = o_DBFactory.ABC_toTest.vExecSQL("SELECT SN FROM O_TRAINING_RESULTS WHERE MZ_ID = '" + txt_MZ_ID.Text + "' and YEAR = '" + txt_Year.Text + "' and YEAR_INTERVAL = '" + ddl_Year_Interval.SelectedValue + "'");
                    //                    if (string.IsNullOrEmpty(sn))
                    //                    {
                    //                        string strSQL = @"INSERT INTO O_TRAINING_RESULTS (SN,MZ_ID,MZ_NAME,MZ_BIR,MZ_AD,MZ_UNIT,MZ_OCCC,MZ_SEX,YEAR,YEAR_INTERVAL,TRANSCRIPTS_1,TRANSCRIPTS_1_1,TRANSCRIPTS_1_2,REMARK_1,TRANSCRIPTS_2,TRANSCRIPTS_2_1,TRANSCRIPTS_2_2,REMARK_2,TRANSCRIPTS_TIME_2,TRANSCRIPTS_TIME_2_1,TRANSCRIPTS_TIME_2_2,TRANSCRIPTS_3,TRANSCRIPTS_3_1,TRANSCRIPTS_3_2,REMARK_3,TRANSCRIPTS_4,TRANSCRIPTS_4_1,TRANSCRIPTS_4_2,REMARK_4,TRANSCRIPTS_5,TRANSCRIPTS_5_1,TRANSCRIPTS_5_2,REMARK_5,TRANSCRIPTS_6,TRANSCRIPTS_6_1,TRANSCRIPTS_6_2,REMARK_6)
                    //                           VALUES ( NEXT VALUE FOR dbo.O_TRAINING_RESULTS_SN,@MZ_ID,@MZ_NAME,@MZ_BIR,@MZ_AD,@MZ_UNIT,@MZ_OCCC,@MZ_SEX,@YEAR,@YEAR_INTERVAL,@TRANSCRIPTS_1,@TRANSCRIPTS_1_1,@TRANSCRIPTS_1_2,@REMARK_1,@TRANSCRIPTS_2,@TRANSCRIPTS_2_1,@TRANSCRIPTS_2_2,@REMARK_2,@TRANSCRIPTS_TIME_2,@TRANSCRIPTS_TIME_2_1,@TRANSCRIPTS_TIME_2_2,@TRANSCRIPTS_3,@TRANSCRIPTS_3_1,@TRANSCRIPTS_3_2,@REMARK_3,@TRANSCRIPTS_4,@TRANSCRIPTS_4_1,@TRANSCRIPTS_4_2,@REMARK_4,@TRANSCRIPTS_5,@TRANSCRIPTS_5_1,@TRANSCRIPTS_5_2,@REMARK_5,@TRANSCRIPTS_6,@TRANSCRIPTS_6_1,@TRANSCRIPTS_6_2,@REMARK_6) ";

                    //                        o_DBFactory.ABC_toTest.ExecuteNonQuery( strSQL, parameterList);
                    //                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('新增成功');", true);
                    //                    }
                    //                    else
                    //                    {
                    //                        List<SqlParameter> tmp = parameterList.ToList();
                    //                        tmp.Add(new SqlParameter("SN", SqlDbType.NVarChar) { Value = sn });
                    //                        parameterList = tmp.ToArray();

                    //                        string strSQL = @"UPDATE O_TRAINING_RESULTS SET MZ_ID = @MZ_ID, MZ_NAME = @MZ_NAME, MZ_BIR=@MZ_BIR,MZ_AD=@MZ_AD,MZ_UNIT=@MZ_UNIT,MZ_OCCC=@MZ_OCCC,MZ_SEX=@MZ_SEX,
                    //                                        YEAR=@YEAR, YEAR_INTERVAL=@YEAR_INTERVAL,
                    //                                        TRANSCRIPTS_1=@TRANSCRIPTS_1,TRANSCRIPTS_1_1=@TRANSCRIPTS_1_1,TRANSCRIPTS_1_2=@TRANSCRIPTS_1_2,REMARK_1=@REMARK_1,
                    //                                        TRANSCRIPTS_2=@TRANSCRIPTS_2,TRANSCRIPTS_2_1=@TRANSCRIPTS_2_1,TRANSCRIPTS_2_2=@TRANSCRIPTS_2_2,REMARK_2=@REMARK_2,
                    //                                        TRANSCRIPTS_TIME_2=@TRANSCRIPTS_TIME_2,TRANSCRIPTS_TIME_2_1=@TRANSCRIPTS_TIME_2_1,TRANSCRIPTS_TIME_2_2=@TRANSCRIPTS_TIME_2_2,
                    //                                        TRANSCRIPTS_3=@TRANSCRIPTS_3,TRANSCRIPTS_3_1=@TRANSCRIPTS_3_1,TRANSCRIPTS_3_2=@TRANSCRIPTS_3_2,REMARK_3=@REMARK_3,
                    //                                        TRANSCRIPTS_4=@TRANSCRIPTS_4,TRANSCRIPTS_4_1=@TRANSCRIPTS_4_1,TRANSCRIPTS_4_2=@TRANSCRIPTS_4_2,REMARK_4=@REMARK_4,
                    //                                        TRANSCRIPTS_5=@TRANSCRIPTS_5,TRANSCRIPTS_5_1=@TRANSCRIPTS_5_1,TRANSCRIPTS_5_2=@TRANSCRIPTS_5_2,REMARK_5=@REMARK_5,
                    //                                        TRANSCRIPTS_6=@TRANSCRIPTS_6,TRANSCRIPTS_6_1=@TRANSCRIPTS_6_1,TRANSCRIPTS_6_2=@TRANSCRIPTS_6_2,REMARK_6=@REMARK_6
                    //                                    WHERE SN=@SN";

                    //                        o_DBFactory.ABC_toTest.ExecuteNonQuery( strSQL, parameterList);
                    //                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('更新成功');", true);
                    //                    }
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('更新失敗');", true);
                }
            }
        }
        //取消
        protected void btn_cancel_Click(object sender, EventArgs e)
        {

        }

        void preLoad(string SN)
        {
            ViewState["SN"] = SN;
            string strSQL = string.Format(@"SELECT * FROM C_CARDHISTORY_EDIT WHERE SN='{0}'", SN);

            DataTable dt = new DataTable();

            dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");

            if (dt.Rows.Count > 0)
            {
                //txt_Date.Text = DateTime.ParseExact(dt.Rows[0]["DATETIME"].ToString(), "dd-MMM-yy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy/MM/dd");
                txt_Date.Text = DateTime.Parse(dt.Rows[0]["DATETIME"].ToString()).ToString("yyyy/MM/dd");
                DropDownList_Type.SelectedValue = dt.Rows[0]["Type"].ToString();
                DropDownList_MZ_AD.SelectedValue = dt.Rows[0]["MZ_AD"].ToString();
                ChangeUnit(DropDownList_MZ_AD, DropDownList_MZ_UNIT);
                DropDownList_MZ_UNIT.SelectedValue = dt.Rows[0]["MZ_UNIT"].ToString();
                TextBox_MZ_OCCC.Text = dt.Rows[0]["MZ_OCCC"].ToString();
                Select_Name();
                DropDownList_NAME.SelectedValue = dt.Rows[0]["MZ_ID"].ToString();
                C_CARDHISTORY_EDIT_Init();
                Text_INTIME.Text = dt.Rows[0]["INTIME"].ToString();
                Text_OUTTIME.Text = dt.Rows[0]["OUTTIME"].ToString();
                if (dt.Rows[0]["CHECKER"].ToString() != "")
                { GV_CHECK_init(); }              
                ddl_CHECKER.SelectedValue = dt.Rows[0]["CHECKER"].ToString();
                Text_MEMO.Text = dt.Rows[0]["MEMO"].ToString();
                //產生CTYPE的呈現 Mark 20210602;
                rd_CTYPE.SelectedValue = dt.Rows[0]["CTYPE"].ToString();
                if (rd_CTYPE.SelectedValue == "0")
                {
                    aacount1.Visible = false;
                    //radl_Check.Text = "";
                    //radl_Check.SelectedValue = "";
                    radl_Check.ClearSelection();
                    radl_Check.Enabled = true;

                    //判斷紙本或線上
                    if (ddl_CHECKER.SelectedValue != "")
                    {   GV_CHECK_init();
                        radl_Check.SelectedValue = "Online";
                    }
                    else
                    {   ddl_CHECKER.Enabled = false;
                        radl_Check.SelectedValue = "Paper";
                    }                    

                    Text_INTIME.Enabled = false;
                    Text_OUTTIME.Enabled = false;
                }
                else
                {
                    aacount1.Visible = true;
                    radl_Check.ClearSelection();
                    radl_Check.Enabled = true;
                    ddl_CHECKER.Enabled = true;
                    //判斷紙本或線上
                    if (ddl_CHECKER.SelectedValue != "")
                    {
                        GV_CHECK_init();
                        radl_Check.SelectedValue = "Online";
                    }
                    else
                    {
                        ddl_CHECKER.Enabled = false;
                        radl_Check.SelectedValue = "Paper";
                    }

                    if (DropDownList_Type.SelectedValue == "上班")
                    {
                        Text_INTIME.Enabled = true;
                        Text_OUTTIME.Enabled = false;
                    }
                    else if (DropDownList_Type.SelectedValue == "下班")
                    {
                        Text_INTIME.Enabled = false;
                        Text_OUTTIME.Enabled = true;
                    }
                    else if (DropDownList_Type.SelectedValue == "上下班")
                    {
                        Text_INTIME.Enabled = true;
                        Text_OUTTIME.Enabled = true;
                    }
                }

                //HyperLink_FILENAME1.NavigateUrl = Find_Url(SN, "FILE1");
                //HyperLink_FILENAME2.NavigateUrl = Find_Url(SN, "FILE2");
                string path1 = dt.Rows[0]["FILE1"].ToString();
                string path2 = dt.Rows[0]["FILE2"].ToString();

                if (path1 != "")
                {
                    HyperLink_FILENAME1.Visible = true;
                    HyperLink_FILENAME1.NavigateUrl = show_upload_path() + path1;
                    Button_DelFILE1.Visible = true;
                    FileUpload1.Visible = false;
                }
                else
                {
                    HyperLink_FILENAME1.Visible = false;
                    Button_DelFILE1.Visible = false;
                    FileUpload1.Visible = true;
                }


                if (path2 != "")
                {
                    HyperLink_FILENAME2.Visible = true;
                    HyperLink_FILENAME2.NavigateUrl = show_upload_path() + path2;
                    Button_DelFILE2.Visible = true;
                    FileUpload2.Visible = false;
                }
                else
                {
                    HyperLink_FILENAME2.Visible = false;
                    Button_DelFILE2.Visible = false;
                    FileUpload2.Visible = true;
                }
                string STATUS = dt.Rows[0]["STATUS"].ToString();
                if (isAdmin == "Y")//人事室人員
                {
                    btInsert.Visible = false;
                    btOK.Visible = true;
                    btNOK.Visible = false;
                }
                else if ((STATUS == "1" || STATUS == "2") && Session["ADPMZ_ID"].ToString() != dt.Rows[0]["ADD_USER"].ToString())
                {
                    btInsert.Visible = false;
                    btOK.Visible = true;
                    btNOK.Visible = true;
                }
                else if ((STATUS == "1" || STATUS == "0") && Session["ADPMZ_ID"].ToString() == dt.Rows[0]["ADD_USER"].ToString())
                {
                    btInsert.Visible = true;
                    btOK.Visible = false;
                    btNOK.Visible = false;
                }
                else if (STATUS == "3")
                {
                    btInsert.Visible = false;
                    btOK.Visible = false;
                    btNOK.Visible = false;
                }

                //狀態非退回及新增，且當前使用者非新增人的話，關閉所有操作功能
                if (STATUS != "0" && STATUS != "1" && Session["ADPMZ_ID"].ToString() != dt.Rows[0]["ADD_USER"].ToString())
                {
                    txt_Date.Enabled = false;
                    DropDownList_Type.Enabled = false;
                    DropDownList_MZ_AD.Enabled = false;
                    DropDownList_MZ_UNIT.Enabled = false;
                    TextBox_MZ_OCCC.Enabled = false;
                    DropDownList_NAME.Enabled = false;
                    Text_INTIME.Enabled = false;
                    Text_OUTTIME.Enabled = false;

                    Text_MEMO.Enabled = false;
                    Button_DelFILE1.Visible = false;
                    FileUpload1.Visible = false;
                    Button_DelFILE2.Visible = false;
                    FileUpload2.Visible = false;
                }
                //if (STATUS == "2")
                //{
                //    txt_Date.Enabled = false;
                //    DropDownList_Type.Enabled = false;
                //    DropDownList_MZ_AD.Enabled = false;
                //    DropDownList_MZ_UNIT.Enabled = false;
                //    TextBox_MZ_OCCC.Enabled = false;
                //    DropDownList_NAME.Enabled = false;
                //    Text_INTIME.Enabled = false;
                //    Text_OUTTIME.Enabled = false;

                //    Text_MEMO.Enabled = false;
                //    Button_DelFILE1.Visible = true;
                //    FileUpload1.Visible = false;
                //    Button_DelFILE2.Visible = false;
                //    FileUpload2.Visible = false;

                //    btInsert.Visible = false;
                //    btOK.Text = "確認";
                //}
            }
        }
        public static string show_upload_path()
        {
            string rs = System.Configuration.ConfigurationManager.AppSettings["c_upload_showpath"];
            return rs;
        }
        protected void DropDownList_MZ_AD_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeUnit(DropDownList_MZ_AD, DropDownList_MZ_UNIT);
        }
        protected void ChangeUnit(DropDownList ddl_ad, DropDownList ddl_unit)
        {
            DataTable temp = new DataTable();
            string strSQL = "SELECT MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE='25' AND MZ_KCODE IN (SELECT MZ_UNIT FROM A_UNIT_AD WHERE MZ_AD='" + ddl_ad.SelectedValue + "')";
            temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");

            ddl_unit.DataSource = temp;
            ddl_unit.DataTextField = "MZ_KCHI";
            ddl_unit.DataValueField = "MZ_KCODE";
            ddl_unit.DataBind();
            ddl_unit.Items.Insert(0, "");
        }

        protected void btInsert_Click(object sender, EventArgs e)
        {
            try
            {
                //DateTime datetime = DateTime.Parse(txt_Date.Text.Trim());
                string datetime = txt_Date.Text.ToString().Trim();
                string Type = DropDownList_Type.SelectedValue;
                string MZ_AD = DropDownList_MZ_AD.SelectedValue;
                string MZ_UNIT = DropDownList_MZ_UNIT.SelectedValue;
                string MZ_OCCC = TextBox_MZ_OCCC.Text.ToString().Trim();
                string MZ_NAME = "";
                if (DropDownList_NAME.SelectedItem != null)
                    MZ_NAME = DropDownList_NAME.SelectedItem.Text;
               
                string MZ_ID = DropDownList_NAME.SelectedValue;
                string INTIME = Text_INTIME.Text.Trim().ToString();
                string OUTTIME = Text_OUTTIME.Text.Trim().ToString();
                string MEMO = Text_MEMO.Text.Trim().ToString();
                string CHECKER = ddl_CHECKER.SelectedValue;
                string CTYPE = rd_CTYPE.SelectedValue ;
                string ADD_USER = Session["ADPMZ_ID"].ToString();
                string MOD_USER = Session["ADPMZ_ID"].ToString();
                DateTime ADD_DATE = DateTime.Now;
                DateTime MOD_DATE = DateTime.Now;
                string sql = "";
                string STATUS = "1";
                 if (CTYPE == "1")
                {
                    sql = "select count(*) as count1 from C_CARDHISTORY_EDIT where MZ_ID = '" + MZ_ID + "' and CTYPE = '1' and (TYPE = '上班' or TYPE = '下班') and Status <> '0' and Datetime like '" + datetime.Substring(0,4) + "%'" ;
                    string count1 = "0" + o_DBFactory.ABC_toTest.vExecSQL(sql);
                    sql = "select count(*) as count2 from C_CARDHISTORY_EDIT where MZ_ID = '" + MZ_ID + "' and CTYPE = '1' and TYPE = '上下班' and Status <> '0' and Datetime like '" + datetime.Substring(0, 4) + "%'";
                    string count2 = "0" + o_DBFactory.ABC_toTest.vExecSQL(sql);
                    string nowcount = "0"; //本次增加
                    if (Type == "上班" || Type == "下班")
                    {
                        nowcount = "1";
                    }
                    else if(Type == "上下班")
                    {
                        nowcount = "2";
                    }
                    if (int.Parse(nowcount) + int.Parse(count1) + (int.Parse(count2)*2)  > 12)
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('本年度忘記刷卡已經超過12次')", true);
                        return;
                    }
                    else
                    {
                        isAdmin = "Y";
                      //  STATUS = "3";
                     
                    }
                }
               

                if (INTIME == "" && OUTTIME == "") //modified by alex, 2020/3/27, 不可同時為空值
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請確定上下班打卡時間有填寫完畢')", true);
                }
                else if (DropDownList_Type.SelectedValue == "" || txt_Date.Text == "")
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請確定日期有填寫完畢')", true);
                }
                else if (DropDownList_NAME.SelectedValue == "")
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請確定人員基本資料有填寫完畢')", true);
                }
                else if (radl_Check.SelectedValue == "Online" && ddl_CHECKER.SelectedIndex == -1 && ddl_CHECKER.SelectedValue == "")
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請確定審查者有填寫完畢')", true);
                }
                else
                {
                    STATUS = "1";
                    //走紙本
                    if (radl_Check.SelectedValue == "Paper")
                    {
                        STATUS = "5";
                    }
                    string ErrorString = "";
                    if (Request.QueryString["SN"] != null && Request.QueryString["SN"].ToString() != "")
                    {
                        string FILE1 = CHECK_UPLOADFILE(ViewState["dpath1"] == null ? "" : ViewState["dpath1"].ToString(), ErrorString, FileUpload1, Button_DelFILE1, HyperLink_FILENAME1, "FILE1", "dpath1");
                        string FILE2 = CHECK_UPLOADFILE(ViewState["dpath2"] == null ? "" : ViewState["dpath2"].ToString(), ErrorString, FileUpload2, Button_DelFILE2, HyperLink_FILENAME2, "FILE2", "dpath2");
                        string strSQL = "UPDATE C_CARDHISTORY_EDIT SET MZ_ID=@MZ_ID,MZ_NAME=@MZ_NAME,MZ_OCCC=@MZ_OCCC,MZ_AD=@MZ_AD,MZ_UNIT=@MZ_UNIT,TYPE=@TYPE,DATETIME=@DATETIME,INTIME=@INTIME,OUTTIME=@OUTTIME,MOD_USER=@MOD_USER,MOD_DATE=@MOD_DATE,MEMO=@MEMO,FILE1=@FILE1,FILE2=@FILE2,STATUS=@STATUS,CHECKER=@CHECKER,CTYPE=@CTYPE WHERE SN=@SN";
                        SqlParameter[] parameterList = {
                            new SqlParameter("MZ_ID",SqlDbType.VarChar){Value = MZ_ID},
                            new SqlParameter("MZ_NAME",SqlDbType.VarChar){Value = MZ_NAME},
                            new SqlParameter("MZ_OCCC",SqlDbType.VarChar){Value = MZ_OCCC},
                            new SqlParameter("MZ_AD",SqlDbType.VarChar){Value = MZ_AD},
                            new SqlParameter("MZ_UNIT",SqlDbType.VarChar){Value = MZ_UNIT},
                            new SqlParameter("TYPE",SqlDbType.VarChar){Value = Type},
                            new SqlParameter("DATETIME",SqlDbType.VarChar){Value = datetime},
                            new SqlParameter("INTIME",SqlDbType.VarChar){Value = INTIME},
                            new SqlParameter("OUTTIME",SqlDbType.VarChar){Value = OUTTIME},
                            new SqlParameter("MOD_USER",SqlDbType.VarChar){Value = MOD_USER},
                            new SqlParameter("MOD_DATE",SqlDbType.DateTime){Value = MOD_DATE},
                            new SqlParameter("MEMO",SqlDbType.VarChar){Value = MEMO},
                            new SqlParameter("FILE1",SqlDbType.VarChar){Value = FILE1},
                            new SqlParameter("FILE2",SqlDbType.VarChar){Value = FILE2},
                            new SqlParameter("STATUS",SqlDbType.VarChar){Value = STATUS},
                            new SqlParameter("CHECKER",SqlDbType.VarChar){Value = CHECKER},
                            new SqlParameter("CTYPE",SqlDbType.VarChar){Value = CTYPE},
                            new SqlParameter("SN",SqlDbType.VarChar){Value = Request.QueryString["SN"].ToString()},
                            };

                        o_DBFactory.ABC_toTest.ExecuteNonQuery( strSQL, parameterList);
                    }
                    else
                    {
                        string FILE1 = CHECK_UPLOADFILE(ViewState["dpath1"] == null ? "" : ViewState["dpath1"].ToString(), ErrorString, FileUpload1, Button_DelFILE1, HyperLink_FILENAME1, "FILE1", "dpath1");
                        string FILE2 = CHECK_UPLOADFILE(ViewState["dpath2"] == null ? "" : ViewState["dpath2"].ToString(), ErrorString, FileUpload2, Button_DelFILE2, HyperLink_FILENAME2, "FILE2", "dpath2");
                        string strSQL = "INSERT INTO " + "C_CARDHISTORY_EDIT" +
                                                                        "(SN,MZ_ID,MZ_NAME,MZ_OCCC,MZ_AD,MZ_UNIT,TYPE,DATETIME,INTIME,OUTTIME,ADD_USER,ADD_DATE,MEMO,FILE1,FILE2,STATUS,CHECKER,CTYPE)" +
                                                                    " VALUES" +
                                                                        "( NEXT VALUE FOR dbo.C_CARDHISTORY_EDIT_SN,@MZ_ID,@MZ_NAME,@MZ_OCCC,@MZ_AD,@MZ_UNIT,@TYPE,@DATETIME,@INTIME,@OUTTIME,@ADD_USER,@ADD_DATE,@MEMO,@FILE1,@FILE2,@STATUS,@CHECKER,@CTYPE)";

                        SqlParameter[] parameterList = {
                            new SqlParameter("MZ_ID",SqlDbType.VarChar){Value = MZ_ID},
                            new SqlParameter("MZ_NAME",SqlDbType.VarChar){Value = MZ_NAME},
                            new SqlParameter("MZ_OCCC",SqlDbType.VarChar){Value = MZ_OCCC},
                            new SqlParameter("MZ_AD",SqlDbType.VarChar){Value = MZ_AD},
                            new SqlParameter("MZ_UNIT",SqlDbType.VarChar){Value = MZ_UNIT},
                            new SqlParameter("TYPE",SqlDbType.VarChar){Value = Type},
                            new SqlParameter("DATETIME",SqlDbType.VarChar){Value = datetime},
                            new SqlParameter("INTIME",SqlDbType.VarChar){Value = INTIME},
                            new SqlParameter("OUTTIME",SqlDbType.VarChar){Value = OUTTIME},
                            new SqlParameter("ADD_USER",SqlDbType.VarChar){Value = ADD_USER},
                            new SqlParameter("ADD_DATE",SqlDbType.DateTime){Value = ADD_DATE},
                            new SqlParameter("MEMO",SqlDbType.VarChar){Value = MEMO},
                            new SqlParameter("FILE1",SqlDbType.VarChar){Value = FILE1},
                            new SqlParameter("FILE2",SqlDbType.VarChar){Value = FILE2},
                            new SqlParameter("STATUS",SqlDbType.VarChar){Value = STATUS},
                            new SqlParameter("CHECKER",SqlDbType.VarChar){Value = CHECKER},
                            new SqlParameter("CTYPE",SqlDbType.VarChar){Value = CTYPE},
                            };

                        o_DBFactory.ABC_toTest.ExecuteNonQuery( strSQL, parameterList);
                    }
                }
                //將忘記刷卡自動簽核刪除
                //if (CTYPE == "1"){btOK_Click(sender, e);}
                get_usecount();
                
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('資料已送出')", true);
                //Response.Redirect("C_CARDHISTORY_EDIT.aspx?TPM_FION=" + Request.QueryString["TPM_FION"].ToString());
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('資料更新失敗')", true);
            }


        }

        protected void btOK_Click(object sender, EventArgs e)
        {
            string STATUS = "0";
            string SQL = "";
            if (isAdmin == "Y")
            {
                STATUS = "3";
            }
            else
            {
                STATUS = "2";
            }
            try
            {
                string MOD_USER = Session["ADPMZ_ID"].ToString();
                DateTime MOD_DATE = DateTime.Now;
                if (rd_CTYPE.SelectedValue != "1")
                {
                    string updateString = "UPDATE C_CARDHISTORY_EDIT SET CHECKER='" + ddl_CHECKER.SelectedValue + "',STATUS='" + STATUS + "' ,MOD_USER='" + MOD_USER + "' ,MOD_DATE=dbo.TO_DATE('" + MOD_DATE.ToString("yyyy/MM/dd HH:mm:ss") + "', 'YYYY-MM-DD hh24:mi:ss')" + " WHERE SN='" + Request.QueryString["SN"].ToString() + "'";
                    o_DBFactory.ABC_toTest.Edit_Data(updateString);
                }
                
                if (STATUS == "3")
                {
                    string SQLstr = "";
                    
                    if (rd_CTYPE.SelectedValue != "1")
                    {
                        SQLstr = string.Format(@"SELECT * FROM C_CARDHISTORY_EDIT WHERE SN='{0}'", Request.QueryString["SN"].ToString());
                    }
                    else
                    {
                        string datetime = txt_Date.Text.ToString().Trim();
                        string MZ_ID = DropDownList_NAME.SelectedValue;
                        SQLstr = string.Format(@"SELECT * FROM C_CARDHISTORY_EDIT WHERE MZ_ID = '{0}' and DateTime = '{1}' order by SN desc", MZ_ID , datetime);
                    }
                    
                    DataTable dt = new DataTable();

                    dt = o_DBFactory.ABC_toTest.Create_Table(SQLstr, "get");
                    string strSQL = "";
                    if (dt.Rows.Count > 0)
                    {
                        string MZ_ID = dt.Rows[0]["MZ_ID"].ToString();
                        string MZ_NAME = dt.Rows[0]["MZ_NAME"].ToString();
                        MZ_NAME = MZ_ID + " " + MZ_NAME;
                        MZ_ID = "00000" + MZ_ID.Substring(5, 5);
                        string DATETIME = dt.Rows[0]["DATETIME"].ToString();
                        string INTIME = dt.Rows[0]["INTIME"].ToString();
                        string OUTTIME = dt.Rows[0]["OUTTIME"].ToString();
                        if (INTIME != "") { 
                        SqlParameter[] parameterList = { 
                                        new SqlParameter("TID", SqlDbType.Float){Value = 99},                            
                                        new SqlParameter("TERMINALNAME", SqlDbType.NVarChar){Value = "補登"},
                                        new SqlParameter("USERID", SqlDbType.NVarChar){Value = MZ_ID},
                                        new SqlParameter("USERNAME", SqlDbType.NVarChar){Value = MZ_NAME},
                                        new SqlParameter("LOGDATE", SqlDbType.NVarChar){Value = DATETIME},
                                        new SqlParameter("LOGTIME", SqlDbType.NVarChar){Value = INTIME},
                                        new SqlParameter("VERIFY", SqlDbType.NVarChar){Value = "IN"},
                                        new SqlParameter("FKEY", SqlDbType.NVarChar){Value = "NONE"},
                                        new SqlParameter("DOOR", SqlDbType.NVarChar){Value = "主門"},
                                        };


                             strSQL = @"INSERT INTO C_CARDHISTORY_NEW (TID,TERMINALNAME,USERID,USERNAME,LOGDATE,LOGTIME,VERIFY,FKEY,DOOR)
                                        VALUES (@TID,@TERMINALNAME,@USERID,@USERNAME,@LOGDATE,@LOGTIME,@VERIFY,@FKEY,@DOOR) ";

                            o_DBFactory.ABC_toTest.ExecuteNonQuery( strSQL, parameterList);
                        }
                        if (OUTTIME != "")
                        {
                            SqlParameter[] parameterList1 = {
                                        new SqlParameter("TID", SqlDbType.Float){Value = 99},
                                        new SqlParameter("TERMINALNAME", SqlDbType.NVarChar){Value = "補登"},
                                        new SqlParameter("USERID", SqlDbType.NVarChar){Value = MZ_ID},
                                        new SqlParameter("USERNAME", SqlDbType.NVarChar){Value = MZ_NAME},
                                        new SqlParameter("LOGDATE", SqlDbType.NVarChar){Value = DATETIME},
                                        new SqlParameter("LOGTIME", SqlDbType.NVarChar){Value = OUTTIME},
                                        new SqlParameter("VERIFY", SqlDbType.NVarChar){Value = "IN"},
                                        new SqlParameter("FKEY", SqlDbType.NVarChar){Value = "NONE"},
                                        new SqlParameter("DOOR", SqlDbType.NVarChar){Value = "主門"},
                                        };


                            strSQL = @"INSERT INTO C_CARDHISTORY_NEW (TID,TERMINALNAME,USERID,USERNAME,LOGDATE,LOGTIME,VERIFY,FKEY,DOOR)
                                        VALUES (@TID,@TERMINALNAME,@USERID,@USERNAME,@LOGDATE,@LOGTIME,@VERIFY,@FKEY,@DOOR) ";

                            o_DBFactory.ABC_toTest.ExecuteNonQuery( strSQL, parameterList1);
                        }
                        //直接新增到刷卡資料系統內 , 但如果忘記刷卡不要彈畫面 Mark
                        if (rd_CTYPE.SelectedValue != "1")
                        {
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('資料已確認')", true);
                        }
                    }

                }
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('資料確認失敗')", true);
            }
        }

        protected void btNOK_Click(object sender, EventArgs e)
        {
            string STATUS = "0";
            string updateString = "UPDATE C_CARDHISTORY_EDIT SET STATUS='" + STATUS + "' WHERE SN='" + Request.QueryString["SN"].ToString() + "'";

            o_DBFactory.ABC_toTest.Edit_Data(updateString);
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('資料已退回')", true);
        }
        protected void GV_CHECK_init()
        {
            DataTable temp = new DataTable();
            temp = o_DBFactory.ABC_toTest.Create_Table("SELECT (a_dlbase.mz_name + ' ' + (SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='26' AND MZ_KCODE=C_REVIEW_MANAGEMENT.MZ_OCCC)) AS MZ_NAME,c_review_management.MZ_ID FROM c_review_management,a_dlbase WHERE A_DLBASE.MZ_ID = C_REVIEW_MANAGEMENT.MZ_ID AND C_REVIEW_MANAGEMENT.MZ_EXAD = '" + DropDownList_MZ_AD.SelectedValue + "' AND C_REVIEW_MANAGEMENT.MZ_EXUNIT = '" + DropDownList_MZ_UNIT.SelectedValue + "' AND C_REVIEW_MANAGEMENT.REVIEW_LEVEL = '2'", "get");
            //ddl_CHECKER.Items.Clear();
            //ListItem li = new ListItem("請選擇", "");
            //ddl_CHECKER.Items.Add(li);
            ddl_CHECKER.DataSource = temp;
            ddl_CHECKER.DataValueField = "MZ_ID";        //在此輸入的是資料表的欄位名稱
            ddl_CHECKER.DataTextField = "MZ_NAME";      //在此輸入的是資料表的欄位名稱
            ddl_CHECKER.DataBind();
        }
        protected void DropDownList_Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rd_CTYPE.SelectedValue == "1")
            {
                if (DropDownList_Type.SelectedValue == "上班")
                {
                    Text_INTIME.Enabled = true;
                    Text_OUTTIME.Enabled = false;
                }
                else if (DropDownList_Type.SelectedValue == "下班")
                {
                    Text_INTIME.Enabled = false;
                    Text_OUTTIME.Enabled = true;
                }
                else if (DropDownList_Type.SelectedValue == "上下班")
                {
                    Text_INTIME.Enabled = true;
                    Text_OUTTIME.Enabled = true;
                }
                
            }
            else
            {
                Text_INTIME.Enabled = false;
                Text_OUTTIME.Enabled = false;

                if (DropDownList_Type.SelectedValue == "上班")
                { Text_INTIME.Text = "09:00:00"; }
                else if (DropDownList_Type.SelectedValue == "下班")
                { Text_OUTTIME.Text = "18:00:00"; }
                else if (DropDownList_Type.SelectedValue == "上下班")
                {
                    Text_INTIME.Text = "09:00:00";
                    Text_OUTTIME.Text = "18:00:00";
                }
            }
            string MZ_ID = DropDownList_NAME.SelectedValue;
            string LOGDATE = txt_Date.Text.Trim().ToString();
            if (DropDownList_Type.SelectedValue != "上班" && DropDownList_Type.SelectedValue != "上下班")
            {
                string OverINTIME = o_DBFactory.ABC_toTest.vExecSQL("SELECT LOGTIME FROM (SELECT LOGTIME,ROW_NUMBER() OVER(ORDER BY LOGTIME) AS ROWCOUNT FROM C_CARDHISTORY_NEW WHERE dbo.SUBSTR(USERNAME,0,10)='" + MZ_ID + "' AND LOGDATE='" + LOGDATE + "' AND VERIFY='IN') WHERE ROWCOUNT=1"); //計算當天最早刷卡時間
                Text_INTIME.Text = OverINTIME;
            }
            else
            {
                Text_INTIME.Text = "09:00:00";
            }
            if (DropDownList_Type.SelectedValue != "下班" && DropDownList_Type.SelectedValue != "上下班")
            {
                string OverOUTTIME = o_DBFactory.ABC_toTest.vExecSQL("SELECT LOGTIME FROM (SELECT LOGTIME,ROW_NUMBER() OVER(ORDER BY LOGTIME DESC) AS ROWCOUNT FROM C_CARDHISTORY_NEW WHERE dbo.SUBSTR(USERNAME,0,10)='" + MZ_ID + "' AND LOGDATE='" + LOGDATE + "' AND VERIFY='IN') WHERE ROWCOUNT=1"); //計算當天最晚刷卡時間
                Text_OUTTIME.Text = OverOUTTIME;
            }
            else
            {
                Text_OUTTIME.Text = "18:00:00";
            }
        }
        protected void DropDownList_AD_SelectedIndexChanged(object sender, EventArgs e)
        {
            Select_UNIT();
            Select_Name();
            Time_Changed();
            GV_CHECK_init();
        }
        protected void DropDownList_UNIT_SelectedIndexChanged(object sender, EventArgs e)
        {
            Select_Name();
            Time_Changed();
            GV_CHECK_init();
        }
        protected void Select_UNIT()
        {
            string ad = DropDownList_MZ_AD.SelectedValue;
            string unit = DropDownList_MZ_UNIT.SelectedValue;
            DataTable dt = new DataTable();
            string strSQL = "SELECT MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE='25' AND MZ_KCODE IN (SELECT MZ_UNIT FROM A_UNIT_AD WHERE MZ_AD='" + ad + "')";
            dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
            DropDownList_MZ_UNIT.DataSource = dt;
            DropDownList_MZ_UNIT.DataTextField = "MZ_KCHI";
            DropDownList_MZ_UNIT.DataValueField = "MZ_KCODE";
            DropDownList_MZ_UNIT.DataBind();
        }
        protected void Select_Name()
        {
            string ad = DropDownList_MZ_AD.SelectedValue;
            string unit = DropDownList_MZ_UNIT.SelectedValue;
            string whereStr = "";
            //if (DropDownList_MZ_AD.SelectedIndex == 0)
            //{
            //    whereStr = " where MZ_STATUS2='Y'";
            //}
            //else if (DropDownList_MZ_UNIT.SelectedIndex == 0)
            //{
            //    whereStr = " where MZ_EXAD='" + DropDownList_MZ_AD.SelectedValue + "' AND MZ_STATUS2='Y'";
            //}
            //else
            //{
            whereStr = " where MZ_EXAD='" + DropDownList_MZ_AD.SelectedValue + "' AND MZ_EXUNIT='" + DropDownList_MZ_UNIT.SelectedValue + "' AND MZ_STATUS2='Y'";
            //}
            DataTable dt = new DataTable();
            string strSQL = "SELECT MZ_NAME,MZ_ID FROM A_DLBASE " + whereStr + " ORDER BY MZ_EXAD, MZ_EXUNIT, MZ_TBDV";
            dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");

            DropDownList_NAME.Items.Clear();
            DropDownList_NAME.DataSource = dt;
            DropDownList_NAME.DataTextField = "MZ_NAME";
            DropDownList_NAME.DataValueField = "MZ_ID";
            DropDownList_NAME.DataBind();
        }
        protected void C_CARDHISTORY_EDIT_Init()
        {
            string MZ_ID = DropDownList_NAME.SelectedValue;
            string SQL = "SELECT * FROM C_CARDHISTORY_EDIT WHERE  MZ_ID='" + MZ_ID + "' order by DATETIME desc";
            DataTable dt_tmp = o_DBFactory.ABC_toTest.Create_Table(SQL, "GET");
            gv_C_CARDHISTORY_EDIT.DataSource = dt_tmp;
            gv_C_CARDHISTORY_EDIT.DataBind();
        }
        protected void gv_C_CARDHISTORY_EDIT_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;
                Label lbl_DATETIME = (Label)e.Row.FindControl("lbl_DATETIME");
                //string DATETIME = DateTime.Parse(drv["DATETIME"].ToString()).ToString("yyyy/MM/dd");
                string DATETIME = drv["DATETIME"].ToString();
                string INTIME = drv["INTIME"].ToString();
                string OUTTIME = drv["OUTTIME"].ToString();
                lbl_DATETIME.Text = DATETIME + " " + INTIME + "~" + OUTTIME;

                string STATUS = drv["STATUS"].ToString();
                Label lbl_STATUS = (Label)e.Row.FindControl("lbl_STATUS");
                HyperLink hlk_check = (HyperLink)e.Row.FindControl("HyperLink1");
                if (STATUS == "0")
                    lbl_STATUS.Text = "退回";
                else if (STATUS == "1")
                    lbl_STATUS.Text = "新增";
                else if (STATUS == "2")
                {
                    lbl_STATUS.Text = "陳核";
                    hlk_check.Visible = false;
                }
                else if (STATUS == "3")
                {
                    lbl_STATUS.Text = "決行";
                    hlk_check.Visible = false;
                }
                else if (STATUS == "4")
                {
                    lbl_STATUS.Text = "收件";
                    hlk_check.Visible = false;
                }
                else if (STATUS == "5")
                    lbl_STATUS.Text = "紙本";
                Label lbl_MOD_USER = (Label)e.Row.FindControl("lbl_MOD_USER");
                lbl_MOD_USER.Text = o_A_DLBASE.CNAME(drv["CHECKER"].ToString());
                Label lbl_SURE_USER = (Label)e.Row.FindControl("lbl_SURE_USER");
                lbl_SURE_USER.Text = o_A_DLBASE.CNAME(drv["SURE_USER"].ToString());
                //原因別列表呈現
                string CTYPE_1 = drv["CTYPE"].ToString();
                Label lbl_CTYPE = (Label)e.Row.FindControl("lbl_CTYPE");
                if(CTYPE_1=="0")
                {
                    lbl_CTYPE.Text = "因公未刷";
                }
                else if(CTYPE_1 == "1")
                {
                    lbl_CTYPE.Text = "忘記刷卡";
                }
                //刪除鈕權限判斷
                Button bt_del = (Button)e.Row.FindControl("btndel");
                if (_strGID=="A"|| _strGID == "B" || _strGID == "C")
                {
                    bt_del.Enabled = true;
                }
                else
                {
                    bt_del.Enabled = false;
                }


            }
        }
        protected void Button_DelFILE1_Command(object sender, CommandEventArgs e)
        {
            //del_file(e.CommandArgument.ToString());
            string path = string.Empty;
            if (ViewState["SN"] == null)
            {
                return;
            }
            path = Find_Url(ViewState["SN"].ToString(), "FILE1");
            if (!string.IsNullOrEmpty(path.Trim()))
            {
                ViewState["dpath1"] = path;
            }
            FileUpload1.Visible = true;

            Button_DelFILE1.Visible = false;
            HyperLink_FILENAME1.Visible = false;

        }

        protected void Button_DelFILE2_Command(object sender, CommandEventArgs e)
        {
            //del_file(e.CommandArgument.ToString());
            string path = string.Empty;
            if (ViewState["SN"] == null)
            {
                return;
            }
            path = Find_Url(ViewState["SN"].ToString(), "FILE2");
            if (!string.IsNullOrEmpty(path.Trim()))
            {
                ViewState["dpath2"] = path;
            }
            FileUpload2.Visible = true;

            Button_DelFILE2.Visible = false;
            HyperLink_FILENAME2.Visible = false;
        }
        public string Find_Url(string bbsid, string filed)
        {
            string strSQL = @"select " + filed + @" FROM C_CARDHISTORY_EDIT WHERE SN ='" + bbsid + "'";
            DataTable temp = new DataTable();
            temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
            string File_Url;
            File_Url = temp.Rows[0][0].ToString();
            return File_Url;
        }
        protected string CHECK_UPLOADFILE(string path, string ErrorString, FileUpload fu, Button btn, HyperLink hl, string field, string dPath)
        {
            //檢查有無上傳檔案
            if (path != "")
            {
                del_file(path);
                ViewState.Remove(dPath);

                fu.Visible = true;
                fu.Enabled = false;
                btn.Visible = false;
                hl.Visible = false;

            }
            if (fu.Visible == false && btn.Visible == true)
            {
                return Find_Url(ViewState["SN"].ToString(), field);
            }
            else if (fu.HasFile)
            {
                string file_name = fu.FileName;// +"." + vc[vc.Count() - 1].ToLower();
                if (!saveUploadFile(fu.PostedFile, file_name))
                {
                    ErrorString += "檔案上傳有誤" + "\\r\\n";
                    return string.Empty;
                }

                fu.Visible = false;
                btn.Visible = true;
                btn.Enabled = false;
                hl.Visible = true;
                hl.NavigateUrl = show_upload_path() + ViewState["path"].ToString();
                return ViewState["path"].ToString();
            }
            else
            {
                return string.Empty;
            }
        }
        protected void del_file(string del_url)
        {
            try
            {
                del_url = get_upload_path() + del_url;
                System.IO.File.Delete(del_url);
            }
            catch { }
        }
        public static string get_upload_path()
        {
            string rs = System.Configuration.ConfigurationManager.AppSettings["c_upload_filepath"];
            rs = System.Web.HttpContext.Current.Server.MapPath(rs);
            return rs;
        }
        public bool saveUploadFile(HttpPostedFile fu, string file_name)
        {
            try
            {
                HttpPostedFile tUploadFile = fu;
                int tFileLength = tUploadFile.ContentLength;
                byte[] tFileByte = new byte[tFileLength];
                tUploadFile.InputStream.Read(tFileByte, 0, tFileLength);
                if (tFileLength <= 10000000)  //限制檔案大小10000000
                {
                    // 如無此路徑，則先建立
                    if (!Directory.Exists(get_upload_path()))
                    {
                        Directory.CreateDirectory(get_upload_path());
                    }
                    string[] kkk = file_name.Split('.');
                    file_name = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Session["TPM_MID"].ToString() + "." + kkk[1];
                    FileStream tNewfile = new FileStream(get_upload_path() + file_name, FileMode.Create);
                    tNewfile.Write(tFileByte, 0, tFileByte.Length);
                    tNewfile.Close();
                    ViewState["path"] = file_name;
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "alert('檔案大小超過限制！');", true);
                    return false;
                }
            }

            catch
            {
                ViewState["path"] = string.Empty;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "alert('請選擇檔案！');", true);
                return false;
            }
            return true;

        }


        protected void gv_C_CARDHISTORY_EDIT_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            String ErrMsg = String.Empty;
            String SN = e.CommandArgument.ToString();
            int index = int.Parse(e.CommandArgument.ToString());
            switch (e.CommandName)
            {
                #region Crystal Report作法
                //case "doRpt":
                //    string strSQL = string.Format(@"SELECT * FROM C_CARDHISTORY_EDIT WHERE SN='{0}'", SN);

                //    DataTable rpt_dt = new DataTable();

                //    Session["RPT_SQL_A"] = strSQL;
                //    Session["C_CARDHISTORY_SN"] = SN;

                //    Session["TITLE"] = "新北市政府警察局　職員因公疏未辦理按卡紀錄報告單";

                //    string tmp_url = "A_rpt.aspx?fn=C_CARDHISTORY_edit&TPM_FION=" + Request.QueryString["TPM_FION"].ToString();

                //    ScriptManager.RegisterClientScriptBlock(panel_Apply, this.GetType(), "click", "go_print('" + tmp_url + "');", true);


                //    break;
                #endregion
                case "doRpt":

                    string pdflink1 = System.Configuration.ConfigurationManager.AppSettings["pdflink"].ToString();
                    Session["C_CARDHISTORY_SN"] = SN;

                    string tmp_url = "C_CARDHISTORY_rpt.aspx?&SN=" + Session["C_CARDHISTORY_SN"]+"&TPM_FION=" + Request.QueryString["TPM_FION"].ToString();
                    string filename = "/userfiles/" + DateTime.Now.ToString("yyyyMMddHHmmssff") + ".pdf";
                    string url = "http://" + Request.ServerVariables["Server_Name"] + ":" + Request.ServerVariables["Server_port"] + "/1-personnel/" + tmp_url;
                    System.Diagnostics.Process.Start(pdflink1, " -s A4 -L 10 -R 10 -T 15 -B 10 " + url + " " + Server.MapPath(filename));
                    System.Threading.Thread.Sleep(8000);
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('" + filename + "');", true);


                    break;

                case "dodel":
                    string strSQL1 = string.Format(@"delete FROM C_CARDHISTORY_EDIT WHERE SN='{0}'", SN);
                    o_DBFactory.ABC_toTest.vExecSQL(strSQL1);
                    get_usecount();
                    //DataTable rpt_dt = new DataTable();

                    //Session["RPT_SQL_A"] = strSQL;
                    //Session["C_CARDHISTORY_SN"] = SN;

                    //Session["TITLE"] = "新北市政府警察局　職員因公疏未辦理按卡紀錄報告單";

                    //string tmp_url = "A_rpt.aspx?fn=C_CARDHISTORY_edit&TPM_FION=" + Request.QueryString["TPM_FION"].ToString();
                    //string NowYear = DateTime.Now.Year.ToString();
                    //string SQL = "SELECT * FROM C_CARDHISTORY_EDIT WHERE  MZ_ID='" + Session["ADPMZ_ID"].ToString() + "' and DATETIME like '" + NowYear + "%' order by DATETIME desc";
                    //DataTable dt_tmp = o_DBFactory.ABC_toTest.Create_Table(SQL, "GET");
                    //gv_C_CARDHISTORY_EDIT.DataSource = dt_tmp;
                    //gv_C_CARDHISTORY_EDIT.DataBind();
                    ScriptManager.RegisterClientScriptBlock(panel_Apply, this.GetType(), "click", "alert('刪除成功');", true);


                    break;
            }
        }

        protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void radl_Check_SelectedIndexChanged(object sender, EventArgs e)
        {
            string text1 = radl_Check.SelectedValue;
            if (radl_Check.SelectedValue == "Online")
            {
                GV_CHECK_init();
                ddl_CHECKER.Enabled = true;
            }
            else
            {
                ddl_CHECKER.Items.Clear();
                ddl_CHECKER.Enabled = false;
            }
        }

        protected void rd_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rd_CTYPE.SelectedValue == "0")
            {
                aacount1.Visible = false;
                //radl_Check.Text = "";
                //radl_Check.SelectedValue = "";
                radl_Check.ClearSelection();
                radl_Check.Enabled = true;
                GV_CHECK_init();
                ddl_CHECKER.Enabled = true;

                Text_INTIME.Enabled = false;
                Text_OUTTIME.Enabled = false;

                if (DropDownList_Type.SelectedValue == "上班")
                { Text_INTIME.Text = "09:00:00"; }
                else if (DropDownList_Type.SelectedValue == "下班")
                { Text_OUTTIME.Text = "18:00:00"; }
                else if (DropDownList_Type.SelectedValue == "上下班")
                {
                    Text_INTIME.Text = "09:00:00";
                    Text_OUTTIME.Text = "18:00:00";
                }
            }
            else
            {
                aacount1.Visible = true;
                radl_Check.SelectedValue = "Paper";
                radl_Check.Enabled = true;
                ddl_CHECKER.Items.Clear();
                ddl_CHECKER.Enabled = true;

                if (DropDownList_Type.SelectedValue == "上班")
                {
                    Text_INTIME.Enabled = true;
                    Text_OUTTIME.Enabled = false;
                }
                else if (DropDownList_Type.SelectedValue == "下班")
                {
                    Text_INTIME.Enabled = false;
                    Text_OUTTIME.Enabled = true;
                }
                else if (DropDownList_Type.SelectedValue == "上下班")
                {
                    Text_INTIME.Enabled = true;
                    Text_OUTTIME.Enabled = true;
                }
            }
        }

    }
}
