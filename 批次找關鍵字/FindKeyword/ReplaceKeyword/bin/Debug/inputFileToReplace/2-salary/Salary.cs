using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.IO;
using NPOI.HSSF.Util;
using NPOI.Util;
using NPOI.HSSF.UserModel;
using LinqToExcel;
using Remotion;
using Remotion.Mixins.Definitions;

namespace TPPDDB._2_salary
{
    public class Salary
    {
        /// <summary>
        /// 代碼轉金額
        /// </summary>
        public static int getPay(string id, string table)
        {
            string result;
            string sql;
            List<SqlParameter> ops = new List<SqlParameter>();

            if (id == null) return 0;

            sql = string.Format("SELECT PAY FROM {0} WHERE \"ID\"=@ID", table);

            ops.Add(new SqlParameter("ID", id));

            result = o_DBFactory.ABC_toTest.GetValue(sql, ops);

            if (result == "")
                return 0;

            return int.Parse(result);
        }

        /// <summary>
        /// 取得人事資料中各發薪機關現有人數
        /// </summary>
        public static DataTable getPayadCounts()
        {
            string sql;

            sql = "SELECT PAY_AD, COUNT(*) Count FROM VW_ALL_BASE_DATA WHERE PAY_AD IS NOT NULL GROUP BY PAY_AD ORDER BY PAY_AD";

            return o_DBFactory.ABC_toTest.DataSelect(sql);
        }

        /// <summary>
        /// 取得人事資料未停職各發薪機關現有人數 2013/01/14
        /// </summary>        
        public static DataTable getPayadOffDutyCounts()
        {
            string sql;

            sql = "SELECT PAY_AD, COUNT(*) Count FROM VW_ALL_BASE_DATA WHERE PAY_AD IS NOT NULL AND (ISOFFDUTY ='否' or ISOFFDUTY is null) GROUP BY PAY_AD ORDER BY PAY_AD";

            return o_DBFactory.ABC_toTest.DataSelect(sql);
        }

        #region 代碼轉中文



        /// <summary>
        /// 健保狀態代碼轉中文
        /// </summary>
        public static string getMode(string id)
        {
            string sql;
            List<SqlParameter> ops = new List<SqlParameter>();

            sql = "SELECT TEXT FROM B_HEALTH_MODE WHERE ID=@ID";

            ops.Add(new SqlParameter("ID", id));

            return o_DBFactory.ABC_toTest.GetValue(sql, ops);
        }
        /// <summary>
        /// 健保關係代碼轉中文
        /// </summary>
        public static string getRelation(string id)
        {
            string sql;
            List<SqlParameter> ops = new List<SqlParameter>();

            sql = "SELECT TEXT FROM B_HEALTH_RELATION WHERE ID=@ID";

            ops.Add(new SqlParameter("ID", id));

            return o_DBFactory.ABC_toTest.GetValue(sql, ops);
        }

        #endregion

        #region 計算薪資資料

        /// <summary>
        /// 計算薪俸
        /// </summary>
        public static int getSalaryPay(string policeType, bool isPolice, string spt, string tempSpt)
        {
            string result = "";
            string sql = "";
            List<SqlParameter> ops = new List<SqlParameter>();

            // 警察人員，採用B_SALARY中的ORIGIN2薪級比對級距金額，其餘採用B_SALARY中的ORIGIN1俸點比對級距金額
            if (isPolice)
                sql = "SELECT PAY1 FROM B_SALARY WHERE NAME1=@NAME1 AND ORIGIN2=@ORIGIN";
            else
                sql = "SELECT PAY1 FROM B_SALARY WHERE NAME1=@NAME1 AND ORIGIN1=@ORIGIN";

            ops.Add(new SqlParameter("NAME1", policeType));

            // 有暫之俸點的話以暫之俸點(MZ_SPT1)計算；沒有的話才用俸點(MZ_SPT)
            if (tempSpt == "")
                ops.Add(new SqlParameter("ORIGIN", spt));
            else
                ops.Add(new SqlParameter("ORIGIN", tempSpt));

            result = o_DBFactory.ABC_toTest.GetValue(sql, ops);

            if (result == "")
                return 0;

            return int.Parse(result);
        }

