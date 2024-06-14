 <%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Online_work.aspx.cs" Inherits="TPPDDB.Online_Leave.Online_work" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../styles/Stylesheet.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .TableStyleWithTD {  
            border-collapse: collapse;
            border: solid 1px dimgray;
            font-family: 微軟正黑體;
        }

            .TableStyleWithTD th {
                text-align: right;
                background-color: #EEEEFC;
                border: solid 1px dimgray;
                font-size: 10pt;
                padding-right: 6px;
            }

            .TableStyleWithTD td {
                height: 25px;
                border: solid 1px dimgray;
                padding-left: 6px;
                font-size: 10pt;
                text-align: left;
                background-color: White;
            }

        .headerA {
            background-image: url('../18-Online_Leave/images/K/headerAA.jpg');
            background-repeat: no-repeat;
            height: 67px;
            line-height: 25px;
            font-size: 16px;
            font-weight: bold;
            width: 930px;
        }

        .headerA2 {
            background-image: url('../18-Online_Leave/Images/K/headerBB.JPG');
            background-repeat: no-repeat;
            height: 38px;
            line-height: 25px;
            font-size: 16px;
            font-weight: bold;
            width: 930px;
        }

        .style58 {
            width: 100%;
        }

        .style59 {
            margin-left: 0px;
        }

        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }

        .modalPopup {
            border: 3px solid White;
            background-color: White;
            padding: 3px;
        }
    </style>
    <title></title>
