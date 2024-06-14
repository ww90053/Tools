using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using TPPDDB.App_Code;

namespace TPPDDB._3_forleave
{
    public partial class C_codecount_rpt : System.Web.UI.Page
    {
        int TPM_FION = 0;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                C.check_power();

                //by MQ ------------------------------20100331

                C.set_Panel_EnterToTAB(ref this.Panel1);

                C.fill_AD_POST(DropDownList_MZ_AD);
                ////如果是中和分局進來改成只抓中和一&中和二&中和 matthew
                //if (Session["ADPMZ_EXAD"].ToString() == "382133600C")
                //{
                //    C.fill_DLL_ONE_TWO(DropDownList_MZ_AD);
                //}
                //else
                //{
                //    //把所有機關撈出來包含台北縣
                //    C.fill_AD_POST(DropDownList_MZ_AD);
                //}
                DropDownList_MZ_AD.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                unit();
                chk_TPMGroup();
            }
        }

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
                strSQL = "SELECT  MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE = '04' AND (MZ_KCODE='" + tempDT.Rows[0][0].ToString() + "' OR MZ_KCODE='" + tempDT.Rows[0][1].ToString() + "')";

            }
            DataTable dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");
            DropDownList_MZ_AD.DataSource = dt;
            DropDownList_MZ_AD.DataTextField = "MZ_KCHI";
            DropDownList_MZ_AD.DataValueField = "MZ_KCODE";
            DropDownList_MZ_AD.DataBind();

            DropDownList_MZ_AD.SelectedValue = Session["ADPMZ_EXAD"].ToString();

        }

        /// <summary>
        /// 群組權限
        /// </summary>
        protected void chk_TPMGroup()
        {
            switch (o_a_Function.strGID(Session["ADPMZ_ID"].ToString()))
            {
                case "A":
                case "B":

                    break;
                case "C":
                    ChangeDropDownList_AD();
                    break;
                case "D":
                    //無權限
                    TPMPermissions._boolErrorData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "ADUXXX0001");
                    Response.Redirect("~/ErrorPage.aspx?erid=ADUXXX0001");
                    break;
                case "E":
                    ChangeDropDownList_AD();
                    DropDownList_MZ_UNIT.SelectedValue = Session["ADPMZ_EXAD"].ToString();
                    DropDownList_MZ_UNIT.Enabled = false;                 
                    break;
            }
        }

        protected void btPrint_Click(object sender, EventArgs e)
        {
            DataTable CODECOUNT = new DataTable();

            CODECOUNT.Columns.Clear();

            CODECOUNT.Columns.Add("MZ_CODE", typeof(string));
            CODECOUNT.Columns.Add("MZ_COUNT", typeof(string));
            CODECOUNT.Columns.Add("MZ_DAYS", typeof(string));

            string strSQL = "SELECT MZ_CODE FROM C_DLCODE ORDER BY MZ_CODE ";

            DataTable CODE_DT = new DataTable();

            CODE_DT = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GETCODE");

            string UNIT = "";

            if (DropDownList_MZ_UNIT.SelectedValue != "")
            {
                UNIT = " AND MZ_EXUNIT='" + DropDownList_MZ_UNIT.SelectedValue + "'";
            }


            for (int i = 0; i < CODE_DT.Rows.Count; i++)
            {
                TextBox_IDATE1.Text = TextBox_IDATE1.Text.PadLeft(7, '0');
                TextBox_IDATE2.Text = TextBox_IDATE2.Text.PadLeft(7, '0');
                string COUNT_STRSQL = o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM C_DLTB01 WHERE MZ_EXAD='" + DropDownList_MZ_AD.SelectedValue + "'" + UNIT + " AND MZ_CODE='" + CODE_DT.Rows[i]["MZ_CODE"].ToString() + "' AND MZ_IDATE1>='" + o_str.tosql(TextBox_IDATE1.Text.Trim().Replace("/", string.Empty).PadLeft(7, '0')) + "' AND MZ_IDATE1<='" + o_str.tosql(TextBox_IDATE2.Text.Trim().Replace("/", string.Empty).PadLeft(7, '0')) + "'");
                string SUM_TDAY = o_DBFactory.ABC_toTest.vExecSQL("SELECT SUM(MZ_TDAY) FROM C_DLTB01 WHERE MZ_EXAD='" + DropDownList_MZ_AD.SelectedValue + "'" + UNIT + " AND MZ_CODE='" + CODE_DT.Rows[i]["MZ_CODE"].ToString() + "' AND MZ_IDATE1>='" + o_str.tosql(TextBox_IDATE1.Text.Trim().Replace("/", string.Empty).PadLeft(7, '0')) + "' AND MZ_IDATE1<='" + o_str.tosql(TextBox_IDATE2.Text.Trim().Replace("/", string.Empty).PadLeft(7, '0')) + " GROUP BY MZ_CODE'");
                string SUM_TTIME = o_DBFactory.ABC_toTest.vExecSQL("SELECT SUM(MZ_TTIME) FROM C_DLTB01 WHERE MZ_EXAD='" + DropDownList_MZ_AD.SelectedValue + "'" + UNIT + " AND MZ_CODE='" + CODE_DT.Rows[i]["MZ_CODE"].ToString() + "' AND MZ_IDATE1>='" + o_str.tosql(TextBox_IDATE1.Text.Trim().Replace("/", string.Empty).PadLeft(7, '0')) + "' AND MZ_IDATE1<='" + o_str.tosql(TextBox_IDATE2.Text.Trim().Replace("/", string.Empty).PadLeft(7, '0')) + "' GROUP BY MZ_CODE");

                if ((SUM_TDAY == "" && SUM_TTIME == "") || (SUM_TDAY == "0" && SUM_TTIME == "0"))
                {

                }
                else
                {
                    DataRow dr = CODECOUNT.NewRow();

                    dr["MZ_CODE"] = o_DBFactory.ABC_toTest.vExecSQL("SELECT MZ_CNAME FROM C_DLCODE WHERE MZ_CODE='" + CODE_DT.Rows[i]["MZ_CODE"].ToString() + "'");
                    dr["MZ_COUNT"] = COUNT_STRSQL;
                    dr["MZ_DAYS"] = ((int.Parse(SUM_TDAY) * 8 + int.Parse(SUM_TTIME)) / 8).ToString() + "日" + ((int.Parse(SUM_TDAY) * 8 + int.Parse(SUM_TTIME)) % 8).ToString() + "時";
                    CODECOUNT.Rows.Add(dr);
                }
            }

            if (CODECOUNT.Rows.Count > 0)
            {
                if (DateManange.Check_date(TextBox_IDATE1.Text) && DateManange.Check_date(TextBox_IDATE2.Text))
                {
                    string BEGINDATE = TextBox_IDATE1.Text.Trim().Replace("/", string.Empty).PadLeft(7, '0');
                    string ENDDATE = TextBox_IDATE2.Text.Trim().Replace("/", string.Empty).PadLeft(7, '0');
                    Session["BEGINDATE"] = BEGINDATE.Substring(0, 3) + "年" + BEGINDATE.Substring(3, 2) + "月" + BEGINDATE.Substring(5, 2) + "日";
                    Session["ENDDATE"] = ENDDATE.Substring(0, 3) + "年" + ENDDATE.Substring(3, 2) + "月" + ENDDATE.Substring(5, 2) + "日"; ;
                    Session["TITLE"] = string.Format("{0} {1} 員警差假勤惰統計表", DropDownList_MZ_AD.SelectedItem.Text, DropDownList_MZ_UNIT.SelectedItem.Text);

                    Session["rpt_dt"] = CODECOUNT;

                    string tmp_url = "C_rpt.aspx?fn=codecount&TPM_FION=" + TPM_FION;

                    ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "alert('請檢查欲查詢之起迄日期');", true);
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "alert('無此日期之相關資料');", true);

            }
        }

        protected void unit()
        {
            DataTable temp = new DataTable();
            string strSQL = "SELECT RTRIM(MZ_KCHI),RTRIM(MZ_KCODE) FROM A_KTYPE WHERE MZ_KTYPE='25' AND MZ_KCODE IN (SELECT MZ_UNIT FROM A_UNIT_AD WHERE MZ_AD='" + DropDownList_MZ_AD.SelectedValue + "')";
            temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");
            DropDownList_MZ_UNIT.DataSource = temp;
            DropDownList_MZ_UNIT.DataTextField = "RTRIM(MZ_KCHI)";
            DropDownList_MZ_UNIT.DataValueField = "RTRIM(MZ_KCODE)";
            DropDownList_MZ_UNIT.DataBind();
            DropDownList_MZ_UNIT.Items.Insert(0, "");
        }

        protected void DropDownList_MZ_EXAD_SelectedIndexChanged(object sender, EventArgs e)
        {
            unit();
        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            TextBox_IDATE1.Text = string.Empty;
            TextBox_IDATE2.Text = string.Empty;
        }

        protected void DropDownList_MZ_UNIT_DataBound(object sender, EventArgs e)
        {
            ListItem li = new ListItem(" ", "");
            DropDownList_MZ_UNIT.Items.Insert(0, li);
        }
    }
}
