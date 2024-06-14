<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="Personal_Gradenotice_rpt.aspx.cs" Inherits="TPPDDB._1_personnel.Personal_Gradenotice_rpt" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register src="A_UCLoading.ascx" tagname="A_UCLoading" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <style type="text/css">
        .style3
        {
            text-align: center;
        }
        .style4
        {
            width: 98px;
            text-align: right;
        }
        .style5
        {
            width: 56px;
            text-align: right;
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
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="title_s1">
                功過相抵申誡以上通知書</div>
            <div>
            </div>
            <div>
                <asp:Panel ID="Panel1" runat="server" GroupingText="查詢條件" Width="500px">
                    <table width="100%">
                        <tbody class="style3">
                            <tr>
                                <td class="style4">
                                    機關名稱</td>
                                <td style="text-align: left">
                                    <asp:DropDownList ID="DropDownList_MZ_AD" runat="server" AutoPostBack="True" 
                                        onselectedindexchanged="DropDownList_MZ_AD_SelectedIndexChanged">
                                    </asp:DropDownList>
                                
                                </td>
                            </tr>
                            <tr>
                                <td class="style4">
                                    單位名稱
                                </td>
                                <td style="text-align: left">
                                    <asp:DropDownList ID="DropDownList_MZ_UNIT" runat="server"
                                         ondatabound="DropDownList_MZ_UNIT_DataBound">
                                        <asp:ListItem></asp:ListItem>
                                    </asp:DropDownList>                                   
                                </td>
                            </tr>
                            <tr>
                                <td class="style4">
                                    日期區間
                                </td>
                                <td style="text-align: left">
                                    <asp:TextBox ID="TextBox_MZ_DATE1" runat="server" Width="65px" AutoPostBack="True"
                                        OnTextChanged="TextBox_MZ_DATE1_TextChanged"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="TextBox_MZ_DATE1_FilteredTextBoxExtender" 
                                        runat="server" Enabled="True" FilterType="Numbers" 
                                        TargetControlID="TextBox_MZ_DATE1">
                                    </cc1:FilteredTextBoxExtender>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="TextBox_MZ_DATE1"
                                        Display="Dynamic" ErrorMessage="不可空白">不可空白</asp:RequiredFieldValidator>
                                    至
                                    <asp:TextBox ID="TextBox_MZ_DATE2" runat="server" Width="65px" OnTextChanged="TextBox_MZ_DATE2_TextChanged"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="TextBox_MZ_DATE2_FilteredTextBoxExtender" 
                                        runat="server" Enabled="True" FilterType="Numbers" 
                                        TargetControlID="TextBox_MZ_DATE2">
                                    </cc1:FilteredTextBoxExtender>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="TextBox_MZ_DATE2"
                                        Display="Dynamic" ErrorMessage="不可空白">不可空白</asp:RequiredFieldValidator>
                                    （EX:0990101）
                                </td>
                            </tr>
                            <tr>
                                <td class="style4">
                                    申誡次數
                                </td>
                                <td style="text-align: left">
                                    <asp:TextBox ID="TextBox_MZ_5001" runat="server" Width="50px"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="TextBox_MZ_5001_FilteredTextBoxExtender" runat="server"
                                        Enabled="True" FilterType="Numbers" TargetControlID="TextBox_MZ_5001">
                                    </cc1:FilteredTextBoxExtender>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TextBox_MZ_5001"
                                        Display="Dynamic" ErrorMessage="不可空白">不可空白</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </asp:Panel>
                <asp:Panel ID="Panel2" runat="server" GroupingText="自行輸入項目" Width="500px">
                    <table width="100%">
                        <tr>
                            <td class="style4">
                                發文日期
                            </td>
                            <td style="text-align: left">
                                <asp:TextBox ID="TextBox_DATE" runat="server"></asp:TextBox>
                                （EX:98年1月1號）
                            </td>
                        </tr>
                        <tr>
                            <td class="style4">
                                文號</td>
                            <td style="text-align: left">
                                <asp:TextBox ID="TextBox_PRID" runat="server" Width="200px"></asp:TextBox>
                                (EX:北縣警人第12345號)
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="Panel3" runat="server" GroupingText="功能列" Width="423px">
                    <table width="100%">
                        <tr>
                            <td class="style2" colspan="2" style="text-align: center">
                                <asp:Button ID="btPrint" runat="server" Text="列印" OnClick="btPrint_Click" />
                                &nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btCancel" runat="server" Text="取消" OnClick="btCancel_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <uc1:A_UCLoading ID="A_UCLoading1" runat="server" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
