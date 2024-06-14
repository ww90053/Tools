<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="Personal3-2.aspx.cs" Inherits="TPPDDB._1_personnel.Personal3_2" %>

<%@ Register Assembly="TBGridView" Namespace="TPPDDB" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="../styles/Stylesheet_1.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style2
        {
            width: 56px;
            text-align: left;
            background-color: #FFFF66;
        }
        .style4
        {
            text-align: left;
        }
        .style5
        {
            width: 155px;
            text-align: left;
        }
        .style7
        {
            text-align: left;
            font-weight: 700;
        }
        .style8
        {
            text-align: left;
        }
        .style9
        {
            text-align: left;
            font-size: large;
            font-family: 標楷體;
        }
        .style110
        {
        }
        .style111
        {
            width: 56px;
        }
    </style>
    <%--<script language="javascript" type="text/javascript">
        function toLocalDate(source, clientside_arguments) {
            var DateValue = source._textbox.get_Value();
            var year = parseInt(DateValue.substr(0, 4)) - 1911;
            source._textbox.set_Value(year + DateValue.substr(4, 6));
        }
    </script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel_POSIT1" runat="server">
                <table style="background-color: #6699FF; color: White; width: 100%">
                    <tr>
                        <td class="style9">
                            任免令發布主檔
                        </td>
                    </tr>
                </table>
                <table border="1" class="style1">
                    <tr>
                        <td style="text-align: left; font-weight: 700; color: #0033CC; background-color: #FF99CC"
                            class="style111">
                            發文文號
                        </td>
                        <td class="style4">
                            <asp:TextBox ID="TextBox_MZ_PRID" runat="server" ReadOnly="True"></asp:TextBox>
                        </td>
                        <td style="text-align: left; font-weight: 700; color: #0033CC; background-color: #FF99CC">
                            字&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 第
                        </td>
                        <td class="style5">
                            <asp:TextBox ID="TextBox_MZ_PRID1" runat="server" MaxLength="10" Width="100px"></asp:TextBox>
                            <cc2:FilteredTextBoxExtender ID="TextBox_MZ_PRID1_FilteredTextBoxExtender" runat="server"
                                Enabled="True" FilterType="Numbers" TargetControlID="TextBox_MZ_PRID1">
                            </cc2:FilteredTextBoxExtender>
                            號
                            </td>
                        <td style="text-align: left; font-weight: 700; color: #0033CC; background-color: #FF99CC">
                            總共謄
                        </td>
                        <td class="style8">
                            <asp:TextBox ID="TextBox_GrivewCount" runat="server" Width="100px" ReadOnly="True"></asp:TextBox>
                            筆
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; font-weight: 700; color: #0033CC; background-color: #FF99CC;"
                            class="style111">
                            發文日期
                        </td>
                        <td class="style7">
                            <asp:TextBox ID="TextBox_MZ_DATE" runat="server" Width="65px" OnTextChanged="TextBox_MZ_DATE_TextChanged"
                                AutoPostBack="True"></asp:TextBox>
                        </td>
                        <td style="text-align: left; font-weight: 700; color: #0033CC; background-color: #FF99CC">
                            生效日期
                        </td>
                        <td class="style4">
                            <asp:TextBox ID="TextBox_MZ_IDATE" runat="server" OnTextChanged="TextBox_MZ_IDATE_TextChanged"
                                Width="65px"></asp:TextBox>
                        </td>
                        <td style="text-align: left; font-weight: 700; color: #0033CC; background-color: #FF99CC">
                            到職日期
                        </td>
                        <td class="style5">
                            <asp:TextBox ID="TextBox_MZ_ADATE" runat="server" Width="65px" OnTextChanged="TextBox_MZ_ADATE_TextChanged"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style2">
                            正&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 本
                        </td>
                        <td class="style4" colspan="5">
                            <asp:TextBox ID="TextBox_MZ_EXPLAIN0" runat="server" Width="500px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style2">
                            副&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 本
                        </td>
                        <td class="style4" colspan="5">
                            <asp:TextBox ID="TextBox_MZ_EXPLAIN1" runat="server" Width="500px"></asp:TextBox>
                            <asp:Button ID="bt_EXPLAIN1" runat="server" CausesValidation="False" CssClass="style110"
                                OnClick="bt_EXPLAIN1_Click" TabIndex="-1" Text="V" />
                            <asp:Button ID="btAddNote" runat="server" CausesValidation="False" OnClick="btAddNote_Click"
                                OnClientClick="return confirm(&quot;確定新增？&quot;);" Text="新增片語" Width="60px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="style2">
                            說&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 明
                        </td>
                        <td class="style4" colspan="5">
                            <asp:TextBox ID="TextBox_TEMP_EXPLAIN" runat="server" MaxLength="50" Width="600px"
                                AutoPostBack="True" OnTextChanged="TextBox_TEMP_EXPLAIN_TextChanged"></asp:TextBox>
                            <asp:Button ID="btEXPLAIN" runat="server" CausesValidation="False" CssClass="style110"
                                OnClick="btEXPLAIN_Click" TabIndex="-1" Text="V" />
                            <asp:Button ID="btAddNote1" runat="server" CausesValidation="False" OnClick="btAddNote_Click"
                                OnClientClick="return confirm(&quot;確定新增？&quot;);" Text="新增片語" Width="60px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td class="style4" colspan="5">
                            <asp:TextBox ID="TextBox_MZ_EXPLAIN" runat="server" Height="48px" TextMode="MultiLine"
                                Width="673px" AutoPostBack="True" OnTextChanged="TextBox_MZ_EXPLAIN_TextChanged"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="Panel3" runat="server">
                <table>
                    <tr>
                        <td>
                            <asp:Button ID="Button1" runat="server" Text="任免令稿" Width="116px" OnClick="Button1_Click"
                                CausesValidation="False" Font-Bold="True" ForeColor="Red" />
                            <asp:Button ID="Button2" runat="server" Text="印任免建議函" Width="155px" OnClick="Button2_Click"
                                CausesValidation="False" Font-Bold="True" ForeColor="Red" />
                            <asp:Button ID="Button3" runat="server" Text="印任免令" Width="120px" OnClick="Button3_Click"
                                Font-Bold="True" ForeColor="Red" />
                            <asp:Button ID="Button4" runat="server" Text="依局令謄稿" OnClick="Button4_Click" CausesValidation="False"
                                Enabled="False" Font-Bold="True" ForeColor="Red" />
                            <asp:Button ID="Button5" runat="server" Text="權責單位謄稿" OnClick="Button5_Click" CausesValidation="False"
                                Enabled="False" Font-Bold="True" ForeColor="Red" />
                            <asp:Button ID="GV2_BT" runat="server" OnClick="GV2_BT_Click" Style="display: none;" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="Panel2" runat="server" Height="180px">
                <cc1:TBGridView ID="GridView1" runat="server" CellPadding="4" DataSourceID="SqlDataSource1"
                    EnableEmptyContentRender="True" GridLines="None" Width="100%" AutoGenerateColumns="False"
                    EmptyDataText="查無資料" OnDataBound="GridView1_DataBound" ForeColor="#333333">
                    <RowStyle BackColor="#EFF3FB" />
                    <Columns>
                        <asp:BoundField DataField="MZ_ID" HeaderText="身分證號" SortExpression="MZ_ID" />
                        <asp:BoundField DataField="MZ_NAME" HeaderText="姓名" SortExpression="MZ_NAME" />
                        <asp:BoundField DataField="MZ_AD" HeaderText="機關" SortExpression="MZ_AD" />
                        <asp:BoundField DataField="MZ_UNIT" HeaderText="單位" SortExpression="MZ_UNIT" />
                        <asp:BoundField DataField="MZ_OCCC" HeaderText="職稱" SortExpression="MZ_OCCC" />
                        <asp:BoundField DataField="MZ_SRANK" HeaderText="薪資職等" SortExpression="MZ_SRANK" />
                    </Columns>
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#2461BF" />
                    <AlternatingRowStyle BackColor="White" />
                </cc1:TBGridView>
                <asp:GridView ID="GridView2" runat="server" CellPadding="4" GridLines="None" Width="100%"
                    AutoGenerateColumns="False" OnDataBound="GridView2_DataBound" OnPageIndexChanging="GridView2_PageIndexChanging"
                    Visible="False" ForeColor="#333333">
                    <RowStyle BackColor="#EFF3FB" />
                    <Columns>
                        <asp:BoundField DataField="MZ_ID" HeaderText="身分證號" SortExpression="MZ_ID" />
                        <asp:BoundField DataField="MZ_NAME" HeaderText="姓名" SortExpression="MZ_NAME" />
                        <asp:BoundField DataField="MZ_AD" HeaderText="機關" SortExpression="MZ_AD" />
                        <asp:BoundField DataField="MZ_UNIT" HeaderText="單位" SortExpression="MZ_UNIT" />
                        <asp:BoundField DataField="MZ_OCCC" HeaderText="職稱" SortExpression="MZ_OCCC" />
                        <asp:BoundField DataField="MZ_SRANK" HeaderText="薪資職等" SortExpression="MZ_SRANK" />
                    </Columns>
                    <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#2461BF" />
                    <AlternatingRowStyle BackColor="White" />
                </asp:GridView>
            </asp:Panel>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT MZ_NO,MZ_ID,MZ_NAME,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE = '04' AND MZ_KCODE = MZ_AD) AS MZ_AD,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE = '25' AND MZ_KCODE =MZ_UNIT ) AS MZ_UNIT
                    ,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE = '26' AND MZ_KCODE=MZ_OCCC) AS MZ_OCCC,(SELECT MZ_KCHI FROM A_KTYPE WHERE MZ_KTYPE = '09' AND MZ_KCODE=MZ_SRANK) AS MZ_SRANK FROM A_POSIT2  WHERE MZ_PRID=@MZ_PRID AND MZ_PRID1=@MZ_PRID1 ORDER BY MZ_PRID,MZ_PRID1">
                <SelectParameters>
                    <asp:ControlParameter ControlID="TextBox_MZ_PRID" Name="MZ_PRID" PropertyName="Text" />
                    <asp:ControlParameter ControlID="TextBox_MZ_PRID1" Name="MZ_PRID1" PropertyName="Text" />
                </SelectParameters>
            </asp:SqlDataSource>
            <asp:Panel ID="Panel1" runat="server">
                <table style="background-color: #6699FF; color: White; width: 100%">
                    <tr  bgcolor="#CCFFFF">
                        <td>
                            <asp:Button ID="btSearch" runat="server" Text="查詢" OnClick="btSearch_Click" 
                                CausesValidation="False" CssClass="KEY_IN_BUTTON_BLUE" />
                            <asp:Button ID="btInsert" runat="server" Text="新增" Width="40px" OnClick="btInsert_Click"
                                CausesValidation="False" CssClass="KEY_IN_BUTTON_BLUE" AccessKey="a" />
                            <asp:Button ID="btUpdate" runat="server" Text="修改" OnClick="btUpdate_Click" Enabled="False"
                                CausesValidation="False" CssClass="KEY_IN_BUTTON_BLUE" />
                            <asp:Button ID="btOK" runat="server" OnClick="btok_Click" Text="確認" 
                                Enabled="False" CssClass="KEY_IN_BUTTON_BLUE" AccessKey="s" />
                            <asp:Button ID="btDelete" runat="server" Text="刪除" OnClick="btDelete_Click"
                                Enabled="False" CausesValidation="False" CssClass="KEY_IN_BUTTON_BLUE" 
                                AccessKey="d" />
                            <asp:Button ID="btCancel" runat="server" CausesValidation="False" meta:resourcekey="btCancelResource1"
                                OnClick="btCancel_Click" Text="取消" Enabled="False" 
                                CssClass="KEY_IN_BUTTON_BLUE" />
                            <asp:Button ID="btUpper" runat="server" CausesValidation="False" Enabled="False"
                                meta:resourcekey="btUpperResource1" OnClick="btUpper_Click" Text="上一筆" 
                                CssClass="KEY_IN_BUTTON_BLUE" />
                            <asp:Label ID="xcount" runat="server" meta:resourcekey="xcountResource1" Visible="False"></asp:Label>
                            <asp:Button ID="btNEXT" runat="server" CausesValidation="False" Enabled="False" meta:resourcekey="btNEXTResource1"
                                OnClick="btNEXT_Click" Text="下一筆" CssClass="KEY_IN_BUTTON_BLUE" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
