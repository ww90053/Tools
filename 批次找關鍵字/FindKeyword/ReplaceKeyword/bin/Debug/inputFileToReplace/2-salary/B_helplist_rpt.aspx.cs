using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace TPPDDB._2_salary
{
    public partial class B_helplist_rpt : System.Web.UI.Page
    {
        string strSQL;
        DataTable temp = new DataTable();
        DataTable rpt_dt;
        //int TPM_FION;
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                SalaryPublic.checkPermission();
                SalaryPublic.fillDropDownList(ref this.DropDownList_AD);
            }
        }

        private String strAD
        {
            get
            {
                return DropDownList_AD.SelectedValue;
            }
        }

        private string strAYEAR
        {
            get
            {
                return TextBox_YEAR.Text;
            }
        }

        private string strUNIT
        {
            get
            {
                return DropDownList_UNIT.SelectedValue;
            }
        }

        protected void Button_SEARCH_Click(object sender, EventArgs e)
        {
            rpt_dt = rpt_dt_init();
            //發薪機關及編制單位下的所有人。
            int num = 1;
            strSQL = string.Format("SELECT ROWNUM NUM,MZ_OCCC, MZ_NAME,MONTHPAY,PATCH,SOLE,YEARPAY,EFFECT,(MONTHPAY+PATCH+SOLE+YEARPAY+EFFECT) ADD_SUM,MONTHPAYTAX,PATCHTAX,SOLETAX,YEARPAYTAX,EFFECTTAX,(MONTHPAYTAX+PATCHTAX+SOLETAX+YEARPAYTAX+EFFECTTAX) DES_SUM FROM B_TAXES WHERE BMM.MZ_UNIT='{0}' AND AYEAR='{1}' AND PAY_AD='{2}'",
                                   strUNIT, strAYEAR, strAD);
            DataTable dt = new DataTable();
            dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
            if (dt.Rows.Count > 0)
            {
                int page = 1;
                foreach (DataRow dr in dt.Rows)
                {
                    DataRow dr1 = rpt_dt.NewRow();
                    dr1[0] = num;//流水編號
                    num++;
                    dr1[1] = o_A_KTYPE.CODE_TO_NAME(dr["MZ_OCCC"].ToString(), "26");
                    dr1[2] = dr["MZ_NAME"];
                    dr1[3] = dr["MONTHPAY"];
                    dr1[4] = dr["PATCH"];
                    dr1[5] = dr["YEARPAY"];
                    dr1[6] = dr["EFFECT"];
                    dr1[7] = dr["SOLE"];
                    dr1[8] = dr["ADD_SUM"];
                    dr1[9] = dr["MONTHPAYTAX"];
                    dr1[10] = dr["PATCHTAX"];
                    dr1[11] = dr["YEARPAYTAX"];
                    dr1[12] = dr["EFFECTTAX"];
                    dr1[13] = dr["SOLETAX"];
                    dr1[14] = dr["DES_SUM"];
                    if (page % 30 == 0)
                    {
                        dr1[15] = page;
                        page++;
                    }
                    else
                    {
                        dr1[15] = page;
                    }
                }
            }

            for (int i = 0; i < 14; i++)
            {
                DataRow dr = rpt_dt.NewRow();
                dr["num"] = num;
                num++;
                rpt_dt.Rows.Add(dr);
            }
            //GridView1.DataSource = rpt_dt;
            //GridView1.DataBind();
            Session["rpt_dt"] = rpt_dt;
            Session["TITLE"] = o_A_KTYPE.CODE_TO_NAME(strAD, "04") + " " + o_A_KTYPE.CODE_TO_NAME(strUNIT, "25") + "                    員工發給" + strAYEAR + "年  考績獎金印領清冊";
            string tmp_url = "B_rpt.aspx?fn=monthpay_detail&TPM_FION=" + Request["TPM_FION"];  //+ TPM_FION;
            ScriptManager.RegisterClientScriptBlock(Panel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
        }

        protected DataTable rpt_dt_init()
        {
            rpt_dt = new DataTable();
            rpt_dt.Columns.Add("NUM", typeof(string));
            rpt_dt.Columns.Add("MZ_OCCC", typeof(string));
            rpt_dt.Columns.Add("MZ_NAME", typeof(string));
            rpt_dt.Columns.Add("MONTHPAY", typeof(int));
            rpt_dt.Columns.Add("PATCH", typeof(int));
            rpt_dt.Columns.Add("YEARPAY", typeof(int));
            rpt_dt.Columns.Add("EFFECT", typeof(int));
            rpt_dt.Columns.Add("SOLE", typeof(int));
            rpt_dt.Columns.Add("ADD_SUM", typeof(int));
            rpt_dt.Columns.Add("MONTHPAYTAX", typeof(int));
            rpt_dt.Columns.Add("PATCHTAX", typeof(int));
            rpt_dt.Columns.Add("EFFECTTAX", typeof(int));
            rpt_dt.Columns.Add("SOLETAX", typeof(int));
            rpt_dt.Columns.Add("DES_SUM", typeof(int));
            rpt_dt.Columns.Add("PAGE", typeof(int));
            return rpt_dt;
        }

        protected void DropDownList_AD_SelectedIndexChanged(object sender, EventArgs e)
        {
            strSQL = string.Format("SELECT MZ_KCODE,MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='25' AND MZ_KCODE IN (SELECT MZ_UNIT FROM A_UNIT_AD WHERE MZ_AD='{0}')", strAD);
            temp = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
            DropDownList_UNIT.DataSource = temp;
            DropDownList_UNIT.DataTextField = "MZ_KCHI";
            DropDownList_UNIT.DataValueField = "MZ_KCODE";
            DropDownList_UNIT.DataBind();
            DropDownList_UNIT.Items.Insert(0, "請選擇");
        }

        protected void Button_CANCEL_Click(object sender, EventArgs e)
        {
            Response.Redirect("B_taxlist_rpt.aspx");
        }
    }
}

