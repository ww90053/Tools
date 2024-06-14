<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="C_ForLeaveOvertime_ONDUTYHOUR_KEYIN.aspx.cs" Inherits="TPPDDB._3_forleave.C_ForLeaveOvertime_ONDUTYHOUR_KEYIN" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="../styles/Stylesheet.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel_ONDUTYHOUR_KEYIN" runat="server">
                <table class="style10">
                    <tr>
                        <td class="style8">
                            <asp:Label ID="Label2" runat="server"></asp:Label>
                        </td>
                        <td class="style4">
                            <asp:Label ID="Label1" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table class="style6" border="1">
                    <tr>
                        <td class="style7">
                            輪值類型
                        </td>
                        <td class="style3">
                        <asp:DropDownList ID="ddl_type" runat="server" 
                                onselectedindexchanged="ddl_type_SelectedIndexChanged"  AutoPostBack="true" />
                           <%-- <asp:RadioButton ID="RadioButton1" runat="server" Text="一般輪值" />--%>
                        </td>
                        <td class="style7">
                            值勤別
                        </td>
                        <td class="style3">
                        <asp:DropDownList ID="ddl_Kind" runat="server"   AutoPostBack="true"
                                onselectedindexchanged="ddl_Kind_SelectedIndexChanged">
                               
                            </asp:DropDownList>
                        
                           <%-- <asp:DropDownList ID="DropDownList_MZ_ONDUTY_KIND" runat="server">
                                <asp:ListItem></asp:ListItem>
                                <asp:ListItem Value="1">一般輪值</asp:ListItem>
                            </asp:DropDownList>--%>
                        </td>
                        <td class="style7">
                            目前值日費
                        </td>
                        <td class="style3">
                            <asp:Label ID="lb_PAY" runat="server" Text="Label"></asp:Label>
                            <%--<asp:DropDownList ID="DropDownList_MZ_ONDUTY_PAY" runat="server">
                                <asp:ListItem>250</asp:ListItem>
                            </asp:DropDownList>--%>
                        </td>
                    </tr>
                </table>
                <table class="style6" border="1">
                    <tr>
                        <td class="style7">
                            輪值月份
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_DUTYMONTH" runat="server" Width="100px"></asp:TextBox>
                        </td>
                        <td class="style5">
                            身分證號
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_ID" runat="server" Width="100px" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_ID_TextChanged"></asp:TextBox>
                        </td>
                        <td class="style1">
                            姓名
                        </td>
                        <td class="style3">
                            <asp:DropDownList ID="DropDownList_MZ_NAME" runat="server" AutoPostBack="True" DataSourceID="SqlDataSource2"
                                DataTextField="MZ_NAME" DataValueField="MZ_ID" OnSelectedIndexChanged="DropDownList_MZ_NAME_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                                ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT MZ_ID,MZ_NAME FROM A_DLBASE WHERE MZ_EXAD=@MZ_EXAD AND MZ_EXUNIT=@MZ_EXUNIT" DataSourceMode="DataReader">
                                <SelectParameters>
                                    <asp:SessionParameter Name="MZ_EXAD" SessionField="ADPMZ_EXAD" />
                                    <asp:SessionParameter Name="MZ_EXUNIT" SessionField="ADPMZ_EXUNIT" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </td>
                        <td class="style1">
                            單位
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_UNIT" runat="server" Width="105px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                 <asp:Panel ID="Panel_TextBox" runat="server">
                <table class="style6" border="1">
                    <tr>
                        <td class="style15">
                            1
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox1" runat="server" Width="35px" Style="height: 19px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="TextBox1_FilteredTextBoxExtender" runat="server"
                                Enabled="True" FilterType="Numbers" TargetControlID="TextBox1">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td class="style15">
                            2
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox2" runat="server" Width="35px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                FilterType="Numbers" TargetControlID="TextBox2">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td class="style15">
                            3
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox3" runat="server" Width="35px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                FilterType="Numbers" TargetControlID="TextBox3">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td class="style15">
                            4
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox4" runat="server" Width="35px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True"
                                FilterType="Numbers" TargetControlID="TextBox4">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td class="style15">
                            5
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox5" runat="server" Width="35px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True"
                                FilterType="Numbers" TargetControlID="TextBox5">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td class="style15">
                            6
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox6" runat="server" Width="35px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" Enabled="True"
                                FilterType="Numbers" TargetControlID="TextBox6">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td class="style15">
                            7
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox7" runat="server" Width="35px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" Enabled="True"
                                FilterType="Numbers" TargetControlID="TextBox7">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td class="style15">
                            8
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox8" runat="server" Width="35px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" Enabled="True"
                                FilterType="Numbers" TargetControlID="TextBox8">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td class="style15">
                            9
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox9" runat="server" Width="35px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" Enabled="True"
                                FilterType="Numbers" TargetControlID="TextBox9">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td class="style15">
                            10
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox10" runat="server" Width="35px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" Enabled="True"
                                FilterType="Numbers" TargetControlID="TextBox10">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td class="style15">
                            11
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox11" runat="server" Width="35px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" Enabled="True"
                                FilterType="Numbers" TargetControlID="TextBox11">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td class="style15">
                            12
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox12" runat="server" Width="35px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" Enabled="True"
                                FilterType="Numbers" TargetControlID="TextBox12">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td class="style15">
                            13
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox13" runat="server" Width="35px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" Enabled="True"
                                FilterType="Numbers" TargetControlID="TextBox13">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td class="style15">
                            14
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox14" runat="server" Width="35px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" runat="server" Enabled="True"
                                FilterType="Numbers" TargetControlID="TextBox14">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td class="style15">
                            15
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox15" runat="server" Width="35px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender14" runat="server" Enabled="True"
                                FilterType="Numbers" TargetControlID="TextBox15">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td class="style15">
                            16
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox16" runat="server" Width="35px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender15" runat="server" Enabled="True"
                                FilterType="Numbers" TargetControlID="TextBox16">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td class="style15">
                            17
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox17" runat="server" Width="35px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender16" runat="server" Enabled="True"
                                FilterType="Numbers" TargetControlID="TextBox17">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td class="style15">
                            18
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox18" runat="server" Width="35px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender17" runat="server" Enabled="True"
                                FilterType="Numbers" TargetControlID="TextBox18">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td class="style15">
                            19
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox19" runat="server" Width="35px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender18" runat="server" Enabled="True"
                                FilterType="Numbers" TargetControlID="TextBox19">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td class="style15">
                            20
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox20" runat="server" Width="35px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender19" runat="server" Enabled="True"
                                FilterType="Numbers" TargetControlID="TextBox20">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td class="style15">
                            21
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox21" runat="server" Width="35px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender20" runat="server" Enabled="True"
                                FilterType="Numbers" TargetControlID="TextBox21">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td class="style15">
                            22
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox22" runat="server" Width="35px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender21" runat="server" Enabled="True"
                                FilterType="Numbers" TargetControlID="TextBox22">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td class="style15">
                            23
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox23" runat="server" Width="35px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender22" runat="server" Enabled="True"
                                FilterType="Numbers" TargetControlID="TextBox23">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td class="style15">
                            24
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox24" runat="server" Width="35px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender23" runat="server" Enabled="True"
                                FilterType="Numbers" TargetControlID="TextBox24">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td class="style15">
                            25
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox25" runat="server" Width="35px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender24" runat="server" Enabled="True"
                                FilterType="Numbers" TargetControlID="TextBox25">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td class="style15">
                            26
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox26" runat="server" Width="35px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender25" runat="server" Enabled="True"
                                FilterType="Numbers" TargetControlID="TextBox26">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td class="style15">
                            27
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox27" runat="server" Width="35px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender26" runat="server" Enabled="True"
                                FilterType="Numbers" TargetControlID="TextBox27">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td class="style15">
                            28
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox28" runat="server" Width="35px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender27" runat="server" Enabled="True"
                                FilterType="Numbers" TargetControlID="TextBox28">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td class="style15">
                            29
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox29" runat="server" Width="35px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender28" runat="server" Enabled="True"
                                FilterType="Numbers" TargetControlID="TextBox29">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td class="style15">
                            30
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox30" runat="server" Width="35px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender29" runat="server" Enabled="True"
                                FilterType="Numbers" TargetControlID="TextBox30">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td class="style15">
                            31
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox31" runat="server" Width="35px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender30" runat="server" Enabled="True"
                                FilterType="Numbers" TargetControlID="TextBox31">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                </table>
                </asp:Panel>
                <table class="style6" border="1">
                    <tr>
                        <td class="style7">
                            共合計：
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_TOTAL" runat="server"></asp:TextBox>
                        </td>
                        <td class="style7">
                            備考
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_REMARK" runat="server"></asp:TextBox>
                        </td>
                        <td width="200px">
                            請輸入--> 1 代表正值 2 代表副值
                        </td>
                    </tr>
                </table>
                <table class="style6" border="1">
                    <tr>
                        <td class="style7">
                            備註
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox32" runat="server" TextMode="MultiLine" Width="745px" Height="78px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <div style="width: 790px; text-align: left; overflow: auto; height: 150px;">
                <asp:GridView ID="GridView1" Width="1000px" runat="server" CellPadding="4" GridLines="None"
                    AutoGenerateColumns="False" DataSourceID="SqlDataSource1" OnRowCommand="GridView1_RowCommand"
                    OnRowDataBound="GridView1_RowDataBound" DataKeyNames="MZ_ID,MZ_DUTYMONTH" ForeColor="#333333">
                    <RowStyle BackColor="#EFF3FB" />
                    <Columns>
                        <asp:BoundField DataField="MZ_NAME" HeaderText="姓名" SortExpression="MZ_NAME">
                            <ItemStyle Width="75px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="MZ_DUTYMONTH" HeaderText="月份" SortExpression="MZ_DUTYMONTH">
                            <ItemStyle Width="75px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="MZ_ONDUTY_KIND" HeaderText="值勤方式" SortExpression="MZ_ONDUTY_KIND">
                            <ItemStyle Width="75px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="1" HeaderText="1" SortExpression="1" />
                        <asp:BoundField DataField="2" HeaderText="2" SortExpression="2" />
                        <asp:BoundField DataField="3" HeaderText="3" SortExpression="3" />
                        <asp:BoundField DataField="4" HeaderText="4" SortExpression="4" />
                        <asp:BoundField DataField="5" HeaderText="5" SortExpression="5" />
                        <asp:BoundField DataField="6" HeaderText="6" SortExpression="6" />
                        <asp:BoundField DataField="7" HeaderText="7" SortExpression="7" />
                        <asp:BoundField DataField="8" HeaderText="8" SortExpression="8" />
                        <asp:BoundField DataField="9" HeaderText="9" SortExpression="9" />
                        <asp:BoundField DataField="10" HeaderText="10" SortExpression="10" />
                        <asp:BoundField DataField="11" HeaderText="11" SortExpression="11" />
                        <asp:BoundField DataField="12" HeaderText="12" SortExpression="12" />
                        <asp:BoundField DataField="13" HeaderText="13" SortExpression="13" />
                        <asp:BoundField DataField="14" HeaderText="14" SortExpression="14" />
                        <asp:BoundField DataField="15" HeaderText="15" SortExpression="15" />
                        <asp:BoundField DataField="16" HeaderText="16" SortExpression="16" />
                        <asp:BoundField DataField="17" HeaderText="17" SortExpression="17" />
                        <asp:BoundField DataField="18" HeaderText="18" SortExpression="18" />
                        <asp:BoundField DataField="19" HeaderText="19" SortExpression="19" />
                        <asp:BoundField DataField="20" HeaderText="20" SortExpression="20" />
                        <asp:BoundField DataField="21" HeaderText="21" SortExpression="21" />
                        <asp:BoundField DataField="22" HeaderText="22" SortExpression="22" />
                        <asp:BoundField DataField="23" HeaderText="23" SortExpression="23" />
                        <asp:BoundField DataField="24" HeaderText="24" SortExpression="24" />
                        <asp:BoundField DataField="25" HeaderText="25" SortExpression="25" />
                        <asp:BoundField DataField="26" HeaderText="26" SortExpression="26" />
                        <asp:BoundField DataField="27" HeaderText="27" SortExpression="27" />
                        <asp:BoundField DataField="28" HeaderText="28" SortExpression="28" />
                        <asp:BoundField DataField="29" HeaderText="29" SortExpression="29" />
                        <asp:BoundField DataField="30" HeaderText="30" SortExpression="30" />
                        <asp:BoundField DataField="31" HeaderText="31" SortExpression="31" />
                        <asp:BoundField DataField="MZ_ID" HeaderText="MZ_ID" SortExpression="MZ_ID" />
                        <asp:ButtonField CommandName="SELECT" Text="按鈕" />
                    </Columns>
                    <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#2461BF" />
                    <AlternatingRowStyle BackColor="White" />
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                    ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT (SELECT MZ_NAME FROM A_DLBASE WHERE MZ_ID=C_ONDUTY_HOUR.MZ_ID) AS MZ_NAME,MZ_ID, MZ_YEAR+MZ_MONTH AS MZ_DUTYMONTH, &quot;MZ_ONDUTY_KIND&quot;, &quot;1&quot; AS &quot;1&quot;, &quot;2&quot; AS &quot;2&quot;, &quot;3&quot; AS &quot;3&quot;, &quot;4&quot; AS &quot;4&quot;, &quot;5&quot; AS &quot;5&quot;, &quot;6&quot; AS &quot;6&quot;, &quot;7&quot; AS &quot;7&quot;, &quot;8&quot; AS &quot;8&quot;, &quot;9&quot; AS &quot;9&quot;, &quot;10&quot; AS &quot;10&quot;, &quot;11&quot; AS &quot;11&quot;, &quot;12&quot; AS &quot;12&quot;, &quot;13&quot; AS &quot;13&quot;, &quot;14&quot; AS &quot;14&quot;, &quot;15&quot; AS &quot;15&quot;, &quot;16&quot; AS &quot;16&quot;, &quot;17&quot; AS &quot;17&quot;, &quot;18&quot; AS &quot;18&quot;, &quot;19&quot; AS &quot;19&quot;, &quot;20&quot; AS &quot;20&quot;, &quot;21&quot; AS &quot;21&quot;, &quot;22&quot; AS &quot;22&quot;, &quot;23&quot; AS &quot;23&quot;, &quot;24&quot; AS &quot;24&quot;, &quot;25&quot; AS &quot;25&quot;, &quot;26&quot; AS &quot;26&quot;, &quot;27&quot; AS &quot;27&quot;, &quot;28&quot; AS &quot;28&quot;, &quot;29&quot; AS &quot;29&quot;, &quot;30&quot; AS &quot;30&quot;, &quot;31&quot; AS &quot;31&quot; FROM &quot;C_ONDUTY_HOUR&quot; WHERE MZ_YEAR=dbo.LPAD(dbo.TO_CHAR(GETDATE(),'YYYY')-1911,3,'0') AND MZ_MONTH =dbo.TO_CHAR(GETDATE(),'MM') AND MZ_ID IN (SELECT MZ_ID FROM A_DLBASE WHERE MZ_EXAD = @MZ_EXAD) AND MZ_ID IN (SELECT MZ_ID FROM A_DLBASE WHERE MZ_EXUNIT = @MZ_EXUNIT) ORDER BY MZ_ID ">
                    <SelectParameters>
                        <asp:SessionParameter Name="MZ_EXAD" SessionField="ADPMZ_EXAD" />
                        <asp:SessionParameter Name="MZ_EXUNIT" SessionField="ADPMZ_EXUNIT" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
            <asp:Panel ID="Panel1" runat="server">
                <table style="background-color: #6699FF; color: White; width: 100%">
                    <tr>
                        <td class="style34">
                            <asp:Button ID="btSearch" runat="server" meta:resourcekey="btP37_DLBASETableResource1"
                                OnClick="btSearch_Click" Text="查詢" CausesValidation="False" class="style9" />
                            <asp:Button ID="btInsert" runat="server" meta:resourcekey="btInsertResource1" OnClick="btInsert_Click"
                                Text="新增" CausesValidation="False" class="style9" AccessKey="a" />
                            <asp:Button ID="btUpdate" runat="server" OnClick="btUpdate_Click" Text="修改" CausesValidation="False"
                                Enabled="False" class="style9" />
                            <asp:Button ID="btOK" runat="server" meta:resourcekey="btOKResource1" OnClick="btOK_Click"
                                Text="確定" Enabled="False" class="style9" AccessKey="s" />
                            <asp:Button ID="btCancel" runat="server" CausesValidation="False" meta:resourcekey="btCancelResource1"
                                OnClick="btCancel_Click" Text="取消" Enabled="False" class="style9" />
                            <asp:Button ID="btDelete" runat="server" meta:resourcekey="btDeleteResource1" OnClick="btDelete_Click"
                                Text="刪除" CausesValidation="False" OnClientClick="return confirm(&quot;確定刪除？&quot;);"
                                Enabled="False" class="style9" AccessKey="d" />
                            <asp:Button ID="btUpper" runat="server" CausesValidation="False" Enabled="False"
                                meta:resourcekey="btUpperResource1" OnClick="btUpper_Click" Text="上一筆" class="style9" />
                            <asp:Label ID="xcount" runat="server" meta:resourcekey="xcountResource1" Visible="False"></asp:Label>
                            <asp:Button ID="btNEXT" runat="server" CausesValidation="False" Enabled="False" meta:resourcekey="btNEXTResource1"
                                OnClick="btNEXT_Click" Text="下一筆" class="style9" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
        <%--<Triggers>
        <asp:AsyncPostBackTrigger ControlID="ddl_type" />
        </Triggers>--%>
    </asp:UpdatePanel>
</asp:Content>
