<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Rules.ascx.cs" Inherits="DagensIndustri.Tools.Admin.Apsis.Rules" %>

<b>Regler</b>
<br />
<br />

<div style="border:solid 1px gray; width:350px; padding:10px;">
    En regel är texten framför '@' tecknet i en e-postadress.<br />
    Kunder vars e-postadresser inleds med en regel får vanliga brev istället för e-postutskick.
</div>

<br />

<i>Lägg till ny regel</i><br />
<div style="margin-top:3px; margin-bottom:3px;">
    <asp:TextBox ID="TextBoxNewRule" runat="server"></asp:TextBox><br />
</div>
<asp:Button ID="ButtonNew" runat="server" Text="Spara" onclick="ButtonNew_Click" />

<br />
<br />

<div style="margin-bottom:3px;">
    <i>Befintliga regler</i><br />
</div>

<asp:DataList 
    ID="DataList1" 
    OnEditCommand="Edit_Command"
    OnUpdateCommand="Update_Command"
    OnDeleteCommand="Delete_Command"
    OnCancelCommand="Cancel_Command"
    runat="server">
    
    <ItemTemplate>
        <td width="150"><%# DataBinder.Eval(Container.DataItem, "emailRule") %>&nbsp;</td>
        <td><asp:LinkButton id="EditButton" Text="Redigera" CommandName="Edit" runat="server"/></td>
    </ItemTemplate>
    
    <EditItemTemplate>
        <td width="150"><asp:TextBox id="TextBoxRule" Width="130" Text='<%# DataBinder.Eval(Container.DataItem, "emailRule") %>' runat="server"/></td>
        <td>
            <asp:LinkButton id="UpdateButton" Text="Spara" CommandName="Update" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "emailRuleId") %>' runat="server"/>&nbsp;
            <asp:LinkButton id="DeleteButton" Text="Radera" CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "emailRuleId") %>' runat="server"/>&nbsp;
            <asp:LinkButton id="CancelButton" Text="Avbryt" CommandName="Cancel" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "emailRuleId") %>' runat="server"/>
        </td>
    </EditItemTemplate>
    
</asp:DataList>