<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="tmp.aspx.cs" Inherits="WS.tmp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:placeholder ID="PlaceholderTestGui" Visible="false" runat="server">
          
          IssueDate<br/>
          <asp:TextBox ID="TextBoxDate" runat="server"></asp:TextBox>
          <br/>
          <br/>
          ServicePlusUserId<br/>
          <asp:TextBox ID="TextBoxServicePlusUserId" Text="apa" runat="server"></asp:TextBox>
          <br/>
          <br/>
          IP-number<br/>
          <asp:TextBox ID="TextBoxIpNumber" runat="server"></asp:TextBox>
          <br/>
          <br/>
          SiteProvidedDownload<br/>
          <asp:TextBox ID="TextBoxSiteProvidedDownload" Text="ws.dagensindustri.se" runat="server"></asp:TextBox>
          <br/>
          <br/>
          <asp:Button ID="ButtonDownloadFile" runat="server" Text="DownloadFile" onclick="ButtonDownloadFile_Click" />

            <!--
            GetPlusCustomer (by cusno)<br />
            <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
            <br />
            <br />
            <asp:Button ID="Button1" runat="server" Text="Button" onclick="Button1_Click" />
            <br />
            <br />
            <br />
            GetUpdatedCusnosInDateInterval()<br />
            <asp:TextBox ID="TextBoxDateMin" runat="server"></asp:TextBox><br />
            <asp:TextBox ID="TextBoxDateMax" runat="server"></asp:TextBox><br />
            <asp:Button ID="Button2" runat="server" Text="Button" onclick="Button2_Click" />
            <hr />
            <br />
            GetSingleSignOnCodeByCusno()<br />
            <asp:TextBox ID="TextBoxCusno" runat="server"></asp:TextBox><br />
            Kod: <asp:Label ID="LabelCode" runat="server"></asp:Label><br />
            <asp:Button ID="Button3" runat="server" Text="Button" onclick="Button3_Click" />
            <br />
            <br />
            -->
        </asp:placeholder>
    </div>
    </form>
</body>
</html>
