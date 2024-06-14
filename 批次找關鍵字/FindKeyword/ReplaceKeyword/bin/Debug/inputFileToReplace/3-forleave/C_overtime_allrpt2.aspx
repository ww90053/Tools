ccc<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="C_overtime_allrpt2.aspx.cs" Inherits="TPPDDB._3_forleave.C_overtime_allrpt2" %>
<% string textfont = "";
    if (Request["pdf"] != "1")
    {
        string filename = HttpContext.Current.Server.UrlEncode("超勤加班費補發總清冊.xls");
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
       string year2 = Request["Yearend"];
       string type1 = Request["ctype"];
       id = Request["mz_id"];
       ad = Request["ad"];
       unit = Request["unit"];

       string strSQL = "SELECT distinct MZ_AD, MZ_UNIT, MZ_EXAD, MZ_EXUNIT, " +
                    " MZ_ID , (SELECT distinct MZ_NAME FROM A_DLBASE WHERE MZ_ID=C_DUTYMONTHOVERTIME_HOUR.MZ_ID) AS MZ_NAME" +
                     " , (SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE='26' AND MZ_KCODE=(SELECT distinct MZ_OCCC FROM A_DLBASE WHERE MZ_ID=C_DUTYMONTHOVERTIME_HOUR.MZ_ID)) AS MZ_OCCC " +
                    " FROM C_DUTYMONTHOVERTIME_HOUR " +
                    " WHERE " +
                    "      MZ_YEAR='" + year1.Substring(0,3) +
                    "' AND MZ_MONTH >='" + year1.Substring(3,2) + "' AND MZ_MONTH <='" + year2.Substring(3,2) + "' and MZ_OVERTIME_PAY1 > 0 " ;


       // Joy 新增身份證字號欄位
       // 新增需求 如果身分證有輸入資料的話 機關單位不做判斷
       if (!string.IsNullOrEmpty(id))
       {
           strSQL += " AND MZ_ID = '" + id + "' ";

       }
      
           // 將機關單位查詢抽出來獨立
           if (!string.IsNullOrEmpty(ad))
           {
               if (type1 == "0")
               {
                   strSQL += "  AND  MZ_AD='" + ad + "' ";
               }
               else
               {
                   strSQL += "  AND  MZ_EXAD='" + ad + "' ";
               }

           }
           if (!string.IsNullOrEmpty(unit))
           {
               if (type1 == "0")
               {
                   strSQL += "  AND MZ_UNIT='" + unit + "'";
               }
               else
               {
                   strSQL += "  AND MZ_EXUNIT='" + unit + "'";
               }

           }
       

       //20210121 - 人事管理-6.超勤管理作業-6.4 超勤時數審核：清單排序需要調整，順序要跟 6.3.2.超勤印領清冊 一致
       //strSQL += " ORDER BY (SELECT REPLACE(MZ_TBDV,'Z99','999') FROM A_DLBASE WHERE MZ_ID=C_DUTYMONTHOVERTIME_HOUR.MZ_ID),(SELECT MZ_OCCC FROM A_DLBASE WHERE MZ_ID=C_DUTYMONTHOVERTIME_HOUR.MZ_ID),(SELECT MZ_ID FROM A_DLBASE WHERE MZ_ID=C_DUTYMONTHOVERTIME_HOUR.MZ_ID)";
       //strSUMSQL += " ORDER BY (SELECT REPLACE(MZ_TBDV,'Z99','999') FROM A_DLBASE WHERE MZ_ID=C_DUTYMONTHOVERTIME_HOUR.MZ_ID),(SELECT MZ_OCCC FROM A_DLBASE WHERE MZ_ID=C_DUTYMONTHOVERTIME_HOUR.MZ_ID),(SELECT MZ_ID FROM A_DLBASE WHERE MZ_ID=C_DUTYMONTHOVERTIME_HOUR.MZ_ID)";
       strSQL += " ORDER BY (SELECT distinct REPLACE(MZ_TBDV,'Z99','999') FROM A_DLBASE WHERE MZ_ID = C_DUTYMONTHOVERTIME_HOUR.MZ_ID), (SELECT distinct MZ_PCHIEF FROM A_DLBASE WHERE MZ_ID = C_DUTYMONTHOVERTIME_HOUR.MZ_ID), (SELECT distinct MZ_OCCC FROM A_DLBASE WHERE MZ_ID = C_DUTYMONTHOVERTIME_HOUR.MZ_ID), (SELECT distinct MZ_OFFYY FROM   A_DLBASE WHERE  MZ_ID = C_DUTYMONTHOVERTIME_HOUR.MZ_ID) DESC, C_DUTYMONTHOVERTIME_HOUR.MZ_ID";


       cmd.CommandText = strSQL;
       cmd.Connection = conn;
       cmd1.Connection = conn1;
       System.Data.SqlClient.SqlDataReader dr = cmd.ExecuteReader();
       System.Data.SqlClient.SqlDataReader dr1;
       int i = 1;

       while (dr.Read())
       {
           string unitname = "", titlename = "", mzname = "" , adtitle = "" , hour1 = "" , hourdiff = "" , salarydiff = "", etime = "", status = "", holiday = "", reason = "";

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


           if (i == 1)
           {
                        %>
                             <table border="0" width="900" style="font-family:標楷體;">
	<tr height=50 style="font-size:20px;" > 
		<td align=center colspan="6"><B><%=adtitle%><%=unitname%><%=year1.Substring(0,3)%>年<%=year1.Substring(3,2)%>月至<%=year2.Substring(3,2)%>月超勤加班費補發總清冊</B></td>
	</tr>
	<tr>
		<td colspan="8">  
			<table cellpadding=3 cellspacing=0 bordercolor=#000000 border=1 width=100% bgcolor=ffffff  style='font-family:標楷體;border-collapse:collapse;font-size:11pt'>
				<tr align=center style='font-size:10pt'>
					<td width="80" rowspan="2">身分證號</td>
					<td width="70" rowspan="2">職稱</td>
					<td width="70" rowspan="2"> 姓名</td>
					<td colspan="6">晉級後超勤加班費差額</td>
					<td width="70" rowspan="2">合計</td>
					<%--<td rowspan="2">說明</td>--%>
                    <td rowspan="2">簽章</td>
				</tr>
				<tr>
					<td width="40">1月</td>
					<td width="40">2月</td>
					<td width="40">3月</td>
					<td width="40">4月</td>
					<td width="40">5月</td>
					<td width="40">6月</td>
				</tr>
                        <%
                        }
                        %>
                <tr height="25">
                    <td><%=dr["MZ_ID"]%></td>
                    <td><%=titlename%></td>
                    <td><%=mzname%></td>
                    <%
                        decimal over_total = 0 , pay1 = 0;
                        for(int j = 1; j <=6; j++) {
                            strSQL = "SELECT MZ_OVERTIME_PAY1 " +
                           " FROM C_DUTYMONTHOVERTIME_HOUR " +
                           " WHERE " +
                           "      MZ_YEAR='" + year1.Substring(0,3) +
                           "' AND MZ_MONTH='" + j.ToString("00") + "' and MZ_OVERTIME_PAY1 > 0 ";
                            // 新增需求 如果身分證有輸入資料的話 機關單位不做判斷
                           
                            strSQL += " AND MZ_ID = '" + dr["MZ_ID"] + "' ";


                            cmd1.CommandText = strSQL;
                            dr1 = cmd1.ExecuteReader();
                            if (dr1.Read())
                            {
                                pay1 = decimal.Parse(dr1["MZ_OVERTIME_PAY1"].ToString());//月加班金額
                                over_total += pay1;
                            }
                            else
                            {
                                pay1 = 0;
                            }
                            dr1.Close();
                     %>
                    <td align="right"><%=pay1 %></td>
                     <% } %>  
                    <td align="right"><%=over_total %></td>
                    <%--<td></td>--%>
                    <td width="100" Height="50"></td>
                    <%      
                        i++;
                    }
                    dr.Close();
                    conn.Close();
                    conn.Dispose();
                    conn1.Close();
                    conn1.Dispose();
                    %>
			</table>	
				
		</td>
	</tr>
</table>
        <table style="font-family:標楷體">
            <tr>
					<td width="200">承辦單位</td>
					<td width="200">人事單位</td>
					<td width="200">會計單位</td>
					<td width="200">機關首長</td>
				</tr>
        </table>
      </center>
       
</body>
</html>
