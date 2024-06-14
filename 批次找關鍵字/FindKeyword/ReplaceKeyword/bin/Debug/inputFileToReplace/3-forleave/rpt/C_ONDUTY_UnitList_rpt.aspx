<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true" CodeBehind="C_ONDUTY_UnitList_rpt.aspx.cs" Inherits="TPPDDB._3_forleave.rpt.C_ONDUTY_UnitList_rpt" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">

<link href="../../2-salary/style/Master.css" rel="stylesheet" type="text/css" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="PageTitle">
        <asp:Label ID="lb_Title" runat="server" Text="Label"></asp:Label>
    </div>
    <table class="TableStyleBlue" style="width: 90%; margin-top: 10px;">
        <tr>
            <th>
                機關：
            </th>
            <td>
                <asp:DropDownList ID="ddl_EXAD" runat="server" 
                    onselectedindexchanged="ddl_EXAD_SelectedIndexChanged"  > 
                </asp:DropDownList>
            </td>
        </tr>
      <tr>
            <th>
                機關：
            </th>
            <td>
                <asp:DropDownList ID="ddl_EXUNIT" runat="server"  > 
                </asp:DropDownList>
            </td>
        </tr>
        
        <tr>
            <th>
                日期：
            </th>
            <td>
                <asp:TextBox ID="TextBox_DATE" runat="server" MaxLength="5" Width="60px"></asp:TextBox>
                <cc1:FilteredTextBoxExtender ID="TextBox_YEAR2_FilteredTextBoxExtender4" runat="server"
                    Enabled="True" FilterType="Numbers" TargetControlID="TextBox_DATE">
                </cc1:FilteredTextBoxExtender>
                &nbsp;年 (民國年，如：09801)
            </td>
        </tr>
       
        <tr>
            <td colspan="2" style="text-align: center;">
                
               <asp:Button ID="Button_SEARCH" runat="server"  Text="預覽" OnClick="Button_SEARCH_Click" />
               <asp:Button ID="Button_SEARCH_New" runat="server"  Text="預覽(新版)" OnClick="Button_SEARCH_New_Click" />
                
             
            </td>
        </tr>
    </table>
</asp:Content>