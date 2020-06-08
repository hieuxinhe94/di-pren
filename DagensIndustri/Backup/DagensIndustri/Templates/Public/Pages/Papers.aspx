<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master" CodeBehind="Papers.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.Papers" %>
<%@ Register TagPrefix="di" TagName="Heading" Src="~/Templates/Public/Units/Placeable/Heading.ascx" %>
<%--<%@ Register TagPrefix="di" TagName="BuyTodaysPaper" Src="~/Templates/Public/Units/Placeable/SideBarBoxes/BuyTodaysPaper.ascx" %>--%>
<%@ Register TagPrefix="di" TagName="sidebar" Src="~/Templates/Public/Units/Placeable/SideBarBoxes/SideBar.ascx" %>
<%@ Register tagPrefix="di" tagName="Papers" src="~/Templates/Public/Units/Placeable/Papers.ascx" %>

<asp:Content ContentPlaceHolderID="TopContentPlaceHolder" runat="server" />

<asp:Content ContentPlaceHolderID="HeadingContentPlaceHolder" runat="server">
    <di:Heading runat="server" />
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    
    <di:Papers runat="server"/>

</asp:Content>

<asp:Content ContentPlaceHolderID="RightColumnPlaceHolder" runat="server">
    <di:sidebar ID="Sidebar1" runat="server" />
    <%--<di:BuyTodaysPaper runat="server" />--%>
</asp:Content>