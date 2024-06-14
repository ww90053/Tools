using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TPPDDB._2_salary.rpt
{
    public partial class B_monthpay_list_rpt : System.Web.UI.Page
    {
        #region 屬性
        private String strAD
        {
            get
            {
                return DropDownList_AD.SelectedValue;
            }
        }

        private string strAYEAR
        {
            get
            {
                return TextBox_YEAR.Text.PadLeft(3, '0');
            }
        }

        private string strMONTH
        {
            get
            {
                return DropDownList_MONTH.SelectedValue;
            }
        }
        #endregion 屬性

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                SalaryPublic.checkPermission();
                SalaryPublic.fillDropDownList(ref this.DropDownList_AD);
                TextBox_YEAR.Text = SalaryPublic.strRepublicDate().Substring(0, 3);
                DropDownList_MONTH.SelectedValue = System.DateTime.Now.Month.ToString();
                switch (Request["TYPE"].ToString())
                {
                    case "PAY":
                        lb_Title.Text = "每月薪資總表(應發)";
                        break;
                    case "TAKE_OFF":
                        lb_Title.Text = "每月薪資總表(應扣)";
                        break;
                    case "REPAIR_PAY":
                        lb_Title.Text = "補發薪資總表(應發)";
                        tr_batch.Visible = true;
                        break;
                    case "REPAIR_TAKE_OFF":
                        lb_Title.Text = "補發薪資總表(應扣)";
                        tr_batch.Visible = true;
                        break;

                    case "PAY2":
                        lb_Title.Text = "每月薪資總表(應發)New";
                        break;
                    case "TAKE_OFF2":
                        lb_Title.Text = "每月薪資總表(應扣)New";
                        break;

                    //應政府新預算編制分開職稱列印 20180410 by sky
                    case "REPAIR_PAY2":
                        lb_Title.Text = "補發薪資總表(應發)New";
                        tr_batch.Visible = true;
                        break;
                    case "REPAIR_TAKE_OFF2":
                        lb_Title.Text = "補發薪資總表(應扣)New";
                        tr_batch.Visible = true;
                        break;
                }

            }
        }



        protected void Button_SEARCH_Click(object sender, EventArgs e)
        {
            string tmp_url = "";
            switch (Request["TYPE"].ToString())
            {
                case "PAY":
                    tmp_url = "../B_rpt.aspx?fn=monthpay_list" + "&YEAR=" + strAYEAR + "&MONTH=" + strMONTH + "&PAY_AD=" + strAD;
                    break;
                case "TAKE_OFF":
                    tmp_url = "../B_rpt.aspx?fn=monthpaytakeoff_list" + "&YEAR=" + strAYEAR + "&MONTH=" + strMONTH + "&PAY_AD=" + strAD;
                    break;

                case "REPAIR_PAY":
                    tmp_url = "../B_rpt.aspx?fn=repairpay_list" + "&YEAR=" + strAYEAR + "&MONTH=" + strMONTH + "&PAY_AD=" + strAD + "&BATCH_NUMBER=" + txt_batch.Text;
                    break;
                case "REPAIR_TAKE_OFF":
                    tmp_url = "../B_rpt.aspx?fn=repairtakeoff_list" + "&YEAR=" + strAYEAR + "&MONTH=" + strMONTH + "&PAY_AD=" + strAD + "&BATCH_NUMBER=" + txt_batch.Text;
                    break;

                case "PAY2":
                    tmp_url = "../B_rpt.aspx?fn=monthpay_list2" + "&YEAR=" + strAYEAR + "&MONTH=" + strMONTH + "&PAY_AD=" + strAD;
                    break;

                case "TAKE_OFF2":
                    tmp_url = "../B_rpt.aspx?fn=monthpaytakeoff_list2" + "&YEAR=" + strAYEAR + "&MONTH=" + strMONTH + "&PAY_AD=" + strAD;
                    break;

                //應政府新預算編制分開職稱列印 20180410 by sky
                case "REPAIR_PAY2":
                    tmp_url = "../B_rpt.aspx?fn=repairpay_list2" + "&YEAR=" + strAYEAR + "&MONTH=" + strMONTH + "&PAY_AD=" + strAD + "&BATCH_NUMBER=" + txt_batch.Text;
                    break;
                case "REPAIR_TAKE_OFF2":
                    tmp_url = "../B_rpt.aspx?fn=repairtakeoff_list2" + "&YEAR=" + strAYEAR + "&MONTH=" + strMONTH + "&PAY_AD=" + strAD + "&BATCH_NUMBER=" + txt_batch.Text;
                    break;
            }

           
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
        }



        protected void Button_CANCEL_Click(object sender, EventArgs e)
        {
            Response.Redirect("B_monthpay_list_rpt.aspx?TPM_FION=" + Request["TPM_FION"] + "&TYPE=" + Request["TYPE"]);
        }
    }
}
