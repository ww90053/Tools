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
    public partial class C_offCFM_rpt : System.Web.UI.Page
    {
        int TPM_FION=0;
        

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                C.check_power();

                
            ///群組權限
            //C_strGID = o_a_Function.strGID(Session["ADPMZ_ID"].ToString());
            //MQ-----------------------20100331
            C.set_Panel_EnterToTAB(ref this.Panel1);
            C.set_Panel_EnterToTAB(ref this.Panel3);
            
                C.fill_AD(DropDownList_MZ_AD);
                C.fill_MZ_PRID(ListBox_PPRID, 2);
                C.fill_MZ_PRID(ListBox_CPRID, 2);
                TextBox_MZ_ID.Text = Session["ADPMZ_ID"].ToString();
            }
        }

        protected void btPrint_Click(object sender, EventArgs e)
        {
            string strSQL = "SELECT * FROM C_DLTB01 WHERE MZ_CHK1='Y'  AND (MZ_TDAY>0 OR MZ_TTIME>0) AND MZ_IDATE1='" + TextBox_MZ_DATE.Text.Trim().PadLeft(7, '0') + "' AND MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "'";

            if (TextBox_MZ_NAME.Text != "")
            {
                strSQL += " AND MZ_NAME='" + o_str.tosql(TextBox_MZ_NAME.Text.Trim()) + "'";
            }

            DataTable offCFM = new DataTable();

            offCFM = o_DBFactory.ABC_toTest.Create_Table(strSQL, "offCFM");

            if (offCFM.Rows.Count > 0)
            {
                if (DateManange.Check_date(TextBox_PDATE.Text) && DateManange.Check_date(TextBox_CDATE.Text))
                {
                    TextBox_PDATE.Text = TextBox_PDATE.Text.PadLeft(7, '0');
                    TextBox_CDATE.Text = TextBox_CDATE.Text.PadLeft(7, '0');
                    Session["TITLE"] = string.Format("{0}員警請假核定通知書", o_A_KTYPE.RAD(Session["ADPMZ_EXAD"].ToString()));
                    Session["offCM_DATE1"] = string.IsNullOrEmpty(TextBox_PDATE.Text) ? "" : (TextBox_PDATE.Text.Substring(0, 3) + "年" + TextBox_PDATE.Text.Substring(3, 2) + "月" + TextBox_PDATE.Text.Substring(5, 2) + "日");
                    Session["offCM_DATE2"] = string.IsNullOrEmpty(TextBox_CDATE.Text) ? "" : (TextBox_CDATE.Text.Substring(0, 3) + "年" + TextBox_CDATE.Text.Substring(3, 2) + "月" + TextBox_CDATE.Text.Substring(5, 2) + "日");
                    Session["offCM_PRID1"] = ListBox_PPRID.SelectedItem.Text + "字第" + TextBox_PNO.Text + "號";
                    Session["offCM_PRID2"] = ListBox_CPRID.SelectedItem.Text + "字第" + TextBox_CNO.Text + "號";
                    Session["CM_AD"] = DropDownList_MZ_AD.SelectedItem.Text;
                    Session["rpt_dt"] = offCFM;

                    string tmp_url = "C_rpt.aspx?fn=offCFM&TPM_FION=" + TPM_FION;

                    ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "alert('請檢查來文日期及發文日期');", true);
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "alert('無此日期之核定通過相關資料');", true);

            }
        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            TextBox_MZ_DATE.Text = string.Empty;
            TextBox_MZ_ID.Text = string.Empty;
            TextBox_MZ_NAME.Text = string.Empty;
        }

    }
}
