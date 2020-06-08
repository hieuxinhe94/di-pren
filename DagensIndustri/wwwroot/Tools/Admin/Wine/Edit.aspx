<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="DagensIndustri.Tools.Admin.Wine.Edit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Administration av viner - <%=ModeString%></title>

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
    <h1><%=ModeString %> vin</h1>
    <div class="error">
       <asp:Label ID="lbError" runat="server" ForeColor="Red" />
       <asp:Label ID="lbSuccess" runat="server" ForeColor="Green" />
    </div>
    <table>
        <tr>
            <td><asp:Label ID="lbVarnummer" runat="server" Text="Varunummer" AssociatedControlID="tbVarnummer" /> </td>
            <td><asp:TextBox ID="tbVarnummer" runat="server" />
            <asp:Button ID="btnGetArticle" runat="server" Text="Hämta artikel" OnClick="btnGetArticle_Click" />
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <div id="divArticleInfo" runat="server" style="font-weight:bold;width:200px;">
                </div>
            </td>
        </tr>
        <tr>
            <td> <asp:Label ID="lbDate" runat="server" Text="Datum (åååå-mm-dd)" AssociatedControlID="tbDate" /> </td>
            <td><asp:TextBox ID="tbDate" runat="server" /></td>
            <td><i>Datumet styr publiceringen</i></td>
        </tr>
        <tr>
            <td> <asp:Label ID="lbLongitude" runat="server" Text="Longitud" AssociatedControlID="tbLongitude" /> </td>
            <td><asp:TextBox ID="tbLongitude" runat="server" /></td>
            <td><i>Longitud, ej obligatorisk</i></td>
        </tr>
        <tr>
            <td> <asp:Label ID="lbLatitude" runat="server" Text="Latitud" AssociatedControlID="tbLatitude" /> </td>
            <td><asp:TextBox ID="tbLatitude" runat="server" /></td>
            <td><i>Latitud, ej obligatorisk</i></td>
        </tr>
        <tr>
            <td><asp:Label ID="lbAbout" runat="server" Text="Text" AssociatedControlID="tbAbout" /> </td>
            <td><asp:TextBox ID="tbAbout" runat="server" TextMode="MultiLine" Rows="3" Columns="50" /></td>
        </tr>
        <tr>
            <td><asp:Label ID="lbCharacter" runat="server" Text="karaktär" /></td>
            <td>
                <asp:CheckBoxList ID="cblCharacter" runat="server" DataSourceID="dsCharacters" DataTextField="name" DataValueField="id" OnDataBound="cblCharacter_DataBound" />
                <asp:ObjectDataSource ID="dsCharacters" TypeName="DIClassLib.Wine.WineHandler" SelectMethod="GetWineCharacters" runat="server" />
            </td>
        </tr>
    </table>
    
    <div>
       
    </div>
    <div>
       <asp:Label ID="lbAddCharacter" runat="server" Text="Lägg till karaktär" /> 
       <asp:TextBox ID="tbAddCharacter" runat="server" />
       <asp:Button ID="btnAddCharacter" runat="server" Text="Lägg till" OnClick="btnAddCharacter_Click" />
    </div>
    <div>
        <asp:Button ID="btnSave" runat="server" Text="Spara" OnClick="btnSave_Click" />
    </div>
    <div>
        <a href="WineAdmin.aspx">Avbryt</a>
    </div>
    </form>
</body>
</html>
