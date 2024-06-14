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
using System.Web.Configuration;
using System.Data.SqlClient;
using AjaxControlToolkit;
using System.Drawing;
using System.Collections.Generic;
using TPPDDB._2_salary;
using TPPDDB.App_Code;
using TPPDDB.Helpers;

namespace TPPDDB._3_forleave
{
    public partial class C_ForLeaveOvertime_Inside_Overtime : System.Web.UI.Page
    {
       

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                C.check_power();
                ViewState["C_strGID"] = o_a_Function.strGID(Session["ADPMZ_ID"].ToString());
            }

            ///群組權限
            //C_strGID = o_a_Function.strGID(Session["ADPMZ_ID"].ToString());

            ViewState["MZ_MONTH"] = Request["SEARCH_MONTH"]; //(DateTime.Now.Year - 1911).ToString().PadLeft(3, '0') + DateTime.Now.Month.ToString().PadLeft(2, '0');

            ViewState["AD"] = Request["AD"];
            ViewState["UNIT"] = Request["UNIT"];

            if (ViewState["AD"] == null)
                ViewState["AD"] = Session["ADPMZ_EXAD"].ToString();

            if (ViewState["UNIT"] == null)
                ViewState["UNIT"] = Session["ADPMZ_EXUNIT"].ToString();



          

            if (!Page.IsPostBack)
            { 
                //MQ-----------------------20100331
                C.set_Panel_EnterToTAB(ref this.Panel2);

                string sql = @"SELECT MZ_NAME, AKD.MZ_KCHI MZ_AD,  AKU.MZ_KCHI MZ_UNIT
                             FROM A_DLBASE
                            LEFT JOIN  A_KTYPE AKD ON AKD.MZ_KCODE=MZ_AD AND AKD.MZ_KTYPE='04' 
                            LEFT JOIN  A_KTYPE AKU ON AKU.MZ_KCODE=MZ_UNIT AND AKU.MZ_KTYPE='25' 
                            WHERE MZ_ID='" + Session["ADPMZ_ID"].ToString() + "'";

                DataTable ad_unit = o_DBFactory.ABC_toTest.Create_Table(sql, "get");
                //if (ad_unit.Rows.Count > 0)
                //    Label1.Text = ad_unit.Rows[0]["MZ_AD"].ToString() + ad_unit.Rows[0]["MZ_UNIT"].ToString() + ad_unit.Rows[0]["MZ_NAME"].ToString();



                DL_DATABIND(ViewState["AD"].ToString(), ViewState["UNIT"].ToString());

                if (ViewState["AD"].ToString() == Session["ADPMZ_EXAD"].ToString() && ViewState["UNIT"].ToString() == Session["ADPMZ_EXUNIT"].ToString())
                {
                    DropDownList_MZ_NAME.SelectedValue = Session["ADPMZ_ID"].ToString();
                    TextBox_MZ_ID.Text = Session["ADPMZ_ID"].ToString();
                }
                else
                {
                    DropDownList_MZ_NAME.SelectedIndex = 0;
                    TextBox_MZ_ID.Text = DropDownList_MZ_NAME.SelectedValue;
                }
                TextBox_MZ_DATE.Text = o_CommonService.Personal_ReturnDateString((DateTime.Now.Year - 1911).ToString().PadLeft(3, '0') + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0'));

                TextBox_MZ_EXUNIT.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT (SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=MZ_EXUNIT AND MZ_KTYPE='25') FROM A_DLBASE WHERE MZ_ID='" + DropDownList_MZ_NAME.SelectedValue + "'");



                TextBox_MZ_OCCC.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT (SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=MZ_OCCC AND MZ_KTYPE='26') FROM A_DLBASE WHERE MZ_ID='" + DropDownList_MZ_NAME.SelectedValue + "'");

                TextBox_MZ_POLNO.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_POLNO FROM A_DLBASE WHERE MZ_ID='" + DropDownList_MZ_NAME.SelectedValue + "'");

                TextBox_HOUR_PAY.Text = Hour_Pay(DropDownList_MZ_NAME.SelectedValue);

                GV_DATABIND(DropDownList_MZ_NAME.SelectedValue);

                CountAll(o_str.tosql(TextBox_MZ_ID.Text));

                
            }
        }

        

