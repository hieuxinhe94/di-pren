<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Logo.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Static.Logo" %>


<asp:PlaceHolder ID="PlaceHolderLink" Visible="true" runat="server">
    <a href='<%= logoLinkUrl %>' title="Dagens industri" rel="home" class="logo">
        <img src="<%= logoUrl %>" alt="<%= altText %>" />
    </a>
</asp:PlaceHolder>


<asp:PlaceHolder ID="PlaceHolderNoLink" Visible="false" runat="server">
    <div class="logo">
        <img src="<%= logoUrl %>" alt="<%= altText %>" />
    </div>
</asp:PlaceHolder>