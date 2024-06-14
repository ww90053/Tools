<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="B_SalaryLOCKDB.aspx.cs" Inherits="TPPDDB._2_salary.B_SalaryLOCKDB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="style/Master.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="0">
    </asp:ScriptManager>
    <div class="PageTitle">
        關帳作業
    </div>
    <asp:Panel ID="Panel_LOCKDB" runat="server" Width="417px" GroupingText="在下列條件選擇關帳目標">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table class="TableStyleBlue">
                    <tr>
                        <th>
                            發薪機關
                        </th>
                        <td>
                            <asp:DropDownList ID="DropDownList_PAY_AD" runat="server" DataTextField="MZ_KCHI"
                                DataValueField="MZ_KCODE">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            類別
                        </th>
                        <td>
                            <asp:DropDownList ID="DropDownList_GROUP" runat="server" OnSelectedIndexChanged="DropDownList_GROUP_SelectedIndexChanged"
                                AutoPostBack="True">
                                <asp:ListItem Value="SALARY" Selected="True">每月薪資</asp:ListItem>
                                <asp:ListItem Value="REPAIR">補發薪資</asp:ListItem>
                                <asp:ListItem Value="SOLE">單一發放</asp:ListItem>
                                <asp:ListItem Value="YEARPAY">年終獎金</asp:ListItem>
                                <asp:ListItem Value="EFFECT">考績獎金</asp:ListItem>
                                <asp:ListItem Value="TAXES">所得稅</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            <asp:Label ID="lb_da" runat="server" Text=""></asp:Label>
                        </th>
                        <td>
                            <asp:TextBox ID="TextBox_DA" runat="server" Width="70px" AutoPostBack="True" MaxLength="7"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="TextBox_DA_FilteredTextBoxExtender" runat="server"
                                Enabled="True" FilterType="Numbers" TargetControlID="TextBox_DA">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr id="batch" runat="server">
                        <th>
                            <a class="must">*批號</a>
                        </th>
                        <td style="text-align: left">
                            <asp:TextBox ID="txt_caseid" runat="server" Width="50px" MaxLength="3"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="text-align: center;">
                            <asp:Button ID="btSave" runat="server" OnClick="btSave_Click" Text="確定" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:Content>
