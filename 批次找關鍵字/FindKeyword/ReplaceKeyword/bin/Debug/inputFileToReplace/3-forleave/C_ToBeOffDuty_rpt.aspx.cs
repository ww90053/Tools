using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TPPDDB._3_forleave
{
    public partial class C_ToBeOffDuty_rpt : System.Web.UI.Page
    {
        int TPM_FION = 0;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                C.check_power();
                //matthew 為了中和分局判斷功能權限用
                ViewState["C_strGID"] = o_a_Function.strGID(Session["ADPMZ_ID"].ToString());

                TextBox_MZ_YEAR.Text = (int.Parse(DateTime.Now.Year.ToString()) - 1911).ToString().PadLeft(3, '0');

                //MQ-----------------------20100331

                C.set_Panel_EnterToTAB(ref this.Panel1);
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
                chk_TPMGroup();
            }
        }

        /// <summary>
        /// 群組權限
        /// </summary>
        protected void chk_TPMGroup()
        {
            switch (o_a_Function.strGID(Session["ADPMZ_ID"].ToString()))
            {
                case "A":
                case "B":
                    DropDownList_EXAD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                    C.fill_unit(DropDownList_EXUNIT, DropDownList_EXAD.SelectedValue);
                    break;
                case "C":
                    DropDownList_EXAD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                    C.fill_unit(DropDownList_EXUNIT, DropDownList_EXAD.SelectedValue);
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
                    DropDownList_EXAD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                    //DropDownList_EXAD.Enabled = false;
                    //如果是中和分局進來改成只抓中和一&中和二&中和 matthew
                    if (Session["ADPMZ_EXAD"].ToString() != "382133600C")
                    {
                        DropDownList_EXAD.Enabled = false;
                    }
                    C.fill_unit(DropDownList_EXUNIT, DropDownList_EXAD.SelectedValue);
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
            string strSQL = "SELECT (SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=MZ_EXUNIT AND MZ_KTYPE='25') MZ_EXUNIT,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=MZ_OCCC AND MZ_KTYPE='26') MZ_OCCC," +
                            " MZ_NAME,MZ_FDATE,MZ_HDAY,MZ_HTIME,MZ_TYEAR,MZ_TMONTH,MZ_RYEAR,MZ_RMONTH,MZ_OFFYY,MZ_OFFMM,MZ_OCCC AS OCCC,dbo.to_number(CASE WHEN MZ_TBDV IS NULL THEN '999' WHEN MZ_TBDV='Z99' THEN '999' ELSE MZ_TBDV END) AS TBDV " +
                            " FROM A_DLBASE AD,C_DLTBB CD" +
                            " WHERE AD.MZ_ID = CD.MZ_ID ";

            ;

            List<string> condition = new List<string>();
            if (TextBox_MZ_YEAR.Text.Trim() != string.Empty)
            {
                condition.Add("MZ_YEAR = '" + TextBox_MZ_YEAR.Text.Replace("/", string.Empty) + "'");
            }

            if (DropDownList_EXAD.SelectedValue != string.Empty)
            {
                condition.Add("MZ_EXAD='" + DropDownList_EXAD.SelectedValue + "'");
            }

            if (DropDownList_EXUNIT.SelectedValue != string.Empty)
            {
                condition.Add("MZ_EXUNIT='" + DropDownList_EXUNIT.SelectedValue + "'");
            }
            string where = (condition.Count > 0 ? " AND " + string.Join(" AND ", condition.ToArray()) : string.Empty);

            strSQL += where;

            strSQL += " ORDER BY AD.MZ_EXUNIT,TBDV,OCCC";

            Session["RPT_SQL_C"] = strSQL;

            DataTable ToBeOffDuty = new DataTable();

            ToBeOffDuty = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET2");

            //for (int i = 0; i < ToBeOffDuty.Rows.Count; i++)
            //{
            //    //任公職年資月數
            //    string FDATE = ToBeOffDuty.Rows[i]["MZ_FDATE"].ToString();

            //    int tyear = int.Parse(string.IsNullOrEmpty(ToBeOffDuty.Rows[i]["MZ_TYEAR"].ToString()) ? "0" : ToBeOffDuty.Rows[i]["MZ_TYEAR"].ToString());

            //    int tmonth = int.Parse(string.IsNullOrEmpty(ToBeOffDuty.Rows[i]["MZ_TMONTH"].ToString()) ? "0" : ToBeOffDuty.Rows[i]["MZ_TMONTH"].ToString());

            //    int ryear = int.Parse(string.IsNullOrEmpty(ToBeOffDuty.Rows[i]["MZ_RYEAR"].ToString()) ? "0" : ToBeOffDuty.Rows[i]["MZ_RYEAR"].ToString());

            //    int rmonth = int.Parse(string.IsNullOrEmpty(ToBeOffDuty.Rows[i]["MZ_RMONTH"].ToString()) ? "0" : ToBeOffDuty.Rows[i]["MZ_RMONTH"].ToString());


            //    if (!string.IsNullOrEmpty(FDATE))
            //    {

            //        System.DateTime dt1;

            //        try
            //        {
            //            dt1 = DateTime.Parse((int.Parse(FDATE.Substring(0, 3)) + 1911).ToString() + "-" + FDATE.Substring(3, 2) + "-" + FDATE.Substring(5, 2));
            //        }
            //        catch
            //        {
            //            dt1 = DateTime.Now;
            //        }

            //        System.DateTime dt2 = DateTime.Now;

            //        int monthDiff = Convert.ToInt32(Microsoft.VisualBasic.DateAndTime.DateDiff("M", dt1, dt2, Microsoft.VisualBasic.FirstDayOfWeek.Sunday, Microsoft.VisualBasic.FirstWeekOfYear.FirstFullWeek)) + tyear * 12 + tmonth - ryear * 12 - rmonth;

            //        if (monthDiff > 0)
            //        {
            //            ToBeOffDuty.Rows[i]["MZ_OFFYY"] = monthDiff / 12;
            //            ToBeOffDuty.Rows[i]["MZ_OFFMM"] = monthDiff % 12;
            //        }
            //        else
            //        {
            //            ToBeOffDuty.Rows[i]["MZ_OFFYY"] = 0;
            //            ToBeOffDuty.Rows[i]["MZ_OFFMM"] = 0;
            //        }
            //    }
            //    else
            //    {
            //        ToBeOffDuty.Rows[i]["MZ_OFFYY"] = 0;
            //        ToBeOffDuty.Rows[i]["MZ_OFFMM"] = 0;
            //    }
            //}
            if (ToBeOffDuty.Rows.Count > 0)
            {
                //Session["TITLE"] = string.Format("{0}員警應休假查核表", o_A_KTYPE.RAD(DropDownList_EXAD.SelectedValue));

                //Session["rpt_dt"] = ToBeOffDuty;

                string tmp_url = "C_rpt.aspx?fn=ToBeOffDuty&EXAD=" + DropDownList_EXAD.SelectedValue + "&TPM_FION=" + TPM_FION;

                ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "alert('無此年份之相關資料');", true);

            }
        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            TextBox_MZ_YEAR.Text = string.Empty;
        }

        protected void DropDownList_EXUNIT_DataBound(object sender, EventArgs e)
        {
            ListItem li = new ListItem(" ", "");
            DropDownList_EXUNIT.Items.Insert(0, li);
            if (ViewState["C_strGID"].ToString() == "E")//權限E選擇所屬單位並鎖單位
            {
                DropDownList_EXUNIT.SelectedValue = Session["ADPMZ_EXUNIT"].ToString();
                DropDownList_EXUNIT.Enabled = false;

            }
            if (ViewState["C_strGID"].ToString() == "C")//權限C選擇所屬單位並鎖單位
            {
                DropDownList_EXUNIT.SelectedValue = Session["ADPMZ_EXUNIT"].ToString();
                //DropDownList_EXUNIT.Enabled = false;

            }
        }

        protected void DropDownList_EXAD_SelectedIndexChanged(object sender, EventArgs e)
        {
            C.fill_unit(DropDownList_EXUNIT, DropDownList_EXAD.SelectedValue);
        }
    }
}
