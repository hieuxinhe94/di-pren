<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UpdateDatabase.aspx.cs" Inherits="DagensIndustri.Tools.Admin.Wine.UpdateDatabase" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

        <title>Administration av viner</title>

    <style type="text/css">
        .AdminTable td {white-space:nowrap; height:20px;vertical-align:top;padding:10px;}
        .AdminTable th {text-align:left;padding:2px 10px 2px 10px;}
        .AdminTable tr.odd {background-color: rgb(249, 248, 248);}
        .AdminTable .sem{display:block;}
        .AdminTable .header{font-weight:bold;background-color:#BCBCBC;}
                
        .editactivities {background-color:#FFF;}
        .editactivities td{padding:5px;vertical-align:middle;}
        
        .searchheading{border:1px solid #BCBCBC;font-weight:bold;margin:10px 0 10px 0;padding:10px;width:300px;} 
        #advancedinfo{padding-top:10px;display:none;}
        
        .error { color:#FF0000; }
    </style>  
    <link href="/Tools/Admin/Styles/StyleSheetPublic.css" rel="stylesheet" type="text/css" />    
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="lbMessage" runat="server" Font-Bold="true" />
    </div>
    <div>
        
        <h1>Uppdatera datasen från Systembolagets API</h1>
        URI: <asp:TextBox ID="tbUri" runat="server" />
        <br />
        <asp:Button ID="btnUpdate" runat="server" Text="Uppdatera" OnClick="btnUpdate_Click" />
    </div>
    </form>
</body>
</html>
