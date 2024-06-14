using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TPPDDB._3_forleave
{
    public partial class C_CARDSET_GROUPUPDATE : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //by MQ ------------------------------20100331            
                C.set_Panel_EnterToTAB(ref this.Panel1);
                DropDownList_MZ_EXAD.DataBind();
                DropDownList_MZ_EXAD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
            }
        }

        protected void btOk_Click(object sender, EventArgs e)
        {
            try
            {
                string DeleteString = "DELETE FROM C_CARDSET WHERE MZ_ID IN (SELECT MZ_ID FROM A_DLBASE WHERE MZ_EXAD='" + DropDownList_MZ_EXAD.SelectedValue + "' AND MZ_EXUNIT='" + DropDownList_MZ_EXUNIT.SelectedValue + "')";
                o_DBFactory.ABC_toTest.Edit_Data(DeleteString);
                string CLOCK = CheckBox_MZ_CLOCK.Checked ? "Y" : "N";
                string OPENDOOR = CheckBox_MZ_OPENDOOR.Checked ? "Y" : "N";
                string OVERTIME = CheckBox_MZ_OVERTIME.Checked ? "Y" : "N";
                string INSERTSTRING = "INSERT INTO C_CARDSET SELECT MZ_ID,'" + CLOCK + "','" + OPENDOOR + "','" + OVERTIME + "' FROM A_DLBASE WHERE MZ_EXAD='" + DropDownList_MZ_EXAD.SelectedValue + "' AND MZ_EXUNIT='" + DropDownList_MZ_EXUNIT.SelectedValue + "'";
                o_DBFactory.ABC_toTest.Edit_Data(INSERTSTRING);
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('整批修改成功');window.close();", true);
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('整批修改失敗');", true);
            }
        }

       

        protected void btLeave_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.close();", true);
        }
    }
}