        /// <summary>
        /// 計算主管加給
        /// </summary>
        public static int getBossPay(string extposSrank, string chief, string srank)
        {
            string result = "";
            string sql;
            List<SqlParameter> ops = new List<SqlParameter>();

            if (extposSrank == "" && chief == "")
                return 0;

            sql = "SELECT PAY FROM B_BOSS WHERE \"ID\"=@ID";

            // 有兼任主管以兼任的職等對照主管加給表，否則以本身職等對照主管加給表
            if (extposSrank != "")
                ops.Add(new SqlParameter("ID", extposSrank + "01D"));
            else
                ops.Add(new SqlParameter("ID", srank + "01D"));

            result = o_DBFactory.ABC_toTest.GetValue(sql, ops);

            if (result == "")
                return 0;

            return int.Parse(result);
        }

        /// <summary>
        /// 計算專業加給
        /// </summary>
        public static int getProfessPay(string srank, bool isCrimelab, string ahp_rank = "")
        {
            string result = "";
            string sql;
            List<SqlParameter> ops = new List<SqlParameter>();

            // 如果有技術加給，就是鑑識人員，鑑識人員的專業加給對照[B_PROFESS]資料表中的[PAY]欄位

            if (isCrimelab)
            {
                //原本有差異,現在沒有了,都抓PAY欄位
                sql = "SELECT PAY FROM B_PROFESS WHERE \"ID\"=@ID";
            }
            else
            {
                sql = "SELECT PAY FROM B_PROFESS WHERE \"ID\"=@ID";
                if (!string.IsNullOrEmpty(ahp_rank))
                    srank = ahp_rank;
            }

            ops.Add(new SqlParameter("ID", srank + "01D"));
            result = o_DBFactory.ABC_toTest.GetValue(sql, ops);

            if (result == "")
                return 0;

            return int.Parse(result);
        }

        /// <summary>
        /// 計算公/勞保費
        /// </summary>       
        /// <param name="policeType">類型</param>
        /// <param name="isPolice">是否為警察</param>
        /// <param name="spt">俸點</param>
        /// <param name="tempSpt">臨時俸點</param>
        /// <param name="INSURANCE_TYPE">公保要走新制還是舊制? 舊制D,新制F</param>
        /// <returns></returns>
        public static int getInsurancePay(string policeType, bool isPolice, string spt, string tempSpt, string INSURANCE_TYPE)
        {
            //// 任公職滿30年不用公保費
            //if (is30Years)
            //    return 0;

            string result = "";
            string sql = "";
            List<SqlParameter> ops = new List<SqlParameter>();

            // 警察人員，採用B_SALARY中的ORIGIN2薪級比對級距金額，其餘採用B_SALARY中的ORIGIN1俸點比對級距金額
            if (isPolice)
                sql = "SELECT INSURANCE_F FROM B_SALARY WHERE NAME1=@NAME1 AND ORIGIN2=@ORIGIN";
            else
                sql = "SELECT INSURANCE_F FROM B_SALARY WHERE NAME1=@NAME1 AND ORIGIN1=@ORIGIN";

            //原本的語法走新制,這邊判斷,是不是走舊制就好
            if (INSURANCE_TYPE == "D")
            {   //把欄位改掉
                sql = sql.Replace("INSURANCE_F", "INSURANCE_D");
            }

            ops.Add(new SqlParameter("NAME1", policeType));


            // 有暫之俸點的話以暫之俸點(MZ_SPT1)計算；沒有的話才用俸點(MZ_SPT)
            if (tempSpt == "")
                ops.Add(new SqlParameter("ORIGIN", spt));
            else
                ops.Add(new SqlParameter("ORIGIN", tempSpt));


            result = o_DBFactory.ABC_toTest.GetValue(sql, ops);



            if (result == "")
                return 0;

            return int.Parse(result);
        }

