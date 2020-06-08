<%@ Page Title="" Language="C#" MasterPageFile="~/DiIpad/Master.Master" AutoEventWireup="true" CodeBehind="RssSmall.aspx.cs" Inherits="WS.DiIpad.RssSmall" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderHead" runat="server">
    <meta http-equiv="refresh" content="120"/>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript">
        var xmlDoc = GetXmlDoc();
        document.write(GetLinkListAsHtml(xmlDoc, false));
    </script>

</asp:Content>
