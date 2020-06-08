<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConferenceMail.aspx.cs" Inherits="DagensIndustri.Tools.Admin.Conference.ConferenceMail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Mail till konferensdeltagare</title>
    <link href="/Tools/Admin/Styles/StyleSheetPublic.css" rel="stylesheet" type="text/css" />  
    <script src="/Tools/Admin/Styles/Scripts/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script src="/Tools/Admin/Styles/Scripts/jquery-ui-1.8.2.custom.min.js" type="text/javascript"></script>
    
    
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
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div id="MainBodyArea">

            <h1>Mail till konferensdeltagare</h1>  
            <asp:DropDownList runat="server" ID="ddlConferences" DataTextField="name" DataValueField="epipageid" 
                DataSource='<%#DIClassLib.DbHandlers.MsSqlHandler.GetAllConferences()%>'
                AppendDataBoundItems="true"
                OnSelectedIndexChanged="ddlConferences_SelectedIndexChanged"
                AutoPostBack="true">
                <asp:ListItem Text="Välj konferens" Value="0" />
            </asp:DropDownList>
            
            <hr />
            
            <asp:PlaceHolder ID="phConferenceMail" runat="server" Visible='<%#SelectedConference != null %>'>

                <h2><%#ddlConferences.SelectedItem.Text %></h2>
                <h3>Mail text</h3>
                
                    <asp:PlaceHolder ID="phMailText" runat="server" Visible='<%#!String.IsNullOrEmpty(MailText) %>'>
                        <p><%#MailText %></p>
                        <asp:Button ID="btnSendAll" runat="server" Text="Skicka till alla deltagare" OnClick="btnSendAll_Click"  />
                    </asp:PlaceHolder>

                    <asp:PlaceHolder ID="phNoMailText" runat="server" Visible='<%#String.IsNullOrEmpty(MailText) %>'>
                        <p><i>Text till mail saknas</i></p>
                    </asp:PlaceHolder>

                    <asp:PlaceHolder ID="phSent" runat="server" Visible="false">
                        <p><i>Mailen har skickats</i></p>
                    </asp:PlaceHolder>
                
            </asp:PlaceHolder>
    </div>
    </form>
</body>
</html>