        /// <summary>
        /// 計算公/勞保費,多一個 殘疾減免百分比的數字
        /// </summary>   
        /// <param name="policeType">類型</param>
        /// <param name="isPolice">是否為警察</param>
        /// <param name="spt">俸點</param>
        /// <param name="tempSpt">臨時俸點</param>
        /// <param name="perno">殘疾減免百分比的數字,EX: 100,75,50,25 </param>
        /// <returns></returns>
        public static int getInsurancePay_withPerno(string policeType, bool isPolice, string spt, string tempSpt, decimal perno, string INSURANCE_TYPE)
        {
            int InsurancePay = getInsurancePay(policeType, isPolice, spt, tempSpt, INSURANCE_TYPE);
            //有殘疾減免的保費
            return Convert.ToInt32(Math.Floor(InsurancePay * perno / 100));
        }

        /// <summary>
        /// 計算健保年功俸(不含眷屬)
        /// </summary> 
        public static int getHealthPersonal(int intSalary, int intBoss, int intProfess, int intWorkp, int intTechnics, int intBonus, int intAdventive, int intFar, int intElectric, string insuranceType)
        {
            string strSQL;
            string strResult;
            int intResult;
            int intTotal = 0;

            intTotal = intSalary + intBoss + intProfess + intWorkp + intTechnics + intBonus + intAdventive + intFar + intElectric;


            strSQL = string.Format("SELECT INSURANCE FROM B_HEALTH_INSURANCE WHERE PAY1 <= {0} AND {0} <= PAY2", intTotal);

            strResult = o_DBFactory.ABC_toTest.vExecSQL(strSQL);
            int.TryParse(strResult, out intResult);

            return intResult;
        }

        /// <summary>
        /// 個人健保費異動時，更新健保加保資料
        /// </summary>
        public static DataTable getHealthInfo(DataTable healthInfo, int healthPersonal)
        {
            if (healthInfo == null)
                return null;

            // 個人健保費有異動時，加保的保費也要更新
            foreach (DataRow row in healthInfo.Rows)
            {
                if (int.Parse(row["Percent"].ToString()) > 100)
                    continue;

                //健保費無條件捨去小數點
                if (row[1].ToString() == "05")
                {
                    row["Cost"] = healthPersonal - 826;
                }
                else
                {
                    row["Cost"] = Math.Floor((float)healthPersonal * int.Parse(row["Percent"].ToString()) / 100);
                }
            }

            return healthInfo;
        }

        /// <summary>
        /// 計算健保費總額
        /// </summary> 
        /// <param name="healthInfo">眷屬保費資料的DataTable,Cost欄位是每個眷屬的健保費</param>
        /// <param name="healthPersonal">個人的健保費用</param>
        /// <returns></returns>
        public static int getHealth(DataTable healthInfo, int healthPersonal)
        {
            if (healthInfo == null)
                return healthPersonal;


            //擷取眷屬的保費 到list中
            List<int> list_Cost_forFamily = new List<int>();
            foreach (DataRow row in healthInfo.Select("1=1", "cost"))
            {
                int cost = int.Parse(row["Cost"].ToString());
                list_Cost_forFamily.Add(cost);
            }
            // 計算健保費總額，加保人數超過3人以最便宜的3人保費計算
            //依照金額由小到大排序
            list_Cost_forFamily.OrderBy(x => x);
            //抓前三名
            list_Cost_forFamily = list_Cost_forFamily.Take(3).ToList();
            //將 個人的健保費用 加上 最便宜的3人眷屬保費,作為 健保費總額
            return healthPersonal + list_Cost_forFamily.Sum();


            //int health;
            //// 計算健保費總額，加保人數超過3人以最便宜的3人保費計算
            //health = healthPersonal;
            //int i = 0;
            //foreach (DataRow row in healthInfo.Select("1=1", "cost"))
            //{
            //    if (i == 3)
            //        break;
            //    health += int.Parse(row["Cost"].ToString());
            //    i++;
            //}

            //// 任公職滿30年不用算本人的健保費
            ////if (is30Years)
            ////    health -= healthPersonal;

            //return health;
        }

