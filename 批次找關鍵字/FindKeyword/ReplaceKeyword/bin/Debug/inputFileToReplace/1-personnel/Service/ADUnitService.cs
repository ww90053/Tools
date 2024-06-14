using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TPPDDB.Model.Const;

namespace TPPDDB.Service
{
    public class ADUnitService
    {
        /// <summary>
        /// 取得資料,新北市警察局機關清單
        /// </summary>
        /// <returns></returns>
        public static DataTable GetDataSet_AD()
        {
            return o_DBFactory.ABC_toTest.Create_Table("  (SELECT  MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE = '04' AND MZ_KCODE LIKE '38213%') ", "GET");
        }

        /// <summary>
        /// 取得資料,新北市警察局機關清單,再根據指定的機關代碼,篩選出可以存取的機關
        /// (其實就相當於撈出來之後再篩掉可以用的)
        /// </summary>
        /// <param name="list_AD"></param>
        /// <returns></returns>
        public static DataTable GetDataSet_AD(List<String> list_AD)
        {
            var dt = GetDataSet_AD();
            //特殊:如果三個單位裡面有中和分局 要帶出中和一&中和二
            if (list_AD.Where(x => x.Contains("382133600C")).Count() > 0)
            {
                list_AD.AddRange(new List<string>() { "382133400C", "382133500C" });
            }
            // 使用 LINQ 過濾符合 ID 的資料
            var filteredRows = from DataRow row in dt.Rows
                               where list_AD.Contains((string)row["MZ_KCODE"])
                               select row;

            // 複製出新的 DataTable ,把過濾後的資料加入
            DataTable filteredDataTable = dt.Clone();
            foreach (DataRow row in filteredRows)
            {
                filteredDataTable.ImportRow(row);
            }

            return filteredDataTable;
        }

        /// <summary>
        /// 自動根據Session中的設定值,判斷使用者目前所屬單位有哪些
        /// </summary>
        /// <returns></returns>
        public static List<String> GetList_AD_BySession()
        {
            //帶入三個單位資料 編制機關 現服機關 發薪機關 
            List<String> Depts = new List<String>();
            var Session = HttpContext.Current.Session;
            if (Session[ConstSession.AD] != null)
            {
                Depts.Add(Session[ConstSession.AD].ToString());
            }
            if (Session[ConstSession.EXAD] != null)
            {
                Depts.Add(Session[ConstSession.EXAD].ToString());
            }
            if (Session[ConstSession.ADPPAY_AD] != null)
            {
                Depts.Add(Session[ConstSession.ADPPAY_AD].ToString());
            }
            return Depts;
        }

        /// <summary>
        /// 自動根據Session中的設定值,判斷使用者目前是否在新中和分局?
        /// </summary>
        /// <returns></returns>
        public static bool IsNewZhonghe_BySession()
        {
            //帶入三個單位資料 編制機關 現服機關 發薪機關 
            List<String> Depts = GetList_AD_BySession();
            //如果三個單位裡面有中和分局 要帶出中和一&中和二
            if (Depts.Where(x => x.Contains("382133600C")).Count() > 0)
            {
                return true;
            }
            return false;
        }

        public bool UNITTEST(ref string msg)
        {
            try
            {
                var dt = GetDataSet_AD();
            }
            catch (Exception ex)
            {
                msg = ex.StackTrace.ToString() + ex.ToString();
                return false;
                throw ex;
            }

            try
            {
                List<String> list_AD = new List<string>() { "382133600C" };
                var dt2 = ADUnitService.GetDataSet_AD(list_AD);
            }
            catch (Exception ex)
            {
                msg = ex.StackTrace.ToString() + ex.ToString();
                return false;
                throw ex;
            }
            //通過測試
            return true;
        }
    }
}