<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/Public/MasterPages/GasellernaMasterPage.Master" AutoEventWireup="true" CodeBehind="GasellRoot.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.Gasellerna.GasellRoot" %>
<%@ Register TagPrefix="DI" TagName="GasellRoot" Src="~/Templates/Public/Units/Placeable/Gasellerna/GasellRoot.ascx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <DI:GasellRoot runat="server" />
</asp:Content>
