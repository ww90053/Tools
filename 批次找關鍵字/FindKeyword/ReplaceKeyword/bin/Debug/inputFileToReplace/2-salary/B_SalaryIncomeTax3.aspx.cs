using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Text;

namespace TPPDDB._2_salary
{
    public partial class B_SalaryIncomeTax3 : System.Web.UI.Page
    {
        public List<int> listData
        {
            get { return (List<int>)ViewState["listData"]; }
            set { ViewState["listData"] = value; }
        }

        int intCurrentIndex
        {
            get { return (int)ViewState["pageIndex"]; }
            set { ViewState["pageIndex"] = value; }
        }

        public Tax tax
        {
            get { return (Tax)ViewState["tax"]; }
            set { ViewState["tax"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
           

            if (IsPostBack != true)
            { 
                //檢驗權限
            SalaryPublic.checkPermission();
                listData = new List<int>();
                intCurrentIndex = 0;

                TextBox_AYEAR.Enabled = false;
                TextBox_TAX_TYPE.Enabled = false;
                TextBox_MZ_POLNO.Enabled = false;
                TextBox_MZ_NAME.Enabled = false;
                TextBox_MZ_ID.Enabled = false;

                txt_searchAyear.Text = (DateTime.Now.Year - 1911 - 1).ToString();

                voidClear();
                Label_MSG.Text = "";
                //20100312取消刪除功能
                btDelete.Visible = false;

                SalaryPublic.fillDropDownList(ref ddl_searchPayad);
                SalaryPublic.fillUnitDropDownList(ref ddl_searchUnit, ddl_searchPayad.SelectedValue);
            }
        }

        void ShowData()
        {
            // 避免輸入的索引大於資料總數
            if ((intCurrentIndex + 1) > listData.Count)
            {
                intCurrentIndex = listData.Count - 1;
                ViewState["CurrentIndex"] = intCurrentIndex;
            }

            btBack_Data.Enabled = false;
            btNext_Data.Enabled = false;

            if (listData.Count > 1)
            {
                // 不在第一筆的時候
                if (intCurrentIndex != 0)
                    btBack_Data.Enabled = true;

                // 不在最後一筆的時候
                if (intCurrentIndex != listData.Count - 1)
                    btNext_Data.Enabled = true;
            }

            Label_Pages.Visible = true;
            Label_Pages.Text = "第" + (intCurrentIndex + 1).ToString() + "筆；共" + listData.Count.ToString() + "筆";

            finddata(intCurrentIndex);
        }

        #region 介面屬性

        //年度
        private string strAYEAR
        {
            get
            {
                return TextBox_AYEAR.Text;
            }
            set
            {
                TextBox_AYEAR.Text = value;
            }
        }
        //所得格式
        private string strTAX_TYPE
        {
            get
            {
                return TextBox_TAX_TYPE.Text;
            }
            set
            {
                TextBox_TAX_TYPE.Text = value;
            }
        }
        //員工編號
        private string strMZ_POLNO
        {
            get
            {
                return TextBox_MZ_POLNO.Text;
            }
            set
            {
                TextBox_MZ_POLNO.Text = value;
            }
        }
        //姓名
        private string strMZ_NAME
        {
            get
            {
                return TextBox_MZ_NAME.Text;
            }
            set
            {
                TextBox_MZ_NAME.Text = value;
            }
        }
        //身分證號
        private string strMZ_ID
        {
            get
            {
                return TextBox_MZ_ID.Text;
            }
            set
            {
                TextBox_MZ_ID.Text = value;
            }
        }
        //戶籍地址
        private string strADDRESS1
        {
            get
            {
                return TextBox_ADDRESS1.Text;
            }
            set
            {
                TextBox_ADDRESS1.Text = value;
            }
        }
        //應發總額
        private int intINCREASE
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_INCREASE.Text);
            }
            set
            {
                TextBox_INCREASE.Text = value.ToString();
            }
        }
        //退撫免稅額
        int intCONCUR3PAY
        {
            get
            {
                return SalaryPublic.intdelimiterChars(txt_CONCUR3PAY.Text);
            }
            set
            {
                txt_CONCUR3PAY.Text = value.ToString();
            }
        }
        //給付總額 = 應發總額 - 免稅金額 - 主管加給 - 退撫免稅額
        int intPaysum
        {
            get
            {
                return SalaryPublic.intdelimiterChars(txt_paysum.Text);
            }
            set
            {
                txt_paysum.Text = value.ToString();
            }
        }
        //扣繳稅額
        private int intDECREASE
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_DECREASE.Text);
            }
            set
            {
                TextBox_DECREASE.Text = value.ToString();
            }
        }
        //給付淨額 
        private int intTOTAL
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_TOTAL.Text);
            }
            set
            {
                TextBox_TOTAL.Text = value.ToString();
            }
        }
        //主管加給
        int intBoss
        {
            get
            {
                return SalaryPublic.intdelimiterChars(txt_boss.Text);
            }
            set
            {
                txt_boss.Text = value.ToString();
            }
        }

        private int intOTHERSUB
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_OTHERSUB.Text);
            }
            set
            {
                TextBox_OTHERSUB.Text = value.ToString();
            }
        }

        int intOthersubTax
        {
            get
            {
                return SalaryPublic.intdelimiterChars(txt_othersubTax.Text);
            }
            set
            {
                txt_othersubTax.Text = value.ToString();
            }
        }
        //免稅金額
        int intDecrease
        {
            get
            {
                return SalaryPublic.intdelimiterChars(txt_decrease.Text);
            }
            set
            {
                txt_decrease.Text = value.ToString();
            }
        }

        private int intMONTHPAY
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_MONTHPAY.Text);
            }
            set
            {
                TextBox_MONTHPAY.Text = value.ToString();
            }
        }

        private int intMONTHPAYTAX
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_MONTHPAYTAX.Text);
            }
            set
            {
                TextBox_MONTHPAYTAX.Text = value.ToString();
            }
        }

        private int intREPAIR
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_REPAIR.Text);
            }
            set
            {
                TextBox_REPAIR.Text = value.ToString();
            }
        }

        private int intREPAIRTAX
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_REPAIRTAX.Text);
            }
            set
            {
                TextBox_REPAIRTAX.Text = value.ToString();
            }
        }

        private int intSOLE
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_SOLE.Text);
            }
            set
            {
                TextBox_SOLE.Text = value.ToString();
            }
        }

        private int intSOLETAX
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_SOLETAX.Text);
            }
            set
            {
                TextBox_SOLETAX.Text = value.ToString();
            }
        }

        private int intYEARPAY
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_YEARPAY.Text);
            }
            set
            {
                TextBox_YEARPAY.Text = value.ToString();
            }
        }

        private int intYEARPAYTAX
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_YEARPAYTAX.Text);
            }
            set
            {
                TextBox_YEARPAYTAX.Text = value.ToString();
            }
        }

        private int intEFFECT
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_EFFECT.Text);
            }
            set
            {
                TextBox_EFFECT.Text = value.ToString();
            }
        }

        private int intEFFECTTAX
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_EFFECTTAX.Text);
            }
            set
            {
                TextBox_EFFECTTAX.Text = value.ToString();
            }
        }

        private int intLABORPAY
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_LABORPAY.Text);
            }
            set
            {
                TextBox_LABORPAY.Text = value.ToString();
            }
        }

        //查詢資料使用

       

        #endregion

        private void voidClear()
        {
            Label_Pages.Text = "第0筆；共0筆";

            btBack_Data.Enabled = false;
            btNext_Data.Enabled = false;
            btUpdate.Enabled = false;
            btDelete.Enabled = false;
            btExit.Enabled = false;

            strAYEAR = "";
            strTAX_TYPE = "";
            lbl_payad.Text = "";
            strMZ_POLNO = "";
            strMZ_NAME = "";
            strMZ_ID = "";
            strADDRESS1 = "";
            //總計
            intINCREASE = 0;
            intDECREASE = 0;
            intTOTAL = 0;
            intOTHERSUB = 0;
            //各項給付
            intMONTHPAY = 0;
            intMONTHPAYTAX = 0;
            intREPAIR = 0;
            intREPAIRTAX = 0;
            intSOLE = 0;
            intSOLETAX = 0;
            intYEARPAY = 0;
            intYEARPAYTAX = 0;
            intEFFECT = 0;
            intEFFECTTAX = 0;
            intLABORPAY = 0;
            intBoss = 0;
            intPaysum = 0;

            gv_salary.DataSource = null;
            gv_salary.DataBind();

            SqlDataSource_TAXES_DATA.SelectCommand = "";
        }

        protected void btBack_Click(object sender, EventArgs e)
        {
            intCurrentIndex--;
            ShowData();
        }

        protected void btNext_Click(object sender, EventArgs e)
        {
            intCurrentIndex++;
            ShowData();
        }

        protected void finddata(int index)
        {
            btUpdate.Enabled = false;
            btDelete.Enabled = false;
            btExit.Enabled = false;
            tax = new Tax(listData[index]);
            if (tax.sn == 0)
                return;

            //還沒關帳的資料才能修改
            if (!tax.lockdb)
            {
                btUpdate.Enabled = true;
                btDelete.Enabled = true;
                btExit.Enabled = true;
            }

            //基本資料
            strAYEAR = tax.year;
            strTAX_TYPE = tax.taxType;
            lbl_payad.Text = tax.chiad;
            lbl_unit.Text = tax.chiunit;
            strMZ_POLNO = tax.polno;
            lbl_occc.Text = tax.chioccc;
            strMZ_NAME = tax.name;
            strMZ_ID = tax.idcard;
            strADDRESS1 = tax.address;

            //各項給付
            intMONTHPAY = tax.monthPay;
            intMONTHPAYTAX = tax.monthTax;
            intREPAIR = tax.repairPay;
            intREPAIRTAX = tax.repairTax;
            intSOLE = tax.solePay;
            intSOLETAX = tax.soleTax;
            intYEARPAY = tax.yearPay;
            intYEARPAYTAX = tax.yearTax;
            intEFFECT = tax.effectPay;
            intEFFECTTAX = tax.effectTax;
            intBoss = tax.bosspay;
            intOTHERSUB = tax.otherSub;
            intOthersubTax = tax.othersubTax;
            intDecrease = tax.decrease;
            intLABORPAY = tax.laborpay;
            intCONCUR3PAY = tax.CONCUR3PAY;

            gv_salary.DataSource = tax.salaryData;
            gv_salary.DataBind();

            refreshTax();
        }

        private static DataTable Dt = new DataTable();

        //處理每種金額的明細，已先拿掉
        //private void voidDATA_VIEW(string strIDCARD, string strAYEAR)
        //{
        //    //DataTable Dt = new DataTable();
        //    Dt.Clear();
        //    Dt.Columns.Clear();
        //    Dt.Columns.Add("DATA_DATE", typeof(String));
        //    Dt.Columns.Add("DATA_INREASE", typeof(String));
        //    Dt.Columns.Add("DATA_DEREASE", typeof(String));
        //    Dt.Columns.Add("DATA_TYPE", typeof(String));

        //    int intINREASE = 0;
        //    int intDEREASE = 0;

        //    for (int i = 1; i <= 12; i++)
        //    {
        //        DataRow row = Dt.NewRow();

        //        row["DATA_DATE"] = (strAYEAR + i.ToString("00")).ToString();
        //        intINREASE = (SalaryMonth_View.intSearch_SalaryMonth_Data_Increase(strAYEAR + i.ToString("00"), strIDCARD) - SalaryMonth_View.intSearch_SalaryMonth_Data_Boss(strAYEAR + i.ToString("00"), strIDCARD));
        //        string strINREASE_Data = "";
        //        if (intINREASE == 0)
        //        {
        //            strINREASE_Data = "$0";
        //        }
        //        else
        //        {
        //            strINREASE_Data = intINREASE.ToString("$#,#");
        //        }
        //        row["DATA_INREASE"] = strINREASE_Data;

        //        intDEREASE = (SalaryMonth_View.intSearch_SalaryMonth_Data_MonthpayTax(strAYEAR + i.ToString("00"), strIDCARD) + SalaryMonth_View.intSearch_SalaryMonth_Data_Tax(strAYEAR + i.ToString("00"), strIDCARD));
        //        string strDEREASE_Data = "";
        //        if (intDEREASE == 0)
        //        {
        //            strDEREASE_Data = "$0";
        //        }
        //        else
        //        {
        //            strDEREASE_Data = intDEREASE.ToString("$#,#");
        //        }
        //        row["DATA_DEREASE"] = strDEREASE_Data;

        //        row["DATA_TYPE"] = "每月";
        //        Dt.Rows.Add(row);
        //    }


        //    for (int i = 1; i <= 12; i++)
        //    {
        //        DataRow row = Dt.NewRow();

        //        row["DATA_DATE"] = (strAYEAR + i.ToString("00")).ToString();
        //        intINREASE = (SalaryRepair.intSearch_SalaryRepair_Data_Increase(strAYEAR + i.ToString("00"), strIDCARD) - SalaryRepair.intSearch_SalaryRepair_Data_BOSS(strAYEAR + i.ToString("00"), strIDCARD));
        //        string strINREASE_Data = "";
        //        if (intINREASE == 0)
        //        {
        //            strINREASE_Data = "$0";
        //        }
        //        else
        //        {
        //            strINREASE_Data = intINREASE.ToString("$#,#");
        //        }
        //        row["DATA_INREASE"] = strINREASE_Data;

        //        intDEREASE = SalaryRepair.intSearch_SalaryRepair_Data_MONTHPAY(strAYEAR + i.ToString("00"), strIDCARD);
        //        string strDEREASE_Data = "";
        //        if (intDEREASE == 0)
        //        {
        //            strDEREASE_Data = "$0";
        //        }
        //        else
        //        {
        //            strDEREASE_Data = intDEREASE.ToString("$#,#");
        //        }
        //        row["DATA_DEREASE"] = strDEREASE_Data;

        //        row["DATA_TYPE"] = "補發";
        //        Dt.Rows.Add(row);
        //    }

        //    for (int i = 0; i < 1; i++)
        //    {
        //        DataRow row = Dt.NewRow();

        //        row["DATA_DATE"] = strAYEAR;

        //        intINREASE = (SalaryTax.intSOLE_IN_Create(strIDCARD, strAYEAR, strPAY_AD, strTAX_TYPE) - SalaryTax.intSOLE_OUT_Create(strIDCARD, strAYEAR, strPAY_AD, strTAX_TYPE));
        //        string strINREASE_Data = "";
        //        if (intINREASE == 0)
        //        {
        //            strINREASE_Data = "$0";
        //        }
        //        else
        //        {
        //            strINREASE_Data = intINREASE.ToString("$#,#");
        //        }
        //        row["DATA_INREASE"] = strINREASE_Data;

        //        intDEREASE = (SalaryTax.intSOLETAX_IN_Create(strIDCARD, strAYEAR, strPAY_AD, strTAX_TYPE) + SalaryTax.intSOLETAX1_IN_Create(strIDCARD, strAYEAR, strPAY_AD, strTAX_TYPE)) - (SalaryTax.intSOLETAX_OUT_Create(strIDCARD, strAYEAR, strPAY_AD, strTAX_TYPE) + SalaryTax.intSOLETAX1_OUT_Create(strIDCARD, strAYEAR, strPAY_AD, strTAX_TYPE));
        //        string strDEREASE_Data = "";
        //        if (intDEREASE == 0)
        //        {
        //            strDEREASE_Data = "$0";
        //        }
        //        else
        //        {
        //            strDEREASE_Data = intDEREASE.ToString("$#,#");
        //        }
        //        row["DATA_DEREASE"] = strDEREASE_Data;
        //        row["DATA_TYPE"] = "單一";
        //        Dt.Rows.Add(row);
        //    }

        //    for (int i = 0; i < 1; i++)
        //    {
        //        DataRow row = Dt.NewRow();

        //        row["DATA_DATE"] = strAYEAR;

        //        intINREASE = SalaryTax.intYearEndbonus_Create(strIDCARD, strAYEAR);
        //        string strINREASE_Data = "";
        //        if (intINREASE == 0)
        //        {
        //            strINREASE_Data = "$0";
        //        }
        //        else
        //        {
        //            strINREASE_Data = intINREASE.ToString("$#,#");
        //        }
        //        row["DATA_INREASE"] = strINREASE_Data;

        //        intDEREASE = SalaryTax.intYearEndbonusTAX_Create(strIDCARD, strAYEAR);
        //        string strDEREASE_Data = "";
        //        if (intDEREASE == 0)
        //        {
        //            strDEREASE_Data = "$0";
        //        }
        //        else
        //        {
        //            strDEREASE_Data = intDEREASE.ToString("$#,#");
        //        }
        //        row["DATA_DEREASE"] = strDEREASE_Data;
        //        row["DATA_TYPE"] = "年終";
        //        Dt.Rows.Add(row);
        //    }

        //    for (int i = 0; i < 1; i++)
        //    {
        //        DataRow row = Dt.NewRow();

        //        row["DATA_DATE"] = strAYEAR;
        //        intINREASE = SalaryTax.intEFFECT_Create(strIDCARD, strAYEAR);
        //        string strINREASE_Data = "";
        //        if (intINREASE == 0)
        //        {
        //            strINREASE_Data = "$0";
        //        }
        //        else
        //        {
        //            strINREASE_Data = intINREASE.ToString("$#,#");
        //        }
        //        row["DATA_INREASE"] = strINREASE_Data;

        //        intDEREASE = SalaryTax.intEFFECTTAX_Create(strIDCARD, strAYEAR);
        //        string strDEREASE_Data = "";
        //        if (intDEREASE == 0)
        //        {
        //            strDEREASE_Data = "$0";
        //        }
        //        else
        //        {
        //            strDEREASE_Data = intDEREASE.ToString("$#,#");
        //        }
        //        row["DATA_DEREASE"] = strDEREASE_Data;
        //        row["DATA_TYPE"] = "考績";
        //        Dt.Rows.Add(row);
        //    }

        //    //TBGridView_DATA_VIEW.DataSource = Dt;
        //    //TBGridView_DATA_VIEW.DataBind();
        //}

        private void voidTAX_TYPE(string strIDCARD)
        {
            SqlDataSource_TAXES_DATA.SelectCommand = string.Format("SELECT T_SNID, IDCARD, TAX_TYPE FROM B_TAXES WHERE IDCARD = '{0}' and AYEAR = '{1}'", strIDCARD, strAYEAR);
        }

        protected void btUpdate_Click(object sender, EventArgs e)
        {
            if (tax == null)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('讀取所得稅資料時發生異常');", true);
                return;
            }

            if (tax.update(intOTHERSUB, intOthersubTax, intDecrease, intLABORPAY, TextBox_ADDRESS1.Text,intCONCUR3PAY))
            {
                tax = new Tax(tax.sn);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('修改成功');", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('修改失敗，請再試一次');", true);
            }
        }

        protected void btDelete_Click(object sender, EventArgs e)
        {
            if (tax == null)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('讀取所得稅資料時發生異常');", true);
                return;
            }

            tax.delete();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('刪除成功');", true);
        }

        protected void btExit_Click(object sender, EventArgs e)
        {
            voidClear();
        }

        protected void refreshTax(object sender, EventArgs e)
        {
            refreshTax();
        }

        protected void refreshTax()
        {
            intINCREASE = intMONTHPAY + intREPAIR + intSOLE + intYEARPAY + intEFFECT - intOTHERSUB;
            //給付總額 = 應發總額 - 免稅金額 - 主管加給 - 退撫免稅額
            intPaysum = intINCREASE - intBoss - intDecrease - intCONCUR3PAY;
            intDECREASE = intMONTHPAYTAX + intREPAIRTAX + intSOLETAX + intYEARPAYTAX + intEFFECTTAX - intLABORPAY - intOthersubTax;
            intTOTAL = intPaysum - intDECREASE;
        }

        /*
        protected void voidChanged(object sender, EventArgs e)
        {
            switch ((sender as TextBox).ID)
            {
                case "TextBox_OTHERSUB":
                    intOTHERSUB = SalaryPublic.intdelimiterChars(TextBox_OTHERSUB.Text);
                    voidSelectControl(TextBox_MONTHPAY);
                    break;
                case "TextBox_MONTHPAY":
                    intMONTHPAY = SalaryPublic.intdelimiterChars(TextBox_MONTHPAY.Text);
                    voidSelectControl(TextBox_MONTHPAYTAX);
                    break;
                case "TextBox_MONTHPAYTAX":
                    intMONTHPAYTAX = SalaryPublic.intdelimiterChars(TextBox_MONTHPAYTAX.Text);
                    voidSelectControl(TextBox_REPAIR);
                    break;
                case "TextBox_REPAIR":
                    intREPAIR = SalaryPublic.intdelimiterChars(TextBox_REPAIR.Text);
                    voidSelectControl(TextBox_REPAIRTAX);
                    break;
                case "TextBox_REPAIRTAX":
                    intREPAIRTAX = SalaryPublic.intdelimiterChars(TextBox_REPAIRTAX.Text);
                    voidSelectControl(TextBox_SOLE);
                    break;
                case "TextBox_SOLE":
                    intSOLE = SalaryPublic.intdelimiterChars(TextBox_SOLE.Text);
                    voidSelectControl(TextBox_SOLETAX);
                    break;
                case "TextBox_SOLETAX":
                    intSOLETAX = SalaryPublic.intdelimiterChars(TextBox_SOLETAX.Text);
                    voidSelectControl(TextBox_YEARPAY);
                    break;
                case "TextBox_YEARPAY":
                    intYEARPAY = SalaryPublic.intdelimiterChars(TextBox_YEARPAY.Text);
                    voidSelectControl(TextBox_YEARPAYTAX);
                    break;
                case "TextBox_YEARPAYTAX":
                    intYEARPAYTAX = SalaryPublic.intdelimiterChars(TextBox_YEARPAYTAX.Text);
                    voidSelectControl(TextBox_EFFECT);
                    break;
                case "TextBox_EFFECT":
                    intEFFECT = SalaryPublic.intdelimiterChars(TextBox_EFFECT.Text);
                    voidSelectControl(TextBox_EFFECTTAX);
                    break;
                case "TextBox_EFFECTTAX":
                    intEFFECTTAX = SalaryPublic.intdelimiterChars(TextBox_EFFECTTAX.Text);
                    voidSelectControl(TextBox_LABORPAY);
                    break;
                case "TextBox_LABORPAY":
                    intLABORPAY = SalaryPublic.intdelimiterChars(TextBox_LABORPAY.Text);
                    break;
            }

            intINCREASE = ((intMONTHPAY + intREPAIR + intSOLE + intYEARPAY + intEFFECT) - intOTHERSUB);
            intDECREASE = intMONTHPAYTAX + intREPAIRTAX + intSOLETAX + intYEARPAYTAX + intEFFECTTAX;
            intTOTAL = (intINCREASE - intDECREASE);
        }
        */

        private void voidSelectControl(object ojNext)
        {
            if (ojNext is Button)
            {
                (ojNext as Button).Focus();
            }
            else if (ojNext is TextBox)
            {
                (ojNext as TextBox).Focus();
            }
            else if (ojNext is DropDownList)
            {
                (ojNext as DropDownList).Focus();
            }
            else if (ojNext is RadioButtonList)
            {
                (ojNext as RadioButtonList).Focus();
            }
        }

        protected void TextBox_TAX_TYPE_TextChanged(object sender, EventArgs e)
        {
            if (strTAX_TYPE == "51")
            {
                trLabor.Visible = false;
                intLABORPAY = 0;
            }
            else
            {
                trLabor.Visible = true;
            }
        }

        protected void TBGridView_DATA_VIEW_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //TBGridView_DATA_VIEW.DataSource = Dt;
            //TBGridView_DATA_VIEW.PageIndex = e.NewPageIndex;
            //TBGridView_DATA_VIEW.DataBind();
        }

        protected void ddl_searchPayad_SelectedIndexChanged(object sender, EventArgs e)
        {
            SalaryPublic.fillUnitDropDownList(ref ddl_searchUnit, ddl_searchPayad.SelectedValue);
            btn_popup_ModalPopupExtender.Show();
        }

        //查詢確認
        protected void btn_searchConfirm_Click(object sender, EventArgs e)
        {
            if (txt_searchAyear.Text == "")
            {
                btn_popup_ModalPopupExtender.Show();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('請輸入年度後才可查詢');", true);
                return;
            }

            btNext_Data.Enabled = false;
            btBack_Data.Enabled = false;
            listData.Clear();
            DataTable dt;

            dt = Tax.getTaxes(txt_searchAyear.Text, ddl_searchPayad.SelectedValue,
                ddl_searchUnit.SelectedIndex == 0 ? null : ddl_searchUnit.SelectedValue,
                txt_searchTaxType.Text == "" ? null : txt_searchTaxType.Text,
                txt_searchID.Text == "" ? null : txt_searchID.Text,
                txt_searchName.Text == "" ? null : txt_searchName.Text);

            if (dt == null)
            {
                voidClear();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('查無資料');", true);
                return;
            }

            foreach (DataRow row in dt.Rows)
            {
                listData.Add(int.Parse(row["T_SNID"].ToString()));
            }

            ShowData();
        }

        protected void bt_InData_Click(object sender, EventArgs e)
        {
            //?TPM_FION=1064
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", string.Format("window.location='35-DecreaseImport.aspx?TPM_FION={0}'", Request.QueryString["TPM_FION"]), true);
        }

    }
}
