using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TPPDDB._2_Salary.UserControl
{
    public partial class SalaryCode : System.Web.UI.UserControl
    {
        public string TableName
        {
            get
            {
                if (ViewState["TableName"] == null)
                    return "";

                return ViewState["TableName"].ToString();
            }
            set { ViewState["TableName"] = value; }
        }
        private string Mode
        {
            get
            {
                if (ViewState["Mode"] == null)
                    return "";

                return ViewState["Mode"].ToString();
            }
            set { ViewState["Mode"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                DoSearch();
        }

        protected void gv_Data_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Upd":
                    txt_ID1.Enabled = false;
                    txt_ID1.Text = e.CommandArgument.ToString();
                    txt_Name1.Text = GetName(e.CommandArgument.ToString());
                    txt_Pay1.Text = GetPay(e.CommandArgument.ToString());

                    Mode = "Update";

                    popup_ModalPopupExtender.Show();
                    break;

                case "Del":
                    DeleteData(e.CommandArgument.ToString());
                    break;
            }
        }

        protected void txt_Create_Click(object sender, EventArgs e)
        {
            txt_ID1.Enabled = true;
            txt_ID1.Text = "";
            txt_Name1.Text = "";
            txt_Pay1.Text = "";

            Mode = "Create";

            popup_ModalPopupExtender.Show();
        }

        protected void btn_Save_Click(object sender, EventArgs e)
        {
            if (txt_ID1.Text.Length == 0)
                return;
            if (txt_Name1.Text.Length == 0)
                return;
            if (txt_Pay1.Text.Length == 0)
                return;

            string strSQL;

            strSQL = "";

            switch (Mode)
            {
                case "Create":
                    strSQL = string.Format("INSERT INTO {0}(ID, NAME, PAY) VALUES('{1}', '{2}', {3})", TableName, txt_ID1.Text, txt_Name1.Text, txt_Pay1.Text);
                    break;

                case "Update":
                    strSQL = string.Format("UPDATE {0} SET NAME='{2}', PAY={3} WHERE ID='{1}'", TableName, txt_ID1.Text, txt_Name1.Text, txt_Pay1.Text);
                    break;
            }

            o_DBFactory.ABC_toTest.Edit_Data(strSQL);

            DoSearch();
        }

        protected void gv_Data_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DoSearch();

            gv_Data.PageIndex = e.NewPageIndex;
        }

        private void DoSearch()
        {
            string strSQL;
            DataTable dt = new DataTable();

            strSQL = string.Format("SELECT ID, NAME, PAY FROM {0} ORDER BY ID", TableName);
            dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "TAG");

            gv_Data.DataSource = dt;
            gv_Data.DataBind();
        }

        private string GetName(string ID)
        {
            string strSQL;

            strSQL = string.Format("SELECT NAME FROM {0} WHERE ID='{1}'", TableName, ID);

            return o_DBFactory.ABC_toTest.vExecSQL(strSQL);
        }

        private string GetPay(string ID)
        {
            string strSQL;

            strSQL = string.Format("SELECT PAY FROM {0} WHERE ID='{1}'", TableName, ID);

            return o_DBFactory.ABC_toTest.vExecSQL(strSQL);
        }

        private void DeleteData(string ID)
        {
            string strSQL;

            strSQL = string.Format("DELETE {0} WHERE ID='{1}'", TableName, ID);

            o_DBFactory.ABC_toTest.Edit_Data(strSQL);
            DoSearch();
        }
    }
}