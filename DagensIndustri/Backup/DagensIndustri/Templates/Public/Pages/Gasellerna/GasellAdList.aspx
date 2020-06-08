<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/Public/MasterPages/GasellernaMasterPage.Master" AutoEventWireup="true" CodeBehind="GasellAdList.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.Gasellerna.GasellAdList" %>
<%@ Register TagPrefix="DI" TagName="GasellAdList" Src="~/Templates/Public/Units/Placeable/Gasellerna/GasellAdList.ascx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <DI:GasellAdList ID="GasellAdList1" runat="server" />
</asp:Content>

