using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace TPPDDB._3_forleave
{
    public partial class C_ForLeaveOvertime_OVERTIME_VERIFY : System.Web.UI.Page
    {
        //時間排在前面的版本
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                C.check_power();
            

            //MQ-----------------------20100331
            C.set_Panel_EnterToTAB(ref this.Panel1);
            
                Label1.Text = "超勤加班費審核";
                TextBox_MZ_YEAR.Text = (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0');
            }
        }

        protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridView gv = (GridView)sender;
                e.Row.Cells.Clear();
                GridViewRow gvRow = new

                GridViewRow(0, 0, DataControlRowType.Header,
                            DataControlRowState.Insert);

                TableCell t1 = new TableCell();
                t1.Text = "1";
                gvRow.Cells.Add(t1);

                TableCell t2 = new TableCell();
                t2.Text = "2";
                gvRow.Cells.Add(t2);

                TableCell t3 = new TableCell();
                t3.Text = "3";
                gvRow.Cells.Add(t3);

                TableCell t4 = new TableCell();
                t4.Text = "4";
                gvRow.Cells.Add(t4);

                TableCell t5 = new TableCell();
                t5.Text = "5";
                gvRow.Cells.Add(t5);

                TableCell t6 = new TableCell();
                t6.Text = "6";
                gvRow.Cells.Add(t6);

                TableCell t7 = new TableCell();
                t7.Text = "7";
                gvRow.Cells.Add(t7);

                TableCell t8 = new TableCell();
                t8.Text = "8";
                gvRow.Cells.Add(t8);

                TableCell t9 = new TableCell();
                t9.Text = "9";
                gvRow.Cells.Add(t9);

                TableCell t10 = new TableCell();
                t10.Text = "10";
                gvRow.Cells.Add(t10);

                TableCell t11 = new TableCell();
                t11.Text = "11";
                gvRow.Cells.Add(t11);

                TableCell t12 = new TableCell();
                t12.Text = "12";
                gvRow.Cells.Add(t12);

                TableCell t13 = new TableCell();
                t13.Text = "13";
                gvRow.Cells.Add(t13);

                TableCell t14 = new TableCell();
                t14.Text = "14";
                gvRow.Cells.Add(t14);

                TableCell t15 = new TableCell();
                t15.Text = "15";
                gvRow.Cells.Add(t15);

                TableCell t16 = new TableCell();
                t16.Text = "16";
                gvRow.Cells.Add(t16);

                TableCell t17 = new TableCell();
                t17.Text = "17";
                gvRow.Cells.Add(t17);

                TableCell t18 = new TableCell();
                t18.Text = "18";
                gvRow.Cells.Add(t18);

                TableCell t19 = new TableCell();
                t19.Text = "19";
                gvRow.Cells.Add(t19);

                TableCell t20 = new TableCell();
                t20.Text = "20";
                gvRow.Cells.Add(t20);

                TableCell t21 = new TableCell();
                t21.Text = "21";
                gvRow.Cells.Add(t21);

                TableCell t22 = new TableCell();
                t22.Text = "22";
                gvRow.Cells.Add(t22);

                TableCell t23 = new TableCell();
                t23.Text = "23";
                gvRow.Cells.Add(t23);

                TableCell t24 = new TableCell();
                t24.Text = "24";
                gvRow.Cells.Add(t24);

                TableCell t25 = new TableCell();
                t25.Text = "25";
                gvRow.Cells.Add(t25);

                TableCell t26 = new TableCell();
                t26.Text = "26";
                gvRow.Cells.Add(t26);

                TableCell t27 = new TableCell();
                t27.Text = "27";
                gvRow.Cells.Add(t27);

                TableCell t28 = new TableCell();
                t28.Text = "28";
                gvRow.Cells.Add(t28);

                TableCell t29 = new TableCell();
                t29.Text = "29";
                gvRow.Cells.Add(t29);

                TableCell t30 = new TableCell();
                t30.Text = "30";
                gvRow.Cells.Add(t30);

                TableCell t31 = new TableCell();
                t31.Text = "31";
                gvRow.Cells.Add(t31);

                gv.Controls[0].Controls.AddAt(0, gvRow);

                gvRow = new
                 GridViewRow(0, 0, DataControlRowType.Header,
                 DataControlRowState.Insert);

                TableCell tc13 = new TableCell();
                CheckBox cb = new CheckBox();
                cb.TextAlign = TextAlign.Left;
                cb.AutoPostBack = true;
                cb.CheckedChanged += cbadd;
                tc13.Controls.Add(cb);
                tc13.RowSpan = 2;
                gvRow.Cells.Add(tc13);

                TableCell tc1 = new TableCell();
                tc1.Text = "編號";
                tc1.RowSpan = 2;
                gvRow.Cells.Add(tc1);

                TableCell tc2 = new TableCell();
                tc2.Text = "職稱";
                tc2.RowSpan = 2;
                gvRow.Cells.Add(tc2);

                TableCell tc3 = new TableCell();
                tc3.Text = "姓名";
                tc3.RowSpan = 2;
                gvRow.Cells.Add(tc3);

                TableCell tc4 = new TableCell();
                tc4.Text = "超勤日期及超勤時數";
                tc4.ColumnSpan = 31;
                gvRow.Cells.Add(tc4);

                TableCell tc5 = new TableCell();
                tc5.Text = "超勤時數";
                tc5.RowSpan = 2;
                gvRow.Cells.Add(tc5);

                TableCell tc6 = new TableCell();
                tc6.Text = "實際支領時數";
                tc6.RowSpan = 2;
                gvRow.Cells.Add(tc6);

                TableCell tc7 = new TableCell();
                tc7.Text = "結餘時數";
                tc7.RowSpan = 2;
                gvRow.Cells.Add(tc7);

                TableCell tc8 = new TableCell();
                tc8.Text = "俸給月支數額";
                tc8.RowSpan = 2;

                gvRow.Cells.Add(tc8);

                TableCell tc9 = new TableCell();
                tc9.Text = "專業加給";
                tc9.RowSpan = 2;
                gvRow.Cells.Add(tc9);

                TableCell tc10 = new TableCell();
                tc10.Text = "主管加給";
                tc10.RowSpan = 2;
                gvRow.Cells.Add(tc10);

                TableCell tc11 = new TableCell();
                tc11.Text = "每小時支領數";
                tc11.RowSpan = 2;
                gvRow.Cells.Add(tc11);

                TableCell tc12 = new TableCell();
                tc12.Text = "超勤金額";
                tc12.RowSpan = 2;
                gvRow.Cells.Add(tc12);

                gv.Controls[0].Controls.AddAt(0, gvRow);

                gvRow = new
                GridViewRow(0, 0, DataControlRowType.Header,
                DataControlRowState.Insert);

                TableCell tcc2 = new TableCell();

                Button bt = new Button();

                bt.Text = "審核";
                bt.OnClientClick = "return confirm('確定審核？');";
                bt.Click += btClick;
                tcc2.Controls.Add(bt);
                gvRow.Cells.Add(tcc2);


                TableCell tcc1 = new TableCell();

                tcc1.Text = DropDownList_EXAD.SelectedItem.Text + DropDownList_EXUNIT.SelectedItem.Text + o_str.tosql(ViewState["year"].ToString().Trim()) + "年" + o_str.tosql(ViewState["month"].ToString().Trim()) + "月超勤加班費審核";
                tcc1.HorizontalAlign = HorizontalAlign.Center;
                tcc1.Font.Size = 16;
                tcc1.ColumnSpan = 42;
                gvRow.Cells.Add(tcc1);

                gv.Controls[0].Controls.AddAt(0, gvRow);
            }
        }

        DataTable temp = new DataTable();

        protected void btClick(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {

                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();
                temp = Session["OVERTIME_VERIFY_TEMP"] as DataTable;
                try
                {
                    int j = 0;
                    for (int i = 0; i < GridView1.Rows.Count; i++)
                    {
                        if ((GridView1.Rows[i].Cells[0].Controls[1] as CheckBox).Checked)
                        {
                            j++;
                            string command1 = "UPDATE C_DUTYMONTHOVERTIME_HOUR SET MZ_REAL_HOUR='" + temp.Rows[i]["MZ_REAL_HOUR"].ToString() +
                                                                                "',MZ_VERIFY='Y',MZ_BALANCE_HOUR='" + temp.Rows[i]["MZ_BALANCE_HOUR"].ToString() +
                                                                                "',MZ_OVERTIME_PAY='" + temp.Rows[i]["MZ_OVERTIME_PAY"].ToString() +
                                                                                "',MZ_VERIFY_MAN='" + Session["ADPMZ_ID"].ToString() +
                                                                                "',MZ_VERIFY_DATE='" + o_str.tosql(TextBox_MZ_YEAR.Text.Trim()).PadLeft(3, '0') + o_str.tosql(DropDownList_MZ_MONTH.SelectedValue) + DateTime.Now.Day.ToString("D2") +
                                               "' WHERE MZ_ID='" + temp.Rows[i]["MZ_ID"].ToString() +
                                                 "' AND MZ_YEAR='" + o_str.tosql(TextBox_MZ_YEAR.Text.Trim().PadLeft(3, '0')) +
                                                 "' AND MZ_MONTH='" + o_str.tosql(DropDownList_MZ_MONTH.SelectedValue) + "'";
                            SqlCommand cmd1 = new SqlCommand(command1, conn);
                            cmd1.CommandType = CommandType.Text;
                            cmd1.Transaction = transaction;

                            cmd1.ExecuteNonQuery();
                        }
                        else if ((GridView1.Rows[i].Cells[0].Controls[1] as CheckBox).Checked == false)
                        {
                            j++;
                            string command1 = "UPDATE C_DUTYMONTHOVERTIME_HOUR SET MZ_REAL_HOUR='" + temp.Rows[i]["MZ_REAL_HOUR"].ToString() +
                                                                                "',MZ_VERIFY='N',MZ_BALANCE_HOUR='" + temp.Rows[i]["MZ_BALANCE_HOUR"].ToString() +
                                                                                "',MZ_OVERTIME_PAY='" + temp.Rows[i]["MZ_OVERTIME_PAY"].ToString() +
                                //"',MZ_VERIFY_MAN='" + Session["ADPMZ_ID"].ToString() +
                                //"',MZ_VERIFY_DATE='" + o_str.tosql(TextBox_MZ_YEAR.Text.Trim()) + o_str.tosql(TextBox_MZ_MONTH.Text.Trim()) + DateTime.Now.Day.ToString() +
                                               "' WHERE MZ_ID='" + temp.Rows[i]["MZ_ID"].ToString() +
                                                 "' AND MZ_YEAR='" + o_str.tosql(TextBox_MZ_YEAR.Text.Trim().PadLeft(3, '0')) +
                                                 "' AND MZ_MONTH='" + o_str.tosql(DropDownList_MZ_MONTH.SelectedValue) + "'";
                            SqlCommand cmd1 = new SqlCommand(command1, conn);
                            cmd1.CommandType = CommandType.Text;
                            cmd1.Transaction = transaction;

                            cmd1.ExecuteNonQuery();
                        }
                    }
                    if (j == 0)
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('請先選取資料')", true);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('審核成功')", true);
                        transaction.Commit();
                        btOK_Click(sender, e);
                    }
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
                finally
                {
                    conn.Close();

                    //XX2013/06/18 
                    conn.Dispose();
                }
            }
        }

        protected void cbadd(object sender, EventArgs e)
        {
            if (((sender) as CheckBox).Checked)
            {
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    (GridView1.Rows[i].Cells[0].Controls[1] as CheckBox).Checked = true;
                }
            }
            else
            {
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    (GridView1.Rows[i].Cells[0].Controls[1] as CheckBox).Checked = false;
                }
            }
        }

        protected void btOK_Click(object sender, EventArgs e)
        {
            string strSQL = "SELECT MZ_ID,MZ_HOUR_LIMIT,MZ_MONEY_LIMIT,(SELECT MZ_POLNO FROM A_DLBASE WHERE MZ_ID=C_DUTYMONTHOVERTIME_HOUR.MZ_ID) AS MZ_POLNO," +
                                  " (SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='26' AND MZ_KCODE=(SELECT MZ_OCCC FROM A_DLBASE WHERE MZ_ID=C_DUTYMONTHOVERTIME_HOUR.MZ_ID)) AS MZ_OCCC," +
                                  " (SELECT MZ_NAME FROM A_DLBASE WHERE MZ_ID=C_DUTYMONTHOVERTIME_HOUR.MZ_ID) AS MZ_NAME," +
                                  " \"1\",\"2\",\"3\",\"4\",\"5\",\"6\",\"7\",\"8\",\"9\",\"10\",\"11\",\"12\",\"13\",\"14\"," +
                                  " \"15\",\"16\",\"17\",\"18\",\"19\",\"20\",\"21\",\"22\",\"23\",\"24\",\"25\",\"26\",\"27\"," +
                                  " \"28\",\"29\",\"30\",\"31\",MZ_BUDGET_HOUR,CASE WHEN MZ_REAL_HOUR>=MZ_HOUR_LIMIT THEN MZ_HOUR_LIMIT ELSE MZ_REAL_HOUR END MZ_REAL_HOUR,MZ_REAL_HOUR-CASE WHEN MZ_REAL_HOUR>=MZ_HOUR_LIMIT THEN MZ_HOUR_LIMIT ELSE MZ_REAL_HOUR END AS MZ_BALANCE_HOUR,PAY1,PROFESS,BOSS," +
                                  " MZ_HOUR_PAY,CASE WHEN MZ_REAL_HOUR>=MZ_HOUR_LIMIT THEN CASE  WHEN MZ_HOUR_PAY*MZ_HOUR_LIMIT > MZ_MONEY_LIMIT THEN MZ_MONEY_LIMIT ELSE MZ_HOUR_PAY*MZ_HOUR_LIMIT END ELSE CASE WHEN MZ_OVERTIME_PAY > MZ_MONEY_LIMIT THEN MZ_MONEY_LIMIT ELSE MZ_OVERTIME_PAY END  END MZ_OVERTIME_PAY,MZ_ID,MZ_VERIFY  " +
                            " FROM C_DUTYMONTHOVERTIME_HOUR " +
                            " WHERE " +
                                  "      MZ_YEAR='" + o_str.tosql(TextBox_MZ_YEAR.Text.Trim().PadLeft(3, '0')) +
                                  "' AND MZ_MONTH='" + o_str.tosql(DropDownList_MZ_MONTH.SelectedValue) +
                //"' AND (MZ_VERIFY <>'Y' OR MZ_VERIFY IS NULL) "+
                                  "'  AND  MZ_AD='" + DropDownList_EXAD.SelectedValue +
                                                                             "'  AND MZ_UNIT='" + DropDownList_EXUNIT.SelectedValue + "'";

            strSQL += " ORDER BY (SELECT REPLACE(MZ_TBDV,'Z99','999') FROM A_DLBASE WHERE MZ_ID=C_DUTYMONTHOVERTIME_HOUR.MZ_ID),(SELECT MZ_OCCC FROM A_DLBASE WHERE MZ_ID=C_DUTYMONTHOVERTIME_HOUR.MZ_ID)";

            temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");

            if (temp.Rows.Count > 0)
            {
                Session["OVERTIME_VERIFY_TEMP"] = temp;
                ViewState["unit"] = DropDownList_EXUNIT.SelectedItem.Text;
                ViewState["year"] = TextBox_MZ_YEAR.Text.Trim().PadLeft(3, '0');
                ViewState["month"] = DropDownList_MZ_MONTH.SelectedValue;
                GridView1.DataSource = temp;
                GridView1.DataBind();
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('無搜尋條件下之資料，請檢查')", true);
            }
        }

        protected void TextBox_MZ_REAL_HOUR_TextChanged(object sender, EventArgs e)
        {
            temp = Session["OVERTIME_VERIFY_TEMP"] as DataTable;

            GridViewRow gvr = (GridViewRow)((TextBox)sender).NamingContainer;
            GridView gv = (GridView)gvr.NamingContainer;

            int rowCount = gv.Rows.Count;
            int index = gvr.RowIndex;

            TextBox tb = (TextBox)gv.Rows[index].Cells[36].Controls[1] as TextBox;

            string MZ_BUDGET_HOUR = temp.Rows[index]["MZ_BUDGET_HOUR"].ToString();
            string MZ_REAL_HOUR = temp.Rows[index]["MZ_REAL_HOUR"].ToString();
            string MZ_HOUR_PAY = temp.Rows[index]["MZ_HOUR_PAY"].ToString();
            string MHL = temp.Rows[index]["MZ_HOUR_LIMIT"].ToString();
            string MML = temp.Rows[index]["MZ_MONEY_LIMIT"].ToString();
            if (!string.IsNullOrEmpty(MHL))
            {
                if (int.Parse(tb.Text) > int.Parse(MHL))
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('已超過超勤時數上限，請檢查')", true);
                }
                else
                {
                    if (int.Parse(tb.Text) > int.Parse(MZ_BUDGET_HOUR))
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('實際支付時數,不得大於超勤時數')", true);
                    }
                    else
                    {

                        temp.Rows[index]["MZ_BALANCE_HOUR"] = (int.Parse(MZ_BUDGET_HOUR) - int.Parse(tb.Text)).ToString();
                        temp.Rows[index]["MZ_OVERTIME_PAY"] = string.IsNullOrEmpty(MML) ? MathHelper.Round(float.Parse(MZ_HOUR_PAY) * int.Parse(tb.Text)) : (MathHelper.Round(float.Parse(MZ_HOUR_PAY) * int.Parse(tb.Text)) > double.Parse(MML) ? int.Parse(MML) : MathHelper.Round(float.Parse(MZ_HOUR_PAY) * int.Parse(tb.Text)));
                        temp.Rows[index]["MZ_REAL_HOUR"] = tb.Text;
                    }
                }
            }
            else
            {
                if (int.Parse(tb.Text) > int.Parse(MZ_BUDGET_HOUR))
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('實際支付時數,不得大於超勤時數')", true);
                }
                else
                {
                    temp.Rows[index]["MZ_BALANCE_HOUR"] = (int.Parse(MZ_BUDGET_HOUR) - int.Parse(tb.Text)).ToString();
                    temp.Rows[index]["MZ_OVERTIME_PAY"] = string.IsNullOrEmpty(MML) ? MathHelper.Round(float.Parse(MZ_HOUR_PAY) * int.Parse(tb.Text)) : (MathHelper.Round(float.Parse(MZ_HOUR_PAY) * int.Parse(tb.Text)) > double.Parse(MML) ? int.Parse(MML) : MathHelper.Round(float.Parse(MZ_HOUR_PAY) * int.Parse(tb.Text)));
                    temp.Rows[index]["MZ_REAL_HOUR"] = tb.Text;
                }
            }

            GridView1.DataSource = temp;
            GridView1.DataBind();
            Session["OVERTIME_VERIFY_TEMP"] = temp;
        }

        protected void DropDownList_EXUNIT_DataBound(object sender, EventArgs e)
        {
            ListItem li = new ListItem(" ", "");
            DropDownList_EXUNIT.Items.Insert(0, li);
        }
    }
}
