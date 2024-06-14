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
using TPPDDB.Service;

namespace TPPDDB._3_forleave
{
    public partial class C_ForLeaveOvertime_Otheradd : System.Web.UI.Page
    {
        //時間排在後面的版本
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                C.check_power();


                //MQ-----------------------20100331
                C.set_Panel_EnterToTAB(ref this.Panel1);

                Label1.Text = "加班費補發清冊";
                TextBox_MZ_YEAR.Text = (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0');


                DropDownList_EXAD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                // C、D權限要鎖定發薪機關
                switch (o_a_Function.strGID(Session["ADPMZ_ID"].ToString()))
                {
                    case "C":
                        DropDownList_EXAD.Enabled = false;
                        break;
                    case "D":
                        DropDownList_EXAD.Enabled = false;
                        break;
                }
            }
        }

        /// <summary>
        /// 匯出
        /// </summary>
        DataTable _DT
        {
            set { ViewState["_DT"] = value; }
            get
            {
                if (ViewState["_DT"] == null)
                {
                    DataTable DT = new DataTable();
                    ViewState["_DT"] = DT;
                }
                return (DataTable)ViewState["_DT"];
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

                //TableCell t1 = new TableCell();
                //t1.Text = "1";
                //gvRow.Cells.Add(t1);

                //TableCell t2 = new TableCell();
                //t2.Text = "2";
                //gvRow.Cells.Add(t2);

                //TableCell t3 = new TableCell();
                //t3.Text = "3";
                //gvRow.Cells.Add(t3);

                //TableCell t4 = new TableCell();
                //t4.Text = "4";
                //gvRow.Cells.Add(t4);

                //TableCell t5 = new TableCell();
                //t5.Text = "5";
                //gvRow.Cells.Add(t5);

                //TableCell t6 = new TableCell();
                //t6.Text = "6";
                //gvRow.Cells.Add(t6);

                //TableCell t7 = new TableCell();
                //t7.Text = "7";
                //gvRow.Cells.Add(t7);

                //TableCell t8 = new TableCell();
                //t8.Text = "8";
                //gvRow.Cells.Add(t8);

                //TableCell t9 = new TableCell();
                //t9.Text = "9";
                //gvRow.Cells.Add(t9);

                //TableCell t10 = new TableCell();
                //t10.Text = "10";
                //gvRow.Cells.Add(t10);

                //TableCell t11 = new TableCell();
                //t11.Text = "11";
                //gvRow.Cells.Add(t11);

                //TableCell t12 = new TableCell();
                //t12.Text = "12";
                //gvRow.Cells.Add(t12);

                //TableCell t13 = new TableCell();
                //t13.Text = "13";
                //gvRow.Cells.Add(t13);

                //TableCell t14 = new TableCell();
                //t14.Text = "14";
                //gvRow.Cells.Add(t14);

                //TableCell t15 = new TableCell();
                //t15.Text = "15";
                //gvRow.Cells.Add(t15);

                //TableCell t16 = new TableCell();
                //t16.Text = "16";
                //gvRow.Cells.Add(t16);

                //TableCell t17 = new TableCell();
                //t17.Text = "17";
                //gvRow.Cells.Add(t17);

                //TableCell t18 = new TableCell();
                //t18.Text = "18";
                //gvRow.Cells.Add(t18);

                //TableCell t19 = new TableCell();
                //t19.Text = "19";
                //gvRow.Cells.Add(t19);

                //TableCell t20 = new TableCell();
                //t20.Text = "20";
                //gvRow.Cells.Add(t20);

                //TableCell t21 = new TableCell();
                //t21.Text = "21";
                //gvRow.Cells.Add(t21);

                //TableCell t22 = new TableCell();
                //t22.Text = "22";
                //gvRow.Cells.Add(t22);

                //TableCell t23 = new TableCell();
                //t23.Text = "23";
                //gvRow.Cells.Add(t23);

                //TableCell t24 = new TableCell();
                //t24.Text = "24";
                //gvRow.Cells.Add(t24);

                //TableCell t25 = new TableCell();
                //t25.Text = "25";
                //gvRow.Cells.Add(t25);

                //TableCell t26 = new TableCell();
                //t26.Text = "26";
                //gvRow.Cells.Add(t26);

                //TableCell t27 = new TableCell();
                //t27.Text = "27";
                //gvRow.Cells.Add(t27);

                //TableCell t28 = new TableCell();
                //t28.Text = "28";
                //gvRow.Cells.Add(t28);

                //TableCell t29 = new TableCell();
                //t29.Text = "29";
                //gvRow.Cells.Add(t29);

                //TableCell t30 = new TableCell();
                //t30.Text = "30";
                //gvRow.Cells.Add(t30);

                //TableCell t31 = new TableCell();
                //t31.Text = "31";
                //gvRow.Cells.Add(t31);

                gv.Controls[0].Controls.AddAt(0, gvRow);

                gvRow = new
                 GridViewRow(0, 0, DataControlRowType.Header,
                 DataControlRowState.Insert);

                //TableCell tc14 = new TableCell();
                //CheckBox cb = new CheckBox();
                //cb.TextAlign = TextAlign.Left;
                //cb.AutoPostBack = true;
                //cb.CheckedChanged += cbadd;
                //tc14.Controls.Add(cb);
                //tc14.RowSpan = 2;
                //gvRow.Cells.Add(tc14);

                TableCell tc1 = new TableCell();
                tc1.Text = "身份證號";
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


                TableCell tc5 = new TableCell();
                tc5.Text = "加班<br/>分鐘數";
                tc5.RowSpan = 2;
                tc5.Width = 30;
                gvRow.Cells.Add(tc5);

                TableCell tc6 = new TableCell();
                tc6.Text = "實際支<br/>領時數";
                tc6.RowSpan = 2;
                tc6.Width = 40;
                gvRow.Cells.Add(tc6);

                /*TableCell tc7 = new TableCell();
                tc7.Text = "結餘<br/>時數";
                tc7.Width = 30;
                tc7.RowSpan = 2;
                gvRow.Cells.Add(tc7);*/

                TableCell tc8 = new TableCell();
                tc8.Text = "備註說明";
                tc8.Width = 100;
                tc8.RowSpan = 2;
                gvRow.Cells.Add(tc8);

                TableCell tc9 = new TableCell();
                tc9.Text = "俸給月<br/>支數額";
                tc9.RowSpan = 2;
                gvRow.Cells.Add(tc9);

                TableCell tc10 = new TableCell();
                tc10.Text = "專業<br/>加給";
                tc10.Width = 30;
                tc10.RowSpan = 2;
                gvRow.Cells.Add(tc10);

                TableCell tc11 = new TableCell();
                tc11.Text = "主管<br/>加給";
                tc11.RowSpan = 2;
                tc11.Width = 30;
                gvRow.Cells.Add(tc11);

                TableCell tc12 = new TableCell();
                tc12.Text = "每小時<br/>支領數";
                tc12.RowSpan = 2;
                gvRow.Cells.Add(tc12);

                TableCell tc13 = new TableCell();
                tc13.Text = "加班<br/>總金額";
                tc13.RowSpan = 2;
                tc13.Width = 30;
                gvRow.Cells.Add(tc13);

                TableCell tc15 = new TableCell();
                tc15.Text = "俸給月<br/>支數額(晉級)";
                tc15.RowSpan = 2;
                gvRow.Cells.Add(tc15);

                TableCell tc16 = new TableCell();
                tc16.Text = "專業<br/>加給(晉級)";
                tc16.Width = 30;
                tc16.RowSpan = 2;
                gvRow.Cells.Add(tc16);

                TableCell tc17 = new TableCell();
                tc17.Text = "主管<br/>加給(晉級)";
                tc17.RowSpan = 2;
                tc17.Width = 30;
                gvRow.Cells.Add(tc17);

                TableCell tc18 = new TableCell();
                tc18.Text = "每小時<br/>支領數(晉級)";
                tc18.RowSpan = 2;
                tc18.Width = 30;
                gvRow.Cells.Add(tc18);

                // TableCell tc4 = new TableCell();
                // tc4.Text = "加班日期及加班時數";
                // tc4.ColumnSpan = 31;
                // gvRow.Cells.Add(tc4);
                TableCell tc19 = new TableCell();
                tc19.Text = "加班總<br>金額(晉級)";
                tc19.RowSpan = 2;
                tc19.Width = 30;
                gvRow.Cells.Add(tc19);

                TableCell tc21 = new TableCell();
                tc21.Text = "時數餘數";
                tc21.RowSpan = 2;
                tc21.Width = 30;
                gvRow.Cells.Add(tc21);

                TableCell tc20 = new TableCell();
                tc20.Text = "補發金額";
                tc20.RowSpan = 2;
                tc20.Width = 30;
                gvRow.Cells.Add(tc20);

                gv.Controls[0].Controls.AddAt(0, gvRow);

                gvRow = new
                GridViewRow(0, 0, DataControlRowType.Header,
                DataControlRowState.Insert);

                //TableCell tcc2 = new TableCell();

                //Button bt = new Button();

                ////bt.Text = "審核";
                ////bt.OnClientClick = "return confirm('確定審核？');";
                ////bt.Click += btClick;
                ////tcc2.Controls.Add(bt);
                //gvRow.Cells.Add(tcc2);


                TableCell tcc1 = new TableCell();
                string sumText = "加班補發總金額：" + _DT.Rows[0]["SUM"].ToString();

                tcc1.Text = sumText + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; " + DropDownList_EXAD.SelectedItem.Text + DropDownList_EXUNIT.SelectedItem.Text + o_str.tosql(ViewState["year"].ToString().Trim()) + "年" + o_str.tosql(ViewState["month"].ToString().Trim()) + "月業務加班費補發清冊";
                tcc1.HorizontalAlign = HorizontalAlign.Left;
                tcc1.Font.Size = 15;
                tcc1.ColumnSpan = 20;
                tcc1.ID = "TableTitle";
                gvRow.Cells.Add(tcc1);

                gv.Controls[0].Controls.AddAt(0, gvRow);

            }
        }

