<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomerDisplayer.aspx.cs" Inherits="DagensIndustri.Tools.Admin.Accommodators.CustomerDisplayer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Dagens industri - Förmedlarsida</title>

    <style type="text/css">
    body
    {
        font-family:Verdana, Arial, Sans-Serif;
        font-size:11px;
        line-height:15px;
        color:Black;
        margin:0px;
    }
    h2
    {
        font-size:15px;
        font-weight:bold;
    }
    .small
    {
        font-family:Verdana, Arial, Sans-Serif;
        font-size:10px;
        color:#999999;        
    }
    </style>

</head>
<body>
    <form id="form1" runat="server">
    <div style="margin-left:20px;">
    
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
        <td><asp:Button ID="ButtonSearch" runat="server" Text="Sök" onclick="ButtonSearch_Click" /></td>
        </tr>
        </table>

        <br />
        <br />
        
        <asp:Label ID="LabelMess" Visible="false" ForeColor="Red" runat="server"></asp:Label>


        <asp:PlaceHolder ID="PlaceHolderCust" Visible="false" runat="server">
            <b>Kund</b><br />
            <asp:Label ID="LabelCustName" runat="server"></asp:Label>
            <br />
            <br />
            <b>Permanent adress</b><br />
            <asp:Label ID="LabelAddress" runat="server"></asp:Label>
            <br />
            <br />

            <b>Tillfällig adress</b><br />
            <asp:Label ID="LabelTempAddress" runat="server"></asp:Label>
            <br />
            <br />

            <b>Prenumerationsstatus</b><br />
            <asp:Label ID="LabelSubs" runat="server"></asp:Label>
            <br />
            <b>Fakturor</b><br />
            <asp:Label ID="LabelInvoices" runat="server"></asp:Label>
        </asp:PlaceHolder>

    </div>
    </form>
</body>
</html>