        protected void DL_DATABIND(string AD, string UNIT)
        {
            DataTable dl_dt = new DataTable();

            string dl_select_string = "SELECT MZ_NAME,MZ_ID FROM A_DLBASE WHERE MZ_EXAD='" + AD + "' AND MZ_EXUNIT='" + UNIT + "' AND MZ_STATUS2 = 'Y' ";

            dl_dt = o_DBFactory.ABC_toTest.Create_Table(dl_select_string, "get_dl_data");

            DropDownList_MZ_NAME.DataSource = dl_dt;

            DropDownList_MZ_NAME.DataTextField = "MZ_NAME";

            DropDownList_MZ_NAME.DataValueField = "MZ_ID";

            DropDownList_MZ_NAME.DataBind();

        }

        protected void GV_DATABIND(string MZ_ID)
        {
            DataTable gd_dt = new DataTable();

            //string S = Session["MZ_MONTH"].ToString();

            List<SqlParameter> ps = new List<SqlParameter>();


            string gd_select_string = @"SELECT MZ_CHK,CONVERT(VARCHAR(200),dbo.to_number(dbo.SUBSTR(MZ_DATE,1,3)))+'/'+dbo.SUBSTR(MZ_DATE,4,2)+'/'+
                dbo.SUBSTR(MZ_DATE,6,2) as MZ_DATE,OTREASON,OTIME,HOUR_PAY,PAY_SUM,RESTFLAG,LOCK_FLAG FROM C_OVERTIME_HOUR_INSIDE WHERE MZ_ID=@MZ_ID";

            ps.Add(new SqlParameter("MZ_ID", MZ_ID));

            gd_select_string += " AND MZ_DATE like @MZ_DATE";
            if (ViewState["MZ_MONTH"] != null && ViewState["MZ_MONTH"].ToString() != "00000")
            {

                ps.Add(new SqlParameter("MZ_DATE", ViewState["MZ_MONTH"].ToString() + "%"));
            }
            else
            {
                ps.Add(new SqlParameter("MZ_DATE", string.Format("{0}{1}%", DateTime.Now.Year - 1911, DateTime.Now.Month.ToString().PadLeft(2, '0'))));
            }

            gd_select_string += " ORDER BY dbo.to_number(C_OVERTIME_HOUR_INSIDE.MZ_DATE) DESC";
            gd_dt = o_DBFactory.ABC_toTest.DataSelect(gd_select_string, ps);

            Session["Inside_Overtime_gd_dt"] = gd_dt;

            GridView1.DataSource = gd_dt;

            //GridView1.AllowPaging = true;

            //GridView1.PageSize = 8;
            //
            GridView1.DataBind();
        }

        /// <summary>每小時薪資</summary>
        protected string Hour_Pay(string MZ_ID)
        {
            _2_salary.Police Police = new TPPDDB._2_salary.Police(MZ_ID);

            if (Police.occc.Substring(0, 2) == "Z0")
                return MathHelper.Round(Convert.ToDouble(Police.salary + Police.profess + Police.boss) / 240 * 1.33).ToString();
            else
                return MathHelper.Round(Convert.ToDouble(Police.salary + Police.profess + Police.boss) / 240).ToString();
        }

        protected void DropDownList_MZ_NAME_SelectedIndexChanged(object sender, EventArgs e)
        {
            TextBox_MZ_EXUNIT.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT (SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=MZ_EXUNIT AND MZ_KTYPE='25') FROM A_DLBASE WHERE MZ_ID='" + DropDownList_MZ_NAME.SelectedValue.Trim() + "'");
            TextBox_MZ_ID.Text = DropDownList_MZ_NAME.SelectedValue.Trim();
            TextBox_MZ_OCCC.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT (SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=MZ_OCCC AND MZ_KTYPE='26') FROM A_DLBASE WHERE MZ_ID='" + DropDownList_MZ_NAME.SelectedValue.Trim() + "'");
            TextBox_MZ_POLNO.Text = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_POLNO FROM A_DLBASE WHERE MZ_ID='" + DropDownList_MZ_NAME.SelectedValue.Trim() + "'");
            TextBox_HOUR_PAY.Text = Hour_Pay(DropDownList_MZ_NAME.SelectedValue.Trim());
            GV_DATABIND(DropDownList_MZ_NAME.SelectedValue);

        }

