using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.IO;
using TPPDDB.App_Code.HRF;
using System.Web.Configuration;
using System.Text;

namespace TPPDDB._2_salary
{
    public partial class B_SalaryToBank_back_PWD : System.Web.UI.Page
    {
        //String strType = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void btn_save_Click(object sender, EventArgs e)
        {
            //上傳檔案驗證
            if (!FileUpload_pwd.HasFile)
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "alert('請選擇檔案')", true);
            if (Path.GetExtension(FileUpload_pwd.FileName) != ".txt")
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "alert('請選擇副檔名為「.txt」的txt檔')", true);

            if (FileUpload_pwd.HasFile)
            {
        
                StreamReader sr = new StreamReader(FileUpload_pwd.PostedFile.InputStream, Encoding.Default);

                string PWD = "";
                string txt = sr.ReadToEnd();

                DataTable dt = new DataTable();
                dt = o_DBFactory.ABC_toTest.Create_Table("select PWD from B_BANK_LIST where BANK_ID=" + ddl_bank.SelectedValue, "Bank_PWD");
                PWD = dt.Rows[0]["PWD"].ToString();
                string ms_bank = SalaryToBank_encryption.TaiwanBank.DESDecrypt(PWD, txt);
             
                string _Path = FileUpload_pwd.FileName;
                _Path = HttpContext.Current.Server.UrlPathEncode(_Path);

                // 設定強制下載標頭。
                HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", _Path));
                Response.ContentType = "application/vnd.ms-word";
                Response.Charset = "";
              
                StringBuilder sbToText = new StringBuilder();
                sbToText.Append(ms_bank);
                Response.Write(sbToText.ToString());
                Response.End();
            }



        }
    }


}
