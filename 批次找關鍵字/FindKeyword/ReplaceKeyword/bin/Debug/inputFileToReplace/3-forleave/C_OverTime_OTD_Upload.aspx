<%@ Page Title="" Language="C#" MasterPageFile="~/TPPD.Master" AutoEventWireup="true" CodeBehind="C_OverTime_OTD_Upload.aspx.cs" Inherits="TPPDDB._3_forleave.C_OverTime_OTD_Upload" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <link href="../styles/Stylesheet.css" rel="stylesheet" type="text/css" />
    <link href="../styles/style09.css" rel="Stylesheet" type="text/css" />
    <style type="text/css">
        .row_header {
            width: 100px;
            font-weight: 700;
            color: #0033CC;
            text-align: justify;
            text-align-last: justify;
            background-color: #99FFCC;
        }

        .ViewHeader {
            color: #000000;
            font-weight: 700;
            font-weight: bold;
            text-align: justify;
            text-align-last: justify;
            width: 80px;
            white-space: nowrap;
        }

        .EditHeader {
            color: #0033CC;
            font-weight: 700;
            background-color: #FFFF66;
            font-weight: bold;
            text-align: justify;
            text-align-last: justify;
            width: 80px;
            white-space: nowrap;
        }

        .GV_Header {
            background-color: #507CD1;
            font-weight: bold;
            color: white;
            border: solid 1px #e4e3e3;
            text-align: center;
        }

        .GV_Hidden {
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="style10 style8">
        值日補休附件上傳
    </div>
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnl_Query" runat="server" Width="370px">
                <table width="100%">
                    <tr>
                        <td class="ViewHeader">機 關：
                        </td>
                        <td style="text-align: left;">
                            <asp:DropDownList runat="server" ID="ddl_Upload_AD" AutoPostBack="true" OnSelectedIndexChanged="ddl_Upload_AD_SelectedIndexChanged"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="ViewHeader">單 位：
                        </td>
                        <td style="text-align: left;">
                            <asp:DropDownList runat="server" ID="ddl_Upload_Unit"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="ViewHeader">年 月：
                        </td>
                        <td style="text-align: left;">
                            <asp:TextBox runat="server" ID="txt_UploadYM" MaxLength="5" Width="60px"></asp:TextBox>(ex:10812)
                        </td>
                    </tr>
                    <tr>
                        <td class="ViewHeader">上傳檔案：
                        </td>
                        <td style="text-align: left;">
                            <asp:FileUpload ID="FileUpload1" runat="server" Width="150px" onclick="alert('附件檔案請勿於本頁面外開啟並請解密後再上傳附件！');" />
                            <br />
                            上傳檔案說明：檔案大小限制為10MB以下，若該單位已上傳過，則新檔案會強制覆蓋原檔案。
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="text-align: center;">
                            <asp:Button runat="server" ID="btn_Upload" Text="上傳" CssClass="style9" OnClick="btn_Upload_Click" OnClientClick="return confirm('覆蓋已上傳檔案？')" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
