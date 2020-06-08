<%@ Page Language="C#" MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="SubsSleeps.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.UserSettings.SubsSleeps" %>
<%@ Register TagPrefix="di" TagName="UserMessage" Src="~/Templates/Public/Units/Placeable/UserMessage.ascx" %>
<%@ Register TagPrefix="di" TagName="MySettingsMenu" src="~/Templates/Public/Pages/UserSettings/MySettingsMenu.ascx" %>
<%@ Register TagPrefix="di" TagName="Input" Src="~/Tools/Classes/WebControls/InputWithValidation.ascx" %>
<%@ Register TagPrefix="di" TagName="Mainbody" Src="~/Templates/Public/Units/Placeable/MainBody.ascx" %>
<%@ Register TagPrefix="di" TagName="heading" Src="~/Templates/Public/Units/Placeable/Heading.ascx" %>
<%@ Register TagPrefix="di" TagName="SubsSleep" Src="~/Templates/Public/Units/Placeable/UserSettings/SubsSleepsUC.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadingContentPlaceHolder" runat="server">
    <script type="text/javascript" src="/Templates/Public/js/PreventDoublePost.js"></script>
</asp:Content>

<asp:Content ContentPlaceHolderID="TopContentPlaceHolder" runat="server" ></asp:Content>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    
    <di:MySettingsMenu ID="MySettingsMenu1" runat="server" />

    <h2><%= CurrentPage["Heading"] ?? CurrentPage.PageName %></h2>
    <di:Mainbody ID="Mainbody1" runat="server" />

    <di:UserMessage ID="UserMessageControl" runat="server" />
    
    <di:SubsSleep ID="SubsSleep1" runat="server" />

</asp:Content>
