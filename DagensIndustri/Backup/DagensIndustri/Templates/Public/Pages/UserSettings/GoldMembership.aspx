<%@ Page Language="C#" MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="GoldMembership.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.UserSettings.GoldMembership" %>
<%@ Register TagPrefix="di" TagName="UserMessage" Src="~/Templates/Public/Units/Placeable/UserMessage.ascx" %>
<%@ Register TagPrefix="di" TagName="MySettingsMenu" src="~/Templates/Public/Pages/UserSettings/MySettingsMenu.ascx" %>
<%@ Register TagPrefix="di" TagName="Input" Src="~/Tools/Classes/WebControls/InputWithValidation.ascx" %>
<%@ Register TagPrefix="di" TagName="Mainbody" Src="~/Templates/Public/Units/Placeable/MainBody.ascx" %>


<asp:Content ContentPlaceHolderID="TopContentPlaceHolder" runat="server" ></asp:Content>
<asp:Content ContentPlaceHolderID="HeadingContentPlaceHolder" runat="server"></asp:Content>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    
    <di:MySettingsMenu ID="MySettingsMenu1" runat="server" />

    <h2><%= CurrentPage["Heading"] ?? CurrentPage.PageName %></h2>
    <di:Mainbody ID="Mainbody1" runat="server" />


    <di:UserMessage ID="UserMessageControl" runat="server" />

    <asp:Button ID="ActivateDiGoldButton" CssClass="btn" Text="<%$ Resources: EPiServer, mysettings.personal.digold.becomedigoldmember %>" Visible="true" OnClick="DIGoldMembership_Click" CommandArgument="START" runat="server" />
    <asp:Button ID="DeactivateDiGoldButton" CssClass="btn" Text="<%$ Resources: EPiServer, mysettings.personal.digold.enddigoldmembership %>" Visible="false" OnClick="DIGoldMembership_Click" CommandArgument="END" runat="server" />

</asp:Content>
