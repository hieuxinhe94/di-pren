<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CompanyWorksiteDetails.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.ContactCompanySearch.CompanyWorksiteDetails" %>
<%@ Import Namespace="DIClassLib.Misc" %>

<!-- Company Main Details -->
<div id="company-main-info">
			
	<div class="vcard">
		<h1 class="fn org"><%= MiscFunctions.GetXmlNodeText(WorksiteXmlDocument, "Bizbook-Show-W/BlockBaseWorksite/Name")%></h1>

        <asp:PlaceHolder Visible='<%# HasCommunicationDetails() %>' runat="server">
		    <ul class="fiftyfifty-list border-bottom">
			    <li>
                    <EPiServer:Translate Text="/contactcompanysearch/companydetails/details/phone" runat="server"/> <span class="tel"> <%= MiscFunctions.FormatPhoneNumber(MiscFunctions.GetXmlNodeText(WorksiteXmlDocument, "Bizbook-Show-W/BlockBaseCommunication/Phone"))%></span>
                </li>
			    <li>
                    <EPiServer:Translate Text="/contactcompanysearch/companydetails/details/email" runat="server"/> <span><a class="email" href='<%=string.Format("mailto:{0}", MiscFunctions.GetXmlNodeText(WorksiteXmlDocument, "Bizbook-Show-W/BlockPlusCommunication/Email")) %>'><%= MiscFunctions.GetXmlNodeText(WorksiteXmlDocument, "Bizbook-Show-W/BlockPlusCommunication/Email")%></a></span>
                </li>
			    <li>
                    <EPiServer:Translate Text="/contactcompanysearch/companydetails/details/fax" runat="server"/> <span><%= MiscFunctions.FormatPhoneNumber(MiscFunctions.GetXmlNodeText(WorksiteXmlDocument, "Bizbook-Show-W/BlockBaseCommunication/Fax"))%></span>
                </li>
			    <li>
                    <EPiServer:Translate Text="/contactcompanysearch/companydetails/details/webaddress" runat="server"/> <span><a href='<%= FormatURL(WorksiteXmlDocument, "Bizbook-Show-W/BlockPlusCommunication/WWW") %>' class="url" target="_blank" rel="external"><%= MiscFunctions.GetXmlNodeText(WorksiteXmlDocument, "Bizbook-Show-W/BlockPlusCommunication/WWW")%></a></span>
                </li>
		    </ul>
        </asp:PlaceHolder>
					
		<div class="adr border-bottom">
			<div class="left w50">
				<h3><EPiServer:Translate Text="/contactcompanysearch/companydetails/details/visitaddress" runat="server"/></h3>
				<p>
					<asp:PlaceHolder Visible='<%# !string.IsNullOrEmpty(MiscFunctions.GetXmlNodeText(WorksiteXmlDocument, "Bizbook-Show-W/BlockBaseCommunication/VisitAddress/Address")) %>' runat="server">
                        <span class="street-address"><%= MiscFunctions.GetXmlNodeText(WorksiteXmlDocument, "Bizbook-Show-W/BlockBaseCommunication/VisitAddress/Address")%></span><br/>
                    </asp:PlaceHolder>
					<span class="postal-code"><%= MiscFunctions.GetXmlNodeText(WorksiteXmlDocument, "Bizbook-Show-W/BlockBaseCommunication/VisitAddress/Zipcode")%></span> <span class="locality uc"><%= MiscFunctions.GetXmlNodeText(WorksiteXmlDocument, "Bizbook-Show-W/BlockBaseCommunication/VisitAddress/City")%></span><br/>

                    <asp:HyperLink ID="ShowInMapHyperLink" CssClass="more" NavigateUrl="<%# GetMapsUrl() %>" Target="_blank" Text="<%$ Resources: EPiServer, contactcompanysearch.companydetails.details.showinmap %>" runat="server" />
				</p>
				<div>
					<ul>
					<li><span class="b"><EPiServer:Translate Text="/contactcompanysearch/companydetails/details/municipality" runat="server"/></span> <span><%= MiscFunctions.GetXmlNodeText(WorksiteXmlDocument, "Bizbook-Show-W/BlockCRMGeography/Municipality/Text") %></span></li>
					<li><span class="b"><EPiServer:Translate Text="/contactcompanysearch/companydetails/details/county" runat="server"/></span> <span><%= MiscFunctions.GetXmlNodeText(WorksiteXmlDocument, "Bizbook-Show-W/BlockCRMGeography/County/Text")%></span></li>
					<li><span class="b"><EPiServer:Translate Text="/contactcompanysearch/companydetails/details/region" runat="server"/></span> <span><%= MiscFunctions.GetXmlNodeText(WorksiteXmlDocument, "Bizbook-Show-W/BlockCRMGeography/WorkforceRegion/Text")%></span></li>
					</ul>
				</div>
			</div>
			<div class="left">
				<h3><EPiServer:Translate Text="/contactcompanysearch/companydetails/details/postaladdress" runat="server"/></h3>
                <p>
                    <asp:PlaceHolder Visible='<%# !string.IsNullOrEmpty(MiscFunctions.GetXmlNodeText(WorksiteXmlDocument, "Bizbook-Show-W/BlockBaseWorksite/PostalAddress/Address")) %>' runat="server">
                        <span class="street-address"><%= MiscFunctions.GetXmlNodeText(WorksiteXmlDocument, "Bizbook-Show-W/BlockBaseWorksite/PostalAddress/Address")%></span><br/>
                    </asp:PlaceHolder>

					<%= MiscFunctions.GetXmlNodeText(WorksiteXmlDocument, "Bizbook-Show-W/BlockBaseWorksite/PostalAddress/Zipcode")%> <%= MiscFunctions.GetXmlNodeText(WorksiteXmlDocument, "Bizbook-Show-W/BlockBaseWorksite/PostalAddress/City")%><br />
				</p>				
			</div>
		</div>
	</div>
				

	<div class="border-bottom">
		<h3><EPiServer:Translate Text="/contactcompanysearch/companydetails/organisation/organisation" runat="server"/></h3>
		<table class="no-stripes">
			<tr><td><EPiServer:Translate Text="/contactcompanysearch/companydetails/organisation/worksitetype" runat="server"/></td><td><%= MiscFunctions.GetXmlNodeText(WorksiteXmlDocument, "Bizbook-Show-W/BlockCRMOrganization/WorksiteType/Text")%></td></tr>
			<tr><td><EPiServer:Translate Text="/contactcompanysearch/companydetails/organisation/organisationid" runat="server"/></td><td><%= MiscFunctions.GetXmlAttributeText(WorksiteXmlDocument, "Bizbook-Show-W/BlockCRMOrganization", "dbId")%></td></tr>
			<tr><td><EPiServer:Translate Text="/contactcompanysearch/companydetails/organisation/officialworksitenumber" runat="server"/></td><td><%= MiscFunctions.GetXmlNodeText(WorksiteXmlDocument, "Bizbook-Show-W/BlockCRMOrganization/OfficialWorksiteNumber")%></td></tr>
			<tr><td><EPiServer:Translate Text="/contactcompanysearch/companydetails/organisation/numberofemployees" runat="server"/></td><td><%= MiscFunctions.GetXmlNodeText(WorksiteXmlDocument, "Bizbook-Show-W/BlockCRMSize/NumberOfEmployees/Text")%></td></tr>
		</table>
	</div>
				
	<div class="">

		<h3><EPiServer:Translate Text="/contactcompanysearch/companydetails/company/company" runat="server"/></h3>
		<table class="no-stripes">
			<tr><td><EPiServer:Translate Text="/contactcompanysearch/companydetails/company/legalname" runat="server"/></td><td><%= MiscFunctions.GetXmlNodeText(WorksiteXmlDocument, "Bizbook-Show-W/BlockCRMOrganization/LegalName")%></td></tr>
			<tr><td><EPiServer:Translate Text="/contactcompanysearch/companydetails/company/legalform" runat="server"/></td><td><%= MiscFunctions.GetXmlNodeText(WorksiteXmlDocument, "Bizbook-Show-W/BlockCRMOrganization/LegalForm/Text")%></td></tr>
			<tr><td><EPiServer:Translate Text="/contactcompanysearch/companydetails/company/companynumber" runat="server"/></td><td><%= GetCompanyNumber() %></td></tr>
			<tr><td><EPiServer:Translate Text="/contactcompanysearch/companydetails/company/numberofemployees" runat="server"/></td><td><%= MiscFunctions.GetXmlNodeText(WorksiteXmlDocument, "Bizbook-Show-W/BlockCRMSize/CompanyNumberOfEmployees/Text")%></td></tr>
			<tr><td><EPiServer:Translate Text="/contactcompanysearch/companydetails/company/numberofofficeemployees" runat="server"/></td><td><%= MiscFunctions.GetXmlNodeText(WorksiteXmlDocument, "Bizbook-Show-W/BlockCRMSize/NumberOfOfficeEmployees/Text")%></td></tr>
			<tr><td><EPiServer:Translate Text="/contactcompanysearch/companydetails/company/sharecapital" runat="server"/></td><td><%= MiscFunctions.GetXmlNodeText(WorksiteXmlDocument, "Bizbook-Show-W/BlockCRMEconomy/ShareCapital/Value")%></td></tr>
			<tr><td><EPiServer:Translate Text="/contactcompanysearch/companydetails/company/turnover" runat="server"/></td><td><%= MiscFunctions.GetXmlNodeText(WorksiteXmlDocument, "Bizbook-Show-W/BlockCRMEconomy/Turnover/Value")%></td></tr>
			<tr><td><EPiServer:Translate Text="/contactcompanysearch/companydetails/company/numberofworksites" runat="server"/></td><td><%= MiscFunctions.GetXmlNodeText(WorksiteXmlDocument, "Bizbook-Show-W/BlockCRMSize/CompanyNumberOfWorksites")%></td></tr>
		</table>
	</div>
				
</div>
<!-- // Company Main Details -->