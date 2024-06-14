using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using TPPDDB.Helpers;

namespace TPPDDB._3_forleave
{
    public class C_CountCardRecord
    {
        public static List<string> returnCardRecord(string MZ_ID)
        {
            List<String> tempList = new List<string>();

            return tempList;
        }
        /// <summary>
        /// 計算未刷卡當天是否有請假
        /// </summary>
        /// <param name="MZ_ID">身分證號</param>
        /// <param name="MZ_DATE">日期</param>
        /// <returns>一般請假：[0]假別, [1]請假起日, [2]起時, [3]訖日, [4]迄時, [5]日數, [6]時數；國定假日：只回傳假日名稱</returns>
        public static List<String> list_Abnormal(string MZ_ID, string MZ_DATE)
        {
            List<String> CODE = new List<string>();
            //bool x = true;//當月請假
            bool y = true;//跨月請假
            bool Z = true;//國定假日
            MZ_DATE = MZ_DATE.Replace("/", string.Empty);

            string DATE = (int.Parse(MZ_DATE.Substring(0, 4)) - 1911).ToString().PadLeft(3, '0') + MZ_DATE.Substring(4, 2) + MZ_DATE.Substring(6, 2);

            string strIDATE1 = @"SELECT MZ_IDATE1,MZ_ITIME1,MZ_ODATE,MZ_OTIME,MZ_TDAY,MZ_TTIME,C_DLTB01.MZ_CODE ,MZ_CNAME MZ_CODE_CH FROM C_DLTB01
                      LEFT JOIN C_DLCODE ON C_DLCODE.MZ_CODE=C_DLTB01.MZ_CODE
                      WHERE 1=1
                        --排除掉07公差,公差也得正常打卡
                      AND C_DLTB01.MZ_CODE not in('07')
                      AND MZ_ID='" + MZ_ID + @"' 
                        AND dbo.SUBSTR(MZ_IDATE1,1,3)='" + DATE.Substring(0, 3) + @"' AND '" + DATE.Substring(0, 3) + @"'<= dbo.SUBSTR(MZ_ODATE,1,3)
                        AND (MZ_TDAY>0 OR MZ_TTIME>0) " +
                      "ORDER BY MZ_IDATE1 , MZ_ITIME1";

            DataTable DT_C_DLTB01 = new DataTable();

            DT_C_DLTB01 = o_DBFactory.ABC_toTest.Create_Table(strIDATE1, "GETIDATE1");

            //因為會造成報表判定錯誤,先註解掉
            //if (DT_C_DLTB01.Rows.Count == 0)
            //    return CODE;

            for (int j = 0; j < DT_C_DLTB01.Rows.Count; j++)
            {
                int LOGDATE = int.Parse(DATE);

                int IDATE1 = int.Parse(DT_C_DLTB01.Rows[j]["MZ_IDATE1"].ToString());

                int ODATE = int.Parse(DT_C_DLTB01.Rows[j]["MZ_ODATE"].ToString());

                if (LOGDATE >= IDATE1 && LOGDATE <= ODATE)
                {

                    //CODE.Insert(0, o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_CNAME FROM C_DLCODE WHERE MZ_CODE='" + IDATE1_DT.Rows[j]["MZ_CODE"].ToString() + "'"));

                    if (LOGDATE == IDATE1 && LOGDATE == ODATE)
                    {
                        string TTIME = "";

                        if (DT_C_DLTB01.Rows[j]["MZ_TDAY"].ToString() == "1")
                        {
                            TTIME = "8";
                        }
                        else
                        {
                            TTIME = DT_C_DLTB01.Rows[j]["MZ_TTIME"].ToString();
                        }

                        //20141226 如果有人當天有兩筆請假紀錄請累加
                        if (CODE.Count == 0)
                        {
                            CODE.Insert(0, DT_C_DLTB01.Rows[j]["MZ_CODE_CH"].ToString());
                            CODE.Insert(1, DT_C_DLTB01.Rows[j]["MZ_IDATE1"].ToString());
                            CODE.Insert(2, DT_C_DLTB01.Rows[j]["MZ_ITIME1"].ToString());
                            CODE.Insert(3, DT_C_DLTB01.Rows[j]["MZ_ODATE"].ToString());
                            CODE.Insert(4, DT_C_DLTB01.Rows[j]["MZ_OTIME"].ToString());
                            CODE.Insert(5, DT_C_DLTB01.Rows[j]["MZ_TDAY"].ToString());
                            CODE.Insert(6, TTIME);
                        }
                        else
                        {

                            CODE[3] = DT_C_DLTB01.Rows[j]["MZ_ODATE"].ToString();
                            CODE[4] = DT_C_DLTB01.Rows[j]["MZ_OTIME"].ToString();
                            CODE[5] = (int.Parse(CODE[5].ToString()) + int.Parse(DT_C_DLTB01.Rows[j]["MZ_TDAY"].ToString())).ToString();
                            CODE[6] = (int.Parse(CODE[6].ToString()) + int.Parse(TTIME)).ToString();


                        }
                    }
                    else if (LOGDATE == ODATE)
                    {
                        string TTIME = "";
                        if (DT_C_DLTB01.Rows[j]["MZ_TTIME"].ToString() == "0")
                        {
                            TTIME = "8";
                        }
                        else
                        {
                            TTIME = DT_C_DLTB01.Rows[j]["MZ_TTIME"].ToString();
                        }

                        //20141226 如果有人當天有兩筆請假紀錄請累加
                        if (CODE.Count == 0)
                        {
                            CODE.Insert(0, DT_C_DLTB01.Rows[j]["MZ_CODE_CH"].ToString());
                            CODE.Insert(1, DT_C_DLTB01.Rows[j]["MZ_IDATE1"].ToString());
                            CODE.Insert(2, DT_C_DLTB01.Rows[j]["MZ_ITIME1"].ToString());
                            CODE.Insert(3, DT_C_DLTB01.Rows[j]["MZ_ODATE"].ToString());
                            CODE.Insert(4, DT_C_DLTB01.Rows[j]["MZ_OTIME"].ToString());
                            CODE.Insert(5, DT_C_DLTB01.Rows[j]["MZ_TDAY"].ToString());
                            CODE.Insert(6, TTIME);
                        }
                        else
                        {

                            CODE[3] = DT_C_DLTB01.Rows[j]["MZ_ODATE"].ToString();
                            CODE[4] = DT_C_DLTB01.Rows[j]["MZ_OTIME"].ToString();
                            CODE[5] = (int.Parse(CODE[5].ToString()) + int.Parse(DT_C_DLTB01.Rows[j]["MZ_TDAY"].ToString())).ToString();
                            CODE[6] = (int.Parse(CODE[6].ToString()) + int.Parse(TTIME)).ToString();


                        }
                        //CODE.Insert(1, IDATE1_DT.Rows[j]["MZ_IDATE1"].ToString());
                        //CODE.Insert(2, IDATE1_DT.Rows[j]["MZ_ITIME1"].ToString());
                        //CODE.Insert(3, IDATE1_DT.Rows[j]["MZ_ODATE"].ToString());
                        //CODE.Insert(4, IDATE1_DT.Rows[j]["MZ_OTIME"].ToString());
                        //CODE.Insert(5, IDATE1_DT.Rows[j]["MZ_TDAY"].ToString());
                        //CODE.Insert(6, TTIME);
                    }
                    else if (LOGDATE >= IDATE1 && LOGDATE < ODATE)
                    {

                        //20141226 如果有人當天有兩筆請假紀錄請累加
                        if (CODE.Count == 0)
                        {
                            CODE.Insert(0, DT_C_DLTB01.Rows[j]["MZ_CODE_CH"].ToString());
                            CODE.Insert(1, DT_C_DLTB01.Rows[j]["MZ_IDATE1"].ToString());
                            CODE.Insert(2, DT_C_DLTB01.Rows[j]["MZ_ITIME1"].ToString());
                            CODE.Insert(3, DT_C_DLTB01.Rows[j]["MZ_ODATE"].ToString());
                            CODE.Insert(4, DT_C_DLTB01.Rows[j]["MZ_OTIME"].ToString());
                            CODE.Insert(5, DT_C_DLTB01.Rows[j]["MZ_TDAY"].ToString());
                            CODE.Insert(6, "8");
                        }
                        else
                        {

                            CODE[3] = DT_C_DLTB01.Rows[j]["MZ_ODATE"].ToString();
                            CODE[4] = DT_C_DLTB01.Rows[j]["MZ_OTIME"].ToString();
                            CODE[5] = (int.Parse(CODE[5].ToString()) + int.Parse(DT_C_DLTB01.Rows[j]["MZ_TDAY"].ToString())).ToString();
                            CODE[6] = (int.Parse(CODE[6].ToString()) + 8).ToString();


                        }
                        //CODE.Insert(1, IDATE1_DT.Rows[j]["MZ_IDATE1"].ToString());
                        //CODE.Insert(2, IDATE1_DT.Rows[j]["MZ_ITIME1"].ToString());
                        //CODE.Insert(3, IDATE1_DT.Rows[j]["MZ_ODATE"].ToString());
                        //CODE.Insert(4, IDATE1_DT.Rows[j]["MZ_OTIME"].ToString());
                        //CODE.Insert(5, IDATE1_DT.Rows[j]["MZ_TDAY"].ToString());
                        //CODE.Insert(6, "8");
                    }

                    y = false;
                    //Z = false;
                }
            }

            if (y)
            {
                string strODATE = "SELECT MZ_IDATE1,MZ_ITIME1,MZ_ODATE,MZ_OTIME,MZ_TDAY,MZ_TTIME,MZ_CODE FROM C_DLTB01 WHERE MZ_ID='" + MZ_ID + "' AND dbo.SUBSTR(MZ_ODATE,1,5)='" + MZ_DATE.Replace("/", string.Empty).Substring(0, 5) + "' AND dbo.SUBSTR(MZ_IDATE1,1,5)<>'" + MZ_DATE.Replace("/", string.Empty).Substring(0, 5) + "'  AND (MZ_TDAY>0 OR MZ_TTIME>0) ";

                DataTable ODATE_DT = new DataTable();

                ODATE_DT = o_DBFactory.ABC_toTest.Create_Table(strODATE, "GETIDATE2");

                for (int j = 0; j < ODATE_DT.Rows.Count; j++)
                {
                    int LOGDATE = int.Parse(MZ_DATE.Replace("/", string.Empty));

                    int IDATE1 = int.Parse(ODATE_DT.Rows[j]["MZ_IDATE"].ToString());

                    int ODATE = int.Parse(ODATE_DT.Rows[j]["MZ_ODATE"].ToString());

                    if (LOGDATE >= IDATE1 && LOGDATE <= ODATE)
                    {
                        if (LOGDATE == IDATE1 && LOGDATE == ODATE)
                        {
                            string TTIME = "";
                            if (DT_C_DLTB01.Rows[j]["MZ_TDAY"].ToString() == "1")
                            {
                                TTIME = "8";
                            }
                            else
                            {
                                TTIME = DT_C_DLTB01.Rows[j]["MZ_TTIME"].ToString();
                            }

                            CODE.Insert(0, "");
                            CODE.Insert(1, DT_C_DLTB01.Rows[j]["MZ_IDATE1"].ToString());
                            CODE.Insert(2, DT_C_DLTB01.Rows[j]["MZ_ITIME1"].ToString());
                            CODE.Insert(3, DT_C_DLTB01.Rows[j]["MZ_ODATE"].ToString());
                            CODE.Insert(4, DT_C_DLTB01.Rows[j]["MZ_OTIME"].ToString());
                            CODE.Insert(5, DT_C_DLTB01.Rows[j]["MZ_TDAY"].ToString());
                            CODE.Insert(6, TTIME);
                        }
                        else if (LOGDATE == ODATE)
                        {
                            string TTIME = "";
                            if (DT_C_DLTB01.Rows[j]["MZ_TTIME"].ToString() == "0")
                            {
                                TTIME = "8";
                            }
                            else
                            {
                                TTIME = DT_C_DLTB01.Rows[j]["MZ_TTIME"].ToString();
                            }

                            CODE.Insert(0, "");
                            CODE.Insert(1, DT_C_DLTB01.Rows[j]["MZ_IDATE1"].ToString());
                            CODE.Insert(2, DT_C_DLTB01.Rows[j]["MZ_ITIME1"].ToString());
                            CODE.Insert(3, DT_C_DLTB01.Rows[j]["MZ_ODATE"].ToString());
                            CODE.Insert(4, DT_C_DLTB01.Rows[j]["MZ_OTIME"].ToString());
                            CODE.Insert(5, DT_C_DLTB01.Rows[j]["MZ_TDAY"].ToString());
                            CODE.Insert(6, TTIME);
                        }
                        else if (LOGDATE >= IDATE1 && LOGDATE < ODATE)
                        {
                            CODE.Insert(0, "");
                            CODE.Insert(1, DT_C_DLTB01.Rows[j]["MZ_IDATE1"].ToString());
                            CODE.Insert(2, DT_C_DLTB01.Rows[j]["MZ_ITIME1"].ToString());
                            CODE.Insert(3, DT_C_DLTB01.Rows[j]["MZ_ODATE"].ToString());
                            CODE.Insert(4, DT_C_DLTB01.Rows[j]["MZ_OTIME"].ToString());
                            CODE.Insert(5, DT_C_DLTB01.Rows[j]["MZ_TDAY"].ToString());
                            CODE.Insert(6, "8");
                        }

                        //Z = false;
                    }
                }
            }

            if (Z)
            {
                string HOLIDAY = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_HOLIDAY_NAME FROM C_DUTYHOLIDAY WHERE MZ_HOLIDAY_DATE='" + DATE + "'");

                if (CODE.Count > 0)
                {
                    if (String.IsNullOrEmpty(CODE[0].ToString()))
                    {
                        if (!string.IsNullOrEmpty(HOLIDAY))
                        {
                            CODE.Insert(0, HOLIDAY);
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(HOLIDAY))
                        {
                            CODE[0] = HOLIDAY;
                        }
                    }

                }
                else
                {
                    if (!string.IsNullOrEmpty(HOLIDAY))
                    {
                        CODE.Insert(0, HOLIDAY);
                    }
                }

            }

            return CODE;
        }

