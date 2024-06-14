using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

namespace TPPDDB._3_forleave
{
    public partial class C_ForLeaveOvertime_ONDUTY_DAY : System.Web.UI.Page
    {
       

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                C.check_power();
                //MQ-----------------------20100331
                C.set_Panel_EnterToTAB(ref this.Panel_ONDUTYHOUR_KEYIN);

                Label2.Text = "輪值表排班";
                TextBox_DUTYMONTH.Text = (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0') + (DateTime.Now.Month).ToString().PadLeft(2, '0');

                fill_ID(Session["ADPMZ_EXAD"].ToString(), Session["ADPMZ_EXUNIT"].ToString());
                DropDownList_MZ_NAME.SelectedValue = Session["ADPMZ_ID"].ToString();
                TextBox_MZ_ID.Text = DropDownList_MZ_NAME.SelectedValue;

                Search_Data(Session["ADPMZ_ID"].ToString(), TextBox_DUTYMONTH.Text);


                fill_duty_holidaytind();



                //////查詢按鈕
                C.fill_AD(ddl_EXAD);               
                ddl_EXAD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                C.fill_unit(ddl_EXUNIT, ddl_EXAD.SelectedValue);               
                ddl_EXUNIT.SelectedValue = Session["ADPMZ_EXUNIT"].ToString();

                chk_TPMGroup();

               
                
            }
        }

      

         /// <summary>
        /// 群組權限
        /// </summary>
        protected void chk_TPMGroup()
        {

            switch (o_a_Function.strGID(Session["ADPMZ_ID"].ToString()))
            {
                case "D":
               

                  
                    btn_search_show.Visible = false;
                    TextBox_MZ_ID.Enabled = false;
                    DropDownList_MZ_NAME.Enabled = false;
                    //ddl_EXAD.Enabled = false;
                    //ddl_EXUNIT.Enabled = false;
                    break;
                case "C":
                    //matthew 中和分局進來要可以選中一中二
                    if (Session["ADPMZ_EXAD"].ToString() == "382133600C")
                    {
                        C.fill_DLL_New(ddl_EXAD);
                    }
                    else
                    {
                        ddl_EXAD.Enabled = false;
                    }
                    //ddl_EXAD.Enabled = false;
                    break;
                case "E":
                    btn_search_show.Visible = false;
                    TextBox_MZ_ID.Enabled = false;
                   
                    break;
            
            
            
            }


        }


        //------------------------------------------------------

        /// <summary>
        /// 假日別下拉
        /// </summary>
        public void fill_duty_holidaytind()
        {
            string strsql = string.Format("SELECT DUTY_KIND, DUTY_KINDNAME FROM C_ONDUTY_CODE WHERE DUTY_TYPE='0' order by DUTY_KIND ");
            DataTable dt_kind = o_DBFactory.ABC_toTest.Create_Table(strsql, "get");
            ddl_holidaykind.DataSource = dt_kind;
            ddl_holidaykind.DataTextField = "DUTY_KINDNAME";
            ddl_holidaykind.DataValueField = "DUTY_KIND";


            ddl_holidaykind.DataBind();
            
        }

        /// <summary>
        /// 值勤別下拉
        /// </summary>
        /// <param name="holidaykind"></param>
        public void fill_duty_kind(string holidaykind)
        {

            string strsql = string.Format("SELECT DUTY_KIND, DUTY_KINDNAME,DUTY_PAY FROM C_ONDUTY_CODE WHERE DUTY_TYPE='1' AND substr(DUTY_KIND,0,1)='" + holidaykind + "' order by DUTY_KIND");
            DataTable dt_kind = o_DBFactory.ABC_toTest.Create_Table(strsql, "get");
            ddl_Kind.DataSource = dt_kind;
            ddl_Kind.DataTextField = "DUTY_KINDNAME";
            ddl_Kind.DataValueField = "DUTY_KIND";


            ddl_Kind.DataBind();




            if (dt_kind.Rows.Count > 0)
                txt_pay.Text = dt_kind.Rows[0]["DUTY_PAY"].ToString();
        
        }

    
        /// <summary>
        /// 單位人員名單
        /// </summary>
        /// <param name="EXAD">現服機關</param>
        /// <param name="EXUNIT">現服單位</param>
        public void fill_ID(string EXAD ,string EXUNIT)
        {
            string strSQL = string.Format("SELECT MZ_ID,MZ_NAME,MZ_EXAD_CH,MZ_EXUNIT_CH FROM VW_A_DLBASE_S1 WHERE MZ_EXAD='{0}' AND MZ_EXUNIT='{1}' ", EXAD, EXUNIT);
            DataTable dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");
            DropDownList_MZ_NAME.DataSource = dt;
            DropDownList_MZ_NAME.DataTextField = "MZ_NAME";
            DropDownList_MZ_NAME.DataValueField = "MZ_ID";
            DropDownList_MZ_NAME.DataBind();

            lb_MZ_UNIT.Text = dt.Rows[0]["MZ_EXUNIT_CH"].ToString();


        }

