using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace TPPDDB.App_Code
{
    /// <summary>
    /// ExcelDowload 的摘要描述
    /// </summary>
    public class ExcelDowload : IHttpHandler, IReadOnlySessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            using (MemoryStream ms = context.Session["msData"] as MemoryStream)
            {
                string fileName = HttpContext.Current.Server.UrlPathEncode(context.Session["fileName"].ToString());

                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ContentType = "application/octet-stream";
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=\"" + fileName + ".xls\"");
                HttpContext.Current.Response.BinaryWrite(ms.ToArray());
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
                //context.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xls", fileName));
                //fileName = HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8);

                //context.Response.BinaryWrite(ms.ToArray());
                ms.Close();
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}