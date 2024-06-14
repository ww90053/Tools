using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Microsoft.VisualBasic;

namespace TPPDDB._1_personnel
{
    public partial class PersonalPRK1IDSearch_Export : System.Web.UI.Page
    {
        int TPM_FION=0;
          

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                string s = Session["ADPMZ_ID"].ToString();
                DropDownList_EXAD.DataBind();
                ///群組權限抓取
                ViewState["A_strGID"] = o_a_Function.strGID(Session["ADPMZ_ID"].ToString());
                A.set_Panel_EnterToTAB(ref this.Panel1);
                switch (ViewState["A_strGID"].ToString())
                {
                    case "A":
                    case "B":
                        //DropDownList_MZ_SWT3.SelectedValue = "1";
                        DropDownList_EXAD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                        break;
                    case "C":
                        //DropDownList_MZ_SWT3.SelectedValue = "2";
                        DropDownList_EXAD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                        DropDownList_MZ_SWT3.SelectedValue = "2";
                        DropDownList_EXAD.Enabled = false;
                        DropDownList_MZ_SWT3.Enabled = false;
                        //DropDownList_MZ_SWT3.Enabled = false;
                        break;
                    case "D":
                        btOK.Enabled = false;
                        break;
                    case "E":
                        btOK.Enabled = false;
                        break;


                }
            }
        }




        protected void btLeave_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.close();", true);
        }

        protected void btOK_Click(object sender, EventArgs e)
        {
            string strPartSQL1 = "";

            //switch (ViewState["A_strGID"].ToString())
            //{
            //    case "A":
            //    case "B":
            //    case "D":
            //    case "E":
            //    case "C":
            //        strPartSQL1 = " AND MZ_AD='" + Session["ADPMZ_EXAD"].ToString() + "'";
            //        break;

            //}

            string strPartSQL = "";

            string selectSQL = "SELECT COUNT(*) FROM A_PRKB WHERE 1=1";

            if (!string.IsNullOrEmpty(DropDownList_EXAD.SelectedValue.Trim()))
            {
                strPartSQL += " AND MZ_AD='" + DropDownList_EXAD.SelectedValue.Trim() + "'";
            }
            if (!string.IsNullOrEmpty(TextBox_MZ_NO1.Text.Trim()) && !string.IsNullOrEmpty(TextBox_MZ_NO2.Text.Trim()))
            {
                strPartSQL += " AND MZ_NO>='" + TextBox_MZ_NO1.Text.Trim() + "' AND MZ_NO<='" + TextBox_MZ_NO2.Text.Trim() + "'";
            }
            else if (!string.IsNullOrEmpty(TextBox_MZ_NO1.Text.Trim()))
            {
                strPartSQL += " AND MZ_NO='" + TextBox_MZ_NO1.Text.Trim() + "'";
            }
            if (!string.IsNullOrEmpty(DropDownList_MZ_SWT3.SelectedValue.Trim()))
            {
                strPartSQL += " AND MZ_SWT3='" + DropDownList_MZ_SWT3.SelectedValue.Trim() + "'";
            }
            string totalCount = o_DBFactory.ABC_toTest.vExecSQL(selectSQL + strPartSQL);

            if (string.IsNullOrEmpty(totalCount))//
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('查無資料！請重新輸入案號');", true);
                return;
            }


//            Session["PRK1_GV1"] = @" SELECT MZ_NO,MZ_PRCT,MZ_ID,MZ_NAME,
//                                   C1.mz_kchi AS mz_ad ,C2.mz_kchi AS mz_unit ,C3.mz_kchi AS mz_occc ,C4.mz_kchi AS mz_srank ,C5.mz_kchi AS mz_prrst
//                                   FROM A_PRKB  A
//                                   LEFT JOIN A_KTYPE C1 ON C1.MZ_KTYPE = '04' AND A.MZ_AD = C1.MZ_KCODE
//                                   LEFT JOIN A_KTYPE C2 ON C2.MZ_KTYPE = '25' AND A.MZ_UNIT = C2.MZ_KCODE
//                                   LEFT JOIN A_KTYPE C3 ON C3.MZ_KTYPE = '26' AND A.MZ_OCCC = C3.MZ_KCODE
//                                   LEFT JOIN A_KTYPE C4 ON C4.MZ_KTYPE = '09' AND A.MZ_SRANK = C4.MZ_KCODE
//                                   LEFT JOIN A_KTYPE C5 ON C5.MZ_KTYPE = '24' AND A.MZ_PRRST = C5.MZ_KCODE
//                                   WHERE 1=1 AND MZ_SWT4='Y' AND (MZ_SWT1!='Y' )" + strPartSQL + strPartSQL1;
            Session["PRK1_GV1"] = @" SELECT MZ_ID as 身分證號,
                                            MZ_NAME as 姓名,
                                            MZ_AD as 機關代碼,
                                            (MZ_PRK1+'H'+substr(MZ_PROLNO,1,4)+'@'+MZ_PRCT) as 更新資料庫事由,
                                            MZ_PRCT as 獎懲事由,
                                            MZ_PRRST as 獎懲結果 ,
                                            MZ_MEMO as 其他事項,
                                            MZ_PRK as 獎懲類別,
                                            'CD' as 適用法規,
                                            '警察人員獎懲標準' as 適用法規名稱,
                                            substr(MZ_PROLNO,1,2) as 條,
                                            '' as 點,
                                            '' as 項,
                                            substr(MZ_PROLNO,3,2) as 款,
                                            '' as 目
                                            FROM A_PRKB 
                                            WHERE 1=1" + strPartSQL ;

            //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.opener.document.getElementById('" + Session["PRK1_BT"] + "').click();window.close();", true);


           


          





            if (Session["PRK1_GV1"] != null)
            {
                DataTable dt = new DataTable();

                dt = o_DBFactory.ABC_toTest.Create_Table(Session["PRK1_GV1"] .ToString(), "get");
                //20140123

                ////轉全形(20170213 註記BY NICK)
                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    dt.Rows[i]["獎懲事由"] = Strings.StrConv(dt.Rows[i]["獎懲事由"].ToString(), VbStrConv.Wide, 0).PadRight(50, '　');//20140402 為解決罕見字 所以設定2052
                //    dt.Rows[i]["更新資料庫事由"] = Strings.StrConv(dt.Rows[i]["更新資料庫事由"].ToString(), VbStrConv.Wide, 0).PadRight(50, '　');
                //}

                App_Code.ToExcel.Dt2Excel(dt, HttpUtility.UrlEncode("Result", System.Text.Encoding.UTF8));


             

            }


        }

    }
}
