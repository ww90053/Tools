using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TPPDDB.Helpers;
using TPPDDB.Logic;


namespace TPPDDB._3_forleave
{

    /// <summary>
    /// 資料模型,超勤資料
    /// </summary>
    public class LogicModel_C_DUTYTABLE_PERSONAL
    {
        public LogicModel_C_DUTYTABLE_PERSONAL()
        {

        }

        /// <summary>
        /// 是否啟用新制度? 外勤不放放國定假日?
        /// </summary>
        public const bool is外勤不放國定假日 = true;

        /// <summary>
        /// 當天的工作項目代碼,依照itemN順序放好,代碼請參考:C_DUTYITEM資料表
        /// 字串如 : A, B, C, C, I, I 等當日值勤狀況
        /// </summary>
        public List<String> lstSingleDayInputs { get; set; } = new List<string>();

        /// <summary>
        /// 是否修停
        /// </summary>
        public String DUTYSTOPOFF { get; set; } = "";
        /// <summary>
        /// 內外勤
        /// </summary>
        public String DUTYUSERCLASSIFY { get; set; } = "";


        /// <summary>
        /// 根據員警ID和上班日,從資料庫抓取本物件需要的資料
        /// </summary>
        /// <param name="MZ_ID"></param>
        /// <param name="DutyDate"></param>
        public LogicModel_C_DUTYTABLE_PERSONAL(string MZ_ID, string DutyDate)
        {
            DataRow dr = Service_C_DUTYTABLE_PERSONAL.getOvertimeDatarow(MZ_ID, DutyDate);
            if (dr == null)
            {   //沒抓到就不幹了...
                return;
            }
            this.DUTYSTOPOFF = dr["CONVERT_REST_HOURS"].ToStringNullSafe();
            this.DUTYUSERCLASSIFY = dr["DUTYUSERCLASSIFY"].ToStringNullSafe();
            // Joy 20150513 超勤時數增加08-09欄位，但時數統計表未合併計算，所以將i值範圍由原先27改為28
            this.lstSingleDayInputs = new List<string>();
            for (int i = 1; i < 28; i++)
            {
                String s = dr["DUTYITEM" + i.ToString()].ToString();
                this.lstSingleDayInputs.Add(s);
            }
            return;
        }


