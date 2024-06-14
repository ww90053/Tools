using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Data;
using AjaxControlToolkit;
using Microsoft.VisualBasic;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
namespace TPPDDB._1_personnel
{
    public partial class Personal2_5 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                A.check_power();              
                A.set_Panel_EnterToTAB(ref Panel1);

            }
           
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            //避免「型別 xxx 的控制項 xxx 必須置於有 runat=server 的表單標記之中。」的問題
        }

        protected void btOK_Click(object sender, EventArgs e)
        {
            string strSQL = "SELECT MZ_NAME,MZ_ID,MZ_PRK,MZ_PRRST,MZ_IDATE,MZ_CHKAD,'" +
                            TextBox_MZ_PRPASS.Text.Trim() + "' AS MZ_CODE,MZ_PRK1,dbo.SUBSTR(MZ_PROLNO,1,2) AS MZ_PROLNO1," +
                            "dbo.SUBSTR(MZ_PROLNO,3,2) AS MZ_PROLNO2,dbo.SUBSTR(MZ_PROLNO,5,2) AS MZ_PROLNO3,MZ_PCODE,MZ_POLK,MZ_PRCT,MZ_PRID+'字第'+MZ_PRID1+'號' AS MZ_PRID" +
                            " FROM A_PRK2 WHERE MZ_DATE>='" + TextBox_MZ_DATE1.Text.Trim().Replace("/", "").PadLeft(7, '0') +
                            "' AND MZ_DATE<='" + TextBox_MZ_DATE2.Text.Trim().Replace("/", "").PadLeft(7, '0') + "' AND (MZ_SWT!='Y' OR MZ_SWT IS NULL)";

            DataTable Exceldt = new DataTable();
            
           
            Exceldt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");

            //轉全形
            for (int i = 0; i < Exceldt.Rows.Count; i++)
            {
                string pcode = Exceldt.Rows[i]["MZ_PCODE"].ToString();
                //if (pcode == "1")
                //    Exceldt.Rows[i]["MZ_PCODE"] = "配分";
                //else if(pcode == "9")
                //    Exceldt.Rows[i]["MZ_PCODE"] = "不配分";
                ////


                Exceldt.Rows[i]["MZ_NAME"] = Strings.StrConv(Exceldt.Rows[i]["MZ_NAME"].ToString(), VbStrConv.Wide, 2052).PadRight(6, '　');//20140402 為解決罕見字 所以設定2052
                Exceldt.Rows[i]["MZ_PRCT"] = Strings.StrConv(Exceldt.Rows[i]["MZ_PRCT"].ToString(), VbStrConv.Wide, 0).PadRight(50, '　');
                Exceldt.Rows[i]["MZ_PRID"] = Strings.StrConv(Exceldt.Rows[i]["MZ_PRID"].ToString(), VbStrConv.Wide, 0).PadRight(40, '　');
            }
            if (Exceldt.Rows.Count > 0)
            {
                string UpdateString = "UPDATE A_PRK2 SET MZ_PRPASS='" + TextBox_MZ_PRPASS.Text.Trim() + "',MZ_SWT='Y' " +
                                     " WHERE MZ_DATE>='" + TextBox_MZ_DATE1.Text.Trim().Replace("/", "").PadLeft(7, '0') +
                                     "'  AND MZ_DATE<='" + TextBox_MZ_DATE2.Text.Trim().Replace("/", "").PadLeft(7, '0') + "'";

                try
                {
                    o_DBFactory.ABC_toTest.Edit_Data(UpdateString);

                    TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00003", TPMPermissions._boolTPM(), UpdateString);


                }
                catch
                {

                }
            }
            
            
            ExcelTable.DataSource = Exceldt;
            
            

            ExcelTable.AllowPaging = false;
            ExcelTable.AllowSorting = false;
            ExcelTable.EnableViewState = false;
            ExcelTable.DataBind();


            Response.ClearContent();
            Response.Write("<meta http-equiv=Content-Type content=text/html;charset=utf-8>");
            Response.Write("<style>");
            Response.Write("td{mso-number-format:\"\\@\";}"); //將所有欄位格式改為"文字"
            Response.Write("</style>");
            string filename = DateTime.Now.Year.ToString() + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + ".xls";
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + Page.Server.UrlEncode(filename));
            Response.ContentType = "application/excel";
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
            ExcelTable.RenderControl(objHtmlTextWriter);
            Response.Write(objStringWriter.ToString());
            Response.End();
        }




        //public static string CheckHasUnicodeWord(string vstrWord)
        //{
        //    StringBuilder strbResult = new StringBuilder();
        //    int i = 0;
        //    System.Text.Encoding encBig5 = System.Text.Encoding.GetEncoding(950);
        //    System.Text.Encoding encUtf8 = System.Text.Encoding.UTF8;
        //    int iBig5ByteCnt = 0;
        //    int iUtf8ByteCnt = 0;
        //    for (i = 0; i < vstrWord.Length; i++)
        //    {
        //        iBig5ByteCnt = encBig5.GetByteCount(vstrWord.Substring(i, 1));
        //        iUtf8ByteCnt = encUtf8.GetByteCount(vstrWord.Substring(i, 1));
        //        if ((iBig5ByteCnt == 1 && iUtf8ByteCnt == 3) || (iBig5ByteCnt == 1 && iUtf8ByteCnt == 2))
        //        {
        //            strbResult.Append(vstrWord.Substring(i, 1));
        //        }
        //    }
        //    return strbResult.ToString();
        //}
    }
}