        protected void btInsert_Click(object sender, EventArgs e)
        {
            string tmpOverDay = ForDateTime.TWDateToRCDate(TextBox_MZ_DATE.Text);
            string strDate = "2021/11/01";   
            DateTime date = Convert.ToDateTime(strDate);
            DateTime overday = Convert.ToDateTime(tmpOverDay);
            if (!(bool)ForDateTime.CheckDateTime(tmpOverDay))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('加班日期格式錯誤!');", true);
                return;
            }
            else
            {
                if (DateTime.Compare(overday, date) > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('舊加班系統禁止新增自110/11/1以後加班資料!');", true);
                    return;
                }
            }

            string strSQL = "INSERT INTO" +
                          " C_OVERTIME_HOUR_INSIDE (MZ_EXUNIT,MZ_ID,MZ_OCCC,RESTFLAG,OTIME,HOUR_PAY,PAY_SUM,OTREASON," +
                                                   "VFLAG,MSUM,INS_ID,INS_DATE,MZ_EXAD,MZ_DATE,MZ_AD,MZ_UNIT)" +
                                          " VALUES (@MZ_EXUNIT,@MZ_ID,@MZ_OCCC,@RESTFLAG,@OTIME,@HOUR_PAY,@PAY_SUM,@OTREASON," +
                                                   ":VFLAG,@MSUM,@INS_ID,@INS_DATE,@MZ_EXAD,@MZ_DATE,@MZ_AD,@MZ_UNIT) ";

            var qMZ_AD = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_AD FROM VW_A_DLBASE_S1 WHERE MZ_ID='" + TextBox_MZ_ID.Text + "'");
            var qMZ_UNIT = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_UNIT FROM VW_A_DLBASE_S1 WHERE MZ_ID='" + TextBox_MZ_ID.Text + "'");

            SqlParameter[] parameterList = {
            new SqlParameter("MZ_EXUNIT",SqlDbType.VarChar){Value = ViewState["UNIT"].ToString()},
            new SqlParameter("MZ_ID",SqlDbType.VarChar){Value = TextBox_MZ_ID.Text},
            new SqlParameter("MZ_OCCC",SqlDbType.VarChar){Value = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_OCCC FROM A_DLBASE WHERE MZ_ID='"+DropDownList_MZ_NAME.SelectedValue.Trim()+"'")},
            new SqlParameter("RESTFLAG",SqlDbType.VarChar){Value =RadioButtonList_RESTFLAG.SelectedValue},
            new SqlParameter("OTIME",SqlDbType.Float){Value = 0},
            new SqlParameter("HOUR_PAY",SqlDbType.Float){Value = TextBox_HOUR_PAY.Text},
            new SqlParameter("PAY_SUM",SqlDbType.Float){Value = 0},
            new SqlParameter("OTREASON",SqlDbType.VarChar){Value = TextBox_OTREASON.Text},
            new SqlParameter("VFLAG",SqlDbType.VarChar){Value = Convert.DBNull},
            new SqlParameter("MSUM",SqlDbType.Float){Value =0},
            new SqlParameter("INS_ID",SqlDbType.VarChar){Value = Session["ADPMZ_ID"].ToString()},
            new SqlParameter("INS_DATE",SqlDbType.VarChar){Value = (DateTime.Now.Year-1911).ToString().PadLeft(3,'0')+DateTime.Now.Month.ToString().PadLeft(2,'0')+DateTime.Now.Day.ToString().PadLeft(2,'0')},
            new SqlParameter("MZ_DATE",SqlDbType.VarChar){Value = TextBox_MZ_DATE.Text.Trim().Replace("/","").PadLeft(7,'0')},
            new SqlParameter("MZ_EXAD",SqlDbType.VarChar){Value = ViewState["AD"].ToString()},
            new SqlParameter("MZ_AD",SqlDbType.VarChar){Value = qMZ_AD},
            new SqlParameter("MZ_UNIT",SqlDbType.VarChar){Value = qMZ_UNIT}
            };

            //2010.06.07 by 伊珊
            string ErrorString = "";

            //sam WellSince Sam.Hsu 20200908 人事室建議同一日加班資料，加班補修與值日補修擇一選擇  
            string pkey_check = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM C_OVERTIME_HOUR_INSIDE WHERE MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "'AND MZ_DATE='" + TextBox_MZ_DATE.Text.Trim().Replace("/","").PadLeft(7,'0') + "' AND dbo.SUBSTR(RESTFLAG,1,1)='" + RadioButtonList_RESTFLAG.SelectedValue.Substring(0, 1) + "'");

