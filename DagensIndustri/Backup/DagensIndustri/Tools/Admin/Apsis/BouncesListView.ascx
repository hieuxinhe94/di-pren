<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BouncesListView.ascx.cs" Inherits="DagensIndustri.Tools.Admin.Apsis.BouncesListView" %>

<script language="javascript" type="text/javascript">
    function jsConf() {
        if (confirm('Vill du verkligen radera kunden?')) {
            return true;
        }
        else {
            return false;
        }
    }
</script>


<b>Studsar</b>
<br />
<br />
<div style="border:solid 1px gray; width:600px;">
    <div style="margin-left:-15px; padding-right:10px;">
        <ul>
        <li>Felaktiga e-postadresser som ändras nedan uppdateras i Cirix customer- och lettertabell samt i detta system. Nya utskicksförsök görs automatiskt till uppdaterade e-postadresser när det schemalagda jobbet "Apsis mail" körs.</li>
        <li style="margin-top:8px; margin-bottom:8px;">För studsar som uppstått pga tillfälliga fel (mottagarens e-postserver låg nere el dyl) är det lämpligt att göra nya utskicksförsök. Kryssa för checkboxen "gör nytt försök" och spara.</li>
        <li style="margin-bottom:8px;">Då det inte är lämpligt att göra nya utskicksförsök (kundens e-postadress existerar inte el dyl) bör utskick med brev göras. Välj då "skicka vanligt brev" och spara.</li>
        <li>Radera kan användas för testposter alternativt kunder som det inte går att få tag i fullständiga uppgifter för. Vid radera får kunden varken välkomstmail eller välkomstbrev.</li>
        </ul>
    </div>
</div>

<br />
<asp:Label ID="LabelMess" ForeColor="Red" runat="server"></asp:Label>
<br />


