using System;
using System.Data;
using System.Linq;
using System.Web;
using TPPDDB.Helpers;
using System.Text;

namespace TPPDDB._1_personnel
{
    public partial class A_excel_rpt : System.Web.UI.Page
    {
        DataTable rpt_dt = new DataTable();
        //private Microsoft.Office.Interop.Word.Document wordDoc = new Microsoft.Office.Interop.Word.Document();
        //private object oPageBreak = Microsoft.Office.Interop.Word.WdBreakType.wdPageBreak;

        protected void Page_Load(object sender, EventArgs e)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Charset = "";
            HttpContext.Current.Response.ContentType = "application/msword";
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachments;filename=" + HttpContext.Current.Server.UrlEncode("調他機關.doc"));

            //撈資料
            rpt_dt = RPT.punish.A_othergrade.doSearch(Request["MZ_SWT3"], Request["NO"], Request["NO2"]);
            string tmptitle = "",tmpnote = "";
            int pagesize = 19;//一頁顯示幾筆
            int eforcount = 0;
            int aforcount = 0;
            int tmp = 0;

            #region 計算

            int totalcount = rpt_dt.Rows.Count;
            int mocount = totalcount % pagesize;
            int pacount = totalcount / pagesize;
            if(mocount > 0) { pacount++; }

            #endregion

            for (int j = 1; j <= pacount; j++)
            {
                eforcount = j * pagesize;
                if (j == pacount)
                {
                    eforcount = totalcount;
                }

                if (j > 1)
                {
                    aforcount = ((j - 1) * pagesize);
                }

                #region 組table
                
                tmpnote += "<table border='1' width='100%' cellspacing='0' cellpadding='5'>";

                tmpnote += "<tr align='center'>";
                tmpnote += "<td colspan='6'><b><span style='font-size:19.0pt;font-family:標楷體'>" + string.Format("{0}外調人員獎懲建議名冊", o_A_KTYPE.RAD(Session["ADPMZ_EXAD"].ToString())) + "</span></b></td>";
                tmpnote += "</tr>";

                #region 項目標題

                tmpnote += "<tr align='center'>";
                tmpnote += "<td class='tdclass' width='170'><span style='font-size:12.0pt;font-family:標楷體'>服務機關</span></td>";
                tmpnote += "<td class='tdclass' width='100'><span style='font-size:12.0pt;font-family:標楷體'>職稱</span></td>";
                tmpnote += "<td class='tdclass' width='90'><span style='font-size:12.0pt;font-family:標楷體'>姓名<br>身份證號</span></td>";
                tmpnote += "<td class='tdclass'><span style='font-size:12.0pt;font-family:標楷體'>獎懲事由</span></td>";
                tmpnote += "<td class='tdclass' width='90'><span style='font-size:12.0pt;font-family:標楷體'>擬予獎懲</span></td>";
                tmpnote += "<td class='tdclass'><span style='font-size:12.0pt;font-family:標楷體'>備註</span></td>";
                tmpnote += "</tr>";

                #endregion

                #region 內容

                for (int i = aforcount; i < eforcount; i++)
                {
                    tmp++;
                    tmpnote += "<tr>";
                    tmpnote += "<td align='center'><span style='font-size:9.0pt;font-family:標楷體;font-weight: normal;'>" + rpt_dt.Rows[i]["MZ_AD"] + "<br>" + rpt_dt.Rows[i]["MZ_UNIT"] + "</span></td>";
                    tmpnote += "<td align='center'><span style='font-size:10.0pt;font-family:標楷體;font-weight: normal;'>" + rpt_dt.Rows[i]["MZ_OCCC"] + "<br>" + rpt_dt.Rows[i]["MZ_SRANK"] + "</span></td>";
                    tmpnote += "<td align='center'><span style='font-size:10.0pt;font-family:標楷體;font-weight: normal;'><strong>" + rpt_dt.Rows[i]["MZ_NAME"] + "</strong><br>" + rpt_dt.Rows[i]["MZ_ID"] + "</span></td>";
                    tmpnote += "<td><span style='font-size:9.0pt;font-family:標楷體;font-weight: normal;'>" + rpt_dt.Rows[i]["MZ_PRCT"].ToString().Trim().Replace("&N","") + "</span></td>";
                    tmpnote += "<td align='center'><span style='font-size:9.0pt;font-family:標楷體;font-weight: normal;'>" + rpt_dt.Rows[i]["MZ_PRRST"] + "</span></td>";
                    tmpnote += "<td><span style='font-size:9.0pt;font-family:標楷體;font-weight: normal;'>" + rpt_dt.Rows[i]["MZ_MEMO"] + "</span></td>";
                    tmpnote += "</tr>";
                }

                #endregion

                tmpnote += "</table>";

                #endregion

                #region 換頁
                
                if (j < pacount)
                {
                    tmpnote += "<span lang=EN-US style='font-size:12.0pt;font-family:'新細明體','serif';mso-bidi-font-family: 新細明體;mso-ansi-language:EN-US;mso-fareast-language:ZH-TW;mso-bidi-language:AR-SA'><br clear=all style='mso-special-character:line-break;page-break-before:always; mso - special - character:line -break;-break-before:always'></span>";
                }

                #endregion
            }

            #region 組完整HTML
            
            tmptitle = "<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>";
            tmptitle += "";
            tmptitle += "<html xmlns='http://www.w3.org/1999/xhtml' >";
            tmptitle += "<head runat='server'>";
            tmptitle += "<title></title>";
            tmptitle += "<style type='text/css'>";
            tmptitle += ".tdclass {";
            tmptitle += "text-align: justify;";
            tmptitle += "text-justify: distribute-all-lines;";
            tmptitle += "}";

            tmptitle += "@page{mso-page-border-surround-header:no;mso-page-border-surround-footer:no;}@page Section1{size:595.3pt 841.9pt;margin:1.5cm 0.6cm 1.5cm 0.6cm;mso-header-margin:21.55pt;mso-footer-margin:21.6pt;mso-paper-source:0;layout-grid:18.0pt;}div.Section1{page:Section1;}";

            tmptitle += "</style>";
            tmptitle += "</head>";
            tmptitle += "<body>";
            tmptitle += "<div class='Section1'>";

            tmptitle += tmpnote;

            tmptitle += "</div>";
            tmptitle += "</body>";
            tmptitle += "</html>";

            #endregion

            Literal1.Text = tmptitle;
        }
    }
}