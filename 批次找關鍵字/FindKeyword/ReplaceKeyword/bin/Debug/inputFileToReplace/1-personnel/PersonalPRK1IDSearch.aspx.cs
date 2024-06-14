using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;


namespace TPPDDB._1_personnel
{
    public partial class WebForm5 : System.Web.UI.Page
    {
       
        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (!Page.IsPostBack)
            { 
                ///群組權限抓取
           ViewState["A_strGID"] = o_a_Function.strGID(Session["ADPMZ_ID"].ToString());
                A.set_Panel_EnterToTAB(ref this.Panel1);
                switch (ViewState["A_strGID"].ToString())
                {
                    case "A":
                    case "D":
                    case "E":
                        break;
                    case "B":
                        DropDownList_MZ_SWT3.SelectedValue = "1";

                        break;
                    case "C":
                        DropDownList_MZ_SWT3.SelectedValue = "2";
                        DropDownList_MZ_SWT3.Enabled = false;
                        break;
                    

                }
            }
        }

        protected void btLeave_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.close();", true);
        }

        protected void btOK_Click(object sender, EventArgs e)
        {
            string strPartSQL1 = "";

            switch (ViewState["A_strGID"].ToString())
            {
                case "A":
                case "B":
                case "D":  
                case "E":
                    break;
                case "C":
                    strPartSQL1 = " AND MZ_AD='" + Session["ADPMZ_EXAD"].ToString() + "'";
                    break;
                
            }

            string strPartSQL = "";

            string selectSQL = "SELECT COUNT(*) FROM A_PRKB WHERE 1=1";

            if (!string.IsNullOrEmpty(TextBox_MZ_NO1.Text.Trim()) && !string.IsNullOrEmpty(TextBox_MZ_NO2.Text.Trim()))
            {
                strPartSQL += " AND MZ_NO>='" + TextBox_MZ_NO1.Text.Trim() + "' AND MZ_NO<='" + TextBox_MZ_NO2.Text.Trim() + "'";
            }
            else if (!string.IsNullOrEmpty(TextBox_MZ_NO1.Text.Trim()))
            {
                strPartSQL += " AND MZ_NO='" + TextBox_MZ_NO1.Text.Trim() + "'";
            }

            strPartSQL += " AND MZ_SWT3='" + DropDownList_MZ_SWT3.SelectedValue.Trim() + "'";

            string totalCount = o_DBFactory.ABC_toTest.vExecSQL(selectSQL + strPartSQL);

            if (string.IsNullOrEmpty(totalCount))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('查無資料！請重新輸入案號');", true);
                return;
            }

            //新增獎懲類別欄位 20180329 by sky
            Session["PRK1_GV1"] = @" SELECT MZ_NO,MZ_PRK,MZ_PRCT,MZ_ID,MZ_NAME,
                                   C1.mz_kchi AS mz_ad ,C2.mz_kchi AS mz_unit ,C3.mz_kchi AS mz_occc ,C4.mz_kchi AS mz_srank ,C5.mz_kchi AS mz_prrst
                                   FROM A_PRKB  A
                                   LEFT JOIN A_KTYPE C1 ON C1.MZ_KTYPE = '04' AND A.MZ_AD = C1.MZ_KCODE
                                   LEFT JOIN A_KTYPE C2 ON C2.MZ_KTYPE = '25' AND A.MZ_UNIT = C2.MZ_KCODE
                                   LEFT JOIN A_KTYPE C3 ON C3.MZ_KTYPE = '26' AND A.MZ_OCCC = C3.MZ_KCODE
                                   LEFT JOIN A_KTYPE C4 ON C4.MZ_KTYPE = '09' AND A.MZ_SRANK = C4.MZ_KCODE
                                   LEFT JOIN A_KTYPE C5 ON C5.MZ_KTYPE = '24' AND A.MZ_PRRST = C5.MZ_KCODE
                                   WHERE 1=1 AND MZ_SWT4='Y' AND (MZ_SWT1!='Y' OR MZ_SWT1 IS NULL )" + strPartSQL + strPartSQL1;

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.opener.document.getElementById('" + Session["PRK1_BT"] + "').click();window.close();", true);


            //using (SqlConnection UpdateSWT1conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            //{
            //    UpdateSWT1conn.Open();
            //    SqlTransaction transaction = UpdateSWT1conn.BeginTransaction();

            //    string UpdateSWT1sql = "UPDATE A_PRKB SET MZ_SWT1='Y' WHERE 1=1 " + strPartSQL;
            //    SqlCommand UpdateSWT1cmd = new SqlCommand(UpdateSWT1sql, UpdateSWT1conn);
            //    UpdateSWT1cmd.CommandType = CommandType.Text;
            //    UpdateSWT1cmd.Transaction = transaction;

            //    string InsertPRK2sql = "INSERT INTO A_PRK2(MZ_NO,MZ_ID,MZ_NAME,MZ_AD,MZ_UNIT,MZ_OCCC,MZ_RANK,MZ_RANK1,MZ_SRANK," +
            //                                                  " MZ_TBDV,MZ_PRCT,MZ_PRK,MZ_POLK,MZ_PRRST,MZ_PRK1,MZ_MEMO,MZ_PROLNO,MZ_PCODE," +
            //                                                  " MZ_SWT3,MZ_REMARK,MZ_PRID,MZ_PRID1,MZ_CHKAD,MZ_DATE,MZ_IDATE)" +
            //                           " SELECT MZ_NO,MZ_ID,MZ_NAME,MZ_AD,MZ_UNIT,MZ_OCCC,MZ_RANK,MZ_RANK1,MZ_SRANK,MZ_TBDV,MZ_PRCT," +
            //                                                  " MZ_PRK,MZ_POLK,MZ_PRRST,MZ_PRK1,MZ_MEMO,MZ_PROLNO,MZ_PCODE,MZ_SWT3,MZ_REMARK,'"
            //                                                  + ViewState["MZ_PRID"].ToString() + "','" + ViewState["MZ_PRID1"].ToString() + "','"
            //                                                  + ViewState["MZ_CHKAD"].ToString() + "','" + ViewState["MZ_DATE"].ToString() + "','"
            //                                                  + ViewState["MZ_IDATE"].ToString() + "' FROM A_PRKB" +
            //                                                  " WHERE 1=1 " + strPartSQL + strPartSQL1;
            //    SqlCommand InsertPRKcmd = new SqlCommand(InsertPRK2sql, UpdateSWT1conn);
            //    InsertPRKcmd.CommandType = CommandType.Text;
            //    InsertPRKcmd.Transaction = transaction;
            //    try
            //    {
            //        UpdateSWT1cmd.ExecuteNonQuery();
            //        InsertPRKcmd.ExecuteNonQuery();
            //        transaction.Commit();
            //        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.alert('謄稿成功！');window.opener.location.href='Personal2-2.aspx?PRID=" + ViewState["MZ_PRID"] + "&PRID1=" + ViewState["MZ_PRID1"] + "&DATE=&TPM_FION=" + Request.QueryString["TPM_FION"] + "';window.close();", true);
            //    }
            //    catch
            //    {
            //        transaction.Rollback();
            //        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.alert('謄稿失敗！');", true);
            //    }
            //    finally
            //    {
            //        UpdateSWT1conn.Close();
            //    }
            //}
        }

        //protected void Button1_Click(object sender, EventArgs e)
        //{


        //    using (SqlConnection UpdateSWT1conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
        //    {

        //        UpdateSWT1conn.Open();
        //        SqlTransaction transaction = UpdateSWT1conn.BeginTransaction();
        //        for (int i = 0; i < GridView1.Rows.Count; i++)
        //        {

        //            string UpdateSWT1sql = "UPDATE A_PRKB SET MZ_SWT1='Y' WHERE MZ_ID='" + GridView1.Rows[i].Cells[3].Text.Trim() +
        //                                 "' AND MZ_NO='" + GridView1.Rows[i].Cells[1].Text.Trim() + "'";
        //            SqlCommand UpdateSWT1cmd = new SqlCommand(UpdateSWT1sql, UpdateSWT1conn);
        //            UpdateSWT1cmd.CommandType = CommandType.Text;
        //            UpdateSWT1cmd.Transaction = transaction;

        //            string InsertPRK2sql="INSERT INTO A_PRK2(MZ_NO,MZ_ID,MZ_NAME,MZ_AD,MZ_UNIT,MZ_OCCC,MZ_RANK,MZ_RANK1,MZ_SRANK,"+
        //                                                       " MZ_TBDV,MZ_PRCT,MZ_PRK,MZ_POLK,MZ_PRRST,MZ_PRK1,MZ_MEMO,MZ_PROLNO,MZ_PCODE,"+
        //                                                       " MZ_SWT3,MZ_REMARK,MZ_PRID,MZ_PRID1,MZ_CHKAD,MZ_DATE,MZ_IDATE)"+
        //                                " SELECT MZ_NO,MZ_ID,MZ_NAME,MZ_AD,MZ_UNIT,MZ_OCCC,MZ_RANK,MZ_RANK1,MZ_SRANK,MZ_TBDV,MZ_PRCT,"+
        //                                                       " MZ_PRK,MZ_POLK,MZ_PRRST,MZ_PRK1,MZ_MEMO,MZ_PROLNO,MZ_PCODE,MZ_SWT3,MZ_REMARK,'"
        //                                                       + ViewState["MZ_PRID"] + "','" + ViewState["MZ_PRID1"] + "','"
        //                                                       + ViewState["MZ_CHKAD"] + "','" + ViewState["MZ_DATE"] + "','"
        //                                                       + ViewState["MZ_IDATE"] + "' FROM A_PRKB" +
        //                                                       " WHERE MZ_NO='" + GridView1.Rows[i].Cells[1].Text.Trim() + 
        //                                                       "' AND MZ_ID='" + GridView1.Rows[i].Cells[3].Text.Trim() + "'";
        //            SqlCommand InsertPRKcmd = new SqlCommand(InsertPRK2sql, UpdateSWT1conn);
        //            InsertPRKcmd.CommandType = CommandType.Text;
        //            InsertPRKcmd.Transaction = transaction;
        //            try
        //            {
        //                if ((GridView1.Rows[i].FindControl("CheckBox1") as CheckBox).Checked)
        //                {
        //                   InsertPRKcmd.ExecuteNonQuery();
        //                   UpdateSWT1cmd.ExecuteNonQuery();
        //                }

        //                if (i == GridView1.Rows.Count-1)
        //                { 
        //                    transaction.Commit();
        //                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.alert('謄稿成功！');window.opener.location.href='Personal2-2.aspx?PRID=" + ViewState["MZ_PRID"] + "&PRID1=" + ViewState["MZ_PRID1"] + "&DATE=&TPM_FION=" + Request.QueryString["TPM_FION"] + "';window.close();", true);
        //                }

        //            }
        //            catch (Exception)
        //            {
        //                transaction.Rollback();
        //                throw;

        //            }
        //            finally
        //            {
        //                if (i == GridView1.Rows.Count-1)
        //                { 
        //                     UpdateSWT1conn.Close();
        //                }
        //            }
        //        }
        //    }
        //}

        //protected void Button2_Click(object sender, EventArgs e)
        //{
        //    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.close();", true);
        //}
    }
}
