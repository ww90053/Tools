using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TPPDDB._1_personnel
{
    public partial class Personal_Gradedetail_rpt : System.Web.UI.Page
    {
        int TPM_FION=0;
        

        DataTable dt;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                A.check_power();                
              A.set_Panel_EnterToTAB(ref this.Panel1);
          

             // TextBox_MZ_YEAR.Text = (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0');
                TextBox_MZ_ID.Text = Session["ADPMZ_ID"].ToString();
                
                 ///群組權限
                ViewState ["A_strGID"] = o_a_Function.strGID(Session["ADPMZ_ID"].ToString());

                
                chk_TPMGroup();
               
            }
                    
           
        }

        /// <summary>
        /// 群組權限
        /// </summary>
        protected void chk_TPMGroup()
        {
            switch (ViewState["A_strGID"].ToString() )
            {
                case "A":
                case "B":
                case "C":
                case "E":
                    break;
                case "D":
                    TextBox_MZ_NAME.ReadOnly = true;
                    TextBox_MZ_ID.ReadOnly = true;
                    break;
                
            }
        }

        protected void GetValue()
        {
            string SelectstringPart = "";

            switch (ViewState ["A_strGID"].ToString() )
            {
                case "A":
                case "B":
                case "D":
                    break;
                case "C":
                    SelectstringPart = "(AND MZ_AD='" + Session["ADPMZ_EXAD"].ToString() + "' OR MZ_EXAD='" + Session["ADPMZ_EXAD"].ToString() + "' OR PAY_AD='" + Session["ADPMZ_EXAD"].ToString() + "')";
                    break;
                

                case "E":
                    SelectstringPart = "AND MZ_EXAD='" + Session["ADPMZ_EXAD"].ToString() + "' AND MZ_EXUNIT='" + Session["ADPMZ_EXUNIT"].ToString() + "'";
                    break;
            }

            string Selectstring = "  SELECT MZ_NAME,MZ_ID,(SELECT RTRIM(MZ_KCHI) FROM A_KTYPE WHERE RTRIM(MZ_KTYPE)='04' AND RTRIM(MZ_KCODE)=RTRIM(A.MZ_EXAD)) AS MZ_EXAD," +
                                     " (SELECT RTRIM(MZ_KCHI) FROM A_KTYPE WHERE RTRIM(MZ_KTYPE) = '25' AND RTRIM(MZ_KCODE)=RTRIM(A.MZ_EXUNIT) ) AS MZ_EXUNIT, " +
                                     " (SELECT RTRIM(MZ_KCHI) FROM A_KTYPE WHERE RTRIM(MZ_KTYPE)='26' AND RTRIM(MZ_KCODE)=RTRIM(A.MZ_OCCC)) AS MZ_OCCC FROM A_DLBASE A " +
                                     " WHERE 1=1";

            if (!string.IsNullOrEmpty(TextBox_MZ_NAME.Text))
            {
                Selectstring += " AND MZ_NAME='" + TextBox_MZ_NAME.Text.Trim() + "'";
            }

            if (!string.IsNullOrEmpty(TextBox_MZ_ID.Text))
            {
                Selectstring += " AND MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "'";
            }

            Selectstring += SelectstringPart;

            dt = new DataTable();

            dt = o_DBFactory.ABC_toTest.Create_Table(Selectstring, "123");

            Session["gradedetail_DT"] = dt;

            if (dt.Rows.Count > 1)

                GridView1.DataSource = dt;

            GridView1.AllowPaging = true;

            GridView1.PageSize = 10;

            GridView1.DataBind();

            GridView1.Visible = true;
        }

        protected void GD1SHOW()
        {
            string COUNT_NAME = "";

            switch (ViewState["A_strGID"].ToString() )
            {
                case "A":                  
                case "B":
                    COUNT_NAME = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_DLBASE WHERE MZ_NAME LIKE '" + TextBox_MZ_NAME.Text.Trim() + "%'");
                    break;
                case "C":
                    COUNT_NAME = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_DLBASE WHERE MZ_NAME LIKE '" + TextBox_MZ_NAME.Text.Trim() + "%' AND MZ_AD='" + Session["ADPMZ_EXAD"].ToString() + "'");
                    break;
                case "D":

                    break;
                case "E":
                    COUNT_NAME = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_DLBASE WHERE MZ_NAME LIKE '" + TextBox_MZ_NAME.Text.Trim() + "%' AND MZ_EXAD='" + Session["ADPMZ_EXAD"].ToString() + "' AND MZ_EXUNIT='" + Session["ADPMZ_EXUNIT"].ToString() + "'");
                    break;
            }

            if (string.IsNullOrEmpty(COUNT_NAME))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('無資料');", true);
            }
            else if (int.Parse(COUNT_NAME) > 1)
            {
                GetValue();
            }
        }

        protected void Print(string MZ_ID, string MZ_NAME)
        {
                         

            Session["RPT_A_IDNO"] = MZ_ID.Trim();


            string tmp_url = "A_rpt.aspx?fn=gradedetail&TPM_FION=" + TPM_FION +
                "&MZ_NAME=" + HttpUtility.UrlEncode(MZ_NAME.Trim())  +
                "&MZ_IDATE1=" + TextBox_MZ_IDATE.Text + "&MZ_IDATE2=" + TextBox_MZ_IDATE1.Text;

            ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);

        }

        protected void btPrint_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(TextBox_MZ_NAME.Text.Trim()) && string.IsNullOrEmpty(TextBox_MZ_ID.Text))
                GD1SHOW();

            if (GridView1.Visible)
            {
            }
            else
                Print(o_str.tosql(TextBox_MZ_ID.Text), o_str.tosql(TextBox_MZ_NAME.Text));
        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            TextBox_MZ_IDATE.Text = string.Empty;
            TextBox_MZ_IDATE1.Text = string.Empty;
            TextBox_MZ_ID.Text = string.Empty;
            TextBox_MZ_NAME.Text = string.Empty;
        }

        protected void TextBox_MZ_NAME_TextChanged(object sender, EventArgs e)
        {
            GD1SHOW();
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView1.SelectedIndex = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "Select")
            {
                Print(GridView1.SelectedRow.Cells[1].Text, GridView1.SelectedRow.Cells[0].Text);
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dt = Session["gradedetail_DT"] as DataTable;

            GridView1.PageIndex = e.NewPageIndex;
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
    }
}
