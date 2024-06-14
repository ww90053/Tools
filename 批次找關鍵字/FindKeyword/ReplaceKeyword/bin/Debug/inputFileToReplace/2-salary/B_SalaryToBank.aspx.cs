using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Drawing;

namespace TPPDDB._2_salary
{
    public partial class B_SalaryToBank : System.Web.UI.Page
    {
        
        DataTable dtUnitAccounts//目前各機關的各銀行劃撥帳號
        {
            get { return (DataTable)ViewState["dtUnitAccounts"]; }
            set { ViewState["dtUnitAccounts"] = value; }
        }
       
        private string strBANK
        {
            get
            {
                return DropDownList_BANK.SelectedValue;
            }
        }
        private string strMEMO
        {
            get
            {
                return TextBox_MEMO.Text;
            }
        }
        private string strTYPE
        {
            get
            {
                return DropDownList_TYPE.SelectedValue;
            }
        }

        private string strTYPENAME
        {
            get
            {
                return DropDownList_TYPE.SelectedItem.Value;
            }
        }

        private string strAD
        {
            get
            {
                return DropDownList_AD.SelectedValue;
            }
        }
        string caseid { get { return txt_BatchNumber.Text; } }
        private string strDATE
        {
            get
            {
                return TextBox_DATE.Text;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["ADPMZ_ID"].ToString() == "R1XXX11721")
                //if(Session["ADPMZ_ID"]=="R122311721")
                btToTXT_encryption.Visible = true;
            
            if (!IsPostBack)
            {SalaryPublic.checkPermission();

                dtUnitAccounts = o_DBFactory.ABC_toTest.DataSelect("SELECT B_BRANCH.ID PAY_AD, B_BANK.ID BANKID, \"GROUP\", FIANCENO1 FROM B_BRANCH_BANK JOIN B_BANK ON B_BANK.B_SNID=B_BRANCH_BANK.BANK_ID JOIN B_BRANCH ON B_BRANCH_BANK.B_SNID=B_BRANCH.B_SNID");
                txt_unit.Text = "人事";

                SalaryPublic.fillDropDownList(ref this.DropDownList_AD);
                switch (Request.QueryString["date"])
                {
                    case "0":
                        Label_MSG.Text = "請檢查日期格式，未輸入或格式不合規格";
                        break;
                    default:
                        Label_MSG.Text = "";
                        break;
                }
                switch (Request.QueryString["data"])
                {
                    case "0":
                        Label_MSG.Text = "查詢結果無資料";
                        break;
                    default:
                        break;
                }
            }
            else
            {
                Label_MSG.Text = "";
            }
        }

       

        protected void DropDownList_BANK_SelectedIndexChanged(object sender, EventArgs e)
        {
            //切換銀行時把此機關的劃撥帳號帶出
            DataRow[] dra = dtUnitAccounts.Select("PAY_AD='" + strAD + "' AND BANKID='" + strBANK + "'");
            if (dra.Length == 0)
                txt_unitacc.Text = "未設定此銀行劃撥帳號";
            else
            {
                txt_unitacc.Text = dra[0]["FIANCENO1"].ToString();
            }

            // 淡水的格式有疑慮，先把額外欄位的功能拿掉
            return;

            /*if (strBANK == "119")
            {
                memo.Visible = true;
                sMEMO.Visible = true;
            }
            else
            {
                memo.Visible = false;
                sMEMO.Visible = false;
            }*/
        }

     

        private bool HasCorrectFormats()
        {
            switch (DropDownList_TYPE.SelectedValue)
            {
                case "MONTH":
                case "REPAIR":
                case "OFFER":
                    if (strDATE.Length != 5)
                        return false;
                    break;

                case "YEAR":
                case "EFFECT":
                    if (strDATE.Length != 3)
                        return false;
                    break;

                default:
                    if (strDATE.Length != 7)
                        return false;
                    break;
            }

            if (TextBox_TransDate.Text.Length != 7)
                return false;

            return true;
        }

