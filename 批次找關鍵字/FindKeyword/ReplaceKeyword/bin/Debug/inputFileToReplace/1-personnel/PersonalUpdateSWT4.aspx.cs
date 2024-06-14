using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Data;


namespace TPPDDB._1_personnel
{
    public partial class PersonalUpdateSWT4 : System.Web.UI.Page
    {
        string A_strGID = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            A_strGID = o_a_Function.strGID(Session["ADPMZ_ID"].ToString());

            A.set_Panel_EnterToTAB(ref this.Panel1);

            ViewState["MZ_NO"] = Request["MZ_NO"].ToString();

            if (!IsPostBack)
            {
                TextBox_MZ_NO.Text = ViewState["MZ_NO"].ToString();
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string strSQL = "";

            string strSQL1 = "";

            switch (A_strGID)
            {
                case "A":
                    //20140113
                case "B":
                    //strSQL = "SELECT " +
                    //                   "MZ_SWT4,MZ_ID,MZ_NAME," +
                    //                   "(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE = '04' AND MZ_KCODE = MZ_AD)  AS MZ_AD," +
                    //                   "(SELECT MZ_KCHI  FROM A_KTYPE WHERE MZ_KTYPE = '25' AND MZ_KCODE =MZ_UNIT) AS MZ_UNIT," +
                    //                   "(SELECT MZ_KCHI  FROM A_KTYPE WHERE MZ_KTYPE = '26' AND MZ_KCODE =MZ_OCCC) AS MZ_OCCC," +
                    //                   "(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='09' AND MZ_KCODE=MZ_SRANK ) AS MZ_SRANK " +
                    //        "FROM " +
                    //                   "A_PRKB  " +
                    //        "WHERE  " +
                    //                   " MZ_NO='" + TextBox_MZ_NO.Text.Trim() +
                    //                   "' AND (MZ_SWT3='1' OR MZ_SWT3='2')";

                    //strSQL1 = " AND (MZ_SWT3='1' OR MZ_SWT3='2')";
                    strSQL = @"SELECT MZ_SWT4,MZ_ID,MZ_NAME,
                              AKD.MZ_KCHI MZ_AD ,  AKU.MZ_KCHI MZ_UNIT ,  AKO.MZ_KCHI MZ_OCCC , AKS.MZ_KCHI MZ_SRANK       
                              FROM A_PRKB  
                             LEFT JOIN A_KTYPE AKD ON RTRIM(AKD.MZ_KCODE)=RTRIM(A_PRKB.MZ_AD) AND RTRIM(AKD.MZ_KTYPE)='04' 
                             LEFT JOIN A_KTYPE AKU ON RTRIM(AKU.MZ_KCODE)=RTRIM(A_PRKB.MZ_UNIT) AND RTRIM(AKU.MZ_KTYPE)='25' 
                             LEFT JOIN A_KTYPE AKO ON RTRIM(AKO.MZ_KCODE)=RTRIM(A_PRKB.MZ_OCCC) AND RTRIM(AKO.MZ_KTYPE)='26' 
                             LEFT JOIN A_KTYPE AKS ON RTRIM(AKS.MZ_KCODE)=RTRIM(A_PRKB.MZ_SRANK) AND RTRIM(AKS.MZ_KTYPE)='09' 
                             WHERE  " +
                                        " MZ_NO='" + TextBox_MZ_NO.Text.Trim() +
                                        "' AND (MZ_SWT3='1' OR MZ_SWT3='2')";

                    strSQL1 = " AND (MZ_SWT3='1' OR MZ_SWT3='2')";

                    break;
                //case "B":
                //    strSQL = "SELECT " +
                //                         "MZ_SWT4,MZ_ID,MZ_NAME," +
                //                         "(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE = '04' AND MZ_KCODE = MZ_AD)  AS MZ_AD," +
                //                         "(SELECT MZ_KCHI  FROM A_KTYPE WHERE MZ_KTYPE = '25' AND MZ_KCODE =MZ_UNIT) AS MZ_UNIT," +
                //                         "(SELECT MZ_KCHI  FROM A_KTYPE WHERE MZ_KTYPE = '26' AND MZ_KCODE =MZ_OCCC) AS MZ_OCCC," +
                //                         "(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='09' AND MZ_KCODE=MZ_SRANK ) AS MZ_SRANK " +
                //              "FROM " +
                //                         "A_PRKB  " +
                //              "WHERE  " +
                //                         " MZ_NO='" + TextBox_MZ_NO.Text.Trim() +
                //                         "' AND (MZ_SWT3='1' OR MZ_SWT3='2')";

                //    strSQL1 = " AND (MZ_SWT3='1' OR MZ_SWT3='2')";


                //    break;
                case "C":

                    string strSQL2 = "WHERE  " +
                                         "      MZ_AD='" + Session["ADPMZ_EXAD"].ToString() +
                                         "' AND MZ_NO='" + TextBox_MZ_NO.Text.Trim() +
                                         "' AND MZ_SWT3='2'";

                   

                    //string CHECKSTRING = o_DBFactory.ABC_toTest.vExecSQL("SELECT SWT4_USER FROM A_PRKB " + strSQL2 + " AND ROWNUM=1");

                    //if (!string.IsNullOrEmpty(CHECKSTRING))
                    //{
                    //    if (o_a_Function.strGID(CHECKSTRING) == "B" || o_a_Function.strGID(CHECKSTRING) == "A")
                    //    {
                    //        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.alert('無審核權限！');window.close();", true);
                    //        return;
                    //    }
                    //}


                    //strSQL = "SELECT " +
                    //                     "MZ_SWT4,MZ_ID,MZ_NAME," +
                    //                     "(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE = '04' AND MZ_KCODE = MZ_AD)  AS MZ_AD," +
                    //                     "(SELECT MZ_KCHI  FROM A_KTYPE WHERE MZ_KTYPE = '25' AND MZ_KCODE =MZ_UNIT) AS MZ_UNIT," +
                    //                     "(SELECT MZ_KCHI  FROM A_KTYPE WHERE MZ_KTYPE = '26' AND MZ_KCODE =MZ_OCCC) AS MZ_OCCC," +
                    //                     "(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='09' AND MZ_KCODE=MZ_SRANK ) AS MZ_SRANK " +
                    //          "FROM " +
                    //                     "A_PRKB  " + strSQL2;

                    strSQL = @"SELECT MZ_SWT4,MZ_ID,MZ_NAME, 
                             AKD.MZ_KCHI MZ_AD ,  AKU.MZ_KCHI MZ_UNIT ,  AKO.MZ_KCHI MZ_OCCC , AKS.MZ_KCHI MZ_SRANK  
                                         
                              FROM A_PRKB  
                             LEFT JOIN A_KTYPE AKD ON RTRIM(AKD.MZ_KCODE)=RTRIM(A_PRKB.MZ_AD) AND RTRIM(AKD.MZ_KTYPE)='04' 
                             LEFT JOIN A_KTYPE AKU ON RTRIM(AKU.MZ_KCODE)=RTRIM(A_PRKB.MZ_UNIT) AND RTRIM(AKU.MZ_KTYPE)='25' 
                             LEFT JOIN A_KTYPE AKO ON RTRIM(AKO.MZ_KCODE)=RTRIM(A_PRKB.MZ_OCCC) AND RTRIM(AKO.MZ_KTYPE)='26' 
                             LEFT JOIN A_KTYPE AKS ON RTRIM(AKS.MZ_KCODE)=RTRIM(A_PRKB.MZ_SRANK) AND RTRIM(AKS.MZ_KTYPE)='09' " +
                                         
                                         
                                         strSQL2;


                    strSQL1 = " AND MZ_SWT3='2'";
                    break;
                case "D":

                    break;
                case "E":

                    break;
            }
            if (RadioButtonList1.SelectedIndex == 0)
            {
                SqlDataSource1.SelectParameters.Clear();
                SqlDataSource1.SelectCommand = strSQL;
                GridView1.DataBind();

                if (GridView1.Rows.Count == 0)
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.alert('查無資料！');", true);
                }
                else
                {
                    GridView1.Visible = true;
                    Panel1.Visible = false;
                    Button2.Visible = true;
                    Button4.Visible = true;
                }

            }
            else
            {
                if (string.IsNullOrEmpty(o_DBFactory.ABC_toTest.vExecSQL("SELECT COUNT(*) FROM A_PRKB WHERE RTRIM(MZ_NO)='" + TextBox_MZ_NO.Text.Trim() + "'" + strSQL1)))
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.alert('查無資料！');", true);
                    return;
                }

