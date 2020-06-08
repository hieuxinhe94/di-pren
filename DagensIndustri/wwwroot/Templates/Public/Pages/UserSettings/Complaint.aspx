<%@ Page Language="C#" MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="Complaint.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.UserSettings.Complaint" %>
<%@ Register TagPrefix="di" TagName="UserMessage" Src="~/Templates/Public/Units/Placeable/UserMessage.ascx" %>
<%@ Register TagPrefix="di" TagName="MySettingsMenu" src="~/Templates/Public/Pages/UserSettings/MySettingsMenu.ascx" %>
<%@ Register TagPrefix="di" TagName="Input" Src="~/Tools/Classes/WebControls/InputWithValidation.ascx" %>
<%@ Register TagPrefix="di" TagName="Mainbody" Src="~/Templates/Public/Units/Placeable/MainBody.ascx" %>
<%@ Register TagPrefix="di" TagName="heading" Src="~/Templates/Public/Units/Placeable/Heading.ascx" %>
<%@ Register TagPrefix="di" tagname="Complaint" src="~/Templates/Public/Units/Placeable/UserSettings/ComplaintUC.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadingContentPlaceHolder" runat="server">
    <%--<di:heading ID="Heading1" runat="server" />--%>
    <script type="text/javascript" src="/Templates/Public/js/PreventDoublePost.js"></script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="TopContentPlaceHolder" runat="server" ></asp:Content>


<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

    <di:MySettingsMenu ID="MySettingsMenu1" runat="server" />

    <h2><%= CurrentPage["Heading"] ?? CurrentPage.PageName %></h2>
    <di:Mainbody ID="Mainbody1" runat="server" />

    <di:UserMessage ID="UserMessageControl" runat="server" />

    <di:Complaint ID="Complaint1" runat="server" />


</asp:Content>

