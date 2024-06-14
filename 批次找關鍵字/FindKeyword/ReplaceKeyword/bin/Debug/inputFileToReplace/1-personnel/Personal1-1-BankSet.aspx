<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="Personal1-1-BankSet.aspx.cs" Inherits="TPPDDB._1_personnel.Personal1_1_BankSet" Culture="auto"
    meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="TPPDDB" Namespace="TPPDDB" TagPrefix="cc2" %>
<%@ Register Assembly="TBGridView" Namespace="TPPDDB" TagPrefix="cc3" %>
<%@ Register Src="~/2-salary/UserControl/DLBASESeardhPanel.ascx" TagName="DLBASESeardhPanel"
    TagPrefix="uc1" %>
<%@ Register Src="~/2-salary/UserControl/UserSelector.ascx" TagName="UserSelector" TagPrefix="uc2" %>
<%@ Register Src="~/2-salary/UserControl/PoliceSearchPanel.ascx" TagName="PoliceSearchPanel"
    TagPrefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="../2-salary/style/Master.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style2 {
            text-align: left;
            margin-left: 0px;
        }

        .style12 {
            text-align: left;
        }

        .style30 {
            text-align: left;
            width: 298px;
        }

        .style34 {
            text-align: center;
        }

        .style35 {
            font-size: large;
            font-family: 標楷體;
        }

        .style90 {
            width: 42px;
        }

        .style93 {
            text-align: left;
            background-color: #CCFFFF;
        }

        .style94 {
            width: 43px;
        }

        .style96 {
            text-align: right;
        }

        .style97 {
            text-align: left;
            width: 409px;
        }

        .style98 {
            text-align: left;
        }

        .style99 {
            text-align: left;
            width: 41px;
        }

        .style101 {
            text-align: right;
        }

        .style104 {
            text-align: left;
        }

        .style109 {
            text-align: left;
            width: 53px;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
        }

        .style110 {
            text-align: center;
            width: 53px;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFCCFF;
        }

        .style111 {
            border: solid 0px;
        }

        .style118 {
            text-align: left;
        }

        .style119 {
            text-align: left;
        }

        .style132 {
            text-align: left;
        }

        .style134 {
            text-align: left;
        }

        .style136 {
            text-align: left;
        }

        .style138 {
        }

        .style140 {
            text-align: left;
        }

        .style143 {
            width: 44px;
        }

        .style144 {
            width: 41px;
        }

        .style145 {
            width: 50px;
            text-align: left;
        }

        .style153 {
            text-align: left;
            width: 210px;
        }

        .style154 {
            width: 245px;
        }

        .style158 {
            text-align: left;
            width: 77px;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
        }

        .style159 {
            text-align: left;
            width: 95px;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
        }

        .style160 {
            text-align: left;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
        }

        .style161 {
        }

        .style_tr {
            border: 1px solid maroon;
        }

        .style162 {
            text-align: center;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
            width: 48px;
        }

        .style163 {
            text-align: left;
            width: 68px;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
        }

        .style164 {
            text-align: left;
            width: 66px;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
        }

        .style165 {
            text-align: left;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
            width: 32px;
        }

        .style166 {
            text-align: left;
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
            width: 46px;
        }

        .style167 {
            text-align: left;
            width: 386px;
        }

        .style168 {
            text-align: left;
            width: 40px;
        }
    </style>

    <script src="jquery/IDNO_Vaild.js" type="text/javascript"></script>

    <script src="jquery/jquery.js" type="text/javascript"></script>

    <script type="text/javascript">
        var errorMsg;
        function CV_ID(sender, args) {
            inputValue = args.Value;
            if (inputValue === '') {
                args.IsValid = false;
                sender.innerHTML = "請輸入身分證字號"
            }
            else if (!idChech($(":text[id$=TextBox_MZ_ID]").get(0))) { //idChech函式傳入參數為 text 的 Dom 物件
                args.IsValid = false;
                sender.innerHTML = errorMsg;
            }
        }
    </script>

    <%--<script language="javascript" type="text/javascript">
        function toLocalDate(source, clientside_arguments) {
            var DateValue = source._textbox.get_Value();
            var year = parseInt(DateValue.substr(0, 4)) - 1911;
            source._textbox.set_Value(year + DateValue.substr(4, 6));
        }
    </script>--%>

    <script type="text/javascript">
        $(function () {
            $("input").attr("readonly", "readonly");
            $("#ctl00_ContentPlaceHolder1_TextBox_STOCKPILE_BANKID").attr("readonly", false);
        });        
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True">
    </asp:ScriptManager>
    <div style="width: 100%;">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>

                <div style="width: 100%;">
                    <div style="width: 100%;">
                        <table style="background-color: #6699FF; color: White; width: 100%;">
                            <tr>
                                <td class="style97">
                                    <span class="style35">個人基本資料管理</span>
                                </td>
                                <td class="style96">
                                    <span class="style35">
                                        <asp:Label ID="Label1" runat="server" CssClass="style101" Text="Label" Visible="False"></asp:Label>
                                    </span>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="width: 83%; height: 135px; float: left;">
                        <asp:Panel ID="Panel_Title" runat="server">
                            <table border="1" style="width: 100%; text-align: left;">
                                <tr>
                                    <td colspan="4">
                                        <table border="1" style="width: 100%; text-align: left;">
                                            <tr>
                                                <td class="style110" style="width: 40px;">姓名
                                                </td>
                                                <td class="style2" style="width: 130px;">
                                                    <asp:TextBox ID="TextBox_MZ_NAME" runat="server" Enabled="False" MaxLength="12"
                                                        Width="70px"></asp:TextBox>


                                                    <asp:TextBox ID="txt_Account" runat="server" BackColor="White" CssClass="style111"
                                                        TabIndex="-1" Width="50px" Font-Bold="True" ForeColor="#0033CC" Enabled="False"></asp:TextBox>

                                                </td>

                                                <td class="style110">身分證號
                                                </td>
                                                <td class="style14" style="width: 85px;">
                                                    <asp:TextBox ID="TextBox_MZ_ID" runat="server" Enabled="False" MaxLength="10" meta:resourcekey="TextBox_MZ_IDResource1"
                                                        Width="80px"></asp:TextBox>
                                                    <%--<asp:CustomValidator ID="CV_ID" runat="server" ControlToValidate="TextBox_MZ_ID"
                                            Display="Dynamic" ValidateEmptyText="True" ClientValidationFunction="CV_ID"></asp:CustomValidator>--%>
                                                </td>
                                                <td class="style110">發薪機關
                                                </td>
                                                <td class="style161">
                                                    <asp:TextBox ID="TextBox_PAY_AD" runat="server" AutoPostBack="True" Enabled="False"
                                                        MaxLength="10" Width="75px"></asp:TextBox>
                                                    <asp:TextBox ID="TextBox_PAY_AD1" runat="server" BackColor="White" CssClass="style111" Font-Bold="True" ForeColor="#0033CC" TabIndex="-1" Width="115px"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>

                                </tr>
                                <tr>
                                    <td class="style110">編制機關
                                    </td>
                                    <td class="style161">
                                        <asp:TextBox ID="TextBox_MZ_AD" runat="server" AutoPostBack="True" Enabled="False"
                                            MaxLength="10" Width="75px"></asp:TextBox>
                                        <asp:TextBox ID="TextBox_MZ_AD1" runat="server" BackColor="White" CssClass="style111"
                                            TabIndex="-1" Width="200px" Font-Bold="True" ForeColor="#0033CC"></asp:TextBox>
                                    </td>
                                    <td class="style110">編制單位
                                    </td>
                                    <td class="style14">
                                        <asp:TextBox ID="TextBox_MZ_UNIT" runat="server" AutoPostBack="True" Enabled="False"
                                            MaxLength="4" Width="35px"></asp:TextBox>
                                        <asp:TextBox ID="TextBox_MZ_UNIT1" runat="server" BackColor="White" CssClass="style111"
                                            TabIndex="-1" Width="105px" Font-Bold="True" ForeColor="#0033CC"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style110">現服機關
                                    </td>
                                    <td class="style161">
                                        <asp:TextBox ID="TextBox_MZ_EXAD" runat="server" AutoPostBack="True" Enabled="False"
                                            MaxLength="10" Width="75px"></asp:TextBox>
                                        <asp:TextBox ID="TextBox_MZ_EXAD1" runat="server" BackColor="White" CssClass="style111"
                                            TabIndex="-1" Width="200px" Font-Bold="True" ForeColor="#0033CC"></asp:TextBox>
                                    </td>
                                    <td class="style110">現服單位
                                    </td>
                                    <td class="style14">
                                        <asp:TextBox ID="TextBox_MZ_EXUNIT" runat="server" AutoPostBack="True" Enabled="False"
                                            MaxLength="4" Width="35px"></asp:TextBox>
                                        <asp:TextBox ID="TextBox_MZ_EXUNIT1" runat="server" BackColor="White" CssClass="style111"
                                            TabIndex="-1" Width="105px" Font-Bold="True" ForeColor="#0033CC"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <table border="1" style="border-style: solid; border-width: 1px; width: 100%; text-align: left;">
                            <tr>
                                <td style="background-color: #CCFFFF;">
                                    <asp:Panel ID="Panel_ButtonChange" runat="server">
                                        <table style="background-color: #CCFFFF; width: 100%; text-align: left;">
                                            <tr>

                                                <td class="style93">
                                                    <asp:Button ID="btBankSet" runat="server" CssClass="style98" TabIndex="-1"
                                                        Text="薪資帳號" Width="75px" CausesValidation="False" Font-Bold="True"
                                                        ForeColor="Maroon" />

                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </div>
                    
                    <div style="width: 17%; height: 135px; float: right;">
                        <div style="width: 83%; height: 135px; float: left;">
                            <asp:Image ID="Image1" runat="server" Height="130px" ImageUrl="~/1-personnel/images/nopic.jpg"
                                meta:resourcekey="Image1Resource1" Width="101px" Style="text-align: left" />
                        </div>
                        <div style="width: 17%; height: 135px; float: right;">
                            <div>
                                <asp:Button ID="btUpperPic" runat="server" Text="△" CausesValidation="False" Enabled="False"
                                    Font-Bold="True" ForeColor="Maroon" />
                            </div>
                            <div>
                                <asp:Button ID="btDeletePic" runat="server" Text="╳" CausesValidation="False"
                                    Font-Bold="True" ForeColor="Maroon" />
                            </div>
                            <div>
                                <asp:Button ID="btNextPic" runat="server" Text="▽" CausesValidation="False" Enabled="False"
                                    Font-Bold="True" ForeColor="Maroon" />
                            </div>
                            <asp:Label ID="xcount1" runat="server" Text="" Visible="False"></asp:Label>
                        </div>
                    </div>
                    <div>
                        <cc1:ModalPopupExtender ID="btn_showSearch_ModalPopupExtender" runat="server" DynamicServicePath=""
                            Enabled="True" TargetControlID="btTable" PopupControlID="Panel1" BackgroundCssClass="modalBackground">
                        </cc1:ModalPopupExtender>
                        <asp:Panel ID="Panel1" runat="server" Style="display: none;" CssClass="DivPanel">
                            <div style="text-align: right;">
                                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/Back.gif" />
                            </div>
                            <uc3:PoliceSearchPanel ID="PoliceSearchPanel1" runat="server" />
                        </asp:Panel>
                    </div>
                </div>
                <br />
                <br />
                <br />

                <asp:Panel ID="Panel_v4" runat="server" Height="350px" GroupingText="銀行資料">
                    <table class="TableStyleBlue" style="width: 100%;">
                        <tr>
                            <th>
                                <a class="must">*群組</a>
                            </th>
                            <td colspan="3" style="text-align: left;">
                                <asp:DropDownList ID="DropDownList_GROUP" runat="server">
                                    <asp:ListItem Value="1">優惠存款</asp:ListItem>
                                    <asp:ListItem Selected="True" Value="2">薪資轉帳</asp:ListItem>
                                    <asp:ListItem Value="3">國宅貸款</asp:ListItem>
                                    <asp:ListItem Value="4">退撫金貸款</asp:ListItem>
                                    <asp:ListItem Value="5">分期付款</asp:ListItem>
                                    <asp:ListItem Value="6">銀行代款</asp:ListItem>
                                    <asp:ListItem Value="7">法院扣款</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                <a class="must">*銀行</a>
                            </th>
                            <td style="text-align: left;">
                                <asp:DropDownList ID="DropDownList_BANK" runat="server" DataSourceID="SqlDataSource_BANK_LIST"
                                    DataTextField="IDNAME" DataValueField="BANK_ID">
                                </asp:DropDownList>
                                <asp:SqlDataSource ID="SqlDataSource_BANK_LIST" runat="server" ConnectionString="<%$ ConnectionStrings:MSDBConnectionString_toTest %>"
                                    ProviderName="<%$ ConnectionStrings:MSDBConnectionString_toTest.ProviderName %>" SelectCommand="SELECT BANK_ID, BANK_NAME, BANK_ID +'('+ BANK_NAME +')' AS IDNAME FROM B_BANK_LIST ORDER BY BANK_ID"></asp:SqlDataSource>
                            </td>
                            <th>
                                <a class="must">*帳號</a>
                            </th>
                            <td style="text-align: left;">
                                <asp:TextBox ID="TextBox_STOCKPILE_BANKID" runat="server" MaxLength="16"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="TextBox_STOCKPILE_BANKID_FilteredTextBoxExtender"
                                    runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="TextBox_STOCKPILE_BANKID"
                                    ValidChars="-">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                        </tr>
                    </table>
                    <asp:GridView ID="GridView_STOCKPILE" runat="server" CellPadding="4" GridLines="None"
                        Width="100%" AutoGenerateColumns="False" DataKeyNames="BS_SNID,PAY_AD" EmptyDataText="無銀行資料"
                        OnRowCommand="GridView_STOCKPILE_RowCommand" ForeColor="#333333" OnRowDataBound="GridView_STOCKPILE_RowDataBound"
                        AllowPaging="True" PageSize="7" OnPageIndexChanging="GridView_STOCKPILE_PageIndexChanging">
                        <RowStyle BackColor="#EFF3FB" />
                        <Columns>
                            <%--<asp:ButtonField CommandName="btSelect" HeaderText="選取" Text="選取" />--%>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Button ID="btnSelect" runat="server" Text="選取" CommandName="btSelect" CommandArgument='<%# Eval("BS_SNID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="BANK_NAME_DATA" HeaderText="銀行" SortExpression="BANK_NAME_DATA" />
                            <asp:BoundField DataField="STOCKPILE_BANKID" HeaderText="銀行帳號" SortExpression="STOCKPILE_BANKID" />
                            <asp:BoundField DataField="GROUP" HeaderText="群組" SortExpression="GROUP" />
                            <asp:BoundField DataField="MZ_KCHI" HeaderText="機關" SortExpression="PAY_AD" />
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:Button ID="btn_DeleteAccount" runat="server" CausesValidation="false" CommandName="btDelete"
                                        CommandArgument='<%# Eval("BS_SNID") %>' OnClientClick="return confirm('確定要刪除？');"
                                        Text="刪除" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True" />
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <EditRowStyle BackColor="#2461BF" />
                        <AlternatingRowStyle BackColor="White" />
                    </asp:GridView>
                </asp:Panel>
                <asp:Panel ID="Panel_00" runat="server">
                    <table class="style1">
                        <tr>
                            <td style="background-color: #6699FF; color: White;">
                                <asp:Button ID="btTable" runat="server" OnClick="btTable_Click" Text="查詢" Visible="False" />
                                <asp:Button ID="btCreate" runat="server" OnClick="btCreate_Click" Text="新增" />
                                <asp:Button ID="btUpdate" runat="server" Text="儲存" OnClick="btUpdate_Click" />
                                <asp:Button ID="btBack_Data" runat="server" Text="上一筆" OnClick="btBack_Data_Click" />
                                <asp:Label ID="xcount" runat="server" meta:resourcekey="xcountResource1" Visible="False"></asp:Label>
                                <asp:Button ID="btNext_Data" runat="server" Text="下一筆" OnClick="btNext_Data_Click" />
                                <asp:Button ID="btExit" runat="server" Text="返回" OnClientClick="history.back();" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
