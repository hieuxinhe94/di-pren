<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="DagensIndustri.Tools.Admin.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Dagens industri AB, verktyg</title>
    <link href="/Tools/Admin/Styles/StyleSheetPublic.css" rel="stylesheet" type="text/css" />  
</head>
<body>
    <form id="form1" runat="server">
        <div id="MainBodyArea">
            <strong>Prenumeration och Bilagor - Administrering </strong>
            <ul>        
                <li><a href="/Tools/Admin/Appendix/AppendixAdmin.aspx">Bilageadmin</a></li>
                <li><a href="/Tools/Admin/CustomerInfo/CustomerAdmin.aspx">Kundinfo (DI på nätet)</a></li>
                <li><a href="/Tools/Admin/PayTrans/PaymentAdmin.aspx">Betalningsadmin</a></li>
                <li><a href="/Tools/Admin/CustomerInfo/FindCustomer.aspx">Kundsök</a></li>
               <%-- <li><a href="/Tools/Admin/Subscription/AdminSubscriptionPrices.aspx">Prenumerationsadmin, prisintervall</a></li>--%>
            </ul>
        </div>
    </form>
    
    <form action="http://www.3pr.se/shop/admin/di/" method="post" target="_blank">
        <input type="hidden" name="user" value="dagensindustri">
        <input type="hidden" name="pwd" value="di1985">
        <input type="hidden" name="lang" value="1"/> 
        <input type="hidden" name="login" value="yes">
        <input type="submit" value="Premier">
    </form>

</body>
</html>
