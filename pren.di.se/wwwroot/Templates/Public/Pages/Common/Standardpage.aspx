<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master" CodeBehind="Standardpage.aspx.cs" Inherits="PrenDiSe.Templates.Public.Pages.Common.Standardpage" %>

<asp:Content ContentPlaceHolderID="NavigationPlaceHolder" runat="server">
  <asp:Image ID="imgLogotype" CssClass="standardImgLogo" Visible="False" runat="server"/>
</asp:Content>

<asp:Content ContentPlaceHolderID="HeadingContentPlaceHolder" runat="server">
  
  <h1><EPiServer:Property PropertyName="Heading" runat="server"/></h1>

</asp:Content>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
  
  <EPiServer:Property PropertyName="MainIntro" runat="server"/>
  
  <EPiServer:Property PropertyName="MainBody" runat="server"/>
  
</asp:Content>