        protected void DropDownList_TYPE_SelectedIndexChanged(object sender, EventArgs e)
        {
            tr_BatchNumber.Visible = false;
            tr_id.Visible = false;
            tr_name.Visible = false;
            //2013/09/12
            order_by.Visible = false;
            //2013/09/12
            switch (DropDownList_TYPE.SelectedValue)
            {
                case "MONTH":
                case "OFFER":
                    lbl_Tips.Text = "格式範例：09901";
                    break;

                case "REPAIR":
                    tr_BatchNumber.Visible = true;
                    lbl_Tips.Text = "格式範例：09901";
                    break;

                case "YEAR":
                case "EFFECT":
                    lbl_Tips.Text = "格式範例：099";
                    break;
                //2013/09/12
                case "SOLE":
                    tr_BatchNumber.Visible = true;
                    lbl_Tips.Text = "格式範例：0990101";
                    order_by.Visible = true;
                    break;
                //2013/09/12
                default:
                    tr_BatchNumber.Visible = true;
                    lbl_Tips.Text = "格式範例：0990101";
                    
                    break;
            }
        }

        private string GetTransDate()
        {
            return TextBox_TransDate.Text.Substring(0, 3) + "年" + TextBox_TransDate.Text.Substring(3, 2) + "月" + TextBox_TransDate.Text.Substring(5, 2) + "日";
        }

        // 產生轉存電子檔
        protected void btToTXT_Click(object sender, EventArgs e)
        {
            string strAD = DropDownList_AD.SelectedValue;
            string strFileName = "";

            Label_MSG.ForeColor = Color.Red;
            Label_MSG.Text = "";
            if (!HasCorrectFormats())
            {
                Label_MSG.Text = "請檢查日期格式，未輸入或格式不合規格";
                return;
            }

            if (DropDownList_TYPE.SelectedValue == "SOLE")
            {
                strFileName = string.Format("PSBP-PAY-NEW-{0}.txt", caseid);
            }
            else
            {
                strFileName = "PSBP-PAY-NEW.txt";
            }

            //匯出txt檔
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=" + strFileName);//txt檔名
            Response.ContentType = "application/vnd.ms-word";
            Response.Charset = "";
            StringBuilder sbToText = new StringBuilder();

            //if (strBANK == "119")
            //    Session["sMEMO"] = TextBox_sMEMO.Text.Trim();

            sbToText.Append(SalaryToBank.sb_Is_Bank(strAD, strTYPE, strDATE, TextBox_TransDate.Text, strBANK, strMEMO, caseid,"1",txt_item.Text,  1 ,1));

            Response.Write(sbToText.ToString());
            Response.End();
        }

        #region 機關團體戶存款單

        protected void Button_PRINT_Click(object sender, EventArgs e)
        {
            Label_MSG.Text = "";
            DropDownList_TYPE.BackColor = Color.White;
            DropDownList_BANK.BackColor = Color.White;
            TextBox_DATE.BackColor = Color.White;
            if (DropDownList_TYPE.SelectedValue == "-1")
            {
                DropDownList_TYPE.BackColor = Color.Orange;
            }
            if (DropDownList_BANK.SelectedValue == "-1")
            {
                DropDownList_BANK.BackColor = Color.Orange;
            }
            if (TextBox_DATE.Text.Trim().Length == 0)
            {
                Label_MSG.Text = "請檢查日期格式，未輸入或格式不合規格";
                Label_MSG.ForeColor = Color.Red;
                TextBox_DATE.BackColor = Color.Orange;
                return;
            }
           

            if (!HasCorrectFormats())
            {
                Label_MSG.Text = "請檢查日期格式，未輸入或格式不合規格";
                return;
            }

            string tmp_url = "B_rpt.aspx?fn=bank_list&PAYAD=" + DropDownList_AD.SelectedValue +
                "&BANK=" + DropDownList_BANK.SelectedValue + "&TYPE=" + DropDownList_TYPE.SelectedValue + "&DATE=" + TextBox_DATE.Text +
                "&BNO=" + txt_BatchNumber.Text + "&CASE=" + caseid +
                "&TRANSDATE=" + TextBox_TransDate.Text +
                "&BUNITNO=" + HttpUtility.UrlEncode(txt_unitacc.Text) +
                "&toUnit=" + HttpUtility.UrlEncode(txt_unit.Text) + "&item=" + HttpUtility.UrlEncode(txt_item.Text) +
                "&TPM_FION=" + Request.QueryString["TPM_FION"];

            ScriptManager.RegisterClientScriptBlock(UpdatePanel2, this.GetType(), "click", "go_print('" + tmp_url + "');", true);

        }
               
        #endregion

        #region 薪資存款帳戶移送單

