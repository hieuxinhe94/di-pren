<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CompanyFinancialDetails.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.ContactCompanySearch.CompanyFinancialDetails" %>

<!-- Section -->
<div class="section">
	<h2><EPiServer:Translate Text="/contactcompanysearch/companydetails/companyfinancialdetails/financialdetails" runat="server" /></h2>
				
	<h3 class="box-header"><EPiServer:Translate Text="/contactcompanysearch/companydetails/companyfinancialdetails/financialdetailsaboutcompany" runat="server" /></h3>
				
	<!-- Content -->
	<div class="content">

		<asp:Table ID="AnnualAccountsTable" CssClass="left-term top-left-term" runat="server" />
	</div>
	<!-- // Content -->
				
</div>
<!-- // Section -->		