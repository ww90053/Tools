using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Configuration;
using System.Data.SqlClient;

namespace TPPDDB._2_salary
{
    public partial class B_SalaryMonth3 : System.Web.UI.Page
    {
        public List<string> dataNext;
        List<int> searchList
        {
            get { return (List<int>)ViewState["searchList"]; }
            set { ViewState["searchList"] = value; }
        }
        int currentIndex
        {
            set { ViewState["currentIndex"] = value; }
            get { return (int)ViewState["currentIndex"]; }
        }
        Monthpay monthpay
        {
            get { return (Monthpay)ViewState["monthpay"]; }
            set { ViewState["monthpay"] = value; }
        }

        #region 變數

        #region 基本資料

        /// <summary>
        /// 發薪機關
        /// </summary>
        string strPAY_AD  
        {
            get { return lbl_payad.Text; }
            set { lbl_payad.Text = value; }
        }

        /// <summary>
        /// 年月
        /// </summary>
        private string strAMONTH  
        {
            get
            {
                return lb_amonth.Text;
            }
            set
            {
                lb_amonth.Text = value;
            }
        }

        /// <summary>
        /// 身分證號
        /// </summary>
        private string strIDCARD  
        {
            get
            {
                return lbl_idcard.Text;
            }
            set
            {
                lbl_idcard.Text = value;
            }
        }

        /// <summary>
        /// 姓名
        /// </summary>
        private string strMZ_NAME  
        {
            get
            {
                return lbl_name.Text;
            }
            set
            {
                lbl_name.Text = value;
            }
        }

        /// <summary>
        /// 員工編號
        /// </summary>        
        private string strMZ_POLNO  
        {
            get
            {
                return lbl_polno.Text;
            }
            set
            {
                lbl_polno.Text = value;
            }
        }

        /// <summary>
        /// 單位
        /// </summary> 
        private string strMZ_UNIT  
        {
            get
            {
                return lbl_unit.Text;
            }
            set
            {
                lbl_unit.Text = value;
            }
        }

        /// <summary>
        /// 職稱
        /// </summary> 
        private string strMZ_OCCC
        {
            get
            {
                return lbl_occc.Text;
            }
            set
            {
                lbl_occc.Text = value;
            }
        }


        /// <summary>
        /// 薪俸職等
        /// </summary> 
        private string strMZ_SRANK
        {
            get
            {
                return lbl_srank.Text;
            }
            set
            {
                lbl_srank.Text = value;
            }
        }

        /// <summary>
        /// 俸階
        /// </summary>
        private string strMZ_SLVC
        {
            get
            {
                return lbl_slvc.Text;
            }
            set
            {
                lbl_slvc.Text = value;
            }
        }

        /// <summary>
        /// 俸點
        /// </summary>
        private string strMZ_SPT
        {
            get
            {
                return lbl_spt.Text;
            }
            set
            {
                lbl_spt.Text = value;
            }
        }

        /// <summary>
        /// 應發總計
        /// </summary>
        private int intINCREASE
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_INCREASE.Text);
            }
            set
            {
                TextBox_INCREASE.Text = SalaryPublic.strMoneyFormat(value.ToString());
                TextBox_INCREASE.Enabled = false;
            }
        }

        /// <summary>
        /// 應扣總計
        /// </summary>
        private int intDECREASE
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_DECREASE.Text);
            }
            set
            {
                TextBox_DECREASE.Text = SalaryPublic.strMoneyFormat(value.ToString());
                TextBox_DECREASE.Enabled = false;
            }
        }

        /// <summary>
        /// 實發金額
        /// </summary>
        private int intTPAY
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_TPAY.Text);
            }
            set
            {
                TextBox_TPAY.Text = SalaryPublic.strMoneyFormat(value.ToString());
                TextBox_TPAY.Enabled = false;
            }
        }

        /// <summary>
        /// 備考
        /// </summary>
        private string strNOTE
        {
            get
            {
                return TextBox_NOTE.Text;
            }
            set
            {
                TextBox_NOTE.Text = value;
            }
        }

        private static string _strLOCKDB;
        private string strLOCKDB
        {
            get
            {
                //return SalaryMonth_View.strSearch_SalaryMonth_Data_LockDB(strAMONTH, strIDCARD);
                return _strLOCKDB;
            }
            set
            {
                _strLOCKDB = value;
            }
        }

