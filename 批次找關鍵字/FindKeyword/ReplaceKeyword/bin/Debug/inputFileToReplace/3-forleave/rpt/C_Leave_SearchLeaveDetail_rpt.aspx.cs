using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TPPDDB._3_forleave.rpt
{
    public partial class C_Leave_SearchLeaveDetail_rpt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                C.check_power();
                ViewState["C_strGID"] = o_a_Function.strGID(Session["ADPMZ_ID"].ToString());

                ///群組權限
                
                //MQ-----------------------20100331
                C.set_Panel_EnterToTAB(ref this.Panel1);


                C.fill_AD_POST(DropDownList_EXAD);

                chk_TPMGroup();
                
            }
        }


        /// <summary>
        /// 群組權限
        /// </summary>
        protected void chk_TPMGroup()
        {
            switch (ViewState["C_strGID"].ToString())
            {
                case "A":                 
                case "B":
                    DropDownList_EXAD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                    break;
                case "C":
                case "E":
                    DropDownList_EXAD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                    DropDownList_EXAD.Enabled = false;
                    break;
                case "D":
                    //無權限
                    TPMPermissions._boolErrorData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "ADUXXX0001");
                    Response.Redirect("~/ErrorPage.aspx?erid=ADUXXX0001");
                    break;


            }
        }


        protected void btPrint_Click(object sender, EventArgs e)
        {
            string strSQL = @"SELECT MZ_NAME 姓名,AKEU.MZ_KCHI 單位,AKO.MZ_KCHI 職稱 ,CC.MZ_IDATE1 日期起 ,CC.MZ_ITIME1 時間起, CC.MZ_ODATE 日期迄 , CC.MZ_OTIME 時間迄 ,CC.MZ_TDAY 合計日,CC.MZ_TTIME 合計時,CCO.MZ_CNAME 假別, CC.MZ_CAUSE 事由 ,MZ_RNAME 代理人
   FROM C_DLTB01 CC  
LEFT JOIN  C_DLCODE CCO ON CCO. MZ_CODE =CC.MZ_CODE
LEFT JOIN  A_KTYPE AKED ON AKED.MZ_KCODE=CC.MZ_EXAD AND AKED.MZ_KTYPE='04'
LEFT JOIN  A_KTYPE AKEU ON AKEU.MZ_KCODE=CC.MZ_EXUNIT AND AKEU.MZ_KTYPE='25'

LEFT JOIN  A_KTYPE AKO ON AKO.MZ_KCODE=CC.MZ_OCCC AND AKO.MZ_KTYPE='26'
WHERE CC.MZ_EXAD=@EXAD AND (CC.MZ_IDATE1 BETWEEN @DATE1 AND @DATE2 OR MZ_ODATE BETWEEN @DATE1 AND @DATE2 )
ORDER BY CC.MZ_EXUNIT,CC.MZ_OCCC,CC.MZ_ID,MZ_IDATE1";


            DateTime Date1 = Convert.ToDateTime(( (int.Parse(TextBox_MZ_DATE1.Text.Substring(0, 3))+1911) + "/" + TextBox_MZ_DATE1.Text.Substring(3, 2) + "/01"));
            Date1 = Date1.AddMonths(1).AddDays(-1);
            string date1 = TextBox_MZ_DATE1.Text+"01";

            string date2 = (Date1.Year - 1911).ToString().PadLeft(3, '0') + Date1.Month.ToString().PadLeft(2, '0') + Date1.Day.ToString().PadLeft(2, '0');

           
            

            if (!string.IsNullOrEmpty(TextBox_MZ_DATE1.Text.Trim()) )
            {
                if (TextBox_MZ_DATE1.Text.Length !=5)
                {                
                    ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "alert('請檢查欲查詢之起迄日期');", true);
                    return;
                }
            }


            DataTable tempID = new DataTable();


            List<System.Data.SqlClient.SqlParameter> para = new List<System.Data.SqlClient.SqlParameter>();
            para.Add(new System.Data.SqlClient.SqlParameter("EXAD", DropDownList_EXAD.SelectedValue));
            para.Add(new System.Data.SqlClient.SqlParameter("DATE1", date1));
            para.Add(new System.Data.SqlClient.SqlParameter("DATE2", date2));

           tempID= o_DBFactory.ABC_toTest.GetDataTable(strSQL, para);




            if (tempID.Rows.Count > 0)
            {

                TPPDDB.App_Code.ToExcel.Dt2Excel(tempID, DropDownList_EXAD.SelectedItem.Text + TextBox_MZ_DATE1.Text  + "請假清單");
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "alert('無此日期之相關資料');", true);

            }
        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            TextBox_MZ_DATE1.Text = string.Empty;
           
        }

        


    }
}