        /// <summary>
        /// 計算退撫金費
        /// </summary> 
        public static int getConcur3(string policeType, bool isPolice, string spt, string tempSpt)
        {
            string result = "";
            string sql = "";
            List<SqlParameter> ops = new List<SqlParameter>();

            // 警察人員，採用B_SALARY中的ORIGIN2薪級比對級距金額，其餘採用B_SALARY中的ORIGIN1俸點比對級距金額
            if (isPolice)
                sql = "SELECT CONCUR3 FROM B_SALARY WHERE NAME1=@NAME1 AND ORIGIN2=@ORIGIN";
            else
                sql = "SELECT CONCUR3 FROM B_SALARY WHERE NAME1=@NAME1 AND ORIGIN1=@ORIGIN";

            ops.Add(new SqlParameter("NAME1", policeType));

            // 有暫之俸點的話以暫之俸點(MZ_SPT1)計算；沒有的話才用俸點(MZ_SPT)
            if (tempSpt == "")
                ops.Add(new SqlParameter("ORIGIN", spt));
            else
                ops.Add(new SqlParameter("ORIGIN", tempSpt));

            result = o_DBFactory.ABC_toTest.GetValue(sql, ops);

            if (result == "")
                return 0;

            return int.Parse(result);
        }

        /// <summary>
        /// 計算繁重加給
        /// </summary>
        /// <param name="police">人員資料</param>
        /// <returns></returns>
        public static int getElectric(Police police)
        {
            int electric = 0;

            if (police.isOffduty)
            {
                return 0;
            }

            if (police.isCrimelab)
            {
                return 0;
            }

            if (!police.ADLBASE_isPolice)
            {
                return 0;
            }

            if (police.TRAINING != null)
            {
                if (police.TRAINING.ToUpper().Equals("Y"))
                {
                    return 0;
                }
            }

            if (police.srank.ToUpper().StartsWith("P"))
            {
                return 3880;
            }

            string sql = @"SELECT ELECTRIC FROM A_ELECTRIC WHERE MZ_EXAD = @MZ_EXAD AND MZ_EXUNIT = @MZ_EXUNIT";

            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("MZ_EXAD", police.exad));
            parameters.Add(new SqlParameter("MZ_EXUNIT", police.exunit));

            string result = o_DBFactory.ABC_toTest.Get_First_Field(sql, parameters);

            int.TryParse(result, out electric);

