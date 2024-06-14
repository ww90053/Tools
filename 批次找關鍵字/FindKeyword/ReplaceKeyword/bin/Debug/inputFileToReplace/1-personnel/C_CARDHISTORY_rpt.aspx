<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="C_CARDHISTORY_rpt.aspx.cs" Inherits="TPPDDB._1_personnel.C_CARDHISTORY_rpt" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <center>
            <%
                     string connstr = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["MSDBConnectionString_toTest"].ConnectionString;
                     System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(connstr);
                     System.Data.SqlClient.SqlConnection conn1 = new System.Data.SqlClient.SqlConnection(connstr);
                     conn.Open();
                     conn1.Open();
                     string sql = "";
                     //string date1 = "", date2 = "" , dt1 = "" , dt2 = "", ad = "",unit="" , name = "" , id = "" , type= "";
                     System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                     System.Data.SqlClient.SqlCommand cmd1 = new System.Data.SqlClient.SqlCommand();
                     string SN = Request["SN"];
                     string strSQL = string.Format(@"SELECT * FROM C_CARDHISTORY_EDIT WHERE SN='{0}'", SN);
                     cmd.CommandText = strSQL;
                     cmd.Connection = conn;
                     cmd1.Connection = conn1;
                     System.Data.SqlClient.SqlDataReader dr = cmd.ExecuteReader();
                     System.Data.SqlClient.SqlDataReader dr1;
                     while (dr.Read())
                     {          //單位    職稱          姓名      原因      日期              上班時間   下班時間     事由          申請者     決行者         決行日期    收件者         收件日期
                         string unit = "", occc = "", name = "", ctype = "", datetime = "", intime = "", outtime = "", memo = "", adduser = "", moduser = "", moddate = "", suruser = "", surdate = "";
                         name = dr["MZ_NAME"].ToString();
                         occc = dr["MZ_OCCC"].ToString();
                         datetime = dr["DATETIME"].ToString();
                         intime = dr["INTIME"].ToString();
                         outtime = dr["OUTTIME"].ToString();
                         if (dr["MOD_DATE"].ToString() != "")
                         { moddate = Convert.ToDateTime(dr["MOD_DATE"].ToString()).ToString("yyyy/MM/dd HH:mm:ss"); }
                         else { moddate = ""; }

                         surdate = dr["SURE_DATE"].ToString();
                         memo = dr["MEMO"].ToString();

                         string ddlAD_SQL = "SELECT (SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KCODE=MZ_UNIT AND MZ_KTYPE='25') MZ_KCHI From C_CARDHISTORY_EDIT WHERE SN = " + SN;
                         cmd1.CommandText = ddlAD_SQL;
                         dr1 = cmd1.ExecuteReader();
                         if (dr1.Read())
                         {  unit = dr1["MZ_KCHI"].ToString(); } //單位  
                         dr1.Close();

                         ddlAD_SQL = "SELECT (SELECT MZ_NAME FROM A_DLBASE WHERE MZ_ID=ADD_USER ) ADD_USER FROM C_CARDHISTORY_EDIT WHERE SN = " + SN;
                         cmd1.CommandText = ddlAD_SQL;
                         dr1 = cmd1.ExecuteReader();
                         if (dr1.Read())
                         {  adduser = dr1["ADD_USER"].ToString();  }//申請者 
                         dr1.Close();

                         ddlAD_SQL = "SELECT (SELECT MZ_NAME FROM A_DLBASE WHERE MZ_ID=MOD_USER ) MOD_USER FROM C_CARDHISTORY_EDIT WHERE SN = " + SN;
                         cmd1.CommandText = ddlAD_SQL;
                         dr1 = cmd1.ExecuteReader();
                         if (dr1.Read())
                         {   moduser = dr1["MOD_USER"].ToString();  }//決行者  
                         dr1.Close();

                         ddlAD_SQL = "SELECT (SELECT MZ_NAME FROM A_DLBASE WHERE MZ_ID=SURE_USER ) SURE_USER FROM C_CARDHISTORY_EDIT WHERE SN = " + SN;
                         cmd1.CommandText = ddlAD_SQL;
                         dr1 = cmd1.ExecuteReader();
                         if (dr1.Read())
                         {  suruser = dr1["SURE_USER"].ToString(); }//收件者  
                         dr1.Close();

                         ddlAD_SQL = "SELECT CASE CTYPE WHEN '0' THEN '因公未刷' ELSE '忘記刷卡' END AS CTYPE FROM C_CARDHISTORY_EDIT WHERE SN = " + SN;
                         cmd1.CommandText = ddlAD_SQL;
                         dr1 = cmd1.ExecuteReader();
                         if (dr1.Read())
                         {  ctype = dr1["CTYPE"].ToString(); }//收件者 
                         dr1.Close();

                        %>
        <table cellpadding=3 cellspacing=0 bordercolor=#000000 border=1 width="800"  style="font-family:DFKai-sb;">
	        <tr height="150px" style="font-size:30px;" > 
		        <td align=center colspan="6" ><B>新北市政府警察局&emsp;&emsp;職員未辦理按卡紀錄報告單</B></td>
	        </tr>
                        <tr style='font-size:20pt;' height="150px">
					        <td width="15%" align="center">單位</td>
					        <td width="20%">&ensp;<%=unit%></td>
					        <td width="15%" align=center>職稱</td>
					        <td width="20%">&ensp;<%=occc%></td>
					        <td width="15%" align=center>姓名</td>
					        <td width="20%">&ensp;<%=name%></td>
				        </tr>
				        <tr style='font-size:20pt;' height="150px">
					        <td align=center>原因</td>
					        <td>&ensp;<%=ctype%></td>
					        <td align=center>日期</td>
					        <td colspan = '3'>&ensp;<%=datetime%></td>
				        </tr>
				        <tr style='font-size:20pt;' height="150px">
					        <td align=center>上班紀錄</td>
					        <td colspan = '2'>&ensp;<%=intime%></td>
					        <td align=center>下班紀錄</td>
					        <td colspan = '2'>&ensp;<%=outtime%></td>
				        </tr>
				        <tr style='font-size:20pt;' height="150px">
					        <td align=center>事由</td>
					        <td colspan = '5'>&ensp;<%=memo%></td>
				        </tr>
				        <tr style='font-size:20pt;' height="150px">
					        <td align=center>申請者</td>
					        <td colspan = '5'>&ensp;<%=adduser%></td>
				        </tr>
            <tr>
                <td colspan="6">
                    <table style='font-size:20pt;' height="250px" border="0" width ="100%">
                        <tr>
                            <td colspan ="6" align="left">
                                主管決行:&ensp;<%=moduser %> &emsp; <%=moddate %>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6" align="center">
                                收件:&ensp;<%=suruser %> &emsp; <%=surdate %>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <%
                                       }
                    dr.Close();
                    conn.Close();
                    conn.Dispose();
                    conn1.Close();
                    conn1.Dispose();
 
            %>
        </table>
     
        </center>

</body>
</html>
