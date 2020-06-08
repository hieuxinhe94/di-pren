<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustNameCounter.aspx.cs" Inherits="DagensIndustri.Tools.Admin.CustNameCounter.CustNameCounter" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <style>
        body { background-color:#ffe1d8; font-family:Arial, Verdana, Sans-Serif; }
        .h1 { font-size:16px; line-height:20px; }
        .p { font-size:11px; line-height:15px; }
    </style>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        
        <h1>Kund-namns-räknare</h1>

        Sökningen visar antalet kunder som har angivet kundnamn i Cirix ROWTEXT1-fält.
        <br />
        <br />

        Ange kundnamn<br />
        <table border="0" cellpadding="0" cellspacing="0">
            <tr>
            <td><asp:TextBox ID="TextBoxName" Width="200" runat="server"></asp:TextBox></td>
            <td width="10"></td>
            <td><asp:Button ID="ButtonDoSearch" runat="server" Text="Sök" onclick="ButtonDoSearch_Click" /></td>
            </tr>
        </table>
        
        <div style="margin-top:5px;">
            <b>xxx xxx</b> - söker exakt på sökbegreppet<br />
            <b>xxx xxx%</b> - söker på sökbegreppet och vad-som-helst efter<br />
            <b>%xxx xxx%</b> - söker på vad-som-helst före och efter sökbegreppet<br />
        </div>

        <br />
        <br />

        <asp:PlaceHolder ID="PlaceHolderSearchRes" runat="server">
            <table border="0" cellpadding="0" cellspacing="0">
                <tr>
                <td></td>
                <td width="10"></td>
                <td></td>
                </tr>
                <asp:Literal ID="LiteralList" runat="server"></asp:Literal>
            </table>
        </asp:PlaceHolder>


    </div>
    </form>
</body>

</html>
