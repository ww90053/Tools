<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="C_ForLeaveOvertime_Leave_Check.aspx.cs" Inherits="TPPDDB._3_forleave.C_ForLeaveOvertime_Leave_Check" %>

<%@ Register Assembly="TBGridView" Namespace="TPPDDB" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="../styles/Stylesheet_1.css" rel="stylesheet" type="text/css" />
    <style>
        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel2" runat="server">
                <table style="background-color: #6699FF; color: White; width: 100%">
                    <tr>
                        <td style="text-align: left">
                            <asp:Label ID="Label1" runat="server" Style="font-family: 標楷體; font-size: large;
                                font-weight: 700"></asp:Label>
                        </td>
                    </tr>
                </table>
                
                
               
                
                
                
                
                
                
                <asp:Panel ID="Panel3" runat="server" Height="460px" ScrollBars="Both" Width="830">
                    <cc1:TBGridView ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="4"
                        DataSourceID="SqlDataSource2" EmptyDataText="無資料" EnableEmptyContentRender="True"
                        GridLines="None" Width="100%" ForeColor="#333333" OnRowDataBound="GridView1_RowDataBound">
                        <RowStyle BackColor="#EFF3FB" />
                        <Columns>
                            <asp:BoundField DataField="MZ_CODE" HeaderText="MZ_CODE" />
                            <asp:TemplateField HeaderText="核可否" SortExpression="MZ_CHK1">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("MZ_CHK1") %>'></asp:TextBox></EditItemTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="CheckBox_CHK1" runat="server" Checked='<%# Eval("MZ_CHK1").ToString()=="Y"?true:false %>' /></ItemTemplate>
                                <%-- <HeaderTemplate>
                                    <asp:CheckBox ID="CheckBox1" runat="server" AutoPostBack="True" OnCheckedChanged="CheckBox1_CheckedChanged"
                                        Text="全選" />
                                </HeaderTemplate>--%>
                            </asp:TemplateField>
                            <asp:BoundField DataField="SIGN_KIND" HeaderText="類型" />
                            <asp:BoundField DataField="MZ_ID" HeaderText="身分證號" />
                            <asp:BoundField DataField="MZ_NAME" HeaderText="姓名" />
                            <asp:BoundField DataField="MZ_CODE_NAME" HeaderText="假別" />
                            <asp:BoundField DataField="MZ_CAUSE" HeaderText="請假事由" SortExpression="MZ_CAUSE" />
                            <asp:BoundField DataField="MZ_RNAME" HeaderText="代理人" SortExpression="MZ_RNAME" />
                            <asp:BoundField DataField="MZ_IDATE1" HeaderText="請假日起" SortExpression="MZ_IDATE1" />
                            <asp:BoundField DataField="MZ_ITIME1" HeaderText="時分" SortExpression="MZ_ITIME1" />
                            <asp:BoundField DataField="MZ_ODATE" HeaderText="請假日迄" SortExpression="MZ_ODATE" />
                            <asp:BoundField DataField="MZ_OTIME" HeaderText="時分" SortExpression="MZ_OTIME" />
                            <asp:BoundField DataField="MZ_TDAY" HeaderText="天數" SortExpression="MZ_TDAY" />
                            <asp:BoundField DataField="MZ_TTIME" HeaderText="時數" SortExpression="MZ_TTIME" />
                        </Columns>
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <EditRowStyle BackColor="#2461BF" />
                        <AlternatingRowStyle BackColor="White" />
                    </cc1:TBGridView>
                    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                        ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" 
                       >
                        
                        
                        <SelectParameters> <%--SelectCommand="SELECT (CASE WHEN SIGN_KIND=1 THEN '紙本' ELSE '線上' END) SIGN_KIND,MZ_CODE,MZ_ID,MZ_CHK1,(SELECT MZ_NAME FROM A_DLBASE WHERE MZ_ID=C_DLTB01.MZ_ID) AS MZ_NAME,(SELECT MZ_CNAME FROM C_DLCODE WHERE MZ_CODE=C_DLTB01.MZ_CODE) AS MZ_CODE_NAME,MZ_CAUSE,MZ_RNAME,CONVERT(VARCHAR(200),dbo.to_number(dbo.SUBSTR(MZ_IDATE1,1,3)))+'/'+dbo.SUBSTR(MZ_IDATE1,4,2)+'/'+dbo.SUBSTR(MZ_IDATE1,6,2) AS MZ_IDATE1,MZ_ITIME1,CONVERT(VARCHAR(200),dbo.to_number(dbo.SUBSTR(MZ_ODATE,1,3)))+'/'+dbo.SUBSTR(MZ_ODATE,4,2)+'/'+dbo.SUBSTR(MZ_ODATE,6,2) AS MZ_ODATE,MZ_OTIME,MZ_TDAY,MZ_TTIME FROM C_DLTB01 WHERE MZ_EXAD=@MZ_EXAD AND (MZ_CHK1&lt;&gt;'Y' OR MZ_CHK1 IS NULL)  ORDER BY MZ_IDATE1 DESC"--%>
                            <asp:SessionParameter Name="MZ_EXAD" SessionField="ADPMZ_EXAD" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </asp:Panel>
            </asp:Panel>
            <table style="background-color: #6699FF; color: White; width: 100%">
                <tr>
                    <td>
                        <asp:Button ID="btSearch" runat="server" Text="查詢" OnClick="btSearch_Click" CssClass="KEY_IN_BUTTON_BLUE" />
                        <asp:Button ID="Button_Check" runat="server" Text="確定" OnClick="Button_Check_Click"
                            CssClass="KEY_IN_BUTTON_BLUE" />
                        &nbsp;<asp:Button ID="btnException" runat="server" onclick="btnException_Click" 
                            onclientclick="return confirm(&quot;重行確認核定狀況？&quot;)" Text="排除" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
