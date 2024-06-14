<%@ Page Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true" CodeBehind="C_ForLeaveOvertime_Year_Valcation_Create.aspx.cs"
    Inherits="TPPDDB._3_forleave.C_ForLeaveOvertime_Year_Valcation_Create"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="../styles/Stylesheet.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style58
        {
            text-align: left;
            width: 70px;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
            font-weight: bold;
        }
        .style59
        {
            text-align: center;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="1000">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server">
                <table class="style10">
                    <tr>
                        <td class="style8">
                            年度休假計算
                        </td>
                        <td class="style4">
                            <asp:Label ID="Label1" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table class="style6" border="1">
                    <tr>
                        <td class="style58">
                            年度：
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_YEAR" runat="server" Width="50px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style58">
                            身分證號：
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_ID" runat="server" Width="75px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style58">
                            計算類別：
                        </td>
                        <td class="style3">
                            <asp:RadioButtonList ID="RadioButtonList1" runat="server" RepeatColumns="3">
                                <asp:ListItem>更新全部人員</asp:ListItem>
                                <asp:ListItem>新增全部人員</asp:ListItem>
                                <asp:ListItem  Selected="True">單人</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="style59" colspan="2">
                            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="確定" class="style9" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label runat="server" ID="lblErrorMsg"></asp:Label>
                        </td>
                    </tr>
                </table>
                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                    <ProgressTemplate>
                        <br />
                        <img alt="" src="../images/ajax-loader.gif" style="width: 328px; height: 19px" /><br />
                        <span class="style3">資料量多，產生中 請稍待…</span>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
