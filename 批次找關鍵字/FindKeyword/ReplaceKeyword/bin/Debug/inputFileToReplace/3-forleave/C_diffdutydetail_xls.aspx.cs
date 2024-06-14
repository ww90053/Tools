using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TPPDDB._3_forleave
{
    public partial class C_diffdutydetail_xls : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        /// <summary>
        /// 判斷讀卡機的紀錄,回傳格式為:"上班刷卡時間,下班刷卡時間,狀況,備註"
        /// </summary>
        /// <param name="MZ_ID">證號</param>
        /// <param name="DATE">日期</param>
        /// <param name="type">無用欄位</param>
        /// <param name="punit">無用欄位</param>
        /// <param name="name">無用欄位</param>
        /// <param name="occc">無用欄位</param>
        /// <returns>回傳格式:"上班刷卡時間,下班刷卡時間,狀況,備註"
        /// </returns>
        public static string Count_Card_Record(string MZ_ID, DateTime DATE, string type, string punit, string name, string occc)
        {
            int INTIME = 90100, OUTTIME = 170000;

            if (DATE >= Convert.ToDateTime("2021/05/17") && DATE <= Convert.ToDateTime("2021/07/26"))
            {
                INTIME = 100100;
                OUTTIME = 160000;
            }

            if (DATE >= Convert.ToDateTime("2021/07/27") && DATE <= Convert.ToDateTime("2022/12/31"))
            {
                INTIME = 093100;
                OUTTIME = 163000;
            }

            string LOGDATE = DATE.Year.ToString() + "/" + DATE.Month.ToString().PadLeft(2, '0') + "/" + DATE.Day.ToString().PadLeft(2, '0');
            string LOGDATE1 = (DATE.Year - 1911).ToString().PadLeft(3, '0') + DATE.Month.ToString().PadLeft(2, '0') + DATE.Day.ToString().PadLeft(2, '0');

            string KIND = "";
            string MEMO = "";
            string CODE = "";

            //請假/國定休假日紀錄清單
            // 陣列意義 > 一般請假：[0]假別, [1]請假起日, [2]起時, [3]訖日, [4]迄時, [5]日數, [6]時數；國定假日：只回傳假日名稱
            List<String> lstDayOff = new List<String>();
            //這是在找有休假的的資料 
            lstDayOff = C_CountCardRecord.list_Abnormal(MZ_ID, LOGDATE);

            string selectINTIME = "";
            string selectOUTTIME = "";
            //matthew 因公未刷卡
            string C_CARD_EDIT = "";

            #region 全部

            //全部
            {
                //matthew 新增 因公未刷卡 如果TID = 99 就是因公 還要看是上班還是下班目前沒判斷 警局也沒要求 所以先不改
                C_CARD_EDIT = o_DBFactory.ABC_toTest.vExecSQL("SELECT TID FROM C_CARDHISTORY_NEW WHERE dbo.SUBSTR(USERNAME,0,10)='" + MZ_ID + "'" + "AND LOGDATE='" + LOGDATE + "'" + "AND TID ='99'");
                //C_CARD_EDIT = o_DBFactory.ABC_toTest.vExecSQL("SELECT TID FROM (SELECT TID,LOGTIME,ROW_NUMBER() OVER(ORDER BY LOGTIME) AS ROWCOUNT FROM C_CARDHISTORY_NEW WHERE dbo.SUBSTR(USERNAME,0,10)='" + MZ_ID + "' AND LOGDATE='" + LOGDATE + "' AND VERIFY='IN') WHERE ROWCOUNT=1");
                //雖然說為什麼要進出資料庫找兩次.但找起來還算快.就先不改
                selectINTIME = o_DBFactory.ABC_toTest.vExecSQL("SELECT LOGTIME FROM (SELECT LOGTIME,ROW_NUMBER() OVER(ORDER BY LOGTIME) AS ROWCOUNT FROM C_CARDHISTORY_NEW WHERE dbo.SUBSTR(USERNAME,0,10)='" + MZ_ID + "' AND LOGDATE='" + LOGDATE + "' AND VERIFY='IN') WHERE ROWCOUNT=1");
                if (selectINTIME.Length > 0 && selectINTIME.Substring(0, 3) == "24:")
                {
                    selectINTIME = "00:" + selectINTIME.Substring(3, 5);
                }

                selectOUTTIME = o_DBFactory.ABC_toTest.vExecSQL("SELECT LOGTIME FROM (SELECT LOGTIME,ROW_NUMBER() OVER(ORDER BY LOGTIME DESC) AS ROWCOUNT FROM C_CARDHISTORY_NEW WHERE dbo.SUBSTR(USERNAME,0,10)='" + MZ_ID.Trim() + "' AND LOGDATE='" + LOGDATE + "' AND VERIFY='IN') WHERE ROWCOUNT=1");
                if (selectOUTTIME.Length > 0 && selectOUTTIME.Substring(0, 3) == "24:")
                {
                    selectOUTTIME = "00:" + selectOUTTIME.Substring(3, 5);
                }

                //雖然說為什麼要進出資料庫找兩次找.但找起來還算快.就先不改

                //取得免刷卡人員做判斷 20190718 by sky
                string noSwipe = o_DBFactory.ABC_toTest.vExecSQL(string.Format("SELECT MZ_CLOCK FROM C_CARDSET WHERE MZ_ID = '{0}' ", MZ_ID.Trim()));

                string qdate = (DATE.Year - 1911).ToString().PadLeft(3, '0') + DATE.Month.ToString().PadLeft(2, '0') + DATE.Day.ToString().PadLeft(2, '0');
                //抓取補班日的資料?
                string C_SPRINGDAY = o_DBFactory.ABC_toTest.vExecSQL(string.Format("SELECT mz_spring_name FROM C_SPRINGDAY WHERE mz_spring_date = '{0}' ", qdate));

                //如果當天 刷到&刷退都沒有紀錄
                if (string.IsNullOrEmpty(selectINTIME) && string.IsNullOrEmpty(selectOUTTIME))//如果當天都沒刷卡紀錄
                {
                    //如果 當天是六日 且 不是補班
                    if ((DateTime.Parse(LOGDATE).DayOfWeek == DayOfWeek.Saturday || DateTime.Parse(LOGDATE).DayOfWeek == DayOfWeek.Sunday) && C_SPRINGDAY == "")
                    {
                        MEMO = "例假日";
                    }
                    //免刷卡人員不進行判斷
                    else if (noSwipe == "N")
                    {
                        //免刷卡人員不進行判斷
                    }
                    //如果當天也沒有請假紀錄
                    else if (lstDayOff.Count == 0) ///無刷卡狀態
                    {
                        //如果紀錄時間還在當天
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
                        if (lstDayOff.Count > 1)///有請假
                        {
                            CODE = lstDayOff[0];

                            if (int.Parse(LOGDATE1) >= int.Parse(lstDayOff[1]) && int.Parse(LOGDATE1) <= int.Parse(lstDayOff[3]) && int.Parse(lstDayOff[6]) > 0)///正常
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
                        else if (lstDayOff.Count == 1)
                        {
                            KIND = "";
                            MEMO = "國定假日";
                        }
                    }
                    if (C_CARD_EDIT == "99")
                    {
                        MEMO = "因公疏未"
                            + "<br>"
                            + "辦理刷卡";
                    }
                }
                //如果當天有刷卡紀錄
                else
                {
                    //當天是六日,且 非補班日
                    if ((DateTime.Parse(LOGDATE).DayOfWeek == DayOfWeek.Saturday || DateTime.Parse(LOGDATE).DayOfWeek == DayOfWeek.Sunday) && C_SPRINGDAY == "")
                    {
                        MEMO = "例假日";
                    }
                    //免刷卡人員不進行判斷
                    else if (noSwipe == "N")
                    {
                        //免刷卡人員不進行判斷
                    }
                    //如果是執行此動作當天當日
                    else if (DATE.Year.ToString() + DATE.Month.ToString() + DATE.Day.ToString() == DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString())
                    {
                        if (int.Parse(selectINTIME.Replace(":", string.Empty)) <= INTIME)///當日刷9點前的卡
                        {
                            if (selectINTIME == selectOUTTIME && lstDayOff.Count == 0)///只有一筆刷卡資料而且無請假
                            {
                                KIND = "下班未刷卡";
                                MEMO = "上班異常";
                                selectOUTTIME = "";
                            }
                            else if (selectINTIME == selectOUTTIME && lstDayOff.Count > 1)///只有一筆刷卡資料而且有請假
                            {
                                KIND = "下班未刷卡"; //事實上仍未刷卡故顯示
                                MEMO = "請假"; //但備註欄補上請假資訊
                                selectOUTTIME = "";

                                CODE = lstDayOff[0];

                                DateTime TS1 = DateTime.Parse(LOGDATE + " " + lstDayOff[2] + ":00");///跟請假起時比

                                if (DateTime.Now > TS1)///超過就算下班未刷卡
                                {
                                    selectOUTTIME = "";
                                    KIND = "下班未刷卡";
                                    MEMO = "上班異常";
                                }

                            }
                            else if (int.Parse(selectOUTTIME.Replace(":", string.Empty)) < INTIME)
                            {
                                KIND = "早退";
                                MEMO = "上班異常";
                                selectOUTTIME = "";
                            }
                            else if (int.Parse(selectOUTTIME.Replace(":", string.Empty)) < OUTTIME)///下班時間少於17點
                            {
                                if (lstDayOff.Count > 1)///有請假
                                {
                                    CODE = lstDayOff[0];

                                    DateTime TS1 = DateTime.Parse(LOGDATE + " " + selectINTIME);

                                    DateTime TS2 = DateTime.Parse(LOGDATE + " " + selectOUTTIME);

                                    TimeSpan TS = TS2 - TS1;

                                    if (TS.Hours + int.Parse(lstDayOff[6]) >= 8)
                                    {
                                        MEMO = "請假";
                                    }
                                    else
                                    {
                                        KIND = "早退";
                                        MEMO = "請假早退";
                                    }
                                }
                                else if (lstDayOff.Count == 1)
                                {
                                    KIND = "";
                                    MEMO = "國定假日";
                                }
                                else
                                {
                                    KIND = "早退";
                                    MEMO = "上班異常";
                                }
                            }
                            else ///下班時間大於17點
                            {
                                if (lstDayOff.Count > 1)
                                {
                                    CODE = lstDayOff[0];
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
                            if (selectINTIME == selectOUTTIME && lstDayOff.Count == 0)///只有一筆刷卡紀錄
                            {
                                if (int.Parse(selectINTIME.Replace(":", string.Empty)) > OUTTIME)///記錄大於17點
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
                            else if (selectINTIME == selectOUTTIME && lstDayOff.Count > 1)///只有一筆刷卡紀錄有請假
                            {
                                //假別
                                CODE = lstDayOff[0];
                                //請假 起時早於九點,迄時早於13:30
                                if (int.Parse(/*[2]起時*/lstDayOff[2].Replace(":", "")) <= 900 && int.Parse(/*[4]迄時*/lstDayOff[4].Replace(":", "")) <= 1330)
                                {
                                    //如果 上班刷卡時間,早於1330
                                    if (int.Parse(selectINTIME.Replace(":", string.Empty)) < 133000)///未超過1330刷卡
                                    {
                                        KIND = "下班未刷卡";
                                        MEMO = "上班異常";
                                        selectOUTTIME = "";
                                    }
                                    else///超過1330
                                    {
                                        KIND = "遲到";
                                        MEMO = "上班異常";
                                        selectOUTTIME = "";
                                    }
                                }
                                //如果 請假 起時早於九點
                                else if (int.Parse(/*[2]起時*/lstDayOff[2].Replace(":", "")) <= 900)
                                {
                                    if (int.Parse(selectINTIME.Replace(":", string.Empty)) < int.Parse(lstDayOff[4].Replace(":", string.Empty) + "00"))
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
                                if (lstDayOff.Count > 1)
                                {
                                    CODE = lstDayOff[0];

                                    DateTime TS1 = DateTime.Parse(LOGDATE + " " + lstDayOff[2] + ":00");

                                    DateTime TS2 = DateTime.Parse(LOGDATE + " " + selectOUTTIME);

                                    TimeSpan TS = TS1 - TS2;

                                    if (TS.Hours + int.Parse(lstDayOff[6]) < 8)
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
                                else if (lstDayOff.Count == 1)
                                {
                                    KIND = "";
                                    MEMO = "國定假日";
                                }
                                else
                                {
                                    KIND = "遲到早退";
                                    MEMO = "上班異常";
                                }
                            }
                            else
                            {
                                if (lstDayOff.Count > 1)
                                {
                                    CODE = lstDayOff[0];

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
                    //上班日,非免刷卡人員,也非當天當日
                    else
                    {
                        //取得時間差
                        DateTime dt1 = DateTime.Parse(LOGDATE + " " + selectINTIME); //刷卡時間(上班)
                        DateTime dt2 = DateTime.Parse(LOGDATE + " " + selectOUTTIME); //刷卡時間(下班)，若當日無下班刷卡紀錄則會跟上班時間相同
                        TimeSpan dt_diff = dt2 - dt1; //時間差

                        //非當日刷卡時間未超過9點1分
                        if (int.Parse(selectINTIME.Replace(":", string.Empty)) <= INTIME)
                        {
                            if (selectINTIME == selectOUTTIME && lstDayOff.Count == 0)///只有一筆刷卡資料且未請假
                            {
                                KIND = "下班未刷卡";
                                //MEMO = ""; //20151209 Neil: 小隊長建議移除備註欄即可
                                MEMO = "上班異常";//20160112 JACK: 1041218 江杰昊 異常報表查不到資料 memo為空的問題
                                selectOUTTIME = "";
                            }
                            else if (selectINTIME == selectOUTTIME && lstDayOff.Count > 0) ///只有一筆刷卡資料且有請假
                            {
                                KIND = "下班未刷卡"; //事實上仍未刷卡故顯示
                                MEMO = "請假"; //但備註欄補上請假資訊
                                selectOUTTIME = "";
                            }
                            ///20151209 Neil : 此部分原先的設計共有兩種情況會進入下面的 else if :
                            /// 1. 有人下午請假，但忘記刷下班卡
                            /// 2. 有人平時忘記打下班卡，只好用補請假的方式去補回
                            ///但此兩種情況均不符合原先 if 裡面的設計(下班未刷、上班異常)
                            /// 
                            /// 因此補上條件 selectINTIME != selectOUTTIME，用於判斷短時間內上下班，且早於 09:01
                            /// 此 Else 適用情況 : 
                            /// 若當日有正常上下班刷卡，但下班刷卡時間早於 09:01
                            else if (selectINTIME != selectOUTTIME && int.Parse(selectOUTTIME.Replace(":", string.Empty)) < INTIME)///多筆資料都在九點前
                            {
                                if (dt_diff.Hours >= 1)
                                {
                                    if (lstDayOff.Count == 0) //無請假
                                    {
                                        KIND = "早退";
                                        MEMO = "上班異常";
                                        if (string.IsNullOrEmpty(selectOUTTIME))
                                        {
                                            selectOUTTIME = "";
                                        }

                                    }
                                    else if (lstDayOff.Count == 1)
                                    {
                                        KIND = "";
                                        MEMO = "國定假日";
                                    }
                                    else
                                    {
                                        if (lstDayOff[4].ToString().IsNormalized())
                                        {
                                            KIND = "";
                                            MEMO = lstDayOff[0].ToString();
                                        }
                                        else
                                        {
                                            string strETime = lstDayOff[4].ToString().Substring(0, 2) + lstDayOff[4].ToString().Substring(3, 2);
                                            int iETime = int.Parse(strETime);

                                            if (iETime < 1330)
                                            {
                                                KIND = "";
                                                MEMO = "早上請假";
                                            }
                                            else
                                            {
                                                KIND = "";
                                                MEMO = lstDayOff[0].ToString();
                                            }
                                        }

                                    }
                                }
                                else
                                {
                                    KIND = "早退";
                                    MEMO = "上班異常";
                                    selectOUTTIME = "";
                                }


                            }
                            else if (int.Parse(selectOUTTIME.Replace(":", string.Empty)) < OUTTIME)///下班刷卡時間早於5點
                            {
                                if (lstDayOff.Count > 1) ///有請假
                                {
                                    CODE = lstDayOff[0];

                                    DateTime TS1 = DateTime.Parse(LOGDATE + " " + selectINTIME);

                                    DateTime TS2 = DateTime.Parse(LOGDATE + " " + selectOUTTIME);

                                    TimeSpan TS = TS2 - TS1;


                                    if (TS.Hours + int.Parse(lstDayOff[6]) >= 8)
                                    {
                                        MEMO = "下午請假";
                                    }
                                    else
                                    {
                                        KIND = "早退";
                                        MEMO = "上班異常";
                                    }
                                }
                                else if (lstDayOff.Count == 1)
                                {
                                    KIND = "";
                                    MEMO = "國定假日";
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

                                if (lstDayOff.Count > 1) ///有請假
                                {
                                    if (TS.Hours + int.Parse(lstDayOff[6]) > 8)
                                    {
                                        KIND = "";
                                    }
                                    //else if (TS.Hours + int.Parse(temp[6]) == 8)
                                    //{
                                    //    KIND = "早退";
                                    //    MEMO = "請假早退";
                                    //}
                                    else
                                    {
                                        KIND = "早退";
                                        MEMO = "請假早退";
                                    }
                                }
                                else
                                {
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
                        }
                        //上班刷卡時間大於9點1分
                        else if (int.Parse(selectINTIME.Replace(":", string.Empty)) > INTIME)
                        {
                            if (selectINTIME == selectOUTTIME && lstDayOff.Count == 0)///只一筆刷卡資料
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
                            else if (selectINTIME == selectOUTTIME && lstDayOff.Count > 1)///只一筆有請假
                            {
                                CODE = lstDayOff[0];

                                if (lstDayOff[2] == "08:30" && lstDayOff[4] == "12:30")
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
                                        MEMO = "請假";
                                        selectINTIME = "";
                                    }
                                    else
                                    {
                                        KIND = "遲到下班未刷卡";
                                        MEMO = "上班異常";
                                        selectOUTTIME = "";
                                    }

                                }
                                else if (int.Parse(lstDayOff[2].Replace(":", "")) <= 900)
                                {
                                    if (int.Parse(selectINTIME.Replace(":", string.Empty)) < int.Parse(lstDayOff[4].Replace(":", string.Empty) + "00"))
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
                                if (lstDayOff.Count > 1)
                                {
                                    CODE = lstDayOff[0];

                                    DateTime TS1 = DateTime.Parse(LOGDATE + " " + selectINTIME);

                                    DateTime TS2 = DateTime.Parse(LOGDATE + " " + selectOUTTIME);

                                    TimeSpan TS = TS2 - TS1;

                                    if (TS.Hours + int.Parse(lstDayOff[6]) >= 8)
                                    {
                                        KIND = "";
                                        MEMO = "早上請假";
                                    }
                                    else
                                    {
                                        KIND = "遲到早退";
                                        // Joy 當天有請假紀錄，則不出現上班異常備註，新需求要加註請假類別。
                                        // MEMO = "上班異常";  
                                        if (!string.IsNullOrEmpty(CODE))
                                        {
                                            MEMO = CODE;
                                        }
                                    }
                                }
                                else if (lstDayOff.Count == 1)
                                {
                                    KIND = "";
                                    MEMO = "國定假日";
                                }
                                else
                                {
                                    KIND = "遲到早退";
                                    MEMO = "上班異常";
                                }
                            }
                            else
                            {
                                if (lstDayOff.Count > 1)
                                {
                                    CODE = lstDayOff[0];

                                    MEMO = "正常";
                                }
                                else if (lstDayOff.Count == 1)
                                {
                                    KIND = "";
                                    MEMO = "國定假日";
                                }
                                else
                                {
                                    KIND = "遲到";
                                    MEMO = "上班異常";
                                }
                            }
                        }
                        //沒有上班刷卡時間(這條應該不會觸發?因為上一層的else條件,已經過濾掉)
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
                    if (C_CARD_EDIT == "99")
                    {
                        MEMO = "因公疏未"
                            + "<br>"
                            + "辦理刷卡";
                    }
                }
            }
            #endregion 其他 含請假 上班異常 遲到...等


            string final = selectINTIME + "," + selectOUTTIME + "," + KIND + "" + "," + MEMO + "";
            return final;


        }

        public static string vExecSQL(string sqlstr)
        {
            DataTable dt = o_DBFactory.ABC_toTest.Create_Table(sqlstr, "Get");
            if (dt != null && dt.Rows != null && dt.Rows.Count > 0)
            {
                return dt.Rows[0][0].ToString();
            }
            else
            {
                return "";
            }
        }


    }
}