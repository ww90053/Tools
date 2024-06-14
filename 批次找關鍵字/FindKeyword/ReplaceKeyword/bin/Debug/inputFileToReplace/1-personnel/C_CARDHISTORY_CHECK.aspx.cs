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
    public partial class C_CARDHISTORY_CHECK : o_score_Function
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
                //收件改成另開功能
                //string strSQL = "SELECT * FROM TP_MODEL_MEMBER where tpmn_gid in ('1901','1902','1909') and TPMID='" + Session["TPM_MID"] + "'";
                //DataTable dttemp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
                //dttemp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "REPORT");
                //if (!(dttemp.Rows.Count > 0))
                //{
                //    gv_C_CARDHISTORY_EDIT.Columns[8].Visible = false;
                //}
                //else
                //{
                //    gv_C_CARDHISTORY_EDIT.Columns[9].Visible = false;
                //    gv_C_CARDHISTORY_EDIT.Columns[7].Visible = false;
                //    gv_C_CARDHISTORY_EDIT.Columns[6].Visible = false;
                //    gv_C_CARDHISTORY_EDIT.Columns[5].Visible = false;
                //    gv_C_CARDHISTORY_EDIT.Columns[4].Visible = false;
                //}
                //GetData();

                //20210820
                //由首頁  因公疏未辦理按卡尚未審核   進入
                string type = Request.QueryString["type"];

                if (type == "1")
                {
                    var qsn = Request.QueryString["SN"];

                    string strSQL = @"Select * From C_CARDHISTORY_EDIT Where SN=@SN ";
                    List<SqlParameter> parameters = new List<SqlParameter>()
                    {
                        new SqlParameter("SN", qsn)
                    };

                    DataTable dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, parameters);

                    if (dt.Rows != null && dt.Rows.Count > 0)
                    {
                        txt_SDATETIME.Text = dt.Rows[0]["DATETIME"].ToString();//申請日期起
                        txt_EDATETIME.Text = dt.Rows[0]["DATETIME"].ToString();//申請日期迄

                        //審查者
                        DropDownList_NAME.SelectedValue = dt.Rows[0]["CHECKER"].ToString();
                        bool selectedNAME = DropDownList_NAME.SelectedValue == dt.Rows[0]["CHECKER"].ToString();
                        if(!selectedNAME)
                        {
                            DropDownList_NAME.SelectedValue = "all";
                        }

                        //服務機關
                        bool selectedAD = DropDownList_AD.SelectedValue == dt.Rows[0]["MZ_AD"].ToString();
                        if (!selectedAD)
                        {
                            DropDownList_AD.SelectedValue = dt.Rows[0]["MZ_AD"].ToString();
                        }

                        //現服單位
                        bool selectedUNIT = DropDownList_UNIT.SelectedValue == dt.Rows[0]["MZ_UNIT"].ToString();
                        if (!selectedUNIT)
                        {
                            DropDownList_UNIT.SelectedValue = dt.Rows[0]["MZ_UNIT"].ToString();
                        }

                        Button1_Click(sender, e);

                    }
                }
            }
        }
        private void group_control(string strGID)
        {
            switch (strGID)
            {
                case "A":
                    break;
                case "B":
                    //DropDownList_MZ_AD.DataBind();
                    //DropDownList_MZ_AD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                    //DropDownList_MZ_AD.Enabled = false;
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
                    DropDownList_UNIT.SelectedValue = Session["ADPMZ_EXUNIT"].ToString();
                    DropDownList_UNIT.Enabled = false;
                    Select_Name();
                    DropDownList_NAME.SelectedValue = Session["ADPMZ_ID"].ToString();
                    break;
                case "E":
                    DropDownList_AD.DataBind();
                    DropDownList_AD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                    DropDownList_AD.Enabled = false;
                    Select_UNIT();
                    DropDownList_UNIT.SelectedValue = Session["ADPMZ_EXUNIT"].ToString();
                    DropDownList_UNIT.Enabled = false;
                    Select_Name();
                    DropDownList_NAME.SelectedValue = Session["ADPMZ_ID"].ToString();
                    //DropDownList_NAME.Enabled = false;
                    break;
                default:
                    DropDownList_AD.DataBind();
                    DropDownList_AD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                    DropDownList_AD.Enabled = false;
                    Select_UNIT();
                    DropDownList_UNIT.SelectedValue = Session["ADPMZ_EXUNIT"].ToString();
                    DropDownList_UNIT.Enabled = false;
                    Select_Name();
                    DropDownList_NAME.SelectedValue = Session["ADPMZ_ID"].ToString();
                    DropDownList_NAME.Enabled = false;
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
                where = " where " + where.Substring(5) + "and STATUS != '5'";
                SQL = "SELECT (SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE = '04' AND MZ_KCODE = MZ_AD) + ' ' + (SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE = '25' AND MZ_KCODE =MZ_UNIT ) + ' ' + MZ_OCCC + ' ' + MZ_NAME as MZ_NAME,SN,DATETIME,INTIME,OUTTIME,TYPE,STATUS,ADD_USER,CHECKER,MOD_USER,(SELECT MZ_NAME FROM A_DLBASE WHERE MZ_ID = CHECKER) as CHECKNAME,BOUNCED_REASON FROM C_CARDHISTORY_EDIT " + where + " order by DATETIME desc";
            }
            else
            {
                SQL = "SELECT (SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE = '04' AND MZ_KCODE = MZ_AD) + ' ' + (SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE = '25' AND MZ_KCODE =MZ_UNIT ) + ' ' + MZ_OCCC + ' ' + MZ_NAME as MZ_NAME,SN,DATETIME,INTIME,OUTTIME,TYPE,STATUS,ADD_USER,CHECKER,MOD_USER,(SELECT MZ_NAME FROM A_DLBASE WHERE MZ_ID = CHECKER) as CHECKNAME,BOUNCED_REASON FROM C_CARDHISTORY_EDIT where STATUS != '5' order by DATETIME desc";
            }
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
            Btn_Excel.Visible = true;
        }

        protected void gv_C_CARDHISTORY_EDIT_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            TPM_FION = get_TPM_FION();
            String ErrMsg = String.Empty;
            String SN = e.CommandArgument.ToString();
            string MOD_USER = "";
            DateTime MOD_DATE;
            GridViewRow row = ((Button)e.CommandSource).Parent.Parent as GridViewRow;
            DropDownList ddl_CHECKER = (DropDownList)row.FindControl("ddl_CHECKER");
            string updateString = "";
            switch (e.CommandName)
            {
                //陳核
                case "doCheck":
                    MOD_USER = Session["ADPMZ_ID"].ToString();
                    MOD_DATE = DateTime.Now;
                    updateString = "UPDATE C_CARDHISTORY_EDIT SET CHECKER='" + ddl_CHECKER.SelectedValue + "',STATUS='2' ,MOD_USER='" + MOD_USER + "' ,MOD_DATE=dbo.TO_DATE('" + MOD_DATE.ToString("yyyy/MM/dd HH:mm:ss") + "', 'YYYY-MM-DD hh24:mi:ss')" + " WHERE SN='" + SN + "'";
                    o_DBFactory.ABC_toTest.Edit_Data(updateString);
                    GetData();
                    break;
                //決行
                case "doDecide":
                    MOD_USER = Session["ADPMZ_ID"].ToString();
                    MOD_DATE = DateTime.Now;
                    updateString = "UPDATE C_CARDHISTORY_EDIT SET CHECKER='" + ddl_CHECKER.SelectedValue + "',STATUS='3' ,MOD_USER='" + MOD_USER + "' ,MOD_DATE=dbo.TO_DATE('" + MOD_DATE.ToString("yyyy/MM/dd HH:mm:ss") + "', 'YYYY-MM-DD hh24:mi:ss')" + " WHERE SN='" + SN + "'";
                    o_DBFactory.ABC_toTest.Edit_Data(updateString);
                    GetData();
                    break;
                //退回
                case "doBounced":
                    MOD_USER = Session["ADPMZ_ID"].ToString();
                    MOD_DATE = DateTime.Now;
                    TextBox txt_BOUNCED_REASON = (TextBox)row.FindControl("txt_BOUNCED_REASON");
                    if (txt_BOUNCED_REASON.Text.Trim() == "")
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "alert('退回原因不能空白！');", true);
                        break;
                    }
                    updateString = "UPDATE C_CARDHISTORY_EDIT SET CHECKER='" + ddl_CHECKER.SelectedValue + "',STATUS='0' ,MOD_USER='" + MOD_USER + "' ,MOD_DATE=dbo.TO_DATE('" + MOD_DATE.ToString("yyyy/MM/dd HH:mm:ss") + "', 'YYYY-MM-DD hh24:mi:ss'),BOUNCED_REASON='" + txt_BOUNCED_REASON.Text + "'" + " WHERE SN='" + SN + "'";
                    o_DBFactory.ABC_toTest.Edit_Data(updateString);
                    GetData();
                    break;
            }
        }

        protected void gv_C_CARDHISTORY_EDIT_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;
                TextBox txt_BOUNCED_REASON = (TextBox)e.Row.FindControl("txt_BOUNCED_REASON");
                txt_BOUNCED_REASON.Text = drv["BOUNCED_REASON"].ToString();
                Label lbl_DATETIME = (Label)e.Row.FindControl("lbl_DATETIME");
                //string DATETIME = DateTime.Parse(drv["DATETIME"].ToString()).ToString("yyyy/MM/dd");
                string DATETIME = drv["DATETIME"].ToString();
                string INTIME = drv["INTIME"].ToString();
                string OUTTIME = drv["OUTTIME"].ToString();
                lbl_DATETIME.Text = DATETIME + " " + INTIME + "~" + OUTTIME;

                //根據目前狀態隱藏功能
                Button btnCheck = (Button)e.Row.FindControl("btnCheck");
                Button btnDecide = (Button)e.Row.FindControl("btnDecide");
                Button btnBounced = (Button)e.Row.FindControl("btnBounced");
                DropDownList ddl_CHECKER = (DropDownList)e.Row.FindControl("ddl_CHECKER");

                string STATUS = drv["STATUS"].ToString();
                Label lbl_STATUS = (Label)e.Row.FindControl("lbl_STATUS");
                if (STATUS == "0")
                    lbl_STATUS.Text = "退回";
                else if (STATUS == "1")
                    lbl_STATUS.Text = "新增";
                else if (STATUS == "2")
                    lbl_STATUS.Text = "陳核";
                else if (STATUS == "3")
                {
                    lbl_STATUS.Text = "決行";
                    btnCheck.Visible = false;
                    btnDecide.Visible = false;
                    ddl_CHECKER.Visible = false;
                }
                else if (STATUS == "4")
                {
                    lbl_STATUS.Text = "收件";
                    btnCheck.Visible = false;
                    btnDecide.Visible = false;
                    btnBounced.Visible = false;
                    txt_BOUNCED_REASON.Visible = false;
                    ddl_CHECKER.Visible = false;
                }

                //增加人事權限E可瀏覽，關閉陳核相關功能 20190709 by sky
                if (_strGID == "E")
                {
                    ddl_CHECKER.Visible = false;
                    btnCheck.Visible = false;
                    btnDecide.Visible = false;
                    btnBounced.Visible = false;
                    txt_BOUNCED_REASON.Visible = false;
                }

                string MOD_USER_NAME = drv["MOD_USER"].ToString();
                MOD_USER_NAME = o_A_DLBASE.CNAME(drv["MOD_USER"].ToString());
                Label lbl_MOD_USER = (Label)e.Row.FindControl("lbl_MOD_USER");
                lbl_MOD_USER.Text = MOD_USER_NAME;
                DataTable temp = new DataTable();
                temp = o_DBFactory.ABC_toTest.Create_Table("SELECT MZ_EXAD,MZ_EXUNIT FROM A_DLBASE WHERE MZ_ID='" + drv["ADD_USER"].ToString() + "'", "get");
                if (temp.Rows.Count > 0)
                {
                    DataTable temp1 = new DataTable();
                    temp1 = o_DBFactory.ABC_toTest.Create_Table("SELECT (a_dlbase.mz_name + ' ' + (SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='26' AND MZ_KCODE=C_REVIEW_MANAGEMENT.MZ_OCCC)) AS MZ_NAME,c_review_management.MZ_ID FROM c_review_management,a_dlbase WHERE A_DLBASE.MZ_ID = C_REVIEW_MANAGEMENT.MZ_ID AND C_REVIEW_MANAGEMENT.MZ_EXAD = '" + temp.Rows[0]["MZ_EXAD"].ToString() + "' AND C_REVIEW_MANAGEMENT.MZ_EXUNIT = '" + temp.Rows[0]["MZ_EXUNIT"].ToString() + "' AND C_REVIEW_MANAGEMENT.REVIEW_LEVEL = '2'", "get");                    //temp1 = o_DBFactory.ABC_toTest.Create_Table("SELECT (a_dlbase.mz_name + ' ' + (SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='26' AND MZ_KCODE=C_REVIEW_MANAGEMENT.MZ_OCCC)) AS MZ_NAME,c_review_management.MZ_ID FROM c_review_management,a_dlbase WHERE A_DLBASE.MZ_ID = C_REVIEW_MANAGEMENT.MZ_ID AND C_REVIEW_MANAGEMENT.REVIEW_LEVEL = '2'", "get");
                    ddl_CHECKER.Items.Clear();
                    ddl_CHECKER.DataSource = temp1;
                    ddl_CHECKER.DataValueField = "MZ_ID";        //在此輸入的是資料表的欄位名稱
                    ddl_CHECKER.DataTextField = "MZ_NAME";      //在此輸入的是資料表的欄位名稱
                    ddl_CHECKER.DataBind();
                }
                ddl_CHECKER.SelectedValue = drv["CHECKER"].ToString();
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            //看帳號D,E是否為審核者
            if (_strGID == "D" || _strGID == "E")
            {
                DataTable temp1 = new DataTable();
                temp1 = o_DBFactory.ABC_toTest.Create_Table("SELECT (a_dlbase.mz_name + ' ' + (SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='26' AND MZ_KCODE=C_REVIEW_MANAGEMENT.MZ_OCCC)) AS MZ_NAME,c_review_management.MZ_ID FROM c_review_management,a_dlbase WHERE A_DLBASE.MZ_ID = C_REVIEW_MANAGEMENT.MZ_ID AND C_REVIEW_MANAGEMENT.REVIEW_LEVEL = '2'AND C_REVIEW_MANAGEMENT.MZ_ID = '" + Session["ADPMZ_ID"].ToString() + "' ", "get");
                //開放人事權限E無簽核權限可瀏覽 20190709 by sky
                //if (temp1.Rows.Count == 0)
                //{
                //    Response.Redirect("~/ErrorPage.aspx?erid=TPFXXX0001");
                //}
            }
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
                        cells[i + 1, 1].PutValue(dr["DATETIME"].ToString().Replace("上午 12:00:00","") + " " + dr["INTIME"].ToString() + "~" + dr["OUTTIME"].ToString());
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
                    Response.Redirect("~/ErrorPage.aspx?erid=TPFXXX0001");
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

        //class ReportObject
        //{
        //    public string MZ_NAME { get; set; }
        //    public string MZ_AD { get; set; }
        //    public string MZ_UNIT { get; set; }
        //    public string MZ_OCCC { get; set; }
        //    public string STATUS { get; set; }

        //    public ReportObject() { }
        //    public ReportObject(ReportObject old)
        //    {
        //        MZ_NAME = old.MZ_NAME;
        //        MZ_AD = old.MZ_AD;
        //        MZ_OCCC = old.MZ_OCCC;
        //        MZ_UNIT = old.MZ_UNIT;
        //    }
        //}
    }
}