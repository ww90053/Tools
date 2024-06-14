using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TPPDDB.Helpers;

namespace TPPDDB._1_personnel
{
    public partial class Personal_Salary_Search : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ///群組權限
            string strGID = o_a_Function.strGID(Session["ADPMZ_ID"].ToString());

            if (!IsPostBack)
            {
                this.ddlStatus2.Items.Add(new ListItem("是", "Y"));
                this.ddlStatus2.Items.Add(new ListItem("否", "N"));
            }

            switch (strGID)
            {
                case "A":
                case "B":
                    this.DropDownList_MZ_EXAD.Enabled = true;
                    break;
                default:
                    this.DropDownList_MZ_EXAD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                    this.DropDownList_MZ_EXAD.Enabled = false;
                    break;
            }

        }

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            PersonalService svc = new PersonalService();

            dt = svc.GetSalaryMedata(new P_SalaryMetaData_Model()
            {
                AD = this.DropDownList_MZ_EXAD.SelectedValue,
                Status2 = this.ddlStatus2.SelectedValue
            });

            DataTable dtCloned = dt.Clone();

            foreach (DataColumn col in dtCloned.Columns)
            {
                col.DataType = typeof(string);
            }

            foreach (DataRow row in dt.Rows)
            {
                dtCloned.ImportRow(row);
            }


            // sam 匯出 薪資介接資料  第一行欄位名稱 還有全儲存格式要為文字
            new OfficeHelpers.ExcelHelpers().DtToExcelForXLSSalary(dtCloned, "薪資介接資料", true);

        }
    }
}