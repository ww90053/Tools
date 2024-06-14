<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="PersonalPRK1IDSearch_Export.aspx.cs" Inherits="TPPDDB._1_personnel.PersonalPRK1IDSearch_Export" %>

<%@ Register src="A_UCLoading.ascx" tagname="A_UCLoading" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <style type="text/css">
        .style3
        {
            text-align: center;
        }
        .auto-style1 {
            height: 59px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" 
        AsyncPostBackTimeout="10000">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="title_s1">
                �ץXWEBHR�Ϋ�ĳ�W�U</div>
            <div>
            </div>
            <div>
                <asp:Panel ID="Panel1" runat="server">
                    <table border="1" style="width: 100%; text-align: left;">
                        <tr>
                        <td class="auto-style1">�����W�١G</td>
                        <td style="text-align: left" class="auto-style1">
                            <asp:DropDownList ID="DropDownList_EXAD" runat="server" AppendDataBoundItems="True"
                                    AutoPostBack="True" 
                                    DataSourceID="SqlDataSourcel" DataTextField="MZ_KCHI" DataValueField="MZ_KCODE" >
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="SqlDataSourcel" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                                ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" 
                                SelectCommand="SELECT MZ_KCHI,MZ_KCODE FROM A_KTYPE WHERE MZ_KCODE LIKE '38213%' AND MZ_KTYPE='04'" 
                                DataSourceMode="DataReader">
                            </asp:SqlDataSource>
                        </td>
                             </tr>
                         <tr>
                            <td class="auto-style1">
                                �׸��_:</td>
                            <td class="auto-style1">
                                <asp:TextBox ID="TextBox_MZ_NO1" runat="server" Width="120px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TextBox_MZ_NO1"
                                    Display="Dynamic" ErrorMessage="RequiredFieldValidator">���i�ť�</asp:RequiredFieldValidator>
                            </td>
                            <td class="auto-style1">
                                �׸���:</td>
                            <td class="auto-style1">
                                <asp:TextBox ID="TextBox_MZ_NO2" runat="server" Width="120px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="SEARCH_TITLE">
                                �v&nbsp;&nbsp;&nbsp;&nbsp; �d:</td>
                            <td>
                                <asp:DropDownList ID="DropDownList_MZ_SWT3" runat="server">
                                    <asp:ListItem></asp:ListItem>
                                    <asp:ListItem Value="1">1.ĵ��v�d</asp:ListItem>
                                    <asp:ListItem Value="2">2.ĵ�����v�d</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                    <table style="width: 100%; text-align: left;">
                        <tr>
                            <td class="style2">
                                <asp:Button ID="btOK" runat="server" OnClick="btOK_Click" Text="�T�w" 
                                    CssClass="SEARCH_BUTTON_BLUE" />
                                &nbsp;&nbsp;&nbsp;
                               <%-- <asp:Button ID="btLeave" runat="server" Text="���}" OnClick="btLeave_Click" 
                                    CausesValidation="False" CssClass="SEARCH_BUTTON_BLACK" />--%>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <uc1:A_UCLoading ID="A_UCLoading1" runat="server" />
            </div>
        </ContentTemplate>
                <Triggers>
            <asp:PostBackTrigger ControlID="btOK" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
