using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using TPPDDB.Logic;

namespace TPPDDB._3_forleave
{
    public partial class C_Overtimepay_detail_rpt : System.Web.UI.Page
    {
        int TPM_FION = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                C.check_power();
                //C.fill_AD_POST(DropDownList_AD);
                //C.fill_unit(DropDownList_UNIT, DropDownList_AD.SelectedValue);

                switch (o_a_Function.strGID(Session["ADPMZ_ID"].ToString()))
                {
                    case "A":
                        ChangeDropDownList_AD_ALL();
                        DropDownList_AD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                        ChangeUnit();
                        break;
                    case "B":
                        ChangeDropDownList_AD_ALL();
                        //DropDownList_AD.DataBind();
                        DropDownList_AD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                        ChangeUnit();
                        break;
                    case "C":
                        ChangeDropDownList_AD();
                        //DropDownList_AD.DataBind();
                        DropDownList_AD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                        //DropDownList_AD.Enabled = false;
                        //如果是中和分局進來改成只抓中和一&中和二&中和 matthew
                        if (Session["ADPMZ_EXAD"].ToString() != "382133600C")
                        {
                            DropDownList_AD.Enabled = false;
                        }
                        ChangeUnit();
                        break;
                    case "D":
                        ChangeDropDownList_AD();
                        //DropDownList_AD.DataBind();
                        DropDownList_AD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                        //DropDownList_AD.Enabled = false;
                        //如果是中和分局進來改成只抓中和一&中和二&中和 matthew
                        if (Session["ADPMZ_EXAD"].ToString() != "382133600C")
                        {
                            DropDownList_AD.Enabled = false;
                        }
                        ChangeUnit();
                        DropDownList_UNIT.SelectedValue = Session["ADPMZ_EXUNIT"].ToString();
                        DropDownList_UNIT.Enabled = false;
                        TextBox_MZ_ID.Text = Session["ADPMZ_ID"].ToString();
                        TextBox_MZ_ID.Enabled = false;

                        break;
                    case "E":
                        ChangeDropDownList_AD();
                        //DropDownList_AD.DataBind();
                        DropDownList_AD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                        //DropDownList_AD.Enabled = false;
                        //如果是中和分局進來改成只抓中和一&中和二&中和 matthew
                        if (Session["ADPMZ_EXAD"].ToString() != "382133600C")
                        {
                            DropDownList_AD.Enabled = false;
                        }
                        ChangeUnit();
                        DropDownList_UNIT.Text = Session["ADPMZ_EXUNIT"].ToString();
                        DropDownList_UNIT.Enabled = false;

                        break;
                }


