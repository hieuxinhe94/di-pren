<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/Public/MasterPages/GasellernaMasterPage.Master" AutoEventWireup="true" CodeBehind="GasellSearch.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.Gasellerna.GasellSearch" %>
<%@ Register TagPrefix="DI" TagName="GasellSearch" Src="~/Templates/Public/Units/Placeable/Gasellerna/GasellSearch.ascx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <DI:GasellSearch ID="GasellSearch1" runat="server" />
</asp:Content>

