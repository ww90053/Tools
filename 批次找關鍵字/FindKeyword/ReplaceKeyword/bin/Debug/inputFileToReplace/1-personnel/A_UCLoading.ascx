<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="A_UCLoading.ascx.cs"
    Inherits="TPPDDB._1_personnel.A_UCLoading" %>

<script type="text/javascript">
         function pageLoad() {
             var ppm = Sys.WebForms.PageRequestManager.getInstance();
             ppm.add_beginRequest(beginRequestHandler);
             //ppm.add_pageLoaded(pageLoaded);
         }

         function beginRequestHandler(sender, args) {
             if (args.get_postBackElement().id === 'ctl00_ContentPlaceHolder1_btPrint') {
                 document.getElementById("ctl00_ContentPlaceHolder1_btCancel").disabled = true;
                 args.get_postBackElement().disabled = true;
             }
         }
</script>


<asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
    <ProgressTemplate>
        <div style="text-align:center;">
            <img alt="" src="../images/ajax-loader.gif" 
                style="width: 220px; height: 19px" /><br />
            <span style="color:Red;font-weight:bold;font-size:x-medium;">
            <b style="font-size: medium">資料量多，產生中，請稍待…</b></span></div>
    </ProgressTemplate>
</asp:UpdateProgress>



