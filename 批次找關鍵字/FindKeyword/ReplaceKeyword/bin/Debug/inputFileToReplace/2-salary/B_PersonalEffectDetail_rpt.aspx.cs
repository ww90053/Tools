using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace TPPDDB._2_salary
{
    public partial class B_PersonalEffectDetail_rpt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Session["MZ_IDforSalary"] = "H125654513";
            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                Selectconn.Open();
                try
                {
                    string strSQL = "SELECT MZ_NAME, IDCARD, MZ_POLNO, AYEAR, MZ_OCCC, PAY, BOSS_AMONTH, SALARYPAY1, PROFESS, BOSS, EXTRA01, WORKP, TECHNICS, FAR, TAX, NOTE, TOTAL FROM B_EFFECT WHERE IDCARD = '" + Session["MZ_IDforSalary"] + "' AND AYEAR = '" + Request.QueryString["AYEAR"] + "'";
                    SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
                    DataTable rpt_dt = new DataTable();
                    rpt_dt.Load(Selectcmd.ExecuteReader());
                    if (rpt_dt.Rows.Count == 0)
                    {
                        if (Request.QueryString["from"] == "3")
                        {
                            Response.Redirect("B_SearchYearSalary3.aspx?chkType=2&statData=0&TPM_FION=" + Request.QueryString["TPM_FION"]);
                        }
                        else
                        {
                            Response.Redirect("B_SearchYearSalary.aspx?chkType=2&statData=0&TPM_FION=" + Request.QueryString["TPM_FION"]);
                        }
                    }
                    else
                    {
                        rpt_dt.Columns.Add("ADDSUM", typeof(double));
                        rpt_dt.Columns.Add("DESSUM", typeof(double));
                        rpt_dt.Columns.Add("PAY2", typeof(String));
                        rpt_dt.Columns.Add("BOSS_AMONTH2", typeof(String));

                        rpt_dt.Rows[0]["ADDSUM"] = Convert.ToInt32(rpt_dt.Rows[0]["SALARYPAY1"]) + Convert.ToInt32(rpt_dt.Rows[0]["PROFESS"]) + Convert.ToInt32(rpt_dt.Rows[0]["BOSS"]);
                        rpt_dt.Rows[0]["DESSUM"] = Convert.ToInt32(rpt_dt.Rows[0]["EXTRA01"]) + Convert.ToInt32(rpt_dt.Rows[0]["TAX"]);

                        rpt_dt.Rows[0]["PAY2"] = rpt_dt.Rows[0]["PAY"].ToString();
                        rpt_dt.Rows[0]["BOSS_AMONTH2"] = rpt_dt.Rows[0]["BOSS_AMONTH"].ToString();
                        rpt_dt.Rows[0]["MZ_OCCC"] = o_A_KTYPE.Find_Ktype_Cname(rpt_dt.Rows[0]["MZ_OCCC"].ToString(), "26");

                        Session["rpt_dt"] = rpt_dt;
                        string tmp_url = "B_rpt.aspx?fn=PersonalEffectDetail&TPM_FION=" + Request.QueryString["TPM_FION"];
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
                    }
                }
                catch (Exception) { throw; }
                finally { Selectconn.Close();
                //XX2013/06/18 
                Selectconn.Dispose();
                }
            }
        }
    }
}
