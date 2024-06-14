using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using TPPDDB.Helpers;

namespace TPPDDB._3_forleave
{
    //TODO SKY 正式上線時刪除
    public partial class C_OvertimeInsideTotal_rpt_New : C_BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //一般權限檢查
            C.check_power();
            //取得群組權限，人事系統取得 MZ_POWER
            _strGID = o_a_Function.strGID(Session["ADPMZ_ID"].ToStringNullSafe());
            //取得模組
            _TPM_FION = Request.QueryString["TPM_FION"].ToStringNullSafe();

            if (!IsPostBack)
            {
                C.set_Panel_EnterToTAB(ref this.Panel1);

                C.fill_AD_POST(DropDownList_AD);
                DropDownList_AD.SelectedValue = Session["ADPMZ_EXAD"].ToString();

                C.fill_unit(DropDownList_UNIT, DropDownList_AD.SelectedValue);
                DropDownList_UNIT.SelectedValue = Session["ADPMZ_EXUNIT"].ToString();

                chk_TPMGroup();
            }
        }

        protected void ChangeDropDownList_AD()
        {
            string strSQL = "SELECT * FROM Z_CODE_ON WHERE NEW='" + Session["ADPMZ_EXAD"].ToString() + "'";
            DataTable tempDT = new DataTable();
            tempDT = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");

            //SqlDataSource_AD.SelectCommand = "SELECT  MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE = '04' AND (MZ_KCODE='" + tempDT.Rows[0][0].ToString() + "' OR MZ_KCODE='" + tempDT.Rows[0][1].ToString() + "')";
            string SQL = "";
            //如果是中和分局進來改成只抓中和一&中和二&中和 matthew
            if (Session["ADPMZ_EXAD"].ToString() == "382133600C")
            {
                SQL = "SELECT  MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE = '04' AND (MZ_KCODE in ('382133400C','382133500C','382133600C'))";
            }
            else
            {
                SQL = "SELECT  MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE = '04' AND (MZ_KCODE='" + tempDT.Rows[0][0].ToString() + "' OR MZ_KCODE='" + tempDT.Rows[0][1].ToString() + "')";

            }
            DataTable dt = o_DBFactory.ABC_toTest.Create_Table(SQL, "GET");
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
                    DropDownList_UNIT.SelectedValue = Session["ADPMZ_EXUNIT"].ToString();
                    break;
                case "C":
                    ChangeDropDownList_AD();
                    //DropDownList_AD.Enabled = false;
                    break;
                case "D":
                    ChangeDropDownList_AD();
                    // DropDownList_AD.Enabled = false;
                    DropDownList_UNIT.Enabled = false;
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
            }
        }

        protected void btPrint_Click(object sender, EventArgs e)
        {
            Session["TITLE"] = DropDownList_AD.SelectedItem.Text;
            Session["TITLE2"] = TextBox_MZ_DATE.Text.SubstringOutToEmpty(0, 3) + "年" + TextBox_MZ_DATE.Text.SubstringOutToEmpty(3, 2) + "月";
            Session["RPT_C_DATE"] = TextBox_MZ_DATE.Text;
            string tmp_url = string.Format("C_rpt.aspx?fn=OvertimeInsideTotal_New&TPM_FION={0}&AD={1}&UNIT={2}", _TPM_FION, DropDownList_AD.SelectedValue, DropDownList_UNIT.SelectedValue);

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

        /// <summary>每小時薪資</summary>
        protected string Hour_Pay(string MZ_ID)
        {
            _2_salary.Police Police = new TPPDDB._2_salary.Police(MZ_ID);

            if (Police.occc.Substring(0, 2) == "Z0")
                return MathHelper.Round(Convert.ToDouble(Police.salary + Police.profess + Police.boss) / 240 * 1.33).ToString();
            else
                return MathHelper.Round(Convert.ToDouble(Police.salary + Police.profess + Police.boss) / 240).ToString();
        }
    }
}