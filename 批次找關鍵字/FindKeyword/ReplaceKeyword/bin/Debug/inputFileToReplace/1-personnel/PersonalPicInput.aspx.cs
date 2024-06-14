using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data.OleDb;
using System.Data;


namespace TPPDDB._1_personnel
{
    public partial class PersonalPicInput : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            HttpCookie PersonalPicInput_ID_Cookie = new HttpCookie("PersonalPicInput_ID");
            PersonalPicInput_ID_Cookie = Request.Cookies["PersonalPicInput_ID"];


            string ID = TPMPermissions._strDecod(PersonalPicInput_ID_Cookie.Value.ToString());
            Response.Cookies["PersonalPicInput_ID"].Expires = DateTime.Now.AddYears(-1);

            if (FileUpload1.HasFile)
            {
                try
                {
                    
                    HttpPostedFile tUploadFile = FileUpload1.PostedFile;
                    int tFileLength = tUploadFile.ContentLength;
                    byte[] tFileByte = new byte[tFileLength];
                    tUploadFile.InputStream.Read(tFileByte, 0, tFileLength);

                    string findID = "SELECT MZ_ID FROM A_PICPATH WHERE MZ_ID='" + ID + "'";
                    DataTable findIDTable = o_DBFactory.ABC_toTest.Create_Table(findID, "findIDTable");

                    string insertPicPath = "INSERT INTO A_PICPATH(MZ_ID,PICTUREPATH,BUDATE) VALUES('" + ID + "','~/1-personnel/images/" + ID + "-" + (findIDTable.Rows.Count + 1).ToString() + ".jpg',GETDATE())";
                    o_DBFactory.ABC_toTest.Edit_Data(insertPicPath);

                    FileStream tNewfile = new FileStream(System.Web.HttpContext.Current.Server.MapPath("~/1-personnel/images/")+ID+"-"+(findIDTable.Rows.Count+1).ToString()+".jpg", FileMode.Create);
                    tNewfile.Write(tFileByte, 0, tFileByte.Length);
                    tNewfile.Close();

                    //XX2013/06/18 
                    tNewfile.Dispose();

                    //HttpCookie Cookie1 = new HttpCookie("PersonalSearch_ID");
                    //Cookie1.Value = TPMPermissions._strEncood(ID);
                    //Response.Cookies.Add(Cookie1);

                    string Script;
                    Script = "<Script>";
                    Script += "window.opener.location.href='Personal1-1.aspx?XCOUNT="+Request["XCOUNT"].ToString()+"&TPM_FION=" + Request.QueryString["TPM_FION"] + "';";
                    Script += "window.close();";
                    Script = Script + "</Script>";
                    ClientScript.RegisterStartupScript(this.GetType(), "", Script);
                   
                }
                catch (Exception)
                {

                    throw;
                }
            }
            else
            {

            }
        }
    }
}
