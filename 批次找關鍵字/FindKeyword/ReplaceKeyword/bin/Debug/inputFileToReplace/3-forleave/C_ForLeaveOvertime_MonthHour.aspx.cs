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
    public partial class C_ForLeaveOvertime_MonthHour : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                C.check_power();
            

   
                //MQ-----------------------20100331
            C.set_Panel_EnterToTAB(ref this.Panel2);
            
                TextBox_DUTYYEAR.Text = (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0');
                DropDownList_DUTYMONTH.SelectedValue = DateTime.Now.Month.ToString();

                string sql = @"SELECT AKD.MZ_KCHI MZ_EXAD,  AKU.MZ_KCHI MZ_EXUNIT
                             FROM A_DLBASE
                            LEFT JOIN  A_KTYPE AKD ON AKD.MZ_KCODE=MZ_EXAD AND AKD.MZ_KTYPE='04' 
                            LEFT JOIN  A_KTYPE AKU ON AKU.MZ_KCODE=MZ_EXUNIT AND AKU.MZ_KTYPE='25' 
                            WHERE MZ_ID='" + Session["ADPMZ_ID"].ToString() + "'";

                DataTable ad_unit = o_DBFactory.ABC_toTest.Create_Table(sql, "get");
                if (ad_unit.Rows.Count > 0)
                    Label1.Text = ad_unit.Rows[0]["MZ_EXAD"].ToString() + ad_unit.Rows[0]["MZ_EXUNIT"].ToString() + "每月上班時數表";
            
            }
        }

        protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
        {
            string MonthLastDay = "";
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (!string.IsNullOrEmpty(DropDownList_DUTYMONTH.SelectedValue))
                {
                    MonthLastDay = (DateTime.Parse((int.Parse(TextBox_DUTYYEAR.Text.Trim()) + 1911).ToString() + "/" + DropDownList_DUTYMONTH.SelectedValue.Trim().PadLeft(2, '0') + "/" + "01").AddMonths(1).AddDays(-1).Day).ToString();
                }
                GridView gv = (GridView)sender;
                e.Row.Cells.Clear();
                GridViewRow gvRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

                TableCell tc00 = new TableCell();
                tc00.Text = "番號";
                gvRow.Cells.Add(tc00);

                TableCell tc0 = new TableCell();
                tc0.Text = "姓名";
                gvRow.Cells.Add(tc0);

                TableCell tc1 = new TableCell();
                tc1.Text = "1";
                gvRow.Cells.Add(tc1);

                TableCell tc2 = new TableCell();
                tc2.Text = "2";
                gvRow.Cells.Add(tc2);

                TableCell tc3 = new TableCell();
                tc3.Text = "3";
                gvRow.Cells.Add(tc3);

                TableCell tc4 = new TableCell();
                tc4.Text = "4";
                gvRow.Cells.Add(tc4);

                TableCell tc5 = new TableCell();
                tc5.Text = "5";
                gvRow.Cells.Add(tc5);

                TableCell tc6 = new TableCell();
                tc6.Text = "6";
                gvRow.Cells.Add(tc6);

                TableCell tc7 = new TableCell();
                tc7.Text = "7";
                gvRow.Cells.Add(tc7);

                TableCell tc8 = new TableCell();
                tc8.Text = "8";
                gvRow.Cells.Add(tc8);

                TableCell tc9 = new TableCell();
                tc9.Text = "9";
                gvRow.Cells.Add(tc9);

                TableCell tc10 = new TableCell();
                tc10.Text = "10";
                gvRow.Cells.Add(tc10);

                TableCell tc11 = new TableCell();
                tc11.Text = "11";
                gvRow.Cells.Add(tc11);

                TableCell tc12 = new TableCell();
                tc12.Text = "12";
                gvRow.Cells.Add(tc12);

                TableCell tc13 = new TableCell();
                tc13.Text = "13";
                gvRow.Cells.Add(tc13);

                TableCell tc14 = new TableCell();
                tc14.Text = "14";
                gvRow.Cells.Add(tc14);

                TableCell tc15 = new TableCell();
                tc15.Text = "15";
                gvRow.Cells.Add(tc15);

                TableCell tc16 = new TableCell();
                tc16.Text = "16";
                gvRow.Cells.Add(tc16);

                TableCell tc17 = new TableCell();
                tc17.Text = "17";
                gvRow.Cells.Add(tc17);

                TableCell tc18 = new TableCell();
                tc18.Text = "18";
                gvRow.Cells.Add(tc18);

                TableCell tc19 = new TableCell();
                tc19.Text = "19";
                gvRow.Cells.Add(tc19);

                TableCell tc20 = new TableCell();
                tc20.Text = "20";
                gvRow.Cells.Add(tc20);

                TableCell tc21 = new TableCell();
                tc21.Text = "21";
                gvRow.Cells.Add(tc21);

                TableCell tc22 = new TableCell();
                tc22.Text = "22";
                gvRow.Cells.Add(tc22);

                TableCell tc23 = new TableCell();
                tc23.Text = "23";
                gvRow.Cells.Add(tc23);

                TableCell tc24 = new TableCell();
                tc24.Text = "24";
                gvRow.Cells.Add(tc24);

                TableCell tc25 = new TableCell();
                tc25.Text = "25";
                gvRow.Cells.Add(tc25);

                TableCell tc26 = new TableCell();
                tc26.Text = "26";
                gvRow.Cells.Add(tc26);

                TableCell tc27 = new TableCell();
                tc27.Text = "27";
                gvRow.Cells.Add(tc27);

                TableCell tc28 = new TableCell();
                tc28.Text = "28";
                gvRow.Cells.Add(tc28);

                TableCell tc29 = new TableCell();
                tc29.Text = "29";
                gvRow.Cells.Add(tc29);

                TableCell tc30 = new TableCell();
                tc30.Text = "30";
                gvRow.Cells.Add(tc30);

                TableCell tc31 = new TableCell();
                tc31.Text = "31";
                gvRow.Cells.Add(tc31);

                TableCell tc32 = new TableCell();
                tc32.Text = "合計";
                gvRow.Cells.Add(tc32);

                gv.Controls[0].Controls.AddAt(0, gvRow);
            }
        }
        //就是......組SQL語法塞資料.....
        protected void Button1_Click(object sender, EventArgs e)
        {
            string MonthLastDay = (DateTime.Parse((int.Parse(TextBox_DUTYYEAR.Text.Trim()) + 1911).ToString() + "/" + DropDownList_DUTYMONTH.SelectedValue.Trim().PadLeft(2, '0') + "/" + "01").AddMonths(1).AddDays(-1).Day).ToString();

            string strSQL = "";

            if (MonthLastDay == "28")
            {
                strSQL = "SELECT DISTINCT B.MZ_ID,'" + TextBox_DUTYYEAR.Text.Trim().PadLeft(3, '0') + "','" + DropDownList_DUTYMONTH.SelectedValue.Trim().PadLeft(2, '0') + "',";
                for (int i = 1; i <= 28; i++)
                {
                    if (i == 28)
                    {
                        strSQL += "(SELECT TOTAL_HOURS FROM C_DUTYTABLE_PERSONAL WHERE MZ_ID=B.MZ_ID AND DUTYDATE='"
                                    + TextBox_DUTYYEAR.Text.Trim().PadLeft(3, '0') + DropDownList_DUTYMONTH.SelectedValue.Trim().PadLeft(2, '0') + i.ToString().PadLeft(2, '0') + "' ),NULL,NULL,NULL,"
                                    + "(SELECT SUM(TOTAL_HOURS) FROM C_DUTYTABLE_PERSONAL WHERE MZ_ID=B.MZ_ID AND dbo.SUBSTR(DUTYDATE,1,5)='" + TextBox_DUTYYEAR.Text.Trim().PadLeft(3, '0')
                                    + DropDownList_DUTYMONTH.SelectedValue.Trim().PadLeft(2, '0') + "' GROUP BY MZ_ID) ";
                    }
                    else
                    {
                        strSQL += "(SELECT TOTAL_HOURS FROM C_DUTYTABLE_PERSONAL WHERE   ISDIRECTTIME IS NULL AND  MZ_ID=B.MZ_ID AND DUTYDATE='"
                                   + TextBox_DUTYYEAR.Text.Trim().PadLeft(3, '0') + DropDownList_DUTYMONTH.SelectedValue.Trim().PadLeft(2, '0') + i.ToString().PadLeft(2, '0') + "' ),";
                    }
                }

                strSQL += "FROM C_DUTYTABLE_PERSONAL A,A_DLBASE B WHERE  ISDIRECTTIME IS NULL AND  A.MZ_ID=B.MZ_ID AND A.DUTYDATE>='" + TextBox_DUTYYEAR.Text.Trim().PadLeft(3, '0') + DropDownList_DUTYMONTH.SelectedValue.Trim().PadLeft(2, '0') + "01'"
                                         + " AND A.DUTYDATE<='" + TextBox_DUTYYEAR.Text.Trim().PadLeft(3, '0') + DropDownList_DUTYMONTH.SelectedValue.Trim().PadLeft(2, '0') + MonthLastDay +
                                              "'  AND A.MZ_AD='" + Session["ADPMZ_EXAD"].ToString() + "' AND A.MZ_UNIT='" + Session["ADPMZ_EXUNIT"].ToString() + "'";
            }
            else if (MonthLastDay == "29")
            {
                strSQL = "SELECT DISTINCT B.MZ_ID,'" + TextBox_DUTYYEAR.Text.Trim().PadLeft(3, '0') + "','" + DropDownList_DUTYMONTH.SelectedValue.Trim().PadLeft(2, '0') + "',";
                for (int i = 1; i <= 29; i++)
                {
                    if (i == 29)
                    {
                        strSQL += "(SELECT TOTAL_HOURS FROM C_DUTYTABLE_PERSONAL WHERE MZ_ID=B.MZ_ID AND  DUTYDATE='"
                                    + TextBox_DUTYYEAR.Text.Trim().PadLeft(3, '0') + DropDownList_DUTYMONTH.SelectedValue.Trim().PadLeft(2, '0') + i.ToString().PadLeft(2, '0') + "' ),NULL,NULL,"
                                    + "(SELECT SUM(TOTAL_HOURS) FROM C_DUTYTABLE_PERSONAL WHERE   ISDIRECTTIME IS NULL AND  MZ_ID=B.MZ_ID AND dbo.SUBSTR(DUTYDATE,1,5)='" + TextBox_DUTYYEAR.Text.Trim().PadLeft(3, '0')
                                    + DropDownList_DUTYMONTH.SelectedValue.Trim().PadLeft(2, '0') + "' GROUP BY MZ_ID) ";
                    }
                    else
                    {
                        strSQL += "(SELECT TOTAL_HOURS FROM C_DUTYTABLE_PERSONAL WHERE MZ_ID=B.MZ_ID AND  DUTYDATE='"
                                   + TextBox_DUTYYEAR.Text.Trim().PadLeft(3, '0') + DropDownList_DUTYMONTH.SelectedValue.Trim().PadLeft(2, '0') + i.ToString().PadLeft(2, '0') + "' ),";
                    }
                }

                strSQL += " FROM C_DUTYTABLE_PERSONAL A,A_DLBASE B WHERE  ISDIRECTTIME IS NULL AND A.MZ_ID=B.MZ_ID AND A.DUTYDATE>='" + TextBox_DUTYYEAR.Text.Trim().PadLeft(3, '0') + DropDownList_DUTYMONTH.SelectedValue.Trim().PadLeft(2, '0') + "01'"
                                          + " AND A.DUTYDATE<='" + TextBox_DUTYYEAR.Text.Trim().PadLeft(3, '0') + DropDownList_DUTYMONTH.SelectedValue.Trim().PadLeft(2, '0') + MonthLastDay +
                                               "'  AND A.MZ_AD='" + Session["ADPMZ_EXAD"].ToString() + "' AND A.MZ_UNIT='" + Session["ADPMZ_EXUNIT"].ToString() + "'";
            }
            else if (MonthLastDay == "30")
            {
                strSQL = "SELECT DISTINCT B.MZ_ID,'" + TextBox_DUTYYEAR.Text.Trim().PadLeft(3, '0') + "','" + DropDownList_DUTYMONTH.SelectedValue.Trim().PadLeft(2, '0') + "',";

                for (int i = 1; i <= 30; i++)
                {
                    if (i == 30)
                    {
                        strSQL += "(SELECT TOTAL_HOURS FROM C_DUTYTABLE_PERSONAL WHERE MZ_ID=B.MZ_ID AND DUTYDATE='"
                                    + TextBox_DUTYYEAR.Text.Trim().PadLeft(3, '0') + DropDownList_DUTYMONTH.SelectedValue.Trim().PadLeft(2, '0') + i.ToString().PadLeft(2, '0') + "' ),NULL,"
                                    + "(SELECT SUM(TOTAL_HOURS) FROM C_DUTYTABLE_PERSONAL WHERE MZ_ID=B.MZ_ID AND dbo.SUBSTR(DUTYDATE,1,5)='" + TextBox_DUTYYEAR.Text.Trim().PadLeft(3, '0')
                                    + DropDownList_DUTYMONTH.SelectedValue.Trim().PadLeft(2, '0') + "' GROUP BY MZ_ID) ";
                    }
                    else
                    {
                        strSQL += "(SELECT TOTAL_HOURS FROM C_DUTYTABLE_PERSONAL WHERE MZ_ID=B.MZ_ID AND DUTYDATE='"
                                   + TextBox_DUTYYEAR.Text.Trim().PadLeft(3, '0') + DropDownList_DUTYMONTH.SelectedValue.Trim().PadLeft(2, '0') + i.ToString().PadLeft(2, '0') + "' ),";
                    }
                }

                strSQL += " FROM C_DUTYTABLE_PERSONAL A,A_DLBASE B WHERE  ISDIRECTTIME IS NULL AND  A.MZ_ID=B.MZ_ID AND A.DUTYDATE>='" + TextBox_DUTYYEAR.Text.Trim().PadLeft(3, '0') + DropDownList_DUTYMONTH.SelectedValue.Trim().PadLeft(2, '0') + "01'"
                                          + " AND A.DUTYDATE<='" + TextBox_DUTYYEAR.Text.Trim().PadLeft(3, '0') + DropDownList_DUTYMONTH.SelectedValue.Trim().PadLeft(2, '0') + MonthLastDay +
                                              "'  AND A.MZ_AD='" + Session["ADPMZ_EXAD"].ToString() + "' AND A.MZ_UNIT='" + Session["ADPMZ_EXUNIT"].ToString() + "'";
            }
            else if (MonthLastDay == "31")
            {
                strSQL = "SELECT DISTINCT B.MZ_ID,'" + TextBox_DUTYYEAR.Text.Trim().PadLeft(3, '0') + "','" + DropDownList_DUTYMONTH.SelectedValue.Trim().PadLeft(2, '0') + "',";
                for (int i = 1; i <= 31; i++)
                {
                    if (i == 31)
                    {
                        strSQL += "(SELECT TOTAL_HOURS FROM C_DUTYTABLE_PERSONAL WHERE MZ_ID=B.MZ_ID AND DUTYDATE='"
                                    + TextBox_DUTYYEAR.Text.Trim().PadLeft(3, '0') + DropDownList_DUTYMONTH.SelectedValue.Trim().PadLeft(2, '0') + i.ToString().PadLeft(2, '0') + "' ),"
                                    + "(SELECT SUM(TOTAL_HOURS) FROM C_DUTYTABLE_PERSONAL WHERE MZ_ID=B.MZ_ID AND dbo.SUBSTR(DUTYDATE,1,5)='" + TextBox_DUTYYEAR.Text.Trim().PadLeft(3, '0')
                                    + DropDownList_DUTYMONTH.SelectedValue.Trim().PadLeft(2, '0') + "' GROUP BY MZ_ID) ";
                    }
                    else
                    {
                        strSQL += "(SELECT TOTAL_HOURS FROM C_DUTYTABLE_PERSONAL WHERE MZ_ID=B.MZ_ID AND DUTYDATE='"
                                   + TextBox_DUTYYEAR.Text.Trim().PadLeft(3, '0') + DropDownList_DUTYMONTH.SelectedValue.Trim().PadLeft(2, '0') + i.ToString().PadLeft(2, '0') + "' ),";
                    }
                }

                strSQL += " FROM C_DUTYTABLE_PERSONAL A,A_DLBASE B WHERE  ISDIRECTTIME IS NULL AND  A.MZ_ID=B.MZ_ID AND A.DUTYDATE>='" + TextBox_DUTYYEAR.Text.Trim().PadLeft(3, '0') + DropDownList_DUTYMONTH.SelectedValue.Trim().PadLeft(2, '0') + "01'"
                                          + " AND A.DUTYDATE<='" + TextBox_DUTYYEAR.Text.Trim().PadLeft(3, '0') + DropDownList_DUTYMONTH.SelectedValue.Trim().PadLeft(2, '0') + MonthLastDay +
                                            "' AND A.MZ_AD='" + Session["ADPMZ_EXAD"].ToString() + "' AND A.MZ_UNIT='" + Session["ADPMZ_EXUNIT"].ToString() + "'";
            }


            strSQL = "INSERT INTO C_DUTYMONTH_HOUR " + strSQL;
            
            string PP = string.Format("SELECT MZ_ID FROM A_DLBASE WHERE MZ_EXAD='{0}' AND MZ_EXUNIT='{1}'",Session["ADPMZ_EXAD"],Session["ADPMZ_EXUNIT"]);
            DataTable temp = new DataTable();
            temp =o_DBFactory.ABC_toTest.Create_Table(PP,"GET");
            foreach(DataRow dr in temp.Rows)
            {
                string delSQL = string.Format("DELETE FROM C_DUTYMONTH_HOUR WHERE MZ_ID='{0}' AND MZ_YEAR={1} AND MZ_MONTH={2}", dr["MZ_ID"].ToString(), TextBox_DUTYYEAR.Text.Trim().PadLeft(3, '0'), DropDownList_DUTYMONTH.SelectedValue.Trim());
                o_DBFactory.ABC_toTest.Edit_Data(delSQL);
            }

            try
            {
                o_DBFactory.ABC_toTest.Edit_Data(strSQL);

                string SelectSQL = "SELECT '' as PNO,(SELECT MZ_NAME FROM A_DLBASE WHERE MZ_ID=C_DUTYMONTH_HOUR.MZ_ID ) MZ_NAME,MZ_ID,";
                for (int j = 1; j <= 31; j++)
                {
                    SelectSQL += "\"" + j.ToString() + "\",";
                }

                SelectSQL += "TOTAL FROM C_DUTYMONTH_HOUR WHERE MZ_ID IN (SELECT MZ_ID FROM A_DLBASE WHERE" +
                                                                    " MZ_EXAD='" + Session["ADPMZ_EXAD"].ToString() +
                                                               "' AND MZ_EXUNIT='" + Session["ADPMZ_EXUNIT"].ToString() + "' AND MZ_YEAR='" + TextBox_DUTYYEAR.Text.PadLeft(3,'0') + "' AND MZ_MONTH='" + DropDownList_DUTYMONTH.SelectedValue.PadLeft(2,'0') + "')";

                DataTable dt = o_DBFactory.ABC_toTest.Create_Table(SelectSQL, "GET");

                if (dt.Rows.Count == 0)
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('無資料!')", true);
                }
                else
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        dr["PNO"] = Service_C_DUTYPEOPLE.get_pno_ym(Session["ADPMZ_EXAD"].ToString(), Session["ADPMZ_EXUNIT"].ToString(), dr["MZ_ID"].ToString(), TextBox_DUTYYEAR.Text.PadLeft(3, '0'), DropDownList_DUTYMONTH.SelectedValue.PadLeft(2, '0'));
                    }

                    dt.Columns.Remove("MZ_ID");
                    dt = dt.AsEnumerable().OrderBy(dr => dr["PNO"]).CopyToDataTable();
                    ViewState["rpt_dt"] = dt;
                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                }
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('產生失敗!')", true);
            }
        }

        int TPM_FION=0;

        protected void btPrint_Click(object sender, EventArgs e)
        {
            //Session["C_MONTHHOUR_EXUNIT"] = Session["ADPMZ_EXUNIT"];
            //Session["C_MONTHHOUR_EXAD"] = Session["ADPMZ_EXAD"];
            //Session["C_MONTHHOUR_YEAR"] = TextBox_DUTYYEAR.Text.Trim().PadLeft(3,'0');
            //Session["C_MONTHHOUR_MONTH"] = DropDownList_DUTYMONTH.SelectedValue.PadLeft(2,'0');
            Session["rpt_dt"] = ViewState["rpt_dt"] as DataTable;
            Session["TITLE"] = string.Format("{0}{1}{2}年{3}月個人勤務時數統計表", o_A_KTYPE.RAD(Session["ADPMZ_EXAD"].ToString()), o_A_KTYPE.RUNIT(Session["ADPMZ_EXUNIT"].ToString()), TextBox_DUTYYEAR.Text.Trim(), DropDownList_DUTYMONTH.SelectedValue);

            string tmp_url = "C_rpt.aspx?fn=dutymonthhour&TPM_FION=" + TPM_FION;

            ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
        }
    }
}
