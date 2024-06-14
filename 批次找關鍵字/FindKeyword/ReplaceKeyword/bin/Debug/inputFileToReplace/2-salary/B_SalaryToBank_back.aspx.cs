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
    public partial class B_SalaryToBank_back : System.Web.UI.Page
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
                    order_by.Visible = true;
                    lbl_Tips.Text = "格式範例：09901";
                    break;

                case "YEAR":
                case "EFFECT":
                    lbl_Tips.Text = "格式範例：099";
                    break;
               
                case "SOLE":
                    tr_BatchNumber.Visible = true;
                    lbl_Tips.Text = "格式範例：0990101";
                    order_by.Visible = true;
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



        protected void btn_export_Click(object sender, EventArgs e)
        {

            string strAD = DropDownList_AD.SelectedValue;
           

            Label_MSG.ForeColor = Color.Red;
            Label_MSG.Text = "";
            if (!HasCorrectFormats())
            {
                Label_MSG.Text = "請檢查日期格式，未輸入或格式不合規格";
                return;
            }


            string error = "";
            //if ( DropDownList_BANK.SelectedValue == "951")
            //{

            //    error += @"該金融機構尚未完成\r\n";

            //}
            if (!ful_txt.HasFile)
            {
                error += @"請選擇上傳的檔案\r\n";
            
            }

            if (!chk_DataCount())
            {
                error += @"該條件未有薪資資料\r\n";
            
            }

            Stream TSR = ful_txt.PostedFile.InputStream;
           
            if (!chk_IN_ACCOUNT_DATE(TSR))
            {
                error += @"輸入的入帳日期與回饋檔不符\r\n";
            }


            if (string.IsNullOrEmpty(error))
            {

                Stream oSR = ful_txt.PostedFile.InputStream;





                Salary_BACK_BANK.sb_Is_Bank_export(oSR, strAD, strTYPE, strDATE, TextBox_TransDate.Text, strBANK, caseid, 1);

                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "alert('匯入成功')", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "alert('"+error +"')", true);
                return;
            }


        }

        /// <summary>
        /// 檢查薪資是否有該資料日期資料
        /// </summary>
        /// <returns></returns>
        public bool chk_DataCount()
        {
            int Count = SalaryToBank.GetTxtDataCount(strAD, strTYPE, strDATE, caseid, strBANK, TextBox_TransDate.Text);
        
               if (Count>0)
                   return true;
               else
                   return false;
        
        }

        /// <summary>
        /// 檢查文字檔的入帳日期與這頁輸入的日障日期是否吻合
        /// </summary>
        /// <param name="oSR"></param>
        /// <returns></returns>
        public   bool chk_IN_ACCOUNT_DATE(Stream oSR)
        {
             StreamReader SR = new StreamReader(oSR, System.Text.Encoding.Default);

                string str_IN_ACCOUNT_DATE_txt="";
                string line = "";

                int i = 0;

                while ((line = SR.ReadLine()) != null)
                {

                    switch (strBANK)
                    {
                        case "005"://土地銀行
                            if (line.Substring(0, 1) == "2")
                            {
                                str_IN_ACCOUNT_DATE_txt = o_str.SubString(line, 23, 7).Trim();
                                if (str_IN_ACCOUNT_DATE_txt == TextBox_TransDate.Text.Trim())
                                    return true;
                            }
                            break;
                        case "008"://華南銀行
                            str_IN_ACCOUNT_DATE_txt = (int.Parse(o_str.SubString(line, 0, 4))-1911).ToString()+o_str.SubString(line, 4, 4).Trim();
                            if (str_IN_ACCOUNT_DATE_txt == TextBox_TransDate.Text.Trim())
                                return true;
                            break;
                        case "004"://臺灣銀行


                            //while ((line = SR.ReadToEnd()) != null)
                            //{
                                if (string.IsNullOrEmpty(line.Trim()))
                                    continue;

                                //姓名碰到罕見字就會照字數來數


                                //付款日期	起始位置17	總長度8
                                str_IN_ACCOUNT_DATE_txt = line.Substring(16, 8).Trim();
                                if (str_IN_ACCOUNT_DATE_txt == TextBox_TransDate.Text.Trim())
                                    return true;
                            //}
                            break;
                        case "700"://中華郵政

                            if (i == 0)
                            {


                            }
                            else if (i == 2)
                            {
                                str_IN_ACCOUNT_DATE_txt = o_str.SubString(line, 25, 9).Trim();
                                if (str_IN_ACCOUNT_DATE_txt == TextBox_TransDate.Text.Trim())
                                    return true;
                            }

                            i++;

                            break;
                        case "951"://農會
                            //20141217
                            str_IN_ACCOUNT_DATE_txt = o_str.SubString(line, 0, 8);
                            //str_IN_ACCOUNT_DATE_txt  = o_str.SubStr(line, 43, 8);
                            str_IN_ACCOUNT_DATE_txt = (int.Parse(o_str.SubString(str_IN_ACCOUNT_DATE_txt, 0, 4)) - 1911).ToString() + o_str.SubString(str_IN_ACCOUNT_DATE_txt, 4, 4).Trim();
                            if (str_IN_ACCOUNT_DATE_txt == TextBox_TransDate.Text.Trim())
                                return true;
                            break;
                        case "119"://淡水一信
                            str_IN_ACCOUNT_DATE_txt = o_str.SubString(line, 0, 8);
                            str_IN_ACCOUNT_DATE_txt = (int.Parse(o_str.SubString(str_IN_ACCOUNT_DATE_txt, 0, 4)) - 1911).ToString() + o_str.SubString(str_IN_ACCOUNT_DATE_txt, 4, 4).Trim();
                            if (str_IN_ACCOUNT_DATE_txt == TextBox_TransDate.Text.Trim())
                                return true;
                            break;
                        default:

                            // return new StringBuilder("不支援此銀行格式！！");
                            break;

                    }
                }

                    return false;
 
        
        
        }

     

  
    }
}
