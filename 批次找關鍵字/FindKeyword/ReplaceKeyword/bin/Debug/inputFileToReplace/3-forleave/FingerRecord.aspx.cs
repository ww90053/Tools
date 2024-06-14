using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Data.SqlClient;

namespace TPPDDB._3_forleave
{
    public partial class FingerRecord : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

           
        }

        protected void btTable_Click(object sender, EventArgs e)
        {
            
            string ADPMZ_ID = Session["ADPMZ_ID"].ToString();
            string ID = ADPMZ_ID.Substring(ADPMZ_ID.Length -5,5);//最後五碼
            string C_NAME = _strUserName;

            if (Session["ADServerID"] != null)
            {
                switch (Request.QueryString["TPM_FION"])
                {
                    case "":
                    case null:
                        TPMPermissions._boolErrorData(Session["ADServerID"].ToString(), "0", "TPFXXX0001");
                        Response.Redirect("~/ErrorPage.aspx?erid=TPFXXX0001");
                        break;
                    default:
                        if (TPMPermissions._boolPermissionID(int.Parse(Session["TPM_MID"].ToString()), Request.QueryString["TPM_FION"].ToString(), "PVIEW") == false)
                        {
                            //無權限
                            TPMPermissions._boolErrorData(Session["ADServerID"].ToString(), Request.QueryString["TPM_FION"].ToString(), "ADUXXX0001");
                            Response.Redirect("~/ErrorPage.aspx?erid=ADUXXX0001");
                        }
                        break;
                }
            }
            else
            {
                Response.Redirect("~/Login.aspx");
            }
            string strGID = TPMPermissions._strGroupData_ID(int.Parse(Session["TPM_MID"].ToString()), int.Parse(Request.QueryString["TPM_FION"].ToString()));
            switch (strGID)
            {
                case "A":
                case "TPMIDISAdmin":
                    if (TextBox1.Text.Length > 0)
                    {
                        //日期轉換格式--因為少一個0所以要轉換
                        string txtDate = ((Convert.ToDateTime(TextBox1.Text))).ToString("yyyy/MM/dd");
                        //Response.Write(txtDate);
                        string strDateFrom = txtDate.Replace("/", "");
                        //Response.Write(strDateFrom);
                        string txtDateEnd = ((Convert.ToDateTime(TextBox_DateEnd.Text))).ToString("yyyy/MM/dd");
                        string strDateEnd = txtDateEnd.Replace("/", "");
                        //Response.Write(strDateEnd);
                        SqlDataSource1.SelectCommand = "SELECT TERMINALNAME, USERID, RTRIM(dbo.SUBSTR(USERNAME,INSTR(USERNAME,' '))) AS USERNAME, LOGTIME, VERIFY, REPLACE(FKEY, 'F3', '加班') AS FKEY, REPLACE(LOGDATE, '/', '') AS F_LOGDATE FROM  C_CARDHISTORY_NEW WHERE REPLACE(LOGDATE, '/', '') BETWEEN '" + strDateFrom + "' AND '" + strDateEnd + "'";
                    }
                    else
                    {
                        SqlDataSource1.SelectCommand = "SELECT TERMINALNAME, USERID, RTRIM(dbo.SUBSTR(USERNAME,INSTR(USERNAME,' '))) AS USERNAME, LOGTIME, VERIFY, REPLACE(FKEY, 'F3', '加班') AS FKEY, REPLACE(LOGDATE, '/', '') AS F_LOGDATE FROM C_CARDHISTORY_NEW";
                    }
                    break;
                case "B":
                    if (TextBox1.Text.Length > 0)
                    {
                        //日期轉換格式--因為少一個0所以要轉換
                        string txtDate = ((Convert.ToDateTime(TextBox1.Text))).ToString("yyyy/MM/dd");
                        //Response.Write(txtDate);
                        string strDateFrom = txtDate.Replace("/", "");
                        //Response.Write(strDateFrom);
                        string txtDateEnd = ((Convert.ToDateTime(TextBox_DateEnd.Text))).ToString("yyyy/MM/dd");
                        string strDateEnd = txtDateEnd.Replace("/", "");
                        //Response.Write(strDateEnd);
                        SqlDataSource1.SelectCommand = "SELECT TERMINALNAME, USERID, RTRIM(dbo.SUBSTR(USERNAME,INSTR(USERNAME,' '))) AS USERNAME, LOGTIME, VERIFY, REPLACE(FKEY, 'F3', '加班') AS FKEY, REPLACE(LOGDATE, '/', '') AS F_LOGDATE FROM  C_CARDHISTORY_NEW WHERE REPLACE(LOGDATE, '/', '') BETWEEN '" + strDateFrom + "' AND '" + strDateEnd + "'";
                    }
                    else
                    {
                        SqlDataSource1.SelectCommand = "SELECT TERMINALNAME, USERID, RTRIM(dbo.SUBSTR(USERNAME,INSTR(USERNAME,' '))) AS USERNAME, LOGTIME, VERIFY, REPLACE(FKEY, 'F3', '加班') AS FKEY, REPLACE(LOGDATE, '/', '') AS F_LOGDATE FROM C_CARDHISTORY_NEW";
                    }
                    break;
                case "C":
                    if (TextBox1.Text.Length > 0)
                    {
                        //日期轉換格式--因為少一個0所以要轉換
                        string txtDate = ((Convert.ToDateTime(TextBox1.Text))).ToString("yyyy/MM/dd");
                        //Response.Write(txtDate);
                        string strDateFrom = txtDate.Replace("/", "");
                        //Response.Write(strDateFrom);
                        string txtDateEnd = ((Convert.ToDateTime(TextBox_DateEnd.Text))).ToString("yyyy/MM/dd");
                        string strDateEnd = txtDateEnd.Replace("/", "");
                        //Response.Write(strDateEnd);
                        SqlDataSource1.SelectCommand = "SELECT TERMINALNAME, USERID, dbo.SUBSTR(USERNAME,1,LEN(RTRIM(USERNAME))-LEN(RTRIM(dbo.SUBSTR(USERNAME,INSTR(USERNAME,' '))))) AS NUM,RTRIM(dbo.SUBSTR(USERNAME,INSTR(USERNAME,' '))) AS USERNAME, LOGTIME, VERIFY, REPLACE(FKEY, 'F3', '加班') AS FKEY, REPLACE(LOGDATE, '/', '') AS F_LOGDATE FROM  C_CARDHISTORY_NEW WHERE REPLACE(LOGDATE, '/', '') BETWEEN '" + strDateFrom + "' AND '" + strDateEnd + "' AND RTRIM(dbo.SUBSTR(USERNAME,1,LEN(RTRIM(USERNAME))-LEN(RTRIM(dbo.SUBSTR(USERNAME,INSTR(USERNAME,' ')))))) = '" + ID + "' AND RTRIM(dbo.SUBSTR(USERNAME,INSTR(USERNAME,' '))) = '" + C_NAME + "' ";
                    }
                    else
                    {
                        SqlDataSource1.SelectCommand = "SELECT TERMINALNAME, USERID, dbo.SUBSTR(USERNAME,1,LEN(RTRIM(USERNAME))-LEN(RTRIM(dbo.SUBSTR(USERNAME,INSTR(USERNAME,' '))))) AS NUM,RTRIM(dbo.SUBSTR(USERNAME,INSTR(USERNAME,' '))) AS USERNAME, LOGTIME, VERIFY, REPLACE(FKEY, 'F3', '加班') AS FKEY, REPLACE(LOGDATE, '/', '') AS F_LOGDATE FROM  C_CARDHISTORY_NEW WHERE RTRIM(dbo.SUBSTR(USERNAME,1,LEN(RTRIM(USERNAME))-LEN(RTRIM(dbo.SUBSTR(USERNAME,INSTR(USERNAME,' ')))))) = '" + ID + "'";
                    }
                    break;
                case "D":
                    if (TextBox1.Text.Length > 0)
                    {
                        //日期轉換格式--因為少一個0所以要轉換
                        string txtDate = ((Convert.ToDateTime(TextBox1.Text))).ToString("yyyy/MM/dd");
                        //Response.Write(txtDate);
                        string strDateFrom = txtDate.Replace("/", "");
                        //Response.Write(strDateFrom);
                        string txtDateEnd = ((Convert.ToDateTime(TextBox_DateEnd.Text))).ToString("yyyy/MM/dd");
                        string strDateEnd = txtDateEnd.Replace("/", "");
                        //Response.Write(strDateEnd);
                        SqlDataSource1.SelectCommand = "SELECT TERMINALNAME, USERID, dbo.SUBSTR(USERNAME,1,LEN(RTRIM(USERNAME))-LEN(RTRIM(dbo.SUBSTR(USERNAME,INSTR(USERNAME,' '))))) AS NUM,RTRIM(dbo.SUBSTR(USERNAME,INSTR(USERNAME,' '))) AS USERNAME, LOGTIME, VERIFY, REPLACE(FKEY, 'F3', '加班') AS FKEY, REPLACE(LOGDATE, '/', '') AS F_LOGDATE FROM  C_CARDHISTORY_NEW WHERE REPLACE(LOGDATE, '/', '') BETWEEN '" + strDateFrom + "' AND '" + strDateEnd + "' AND RTRIM(dbo.SUBSTR(USERNAME,1,LEN(RTRIM(USERNAME))-LEN(RTRIM(dbo.SUBSTR(USERNAME,INSTR(USERNAME,' ')))))) = '" + ID + "' AND RTRIM(dbo.SUBSTR(USERNAME,INSTR(USERNAME,' '))) = '" + C_NAME + "' ";
                    }
                    else
                    {
                        SqlDataSource1.SelectCommand = "SELECT TERMINALNAME, USERID, dbo.SUBSTR(USERNAME,1,LEN(RTRIM(USERNAME))-LEN(RTRIM(dbo.SUBSTR(USERNAME,INSTR(USERNAME,' '))))) AS NUM,RTRIM(dbo.SUBSTR(USERNAME,INSTR(USERNAME,' '))) AS USERNAME, LOGTIME, VERIFY, REPLACE(FKEY, 'F3', '加班') AS FKEY, REPLACE(LOGDATE, '/', '') AS F_LOGDATE FROM  C_CARDHISTORY_NEW WHERE RTRIM(dbo.SUBSTR(USERNAME,1,LEN(RTRIM(USERNAME))-LEN(RTRIM(dbo.SUBSTR(USERNAME,INSTR(USERNAME,' ')))))) = '" + ID + "'";
                    }
                    break;
            }

           
        }









        private string _strUserName
        {
            get
            {
                string strSQL = "SELECT MZ_NAME FROM A_DLBASE WHERE RTRIM(MZ_ID) = '" + Session["ADPMZ_ID"] + "' ";
                DataTable ADtoAdt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");
                if (ADtoAdt.Rows.Count == 1)
                {
                    return ADtoAdt.Rows[0]["MZ_NAME"].ToString().Trim();
                }
                else
                {
                    strSQL = "SELECT  \"NAME\" FROM H_VPBASE WHERE RTRIM(IDNO) = '" + Session["ADPMZ_ID"] + "' ";
                    DataTable UNITNOdt = o_DBFactory.ABC_toTest.Create_Table(strSQL, "get");
                    if (UNITNOdt.Rows.Count == 1)
                    {
                       
                        return ADtoAdt.Rows[0]["NAME"].ToString().Trim();
                    }
                    else
                    { return ""; }
                }


                //using (SqlConnection Selectconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString))
                //{
                //    Selectconn.Open();
                //    try
                //    {
                //        string strSQL = "SELECT * FROM A_DLBASE Inner Join A_KTYPE ON A_KTYPE.MZ_KCODE = A_DLBASE.MZ_OCCC "
                //        + " WHERE A_KTYPE.MZ_KTYPE = '26' AND RTRIM(MZ_ID) = '" + Session["ADPMZ_ID"] + "' ";
                //        SqlCommand Selectcmd = new SqlCommand(strSQL, Selectconn);
                //        DataTable ADtoAdt = new DataTable();
                //        ADtoAdt.Load(Selectcmd.ExecuteReader());


                //        string strSQL1 = "SELECT * FROM H_VPBASE WHERE RTRIM(IDNO) = '" + Session["ADPMZ_ID"] + "' ";
                //        SqlCommand Selectcmd1 = new SqlCommand(strSQL, Selectconn);
                //        DataTable UNITNOdt = new DataTable();
                //        UNITNOdt.Load(Selectcmd1.ExecuteReader());

                //        if (ADtoAdt.Rows.Count == 1)
                //        {
                //            string MZ_KCHI = ADtoAdt.Rows[0]["MZ_KCHI"].ToString().Trim();
                //            string MZ_NAME = ADtoAdt.Rows[0]["MZ_NAME"].ToString().Trim();
                //            return MZ_NAME;
                //        }
                //        else if (UNITNOdt.Rows.Count == 1)
                //        {
                //            string OCCC = ADtoAdt.Rows[0]["OCCC"].ToString().Trim();
                //            string NAME = ADtoAdt.Rows[0]["NAME"].ToString().Trim();
                //            return NAME;
                //        }
                //        else
                //        { return ""; }

                //    }
                //    catch { return ""; }
                //    finally { Selectconn.Close();
                //    //XX2013/06/18 
                //    Selectconn.Dispose();
                //    }
                //}
            }
        }
        //============================
       
    }
}