        /// <summary>
        /// 計算未刷卡當天是否有請假
        /// </summary>
        /// <param name="MZ_ID">身分證號</param>
        /// <param name="MZ_DATE">日期</param>
        /// <returns>一般請假：假別,請假起日,起時,訖日,迄時,日數,時數</returns>
        ///          國定假日：只回傳假日名稱
        public static List<String> list_Abnormal(string MZ_ID, string MZ_DATE, int mode)
        {
            List<String> CODE = new List<string>();
            //bool x = true;//當月請假
            bool y = true;//跨月請假
            bool Z = true;//國定假日
            MZ_DATE = MZ_DATE.Replace("/", string.Empty);

            string DATE = (int.Parse(MZ_DATE.Substring(0, 4)) - 1911).ToString().PadLeft(3, '0') + MZ_DATE.Substring(4, 2) + MZ_DATE.Substring(6, 2);
            //根據 員警證號 當天日期,有請假的天數或時數
            string strIDATE1 = @"SELECT MZ_IDATE1,MZ_ITIME1,MZ_ODATE,MZ_OTIME,MZ_TDAY,MZ_TTIME,C_DLTB01.MZ_CODE ,MZ_CNAME MZ_CODE_CH FROM C_DLTB01
                      LEFT JOIN C_DLCODE ON C_DLCODE.MZ_CODE=C_DLTB01.MZ_CODE 
                      WHERE 1=1
                        --排除掉07公差,公差也得正常打卡
                      AND C_DLTB01.MZ_CODE not in('07')
                        AND MZ_ID='" + MZ_ID + "' AND dbo.SUBSTR(MZ_IDATE1,1,3)='" + DATE.Substring(0, 3) + "'  AND (MZ_TDAY>0 OR MZ_TTIME>0) " +
                      "ORDER BY MZ_IDATE1 , MZ_ITIME1";

            DataTable DT_C_DLTB01 = new DataTable();
            DT_C_DLTB01 = o_DBFactory.ABC_toTest.Create_Table(strIDATE1, "GETIDATE1");

            for (int j = 0; j < DT_C_DLTB01.Rows.Count; j++)
            {
                //應簽到日期
                int LOGDATE = int.Parse(DATE);

                int IDATE1 = int.Parse(DT_C_DLTB01.Rows[j]["MZ_IDATE1"].ToString());

                int ODATE = int.Parse(DT_C_DLTB01.Rows[j]["MZ_ODATE"].ToString());
                //如果 應簽到日期 晚於等於請假開始,早於等於請假結束,代表這在我們要搜尋的範圍內,反之則跳過
                if ((LOGDATE >= IDATE1 && LOGDATE <= ODATE) == false)
                {   //跳過
                    continue;
                }

                //在那之前,先抓取CODE[6] ,即MZ_TTIME
                //Code[6]有東西嗎? 如果陣列有七個元素代表有東西
                int iTTIME_Prev = 0;
                if (CODE.Count > 6)
                {   //先擷取出來
                    int.TryParse(CODE[6], out iTTIME_Prev);
                }

                //將查詢到的結果開始寫入,這邊比較特殊,用的是List<String>型別,取六個元素
                CODE.Insert(0, DT_C_DLTB01.Rows[j]["MZ_CODE_CH"].ToString());
                //CODE.Insert(0, o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_CNAME FROM C_DLCODE WHERE MZ_CODE='" + IDATE1_DT.Rows[j]["MZ_CODE"].ToString() + "'"));
                //如果簽到日期等於請假起訖日,也就是當天一天請假
                if (LOGDATE == IDATE1 && LOGDATE == ODATE)
                {
                    //計算當天到底請了幾個小時
                    string TTIME = "";
                    //MZ_TDAY 請假的天數,扣除剩餘的小時,EX: 1天又4小時,則MZ_TDAY為 1
                    if (DT_C_DLTB01.Rows[j]["MZ_TDAY"].ToString() == "1")
                    {   //一天折合8小時
                        TTIME = "8";
                    }
                    else
                    {   //MZ_TTIME 請假的小時數,扣除掉日數後,EX: 1天又4小時,則MZ_TTIME為4
                        TTIME = DT_C_DLTB01.Rows[j]["MZ_TTIME"].ToString();
                    }


                    //
                    CODE.Insert(1, DT_C_DLTB01.Rows[j]["MZ_IDATE1"].ToString());
                    CODE.Insert(2, DT_C_DLTB01.Rows[j]["MZ_ITIME1"].ToString());
                    CODE.Insert(3, DT_C_DLTB01.Rows[j]["MZ_ODATE"].ToString());
                    CODE.Insert(4, DT_C_DLTB01.Rows[j]["MZ_OTIME"].ToString());
                    CODE.Insert(5, DT_C_DLTB01.Rows[j]["MZ_TDAY"].ToString());
                    //這邊比較特殊,需要跟上一回合得到的小時數字相加,如果當天申請兩次請假資料,小時數才會疊加上去 
                    int iTTIME = 0;
                    int.TryParse(TTIME, out iTTIME);
                    CODE.Insert(6, (iTTIME_Prev + iTTIME).ToString());
                }
                //否則,如果只有請假迄日當天,相當於請多天假,且在收假最後一天
                //EX: 請假 2023/03/01 ~ 2023/03/05 ,且目前要找的刷卡日期在03/05
                else if (LOGDATE == ODATE)
                {
                    string TTIME = "";
                    if (DT_C_DLTB01.Rows[j]["MZ_TTIME"].ToString() == "0")
                    {
                        TTIME = "8";
                    }
                    else
                    {
                        TTIME = DT_C_DLTB01.Rows[j]["MZ_TTIME"].ToString();
                    }

                    CODE.Insert(1, DT_C_DLTB01.Rows[j]["MZ_IDATE1"].ToString());
                    CODE.Insert(2, DT_C_DLTB01.Rows[j]["MZ_ITIME1"].ToString());
                    CODE.Insert(3, DT_C_DLTB01.Rows[j]["MZ_ODATE"].ToString());
                    CODE.Insert(4, DT_C_DLTB01.Rows[j]["MZ_OTIME"].ToString());
                    CODE.Insert(5, DT_C_DLTB01.Rows[j]["MZ_TDAY"].ToString());
                    CODE.Insert(6, TTIME);
                }
                //否則,應簽到日期 不在 請假的最後一天
                else if (LOGDATE >= IDATE1 && LOGDATE < ODATE)
                {
                    CODE.Insert(1, DT_C_DLTB01.Rows[j]["MZ_IDATE1"].ToString());
                    CODE.Insert(2, DT_C_DLTB01.Rows[j]["MZ_ITIME1"].ToString());
                    CODE.Insert(3, DT_C_DLTB01.Rows[j]["MZ_ODATE"].ToString());
                    CODE.Insert(4, DT_C_DLTB01.Rows[j]["MZ_OTIME"].ToString());
                    CODE.Insert(5, DT_C_DLTB01.Rows[j]["MZ_TDAY"].ToString());
                    CODE.Insert(6, "8");
                }

                y = false;
                Z = false;

            }

            if (y)
            {
                string strODATE = "SELECT  MZ_IDATE1,MZ_ITIME1,MZ_ODATE,MZ_OTIME,MZ_TDAY,MZ_TTIME FROM C_DLTB01 WHERE MZ_ID='" + MZ_ID + "' AND dbo.SUBSTR(MZ_ODATE,1,5)='" + MZ_DATE.Replace("/", string.Empty).Substring(0, 5) + "' AND dbo.SUBSTR(MZ_IDATE1,1,5)<>'" + MZ_DATE.Replace("/", string.Empty).Substring(0, 5) + "'  AND (MZ_TDAY>0 OR MZ_TTIME>0)";

                DataTable ODATE_DT = new DataTable();

                ODATE_DT = o_DBFactory.ABC_toTest.Create_Table(strODATE, "GETIDATE2");

                for (int j = 0; j < ODATE_DT.Rows.Count; j++)
                {
                    int LOGDATE = int.Parse(MZ_DATE.Replace("/", string.Empty));

                    int IDATE1 = int.Parse(ODATE_DT.Rows[j]["MZ_IDATE"].ToString());

                    int ODATE = int.Parse(ODATE_DT.Rows[j]["MZ_ODATE"].ToString());

                    if (LOGDATE >= IDATE1 && LOGDATE <= ODATE)
                    {
                        if (LOGDATE == IDATE1 && LOGDATE == ODATE)
                        {
                            string TTIME = "";
                            if (DT_C_DLTB01.Rows[j]["MZ_TDAY"].ToString() == "1")
                            {
                                TTIME = "8";
                            }
                            else
                            {
                                TTIME = DT_C_DLTB01.Rows[j]["MZ_TTIME"].ToString();
                            }

                            CODE.Insert(1, DT_C_DLTB01.Rows[j]["MZ_IDATE1"].ToString());
                            CODE.Insert(2, DT_C_DLTB01.Rows[j]["MZ_ITIME1"].ToString());
                            CODE.Insert(3, DT_C_DLTB01.Rows[j]["MZ_ODATE"].ToString());
                            CODE.Insert(4, DT_C_DLTB01.Rows[j]["MZ_OTIME"].ToString());
                            CODE.Insert(5, DT_C_DLTB01.Rows[j]["MZ_TDAY"].ToString());
                            CODE.Insert(6, TTIME);
                        }
                        else if (LOGDATE == ODATE)
                        {
                            string TTIME = "";
                            if (DT_C_DLTB01.Rows[j]["MZ_TTIME"].ToString() == "0")
                            {
                                TTIME = "8";
                            }
                            else
                            {
                                TTIME = DT_C_DLTB01.Rows[j]["MZ_TTIME"].ToString();
                            }

                            CODE.Insert(1, DT_C_DLTB01.Rows[j]["MZ_IDATE1"].ToString());
                            CODE.Insert(2, DT_C_DLTB01.Rows[j]["MZ_ITIME1"].ToString());
                            CODE.Insert(3, DT_C_DLTB01.Rows[j]["MZ_ODATE"].ToString());
                            CODE.Insert(4, DT_C_DLTB01.Rows[j]["MZ_OTIME"].ToString());
                            CODE.Insert(5, DT_C_DLTB01.Rows[j]["MZ_TDAY"].ToString());
                            CODE.Insert(6, TTIME);
                        }
                        else if (LOGDATE >= IDATE1 && LOGDATE < ODATE)
                        {
                            CODE.Insert(1, DT_C_DLTB01.Rows[j]["MZ_IDATE1"].ToString());
                            CODE.Insert(2, DT_C_DLTB01.Rows[j]["MZ_ITIME1"].ToString());
                            CODE.Insert(3, DT_C_DLTB01.Rows[j]["MZ_ODATE"].ToString());
                            CODE.Insert(4, DT_C_DLTB01.Rows[j]["MZ_OTIME"].ToString());
                            CODE.Insert(5, DT_C_DLTB01.Rows[j]["MZ_TDAY"].ToString());
                            CODE.Insert(6, "8");
                        }

                        Z = false;
                    }
                }
            }

            if (Z)
            {
                string HOLIDAY = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_HOLIDAY_NAME FROM C_DUTYHOLIDAY WHERE MZ_HOLIDAY_DATE='" + DATE + "'");
                if (!string.IsNullOrEmpty(HOLIDAY))
                {
                    CODE.Insert(0, HOLIDAY);
                }
            }

            return CODE;
        }

