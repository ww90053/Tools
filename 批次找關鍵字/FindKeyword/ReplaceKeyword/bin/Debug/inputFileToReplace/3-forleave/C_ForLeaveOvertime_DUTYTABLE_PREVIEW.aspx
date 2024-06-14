<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="C_ForLeaveOvertime_DUTYTABLE_PREVIEW.aspx.cs" Inherits="TPPDDB._3_forleave.C_ForLeaveOvertime_DUTYTABLE_PREVIEW" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>未命名頁面</title>
    <style type="text/css">
        .style1
        {
            width: 541px;
        }
        .no
        {
        	display:none;
        }
    </style>
    
</head>
<body>
    <form id="form1" runat="server">    
    <div>
        <asp:Button ID="Button1" runat="server" Text="匯出至Excel" 
            onclick="Button1_Click" /></div>
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table style="width:100%; text-align: left; background-color:#6699ff; color:White; font-size: large; font-family:標楷體">
                   <tr>
                     <td class="style1">
                        <asp:Label ID="Label1" runat="server"></asp:Label>
                     </td>
                
                     <td style="text-align: right">
                        <asp:Label ID="Label2" runat="server"></asp:Label>
                     </td>
                   </tr>
               </table>
                <asp:GridView ID="GridView1" runat="server" Width="100%" CellPadding="4" onrowcreated="GridView1_RowCreated" 
                    onrowdatabound="GridView1_RowDataBound" ForeColor="#333333">
                    <RowStyle BackColor="#EFF3FB" />
                    <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#2461BF" />
                    <AlternatingRowStyle BackColor="White" />
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <p></p>
    <div>
        <asp:GridView ID="GridView3" runat="server" CellPadding="4" ForeColor="#333333" 
            GridLines="None" Width="100%">
            <RowStyle BackColor="#EFF3FB" />
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" CssClass="no" />
            <EditRowStyle BackColor="#2461BF" />
            <AlternatingRowStyle BackColor="White" />
            
        </asp:GridView>
    </div>
    <p></p>
    <div>
        <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" 
            CellPadding="4" DataKeyNames="MZ_CNO" ForeColor="#333333" GridLines="None" 
            onrowcommand="GridView2_RowCommand">
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <Columns>
                <asp:CommandField ButtonType="Button" ShowSelectButton="True" />
                <asp:BoundField DataField="MZ_CNO" HeaderText="番號" />
                <asp:BoundField DataField="MZ_NAME" HeaderText="姓名" />
                <asp:BoundField DataField="BECAUSE" HeaderText="原因" />
            </Columns>
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#999999" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        </asp:GridView>
    </div>
    </form>
</body>
</html>

