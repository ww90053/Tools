<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="C_UpdateMemo.aspx.cs" Inherits="TPPDDB._3_forleave.C_UpdateMemo" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="../styles/Stylesheet.css" rel="stylesheet" type="text/css" />

    <script language="javascript" type="text/javascript">
        function toLocalDate(source, clientside_arguments) {
            var DateValue = source._textbox.get_Value();
            var year = parseInt(DateValue.substr(0, 4)) - 1911;
            source._textbox.set_Value(year + DateValue.substr(4, 6));
        }
    </script>

    <style type="text/css">
        .style58
        {
            width: 73px;
            font-weight: 700;
            color: #0033CC;
            text-align: left;
            background-color: #99FFCC;
        }
        .style59
        {
            text-align: left;
            width: 72px;
        }
        .style60
        {
            text-align: left;
            width: 351px;
        }
        .style61
        {
            width: 50px;
            font-weight: 700;
            color: #0033CC;
            text-align: left;
            background-color: #99FFCC;
        }
        .style62
        {
            width: 86px;
            font-weight: 700;
            color: #0033CC;
            text-align: left;
            background-color: #99FFCC;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Panel ID="Panel1" runat="server">
                    <table border="1" class="style56">
                        <tr>
                            <td class="style61">
                                備註 :
                            </td>
                            <td class="style60">
                                <asp:TextBox ID="TextBox_Memo" runat="server" MaxLength="50" Width="65px" ></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <%--<table class="style52">
                        <tr>
                            <td>
                                <asp:Button ID="btOK" runat="server" Text="確認" OnClick="btOK_Click" class="style53" />
                                <asp:Button ID="btCancel" runat="server" Text="取消" class="style54" />
                            </td>
                        </tr>
                    </table>--%>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
