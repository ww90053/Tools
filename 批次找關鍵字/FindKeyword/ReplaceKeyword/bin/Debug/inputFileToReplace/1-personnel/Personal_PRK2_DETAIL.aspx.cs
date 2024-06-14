using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TPPDDB._1_personnel
{
    public partial class Personal_PRK2_DETAIL : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ViewState["MZ_PRID"] = Request["MZ_PRID"];
            ViewState["MZ_PRID1"] = Request["MZ_PRID1"];

            if (!Page.IsPostBack)
            {
                if (Session["23SQL"] != null)
                {
                    string strSQL = string.Empty;

                    strSQL = Session["23SQL"].ToString();

                    DataTable dt = new DataTable();

                    dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");

                    GridView1.DataSource = dt;

                    GridView1.DataBind();
                }
            }
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                GridView1.SelectedIndex = Convert.ToInt32(e.CommandArgument);

                int s = GridView1.SelectedIndex;

                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.opener.location.href='Personal2-3.aspx?XCOUNT=" + s + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "';", true);
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.Header)
            {
                //e.Row.Cells[3].Text = o_A_KTYPE.RAD(e.Row.Cells[3].Text);
                //e.Row.Cells[4].Text = o_A_KTYPE.RUNIT(e.Row.Cells[4].Text);
                //e.Row.Cells[5].Text = o_A_KTYPE.RAD(e.Row.Cells[5].Text);
                //e.Row.Cells[6].Text = o_A_KTYPE.RUNIT(e.Row.Cells[6].Text);
                //e.Row.Cells[7].Text = o_A_KTYPE.CODE_TO_NAME(e.Row.Cells[7].Text, "26");
                //e.Row.Cells[8].Text = o_A_KTYPE.CODE_TO_NAME(e.Row.Cells[8].Text, "09");
                //e.Row.Cells[9].Text = o_A_KTYPE.CODE_TO_NAME(e.Row.Cells[9].Text, "09");
                //e.Row.Cells[10].Text = o_A_KTYPE.CODE_TO_NAME(e.Row.Cells[10].Text, "09");
                //e.Row.Cells[11].Text = o_A_KTYPE.CODE_TO_NAME(e.Row.Cells[11].Text, "43");
                //e.Row.Cells[13].Text = o_A_KTYPE.CODE_TO_NAME(e.Row.Cells[13].Text, "22");
                //e.Row.Cells[14].Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT STATIC_NAME FROM A_STATIC WHERE STATIC_NO='" + e.Row.Cells[14].Text + "'");
                //e.Row.Cells[15].Text = o_A_KTYPE.CODE_TO_NAME(e.Row.Cells[15].Text, "24");
                //e.Row.Cells[16].Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_PRONAME  FROM A_PROLNO WHERE MZ_PROLNO='" + e.Row.Cells[16].Text + "'");
                //if (e.Row.Cells[18].Text == "9")
                //    e.Row.Cells[18].Text = "不配分";
                //else
                //    e.Row.Cells[18].Text = "配分";
                //e.Row.Cells[19].Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_PNAME FROM A_POLNUM WHERE MZ_POLNO='" + e.Row.Cells[19].Text + "'");
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            Response.Write(@"<script language=javascript>window.close();</script>");
        }
    }
}
