<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/Public/MasterPages/GasellernaMasterPage.Master" AutoEventWireup="true" CodeBehind="GasellAdEditor.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.Gasellerna.GasellAdEditor" %>
<%@ Register TagPrefix="DI" TagName="GasellAdEditor" Src="~/Templates/Public/Units/Placeable/Gasellerna/GasellAdEditor.ascx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <DI:GasellAdEditor runat="server" />
</asp:Content>

