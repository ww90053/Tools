<%@ Page Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true" 
    CodeBehind="C_ForLeaveOvertime_Inside_Overtime.aspx.cs"
    Inherits="TPPDDB._3_forleave.C_ForLeaveOvertime_Inside_Overtime"  %>

<%@ Register Assembly="TBGridView" Namespace="TPPDDB" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="../styles/Stylesheet.css" rel="stylesheet" type="text/css" />

    <script language="javascript" type="text/javascript">
        function toLocalDate(source, clientside_arguments) {
            var DateValue = source._textbox.get_Value();
            var year = parseInt(DateValue.substr(0, 4)) - 1911;
            source._textbox.set_Value(year + DateValue.substr(4, 6));
        }
    </script>

    <style type="text/css">
        .style59
        {
            text-align: left;
            width: 55px;
            color: #00查詢
            font-weight: 700;
            background-color: #FFFF66;
            font-weight: bold;
        }
        .style60
        {
            text-align: left;
            width: 46px;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
            font-weight: bold;
        }
        .style61
        {
            text-align: left;
            width: 56px;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
            font-weight: bold;
        }
        .style62
        {
            text-align: left;
            height: 20px;
            width: 357px;
        }
        .style64
        {
            text-align: left;
            height: 20px;
            width: 200px;
        }
        .style65
        {
            text-align: left;
            width: 57px;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
            font-weight: bold;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="HiddenField_MZ_ID" runat="server" />
            <table class="style10">
                <tr>
                    <td class="style8">
                        加班費管理
                    </td>
                    <td class="style4">
                     <%--   <asp:Label ID="Label1" runat="server"></asp:Label>--%>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="Panel2" runat="server">
                <table class="style6" border="1">
                    <tr>
                        <td class="style59">
                            單&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 位
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_EXUNIT" runat="server" ReadOnly="True"></asp:TextBox>
                        </td>
                        <td class="style60">
                            職&nbsp;&nbsp;&nbsp; 別
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_OCCC" runat="server" ReadOnly="True" Width="100px"></asp:TextBox>
                        </td>
                        <td class="style61">
                            員工編號
                        </td>
                        <td class="style3" colspan="3">
                            <asp:TextBox ID="TextBox_MZ_POLNO" runat="server" ReadOnly="True" MaxLength="8" Width="100px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style59">
                            姓&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 名
                        </td>
                        <td class="style3">
                            <asp:DropDownList ID="DropDownList_MZ_NAME" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList_MZ_NAME_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td class="style60">
                            身分證
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_ID" runat="server" ReadOnly="True" Width="100px"></asp:TextBox>
                        </td>
                        <td class="style61">
                            加班日期
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_DATE" runat="server" Width="75px" AutoPostBack="True"
                                OnTextChanged="TextBox_MZ_DATE_TextChanged"></asp:TextBox>
                        </td>
                        <td class="style59">
                            每時金額
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_HOUR_PAY" runat="server" ReadOnly="True" Width="60px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table class="style6" border="1">
                    <tr>
                        <td class="style59">
                            加班事由
                        </td>
                        <td class="style62">
                            <asp:TextBox ID="TextBox_OTREASON" runat="server" Width="350px" MaxLength="40"></asp:TextBox>
                        </td>
                        <td>
                            <asp:RadioButtonList ID="RadioButtonList_RESTFLAG" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Value="N" Selected="True">支領加班費</asp:ListItem>
                                <asp:ListItem Value="YO">加班補休</asp:ListItem>
                                <asp:ListItem Value="YU" Enabled = "false" style="color: darkgray;">超勤補休</asp:ListItem>
                                <asp:ListItem Value="YD">值日補休</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <table class="style12">
                <tr>
                    <td>
                        <asp:Button ID="btSearch" runat="server" meta:resourcekey="btInsertResource1" OnClick="btSearch_Click"
                            Text="查詢" Style="width: 40px" class="style9" AccessKey="a" />
                        <asp:Button ID="btInsert" runat="server" AccessKey="a" class="style9" meta:resourcekey="btInsertResource1"
                            OnClick="btInsert_Click" Style="width: 40px" Text="新增" />
                        <asp:Button ID="btUpdate" runat="server" OnClick="btUpdate_Click" Text="修改" Enabled="False"
                            class="style9" />
                        <asp:Button ID="btDelete" runat="server" meta:resourcekey="btDeleteResource1" OnClick="btDelete_Click"
                            Text="刪除" CausesValidation="False" OnClientClick="return confirm(&quot;確定刪除？&quot;);"
                            Enabled="False" class="style9" AccessKey="d" />
                        <asp:Button ID="btOvertimeAsk" runat="server" Text="列印加班請示單" CausesValidation="False"
                            OnClick="btOvertimeAsk_Click" class="style13" />
                        <asp:Button ID="btOvertimeDetail" runat="server" Text="加班費管制卡" CausesValidation="False"
                            OnClick="btOvertimeDetail_Click" class="style13" />
                        <asp:Button ID="btOvertimeTotal" runat="server" Text="加班費總表" OnClick="btOvertimeTotal_Click"
                            CausesValidation="False" class="style13" />
                    </td>
                </tr>
            </table>
             <%--OnPageIndexChanging="GridView1_PageIndexChanging"--%>
            <asp:Panel ID="Panel1" runat="server" Width="100%" Height="300px" style="overflow:scroll;">
                <cc1:TBGridView ID="GridView1" runat="server" Width="100%" CellPadding="4" EnableEmptyContentRender="True"
                    GridLines="None" AutoGenerateColumns="False" EmptyDataText="無資料" OnRowCommand="GridView1_RowCommand"
                    OnRowDataBound="GridView1_RowDataBound" Style="text-align: left; margin-top: 0px"
                    ForeColor="#333333">
                    <RowStyle BackColor="#EFF3FB" />
                    <Columns>
                        <asp:TemplateField HeaderText="核定">
                            <ItemTemplate>
                                <asp:CheckBox ID="CheckBox_MZ_CHK" runat="server" AutoPostBack="True" OnCheckedChanged="CheckBox_MZ_CHK_CheckedChanged" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("MZ_CHK") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemStyle HorizontalAlign="Left" Width="35px" />
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="加班日期" DataField="MZ_DATE">
                            <ItemStyle Width="70px" HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="加班事由" DataField="OTREASON">
                            <ItemStyle Width="400px" HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="加班時數">
                            <ItemTemplate>
                                <asp:TextBox ID="TextBox_OTIME" runat="server" OnTextChanged="TextBoxOTIME_TextChanged"
                                    Width="30px" AutoPostBack="True" Text='<%# Bind("OTIME") %>'></asp:TextBox>
                                <cc2:FilteredTextBoxExtender ID="TextBox_OTIME_FilteredTextBoxExtender" runat="server"
                                    Enabled="True" FilterType="Numbers" TargetControlID="TextBox_OTIME">
                                </cc2:FilteredTextBoxExtender>
                            </ItemTemplate>
                            <ItemStyle Width="75px" HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="每時金額" DataField="HOUR_PAY">
                            <ItemStyle Width="75px" HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="金額小計" DataField="PAY_SUM">
                            <ItemStyle Width="105px" HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="補休狀態" DataField="RESTFLAG">
                            <ItemStyle Width="75px" HorizontalAlign="center" />
                        </asp:BoundField>
                        <asp:CommandField ShowSelectButton="True" />
                        <asp:BoundField DataField="LOCK_FLAG" HeaderText="LOCK_FLAG" />
                    </Columns>
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#2461BF" />
                    <AlternatingRowStyle BackColor="White" />
                </cc1:TBGridView>
            </asp:Panel>
            <table class="style6">
                <tr>
                    <td class="style14">
                        支領加班費＝
                    </td>
                    <td class="style3">
                        <asp:Label ID="Label2" runat="server"></asp:Label>元
                    </td>
                    <td class="style14">
                        支領加班小時數＝
                    </td>
                    <td class="style3">
                        <asp:Label ID="Label3" runat="server"></asp:Label>小時
                    </td>
                    <td class="style14">
                        加班總小時數＝
                    </td>
                    <td class="style3">
                        <asp:Label ID="Label4" runat="server"></asp:Label>小時
                    </td>
                    <td class="style14">
                        擇日補休小時數＝
                    </td>
                    <td class="style3">
                        <asp:Label ID="Label5" runat="server"></asp:Label>小時
                    </td>
                </tr>
            </table>
        </ContentTemplate>
         
         <Triggers >
         <asp:postbacktrigger  ControlID ="btOvertimeDetail" />
            <asp:postbacktrigger  ControlID ="btOvertimeTotal" />
        </Triggers>
        
        
    </asp:UpdatePanel>
</asp:Content>
