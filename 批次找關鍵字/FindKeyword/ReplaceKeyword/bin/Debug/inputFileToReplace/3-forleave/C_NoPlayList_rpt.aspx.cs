using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Data;
using AjaxControlToolkit;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;


namespace TPPDDB._3_forleave
{
    public partial class C_NoPlayList_rpt : System.Web.UI.Page
    {
        int TPM_FION = 0;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                C.check_power();
                //matthew 為了中和分局判斷功能權限用
                ViewState["C_strGID"] = o_a_Function.strGID(Session["ADPMZ_ID"].ToString());
                //MQ-----------------------20100331
                C.set_Panel_EnterToTAB(ref this.Panel1);

                //C.fill_AD_POST(DropDownList_EXAD);
                //如果是中和分局進來改成只抓中和一&中和二&中和 matthew
                if (Session["ADPMZ_EXAD"].ToString() == "382133600C")
                {
                    C.fill_DLL_ONE_TWO(DropDownList_EXAD);
                }
                else
                {
                    //把所有機關撈出來包含台北縣
                    C.fill_AD_POST(DropDownList_EXAD);
                }
                DropDownList_EXAD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                C.fill_unit(DropDownList_EXUNIT, DropDownList_EXAD.SelectedValue);
                chk_TPMGroup();
                TextBox_YEAR.Text = (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0');

            }
        }


