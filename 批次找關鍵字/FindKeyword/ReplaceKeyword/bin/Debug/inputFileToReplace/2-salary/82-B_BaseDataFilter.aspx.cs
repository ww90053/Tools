using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TPPDDB._2_salary
{
    public partial class _2_B_BaseDataFilter : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                CustomInit();
        }

        private void CustomInit()
        {
            SalaryPublic.fillDropDownList(ref ddl_PayAD);
            SalaryPublic.AddFirstSelection(ref ddl_PayAD);

            btn_Export.Enabled = false;
        }

        protected void btn_Send_Click(object sender, EventArgs e)
        {
            Search();
        }

        private void Search()
        {
            string strSQL;

            strSQL = "SELECT CHIAD 發薪機關, CHIUNIT 單位, MZ_POLNO 員工編號, CHIOCCC 職稱, MZ_NAME 姓名 FROM VW_ALL_BASE_DATA WHERE 1=1";

            if (ddl_PayAD.SelectedValue != "")
                strSQL += string.Format(" AND PAY_AD='{0}'", ddl_PayAD.SelectedValue);

            if (rbl_Is30.SelectedValue == "是")
                strSQL += " AND IS30YEAR='是'";

            if (rbl_IsBoss.SelectedValue == "1")
                strSQL += " AND ISBOSS='1'";

            strSQL += " AND ROWNUM<=1000 ORDER BY MZ_POLNO";

            ViewState["Result"] = o_DBFactory.ABC_toTest.Create_Table(strSQL, "TAG");

            gv_Result.DataSource = (DataTable)ViewState["Result"];
            gv_Result.DataBind();

            if (((DataTable)ViewState["Result"]).Rows.Count > 0)
                btn_Export.Enabled = true;
        }

        protected void gv_Result_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_Result.PageIndex = e.NewPageIndex;
            gv_Result.DataSource = (DataTable)ViewState["Result"];
            gv_Result.DataBind();
        }

        protected void btn_Export_Click(object sender, EventArgs e)
        {
            GridView gvExport = new GridView();
            string strExportFilename = "ExportedData.xls";

            gvExport.DataSource = (DataTable)ViewState["Result"];
            gvExport.DataBind();

            Response.Clear();
            Response.AddHeader("content-disposition",
                 "attachment;filename=" + strExportFilename + ".xls");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.xls";
            Response.Charset = "big5";

            System.IO.StringWriter stringWrite = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
            gvExport.RenderControl(htmlWrite);
            Response.Write(stringWrite.ToString().Replace("<div>", "").Replace("</div>", ""));
            Response.End();
        }
    }
}
