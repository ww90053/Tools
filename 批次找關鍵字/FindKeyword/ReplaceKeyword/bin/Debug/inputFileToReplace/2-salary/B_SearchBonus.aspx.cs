using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace TPPDDB._2_salary
{
    public partial class B_SearchBonus : System.Web.UI.Page
    {
        string strSQL = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {//檢查權限
                SalaryPublic.checkPermission();            
                SalaryPublic.fillDropDownList(ref this.DropDownList_PAY_AD);//只有權限為A.B能用下拉選單
                TextBox_YEAR.Text = SalaryPublic.strRepublicYear();
            }

            Label_TITLE.Text = "個人總表";

            
        }


        /// <summary>
        /// 判斷年分是否還停留在初始化設定
        /// </summary>
        /// <returns></returns>
        protected bool chkYearType()
        {
            if (TextBox_YEAR.Text.Length == 3)
            {
                return true;
            }
            return false;
        }

        protected void Button_SEARCH_Click(object sender, EventArgs e)
        {
            if (chkYearType())
            {
                string strAD = DropDownList_PAY_AD.SelectedValue.ToString();
                string Year = TextBox_YEAR.Text;
                

                strSQL = "SELECT substr(IDCARD,0,2)+'XXXXX'+ substr(IDCARD,8,3 ) IDNumber,MZ_NAME Name,EFFECT ,YEARPAY ,SOLE ,TOTAL ,INCREASE ,INCREASE_X4 ,CASE  EXCEED WHEN 'Y' THEN '是' WHEN 'N' THEN '否' END EXCEED  ,AKO.MZ_KCHI OCCC,AKU.MZ_KCHI Unit " +
                    "FROM dbo.B_SUM_BONUS B " +
                    "LEFT JOIN dbo.A_KTYPE AKU ON AKU.MZ_KCODE =MZ_UNIT AND AKU.MZ_KTYPE='25' " +
                    "LEFT JOIN dbo.A_KTYPE AKO ON AKO.MZ_KCODE =MZ_OCCC AND AKO.MZ_KTYPE='26' " +
                    " WHERE AYEAR='" + Year + "' AND PAY_AD='" + strAD + "' ";


                if(!(string.IsNullOrEmpty( txt_IDCard.Text)))
                    strSQL += "AND IDCARD='" + txt_IDCard.Text.ToUpper() + "'";


                if (RadioButtonList_TYPE.SelectedValue == "EXCEED")
                    strSQL += "AND Exceed='Y'";



                strSQL += "ORDER BY MZ_UNIT, MZ_OCCC";


                DataTable dt_all_AD_View = new DataTable();
                dt_all_AD_View = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");
               


                if (dt_all_AD_View.Rows.Count == 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "alert('找不到資料');", true);
                   
                }
                else
                {
                    
                    Session["rpt_dt"] = dt_all_AD_View;
                    Session["TITLE"] = " " + Year + "年";
                    Session["SalaryReportAD"] = " " + o_A_KTYPE.CODE_TO_NAME(strAD, "04");
                    string tmp_url = "B_rpt.aspx?fn=SearchBonus&TPM_FION=" + Request.QueryString["TPM_FION"];
                    ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
                }


            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "alert('輸入年份格式錯誤');", true);

            }
        }

       
     
    }
}
