<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="C_rpt.aspx.cs" Inherits="TPPDDB._3_forleave.C_rpt" %>

<%@ Register assembly="CrystalDecisions.Web, Version=12.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="lb_tip" runat="server"  Visible ="false"></asp:Label>
        <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" 
            AutoDataBind="true" PrintMode="ActiveX" ToolPanelView="None" />
    
    <CR:CrystalReportViewer ID="CrystalReportViewer2" runat="server" 
        AutoDataBind="true" PrintMode="ActiveX" ToolPanelView="None" />
    
    </div>
    </form>
</body>
</html>
