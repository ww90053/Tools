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
    public partial class Personal_Gradenotice_rpt : System.Web.UI.Page
    {
        int TPM_FION=0;
        

        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!Page.IsPostBack)
            {//判斷使用者是否有權限進入系統
                A.check_power();
                 A.set_Panel_EnterToTAB(ref this.Panel1);
            A.set_Panel_EnterToTAB(ref this.Panel2);

 A.fill_AD_POST(DropDownList_MZ_AD);
                DropDownList_MZ_AD.DataBind();
                DropDownList_MZ_AD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                A.fill_unit(DropDownList_MZ_UNIT, DropDownList_MZ_AD.SelectedValue);
                
                ViewState["A_strGID"] = o_a_Function.strGID(Session["ADPMZ_ID"].ToString());

                
                
                chk_TPMGroup();

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

                    break;
                case "C":
                    //matthew 中和分局進來要加中和一&中和二
                    if (Session["ADPMZ_EXAD"].ToString() == "382133600C")
                    {
                        DropDownList_MZ_AD.Items.Clear();
                        string strSQL = "";
                        strSQL = "SELECT  MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE = '04' AND MZ_KCODE in('382133400C','382133500C','382133600C')";
                        DataTable temp = new DataTable();
                        temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");

                        DropDownList_MZ_AD.DataSource = temp;

                        DropDownList_MZ_AD.DataTextField = "MZ_KCHI";
                        DropDownList_MZ_AD.DataValueField = "MZ_KCODE";
                        DropDownList_MZ_AD.DataBind();
                    }
                    else
                    {
                        DropDownList_MZ_AD.Enabled = false;
                    }
                    //DropDownList_MZ_AD.Enabled = false;
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
            string strPart = "";

            if (!string.IsNullOrEmpty(DropDownList_MZ_AD.SelectedValue) &&
                !string.IsNullOrEmpty(DropDownList_MZ_UNIT.SelectedValue))
            {
                strPart += " AND MZ_AD='" + DropDownList_MZ_AD.SelectedValue + "' AND MZ_UNIT='" + DropDownList_MZ_UNIT.SelectedValue + "'";
            }
            else if (!string.IsNullOrEmpty(DropDownList_MZ_AD.SelectedValue))
            {
                strPart += " AND MZ_AD='" + DropDownList_MZ_AD.SelectedValue + "'";
            }

            if (!string.IsNullOrEmpty(TextBox_MZ_DATE1.Text.Trim()) &&
                !string.IsNullOrEmpty(TextBox_MZ_DATE2.Text.Trim()))
            {
                strPart += " AND MZ_DATE>='" + TextBox_MZ_DATE1.Text.Trim().Replace("/", "").PadLeft(7, '0') +
                           "' AND MZ_DATE<='" + TextBox_MZ_DATE2.Text.Trim().Replace("/", "").PadLeft(7, '0') + "'";
            }
            else if (!string.IsNullOrEmpty(TextBox_MZ_DATE1.Text.Trim()))
            {
                strPart += " AND MZ_DATE='" + TextBox_MZ_DATE1.Text.Trim().Replace("/", "").PadLeft(7, '0') + "'";
            }

            string P4001 = "((SELECT COUNT(*) FROM A_PRK2 WHERE MZ_PRRST='4001' AND MZ_ID=BBB.MZ_ID" + strPart + ")+(SELECT COUNT(*) FROM A_PRK2 WHERE MZ_PRRST='4002' AND MZ_ID=BBB.MZ_ID" + strPart + ")*2)";
            string P4010 = "((SELECT COUNT(*) FROM A_PRK2 WHERE MZ_PRRST='4010' AND MZ_ID=BBB.MZ_ID" + strPart + ")+(SELECT COUNT(*) FROM A_PRK2 WHERE MZ_PRRST='4020' AND MZ_ID=BBB.MZ_ID" + strPart + ")*2)";
            string P4100 = "((SELECT COUNT(*) FROM A_PRK2 WHERE MZ_PRRST='4100' AND MZ_ID=BBB.MZ_ID" + strPart + ")+(SELECT COUNT(*) FROM A_PRK2 WHERE MZ_PRRST='4200' AND MZ_ID=BBB.MZ_ID" + strPart + ")*2)";
            string P5001 = "((SELECT COUNT(*) FROM A_PRK2 WHERE MZ_PRRST='5001' AND MZ_ID=BBB.MZ_ID" + strPart + ")+(SELECT COUNT(*) FROM A_PRK2 WHERE MZ_PRRST='5002' AND MZ_ID=BBB.MZ_ID" + strPart + ")*2)";
            string P5010 = "((SELECT COUNT(*) FROM A_PRK2 WHERE MZ_PRRST='5010' AND MZ_ID=BBB.MZ_ID" + strPart + ")+(SELECT COUNT(*) FROM A_PRK2 WHERE MZ_PRRST='5020' AND MZ_ID=BBB.MZ_ID" + strPart + ")*2)";
            string P5100 = "(SELECT COUNT(*) FROM A_PRK2 WHERE MZ_PRRST='5100' AND MZ_ID=BBB.MZ_ID" + strPart + ")";

            string strSQL = "SELECT '" + TextBox_MZ_DATE1.Text.Trim() + "' AS DATE1,'" + TextBox_MZ_DATE2.Text.Trim() + "' AS DATE2,TEMPDB.* " +
                            "FROM " +
                            "(SELECT " +
                                       "MZ_ID,MZ_NAME,MZ_DATE,MZ_AD,MZ_UNIT AS MZ_UNIT1," +
                                       "(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=MZ_UNIT AND MZ_KTYPE='25') AS MZ_UNIT," +
                                       "(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=MZ_OCCC AND MZ_KTYPE='26') AS MZ_OCCC," +
                                       "(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=MZ_PRRST AND MZ_KTYPE='24') AS MZ_PRRST," +
                                       "MZ_PRID,MZ_PRID1,MZ_PRCT,MZ_MEMO," +
                                       P4001 + " AS I4001," +
                                       P4010 + " AS I4010," +
                                       P4100 + " AS I4100," +
                                       P5001 + " AS I5001," +
                                       P5010 + " AS I5010," +
                                       P5100 + " AS I5100," +
                                       "(" + P4001 + "+" + P4010 + "*3+" + P4100 + "*9-" + P5001 + "-" + P5010 + "*3-" + P5100 + "*9) AS TOTAL FROM A_PRK2 BBB)  TEMPDB " +
                             "WHERE " +
                                      "TOTAL <=" + int.Parse(TextBox_MZ_5001.Text.Trim()) * -1 + strPart;

            //2013/12/02
            Session["RPT_SQL_A"] = strSQL;
         

            string year = TextBox_MZ_DATE2.Text.Trim().Replace("/", "").PadLeft(7, '0').Substring(0, 3);
            string month = TextBox_MZ_DATE2.Text.Trim().Replace("/", "").PadLeft(7, '0').Substring(3, 2);
            string day = TextBox_MZ_DATE2.Text.Trim().Replace("/", "").PadLeft(7, '0').Substring(5, 2);

            Session["DATE"] = TextBox_DATE.Text.Trim().Length == 7 ? TextBox_DATE.Text.Trim().Substring(0, 3) + "年" + TextBox_DATE.Text.Trim().Substring(3, 2) + "月" + TextBox_DATE.Text.Trim().Substring(3, 2) + "日" : TextBox_DATE.Text;

            Session["PRID"] = TextBox_PRID.Text;

            Session["TITLE"] = string.Format("{0}{1}年功過相抵申誡以上通知書", DropDownList_MZ_AD.SelectedItem.Text, int.Parse(year).ToString());

            Session["MEMO"] = "台端  " + year + "年度平時考績獎懲互相抵銷後,至 " + year + " 年 " + month + " 月 " + day + " 日";

            //發文日期
            string year1 = TextBox_MZ_DATE1.Text.Trim().Replace("/", "").PadLeft(7, '0').Substring(0, 3);
            string month1 = TextBox_MZ_DATE1.Text.Trim().Replace("/", "").PadLeft(7, '0').Substring(3, 2);
            string day1 = TextBox_MZ_DATE1.Text.Trim().Replace("/", "").PadLeft(7, '0').Substring(5, 2);
            Session["DATE1"] = year1 + " 年 " + month1 + " 月 " + day1 + " 日";

            string tmp_url = "A_rpt.aspx?fn=gradenotice&TPM_FION=" + TPM_FION;

            ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);

        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            TextBox_MZ_5001.Text = string.Empty;
            TextBox_PRID.Text = string.Empty;
            TextBox_MZ_DATE1.Text = string.Empty;
            TextBox_MZ_DATE2.Text = string.Empty;
            TextBox_DATE.Text = string.Empty;
        }

        protected void returnSameDataType(TextBox tb, object ob1)
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

        protected void TextBox_MZ_DATE1_TextChanged(object sender, EventArgs e)
        {
            returnSameDataType(TextBox_MZ_DATE1, TextBox_MZ_DATE2);
        }

        protected void TextBox_MZ_DATE2_TextChanged(object sender, EventArgs e)
        {
            returnSameDataType(TextBox_MZ_DATE2, TextBox_MZ_5001);
        }

        protected void DropDownList_MZ_UNIT_DataBound(object sender, EventArgs e)
        {
            ListItem li = new ListItem(" ", "");
            DropDownList_MZ_UNIT.Items.Insert(0, li);
        }

        protected void DropDownList_MZ_AD_SelectedIndexChanged(object sender, EventArgs e)
        {
            A.fill_unit(DropDownList_MZ_UNIT, DropDownList_MZ_AD.SelectedValue);
        }
    }
}
