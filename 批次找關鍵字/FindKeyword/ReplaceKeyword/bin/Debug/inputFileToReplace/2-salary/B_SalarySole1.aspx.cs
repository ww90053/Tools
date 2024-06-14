using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Configuration;
using System.Data.SqlClient;
using TPPDDB.App_Code;


namespace TPPDDB._2_salary
{
    public partial class B_SalarySole1 : System.Web.UI.Page
    {
        Sole sole
        {

            set { ViewState["sole"] = value; }
            get { return (Sole)ViewState["sole"]; }
        }

        [Serializable]
        struct sPeople
        {
            public string id;
            public string name;
            public string polno;
            public string unit;
            public string occc;
            public string srank;
            public string slvc;
        }
        sPeople people
        {
            get { return (sPeople)ViewState["people"]; }
            set { ViewState["people"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {


            if (IsPostBack != true)
            {//檢驗權限
                SalaryPublic.checkPermission();
                people = new sPeople();

                btCreate.Enabled = true;
                btUpdate.Enabled = false;
                btDelete.Enabled = false;
                btExit.Enabled = false;
                Panel_ADD.Visible = false;

                Sole.setDDLTaxID(ref ddl_taxid, strNUM);
                Sole.setDDLTaxID1(ref ddl_taxid1, ddl_taxid.SelectedValue);
            }

            //TextBox_DA.Attributes.Add("onkeypress", "changeFocus(document.getElementById('" + this.txt_caseid.UniqueID + "'));");
            //txt_caseid.Attributes.Add("onkeypress", "changeFocus(document.getElementById('" + this.txt_num.UniqueID + "'));");
            //TextBox_PAY.Attributes.Add("onkeypress", "changeFocus(document.getElementById('" + this.btCreate.UniqueID + "'));");

            // textbox控制項把enter換成tab
            TextBox_DA.Attributes.Add("onkeydown", "javascript:if(event.keyCode == 13)event.keyCode = 9;");
            txt_caseid.Attributes.Add("onkeydown", "javascript:if(event.keyCode == 13)event.keyCode = 9;");
            TextBox_PAY.Attributes.Add("onkeydown", "javascript:if(event.keyCode == 13)event.keyCode = 9;");
            txt_num.Attributes.Add("onkeydown", "javascript:if(event.keyCode == 13)event.keyCode = 9;");
            TextBox_MZ_POLNO.Attributes.Add("onkeydown", "javascript:if(event.keyCode == 13)event.keyCode = 9;");
            TextBox_IDCARD.Attributes.Add("onkeydown", "javascript:if(event.keyCode == 13)event.keyCode = 9;");
            TextBox_MZ_NAME.Attributes.Add("onkeydown", "javascript:if(event.keyCode == 13)event.keyCode = 9;");
            TextBox_TAX.Attributes.Add("onkeydown", "javascript:if(event.keyCode == 13)event.keyCode = 9;");
            TextBoxSaveUNTax.Attributes.Add("onkeydown", "javascript:if(event.keyCode == 13)event.keyCode = 9;");
            TextBox_PAY1.Attributes.Add("onkeydown", "javascript:if(event.keyCode == 13)event.keyCode = 9;");
            TextBox_PAY2.Attributes.Add("onkeydown", "javascript:if(event.keyCode == 13)event.keyCode = 9;");
            TextBox_PAY3.Attributes.Add("onkeydown", "javascript:if(event.keyCode == 13)event.keyCode = 9;");
        }

        protected void finddata(int snid, bool isTrue)
        {
            string strSQL2 = @"SELECT IDCARD,MZ_NAME ,MZ_POLNO ,DA_INOUT_GROUP , DA ,CASEID,
                               NUM ,TAXES_ID1 ,PAY ,TAX , PAY1 ,PAY2 ,PAY3 ,SAVEUNTAX , NOTE ,SECOND_HEALTHPAY_PAY , EXTERNAL
                               FROM B_SOLE WHERE S_SNID = " + snid;
            DataTable dt = new DataTable();
            dt = o_DBFactory.ABC_toTest.Create_Table(strSQL2, "S_SNID");
            if (dt.Rows.Count == 1)
            {
                strIDCARD = dt.Rows[0]["IDCARD"].ToString();
                strMZ_NAME = dt.Rows[0]["MZ_NAME"].ToString();
                strMZ_POLNO = dt.Rows[0]["MZ_POLNO"].ToString();
                strDA_INOUT_GROUP = dt.Rows[0]["DA_INOUT_GROUP"].ToString();
                strDA = dt.Rows[0]["DA"].ToString();
                strCASEID = dt.Rows[0]["CASEID"].ToString();
                strNUM = dt.Rows[0]["NUM"].ToString();
                strTAXES_ID1 = dt.Rows[0]["TAXES_ID1"].ToString();

                intPAY = int.Parse(dt.Rows[0]["PAY"].ToString());
                intTAX = int.Parse(dt.Rows[0]["TAX"].ToString());
                intPAY1 = int.Parse(dt.Rows[0]["PAY1"].ToString());
                intPAY2 = int.Parse(dt.Rows[0]["PAY2"].ToString());
                intPAY3 = int.Parse(dt.Rows[0]["PAY3"].ToString());
                intUnTaxSave = int.Parse(dt.Rows[0]["SAVEUNTAX"].ToString());
                strNOTE = dt.Rows[0]["NOTE"].ToString();

                ////
                if (dt.Rows[0]["SECOND_HEALTHPAY_PAY"].ToString() != "" && dt.Rows[0]["SECOND_HEALTHPAY_PAY"].ToString() != null)
                    intSecand_HealthyPay = int.Parse(dt.Rows[0]["SECOND_HEALTHPAY_PAY"].ToString());
                else
                    intSecand_HealthyPay = 0;


                if (dt.Rows[0]["EXTERNAL"].ToString() != "" && dt.Rows[0]["EXTERNAL"].ToString() != null)
                    strExternal = dt.Rows[0]["EXTERNAL"].ToString();
                else
                    strExternal = "N";
                ////
            }
        }

        #region 變數

        private string strIDCARD
        {
            get
            {
                return TextBox_IDCARD.Text.ToUpper();
            }
            set
            {
                TextBox_IDCARD.Text = value;
            }
        }

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

        //入賬類別
        private string strDA_INOUT_GROUP
        {
            get
            {
                return RadioButtonList_DA_INOUT_GROUP.SelectedValue;
            }
            set
            {
                RadioButtonList_DA_INOUT_GROUP.SelectedValue = value;
            }
        }

        //入帳日期
        private string strDA
        {
            get
            {
                return TextBox_DA.Text;
            }
            set
            {
                TextBox_DA.Text = value;
            }
        }

        //入帳案號
        private string strCASEID
        {
            get { return txt_caseid.Text; }
            set { txt_caseid.Text = value; }
        }

        //項目名稱
        private string strNUM
        {
            get
            {
                return txt_num.Text;
            }
            set
            {
                txt_num.Text = value;
                lbl_num.Text = Sole.getName(txt_num.Text);
            }
        }

        private bool boolTAXFLAG
        {
            get
            {
                return CheckBox_TAXFLAG.Checked;
            }
            set
            {
                CheckBox_TAXFLAG.Checked = value;
            }
        }

        //所得格式代碼
        private string strTAXES_ID1
        {
            get { return ddl_taxid1.SelectedValue; }
            set
            {
                try
                {
                    ddl_taxid1.SelectedValue = value;
                }
                catch { }
            }
        }

        //項目說明
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

        private int intPAY
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_PAY.Text);
            }
            set
            {
                if (value == 0)
                    TextBox_PAY.Text = "";
                else
                    TextBox_PAY.Text = value.ToString();
                //TextBox_PAY.Text = String.Format("{0:$#,#0}", int.Parse(value.ToString()));
            }
        }

