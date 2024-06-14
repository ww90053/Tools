using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TPPDDB._1_personnel
{
    public partial class Personal_Search_Result : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ViewState["MZ_NAME"] = Session["Personal_NAME"];
            Session.Remove("MZ_NAME");

            ViewState["MZ_AD"] = Request["AD"];//查詢機關

            ViewState["MZ_UNIT"] = Request["UNIT"];//查詢單位


            if (!IsPostBack)
            {
                if (ViewState["MZ_NAME"] != null)
                {
                    string strSQL = "SELECT MZ_ID,MZ_NAME,dbo.to_number(CASE WHEN MZ_TBDV IS NULL THEN '999' WHEN MZ_TBDV='Z99' THEN '999' ELSE MZ_TBDV END) AS TBDV FROM A_DLBASE  WHERE 1=1";

                    if (ViewState["MZ_NAME"] != null)
                    {
                        if (ViewState["MZ_NAME"].ToString() != "")
                        {
                            strSQL = strSQL + " AND MZ_NAME LIKE '" + ViewState["MZ_NAME"].ToString() + "%'";
                        }
                    }

                    if (ViewState["MZ_AD"].ToString() != "" && ViewState["MZ_UNIT"].ToString() != "")
                    {
                        // sam.hsu 20201215
                        //strSQL += " AND MZ_EXAD ='" + ViewState["MZ_AD"].ToString() + "' OR PAY_AD ='" + ViewState["MZ_AD"].ToString() + "' AND MZ_EXUNIT='" + ViewState["MZ_UNIT"].ToString() + "'";
                        strSQL += " AND MZ_EXAD ='" + ViewState["MZ_AD"].ToString() + "' AND MZ_EXUNIT='" + ViewState["MZ_UNIT"].ToString() + "'";
                    }
                    else if (ViewState["MZ_AD"].ToString() != "")
                    {
                        strSQL += " AND (MZ_EXAD='" + ViewState["MZ_AD"].ToString() + "' OR MZ_AD='" + ViewState["MZ_AD"].ToString() + "' OR PAY_AD='" + ViewState["MZ_AD"].ToString() + "')";
                    }

                    strSQL = strSQL + " AND MZ_STATUS2 = 'Y' ORDER BY TBDV,MZ_PCHIEF,MZ_OCCC";

                    ViewState["SelectCommand"] = strSQL;
                }
            }

            SqlDataSource1.SelectCommand = ViewState["SelectCommand"].ToString();
        }

        protected void ddlPageJump_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlPage = (DropDownList)GridView1.BottomPagerRow.FindControl("ddlPageJump");
            GridView1.PageIndex = ddlPage.SelectedIndex;
        }

        protected void Quick_Click(object sender, EventArgs e)
        {
            int intPageIndex = 0;
            LinkButton lkbtn = (LinkButton)sender;
            switch (lkbtn.Text.Trim())
            {
                case "上一頁":
                    if (GridView1.PageIndex > 1)
                        intPageIndex = GridView1.PageIndex - 1;
                    break;
                case "下一頁":
                    if (GridView1.PageIndex < GridView1.PageCount - 1)
                        intPageIndex = GridView1.PageIndex + 1;
                    break;
                case "最後頁":
                    intPageIndex = GridView1.PageCount - 1;
                    break;
            }
            GridView1.PageIndex = intPageIndex;
        }

        protected void PageSet(object gvQuery)
        {
            GridView gv = (GridView)gvQuery;
            Label lbPage = (Label)gv.BottomPagerRow.FindControl("lbAllPage");
            DropDownList ddlJumpPage = (DropDownList)gv.BottomPagerRow.FindControl("ddlPageJump");

            lbPage.Text = "共" + gv.PageCount.ToString() + "頁";

            for (int i = 1; i <= gv.PageCount; i++)
                ddlJumpPage.Items.Add(new ListItem(i.ToString()));
            if (gv.PageIndex == 0)
            {
                ((LinkButton)gv.BottomPagerRow.FindControl("LinkFirst")).Enabled = false;
                ((LinkButton)gv.BottomPagerRow.FindControl("LinkPrevious")).Enabled = false;
            }
            else if (gv.PageIndex == (gv.PageCount - 1))
            {
                ((LinkButton)gv.BottomPagerRow.FindControl("LinkNext")).Enabled = false;
                ((LinkButton)gv.BottomPagerRow.FindControl("LinkLast")).Enabled = false;
            }

            ddlJumpPage.SelectedIndex = gv.PageIndex;
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                GridView1.SelectedIndex = Convert.ToInt32(e.CommandArgument);

                //HttpCookie Cookie1 = new HttpCookie("Personal_Search_Result_ID");
                //Cookie1.Value = TPMPermissions._strEncood(GridView1.SelectedRow.Cells[1].Text);
                //Response.Cookies.Add(Cookie1);

                Session["Personal_Search_Result_ID"] = GridView1.SelectedRow.Cells[1].Text;

                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.opener.location.href='Personal1-1.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "';", true);
            }
        }

        protected void GridView1_DataBound(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();

            dt = o_DBFactory.ABC_toTest.Create_Table(ViewState["SelectCommand"].ToString(), "GET");

            if (dt.Rows.Count == 0)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('查無資料,請先新增資料');window.close();", true);
            }
            else
            {
                PageSet(GridView1);
            }
        }
    }
}
