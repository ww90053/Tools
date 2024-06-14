using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Web.Configuration;
using System.Data.SqlClient;
using AjaxControlToolkit;
using System.Collections.Generic;
using TPPDDB.Service;
using TPPDDB.Model;
using TPPDDB.Logic;
using TPPDDB.Model;
using TPPDDB.Model.Const;
using TPPDDB.Helpers;

namespace TPPDDB._3_forleave
{
    public partial class C_ForLeaveOvertime_OVERTIMEHOUR : System.Web.UI.Page
    {
        string selectUnit
        {
            get { return ddl_MZUNIT.SelectedValue; }
        }
        string MZPower
        {
            get
            {
                return o_a_Function.strGID(Session["ADPMZ_ID"].ToString());
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                C.check_power();
                C.set_Panel_EnterToTAB(ref this.Panel2);

                TextBox_DUTYYEAR.Text = (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0');
                DropDownList_DUTYMONTH.SelectedValue = DateTime.Now.Month.ToString();

                FillAD(ref DropDownList_AD);
                // Joy 現服機關改為編制機關
                // Neil 改為發薪機關
                //發薪機關如果空值，改抓  "編制機關"
                String MZCODE = "";
                if (Session[ConstSession.ADPPAY_AD] != null)
                {
                    if (Session[ConstSession.ADPPAY_AD].ToString() != "")
                    {
                        MZCODE = Session[ConstSession.ADPPAY_AD].ToString();
                    }
                    else
                    {
                        MZCODE = Session["ADPMZ_AD"].ToString();
                    }
                }
                else
                {
                    MZCODE = Session["ADPMZ_AD"].ToString();
                }

                DropDownList_AD.SelectedValue = MZCODE;

                UNIT();
                // Joy 現服單位改為編制單位
                ddl_MZUNIT.SelectedValue = Session["ADPMZ_UNIT"].ToString();

                GV1_DataBind();
            }
        }
        protected void FillAD(ref DropDownList ddl)
        {

            //20150629 原先這邊是撈取 EXAD，改為發薪機關
            //注意 : 這邊是以機關的前五碼作為撈取條件，主要是根據縣市而不同
            //String MZCODE = String.Format("{0}%", Session[ConstSession.ADPPAY_AD].ToString().Substring(0, 5));
            String MZCODE = "";
            if (Session[ConstSession.ADPPAY_AD] != null)
            {
                if (Session[ConstSession.ADPPAY_AD].ToString() != "")
                {
                    MZCODE = String.Format("{0}%", Session[ConstSession.ADPPAY_AD].ToString().Substring(0, 5));
                }
                else
                {
                    MZCODE = String.Format("{0}%", Session["ADPMZ_AD"].ToString().Substring(0, 5));
                }
            }
            else
            {
                MZCODE = String.Format("{0}%", Session["ADPMZ_AD"].ToString().Substring(0, 5));
            }
            String sql = @"SELECT MZ_KCODE, MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE = '04' AND MZ_KCODE like @MZCODE";

            List<SqlParameter> ps = new List<SqlParameter>();
            ps.Add(new SqlParameter("MZCODE", MZCODE));
            ddl.DataSource = o_DBFactory.ABC_toTest.DataSelect(sql, ps);
            ddl.DataBind();

            //將中和分局的限制取消
            //if (Session[ConstSession.ADPPAY_AD].ToString() == "382133600C")
            //{
            //    sql = @"SELECT MZ_KCODE, MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE = '04' AND MZ_KCODE in ('382133400C','382133500C','382133600C')";
            //    ddl.DataSource = o_DBFactory.ABC_toTest.DataSelect(sql);
            //    ddl.DataBind();
            //}
            //else
            //{
            //    List<SqlParameter> ps = new List<SqlParameter>();
            //    ps.Add(new SqlParameter("MZCODE", MZCODE));
            //    ddl.DataSource = o_DBFactory.ABC_toTest.DataSelect(sql, ps);
            //    ddl.DataBind();
            //}


            switch (MZPower) //注意: 全域變數
            {
                case "A":
                case "B":
                    ddl.Enabled = true;
                    break;
                case "C":
                case "D":
                case "E":
                    //matthew 如果是中和分局進來要可以下拉中和一&中和二
                    if (MZCODE == "382133600C")
                    {
                        ddl.Enabled = true;
                    }
                    else
                    {
                        ddl.Enabled = false;
                    }

                    break;
            }
        }

        //取得UNIT
        protected void UNIT()
        {
            string sql = @"
                        SELECT MZ_KCODE,MZ_KCHI FROM A_UNIT_AD
                        JOIN A_KTYPE ON A_UNIT_AD.MZ_UNIT = A_KTYPE.MZ_KCODE AND A_KTYPE.MZ_KTYPE='25' AND A_UNIT_AD.MZ_AD = @MZAD";
            List<SqlParameter> ps = new List<SqlParameter>();
            ps.Add(new SqlParameter("MZAD", DropDownList_AD.SelectedValue));
            ddl_MZUNIT.DataSource = o_DBFactory.ABC_toTest.DataSelect(sql, ps);
            ddl_MZUNIT.DataTextField = "MZ_KCHI";
            ddl_MZUNIT.DataValueField = "MZ_KCODE";
            ddl_MZUNIT.DataBind();
            switch (MZPower)
            {
                case "A":
                case "B":
                case "C":
                    //matthew C 可以下拉單位
                    ddl_MZUNIT.Enabled = true;
                    break;
                case "D":
                case "E":
                    //matthew 如果發薪單位是中和分局不鎖單位
                    //if (Session[ConstSession.ADPPAY_AD].ToString() == "382133600C")
                    //{
                    //    ddl_MZUNIT.Enabled = true;                   
                    //}
                    //else
                    //{
                    //    ddl_MZUNIT.Enabled = false;                   
                    //}
                    ddl_MZUNIT.Enabled = false;
                    ddl_MZUNIT.SelectedValue = Session["ADPMZ_UNIT"].ToString();
                    break;
            }
        }

        //TODO : 此部分須修正寫法，並排入工作清單項目中
        ///20150512 Neil
        protected void Button2_Click(object sender, EventArgs e)
        {
            string DeleteString = "DELETE FROM C_DUTYMONTHOVERTIME_HOUR WHERE  MZ_AD='" + DropDownList_AD.SelectedValue + "' AND MZ_UNIT='" + selectUnit + "'  AND MZ_YEAR='" + TextBox_DUTYYEAR.Text.Trim().PadLeft(3, '0') + "' AND MZ_MONTH='" + DropDownList_DUTYMONTH.SelectedValue.Trim().PadLeft(2, '0') + "'";

            o_DBFactory.ABC_toTest.Edit_Data(DeleteString);


            //判斷每月最後天數
            string MonthLastDay = (DateTime.Parse((int.Parse(TextBox_DUTYYEAR.Text.Trim()) + 1911).ToString() + "/" + DropDownList_DUTYMONTH.SelectedValue.Trim().PadLeft(2, '0') + "/" + "01").AddMonths(1).AddDays(-1).Day).ToString();

            string strSQL = "";
            //計算每月超勤總時數（含國定假日設定）
            string Total = "(NVL((SELECT SUM(TOTAL_HOURS-8-ISDIRECTTIME) FROM C_DUTYTABLE_PERSONAL WHERE TOTAL_HOURS>8 AND MZ_ID=B.MZ_ID AND dbo.SUBSTR(DUTYDATE,1,5)='" + TextBox_DUTYYEAR.Text.Trim().PadLeft(3, '0')
                                    + DropDownList_DUTYMONTH.SelectedValue.Trim().PadLeft(2, '0') + "' AND DUTYDATE NOT IN (SELECT MZ_HOLIDAY_DATE FROM C_DUTYHOLIDAY) GROUP BY MZ_ID),0)" +
                           "+" +
                           " NVL((SELECT SUM(TOTAL_HOURS) FROM C_DUTYTABLE_PERSONAL WHERE MZ_ID=B.MZ_ID AND dbo.SUBSTR(DUTYDATE,1,5)='" + TextBox_DUTYYEAR.Text.Trim().PadLeft(3, '0')
                                    + DropDownList_DUTYMONTH.SelectedValue.Trim().PadLeft(2, '0') + "' AND DUTYDATE IN (SELECT MZ_HOLIDAY_DATE FROM C_DUTYHOLIDAY) GROUP BY MZ_ID),0)" +
                           "+" +
                           " NVL((SELECT SUM(ISDIRECTTIME) FROM C_DUTYTABLE_PERSONAL WHERE MZ_ID=B.MZ_ID AND dbo.SUBSTR(DUTYDATE,1,5)='" + TextBox_DUTYYEAR.Text.Trim().PadLeft(3, '0')
                                    + DropDownList_DUTYMONTH.SelectedValue.Trim().PadLeft(2, '0') + "' GROUP BY MZ_ID),0)) ";


            //個人薪俸
            /*  B_SALARY : 俸點及俸級之級距表
             *  origin1 > 俸點 for 警察人員
             *  origin2 > 俸級 for 公務人員
             *  
             *  MZ_SRANK = 使用者職等類型，若開頭為: 
             *  P 為警務人員，使用俸點為單位，故採用 origin1 
             *  G 為政府人員，使用俸級為單位，故採用 origin2
             */
            string PAY1 = @"NVL(
                                 (SELECT 
                                     case dbo.SUBSTR(MZ_SRANK, 0, 1)
                                     when 'P' THEN
                                         (SELECT PAY1 FROM B_SALARY WHERE ORIGIN1 = 
                                             (SELECT CASE WHEN LEN(MZ_SPT)=3 THEN '0'+ MZ_SPT
                                                     ELSE MZ_SPT END
                                              FROM A_DLBASE WHERE MZ_ID=A.MZ_ID
                                             )
                                          )
                                     WHEN 'G' THEN
                                          (SELECT PAY1 FROM B_SALARY WHERE ORIGIN2 = 
                                             (SELECT CASE WHEN LEN(MZ_SPT)=3 THEN '0'+ MZ_SPT
                                                     ELSE MZ_SPT END
                                              FROM A_DLBASE WHERE MZ_ID=A.MZ_ID
                                             )
                                          )
                                     END
                                  FROM  A_DLBASE
                                  WHERE MZ_ID = A.MZ_ID)
                           ,0)";
            //個人專業加給
            string PROFESS = "NVL((SELECT PAY FROM B_PROFESS WHERE \"ID\"=(SELECT MZ_SRANK+'01D' FROM A_DLBASE WHERE MZ_ID=A.MZ_ID)),0)";
            //個人主管加給
            //string BOSS = "NVL((SELECT PAY FROM B_BOSS WHERE \"ID\"=(SELECT MZ_SRANK+'01D' FROM A_DLBASE WHERE MZ_ID=A.MZ_ID AND MZ_PCHIEF IS NOT NULL)),0)";
            //20150514 Neil : 改為由 B_BASE 取值
            string BOSS = "NVL((select BOSSPAY from B_BASE where IDCARD = A.MZ_ID),0)";


            //每小時超勤金額（(薪俸＋專業加給＋主管加給)/240）
            string HOUR_PAY = "ROUND((" + PAY1 + "+" + PROFESS + "+" + BOSS + ")/240,2)";
            //總超勤金額
            string OVERTIME_PAY = "ROUND(" + HOUR_PAY + "*" + Total + ",0)";

            //超勤小時上限
            string HOUR_LIMIT = " CASE " +
                                " WHEN (SELECT MZ_HOUR_LIMIT FROM C_DUTYLIMIT WHERE MZ_ID=A.MZ_ID) IS NOT NULL " +
                                " THEN " +
                                      "(SELECT MZ_HOUR_LIMIT FROM C_DUTYLIMIT WHERE MZ_AD='" + DropDownList_AD.SelectedValue +
                                                                             "' AND MZ_UNIT='" + selectUnit +
                                                                             "' AND MZ_ID=A.MZ_ID) " +

                                " WHEN (SELECT MZ_HOUR_LIMIT FROM C_DUTYLIMIT WHERE MZ_ID=A.MZ_ID) IS NULL AND (SELECT MZ_MONEY_LIMIT FROM C_DUTYLIMIT WHERE MZ_ID=A.MZ_ID) IS NULL" +
                                " THEN " +
                                        " CASE WHEN (SELECT MZ_HOUR_LIMIT FROM C_DUTYLIMIT WHERE MZ_AD= '" + DropDownList_AD.SelectedValue +
                                                                                     "' AND MZ_UNIT='" + selectUnit +
                                                                                     "' AND MZ_ID = 'NULL') IS NOT NULL" +
                                        " THEN " +
                                              "(SELECT MZ_HOUR_LIMIT FROM C_DUTYLIMIT WHERE MZ_AD='" + DropDownList_AD.SelectedValue +
                                                                                     "' AND MZ_UNIT='" + selectUnit +
                                                                                     "' AND MZ_ID = 'NULL') ELSE NULL END" +
                                " ELSE null END";
            //超勤金額上限
            string MONEY_LIMIT = " CASE " +
                                " WHEN (SELECT MZ_MONEY_LIMIT FROM C_DUTYLIMIT WHERE MZ_ID=A.MZ_ID) IS NOT NULL " +
                                " THEN " +
                                      "(SELECT MZ_MONEY_LIMIT FROM C_DUTYLIMIT WHERE MZ_AD='" + DropDownList_AD.SelectedValue +
                                                                             "' AND MZ_UNIT='" + selectUnit +
                                                                             "' AND MZ_ID=A.MZ_ID) " +

                                " WHEN (SELECT MZ_MONEY_LIMIT FROM C_DUTYLIMIT WHERE MZ_ID=A.MZ_ID) IS  NULL AND (SELECT MZ_HOUR_LIMIT FROM C_DUTYLIMIT WHERE MZ_ID=A.MZ_ID) IS NOT NULL" +
                                " THEN " +
                                        " CASE WHEN (SELECT MZ_MONEY_LIMIT FROM C_DUTYLIMIT WHERE  MZ_AD='" + DropDownList_AD.SelectedValue +
                                                                                     "' AND MZ_UNIT='" + selectUnit +
                                                                                     "' AND MZ_ID = 'NULL') IS NOT NULL" +
                                        " THEN " +
                                              "(SELECT MZ_MONEY_LIMIT FROM C_DUTYLIMIT WHERE MZ_AD='" + DropDownList_AD.SelectedValue +
                                                                                     "' AND MZ_UNIT='" + selectUnit +
                                                                                     "' AND MZ_ID = 'NULL') ELSE NULL END" +
                                " ELSE null END";

            string strSQL1 = "''," + Total + "," + Total + ",0,0,0," + PAY1 + "," + PROFESS + "," + BOSS + "," +
                             HOUR_PAY + "," + OVERTIME_PAY + "," + OVERTIME_PAY + "," + OVERTIME_PAY + "," +
                             HOUR_LIMIT + "," + MONEY_LIMIT;


            if (MonthLastDay == "28")
            {
                strSQL = "SELECT DISTINCT B.MZ_ID,'" + TextBox_DUTYYEAR.Text.Trim().PadLeft(3, '0') + "','" + DropDownList_DUTYMONTH.SelectedValue.Trim().PadLeft(2, '0') + "',";
                for (int i = 1; i <= 28; i++)
                {
                    if (i == 28)//每月最後天數28
                    {
                        //計算超勤時數（此處有判斷國定假日！國定假日不扣8小時） 
                        strSQL += " CASE " +
                                  " WHEN (SELECT MZ_HOLIDAY_DATE FROM C_DUTYHOLIDAY" +
                                          " WHERE" +
                                          " MZ_HOLIDAY_DATE='" + TextBox_DUTYYEAR.Text.Trim().PadLeft(3, '0')
                                                               + DropDownList_DUTYMONTH.SelectedValue.Trim().PadLeft(2, '0')
                                                               + i.ToString().PadLeft(2, '0') + "') IS NULL" +
                                   " THEN (SELECT CASE WHEN (TOTAL_HOURS-8)<=0 THEN ISDIRECTTIME ELSE TOTAL_HOURS-8+ISDIRECTTIME END FROM C_DUTYTABLE_PERSONAL " +
                                         " WHERE " +
                                         " MZ_ID=B.MZ_ID AND (TOTAL_HOURS>8 OR ISDIRECTTIME>0) AND DUTYDATE='" + TextBox_DUTYYEAR.Text.Trim().PadLeft(3, '0')
                                                                                     + DropDownList_DUTYMONTH.SelectedValue.Trim().PadLeft(2, '0')
                                                                                     + i.ToString().PadLeft(2, '0') + "' ) " +
                                   " ELSE (SELECT ISDIRECTTIME FROM C_DUTYTABLE_PERSONAL " +
                                         " WHERE " +
                                         " MZ_ID=B.MZ_ID AND DUTYDATE='" + TextBox_DUTYYEAR.Text.Trim().PadLeft(3, '0')
                                                                         + DropDownList_DUTYMONTH.SelectedValue.Trim().PadLeft(2, '0')
                                                                         + i.ToString().PadLeft(2, '0') + "' ) " +
                                   " END,NULL,NULL,NULL," + Total + ",";
                    }
                    else
                    {
                        strSQL += " CASE " +
                                  " WHEN (SELECT MZ_HOLIDAY_DATE FROM C_DUTYHOLIDAY" +
                                          " WHERE" +
                                          " MZ_HOLIDAY_DATE='" + TextBox_DUTYYEAR.Text.Trim().PadLeft(3, '0')
                                                               + DropDownList_DUTYMONTH.SelectedValue.Trim().PadLeft(2, '0')
                                                               + i.ToString().PadLeft(2, '0') + "') IS NULL" +
                                   " THEN (SELECT CASE WHEN (TOTAL_HOURS-8)<=0 THEN ISDIRECTTIME ELSE TOTAL_HOURS-8+ISDIRECTTIME END FROM C_DUTYTABLE_PERSONAL " +
                                         " WHERE " +
                                         " MZ_ID=B.MZ_ID AND (TOTAL_HOURS>8 OR ISDIRECTTIME>0) AND DUTYDATE='" + TextBox_DUTYYEAR.Text.Trim().PadLeft(3, '0')
                                                                                     + DropDownList_DUTYMONTH.SelectedValue.Trim().PadLeft(2, '0')
                                                                                     + i.ToString().PadLeft(2, '0') + "' ) " +
                                   " ELSE (SELECT ISDIRECTTIME FROM C_DUTYTABLE_PERSONAL " +
                                         " WHERE " +
                                         " MZ_ID=B.MZ_ID AND DUTYDATE='" + TextBox_DUTYYEAR.Text.Trim().PadLeft(3, '0')
                                                                         + DropDownList_DUTYMONTH.SelectedValue.Trim().PadLeft(2, '0')
                                                                         + i.ToString().PadLeft(2, '0') + "' ) " +
                                   " END,";
                    }
                }

                strSQL += strSQL1;
                strSQL += ",'N','','', B.MZ_AD, B.MZ_EXAD, B.MZ_UNIT, B.MZ_EXUNIT FROM C_DUTYTABLE_PERSONAL A,A_DLBASE B WHERE A.MZ_ID=B.MZ_ID AND A.DUTYDATE>='" + TextBox_DUTYYEAR.Text.Trim().PadLeft(3, '0') + DropDownList_DUTYMONTH.SelectedValue.Trim().PadLeft(2, '0') + "01'"
                                         + " AND A.DUTYDATE<='" + TextBox_DUTYYEAR.Text.Trim().PadLeft(3, '0') + DropDownList_DUTYMONTH.SelectedValue.Trim().PadLeft(2, '0') + MonthLastDay +
                                              "'  AND B.MZ_AD = '" + DropDownList_AD.SelectedValue + "'  AND B.MZ_AD='" + DropDownList_AD.SelectedValue + "' AND B.MZ_UNIT='" + selectUnit + "'";
                //"'  AND B.PAY_AD = '" + DropDownList_AD.SelectedValue + "'  AND A.MZ_AD='" + DropDownList_AD.SelectedValue + "' AND A.MZ_UNIT='" + selectUnit + "'";
            }
            else if (MonthLastDay == "29")
            {
                strSQL = "SELECT DISTINCT B.MZ_ID,'" + TextBox_DUTYYEAR.Text.Trim().PadLeft(3, '0') + "','" + DropDownList_DUTYMONTH.SelectedValue.Trim().PadLeft(2, '0') + "',";
                for (int i = 1; i <= 29; i++)
                {
                    if (i == 29)
                    {
                        strSQL += " CASE " +
                                  " WHEN (SELECT MZ_HOLIDAY_DATE FROM C_DUTYHOLIDAY" +
                                          " WHERE" +
                                          " MZ_HOLIDAY_DATE='" + TextBox_DUTYYEAR.Text.Trim().PadLeft(3, '0')
                                                               + DropDownList_DUTYMONTH.SelectedValue.Trim().PadLeft(2, '0')
                                                               + i.ToString().PadLeft(2, '0') + "') IS NULL" +
                                   " THEN (SELECT CASE WHEN (TOTAL_HOURS-8)<=0 THEN ISDIRECTTIME ELSE ISDIRECTTIME END FROM C_DUTYTABLE_PERSONAL " +
                                         " WHERE " +
                                         " MZ_ID=B.MZ_ID AND (TOTAL_HOURS>8 OR ISDIRECTTIME>0) AND DUTYDATE='" + TextBox_DUTYYEAR.Text.Trim().PadLeft(3, '0')
                                                                                     + DropDownList_DUTYMONTH.SelectedValue.Trim().PadLeft(2, '0')
                                                                                     + i.ToString().PadLeft(2, '0') + "' ) " +
                                   " ELSE (SELECT ISDIRECTTIME FROM C_DUTYTABLE_PERSONAL " +
                                         " WHERE " +
                                         " MZ_ID=B.MZ_ID AND DUTYDATE='" + TextBox_DUTYYEAR.Text.Trim().PadLeft(3, '0')
                                                                         + DropDownList_DUTYMONTH.SelectedValue.Trim().PadLeft(2, '0')
                                                                         + i.ToString().PadLeft(2, '0') + "' ) " +
                                   " END,0,0," + Total + ",";

                    }
                    else
                    {
                        strSQL += " CASE " +
                                  " WHEN (SELECT MZ_HOLIDAY_DATE FROM C_DUTYHOLIDAY" +
                                          " WHERE" +
                                          " MZ_HOLIDAY_DATE='" + TextBox_DUTYYEAR.Text.Trim().PadLeft(3, '0')
                                                               + DropDownList_DUTYMONTH.SelectedValue.Trim().PadLeft(2, '0')
                                                               + i.ToString().PadLeft(2, '0') + "') IS NULL" +
                                   " THEN (SELECT CASE WHEN (TOTAL_HOURS-8)<=0 THEN ISDIRECTTIME ELSE ISDIRECTTIME END FROM C_DUTYTABLE_PERSONAL " +
                                         " WHERE " +
                                         " MZ_ID=B.MZ_ID AND (TOTAL_HOURS>8 OR ISDIRECTTIME>0) AND DUTYDATE='" + TextBox_DUTYYEAR.Text.Trim().PadLeft(3, '0')
                                                                                     + DropDownList_DUTYMONTH.SelectedValue.Trim().PadLeft(2, '0')
                                                                                     + i.ToString().PadLeft(2, '0') + "' ) " +
                                   " ELSE (SELECT ISDIRECTTIME FROM C_DUTYTABLE_PERSONAL " +
                                         " WHERE " +
                                         " MZ_ID=B.MZ_ID AND DUTYDATE='" + TextBox_DUTYYEAR.Text.Trim().PadLeft(3, '0')
                                                                         + DropDownList_DUTYMONTH.SelectedValue.Trim().PadLeft(2, '0')
                                                                         + i.ToString().PadLeft(2, '0') + "' ) " +
                                   " END,";
                    }
                }
                strSQL += strSQL1;
                strSQL += ",'N','','', B.MZ_AD, B.MZ_EXAD, B.MZ_UNIT, B.MZ_EXUNIT FROM C_DUTYTABLE_PERSONAL A,A_DLBASE B WHERE A.MZ_ID=B.MZ_ID AND A.DUTYDATE>='" + TextBox_DUTYYEAR.Text.Trim().PadLeft(3, '0') + DropDownList_DUTYMONTH.SelectedValue.Trim().PadLeft(2, '0') + "01'"
                                          + " AND A.DUTYDATE<='" + TextBox_DUTYYEAR.Text.Trim().PadLeft(3, '0') + DropDownList_DUTYMONTH.SelectedValue.Trim().PadLeft(2, '0') + MonthLastDay +
                                               "'  AND B.MZ_AD = '" + DropDownList_AD.SelectedValue + "'  AND B.MZ_AD='" + DropDownList_AD.SelectedValue + "' AND B.MZ_UNIT='" + selectUnit + "'";
                //"'  AND B.PAY_AD = '" + DropDownList_AD.SelectedValue + "'  AND A.MZ_AD='" + DropDownList_AD.SelectedValue + "' AND A.MZ_UNIT='" + selectUnit + "'";
            }
            else if (MonthLastDay == "30")
            {



                strSQL = "SELECT DISTINCT B.MZ_ID,'" + TextBox_DUTYYEAR.Text.Trim().PadLeft(3, '0') + "','" + DropDownList_DUTYMONTH.SelectedValue.Trim().PadLeft(2, '0') + "',";




                for (int i = 1; i <= 30; i++)
                {
                    if (i == 30)
                    {
                        strSQL += " CASE " +
                                  " WHEN (SELECT MZ_HOLIDAY_DATE FROM C_DUTYHOLIDAY" +
                                          " WHERE" +
                                          " MZ_HOLIDAY_DATE='" + TextBox_DUTYYEAR.Text.Trim().PadLeft(3, '0')
                                                               + DropDownList_DUTYMONTH.SelectedValue.Trim().PadLeft(2, '0')
                                                               + i.ToString().PadLeft(2, '0') + "') IS NULL" +
                                   " THEN (SELECT CASE WHEN (TOTAL_HOURS-8)<=0 THEN ISDIRECTTIME ELSE ISDIRECTTIME END FROM C_DUTYTABLE_PERSONAL " +
                                         " WHERE " +
                                         " MZ_ID=B.MZ_ID AND (TOTAL_HOURS>8 OR ISDIRECTTIME>0) AND DUTYDATE='" + TextBox_DUTYYEAR.Text.Trim().PadLeft(3, '0')
                                                                                     + DropDownList_DUTYMONTH.SelectedValue.Trim().PadLeft(2, '0')
                                                                                     + i.ToString().PadLeft(2, '0') + "' ) " +
                                   " ELSE (SELECT ISDIRECTTIME FROM C_DUTYTABLE_PERSONAL " +
                                         " WHERE " +
                                         " MZ_ID=B.MZ_ID AND DUTYDATE='" + TextBox_DUTYYEAR.Text.Trim().PadLeft(3, '0')
                                                                         + DropDownList_DUTYMONTH.SelectedValue.Trim().PadLeft(2, '0')
                                                                         + i.ToString().PadLeft(2, '0') + "' ) " +
                                   " END,NULL," + Total + ",";
                    }
                    else
                    {
                        strSQL += " CASE " +
                                  " WHEN (SELECT MZ_HOLIDAY_DATE FROM C_DUTYHOLIDAY" +
                                          " WHERE" +
                                          " MZ_HOLIDAY_DATE='" + TextBox_DUTYYEAR.Text.Trim().PadLeft(3, '0')
                                                               + DropDownList_DUTYMONTH.SelectedValue.Trim().PadLeft(2, '0')
                                                               + i.ToString().PadLeft(2, '0') + "') IS NULL" +
                                   " THEN (SELECT CASE WHEN (TOTAL_HOURS-8)<=0 THEN ISDIRECTTIME ELSE ISDIRECTTIME END FROM C_DUTYTABLE_PERSONAL " +
                                         " WHERE " +
                                         " MZ_ID=B.MZ_ID AND (TOTAL_HOURS>8 OR ISDIRECTTIME>0) AND DUTYDATE='" + TextBox_DUTYYEAR.Text.Trim().PadLeft(3, '0')
                                                                                     + DropDownList_DUTYMONTH.SelectedValue.Trim().PadLeft(2, '0')
                                                                                     + i.ToString().PadLeft(2, '0') + "' ) " +
                                   " ELSE (SELECT ISDIRECTTIME FROM C_DUTYTABLE_PERSONAL " +
                                         " WHERE " +
                                         " MZ_ID=B.MZ_ID AND DUTYDATE='" + TextBox_DUTYYEAR.Text.Trim().PadLeft(3, '0')
                                                                         + DropDownList_DUTYMONTH.SelectedValue.Trim().PadLeft(2, '0')
                                                                         + i.ToString().PadLeft(2, '0') + "' ) " +
                                   " END,";
                    }
                }
                strSQL += strSQL1;
                strSQL += ",'N','','', B.MZ_AD, B.MZ_EXAD, B.MZ_UNIT, B.MZ_EXUNIT FROM C_DUTYTABLE_PERSONAL A,A_DLBASE B WHERE A.MZ_ID=B.MZ_ID AND A.DUTYDATE>='" + TextBox_DUTYYEAR.Text.Trim().PadLeft(3, '0') + DropDownList_DUTYMONTH.SelectedValue.Trim().PadLeft(2, '0') + "01'"
                                          + " AND A.DUTYDATE<='" + TextBox_DUTYYEAR.Text.Trim().PadLeft(3, '0') + DropDownList_DUTYMONTH.SelectedValue.Trim().PadLeft(2, '0') + MonthLastDay +
                                               "'  AND B.MZ_AD = '" + DropDownList_AD.SelectedValue + "'  AND B.MZ_AD='" + DropDownList_AD.SelectedValue + "' AND B.MZ_UNIT='" + selectUnit + "'";

                // AND B.PAY_AD = '" + DropDownList_AD.SelectedValue + "'


            }
            else if (MonthLastDay == "31")
            {
                strSQL = "SELECT DISTINCT B.MZ_ID,'" + TextBox_DUTYYEAR.Text.Trim().PadLeft(3, '0') + "','" + DropDownList_DUTYMONTH.SelectedValue.Trim().PadLeft(2, '0') + "',";

                for (int i = 1; i <= 31; i++)
                {
                    if (i == 31)
                    {
                        strSQL += " CASE " +
                                  " WHEN (SELECT MZ_HOLIDAY_DATE FROM C_DUTYHOLIDAY" +
                                          " WHERE" +
                                          " MZ_HOLIDAY_DATE='" + TextBox_DUTYYEAR.Text.Trim().PadLeft(3, '0')
                                                               + DropDownList_DUTYMONTH.SelectedValue.Trim().PadLeft(2, '0')
                                                               + i.ToString().PadLeft(2, '0') + "') IS NULL" +
                                   " THEN (SELECT CASE WHEN (TOTAL_HOURS-8)<=0 THEN ISDIRECTTIME ELSE ISDIRECTTIME END FROM C_DUTYTABLE_PERSONAL " +
                                         " WHERE " +
                                         " MZ_ID=B.MZ_ID AND (TOTAL_HOURS>8 OR ISDIRECTTIME>0) AND DUTYDATE='" + TextBox_DUTYYEAR.Text.Trim().PadLeft(3, '0')
                                                                                     + DropDownList_DUTYMONTH.SelectedValue.Trim().PadLeft(2, '0')
                                                                                     + i.ToString().PadLeft(2, '0') + "' ) " +
                                   " ELSE (SELECT ISDIRECTTIME FROM C_DUTYTABLE_PERSONAL " +
                                         " WHERE " +
                                         " MZ_ID=B.MZ_ID AND DUTYDATE='" + TextBox_DUTYYEAR.Text.Trim().PadLeft(3, '0')
                                                                         + DropDownList_DUTYMONTH.SelectedValue.Trim().PadLeft(2, '0')
                                                                         + i.ToString().PadLeft(2, '0') + "' )" +
                                   " END," + Total + ",";
                    }
                    else
                    {
                        strSQL += " CASE " +
                                  " WHEN (SELECT MZ_HOLIDAY_DATE FROM C_DUTYHOLIDAY" +
                                          " WHERE" +
                                          " MZ_HOLIDAY_DATE='" + TextBox_DUTYYEAR.Text.Trim().PadLeft(3, '0')
                                                               + DropDownList_DUTYMONTH.SelectedValue.Trim().PadLeft(2, '0')
                                                               + i.ToString().PadLeft(2, '0') + "') IS NULL" +
                                   " THEN (SELECT CASE WHEN (TOTAL_HOURS-8)<=0 THEN ISDIRECTTIME ELSE ISDIRECTTIME END FROM C_DUTYTABLE_PERSONAL " +
                                         " WHERE " +
                                         " MZ_ID=B.MZ_ID AND (TOTAL_HOURS>8 OR ISDIRECTTIME>0) AND DUTYDATE='" + TextBox_DUTYYEAR.Text.Trim().PadLeft(3, '0')
                                                                                     + DropDownList_DUTYMONTH.SelectedValue.Trim().PadLeft(2, '0')
                                                                                     + i.ToString().PadLeft(2, '0') + "' ) " +
                                   " ELSE (SELECT ISDIRECTTIME FROM C_DUTYTABLE_PERSONAL " +
                                         " WHERE " +
                                         " MZ_ID=B.MZ_ID AND DUTYDATE='" + TextBox_DUTYYEAR.Text.Trim().PadLeft(3, '0')
                                                                         + DropDownList_DUTYMONTH.SelectedValue.Trim().PadLeft(2, '0')
                                                                         + i.ToString().PadLeft(2, '0') + "' ) " +
                                   " END,";
                    }
                }

                strSQL += strSQL1;

                //改為撈取使用者全部的單位名稱
                strSQL += ",'N','','',"  //+"'" + DropDownList_AD.SelectedValue + "','','" + selectUnit + "','' " + 
                                        + " B.MZ_AD, B.MZ_EXAD, B.MZ_UNIT, B.MZ_EXUNIT " +

                    " FROM C_DUTYTABLE_PERSONAL A, A_DLBASE B WHERE A.MZ_ID=B.MZ_ID AND A.DUTYDATE>='" + TextBox_DUTYYEAR.Text.Trim().PadLeft(3, '0') + DropDownList_DUTYMONTH.SelectedValue.Trim().PadLeft(2, '0') + "01'"
                                            + " AND A.DUTYDATE<='" + TextBox_DUTYYEAR.Text.Trim().PadLeft(3, '0') + DropDownList_DUTYMONTH.SelectedValue.Trim().PadLeft(2, '0') + MonthLastDay +
                // irk 修改                            "'  AND B.PAY_AD = '" + DropDownList_AD.SelectedValue + "' AND A.MZ_AD='" + DropDownList_AD.SelectedValue + "' AND A.MZ_UNIT='" + selectUnit + "'";
                "' AND B.MZ_AD='" + DropDownList_AD.SelectedValue + "' AND B.MZ_UNIT='" + selectUnit + "'";
            }

            //if (RadioButtonList1.SelectedValue != "全部人員")
            //{
            strSQL += " AND B.MZ_ID NOT IN (SELECT MZ_ID FROM C_DUTYMONTHOVERTIME_HOUR WHERE MZ_AD='" + DropDownList_AD.SelectedValue + "' AND MZ_UNIT='" + selectUnit + "' AND MZ_YEAR='" + TextBox_DUTYYEAR.Text.Trim().PadLeft(3, '0') + "' AND MZ_MONTH='" + DropDownList_DUTYMONTH.SelectedValue.PadLeft(2, '0') + "')";
            //}

            strSQL = "INSERT INTO C_DUTYMONTHOVERTIME_HOUR " + strSQL;

            try
            {
                o_DBFactory.ABC_toTest.Edit_Data(strSQL);
                ///判斷是0的話 刪除
                string delete_string = "DELETE FROM C_DUTYMONTHOVERTIME_HOUR WHERE TOTAL = 0";
                ///塞機關單位（因為前面不想改～）
                //                string updateString = @"UPDATE C_DUTYMONTHOVERTIME_HOUR SET MZ_AD=(SELECT MZ_AD FROM A_DLBASE WHERE MZ_ID=C_DUTYMONTHOVERTIME_HOUR.MZ_ID)
                //                                                                           ,MZ_UNIT=(SELECT MZ_UNIT FROM A_DLBASE WHERE MZ_ID=C_DUTYMONTHOVERTIME_HOUR.MZ_ID)
                //                                                                           ,MZ_EXAD=(SELECT MZ_EXAD FROM A_DLBASE WHERE MZ_ID=C_DUTYMONTHOVERTIME_HOUR.MZ_ID)
                //                                                                           ,MZ_EXUNIT=(SELECT MZ_EXUNIT FROM A_DLBASE WHERE MZ_ID=C_DUTYMONTHOVERTIME_HOUR.MZ_ID) 
                //                                         WHERE MZ_ID IN (SELECT DISTINCT A.MZ_ID FROM C_DUTYTABLE_PERSONAL A,A_DLBASE B WHERE B.PAY_AD = '" + DropDownList_AD.SelectedValue + "' AND A.MZ_AD='" + DropDownList_AD.SelectedValue + "' AND A.MZ_UNIT='" + selectUnit + "' AND dbo.SUBSTR(A.DUTYDATE,0,5)='" + TextBox_DUTYYEAR.Text.Trim().PadLeft(3, '0') + DropDownList_DUTYMONTH.SelectedValue.Trim().PadLeft(2, '0') + "') AND MZ_YEAR='" + TextBox_DUTYYEAR.Text.Trim().PadLeft(3, '0') + "' AND MZ_MONTH='" + DropDownList_DUTYMONTH.SelectedValue.PadLeft(2, '0') + "'";
                o_DBFactory.ABC_toTest.Edit_Data(delete_string);
                //o_DBFactory.ABC_toTest.Edit_Data(updateString);
                ///GRIDVIEW SHOW 當月資料用！
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.alert('產生成功！');location.href='C_ForLeaveOvertime_OVERTIMEHOUR.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "';", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('產生失敗')", true);
            }
        }

