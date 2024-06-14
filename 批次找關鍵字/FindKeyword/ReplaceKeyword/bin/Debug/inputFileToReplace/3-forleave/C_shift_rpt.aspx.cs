using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TPPDDB._3_forleave
{
    public partial class C_shift_rpt : System.Web.UI.Page
    {
        int TPM_FION=0;
        

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                C.check_power();
            
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
            switch (o_a_Function.strGID(Session["ADPMZ_ID"].ToString()))
            {
                case "A":
                case "B":
                case "C":                   
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
           

            List<string> condition = new List<string>();
            if (!string.IsNullOrEmpty(TextBox_MZ_IDATE1.Text) && !string.IsNullOrEmpty(TextBox_MZ_IDATE1_1.Text))
            {
                condition.Add("MZ_IDATE1>='" + TextBox_MZ_IDATE1.Text.Trim().PadLeft(7, '0') + "' AND MZ_IDATE1<='" + TextBox_MZ_IDATE1_1.Text.Trim().PadLeft(7, '0') + "'");
            }

            if (TextBox_MZ_ID.Text != string.Empty)
            {
                condition.Add("CD.MZ_ID='" + o_str.tosql(TextBox_MZ_ID.Text.Trim()) + "'");
            }
           

            string where = (condition.Count > 0 ? " AND " + string.Join(" AND ", condition.ToArray()) : string.Empty);

            string strSQL = string.Format(@"SELECT MZ_HTIME,
(SELECT SUM(MZ_TDAY) FROM C_DLTB01 WHERE MZ_CODE='03' AND MZ_ID=CD.MZ_ID AND MZ_IDATE1>='{0}' AND MZ_IDATE1<='{1}') DAY_USED,
(SELECT SUM(MZ_TTIME) FROM C_DLTB01 WHERE MZ_CODE='03' AND MZ_ID=CD.MZ_ID AND MZ_IDATE1>='{0}' AND MZ_IDATE1<='{1}') HOUR_USED,
MZ_SDAY,MZ_SDAY_HOUR,MZ_SDAY2,MZ_SDAY2_HOUR,MZ_HDAY,CD.MZ_ID,CD.MZ_NAME,
MZ_EXAD_CH MZ_EXAD,
MZ_EXUNIT_CH MZ_EXUNIT,
MZ_OCCC_CH MZ_OCCC ,
MZ_CODE_CH MZ_CODE,
MZ_IDATE1,MZ_ITIME1, A_DLBASE.MZ_ODATE,MZ_OTIME,MZ_TDAY,MZ_TTIME,MZ_CAUSE, A_DLBASE.MZ_MEMO 
,MZ_FDATE,MZ_POLNO,MZ_TYEAR,MZ_TMONTH,MZ_RYEAR,MZ_RMONTH,MZ_OFFYY,MZ_OFFMM,' ' as DOWN 
FROM VW_C_DLTB01 CD,C_DLTBB CBB 
LEFT JOIN A_DLBASE ON CBB.MZ_ID = A_DLBASE.MZ_ID
WHERE (CD.MZ_TDAY>0 OR CD.MZ_TTIME>0) AND  CD.MZ_CHK1='Y' AND CD.MZ_ID = CBB.MZ_ID AND CBB.MZ_YEAR='{2}'   {3}
ORDER BY MZ_IDATE1
", TextBox_MZ_IDATE1.Text.Trim().PadLeft(7, '0'), TextBox_MZ_IDATE1_1.Text.Trim().PadLeft(7, '0'), TextBox_MZ_IDATE1_1.Text.Trim().PadLeft(7, '0').Substring(0, 3), where);
            DataTable shift = new DataTable();
            shift = o_DBFactory.ABC_toTest.Create_Table(strSQL, "DETAIL");

            ////已休未休
            ////抓應休天數及已休天數
           
           Session["RPT_C_IDNO"] = TextBox_MZ_ID.Text;

           if (shift.Rows.Count == 0)
           {

               string tmp_url = "C_rpt.aspx?fn=shift1&YEAR=" + TextBox_MZ_IDATE1.Text.Trim().PadLeft(7, '0') + "&TPM_FION=" + TPM_FION;
               ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
           }
           else
           {

               string tmp_url = "C_rpt.aspx?fn=shift&YEAR=" + TextBox_MZ_IDATE1.Text.Trim().PadLeft(7, '0') + "&YEAR2=" + TextBox_MZ_IDATE1_1.Text.Trim().PadLeft(7, '0') + "&TPM_FION=" + TPM_FION;
               ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
           }
        }


        protected List<string> DLTB_DAYS_COUNT(string MZ_ID)
        {
            List<string> result = new List<string>();

            string selectSQL = "SELECT MZ_FDATE,MZ_TYEAR,MZ_TMONTH,MZ_RYEAR,MZ_RMONTH,MZ_MEMO FROM A_DLBASE WHERE MZ_ID='" + MZ_ID + "'";

            DataTable temp = new DataTable();

            temp = o_DBFactory.ABC_toTest.Create_Table(selectSQL, "GET");

            int tyear = int.Parse(string.IsNullOrEmpty(temp.Rows[0]["MZ_TYEAR"].ToString()) ? "0" : temp.Rows[0]["MZ_TYEAR"].ToString());

            int tmonth = int.Parse(string.IsNullOrEmpty(temp.Rows[0]["MZ_TMONTH"].ToString()) ? "0" : temp.Rows[0]["MZ_TMONTH"].ToString());

            int ryear = int.Parse(string.IsNullOrEmpty(temp.Rows[0]["MZ_RYEAR"].ToString()) ? "0" : temp.Rows[0]["MZ_RYEAR"].ToString());

            int rmonth = int.Parse(string.IsNullOrEmpty(temp.Rows[0]["MZ_RMONTH"].ToString()) ? "0" : temp.Rows[0]["MZ_RMONTH"].ToString());


            string FDATE = temp.Rows[0]["MZ_FDATE"].ToString();

            if (string.IsNullOrEmpty(FDATE))
            {
                FDATE = (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0') + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0');
            }

            //任公職年資月數
            System.DateTime dt1 = DateTime.Parse((int.Parse(FDATE.Substring(0, 3)) + 1911).ToString() + "-" + FDATE.Substring(3, 2) + "-" + FDATE.Substring(5, 2));

            System.DateTime dt2 = DateTime.Now;

            int monthDiff = Convert.ToInt32(Microsoft.VisualBasic.DateAndTime.DateDiff("M", dt1, dt2, Microsoft.VisualBasic.FirstDayOfWeek.Sunday, Microsoft.VisualBasic.FirstWeekOfYear.FirstFullWeek)) + tyear * 12 + tmonth - ryear * 12 - rmonth;

            if (monthDiff <= 0)
            {
                result.Insert(0, "0");
                result.Insert(1, "0");
            }
            else
            {
                result.Insert(0, (monthDiff / 12).ToString());
                result.Insert(1, (monthDiff % 12).ToString());
            }

            return result;
        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            TextBox_MZ_IDATE1.Text = string.Empty;
            TextBox_MZ_IDATE1_1.Text = string.Empty;
            TextBox_MZ_ID.Text = string.Empty;
            //DropDownList_MZ_AD.Text = string.Empty;
        }

    }
}