        /// <summary>
        /// 根據 身分證號 日期,抓取當天的請假資料(包括請假起訖時間),也包括國定假日
        /// </summary>
        /// <param name="MZ_ID"></param>
        /// <param name="MZ_DATE"></param>
        /// <returns></returns>
        public static Data_CountCardRecord Get_Abnormal(string MZ_ID, string MZ_DATE)
        {
            Data_CountCardRecord CODE = new Data_CountCardRecord();
            //bool x = true;//當月請假
            bool y = true;//跨月請假
            bool Z = true;//國定假日
            MZ_DATE = MZ_DATE.Replace("/", string.Empty);

            string DATE = (int.Parse(MZ_DATE.Substring(0, 4)) - 1911).ToString().PadLeft(3, '0') + MZ_DATE.Substring(4, 2) + MZ_DATE.Substring(6, 2);
            //根據 員警證號 當天日期,有請假的天數或時數
            string strIDATE1 = @"SELECT MZ_IDATE1,MZ_ITIME1,MZ_ODATE,MZ_OTIME,MZ_TDAY,MZ_TTIME,C_DLTB01.MZ_CODE ,MZ_CNAME MZ_CODE_CH FROM C_DLTB01
                      LEFT JOIN C_DLCODE ON C_DLCODE.MZ_CODE=C_DLTB01.MZ_CODE 
                      WHERE 1=1
                        --排除掉07公差,公差也得正常打卡
                      AND C_DLTB01.MZ_CODE not in('07')
                        AND MZ_ID='" + MZ_ID + "' AND dbo.SUBSTR(MZ_IDATE1,1,3)='" + DATE.Substring(0, 3) + "'  AND (MZ_TDAY>0 OR MZ_TTIME>0) " +
                      "ORDER BY MZ_IDATE1 , MZ_ITIME1";

            DataTable DT_C_DLTB01 = new DataTable();
            DT_C_DLTB01 = o_DBFactory.ABC_toTest.Create_Table(strIDATE1, "GETIDATE1");

            for (int j = 0; j < DT_C_DLTB01.Rows.Count; j++)
            {
                //應簽到日期
                int LOGDATE = int.Parse(DATE);

                int IDATE1 = int.Parse(DT_C_DLTB01.Rows[j]["MZ_IDATE1"].ToString());

                int ODATE = int.Parse(DT_C_DLTB01.Rows[j]["MZ_ODATE"].ToString());
                //如果 應簽到日期 晚於等於請假開始,早於等於請假結束,代表這在我們要搜尋的範圍內,反之則跳過
                if ((LOGDATE >= IDATE1 && LOGDATE <= ODATE) == false)
                {   //跳過
                    continue;
                }

                //在那之前,先抓取CODE[6] ,即MZ_TTIME
                //Code[6]有東西嗎? 如果陣列有七個元素代表有東西
                int iTTIME_Prev = 0;
                if (CODE.MZ_TTIME.IsNullOrEmpty())
                {   //先擷取出來
                    int.TryParse(CODE.MZ_TTIME, out iTTIME_Prev);
                }

                //將查詢到的結果開始寫入,這邊比較特殊,用的是List<String>型別,取六個元素
                CODE.MZ_CODE_CH = DT_C_DLTB01.Rows[j]["MZ_CODE_CH"].ToString();
                //CODE.Insert(0, DT_C_DLTB01.Rows[j]["MZ_CODE_CH"].ToString());
                //CODE.Insert(0, o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_CNAME FROM C_DLCODE WHERE MZ_CODE='" + IDATE1_DT.Rows[j]["MZ_CODE"].ToString() + "'"));
                //如果簽到日期等於請假起訖日,也就是當天一天請假
                if (LOGDATE == IDATE1 && LOGDATE == ODATE)
                {
                    //計算當天到底請了幾個小時
                    string TTIME = "";
                    //MZ_TDAY 請假的天數,扣除剩餘的小時,EX: 1天又4小時,則MZ_TDAY為 1
                    if (DT_C_DLTB01.Rows[j]["MZ_TDAY"].ToString() == "1")
                    {   //一天折合8小時
                        TTIME = "8";
                    }
                    else
                    {   //MZ_TTIME 請假的小時數,扣除掉日數後,EX: 1天又4小時,則MZ_TTIME為4
                        TTIME = DT_C_DLTB01.Rows[j]["MZ_TTIME"].ToString();
                    }


                    //綁定資料,從C_DLTB01表的DataSet
                    CODE.Binding(DT_C_DLTB01.Rows[j]);

                    //這邊比較特殊,需要跟上一回合得到的小時數字相加,如果當天申請兩次請假資料,小時數才會疊加上去 
                    int iTTIME = 0;
                    int.TryParse(TTIME, out iTTIME);
                    CODE.MZ_TTIME = (iTTIME_Prev + iTTIME).ToString();
                }
                //否則,如果只有請假迄日當天,相當於請多天假,且在收假最後一天
                //EX: 請假 2023/03/01 ~ 2023/03/05 ,且目前要找的刷卡日期在03/05
                else if (LOGDATE == ODATE)
                {
                    string TTIME = "";
                    if (DT_C_DLTB01.Rows[j]["MZ_TTIME"].ToString() == "0")
                    {
                        TTIME = "8";
                    }
                    else
                    {
                        TTIME = DT_C_DLTB01.Rows[j]["MZ_TTIME"].ToString();
                    }

                    //綁定資料,從C_DLTB01表的DataSet
                    CODE.Binding(DT_C_DLTB01.Rows[j]);
                    CODE.MZ_TTIME = TTIME;
                    //CODE.Insert(6, TTIME);
                }
                //否則,應簽到日期 不在 請假的最後一天
                else if (LOGDATE >= IDATE1 && LOGDATE < ODATE)
                {
                    //綁定資料,從C_DLTB01表的DataSet
                    CODE.Binding(DT_C_DLTB01.Rows[j]);
                    CODE.MZ_TTIME = "8";
                    //CODE.Insert(6, "8");
                }

                y = false;
                Z = false;

            }

            if (y)
            {
                string strODATE = "SELECT  MZ_IDATE1,MZ_ITIME1,MZ_ODATE,MZ_OTIME,MZ_TDAY,MZ_TTIME FROM C_DLTB01 WHERE MZ_ID='" + MZ_ID + "' AND dbo.SUBSTR(MZ_ODATE,1,5)='" + MZ_DATE.Replace("/", string.Empty).Substring(0, 5) + "' AND dbo.SUBSTR(MZ_IDATE1,1,5)<>'" + MZ_DATE.Replace("/", string.Empty).Substring(0, 5) + "'  AND (MZ_TDAY>0 OR MZ_TTIME>0)";

                DataTable ODATE_DT = new DataTable();

                ODATE_DT = o_DBFactory.ABC_toTest.Create_Table(strODATE, "GETIDATE2");

                for (int j = 0; j < ODATE_DT.Rows.Count; j++)
                {
                    int LOGDATE = int.Parse(MZ_DATE.Replace("/", string.Empty));

                    int IDATE1 = int.Parse(ODATE_DT.Rows[j]["MZ_IDATE"].ToString());

                    int ODATE = int.Parse(ODATE_DT.Rows[j]["MZ_ODATE"].ToString());

                    if (LOGDATE >= IDATE1 && LOGDATE <= ODATE)
                    {
                        if (LOGDATE == IDATE1 && LOGDATE == ODATE)
                        {
                            string TTIME = "";
                            if (DT_C_DLTB01.Rows[j]["MZ_TDAY"].ToString() == "1")
                            {
                                TTIME = "8";
                            }
                            else
                            {
                                TTIME = DT_C_DLTB01.Rows[j]["MZ_TTIME"].ToString();
                            }


                            //綁定資料,從C_DLTB01表的DataSet
                            CODE.Binding(DT_C_DLTB01.Rows[j]);
                            CODE.MZ_TTIME = TTIME;
                            //CODE.Insert(6, TTIME);
                        }
                        else if (LOGDATE == ODATE)
                        {
                            string TTIME = "";
                            if (DT_C_DLTB01.Rows[j]["MZ_TTIME"].ToString() == "0")
                            {
                                TTIME = "8";
                            }
                            else
                            {
                                TTIME = DT_C_DLTB01.Rows[j]["MZ_TTIME"].ToString();
                            }


                            //綁定資料,從C_DLTB01表的DataSet
                            CODE.Binding(DT_C_DLTB01.Rows[j]);
                            CODE.MZ_TTIME = TTIME;
                            //CODE.Insert(6, TTIME);
                        }
                        else if (LOGDATE >= IDATE1 && LOGDATE < ODATE)
                        {

                            //綁定資料,從C_DLTB01表的DataSet
                            CODE.Binding(DT_C_DLTB01.Rows[j]);
                            CODE.MZ_TTIME = "8";
                            //CODE.Insert(6, "8");
                        }

                        Z = false;
                    }
                }
            }

            if (Z)
            {
                string HOLIDAY = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_HOLIDAY_NAME FROM C_DUTYHOLIDAY WHERE MZ_HOLIDAY_DATE='" + DATE + "'");
                if (!string.IsNullOrEmpty(HOLIDAY))
                {
                    CODE.isNationlHoliday = true;
                    //國定假日名稱
                    CODE.MZ_CODE_CH = HOLIDAY;
                    //CODE.Insert(0, HOLIDAY);
                }
            }

            return CODE;
        }


