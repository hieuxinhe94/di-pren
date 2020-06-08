<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VerifyStudent.aspx.cs" Inherits="DagensIndustri.Tools.Admin.Student.VerifyStudent" %>
<%@ Register src="StudentVerifierBox.ascx" tagname="StudentVerifierBox" tagprefix="uc1" %>

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
        <h1>Di - studentverifiering</h1>

        <p>
            Ange personnummer för att verifiera att personen är heltidsstudent
        </p>

        <uc1:StudentVerifierBox ID="StudentVerifierBox1" runat="server" />
    
    </div>
    </form>
</body>
</html>
