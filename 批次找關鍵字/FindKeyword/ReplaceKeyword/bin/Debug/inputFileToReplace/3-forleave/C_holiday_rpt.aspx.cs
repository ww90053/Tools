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
    public partial class C_holiday_rpt : System.Web.UI.Page
    {
        int TPM_FION=0;
        

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                C.check_power();
                     
                ViewState["C_strGID"] =o_a_Function.strGID(Session["ADPMZ_ID"].ToString());

          
            //MQ-----------------------20100331
            C.set_Panel_EnterToTAB(ref this.Panel1);
            
                TextBox_MZ_ID.Text = Session["ADPMZ_ID"].ToString();
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
                case "C":
                case "E":
                    break;
                case "D":
                    TextBox_MZ_ID.ReadOnly = true;
                    break;
                
            }
        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            TextBox_MZ_DATE1.Text = string.Empty;
            TextBox_MZ_DATE2.Text = string.Empty;
            TextBox_MZ_ID.Text = string.Empty;
        }

        protected void btPrint_Click(object sender, EventArgs e)
        {

            //if (string.IsNullOrEmpty(TextBox_MZ_DATE1.Text) || string.IsNullOrEmpty(TextBox_MZ_DATE2.Text))
            //{
            //    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('日期區間不可空白！')", true);
            //    return;
            
            //}
            string strSQL = "SELECT * FROM (SELECT MZ_ID,MZ_NAME,MZ_EXAD,MZ_EXUNIT,MZ_OCCC ,(SELECT MZ_CNAME FROM C_DLCODE WHERE C_DLCODE.MZ_CODE=C_DLTB01.MZ_CODE) MZ_CODE,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=MZ_RANK1 AND MZ_KTYPE='09') MZ_RANK1,MZ_SYSDAY,MZ_IDATE1,MZ_ITIME1,MZ_ODATE,MZ_OTIME,MZ_TDAY,MZ_TTIME,MZ_CAUSE,MZ_MEMO,MZ_SWT,MZ_TADD,MZ_RNAME,MZ_ROCCC,MZ_CHK1,MZ_SYSTIME,MZ_FOREIGN,MZ_CHINA,(SELECT MZ_AD FROM A_DLBASE WHERE MZ_ID=C_DLTB01.MZ_ID)  AS MZ_AD  " +
                            "FROM C_DLTB01) ";

            string up = string.Empty;
            List<string> condition = new List<string>();
            TextBox_MZ_DATE1.Text = TextBox_MZ_DATE1.Text.PadLeft(7, '0');
            TextBox_MZ_DATE2.Text = TextBox_MZ_DATE2.Text.PadLeft(7, '0');
            if (TextBox_MZ_DATE1.Text.Trim() != string.Empty)
            {
                if (DateManange.Check_date(TextBox_MZ_DATE1.Text) && DateManange.Check_date(TextBox_MZ_DATE2.Text))
                {
                    up += "統計日期： " + TextBox_MZ_DATE1.Text.Substring(0, 3) + "年" + TextBox_MZ_DATE1.Text.Substring(3, 2) + "月" + TextBox_MZ_DATE1.Text.Substring(5, 2) + "日";

                    condition.Add("MZ_IDATE1 >= '" + o_str.tosql(TextBox_MZ_DATE1.Text.Replace("/", string.Empty)) + "'");
                    if (TextBox_MZ_DATE2.Text.Trim() != string.Empty)
                    {
                        condition.Add("MZ_IDATE1 <= '" + o_str.tosql(TextBox_MZ_DATE2.Text.Replace("/", string.Empty)) + "'");
                        up += "至" + TextBox_MZ_DATE2.Text.Substring(0, 3) + "年" + TextBox_MZ_DATE2.Text.Substring(3, 2) + "月" + TextBox_MZ_DATE2.Text.Substring(5, 2) + "日";
                    }
                    else
                    {
                        condition.Add("MZ_IDATE1 <= '" + o_str.tosql(TextBox_MZ_DATE1.Text.Replace("/", string.Empty)) + "'");
                        up += "至" + TextBox_MZ_DATE1.Text.Substring(0, 3) + "年" + TextBox_MZ_DATE1.Text.Substring(3, 2) + "月" + TextBox_MZ_DATE1.Text.Substring(5, 2) + "日";
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "alert('請檢查欲查詢之起迄日期');", true);
                }
            }

            if (TextBox_MZ_ID.Text.Trim() != string.Empty)
            {
                condition.Add("MZ_ID='" + o_str.tosql(TextBox_MZ_ID.Text) + "'");
            }

            switch (ViewState["C_strGID"].ToString())
            {
                case "A":
                case "B":
                case "D":
                    break;
                case "C":
                    condition.Add("(MZ_EXAD='" + Session["ADPMZ_EXAD"].ToString() + "' OR MZ_AD='" + Session["ADPMZ_EXAD"].ToString() + "')");
                    break;
                
                case "E":
                    condition.Add("MZ_EXAD='" + Session["ADPMZ_EXAD"].ToString() + "' AND MZ_EXUNIT='" + Session["ADPMZ_EXUNIT"].ToString() + "'");
                    break;
            }

            condition.Add("(MZ_TDAY>0 OR MZ_TTIME>0)");

            string where = (condition.Count > 0 ? " WHERE " + string.Join(" AND ", condition.ToArray()) : string.Empty);

            strSQL += where;

            DataTable holiday = new DataTable();

            holiday = o_DBFactory.ABC_toTest.Create_Table(strSQL, "DETAIL");

            Session["RPT_SQL_C"] = strSQL;

            if (holiday.Rows.Count > 0)
            {
               
                Session["HOLIDAY_UP"] = up;
             
                string tmp_url = "C_rpt.aspx?fn=holiday&TPM_FION=" + TPM_FION;

                ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);

            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "alert('無此日期之相關資料');", true);

            }

        }

    }
}
