<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="C_diffdutydetail_rpt.aspx.cs" Inherits="TPPDDB._3_forleave.C_diffdutydetail_rpt" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <style type="text/css">
        .style1
        {
            text-align: left;
        }
        .style2
        {
            text-align: right;
        }
        .style3
        {
            font-weight: bold;
            color: red;
            font-size: medium;
        }
        .style4
        {
            text-align: center;
            width: 72px;
        }
    </style>

    <script type="text/javascript">
        function pageLoad() {
            var ppm = Sys.WebForms.PageRequestManager.getInstance();
            ppm.add_beginRequest(beginRequestHandler);
            //ppm.add_pageLoaded(pageLoaded);
        }

        function beginRequestHandler(sender, args) {
            if (args.get_postBackElement().id === 'ctl00_ContentPlaceHolder1_btPrint') {
                document.getElementById("ctl00_ContentPlaceHolder1_btCancel").disabled = true;
                args.get_postBackElement().disabled = true;
            }
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="100000">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="65%">
                <tr>
                    <td class="title_s1">
                        刷卡(勤惰)明細表
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="Panel1" runat="server" GroupingText="查詢條件" Width="514px">
                            <table style="width: 100%;">
                                <tr>
                                    <td class="style2">
                                        日期區間：
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="TextBox_LOGDATE1" runat="server" Width="85px"
                                             MaxLength="7"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="TextBox_LOGDATE1_FilteredTextBoxExtender" 
                                            runat="server" Enabled="True" FilterType="Numbers" 
                                            TargetControlID="TextBox_LOGDATE1">
                                        </cc1:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="TextBox_LOGDATE1"
                                            Display="Dynamic" ErrorMessage="不可空白">不可空白</asp:RequiredFieldValidator>
                                        &nbsp; 至<b>&nbsp; </b>
                                        <asp:TextBox ID="TextBox_LOGDATE2" runat="server" Width="85px" 
                                            MaxLength="7"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="TextBox_LOGDATE2_FilteredTextBoxExtender" 
                                            runat="server" Enabled="True" FilterType="Numbers" 
                                            TargetControlID="TextBox_LOGDATE2">
                                        </cc1:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="TextBox_LOGDATE2"
                                            Display="Dynamic" ErrorMessage="不可空白">不可空白</asp:RequiredFieldValidator>
                                        ( 範例：0990101)
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
                                    <td class="style2">
                                        身分證號：
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="TextBox_MZ_ID" runat="server" Width="100px" MaxLength="10"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style2">
                                        姓 名：
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="TextBox_MZ_NAME" runat="server" MaxLength="6" Width="100px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style2">
                                        報表類型：
                                    </td>
                                    <td style="text-align: left">
                                        <asp:RadioButtonList ID="rbl_check" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Text ="全部" Value="0" Selected="True"></asp:ListItem>  
                                        <asp:ListItem Text ="異常報表" Value="1" ></asp:ListItem>  
                                        <asp:ListItem Text ="加班報表" Value="2" ></asp:ListItem>  
                                        <asp:ListItem Text ="超勤報表" Value="3" ></asp:ListItem> 
                                        <asp:ListItem Text ="值日報表" Value="4" ></asp:ListItem> 
                                        </asp:RadioButtonList>
                                        <%--<asp:CheckBox ID="CheckBox3" runat="server" AutoPostBack="True" OnCheckedChanged="CheckBox3_CheckedChanged"
                                            Text="全部" />
                                        <asp:CheckBox ID="CheckBox1" runat="server" Text="異常報表" AutoPostBack="True" OnCheckedChanged="CheckBox1_CheckedChanged" />&nbsp;<asp:CheckBox
                                            ID="CheckBox2" runat="server" Text="加班報表 " AutoPostBack="True" OnCheckedChanged="CheckBox2_CheckedChanged" />--%>
                                       
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="Panel2" runat="server" GroupingText="功能列" Width="514px">
                            <table width="100%">
                                <tr>
                                    <td class="style2" colspan="2" style="text-align: center">
                                        <asp:Button ID="btPrint2" runat="server" Text="新報表" OnClick="btPrint2_Click" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btPrint" runat="server" Text="舊報表" OnClick="btPrint_Click" />
                                         &nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btPrint1" runat="server" Text="匯出" OnClick="btPrint1_Click" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btCancel" runat="server" Text="取消" OnClick="btCancel_Click" CausesValidation="False" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                            <ProgressTemplate>
                                <br />
                                <img alt="" src="../images/ajax-loader.gif" style="width: 328px; height: 19px" /><br />
                                <span class="style3">資料量多，產生中 請稍待…</span>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
