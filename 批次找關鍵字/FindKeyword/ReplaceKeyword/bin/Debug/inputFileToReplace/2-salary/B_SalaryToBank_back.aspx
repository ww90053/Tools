<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true" CodeBehind="B_SalaryToBank_back.aspx.cs" Inherits="TPPDDB._2_salary.B_SalaryToBank_back" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlExtenders" Namespace="AjaxControlExtenders" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="style/Master.css" rel="stylesheet" type="text/css" />
    
    
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="PageTitle">
        轉帳回饋檔匯入
    </div>
    <div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table class="TableStyleBlue">
                    <tr>
                        <th>
                            <a class="must">*發薪機關</a>
                        </th>
                        <td>
                            <asp:DropDownList ID="DropDownList_AD" runat="server" AppendDataBoundItems="True"
                                DataTextField="MZ_KCHI" DataValueField="MZ_KCODE">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            <a class="must">*發放類別</a>
                        </th>
                        <td>
                            <asp:DropDownList ID="DropDownList_TYPE" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList_TYPE_SelectedIndexChanged">
                                <asp:ListItem Value="-1">請選擇</asp:ListItem>
                                <asp:ListItem Value="MONTH">每月薪資</asp:ListItem>
                                <asp:ListItem Value="REPAIR">補發薪資</asp:ListItem>
                                <asp:ListItem Value="YEAR">年終獎金</asp:ListItem>
                                <asp:ListItem Value="EFFECT">考績獎金</asp:ListItem>
                                <asp:ListItem Value="SOLE">單一發放</asp:ListItem>
                                <asp:ListItem Value="OFFER">優惠存款</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            <a class="must">*存款銀行</a>
                        </th>
                        <td>
                            <asp:DropDownList ID="DropDownList_BANK" runat="server" >
                                <asp:ListItem Value="-1">請選擇</asp:ListItem>
                                <asp:ListItem Value="005">土地銀行</asp:ListItem>
                                <asp:ListItem Value="008">華南銀行</asp:ListItem>
                                <asp:ListItem Value="004">台灣銀行</asp:ListItem>
                                <asp:ListItem Value="700">中華郵政</asp:ListItem>
                                <asp:ListItem Value="951">農會</asp:ListItem>
                                <asp:ListItem Value="119">淡水一信</asp:ListItem>
                                <asp:ListItem Value="013">國泰世華</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <%--<tr>
                        <th>
                            劃撥帳號
                        </th>
                        <td>
                            <asp:TextBox ID="txt_unitacc" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            入帳項目
                        </th>
                        <td>
                            <asp:TextBox ID="txt_item" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            會辦單位
                        </th>
                        <td>
                            <asp:TextBox ID="txt_unit" runat="server" MaxLength="6" Width="100px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr id="memo" runat="server" visible="false">
                        <th>
                            摘要代碼
                        </th>
                        <td>
                            <asp:TextBox ID="TextBox_MEMO" runat="server" Style="text-align: center">150</asp:TextBox>
                        </td>
                    </tr>
                    <tr id="sMEMO" runat="server" visible="false">
                        <th>
                            存摺區分
                        </th>
                        <td>
                            <asp:TextBox ID="TextBox_sMEMO" runat="server" Style="text-align: center" MaxLength="18">TCODE</asp:TextBox>
                        </td>
                    </tr>--%>
                    <tr>
                        <th>
                            <a class="must">*資料日期</a>
                        </th>
                        <td>
                            <asp:TextBox ID="TextBox_DATE" runat="server" MaxLength="7" Style="text-align: center;
                                width: 70px;"></asp:TextBox>
                            <asp:Label ID="lbl_Tips" runat="server" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr id="tr_BatchNumber" runat="server" visible="false">
                        <th>
                            批號
                        </th>
                        <td>
                            <asp:TextBox ID="txt_BatchNumber" runat="server" MaxLength="2" Style="text-align: center"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            <a class="must">*入帳日期</a>
                        </th>
                        <td>
                            <asp:TextBox ID="TextBox_TransDate" runat="server" MaxLength="7" Style="text-align: center;
                                width: 70px;"></asp:TextBox>
                            <asp:Label ID="Label1" runat="server" ForeColor="Red">格式：0990101</asp:Label>
                        </td>
                    </tr>
                    <tr id="tr_name" runat="server" visible="false">
                        <th>
                            姓名
                        </th>
                        <td>
                            <asp:TextBox ID="TextBox_Name" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr id="tr_id" runat="server" visible="false">
                        <th>
                            身分證字號
                        </th>
                        <td>
                            <asp:TextBox ID="TextBox_ID" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <!--//2013/09/12-->
                    <tr id="order_by" runat="server" visible="false">
                        <th>
                            排序
                        </th>
                        <td>
                            <asp:DropDownList ID="ddl_ORDER_BY" runat="server">
                             <asp:ListItem Selected="True"  Value="1">員工編號</asp:ListItem>
                            <asp:ListItem  Value="2">匯入順序</asp:ListItem>
                            </asp:DropDownList>
                            
                            
                        </td>
                    </tr>
                    <!--//2013/09/12-->
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="Label_MSG" runat="server" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table width="100%">
                                <tr>
                                    <td style="text-align: center;">
                                        <asp:FileUpload ID="ful_txt" runat="server" />
                                    </td>
                                    <td style="text-align: center;">
                                        <asp:Button ID="btn_export" runat="server" Text="回饋檔上傳"  
                                            onclick="btn_export_Click" />
                                    </td>
                                </tr>
                                
                            </table>
                        </td>
                    </tr>
                    <tr>
                    <td>
                    
                   
               
                    
                    </td>
                    </tr>
                </table>
                 <asp:UpdateProgress ID="UpdateProgress1" runat="server"   AssociatedUpdatePanelID="UpdatePanel1">
                        <ProgressTemplate>
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/images/ajax-loader.gif" />
                            <span class="style3">資料量多，產生中 請稍待…</span>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                
                
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btn_export"  />
            </Triggers>
        </asp:UpdatePanel>
       <%-- <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>--%>
                
                   <%-- <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                        <ProgressTemplate>
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/images/ajax-loader.gif" />
                        </ProgressTemplate>
                    </asp:UpdateProgress>--%>
                
            <%--</ContentTemplate>
        </asp:UpdatePanel>--%>
    </div>
</asp:Content>

