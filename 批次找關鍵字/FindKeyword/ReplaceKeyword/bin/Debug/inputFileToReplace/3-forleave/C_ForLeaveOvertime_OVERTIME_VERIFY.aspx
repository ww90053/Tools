<%@ Page Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true" CodeBehind="C_ForLeaveOvertime_OVERTIME_VERIFY.aspx.cs"
    Inherits="TPPDDB._3_forleave.C_ForLeaveOvertime_OVERTIME_VERIFY"  %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="../styles/Stylesheet.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style58
        {
            text-align: left;
            width: 41px;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
            font-weight: bold;
        }
        .style59
        {
            text-align: left;
            width: 35px;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
            font-weight: bold;
        }
        .style60
        {
            text-align: left;
            width: 42px;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
            font-weight: bold;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="text-align: left;">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div>
        </div>
        <div>
            <div>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="Panel1" runat="server">
                            <table class="style10" border="1">
                                <tr>
                                    <td class="style4">
                                        <asp:Label ID="Label1" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <table style="border-style: solid; border-width: 1px; width: 100%;" border="1">
                                <tr>
                                    <td class="style58">
                                        年度
                                    </td>
                                    <td class="style3">
                                        <asp:TextBox ID="TextBox_MZ_YEAR" runat="server" MaxLength="3" Width="70px"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="TextBox_MZ_YEAR_FilteredTextBoxExtender" runat="server"
                                            Enabled="True" FilterType="Numbers" TargetControlID="TextBox_MZ_YEAR">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <td class="style59">
                                        月份
                                    </td>
                                    <td class="style3">
                                        <asp:DropDownList ID="DropDownList_MZ_MONTH" runat="server">
                                <asp:ListItem Selected="True"></asp:ListItem>
                                <asp:ListItem Value="01">1</asp:ListItem>
                                <asp:ListItem Value="02">2</asp:ListItem>
                                <asp:ListItem Value="03">3</asp:ListItem>
                                <asp:ListItem Value="04">4</asp:ListItem>
                                <asp:ListItem Value="05">5</asp:ListItem>
                                <asp:ListItem Value="06">6</asp:ListItem>
                                <asp:ListItem Value="07">7</asp:ListItem>
                                <asp:ListItem Value="08">8</asp:ListItem>
                                <asp:ListItem Value="09">9</asp:ListItem>
                                <asp:ListItem Value="10">10</asp:ListItem>
                                <asp:ListItem Value="11">11</asp:ListItem>
                                <asp:ListItem Value="12">12</asp:ListItem>
                            </asp:DropDownList>
                                    </td>
                                    <td class="style60">
                                        機關
                                    </td>
                                    <td class="style3">
                                        <asp:DropDownList ID="DropDownList_EXAD" runat="server" AutoPostBack="True" DataSourceID="SqlDataSource_AD"
                                            DataTextField="MZ_KCHI" DataValueField="MZ_KCODE" AppendDataBoundItems="True">
                                            <asp:ListItem></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource_AD" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                                            ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" 
                                            SelectCommand="SELECT RTRIM(MZ_KCHI) AS MZ_KCHI, RTRIM(MZ_KCODE) AS MZ_KCODE FROM A_KTYPE WHERE (RTRIM(MZ_KTYPE) = '04') AND (RTRIM(MZ_KCODE) LIKE '38213%')" 
                                            DataSourceMode="DataReader">
                                        </asp:SqlDataSource>
                                    </td>
                                    <td class="style7">
                                        單位
                                    </td>
                                    <td class="style3">
                                        <asp:DropDownList ID="DropDownList_EXUNIT" runat="server" AppendDataBoundItems="false"
                                            DataSourceID="SqlDataSource_UNIT" DataTextField="RTRIM(MZ_KCHI)" DataValueField="RTRIM(MZ_KCODE)"
                                            OnDataBound="DropDownList_EXUNIT_DataBound">
                                            <asp:ListItem>請先選擇機關</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource_UNIT" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                                            ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT RTRIM(MZ_KCHI),RTRIM(MZ_KCODE) FROM A_KTYPE WHERE MZ_KTYPE='25' AND MZ_KCODE IN (SELECT MZ_UNIT FROM A_UNIT_AD WHERE MZ_AD=@MZ_AD)">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="DropDownList_EXAD" Name="MZ_AD" PropertyName="SelectedValue" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                    </td>
                                    <td>
                                        <asp:Button ID="btOK" runat="server" Text="確定" OnClick="btOK_Click" class="style9" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:GridView ID="GridView1" runat="server" Width="1500px" HeaderStyle-BackColor="Aqua"
                            AutoGenerateColumns="False" OnRowCreated="GridView1_RowCreated" Font-Size="Small"
                            DataKeyNames="MZ_ID" CellPadding="4" ForeColor="#333333" GridLines="Both">
                            <RowStyle BackColor="#EFF3FB" />
                            <Columns>
                                <asp:TemplateField HeaderText="CHECK">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Eval("MZ_VERIFY").ToString() =="Y"?true:false %>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemStyle Width="10px" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="MZ_POLNO" HeaderText="MZ_POLNO" SortExpression="MZ_POLNO">
                                    <ItemStyle Width="50px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="MZ_OCCC" HeaderText="MZ_OCCC" SortExpression="MZ_OCCC">
                                    <ItemStyle Width="55px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="MZ_NAME" HeaderText="MZ_NAME" SortExpression="MZ_NAME">
                                    <ItemStyle Width="40px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="1" HeaderText="1" SortExpression="1">
                                    <ItemStyle Width="10px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="2" HeaderText="2" SortExpression="2">
                                    <ItemStyle Width="10px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="3" HeaderText="3" SortExpression="3">
                                    <ItemStyle Width="10px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="4" HeaderText="4" SortExpression="4">
                                    <ItemStyle Width="10px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="5" HeaderText="5" SortExpression="5">
                                    <ItemStyle Width="10px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="6" HeaderText="6" SortExpression="6">
                                    <ItemStyle Width="10px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="7" HeaderText="7" SortExpression="7">
                                    <ItemStyle Width="10px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="8" HeaderText="8" SortExpression="8">
                                    <ItemStyle Width="10px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="9" HeaderText="9" SortExpression="9">
                                    <ItemStyle Width="10px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="10" HeaderText="10" SortExpression="10">
                                    <ItemStyle Width="10px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="11" HeaderText="11" SortExpression="11">
                                    <ItemStyle Width="10px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="12" HeaderText="12" SortExpression="12">
                                    <ItemStyle Width="10px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="13" HeaderText="13" SortExpression="13">
                                    <ItemStyle Width="10px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="14" HeaderText="14" SortExpression="14">
                                    <ItemStyle Width="10px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="15" HeaderText="15" SortExpression="15">
                                    <ItemStyle Width="10px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="16" HeaderText="16" SortExpression="16">
                                    <ItemStyle Width="10px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="17" HeaderText="17" SortExpression="17">
                                    <ItemStyle Width="10px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="18" HeaderText="18" SortExpression="18">
                                    <ItemStyle Width="10px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="19" HeaderText="19" SortExpression="19">
                                    <ItemStyle Width="10px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="20" HeaderText="20" SortExpression="20">
                                    <ItemStyle Width="10px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="21" HeaderText="21" SortExpression="21">
                                    <ItemStyle Width="10px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="22" HeaderText="22" SortExpression="22">
                                    <ItemStyle Width="10px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="23" HeaderText="23" SortExpression="23">
                                    <ItemStyle Width="10px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="24" HeaderText="24" SortExpression="24">
                                    <ItemStyle Width="10px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="25" HeaderText="25" SortExpression="25">
                                    <ItemStyle Width="10px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="26" HeaderText="26" SortExpression="26">
                                    <ItemStyle Width="10px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="27" HeaderText="27" SortExpression="27">
                                    <ItemStyle Width="10px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="28" HeaderText="28" SortExpression="28">
                                    <ItemStyle Width="10px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="29" HeaderText="29" SortExpression="29">
                                    <ItemStyle Width="10px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="30" HeaderText="30" SortExpression="30">
                                    <ItemStyle Width="10px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="31" HeaderText="31" SortExpression="31">
                                    <ItemStyle Width="10px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="MZ_BUDGET_HOUR" HeaderText="MZ_BUDGET_HOUR" SortExpression="MZ_BUDGET_HOUR" />
                                <asp:TemplateField HeaderText="MZ_REAL_HOUR" SortExpression="MZ_REAL_HOUR">
                                    <ItemTemplate>
                                        <asp:TextBox ID="TextBox_MZ_REAL_HOUR" runat="server" AutoPostBack="True" OnTextChanged="TextBox_MZ_REAL_HOUR_TextChanged"
                                            Text='<%# Eval("MZ_REAL_HOUR") %>' Enabled='<%# Eval("MZ_VERIFY").ToString() =="Y"?false:true %>'
                                            Width="25px"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="TextBox_MZ_REAL_HOUR_FilteredTextBoxExtender" runat="server"
                                            Enabled="True" FilterType="Numbers" TargetControlID="TextBox_MZ_REAL_HOUR">
                                        </cc1:FilteredTextBoxExtender>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("MZ_REAL_HOUR") %>' Enabled='<%# Eval("MZ_VERIFY").ToString() =="Y"?false:true %>'></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="MZ_BALANCE_HOUR" HeaderText="MZ_BALANCE_HOUR" SortExpression="MZ_BALANCE_HOUR" />
                                <asp:BoundField DataField="PAY1" HeaderText="PAY1" SortExpression="PAY1">
                                    <ItemStyle Width="80px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="PROFESS" HeaderText="PROFESS" SortExpression="PROFESS">
                                    <ItemStyle Width="80px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="BOSS" HeaderText="BOSS" SortExpression="BOSS">
                                    <ItemStyle Width="80px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="MZ_HOUR_PAY" HeaderText="MZ_HOUR_PAY" SortExpression="MZ_HOUR_PAY">
                                    <ItemStyle Width="80px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="MZ_OVERTIME_PAY" HeaderText="MZ_OVERTIME_PAY" SortExpression="MZ_OVERTIME_PAY">
                                    <ItemStyle Width="80px" />
                                </asp:BoundField>
                            </Columns>
                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <EditRowStyle BackColor="#2461BF" />
                            <AlternatingRowStyle BackColor="White" />
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
</asp:Content>
