<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="C_Return_Time_New.aspx.cs" Inherits="TPPDDB._3_forleave.C_Return_Time_New" %>

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
                                        加班補休日期(必填)：
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="TextBox_MZ_DATE" runat="server" Width="65px"></asp:TextBox>
                                        &nbsp;(範例：0980101)
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" class="style1">
                                        請假起時(非必填)：
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="TextBox_MZ_TIME" runat="server" Width="65px"></asp:TextBox>
                                        &nbsp;(範例：1530)
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        服務機關：
                                    </td>
                                    <td style="text-align: left">
                                        <asp:DropDownList ID="DropDownList_EXAD" runat="server" AutoPostBack="True" 
                                            onselectedindexchanged="DropDownList_EXAD_SelectedIndexChanged" >
                                        </asp:DropDownList>
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        服務單位：
                                    </td>
                                    <td style="text-align: left">
                                        <asp:DropDownList ID="DropDownList_EXUNIT" runat="server" AppendDataBoundItems="false"
                                            
                                            OnDataBound="DropDownList_EXUNIT_DataBound">
                                            <asp:ListItem></asp:ListItem>
                                        </asp:DropDownList>
                                        
                                    </td>
                                </tr>
                                 <tr>
                                    <td align="right" class="style1">
                                        身分證號(必填)：
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="TextBox_MZ_ID" runat="server" MaxLength="10" Width="85px"></asp:TextBox>
                                    </td>
                                    <tr>
                                        <td class="style2" colspan="2" style="text-align: center">
                                            <asp:Button ID="btSearchlist" runat="server" Text="查詢" OnClick="btSearchlist_Click" />
                                            &nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:Button ID="btSearch" runat="server" Text="還原" OnClick="btSearch_Click" />
                                            &nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:Button ID="btCancel" runat="server" Text="取消" OnClick="btCancel_Click" CausesValidation="False" />
                                        </td>
                                    </tr>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
            <asp:GridView ID="gv_data" runat="server" AutoGenerateColumns="False" DataKeyNames="MZ_HOUR"
                OnRowCommand="gv_data_RowCommand" Width="95%" EmptyDataText="查無資料" >
                <Columns>
                    <asp:BoundField HeaderText="使用加班日期" DataField="MZ_IDATE1" />
                    <asp:BoundField HeaderText="使用分鐘數" DataField="MZ_HOUR" />
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