                //MQ-----------------------20100331
                C.set_Panel_EnterToTAB(ref this.Panel1);




            }
        }
        //matthew 中和分局
        protected void ChangeDropDownList_AD()
        {
            string strSQL = "SELECT MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KCODE LIKE '38213%' AND MZ_KTYPE='04'";
            DataTable tempDT = new DataTable();
            tempDT = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
            //如果是中和分局進來改成只抓中和一&中和二&中和 matthew
            if (Session["ADPMZ_EXAD"].ToString() == "382133600C")
            {
                strSQL = "SELECT  MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE = '04' AND (MZ_KCODE in ('382133400C','382133500C','382133600C'))";
            }
            else
            {
                strSQL = "SELECT  MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE = '04' AND (MZ_KCODE='" + Session["ADPMZ_EXAD"].ToString() + "')";
                //strSQL = "SELECT  MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE = '04' AND (MZ_KCODE='" + tempDT.Rows[0][1].ToString() + "')";
            }
            DataTable dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");
            DropDownList_AD.DataSource = dt;
            DropDownList_AD.DataTextField = "MZ_KCHI";
            DropDownList_AD.DataValueField = "MZ_KCODE";
            DropDownList_AD.DataBind();

            DropDownList_AD.SelectedValue = Session["ADPMZ_EXAD"].ToString();

        }
        protected void ChangeDropDownList_AD_ALL()
        {
            string strSQL = "SELECT MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KCODE LIKE '38213%' AND MZ_KTYPE='04'";
            DataTable tempDT = new DataTable();
            tempDT = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
            //如果是中和分局進來改成只抓中和一&中和二&中和 matthew
            //if (Session["ADPMZ_EXAD"].ToString() == "382133600C")
            //{
            //    strSQL = "SELECT  MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE = '04' AND (MZ_KCODE in ('382133400C','382133500C','382133600C'))";
            //}
            //else
            //{
            //    strSQL = "SELECT  MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE = '04' AND (MZ_KCODE='" + tempDT.Rows[0][1].ToString() + "')";
            //    //strSQL = "SELECT  MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE = '04' AND (MZ_KCODE='" + tempDT.Rows[0][1].ToString() + "')";
            //}
            DataTable dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");
            DropDownList_AD.DataSource = dt;
            DropDownList_AD.DataTextField = "MZ_KCHI";
            DropDownList_AD.DataValueField = "MZ_KCODE";
            DropDownList_AD.DataBind();

            DropDownList_AD.SelectedValue = Session["ADPMZ_EXAD"].ToString();

        }

        protected void ChangeUnit()
        {
            DataTable temp = new DataTable();
            string strSQL = "SELECT RTRIM(MZ_KCHI),RTRIM(MZ_KCODE) FROM A_KTYPE WHERE MZ_KTYPE='25' AND MZ_KCODE IN (SELECT MZ_UNIT FROM A_UNIT_AD WHERE MZ_AD='" + DropDownList_AD.SelectedValue + "')";
            temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");
            DropDownList_UNIT.DataSource = temp;
            DropDownList_UNIT.DataTextField = "RTRIM(MZ_KCHI)";
            DropDownList_UNIT.DataValueField = "RTRIM(MZ_KCODE)";
            DropDownList_UNIT.DataBind();
            DropDownList_UNIT.Items.Insert(0, "");
            //如果是中和分局進來改成只抓中和一&中和二&中和 matthew
            if (Session["ADPMZ_EXAD"].ToString() == "382133600C")
            {
                DropDownList_UNIT.Text = Session["ADPMZ_EXUNIT"].ToString();
            }
        }

        protected void btPrint_Click(object sender, EventArgs e)
        {
            //產生本報表需要的資料,初始化
            DataTable rpt = Create_Report_DataTable_Init();

            //請輸入身分證號
            if (string.IsNullOrEmpty(TextBox_MZ_ID.Text))
            {
                ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "alert('請輸入身分證號');", true);
                return;
            }

            if (Radio_Type.SelectedValue == "0")//_ㄧ般人員
            {
                GeneralPeople_Processing(rpt);

            }
            else if (Radio_Type.SelectedValue == "1")//_刑事、少年、婦幼
            {
                //刑事 382130200C 少年 382135000C  婦幼 382135000C

                OtherPeople_Processing(rpt);
            }

        }

        /// <summary>
        /// 產生本報表需要的資料,初始化
        /// </summary>
        /// <returns></returns>
        private static DataTable Create_Report_DataTable_Init()
        {
            DataTable rpt = new DataTable();

            rpt.Columns.Add("MZ_ID", typeof(string));
            rpt.Columns.Add("MZ_NAME", typeof(string));
            rpt.Columns.Add("MZ_OCCC", typeof(string));
            rpt.Columns.Add("MZ_SRANK", typeof(string));
            rpt.Columns.Add("ITEMA", typeof(string));
            rpt.Columns.Add("ITEMB", typeof(string));
            rpt.Columns.Add("ITEMC", typeof(string));
            rpt.Columns.Add("ITEMD", typeof(string));
            rpt.Columns.Add("ITEME", typeof(string));
            rpt.Columns.Add("ITEMF", typeof(string));
            rpt.Columns.Add("ITEMG", typeof(string));
            rpt.Columns.Add("ITEMH", typeof(string));
            rpt.Columns.Add("ITEMI", typeof(string));
            rpt.Columns.Add("ITEMII", typeof(string));
            rpt.Columns.Add("TOTAL", typeof(string));
            rpt.Columns.Add("LIMIT", typeof(string));
            rpt.Columns.Add("OVERTIME", typeof(string));
            rpt.Columns.Add("ISHOLIDAY", typeof(string));
            rpt.Columns.Add("DATE", typeof(string));
            rpt.Columns.Add("MZ_MEMO", typeof(string));
            rpt.Columns.Add("ITEMM_ADDHOUR", typeof(string));
            rpt.Columns.Add("REST_HOUR", typeof(string));
            return rpt;
        }


        /// <summary>   ㄧ般人員 </summary>
        private void GeneralPeople_Processing(DataTable rpt)
        {
            //若無機關，則跳出。
            if (string.IsNullOrEmpty(DropDownList_AD.SelectedValue))
            {
                return;
            }

            //讀取資料
            Load_DataTable(rpt, "0");

            //如果有抓到資料
            if (rpt.Rows.Count > 0)
            {
                Session["rpt_dt"] = rpt;

                Session["TITLE"] = string.Format("{0}{1}{2}年{3}月超勤時數統計表", DropDownList_AD.SelectedItem.Text, DropDownList_UNIT.SelectedItem.Text, int.Parse(TextBox_DUTYDATE.Text.Substring(0, 3)).ToString(), int.Parse(TextBox_DUTYDATE.Text.Substring(3, 2)).ToString());
                //抓取內外勤參數
                Session["DutyUserClassify"] = Service.AccountService.getDutyUserClassify_V1(TextBox_MZ_ID.Text).ToString();

                string tmp_url = "C_rpt.aspx?fn=Overtimepay_detail&TPM_FION=" + TPM_FION;

                ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "alert('無此日期之相關資料');", true);

            }
        }

        /// <summary>
        /// 讀取與計算資料
        /// </summary>
        /// <param name="rpt">DataTable 資料</param>
        /// <param name="Type">對象類型,0:一般 , 1:刑事、少年、婦幼</param>
        private void Load_DataTable(DataTable rpt, string Type)
        {
            DataTable tempDT;

            //抓取資料,主要是以下加班相關資料表:
            //C_DUTYTABLE_PERSONAL
            //C_OVERTIME_BASE
            //C_ONDUTY_DAY
            //原則上抓取出來的資料是,每人每天一筆
            Get_DataTable(rpt, out tempDT);

            //處理每一天的迴圈
            for (int i = 0; i < tempDT.Rows.Count; i++)
            {
                //統計當天累積時數  //TODO 跑迴圈且一筆一筆回資料庫統計.想死人嗎
                String MZ_ID = tempDT.Rows[i]["MZ_ID"].ToString();
                String FULLDATE = tempDT.Rows[i]["DUTYDATE"].ToString(); //包含日期
                String DATE = FULLDATE.Substring(5, 2); //僅有日數

                //特殊:會因為對象類型,0:一般 , 1:刑事、少年、婦幼
                //不同而使用不同的勤務代碼(ABCDE)

                //對象 0:一般人員
                string Mode_A = "A";
                string Mode_B = "B";
                string Mode_C = "C";
                string Mode_D = "D";
                string Mode_E = "E";
                // 1:刑事、少年、婦幼,用的勤務項目代碼
                if (Type == "1")
                {
                    Mode_A = "A";
                    Mode_B = "Q";
                    Mode_C = "R";
                    Mode_D = "S";
                    Mode_E = "K";
                }
                string ITEMA = Service_C_DUTYTABLE_PERSONAL.Count(MZ_ID, Mode_A, tempDT.Rows[i]["DUTYDATE"].ToString());
                string ITEMB = Service_C_DUTYTABLE_PERSONAL.Count(MZ_ID, Mode_B, tempDT.Rows[i]["DUTYDATE"].ToString());
                string ITEMC = Service_C_DUTYTABLE_PERSONAL.Count(MZ_ID, Mode_C, tempDT.Rows[i]["DUTYDATE"].ToString());
                string ITEMD = Service_C_DUTYTABLE_PERSONAL.Count(MZ_ID, Mode_D, tempDT.Rows[i]["DUTYDATE"].ToString());
                string ITEME = Service_C_DUTYTABLE_PERSONAL.Count(MZ_ID, Mode_E, tempDT.Rows[i]["DUTYDATE"].ToString()); //已改用廢棄函數，應廢掉

                //下面的都一樣,不因對象受影響
                string ITEMF = Service_C_DUTYTABLE_PERSONAL.Count(MZ_ID, "F", tempDT.Rows[i]["DUTYDATE"].ToString());
                string ITEMG = Service_C_DUTYTABLE_PERSONAL.Count(MZ_ID, "G", tempDT.Rows[i]["DUTYDATE"].ToString()); //已改用廢棄函數，應廢掉
                // Joy 新增 判斷是否有值宿
                string ITEMM = Service_C_DUTYTABLE_PERSONAL.Count(MZ_ID, "M", tempDT.Rows[i]["DUTYDATE"].ToString());
                //Joy 20150408 常訓併計於其他勤務下

                //已改用廢棄函數，應廢掉
                string ITEMH = Convert.ToString(int.Parse(Service_C_DUTYTABLE_PERSONAL.Count(tempDT.Rows[i]["MZ_ID"].ToString(), "H", tempDT.Rows[i]["DUTYDATE"].ToString())) + int.Parse(Service_C_DUTYTABLE_PERSONAL.Count(tempDT.Rows[i]["MZ_ID"].ToString(), "J", tempDT.Rows[i]["DUTYDATE"].ToString())));
                string ITEMI = Service_C_DUTYTABLE_PERSONAL.Count(MZ_ID, "I", tempDT.Rows[i]["DUTYDATE"].ToString());

                //sam 想算一日二十四小時的累積分數 可以從這 think about it!!!
                //G督勤 
                int ITEMGG = 0;
                //EX:　ITEMG="20.30.45" ,代表當天G類型勤務分別加班多少時間
                //EX:  ITEMGG=3
                if (ITEMG != "")
                {
                    string[] COUNTG = ITEMG.Split('.');
                    ITEMGG = COUNTG.Length;
                }

                //I專案情務
                int ITEMII = 0;
                if (ITEMI != "")
                {
                    string[] COUNTI = ITEMI.Split('.');
                    ITEMII = COUNTI.Length;
                }

                //判斷是否為國定假日
                String s = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_HOLIDAY_NAME FROM C_DUTYHOLIDAY WHERE MZ_HOLIDAY_DATE='" + tempDT.Rows[i]["DUTYDATE"].ToString() + "'");
                string tmp = "補假";
                string tmp2 = "彈性";
                Boolean isHoliday = false;
                string strdate = tempDT.Rows[i]["DUTYDATE"].ToString();
                if (!String.IsNullOrEmpty(s))
                {
                    if (!s.Contains(tmp) && !s.Contains(tmp2))
                    {
                        isHoliday = true;
                    }
                }

                //開始計算超勤當日
                Int32 DutyTime = 0;
                Int32 OverTime = 0;
                Int32 HAmount = 0;
                Int32 EAmount = 0;


                ////sam test
                //if (FULLDATE == "1090806")
                //{
                //}

                ////sam test 測試時段
                //if (FULLDATE == "1090808")
                //{
                //    ITEMG = "0906-1006.1006-1156.1320-1420.1420-1520.1520-1650";
                //}

                LogicOvertime Logic = new LogicOvertime();
                //EX:  {"A","B","B","C","D","Y","F","B"......}
                //List<String> lstSingleDayInputs = Service_C_DUTYTABLE_PERSONAL.GetSingleDayOverTimeList(MZ_ID, FULLDATE); //每日超勤狀況使用者輸入值
                //抓取當天的超勤資料模型
                LogicModel_C_DUTYTABLE_PERSONAL data_DUTYTABLE_PERSONAL = new LogicModel_C_DUTYTABLE_PERSONAL(MZ_ID, FULLDATE);
                
                /// Joy 新增判斷是否停休
                String LastValue = GetDutyStopOff(MZ_ID, FULLDATE);
                Boolean isDutyStopOff = (LastValue == "Y") ? true : false;

                //特殊:勤務時數計算的相關邏輯,會因為對象類型影響
                // 需要注意的是,目前版本其實只需要DutyTime的結果,而OverTime其實不需要了,後面會有另外一段SQL語法去算超勤時數
                //0:ㄧ般人員
                if (Type == "0")
                {
                    // sam 20200908 職務：局長、副局長、主任祕書、督察長 這四個職務可超勤時數大於四小時
                    string str_OCCC = o_A_DLBASE.OCCC(TextBox_MZ_ID.Text);
                    //EX: ITEMG="2156-2200.2300-2400"

                    //ref OverTime(超勤時數) 其實後面沒有用到,但因為該模組會回傳,故還是要納入ref參數之一
                    data_DUTYTABLE_PERSONAL.calSingleDayOvertime_MZ_OCCC(ref DutyTime, ref OverTime, str_OCCC, ITEMG, isHoliday); //取得上班時數總時數及超勤時數總時數
                    //Logic.calSingleDayOvertime_MZ_OCCC(data_DUTYTABLE_PERSONAL.lstSingleDayInputs, ref DutyTime, ref OverTime, str_OCCC, ITEMG, isDutyStopOff, isHoliday); //取得上班時數總時數及超勤時數總時數


                    Logic.calSingleDayOverTime_HAmount(data_DUTYTABLE_PERSONAL.lstSingleDayInputs, ref HAmount); //取得其他勤務總時數
                    Logic.calSingleDayOverTime_EAmount(data_DUTYTABLE_PERSONAL.lstSingleDayInputs, ref EAmount); //取得勤區查察總時數
                }
                //1:刑事、少年、婦幼
                else if (Type == "1")
                {
                    //ref OverTime(超勤時數) 其實後面沒有用到,但因為該模組會回傳,故還是要納入ref參數之一
                    data_DUTYTABLE_PERSONAL.calSingleDayOvertime( ref DutyTime, ref OverTime,  isHoliday);//取得上班時數總時數及超勤時數總時數
                    //Logic.calSingleDayOvertime(data_DUTYTABLE_PERSONAL.lstSingleDayInputs, ref DutyTime, ref OverTime, isDutyStopOff, isHoliday); //取得上班時數總時數及超勤時數總時數
                    Logic.calSingleDayOverTime_HAmount_forOther(data_DUTYTABLE_PERSONAL.lstSingleDayInputs, ref HAmount); //取得其他勤務總時數
                }

                int ITEMM_ADDHOUR = 0;
                //Joy 判斷是否有值宿且超勤時數>12
                if (ITEMM != "0" && DutyTime > 12)
                {
                    ITEMM_ADDHOUR = DutyTime - 12;
                }

                //追加超勤補休時數,因為該時數等同於沒有申報的超勤時數,雖然人在放假,但也得列入
                Int32 RestHour = Service_C_DUTYTABLE_PERSONAL.GetSingleDayOvertimeRest(MZ_ID, FULLDATE);
                //因為沒有實際用到透過工作項目計算的超勤時數,故以下這段註解掉吧
                //OverTime += RestHour;                

                //增加超勤時數計算判斷 20181004 by sky
                int Y_SumTime = data_DUTYTABLE_PERSONAL.lstSingleDayInputs.Where(x => x.ToString() == "Y").Count(); //統計輪休時數
                bool is輪休 = (Y_SumTime > 0);//如果當天有輪休,就標記起來
                /*
                int dayinputSum = lstSingleDayInputs.Where(x => !string.IsNullOrEmpty(x)).Count(); //統計有排班的時數
                if (RestHour != 0)
                {
                    //如果 輪休時數 不等於 有排班的時數,則把RestHour(補休時數)扣掉
                    if (Y_SumTime != dayinputSum)
                    {
                        //這段依照維護單:1121201要求,先不扣除補休時數
                        //OverTime -= RestHour;
                    }
                }
                */


                for (int j = 0; j < rpt.Rows.Count; j++)
                {
                    //找到對應的日期
                    if (rpt.Rows[j]["MZ_ID"].ToString() == tempDT.Rows[i]["MZ_ID"].ToString() &&
                        rpt.Rows[j]["DATE"].ToString() == DATE)
                    {
                        rpt.Rows[j]["ITEMA"] = ITEMA;
                        rpt.Rows[j]["ITEMB"] = ITEMB;
                        rpt.Rows[j]["ITEMC"] = ITEMC;
                        rpt.Rows[j]["ITEMD"] = ITEMD;
                        //特殊: 此處會因為對象不同而有區別

                        if (Type == "0")
                        {
                            rpt.Rows[j]["ITEME"] = EAmount.ToString(); //勤區查察總時數
                        }
                        else if (Type == "1")
                        {
                            rpt.Rows[j]["ITEME"] = ITEME; //勤區查察總時數
                        }
                        rpt.Rows[j]["ITEMF"] = ITEMF;
                        rpt.Rows[j]["ITEMG"] = ITEMG;
                        rpt.Rows[j]["ITEMH"] = HAmount.ToString(); //其他勤務總時數
                        rpt.Rows[j]["ITEMI"] = ITEMI;
                        rpt.Rows[j]["ITEMII"] = ITEMII;
                        rpt.Rows[j]["TOTAL"] = DutyTime.ToString(); //上班時數
                        rpt.Rows[j]["OVERTIME"] = tempDT.Rows[i]["OVERTIME"].ToString(); //超勤時數,這邊透過SQL算出
                        rpt.Rows[j]["ISHOLIDAY"] = (isHoliday) ? "Y" : "N"; //是否為國定假日
                        rpt.Rows[j]["MZ_MEMO"] = tempDT.Rows[i]["MZ_MEMO"].ToString() + "  " + tempDT.Rows[i]["MEMO_NEW"].ToString();
                        rpt.Rows[j]["ITEMM_ADDHOUR"] = (is輪休) ? 1 : 0;//2024-03-05 此欄位定義被改成輪休時數
                        rpt.Rows[j]["REST_HOUR"] = RestHour.ToString();
                    }
                }//for (int j = 0; j < rpt.Rows.Count; j++)
            }//for 處理每一天的迴圈
        }

        /// <summary>  刑事、少年、婦幼 </summary>
        private void OtherPeople_Processing(DataTable rpt)
        {
            //若無機關，則跳出。
            if (string.IsNullOrEmpty(DropDownList_AD.SelectedValue))
            {
                return;
            }

            //讀取資料
            Load_DataTable(rpt, "1");

            if (rpt.Rows.Count > 0)
            {
                Session["rpt_dt"] = rpt;

                Session["TITLE"] = string.Format("{0}{1}{2}年{3}月超勤時數統計表", DropDownList_AD.SelectedItem.Text, DropDownList_UNIT.SelectedItem.Text, int.Parse(TextBox_DUTYDATE.Text.Substring(0, 3)).ToString(), int.Parse(TextBox_DUTYDATE.Text.Substring(3, 2)).ToString());
                //抓取內外勤參數
                Session["DutyUserClassify"] = Service.AccountService.getDutyUserClassify_V1(TextBox_MZ_ID.Text).ToString();
                string tmp_url = "C_rpt.aspx?fn=C_Overtimepay_detail_Criminal&TPM_FION=" + TPM_FION;

                ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "alert('無此日期之相關資料');", true);

            }
        }

        /// <summary>
        /// 抓取資料
        /// </summary>
        /// <param name="rpt">準備好1號~31號的資料,並帶入部分欄位,但是尚未寫入勤務相關的時數資訊</param>
        /// <param name="tempDT">從SQL抓取的每日超勤資料</param>
        private void Get_DataTable(DataTable rpt, out DataTable tempDT)
        {
            string IDSQL = "SELECT DISTINCT A.MZ_ID,CASE " +
                                " WHEN (SELECT MZ_HOUR_LIMIT FROM C_DUTYLIMIT WHERE MZ_AD=A.MZ_EXAD" +
                                                                             " AND MZ_UNIT=A.MZ_EXUNIT" +
                                                                             " AND MZ_ID=A.MZ_ID) IS NOT NULL " +
                                " THEN " +
                                      "(SELECT MZ_HOUR_LIMIT FROM C_DUTYLIMIT WHERE MZ_AD=A.MZ_EXAD" +
                                                                             " AND MZ_UNIT=A.MZ_EXUNIT" +
                                                                             " AND MZ_ID=A.MZ_ID) " +

                                " WHEN (SELECT MZ_HOUR_LIMIT FROM C_DUTYLIMIT WHERE MZ_AD=A.MZ_EXAD" +
                                                                             " AND MZ_UNIT=A.MZ_EXUNIT" +
                                                                             " AND MZ_ID ='NULL') IS NOT NULL" +
                                " THEN " +
                                      "(SELECT MZ_HOUR_LIMIT FROM C_DUTYLIMIT WHERE MZ_AD=A.MZ_EXAD" +
                                                                             " AND MZ_UNIT=A.MZ_EXUNIT" +
                                                                             " AND MZ_ID ='NULL')" +
                                " ELSE -1 END AS \"LIMIT\" FROM C_DUTYTABLE_PERSONAL A,A_DLBASE B WHERE A.MZ_ID=B.MZ_ID AND dbo.SUBSTR(A.DUTYDATE,1,5)='" + TextBox_DUTYDATE.Text + "'";

            string strSQL = @"SELECT A.DUTYDATE, A.MZ_ID, A.MZ_MEMO , A.DutyUserClassify,
                            CASE
                                WHEN COB.OVERTIME_TYPE ='OTT' THEN  '業務加班'
                                WHEN COB.OVERTIME_TYPE = 'OTB' THEN '業務加班'
                                WHEN COB.OVERTIME_TYPE = 'OTU' THEN '超勤補休'
                                WHEN COB.OVERTIME_TYPE = 'OTD' THEN '值日補休'
                                ELSE '' END + --Oracle的字串連接符號
                            CASE 
                                WHEN CD.MZ_ID IS NULL THEN '' 
                                ELSE  '值日 ' 
                            END  AS MEMO_NEW ,
                            --當日超勤補休時數
                           NVL( A.CONVERT_REST_HOURS,0) as CONVERT_REST_HOURS
                           --當日實支超勤時數(即當日的 超勤時數 扣除 超勤補休時數 後)
                           ,NVL( A.ISDIRECTTIME,0) as ISDIRECTTIME
                           --兩者加總,得到當天的超勤時數
                           ,NVL( A.CONVERT_REST_HOURS,0)+NVL( A.ISDIRECTTIME,0) as OVERTIME
                            FROM  C_DUTYTABLE_PERSONAL A
                            LEFT JOIN C_OVERTIME_BASE  COB ON COB.MZ_ID = A.MZ_ID AND COB.OVER_DAY = A.DUTYDATE
                            LEFT JOIN C_ONDUTY_DAY  CD ON CD.MZ_ID = A.MZ_ID AND CD.DATE_TAG = A.DUTYDATE 
                            WHERE dbo.SUBSTR(A.DUTYDATE,1,5)='" + TextBox_DUTYDATE.Text + "'";

            string C_DLTB01_strSQL = "select * from C_DLTB01 left join C_DLCODE on C_DLTB01.MZ_CODE=C_DLCODE.MZ_CODE where dbo.SUBSTR(MZ_IDATE1,0,5)='" + TextBox_DUTYDATE.Text + "'";//當月差價資料

            if (!string.IsNullOrEmpty(DropDownList_AD.SelectedValue))
            {
                strSQL += " AND A.MZ_EXAD='" + DropDownList_AD.SelectedValue + "'";
                IDSQL += " AND A.MZ_EXAD='" + DropDownList_AD.SelectedValue + "'";
                C_DLTB01_strSQL += " AND MZ_EXAD='" + DropDownList_AD.SelectedValue + "'";
            }
            //else
            //{
            //    return;//若無機關，則跳出。
            //}


            if (!string.IsNullOrEmpty(DropDownList_UNIT.SelectedValue))
            {
                strSQL += " AND A.MZ_EXUNIT='" + DropDownList_UNIT.SelectedValue + "'";
                IDSQL += " AND A.MZ_EXUNIT='" + DropDownList_UNIT.SelectedValue + "'";
                C_DLTB01_strSQL += " AND MZ_EXUNIT='" + DropDownList_UNIT.SelectedValue + "'";
            }

            if (!string.IsNullOrEmpty(TextBox_MZ_ID.Text))
            {
                strSQL += " AND A.MZ_ID='" + TextBox_MZ_ID.Text + "'";
                IDSQL += " AND A.MZ_ID='" + TextBox_MZ_ID.Text + "'";
                C_DLTB01_strSQL += " AND MZ_ID='" + TextBox_MZ_ID.Text + "'";
            }

            strSQL += " ORDER BY A.DUTYDATE";

            tempDT = new DataTable();
            tempDT = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GETVALUE");

            DataTable C_DLTB01_tempDT = new DataTable();

            C_DLTB01_tempDT = o_DBFactory.ABC_toTest.Create_Table(C_DLTB01_strSQL, "GETVALUE");

            for (int i = 0; i < tempDT.Rows.Count; i++)
            {
                if (String.IsNullOrEmpty(tempDT.Rows[i]["MEMO_NEW"].ToString()))
                {
                    for (int j = 0; j < C_DLTB01_tempDT.Rows.Count; j++)
                    {

                        if (tempDT.Rows[i]["MZ_ID"].ToString() == C_DLTB01_tempDT.Rows[j]["MZ_ID"].ToString())
                        {
                            //先把"MEMO_NEW"內容擷取出來,等一下有用
                            string MEMO_NEW = tempDT.Rows[i]["MEMO_NEW"].ToString();

                            //這邊似乎早期的設計是打算透過 C_OVERTIME_BASE.OVERTIME_TYPE 判定並顯示在備註上
                            //後來則是變成再追加透過 C_DLTB01 抓取
                            //拼接出來的結果, "差勤：請假事由"
                            if (tempDT.Rows[i]["DUTYDATE"].ToString() == C_DLTB01_tempDT.Rows[j]["MZ_IDATE1"].ToString())//督勤日等於請假日起
                            {
                                tempDT.Rows[i]["MEMO_NEW"] = "差勤：" + C_DLTB01_tempDT.Rows[j]["MZ_CNAME"].ToString();
                            }
                            else if (C_DLTB01_tempDT.Rows[j]["MZ_ITIME1"].ToString() != C_DLTB01_tempDT.Rows[j]["MZ_OTIME"].ToString())//判斷請假時間是否相同
                            {
                                if (StringtoDate(tempDT.Rows[i]["DUTYDATE"].ToString()) >= StringtoDate(C_DLTB01_tempDT.Rows[j]["MZ_IDATE1"].ToString())//督勤日等於請假迄日
                               && StringtoDate(tempDT.Rows[i]["DUTYDATE"].ToString()) <= StringtoDate(C_DLTB01_tempDT.Rows[j]["MZ_ODATE"].ToString())
                                )
                                {
                                    tempDT.Rows[i]["MEMO_NEW"] = "差勤：" + C_DLTB01_tempDT.Rows[j]["MZ_CNAME"].ToString();
                                }
                            }
                            else
                            {
                                if (StringtoDate(tempDT.Rows[i]["DUTYDATE"].ToString()) >= StringtoDate(C_DLTB01_tempDT.Rows[j]["MZ_IDATE1"].ToString())//督勤日不等於請假迄日
                              && StringtoDate(tempDT.Rows[i]["DUTYDATE"].ToString()) < StringtoDate(C_DLTB01_tempDT.Rows[j]["MZ_ODATE"].ToString())
                               )
                                {
                                    tempDT.Rows[i]["MEMO_NEW"] = "差勤：" + C_DLTB01_tempDT.Rows[j]["MZ_CNAME"].ToString();
                                }
                            }
                        }
                    }
                }

            }


            DataTable idDT = new DataTable();

            idDT = o_DBFactory.ABC_toTest.Create_Table(IDSQL, "GETVALUE2");

            // sam test 20200908 局長、副局長、主任祕書、督察長                             
            //str_OCCC = string.Empty;
            for (int j = 0; j < idDT.Rows.Count; j++)
            {
                //TODO 20141229 今天沒空.這之後要改掉,為什麼每次換代碼都要回資料庫撈.不會一開始就兜出來嗎
                string NAME = o_A_DLBASE.CNAME(idDT.Rows[j]["MZ_ID"].ToString());
                string OCCC = o_A_DLBASE.OCCC(idDT.Rows[j]["MZ_ID"].ToString());
                string SRANK = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=(SELECT MZ_SRANK FROM A_DLBASE WHERE MZ_ID='" + idDT.Rows[j]["MZ_ID"].ToString() + "') AND MZ_KTYPE='09' ");
                //str_OCCC = OCCC;

                //塞取空白日期
                for (int i = 1; i <= 31; i++)
                {
                    DataRow dr = rpt.NewRow();

                    dr["MZ_ID"] = idDT.Rows[j]["MZ_ID"].ToString();
                    dr["MZ_NAME"] = NAME;
                    dr["MZ_OCCC"] = OCCC;
                    dr["MZ_SRANK"] = SRANK;
                    dr["ITEMA"] = "";
                    dr["ITEMB"] = "";
                    dr["ITEMC"] = "";
                    dr["ITEMD"] = "";
                    dr["ITEME"] = "";
                    dr["ITEMF"] = "";
                    dr["ITEMG"] = "";
                    dr["ITEMH"] = "";
                    dr["ITEMI"] = "";
                    dr["ITEMII"] = "";
                    dr["TOTAL"] = "";
                    dr["LIMIT"] = idDT.Rows[j]["LIMIT"].ToString();
                    dr["OVERTIME"] = "";
                    dr["ISHOLIDAY"] = "";
                    dr["DATE"] = i.ToString().PadLeft(2, '0');
                    dr["MZ_MEMO"] = "";
                    dr["ITEMM_ADDHOUR"] = "";
                    //補休時數
                    dr["REST_HOUR"] = "";
                    rpt.Rows.Add(dr);
                }
            }
        }



        /// <summary>
        /// 字串轉DATE(民國年)
        /// </summary>
        /// <param name="Date"></param>
        /// <returns></returns>
        private DateTime StringtoDate(String Date)
        {
            DateTime dt;
            int ROC_index = 3;
            if (Date.Length > 7)
            {
                ROC_index += Date.Length - 7;
            }
            int WYear = int.Parse(Date.Substring(0, ROC_index)) + 1911;//

            string DUTYDATE = WYear.ToString() + "/" + Date.Substring(ROC_index, 2) + "/" + Date.Substring(ROC_index + 2, 2);

            dt = Convert.ToDateTime(DUTYDATE);

            return dt;

        }
        /// <summary>
        /// 字串轉TIME
        /// </summary>
        /// <param name="Time1"></param>
        /// <returns></returns>
        private DateTime Time_Compare(String Time1)
        {
            DateTime dt;
            //Time1 = Time1.Substring(0, 2) + ":" + Time1.Substring(1, 2);
            dt = Convert.ToDateTime(Time1);
            return dt;

        }


        /// <summary>取得某特定日期之是否停休</summary>
        /// <returns>是否停休</returns>
        /// 20150602 Joy
        private string GetDutyStopOff(String MZ_ID, String DATE)
        {
            List<String> result = new List<String>();
            DataRow r = Service_C_DUTYTABLE_PERSONAL.getOvertimeDatarow(MZ_ID, DATE);
            string s = "";
            if (r != null)
            {
                s = r["DUTYSTOPOFF"].ToString();
            }
            return s;
        }

        /// <summary>取得某特定日期之督勤時間區段</summary>
        /// <returns>督勤時間區段</returns>
        /// 20150506 Neil : 經詢問林小隊長，確認督勤於業務面上應不會有"區段斷開"的問題，故應為連續區段
        private String getSingleDayOverTimeGType(String MZ_ID, String DATE)
        {
            String TYPE = "G"; //定義督勤之代碼
            DataRow r = Service_C_DUTYTABLE_PERSONAL.getOvertimeDatarow(MZ_ID, DATE);

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

        protected void btCancel_Click(object sender, EventArgs e)
        {
            TextBox_DUTYDATE.Text = string.Empty;
            TextBox_MZ_ID.Text = string.Empty;
        }



        protected void DropDownList_UNIT_DataBound(object sender, EventArgs e)
        {
            ListItem li = new ListItem(" ", "");
            DropDownList_UNIT.Items.Insert(0, li);
        }

        protected void DropDownList_AD_SelectedIndexChanged(object sender, EventArgs e)
        {
            C.fill_unit(DropDownList_UNIT, DropDownList_AD.SelectedValue);
            //如果是中和分局進來改成只抓中和一&中和二&中和 matthew
            if (Session["ADPMZ_EXAD"].ToString() == "382133600C")
            {
                DropDownList_UNIT.Text = Session["ADPMZ_EXUNIT"].ToString();
            }
        }
    }
}
