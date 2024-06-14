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
    public partial class B_SearchBonus_AD : System.Web.UI.Page
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

            Label_TITLE.Text = "機關總表";
        }


      

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
                 dt_all_AD_View.Columns.Add("MonthPay", typeof(double));
                 dt_all_AD_View.Columns.Add("MonthPay_Boss", typeof(double));

                 dt_all_AD_View.Columns.Add("RepairPay", typeof(double));
                 dt_all_AD_View.Columns.Add("RepairPay_Boss", typeof(double));
                 
                 dt_all_AD_View.Columns.Add("Sole", typeof(double));
                 
                 
                 dt_all_AD_View.Columns.Add("Total", typeof(double));


                 double MonthPay = 0;
                 double MonthPay_Boss = 0;
                 double RepairPay = 0;
                 double RepairPay_Boss = 0;
                 double Sole = 0;

                 //double Total = 0;


                 string note = "";

                 #region 公職人員
                 if (RadioButtonList1.SelectedValue == "0")//公職人員
                 {
                     

                     //每月薪資不包含主管加給
                     //應發
                     strSQL = " SELECT SUM(SALARYPAY1+PROFESS+WORKP+TECHNICS+BONUS+ADVENTIVE+FAR+ELECTRIC) " +//應發

                     "PAY FROM B_MONTHPAY_MAIN WHERE PAY_AD='" + strAD + "'  AND AMONTH ='" + Date + "'" +

                        " AND (MZ_SRANK LIKE  'P%' OR MZ_SRANK LIKE  'G%' OR  MZ_SRANK LIKE  'B%' )    GROUP BY  PAY_AD";



                     note = o_DBFactory.ABC_toTest.vExecSQL(strSQL);
                     if (note != "" && note != null)
                     {
                         MonthPay = Convert.ToDouble(note);
                         note = "";
                     }

                     //每月薪資的主管加給

                     strSQL = " SELECT SUM(BOSS) " +//應發

                     "PAY FROM B_MONTHPAY_MAIN WHERE PAY_AD='" + strAD + "'  AND AMONTH ='" + Date + "'" +

                        " AND (MZ_SRANK LIKE  'P%' OR MZ_SRANK LIKE  'G%' OR  MZ_SRANK LIKE  'B%' )    GROUP BY  PAY_AD";



                     note = o_DBFactory.ABC_toTest.vExecSQL(strSQL);
                     if (note != "" && note != null)
                     {
                         MonthPay_Boss = Convert.ToDouble(note);
                         note = "";
                     }



                     //補發不包含主管加給
                     

                     strSQL = " SELECT SUM(SALARYPAY1+PROFESSPAY+WORKPPAY+TECHNICSPAY+BONUSPAY+ADVENTIVEPAY+FARPAY+ELECTRICPAY) " +//應發

                         "PAY FROM B_REPAIRPAY WHERE PAY_AD='" + strAD + "'  AND AMONTH ='" + Date + "'" +
                          " AND (MZ_SRANK LIKE  'P%' OR MZ_SRANK LIKE  'G%' OR  MZ_SRANK LIKE  'B%' )  GROUP BY  PAY_AD";

                     note = o_DBFactory.ABC_toTest.vExecSQL(strSQL);
                     if (note != "" && note != null)
                     {
                         RepairPay = Convert.ToDouble(note);
                         note = "";
                     }

                     //補發的主管加給
                     
                     strSQL = " SELECT SUM(BOSSPAY) " +//應發

                         "PAY FROM B_REPAIRPAY WHERE PAY_AD='" + strAD + "'  AND AMONTH ='" + Date + "'" +
                          " AND (MZ_SRANK LIKE  'P%' OR MZ_SRANK LIKE  'G%' OR  MZ_SRANK LIKE  'B%' )  GROUP BY  PAY_AD";

                     note = o_DBFactory.ABC_toTest.vExecSQL(strSQL);
                     if (note != "" && note != null)
                     {
                         RepairPay_Boss = Convert.ToDouble(note);
                         note = "";
                     }

                     //單一發放
                    
                     strSQL = " SELECT SUM(PAY) PAY FROM B_SOLE WHERE PAY_AD='" + strAD + "' AND TAXES_ID='50' AND dbo.SUBSTR(DA, 1, 5) ='" + Date + "'" +
                          " AND (MZ_SRANK LIKE  'P%' OR MZ_SRANK LIKE  'G%' OR  MZ_SRANK LIKE  'B%' )  GROUP BY  PAY_AD";

                     note = o_DBFactory.ABC_toTest.vExecSQL(strSQL);
                     if (note != "" && note != null)
                     {
                         Sole = Convert.ToDouble(note);
                         note = "";
                     }

                     //Total = MonthPay + RepairPay + Sole;
                 }
                 #endregion 公職人員

                 #region  駕駛工友
                 else if (RadioButtonList1.SelectedValue == "1")//駕駛工友
                 {
                    
                     //每月薪資不包含主管加給
                     //應發
                     strSQL = " SELECT SUM(SALARYPAY1+PROFESS+WORKP+TECHNICS+BONUS+ADVENTIVE+FAR+ELECTRIC) " +//應發

                     "PAY FROM B_MONTHPAY_MAIN WHERE PAY_AD='" + strAD + "'  AND AMONTH ='" + Date + "'" +

                        " AND (MZ_SRANK='J20'  OR  MZ_SRANK='J30'  )    GROUP BY  PAY_AD";



                     note = o_DBFactory.ABC_toTest.vExecSQL(strSQL);
                     if (note != "" && note != null)
                     {
                         MonthPay = Convert.ToDouble(note);
                         note = "";
                     }

                     //每月薪資的主管加給

                     strSQL = " SELECT SUM(BOSS) " +//應發

                     "PAY FROM B_MONTHPAY_MAIN WHERE PAY_AD='" + strAD + "'  AND AMONTH ='" + Date + "'" +

                        " AND (MZ_SRANK='J20'  OR  MZ_SRANK='J30'  )   GROUP BY  PAY_AD";



                     note = o_DBFactory.ABC_toTest.vExecSQL(strSQL);
                     if (note != "" && note != null)
                     {
                         MonthPay_Boss = Convert.ToDouble(note);
                         note = "";
                     }



                    
                     strSQL = " SELECT SUM(SALARYPAY1+PROFESSPAY+WORKPPAY+TECHNICSPAY+BONUSPAY+ADVENTIVEPAY+FARPAY+ELECTRICPAY) " +//應發

                         "PAY FROM B_REPAIRPAY WHERE PAY_AD='" + strAD + "'  AND AMONTH ='" + Date + "'" +
                          " AND (MZ_SRANK='J20'  OR  MZ_SRANK='J30'  )  GROUP BY  PAY_AD";

                     note = o_DBFactory.ABC_toTest.vExecSQL(strSQL);
                     if (note != "" && note != null)
                     {
                         RepairPay = Convert.ToDouble(note);
                         note = "";
                     }

                  
                     strSQL = " SELECT SUM(BOSSPAY) " +//應發

                         "PAY FROM B_REPAIRPAY WHERE PAY_AD='" + strAD + "'  AND AMONTH ='" + Date + "'" +
                          " AND (MZ_SRANK='J20'  OR  MZ_SRANK='J30'  )  GROUP BY  PAY_AD";

                     note = o_DBFactory.ABC_toTest.vExecSQL(strSQL);
                     if (note != "" && note != null)
                     {
                         RepairPay_Boss = Convert.ToDouble(note);
                         note = "";
                     }

                    
                     strSQL = " SELECT SUM(PAY) PAY FROM B_SOLE WHERE PAY_AD='" + strAD + "' AND TAXES_ID='50' AND dbo.SUBSTR(DA, 1, 5) ='" + Date + "'" +
                          " AND (MZ_SRANK='J20'  OR  MZ_SRANK='J30'  ) GROUP BY  PAY_AD";

                     note = o_DBFactory.ABC_toTest.vExecSQL(strSQL);
                     if (note != "" && note != null)
                     {
                         Sole = Convert.ToDouble(note);
                         note = "";
                     }

                 }
                 #endregion  駕駛工友


                 DataRow dr = dt_all_AD_View.NewRow();
                 dr["MonthPay"] = MonthPay;
                 dr["MonthPay_Boss"] = MonthPay_Boss;
                 dr["RepairPay"] = RepairPay;
                 dr["RepairPay_Boss"] = RepairPay_Boss;
                 dr["Sole"] = Sole;
                 
                 

                 dt_all_AD_View.Rows.Add(dr);



                 if (dt_all_AD_View.Rows.Count == 0)
                 {
                     ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "alert('此搜尋條件查無資料');", true);

                  
                 }
                 else
                 {
                    
                     Session["rpt_dt"] = dt_all_AD_View;
                     Session["TITLE"] = " " + Date.Substring(0,3) + " 年 "+Date.Substring(Date.Length-2,2)+" 月 ";

                     Session["SalaryReportAD"] = " " +o_A_KTYPE.CODE_TO_NAME(strAD, "04");
                     string tmp_url = "B_rpt.aspx?fn=SearchBonus_AD&TPM_FION=" + Request.QueryString["TPM_FION"];
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
