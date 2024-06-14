<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true" CodeBehind="C_OverTime_EXCARD_Input.aspx.cs" Inherits="TPPDDB._3_forleave.C_OverTime_EXCARD_Input" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="../styles/Stylesheet.css" rel="stylesheet" type="text/css" />
    <link href="style/SearchControl.css" rel="Stylesheet" type="text/css" />
    <style type="text/css">
        .row_header {
            width: 100px;
            font-weight: 700;
            color: #0033CC;
            text-align: justify;
            text-align-last: justify;
            background-color: #99FFCC;
        }

        .ViewHeader {
            color: #000000;
            font-weight: 700;
            font-weight: bold;
            text-align: justify;
            text-align-last: justify;
            width: 80px;
            white-space: nowrap;
        }

        .EditHeader {
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
            font-weight: bold;
            text-align: justify;
            text-align-last: justify;
            width: 80px;
            white-space: nowrap;
        }

        .GV_Header {
            background-color: #507CD1;
            font-weight: bold;
            color: white;
            border: solid 1px #e4e3e3;
            text-align: center;
        }

        .GV_Hidden {
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="style10 style8">
        刷卡指紋例外人員管理
    </div>
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnl_Query" runat="server">
                <table class="style6" border="1">
                    <tr>
                        <td class="style5">
                            身分證號
                        </td>
                        <td class="style2">
                            <asp:TextBox runat="server" ID="txt_MZ_ID" AutoPostBack="True" MaxLength="10" Width="75px" Style="height: 19px"
                                OnTextChanged="txt_MZ_ID_TextChanged"></asp:TextBox>
                        </td>
                        <td class="style1">
                            姓名
                        </td>
                        <td class="style3">
                            <asp:TextBox runat="server" ID="txt_MZ_NAME" MaxLength="6" Width="100px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table class="style6" border="1">
                    <tr>
                        <td class="style1">
                            現服機關
                        </td>
                        <td class="style3">
                            <asp:Label ID="lbl_MZ_EXAD" runat="server" Font-Bold="True" ForeColor="Black"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">
                            現服單位
                        </td>
                        <td class="style3">
                            <asp:Label ID="lbl_MZ_EXUNIT" runat="server" Font-Bold="True" ForeColor="Black"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">
                            職稱
                        </td>
                        <td class="style3">
                            <asp:Label ID="lbl_OCCC" runat="server" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="style7">
                            是否為主管
                        </td>
                        <td class="style3">
                            <asp:RadioButtonList runat="server" ID="rbl_IS_MANAGER" RepeatDirection="Horizontal">
                                <asp:ListItem Value="N">否</asp:ListItem>
                                <asp:ListItem Value="Y">是</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="pnl_button" runat="server">
                <table style="background-color: #CCFFFF; color: White; width: 100%">
                    <tr>
                        <td>
                            <asp:Button ID="btn_Search" runat="server" Text="查詢" CausesValidation="False" class="style9" OnClick="btn_Search_Click" />
                            <asp:Button ID="btn_Insert" runat="server" Text="新增" AccessKey="a" CausesValidation="False" class="style9" OnClick="btn_Insert_Click" />
                            <asp:Button ID="btn_Update" runat="server" Text="修改" Style="width: 40px" CausesValidation="False" class="style9" OnClick="btn_Update_Click" Enabled="False" />
                            <asp:Button ID="btn_OK" runat="server" Text="確定" class="style9" OnClick="btn_OK_Click" Enabled="False" />
                            <asp:Button ID="btn_Cancel" runat="server" Text="取消" CausesValidation="False" class="style9" OnClick="btn_Cancel_Click" Enabled="False" />
                            <asp:Button ID="btn_Delete" runat="server" Text="刪除" AccessKey="d" CausesValidation="False" class="style9" OnClick="btn_Delete_Click"
                                OnClientClick="return confirm(&quot;確定刪除？&quot;);" Enabled="False" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="pnl_GridView" runat="server">
                <asp:GridView ID="gv_resultat" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                    CellPadding="4" DataKeyNames="MZ_ID" ForeColor="#333333" GridLines="None"
                    Width="100%" EmptyDataText="查無資料" PageSize="10"
                    OnPageIndexChanging="gv_resultat_PageIndexChanging">
                    <Columns>
                        <asp:BoundField DataField="MZ_ID" HeaderText="身份證字號" SortExpression="MZ_ID" />
                        <asp:BoundField DataField="NAME" HeaderText="姓名" SortExpression="NAME" />
                        <asp:BoundField DataField="MZ_EXAD_CH" HeaderText="現服機關" SortExpression="MZ_EXAD" />
                        <asp:BoundField DataField="MZ_EXUNIT_CH" HeaderText="現服單位" SortExpression="MZ_EXUNIT" />
                        <asp:BoundField DataField="MZ_OCCC_CH" HeaderText="職稱" SortExpression="MZ_OCCC" />
                        <asp:BoundField DataField="IS_MANAGER" HeaderText="是否主管" />
                    </Columns>
                    <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#2461BF" />
                    <AlternatingRowStyle BackColor="White" />
                </asp:GridView>
            </asp:Panel>

            <cc1:ModalPopupExtender runat="server" ID="mpe_Search" Enabled="True" BackgroundCssClass="modalBackground"
                TargetControlID="btn_Search" CancelControlID="btn_Search_Cancel" PopupControlID="pl_search">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="pl_search" runat="server" Style="display: none; " CssClass="DivPanel" >
                <table class="style52" border="1">
                    <tr>
                        <td class="style102">姓&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 名:     
                        </td>
                        <td class="style101">
                            <asp:TextBox ID="txt_Search_NAME" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style102">身分證號:
                        </td>
                        <td class="style101">
                            <asp:TextBox ID="txt_Search_ID" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style102">機&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 關:
                        </td>
                        <td class="style101">
                            <asp:DropDownList ID="ddl_Search_MZ_EXAD" runat="server" AutoPostBack="True"
                                OnSelectedIndexChanged="ddl_Search_MZ_EXAD_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="style102">單&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 位:
                        </td>
                        <td class="style101">
                            <asp:DropDownList ID="ddl_Search_MZ_EXUNIT" runat="server" class="style55">
                            </asp:DropDownList>

                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Button runat="server" ID="btn_Search_OK" Text="確定" class="style53" OnClick="btn_Search_OK_Click" />
                            &nbsp;&nbsp;&nbsp;
                            <asp:Button runat="server" ID="btn_Search_Cancel" CausesValidation="False" Text="離開" class="style54" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
