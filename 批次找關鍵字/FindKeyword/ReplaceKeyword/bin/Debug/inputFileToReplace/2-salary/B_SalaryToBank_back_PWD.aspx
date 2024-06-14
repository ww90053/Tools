<%@ Page Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true" CodeBehind="B_SalaryToBank_back_PWD.aspx.cs"
    Inherits="TPPDDB._2_salary.B_SalaryToBank_back_PWD" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="style/Master.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="PageTitle">
        加密檔解密
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table class="TableStyleBlue" style="width: 90%; margin-top: 10px;">
                <tr>
                    <th>
                        金融機構：
                    </th>
                    <td>
                        <asp:DropDownList ID="ddl_bank" runat="server" AutoPostBack="true">
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
                <tr>
                    <th>
                        解密檔:
                    </th>
                    <td>
                       
                        <asp:FileUpload ID="FileUpload_pwd" runat="server" />
       
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: center;">
                        <asp:Button ID="btn_save" runat="server" Text="解密下載" OnClick="btn_save_Click" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
       
         <asp:PostBackTrigger ControlID="btn_save" />
        
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
