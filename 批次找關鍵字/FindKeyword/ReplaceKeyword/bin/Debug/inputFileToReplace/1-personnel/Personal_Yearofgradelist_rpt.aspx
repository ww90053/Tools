﻿<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="Personal_Yearofgradelist_rpt.aspx.cs" Inherits="TPPDDB._1_personnel.Personal_Yearofgradelist_rpt" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="A_UCLoading.ascx" TagName="A_UCLoading" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <style type="text/css">
        .style3
        {
            text-align: center;
        }
        .style4
        {
            width: 70px;
        }
    </style>

    <script language="javascript" type="text/javascript">
        function toLocalDate(source, clientside_arguments) {
            var DateValue = source._textbox.get_Value();
            var year = parseInt(DateValue.substr(0, 4)) - 1911;
            source._textbox.set_Value(year + DateValue.substr(4, 6));
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="title_s1">
        各機關年度獎懲統計表</div>
    <div>
    </div>
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="1000">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Panel ID="Panel1" runat="server" GroupingText="查詢條件" Width="362px">
                    <table width="100%">
                        <tbody class="style3">
                            <tr class="style3">
                                <td class="style4" style="text-align: left">
                                    日期區間
                                </td>
                                <td style="text-align: left">
                                    <asp:TextBox ID="TextBox_MZ_DATE1" runat="server" Width="65px"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="TextBox_MZ_DATE1_FilteredTextBoxExtender" 
                                        runat="server" Enabled="True" FilterType="Numbers" 
                                        TargetControlID="TextBox_MZ_DATE1">
                                    </cc1:FilteredTextBoxExtender>
                                    &nbsp; <b>─&nbsp; </b>
                                    <asp:TextBox ID="TextBox_MZ_DATE2" runat="server" Width="65px"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="TextBox_MZ_DATE2_FilteredTextBoxExtender" 
                                        runat="server" Enabled="True" FilterType="Numbers" 
                                        TargetControlID="TextBox_MZ_DATE2">
                                    </cc1:FilteredTextBoxExtender>
                                    (範例：0990101)
                                </td>
                            </tr>
                            <tr>
                                <td class="style4" style="text-align: left">
                                    機關名稱
                                </td>
                                <td style="text-align: left">
                                    <asp:DropDownList ID="DropDownList_MZ_AD" runat="server" >
                                    </asp:DropDownList>
                                   
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </asp:Panel>
                <asp:Panel ID="Panel3" runat="server" GroupingText="功能列" Width="362px">
                    <table width="100%">
                        <tr>
                            <td class="style2" colspan="2" style="text-align: center">
                                <asp:Button ID="btPrint" runat="server" Text="列印" OnClick="btPrint_Click" />
                                &nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btCancel" runat="server" Text="取消" OnClick="btCancel_Click" />
                                &nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnexcel" runat="server" Text="匯出Excel" OnClick="btnexcel_Click"
                                        />
                            </td>
                        </tr>
                    </table>
                    
                     
                </asp:Panel>
                
                     
                            
                
                <uc1:A_UCLoading ID="A_UCLoading1" runat="server" />
            </ContentTemplate>
             <Triggers >
         <asp:postbacktrigger  ControlID ="btnexcel" />

        </Triggers>
            
        </asp:UpdatePanel>
    </div>
</asp:Content>
