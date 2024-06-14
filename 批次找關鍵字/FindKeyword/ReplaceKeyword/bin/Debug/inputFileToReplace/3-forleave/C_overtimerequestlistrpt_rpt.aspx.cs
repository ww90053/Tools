using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using TPPDDB.Model.Const;

namespace TPPDDB._3_forleave
{
    public partial class C_overtimerequestlistrpt_rpt : System.Web.UI.Page
    {
        int TPM_FION = 0;
        bool IsNewZhonghe = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                C.check_power();

                ViewState["C_strGID"] = o_a_Function.strGID(Session["ADPMZ_ID"].ToString());

                //預填今天月份
                TextBox_DATE.Text = (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0') + DateTime.Now.Month.ToString().PadLeft(2, '0');
                chk_TPMGroup();
            }
        }

        private void doSearch(int Mode)
        {
            DataTable dt;
            String SQL = "";
            String SQL_IN = "";

            //AB權限會帶入1
            if (Mode == 1)
                dt = o_DBFactory.ABC_toTest.Create_Table("  (SELECT  MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE = '04' AND MZ_KCODE LIKE '38213%') ", "GET");
            else
            {
                //帶入三個單位資料 編制機關 現服機關 發薪機關 
                List<String> Depts = new List<String>();
                if (Session[ConstSession.AD] != null)
                    Depts.Add(Session[ConstSession.AD].ToString());
                if (Session[ConstSession.EXAD] != null)
                    Depts.Add(Session[ConstSession.EXAD].ToString());
                if (Session[ConstSession.ADPPAY_AD] != null)
                    Depts.Add(Session[ConstSession.ADPPAY_AD].ToString());

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
        /// 群組權限
        /// </summary>
        protected void chk_TPMGroup()
        {
            String premission = ViewState["C_strGID"].ToString();
            switch (premission)
            {
                case "A":
                case "B":
                    doSearch(1);
                    break;
                //Joy修改權限D 要能查詢其他單位
                // 20200623 人事室要求 將 D、E 權限 鎖下拉單位不得看其他
                case "D":
                    doSearch(2);
                    DropDownList_MZ_AD.Enabled = false;
                    DropDownList_MZ_UNIT.Enabled = false;
                    break;
                case "C":
                    doSearch(2);

                    break;
                //case "D":
                case "E":
                    doSearch(2);
                    DropDownList_MZ_UNIT.Enabled = false;
                    if (!IsNewZhonghe)
                    {
                        DropDownList_MZ_AD.Enabled = false;
                    }
                    DropDownList_MZ_AD.Enabled = false;
                    //DropDownList_MZ_UNIT.Enabled = false; //20150730 因小隊長反應跨單位支援情形相當頻繁，故針對E權限先不上鎖單位選項
                    break;
                default:
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click",
                        "alert('此人員查無超勤之權限資料，無法使用此功能'); window.location.href='/10-knowledge/J_Login_after.aspx?TPM_FION=-1';", true);
                    return;
                    break;
            }
            //DropDownList_MZ_AD.SelectedValue = Session["ADPMZ_AD"].ToString();

            try
            {

                ///20150629 此部分原先是抓現服機關單位為預設值
                ///但因機關改為抓取發薪機關後，若仍採用現服，會導致現服單位找不到
                ///(發薪機關與編制機關其所屬單位必定有；但發薪機關與現服單位不一定對得起來)
                ///故預設值均採用發薪機關及編制單位，避免程式出錯
                String AD = Session[ConstSession.ADPPAY_AD].ToString(); //20150629 改為發薪機關優先
                if (AD == "")
                {
                    AD = Session[ConstSession.AD].ToString(); //發薪機關若無值，則帶入編制機關
                }
                String Unit = Session[ConstSession.UNIT].ToString(); //20150629 改為編制單位優先
                DropDownList_MZ_AD.SelectedValue = AD;
                DropDownList_MZ_UNIT.SelectedValue = Unit;
            }
            catch
            {
                DropDownList_MZ_AD.SelectedValue = "";
                DropDownList_MZ_UNIT.SelectedValue = "";
            }

            C.fill_unit(DropDownList_MZ_UNIT, DropDownList_MZ_AD.SelectedValue);

        }
        protected void btPrint_Click(object sender, EventArgs e)
        {
            //2012/10/26 irk 小隊長 要求員編改成身份證

            //SQL第一段 基本欄位
            string SelectSQL = @"SELECT '' AS PAGE,
                                        (SELECT MZ_ID FROM A_DLBASE WHERE MZ_ID=C_DUTYMONTHOVERTIME_HOUR.MZ_ID) AS MZ_POLNO,
                                        (SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='26' AND MZ_KCODE=(SELECT MZ_OCCC FROM A_DLBASE WHERE MZ_ID=C_DUTYMONTHOVERTIME_HOUR.MZ_ID)) AS MZ_OCCC,
                                        (SELECT MZ_NAME FROM A_DLBASE WHERE MZ_ID=C_DUTYMONTHOVERTIME_HOUR.MZ_ID) AS MZ_NAME, ";

            //SQL第二段 每天日期
            for (int j = 1; j <= 31; j++)
            {
                SelectSQL += "\"" + j.ToString() + "\",";
            }

            //SQL第三段 
            SelectSQL += " MZ_BUDGET_HOUR,MZ_REAL_HOUR,MZ_BALANCE_HOUR,PAY1,PROFESS,BOSS, MZ_HOUR_PAY,MZ_OVERTIME_PAY,MZ_ID FROM C_DUTYMONTHOVERTIME_HOUR WHERE MZ_YEAR='" + TextBox_DATE.Text.Trim().Substring(0, 3) +
                                                              "' AND MZ_MONTH='" + TextBox_DATE.Text.Trim().Substring(3, 2) +
                                                              "' AND MZ_AD ='" + DropDownList_MZ_AD.SelectedValue + "' ";

            //如果編制單位有選，則判斷是以編制單位還是現服單位去列印
            if (!string.IsNullOrEmpty(DropDownList_MZ_UNIT.SelectedValue))
            {
                //編制單位(原設定)
                if (RadioButtonList_UNIT.SelectedValue == "UNIT")
                {
                    SelectSQL += " AND MZ_UNIT='" + DropDownList_MZ_UNIT.SelectedValue + "'";
                }
                //現服單位
                else if (RadioButtonList_UNIT.SelectedValue == "EXUNIT")
                {
                    SelectSQL += " AND MZ_EXUNIT='" + DropDownList_MZ_UNIT.SelectedValue + "'";
                }
            }

            /// Joy 加入身分證判斷
            if (!string.IsNullOrEmpty(TextBox_ID.Text))
            {
                SelectSQL += " AND MZ_ID='" + TextBox_ID.Text + "'";
            }

            //SQL第四段 排序
            //新增主管級別(MZ_PCHIEF)排序 20180410 by sky
            // sam.hsu wellsince 20201109 主任祕書室建議 相同職序下 依 任職年資高者排到低 
            bool is主任秘書室
                = (DropDownList_MZ_AD.SelectedValue == "382130000C" && DropDownList_MZ_UNIT.SelectedValue == "0002");
            if (is主任秘書室)
            {   //針對主任秘書室需要設計特殊的排序邏輯
                SelectSQL += @" 
               --排序條件
                ORDER BY 
                --特殊:主任秘書1030→警政監1492,其他往後排,這個條件僅針對主任秘書室(介面上的列印條件為382130000C’and ‘0002’進行調整，其餘單位不動
                (SELECT case MZ_OCCC when  '1030' then 2 when '1492' then 1 else 0 end FROM A_DLBASE WHERE MZ_ID = C_DUTYMONTHOVERTIME_HOUR.MZ_ID) DESC,
                --年資排序
                (SELECT REPLACE(MZ_TBDV,'Z99','999')FROM A_DLBASE WHERE MZ_ID = C_DUTYMONTHOVERTIME_HOUR.MZ_ID),
                --主管級別
                (SELECT MZ_PCHIEF FROM A_DLBASE WHERE MZ_ID = C_DUTYMONTHOVERTIME_HOUR.MZ_ID),
                --職稱
                (SELECT MZ_OCCC FROM A_DLBASE WHERE MZ_ID = C_DUTYMONTHOVERTIME_HOUR.MZ_ID),
                --公職年資
                (SELECT MZ_OFFYY FROM   A_DLBASE WHERE  MZ_ID = C_DUTYMONTHOVERTIME_HOUR.MZ_ID) DESC,
                --最後再依據身分證號排序
                MZ_ID
            ";
            }
            else
            {   //一般的排序條件
                SelectSQL += @" 
               --排序條件
                ORDER BY 
                --年資排序
                (SELECT REPLACE(MZ_TBDV,'Z99','999')FROM A_DLBASE WHERE MZ_ID = C_DUTYMONTHOVERTIME_HOUR.MZ_ID),
                --主管級別
                (SELECT MZ_PCHIEF FROM A_DLBASE WHERE MZ_ID = C_DUTYMONTHOVERTIME_HOUR.MZ_ID),
                --職稱
                (SELECT MZ_OCCC FROM A_DLBASE WHERE MZ_ID = C_DUTYMONTHOVERTIME_HOUR.MZ_ID),
                --公職年資
                (SELECT MZ_OFFYY FROM   A_DLBASE WHERE  MZ_ID = C_DUTYMONTHOVERTIME_HOUR.MZ_ID) DESC,
                --最後再依據身分證號排序
                MZ_ID
            ";

            }



            // sam wellsince 20201006 主任祕書 的 職序 是有個Range    
            //經查後 直轄市的主任祕書 職序應為第三序列 已請人事室進行修改資料
            //if (DropDownList_MZ_AD.SelectedValue == "382130000C" && DropDownList_MZ_UNIT.SelectedValue == "0002")
            //{                                                                                                
            //    SelectSQL += @" ORDER BY 
            //                             (SELECT MZ_OCCC 
            //                              FROM A_DLBASE WHERE MZ_ID = C_DUTYMONTHOVERTIME_HOUR.MZ_ID),MZ_ID";
            //}
            //else
            //{
            //    SelectSQL += @" ORDER BY (SELECT REPLACE(MZ_TBDV,'Z99','999') 
            //                              FROM A_DLBASE WHERE MZ_ID = C_DUTYMONTHOVERTIME_HOUR.MZ_ID),
            //                             (SELECT MZ_PCHIEF FROM A_DLBASE WHERE MZ_ID = C_DUTYMONTHOVERTIME_HOUR.MZ_ID),
            //                             (SELECT MZ_OCCC 
            //                              FROM A_DLBASE WHERE MZ_ID = C_DUTYMONTHOVERTIME_HOUR.MZ_ID),MZ_ID";
            //}

            DataTable dt = o_DBFactory.ABC_toTest.Create_Table(SelectSQL, "GET");

            Session["RPT_SQL_C"] = SelectSQL;

            if (dt.Rows.Count > 0)
            {
                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    dt.Rows[i]["PAGE"] = i / 15;
                //}

                //Session["rpt_dt"] = dt;

                Session["TITLE"] = string.Format("{0}{1}{2}年{3}月超勤加班費印領清冊", DropDownList_MZ_AD.SelectedItem.Text, DropDownList_MZ_UNIT.SelectedItem.Text, TextBox_DATE.Text.Substring(0, 3), TextBox_DATE.Text.Substring(3, 2));

                string tmp_url = "C_rpt.aspx?fn=overtimerequestlistrpt&TPM_FION=" + TPM_FION;

                ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "alert('無此日期之相關資料');", true);

            }
        }
        protected void btPrintExcel_Click(object sender, EventArgs e)
        {
            //2012/10/26 irk 小隊長 要求員編改成身份證

            //SQL第一段 基本欄位
            //            string SelectSQL = @"SELECT 
            //                                        (SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=C_DUTYMONTHOVERTIME_HOUR.MZ_UNIT) AS MZ_UNIT,
            //                                        (SELECT MZ_ID FROM A_DLBASE WHERE MZ_ID=C_DUTYMONTHOVERTIME_HOUR.MZ_ID) AS MZ_POLNO,
            //                                        (SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='26' AND MZ_KCODE=(SELECT MZ_OCCC FROM A_DLBASE WHERE MZ_ID=C_DUTYMONTHOVERTIME_HOUR.MZ_ID)) AS MZ_OCCC,
            //                                        (SELECT MZ_NAME FROM A_DLBASE WHERE MZ_ID=C_DUTYMONTHOVERTIME_HOUR.MZ_ID) AS MZ_NAME, ";

            //            //SQL第三段 
            //            SelectSQL += " MZ_BUDGET_HOUR,MZ_REAL_HOUR,MZ_BALANCE_HOUR,PAY1,PROFESS,BOSS, MZ_HOUR_PAY,MZ_OVERTIME_PAY, MZ_REMARK FROM C_DUTYMONTHOVERTIME_HOUR WHERE MZ_YEAR='" + TextBox_DATE.Text.Trim().Substring(0, 3) +
            //                                                              "' AND MZ_MONTH='" + TextBox_DATE.Text.Trim().Substring(3, 2) +
            //                                                              "' AND MZ_AD ='" + DropDownList_MZ_AD.SelectedValue + "' ";

            string SelectSQL = @"select ak1.MZ_KCHI MZ_UNIT,A_DLBASE.mz_id MZ_POLNO, A_DLBASE.MZ_NAME,ak2.MZ_KCHI MZ_OCCC
                                ,MZ_BUDGET_HOUR,MZ_REAL_HOUR,MZ_BALANCE_HOUR,PAY1,PROFESS,BOSS, MZ_HOUR_PAY,MZ_OVERTIME_PAY, MZ_REMARK
                                from C_DUTYMONTHOVERTIME_HOUR
                                left join A_KTYPE ak1 on ak1.MZ_KCODE = C_DUTYMONTHOVERTIME_HOUR.MZ_UNIT
                                left join A_DLBASE on A_DLBASE.MZ_ID = C_DUTYMONTHOVERTIME_HOUR.MZ_ID
                                left join A_KTYPE ak2 on ak2.MZ_KTYPE = '26' and ak2.MZ_KCODE = A_DLBASE.MZ_OCCC 
                                WHERE ak1.MZ_KTYPE = '25' and MZ_YEAR='" + TextBox_DATE.Text.Trim().Substring(0, 3) +
                                                              "' AND MZ_MONTH='" + TextBox_DATE.Text.Trim().Substring(3, 2) +
                                                             "' AND C_DUTYMONTHOVERTIME_HOUR.MZ_AD ='" + DropDownList_MZ_AD.SelectedValue + "' ";


            //如果編制單位有選，則判斷是以編制單位還是現服單位去列印
            if (!string.IsNullOrEmpty(DropDownList_MZ_UNIT.SelectedValue))
            {
                //20150104
                //Jack修改
                //編制單位(原設定)
                if (RadioButtonList_UNIT.SelectedValue == "UNIT")
                {
                    SelectSQL += " AND C_DUTYMONTHOVERTIME_HOUR.MZ_UNIT='" + DropDownList_MZ_UNIT.SelectedValue + "'";
                }
                //現服單位
                else if (RadioButtonList_UNIT.SelectedValue == "EXUNIT")
                {
                    SelectSQL += " AND C_DUTYMONTHOVERTIME_HOUR.MZ_EXUNIT='" + DropDownList_MZ_UNIT.SelectedValue + "'";
                }
            }

            /// Joy 加入身分證判斷
            if (!string.IsNullOrEmpty(TextBox_ID.Text))
            {
                SelectSQL += " AND MZ_ID='" + TextBox_ID.Text + "'";
            }

            //SQL第四段 排序
            //SelectSQL += @" ORDER BY MZ_UNIT,(SELECT REPLACE(MZ_TBDV,'Z99','999') 
            //                          FROM A_DLBASE WHERE MZ_ID = C_DUTYMONTHOVERTIME_HOUR.MZ_ID),
            //                         (SELECT MZ_OCCC 
            //                          FROM A_DLBASE WHERE MZ_ID = C_DUTYMONTHOVERTIME_HOUR.MZ_ID),C_DUTYMONTHOVERTIME_HOUR.MZ_ID";

            //20210225 - 排序跟 人事管理-6.超勤管理作業-6.4 超勤時數審核：清單排序需要調整，順序要跟 6.3.2.超勤印領清冊 列印 一致
            SelectSQL += @" ORDER BY (SELECT REPLACE(MZ_TBDV,'Z99','999') FROM A_DLBASE WHERE MZ_ID = C_DUTYMONTHOVERTIME_HOUR.MZ_ID), (SELECT MZ_PCHIEF FROM A_DLBASE WHERE MZ_ID = C_DUTYMONTHOVERTIME_HOUR.MZ_ID), (SELECT MZ_OCCC FROM A_DLBASE WHERE MZ_ID = C_DUTYMONTHOVERTIME_HOUR.MZ_ID), (SELECT MZ_OFFYY FROM   A_DLBASE WHERE  MZ_ID = C_DUTYMONTHOVERTIME_HOUR.MZ_ID) DESC, C_DUTYMONTHOVERTIME_HOUR.MZ_ID";


            DataTable dt = o_DBFactory.ABC_toTest.Create_Table(SelectSQL, "GET");
            if (dt.Rows.Count > 0)
            {
                dt.Columns[0].ColumnName = "單位";
                dt.Columns[1].ColumnName = "身分證號";
                dt.Columns[2].ColumnName = "職稱";
                dt.Columns[3].ColumnName = "姓名";
                dt.Columns[4].ColumnName = "超勤時數";
                dt.Columns[5].ColumnName = "實際支領時數";
                dt.Columns[6].ColumnName = "結餘時數";
                dt.Columns[7].ColumnName = "俸給月支數額";
                dt.Columns[8].ColumnName = "專業加給";
                dt.Columns[9].ColumnName = "主管加給";
                dt.Columns[10].ColumnName = "每小時支領時數";
                dt.Columns[11].ColumnName = "超勤金額";
                dt.Columns[12].ColumnName = "備註說明";
                App_Code.ToExcel.Dt2Excel(dt, DropDownList_MZ_AD.SelectedItem.ToString() + DropDownList_MZ_UNIT.SelectedItem.ToString() + "報表");
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "alert('無此日期之相關資料');", true);
            }

        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            TextBox_DATE.Text = string.Empty;
        }

        protected void DropDownList_MZ_UNIT_DataBound(object sender, EventArgs e)
        {
            ListItem li = new ListItem(" ", "");
            DropDownList_MZ_UNIT.Items.Insert(0, li);
        }

        protected void DropDownList_MZ_AD_SelectedIndexChanged(object sender, EventArgs e)
        {
            C.fill_unit(DropDownList_MZ_UNIT, DropDownList_MZ_AD.SelectedValue);

            String ID = Session["ADPMZ_ID"].ToString();
            String AD = Session["ADPMZ_AD"].ToString();
            String EXAD = Session["ADPMZ_EXAD"].ToString();
            String UNIT = Session["ADPMZ_UNIT"].ToString();
            String MZPOWER = o_a_Function.strGID(ID);

            if (DropDownList_MZ_AD.SelectedValue == AD)
            {
                DropDownList_MZ_UNIT.SelectedValue = UNIT;

                if (MZPOWER != "A" && MZPOWER != "B" && MZPOWER != "D")
                {
                    if (AD != EXAD)
                        DropDownList_MZ_UNIT.Enabled = false;
                    else
                        DropDownList_MZ_UNIT.Enabled = true;
                }

            }

        }


    }
}
