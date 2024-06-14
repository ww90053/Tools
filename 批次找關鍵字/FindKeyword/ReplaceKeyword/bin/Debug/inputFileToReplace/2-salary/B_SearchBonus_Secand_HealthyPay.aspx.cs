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
    public partial class B_SearchBonus_Secand_HealthyPay : System.Web.UI.Page
    {
        string strSQL = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {//檢查權限
                SalaryPublic.checkPermission();
          
                SalaryPublic.fillDropDownList(ref this.DropDownList_PAY_AD);//只有權限為A.B能用下拉選單
                TextBox_Date.Text = SalaryPublic.strRepublicYearMonth();
            }

            Label_TITLE.Text = "二代健保補充保費明細表";
        }



        /// <summary>
        /// 判斷年分是否還停留在初始化設定
        /// </summary>
        /// <returns></returns>
        protected bool chkYearType()
        {
            if (TextBox_Date.Text.Length == 5)
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

                string Date = TextBox_Date.Text;


                DataTable dt_all_AD_View = new DataTable();

                strSQL = " SELECT CASEID \"批號\",AKU.MZ_KCHI \"單位\", AKO.MZ_KCHI  \"職位\" , MZ_NAME  \"姓名\" ,(dbo.SUBSTR(DA, 1, 3) + '/' + dbo.SUBSTR(DA, 4, 2) + '/' + dbo.SUBSTR(DA, 6, 2)) \"入帳日期\"  ," +
                    " CODE.NAME \"發放項目\" ,   SECOND_HEALTHPAY_PAY  \"二代健保補充保費\", NOTE \"項目說明\"  FROM dbo.B_SOLE SOLE" +
                    " INNER JOIN B_SOLEITEM CODE ON  SOLE.NUM=CODE.ID" +
                    " INNER JOIN dbo.A_KTYPE AKU ON AKU.MZ_KCODE =MZ_UNIT AND AKU.MZ_KTYPE='25' " +
                    " INNER JOIN dbo.A_KTYPE AKO ON AKO.MZ_KCODE =MZ_OCCC AND AKO.MZ_KTYPE='26' " +
                    " WHERE PAY_AD='" + strAD + "' AND  dbo.SUBSTR(DA, 1, 5) ='" + Date + "'" +
                    " AND SECOND_HEALTHPAY_PAY>0" +
                   
                    " AND (MZ_SRANK LIKE  'P%' OR MZ_SRANK LIKE  'G%' OR  MZ_SRANK LIKE  'B%' )   ";//MZ_UNIT,MZ_OCCC,MZ_NAME,

                if (!(string.IsNullOrEmpty(TextBox_CaseID.Text)))
                    strSQL += " AND CASEID='" + TextBox_CaseID.Text + "' ";


                strSQL += "ORDER BY DA, MZ_UNIT, MZ_OCCC";

                dt_all_AD_View = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");

               




                if (dt_all_AD_View.Rows.Count == 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "alert('此搜尋條件查無資料');", true);

                }
                else
                {
                    //如果有更改欄位位置 O_Function中的RenderDataTableToExcel 的例外也要更動
                    App_Code.ToExcel.Dt2Excel(dt_all_AD_View, "EXPORT");


                }


            }

            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "alert('輸入年份格式錯誤');", true);


            }

        }
    }
}
