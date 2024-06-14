using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Drawing;
using System.Data;
using System.Net.Mail;
using System.IO;
using System.Net;
using Microsoft.Exchange.WebServices.Data;

//using EASendMail;

namespace TPPDDB._2_salary
{
    public partial class B_SalaryToBank_2 : System.Web.UI.Page
    {
        DataTable dtUnitAccounts//目前各機關的各銀行劃撥帳號
        {
            get { return (DataTable)ViewState["dtUnitAccounts"]; }
            set { ViewState["dtUnitAccounts"] = value; }
        }

        /// <summary>
        /// 取得存款銀行選項Value
        /// </summary>
        private string strBANK
        {
            get
            {
                return DropDownList_BANK.SelectedValue;
            }
        }
        //private string strMEMO
        //{
        //    get
        //    {
        //        return TextBox_MEMO.Text;
        //    }
        //}

        /// <summary>
        /// 取得發放類別選項Value
        /// </summary>
        private string strTYPE
        {
            get
            {
                return DropDownList_TYPE.SelectedValue;
            }
        }
        /// <summary>
        /// 取得發放類別選項文字
        /// </summary>
        private string strTYPENAME
        {
            get
            {
                return DropDownList_TYPE.SelectedItem.Value;
            }
        }
        /// <summary>
        /// 取得發薪機關選項Value
        /// </summary>
        private string strAD
        {
            get
            {
                return DropDownList_AD.SelectedValue;
            }
        }
        string caseid { get { return txt_BatchNumber.Text; } }
        /// <summary>
        /// 取得資料日期
        /// </summary>
        private string strDATE
        {
            get
            {
                return TextBox_DATE.Text;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
           
                btToTXT_encryption.Visible = true;

            if (!IsPostBack)
            {
                SalaryPublic.checkPermission();

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
            {
                txt_unitacc.Text = "未設定此銀行劃撥帳號";
            }
            else
            {
                txt_unitacc.Text = dra[0]["FIANCENO1"].ToString();
            }

            //入帳電子檔測試按鈕
            //移除汐止國泰測試按鈕 20181227 by sky
            if ((strAD.Equals("382132600C") && strBANK.Equals("951")) || (strAD.Equals("382132800C") && strBANK.Equals("013")))
            {
                tr_test.Visible = true;
                switch (strBANK)
                {
                    case "951":
                        btToTXT_Test.Visible = true;
                        btToTXT_Test2.Visible = false;
                        break;
                    //case "013":
                    //    btToTXT_Test.Visible = false;
                    //    btToTXT_Test2.Visible = true;
                    //    break;
                }
            }
            else
            {
                tr_test.Visible = false;
                btToTXT_Test.Visible = false;
                //btToTXT_Test2.Visible = false;
            }

            // 淡水的格式有疑慮，先把額外欄位的功能拿掉
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
          
            switch (DropDownList_TYPE.SelectedValue)
            {
                case "MONTH":
                    txt_item.Text = "每月薪資";
                    lbl_Tips.Text = "格式範例：09901";
                    break;
                case "OFFER":
                    txt_item.Text = "優惠存款";
                    lbl_Tips.Text = "格式範例：09901";
                    break;

                case "REPAIR":
                    tr_BatchNumber.Visible = true;
                    txt_item.Text = "補發薪資";
                    lbl_Tips.Text = "格式範例：09901";
                    break;

                case "YEAR":
                    txt_item.Text = "年終獎金";
                    lbl_Tips.Text = "格式範例：099";
                    break;
                case "EFFECT":
                    txt_item.Text = "考績獎金";
                    lbl_Tips.Text = "格式範例：099";
                    break;
                //2013/09/12
                case "SOLE":
                    tr_BatchNumber.Visible = true;
                    lbl_Tips.Text = "格式範例：0990101";
                   
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

        /// <summary>
        /// 產生轉存電子檔
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            Response.AddHeader("content-disposition", "attachment;filename=" + strFileName);//txt檔名 charset=UTF-8;
            Response.ContentType = "application/vnd.ms-word";
            if (DropDownList_BANK.SelectedValue == "013")
            {
                Response.ContentEncoding = Encoding.Default;
            }
            else
            {
                Response.Charset = "";
            }
            StringBuilder sbToText = new StringBuilder();

            //if (strBANK == "119")
            //    Session["sMEMO"] = TextBox_sMEMO.Text.Trim();

            sbToText.Append(SalaryToBank.sb_Is_Bank(strAD, strTYPE, strDATE, TextBox_TransDate.Text, strBANK, "", caseid, ddl_ORDER_BY.SelectedValue, txt_item.Text, 2, 1));

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
            //    "&SORT=" +ddl_ORDER_BY.SelectedValue+
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
                "&SORT=" + ddl_ORDER_BY.SelectedValue +
              "&BUNITNO=" + HttpUtility.UrlEncode(txt_unitacc.Text) +

                "&toUnit=" + HttpUtility.UrlEncode(txt_unit.Text) + "&item=" + HttpUtility.UrlEncode(txt_item.Text) +
                "&TPM_FION=" + Request.QueryString["TPM_FION"];

            switch (DropDownList_TYPE.SelectedValue)
            {
                case "SOLE":
                    tmp_url = tmp_url.Replace("AmountDetail", "AmountDetail2");
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
                "&SORT=" + ddl_ORDER_BY.SelectedValue +
              "&BUNITNO=" + HttpUtility.UrlEncode(txt_unitacc.Text) +

                "&toUnit=" + HttpUtility.UrlEncode(txt_unit.Text) + "&item=" + HttpUtility.UrlEncode(txt_item.Text) +
                "&TPM_FION=" + Request.QueryString["TPM_FION"];


            switch (DropDownList_TYPE.SelectedValue)
            {
                case "SOLE":

                    tmp_url = tmp_url.Replace("AmountDetail", "AmountDetail2");
                    break;

                default:
                    break;
            }



            ScriptManager.RegisterClientScriptBlock(UpdatePanel2, this.GetType(), "click", "go_print('" + tmp_url + "');", true);



        }


        #endregion

        /// <summary>
        /// 儲存成加密檔(台銀)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            Response.AddHeader("content-disposition", "attachment;filename=" + strFileName);//txt檔名 charset=UTF-8;
            Response.ContentType = "application/vnd.ms-word";
            //Response.ContentType = "text/html; charset=UTF-8";
            Response.Charset = "";
            StringBuilder sbToText = new StringBuilder();

            //if (strBANK == "119")
            //    Session["sMEMO"] = TextBox_sMEMO.Text.Trim();

            sbToText.Append(SalaryToBank.sb_Is_Bank(strAD, strTYPE, strDATE, TextBox_TransDate.Text, strBANK, "", caseid,ddl_ORDER_BY.SelectedValue ,txt_item.Text,  2, 2));

            Response.Write(sbToText.ToString());
            Response.End();
        }

        /// <summary>
        /// 寄發電子郵件至金融機構
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_send_mail_Click(object sender, EventArgs e)
        {
            if (DropDownList_BANK.SelectedValue == "004" )
            {
                Label_MSG.Text = DropDownList_BANK.SelectedItem.Text + "未提供MAIL傳送服務";
                return;
            }
            string mail_address ="";
            if (DropDownList_BANK.SelectedValue == "700")
            {
                mail_address = o_DBFactory.ABC_toTest.vExecSQL(@" SELECT B_BRANCH_BANK .MAIL
                                                            FROM B_BRANCH_BANK 
                                                            JOIN B_BANK ON B_BANK.B_SNID=B_BRANCH_BANK.BANK_ID 
                                                            JOIN B_BRANCH ON B_BRANCH_BANK.B_SNID=B_BRANCH.B_SNID
                                                            WHERE  B_BRANCH.ID ='" + DropDownList_AD.SelectedValue + "' AND B_BANK.ID='700' ");
            }
            else
            {
                mail_address = o_DBFactory.ABC_toTest.vExecSQL("SELECT MAIL FROM B_BANK_LIST WHERE BANK_ID='" + DropDownList_BANK.SelectedValue + "'");
                if (string.IsNullOrEmpty(mail_address))
                {
                    Label_MSG.Text = DropDownList_BANK.SelectedItem.Text + "未設定電子信箱";
                    return;
                }
            }
            string strAD = DropDownList_AD.SelectedValue;
            string strFileName = "";

            Label_MSG.ForeColor = Color.Red;
            Label_MSG.Text = "";
            if (!HasCorrectFormats())
            {
                Label_MSG.Text = "請檢查日期格式，未輸入或格式不合規格";
                return;
            }

            string AD = DropDownList_AD.SelectedItem.Text;
            string Code = AD.Substring(0, 10);
            AD = AD.Replace(Code, "");
            AD = AD == "新北市政府警察局" ? AD : AD.Replace("新北市政府警察局", "");

            if (DropDownList_TYPE.SelectedValue == "SOLE")
            {
                strFileName = string.Format(AD+"-"+DropDownList_TYPE.SelectedItem.Text+"-{0}.txt", caseid);
            }
            else
            {
                strFileName = AD + "-" + DropDownList_TYPE.SelectedItem.Text + ".txt";
            }


            //20140730 華南銀行的檔名
            if (DropDownList_BANK.SelectedValue == "008")
            {
                strFileName = "HNCBIN.txt";
            }

            
            if (DropDownList_BANK.SelectedValue == "005"|| DropDownList_BANK.SelectedValue == "951" || DropDownList_BANK.SelectedValue == "119" || DropDownList_BANK.SelectedValue == "013")
            {
                
            }

            StringBuilder sbToText = new StringBuilder();

            //if (strBANK == "119")
            //    Session["sMEMO"] = TextBox_sMEMO.Text.Trim();

            sbToText.Append(SalaryToBank.sb_Is_Bank(strAD, strTYPE, strDATE, TextBox_TransDate.Text, strBANK, "", caseid, ddl_ORDER_BY.SelectedValue, txt_item.Text, 1, 3));
            
            //20210208 Mark 取出後要壓縮(新莊/三峽/淡水/汐止)或是一般文字檔案

            byte[] to_byte ;

             if (DropDownList_BANK.SelectedValue == "005"|| DropDownList_BANK.SelectedValue == "951" || DropDownList_BANK.SelectedValue == "119" || DropDownList_BANK.SelectedValue == "013")
            {
                System.IO.File.Delete(Server.MapPath("../tempbank/" + strFileName.ToLower().Replace(".txt", "") + ".zip"));
                System.IO.File.WriteAllText(Server.MapPath("../tempbank/" + strFileName), sbToText.ToString());
                System.Threading.Thread.Sleep(1000);
                string pathcmd = Server.MapPath("../tempbank/rar.exe");
                string encryption = o_DBFactory.ABC_toTest.vExecSQL("SELECT PWD FROM B_BANK_LIST WHERE BANK_ID='" + DropDownList_BANK.SelectedValue + "'");
                string path1 = " a -hp" + encryption + " -ep " + Server.MapPath("../tempbank/" + strFileName.ToLower().Replace(".txt", "") + ".zip") + " " + Server.MapPath("../tempbank/" + strFileName);
                System.Diagnostics.Process.Start(pathcmd, path1);
                System.Threading.Thread.Sleep(3000);

                to_byte = System.IO.File.ReadAllBytes(Server.MapPath("../tempbank/" + strFileName.ToLower().Replace(".txt", "") + ".zip")) ;

            }
            else
            {
                to_byte = Encoding.Default.GetBytes(sbToText.ToString());
            }

            

            Stream st = new MemoryStream(to_byte, 0, to_byte.Length);


            string File_Name = strFileName;//DropDownList_AD.SelectedItem.Text +"_"+ TextBox_TransDate.Text + "_" + 
            if (DropDownList_BANK.SelectedValue == "005" || DropDownList_BANK.SelectedValue == "951" || DropDownList_BANK.SelectedValue == "119" || DropDownList_BANK.SelectedValue == "013")
            {
                File_Name = strFileName.ToLower().Replace(".txt", "") + ".zip";
                
            }

            Dictionary<string, Stream> listStream = new Dictionary<string, Stream>();
            listStream.Add(File_Name, st);

           
            string Title = AD +
                "-"+DropDownList_TYPE.SelectedItem.Text+"-入帳電子檔";
            //20210208 Mark國泰世華的名稱比較特別
            if(DropDownList_BANK.SelectedValue == "013")
            {
                Title = "致國泰世華銀行汐止分行入汐止分局員工帳戶";
            }
         

            
            //SendMail_1(mail_address, Title, "", Server.MapPath("../tempbank/" + File_Name) , File_Name);

            //發給自己承辦人
            string mail_address1 = HttpContext.Current.Session["ADServerID"] + "@ntpd.gov.tw";
            string File_Name1 = File_Name.Replace(".zip", "") + ".txt";
            //SendMail_1(mail_address1, Title, "", Server.MapPath("../tempbank/" + File_Name1), File_Name1);

            //ScriptManager.RegisterClientScriptBlock(UpdatePanel2, this.GetType(), "click", "alert('信件已寄發');", true);

            string localip = System.Configuration.ConfigurationManager.AppSettings["ip"].ToString();
            if(localip == "155" )
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "location.href='http://10.2.6.155:98/Send/list?addr=" + mail_address + "," + mail_address1 + "&tmp1=" + HttpContext.Current.Server.UrlEncode(File_Name) + "," + HttpContext.Current.Server.UrlEncode(File_Name1) + "&Title=" + Title + "&tmpwebip=155';", true);

            }
            else if (localip == "156")
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "location.href='http://10.2.6.156:97/Send/list?addr=" + mail_address + "," + mail_address1 + "&tmp1=" + HttpContext.Current.Server.UrlEncode(File_Name) + "," + HttpContext.Current.Server.UrlEncode(File_Name1) + "&Title=" + Title + "&tmpwebip=156';", true);

            }
            else if (localip == "158")
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "location.href='http://10.2.6.158:99/Send/list?addr=" + mail_address + "," + mail_address1 + "&tmp1=" + HttpContext.Current.Server.UrlEncode(File_Name) + "," + HttpContext.Current.Server.UrlEncode(File_Name1) + "&Title=" + Title + "&tmpwebip=158';", true);
            }


        }

        /// <summary>
        /// 寄發電子郵件至金融機構
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_zip_Click(object sender, EventArgs e)
        {
            if (DropDownList_BANK.SelectedValue == "004")
            {
                Label_MSG.Text = DropDownList_BANK.SelectedItem.Text + "未提供MAIL傳送服務";
                return;
            }
            string mail_address = "";
            if (DropDownList_BANK.SelectedValue == "700")
            {
                mail_address = o_DBFactory.ABC_toTest.vExecSQL(@" SELECT B_BRANCH_BANK .MAIL
                                                            FROM B_BRANCH_BANK 
                                                            JOIN B_BANK ON B_BANK.B_SNID=B_BRANCH_BANK.BANK_ID 
                                                            JOIN B_BRANCH ON B_BRANCH_BANK.B_SNID=B_BRANCH.B_SNID
                                                            WHERE  B_BRANCH.ID ='" + DropDownList_AD.SelectedValue + "' AND B_BANK.ID='700' ");
            }
            else
            {
                mail_address = o_DBFactory.ABC_toTest.vExecSQL("SELECT MAIL FROM B_BANK_LIST WHERE BANK_ID='" + DropDownList_BANK.SelectedValue + "'");
                if (string.IsNullOrEmpty(mail_address))
                {
                    Label_MSG.Text = DropDownList_BANK.SelectedItem.Text + "未設定電子信箱";
                    return;
                }
            }
            string strAD = DropDownList_AD.SelectedValue;
            string strFileName = "";

            Label_MSG.ForeColor = Color.Red;
            Label_MSG.Text = "";
            if (!HasCorrectFormats())
            {
                Label_MSG.Text = "請檢查日期格式，未輸入或格式不合規格";
                return;
            }

            string AD = DropDownList_AD.SelectedItem.Text;
            string Code = AD.Substring(0, 10);
            AD = AD.Replace(Code, "");
            AD = AD == "新北市政府警察局" ? AD : AD.Replace("新北市政府警察局", "");

            if (DropDownList_TYPE.SelectedValue == "SOLE")
            {
                strFileName = string.Format(AD + "-" + DropDownList_TYPE.SelectedItem.Text + "-{0}.txt", caseid);
            }
            else
            {
                strFileName = AD + "-" + DropDownList_TYPE.SelectedItem.Text + ".txt";
            }


            //20140730 華南銀行的檔名
            if (DropDownList_BANK.SelectedValue == "008")
            {
                strFileName = "HNCBIN.txt";
            }


            if (DropDownList_BANK.SelectedValue == "005" || DropDownList_BANK.SelectedValue == "951" || DropDownList_BANK.SelectedValue == "119" || DropDownList_BANK.SelectedValue == "013")
            {

            }

            StringBuilder sbToText = new StringBuilder();

            //if (strBANK == "119")
            //    Session["sMEMO"] = TextBox_sMEMO.Text.Trim();

            sbToText.Append(SalaryToBank.sb_Is_Bank(strAD, strTYPE, strDATE, TextBox_TransDate.Text, strBANK, "", caseid, ddl_ORDER_BY.SelectedValue, txt_item.Text, 1, 3));

            //20210208 Mark 取出後要壓縮(新莊/三峽/淡水/汐止)或是一般文字檔案

            byte[] to_byte;
            string encryptionstr = "";

            if (DropDownList_BANK.SelectedValue == "005" || DropDownList_BANK.SelectedValue == "951" || DropDownList_BANK.SelectedValue == "119" || DropDownList_BANK.SelectedValue == "013")
            {
                System.IO.File.Delete(Server.MapPath("../tempbank/" + strFileName.ToLower().Replace(".txt", "") + ".zip"));
                System.IO.File.WriteAllText(Server.MapPath("../tempbank/" + strFileName), sbToText.ToString());
                System.Threading.Thread.Sleep(1000);
                string pathcmd = Server.MapPath("../tempbank/rar.exe");
                string encryption = o_DBFactory.ABC_toTest.vExecSQL("SELECT PWD FROM B_BANK_LIST WHERE BANK_ID='" + DropDownList_BANK.SelectedValue + "'");
                encryptionstr = encryption;
                string path1 = " a -hp" + encryption + " -ep " + Server.MapPath("../tempbank/" + strFileName.ToLower().Replace(".txt", "") + ".zip") + " " + Server.MapPath("../tempbank/" + strFileName);
                System.Diagnostics.Process.Start(pathcmd, path1);
                System.Threading.Thread.Sleep(3000);

                to_byte = System.IO.File.ReadAllBytes(Server.MapPath("../tempbank/" + strFileName.ToLower().Replace(".txt", "") + ".zip"));

            }
            else
            {
                to_byte = Encoding.Default.GetBytes(sbToText.ToString());
            }



            Stream st = new MemoryStream(to_byte, 0, to_byte.Length);


            string File_Name = strFileName;//DropDownList_AD.SelectedItem.Text +"_"+ TextBox_TransDate.Text + "_" + 
            if (DropDownList_BANK.SelectedValue == "005" || DropDownList_BANK.SelectedValue == "951" || DropDownList_BANK.SelectedValue == "119" || DropDownList_BANK.SelectedValue == "013")
            {
                File_Name = strFileName.ToLower().Replace(".txt", "") + ".zip";

            }

            Dictionary<string, Stream> listStream = new Dictionary<string, Stream>();
            listStream.Add(File_Name, st);


            string Title = AD +
                "-" + DropDownList_TYPE.SelectedItem.Text + "-入帳電子檔";
            //20210208 Mark國泰世華的名稱比較特別
            if (DropDownList_BANK.SelectedValue == "013")
            {
                Title = "致國泰世華銀行汐止分行入汐止分局員工帳戶";
            }
            //mail_address = "ei31529@hotmail.com";
            //mail_address = "r11721@ntpd.gov.tw"; //小隊長Email
            //SendMail("ei31529@hotmail.com", "test123", body_detail, listStream);
            //mail_address = "mark@netdoing.com.tw"; //mark Email
            //mail_address = "mark@netdoing.com.tw"; //mark Email

            string downloadfile = File_Name;
            //SendMail_1(mail_address, Title, "", Server.MapPath("../tempbank/" + File_Name), File_Name);
            //發給自己承辦人
            //mail_address = HttpContext.Current.Session["ADServerID"] + "@ntpd.gov.tw";
            string mail_address1 = HttpContext.Current.Session["ADServerID"] + "@ntpd.gov.tw";

            File_Name = File_Name.Replace(".zip", "") + ".txt";
            //SendMail_1(mail_address, Title, "", Server.MapPath("../tempbank/" + File_Name), File_Name);

            string localip = System.Configuration.ConfigurationManager.AppSettings["ip"].ToString();

            string linkdownload = "http://10.2.6."+ localip + ":90/tempbank/" + downloadfile;
            //send_mail_ip.Text = " 發給銀行 ：" + mail_address + " ，發給自己承辦人 ：" + mail_address1 + " ，密 ：" + encryptionstr;
            send_mail_ip.Text = " 發給銀行 ：" + mail_address ;

            ScriptManager.RegisterClientScriptBlock(UpdatePanel2, this.GetType(), "click", "location.href='" + linkdownload + "'", true);
        }

        /// <summary>
        /// Mail發送入帳電子檔
        /// </summary>
        /// <param name="to_email">收件人</param>
        /// <param name="subject">標題</param>
        /// <param name="mail_body">內容</param>
        /// <param name="files">要夾帶的附檔</param>
        public static void SendMail_1(string to_email, string subject, string mail_body, string pathname , string filename)
        {
           
          

            #region 公司
            //client.Credentials = new System.Net.NetworkCredential("ei31529@drinfotech.com.tw", "dRINFO1!"); // 用公司自己信箱          
            //client.Port = 25;//or 587
            //client.Host = "smtp.gmail.com";
            //////啟動SSL 
            //client.EnableSsl = true;
            //client.Send(message);
            #endregion 公司

            #region 北警
            //北警 
            //debug 模式下無法執行 
            //另因SMTP只開放給現在特定三台
            //如之後還有新增SERVER 遇到  SMTP伺服器斯要安全連線.或用戶端未經處理驗證.伺服器回應為 5.7.1 Clinet was not
            //請謝股開放SMTP給那台
            ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2016);
            service.Url = new Uri("https://ntpd97003.tcp.Intra/ews/Exchange.asmx");
            //service.Url = new Uri("https://mail.tcp.Intra/ews/Exchange.asmx");

            // service.Credentials = new NetworkCredential("S62786", "TEL@@7885589", "tcp.Intra");
            service.Credentials = new NetworkCredential("NTPDBANK", "NTPD!qaz@wsx123", "tcp.Intra");
            EmailMessage msg = new EmailMessage(service);

           // EmailMessage msg = new EmailMessage(service);
            msg.ToRecipients.Add(new EmailAddress(to_email));
           //msg.CcRecipients.Add(new EmailAddress(HttpContext.Current.Session["ADServerID"] + "@ntpd.gov.tw"));
            //msg.CcRecipients.Add(new EmailAddress(to_email));
            msg.Subject = subject;
            msg.Body = new MessageBody(BodyType.HTML, mail_body);
                   

            try
            {
                msg.Attachments.AddFileAttachment(filename , pathname);
                //System.IO.File.WriteAllText("C:\\temp\\web" + DateTime.Now.ToString("yyyyMMdd") + ".txt", filename);
                msg.SendAndSaveCopy();
              // client.Send(message);
            }
            catch (Exception e)
            {
                System.IO.File.WriteAllText("C:\\temp\\web" + DateTime.Now.ToString("yyyyMMdd") + ".txt", e.Message);
            }
            #endregion 北警
        }


        /// <summary>
        /// Mail發送入帳電子檔
        /// </summary>
        /// <param name="to_email">收件人</param>
        /// <param name="subject">標題</param>
        /// <param name="mail_body">內容</param>
        /// <param name="files">要夾帶的附檔</param>
        public static void SendMail(string to_email, string subject, string mail_body, Dictionary<string, Stream> files)
        {


            //MailAddress from = new MailAddress("帳號@gmail.com");
           // MailAddress from = new MailAddress("map@ntpd.gov.tw");

            MailAddress from = new MailAddress(HttpContext.Current.Session["ADServerID"] + "@ntpd.gov.tw");
            MailMessage message = new MailMessage();
            
            message.CC.Add(new MailAddress(HttpContext.Current.Session["ADServerID"] + "@ntpd.gov.tw"));
            string[] toa = to_email.Trim().Split(';');
            if (toa.Count() == 1)
            {
                message.To.Add(new MailAddress(to_email));
            }
            else
            {
                foreach (string to in toa)
                {
                    message.To.Add(new MailAddress(to));
                }
            }
            // form
            message.From = from;

            message.BodyEncoding = Encoding.UTF8;

            //信件主旨, 標題
            message.Subject = subject;

            //內容         
            message.Body = mail_body;
            message.IsBodyHtml = true;

            //Attachment attfile = null;

            //if (files != null && files.Count > 0)//有指定夾帶附檔  
            //{

            //    foreach (string fileName in files.Keys)
            //    {

            //        attfile = new Attachment(files[fileName], fileName);

            //        message.Attachments.Add(attfile);

            //    }

            //}



            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();


            #region 公司
            //client.Credentials = new System.Net.NetworkCredential("ei31529@drinfotech.com.tw", "dRINFO1!"); // 用公司自己信箱          
            //client.Port = 25;//or 587
            //client.Host = "smtp.gmail.com";
            //////啟動SSL 
            //client.EnableSsl = true;
            //client.Send(message);
            #endregion 公司

            #region 北警
            //北警 
            //debug 模式下無法執行 
            //另因SMTP只開放給現在特定三台
            //如之後還有新增SERVER 遇到  SMTP伺服器斯要安全連線.或用戶端未經處理驗證.伺服器回應為 5.7.1 Clinet was not
            //請謝股開放SMTP給那台


            //client.Credentials = new System.Net.NetworkCredential("map@ntpd.gov.tw", "1qaz2wsx");
           // client.EnableSsl = true ;
            client.Credentials = new System.Net.NetworkCredential("map@ntpd.gov.tw", "NTPD!qaz@wsx");
            client.Port = 25;
            //近期有修改郵件伺服器設定，已全面改為ntpd.gov.tw。 20190912 by sky
            //client.Host = "tcp.gov.tw";
            client.Host = "ntpd.gov.tw";
            try
            { 
            client.Send(message);
            }
            catch (Exception e)
            {
                System.IO.File.WriteAllText("C:\\temp\\web" + DateTime.Now.ToString("yyyyMMdd") + ".txt", e.Message);
            }
            #endregion 北警
        }

        /// <summary>
        /// 三峽農會入帳電子檔測試
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btToTXT_Test_Click(object sender, EventArgs e)
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
            Response.AddHeader("content-disposition", "attachment;filename=" + strFileName);
            Response.ContentType = "application/vnd.ms-word";
            Response.Charset = "";
            StringBuilder sbToText = new StringBuilder();
            sbToText.Append(SalaryToBank.sb_Is_Bank_Test(strAD, strTYPE, strDATE, TextBox_TransDate.Text, strBANK, "", caseid, ddl_ORDER_BY.SelectedValue, txt_item.Text, 2, 1));

            Response.Write(sbToText.ToString());
            Response.End();
        }
        /// <summary>
        /// 發薪機關選項變更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DropDownList_AD_SelectedIndexChanged(object sender, EventArgs e)
        {
            //移除汐止國泰測試按鈕 20181227 by sky
            if ((strAD.Equals("382132600C") && strBANK.Equals("951")) || (strAD.Equals("382132800C") && strBANK.Equals("013")))
            {
                tr_test.Visible = true;
                switch (strBANK)
                {
                    case "951":
                        btToTXT_Test.Visible = true;
                        btToTXT_Test2.Visible = false;
                        break;
                    //case "013":
                    //    btToTXT_Test.Visible = false;
                    //    btToTXT_Test2.Visible = true;
                    //    break;
                }
            }
            else
            {
                tr_test.Visible = false;
                btToTXT_Test.Visible = false;
                //btToTXT_Test2.Visible = false;
            }
        }
        /// <summary>
        /// 汐止分局國泰入帳電子檔測試
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btToTXT_Test2_Click(object sender, EventArgs e)
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
            Response.AddHeader("content-disposition", "attachment;filename=" + strFileName);
            Response.ContentType = "application/vnd.ms-word";
            Response.ContentEncoding = Encoding.Default;
            StringBuilder sbToText = new StringBuilder();
            sbToText.Append(SalaryToBank.sb_Is_Bank_Test(strAD, strTYPE, strDATE, TextBox_TransDate.Text, strBANK, "", caseid, ddl_ORDER_BY.SelectedValue, txt_item.Text, 2, 1));

            Response.Write(sbToText.ToString());
            Response.End();
        }
    }
}
