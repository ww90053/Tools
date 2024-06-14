using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TPPDDB._1_personnel
{
    public partial class Personal_PRK2_UPDATE_SWT2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                A.fill_MZ_PRID(DropDownList_MZ_PRID, 2);
            
            }

            string PRK2_PRID = "";
            if (Request["MZ_PRID"] != null)
            {
                if (!string.IsNullOrEmpty (Request["MZ_PRID"].ToString()))
                PRK2_PRID = HttpUtility.UrlDecode(Request["MZ_PRID"].ToString());
            }
            DropDownList_MZ_PRID.SelectedItem.Text = PRK2_PRID;
            //DropDownList_MZ_PRID.SelectedItem.Text = Session["PRK2_PRID"].ToString();
            TextBox_MZ_PRID1.Text = Request["MZ_PRID1"];
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string UpdateString = "UPDATE A_PRK2 SET MZ_SWT2='" + RadioButtonList2.SelectedValue + "' WHERE MZ_PRID='" + DropDownList_MZ_PRID.SelectedItem.Text + "' AND MZ_PRID1='" + TextBox_MZ_PRID1.Text + "'";

            try
            {
                o_DBFactory.ABC_toTest.Edit_Data(UpdateString);
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('修改成功！');", true);
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('修改失敗！');", true);
            }
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.close()", true);
        }
    }
}
