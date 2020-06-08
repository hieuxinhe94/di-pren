<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="AutowithdrawalPayPage.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.AutowithdrawalPayPage" %>
<%@ MasterType virtualpath="~/Templates/Public/MasterPages/MasterPage.Master" %>
<%@ Register TagPrefix="di" TagName="heading" Src="~/Templates/Public/Units/Placeable/Heading.ascx" %>
<%@ Register TagPrefix="di" TagName="mainintro" Src="~/Templates/Public/Units/Placeable/MainIntro.ascx" %>
<%@ Register TagPrefix="di" TagName="mainbody" Src="~/Templates/Public/Units/Placeable/MainBody.ascx" %>
<%@ Register TagPrefix="di" TagName="sidebarboxlist" Src="~/Templates/Public/Units/Placeable/SideBarBoxes/SideBar.ascx" %>
<%@ Register TagPrefix="DI" TagName="UserMessage" Src="~/Templates/Public/Units/Placeable/UserMessage.ascx" %>


<asp:Content ContentPlaceHolderID="HeadingContentPlaceHolder" runat="server">
    <di:heading ID="Heading1" runat="server" />
</asp:Content>

<asp:Content ID="MainContentPlaceHolder1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

    <di:mainintro ID="Mainintro1" runat="server" />
    <di:mainbody ID="Mainbody1" runat="server" />

    <di:UserMessage ID="UserMessageControl1" runat="server" />
    
    <asp:PlaceHolder ID="PlaceHolderSub" runat="server">
        <h2>Din prenumeration</h2>
        <p>
            Produkt: <asp:Literal ID="LiteralProduct" runat="server" /><br />
            Kundnummer: <asp:Literal ID="LiteralCusno" runat="server" /><br />
            Prenumerationsnummer: <asp:Literal ID="LiteralSubsno" runat="server" /><br />
            Prenumerationsslut: <asp:Literal ID="LiteralSubsEndDate" runat="server" /><br />
            Pris: <asp:Literal ID="LiteralPrice" runat="server" /> kr (inkl. moms)<br />
            <br />
            <asp:Button ID="ButtonBuy" runat="server" Text="Genomför kortköp" onclick="ButtonBuy_Click" />
        </p>
    </asp:PlaceHolder>

</asp:Content>

<asp:Content ContentPlaceHolderID="RightColumnPlaceHolder" runat="server">
    <di:sidebarboxlist runat="server" />
</asp:Content>


<%--
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="NavigationPlaceHolder" runat="server"></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TopContentPlaceHolder" runat="server"></asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="RSSPlaceHolder" runat="server"></asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="HeadingContentPlaceHolder" runat="server"></asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="WideMainContentPlaceHolder" runat="server"></asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="MainContentPlaceHolder" runat="server"></asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="RightColumnPlaceHolder" runat="server"></asp:Content>
<asp:Content ID="Content9" ContentPlaceHolderID="PlaceHolderOnTopOfSidebarBoxes" runat="server"></asp:Content>
<asp:Content ID="Content10" ContentPlaceHolderID="FooterPlaceHolder" runat="server"></asp:Content>
--%>