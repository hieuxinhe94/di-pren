<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="XForm.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.XForm" MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master" %>
<%@ Register TagPrefix="di" TagName="xform" Src="~/Templates/Public/Units/Placeable/XForm.ascx" %>

<asp:Content ContentPlaceHolderID="WideMainContentPlaceHolder" runat="server">
    <di:xform ShowStatistics="true" XFormProperty="FormProperty" runat="server" />
</asp:Content>