<asp:ListView 
ID="ListView1" 
OnItemUpdating="ListView_Update_Command" 
OnItemDeleting="ListView_Delete_Command"
ItemPlaceholderID="PanelLayoutTempl" 
OnPagePropertiesChanging="PagePropertiesChanging" 
runat="server">
    
    <LayoutTemplate>
        <table border="0" cellpadding="0" cellspacing="0">
            <asp:Panel ID="PanelLayoutTempl" runat="server"></asp:Panel>
        </table>
    </LayoutTemplate>

    <ItemTemplate>
        
        <tr><td colspan="2"><hr /></td></tr>
       
        <tr>
        <td width="150">Namn</td>
        <td><%# DataBinder.Eval(Container.DataItem, "name") %></td>
        </tr>
        
        <tr>
        <td>Tel hem</td>
        <td><asp:Literal ID="LiteralPhoneHome" EnableViewState="false" runat="server"></asp:Literal></td>
        </tr>
        <tr>
        <td>Tel arbete</td>
        <td><asp:Literal ID="LiteralPhoneWork" EnableViewState="false" runat="server"></asp:Literal></td>
        </tr>
        <tr>
        <td>Tel annan</td>
        <td><asp:Literal ID="LiteralPhoneOther" EnableViewState="false" runat="server"></asp:Literal></td>
        </tr>
        
        <tr>
        <td>E-post</td>
        <td><asp:TextBox ID="TextBoxEmail" Width="200" Text='<%# DataBinder.Eval(Container.DataItem, "email") %>' EnableViewState="false" runat="server"></asp:TextBox></td>
        <asp:HiddenField ID="HiddenFieldEmail" Value='<%# DataBinder.Eval(Container.DataItem, "email") %>' EnableViewState="false" runat="server" />
        </tr>
        
        <tr>
        <td valign="top">Kontaktstatus</td>
        <td><asp:TextBox ID="TextBoxContactStatus" Width="200" TextMode="MultiLine" Rows="2" Text='<%# DataBinder.Eval(Container.DataItem, "contactStatus") %>' EnableViewState="false" runat="server"></asp:TextBox></td>
        <asp:HiddenField ID="HiddenFieldContactStatus" Value='<%# DataBinder.Eval(Container.DataItem, "contactStatus") %>' EnableViewState="false" runat="server" />
        </tr>
        
        <tr>
        <td>Gör nytt försök</td>
        <td><asp:CheckBox ID="CheckBoxForceRetry" EnableViewState="false" runat="server" /></td>
        </tr>
        
        <tr>
        <td>Skicka vanligt brev
        </td>
        <td><asp:CheckBox ID="CheckBoxRegularLetter" EnableViewState="false" runat="server" /></td>
        </tr>
        
        <tr>
        <td></td>
        <td>
            <table border="0" cellpadding="0" cellspacing="0">
            <tr>
            <td><asp:Button ID="UpdateButton" Text="Spara" CommandName="Update" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "customerId") %>' runat="server" /></td>
            <td width="5"></td>
            <td><asp:Button ID="ButtonDelete" Text="Radera" CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "customerId") %>' runat="server" OnClientClick="return jsConf()" /></td>
            </tr>
            </table>
        </td>
        </tr>
        
        <tr><td colspan="3">&nbsp;</td></tr>
        
        <tr>
        <td>Cirix kundId</td>
        <td><%# DataBinder.Eval(Container.DataItem, "customerId") %></td>
        </tr>
        
        <tr>
        <td>Målgrupp</td>
        <td><%# DataBinder.Eval(Container.DataItem, "targetGroup") %></td>
        </tr>
        
        <tr>
        <td>CampId</td>
        <td><%# DataBinder.Eval(Container.DataItem, "campId") %></td>
        </tr>
        
        <tr>
        <td>Prenperiod</td>
        <td><%# GetDate(Container.DataItem, "subsStartDate")%> - <%# GetDate(Container.DataItem, "subsEndDate") %></td>
        </tr>
        
        
        <asp:Panel ID="PanelBounceDetails" EnableViewState="false" runat="server">
            
            <tr><td colspan="3">&nbsp;</td></tr>
            
            <tr>
            <td colspan="2"><b>Studsfakta</b></td>
            </tr>
            
            <tr>
            <td>Utskicksdatum</td>
            <td><asp:Literal ID="LiteralDateSaved" EnableViewState="false" runat="server"></asp:Literal></td>
            </tr>
            
            <tr>
            <td>Studsdatum</td>
            <td><asp:Literal ID="LiteralDateBounce" EnableViewState="false" runat="server"></asp:Literal></td>
            </tr>
            
            <tr>
            <td>Mottagaradress</td>
            <td><asp:Literal ID="LiteralEmailRec" EnableViewState="false" runat="server"></asp:Literal></td>
            </tr>
            
            <tr>
            <td>Studskategori</td>
            <td><asp:Literal ID="LiteralBounceCat" EnableViewState="false" runat="server"></asp:Literal></td>
            </tr>
            
            <tr>
            <td>Studsorsak</td>
            <td><asp:Literal ID="LiteralBounceReason" EnableViewState="false" runat="server"></asp:Literal></td>
            </tr>
            
            <tr>
            <td>Apsis mail-id</td>
            <td><asp:Literal ID="LiteralApsisMailId" EnableViewState="false" runat="server"></asp:Literal></td>
            </tr>
            
            <tr>
            <td>Apsis identifierare</td>
            <td><asp:Literal ID="LiteralApsisIdentifier" EnableViewState="false" runat="server"></asp:Literal></td>
            </tr>
        </asp:Panel>
        
    </ItemTemplate>
    
</asp:ListView>

<br />


<div style="width:420px; text-align:center;">
    <hr />
    <asp:DataPager ID="DataPager1" PagedControlID="ListView1" PageSize="10" runat="server">
        <Fields>
            <asp:NextPreviousPagerField 
            PreviousPageText="Föregående" 
            RenderNonBreakingSpacesBetweenControls="true" 
            NextPageText="Nästa" />
        </Fields>
    </asp:DataPager>
</div>

<br />
<br />

