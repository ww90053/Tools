<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="C_overtime_monthrpt1.aspx.cs" Inherits="TPPDDB._3_forleave.C_overtime_monthrpt1" %>
<% string textfont = "";
    if (Request["pdf"] != "1")
    {
        string filename = HttpContext.Current.Server.UrlEncode("業務加班費補發清冊.xls");
        Response.AddHeader("content-disposition", "attachment;filename=" + filename);
        Response.ContentType = "application/vnd.ms-excel";
       textfont = "10";
    }
    else
    {
        
       textfont = "13";
    }
%>
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
                     string date1 = "", date2 = "" , dt1 = "" , dt2 = "", ad = "",unit="" , name = "" , id = "" , type= "";
                     System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                     System.Data.SqlClient.SqlCommand cmd1 = new System.Data.SqlClient.SqlCommand();
                     //  sql = "select * from C_OVERTIME_BASE where 1=1 ";
                     string year1 = Request["Year"];
                     string type1 = Request["ctype"];
                     id = Request["mz_id"];
                     ad = Request["ad"];
                     unit = Request["unit"];

                     string strSQL = "SELECT MZ_AD, MZ_UNIT, MZ_EXAD, MZ_EXUNIT, " +
                                  " MZ_REMARK_UP,MZ_ID,MZ_HOUR_LIMIT,MZ_MONEY_LIMIT,(SELECT distinct MZ_POLNO FROM A_DLBASE WHERE MZ_ID=C_OVERTIMEMONTH_HOUR.MZ_ID) AS MZ_POLNO," +
                                  " (SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='26' AND MZ_KCODE=(SELECT distinct MZ_OCCC FROM A_DLBASE WHERE MZ_ID=C_OVERTIMEMONTH_HOUR.MZ_ID)) AS MZ_OCCC," +
                                  " (SELECT distinct MZ_NAME FROM A_DLBASE WHERE MZ_ID=C_OVERTIMEMONTH_HOUR.MZ_ID) AS MZ_NAME," +
                                  " MZ_BUDGET_HOUR , TOTAL ,CASE WHEN MZ_REAL_HOUR>=MZ_HOUR_LIMIT THEN MZ_HOUR_LIMIT ELSE MZ_REAL_HOUR END MZ_REAL_HOUR,MZ_BALANCE_HOUR,PAY1,PROFESS,BOSS," +
                                  " MZ_HOUR_PAY,CASE WHEN MZ_REAL_HOUR>=MZ_HOUR_LIMIT THEN CASE  WHEN MZ_HOUR_PAY*MZ_HOUR_LIMIT > MZ_MONEY_LIMIT THEN MZ_MONEY_LIMIT ELSE MZ_HOUR_PAY*MZ_HOUR_LIMIT END ELSE CASE WHEN MZ_OVERTIME_PAY > MZ_MONEY_LIMIT THEN MZ_MONEY_LIMIT ELSE MZ_OVERTIME_PAY END  END MZ_OVERTIME_PAY,MZ_ID,MZ_VERIFY , " +
                                  " PAY1_UP,PROFESS_UP,BOSS_UP, MZ_HOUR_PAY_UP, MZ_OVERTIME_PAY_UP ,  MZ_OVERTIME_PAY1 , EFF_HOUR_UP " +
                                  " FROM C_OVERTIMEMONTH_HOUR " +
                                  " WHERE " +
                                  "      MZ_YEAR='" + year1.Substring(0,3) +
                                  "' AND MZ_MONTH='" + year1.Substring(3,2) + "' and (MZ_OVERTIME_PAY1 > 0 or EFF_HOUR_UP > 0)  ";

                     // Joy 合計總數
                     string strSUMSQL = "SELECT sum(MZ_OVERTIME_PAY_UP - mz_overtime_pay) as SUM   FROM C_OVERTIMEMONTH_HOUR " +
                                           " WHERE " +
                                           "      MZ_YEAR='" + year1.Substring(0,3) +
                                           "' AND MZ_MONTH='" + year1.Substring(3,2) +"'";
                     // Joy 新增身份證字號欄位
                     // 新增需求 如果身分證有輸入資料的話 機關單位不做判斷
                     if (!string.IsNullOrEmpty(id))
                     {
                         strSQL += " AND MZ_ID = '" + id + "' ";
                         strSUMSQL += " AND MZ_ID = '" + id + "' ";
                     }

                        if (!string.IsNullOrEmpty(ad))
                           {
                               if (type1 == "0")
                               {
                                    strSQL += "  AND  MZ_AD='" + ad + "' ";
                                    strSUMSQL += "  AND  MZ_AD='" + ad + "' ";
                               }
                               else
                               {
                                   strSQL += "  AND  MZ_EXAD='" + ad + "' ";
                                   strSUMSQL += "  AND  MZ_EXAD='" + ad + "' ";

                               }

                           }
                           if (!string.IsNullOrEmpty(unit))
                           {
                               if (type1 == "0")
                               {
                                   strSQL += "  AND MZ_UNIT='" + unit + "'";
                                   strSUMSQL += "  AND MZ_UNIT='" + unit + "'";
                               }
                               else
                               {
                                    strSQL += "  AND MZ_EXUNIT='" + unit + "'";
                                    strSUMSQL += "  AND MZ_EXUNIT='" + unit + "'";
                               }

                           }
                     
                        

                     //20210121 - 人事管理-6.超勤管理作業-6.4 超勤時數審核：清單排序需要調整，順序要跟 6.3.2.超勤印領清冊 一致
                     //strSQL += " ORDER BY (SELECT REPLACE(MZ_TBDV,'Z99','999') FROM A_DLBASE WHERE MZ_ID=C_OVERTIMEMONTH_HOUR.MZ_ID),(SELECT MZ_OCCC FROM A_DLBASE WHERE MZ_ID=C_OVERTIMEMONTH_HOUR.MZ_ID),(SELECT MZ_ID FROM A_DLBASE WHERE MZ_ID=C_OVERTIMEMONTH_HOUR.MZ_ID)";
                     //strSUMSQL += " ORDER BY (SELECT REPLACE(MZ_TBDV,'Z99','999') FROM A_DLBASE WHERE MZ_ID=C_OVERTIMEMONTH_HOUR.MZ_ID),(SELECT MZ_OCCC FROM A_DLBASE WHERE MZ_ID=C_OVERTIMEMONTH_HOUR.MZ_ID),(SELECT MZ_ID FROM A_DLBASE WHERE MZ_ID=C_OVERTIMEMONTH_HOUR.MZ_ID)";
                     strSQL += " ORDER BY (SELECT distinct REPLACE(MZ_TBDV,'Z99','999') FROM A_DLBASE WHERE MZ_ID = C_OVERTIMEMONTH_HOUR.MZ_ID), (SELECT distinct MZ_PCHIEF FROM A_DLBASE WHERE MZ_ID = C_OVERTIMEMONTH_HOUR.MZ_ID), (SELECT distinct MZ_OCCC FROM A_DLBASE WHERE MZ_ID = C_OVERTIMEMONTH_HOUR.MZ_ID), (SELECT distinct MZ_OFFYY FROM   A_DLBASE WHERE  MZ_ID = C_OVERTIMEMONTH_HOUR.MZ_ID) DESC, C_OVERTIMEMONTH_HOUR.MZ_ID";
                     strSUMSQL += " ORDER BY (SELECT distinct REPLACE(MZ_TBDV,'Z99','999') FROM A_DLBASE WHERE MZ_ID = C_OVERTIMEMONTH_HOUR.MZ_ID), (SELECT distinct MZ_PCHIEF FROM A_DLBASE WHERE MZ_ID = C_OVERTIMEMONTH_HOUR.MZ_ID), (SELECT distinct MZ_OCCC FROM A_DLBASE WHERE MZ_ID = C_OVERTIMEMONTH_HOUR.MZ_ID), (SELECT distinct MZ_OFFYY FROM   A_DLBASE WHERE  MZ_ID = C_OVERTIMEMONTH_HOUR.MZ_ID) DESC, C_OVERTIMEMONTH_HOUR.MZ_ID";

                     cmd.CommandText = strSQL;
                     cmd.Connection = conn;
                     cmd1.Connection = conn1;
                     System.Data.SqlClient.SqlDataReader dr = cmd.ExecuteReader();
                     System.Data.SqlClient.SqlDataReader dr1;
                     int i = 1;
                     decimal over_total = 0;
                     while (dr.Read())
                     {
                         string show = "", unitname = "", titlename = "", mzname = "" , adtitle = "" , hour1 = "" , hourdiff = "" , salarydiff = "", etime = "", status = "", holiday = "", reason = "";

                         DateTime tmpdate;
                         string tmpdatestr = "", etmpdatestr = "";
                         mzname = dr["MZ_NAME"].ToString(); //姓名
                         string ddlAD_SQL = "SELECT RTRIM(MZ_KCHI) MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='04' and MZ_KCODE = '" + ad + "'";
                         cmd1.CommandText = ddlAD_SQL;
                         dr1 = cmd1.ExecuteReader();
                         if (dr1.Read())
                         {
                             adtitle = dr1["MZ_KCHI"].ToString();
                         }
                         dr1.Close();
                         ddlAD_SQL = "SELECT RTRIM(MZ_KCHI) MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='25' and MZ_KCODE = '" + dr["MZ_EXUNIT"].ToString() + "'";
                         cmd1.CommandText = ddlAD_SQL;
                         dr1 = cmd1.ExecuteReader();
                         if (dr1.Read())
                         {
                             unitname = dr1["MZ_KCHI"].ToString();//單位
                         }
                         dr1.Close();
                         //unitname = dr["MZ_EXUNIT"].ToString(); 
                         titlename = dr["MZ_OCCC"].ToString();   //職稱
                         hour1 = dr["TOTAL"].ToString() ;
                         hourdiff = (decimal.Parse(dr["MZ_Hour_Pay_UP"].ToString()) - decimal.Parse(dr["MZ_Hour_Pay"].ToString())).ToString();
                         salarydiff = dr["MZ_OVERTIME_Pay1"].ToString();
                         over_total += decimal.Parse(salarydiff);
                         if (i == 1)
                         {
                        %>
                             <table border="0" width="900" style="font-family:標楷體;">
	<tr height=50 style="font-size:20px;" > 
		<td align=center colspan="6"><B><%=adtitle%><%=unitname%><%=year1.Substring(0,3)%>年<%=year1.Substring(3,2)%>月份業務加班費補發清冊</B></td>
	</tr>
	<tr>
		<td colspan="8">  
			<table cellpadding=3 cellspacing=0 bordercolor=#000000 border=1 width=100% bgcolor=ffffff  style='font-family:標楷體;border-collapse:collapse;font-size:11pt'>
				<tr align=center style='font-size:10pt'>
					<td width="80" rowspan="3">身分證號</td>
					<td width="70" rowspan="3">職稱</td>
					<td width="70" rowspan="3"> 姓名</td>
					<td colspan="3">原報領時薪、時數及金額</td>
					<td colspan="4">晉級後報領時薪、時數及金額</td>
					<td colspan="2">補發內容</td>
					<td rowspan="3">說明</td>
				</tr>
				<tr align=center style='font-size:10pt' >
					<td width="45" rowspan="2">原領 </br> 時薪</td>
					<td colspan="2">業務加班費</td>
					<td width="45" rowspan="2">晉級 </br> 時薪</td>
					<td width="45" rowspan="2">時薪 </br> 差額</td>
					<td colspan="2">業務加班費</td>
					<td width="50" style="border-bottom-width:0pt">時數 </br> 餘數</td>
					<td width="45" rowspan="2">加班費差額</td>
				</tr>
				<tr align=center>
					<td width="45" style='font-size:10pt'>原報 </br> 時數</td>
					<td width="45" style="font-size:10pt">原報 </br> 金額</td>
					<td width="45" style="font-size:10pt">晉級 </br> 時數</td>
					<td width="45" style="font-size:10pt">晉級 </br> 金額</td>
					<td width="50" style='font-size:7pt;border-top-width:0pt'>(改為敘獎或補休時數)</td>
				</tr>

                        <%
                        }
                       
                        %>
                        <tr height="25">
                            <td><%=dr["MZ_ID"]%></td>
                            <td><%=titlename%></td>
                            <td><%=mzname%></td>
                            <td align="right"><%=dr["MZ_Hour_Pay"]%></td>
                            <td align="center"><%=hour1%></td>
                            <td align="right"><%=dr["MZ_OVERTIME_Pay"]%></td>
                            <td align="right"><%=dr["MZ_Hour_Pay_UP"]%></td>
                            <td align="right"><%=hourdiff%></td>
                            <td align="center"><%=hour1%></td>
                            <td align="right"><%=dr["MZ_OVERTIME_Pay_UP"]%></td>
                            <td align="center"><%=dr["EFF_HOUR_UP"]%></td>
                            <td align="right"><%=salarydiff%></td>
                            <td><%=dr["MZ_REMARK_UP"]%></td>
                        </tr>				
                    <%      
                        i++;
                    }
                    dr.Close();
                    conn.Close();
                    conn.Dispose();
                    conn1.Close();
                    conn1.Dispose();
                    %>
                	<tr>
					<td colspan="3">合計</td>
					<td></td>
					<td></td>
					<td></td>
					<td></td>
					<td></td>
					<td></td>
					<td></td>
					<td></td>
					<td align="right"><%=over_total%></td>
					<td></td>
				</tr>
			</table>	
			
		</td>
	</tr>
</table>
         <table style="font-family:標楷體">
            <tr>
					<td width="200">承辦單位</td>
					<td width="200">人事單位</td>
					<td width="200">會計單位</td>
					<td width="150">機關首長</td>
				</tr>
        </table>
        </center>
     
</body>
</html>
