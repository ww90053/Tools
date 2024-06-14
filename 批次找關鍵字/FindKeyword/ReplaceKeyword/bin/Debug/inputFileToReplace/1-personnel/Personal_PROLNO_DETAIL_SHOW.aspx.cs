using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TPPDDB._1_personnel
{
    public partial class Personal_PROLNO_DETAIL_SHOW : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                GridView1.SelectedIndex = Convert.ToInt32(e.CommandArgument);

                string strSQL = "SELECT * FROM A_PROLNO WHERE MZ_PROLNO='" + GridView1.SelectedRow.Cells[1].Text.Trim() + "'";

                DataTable temp = new DataTable();

                temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");

                string prolno = temp.Rows[0]["MZ_PROLNO"].ToString();
                string proname = temp.Rows[0]["MZ_PRONAME"].ToString();

                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.opener.document.getElementById('" + Session["PROLNO_CID"] + "').value='" + prolno +
                                                                                                   "';window.opener.document.getElementById('" + Session["PROLNO_CID1"] + "').value='" + proname +
                                                                                                   "';window.opener.__doPostBack('" + Session["PROLNO_CID"] + "','');window.close();", true);
            }
        }
    }
}