        /// <summary>
        /// 產生印領清冊資料
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button2_NewClick(object sender, EventArgs e)
        {
            /*
                參考的資料表:
                C_DUTYMONTHOVERTIME_HOUR(超勤的月結算資料):
                    -刪除/新增
                    -MZ_BUDGET_HOUR:總超勤時數
                C_dutytable_personal(每日的超勤資料)
	                -讀取
                    -CONVERT_REST_HOURS:超勤補休時數
                處理摘要:
                -先刪除條件內員警&年月對應的C_DUTYMONTHOVERTIME_HOUR資料表(超勤的月結算資料)
                -抓取條件內的員警名單
                -foreach逐一處理該機關內所有人員
	                -根據使用者ID&年月,取得該員有超勤資料(C_dutytable_personal)的機關單位,通常是當時調職者會有多筆
	                -foreach逐一處理,上述取得的機關單位
		                -1.計算處理此使用者，本月份每一筆超勤資料
		                -2.INSERT C_DUTYMONTHOVERTIME_HOUR 資料表, 該使用者本月份資料
             */

            String Message = "";                        //儲存訊息
            String ID = txtMZ_ID.Text;                  //輸入 : 身分證字號
            String AD = DropDownList_AD.SelectedValue;  //輸入 : 發薪機關
            String Unit = selectUnit;                   //輸入 : 編制單位
            String Year = TextBox_DUTYYEAR.Text.Trim().PadLeft(3, '0'); //年分
            String Month = DropDownList_DUTYMONTH.SelectedValue.Trim().PadLeft(2, '0'); //月份

            DateTime FullDatetime = new DateTime(Convert.ToInt32(Year) + 1911,
                                                 Convert.ToInt32(Month), 1); //標準 Datetime 參數

            //一般參數
            DataTable dt = new DataTable();
            LogicOvertime Logic = new LogicOvertime();
            List<SqlParameter> lstParameter = new List<SqlParameter>();

            #region 刪除全部舊有資料(依編制)

            ///目前外面表格是寫"發薪機關跟單位"
            ///故統一都是以編制為主
            List<String> WHEREs_Del = new List<String>();

            String MainSQL = @" C_DUTYMONTHOVERTIME_HOUR WHERE ";

            if (!String.IsNullOrEmpty(ID))
                WHEREs_Del.Add(String.Format(@"MZ_ID='{0}'", ID));
            if (!String.IsNullOrEmpty(AD))
                WHEREs_Del.Add(String.Format(@"MZ_AD='{0}'", AD));
            if (!String.IsNullOrEmpty(Unit))
                WHEREs_Del.Add(String.Format(@"MZ_UNIT='{0}'", Unit));
            if (!String.IsNullOrEmpty(Year))
                WHEREs_Del.Add(String.Format(@"MZ_YEAR='{0}'", Year));
            if (!String.IsNullOrEmpty(Month))
                WHEREs_Del.Add(String.Format(@"MZ_MONTH='{0}'", Month));

            if (WHEREs_Del.Count > 0)
            {
                //查詢已審核筆數，用於提示使用者有哪些資料是被審核過的
                String CheckSQL = @"SELECT MZ_ID FROM " + MainSQL;
                CheckSQL += String.Join(" AND ", WHEREs_Del.ToArray());
                CheckSQL += @" AND MZ_VERIFY='Y' ";

                DataTable dt_Verified = o_DBFactory.ABC_toTest.Create_Table(CheckSQL, "C_DUTYMONTHOVERTIME_HOUR");
                foreach (DataRow item in dt_Verified.Rows)
                    Message += String.Format(@"人員 : {0} 之資料已審核，無法變更\n", item["MZ_ID"].ToString());

                //真正刪除時，僅能刪除未審核的
                WHEREs_Del.Add(@"NVL(MZ_VERIFY, 'N') = 'N'");
                String DeleteString = @"DELETE FROM  " + MainSQL;
                DeleteString += String.Join(" AND ", WHEREs_Del.ToArray());

                o_DBFactory.ABC_toTest.Edit_Data(DeleteString);
            }

            #endregion 

            //Step1. 先撈出該編制機關單位的所有人員
            List<String> AdditionalWhere = new List<String>();
            //AdditionalWhere.Add(String.Format(@" {0} like '{1}' ", "MZ_SRANK", "G%")); //僅撈出警職
            //AdditionalWhere.Add(String.Format(@" {0} = '{1}' ", "MZ_STATUS2", "Y")); //僅撈出在職


            ///20150626 雖然畫面上是寫「編制機關」跟「編制單位」，
            ///但為了薪資發放系統，故將機關改為「發薪機關」而非編制機關
            /*List<AccountModel> lstUsers =  AccountService.lookupAccount(ID, AD, Unit,
                                                                        "Pay", //採發薪機關
                                                                        String.Join(" AND ", AdditionalWhere.ToArray()));
            */

            //取得使用者清單
            List<AccountModel> lstUsers = new List<AccountModel>();  // Model 集合
            List<String> lstPersonal = DutyService.Lookup_Personals(AD, Unit, Year + Month, ID); //取得本月份、本發薪機關、本編制單位的所有員工資料

            //目前雖然又透過 foreach 去取得每一個使用者的資料模型
            //但實際上只有使用到該模型的 MZ_ID，其餘欄位均未使用
            foreach (String item in lstPersonal)
            {
                String noneValidID = item;
                if (!String.IsNullOrEmpty(item))
                {
                    //檢查是否為正確身分證字號
                    if (AccountService.isIDRight(noneValidID))
                    {
                        var userModel = AccountService.lookupAccount(item);
                        if (userModel is null || (userModel.ID == "" || userModel.ID is null))
                        {
                            Message += string.Format(@"人員身分證字號錯誤，錯誤字號 : {0}\n\n", item);
                        }
                        else
                        {
                            lstUsers.Add(userModel);
                        }
                    }
                    else
                    {
                        Message += String.Format(@"人員身分證字號錯誤，錯誤字號 : {0}\n\n", item);
                    }
                }
            }

            //逐一處理該機關內所有人員
            foreach (AccountModel item in lstUsers)
            {
                #region 0.取得使用者固定欄位

                //取得使用者之月俸、專業加給、主管加給、每小時支領數等
                _2_salary.Police Police = new TPPDDB._2_salary.Police(item.ID);

                Int32 iPAY1 = Police.salary; //月俸
                Int32 iProfess = Police.profess; //專業加給
                Int32 iBoss = Police.boss; //主管加給
                Int32 iMZ_HOUR_PAY = Convert.ToInt32(Math.Round(Logic.getHourPay(item.ID), 0, MidpointRounding.AwayFromZero)); //每小時超勤支領金額
                                                                                                                               //浮動欄位
                                                                                                                               //Int32 iAllOverTime = ; //最終實際招領金額

                #endregion

                //取得使用者於此月份的所有單位資料
                //依照發薪機關、編制單位、現服機關、現服單位進行 GroupBy
                List<StateDepartmentModel> dicDept = DutyService.Lookup_ExDept(item.ID, Year + Month);

                //若使用者曾經有調過職，則此部分的字典檔會有多組
                foreach (StateDepartmentModel StateDept in dicDept)
                {
                    try
                    {
                        //跳過非本次產生單位
                        if (StateDept.UNIT != Unit) continue;

                        #region 1.處理此使用者，本月份每一筆超勤資料

                        dt.Clear();
                        dt = DutyService.lookupOverTime(item.ID, Year + Month, AD, Unit, StateDept.EX_AD, StateDept.EX_UNIT); //取得每日超勤紀錄(依照編制機關單位)

                        if (item.ID == "A124014481")
                        {

                        }

                        Int32[] Days = new Int32[31];
                        Int32 iMZ_BUDGET_HOUR = 0;
                        Int32 iMZ_REAL_HOUR = 0;
                        Int32 ItemMAdd = 0;

                        int iTotal_CONVERT_REST_HOURS = 0;

                        //開始加總當月每一日的超勤表
                        //金額運算兩者相同，僅有在 ExAD, ExUnit 帶入不同的單位
                        foreach (DataRow r in dt.Rows)
                        {
                            Int32 DayIndex = Convert.ToInt32(r["DUTYDATE"].ToString().Substring(5, 2)) - 1;
                            Int32 iDuty = Convert.ToInt32(r["TOTAL_HOURS"]); //實際上班時數
                            Int32 iOver = Convert.ToInt32(r["ISDIRECTTIME"]); //實際超勤時數
                            //超勤補休時數
                            int iCONVERT_REST_HOURS = r["CONVERT_REST_HOURS"].ToString().SafeToInt();
                            //超勤補休時數,當月累加值
                            iTotal_CONVERT_REST_HOURS += iCONVERT_REST_HOURS;


                            bool IsItemM = false;
                            for (int i = 1; i < 28; i++)
                            {
                                String s = r["DUTYITEM" + i.ToString()].ToString();
                                if (s == "M")
                                {
                                    IsItemM = true;
                                    break;
                                }
                            }
                            if (IsItemM && iDuty > 12)
                            {
                                ItemMAdd += iDuty - 12;
                            }


                            ///超勤輸入「當下」的機關單位，共有兩種情形
                            /// 1.使用者或該使用者承辦人輸入時，此部分的值就等於「當下」該名使用者的編制機關與單位
                            /// 2.若採用「調離單位申請」輸入，此部分的值就等於「當下」登入者的編制機關與單位
                            /// (原先這部分是直接抓取使用者當下在 A_DLBASE 內的值)
                            //SinceAD = r["MZ_AD"].ToString();
                            //SinceUnit = r["MZ_UNIT"].ToString();
                            //SinceEXAD = r["MZ_EXAD"].ToString();
                            //SinceEXUNIT = r["MZ_EXUNIT"].ToString();

                            //儲存當天所使用之"實際支領超勤數"
                            Days[DayIndex] = iOver + iCONVERT_REST_HOURS;

                            ///Joy 超勤印領清冊規則
                            int IAmount = 0;
                            if (iDuty > 8 && iDuty <= 12)
                            {
                                IAmount = iOver - (iDuty - 8);
                            }
                            else if (iDuty > 12)
                            {
                                IAmount = iOver - 4;
                            }
                            else
                            {
                                IAmount = iOver;
                            }

                            //加總本月數字

                            if (iDuty > 8)
                            {
                                iDuty = iDuty - 8 + IAmount;
                            }
                            else
                            {
                                iDuty = iDuty + IAmount;
                            }

                            //Month_DutyTimeHour += iDuty; //上班時數
                            /// 上班/超勤時數現改為超勤時數總額
                            /// 20150602
                            iMZ_BUDGET_HOUR += iOver;
                            iMZ_REAL_HOUR += iOver; //實際招領時數
                        }

                        //嘗試取得使用者之時數及金額上限限制條件
                        Int32 MoneyLimit = 0;
                        Int32 HourLimit = 0;
                        DataTable dt_Limit = DutyService.lookupOverTime_Limit(item.ID, AD, Unit); //增加發薪機關及編制單位條件來因應人員多筆超勤時數上限 20180615 by sky
                        if (dt_Limit.Rows.Count > 0)
                        {
                            DataRow dr_Limit = dt_Limit.Rows[0];
                            MoneyLimit = Convert.ToInt32(dr_Limit["MZ_MONEY_LIMIT"]);
                            HourLimit = Convert.ToInt32(dr_Limit["MZ_HOUR_LIMIT"]);

                            //判斷小時是否超過上限
                            if (iMZ_REAL_HOUR > HourLimit) iMZ_REAL_HOUR = HourLimit;
                        }

                        //超勤金額總額計算
                        //邏輯 : 因其上限小時已在上限設定那邊進行除法過，讓其總額不會大於 17000
                        Int32 iMZ_OVERTIME_PAY = iMZ_HOUR_PAY * iMZ_REAL_HOUR; //每小時支付金額 * 總時數

                        //但因超勤金額上限之小時數時，計算上採用無條件進位，故會有多餘的小數點導致值大於 17000 
                        //(ex: HourPay 為 288，17000 / 288 = 59.027 ≒ 60；但 60 * 288 = 17280；故仍需要進行過濾) 
                        iMZ_OVERTIME_PAY = (iMZ_OVERTIME_PAY > MoneyLimit) ? MoneyLimit : iMZ_OVERTIME_PAY;

                        #endregion

                        if (iMZ_BUDGET_HOUR == 0 && iMZ_REAL_HOUR == 0) continue; //若本月份此人無任何超勤資料及上班時數，則略過資料儲存階段

                        #region 2.INSERT 該使用者本月份資料

                        String DutyUserClassify = AccountService.getDutyUserClassify_V1(item.ID).ToString();
                        //特殊邏輯:
                        /*
                         如果執行時年度>=112修正以下計算流程。
                        如果判斷後為「Inner」且MZ_BUDGET_HOUR計算>60者，新增資料的 MZ_BUDGET_HOUR 一律塞60，
                        MZ_REAL_HOUR計算方式不變(但如果>60者，也一律以60計算)，MZ_BALANCE_HOUR一律塞0。
                        另外，如果判斷是Outter或INNER但MZ_BUDGET_HOUR<=60(基本上就是其他所有的態樣)，計算及新增都還是跑原先的流程。
                         */
                        /*
                         先不要了
                       //年度
                       int iYear = 0;
                       int.TryParse(Year, out iYear);
                       //如果執行時年度>=112 判斷後為「Inner」
                       if (iYear >= 112 && DutyUserClassify == "Inner")
                       {
                           //MZ_BUDGET_HOUR
                           if (Month_DutyTimeHour > 60)
                               Month_DutyTimeHour = 60;
                           //MZ_REAL_HOUR
                           if (Month_RealOverTimeHour > 60)
                               Month_RealOverTimeHour = 60;
                       }
                       */
                        lstParameter.Clear();
                        lstParameter.Add(new SqlParameter(":MZ_ID", SqlDbType.NVarChar) { Value = item.ID });
                        lstParameter.Add(new SqlParameter(":MZ_YEAR", SqlDbType.NVarChar) { Value = Year });
                        lstParameter.Add(new SqlParameter(":MZ_MONTH", SqlDbType.NVarChar) { Value = Month });
                        lstParameter.Add(new SqlParameter(":D1", SqlDbType.Float) { Value = Days[0] });
                        lstParameter.Add(new SqlParameter(":D2", SqlDbType.Float) { Value = Days[1] });
                        lstParameter.Add(new SqlParameter(":D3", SqlDbType.Float) { Value = Days[2] });
                        lstParameter.Add(new SqlParameter(":D4", SqlDbType.Float) { Value = Days[3] });
                        lstParameter.Add(new SqlParameter(":D5", SqlDbType.Float) { Value = Days[4] });
                        lstParameter.Add(new SqlParameter(":D6", SqlDbType.Float) { Value = Days[5] });
                        lstParameter.Add(new SqlParameter(":D7", SqlDbType.Float) { Value = Days[6] });
                        lstParameter.Add(new SqlParameter(":D8", SqlDbType.Float) { Value = Days[7] });
                        lstParameter.Add(new SqlParameter(":D9", SqlDbType.Float) { Value = Days[8] });
                        lstParameter.Add(new SqlParameter(":D10", SqlDbType.Float) { Value = Days[9] });
                        lstParameter.Add(new SqlParameter(":D11", SqlDbType.Float) { Value = Days[10] });
                        lstParameter.Add(new SqlParameter(":D12", SqlDbType.Float) { Value = Days[11] });
                        lstParameter.Add(new SqlParameter(":D13", SqlDbType.Float) { Value = Days[12] });
                        lstParameter.Add(new SqlParameter(":D14", SqlDbType.Float) { Value = Days[13] });
                        lstParameter.Add(new SqlParameter(":D15", SqlDbType.Float) { Value = Days[14] });
                        lstParameter.Add(new SqlParameter(":D16", SqlDbType.Float) { Value = Days[15] });
                        lstParameter.Add(new SqlParameter(":D17", SqlDbType.Float) { Value = Days[16] });
                        lstParameter.Add(new SqlParameter(":D18", SqlDbType.Float) { Value = Days[17] });
                        lstParameter.Add(new SqlParameter(":D19", SqlDbType.Float) { Value = Days[18] });
                        lstParameter.Add(new SqlParameter(":D20", SqlDbType.Float) { Value = Days[19] });
                        lstParameter.Add(new SqlParameter(":D21", SqlDbType.Float) { Value = Days[20] });
                        lstParameter.Add(new SqlParameter(":D22", SqlDbType.Float) { Value = Days[21] });
                        lstParameter.Add(new SqlParameter(":D23", SqlDbType.Float) { Value = Days[22] });
                        lstParameter.Add(new SqlParameter(":D24", SqlDbType.Float) { Value = Days[23] });
                        lstParameter.Add(new SqlParameter(":D25", SqlDbType.Float) { Value = Days[24] });
                        lstParameter.Add(new SqlParameter(":D26", SqlDbType.Float) { Value = Days[25] });
                        lstParameter.Add(new SqlParameter(":D27", SqlDbType.Float) { Value = Days[26] });
                        lstParameter.Add(new SqlParameter(":D28", SqlDbType.Float) { Value = Days[27] });
                        lstParameter.Add(new SqlParameter(":D29", SqlDbType.Float) { Value = Days[28] });
                        lstParameter.Add(new SqlParameter(":D30", SqlDbType.Float) { Value = Days[29] });
                        lstParameter.Add(new SqlParameter(":D31", SqlDbType.Float) { Value = Days[30] });
                        //MZ_BUDGET_HOUR= 每日的 ISDIRECTTIME + 
                        lstParameter.Add(new SqlParameter(":MZ_BUDGET_HOUR", SqlDbType.Float) { Value = iMZ_BUDGET_HOUR + iTotal_CONVERT_REST_HOURS });
                        lstParameter.Add(new SqlParameter(":MZ_REAL_HOUR", SqlDbType.Float) { Value = iMZ_REAL_HOUR });
                        lstParameter.Add(new SqlParameter(":MZ_BALANCE_HOUR", SqlDbType.Float) { Value = iMZ_BUDGET_HOUR - iMZ_REAL_HOUR + ItemMAdd });
                        lstParameter.Add(new SqlParameter(":PAY1", SqlDbType.Float) { Value = iPAY1 });
                        lstParameter.Add(new SqlParameter(":PROFESS", SqlDbType.Float) { Value = iProfess });
                        lstParameter.Add(new SqlParameter(":BOSS", SqlDbType.Float) { Value = iBoss });
                        lstParameter.Add(new SqlParameter(":MZ_HOUR_PAY", SqlDbType.Float) { Value = iMZ_HOUR_PAY });
                        lstParameter.Add(new SqlParameter(":MZ_OVERTIME_PAY", SqlDbType.Float) { Value = iMZ_OVERTIME_PAY });
                        lstParameter.Add(new SqlParameter(":MZ_AD", SqlDbType.NVarChar) { Value = AD });
                        lstParameter.Add(new SqlParameter(":MZ_EXAD", SqlDbType.NVarChar) { Value = StateDept.EX_AD });
                        lstParameter.Add(new SqlParameter(":MZ_UNIT", SqlDbType.NVarChar) { Value = Unit });
                        lstParameter.Add(new SqlParameter(":MZ_EXUNIT", SqlDbType.NVarChar) { Value = StateDept.EX_UNIT });
                        lstParameter.Add(new SqlParameter(":DUTYUSERCLASSIFY", SqlDbType.NVarChar) { Value = DutyUserClassify });

                        String ErrMsg = "", strSQL = "";
                        Boolean isInsertSuccess = DutyService.Insert_OverTimeStatistics(lstParameter, out strSQL, ref ErrMsg);

                        if (!isInsertSuccess)
                            Message += String.Format(@"人員 : {0}, (AD:{1}, UNIT:{2}, EXUINT:{3})\n\n", item.ID, AD, Unit, StateDept.EX_UNIT);

                        //進行Log紀錄 20190212 by sky
                        LogModel.saveLog("CDH", "A", strSQL, lstParameter, Request.QueryString["TPM_FION"]
                            , isInsertSuccess ? string.Format("人員 : {0}, (AD:{1}, UNIT:{2}, EXUINT:{3}) 每月超勤時數表產生。", item.ID, AD, Unit, StateDept.EX_UNIT) :
                            string.Format("人員 : {0}, 於插入資料至資料庫時發生錯誤，參考錯誤訊息 : {1} (AD:{2}, UNIT:{3}, EXUINT:{4})", item.ID, ErrMsg.Replace('\r', ' ').Replace('\n', ' '), AD, Unit, StateDept.EX_UNIT));
                        #endregion

                    }
                    catch (Exception ex)
                    {
                        Message += String.Format(@"人員 : {0}, (AD:{1}, UNIT:{2}, EXUINT:{3})\n\n", item.ID, AD, Unit, StateDept.EX_UNIT);

                        //進行Log紀錄 20190212 by sky
                        LogModel.saveLog("CDH", "A", "", lstParameter, Request.QueryString["TPM_FION"]
                            , string.Format("人員 : {0}, 於插入資料至資料庫時發生錯誤，參考錯誤訊息 : {1} (AD:{2}, UNIT:{3}, EXUINT:{4})", item.ID, ex.Message.Replace('\r', ' ').Replace('\n', ' '), AD, Unit, StateDept.EX_UNIT));
                    }
                }

            } //end foreach

            if (String.IsNullOrEmpty(Message))
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.alert('產生完成！');location.href='C_ForLeaveOvertime_OVERTIMEHOUR.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "';", true);
            else
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.alert('產生完成！ 但下列人員資料未重新產生:\\n\\n" + Message + "');location.href='C_ForLeaveOvertime_OVERTIMEHOUR.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "';", true);
        }

        protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
        {
            string MonthLastDay = "";
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (!string.IsNullOrEmpty(DropDownList_DUTYMONTH.SelectedValue))
                {
                    if (!string.IsNullOrEmpty(TextBox_DUTYYEAR.Text))
                    {
                        MonthLastDay = (DateTime.Parse((int.Parse(TextBox_DUTYYEAR.Text.Trim()) + 1911).ToString() + "/" + DropDownList_DUTYMONTH.SelectedValue.Trim().PadLeft(2, '0') + "/" + "01").AddMonths(1).AddDays(-1).Day).ToString();
                    }
                }
                GridView gv = (GridView)sender;
                e.Row.Cells.Clear();
                GridViewRow gvRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

                TableCell tc0 = new TableCell();
                tc0.Text = "姓名";
                gvRow.Cells.Add(tc0);

                TableCell tc1 = new TableCell();
                tc1.Text = "1";
                gvRow.Cells.Add(tc1);

                TableCell tc2 = new TableCell();
                tc2.Text = "2";
                gvRow.Cells.Add(tc2);

                TableCell tc3 = new TableCell();
                tc3.Text = "3";
                gvRow.Cells.Add(tc3);

                TableCell tc4 = new TableCell();
                tc4.Text = "4";
                gvRow.Cells.Add(tc4);

                TableCell tc5 = new TableCell();
                tc5.Text = "5";
                gvRow.Cells.Add(tc5);

                TableCell tc6 = new TableCell();
                tc6.Text = "6";
                gvRow.Cells.Add(tc6);

                TableCell tc7 = new TableCell();
                tc7.Text = "7";
                gvRow.Cells.Add(tc7);

                TableCell tc8 = new TableCell();
                tc8.Text = "8";
                gvRow.Cells.Add(tc8);

                TableCell tc9 = new TableCell();
                tc9.Text = "9";
                gvRow.Cells.Add(tc9);

                TableCell tc10 = new TableCell();
                tc10.Text = "10";
                gvRow.Cells.Add(tc10);

                TableCell tc11 = new TableCell();
                tc11.Text = "11";
                gvRow.Cells.Add(tc11);

                TableCell tc12 = new TableCell();
                tc12.Text = "12";
                gvRow.Cells.Add(tc12);

                TableCell tc13 = new TableCell();
                tc13.Text = "13";
                gvRow.Cells.Add(tc13);

                TableCell tc14 = new TableCell();
                tc14.Text = "14";
                gvRow.Cells.Add(tc14);

                TableCell tc15 = new TableCell();
                tc15.Text = "15";
                gvRow.Cells.Add(tc15);

                TableCell tc16 = new TableCell();
                tc16.Text = "16";
                gvRow.Cells.Add(tc16);

                TableCell tc17 = new TableCell();
                tc17.Text = "17";
                gvRow.Cells.Add(tc17);

                TableCell tc18 = new TableCell();
                tc18.Text = "18";
                gvRow.Cells.Add(tc18);

                TableCell tc19 = new TableCell();
                tc19.Text = "19";
                gvRow.Cells.Add(tc19);

                TableCell tc20 = new TableCell();
                tc20.Text = "20";
                gvRow.Cells.Add(tc20);

                TableCell tc21 = new TableCell();
                tc21.Text = "21";
                gvRow.Cells.Add(tc21);

                TableCell tc22 = new TableCell();
                tc22.Text = "22";
                gvRow.Cells.Add(tc22);

                TableCell tc23 = new TableCell();
                tc23.Text = "23";
                gvRow.Cells.Add(tc23);

                TableCell tc24 = new TableCell();
                tc24.Text = "24";
                gvRow.Cells.Add(tc24);

                TableCell tc25 = new TableCell();
                tc25.Text = "25";
                gvRow.Cells.Add(tc25);

                TableCell tc26 = new TableCell();
                tc26.Text = "26";
                gvRow.Cells.Add(tc26);

                TableCell tc27 = new TableCell();
                tc27.Text = "27";
                gvRow.Cells.Add(tc27);

                TableCell tc28 = new TableCell();
                tc28.Text = "28";
                gvRow.Cells.Add(tc28);

                TableCell tc29 = new TableCell();
                tc29.Text = "29";
                gvRow.Cells.Add(tc29);

                TableCell tc30 = new TableCell();
                tc30.Text = "30";
                gvRow.Cells.Add(tc30);

                TableCell tc31 = new TableCell();
                tc31.Text = "31";
                gvRow.Cells.Add(tc31);

                TableCell tc32 = new TableCell();
                tc32.Text = "合計";
                gvRow.Cells.Add(tc32);

                gv.Controls[0].Controls.AddAt(0, gvRow);
            }
        }

        protected void GV1_DataBind()
        {
            string SelectSQL = "SELECT (SELECT MZ_NAME FROM A_DLBASE WHERE MZ_ID=C_DUTYMONTHOVERTIME_HOUR.MZ_ID ),";

            for (int j = 1; j <= 31; j++)
            {
                SelectSQL += "\"" + j.ToString() + "\",";
            }

            SelectSQL += " TOTAL FROM C_DUTYMONTHOVERTIME_HOUR WHERE MZ_YEAR='" + TextBox_DUTYYEAR.Text.Trim().PadLeft(3, '0') +
                                                              "' AND MZ_MONTH='" + DropDownList_DUTYMONTH.SelectedValue.Trim().PadLeft(2, '0') +
                                                              //"' AND MZ_AD='" + Session["ADPMZ_AD"].ToString() + "' AND MZ_UNIT='" + Session["ADPMZ_UNIT"].ToString() + "'";
                                                              "' AND MZ_AD='" + DropDownList_AD.SelectedValue + "' AND MZ_UNIT='" + ddl_MZUNIT.SelectedValue + "'";

            DataTable dt = o_DBFactory.ABC_toTest.Create_Table(SelectSQL, "GET");
            if (dt.Rows.Count > 0)
            {
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
        }

        protected void DropDownList_DUTYMONTH_SelectedIndexChanged(object sender, EventArgs e)
        {
            GV1_DataBind();
        }

        protected void DropDownList_AD_SelectedIndexChanged(object sender, EventArgs e)
        {
            UNIT();

        }
    }
}