        /// <summary>
        /// 單元測試方法,要比對這兩個方法是不是有相同結果,以便未來整合一起
        /// </summary>
        /// <param name="MZ_ID"></param>
        /// <returns></returns>
        public static string UnitTEST_list_Abnormal(string MZ_ID)
        {
            //從昨天開始
            DateTime DT = DateTime.Now.Date.AddDays(-1);

            for (int i = 0; i < 365; i++)
            {
                //昨天開始,減去 i 天
                DateTime DT_CurrentRound = DT.AddDays(i * -1);
                //轉成民國年日期字串 YYYMMdd
                string MZ_DATE = DT_CurrentRound.ToString("yyyy/MM/dd");

                //抓取資料A
                var list_A = list_Abnormal(MZ_ID, MZ_DATE);
                //抓取資料B
                var list_B = list_Abnormal(MZ_ID, MZ_DATE, 0);
                //轉換成字串
                string strA = string.Join("|", list_A.ToArray());
                string strB = string.Join("|", list_B.ToArray());

                //長不一樣?代表兩者有出入
                if (strA != strB)
                {
                    string msg = @" 測試案例:" + MZ_ID + @" " + MZ_DATE + @" 資料比對不符合 strA=" + strA + @"  strB=" + strB;

                    TPPDDB.App_Code.Log.SaveLog("UnitTEST_list_Abnormal", "1", msg);
                    return msg;
                }

            }
            return "測試成功";
        }

    }
    /// <summary>
    /// 請假資料
    /// </summary>
    public class Data_CountCardRecord
    {
        /// <summary>
        /// 國定假日嗎?
        /// </summary>
        public bool isNationlHoliday { get; set; } = false;

