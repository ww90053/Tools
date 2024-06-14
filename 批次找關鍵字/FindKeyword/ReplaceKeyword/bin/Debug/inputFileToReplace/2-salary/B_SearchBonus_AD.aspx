<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true" CodeBehind="B_SearchBonus_AD.aspx.cs" Inherits="TPPDDB._2_salary.B_SearchBonus_AD" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <style type="text/css">

        .style1
        {
            width: 100%;
        }
        

        .style2
        {
            width: 158px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="title_s1">
    <asp:Label ID="Label_TITLE" runat="server"></asp:Label>
    </div>
    <asp:Panel ID="Panel1" runat="server" Width="417px" 
        GroupingText="在下列條件選擇，欲產生之報表">
        <table class="style1">
            <tr>
                <td style="text-align: right" class="style2">
                </td>
                <td style="text-align: left">
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="style2" style="text-align: right">
                    <asp:Label ID="Label_AD" runat="server" Text="發薪機關："></asp:Label>
                </td>
                <td style="text-align: left">
                    <asp:DropDownList ID="DropDownList_PAY_AD" runat="server" 
                        DataTextField="MZ_KCHI" 
                        DataValueField="MZ_KCODE">
                    </asp:DropDownList>
                </td>
            </tr>
           
           
            <tr>
                <td class="style2" style="text-align: right">
                    年月：</td>
                <td style="text-align: left">
                    <asp:TextBox ID="TextBox_Date" runat="server" Height="19px" MaxLength="5" 
                        Width="85px"></asp:TextBox>
                    <cc1:FilteredTextBoxExtender ID="TextBox_Date_FilteredTextBoxExtender7" 
                        runat="server" Enabled="True" FilterType="Numbers" 
                        TargetControlID="TextBox_Date">
                    </cc1:FilteredTextBoxExtender>
                    &nbsp; (民國年，如：09801)</td>
            </tr>
            
           <tr>
              <td class="style2" style="text-align: right">
                  </td>
                
                <td  style="text-align: left">
                    <asp:RadioButtonList ID="RadioButtonList1" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Text="公職人員" Value="0" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="駕駛工友" Value="1"></asp:ListItem>

                    </asp:RadioButtonList>
                </td>
            </tr>
            
          
            <tr>
                <td colspan="2">
                    <asp:Button ID="Button_SEARCH" runat="server" onclick="Button_SEARCH_Click" 
                        Text="產生" />
                   
                    <asp:ScriptManager ID="ScriptManager1" runat="server">
                    </asp:ScriptManager>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>

