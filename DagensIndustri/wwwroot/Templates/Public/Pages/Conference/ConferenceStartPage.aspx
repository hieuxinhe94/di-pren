<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConferenceStartPage.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.Conference.ConferenceStartPage" MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master" %>
<%@ Register TagPrefix="di" TagName="heading" Src="~/Templates/Public/Units/Placeable/Heading.ascx" %>
<%@ Register TagPrefix="di" TagName="conferencelist" Src="~/Templates/Public/Units/Placeable/Conference/ConferenceList.ascx" %>
<%@ Register TagPrefix="di" TagName="language" Src="~/Templates/Public/Units/Placeable/LanguageLink.ascx" %>
<%@ Register TagPrefix="di" TagName="sidebarlist" Src="~/Templates/Public/Units/Placeable/SideBarBoxes/SideBar.ascx"  %>

<asp:Content ContentPlaceHolderID="HeadingContentPlaceHolder" runat="server">

</asp:Content>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <di:conferencelist runat="server" />
</asp:Content>

<asp:Content ContentPlaceHolderID="RightColumnPlaceHolder" runat="server">
    <di:language runat="server" />
    <di:sidebarlist runat="server" />
</asp:Content>