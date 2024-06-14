using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TPPDDB.App_Code;

namespace TPPDDB._3_forleave.rpt
{
    public partial class C_ONDUTY_UnitList_rpt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                switch (Request["TYPE"])
                {
                    case "UnitList":

                        lb_Title.Text = "輪值表列印";
                        break;

                    case "UnitList_PAY":

                        lb_Title.Text = "值日費印領清冊";
                        break;
                }
            
            //C.check_power();
                C.fill_AD(ddl_EXAD);
                ddl_EXAD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                ddl_EXAD_SelectedIndexChanged(null, null);
                ddl_EXUNIT.SelectedValue = Session["ADPMZ_EXUNIT"].ToString();




                switch (o_a_Function.strGID(Session["ADPMZ_ID"].ToString()))
                {
                    case "D" :
                    case "E":
                        //matthew 中和分局進來要可以選中一中二
                        if (Session["ADPMZ_EXAD"].ToString() == "382133600C")
                        {
                            C.fill_DLL_New(ddl_EXAD);
                        }
                        else
                        {
                            ddl_EXAD.Enabled = false;
                        }
                        //ddl_EXAD.Enabled = false;
                        ddl_EXUNIT.Enabled = false;
                        break;
                    case "C":
                        //matthew 中和分局進來要可以選中一中二
                        if (Session["ADPMZ_EXAD"].ToString() == "382133600C")
                        {
                            C.fill_DLL_New(ddl_EXAD);
                        }
                        else
                        {
                            ddl_EXAD.Enabled = false;
                        }
                         //ddl_EXAD.Enabled = false;
                         break;
                
                
                
                }
            }

        }




        protected void ddl_EXAD_SelectedIndexChanged(object sender, EventArgs e)
        {
            C.fill_unit(ddl_EXUNIT, ddl_EXAD.SelectedValue);
        }

        protected void Button_SEARCH_Click(object sender, EventArgs e)
        {
            if (DateManange.Check_date(TextBox_DATE.Text))
            {           
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "alert('請檢查輸入的起迄日期');", true);
                return;
            }

            string tmp_url="";

            switch (Request["TYPE"])
            {
                case "UnitList":

                tmp_url = "../C_rpt.aspx?fn=ONDUTY_LIST&EXAD=" + ddl_EXAD.SelectedValue + "&EXUNIT=" + ddl_EXUNIT.SelectedValue 
                    + "&DATE=" + TextBox_DATE.Text + "&TPM_FION=" + 0;
                break;

                case "UnitList_PAY":

                tmp_url = "../C_rpt.aspx?fn=ONDUTY_PAY_LIST&EXAD=" + ddl_EXAD.SelectedValue + "&EXUNIT=" + ddl_EXUNIT.SelectedValue 
                    + "&DATE=" + TextBox_DATE.Text + "&TPM_FION=" + 0;
                break;
            }


            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
        }
        protected void Button_SEARCH_New_Click(object sender, EventArgs e)
        {
            if (DateManange.Check_date(TextBox_DATE.Text))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "alert('請檢查輸入的起迄日期');", true);
                return;
            }

            string tmp_url = "";

            switch (Request["TYPE"])
            {
                case "UnitList":
                    tmp_url = "../C_rpt.aspx?fn=ONDUTY_LIST_NEW&EXAD=" + ddl_EXAD.SelectedValue + "&EXUNIT=" + ddl_EXUNIT.SelectedValue
                    + "&DATE=" + TextBox_DATE.Text + "&TPM_FION=" + 0;
                    break;

                case "UnitList_PAY":
                    tmp_url = "../C_rpt.aspx?fn=ONDUTY_PAY_LIST_NEW&EXAD=" + ddl_EXAD.SelectedValue + "&EXUNIT=" + ddl_EXUNIT.SelectedValue
                    + "&DATE=" + TextBox_DATE.Text + "&TPM_FION=" + 0;
                    break;
            }
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "go_print('" + tmp_url + "');", true);


        }
    }
}
