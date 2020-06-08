<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/Public/MasterPages/GasellernaMasterPage.Master" AutoEventWireup="true" CodeBehind="GasellContainer.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.Gasellerna.GasellContainer" %>
<%@ Register TagPrefix="DI" TagName="GasellContainer" Src="~/Templates/Public/Units/Placeable/Gasellerna/GasellContainer.ascx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
      <div id="GasellContainer">
        <DI:GasellContainer runat="server" />        
    </div>
</asp:Content>

