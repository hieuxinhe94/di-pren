<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConsumeProduct.aspx.cs" Inherits="WS.ConcurrentUsers.ConsumeProduct" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
      
      <h2>Settings</h2>
      
      <table>
        <tr>
          <td align="left" valign="top">
            <b>Brand:</b><br/>
            Dagens Industri Session Limit <br/>
            Session limit: 3 <br/>
            Auto force logout: true 
            <br/>
            <br/>
      
            <b>Applications</b><br/>
            <asp:DropDownList ID="DropDownListAppIds" runat="server">
              <asp:ListItem Text="dagensindustri2.se" Value="dagensindustri2.se"></asp:ListItem>
              <asp:ListItem Text="di2.se" Value="di2.se"></asp:ListItem>
            </asp:DropDownList>
            <br/>
            <br/>
      
      <%--
            Products<br/>
            <asp:DropDownList ID="DropDownListProducts" runat="server">
              <asp:ListItem Text="DI2SE_WWW (Product Tags: [DI2_SE])" Value="DI2SE_WWW"></asp:ListItem>
              <asp:ListItem Text="Dagens Industri Paper 2 (Product Tags: [DIPAPER, DITABLET] )" Value="Dagens Industri Paper 2 "></asp:ListItem>
            </asp:DropDownList>
            <br/>
            <br/>
      --%>
            <b>Resources</b><br/>
            <asp:DropDownList ID="DropDownListExtResIds" runat="server">
              <asp:ListItem Text="DITABLET2 (Tags: [DITABLET], Session Limit: 1)" Value="DITABLET2"></asp:ListItem>
              <asp:ListItem Text="di2.se (Tags: [DI2_SE], Session Limit: 2)" Value="di2.se"></asp:ListItem>
              <asp:ListItem Text="DITABLET3 (Tags: [DITABLET], Session Limit: unlimited)" Value="DITABLET3"></asp:ListItem>
            </asp:DropDownList>
          </td>
          <td width="30"></td>
          <td align="left" valign="top">
            <b>Users</b><br/>
            limit@di2.se <br/>
            Password: 123456 <br/>
            Entitlements: DI2SE_WWW AND Dagens Industri Paper 2
            <br/>
            <br/>
            limit2@di2.se <br/>
            Password: 123456 <br/>
            Entitlements: DI2SE_WWW
          </td>
        </tr>
      </table>
      <br/>
      <br/>
      
      
      <table>
        <tr>
          <td><asp:Button ID="ButtonConsume" runat="server" Text="Consume product" onclick="ButtonConsume_Click" /></td>
          <td width="10"></td>
          <td><asp:Button ID="ButtonLogout" runat="server" Text="Log out" onclick="ButtonLogout_Click" /></td>
        </tr>
      </table>
      
      <br/>

      <hr/>
      <h2>Status</h2>

      <asp:PlaceHolder ID="PlaceHolderNotLoggedIn" runat="server">
        <ul>
          <li>not logged in</li>
        </ul>
        <br/>
        <asp:Button ID="ButtonLogin" runat="server" Text="Log in" onclick="ButtonLogin_Click" /><br/>
      </asp:PlaceHolder>
      

      <asp:PlaceHolder ID="PlaceHolderNotEntitled" runat="server">
        <ul>
          <li>logged in</li>
          <li>cannot consume product</li>
        </ul>
        <br/>
        <asp:Literal ID="LiteralNotEntitledMess" runat="server"></asp:Literal>
      </asp:PlaceHolder>
      

      <asp:PlaceHolder ID="PlaceHolderEntitled" runat="server">
        <ul>
          <li>logged in</li>
          <li>can to consume product</li>
        </ul>
        <br/>
        Last time consumed: <asp:Label ID="LabelConsumeTime" runat="server"></asp:Label><br/>
      </asp:PlaceHolder>
      
    </div>
    

    </form>
</body>
</html>
