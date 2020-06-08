<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="DiGoldStartPage.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.DiGoldStartPage" %>
<%@ Register TagPrefix="di" TagName="PuffList" Src="~/Templates/Public/Units/Placeable/StartPagePuffList.ascx" %>
<%@ Register TagPrefix="di" TagName="DiGoldMembershipPopup" Src="~/Templates/Public/Units/Placeable/DiGoldMembershipPopup/DiGoldMembershipPopup.ascx" %>
<%@ Register TagPrefix="di" TagName="TopImage" Src="~/Templates/Public/Units/Placeable/TopImage.ascx" %>
<%@ Register TagPrefix="di" TagName="SubscriptionWelcome" Src="~/Templates/Public/Units/Placeable/Subscription/SubscriptionWelcome.ascx"  %>
<%@ Register TagPrefix="di" TagName="RssSlider" Src="~/Templates/Public/Units/Static/RssSlider.ascx" %>

<asp:Content ID="TopContent" ContentPlaceHolderID="TopContentPlaceHolder" runat="server">
    <!-- Top image -->
    <di:TopImage ID="TopImageControl" runat="server" />
    <!-- //Top image -->

    <!-- Subscription welcome -->
    <di:SubscriptionWelcome ID="SubscriptionWelcomeControl" runat="server" />
    <!-- //Subscription welcome -->
</asp:Content>

<asp:Content ID="RSS" ContentPlaceHolderID="RSSPlaceHolder" runat="server">
    <di:RssSlider ID="RssSliderControl" Visible="false" runat="server" />
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <di:PuffList runat="server" />
</asp:Content>