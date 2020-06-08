<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SmsAdmin.aspx.cs" Inherits="DagensIndustri.Tools.Admin.Sms.SmsAdmin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>SmsAdmin</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        Vintips - SMS-hantering<br />
        <br />
        <asp:Button ID="ButtonNew" runat="server" Text="Skapa nytt SMS" />
        <br />
        <br />
        Välj SMS (redigera/skicka)<br />
        <asp:DropDownList ID="DropDownListMess" runat="server" Width="300px"></asp:DropDownList>
        <br />
        <br />
        SMS-text<br />
        <asp:TextBox ID="TextBoxMess" MaxLength="160" Rows="6" runat="server" 
            TextMode="MultiLine" Width="300px"></asp:TextBox>
        <br />
        <br />
        <asp:Button ID="ButtonSave" runat="server" Text="Spara" />&nbsp;&nbsp;
        <asp:Button ID="ButtonSaveAndSend" runat="server" Text="Spara och skicka" />&nbsp;&nbsp;
        <asp:Button ID="ButtonDelete" runat="server" Text="Radera" />
        <br />



    </div>
    </form>
</body>
</html>
