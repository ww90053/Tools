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
    public partial class PersonalPOSITIDSearch : System.Web.UI.Page
    {

        
        protected void Page_Load(object sender, EventArgs e)
        {
          
            ViewState["MZ_PRID"] = Request["MZ_PRID"];
            ViewState["MZ_PRID1"] = Request["MZ_PRID1"];
            ViewState["MZ_DATE"] = Request["MZ_DATE"];
            ViewState["MZ_IDATE"] = Request["MZ_IDATE"];
            ViewState["MZ_ADATE"] = Request["MZ_ADATE"];
            ViewState["MZ_AD"] = Request["MZ_AD"];
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string strPartSQL1 = "";
            //權限
            switch (o_a_Function.strGID(Session["ADPMZ_ID"].ToString()))
            {
                case "A":
                case "B":
                case "D":
                case "E":
                    break;
                case "C":
                    strPartSQL1 = " AND MZ_CHKAD='" + Session["ADPMZ_EXAD"].ToString() + "'";
                    break;
                

            }

            string strPartSQL = "";

            string selectSQL = "SELECT COUNT(*) FROM A_POSIT WHERE 1=1";

            if (!string.IsNullOrEmpty(TextBox_MZ_NO1.Text.Trim()) && !string.IsNullOrEmpty(TextBox_MZ_NO2.Text.Trim()))
            {
                strPartSQL += "AND MZ_NO>='" + o_str.tosql(TextBox_MZ_NO1.Text.Trim()) + "' AND MZ_NO<='" + o_str.tosql(TextBox_MZ_NO2.Text.Trim()) + "'";
            }
            else if (!string.IsNullOrEmpty(TextBox_MZ_NO1.Text.Trim()))
            {
                strPartSQL += "AND MZ_NO='" + o_str.tosql(TextBox_MZ_NO1.Text.Trim()) + "'";
            }

            string totalCount = o_DBFactory.ABC_toTest.vExecSQL(selectSQL + strPartSQL);

            if (string.IsNullOrEmpty(totalCount))
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert('查無資料！請重新輸入案號');", true);
                return;
            }

           
            Session["POSIT1_GV1"] = @"  SELECT MZ_NO,MZ_ID,MZ_NAME,
                                     AKD.MZ_KCHI MZ_AD ,  AKU.MZ_KCHI MZ_UNIT ,  AKO.MZ_KCHI MZ_OCCC , AKS.MZ_KCHI MZ_SRANK  
                                    FROM A_POSIT 
LEFT JOIN A_KTYPE AKD ON RTRIM(AKD.MZ_KCODE)=RTRIM(A_POSIT.MZ_AD) AND RTRIM(AKD.MZ_KTYPE)='04' 
LEFT JOIN A_KTYPE AKU ON RTRIM(AKU.MZ_KCODE)=RTRIM(A_POSIT.MZ_UNIT) AND RTRIM(AKU.MZ_KTYPE)='25' 
LEFT JOIN A_KTYPE AKO ON RTRIM(AKO.MZ_KCODE)=RTRIM(A_POSIT.MZ_OCCC) AND RTRIM(AKO.MZ_KTYPE)='26' 
LEFT JOIN A_KTYPE AKS ON RTRIM(AKS.MZ_KCODE)=RTRIM(A_POSIT.MZ_SRANK) AND RTRIM(AKS.MZ_KTYPE)='09' 
                                     WHERE 1=1  AND MZ_SWT4='Y' AND (MZ_SWT1!='Y' )" + strPartSQL + strPartSQL1;


            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.opener.document.getElementById('" + Session["POSIT1_BT"] + "').click();window.close();", true);


            //using (SqlConnection UpdateSWT1conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
            //{
            //    UpdateSWT1conn.Open();
            //    string sysday = (DateTime.Now.Year - 1911).ToString().PadLeft(3, '0') + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0');
            //    SqlTransaction transaction = UpdateSWT1conn.BeginTransaction();
            //    string UpdateSWT1sql = "UPDATE A_POSIT SET MZ_SWT1='Y' WHERE 1=1 " + strPartSQL;
            //    SqlCommand UpdateSWT1cmd = new SqlCommand(UpdateSWT1sql, UpdateSWT1conn);
            //    UpdateSWT1cmd.CommandType = CommandType.Text;
            //    UpdateSWT1cmd.Transaction = transaction;

            //    string InsertPOSIT2_Part;

            //    if (ViewState["MZ_AD"].ToString() == "38213800C")
            //    {
            //        InsertPOSIT2_Part = "INSERT INTO A_POSIT2(MZ_NO,MZ_ID,MZ_NAME,MZ_CHKAD,MZ_EXOAD,MZ_EXOUNIT,MZ_EXOOCC," +
            //                                              "  MZ_EXORANK,MZ_EXRANK1,MZ_EXOPOS,MZ_EXOPCHIEF,MZ_TBNREA," +
            //                                              "  MZ_TBDATE,MZ_TBID,MZ_EXOPNO1,MZ_EXOPNO,MZ_AD,MZ_UNIT,MZ_OCCC,MZ_RANK,MZ_RANK1," +
            //                                              "  MZ_CHISI,MZ_POSIND,MZ_NIN,MZ_NREA,MZ_TBDV,MZ_PCHIEF,MZ_PESN," +
            //                                              "  MZ_PNO,MZ_TNO,MZ_WDATE,MZ_SRANK,MZ_SLVC,MZ_SPT,MZ_SPT1,MZ_MEMO," +
            //                                              "  MZ_RET,MZ_REMARK,MZ_NRT,OTH_THING,CONDITION," +
            //                                              "  MZ_PNO1,MUSER,MDATE,MZ_EXTPOS,MZ_EXTPOS2,MZ_ISEXTPOS,MZ_ISEXTPOS2," +
            //                                              "  MZ_BPRID,MZ_BPRID1,MZ_BCHKAD,MZ_DATE,MZ_IDATE,MZ_ADATE,MZ_PRID,MZ_PRID1) ";
            //    }
            //    else
            //    {
            //        InsertPOSIT2_Part = "INSERT INTO A_POSIT2(MZ_NO,MZ_ID,MZ_NAME,MZ_CHKAD,MZ_EXOAD,MZ_EXOUNIT,MZ_EXOOCC," +
            //                                               "  MZ_EXORANK,MZ_EXRANK1,MZ_EXOPOS,MZ_EXOPCHIEF,MZ_TBNREA," +
            //                                               "  MZ_TBDATE,MZ_TBID,MZ_EXOPNO1,MZ_EXOPNO,MZ_AD,MZ_UNIT,MZ_OCCC,MZ_RANK,MZ_RANK1," +
            //                                               "  MZ_CHISI,MZ_POSIND,MZ_NIN,MZ_NREA,MZ_TBDV,MZ_PCHIEF,MZ_PESN," +
            //                                               "  MZ_PNO,MZ_TNO,MZ_WDATE,MZ_SRANK,MZ_SLVC,MZ_SPT,MZ_SPT1,MZ_MEMO," +
            //                                               "  MZ_RET,MZ_REMARK,MZ_NRT,OTH_THING,CONDITION," +
            //                                               "  MZ_PNO1,MUSER,MDATE,MZ_EXTPOS,MZ_EXTPOS2,MZ_ISEXTPOS,MZ_ISEXTPOS2," +
            //                                               "  MZ_PRID,MZ_PRID1,MZ_DATE,MZ_IDATE,MZ_ADATE) ";
            //    }

            //    string InsertPOSIT2sql;

            //    if (ViewState["MZ_AD"].ToString() == "38213800C")
            //    {
            //        InsertPOSIT2sql = InsertPOSIT2_Part + "  SELECT MZ_NO,MZ_ID,MZ_NAME,MZ_CHKAD,MZ_EXOAD,MZ_EXOUNIT,MZ_EXOOCC," +
            //                                                    "  MZ_EXORANK,MZ_EXRANK1,MZ_EXOPOS,MZ_EXOPCHIEF,MZ_TBNREA," +
            //                                                    "  MZ_TBDATE,MZ_TBID,MZ_EXOPNO1,MZ_EXOPNO,MZ_AD,MZ_UNIT,MZ_OCCC,MZ_RANK,MZ_RANK1," +
            //                                                    "  MZ_CHISI,MZ_POSIND,MZ_NIN,MZ_NREA,MZ_TBDV,MZ_PCHIEF,MZ_PESN," +
            //                                                    "  MZ_PNO,MZ_TNO,MZ_WDATE,MZ_SRANK,MZ_SLVC,MZ_SPT,MZ_SPT1,MZ_MEMO," +
            //                                                    "  MZ_RET,MZ_REMARK,MZ_NRT,OTH_THING,CONDITION," +
            //                                                    "  MZ_PNO1,'" + ViewState["MZ_ID"].ToString() + "','" + sysday + "',MZ_EXTPOS,MZ_EXTPOS2,MZ_ISEXTPOS,MZ_ISEXTPOS2, '"
            //                                                     + ViewState["MZ_PRID"].ToString() + "','" + ViewState["MZ_PRID1"].ToString() + "','"
            //                                                     + ViewState["MZ_AD"].ToString() + "','" + ViewState["MZ_DATE"].ToString() + "','"
            //                                                     + ViewState["MZ_IDATE"].ToString() + "','" + ViewState["MZ_ADATE"].ToString() + "','"
            //                                                     + ViewState["MZ_PRID"].ToString() + "','" + ViewState["MZ_PRID1"].ToString() + "'  FROM A_POSIT " +
            //                                                    "  WHERE 1=1 " + strPartSQL;
            //    }
            //    else
            //    {
            //        InsertPOSIT2sql = InsertPOSIT2_Part + "  SELECT MZ_NO,MZ_ID,MZ_NAME,MZ_CHKAD,MZ_EXOAD,MZ_EXOUNIT,MZ_EXOOCC," +
            //                                                    "  MZ_EXORANK,MZ_EXRANK1,MZ_EXOPOS,MZ_EXOPCHIEF,MZ_TBNREA," +
            //                                                    "  MZ_TBDATE,MZ_TBID,MZ_EXOPNO1,MZ_EXOPNO,MZ_AD,MZ_UNIT,MZ_OCCC,MZ_RANK,MZ_RANK1," +
            //                                                    "  MZ_CHISI,MZ_POSIND,MZ_NIN,MZ_NREA,MZ_TBDV,MZ_PCHIEF,MZ_PESN," +
            //                                                    "  MZ_PNO,MZ_TNO,MZ_WDATE,MZ_SRANK,MZ_SLVC,MZ_SPT,MZ_SPT1,MZ_MEMO," +
            //                                                    "  MZ_RET,MZ_REMARK,MZ_NRT,OTH_THING,CONDITION," +
            //                                                    "  MZ_PNO1,'" + ViewState["MZ_ID"].ToString() + "','" + sysday + "',MZ_EXTPOS,MZ_EXTPOS2,MZ_ISEXTPOS,MZ_ISEXTPOS2, '"
            //                                                     + ViewState["MZ_PRID"].ToString() + "','" + ViewState["MZ_PRID1"].ToString() + "','" + ViewState["MZ_DATE"].ToString() + "','"
            //                                                     + ViewState["MZ_IDATE"].ToString() + "','" + ViewState["MZ_ADATE"].ToString() + "'  FROM A_POSIT " +
            //                                                    " WHERE 1=1 " + strPartSQL + strPartSQL1;
            //    }

            //    SqlCommand InsertPOSIT2cmd = new SqlCommand(InsertPOSIT2sql, UpdateSWT1conn);
            //    InsertPOSIT2cmd.CommandType = CommandType.Text;
            //    InsertPOSIT2cmd.Transaction = transaction;
            //    try
            //    {
            //        InsertPOSIT2cmd.ExecuteNonQuery();
            //        UpdateSWT1cmd.ExecuteNonQuery();
            //        transaction.Commit();
            //        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.alert('謄稿成功！');window.opener.location.href='Personal3-2.aspx?PRID=" + ViewState["MZ_PRID"].ToString() + "&PRID1=" + ViewState["MZ_PRID1"].ToString() + "&DATE=&TPM_FION=" + Request.QueryString["TPM_FION"] + "';window.close();", true);
            //    }
            //    catch
            //    {
            //        transaction.Rollback();
            //        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "alert(謄稿失敗);", true);
            //    }
            //    finally
            //    {
            //        UpdateSWT1conn.Close();
            //    }
            //}
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "click", "window.close();", true);
        }
    }
}
