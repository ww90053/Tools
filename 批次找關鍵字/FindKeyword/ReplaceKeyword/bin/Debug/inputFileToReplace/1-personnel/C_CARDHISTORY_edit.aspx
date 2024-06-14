<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true" CodeBehind="C_CARDHISTORY_edit.aspx.cs" Inherits="TPPDDB._15_score.C_CARDHISTORY_edit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <style type="text/css">
        .tdstyle
        {
            border: solid 1px #999999;
        }
        .auto-style1 {
            font-size: small;
            width: 21%;
        }
        .auto-style2 {
            border: solid 1px #999999;
            width: 21%;
        }
        .auto-style3 {
            width: 21%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--  <asp:SqlDataSource ID="SqlDataSource_AD" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
        ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT RTRIM(MZ_KCHI) AS MZ_KCHI, RTRIM(MZ_KCODE) AS MZ_KCODE FROM A_KTYPE WHERE (RTRIM(MZ_KTYPE) = '04') AND (RTRIM(MZ_KCODE) LIKE '38213%')  order by mz_kcode">
    </asp:SqlDataSource>--%>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="title_s1"></div>
            <asp:Panel ID="panel_Apply" runat="server" Width="85%" runat="server" BackColor="#cccccc"
                BorderColor="#999999" BorderStyle="Double" BorderWidth="5px">
                <table id="tb1" width ="100%">
                    <tr>
                        <td style="text-align: right; " class ="auto-style2">
                            申請日期：
                        </td>
                        <td style="text-align: left; width :80%" class ="tdstyle" colspan="3" >
                            <asp:TextBox ID="txt_Date" runat="server" AutoPostBack="True" OnTextChanged="txt_Date_TextChanged"></asp:TextBox>
                            <cc1:CalendarExtender ID="txt_Date_CalendarExtender" runat="server" Format="yyyy/MM/dd"
                                Enabled="True" TargetControlID="txt_Date">
                            </cc1:CalendarExtender>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                ControlToValidate="txt_Date" ErrorMessage="不可空白"></asp:RequiredFieldValidator>
                            <asp:DropDownList ID="DropDownList_Type" runat="server"  AutoPostBack="True" OnSelectedIndexChanged="DropDownList_Type_SelectedIndexChanged">
                                <asp:ListItem Selected="True" Text=""></asp:ListItem>
                                <asp:ListItem Text="上班">上班</asp:ListItem>
                                <asp:ListItem Text="下班">下班</asp:ListItem>
                                <asp:ListItem Text="上下班">上下班</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;" class ="auto-style2">
                            服務機關：
                        </td>
                        <td style="text-align: left;" class ="tdstyle">
                            <asp:DropDownList ID="DropDownList_MZ_AD" runat="server" DataSourceID="SqlDataSource_AD"
                                DataTextField="MZ_KCHI" DataValueField="MZ_KCODE" AutoPostBack="True" OnSelectedIndexChanged="DropDownList_AD_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="SqlDataSource_AD" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                                ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT RTRIM(MZ_KCHI) AS MZ_KCHI, RTRIM(MZ_KCODE) AS MZ_KCODE FROM A_KTYPE WHERE (RTRIM(MZ_KTYPE) = '04') AND (RTRIM(MZ_KCODE) LIKE '38213%') ORDER BY MZ_KCODE">
                            </asp:SqlDataSource>
                        </td>
                        <td style="text-align: right;" class ="tdstyle">
                            現服單位：
                        </td>
                        <td style="text-align: left;" class ="tdstyle">
                            <asp:DropDownList ID="DropDownList_MZ_UNIT" runat="server" AppendDataBoundItems="false"
                                DataTextField="MZ_KCHI" DataValueField="MZ_KCODE"  AutoPostBack="True" OnSelectedIndexChanged="DropDownList_UNIT_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;" class ="auto-style2">
                            職稱：
                        </td>
                        <td style="text-align: left;" class ="tdstyle">
                                <asp:TextBox ID="TextBox_MZ_OCCC" runat="server"></asp:TextBox>
                        </td>
                        <td style="text-align: right;" class ="tdstyle">
                            姓名：
                        </td>
                        <td style="text-align: left;" class ="tdstyle">
                                 <asp:DropDownList ID="DropDownList_NAME" runat="server"  AutoPostBack="True" AppendDataBoundItems="true"  OnSelectedIndexChanged="txt_Date_TextChanged" ></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;" class ="auto-style2">
                            上班時間：
                        </td>
                        <td style="text-align: left;" class ="tdstyle">
                                <asp:TextBox ID="Text_INTIME" runat="server" Enabled="false"></asp:TextBox>
                        </td>
                        <td style="text-align: right;" class ="tdstyle">
                            下班時間：
                        </td>
                        <td style="text-align: left;" class ="tdstyle">
                                 <asp:TextBox ID="Text_OUTTIME" runat="server" Enabled="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                         <td style="text-align: right;" class ="auto-style2">
                            &nbsp;原因</td>
                         <td style="text-align: left;" class ="tdstyle">
                            <asp:RadioButtonList ID="rd_CTYPE" runat="server" OnSelectedIndexChanged="rd_SelectedIndexChanged"  RepeatDirection="Horizontal" AutoPostBack="true">
                                <asp:ListItem Value="0">因公未刷</asp:ListItem>
                                <asp:ListItem Value="1">忘記刷卡</asp:ListItem>
                            </asp:RadioButtonList>
                             <span id="aacount1" runat="server"></span>
                        </td>
                        <td style="text-align: right;" class ="auto-style2">
                            &nbsp;審查方式</td>
                        <td style="text-align: left;" class ="tdstyle">
                            <asp:RadioButtonList ID="radl_Check" runat="server" OnSelectedIndexChanged="radl_Check_SelectedIndexChanged" RepeatDirection="Horizontal" AutoPostBack="true">
                                <asp:ListItem Value="Online">線上</asp:ListItem>
                                <asp:ListItem Value="Paper">紙本</asp:ListItem>
                            </asp:RadioButtonList>
                            審查者:
                            <asp:DropDownList ID="ddl_CHECKER" runat="server" Enabled="false" >
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;" class ="auto-style2">
                            因公疏未辦理按卡事由
                        </td>
                        <td style="text-align: left;" class ="tdstyle" colspan="3">
                            <asp:TextBox ID="Text_MEMO" runat="server" Width="450px" MaxLength="50"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;" class ="auto-style2">
                            上傳附件1
                        </td>
                        <td style="text-align: left;" class ="tdstyle">
                            <asp:FileUpload ID="FileUpload1" runat="server" Width="150px" />
                            <asp:HyperLink ID="HyperLink_FILENAME1" runat="server" Text="查看附件" Visible="false"
                                Width="80px" Target="_blank"></asp:HyperLink>
                            <asp:Button ID="Button_DelFILE1" runat="server" Text="刪除" Visible="false" CausesValidation="false"
                                OnCommand="Button_DelFILE1_Command" Width="40px" />
                        </td>
                       <td style="text-align: right;" class ="tdstyle">
                            上傳附件2
                        </td>
                        <td style="text-align: left;" class ="tdstyle">
                            <asp:FileUpload ID="FileUpload2" runat="server" Width="150px" />
                            <asp:HyperLink ID="HyperLink_FILENAME2" runat="server" Text="查看附件" Visible="false"
                                Width="80px" Target="_blank"></asp:HyperLink>
                            <asp:Button ID="Button_DelFILE2" runat="server" Text="刪除" Visible="false" CausesValidation="false"
                                Width="40px" OnCommand="Button_DelFILE2_Command" />
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style3">
                            <asp:Button ID="btInsert" runat="server" meta:resourcekey="btInsertResource1" OnClick="btInsert_Click" Text="新增" />
                            <asp:Button ID="btOK" runat="server"  OnClick="btOK_Click" Text="簽核" Visible="false" />
                            <asp:Button ID="btNOK" runat="server"  OnClick="btNOK_Click" Text="退回" Visible="false" />
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style1"><strong>年度申請紀錄</strong></td>
                    </tr>
                </table> 
                <table width="100%">
                    <tr>
                        <td colspan="5">
                            <asp:GridView ID="gv_C_CARDHISTORY_EDIT" runat="server" Width="100%" AllowSorting="True"
                                AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None"
                                Style="font-family: 微軟正黑體;" DataKeyNames="SN" OnRowDataBound="gv_C_CARDHISTORY_EDIT_RowDataBound" OnRowCommand="gv_C_CARDHISTORY_EDIT_RowCommand">
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                <Columns>
                                    <asp:TemplateField HeaderText="原因">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_CTYPE" runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="40" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="MZ_NAME" HeaderText="申請者">
                                        <ItemStyle Width="60" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="申請日期">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_DATETIME" runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="80" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="目前狀態">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_STATUS" runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="40" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="陳核／決行人">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_MOD_USER" runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="60" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="MOD_DATE" HeaderText="陳核／決行時間">
                                        <ItemStyle Width="80" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="收件人">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_SURE_USER" runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="60" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="SURE_DATE" HeaderText="收件時間">
                                        <ItemStyle Width="80" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="BOUNCED_REASON" HeaderText="退件原因">
                                        <ItemStyle Width="80" />
                                    </asp:BoundField>
                                     <asp:TemplateField HeaderText="刪除">
                                        <ItemTemplate>
                                            <asp:Button ID="btndel" runat="server" Text="刪除" CommandName="dodel" CommandArgument='<%#Eval("SN") %>' />
                                        </ItemTemplate>
                                         <ItemStyle HorizontalAlign="Center" Width="50" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="修改">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# "C_CARDHISTORY_edit.aspx?TPM_FION=9&sn="+Eval("SN") %>'>修改</asp:HyperLink>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="50" />
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <asp:Button ID="btnRpt" runat="server" Text="列印" CommandName="doRpt" CommandArgument='<%#Eval("SN") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#999999" />
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btInsert" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
