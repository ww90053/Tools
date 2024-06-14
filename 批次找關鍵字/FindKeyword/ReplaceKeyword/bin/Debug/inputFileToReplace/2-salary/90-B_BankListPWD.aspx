<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true" CodeBehind="90-B_BankListPWD.aspx.cs" Inherits="TPPDDB._2_salary._0_B_BankListPWD" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">

<link href="style/Master.css" rel="stylesheet" type="text/css" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="PageTitle">
        <asp:Label ID="lb_title" runat="server" Text="Label"></asp:Label>
    </div>
     <asp:UpdatePanel ID="UpdatePanel1" runat="server">
     <ContentTemplate>
    <table class="TableStyleBlue" style="width: 90%; margin-top: 10px;">
        <tr>
            <th>
                金融機構：
            </th>
            <td>
               
              
                <asp:DropDownList ID="ddl_bank" runat="server"  AutoPostBack="true" OnSelectedIndexChanged="ddl_bank_SelectedIndexChanged">
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
      
      <tr >
            <th>
               <asp:Label ID="lb_old_data" runat="server" Text="Label"></asp:Label>  
            </th>
         
            <td>
               
                <asp:TextBox ID="txt_OldPwd" runat="server" Width ="200px"  MaxLength="24" Enabled="false"></asp:TextBox>
                
            </td>
        </tr>
        <tr>
            <th>
               <asp:Label ID="lb_new_data" runat="server" Text="Label"></asp:Label> 
            </th>
            <td>
               
                <asp:TextBox ID="txt_Pwd" runat="server" Width ="200px"  MaxLength="24"></asp:TextBox>
                
            </td>
        </tr>
         <tr id="tr_chkPWD" runat="server">
            <th>
                確認密碼：
            </th>
            <td>
               
                <asp:TextBox ID="txt_Pwd_check" runat="server" Width ="200px"  MaxLength="24"></asp:TextBox>
                
            </td>
        </tr>
        <tr>
            <td colspan="2" style="text-align: center;">
                <asp:Button ID="btn_save" runat="server" Text="儲存" onclick="btn_save_Click" />
            </td>
        </tr>
    </table>
     </ContentTemplate>
      </asp:UpdatePanel>
</asp:Content>