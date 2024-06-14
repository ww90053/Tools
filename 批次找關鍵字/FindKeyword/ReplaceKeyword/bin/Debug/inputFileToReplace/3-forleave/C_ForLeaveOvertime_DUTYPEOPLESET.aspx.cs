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
    public partial class C_DUTYPEOPLESET : System.Web.UI.Page
    {
        string DUTYDATE = (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0') + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                C.check_power();
            
            //by MQ 20100312---------   
            C.set_Panel_EnterToTAB(ref this.Panel1);

            string sql = @"SELECT AKD.MZ_KCHI MZ_AD,  AKU.MZ_KCHI MZ_UNIT
                             FROM A_DLBASE
                            LEFT JOIN  A_KTYPE AKD ON AKD.MZ_KCODE=MZ_AD AND AKD.MZ_KTYPE='04' 
                            LEFT JOIN  A_KTYPE AKU ON AKU.MZ_KCODE=MZ_UNIT AND AKU.MZ_KTYPE='25' 
                            WHERE MZ_ID='" + Session["ADPMZ_ID"].ToString() + "'";

            DataTable ad_unit = o_DBFactory.ABC_toTest.Create_Table(sql, "get");
            if (ad_unit.Rows.Count > 0)
                Label1.Text = ad_unit.Rows[0]["MZ_EXAD"].ToString() + ad_unit.Rows[0]["MZ_EXUNIT"].ToString() + "勤務分配表輪番設定";
            
            }
        }
        protected void bt1_Click(object sender, EventArgs e)
        {
            string DeleteString = "DELETE FROM C_DUTYPEOPLE WHERE MZ_AD='" + Session["ADPMZ_EXAD"].ToString() + "' AND MZ_UNIT='" + Session["ADPMZ_EXUNIT"].ToString() + "' AND DUTYDATE>='" + DUTYDATE + "'";

            try
            {
                o_DBFactory.ABC_toTest.Edit_Data(DeleteString);
            }
            catch (Exception)
            {
                throw;
            }

            string InsertString = "INSERT INTO C_DUTYPEOPLE(MZ_ID,MZ_AD,MZ_UNIT,DUTYDATE,ITEM) SELECT '   ',MZ_AD,MZ_UNIT,'" + DUTYDATE + "',ROW_NUMBER() OVER (ORDER BY MZ_ID) FROM A_DLBASE WHERE MZ_AD='" + Session["ADPMZ_EXAD"].ToString() + "' AND MZ_UNIT='" + Session["ADPMZ_EXUNIT"].ToString() + "'";
            try
            {
                o_DBFactory.ABC_toTest.Edit_Data(InsertString);
            }
            catch (Exception)
            {

                throw;
            }
            GridView1.DataBind();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();
                string strSQL;
                try
                {
                    foreach (GridViewRow row in GridView1.Rows)
                    {
                        Label lb = (Label)row.Cells[2].FindControl("Label_MZ_ID");
                        TextBox tb = (TextBox)row.Cells[4].FindControl("TextBox_MZ_PNO");
                        TextBox tb1 = (TextBox)row.Cells[5].FindControl("TextBox_MZ_CNOBEGIN");
                        TextBox tb2 = (TextBox)row.Cells[6].FindControl("TextBox_MZ_CNOEND");
                        TextBox tb3 = (TextBox)row.Cells[7].FindControl("TextBox_MZ_CNO");

                        if (lb.Text.Trim() != "")
                        {
                            strSQL = "UPDATE C_DUTYPEOPLE SET " +
                                                              "  MZ_PNO='" + tb.Text.Trim() +
                                                              "',MZ_CNOBEGIN='" + tb1.Text.Trim() +
                                                              "',MZ_CNOEND='" + tb2.Text.Trim() +
                                                              "',MZ_CNO='" + tb3.Text.Trim() +
                                                              "',MZ_ID='" + lb.Text.Trim() +

                                                              "' WHERE ITEM='" + row.Cells[0].Text.Trim() +
                                                              "' AND MZ_AD='" + Session["ADPMZ_EXAD"].ToString() +
                                                              "' AND MZ_UNIT='" + Session["ADPMZ_EXUNIT"].ToString() +
                                                              "' AND DUTYDATE='" + DUTYDATE + "'";
                        }
                        else
                        {
                            strSQL = "DELETE FROM C_DUTYPEOPLE WHERE ITEM='" + row.Cells[0].Text.Trim() +
                                                              "' AND MZ_AD='" + Session["ADPMZ_EXAD"].ToString() +
                                                              "' AND MZ_UNIT='" + Session["ADPMZ_EXUNIT"].ToString() +
                                                              "' AND DUTYDATE='" + DUTYDATE + "'";
                        }
                        SqlCommand cmd = new SqlCommand(strSQL, conn);
                        cmd.Transaction = transaction;
                        cmd.ExecuteNonQuery();

                    }
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                }
                finally
                {
                    GridView1.DataBind();

                    conn.Close();

                    //XX2013/06/18 
                    conn.Dispose();
                }
            }
            //計算次月的最後一天
            int MonthLastDay = DateTime.Parse((int.Parse(DUTYDATE.Substring(0, 3)) + 1911).ToString() + "/" + DUTYDATE.Substring(3, 2) + "/" + "01").AddMonths(1).AddDays(-1).Day;
            //次月的日期
            string DUTYDATE2 = (DateTime.Now.AddMonths(1).Year - 1911).ToString() + (DateTime.Now.AddMonths(1).Month).ToString().PadLeft(2, '0') + "00";
            //因為要塞30天資料...所以先把今天到月底的日期差先算出來
            int DayDiff = MonthLastDay - DateTime.Now.Day;

            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();
                string strSQL;
                try
                {
                    for (int i = 1; i <= 30; i++)
                    {
                        if (i <= DayDiff)
                        {
                            strSQL = " INSERT INTO C_DUTYPEOPLE" +
                                     " SELECT MZ_ID," +
                                             "'" + (int.Parse(DUTYDATE) + i).ToString().PadLeft(7, '0') + "'," +
                                             "MZ_PNO," +
                                             "MZ_CNOBEGIN," +
                                             "MZ_CNOEND," +
                                             "CASE WHEN dbo.to_number(MZ_CNO)+1<=dbo.to_number(MZ_CNOEND) THEN CONVERT(VARCHAR(200),dbo.to_number(MZ_CNO)+1) ELSE MZ_CNOBEGIN END, " +
                                             "MZ_AD," +
                                             "MZ_UNIT," +
                                             "ITEM" +
                                     " FROM C_DUTYPEOPLE " +
                                     " WHERE  MZ_AD='" + Session["ADPMZ_EXAD"].ToString() +
                                       "' AND MZ_UNIT='" + Session["ADPMZ_EXUNIT"].ToString() +
                                       "' AND DUTYDATE='" + DUTYDATE + "'";
                        }
                        else
                        {

                            strSQL = " INSERT INTO C_DUTYPEOPLE" +
                                     " SELECT MZ_ID," +
                                             "'" + (int.Parse(DUTYDATE2) + i - DayDiff).ToString().PadLeft(7, '0') + "'," +
                                             "MZ_PNO," +
                                             "MZ_CNOBEGIN," +
                                             "MZ_CNOEND," +
                                             "CASE WHEN dbo.to_number(MZ_CNO)+1<=dbo.to_number(MZ_CNOEND) THEN CONVERT(VARCHAR(200),dbo.to_number(MZ_CNO)+1) ELSE MZ_CNOBEGIN END, " +
                                             "MZ_AD," +
                                             "MZ_UNIT," +
                                             "ITEM" +
                                     " FROM C_DUTYPEOPLE " +
                                     " WHERE  MZ_AD='" + Session["ADPMZ_EXAD"].ToString() +
                                       "' AND MZ_UNIT='" + Session["ADPMZ_EXUNIT"].ToString() +
                                       "' AND DUTYDATE='" + DUTYDATE + "'";
                        }

                        SqlCommand cmd = new SqlCommand(strSQL, conn);
                        cmd.Transaction = transaction;
                        cmd.ExecuteNonQuery();

                    }
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                }
                finally
                {
                    GridView1.DataBind();

                    conn.Close();

                    //XX2013/06/18 
                    conn.Dispose();
                }
            }
        }

        protected void TextBox_MZ_CNOBEGIN_TextChanged(object sender, EventArgs e)
        {

            GridViewRow gvr = (GridViewRow)((TextBox)sender).NamingContainer;

            GridView gv = (GridView)gvr.NamingContainer;
            int rowCount = gv.Rows.Count;
            int index = gvr.RowIndex;

            if (index == 0)
            {
                string CNOBEGIN_string = (gv.Rows[index].Cells[5].Controls[1] as TextBox).Text;
                for (int i = 1; i < rowCount; i++)
                {
                    TextBox CNOBEGIN = (TextBox)gv.Rows[i].Cells[5].Controls[1];
                    CNOBEGIN.Text = CNOBEGIN_string;
                }
            }
            else
            {
                string CNOBEGIN_string = (gv.Rows[index].Cells[5].Controls[1] as TextBox).Text;
                for (int i = index + 1; i < rowCount; i++)
                {
                    TextBox CNOBEGIN = (TextBox)gv.Rows[i].Cells[5].Controls[1];
                    CNOBEGIN.Text = CNOBEGIN_string;
                }
            }
        }

        protected void TextBox_MZ_CNOEND_TextChanged(object sender, EventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((TextBox)sender).NamingContainer;
            GridView gv = (GridView)gvr.NamingContainer;

            int rowCount = gv.Rows.Count;
            int index = gvr.RowIndex;

            if (index == 0)
            {
                string CNOBEGIN_string = (gv.Rows[index].Cells[6].Controls[1] as TextBox).Text;
                for (int i = 1; i < rowCount; i++)
                {
                    TextBox CNOBEGIN = (TextBox)gv.Rows[i].Cells[6].Controls[1];
                    CNOBEGIN.Text = CNOBEGIN_string;
                }
            }
            else
            {
                string CNOBEGIN_string = (gv.Rows[index].Cells[6].Controls[1] as TextBox).Text;
                for (int i = index + 1; i < rowCount; i++)
                {
                    TextBox CNOBEGIN = (TextBox)gv.Rows[i].Cells[6].Controls[1];
                    CNOBEGIN.Text = CNOBEGIN_string;
                }
            }
        }

        protected void TextBox_MZ_CNO_TextChanged(object sender, EventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((TextBox)sender).NamingContainer;
            GridView gv = (GridView)gvr.NamingContainer;

            int rowCount = gv.Rows.Count;
            int index = gvr.RowIndex;

            int intCNOBEGIN = int.Parse((gv.Rows[index].Cells[5].Controls[1] as TextBox).Text);
            int intCONEND = int.Parse((gv.Rows[index].Cells[6].Controls[1] as TextBox).Text);
            int intCNO = int.Parse((gv.Rows[index].Cells[7].Controls[1] as TextBox).Text);

            int j = 0;
            for (int i = index + 1; i < rowCount; i++)
            {
                j++;

                TextBox CNOBEGIN = (TextBox)gv.Rows[i].Cells[5].Controls[1];
                TextBox CNOEND = (TextBox)gv.Rows[i].Cells[6].Controls[1];
                TextBox CNO = (TextBox)gv.Rows[i].Cells[7].Controls[1];

                if (intCONEND.ToString() == (gv.Rows[i - 1].Cells[7].Controls[1] as TextBox).Text)
                {
                    CNO.Text = CNOBEGIN.ToString();
                    j = 0;
                    intCNO = intCNOBEGIN;
                }

                CNO.Text = (intCNO + j).ToString();
            }
        }

        protected void ComboBox1_TextChanged(object sender, EventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((ComboBox)sender).NamingContainer;
            GridView gv = (GridView)gvr.NamingContainer;

            int rowCount = gv.Rows.Count;
            int index = gvr.RowIndex;

            ComboBox cb = (gv.Rows[index].Cells[1].Controls[1] as ComboBox);
            Label lb = (gv.Rows[index].Cells[2].Controls[1] as Label);

            for (int i = 0; i < rowCount; i++)
            {
                if (i != index)
                {
                    ComboBox cb2 = (gv.Rows[i].Cells[1].Controls[1] as ComboBox);
                    if (cb.Text == cb2.Text && cb2.Text != "")
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('人員重複,請重新選取！');", true);
                        cb.Text = "";
                        cb.Focus();
                        lb.Text = "";
                    }
                }
            }

            string ID = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_ID FROM A_DLBASE WHERE RTRIM(MZ_NAME)='" + cb.Text.Trim() + "' AND MZ_EXAD='" + Session["ADPMZ_EXAD"].ToString() + "' AND MZ_EXUNIT='" + Session["ADPMZ_EXUNIT"].ToString() + "'");

            if (string.IsNullOrEmpty(ID))
            {
                cb.Focus();
            }
            else
            {
                lb.Text = ID;
            }
        }
    }
}
