<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SendCodeToEmail.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.SingleSingOn.SendCodeToEmail" %>



<div class="borderBox" style="width:300px;">
    <div class="idHeader">Saknar du kod?</div>
    Ange ditt kundnummer eller e-postadress och tryck "OK" så skickas koden till den mailadress vi har registrerad hos oss.<br /><br />
    Ange kundnummer eller e-post<br />
    <asp:TextBox ID="TextBoxRemind" CssClass="spaceTop" Width="200" runat="server" /><br />
    <asp:Button ID="ButtonSendLogin" runat="server" Text="OK" CssClass="spaceTop" onclick="ButtonSendCode_Click" />

    <%--    
    <br />
    <br />
    <br />
    Kontakta kundtjänst om du saknar uppgifter
    --%>
</div>
