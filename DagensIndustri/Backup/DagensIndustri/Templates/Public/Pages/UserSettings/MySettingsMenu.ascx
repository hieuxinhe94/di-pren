<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MySettingsMenu.ascx.cs" Inherits="DagensIndustri.Templates.Public.Pages.UserSettings.MySettingsMenu" %>

<style type="text/css">
.menuHeader { font-size:19px; line-height:24px; font-weight:bold; }
.menuItem { text-decoration:none; font-size:0.9em; }
</style>

<h1>Hantera kunduppgifter</h1>
<p>Via denna sida kan du som är prenumerant hantera dina kunduppgifter. 
Använd menyn nedan för att utföra önskade ärenden.</p>

<table border="0" cellpadding="0" cellspacing="0">
<tr>
<td width="200" valign="top">
    <!--<span class="menuHeader">Kund</span>-->
    <p>
        <asp:HyperLink ID="HyperLinkStart" CssClass="menuItem" runat="server">Start</asp:HyperLink><br />
        <asp:HyperLink ID="HyperLinkPerson" CssClass="menuItem" runat="server">Kontaktuppgifter</asp:HyperLink><br />
        <!--<asp:HyperLink ID="HyperLinkLogin" CssClass="menuItem" runat="server">Inloggningsuppgifter</asp:HyperLink><br />-->
        <asp:HyperLink ID="HyperLinkPermAddress" CssClass="menuItem" runat="server">Permanent adress</asp:HyperLink><asp:Literal ID="LitPermAdrBr" runat="server"><br/></asp:Literal>
        <asp:HyperLink ID="HyperLinkInvoice" CssClass="menuItem" runat="server">Faktura</asp:HyperLink><br />
        <asp:HyperLink ID="HyperLinkGold" CssClass="menuItem" runat="server">Di Guld medlemskap</asp:HyperLink><br />
        <asp:HyperLink ID="HyperLinkComplaint" CssClass="menuItem" runat="server">Reklamation</asp:HyperLink><asp:Literal ID="LitReklBr" runat="server"><br/></asp:Literal>
        <asp:HyperLink ID="HyperLinkCancelSubs" CssClass="menuItem" runat="server">Avsluta prenumeration</asp:HyperLink><br />
    </p>
</td>
<td width="200" valign="top">
    <!--<span class="menuHeader">Prenumeration</span>-->
    <p>
        <asp:PlaceHolder ID="PlaceHolderSubsLinks" runat="server"></asp:PlaceHolder>
    </p>
</td>
<td width="200" valign="top">
    <div style="background-color:#FFFFFF; border:1px solid #E2DFDF; padding:10px; width:100%">
        <asp:Literal ID="LiteralCustomerFacts" runat="server"></asp:Literal>
    </div>
</td>
</tr>
</table>


<%--<div style="float:left;">
    <div style="float:left; width:200px;">
        
    </div>

    <div style="width:420px; float:right;">
        <div style="float:left; width:200px;">
            
            
        </div>
    </div>
</div>--%>

&nbsp;