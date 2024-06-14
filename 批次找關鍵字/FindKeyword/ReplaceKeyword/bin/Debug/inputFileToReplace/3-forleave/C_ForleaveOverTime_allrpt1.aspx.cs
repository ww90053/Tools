using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TPPDDB.Helpers;
using TPPDDB.Models._3_ForLeave;
using TPPDDB.Service;

namespace TPPDDB._3_forleave
{
    public partial class C_ForleaveOverTime_allrpt1 : System.Web.UI.Page
    {
        string TPM_FION = "";
        /// <summary>
        /// 初始化各頁面功能
        /// </summary>
        private void PageInitial(string strGID)
        {
            //查詢機關選單
            string ddlAD_SQL = "SELECT RTRIM(MZ_KCHI) MZ_KCHI, RTRIM(MZ_KCODE) MZ_KCODE FROM A_KTYPE WHERE MZ_KTYPE='04' ";
            switch (strGID)
            {
                case "A": //系統管理員
                case "TPMIDISAdmin": //系統開發管理員
                case "B": //局本部承辦人
                    //如果是「中和分局」進來抓 中和第一、中和第二、中和
                    if (Session["ADPMZ_EXAD"].ToStringNullSafe() == "382133600C")
                    {
                        ddlAD_SQL += "AND MZ_KCODE in ('382133400C','382133500C','382133600C') ";
                    }
                    else
                    {
                        ddlAD_SQL += "AND MZ_KCODE LIKE '38213%' ";
                    }
                    break;
                case "C": //分局、大隊、隊承辦人
                case "D": //一般使用者
                case "E": //所屬單位承辦人
                    ddlAD_SQL += string.Format("AND MZ_KCODE ='{0}' ", Session["ADPMZ_EXAD"].ToStringNullSafe());
                    break;
                default:
                    Response.Redirect("~/ErrorPage.aspx?erid=TPFXXX0001");
                    break;
            }
            WebUIHelpers.DropDownList_DataBind(ddlAD_SQL, ddl_Search_AD, "MZ_KCHI", "MZ_KCODE", new WebUIHelpers.OtherListItem(0, new ListItem("全部", "")));
            ddl_Search_AD.SelectedValue = Session["ADPMZ_EXAD"].ToStringNullSafe();

            //查詢單位選單
            string ddlUNIT_SQL = string.Format(@"SELECT RTRIM(MZ_KCHI) MZ_KCHI,RTRIM(MZ_KCODE) MZ_KCODE FROM A_KTYPE 
                                                 WHERE MZ_KTYPE='25' AND MZ_KCODE IN (SELECT MZ_UNIT FROM A_UNIT_AD WHERE MZ_AD='{0}') ",
                                                 ddl_Search_AD.SelectedValue);
            switch (strGID)
            {
                case "D": //一般使用者
                case "E": //所屬單位承辦人
                    ddlUNIT_SQL += string.Format(@"AND MZ_KCODE='{0}' ", Session["ADPMZ_EXUNIT"].ToStringNullSafe());
                    break;
            }
            WebUIHelpers.DropDownList_DataBind(ddlUNIT_SQL, ddl_Search_Unit, "MZ_KCHI", "MZ_KCODE", new WebUIHelpers.OtherListItem(0, new ListItem("全部", "")));
            ddl_Search_Unit.SelectedValue = Session["ADPMZ_EXUNIT"].ToStringNullSafe();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //取得模組
            TPM_FION = Request.QueryString["TPM_FION"].ToStringNullSafe();
            string sg = o_a_Function.strGID(Session["ADPMZ_ID"].ToStringNullSafe());
            if (!IsPostBack)
            {
                PageInitial(sg);
                txt_Year.Text = (DateTime.Today.Year - 1911).ToString() + "01";
                txt_endYear.Text = (DateTime.Today.Year - 1911).ToString() + "06";
            }
        }

        /// <summary>
        /// 下拉選單: 查詢單位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddl_Search_AD_SelectedIndexChanged(object sender, EventArgs e)
        {
            string ddlUNIT_SQL = string.Format(@"SELECT RTRIM(MZ_KCHI) MZ_KCHI,RTRIM(MZ_KCODE) MZ_KCODE FROM A_KTYPE 
                                                 WHERE MZ_KTYPE='25' AND MZ_KCODE IN (SELECT MZ_UNIT FROM A_UNIT_AD WHERE MZ_AD='{0}') ",
                                     ddl_Search_AD.SelectedValue);

            WebUIHelpers.DropDownList_DataBind(ddlUNIT_SQL, ddl_Search_Unit, "MZ_KCHI", "MZ_KCODE", new WebUIHelpers.OtherListItem(0, new ListItem("全部", "")));
        }


        //protected void btn_allrpt1_Click(object sender, EventArgs e)
        //{
        //    string pdflink1 = System.Configuration.ConfigurationManager.AppSettings["pdflink"].ToString();