                using (SqlConnection UpdateSWT4conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
                {
                    UpdateSWT4conn.Open();

                    string UpdateSWT4sql = "";

                    if (RadioButtonList2.SelectedIndex == 0)
                    {
                        UpdateSWT4sql = "UPDATE " +
                                                   "A_PRKB " +
                                        "SET " +
                                                   "MZ_SWT4='Y',SWT4_USER='" + Session["ADPMZ_ID"].ToString() +
                                                   "',SWT4_DATE='" + Request["SWT4_DATE"].ToString() + "' " +
                                        "WHERE " +
                                                   "MZ_NO='" + TextBox_MZ_NO.Text.Trim() + "'  AND (MZ_SWT1='N' OR MZ_SWT1 IS NULL) " + strSQL1;
                    }
                    else
                    {
                        UpdateSWT4sql = "UPDATE " +
                                                   "A_PRKB " +
                                        "SET " +
                                                   "MZ_SWT4='N',SWT4_USER='" + Session["ADPMZ_ID"].ToString() +
                                                   "',SWT4_DATE='" + Request["SWT4_DATE"].ToString() + "' " +
                                        "WHERE  " +
                                                   "MZ_NO='" + TextBox_MZ_NO.Text.Trim() + "' AND (MZ_SWT1='N' OR MZ_SWT1 IS NULL) " + strSQL1;
                    }

                    SqlCommand UpdateSWT4cmd = new SqlCommand(UpdateSWT4sql, UpdateSWT4conn);
                    try
                    {
                        UpdateSWT4cmd.ExecuteNonQuery();
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.alert('審核成功！');window.opener.location.href='Personal2-1.aspx?Personl_PRKB_SWT4_MZ_NO=" + TextBox_MZ_NO.Text.ToString() + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "';window.close();", true);
                    }
                    catch
                    {

                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.alert('審核失敗！');window.close();", true);
                    }
                    finally
                    {
                        UpdateSWT4conn.Close();

                        //XX2013/06/18 
                        UpdateSWT4conn.Dispose();
                    }
                }
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            using (SqlConnection UpdateSWT4conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            {
                UpdateSWT4conn.Open();
                SqlTransaction transaction = UpdateSWT4conn.BeginTransaction();
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    string UpdateSWT4sql = "";

                    if (RadioButtonList2.SelectedIndex == 0)
                    {
                        UpdateSWT4sql = "UPDATE " +
                                                   "A_PRKB " +
                                        "SET " +
                                                   "MZ_SWT4='Y',SWT4_USER='" + Session["ADPMZ_ID"].ToString() +
                                                   "',SWT4_DATE='" + Request["SWT4_DATE"].ToString() + "' " +
                                        "WHERE " +
                                                   "MZ_ID='" + GridView1.DataKeys[i].Value +
                                                   "' AND MZ_NO='" + TextBox_MZ_NO.Text.Trim() +
                                                   "' AND (MZ_SWT1='N' )";
                    }
                    else
                    {
                        UpdateSWT4sql = "UPDATE " +
                                                   "A_PRKB " +
                                        "SET " +
                                                   "MZ_SWT4='N',SWT4_USER='" + Session["ADPMZ_ID"].ToString() +
                                                   "',SWT4_DATE='" + Request["SWT4_DATE"].ToString() + "' " +
                                        "WHERE " +
                                                   "MZ_ID='" + GridView1.DataKeys[i].Value +
                                                   "' AND MZ_NO='" + TextBox_MZ_NO.Text.Trim() +
                                                   "' AND (MZ_SWT1='N' )";
                    }

                    SqlCommand UpdateSWT4cmd = new SqlCommand(UpdateSWT4sql, UpdateSWT4conn);

                    UpdateSWT4cmd.Transaction = transaction;

                    try
                    {
                        if ((GridView1.Rows[i].FindControl("CheckBox_SWT4") as CheckBox).Checked)
                        {
                            UpdateSWT4cmd.ExecuteNonQuery();
                        }

                        if (i == GridView1.Rows.Count - 1)
                        {
                            transaction.Commit();

                            UpdateSWT4cmd.Dispose();
                            UpdateSWT4conn.Close();
                            UpdateSWT4conn.Dispose();

                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.alert('審核成功！');window.opener.location.href='Personal2-1.aspx?Personl_PRKB_SWT4_MZ_NO=" + TextBox_MZ_NO.Text.ToString() + "&TPM_FION=" + Request.QueryString["TPM_FION"] + "';window.close();", true);
                        }
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                    finally
                    {
                        if (i == GridView1.Rows.Count - 1)
                        {
                            UpdateSWT4cmd.Dispose();
                            UpdateSWT4conn.Close();
                            UpdateSWT4conn.Dispose();
                        }
                    }
                }
            }
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.close()", true);
        }

        protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RadioButtonList1.SelectedIndex == 0)
            {
                RadioButtonList1.Items[0].Selected = true;
                RadioButtonList1.Items[1].Selected = false;
            }
            else
            {
                RadioButtonList1.Items[0].Selected = false;
                RadioButtonList1.Items[2].Selected = true;
            }
        }
    }
}
