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
    public partial class B_SalaryBasicDetail : System.Web.UI.Page
    {
        SalaryPublic SP = new SalaryPublic();
        
        protected void Page_Load(object sender, EventArgs e)
        {
           

            if (!IsPostBack)
            { SalaryPublic.checkPermission();
                SalaryPublic.fillDropDownList(ref this.DropDownList_PAY_AD);
                Label_MSG.Visible = false;
            }
        }

        protected void btSave_Click(object sender, EventArgs e)
        {
            SendToReport();
        }

        //---------------送出DataTable到報表-----------------
        protected void SendToReport()
        {
            string strSQL;
           // string strSRANK, strMZ_OCCC, strMZ_SPT, strINSURANCE_GROUP;
           // string strSALARY_NAME1 = "1";
            string strDate;
            //int intS_SNID,intSALARY_PAY;

            //int intSalaryOriginType;

            //20140701
            //SalaryPublic SP = new SalaryPublic();
            //strDate = SP.getNowYearMonth();
            strDate = SalaryPublic.strRepublicYearMonth();         

            DataTable rpt_dt = initrptdt();

            List<String> lsSQL = new List<string>();
            if (TextBox_MZ_POLNO.Text.Length > 0)
            {
                lsSQL.Add(" MZ_POLNO = '" + TextBox_MZ_POLNO.Text + "'");
            }
            if (TextBox_MZ_ID.Text.Length > 0)
            {
                lsSQL.Add(" MZ_ID = '" + TextBox_MZ_ID.Text + "'");
            }
            if (TextBox_MZ_NAME.Text.Length > 0)
            {
                lsSQL.Add(" MZ_NAME = '" + TextBox_MZ_NAME.Text + "'");
            }

          

            strSQL = string.Format("select * from VW_SALARY_BASIC_DETAIL WHERE PAY_AD = '{0}' {1}", DropDownList_PAY_AD.SelectedValue.ToString(), lsSQL.Count > 0 ? " AND " + String.Join(" AND ", lsSQL.ToArray()) : String.Empty);

            rpt_dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "SalaryBasic");

            //foreach (DataRow dr in rpt_dt.Rows)
            //{
                //string strIDCard = dr["MZ_ID"].ToString();

                #region 薪資帳戶

                //strSQL = "SELECT BANK_NAME,\"GROUP\" FROM B_BASE_STOCKPILE,B_BANK_LIST WHERE BANKID=BANK_ID AND IDCARD = '" + strIDCard + "'";

                //DataTable dt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "GET");
                //if (dt.Rows.Count > 0)
                //{
                //    foreach (DataRow dr2 in dt.Rows)
                //    {
                //        dr["BANK_NAME"] = dr2["BANK_NAME"];
                //        switch (dr2["GROUP"].ToString())
                //        {
                //            case "1":
                //                dr["PDEPOSIT"] = dr2["BANK_NAME"];
                //                break;
                //            case "2":
                //                dr["BANK_NAME"] = dr2["BANK_NAME"];
                //                break;
                //            case "3":
                //                dr["COUNTRYHOUSE"] = dr2["BANK_NAME"];
                //                break;
                //            case "4":
                //                dr["RETREIVE"] = dr2["BANK_NAME"];
                //                break;
                //            case "5":
                //                dr["INSTALLMENT"] = dr2["BANK_NAME"];
                //                break;
                //            case "6":
                //                dr["BANKLOAN"] = dr2["BANK_NAME"];
                //                break;
                //            case "7":
                //                dr["COURT"] = dr2["BANK_NAME"];
                //                break;
                //        }
                //    }
                //}

                #endregion

                #region 其他所得

                //int otherpaym = 0;
                //int otherpayp = 0;

                //strSQL = "SELECT PAY,MP FROM B_BASE_OTHER INNER JOIN B_SALARY_2 ON S_SNID=ID WHERE IDCARD ='" + strIDCard + "'";
                //DataTable otherdt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "OTHERPAY");

                //foreach (DataRow otherdr in otherdt.Rows)
                //{
                //    switch (otherdr["MP"].ToString())
                //    {
                //        case "M":
                //            otherpaym += Convert.ToInt32(otherdr["PAY"]);
                //            break;
                //        case "P":
                //            otherpayp += Convert.ToInt32(otherdr["PAY"]);
                //            break;
                //        default:
                //            break;
                //    }
                //}

                //dr["OTHERPAYM"] = otherpaym;
                //dr["OTHERPAYP"] = otherpayp;
                //dr["PAYSUM"] = otherpayp + Convert.ToInt32(dr["FARPAY"]) + Convert.ToInt32(dr["BONUSPAY"]) + Convert.ToInt32(dr["ADVENTIVEPAY"]) + Convert.ToInt32(dr["PROFESSPAY"]) + Convert.ToInt32(dr["BOSSPAY"]) + Convert.ToInt32(dr["TECHNICSPAY"]) + Convert.ToInt32(dr["WORKPPAY"]) + Convert.ToInt32(dr["ELECTRICPAY"]);
                //dr["COSTSUM"] = otherpaym + Convert.ToInt32(dr["CONCUR3PAY"]) + Convert.ToInt32(dr["INSURANCEPAY"]) + Convert.ToInt32(dr["DE_HEALTH"]) + Convert.ToInt32(dr["MONTHPAY_TAX"]) + Convert.ToInt32(dr["MONTHPAY"]) + Convert.ToInt32(dr["TAXPER"]) + Convert.ToInt32(dr["EXTRA01"]) + Convert.ToInt32(dr["EXTRA02"]) + Convert.ToInt32(dr["EXTRA03"]) + Convert.ToInt32(dr["EXTRA04"]) + Convert.ToInt32(dr["EXTRA05"]) + Convert.ToInt32(dr["EXTRA06"]) + Convert.ToInt32(dr["EXTRA07"]) + Convert.ToInt32(dr["EXTRA08"]) + Convert.ToInt32(dr["EXTRA09"]) + int.Parse(dr["HEALPER"].ToString());
                //dr["SENIORITYPAY"] = SalaryPublic.intRound_HEALTH_PAY_CHK_Data(SalaryPublic.intRound_HEALTH_PAY_Data(Convert.ToInt32(dr["PAYSUM"])), 1);
                //dr["OTHER1"] = 0;
                //dr["OTHER2"] = 0;
                //dr["OTHER3"] = 0;

                #endregion
            //}

            Session["rpt_dt"] = rpt_dt;
            string tmp_url = "B_rpt.aspx?fn=SalaryBasicDetail&TPM_FION=" + Request.QueryString["TPM_FION"];
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "go_print('" + tmp_url + "');", true);
        }

        //------------------初始化送至報表的DataTable----------------
        protected DataTable initrptdt()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MZ_POLNO", typeof(string));
            dt.Columns.Add("MZ_NAME", typeof(string));
            dt.Columns.Add("MZ_ID", typeof(string));
            dt.Columns.Add("MZ_SEX", typeof(string));
            dt.Columns.Add("ODDATE", typeof(int));
            dt.Columns.Add("MZ_SLVC", typeof(string));
            dt.Columns.Add("MZ_SPT", typeof(string));
            dt.Columns.Add("MZ_AD", typeof(string));
            dt.Columns.Add("MZ_UNIT", typeof(string));
            dt.Columns.Add("MZ_SRANK", typeof(string));
            dt.Columns.Add("MZ_POSIND", typeof(string));
            dt.Columns.Add("MZ_PCHIEFDATE", typeof(string));
            dt.Columns.Add("SENIORITYPAY", typeof(int));
            dt.Columns.Add("MZ_FDATE", typeof(string));
            dt.Columns.Add("MZ_ADATE", typeof(string));
            dt.Columns.Add("MZ_LDATE", typeof(string));
            dt.Columns.Add("MZ_ODATE", typeof(string));
            dt.Columns.Add("MZ_TDATE", typeof(string));
            dt.Columns.Add("MZ_NREA", typeof(string));
            dt.Columns.Add("NOTE", typeof(string));
            dt.Columns.Add("MZ_MOVETEL", typeof(string));
            dt.Columns.Add("MZ_PHONE", typeof(string));
            dt.Columns.Add("MZ_ADD1", typeof(string));
            dt.Columns.Add("MZ_ADD2", typeof(string));
            dt.Columns.Add("MZ_OCCC", typeof(string));
            dt.Columns.Add("PROFESS_PAY", typeof(int));
            dt.Columns.Add("BOSS_PAY", typeof(int));
            dt.Columns.Add("TECHNICS", typeof(int));
            dt.Columns.Add("BONUS", typeof(int));
            dt.Columns.Add("WORKP", typeof(int));
            dt.Columns.Add("ADVENTIVE", typeof(int));
            dt.Columns.Add("FAR", typeof(int));
            dt.Columns.Add("ELECTRIC", typeof(int));
            dt.Columns.Add("OTHERPAYP", typeof(int));
            dt.Columns.Add("CONCUR3", typeof(int));
            dt.Columns.Add("TAXPER", typeof(int));
            dt.Columns.Add("HEALPER", typeof(int));
            dt.Columns.Add("DE_HEALTH", typeof(int));
            dt.Columns.Add("MONTHPAY_TAX", typeof(int));
            dt.Columns.Add("MONTHPAY", typeof(int));
            dt.Columns.Add("EXTRA01", typeof(int));
            dt.Columns.Add("EXTRA02", typeof(int));
            dt.Columns.Add("EXTRA03", typeof(int));
            dt.Columns.Add("EXTRA04", typeof(int));
            dt.Columns.Add("EXTRA05", typeof(int));
            dt.Columns.Add("EXTRA06", typeof(int));
            dt.Columns.Add("EXTRA07", typeof(int));
            dt.Columns.Add("EXTRA08", typeof(int));
            dt.Columns.Add("EXTRA09", typeof(int));
            dt.Columns.Add("INSURANCE_PAY", typeof(int));
            dt.Columns.Add("OTHERPAYM", typeof(int));
            dt.Columns.Add("PAYSUM", typeof(int));
            dt.Columns.Add("COSTSUM", typeof(int));
            dt.Columns.Add("SALARYPAY", typeof(int));
            dt.Columns.Add("PDEPOSIT", typeof(string));
            dt.Columns.Add("BANK_NAME", typeof(string));
            dt.Columns.Add("COUNTRYHOUSE", typeof(string));
            dt.Columns.Add("RETREIVE", typeof(string));
            dt.Columns.Add("INSTALLMENT", typeof(string));
            dt.Columns.Add("BANKLOAN", typeof(string));
            dt.Columns.Add("COURT", typeof(string));
            dt.Columns.Add("OTHER1", typeof(int));
            dt.Columns.Add("OTHER2", typeof(int));
            dt.Columns.Add("OTHER3", typeof(int));
            return dt;
        }

        protected string[] GetFormula(string FIdStr, string strIDCARD)
        {
            string[] result = new string[2];
            try
            {
                using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
                {
                    string strOrg = "";
                    string cashStr = "";
                    Selectconn.Open();
                    //string strSQL = "SELECT B.I_NAME,A.* FROM B_FORMULA_DETAIL A  LEFT JOIN B_FORMULA_ITEM B ON dbo.TO_CHAR(B.ID)=A.FORMULA AND A.G_TYPE='TABLE' WHERE A.ID = " + strID + " ORDER BY SEQ";
                    string strSQL = "SELECT B.I_NAME,I_FROM_TB,I_FROM_CL,I_TARGET_TB,I_TARGET_CL,I_PAY,I_IDENT_ID,A.* FROM B_FORMULA_DETAIL A  LEFT JOIN B_FORMULA_ITEM B ON dbo.TO_CHAR(B.ID)=A.FORMULA AND A.G_TYPE='TABLE' WHERE A.ID = " + FIdStr + " ORDER BY SEQ";
                    SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);

                    DataTable dt = new DataTable();
                    dt.Load(Selectcmd.ExecuteReader());
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            string tmp = dr["G_TYPE"].ToString();
                            if (tmp == "TABLE")
                            {
                                strOrg += dr["I_NAME"].ToString();
                                cashStr += TransToPay(dr["I_FROM_TB"].ToString(), dr["I_FROM_CL"].ToString(), dr["I_TARGET_TB"].ToString(), dr["I_TARGET_CL"].ToString(), dr["I_PAY"].ToString(), dr["I_IDENT_ID"].ToString(), strIDCARD);

                            }
                            else
                            {
                                strOrg += dr["FORMULA"].ToString().Trim();
                                cashStr += dr["FORMULA"].ToString().Trim();
                            }
                        }
                    }

                    Selectconn.Close();
                    //XX2013/06/18 
                    Selectconn.Dispose();
                    result[0] = strOrg;
                    result[1] = cashStr;
                }
            }
            catch
            {
                result[0] = "";
                result[1] = "";

            }
            return result;
        }

        protected string TransToPay(string f_tb, string f_cl, string t_tb, string t_cl, string paycl, string idtID, string strIDCARD)
        {
            string sqlStr = "";
            if (t_tb == "A_DLBASE")
            {
                sqlStr = "SELECT A." + paycl + " FROM " + f_tb + " A," + t_tb + " B" + " WHERE A." + f_cl + " = B." + t_cl + "+'01D' AND B." + idtID + " = '" + strIDCARD + "'";
            }
            else
            {
                sqlStr = "SELECT A." + paycl + " FROM " + f_tb + " A," + t_tb + " B" + " WHERE A." + f_cl + " = B." + t_cl + " AND B." + idtID + " = '" + strIDCARD + "'";
            }
            string payVal = "";
            //select * from B_BASE A,B_TECHNICS B where B.ID=A.Technics
            using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                try
                {
                    Selectconn.Open();
                    SqlCommand Selectcmd = new SqlCommand(sqlStr, Selectconn);
                    SqlDataReader dr = Selectcmd.ExecuteReader(CommandBehavior.SingleRow);
                    if (dr.Read())
                    {
                        payVal = dr[0].ToString();
                    }
                    else
                    {
                        payVal = "0";
                    }
                }
                catch
                {
                    payVal = "0";
                }
                finally
                {
                    Selectconn.Close();
                    //XX2013/06/18 
                    Selectconn.Dispose();
                }
            }
            return payVal;
        }
    }
}