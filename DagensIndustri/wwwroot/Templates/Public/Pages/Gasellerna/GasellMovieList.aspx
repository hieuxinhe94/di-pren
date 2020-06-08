<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/Public/MasterPages/GasellernaMasterPage.Master" AutoEventWireup="true" CodeBehind="GasellMovieList.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.Gasellerna.GasellMovieList" %>
<%@ Register TagPrefix="DI" TagName="GasellMovieList" Src="~/Templates/Public/Units/Placeable/Gasellerna/GasellMovieList.ascx" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <DI:GasellMovieList ID="GasellMovieList1" runat="server" />
</asp:Content>

