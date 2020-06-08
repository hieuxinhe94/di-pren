<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master" CodeBehind="Search.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.ContactCompanySearch.Search" %>
<%@ Register TagPrefix="di" TagName="Heading" Src="~/Templates/Public/Units/Placeable/Heading.ascx" %>
<%@ Register TagPrefix="di" TagName="MainIntro" Src="~/Templates/Public/Units/Placeable/MainIntro.ascx" %>
<%@ Register TagPrefix="di" TagName="MainBody" Src="~/Templates/Public/Units/Placeable/MainBody.ascx" %>
<%@ Register TagPrefix="di" TagName="UserMessage" Src="~/Templates/Public/Units/Placeable/UserMessage.ascx" %>
<%@ Register TagPrefix="di" TagName="Search" Src="~/Templates/Public/Units/Placeable/ContactCompanySearch/SearchControl.ascx" %>

<asp:Content ContentPlaceHolderID="HeadingContentPlaceHolder" runat="server">
    <di:Heading runat="server" />
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

    <di:UserMessage ID="UserMessageControl" runat="server" />
    <di:Search ID="SearchControl" runat="server" />    
    <di:MainIntro ID="MainIntro" runat="server" />
    <di:MainBody ID="MainBody" runat="server" />
	
</asp:Content>