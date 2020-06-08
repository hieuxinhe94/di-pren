<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConferenceStartPageEn.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.Conference.ConferenceStartPageEn" MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master" %>
<%@ Register TagPrefix="di" TagName="heading" Src="~/Templates/Public/Units/Placeable/Heading.ascx" %>
<%@ Register TagPrefix="di" TagName="mainintro" Src="~/Templates/Public/Units/Placeable/MainIntro.ascx" %>
<%@ Register TagPrefix="di" TagName="mainbody" Src="~/Templates/Public/Units/Placeable/MainBody.ascx" %>
<%@ Register TagPrefix="di" TagName="conferencelist" Src="~/Templates/Public/Units/Placeable/Conference/ConferenceListEn.ascx" %>
<%@ Register TagPrefix="di" TagName="language" Src="~/Templates/Public/Units/Placeable/LanguageLink.ascx" %>
<%@ Register TagPrefix="di" TagName="sidebarsubmenu" Src="~/Templates/Public/Units/Placeable/SidebarSubMenu.ascx" %>
<%@ Register TagPrefix="di" TagName="sidebarlist" Src="~/Templates/Public/Units/Placeable/SideBarBoxes/SideBar.ascx"  %>

<asp:Content ContentPlaceHolderID="HeadingContentPlaceHolder" runat="server">
    <di:heading runat="server" />
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <di:mainintro runat="server" />
    <di:mainbody runat="server" />
    <di:conferencelist runat="server" />
</asp:Content>

<asp:Content ContentPlaceHolderID="RightColumnPlaceHolder" runat="server">
    <di:language runat="server" />
    <di:sidebarsubmenu runat="server" />
    <di:sidebarlist runat="server" />
</asp:Content>
