<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/Public/MasterPages/CampaignMaster.Master" AutoEventWireup="true" CodeBehind="EventPage.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.Campaign.EventPage" %>
<%@ Register TagPrefix="public" TagName="MainBody"      Src="~/Templates/Public/Units/Placeable/OldMainBody.ascx" %>
<%@ Register TagPrefix="DI" TagName="Input" Src="~/Templates/Public/Units/Placeable/InputWithValidation.ascx" %>
<%@ Register TagPrefix="DI" TagName="EventArea" Src="~/Templates/Public/Units/Placeable/Campaign/EventArea.ascx" %>


<asp:Content ID="Content2" ContentPlaceHolderID="CampaignRegion" runat="server">
	<div id="EventArea">
	
	    <DI:EventArea runat="server"></DI:EventArea>
	
    </div>
</asp:Content>

