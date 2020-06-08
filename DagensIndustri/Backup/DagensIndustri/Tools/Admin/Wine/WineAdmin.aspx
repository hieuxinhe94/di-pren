<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WineAdmin.aspx.cs" Inherits="DagensIndustri.Tools.Admin.Wine.WineAdmin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
        
        .error { color:#FF0000; margin-top:18px; margin-bottom:18px; font-weight:bold; }
        .success { color:green; margin-top:18px; margin-bottom:18px; font-weight:bold; }
        .container{padding:20px 20px 20px 20px;}
        
    </style>  
    <link href="/Tools/Admin/Styles/StyleSheetPublic.css" rel="stylesheet" type="text/css" />    
</head>
<body>
    <form id="form1" runat="server">
    <div class="container">
        <h1>Administration av viner</h1>
        <div style="padding-top:18px; padding-bottom:18px;">
           <a href="/Tools/Admin/Wine/Edit.aspx">&gt; Lägg till vin</a> <br />
           <a href="/Tools/Admin/Wine/UpdateDatabase.aspx">&gt; Uppdatera vindatabas</a> <br />
           <asp:MultiView ID="multiCharacters" runat="server" ActiveViewIndex="0">
            <asp:View ID="viewCharacterDefault" runat="server">
                Redigera/ta bort karaktär:
                   <asp:DropDownList ID="ddlCharacters" runat="server" DataSourceID="dsCharacters" DataTextField="name" DataValueField="id" />
                   <asp:ObjectDataSource ID="dsCharacters" TypeName="DIClassLib.Wine.WineHandler" SelectMethod="GetWineCharacters" runat="server" />
                   <asp:Button ID="btnDeleteCharacter" runat="server" OnClick="btnDeleteCharacter_Click" Text="Ta bort" OnClientClick="return confirm('Är du säker på att du vill ta bort karaktären?');" />
                   <asp:Button ID="btnEditCharacter" runat="server" OnClick="btnEditCharacter_Click" Text="Redigera" />
            </asp:View>
            <asp:View ID="viewCharacterEdit" runat="server">
                <b>Redigera karaktär</b>
                <asp:TextBox ID="tbCharacterName" runat="server" />
                <asp:Button ID="btnUpdateCharacter" runat="server" OnClick="btnUpdateCharacter_Click" Text="Spara" />
                <asp:Button ID="btnCancelEditCharacter" runat="server" OnClick="btnCancelEditCharacter_Click" Text="Avbryt" />
            </asp:View>
       
           </asp:MultiView>
       
        </div>
       
        <div>
            <asp:Label ID="lbSuccess" runat="server" CssClass="success" />

             <asp:Label ID="lbError" runat="server" CssClass="error" />
        </div>
        <div style="padding-top:18px; padding-bottom:18px;">
            <asp:GridView ID="gvWines" runat="server" DataSourceID="dsWines" AutoGenerateColumns="false" OnRowCommand="gvWines_RowCommand" CssClass="AdminTable">
                <Columns>
                    <asp:BoundField DataField="varnummer" HeaderText="Nummer" />
                    <asp:BoundField DataField="namn" HeaderText="Namn" />
                    <asp:BoundField DataField="namn2" HeaderText="Namn 2" />
                    <asp:BoundField DataField="date" HeaderText="Datum" DataFormatString="{0:yyyy-MM-dd}" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <a href="Edit.aspx?wine=<%#Eval("id") %>">Redigera</a>

                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
          
                            <asp:LinkButton ID="lbtnDelete" runat="server" OnClientClick="return confirm('Är du säker på att du vill radera vinet?');"
                                CommandName="DeleteWine" CommandArgument='<%#Eval("id") %>' Text="Radera" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>

            </asp:GridView>    
            <asp:ObjectDataSource ID="dsWines" runat="server" TypeName="DIClassLib.Wine.WineHandler" SelectMethod="GetAllWines" />
        </div>
    </div>
    </form>
    
</body>
</html>
