<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="C_diffdutydetail_xls.aspx.cs" Inherits="TPPDDB._3_forleave.C_diffdutydetail_xls" %>
<% string textfont = "";
    if (Request["pdf"] != "1")
    {
        string filename = HttpContext.Current.Server.UrlEncode("刷卡勤惰明細表.xls");
        Response.AddHeader("content-disposition", "attachment;filename=" + filename);
        Response.ContentType = "application/vnd.ms-excel";
        textfont = "10";
    }
    else
    {

        textfont = "13";
    }

    string List_EXAD = vExecSQL("SELECT  MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE = '04' and ((MZ_KCODE) LIKE (select '38213%'  ) or (MZ_KCODE) LIKE (select '376411%'  )) and MZ_KCODE='" + Request["ad"] + "' ");

%>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style type="text/css">
        .auto-style2 {
            height: 35px;
        }
        .auto-style4 {
            width: 70px;
        }
        .auto-style5 {
            height: 35px;
            width: 70px;
        }
        .auto-style6 {
            width: 980px;
            height: 73px;
        }
        .auto-style9 {
            width: 88px;
        }
        .auto-style10 {
            height: 35px;
            width: 88px;
        }
        .auto-style11 {
            width: 60px;
        }
        .auto-style12 {
            height: 35px;
            width: 60px;
        }
        .auto-style13 {
            width: 55px;
        }
        .auto-style14 {
            height: 35px;
            width: 55px;
        }
        .auto-style17 {
            width: 120px;
        }
        .auto-style18 {
            width: 120px;
            height: 35px;
        }
        .auto-style19 {
            width: 77px;
        }
        .auto-style20 {
            height: 35px;
            width: 77px;
        }
         .auto-style21 {
            width: 980px;
        }
    </style>
