using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TPPDDB._2_salary.UserControl
{
    public partial class SoleItemSelector : System.Web.UI.UserControl
    {
        public string item
        {
            get { return txt_item.Text.PadLeft(2, '0'); }
        }

        public bool taxFlag
        {
            get { return SalarySole.boolTAXES_YESNO(txt_item.Text.PadLeft(2, '0')); }
        }

        public string tax_id
        {
            get { return ddl_taxid.SelectedValue; }
        }

        public string tax_id1
        {
            get { return ddl_taxid1.SelectedValue; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void txt_item_TextChanged(object sender, EventArgs e)
        {
            numSelected();
        }

        protected void btn_showNum_Click(object sender, EventArgs e)
        {
            showCode();
            btn_showNum_ModalPopupExtender.Show();
        }

        protected void gv_num_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            txt_item.Text = e.CommandArgument.ToString();
            numSelected();
        }

        protected void gv_num_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_num.PageIndex = e.NewPageIndex;
            showCode();
            btn_showNum_ModalPopupExtender.Show();
        }

        protected void ddl_taxid_SelectedIndexChanged(object sender, EventArgs e)
        {
            Sole.setDDLTaxID1(ref ddl_taxid1, ddl_taxid.SelectedValue);
        }

        // 產生代碼選擇器的資料
        void showCode()
        {
            string sql;

            sql = "SELECT ID, NAME FROM B_SOLEITEM ORDER BY ID";

            gv_num.DataSource = o_DBFactory.ABC_toTest.DataSelect(sql);
            gv_num.DataBind();
        }

        // 項目代碼改變時需要做的處理
        void numSelected()
        {
            lbl_item.Text = Sole.getName(txt_item.Text);
            Sole.setDDLTaxID(ref ddl_taxid, item);
            Sole.setDDLTaxID1(ref ddl_taxid1, ddl_taxid.SelectedValue);
            CheckBox_TAXFLAG.Checked = taxFlag;

            if (itemSelected != null)
                itemSelected();
        }

        public delegate void ItemSelected();
        public event ItemSelected itemSelected;
    }
}