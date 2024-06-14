using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TPPDDB._3_forleave
{
    public partial class C_ReviewManagement_GROUPUPDATE : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (!Page.IsPostBack)
            {
                C.set_Panel_EnterToTAB(ref this.Panel1);
                DropDownList_MZ_EXAD.DataBind();
                DropDownList_MZ_EXAD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
            }
        }

        protected void btOk_Click(object sender, EventArgs e)
        {
            try
            {
                string DeleteString = "DELETE FROM C_REVIEW_MANAGEMENT WHERE MZ_ID IN (SELECT MZ_ID FROM A_DLBASE WHERE MZ_EXAD='" + DropDownList_MZ_EXAD.SelectedValue + "' AND MZ_EXUNIT='" + DropDownList_MZ_EXUNIT.SelectedValue + "')";
                o_DBFactory.ABC_toTest.Edit_Data(DeleteString);
                string lv1 = CheckBox1.Checked ? "Y" : "N";
                string lv2 = CheckBox2.Checked ? "Y" : "N";
                string LEVEL = "";
                if (lv1 == lv2)
                {
                    if (lv1 == "Y")
                    {
                        LEVEL = "1";
                        string INSERTSTRING = "INSERT INTO C_REVIEW_MANAGEMENT SELECT  NEXT VALUE FOR dbo.c_review_management_sn,MZ_EXAD ,MZ_EXUNIT,MZ_ID,MZ_OCCC,'" + LEVEL + "' FROM A_DLBASE WHERE MZ_EXAD='" + DropDownList_MZ_EXAD.SelectedValue + "' AND MZ_EXUNIT='" + DropDownList_MZ_EXUNIT.SelectedValue + "'";
                        o_DBFactory.ABC_toTest.Edit_Data(INSERTSTRING);
                        LEVEL = "2";
                        string INSERTSTRING1 = "INSERT INTO C_REVIEW_MANAGEMENT SELECT  NEXT VALUE FOR dbo.c_review_management_sn,MZ_EXAD ,MZ_EXUNIT,MZ_ID,MZ_OCCC,'" + LEVEL + "' FROM A_DLBASE WHERE MZ_EXAD='" + DropDownList_MZ_EXAD.SelectedValue + "' AND MZ_EXUNIT='" + DropDownList_MZ_EXUNIT.SelectedValue + "'";
                        o_DBFactory.ABC_toTest.Edit_Data(INSERTSTRING1);
                    }
                    else {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請選擇層級');", true);
                    }
                }
                else if (lv1 == "Y") {

                    LEVEL = "1";
                    string INSERTSTRING = "INSERT INTO C_REVIEW_MANAGEMENT SELECT  NEXT VALUE FOR dbo.c_review_management_sn,MZ_EXAD ,MZ_EXUNIT,MZ_ID,MZ_OCCC,'" + LEVEL + "' FROM A_DLBASE WHERE MZ_EXAD='" + DropDownList_MZ_EXAD.SelectedValue + "' AND MZ_EXUNIT='" + DropDownList_MZ_EXUNIT.SelectedValue + "'";
                    o_DBFactory.ABC_toTest.Edit_Data(INSERTSTRING);
                }else if(lv2=="Y"){

                    LEVEL = "2";
                    string INSERTSTRING = "INSERT INTO C_REVIEW_MANAGEMENT SELECT  NEXT VALUE FOR dbo.c_review_management_sn,MZ_EXAD ,MZ_EXUNIT,MZ_ID,MZ_OCCC,'" + LEVEL + "' FROM A_DLBASE WHERE MZ_EXAD='" + DropDownList_MZ_EXAD.SelectedValue + "' AND MZ_EXUNIT='" + DropDownList_MZ_EXUNIT.SelectedValue + "'";
                    o_DBFactory.ABC_toTest.Edit_Data(INSERTSTRING);
                }
                    
                

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
