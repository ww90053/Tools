<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="On_rpt.aspx.cs" Inherits="TPPDDB._18_Online_Leave.On_rpt" %>

<%@ Register assembly="CrystalDecisions.Web, Version=12.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script type="text/javascript">
        function go_print(tmp_url) {
            window.open(tmp_url);
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true"
            PrintMode="ActiveX" ToolPanelView="None" />
    </form>
</body>
</html>
