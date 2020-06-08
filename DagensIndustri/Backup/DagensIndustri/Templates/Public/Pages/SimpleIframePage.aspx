<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SimpleIframePage.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.SimpleIframePage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body style="margin:0 auto; background-color:<%= frameBackgroundColor %>;">
    <form id="form1" runat="server">
        <iframe src='<%= frameSource %>' height='<%= frameHeight %>' width='<%= frameWidth %>' frameborder="0" scrolling="no">
    
        </iframe>
    </form>
</body>
</html>
