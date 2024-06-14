using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TPPDDB._1_personnel
{
    public partial class Personal_Efficiency1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                A.check_power();
           
                if (Session["Personal_Effiency1_MZ_AD"] != null)
                {
                    ViewState["SelectCommand"] = "SELECT A_REV_BASE.*,dbo.to_number(CASE WHEN MZ_TBDV IS NULL THEN '999' WHEN MZ_TBDV='Z99' THEN '999' ELSE MZ_TBDV END) AS TBDV FROM A_REV_BASE WHERE MZ_SWT!='1' AND MZ_YEAR='" + Session["Personal_Effiency1_MZ_YEAR"].ToString() + "'";

                    if (Session["Personal_Effiency1_MZ_AD"].ToString() != string.Empty)
                    {
                        ViewState["SelectCommand"] += " AND MZ_AD='" + Session["Personal_Effiency1_MZ_AD"].ToString() + "'";
                    }
                    
                    if (Session["Personal_Effiency1_MZ_UNIT"].ToString() != string.Empty)
                    {
                        if (Session["Personal_Effiency1_MZ_AD"].ToString() == "382130100C" || Session["Personal_Effiency1_MZ_AD"].ToString() == "382130200C" || Session["Personal_Effiency1_MZ_AD"].ToString() == "382130300C")
                        {
                            ViewState["SelectCommand"] += " AND MZ_UNIT='" + Session["Personal_Effiency1_MZ_UNIT"].ToString() + "'";
                        }
                        else
                        {
                            ViewState["SelectCommand"] += " AND MZ_EXUNIT='" + Session["Personal_Effiency1_MZ_UNIT"].ToString() + "'";
                        }
                    }

                    if (Session["Personal_Effiency1_MZ_AD"].ToString() == "382130100C" || Session["Personal_Effiency1_MZ_AD"].ToString() == "382130200C" || Session["Personal_Effiency1_MZ_AD"].ToString() == "382130300C")
                    {
                        ViewState["SelectCommand"] += " ORDER BY MZ_AD,MZ_UNIT,TBDV,MZ_OCCC,MZ_NUM";
                    }
                    else
                    {
                        ViewState["SelectCommand"] += " ORDER BY MZ_AD,MZ_EXUNIT,TBDV,MZ_OCCC,MZ_NUM";
                    }
                }
                else
                {
                    ViewState["SelectCommand"] = "SELECT * FROM A_REV_BASE WHERE 1<>1 ";
                }

                A.set_Panel_EnterToTAB(ref this.Panel1);
            }

            SqlDataSource1.SelectCommand = ViewState["SelectCommand"].ToString();
        }

        protected void btSearch_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('Personal_Efficiency_reach.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "','查詢','top=190,left=200,width=420,height=250,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }

        protected void TextBox_POINT_TextChanged(object sender, EventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((TextBox)sender).NamingContainer;
            GridView gv = (GridView)gvr.NamingContainer;
            int index = gvr.RowIndex;

            TextBox tbPOINT = gv.Rows[index].Cells[3].Controls[1] as TextBox;

            string GRADE;

            if (!string.IsNullOrEmpty(tbPOINT.Text.Trim()) && int.Parse(tbPOINT.Text.Trim()) != 0)
            {
                if (int.Parse(tbPOINT.Text.Trim()) >= 80)
                {
                    GRADE = "甲";
                }
                else if (int.Parse(tbPOINT.Text.Trim()) < 80 && int.Parse(tbPOINT.Text.Trim()) >= 70)
                {
                    GRADE = "乙";
                }
                else if (int.Parse(tbPOINT.Text.Trim()) < 70 && int.Parse(tbPOINT.Text.Trim()) >= 60)
                {
                    GRADE = "丙";
                }
                else
                {
                    GRADE = "丁";
                }

                string UpdateString = "UPDATE A_REV_BASE SET MZ_NUM='" + tbPOINT.Text + "',MZ_GRADE='" + GRADE + "' WHERE MZ_ID='" + gv.Rows[index].Cells[0].Text + "' AND MZ_YEAR='" + Session["Personal_Effiency1_MZ_YEAR"].ToString() + "'";

                o_DBFactory.ABC_toTest.Edit_Data(UpdateString);
            }
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

        protected void GridView1_DataBound(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();

            dt = o_DBFactory.ABC_toTest.Create_Table(ViewState["SelectCommand"].ToString(), "GET");

            if (dt.Rows.Count == 0)
            {

            }
            else
            {
                PageSet(GridView1);
            }
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                GridView1.SelectedIndex = Convert.ToInt32(e.CommandArgument);

                TextBox_MZ_YEAR.Text = Session["Personal_Effiency1_MZ_YEAR"].ToString();
                TextBox_MZ_NAME.Text = GridView1.SelectedRow.Cells[1].Text;
                TextBox_MZ_OCCC.Text = o_A_DLBASE.OCCC(GridView1.SelectedRow.Cells[0].Text);
                TextBox_MZ_SRANK.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='09' AND MZ_KCODE=(SELECT MZ_SRANK FROM A_DLBASE WHERE MZ_ID='" + GridView1.SelectedRow.Cells[0].Text + "')");
                TextBox_MZ_AD.Text = o_A_DLBASE.CUNIT(GridView1.SelectedRow.Cells[0].Text);

                //(GridView1.SelectedRow.Cells[3].Controls[1] as TextBox).Focus();
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
            //{
            //    e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GridView1, "Select$" + e.Row.RowIndex);
            //    e.Row.Cells[14].Attributes.Add("Style", "display:none");
            //}
        }

        protected void DropDownList_MZ_SWT_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((DropDownList)sender).NamingContainer;
            GridView gv = (GridView)gvr.NamingContainer;
            int index = gvr.RowIndex;

            DropDownList dl_SWT = gv.Rows[index].Cells[13].Controls[1] as DropDownList;

            string UpdateString = "UPDATE A_REV_BASE SET MZ_SWT='" + dl_SWT.SelectedValue + "' WHERE MZ_ID='" + gv.Rows[index].Cells[0].Text + "' AND MZ_YEAR='" + Session["Personal_Effiency1_MZ_YEAR"].ToString() + "'";

            o_DBFactory.ABC_toTest.Edit_Data(UpdateString);
        }
    }
}