        /// <summary>
        /// 找尋當月輪值資料
        /// </summary>
        /// <param name="MZ_ID">身分證號</param>
        /// <param name="MONTH">年月 (09901)</param>
        public void Search_Data(string MZ_ID, string MONTH)
        {
            string strSQL = string.Format(@"SELECT SN,DATE_TAG,KIND ,DUTY_KINDNAME  KIND_CH,IS_CHK,IS_CHK_UNIT,IS_POSITIVE,IS_OVERTIME_HOUR_INSIDE FROM C_ONDUTY_DAY 
LEFT JOIN C_ONDUTY_CODE ON KIND=DUTY_KIND
WHERE MZ_ID='{0}' AND substr(DATE_TAG,0,5)='{1}'", MZ_ID, MONTH);
            DataTable ONDUTY_Dt_By_Day = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");

            DataTable ONDUTY_Dt_By_Month = new DataTable();

            ONDUTY_Dt_By_Month.Columns.Add("SN",typeof (int));
            ONDUTY_Dt_By_Month.Columns.Add("DATE_TAG", typeof(string));
            ONDUTY_Dt_By_Month.Columns.Add("KIND", typeof(string));
            ONDUTY_Dt_By_Month.Columns.Add("KIND_CH", typeof(string));
            ONDUTY_Dt_By_Month.Columns.Add("IS_CHK", typeof(string));
            ONDUTY_Dt_By_Month.Columns.Add("IS_CHK_UNIT", typeof(string));
            ONDUTY_Dt_By_Month.Columns.Add("IS_POSITIVE", typeof(string));
            ONDUTY_Dt_By_Month.Columns.Add("IS_HOLIDAY", typeof(string));
            ONDUTY_Dt_By_Month.Columns.Add("IS_OVERTIME_HOUR_INSIDE", typeof(string));

            if (!string.IsNullOrEmpty(TextBox_DUTYMONTH.Text))
            {
                DateTime date = DateTime.Parse((int.Parse(TextBox_DUTYMONTH.Text.Substring(0, 3)) + 1911) + "/" + TextBox_DUTYMONTH.Text.Substring(3, 2) + "/01");//從一號開始
                int days_count = DateTime.DaysInMonth(date.Year, date.Month);

                for (int i = 1; i <= days_count; i++)
                {
                    DataRow dr = ONDUTY_Dt_By_Month.NewRow();

                    DataRow[] adddr = ONDUTY_Dt_By_Day.Select("DATE_TAG='" + TextBox_DUTYMONTH.Text + i.ToString().PadLeft(2, '0') + "'");

                    //假日與國定假日
                    DateTime singal_day = TPPDDB.App_Code.DateManange.strLunar_to_DateTime(TextBox_DUTYMONTH.Text);// DateTime.Parse((int.Parse(TextBox_DUTYMONTH.Text.Substring(0, 3)) + 1911) + "/" + TextBox_DUTYMONTH.Text.Substring(3, 2) + "/" + i.ToString().PadLeft(2, '0'));

                    string strSQL1 = " SELECT * FROM C_DUTYHOLIDAY WHERE substr(MZ_HOLIDAY_DATE,0,3)='" + TextBox_DUTYMONTH.Text.Substring(0, 3) + "'";
                    DataTable Holday_dt = o_DBFactory.ABC_toTest.Create_Table(strSQL1, "get");
                    DataRow[] Holday_dr = Holday_dt.Select(@"MZ_HOLIDAY_NAME='除夕' OR MZ_HOLIDAY_NAME='端午節' OR MZ_HOLIDAY_NAME='中秋節' ");
                    List<string> holiday_list = new List<string>();

                    foreach (DataRow fordr in Holday_dr)
                    {

                        holiday_list.Add(fordr["MZ_HOLIDAY_DATE"].ToString());

                    }

                    bool bool_is_holiday = false;
                    for (int j = 0; j < holiday_list.Count; j++)
                    {
                        if (TextBox_DUTYMONTH.Text + i.ToString().PadLeft(2, '0') == holiday_list[j])
                        {
                            bool_is_holiday = true;
                        }
 
                    
                    }

                    string str_isholiday = "A";
                    if (bool_is_holiday == true)
                    {
                        str_isholiday = "C";
                    }
                    else if (singal_day.ToString("ddd") == "星期六" || singal_day.ToString("ddd") == "星期日")
                    {
                        str_isholiday = "B";
                    }
                    


                    if (adddr.Length > 0)
                    {
                        dr["SN"] = adddr[0]["SN"].ToString();
                        dr["DATE_TAG"] = adddr[0]["DATE_TAG"].ToString();
                        dr["KIND"] = adddr[0]["KIND"].ToString();
                        dr["KIND_CH"] = adddr[0]["KIND_CH"].ToString();
                        dr["IS_CHK"] = adddr[0]["IS_CHK"].ToString();
                        dr["IS_CHK_UNIT"] = adddr[0]["IS_CHK_UNIT"].ToString();
                        dr["IS_POSITIVE"] = adddr[0]["IS_POSITIVE"].ToString();
                        dr["IS_HOLIDAY"] = str_isholiday;
                        dr["IS_OVERTIME_HOUR_INSIDE"] = adddr[0]["IS_OVERTIME_HOUR_INSIDE"].ToString();
                        ONDUTY_Dt_By_Month.Rows.Add(dr);
                    }
                    else
                    {
                        dr["SN"] = 0;
                        dr["DATE_TAG"] = TextBox_DUTYMONTH.Text + i.ToString().PadLeft(2, '0');
                        dr["KIND"] = "";
                        dr["KIND_CH"] = "";
                        dr["IS_CHK"] = "";
                        dr["IS_CHK_UNIT"] = "";
                        dr["IS_POSITIVE"] = "";
                        dr["IS_HOLIDAY"] = str_isholiday;
                        dr["IS_OVERTIME_HOUR_INSIDE"] = "";
                        ONDUTY_Dt_By_Month.Rows.Add(dr);

                    }


                }

            }
            else
            {
                //如果日期為輸入的防呆
            
            }

            DV_MONTH_DATA.DataSource = ONDUTY_Dt_By_Month;
            DV_MONTH_DATA.DataBind();


        
        
        }

        protected void DV_MONTH_DATA_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            Label lb_DayNo = (Label)e.Item.FindControl("lb_DayNo");
            LinkButton lbtn_DayDetail = (LinkButton)e.Item.FindControl("lbtn_DayDetail");
           
            Label lb_POSITIVE = (Label)e.Item.FindControl("lb_POSITIVE");
        
            Label lb_OVERTIME_HOUR_INSIDE = (Label)e.Item.FindControl("lb_OVERTIME_HOUR_INSIDE");
          


            string[] lbtn_CommandArgument = lbtn_DayDetail.CommandArgument.ToString().Split(';');

            if (lbtn_CommandArgument[3] == "B" || lbtn_CommandArgument[3] == "C")
            {
                lb_DayNo.ForeColor = System.Drawing.Color.Red;
            }

            

            if (lbtn_CommandArgument[0] == "0")
            {
                lbtn_DayDetail.CommandName = "INSERT";
                lbtn_DayDetail.Text = "新增";

            }
            else
            {
                lbtn_DayDetail.CommandName = "UPDATE";
                lbtn_DayDetail.Text = "修改";
            }

            if (lbtn_CommandArgument[2] == "Y" || lbtn_CommandArgument[6] == "Y")
            {
                lbtn_DayDetail.Enabled = false;
            }

            //如果用SQL句轉.這個修掉
            if (lb_POSITIVE.Text == "1")
            {
                lb_POSITIVE.Text = "正值";


            }
            else if (lb_POSITIVE.Text == "2")
            {
                lb_POSITIVE.Text = "副值";
            }

            if (lb_OVERTIME_HOUR_INSIDE.Text == "Y")
            {
                lb_OVERTIME_HOUR_INSIDE.Text = "補休";

            }
            else
            {
                lb_OVERTIME_HOUR_INSIDE.Text = "";
            }


           

        }

       
        

