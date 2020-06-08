<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/Public/MasterPages/GasellernaMasterPage.Master" AutoEventWireup="true" CodeBehind="GasellPuff.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.Gasellerna.GasellPuff" %>
<%@ Register TagPrefix="DI" TagName="GasellPuff" Src="~/Templates/Public/Units/Placeable/Gasellerna/GasellPuff.ascx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <DI:GasellPuff ID="GasellPuff1" runat="server" />
</asp:Content>

