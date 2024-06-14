using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace TPPDDB._2_salary
{
    public partial class _0_B_BankListPWD : System.Web.UI.Page
    {
        string strGroup_Data_Function
        {
            get { return ViewState["B_strGID"] != null ? ViewState["B_strGID"].ToString() : string.Empty; }
            set { ViewState["B_strGID"] = value; }

        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SalaryPublic.checkPermission();

                strGroup_Data_Function = TPMPermissions._strGroupData_ID(int.Parse(Session["TPM_MID"].ToString()), int.Parse(Request.QueryString["TPM_FION"].ToString()));


                switch (Request["mode"].ToString())
                {
                    case "PWD" :
                        lb_title.Text = "銀行密碼設定";
                        lb_old_data.Text="舊密碼：";
                        lb_new_data.Text="新密碼：";

                        break;
                    case "MAIL":
                        lb_title.Text = "金融機構電子郵件設定";
                        lb_old_data.Text = "舊電子信箱：";
                        lb_new_data.Text = "新電子信箱：";
                        tr_chkPWD.Visible = false;
                        break;
                
                
                }


                switch (strGroup_Data_Function)
                {
                                            
                    case "D":
                    case "E":
                       btn_save.Visible = false;
                        break;


                }





            }

        }

        protected void btn_save_Click(object sender, EventArgs e)
        {
            string error = "";

            switch (Request["mode"].ToString())
            {
                case "PWD":

                    if (ddl_bank.SelectedValue == "-1")
                    {
                        error += "請選擇銀行！\\r\\n";
                    }
                    if (txt_Pwd.Text != txt_Pwd_check.Text)
                    {
                        error += "密碼不一致，請重新確認！\\r\\n";
                    }

                    if (!string.IsNullOrEmpty(error))
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "alert('" + error + "')", true);
                        return;
                    }


                    save_pwd();
                    break;

                case "MAIL":
                    if (ddl_bank.SelectedValue == "-1")
                    {
                        error += "請選擇銀行！\\r\\n";
                    }
                    else if (ddl_bank.SelectedValue == "004" || ddl_bank.SelectedValue == "700")
                    {
                        error += ddl_bank.SelectedItem.Text +"未提供MAIL服務！\\r\\n";
                    
                    }
                   

                    if (!string.IsNullOrEmpty(error))
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "alert('" + error + "')", true);
                        return;
                    }

                    save_mail();
                    break;

            }
            
        }

        public void save_mail()
        {
            
          
            ////前一次密碼

            try
            {
                o_DBFactory.ABC_toTest.vExecSQL(" UPDATE B_BANK_LIST SET MAIL='" + txt_Pwd.Text + "' WHERE BANK_ID='" + ddl_bank.SelectedValue + "'");

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "alert('設定成功')", true);
                txt_OldPwd.Text = txt_Pwd.Text;
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "alert('設定失敗')", true);
            }



        }

        public void save_pwd()
        {
            string error = "";

           
                if (!(Encoding.Default.GetByteCount(txt_Pwd.Text) == 16 || Encoding.Default.GetByteCount(txt_Pwd.Text) == 24))
                {
                    error += "密碼長度為16或24！\\r\\n";
                }
                else
                {
                    if (TPPDDB._2_salary.SalaryToBank_encryption.TaiwanBank.IsWeakKey(txt_Pwd.Text))
                    {
                        error += "密碼未符合安全性規定，請重新輸入！\\r\\n";

                    }
                }

                               
           

            if (!string.IsNullOrEmpty(error))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "alert('" + error + "')", true);
                return;
            }



            ////前一次密碼
            
                try
                {
                    o_DBFactory.ABC_toTest.vExecSQL(" UPDATE B_BANK_LIST SET PWD='" + txt_Pwd.Text + "' ,PWD_OLD=PWD WHERE BANK_ID='" + ddl_bank.SelectedValue + "'");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "alert('設定成功')", true);
                    txt_OldPwd.Text = txt_Pwd.Text;
                }
                catch
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "alert('設定失敗')", true);
                }
           

           
        }

        protected void ddl_bank_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (Request["mode"].ToString())
            {
                case "PWD":
                    if (ddl_bank.SelectedValue != "-1")
                        txt_OldPwd.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT PWD FROM B_BANK_LIST WHERE BANK_ID='" + ddl_bank.SelectedValue + "'");
                    else
                        txt_OldPwd.Text = string.Empty;

                    break;
                case "MAIL":
                    if (ddl_bank.SelectedValue != "-1")
                        txt_OldPwd.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MAIL FROM B_BANK_LIST WHERE BANK_ID='" + ddl_bank.SelectedValue + "'");
                    else
                        txt_OldPwd.Text = string.Empty;
                    break;
            }
        }

       
    }
}
