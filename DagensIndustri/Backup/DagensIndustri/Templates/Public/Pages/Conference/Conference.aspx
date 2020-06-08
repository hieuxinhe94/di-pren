<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeBehind="Conference.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.Conference.Conference" %>
<%@ Register TagPrefix="di" TagName="heading" Src="~/Templates/Public/Units/Placeable/Heading.ascx" %>
<%@ Register TagPrefix="di" TagName="mainintro" Src="~/Templates/Public/Units/Placeable/MainIntro.ascx" %>
<%@ Register TagPrefix="di" TagName="mainbody" Src="~/Templates/Public/Units/Placeable/MainBody.ascx" %>
<%@ Register TagPrefix="di" TagName="form" Src="~/Templates/Public/Units/Placeable/Conference/ConferenceApplicationForm.ascx" %>
<%@ Register TagPrefix="di" TagName="languagelink" Src="~/Templates/Public/Units/Placeable/LanguageLink.ascx" %>
<%@ Register TagPrefix="di" TagName="sidebarsubmenu" Src="~/Templates/Public/Units/Placeable/SidebarSubMenu.ascx" %>
<%@ Register TagPrefix="di" TagName="conferenceinformation" Src="~/Templates/Public/Units/Placeable/Conference/ConferenceInformation.ascx" %>
<%@ Register TagPrefix="di" TagName="conferencePdfForm" Src="~/Templates/Public/Units/Placeable/SideBarBoxes/ConfPdfDownloadBox.ascx" %>

<asp:Content ContentPlaceHolderID="HeadingContentPlaceHolder" runat="server">
    <di:heading runat="server" />
</asp:Content>


<asp:Content ID="Content5" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <di:mainintro runat="server" />
    <di:mainbody runat="server" />

    <asp:PlaceHolder ID="FormPlaceHolder" Visible="false" runat="server">

<%--        <!-- Errors -->
        <asp:ValidationSummary ID="ErrorValidationSummary" DisplayMode="BulletList" CssClass="server-error" runat="server" />
	    <!-- // Errors -->--%>

        <!-- Hidden Fields -->
        <asp:HiddenField ID="SelectedTabHiddenField" runat="server" />
        <asp:HiddenField ID="SelectedSectionHiddenField" runat="server" />
        <!-- // Hidden Fields -->
        
        <di:form runat="server" />

        <div id="error" class="popup hidden" title="Felmeddelande">
            <asp:Label runat="server" CssClass="error" ID="LblError"></asp:Label>
        </div>   
        <div id="message" class="popup hidden" title="Meddelande">
            <asp:Label runat="server" ID="LblMessage"></asp:Label>
        </div>  
    </asp:PlaceHolder>
</asp:Content>

<asp:Content ID="Content6" ContentPlaceHolderID="PlaceHolderOnTopOfSidebarBoxes" runat="server">
    <di:languagelink runat="server" />
    <di:sidebarsubmenu runat="server" />
    <di:conferencePdfForm runat="server" />
    <di:conferenceinformation runat="server" />
</asp:Content>
