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
    public partial class B_SalaryBasic1 : System.Web.UI.Page
    {
        SalaryPublic SP = new SalaryPublic();
        // string bperno = "100";
        Police police
        {
            get { return (Police)ViewState["police"]; }
            set { ViewState["police"] = value; }
        }
        DataTable healthInfo
        {
            get { return (DataTable)ViewState["healthInfo"]; }
            set { ViewState["healthInfo"] = value; }
        }
        string searchSQL
        {
            get { return ViewState["searchSQL"].ToString(); }
            set { ViewState["searchSQL"] = value; }
        }

        string GroupPermission { get { return SalaryPublic.GetGroupPermission(); } }


        #region 屬性

        /// <summary>
        /// 薪俸職等
        /// </summary> 
        string srank
        {
            get { return txt_MZ_SRANK.Text; }
            set
            {
                txt_MZ_SRANK.Text = value;
                lbl_MZ_SRANK.Text = o_A_KTYPE.CODE_TO_NAME(srank, "09");
            }
        }


        /// <summary>
        /// 俸階
        /// </summary> 
        string slvc
        {
            get { return txt_slvc.Text; }
            set
            {
                txt_slvc.Text = value;
                lbl_slvc.Text = o_A_KTYPE.CODE_TO_NAME(slvc, "64");
            }
        }


        /// <summary>
        /// 俸階
        /// </summary> 
        string tempSPT
        {
            get { return txt_tempSPT.Text; }
            set { txt_tempSPT.Text = value; }
        }

        /// <summary>
        /// 主管級別
        /// </summary>
        string pchief
        {
            get { return txt_pchief.Text; }
            set
            {
                txt_pchief.Text = value;
                lbl_pchief.Text = o_A_KTYPE.CODE_TO_NAME(pchief, "56");
            }
        }

        /// <summary>
        ///  是否兼代職
        /// </summary>
        string isExtpos
        {
            get { return rbl_isExtpos.SelectedValue; }
            set
            {
                rbl_isExtpos.SelectedValue = value;
            }
        }


        /// <summary>
        /// 兼代職級別
        /// </summary>
        string extposSrank
        {
            get { return txt_extposSrank.Text; }
            set
            {
                txt_extposSrank.Text = value;
                lbl_extposSrank.Text = o_A_KTYPE.CODE_TO_NAME(extposSrank, "09");
            }
        }

        /// <summary>
        /// 記錄彈出的CodeSelector是哪個控制項觸發的
        /// </summary>
        string codeType
        {
            get { return ViewState["codeType"].ToString(); }
            set { ViewState["codeType"] = value; }
        }


        /// <summary>
        /// 是否停職
        /// </summary>
        string isOffDuty
        {
            get { return rbl_IsOffDuty.SelectedValue; }
            set
            {
                if (value == "")
                    value = "否";
                rbl_IsOffDuty.SelectedValue = value;
            }
        }


        /// <summary>
        /// 是不是鑑識人員
        /// </summary>
        string isCrimlab
        {
            get { return rbl_IsCrimelab.SelectedValue; }
            set
            {
                if (value == "")
                    rbl_IsCrimelab.SelectedValue = "否";
                else
                    rbl_IsCrimelab.SelectedValue = value;
            }
        }


        ///// <summary>
        ///// 任公職是否滿30年
        ///// </summary>
        //string is30year
        //{
        //    get { return rbl_30Year.SelectedValue; }
        //    set
        //    {
        //        if (value == "")
        //            rbl_30Year.SelectedValue = "否";
        //        else
        //            rbl_30Year.SelectedValue = value;
        //    }
        //}

        bool healthChanged
        {
            get { return (bool)ViewState["healthChanged"]; }
            set
            {
                ViewState["healthChanged"] = value;
            }
        }

        /// <summary>
        /// 個人健保費
        /// </summary>
        int healthPersonal
        {
            get
            {
                if (txt_HealthPersonal.Text == "")
                    return 0;
                return int.Parse(txt_HealthPersonal.Text);
            }
            set { txt_HealthPersonal.Text = value.ToString(); }
        }
        #endregion 屬性

        #region 變數

        private string strMZ_ID_Data
        {
            get
            {
                return TextBox_MZ_ID.Text.ToUpper();
            }
            set
            {
                TextBox_MZ_ID.Text = value;
            }
        }

        private string strMZ_NAME_Data
        {
            get { return TextBox_MZ_NAME.Text; }
            set
            {
                TextBox_MZ_NAME.Text = value;
            }
        }

        private string strMZ_OCCC_Data
        {
            get
            {
                return TextBox_MZ_OCCC.Text;
            }
            set
            {
                TextBox_MZ_OCCC.Text = value;
                lb_occc.Text = o_A_KTYPE.CODE_TO_NAME(TextBox_MZ_OCCC.Text, "26");
            }
        }

        private string strMZ_AD_Data
        {
            get { return TextBox_MZ_AD.Text; }
            set
            {
                TextBox_MZ_AD.Text = value;
                lb_ad.Text = o_A_KTYPE.CODE_TO_NAME(TextBox_MZ_AD.Text, "04");
            }
        }

        private string strMZ_UNIT_Data
        {
            get { return TextBox_MZ_UNIT.Text; }
            set
            {
                TextBox_MZ_UNIT.Text = value;
                lb_unit.Text = o_A_KTYPE.CODE_TO_NAME(TextBox_MZ_UNIT.Text, "25");
            }
        }

        private string strMZ_EXAD_Data
        {
            get { return TextBox_MZ_EXAD.Text; }
            set
            {
                TextBox_MZ_EXAD.Text = value;
                lb_exad.Text = o_A_KTYPE.CODE_TO_NAME(TextBox_MZ_EXAD.Text, "04");
            }
        }

        private string strMZ_EXUNIT_Data
        {
            get { return TextBox_MZ_EXUNIT.Text; }
            set
            {
                TextBox_MZ_EXUNIT.Text = value;
                lb_exunit.Text = o_A_KTYPE.CODE_TO_NAME(TextBox_MZ_EXUNIT.Text, "25");
            }
        }

        private string strPAY_AD_Data
        {
            get
            {
                return DropDownList_PAY_AD.SelectedValue;
            }
            set
            {
                DropDownList_PAY_AD.Items.Clear();
                DropDownList_PAY_AD.Items.Add(new ListItem("", ""));
                if (value != "")
                    DropDownList_PAY_AD.Items.Add(new ListItem(o_A_KTYPE.CODE_TO_NAME(value, "04"), value));
                if (value != SalaryPublic.strLoginEXAD)
                    DropDownList_PAY_AD.Items.Add(new ListItem(o_A_KTYPE.CODE_TO_NAME(SalaryPublic.strLoginEXAD, "04"), SalaryPublic.strLoginEXAD));
                DropDownList_PAY_AD.SelectedValue = value;
            }
        }

        private string strMZ_POLNO_Data
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

        private string strMZ_BIR
        {
            set
            {
                TextBox_MZ_BIR.Text = value;
            }
        }

        private string strMZ_PHONO
        {
            set
            {
                TextBox_MZ_PHONO.Text = value;
            }
        }

        private string strMZ_PHONE
        {
            set
            {
                TextBox_MZ_PHONE.Text = value;
            }
        }

        private string strMZ_ADD2
        {
            get { return TextBox_MZ_ADD2.Text; }
            set
            {
                TextBox_MZ_ADD2.Text = value;
            }

        }

        private string strMZ_ADD1
        {
            set
            {
                TextBox_MZ_ADD1.Text = value;
            }

        }

        private string strMZ_SEX
        {
            set
            {
                if (value != "1" && value != "2")
                    value = "1";
                rbl_sex.SelectedValue = value;
            }
        }

        private string strHEALPER_Data
        {
            get
            {
                return TextBox_HEALPER_Data.Text;
            }
            set
            {
                TextBox_HEALPER_Data.Text = value;
            }
        }

        private string strMZ_SPT
        {
            get
            {
                return TextBox_MZ_SPT.Text;
            }
            set
            {
                TextBox_MZ_SPT.Text = value;
            }
        }

        private string strMZ_FDATE
        {
            get
            {
                return TextBox_MZ_FDATE.Text;
            }
            set
            {
                TextBox_MZ_FDATE.Text = value;
            }
        }

        private string strMZ_ADATE
        {
            set
            {
                TextBox_MZ_ADATE.Text = value;
            }
            get
            {
                return TextBox_MZ_ADATE.Text;
            }
        }

        private string strMZ_LDATE
        {
            set
            {
                TextBox_MZ_LDATE.Text = value;
            }
            get
            {
                return TextBox_MZ_LDATE.Text;
            }
        }

        private string strMZ_GRADE
        {
            set
            {
                TextBox_MZ_GRADE.Text = value;
            }
            get
            {
                return TextBox_MZ_GRADE.Text;
            }
        }

        private string strMZ_TDATE
        {
            get { return txt_MZ_TDATE.Text; }
            set { txt_MZ_TDATE.Text = value; }
        }

        private string strMZ_ODATE
        {
            get { return txt_MZ_ODATE.Text; }
            set { txt_MZ_ODATE.Text = value; }
        }

        private string strMZ_PCHIEFDATE
        {
            set
            {
                TextBox_MZ_PCHIEFDATE.Text = value;
            }
        }

        private int intSALARY_PAY1
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_SALARY_PAY1.Text);
            }
            set
            {
                TextBox_SALARY_PAY1.Text = SalaryPublic.strMoneyFormat(value.ToString());
            }
        }

        private int intCONCUR3_Txt
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_CONCUR3.Text);
            }
            set
            {
                TextBox_CONCUR3.Text = SalaryPublic.strMoneyFormat(value.ToString());
            }
        }

        private int intHEALPER_Data
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_HEALPER.Text);
            }
            set
            {
                TextBox_HEALPER.Text = SalaryPublic.strMoneyFormat(value.ToString());
            }
        }

        private string strINSURANCE_GROUP
        {
            get
            {
                return RadioButtonList_INSURANCE_GROUP.SelectedValue;
            }
            set
            {
                RadioButtonList_INSURANCE_GROUP.SelectedValue = value;
            }
        }

        private int intSALARY_GOV
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_SALARY_GOV_INSURANCE.Text);
            }
            set
            {
                TextBox_SALARY_GOV_INSURANCE.Text = SalaryPublic.strMoneyFormat(value.ToString());
            }
        }

        private string strCONCUR3_Data
        {
            get
            {
                return RadioButtonList_CONCUR3.SelectedValue;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    RadioButtonList_CONCUR3.SelectedValue = "2";
                }
                else if (value == "1")
                {
                    RadioButtonList_CONCUR3.SelectedValue = "1";
                }
                else if (value == "2")
                {
                    RadioButtonList_CONCUR3.SelectedValue = "2";
                }
            }
        }

        private int intBOSS_Data
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_BOSS.Text);
            }
            set
            {
                TextBox_BOSS.Text = SalaryPublic.strMoneyFormat(value.ToString());
            }
        }

        private int intPROFESS_Data
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_PROFESS.Text);
            }
            set
            {
                TextBox_PROFESS.Text = SalaryPublic.strMoneyFormat(value.ToString());
            }
        }

        private string strTECHNICS_Data
        {
            get
            {
                return DropDownList_TECHNICS.SelectedValue;
            }
            set
            {
                if (value == "")
                {
                    value = "0";
                }
                DropDownList_TECHNICS.SelectedValue = value;
            }
        }

        private string strBONUS_Data
        {
            get
            {
                return DropDownList_BONUS.SelectedValue;
            }
            set
            {
                if (value == "")
                {
                    value = "0";
                }
                DropDownList_BONUS.SelectedValue = value;
            }
        }

        private int intWORKPPAY_Data
        {
            get
            {
                return SalaryPublic.intdelimiterChars(police.workp.ToString());
            }
            set
            {
                return;
            }
        }
        private int intTECHNICS_Data
        {
            get
            {
                return SalaryPublic.intdelimiterChars(police.technic.ToString());
            }
            set
            {
                return;
            }
        }
        private int intBONUS_Data
        {
            get
            {
                return SalaryPublic.intdelimiterChars(police.bonus.ToString());
            }
            set
            {
                return;
            }
        }


        private string strWORKP_Data
        {
            get
            {
                return DropDownList_WORKP.SelectedValue;
            }
            set
            {
                if (value == "")
                {
                    value = "0";
                }
                //DropDownList_WORKP.SelectedValue = value;
                DropDownList_WORKP.SelectedItem.Text = value;
            }
        }

        private string strADVENTIVE_Data
        {
            get
            {
                return DropDownList_ADVENTIVE.SelectedValue;
            }
            set
            {
                if (value == "")
                {
                    value = "0";
                }
                DropDownList_ADVENTIVE.SelectedValue = value;
            }
        }

        private int intFarPay
        {
            get
            {
                return SalaryPublic.intdelimiterChars(txt_Far.Text);
            }
            set
            {
                txt_Far.Text = SalaryPublic.strMoneyFormat(value.ToString());
            }
        }



        private int intELECTRIC_Data
        {
            get
            {

                return int.Parse(txt_ELECTRIC.Text);
            }
            set
            {
                if (value.ToString() == "")
                {
                    value = 0;
                }
                //DropDownList_ELECTRIC.SelectedValue = value;
                txt_ELECTRIC.Text = value.ToString();
            }
        }

        //private string strELECTRIC_Data
        //{
        //    get
        //    {
        //       
        //        return DropDownList_ELECTRIC.SelectedValue;
        //    }
        //    set
        //    {
        //        if (value == "")
        //        {
        //            value = "0";
        //        }
        //        DropDownList_ELECTRIC.SelectedValue = value;
        //       
        //    }
        //}

        private int intTAXPER_Data
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_TAXPER.Text);
            }
            set
            {
                TextBox_TAXPER.Text = SalaryPublic.strMoneyFormat(value.ToString());
            }
        }

        private int intDE_HEALTH
        {
            get
            {
                return int.Parse(TextBox_DE_HEALTH.Text);
                //return SalaryPublic.intdelimiterChars(TextBox_DE_HEALTH.Text);
            }
            set
            {
                TextBox_DE_HEALTH.Text = value.ToString();
                //TextBox_DE_HEALTH.Text = SalaryPublic.strMoneyFormat(value.ToString());
            }
        }

        private string strTAXPER_GROUP
        {
            get
            {
                return RadioButtonList_TAXPER_GROUP.SelectedValue;
            }
            set
            {
                if (value == "")
                {
                    value = "否";
                }
                RadioButtonList_TAXPER_GROUP.SelectedValue = value;
            }
        }

        private int intTAXCHILD
        {
            get
            {
                if (TextBox_TAXCHILD.Text == "")
                {
                    TextBox_TAXCHILD.Text = "0";
                }
                return int.Parse(TextBox_TAXCHILD.Text);
            }
            set
            {
                TextBox_TAXCHILD.Text = value.ToString();
            }
        }

        private string strEXTRA01_GROUP
        {
            get
            {
                return RadioButtonList_EXTRA01_GROUP.SelectedValue;
            }
            set
            {
                if (value == "")
                {
                    value = "否";
                }
                RadioButtonList_EXTRA01_GROUP.SelectedValue = value;
            }
        }

        private int intEXTRA01_Data
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_EXTRA01.Text);
            }
            set
            {
                TextBox_EXTRA01.Text = SalaryPublic.strMoneyFormat(value.ToString());
            }
        }

        private int intEXTRA02_Data
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_EXTRA02.Text);
            }
            set
            {
                TextBox_EXTRA02.Text = SalaryPublic.strMoneyFormat(value.ToString());
            }
        }

        private int intEXTRA03_Data
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_EXTRA03.Text);
            }
            set
            {
                TextBox_EXTRA03.Text = SalaryPublic.strMoneyFormat(value.ToString());
            }
        }

        private int intEXTRA04_Data
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_EXTRA04.Text);
            }
            set
            {
                TextBox_EXTRA04.Text = SalaryPublic.strMoneyFormat(value.ToString());
            }
        }

        private int intEXTRA05_Data
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_EXTRA05.Text);
            }
            set
            {
                TextBox_EXTRA05.Text = SalaryPublic.strMoneyFormat(value.ToString());
            }
        }

        private int intEXTRA06_Data
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_EXTRA06.Text);
            }
            set
            {
                TextBox_EXTRA06.Text = SalaryPublic.strMoneyFormat(value.ToString());
            }
        }

        private int intEXTRA07_Data
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_EXTRA07.Text);
            }
            set
            {
                TextBox_EXTRA07.Text = SalaryPublic.strMoneyFormat(value.ToString());
            }
        }

        private int intEXTRA08_Data
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_EXTRA08.Text);
            }
            set
            {
                TextBox_EXTRA08.Text = SalaryPublic.strMoneyFormat(value.ToString());
            }
        }

        private int intEXTRA09_Data
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_EXTRA09.Text);
            }
            set
            {
                TextBox_EXTRA09.Text = SalaryPublic.strMoneyFormat(value.ToString());
            }
        }

        private int intMONTHPAY_TAX_Data
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_MONTHPAY_TAX.Text);
            }
            set
            {
                TextBox_MONTHPAY_TAX.Text = SalaryPublic.strMoneyFormat(value.ToString());
            }
        }

        private int intMONTHPAY_Data
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_MONTHPAY.Text);
            }
            set
            {
                TextBox_MONTHPAY.Text = SalaryPublic.strMoneyFormat(value.ToString());
            }
        }

        private string strSALARYPAY_NOTE_Data
        {
            get
            {
                return TextBox_SALARYPAY_NOTE.Text;
            }
            set
            {
                TextBox_SALARYPAY_NOTE.Text = value;
            }
        }

        private string strBANKID_Data
        {
            get
            {
                return DropDownList_BANK.SelectedValue;
            }
            set
            {
                if (value == "")
                    DropDownList_BANK.SelectedIndex = 0;
                else
                    DropDownList_BANK.SelectedValue = value;
            }
        }

        private string strSTOCKPILE_BANKID_Data
        {
            get
            {
                return TextBox_STOCKPILE_BANKID.Text;
            }
            set
            {
                TextBox_STOCKPILE_BANKID.Text = value;
            }
        }

        private static string _strGROUP_Data;
        private string strGROUP_Data
        {
            get
            {
                return DropDownList_GROUP.SelectedValue;
            }
            set
            {
                _strGROUP_Data = value;
                DropDownList_GROUP.SelectedValue = value;
            }
        }

        // 其他應發
        int otherAdd
        {
            set { txt_OtherAdd.Text = value.ToString(); }
            get
            {
                int i;
                int.TryParse(txt_OtherAdd.Text, out i);
                return i;
            }
        }

        // 其他應扣
        int otherMinus
        {
            set { txt_otherMinus.Text = value.ToString(); }
            get
            {
                int i;
                int.TryParse(txt_otherMinus.Text, out i);
                return i;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //檢查權限
                SalaryPublic.checkPermission();
            }
            DLBASESeardhPanel1.CodeSelectd += new EventHandler(DLBASESeardhPanel1_CodeSelectd);
            DLBASESeardhPanel1.SelectdPageChanged += new EventHandler(DLBASESeardhPanel1_SelectdPageChanged);
            DLBASESeardhPanel1.SearchClicked += new EventHandler(DLBASESeardhPanel1_SearchClicked);
            UserSelector1.UserSelectd += new GridViewCommandEventHandler(UserSelector1_UserSelectd);
            UserSelector1.PageIndexChanging += new GridViewPageEventHandler(UserSelector1_PageIndexChanging);
            PoliceSearchPanel1.postbacked += new TPPDDB._2_salary.UserControl.PoliceSearchPanel.Postbacked(PoliceSearchPanel1_postbacked);
            PoliceSearchPanel1.searchClicked += new TPPDDB._2_salary.UserControl.PoliceSearchPanel.SearchClicked(PoliceSearchPanel1_searchClicked);

            //剛進入頁面的時候
            if (!IsPostBack)
            {
                //20140321

                //跳窗區塊,點下 重新計算個人薪資 按鈕後,輸入 身分證號 等資訊
                //裡面的 發新機關欄位
                lb_RE_PAYAD.Text = Session["ADPPAY_AD"].ToString() + " " + o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE='" + Session["ADPPAY_AD"] + "' AND MZ_KTYPE='04' ");


                CustomInit();

                MultiView1.ActiveViewIndex = 0;
                if (Session["Session_ActiveViewIndex"] != null)
                {
                    MultiView1.ActiveViewIndex = int.Parse(Session["Session_ActiveViewIndex"].ToString());
                }

                btCreate.Enabled = true;
                btUpdate.Enabled = false;

                btExit.Enabled = false;
                btBack_Data.Enabled = false;
                btNext_Data.Enabled = false;

                DropDownList_TECHNICS.DataBind();
                DropDownList_BONUS.DataBind();
                DropDownList_WORKP.DataBind();

                voidLableText();
            }

            TextBox_MZ_BIR.Attributes.Add("onkeydown", "javascript:return false;");
            TextBox_MZ_PHONE.Attributes.Add("onkeydown", "javascript:return false;");
            TextBox_MZ_PHONO.Attributes.Add("onkeydown", "javascript:return false;");

            rbl_sex.Attributes.Add("onkeydown", "javascript:return false;");
            TextBox_HEALPER_Data.Attributes.Add("onkeydown", "javascript:return false;");
        }

        void PoliceSearchPanel1_searchClicked()
        {
            MakeDataList();
            ShowData();
        }

        void PoliceSearchPanel1_postbacked()
        {
            btn_showSearch_ModalPopupExtender.Show();
        }

        private void CustomInit()
        {
            SalaryPublic.FillHealthMode(ref ddl_HealthMode);
            SalaryPublic.FillHealthRelation(ref ddl_HealthRelation);
            SalaryPublic.fillMZ_EXTPOS(ref ddl_MZ_EXTPOS);
            SalaryPublic.fillMZ_NREA(ref ddl_MZ_NREA);
            SalaryPublic.FillHealthMode_noold(ref ddl_Healper_Insurance);

            txt_HealthCost.Enabled = false;
            healthChanged = false;
        }

        // 呈現資料
        private void ShowData()
        {
            if (ViewState["CurrentIndex"] == null)
                return;

            List<string> listData = (List<string>)ViewState["DataList"];
            int intCurrentIndex = (int)ViewState["CurrentIndex"];

            btNext_Data.Enabled = false;
            btBack_Data.Enabled = false;

            if (listData.Count == 0)
            {
                Label_Pages.Visible = false;
                txt_PageIndex.Visible = false;
                Label_PagesFooter.Visible = false;

                return;
            }

            // 避免輸入的索引大於資料總數
            if ((intCurrentIndex + 1) > listData.Count)
            {
                intCurrentIndex = listData.Count - 1;
                ViewState["CurrentIndex"] = intCurrentIndex;
            }

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
            txt_PageIndex.Visible = true;
            Label_PagesFooter.Visible = true;
            Label_Pages.Text = "第";
            txt_PageIndex.Text = (intCurrentIndex + 1).ToString();
            Label_PagesFooter.Text = "筆；共" + listData.Count.ToString() + "筆";

            finddata(intCurrentIndex);

            // A權限不受限
            if (GroupPermission == "A")
                return;
        }

        // 產生查詢的結果清單
        private void MakeDataList()
        {
            string strID;
            string strMZ_POLNO;
            string strMZ_NAME;
            string strPAY_AD;
            string strPAY_UNIT;

            strID = PoliceSearchPanel1.idcard; ;
            strMZ_POLNO = PoliceSearchPanel1.polno;
            strMZ_NAME = PoliceSearchPanel1.name;
            strPAY_AD = PoliceSearchPanel1.ad;
            strPAY_UNIT = PoliceSearchPanel1.unit;

            btNext_Data.Enabled = false;
            btBack_Data.Enabled = false;

            string strSQL = @"SELECT AKP.MZ_KCHI PAY_AD, MZ_ID IDCARD, MZ_POLNO, MZ_NAME NAME, AKO.MZ_KCHI CHIOCCC FROM A_DLBASE 
                                LEFT JOIN A_KTYPE AKP ON AKP.MZ_KCODE = A_DLBASE.PAY_AD AND AKP.MZ_KTYPE = '04' 
                                LEFT JOIN A_KTYPE AKO ON AKO.MZ_KCODE = A_DLBASE.MZ_OCCC AND AKO.MZ_KTYPE = '26' WHERE 1=1 ";

            // 用在匯出excel的
            //irk 增加欄位 發薪機關、現服務單位、員工編號、職稱、姓名、身份證號、薪俸職等、俸階、俸點、考績等次、戶籍地址、現住地址
            string sql = @"SELECT AKP.MZ_KCHI AD, AKU.MZ_KCHI UNIT, MZ_POLNO POLNO, AKO.MZ_KCHI OCCC, MZ_NAME NAME, MZ_ID ID,MZ_SRANK,AKSR.MZ_KCHI SRANK
                                  ,MZ_SLVC,AKS.MZ_KCHI SLVC,MZ_SPT, '' SRANKNEW, '' SLVCNEW, '' SPTNEW,GRADE,MZ_ADD1,MZ_ADD2
                            FROM A_DLBASE 
                                LEFT JOIN A_KTYPE AKP ON AKP.MZ_KCODE = A_DLBASE.PAY_AD AND AKP.MZ_KTYPE = '04'
                                LEFT JOIN A_KTYPE AKU ON AKU.MZ_KCODE = A_DLBASE.MZ_EXUNIT AND AKU.MZ_KTYPE = '25'
                                LEFT JOIN A_KTYPE AKR ON AKR.MZ_KCODE = A_DLBASE.MZ_SRANK AND AKR.MZ_KTYPE = '09'
                                LEFT JOIN A_KTYPE AKO ON AKO.MZ_KCODE = A_DLBASE.MZ_OCCC AND AKO.MZ_KTYPE = '26'
                                LEFT JOIN A_KTYPE AKS ON AKS.MZ_KCODE = A_DLBASE.MZ_SLVC AND AKS.MZ_KTYPE = '64' 
                                LEFT JOIN A_KTYPE AKSR ON AKSR.MZ_KCODE = A_DLBASE.MZ_SRANK AND AKSR.MZ_KTYPE = '09'
                                LEFT JOIN B_BASE ON A_DLBASE.MZ_ID=B_BASE.IDCARD
                            WHERE 1=1";


            if (strID != "")
            {
                strSQL = strSQL + " AND MZ_ID = '" + strID + "'";
                sql += " AND MZ_ID = '" + strID + "'";
            }
            if (strMZ_POLNO != "")
            {
                strSQL = strSQL + " AND MZ_POLNO = '" + strMZ_POLNO + "'";
                sql += " AND MZ_POLNO = '" + strMZ_POLNO + "'";
            }
            if (strMZ_NAME != "")
            {
                strSQL = strSQL + " AND MZ_NAME LIKE N'%" + strMZ_NAME + "%'";
                sql += " AND MZ_NAME LIKE N'%" + strMZ_NAME + "%'";
            }
            if (strPAY_AD != "")
            {
                strSQL = strSQL + " AND PAY_AD = '" + strPAY_AD + "'";
                sql += " AND PAY_AD = '" + strPAY_AD + "'";
            }
            if (strPAY_UNIT != "")// 小隊長需求：strPAY_UNIT做為員工編號前4碼的判斷 2012.04.11
            {
                strSQL = strSQL + " AND dbo.SUBSTR(MZ_POLNO, 1, 4) LIKE '" + strPAY_UNIT + "%'";
                sql += " AND dbo.SUBSTR(MZ_POLNO, 1, 4)  LIKE '" + strPAY_UNIT + "%'";
            }

            strSQL = strSQL + " ORDER BY MZ_POLNO";
            sql += " ORDER BY MZ_POLNO";

            searchSQL = sql;

            ViewState["DataList"] = o_DBFactory.ABC_toTest.DataListArray(strSQL, "IDCARD");
            UserSelector1.SetData(o_DBFactory.ABC_toTest.DataSelect(strSQL));

            if (((List<string>)ViewState["DataList"]).Count == 0)
            {
                lockControls();
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('查無資料！');", true);
            }

            ViewState["CurrentIndex"] = 0;
        }

        protected void finddata(int Datacount)
        {
            List<string> dataNext = (List<string>)ViewState["DataList"];
            this.police = new Police(dataNext[Datacount]);
            //搜尋員警資料,根據 this.police 的物件內容
            _finddata();
        }


        protected void finddata(string id)
        {
            this.police = new Police(id);

            //搜尋員警資料,根據 this.police 的物件內容
            _finddata();
        }

        /// <summary>
        /// 搜尋員警資料,根據 this.police 的物件內容
        /// </summary>
        protected void _finddata()
        {
            strMZ_ID_Data = police.id;
            strMZ_NAME_Data = police.name;
            strMZ_OCCC_Data = police.occc;
            strMZ_AD_Data = police.ad;
            strMZ_UNIT_Data = police.unit;
            strMZ_EXAD_Data = police.exad;
            strMZ_EXUNIT_Data = police.exunit;

            strMZ_POLNO_Data = police.polno;
            if (police.payad == "" || police.payad == "0")
                strPAY_AD_Data = "";
            else
                strPAY_AD_Data = police.payad;

            //3/21 irk增加 登入者現服機關若與發薪機關相同，則能進行修改
            //若無發薪機關也能進行修改。
            if (police.payad == Session["ADPMZ_EXAD"].ToString())
                DropDownList_PAY_AD.Enabled = true;
            else if (police.payad == "" || police.payad == "0" || DropDownList_PAY_AD.SelectedIndex == 0 || DropDownList_PAY_AD.SelectedIndex == -1)
                DropDownList_PAY_AD.Enabled = true;
            else
                DropDownList_PAY_AD.Enabled = false;

            strMZ_BIR = police.birth;
            strMZ_PHONE = police.phone;
            strMZ_PHONO = police.cellphone;
            strMZ_ADD2 = police.contactAddress;
            strMZ_ADD1 = police.address;
            strMZ_SEX = police.id.Substring(1, 1);
            srank = police.srank;
            slvc = police.slvc;
            strMZ_SPT = police.spt;
            tempSPT = police.tempSpt;
            strMZ_ADATE = police.adate;
            strMZ_LDATE = police.ldate;
            strMZ_TDATE = police.tdate;// 停職日
            strMZ_ODATE = police.odate;// 復職日
            strMZ_GRADE = police.grade;// 去年考績等級
            pchief = police.chief;// 主管級別
            strMZ_PCHIEFDATE = police.chiefDate;// 任主管日期
            isExtpos = police.isExtpos;// 是否兼代職
            try
            {
                ddl_MZ_EXTPOS.SelectedValue = police.extpos;// 兼代職級別
            }
            catch
            {

            }

            extposSrank = police.extposSrank;// 兼代職職等
            ddl_MZ_NREA.SelectedValue = police.nrea;// 任(退)職原因  
            strMZ_FDATE = police.fdate;// 初任公職日

            if (police.isCrimelab)
                isCrimlab = "是";
            else
                isCrimlab = "否";

            if (SalaryBasic.boolCHK_MZ_OCCC_Data_Serach(police.id))
            {
                voidOpen_Control();
            }
            else
            {
                voidLock_Control();
            }

            //設定 "重新計算個人薪資"視窗內的資訊,請參考id=btn_recalculate_ModalPopupExtender
            void_UpdateInfo_recalculate_ModalPopupExtender();

            //銀行帳號
            voidSTOCKPILE_SQL_Command();
            // 薪資資料
            voidView_Data();

            lockControls();


            //20140514
            TextBox_MZ_GRADE.Enabled = false;
            txt_pchief.Enabled = false;
            //btn_showPchief.Enabled = false;

            TextBox_SALARY_PAY1.Enabled = false;
            TextBox_BOSS.Enabled = false;

            //20140219
            txt_MZ_SRANK.Enabled = false;
            //btn_ShowMZ_SRANK.Enabled = false;
            txt_slvc.Enabled = false;
            //btn_showSlvc.Enabled = false;
            TextBox_MZ_SPT.Enabled = false;

            //20150327
            TextBox_PROFESS.Enabled = false;
            DropDownList_TECHNICS.Enabled = false;
        }

        /// <summary>
        /// 設定 "重新計算個人薪資"視窗內的資訊,請參考id=btn_recalculate_ModalPopupExtender
        /// </summary>
        private void void_UpdateInfo_recalculate_ModalPopupExtender()
        {
            //設定 "重新計算個人薪資"視窗內的資訊,請參考id=btn_recalculate_ModalPopupExtender
            //抓取 目前選取的 發新機關代碼
            string ADPPAY_AD = _Get_ADPPAY_AD();
            //更新 modal跳窗內的發薪機關資訊
            lb_RE_PAYAD.Text = ADPPAY_AD + " " + o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE='" + ADPPAY_AD + "' AND MZ_KTYPE='04' ");
            //將身分證欄位歸0
            txt_recalculate_ID.Text = string.Empty;
        }

        // 用薪資資料庫的資料刷新畫面數據
        private void voidView_Data()
        {
            //20140618
            //if (police.is30Years)
            //    is30year = "是";
            //else
            //    is30year = "否";

            if (police.isOffduty)
                isOffDuty = "是";
            else
                isOffDuty = "否";
            //
            intBOSS_Data = police.boss;
            intPROFESS_Data = police.profess;
            strINSURANCE_GROUP = police.insuranceType;
            intSALARY_PAY1 = police.salary;


            if (police.concur3 == 0)
            {
                strCONCUR3_Data = "2";
                TextBox_CONCUR3.Enabled = false;
                intCONCUR3_Txt = 0;
            }
            else
            {
                strCONCUR3_Data = "1";
                TextBox_CONCUR3.Enabled = true;
                intCONCUR3_Txt = police.concur3;
            }

            strTECHNICS_Data = police.technicCode;
            strBONUS_Data = police.bonusCode;
            strWORKP_Data = police.workp.ToString();

            DropDownList_ADVENTIVE.DataBind();
            strADVENTIVE_Data = police.adventiveCode;
            intFarPay = police.far;

            //DropDownList_ELECTRIC.DataBind();

            intELECTRIC_Data = police.electric;
            otherAdd = police.otheradd;// 其他應發
            strSALARYPAY_NOTE_Data = police.note;

            healthInfo = police.healthInfo;
            healthPersonal = police.healthPersonal;
            //20210115 百分比處理 Mark
            ddl_Healper_Insurance.SelectedValue = police.healthper.ToString();
            intHEALPER_Data = police.health;
            intSALARY_GOV = police.insurance;
            intTAXPER_Data = police.tax;
            if (intTAXPER_Data == 0)
            {
                strTAXPER_GROUP = "否";
                TextBox_TAXPER.Enabled = false;
            }
            else
            {
                strTAXPER_GROUP = "是";
                TextBox_TAXPER.Enabled = true;
            }

            intDE_HEALTH = police.deHealth;
            intMONTHPAY_TAX_Data = police.salaryCostTax;
            intMONTHPAY_Data = police.salaryCost;

            intTAXCHILD = police.taxChild;

            intEXTRA01_Data = police.extra01;
            if (intEXTRA01_Data == 0)
            {
                strEXTRA01_GROUP = "否";
                TextBox_EXTRA01.Enabled = false;
            }
            else
            {
                strEXTRA01_GROUP = "是";
                TextBox_EXTRA01.Enabled = true;
            }

            intEXTRA02_Data = police.extra02;
            intEXTRA03_Data = police.extra03;
            intEXTRA04_Data = police.extra04;
            intEXTRA05_Data = police.extra05;
            intEXTRA06_Data = police.extra06;
            intEXTRA07_Data = police.extra07;
            intEXTRA08_Data = police.extra08;
            intEXTRA09_Data = police.extra09;
            otherMinus = police.otherminus;

            if (police.policeType == "1")
            {
                voidOpen_Control();
            }
            else
            {
                voidLock_Control();
                strTECHNICS_Data = "";
                strWORKP_Data = "";
                strBONUS_Data = "";
                strADVENTIVE_Data = "";
                //20140721
                intELECTRIC_Data = police.electric;
                //strELECTRIC_Data = "";
            }
        }

        private void voidLableText()
        {
            strMZ_ID_Data = "";
            strMZ_NAME_Data = "";
            strMZ_AD_Data = "";
            strMZ_UNIT_Data = "";
            strPAY_AD_Data = "";
            strMZ_POLNO_Data = "";

            strMZ_BIR = "";
            strMZ_PHONE = "";
            strMZ_PHONO = "";
            strMZ_ADD2 = "";
            strMZ_ADD1 = "";
            strMZ_SEX = "1";
            srank = "";
            slvc = "";
            strMZ_SPT = "";
            strMZ_FDATE = "";
            strMZ_ADATE = "";
            strMZ_LDATE = "";
            strMZ_GRADE = "";

            intCONCUR3_Txt = 0;

            txt_PageIndex.Visible = false;
        }

        private void voidSALARYPAY_Clear()
        {
            strCONCUR3_Data = "1";
            TextBox_CONCUR3.Enabled = true;

            DropDownList_TECHNICS.Enabled = true;
            strTECHNICS_Data = "";
            strBONUS_Data = "";

            DropDownList_WORKP.Enabled = false;
            strWORKP_Data = "";
            strADVENTIVE_Data = "";
            intFarPay = 0;
            //20140721
            //strELECTRIC_Data = "";
            intELECTRIC_Data = 0;
            intDE_HEALTH = 0;
            intMONTHPAY_TAX_Data = 0;
            intMONTHPAY_Data = 0;
            strTAXPER_GROUP = "";
            TextBox_TAXPER.Enabled = false;
            intTAXCHILD = 0;
            intTAXPER_Data = 0;
            strEXTRA01_GROUP = "";
            TextBox_EXTRA01.Enabled = false;
            intEXTRA01_Data = 0;
            intEXTRA02_Data = 0;
            intEXTRA03_Data = 0;
            intEXTRA04_Data = 0;
            intEXTRA05_Data = 0;
            intEXTRA06_Data = 0;
            intEXTRA07_Data = 0;
            intEXTRA08_Data = 0;
            intEXTRA09_Data = 0;
            strSALARYPAY_NOTE_Data = "";

            if (SalaryBasic.boolCHK_MZ_OCCC_Data_Serach(strMZ_ID_Data))
            {
                voidOpen_Control();
            }
            else
            {
                voidLock_Control();
            }
        }

        private bool boolCHK_ID(string strID)
        {
            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn.Open();
                try
                {
                    string strSQL = "SELECT * FROM B_BASE WHERE IDCARD = '" + strID + "'";
                    DataTable dt = new DataTable();
                    SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
                    dt.Load(Selectcmd.ExecuteReader(CommandBehavior.CloseConnection));
                    if (dt.Rows.Count == 1)
                    {
                        return true;
                    }
                    else
                    { return false; }
                }
                catch { return false; }
                finally
                {
                    Selectconn.Close();
                    //XX2013/06/18 
                    Selectconn.Dispose();
                }
            }
        }

        private void voidTECHNICS_Selected_CHK()
        {
            if (DropDownList_TECHNICS.SelectedValue == "0")
            {
                DropDownList_WORKP.Enabled = false;
            }
            else
            {
                DropDownList_WORKP.Enabled = false;
                strWORKP_Data = "";
            }
            refreshHealthData();
        }

        private void voidWORKP_Selected_CHK()
        {
            if (DropDownList_WORKP.SelectedValue == "0")
            {
                DropDownList_TECHNICS.Enabled = false;
            }
            else
            {
                DropDownList_TECHNICS.Enabled = false;
                strTECHNICS_Data = "";

                //intELECTRIC_Data = Salary.getElectric(int.Parse(DropDownList_WORKP.SelectedItem.Text));

            }
            refreshHealthData();
        }

        private int voidHEALPER_STATISTICS(int intPROFESS_PAY_Data, int intTECHNICS_PAY_Data,
            int intBONUS_PAY_Data, int intWORKP_PAY_Data, int intADVENTIVE_PAY_Data,
            int intFAR_PAY_Data, int intELECTRIC_PAY_Data)
        {
            SalaryBasic.boolCHK_ORTHE_Increase_Data(strMZ_ID_Data);

            return intPROFESS_PAY_Data + intTECHNICS_PAY_Data +
                intBONUS_PAY_Data + intWORKP_PAY_Data + intADVENTIVE_PAY_Data +
                intFAR_PAY_Data + intELECTRIC_PAY_Data + SalaryBasic.intOrthe_Increase_Data;
        }



        private void voidLock_Control()
        {
            //鎖定部份應發、應扣，控制項
            TextBox_BOSS.Enabled = false;
            intBOSS_Data = 0;
            DropDownList_TECHNICS.Enabled = false;
            //strTECHNICS_Data = "";
            DropDownList_WORKP.Enabled = false;
            //strWORKP_Data = "";
            DropDownList_BONUS.Enabled = false;
            //strBONUS_Data = "";
            DropDownList_ADVENTIVE.Enabled = false;
            //strADVENTIVE_Data = "";
            //DropDownList_FAR.Enabled = false;
            //strFAR_Data = "";
            //20140721
            //DropDownList_ELECTRIC.Enabled = false;

            //strELECTRIC_Data = "";


        }

        private void voidOpen_Control()
        {
            //開啟部份應發、應扣，控制項
            TextBox_BOSS.Enabled = false;
            DropDownList_TECHNICS.Enabled = false;
            //strTECHNICS_Data = "";
            DropDownList_WORKP.Enabled = false;
            //strWORKP_Data = "";
            DropDownList_BONUS.Enabled = false;
            //strBONUS_Data = "";
            DropDownList_ADVENTIVE.Enabled = false;
            //strADVENTIVE_Data = "";
            //DropDownList_FAR.Enabled = true;
            //strFAR_Data = "";
            //20140721
            //DropDownList_ELECTRIC.Enabled = true;
            //strELECTRIC_Data = "";
        }

        protected void voidChanged(object sender, EventArgs e)
        {
            switch ((sender as TextBox).ID)
            {
                case "TextBox_PROFESS":
                    intPROFESS_Data = SalaryPublic.intdelimiterChars(TextBox_PROFESS.Text);
                    break;
                case "TextBox_TAXPER":
                    intTAXPER_Data = SalaryPublic.intdelimiterChars(TextBox_TAXPER.Text);
                    break;
                case "TextBox_DE_HEALTH":
                    intDE_HEALTH = int.Parse(TextBox_DE_HEALTH.Text);
                    //intDE_HEALTH = SalaryPublic.intdelimiterChars(TextBox_DE_HEALTH.Text);
                    break;
                case "TextBox_MONTHPAY_TAX":
                    intMONTHPAY_TAX_Data = SalaryPublic.intdelimiterChars(TextBox_MONTHPAY_TAX.Text);
                    break;
                case "TextBox_MONTHPAY":
                    intMONTHPAY_Data = SalaryPublic.intdelimiterChars(TextBox_MONTHPAY.Text);
                    break;
                case "TextBox_EXTRA01":
                    intEXTRA01_Data = SalaryPublic.intdelimiterChars(TextBox_EXTRA01.Text);
                    break;
                case "TextBox_EXTRA02":
                    intEXTRA02_Data = SalaryPublic.intdelimiterChars(TextBox_EXTRA02.Text);
                    break;
                case "TextBox_EXTRA03":
                    intEXTRA03_Data = SalaryPublic.intdelimiterChars(TextBox_EXTRA03.Text);
                    break;
                case "TextBox_EXTRA04":
                    intEXTRA04_Data = SalaryPublic.intdelimiterChars(TextBox_EXTRA04.Text);
                    break;
                case "TextBox_EXTRA05":
                    intEXTRA05_Data = SalaryPublic.intdelimiterChars(TextBox_EXTRA05.Text);
                    break;
                case "TextBox_EXTRA06":
                    intEXTRA06_Data = SalaryPublic.intdelimiterChars(TextBox_EXTRA06.Text);
                    break;
                case "TextBox_EXTRA07":
                    intEXTRA07_Data = SalaryPublic.intdelimiterChars(TextBox_EXTRA07.Text);
                    break;
                case "TextBox_EXTRA08":
                    intEXTRA08_Data = SalaryPublic.intdelimiterChars(TextBox_EXTRA08.Text);
                    break;
                case "TextBox_EXTRA09":
                    intEXTRA09_Data = SalaryPublic.intdelimiterChars(TextBox_EXTRA09.Text);
                    break;
            }
        }

        #region ButtonClick

        protected void btTable_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('B_SalaryBasic_Search.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "&reID=BASIC','查詢','top=80,left=20,width=400,height=350,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }

        protected void btBack_Data_Click(object sender, EventArgs e)
        {
            #region 原本的

            /*
            if (int.Parse(xcount.Text) != 0)
            {
                xcount.Text = (Convert.ToInt32(xcount.Text) - 1).ToString();
                finddata(int.Parse(xcount.Text));

                if (int.Parse(xcount.Text) != dataNext.Count - 1)
                {
                    btNext_Data.Enabled = true;
                }
                if (int.Parse(xcount.Text) == 0)
                {
                    btBack_Data.Enabled = false;
                }
            }
            else if (int.Parse(xcount.Text) == 0)
            {
                finddata(int.Parse(xcount.Text));
                btBack_Data.Enabled = false;
            }

            Session["Session_xcount_Data"] = int.Parse(xcount.Text);

            Label_Pages.Text = "第" + (int.Parse(xcount.Text) + 1).ToString() + "筆；共" + dataNext.Count.ToString() + "筆";

            dataNext = Session["datanext"] as List<string>;
            string strID = dataNext[int.Parse(xcount.Text)];
            if (boolCHK_ID(strID))
            {
                voidView_Data(strID);
            }
            else
            {
                voidSALARYPAY_Clear();
            }

            voidBASE_OTHER_SQL_Command(strID);
            voidSTOCKPILE_SQL_Command(strID);
            */

            #endregion

            int intCurrentIndex = (int)ViewState["CurrentIndex"];

            intCurrentIndex--;

            ViewState["CurrentIndex"] = intCurrentIndex;

            ShowData();
        }

        protected void btNext_Data_Click(object sender, EventArgs e)
        {
            #region 原本的

            /*
            if (int.Parse(xcount.Text) == 0)
            {
                xcount.Text = (Convert.ToInt32(xcount.Text) + 1).ToString();
                btBack_Data.Enabled = true;
                finddata(int.Parse(xcount.Text));

                if (int.Parse(xcount.Text) == dataNext.Count - 1)
                {
                    btNext_Data.Enabled = false;
                }
            }
            else
            {
                xcount.Text = (Convert.ToInt32(xcount.Text) + 1).ToString();
                finddata(int.Parse(xcount.Text));

                if (int.Parse(xcount.Text) == dataNext.Count - 1)
                {
                    btNext_Data.Enabled = false;
                }
            }

            Session["Session_xcount_Data"] = int.Parse(xcount.Text);

            Label_Pages.Text = "第" + (int.Parse(xcount.Text) + 1).ToString() + "筆；共" + dataNext.Count.ToString() + "筆";

            dataNext = Session["datanext"] as List<string>;
            string strID = dataNext[int.Parse(xcount.Text)];
            if (boolCHK_ID(strID))
            {
                voidView_Data(strID);
            }
            else
            {
                voidSALARYPAY_Clear();
            }

            voidBASE_OTHER_SQL_Command(strID);
            voidSTOCKPILE_SQL_Command(strID);
            
            */

            #endregion

            int intCurrentIndex = (int)ViewState["CurrentIndex"];

            intCurrentIndex++;

            ViewState["CurrentIndex"] = intCurrentIndex;

            ShowData();
        }

        protected void btCreate_Click(object sender, EventArgs e)
        {
            //如果沒有查詢員警的情況下按新增，建立員警資料
            if (police == null)
            {
                string error = "";
                if (strMZ_ID_Data.Trim().Length == 0)
                    error += "身份證號不可空白\\r\\n";
                if (!Police.isValidID(strMZ_ID_Data))
                    error += "身份證號格式不正確\\r\\n";
                if (strMZ_NAME_Data.Trim().Length == 0)
                    error += "姓名不可空白\\r\\n";
                if (strMZ_OCCC_Data.Trim().Length == 0)
                    error += "職稱不可空白\\r\\n";
                if (strMZ_AD_Data.Trim().Length == 0)
                    error += "編制機關不可空白\\r\\n";
                if (strMZ_UNIT_Data.Trim().Length == 0)
                    error += "編制單位不可空白\\r\\n";
                if (strMZ_EXAD_Data.Trim().Length == 0)
                    error += "現服機關不可空白\\r\\n";
                if (strMZ_EXUNIT_Data.Trim().Length == 0)
                    error += "現服單位不可空白\\r\\n";
                if (error.Length > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('" + error + "');", true);
                    return;
                }

                if (!validData())
                    return;

                Police police1 = new Police(strMZ_ID_Data);
                if (police1.id != null)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('此員警已有資料');", true);
                    return;
                }
                Police.createData(strMZ_ID_Data, strMZ_NAME_Data, strMZ_POLNO_Data, strMZ_ADD2, strMZ_AD_Data, strMZ_UNIT_Data, strMZ_EXAD_Data, strMZ_EXUNIT_Data, strPAY_AD_Data, strMZ_OCCC_Data, srank, slvc, strMZ_SPT, tempSPT, strMZ_FDATE, strMZ_ADATE, pchief, ddl_MZ_NREA.SelectedValue);

                //新增完成後直接進入查詢此員警的狀態
                string sql = string.Format("SELECT PAY_AD, MZ_UNIT UNIT, MZ_ID IDCARD, MZ_POLNO, MZ_NAME NAME FROM A_DLBASE WHERE 1=1 AND MZ_ID='{0}'", strMZ_ID_Data);
                ViewState["DataList"] = o_DBFactory.ABC_toTest.DataListArray(sql, "IDCARD");
                //UserSelector1.SetData(o_DBFactory.ABC_toTest.DataSelect(sql));
                ViewState["CurrentIndex"] = 0;
                ShowData();

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('員警新增完成');", true);
                return;
            }

            if (ViewState["CurrentIndex"] == null)
                return;

            List<string> dataNext = (List<string>)ViewState["DataList"];
            int intCurrentIndex = (int)ViewState["CurrentIndex"];
            string strID = dataNext[intCurrentIndex];

            if (MultiView1.Views[MultiView1.ActiveViewIndex].ID == "vw_Account")
            {
                //irk 4/18 增加 銀行帳號驗証
                if (!validDataAccount())
                    return;
                //銀行帳戶
                if (SalaryBasic.boolCHK_STOCKPILE_Data(strMZ_ID_Data, strGROUP_Data, SalaryPublic.strLoginEXAD))
                {
                    if (SalaryBasic.boolSalaryPay_Stockpile_Set_Create(strID, strBANKID_Data, strSTOCKPILE_BANKID_Data, int.Parse(strGROUP_Data), SalaryPublic.strLoginEXAD))
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('新增完成');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('新增失敗');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('該群組已有帳戶');", true);
                }
            }

            ShowData();
        }

        protected void btUpdate_Click(object sender, EventArgs e)
        {
            if (ViewState["CurrentIndex"] == null)
                return;

            List<string> dataNext = (List<string>)ViewState["DataList"];
            int intCurrentIndex = (int)ViewState["CurrentIndex"];
            string strID = dataNext[intCurrentIndex];

            if (!SalaryBasic.HasSalary(strID))
                SalaryBasic.CreateSalaryData(strID);

            if (!validData())
                return;

            // 100年11月17日加入，改用poice類別
            List<SqlParameter> ops = new List<SqlParameter>();
            List<SqlParameter> opsForSalary = new List<SqlParameter>();

            if (MultiView1.Views[MultiView1.ActiveViewIndex].ID == "vw_Account")//帳戶資料
            {
                if (Session["B_STOCKPILE_Keyword"] != null)
                {
                    if (SalaryBasic.boolSalaryPay_Stockpile_Set_Update(Session["B_STOCKPILE_Keyword"].ToString(), strBANKID_Data, strSTOCKPILE_BANKID_Data, int.Parse(strGROUP_Data), SalaryPublic.strLoginEXAD))
                    {
                        Session["B_STOCKPILE_Keyword"] = null;
                    }
                }

                voidSTOCKPILE_SQL_Command();
            }
            else
            {

                #region 人事資料

                if (DropDownList_PAY_AD.SelectedValue != this.police.payad)
                {
                    if (DropDownList_PAY_AD.SelectedIndex == 0)
                        ops.Add(new SqlParameter("PAY_AD", DBNull.Value));
                    else
                        ops.Add(new SqlParameter("PAY_AD", DropDownList_PAY_AD.SelectedValue));
                }

                if (TextBox_MZ_POLNO.Text != this.police.polno)
                {
                    if (!Police.avaliblePolno(strMZ_POLNO_Data.Trim(), SalaryPublic.strLoginEXAD))
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('員工編號已經使用中')", true);
                        return;
                    }
                    ops.Add(new SqlParameter("MZ_POLNO", TextBox_MZ_POLNO.Text));
                }

                if (TextBox_MZ_ADD2.Text != this.police.contactAddress)
                    ops.Add(new SqlParameter("MZ_ADD2", TextBox_MZ_ADD2.Text));

                if (TextBox_MZ_ADD1.Text != this.police.address)
                    ops.Add(new SqlParameter("MZ_ADD1", TextBox_MZ_ADD1.Text));

                if (srank != this.police.srank)
                {
                    ops.Add(new SqlParameter("MZ_SRANK", srank));

                    //needUpdateBoss = true;
                    //needUpdateHealth = true;
                    //opsForSalary.Add(new SqlParameter("PROFESSPAY", intPROFESS_Data));
                }

                if (slvc != this.police.slvc)
                    ops.Add(new SqlParameter("MZ_SLVC", slvc));

                if (TextBox_MZ_SPT.Text != this.police.spt)
                {
                    ops.Add(new SqlParameter("MZ_SPT", TextBox_MZ_SPT.Text));

                    //needUpdateHealth = true;
                    //opsForSalary.Add(new SqlParameter("SALARYPAY", intSALARY_PAY1));
                    //opsForSalary.Add(new SqlParameter("INSURANCEPAY", intSALARY_GOV));
                }
                //暫支薪點的修改
                if (tempSPT != this.police.tempSpt)
                {
                    ops.Add(new SqlParameter("MZ_SPT1", tempSPT));

                    //needUpdateHealth = true;
                    //opsForSalary.Add(new SqlParameter("SALARYPAY", intSALARY_PAY1));
                    //opsForSalary.Add(new SqlParameter("INSURANCEPAY", intSALARY_GOV));
                }
                if (TextBox_MZ_FDATE.Text != this.police.fdate)
                    ops.Add(new SqlParameter("MZ_FDATE", TextBox_MZ_FDATE.Text));

                if (TextBox_MZ_ADATE.Text != this.police.adate)
                    ops.Add(new SqlParameter("MZ_ADATE", TextBox_MZ_ADATE.Text));

                if (TextBox_MZ_LDATE.Text != this.police.ldate)
                    ops.Add(new SqlParameter("MZ_LDATE", TextBox_MZ_LDATE.Text));

                if (txt_MZ_TDATE.Text != this.police.tdate)
                    ops.Add(new SqlParameter("MZ_TDATE", txt_MZ_TDATE.Text));

                if (txt_MZ_ODATE.Text != this.police.odate)
                    ops.Add(new SqlParameter("MZ_ODATE", txt_MZ_ODATE.Text));

                if (pchief != this.police.chief)
                {
                    ops.Add(new SqlParameter("MZ_PCHIEF", pchief));

                    //needUpdateBoss = true;
                }

                if (ddl_MZ_EXTPOS.SelectedValue != this.police.extpos)
                    ops.Add(new SqlParameter("MZ_EXTPOS", ddl_MZ_EXTPOS.SelectedValue));

                if (extposSrank != this.police.extposSrank)
                {
                    ops.Add(new SqlParameter("MZ_EXTPOS_SRANK", extposSrank));

                    //needUpdateBoss = true;
                }

                if (ddl_MZ_NREA.SelectedValue != this.police.nrea)
                    ops.Add(new SqlParameter("MZ_NREA", ddl_MZ_NREA.SelectedValue));

                this.police.updateBasicData(ops, false);

                #endregion

                #region 薪資資料

                List<SqlParameter> opsSalary = new List<SqlParameter>();

                opsSalary.Add(new SqlParameter("GRADE", TextBox_MZ_GRADE.Text));

                //以下這些與薪資加幾有關的欄位都不應該更新,這不是本功能應該更新的欄位
                //只有 其他應發OtherAdd/薪資備註SALARYPAY_NOTE 可被更新
                //opsSalary.Add(new SqlParameter("SALARYPAY", intSALARY_PAY1));
                //opsSalary.Add(new SqlParameter("BOSSPAY", intBOSS_Data));
                //opsSalary.Add(new SqlParameter("PROFESSPAY", intPROFESS_Data));
                //opsSalary.Add(new SqlParameter("TECHNICS", strTECHNICS_Data));
                //opsSalary.Add(new SqlParameter("TECHNICSPAY", Salary.getPay(strTECHNICS_Data, "B_TECHNICS")));
                //opsSalary.Add(new SqlParameter("BONUS", strBONUS_Data));
                //opsSalary.Add(new SqlParameter("BONUSPAY", Salary.getPay(strBONUS_Data, "B_BONUS")));
                //opsSalary.Add(new SqlParameter("WORKP", strWORKP_Data));
                //opsSalary.Add(new SqlParameter("WORKPPAY", Salary.getPay(strWORKP_Data, "B_WORKP")));
                //opsSalary.Add(new SqlParameter("WORKPPAY", Salary.getWORKPPY(police).ToString()));
                //opsSalary.Add(new SqlParameter("ADVENTIVE", strADVENTIVE_Data));
                //opsSalary.Add(new SqlParameter("ADVENTIVEPAY", Salary.getPay(strADVENTIVE_Data, "B_ADVENTIVE")));
                //opsSalary.Add(new SqlParameter("FARPAY", intFarPay));
                //20140721
                //opsSalary.Add(new SqlParameter("ELECTRIC", strELECTRIC_Data));
                //opsSalary.Add(new SqlParameter("ELECTRICPAY", intELECTRIC_Data));

                opsSalary.Add(new SqlParameter("OTHERADD", otherAdd));
                opsSalary.Add(new SqlParameter("NOTE", strSALARYPAY_NOTE_Data));
                //opsSalary.Add(new SqlParameter("IS30YEAR", is30year));
                opsSalary.Add(new SqlParameter("IS30YEAR", "否"));
                opsSalary.Add(new SqlParameter("CONCUR3PAY", intCONCUR3_Txt));
                opsSalary.Add(new SqlParameter("PERSONALHEALTHPAY", healthPersonal));
                opsSalary.Add(new SqlParameter("HEALTHPAY", intHEALPER_Data));
                opsSalary.Add(new SqlParameter("INSURANCEPAY", intSALARY_GOV));
                opsSalary.Add(new SqlParameter("TAXPER", intTAXPER_Data));
                opsSalary.Add(new SqlParameter("TAXCHILD", intTAXCHILD));
                opsSalary.Add(new SqlParameter("DE_HEALTH", intDE_HEALTH));
                opsSalary.Add(new SqlParameter("MONTHPAY_TAX", intMONTHPAY_TAX_Data));
                opsSalary.Add(new SqlParameter("MONTHPAY", intMONTHPAY_Data));
                opsSalary.Add(new SqlParameter("EXTRA01", intEXTRA01_Data));
                opsSalary.Add(new SqlParameter("EXTRA02", intEXTRA02_Data));
                opsSalary.Add(new SqlParameter("EXTRA03", intEXTRA03_Data));
                opsSalary.Add(new SqlParameter("EXTRA04", intEXTRA04_Data));
                opsSalary.Add(new SqlParameter("EXTRA05", intEXTRA05_Data));
                opsSalary.Add(new SqlParameter("EXTRA06", intEXTRA06_Data));
                opsSalary.Add(new SqlParameter("EXTRA07", intEXTRA07_Data));
                opsSalary.Add(new SqlParameter("EXTRA08", intEXTRA08_Data));
                opsSalary.Add(new SqlParameter("EXTRA09", intEXTRA09_Data));
                opsSalary.Add(new SqlParameter("OTHERMINUS", otherMinus));
                opsSalary.Add(new SqlParameter("ISOFFDUTY", isOffDuty));
                opsSalary.Add(new SqlParameter("CRIMELAB", isCrimlab));
                //增加健保百分比作業 20210115 Mark處理
                opsSalary.Add(new SqlParameter("Healper_insurance", ddl_Healper_Insurance.SelectedValue));

                police.updateSalaryData(opsSalary);

                if (healthChanged)
                {
                    police.updateHealthInfo(healthInfo);
                    healthChanged = false;
                }

                #endregion
            }

            ShowData();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('修改完成');", true);
        }



        protected void btBasic_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 0;

            Session["Session_ActiveViewIndex"] = MultiView1.ActiveViewIndex;

            if (ViewState["DataList"] == null)
                return;
            List<string> listData = (List<string>)ViewState["DataList"];
            if (listData.Count == 0)
                return;

            lockControls();

        }

        protected void btMonneySet_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 1;

            Session["Session_ActiveViewIndex"] = MultiView1.ActiveViewIndex;

            if (ViewState["DataList"] == null)
                return;
            List<string> listData = (List<string>)ViewState["DataList"];
            if (listData.Count == 0)
                return;

            lockControls();
        }

        protected void btMonneySet_DE_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 2;

            Session["Session_ActiveViewIndex"] = MultiView1.ActiveViewIndex;

            if (ViewState["DataList"] == null)
                return;
            List<string> listData = (List<string>)ViewState["DataList"];
            if (listData.Count == 0)
                return;

            lockControls();
        }

        protected void btBankSet_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 3;

            Session["Session_ActiveViewIndex"] = MultiView1.ActiveViewIndex;

            if (ViewState["DataList"] == null)
                return;
            List<string> listData = (List<string>)ViewState["DataList"];
            if (listData.Count == 0)
                return;

            lockControls();
        }

        protected void btExit_Click(object sender, EventArgs e)
        {
            if (Session["B_STOCKPILE_Keyword"] != null)
                voidBASE_STOCKPILE_Clear();
        }

        // 把查詢結果匯出excel
        protected void btn_export_Click(object sender, EventArgs e)
        {
            if (searchSQL == null)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('請先查詢人員');", true);
                return;
            }

            DataTable searchResult = o_DBFactory.ABC_toTest.DataSelect(searchSQL);
            searchResult.Columns[0].ColumnName = "發薪機關";
            searchResult.Columns[1].ColumnName = "現服單位";
            searchResult.Columns[2].ColumnName = "員工編號";
            searchResult.Columns[3].ColumnName = "職稱";
            searchResult.Columns[4].ColumnName = "姓名";
            searchResult.Columns[5].ColumnName = "身份證號";
            searchResult.Columns[6].ColumnName = "原職等代碼";
            searchResult.Columns[7].ColumnName = "原薪俸職等";
            searchResult.Columns[8].ColumnName = "原俸階代碼";
            searchResult.Columns[9].ColumnName = "原俸階";
            searchResult.Columns[10].ColumnName = "原俸點";
            searchResult.Columns[11].ColumnName = "新職等代碼";
            searchResult.Columns[12].ColumnName = "新俸階代碼";
            searchResult.Columns[13].ColumnName = "新俸點";
            searchResult.Columns[14].ColumnName = "考績等次";
            searchResult.Columns[15].ColumnName = "戶籍地址";
            searchResult.Columns[16].ColumnName = "現居地址";
            App_Code.ToExcel.Dt2Excel(searchResult, "薪資基本資料");
        }

        //導到考績資料匯入頁面
        protected void btn_import_Click(object sender, EventArgs e)
        {
            Response.Redirect("11-PoliceImport.aspx?TPM_FION=3614");//2013/03/29 by 立廷 避免建立 DATAEVENT 時 TPM_FION為空值
        }

        #endregion

        #region DataBound

        protected void DropDownList_TECHNICS_DataBound(object sender, EventArgs e)
        {
            ListItem IDselect = new ListItem("無", "0");
            DropDownList_TECHNICS.Items.Insert(0, IDselect);
        }

        protected void DropDownList_BONUS_DataBound(object sender, EventArgs e)
        {
            ListItem IDselect = new ListItem("無", "0");
            DropDownList_BONUS.Items.Insert(0, IDselect);
        }

        protected void DropDownList_WORKP_DataBound(object sender, EventArgs e)
        {
            ListItem IDselect = new ListItem("無", "0");
            DropDownList_WORKP.Items.Insert(0, IDselect);
        }

        protected void DropDownList_ADVENTIVE_DataBound(object sender, EventArgs e)
        {
            ListItem IDselect = new ListItem("無", "0");
            DropDownList_ADVENTIVE.Items.Insert(0, IDselect);
        }

        //protected void DropDownList_ELECTRIC_DataBound(object sender, EventArgs e)
        //{
        //    ListItem IDselect = new ListItem("無", "0");
        //    DropDownList_ELECTRIC.Items.Insert(0, IDselect);
        //}

        #endregion

        // 手動輸入索引時的事件, 將指定索引的值載入
        protected void txt_PageIndex_TextChanged(object sender, EventArgs e)
        {
            int intCurrentIndex = int.Parse(txt_PageIndex.Text) - 1;

            ViewState["CurrentIndex"] = intCurrentIndex;

            ShowData();
        }

        //依發薪機關鎖定按鈕
        void lockControls()
        {
            btCreate.Enabled = false;
            btUpdate.Enabled = false;

            btExit.Enabled = false;

            //如果沒有查到員警，開放建立員警資料的功能
            if (police == null)
            {
                btCreate.Enabled = true;
                TextBox_MZ_ID.Enabled = true;
                TextBox_MZ_NAME.Enabled = true;
                TextBox_MZ_OCCC.Enabled = true;
                btn_occc.Enabled = true;
                TextBox_MZ_AD.Enabled = true;
                btn_ad.Enabled = true;
                TextBox_MZ_UNIT.Enabled = true;
                btn_unit.Enabled = true;
                TextBox_MZ_EXAD.Enabled = true;
                btn_exad.Enabled = true;
                TextBox_MZ_EXUNIT.Enabled = true;
                btn_exunit.Enabled = true;
                return;
            }

            //最上面的基本資料都不能修改
            TextBox_MZ_ID.Enabled = false;
            TextBox_MZ_NAME.Enabled = false;
            TextBox_MZ_OCCC.Enabled = false;
            btn_occc.Enabled = false;
            TextBox_MZ_AD.Enabled = false;
            btn_ad.Enabled = false;
            TextBox_MZ_UNIT.Enabled = false;
            btn_unit.Enabled = false;
            TextBox_MZ_EXAD.Enabled = false;
            btn_exad.Enabled = false;
            TextBox_MZ_EXUNIT.Enabled = false;
            btn_exunit.Enabled = false;

            //人員的發薪機關是出納的現服機關才開放修改
            if (police.payad == SalaryPublic.strLoginEXAD || police.payad == "")
            {
                //帳戶處理的頁面，預設只開放新增按鈕；選取帳戶資料後才開放修改按鈕
                if (MultiView1.Views[MultiView1.ActiveViewIndex].ID == "vw_Account")
                {
                    btCreate.Enabled = true;
                    return;
                }

                btUpdate.Enabled = true;
                return;
            }

            //4/18 irk 增加 不鎖定銀行帳號新增
            if (MultiView1.Views[MultiView1.ActiveViewIndex].ID == "vw_Account")
            {
                btCreate.Enabled = true;
                return;
            }

            // sam.hsu 20201215 應發
            if (MultiView1.Views[MultiView1.ActiveViewIndex].ID == "vw_Amount")
            {
                btUpdate.Enabled = true;
                return;
            }


            // sam.hsu 20201215 應扣
            if (MultiView1.Views[MultiView1.ActiveViewIndex].ID == "vw_Des")
            {
                btUpdate.Enabled = true;
                return;
            }


        }

        //驗證必填欄位
        bool validData()
        {
            string error = "";

            //if (strPAY_AD_Data == "")
            //    error += "發薪機關不可空白\\r\\n";
            if (strMZ_POLNO_Data.Trim().Length != 8)
                error += "員工編號必須8碼\\r\\n";
            if (srank.Trim().Length == 0)
                error += "薪俸職等不可空白\\r\\n";
            if (slvc.Trim().Length == 0)
                error += "俸階不可空白\\r\\n";
            if (strMZ_SPT.Trim().Length == 0)
                error += "俸點不可空白\\r\\n";
            if (!SalaryPublic.IsValidRepulicDate(strMZ_ADATE))
                error += "到職日期格式不正確！標準格式：0990101\\r\\n";
            if (!SalaryPublic.IsValidRepulicDate(strMZ_LDATE))
                error += "離職日期格式不正確！標準格式：0990101\\r\\n";
            if (!SalaryPublic.IsValidRepulicDate(strMZ_TDATE))
                error += "停職日期格式不正確！標準格式：0990101\\r\\n";
            if (!SalaryPublic.IsValidRepulicDate(strMZ_ODATE))
                error += "復職日期格式不正確！標準格式：0990101\\r\\n";
            if (!SalaryPublic.IsValidGrade(strMZ_GRADE))
                error += "考績等級格式不正確！標準格式：甲\\r\\n";
            //if (strMZ_ADATE.Length > 0 && strMZ_LDATE.Length > 0)取消限制2012/4/11 by小隊長
            //    error += "不可以同時輸入到職日與離職日！\\r\\n";

            if (error.Length > 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('" + error + "');", true);
                return false;
            }

            return true;
        }

        //4/18 Irk增加 銀行帳號驗証
        //驗證必填欄位
        bool validDataAccount()
        {
            string error = "";

            if (strSTOCKPILE_BANKID_Data.Trim().Length == 0)
                error += "銀行帳號不可空白\\r\\n";

            if (error.Length > 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('" + error + "');", true);
                return false;
            }

            return true;
        }

        #region 健保加保

        protected void btn_HealthInfo_Click(object sender, EventArgs e)
        {
            ddl_HealthMode_SelectedIndexChanged(sender, e);
            ShowHealthInfo();
            btn_HealthInfo_ModalPopupExtender.Show();
        }

        protected void btn_HealthAdd_Click(object sender, EventArgs e)
        {
            //string strSQL;
            int intCost;

            int.TryParse(txt_HealthCost.Text, out intCost);

            //strSQL = string.Format("INSERT INTO B_HEALTH_INFORMATION(SN, IDCARD, HEALTH_MODE_ID, COST, RELATION) VALUES( NEXT VALUE FOR dbo.B_HEALTH_INFO_SN, '{0}', '{1}', {2}, '{3}')", strMZ_ID_Data, ddl_HealthMode.SelectedValue, intCost, ddl_HealthRelation.SelectedValue);
            //o_DBFactory.ABC_toTest.Edit_Data(strSQL);

            //// 也要更新薪資基本資料的健保費和加保人數
            //intHEALPER_Data = SalaryBasic.CaculateHealthPay(strMZ_ID_Data, GetHealthID(), rbl_30Year.SelectedValue == "1" ? true : false);
            //intDE_HEALTH = GetHealthCount();
            //strSQL = string.Format("UPDATE B_BASE SET HEALTHPAY={0}, DE_HEALTH={2} WHERE IDCARD='{1}'", intHEALPER_Data, strMZ_ID_Data, intDE_HEALTH);
            //o_DBFactory.ABC_toTest.Edit_Data(strSQL);

            DataRow dr = healthInfo.NewRow();
            dr["SN"] = getMaxSN();
            dr["ModeID"] = ddl_HealthMode.SelectedValue;
            dr["Mode"] = Salary.getMode(ddl_HealthMode.SelectedValue);
            //先拿掉Mark 20210115
            //  dr["Cost"] = intCost;
            dr["Percent"] = txt_HealthPer.Text == "" ? "101" : txt_HealthPer.Text.Replace("%", "");// 百分比>100在Salary類別中才會判斷為老人
            dr["RelationID"] = ddl_HealthRelation.SelectedValue;
            dr["Relation"] = Salary.getRelation(ddl_HealthRelation.SelectedValue);
            healthInfo.Rows.Add(dr);

            refreshHealthData();
            ShowHealthInfo();
            btn_HealthInfo_ModalPopupExtender.Show();
        }

        private void ShowHealthInfo()
        {
            if (healthInfo == null) return;

            gv_HealthInfo.DataSource = healthInfo;
            gv_HealthInfo.DataBind();
        }

        private int CalculateCost(double cost)
        {
            int healthID;
            //20210115 Mark 每次以基本健保費產生
            string idno = TextBox_MZ_ID.Text;
            Police police = new Police(idno);
            decimal basehealth = Salary.getHealthPersonal(intSALARY_PAY1, intBOSS_Data, intPROFESS_Data, police.workp, Salary.getPay(strTECHNICS_Data, "B_TECHNICS"), Salary.getPay(strBONUS_Data, "B_BONUS"), Salary.getPay(strADVENTIVE_Data, "B_ADVENTIVE"), intFarPay, intELECTRIC_Data, RadioButtonList_INSURANCE_GROUP.SelectedValue);
            healthID = (int)basehealth;

            //健保費無條件捨去小數點
            return Convert.ToInt32(Math.Floor(healthID * cost));
        }

        private double GetHealthModeCost(string modeID)
        {
            string sql;
            double cost;

            sql = string.Format("SELECT COST FROM B_HEALTH_MODE WHERE ID='{0}'", modeID);

            double.TryParse(o_DBFactory.ABC_toTest.vExecSQL(sql), out cost);

            return cost;
        }

        protected void gv_HealthInfo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //string strSQL;
            //int sn;

            //sn = int.Parse(e.CommandArgument.ToString());

            //strSQL = string.Format("DELETE B_HEALTH_INFORMATION WHERE SN={0}", sn);
            //o_DBFactory.ABC_toTest.Edit_Data(strSQL);

            //// 也要更新薪資基本資料的健保費
            //intHEALPER_Data = SalaryBasic.CaculateHealthPay(strMZ_ID_Data, GetHealthID(), rbl_30Year.SelectedValue == "1" ? true : false);
            //intDE_HEALTH = GetHealthCount();
            //strSQL = string.Format("UPDATE B_BASE SET HEALTHPAY={0}, DE_HEALTH={2} WHERE IDCARD='{1}'", intHEALPER_Data, strMZ_ID_Data, intDE_HEALTH);
            //o_DBFactory.ABC_toTest.Edit_Data(strSQL);

            healthInfo.Rows.Remove(healthInfo.Select("SN=" + e.CommandArgument).First());
            refreshHealthData();
            ShowHealthInfo();
            btn_HealthInfo_ModalPopupExtender.Show();
        }

        protected void ddl_HealthMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 代碼05 老人狀態加保, 由出納自行輸入保費金額
            if (ddl_HealthMode.SelectedValue == "05")
            {
                txt_HealthCost.Enabled = true;
                //txt_HealthCost.Text = (CalculateCost(GetHealthModeCost("01") / 100) - 749 < 0 ? 0 : CalculateCost(GetHealthModeCost("01") / 100) - 749).ToString();

                int HealthPersonalstr = 0;

                if (txt_HealthPersonal.Text != "")
                {
                    HealthPersonalstr = int.Parse(txt_HealthPersonal.Text);
                }

                //20210233 個人健保費 減 826
                int coststr = HealthPersonalstr - 826;

                txt_HealthCost.Text = coststr.ToString();

                //直接帶出金額
                //txt_HealthCost.Text = "826";
                txt_HealthPer.Text = "100%";
            }
            else
            {
                txt_HealthCost.Enabled = false;
                txt_HealthCost.Text = CalculateCost(GetHealthModeCost(ddl_HealthMode.SelectedValue) / 100).ToString();
                txt_HealthPer.Text = GetHealthModeCost(ddl_HealthMode.SelectedValue).ToString() + "%";
            }

            btn_HealthInfo_ModalPopupExtender.Show();
        }

        protected void ddl_Healper_Insurance_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 取得百分比
            // intHEALPER_Data;
            //目前眷屬+自己保費
            string all_health = TextBox_HEALPER.Text;
            //自己的保費
            string now_self = txt_HealthPersonal.Text;
            //眷屬的保費
            string all_health_noself = (decimal.Parse(all_health) - decimal.Parse(now_self)).ToString();
            string COST = ddl_Healper_Insurance.SelectedValue;
            string idno = TextBox_MZ_ID.Text;
            Police police = new Police(idno);
            //計算出基本保費
            decimal basehealth = Salary.getHealthPersonal(intSALARY_PAY1, intBOSS_Data, intPROFESS_Data, police.workp, Salary.getPay(strTECHNICS_Data, "B_TECHNICS"), Salary.getPay(strBONUS_Data, "B_BONUS"), Salary.getPay(strADVENTIVE_Data, "B_ADVENTIVE"), intFarPay, intELECTRIC_Data, RadioButtonList_INSURANCE_GROUP.SelectedValue);
            //區分計算方法  
            // A: 一般/清殘/半殘/全殘:依照傷殘程度計算減免比例
            // B: 老人: 直接扣除金額
            //這邊不考慮員警本人是老人又傷殘的問題,屬於極少數個案
            string mode = "A";
            if (ddl_Healper_Insurance.SelectedItem.Text == "老人")
            {
                mode = "B";
            }
            string f_health = "0";
            switch (mode)
            {
                case "A":
                    //有殘疾減免的保費 , 基本保費 * 減免打折%數 
                    f_health = Math.Floor(basehealth * decimal.Parse(COST) / 100).ToString();
                    break;
                case "B":
                    //老人, 直接扣減免金額
                    f_health = Math.Floor(basehealth - decimal.Parse(COST)).ToString();
                    break;
            }

            txt_HealthPersonal.Text = f_health;
            TextBox_HEALPER.Text = (decimal.Parse(all_health_noself) + decimal.Parse(f_health)).ToString();


            //因為殘疾狀態變動 所以 重新計算公/勞保費
            intSALARY_GOV = Salary.getInsurancePay_withPerno(Police.getPoliceType(strMZ_OCCC_Data), Police.checkIsPolice(srank), TextBox_MZ_SPT.Text, tempSPT, decimal.Parse(COST),police.INSURANCE_TYPE);

            //TextBox_HEALPER.Text = (decimal.Parse(all_health) - (basehealth - decimal.Parse(f_health))).ToString();
            // bperno = perno;
        }

        int getMaxSN()
        {
            int max = 0;

            foreach (DataRow dr in healthInfo.Rows)
            {
                if (int.Parse(dr["SN"].ToString()) > max)
                    max = int.Parse(dr["SN"].ToString());
            }

            return max + 1;
        }

        #endregion

        #region 銀行帳戶

        private void voidBASE_STOCKPILE_Clear()
        {
            strBANKID_Data = "001";
            strSTOCKPILE_BANKID_Data = "";
            DropDownList_GROUP.DataBind();
            lockControls();
        }

        private void voidSTOCKPILE_SQL_Command()
        {
            GridView_STOCKPILE.DataSource = police.accountInfo;
            GridView_STOCKPILE.DataBind();
        }

        protected void GridView_STOCKPILE_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ((GridView)sender).PageIndex = e.NewPageIndex;
            voidSTOCKPILE_SQL_Command();
        }

        protected void GridView_STOCKPILE_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
                return;

            string payAD = SalaryPublic.strLoginEXAD;

            if (((GridView)sender).DataKeys[e.Row.RowIndex].Values["PAY_AD"].ToString() != payAD)
            {
                ((Button)e.Row.Cells[0].FindControl("btnSelect")).Enabled = false;
                ((Button)e.Row.Cells[5].FindControl("btn_DeleteAccount")).Enabled = false;
            }
        }

        protected void GridView_STOCKPILE_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn.Open();

                if (e.CommandName == "btSelect")
                {
                    btUpdate.Enabled = true;


                    //int index = Convert.ToInt32(e.CommandArgument);
                    string strKeyword = e.CommandArgument.ToString();

                    Session["B_STOCKPILE_Keyword"] = strKeyword;

                    string strSQL = "SELECT BS_SNID, IDCARD, BANKID, STOCKPILE_BANKID, \"GROUP\" FROM B_BASE_STOCKPILE WHERE BS_SNID = '" + strKeyword + "' ORDER BY \"GROUP\"";
                    SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);

                    Selectcmd.ExecuteNonQuery();

                    SqlDataReader dr = Selectcmd.ExecuteReader(CommandBehavior.SingleRow);

                    if (dr.Read())
                    {
                        strBANKID_Data = dr["BANKID"].ToString();
                        strSTOCKPILE_BANKID_Data = dr["STOCKPILE_BANKID"].ToString();
                        strGROUP_Data = dr["GROUP"].ToString();

                        btCreate.Enabled = false;
                        btExit.Enabled = true;
                    }
                }
                else
                {
                    string strKeyword = e.CommandArgument.ToString();

                    if (SalaryBasic.boolSalaryPay_Stockpile_Set_Delete(strKeyword))
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('刪除完成');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('刪除失敗');", true);
                    }
                    voidSTOCKPILE_SQL_Command();
                    voidBASE_STOCKPILE_Clear();
                }
                Selectconn.Close();
                //XX2013/06/18 
                Selectconn.Dispose();
            }
        }

        #endregion

        #region 資料變動，計算薪資資料





        protected void TextBox_MZ_SPT_TextChanged(object sender, EventArgs e)
        {
            TextBox_MZ_SPT.Text = TextBox_MZ_SPT.Text.PadLeft(4, '0');

            if (isOffDuty == "是")
            {
                intSALARY_PAY1 = Salary.getSalaryPay(Police.getPoliceType(strMZ_OCCC_Data), Police.checkIsPolice(srank), TextBox_MZ_SPT.Text, tempSPT);
                intSALARY_PAY1 = (int)Salary.round((double)intSALARY_PAY1 / 2);
            }
            else
            {
                intSALARY_PAY1 = Salary.getSalaryPay(Police.getPoliceType(strMZ_OCCC_Data), Police.checkIsPolice(srank), TextBox_MZ_SPT.Text, tempSPT);
                //殘疾狀態
                string perno = ddl_Healper_Insurance.SelectedValue;
                //計算公/勞保費
                string idno = TextBox_MZ_ID.Text;
                Police police = new Police(idno);
                intSALARY_GOV = Salary.getInsurancePay_withPerno(Police.getPoliceType(strMZ_OCCC_Data), Police.checkIsPolice(srank), TextBox_MZ_SPT.Text, tempSPT, decimal.Parse(perno), police.INSURANCE_TYPE);
                //intSALARY_GOV = Salary.getInsurancePay(Police.getPoliceType(strMZ_OCCC_Data), Police.checkIsPolice(srank), TextBox_MZ_SPT.Text, rbl_30Year.SelectedValue == "1" ? true : false);
                if (intCONCUR3_Txt != 0)
                    intCONCUR3_Txt = Salary.getConcur3(Police.getPoliceType(strMZ_OCCC_Data), Police.checkIsPolice(srank), TextBox_MZ_SPT.Text, tempSPT);
            }
            refreshHealthData();
        }



        protected void txt_extposSrank_TextChanged(object sender, EventArgs e)
        {
            extposSrank = txt_extposSrank.Text;
            if (isOffDuty == "是")
            {
                intBOSS_Data = 0;
            }
            else
            {
                intBOSS_Data = Salary.getBossPay(extposSrank, police.chief, srank);
            }
            refreshHealthData();
        }

        protected void rbl_IsCrimelab_SelectedIndexChanged1(object sender, EventArgs e)
        {
            if (isCrimlab == "是")
            {
                strWORKP_Data = "0";

                switch (srank)
                {
                    case "G21":// 警正一階
                    case "P09":
                        strTECHNICS_Data = "01";
                        break;

                    case "G22":// 警正二階
                    case "P08":
                        strTECHNICS_Data = "02";
                        break;

                    case "G23":// 警正三階
                        strTECHNICS_Data = "03";
                        break;

                    case "G24":// 警正四階
                        strTECHNICS_Data = "04";
                        break;
                }



            }
            else
            {

                strTECHNICS_Data = "0";
            }

            intPROFESS_Data = Salary.getProfessPay(srank, isCrimlab == "是" ? true : false, police != null ? police.AHP_RANK : "");

        }

        protected void txt_MZ_LDATE_TextChanged(object sender, EventArgs e)
        {
            //輸入離職日要把發薪機關清空
            if (TextBox_MZ_LDATE.Text.Length > 0)
                strPAY_AD_Data = "";
        }

        protected void DropDownList_TECHNICS_SelectedIndexChanged(object sender, EventArgs e)
        {
            voidTECHNICS_Selected_CHK();
        }

        protected void DropDownList_WORKP_SelectedIndexChanged(object sender, EventArgs e)
        {
            voidWORKP_Selected_CHK();
        }

        protected void RadioButtonList_TAXPER_GROUP_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (strTAXPER_GROUP == "是")
            {
                if (isOffDuty == "是")
                    intTAXPER_Data = 0;
                TextBox_TAXPER.Enabled = true;
            }
            else
            {
                TextBox_TAXPER.Enabled = false;
                intTAXPER_Data = 0;
            }
        }

        protected void payChanged(object sendet, EventArgs e)
        { refreshHealthData(); }

        // 更新健保加保狀態DataTable、加保人數、健保費
        void refreshHealthData()
        {
            string idno = TextBox_MZ_ID.Text;
            Police police = new Police(idno);
            decimal basehealth = Salary.getHealthPersonal(intSALARY_PAY1, intBOSS_Data, intPROFESS_Data, police.workp, Salary.getPay(strTECHNICS_Data, "B_TECHNICS"), Salary.getPay(strBONUS_Data, "B_BONUS"), Salary.getPay(strADVENTIVE_Data, "B_ADVENTIVE"), intFarPay, intELECTRIC_Data, RadioButtonList_INSURANCE_GROUP.SelectedValue);


            if (isOffDuty == "是")
            {
                healthPersonal = 0;
                // healthInfo = Salary.getHealthInfo(healthInfo, healthPersonal);
                healthInfo = Salary.getHealthInfo(healthInfo, (int)basehealth);
                // healthInfo = Salary.getHealthInfo(healthInfo, int.Parse(txt_HealthCost.ToString()));
                intHEALPER_Data = 0;
            }
            else
            {
                //20140721
                //healthPersonal = Salary.getHealthPersonal(intSALARY_PAY1, intBOSS_Data, intPROFESS_Data, Salary.getPay(strWORKP_Data, "B_WORKP"), Salary.getPay(strTECHNICS_Data, "B_TECHNICS"), Salary.getPay(strBONUS_Data, "B_BONUS"), Salary.getPay(strADVENTIVE_Data, "B_ADVENTIVE"), intFarPay, Salary.getPay(strELECTRIC_Data, "B_ELECTRIC"), RadioButtonList_INSURANCE_GROUP.SelectedValue);
                //healthPersonal = Salary.getHealthPersonal(intSALARY_PAY1, intBOSS_Data, intPROFESS_Data, Salary.getPay(strWORKP_Data, "B_WORKP"), Salary.getPay(strTECHNICS_Data, "B_TECHNICS"), Salary.getPay(strBONUS_Data, "B_BONUS"), Salary.getPay(strADVENTIVE_Data, "B_ADVENTIVE"), intFarPay, intELECTRIC_Data, RadioButtonList_INSURANCE_GROUP.SelectedValue);

                // healthInfo = Salary.getHealthInfo(healthInfo, healthPersonal);
                healthInfo = Salary.getHealthInfo(healthInfo, (int)basehealth);
                //healthInfo = Salary.getHealthInfo(healthInfo, int.Parse(txt_HealthCost.ToString()));
                //20140618
                //  intHEALPER_Data = Salary.getHealth(healthInfo, healthPersonal);
                //20210115整個健保費
                intHEALPER_Data = Salary.getHealth(healthInfo, healthPersonal);
                //intHEALPER_Data = Salary.getHealth(healthInfo, healthPersonal, is30year == "是" ? true : false);
            }
            if (healthInfo != null)
                intDE_HEALTH = healthInfo.Rows.Count;

            healthChanged = true;
            ShowHealthInfo();
        }

        #endregion

        #region 薪資RadioButton選擇後，計算薪資資料

        // 有沒有法院扣款
        protected void RadioButtonList_EXTRA01_GROUP_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (strEXTRA01_GROUP == "是")
            {
                TextBox_EXTRA01.Enabled = true;
                //intEXTRA01_Data = Salary.getExtra01(intSALARY_PAY1 + intBOSS_Data + intPROFESS_Data + Salary.getPay(strTECHNICS_Data, "B_TECHNICS") + Salary.getPay(strBONUS_Data, "B_BONUS") + Salary.getPay(strWORKP_Data, "B_WORKP") + intFarPay + intELECTRIC_Data);
                intEXTRA01_Data = Salary.getExtra01(intSALARY_PAY1 + intBOSS_Data + intPROFESS_Data + intTECHNICS_Data + intBONUS_Data + intWORKPPAY_Data + intFarPay + intELECTRIC_Data);
            }
            else
            {
                TextBox_EXTRA01.Enabled = false;
                intEXTRA01_Data = 0;
            }
        }

        // 是否停職
        protected void rbl_IsOffDuty_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (police == null)
                return;

            // 停職領半薪
            if (police.isOffduty)
            {
                if (isOffDuty == "是")
                {
                    intSALARY_PAY1 = police.salary;
                    //intSALARY_PAY1 = (int)Salary.round((double)SalaryBasic.GetSalaryPaybyIDCard(police.id) / 2);
                    intBOSS_Data = 0;
                    intPROFESS_Data = 0;
                    strTECHNICS_Data = "0";
                    strWORKP_Data = "0";
                    strBONUS_Data = "0";
                    strADVENTIVE_Data = "0";
                    intFarPay = 0;

                    //20140721
                    intELECTRIC_Data = 0;
                    //strELECTRIC_Data = "0";

                    //irk 4/20 停職 應扣全不扣
                    intSALARY_GOV = 0;
                    //////intHEALPER_Data = 0;
                    //strCONCUR3_Data = string.Empty;
                    //////intCONCUR3_Txt = 0;
                    //////intTAXPER_Data = 0;
                    intMONTHPAY_TAX_Data = 0;
                    intMONTHPAY_Data = 0;
                    //strEXTRA01_GROUP = "";
                    intEXTRA01_Data = 0;
                    intEXTRA02_Data = 0;
                    intEXTRA03_Data = 0;
                    intEXTRA04_Data = 0;
                    intEXTRA05_Data = 0;
                    intEXTRA06_Data = 0;
                    intEXTRA07_Data = 0;
                    intEXTRA08_Data = 0;
                    intEXTRA09_Data = 0;

                    //20140618
                    //intSALARY_GOV = Salary.getInsurancePay(police.policeType, police.isPolice, police.spt, police.is30Years);
                    intSALARY_GOV = police.getInsurancePay();
                    intCONCUR3_Txt = Salary.getConcur3(police.policeType, police.isPolice_G, police.spt, police.tempSpt);
                    intHEALPER_Data = Salary.getHealth(police.healthInfo, police.healthPersonal);
                    //intHEALPER_Data = Salary.getHealth(police.healthInfo, police.healthPersonal, police.is30Years);


                }
                else
                {
                    intSALARY_PAY1 = Salary.getSalaryPay(police.policeType, police.isPolice_G, police.spt, police.tempSpt);

                    intBOSS_Data = Salary.getBossPay(police.extposSrank, police.chief, police.srank);
                    intPROFESS_Data = Salary.getProfessPay(police.srank, police.isCrimelab, police.AHP_RANK);
                    //20140618
                    //intSALARY_GOV = Salary.getInsurancePay(police.policeType, police.isPolice, police.spt, police.is30Years);
                    intSALARY_GOV = police.getInsurancePay();
                    intCONCUR3_Txt = Salary.getConcur3(police.policeType, police.isPolice_G, police.spt, police.tempSpt);
                    //計算個人健保費
                    //this.healthPersonal = Salary.getHealthPersonal(police.salary, police.boss, police.profess, police.workp, police.technic, police.bonus, police.adventive, police.far, police.electric, police.insuranceType);
                    //用新的個人健保費更新加保人保費
                    //this.healthInfo = Salary.getHealthInfo(police.healthInfo, police.healthPersonal);
                    //將更新後的加保人保費存進資料庫
                    //updateHealthInfo(police.healthInfo);
                    //取得新的健保費總額
                    //20140618
                    //intHEALPER_Data = Salary.getHealth(police.healthInfo, police.healthPersonal, police.is30Years);
                    intHEALPER_Data = Salary.getHealth(police.healthInfo, police.healthPersonal);




                }
            }
            else
            {
                if (isOffDuty == "是")
                {
                    intSALARY_PAY1 = (int)Salary.round((double)SalaryBasic.GetSalaryPaybyIDCard(police.id) / 2);
                    intBOSS_Data = 0;
                    intPROFESS_Data = 0;
                    strTECHNICS_Data = "0";
                    strWORKP_Data = "0";
                    strBONUS_Data = "0";
                    strADVENTIVE_Data = "0";
                    intFarPay = 0;
                    //20140721
                    intELECTRIC_Data = 0;
                    //strELECTRIC_Data = "0";

                    //irk 4/20 停職 應扣全不扣
                    //////intSALARY_GOV = 0;
                    //////intHEALPER_Data = 0;
                    //strCONCUR3_Data = string.Empty;
                    //////intCONCUR3_Txt = 0;
                    intTAXPER_Data = 0;
                    intMONTHPAY_TAX_Data = 0;
                    intMONTHPAY_Data = 0;
                    //strEXTRA01_GROUP = "";
                    intEXTRA01_Data = 0;
                    intEXTRA02_Data = 0;
                    intEXTRA03_Data = 0;
                    intEXTRA04_Data = 0;
                    intEXTRA05_Data = 0;
                    intEXTRA06_Data = 0;
                    intEXTRA07_Data = 0;
                    intEXTRA08_Data = 0;
                    intEXTRA09_Data = 0;

                    //intSALARY_GOV = Salary.getInsurancePay(police.policeType, police.isPolice, police.spt, police.is30Years);
                    intSALARY_GOV = police.getInsurancePay();
                    intCONCUR3_Txt = Salary.getConcur3(police.policeType, police.isPolice_G, police.spt, police.tempSpt);
                    intHEALPER_Data = Salary.getHealth(police.healthInfo, police.healthPersonal);
                    //intHEALPER_Data = Salary.getHealth(police.healthInfo, police.healthPersonal, police.is30Years);

                }
                else
                {
                    intSALARY_PAY1 = police.salary;
                    intBOSS_Data = police.boss;
                    intPROFESS_Data = police.profess;
                    strTECHNICS_Data = police.technicCode;
                    strWORKP_Data = police.workpCode;
                    strBONUS_Data = police.bonusCode;
                    strADVENTIVE_Data = police.adventiveCode;
                    intFarPay = police.far;
                    //20140721
                    //strELECTRIC_Data = police.electricCode;
                    intELECTRIC_Data = police.electric;
                    //irk 4/20 停職 應扣恢復
                    intSALARY_GOV = police.insurance;
                    intHEALPER_Data = police.health;
                    //strCONCUR3_Data = string.Empty;
                    intCONCUR3_Txt = police.concur3;
                    intTAXPER_Data = police.tax;
                    intMONTHPAY_TAX_Data = police.salaryCostTax;
                    intMONTHPAY_Data = police.salaryCost;
                    //strEXTRA01_GROUP = "";
                    intEXTRA01_Data = police.extra01;
                    intEXTRA02_Data = police.extra02;
                    intEXTRA03_Data = police.extra03;
                    intEXTRA04_Data = police.extra04;
                    intEXTRA05_Data = police.extra05;
                    intEXTRA06_Data = police.extra06;
                    intEXTRA07_Data = police.extra07;
                    intEXTRA08_Data = police.extra08;
                    intEXTRA09_Data = police.extra09;
                }
            }

        }

        //// 是否任公職滿30年
        //protected void rbl_30Year_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (police == null)
        //        return;

        //    intHEALPER_Data = Salary.getHealth(healthInfo, healthPersonal, is30year == "是" ? true : false);
        //    intSALARY_GOV = Salary.getInsurancePay(police.policeType, police.isPolice, strMZ_SPT, is30year == "是" ? true : false);
        //}

        // 是否是鑑識人員
        protected void rbl_IsCrimelab_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (police == null)
                return;

            // 鑑識人員有技術加給、專業加給；沒有警勤加給
            if (rbl_IsCrimelab.SelectedValue == "1")
            {
                strWORKP_Data = "";
            }
            else
                strWORKP_Data = police.workpCode;
        }

        // 是否扣退撫金費
        protected void RadioButtonList_CONCUR3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (police == null)
                return;

            if (RadioButtonList_CONCUR3.SelectedValue == "1")
            {
                intCONCUR3_Txt = Salary.getConcur3(police.policeType, police.isPolice_G, police.spt, police.tempSpt);
                TextBox_CONCUR3.Enabled = true;
            }
            else
            {
                intCONCUR3_Txt = 0;
                TextBox_CONCUR3.Enabled = false;
            }
        }

        #endregion

        #region 各個呈現代碼選單的事件

        protected void btn_occc_Click(object sender, EventArgs e)
        {
            codeType = "occc";
            DLBASESeardhPanel1.ktype = "26";
            DLBASESeardhPanel1.condition = "";
            DLBASESeardhPanel1.Search();
            btn_ShowModalPopupExtender.Show();
        }

        protected void btn_ad_Click(object sender, EventArgs e)
        {
            codeType = "ad";
            DLBASESeardhPanel1.ktype = "04";
            DLBASESeardhPanel1.condition = " AND MZ_KCODE LIKE '38213%'";
            DLBASESeardhPanel1.Search();
            btn_ShowModalPopupExtender.Show();
        }

        protected void btn_unit_Click(object sender, EventArgs e)
        {
            codeType = "unit";
            DLBASESeardhPanel1.ktype = "25";
            DLBASESeardhPanel1.condition = "";
            DLBASESeardhPanel1.Search();
            btn_ShowModalPopupExtender.Show();
        }

        protected void btn_exad_Click(object sender, EventArgs e)
        {
            codeType = "exad";
            DLBASESeardhPanel1.ktype = "04";
            DLBASESeardhPanel1.condition = " AND MZ_KCODE LIKE '38213%'";
            DLBASESeardhPanel1.Search();
            btn_ShowModalPopupExtender.Show();
        }

        protected void btn_exunit_Click(object sender, EventArgs e)
        {
            codeType = "exunit";
            DLBASESeardhPanel1.ktype = "25";
            DLBASESeardhPanel1.condition = "";
            DLBASESeardhPanel1.Search();
            btn_ShowModalPopupExtender.Show();
        }



        protected void btn_showExtposSrank_Click(object sender, EventArgs e)
        {
            codeType = "extposSrank";
            DLBASESeardhPanel1.ktype = "09";
            DLBASESeardhPanel1.condition = " AND (MZ_KCODE LIKE 'B%' OR MZ_KCODE LIKE 'P%' OR MZ_KCODE LIKE 'G%')";
            DLBASESeardhPanel1.Search();
            btn_ShowModalPopupExtender.Show();
        }

        protected void btn_ShowNREA_Click(object sender, EventArgs e)
        {
            codeType = "nrea";
            DLBASESeardhPanel1.ktype = "11";
            DLBASESeardhPanel1.condition = "";
            DLBASESeardhPanel1.Search();
            btn_ShowModalPopupExtender.Show();
        }

        void DLBASESeardhPanel1_CodeSelectd(object sender, EventArgs e)
        {
            switch (codeType)
            {
                case "occc":
                    strMZ_OCCC_Data = DLBASESeardhPanel1.selectedCode;
                    break;

                case "ad":
                    strMZ_AD_Data = DLBASESeardhPanel1.selectedCode;
                    break;

                case "unit":
                    strMZ_UNIT_Data = DLBASESeardhPanel1.selectedCode;
                    break;

                case "exad":
                    strMZ_EXAD_Data = DLBASESeardhPanel1.selectedCode;
                    break; ;

                case "exunit":
                    strMZ_EXUNIT_Data = DLBASESeardhPanel1.selectedCode;
                    break;



                case "extposSrank":
                    extposSrank = DLBASESeardhPanel1.selectedCode;
                    txt_extposSrank_TextChanged(sender, e);
                    break;

                case "nrea":
                    ddl_MZ_NREA.SelectedValue = DLBASESeardhPanel1.selectedCode;
                    break;
            }
        }

        void DLBASESeardhPanel1_SelectdPageChanged(object sender, EventArgs e)
        {
            btn_ShowModalPopupExtender.Show();
        }

        void DLBASESeardhPanel1_SearchClicked(object sender, EventArgs e)
        {
            btn_ShowModalPopupExtender.Show();
        }

        #endregion

        protected void txt_occc_TextChanged(object sender, EventArgs e)
        {
            strMZ_OCCC_Data = TextBox_MZ_OCCC.Text;
        }

        protected void txt_ad_TextChanged(object sender, EventArgs e)
        {
            strMZ_AD_Data = TextBox_MZ_AD.Text;
        }

        protected void txt_unit_TextChanged(object sender, EventArgs e)
        {
            strMZ_UNIT_Data = TextBox_MZ_UNIT.Text;
        }

        protected void txt_exad_TextChanged(object sender, EventArgs e)
        {
            strMZ_EXAD_Data = TextBox_MZ_EXAD.Text;
        }

        protected void txt_exunit_TextChanged(object sender, EventArgs e)
        {
            strMZ_EXUNIT_Data = TextBox_MZ_EXUNIT.Text;
        }

        #region 快速選單

        protected void btn_fastMenu_Click(object sender, EventArgs e)
        {
            btn_showFastMenu_ModalPopupExtender.Show();
        }

        void UserSelector1_UserSelectd(object sender, GridViewCommandEventArgs e)
        {
            finddata(e.CommandArgument.ToString());
        }

        void UserSelector1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            btn_showFastMenu_ModalPopupExtender.Show();
        }

        #endregion

        #region 沒用到的

        private int GetHealthCount()
        {
            string sql;
            int count;

            sql = string.Format("SELECT COUNT(*) FROM B_HEALTH_INFORMATION WHERE IDCARD='{0}'", strMZ_ID_Data);

            int.TryParse(o_DBFactory.ABC_toTest.vExecSQL(sql), out count);

            return count;
        }

        #endregion

        #region 這個事件並沒有實際被觸發

        ///// <summary>
        ///// (這個事件並沒有實際被觸發) 事件 按下按鈕 重新計算個人薪資
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void btn_recalculate_Click(object sender, EventArgs e)
        //{
        //    //抓取 目前選取的 發新機關代碼
        //    string ADPPAY_AD = _Get_ADPPAY_AD();
        //    //更新 modal跳窗內的發薪機關資訊
        //    lb_RE_PAYAD.Text = ADPPAY_AD + " " + o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE='" + ADPPAY_AD + "' AND MZ_KTYPE='04' ");

        //    txt_recalculate_ID.Text = string.Empty;

        //    //打開跳窗區塊,點下 重新計算個人薪資 按鈕後,輸入 身分證號 等資訊
        //    btn_recalculate_ModalPopupExtender.Show();
        //} 
        #endregion

        /// <summary>
        /// 抓取 發新機關的代碼
        /// 1. 如果 下拉選單"發薪機關" 有選擇,則以下拉選單為主
        /// 2. 如果沒有 改以使用者自己所屬的發薪機關為主
        /// </summary>
        /// <returns></returns>
        public string _Get_ADPPAY_AD() {
            //抓取 目前選取的 發新機關代碼
            string ADPPAY_AD = strPAY_AD_Data;
            //如果沒選擇,才用 Session的
            if (string.IsNullOrEmpty(ADPPAY_AD))
            {
                ADPPAY_AD = Session["ADPPAY_AD"].ToString();
            }
            return ADPPAY_AD;
        }

        /// <summary>
        /// 事件 重新計算薪資的按鈕 (真正在算的)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_recalculate_start_Click(object sender, EventArgs e)
        {
            string ADPPAY_AD = _Get_ADPPAY_AD();
            //因為少年隊人比較少,拿來測試用
            //string strSQL = "SELECT MZ_ID FROM A_DLBASE  WHERE PAY_AD IS NOT NULL AND PAY_AD='382135000C' ";


            string strSQL = "SELECT MZ_ID FROM A_DLBASE  WHERE MZ_OCCC   not in ('Z014','Z015','Z016')  AND PAY_AD='" + ADPPAY_AD + "' ";

            if (!string.IsNullOrEmpty(txt_recalculate_ID.Text))
                strSQL += " AND MZ_ID='" + txt_recalculate_ID.Text + "'";


            DataTable ALL_MZ_ID = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");

            if (ALL_MZ_ID.Rows.Count > 0)
            {
                for (int i = 0; i < ALL_MZ_ID.Rows.Count; i++)
                {

                    Police tempPolice = new Police(ALL_MZ_ID.Rows[i]["MZ_ID"].ToString());

                    tempPolice.updateSalary();


                }


                if (!string.IsNullOrEmpty(txt_recalculate_ID.Text))
                {
                    finddata(txt_recalculate_ID.Text);
                }
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('計算完成');", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('查無此人');", true);
            }


        }

        protected void txt_ELECTRIC_TextChanged(object sender, EventArgs e)
        {
            //更動法院扣款
            if (strEXTRA01_GROUP == "是")
            {
                TextBox_EXTRA01.Enabled = true;
                //intEXTRA01_Data = Salary.getExtra01(intSALARY_PAY1 + intBOSS_Data + intPROFESS_Data + Salary.getPay(strTECHNICS_Data, "B_TECHNICS") + Salary.getPay(strBONUS_Data, "B_BONUS") + Salary.getPay(strWORKP_Data, "B_WORKP") + intFarPay + intELECTRIC_Data);
                intEXTRA01_Data = Salary.getExtra01(intSALARY_PAY1 + intBOSS_Data + intPROFESS_Data + intTECHNICS_Data + intBONUS_Data + intWORKPPAY_Data + intFarPay + intELECTRIC_Data);
            }
            else
            {
                TextBox_EXTRA01.Enabled = false;
                intEXTRA01_Data = 0;
            }

            //更動法院扣款更動健保費
            refreshHealthData();
        }
    }
}
