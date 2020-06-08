<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="DateArchiveSearch.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.DateArchiveSearch" %>
<%@ Register TagPrefix="di" TagName="heading" Src="~/Templates/Public/Units/Placeable/Heading.ascx" %>
<%@ Register TagPrefix="di" TagName="mainintro" Src="~/Templates/Public/Units/Placeable/MainIntro.ascx" %>
<%@ Register TagPrefix="di" TagName="datesearch" Src="~/Templates/Public/Units/Placeable/DateArchiveSearch.ascx" %>

<asp:Content ContentPlaceHolderID="HeadingContentPlaceHolder" runat="server">
    <di:heading runat="server" />
</asp:Content>

<asp:Content ID="Content6" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <di:mainintro runat="server" />
    <di:datesearch runat="server" />
</asp:Content>