        protected void DropDownList_MZ_NAME_SelectedIndexChanged(object sender, EventArgs e)
        {
            TextBox_MZ_ID.Text = DropDownList_MZ_NAME.SelectedValue;

            //20141231

            #region 防呆
            string error = "";
            if (string.IsNullOrEmpty(TextBox_MZ_ID.Text) || TextBox_MZ_ID.Text.Length != 10)
            {
                error += @"請輸入正確身分證號\r\n";

            }
            if (TextBox_DUTYMONTH.Text.Length != 5)
            {
                error += @"請輸入正確日期\r\n";

            }
            if (!string.IsNullOrEmpty(error))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('" + error + "');", true);
                return;
            }

            #endregion 防呆

            Search_Data(TextBox_MZ_ID.Text, TextBox_DUTYMONTH.Text);
        }

        /// <summary>
        /// title 的尋找按鈕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_search_Click(object sender, EventArgs e)
        {
            #region 防呆
            string error = "";
            if (string.IsNullOrEmpty(TextBox_MZ_ID.Text) || TextBox_MZ_ID.Text.Length != 10)
            {
                error += @"請輸入正確身分證號\r\n";

            }
            if (TextBox_DUTYMONTH.Text.Length != 5)
            {
                error += @"請輸入正確日期\r\n";

            }
            if (!string.IsNullOrEmpty(error))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('" + error + "');", true);
                return;
            }

            #endregion 防呆

            Search_Data(TextBox_MZ_ID.Text, TextBox_DUTYMONTH.Text);



        }

        protected void lbtn_DayDetail_OnCommand(object sender, CommandEventArgs e)
        {
            
            string[] lbtn_CommandArgument = e.CommandArgument.ToString().Split(';');

            if (e.CommandName == "INSERT")
            {

                lb_Title.Text = "新增";
                lb_SN.Text = lbtn_CommandArgument[0];//通常是0
                lb_DATE.Text = lbtn_CommandArgument[1];

                ddl_holidaykind.SelectedValue = lbtn_CommandArgument[3];
                fill_duty_kind(ddl_holidaykind.SelectedValue);
                ddl_Kind_SelectedIndexChanged(null, null);

                txt_DATE_START.Text = lbtn_CommandArgument[1];
                txt_TIME_START.Text = "0800";

                DateTime END = TPPDDB.App_Code.DateManange.strLunar_to_DateTime(txt_DATE_START.Text);// DateTime.Parse((int.Parse(txt_DATE_START.Text.Substring(0, 3)) + 1911).ToString() + "/" + txt_DATE_START.Text.Substring(3, 2) + "/" + txt_DATE_START.Text.Substring(5, 2));
                END=END.AddDays(1);

                txt_DATE_END.Text = TPPDDB.App_Code.DateManange.DateTime_to_strLunar(END);//(END.Year - 1911).ToString() + END.Month.ToString().PadLeft(2, '0') + END.Day.ToString().PadLeft(2, '0');
                txt_TIME_END.Text = "0800";
                txt_MEMO.Text = string.Empty;
                btn_Delete.Visible = false;

                rbl_OVERTIME_HOUR_INSIDE.SelectedValue = "N";
                rbl_POSITIVE.SelectedValue = "1";

                DV_MONTH_DATA_ModalPopupExtender.Show();

            }
            else if (e.CommandName == "UPDATE")
            {

                lb_Title.Text = "修改";

                string strsql = @" SELECT  SN , MZ_ID ,DATE_TAG ,  KIND ,PAY , TIME_SATRT ,  TIME_END , IS_OVERTIME_HOUR_INSIDE , IS_POSITIVE ,MEMO
                FROM C_ONDUTY_DAY WHERE SN='" + lbtn_CommandArgument[0] + "' ";
                DataTable detail_dt = o_DBFactory.ABC_toTest.Create_Table(strsql, "get");
                if (detail_dt.Rows.Count > 0)//通常有資料才會進這邊.但還是防一下呆
                {
                    lb_SN.Text = detail_dt.Rows[0]["SN"].ToString();
                    lb_DATE.Text = detail_dt.Rows[0]["DATE_TAG"].ToString();
                    ddl_holidaykind.SelectedValue = detail_dt.Rows[0]["KIND"].ToString().Substring(0, 1);
                    fill_duty_kind(ddl_holidaykind.SelectedValue);
                    ddl_Kind.SelectedValue = detail_dt.Rows[0]["KIND"].ToString();
                    txt_pay.Text = detail_dt.Rows[0]["PAY"].ToString();

                    DateTime START = DateTime.Parse(detail_dt.Rows[0]["TIME_SATRT"].ToString());
                    DateTime END = DateTime.Parse(detail_dt.Rows[0]["TIME_END"].ToString());
                    txt_DATE_START.Text = TPPDDB.App_Code.DateManange.DateTime_to_strLunar(START); //START.Year - 1911  + START.ToString("MMdd");
                    //txt_DATE_START.Text = START.Year - 1911 + "/" + START.ToString("MM/dd");
                    txt_TIME_START.Text = START.ToString("HHmm");

                    //txt_DATE_END.Text = END.Year - 1911 + "/" + END.ToString("MM/dd");
                    txt_DATE_END.Text = TPPDDB.App_Code.DateManange.DateTime_to_strLunar(END); //END.Year - 1911 + END.ToString("MMdd");
                    txt_TIME_END.Text = END.ToString("HHmm");

                    rbl_OVERTIME_HOUR_INSIDE.SelectedValue = detail_dt.Rows[0]["IS_OVERTIME_HOUR_INSIDE"].ToString();
                    rbl_POSITIVE.SelectedValue = detail_dt.Rows[0]["IS_POSITIVE"].ToString();
                    txt_MEMO.Text = detail_dt.Rows[0]["MEMO"].ToString();

                    btn_Delete.Visible = true;
                    DV_MONTH_DATA_ModalPopupExtender.Show();
                }
            }
        }


        #region 編輯視窗事件

        protected void ddl_holidaykind_SelectedIndexChanged(object sender, EventArgs e)
        {

            fill_duty_kind(ddl_holidaykind.SelectedValue);

            DV_MONTH_DATA_ModalPopupExtender.Show();
        }

        protected void ddl_Kind_SelectedIndexChanged(object sender, EventArgs e)
        {
                        
            txt_pay.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT DUTY_PAY FROM C_ONDUTY_CODE WHERE DUTY_TYPE='1' AND DUTY_KIND='" + ddl_Kind.SelectedValue + "'");

            if (txt_DATE_START.Text.Length == 7 && txt_TIME_START.Text.Length == 4)
            {

                if (ddl_Kind.SelectedValue.Substring(1, 1) == "1")//全日
                {
                    DateTime END = TPPDDB.App_Code.DateManange.strLunar_to_DateTime(txt_DATE_START.Text, txt_TIME_START.Text );
                        //DateTime.Parse((int.Parse(txt_DATE_START.Text.Substring(0, 3)) + 1911).ToString() + "/" + txt_DATE_START.Text.Substring(3, 2) + "/" + txt_DATE_START.Text.Substring(5, 2)
                        //+ " " + txt_TIME_START.Text.Substring(0,2)+":"+txt_TIME_START.Text.Substring(2,2)+":00");
                    END = END.AddDays(1);

                    txt_DATE_END.Text = TPPDDB.App_Code.DateManange.DateTime_to_strLunar(END);//(END.Year - 1911).ToString() + END.Month.ToString().PadLeft(2, '0') + END.Day.ToString().PadLeft(2, '0');
                    txt_TIME_END.Text = END.Hour.ToString().PadLeft(2, '0') + END.Minute.ToString().PadLeft(2, '0'); ;
                }
                else if (ddl_Kind.SelectedValue.Substring(1, 1) == "2")//半日
                {
                    DateTime END = TPPDDB.App_Code.DateManange.strLunar_to_DateTime(txt_DATE_START.Text, txt_TIME_START.Text);
                        //DateTime.Parse((int.Parse(txt_DATE_START.Text.Substring(0, 3)) + 1911).ToString() + "/" + txt_DATE_START.Text.Substring(3, 2) + "/" + txt_DATE_START.Text.Substring(5, 2)
                         //  + " " + txt_TIME_START.Text.Substring(0, 2) + ":" + txt_TIME_START.Text.Substring(2, 2) + ":00");
                    END = END.AddHours(12);
                    txt_DATE_END.Text = TPPDDB.App_Code.DateManange.DateTime_to_strLunar(END);//(END.Year - 1911).ToString() + END.Month.ToString().PadLeft(2, '0') + END.Day.ToString().PadLeft(2, '0');
                    txt_TIME_END.Text = END.Hour.ToString().PadLeft(2, '0') + END.Minute.ToString().PadLeft(2, '0'); ;
                
                }
            }


            DV_MONTH_DATA_ModalPopupExtender.Show();

        }

        protected void btn_Save_Click(object sender, EventArgs e)
        {
            #region 防呆


            string error = "";
            if (string.IsNullOrEmpty(txt_DATE_START.Text) || string.IsNullOrEmpty(txt_TIME_START.Text) ||
                string.IsNullOrEmpty(txt_DATE_END.Text) || string.IsNullOrEmpty(txt_TIME_END.Text))
            {
                error += @"請輸入實際起迄日期及時間\r\n";

            }
            else
            {
                if (!TPPDDB.App_Code.DateManange.Check_date(txt_DATE_START.Text) || !TPPDDB.App_Code.DateManange.Check_date(txt_DATE_END.Text))
                {
                    error += @"實際起迄日期錯誤\r\n";
                }

                if (txt_TIME_START.Text.Length != 4 || txt_TIME_END.Text.Length != 4)
                {
                    error += @"實際起迄時間格式錯誤\r\n";
                }
                else
                {
                    if (int.Parse(txt_TIME_START.Text.Substring(0, 2)) > 24 || int.Parse(txt_TIME_END.Text.Substring(0, 2)) > 24 ||
                        int.Parse(txt_TIME_START.Text.Substring(2, 2)) > 60 || int.Parse(txt_TIME_END.Text.Substring(0, 2)) > 60)
                    {

                        error += @"實際起迄時間超出限定時分\r\n";
                    
                    }
                
                }
            
            
            }

            if (rbl_OVERTIME_HOUR_INSIDE.SelectedValue == "Y")
            {
                if(string.IsNullOrEmpty(txt_MEMO.Text.Trim()))
                error += @"值日補休須填寫備註內容,以帶入加班資料\r\n";
            }
          
            if (!string.IsNullOrEmpty(error))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('" + error + "');", true);
                DV_MONTH_DATA_ModalPopupExtender.Show();
                return;
            }
            #endregion 防呆

            if (lb_Title.Text == "新增")
            {

                string ErrorString = "";

                //這個看當日有沒有加班,超勤或值日.但因為有跨日問題.加班TABLE又沒有詳盡輸入時數.所以這樣判定有漏洞
                string pkey_check = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM C_OVERTIME_HOUR_INSIDE WHERE MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "'  AND RESTFLAG in ('YO', 'YU' )" + "AND MZ_DATE='" + lb_DATE.Text + "'");

                if (pkey_check != "0")
                {
                    ErrorString += "已有加班重複，無法新增" + "\\r\\n";

                }
                if (!string.IsNullOrEmpty(ErrorString))
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('" + ErrorString + "');", true);
                    DV_MONTH_DATA_ModalPopupExtender.Show();
                    return;
                }


                string strsql = @"INSERT INTO  C_ONDUTY_DAY (SN, MZ_EXAD, MZ_EXUNIT, MZ_ID, MZ_OCCC, DATE_TAG, KIND, PAY, TIME_SATRT, TIME_END, IS_CHK, IS_CHK_UNIT, IS_OVERTIME_HOUR_INSIDE, IS_POSITIVE, ADDDATE, ADDUSER,  MEMO)
                  VALUES( NEXT VALUE FOR dbo.C_ONDUTY_DAY_SN ,  @MZ_EXAD, @MZ_EXUNIT, @MZ_ID, @MZ_OCCC, @DATE_TAG, @KIND, @PAY, @TIME_SATRT, @TIME_END, @IS_CHK, @IS_CHK_UNIT, @IS_OVERTIME_HOUR_INSIDE, @IS_POSITIVE, @ADDDATE, @ADDUSER, @MEMO) ";

                DataTable ID_Detail_dt = o_DBFactory.ABC_toTest.Create_Table(" SELECT MZ_EXAD, MZ_EXUNIT ,MZ_OCCC FROM A_DLBASE WHERE MZ_ID='" + DropDownList_MZ_NAME.SelectedValue + "'","get");

                DateTime Time_Start = TPPDDB.App_Code.DateManange.strLunar_to_DateTime(txt_DATE_START.Text, txt_TIME_START.Text);
                //DateTime.Parse(TPPDDB.App_Code.DateManange.strtodate(txt_DATE_START.Text) + " " + txt_TIME_START.Text.Substring(0, 2) + ":" + txt_TIME_START.Text.Substring(2, 2) + ":00");
                DateTime Time_End = TPPDDB.App_Code.DateManange.strLunar_to_DateTime(txt_DATE_END.Text, txt_TIME_END.Text);
                    //DateTime.Parse(TPPDDB.App_Code.DateManange.strtodate(txt_DATE_END.Text) + " " + txt_TIME_END.Text.Substring(0, 2) + ":" + txt_TIME_END.Text.Substring(2, 2) + ":00");
                


               
                List<SqlParameter> par = new List<SqlParameter>();
                par.Add(new SqlParameter("MZ_EXAD", SqlDbType.VarChar) { Value = ID_Detail_dt.Rows[0]["MZ_EXAD"].ToString() });
                par.Add(new SqlParameter("MZ_EXUNIT", SqlDbType.VarChar) { Value = ID_Detail_dt.Rows[0]["MZ_EXUNIT"].ToString() });
                par.Add(new SqlParameter("MZ_ID", SqlDbType.VarChar) { Value = DropDownList_MZ_NAME.SelectedValue });
                par.Add(new SqlParameter("MZ_OCCC", SqlDbType.VarChar) { Value = ID_Detail_dt.Rows[0]["MZ_OCCC"].ToString() });
                par.Add(new SqlParameter("DATE_TAG", SqlDbType.VarChar) { Value = lb_DATE.Text });
                par.Add(new SqlParameter("KIND", SqlDbType.VarChar) { Value = ddl_Kind.SelectedValue });
                //par.Add(new SqlParameter("PAY", SqlDbType.Float) { Value =rbl_OVERTIME_HOUR_INSIDE.SelectedValue=="Y"?0: int.Parse(txt_pay.Text) });
                par.Add(new SqlParameter("PAY", SqlDbType.Float) { Value = int.Parse(txt_pay.Text) });
                par.Add(new SqlParameter("TIME_SATRT", SqlDbType.DateTime) { Value = Time_Start });
                par.Add(new SqlParameter("TIME_END", SqlDbType.DateTime) { Value = Time_End });
                par.Add(new SqlParameter("IS_CHK", SqlDbType.VarChar) { Value = "N" });
                par.Add(new SqlParameter("IS_CHK_UNIT", SqlDbType.VarChar) { Value = "N" });
                par.Add(new SqlParameter("IS_OVERTIME_HOUR_INSIDE", SqlDbType.VarChar) { Value = rbl_OVERTIME_HOUR_INSIDE.SelectedValue });
                par.Add(new SqlParameter("IS_POSITIVE", SqlDbType.VarChar) { Value = rbl_POSITIVE.SelectedValue });
                par.Add(new SqlParameter("ADDDATE", SqlDbType.DateTime) { Value = DateTime.Now });
                par.Add(new SqlParameter("ADDUSER", SqlDbType.VarChar) { Value = Session["ADPMZ_ID"].ToString() });
                par.Add(new SqlParameter("MEMO", SqlDbType.VarChar) { Value = txt_MEMO.Text });

                o_DBFactory.ABC_toTest.SQLExecute(strsql, par);


                if (rbl_OVERTIME_HOUR_INSIDE.SelectedValue == "Y")
                {
                    //20141231 如果已有先建加班資日資料
                    string have_OVERTIME_HOUR_INSIDE_date = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM C_OVERTIME_HOUR_INSIDE WHERE MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "'  AND RESTFLAG ='YD'" + "AND MZ_DATE='" + lb_DATE.Text + "'");
                    if (have_OVERTIME_HOUR_INSIDE_date != "0")
                    {
                        string strSQL = @"UPDATE C_OVERTIME_HOUR_INSIDE SET OTIME=@OTIME ,INS_ID =@INS_ID ,INS_DATE = @INS_DATE,OTREASON=@OTREASON WHERE MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "'AND MZ_DATE='" + lb_DATE.Text + "' AND RESTFLAG='YD'";

                        List<SqlParameter> parameterList = new List<SqlParameter>();


                        parameterList.Add(new SqlParameter("OTIME", SqlDbType.Float) { Value = int.Parse(o_DBFactory.ABC_toTest.vExecSQL("SELECT DUTY_HOUR FROM C_ONDUTY_CODE WHERE DUTY_KIND='" + ddl_Kind.SelectedValue + "'")) });
                        //parameterList.Add(new SqlParameter("HOUR_PAY", SqlDbType.Float) { Value = 0 });//??
                        //parameterList.Add(new SqlParameter("PAY_SUM", SqlDbType.Float) { Value = 0 });
                        parameterList.Add(new SqlParameter("OTREASON", SqlDbType.VarChar) { Value = txt_MEMO.Text });
                        // parameterList.Add(new SqlParameter("VFLAG", SqlDbType.VarChar) { Value = Convert.DBNull });
                        //parameterList.Add(new SqlParameter("MSUM", SqlDbType.Float) { Value = 0 });
                        parameterList.Add(new SqlParameter("INS_ID", SqlDbType.VarChar) { Value = Session["ADPMZ_ID"].ToString() });
                        parameterList.Add(new SqlParameter("INS_DATE", SqlDbType.VarChar) { Value = (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0') + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') });
                        //parameterList.Add(new SqlParameter("MZ_DATE", SqlDbType.VarChar) { Value = lb_DATE.Text });
                        try
                        {
                            o_DBFactory.ABC_toTest.SQLExecute(strSQL, parameterList);

                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('值日及加班補休修改成功');", true);
                            ////2010.06.04 LOG紀錄 by伊珊
                            //TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00002", TPMPermissions._boolTPM(), o_DBFactory.ABC_toTest.RegexSQL(strSQL, parameterList));


                        }
                        catch
                        {
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('值日及加班補休新增失敗');", true);
                        }
                    }
                    else
                    {

                        //寫入加班資料
                        string strSQL = @"INSERT INTO C_OVERTIME_HOUR_INSIDE (MZ_EXUNIT,MZ_ID,MZ_OCCC,RESTFLAG,OTIME,HOUR_PAY,PAY_SUM,OTREASON, VFLAG,MSUM,INS_ID,INS_DATE,MZ_EXAD,MZ_DATE )
                                       VALUES (@MZ_EXUNIT,@MZ_ID,@MZ_OCCC,@RESTFLAG,@OTIME,@HOUR_PAY,@PAY_SUM,@OTREASON,@VFLAG,@MSUM,@INS_ID,@INS_DATE,@MZ_EXAD,@MZ_DATE) ";

                        List<SqlParameter> parameterList = new List<SqlParameter>();


                        parameterList.Add(new SqlParameter("MZ_EXUNIT", SqlDbType.VarChar) { Value = ID_Detail_dt.Rows[0]["MZ_EXUNIT"].ToString() });
                        parameterList.Add(new SqlParameter("MZ_ID", SqlDbType.VarChar) { Value = DropDownList_MZ_NAME.SelectedValue });
                        parameterList.Add(new SqlParameter("MZ_OCCC", SqlDbType.VarChar) { Value = ID_Detail_dt.Rows[0]["MZ_OCCC"].ToString() });
                        parameterList.Add(new SqlParameter("RESTFLAG", SqlDbType.VarChar) { Value = "YD" });
                        parameterList.Add(new SqlParameter("OTIME", SqlDbType.Float) { Value = int.Parse(o_DBFactory.ABC_toTest.vExecSQL("SELECT DUTY_HOUR FROM C_ONDUTY_CODE WHERE DUTY_KIND='" + ddl_Kind.SelectedValue + "'")) });
                        parameterList.Add(new SqlParameter("HOUR_PAY", SqlDbType.Float) { Value = 0 });//??
                        parameterList.Add(new SqlParameter("PAY_SUM", SqlDbType.Float) { Value = 0 });
                        parameterList.Add(new SqlParameter("OTREASON", SqlDbType.VarChar) { Value = txt_MEMO.Text });
                        //parameterList.Add(new SqlParameter("OTREASON", SqlDbType.VarChar) { Value = lb_DATE.Text + "值日補休" });
                        parameterList.Add(new SqlParameter("VFLAG", SqlDbType.VarChar) { Value = Convert.DBNull });
                        parameterList.Add(new SqlParameter("MSUM", SqlDbType.Float) { Value = 0 });
                        parameterList.Add(new SqlParameter("INS_ID", SqlDbType.VarChar) { Value = Session["ADPMZ_ID"].ToString() });
                        parameterList.Add(new SqlParameter("INS_DATE", SqlDbType.VarChar) { Value = (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0') + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') });
                        parameterList.Add(new SqlParameter("MZ_DATE", SqlDbType.VarChar) { Value = lb_DATE.Text });
                        parameterList.Add(new SqlParameter("MZ_EXAD", SqlDbType.VarChar) { Value = ID_Detail_dt.Rows[0]["MZ_EXAD"].ToString() });


                        try
                        {
                            o_DBFactory.ABC_toTest.SQLExecute(strSQL, parameterList);

                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('值日及加班補休新增成功');", true);
                            ////2010.06.04 LOG紀錄 by伊珊
                            //TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00002", TPMPermissions._boolTPM(), o_DBFactory.ABC_toTest.RegexSQL(strSQL, parameterList));


                        }
                        catch
                        {
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('值日新增成功,及加班補休新增失敗');", true);
                        }

                    }
                }
                else
                {

                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('值日新增成功');", true);
                }
                
            }
            else if (lb_Title.Text == "修改")
            {


                string strsql = @"UPDATE  C_ONDUTY_DAY SET  KIND=@KIND, PAY=@PAY, TIME_SATRT=@TIME_SATRT, TIME_END=@TIME_END, IS_OVERTIME_HOUR_INSIDE=@IS_OVERTIME_HOUR_INSIDE, IS_POSITIVE=@IS_POSITIVE,  MEMO=@MEMO 
                               WHERE SN=@SN";


                DateTime Time_Start = TPPDDB.App_Code.DateManange.strLunar_to_DateTime(txt_DATE_START.Text, txt_TIME_START.Text);
                    //DateTime.Parse(TPPDDB.App_Code.DateManange.strtodate(txt_DATE_START.Text) + " " + txt_TIME_START.Text.Substring(0, 2) + ":" + txt_TIME_START.Text.Substring(2, 2) + ":00");
                DateTime Time_End = TPPDDB.App_Code.DateManange.strLunar_to_DateTime(txt_DATE_END.Text, txt_TIME_END.Text);
                    //DateTime.Parse(TPPDDB.App_Code.DateManange.strtodate(txt_DATE_END.Text) + " " + txt_TIME_END.Text.Substring(0, 2) + ":" + txt_TIME_END.Text.Substring(2, 2) + ":00");



               
                List<SqlParameter> par = new List<SqlParameter>();

                par.Add(new SqlParameter("SN", SqlDbType.Float) { Value = int.Parse(lb_SN.Text) });
                par.Add(new SqlParameter("KIND", SqlDbType.VarChar) { Value = ddl_Kind.SelectedValue });
                //par.Add(new SqlParameter("PAY", SqlDbType.Float) { Value = rbl_OVERTIME_HOUR_INSIDE.SelectedValue == "Y" ? 0 : int.Parse(txt_pay.Text) });
                par.Add(new SqlParameter("PAY", SqlDbType.Float) { Value =  int.Parse(txt_pay.Text) });
                par.Add(new SqlParameter("TIME_SATRT", SqlDbType.DateTime) { Value = Time_Start });
                par.Add(new SqlParameter("TIME_END", SqlDbType.DateTime) { Value = Time_End });
                par.Add(new SqlParameter("IS_OVERTIME_HOUR_INSIDE", SqlDbType.VarChar) { Value = rbl_OVERTIME_HOUR_INSIDE.SelectedValue });
                par.Add(new SqlParameter("IS_POSITIVE", SqlDbType.VarChar) { Value = rbl_POSITIVE.SelectedValue });
                par.Add(new SqlParameter("MEMO", SqlDbType.VarChar) { Value = txt_MEMO.Text });
                o_DBFactory.ABC_toTest.SQLExecute(strsql, par);

                if (rbl_OVERTIME_HOUR_INSIDE.SelectedValue == "Y")
                {
                    //寫入加班資料
                    //20141231 如果已有先建加班資日資料
                    string pkey_check = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM C_OVERTIME_HOUR_INSIDE WHERE MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "'AND MZ_DATE='" + lb_DATE.Text + "' AND RESTFLAG='YD'");

                    if (pkey_check == "0")
                    {

                        DataTable ID_Detail_dt = o_DBFactory.ABC_toTest.Create_Table(" SELECT MZ_EXAD, MZ_EXUNIT ,MZ_OCCC FROM A_DLBASE WHERE MZ_ID='" + DropDownList_MZ_NAME.SelectedValue + "'", "get");


                        //寫入加班資料
                        string strSQL = @"INSERT INTO C_OVERTIME_HOUR_INSIDE (MZ_EXUNIT,MZ_ID,MZ_OCCC,RESTFLAG,OTIME,HOUR_PAY,PAY_SUM,OTREASON, VFLAG,MSUM,INS_ID,INS_DATE,MZ_EXAD,MZ_DATE )
                                       VALUES (@MZ_EXUNIT,@MZ_ID,@MZ_OCCC,@RESTFLAG,@OTIME,@HOUR_PAY,@PAY_SUM,@OTREASON,@VFLAG,@MSUM,@INS_ID,@INS_DATE,@MZ_EXAD,@MZ_DATE) ";

                        List<SqlParameter> parameterList = new List<SqlParameter>();


                        parameterList.Add(new SqlParameter("MZ_EXUNIT", SqlDbType.VarChar) { Value = ID_Detail_dt.Rows[0]["MZ_EXUNIT"].ToString() });
                        parameterList.Add(new SqlParameter("MZ_ID", SqlDbType.VarChar) { Value = DropDownList_MZ_NAME.SelectedValue });
                        parameterList.Add(new SqlParameter("MZ_OCCC", SqlDbType.VarChar) { Value = ID_Detail_dt.Rows[0]["MZ_OCCC"].ToString() });
                        parameterList.Add(new SqlParameter("RESTFLAG", SqlDbType.VarChar) { Value = "YD" });
                        parameterList.Add(new SqlParameter("OTIME", SqlDbType.Float) { Value = int.Parse(o_DBFactory.ABC_toTest.vExecSQL("SELECT DUTY_HOUR FROM C_ONDUTY_CODE WHERE DUTY_KIND='" + ddl_Kind.SelectedValue + "'")) });
                        parameterList.Add(new SqlParameter("HOUR_PAY", SqlDbType.Float) { Value = 0 });//??
                        parameterList.Add(new SqlParameter("PAY_SUM", SqlDbType.Float) { Value = 0 });
                        //20141231
                        parameterList.Add(new SqlParameter("OTREASON", SqlDbType.VarChar) { Value = txt_MEMO.Text });
                        //parameterList.Add(new SqlParameter("OTREASON", SqlDbType.VarChar) { Value = lb_DATE.Text + "值日補休" });
                        parameterList.Add(new SqlParameter("VFLAG", SqlDbType.VarChar) { Value = Convert.DBNull });
                        parameterList.Add(new SqlParameter("MSUM", SqlDbType.Float) { Value = 0 });
                        parameterList.Add(new SqlParameter("INS_ID", SqlDbType.VarChar) { Value = Session["ADPMZ_ID"].ToString() });
                        parameterList.Add(new SqlParameter("INS_DATE", SqlDbType.VarChar) { Value = (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0') + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') });
                        parameterList.Add(new SqlParameter("MZ_DATE", SqlDbType.VarChar) { Value = lb_DATE.Text });
                        parameterList.Add(new SqlParameter("MZ_EXAD", SqlDbType.VarChar) { Value = ID_Detail_dt.Rows[0]["MZ_EXAD"].ToString() });

                        try
                        {
                            o_DBFactory.ABC_toTest.SQLExecute(strSQL, parameterList);

                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('值日修改成功,加班補休新增成功');", true);
                            ////2010.06.04 LOG紀錄 by伊珊
                            //TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00002", TPMPermissions._boolTPM(), o_DBFactory.ABC_toTest.RegexSQL(strSQL, parameterList));


                        }
                        catch
                        {
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('值日修改成功,加班補休修改失敗');", true);
                        }

                    }
                    else
                    {

                        string strSQL = @"UPDATE C_OVERTIME_HOUR_INSIDE SET OTIME=@OTIME ,INS_ID =@INS_ID ,INS_DATE = @INS_DATE,OTREASON=@OTREASON WHERE MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "'AND MZ_DATE='" + lb_DATE.Text + "' AND RESTFLAG='YD'";

                        List<SqlParameter> parameterList = new List<SqlParameter>();


                        parameterList.Add(new SqlParameter("OTIME", SqlDbType.Float) { Value = int.Parse(o_DBFactory.ABC_toTest.vExecSQL("SELECT DUTY_HOUR FROM C_ONDUTY_CODE WHERE DUTY_KIND='" + ddl_Kind.SelectedValue + "'")) });
                        //parameterList.Add(new SqlParameter("HOUR_PAY", SqlDbType.Float) { Value = 0 });//??
                        //parameterList.Add(new SqlParameter("PAY_SUM", SqlDbType.Float) { Value = 0 });
                        parameterList.Add(new SqlParameter("OTREASON", SqlDbType.VarChar) { Value = txt_MEMO.Text });
                        // parameterList.Add(new SqlParameter("VFLAG", SqlDbType.VarChar) { Value = Convert.DBNull });
                        //parameterList.Add(new SqlParameter("MSUM", SqlDbType.Float) { Value = 0 });
                        parameterList.Add(new SqlParameter("INS_ID", SqlDbType.VarChar) { Value = Session["ADPMZ_ID"].ToString() });
                        parameterList.Add(new SqlParameter("INS_DATE", SqlDbType.VarChar) { Value = (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0') + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') });
                        //parameterList.Add(new SqlParameter("MZ_DATE", SqlDbType.VarChar) { Value = lb_DATE.Text });
                        try
                        {
                            o_DBFactory.ABC_toTest.SQLExecute(strSQL, parameterList);

                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('值日及加班補休修改成功');", true);
                            ////2010.06.04 LOG紀錄 by伊珊
                            //TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00002", TPMPermissions._boolTPM(), o_DBFactory.ABC_toTest.RegexSQL(strSQL, parameterList));


                        }
                        catch
                        {
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('值日及加班補休修改失敗');", true);
                        }
                    }


                }
                else
                {

                    //如果本選填補休又決定不要.刪掉加班資料
                    string pkey_check = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM C_OVERTIME_HOUR_INSIDE WHERE MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "'AND MZ_DATE='" + lb_DATE.Text + "' AND RESTFLAG='YD'");

                    if (pkey_check != "0")
                    {

                        o_DBFactory.ABC_toTest.vExecSQL("DELETE C_OVERTIME_HOUR_INSIDE WHERE MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "'AND MZ_DATE='" + lb_DATE.Text + "' AND RESTFLAG='YD'");


                      

                    }





                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('值日修改成功');", true);
                }
            }


            Search_Data(DropDownList_MZ_NAME.SelectedValue, TextBox_DUTYMONTH.Text);



        }

        protected void btn_Delete_Click(object sender, EventArgs e)
        {


            string strsql = @"DELETE  C_ONDUTY_DAY  WHERE SN=@SN";

            
            List<SqlParameter> par = new List<SqlParameter>();

            par.Add(new SqlParameter("SN", SqlDbType.Float) { Value = int.Parse(lb_SN.Text) });
            o_DBFactory.ABC_toTest.SQLExecute(strsql, par);

            //if (rbl_OVERTIME_HOUR_INSIDE.SelectedValue == "Y")
            //{
                //刪除加班資料
                string pkey_check = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM C_OVERTIME_HOUR_INSIDE WHERE MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "'AND MZ_DATE='" + lb_DATE.Text + "' AND RESTFLAG='YD'");

                if (pkey_check != "0")
                {
                    string strSQL ="DELETE C_OVERTIME_HOUR_INSIDE  WHERE MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "'AND MZ_DATE='" + lb_DATE.Text + "' AND RESTFLAG='YD'";
                    o_DBFactory.ABC_toTest.vExecSQL(strSQL);
                }
                

            
            //}



            Search_Data(DropDownList_MZ_NAME.SelectedValue, TextBox_DUTYMONTH.Text);

        }

        #endregion 編輯視窗事件

        #region 收尋其他人員視窗事件


        /// <summary>
        /// 尋找其他人員的收尋按鈕顯示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_search1_Click(object sender, EventArgs e)
        {

            pl_Search_ModalPopupExtender.Show();
        }
        
        
        /// <summary>
        /// 尋找其他人員的收尋按鈕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_search2_Click(object sender, EventArgs e)
        {
            string error = "";
            if (!string.IsNullOrEmpty(txtdate_search.Text))
            {
                if (txtdate_search.Text.Length !=5)
                {

                    error += @"請輸入正確日期\r\n";

                }
            }
            if (!string.IsNullOrEmpty(txt_ID.Text))
            {
                if (txt_ID.Text .Length != 10)
                {

                    error += @"請輸入正確身分證號\r\n";

                }
            }


            if (!string.IsNullOrEmpty(error))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('" + error + "');", true);
                pl_Search_ModalPopupExtender.Show();
                return;
            }
             
            

            fill_ID(ddl_EXAD.SelectedValue, ddl_EXUNIT.SelectedValue);



            if (txt_ID.Text != ""  )
            {
                
                DropDownList_MZ_NAME.SelectedValue = txt_ID.Text;
               
            }
            else if (txt_NAME.Text != "")
            {
                DropDownList_MZ_NAME.SelectedValue = o_A_DLBASE.name_ID(txt_NAME.Text);
            }


            if (txtdate_search.Text != "")
            {
                TextBox_DUTYMONTH.Text = txtdate_search.Text;
            }


            Search_Data(DropDownList_MZ_NAME.SelectedValue,  TextBox_DUTYMONTH.Text );

            TextBox_MZ_ID.Text = DropDownList_MZ_NAME.SelectedValue;


            //查完初始化
            txt_ID.Text = "";
            txt_NAME.Text = "";
            txtdate_search.Text = "";
        }

       

        protected void ddl_EXAD_SelectedIndexChanged(object sender, EventArgs e)
        {
            C.fill_unit(ddl_EXUNIT, ddl_EXAD.SelectedValue);
            pl_Search_ModalPopupExtender.Show();
        }


        #endregion 收尋其他人員視窗事件


        //protected void rbl_OVERTIME_HOUR_INSIDE_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (rbl_OVERTIME_HOUR_INSIDE.SelectedValue == "Y")
        //    {
        //        txt_pay.Text = "0";
        //    }
        //    else
        //    {
        //        string strsql = string.Format("SELECT DUTY_PAY FROM C_ONDUTY_CODE WHERE DUTY_TYPE='1' AND DUTY_KIND='" + ddl_Kind.SelectedValue + "' order by DUTY_KIND");

        //        txt_pay.Text = o_DBFactory.ABC_toTest.vExecSQL(strsql);
        //    }
            
        //    DV_MONTH_DATA_ModalPopupExtender.Show();
        //}
    }
}
