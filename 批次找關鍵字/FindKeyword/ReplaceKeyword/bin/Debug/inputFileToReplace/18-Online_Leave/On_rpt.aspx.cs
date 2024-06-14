using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace TPPDDB._18_Online_Leave
{
    public partial class On_rpt : System.Web.UI.Page
    {
       
        DataTable temp = new DataTable();
        //DataTable besup;
        //DataTable print_dt;
        CrystalDecisions.CrystalReports.Engine.ReportDocument report;
        protected void Page_Unload(object sender, EventArgs e)
        {
            report.Close();
            report.Dispose();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            string fkind = "";
            string filename = "";
            
            string filepath = "report/";

            //報表登入
            string db_ServerName;
            string db_UserID;
            string db_Password;
            string db_DatabaseName;

            db_ServerName = System.Configuration.ConfigurationManager.AppSettings["servername"];
            db_UserID = System.Configuration.ConfigurationManager.AppSettings["userid"];
            db_Password = System.Configuration.ConfigurationManager.AppSettings["pass"];
            db_DatabaseName = System.Configuration.ConfigurationManager.AppSettings["database"];

            if (Request["fn"] != null)
            {
                fkind = Request["fn"].ToString();
            }

            switch (fkind)
            {
                case "History":
                    filename = "On_History.rpt";
                    break;
            }

            report = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
            report.Load(Server.MapPath(filepath + filename));
            report.SetDatabaseLogon(db_UserID, db_Password, db_ServerName, db_DatabaseName);
            //string filename2 = "";

            switch (fkind)
            {
                case "History"://ONline
                    
                    report.SetDataSource(Session["on_history_rpt"] as DataTable);
                    report.ParameterFields["onRptSn"].CurrentValues.AddValue(Session["onRptSn"].ToString());
                    CrystalReportViewer1.ReportSource = report;
                    break;
            }
        }
    }
}