#endregion


        #region 以下應發金額
        

        /// <summary>
        /// 月支數額
        /// </summary>
        private int intSALARYPAY1
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_SALARYPAY1.Text);
            }
            set
            {
                TextBox_SALARYPAY1.Text = SalaryPublic.strMoneyFormat(value.ToString());
            }
        }

        /// <summary>
        /// 主管加給
        /// </summary>
        private int intBOSS
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

        /// <summary>
        /// 專業加給
        /// </summary>
        private int intPROFESS
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

        /// <summary>
        /// 警勤加給-DDL
        /// </summary>
        private string strWORKP_Dr
        {
            get
            {
                return DropDownList_WORKP.SelectedValue;
            }
            set
            {
                if (value == "")
                {
                    DropDownList_WORKP.DataBind();
                    DropDownList_WORKP.SelectedIndex = 0;
                    return;
                }
                DropDownList_WORKP.DataBind();
                DropDownList_WORKP.SelectedValue = value;
            }
        }

        /// <summary>
        /// 警勤加給-txt
        /// </summary>
        private int intWORKP
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_WORKP.Text);
            }
            set
            {
                TextBox_WORKP.Text = SalaryPublic.strMoneyFormat(value.ToString());
            }
        }

        /// <summary>
        /// 技術加給-DDL
        /// </summary>
        private string strTECHNICS_Dr
        {
            get
            {
                return DropDownList_TECHNICS.SelectedValue;
            }
            set
            {
                if (value == "")
                {
                    DropDownList_TECHNICS.DataBind();
                    DropDownList_TECHNICS.SelectedIndex = 0;
                    return;
                }
                DropDownList_TECHNICS.DataBind();
                DropDownList_TECHNICS.SelectedValue = value;
            }
        }

        /// <summary>
        /// 技術加給-txt
        /// </summary>
        private int intTECHNICS
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_TECHNICS.Text);
            }
            set
            {
                TextBox_TECHNICS.Text = SalaryPublic.strMoneyFormat(value.ToString());
            }
        }

        /// <summary>
        /// 工作獎助金-DDL
        /// </summary>
        private string strBONUS_Dr
        {
            get
            {
                return DropDownList_BONUS.SelectedValue;
            }
            set
            {
                if (value == "")
                {
                    DropDownList_BONUS.DataBind();
                    DropDownList_BONUS.SelectedIndex = 0;
                    return;
                }
                DropDownList_BONUS.DataBind();
                DropDownList_BONUS.SelectedValue = value;
            }
        }

        /// <summary>
        /// 工作獎助金-txt
        /// </summary>
        private int intBONUS
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_BONUS.Text);
            }
            set
            {
                TextBox_BONUS.Text = SalaryPublic.strMoneyFormat(value.ToString());
            }
        }

        /// <summary>
        /// 外事加給-DDL
        /// </summary>
        private string strADVENTIVE_Dr
        {
            get
            {
                return DropDownList_ADVENTIVE.SelectedValue;
            }
            set
            {
                if (value == "")
                {
                    DropDownList_ADVENTIVE.DataBind();
                    DropDownList_ADVENTIVE.SelectedIndex = 0;
                    return;
                }
                DropDownList_ADVENTIVE.DataBind();
                DropDownList_ADVENTIVE.SelectedValue = value;
            }
        }

        /// <summary>
        /// 外事加給-txt
        /// </summary>
        private int intADVENTIVE
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_ADVENTIVE.Text);
            }
            set
            {
                TextBox_ADVENTIVE.Text = SalaryPublic.strMoneyFormat(value.ToString());
            }
        }

        /// <summary>
        /// 偏遠加給
        /// </summary>
        private int intFAR
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_FAR.Text);
            }
            set
            {
                TextBox_FAR.Text = SalaryPublic.strMoneyFormat(value.ToString());
            }
        }

        /// <summary>
        /// 水電補助-DDL
        /// </summary>
        private string strELECTRIC_Dr
        {
            get
            {
                return DropDownList_ELECTRIC.SelectedValue;
            }
            set
            {
                if (value == "")
                {
                    DropDownList_ELECTRIC.DataBind();
                    DropDownList_ELECTRIC.SelectedIndex = 0;
                    return;
                }
                DropDownList_ELECTRIC.DataBind();
                DropDownList_ELECTRIC.SelectedValue = value;
            }
        }

        /// <summary>
        /// 水電補助-txt
        /// </summary>
        private int intELECTRIC
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_ELECTRIC.Text);
            }
            set
            {
                TextBox_ELECTRIC.Text = SalaryPublic.strMoneyFormat(value.ToString());
            }
        }

        private int intOtheradd
        {
            get
            {
                return SalaryPublic.intdelimiterChars(txt_otheradd.Text);
            }
            set
            {
                txt_otheradd.Text = value.ToString();
            }
        }

