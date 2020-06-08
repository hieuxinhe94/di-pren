<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OldOldOldMainBody.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.OldMainBody" %>

<asp:PlaceHolder runat="server" ID="PhHeading">
    <h1><%= Heading %></h1>
</asp:PlaceHolder>
<asp:PlaceHolder runat="server" ID="PhBody">
    <EPiServer:Property ID="Property1" PropertyName="MainBody" DisplayMissingMessage="false" EnableViewState="false" runat="server" />
</asp:PlaceHolder>