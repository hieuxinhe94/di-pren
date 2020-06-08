<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master" CodeBehind="SimplePage.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.SimplePage" %>
<%@ Register TagPrefix="di" TagName="Heading" Src="~/Templates/Public/Units/Placeable/Heading.ascx" %>
<%@ Register TagPrefix="di" TagName="Mainintro" Src="~/Templates/Public/Units/Placeable/MainIntro.ascx" %>
<%@ Register TagPrefix="di" TagName="Mainbody" Src="~/Templates/Public/Units/Placeable/MainBody.ascx" %>

<asp:Content ContentPlaceHolderID="TopContentPlaceHolder" runat="server">
    <di:Heading runat="server" />
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <!-- Page primary content goes here -->
    <di:Mainintro runat="server" />

    <di:Mainbody runat="server" />				
	<!-- // Page primary content goes here -->
</asp:Content>

<asp:Content ContentPlaceHolderID="RightColumnPlaceHolder" runat="server">
</asp:Content>