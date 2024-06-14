using NPOI.HSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TPPDDB.Helpers;
using TPPDDB.Model.Const;
using TPPDDB.Service;


namespace TPPDDB._3_forleave
{
    public partial class C_OVERTIME_OVERWORK_DETAIL_RPT : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //檢查權限
                C.check_power();

                //依群組權限建立機關選單內容
                chk_TPMGroup();

                //預設給予年月,上個月
                DateTime BEG = DateTime.Now.AddMonths(-1);
                this.TextBox_YEARMONTH.Text = (BEG.Year - 1911).ToString() + BEG.Month.ToString("D2");


            }
        }

        /// <summary>
        /// 依群組權限建立機關選單內容
        /// </summary>
        protected void chk_TPMGroup()
        {
            if (MZ_PowerService.isOK() == false)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click",
                       "alert('此人員查無超勤之權限資料，無法使用此功能'); window.location.href='/10-knowledge/J_Login_after.aspx?TPM_FION=-1';", true);

            }

            DataTable dt;
            //是否可以抓取所有機關?
            if (MZ_PowerService.isAll_AD())
            {
                dt = ADUnitService.GetDataSet_AD();
            }
            else
            {
                //帶入三個機關資料 編制機關 現服機關 發薪機關 
                List<String> Depts = ADUnitService.GetList_AD_BySession();
                dt = ADUnitService.GetDataSet_AD(Depts);
            }
            //綁定 機關選單
            DropDownList_MZ_AD.DataSource = dt;
            DropDownList_MZ_AD.DataBind();
            //權限不可抓取 全部單位 ,且非新中和
            if (MZ_PowerService.isAll_UNIT() == false && !ADUnitService.IsNewZhonghe_BySession())
            {
                //機關單位都先鎖定住
                DropDownList_MZ_AD.Enabled = false;
                DropDownList_MZ_UNIT.Enabled = false;
            }


            try
            {
                //2023-05-02 以 現服機關單位 MZ_EXAD 跟 MZ_EXUNIT 為主
                //現服機關 MZ_EXAD
                string EXAD = Session[ConstSession.EXAD].ToString();
                DropDownList_MZ_AD.SelectedValue = EXAD;
                ////現服單位 MZ_EXUNIT
                string EXUNIT = Session[ConstSession.EXUNIT].ToString();
                ////綁定之
                DropDownList_MZ_UNIT.SelectedValue = EXUNIT;
            }
            catch (Exception)
            {
                DropDownList_MZ_AD.SelectedValue = "";
            }

            //判斷當前權限等級
            //設定機關下拉選單
            //設定單位下拉選單
            o_A_KTYPE.AddUNIT(DropDownList_MZ_AD.SelectedValue
                , this.DropDownList_MZ_UNIT
                , "全部");
        }


        /// <summary>
        /// 當機關 下拉選單變動時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DropDownList_AD_SelectedIndexChanged(object sender, EventArgs e)
        {
            //設定單位下拉選單
            o_A_KTYPE.AddUNIT(DropDownList_MZ_AD.SelectedValue
                , this.DropDownList_MZ_UNIT
                , "全部");
        }

        /// <summary>
        /// 列印按鈕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btPrintExcel_Click(object sender, EventArgs e)
        {
            //判斷機關是否選擇
            if (DropDownList_MZ_AD.SelectedIndex == 0 || DropDownList_MZ_AD.SelectedValue == "")
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('機關未選擇');", true);
                return;
            }
            //判斷民國年日期正確
            string YEARMONTH = this.TextBox_YEARMONTH.Text;
            if (ForDateTime.Check_TWYearMonth(YEARMONTH) == false)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('年月格式輸入錯誤');", true);
                return;
            }

            //抓取目前要找的機關單位
            string AD = DropDownList_MZ_AD.SelectedItem.Value;
            string UNIT = DropDownList_MZ_UNIT.SelectedItem.Value;
            //======查詢資料
            string YEAR = YEARMONTH.Substring(0, 3);
            string MONTH = YEARMONTH.Substring(3, 2);
            var dt = GetDataTable(AD, UNIT, YEAR, MONTH);

            //轉換報表需要的參數
            string ADName = DropDownList_MZ_AD.SelectedItem.Text;
            //抓取單位名稱
            string UnitName = DropDownList_MZ_UNIT.SelectedItem.Text;
            //特殊:單位名稱如果是全部,則不顯示名稱
            UnitName = UnitName.Replace("全部", "");

            //產生Excel報表
            var workbook = GetWorkbook(dt, ADName, UnitName,  YEAR, MONTH);
            //匯出
            OfficeHelpers.ExcelHelpers.Response_Workbook(workbook, "加班情形統計表");

        }

        /// <summary>
        /// 取消按鈕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btCancel_Click(object sender, EventArgs e)
        {
            TextBox_YEARMONTH.Text = string.Empty;
            //TextBox_MZ_ID.Text = string.Empty;
        }

        /// <summary>
        /// 讀取資料
        /// 
        /// </summary>
        /// <param name="MZ_EXAD">機關代碼</param>
        /// <param name="MZ_EXUNIT">單位代碼</param>
        /// <param name="YEARMONTH">民國年月,EX: 11201 </param>
        /// <returns></returns>
        public DataTable GetDataTable(string MZ_EXAD, string MZ_EXUNIT, string YEAR, string Month)
        {
            //預計的查詢結果
            /*
UNIT_NAME：後勤科
MZ_NAME：李OO
MZ_ID：P123456789
TOTALHOUR1：55
TOTALHOUR2：69
TOTALHOUR3：60

             */

            string SQL = @"
                
/*
報表大概分成:
-全月加班總時數
    CTE01_LV1: 每月員警當月的加班時數明細,包括業務加班和超勤加班,兩種都個別一筆資料
    CTE01_LV2: 根據上述CTE,再統計出每月員警當月的加班時數統計,將業務加班和超勤加班兩份月結算資料進行加總成同一筆資料
    CTE01_LV3: 根據上述CTE,再統計出每個機關單位當月的加班人數分布統計
-每日(含放假日及例假日)加班時數連同法定辦公時數
    CTE02_LV1: 員警每天的加班時數明細,包括業務加班和超勤加班(又分內外勤),每種都個別一筆資料
    CTE02_LV2: 根據上述CTE,再統計出員警每天的加班時數統計,將上述多種加班資料進行每天的加總
    CTE02_LV3: 根據上述CTE,再統計出每個員警最高記錄曾經一天上班多少小時?
    CTE02_LV4: 根據上述CTE,再統計出每個機關單位出現最高上班時間的人數分布統計
*/

WITH
CTE01_LV1 AS(
        /*CTE01_LV1: 每月員警當月的加班時數明細,包括業務加班和超勤加班,兩種都個別一筆資料*/
        /*員警ID,單位ID,當月總業務加班工時 */
        --業務加班月結時數
        select MZ_ID,MZ_EXAD,MZ_EXUNIT,MZ_BUDGET_HOUR
        --,A.* 
        from c_overtimeMONTH_HOUR A /*加班月報表*/
        where MZ_YEAR = @YEAR --指定年 EX: 111
        and MZ_Month = @Month --指定月 EX: 09
        and MZ_EXAD= @MZ_EXAD -- EX:'382132200C'
        --邏輯條件:單位代碼 and MZ_EXUNIT= @MZ_EXUNIT -- EX:'0065'
        and MZ_VERIFY='Y' --限定審核通過的資料才算數
        union all
        --超勤加班月結時數
        select MZ_ID,MZ_EXAD,MZ_EXUNIT,MZ_BUDGET_HOUR * 60 AS MZ_BUDGET_HOUR /*超勤紀錄的是小時,這邊讓他變成分鐘*/
        --,A.* 
        from c_dutymonthovertime_hour A /*加班月報表*/
        where MZ_YEAR = @YEAR --指定年 EX: 111
        and MZ_Month = @Month --指定月 EX: 09
        and MZ_EXAD= @MZ_EXAD -- EX:'382132200C'
        --邏輯條件:單位代碼 and MZ_EXUNIT= @MZ_EXUNIT -- EX:'0065'
        and MZ_VERIFY='Y' --限定審核通過的資料才算數
),
CTE01_LV2 AS
(
        /*CTE01_LV2: 根據上述CTE,再統計出每月員警當月的加班時數統計,將業務加班和超勤加班兩份月結算資料進行加總成同一筆資料*/
        /*    
        預計抓出欄位:MZ_KCHI, MZ_KCODE, MZ_ID, MZ_EXUNIT, TOTAL_MZ_BUDGET_HOUR
        */
        select 
        MZ_ID,MZ_EXAD,MZ_EXUNIT
        ,sum( MZ_BUDGET_HOUR  ) as total_MZ_BUDGET_HOUR
        FROM  CTE01_LV1 T1 
        group by MZ_ID,MZ_EXAD,MZ_EXUNIT
),
CTE01_LV3 AS
(
        /*CTE01_LV3: 根據上述CTE,再統計出每個機關單位當月的加班人數分布統計*/
        /*    
        預計抓出欄位:MZ_KCHI, MZ_KCODE, MZ_ID, MZ_EXUNIT, TOTAL_MZ_BUDGET_HOUR
        */
        select 
        MZ_EXAD,MZ_EXUNIT
        --20小時以下之人數
        ,sum(case when total_MZ_BUDGET_HOUR < 60*21 then 1 else 0 end) as Under20 
        --21小時以上未滿45小時之人數
        ,sum(case when total_MZ_BUDGET_HOUR >= 60*21 and total_MZ_BUDGET_HOUR < 60*45 then 1 else 0 end) as Over20To45
        --45小時以上未滿61小時之人數
        ,sum(case when total_MZ_BUDGET_HOUR >= 60*45 and total_MZ_BUDGET_HOUR < 60*61 then 1 else 0 end) as Over45To61
        --61小時以上之人數
        ,sum(case when total_MZ_BUDGET_HOUR >= 60*61 then 1 else 0 end) as Over61
        FROM  CTE01_LV2 T1 
        group by MZ_EXAD,MZ_EXUNIT
)
,CTE02_LV1 AS
(   /*CTE02_LV1: 員警每天的加班時數明細,包括業務加班和超勤加班(又分內外勤),每種都個別一筆資料*/
    select MZ_EXAD/*機關*/
    ,MZ_EXUNIT/*機關單位*/
    ,OVER_DAY/*上班年月日*/
    ,MZ_ID/*員警ID*/
    ,OVER_TOTAL/*加班時數*/
    , '業務加班' as OTYPE /*業務/超勤*/
    /*H:假日/D:工作日,對應不到的話就當作工作日*/
    ,CASE NVL(H.MZ_DAYTYPE,'D')
        WHEN 'D' THEN '1工作日' 
        WHEN 'H' THEN '2假日' 
        ELSE '0其他' END as DTYPE 
    FROM C_OVERTIME_BASE O
    --新開的上班日判定資料表
    LEFT JOIN C_DUTYHOLIDAY_V2 H on O.OVER_DAY=H.MZ_DATE
    --篩選年月
    where OVER_DAY like @YEAR + @Month+'%'
    --篩選機關
    AND MZ_EXAD=@MZ_EXAD
    --邏輯條件:單位代碼 and MZ_EXUNIT= @MZ_EXUNIT -- EX:'0065'
    --限定 業務加班 其他業務
    AND OVERTIME_TYPE in ('OTB','OTT')
    union all    
    select MZ_EXAD/*機關*/
    ,MZ_EXUNIT/*機關單位*/
    ,DUTYDATE as OVER_DAY/*上班年月日*/
    ,MZ_ID/*員警ID*/
    ,ISDIRECTTIME as OVER_TOTAL/*加班時數*/
    ,CASE 
        WHEN DUTYUSERCLASSIFY='Inner' THEN '內勤' 
        WHEN DUTYUSERCLASSIFY='Outter' THEN '外勤' 
        ELSE '' END
        + '超勤加班' as OTYPE /*業務/超勤*/
    /*內外勤對於上班日的判定也會不同*/
    , CASE 
        WHEN DUTYUSERCLASSIFY='Inner' AND NVL(H.MZ_DAYTYPE,'D')='D' then '1工作日'
        WHEN DUTYUSERCLASSIFY='Inner' AND NVL(H.MZ_DAYTYPE,'D')='H' then '2假日'
        WHEN DUTYUSERCLASSIFY='Outter' AND DUTYSTOPOFF='N' then '1工作日'
        WHEN DUTYUSERCLASSIFY='Outter' AND DUTYSTOPOFF='Y' then '2假日'
        ELSE '0其他' END as DTYPE 
    from C_DUTYTABLE_PERSONAL O
    --新開的上班日判定資料表
    LEFT JOIN C_DUTYHOLIDAY_V2 H on O.DUTYDATE=H.MZ_DATE
    --篩選年月
    where DUTYDATE like @YEAR + @Month+'%'
    --篩選機關
    AND MZ_EXAD=@MZ_EXAD
    --邏輯條件:單位代碼 and MZ_EXUNIT= @MZ_EXUNIT -- EX:'0065'
),
CTE02_LV2 AS (
/* CTE02_LV2: 根據上述CTE,再統計出員警每天的加班時數統計,將上述多種加班資料進行每天的加總 */
SELECT MZ_EXAD/*機關*/
    ,MZ_EXUNIT/*機關單位*/
    ,OVER_DAY/*上班年月日*/
    ,MZ_ID/*員警ID*/
    ,SUM(FLOOR(OVER_TOTAL/60)) as OVER_TOTAL_HOUR 
    ,MAX(DTYPE) as DTYPE /*1工作日,2假日,若同一天出現兩者,則視同假日*/
FROM CTE02_LV1
GROUP BY MZ_EXAD/*機關*/
    ,MZ_EXUNIT/*機關單位*/
    ,OVER_DAY/*上班年月日*/
    ,MZ_ID/*員警ID*/
),
CTE02_LV3 AS(
    /*CTE02_LV3: 根據上述CTE,再統計出每個員警最高記錄曾經一天上班多少小時?*/
    select MZ_EXAD/*機關*/
    ,MZ_EXUNIT/*機關單位*/
    ,MZ_ID/*員警ID*/
    ,MAX( CASE when DTYPE='1' then OVER_TOTAL_HOUR+8/*平日+8*/ else OVER_TOTAL_HOUR/*假日照加班時數算*/ END ) as MAX_WORK_HOUR_IN_MONTH
    from CTE02_LV2
    GROUP BY MZ_EXAD/*機關*/
    ,MZ_EXUNIT/*機關單位*/
    ,MZ_ID/*員警ID*/    
),
CTE02_LV4 AS(
    /*CTE02_LV4: 根據上述CTE,再統計出每個機關單位出現最高上班時間的人數分布統計*/
    select MZ_EXAD/*機關*/
    ,MZ_EXUNIT/*機關單位*/
    --每日(含放假日及例假日)加班時數連同法定辦公時數 的相關統計
    ,SUM( case when MAX_WORK_HOUR_IN_MONTH>12 and MAX_WORK_HOUR_IN_MONTH<=14 then 1 else 0 end ) as CNT_12TO14
    ,SUM( case when MAX_WORK_HOUR_IN_MONTH>14 then 1 else 0 end ) as CNT_OVER14     
    from CTE02_LV3
    GROUP BY MZ_EXAD/*機關*/
    ,MZ_EXUNIT/*機關單位*/
)

/*
    預計抓出欄位:MZ_KCHI, MZ_UNIT, UNDER20, OVER20TO45, OVER45TO61, OVER61
    */
select         
    AU.MZ_AD
    ,K.MZ_KCHI --機關名稱
    ,AU.MZ_UNIT --機關代碼
    --20小時以下之人數
    ,NVL(T1.Under20,0) Under20
    --21小時以上未滿45小時之人數
    ,NVL(T1.Over20To45,0) Over20To45
    --45小時以上未滿61小時之人數
    ,NVL(T1.Over45To61,0) Over45To61
    --61小時以上之人數
    ,NVL(T1.Over61,0) Over61
--每日(含放假日及例假日)加班時數連同法定辦公時數 的相關統計
    ,NVL(T2.CNT_12TO14,0) CNT_12TO14
    ,NVL(T2.CNT_OVER14 ,0) CNT_OVER14
FROM A_UNIT_AD AU
--代碼表,警局單位:
join A_KTYPE K on AU.MZ_UNIT=K.MZ_KCODE and MZ_KTYPE='25'--限定代碼類型 單位
/*針對當月加班總時數的相關統計,這邊只抓取有統計到月結算加班資料的單位,若無則不顯示*/
join CTE01_LV3 T1 on AU.MZ_AD = T1.MZ_EXAD /*機關*/ and AU.MZ_UNIT = T1.MZ_EXUNIT/*單位*/
/*針對每日(含放假日及例假日)加班時數連同法定辦公時數 的相關統計,相當於去計算有多少同仁曾經在一天內上班超過指定時數 */
left join CTE02_LV4 T2 on AU.MZ_AD = T2.MZ_EXAD /*機關*/ and AU.MZ_UNIT = T2.MZ_EXUNIT/*單位*/
WHERE  1 = 1
--根據機關ID篩選旗下的單位
AND AU.MZ_AD =@MZ_EXAD
--邏輯條件:單位代碼 AND AU.MZ_UNIT = @MZ_EXUNIT
order by  AU.MZ_NO1 ASC,K.MZ_KCHI,AU.MZ_UNIT
";



            List<SqlParameter> SqlPrams = new List<SqlParameter>();
            SqlPrams.Add(new SqlParameter(":MZ_EXAD", MZ_EXAD));
            SqlPrams.Add(new SqlParameter(":YEAR", YEAR));
            SqlPrams.Add(new SqlParameter(":Month", Month));

            //特殊:有指定 單位,就解除備註,
            if (MZ_EXUNIT.IsNullOrEmpty() == false)
            {
                //否則就把這段註解拿掉,讓語法有效
                SQL = SQL.Replace("--邏輯條件:單位代碼", "");
                SqlPrams.Add(new SqlParameter(":MZ_EXUNIT", MZ_EXUNIT));
            }

            return o_DBFactory.ABC_toTest.Create_Table(SQL, SqlPrams);
        }


        public HSSFWorkbook GetWorkbook(DataTable dt
            , string ADName, string UnitName
            , string YEAR, string MONTH)
        {


            HSSFWorkbook workbook = null;
            //抓取範本
            string SamplePath = Server.MapPath("~/3-forleave/report/C_OVERTIME_OVERWORK_DETAIL_RPT.xls");
            using (FileStream fileStream = new FileStream(SamplePath, FileMode.Open, FileAccess.Read))
            {
                workbook = new HSSFWorkbook(fileStream);
            }
            //第一個分業
            HSSFSheet sheet = workbook.GetSheetAt(0);

            //抓取表頭文字
            //新北市政府警察局ＸＸ分局○年○月加班情形統計表
            //#ADName##YEAR#年#MONTH#月加班情形統計表

            string title = "#ADName##UnitName##YEAR#年#MONTH#月加班情形統計表";
            title = title.Replace("#ADName#", ADName);
            title = title.Replace("#UnitName#", UnitName);
            title = title.Replace("#YEAR#", YEAR);
            title = title.Replace("#MONTH#", MONTH);
            //將資料綁到內容中
            sheet.GetRow(0).GetCell(0).SetCellValue(title);

            //一開始簽名區的index為3,也就是第4列
            int i_sheetRow = 3;

            //先把範本中的下半部往下搬動
            if (dt.Rows.Count > 0)
            {
                //從 index=3 到 index=4, DB抓到多少筆資料,就往下搬動多少筆
                sheet.ShiftRows(i_sheetRow, i_sheetRow + 1, dt.Rows.Count);
            }
            //備註這列比較高
            sheet.GetRow(i_sheetRow + dt.Rows.Count).Height = 3700;

            int i_MaxCell = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                //然後建立新的列
                var sheetRow = sheet.CreateRow(i_sheetRow);
                i_sheetRow++;


                //綁定數值
                DataRow dataRow = dt.Rows[i];
                int i_sheetCell = 0;//歸零

                //DataTable預計格式
                /*
                MZ_AD	382130000C
                MZ_KCHI	後勤科
                MZ_UNIT	65
                UNDER20	7
                OVER20TO45	0
                OVER45TO61	0
                OVER61	1
                CNT_12TO14	0
                CNT_OVER14	0
                     */

                //EXCEL預計格式
                /*
                機關名稱	後勤科
                20小時以下之人數	5
                21小時以上未滿45小時之人數	4
                45小時以上未滿61小時之人數	6
                61小時以上之人數	5
                超過12小時14小時以下之人數	5
                超過14小時之人數	4
                備註	
                */

                //機關名稱 後勤科
                var cell = sheetRow.CreateCell(i_sheetCell++);
                cell.SetCellValue(dataRow["MZ_KCHI"].ToString());
                //20小時以下之人數   5
                cell = sheetRow.CreateCell(i_sheetCell++);
                cell.SetCellValue(dataRow["UNDER20"].ToString().SafeToInt());
                //21小時以上未滿45小時之人數 4
                cell = sheetRow.CreateCell(i_sheetCell++);
                cell.SetCellValue(dataRow["OVER20TO45"].ToString().SafeToInt());
                //45小時以上未滿61小時之人數 6
                cell = sheetRow.CreateCell(i_sheetCell++);
                cell.SetCellValue(dataRow["OVER45TO61"].ToString().SafeToInt());
                //61小時以上之人數   5
                cell = sheetRow.CreateCell(i_sheetCell++);
                cell.SetCellValue(dataRow["OVER61"].ToString().SafeToInt());
                //超過12小時14小時以下之人數 5
                cell = sheetRow.CreateCell(i_sheetCell++);
                cell.SetCellValue(dataRow["CNT_12TO14"].ToString().SafeToInt());
                //超過14小時之人數   4
                cell = sheetRow.CreateCell(i_sheetCell++);
                cell.SetCellValue(dataRow["CNT_OVER14"].ToString().SafeToInt());
                //備註
                cell = sheetRow.CreateCell(i_sheetCell++);

                //紀錄Cell的index最大值,等一下有用
                i_MaxCell = i_sheetCell - 1;
            }

            //統一設定樣式 
            OfficeHelpers.ExcelHelpers.CellStyle style = new OfficeHelpers.ExcelHelpers.CellStyle();
            style.CenterText = true;//文字靠中
            style.UseBorder = true;//有框線
            style.FontName = "標楷體";//標楷體

            OfficeHelpers.ExcelHelpers.Set_CellStyle_Range(workbook, sheet, style
                , 3, i_sheetRow - 1
                , 0, i_MaxCell);

            return workbook;
        }

        public bool TEST_GetDataTable()
        {

            //不限制單位的劃一定會超過10筆,因為每個單位都會一筆
            var dt = GetDataTable("382130000C", "", "111", "09");
            //不到10
            if (dt.Rows.Count < 10)
            {
                return false;
            }
            return true;
        }



        public bool TEST_GetWorkbook()
        {
            var dt = GetDataTable("382130000C", "0065", "111", "09");
            var workbook = GetWorkbook(dt, "新北市警察局OO分局", "OO派出所", "111", "09");
            //匯出
            OfficeHelpers.ExcelHelpers.Response_Workbook(workbook, "加班情形統計表");
            return false;
        }
    }
}