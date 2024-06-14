using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using TPPDDB.App_Code; 

namespace TPPDDB._3_forleave
{
    public partial class C_dutydetail_rpt : System.Web.UI.Page
    {
        int TPM_FION=0;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                C.check_power();
                //matthew 為了中和分局判斷功能權限用
                ViewState["C_strGID"] = o_a_Function.strGID(Session["ADPMZ_ID"].ToString());
                //by MQ 20100312---------   
                C.set_Panel_EnterToTAB(ref this.Panel1);
           
                ///群組權限
             
                //C.fill_AD_POST(DropDownList_EXAD);
                //如果是中和分局進來改成只抓中和一&中和二&中和 matthew
                if (Session["ADPMZ_EXAD"].ToString() == "382133600C")
                {
                    C.fill_DLL_ONE_TWO(DropDownList_EXAD);
                }
                else
                {
                    //把所有機關撈出來包含台北縣
                    C.fill_AD_POST(DropDownList_EXAD);
                }
                DropDownList_EXAD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                C.fill_unit(DropDownList_EXUNIT, DropDownList_EXAD.SelectedValue);
                DropDownList_EXUNIT.Items.Insert(0, new ListItem("全部", ""));
                chk_TPMGroup();
            }
        }

        /// <summary>
        /// 群組權限
        /// </summary>
        protected void chk_TPMGroup()
        {
            switch (o_a_Function.strGID(Session["ADPMZ_ID"].ToString()) )
            {
                case "A":                   
                case "B":                   
                    break;
                case "C":                  
                    //DropDownList_EXAD.Enabled = false;
                    //如果是中和分局進來改成只抓中和一&中和二&中和 matthew
                    if (Session["ADPMZ_EXAD"].ToString() != "382133600C")
                    {
                        DropDownList_EXAD.Enabled = false;
                    }
                    break;
                case "D":
                    //無權限
                    TPMPermissions._boolErrorData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "ADUXXX0001");
                    Response.Redirect("~/ErrorPage.aspx?erid=ADUXXX0001");
                    break;
                case "E":                   
                    //DropDownList_EXAD.Enabled = false;
                    //如果是中和分局進來改成只抓中和一&中和二&中和 matthew
                    if (Session["ADPMZ_EXAD"].ToString() != "382133600C")
                    {
                        DropDownList_EXAD.Enabled = false;
                    }
                    DropDownList_EXUNIT.SelectedValue = Session["ADPMZ_EXUNIT"].ToString();
                    DropDownList_EXUNIT.Enabled = false;
                    break;
            }
        }

        private void Ktype_Cname_Check(string Cname, TextBox tb1, TextBox tb2, object obj)
        {
            tb2.Text = tb2.Text.ToUpper();
            //檢查代碼中文名稱是否存在
            if (string.IsNullOrEmpty(Cname))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('查無資料')", true);
                tb1.Text = string.Empty;
                tb2.Text = string.Empty;
                tb2.Focus();
            }
            else
            {
                tb1.Text = Cname;
                if (obj is TextBox)
                {
                    (obj as TextBox).Focus();
                }
                else if (obj is RadioButtonList)
                {
                    (obj as RadioButtonList).Focus();
                }
            }
        }


        protected void btPrint_Click(object sender, EventArgs e)
        {
            string strSQL = @"  SELECT CC.MZ_ID, CC.MZ_NAME, AKD.MZ_KCHI MZ_EXAD, AKU.MZ_KCHI MZ_EXUNIT,   CC.MZ_OCCC ,    CD.MZ_CNAME  MZ_CODE,   AKR.MZ_KCHI  MZ_RANK1,
             CC.MZ_SYSDAY, CC.MZ_IDATE1, CC.MZ_ITIME1, CC.MZ_ODATE, CC.MZ_OTIME, CC.MZ_TDAY, CC.MZ_TTIME, CC.MZ_CAUSE, CC.MZ_MEMO,
             CC.MZ_SWT, MZ_TADD, CC.MZ_RNAME,  AKRO.MZ_KCHI   MZ_ROCCC,     CC.MZ_CHK1, MZ_SYSTIME,  MZ_FOREIGN,MZ_CHINA,
            TO_NUMBER (CASE WHEN  ABB.MZ_TBDV  IS NULL THEN '999' WHEN  ABB.MZ_TBDV='Z99' THEN '999' ELSE  ABB.MZ_TBDV END) AS TBDV, 
            CC.MZ_OCCC AS OCCC 
                            FROM C_DLTB01 CC
   LEFT JOIN  A_KTYPE AKD ON AKD.MZ_KCODE=CC.MZ_EXAD AND AKD.MZ_KTYPE='04' 
                            LEFT JOIN  A_KTYPE AKU ON AKU.MZ_KCODE=CC.MZ_EXUNIT AND AKU.MZ_KTYPE='25' 
                            LEFT JOIN  C_DLCODE  CD ON  CD.MZ_CODE=CC.MZ_CODE 
                            LEFT JOIN  A_KTYPE AKR ON AKR.MZ_KCODE=CC.MZ_RANK1 AND AKR.MZ_KTYPE='09' 
                            LEFT JOIN  A_KTYPE AKRO ON AKRO.MZ_KCODE=CC.MZ_ROCCC AND AKRO.MZ_KTYPE='26' 
                            LEFT JOIN  A_KTYPE AKO ON AKO.MZ_KCODE=CC.MZ_OCCC AND AKO.MZ_KTYPE='26'
   LEFT  JOIN  A_DLBASE  ABB ON ABB.MZ_ID=CC.MZ_ID ";

            TextBox_MZ_DATE1.Text = TextBox_MZ_DATE1.Text.PadLeft(7, '0');
            TextBox_MZ_DATE2.Text = TextBox_MZ_DATE2.Text.PadLeft(7, '0');

            List<string> condition = new List<string>();
            
            if (TextBox_MZ_DATE1.Text.Trim() != string.Empty)
            {
                condition.Add("CC.MZ_IDATE1 >= '" + TextBox_MZ_DATE1.Text.Replace("/", string.Empty) + "'");
                if (TextBox_MZ_DATE2.Text.Trim() != string.Empty)
                {
                    condition.Add("CC.MZ_IDATE1 <= '" + TextBox_MZ_DATE2.Text.Replace("/", string.Empty) + "'");
                }
                else
                {
                    condition.Add("CC.MZ_IDATE1 <= '" + TextBox_MZ_DATE1.Text.Replace("/", string.Empty) + "'");
                }
            }

            if (DropDownList_EXAD.SelectedValue != string.Empty)
            {
                condition.Add("CC.MZ_EXAD='" + DropDownList_EXAD.SelectedValue + "'");
            }
            if (DropDownList_EXUNIT.SelectedValue != string.Empty)
            {
                condition.Add("CC.MZ_EXUNIT='" + DropDownList_EXUNIT.SelectedValue + "'");
            }

            condition.Add("(CC.MZ_TDAY>0 OR CC.MZ_TTIME>0)");

            string where = (condition.Count > 0 ? " WHERE " + string.Join(" AND ", condition.ToArray()) : string.Empty);

            strSQL += where;

            strSQL += " ORDER BY TBDV,OCCC";

            Session["RPT_SQL_C"] = strSQL;   

            DataTable dutydetail = new DataTable();

            dutydetail = o_DBFactory.ABC_toTest.Create_Table(strSQL, "dutydetail");

            if (dutydetail.Rows.Count > 0)
            {
                if (DateManange.Check_date(TextBox_MZ_DATE1.Text) && DateManange.Check_date(TextBox_MZ_DATE2.Text))
                {
                    for (int i = 0; i < dutydetail.Rows.Count; i++)
                    {
                        dutydetail.Rows[i]["MZ_OCCC"] = o_A_DLBASE.EXTPOS_OR_OCCC(dutydetail.Rows[i]["MZ_ID"].ToString(), dutydetail.Rows[i]["MZ_OCCC"].ToString());
                    }

                    Session["TITLE"] = string.Format("{0}員警差假明細表", o_A_KTYPE.RAD(DropDownList_EXAD.SelectedValue));



                    //Session["rpt_dt"] = dutydetail;

                    string tmp_url = "C_rpt.aspx?fn=dutydetail&TPM_FION=" + TPM_FION;

                    ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "alert('請檢查查詢之起迄日期');", true);
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "alert('無此日期之相關資料');", true);

            }
        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            TextBox_MZ_DATE1.Text = string.Empty;
            TextBox_MZ_DATE2.Text = string.Empty;
        }

        protected void DropDownList_EXUNIT_DataBound(object sender, EventArgs e)
        {

        }

        protected void DropDownList_EXAD_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ViewState["C_strGID"].ToString() == "E")//權限E選擇所屬單位並鎖單位
            {
                DropDownList_EXUNIT.SelectedValue = Session["ADPMZ_EXUNIT"].ToString();
                DropDownList_EXUNIT.Enabled = false;

            }
            else
            {
                C.fill_unit(DropDownList_EXUNIT, DropDownList_EXAD.SelectedValue);
                DropDownList_EXUNIT.Items.Insert(0, new ListItem("全部", ""));
            }
        }

    }
}
