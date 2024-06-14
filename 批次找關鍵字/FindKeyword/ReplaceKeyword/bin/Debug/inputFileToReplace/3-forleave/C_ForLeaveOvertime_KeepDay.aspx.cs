using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using TPPDDB.Helpers;
using TPPDDB.Models._3_ForLeave;

namespace TPPDDB._3_forleave
{
    public partial class C_ForLeaveOvertime_KeepDay : C_BasePage
    {
        C_ForLeaveOvertime_KeepDay_Query _query
        {
            get { return ViewState["query"] != null ? (C_ForLeaveOvertime_KeepDay_Query)ViewState["query"] : null; }
            set { ViewState["query"] = value; }
        }

        /// <summary>
        /// 查詢資料
        /// </summary>
        protected void Do_Search(C_ForLeaveOvertime_KeepDay_Query query)
        {
            GridView1.DataSource = CFService.GetC_DLTBB(query);
            GridView1.DataBind();
        }
        /// <summary>
        /// 檢查代碼是否有對應中文
        /// </summary>
        /// <param name="Cname"></param>
        /// <param name="tb1"></param>
        /// <param name="tb2"></param>
        /// <param name="obj"></param>
        private void Ktype_Cname_Check(string Cname, TextBox tb1, TextBox tb2, object obj)
        {

            tb2.Text = tb2.Text.ToUpper();
            //檢查代碼中文名稱是否存在
            if (string.IsNullOrEmpty(Cname))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('查無資料')", true);
                tb1.Text = string.Empty;
                tb2.Text = string.Empty;
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + tb2.ClientID + "').focus();$get('" + tb2.ClientID + "').focus();", true);
            }
            else
            {
                tb1.Text = Cname;
                if (obj is TextBox)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(string), "", "$get('" + (obj as TextBox).ClientID + "').focus();$get('" + (obj as TextBox).ClientID + "').focus();", true);
                }
                else if (obj is Button)
                {
                    (obj as Button).Focus();
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //取得群組權限，人事系統取得 MZ_POWER
            _strGID = o_a_Function.strGID(Session["ADPMZ_ID"].ToStringNullSafe());
            //取得模組
            _TPM_FION = Request.QueryString["TPM_FION"].ToStringNullSafe();

            if (!IsPostBack)
            {
                C.check_power();
                //MQ-----------------------20100331
                C.set_Panel_EnterToTAB(ref this.Panel2);

                txt_Year.Text = (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0');
            }
        }
        /// <summary>
        /// 按鈕: 查詢
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button_Search_Click(object sender, EventArgs e)
        {
            _query = new C_ForLeaveOvertime_KeepDay_Query()
            {
                MZ_YEAR = txt_Year.Text.SafeTrim(),
                MZ_ID = TextBox_MZ_ID.Text.SafeTrim(),
                MZ_EXAD = TextBox_MZ_AD.Text.SafeTrim(),
                MZ_EXUNIT = TextBox_MZ_UNIT.Text.SafeTrim(),
                TPMFION = _TPM_FION
            };

            Do_Search(_query);
        }
        /// <summary>
        /// 按鈕: 開啟機關選擇視窗
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btAD_Click(object sender, EventArgs e)
        {
            Session["KTYPE_CID"] = TextBox_MZ_AD.ClientID;
            Session["KTYPE_CID1"] = TextBox_MZ_AD1.ClientID;

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('../1-personnel/Personal_Ktype_Search.aspx?CID_NAME=AD&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin','top=190,left=200,width=420,height=450,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }
        /// <summary>
        /// 按鈕: 開啟單位選擇視窗
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btUNIT_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(TextBox_MZ_AD.Text))
            {
                Session["KTYPE_CID"] = TextBox_MZ_UNIT.ClientID;
                Session["KTYPE_CID1"] = TextBox_MZ_UNIT1.ClientID;
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('../1-personnel/Personal_Ktype_Search.aspx?CID_NAME=UNIT&AD=" + TextBox_MZ_AD.Text.Trim() + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin','top=190,left=200,width=420,height=450,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "alert('請先輸入機關!')", true);
            }
        }


        /// <summary>
        /// 輸入框: 身分證號輸入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TextBox_MZ_ID_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(TextBox_MZ_ID.Text))
            {
                string CheckID = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_ID FROM A_DLBASE WHERE MZ_ID = '" + TextBox_MZ_ID.Text + "'");
                if (string.IsNullOrEmpty(CheckID))
                {
                    TextBox_MZ_AD.Text = string.Empty;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "alert('查無資料')", true);
                }
            }
        }
        /// <summary>
        /// 輸入框: 機關代碼輸入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TextBox_MZ_AD_TextChanged(object sender, EventArgs e)
        {
            string CName = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM (SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)='38213' UNION ALL SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' AND dbo.SUBSTR(MZ_KCODE,1,5)<>'38213') WHERE MZ_KCODE='" + TextBox_MZ_AD.Text.Trim().ToUpper() + "'");
            Ktype_Cname_Check(CName, TextBox_MZ_AD1, TextBox_MZ_AD, TextBox_MZ_UNIT);
        }
        /// <summary>
        /// 輸入框: 單位代碼輸入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TextBox_MZ_UNIT_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBox_MZ_AD.Text.Trim()))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "alert('請先輸入機關!')", true);
                return;
            }
            string CName = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='25' AND MZ_KCODE IN (SELECT MZ_UNIT FROM A_UNIT_AD WHERE MZ_AD='" + TextBox_MZ_AD.Text.Trim().ToUpper() + "') AND MZ_KCODE='" + TextBox_MZ_UNIT.Text.Trim().ToUpper() + "'");
            Ktype_Cname_Check(CName, TextBox_MZ_UNIT1, TextBox_MZ_UNIT, Button_Search);
        }
        /// <summary>
        /// 輸入框: 保留假1 天數修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TextBox_MZ_SDAY_TextChanged(object sender, EventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((TextBox)sender).NamingContainer;
            GridView gv = (GridView)gvr.NamingContainer;
            int index = gvr.RowIndex;
            string MZ_ID = gv.Rows[index].Cells[2].Text;
            TextBox TB_MZ_SDAY = gv.Rows[index].Cells[3].Controls[1] as TextBox;
            string MZ_YEAR = gv.DataKeys[index]["MZ_YEAR"].ToStringNullSafe();
            if (string.IsNullOrEmpty(TB_MZ_SDAY.Text))
            {
                TB_MZ_SDAY.Text = "0";
            }
            string UpdateStrSql = string.Format(@"UPDATE C_DLTBB SET MZ_SDAY={0} WHERE MZ_ID='{1}' AND MZ_YEAR='{2}' ", TB_MZ_SDAY.Text, MZ_ID, MZ_YEAR);
            o_DBFactory.ABC_toTest.vExecSQL(UpdateStrSql);
            LogModel.saveLog("CF", "U", UpdateStrSql, new List<SqlParameter>(), _TPM_FION, "修改保留假1天資料。");
        }
        /// <summary>
        /// 輸入框: 保留假1 時數修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TextBox_MZ_SDAY_HOUR_TextChanged(object sender, EventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((TextBox)sender).NamingContainer;
            GridView gv = (GridView)gvr.NamingContainer;
            int index = gvr.RowIndex;
            string MZ_ID = gv.Rows[index].Cells[2].Text;
            TextBox TB_MZ_SDAY_HOUR = gv.Rows[index].Cells[4].Controls[1] as TextBox;
            string MZ_YEAR = gv.DataKeys[index]["MZ_YEAR"].ToStringNullSafe();
            if (string.IsNullOrEmpty(TB_MZ_SDAY_HOUR.Text))
            {
                TB_MZ_SDAY_HOUR.Text = "0";
            }
            string UpdateStrSql = string.Format(@"UPDATE C_DLTBB SET MZ_SDAY_HOUR={0} WHERE MZ_ID='{1}' AND MZ_YEAR='{2}' ", TB_MZ_SDAY_HOUR.Text, MZ_ID, MZ_YEAR);
            o_DBFactory.ABC_toTest.vExecSQL(UpdateStrSql);
            LogModel.saveLog("CF", "U", UpdateStrSql, new List<SqlParameter>(), _TPM_FION, "修改保留假1時資料。");
        }
        /// <summary>
        /// 輸入框: 保留假2 天數修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TextBox_MZ_SDAY2_TextChanged(object sender, EventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((TextBox)sender).NamingContainer;
            GridView gv = (GridView)gvr.NamingContainer;
            int index = gvr.RowIndex;
            string MZ_ID = gv.Rows[index].Cells[2].Text;
            TextBox TB_MZ_SDAY2 = gv.Rows[index].Cells[5].Controls[1] as TextBox;
            string MZ_YEAR = gv.DataKeys[index]["MZ_YEAR"].ToStringNullSafe();
            if (string.IsNullOrEmpty(TB_MZ_SDAY2.Text))
            {
                TB_MZ_SDAY2.Text = "0";
            }
            string UpdateStrSql = string.Format(@"UPDATE C_DLTBB SET MZ_SDAY2={0} WHERE MZ_ID='{1}' AND MZ_YEAR='{2}' ", TB_MZ_SDAY2.Text, MZ_ID, MZ_YEAR);
            o_DBFactory.ABC_toTest.vExecSQL(UpdateStrSql);
            LogModel.saveLog("CF", "U", UpdateStrSql, new List<SqlParameter>(), _TPM_FION, "修改保留假2天資料。");
        }
        /// <summary>
        /// 輸入框: 保留假2 時數修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TextBox_MZ_SDAY2_HOUR_TextChanged(object sender, EventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((TextBox)sender).NamingContainer;
            GridView gv = (GridView)gvr.NamingContainer;
            int index = gvr.RowIndex;
            string MZ_ID = gv.Rows[index].Cells[2].Text;
            TextBox TB_MZ_SDAY2_HOUR = gv.Rows[index].Cells[6].Controls[1] as TextBox;
            string MZ_YEAR = gv.DataKeys[index]["MZ_YEAR"].ToStringNullSafe();
            if (string.IsNullOrEmpty(TB_MZ_SDAY2_HOUR.Text))
            {
                TB_MZ_SDAY2_HOUR.Text = "0";
            }
            string UpdateStrSql = string.Format(@"UPDATE C_DLTBB SET MZ_SDAY2_HOUR={0} WHERE MZ_ID='{1}' AND MZ_YEAR='{2}' ", TB_MZ_SDAY2_HOUR.Text, MZ_ID, MZ_YEAR);
            o_DBFactory.ABC_toTest.vExecSQL(UpdateStrSql);
            LogModel.saveLog("CF", "U", UpdateStrSql, new List<SqlParameter>(), _TPM_FION, "修改保留假2時資料。");
        }
        /// <summary>
        /// 輸入框: 保留假 天數修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TextBox_MZ_SDAY3_TextChanged(object sender, EventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((TextBox)sender).NamingContainer;
            GridView gv = (GridView)gvr.NamingContainer;
            int index = gvr.RowIndex;
            string MZ_ID = gv.Rows[index].Cells[2].Text;
            TextBox TB_MZ_SDAY3 = gv.Rows[index].Cells[7].Controls[1] as TextBox;
            string MZ_YEAR = gv.DataKeys[index]["MZ_YEAR"].ToStringNullSafe();
            if (string.IsNullOrEmpty(TB_MZ_SDAY3.Text))
            {
                TB_MZ_SDAY3.Text = "0";
            }
            string UpdateStrSql = string.Format(@"UPDATE C_DLTBB SET MZ_SDAY3={0} WHERE MZ_ID='{1}' AND MZ_YEAR='{2}' ", TB_MZ_SDAY3.Text, MZ_ID, MZ_YEAR);
            o_DBFactory.ABC_toTest.vExecSQL(UpdateStrSql);
            LogModel.saveLog("CF", "U", UpdateStrSql, new List<SqlParameter>(), _TPM_FION, "修改保留假天資料。");
        }
        /// <summary>
        /// 輸入框: 保留假 時數修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TextBox_MZ_SDAY3_HOUR_TextChanged(object sender, EventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((TextBox)sender).NamingContainer;
            GridView gv = (GridView)gvr.NamingContainer;
            int index = gvr.RowIndex;
            string MZ_ID = gv.Rows[index].Cells[2].Text;
            TextBox TB_MZ_SDAY3_HOUR = gv.Rows[index].Cells[8].Controls[1] as TextBox;
            string MZ_YEAR = gv.DataKeys[index]["MZ_YEAR"].ToStringNullSafe();
            if (string.IsNullOrEmpty(TB_MZ_SDAY3_HOUR.Text))
            {
                TB_MZ_SDAY3_HOUR.Text = "0";
            }
            string UpdateStrSql = string.Format(@"UPDATE C_DLTBB SET MZ_SDAY3_HOUR={0} WHERE MZ_ID='{1}' AND MZ_YEAR='{2}' ", TB_MZ_SDAY3_HOUR.Text, MZ_ID, MZ_YEAR);
            o_DBFactory.ABC_toTest.vExecSQL(UpdateStrSql);
            LogModel.saveLog("CF", "U", UpdateStrSql, new List<SqlParameter>(), _TPM_FION, "修改保留假時資料。");
        }
        /// <summary>
        /// 輸入框: 併計年 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TextBox_MZ_TYEAR_TextChanged(object sender, EventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((TextBox)sender).NamingContainer;
            GridView gv = (GridView)gvr.NamingContainer;
            int index = gvr.RowIndex;
            string MZ_ID = gv.Rows[index].Cells[2].Text;
            TextBox TB_TYEAR = gv.Rows[index].Cells[9].Controls[1] as TextBox;
            if (string.IsNullOrEmpty(TB_TYEAR.Text))
            {
                TB_TYEAR.Text = "0";
            }
            string UpdateStrSql = string.Format(@"UPDATE A_DLBASE SET MZ_TYEAR={0} WHERE MZ_ID='{1}' ", TB_TYEAR.Text, MZ_ID);
            o_DBFactory.ABC_toTest.vExecSQL(UpdateStrSql);
            LogModel.saveLog("CF", "U", UpdateStrSql, new List<SqlParameter>(), _TPM_FION, "修改併計年資料。");
        }
        /// <summary>
        /// 輸入框: 併計月 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TextBox_MZ_TMONTH_TextChanged(object sender, EventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((TextBox)sender).NamingContainer;
            GridView gv = (GridView)gvr.NamingContainer;
            int index = gvr.RowIndex;
            string MZ_ID = gv.Rows[index].Cells[2].Text;
            TextBox TB_MZ_TMONTH = gv.Rows[index].Cells[10].Controls[1] as TextBox;
            if (string.IsNullOrEmpty(TB_MZ_TMONTH.Text))
            {
                TB_MZ_TMONTH.Text = "0";
            }
            string UpdateStrSql = string.Format(@"UPDATE A_DLBASE SET MZ_TMONTH={0} WHERE MZ_ID='{1}' ", TB_MZ_TMONTH.Text, MZ_ID);
            o_DBFactory.ABC_toTest.vExecSQL(UpdateStrSql);
            LogModel.saveLog("CF", "U", UpdateStrSql, new List<SqlParameter>(), _TPM_FION, "修改併計月資料。");
        }
        /// <summary>
        /// 輸入框: 併計年資相關文件 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TextBox_MZ_MEMO_TextChanged(object sender, EventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((TextBox)sender).NamingContainer;
            GridView gv = (GridView)gvr.NamingContainer;
            int index = gvr.RowIndex;
            string MZ_ID = gv.Rows[index].Cells[2].Text;
            TextBox TB_MZ_MEMO = gv.Rows[index].Cells[11].Controls[1] as TextBox;
            string UpdateStrSql = string.Format(@"UPDATE A_DLBASE SET MZ_MEMO='{0}' WHERE MZ_ID='{1}' ", TB_MZ_MEMO.Text, MZ_ID);
            o_DBFactory.ABC_toTest.vExecSQL(UpdateStrSql);
            LogModel.saveLog("CF", "U", UpdateStrSql, new List<SqlParameter>(), _TPM_FION, "修改併計年資相關文件資料。");
        }
        /// <summary>
        /// 輸入框: 減年資 年數修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TextBox_MZ_RYEAR_TextChanged(object sender, EventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((TextBox)sender).NamingContainer;
            GridView gv = (GridView)gvr.NamingContainer;
            int index = gvr.RowIndex;
            string MZ_ID = gv.Rows[index].Cells[2].Text;
            TextBox TB_RYEAR = gv.Rows[index].Cells[12].Controls[1] as TextBox;
            if (string.IsNullOrEmpty(TB_RYEAR.Text))
            {
                TB_RYEAR.Text = "0";
            }
            string UpdateStrSql = string.Format(@"UPDATE A_DLBASE SET MZ_RYEAR={0} WHERE MZ_ID='{1}' ", TB_RYEAR.Text, MZ_ID);
            o_DBFactory.ABC_toTest.vExecSQL(UpdateStrSql);
            LogModel.saveLog("CF", "U", UpdateStrSql, new List<SqlParameter>(), _TPM_FION, "修改減年資年資料。");
        }
        /// <summary>
        /// 輸入框: 減年資 月數修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TextBox_MZ_RMONTH_TextChanged(object sender, EventArgs e)
        {
            GridViewRow gvr = (GridViewRow)((TextBox)sender).NamingContainer;
            GridView gv = (GridView)gvr.NamingContainer;
            int index = gvr.RowIndex;
            string MZ_ID = gv.Rows[index].Cells[2].Text;
            TextBox TB_RMONTH = gv.Rows[index].Cells[13].Controls[1] as TextBox;
            if (string.IsNullOrEmpty(TB_RMONTH.Text))
            {
                TB_RMONTH.Text = "0";
            }
            string UpdateStrSql = string.Format(@"UPDATE A_DLBASE SET MZ_RMONTH={0} WHERE MZ_ID='{1}' ", TB_RMONTH.Text, MZ_ID);
            o_DBFactory.ABC_toTest.vExecSQL(UpdateStrSql);
            LogModel.saveLog("CF", "U", UpdateStrSql, new List<SqlParameter>(), _TPM_FION, "修改減年資月資料。");
        }

        /// <summary>
        /// GridView: 換頁查詢
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            Do_Search(_query);
        }


        protected void Ktype_Search(TextBox tb1, TextBox tb2, string ktype)
        {
            //回傳值
            Session["KTYPE_CID"] = tb1.ClientID;
            Session["KTYPE_CID1"] = tb2.ClientID;
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('../1-personnel/Personal_Ktype_Search.aspx?MZ_KTYPE=" + ktype + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "','newwin','top=190,left=200,width=420,height=450,toolbar=0,menubar=0,location=0,directories=0,status=0');", true);
        }
    }
}
