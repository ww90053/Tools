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
    public partial class C_askleave_rpt : System.Web.UI.Page
    {
        int TPM_FION=0;
        

        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (!Page.IsPostBack)
            {
                C.check_power();
                TextBox_MZ_ID.Text = Session["ADPMZ_ID"].ToString();


                C.fill_AD(DropDownList_MZ_AD);
                C.fill_MZ_PRID(ListBox_MZ_PRID, 1);

            
                //by MQ ------------------------------20100331           
                C.set_Panel_EnterToTAB(ref this.Panel1);           
                C.set_Panel_EnterToTAB(ref this.Panel2);
            }
           

        }

        protected void btPrint_Click(object sender, EventArgs e)
        {
            List<string> condition = new List<string>();
            if (TextBox_MZ_DATE.Text.Trim() != string.Empty)
            {
                condition.Add("MZ_IDATE1 = '" + TextBox_MZ_DATE.Text.Replace("/", string.Empty).PadLeft(7, '0') + "'");
            }

            if (TextBox_MZ_ID.Text.Trim() != string.Empty)
            {
                condition.Add("MZ_ID='" + TextBox_MZ_ID.Text + "'");
            }
            if (TextBox_MZ_NAME.Text.Trim() != string.Empty)
            {
                condition.Add("MZ_NAME='" + TextBox_MZ_NAME.Text + "'");
            }

            string where = (condition.Count > 0 ? " AND " + string.Join(" AND ", condition.ToArray()) : string.Empty);

            string strSQL = string.Format(
                            "SELECT AAA.*,ROWNUM AS NUM,'' AS DAY_USED,'' AS HOUR_USED" +
                            " FROM" +
                            " (" +
                            " SELECT MZ_ID,MZ_NAME," +
                            " (SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=MZ_EXAD AND MZ_KTYPE='04') MZ_EXAD," +
                            " (SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=MZ_EXUNIT AND MZ_KTYPE='25') MZ_EXUNIT," +
                            " MZ_OCCC ," +
                            " (SELECT MZ_CNAME FROM C_DLCODE WHERE C_DLCODE.MZ_CODE=C_DLTB01.MZ_CODE) MZ_CODE," +
                            " (SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=MZ_RANK1 AND MZ_KTYPE='09') MZ_RANK1,MZ_SYSDAY,MZ_IDATE1,MZ_ITIME1,MZ_ODATE,MZ_OTIME,MZ_TDAY,MZ_TTIME,MZ_CAUSE,MZ_MEMO,MZ_SWT,MZ_TADD,MZ_RNAME,MZ_ROCCC,MZ_CHK1,MZ_SYSTIME,MZ_FOREIGN,MZ_CHINA  " +
                            " FROM C_DLTB01" +
                            " WHERE (MZ_TDAY>0 OR MZ_TTIME>0) {0}" +
                            ") AAA"
                            , where);

            DataTable askleave = new DataTable();
            askleave = o_DBFactory.ABC_toTest.Create_Table(strSQL, "askleave");

            for (int i = 0; i < askleave.Rows.Count; i++)
            {
                C_COUNT_HDAY COUNT = new C_COUNT_HDAY()
                    {
                        //k = askleave.Rows[i]["MZ_ID"].ToString(),
                        r = askleave.Rows[i]["MZ_ID"].ToString()
                    };
                List<int> count_result = COUNT.COUNT_HDAY;
                askleave.Rows[i]["DAY_USED"] = count_result[0].ToString();
                askleave.Rows[i]["HOUR_USED"] = count_result[1].ToString();
                askleave.Rows[i]["MZ_OCCC"] = o_A_DLBASE.EXTPOS_OR_OCCC(askleave.Rows[i]["MZ_ID"].ToString(), askleave.Rows[i]["MZ_OCCC"].ToString());
            }
            if (askleave.Rows.Count > 0)
            {
                if (DateManange.Check_date(TextBox_PDATE.Text))
                {
                    Session["TO"] = DropDownList_MZ_AD.SelectedItem.Text;
                    Session["rpt_dt"] = askleave;
                    string date = TextBox_PDATE.Text.Replace("/", string.Empty).PadLeft(7, '0');
                    Session["DATE"] = int.Parse(date.Substring(0, 3)).ToString() + " 年 " + date.Substring(3, 2) + " 月 " + date.Substring(5, 2) + " 日 ";
                    Session["PRID"] = ListBox_MZ_PRID.SelectedItem.Text + "字第" + TextBox_NO.Text + "號";
                    Session["MEMO"] = TextBox_MEMO.Text;
                    
                    string tmp_url = "C_rpt.aspx?fn=askleave&TPM_FION=" + TPM_FION;
                    ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "alert('請檢查發文日期');", true);
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "alert('無此日期之相關資料');", true);

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
