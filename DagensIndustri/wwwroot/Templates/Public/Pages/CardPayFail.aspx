<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="CardPayFail.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.CardPayFail" %>
<%@ Register TagPrefix="di" TagName="Heading" Src="~/Templates/Public/Units/Placeable/Heading.ascx" %>
<%--<%@ Register TagPrefix="di" TagName="Mainintro" Src="~/Templates/Public/Units/Placeable/MainIntro.ascx" %>--%>
<%@ Register TagPrefix="di" TagName="Mainbody" Src="~/Templates/Public/Units/Placeable/MainBody.ascx" %>
<%@ Register TagPrefix="di" TagName="UserMessage" Src="~/Templates/Public/Units/Placeable/UserMessage.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TopContentPlaceHolder" runat="server">
    <di:Heading ID="Heading1" runat="server" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    
    <di:UserMessage ID="UserMessageControl1" runat="server" />
    
    <%--<di:Mainintro ID="Mainintro1" runat="server" />--%>    
    <di:Mainbody ID="Mainbody1" runat="server" />

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="RightColumnPlaceHolder" runat="server">
</asp:Content>


<%--
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="NavigationPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TopContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="RSSPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="HeadingContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="WideMainContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="RightColumnPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content9" ContentPlaceHolderID="PlaceHolderOnTopOfSidebarBoxes" runat="server">
</asp:Content>
<asp:Content ID="Content10" ContentPlaceHolderID="FooterPlaceHolder" runat="server">
</asp:Content>
--%>