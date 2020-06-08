<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Appendices.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.Appendices" MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master" %>
<%@ Register TagPrefix="di" TagName="heading" Src="~/Templates/Public/Units/Placeable/Heading.ascx" %>
<%@ Register TagPrefix="di" TagName="search" Src="~/Templates/Public/Units/Placeable/AppendicesSearch.ascx" %>

<asp:Content ContentPlaceHolderID="HeadingContentPlaceHolder" runat="server">
    <di:heading runat="server" />
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <di:search runat="server" />
</asp:Content>