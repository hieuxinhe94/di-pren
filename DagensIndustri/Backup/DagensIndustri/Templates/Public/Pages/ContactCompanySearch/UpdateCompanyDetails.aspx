<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master"  CodeBehind="UpdateCompanyDetails.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.ContactCompanySearch.UpdateCompanyDetails" %>
<%@ Register TagPrefix="di" TagName="Heading" Src="~/Templates/Public/Units/Placeable/Heading.ascx" %>
<%@ Register TagPrefix="di" TagName="UserMessage" Src="~/Templates/Public/Units/Placeable/UserMessage.ascx" %>
<%@ Register TagPrefix="di" TagName="UpdateCompanyDetails" Src="~/Templates/Public/Units/Placeable/ContactCompanySearch/UpdateCompanyDetails.ascx" %>

<asp:Content ContentPlaceHolderID="HeadingContentPlaceHolder" runat="server">
     <di:Heading ID="HeadingControl" runat="server"/>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    
    <di:UserMessage ID="UserMessageControl" runat="server" />
    <di:UpdateCompanyDetails ID="UpdateCompanyDetailsControl" runat="server" />

</asp:Content>