using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Text;
using System.Data;
using System.IO;

namespace TPPDDB._2_salary
{
    public partial class B_SalaryToBank_back_compare : System.Web.UI.Page
    {
       

        private string strBANK
        {
            get
            {
                return DropDownList_BANK.SelectedValue;
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


            if (!IsPostBack)
            {
                SalaryPublic.checkPermission();

           
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




        /// <summary>
        /// 檢查日期是否符合該項目格式
        /// </summary>
        /// <returns></returns>
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
                
                case "SOLE":
                    tr_BatchNumber.Visible = true;
                    lbl_Tips.Text = "格式範例：0990101";
                    
                    break;
                
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



        protected void btn_compare_Click(object sender, EventArgs e)
        {
            Label_MSG.ForeColor = Color.Red;
            Label_MSG.Text = "";
            if (!HasCorrectFormats())
            {
                Label_MSG.Text = "請檢查日期格式，未輸入或格式不合規格";
                return;
            }

            string error = "";
            //if (!(DropDownList_BANK.SelectedValue == "004" || DropDownList_BANK.SelectedValue == "700" || DropDownList_BANK.SelectedValue == "005" || DropDownList_BANK.SelectedValue == "008" || DropDownList_BANK.SelectedValue == "951"))
            //{

            //    error += @"該金融機構尚未完成\r\n";

            //}

            if (DropDownList_TYPE.SelectedValue == "-1")
            {
                error += @"請選擇發放類別\r\n";
            }
            if (string.IsNullOrEmpty(error))
            {


                string tmp_url = "B_rpt.aspx?fn=salary_back_different&TPM_FION=" + Request["TPM_FION"] +
             "&AD=" + strAD + "&TYPE=" + DropDownList_TYPE.SelectedValue + "&BANKID=" + DropDownList_BANK.SelectedValue + "&CASEID=" + txt_BatchNumber.Text + "&DATADATE=" + TextBox_DATE.Text + "&IN_ACCOUNT_DATE=" + TextBox_TransDate.Text;
                

               ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "alert('" + error + "')", true);
                return;
            }
        }
              
          

     
    }
}