        protected void btn_detailAll_Click(object sender, EventArgs e)
        {
            if (!HasCorrectFormats())
            {
                Label_MSG.Text = "請檢查日期格式，未輸入或格式不合規格";
                return;
            }
                       

            Label_MSG.Text = "";
            TextBox_DATE.BackColor = Color.White;
            
            string tmp_url = "B_rpt.aspx?fn=AmountDetailAll&PAYAD=" + DropDownList_AD.SelectedValue +
                "&BANK=" + DropDownList_BANK.SelectedValue + "&TYPE=" + DropDownList_TYPE.SelectedValue + "&DATE=" + TextBox_DATE.Text +
                "&BNO=" + txt_BatchNumber.Text + "&CASE=" + caseid +
                "&TRANSDATE=" + TextBox_TransDate.Text +
                "&ORDER=" + ddl_ORDER_BY.SelectedValue +
              "&BUNITNO=" + HttpUtility.UrlEncode(txt_unitacc.Text) +

                "&toUnit=" + HttpUtility.UrlEncode(txt_unit.Text) + "&item=" + HttpUtility.UrlEncode(txt_item.Text) +
                "&TPM_FION=" + Request.QueryString["TPM_FION"];

            switch (DropDownList_TYPE.SelectedValue)
            {
                case "SOLE":
                   tmp_url= tmp_url.Replace("AmountDetail", "AmountDetail2");
                    break;
            }

          
            ScriptManager.RegisterClientScriptBlock(UpdatePanel2, this.GetType(), "click", "go_print('" + tmp_url + "');", true);



        }

        //存款帳戶移送單依單位
        protected void Button_PRINT1_Click(object sender, EventArgs e)
        {
            if (!HasCorrectFormats())
            {
                Label_MSG.Text = "請檢查日期格式，未輸入或格式不合規格";
                return;
            }

          

            Label_MSG.Text = "";
            TextBox_DATE.BackColor = Color.White;

           
            string tmp_url = "B_rpt.aspx?fn=AmountDetail&PAYAD=" + DropDownList_AD.SelectedValue +
                "&BANK=" + DropDownList_BANK.SelectedValue + "&TYPE=" + DropDownList_TYPE.SelectedValue + "&DATE=" + TextBox_DATE.Text +
                "&BNO=" + txt_BatchNumber.Text + "&CASE=" + caseid +
                "&TRANSDATE=" + TextBox_TransDate.Text +
                "&ORDER=" + ddl_ORDER_BY.SelectedValue +
              "&BUNITNO=" + HttpUtility.UrlEncode(txt_unitacc.Text) +

                "&toUnit=" + HttpUtility.UrlEncode(txt_unit.Text) + "&item=" + HttpUtility.UrlEncode(txt_item.Text) +
                "&TPM_FION=" + Request.QueryString["TPM_FION"];


            switch (DropDownList_TYPE.SelectedValue)
            {
                case "SOLE":
                    
                    tmp_url=tmp_url.Replace("AmountDetail", "AmountDetail2");
                    break;

                default:
                    break;
            }

           

            ScriptManager.RegisterClientScriptBlock(UpdatePanel2, this.GetType(), "click", "go_print('" + tmp_url + "');", true);



        }


        #endregion

        protected void btToTXT_encryption_Click(object sender, EventArgs e)
        {
            string strAD = DropDownList_AD.SelectedValue;
            string strFileName = "";

            Label_MSG.ForeColor = Color.Red;
            Label_MSG.Text = "";
            if (!HasCorrectFormats())
            {
                Label_MSG.Text = "請檢查日期格式，未輸入或格式不合規格";
                return;
            }

            if (DropDownList_TYPE.SelectedValue == "SOLE")
            {
                strFileName = string.Format("PSBP-PAY-NEW-{0}.txt", caseid);
            }
            else
            {
                strFileName = "PSBP-PAY-NEW.txt";
            }

            //匯出txt檔
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=" + strFileName);//txt檔名
            Response.ContentType = "application/vnd.ms-word";
            Response.Charset = "";
            StringBuilder sbToText = new StringBuilder();

            //if (strBANK == "119")
            //    Session["sMEMO"] = TextBox_sMEMO.Text.Trim();

            sbToText.Append(SalaryToBank.sb_Is_Bank(strAD, strTYPE, strDATE, TextBox_TransDate.Text, strBANK, strMEMO, caseid,"1",txt_item.Text, 1,1));

            Response.Write(sbToText.ToString());
            Response.End();
        }
    }
}
