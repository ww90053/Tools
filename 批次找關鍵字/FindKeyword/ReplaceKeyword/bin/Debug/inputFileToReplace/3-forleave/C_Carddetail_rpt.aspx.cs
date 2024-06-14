using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using TPPDDB.App_Code; 

namespace TPPDDB._3_forleave
{
    public partial class C_Carddetail_rpt : System.Web.UI.Page
    {
        int TPM_FION=0;
        
        DataTable temp_DT;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                C.check_power();
                //matthew 為了中和分局判斷功能權限用
                ViewState["C_strGID"] = o_a_Function.strGID(Session["ADPMZ_ID"].ToString());
                //by MQ ------------------------------20100331           
                C.set_Panel_EnterToTAB(ref this.Panel1);

                //C.fill_AD_POST(DropDownList_EXAD);
                //如果是中和分局進來改成只抓中和一&中和二&中和 matthew
                if (Session["ADPMZ_EXAD"].ToString() == "382133600C")
                {
                    C.fill_DLL_ONE_TWO(DropDownList_EXAD);
                }
                else
                {
                    //把所有機關撈出來包含台北縣
                    C.fill_AD_POST(DropDownList_EXAD);
                }     
               
                DropDownList_EXAD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                C.fill_unit(DropDownList_EXUNIT, DropDownList_EXAD.SelectedValue);
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
                case "A":
                case "B":
                    break;
                case "C":
                    //DropDownList_EXAD.Enabled = false;
                    //如果是中和分局進來改成只抓中和一&中和二&中和 matthew
                    if (Session["ADPMZ_EXAD"].ToString() != "382133600C")
                    {
                        DropDownList_EXAD.Enabled = false;
                    }
                    break;
                case "D":
                    //無權限
                    TPMPermissions._boolErrorData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "ADUXXX0001");
                    Response.Redirect("~/ErrorPage.aspx?erid=ADUXXX0001");
                    break;
                case "E":
                    //DropDownList_EXAD.Enabled = false;
                    //如果是中和分局進來改成只抓中和一&中和二&中和 matthew
                    if (Session["ADPMZ_EXAD"].ToString() != "382133600C")
                    {
                        DropDownList_EXAD.Enabled = false;
                    }
                    DropDownList_EXUNIT.SelectedValue = Session["ADPMZ_EXUNIT"].ToString();
                    DropDownList_EXUNIT.Enabled = false;
                    break;
            }
        }

        protected void btPrint_Click(object sender, EventArgs e)
        {
            if (DateManange.Check_date(TextBox_LOGDATE1.Text) && DateManange.Check_date(TextBox_LOGDATE2.Text))
            {
                temp_DT = new DataTable();
                temp_DT.Columns.Clear();
                temp_DT.Columns.Add("OCCC", typeof(string));
                temp_DT.Columns.Add("NAME", typeof(string));
                temp_DT.Columns.Add("LOGDATE", typeof(string));
                temp_DT.Columns.Add("INTIME", typeof(string));
                temp_DT.Columns.Add("OUTTIME", typeof(string));
                temp_DT.Columns.Add("CODE", typeof(string));
                temp_DT.Columns.Add("TDAY", typeof(string));
                temp_DT.Columns.Add("KIND", typeof(string));
                temp_DT.Columns.Add("MEMO", typeof(string));

                string SQLUNIT = "";
                string SQLDATE = "";

                string date1 = "";
                string date2 = "";

                string begindate = "";
                string enddate = "";

                if (TextBox_LOGDATE1.Text.Trim() != string.Empty && TextBox_LOGDATE2.Text.Trim() != string.Empty)
                {
                    date1 = o_str.tosql(TextBox_LOGDATE1.Text.Replace("/", string.Empty).PadLeft(7, '0'));
                    date2 = o_str.tosql(TextBox_LOGDATE2.Text.Replace("/", string.Empty).PadLeft(7, '0'));
                    begindate = (int.Parse(date1.Substring(0, 3)) + 1911).ToString() + "/" + date1.Substring(3, 2) + "/" + date1.Substring(5, 2);
                    enddate = (int.Parse(date2.Substring(0, 3)) + 1911).ToString() + "/" + date2.Substring(3, 2) + "/" + date2.Substring(5, 2);

                    SQLDATE = " AND LOGDATE>='" + begindate + "' AND LOGDATE<='" + enddate + "'";
                }
                else if (TextBox_LOGDATE1.Text.Trim() != string.Empty)
                {
                    date1 = o_str.tosql(TextBox_LOGDATE1.Text.Replace("/", string.Empty).PadLeft(7, '0'));
                    begindate = (int.Parse(date1.Substring(0, 3)) + 1911).ToString() + "/" + date1.Substring(3, 2) + "/" + date1.Substring(5, 2);
                    SQLDATE = " AND LOGDATE='" + begindate + "'";
                }

                if (DropDownList_EXUNIT.SelectedValue.Trim() != string.Empty)
                {
                    SQLUNIT = " AND MZ_EXUNIT='" + DropDownList_EXUNIT.SelectedValue + "'";
                }

                //增加主管級別(MZ_PCHIEF)排序 20190103 by sky
                string strSQL = string.Format(@"SELECT MZ_ID, MZ_OCCC AS OCCC, dbo.to_number(CASE WHEN MZ_TBDV IS NULL THEN '999' WHEN MZ_TBDV='Z99' THEN '999' ELSE MZ_TBDV END) AS TBDV 
                                                FROM A_DLBASE 
                                                WHERE MZ_EXAD='{0}' {1} AND MZ_ID IN (SELECT MZ_ID FROM C_CARDSET WHERE MZ_CLOCK='Y') 
                                                ORDER BY TBDV,MZ_PCHIEF,OCCC "
                                                , DropDownList_EXAD.SelectedValue
                                                , SQLUNIT);

                DataTable tempDT = new DataTable();

                tempDT = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GETID");


                if (TextBox_LOGDATE1.Text.Trim() != string.Empty && TextBox_LOGDATE2.Text.Trim() != string.Empty)
                {

                    DateTime TS1 = DateTime.Parse(begindate);
                    DateTime TS2 = DateTime.Parse(enddate);

                    TimeSpan TS = TS2 - TS1;

                    for (int i = 0; i < tempDT.Rows.Count; i++)
                    {
                        for (int j = 0; j <= TS.Days; j++)
                        {
                            temp_DT.Rows.Add(Count_Card_Record(tempDT.Rows[i]["MZ_ID"].ToString(), TS1.AddDays(j)));
                        }

                    }

                }
                else if (TextBox_LOGDATE1.Text.Trim() != string.Empty)
                {
                    for (int i = 0; i < tempDT.Rows.Count; i++)
                    {
                        DateTime TS1 = DateTime.Parse(begindate);

                        temp_DT.Rows.Add(Count_Card_Record(tempDT.Rows[i]["MZ_ID"].ToString(), TS1));
                    }
                }
                if (temp_DT.Rows.Count > 0)
                {
                    if (DateManange.Check_date(TextBox_LOGDATE1.Text) && DateManange.Check_date(TextBox_LOGDATE2.Text))
                    {
                        Session["rpt_dt"] = temp_DT;

                        Session["TITLE"] = string.Format("{0}{1}勤惰(刷卡)紀錄明細表", DropDownList_EXAD.SelectedItem.Text, DropDownList_EXUNIT.SelectedItem.Text);

                        string tmp_url = "C_rpt.aspx?fn=carddetail&TPM_FION=" + TPM_FION;

                        ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "alert('請檢查輸入起迄日期');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "alert('無此日期之相關資料');", true);

                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "alert('請檢查輸入起迄日期');", true);
            }
        }

        protected DataRow Count_Card_Record(string MZ_ID, DateTime DATE)
        {

            string LOGDATE = DATE.Year.ToString() + "/" + DATE.Month.ToString().PadLeft(2, '0') + "/" + DATE.Day.ToString().PadLeft(2, '0');
            string LOGDATE1 = (DATE.Year - 1911).ToString().PadLeft(3, '0') + DATE.Month.ToString().PadLeft(2, '0') + DATE.Day.ToString().PadLeft(2, '0');
            int INTIME = 90100, OUTTIME = 170000;

            List<String> temp = new List<string>();

            temp = C_CountCardRecord.list_Abnormal(MZ_ID, LOGDATE);

            if (DateTime.Parse(LOGDATE) >= Convert.ToDateTime("2021/05/17") && DateTime.Parse(LOGDATE) <= Convert.ToDateTime("2021/07/26"))
            {
                INTIME = 100100;
                OUTTIME = 160000;
            }

            if (DateTime.Parse(LOGDATE) >= Convert.ToDateTime("2021/07/27") && DateTime.Parse(LOGDATE) <= Convert.ToDateTime("2022/12/31"))
            {
                INTIME = 093100;
                OUTTIME = 163000;
            }

            string selectINTIME = o_DBFactory.ABC_toTest.vExecSQL("SELECT LOGTIME FROM (SELECT LOGTIME,ROW_NUMBER() OVER(ORDER BY LOGTIME) AS ROWCOUNT FROM C_CARDHISTORY_NEW WHERE dbo.SUBSTR(USERNAME,0,10)='" + o_str.tosql(MZ_ID.Trim()) + "' AND LOGDATE='" + LOGDATE + "' AND VERIFY='IN') WHERE ROWCOUNT=1");
            string selectOUTTIME = o_DBFactory.ABC_toTest.vExecSQL("SELECT LOGTIME FROM (SELECT LOGTIME,ROW_NUMBER() OVER(ORDER BY LOGTIME DESC) AS ROWCOUNT FROM C_CARDHISTORY_NEW WHERE dbo.SUBSTR(USERNAME,0,10)='" + o_str.tosql(MZ_ID.Trim()) + "' AND LOGDATE='" + LOGDATE + "' AND VERIFY='IN') WHERE ROWCOUNT=1");
            //string selectINOVERTIME = o_DBFactory.ABC_toTest.vExecSQL("SELECT LOGTIME FROM (SELECT LOGTIME,ROW_NUMBER() OVER(ORDER BY LOGTIME) AS ROWCOUNT FROM C_CARDHISTORY_NEW WHERE USERID='" + o_str.tosql(MZ_ID.Trim()) + "' AND LOGDATE='" + LOGDATE + "' AND FKEY='F3' ) WHERE ROWCOUNT=1");
            //string selectOUTOVERTIME = o_DBFactory.ABC_toTest.vExecSQL("SELECT LOGTIME FROM (SELECT LOGTIME,ROW_NUMBER() OVER(ORDER BY LOGTIME DESC) AS ROWCOUNT FROM C_CARDHISTORY_NEW WHERE USERID='" + o_str.tosql(MZ_ID.Trim()) + "' AND LOGDATE='" + LOGDATE + "' AND FKEY='F3' ) WHERE ROWCOUNT=1");

            string KIND = "";
            string MEMO = "";
            string CODE = "";

            if (string.IsNullOrEmpty(selectINTIME) && string.IsNullOrEmpty(selectOUTTIME))
            {
                if (DateTime.Parse(LOGDATE).DayOfWeek == DayOfWeek.Saturday || DateTime.Parse(LOGDATE).DayOfWeek == DayOfWeek.Sunday)
                {
                    

                    MEMO = "例假日";
                }
                else if (temp.Count == 0) ///無刷卡狀態
                {
                    if (DATE.Year.ToString() + DATE.Month.ToString() + DATE.Day.ToString() == DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString())
                    {
                        int NOWTIME = int.Parse(DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0'));

                        if (NOWTIME <= INTIME)///本日九點前
                        {
                            KIND = "";
                            MEMO = "";
                        }
                        else if (NOWTIME > INTIME)///本日九點後
                        {
                            KIND = "上班未刷卡";
                            MEMO = "上班異常";
                        }
                    }
                    else///過本日後
                    {
                        KIND = "未刷卡";
                        MEMO = "上班異常";
                    }
                }
                else
                {
                    if (temp.Count > 1)///有請假
                    {
                        CODE = temp[0];

                        if (int.Parse(LOGDATE1) >= int.Parse(temp[1]) && int.Parse(LOGDATE1) <= int.Parse(temp[3]) && int.Parse(temp[6]) > 0)///正常
                        {
                            KIND = "";
                            MEMO = "請假";
                        }
                        else
                        {
                            KIND = "未刷卡";
                            MEMO = "上班異常";
                        }
                    }
                    else if (temp.Count == 1)
                    {
                        KIND = "";
                        MEMO = "國定假日";
                    }
                }
            }
            else
            {
                if (DateTime.Parse(LOGDATE).DayOfWeek == DayOfWeek.Saturday || DateTime.Parse(LOGDATE).DayOfWeek == DayOfWeek.Sunday)
                {
                    

                    MEMO = "例假日";
                }
                else if (DATE.Year.ToString() + DATE.Month.ToString() + DATE.Day.ToString() == DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString())///當日
                {
                    if (int.Parse(selectINTIME.Replace(":", string.Empty)) <= INTIME)///當日刷9點前的卡
                    {
                        if (selectINTIME == selectOUTTIME && temp.Count == 0)///只有一筆刷卡資料而且無請假
                        {
                            KIND = "";
                            MEMO = "";
                            selectOUTTIME = "";
                        }
                        else if (selectINTIME == selectOUTTIME && temp.Count > 1)///只有一筆刷卡資料而且有請假
                        {
                            selectOUTTIME = "";

                            CODE = temp[0];

                            DateTime TS1 = DateTime.Parse(LOGDATE + " " + temp[2] + ":00");///跟請假起時比

                            if (DateTime.Now > TS1)///超過就算下班未刷卡
                            {
                                selectOUTTIME = "";
                                KIND = "下班未刷卡";
                                MEMO = "上班異常";
                            }

                        }
                        else if (int.Parse(selectOUTTIME.Replace(":", string.Empty)) < INTIME)
                        {
                            KIND = "";
                            MEMO = "";
                            selectOUTTIME = "";
                        }
                        else if (int.Parse(selectOUTTIME.Replace(":", string.Empty)) < OUTTIME)///下班時間少於17點
                        {
                            if (temp.Count > 1)///有請假
                            {
                                CODE = temp[0];

                                DateTime TS1 = DateTime.Parse(LOGDATE + " " + selectINTIME);

                                DateTime TS2 = DateTime.Parse(LOGDATE + " " + selectOUTTIME);

                                TimeSpan TS = TS2 - TS1;

                                if (TS.Hours + int.Parse(temp[6]) >= 8)
                                {
                                    MEMO = "請假";
                                }
                                else
                                {
                                    KIND = "早退";
                                    MEMO = "上班異常";
                                }
                            }
                            else
                            {
                                KIND = "早退";
                                MEMO = "上班異常";
                            }
                        }
                        else ///下班時間大於17點
                        {
                            if (temp.Count > 1)
                            {
                                CODE = temp[0];
                            }

                            DateTime TS1 = DateTime.Parse(LOGDATE + " " + selectINTIME);

                            DateTime TS2 = DateTime.Parse(LOGDATE + " " + selectOUTTIME);

                            TimeSpan TS = TS2 - TS1;

                            if (TS.Hours > 8)
                            {
                                MEMO = "";
                            }
                            else
                            {
                                KIND = "早退";
                                MEMO = "上班異常";
                            }
                        }
                    }
                    else if (int.Parse(selectINTIME.Replace(":", string.Empty)) > INTIME)///當天刷卡時間大於9點1分
                    {
                        if (selectINTIME == selectOUTTIME && temp.Count == 0)///只有一筆刷卡紀錄
                        {
                            if (int.Parse(selectINTIME.Replace(":", string.Empty)) > OUTTIME)///記錄大於17點
                            {
                                KIND = "上班未刷卡";
                                MEMO = "上班異常";
                                selectINTIME = "";
                            }
                            else
                            {
                                KIND = "遲到";
                                MEMO = "上班異常";
                                selectOUTTIME = "";
                            }
                        }
                        else if (selectINTIME == selectOUTTIME && temp.Count > 1)///只有一筆刷卡紀錄有請假
                        {
                            CODE = temp[0];

                            if (int.Parse(temp[2].Replace(":", "")) <= 900 && int.Parse(temp[4].Replace(":", "")) <= 1330)
                            {
                                if (int.Parse(selectINTIME.Replace(":", string.Empty)) < 133000)///未超過1330刷卡
                                {
                                    KIND = "";
                                    MEMO = "";
                                    selectOUTTIME = "";
                                }
                                else///超過1330
                                {
                                    KIND = "遲到";
                                    MEMO = "上班異常";
                                    selectOUTTIME = "";
                                }
                            }
                            else if (int.Parse(temp[2].Replace(":", "")) <= 900)
                            {
                                if (int.Parse(selectINTIME.Replace(":", string.Empty)) < int.Parse(temp[4].Replace(":", string.Empty) + "00"))
                                {
                                    if (int.Parse(selectINTIME.Replace(":", string.Empty)) > 173000)
                                    {
                                        KIND = "上班未刷卡";
                                        MEMO = "上班異常";
                                        selectINTIME = "";
                                    }
                                    else
                                    {
                                        KIND = "下班未刷卡";
                                        MEMO = "上班異常";
                                        selectOUTTIME = "";
                                    }
                                }
                                else
                                {
                                    KIND = "遲到";
                                    MEMO = "上班異常";
                                    selectOUTTIME = "";
                                }
                            }
                            else
                            {
                                KIND = "遲到";
                                MEMO = "上班異常";
                                selectOUTTIME = "";
                            }
                        }
                        else if (int.Parse(selectOUTTIME.Replace(":", string.Empty)) < 180000)
                        {
                            if (temp.Count > 1)
                            {
                                CODE = temp[0];

                                DateTime TS1 = DateTime.Parse(LOGDATE + " " + temp[2] + ":00");

                                DateTime TS2 = DateTime.Parse(LOGDATE + " " + selectOUTTIME);

                                TimeSpan TS = TS1 - TS2;

                                if (TS.Hours + int.Parse(temp[6]) < 8)
                                {
                                    KIND = "遲到";
                                    MEMO = "上班異常";
                                }
                                else
                                {
                                    KIND = "遲到早退";
                                    MEMO = "上班異常";
                                }
                            }
                            else
                            {
                                KIND = "遲到早退";
                                MEMO = "上班異常";
                            }
                        }
                        else
                        {
                            if (temp.Count > 1)
                            {
                                CODE = temp[0];

                                MEMO = "";
                            }
                            else
                            {
                                KIND = "遲到";
                                MEMO = "上班異常";
                            }
                        }

                    }
                }
                else
                {
                    if (int.Parse(selectINTIME.Replace(":", string.Empty)) <= INTIME)///非當日刷卡時間未超過9點1分
                    {
                        if (selectINTIME == selectOUTTIME && temp.Count == 0)///只有一筆刷卡資料
                        {
                            KIND = "下班未刷卡";
                            MEMO = "上班異常";
                            selectOUTTIME = "";
                        }
                        else if (int.Parse(selectOUTTIME.Replace(":", string.Empty)) < INTIME && temp.Count == 0)///多筆資料都在九點前，且當日無請假紀錄
                        {
                            KIND = "下班未刷卡";
                            MEMO = "上班異常";
                            selectOUTTIME = "";
                        }
                        else if (int.Parse(selectOUTTIME.Replace(":", string.Empty)) < OUTTIME)///下班刷卡時間早於5點
                        {
                            if (temp.Count > 1) ///有請假
                            {
                                CODE = temp[0];

                                DateTime TS1 = DateTime.Parse(LOGDATE + " " + selectINTIME);

                                DateTime TS2 = DateTime.Parse(LOGDATE + " " + selectOUTTIME);

                                TimeSpan TS = TS2 - TS1;

                                if (TS.Hours + int.Parse(temp[6]) >= 8)
                                {
                                    MEMO = "下午請假";
                                }
                                else
                                {
                                    KIND = "早退";
                                    MEMO = "上班異常";
                                }
                            }
                            else
                            {
                                KIND = "早退";
                                MEMO = "上班異常";
                            }
                        }
                        else
                        {
                            DateTime TS1 = DateTime.Parse(LOGDATE + " " + selectINTIME);

                            DateTime TS2 = DateTime.Parse(LOGDATE + " " + selectOUTTIME);

                            TimeSpan TS = TS2 - TS1;

                            if (TS.Hours > 8)
                            {
                                MEMO = "";
                            }
                            else if (temp.Count > 1)
                            {
                                CODE = temp[0];
                                if (TS.Hours + int.Parse(temp[6]) >= 8)
                                {
                                    MEMO = "下午請假";
                                }
                            }
                            else
                            {
                                KIND = "早退";
                                MEMO = "上班異常";
                            }
                        }
                    }
                    else if (int.Parse(selectINTIME.Replace(":", string.Empty)) > INTIME)///上班刷卡時間大於9點1分
                    {
                        if (selectINTIME == selectOUTTIME && temp.Count == 0)///只一筆刷卡資料
                        {
                            if (int.Parse(selectINTIME.Replace(":", string.Empty)) > OUTTIME)
                            {
                                KIND = "上班未刷卡";
                                MEMO = "上班異常";
                                selectINTIME = "";
                            }
                            else
                            {
                                KIND = "遲到下班未刷卡";
                                MEMO = "上班異常";
                                selectINTIME = "";
                            }
                        }
                        else if (selectINTIME == selectOUTTIME && temp.Count > 1)///只一筆有請假
                        {
                            CODE = temp[0];

                            if (temp[2] == "08:30" && temp[4] == "12:30")
                            {

                                if (int.Parse(selectINTIME.Replace(":", string.Empty)) < 133000)
                                {
                                    KIND = "下班未刷卡";
                                    MEMO = "上班異常";
                                    selectOUTTIME = "";
                                }
                                else if (int.Parse(selectINTIME.Replace(":", string.Empty)) >= 173000)
                                {
                                    KIND = "上班未刷卡";
                                    MEMO = "上班異常";
                                    selectINTIME = "";
                                }
                                else
                                {
                                    KIND = "遲到下班未刷卡";
                                    MEMO = "上班異常";
                                    selectOUTTIME = "";
                                }

                            }
                            else if (int.Parse(temp[2].Replace(":", "")) <= 900)
                            {
                                if (int.Parse(selectINTIME.Replace(":", string.Empty)) < int.Parse(temp[4].Replace(":", string.Empty) + "00"))
                                {
                                    KIND = "下班未刷卡";
                                    MEMO = "上班異常";
                                    selectOUTTIME = "";
                                }
                                else
                                {
                                    KIND = "遲到下班未刷卡";
                                    MEMO = "上班異常";
                                    selectOUTTIME = "";
                                }
                            }
                            else
                            {
                                KIND = "遲到下班未刷卡";
                                MEMO = "上班異常";
                                selectOUTTIME = "";
                            }
                        }
                        else if (int.Parse(selectOUTTIME.Replace(":", string.Empty)) < 180000)
                        {
                            if (temp.Count > 1)
                            {
                                CODE = temp[0];

                                DateTime TS1 = DateTime.Parse(LOGDATE + " " + selectINTIME);

                                DateTime TS2 = DateTime.Parse(LOGDATE + " " + selectOUTTIME);

                                TimeSpan TS = TS2 - TS1;

                                if (TS.Hours + int.Parse(temp[6]) >= 8)
                                {
                                    KIND = "";
                                    MEMO = "早上請假";
                                }
                                else
                                {
                                    KIND = "遲到早退";
                                    //MEMO = "上班異常";   // Joy 當天有請假紀錄，則不出現上班異常備註。
                                }
                            }
                            else
                            {
                                KIND = "遲到早退";
                                MEMO = "上班異常";
                            }
                        }
                        else
                        {
                            if (temp.Count > 1)
                            {
                                CODE = temp[0];

                                MEMO = "正常";
                            }
                            else
                            {
                                KIND = "遲到";
                                MEMO = "上班異常";
                            }
                        }
                    }
                    else if (string.IsNullOrEmpty(selectINTIME))
                    {
                        if (int.Parse(selectOUTTIME.Replace(":", string.Empty)) < OUTTIME)
                        {
                            KIND = "遲到早退上班未刷卡";

                            MEMO = "上班異常";
                        }
                        else
                        {
                            KIND = "遲到上班未刷卡";

                            MEMO = "上班異常";
                        }
                    }
                }
            }

           

            DataRow dr = temp_DT.NewRow();
            dr["OCCC"] = o_A_DLBASE.OCCC(MZ_ID);
            dr["NAME"] = o_A_DLBASE.CNAME(MZ_ID);
            dr["LOGDATE"] = int.Parse(DateManange.datetostr(LOGDATE).Substring(0, 3)).ToString() + "/" + DateManange.datetostr(LOGDATE).Substring(3, 2) + "/" + DateManange.datetostr(LOGDATE).Substring(5, 2);
            dr["INTIME"] = selectINTIME;
            dr["OUTTIME"] = selectOUTTIME;
            dr["CODE"] = CODE;
            if (string.IsNullOrEmpty(CODE))
            {
                dr["TDAY"] = string.Empty;
            }
            else
            {
                if (temp.Count > 1)
                {
                    if (temp[6] == "0")
                    {
                        dr["TDAY"] = "1日0時";
                    }
                    else if (temp[6] == "8")
                    {
                        dr["TDAY"] = "1日0時";
                    }
                    else
                    {
                        dr["TDAY"] = "0日" + temp[6] + "時";
                    }
                }
                else
                {
                    dr["TDAY"] = "1日0時";
                }
            }
            dr["KIND"] = KIND;
            dr["MEMO"] = MEMO;

            return dr;
        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            TextBox_LOGDATE1.Text = string.Empty;
            TextBox_LOGDATE2.Text = string.Empty;
        }


        protected void returnSameDataType(TextBox tb, object ob1)
        {
            tb.Text = o_str.tosql(tb.Text.Trim().Replace("/", ""));

            if (tb.Text != "")
            {
                if (!DateManange.Check_date(tb.Text))
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('日期格式錯誤')", true);
                    tb.Focus();
                }
                else
                {
                    tb.Text = o_CommonService.Personal_ReturnDateString(tb.Text.Trim());

                    if (ob1 is DropDownList)
                    {
                        (ob1 as DropDownList).Focus();
                    }
                    else if (ob1 is TextBox)
                    {
                        (ob1 as TextBox).Focus();
                    }
                }
            }
        }

        protected void TextBox_LOGDATE1_TextChanged(object sender, EventArgs e)
        {
            returnSameDataType(TextBox_LOGDATE1, TextBox_LOGDATE2);
        }

        protected void TextBox_LOGDATE2_TextChanged(object sender, EventArgs e)
        {
            returnSameDataType(TextBox_LOGDATE2, TextBox_LOGDATE2);
        }

        protected void DropDownList_EXUNIT_DataBound(object sender, EventArgs e)
        {
            ListItem li = new ListItem(" ", "");
            DropDownList_EXUNIT.Items.Insert(0, li);
        }

        protected void DropDownList_EXAD_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ViewState["C_strGID"].ToString() == "E")//權限E選擇所屬單位並鎖單位
            {
                DropDownList_EXUNIT.SelectedValue = Session["ADPMZ_EXUNIT"].ToString();
                DropDownList_EXUNIT.Enabled = false;

            }
            else
            {
                C.fill_unit(DropDownList_EXUNIT, DropDownList_EXAD.SelectedValue);
            }
        }
    }
}
