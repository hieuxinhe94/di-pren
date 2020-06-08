<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomerDisplayer.aspx.cs" Inherits="DagensIndustri.Tools.Admin.Accommodators.CustomerDisplayer" %>
<%@ Register Tagprefix="di" Tagname="CustDetailView" Src="CustDetailView.ascx" %>
<%@ Register TagPrefix="di" TagName="AddrPerm" Src="~/Templates/Public/Units/Placeable/UserSettings/AddressPermUC.ascx" %>
<%@ Register TagPrefix="di" TagName="AddrTemp" Src="~/Templates/Public/Units/Placeable/UserSettings/AddressTempUC.ascx" %>
<%@ Register TagPrefix="di" TagName="SubsSleep" Src="~/Templates/Public/Units/Placeable/UserSettings/SubsSleepsUC.ascx" %>
<%@ Register TagPrefix="di" TagName="Complaint" Src="~/Templates/Public/Units/Placeable/UserSettings/ComplaintUC.ascx" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Dagens industri - Förmedlarsida</title>
    
    <link media="all" rel="stylesheet" type="text/css" href="/Templates/Public/Styles/reset.css" />
    <link media="all" rel="stylesheet" type="text/css" href="/Templates/Public/Styles/shared.css" />
    <link media="all" rel="stylesheet" type="text/css" href="/Templates/Public/Styles/bizbook.css" />
    <link media="all" rel="stylesheet" type="text/css" href="/Templates/Public/Styles/jqueryui/jquery-ui-1.8.9.custom.css" />
    <link media="all" rel="stylesheet" type="text/css" href="/Templates/Public/Styles/dagensindustri.css" />	
    <link media="all" rel="stylesheet" type="text/css" href="/Templates/Public/Styles/mods.css" />

    <script type="text/javascript" src="/Templates/Public/js/DI.js"></script>
    <script type="text/javascript" src="/Templates/Public/js/Functions.js"></script>
    <script type="text/javascript" src="/Templates/Public/js/jquery-1.5.1.min.js"></script>
    <script type="text/javascript" src="/Templates/Public/js/cufon-yui.js"></script>
    <script type="text/javascript" src="/Templates/Public/js/BentonCompBlack_900.font.js"></script>
    <script type="text/javascript" src="/Templates/Public/js/jquery-ui-1.8.9.custom.min.js"></script>
    <script type="text/javascript" src="/Templates/Public/js/jquery.QB.DiSeRSS.js"></script>	
    <script type="text/javascript" src="/Templates/Public/js/jquery.QB.SlideShow.js"></script>
    <script type="text/javascript" src="/Templates/Public/js/jquery.tools.min.js"></script>
    <script type="text/javascript" src="/Templates/Public/js/jquery.ui.datepicker-sv.js"></script>
    <script type="text/javascript" src="/Templates/Public/js/master.js"></script>
    <script type="text/javascript" src="/Templates/Public/js/swfobject.js"></script>
    <script type="text/javascript" src="/Templates/Public/js/jquery.QB.Bookshelf.js"></script>

    <script type="text/javascript" src="/Templates/Public/js/underscore-min.js"></script>
    <script type="text/javascript" src="/Templates/Public/js/jquery.jqEasyCharCounter.min.js"></script>

    
    <style type="text/css">
    .margBtm
    {
        margin-bottom: 20px;
    }
    </style>

</head>
<body id="Body" runat="server">
    <form id="form1" novalidate="novalidate" runat="server">
    <div id="wrapper">
    <div id="content-wrapper">
    <div id="content">
    
        <table border="0" cellpadding="0" cellspacing="0">
        <tr>
        <td><a href="CustomerDisplayer.aspx"><img src="/templates/public/images/logo.png" alt="" border="0"></a></td>
        <td width="40"></td>
        <td><asp:Button ID="ButtonLogout" runat="server" Text="Logga ut" onclick="ButtonLogout_Click"></asp:Button></td>
        </tr>
        </table>

        <br />
        <h2>Förmedlarsida</h2>
        <br />

        <table border="0" cellpadding="0" cellspacing="0">
        <tr>
        <td>Kundnummer</td>
        <td width="10"></td>
        <td><asp:TextBox ID="TextBoxCusno" runat="server" Width="60px"></asp:TextBox></td>
        <td width="10"></td>
        <td><%--<asp:Button ID="ButtonSearch" runat="server" Text="Sök" onclick="ButtonSearch_Click" />--%></td>
        </tr>
        </table>
        
        <br/>
        <asp:LinkButton ID="LinkButtonOverview" runat="server" OnClick="LinkButtonOverview_Click">Översikt</asp:LinkButton>&nbsp;&nbsp;
        <asp:LinkButton ID="LinkButtonPermAddr" runat="server" OnClick="LinkButtonPermAddr_Click">Permanent adress</asp:LinkButton>&nbsp;&nbsp;
        <asp:LinkButton ID="LinkButtonTempAddr" runat="server" OnClick="LinkButtonTempAddr_Click">Tillfällig adress</asp:LinkButton>&nbsp;&nbsp;
        <asp:LinkButton ID="LinkButtonSubsSleep" runat="server" OnClick="LinkButtonSubsSleep_Click">Uppehåll</asp:LinkButton>&nbsp;&nbsp;
        <asp:LinkButton ID="LinkButtonComplaint" runat="server" OnClick="LinkButtonComplaint_Click">Reklamation</asp:LinkButton>&nbsp;&nbsp;
        <br />
        <br />
        <hr style="border-bottom: 1px solid gray;"/>
        <h3><asp:Literal ID="LiteralHeadline" runat="server"></asp:Literal></h3>
        <br />
        
        <asp:DropDownList ID="DDLSubs" Visible="False" AutoPostBack="True" runat="server" OnSelectedIndexChanged="DDLSubs_SelectedIndexChanged" CssClass="margBtm"></asp:DropDownList>

        <asp:Label ID="LabelMess" Visible="false" ForeColor="Red" runat="server"></asp:Label>
      
        <di:CustDetailView ID="CustDetailView1" runat="server" Visible="False" />
        <di:AddrPerm ID="AddrPerm1" runat="server" Visible="False" />
        <di:AddrTemp ID="AddrTemp1" runat="server" Visible="False" />
        <di:SubsSleep ID="SubsSleep1" runat="server" Visible="False" />
        <di:Complaint ID="Complaint1" runat="server" Visible="False" />
        
    </div>
    </div>
    </div>
    </form>
</body>
</html>