            return electric;
        }
        /*
         原本警勤加給的參數,定義我也不清楚,
         原本是魔術數字,這邊先抽離出來統一修改

         2023-06-19 請協助更改程式數值：
        原本→更新
        6745→7760
        8435→9700
        13496→15520
        7590→8730
        10792→12416
             */
        public static int WorkP_6745 = 7760;
        public static int WorkP_8435 = 9700;
        /// <summary>
        /// 偵查隊的加給
        /// </summary>
        public static int WorkP_13496 = 15520;
        public static int WorkP_7590 = 8730;
        /// <summary>
        /// 刑警的加給,非偵查隊
        /// </summary>
        public static int WorkP_10792 = 12416;
        /// <summary>
        /// 計算警勤加給
        /// </summary>
        /// <param name="police">人員資料</param>
        /// <returns>WORKPPY 警勤加給</returns>
        public static int getWORKPPY(Police police)
        {
            /*
             邏輯說明:
             (優先序由上到下)
             -停職沒有加給
             -鑑識人員沒有加給
             -受訓中:WorkP_6745
             -非警職沒有加給
             -會計人員,srank P開頭:WorkP_6745
             -編制機關或現服機關為局本部,
             */


            // sam.hsu 20201126 修改 警勤加給 急件
            int WORKPPY = 0;

            if (police.isOffduty)
            {
                return 0;
            }

            if (police.isCrimelab)
            {
                return 0;
            }

            if (police.TRAINING != null)
            {
                if (police.TRAINING.ToUpper().Equals("Y"))
                {
                    return WorkP_6745;
                }
            }

            #region "是否為警職"
            if (!police.ADLBASE_isPolice)
            {
                return 0;
            }
            #endregion


            #region "人事會計警職人員"
            if (police.srank.StartsWith("P"))
            {
                return WorkP_6745;
            }

            #endregion

            #region "局本部"
            if (police.ad.Equals("382130000C") || police.exad.Equals("382130000C"))
            {
                if (police.exunit.ToUpper().Equals("DH90"))
                {
                    return WorkP_8435;
                }
                else
                {
                    return WorkP_6745;
                }
            }
            #endregion

            #region "保安警察大隊"                 
            if (police.ad.Equals("382130100C") || police.exad.Equals("382130100C"))
            {
                if (police.exunit.Equals("0173") || police.exunit.Equals("0174") ||
                    police.exunit.Equals("0175") || police.exunit.Equals("DE05"))
                {
                    return WorkP_8435;
                }
                else
                {
                    return WorkP_6745;
                }
            }

            #endregion

            #region "刑事人員外勤人員"
            //如果是刑事警察大隊
            if (police.ad.Equals("382130200C"))
            {
                //如果是 代理所長副所長
                if (police.extpos=="01"|| police.extpos == "02")
                {
                    return WorkP_8435;
                }

                //如果是偵查隊
                if (police.exunit.Equals("0104") || police.exunit.Equals("0105") ||
                    police.exunit.Equals("0106") || police.exunit.Equals("0107") ||
                    police.exunit.Equals("0108") || police.exunit.Equals("0109") ||
                    police.exunit.Equals("0110") || police.exunit.Equals("0111") ||
                    police.exunit.Equals("0112") || police.exunit.Equals("0460"))
                {
                    return WorkP_13496;
                }
                //如果是偵查隊
                else if (police.exunit.StartsWith("DH"))
                {
                    return WorkP_13496;
                }
                else if (police.ad.Equals("382130100C"))
                {
                    return WorkP_6745;
                }
                else
                {
                    return WorkP_10792;
                }
            }

            #endregion

            #region "交通警察大隊"
            if (police.exad.Equals("382130300C"))
            {
                if (police.exunit.Equals("0302") || police.exunit.Equals("DG19"))
                {
                    return WorkP_8435;
                }
                else
                {
                    return WorkP_6745;
                }
            }
            #endregion

            #region "少年警察隊"
            if (police.ad.Equals("382135000C") || police.exad.Equals("382135000C"))
            {
                return WorkP_13496;
            }

            #endregion

            #region "婦幼警察隊"
            if (police.ad.Equals("382130500C") || police.exad.Equals("382130500C"))
            {
                //20210127 - 多 偵查領刑事人員加給 的判斷
                if (police.unit.Equals("0367") && (police.occc == "1080" || police.occc == "1431" || police.occc == "1432" || police.occc == "1495" || police.occc == "1455"))
                {
                    return WorkP_13496;
                }
                else
                {
                    return WorkP_8435;
                }
            }
            #endregion

            #region "各分局偵察隊長"
            if ((police.unit.Equals("0360") || police.exunit.Equals("0360")) && police.occc.Equals("1413"))
            {
                return WorkP_13496;
            }
            #endregion

            #region "其他各分局"

            long startAD = 382132000;
            long endAD = 382133700;
            long pAD = 0;
            long.TryParse(police.ad.Replace("C", ""), out pAD);
            long eAD = 0;
            long.TryParse(police.exad.Replace("C", ""), out eAD);

            if ((pAD >= startAD && pAD <= endAD) || (eAD >= startAD && eAD <= endAD))
            {
                return WorkP_8435;
            }

            #endregion


            return WORKPPY;
        }

        /// <summary>
        /// 計算偏遠(地域加給)
        /// </summary>
        /// <param name="police"></param>
        /// <returns></returns>
        public static int getFARPAY(Police police)
        {
            double year = 0;
            double year_per = 0.0;
            double FARPAY = 0;
            /*
    派出所有變動
 忠治(D60M)→桶壁派出所(D60S)
 信賢(D60N)→哪哮派出所(D60T)
 福山(D609)→德拉楠派出所(D60U)
 */
            Dictionary<string, double> cMoney = new Dictionary<string, double>();
            cMoney.Add("D609", 4120);//福山(D609)
            cMoney.Add("D60H", 4120);
            cMoney.Add("D90K", 4120);
            cMoney.Add("D60U", 4120);//德拉楠派出所(D60U)

            cMoney.Add("D60E", 3090);
            cMoney.Add("D60G", 3090);
            cMoney.Add("D60I", 3090);
            cMoney.Add("D60J", 3090);
            cMoney.Add("D60N", 3090);//信賢(D60N)
            cMoney.Add("D60K", 3090);
            cMoney.Add("D60L", 3090);
            cMoney.Add("D60M", 3090);//忠治(D60M)
            cMoney.Add("D90F", 3090);
            cMoney.Add("D60S", 3090);//桶壁派出所
            cMoney.Add("D60T", 3090);//哪哮派出所


            if (!cMoney.ContainsKey(police.exunit))
            {
                return 0;
            }

            double.TryParse(police.FYEAR, out year);
            if (year != 0)
            {
                year_per = (year * 0.02);

                if ((int)cMoney[police.exunit] == 3090)
                {
                    if (year_per >= 0.10)
                    {
                        year_per = 0.10;
                    }
                }
                else if ((int)cMoney[police.exunit] == 4120)
                {
                    if (year_per >= 0.20)
                    {
                        year_per = 0.20;
                    }
                }
            }

            return (int)(Math.Round((double)police.salary * year_per, 0, MidpointRounding.AwayFromZero) +
                          (int)cMoney[police.exunit]);
        }