        /// <summary>
        /// 周末六日嗎?
        /// </summary>
        public bool isWeekend { get; set; } = false;

        /// <summary>
        /// 放假嗎?
        /// </summary>
        public bool isHoliday { get { return isNationlHoliday || isWeekend; } }

        /// <summary>
        /// 假別名稱,或者國定假日名稱
        /// </summary>
        public string MZ_CODE_CH { get; set; } = "";

        /// <summary>
        /// 請假起 日期
        /// </summary>
        public string MZ_IDATE1 { get; set; } = "";
        /// <summary>
        /// 請假起 時間
        /// </summary>
        public string MZ_ITIME1 { get; set; } = "";
        /// <summary>
        /// 請假迄 日期
        /// </summary>
        public string MZ_ODATE { get; set; } = "";
        /// <summary>
        /// 請假迄 時間
        /// </summary>
        public string MZ_OTIME { get; set; } = "";
        /// <summary>
        /// 請假日數
        /// </summary>
        public string MZ_TDAY { get; set; } = "";

        /// <summary>
        /// 請假時數
        /// </summary>
        public string MZ_TTIME { get; set; } = "";

        /// <summary>
        /// 綁定資料
        /// </summary>
        /// <param name="DR_C_DLTB01"></param>
        /// <param name="j"></param>
        public void Binding(DataRow DR_C_DLTB01)
        {
            this.MZ_IDATE1 = DR_C_DLTB01["MZ_IDATE1"].ToString();
            this.MZ_ITIME1 = DR_C_DLTB01["MZ_ITIME1"].ToString();
            this.MZ_ODATE = DR_C_DLTB01["MZ_ODATE"].ToString();
            this.MZ_OTIME = DR_C_DLTB01["MZ_OTIME"].ToString();
            this.MZ_TDAY = DR_C_DLTB01["MZ_TDAY"].ToString();
        }
    }
}
