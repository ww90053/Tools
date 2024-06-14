using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TPPDDB._2_salary
{
    public partial class _0_B_BankListManager : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_Save_Click(object sender, EventArgs e)
        {
            string strSQL;

            if (HasSameBank(txt_ID.Text))
                strSQL = string.Format("UPDATE B_BANK_LIST SET BANK_NAME = '{1}' WHERE BANK_ID = '{0}'", txt_ID.Text, txt_Name.Text);
            else
                strSQL = string.Format("INSERT INTO B_BANK_LIST( BANK_ID, BANK_NAME) VALUES( '{0}', '{1}')", txt_ID.Text, txt_Name.Text);

            o_DBFactory.ABC_toTest.Edit_Data(strSQL);
        }

        protected void btn_Update_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string strSQL;

            strSQL = string.Format("SELECT BANK_NAME FROM B_BANK_LIST WHERE BANK_ID = '{0}'", btn.ID);
            txt_Name.Text = o_DBFactory.ABC_toTest.vExecSQL(strSQL);

            btnCreate_ModalPopupExtender.Show();
        }

        protected void ListView1_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            Label lbl = (Label)e.Item.FindControl("BANK_IDLabel");
            Panel pl = (Panel)e.Item.FindControl("pl_Btn");
            Button btn = new Button();

            btn.ID = lbl.Text;
            btn.Text = "修改";
            btn.Click += new EventHandler(btn_Update_Click);

            pl.Controls.Add(btn);
        }

        protected void btn_Search_Click(object sender, EventArgs e)
        {
            DoSearch();
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            DoSearch();
        }

        private bool HasSameBank(string strID)
        {
            string strSQL;

            strSQL = string.Format("SELECT COUNT(*) FROM B_BANK_LIST WHERE BANK_ID = '{0}'", strID);

            if (o_DBFactory.ABC_toTest.vExecSQL(strSQL) != "0")
                return true;

            return false;
        }

        private void DoSearch()
        {
            DataTable dt = new DataTable();
            string strSQL;

            strSQL = "SELECT BANK_ID, BANK_NAME FROM B_BANK_LIST WHERE 1=1";

            if (txt_ID.Text.Length > 0)
                strSQL += string.Format(" BANK_ID LIKE '%{0}%'", txt_ID.Text);

            if (txt_Name.Text.Length > 0)
                strSQL += string.Format(" BANK_NAME LIKE '%{0}%')", txt_Name.Text);

            dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "BankList");

            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
    }
}