</head>
<body style="margin-top: -1px; margin-left: -1px;">
    <form id="form1" runat="server">

        <script type="text/javascript" language="javascript">
            var ModalProgress = '<%= Panel_Progress_ModalPopupExtender.ClientID %>';
        </script>

        <script type="text/javascript" src="jsUpdateProgress.js"></script>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div>
                    <table cellpadding="0">
                        <tr>
                            <td class="headerA">
                                <div style="left: 427px; position: absolute; width: 500px; top: 74px; text-align: right;">
                                    <asp:Label ID="Label_ADData" runat="server" Text="Label"></asp:Label>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="headerA2" style="text-align: center; font-size: 14pt; font-family: 微軟正黑體;">
                                <asp:Label ID="Label2" runat="server" Text="線上簽核"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="width: 930px">
                    <div style="float: left; width: 20%">
                        <%--左列按鈕區塊--%>
                        <table style="width: 100%; background-color: #B0E2FF; min-height: 480px; height: 480px; font-family: 微軟正黑體;">
                            <tr>
                                <td style="height: 5%;">
                                    <asp:LinkButton ID="LinkButton1" runat="server" OnClick="back_Click">回首頁</asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 5%;">
                                    <asp:LinkButton ID="lbtn_sign" runat="server" OnClick="lbtn_sign_Click">差假線上簽核</asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 5%;">
                                    <asp:LinkButton ID="btn_left_overtime" runat="server" OnClick="btn_left_overtime_Click">加班線上簽核</asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 5%;">
                                    <asp:LinkButton ID="lbtn_agents" runat="server" OnClick="lbtn_agents_Click">代理人線上簽核</asp:LinkButton>
                                </td>
                            </tr>
                            <%--0826→Dean 差旅費簽核button--%>
                            <tr>
                                <td style="height: 5%;">
                                    <asp:LinkButton ID="lbtn_bussinesstrip" runat="server" OnClick="lbtn_bussinesstrip_Click">差旅費線上簽核</asp:LinkButton>
                                </td>
                            </tr>
                            <%--1212→kim 銷假簽核button--%>
                            <tr>
                                <td style="height: 5%;">
                                    <asp:LinkButton ID="lbtn_change_dltb01" runat="server" OnClick="lbtn_change_dltb01_Click"> 銷假線上簽核</asp:LinkButton>
                                </td>
                            </tr>
                            <%--0812→Dean--%>
                            <tr>
                                <td style="height: 5%;">
                                    <asp:LinkButton ID="lbtn_re" runat="server" OnClick="lbtn_re_Click">抽回差假簽核</asp:LinkButton>
                                </td>
                            </tr>
                            <%--0826→Dean 差旅費抽回button--%>
                            <tr>
                                <td style="height: 5%;">
                                    <asp:LinkButton ID="lbtn_bussiness_back" runat="server" OnClick="lbtn_bussiness_back_Click">抽回差旅費簽核</asp:LinkButton>
                                </td>
                            </tr>
                            <%--1212→Dean 銷假抽回button--%>
                            <tr>
                                <td style="height: 5%;">
                                    <asp:LinkButton ID="lbtn_change_dltb01_back" runat="server" OnClick="lbtn_change_dltb01_back_Click">抽回銷假簽核</asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 5%;">
                                    <asp:LinkButton ID="lbtn_flow" runat="server" OnClick="lbtn_flow_Click">個人簽核狀態</asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 5%;">
                                    <asp:LinkButton ID="lbtn_flow_all" runat="server" OnClick="lbtn_flow_all_Click">查詢簽核狀態</asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 5%;">
                                    <asp:LinkButton ID="lbtn_person" runat="server" OnClick="lbtn_person_Click">已決行案件</asp:LinkButton>
                                </td>
                            </tr>
                            <%--0826→Dean 差旅費已決行button--%>
                            <%-- <tr>
                            <td style="height: 5%;">
                                <asp:LinkButton ID="lbtn_bussiness_decision" runat="server" OnClick="lbtn_bussiness_decision_Click">已決行差旅費簽核案件</asp:LinkButton>
                            </td>
                        </tr>--%>
                            <tr>
                                <td style="height: 5%;">
                                    <asp:LinkButton ID="btn_allOK" runat="server" OnClick="btn_allOK_Click">已核定假單</asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 5%;">
                                    <asp:LinkButton ID="btn_all_b_OK" runat="server" OnClick="btn_all_b_OK_Click">已核定差旅費</asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 5%;">
                                    <asp:LinkButton ID="btn_all_c_OK" runat="server" OnClick="btn_all_c_OK_Click">已核定銷假單</asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 5%;">
                                    <asp:LinkButton ID="btn_left_overtime_decision" runat="server" OnClick="btn_left_overtime_decision_Click">已決行加班案件</asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 65%;"></td>
                            </tr>
                        </table>
                    </div>
                    <div style="float: right; width: 80%">
                        <%--右列資料區塊--%>
                        <table style="width: 100%; font-family: 微軟正黑體;">
                            <tr>
                                <td>
                                    <div style="float: left; width: 50%">
                                        <div style="text-align: right; width: 100%;">
                                            <asp:Label ID="lbl_sign" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <div style="float: right; width: 50%">
                                        <!--一般差假-->
                                        <!--主管簽核-->
                                        <asp:Panel ID="pl_data" runat="server" Visible="False">
                                            <div style="text-align: right; width: 100%;">
                                                <asp:Button ID="btn_back" runat="server" Style="background-color: White;" Width="80px"
                                                    Text="退回" OnClick="btn_back_Click" OnClientClick="return confirm('確定退回嗎？');" />
                                                <asp:Button ID="btn_upper" runat="server" Style="background-color: White;" Text="第一層陳核"
                                                    Width="80px" OnClick="btn_upper_Click" />
                                                <asp:Button ID="btn_sign" runat="server" Style="background-color: Blue; color: White;"
                                                    Text="陳核" OnClick="btn_sign_Click" Width="80px" />
                                                <asp:Button ID="btn_check" runat="server" Style="background-color: #DC143C; color: White;"
                                                    Text="決行" Width="80px" OnClick="btn_check_Click" />
                                            </div>
                                        </asp:Panel>
                                        <!--代理人簽核-->
                                        <asp:Panel ID="pl_agents" runat="server" Visible="False">
                                            <div style="text-align: right; width: 100%;">
                                                <asp:Button ID="btn_notagree" runat="server" Style="background-color: White;" Text="不同意"
                                                    Width="80px" OnClick="btn_notagree_Click" />
                                                <asp:Button ID="btn_agree" runat="server" Style="background-color: #DC143C; color: White;"
                                                    Text="同意" Width="80px" OnClick="btn_agree_Click" />
                                            </div>
                                        </asp:Panel>
                                        <!--已決行案件(人事室呈辦人)-->
                                        <asp:Panel ID="pl_person" runat="server" Visible="False">
                                            <div style="text-align: right; width: 100%;">
                                                <asp:Button ID="btn_no" runat="server" Style="background-color: White;" Text="退回"
                                                    CommandName="no" OnCommand="person_command" />
                                                <asp:Button ID="btn_ok" runat="server" Style="background-color: #DC143C; color: White;"
                                                    Text="核定" CommandName="ok" OnCommand="person_command" />
                                            </div>
                                        </asp:Panel>
                                        <!--加班陳核按鈕-->
                                        <asp:Panel ID="pl_overtime" runat="server" Visible="False">
                                            <div style="text-align: right; width: 100%;">
                                                <asp:Button ID="btn_overtime_no" runat="server" Style="background-color: White;"
                                                    Text="退回" CommandName="OverTime_no" OnClientClick="return confirm('確定退回嗎？');"
                                                    OnClick="btn_overtime_no_Click" />
                                                <!--暫時只處理一層-->
                                                <asp:Button ID="btn_overtime_sign" runat="server" Style="background-color: Blue; color: White;"
                                                    Text="陳核" CommandName="OverTime_sign" OnClick="btn_overtime_sign_Click"  />
                                                <!--暫時只處理一層-->
                                                <asp:Button ID="btn_overtime_yes" runat="server" Style="background-color: #DC143C; color: White;"
                                                    Text="決行" CommandName="OverTime_yes" OnClick="btn_overtime_yes_Click" />
                                            </div>
                                        </asp:Panel>
                                        <!--加班修改按鈕-->
                                        <asp:Panel ID="pl_overtime_person" runat="server" Visible="False">
                                            <div style="text-align: right; width: 100%;">
                                                <asp:Button ID="btn_overtime_person_no" runat="server" Text="退回" CommandName="overtime_person_no"
                                                    OnClick="btn_overtime_person_no_Click" />
                                                <asp:Button ID="btn_overtime_person_yes" runat="server" Style="background-color: #DC143C; color: White;"
                                                    Text="核定" CommandName="overtime_person_yes" OnClick="btn_overtime_person_yes_Click" />
                                            </div>
                                        </asp:Panel>
                                        <!--抽回差假簽核-->   
                                        <asp:Panel ID="Panel_re" runat="server" Visible="false">
                                            <div style="text-align: right;">
                                                <asp:Button ID="btn_re" runat="server" Text="抽回" OnClick="btn_re_Click" />
                                            </div>
                                        </asp:Panel>
                                        <!--差旅費線上簽核-->
                                        <asp:Panel ID="Panel_bussiness_btn" runat="server" Visible="False">
                                            <div style="text-align: right; width: 100%;">
                                                <asp:Button ID="btn_b_back" runat="server" Style="background-color: White;" Width="80px"
                                                    Text="退回" OnClick="btn_b_back_Click" OnClientClick="return confirm('確定退回嗎？');" />
                                                <asp:Button ID="btn_b_upper" runat="server" Style="background-color: White;" Text="第一層陳核"
                                                    Width="80px" OnClick="btn_b_upper_Click" />
                                                <asp:Button ID="btn_b_sign" runat="server" Style="background-color: Blue; color: White;"
                                                    Text="陳核" Width="80px" OnClick="btn_b_sign_Click" />
                                                <asp:Button ID="btn_b_check" runat="server" Style="background-color: #DC143C; color: White;"
                                                    Text="決行" Width="80px" OnClick="btn_b_check_Click" />
                                            </div>
                                        </asp:Panel>
                                        <%--  <asp:Panel ID="Panel_bussiness_check_btn" runat="server" Visible="False">
                                        <div style="text-align: right; width: 100%;">
                                            <asp:Button ID="btn_b_no" runat="server" Text="退回" OnClick="btn_b_no_Click" OnClientClick="return confirm('確定退回嗎？');" />
                                            <asp:Button ID="btn_b_yes" runat="server" Style="background-color: #DC143C; color: White;"
                                                Text="核定" OnClick="btn_b_yes_Click" />
                                        </div>
                                    </asp:Panel>--%>
                                        <asp:Panel ID="Panel_b_re" runat="server" Visible="false">
                                            <div style="text-align: right;">
                                                <asp:Button ID="btn_b_re" runat="server" Text="抽回" OnClick="btn_b_re_Click" />
                                            </div>
                                        </asp:Panel>
                                        <!--銷假決行-->
                                        <asp:Panel ID="Panel_change_dltb01_btn" runat="server" Visible="False">
                                            <div style="text-align: right; width: 100%;">
                                                <asp:Button ID="btn_change_dltb01_back" runat="server" Style="background-color: White;"
                                                    Width="80px" Text="退回" OnClientClick="return confirm('確定退回嗎？');" OnClick="btn_change_dltb01_back_Click" />
                                                <asp:Button ID="btn_change_dltb01_upper" runat="server" Style="background-color: White;"
                                                    Text="第一層陳核" Width="80px" OnClick="btn_change_dltb01_upper_Click" />
                                                <asp:Button ID="btn_change_dltb01_sign" runat="server" Style="background-color: Blue; color: White;"
                                                    Text="陳核" OnClick="btn_change_dltb01_sign_Click" Width="80px" />
                                                <asp:Button ID="btn_change_dltb01_check" runat="server" Style="background-color: #DC143C; color: White;"
                                                    Text="決行" OnClick="btn_change_dltb01_check_Click" Width="80px" />
                                            </div>
                                        </asp:Panel>
                                        <asp:Panel ID="Panel_change_dltb01_re" runat="server" Visible="false">
                                            <div style="text-align: right;">
                                                <asp:Button ID="btn_change_dltb01_re" runat="server" Text="抽回" OnClick="btn_change_dltb01_re_Click" />
                                            </div>
                                        </asp:Panel>
                                        <asp:Panel ID="pl_self_item" runat="server" Visible="False">
                                            <div style="text-align: right; width: 100%;">
                                                <asp:LinkButton ID="lbtn_self_data" runat="server" Text="請假紀錄" OnClick="lbtn_flow_Click"></asp:LinkButton>
                                                &nbsp;&nbsp;
                                            <asp:LinkButton ID="lbtn_self_overtime" runat="server" Text="加班紀錄" OnClick="lbtn_overtime_self_Click"></asp:LinkButton>
                                                &nbsp;&nbsp;
                                            <asp:LinkButton ID="lbtn_self_businesstrip" runat="server" Text="差旅費紀錄" OnClick="lbtn_self_businesstrip_Click"></asp:LinkButton>
                                                &nbsp;&nbsp;
                                            <asp:LinkButton ID="lbtn_self_change_dltb01" runat="server" Text="銷假紀錄" OnClick="lbtn_self_change_dltb01_Click"></asp:LinkButton>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <%--0812→Dean--%>
                                    <asp:Panel ID="drop" runat="server" Visible="False">
                                        <asp:Label ID="Label1" runat="server" Text="年："></asp:Label>
                                        <asp:TextBox ID="txt_m" runat="server" Width="73px"></asp:TextBox>
                                        <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True" OnInit="DropDownList1_Init"
                                            OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
                                            <asp:ListItem Value="01">1月</asp:ListItem>
                                            <asp:ListItem Value="02">2月</asp:ListItem>
                                            <asp:ListItem Value="03">3月</asp:ListItem>
                                            <asp:ListItem Value="04">4月</asp:ListItem>
                                            <asp:ListItem Value="05">5月</asp:ListItem>
                                            <asp:ListItem Value="06">6月</asp:ListItem>
                                            <asp:ListItem Value="07">7月</asp:ListItem>
                                            <asp:ListItem Value="08">8月</asp:ListItem>
                                            <asp:ListItem Value="09">9月</asp:ListItem>
                                            <asp:ListItem Value="10">10月</asp:ListItem>
                                            <asp:ListItem Value="11">11月</asp:ListItem>
                                            <asp:ListItem Value="12">12月</asp:ListItem>
                                        </asp:DropDownList>
                                        <br />
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                        <%--差假簽核S--%>
                        <asp:Panel ID="Panel_search" runat="server" GroupingText="差假簽核搜尋">
                            <table style="font-family: 微軟正黑體; height: 53px;" class="style58">
                                <tr>
                                    <td style="width: 40px;">機關
                                    </td>
                                    <td style="width: 150px;">
                                        <asp:DropDownList ID="DDL_AD" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DDL_AD_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 40px;">單位
                                    </td>
                                    <td style="width: 150px;">
                                        <asp:DropDownList ID="DDL_UNIT" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                    <td>類別
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_Kind" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddl_Kind_SelectedIndexChanged"
                                            Visible="False">
                                            <asp:ListItem>差假</asp:ListItem>
                                            <asp:ListItem>銷假</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 40px;">姓名
                                    </td>
                                    <td style="width: 150px;">
                                        <asp:TextBox ID="txt_name" Width="100px" runat="server"></asp:TextBox>
                                    </td>
                                    <td style="width: 90px;">身份證字號
                                    </td>
                                    <td style="width: 100px;">
                                        <asp:TextBox ID="txt_idno" runat="server" Width="100px"></asp:TextBox>
                                    </td>
                                    <td style="width: 40px;">日期
                                    </td>
                                    <td style="width: 190px;">
                                        <asp:TextBox ID="txt_date" runat="server" Width="70px"></asp:TextBox>～
                                                <asp:TextBox ID="txt_date1" runat="server" Width="70px"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <div style="text-align: center;">
                                <asp:Button ID="btn_search" runat="server" Text="搜尋" CssClass="style59" OnClick="btn_search_Click" />
                            </div>
                        </asp:Panel>
                        <br />
                        <asp:Panel ID="pl_data2" runat="server" GroupingText="差假記錄">
                            <asp:CheckBox ID="ckboxall" runat="server" onclick="javascript:checkallbox(this);"/>全選
                            <!--差假記錄Gridview-->
                            <asp:GridView ID="gv_data" runat="server" AutoGenerateColumns="False" OnRowDataBound="gv_data_RowDataBound"
                                Width="100%" DataKeyNames="SN,MZ_NAME,LEAVE_SN,MZ_DLTB01_SN,LEAVE_SCHEDULE_SN"
                                CellPadding="4" ForeColor="#333333" GridLines="None" AllowPaging="True" OnPageIndexChanging="gv_data_PageIndexChanging"
                                Style="text-align: center">
                                <RowStyle BackColor="#EFF3FB" />
                                <Columns>
                                    <asp:TemplateField HeaderText="選取">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="ckbox_select" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="差假日期">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="hlink1" runat="server" OnCommand="lbtn_content" CommandArgument='<%# Eval("MZ_DLTB01_SN") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="MZ_CNAME" HeaderText="假別" />
                                    <asp:BoundField DataField="MZ_EXAD" HeaderText="機關" />
                                    <asp:BoundField DataField="MZ_EXUNIT" HeaderText="單位" />
                                    <asp:BoundField DataField="MZ_NAME" HeaderText="申請人" />
                                    <asp:TemplateField HeaderText="歷程">
                                        <ItemTemplate>
                                       <%-- <asp:Button ID="btn_history" runat="server" CommandArgument='<%# Eval("MZ_DLTB01_SN") %>'
                                                Text="歷程" OnCommand="btn_history_Command" />--%>

                                       <%--dave 2020/07/29 假單歷程錯誤  MZ_DLTB01_SN=2455707=>改成傳回 SN  --%>
                                       <%--<asp:Button ID="btn_history" runat="server" CommandArgument='<%# Eval("SN") %>'
                                           Text="歷程" OnCommand="btn_history_Command" />--%>
                                       <%--dave 2020/08/12 參數改成2個 --%>
                                       <asp:Button ID="btn_history" runat="server" CommandArgument='<%#Eval("MZ_DLTB01_SN") %>'
                                           Text="歷程" OnCommand="btn_history_Command" />

                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="意見">
                                        <ItemTemplate>
                                            <asp:Button ID="btn_message" runat="server" CommandArgument='<%# Eval("MZ_DLTB01_SN") %>'
                                                Text="填寫" OnCommand="btn_message_Command" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="附檔">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# "~/Files/C_upload/"+ Eval("MZ_FILE") %>'
                                                Target="_blank">1.</asp:HyperLink>
                                            <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl='<%# "~/Files/C_upload/"+ Eval("MZ_FILE1") %>'
                                                Target="_blank">2.</asp:HyperLink>
                                            <asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl='<%# "~/Files/C_upload/"+ Eval("MZ_FILE2") %>'
                                                Target="_blank">3.</asp:HyperLink>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#2461BF" />
                                <AlternatingRowStyle BackColor="White" />
                            </asp:GridView>
                        </asp:Panel>
                        <br />
                        <asp:Panel ID="pl_history" runat="server" GroupingText="差假簽核歷程">
                            <div style="text-align: right;" runat="server" id="process_bar">
                                <asp:Button ID="btn_bef_process" runat="server" Text="退回上個流程" OnCommand="btn_bef_process_Command" />
                            </div>
                            <asp:GridView ID="gv_history" runat="server" AutoGenerateColumns="False" OnRowDataBound="gv_history_RowDataBound"
                                CellPadding="4" ForeColor="#333333" GridLines="None" Width="100%" Style="text-align: center">
                                <RowStyle BackColor="#EFF3FB" />
                                <Columns>
                                    <asp:TemplateField HeaderText="簽核時間">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_history_date" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="機關" DataField="MZ_EXAD" />
                                    <asp:BoundField HeaderText="單位" DataField="MZ_EXUNIT" />
                                    <asp:BoundField HeaderText="職稱" DataField="MZ_OCCC" />
                                    <asp:BoundField HeaderText="審核者" DataField="MZ_NAME" />
                                    <asp:BoundField HeaderText="狀態" DataField="C_STATUS_NAME" />
                                    <asp:BoundField DataField="REVIEW_MESSAGE" HeaderText="意見" />
                                </Columns>
                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#2461BF" />
                                <AlternatingRowStyle BackColor="White" />
                            </asp:GridView>
                        </asp:Panel>
                        <cc1:ModalPopupExtender ID="P_E_ModalPopupExtender" runat="server" BackgroundCssClass="modalBackground"
                            DynamicServicePath="" Enabled="True" PopupControlID="pl_Show" TargetControlID="btn_fake">
                        </cc1:ModalPopupExtender>
                        <asp:Button ID="btn_fake" runat="server" Style="display: none;" Text="Button" />
                        <asp:Panel ID="pl_Show" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Double"
                            Style="display: none;">
                            <table style="width: 800px;">
                                <tr>
                                    <td style="background: #CCCCFF; width: 120px;">差假申請人：
                                    </td>
                                    <td style="background: #FFDAB5; width: 280px;">
                                        <asp:Label ID="lbl_MZ_NAME" runat="server" Text="Label"></asp:Label>
                                    </td>
                                    <td style="background: #CCCCFF; width: 180px;">差假類別：
                                    </td>
                                    <td style="background: #FFDAB5; width: 220px;">
                                        <asp:Label ID="lbl_MZ_CNAME" runat="server" Text="Label"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="background: #CCCCFF; width: 120px;">職稱：</td>
                                    <td style="background: #FFDAB5; width: 280px;">
                                        <asp:Label ID="lblOccc" runat="server"></asp:Label>
                                    </td>
                                    <td style="background: #CCCCFF; width: 160px;">身份證號：</td>
                                    <td style="background: #FFDAB5; width: 240px;">
                                        <asp:Label ID="lblMZ_ID" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="background: #CCCCFF;">現服機關：
                                    </td>
                                    <td style="background: #FFDAB5;">
                                        <asp:Label ID="lbl_MZ_EXAD" runat="server" Text="Label"></asp:Label>
                                    </td>
                                    <td style="background: #CCCCFF;">現服單位：
                                    </td>
                                    <td style="background: #FFDAB5;">
                                        <asp:Label ID="lbl_MZ_EXUNIT" runat="server" Text="Label"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="background: #CCCCFF;">請假日期：
                                    </td>
                                    <td colspan="3" style="background: #FFDAB5;">
                                        <asp:Label ID="lbl_Date" runat="server" Text="Label"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="background: #CCCCFF;">請假事由：
                                    </td>
                                    <td style="background: #FFDAB5;">
                                        <asp:Label ID="lbl_MZ_CAUSE" runat="server" Text="Label"></asp:Label>
                                    </td>
                                    <td style="background: #CCCCFF;">已請休假天數：
                                    </td>
                                    <td style="background: #FFDAB5;">
                                        <asp:Label ID="lbl_QKDay" runat="server" Text="Label"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="background: #CCCCFF;">代理人：
                                    </td>
                                    <td style="background: #FFDAB5;">
                                        <asp:Label ID="lbl_agent" runat="server" Text="Label"></asp:Label>
                                    </td>
                                    <td style="background: #CCCCFF;">地點：
                                    </td>
                                    <td style="background: #FFDAB5;">
                                        <asp:Label ID="lbl_location" runat="server" Text="Label"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="background: #CCCCFF;">有無申請補助：
                                    </td>
                                    <td style="background: #FFDAB5;">
                                        <asp:Label ID="lbl_allowance" runat="server" Text="Label"></asp:Label>
                                    </td>
                                    <td style="background: #CCCCFF;">是否為出國、赴大陸：
                                    </td>
                                    <td style="background: #FFDAB5;">
                                        <asp:Label ID="lbl_abroad" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="background: #CCCCFF;">應休假日數：</td>
                                    <td style="background: #FFDAB5; margin-left: 40px;">
                                        <asp:Label ID="lblMZ__HDAY" runat="server"></asp:Label>
                                    </td>
                                    <td style="background: #CCCCFF;">假單建立日期：</td>
                                    <td style="background: #FFDAB5;">
                                        <asp:Label ID="lblADDDATE" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="background: #CCCCFF;">
                                        <asp:Label ID="lblPox" runat="server" Text=""></asp:Label>已請數：</td>
                                    <td style="background: #FFDAB5; margin-left: 40px;">
                                        <asp:Label ID="lbOtherDate" runat="server"></asp:Label>
                                    </td>
                                    <td style="background: #CCCCFF;">&nbsp;</td>
                                    <td style="background: #FFDAB5;">&nbsp;</td>
                                </tr>
                            </table>
                            <br />
                            <asp:GridView ID="gv_Business_Detail" runat="server" AutoGenerateColumns="False"
                                Visible="False" CellPadding="4" ForeColor="#333333" GridLines="None" AllowPaging="True"
                                PageSize="5" Width="800px" OnPageIndexChanging="gv_Business_Detail_PageIndexChanging">
                                <RowStyle BackColor="#EFF3FB" />
                                <Columns>
                                    <asp:BoundField HeaderText="月" DataField="MONTH"></asp:BoundField>
                                    <asp:BoundField HeaderText="日" DataField="DAY"></asp:BoundField>
                                    <asp:BoundField HeaderText="起迄地點" DataField="LOCATION"></asp:BoundField>
                                    <asp:BoundField HeaderText="工作記要" DataField="WORKNOTES"></asp:BoundField>
                                    <asp:BoundField HeaderText="總計" DataField="TOTAL"></asp:BoundField>
                                </Columns>
                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#2461BF" />
                                <AlternatingRowStyle BackColor="White" />
                            </asp:GridView>
                            <asp:Panel ID="pl_change_dltb01_show" runat="server">
                                <table>
                                    <tr>
                                        <td style="background: #CCCCFF;">原請假時間
                                        </td>
                                        <td style="background: #FFDAB5;">
                                            <asp:Label ID="ORIGINAL_DLTB01" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="background: #CCCCFF;">欲修改時間
                                        </td>
                                        <td style="background: #FFDAB5;">
                                            <asp:Label ID="NEW_DLTB01" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <div style="text-align: center; width: 100%;">
                                <asp:Button ID="Button1" runat="server" Text="確定" />
                            </div>
                        </asp:Panel>
                        <cc1:ModalPopupExtender ID="P_E_BACK" runat="server" BackgroundCssClass="modalBackground"
                            DynamicServicePath="" Enabled="True" PopupControlID="pl_message" TargetControlID="btn_fake2">
                        </cc1:ModalPopupExtender>
                        <asp:Button ID="btn_fake2" runat="server" Text="確定" Style="display: none;" />
                        <asp:Panel ID="pl_message" runat="server" BackColor="White" Style="text-align: center; display: none;">
                            <div style="border: solid 1px gray; background-color: #ADD8E6; width: 420px; height: 270px;">
                                <div style="margin: 10px; background-color: #FFFFFF; width: 400px; height: 250px;">
                                    <table class="TableStyleWithTD" width="100%">
                                        <tr>
                                            <th style="text-align: center;">填寫意見
                                            </th>
                                        </tr>
                                        <tr>
                                            <td style="text-align: center;">
                                                <asp:TextBox ID="txt_message" runat="server" Height="200px" TextMode="MultiLine"
                                                    Width="372px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: center;">
                                                <asp:Button ID="btn_ck" runat="server" Text="確定" OnClick="btn_ck_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </asp:Panel>
                        <cc1:ModalPopupExtender ID="Panel_select_ModalPopupExtender" runat="server" BackgroundCssClass="modalBackground"
                            DynamicServicePath="" Enabled="True" TargetControlID="btn_popup" PopupControlID="Panel_select">
                        </cc1:ModalPopupExtender>
                        <asp:Button ID="btn_popup" runat="server" Text="Button" Style="display: none;" />
                        <asp:Panel ID="Panel_select" runat="server" Style="display: none; text-align: center;"
                            Width="100%">
                            <div style="border: solid 1px gray; background-color: #ADD8E6; width: 320px; height: 350px;">
                                <div style="margin: 20px; background-color: #FFFFFF; width: 280px; height: 310px;">
                                    <div style="margin: 0px; text-align: center;">
                                        <h4 style="text-align: center; width: 100%;">陳核</h4>
                                        <asp:GridView ID="GV_CHECKER" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                            ForeColor="#333333" GridLines="None" AllowPaging="True" OnPageIndexChanging="GV_CHECKER_PageIndexChanging"
                                            OnRowCommand="GV_CHECKER_RowCommand" PageSize="5">
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
                                            <asp:Button ID="btn_bak" runat="server" Text="送出" Style="display: none;" />
                                            <asp:Button ID="btn_exit" runat="server" Text="取消" OnClick="btn_exit_Click" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                        <%--差假簽核E--%>
                        <%--加班簽核S--%>
                        <asp:Panel ID="pl_search" runat="server" Visible="false" GroupingText="加班簽核搜尋">
                            <table style="font-family: 微軟正黑體;" class="style58">
                                <tr>
                                    <td style="width: 40px;">機關
                                    </td>
                                    <td style="width: 150px;">
                                        <asp:DropDownList ID="DDL_EXAD" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DDL_EXAD_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 40px;">單位
                                    </td>
                                    <td style="width: 150px;">
                                        <asp:DropDownList ID="DDL_EXUNIT" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 40px;">姓名
                                    </td>
                                    <td style="width: 150px;">
                                        <asp:TextBox ID="txt_MZ_NAME" Width="100px" runat="server"></asp:TextBox>
                                    </td>
                                    <td style="width: 90px;">身份證字號
                                    </td>
                                    <td style="width: 100px;">
                                        <asp:TextBox ID="txt_MZ_ID" runat="server" Width="100px"></asp:TextBox>
                                    </td>
                                    <td style="width: 40px;">日期
                                    </td>
                                    <td style="width: 190px;">
                                        <asp:TextBox ID="txt_MZ_DATE" runat="server" Width="70px"></asp:TextBox>～
                                                <asp:TextBox ID="txt_MZ_DATE1" runat="server" Width="70px"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <div style="text-align: center;">
                                <asp:Button ID="btn_search1" runat="server" Text="搜尋" CssClass="style59" OnClick="btn_search1_Click" />
                            </div>
                        </asp:Panel>
                        <br />
                        <asp:Panel ID="pl_overtime_gv" runat="server" GroupingText="加班記錄">
                            <asp:CheckBox ID="ckboxall2" runat="server" onclick="javascript:checkallbox2(this);"/>全選
                            <asp:GridView ID="gv_OT" runat="server" Width="105%" DataKeyNames="O_SN,OVERTIME_SN,MZ_ID,COB_OVER_DAY,PAYTIME"
                                AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" EmptyDataText="無資料"
                                OnRowDataBound="gv_OT_RowDataBound" OnPageIndexChanging="gv_OT_PageIndexChanging"
                                Style="text-align: center">
                                <RowStyle BackColor="#EFF3FB" />
                                <Columns>
                                    <asp:TemplateField HeaderText="選取">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="ckb_Select_OT" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="OVER_DAY" HeaderText="加班日期" />
                                    <asp:BoundField DataField="MZ_NAME" HeaderText="加班者" />
                                    <asp:BoundField DataField="REASON" HeaderText="加班/修改事由" HeaderStyle-Width="110px" />
                                    <%--<asp:BoundField DataField="SUMMONTH" HeaderText="本月已核定" />--%>
                                    <asp:BoundField DataField="PAYTIME" HeaderText="加班時間" />
                                    <asp:BoundField DataField="FIRSTTIME" HeaderText="最早刷卡" />
                                    <asp:BoundField DataField="LASTTIME" HeaderText="最晚刷卡" />
                                    <%--<asp:BoundField DataField="C_STATUS" HeaderText="狀態" />--%>
                                    <asp:TemplateField HeaderText="歷程" HeaderStyle-Width="60px" >
                                        <ItemTemplate>
                                            <asp:Button ID="btn_history_ot" runat="server" CommandArgument='<%# Eval("OVERTIME_SN") %>'
                                                Text="歷程" OnCommand="btn_history_ot_Command" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="附件">
                                        <ItemTemplate>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#2461BF" />
                                <AlternatingRowStyle BackColor="White" />
                            </asp:GridView>
                        </asp:Panel>
                        <br />
                        <asp:Panel ID="pl_overtime_history" runat="server" GroupingText="加班簽核歷程">
                            <asp:GridView ID="gv_OT_History" runat="server" AutoGenerateColumns="False"
                                CellPadding="4" ForeColor="#333333" GridLines="None" Style="margin-right: 0px; text-align: center;" Width="100%">
                                <RowStyle BackColor="#EFF3FB" />
                                <Columns>
                                    <asp:BoundField DataField="PROCESS_DATE" HeaderText="簽核時間" />
                                    <asp:BoundField DataField="REVIEW_AD" HeaderText="機關" />
                                    <asp:BoundField DataField="REVIEW_UNIT" HeaderText="單位" />
                                    <asp:BoundField DataField="REVIEW_OCCC" HeaderText="職稱" />
                                    <asp:BoundField DataField="REVIEW_NAME" HeaderText="審核者" />
                                    <asp:BoundField DataField="C_STATUS_NAME" HeaderText="狀態"></asp:BoundField>
                                    <asp:BoundField DataField="REVIEW_MESSAGE" HeaderText="意見"></asp:BoundField>
                                </Columns>
                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#2461BF" />
                                <AlternatingRowStyle BackColor="White" />
                            </asp:GridView>
                        </asp:Panel>
                        <cc1:ModalPopupExtender ID="M_P_E_Opinion" runat="server" BackgroundCssClass="modalBackground"
                            DynamicServicePath="" Enabled="True" PopupControlID="pl_opinion" TargetControlID="btn_opinion_f">
                        </cc1:ModalPopupExtender>
                        <asp:Button ID="btn_opinion_f" runat="server" Style="display: none;" Text="Button" />
                        <asp:Panel ID="pl_opinion" runat="server" BackColor="White" Style="display: none; text-align: center;">
                            <div style="border: solid 1px gray; background-color: #ADD8E6; width: 420px; height: 270px;">
                                <div style="margin: 10px; background-color: #FFFFFF; width: 400px; height: 250px;">
                                    <table class="TableStyleWithTD" width="100%">
                                        <tr>
                                            <th style="text-align: center;">填寫意見
                                            </th>
                                        </tr>
                                        <tr>
                                            <td style="text-align: center;">
                                                <asp:TextBox ID="txt_opinion" runat="server" TextMode="MultiLine" Rows="5" Height="200px"
                                                    Width="372px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: center;">
                                                <asp:Button ID="btn_opinion_yes" runat="server" Text="確定" CommandName="opinion_yes"
                                                    OnClick="btn_opinion_yes_Click"></asp:Button>
                                                <asp:Label ID="opinion_number" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="opinion_p" runat="server" Visible="false"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </asp:Panel>
                        <cc1:ModalPopupExtender ID="M_P_E_overtime_selcet" runat="server" BackgroundCssClass="modalBackground"
                            DynamicServicePath="" Enabled="True" PopupControlID="pl_select" TargetControlID="overtime_select">
                        </cc1:ModalPopupExtender>
                        <asp:Button ID="overtime_select" runat="server" Style="display: none;" Text="Button" />
                        <asp:Panel ID="pl_select" runat="server" BackColor="White" Style="display: none; text-align: center;">
                            <div style="border: solid 1px gray; background-color: #ADD8E6; width: 320px; height: 350px;overflow-y: auto;">
                                <div style="margin: 20px; background-color: #FFFFFF; width: 280px; height: 310px;">
                                    <div style="margin: 0px;">
                                        <h4 style="text-align: center; width: 100%; color: Blue;">陳核</h4>
                                        <asp:GridView ID="gv_overtime_select" runat="server" AutoGenerateColumns="False"
                                            CellPadding="4" DataKeyNames="MZ_ID" ForeColor="#333333" GridLines="None" OnRowDataBound="gv_overtime_select_RowDataBound"
                                            Width="100%">
                                            <RowStyle BackColor="#EFF3FB" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="選取">
                                                    <ItemTemplate>
                                                        <asp:Button ID="btn_overtime_select" runat="server" CommandArgument='<%# Eval("MZ_ID") %>'
                                                            CommandName="btn_overtime_select" OnCommand="btn_overtime_select_Command" Text="選取" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="職稱">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_man" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="姓名">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_mz_name" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="OCC" Visible="False">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_occc" runat="server" Text='<%# Bind("MZ_OCCC") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                            <EditRowStyle BackColor="#2461BF" />
                                            <AlternatingRowStyle BackColor="White" />
                                        </asp:GridView>
                                        <div style="text-align: center;">
                                            <asp:Button ID="btn_overtime_clear" runat="server" Text="取消" CommandName="btn_overtime_clear"
                                                OnClick="btn_overtime_clear_Click" />
                                        </div>
                                        <asp:Label ID="lbl_no_overtime" runat="server" Text="Label"></asp:Label>
                                        <asp:Button ID="Button3" runat="server" Text="確定" OnClick="Button3_Click" />
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                        <cc1:ModalPopupExtender ID="M_P_E_Search" runat="server" BackgroundCssClass="modalBackground"
                            DynamicServicePath="" Enabled="True" PopupControlID="pl_serach_repeat" TargetControlID="Button2">
                        </cc1:ModalPopupExtender>
                        <asp:Button ID="Button2" runat="server" Style="display: none;" Text="Button" />
                        <asp:Panel ID="pl_serach_repeat" runat="server" Style="display: none;">
                            <div style="border: solid 1px gray; background-color: #ADD8E6; width: 450px; height: 350px;">
                                <div style="margin: 20px; background-color: #FFFFFF; width: 410px; height: 330px;">
                                    <div style="margin: 0px;">
                                        <h4 style="text-align: center; width: 100%; color: Blue;">陳核</h4>
                                        <asp:GridView ID="gv_search" runat="server" AutoGenerateColumns="False" Width="100%"
                                            CellPadding="4" ForeColor="#333333" GridLines="None" OnRowDataBound="gv_search_RowDataBound">
                                            <RowStyle BackColor="#EFF3FB" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="選擇">
                                                    <ItemTemplate>
                                                        <asp:Button ID="btn_search" runat="server" CommandArgument='<%# Eval("MZ_ID") %>'
                                                            Text="選擇" OnCommand="btn_search_Command" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="MZ_NAME" HeaderText="姓名" />
                                                <asp:BoundField DataField="MZ_ID" HeaderText="身份證" />
                                                <asp:TemplateField HeaderText="機關">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_search_exad" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="單位">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_search_exunit" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                            <EditRowStyle BackColor="#2461BF" />
                                            <AlternatingRowStyle BackColor="White" />
                                        </asp:GridView>
                                        <div style="width: 100%; text-align: center">
                                            <asp:Button ID="btn_search_go" runat="server" Text="取消" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                        <%--加班簽核E--%>
                        <%--差旅費簽核S--%>
                        <asp:Panel ID="Panel_bussinesstrip_GV" runat="server" GroupingText="差旅費記錄">
                            <asp:GridView ID="GV_bussinesstrip" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                ForeColor="#333333" GridLines="None" DataKeyNames="SN,LEAVE_SCHEDULE_SN,MZ_DLTB01_SN,LEAVE_SN"
                                Width="100%" OnPageIndexChanging="GV_bussinesstrip_PageIndexChanging" OnRowDataBound="GV_bussinesstrip_RowDataBound"
                                Style="text-align: center">
                                <RowStyle BackColor="#EFF3FB" />
                                <Columns>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="CB_select" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="公差時間" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbtn_time" runat="server" OnCommand="lbtn_b_content" CommandArgument='<%# Eval("MZ_DLTB01_SN") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="機關" DataField="MZ_EXAD" />
                                    <asp:BoundField HeaderText="單位" DataField="MZ_EXUNIT" />
                                    <asp:BoundField HeaderText="申請人" ItemStyle-HorizontalAlign="Center" />
                                    <asp:TemplateField HeaderText="歷程" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Button ID="btn_b_history" runat="server" Text="歷程" CommandArgument='<%# Eval("MZ_DLTB01_SN") %>'
                                                OnCommand="btn_b_history_Command" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="意見">
                                        <ItemTemplate>
                                            <asp:Button ID="btn_b_message" runat="server" CommandArgument='<%# Eval("MZ_DLTB01_SN") %>'
                                                Text="填寫" OnCommand="btn_b_message_Command" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#2461BF" />
                                <AlternatingRowStyle BackColor="White" />
                            </asp:GridView>
                        </asp:Panel>
                        <br />
                        <asp:Panel ID="pl_b_history" runat="server" BackColor="White" BorderColor="Black"
                            GroupingText="差旅費簽核歷程">
                            <div>
                                <asp:GridView ID="gv_b_history" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                    ForeColor="#333333" GridLines="None" Width="100%" OnRowDataBound="gv_b_history_RowDataBound"
                                    Style="text-align: center">
                                    <RowStyle BackColor="#EFF3FB" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="簽核時間">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_history_date" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="機關" DataField="MZ_EXAD" />
                                        <asp:BoundField HeaderText="單位" DataField="MZ_EXUNIT" />
                                        <asp:BoundField HeaderText="職稱" DataField="MZ_OCCC" />
                                        <asp:BoundField HeaderText="審核者" DataField="MZ_NAME" />
                                        <asp:BoundField HeaderText="狀態" DataField="C_STATUS_NAME" />
                                        <asp:BoundField DataField="REVIEW_MESSAGE" HeaderText="意見" />
                                    </Columns>
                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <EditRowStyle BackColor="#2461BF" />
                                    <AlternatingRowStyle BackColor="White" />
                                </asp:GridView>
                            </div>
                        </asp:Panel>
                        <cc1:ModalPopupExtender ID="panel_b_message_pop" runat="server" BackgroundCssClass="modalBackground"
                            DynamicServicePath="" Enabled="True" PopupControlID="pl_b_message" TargetControlID="btn_fake3">
                        </cc1:ModalPopupExtender>
                        <asp:Button ID="btn_fake3" runat="server" Text="確定" Style="display: none;" />
                        <asp:Panel ID="pl_b_message" runat="server" BackColor="White" Style="display: none; text-align: center;">
                            <div style="border: solid 1px gray; background-color: #ADD8E6; width: 420px; height: 270px;">
                                <div style="margin: 10px; background-color: #FFFFFF; width: 400px; height: 250px;">
                                    <table class="TableStyleWithTD" width="100%">
                                        <tr>
                                            <th style="text-align: center;">填寫意見
                                            </th>
                                        </tr>
                                        <tr>
                                            <td style="text-align: center;">
                                                <asp:TextBox ID="txt_b_message" runat="server" Height="200px" TextMode="MultiLine"
                                                    Width="372px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: center;">
                                                <asp:Button ID="btn_b_ck" runat="server" Text="確定" OnClick="btn_b_ck_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </asp:Panel>
                        <cc1:ModalPopupExtender ID="Panel_b_select_ModalPopupExtender" runat="server" BackgroundCssClass="modalBackground"
                            DynamicServicePath="" Enabled="True" TargetControlID="btn_popup4" PopupControlID="Panel_b_select">
                        </cc1:ModalPopupExtender>
                        <asp:Button ID="btn_popup4" runat="server" Text="Button" Style="display: none;" />
                        <asp:Panel ID="Panel_b_select" runat="server" Style="display: none; text-align: center;">
                            <div style="border: solid 1px gray; background-color: #ADD8E6; width: 320px; height: 350px;">
                                <div style="margin: 20px; background-color: #FFFFFF; width: 280px; height: 310px;">
                                    <div style="margin: 0px;">
                                        <h4 style="text-align: center; width: 100%;">陳核</h4>
                                        <asp:GridView ID="GV_b_CHECKER" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                            ForeColor="#333333" GridLines="None" AllowPaging="True" OnPageIndexChanging="GV_b_CHECKER_PageIndexChanging"
                                            OnRowCommand="GV_b_CHECKER_RowCommand" PageSize="5">
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
                                            <asp:Button ID="btn_bak1" runat="server" Text="送出" Style="display: none;" />
                                            <asp:Button ID="btn_exit1" runat="server" Text="取消" OnClick="btn_exit1_Click" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                        <%--差旅費簽核E--%>
                        <%--銷假簽核S--%>
                        <asp:Panel ID="pl_change_dltb01" runat="server" GroupingText="銷假記錄">
                            <asp:GridView ID="gv_change_dltb01" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                ForeColor="#333333" GridLines="None" DataKeyNames="SN,LEAVE_SCHEDULE_SN,MZ_DLTB01_SN,LEAVE_SN"
                                Width="100%" OnPageIndexChanging="gv_change_dltb01_PageIndexChanging" OnRowDataBound="gv_change_dltb01_RowDataBound"
                                Style="text-align: center">
                                <RowStyle BackColor="#EFF3FB" />
                                <Columns>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="CB_select" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="銷假時間" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbtn_time" runat="server" OnCommand="lbtn_c_content" CommandArgument='<%# Eval("MZ_DLTB01_SN") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="機關" DataField="MZ_EXAD" />
                                    <asp:BoundField HeaderText="單位" DataField="MZ_EXUNIT" />
                                    <asp:BoundField HeaderText="申請人" ItemStyle-HorizontalAlign="Center" />
                                    <asp:TemplateField HeaderText="歷程" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Button ID="btn_c_history" runat="server" Text="歷程" CommandArgument='<%# Eval("MZ_DLTB01_SN") %>'
                                                OnCommand="btn_c_history_Command" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="意見">
                                        <ItemTemplate>
                                            <asp:Button ID="btn_c_message" runat="server" CommandArgument='<%# Eval("MZ_DLTB01_SN") %>'
                                                Text="填寫" OnCommand="btn_c_message_Command" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#2461BF" />
                                <AlternatingRowStyle BackColor="White" />
                            </asp:GridView>
                        </asp:Panel>
                        <br />
                        <asp:Panel ID="pl_change_dltb01_history" runat="server" BackColor="White" BorderColor="Black"
                            GroupingText="銷假簽核歷程">
                            <div>
                                <asp:GridView ID="gv_change_dltb01_history" runat="server" AutoGenerateColumns="False"
                                    CellPadding="4" ForeColor="#333333" GridLines="None" Width="100%" OnRowDataBound="gv_change_dltb01_history_RowDataBound"
                                    Style="text-align: center">
                                    <RowStyle BackColor="#EFF3FB" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="簽核時間">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_history_date" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="機關" DataField="MZ_EXAD" />
                                        <asp:BoundField HeaderText="單位" DataField="MZ_EXUNIT" />
                                        <asp:BoundField HeaderText="職稱" DataField="MZ_OCCC" />
                                        <asp:BoundField HeaderText="審核者" DataField="MZ_NAME" />
                                        <asp:BoundField HeaderText="狀態" DataField="C_STATUS_NAME" />
                                        <asp:BoundField DataField="REVIEW_MESSAGE" HeaderText="意見" />
                                    </Columns>
                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <EditRowStyle BackColor="#2461BF" />
                                    <AlternatingRowStyle BackColor="White" />
                                </asp:GridView>
                            </div>
                        </asp:Panel>
                        <cc1:ModalPopupExtender ID="btn_popup6_ModalPopupExtender" runat="server" DynamicServicePath=""
                            Enabled="True" TargetControlID="btn_popup6" PopupControlID="pl_c_select">
                        </cc1:ModalPopupExtender>
                        <asp:Button ID="btn_popup6" runat="server" Text="Button" Style="display: none;" />
                        <asp:Panel ID="pl_c_select" runat="server" Style="display: none; text-align: center;">
                            <div style="border: solid 1px gray; background-color: #ADD8E6; width: 320px; height: 350px;">
                                <div style="margin: 20px; background-color: #FFFFFF; width: 280px; height: 310px;">
                                    <div style="margin: 0px;">
                                        <h4 style="text-align: center; width: 100%;">陳核</h4>
                                        <asp:GridView ID="GV_C_CHECKER" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                            ForeColor="#333333" GridLines="None" AllowPaging="True" OnPageIndexChanging="GV_C_CHECKER_PageIndexChanging"
                                            OnRowCommand="GV_C_CHECKER_RowCommand" PageSize="5">
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
                                            <asp:Button ID="Button4" runat="server" Text="送出" Style="display: none;" />
                                            <asp:Button ID="btn_c_exit" runat="server" Text="取消" OnClick="btn_c_exit_Click" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                        <cc1:ModalPopupExtender ID="btn_fake4_ModalPopupExtender" runat="server" DynamicServicePath=""
                            Enabled="True" PopupControlID="pl_c_message" TargetControlID="btn_fake4">
                        </cc1:ModalPopupExtender>
                        <asp:Button ID="btn_fake4" runat="server" Text="確定" Style="display: none;" />
                        <asp:Panel ID="pl_c_message" runat="server" BackColor="White" Style="display: none; text-align: center;">
                            <div style="border: solid 1px gray; background-color: #ADD8E6; width: 420px; height: 270px;">
                                <div style="margin: 10px; background-color: #FFFFFF; width: 400px; height: 250px;">
                                    <table class="TableStyleWithTD" width="100%">
                                        <tr>
                                            <th style="text-align: center;">填寫意見
                                            </th>
                                        </tr>
                                        <tr>
                                            <td style="text-align: center;">
                                                <asp:TextBox ID="txt_c_message" runat="server" Height="200px" TextMode="MultiLine"
                                                    Width="372px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: center;">
                                                <asp:Button ID="btn_c_ck" runat="server" Text="確定" OnClick="btn_c_ck_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </asp:Panel>
                        <%--銷假簽核E--%>
                    </div>
                </div>
                <asp:Panel ID="Panel_Progress" runat="server" BackColor="White" BorderWidth="2px"
                    Style="display: none;" Width="200px" Height="100px">
                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="100">
                        <ProgressTemplate>
                            <div style="position: relative; top: 40%; text-align: center;">
                                <img src="loading.gif" style="vertical-align: middle" alt="Processing" />
                                處理中，請稍等
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </asp:Panel>
                <cc1:ModalPopupExtender ID="Panel_Progress_ModalPopupExtender" runat="server" PopupControlID="Panel_Progress"
                    DynamicServicePath="" Enabled="True" TargetControlID="Panel_Progress">
                </cc1:ModalPopupExtender>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
    
    <script type="text/javascript">
        function checkallbox(tmpcheckall) {
            var GridView1 = document.getElementById("<%=gv_data.ClientID %>");
            var inputs = GridView1.getElementsByTagName("checkbox");
           for(i = 1;i < GridView1.rows.length; i++) 
           {
               var inputs = GridView1.rows[i].getElementsByTagName("INPUT")[0];
               if (tmpcheckall.checked) {
                   inputs.checked = true;
               }
               else {
                   inputs.checked = false;
               }
           }
        }

        function checkallbox2(tmpcheckall) {
            var GridView1 = document.getElementById("<%=gv_OT.ClientID %>");
            var inputs = GridView1.getElementsByTagName("checkbox");
           for(i = 1;i < GridView1.rows.length; i++) 
           {
               var inputs = GridView1.rows[i].getElementsByTagName("INPUT")[0];
               if (tmpcheckall.checked) {
                   inputs.checked = true;
               }
               else {
                   inputs.checked = false;
               }
           }
      }

    </script>
</html>
