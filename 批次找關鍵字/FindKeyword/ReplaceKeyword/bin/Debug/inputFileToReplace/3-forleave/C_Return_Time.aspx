<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="C_Return_Time.aspx.cs" Inherits="TPPDDB._3_forleave.C_Return_Time" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <style type="text/css">
        .style1
        {
            width: 186px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hdf_OTIME" runat="server" />
            <asp:HiddenField ID="hdf_MZ_RESTDATE" runat="server" />
            <table width="75%">
                <tr>
                    <td class="title_s1">
                        補休時數回復
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="Panel1" runat="server" GroupingText="查詢條件" Width="604px">
                            <table style="width: 100%;">
                                <tr>
                                    <td align="right" class="style1">
                                        加班補休日期：
                                    </td>
                                    <td align="left">
                                        &nbsp;
                                        <asp:TextBox ID="TextBox_MZ_DATE" runat="server" Width="65px"></asp:TextBox>
                                        &nbsp;(範例：0980101)
                                    </td>
                                </tr>
                               <tr>
                                    <td align="right" class="style1">
                                        服務機關：
                                    </td>
                                    <td align="left">
                                         &nbsp;
                                        <asp:DropDownList ID="ddlSAd" runat="server" AutoPostBack="True" Width="150px"
                                            OnSelectedIndexChanged="ddlSAd_SelectedIndexChanged">
                                        </asp:DropDownList>
                                         &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" class="style1">
                                        身分證號：
                                    </td>
                                    <td align="left">
                                        &nbsp;
                                        <asp:TextBox ID="TextBox_MZ_ID" runat="server" MaxLength="10" Width="85px"></asp:TextBox>
                                    </td>
                                    <tr>
                                        <td class="style2" colspan="2" style="text-align: center">
                                            <asp:Button ID="btSearch" runat="server" Text="查詢" OnClick="btSearch_Click" />
                                            &nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:Button ID="btCancel" runat="server" Text="取消" CausesValidation="False" />
                                        </td>
                                    </tr>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
            <asp:GridView ID="gv_data" runat="server" AutoGenerateColumns="False" DataKeyNames="MZ_HOUR"
                OnRowCommand="gv_data_RowCommand">
                <Columns>
                    <asp:BoundField HeaderText="使用日期" DataField="MZ_IDATE1" />
                    <asp:TemplateField HeaderText="使用時數">
                        <ItemTemplate>
                            <asp:TextBox ID="TextBox_MZ_HOUR" runat="server" AutoPostBack="True" OnTextChanged="TextBox_MZ_HOUR_TextChanged"
                                Text='<%# Bind("MZ_HOUR") %>' Width="40px"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="TextBox_MZ_HOUR_FilteredTextBoxExtender" runat="server"
                                Enabled="True" FilterType="Numbers" TargetControlID="TextBox_MZ_HOUR">
                            </cc1:FilteredTextBoxExtender>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:ButtonField ButtonType="Button" CommandName="Del" HeaderText="刪除" Text="刪除" />
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
