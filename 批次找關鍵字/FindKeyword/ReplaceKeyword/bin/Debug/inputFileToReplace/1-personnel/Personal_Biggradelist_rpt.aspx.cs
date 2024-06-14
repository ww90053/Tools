using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TPPDDB._1_personnel
{
    public partial class Personal_Biggradelist_rpt : System.Web.UI.Page
    {

        int TPM_FION=0;
       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                A.check_power();
                ViewState["A_strGID"] = o_a_Function.strGID(Session["ADPMZ_ID"].ToString());


                A.set_Panel_EnterToTAB(ref this.Panel1);


                chk_TPMGroup();
                TextBox_MZ_YM.Text = (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0') + DateTime.Now.Month.ToString().PadLeft(2, '0') + "01";
                TextBox_MZ_YM_2.Text = (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0') + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.AddDays(-DateTime.Now.Day).ToString("dd");
            }

           
            
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
                case "C":
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
           
            string tmp_url = "A_rpt.aspx?fn=biggradelist&TPM_FION=" + TPM_FION +
                 "&MZ_PRRST=" + DropDownList_MZ_PRRST.SelectedValue + "&DATE1=" + TextBox_MZ_YM.Text.Trim() + "&DATE2= " + TextBox_MZ_YM_2.Text.Trim();

            ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);

        
        }

      

        protected void btn_to_excel_Click(object sender, EventArgs e)
        {
            string strSQLPart = "";


            string strSQL = @"SELECT AKD.MZ_KCHI 機關, AKU.MZ_KCHI 單位 ,AKO.MZ_KCHI  職稱, MZ_NAME 姓名, MZ_ID 身份證字號, 
                             MZ_DATE 發文日期,  MZ_PRID + MZ_PRID1 發文字號,  
                             MZ_PRCT 獎懲內容,  AKP.MZ_KCHI   獎懲結果
                             FROM A_PRK2 
                             LEFT JOIN A_KTYPE AKP ON AKP.MZ_KCODE=MZ_PRRST AND AKP.MZ_KTYPE='24'
                             LEFT JOIN A_KTYPE AKD ON AKD.MZ_KCODE=MZ_AD AND AKD.MZ_KTYPE='04'
                             LEFT JOIN A_KTYPE AKU ON AKU.MZ_KCODE=MZ_UNIT AND AKU.MZ_KTYPE='25'
                             LEFT JOIN A_KTYPE AKO ON AKO.MZ_KCODE=MZ_OCCC AND AKO.MZ_KTYPE='26'
                             WHERE 
                             MZ_CHKAD='" + Session["ADPMZ_EXAD"].ToString().Trim() +
                            "' AND MZ_PRRST='" + DropDownList_MZ_PRRST.SelectedValue + "'" + strSQLPart;


            if (!string.IsNullOrEmpty(TextBox_MZ_YM.Text) && !string.IsNullOrEmpty(TextBox_MZ_YM_2.Text))
            {
                strSQL += " AND MZ_DATE BETWEEN '" + TextBox_MZ_YM.Text.Trim() + "' AND '" + TextBox_MZ_YM_2.Text.Trim() + "'";
            }
            else if (!string.IsNullOrEmpty(TextBox_MZ_YM.Text))
            {
                strSQL += " AND MZ_DATE='" + TextBox_MZ_YM.Text.Trim()+ "'";
            }

            strSQL += "  ORDER BY MZ_DATE,MZ_AD,MZ_UNIT,MZ_OCCC";

            DataTable dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");

           
            TPPDDB.App_Code.ToExcel.Dt2Excel(dt,string.Format("{0} 記功過以上獎懲明細表", o_A_KTYPE.RAD(Session["ADPMZ_EXAD"].ToString())));
        }
    }
}
