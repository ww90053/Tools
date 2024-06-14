<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TEST.aspx.cs" Inherits="TPPDDB._2_salary.TEST" %>
<%@ Register assembly="CrystalDecisions.Web, Version=12.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="lb_tip" runat="server" Visible="false"></asp:Label>
            <cr:crystalreportviewer id="CrystalReportViewer1" runat="server" autodatabind="true"
                toolpanelview="None" printmode="ActiveX" />
        </div>
    </form>
</body>
</html>
