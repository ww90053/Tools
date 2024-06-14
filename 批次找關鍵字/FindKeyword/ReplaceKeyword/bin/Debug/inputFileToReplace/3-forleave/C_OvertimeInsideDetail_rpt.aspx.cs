using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TPPDDB._3_forleave
{
    public partial class C_OvertimeInsideDetail_rpt : System.Web.UI.Page
    {
        int TPM_FION = 0;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                C.check_power();

                //MQ-----------------------20100331
                C.set_Panel_EnterToTAB(ref this.Panel1);


                //C_strGID = o_a_Function.strGID(Session["ADPMZ_ID"].ToString());

                C.fill_AD_POST(DropDownList_AD);
                TextBox_MZ_ID.Text = Session["ADPMZ_ID"].ToString();


                chk_TPMGroup();
            }
        }

        protected void ChangeDropDownList_AD()
        {
            string strSQL = "SELECT * FROM Z_CODE_ON WHERE NEW='" + Session["ADPMZ_EXAD"].ToString() + "'";
            DataTable tempDT = new DataTable();
            tempDT = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
            
            //如果是中和分局進來改成只抓中和一&中和二&中和 matthew
            string SQL = "";
            if (Session["ADPMZ_EXAD"].ToString() == "382133600C")
            {
                SQL = "SELECT  MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE = '04' AND (MZ_KCODE in ('382133400C','382133500C','382133600C'))";
            }
            else
            {
                SQL = "SELECT  MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE = '04' AND (MZ_KCODE='" + tempDT.Rows[0][0].ToString() + "' OR MZ_KCODE='" + tempDT.Rows[0][1].ToString() + "')";
            }
            DataTable dt = o_DBFactory.ABC_toTest.Create_Table(SQL, "GET");
            //SqlDataSource_AD.SelectCommand = "SELECT  MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE = '04' AND (MZ_KCODE='" + tempDT.Rows[0][0].ToString() + "' OR MZ_KCODE='" + tempDT.Rows[0][1].ToString() + "')";
            DropDownList_AD.DataSource = dt;
            DropDownList_AD.DataTextField = "MZ_KCHI";
            DropDownList_AD.DataValueField = "MZ_KCODE";
            DropDownList_AD.DataBind();
            DropDownList_AD.SelectedValue = Session["ADPMZ_EXAD"].ToString();

            C.fill_unit(DropDownList_UNIT, DropDownList_AD.SelectedValue);

            DropDownList_UNIT.SelectedValue = Session["ADPMZ_EXUNIT"].ToString();
        }

        protected void chk_TPMGroup()
        {
            switch (o_a_Function.strGID(Session["ADPMZ_ID"].ToString()))
            {
                case "A":
                case "B":
                    DropDownList_AD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                    C.fill_unit(DropDownList_UNIT, DropDownList_AD.SelectedValue);
                    DropDownList_UNIT.SelectedValue = Session["ADPMZ_EXUNIT"].ToString();
                    break;

                case "D":
                    ChangeDropDownList_AD();

                    // DropDownList_AD.Enabled = false;
                    DropDownList_UNIT.Enabled = false;
                    TextBox_MZ_ID.Text = Session["ADPMZ_ID"].ToString();
                    TextBox_MZ_ID.Enabled = false;
                    break;
                case "E":
                    ChangeDropDownList_AD();

                    //DropDownList_AD.Enabled = false;
                    //如果是中和分局進來改成只抓中和一&中和二&中和 matthew
                    if (Session["ADPMZ_EXAD"].ToString() != "382133600C")
                    {
                        DropDownList_AD.Enabled = false;
                    }
                    DropDownList_UNIT.Enabled = false;
                    break;
                default:
                    ChangeDropDownList_AD();
                    break;

            }


        }

        protected void btPrint_Click(object sender, EventArgs e)
        {


            Session.Remove("RPT_C_IDNO");
            Session["RPT_C_IDNO"] = TextBox_MZ_ID.Text.Trim();

            string tmp_url = "C_rpt.aspx?fn=OvertimeInsideDetail&TPM_FION=" + TPM_FION +
                "&DATE=" + TextBox_MZ_DATE.Text.Trim() +
                "&AD=" + DropDownList_AD.SelectedValue + "&UNIT=" + DropDownList_UNIT.SelectedValue;

            ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            TextBox_MZ_DATE.Text = string.Empty;
        }

        protected void DropDownList_AD_SelectedIndexChanged(object sender, EventArgs e)
        {
            C.fill_unit(DropDownList_UNIT, DropDownList_AD.SelectedValue);
        }
    }
}
