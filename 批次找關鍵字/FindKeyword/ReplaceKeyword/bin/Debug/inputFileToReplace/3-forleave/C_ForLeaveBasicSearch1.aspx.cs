using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TPPDDB._3_forleave
{
    public partial class C_ForLeaveBasicSearch1 : System.Web.UI.Page
    {
      

        protected void Page_Load(object sender, EventArgs e)
        {
            ViewState["TableName"] = Request["TableName"];

           
            if (!IsPostBack)
            {
                //MQ ------------------20100331
            C.set_Panel_EnterToTAB(ref this.Panel2);

                 
                switch (o_a_Function.strGID(Session["ADPMZ_ID"].ToString()))
                {
                    case "A":                        
                    case "B":
                        DropDownList_MZ_EXAD.DataBind();
                        DropDownList_MZ_EXAD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                        ChangeUnit();
                        break;
                    case "C":
                        DropDownList_MZ_EXAD.DataBind();
                        DropDownList_MZ_EXAD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                        DropDownList_MZ_EXAD.Enabled = false;
                        //matthew 中和分局進來要可以選中一中二
                        if (Session["ADPMZ_EXAD"].ToString() == "382133600C")
                        {
                            DropDownList_MZ_EXAD.Items.Clear();
                            DropDownList_MZ_EXAD.DataSourceID = null;
                            string strSQL2 = "";
                            strSQL2 = "SELECT ' ' as MZ_KCHI, ' ' as MZ_KCODE  union all SELECT NVL(REPLACE( RTRIM(MZ_KCHI),'新北市政府警察局',''),'警察局') AS MZ_KCHI, RTRIM(MZ_KCODE) AS MZ_KCODE FROM A_KTYPE WHERE (RTRIM(MZ_KTYPE) = '04') AND (RTRIM(MZ_KCODE) LIKE '38213%') and mz_kchi like '%分局' or RTRIM(mz_kcode) in ('382133400C','382133500C','382133600C')  order by mz_kcode";
                            DataTable temp2 = new DataTable();
                            temp2 = o_DBFactory.ABC_toTest.Create_Table(strSQL2, "get");
                            DropDownList_MZ_EXAD.DataSource = temp2;

                            DropDownList_MZ_EXAD.DataTextField = "MZ_KCHI";
                            DropDownList_MZ_EXAD.DataValueField = "MZ_KCODE";
                            DropDownList_MZ_EXAD.DataBind();
                            DropDownList_MZ_EXAD.Enabled = true;
                        }
                        ChangeUnit();
                        break;
                    case "D":                       
                    case "E":
                        DropDownList_MZ_EXAD.DataBind();
                        DropDownList_MZ_EXAD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                        DropDownList_MZ_EXAD.Enabled = false;
                        //matthew 中和分局進來要可以選中一中二
                        if (Session["ADPMZ_EXAD"].ToString() == "382133600C")
                        {
                            DropDownList_MZ_EXAD.Items.Clear();
                            DropDownList_MZ_EXAD.DataSourceID = null;
                            string strSQL2 = "";
                            strSQL2 = "SELECT ' ' as MZ_KCHI, ' ' as MZ_KCODE  union all SELECT NVL(REPLACE( RTRIM(MZ_KCHI),'新北市政府警察局',''),'警察局') AS MZ_KCHI, RTRIM(MZ_KCODE) AS MZ_KCODE FROM A_KTYPE WHERE (RTRIM(MZ_KTYPE) = '04') AND (RTRIM(MZ_KCODE) LIKE '38213%') and mz_kchi like '%分局' or RTRIM(mz_kcode) in ('382133400C','382133500C','382133600C')  order by mz_kcode";
                            DataTable temp2 = new DataTable();
                            temp2 = o_DBFactory.ABC_toTest.Create_Table(strSQL2, "get");
                            DropDownList_MZ_EXAD.DataSource = temp2;

                            DropDownList_MZ_EXAD.DataTextField = "MZ_KCHI";
                            DropDownList_MZ_EXAD.DataValueField = "MZ_KCODE";
                            DropDownList_MZ_EXAD.DataBind();
                            DropDownList_MZ_EXAD.Enabled = true;
                        }
                        ChangeUnit();
                        DropDownList_MZ_EXUNIT.Text = Session["ADPMZ_EXUNIT"].ToString();
                        DropDownList_MZ_EXUNIT.Enabled = false;
                        break;
                }

                if (ViewState["TableName"].ToString() == "CHECK")//4.2差假核定作業
                {
                    Panel1.Visible = true;
                }
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.close();", true);
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            HttpCookie Cookie1 = new HttpCookie("ForLeaveBasicSearch_ID");
            Cookie1.Value = TPMPermissions._strEncood(TextBox_ID.Text.Trim());
            Response.Cookies.Add(Cookie1);

            HttpCookie Cookie2 = new HttpCookie("ForLeaveBasicSearch_NAME");
            Cookie2.Value = TPMPermissions._strEncood(TextBox_NAME.Text.Trim());
            Response.Cookies.Add(Cookie2);

            string EXUNIT = "";
            string EXAD = "";

            if (DropDownList_MZ_EXAD.SelectedValue == "請選擇")
            {
                EXAD = "";
            }
            else
            {
                EXAD = DropDownList_MZ_EXAD.SelectedValue;
            }

            if (DropDownList_MZ_EXUNIT.SelectedValue == "請選擇")
            {
                EXUNIT = "";
            }
            else
            {
                EXUNIT = DropDownList_MZ_EXUNIT.SelectedValue;
            }

            //2013/09/26
            string is_check = "";
            if (!string.IsNullOrEmpty(rbl_check.SelectedValue.ToString()))
            {
                is_check = rbl_check.SelectedValue; 
            
            }
            //2013/09/26
            if (ViewState["TableName"].ToString() == "CHECK")//4.2差假核定作業
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.opener.location.href='C_ForLeaveOvertime_Leave_Check.aspx?MZ_EXAD=" + EXAD + "&MZ_EXUNIT=" + EXUNIT + "&MZ_IDATE1=" + TextBox_MZ_IDATE1.Text.Replace("/", string.Empty).PadLeft(7, '0') + "&MZ_IDATE2=" + TextBox_MZ_IDATE2.Text.Replace("/", string.Empty).PadLeft(7, '0') +"&ISCHECK="+is_check+ "&TPM_FION=" + Request.QueryString["TPM_FION"] + "';window.close();", true);
            }
            else if (ViewState["TableName"].ToString() == "CARDSET")
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.opener.location.href='C_CARDSET.aspx?MZ_EXAD=" + EXAD + "&MZ_EXUNIT=" + EXUNIT + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "';window.close();", true);
            }
        }

        protected void ChangeUnit()
        {
            DataTable temp = new DataTable();
            string strSQL = "SELECT RTRIM(MZ_KCHI),RTRIM(MZ_KCODE) FROM A_KTYPE WHERE MZ_KTYPE='25' AND MZ_KCODE IN (SELECT MZ_UNIT FROM A_UNIT_AD WHERE MZ_AD='" + DropDownList_MZ_EXAD.SelectedValue + "')";
            temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");
            DropDownList_MZ_EXUNIT.DataSource = temp;
            DropDownList_MZ_EXUNIT.DataTextField = "RTRIM(MZ_KCHI)";
            DropDownList_MZ_EXUNIT.DataValueField = "RTRIM(MZ_KCODE)";
            DropDownList_MZ_EXUNIT.DataBind();
            DropDownList_MZ_EXUNIT.Items.Insert(0, "請選擇");
        }

        protected void DropDownList_MZ_EXAD_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeUnit();
        }

    }
}