        /// <summary>
        /// 計算法院扣款
        /// </summary>         
        public static int getExtra01(int totalPay)
        {
            return totalPay / 3;
        }

        /// <summary>
        /// 同時撈薪俸,公保,退撫金
        /// 回傳List型別:[薪俸,公保,退撫金]
        /// 
        /// Allen:不是很理解為何要這樣抓? 如果是想要節省效能,也應該回傳資料物件才是?
        /// </summary>
        /// <param name="policeType"></param>
        /// <param name="isPolice"></param>
        /// <param name="spt"></param>
        /// <param name="tempSpt"></param>
        /// <returns></returns>
        public static List<int> getSalaryPay__InsurancePay_Concur3(string policeType, bool isPolice, string spt, string tempSpt, Police police)
        {
            List<int> pay = new List<int>();

            DataTable result = new DataTable();
            string sql = "";
            List<SqlParameter> ops = new List<SqlParameter>();

            // 警察人員，採用B_SALARY中的ORIGIN2薪級比對級距金額，其餘採用B_SALARY中的ORIGIN1俸點比對級距金額
            if (isPolice)
            {
                if (police.srank.ToUpper().StartsWith("P"))
                {
                    sql = "SELECT PAY1,CONCUR3 FROM B_SALARY WHERE NAME1=@NAME1 AND ORIGIN1=@ORIGIN";

                }
                else
                {
                    sql = "SELECT PAY1,CONCUR3 FROM B_SALARY WHERE NAME1=@NAME1 AND ORIGIN2=@ORIGIN";
                }
            }
            else
            {
                sql = "SELECT PAY1,CONCUR3 FROM B_SALARY WHERE NAME1=@NAME1 AND ORIGIN1=@ORIGIN";
            }


            ops.Add(new SqlParameter("NAME1", policeType));

            // 有暫之俸點的話以暫之俸點(MZ_SPT1)計算；沒有的話才用俸點(MZ_SPT)
            if (tempSpt == "")
            {
                spt = spt.PadLeft(4, '0');//靠右對齊 四碼補零
                ops.Add(new SqlParameter("ORIGIN", spt));
            }
            else
            {
                tempSpt = tempSpt.PadLeft(4, '0');//靠右對齊 四碼補零
                ops.Add(new SqlParameter("ORIGIN", tempSpt));
            }

            result = o_DBFactory.ABC_toTest.GetDataTable(sql, ops);

            for (int i = 0; i < result.Columns.Count; i++)
            {
                //20150327 因為有人俸點級距找不出資料列.進入迴圈會掛掉
                if (result.Rows.Count > 0)
                {
                    if (result.Rows[0][i].ToString() == "")
                        pay.Add(0);
                    else
                        pay.Add(int.Parse(result.Rows[0][i].ToString()));
                }
                else
                {
                    pay.Add(0);
                }
            }



            return pay;
        }
        #endregion

