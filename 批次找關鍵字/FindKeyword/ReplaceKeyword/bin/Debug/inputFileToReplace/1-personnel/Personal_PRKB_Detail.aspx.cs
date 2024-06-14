using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;

namespace TPPDDB._1_personnel
{
    public partial class Personal_PRKB_Detail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ViewState["MZ_NO"] = Request["NO"];
            ViewState["MZ_AD"] = Request["MZ_AD"];
            ViewState["MZ_UNIT"] = Request["MZ_UNIT"];
            ViewState["MZ_ID"] = Session["PRKB_DETAIL_ID"];
            Session.Remove("PRKB_DETAIL_ID");
            ViewState["MZ_NAME"] = Session["PRKB_DETAIL_NAME"];
            Session.Remove("PRKB_DETAIL_NAME");

            if (!Page.IsPostBack)
            {
                if (ViewState["MZ_NO"] != null)
                {
                    string strSQL = "SELECT * FROM A_PRKB WHERE 1=1 ";

                    if (ViewState["MZ_ID"].ToString() != "")
                    {
                        strSQL = strSQL + " AND MZ_ID='" + ViewState["MZ_ID"].ToString() + "'";
                    }

                    if (ViewState["MZ_NAME"].ToString() != "")
                    {
                        strSQL = strSQL + " AND MZ_NAME='" + ViewState["MZ_NAME"].ToString() + "'";
                    }

                    if (ViewState["MZ_AD"].ToString() != "" && ViewState["MZ_UNIT"].ToString() == "")
                    {
                        strSQL = strSQL + " AND (MZ_AD='" + ViewState["MZ_AD"].ToString() + "' OR MZ_EXAD='" + ViewState["MZ_AD"].ToString() + "')";
                    }
                    else if (ViewState["MZ_AD"].ToString() != "" && ViewState["MZ_UNIT"].ToString() != "" && ViewState["cbAD"] != null && ViewState["cbUNIT"] != null)
                    {
                        strSQL = strSQL + " AND ((MZ_EXAD ='" + ViewState["MZ_AD"].ToString() + "' AND MZ_EXUNIT='" + ViewState["MZ_UNIT"].ToString() + "') OR (MZ_AD ='" + ViewState["cbAD"].ToString() + "' AND MZ_UNIT='" + ViewState["cbUNIT"].ToString() + "'))";
                    }
                    else if (ViewState["MZ_AD"].ToString() != "" && ViewState["MZ_UNIT"].ToString() != "")
                    {
                        strSQL = strSQL + " AND ((MZ_EXAD ='" + ViewState["MZ_AD"].ToString() + "' AND MZ_EXUNIT='" + ViewState["MZ_UNIT"].ToString() + "'))";
                    }

                    if (ViewState["MZ_NO"].ToString() != "")
                    {
                        strSQL = strSQL + " AND MZ_NO='" + ViewState["MZ_NO"].ToString() + "'";
                    }

                    strSQL = strSQL + " ORDER BY MZ_NO,MZ_AD,MZ_UNIT,MZ_ID";

                    DataTable dt = new DataTable();

                    dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");

                    // GridView1.DataSource = dt;

                    TBGridView1.DataSource = dt;

                    // GridView1.DataBind();

                    TBGridView1.DataBind();

                }
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            //避免「型別 xxx 的控制項 xxx 必須置於有 runat=server 的表單標記之中。」的問題
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.ClearContent();
            Response.Write("<meta http-equiv=Content-Type content=text/html;charset=utf-8>");
            Response.Write("<style>");
            Response.Write("td{mso-number-format:\"\\@\";}"); //將所有欄位格式改為"文字"
            Response.Write("</style>");
            string filename = DateTime.Now.Year.ToString() + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + ".xls";
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + Page.Server.UrlEncode(filename));
            Response.ContentType = "application/excel";
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
            TBGridView1.RenderControl(objHtmlTextWriter);
            Response.Write(objStringWriter.ToString());
            Response.End();
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            Response.Write(@"<script language=javascript>window.close();</script>");
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.Header)
            {
                e.Row.Cells[4].Text = o_A_KTYPE.RAD(e.Row.Cells[4].Text);
                e.Row.Cells[5].Text = o_A_KTYPE.RUNIT(e.Row.Cells[5].Text);
                e.Row.Cells[6].Text = o_A_KTYPE.RAD(e.Row.Cells[6].Text);
                e.Row.Cells[7].Text = o_A_KTYPE.RUNIT(e.Row.Cells[7].Text);
                e.Row.Cells[8].Text = o_A_KTYPE.CODE_TO_NAME(e.Row.Cells[8].Text, "26");
                e.Row.Cells[9].Text = o_A_KTYPE.CODE_TO_NAME(e.Row.Cells[9].Text, "09");
                e.Row.Cells[10].Text = o_A_KTYPE.CODE_TO_NAME(e.Row.Cells[10].Text, "09");
                e.Row.Cells[11].Text = o_A_KTYPE.CODE_TO_NAME(e.Row.Cells[11].Text, "09");
                e.Row.Cells[12].Text = o_A_KTYPE.CODE_TO_NAME(e.Row.Cells[12].Text, "43");
                e.Row.Cells[14].Text = o_A_KTYPE.CODE_TO_NAME(e.Row.Cells[14].Text, "22");
                e.Row.Cells[15].Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT STATIC_NAME FROM A_STATIC WHERE STATIC_NO='" + e.Row.Cells[15].Text + "'");
                e.Row.Cells[16].Text = o_A_KTYPE.CODE_TO_NAME(e.Row.Cells[16].Text, "24");
                e.Row.Cells[17].Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_PRONAME  FROM A_PROLNO WHERE MZ_PROLNO='" + e.Row.Cells[17].Text + "'");
                if (e.Row.Cells[19].Text == "9")
                    e.Row.Cells[19].Text = "不配分";
                else
                    e.Row.Cells[19].Text = "配分";
                e.Row.Cells[20].Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_PNAME FROM A_POLNUM WHERE MZ_POLNO='" + e.Row.Cells[20].Text + "'");
                switch (e.Row.Cells[22].Text)
                {
                    case "1":
                        e.Row.Cells[22].Text = "警察局權責";
                        break;
                    case "2":
                        e.Row.Cells[22].Text = "警分局權責";
                        break;
                    case "3":
                        e.Row.Cells[22].Text = "調他機關";
                        break;
                    case "4":
                        e.Row.Cells[22].Text = "陳報警政署";
                        break;
                    case "":
                        e.Row.Cells[22].Text = string.Empty;
                        break;
                }
                if (e.Row.Cells[23].Text == "Y")
                    e.Row.Cells[24].Text = "是";
                else
                    e.Row.Cells[23].Text = "否";
                if (e.Row.Cells[24].Text == "Y")
                    e.Row.Cells[24].Text = "是";
                else
                    e.Row.Cells[24].Text = "否";
            }
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                GridView1.SelectedIndex = Convert.ToInt32(e.CommandArgument);

                Session["Personal_PRKB_Detail_ID"] = GridView1.SelectedRow.Cells[2].Text;

                Session["Personal_PRKB_Detail_PRCT"] = GridView1.SelectedRow.Cells[13].Text;

                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.opener.location.href='Personal2-1.aspx?Detail_NO=" + GridView1.SelectedRow.Cells[1].Text + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "';", true);
            }
        }

        protected void TBGridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.Header)
            {
                e.Row.Cells[3].Text = o_A_KTYPE.RAD(e.Row.Cells[3].Text);
                e.Row.Cells[4].Text = o_A_KTYPE.RUNIT(e.Row.Cells[4].Text);
                e.Row.Cells[5].Text = o_A_KTYPE.RAD(e.Row.Cells[5].Text);
                e.Row.Cells[6].Text = o_A_KTYPE.RUNIT(e.Row.Cells[6].Text);
                e.Row.Cells[7].Text = o_A_KTYPE.CODE_TO_NAME(e.Row.Cells[7].Text, "26");
                e.Row.Cells[8].Text = o_A_KTYPE.CODE_TO_NAME(e.Row.Cells[8].Text, "09");
                e.Row.Cells[9].Text = o_A_KTYPE.CODE_TO_NAME(e.Row.Cells[9].Text, "09");
                e.Row.Cells[10].Text = o_A_KTYPE.CODE_TO_NAME(e.Row.Cells[10].Text, "09");
                e.Row.Cells[11].Text = o_A_KTYPE.CODE_TO_NAME(e.Row.Cells[11].Text, "43");
                e.Row.Cells[13].Text = o_A_KTYPE.CODE_TO_NAME(e.Row.Cells[13].Text, "22");
                e.Row.Cells[14].Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT STATIC_NAME FROM A_STATIC WHERE STATIC_NO='" + e.Row.Cells[14].Text + "'");
                e.Row.Cells[15].Text = o_A_KTYPE.CODE_TO_NAME(e.Row.Cells[15].Text, "24");
                e.Row.Cells[16].Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_PRONAME  FROM A_PROLNO WHERE MZ_PROLNO='" + e.Row.Cells[16].Text + "'");
                if (e.Row.Cells[18].Text == "9")
                    e.Row.Cells[18].Text = "不配分";
                else
                    e.Row.Cells[18].Text = "配分";
                e.Row.Cells[19].Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_PNAME FROM A_POLNUM WHERE MZ_POLNO='" + e.Row.Cells[19].Text + "'");
                switch (e.Row.Cells[21].Text)
                {
                    case "1":
                        e.Row.Cells[21].Text = "警察局權責";
                        break;
                    case "2":
                        e.Row.Cells[21].Text = "警分局權責";
                        break;
                    case "3":
                        e.Row.Cells[21].Text = "調他機關";
                        break;
                    case "4":
                        e.Row.Cells[21].Text = "陳報警政署";
                        break;
                    case "":
                        e.Row.Cells[21].Text = string.Empty;
                        break;
                }
                if (e.Row.Cells[22].Text == "Y")
                    e.Row.Cells[23].Text = "是";
                else
                    e.Row.Cells[22].Text = "否";
                if (e.Row.Cells[23].Text == "Y")
                    e.Row.Cells[23].Text = "是";
                else
                    e.Row.Cells[23].Text = "否";
            }
        }
    }
}