</head>
<body>
    <center>
  <span style="font-size:24.0pt;font-family:標楷體;"><strong>刷卡勤惰明細表</strong></span>
        <table  border=0  style='border-collapse:collapse;font-family:標楷體;font-size:<%=textfont%>pt' class="auto-style21">
                <tr>               
                    <td align="left" colspan="6" class="auto-style2">
                        機關：<%=List_EXAD%>
                    </td>

                    <td align="right" colspan="6" class="auto-style2">
                        製表時間：<%=(DateTime.Now.Year-1911).ToString() + "/" + DateTime.Now.Month.ToString().PadLeft(2, '0') + "/" + DateTime.Now.Day.ToString().PadLeft(2, '0')%>
                    </td>
                </tr>
        </table>
			<table cellpadding=1 cellspacing=0 bordercolor=#000000 border=1 bgcolor=ffffff style='border-collapse:collapse;font-family:標楷體;font-size:<%=textfont%>pt' class="auto-style6">
				<tr align=center>				
					<%--<th rowspan=2>序號</td>--%>
					<th rowspan=2 class="auto-style9">服務單位</td>
					<th rowspan=2 class="auto-style11">職稱</td>
					<th rowspan=2 class="auto-style11">姓名</td>
					<th colspan=2>差勤</td>
					<th rowspan=2 class="auto-style17">上班刷卡</td>
					<th rowspan=2 class="auto-style17">下班刷卡</td>
				    <th colspan=2>加班時數</td>
					<th rowspan=2>狀況</td>
					<th rowspan=2>備註(加班時數<br />
                        修改請敘明原因)</td>
					<th rowspan=2 class="auto-style4">主管簽<br>核狀態</td>
				</tr>
				<tr align=center>
				<th class="auto-style13">日期</th>
				<th class="auto-style19">備考</th>
				<th>時</th>
				<th>分</th>
				</tr>
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
                    date1 = Request["date1"];
                    date2 = Request["date2"];
                    dt1 = (int.Parse(date1.Substring(0, 3)) + 1911).ToString() + "/" + date1.Substring(3, 2) + "/" + date1.Substring(5, 2);
                    dt2 = (int.Parse(date2.Substring(0, 3)) + 1911).ToString() + "/" + date2.Substring(3, 2) + "/"  + date2.Substring(5, 2);
                    int daycount = new TimeSpan(DateTime.Parse(dt2).Ticks - DateTime.Parse(dt1).Ticks).Days;
                    id = Request["mz_id"];
                    ad = Request["ad"];
                    unit = Request["unit"];
                    name = Request["name"];
                    type  = Request["type"];
                    sql = "Select distinct MZ_NAME , AKU.MZ_KCHI MZ_EXUNIT, AKO.MZ_KCHI MZ_OCCC, MZ_ID From A_DLBASE ";
                    sql += " left join A_KTYPE AKU ON AKU.MZ_KCODE=MZ_EXUNIT AND AKU.MZ_KTYPE='25' ";
                    sql += " left join A_KTYPE AKO ON AKO.MZ_KCODE=MZ_OCCC AND AKO.MZ_KTYPE='26' where 1 = 1 ";
                    //Where MZ_ID='{0}' ", dr["MZ_ID"]);

                    //if(date1!="")
                    //{
                    //    sql += " and over_day >= '" + date1 + "'";
                    //}
                    //if(date2!="")
                    //{
                    //    sql += " and over_day <= '" + date2 + "'";
                    //}

                    if(id != "")
                    {
                        sql += " and MZ_ID = '" + id + "'";
                    }

                    if(ad!="")
                    {
                        sql += " and MZ_EXAD = '" + ad + "'";
                    }
                    if(unit!="")
                    {
                        sql += " and MZ_EXUNIT = '" + unit + "'";
                    }
                    if(name!="")
                    {
                        sql += " and MZ_ID in (select MZ_ID from A_DLBASE where MZ_Name like '%" + name + "%')";
                    }

                    cmd.CommandText = sql;
                    cmd.Connection = conn;
                    cmd1.Connection = conn1;
                    System.Data.SqlClient.SqlDataReader dr = cmd.ExecuteReader();
                    System.Data.SqlClient.SqlDataReader dr1;
                    int i = 1;

                    while (dr.Read())
                    {
                        string show = "", unitname = "", titlename = "",mzid = "" , mzname = "" , stime = "" , etime = "" , status ="" , holiday = "" , reason = "";
                        int over_total = 0;
                        DateTime tmpdate;
                        string tmpdatestr = "",etmpdatestr = "";
                        mzid = dr["MZ_ID"].ToString(); //身分證字號
                        mzname = dr["MZ_NAME"].ToString(); //姓名
                        unitname = dr["MZ_EXUNIT"].ToString(); //單位
                        titlename= dr["MZ_OCCC"].ToString();   //職稱

                        for(int j = 0; j <= daycount; j++)
                        {

                            tmpdate = DateTime.Parse(dt1).AddDays(j);

                            string strdata = Count_Card_Record(mzid,tmpdate,type,unitname,mzname,titlename);
                            string[] split_strdata = strdata.Split(',');
                            string intime = split_strdata[0].ToString(); //上班刷卡
                            string outtime = split_strdata[1].ToString(); //下班刷卡
                            string kind = split_strdata[2].ToString(); //狀況
                            string memo = split_strdata[3].ToString(); //備註

                            tmpdatestr = (tmpdate.Year - 1911).ToString("000") + tmpdate.ToString("MMdd");
                            etmpdatestr = (tmpdate.Year - 1911).ToString("000") + "/" + tmpdate.ToString("MM/dd");
                            #region 刷卡使用cs來判斷
                            //sql = "select * from C_CardHistory_New where logdate = '" + tmpdate.ToString("yyyy/MM/dd") + "' and username like '%" + dr["MZ_ID"].ToString() + "%' order by logtime asc";
                            //cmd1.CommandText = sql;
                            //dr1 = cmd1.ExecuteReader();
                            //if(dr1.Read())
                            //{
                            //    stime = dr1["logtime"].ToString();
                            //}
                            //else
                            //{
                            //    stime = "";
                            //}
                            //dr1.Close();

                            //sql = "select * from C_CardHistory_New where logdate = '" + tmpdate.ToString("yyyy/MM/dd") + "' and username like '%" + dr["MZ_ID"].ToString() + "%' order by logtime desc";
                            //cmd1.CommandText = sql;
                            //dr1 = cmd1.ExecuteReader();
                            //if(dr1.Read())
                            //{
                            //    etime = dr1["logtime"].ToString();
                            //}
                            //else
                            //{
                            //    etime = "";
                            //}
                            //dr1.Close();
                            #endregion

                            sql = "  Select (Select C_STATUS_NAME From (Select cs.C_STATUS_NAME, coh.OVERTIME_SN, ROW_NUMBER() OVER (partition by OVERTIME_SN ORDER BY coh.O_SN desc) AS RN From C_OVERTIME_HISTORY coh INNER JOIN C_STATUS cs ON cs.C_STATUS_SN=coh.PROCESS_STATUS)  ";
                            sql += "  Where RN=1 And OVERTIME_SN=cob.SN ) OVER_STATUS From C_OVERTIME_BASE cob   ";
                            sql += "   Where TWDATE_FORMAT(cob.OVER_DAY, 'yyy/mm/dd') >='"+etmpdatestr+"' And TWDATE_FORMAT(cob.OVER_DAY, 'yyy/mm/dd') <='"+etmpdatestr+"' And MZ_ID='"+dr["MZ_ID"].ToString()+"'  ";
                            sql += "   Order by cob.OVER_DAY ";
                            string qOVER_STATUS = "";
                            cmd1.CommandText = sql;
                            dr1 = cmd1.ExecuteReader();
                            if(dr1.Read())
                            {
                                qOVER_STATUS = dr1["OVER_STATUS"].ToString();
                            }
                            else
                            {
                                qOVER_STATUS = "";
                            }
                            dr1.Close();

                            #region 上下班刷卡
                            #region
                            //if (stime=="")
                            //{
                            //    stime = "未刷卡";
                            //}
                            //if (etime=="")
                            //{
                            //    etime = "未刷卡";
                            //}
                            //if(stime=="未刷卡"&&etime=="未刷卡")
                            //{
                            //    status = "刷卡異常";
                            //}
                            //else
                            //{
                            //    if(stime=="未刷卡")
                            //    {
                            //        status = "上班異常";
                            //    }

                            //    if(etime=="未刷卡")
                            //    {
                            //        status = "下班異常";
                            //    }
                            //}
                            #endregion
                            //bool sflag = false;
                            //bool eflag = false;
                            //status = "";
                            //if(stime=="")
                            //{   //狀況
                            //    status = "上班未刷卡";
                            //    sflag = true;
                            //}
                            //if (etime=="")
                            //{
                            //    status = "下班未刷卡";
                            //    eflag = true;
                            //}
                            //if(sflag == true && eflag== true)
                            //{
                            //    status = "未刷卡";
                            //}
                            #endregion
                            #region 例假日判斷(備考)
                            //holiday = "";
                            //sql = "select * from C_DUTYHOLIDAY where MZ_HOLIDAY_DATE = '" + tmpdatestr + "'";
                            //cmd1.CommandText = sql;
                            //dr1 = cmd1.ExecuteReader();
                            //if (dr1.Read())
                            //{
                            //    holiday = "例假日"; //姓名
                            //}
                            //dr1.Close();

                            ////holiday = tmpdate.DayOfWeek.ToString();
                            //if (tmpdate.DayOfWeek.ToString() == "Saturday" || tmpdate.DayOfWeek.ToString() == "Sunday")
                            //{
                            //    holiday = "例假日";
                            //}

                            //if (holiday == "例假日")
                            //{
                            //    status = "";
                            //    #region
                            //    /*if (stime == "未刷卡")
                            //    {
                            //        stime = "";
                            //    }

                            //    if (etime == "未刷卡")
                            //    {
                            //        etime = "";
                            //    }*/
                            //    #endregion
                            //}
                            ////假勤判斷
                            //sql = "select C_DLCODE.MZ_CName from C_DLTB01 inner join C_DLCODE on C_DLTB01.MZ_Code = C_DLCODE.MZ_Code where MZ_idate1 <= '" + tmpdatestr + "' and MZ_odate >= '" + tmpdatestr + "' and MZ_ID = '" + dr["MZ_ID"].ToString() + "'";
                            //cmd1.CommandText = sql;
                            //dr1 = cmd1.ExecuteReader();
                            //if (dr1.Read())
                            //{
                            //    status = dr1["MZ_CName"].ToString() ;
                            //}
                            ////判斷備考(status有假別與未刷卡)
                            //if (status != "" && status != "未刷卡") { holiday = ""; }
                            //else if (holiday == "" && sflag == true && eflag == true) { holiday = "上班異常"; }
                            #endregion

                            dr1.Close();

                            if (type != "1")
                            {
                                //加班判斷
                                sql = "select * from C_overtime_base where over_day = '" + tmpdatestr + "' and MZ_ID = '" + dr["MZ_ID"].ToString() + "'";
                                cmd1.CommandText = sql;
                                dr1 = cmd1.ExecuteReader();
                                if (dr1.Read())
                                {
                                    over_total = int.Parse(dr1["over_total"].ToString()); //加班
                                    reason = dr1["reason"].ToString();
                                    intime = dr1["OVER_STIME"].ToString();
                                    outtime = dr1["OVER_ETIME"].ToString();
                                }
                                else
                                {
                                    over_total = 0;
                                    reason = "";
                                }
                                dr1.Close();
                            }
                            else
                            {
                                over_total = 0;
                                reason = "";
                            }
                            show = "0";
                            //呈現
                            if (type == "0")
                            {
                                show = "1";
                            }
                            else if (type == "1")
                            {
                                if(status.IndexOf("異常")>=0)
                                {
                                    show = "1";
                                }
                            }
                            else if (type == "2")
                            {
                                if(reason.Length > 0 )
                                {
                                    show = "1";
                                }
                            }


                    %>
                <% if (show == "1")
                         { %>
				<tr align=center>
					<%--<td><%=i%></td>--%>
					<td class="auto-style10" style="font-size:15px;"><%=unitname%></td>
					<td class="auto-style12"><%=titlename%></td>
					<td class="auto-style12"><%=mzname%></td>
					<td class="auto-style14"><%=tmpdatestr%></td>
					<td class="auto-style20"><%=memo%></td>    <!--備考holiday-->
					<td class="auto-style18" style="font-size:20px;"><%=intime%></td>
				    <td class="auto-style18" style="font-size:20px;"><%=outtime%></td>
					<td class="auto-style2"><%=Math.Truncate(decimal.Parse(over_total.ToString()) / 60)%></td>
					<td class="auto-style2"><%=decimal.Parse(over_total.ToString()) % 60%></td>
					<td class="auto-style2" style="font-size:12px;"><%=kind %></td>  <!--狀況status-->
					<td class="auto-style2"><%=reason%></td>    <!--備註-->
					<td class="auto-style5"><%=qOVER_STATUS%></td>
				</tr>	
                <% } %>
                <%      
                        i++;
                        }
                    }
                    dr.Close();
                    conn.Close();
                    conn.Dispose();
                    %>
			</table>
        </center>
</body>
</html>
