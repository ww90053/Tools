<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true"
    CodeBehind="00-test.aspx.cs" Inherits="TPPDDB._2_salary._0_test" Culture="auto"
    meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="TPPDDB" Namespace="TPPDDB" TagPrefix="cc2" %>
<%@ Register Assembly="TBGridView" Namespace="TPPDDB" TagPrefix="cc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">

    <script src="../1-personnel/jquery/jquery.js" type="text/javascript"></script>

    <script src="../1-personnel/jquery/IDNO_Vaild.js" type="text/javascript"></script>

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

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="width: 83%; height: 135px; float: left;">
                <asp:Panel ID="Panel1" runat="server">
                    <asp:TextBox ID="TextBox1" runat="server" AutoPostBack="True" OnTextChanged="TextBox1_TextChanged"></asp:TextBox>
                    <asp:TextBox ID="TextBox2" runat="server" AutoPostBack="True" OnTextChanged="TextBox2_TextChanged"></asp:TextBox>
                    <asp:TextBox ID="TextBox3" runat="server" AutoPostBack="True" OnTextChanged="TextBox3_TextChanged"></asp:TextBox>
                    <asp:TextBox ID="TextBox4" runat="server" AutoPostBack="True" OnTextChanged="TextBox4_TextChanged"></asp:TextBox>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
