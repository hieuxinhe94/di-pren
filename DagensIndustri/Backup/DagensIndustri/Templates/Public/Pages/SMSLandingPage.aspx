﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="SMSLandingPage.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.SMSLandingPage" %>
<%@ Register TagPrefix="di" TagName="heading" Src="~/Templates/Public/Units/Placeable/Heading.ascx" %>
<%@ Register TagPrefix="di" TagName="mainintro" Src="~/Templates/Public/Units/Placeable/MainIntro.ascx" %>
<%@ Register TagPrefix="di" TagName="mainbody" Src="~/Templates/Public/Units/Placeable/MainBody.ascx" %>
<%@ Register TagPrefix="di" TagName="sidebarboxlist" Src="~/Templates/Public/Units/Placeable/SideBarBoxes/SideBar.ascx" %>

<asp:Content ContentPlaceHolderID="HeadingContentPlaceHolder" runat="server">
    <di:heading ID="Heading1" runat="server" />
</asp:Content>

<asp:Content  ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <!-- Page primary content goes here -->
    <di:mainintro ID="Mainintro1" runat="server" />

    <di:mainbody ID="Mainbody1" runat="server" />
    
    <p>
        <asp:Literal ID="UsernameLiteral" runat="server" /><br />
        <asp:Literal ID="PasswordLiteral" runat="server" />
    </p>
    				
	<!-- // Page primary content goes here -->
</asp:Content>

<asp:Content  ContentPlaceHolderID="RightColumnPlaceHolder" runat="server">
    <di:sidebarboxlist ID="Sidebarboxlist1" runat="server" />
</asp:Content>
