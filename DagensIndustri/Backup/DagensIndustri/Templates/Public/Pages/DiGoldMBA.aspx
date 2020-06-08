<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="DiGoldMBA.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.DiGoldMBA" %>
<%@ Register TagPrefix="di" TagName="heading" Src="~/Templates/Public/Units/Placeable/Heading.ascx" %>
<%@ Register TagPrefix="di" TagName="mainintro" Src="~/Templates/Public/Units/Placeable/MainIntro.ascx" %>
<%@ Register TagPrefix="di" TagName="mainbody" Src="~/Templates/Public/Units/Placeable/MainBody.ascx" %>
<%@ Register TagPrefix="di" TagName="UserMessage" Src="~/Templates/Public/Units/Placeable/UserMessage.ascx" %>
<%@ Register TagPrefix="di" TagName="mba" Src="~/Templates/Public/Units/Placeable/MBAForm.ascx" %>
<%@ Register TagPrefix="di" TagName="sidebarboxlist" Src="~/Templates/Public/Units/Placeable/SideBarBoxes/SideBar.ascx" %>

<asp:Content ContentPlaceHolderID="HeadingContentPlaceHolder" runat="server">
    <di:heading runat="server" />
</asp:Content>

<asp:Content  ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    
    <!-- Page primary content -->
    <di:mainintro ID="MainIntro" runat="server" />
    <di:mainbody ID="MainBody" runat="server" />

    <di:UserMessage ID="UserMessageControl" runat="server" />

    <di:mba ID="DiMBA" Visible="true" runat="server" RegisterDiGoldMembershipOnSubmit="true" />

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="RightColumnPlaceHolder" runat="server">
    <di:sidebarboxlist runat="server" />
</asp:Content>