<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master" CodeBehind="CompanyDetails.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.ContactCompanySearch.CompanyDetails" %>
<%@ Register TagPrefix="di" TagName="WorksiteDetails" Src="~/Templates/Public/Units/Placeable/ContactCompanySearch/CompanyWorksiteDetails.ascx" %>
<%@ Register TagPrefix="di" TagName="DecisionMakers" Src="~/Templates/Public/Units/Placeable/ContactCompanySearch/CompanyDecisionMakers.ascx" %>
<%@ Register TagPrefix="di" TagName="BusinessDetails" Src="~/Templates/Public/Units/Placeable/ContactCompanySearch/CompanyBusiness.ascx" %>
<%@ Register TagPrefix="di" TagName="FinancialDetails" Src="~/Templates/Public/Units/Placeable/ContactCompanySearch/CompanyFinancialDetails.ascx" %>
<%@ Register TagPrefix="di" TagName="UserMessage" Src="~/Templates/Public/Units/Placeable/UserMessage.ascx" %>

<asp:Content ContentPlaceHolderID="WideMainContentPlaceHolder" runat="server">    
    <ul id="company-top-menu">
	    <li>
            <asp:HyperLink ID="UpdateCompanyDetailsHyperLink" NavigateUrl="<%# CreateUpdateCompanyDetailsUrl() %>" CssClass="more" Text="<%$ Resources: EPiServer, contactcompanysearch.companydetails.updatecompanydetails %>" runat="server" />
        </li>
	    <li>
            <asp:HyperLink ID="BackToSearchResultHyperLink" NavigateUrl="<%# CreateBackToSearchResultUrl() %>" CssClass="more" Text="<%$ Resources: EPiServer, contactcompanysearch.companydetails.backtosearchresult %>" runat="server" />
        </li>
    </ul>
		    
	<!-- Content -->
	<div id="content" class="full">
        <di:UserMessage ID="UserMessageControl" runat="server" />
        <di:WorksiteDetails ID="WorksiteDetailsControl" runat="server" />
        <di:DecisionMakers ID="DecisionMakersControl" runat="server" />
        <di:BusinessDetails ID="BusinessControl" runat="server" />
        <di:FinancialDetails ID="FinancialDetailsControl" runat="server" />        
	</div>
	<!-- // Content -->

</asp:Content>