            if (pkey_check != "0")
            {
                ErrorString += "輸入資料重複，無法新增" + "\\r\\n";
                TextBox_MZ_ID.BackColor = Color.Orange;
                TextBox_MZ_DATE.BackColor = Color.Orange;
            }
            else
            {
                TextBox_MZ_ID.BackColor = Color.Orange;
                TextBox_MZ_DATE.BackColor = Color.Orange;
            }
            //20141216 這應該是想做控制項檢核.但 DBDIC 又沒有資料
            //foreach (Object ob in Panel2.Controls)
            //{
            //    if (ob is TextBox)
            //    {
            //        TextBox tbox = (TextBox)ob;

            //        string result = o_Check_Dbdic.ErrorString(tbox.ID, "C_OVERTIME_HOUR_INSIDE", tbox.Text);

            //        if (!string.IsNullOrEmpty(result))
            //        {
            //            ErrorString += result + "\\r\\n";
            //            tbox.BackColor = Color.Orange;
            //        }
            //        else
            //        {
            //            tbox.BackColor = Color.White;
            //        }
            //    }
            //}
            //20141216 這應該是想做控制項檢核.但 DBDIC 又沒有資料
            if (!string.IsNullOrEmpty(ErrorString))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('" + ErrorString + "請更改不可重複欄位與輸入變色之欄位');", true);
                return;
            }

            try
            {
                o_DBFactory.ABC_toTest.ExecuteNonQuery( strSQL, parameterList);

                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('新增成功');", true);
                //2010.06.04 LOG紀錄 by伊珊
                TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00002", TPMPermissions._boolTPM(), o_DBFactory.ABC_toTest.RegexSQL(strSQL, parameterList));

                GV_DATABIND(TextBox_MZ_ID.Text);
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('新增失敗');", true);
            }
        }

        protected void btDelete_Click(object sender, EventArgs e)
        {

            string restflag = "";

            switch (GridView1.SelectedRow.Cells[6].Text)
            {
                case "":
                    restflag = "N";
                    break;
                case "補休":
                    restflag = "Y";
                    break;
                case "加班補休":
                    restflag = "YO";
                    break;
                case "超勤補休":
                    restflag = "YU";
                    break;
                case "值日補休":
                    restflag = "YD";
                    break;
            }

            string DeleteString = "DELETE FROM C_OVERTIME_HOUR_INSIDE WHERE MZ_DATE='" + GridView1.SelectedRow.Cells[1].Text.Trim().Replace("/", "").PadLeft(7, '0') + "' AND MZ_ID='" + o_str.tosql(TextBox_MZ_ID.Text.Trim()) + "' AND RESTFLAG='" + restflag + "'";

            try
            {
                o_DBFactory.ABC_toTest.Edit_Data(DeleteString);
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除成功');", true);
                //2010.06.04 LOG紀錄 by伊珊
                TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00004", TPMPermissions._boolTPM(), DeleteString);
                btDelete.Enabled = false;
                GV_DATABIND(DropDownList_MZ_NAME.SelectedValue);
                CountAll(o_str.tosql(TextBox_MZ_ID.Text.Trim()));
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('刪除失敗');", true);
            }
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                GridView1.SelectedIndex = Convert.ToInt32(e.CommandArgument);
                TextBox_MZ_DATE.Text = GridView1.SelectedRow.Cells[1].Text;
                TextBox_OTREASON.Text = GridView1.SelectedRow.Cells[2].Text == "&nbsp;" ? "" : GridView1.SelectedRow.Cells[2].Text;

                switch (GridView1.SelectedRow.Cells[6].Text)
                {
                    case "":
                        RadioButtonList_RESTFLAG.SelectedValue = "N";
                        break;
                    case "補休":
                        RadioButtonList_RESTFLAG.SelectedValue = "YO";
                        break;
                    case "加班補休":
                        RadioButtonList_RESTFLAG.SelectedValue = "YO";
                        break;
                    case "超勤補休":
                        RadioButtonList_RESTFLAG.SelectedValue = "YU";
                        break;
                    case "值日補休":
                        RadioButtonList_RESTFLAG.SelectedValue = "YD";
                        break;
                }

                HttpCookie Cookie1 = new HttpCookie("PKEY_MZ_ID");
                Cookie1.Value = TextBox_MZ_ID.Text.Trim();
                Response.Cookies.Add(Cookie1);

                if (GridView1.SelectedRow.Cells[8].Text == "Y")
                {
                    btUpdate.Enabled = false;
                    btDelete.Enabled = false;
                }
                else
                {
                    btUpdate.Enabled = true;
                    btDelete.Enabled = true;
                }

                (GridView1.SelectedRow.Cells[3].Controls[1] as TextBox).Focus();
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[6].Text == "Y")
                {
                    e.Row.Cells[6].Text = "補休";
                }
                else if (e.Row.Cells[6].Text == "YO")
                {
                    e.Row.Cells[6].Text = "加班補休";
                }
                else if (e.Row.Cells[6].Text == "YU")
                {
                    e.Row.Cells[6].Text = "超勤補休";
                    (e.Row.Cells[3].Controls[1] as TextBox).Enabled = false;
                }
                else if (e.Row.Cells[6].Text == "YD")
                {
                    e.Row.Cells[6].Text = "值日補休";
                }
                else
                {
                    e.Row.Cells[6].Text = "";
                }

                if (e.Row.Cells[8].Text.Substring(0, 1) == "Y")
                {
                    (e.Row.Cells[3].Controls[1] as TextBox).Enabled = false;

                }

                if ((e.Row.Cells[0].Controls[1] as CheckBox).Checked)
                {
                    if (ViewState["C_strGID"].ToString() == "D" || ViewState["C_strGID"].ToString() == "E")
                    {
                        (e.Row.Cells[3].Controls[1] as TextBox).Enabled = false;
                        (e.Row.Cells[6].Controls[1] as CheckBox).Enabled = false;
                    }
                }

                e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GridView1, "Select$" + e.Row.RowIndex);
            }

            if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[7].Attributes.Add("Style", "display:none");
                e.Row.Cells[8].Attributes.Add("Style", "display:none");

                if (ViewState["C_strGID"].ToString() == "D" || ViewState["C_strGID"].ToString() == "E")
                {
                    e.Row.Cells[0].Attributes.Add("Style", "display:none");
                }
            }
        }

        ///TODO : 注意 ! MZ_RESTHOUR 只有在這邊儲存，其餘地方無儲存
        protected void TextBoxOTIME_TextChanged(object sender, EventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((TextBox)sender).NamingContainer;
            GridView gv = (GridView)gvr.NamingContainer;
            int index = gvr.RowIndex;

            TextBox tbOTIME = gv.Rows[index].Cells[3].Controls[1] as TextBox;

            string restflag = "";

            switch (gv.Rows[index].Cells[6].Text)
            {
                case "":
                    restflag = "N";
                    break;
                case "補休":
                    restflag = "Y";
                    break;
                case "加班補休":
                    restflag = "YO";
                    break;
                case "超勤補休":
                    restflag = "YU";
                    break;
                case "值日補休":
                    restflag = "YD";
                    break;
            }


            if (!string.IsNullOrEmpty(tbOTIME.Text.Trim()))
            {
                string UpdateString = "UPDATE C_OVERTIME_HOUR_INSIDE SET OTIME=" + tbOTIME.Text.Trim() +
                                                                   ",MZ_RESTHOUR='" + tbOTIME.Text.Trim() + "',PAY_SUM=" + MathHelper.Round(float.Parse(tbOTIME.Text.Trim()) * float.Parse(gv.Rows[index].Cells[4].Text.Trim()), 0) +
                                      " WHERE MZ_ID='" + DropDownList_MZ_NAME.SelectedValue.Trim() +
                                      "'  AND MZ_DATE='" + gv.Rows[index].Cells[1].Text.Trim().Replace("/", string.Empty).PadLeft(7, '0') + "' AND RESTFLAG='" + restflag + "'";

                o_DBFactory.ABC_toTest.Edit_Data(UpdateString);
            }

            CountAll(o_str.tosql(TextBox_MZ_ID.Text));

            GV_DATABIND(DropDownList_MZ_NAME.SelectedValue);
        }

        

        protected void CountAll(string MZ_ID)
        {
            //string SelectString = " SELECT SUM(PAY_SUM) AS TOTAL_PAY_SUM,SUM(OTIME) AS TOTAL_OTIME " +
            //                      " FROM C_OVERTIME_HOUR_INSIDE " +
            //                      " WHERE (dbo.SUBSTR(RESTFLAG,1,1)<>'Y' OR RESTFLAG IS NULL) " +
            //                      " AND MZ_ID='" + MZ_ID + "'" +
            //                      " AND dbo.SUBSTR(MZ_DATE,1,5)='" + (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0') + DateTime.Now.Month.ToString().PadLeft(2, '0') + "' ";

            string SelectString = " SELECT PAY_SUM,OTIME,RESTFLAG " +
                                  " FROM C_OVERTIME_HOUR_INSIDE " +
                                  " WHERE " +
                                  " MZ_ID='" + MZ_ID + "'" +
                                  " AND dbo.SUBSTR(MZ_DATE,1,5)='" + (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0') + DateTime.Now.Month.ToString().PadLeft(2, '0') + "' ";

            DataTable temp = new DataTable();
            temp = o_DBFactory.ABC_toTest.Create_Table(SelectString, "GET");
            int s = 0;
            for (int i = 0; i < temp.Rows.Count; i++)
            {
                int OTIME = 0;
                if (int.TryParse(temp.Rows[i]["OTIME"].ToString(), out OTIME))
                    s += OTIME;
            }
            Label4.Text = s.ToString();

            DataRow[] dr = temp.Select("substring(RESTFLAG,1,1)<>'Y' OR RESTFLAG IS NULL");
            int TOTAL_PAY_SUM = 0;
            int TOTAL_OTIME = 0;
            for (int i = 0; i < dr.Length; i++)
            {
                int PAY_SUM = 0;
                int OTIME = 0;
                if (int.TryParse(dr[i]["PAY_SUM"].ToString(), out PAY_SUM))
                    TOTAL_PAY_SUM += PAY_SUM;
                if (int.TryParse(dr[i]["OTIME"].ToString(), out OTIME))
                    TOTAL_OTIME += OTIME;
            }
            //Label2.Text = string.IsNullOrEmpty(temp.Rows[0]["TOTAL_PAY_SUM"].ToString()) ? "0" : temp.Rows[0]["TOTAL_PAY_SUM"].ToString();
            //Label3.Text = string.IsNullOrEmpty(temp.Rows[0]["TOTAL_OTIME"].ToString()) ? "0" : temp.Rows[0]["TOTAL_OTIME"].ToString();
            Label2.Text = TOTAL_PAY_SUM.ToString();
            Label3.Text = TOTAL_OTIME.ToString();
            int x = 0;
            for (int i = 0; i < dr.Length; i++)
            {
                int OTIME = 0;
                if (int.TryParse(dr[i]["OTIME"].ToString(), out OTIME))
                    x += OTIME;
            }
            Label5.Text = x.ToString();
            //string s = o_DBFactory.ABC_toTest.vExecSQL(" SELECT SUM(OTIME) FROM C_OVERTIME_HOUR_INSIDE " +
            //                               " WHERE MZ_ID='" + MZ_ID + "'" +
            //                               " AND dbo.SUBSTR(MZ_DATE,1,5)='" + (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0') + DateTime.Now.Month.ToString().PadLeft(2, '0') + "' ");

            //string x = o_DBFactory.ABC_toTest.vExecSQL(" SELECT SUM(OTIME) FROM C_OVERTIME_HOUR_INSIDE " +
            //                               " WHERE dbo.SUBSTR(RESTFLAG,1,1)='Y' AND MZ_ID='" + MZ_ID + "'" +
            //                               " AND dbo.SUBSTR(MZ_DATE,1,5)='" + (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0') + DateTime.Now.Month.ToString().PadLeft(2, '0') + "' ");


            //Label4.Text = string.IsNullOrEmpty(s) ? "0" : s;

            //Label5.Text = string.IsNullOrEmpty(x) ? "0" : x;
        }
                      
        /// <summary>
        /// 按鈕 : 更新加班資料
        /// </summary>
        protected void btUpdate_Click(object sender, EventArgs e)
        {
            string tmpOverDay = ForDateTime.TWDateToRCDate(TextBox_MZ_DATE.Text);
            string strDate = "2021/08/01";
            DateTime date = Convert.ToDateTime(strDate);
            DateTime overday = Convert.ToDateTime(tmpOverDay);
            if (!(bool)ForDateTime.CheckDateTime(tmpOverDay))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('加班日期格式錯誤!');", true);
                return;
            }
            else
            {
                if (DateTime.Compare(overday, date) > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, GetType(), "click", "alert('舊加班系統禁止新增自110/8/1以後加班資料!');", true);
                    return;
                }
            }

            //2010.06.07 by 伊珊
            string ErrorString = "";

            string pkey_check;

            string restflag = "";

            switch (GridView1.SelectedRow.Cells[6].Text)
            {
                case "":
                    restflag = "N";
                    break;
                case "補休":
                    restflag = "Y";
                    break;
                case "加班補休":
                    restflag = "YO";
                    break;
                case "超勤補休":
                    restflag = "YU";
                    break;
                case "值日補休":
                    restflag = "YD";
                    break;
            }            

            if (restflag == RadioButtonList_RESTFLAG.SelectedValue && GridView1.SelectedRow.Cells[1].Text.Trim().Replace("/", string.Empty).PadLeft(7, '0') == TextBox_MZ_DATE.Text.Trim().Replace("/", string.Empty).PadLeft(7, '0'))
                pkey_check = "0";
            else
                pkey_check = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM C_OVERTIME_HOUR_INSIDE WHERE MZ_ID='" + TextBox_MZ_ID.Text.Trim() + "'AND MZ_DATE='" + TextBox_MZ_DATE.Text.Trim() + "'");

            if (pkey_check != "0")
            {
                ErrorString += "輸入資料重複，無法修改" + "\\r\\n";
                TextBox_MZ_ID.BackColor = Color.Orange;
                TextBox_MZ_DATE.BackColor = Color.Orange;
            }
            else
            {
                TextBox_MZ_ID.BackColor = Color.Orange;
                TextBox_MZ_DATE.BackColor = Color.Orange;
            }                     

            //20141216 這應該是想做控制項檢核.但 DBDIC 又沒有資料
            //foreach (Object ob in Panel2.Controls)
            //{
            //    if (ob is TextBox)
            //    {
            //        TextBox tbox = (TextBox)ob;

            //        string result = o_Check_Dbdic.ErrorString(tbox.ID, "C_OVERTIME_HOUR_INSIDE", tbox.Text);

            //        if (!string.IsNullOrEmpty(result))
            //        {
            //            ErrorString += result + "\\r\\n";
            //            tbox.BackColor = Color.Orange;
            //        }
            //        else
            //        {
            //            tbox.BackColor = Color.White;
            //        }
            //    }
            //}
            //20141216 這應該是想做控制項檢核.但 DBDIC 又沒有資料
            if (!string.IsNullOrEmpty(ErrorString))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('" + ErrorString + "請更改不可重複欄位與輸入變色之欄位');", true);
                return;
            }

            TextBox tbOTIME = GridView1.SelectedRow.Cells[3].Controls[1] as TextBox;

            string UpdateSQL = "UPDATE C_OVERTIME_HOUR_INSIDE SET  MZ_DATE='" + o_str.tosql(TextBox_MZ_DATE.Text.Trim().Replace("/", "").PadLeft(7, '0')) +
                                                               "',OTREASON='" + o_str.tosql(TextBox_OTREASON.Text.Trim()) +
                                                               "',PAY_SUM=" + MathHelper.Round(float.Parse(tbOTIME.Text) * float.Parse(TextBox_HOUR_PAY.Text.Trim()), 0) +
                                                               ",RESTFLAG='" + RadioButtonList_RESTFLAG.SelectedValue +
                                                               "' ,HOUR_PAY='" + TextBox_HOUR_PAY.Text.Trim() +
                                                               "' ,MZ_RESTHOUR = '" + tbOTIME.Text +
                                                               "' WHERE MZ_DATE='" + GridView1.SelectedRow.Cells[1].Text.Replace("/", "").PadLeft(7, '0') +
                                                               "' AND MZ_ID='" + o_str.tosql(TextBox_MZ_ID.Text.Trim()) + "'";
            try
            {
                o_DBFactory.ABC_toTest.Edit_Data(UpdateSQL);

                Response.Cookies["PKEY_MZ_ID"].Expires = DateTime.Now.AddYears(-1);
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('修改成功');", true);
                //2010.06.04 LOG紀錄 by伊珊
                TPMPermissions._strEventData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "EBXXX00003", TPMPermissions._boolTPM(), UpdateSQL);
                GV_DATABIND(TextBox_MZ_ID.Text.Trim());
                CountAll(TextBox_MZ_ID.Text);
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('修改失敗');", true);
            }

        }

        protected void btOvertimeTotal_Click(object sender, EventArgs e)
        {
            Response.Redirect("C_OvertimeInsideTotal_rpt.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] );
            
            //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "location.href('C_OvertimeInsideTotal_rpt.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "');", true);
        }

        protected void btOvertimeAsk_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(TextBox_MZ_ID.Text) && !string.IsNullOrEmpty(TextBox_MZ_DATE.Text))
            {
                
                Session["RPT_C_IDNO"] = TextBox_MZ_ID.Text;

                string tmp_url = "C_rpt.aspx?fn=OvertimeInsideAsk&TPM_FION=" + Request.QueryString["TPM_FION"]
                    + "&DATE=" + TextBox_MZ_DATE.Text.Trim().Replace("/", "").PadLeft(7, '0');
                    //+ "&TYPE=" + 1;

                ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "location.href('C_OvertimeInsideAsk_rpt.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "');", true);
            }
        }

        protected void btOvertimeDetail_Click(object sender, EventArgs e)
        {
            Response.Redirect("C_OvertimeInsideDetail_rpt.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] );
            
          
        }

        protected void TextBox_MZ_DATE_TextChanged(object sender, EventArgs e)
        {
            TextBox_MZ_DATE.Text = o_str.tosql(TextBox_MZ_DATE.Text.Trim().Replace("/", ""));

            if (TextBox_MZ_DATE.Text != "")
            {
                if (!DateManange.Check_date(TextBox_MZ_DATE.Text))
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('日期格式錯誤')", true);
                    TextBox_MZ_DATE.Text = string.Empty;
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_MZ_DATE.ClientID + "').focus();$get('" + TextBox_MZ_DATE.ClientID + "').focus();", true);
                }
                else
                {
                    TextBox_MZ_DATE.Text = o_CommonService.Personal_ReturnDateString(TextBox_MZ_DATE.Text.Trim());
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + TextBox_OTREASON.ClientID + "').focus();$get('" + TextBox_OTREASON.ClientID + "').focus();", true);
                }
            }
        }

        protected void btSearch_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('C_ForLeaveOvertime_Inside_Overtime_Search.aspx?TPM_FION=" + Request.QueryString["TPM_FION"] + "','查詢','top=190,left=200,width=420,height=250,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataTable gd_dt = Session["Inside_Overtime_gd_dt"] as DataTable;

            GridView1.PageIndex = e.NewPageIndex;

            GridView1.DataSource = gd_dt;

            GridView1.AllowPaging = true;

            GridView1.PageSize = 8;

            GridView1.DataBind();
        }

        protected void CheckBox_MZ_CHK_CheckedChanged(object sender, EventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((CheckBox)sender).NamingContainer;
            GridView gv = (GridView)gvr.NamingContainer;
            int index = gvr.RowIndex;

            gv.SelectedIndex = index;
            btUpdate.Enabled = true;

            CheckBox CHK = gv.Rows[index].Cells[0].Controls[1] as CheckBox;

            if (CHK.Checked)
            {
                string UpdateSQL = "UPDATE C_OVERTIME_HOUR_INSIDE SET MZ_CHK='Y' WHERE MZ_ID='" + DropDownList_MZ_NAME.SelectedValue.Trim() + "' AND MZ_DATE='" + gv.Rows[index].Cells[1].Text.Trim().Replace("/", "").PadLeft(7, '0') + "'";

                o_DBFactory.ABC_toTest.Edit_Data(UpdateSQL);
            }
            else
            {
                string UpdateSQL = "UPDATE C_OVERTIME_HOUR_INSIDE SET MZ_CHK='N' WHERE MZ_ID='" + DropDownList_MZ_NAME.SelectedValue.Trim() + "' AND MZ_DATE='" + gv.Rows[index].Cells[1].Text.Trim().Replace("/", "").PadLeft(7, '0') + "'";

                o_DBFactory.ABC_toTest.Edit_Data(UpdateSQL);
            }

        }
    }
}
