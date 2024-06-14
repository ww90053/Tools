<%@ Page Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true" CodeBehind="C_ForLeaveOvertime_Otheradd.aspx.cs"
    Inherits="TPPDDB._3_forleave.C_ForLeaveOvertime_Otheradd" %>

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
                                    <td style="text-align: left;" class="style4">
                                        <asp:Label ID="Label1" runat="server"></asp:Label>
                                        <!--<asp:Button ID="btnCheck" runat="server" ForeColor="#0033cc" Text="審核" OnClientClick="return confirm('確定審核？');"
                                            OnClick="btClick" />
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;          
                                            <asp:Button ID="btnDelete" runat="server" ForeColor="#0033cc" Text="刪除" OnClientClick="return confirm('確定刪除？');"
                                            OnClick="btDeleteClick" />-->
                                    </td>
                                    <%--<td class="style4">
                                        <asp:Label ID="Label1" runat="server"></asp:Label>
                                    </td>--%>
                                </tr>
                            </table>
                            <table style="border-style: solid; border-width: 1px; width: 100%;" border="1">
                                <tr>
                                    <td class="style58">
                                        年度
                                    </td>
                                    <td class="style3">
                                        <asp:TextBox ID="TextBox_MZ_YEAR" runat="server" MaxLength="3" Width="50px"></asp:TextBox>
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
                                        現服機關
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
                                        現服單位
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
                                     <td class="style58">
                                        身分證號
                                    </td>
                                    <td class="style3">
                                        <asp:TextBox ID="MZ_ID" runat="server" MaxLength="10" Width="100px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Button ID="btOK" runat="server" Text="查詢儲存" OnClick="btOK_Click" class="style9" />
                                        <asp:Button ID="btOK1" runat="server" Text="晉級補發" OnClick="btOK1_Click" class="style9" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <div style="width:800px; height:400px;overflow:auto;">
                            <asp:GridView ID="GridView1" runat="server" Width="1000px" HeaderStyle-BackColor="Aqua"
                                AutoGenerateColumns="False" OnRowCreated="GridView1_RowCreated" Font-Size="Small"
                                DataKeyNames="MZ_ID" CellPadding="4" ForeColor="#333333">
                                <RowStyle BackColor="#EFF3FB" />
                                <Columns>
                                    <asp:BoundField DataField="MZ_ID" HeaderText="MZ_ID" SortExpression="MZ_ID">
                                        <ItemStyle Width="40px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MZ_OCCC" HeaderText="MZ_OCCC" SortExpression="MZ_OCCC">
                                        <ItemStyle Width="40px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MZ_NAME" HeaderText="MZ_NAME" SortExpression="MZ_NAME">
                                        <ItemStyle Width="40px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MZ_BUDGET_HOUR" HeaderText="MZ_BUDGET_HOUR" SortExpression="MZ_BUDGET_HOUR">
                                        <ItemStyle Width="40px" />
                                    </asp:BoundField>
                                   <asp:BoundField HeaderText="TOTAL" DataField="TOTAL" SortExpression="TOTAL">
                                   <ItemStyle Width="30px" /> 
                                   </asp:BoundField>
                                     <asp:TemplateField HeaderText="MZ_REMARK" SortExpression="MZ_REMARK">
                                        <ItemTemplate>
                                            <asp:TextBox ID="TXT_MZ_REMARK" runat="server" OnTextChanged="TextBox_MZ_REMARK_TextChanged"  
                                                Width="96px" Text='<%# Eval("MZ_REMARK_UP") %>'></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Width="100px" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="PAY1"  HeaderText="PAY1" SortExpression="PAY1">
                                        <ItemStyle Width="40px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="PROFESS" HeaderText="PROFESS" SortExpression="PROFESS">
                                        <ItemStyle Width="30px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="BOSS" DataField="BOSS"  SortExpression="BOSS">
                                       <ItemStyle Width="30px" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="MZ_HOUR_PAY" SortExpression="MZ_HOUR_PAY">
                                        <ItemTemplate>
                                            <asp:TextBox ID="HOUR_PAY" runat="server" AutoPostBack="True" 
                                                Width="40px" Text='<%# Eval("MZ_HOUR_PAY") %>' Enabled='false'></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Width="40px" />
                                    </asp:TemplateField>
                                     <asp:BoundField DataField="MZ_OVERTIME_PAY" HeaderText="MZ_OVERTIME_PAY" SortExpression="MZ_OVERTIME_PAY">
                                        <ItemStyle Width="30px" />
                                    </asp:BoundField>
                                     <asp:TemplateField HeaderText="PAY1_UP" SortExpression="PAY1_UP">
                                        <ItemTemplate>
                                        <asp:TextBox ID="TextBox_PAY1_UP" runat="server" Width="40px" Text='<%# Eval("PAY1_UP") %>'  OnTextChanged="TextBox_MZ_REMARK_TextChanged"></asp:TextBox>
                                     </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="PROFESS_UP" SortExpression="PROFESS_UP">
                                        <ItemTemplate>
                                        <asp:TextBox ID="TextBox_PROFESS_UP" runat="server" Width="40px" Text='<%# Eval("PROFESS_UP") %>'   OnTextChanged="TextBox_MZ_REMARK_TextChanged"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="BOSS_UP" SortExpression="BOSS_UP">
                                        <ItemTemplate>
                                       <asp:TextBox ID="TextBox_BOSS_UP" runat="server" Width="40px" Text='<%# Eval("BOSS_UP") %>'   OnTextChanged="TextBox_MZ_REMARK_TextChanged"></asp:TextBox>
                                     </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="MZ_HOUR_PAY_UP" SortExpression="MZ_HOUR_PAY_UP">
                                        <ItemTemplate>
                                            <asp:TextBox ID="HOUR_PAY_UP" runat="server" AutoPostBack="True" 
                                                Width="40px" Text='<%# Eval("MZ_HOUR_PAY_UP") %>' Enabled='false'></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Width="40px" />
                                    </asp:TemplateField>
                                     <asp:BoundField DataField="MZ_OVERTIME_PAY_UP" HeaderText="MZ_OVERTIME_PAY_UP" SortExpression="MZ_OVERTIME_PAY_UP">
                                        <ItemStyle Width="50px" />
                                    </asp:BoundField>
                                     <asp:BoundField DataField="EFF_HOUR_UP" HeaderText="EFF_HOUR_UP" SortExpression="EFF_HOUR_UP">
                                        <ItemStyle Width="40px" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="MZ_OVERTIME_PAY1" SortExpression="MZ_OVERTIME_PAY1">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_MZ_OVERTIME_PAY1" runat="server"  OnTextChanged="TextBox_MZ_REMARK_TextChanged" 
                                                Width="50px" Text='<%# Eval("MZ_OVERTIME_PAY1") %>'></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Width="50px" />
                                    </asp:TemplateField>
                                     <%-- <asp:BoundField DataField="MZ_OVERTIME_PAY1" HeaderText="MZ_OVERTIME_PAY1" SortExpression="MZ_OVERTIME_PAY1">
                                        <ItemStyle Width="50px" />
                                    </asp:BoundField>--%>
                                </Columns>
                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#2461BF" />
                                <AlternatingRowStyle BackColor="White" />
                            </asp:GridView>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
</asp:Content>
