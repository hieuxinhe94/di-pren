<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/Public/MasterPages/GasellernaMasterPage.Master" AutoEventWireup="true" CodeBehind="GasellAdContainer.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.Gasellerna.GasellAdContainer" %>
<%@ Register TagPrefix="DI" TagName="GasellAdContainer" Src="~/Templates/Public/Units/Placeable/Gasellerna/GasellAdContainer.ascx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <DI:GasellAdContainer runat="server" />   
</asp:Content>

