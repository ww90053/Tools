<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="Personal_Efficiency3.aspx.cs" Inherits="TPPDDB._1_personnel.Personal_Efficiency3" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="../styles/Stylesheet_1.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style3
        {
            color: red;
            font-weight: bold;
            font-size: medium;
        }
        .style4
        {
            border: solid 0px;
        }
        .style5
        {
            text-align: left;
            width: 56px;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
            font-weight: bold;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" 
        AsyncPostBackTimeout="10000">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server">
                <table class="style1" border="1">
                    <tr>
                        <td class="style5">
                            年&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 度
                        </td>
                        <td style="text-align: left;">
                            <asp:TextBox ID="TextBox_MZ_YEAR" runat="server" Width="50px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="TextBox_MZ_YEAR_FilteredTextBoxExtender" runat="server"
                                Enabled="True" TargetControlID="TextBox_MZ_YEAR" FilterType="Numbers">
                            </cc1:FilteredTextBoxExtender>
                            (範例：099)
                        </td>
                    </tr>
                    <tr>
                        <td class="style5">
                            服務機關
                        </td>
                        <td style="text-align: left;">
                            <asp:TextBox ID="TextBox_MZ_AD" runat="server" Width="75px" AutoPostBack="True" OnTextChanged="TextBox_MZ_AD_TextChanged"
                                MaxLength="10"></asp:TextBox>
                            <asp:Button ID="btAD" runat="server" Text="V" CausesValidation="False" OnClick="btAD_Click"
                                TabIndex="-1" />
                            <asp:TextBox ID="TextBox_MZ_AD1" runat="server" CssClass="style4" Width="200px" TabIndex="-1"
                                Font-Bold="True" ForeColor="#0033CC"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style5">
                            匯&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 入
                        </td>
                        <td style="text-align: left;">
                            <asp:RadioButtonList ID="RadioButtonList1" runat="server" RepeatColumns="2">
                                <asp:ListItem>全部人員</asp:ListItem>
                                <asp:ListItem>新進人員</asp:ListItem>
                            </asp:RadioButtonList>
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
                <table style="background-color: #CCFFFF; color: White; width: 100%">
                    <tr>
                        <td>
                            <asp:Button ID="Button1" runat="server" Text="確定" OnClick="Button1_Click" 
                                CssClass="KEY_IN_BUTTON_BLUE" />
                            <asp:Button ID="Button2" runat="server" Text="檢查" 
                                CssClass="KEY_IN_BUTTON_BLUE" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
