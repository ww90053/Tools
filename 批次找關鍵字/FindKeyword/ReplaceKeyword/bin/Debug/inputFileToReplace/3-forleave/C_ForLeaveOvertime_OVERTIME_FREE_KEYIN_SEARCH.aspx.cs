using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TPPDDB._3_forleave
{
    public partial class C_ForLeaveOvertime_OVERTIME_FREE_KEYIN_SEARCH : System.Web.UI.Page
    {
        /// <summary>表單 : 載入</summary>
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                switch (o_a_Function.strGID(Session["ADPMZ_ID"].ToString()))
                {
                    case "A":
                        ChangeDropDownList_AD();
                        break;
                    case "B":
                        ChangeDropDownList_AD();
                        //DropDownList_MZ_EXAD.DataBind();
                        DropDownList_MZ_EXAD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                        ChangeUnit();
                        break;
                    case "C":
                        ChangeDropDownList_AD();
                        //DropDownList_MZ_EXAD.DataBind();
                        DropDownList_MZ_EXAD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                        //DropDownList_MZ_EXAD.Enabled = false;
                        //如果是中和分局進來改成只抓中和一&中和二&中和 matthew
                        if (Session["ADPMZ_EXAD"].ToString() != "382133600C")
                        {
                            DropDownList_MZ_EXAD.Enabled = false;
                        }
                        ChangeUnit();
                        break;
                    case "D":
                        ChangeDropDownList_AD();
                        if (!(Session["ADPMZ_ID"].ToString() == "K220886357"))//20141104先開給鳳美姊 之後再釐清邱課是怎麼開權限給他用超勤
                        {
                            //DropDownList_MZ_EXAD.DataBind();
                            DropDownList_MZ_EXAD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                            //DropDownList_MZ_EXAD.Enabled = false;
                            //如果是中和分局進來改成只抓中和一&中和二&中和 matthew
                            if (Session["ADPMZ_EXAD"].ToString() != "382133600C")
                            {
                                DropDownList_MZ_EXAD.Enabled = false;
                            }
                            ChangeUnit();
                            DropDownList_MZ_EXUNIT.Text = Session["ADPMZ_EXUNIT"].ToString();
                            DropDownList_MZ_EXUNIT.Enabled = false;
                            TextBox_MZ_ID.Text = Session["ADPMZ_ID"].ToString();
                            TextBox_MZ_ID.Enabled = false;
                            TextBox_MZ_NAME.Text = o_A_DLBASE.CNAME(Session["ADPMZ_ID"].ToString());
                            TextBox_MZ_NAME.Enabled = false;
                        }
                        else
                        {
                            //DropDownList_MZ_EXAD.DataBind();
                            DropDownList_MZ_EXAD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                            ChangeUnit();
                        }
                        break;
                    case "E":
                        ChangeDropDownList_AD();
                        //DropDownList_MZ_EXAD.DataBind();
                        DropDownList_MZ_EXAD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                        //DropDownList_MZ_EXAD.Enabled = false;
                        //如果是中和分局進來改成只抓中和一&中和二&中和 matthew
                        if (Session["ADPMZ_EXAD"].ToString() != "382133600C")
                        {
                            DropDownList_MZ_EXAD.Enabled = false;
                        }
                        ChangeUnit();
                        DropDownList_MZ_EXUNIT.Text = Session["ADPMZ_EXUNIT"].ToString();
                        DropDownList_MZ_EXUNIT.Enabled = false;
                        break;
                }
            }
            C.set_Panel_EnterToTAB(ref this.Panel1);
        }
        //matthew 中和分局
        protected void ChangeDropDownList_AD()
        {
            string strSQL = "SELECT * FROM Z_CODE_ON WHERE NEW='" + Session["ADPMZ_EXAD"].ToString() + "'";
            DataTable tempDT = new DataTable();
            tempDT = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
            //如果是中和分局進來改成只抓中和一&中和二&中和 matthew
            if (Session["ADPMZ_EXAD"].ToString() == "382133600C")
            {
                strSQL = "SELECT  MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE = '04' AND (MZ_KCODE in ('382133400C','382133500C','382133600C'))";
            }
            else
            {
                strSQL = "SELECT MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KCODE LIKE '38213%' AND MZ_KTYPE='04'";
            }
            DataTable dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");
            DropDownList_MZ_EXAD.DataSource = dt;
            DropDownList_MZ_EXAD.DataTextField = "MZ_KCHI";
            DropDownList_MZ_EXAD.DataValueField = "MZ_KCODE";
            DropDownList_MZ_EXAD.DataBind();

            DropDownList_MZ_EXAD.SelectedValue = Session["ADPMZ_EXAD"].ToString();

        }

        /// <summary>下拉 : 現服機關變更</summary>
        protected void DropDownList_MZ_EXAD_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeUnit();
        }

        /// <summary>按鈕 : 確認送出</summary>
        protected void btOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBox_MZ_DUTYDATE.Text))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "click", "alert('請輸入年度月份')", true);
                return;
            }

            //現服機關單位
            String AD = DropDownList_MZ_EXAD.SelectedValue.Trim();
            String UNIT = DropDownList_MZ_EXUNIT.SelectedValue.Trim();
            String DutyDate = String.IsNullOrEmpty(TextBox_MZ_DUTYDATE.Text.Trim()) ? String.Empty : TextBox_MZ_DUTYDATE.Text.Trim().Replace("/", String.Empty).PadLeft(5, '0');

            #region 此段Cookie應不必用到，確定修正完成後可刪除
            HttpCookie Cookie1 = new HttpCookie("ForLeaveOvertime_OVERTIME_FREE_KEYIN_ID");
            HttpCookie Cookie2 = new HttpCookie("ForLeaveOvertime_OVERTIME_FREE_KEYIN_NAME");
            Cookie1.Value = TPMPermissions._strEncood(TextBox_MZ_ID.Text.Trim());
            Cookie2.Value = TPMPermissions._strEncood(TextBox_MZ_NAME.Text.Trim());
            Response.Cookies.Add(Cookie1);
            Response.Cookies.Add(Cookie2);
            #endregion

            //20150527 Neil 所有查詢參數統一改為Session儲存，但在接收端與傳送端均須做好 Session 管理動作
            Session["OverTime_Search_DutyDate"] = DutyDate; //日期
            Session["OverTime_Search_ID"]       = TextBox_MZ_ID.Text.Trim(); //身分證
            Session["OverTime_Search_Name"]     = TextBox_MZ_NAME.Text.Trim(); //姓名
            Session["OverTime_Search_EXAD"]     = AD; //現服機關
            Session["OverTime_Search_EXUNIT"]   = UNIT; //現服單位

            //↓下面的部分在 Session 確認成功後，應也可進行刪除動作
            //AD 跟 UNIT 透過網址列進行傳送
            //重新刷新主要輸入頁面，並在網址列額外傳入 AD 跟 UNIT
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click",
                "window.opener.location.href='C_ForLeaveOvertime_OVERTIME_FREE_KEYIN.aspx?TYPE=search&AD=" + AD + "&UNIT=" + UNIT + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "';window.close();", true);

        }

        /// <summary>變更單位</summary>
        private void ChangeUnit()
        {
            DataTable temp = new DataTable();
            string strSQL = "SELECT RTRIM(MZ_KCHI),RTRIM(MZ_KCODE) FROM A_KTYPE WHERE MZ_KTYPE='25' AND MZ_KCODE IN (SELECT MZ_UNIT FROM A_UNIT_AD WHERE MZ_AD='" + DropDownList_MZ_EXAD.SelectedValue + "')";
            temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");
            DropDownList_MZ_EXUNIT.DataSource = temp;
            DropDownList_MZ_EXUNIT.DataTextField = "RTRIM(MZ_KCHI)";
            DropDownList_MZ_EXUNIT.DataValueField = "RTRIM(MZ_KCODE)";
            DropDownList_MZ_EXUNIT.DataBind();
            DropDownList_MZ_EXUNIT.Items.Insert(0, "");
            //如果是中和分局進來改成只抓中和一&中和二&中和 matthew
            if (Session["ADPMZ_EXAD"].ToString() == "382133600C")
            {
                DropDownList_MZ_EXUNIT.Text = Session["ADPMZ_EXUNIT"].ToString();
            }
        }
    }
}
