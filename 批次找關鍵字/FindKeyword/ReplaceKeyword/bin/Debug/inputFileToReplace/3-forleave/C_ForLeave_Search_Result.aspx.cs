using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TPPDDB._3_forleave
{
    public partial class C_ForLeave_Search_Result : System.Web.UI.Page
    {
        

        protected void Page_Load(object sender, EventArgs e)
        {
            //C_strGID = o_a_Function.strGID(Session["ADPMZ_ID"].ToString());

            ViewState["MZ_NAME"] = Session["DLTB01_NAME"];
            Session.Remove("DLTB01_NAME");

            ViewState["MZ_ID"] = Session["DLTB01_ID"];
            Session.Remove("DLTB01_ID");

            ViewState["MZ_EXAD"] = Request["AD"];//查詢機關

            ViewState["MZ_EXUNIT"] = Request["UNIT"];//查詢單位

            ViewState["MZ_IDATE1"] = Request["IDATE1"];

            if (!Page.IsPostBack)
            {
                ViewState["C_strGID"] = o_a_Function.strGID(Session["ADPMZ_ID"].ToString());
                if (ViewState["MZ_ID"] != null)
                {
                    string strSQL = "SELECT C_DLTB01.MZ_CHK1,C_DLTB01.MZ_ID,C_DLTB01.MZ_NAME,C_DLTB01.MZ_IDATE1,C_DLTB01.MZ_ODATE,C_DLTB01.MZ_ITIME1,C_DLTB01.MZ_OTIME,C_DLTB01.MZ_CODE,C_DLTB01.MZ_TDAY,C_DLTB01.MZ_TTIME FROM C_DLTB01,A_DLBASE WHERE C_DLTB01.MZ_ID=A_DLBASE.MZ_ID";

                    if (ViewState["MZ_ID"].ToString() != "")
                    {
                        strSQL += " AND C_DLTB01.MZ_ID='" + ViewState["MZ_ID"].ToString().Trim().ToUpper() + "'";
                    }

                    if (ViewState["MZ_NAME"].ToString() != "")
                    {
                        strSQL += " AND C_DLTB01.MZ_NAME='" + ViewState["MZ_NAME"].ToString().Trim() + "'";
                    }

                    if (ViewState["MZ_EXAD"].ToString() != "" && ViewState["MZ_EXUNIT"].ToString() != "")
                    {
                        strSQL += " AND C_DLTB01.MZ_EXAD ='" + ViewState["MZ_EXAD"].ToString() + "' AND C_DLTB01.MZ_EXUNIT='" + ViewState["MZ_EXUNIT"].ToString() + "'";
                    }
                    else if (ViewState["MZ_EXAD"].ToString() != "")
                    {
                        strSQL += " AND (C_DLTB01.MZ_EXAD='" + ViewState["MZ_EXAD"].ToString() + "' OR A_DLBASE.MZ_AD='" + ViewState["MZ_EXAD"].ToString() + "' OR A_DLBASE.PAY_AD='" + ViewState["MZ_EXAD"].ToString() + "')";
                    }

                    if (ViewState["MZ_IDATE1"].ToString() != "")
                    {
                        strSQL += " AND C_DLTB01.MZ_IDATE1='" + ViewState["MZ_IDATE1"].ToString().Trim() + "'";
                    }
                    else
                    {
                        strSQL += " AND dbo.SUBSTR(MZ_IDATE1,1,3)>='" + (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0') + "'";
                    }

                    strSQL += " ORDER BY MZ_CHK1,MZ_IDATE1 DESC,C_DLTB01.MZ_EXUNIT,C_DLTB01.MZ_ID";

                    DataTable dt = new DataTable();

                    dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GETVALUE");

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

                //2021/03/26 C_ForLeaveBasic.aspx 內容 少幾個功能
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.opener.location.href='C_ForLeaveBasic_New.aspx?XCOUNT=" + s + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "';", true);
                //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.opener.location.href='C_ForLeaveBasic.aspx?XCOUNT=" + s + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "';", true);
            }
        }

        protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((CheckBox)sender).NamingContainer;
            GridView gv = (GridView)gvr.NamingContainer;

            int rowCount = gv.Rows.Count;
            int index = gvr.RowIndex;

            CheckBox CHK1 = (CheckBox)gv.Rows[index].Cells[1].Controls[1];

            string updateString = "";

            if (CHK1.Checked)
            {
                updateString = "UPDATE C_DLTB01 SET MZ_CHK1='Y' WHERE MZ_ID='" + gv.Rows[index].Cells[2].Text + "' AND MZ_IDATE1='" + gv.Rows[index].Cells[5].Text + "' AND MZ_ITIME1='" + gv.Rows[index].Cells[7].Text + "'";
            }
            else
            {
                updateString = "UPDATE C_DLTB01 SET MZ_CHK1='N' WHERE MZ_ID='" + gv.Rows[index].Cells[2].Text + "' AND MZ_IDATE1='" + gv.Rows[index].Cells[5].Text + "' AND MZ_ITIME1='" + gv.Rows[index].Cells[7].Text + "'";
            }

            o_DBFactory.ABC_toTest.Edit_Data(updateString);
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[4].Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_CNAME FROM C_DLCODE WHERE MZ_CODE='" + e.Row.Cells[4].Text + "'");
            }

            if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
            {
                if (ViewState["C_strGID"].ToString() == "D" || ViewState["C_strGID"].ToString() == "E")
                {
                    e.Row.Cells[1].Attributes.Add("style", "display:none");
                }
            }
        }
    }
}
