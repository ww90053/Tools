using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TPPDDB._3_forleave
{
    public partial class C_ForLeaveOvertime_Inside_Overtime_Search : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                switch (o_a_Function.strGID(Session["ADPMZ_ID"].ToString()))
                {
                    case "A":
                        ChangeDropDownList_AD();
                        break;
                    case "B":
                        ChangeDropDownList_AD();
                        //DropDownList_MZ_EXAD.DataBind();
                        DropDownList_MZ_EXAD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                        ChangeUnit();
                        break;
                    case "C":
                        ChangeDropDownList_AD();
                        //DropDownList_MZ_EXAD.DataBind();
                        DropDownList_MZ_EXAD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                        DropDownList_MZ_EXAD.Enabled = false;
                        //如果是中和分局進來改成只抓中和一&中和二&中和 matthew
                        //if (Session["ADPMZ_EXAD"].ToString() != "382133600C")
                        //{
                        //    DropDownList_MZ_EXAD.Enabled = false;
                        //}
                        ChangeUnit();
                        break;
                    case "D":
                        ChangeDropDownList_AD();
                        DropDownList_MZ_EXAD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                        DropDownList_MZ_EXAD.Enabled = false;
                        //如果是中和分局進來改成只抓中和一&中和二&中和 matthew
                        //if (Session["ADPMZ_EXAD"].ToString() != "382133600C")
                        //{
                        //    DropDownList_MZ_EXAD.Enabled = false;
                        //}
                        ChangeUnit();
                        DropDownList_MZ_EXUNIT.Text = Session["ADPMZ_EXUNIT"].ToString();
                        DropDownList_MZ_EXUNIT.Enabled = false;
                        break;
                    case "E":
                        ChangeDropDownList_AD();
                        //DropDownList_MZ_EXAD.DataBind();
                        DropDownList_MZ_EXAD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                        DropDownList_MZ_EXAD.Enabled = false;
                        ////如果是中和分局進來改成只抓中和一&中和二&中和 matthew
                        //if (Session["ADPMZ_EXAD"].ToString() != "382133600C")
                        //{
                        //    DropDownList_MZ_EXAD.Enabled = false;
                        //}
                        ChangeUnit();
                        DropDownList_MZ_EXUNIT.Text = Session["ADPMZ_EXUNIT"].ToString();
                        DropDownList_MZ_EXUNIT.Enabled = false;
                        break;
                }
                //switch (o_a_Function.strGID(Session["ADPMZ_ID"].ToString()))
                //{
                //    case "A":
                //        break;
                //    case "B":
                //        DropDownList_AD.DataBind();
                //        DropDownList_AD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                //        break;
                //    case "C":
                //        DropDownList_AD.DataBind();
                //        DropDownList_AD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                //        DropDownList_AD.Enabled = false;
                //        break;
                //    case "D":
                //    case "E":
                //        DropDownList_AD.DataBind();
                //        DropDownList_AD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                //        DropDownList_AD.Enabled = false;
                //        DropDownList_UNIT.DataBind();
                //        DropDownList_UNIT.SelectedValue = Session["ADPMZ_EXUNIT"].ToString();
                //        DropDownList_UNIT.Enabled = false;
                //        break;
                //}
            }
        }
        //matthew 中和分局
        protected void ChangeDropDownList_AD()
        {
            string strSQL = "SELECT * FROM Z_CODE_ON WHERE NEW='" + Session["ADPMZ_EXAD"].ToString() + "'";
            DataTable tempDT = new DataTable();
            tempDT = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
            //如果是中和分局進來改成只抓中和一&中和二&中和 matthew
            if (Session["ADPMZ_EXAD"].ToString() == "382133600C")
            {
                strSQL = "SELECT  MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE = '04' AND (MZ_KCODE in ('382133400C','382133500C','382133600C'))";
            }
            else
            {
                strSQL = "SELECT RTRIM(MZ_KCHI) AS MZ_KCHI, RTRIM(MZ_KCODE) AS MZ_KCODE FROM A_KTYPE WHERE (RTRIM(MZ_KTYPE) = '04') AND (RTRIM(MZ_KCODE) LIKE '38213%')";
            }
            DataTable dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");
            DropDownList_MZ_EXAD.DataSource = dt;
            DropDownList_MZ_EXAD.DataTextField = "MZ_KCHI";
            DropDownList_MZ_EXAD.DataValueField = "MZ_KCODE";
            DropDownList_MZ_EXAD.DataBind();

            DropDownList_MZ_EXAD.SelectedValue = Session["ADPMZ_EXAD"].ToString();

        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.close();", true);
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.opener.location.href='C_ForLeaveOvertime_Inside_Overtime.aspx?AD=" + DropDownList_AD.SelectedValue + "&UNIT=" + DropDownList_UNIT.SelectedValue + "&SEARCH_MONTH=" + TextBox1.Text.Trim().PadLeft(5, '0') + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "';window.close();", true);
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.opener.location.href='C_ForLeaveOvertime_Inside_Overtime.aspx?AD=" + DropDownList_MZ_EXAD.SelectedValue + "&UNIT=" + DropDownList_MZ_EXUNIT.SelectedValue + "&SEARCH_MONTH=" + TextBox1.Text.Trim().PadLeft(5, '0') + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "';window.close();", true);

        }
        /// <summary>下拉 : 現服機關變更</summary>
        protected void DropDownList_MZ_EXAD_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeUnit();
        }
        /// <summary>變更單位</summary>
        private void ChangeUnit()
        {
            DataTable temp = new DataTable();
            string strSQL = "SELECT RTRIM(MZ_KCHI),RTRIM(MZ_KCODE) FROM A_KTYPE WHERE MZ_KTYPE='25' AND MZ_KCODE IN (SELECT MZ_UNIT FROM A_UNIT_AD WHERE MZ_AD='" + DropDownList_MZ_EXAD.SelectedValue + "')";
            temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");
            DropDownList_MZ_EXUNIT.DataSource = temp;
            DropDownList_MZ_EXUNIT.DataTextField = "RTRIM(MZ_KCHI)";
            DropDownList_MZ_EXUNIT.DataValueField = "RTRIM(MZ_KCODE)";
            DropDownList_MZ_EXUNIT.DataBind();
            DropDownList_MZ_EXUNIT.Items.Insert(0, "");
            //如果是中和分局進來改成只抓中和一&中和二&中和 matthew
            if (Session["ADPMZ_EXAD"].ToString() == "382133600C")
            {
                DropDownList_MZ_EXUNIT.Text = Session["ADPMZ_EXUNIT"].ToString();
            }
        }
    }
}
