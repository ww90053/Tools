using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace TPPDDB._1_personnel
{
    public partial class PersonalSearchPRK1 : System.Web.UI.Page
    {
        DataTable dt;

        protected void Page_Load(object sender, EventArgs e)
        {
            A.set_Panel_EnterToTAB(ref this.Panel1);

            ViewState["TableName"] = Request["TableName"];

            if (Request["TableName"].ToString() == "PER_CHECK")
            {
                Panel2.Visible = true;
            }
        }

        protected void GetValue()
        {
           
            string Selectstring = @"SELECT MZ_NAME,MZ_ID, AKD.MZ_KCHI MZ_EXAD ,  AKU.MZ_KCHI MZ_EXUNIT ,  AKO.MZ_KCHI MZ_OCCC 
                                    FROM A_DLBASE A 
                                    LEFT JOIN A_KTYPE AKD ON RTRIM(AKD.MZ_KCODE)=RTRIM(A.MZ_EXAD) AND RTRIM(AKD.MZ_KTYPE)='04' 
                                    LEFT JOIN A_KTYPE AKU ON RTRIM(AKU.MZ_KCODE)=RTRIM(A.MZ_EXUNIT) AND RTRIM(AKU.MZ_KTYPE)='25' 
                                    LEFT JOIN A_KTYPE AKO ON RTRIM(AKO.MZ_KCODE)=RTRIM(A.MZ_OCCC) AND RTRIM(AKO.MZ_KTYPE)='26' 
                                    WHERE 1=1";
            if (!string.IsNullOrEmpty(TextBox_NAME.Text))
            {
                Selectstring += " AND RTRIM(MZ_NAME) LIKE '" + TextBox_NAME.Text.Trim() + "%'";
            }

            if (!string.IsNullOrEmpty(DropDownList_MZ_EXAD.SelectedValue))
            {
                Selectstring += " AND MZ_EXAD='" + DropDownList_MZ_EXAD.SelectedValue + "'";
            }

            if (!string.IsNullOrEmpty(DropDownList_MZ_EXUNIT.SelectedValue))
            {
                Selectstring += " AND MZ_EXUNIT='" + DropDownList_MZ_EXUNIT.SelectedValue + "'";
            }

            if (!string.IsNullOrEmpty(TextBox_MZ_RETRIEVE.Text))
            {
                Selectstring += " AND MZ_RETRIEVE LIKE '" + TextBox_MZ_RETRIEVE.Text.Trim() + "%'";
            }

            dt = new DataTable();
            dt = o_DBFactory.ABC_toTest.Create_Table(Selectstring, "123");
            Session["PersonalSearchIDwithNAMECOUNT"] = dt;

        }

        protected void ChangeUnit()
        {
            DataTable temp = new DataTable();
            string strSQL = "SELECT RTRIM(MZ_KCHI),RTRIM(MZ_KCODE) FROM A_KTYPE WHERE MZ_KTYPE='25' AND MZ_KCODE IN (SELECT MZ_UNIT FROM A_UNIT_AD WHERE MZ_AD='" + DropDownList_MZ_EXAD.SelectedValue + "')";
            temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");
            DropDownList_MZ_EXUNIT.DataSource = temp;
            DropDownList_MZ_EXUNIT.DataTextField = "RTRIM(MZ_KCHI)";
            DropDownList_MZ_EXUNIT.DataValueField = "RTRIM(MZ_KCODE)";
            DropDownList_MZ_EXUNIT.DataBind();
            DropDownList_MZ_EXUNIT.Items.Insert(0, "");
        }

        protected void Button1_Click(object sender, EventArgs e)
        {

            GetValue();

            dt = Session["PersonalSearchIDwithNAMECOUNT"] as DataTable;

            if (dt.Rows.Count == 1)
            {
                if (ViewState["TableName"].ToString() == "PRKB")
                {
                    Session["PersonalSearchIDwithNAME_MZ_ID"] = dt.Rows[0]["MZ_ID"].ToString().Trim();

                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.opener.location.href='Personal2-1.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "';window.close();", true);
                }
                else if (ViewState["TableName"].ToString() == "POSIT")
                {
                    Session["PersonalSearchIDwithNAME_MZ_ID1"] = dt.Rows[0]["MZ_ID"].ToString().Trim();

                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.opener.location.href='Personal3-1.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "';window.close();", true);
                }
                else if (ViewState["TableName"].ToString() == "POSIT2")
                {
                    Session["PersonalSearchIDwithNAME_MZ_ID5"] = dt.Rows[0]["MZ_ID"].ToString().Trim();

                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.opener.location.href='Personal3-3.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "';window.close();", true);
                }
                else if (ViewState["TableName"].ToString() == "PER_CHECK")
                {
                    Session["PersonalSearchIDwithNAME_MZ_ID2"] = dt.Rows[0]["MZ_ID"].ToString().Trim();

                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.opener.location.href='Personal_Check.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "';window.close();", true);
                }
                else if (ViewState["TableName"].ToString() == "TP_PER")
                {
                    Session["PersonalSearchIDwithNAME_MZ_ID3"] = dt.Rows[0]["MZ_ID"].ToString().Trim();

                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.opener.location.href='Personal_PromotionPoint.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "';window.close();", true);
                }
                else if (ViewState["TableName"].ToString() == "CARD")
                {
                    Session["PersonalSearchIDwithNAME_MZ_ID4"] = dt.Rows[0]["MZ_ID"].ToString().Trim();

                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.opener.location.href='Personal_Card.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "';window.close();", true);
                }
            }
            else if (dt.Rows.Count >= 1)
            {
                GridView1.Visible = true;
                GridView1.DataSource = dt;
                GridView1.AllowPaging = true;
                GridView1.PageSize = 10;
                GridView1.DataBind();
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('查無資料')", true);
            }
            //}
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                GridView1.SelectedIndex = int.Parse(e.CommandArgument.ToString());

                if (ViewState["TableName"].ToString() == "PRKB")
                {
                    Session["PersonalSearchIDwithNAME_MZ_ID"] = GridView1.SelectedRow.Cells[2].Text.Trim();

                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.opener.location.href='Personal2-1.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "';window.close();", true);
                }
                else if (ViewState["TableName"].ToString() == "POSIT")
                {
                    Session["PersonalSearchIDwithNAME_MZ_ID1"] = GridView1.SelectedRow.Cells[2].Text.Trim();

                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.opener.location.href='Personal3-1.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "';window.close();", true);
                }
                else if (ViewState["TableName"].ToString() == "POSIT2")
                {
                    Session["PersonalSearchIDwithNAME_MZ_ID5"] = GridView1.SelectedRow.Cells[2].Text.Trim();

                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.opener.location.href='Personal3-3.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "';window.close();", true);
                }
                else if (ViewState["TableName"].ToString() == "PER_CHECK")
                {
                    Session["PersonalSearchIDwithNAME_MZ_ID2"] = GridView1.SelectedRow.Cells[2].Text.Trim();

                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.opener.location.href='Personal_Check.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "';window.close();", true);
                }
                else if (ViewState["TableName"].ToString() == "TP_PER")
                {
                    Session["PersonalSearchIDwithNAME_MZ_ID3"] = GridView1.SelectedRow.Cells[2].Text.Trim();

                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.opener.location.href='Personal_PromotionPoint.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "';window.close();", true);
                }
                else if (ViewState["TableName"].ToString() == "CARD")
                {
                    Session["PersonalSearchIDwithNAME_MZ_ID4"] = GridView1.SelectedRow.Cells[2].Text.Trim();

                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.opener.location.href='Personal_Card.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "';window.close();", true);
                }
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.close();", true);
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dt = Session["PersonalSearchIDwithNAMECOUNT"] as DataTable;

            GridView1.PageIndex = e.NewPageIndex;
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }


        protected void Ktype_Cname_Check(string Cname, TextBox tb1, TextBox tb2, Button bt3)
        {
            tb2.Text = tb2.Text.ToUpper();
            //檢查代碼中文名稱是否存在
            if (string.IsNullOrEmpty(Cname))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('查無資料')", true);
                tb1.Text = string.Empty;
                tb2.Text = string.Empty;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + tb2.ClientID + "').focus();", true);
            }
            else
            {
                tb1.Text = Cname;
                bt3.Focus();

            }
        }

        protected void DropDownList_MZ_EXAD_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeUnit();
        }
    }
}