        /// <summary>
        /// 群組權限
        /// </summary>
        protected void chk_TPMGroup()
        {
            switch (o_a_Function.strGID(Session["ADPMZ_ID"].ToString()))
            {
                case "A":
                case "B":

                    break;
                case "C":
                    //DropDownList_EXAD.Enabled = false;
                    //如果是中和分局進來改成只抓中和一&中和二&中和 matthew
                    if (Session["ADPMZ_EXAD"].ToString() != "382133600C")
                    {
                        DropDownList_EXAD.Enabled = false;
                    }
                    break;
                case "D":
                    //無權限
                    TPMPermissions._boolErrorData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "ADUXXX0001");
                    Response.Redirect("~/ErrorPage.aspx?erid=ADUXXX0001");
                    break;
                case "E":
                    //DropDownList_EXAD.Enabled = false;
                    //如果是中和分局進來改成只抓中和一&中和二&中和 matthew
                    if (Session["ADPMZ_EXAD"].ToString() != "382133600C")
                    {
                        DropDownList_EXAD.Enabled = false;
                    }
                    DropDownList_EXUNIT.SelectedValue = Session["ADPMZ_EXUNIT"].ToString();
                    DropDownList_EXUNIT.Enabled = false;
                    break;
            }
        }

        protected void btPrint_Click(object sender, EventArgs e)
        {
            string PAY_AD = DropDownList_EXAD.SelectedValue;

            DataTable NOPLAYLIST = new DataTable();

            NOPLAYLIST.Columns.Clear();

            NOPLAYLIST.Columns.Add("OCCC", typeof(string));
            NOPLAYLIST.Columns.Add("NAME", typeof(string));
            NOPLAYLIST.Columns.Add("FDATE", typeof(string));
            NOPLAYLIST.Columns.Add("HDAY", typeof(string));
            NOPLAYLIST.Columns.Add("ODAY", typeof(string));
            NOPLAYLIST.Columns.Add("ODAYPAY", typeof(int));
            NOPLAYLIST.Columns.Add("ODAYPAY_TOTAL", typeof(int));
            NOPLAYLIST.Columns.Add("NOPLAYDAY", typeof(string));
            NOPLAYLIST.Columns.Add("SDAY3", typeof(string));
            NOPLAYLIST.Columns.Add("PAYDAY", typeof(string));
            NOPLAYLIST.Columns.Add("DAYPAY", typeof(int));
            NOPLAYLIST.Columns.Add("NOPLAYDAY_TOTAL", typeof(int));
            NOPLAYLIST.Columns.Add("TOTAL_PAY", typeof(int));
            NOPLAYLIST.Columns.Add("POLNO", typeof(string));
            NOPLAYLIST.Columns.Add("PAY1", typeof(int));
            NOPLAYLIST.Columns.Add("PROFESS", typeof(int));
            NOPLAYLIST.Columns.Add("BOSS", typeof(int));
            NOPLAYLIST.Columns.Add("MOMTHPAY", typeof(int));
            NOPLAYLIST.Columns.Add("MEMO", typeof(string));

            string strSQLpart = "";
            string strSQLpart1 = "";

            if (RadioButtonList2.SelectedValue == "0")
            {
                

                if (RadioButtonList1.SelectedValue == "0")
                {
                    strSQLpart += " AND MZ_OCCC!='Z014' AND MZ_OCCC!='Z015' AND MZ_OCCC!='Z016'";
                    strSQLpart1 += " AND MZ_OCCC!='Z014' AND MZ_OCCC!='Z015' AND MZ_OCCC!='Z016'";
                }
                else if (RadioButtonList1.SelectedValue == "1")
                {
                    strSQLpart += " AND MZ_OCCC IN ('Z014','Z015','Z016')";
                    strSQLpart1 += " AND MZ_OCCC IN ('Z014','Z015','Z016')";
                }
                else if (RadioButtonList1.SelectedValue == "3")
                {
                    strSQLpart += " AND MZ_OCCC IN ('1179') ";
                    strSQLpart1 += " AND MZ_OCCC IN ('1179') ";
                }
                else
                {

                }

                if (!string.IsNullOrEmpty(DropDownList_EXUNIT.SelectedValue))
                {

                    if (RadioButtonList1.SelectedValue == "2")
                    {
                        strSQLpart += " AND MZ_ID IN (SELECT MZ_ID FROM A_DLBASE WHERE MZ_AD='" + PAY_AD + "' AND MZ_UNIT='" + DropDownList_EXUNIT.SelectedValue.Trim() + "')";
                        strSQLpart1 += " AND MZ_UNIT='" + DropDownList_EXUNIT.SelectedValue + "'";
                    }
                    else
                    {
                        strSQLpart += " AND MZ_ID IN (SELECT MZ_ID FROM A_DLBASE WHERE PAY_AD='" + PAY_AD + "' AND MZ_UNIT='" + DropDownList_EXUNIT.SelectedValue.Trim() + "')";
                        strSQLpart1 += " AND MZ_UNIT='" + DropDownList_EXUNIT.SelectedValue + "'";
                    }
                }
                string strSQL = string.Empty;
                if (RadioButtonList1.SelectedValue == "2")
                {
                    //增加主管級別(MZ_PCHIEF)排序 20190103 by sky
                    strSQL = string.Format(@"SELECT C_DLTBB.MZ_ID, MZ_OCCC AS OCCC, dbo.to_number(CASE WHEN MZ_TBDV IS NULL THEN '999' WHEN MZ_TBDV='Z99' THEN '999' ELSE MZ_TBDV END) AS TBDV, 
                                                    MZ_AD, MZ_EXAD, MZ_EXUNIT, MZ_UNIT, PAY_AD 
                                             FROM A_DLBASE 
                                             left join C_DLTBB on C_DLTBB.MZ_ID= A_DLBASE.mz_id  
                                             WHERE MZ_STATUS2='N' AND MZ_OCCC!='Z011' AND MZ_OCCC!='1179' And mz_exunit='9999' And mz_year='{0}' AND MZ_AD='{1}' {2} 
                                             ORDER BY TBDV,MZ_PCHIEF,OCCC,MZ_EXUNIT "
                                             , o_str.tosql(TextBox_YEAR.Text.Trim().PadLeft(3, '0'))
                                             , PAY_AD
                                             , strSQLpart1);
                }
                else if (RadioButtonList1.SelectedValue == "3")
                {
                    //增加約僱人員 20200306 by sky
                    strSQL = string.Format(@"SELECT MZ_ID, MZ_OCCC AS OCCC, 
                                                    dbo.to_number(CASE WHEN MZ_TBDV IS NULL THEN '999' WHEN MZ_TBDV='Z99' THEN '999' ELSE MZ_TBDV END) AS TBDV,
                                                    MZ_AD, MZ_EXAD, MZ_EXUNIT, MZ_UNIT, PAY_AD 
                                             FROM A_DLBASE 
                                             WHERE MZ_STATUS2='Y' AND PAY_AD='{0}' {1} 
                                             ORDER BY TBDV,MZ_PCHIEF,OCCC,MZ_EXUNIT "
                                             , PAY_AD
                                             , strSQLpart1);
                }
                else
                {
                    //增加主管級別(MZ_PCHIEF)排序 20190103 by sky
                    strSQL = string.Format(@"SELECT MZ_ID, MZ_OCCC AS OCCC, 
                                                    dbo.to_number(CASE WHEN MZ_TBDV IS NULL THEN '999' WHEN MZ_TBDV='Z99' THEN '999' ELSE MZ_TBDV END) AS TBDV,
                                                    MZ_AD, MZ_EXAD, MZ_EXUNIT, MZ_UNIT, PAY_AD 
                                             FROM A_DLBASE 
                                             WHERE MZ_STATUS2='Y' AND MZ_OCCC!='Z011' AND MZ_OCCC!='1179' AND PAY_AD='{0}' {1} 
                                             ORDER BY TBDV,MZ_PCHIEF,OCCC,MZ_EXUNIT "
                                             , PAY_AD
                                             , strSQLpart1);
                }
                DataTable temp = new DataTable();

                temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "NoPlay1");

                //如遇 預退休人員 人事的PAY_AD會改空值.所以報表會列不出來.因為久久弄一次會遺忘.所以再提就說是如此
                //20151218 Neil : 目前連留職停薪亦會將 PAD_AD 改為空值


                //計算資料,將計算好的結果納入 DataTable NOPLAYLIST
                bool result = _CACULATE(NOPLAYLIST, temp);
                if (result == false)
                {   //計算失敗,中斷執行
                    return;
                }


                //  string selectString = "SELECT (SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='26' AND MZ_KCODE=(SELECT MZ_OCCC FROM A_DLBASE WHERE MZ_ID=C_NOPLAYPAY.MZ_ID)),(SELECT MZ_NAME FROM A_DLBASE WHERE MZ_ID=C_NOPLAYPAY.MZ_ID),C_NOPLAYPAY.* FROM C_NOPLAYPAY WHERE MZ_ID IN (SELECT MZ_ID FROM A_DLBASE WHERE MZ_EXAD='" + o_str.tosql(DropDownList_EXAD.Text.Trim()) + "' AND MZ_EXUNIT='" + o_str.tosql(DropDownList_EXUNIT.SelectedValue.Trim()) + "')";

                Session["rpt_dt"] = NOPLAYLIST;

                if (RadioButtonList1.SelectedValue == "0")
                {
                    Session["TITLE"] = string.Format("{0}{1}{2}年員警因公未休假改發加班費印領清冊", DropDownList_EXAD.SelectedItem.Text, DropDownList_EXUNIT.SelectedItem.Text, (int.Parse(TextBox_YEAR.Text.Trim())).ToString());
                    Session["TITLE1"] = "人事單位";
                }
                else if (RadioButtonList1.SelectedValue == "1")
                {
                    Session["TITLE"] = string.Format("{0}{1}{2}年駕駛工友因公未休假改發加班費印領清冊", DropDownList_EXAD.SelectedItem.Text, DropDownList_EXUNIT.SelectedItem.Text, (int.Parse(TextBox_YEAR.Text.Trim())).ToString());
                    Session["TITLE1"] = "業管單位";
                }
                else if (RadioButtonList1.SelectedValue == "3")
                {
                    Session["TITLE"] = string.Format("{0}{1}{2}年約僱人員因公未休假改發加班費印領清冊", DropDownList_EXAD.SelectedItem.Text, DropDownList_EXUNIT.SelectedItem.Text, (int.Parse(TextBox_YEAR.Text.Trim())).ToString());
                    Session["TITLE1"] = "人事單位";
                }
                else
                {
                    Session["TITLE"] = string.Format("{0}{1}{2}年離退職人員因公未休假改發加班費印領清冊", DropDownList_EXAD.SelectedItem.Text, DropDownList_EXUNIT.SelectedItem.Text, (int.Parse(TextBox_YEAR.Text.Trim())).ToString());
                    Session["TITLE1"] = "人事單位";
                }
                string tmp_url = "C_rpt.aspx?fn=noplaylist&TPM_FION=" + TPM_FION;

                ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);

            }
            else
            {                

                if (RadioButtonList3.SelectedValue == "0")
                {
                    if (string.IsNullOrEmpty(DropDownList_EXUNIT.SelectedValue))
                        strSQLpart += " AND MZ_OCCC!='Z014' AND MZ_OCCC!='Z015' AND MZ_OCCC!='Z016' AND MZ_ID IN (SELECT MZ_ID FROM A_DLBASE WHERE PAY_AD=MZ_EXAD AND PAY_AD='" + PAY_AD + "')";
                    else
                        strSQLpart += " AND MZ_OCCC!='Z014' AND MZ_OCCC!='Z015' AND MZ_OCCC!='Z016' AND MZ_ID IN (SELECT MZ_ID FROM A_DLBASE WHERE PAY_AD=MZ_EXAD AND PAY_AD='" + PAY_AD + "' AND MZ_EXUNIT='" + DropDownList_EXUNIT.SelectedValue + "' )";

                    strSQLpart1 += " AND MZ_OCCC!='Z014' AND MZ_OCCC!='Z015' AND MZ_OCCC!='Z016' AND PAY_AD=MZ_EXAD";
                }
                else if (RadioButtonList3.SelectedValue == "1")
                {
                    strSQLpart += " AND MZ_OCCC IN ('Z014','Z015','Z016')";
                    strSQLpart1 += " AND MZ_OCCC IN ('Z014','Z015','Z016')";
                }
                else if (RadioButtonList3.SelectedValue == "3")
                {
                    strSQLpart += " AND MZ_OCCC IN ('1179') ";
                    strSQLpart1 += " AND MZ_OCCC IN ('1179') ";
                }
                else
                {
                    if (string.IsNullOrEmpty(DropDownList_EXUNIT.SelectedValue))
                        strSQLpart += " AND MZ_OCCC!='Z014' AND MZ_OCCC!='Z015' AND MZ_OCCC!='Z016' AND MZ_ID IN (SELECT MZ_ID FROM A_DLBASE WHERE PAY_AD!=MZ_EXAD AND PAY_AD='" + PAY_AD + "')";
                    else
                        strSQLpart += " AND MZ_OCCC!='Z014' AND MZ_OCCC!='Z015' AND MZ_OCCC!='Z016' AND MZ_ID IN (SELECT MZ_ID FROM A_DLBASE WHERE PAY_AD!=MZ_EXAD AND PAY_AD='" + PAY_AD + "' AND MZ_EXUNIT='" + DropDownList_EXUNIT.SelectedValue + "')";

                    strSQLpart1 += " AND MZ_OCCC!='Z014' AND MZ_OCCC!='Z015' AND MZ_OCCC!='Z016' AND PAY_AD!=MZ_EXAD";
                }

                if (!string.IsNullOrEmpty(DropDownList_EXUNIT.SelectedValue))
                {
                    strSQLpart1 += " AND MZ_EXUNIT='" + DropDownList_EXUNIT.SelectedValue + "'";
                }

                string deleteString = "DELETE FROM C_NOPLAYPAY WHERE PAY_AD='" + PAY_AD + "' " + strSQLpart + " ";

                o_DBFactory.ABC_toTest.Edit_Data(deleteString);

                string strSQL = string.Empty;
                //增加主管級別(MZ_PCHIEF)排序 20190103 by sky
                if (RadioButtonList3.SelectedValue == "3")
                {
                    strSQL = string.Format(@"SELECT MZ_ID, MZ_OCCC AS OCCC, 
                                                    dbo.to_number(CASE WHEN MZ_TBDV IS NULL THEN '999' WHEN MZ_TBDV='Z99' THEN '999' ELSE MZ_TBDV END) AS TBDV,
                                                    MZ_AD, MZ_EXAD, MZ_EXUNIT, MZ_UNIT, PAY_AD 
                                             FROM A_DLBASE 
                                             WHERE MZ_STATUS2='Y' AND MZ_OCCC!='Z011' AND PAY_AD='{0}' {1} 
                                             ORDER BY TBDV,MZ_PCHIEF,OCCC,MZ_EXUNIT "
                                             , PAY_AD
                                             , strSQLpart1);
                }
                else
                {
                    strSQL = string.Format(@"SELECT MZ_ID, MZ_OCCC AS OCCC, 
                                                    dbo.to_number(CASE WHEN MZ_TBDV IS NULL THEN '999' WHEN MZ_TBDV='Z99' THEN '999' ELSE MZ_TBDV END) AS TBDV,
                                                    MZ_AD, MZ_EXAD, MZ_EXUNIT, MZ_UNIT, PAY_AD 
                                             FROM A_DLBASE 
                                             WHERE MZ_STATUS2='Y' AND MZ_OCCC!='Z011' AND MZ_OCCC!='1179' AND PAY_AD='{0}' {1} 
                                             ORDER BY TBDV,MZ_PCHIEF,OCCC,MZ_EXUNIT "
                                             , PAY_AD
                                             , strSQLpart1);
                }

                DataTable temp = new DataTable();

                temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "NoPlay1");

                //計算資料,將計算好的結果納入 DataTable NOPLAYLIST
                bool result = _CACULATE(NOPLAYLIST, temp);
                if (result == false)
                {   //計算失敗,中斷執行
                    return;
                }

                //  string selectString = "SELECT (SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='26' AND MZ_KCODE=(SELECT MZ_OCCC FROM A_DLBASE WHERE MZ_ID=C_NOPLAYPAY.MZ_ID)),(SELECT MZ_NAME FROM A_DLBASE WHERE MZ_ID=C_NOPLAYPAY.MZ_ID),C_NOPLAYPAY.* FROM C_NOPLAYPAY WHERE MZ_ID IN (SELECT MZ_ID FROM A_DLBASE WHERE MZ_EXAD='" + o_str.tosql(DropDownList_EXAD.Text.Trim()) + "' AND MZ_EXUNIT='" + o_str.tosql(DropDownList_EXUNIT.SelectedValue.Trim()) + "')";

                Session["rpt_dt"] = NOPLAYLIST;

                if (RadioButtonList3.SelectedValue == "0")
                {
                    Session["TITLE"] = string.Format("{0}{1}{2}年員警因公未休假改發加班費印領清冊", DropDownList_EXAD.SelectedItem.Text, DropDownList_EXUNIT.SelectedItem.Text, (int.Parse(TextBox_YEAR.Text.Trim())).ToString());
                    Session["TITLE1"] = "人事單位";
                }
                else if (RadioButtonList3.SelectedValue == "1")
                {
                    Session["TITLE"] = string.Format("{0}{1}{2}年駕駛工友因公未休假改發加班費印領清冊", DropDownList_EXAD.SelectedItem.Text, DropDownList_EXUNIT.SelectedItem.Text, (int.Parse(TextBox_YEAR.Text.Trim())).ToString());
                    Session["TITLE1"] = "業管單位";

                }
                else if (RadioButtonList3.SelectedValue == "2")
                {
                    Session["TITLE"] = string.Format("{0}{1}{2}年支援他機關因公未休假改發加班費印領清冊", DropDownList_EXAD.SelectedItem.Text, DropDownList_EXUNIT.SelectedItem.Text, (int.Parse(TextBox_YEAR.Text.Trim())).ToString());
                    Session["TITLE1"] = "人事單位";
                }
                else if (RadioButtonList3.SelectedValue == "3")
                {
                    Session["TITLE"] = string.Format("{0}{1}{2}年約僱人員因公未休假改發加班費印領清冊", DropDownList_EXAD.SelectedItem.Text, DropDownList_EXUNIT.SelectedItem.Text, (int.Parse(TextBox_YEAR.Text.Trim())).ToString());
                    Session["TITLE1"] = "人事單位";
                }
                string tmp_url = "C_rpt.aspx?fn=noplaylist&TPM_FION=" + TPM_FION;

                ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);

            }
        }

        /// <summary>
        /// 計算資料,將計算好的結果納入 DataTable NOPLAYLIST
        /// 
        /// </summary>
        /// <param name="NOPLAYLIST">輸出的資料</param>
        /// <param name="temp">輸入的資料</param>
        /// <returns></returns>
        private bool _CACULATE(DataTable NOPLAYLIST, DataTable temp)
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {

                for (int i = 0; i < temp.Rows.Count; i++)
                {
                    //每次產生報表都會先刪掉舊資料再產生後寫入TABLE;然後產生DATATABLE給rpt;
                    //在 因公未休假改發加班費彙總表 時使用那張寫入的TABLE做統計
                    string deleteString = "DELETE FROM C_NOPLAYPAY WHERE MZ_ID='" + temp.Rows[i]["MZ_ID"].ToString() + "' AND MZ_YEAR='" + o_str.tosql(TextBox_YEAR.Text.Trim().PadLeft(3, '0')) + "'";

                    o_DBFactory.ABC_toTest.Edit_Data(deleteString);

                    // sam.hsu 20201217

                    ////個人薪俸
                    //string SRANK = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_SRANK FROM A_DLBASE WHERE MZ_ID='" + temp.Rows[i]["MZ_ID"].ToString() + "'");

                    //string SPT1 = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_SPT1 FROM A_DLBASE WHERE MZ_ID='" + temp.Rows[i]["MZ_ID"].ToString() + "'");

                    //string SPT = !string.IsNullOrEmpty(SPT1) ? SPT1.PadLeft(4, '0') : o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_SPT FROM A_DLBASE WHERE MZ_ID='" + temp.Rows[i]["MZ_ID"].ToString() + "'").PadLeft(4, '0');

                    //string pay1String = "0";

                    //if (!string.IsNullOrEmpty(SRANK))
                    //{
                    //    if (SRANK.Substring(0, 1) == "G")
                    //        pay1String = o_DBFactory.ABC_toTest.vExecSQL("SELECT PAY1 FROM B_SALARY WHERE ORIGIN2='" + SPT + "' AND NAME1='1'");
                    //    else if (SRANK.Substring(0, 1) == "P" || SRANK.Substring(0, 1) == "B")
                    //        pay1String = o_DBFactory.ABC_toTest.vExecSQL("SELECT PAY1 FROM B_SALARY WHERE ORIGIN1='" + SPT + "' AND NAME1='1'");
                    //    else
                    //        pay1String = o_DBFactory.ABC_toTest.vExecSQL("SELECT PAY1 FROM B_SALARY WHERE ORIGIN1='" + SPT + "' AND NAME1='2'");
                    //}

                    //int pay1 = int.Parse(string.IsNullOrEmpty(pay1String) ? "0" : pay1String);
                    ////個人專業加給
                    //string professString = o_DBFactory.ABC_toTest.vExecSQL("SELECT PAY FROM B_PROFESS WHERE \"ID\"=(SELECT MZ_SRANK+'01D' FROM A_DLBASE WHERE MZ_ID='" + temp.Rows[i]["MZ_ID"].ToString() + "')");

                    //int profess = int.Parse(string.IsNullOrEmpty(professString) ? "0" : professString);
                    //個人主管加給
                    string EXTPOS_RANK = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_EXTPOS_SRANK FROM A_DLBASE WHERE MZ_ID='" + temp.Rows[i]["MZ_ID"].ToString() + "'");

                    string bossString = "";

                    if (string.IsNullOrEmpty(EXTPOS_RANK))
                        bossString = o_DBFactory.ABC_toTest.vExecSQL("SELECT PAY FROM B_BOSS WHERE \"ID\"=(SELECT MZ_SRANK+'01D' FROM A_DLBASE WHERE MZ_ID='" + temp.Rows[i]["MZ_ID"].ToString() + "' AND MZ_PCHIEF IS NOT NULL)");
                    else
                        bossString = o_DBFactory.ABC_toTest.vExecSQL("SELECT NVL(PAY,0) FROM B_BOSS WHERE \"ID\"=(SELECT MZ_EXTPOS_SRANK+'01D' FROM A_DLBASE WHERE MZ_ID='" + temp.Rows[i]["MZ_ID"].ToString() + "' AND MZ_PCHIEF IS NOT NULL)");

                    int profess = TPPDDB._2_salary.SalaryBasic.GetProfessbyIDCard(temp.Rows[i]["MZ_ID"].ToString());

                    int pay1 = TPPDDB._2_salary.SalaryBasic.GetSalaryPaybyIDCard(temp.Rows[i]["MZ_ID"].ToString());

                    //int boss = TPPDDB._2_salary.SalaryBasic.GetBossbyIDCard(temp.Rows[i]["MZ_ID"].ToString());

                    double boss = string.IsNullOrEmpty(bossString) ? 0 : int.Parse(bossString);

                    string pb2String = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_PB2 FROM A_DLBASE WHERE MZ_ID='" + temp.Rows[i]["MZ_ID"].ToString() + "'");

                    int pb2 = int.Parse(string.IsNullOrEmpty(pb2String.Trim()) ? "0" : pb2String);

                    if (1 <= pb2 && pb2 < 12)
                    {
                        boss = Math.Round(boss / 12 * pb2, 0, MidpointRounding.AwayFromZero);
                    }

                    //總休假天數
                    int hday = 0;
                    //總休假小時數
                    int hour = 0;
                    ////今年保留天數
                    int sday3 = 0;
                    ////今年保留小時數
                    int sday3_hour = 0;

                    string stay_day = " SELECT NVL(MZ_HDAY,0) MZ_HDAY,NVL(MZ_HTIME,0) MZ_HTIME,NVL(MZ_SDAY3,0) MZ_SDAY3,NVL(MZ_SDAY3_HOUR,0) MZ_SDAY3_HOUR FROM C_DLTBB WHERE MZ_ID='" + temp.Rows[i]["MZ_ID"].ToString() + "' AND MZ_YEAR='" + TextBox_YEAR.Text + "' ";
                    DataTable temp_stay = o_DBFactory.ABC_toTest.Create_Table(stay_day, "stay");

                    if (temp_stay.Rows.Count > 0)
                    {
                        hday = int.Parse(temp_stay.Rows[0]["MZ_HDAY"].ToString());
                        hour = int.Parse(temp_stay.Rows[0]["MZ_HTIME"].ToString());
                        sday3 = int.Parse(temp_stay.Rows[0]["MZ_SDAY3"].ToString());
                        sday3_hour = int.Parse(temp_stay.Rows[0]["MZ_SDAY3_HOUR"].ToString());
                    }
                    stay_day = " SELECT NVL(SUM(MZ_TDAY),0) MZ_TDAY,NVL(SUM(MZ_TTIME),0) MZ_TTIME FROM C_DLTB01 WHERE MZ_CODE='03' AND MZ_ID='" + temp.Rows[i]["MZ_ID"].ToString() + "' AND dbo.SUBSTR(MZ_IDATE1,1,3)='" + TextBox_YEAR.Text + "' AND MZ_CHK1='Y' ";
                    temp_stay = o_DBFactory.ABC_toTest.Create_Table(stay_day, "stay");

                    //總請假天數
                    int hday_used = 0;
                    //總請假小時
                    int htime_used = 0;

                    if (temp_stay.Rows.Count > 0)
                    {
                        hday_used = int.Parse(temp_stay.Rows[0]["MZ_TDAY"].ToString());
                        htime_used = int.Parse(temp_stay.Rows[0]["MZ_TTIME"].ToString());

                    }


                    //總休假天數
                    //string hdaystring = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_HDAY FROM C_DLTBB WHERE MZ_ID='" + temp.Rows[i]["MZ_ID"].ToString() + "' AND MZ_YEAR='" + TextBox_YEAR.Text + "'");
                    //int hday = string.IsNullOrEmpty(hdaystring) ? 0 : int.Parse(hdaystring);
                    //總請假天數
                    //int hday_used = int.Parse(string.IsNullOrEmpty(o_DBFactory.ABC_toTest.vExecSQL("SELECT SUM(MZ_TDAY) FROM C_DLTB01 WHERE MZ_CODE='03' AND MZ_ID='" + temp.Rows[i]["MZ_ID"].ToString() + "' AND dbo.SUBSTR(MZ_IDATE1,1,3)='" + TextBox_YEAR.Text + "' AND MZ_CHK1='Y'")) ? "0" : o_DBFactory.ABC_toTest.vExecSQL("SELECT SUM(MZ_TDAY) FROM C_DLTB01 WHERE MZ_CODE='03' AND MZ_ID='" + temp.Rows[i]["MZ_ID"].ToString() + "' AND dbo.SUBSTR(MZ_IDATE1,1,3)='" + TextBox_YEAR.Text + "' AND MZ_CHK1='Y'"));
                    ////總請假小時
                    //int htime_used = int.Parse(string.IsNullOrEmpty(o_DBFactory.ABC_toTest.vExecSQL("SELECT SUM(MZ_TTIME) FROM C_DLTB01 WHERE MZ_CODE='03' AND MZ_ID='" + temp.Rows[i]["MZ_ID"].ToString() + "' AND dbo.SUBSTR(MZ_IDATE1,1,3)='" + TextBox_YEAR.Text + "' AND MZ_CHK1='Y'")) ? "0" : o_DBFactory.ABC_toTest.vExecSQL("SELECT SUM(MZ_TTIME) FROM C_DLTB01 WHERE MZ_CODE='03' AND MZ_ID='" + temp.Rows[i]["MZ_ID"].ToString() + "' AND dbo.SUBSTR(MZ_IDATE1,1,3)='" + TextBox_YEAR.Text + "' AND MZ_CHK1='Y'"));
                    //獎勵休假獎金天數 
                    //sam 休假天數14日以後可領不休假獎金，今年改為10日 20200721 by WellSince Sam     
                    int over_14hday = 0;
                    //獎勵休假獎金小時數
                    int over_14htime = 0;
                    //因公未休假天數
                    int noplayday = 0;
                    //因公未休假小時
                    int noplay_hour = 0;
                    ////今年保留天數
                    //int sday3 = int.Parse(string.IsNullOrEmpty(o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_SDAY3 FROM C_DLTBB WHERE MZ_ID='" + temp.Rows[i]["MZ_ID"].ToString() + "' AND MZ_YEAR='" + TextBox_YEAR.Text + "'")) ? "0" : o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_SDAY3 FROM C_DLTBB WHERE MZ_ID='" + temp.Rows[i]["MZ_ID"].ToString() + "' AND MZ_YEAR='" + TextBox_YEAR.Text + "'"));
                    ////今年保留小時數
                    //int sday3_hour = int.Parse(string.IsNullOrEmpty(o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_SDAY3_HOUR FROM C_DLTBB WHERE MZ_ID='" + temp.Rows[i]["MZ_ID"].ToString() + "' AND MZ_YEAR='" + TextBox_YEAR.Text + "'")) ? "0" : o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_SDAY3_HOUR FROM C_DLTBB WHERE MZ_ID='" + temp.Rows[i]["MZ_ID"].ToString() + "' AND MZ_YEAR='" + TextBox_YEAR.Text + "'"));
                    //不休假獎金發放天數
                    int pay_day = 0;
                    //不休假獎金發放小時數
                    int pay_hour = 0;
                    //獎勵休假金額
                    int over_14hday_pay = 0;
                    //不休假獎金一日金額
                    double payday_onedaypay = Math.Round(Convert.ToDouble(pay1 + boss + profess) / 30, 2, MidpointRounding.AwayFromZero);
                    double payday_onehourpay = Math.Round(Convert.ToDouble(pay1 + boss + profess) / 240, 0, MidpointRounding.AwayFromZero);
                    //不休假獎金金額-計算用
                    double payday_pay_double = 0;
                    //不休假獎金金額-顯示儲存用
                    int payday_pay = 0;

                    //總金額
                    int total_pay = 0;
                    //員工編號,併計年資,併計年資月
                    string strSQL1 = "SELECT MZ_POLNO,MZ_TYEAR,MZ_TMONTH FROM A_DLBASE WHERE MZ_ID='" + temp.Rows[i]["MZ_ID"].ToString() + "'";

                    DataTable temp1 = o_DBFactory.ABC_toTest.Create_Table(strSQL1, "GET");

                    string polno = temp1.Rows[0]["MZ_POLNO"].ToString();

                    string tyear = temp1.Rows[0]["MZ_TYEAR"].ToString();

                    if (!string.IsNullOrEmpty(tyear))
                    {
                        tyear = tyear + "年";
                    }

                    string tmonth = temp1.Rows[0]["MZ_TMONTH"].ToString();

                    if (!string.IsNullOrEmpty(tmonth))
                    {
                        tmonth = tmonth + "月";
                    }

                    if (hday < 10)
                    {
                        over_14hday = 0;
                        over_14htime = 0;
                        pay_day = 0;
                        pay_hour = 0;
                    }
                    else if (hday == 10 && hour == 0)
                    {
                        over_14hday = 0;
                        over_14htime = 0;
                        pay_day = 0;
                        pay_hour = 0;
                    }
                    else
                    {
                        //總休假小時
                        int totalhour = (hday_used * 8 + htime_used);

                        if ((totalhour) / 8 >= 10)
                        {
                            over_14hday = totalhour / 8 - 10;
                            over_14htime = totalhour % 8;
                        }
                        else
                        {
                            over_14hday = 0;
                            over_14htime = 0;
                        }

                        if (over_14htime > 0)
                        {
                            //不休假獎金發放小時數
                            pay_hour = 8 - over_14htime;
                            pay_day = hday - over_14hday - sday3 - 10 - 1;

                            if (sday3_hour > 0)//今年保留天數
                            {
                                pay_hour = 0;
                            }
                        }
                        else
                        {
                            //不休假獎金發放小時數
                            pay_hour = hour;
                            pay_day = hday - sday3 - over_14hday - 10;

                            if (sday3_hour > 0)
                            {
                                pay_hour = 8 - sday3_hour;
                                pay_day = hday - sday3 - over_14hday - 10 - 1;
                            }
                        }

                        if (over_14htime > 0)
                        {
                            noplay_hour = 8 - over_14htime;
                            noplayday = hday - 10 - over_14hday - 1;
                        }
                        else
                        {
                            noplay_hour = hour;
                            noplayday = hday - 10 - over_14hday;
                        }
                    }

                    // sam.hsu 新北市政府警察局交通警察大隊事故處理組109年員警因公未休假改發加班費印領清冊
                    if (pay_day < 0)
                    {
                        pay_day = 0;
                        pay_hour = 0;

                        noplayday = 0;
                        noplay_hour = 0;
                    }

                    //休假補助金額合計。  強制4小時改為每小時計算
                    over_14hday_pay = (over_14hday * 600) + (600 / 8 * over_14htime);

                    //不休假獎金一日金額*不休假獎金發放天數
                    //改為每小時計算 20181220 by sky
                    payday_pay_double = payday_onehourpay * 8 * pay_day + payday_onehourpay * pay_hour;
                    //payday_pay_double = payday_onedaypay * pay_day;
                    //if (pay_hour > 0)
                    //{
                    //    payday_pay_double += (payday_onedaypay / 8) * pay_hour;
                    //}
                    payday_pay = Convert.ToInt32(Math.Round(payday_pay_double, 0, MidpointRounding.AwayFromZero));

                    //不休假獎金金額 + 休假補助金和合計=獎勵休假金額
                    total_pay = payday_pay + over_14hday_pay;

                    string InsertString = "INSERT INTO C_NOPLAYPAY VALUES('" + temp.Rows[i]["MZ_ID"].ToString() + "','" + o_str.tosql(TextBox_YEAR.Text.Trim().PadLeft(3, '0')) + "'," + hday + ","
                                                                             + over_14hday + "," + over_14htime + ",600," + over_14hday_pay + "," + noplayday + "," + noplay_hour + "," + sday3 + "," + sday3_hour + ","
                                                                             + pay_day + "," + pay_hour + "," + payday_onedaypay + "," + payday_pay + "," + total_pay + ",'" + tyear.ToString() + tmonth.ToString() + "','" + temp.Rows[i]["PAY_AD"].ToString() + "','" + temp.Rows[i]["MZ_EXUNIT"].ToString() + "','" + temp.Rows[i]["OCCC"].ToString() + "','" + temp.Rows[i]["MZ_UNIT"].ToString() + "') ";

                    conn.Open();
                    //SqlTransaction connTraction = conn.BeginTransaction();
                    SqlCommand cmd = new SqlCommand(InsertString, conn);

                    //cmd.Transaction = connTraction;

                    try
                    {

                        cmd.ExecuteNonQuery();
                        //connTraction.Commit();

                        DataRow dr = NOPLAYLIST.NewRow();

                        dr["OCCC"] = o_A_KTYPE.CODE_TO_NAME(temp.Rows[i]["OCCC"].ToString(), "26");
                        dr["NAME"] = o_A_DLBASE.CNAME(temp.Rows[i]["MZ_ID"].ToString());
                        dr["FDATE"] = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_FDATE FROM A_DLBASE WHERE MZ_ID='" + temp.Rows[i]["MZ_ID"].ToString() + "'");
                        dr["HDAY"] = hday + "日" + hour + "時";
                        dr["ODAY"] = over_14hday.ToString() + "日" + over_14htime + "時";
                        dr["ODAYPAY"] = 600;
                        dr["ODAYPAY_TOTAL"] = over_14hday_pay;
                        dr["NOPLAYDAY"] = noplayday + "日" + noplay_hour + "時";
                        dr["SDAY3"] = sday3 + "日" + sday3_hour + "時";
                        dr["PAYDAY"] = pay_day + "日" + pay_hour + "時";
                        dr["DAYPAY"] = Convert.ToInt32(payday_onehourpay);
                        dr["NOPLAYDAY_TOTAL"] = payday_pay;
                        dr["TOTAL_PAY"] = total_pay;
                        dr["POLNO"] = polno;
                        dr["PAY1"] = pay1;
                        dr["PROFESS"] = profess;
                        dr["BOSS"] = boss;
                        dr["MOMTHPAY"] = pay1 + profess + boss;

                        //要顯示 備考資訊嗎?
                        //條件 搜尋所有單位 或 非搜尋所有單位但列印條件為支援其他機關
                        //如果 不是搜尋所有單位,且 為支援其他單位
                        bool isShowMEMO =
                            (RadioButtonList2.SelectedValue == "0")
                            || (RadioButtonList2.SelectedValue != "0" && RadioButtonList3.SelectedValue == "2");
                        if (isShowMEMO)
                        {
                            if (temp.Rows[i]["MZ_UNIT"].ToString() != temp.Rows[i]["MZ_EXUNIT"].ToString())
                                dr["MEMO"] = "支援" + o_A_KTYPE.CODE_TO_NAME(temp.Rows[i]["MZ_EXUNIT"].ToString(), "25") + "&N" + tyear.ToString() + tmonth.ToString();
                            else
                                dr["MEMO"] = tyear.ToString() + tmonth.ToString();
                        }
                        NOPLAYLIST.Rows.Add(dr);

                    }
                    catch (Exception ex)
                    {
                        //記錄錯誤訊息
                        o_DBFactory.ABC_toTest.SaveSQLLog(InsertString, ex);
                        //connTraction.Rollback();

                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('產生失敗！')", true);

                        return false;
                    }
                    finally
                    {
                        conn.Close();


                    }


                }//for (int i = 0; i < temp.Rows.Count; i++)

                conn.Dispose();
            }//using (SqlConnection conn
            //順利完成執行 不噴錯
            return true;
        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            TextBox_YEAR.Text = string.Empty;
        }

        protected void DropDownList_EXUNIT_DataBound(object sender, EventArgs e)
        {
            DropDownList_EXUNIT.Items.Insert(0, new ListItem(" ", ""));
        }

        protected void RadioButtonList2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RadioButtonList2.SelectedValue == "0")
            {
                RadioButtonList1.Visible = true;
                RadioButtonList3.Visible = false;
                DropDownList_EXUNIT.Enabled = true;
            }
            else
            {
                RadioButtonList3.Visible = true;
                RadioButtonList1.Visible = false;

                if (RadioButtonList3.SelectedValue == "2")
                {
                    DropDownList_EXUNIT.SelectedValue = "";
                    DropDownList_EXUNIT.Enabled = false;
                }
                else
                {
                    DropDownList_EXUNIT.Enabled = true;
                }
            }
        }

        protected void RadioButtonList3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RadioButtonList3.SelectedValue == "2")
            {
                DropDownList_EXUNIT.SelectedValue = "";
                DropDownList_EXUNIT.Enabled = false;
            }
            else
            {
                DropDownList_EXUNIT.Enabled = true;
            }
        }

        protected void DropDownList_EXAD_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (ViewState["C_strGID"].ToString() == "E")//權限E選擇所屬單位並鎖單位
            {
                DropDownList_EXUNIT.SelectedValue = Session["ADPMZ_EXUNIT"].ToString();
                DropDownList_EXUNIT.Enabled = false;

            }
            else
            {
                C.fill_unit(DropDownList_EXUNIT, DropDownList_EXAD.SelectedValue);
            }

        }
    }
}
