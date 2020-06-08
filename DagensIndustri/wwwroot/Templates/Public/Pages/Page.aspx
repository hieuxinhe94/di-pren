<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="Page.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.Page" %>
<%@ Register TagPrefix="di" TagName="heading" Src="~/Templates/Public/Units/Placeable/Heading.ascx" %>
<%@ Register TagPrefix="di" TagName="mainintro" Src="~/Templates/Public/Units/Placeable/MainIntro.ascx" %>
<%@ Register TagPrefix="di" TagName="mainbody" Src="~/Templates/Public/Units/Placeable/MainBody.ascx" %>
<%@ Register TagPrefix="di" TagName="sidebarsubmenu" Src="~/Templates/Public/Units/Placeable/SidebarSubMenu.ascx" %>
<%@ Register TagPrefix="di" TagName="sidebarboxlist" Src="~/Templates/Public/Units/Placeable/SideBarBoxes/SideBar.ascx" %>
<%@ Register TagPrefix="di" TagName="xform" Src="~/Templates/Public/Units/Placeable/XForm.ascx" %>
<%@ Register TagPrefix="di" TagName="UserMessage" Src="~/Templates/Public/Units/Placeable/UserMessage.ascx" %>


<asp:Content ContentPlaceHolderID="HeadingContentPlaceHolder" runat="server">
    <di:heading ID="Heading1" runat="server" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <!-- Page primary content goes here -->
    <di:mainintro runat="server" />

    <di:UserMessage ID="UserMessageControl" runat="server" />

    <di:mainbody runat="server" />				
	<!-- // Page primary content goes here -->

    <di:xform ShowStatistics="true" XFormProperty="XForm" runat="server" />

</asp:Content>

<asp:Content ContentPlaceHolderID="RightColumnPlaceHolder" runat="server">
    <asp:PlaceHolder ID="ConferenceSubMenuPlaceHolder" Visible="false" runat="server">
        <di:sidebarsubmenu ID="Sidebarsubmenu" runat="server" />
    </asp:PlaceHolder>
    <di:sidebarboxlist ID="Sidebarboxlist1" runat="server" />
</asp:Content>
