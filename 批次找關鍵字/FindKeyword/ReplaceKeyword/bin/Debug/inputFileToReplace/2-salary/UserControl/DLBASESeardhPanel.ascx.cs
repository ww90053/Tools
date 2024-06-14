using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TPPDDB._2_salary.UserControl
{
    public partial class DLBASESeardhPanel : System.Web.UI.UserControl
    {
        string sql = "SELECT * FROM A_KTYPE WHERE 1=1";
        string sort = " ORDER BY MZ_KCODE";
        public string selectedCode;

        public event EventHandler CodeSelectd;
        public event EventHandler SelectdPageChanged;
        public event EventHandler SearchClicked;

        public string ktype
        {
            get
            {
                if (ViewState["ktype"] == null)
                    return "04";
                return ViewState["ktype"].ToString();
            }
            set { ViewState["ktype"] = value; }
        }

        public string condition
        {
            get
            {
                if (ViewState["condition"] == null)
                    return "";
                return ViewState["condition"].ToString();
            }
            set { ViewState["condition"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void gv_Result_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_Result.PageIndex = e.NewPageIndex;
            Search();

            if (SelectdPageChanged != null)
                SelectdPageChanged(sender, e);
        }

        protected void btn_Search_Click(object sender, EventArgs e)
        {
            Search();

            if (SearchClicked != null)
                SearchClicked(sender, e);
        }

        protected void gv_Result_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "btnSelect")
            {
                selectedCode = e.CommandArgument.ToString();

                if (CodeSelectd != null)
                    CodeSelectd(sender, e);
            }
        }

        public void Search()
        {
            string strSQL = sql + string.Format(" AND MZ_KTYPE='{0}'", ktype) + condition;

            if (TextBox_MZ_KCHI.Text != "")
                strSQL += " AND MZ_KCHI LIKE '%" + TextBox_MZ_KCHI.Text.Trim() + "%'";
            if (TextBox_MZ_KCODE.Text != "")
                strSQL += " AND MZ_KCODE LIKE '" + TextBox_MZ_KCODE.Text.Trim() + "%'";
            strSQL += sort;

            gv_Result.DataSource = o_DBFactory.ABC_toTest.Create_Table(strSQL, "TAG");
            gv_Result.DataBind();
        }
    }
}