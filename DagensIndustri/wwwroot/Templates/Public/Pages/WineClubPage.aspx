<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WineClubPage.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.WineClubPage" MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master" %>
<%@ Register TagPrefix="di" TagName="heading" Src="~/Templates/Public/Units/Placeable/Heading.ascx" %>
<%@ Register TagPrefix="di" TagName="mainintro" Src="~/Templates/Public/Units/Placeable/MainIntro.ascx" %>
<%@ Register TagPrefix="di" TagName="mainbody" Src="~/Templates/Public/Units/Placeable/MainBody.ascx" %>
<%@ Register TagPrefix="di" TagName="UserMessage" Src="~/Templates/Public/Units/Placeable/UserMessage.ascx" %>
<%@ Register TagPrefix="di" TagName="wineclubform" Src="~/Templates/Public/Units/Placeable/WineClubForm.ascx" %>
<%@ Register TagPrefix="di" TagName="sidebarboxlist" Src="~/Templates/Public/Units/Placeable/SideBarBoxes/SideBar.ascx" %>
<%@ Register TagPrefix="di" TagName="winelist" Src="~/Templates/Public/Units/Placeable/SideBarBoxes/WineList.ascx" %>

<asp:Content ContentPlaceHolderID="HeadingContentPlaceHolder" runat="server">
    <di:heading ID="Heading1" runat="server" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <!-- Page primary content goes here -->
    <di:mainintro ID="Mainintro1" runat="server" />

    <di:mainbody ID="Mainbody1" runat="server" />				
	<!-- // Page primary content goes here --><!-- Errors -->
    <di:UserMessage ID="UserMessageControl" runat="server" />
	<!-- // Errors -->

    <di:wineclubform ID="Wineclubform1" runat="server" />
</asp:Content>

<asp:Content ContentPlaceHolderID="RightColumnPlaceHolder" runat="server">
    <di:sidebarboxlist runat="server" />
    <di:winelist PageID='<%# wineListPageID %>' runat="server" />
</asp:Content>