        /// <summary>
        /// 處理超勤報表單日工時計算
        /// 20150520 Neil 其他勤務新增拖吊跟交通事故
        /// 20181106 sky 修改N內勤上班、L內部管理的統計規則
        /// 20190103 sky 修改V、W、X、Y、Z 大於8小時僅以8小時計算，未滿8小時則直接加
        /// 20190129 sky 刪除輪休(Y)以外的休假統計，將有輪休(Y)的上班時數轉成超勤
        /// 20190306 sky 補上各假別關於超勤統計的公式
        /// </summary>
        /// <param name="DutyTime">回傳 總上班時數，最多僅 12 小時</param>
        /// <param name="OverTime">回傳 總超勤時數，最多僅 4 小時</param>
        /// <param name="strMZ_OCCC">職務：局長、副局長、主任祕書、督察長 這四個職務可督勤時數大於四小時</param>
        /// <param name="ITEMG">督勤項目時間字串,格式EX: "0600-0700.0800-0900.0900-1000"  </param>
        /// <param name="isHoliday">是否為國定假日</param>
        /// <returns>是否成功計算。若否則代表有例外狀況產生</returns>
        public Boolean calSingleDayOvertime_MZ_OCCC(ref Int32 DutyTime, ref Int32 OverTime, string strMZ_OCCC, string ITEMG, Boolean isHoliday)
        {
            //特殊:如果是外勤人員,則無視國定假日這段
            if (is外勤不放國定假日 && this.DUTYUSERCLASSIFY == Enum_C_DUTYTABLE_PERSONAL_DUTYUSERCLASSIFY.Outter.ToString())
            {
                //強制取消掉國定假日,也就是當作這一天是平日
                isHoliday = false;
            }
            //是否休停
            Boolean isDutyStopOff = (this.DUTYSTOPOFF == "Y");
            //今天不上班嗎?
            bool NonWorkingDay = isDutyStopOff || isHoliday;

            Int32 prv_NormalTrainTime = 0; //常訓訓練，每日最高僅能為 8
            Int32 prv_PassNightTime = 0; //值宿時數，其不在每日總時數上限12小時之限制
            // Joy 20150513 新增督勤時數，規則同專案勤務，但受最大值不得超過4限制
            int OverTimeG1 = 0;

            int IAmount = 0;
            //針對休假、請假、補休、輪休、停休計算
            int V_SumTime = 0, W_SumTime = 0, X_SumTime = 0, Y_SumTime = 0, Z_SumTime = 0;
            //判斷有上述休假時數時是否將上班時數轉成超勤
            bool is_Y_OverTime = false;
            int N_SumTime = 0; //內勤上班，每日最高僅能為 8

            try
            {
                #region Step1. 處理所有當日時數
                foreach (String item in this.lstSingleDayInputs)
                {
                    switch (item)
                    {
                        ///一般項目
                        case "A": //值班
                        case "B": //巡邏
                        case "C": //守望交整
                        case "D": //臨檢路檢
                        case "E": //勤區查察
                        case "F": //備勤
                        case "H": //其他勤務
                        case "J": //常訓
                        case "K": //刑案查察
                        // Joy 20150513 其他上班時數新增
                        case "O": // 交通事故
                        case "P": // 拖吊
                        case "Q": // 地區探詢
                        case "R": // 保護查察
                        case "S": // 案件偵處
                            DutyTime++;
                            break;
                        case "I": //專案勤務
                            OverTime++;
                            IAmount++;
                            break;
                        //Joy 20150513 督勤規則同專案勤務
                        case "G": //督勤
                            OverTimeG1++;
                            break;

                        ///獨立例外項目
                        case "M": //值宿
                            prv_PassNightTime++;
                            break;
                        case "N": // 內勤上班
                            //每日最高僅能為8，超過後即不併入每日上班時數 20181106 by sky
                            N_SumTime++;
                            if (N_SumTime <= 8)
                                DutyTime++;
                            break;
                        case "L": //走動式管理(內部管理)
                            //每日最高僅能為 8
                            //超過後即不併入每日上班時數
                            prv_NormalTrainTime++;
                            if (prv_NormalTrainTime <= 8)
                                DutyTime++;
                            break;
                        case "V": // 休假
                            if (V_SumTime < 8) { V_SumTime++; }
                            break;
                        case "W": // 請假
                            if (W_SumTime < 8) { W_SumTime++; }
                            break;
                        case "X": // 補休
                            if (X_SumTime < 8) { X_SumTime++; }
                            break;
                        case "Y": // 輪休
                            if (Y_SumTime < 8) { Y_SumTime++; }
                            break;
                        case "Z": // 停休
                            if (Z_SumTime < 8) { Z_SumTime++; }
                            break;
                    }
                }
                #endregion

                #region Step2. 處理總時數

                // Joy 20150513 計算超勤時數也要加入值宿
                ///轉換 : 處理值宿時數，直接併入上班時數
                DutyTime += prv_PassNightTime;

                // 有正常勤務(上班)時數才將各類休假時數統計至上班時數及計算超勤

                //特殊:
                // 如果今天是上班日,則上班時數也要加上請假時數
                // 如果今天非上班日,且走舊制,則同上            
                // 換言之,新制在假日就不走這段了
                if (NonWorkingDay == false || (NonWorkingDay && is外勤不放國定假日 == false))
                {   //如果當天有一般的工作時數
                    if (DutyTime > 0)
                    {   //加上請假的時數,也當作是工作時數 ,這樣後面去扣8小時去算超勤時數才不會出錯
                        DutyTime += V_SumTime + W_SumTime + X_SumTime + Y_SumTime + Z_SumTime;
                    }
                }

                ///轉換 : 上班時數轉換為超勤的時候，需超過 8 小時的剩餘部分才能轉換為超勤
                Int32 iDutyLeftTime = (DutyTime > 8) ? DutyTime - 8 : 0;

                ///轉換 : 前步驟扣 8 之後剩餘的時數，即可轉換為超勤時數，最多僅為 4
                Int32 iDutyTimeConvert = (iDutyLeftTime > 4) ? 4 : iDutyLeftTime;

                ///轉換 : 若為國定假日，則直接將原始的上班時數加入超勤時數
                if (NonWorkingDay)
                    OverTime += DutyTime;
                ///轉換：若為停休，則規則同國定假日
                else if (NonWorkingDay)
                    OverTime += DutyTime;
                else
                    OverTime += iDutyTimeConvert;

                // sam 20200908 職務：局長、副局長、主任祕書 這三個職務可超勤時數大於四小時
                ///轉換 : 督勤時數轉換為超勤時數
                int OverTimeG2 = 0;

                if (strMZ_OCCC == "局長" || strMZ_OCCC == "副局長" || strMZ_OCCC == "主任祕書" || strMZ_OCCC == "督察長")
                {
                    OverTimeG2 = (OverTimeG1 > 4 && !NonWorkingDay) ? OverTimeG1 : OverTimeG1; // 20150513 Joy 判斷督勤時數是否超過4，如有超過則為4
                }
                else
                {
                    OverTimeG2 = (OverTimeG1 > 4 && !NonWorkingDay) ? 4 : OverTimeG1; // 20150513 Joy 判斷督勤時數是否超過4，如有超過則為4
                }

                // 20150716 Neil 國定假日督勤為實報實銷
                OverTime += OverTimeG2;

                ///轉換 : 處理上班時數上限；若為國定假日則上班時數不轉換
                ///Joy 20150513 加入條件 若其他勤務包含值宿，則不受12小時限制
                if (prv_PassNightTime != 0) { }
                else if (!NonWorkingDay)
                {
                    DutyTime = (DutyTime > 12) ? 12 : DutyTime;
                }

                //if (DutyTime > 8)
                //{
                //    DutyTime = (DutyTime - 8) + IAmount;
                //}
                //else
                //{
                //    DutyTime += IAmount;
                //}
                #endregion

                TimeSpan total = new TimeSpan();
                if (strMZ_OCCC == "局長" || strMZ_OCCC == "副局長" || strMZ_OCCC == "主任祕書" || strMZ_OCCC == "督察長")
                {
                    //EX: ITEMG="2156-2200.2300-2400"
                    //EX: times={"2156-2200","2300-2400"}
                    string[] times = ITEMG.Split('.');
                    //EX: item="2156-2200"
                    foreach (var item in times)
                    {
                        //根據字串 HHmm-HHmm,計算時間差
                        TimeSpan? TimeDiff = (new LogicOvertime()).Get_TimeSpan_HHmm_HHmm(item);
                        if (TimeDiff == null)
                        {
                            continue;
                        }
                        //累加時間差
                        total += TimeDiff.Value;
                    }
                    //如果超過原本計算出來的加班時數,以實際加班時數為主
                    if (total.Hours > OverTime)
                    {
                        OverTime = total.Hours;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 處理超勤報表單日工時計算
        /// 20150520 Neil 其他勤務新增拖吊跟交通事故
        /// 20181106 sky 修改N內勤上班、L內部管理的統計規則
        /// 20190103 sky 修改V、W、X、Y、Z 大於8小時僅以8小時計算，未滿8小時則直接加
        /// 20190129 sky 刪除輪休(Y)以外的休假統計，將有輪休(Y)的上班時數轉成超勤
        /// 20190306 sky 補上各假別關於超勤統計的公式
        /// </summary>
        /// <param name="DutyTime">回傳 總上班時數，最多僅 12 小時</param>
        /// <param name="OverTime">回傳 總超勤時數，最多僅 4 小時</param>
        /// <param name="isHoliday">是否為國定假日</param>
        /// <returns>是否成功計算。若否則代表有例外狀況產生</returns>
        public Boolean calSingleDayOvertime(ref Int32 DutyTime, ref Int32 OverTime, Boolean isHoliday = false)
        {
            //不指定職稱,會遺失掉針對局長等職務對於督勤的特殊計算
            return calSingleDayOvertime_MZ_OCCC(ref DutyTime, ref OverTime, "", "", isHoliday);
        }

    }
}