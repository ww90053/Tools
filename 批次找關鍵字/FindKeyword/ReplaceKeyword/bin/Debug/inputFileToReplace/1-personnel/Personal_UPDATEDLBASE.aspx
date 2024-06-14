<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="Personal_UPDATEDLBASE.aspx.cs" Inherits="TPPDDB._1_personnel.Personal_UPDATEDLBASE" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="../styles/Stylesheet_1.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style32
        {
            font-size: large;
            text-align: left;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server">
                <table style="background-color: #6699FF; color: White; width: 100%; font-size: x-large;
                    font-family: 標楷體;" class="KEY_IN_TITLE_BY_SELF">
                    <tr>
                        <td class="style32">
                            任免更新基本資料
                        </td>
                    </tr>
                </table>
                <table border="1" style="width: 100%; text-align: left;">
                    <tr>
                        <td class="KEY_IN_TITLE_BY_SELF" style="width: 55px;">
                            發文號
                        </td>
                        <td>
                            <asp:DropDownList ID="DropDownList_MZ_PRID" runat="server" DataSourceID="SqlDataSource1"
                                DataTextField="MZ_PRID" DataValueField="MZ_AD">
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                                ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" 
                                SelectCommand="SELECT MZ_PRID,MZ_AD FROM A_CHKAD ORDER BY MZ_AD" DataSourceMode="DataReader">
                            </asp:SqlDataSource>
                        </td>
                    </tr>
                    <tr>
                        <td class="KEY_IN_TITLE_BY_SELF">
                            字&nbsp;&nbsp;&nbsp; 第
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox_MZ_PRID1" runat="server" MaxLength="15"></asp:TextBox>號
                        </td>
                    </tr>
                </table>
                <table style="background-color: #CCFFFF; color: White; width: 100%; text-align: center;">
                    <tr>
                        <td>
                            <asp:Button ID="btOK" runat="server" Text="確定" OnClick="btOK_Click" OnClientClick="return confirm(&quot;確定更新？&quot;);"
                                CssClass="KEY_IN_BUTTON_BLUE" />
                            <asp:Button ID="btCancel" runat="server" Text="取消" OnClick="btCancel_Click" CssClass="KEY_IN_BUTTON_BLUE" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
