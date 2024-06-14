using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Data;
using AjaxControlToolkit;


namespace TPPDDB._3_forleave
{
    public partial class C_ForLeaveOvertime_DUTYDAY : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                C.check_power();
            
     //by MQ 20100312---------   
            C.set_Panel_EnterToTAB(ref this.Panel1);       
            
                //2014/01/21

                string sql = @"SELECT AKD.MZ_KCHI MZ_EXAD,  AKU.MZ_KCHI MZ_EXUNIT
                             FROM A_DLBASE
                            LEFT JOIN  A_KTYPE AKD ON AKD.MZ_KCODE=MZ_EXAD AND AKD.MZ_KTYPE='04' 
                            LEFT JOIN  A_KTYPE AKU ON AKU.MZ_KCODE=MZ_EXUNIT AND AKU.MZ_KTYPE='25' 
                            WHERE MZ_ID='" + Session["ADPMZ_ID"].ToString() + "'";

                DataTable ad_unit = o_DBFactory.ABC_toTest.Create_Table(sql, "get");
                if (ad_unit.Rows.Count>0)
                Label1.Text = ad_unit.Rows[0]["MZ_EXAD"].ToString() + ad_unit.Rows[0]["MZ_EXUNIT"].ToString() + "每日勤務";

              

                TextBox_DUTYDATE.Text = o_CommonService.Personal_ReturnDateString((DateTime.Now.Year - 1911).ToString().PadLeft(3, '0') + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0'));
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string DutyDate = o_str.tosql(TextBox_DUTYDATE.Text.Trim().Replace("/", "").PadLeft(7, '0'));
            //20140731
            string StartTime = o_DBFactory.ABC_toTest.vExecSQL("SELECT DISTINCT dbo.SUBSTR(C_DUTYTABLE_PERSONAL.TIME1,2,1) as TIME1 FROM C_DUTYTABLE_PERSONAL WHERE MZ_AD='" + o_A_DLBASE.PAD(Session["ADPMZ_ID"].ToString()) + "' AND MZ_UNIT='" + o_A_DLBASE.PUNIT(Session["ADPMZ_ID"].ToString()) + "' AND DUTYDATE='" + DutyDate + "'");

            string strSQL;

            if (StartTime == "6")
            {
                strSQL = "SELECT '' PNO,AA.MZ_ID,(SELECT MZ_NAME FROM A_DLBASE WHERE MZ_ID=AA.MZ_ID) NAME,";
                for (int i = 1; i <= 26; i++)
                {
                    if (i == 26)
                    {
                        strSQL += "(SELECT DUTY_NAME FROM C_DUTYITEM WHERE DUTY_NO=(SELECT DUTYITEM" + i.ToString() + " FROM C_DUTYTABLE_PERSONAL WHERE DUTYDATE='" + DutyDate + "' AND MZ_ID=AA.MZ_ID)) as A" + i;
                    }
                    else
                    {
                        strSQL += "(SELECT DUTY_NAME FROM C_DUTYITEM WHERE DUTY_NO=(SELECT DUTYITEM" + i.ToString() + " FROM C_DUTYTABLE_PERSONAL WHERE DUTYDATE='" + DutyDate + "' AND MZ_ID=AA.MZ_ID)) as A" + i + ",";
                    }
                }
                //20140731
                strSQL += "  ,TOTAL_HOURS FROM C_DUTYTABLE_PERSONAL AA  WHERE  ISDIRECTTIME IS NULL AND MZ_AD='" + o_A_DLBASE.PAD(Session["ADPMZ_ID"].ToString()) + "' AND MZ_UNIT='" + o_A_DLBASE.PUNIT(Session["ADPMZ_ID"].ToString()) + "' AND DUTYDATE='" + DutyDate + "'";
            }
            else if (StartTime == "7")
            {
                strSQL = "SELECT '' PNO,AA.MZ_ID,(SELECT MZ_NAME FROM A_DLBASE WHERE MZ_ID=AA.MZ_ID) NAME,";
                for (int i = 1; i <= 25; i++)
                {
                    if (i == 25)
                    {
                        strSQL += "(SELECT DUTY_NAME FROM C_DUTYITEM WHERE DUTY_NO=(SELECT DUTYITEM" + i.ToString() + " FROM C_DUTYTABLE_PERSONAL WHERE DUTYDATE='" + DutyDate + "' AND MZ_ID=AA.MZ_ID)) as A" + i;
                    }
                    else
                    {
                        strSQL += "(SELECT DUTY_NAME FROM C_DUTYITEM WHERE DUTY_NO=(SELECT DUTYITEM" + i.ToString() + " FROM C_DUTYTABLE_PERSONAL WHERE DUTYDATE='" + DutyDate + "' AND MZ_ID=AA.MZ_ID)) as A" + i + ",";
                    }
                }
                //20140731
                strSQL += "  ,TOTAL_HOURS FROM C_DUTYTABLE_PERSONAL AA WHERE ISDIRECTTIME IS NULL AND MZ_AD='" + o_A_DLBASE.PAD(Session["ADPMZ_ID"].ToString()) + "' AND MZ_UNIT='" + o_A_DLBASE.PUNIT(Session["ADPMZ_ID"].ToString()) + "' AND DUTYDATE='" + DutyDate + "'";
            }
            else
            {
                strSQL = "SELECT '' PNO,AA.MZ_ID,(SELECT MZ_NAME FROM A_DLBASE WHERE MZ_ID=AA.MZ_ID) NAME,";
                for (int i = 1; i <= 24; i++)
                {
                    if (i == 24)
                    {
                        strSQL += "(SELECT DUTY_NAME FROM C_DUTYITEM WHERE DUTY_NO=(SELECT DUTYITEM" + i.ToString() + " FROM C_DUTYTABLE_PERSONAL WHERE DUTYDATE='" + DutyDate + "' AND MZ_ID=AA.MZ_ID)) as A" + i;
                    }
                    else
                    {
                        strSQL += "(SELECT DUTY_NAME FROM C_DUTYITEM WHERE DUTY_NO=(SELECT DUTYITEM" + i.ToString() + " FROM C_DUTYTABLE_PERSONAL WHERE DUTYDATE='" + DutyDate + "' AND MZ_ID=AA.MZ_ID)) as A" + i + ",";
                    }
                }
                //20140731
                strSQL += " ,TOTAL_HOURS FROM C_DUTYTABLE_PERSONAL AA WHERE  ISDIRECTTIME IS NULL AND MZ_AD='" + o_A_DLBASE.PAD(Session["ADPMZ_ID"].ToString()) + "' AND MZ_UNIT='" + o_A_DLBASE.PUNIT(Session["ADPMZ_ID"].ToString()) + "' AND DUTYDATE='" + DutyDate + "'";
            }

            DataTable temp = new DataTable();
            temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");
            if (temp.Rows.Count == 0)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('查無相關資料');", true);

            }
            else
            {
                foreach (DataRow dr in temp.Rows)
                {
                    dr["PNO"] = Service_C_DUTYPEOPLE.get_pno(Session["ADPMZ_EXAD"].ToString(), Session["ADPMZ_EXUNIT"].ToString(), dr["MZ_ID"].ToString(), DutyDate);
                }
                temp.Columns.Remove("MZ_ID");
                temp = temp.AsEnumerable().OrderBy(dr => dr["PNO"]).CopyToDataTable();
                ViewState["temp"] = temp;
                GridView1.DataSource = temp;
                GridView1.DataBind();
            }
        }

        protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
        {
            string DutyDate = TextBox_DUTYDATE.Text.Trim().Replace("/", "").PadLeft(7, '0');

            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridView gv = (GridView)sender;
                e.Row.Cells.Clear();
                GridViewRow gvRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
                string StartTime = o_DBFactory.ABC_toTest.vExecSQL("SELECT DISTINCT dbo.SUBSTR(C_DUTYTABLE.TIME1,2,1) AS TIME1 FROM C_DUTYTABLE WHERE MZ_AD='" + Session["ADPMZ_EXAD"].ToString() + "' AND MZ_UNIT='" + Session["ADPMZ_EXUNIT"].ToString() + "' AND DUTYDATE='" + DutyDate + "'");
                if (StartTime == "6")
                {
                    TableCell tc0 = new TableCell();
                    tc0.Text = "番號";
                    gvRow.Cells.Add(tc0);

                    TableCell tc1 = new TableCell();
                    tc1.Text = "姓名";
                    gvRow.Cells.Add(tc1);

                    TableCell tc2 = new TableCell();
                    tc2.Text = "6";
                    gvRow.Cells.Add(tc2);

                    TableCell tc3 = new TableCell();
                    tc3.Text = "7";
                    gvRow.Cells.Add(tc3);

                    TableCell tc4 = new TableCell();
                    tc4.Text = "8";
                    gvRow.Cells.Add(tc4);

                    TableCell tc5 = new TableCell();
                    tc5.Text = "9";
                    gvRow.Cells.Add(tc5);

                    TableCell tc6 = new TableCell();
                    tc6.Text = "10";
                    gvRow.Cells.Add(tc6);

                    TableCell tc7 = new TableCell();
                    tc7.Text = "11";
                    gvRow.Cells.Add(tc7);

                    TableCell tc8 = new TableCell();
                    tc8.Text = "12";
                    gvRow.Cells.Add(tc8);

                    TableCell tc9 = new TableCell();
                    tc9.Text = "13";
                    gvRow.Cells.Add(tc9);

                    TableCell tc10 = new TableCell();
                    tc10.Text = "14";
                    gvRow.Cells.Add(tc10);

                    TableCell tc11 = new TableCell();
                    tc11.Text = "15";
                    gvRow.Cells.Add(tc11);

                    TableCell tc12 = new TableCell();
                    tc12.Text = "16";
                    gvRow.Cells.Add(tc12);

                    TableCell tc13 = new TableCell();
                    tc13.Text = "17";
                    gvRow.Cells.Add(tc13);

                    TableCell tc14 = new TableCell();
                    tc14.Text = "18";
                    gvRow.Cells.Add(tc14);

                    TableCell tc15 = new TableCell();
                    tc15.Text = "19";
                    gvRow.Cells.Add(tc15);

                    TableCell tc16 = new TableCell();
                    tc16.Text = "20";
                    gvRow.Cells.Add(tc16);

                    TableCell tc17 = new TableCell();
                    tc17.Text = "21";
                    gvRow.Cells.Add(tc17);

                    TableCell tc18 = new TableCell();
                    tc18.Text = "22";
                    gvRow.Cells.Add(tc18);

                    TableCell tc19 = new TableCell();
                    tc19.Text = "23";
                    gvRow.Cells.Add(tc19);

                    TableCell tc20 = new TableCell();
                    tc20.Text = "24";
                    gvRow.Cells.Add(tc20);

                    TableCell tc21 = new TableCell();
                    tc21.Text = "1";
                    gvRow.Cells.Add(tc21);

                    TableCell tc22 = new TableCell();
                    tc22.Text = "2";
                    gvRow.Cells.Add(tc22);

                    TableCell tc23 = new TableCell();
                    tc23.Text = "3";
                    gvRow.Cells.Add(tc23);

                    TableCell tc24 = new TableCell();
                    tc24.Text = "4";
                    gvRow.Cells.Add(tc24);

                    TableCell tc25 = new TableCell();
                    tc25.Text = "5";
                    gvRow.Cells.Add(tc25);

                    TableCell tc26 = new TableCell();
                    tc26.Text = "6";
                    gvRow.Cells.Add(tc26);

                    TableCell t27 = new TableCell();
                    t27.Text = "7";
                    gvRow.Cells.Add(t27);

                    TableCell t277 = new TableCell();
                    t277.Text = "總時數";
                    gvRow.Cells.Add(t277);

                }
                else if (StartTime == "7")
                {
                    TableCell tc0 = new TableCell();
                    tc0.Text = "番號";
                    gvRow.Cells.Add(tc0);

                    TableCell tc1 = new TableCell();
                    tc1.Text = "姓名";
                    gvRow.Cells.Add(tc1);

                    TableCell tc3 = new TableCell();
                    tc3.Text = "7";
                    gvRow.Cells.Add(tc3);

                    TableCell tc4 = new TableCell();
                    tc4.Text = "8";
                    gvRow.Cells.Add(tc4);

                    TableCell tc5 = new TableCell();
                    tc5.Text = "9";
                    gvRow.Cells.Add(tc5);

                    TableCell tc6 = new TableCell();
                    tc6.Text = "10";
                    gvRow.Cells.Add(tc6);

                    TableCell tc7 = new TableCell();
                    tc7.Text = "11";
                    gvRow.Cells.Add(tc7);

                    TableCell tc8 = new TableCell();
                    tc8.Text = "12";
                    gvRow.Cells.Add(tc8);

                    TableCell tc9 = new TableCell();
                    tc9.Text = "13";
                    gvRow.Cells.Add(tc9);

                    TableCell tc10 = new TableCell();
                    tc10.Text = "14";
                    gvRow.Cells.Add(tc10);

                    TableCell tc11 = new TableCell();
                    tc11.Text = "15";
                    gvRow.Cells.Add(tc11);

                    TableCell tc12 = new TableCell();
                    tc12.Text = "16";
                    gvRow.Cells.Add(tc12);

                    TableCell tc13 = new TableCell();
                    tc13.Text = "17";
                    gvRow.Cells.Add(tc13);

                    TableCell tc14 = new TableCell();
                    tc14.Text = "18";
                    gvRow.Cells.Add(tc14);

                    TableCell tc15 = new TableCell();
                    tc15.Text = "19";
                    gvRow.Cells.Add(tc15);

                    TableCell tc16 = new TableCell();
                    tc16.Text = "20";
                    gvRow.Cells.Add(tc16);

                    TableCell tc17 = new TableCell();
                    tc17.Text = "21";
                    gvRow.Cells.Add(tc17);

                    TableCell tc18 = new TableCell();
                    tc18.Text = "22";
                    gvRow.Cells.Add(tc18);

                    TableCell tc19 = new TableCell();
                    tc19.Text = "23";
                    gvRow.Cells.Add(tc19);

                    TableCell tc20 = new TableCell();
                    tc20.Text = "24";
                    gvRow.Cells.Add(tc20);

                    TableCell tc21 = new TableCell();
                    tc21.Text = "1";
                    gvRow.Cells.Add(tc21);

                    TableCell tc22 = new TableCell();
                    tc22.Text = "2";
                    gvRow.Cells.Add(tc22);

                    TableCell tc23 = new TableCell();
                    tc23.Text = "3";
                    gvRow.Cells.Add(tc23);

                    TableCell tc24 = new TableCell();
                    tc24.Text = "4";
                    gvRow.Cells.Add(tc24);

                    TableCell tc25 = new TableCell();
                    tc25.Text = "5";
                    gvRow.Cells.Add(tc25);

                    TableCell tc26 = new TableCell();
                    tc26.Text = "6";
                    gvRow.Cells.Add(tc26);

                    TableCell t27 = new TableCell();
                    t27.Text = "7";
                    gvRow.Cells.Add(t27);

                    TableCell t277 = new TableCell();
                    t277.Text = "總時數";
                    gvRow.Cells.Add(t277);
                }
                else if (StartTime == "8")
                {
                    TableCell tc0 = new TableCell();
                    tc0.Text = "番號";
                    gvRow.Cells.Add(tc0);

                    TableCell tc1 = new TableCell();
                    tc1.Text = "姓名";
                    gvRow.Cells.Add(tc1);

                    TableCell tc4 = new TableCell();
                    tc4.Text = "8";
                    gvRow.Cells.Add(tc4);

                    TableCell tc5 = new TableCell();
                    tc5.Text = "9";
                    gvRow.Cells.Add(tc5);

                    TableCell tc6 = new TableCell();
                    tc6.Text = "10";
                    gvRow.Cells.Add(tc6);

                    TableCell tc7 = new TableCell();
                    tc7.Text = "11";
                    gvRow.Cells.Add(tc7);

                    TableCell tc8 = new TableCell();
                    tc8.Text = "12";
                    gvRow.Cells.Add(tc8);

                    TableCell tc9 = new TableCell();
                    tc9.Text = "13";
                    gvRow.Cells.Add(tc9);

                    TableCell tc10 = new TableCell();
                    tc10.Text = "14";
                    gvRow.Cells.Add(tc10);

                    TableCell tc11 = new TableCell();
                    tc11.Text = "15";
                    gvRow.Cells.Add(tc11);

                    TableCell tc12 = new TableCell();
                    tc12.Text = "16";
                    gvRow.Cells.Add(tc12);

                    TableCell tc13 = new TableCell();
                    tc13.Text = "17";
                    gvRow.Cells.Add(tc13);

                    TableCell tc14 = new TableCell();
                    tc14.Text = "18";
                    gvRow.Cells.Add(tc14);

                    TableCell tc15 = new TableCell();
                    tc15.Text = "19";
                    gvRow.Cells.Add(tc15);

                    TableCell tc16 = new TableCell();
                    tc16.Text = "20";
                    gvRow.Cells.Add(tc16);

                    TableCell tc17 = new TableCell();
                    tc17.Text = "21";
                    gvRow.Cells.Add(tc17);

                    TableCell tc18 = new TableCell();
                    tc18.Text = "22";
                    gvRow.Cells.Add(tc18);

                    TableCell tc19 = new TableCell();
                    tc19.Text = "23";
                    gvRow.Cells.Add(tc19);

                    TableCell tc20 = new TableCell();
                    tc20.Text = "24";
                    gvRow.Cells.Add(tc20);

                    TableCell tc21 = new TableCell();
                    tc21.Text = "1";
                    gvRow.Cells.Add(tc21);

                    TableCell tc22 = new TableCell();
                    tc22.Text = "2";
                    gvRow.Cells.Add(tc22);

                    TableCell tc23 = new TableCell();
                    tc23.Text = "3";
                    gvRow.Cells.Add(tc23);

                    TableCell tc24 = new TableCell();
                    tc24.Text = "4";
                    gvRow.Cells.Add(tc24);

                    TableCell tc25 = new TableCell();
                    tc25.Text = "5";
                    gvRow.Cells.Add(tc25);

                    TableCell tc26 = new TableCell();
                    tc26.Text = "6";
                    gvRow.Cells.Add(tc26);

                    TableCell t27 = new TableCell();
                    t27.Text = "7";
                    gvRow.Cells.Add(t27);

                    TableCell t277 = new TableCell();
                    t277.Text = "總時數";
                    gvRow.Cells.Add(t277);
                }
                else if (StartTime == "9")
                {
                    TableCell tc0 = new TableCell();
                    tc0.Text = "番號";
                    gvRow.Cells.Add(tc0);

                    TableCell tc1 = new TableCell();
                    tc1.Text = "姓名";
                    gvRow.Cells.Add(tc1);

                    TableCell tc5 = new TableCell();
                    tc5.Text = "9";
                    gvRow.Cells.Add(tc5);

                    TableCell tc6 = new TableCell();
                    tc6.Text = "10";
                    gvRow.Cells.Add(tc6);

                    TableCell tc7 = new TableCell();
                    tc7.Text = "11";
                    gvRow.Cells.Add(tc7);

                    TableCell tc8 = new TableCell();
                    tc8.Text = "12";
                    gvRow.Cells.Add(tc8);

                    TableCell tc9 = new TableCell();
                    tc9.Text = "13";
                    gvRow.Cells.Add(tc9);

                    TableCell tc10 = new TableCell();
                    tc10.Text = "14";
                    gvRow.Cells.Add(tc10);

                    TableCell tc11 = new TableCell();
                    tc11.Text = "15";
                    gvRow.Cells.Add(tc11);

                    TableCell tc12 = new TableCell();
                    tc12.Text = "16";
                    gvRow.Cells.Add(tc12);

                    TableCell tc13 = new TableCell();
                    tc13.Text = "17";
                    gvRow.Cells.Add(tc13);

                    TableCell tc14 = new TableCell();
                    tc14.Text = "18";
                    gvRow.Cells.Add(tc14);

                    TableCell tc15 = new TableCell();
                    tc15.Text = "19";
                    gvRow.Cells.Add(tc15);

                    TableCell tc16 = new TableCell();
                    tc16.Text = "20";
                    gvRow.Cells.Add(tc16);

                    TableCell tc17 = new TableCell();
                    tc17.Text = "21";
                    gvRow.Cells.Add(tc17);

                    TableCell tc18 = new TableCell();
                    tc18.Text = "22";
                    gvRow.Cells.Add(tc18);

                    TableCell tc19 = new TableCell();
                    tc19.Text = "23";
                    gvRow.Cells.Add(tc19);

                    TableCell tc20 = new TableCell();
                    tc20.Text = "24";
                    gvRow.Cells.Add(tc20);

                    TableCell tc21 = new TableCell();
                    tc21.Text = "1";
                    gvRow.Cells.Add(tc21);

                    TableCell tc22 = new TableCell();
                    tc22.Text = "2";
                    gvRow.Cells.Add(tc22);

                    TableCell tc23 = new TableCell();
                    tc23.Text = "3";
                    gvRow.Cells.Add(tc23);

                    TableCell tc24 = new TableCell();
                    tc24.Text = "4";
                    gvRow.Cells.Add(tc24);

                    TableCell tc25 = new TableCell();
                    tc25.Text = "5";
                    gvRow.Cells.Add(tc25);

                    TableCell tc26 = new TableCell();
                    tc26.Text = "6";
                    gvRow.Cells.Add(tc26);

                    TableCell t27 = new TableCell();
                    t27.Text = "7";
                    gvRow.Cells.Add(t27);

                    TableCell t28 = new TableCell();
                    t28.Text = "8";
                    gvRow.Cells.Add(t28);

                    TableCell t277 = new TableCell();
                    t277.Text = "總時數";
                    gvRow.Cells.Add(t277);
                }
                gv.Controls[0].Controls.AddAt(0, gvRow);
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            if (ViewState["temp"] != null)
            {
                DataTable temp = ViewState["temp"] as DataTable;
                Session["rpt_dt"] = temp;
                string DutyDate = o_str.tosql(TextBox_DUTYDATE.Text.Trim().Replace("/", "").PadLeft(7, '0'));
                string dutyd = TextBox_DUTYDATE.Text.Replace("/", string.Empty);
                string StartTime = o_DBFactory.ABC_toTest.vExecSQL("SELECT DISTINCT dbo.SUBSTR(C_DUTYTABLE.TIME1,2,1) AS TIME1 FROM C_DUTYTABLE WHERE MZ_AD='" + Session["ADPMZ_EXAD"].ToString() + "' AND MZ_UNIT='" + Session["ADPMZ_EXUNIT"].ToString() + "' AND DUTYDATE='" + DutyDate + "'");
                string tmp_url = string.Empty;
                Session["TITLE"] = string.Format("{0}{1}{2}個人勤務明細表", o_A_KTYPE.RAD(Session["ADPMZ_EXAD"].ToString()), o_A_KTYPE.RUNIT(Session["ADPMZ_EXUNIT"].ToString()), string.Format("{0}年{1}月{2}日", dutyd.Substring(0, 2), dutyd.Substring(2, 2), dutyd.Substring(4, 2)));
                
                //20140122

                tmp_url = "C_rpt.aspx?fn=dutyday" + StartTime + "&TPM_FION=" + 0;
                //switch (StartTime)
                //{
                //    case "6":
                //        tmp_url = "C_rpt.aspx?fn=dutyday6&TPM_FION=" + 0;
                //        break;
                //    case "7":
                //        tmp_url = "C_rpt.aspx?fn=dutyday7&TPM_FION=" + 0;
                //        break;
                //    case "8":
                //        tmp_url = "C_rpt.aspx?fn=dutyday8&TPM_FION=" + 0;
                //        break;
                //    case "9":
                //        tmp_url = "C_rpt.aspx?fn=dutyday9&TPM_FION=" + 0;
                //        break;
                //}
                ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('查無相關資料');", true);
            }
        }
    }
}
