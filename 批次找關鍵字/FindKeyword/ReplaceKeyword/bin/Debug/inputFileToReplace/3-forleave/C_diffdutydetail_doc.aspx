<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="C_diffdutydetail_doc.aspx.cs" Inherits="TPPDDB._3_forleave.C_diffdutydetail_doc" %>
<% string textfont = "";
    if (Request["pdf"] != "1")
    {
        string filename = HttpContext.Current.Server.UrlEncode("刷卡勤惰明細表.doc");
        Response.AddHeader("content-disposition", "attachment;filename=" + filename);
        Response.ContentType = "application/vnd.ms-word";;
        textfont = "10";
    }
    else
    {

        textfont = "13";
    }

    string List_EXAD = TPPDDB._3_forleave.C_diffdutydetail_xls.vExecSQL("SELECT  MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE = '04' and ((MZ_KCODE) LIKE (select '38213%'  ) or (MZ_KCODE) LIKE (select '376411%'  )) and MZ_KCODE='" + Request["ad"] + "' ");

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
            width: 100px;
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
            width: 80px;
        }
        .auto-style18 {
            /*/*width: 120px;  拿掉,因為上面的表投就會決定寬度了,不用決定兩次*/
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
				    <th colspan=2 style="width:40px;">加班<br />時數</td>
					<th rowspan=2 style="width:40px;">狀況</td>
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
                    System.Data.SqlClient.SqlConnection conn2 = new System.Data.SqlClient.SqlConnection(connstr);
                    conn.Open();
                    conn1.Open();
                    conn2.Open();
                    string sql = "",sql2;
                    string date1 = "", date2 = "" , dt1 = "" , dt2 = "", ad = "",unit="" , name = "" , id = "" , type= "";
                    System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                    System.Data.SqlClient.SqlCommand cmd1 = new System.Data.SqlClient.SqlCommand();
                    System.Data.SqlClient.SqlCommand cmd2 = new System.Data.SqlClient.SqlCommand();
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
                    //抓取人員人數
                    sql2 = "Select distinct count(MZ_ID) as ID From A_DLBASE ";
                    sql2 += " left join A_KTYPE AKU ON AKU.MZ_KCODE=MZ_EXUNIT AND AKU.MZ_KTYPE='25' ";
                    sql2 += " left join A_KTYPE AKO ON AKO.MZ_KCODE=MZ_OCCC AND AKO.MZ_KTYPE='26' where 1 = 1 AND MZ_STATUS2 = 'Y' ";
                    //抓取人員基本資料
                    sql = "Select distinct MZ_NAME , AKU.MZ_KCHI MZ_EXUNIT, AKO.MZ_KCHI MZ_OCCC, MZ_ID From A_DLBASE ";
                    sql += " left join A_KTYPE AKU ON AKU.MZ_KCODE=MZ_EXUNIT AND AKU.MZ_KTYPE='25' ";
                    sql += " left join A_KTYPE AKO ON AKO.MZ_KCODE=MZ_OCCC AND AKO.MZ_KTYPE='26' where 1 = 1 AND MZ_STATUS2 = 'Y' ";
                    //Where MZ_ID='{0}' ", dr["MZ_ID"]);

                    if(id != "")
                    {
                        sql += " and MZ_ID = '" + id + "'";
                        sql2 += " and MZ_ID = '" + id + "'";
                    }

                    if(ad!="")
                    {
                        sql += " and MZ_EXAD = '" + ad + "'";
                        sql2 += " and MZ_EXAD = '" + ad + "'";
                    }
                    if(unit!="")
                    {
                        sql += " and MZ_EXUNIT = '" + unit + "'";
                        sql2 += " and MZ_EXUNIT = '" + unit + "'";
                    }
                    if(name!="")
                    {
                        sql += " and MZ_ID in (select MZ_ID from A_DLBASE where MZ_Name like '%" + name + "%')";
                        sql2 += " and MZ_ID in (select MZ_ID from A_DLBASE where MZ_Name like '%" + name + "%')";
                    }

                    cmd.CommandText = sql;
                    cmd2.CommandText = sql2;
                    cmd.Connection = conn;
                    cmd1.Connection = conn1;
                    cmd2.Connection = conn2;
                    System.Data.SqlClient.SqlDataReader dr = cmd.ExecuteReader();
                    System.Data.SqlClient.SqlDataReader dr2 = cmd2.ExecuteReader();
                    System.Data.SqlClient.SqlDataReader dr1;

                    int i = 1;
                    int tmpi = 0;
                    dr2.Read();
                    string str = dr2["ID"].ToString();
                    dr2.Close();
                    dr2.Dispose();
                    int a = 0;

                    while (dr.Read())
                    {
                        a ++;
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
                            //判斷讀卡機的紀錄,回傳格式為:"上班刷卡時間,下班刷卡時間,狀況,備註"
                            string strdata = TPPDDB._3_forleave.C_diffdutydetail_xls.Count_Card_Record(mzid,tmpdate,type,unitname,mzname,titlename);
                            string[] split_strdata = strdata.Split(',');
                            string intime = split_strdata[0].ToString(); //上班刷卡
                            string outtime = split_strdata[1].ToString(); //下班刷卡
                            string kind = split_strdata[2].ToString(); //狀況
                            string memo = split_strdata[3].ToString(); //備註

                            tmpdatestr = (tmpdate.Year - 1911).ToString("000") + tmpdate.ToString("MMdd");
                            etmpdatestr = (tmpdate.Year - 1911).ToString("000") + "/" + tmpdate.ToString("MM/dd");

                            sql = @"
                                Select 
                                --ID,加班日,加班時數,理由
                                MZ_ID,cob.OVER_DAY,over_total,reason
                                ,C_STATUS_NAME as OVER_STATUS ,MZ_NAME as Review_Name,MZ_OCCC_NAME
                                  From C_OVERTIME_BASE cob   
                                  join (
                                        --最後一次簽屬
                                        Select * From 
                                        (
                                            --簽名者資訊
                                            Select cs.C_STATUS_NAME, coh.OVERTIME_SN
                                            , ROW_NUMBER() OVER (partition by OVERTIME_SN ORDER BY coh.O_SN desc) AS RN 
                                            --簽名者 姓名 職稱代號 職稱
                                            ,a.MZ_NAME,a.MZ_OCCC,k.MZ_KCHI as MZ_OCCC_NAME
                                            From C_OVERTIME_HISTORY coh 
                                            INNER JOIN C_STATUS cs ON cs.C_STATUS_SN=coh.PROCESS_STATUS
                                            LEFT JOIN A_DLBASE a on a.MZ_ID=coh.Review_ID
                                            --職稱代碼
                                            LEFT JOIN A_KTYPE k on a.MZ_OCCC=k.MZ_KCODE and MZ_KTYPE='26'
                                        )  
                                       Where RN=1
                                  ) LAST_C_OVERTIME_HISTORY on LAST_C_OVERTIME_HISTORY.OVERTIME_SN=cob.SN
                                   Where TWDATE_FORMAT(cob.OVER_DAY, 'yyy/mm/dd') >='"+etmpdatestr+@"'
                                   And TWDATE_FORMAT(cob.OVER_DAY, 'yyy/mm/dd') <='"+etmpdatestr+@"' 
                                   And MZ_ID='"+dr["MZ_ID"].ToString()+@"'  
                                   Order by cob.OVER_DAY 
";
                            //sql = "  Select (Select C_STATUS_NAME From (Select cs.C_STATUS_NAME, coh.OVERTIME_SN, ROW_NUMBER() OVER (partition by OVERTIME_SN ORDER BY coh.O_SN desc) AS RN From C_OVERTIME_HISTORY coh INNER JOIN C_STATUS cs ON cs.C_STATUS_SN=coh.PROCESS_STATUS)  ";
                            //sql += "  Where RN=1 And OVERTIME_SN=cob.SN ) OVER_STATUS From C_OVERTIME_BASE cob   ";
                            //sql += "   Where TWDATE_FORMAT(cob.OVER_DAY, 'yyy/mm/dd') >='"+etmpdatestr+"' And TWDATE_FORMAT(cob.OVER_DAY, 'yyy/mm/dd') <='"+etmpdatestr+"' And MZ_ID='"+dr["MZ_ID"].ToString()+"'  ";
                            //sql += "   Order by cob.OVER_DAY ";
                            string qOVER_STATUS = "";
                            string MZ_OCCC_NAME = "";
                                    over_total = 0;
                                    reason = "";
                            cmd1.CommandText = sql;
                            dr1 = cmd1.ExecuteReader();
                            if(dr1.Read())
                            {
                                qOVER_STATUS = dr1["OVER_STATUS"].ToString();
                                MZ_OCCC_NAME = dr1["MZ_OCCC_NAME"].ToString();
                                //如果主管簽核狀態原本為「決行」或「核定」(C_OVERTIME_HISTORY這張表)，顯示改為 職稱+動作,EX:主任核定
                                if (qOVER_STATUS == "決行" || qOVER_STATUS == "核定")
                                {
                                    qOVER_STATUS = MZ_OCCC_NAME + qOVER_STATUS;
                                }//如果搜尋條件不是 1異常
                                if (type != "1")
                                {
                                    //加班判斷
                                    over_total = int.Parse(dr1["over_total"].ToString()); //加班
                                    reason = dr1["reason"].ToString();                                
                                }
                            }
                            
                            dr1.Close();
                            show = "0";
                            //如果報表類型為:全部
                            if (type == "0")
                            {   //所有資料都秀
                                show = "1";
                            }
                            //如果報表類型為:異常
                            else if (type == "1")
                            {
                                //如果備註有"異常"字眼
                                if(memo.IndexOf("異常")>=0)
                                {
                                    show = "1";//顯示資料
                                }
                                //如果狀態有 未刷卡 字眼
                                if(kind.IndexOf("未刷卡")>=0)
                                {
                                    show = "1";//顯示資料
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
                            <% if (show == "1"){ 
                                    i++;
                                    tmpi++;
                                    %>
				            <tr align=center>
					            <%--<td><%=i%></td>--%>
					            <td class="auto-style10" style="font-size:15px;"><%=unitname%></td>
					            <td class="auto-style12"><%=titlename%></td>
					            <td class="auto-style12"><%=mzname%></td>
					            <td class="auto-style14"><%=tmpdatestr%></td>
					            <td class="auto-style20"><%=memo%></td>    <!--備考holiday-->
					            <td class="auto-style18" style="font-size:16px;"><%=intime%></td>
				                <td class="auto-style18" style="font-size:16px;"><%=outtime%></td>
					            <td class="auto-style2"><%=Math.Truncate(decimal.Parse(over_total.ToString()) / 60)%></td>
					            <td class="auto-style2"><%=decimal.Parse(over_total.ToString()) % 60%></td>
					            <td class="auto-style2" style="font-size:12px;"><%=kind %></td>  <!--狀況status-->
					            <td class="auto-style2"><%=reason%></td>    <!--備註-->
					            <td class="auto-style5" style="font-size:16px;"><%=qOVER_STATUS%></td><!--主管簽核狀態-->
				            </tr>	
                            <% 

                                }
                                int itotal = (i -1) % (daycount+1); //判斷是否需要換頁
                                if (i % 31 == 0 && tmpi == 30 && a != int.Parse(str)) {
                                    tmpi = 0;
                                    i = 1;
                            %>
                </table>
                                <div style='mso-special-character:line-break; page-break-before:always'></div>
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
				    <th colspan=2>加班<br />時數</td>
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
                            }

                        }
                    }
                    dr.Close();
                    dr.Dispose();
                    cmd1.Dispose();
                    cmd.Dispose();
                    conn1.Close();
                    conn1.Dispose();
                    conn.Close();
                    conn.Dispose();
                    %>
			</table>
        </center>
</body>
</html>
