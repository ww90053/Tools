using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using TPPDDB.Helpers;
using TPPDDB.Model.Const;

namespace TPPDDB._3_forleave
{
    public partial class C_OVERTIMEINSIDE_TIMEPAYTOTAL_rpt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                C.check_power();

                ViewState["C_strGID"] = o_a_Function.strGID(Session["ADPMZ_ID"].ToString());

                chk_TPMGroup();
            }
        }

        /// <summary>
        /// 依群組權限建立機關選單內容
        /// </summary>
        protected void chk_TPMGroup()
        {
            string premission = ViewState["C_strGID"].ToString();
            bool IsNewZhonghe = false;
            switch (premission)
            {
                case "A":
                case "B":
                    GetMZ_AD_List(1, ref IsNewZhonghe);
                    break;
                case "C":
                case "D":
                    GetMZ_AD_List(2, ref IsNewZhonghe);
                    break;
                case "E":
                    GetMZ_AD_List(2, ref IsNewZhonghe);
                    if (!IsNewZhonghe)
                    {
                        DropDownList_MZ_AD.Enabled = false;
                    }
                    break;
                default:
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click",
                        "alert('此人員查無超勤之權限資料，無法使用此功能'); window.location.href='/10-knowledge/J_Login_after.aspx?TPM_FION=-1';", true);
                    return;
            }

            try
            {
                ///20150629 此部分原先是抓現服機關單位為預設值
                ///但因機關改為抓取發薪機關後，若仍採用現服，會導致現服單位找不到
                ///(發薪機關與編制機關其所屬單位必定有；但發薪機關與現服單位不一定對得起來)
                ///故預設值均採用發薪機關及編制單位，避免程式出錯
                string AD = Session[ConstSession.ADPPAY_AD].ToString(); //20150629 改為發薪機關優先
                DropDownList_MZ_AD.SelectedValue = AD;
            }
            catch (Exception)
            {
                DropDownList_MZ_AD.SelectedValue = "";
            }
        }

        /// <summary>
        /// 取得機關List內容
        /// </summary>
        /// <param name="Mode"></param>
        private void GetMZ_AD_List(int Mode, ref bool IsNewZhonghe)
        {
            DataTable dt;
            string SQL = "";
            string SQL_IN = "";

            //AB權限會帶入1
            if (Mode == 1)
            {
                dt = o_DBFactory.ABC_toTest.Create_Table("  (SELECT  MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE = '04' AND MZ_KCODE LIKE '38213%') ", "GET");
            }
            else
            {
                //帶入三個單位資料 編制機關 現服機關 發薪機關 
                List<String> Depts = new List<String>();
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

                if (Depts.Count > 0)
                {
                    SQL_IN = String.Join("', '", Depts.ToArray());
                    SQL_IN = String.Format(@" AND (MZ_KCODE IN ('{0}'))", SQL_IN);
                }

                //matthew 如果三個單位裡面有中和分局 要帶出中和一&中和二
                if (Depts.Where(x => x.Contains("382133600C")).Count() > 0)
                {
                    Depts.AddRange(new List<string>() { "382133400C", "382133500C" });
                    string dept_List = String.Join("', '", Depts.ToArray());
                    SQL_IN = String.Format(@" AND (MZ_KCODE IN ('{0}'))", dept_List);
                    IsNewZhonghe = true;
                }
                SQL = @"SELECT MZ_KCHI, MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE='04' AND MZ_KCODE LIKE '38213%'  " + SQL_IN;
                dt = o_DBFactory.ABC_toTest.Create_Table(SQL, "GET");
            }

            DropDownList_MZ_AD.DataSource = dt;
            DropDownList_MZ_AD.DataBind();
        }

        /// <summary>
        /// 列印按鈕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btPrintExcel_Click(object sender, EventArgs e)
        {
            //判斷query是否正確
            if (DropDownList_MZ_AD.SelectedIndex == 0 || DropDownList_MZ_AD.SelectedValue == "")
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('機關未選擇');", true);
                return;
            }

            switch (RadioButtonList_TYPE.SelectedValue)
            {
                case "1":
                    //業務報表
                    GetInsideData();
                    break;
                case "2":
                    //超勤報表
                    GetOverTimeData();
                    break;
                default:
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('未選擇列印項目');", true);
                    break;
            }
        }
        /// <summary>
        /// 取得業務加班時數、金額資料
        /// </summary>
        private void GetInsideData()
        {
            string SQL = @"Select AKK.MZ_KCHI
                            , SUM(case when COTHI.RESTFLAG != 'YU' then COTHI.OTIME else 0 END) TOTLETIME
                            , SUM(case when COTHI.RESTFLAG = 'N' OR COTHI.RESTFLAG IS NULL then COTHI.OTIME else 0 END) NTIME
                            , SUM(case when COTHI.RESTFLAG = 'YO' OR COTHI.RESTFLAG = 'YD' then COTHI.OTIME else 0 END) YOYDTIME
                            , SUM(case when COTHI.RESTFLAG = 'N' OR COTHI.RESTFLAG IS NULL then COTHI.PAY_SUM else 0 END) TOTLEPAY 
                            From C_OVERTIME_HOUR_INSIDE COTHI
                            left join A_DLBASE AD on AD.MZ_ID = COTHI.MZ_ID
                            left join A_KTYPE AKS on AKS.MZ_KCODE = AD.MZ_SRANK AND AKS.MZ_KTYPE = '09'
                            left join A_KTYPE AKK on AKK.MZ_KCODE = COTHI.MZ_EXUNIT AND AKK.MZ_KTYPE = '25' ";
            //機關判斷
            string WHERE = @"Where 1=1 ";
            WHERE += @"AND COTHI.MZ_OCCC!='Z011' AND COTHI.MZ_EXAD='" + DropDownList_MZ_AD.SelectedValue + "' ";

            //職等判斷
            if (DropDownList_MZ_SRANK.SelectedValue != "0")
            {
                switch (DropDownList_MZ_SRANK.SelectedValue.ToString())
                {
                    case "1":
                        WHERE += @"AND (AKS.MZ_KCODE like 'G%') ";
                        break;
                    case "2":
                        WHERE += @"AND (AKS.MZ_KCODE like 'P%' OE AKS.MZ_KCODE like 'B%') ";
                        break;
                    case "3":
                        WHERE += @"AND (AKS.MZ_KCODE like 'J%' OR AKS.MZ_KCODE like 'M%') ";
                        break;
                    default:
                        break;
                }
            }

            //日期判斷
            string printStr = "";
            if (!string.IsNullOrEmpty(TextBox_DATE_S.Text.Trim()) && !string.IsNullOrEmpty(TextBox_DATE_E.Text.Trim()))
            {
                TextBox_DATE_S.Text = TextBox_DATE_S.Text.PadLeft(TextBox_DATE_S.MaxLength, '0');
                TextBox_DATE_E.Text = TextBox_DATE_E.Text.PadLeft(TextBox_DATE_E.MaxLength, '0');

                WHERE += @"AND COTHI.MZ_DATE between '" + o_str.tosql(TextBox_DATE_S.Text.Trim()) + "' AND '" +
                    o_str.tosql(TextBox_DATE_E.Text.Trim()) + "' ";

                printStr = o_str.RCDateConvert(TextBox_DATE_S.Text, "{0}年{1}月{2}日", true) + "至" +
                    o_str.RCDateConvert(TextBox_DATE_E.Text, "{0}年{1}月{2}日", true);
            }
            else if (!string.IsNullOrEmpty(TextBox_DATE_S.Text.Trim()))
            {
                TextBox_DATE_S.Text = TextBox_DATE_S.Text.PadLeft(TextBox_DATE_S.MaxLength, '0');

                WHERE += @"AND COTHI.MZ_DATE >= '" + o_str.tosql(TextBox_DATE_S.Text.Trim()) + "' ";

                printStr = o_str.RCDateConvert(TextBox_DATE_S.Text, "{0}年{1}月{2}日", true) + "至今";
            }
            else if (TextBox_DATE_E.Text != "")
            {
                //暫時限制起始日必填，此段不會使用到
                TextBox_DATE_E.Text = TextBox_DATE_E.Text.PadLeft(TextBox_DATE_E.MaxLength, '0');
                WHERE += @"AND COTHI.MZ_DATE <= '" + o_str.tosql(TextBox_DATE_E.Text.Trim()) + "' ";

                printStr = "至" + o_str.RCDateConvert(TextBox_DATE_E.Text, "{0}年{1}月{2}日", true);
            }

            string OGBY = @"group by AKK.MZ_KCHI,COTHI.MZ_EXUNIT
                            order by COTHI.MZ_EXUNIT ";

            DataTable tempDT = o_DBFactory.ABC_toTest.Create_Table(SQL + WHERE + OGBY, "GET");

            if (tempDT.Rows.Count > 0)
            {
                //計算總計資料
                string sumSQL = string.Format(@"SELECT '總計' MZ_KCHI, SUM(t.TOTLETIME) TOTLETIME, SUM(t.NTIME) NTIME, SUM(t.YOYDTIME) YOYDTIME
                                , SUM(t.TOTLEPAY) TOTLEPAY FROM ({0}) t", SQL + WHERE + OGBY);
                DataTable sumTempDT = o_DBFactory.ABC_toTest.Create_Table(sumSQL, "GET");

                tempDT.ImportRow(sumTempDT.Rows[0]);

                tempDT.Columns[0].ColumnName = "單位";
                tempDT.Columns[1].ColumnName = "申請加班時數";
                tempDT.Columns[2].ColumnName = "支領加班費時數";
                tempDT.Columns[3].ColumnName = "加班補休時數";
                tempDT.Columns[4].ColumnName = "支領金額";

                //建立表格表頭
                List<OfficeHelpers.ExcelHelpers.FormatColumn> exceFormat = new List<OfficeHelpers.ExcelHelpers.FormatColumn>();
                var t1 = new OfficeHelpers.ExcelHelpers.FormatColumn(0, 0, 0, 4, string.Format("{0} 業務加班費時數及金額統計表", DropDownList_MZ_AD.SelectedItem.Text))
                {
                    Option = new OfficeHelpers.ExcelHelpers.CellStyle() { CenterText = true }
                };
                exceFormat.Add(t1);
                var t2 = new OfficeHelpers.ExcelHelpers.FormatColumn(1, 0, 1, 4, string.Format("期間：{0}", printStr))
                {
                    Option = new OfficeHelpers.ExcelHelpers.CellStyle() { CenterText = true }
                };
                exceFormat.Add(t2);

                new OfficeHelpers.ExcelHelpers().DtToExcelForXLS(tempDT, "業務加班報表", true, exceFormat.ToArray());
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "alert('無此日期之相關資料');", true);
            }
        }
        /// <summary>
        /// 取得超勤加班時數、金額資料
        /// </summary>
        private void GetOverTimeData()
        {
            string SQL = @"Select AKK.MZ_KCHI
                            , SUM(CDH.MZ_BUDGET_HOUR) TOTLETIME
                            , SUM(CDH.MZ_REAL_HOUR) REALTIME
                            , SUM(CDH.MZ_BALANCE_HOUR) BALANCETIME
                            , SUM(CDH.MZ_OVERTIME_PAY) TOTLEPAY
                            From C_DUTYMONTHOVERTIME_HOUR CDH
                            left join A_DLBASE AD on AD.MZ_ID = CDH.MZ_ID
                            left join A_KTYPE AKS on AKS.MZ_KCODE = AD.MZ_SRANK AND AKS.MZ_KTYPE = '09'
                            left join A_KTYPE AKK on AKK.MZ_KCODE = CDH.MZ_EXUNIT AND AKK.MZ_KTYPE = '25' ";
            //機關判斷
            string WHERE = @"Where 1=1 ";
            WHERE += @"AND CDH.MZ_EXAD='" + DropDownList_MZ_AD.SelectedValue + "' ";

            //職等判斷
            if (DropDownList_MZ_SRANK.SelectedValue != "0")
            {
                switch (DropDownList_MZ_SRANK.SelectedValue.ToString())
                {
                    case "1":
                        WHERE += @"AND (AKS.MZ_KCODE like 'G%') ";
                        break;
                    case "2":
                        WHERE += @"AND (AKS.MZ_KCODE like 'P%' OE AKS.MZ_KCODE like 'B%') ";
                        break;
                    case "3":
                        WHERE += @"AND (AKS.MZ_KCODE like 'J%' OR AKS.MZ_KCODE like 'M%') ";
                        break;
                    default:
                        break;
                }
            }

            //日期判斷
            string printStr = "";
            if (!string.IsNullOrEmpty(TextBox_DATE_S.Text.Trim()) && !string.IsNullOrEmpty(TextBox_DATE_E.Text.Trim()))
            {
                TextBox_DATE_S.Text = TextBox_DATE_S.Text.PadLeft(TextBox_DATE_S.MaxLength, '0');
                TextBox_DATE_E.Text = TextBox_DATE_E.Text.PadLeft(TextBox_DATE_E.MaxLength, '0');

                WHERE += string.Format("AND ((CDH.MZ_YEAR + 1911) + '-' + CDH.MZ_MONTH + '-01') BETWEEN '{0}' AND '{1}' "
                                        , o_str.RCDateConvert(TextBox_DATE_S.Text, "yyyy-MM-dd", false)
                                        , o_str.RCDateConvert(TextBox_DATE_E.Text, "yyyy-MM-dd", false));

                printStr = o_str.RCDateConvert(TextBox_DATE_S.Text, "{0}年{1}月", true) + "至" +
                    o_str.RCDateConvert(TextBox_DATE_E.Text, "{0}年{1}月", true);
            }
            else if (!string.IsNullOrEmpty(TextBox_DATE_S.Text.Trim()))
            {
                TextBox_DATE_S.Text = TextBox_DATE_S.Text.PadLeft(TextBox_DATE_S.MaxLength, '0');
                WHERE += "AND CDH.MZ_YEAR >='" + o_str.tosql(TextBox_DATE_S.Text.Substring(0, 3).Trim()) + "' "
                    + "AND CDH.MZ_MONTH >='" + o_str.tosql(TextBox_DATE_S.Text.Substring(3, 2).Trim()) + "' ";

                printStr = o_str.RCDateConvert(TextBox_DATE_S.Text, "{0}年{1}月", true) + "至本月";
            }
            else if (TextBox_DATE_E.Text != "")
            {
                //暫時限制起始日必填，此段不會使用到
                TextBox_DATE_E.Text = TextBox_DATE_E.Text.PadLeft(TextBox_DATE_E.MaxLength, '0');
                WHERE += "AND CDH.MZ_YEAR <='" + o_str.tosql(TextBox_DATE_E.Text.Substring(0, 3).Trim()) + "' "
                    + "AND CDH.MZ_MONTH <='" + o_str.tosql(TextBox_DATE_E.Text.Substring(3, 2).Trim()) + "' ";

                printStr = "至" + o_str.RCDateConvert(TextBox_DATE_E.Text, "{0}年{1}月", true);
            }

            string OGBY = @"group by AKK.MZ_KCHI,CDH.MZ_EXUNIT
                            order by CDH.MZ_EXUNIT ";

            DataTable tempDT = o_DBFactory.ABC_toTest.Create_Table(SQL + WHERE + OGBY, "GET");

            if (tempDT.Rows.Count > 0)
            {
                //計算總計資料
                string sumSQL = string.Format(@"SELECT '總計' MZ_KCHI, SUM(t.TOTLETIME) TOTLETIME, SUM(t.REALTIME) REALTIME, SUM(t.BALANCETIME) BALANCETIME
                                , SUM(t.TOTLEPAY) TOTLEPAY FROM ({0}) t", SQL + WHERE + OGBY);
                DataTable sumTempDT = o_DBFactory.ABC_toTest.Create_Table(sumSQL, "GET");

                tempDT.ImportRow(sumTempDT.Rows[0]);

                tempDT.Columns[0].ColumnName = "單位";
                tempDT.Columns[1].ColumnName = "實際超勤時數";
                tempDT.Columns[2].ColumnName = "實際支領時數";
                tempDT.Columns[3].ColumnName = "結餘時數" + Environment.NewLine + "(不含補休時數)";
                tempDT.Columns[4].ColumnName = "支領金額";

                //建立表格表頭
                List<OfficeHelpers.ExcelHelpers.FormatColumn> exceFormat = new List<OfficeHelpers.ExcelHelpers.FormatColumn>();
                var t1 = new OfficeHelpers.ExcelHelpers.FormatColumn(0, 0, 0, 4, string.Format("{0} 超勤加班費時數及金額統計表", DropDownList_MZ_AD.SelectedItem.Text))
                {
                    Option = new OfficeHelpers.ExcelHelpers.CellStyle() { CenterText = true }
                };
                exceFormat.Add(t1);
                var t2 = new OfficeHelpers.ExcelHelpers.FormatColumn(1, 0, 1, 4, string.Format("期間：{0}", printStr))
                {
                    Option = new OfficeHelpers.ExcelHelpers.CellStyle() { CenterText = true }
                };
                exceFormat.Add(t2);

                new OfficeHelpers.ExcelHelpers().DtToExcelForXLS(tempDT, "超勤加班報表", true, exceFormat.ToArray());
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "alert('無此日期之相關資料');", true);
            }
        }

        /// <summary>
        /// 取消按鈕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btCancel_Click(object sender, EventArgs e)
        {
            TextBox_DATE_S.Text = string.Empty;
            TextBox_DATE_E.Text = string.Empty;
        }
    }
}