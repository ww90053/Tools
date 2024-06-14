<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="C_CHANGE_FORLEAVEBASIC.aspx.cs" Inherits="TPPDDB._3_forleave.C_CHANGE_FORLEAVEBASIC" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="../styles/Stylesheet.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style61
        {
            text-align: left;
            height: 20px;
            width: 131px;
        }
        .style62
        {
            text-align: left;
            height: 20px;
            width: 113px;
        }
        .style63
        {
            text-align: left;
            height: 20px;
            width: 149px;
        }
        .style70
        {
            text-align: left;
            width: 55px;
            height: 20px;
            background-color: #FFCCFF;
        }
        .style71
        {
            text-align: left;
            width: 55px;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
            font-weight: bold;
        }
        .style74
        {
            color: #FF0000;
            font-weight: bold;
            background-color: #FFFF99;
            text-align: left;
            width: 55px;
        }
        .style75
        {
            text-align: left;
            width: 30px;
            height: 20px;
            background-color: #FFCCFF;
        }
        .style86
        {
            text-align: left;
            height: 20px;
            width: 93px;
        }
        .style87
        {
            text-align: left;
            height: 20px;
            width: 135px;
        }
        .style88
        {
            height: 21px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server">
                <asp:HiddenField ID="hdf_MZ_DLTB01_SN" runat="server" />
                <asp:HiddenField ID="hdf_C_CHANGE_DLTB01_SN" runat="server" />
                <table class="style10">
                    <tr>
                        <td class="style8">
                            申請銷假作業
                        </td>
                    </tr>
                </table>
                <table class="style6" border="1">
                    <tr>
                        <td class="style74">
                            身分證號
                        </td>
                        <td class="style2">
                            <asp:Label ID="Label_MZ_ID" runat="server"></asp:Label>
                        </td>
                        <td class="style75">
                            姓 名
                        </td>
                        <td class="style87">
                            <asp:Label ID="Label_MZ_NAME" runat="server"></asp:Label>
                        </td>
                        <td style="text-align: left">
                            <asp:Button ID="btn_Search_DLTB01" CssClass="style9" runat="server" Text="查詢請假資料"
                                OnClick="btn_Search_DLTB01_Click" />
                        </td>
                    </tr>
                </table>
                <table class="style6" border="1">
                    <tr>
                        <td class="style70">
                            編制機關
                        </td>
                        <td class="style3">
                            <asp:Label ID="Label_MZ_AD" runat="server" Font-Bold="True" ForeColor="#0033CC"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="style70">
                            現服機關
                        </td>
                        <td class="style3">
                            <asp:Label ID="Label_MZ_EXAD" runat="server" Font-Bold="True" ForeColor="#0033CC"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table class="style6" border="1">
                    <tr>
                        <td class="style71">
                            原日期
                        </td>
                        <td class="style62">
                            <asp:Label ID="Label_ORGINAL_MZ_IDATE1" runat="server"></asp:Label>
                            日
                        </td>
                        <td class="style63">
                            <asp:Label ID="Label_ORGINAL_MZ_ITIME1" runat="server"></asp:Label>
                            分
                        </td>
                        <td class="style3">
                            至
                        </td>
                        <td class="style86">
                            <asp:Label ID="Label_ORGINAL_MZ_ODATE" runat="server"></asp:Label>
                            日
                        </td>
                        <td class="style61">
                            <asp:Label ID="Label_ORGINAL_MZ_OTIME" runat="server"></asp:Label>
                            分
                        </td>
                        <td class="style3">
                            共計
                        </td>
                        <td class="style3">
                            <asp:Label ID="Label_ORGINAL_MZ_TDAY" runat="server"></asp:Label>
                            日
                        </td>
                        <td class="style3">
                            <asp:Label ID="Label_ORGINAL_MZ_TTIME" runat="server"></asp:Label>
                            時
                        </td>
                    </tr>
                </table>
                <table class="style6" border="1">
                    <tr>
                        <td class="style71">
                            修改日期
                        </td>
                        <td class="style62">
                            <asp:TextBox ID="TextBox_MZ_IDATE1" runat="server" Width="65px" MaxLength="7"></asp:TextBox>
                            <cc2:FilteredTextBoxExtender ID="TextBox_MZ_IDATE1_FilteredTextBoxExtender" runat="server"
                                Enabled="True" FilterType="Numbers" TargetControlID="TextBox_MZ_IDATE1">
                            </cc2:FilteredTextBoxExtender>
                            日
                        </td>
                        <td class="style63">
                            <asp:TextBox ID="TextBox_MZ_ITIME1" runat="server" Width="50px" MaxLength="4"></asp:TextBox>
                            <cc2:FilteredTextBoxExtender ID="TextBox_MZ_ITIME1_FilteredTextBoxExtender" runat="server"
                                Enabled="True" FilterType="Numbers" TargetControlID="TextBox_MZ_ITIME1">
                            </cc2:FilteredTextBoxExtender>
                            分 (例：0830)
                        </td>
                        <td class="style3">
                            至
                        </td>
                        <td class="style86">
                            <asp:TextBox ID="TextBox_MZ_ODATE" runat="server" Width="65px" MaxLength="7"></asp:TextBox><cc2:FilteredTextBoxExtender
                                ID="TextBox_MZ_ODATE_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Numbers"
                                TargetControlID="TextBox_MZ_ODATE">
                            </cc2:FilteredTextBoxExtender>
                            日
                        </td>
                        <td class="style61">
                            <asp:TextBox ID="TextBox_MZ_OTIME" runat="server" AutoPostBack="True" Width="50px"
                                MaxLength="4"></asp:TextBox><cc2:FilteredTextBoxExtender ID="TextBox_MZ_OTIME_FilteredTextBoxExtender"
                                    runat="server" Enabled="True" FilterType="Numbers" TargetControlID="TextBox_MZ_OTIME">
                                </cc2:FilteredTextBoxExtender>
                            分(例：1730)
                        </td>
                        <td class="style3">
                            共計
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_TDAY" runat="server" Width="40px" MaxLength="3"></asp:TextBox>日
                            <cc2:FilteredTextBoxExtender ID="TextBox_MZ_TDAY_FilteredTextBoxExtender" runat="server"
                                Enabled="True" FilterType="Numbers" TargetControlID="TextBox_MZ_TDAY">
                            </cc2:FilteredTextBoxExtender>
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="TextBox_MZ_TTIME" runat="server" Width="30px" MaxLength="1"></asp:TextBox>時
                            <cc2:FilteredTextBoxExtender ID="TextBox_MZ_TTIME_FilteredTextBoxExtender" runat="server"
                                Enabled="True" FilterType="Numbers" TargetControlID="TextBox_MZ_TTIME">
                            </cc2:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    
                </table>
                 <table class="style6" border="1">
                 
                 <tr>
                    
                  <td class="style71"  >
                            輔助說明
                            
                        </td>
                        <td class="style3">如欲銷假整筆假單，請新增銷假紀錄，將假單改為０時０分陳核      
                                                  <br>ex:該筆假單為 103/01/03 1330 ~  103/01/03 1730   共計0日 4時
<br>請新增一筆銷假紀錄為  
    103/01/03 1330 ~  103/01/03 1730   共計0日 0時 
                        </td>
                    </tr>
                 
                 </table>
                
                <table class="style12">
                    <tr>
                        <td>
                            <asp:Button ID="btInsert" runat="server" Text="新增" CssClass="style9" CausesValidation="False"
                                OnClick="btInsert_Click" Enabled="false" />
                            <asp:Button ID="btUpdate" runat="server" CausesValidation="False" class="style9"
                                Enabled="False" OnClick="btUpdate_Click" Text="修改" />
                            <asp:Button ID="btOK" runat="server" AccessKey="s" class="style9" Enabled="False"
                                meta:resourcekey="btOKResource1" OnClick="btOK_Click" Text="確定" />
                            <asp:Button ID="btDelete" runat="server" Text="刪除" class="style9" Enabled="False"
                                OnClick="btDelete_Click" OnClientClick="return confirm(&quot;確定刪除？&quot;)" />
                            <asp:Button ID="btCancel" runat="server" CausesValidation="False" class="style9"
                                Enabled="False" meta:resourcekey="btCancelResource1" OnClick="btCancel_Click"
                                Text="取消" />
                            <asp:Button ID="btn_onlinecheck" runat="server" Enabled="false" CssClass="style9"
                                Text="送核" OnClick="btn_onlinecheck_Click" />
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="Panel_choice" runat="server" Style="display: ;">
                    <div style="border: solid 1px gray; background-color: LightBlue; width: 280px; height: 290px;">
                        <div style="margin: 10px; background-color: #FFFFFF; width: 260px; height: 270px;">
                            <table class="style6" border="1">
                                <tr>
                                    <td class="style88" colspan="2">
                                        查詢請假資料
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        請假日期
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="TextBox_Search_MZ_IDATE1" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style88" colspan="2">
                                        <asp:Button ID="btn_Search" runat="server" Text="確定" OnClick="btn_Search_Click" />
                                    </td>
                                </tr>
                            </table>
                            <asp:GridView ID="GridView1" runat="server" BackColor="White" Width="100%" BorderColor="#999999"
                                BorderStyle="None" BorderWidth="1px" CellPadding="3" 
                                DataKeyNames="MZ_DLTB01_SN" GridLines="Vertical"
                                AutoGenerateColumns="False" onrowcommand="GridView1_RowCommand">
                                <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                <Columns>
                                    <asp:CommandField ShowSelectButton="True" ButtonType="Button" HeaderText="選取" />
                                    <asp:BoundField HeaderText="請假日期" DataField="MZ_IDATE1" />
                                </Columns>
                                <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
                                <AlternatingRowStyle BackColor="#DCDCDC" />
                            </asp:GridView>
                        </div>
                    </div>
                </asp:Panel>
                <asp:Button ID="btn_popup2" runat="server" Text="Button" Style="display: none;" />
                <cc2:ModalPopupExtender ID="Panel_choice_ModalPopupExtender" runat="server" BackgroundCssClass="modalBackground"
                    DynamicServicePath="" Enabled="True" TargetControlID="btn_popup2" PopupControlID="Panel_choice">
                </cc2:ModalPopupExtender>
                <asp:Panel ID="Panel_select" runat="server" Style="display: none;">
                    <div style="border: solid 1px gray; background-color: LightBlue; width: 280px; height: 290px;">
                        <div style="margin: 10px; background-color: #FFFFFF; width: 260px; height: 270px;"
                            class="style87">
                            <div style="margin: 10px;">
                                <h3>
                                    陳核
                                </h3>
                                <asp:GridView ID="GV_CHECKER" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                    ForeColor="#333333" GridLines="None" AllowPaging="True" OnPageIndexChanging="GV_CHECKER_PageIndexChanging"
                                    OnRowCommand="GV_CHECKER_RowCommand" PageSize="5" EmptyDataText="查無資料">
                                    <RowStyle BackColor="#EFF3FB" />
                                    <Columns>
                                        <asp:TemplateField ShowHeader="False" ItemStyle-Width="40px">
                                            <ItemTemplate>
                                                <asp:Button ID="btn_select" runat="server" CausesValidation="false" CommandName="checker"
                                                    Text="選取" CommandArgument='<%# Eval("SN") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="MZ_OCCC" HeaderText="職稱" ItemStyle-Width="100px" />
                                        <asp:BoundField DataField="MZ_NAME" HeaderText="姓名" ItemStyle-Width="100px" />
                                    </Columns>
                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <EditRowStyle BackColor="#2461BF" />
                                    <AlternatingRowStyle BackColor="White" />
                                </asp:GridView>
                                <div style="text-align: center;">
                                    <asp:Button ID="btn_check" runat="server" Text="送出" Style="display: none;" />
                                    <asp:Button ID="btn_exit" runat="server" Text="取消" OnClick="btn_exit_Click" />
                                </div>
                                <asp:Button ID="btn_popup" runat="server" Text="Button" Style="display: none;" />
                            </div>
                        </div>
                    </div>
                </asp:Panel>
                <cc2:ModalPopupExtender ID="Panel_select_ModalPopupExtender" runat="server" BackgroundCssClass="modalBackground"
                    DynamicServicePath="" Enabled="True" TargetControlID="btn_popup" PopupControlID="Panel_select">
                </cc2:ModalPopupExtender>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
