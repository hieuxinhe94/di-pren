<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master" CodeBehind="AddContactCompany.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.ContactCompanySearch.AddContactCompany" %>
<%@ Register TagPrefix="di" TagName="Heading" Src="~/Templates/Public/Units/Placeable/Heading.ascx" %>
<%@ Register TagPrefix="di" TagName="UserMessage" Src="~/Templates/Public/Units/Placeable/UserMessage.ascx" %>
<%@ Register TagPrefix="di" TagName="AddContactCompany" Src="~/Templates/Public/Units/Placeable/ContactCompanySearch/AddContactCompany.ascx" %>

<asp:Content ContentPlaceHolderID="HeadingContentPlaceHolder" runat="server">
    <di:Heading ID="HeadingControl" runat="server"/>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    
    <di:UserMessage ID="UserMessageControl" runat="server" />
    <di:AddContactCompany ID="AddContactCompanyControl" runat="server" />

</asp:Content>