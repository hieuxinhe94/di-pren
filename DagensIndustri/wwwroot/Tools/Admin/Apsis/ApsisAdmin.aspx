<%@ Page Language="C#" MaintainScrollPositionOnPostback="true" AutoEventWireup="false" CodeBehind="ApsisAdmin.aspx.cs" Inherits="DagensIndustri.Tools.Admin.Apsis.ApsisAdmin" %>
<%@ Register src="BouncesListView.ascx" tagname="Bounces" tagprefix="uc1" %>
<%@ Register src="Rules.ascx" tagname="Rules" tagprefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Betalningsadmin</title>
    <link href="/Tools/Admin/Styles/StyleSheetPublic.css" rel="stylesheet" type="text/css" />       
</head>
<body>
    <form id="form1" runat="server">       
        <div id="MainBodyArea">
            <h1>Apsisadmin</h1>
            <asp:Button ID="ButtonDispBounces" runat="server" Text="Administrera studsar" onclick="ButtonDispUC_Click" />&nbsp;&nbsp;
            <asp:Button ID="ButtonDispRules" runat="server" Text="Administrera regler" onclick="ButtonDispUC_Click" />
            <br />
            <br />
            <uc1:Bounces ID="Bounces1" runat="server" />
            <uc2:Rules ID="Rules1" runat="server" />    
	    </div>
    </form>
</body>
</html>

