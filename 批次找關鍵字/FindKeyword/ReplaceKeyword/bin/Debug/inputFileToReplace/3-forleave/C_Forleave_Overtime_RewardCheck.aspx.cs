using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TPPDDB.Logic;
using System.Data.SqlClient;
using System.Data;
using TPPDDB.App_Code;

namespace TPPDDB._3_forleave
{
    public partial class C_Forleave_Overtime_RewardCheck : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DropDownList_EXAD.DataBind();
            }
        }
        // 單位
        protected void DropDownList_AD_SelectedIndexChanged(object sender, EventArgs e)
        {
            C.fill_unit(DropDownList_EXUNIT, DropDownList_EXAD.SelectedValue);
        }
        // 單位前方補空值
        protected void DropDownList_UNIT_DataBound(object sender, EventArgs e)
        {
            ListItem li = new ListItem(" ", "");
            DropDownList_EXUNIT.Items.Insert(0, li);
        }
        // 送出審核紐
        protected void btnReward_Click(object sender, EventArgs e)
        {
            string unitstr = "";
            //查詢結果
            for (int i = 0; i < gvList.Rows.Count; i++)
            {
                CheckBox ckb = gvList.Rows[i].Cells[0].Controls[1] as CheckBox;         // 是否勾選
                if (ckb.Checked)
                {
                    String Part = YearRange.SelectedValue; //上或下半年
                    string forYearRange = string.Empty;
                    switch (Part)
                    {
                        default: //若錯誤，則直接使用上半年度作為查詢條件
                        case "Up":
                            forYearRange = "U";
                            break;
                        case "Down":
                            forYearRange = "D";
                            break;
                    }

                    // Update 審核表資料
                    List<SqlParameter> lstParameter = new List<SqlParameter>();
                    lstParameter.Add(new SqlParameter("MZ_EXAD", SqlDbType.Char) { Value = DropDownList_EXAD.SelectedValue });                  // 機關

                    string UPYEAR_LEFT_HOUR = ((TextBox)gvList.Rows[i].Cells[10].FindControl("txtUPYEAR_LEFT_HOUR")).Text;// 剩餘時數
                    string LEFT_HOUR = ((TextBox)gvList.Rows[i].Cells[10].FindControl("txtLEFT_HOUR")).Text;// 合計時數
                    string REWARD_HORU = ((TextBox)gvList.Rows[i].Cells[10].FindControl("txtREWARD_HORU")).Text;// 敘獎時數
                    if (forYearRange == "U")
                    {
                        int Total = 0;
                        int UpYearLeft = 0;
                        int RewardTotal = 0;
                        int LeftHour = 0;
                        if (int.TryParse(UPYEAR_LEFT_HOUR, out UpYearLeft) && int.TryParse(LEFT_HOUR, out LeftHour))//
                        {
                            Total = UpYearLeft + LeftHour;
                            UpYearLeft = Total % 40;// 剩餘時數

                            //敘獎時數
                            RewardTotal = Total - (Total % 40); //以40小時為單位
                            UPYEAR_LEFT_HOUR = UpYearLeft.ToString();
                            REWARD_HORU = RewardTotal.ToString();
                        }
                    }
                    lstParameter.Add(new SqlParameter("MZ_YEAR", SqlDbType.Char) { Value = tb_Year.Text });                                     // 年度
                    lstParameter.Add(new SqlParameter("MZ_YEARRANGE", SqlDbType.Char) { Value = forYearRange });                                // 上、下半年度
                    lstParameter.Add(new SqlParameter("MZ_ID", SqlDbType.Char) { Value = gvList.Rows[i].Cells[1].Text });                       // 身分證字號
                    lstParameter.Add(new SqlParameter("MZ_HOUR_H1_LEFT", SqlDbType.Char) { Value = UPYEAR_LEFT_HOUR });      // 剩餘時數
                    lstParameter.Add(new SqlParameter("MZ_HOUR_H1", SqlDbType.Char) { Value = LEFT_HOUR });                  // 合計時數
                    lstParameter.Add(new SqlParameter("MZ_HOUT_AWARD", SqlDbType.Char) { Value = REWARD_HORU });             // 敘獎時數
                    lstParameter.Add(new SqlParameter("MZ_MEMO", SqlDbType.Char) { Value = ((TextBox)gvList.Rows[i].Cells[13].FindControl("txtMEMO")).Text });             

                    if (DropDownList_EXUNIT.SelectedValue != "")
                    {
                        unitstr = " and MZ_EXUNIT = @MZ_EXUNIT";
                        lstParameter.Add(new SqlParameter("MZ_EXUNIT", SqlDbType.Char) { Value = DropDownList_EXUNIT.SelectedValue });              // 單位
                    }
                    string updateSQL = @"Update  C_DUTYTABLE_REWARD  set MZ_HOUR_H1 = @MZ_HOUR_H1, MZ_HOUR_H1_LEFT = @MZ_HOUR_H1_LEFT, MZ_HOUT_AWARD = @MZ_HOUT_AWARD, MZ_VERIFY = '1',MZ_MEMO = @MZ_MEMO
                                         where MZ_EXAD = @MZ_EXAD" + unitstr + " and MZ_YEAR = @MZ_YEAR and MZ_YEARRANGE = @MZ_YEARRANGE and MZ_ID = @MZ_ID";

                    try
                    {
                        o_DBFactory.ABC_toTest.SQLExecute(updateSQL, lstParameter);
                    }
                    catch
                    {
                    }
                }
                else
                {

                    String Part = YearRange.SelectedValue; //上或下半年
                    string forYearRange = string.Empty;
                    switch (Part)
                    {
                        default: //若錯誤，則直接使用上半年度作為查詢條件
                        case "Up":
                            forYearRange = "U";
                            break;
                        case "Down":
                            forYearRange = "D";
                            break;
                    }

                    // Update 審核表資料
                    List<SqlParameter> lstParameter = new List<SqlParameter>();
                    lstParameter.Add(new SqlParameter("MZ_EXAD", SqlDbType.Char) { Value = DropDownList_EXAD.SelectedValue });                  // 機關

                    lstParameter.Add(new SqlParameter("MZ_YEAR", SqlDbType.Char) { Value = tb_Year.Text });                                     // 年度
                    lstParameter.Add(new SqlParameter("MZ_YEARRANGE", SqlDbType.Char) { Value = forYearRange });                                // 上、下半年度
                    lstParameter.Add(new SqlParameter("MZ_ID", SqlDbType.Char) { Value = gvList.Rows[i].Cells[1].Text });                       // 身分證字號

                    if (DropDownList_EXUNIT.SelectedValue != "")
                    {
                        unitstr = " and MZ_EXUNIT = @MZ_EXUNIT";
                        lstParameter.Add(new SqlParameter("MZ_EXUNIT", SqlDbType.Char) { Value = DropDownList_EXUNIT.SelectedValue });              // 單位
                    }
                    string updateSQL = @"Update  C_DUTYTABLE_REWARD  set  MZ_VERIFY = '0'
                                         where MZ_EXAD = @MZ_EXAD" + unitstr + " and MZ_YEAR = @MZ_YEAR and MZ_YEARRANGE = @MZ_YEARRANGE and MZ_ID = @MZ_ID";
                    try
                    {
                        o_DBFactory.ABC_toTest.SQLExecute(updateSQL, lstParameter);
                    }
                    catch
                    {
                    }
                }
            }
            searchgvlist();
        }

        // 列印按鈕
        protected void btnPrint_click(object sender, EventArgs e)
        {
            String Message = String.Empty;
            DataTable dt = new DataTable();

            //檢驗查詢條件送出值
            if (ValidCondition(ref Message))
            {
                searchPrintDT(ref dt);
                if (dt.Rows.Count > 0)
                {

                    //dt.Columns.Add("備註");
                    dt.Columns["單位"].SetOrdinal(0);         // 擺在第一位
                    dt.Columns.Remove("MZ_ID");
                    dt.Columns.Remove("MZ_VERIFY");
                    dt.Columns["MZ_NAME"].ColumnName = "姓名";
                    dt.Columns["MZ_OCCC"].ColumnName = "職稱";
                    dt.Columns["MZ_HOUR_H1_LEFT"].ColumnName = "剩餘時數";
                    dt.Columns["MZ_HOUR_H1"].ColumnName = "合計時數";
                    dt.Columns["MZ_HOUT_AWARD"].ColumnName = "敘獎時數";
                    dt.Columns["MZ_MEMO"].ColumnName = "備註";
                    dt.Columns["MZ_MEMO1"].ColumnName = "建議敘獎";

                    if (YearRange.SelectedValue == "Up")
                    {
                        dt.Columns["Month1"].ColumnName = "1月";
                        dt.Columns["Month2"].ColumnName = "2月";
                        dt.Columns["Month3"].ColumnName = "3月";
                        dt.Columns["Month4"].ColumnName = "4月";
                        dt.Columns["Month5"].ColumnName = "5月";
                        dt.Columns["Month6"].ColumnName = "6月";
                    }
                    else if (YearRange.SelectedValue == "Down")
                    {
                        dt.Columns["Month1"].ColumnName = "7月";
                        dt.Columns["Month2"].ColumnName = "8月";
                        dt.Columns["Month3"].ColumnName = "9月";
                        dt.Columns["Month4"].ColumnName = "10月";
                        dt.Columns["Month5"].ColumnName = "11月";
                        dt.Columns["Month6"].ColumnName = "12月";
                    }
                    App_Code.ToExcel.Dt2Excel(dt, "未支領超勤加班費敘獎名冊");
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "alert('查無資料')", true);
                    return;
                }
            }
        }

        // 列印報表查詢條件
        protected void searchPrintDT(ref DataTable ReBuildedDt)
        {
            DataTable dt = new DataTable();

            String Year = tb_Year.Text; //年分
            String EXAD = DropDownList_EXAD.SelectedValue; //現服機關  
            String EXUNIT = DropDownList_EXUNIT.SelectedValue; //現服單位 
            String Part = YearRange.SelectedValue; //上或下半年
            enumYearPart enumPart; //年度列舉
            string forYearRange = string.Empty;

            String listMonth = String.Empty; //SQL查詢用之月份清單
            switch (Part)
            {
                default: //若錯誤，則直接使用上半年度作為查詢條件
                case "Up":
                    enumPart = enumYearPart.Up;
                    listMonth = @"'01','02','03','04','05','06'";
                    forYearRange = "U";
                    break;
                case "Down":
                    enumPart = enumYearPart.Down;
                    listMonth = @"'07','08','09','10','11','12'";
                    forYearRange = "D";
                    break;
            }

            //帶入參數
            List<SqlParameter> Params = new List<SqlParameter>();
            Params.Add(new SqlParameter("mz_year", SqlDbType.NVarChar) { Value = Year });
            Params.Add(new SqlParameter("mz_exad", SqlDbType.NVarChar) { Value = EXAD });
            string unitstr = "";
            if (EXUNIT != "")
            {
                unitstr = " AND mz_exunit    = @mz_exunit";
                Params.Add(new SqlParameter("mz_exunit", SqlDbType.NVarChar) { Value = EXUNIT });
            }
            Params.Add(new SqlParameter("mz_yearRange", SqlDbType.NVarChar) { Value = forYearRange });

            #region 長長的SQL
            String SQL = @"SELECT  Statistics.mz_id,            --身分證字號
                                       Statistics.mz_balance_hour,  --每月結餘時數
                                       Statistics.mz_month,         --月份
                                       (SELECT mz_kchi 
                                        FROM   a_ktype 
                                        WHERE  mz_ktype = '26' 
                                               AND mz_kcode = (SELECT mz_occc 
                                                               FROM   a_dlbase 
                                                               WHERE  mz_id = Statistics.mz_id)) 
                                       AS MZ_OCCC,                  --職稱
                                       (SELECT mz_name 
                                        FROM   a_dlbase 
                                        WHERE  mz_id = Statistics.mz_id)                         
                                       AS MZ_NAME ,                  --員警姓名
                                       (SELECT mz_kchi 
                                        FROM   a_ktype 
                                        WHERE   mz_kcode = (SELECT mz_exunit 
                                                               FROM   a_dlbase 
                                                               WHERE  mz_id = Statistics.mz_id) and ROWNUM <= 1)                         
                                       AS MZ_EXUNITNEW,                  --員警姓名
                                       MZ_HOUR_H1_LEFT
                                FROM   (SELECT mz_id, 
                                               mz_balance_hour,
                                               mz_month 
                                        FROM   c_dutymonthovertime_hour 
                                               --主要查詢條件在這邊!
                                        WHERE  mz_year          = @mz_year
                                               AND mz_exad      = @mz_exad"
                                               + unitstr +
                                               @" AND mz_month in ({0}) --依照年度別給予值
                                        ORDER  BY mz_id) Statistics 
                                       LEFT JOIN a_dlbase base 
                                              ON Statistics.mz_id = base.mz_id 
                                       LEFT JOIN (select mz_verify,mz_id from C_DUTYTABLE_REWARD 
                                              where mz_year     = @mz_year
                                               AND mz_exad      = @mz_exad"
                                               + unitstr +
                                               @" AND MZ_YEARRANGE = @mz_yearRange ) REWARD on Statistics.mz_id = REWARD.mz_id
                                       LEFT JOIN (select MZ_HOUR_H1_LEFT,mz_id from C_DUTYTABLE_REWARD 
                                              where mz_year     = @mz_year
                                               AND mz_exad      = @mz_exad"
                                               + unitstr +
                                               @" AND MZ_YEARRANGE = 'U') REWARD1 on Statistics.mz_id = REWARD1.mz_id
                                where (REWARD.mz_verify <> '1' or REWARD.mz_verify is null) " + unitstr +
                                @" ORDER  BY MZ_EXUNIT,(SELECT Replace(mz_tbdv, 'Z99', '999') 
                                           FROM   a_dlbase 
                                           WHERE  mz_id = Statistics.mz_id), 
                                          (SELECT mz_occc 
                                           FROM   a_dlbase 
                                           WHERE  mz_id = Statistics.mz_id), 
                                          mz_id, 
                                          mz_month  ";

            ///注意! 請勿使用 a_dlbase 去 join c_dutymonthovertime_hour，因查詢時要以使用者「輸入當下」的機關單位為主
            ///注意! 這邊查詢為查出各別月份
            #endregion

            //帶入月份子查詢字串
            SQL = String.Format(SQL, listMonth);

            try
            {
                //執行SQL
                dt = o_DBFactory.ABC_toTest.GetDataTable(SQL, Params);
                if (dt.Rows.Count > 0)
                {
                    //重新建構 DataTable
                    ReBuildedDt = ReBuildingDataTable(dt, enumPart);
                }
            }
            catch
            {
            }
        }

        // 統計按鈕(查詢及insert資料進新表)
        protected void btnStatistics_Click(object sender, EventArgs e)
        {
            String Message = String.Empty;
            DataTable dt = new DataTable();

            //檢驗查詢條件送出值
            if (ValidCondition(ref Message))
            {
                String Year = tb_Year.Text; //年分
                String EXAD = DropDownList_EXAD.SelectedValue; //現服機關  
                String EXUNIT = DropDownList_EXUNIT.SelectedValue; //現服單位 
                String Part = YearRange.SelectedValue; //上或下半年
                enumYearPart enumPart; //年度列舉
                string forYearRange = string.Empty;

                //Message += Year + " " + EXAD + " " + EXUNIT + " " + Part + " ";

                String listMonth = String.Empty; //SQL查詢用之月份清單
                switch (Part)
                {
                    default: //若錯誤，則直接使用上半年度作為查詢條件
                    case "Up":
                        enumPart = enumYearPart.Up;
                        listMonth = @"'01','02','03','04','05','06'";
                        forYearRange = "U";
                        break;
                    case "Down":
                        enumPart = enumYearPart.Down;
                        listMonth = @"'07','08','09','10','11','12'";
                        forYearRange = "D";
                        break;
                }

                //帶入參數
                List<SqlParameter> Params = new List<SqlParameter>();
                Params.Add(new SqlParameter("mz_year", SqlDbType.NVarChar) { Value = Year });
                Params.Add(new SqlParameter("mz_exad", SqlDbType.NVarChar) { Value = EXAD });

                Params.Add(new SqlParameter("mz_yearRange", SqlDbType.NVarChar) { Value = forYearRange });

                string unitstr = "";
                if (EXUNIT != "")
                {
                    unitstr = " AND mz_exunit    = @mz_exunit";
                    Params.Add(new SqlParameter("mz_exunit", SqlDbType.NVarChar) { Value = EXUNIT });
                }

                #region 長長的SQL
                //                String SQL = @"SELECT  Statistics.mz_id,            --身分證字號
                //                                       Statistics.mz_balance_hour,  --每月結餘時數
                //                                       Statistics.mz_month,         --月份
                //                                       (SELECT mz_kchi 
                //                                        FROM   a_ktype 
                //                                        WHERE  mz_ktype = '26' 
                //                                               AND mz_kcode = (SELECT mz_occc 
                //                                                               FROM   a_dlbase 
                //                                                               WHERE  mz_id = Statistics.mz_id)) 
                //                                       AS MZ_OCCC,                  --職稱
                //                                       (SELECT mz_name 
                //                                        FROM   a_dlbase 
                //                                        WHERE  mz_id = Statistics.mz_id)                         
                //                                       AS MZ_NAME,                   --員警姓名
                //                                       mz_exunit,                     --單位
                //                                       MZ_HOUR_H1_LEFT                --年度剩餘
                //                                FROM   (SELECT mz_id, 
                //                                               mz_balance_hour,
                //                                               mz_month 
                //                                        FROM   c_dutymonthovertime_hour 
                //                                               --主要查詢條件在這邊!
                //                                        WHERE  mz_year          = @mz_year
                //                                               AND mz_exad      = @mz_exad"
                //                                               + unitstr +
                //                                               @" AND mz_month in ({0}) --依照年度別給予值
                //                                        ORDER  BY mz_id) Statistics 
                //                                       LEFT JOIN a_dlbase base 
                //                                              ON Statistics.mz_id = base.mz_id 
                //                                       LEFT JOIN (select mz_verify,mz_id from C_DUTYTABLE_REWARD 
                //                                              where mz_year     = @mz_year
                //                                               AND mz_exad      = @mz_exad"
                //                                               + unitstr +
                //                                               @" AND MZ_YEARRANGE = @mz_yearRange ) REWARD on Statistics.mz_id = REWARD.mz_id
                //                                       LEFT JOIN (select MZ_HOUR_H1_LEFT,mz_id from C_DUTYTABLE_REWARD 
                //                                              where mz_year     = @mz_year
                //                                               AND mz_exad      = @mz_exad "
                //                                               + unitstr +
                //                                               @" AND MZ_YEARRANGE = 'U' ) REWARD1  on Statistics.mz_id = REWARD1.mz_id
                //                                where REWARD.mz_verify <> '1' or REWARD.mz_verify is null
                //                                ORDER  BY (SELECT Replace(mz_tbdv, 'Z99', '999') 
                //                                           FROM   a_dlbase 
                //                                           WHERE  mz_id = Statistics.mz_id), 
                //                                          (SELECT mz_occc 
                //                                           FROM   a_dlbase 
                //                                           WHERE  mz_id = Statistics.mz_id), 
                //                                          mz_id, 
                //                                          mz_month  ";

                String SQL = @"SELECT  Statistics.mz_id,            --身分證字號
                                       Statistics.mz_balance_hour,  --每月結餘時數
                                       Statistics.mz_month,         --月份
                                       (SELECT mz_kchi 
                                        FROM   a_ktype 
                                        WHERE  mz_ktype = '26' 
                                               AND mz_kcode = (SELECT mz_occc 
                                                               FROM   a_dlbase 
                                                               WHERE  mz_id = Statistics.mz_id)) 
                                       AS MZ_OCCC,                  --職稱
                                       (SELECT mz_name 
                                        FROM   a_dlbase 
                                        WHERE  mz_id = Statistics.mz_id)                         
                                       AS MZ_NAME,                   --員警姓名
                                        (SELECT mz_kchi 
                                        FROM   a_ktype 
                                        WHERE   mz_kcode = (SELECT mz_exunit 
                                                               FROM   a_dlbase 
                                                               WHERE  mz_id = Statistics.mz_id) and ROWNUM <= 1)                         
                                       AS MZ_EXUNITNEW,                  --員警姓名
                                       mz_exunit,                     --單位
                                       MZ_HOUR_H1_LEFT
                                FROM   (SELECT mz_id, 
                                               mz_balance_hour,
                                               mz_month 
                                        FROM   c_dutymonthovertime_hour 
                                               --主要查詢條件在這邊!
                                        WHERE  mz_year          = @mz_year
                                               AND mz_exad      = @mz_exad"
                                               + unitstr +
                                               @" AND mz_month in ({0}) --依照年度別給予值
                                        ORDER  BY mz_id) Statistics 
                                       LEFT JOIN a_dlbase base 
                                              ON Statistics.mz_id = base.mz_id 
                                       LEFT JOIN (select mz_verify,mz_id from C_DUTYTABLE_REWARD 
                                              where mz_year     = @mz_year
                                               AND mz_exad      = @mz_exad"
                                               + unitstr +
                                               @" AND MZ_YEARRANGE = @mz_yearRange ) REWARD on Statistics.mz_id = REWARD.mz_id
                                       LEFT JOIN (select MZ_HOUR_H1_LEFT,mz_id from C_DUTYTABLE_REWARD 
                                              where mz_year     = @mz_year
                                               AND mz_exad      = @mz_exad"
                                               + unitstr +
                                               @" AND MZ_YEARRANGE = 'U') REWARD1 on Statistics.mz_id = REWARD1.mz_id
                                where (REWARD.mz_verify <> '1' or REWARD.mz_verify is null) " + unitstr +
                                @" ORDER  BY MZ_EXUNIT,(SELECT Replace(mz_tbdv, 'Z99', '999') 
                                           FROM   a_dlbase 
                                           WHERE  mz_id = Statistics.mz_id), 
                                          (SELECT mz_occc 
                                           FROM   a_dlbase 
                                           WHERE  mz_id = Statistics.mz_id), 
                                          mz_id, 
                                          mz_month  ";


                ///注意! 請勿使用 a_dlbase 去 join c_dutymonthovertime_hour，因查詢時要以使用者「輸入當下」的機關單位為主
                ///注意! 這邊查詢為查出各別月份
                #endregion

                //帶入月份子查詢字串
                SQL = String.Format(SQL, listMonth);

                try
                {
                    //執行SQL
                    dt = o_DBFactory.ABC_toTest.GetDataTable(SQL, Params);
                    //Message += dt.Rows.Count.ToString() + " ";
                    if (dt.Rows.Count > 0)
                    {
                        //重新建構 DataTable
                        DataTable ReBuildedDt = ReBuildingDataTable(dt, enumPart);
                        gvList.DataSource = ReBuildedDt;
                        gvList.DataBind();
                        gvList.Columns[13].Visible = false;
                    }
                    else
                        Message = "以此條件查無資料";
                }
                catch (Exception ex)
                {
                    Message = "查詢時發生錯誤，請參考錯誤訊息：\\r\\n\\r\\n" + ex.Message;
                }
            }
            //Message += gvList.Rows.Count.ToString();
            // 利用VridView是否有資料，進行insert動作
            if (gvList.Rows.Count > 0)
            {
                // 上、下半年度
                string forYearRange = string.Empty;
                if (YearRange.SelectedValue == "Up")
                {
                    forYearRange = "U";
                }
                else if (YearRange.SelectedValue == "Down")
                {
                    forYearRange = "D";
                }
                // 將舊有資料刪除
                List<SqlParameter> delParameter = new List<SqlParameter>();
                delParameter.Add(new SqlParameter("MZ_EXAD", SqlDbType.Char) { Value = DropDownList_EXAD.SelectedValue });                  // 機關

                delParameter.Add(new SqlParameter("MZ_YEAR", SqlDbType.Char) { Value = tb_Year.Text });                                     // 年度
                delParameter.Add(new SqlParameter("MZ_YEARRANGE", SqlDbType.Char) { Value = forYearRange });                                // 上、下半年度

                string unitstr = "";
                if (DropDownList_EXUNIT.SelectedValue != "")
                {
                    unitstr = " and MZ_EXUNIT = @MZ_EXUNIT";
                    delParameter.Add(new SqlParameter("MZ_EXUNIT", SqlDbType.Char) { Value = DropDownList_EXUNIT.SelectedValue });              // 單位
                }
                string delSQL = @"DELETE C_DUTYTABLE_REWARD WHERE MZ_EXAD = @MZ_EXAD" + unitstr + " AND MZ_YEAR = @MZ_YEAR AND MZ_YEARRANGE = @MZ_YEARRANGE AND MZ_VERIFY = '0'";

                o_DBFactory.ABC_toTest.SQLExecute(delSQL, delParameter);

                // 新增新資料
                for (int i = 0; i < gvList.Rows.Count; i++)
                {
                    string insertSQL = @"INSERT INTO C_DUTYTABLE_REWARD (MZ_ID,MZ_EXAD,MZ_EXUNIT,MZ_YEAR,MZ_HOUR_H1,MZ_HOUR_H1_LEFT,MZ_HOUT_AWARD,MZ_YEARRANGE,MONTH1,MONTH2,MONTH3,MONTH4,MONTH5,MONTH6)
                                         VALUES (@MZ_ID,@MZ_EXAD,@MZ_EXUNIT,@MZ_YEAR,@MZ_HOUR_H1,@MZ_HOUR_H1_LEFT,@MZ_HOUT_AWARD,@MZ_YEARRANGE,@MONTH1,@MONTH2,@MONTH3,@MONTH4,@MONTH5,@MONTH6)";
                    List<SqlParameter> lstParameter = new List<SqlParameter>();
                    lstParameter.Add(new SqlParameter("MZ_EXAD", SqlDbType.Char) { Value = DropDownList_EXAD.SelectedValue });                // 機關
                    if (DropDownList_EXUNIT.SelectedValue != "")
                    {
                        lstParameter.Add(new SqlParameter("MZ_EXUNIT", SqlDbType.Char) { Value = DropDownList_EXUNIT.SelectedValue });          // 單位
                    }
                    else
                    {
                        lstParameter.Add(new SqlParameter("MZ_EXUNIT", SqlDbType.Char) { Value = gvList.Rows[i].Cells[14].Text });          // 單位
                    }
                    lstParameter.Add(new SqlParameter("MZ_YEAR", SqlDbType.Char) { Value = tb_Year.Text });                                 // 年度
                    lstParameter.Add(new SqlParameter("MZ_YEARRANGE", SqlDbType.Char) { Value = forYearRange });                            // 上、下半年度
                    lstParameter.Add(new SqlParameter("MZ_ID", SqlDbType.Char) { Value = gvList.Rows[i].Cells[1].Text });                   // 身分證字號
                    lstParameter.Add(new SqlParameter("MONTH1", SqlDbType.Char) { Value = gvList.Rows[i].Cells[4].Text });                  // 月份1
                    lstParameter.Add(new SqlParameter("MONTH2", SqlDbType.Char) { Value = gvList.Rows[i].Cells[5].Text });                  // 月份2
                    lstParameter.Add(new SqlParameter("MONTH3", SqlDbType.Char) { Value = gvList.Rows[i].Cells[6].Text });                  // 月份3
                    lstParameter.Add(new SqlParameter("MONTH4", SqlDbType.Char) { Value = gvList.Rows[i].Cells[7].Text });                  // 月份4
                    lstParameter.Add(new SqlParameter("MONTH5", SqlDbType.Char) { Value = gvList.Rows[i].Cells[8].Text });                  // 月份5
                    lstParameter.Add(new SqlParameter("MONTH6", SqlDbType.Char) { Value = gvList.Rows[i].Cells[9].Text });                  // 月份6
                    lstParameter.Add(new SqlParameter("MZ_HOUR_H1_LEFT", SqlDbType.Char) { Value = ((TextBox)gvList.Rows[i].Cells[10].FindControl("txtUPYEAR_LEFT_HOUR")).Text });      // 剩餘時數
                    lstParameter.Add(new SqlParameter("MZ_HOUR_H1", SqlDbType.Char) { Value = ((TextBox)gvList.Rows[i].Cells[11].FindControl("txtLEFT_HOUR")).Text });                  // 合計時數
                    lstParameter.Add(new SqlParameter("MZ_HOUT_AWARD", SqlDbType.Char) { Value = ((TextBox)gvList.Rows[i].Cells[12].FindControl("txtREWARD_HORU")).Text });             // 敘獎時數
                    try
                    {
                        o_DBFactory.ABC_toTest.SQLExecute(insertSQL, lstParameter);
                    }
                    catch
                    {
                        Message = "匯入資料發生錯誤。";
                    }
                }
            }

            if (!String.IsNullOrEmpty(Message))
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('" + Message + "');", true);
        }

        // 查詢按鈕(查詢上、下半年度資料)
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            searchgvlist();
        }

        // 審核按鈕全選及取消全選
        protected void AllChek_Click(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)gvList.HeaderRow.Cells[1].FindControl("AllCheck");
            if (cb.Checked)
            {
                for (int i = 0; i < gvList.Rows.Count; i++)
                {
                    CheckBox cbi = (CheckBox)gvList.Rows[i].Cells[1].FindControl("CheckBox1");
                    cbi.Checked = true;
                }
            }
            else
            {
                for (int i = 0; i < gvList.Rows.Count; i++)
                {
                    CheckBox cbi = (CheckBox)gvList.Rows[i].Cells[1].FindControl("CheckBox1");
                    if (cbi.Enabled)
                    {
                        cbi.Checked = false;
                    }
                }
            }
        }

        // 查詢
        protected void searchgvlist()
        {

            String Message = String.Empty;
            DataTable dt = new DataTable();

            //檢驗查詢條件送出值
            if (ValidCondition(ref Message))
            {
                String Year = tb_Year.Text; //年分
                String EXAD = DropDownList_EXAD.SelectedValue; //現服機關  
                String EXUNIT = DropDownList_EXUNIT.SelectedValue; //現服單位 
                String Part = YearRange.SelectedValue; //上或下半年
                enumYearPart enumPart; //年度列舉

                String forYearRange = String.Empty; //上、下半年度
                switch (Part)
                {
                    default: //若錯誤，則直接使用上半年度作為查詢條件
                    case "Up":
                        enumPart = enumYearPart.Up;
                        forYearRange = "U";
                        break;
                    case "Down":
                        enumPart = enumYearPart.Down;
                        forYearRange = "D";
                        break;
                }

                //帶入參數
                List<SqlParameter> Params = new List<SqlParameter>();
                Params.Add(new SqlParameter("mz_year", SqlDbType.NVarChar) { Value = Year });
                Params.Add(new SqlParameter("mz_exad", SqlDbType.NVarChar) { Value = EXAD });

                Params.Add(new SqlParameter("mz_yearrange", SqlDbType.NVarChar) { Value = forYearRange });

                string unitstr = "";
                if (EXUNIT != "")
                {
                    unitstr = " and mz_exunit = @mz_exunit";
                    Params.Add(new SqlParameter("mz_exunit", SqlDbType.NVarChar) { Value = EXUNIT });
                }
                #region SQL
                String SQL = @"select C_DUTYTABLE_REWARD.MZ_ID,mz_name,Month1,Month2,Month3,Month4,Month5,Month6,MZ_VERIFY,MZ_HOUR_H1,MZ_HOUT_AWARD,MZ_MEMO,MZ_HOUR_H1_LEFT_temp as MZ_HOUR_H1_LEFT,MZ_EXUNIT,(SELECT mz_kchi 
                                        FROM   a_ktype 
                                        WHERE  mz_ktype = '26' 
                                               AND mz_kcode = (SELECT mz_occc 
                                                               FROM   a_dlbase 
                                                               WHERE  mz_id = C_DUTYTABLE_REWARD.mz_id)) 
                                       AS MZ_OCCC
                                from C_DUTYTABLE_REWARD
                                left join (select MZ_ID,MZ_HOUR_H1_LEFT as  MZ_HOUR_H1_LEFT_temp  from  C_DUTYTABLE_REWARD where  mz_exad = @mz_exad" + unitstr + @" and mz_year = @mz_year and mz_yearrange ='U' and MZ_ID = C_DUTYTABLE_REWARD.mz_id) cdr on cdr.MZ_ID = C_DUTYTABLE_REWARD.mz_id 
                                left join (select mz_id,mz_occc,mz_name from  a_dlbase) ad1 on ad1.mz_id = C_DUTYTABLE_REWARD.mz_id
                                left join (select mz_kchi,mz_kcode from a_ktype where  mz_ktype = '26') ak1 on ak1.mz_kcode = ad1.mz_occc
                                where mz_exad = @mz_exad" + unitstr + " and mz_year = @mz_year and mz_yearrange = @mz_yearrange" +
                                @" ORDER  BY MZ_EXUNIT,(SELECT Replace(mz_tbdv, 'Z99', '999') 
                                           FROM   a_dlbase 
                                           WHERE  mz_id = C_DUTYTABLE_REWARD.mz_id), 
                                          (SELECT mz_occc 
                                           FROM   a_dlbase 
                                           WHERE  mz_id = C_DUTYTABLE_REWARD.mz_id), 
                                          mz_id";

                #endregion

                try
                {
                    //執行SQL
                    dt = o_DBFactory.ABC_toTest.GetDataTable(SQL, Params);

                    if (dt.Rows.Count > 0)
                    {
                        gvList.DataSource = dt;
                        gvList.DataBind();
                        gvList.Columns[13].Visible = true;
                    }
                    else
                        Message = "以此條件查無資料";
                }
                catch (Exception ex)
                {
                    Message = "查詢時發生錯誤，請參考錯誤訊息：\\r\\n\\r\\n" + ex.Message;
                }
            }

            if (!String.IsNullOrEmpty(Message))
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('" + Message + "');", true);
        }

        /// <summary>當資料綁定時，進行月份表頭格式切換</summary>
        protected void gvList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            String Part = YearRange.SelectedValue; //上或下半年
            enumYearPart enumPart; //年度列舉
            switch (Part)
            {
                default: //若錯誤，則直接使用上半年度作為查詢條件
                case "Up": enumPart = enumYearPart.Up; break;
                case "Down": enumPart = enumYearPart.Down; break;
            }

            if (e.Row.RowType == DataControlRowType.Header)
            {
                //變更表頭格式
                switch (enumPart)
                {
                    case enumYearPart.Up:
                        e.Row.Cells[4].Text = "1月";
                        e.Row.Cells[5].Text = "2月";
                        e.Row.Cells[6].Text = "3月";
                        e.Row.Cells[7].Text = "4月";
                        e.Row.Cells[8].Text = "5月";
                        e.Row.Cells[9].Text = "6月";
                        break;
                    case enumYearPart.Down:
                        e.Row.Cells[4].Text = "7月";
                        e.Row.Cells[5].Text = "8月";
                        e.Row.Cells[6].Text = "9月";
                        e.Row.Cells[7].Text = "10月";
                        e.Row.Cells[8].Text = "11月";
                        e.Row.Cells[9].Text = "12月";
                        break;
                }
            }
        }

        // 送出按鈕(將查詢之資料Insert進結餘時數表)


        // 列印按鈕(將結餘時數表之資料列印)

        #region 私有函數

        /// <summary>檢查搜尋條件是否有欄位錯誤</summary>
        private Boolean ValidCondition(ref String ErrMsg)
        {
            String s = String.Empty; //部分函數會自帶 Msg, 但此部分MSG為客製，故為滿足函數格式直接隨便給值

            //檢驗年份
            String Year = tb_Year.Text + "0101"; //月份及日期隨便給予後，進行 DateTime 驗證
            if (!LogicCommon.isUnfromatDateStringVaild(Year, ref s))
                ErrMsg += "年分格式錯誤 \\r\\n";

            //檢查是否有選單位
            if (String.IsNullOrEmpty(DropDownList_EXAD.SelectedValue))
                ErrMsg += "請選擇機關";

            //if (String.IsNullOrEmpty(DropDownList_EXUNIT.SelectedValue))
            //    ErrMsg += "請選擇單位";

            //若錯誤訊息為空則代表驗證成功
            return (String.IsNullOrEmpty(ErrMsg)) ? true : false;
        }

        /// <summary>子流程 : 重新整合DataTable</summary>
        /// 因 SQL 回傳是每人逐月逐筆，故要重新進行整併動作
        private DataTable ReBuildingDataTable(DataTable dt, enumYearPart part)
        {
            DataTable dtReBuilded = new DataTable();
            dtReBuilded.Columns.Add("MZ_ID", typeof(String));
            dtReBuilded.Columns.Add("MZ_NAME", typeof(String));
            dtReBuilded.Columns.Add("MZ_OCCC", typeof(String));
            dtReBuilded.Columns.Add("Month1", typeof(Int32));
            dtReBuilded.Columns.Add("Month2", typeof(Int32));
            dtReBuilded.Columns.Add("Month3", typeof(Int32));
            dtReBuilded.Columns.Add("Month4", typeof(Int32));
            dtReBuilded.Columns.Add("Month5", typeof(Int32));
            dtReBuilded.Columns.Add("Month6", typeof(Int32));
            dtReBuilded.Columns.Add("MZ_HOUR_H1_LEFT", typeof(Int32));     //上半年度剩餘時數(需要從新表裡面去抓取；上半年度此欄必為0)
            dtReBuilded.Columns.Add("MZ_HOUR_H1", typeof(Int32));            //時數合計(六個月份的加總合計；但若新表裡面有，則需要抓出，代表主管有做修正動作)
            dtReBuilded.Columns.Add("MZ_HOUT_AWARD", typeof(Int32));          //敘獎時數(預先算好；但若新表裏面有，則需要抓出)
            dtReBuilded.Columns.Add("MZ_VERIFY", typeof(String));       //是否已審核
            dtReBuilded.Columns.Add("MZ_MEMO", typeof(String));         //備註
            dtReBuilded.Columns.Add("MZ_MEMO1", typeof(String));         //建議敘獎
            if (dt.Columns.Contains("MZ_EXUNITNEW"))
            {
                dtReBuilded.Columns.Add("單位", typeof(String));    //單位
            }
            if (dt.Columns.Contains("MZ_EXUNIT"))
            {
                dtReBuilded.Columns.Add("MZ_EXUNIT", typeof(String));    //單位
            }

            #region 1.針對資料庫內所撈出之原始資料，進行月份的整併
            foreach (DataRow row in dt.Rows)
            {
                String Name = row["MZ_NAME"].ToString();

                //若此列未在重建陣列內出現，則建立新列
                if (dtReBuilded.Select(String.Format("MZ_NAME = '{0}'", Name)).Length <= 0)
                {
                    //處理後的新資料列
                    DataRow NewRow = dtReBuilded.NewRow(); //建立新列
                    NewRow["MZ_ID"] = row["MZ_ID"];
                    NewRow["MZ_NAME"] = row["MZ_NAME"];
                    NewRow["MZ_OCCC"] = row["MZ_OCCC"];
                    if (dt.Columns.Contains("MZ_EXUNITNEW"))
                    {
                        NewRow["單位"] = row["MZ_EXUNITNEW"];
                    }
                    if (dt.Columns.Contains("MZ_EXUNIT"))
                    {
                        NewRow["MZ_EXUNIT"] = row["MZ_EXUNIT"];
                    }
                    //取出所有陣列內該名人員的資料(預計會至少有六個月份的資料)
                    DataRow[] rs = dt.Select(String.Format("MZ_NAME = '{0}'", Name));

                    //將該使用者六個月份的資料，整併到新的資料列裡面
                    foreach (DataRow r in rs)
                    {
                        String month = r["MZ_MONTH"].ToString(); //某月份
                        String value = r["mz_balance_hour"].ToString(); //該月份之值
                        NewRow["MZ_HOUR_H1_LEFT"] = (!String.IsNullOrEmpty(r["MZ_HOUR_H1_LEFT"].ToString())) ? Convert.ToInt32(r["MZ_HOUR_H1_LEFT"]) : 0;
                        switch (part)
                        {
                            case enumYearPart.Up:
                                switch (month)
                                {
                                    case "01": NewRow["Month1"] = value; break;
                                    case "02": NewRow["Month2"] = value; break;
                                    case "03": NewRow["Month3"] = value; break;
                                    case "04": NewRow["Month4"] = value; break;
                                    case "05": NewRow["Month5"] = value; break;
                                    case "06": NewRow["Month6"] = value; break;
                                }
                                break;
                            case enumYearPart.Down:
                                switch (month)
                                {
                                    case "07": NewRow["Month1"] = value; break;
                                    case "08": NewRow["Month2"] = value; break;
                                    case "09": NewRow["Month3"] = value; break;
                                    case "10": NewRow["Month4"] = value; break;
                                    case "11": NewRow["Month5"] = value; break;
                                    case "12": NewRow["Month6"] = value; break;
                                }
                                break;
                        }
                    }//end foreach

                    //加入新列
                    dtReBuilded.Rows.Add(NewRow);
                }
                else //若已存過則直接略過 
                    continue;
            } //end foreach
            #endregion

            #region 2.針對整併後的資料，進行月份值的加總及額外欄位計算

            for (int i = 0; i < dtReBuilded.Rows.Count; i++) //新表建議可以整合進外面的SQL語句裡面
            {
                DataRow row = dtReBuilded.Rows[i];
                Int32 Total = 0; //總時數
                Int32 RewardTotal = 0; //總敘獎時數
                Int32 UpYearLeft = 0; //上半年度剩餘

                if (true)
                {
                    //新表不存在，重新計算欄位資料
                    Int32[] arrMonth = new Int32[6];
                    arrMonth[0] = (!String.IsNullOrEmpty(row["Month1"].ToString())) ? Convert.ToInt32(row["Month1"]) : 0;
                    arrMonth[1] = (!String.IsNullOrEmpty(row["Month2"].ToString())) ? Convert.ToInt32(row["Month2"]) : 0;
                    arrMonth[2] = (!String.IsNullOrEmpty(row["Month3"].ToString())) ? Convert.ToInt32(row["Month3"]) : 0;
                    arrMonth[3] = (!String.IsNullOrEmpty(row["Month4"].ToString())) ? Convert.ToInt32(row["Month4"]) : 0;
                    arrMonth[4] = (!String.IsNullOrEmpty(row["Month5"].ToString())) ? Convert.ToInt32(row["Month5"]) : 0;
                    arrMonth[5] = (!String.IsNullOrEmpty(row["Month6"].ToString())) ? Convert.ToInt32(row["Month6"]) : 0;

                    //時數總計
                    foreach (int n in arrMonth) Total += n;

                    //上半年度剩餘
                    //if (part == enumYearPart.Up) UpYearLeft = 0;
                    if (part == enumYearPart.Up)
                    {
                        UpYearLeft = Total % 40;

                        //總時數
                        RewardTotal = Total - (Total % 40); //以40小時為單位
                    }
                    else
                    {
                        UpYearLeft = (!String.IsNullOrEmpty(row["MZ_HOUR_H1_LEFT"].ToString())) ? Convert.ToInt32(row["MZ_HOUR_H1_LEFT"]) : 0;

                        //總時數
                        RewardTotal = (Total + UpYearLeft) - ((Total + UpYearLeft) % 40); //以40小時為單位
                    }
                }
                else
                {
                    //若新表存在，則使用新表資料
                    // UpYearLeft = 新表資料
                }

                //因有些人可能是特定月份後才加入，導致為NULL值，故在這邊補零
                if (String.IsNullOrEmpty(row["Month1"].ToString())) dtReBuilded.Rows[i]["Month1"] = 0;
                if (String.IsNullOrEmpty(row["Month2"].ToString())) dtReBuilded.Rows[i]["Month2"] = 0;
                if (String.IsNullOrEmpty(row["Month3"].ToString())) dtReBuilded.Rows[i]["Month3"] = 0;
                if (String.IsNullOrEmpty(row["Month4"].ToString())) dtReBuilded.Rows[i]["Month4"] = 0;
                if (String.IsNullOrEmpty(row["Month5"].ToString())) dtReBuilded.Rows[i]["Month5"] = 0;
                if (String.IsNullOrEmpty(row["Month6"].ToString())) dtReBuilded.Rows[i]["Month6"] = 0;

                //存入DT
                dtReBuilded.Rows[i]["MZ_HOUR_H1"] = Total;
                dtReBuilded.Rows[i]["MZ_HOUT_AWARD"] = RewardTotal;
                dtReBuilded.Rows[i]["MZ_HOUR_H1_LEFT"] = UpYearLeft;
                dtReBuilded.Rows[i]["MZ_VERIFY"] = "N"; //開發階段設為N

                string MEMO = o_A_KTYPE.Find_Ktype_Cname(RewardTotal.ToString(), "@95");
                dtReBuilded.Rows[i]["MZ_MEMO1"] = MEMO;
            }

            #endregion

            return dtReBuilded;
        }

        #endregion

        #region 類別

        /// <summary>年度別</summary>
        public enum enumYearPart
        {
            /// <summary>上半年</summary>
            Up = 1,
            /// <summary>下半年</summary>
            Down = 2
        }

        #endregion



    }
}