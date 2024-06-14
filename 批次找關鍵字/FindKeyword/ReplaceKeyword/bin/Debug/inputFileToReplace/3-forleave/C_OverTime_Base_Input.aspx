<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true" CodeBehind="C_OverTime_Base_Input.aspx.cs" Inherits="TPPDDB._3_forleave.C_OverTime_Base_Input" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="TBGridView" Namespace="TPPDDB" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="../styles/Stylesheet.css" rel="stylesheet" type="text/css" />
    <link href="../styles/style09.css" rel="Stylesheet" type="text/css" />
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
            background-color: #FFFF66;
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
        /*issue_dave 增加顯示每月統計資料 20200817*/
        .FooterStyle {
            background-color: #99FFCC;
            color: White;
            text-align: center;
        }
    </style>

    <script type="text/javascript">
        function ConfirmLastDayOfMonth() {
            //document.getElementById("ctl00_ContentPlaceHolder1_txt_PAY_HOUR").value
            //document.getElementById("ctl00_ContentPlaceHolder1_txt_OVER_HOUR").value

            // PAY_HOUR > OVER_HOUR

            var PAY_HOUR = document.getElementById("ctl00_ContentPlaceHolder1_txt_PAY_HOUR").value;
            var OVER_HOUR = document.getElementById("ctl00_ContentPlaceHolder1_txt_OVER_HOUR").value;
            var OVER_MIN = document.getElementById("ctl00_ContentPlaceHolder1_txt_OVER_MINUTE").value;
            var REMARK = document.getElementById("ctl00_ContentPlaceHolder1_txt_OVER_REMARK").value;
            var STATUS = document.getElementById("ctl00_ContentPlaceHolder1_txt_OVER_STATUS").value;
            if (REMARK != "" && (OVER_HOUR != "0" || OVER_MIN != "0") && STATUS == "決行") {
                if (confirm('你確認要進行加班費時數修改陳核流程？請準備附件電子檔並確認已解密。') == false) {
                    return false;
                }
            }

            // alert(PAY_HOUR);
            // alert(OVER_HOUR);
            var obj1 = document.getElementById("ctl00_ContentPlaceHolder1_rdacc_type_1")
            //  alert('test');
            //   alert(obj1.checked);
            if (obj1.checked == true) {
                //  alert('test');
                //return false;
                document.getElementById("ctl00_ContentPlaceHolder1_LastDay").value = "last";
                return confirm("是否結算目前本月之餘時資料？");
            } else {
                return true;
            }
            //if (parseInt(PAY_HOUR) > parseInt(OVER_HOUR)) {
            //     document.getElementById("ctl00_ContentPlaceHolder1_LastDay").value = "last";
            //     return confirm("當日加班時數不足，是否為本月最後一筆加班資料？");
            // }
            // else {
            // return true;
            // }

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="style10 style8">
        <%= PageTitle %>
    </div>
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel runat="server" ID="pnl_Query">
                <asp:Table runat="server" CssClass="style6" border="1">
                    <asp:TableRow>
                        <asp:TableCell CssClass="ViewHeader">
                            單 位
                        </asp:TableCell>
                        <asp:TableCell CssClass="style3">
                            <asp:Label runat="server" ID="lbl_Unit" Text=""></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell CssClass="EditHeader">
                            姓 名
                        </asp:TableCell>
                        <asp:TableCell CssClass="style3">
                            <asp:DropDownList runat="server" ID="ddl_Query_Name" AutoPostBack="true" OnSelectedIndexChanged="ddl_Query_Name_SelectedIndexChanged"></asp:DropDownList>
                        </asp:TableCell>
                        <asp:TableCell CssClass="ViewHeader">
                            職 別
                        </asp:TableCell>
                        <asp:TableCell CssClass="style3">
                            <asp:Label runat="server" ID="lbl_OCCC" Text="" Width="50"></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell CssClass="ViewHeader">
                            身 分 證
                        </asp:TableCell>
                        <asp:TableCell CssClass="style3">
                            <asp:Label runat="server" ID="lbl_MZ_ID" Text="" Width="50"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell CssClass="EditHeader">
                            加 班 日 期
                        </asp:TableCell>
                        <asp:TableCell CssClass="style3">
                            <asp:TextBox runat="server" ID="txt_OVER_DAY" Width="75px" OnTextChanged="txt_OVER_DAY_TextChanged" AutoPostBack="true"></asp:TextBox>
                        </asp:TableCell>
                        <asp:TableCell CssClass="EditHeader">
                            加 班 事 由
                        </asp:TableCell>
                        <asp:TableCell ColumnSpan="3" CssClass="style3">
                            <asp:TextBox runat="server" ID="txt_REASON" Width="300px" MaxLength="80"></asp:TextBox>
                        </asp:TableCell>
                        <asp:TableCell CssClass="EditHeader">
                            加 班 類 型
                        </asp:TableCell>
                        <asp:TableCell ColumnSpan="2" CssClass="style3">
                            <asp:DropDownList runat="server" ID="ddl_OVERTIME_TYPE" AutoPostBack="true" OnSelectedIndexChanged="ddl_OVERTIME_TYPE_SelectedIndexChanged"></asp:DropDownList>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <asp:Table runat="server" CssClass="style6" ID="Add_Query" border="1">
                    <asp:TableRow>
                        <asp:TableCell CssClass="EditHeader" Width="150px">
                            加 班 時 數
                        </asp:TableCell>
                        <asp:TableCell CssClass="style3" Width="80px">
                            <asp:TextBox runat="server" ID="txt_OVER_HOUR" Width="40px" MaxLength="2" OnTextChanged="txt_IntNumTextChanged"></asp:TextBox>
                            (時)
                        </asp:TableCell>
                        <asp:TableCell CssClass="style3" Width="80px">
                            <asp:TextBox runat="server" ID="txt_OVER_MINUTE" Width="40px" MaxLength="2" OnTextChanged="txt_IntNumTextChanged"></asp:TextBox>
                            (分)
                        </asp:TableCell>
                        <asp:TableCell CssClass="ViewHeader" Width="150px">
                            每 時 金 額
                        </asp:TableCell>
                        <asp:TableCell CssClass="style3" Width="30px">
                            <asp:Label runat="server" ID="lbl_PAY_UNIT" Text=""></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell CssClass="EditHeader">
                            一般/專案 <br />加班
                        </asp:TableCell>
                        <asp:TableCell CssClass="style3" Width="180px">
                            <asp:RadioButtonList ID="rdoptype" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Value="0" Selected="True">一般</asp:ListItem>
                                <asp:ListItem Value="1">專案</asp:ListItem>
                            </asp:RadioButtonList>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell CssClass="EditHeader" Width="180px">
                            申 請 業 務 加 班 費
                        </asp:TableCell>
                        <asp:TableCell CssClass="style3">
                            <asp:TextBox runat="server" ID="txt_PAY_HOUR" Width="40px" MaxLength="2" OnTextChanged="txt_IntNumTextChanged"></asp:TextBox>
                            (時)
                        </asp:TableCell>
                        <asp:TableCell CssClass="style3">
                            <asp:TextBox runat="server" ID="txt_PAY_MIN" Width="40px" MaxLength="2" OnTextChanged="txt_IntNumTextChanged"></asp:TextBox>
                            <asp:TextBox runat="server" ID="txt_OVER_STATUS" Width="40px" MaxLength="2" Style="display: none"></asp:TextBox>
                            (分)
                        </asp:TableCell>
                        <asp:TableCell CssClass="EditHeader" Width="180px" ColumnSpan="2">
                            申 請 輪 值 加 班 費
                        </asp:TableCell>
                        <asp:TableCell CssClass="style3">
                            <asp:TextBox runat="server" ID="txt_SHIFT_HOUR" Width="40px" MaxLength="2" OnTextChanged="txt_IntNumTextChanged"></asp:TextBox>
                            (時)
                        </asp:TableCell>
                        <asp:TableCell CssClass="style3">
                            <asp:TextBox runat="server" ID="txt_SHIFT_MIN" Width="40px" MaxLength="2" OnTextChanged="txt_IntNumTextChanged"></asp:TextBox>
                            (分)
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>

                        <asp:TableCell ColumnSpan="1" CssClass="EditHeader" Width="200px">
                           備註<span style="color: red;font-size:7pt;"><strong>(需敘明原因才能修改加班時數)</strong></span>
                        </asp:TableCell>
                        <asp:TableCell CssClass="style3" ColumnSpan="4">
                            <asp:TextBox runat="server" ID="txt_OVER_REMARK" Width="350px" MaxLength="80"></asp:TextBox>
                            <br />

                        </asp:TableCell>
                        <asp:TableCell ColumnSpan="1" CssClass="EditHeader">
                            設定陳核<br />人員
                        </asp:TableCell>
                        <asp:TableCell ColumnSpan="1" CssClass="style3">
                            <asp:Button runat="server" ID="Button3" Text="修改加班時數" CssClass="style9" OnClick="OnClick_設定人事承辦_變更加班"  />                            
                            <asp:Label runat="server" ID="Label_Reviewer"></asp:Label>
                            <br />
                            <asp:Button runat="server" ID="Button4" Text="值日補休申請" CssClass="style9" OnClick="OnClick_顯示人事室人員_值日補休" Visible="false"  />                            
                            <asp:Label runat="server" ID="Label_Reviewer_OTD"  Visible="false"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>

                    <%--<asp:TableRow>
                        <asp:TableCell ColumnSpan="3" CssClass="EditHeader" Width="180px">
                           加班時數異動人事室審核人員<span style="color: red;"><strong>(需設定人員才能修改加班時數)</strong></span>
                        </asp:TableCell>
                        <asp:TableCell CssClass="style3" ColumnSpan="5">
                        </asp:TableCell>
                    </asp:TableRow>--%>
                    <asp:TableRow Visible="false" ID="esgstr">
                        <asp:TableCell CssClass="EditHeader">
                           退 回 原 因
                        </asp:TableCell>
                        <asp:TableCell CssClass="style3" ColumnSpan="9">
                            <asp:Label runat="server" ID="review_message" Width="630px" Text=""></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:Panel>
            <table class="style12">
                <tr>
                    <td>
                        <asp:HiddenField runat="server" ID="LastDay" />
                        <asp:HiddenField runat="server" ID="HiddenOVERSN" />
                        <asp:Button runat="server" ID="btn_Search" Text="查詢" CssClass="style9" OnClick="btn_Search_Click" />
                        <asp:Button runat="server" ID="btn_Insert" Text="新增" CssClass="style9" OnClick="btn_Insert_Click" />
                        <asp:Button runat="server" ID="btn_Update" OnClientClick="return ConfirmLastDayOfMonth();" Text="修改" CssClass="style9" OnClick="btn_Update_Click" Enabled="false" />
                        <asp:Button runat="server" ID="btn_Delete" Text="刪除" CssClass="style9" OnClick="btn_Delete_Click" Enabled="false" OnClientClick="return confirm(&quot;確定刪除？&quot;)" />
                        <asp:Button runat="server" ID="btn_ReOnline" Text="重新簽核" CssClass="style9" OnClick="btn_ReOnline_Click" Enabled="false" OnClientClick="return confirm(&quot;重新簽核將使資料自單位主管重新開始簽核，你是否確定執行？&quot;)" />
                        <asp:Button runat="server" ID="btn_ReGetTime" Text="更新刷卡時間" CssClass="style9" OnClick="btn_ReGetTime_Click" Enabled="false" />
                        <asp:Button runat="server" ID="btOvertimeAsk" Text="列印加班請示單" CssClass="style13" CausesValidation="False" OnClick="btOvertimeAsk_Click" />
                        <asp:Button runat="server" ID="btOvertimeDetail" Text="加班費管制卡" CssClass="style13" CausesValidation="False" OnClick="btOvertimeDetail_Click" />
                        <asp:Button runat="server" ID="btOvertimeTotal" Text="加班費總表" CssClass="style13" CausesValidation="False" OnClick="btOvertimeTotal_Click" />

                        <%--測試按鈕--%>
                        <br />
                        <%--<asp:Button runat="server" ID="Button4" Text="讀取人事室人員" CssClass="style9" OnClick="OnClick_讀取人事室人員"   />--%>
                        <%--<asp:Button runat="server" ID="Button5" Text="關閉人事室人員" CssClass="style9" OnClick="OnClick_人事室人員_關閉"   />--%>

                    </td>
                </tr>
            </table>
            <asp:Panel runat="server" ID="pnl_GridView" Width="100%" Height="300px" Style="overflow: scroll;">
                <cc1:TBGridView runat="server" ID="gv_resultat" Width="100%" AutoGenerateColumns="False" CellPadding="4" GridLines="None"
                    Style="text-align: left; margin-top: 0px;" ForeColor="#333333"
                    EmptyDataText="無資料" EnableEmptyContentRender="True"
                    DataKeyNames="SN,LOCK_FLAG,OVER_DAY,PAY_UNIT,PAY_CHK,OVERTIME_TYPE,MZ_EXAD,MZ_EXUNIT,MZ_ID,IS_SIGN_RETURN,FORMAT_DAY,OVERTIME_CHG,OVER_STATUS"
                    OnRowCreated="gv_resultat_RowCreated" OnRowDataBound="gv_resultat_RowDataBound" OnRowCommand="gv_resultat_RowCommand"
                    OnSelectedIndexChanging="gv_resultat_SelectedIndexChanging" ShowFooter="True" EnableModelValidation="True">
                    <%--issue_dave 增加顯示每月統計資料 20200817--%>
                    <FooterStyle CssClass="FooterStyle" />
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:CommandField ShowSelectButton="True" ItemStyle-CssClass="GV_Hidden">
                            <ItemStyle CssClass="GV_Hidden" />
                        </asp:CommandField>
                        <asp:BoundField DataField="OVER_TIME" HeaderText="加班日期" ItemStyle-Width="110px">
                            <ItemStyle Width="110px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="REASON" HeaderText="加班事由" ItemStyle-Width="300px">

                            <ItemStyle Width="200px" />
                        </asp:BoundField>

                        <asp:BoundField DataField="OVER_HOUR" HeaderText="加班時" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="OVER_MINUTE" HeaderText="加班分" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="OVER_REMARK" HeaderText="加班備註" />
                        <asp:BoundField DataField="PAY_AND_SHIFT_HOUR" HeaderText="欲申請加班費時數" ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Center" Width="40px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="PAY_AND_SHIFT_MIN" HeaderText="欲申請加班費分數" ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Center" Width="40px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="REST_HOUR" HeaderText="已補休時數" ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Center" Width="40px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="PRIZE_HOUR" HeaderText="已申請敘獎時數" ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Center">

                            <ItemStyle HorizontalAlign="Center" Width="40px" />
                        </asp:BoundField>

                        <%--<asp:BoundField DataField="SURPLUS_HOUR"  HeaderText="累積剩餘時" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="SURPLUS_MINUTE" HeaderText="累積剩餘分" ItemStyle-HorizontalAlign="Center" />--%>

                        <asp:BoundField DataField="LEFT_HOUR" HeaderText="累積剩餘時" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="LEFT_MIN" HeaderText="累積剩餘分" ItemStyle-HorizontalAlign="Center">

                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>

                        <asp:BoundField DataField="OVER_STATUS" HeaderText="狀態" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="OPTYPE" HeaderText="專案加班" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Center" Width="20px" />
                        </asp:BoundField>

                        <asp:TemplateField ItemStyle-CssClass="GV_Hidden">
                            <ItemTemplate>
                                <asp:HiddenField ID="PAY_HOUR" runat="server" Value='<%# Eval("PAY_HOUR") %>' />
                                <asp:HiddenField ID="PAY_MIN" runat="server" Value='<%# Eval("PAY_MIN") %>' />
                                <asp:HiddenField ID="SHIFT_HOUR" runat="server" Value='<%# Eval("SHIFT_HOUR") %>' />
                                <asp:HiddenField ID="SHIFT_MIN" runat="server" Value='<%# Eval("SHIFT_MIN") %>' />
                            </ItemTemplate>
                            <ItemStyle CssClass="GV_Hidden" />
                        </asp:TemplateField>
                    </Columns>
                    <EditRowStyle BackColor="#2461BF" />
                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle CssClass="GV_Header" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="#EFF3FB" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                </cc1:TBGridView>
            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btn_Search_Ok" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btn_Insert" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btn_Update" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="ddl_Query_Name" EventName="SelectedIndexChanged" />
        </Triggers>
    </asp:UpdatePanel>
    <cc1:ModalPopupExtender runat="server" ID="mpe_Search" BackgroundCssClass="PopBackground"
        PopupControlID="pnl_Search" PopupDragHandleControlID="pnl_Search" Enabled="true"
        TargetControlID="btn_fake_Check" CancelControlID="btn_Search_Eixt">
    </cc1:ModalPopupExtender>
    <asp:Button ID="btn_fake_Check" runat="server" Style="display: none;" Text="" />
    <asp:Panel runat="server" ID="pnl_Search" CssClass="DivPanel" Style="display: none;">
        <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Conditional">
            <ContentTemplate>
                <table border="1" width="400px">
                    <tr>
                        <td class="row_header">
                            <font color="red">＊</font>機 關：
                        </td>
                        <td style="text-align: left;">
                            <asp:DropDownList runat="server" ID="ddl_Search_AD" AutoPostBack="true" OnSelectedIndexChanged="ddl_Search_AD_SelectedIndexChanged"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="row_header">
                            <font color="red">＊</font>單 位：
                        </td>
                        <td style="text-align: left;">
                            <asp:DropDownList runat="server" ID="ddl_Search_Unit"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="row_header">
                            <font color="red">＊</font>年 月：
                        </td>
                        <td style="text-align: left;">
                            <asp:TextBox runat="server" ID="txt_Search_YM" MaxLength="5" Width="65px"></asp:TextBox>
                            (範例:10801)
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ddl_Search_AD" EventName="SelectedIndexChanged" />
            </Triggers>
        </asp:UpdatePanel>
        <table border="1" width="400px">
            <tr>
                <td colspan="2" style="text-align: center;">
                    <asp:Button runat="server" ID="btn_Search_Ok" Text="確定" CssClass="style9" OnClick="btn_Search_Ok_Click" />
                    &nbsp;&nbsp;&nbsp;
                    <asp:Button runat="server" ID="btn_Search_Eixt" Text="離開" CssClass="style9" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <%--陳核區塊-控制--%>
    <cc1:ModalPopupExtender runat="server" ID="mpe_OnlienPeople" BackgroundCssClass="PopBackground"
        PopupControlID="pnl_OnlinePeople" PopupDragHandleControlID="pnl_OnlinePeople" Enabled="true"
        TargetControlID="btn_fake_Check2">
    </cc1:ModalPopupExtender>
    <asp:Button ID="btn_fake_Check2" runat="server" Style="display: none;" Text="" />
    <%--陳核區塊-陳核人員--%>
    <asp:Panel runat="server" ID="pnl_OnlinePeople" CssClass="DivPanel" Style="width: 280px; display: none;">
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
                <div style="margin: 10px;">
                    <h3>陳核</h3>
                    <cc1:TBGridView runat="server" ID="gv_online_people" Width="100%" AutoGenerateColumns="false" CellPadding="4" GridLines="None"
                        Style="text-align: left; margin-top: 0px;" ForeColor="#333333"
                        EmptyDataText="無資料" EnableEmptyContentRender="true" DataKeyNames="MZ_ID"
                        OnRowCommand="gv_online_people_RowCommand" OnRowCreated="gv_online_people_RowCreated">
                        <RowStyle BackColor="#EFF3FB" />
                        <Columns>
                            <asp:CommandField ShowSelectButton="false" ItemStyle-CssClass="GV_Hidden" />
                            <asp:BoundField DataField="MZ_OCCC_NM" HeaderText="職稱" ItemStyle-Width="100px" />
                            <asp:BoundField DataField="MZ_NAME" HeaderText="姓名" ItemStyle-Width="100px" />
                            <asp:TemplateField ItemStyle-CssClass="GV_Hidden">
                                <ItemTemplate>
                                    <asp:HiddenField runat="server" ID="hfd_REVIEW_ID" Value='<%# Eval("MZ_ID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <EditRowStyle BackColor="#2461BF" />
                        <AlternatingRowStyle BackColor="White" />
                    </cc1:TBGridView>
                    <asp:HiddenField runat="server" ID="hfd_MZ_ID" />
                    <asp:HiddenField runat="server" ID="hfd_OVERDAY" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div style="text-align: center;">
            <asp:Button runat="server" ID="btn_Online_OK" Text="確定" CssClass="style9" OnClick="btn_Online_OK_Click" />
            <asp:Button runat="server" ID="btn_Online_Exit" Text="取消" CssClass="style9" OnClick="btn_Online_Exit_Click1" />
        </div>
    </asp:Panel>
    <%--陳核區塊-人事室承辦人-加班時數異動--%>
    <cc1:ModalPopupExtender runat="server" ID="mpe_OnlienPeople_Level5" BackgroundCssClass="PopBackground"
        PopupControlID="pnl_OnlinePeople_Level5" PopupDragHandleControlID="pnl_OnlinePeople_Level5" Enabled="true"
        TargetControlID="btn_fake_Check4">
    </cc1:ModalPopupExtender>
    <asp:Button ID="btn_fake_Check4" runat="server" Style="display: none;" Text="" />
    <asp:Panel runat="server" ID="pnl_OnlinePeople_Level5" CssClass="DivPanel" Style="width: 280px; display: none;">
        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
            <ContentTemplate>
                <div style="margin: 10px;">
                    <h3>設定陳核人員</h3>
                    <cc1:TBGridView runat="server" ID="gv_online_people_Level5" Width="100%" AutoGenerateColumns="false" CellPadding="4" GridLines="None"
                        Style="text-align: left; margin-top: 0px;" ForeColor="#333333"
                        EmptyDataText="無資料" EnableEmptyContentRender="true" DataKeyNames="MZ_ID,MZ_NAME"
                        OnRowCommand="gv_online_people_RowCommand"
                        OnRowCreated="gv_online_people_RowCreated">
                        <RowStyle BackColor="#EFF3FB" />
                        <Columns>
                            <asp:CommandField ShowSelectButton="false" ItemStyle-CssClass="GV_Hidden" />
                            <asp:BoundField DataField="MZ_OCCC_NM" HeaderText="職稱" ItemStyle-Width="100px" />
                            <asp:BoundField DataField="MZ_NAME" HeaderText="姓名" ItemStyle-Width="100px" />
                            <asp:TemplateField ItemStyle-CssClass="GV_Hidden">
                                <ItemTemplate>
                                    <asp:HiddenField runat="server" ID="hfd_REVIEW_ID" Value='<%# Eval("MZ_ID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <EditRowStyle BackColor="#2461BF" />
                        <AlternatingRowStyle BackColor="White" />
                    </cc1:TBGridView>

                    <%--當前的人事承辦人名單,是哪一種等級的?4 or 5--%>
                    <asp:HiddenField runat="server" ID="HiddenField_REVIEW_LEVEL" />
                    <%--當前選擇的預設人事室承辦人,這邊暫存其ID,以便如果清單刷新了,可以把人綁定回去--%>
                    <%--LEVEL_4值日補休的審核承辦人--%>
                    <asp:HiddenField runat="server" ID="HiddenField_REVIEW_ID_LEVEL_4" />
                    <%--LEVEL_5加班時數變更的審核承辦人--%>
                    <asp:HiddenField runat="server" ID="HiddenField_REVIEW_ID_LEVEL_5" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div style="text-align: center;">
            <asp:Button runat="server" ID="Button1" Text="確定" CssClass="style9" OnClick="OnClick_人事室人員_確定" />
            <asp:Button runat="server" ID="Button2" Text="取消" CssClass="style9" OnClick="OnClick_人事室人員_關閉" />
        </div>
    </asp:Panel>

    <cc1:ModalPopupExtender runat="server" ID="mpe_OnlienUp" BackgroundCssClass="PopBackground"
        PopupControlID="pnl_OnlineUp" PopupDragHandleControlID="pnl_OnlineUp" Enabled="true"
        TargetControlID="btn_fake_Check3" CancelControlID="btn_OnlineUp_Exit">
    </cc1:ModalPopupExtender>
    <asp:Button ID="btn_fake_Check3" runat="server" Style="display: none;" Text="" />
    <asp:Panel runat="server" ID="pnl_OnlineUp" CssClass="DivPanel" Style="width: 280px; display: none;">
        <table border="1" width="100%">
            <tr>
                <td class="row_header">上傳附件1
                </td>
                <td style="text-align: left;">
                    <asp:FileUpload ID="FileUpload1" runat="server" Width="150px" onclick="alert('附件檔案請勿於本頁面外開啟並請解密後再上傳附件！');" />
                </td>
            </tr>
            <tr>
                <td class="row_header">上傳附件2
                </td>
                <td style="text-align: left;">
                    <asp:FileUpload ID="FileUpload2" runat="server" Width="150px" onclick="alert('附件檔案請勿於本頁面外開啟並請解密後再上傳附件！');" />
                </td>
            </tr>
            <tr>
                <td class="row_header">上傳附件3
                </td>
                <td style="text-align: left;">
                    <asp:FileUpload ID="FileUpload3" runat="server" Width="150px" onclick="alert('附件檔案請勿於本頁面外開啟並請解密後再上傳附件！');" />
                </td>
            </tr>
        </table>
        <div style="text-align: center;">
            <asp:Button runat="server" ID="btn_OnlineUp_OK" Text="上傳" CssClass="style9" OnClick="btn_OnlineUp_OK_Click" />
            <asp:Button runat="server" ID="btn_OnlineUp_Exit" Text="取消" CssClass="style9" />
        </div>
    </asp:Panel>
</asp:Content>