        private int intTAX
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_TAX.Text);
            }
            set
            {
                TextBox_TAX.Text = value.ToString();
                //TextBox_TAX.Text = String.Format("{0:$#,#0}", int.Parse(value.ToString()));
            }
        }

        private int intPAY1
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_PAY1.Text);
            }
            set
            {
                TextBox_PAY1.Text = value.ToString();
                //TextBox_PAY1.Text = String.Format("{0:$#,#0}", int.Parse(value.ToString()));
            }
        }

        private int intPAY2
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_PAY2.Text);
            }
            set
            {
                TextBox_PAY2.Text = value.ToString();
                //TextBox_PAY2.Text = String.Format("{0:$#,#0}", int.Parse(value.ToString()));
            }
        }

        private int intPAY3
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_PAY3.Text);
            }
            set
            {
                TextBox_PAY3.Text = value.ToString();
                //TextBox_PAY3.Text = String.Format("{0:$#,#0}", int.Parse(value.ToString()));
            }
        }

        //法院扣款
        int extra01
        {
            get
            {
                return SalaryPublic.intdelimiterChars(txt_extra01.Text);
            }
            set
            {
                txt_extra01.Text = value.ToString();
            }
        }


        int intSecand_HealthyPay
        {
            get
            {
                return SalaryPublic.intdelimiterChars(txt_Secand_HealthyPay.Text);
            }
            set
            {
                txt_Secand_HealthyPay.Text = value.ToString();
            }
        }

        string strExternal
        {
            get
            {
                return ddl_External.SelectedValue;
            }
            set
            {
                ddl_External.SelectedValue = value.ToString();
            }


        }

        //自提離職儲金
        private int intUnTaxSave
        {
            set
            {
                TextBoxSaveUNTax.Text = value.ToString();
            }
            get
            {
                return SalaryPublic.intdelimiterChars(TextBoxSaveUNTax.Text);
            }
        }

        private int intCREATE_IN
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_CREATE_IN.Text);
            }
            set
            {
                TextBox_CREATE_IN.Text = String.Format("{0:$#,#0}", int.Parse(value.ToString()));
            }
        }

        private int intCREATE_DE
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_CREATE_DE.Text);
            }
            set
            {
                TextBox_CREATE_DE.Text = String.Format("{0:$#,#0}", int.Parse(value.ToString()));
            }
        }

        private int intCREATE_REAL
        {
            get
            {
                return SalaryPublic.intdelimiterChars(TextBox_CREATE_REAL.Text);
            }
            set
            {
                TextBox_CREATE_REAL.Text = String.Format("{0:$#,#0}", int.Parse(value.ToString()));
            }
        }

        private string strTAXESID
        {
            get
            {
                return SalarySole.strTAXES_ID(strNUM);
            }
        }

        // 單一發放取用登入者的現服機關作為發薪機關，已跟出納邱瑞麗小姐確認
        private string strPAY_AD
        {
            get
            {
                return SalaryPublic.strLoginEXAD;
            }
        }

        private string strMZ_OCCC
        {
            get
            {
                string MZ_OCCC = "";
                if (strMZ_POLNO.Trim().Length >= 3)
                {
                    switch (strMZ_POLNO.Substring(0, 3))
                    {
                        case "BSN":
                        case "HSN":
                            MZ_OCCC = "";
                            break;
                        default:
                            //20140701
                            //MZ_OCCC = SalaryPublic.strMZ_OCCC_Data(strMZ_POLNO);
                            MZ_OCCC = SalaryPublic.str_A_Column("MZ_OCCC", 2, strMZ_POLNO);
                            break;
                    }
                }
                else
                {//20140701
                    // MZ_OCCC = SalaryPublic.strMZ_OCCC_Data(strMZ_POLNO);
                    MZ_OCCC = SalaryPublic.str_A_Column("MZ_OCCC", 2, strMZ_POLNO);
                }
                return MZ_OCCC;
            }
        }

        private string strMZ_SRANK
        {
            get
            {
                string MZ_SRANK = "";
                if (strMZ_POLNO.Trim().Length >= 3)
                {
                    switch (strMZ_POLNO.Substring(0, 3))
                    {
                        case "BSN":
                        case "HSN":
                            MZ_SRANK = "";
                            break;
                        default:
                            //20140701
                            MZ_SRANK = SalaryPublic.str_A_Column("MZ_SRANK", 2, strMZ_POLNO);
                            //MZ_SRANK = SalaryPublic.strMZ_SRANK_Data(strMZ_POLNO);
                            break;
                    }
                }
                else
                {
                    MZ_SRANK = SalaryPublic.str_A_Column("MZ_SRANK", 2, strMZ_POLNO);
                    //MZ_SRANK = SalaryPublic.strMZ_SRANK_Data(strMZ_POLNO);
                }
                return MZ_SRANK;
            }
        }

        private string strMZ_SLVC
        {
            get
            {
                string MZ_SLVC = "";
                if (strMZ_POLNO.Trim().Length >= 3)
                {
                    switch (strMZ_POLNO.Substring(0, 3))
                    {
                        case "BSN":
                        case "HSN":
                            MZ_SLVC = "";
                            break;
                        default:
                            //20140701
                            MZ_SLVC = SalaryPublic.str_A_Column("MZ_SLVC", 2, strMZ_POLNO);
                            //MZ_SLVC = SalaryPublic.strMZ_SLVC_Data(strMZ_POLNO);
                            break;
                    }
                }
                else
                {
                    //20140701
                    MZ_SLVC = SalaryPublic.str_A_Column("MZ_SLVC", 2, strMZ_POLNO);
                    //MZ_SLVC = SalaryPublic.strMZ_SLVC_Data(strMZ_POLNO);
                }
                return MZ_SLVC;
            }
        }

        private string strMZ_SPT
        {
            get
            {
                string MZ_SPT = "";
                if (strMZ_POLNO.Trim().Length >= 3)
                {
                    switch (strMZ_POLNO.Substring(0, 3))
                    {
                        case "BSN":
                        case "HSN":
                            MZ_SPT = "";
                            break;
                        default:
                            //20140701
                            MZ_SPT = SalaryPublic.str_A_Column("MZ_SPT", 2, strMZ_POLNO);
                            //MZ_SPT = SalaryPublic.strMZ_SPT_Data(strMZ_POLNO);
                            break;
                    }
                }
                else
                {
                    //20140701
                    MZ_SPT = SalaryPublic.str_A_Column("MZ_SPT", 2, strMZ_POLNO);
                    //MZ_SPT = SalaryPublic.strMZ_SPT_Data(strMZ_POLNO);
                }
                return MZ_SPT;
            }
        }

        private string strLOCKDB
        {
            get
            {
                string LOCKDB = "Y";
                if (Session["B_SOLESelectKeyword"] != null && (string)Session["B_SOLESelectKeyword"] != "")
                {
                    LOCKDB = SalarySole.str_LOCKDB_Data_Serach(int.Parse(Session["B_SOLESelectKeyword"].ToString()));
                }
                return LOCKDB;
            }
        }

        private string strTAXFLAG
        {
            get
            {
                if (boolTAXFLAG)
                {
                    return "Y";
                }
                else
                {
                    return "N";
                }
            }
        }

        #endregion

        protected void voidChanged(object sender, EventArgs e)
        {
            switch ((sender as TextBox).ID)
            {
                case "TextBox_PAY":
                    intPAY = SalaryPublic.intdelimiterChars(TextBox_PAY.Text);
                    break;
                case "TextBox_TAX":
                    intTAX = SalaryPublic.intdelimiterChars(TextBox_TAX.Text);
                    break;
                case "TextBox_PAY1":
                    intPAY1 = SalaryPublic.intdelimiterChars(TextBox_PAY1.Text);
                    break;
                case "TextBox_PAY2":
                    intPAY2 = SalaryPublic.intdelimiterChars(TextBox_PAY2.Text);
                    break;
                case "TextBox_PAY3":
                    intPAY3 = SalaryPublic.intdelimiterChars(TextBox_PAY3.Text);
                    break;
                case "TextBoxSaveUNTax":
                    intUnTaxSave = SalaryPublic.intdelimiterChars(TextBoxSaveUNTax.Text);
                    break;
            }
        }

        #region 員工編號/姓名/身份證號Textbox值改變時查詢

        protected void TextBox_IDCARD_TextChanged(object sender, EventArgs e)
        {
            if (!strIDCARD.Equals(""))
            {
                getPeople(strIDCARD);

                //下面

                //Check_Secand_HealthyPay();

                if (people.id != null)
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "id2pay", "setTimeout(\"$get('" + TextBox_PAY.ClientID + "').focus();\", 100);", true);
            }
            else
            {
                strMZ_NAME = "";
                strMZ_POLNO = "";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('找不到人員')", true);
            }
        }

        protected void TextBox_MZ_NAME_TextChanged(object sender, EventArgs e)
        {
            if (!strMZ_NAME.Equals(""))
            {
                strIDCARD = GetIDCardbyName(strMZ_NAME);


                if (strIDCARD == "")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('找不到人員')", true);
                    return;
                }
                else if (strIDCARD == "many")
                    return;

                getPeople(strIDCARD);

                //下面

                //Check_Secand_HealthyPay();

                if (people.id != null)
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "name2pay", "setTimeout(\"$get('" + TextBox_PAY.ClientID + "').focus();\", 100);", true);
            }
            else
            {
                strIDCARD = "";
                strMZ_POLNO = "";
            }
        }

        protected void TextBox_MZ_POLNO_TextChanged(object sender, EventArgs e)
        {
            if (sole != null)
                return;

            if (!strMZ_POLNO.Equals(""))
            {
                strIDCARD = GetIDCardbyPolno(strMZ_POLNO);

                if (strIDCARD == "")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('找不到人員')", true);
                    return;
                }
                else if (strIDCARD == "many")
                    return;

                getPeople(strIDCARD);

                //下面


                //Check_Secand_HealthyPay();



                if (people.id != null)
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "pol2pay", "setTimeout(\"$get('" + TextBox_PAY.ClientID + "').focus();\", 100);", true);
            }
        }

        void getPeople(string id)
        {
            people = new sPeople();
            Police police = new Police(id);

            //人事資料找不到人，去其他基本資料找
            if (police.id == null)
            {
                OtherPeople op = new OtherPeople(SalaryPublic.strLoginEXAD, id);

                if (op.sn == 0)
                {
                    strMZ_NAME = "";
                    strMZ_POLNO = "";
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('找不到人員')", true);
                    return;
                }

                people = new sPeople()
                {
                    id = op.id,
                    name = op.name,
                    polno = op.polno
                };
                strMZ_NAME = people.name;
                strMZ_POLNO = people.polno;
            }
            else
            {
                //人事資料找到人了

                people = new sPeople()
                {
                    id = police.id,
                    name = police.name,
                    polno = police.polno,
                    occc = police.occc,
                    slvc = police.slvc,
                    srank = police.srank,
                    unit = police.unit
                };
                strMZ_NAME = people.name;
                strMZ_POLNO = people.polno;
            }
        }

        private string GetIDCardbyName(string name)
        {
            DataTable dt = new DataTable();
            string strSQL;

            strSQL = string.Format("SELECT VW_ALL_PEOPLE.*, PAY_AD PA FROM VW_ALL_PEOPLE WHERE NAME='{0}'", name);
            dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "1");
            DataRow[] dr;
            if (dt.Rows.Count > 1)
            {
                dr = dt.Select(string.Format("PA = '{0}'", Session["ADPMZ_EXAD"].ToString()));
                if (dr.Count() == 1)
                {
                    return dr[0]["IDCARD"].ToString();
                }
                else
                {
                    ShowSelector(dt);
                    return "many";
                }
            }
            else if (dt.Rows.Count == 1)
                return dt.Rows[0]["IDCARD"].ToString();

            return "";
        }

        private string GetIDCardbyPolno(string polno)
        {
            DataTable dt = new DataTable();
            string strSQL;

            // 先找人事資料
            strSQL = string.Format("SELECT MZ_ID IDCARD, MZ_NAME NAME, MZ_POLNO, AD.MZ_KCHI PAY_AD, UNIT.MZ_KCHI UNIT,PAY_AD PA FROM A_DLBASE LEFT JOIN A_KTYPE AD ON PAY_AD=AD.MZ_KCODE LEFT JOIN A_KTYPE UNIT ON MZ_EXUNIT=UNIT.MZ_KCODE AND UNIT.MZ_KTYPE='25' WHERE MZ_POLNO='{0}' AND (PAY_AD='{1}' OR PAY_AD IS NULL OR PAY_AD='-1')", polno, SalaryPublic.strLoginEXAD);
            dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "1");
            DataRow[] dr;
            if (dt.Rows.Count > 1)
            {
                dr = dt.Select(string.Format("PA = '{0}'", Session["ADPMZ_EXAD"].ToString()));
                if (dr.Count() == 1)
                {
                    return dr[0]["IDCARD"].ToString();
                }
                else
                {
                    ShowSelector(dt);
                    return "many";
                }
            }
            else if (dt.Rows.Count == 1)
                return dt.Rows[0]["IDCARD"].ToString();

            // 找不到再找其他資料
            strSQL = string.Format("SELECT IDCARD, NAME, MZ_POLNO, AD.MZ_KCHI PAY_AD, '' UNIT,PAY_AD PA FROM B_MANUFACTURER_BASE LEFT JOIN A_KTYPE AD ON PAY_AD=AD.MZ_KCODE WHERE MZ_POLNO='{0}' AND PAY_AD='{1}'", polno, SalaryPublic.strLoginEXAD);
            dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "1");
            if (dt.Rows.Count > 1)
            {
                dr = dt.Select(string.Format("PA = '{0}'", Session["ADPMZ_EXAD"].ToString()));
                if (dr.Count() == 1)
                {
                    return dr[0]["IDCARD"].ToString();
                }
                else
                {
                    ShowSelector(dt);
                    return "many";
                }
            }
            else if (dt.Rows.Count == 1)
                return dt.Rows[0]["IDCARD"].ToString();

            return "";
        }

        private void ShowSelector(DataTable dt)
        {
            gv_Target.DataSource = dt;
            gv_Target.DataBind();

            pl_Selector_ModalPopupExtender.Show();
        }

        private string GetName(string idcard)
        {
            DataTable dt = new DataTable();
            string strSQL;

            // 先找人事資料
            strSQL = string.Format("SELECT MZ_NAME FROM A_DLBASE WHERE MZ_ID='{0}'", idcard);
            dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "1");

            if (dt.Rows.Count != 0)
                return dt.Rows[0][0].ToString();

            // 找不到再找其他資料，其他基本資料限制只能查自己負責的發薪機關下的人
            strSQL = string.Format("SELECT NAME FROM B_MANUFACTURER_BASE WHERE PAY_AD='{0}' AND IDCARD='{1}'", SalaryPublic.strLoginEXAD, idcard);
            dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "1");

            if (dt.Rows.Count != 0)
                return dt.Rows[0][0].ToString();

            // 找不到再找義警民防資料
            strSQL = string.Format("SELECT NAME FROM H_VPBASE WHERE IDNO='{0}'", idcard);
            dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "1");

            if (dt.Rows.Count != 0)
                return dt.Rows[0][0].ToString();

            return "";
        }

        private string GetPOLNO(string idcard)
        {
            DataTable dt = new DataTable();
            string strSQL;

            strSQL = string.Format("SELECT MZ_POLNO FROM A_DLBASE WHERE MZ_ID='{0}'", idcard);
            dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "1");

            if (dt.Rows.Count != 0)
                return dt.Rows[0][0].ToString();

            // 找不到再找其他資料
            strSQL = string.Format("SELECT MZ_POLNO FROM B_MANUFACTURER_BASE WHERE PAY_AD='{0}' AND IDCARD='{1}'", SalaryPublic.strLoginEXAD, idcard);
            dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "1");

            if (dt.Rows.Count != 0)
                return dt.Rows[0][0].ToString();

            // 找不到再找義警民防資料
            strSQL = string.Format("SELECT \"NUMBER\" FROM H_VPBASE WHERE IDNO='{0}'", idcard);
            dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "1");

            if (dt.Rows.Count != 0)
                return dt.Rows[0][0].ToString();

            return "";
        }

        protected void gv_Target_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                int index = int.Parse(e.CommandArgument.ToString());
                strIDCARD = gv_Target.DataKeys[index]["IDCARD"].ToString();

                getPeople(strIDCARD);
                //strMZ_NAME = gv_Target.DataKeys[index]["NAME"].ToString();
                //strMZ_POLNO = gv_Target.DataKeys[index]["MZ_POLNO"].ToString();
            }
        }

        protected void btn_Cancel_Click(object sender, EventArgs e)
        {
            strIDCARD = "";
            strMZ_NAME = "";
            strMZ_POLNO = "";

            pl_Selector_ModalPopupExtender.Hide();
        }

        #endregion

        protected void RadioButtonList_DA_INOUT_GROUP_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RadioButtonList_DA_INOUT_GROUP.SelectedValue == "IN") { }
            else
            {
                TextBox_PAY.Enabled = true;
                TextBox_PAY1.Enabled = true;
                TextBox_TAX.Enabled = true;
            }
        }

        protected void TextBoxSaveUNTax_TextChanged(object sender, EventArgs e)
        {
            voidChanged(sender, e);
        }

        protected void ddl_taxid_SelectedIndexChanged(object sender, EventArgs e)
        {
            Sole.setDDLTaxID1(ref ddl_taxid1, ddl_taxid.SelectedValue);
        }

        #region 呈現項目選擇panel

        protected void txt_num_TextChanged(object sender, EventArgs e)
        {
            strNUM = txt_num.Text.PadLeft(2, '0');
            numSelected();
            strNOTE = lbl_num.Text;

            //下面//

            TextBox_MZ_POLNO.Focus();
        }

        protected void btn_showNum_Click(object sender, EventArgs e)
        {
            showCode();
            btn_showNum_ModalPopupExtender.Show();
        }

        protected void gv_num_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            strNUM = e.CommandArgument.ToString();
            numSelected();
            strNOTE = lbl_num.Text;


            //下面
            //Check_Secand_HealthyPay();
            //Check_immediate();


            TextBox_MZ_POLNO.Focus();
        }

        protected void gv_num_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_num.PageIndex = e.NewPageIndex;
            showCode();
            btn_showNum_ModalPopupExtender.Show();
        }

        // 產生代碼選擇器的資料
        void showCode()
        {
            string sql;

            sql = "SELECT ID, NAME FROM B_SOLEITEM ORDER BY ID";

            gv_num.DataSource = o_DBFactory.ABC_toTest.DataSelect(sql);
            gv_num.DataBind();
        }

        // 項目代碼改變時需要做的處理
        void numSelected()
        {
            boolTAXFLAG = SalarySole.boolTAXES_YESNO(strNUM);
            Sole.setDDLTaxID(ref ddl_taxid, strNUM);
            Sole.setDDLTaxID1(ref ddl_taxid1, ddl_taxid.SelectedValue);
        }

        #endregion

        #region 本次新增清單相關

        // 本次新增清單 2011/7/11改為登入者發薪機關+入帳日期+入帳案號的查詢結果，如果入帳日期是空的 就取今日
        private void GridView_Data_Create()
        {
            string strSQL;
            DataTable dt = new DataTable();

            Panel_ADD.Visible = true;

            strSQL = string.Format("select S_SNID, MZ_POLNO, MZ_NAME, DA, PAY, TAX, DES, \"REAL\", NUM from VW_SOLETEMP where DA = '{0}' AND CASEID = '{1}' AND PAY_AD='{2}' ORDER BY S_SNID DESC", strDA, strCASEID, SalaryPublic.strLoginEXAD);

            dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "TempList");

            // 應發總計、應扣總計、實發總計 的計算，已與總局出納 邱瑞麗小姐確認，修改日期100/02/22 by介入
            intCREATE_IN = 0;
            intCREATE_DE = 0;
            intCREATE_REAL = 0;

            if (dt.Rows.Count > 0)
            {
                intCREATE_IN = int.Parse(dt.Compute("sum(PAY)", string.Empty).ToString());
                intCREATE_DE = int.Parse(dt.Compute("sum(DES)", string.Empty).ToString());
                intCREATE_REAL = int.Parse(dt.Compute("sum(REAL)", string.Empty).ToString());
            }
            //20141119
            //foreach (DataRow item in dt.Rows)
            //{
            //    intCREATE_IN += Convert.ToInt32(item["PAY"]);
            //    intCREATE_DE += Convert.ToInt32(item["DES"]);
            //    intCREATE_REAL += Convert.ToInt32(item["REAL"]);
            //}

            TBGridView_Create.DataSource = dt;
            TBGridView_Create.DataBind();
        }

        protected void TBGridView_Create_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "sel")
                return;
            btCreate.Enabled = false;
            btUpdate.Enabled = true;
            btDelete.Enabled = true;
            btExit.Enabled = true;

            sole = new Sole(e.CommandArgument.ToString());

            strIDCARD = sole.idcard;
            strMZ_NAME = sole.name;
            strMZ_POLNO = sole.polno;
            strDA_INOUT_GROUP = sole.inout;
            strDA = sole.da;
            strCASEID = sole.caseid;
            strNUM = sole.num;
            numSelected();
            strTAXES_ID1 = sole.taxesid1;
            intPAY = sole.pay;
            intTAX = sole.tax;
            intPAY1 = sole.pay1;
            intPAY2 = sole.pay2;
            intPAY3 = sole.pay3;
            intUnTaxSave = sole.saveUntax;
            extra01 = sole.extra01;
            //intREAL = intPAY - (intTAX + intPAY1 + intPAY2 + intPAY3);
            strNOTE = sole.note;

            // 關帳就不能再修改了
            if (sole.lockdb)
            {
                btUpdate.Enabled = false;
                btDelete.Enabled = false;
            }

            ////
            intSecand_HealthyPay = sole.second_health_pay;
            strExternal = sole.external;

            ////

            //下面
            //Check_Secand_HealthyPay();
            //ViewState["SN"] = e.CommandArgument.ToString();
            ViewState["Mode"] = "Update";
        }

        protected void TBGridView_Create_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            TBGridView_Create.PageIndex = e.NewPageIndex;
            GridView_Data_Create();
        }

        protected void txt_caseid_TextChanged1(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(strDA))
            {
                if (TBGridView_Create.Rows.Count == 0)
                    //案號改變就抓出日期+案號的單一發放資料
                    GridView_Data_Create();
            }
        }

        #endregion

        void clearValue()
        {
            sole = null;
            strIDCARD = "";
            strMZ_NAME = "";
            strMZ_POLNO = "";

            intPAY = 0;
            intTAX = 0;
            intPAY1 = 0;
            intPAY2 = 0;
            intPAY3 = 0;
            intUnTaxSave = 0;
            ////intREAL = 0;
            ////
            //下面
            intSecand_HealthyPay = 0;

            TextBox_PAY.Text = "";
            TextBox_TAX.Text = "";
            TextBox_PAY1.Text = "";
            TextBox_PAY2.Text = "";
            TextBox_PAY3.Text = "";
            TextBoxSaveUNTax.Text = "";
            txt_extra01.Text = "";
            txt_Secand_HealthyPay.Text = "";



            strExternal = "N";
            // ddl_External.Enabled = false;
            ////
            btCreate.Enabled = true;
            btUpdate.Enabled = false;
            btDelete.Enabled = false;
            btExit.Enabled = false;
        }

        #region 一連串的值改變事件，改變focus

        protected void TextBox_DA_TextChanged(object sender, EventArgs e)
        {
            int caseid;
            int.TryParse(txt_caseid.Text, out caseid);
            if (Sole.isLocked(strDA, caseid))
                txt_caseid.BackColor = System.Drawing.Color.Orange;
            else
                txt_caseid.BackColor = System.Drawing.Color.White;

            GridView_Data_Create();
            txt_caseid.Focus();
        }

        protected void txt_caseid_TextChanged(object sender, EventArgs e)
        {
            int caseid;
            int.TryParse(txt_caseid.Text, out caseid);

            GridView_Data_Create();

            if (Sole.isLocked(strDA, caseid))
                txt_caseid.BackColor = System.Drawing.Color.Orange;
            else
            {
                txt_caseid.BackColor = System.Drawing.Color.White;
                txt_num.Focus();
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "id2pay", "setTimeout(\"$get('" + txt_num.ClientID + "').focus();\", 100);", true);
            }
        }

        

        #endregion

        #region 新增、修改、刪除、取消按鈕事件

        protected void btCreate_Click(object sender, EventArgs e)
        {
            string msg = "";

            if (strDA == "")
                msg += "請輸入入帳日期\\r\\n";
            if (strCASEID == "")
                msg += "請輸入入帳案號\\r\\n";
            if (strNUM == "")
                msg += "請選擇發放項目\\r\\n";
            if (people.id == null)
                msg += "請先選擇一位人員\\r\\n";

            if (msg != "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('" + msg + "')", true);
                return;
            }

            //intREAL = intREAL_Data(intPAY, intTAX, intPAY1, intPAY2, intPAY3);


            string EXCEED = INSERT_Update_SUM_Bonus(); //4???

            if (EXCEED == "Y")
            {
                //計算 二代健保補充保費
                intSecand_HealthyPay = Caculate_intSecand_HealthyPay(intPAY);
                //intSecand_HealthyPay = (int)(intPAY * (0.02));
            }
            //20141216
            //else
            //    intSecand_HealthyPay = 0;


            Sole.insertData(strIDCARD, strMZ_POLNO, strPAY_AD, strMZ_NAME, people.occc, people.srank, people.slvc, strDA_INOUT_GROUP, strDA, strCASEID, strNUM, strTAXESID, strTAXES_ID1, intPAY, strNOTE, intPAY1, intPAY2, intPAY3, intTAX, intUnTaxSave, extra01, intSecand_HealthyPay, strExternal, false);

            GridView_Data_Create();

            //下面
            //INSERT_Update_SUM_Bonus();

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('新增成功')", true);

            //新增成功後，清空員工編號、身分證字號、姓名欄位並將焦點移至該欄位，金額歸0
            intPAY = 0;
            intTAX = 0;
            intPAY1 = 0;
            intPAY2 = 0;
            intPAY3 = 0;
            intUnTaxSave = 0;
            extra01 = 0;


            //下面
            intSecand_HealthyPay = 0;
            strExternal = "N";



            TextBox_MZ_POLNO.Text = "";
            TextBox_IDCARD.Text = "";
            TextBox_MZ_NAME.Text = "";
            TextBox_MZ_POLNO.Focus();
            people = new sPeople();

            ////
            clearValue();
            ////
        }

        /// <summary>
        /// 計算 二代健保補充保費
        /// </summary>
        /// <param name="intPAY"></param>
        /// <returns></returns>
        protected static int Caculate_intSecand_HealthyPay(int intPAY)
        {
            double value = intPAY * (0.0211);
            return  (int)Math.Round(value, MidpointRounding.AwayFromZero);
        }

        protected void btUpdate_Click(object sender, EventArgs e)
        {

            //下面
            string EXCEED = UPDATE_Update_SUM_Bonus();//4???

            if (EXCEED == "Y")
            {   //計算 二代健保補充保費
                intSecand_HealthyPay = Caculate_intSecand_HealthyPay(intPAY);
            }

                //20141216
            //else
            //    intSecand_HealthyPay = 0;

            sole.update(strMZ_POLNO, strNUM, strTAXESID, strTAXES_ID1, intPAY, intTAX, intPAY1, intPAY2, intPAY3, intUnTaxSave, extra01, strNOTE, intSecand_HealthyPay, strExternal);


            //clearValue();//2
            GridView_Data_Create();//3




            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('修改成功')", true);
            ////
            //ViewState.Remove("SN");
            ViewState.Remove("Mode");
            clearValue();
            ////
        }

        protected void btDelete_Click(object sender, EventArgs e)
        {
            ////
            DELETE_Update_SUM_Bonus();
            


            ////
            sole.delete();
            clearValue();
            GridView_Data_Create();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('刪除成功')", true);
            ////
            //ViewState.Remove("SN");
            ViewState.Remove("Mode");
            ////
        }

        protected void btExit_Click(object sender, EventArgs e)
        {
            clearValue();
            ////
            // ViewState.Remove("SN");
            ViewState.Remove("Mode");
            ////
        }

        #endregion


        /// <summary>
        /// 檢查是否超過4倍並給予狀態
        /// </summary>
        //public void Check_Secand_HealthyPay()
        //{
        //    try
        //    {
        //        if ((string.IsNullOrEmpty(TextBox_DA.Text))) //發放日期要填寫
        //        {
        //            ViewState["Secand_HealthyPay"] = false;
        //            return;

        //        }

        //        if ((string.IsNullOrEmpty(txt_num.Text))) //發放項目要填寫
        //        {
        //            ViewState["Secand_HealthyPay"] = false;
        //            return;

        //        }

        //        if ((string.IsNullOrEmpty(TextBox_IDCARD.Text))) //身分證字號要填寫
        //        {
        //            ViewState["Secand_HealthyPay"] = false;
        //            return;

        //        }

        //        string strSQL = "SELECT MZ_NAME FROM A_DLBASE WHERE   (MZ_SRANK LIKE  'P%' OR MZ_SRANK LIKE  'G%' OR  MZ_SRANK LIKE  'B%' ) AND MZ_ID='" + strIDCARD + "'";


        //        DataTable Check_Srank = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");

        //        ViewState["Secand_HealthyPay"] = false;
        //        if (Check_Srank.Rows.Count > 0)//如果有這個人且是公務員
        //        {
        //            if (txt_num.Text == "08" || txt_num.Text == "16" || txt_num.Text == "19" || txt_num.Text == "20" || txt_num.Text == "22" || txt_num.Text == "27" || txt_num.Text == "40")
        //            {
        //                strSQL = "SELECT COUNT(*) FROM B_SUM_BONUS WHERE  EXCEED='Y' AND IDCARD='" + strIDCARD + "' AND AYEAR='" + strDA.Substring(0, 3) + "' AND PAY_AD='" + strPAY_AD + "'";


        //                int Count = int.Parse(o_DBFactory.ABC_toTest.vExecSQL(strSQL));

        //                if (Count > 0)
        //                {
        //                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('已超過獎金總額的4倍，須扣二代健保');", true);

        //                    ViewState["Secand_HealthyPay"] = true;
        //                }

        //            }
        //        }

        //    }

        //    catch (Exception ex)
        //    {
        //        //ViewState["Secand_HealthyPay"] = false;
        //        string error = "";
        //        error += "方法: Check_Secand_HealthyPay() ID: " + TextBox_IDCARD.Text + " 發放項目:" + txt_num.Text + " 入帳日期:" + TextBox_DA.Text + " 原因: " + ex.Message;
        //        Log.SaveLog("B_SalarySole1", "default", error);
        //    }
        //}


        /// <summary>
        /// 新增後SOLE後 ---  Update 二代健保table
        /// </summary>
        public string  INSERT_Update_SUM_Bonus()
        {
            try
            {
                string strSQL = "SELECT MZ_NAME FROM A_DLBASE WHERE   (MZ_SRANK LIKE  'P%' OR MZ_SRANK LIKE  'G%' OR  MZ_SRANK LIKE  'B%' ) AND MZ_ID='" + strIDCARD + "'";


                DataTable Check_Srank = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");

                if (Check_Srank.Rows.Count > 0)
                {

                    if (txt_num.Text == "08" || txt_num.Text == "16" || txt_num.Text == "19" || txt_num.Text == "20" || txt_num.Text == "22" || txt_num.Text == "27" || txt_num.Text == "40")
                    {

                        strSQL = "SELECT SOLE,TOTAL,INCREASE_X4,EXCEED FROM B_SUM_BONUS WHERE  IDCARD='" + strIDCARD + "' AND AYEAR='" + strDA.Substring(0, 3) + "' AND PAY_AD='" + strPAY_AD + "'";

                        DataTable data = new DataTable();

                        data = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");
                        string Exceed = "N";
                        if (data.Rows.Count > 0)
                        {
                            double Sole = Convert.ToDouble(data.Rows[0]["SOLE"]);
                            double Total = Convert.ToDouble(data.Rows[0]["TOTAL"]);
                            double Increase_x4 = Convert.ToDouble(data.Rows[0]["INCREASE_X4"]);
                            Exceed = data.Rows[0]["EXCEED"].ToString();

                            Sole += Convert.ToDouble(TextBox_PAY.Text);
                            Total += Convert.ToDouble(TextBox_PAY.Text);

                            if (Total > Increase_x4)
                            {

                                Exceed = "Y";


                            }


                            strSQL = "UPDATE B_SUM_BONUS SET SOLE=@SOLE,TOTAL=@TOTAL,EXCEED=@EXCEED WHERE IDCARD=@IDCARD AND AYEAR=@AYEAR AND PAY_AD=@PAY_AD ";

                            SqlParameter[] parameterList = {
                    new SqlParameter("SOLE",SqlDbType.Float){Value = Sole},
                    new SqlParameter("TOTAL",SqlDbType.Float){Value = Total},
                    new SqlParameter("EXCEED",SqlDbType.VarChar){Value = Exceed},
                    new SqlParameter("IDCARD",SqlDbType.VarChar){Value = strIDCARD},
                    new SqlParameter("AYEAR",SqlDbType.VarChar){Value = strDA.Substring(0, 3)},
                     new SqlParameter("PAY_AD",SqlDbType.VarChar){Value = strPAY_AD},
                    };
                            o_DBFactory.ABC_toTest.ExecuteNonQuery( strSQL, parameterList);

                            return Exceed;
                        }

                        //else
                        //{
                        //    return;
                        //}

                    }

                    //else
                    //{ return; }
                }
               

                //else
                //{ return; }

            }

            catch (Exception ex)
            {
                try
                {
                    string error = "";
                    error += "方法: Check_immediate() ID: " + TextBox_IDCARD.Text + " 發放項目:" + txt_num.Text + " 入帳日期:" + TextBox_DA.Text + " 原因: " + ex.Message;
                    Log.SaveLog("B_SalarySole1", "default", error);
                }

                catch { }

            }

            return "N";

        }

        /// <summary>
        /// 修改後SOLE後 ---Update 二代健保table
        /// </summary>
        public string   UPDATE_Update_SUM_Bonus()
        {
            try
            {
                string strSQL = "SELECT MZ_NAME FROM A_DLBASE WHERE   (MZ_SRANK LIKE  'P%' OR MZ_SRANK LIKE  'G%' OR  MZ_SRANK LIKE  'B%' ) AND  MZ_ID='" + strIDCARD + "'";


                DataTable Check_Srank = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");

                if (Check_Srank.Rows.Count > 0)
                {


                    strSQL = "SELECT PAY ,NUM FROM B_SOLE WHERE   S_SNID='" + sole.sn + "'";

                    DataTable old = new DataTable();
                    double Calculate = 0;
                    old = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");

                    //如果有修改發送項目
                    if ((old.Rows[0]["NUM"].ToString() == "08" || old.Rows[0]["NUM"].ToString() == "16" || old.Rows[0]["NUM"].ToString() == "19" || old.Rows[0]["NUM"].ToString() == "20" || old.Rows[0]["NUM"].ToString() == "22" || old.Rows[0]["NUM"].ToString() == "27" || old.Rows[0]["NUM"].ToString() == "40")
                        && (!(txt_num.Text == "08" || txt_num.Text == "16" || txt_num.Text == "19" || txt_num.Text == "20" || txt_num.Text == "22" || txt_num.Text == "27" || txt_num.Text == "40")))
                    {
                        strSQL = "SELECT SOLE,TOTAL,INCREASE_X4,EXCEED FROM B_SUM_BONUS WHERE  IDCARD='" + strIDCARD + "' AND AYEAR='" + strDA.Substring(0, 3) + "' AND PAY_AD='" + strPAY_AD + "'";

                        DataTable data = new DataTable();

                        data = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");
                        string Exceed = "N";
                        if (data.Rows.Count > 0)
                        {
                            Calculate = 0 - Convert.ToDouble(old.Rows[0]["PAY"]);
                            double Sole = Convert.ToDouble(data.Rows[0]["SOLE"]);
                            double Total = Convert.ToDouble(data.Rows[0]["TOTAL"]);
                            double Increase_x4 = Convert.ToDouble(data.Rows[0]["INCREASE_X4"]);
                            Exceed = data.Rows[0]["EXCEED"].ToString();

                            Sole += Calculate;
                            Total += Calculate;

                            if (Total > Increase_x4)
                            {

                                Exceed = "Y";

                            }
                            else { Exceed = "N"; }
                            strSQL = "UPDATE B_SUM_BONUS SET SOLE=@SOLE,TOTAL=@TOTAL,EXCEED=@EXCEED WHERE IDCARD=@IDCARD AND AYEAR=@AYEAR AND PAY_AD=@PAY_AD ";
                            SqlParameter[] parameterList = {
                    new SqlParameter("SOLE",SqlDbType.Float){Value = Sole},
                     new SqlParameter("TOTAL",SqlDbType.Float){Value = Total},
                      new SqlParameter("EXCEED",SqlDbType.VarChar){Value = Exceed},
                     new SqlParameter("IDCARD",SqlDbType.VarChar){Value = strIDCARD},
                     new SqlParameter("AYEAR",SqlDbType.VarChar){Value = strDA.Substring(0, 3)},
                    new SqlParameter("PAY_AD",SqlDbType.VarChar){Value = strPAY_AD},
                                                              };
                            o_DBFactory.ABC_toTest.ExecuteNonQuery( strSQL, parameterList);

                        }

                        else
                        { return Exceed; }


                    }
                    //如果"沒"有修改發送項目
                    else if (txt_num.Text == "08" || txt_num.Text == "16" || txt_num.Text == "19" || txt_num.Text == "20" || txt_num.Text == "22" || txt_num.Text == "27" || txt_num.Text == "40")
                    {

                        strSQL = "SELECT SOLE,TOTAL,INCREASE_X4,EXCEED FROM B_SUM_BONUS WHERE  IDCARD='" + strIDCARD + "' AND AYEAR='" + strDA.Substring(0, 3) + "' AND PAY_AD='" + strPAY_AD + "'";

                        DataTable data = new DataTable();

                        data = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");
                        string Exceed = "N";
                        if (data.Rows.Count > 0)
                        {


                            if (TextBox_PAY.Text != "" && TextBox_PAY.Text != null)
                            {

                                if (old.Rows[0]["NUM"].ToString() == "08" || old.Rows[0]["NUM"].ToString() == "16" || old.Rows[0]["NUM"].ToString() == "19" || old.Rows[0]["NUM"].ToString() == "20" || old.Rows[0]["NUM"].ToString() == "22" || old.Rows[0]["NUM"].ToString() == "27" || old.Rows[0]["NUM"].ToString() == "40")
                                {
                                    Calculate = Convert.ToDouble(TextBox_PAY.Text) - Convert.ToDouble(old.Rows[0]["PAY"]);


                                }

                                else
                                {

                                    Calculate = Convert.ToDouble(TextBox_PAY.Text);

                                }
                            }
                            //else
                            //{
                            //    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('應發金額不可為空白');", true);

                            //    return;
                            //}

                            double Sole = Convert.ToDouble(data.Rows[0]["SOLE"]);
                            double Total = Convert.ToDouble(data.Rows[0]["TOTAL"]);
                            double Increase_x4 = Convert.ToDouble(data.Rows[0]["INCREASE_X4"]);
                            Exceed = data.Rows[0]["EXCEED"].ToString();

                            Sole += Calculate;
                            Total += Calculate;

                            if (Total > Increase_x4)
                            {

                                Exceed = "Y";

                            }
                            else { Exceed = "N"; }
                            strSQL = "UPDATE B_SUM_BONUS SET SOLE=@SOLE,TOTAL=@TOTAL,EXCEED=@EXCEED WHERE IDCARD=@IDCARD AND AYEAR=@AYEAR AND PAY_AD=@PAY_AD ";

                            SqlParameter[] parameterList = {
                    new SqlParameter("SOLE",SqlDbType.Float){Value = Sole},
                     new SqlParameter("TOTAL",SqlDbType.Float){Value = Total},
                      new SqlParameter("EXCEED",SqlDbType.VarChar){Value = Exceed},
                     new SqlParameter("IDCARD",SqlDbType.VarChar){Value = strIDCARD},
                     new SqlParameter("AYEAR",SqlDbType.VarChar){Value = strDA.Substring(0, 3)},
                     new SqlParameter("PAY_AD",SqlDbType.VarChar){Value = strPAY_AD},
                                                              };
                            o_DBFactory.ABC_toTest.ExecuteNonQuery( strSQL, parameterList);

                            return Exceed;
                        }

                        //else
                        //{
                        //    return;
                        //}

                    }

                    //else
                    //{ return; }


                }

            }
            catch (Exception ex)
            {

                try
                {
                    string error = "";
                    error += "方法: INSERT_Update_SUM_Bonus() ID: " + TextBox_IDCARD.Text + " 發放項目:" + txt_num.Text + " 入帳日期:" + TextBox_DA.Text + " 原因: " + ex.Message;
                    Log.SaveLog("B_SalarySole1", "default", error);
                }

                catch { }

            }
            return "N";
        }

        /// <summary>
        /// 刪除SOLE後 --- Update二代健保table
        /// </summary>
        public void DELETE_Update_SUM_Bonus()
        {
            try
            {

                string strSQL = "SELECT MZ_NAME FROM A_DLBASE WHERE   (MZ_SRANK LIKE  'P%' OR MZ_SRANK LIKE  'G%' OR  MZ_SRANK LIKE  'B%' ) AND  MZ_ID='" + strIDCARD + "'";


                DataTable Check_Srank = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");

                if (Check_Srank.Rows.Count > 0)
                {

                    strSQL = "SELECT PAY ,NUM  FROM B_SOLE WHERE   S_SNID='" + sole.sn + "'";

                    DataTable old = new DataTable();
                    double Calculate = 0;
                    old = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");

                    //如果舊資料是指定項目時
                    if (old.Rows[0]["NUM"].ToString() == "08" || old.Rows[0]["NUM"].ToString() == "16" || old.Rows[0]["NUM"].ToString() == "19" || old.Rows[0]["NUM"].ToString() == "20" || old.Rows[0]["NUM"].ToString() == "22" || old.Rows[0]["NUM"].ToString() == "27" || old.Rows[0]["NUM"].ToString() == "40")
                    {

                        strSQL = "SELECT SOLE,TOTAL,INCREASE_X4,EXCEED FROM B_SUM_BONUS WHERE  IDCARD='" + strIDCARD + "' AND AYEAR='" + strDA.Substring(0, 3) + "' AND PAY_AD='" + strPAY_AD + "'";

                        DataTable data = new DataTable();

                        data = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");

                        if (data.Rows.Count > 0)
                        {
                            Calculate = Convert.ToDouble(old.Rows[0]["PAY"]);
                            double Sole = Convert.ToDouble(data.Rows[0]["SOLE"]);
                            double Total = Convert.ToDouble(data.Rows[0]["TOTAL"]);
                            double Increase_x4 = Convert.ToDouble(data.Rows[0]["INCREASE_X4"]);
                            string Exceed = data.Rows[0]["EXCEED"].ToString();

                            Sole -= Calculate;
                            Total -= Calculate;

                            if (Total > Increase_x4)
                            {

                                Exceed = "Y";

                            }
                            else { Exceed = "N"; }
                            strSQL = "UPDATE B_SUM_BONUS SET SOLE=@SOLE,TOTAL=@TOTAL,EXCEED=@EXCEED WHERE IDCARD=@IDCARD AND AYEAR=@AYEAR AND PAY_AD=@PAY_AD ";

                            SqlParameter[] parameterList = {
                    new SqlParameter("SOLE",SqlDbType.Float){Value = Sole},
                    new SqlParameter("TOTAL",SqlDbType.Float){Value = Total},
                    new SqlParameter("EXCEED",SqlDbType.VarChar){Value = Exceed},
                    new SqlParameter("IDCARD",SqlDbType.VarChar){Value = strIDCARD},
                    new SqlParameter("AYEAR",SqlDbType.VarChar){Value = strDA.Substring(0, 3)},
                     new SqlParameter("PAY_AD",SqlDbType.VarChar){Value = strPAY_AD},
                    };
                            o_DBFactory.ABC_toTest.ExecuteNonQuery( strSQL, parameterList);

                        }

                        //else
                        //{ return; }

                    }
                    //else
                    //{
                    //    return;

                    //}
                }
                //else
                //{
                //    return;

                //}

            }

            catch (Exception ex)
            {
                try
                {
                    string error = "";
                    error += "方法: DELETE_Update_SUM_Bonus() ID: " + TextBox_IDCARD.Text + " 發放項目:" + txt_num.Text + " 入帳日期:" + TextBox_DA.Text + " 原因: " + ex.Message;
                    Log.SaveLog("B_SalarySole1", "default", error);
                }

                catch { }
            }
        }

        //20141119
        //protected void TextBox_DA_TextChanged1(object sender, EventArgs e)
        //{

        //    Check_Secand_HealthyPay();
        //    txt_caseid.Focus();

        //}

        protected void TextBox_PAY_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBox_DA.Text.Trim()))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請先選擇入帳日期');", true);
                TextBox_PAY.Text = "";
                return;
            }

            if (string.IsNullOrEmpty(txt_num.Text.Trim()))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請先選擇發放項目');", true);
                TextBox_PAY.Text = "";
                return;
            }

            if (string.IsNullOrEmpty(TextBox_IDCARD.Text.Trim()) || string.IsNullOrEmpty(TextBox_MZ_NAME.Text.Trim()))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請先選擇人員');", true);
                TextBox_PAY.Text = "";
                return;
            }

            //Check_immediate();
            TextBox_TAX.Focus();

        }

        /// <summary>
        /// 即時判定是否有超過二代健保
        /// </summary>
        //public void Check_immediate()
        //{
        //    if (string.IsNullOrEmpty(TextBox_DA.Text) || string.IsNullOrEmpty(txt_num.Text) || (string.IsNullOrEmpty(TextBox_MZ_POLNO.Text) || string.IsNullOrEmpty(TextBox_IDCARD.Text) || string.IsNullOrEmpty(TextBox_MZ_NAME.Text) || string.IsNullOrEmpty(TextBox_PAY.Text)))
        //    {

        //        return;
        //    }


        //    try
        //    {
        //        //ViewState["Secand_HealthyPay"] = false;

        //        //計算是否超過 超過給狀態給true.可計算...沒超過給false.給0



        //        //2013/05/10 原先以為當輸入單筆剛好超過就要扣款.現在說不要就把它拿掉(bug如果超過後.修改單筆後變為不超過 (健保費將還是繼續扣款))
        //        //2013/05/21 沒有要扣款但要提示.所以解鎖大部分. 只有給予//ViewState["Secand_HealthyPay"] 的狀態都未解鎖

        //        string strSQL = "SELECT MZ_NAME FROM A_DLBASE WHERE MZ_ID='" + strIDCARD + "' AND (MZ_SRANK LIKE  'P%' OR MZ_SRANK LIKE  'G%' OR  MZ_SRANK LIKE  'B%' )";


        //        DataTable Check_Srank = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");

        //        if (ViewState["Mode"] == null)
        //        {
        //            if (Check_Srank.Rows.Count > 0)
        //            {
        //                ////bug
        //                //strSQL = "SELECT * FROM B_SOLE WHERE   S_SNID='" + ViewState["SN"].ToString() + "'";

        //                //DataTable old = new DataTable();
        //                //double Calculate = 0;
        //                //old = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");


        //                if (txt_num.Text == "08" || txt_num.Text == "16" || txt_num.Text == "19" || txt_num.Text == "20" || txt_num.Text == "22" || txt_num.Text == "27" || txt_num.Text == "40")
        //                {

        //                    strSQL = "SELECT SOLE,TOTAL,INCREASE_X4,EXCEED FROM B_SUM_BONUS WHERE  IDCARD='" + strIDCARD + "' AND AYEAR='" + strDA.Substring(0, 3) + "' AND PAY_AD='" + strPAY_AD + "'";

        //                    DataTable data = new DataTable();

        //                    data = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");

        //                    if (data.Rows.Count > 0)
        //                    {

        //                        double Calculate = 0;

        //                        if (TextBox_PAY.Text != "" && TextBox_PAY.Text != null)
        //                        {
        //                            Calculate = Convert.ToDouble(TextBox_PAY.Text);
        //                        }
        //                        else
        //                        {
        //                            //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('應發金額不可為空白');", true);

        //                            return;
        //                        }


        //                        double Sole = Convert.ToDouble(data.Rows[0]["SOLE"]);
        //                        double Total = Convert.ToDouble(data.Rows[0]["TOTAL"]);
        //                        double Increase_x4 = Convert.ToDouble(data.Rows[0]["INCREASE_X4"]);
        //                        string Exceed = data.Rows[0]["EXCEED"].ToString();

        //                        Sole += Calculate;
        //                        Total += Calculate;

        //                        if (Total > Increase_x4)
        //                        {



        //                            if ((bool)ViewState["Secand_HealthyPay"] == false)
        //                            {
        //                                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('該筆須扣補充保費');", true);
        //                            }
        //                            Exceed = "Y";


        //                        }
        //                        else
        //                        {
        //                            Exceed = "N";

        //                        }
        //                    }

        //                    else
        //                    {
        //                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('尚未建立二代健保資料');", true);


        //                    }
        //                }

        //            }


        //        }
        //        //在修改的狀態下
        //        else if (ViewState["Mode"] != null && ViewState["Mode"].ToString() == "Update")
        //        {
        //            if (Check_Srank.Rows.Count > 0)
        //            {
        //                //bug
        //                strSQL = "SELECT PAY ,NUM FROM B_SOLE WHERE   S_SNID='" + sole.sn + "'";

        //                DataTable old = new DataTable();
        //                double Calculate = 0;
        //                old = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");


        //                if (txt_num.Text == "08" || txt_num.Text == "16" || txt_num.Text == "19" || txt_num.Text == "20" || txt_num.Text == "22" || txt_num.Text == "27" || txt_num.Text == "40")
        //                {

        //                    strSQL = "SELECT SOLE,TOTAL,INCREASE_X4,EXCEED FROM B_SUM_BONUS WHERE  IDCARD='" + strIDCARD + "' AND AYEAR='" + strDA.Substring(0, 3) + "'";

        //                    DataTable data = new DataTable();

        //                    data = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");

        //                    if (data.Rows.Count > 0)
        //                    {

        //                        if (TextBox_PAY.Text != "" && TextBox_PAY.Text != null)
        //                        {

        //                            if (old.Rows[0]["NUM"].ToString() == "08" || old.Rows[0]["NUM"].ToString() == "16" || old.Rows[0]["NUM"].ToString() == "19" || old.Rows[0]["NUM"].ToString() == "20" || old.Rows[0]["NUM"].ToString() == "22" || old.Rows[0]["NUM"].ToString() == "27" || old.Rows[0]["NUM"].ToString() == "40")
        //                            {
        //                                Calculate = Convert.ToDouble(TextBox_PAY.Text) - Convert.ToDouble(old.Rows[0]["PAY"]);

        //                            }

        //                            else
        //                            {

        //                                Calculate = Convert.ToDouble(TextBox_PAY.Text);

        //                            }
        //                        }
        //                        else
        //                        {
        //                            //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('應發金額不可為空白');", true);

        //                            return;
        //                        }



        //                        double Sole = Convert.ToDouble(data.Rows[0]["SOLE"]);
        //                        double Total = Convert.ToDouble(data.Rows[0]["TOTAL"]);
        //                        double Increase_x4 = Convert.ToDouble(data.Rows[0]["INCREASE_X4"]);
        //                        string Exceed = data.Rows[0]["EXCEED"].ToString();

        //                        Sole += Calculate;
        //                        Total += Calculate;

        //                        if (Total > Increase_x4)
        //                        {

        //                            if ((bool)ViewState["Secand_HealthyPay"] == false)
        //                            {
        //                                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('該筆須扣補充保費');", true);
        //                            }
        //                            Exceed = "Y";


        //                        }
        //                        else
        //                        {
        //                            Exceed = "N";

        //                        }
        //                    }
        //                }


        //            }


        //        }


        //        //依照狀態來決定要不要計算建二代健保補充保費

        //        if ((bool)ViewState["Secand_HealthyPay"] == true)
        //        {
        //            double Pay = Convert.ToDouble(TextBox_PAY.Text);
        //            txt_Secand_HealthyPay.Text = (Pay * 0.02).ToString();

        //        }

        //        else
        //        {
        //            txt_Secand_HealthyPay.Text = "0";
        //        }

        //    }

        //    catch (Exception ex)
        //    {

        //        try
        //        {
        //            string error = "";
        //            error += "方法: Check_immediate() ID: " + TextBox_IDCARD.Text + " 發放項目:" + txt_num.Text + " 入帳日期:" + TextBox_DA.Text + " 原因: " + ex.Message;
        //            Log.SaveLog("B_SalarySole1", "default", error);
        //            /*Log.SaveLog();*/



        //        }

        //        catch { }

        //    }


        //}


    }


 
}