#endregion

        
        #region 以下應扣金額


        /// <summary>
        /// 健保年功俸
        /// </summary>
        private int intHEALTHID
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_HEALTHID.Text);
            }
            set
            {
                TextBox_HEALTHID.Text = SalaryPublic.strMoneyFormat(value.ToString());
            }
        }

        /// <summary>
        /// 健保眷口數
        /// </summary>
        private int intHEALTHMAN
        {
            get
            {
               
                return SalaryPublic.intMoneyDatatoInt(TextBox_HEALTHMAN.Text);
            }
            set
            {
                TextBox_HEALTHMAN.Text = value.ToString();
            }
        }

        /// <summary>
        /// 健保費
        /// </summary>
        private int intHEALTHPAY
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_HEALTHPAY.Text);
            }
            set
            {
                TextBox_HEALTHPAY.Text = SalaryPublic.strMoneyFormat(value.ToString());
            }
        }

        /// <summary>
        /// 健保費補差扣款
        /// </summary>
        private int intHEALTHPAY1
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_HEALTHPAY1.Text);
            }
            set
            {
                TextBox_HEALTHPAY1.Text = SalaryPublic.strMoneyFormat(value.ToString());
            }
        }

        /// <summary>
        /// 薪資扣款1
        /// </summary>
        private int intMONTHPAY_TAX
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

        /// <summary>
        /// 薪資扣款2
        /// </summary>
        private int intMONTHPAY
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

       

        /// <summary>
        /// 薪津所得稅
        /// </summary>
        private int intTAX
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_TAX.Text);
            }
            set
            {
                TextBox_TAX.Text = SalaryPublic.strMoneyFormat(value.ToString());
            }
        }
        
 
        /// <summary>
        /// 公(勞)保費
        /// </summary>
        private int intINSURANCEPAY
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_INSURANCEPAY.Text);
            }
            set
            {
                TextBox_INSURANCEPAY.Text = SalaryPublic.strMoneyFormat(value.ToString());
            }
        }

        /// <summary>
        /// 退輔金費
        /// </summary>
        private int intCONCUR3PAY
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_CONCUR3PAY.Text);
            }
            set
            {
                TextBox_CONCUR3PAY.Text = SalaryPublic.strMoneyFormat(value.ToString());
            }
        }



        /// <summary>
        /// 其他應扣
        /// </summary>
        private int intOtherminus
        {
            get
            {
                return SalaryPublic.intdelimiterChars(txt_otherminus.Text);
            }
            set
            {
                txt_otherminus.Text = value.ToString();
            }
        }

        #endregion

        //private string strS_SNID_De
        //{
        //    get
        //    {
        //        return DropDownList_S_SNID_De.SelectedValue;
        //    }
        //    set
        //    {
        //        DropDownList_S_SNID_De.DataBind();
        //    }
        //}


        #region 以下其他應扣金額

        /// <summary>
        /// 法院扣款
        /// </summary>
        private int intEXTRA01
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

        /// <summary>
        /// 國宅扣款
        /// </summary>
        private int intEXTRA02
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

        /// <summary>
        /// 銀行貸款
        /// </summary>
        private int intEXTRA03
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

        /// <summary>
        /// 分期付款
        /// </summary>
        private int intEXTRA04
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

        /// <summary>
        /// 優惠存款
        /// </summary>
        private int intEXTRA05
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

        /// <summary>
        /// 員工宿舍費
        /// </summary>
        private int intEXTRA06
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

        /// <summary>
        /// 福利互助金費
        /// </summary>
        private int intEXTRA07
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

        /// <summary>
        /// 伙食費
        /// </summary>
        private int intEXTRA08
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

        /// <summary>
        /// 退輔金貸款
        /// </summary>
        private int intEXTRA09
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
        #endregion

        #endregion

        private string strstrUpdate_Delete// 當前狀態(修改/刪除)
        {
            get
            {
                return ViewState["mode"].ToString();
            }
            set
            {
                ViewState["mode"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
           
            
            if (IsPostBack != true)
            { SalaryPublic.checkPermission();
                //自動將上月的資料 Lock
                //SalaryMonth.boolLockDB_Update();

                searchList = new List<int>();
                btUpdate.Enabled = false;
                btDelete.Enabled = false;
                btExit.Enabled = false;
                SalaryPublic.fillDropDownList(ref ddl_searchPayad);
                
                MultiView1.ActiveViewIndex = 0;
                voidClear();
            }
        }

        protected void finddata(int Datacount)
        {
            dataNext = Session["datanext"] as List<string>;

            string strSQL2 = "SELECT * FROM B_MONTHPAY_MAIN WHERE MM_SNID = '" + dataNext[Datacount] + "' ";
            DataTable dt = new DataTable();
            dt = o_DBFactory.ABC_toTest.Create_Table(strSQL2, "MM_SNID");

            monthpay = new Monthpay(int.Parse(dataNext[Datacount]));

            if (dt.Rows.Count == 1)
            {
                lb_amonth.Text = dt.Rows[0]["AMONTH"].ToString().Trim();
                //基本資料
                strIDCARD = dt.Rows[0]["IDCARD"].ToString().Trim();
                strMZ_NAME = dt.Rows[0]["MZ_NAME"].ToString().Trim();
                strMZ_POLNO = dt.Rows[0]["MZ_POLNO"].ToString().Trim();
                strPAY_AD = dt.Rows[0]["PAY_AD"].ToString().Trim();
                strMZ_UNIT = dt.Rows[0]["MZ_UNIT"].ToString().Trim();
                strMZ_OCCC = dt.Rows[0]["MZ_OCCC"].ToString().Trim();
                strMZ_SRANK = dt.Rows[0]["MZ_SRANK"].ToString().Trim();
                strMZ_SLVC = dt.Rows[0]["MZ_SLVC"].ToString().Trim();
                strMZ_SPT = dt.Rows[0]["MZ_SPT"].ToString().Trim();
                strNOTE = dt.Rows[0]["NOTE"].ToString().Trim();
                strLOCKDB = dt.Rows[0]["LOCKDB"].ToString().Trim();

                //應發
                intSALARYPAY1 = SalaryPublic.intMoneyDatatoInt(dt.Rows[0]["SALARYPAY1"].ToString().Trim());
                intBOSS = SalaryPublic.intMoneyDatatoInt(dt.Rows[0]["BOSS"].ToString().Trim());
                intPROFESS = SalaryPublic.intMoneyDatatoInt(dt.Rows[0]["PROFESS"].ToString().Trim());
                intWORKP = SalaryPublic.intMoneyDatatoInt(dt.Rows[0]["WORKP"].ToString().Trim());
                intTECHNICS = SalaryPublic.intMoneyDatatoInt(dt.Rows[0]["TECHNICS"].ToString().Trim());
                intBONUS = SalaryPublic.intMoneyDatatoInt(dt.Rows[0]["BONUS"].ToString().Trim());
                intADVENTIVE = SalaryPublic.intMoneyDatatoInt(dt.Rows[0]["ADVENTIVE"].ToString().Trim());
                intFAR = SalaryPublic.intMoneyDatatoInt(dt.Rows[0]["FAR"].ToString().Trim());
                intELECTRIC = SalaryPublic.intMoneyDatatoInt(dt.Rows[0]["ELECTRIC"].ToString().Trim());
                //應扣
                intHEALTHID = SalaryPublic.intMoneyDatatoInt(dt.Rows[0]["HEALTHID"].ToString().Trim());
                intHEALTHMAN = SalaryPublic.intMoneyDatatoInt(dt.Rows[0]["HEALTHMAN"].ToString().Trim());
                intHEALTHPAY = SalaryPublic.intMoneyDatatoInt(dt.Rows[0]["HEALTHPAY"].ToString().Trim());
                intHEALTHPAY1 = SalaryPublic.intMoneyDatatoInt(dt.Rows[0]["HEALTHPAY1"].ToString().Trim());
                intMONTHPAY_TAX = SalaryPublic.intMoneyDatatoInt(dt.Rows[0]["MONTHPAY_TAX"].ToString().Trim());
                intMONTHPAY = SalaryPublic.intMoneyDatatoInt(dt.Rows[0]["MONTHPAY"].ToString().Trim());
                intTAX = SalaryPublic.intMoneyDatatoInt(dt.Rows[0]["TAX"].ToString().Trim());
                intINSURANCEPAY = SalaryPublic.intMoneyDatatoInt(dt.Rows[0]["INSURANCEPAY"].ToString().Trim());
                intCONCUR3PAY = SalaryPublic.intMoneyDatatoInt(dt.Rows[0]["CONCUR3PAY"].ToString().Trim());
                //其他應扣
                intEXTRA01 = SalaryPublic.intMoneyDatatoInt(dt.Rows[0]["EXTRA01"].ToString().Trim());
                intEXTRA02 = SalaryPublic.intMoneyDatatoInt(dt.Rows[0]["EXTRA02"].ToString().Trim());
                intEXTRA03 = SalaryPublic.intMoneyDatatoInt(dt.Rows[0]["EXTRA03"].ToString().Trim());
                intEXTRA04 = SalaryPublic.intMoneyDatatoInt(dt.Rows[0]["EXTRA04"].ToString().Trim());
                intEXTRA05 = SalaryPublic.intMoneyDatatoInt(dt.Rows[0]["EXTRA05"].ToString().Trim());
                intEXTRA06 = SalaryPublic.intMoneyDatatoInt(dt.Rows[0]["EXTRA06"].ToString().Trim());
                intEXTRA07 = SalaryPublic.intMoneyDatatoInt(dt.Rows[0]["EXTRA07"].ToString().Trim());
                intEXTRA08 = SalaryPublic.intMoneyDatatoInt(dt.Rows[0]["EXTRA08"].ToString().Trim());
                intEXTRA09 = SalaryPublic.intMoneyDatatoInt(dt.Rows[0]["EXTRA09"].ToString().Trim());

                //總計
                intINCREASE = (intSALARYPAY1 + intBOSS + intPROFESS + intWORKP + intTECHNICS + intBONUS + intADVENTIVE + intFAR + intELECTRIC + intBONUS);
                intDECREASE = (intHEALTHPAY + intHEALTHPAY1 + intMONTHPAY_TAX + intMONTHPAY + intTAX + intINSURANCEPAY + intCONCUR3PAY) + (intEXTRA01 + intEXTRA02 + intEXTRA03 + intEXTRA04 + intEXTRA05 + intEXTRA06 + intEXTRA07 + intEXTRA08 + intEXTRA09);
                intTPAY = intINCREASE - intDECREASE;

                //結卡旗標比對判斷

                if (strLOCKDB.Equals("Y"))
                {
                    btUpdate.Enabled = false;
                    btDelete.Enabled = false;
                    btSave.Enabled = false;
                    btExit.Enabled = false;
                    TextBox_NOTE.Enabled = false;
                    lb_islock.Visible = true;
                }
                else
                {
                    btUpdate.Enabled = true;
                    btDelete.Enabled = true;
                    btSave.Enabled = true;
                    btExit.Enabled = true;
                    lb_islock.Visible = false;
                }

                TextBox_NOTE.Enabled = false;
                foreach (Object item in Panel_Incease.Controls)
                {
                    if (item is TextBox)
                    {
                        TextBox tbox = (TextBox)item;
                        tbox.Enabled = false;
                    }
                    else if (item is DropDownList)
                    {
                        DropDownList ddlist = (DropDownList)item;
                        ddlist.Enabled = false;
                    }
                    else if (item is Button)
                    {
                        Button bt = (Button)item;
                        bt.Enabled = false;
                    }
                }

                foreach (Object item in Panel_Decease.Controls)
                {
                    if (item is TextBox)
                    {
                        TextBox tbox = (TextBox)item;
                        tbox.Enabled = false;
                    }
                    else if (item is Button)
                    {
                        Button bt = (Button)item;
                        bt.Enabled = false;
                    }
                }

                foreach (Object item in Panel_OtherDecease.Controls)
                {
                    if (item is TextBox)
                    {
                        TextBox tbox = (TextBox)item;
                        tbox.Enabled = false;
                    }

                }
                
            }
        }


        /// <summary>
        /// 基本資料與薪資-資料初始化
        /// </summary>
        private void voidClear()
        {
            currentIndex = 0;

            btTable.Enabled = true;
            btUpdate.Enabled = false;
            btDelete.Enabled = false;
            btSave.Enabled = false;
            btExit.Enabled = false;

            //清除修改、刪除功能判斷
            strstrUpdate_Delete = "";

            //基本資料
            strIDCARD = "";
            strMZ_NAME = "";
            strMZ_POLNO = "";
            strPAY_AD = "";
            strMZ_UNIT = "";
            strMZ_OCCC = "";
            strMZ_SRANK = "";
            strMZ_SLVC = "";
            strMZ_SPT = "";
            intINCREASE = 0;
            intDECREASE = 0;
            intTPAY = 0;
            strNOTE = "";

            //以下應發金額
            intSALARYPAY1 = 0;
            intBOSS = 0;
            intPROFESS = 0;
            strWORKP_Dr = "";
            intWORKP = 0;
            strTECHNICS_Dr = "";
            intTECHNICS = 0;
            strBONUS_Dr = "";
            intBONUS = 0;
            strADVENTIVE_Dr = "";
            intADVENTIVE = 0;
            intFAR = 0;
            strELECTRIC_Dr = "";
            intELECTRIC = 0;
            intOtheradd = 0;

            //以下應扣金額
            intHEALTHID = 0;
            intHEALTHMAN = 0;
            intHEALTHPAY = 0;
            intHEALTHPAY1 = 0;
            intMONTHPAY_TAX = 0;
            intMONTHPAY = 0;
            intINSURANCEPAY = 0;
            intTAX = 0;
            intCONCUR3PAY = 0;
            intOtherminus = 0;

            //以下其他應扣金額
            intEXTRA01 = 0;
            intEXTRA02 = 0;
            intEXTRA03 = 0;
            intEXTRA04 = 0;
            intEXTRA05 = 0;
            intEXTRA06 = 0;
            intEXTRA07 = 0;
            intEXTRA08 = 0;
            intEXTRA09 = 0;

            //不允許輸入
            enableControls(false);
        }


        private void voidGV_In_De()
        {
            int MonthPay_Data_MM_SNID = SalaryMonth.intMonthPay_Data_MM_SNID(strIDCARD, strAMONTH);

            //SqlDataSource_INCREASE.SelectCommand = "SELECT MO_SNID, '其他'+ CASE(MP) WHEN 'M' THEN '應扣' ELSE '應發' END +'金額：'+PAY AS TEXT_DATA FROM B_MONTHPAY_OTHER_PAY WHERE MM_SNID = " + MonthPay_Data_MM_SNID + " AND MP = 'P' ORDER BY LASTDA DESC";
            //GridView_INCREASE.DataBind();

            //SqlDataSource_DECREASE.SelectCommand = "SELECT MO_SNID, '其他'+ CASE(MP) WHEN 'M' THEN '應扣' ELSE '應發' END +'金額：'+PAY AS TEXT_DATA FROM B_MONTHPAY_OTHER_PAY WHERE MM_SNID = " + MonthPay_Data_MM_SNID + " AND MP = 'M' ORDER BY LASTDA DESC";
            //GridView_DECREASE.DataBind();
        }

        private void voidTPay()
        {
            intOtheradd = SalaryMonth_View.intSearch_SalaryMonth_Data_Other_Increase(strIDCARD, strAMONTH);
            intOtherminus = SalaryMonth_View.intSearch_SalaryMonth_Data_Other_Decrease(strIDCARD, strAMONTH);

            intINCREASE = SalaryMonth_View.intSearch_SalaryMonth_Data_Increase(strAMONTH, strIDCARD);
            intDECREASE = SalaryMonth_View.intSearch_SalaryMonth_Data_Decrease(strAMONTH, strIDCARD);

            int TPay = intINCREASE - intDECREASE;
            intTPAY = TPay;
        }

        #region databound

        protected void DropDownList_WORKP_DataBound(object sender, EventArgs e)
        {
            ListItem IDselect = new ListItem("自訂", "0");
            DropDownList_WORKP.Items.Insert(0, IDselect);
        }

        protected void DropDownList_BONUS_DataBound(object sender, EventArgs e)
        {
            ListItem IDselect = new ListItem("自訂", "0");
            DropDownList_BONUS.Items.Insert(0, IDselect);
        }

        protected void DropDownList_TECHNICS_DataBound(object sender, EventArgs e)
        {
            ListItem IDselect = new ListItem("自訂", "0");
            DropDownList_TECHNICS.Items.Insert(0, IDselect);
        }

        protected void DropDownList_ADVENTIVE_DataBound(object sender, EventArgs e)
        {
            ListItem IDselect = new ListItem("自訂", "0");
            DropDownList_ADVENTIVE.Items.Insert(0, IDselect);
        }

        protected void DropDownList_ELECTRIC_DataBound(object sender, EventArgs e)
        {
            ListItem IDselect = new ListItem("自訂", "0");
            DropDownList_ELECTRIC.Items.Insert(0, IDselect);
        }

        #endregion

        #region selectedindexchanged

        protected void DropDownList_WORKP_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (strWORKP_Dr == "0")
            {
                DropDownList_TECHNICS.Enabled = true;
                TextBox_TECHNICS.Enabled = true;
                strWORKP_Dr = "0";
                intWORKP = 0;
                strTECHNICS_Dr = "0";
                intTECHNICS = 0;
            }
            else
            {
                DropDownList_TECHNICS.Enabled = false;
                TextBox_TECHNICS.Enabled = false;
                strTECHNICS_Dr = "0";
                intTECHNICS = 0;
                intWORKP = SalaryBasic.intWorkp_PAY_Data_Serach(strWORKP_Dr);
            }
        }

        protected void DropDownList_BONUS_SelectedIndexChanged(object sender, EventArgs e)
        {
            intBONUS = SalaryBasic.intBonus_PAY_Data_Serach(strBONUS_Dr);
        }

        protected void DropDownList_TECHNICS_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (strTECHNICS_Dr == "0")
            {
                DropDownList_WORKP.Enabled = true;
                TextBox_WORKP.Enabled = true;
                strTECHNICS_Dr = "0";
                intTECHNICS = 0;
            }
            else
            {
                DropDownList_WORKP.Enabled = false;
                TextBox_WORKP.Enabled = false;
                strWORKP_Dr = "0";
                intWORKP = 0;
                intTECHNICS = SalaryBasic.intTechnics_PAY_Data_Serach(strTECHNICS_Dr);
            }
        }

        protected void DropDownList_ADVENTIVE_SelectedIndexChanged(object sender, EventArgs e)
        {
            intADVENTIVE = SalaryBasic.intAdventivet_PAY_Data_Serach(strADVENTIVE_Dr);
        }

        protected void DropDownList_ELECTRIC_SelectedIndexChanged(object sender, EventArgs e)
        {
            intELECTRIC = SalaryBasic.intElectric_PAY_Data_Serach(strELECTRIC_Dr);
        }

        #endregion

        protected void TextBox_WORKP_TextChanged(object sender, EventArgs e)
        {
            if (intWORKP == 0)
            {
                DropDownList_WORKP.Enabled = true;
                TextBox_WORKP.Enabled = true;
                DropDownList_TECHNICS.Enabled = true;
                TextBox_TECHNICS.Enabled = true;
                TextBox_TECHNICS.Text = "0";

                strWORKP_Dr = "0";
            }
            else
            {
                DropDownList_TECHNICS.Enabled = false;
                TextBox_TECHNICS.Enabled = false;
                strTECHNICS_Dr = "0";
            }
        }

        protected void TextBox_TECHNICS_TextChanged(object sender, EventArgs e)
        {
            if (intTECHNICS == 0)
            {
                TextBox_TECHNICS.Enabled = true;
                DropDownList_WORKP.Enabled = true;
                TextBox_WORKP.Enabled = true;

                strTECHNICS_Dr = "0";
            }
            else
            {
                DropDownList_WORKP.Enabled = false;
                TextBox_WORKP.Enabled = false;
                strWORKP_Dr = "0";
            }
        }

        #region 切換畫面

        protected void btIN_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 0;
        }

        protected void btDE_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 1;
        }

        protected void btOtherDE_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 2;
        }

        #endregion

        protected void btSave_Click(object sender, EventArgs e)
        {
            switch (strstrUpdate_Delete)
            {
                case "UPDATE":
                    //if (SalaryMonth.boolMonthPay_DataUpdate(strIDCARD, strAMONTH, strMZ_POLNO, strPAY_AD, strMZ_NAME, strMZ_OCCC, strMZ_SRANK, strMZ_SLVC, strMZ_SPT, strMZ_UNIT, strLOCKDB, intSALARYPAY1, intWORKP, intPROFESS, intBOSS, intTECHNICS, intBONUS, intADVENTIVE, intFAR, intELECTRIC, intINSURANCEPAY, intHEALTHID, intHEALTHMAN, intHEALTHPAY, intHEALTHPAY1, intMONTHPAY_TAX, intMONTHPAY, intCONCUR3PAY, intTAX, intEXTRA01, intEXTRA02, intEXTRA03, intEXTRA04, intEXTRA05, intEXTRA06, intEXTRA07, intEXTRA08, intEXTRA09, strNOTE))
                    //{
                    //    finddata(SalaryPublic.intMoneyDatatoInt(xcount.Text));
                    //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('修改成功');", true);
                    //}
                    //else
                    //{
                    //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('修改失敗');", true);
                    //}

                    monthpay.update(intSALARYPAY1, intWORKP, intPROFESS, intBOSS, intTECHNICS, intBONUS, intADVENTIVE, intFAR, intELECTRIC, intOtheradd, intINSURANCEPAY, intHEALTHID, intHEALTHMAN,
                        intHEALTHPAY, intHEALTHPAY1, intMONTHPAY_TAX, intMONTHPAY, intCONCUR3PAY, intTAX, intOtherminus, intEXTRA01, intEXTRA02, intEXTRA03, intEXTRA04, intEXTRA05, intEXTRA06,
                        intEXTRA07, intEXTRA08, intEXTRA09, strNOTE);
                    ShowData();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('修改成功')", true);
                    break;
            }
        }

        protected void btExit_Click(object sender, EventArgs e)
        {
            //不允許輸入
            enableControls(false);

            strstrUpdate_Delete = "";
            btSave.Enabled = false;
            btExit.Enabled = false;
        }

        #region 查資料

        // 查詢
        protected void btn_searchConfirm_Click(object sender, EventArgs e)
        {
            string sql;
            List<SqlParameter> ops = new List<SqlParameter>();
            DataTable dt = new DataTable();

            sql = "SELECT ROWNUM-1 RowIndex, MM_SNID, AMONTH, MZ_POLNO, MZ_NAME FROM B_MONTHPAY_MAIN WHERE PAY_AD=@PAY_AD ";
            ops.Add(new SqlParameter("PAY_AD", ddl_searchPayad.SelectedValue));

            if (txt_searchAmonth.Text.Length > 0)
            {
                sql += " AND AMONTH=@AMONTH";
                ops.Add(new SqlParameter("AMONTH", txt_searchAmonth.Text));
            }
            if (txt_searchName.Text.Length > 0)
            {
                sql += " AND MZ_NAME LIKE @MZ_NAME";
                ops.Add(new SqlParameter("MZ_NAME", "%" + txt_searchName.Text + "%"));
            }
            if (txt_searchPolno.Text.Length > 0)
            {
                sql += " AND MZ_POLNO LIKE @MZ_POLNO";
                ops.Add(new SqlParameter("MZ_POLNO", "%" + txt_searchPolno.Text + "%"));
            }
            if (txt_idcard.Text.Length > 0)
            {
                sql += " AND IDCARD=@IDCARD";
                ops.Add(new SqlParameter("IDCARD", txt_idcard.Text));
            }

            sql += " ORDER BY AMONTH, MZ_POLNO";

            dt = o_DBFactory.ABC_toTest.DataSelect(sql, ops);

            if (dt.Rows.Count == 0)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('查無資料');", true);
                return;
            }
            gv_Target.DataSource = dt;
            gv_Target.DataBind();

            searchList.Clear();
            foreach (DataRow row in dt.Rows)
            {
                searchList.Add(int.Parse(row["MM_SNID"].ToString()));
            }

            currentIndex = 0;
            ShowData();
        }

        protected void btn_Selector_Click(object sender, EventArgs e)
        {
            btn_searchResult_ModalPopupExtender.Show();
        }

        protected void gv_Target_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            currentIndex = int.Parse(e.CommandArgument.ToString());
            ShowData();
        }

        // 呈現資料
        private void ShowData()
        {
            btSave.Enabled = false;
            btExit.Enabled = false;
            btNext_Data.Enabled = false;
            btBack_Data.Enabled = false;

            if (searchList.Count == 0)
            {
                Label_Pages.Visible = false;
                voidClear();
                return;
            }

            if (searchList.Count > 1)
            {
                // 不在第一筆的時候
                if (currentIndex != 0)
                    btBack_Data.Enabled = true;

                // 不在最後一筆的時候
                if (currentIndex != searchList.Count - 1)
                    btNext_Data.Enabled = true;
            }

            Label_Pages.Visible = true;
            Label_Pages.Text = "第" + (currentIndex + 1).ToString() + "筆；共" + searchList.Count.ToString() + "筆";

            getMonthpay(searchList[currentIndex]);

            //不允許輸入
            enableControls(false);
        }

        // 取得當前查詢的monthpay資料
        protected void getMonthpay(int sn)
        {
            monthpay = new Monthpay(sn);

            strAMONTH = monthpay.amonth;
            strIDCARD = monthpay.idcard;
            strMZ_NAME = monthpay.name;
            strMZ_POLNO = monthpay.polno;
            strPAY_AD = monthpay.payadName;
            strMZ_UNIT = monthpay.unitName;
            strMZ_SRANK = monthpay.srank;
            strMZ_SLVC = monthpay.slvc;
            strMZ_OCCC = monthpay.occc;
            strMZ_SPT = monthpay.spt;

            intSALARYPAY1 = monthpay.salary;
            intBOSS = monthpay.boss;
            intPROFESS = monthpay.profess;
            intWORKP = monthpay.work;
            intTECHNICS = monthpay.tech;
            intBONUS = monthpay.bonus;
            intADVENTIVE = monthpay.adventive;
            intFAR = monthpay.far;
            intELECTRIC = monthpay.electric;
            intOtheradd = monthpay.otheradd;

            intINSURANCEPAY = monthpay.insurance;
            intHEALTHID = monthpay.healthPersonal;
            intHEALTHMAN = monthpay.healthMan;
            intHEALTHPAY = monthpay.health;
            intHEALTHPAY1 = monthpay.healthpay1;
            intMONTHPAY_TAX = monthpay.monthpayTax;
            intMONTHPAY = monthpay.monthpay;
            intCONCUR3PAY = monthpay.concur3;
            intTAX = monthpay.tax;
            intOtherminus = monthpay.otherminus;

            intEXTRA01 = monthpay.extra01;
            intEXTRA02 = monthpay.extra02;
            intEXTRA03 = monthpay.extra03;
            intEXTRA04 = monthpay.extra04;
            intEXTRA05 = monthpay.extra05;
            intEXTRA06 = monthpay.extra06;
            intEXTRA07 = monthpay.extra07;
            intEXTRA08 = monthpay.extra08;
            intEXTRA09 = monthpay.extra09;

            strNOTE = monthpay.note;

            intINCREASE = intSALARYPAY1 + intBOSS + intPROFESS + intWORKP + intTECHNICS + intBONUS + intADVENTIVE + intFAR + intELECTRIC + intOtheradd;
            intDECREASE = intINSURANCEPAY + intHEALTHPAY + intHEALTHPAY1 + intMONTHPAY_TAX + intMONTHPAY + intCONCUR3PAY + intTAX + intOtherminus + intEXTRA01 + intEXTRA02 + intEXTRA03 + intEXTRA04 + intEXTRA05 + intEXTRA06 + intEXTRA07 + intEXTRA08 + intEXTRA09;
            intTPAY = intINCREASE - intDECREASE;

            // 關帳就不能修改囉
            if (monthpay.lockdb)
            {
                btUpdate.Enabled = false;
                btDelete.Enabled = false;
                lb_islock.Visible = true;
            }
            else
            {
                btUpdate.Enabled = true;
                btDelete.Enabled = true;
                lb_islock.Visible = false;
            }
        }

        #endregion

        protected void btUpdate_Click(object sender, EventArgs e)
        {
            strstrUpdate_Delete = "UPDATE";
            btSave.Enabled = true;
            btExit.Enabled = true;

            //允許輸入
            enableControls(true);
        }

        protected void btDelete_Click(object sender, EventArgs e)
        {
            monthpay.delete();

            searchList.RemoveAt(currentIndex);

            if (searchList.Count == 0)// 刪除後如果沒資料了，回到初始狀態
            {
                voidClear();
            }
            else if (currentIndex >= searchList.Count)// 刪除資料後，如果現在的索引超出清單範圍，取清單的最後一筆資料
                currentIndex = searchList.Count - 1;

            ShowData();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('刪除成功')", true);

        }

        protected void btBack_Data_Click(object sender, EventArgs e)
        {
            currentIndex--;
            ShowData();
        }

        protected void btNext_Data_Click(object sender, EventArgs e)
        {
            currentIndex++;
            ShowData();
        }

        
        // 關閉/開啟控制項
        void enableControls(bool enable)
        {
            TextBox_NOTE.Enabled = enable;

            if (enable != true)//20140213應發不可修改
            {
                foreach (Control item in Panel_Incease.Controls)
                {
                    if (item is TextBox)
                        ((TextBox)item).Enabled = enable;
                    if (item is DropDownList)
                        ((DropDownList)item).Enabled = enable;
                }
            }
            foreach (Control item in Panel_Decease.Controls)
            {
                if (item is TextBox)
                    ((TextBox)item).Enabled = enable;
                if (item is DropDownList)
                    ((DropDownList)item).Enabled = enable;
            }
            foreach (Control item in Panel_OtherDecease.Controls)
            {
                if (item is TextBox)
                    ((TextBox)item).Enabled = enable;
                if (item is DropDownList)
                    ((DropDownList)item).Enabled = enable;
            }
        }
    }
}
