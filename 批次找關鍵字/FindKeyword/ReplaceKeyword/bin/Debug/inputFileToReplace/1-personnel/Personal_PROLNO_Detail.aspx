<%@ Page Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true" CodeBehind="Personal_PROLNO_Detail.aspx.cs" Inherits="TPPDDB._1_personnel.Personal_PROLNO_Detail" Title="未命名頁面" %>

<%@ Register Assembly="TBGridView" Namespace="TPPDDB" TagPrefix="cc1" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
   
    <table style="background-color:#6699FF; color:White; width:100%;  font-family:標楷體;  font-size:large;">
           <tr>
           <td  style=" text-align:left;">
              獎懲依據說明 
           </td>
           <td style="text-align: right">
               <asp:Label ID="Label1" runat="server" CssClass="style38"></asp:Label>
           </td>
           </tr>
         </table>

        
        <table border="1" style=" width:100%; text-align:left;">
        <tr>
        <td>
        
                <asp:FileUpload ID="FileUpload1" runat="server" />
        
                <asp:Button ID="Button_Update_Html" runat="server" 
                    onclick="Button_Update_Html_Click" Text="上傳" />
        
                (說明：上傳檔案格式為.htm或.html檔案。)</td>
        
        </tr>
        
        
        </table>
        

        
            
            
        
        
     
        
    

</asp:Content>