        /// <summary>
        /// 把資料中的指定欄位的特定資料移到資料最前面
        /// </summary>         
        public static void movetoFirst(ref DataTable dt, int index, string target)
        {
            List<DataRow> temp = new List<DataRow>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i][index].ToString() == target)
                    temp.Add(dt.Rows[i]);
            }

            foreach (DataRow row in temp)
            {
                DataRow dr = dt.NewRow();

                foreach (DataColumn col in dt.Columns)
                {
                    dr[col] = row[col];
                }
                dt.Rows.Remove(row);
                dt.Rows.InsertAt(dr, 0);
            }
        }

        /// <summary>
        /// 把資料中的指定欄位的特定資料移到資料最前面
        /// </summary>         
        public static void movetoFirst_new(ref DataTable dt, int index, string target)
        {
            List<DataRow> temp = new List<DataRow>();

            for (int i = dt.Rows.Count - 1; i >= 0; i--)
            {
                if (dt.Rows[i][index].ToString() == target)
                    temp.Add(dt.Rows[i]);
            }

            foreach (DataRow row in temp)
            {
                DataRow dr = dt.NewRow();

                foreach (DataColumn col in dt.Columns)
                {
                    dr[col] = row[col];
                }
                dt.Rows.Remove(row);
                dt.Rows.InsertAt(dr, 0);
            }
        }

        /// <summary>
        /// 把資料中的指定欄位的特定資料移到資料最後面
        /// </summary>
        public static void movetoLast(ref DataTable dt, int index, string target)
        {
            List<DataRow> temp = new List<DataRow>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i][index].ToString() == target)
                    temp.Add(dt.Rows[i]);
            }

            foreach (DataRow row in temp)
            {
                DataRow dr = dt.NewRow();

                foreach (DataColumn col in dt.Columns)
                {
                    dr[col] = row[col];
                }
                dt.Rows.Remove(row);
                dt.Rows.Add(dr);
            }
        }


        /// <summary>
        /// // 四捨五入
        /// </summary>
        public static int round(double value)
        {
            return (int)Math.Round(value, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// 關帳
        /// </summary>
        /// <param name="payad">發薪機關</param>
        /// <param name="amonth">資料年月</param>
        public static void lockDB(string payad, string amonth)
        {
            string sql;
            List<SqlParameter> ps = new List<SqlParameter>();

            sql = "UPDATE B_MONTHPAY_MAIN SET LOCKDB='Y' WHERE PAY_AD=@PAYAD AND AMONTH=@AMONTH";
            ps.Add(new SqlParameter("PAYAD", payad));
            ps.Add(new SqlParameter("AMONTH", amonth));
            o_DBFactory.ABC_toTest.Edit_Data(sql, ps);
        }

        /// <summary>
        /// 依每頁筆數加入頁次
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="rowsPerPage">每頁筆數</param>
        /// <param name="group">群組欄位</param>
        public static DataTable addPageNumber(DataTable dtSource, int rowsPerPage, string group)
        {
            DataTable dt = dtSource;
            string gp = "";
            int count = 0;
            int page = 0;

            dt.Columns.Add("pageNum");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (group != null)
                {
                    count++;

                    if (dt.Rows[i][group].ToString() != gp)
                    {
                        gp = dt.Rows[i][group].ToString();
                        count = 0;
                        page++;
                        dt.Rows[i]["pageNum"] = page;
                        continue;
                    }
                    if (count % rowsPerPage == 0)
                        page++;
                }
                else
                {
                    if (i % rowsPerPage == 0)
                        page++;
                }

                dt.Rows[i]["pageNum"] = page;
            }

            return dt;
        }
    }

    public static partial class IConvert
    {
        /// <summary>
        /// 將 IEnumerable 轉為 DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> collection)
        {
            var dtReturn = new DataTable();

            // column names
            var oProps = typeof(T).GetProperties();
            foreach (var pi in oProps)
            {
                var colType = pi.PropertyType;
                if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                {
                    colType = colType.GetGenericArguments()[0];
                }
                dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
            }

            // Could add a check to verify that there is an element 0
            foreach (var rec in collection)
            {
                var dr = dtReturn.NewRow();
                foreach (var pi in oProps)
                {
                    dr[pi.Name] = pi.GetValue(rec, null) ?? DBNull.Value;
                }
                dtReturn.Rows.Add(dr);
            }

            return (dtReturn);
        }
    }

    public partial class Excel
    {
        //param: 檔案路徑, 工作表名稱
        public static System.Data.DataTable getDataTable(string xlsPath, string sheetName)
        {
            DataTable dt = new DataTable();
            ExcelQueryFactory excel = new ExcelQueryFactory(xlsPath);
            int WorksheetCount = excel.GetWorksheetNames().Count();
            var SheetName = excel.GetWorksheetNames();
            string[] sName = new string[WorksheetCount];

            var excelQuery = from s in excel.Worksheet<Row>(0)
                             select s;

            foreach (var item in excelQuery.First().ColumnNames)
            {
                dt.Columns.Add(item.ToString());
            }
            foreach (var row in excelQuery)
            {
                DataRow dr = dt.NewRow();
                foreach (DataColumn col in dt.Columns)
                {
                    dr[col] = row[col.ColumnName];
                }
                dt.Rows.Add(dr);
            }

            return dt;
        }

        #region DT2EXCEL

        public static void Dt2Excel(DataTable SourceTable, string FileName)
        {
            MemoryStream ms = RenderDataTableToExcel(SourceTable) as MemoryStream;
            //FileStream fs = new FileStream(FileName, FileMode.Create, FileAccess.Write);
            byte[] data = ms.ToArray();

            //fs.Write(data, 0, data.Length);
            //fs.Flush();
            //fs.Close();
            if (HttpContext.Current.Request.Browser.Browser == "IE")
                FileName = HttpContext.Current.Server.UrlPathEncode(FileName);

            HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xls", FileName));
            // 設定強制下載標頭。
            FileName = HttpUtility.UrlEncode(FileName, System.Text.Encoding.UTF8);
            // 輸出檔案。
            HttpContext.Current.Response.BinaryWrite(ms.ToArray());

            ms.Close();
            ms.Dispose();
            data = null;
            ms = null;
            //fs = null;
        }

        public static Stream RenderDataTableToExcel(DataTable SourceTable)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            MemoryStream ms = new MemoryStream();
            HSSFSheet sheet = workbook.CreateSheet();
            HSSFRow headerRow = sheet.CreateRow(0);

            // handling header.
            foreach (DataColumn column in SourceTable.Columns)
                headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);

            // handling value.
            int rowIndex = 1;

            foreach (DataRow row in SourceTable.Rows)
            {
                HSSFRow dataRow = sheet.CreateRow(rowIndex);

                foreach (DataColumn column in SourceTable.Columns)
                {
                    dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                }

                rowIndex++;
            }

            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;

            sheet = null;
            headerRow = null;
            workbook = null;

            return ms;
        }

        public static void Dt2Excel(DataTable SourceTable, string FileName, string title)
        {
            MemoryStream ms = RenderDataTableToExcel(SourceTable, title) as MemoryStream;
            //FileStream fs = new FileStream(FileName, FileMode.Create, FileAccess.Write);
            byte[] data = ms.ToArray();

            //fs.Write(data, 0, data.Length);
            //fs.Flush();
            //fs.Close();
            if (HttpContext.Current.Request.Browser.Browser == "IE")
                FileName = HttpContext.Current.Server.UrlPathEncode(FileName);

            HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xls", FileName));
            // 設定強制下載標頭。
            FileName = HttpUtility.UrlEncode(FileName, System.Text.Encoding.UTF8);
            // 輸出檔案。
            HttpContext.Current.Response.BinaryWrite(ms.ToArray());

            ms.Close();
            ms.Dispose();
            data = null;
            ms = null;
            //fs = null;
        }

        public static Stream RenderDataTableToExcel(DataTable SourceTable, string title)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            MemoryStream ms = new MemoryStream();
            HSSFSheet sheet = workbook.CreateSheet();
            HSSFRow titleRow = sheet.CreateRow(0);
            HSSFRow headerRow = sheet.CreateRow(1);

            //title
            titleRow.CreateCell(0).SetCellValue(title);

            // handling header.
            foreach (DataColumn column in SourceTable.Columns)
                headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);

            // handling value.
            int rowIndex = 2;

            foreach (DataRow row in SourceTable.Rows)
            {
                HSSFRow dataRow = sheet.CreateRow(rowIndex);

                foreach (DataColumn column in SourceTable.Columns)
                {
                    dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                }

                rowIndex++;
            }

            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;

            sheet = null;
            headerRow = null;
            workbook = null;

            return ms;
        }

        #endregion
    }
}
