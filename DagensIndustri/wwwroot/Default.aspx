<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="DagensIndustri.Default" %>
<%@ Register TagPrefix="di" TagName="RssSlider" Src="~/Templates/Public/Units/Static/RssSlider.ascx" %>
<%@ Register TagPrefix="di" TagName="Pufflist" Src="~/Templates/Public/Units/Placeable/StartPagePuffList.ascx" %>
<%--<%@ Register TagPrefix="di" TagName="readtodayspaper" Src="~/Templates/Public/Units/Placeable/SideBarBoxes/ReadTodaysPaper.ascx" %>--%>
<%@ Register TagPrefix="di" TagName="DiGoldMembershipPopup" Src="~/Templates/Public/Units/Placeable/DiGoldMembershipPopup/DiGoldMembershipPopup.ascx" %>
<%@ Register TagPrefix="di" TagName="TopImage" Src="~/Templates/Public/Units/Placeable/TopImage.ascx" %>
<%@ Register TagPrefix="di" TagName="SubscriptionWelcome" Src="~/Templates/Public/Units/Placeable/Subscription/SubscriptionWelcome.ascx"  %>

<asp:Content ID="TopContent" ContentPlaceHolderID="TopContentPlaceHolder" runat="server">
    <!-- Top image -->
    <di:TopImage ID="TopImageControl" runat="server" />
    <!-- //Top image -->
    
    <!-- Subscription welcome -->
    <di:SubscriptionWelcome ID="SubscriptionWelcomeControl" runat="server" />
    <!-- //Subscription welcome -->
</asp:Content>

<asp:Content ID="RSS" ContentPlaceHolderID="RSSPlaceHolder" runat="server">
    <di:RssSlider ID="RssSliderControl" runat="server" />
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
	<di:Pufflist runat="server" />
</asp:Content>

<asp:Content ContentPlaceHolderID="PlaceHolderOnTopOfSidebarBoxes" runat="server">
    <%--<di:readtodayspaper runat="server" />--%>
</asp:Content>