using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.IO;

namespace TPPDDB._1_personnel
{
    public partial class Personal_PROLNO_Detail : System.Web.UI.Page
    {

        List<String> RULE_NO = new List<string>();
    
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                A.check_power();
            }

            
        }

        protected void Button_Update_Html_Click(object sender, EventArgs e)
        {
            try
            {
                saveUploadFile();
            }
            catch
            {
                o_CommonService.ShowErrorAlert("請選擇檔案", this);
            }
        }
        private void saveUploadFile()
        {
            if (FileUpload1.FileName.Substring(FileUpload1.FileName.LastIndexOf('.') + 1, 3) == "htm")
            {
                try
                {
                    File.Delete("1-personnel/images/A_PROLNO_DETAIL.htm");
                }
                catch { }
                HttpPostedFile tUploadFile = FileUpload1.PostedFile;
                int tFileLength = tUploadFile.ContentLength;
                byte[] tFileByte = new byte[tFileLength];
                tUploadFile.InputStream.Read(tFileByte, 0, tFileLength);

                FileStream tNewfile = new FileStream(System.Web.HttpContext.Current.Server.MapPath("images/") + "A_PROLNO_DETAIL.htm", FileMode.Create);
                tNewfile.Write(tFileByte, 0, tFileByte.Length);
                tNewfile.Close();
                //XX2013/06/18 
                tNewfile.Dispose();
                o_CommonService.ShowErrorAlert("上傳成功", this);
                // 這是一個全域變數，記錄excel的上傳路徑  
                //Session["path"] = tNewfile.Name;
            }
            else
            {
                o_CommonService.ShowErrorAlert("請選擇htm格式的檔案", this);
            }
        }


    }
         
}