        DataTable temp = new DataTable();

        /// <summary>
        /// 按鈕 : 審核
        /// </summary>
        protected void btClick(object sender, EventArgs e)
        {
            ///TODO : 這邊使用 Transaction ?? WHY
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                //查詢結果的TABLE直接使用
                temp = Session["OVERTIME_VERIFY_TEMP"] as DataTable;
                try
                {
                    //逐條處理
                    for (int i = 0; i < GridView1.Rows.Count; i++)
                    {
                        String command1 = "";
                        DataTable dt = new DataTable();

                        //基礎欄位
                        String MZ_ID = temp.Rows[i]["MZ_ID"].ToString();
                        String Year = o_str.tosql(TextBox_MZ_YEAR.Text.Trim().PadLeft(3, '0'));
                        String Month = o_str.tosql(DropDownList_MZ_MONTH.SelectedValue);
                        String LoginUserID = Session["ADPMZ_ID"].ToString();
                        String Today = o_str.tosql(TextBox_MZ_YEAR.Text.Trim()).PadLeft(3, '0') + o_str.tosql(DropDownList_MZ_MONTH.SelectedValue) + DateTime.Now.Day.ToString();

                        //時數欄位
                        String REAL_HOUR = temp.Rows[i]["MZ_REAL_HOUR"].ToString();
                        String BALANCE_HOUR = temp.Rows[i]["MZ_BALANCE_HOUR"].ToString();
                        String OVERTIME_PAY = temp.Rows[i]["MZ_OVERTIME_PAY"].ToString();

                        //    CheckBox ckb = GridView1.Rows[i].Cells[0].Controls[1] as CheckBox ;

                        String PAYAD = temp.Rows[i]["MZ_AD"].ToString();
                        String MZ_EXUNIT = temp.Rows[i]["MZ_EXUNIT"].ToString();

                        #region 1.針對加班費清冊表格(C_OVERTIMEMONTH_HOUR) 進行更新
                        ////如果有打勾才進行審核
                        //if (ckb.Checked)
                        //{
                        //    command1 = "UPDATE C_OVERTIMEMONTH_HOUR SET MZ_REAL_HOUR='" + REAL_HOUR +
                        //                                                        "',MZ_VERIFY='Y'"  +
                        //                                                        ",MZ_OVERTIME_PAY='" + OVERTIME_PAY +
                        //                                                        "',MZ_VERIFY_MAN='" + LoginUserID +
                        //                                                        "',MZ_VERIFY_DATE='" + Today +
                        //                       "' WHERE MZ_ID='"    + MZ_ID +
                        //                       "' AND MZ_AD='"      + PAYAD +
                        //                       "' AND MZ_EXUNIT='"  + MZ_EXUNIT +
                        //                       "' AND MZ_YEAR='"    + Year +
                        //                       "' AND MZ_MONTH='"   + Month + "'";
                        //}
                        ////若沒有打勾則進行復原動作
                        //else 
                        //{
                        command1 = "UPDATE C_OVERTIMEMONTH_HOUR SET MZ_REAL_HOUR='" + REAL_HOUR +
                                                                            "',MZ_VERIFY='N'" +
                                                                            ",MZ_OVERTIME_PAY='" + OVERTIME_PAY +
                                           "' WHERE MZ_ID='" + MZ_ID +
                                           "' AND MZ_AD='" + PAYAD +
                                           "' AND MZ_EXUNIT='" + MZ_EXUNIT +
                                           "' AND MZ_YEAR='" + Year +
                                           "' AND MZ_MONTH='" + Month + "'";
                        //}

                        //執行SQL
                        SqlCommand cmd1 = new SqlCommand(command1, conn);
                        cmd1.CommandType = CommandType.Text;
                        cmd1.Transaction = transaction;
                        cmd1.ExecuteNonQuery();
                        #endregion 

                        //#region 2.針對超勤輸入進行鎖定動作
                        ////修改查詢條件 MZ_UNIT 為 MZ_EXUNIT 20190110 by sky
                        //String UpdatePersonalTable = String.Format(@"UPDATE C_dutytable_personal SET MZ_VERIFY = '{0}'
                        //                                             WHERE MZ_ID = '{1}' AND MZ_AD = '{2}' AND MZ_EXUNIT = '{3}' AND DUTYDATE like '{4}%'",
                        //                                           (ckb.Checked) ? "Y" : "N",
                        //                                           MZ_ID,
                        //                                           PAYAD,
                        //                                           MZ_EXUNIT,
                        //                                           Year + Month);
                        ////執行SQL
                        //SqlCommand cmd2 = new SqlCommand(UpdatePersonalTable, conn);
                        //cmd2.CommandType = CommandType.Text;
                        //cmd2.Transaction = transaction;
                        //cmd2.ExecuteNonQuery();
                        //#endregion 

                    }

                    //確認全部完成後，進行 Commit
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('審核成功')", true);
                    transaction.Commit();
                    btOK_Click(sender, e);
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }

        /// <summary>
        /// 按鈕 : 刪除
        /// </summary>
        protected void btDeleteClick(object sender, EventArgs e)
        {
            ///TODO : 這邊使用 Transaction ?? WHY
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                //查詢結果的TABLE直接使用
                temp = Session["OVERTIME_VERIFY_TEMP"] as DataTable;
                try
                {
                    //逐條處理
                    for (int i = 0; i < GridView1.Rows.Count; i++)
                    {
                        String command1 = "";
                        DataTable dt = new DataTable();

                        //基礎欄位
                        String MZ_ID = temp.Rows[i]["MZ_ID"].ToString();
                        String Year = o_str.tosql(TextBox_MZ_YEAR.Text.Trim().PadLeft(3, '0'));
                        String Month = o_str.tosql(DropDownList_MZ_MONTH.SelectedValue);
                        String LoginUserID = Session["ADPMZ_ID"].ToString();
                        String Today = o_str.tosql(TextBox_MZ_YEAR.Text.Trim()).PadLeft(3, '0') + o_str.tosql(DropDownList_MZ_MONTH.SelectedValue) + DateTime.Now.Day.ToString();

                        //時數欄位
                        String REAL_HOUR = temp.Rows[i]["MZ_REAL_HOUR"].ToString();
                        String BALANCE_HOUR = temp.Rows[i]["MZ_BALANCE_HOUR"].ToString();
                        String OVERTIME_PAY = temp.Rows[i]["MZ_OVERTIME_PAY"].ToString();

                        CheckBox ckb = GridView1.Rows[i].Cells[0].Controls[1] as CheckBox;

                        String PAYAD = temp.Rows[i]["MZ_AD"].ToString();
                        String MZ_EXUNIT = temp.Rows[i]["MZ_EXUNIT"].ToString();

                        #region 1.針對超勤清冊表格(C_OVERTIMEMONTH_HOUR) 進行刪除
                        //如果有打勾才進行刪除
                        if (ckb.Checked)
                        {
                            command1 = "Delete C_OVERTIMEMONTH_HOUR where MZ_ID='" + MZ_ID +
                                                       "' AND MZ_AD='" + PAYAD +
                                                       "' AND MZ_EXUNIT='" + MZ_EXUNIT +
                                                       "' AND MZ_YEAR='" + Year +
                                                       "' AND MZ_MONTH='" + Month + "'";


                            //執行SQL
                            SqlCommand cmd1 = new SqlCommand(command1, conn);
                            cmd1.CommandType = CommandType.Text;
                            cmd1.Transaction = transaction;
                            cmd1.ExecuteNonQuery();
                        }

                        #endregion

                    }

                    //確認全部完成後，進行 Commit
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除成功')", true);
                    transaction.Commit();
                    btOK_Click(sender, e);
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
                finally
                {
                    conn.Close();
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
            ///TODO Neil 筆記 : 這邊表單命名雖為 EXAD，但實際上是抓發薪機關跟編制單位
            string strSQL = "SELECT MZ_AD, MZ_UNIT, MZ_EXAD, MZ_EXUNIT, " +
                                  " MZ_REMARK_UP,MZ_ID,MZ_HOUR_LIMIT,MZ_MONEY_LIMIT,(SELECT distinct MZ_POLNO FROM A_DLBASE WHERE MZ_ID=C_OVERTIMEMONTH_HOUR.MZ_ID) AS MZ_POLNO," +
                                  " (SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='26' AND MZ_KCODE=(SELECT distinct MZ_OCCC FROM A_DLBASE WHERE MZ_ID=C_OVERTIMEMONTH_HOUR.MZ_ID)) AS MZ_OCCC," +
                                  " (SELECT distinct MZ_NAME FROM A_DLBASE WHERE MZ_ID=C_OVERTIMEMONTH_HOUR.MZ_ID) AS MZ_NAME," +
                                  " MZ_BUDGET_HOUR , TOTAL ,CASE WHEN MZ_REAL_HOUR>=MZ_HOUR_LIMIT THEN MZ_HOUR_LIMIT ELSE MZ_REAL_HOUR END MZ_REAL_HOUR,MZ_BALANCE_HOUR,PAY1,PROFESS,BOSS," +
                                  " MZ_HOUR_PAY,CASE WHEN MZ_REAL_HOUR>=MZ_HOUR_LIMIT THEN CASE  WHEN MZ_HOUR_PAY*MZ_HOUR_LIMIT > MZ_MONEY_LIMIT THEN MZ_MONEY_LIMIT ELSE MZ_HOUR_PAY*MZ_HOUR_LIMIT END ELSE CASE WHEN MZ_OVERTIME_PAY > MZ_MONEY_LIMIT THEN MZ_MONEY_LIMIT ELSE MZ_OVERTIME_PAY END  END MZ_OVERTIME_PAY,MZ_ID,MZ_VERIFY , " +
                                  " PAY1_UP,PROFESS_UP,BOSS_UP, MZ_HOUR_PAY_UP, MZ_OVERTIME_PAY_UP ,  MZ_OVERTIME_PAY1 , EFF_HOUR_UP " +
                                  " FROM C_OVERTIMEMONTH_HOUR " +
                                  " WHERE " +
                                  "  MZ_VERIFY = 'Y' and  MZ_YEAR='" + o_str.tosql(TextBox_MZ_YEAR.Text.Trim().PadLeft(3, '0')) +
                                  "' AND MZ_MONTH='" + o_str.tosql(DropDownList_MZ_MONTH.SelectedValue) + "'";

            // Joy 合計總數
            string strSUMSQL = "SELECT sum(MZ_OVERTIME_PAY_UP - mz_overtime_pay) as SUM   FROM C_OVERTIMEMONTH_HOUR " +
                                  " WHERE " +
                                  "   MZ_VERIFY = 'Y' and MZ_YEAR='" + o_str.tosql(TextBox_MZ_YEAR.Text.Trim().PadLeft(3, '0')) +
                                  "' AND MZ_MONTH='" + o_str.tosql(DropDownList_MZ_MONTH.SelectedValue) + "'";
            // Joy 新增身份證字號欄位
            // 新增需求 如果身分證有輸入資料的話 機關單位不做判斷
            if (!string.IsNullOrEmpty(MZ_ID.Text))
            {
                strSQL += " AND MZ_ID = '" + MZ_ID.Text + "' ";
                strSUMSQL += " AND MZ_ID = '" + MZ_ID.Text + "' ";
            }

            // 將機關單位查詢抽出來獨立 , 業務加班費使用現服機關 , 非編制機關 
            if (!string.IsNullOrEmpty(DropDownList_EXAD.SelectedValue))
            {
                strSQL += "  AND  MZ_EXAD='" + DropDownList_EXAD.SelectedValue + "' ";
                strSUMSQL += "  AND  MZ_EXAD='" + DropDownList_EXAD.SelectedValue + "' ";
            }
            if (!string.IsNullOrEmpty(DropDownList_EXUNIT.SelectedValue))
            {
                strSQL += "  AND MZ_EXUNIT='" + DropDownList_EXUNIT.SelectedValue + "'";
                strSUMSQL += "  AND MZ_EXUNIT='" + DropDownList_EXUNIT.SelectedValue + "'";
            }


            //20210121 - 人事管理-6.超勤管理作業-6.4 超勤時數審核：清單排序需要調整，順序要跟 6.3.2.超勤印領清冊 一致
            //strSQL += " ORDER BY (SELECT REPLACE(MZ_TBDV,'Z99','999') FROM A_DLBASE WHERE MZ_ID=C_OVERTIMEMONTH_HOUR.MZ_ID),(SELECT MZ_OCCC FROM A_DLBASE WHERE MZ_ID=C_OVERTIMEMONTH_HOUR.MZ_ID),(SELECT MZ_ID FROM A_DLBASE WHERE MZ_ID=C_OVERTIMEMONTH_HOUR.MZ_ID)";
            //strSUMSQL += " ORDER BY (SELECT REPLACE(MZ_TBDV,'Z99','999') FROM A_DLBASE WHERE MZ_ID=C_OVERTIMEMONTH_HOUR.MZ_ID),(SELECT MZ_OCCC FROM A_DLBASE WHERE MZ_ID=C_OVERTIMEMONTH_HOUR.MZ_ID),(SELECT MZ_ID FROM A_DLBASE WHERE MZ_ID=C_OVERTIMEMONTH_HOUR.MZ_ID)";
            strSQL += " ORDER BY (SELECT distinct REPLACE(MZ_TBDV,'Z99','999') FROM A_DLBASE WHERE MZ_ID = C_OVERTIMEMONTH_HOUR.MZ_ID), (SELECT distinct MZ_PCHIEF FROM A_DLBASE WHERE MZ_ID = C_OVERTIMEMONTH_HOUR.MZ_ID), (SELECT distinct MZ_OCCC FROM A_DLBASE WHERE MZ_ID = C_OVERTIMEMONTH_HOUR.MZ_ID), (SELECT distinct MZ_OFFYY FROM   A_DLBASE WHERE  MZ_ID = C_OVERTIMEMONTH_HOUR.MZ_ID) DESC, C_OVERTIMEMONTH_HOUR.MZ_ID";
            strSUMSQL += " ORDER BY (SELECT distinct REPLACE(MZ_TBDV,'Z99','999') FROM A_DLBASE WHERE MZ_ID = C_OVERTIMEMONTH_HOUR.MZ_ID), (SELECT distinct MZ_PCHIEF FROM A_DLBASE WHERE MZ_ID = C_OVERTIMEMONTH_HOUR.MZ_ID), (SELECT distinct MZ_OCCC FROM A_DLBASE WHERE MZ_ID = C_OVERTIMEMONTH_HOUR.MZ_ID), (SELECT distinct MZ_OFFYY FROM   A_DLBASE WHERE  MZ_ID = C_OVERTIMEMONTH_HOUR.MZ_ID) DESC, C_OVERTIMEMONTH_HOUR.MZ_ID";



            temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
            _DT = o_DBFactory.ABC_toTest.Create_Table(strSUMSQL, "GET");

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

        protected void btOK1_Click(object sender, EventArgs e)
        {
            ///TODO Neil 筆記 : 這邊表單命名雖為 EXAD，但實際上是抓發薪機關跟編制單位
            string strSQL = "SELECT MZ_AD, MZ_UNIT, MZ_EXAD, MZ_EXUNIT, " +
                                   " MZ_REMARK_UP,MZ_ID,MZ_HOUR_LIMIT,MZ_MONEY_LIMIT,(SELECT distinct MZ_POLNO FROM A_DLBASE WHERE MZ_ID=C_OVERTIMEMONTH_HOUR.MZ_ID) AS MZ_POLNO," +
                                   " (SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='26' AND MZ_KCODE=(SELECT distinct MZ_OCCC FROM A_DLBASE WHERE MZ_ID=C_OVERTIMEMONTH_HOUR.MZ_ID)) AS MZ_OCCC," +
                                   " (SELECT distinct MZ_NAME FROM A_DLBASE WHERE MZ_ID=C_OVERTIMEMONTH_HOUR.MZ_ID) AS MZ_NAME," +
                                   " MZ_BUDGET_HOUR , TOTAL ,CASE WHEN MZ_REAL_HOUR>=MZ_HOUR_LIMIT THEN MZ_HOUR_LIMIT ELSE MZ_REAL_HOUR END MZ_REAL_HOUR,MZ_BALANCE_HOUR,PAY1,PROFESS,BOSS," +
                                   " MZ_HOUR_PAY,CASE WHEN MZ_REAL_HOUR>=MZ_HOUR_LIMIT THEN CASE  WHEN MZ_HOUR_PAY*MZ_HOUR_LIMIT > MZ_MONEY_LIMIT THEN MZ_MONEY_LIMIT ELSE MZ_HOUR_PAY*MZ_HOUR_LIMIT END ELSE CASE WHEN MZ_OVERTIME_PAY > MZ_MONEY_LIMIT THEN MZ_MONEY_LIMIT ELSE MZ_OVERTIME_PAY END  END MZ_OVERTIME_PAY,MZ_ID,MZ_VERIFY , " +
                                   " PAY1_UP,PROFESS_UP,BOSS_UP, MZ_HOUR_PAY_UP, MZ_OVERTIME_PAY_UP ,  MZ_OVERTIME_PAY1 , EFF_HOUR_UP " +
                                   " FROM C_OVERTIMEMONTH_HOUR " +
                                   " WHERE " +
                                   "  MZ_VERIFY = 'Y' and MZ_YEAR='" + o_str.tosql(TextBox_MZ_YEAR.Text.Trim().PadLeft(3, '0')) +
                                   "' AND MZ_MONTH='" + o_str.tosql(DropDownList_MZ_MONTH.SelectedValue) + "'";

            // Joy 新增身份證字號欄位
            // 新增需求 如果身分證有輸入資料的話 機關單位不做判斷
            if (!string.IsNullOrEmpty(MZ_ID.Text))
            {
                strSQL += " AND MZ_ID = '" + MZ_ID.Text + "' ";

            }

            // 將機關單位查詢抽出來獨立
            if (!string.IsNullOrEmpty(DropDownList_EXAD.SelectedValue))
            {
                strSQL += "  AND  MZ_EXAD='" + DropDownList_EXAD.SelectedValue + "' ";

            }
            if (!string.IsNullOrEmpty(DropDownList_EXUNIT.SelectedValue))
            {
                strSQL += "  AND MZ_EXUNIT='" + DropDownList_EXUNIT.SelectedValue + "'";

            }

            temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");

            //取得使用者之月俸、專業加給、主管加給、每小時支領數等
            TPPDDB.Logic.LogicOvertime Logic = new TPPDDB.Logic.LogicOvertime();
            if (temp.Rows.Count > 0)
            {
                for (int i = 0; i < temp.Rows.Count; i++)
                {
                    //int iHourPay = 0;
                    string MZ_ID = temp.Rows[i]["MZ_ID"].ToString();
                    

                    _2_salary.Police Police = new TPPDDB._2_salary.Police(temp.Rows[i]["MZ_ID"].ToString());
                    Int32 Oldmoney = int.Parse(temp.Rows[i]["MZ_OVERTIME_PAY"].ToString());
                    int iSalary = Police.salary; //月俸
                    int iProfess = Police.profess; //專業加給
                    //Int32 iBoss = Police.boss; //主管加給 , 要抓當時舊的BOSS , 而不是新的BOSS
                    int iBoss = int.Parse(temp.Rows[i]["Boss"].ToString());
                    Police.boss = iBoss;
                    //取得每小時薪資金額,用於計算加班費 回傳整數
                    var cf = new CFService();
                    int iHourPay = cf.GetPayUnit_int(Police);

                    // Convert.ToInt32(MathHelper.Round(Logic.getHourPay(temp.Rows[i]["MZ_ID"].ToString()), 0, MidpointRounding.AwayFromZero)); //每小時金額
                    Int32 iallmoney = iHourPay * int.Parse(temp.Rows[i]["Total"].ToString());
                    Int32 Hour1 = 0;
                    Int32 lefthour = 0; //時數餘數 
                    Int32 OverTimeMoney = 0;
                    lefthour = 0;
                    OverTimeMoney = iallmoney - Oldmoney;
                    #region 時數>20不做處理
                    //Hour1 = (int)((iallmoney - Oldmoney) / iHourPay);
                    //if ((int.Parse(temp.Rows[i]["Total"].ToString()) + Hour1) > 20)
                    //{
                    //       lefthour = int.Parse(temp.Rows[i]["Total"].ToString()) + Hour1 - 20 ;
                    //       OverTimeMoney = iallmoney - Oldmoney - lefthour * iHourPay;
                    //}
                    //else
                    //{
                    //       lefthour = 0;
                    //       OverTimeMoney = iallmoney - Oldmoney;
                    //}
                    // Int32 iallmoney = iHourPay * int.Parse(temp.Rows[i]["Total"].ToString());
                    #endregion
                    //更新金額
                    strSQL = "update C_OVERTIMEMONTH_HOUR set PAY1_UP = " + iSalary + ", Profess_up = " + iProfess + " , Boss_up = " + iBoss + " , MZ_HOUR_PAY_UP = " + iHourPay + " , EFF_HOUR_UP = " + lefthour + " , MZ_OVERTIME_PAY_UP = " + iallmoney + " , MZ_OVERTIME_PAY1 = " + OverTimeMoney;
                    strSQL += " WHERE MZ_ID = '" + temp.Rows[i]["MZ_ID"].ToString() + "'";
                    strSQL += "  AND MZ_YEAR='" + o_str.tosql(TextBox_MZ_YEAR.Text.Trim().PadLeft(3, '0'));
                    strSQL += "' AND MZ_MONTH='" + o_str.tosql(DropDownList_MZ_MONTH.SelectedValue) + "'";
                    o_DBFactory.ABC_toTest.vExecSQL(strSQL);
                }

                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('加班費補發作業完成!!')", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('本月無任何加班費印領清冊')", true);
            }
            btOK_Click(sender, e);

            //if (temp.Rows.Count > 0)
            //{
            //    Session["OVERTIME_VERIFY_TEMP"] = temp;
            //    ViewState["unit"] = DropDownList_EXUNIT.SelectedItem.Text;
            //    ViewState["year"] = TextBox_MZ_YEAR.Text.Trim().PadLeft(3, '0');
            //    ViewState["month"] = DropDownList_MZ_MONTH.SelectedValue;
            //    GridView1.DataSource = temp;
            //    GridView1.DataBind();
            //}
            //else
            //{
            //    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('無搜尋條件下之資料，請檢查')", true);
            //}
        }

        protected void TextBox_MZ_OVERTIME_PAY_TextChanged(object sender, EventArgs e)
        {
            temp = Session["OVERTIME_VERIFY_TEMP"] as DataTable;

            GridViewRow gvr = (GridViewRow)((TextBox)sender).NamingContainer;
            GridView gv = (GridView)gvr.NamingContainer;

            int rowCount = gv.Rows.Count;
            int index = gvr.RowIndex;

            string updateYear = TextBox_MZ_YEAR.Text;
            string updateMonth = DropDownList_MZ_MONTH.SelectedValue;
            //string MZ_AD = DropDownList_EXAD.SelectedValue;
            //string MZ_EXUNIT = DropDownList_EXUNIT.SelectedValue;
            string MZ_AD = temp.Rows[index]["MZ_AD"].ToString();
            string MZ_EXUNIT = temp.Rows[index]["MZ_EXUNIT"].ToString();
            string MZ_ID = temp.Rows[index]["MZ_ID"].ToString();

            // 超勤金額文字
            TextBox tbmop = (TextBox)gv.Rows[index].FindControl("TextBox_MZ_OVERTIME_PAY"); //計算完主管加給後，
            string tbmopNum = tbmop.Text.Trim();

            string updateSQL = "UPDATE C_OVERTIMEMONTH_HOUR SET MZ_OVERTIME_PAY = " + tbmopNum
                                                     + " WHERE  MZ_YEAR = " + updateYear + " AND MZ_MONTH = " + updateMonth +
                                                      " AND MZ_AD  = '" + MZ_AD + "' AND MZ_ID = '" + MZ_ID + "'  AND MZ_EXUNIT = '" + MZ_EXUNIT + "'";

            o_DBFactory.ABC_toTest.Edit_Data(updateSQL);
        }

        protected void TextBox_MZ_REMARK_TextChanged(object sender, EventArgs e)
        {
            temp = Session["OVERTIME_VERIFY_TEMP"] as DataTable;

            GridViewRow gvr = (GridViewRow)((TextBox)sender).NamingContainer;
            GridView gv = (GridView)gvr.NamingContainer;

            int rowCount = gv.Rows.Count;
            int index = gvr.RowIndex;
            string updateYear = TextBox_MZ_YEAR.Text;
            string updateMonth = DropDownList_MZ_MONTH.SelectedValue;
            //string MZ_AD = DropDownList_EXAD.SelectedValue;
            //string MZ_EXUNIT = DropDownList_EXUNIT.SelectedValue;
            string MZ_EXAD = temp.Rows[index]["MZ_EXAD"].ToString();
            string MZ_EXUNIT = temp.Rows[index]["MZ_EXUNIT"].ToString();
            string MZ_ID = temp.Rows[index]["MZ_ID"].ToString();

            // Remark 備註欄文字
            TextBox tbrm = new TextBox();
            tbrm = (TextBox)gv.Rows[index].FindControl("TXT_MZ_REMARK");
            string tbrmmemo = tbrm.Text.Trim();

            // 實際支領時數文字
            //TextBox tbrh = new TextBox();

            string tbrh = temp.Rows[index]["TOTAL"].ToString();
            string tbrhRealNum = tbrh.Trim();

            // 結餘時數文字
            //TextBox tbmrh = (TextBox)gv.Rows[index].FindControl("TextBox_MZ_BALANCE_HOUR");
            //string tbmrhNum = tbmrh.Text.Trim();

            // 本俸文字
            TextBox tbpay1 = (TextBox)gv.Rows[index].FindControl("TextBox_PAY1_UP");
            string tbpay1Num = tbpay1.Text.Trim();

            // 專業加給文字
            TextBox tbpro = (TextBox)gv.Rows[index].FindControl("TextBox_PROFESS_UP");
            string tbproNum = tbpro.Text.Trim();


            // 主管加給文字
            TextBox tbboss = (TextBox)gv.Rows[index].FindControl("TextBox_BOSS_UP");
            string tbbossNum = tbboss.Text.Trim();

            //重新計算每小時金額
            Int32 CustomerBoss = 0; Int32.TryParse(tbbossNum, out CustomerBoss);
            //  String ReduceHourPay = Salary.reduceHourPay(MZ_ID, CustomerBoss); //若失敗則會直接取原先 B_BASE 內的值
            String ReduceHourPay = MathHelper.Round((decimal.Parse(tbpay1Num) + decimal.Parse(tbproNum) + decimal.Parse(tbbossNum)) / 240, 0).ToString();
            // 每小時金額
            TextBox HourPay = (TextBox)gv.Rows[index].FindControl("HOUR_PAY_UP");
            HourPay.Text = ReduceHourPay;




            //string tbmopNum = tbmop.Text.Trim();
            string tbmopNum = (int.Parse(tbrhRealNum) * int.Parse(ReduceHourPay)).ToString();


            Int32 Hour1 = 0;
            Int32 lefthour = 0; //時數餘數 
            Int32 OverTimeMoney = 0;
            Int32 iallmoney = int.Parse(tbmopNum);
            Int32 Oldmoney = int.Parse(temp.Rows[index]["MZ_OVERTIME_PAY"].ToString());


            Hour1 = (int)((iallmoney - Oldmoney) / int.Parse(ReduceHourPay));
            if ((int.Parse(temp.Rows[index]["Total"].ToString()) + Hour1) > 20)
            {
                lefthour = int.Parse(temp.Rows[index]["Total"].ToString()) + Hour1 - 20;
                OverTimeMoney = iallmoney - Oldmoney - lefthour * int.Parse(ReduceHourPay);
            }
            else
            {
                lefthour = 0;
                OverTimeMoney = iallmoney - Oldmoney;
            }

            //補發金額 , 本年度可以調整 Mark , 但要有備註說明
            TextBox tbnew = new TextBox();
            tbnew = (TextBox)gv.Rows[index].FindControl("txt_MZ_OVERTIME_PAY1");
            string tbnewmoney = tbnew.Text.Trim();

            if (tbrmmemo.Length > 0)
            {
                OverTimeMoney = int.Parse(tbnewmoney);
            }

            //string updateSQL = "UPDATE C_OVERTIMEMONTH_HOUR SET MZ_REAL_HOUR = " + tbrhRealNum + " , MZ_REMARK = '" + tbrmmemo
            //                                     + "' , MZ_OVERTIME_PAY = " + tbmopNum + ",MZ_BALANCE_HOUR = " + tbmrhNum + ",BOSS = " + tbbossNum
            //                                     + ", MZ_HOUR_PAY = " + ReduceHourPay
            //                                     + " WHERE  MZ_YEAR = " + updateYear + " AND MZ_MONTH = " + updateMonth +
            //                                      " AND MZ_AD  = '" + MZ_AD + "' AND MZ_ID = '" + MZ_ID + "'  AND MZ_EXUNIT = '" + MZ_EXUNIT + "'";
            string updateSQL = "UPDATE C_OVERTIMEMONTH_HOUR SET  BOSS_UP = " + tbbossNum + " , PROFESS_UP = " + tbproNum + " , PAY1_UP = " + tbpay1Num + " " + " , EFF_HOUR_UP = " + lefthour + " " + " , MZ_OVERTIME_PAY1 = " + OverTimeMoney + ", MZ_REMARK_UP = '" + tbrmmemo + "' "
                                                     + " , MZ_HOUR_PAY_UP = " + ReduceHourPay + " , MZ_OVERTIME_PAY_UP = " + tbmopNum + " WHERE  MZ_YEAR = " + updateYear + " AND MZ_MONTH = " + updateMonth +
                                                      " AND MZ_EXAD  = '" + MZ_EXAD + "' AND MZ_ID = '" + MZ_ID + "'  AND MZ_EXUNIT = '" + MZ_EXUNIT + "'";

            o_DBFactory.ABC_toTest.Edit_Data(updateSQL);
            //}
            btOK_Click(sender, e);
        }


        protected void TextBox_MZ_REAL_HOUR_TextChanged(object sender, EventArgs e)
        {
            temp = Session["OVERTIME_VERIFY_TEMP"] as DataTable;

            GridViewRow gvr = (GridViewRow)((TextBox)sender).NamingContainer;
            GridView gv = (GridView)gvr.NamingContainer;

            int rowCount = gv.Rows.Count;
            int index = gvr.RowIndex;

            TextBox tb = new TextBox();

            tb = (TextBox)gv.Rows[index].FindControl("TextBox_MZ_REAL_HOUR");

            //TextBox tb = (TextBox)gv.Rows[index].Cells[36].Controls[1] as TextBox;

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

                        //temp.Rows[index]["MZ_BALANCE_HOUR"] = (int.Parse(MZ_BUDGET_HOUR) - int.Parse(tb.Text)).ToString();
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
                    //temp.Rows[index]["MZ_BALANCE_HOUR"] = (int.Parse(MZ_BUDGET_HOUR) - int.Parse(tb.Text)).ToString();
                    temp.Rows[index]["MZ_OVERTIME_PAY"] = string.IsNullOrEmpty(MML) ? MathHelper.Round(float.Parse(MZ_HOUR_PAY) * int.Parse(tb.Text)) : (MathHelper.Round(float.Parse(MZ_HOUR_PAY) * int.Parse(tb.Text)) > double.Parse(MML) ? int.Parse(MML) : MathHelper.Round(float.Parse(MZ_HOUR_PAY) * int.Parse(tb.Text)));
                    temp.Rows[index]["MZ_OVERTIME_PAY"] = Convert.ToInt32(temp.Rows[index]["MZ_OVERTIME_PAY"].ToString()) > CFService.Limit_MZ_OVERTIME_PAY ? CFService.Limit_MZ_OVERTIME_PAY : Convert.ToInt32(temp.Rows[index]["MZ_OVERTIME_PAY"].ToString());
                    temp.Rows[index]["MZ_REAL_HOUR"] = tb.Text;
                }
            }

            GridView1.DataSource = temp;
            GridView1.DataBind();
            Session["OVERTIME_VERIFY_TEMP"] = temp;
            //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('C_UpdateMemo.aspx?','備註','top=190,left=200,width=500,height=250,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }

        protected void DropDownList_EXUNIT_DataBound(object sender, EventArgs e)
        {
            ListItem li = new ListItem(" ", "");
            DropDownList_EXUNIT.Items.Insert(0, li);
        }
    }
}
