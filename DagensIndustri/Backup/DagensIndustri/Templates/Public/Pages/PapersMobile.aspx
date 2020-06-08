<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master"  CodeBehind="PapersMobile.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.PapersMobile" %>
<%@ Register TagPrefix="di" TagName="Heading" Src="~/Templates/Public/Units/Placeable/Heading.ascx" %>
<%@ Register tagPrefix="di" tagName="Papers" src="~/Templates/Public/Units/Placeable/Papers.ascx" %>

<asp:Content runat="server" ID="head" ContentPlaceHolderID="head">
  
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="TopContentPlaceHolder" runat="server" />

<asp:Content ID="Content2" ContentPlaceHolderID="HeadingContentPlaceHolder" runat="server">
  <di:Heading ID="Heading1" runat="server" />
</asp:Content>
    
<asp:Content ID="Content3" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
  <di:Papers ID="Papers1" MobileView="True" runat="server"/>
</asp:Content>
    
<asp:Content ID="Content4" ContentPlaceHolderID="RightColumnPlaceHolder" runat="server">
  
</asp:Content>
