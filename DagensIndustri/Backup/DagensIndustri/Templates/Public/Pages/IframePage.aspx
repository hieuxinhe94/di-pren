<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="IframePage.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.IframePage" %>
<asp:Content ID="Content3" ContentPlaceHolderID="RSSPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="HeadingContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="WideMainContentPlaceHolder" runat="server">
    <iframe src='<%= frameSource %>' height='<%= frameHeight %>' width='<%= frameWidth %>' frameborder="0" scrolling="no">
    
    </iframe>
</asp:Content>