        //    string tmp_url = "C_overtime_allrpt1.aspx?pdf=1&MZ_ID=" + txt_MZ_ID.Text.Trim() + "&PM_FION=" + TPM_FION +
        //             "&Year=" + txt_Year.Text + "&Yearend=" + txt_endYear.Text +
        //             "&AD=" + ddl_Search_AD.SelectedValue + "&UNIT=" + ddl_Search_Unit.SelectedValue;
        //    string filename = "/userfiles/" + DateTime.Now.ToString("yyyyMMddHHmmssff") + ".pdf";
        //    string url = "http://" + Request.ServerVariables["Server_Name"] + ":" + Request.ServerVariables["Server_port"] + "/3-forleave/" + tmp_url;
        //    System.Diagnostics.Process.Start(pdflink1, " -s A4 -L 0 -R 0 -T 2 -B 0 " + url + " " + Server.MapPath(filename));
        //    System.Threading.Thread.Sleep(8000);
        //    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('" + filename + "');", true);
        //}

        //protected void btn_allrpt1_1_Click(object sender, EventArgs e)
        //{
        //    string pdflink1 = System.Configuration.ConfigurationManager.AppSettings["pdflink"].ToString();

        //    string tmp_url = "C_overtime_allrpt1.aspx?pdf=0&MZ_ID=" + txt_MZ_ID.Text.Trim() + "&PM_FION=" + TPM_FION +
        //             "&Year=" + txt_Year.Text + "&Yearend=" + txt_endYear.Text +
        //             "&AD=" + ddl_Search_AD.SelectedValue + "&UNIT=" + ddl_Search_Unit.SelectedValue;
        //   // string filename = "/userfiles/" + DateTime.Now.ToString("yyyyMMddHHmmssff") + ".pdf";
        //    string url = "http://" + Request.ServerVariables["Server_Name"] + ":" + Request.ServerVariables["Server_port"] + "/3-forleave/" + tmp_url;
        //  //  System.Diagnostics.Process.Start(pdflink1, " -s A4 -L 0 -R 0 -T 2 -B 0 " + url + " " + Server.MapPath(filename));
        //  //  System.Threading.Thread.Sleep(10000);
        //    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('" + url + "');", true);
        //}
 
        protected void btn_allrpt2_Click(object sender, EventArgs e)
        {
            string pdflink1 = System.Configuration.ConfigurationManager.AppSettings["pdflink"].ToString();

            string tmp_url = "C_overtime_allrptdoc1.aspx?pdf=1&ctype=" + rbl_print.SelectedValue + "&MZ_ID=" + txt_MZ_ID.Text.Trim() + "&PM_FION=" + TPM_FION +
                     "&Year=" + txt_Year.Text + "&Yearend=" + txt_endYear.Text +
                     "&AD=" + ddl_Search_AD.SelectedValue + "&UNIT=" + ddl_Search_Unit.SelectedValue;
            string filename = "/userfiles/" + DateTime.Now.ToString("yyyyMMddHHmmssff") + ".pdf";
            string url = "http://" + Request.ServerVariables["Server_Name"] + ":" + Request.ServerVariables["Server_port"] + "/3-forleave/" + tmp_url;
            //System.Diagnostics.Process.Start(pdflink1, " -s A4 -L 0 -R 0 -T 2 -B 0 -O Landscape  --zoom 1.2  " + url + " " + Server.MapPath(filename));
            System.Diagnostics.Process.Start(pdflink1, " -s A4 -L 0 -R 0 -T 2 -B 0 " + url + " " + Server.MapPath(filename));
            System.Threading.Thread.Sleep(8000);
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('" + filename + "');", true);
        }

        protected void btn_allrpt2_1_Click(object sender, EventArgs e)
        {
            string pdflink1 = System.Configuration.ConfigurationManager.AppSettings["pdflink"].ToString();

            string tmp_url = "C_overtime_allrpt2.aspx?pdf=0&ctype=" + rbl_print.SelectedValue + "&MZ_ID=" + txt_MZ_ID.Text.Trim() + "&PM_FION=" + TPM_FION +
                     "&Year=" + txt_Year.Text + "&Yearend=" + txt_endYear.Text +
                     "&AD=" + ddl_Search_AD.SelectedValue + "&UNIT=" + ddl_Search_Unit.SelectedValue;
           // string filename = "/userfiles/" + DateTime.Now.ToString("yyyyMMddHHmmssff") + ".pdf";
            string url = "http://" + Request.ServerVariables["Server_Name"] + ":" + Request.ServerVariables["Server_port"] + "/3-forleave/" + tmp_url;
          //  System.Diagnostics.Process.Start(pdflink1, " -s A4 -L 0 -R 0 -T 2 -B 0 " + url + " " + Server.MapPath(filename));
          //  System.Threading.Thread.Sleep(10000);
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.open('" + url + "');", true);
        }
    }
}