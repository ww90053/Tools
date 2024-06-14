using Aspose.Cells;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TPPDDB._15_score
{
    public partial class C_CARDHISTORY_SURE : o_score_Function
    {
        string strSQL;
        DataTable temp = new DataTable();
        string TPM_FION;           // 模組     
        protected string _strGID
        {

            get { return ViewState["C_strGID"] != null ? ViewState["C_strGID"].ToString() : string.Empty; }
            set { ViewState["C_strGID"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //權限檢查
                _strGID = o_a_Function.strGID(Session["ADPMZ_ID"].ToString());
                group_control(_strGID);
                //GetData();
            }
        }
        private void group_control(string _strGID)
        {
            switch (_strGID)
            {
                //A全局 B全局 C限自己分局 D,E 按下查詢時才看是否為審核者
                case "A":
                    break;
                case "B":
                    break;
                case "C":
                    DropDownList_AD.DataBind();
                    DropDownList_AD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                    DropDownList_AD.Enabled = false;
                    Select_UNIT();
                    break;
                case "D":
                    DropDownList_AD.DataBind();
                    DropDownList_AD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                    DropDownList_AD.Enabled = false;
                    Select_UNIT();
                    break;
                case "E":
                    DropDownList_AD.DataBind();
                    DropDownList_AD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                    DropDownList_AD.Enabled = false;
                    Select_UNIT();
                    break;
            }
        }

        protected void DropDownList_AD_SelectedIndexChanged(object sender, EventArgs e)
        {
            Select_UNIT();
            Select_Name();
        }
        protected void DropDownList_UNIT_SelectedIndexChanged(object sender, EventArgs e)
        {
            Select_Name();
        }
        protected void Select_UNIT()
        {
            string ad = DropDownList_AD.SelectedValue;
            string unit = DropDownList_UNIT.SelectedValue;
            DataTable dt = new DataTable();
            strSQL = "SELECT MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE='25' AND MZ_KCODE IN (SELECT MZ_UNIT FROM A_UNIT_AD WHERE MZ_AD='" + ad + "')";
            dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
            DropDownList_UNIT.DataSource = dt;
            DropDownList_UNIT.DataTextField = "MZ_KCHI";
            DropDownList_UNIT.DataValueField = "MZ_KCODE";
            DropDownList_UNIT.DataBind();
        }
        protected void Select_Name()
        {
            string ad = DropDownList_AD.SelectedValue;
            string unit = DropDownList_UNIT.SelectedValue;
            string whereStr = "";
            if (DropDownList_AD.SelectedIndex == 0)
            {
                whereStr = " where MZ_STATUS2='Y'";
            }
            else if (DropDownList_UNIT.SelectedIndex == 0)
            {
                whereStr = " where MZ_EXAD='" + DropDownList_AD.SelectedValue + "' AND MZ_STATUS2='Y'";
            }
            else
            {
                whereStr = " where MZ_EXAD='" + DropDownList_AD.SelectedValue + "' AND MZ_EXUNIT='" + DropDownList_UNIT.SelectedValue + "' AND MZ_STATUS2='Y'";
            }
            DataTable dt = new DataTable();
            strSQL = "SELECT MZ_NAME,MZ_ID FROM A_DLBASE " + whereStr + " ORDER BY MZ_EXAD, MZ_EXUNIT, MZ_TBDV";
            dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");

            DropDownList_NAME.Items.Clear();
            DropDownList_NAME.DataSource = dt;
            DropDownList_NAME.DataTextField = "MZ_NAME";
            DropDownList_NAME.DataValueField = "MZ_ID";
            DropDownList_NAME.DataBind();
        }
        protected void DropDownList_AD_DataBound(object sender, EventArgs e)
        {
            DropDownList_AD.Items.Insert(0, new ListItem("全部", "all"));
        }
        protected void DropDownList_UNIT_DataBound(object sender, EventArgs e)
        {
            DropDownList_UNIT.Items.Insert(0, new ListItem("全部", "all"));
        }
        protected void DropDownList_NAME_DataBound(object sender, EventArgs e)
        {
            DropDownList_NAME.Items.Insert(0, new ListItem("全部", "all"));
        }
        protected void GetData()
        {
            //string strSQL = "SELECT * FROM TP_MODEL_MEMBER where tpmn_gid in ('1901','1902','1909') and TPMID='" + Session["TPM_MID"] + "'";
            //DataTable dttemp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
            //dttemp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "REPORT");

            string SQL = "";
            string where = "";
            if (ddl_STATUS.SelectedValue != "")
            {
                where += " and (STATUS=" + ddl_STATUS.SelectedValue + ")";
            }
            //改成分開查詢
            //if (ddl_STATUS.SelectedValue == "2")
            //{
            //    where += " and (STATUS=2 or STATUS=3)";
            //}
            //else if (ddl_STATUS.SelectedValue != "")
            //{
            //    where += " and (STATUS=" + ddl_STATUS.SelectedValue + ")";
            //}
            if (DropDownList_AD.SelectedValue != "all" && DropDownList_AD.SelectedValue != "")
                where += " and  MZ_AD='" + DropDownList_AD.SelectedValue + "'";
            if (DropDownList_UNIT.SelectedValue != "all" && DropDownList_UNIT.SelectedValue != "")
                where += " and MZ_UNIT='" + DropDownList_UNIT.SelectedValue + "'";
            if (DropDownList_NAME.SelectedValue != "all" && DropDownList_NAME.SelectedValue != "")
                where += " and CHECKER='" + DropDownList_NAME.SelectedValue + "'";
            if (txt_SDATETIME.Text != "" || txt_EDATETIME.Text != "")
                where += " and DATETIME BETWEEN '" + txt_SDATETIME.Text + "'" + "AND" + "'" + txt_EDATETIME.Text + "'";
            //if (txt_SDATETIME.Text!="")
            //    where += " and DATETIME>=To_Date ('" + txt_SDATETIME.Text + "', 'YYYY-MM-DD')";

            //if (txt_EDATETIME.Text != "")
            //    where += " and DATETIME<=To_Date ('" + txt_EDATETIME.Text + "', 'YYYY-MM-DD')";
            if (where != "")
            {
                where = " where " + where.Substring(5);
            }
            SQL = "SELECT (SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE = '04' AND MZ_KCODE = MZ_AD) + ' ' + (SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE = '25' AND MZ_KCODE =MZ_UNIT ) + ' ' + MZ_OCCC + ' ' + MZ_NAME as MZ_NAME,SN,DATETIME,INTIME,OUTTIME,TYPE,STATUS,ADD_USER,CHECKER,MOD_USER,(SELECT MZ_NAME FROM A_DLBASE WHERE MZ_ID = CHECKER) as CHECKNAME,BOUNCED_REASON FROM C_CARDHISTORY_EDIT " + where + " order by DATETIME desc";
            //if (dttemp.Rows.Count > 0)
            //{
            //    SQL = "SELECT (SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE = '04' AND MZ_KCODE = MZ_AD) + ' ' + (SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE = '25' AND MZ_KCODE =MZ_UNIT ) + ' ' + MZ_OCCC + ' ' + MZ_NAME as MZ_NAME,SN,DATETIME,INTIME,OUTTIME,TYPE,STATUS,ADD_USER,CHECKER FROM C_CARDHISTORY_EDIT WHERE order by DATETIME desc";
            //}
            //else
            //{
            //    SQL = "SELECT (SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE = '04' AND MZ_KCODE = MZ_AD) + ' ' + (SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE = '25' AND MZ_KCODE =MZ_UNIT ) + ' ' + MZ_OCCC + ' ' + MZ_NAME as MZ_NAME,SN,DATETIME,INTIME,OUTTIME,TYPE,STATUS,ADD_USER,CHECKER FROM C_CARDHISTORY_EDIT WHERE CHECKER='" + Session["ADPMZ_ID"].ToString() + "' order by DATETIME desc";
            //}
            DataTable dt = o_DBFactory.ABC_toTest.Create_Table(SQL, "GET");

            if (dt == null || dt.Rows.Count == 0)
            {
                ScriptManager.RegisterClientScriptBlock(Panel2, this.GetType(), "click", "alert('查無資料');", true);
            }

            gv_C_CARDHISTORY_EDIT.DataSource = dt;
            gv_C_CARDHISTORY_EDIT.DataBind();
            Session.Remove("ReportDataTable");
            Session.Add("ReportDataTable", dt);
        }

        protected void gv_C_CARDHISTORY_EDIT_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            TPM_FION = get_TPM_FION();
            String ErrMsg = String.Empty;
            String SN = e.CommandArgument.ToString();
            string updateString = "";
            switch (e.CommandName)
            {               
                //收件
                case "doSure":
                    //收件為什麼要更新審查者?? 先拿掉改成只更新收件人
                    string Sure_USER = Session["ADPMZ_ID"].ToString();
                    DateTime Sure_DATE = DateTime.Now;
                    //updateString = "UPDATE C_CARDHISTORY_EDIT SET CHECKER='" + ddl_CHECKER.SelectedValue + "',STATUS='4' ,Sure_USER='" + Sure_USER + "' ,Sure_DATE=dbo.TO_DATE('" + Sure_DATE.ToString("yyyy/MM/dd HH:mm:ss") + "', 'YYYY-MM-DD hh24:mi:ss')" + " WHERE SN='" + SN + "'";
                    updateString = "UPDATE C_CARDHISTORY_EDIT SET STATUS='4' ,Sure_USER='" + Sure_USER + "' ,Sure_DATE=dbo.TO_DATE('" + Sure_DATE.ToString("yyyy/MM/dd HH:mm:ss") + "', 'YYYY-MM-DD hh24:mi:ss')" + " WHERE SN='" + SN + "'";
                    o_DBFactory.ABC_toTest.Edit_Data(updateString);
                    //這一段可能是寫配合作業刷卡明細表的功能看不懂在幹嘛 
                    //string SQLstr = string.Format(@"SELECT * FROM C_CARDHISTORY_EDIT WHERE SN='{0}'", Request.QueryString["SN"].ToString());
                    string SQLstr = string.Format(@"SELECT * FROM C_CARDHISTORY_EDIT WHERE SN='{0}'", SN);

                    DataTable dt = new DataTable();

                    dt = o_DBFactory.ABC_toTest.Create_Table(SQLstr, "get");

                    if (dt.Rows.Count > 0)
                    {
                        string MZ_ID = dt.Rows[0]["MZ_ID"].ToString();
                        string MZ_NAME = dt.Rows[0]["MZ_NAME"].ToString();
                        MZ_NAME = MZ_ID + " " + MZ_NAME;
                        MZ_ID = "00000" + MZ_ID.Substring(5, 5);
                        string DATETIME = dt.Rows[0]["DATETIME"].ToString();
                        string INTIME = dt.Rows[0]["INTIME"].ToString();
                        string OUTTIME = dt.Rows[0]["OUTTIME"].ToString();
                        if (INTIME != "")
                        {
                            SqlParameter[] parameterList = {
                                        new SqlParameter("TID", SqlDbType.Float){Value = 99},
                                        new SqlParameter("TERMINALNAME", SqlDbType.NVarChar){Value = "補登"},
                                        new SqlParameter("USERID", SqlDbType.NVarChar){Value = MZ_ID},
                                        new SqlParameter("USERNAME", SqlDbType.NVarChar){Value = MZ_NAME},
                                        new SqlParameter("LOGDATE", SqlDbType.NVarChar){Value = DATETIME},
                                        new SqlParameter("LOGTIME", SqlDbType.NVarChar){Value = INTIME},
                                        new SqlParameter("VERIFY", SqlDbType.NVarChar){Value = "IN"},
                                        new SqlParameter("FKEY", SqlDbType.NVarChar){Value = "NONE"},
                                        new SqlParameter("DOOR", SqlDbType.NVarChar){Value = "主門"},
                                        };


                            strSQL = @"INSERT INTO C_CARDHISTORY_NEW (TID,TERMINALNAME,USERID,USERNAME,LOGDATE,LOGTIME,VERIFY,FKEY,DOOR)
                                        VALUES (@TID,@TERMINALNAME,@USERID,@USERNAME,@LOGDATE,@LOGTIME,@VERIFY,@FKEY,@DOOR) ";

                            o_DBFactory.ABC_toTest.ExecuteNonQuery( strSQL, parameterList);
                        }
                        if (OUTTIME != "")
                        {
                            SqlParameter[] parameterList1 = {
                                        new SqlParameter("TID", SqlDbType.Float){Value = 99},
                                        new SqlParameter("TERMINALNAME", SqlDbType.NVarChar){Value = "補登"},
                                        new SqlParameter("USERID", SqlDbType.NVarChar){Value = MZ_ID},
                                        new SqlParameter("USERNAME", SqlDbType.NVarChar){Value = MZ_NAME},
                                        new SqlParameter("LOGDATE", SqlDbType.NVarChar){Value = DATETIME},
                                        new SqlParameter("LOGTIME", SqlDbType.NVarChar){Value = OUTTIME},
                                        new SqlParameter("VERIFY", SqlDbType.NVarChar){Value = "IN"},
                                        new SqlParameter("FKEY", SqlDbType.NVarChar){Value = "NONE"},
                                        new SqlParameter("DOOR", SqlDbType.NVarChar){Value = "主門"},
                                        };


                            strSQL = @"INSERT INTO C_CARDHISTORY_NEW (TID,TERMINALNAME,USERID,USERNAME,LOGDATE,LOGTIME,VERIFY,FKEY,DOOR)
                                        VALUES (@TID,@TERMINALNAME,@USERID,@USERNAME,@LOGDATE,@LOGTIME,@VERIFY,@FKEY,@DOOR) ";

                            o_DBFactory.ABC_toTest.ExecuteNonQuery( strSQL, parameterList1);
                        }
                        GetData();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "click", "alert('收件成功')", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "click", "alert('無收件資料')", true);
                    }
                    break;
            }
        }

        protected void gv_C_CARDHISTORY_EDIT_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;
                Label lbl_DATETIME = (Label)e.Row.FindControl("lbl_DATETIME");
                //string DATETIME = DateTime.Parse(drv["DATETIME"].ToString()).ToString("yyyy/MM/dd");
                string DATETIME = drv["DATETIME"].ToString();
                string INTIME = drv["INTIME"].ToString();
                string OUTTIME = drv["OUTTIME"].ToString();
                lbl_DATETIME.Text = DATETIME + " " + INTIME + "~" + OUTTIME;

                string STATUS = drv["STATUS"].ToString();
                Label lbl_STATUS = (Label)e.Row.FindControl("lbl_STATUS");
                if (STATUS == "0")
                    lbl_STATUS.Text = "退回";
                else if (STATUS == "1")
                    lbl_STATUS.Text = "新增";
                else if (STATUS == "2")
                    lbl_STATUS.Text = "陳核";
                else if (STATUS == "3")
                    lbl_STATUS.Text = "決行";
                else if (STATUS == "4")
                    lbl_STATUS.Text = "收件";
                string MOD_USER_NAME = drv["MOD_USER"].ToString();
                MOD_USER_NAME = o_A_DLBASE.CNAME(drv["MOD_USER"].ToString());
                Label lbl_MOD_USER = (Label)e.Row.FindControl("lbl_MOD_USER");
                lbl_MOD_USER.Text = MOD_USER_NAME;

                Label lb2_STATUS = (Label)e.Row.FindControl("lb2_STATUS");
                if (STATUS == "5")
                    lb2_STATUS.Text = "紙本";
                else
                    lb2_STATUS.Text = "線上";
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            ////只有人事室或A，B才有權限可以收件
            //if (_strGID != "A" || _strGID != "B")
            //{
            //    string strSQL = "SELECT * FROM TP_MODEL_MEMBER where tpmn_gid in ('1901','1902','1909') and TPMID='" + Session["TPM_MID"] + "'";
            //    DataTable dttemp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
            //    dttemp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "REPORT");
            //    if (!(dttemp.Rows.Count > 0))
            //    {
            //        Response.Redirect("~/ErrorPage.aspx?erid=TPFXXX0001");
            //    }
            //}                              
            GetData();
        }

        protected void Btn_Excel_Click(object sender, EventArgs e)
        {
            //RadGrid1.DataSource = tmp;
            string path = Server.MapPath(@"report\C_CARDHISTORY_CHECK.xlsx");
            string name;
            if (File.Exists(path))
            {
                name = Server.MapPath(DateTime.Now.Ticks.ToString());
                File.Copy(path, name);
                Workbook work = GetExcel(name);
                Worksheet sheet = work.Worksheets[0];
                Cells cells = sheet.Cells;

                if (Session["ReportDataTable"] != null)
                {
                    DataTable LA_ReportData = (DataTable)Session["ReportDataTable"];
                    for (int i = 0; i < LA_ReportData.Rows.Count; i++)
                    {
                        DataRow dr = LA_ReportData.Rows[i];
                        cells[i + 1, 0].PutValue(dr["MZ_NAME"].ToString());
                        cells[i + 1, 1].PutValue(dr["DATETIME"].ToString().Replace("上午 12:00:00", "") + " " + dr["INTIME"].ToString() + "~" + dr["OUTTIME"].ToString());
                        string STATUS = "";
                        switch (dr["STATUS"].ToString())
                        {
                            case "0":
                                STATUS = "退回";
                                break;
                            case "1":
                                STATUS = "新增";
                                break;
                            case "2":
                                STATUS = "陳核";
                                break;
                            case "3":
                                STATUS = "決行";
                                break;
                            case "4":
                                STATUS = "收件";
                                break;
                            default:
                                STATUS = " ";
                                break;

                        }
                        cells[i + 1, 2].PutValue(STATUS);
                        string MOD_USER_NAME = o_A_DLBASE.CNAME(dr["MOD_USER"].ToString());
                        cells[i + 1, 3].PutValue(MOD_USER_NAME);
                        cells[i + 1, 4].PutValue(dr["BOUNCED_REASON"].ToString());
                    }
                    Response.AddHeader("Content-Disposition", "attachment;filename=" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
                    work.Save(HttpContext.Current.Response.OutputStream, new XlsSaveOptions(SaveFormat.Excel97To2003));

                    try
                    {
                        File.Delete(name);
                    }
                    catch { }
                    HttpContext.Current.Response.End();
                }
                else
                {
                    Response.Write("path:" + path);
                }
            }
        }

        public Workbook GetExcel(string FullFileName)
        {
            AsposeLicense();
            Workbook work = new Workbook(FullFileName);

            return work;

        }
        public void AsposeLicense()
        {
            Aspose.Cells.License license = new Aspose.Cells.License();
            license.SetLicense("Aspose.Cells.lic");
        }
    }
}