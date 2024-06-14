using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;

namespace TPPDDB._2_salary
{
    public partial class B_SalaryIncomeTax4 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SalaryPublic.checkPermission();
                SalaryPublic.fillDropDownList(ref this.DropDownList_AD);
            }
        }

        protected void btToTXT_Click(object sender, EventArgs e)
        {
            //匯出txt檔
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=Tax123.txt");//txt檔名
            Response.ContentType = "application/vnd.ms-word";
            Response.Charset = "";
            Response.ContentEncoding = Encoding.GetEncoding(950);

            if (DropDownList_AD.Enabled)
                Response.Write(Tax.makeTaxMedia(new Branch(DropDownList_AD.SelectedValue), txt_year.Text, rblType.SelectedValue));
            else
                Response.Write(Tax.makeTaxMedia(new Branch(SalaryPublic.strLoginEXAD), txt_year.Text, rblType.SelectedValue));
            Response.End();
            return;

        }
    }
}
