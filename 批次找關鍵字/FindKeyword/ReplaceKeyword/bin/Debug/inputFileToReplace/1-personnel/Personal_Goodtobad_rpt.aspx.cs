using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using TPPDDB.App_Code; 

namespace TPPDDB._1_personnel
{
    public partial class Personal_Goodtobad_rpt : System.Web.UI.Page
    {
        int TPM_FION=0;
        

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                A.check_power();
                A.set_Panel_EnterToTAB(ref this.Panel1);
            A.set_Panel_EnterToTAB(ref this.Panel3);
  A.fill_AD_POST(DropDownList_MZ_EXAD); 
              
                DropDownList_MZ_EXAD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                ViewState["A_strGID"]  = o_a_Function.strGID(Session["ADPMZ_ID"].ToString());
              chk_TPMGroup();
            }

            
           
        }

        protected void ChangeDropDownList_AD()
        {
            string strSQL = "SELECT * FROM Z_CODE_ON WHERE NEW='" + Session["ADPMZ_EXAD"].ToString() + "'";
            DataTable tempDT = new DataTable();
            tempDT = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");

            //2013/12/02
            strSQL = "SELECT  MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE = '04' AND (MZ_KCODE='" + tempDT.Rows[0][0].ToString() + "' OR MZ_KCODE='" + tempDT.Rows[0][1].ToString() + "')";
            DataTable source = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
            DropDownList_MZ_EXAD.DataSource = source;
            DropDownList_MZ_EXAD.DataTextField = "MZ_KCHI";
            DropDownList_MZ_EXAD.DataValueField = "MZ_KCODE";            
            DropDownList_MZ_EXAD.DataBind();

            DropDownList_MZ_EXAD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
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

                    break;
                case "C":
                    ChangeDropDownList_AD();
                    break;
                case "D":
                case "E":
                    //無權限
                    TPMPermissions._boolErrorData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "ADUXXX0001");
                    Response.Redirect("~/ErrorPage.aspx?erid=ADUXXX0001");
                    break;

            }
        }

        protected void btPrint_Click(object sender, EventArgs e)
        {

            
            string tmp_url = "A_rpt.aspx?fn=goodtobad&TPM_FION=" + TPM_FION + "&MZ_AD=" + DropDownList_MZ_EXAD.SelectedValue.Trim()+
                "&MZ_DATE1=" + TextBox_MZ_DATE1.Text.Trim().Replace("/", "").PadLeft(7, '0') + "&MZ_DATE2=" + TextBox_MZ_DATE2.Text.Trim().Replace("/", "").PadLeft(7, '0');

            ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);

        }

        protected void returnSameDataType(TextBox tb, Object ob1)
        {
            tb.Text = o_str.tosql(tb.Text.Trim().Replace("/", ""));

            if (tb.Text != "")
            {
                if (!DateManange.Check_date(tb.Text))
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('日期格式錯誤')", true);
                    tb.Focus();
                }
                else
                {
                    tb.Text = o_CommonService.Personal_ReturnDateString(tb.Text.Trim());
                    if (ob1 is DropDownList)
                    {
                        (ob1 as DropDownList).Focus();
                    }
                    else if (ob1 is TextBox)
                    {
                        (ob1 as TextBox).Focus();
                    }
                }
            }
        }

        protected void TextBox_MZ_IDATE1_TextChanged(object sender, EventArgs e)
        {
            returnSameDataType(TextBox_MZ_DATE1, TextBox_MZ_DATE2);
        }

        protected void TextBox_MZ_IDATE2_TextChanged(object sender, EventArgs e)
        {
            returnSameDataType(TextBox_MZ_DATE2, DropDownList_MZ_EXAD);
        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            TextBox_MZ_DATE1.Text = string.Empty;
            TextBox_MZ_DATE2.Text = string.Empty;
        }


    }
}
