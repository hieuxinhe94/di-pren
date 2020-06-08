<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CompanyBusiness.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.ContactCompanySearch.CompanyBusiness" %>
<%@ Import Namespace="System.Xml" %>
<%@ Import Namespace="DIClassLib.Misc" %>

<!-- Section -->
<div class="section">

	<h2><EPiServer:Translate Text="/contactcompanysearch/companydetails/companybusiness/business" runat="server"/></h2>
				
	<h3 class="box-header"><EPiServer:Translate Text="/contactcompanysearch/companydetails/companybusiness/businessasccordingtopar" runat="server"/></h3>
				
	<!-- Content -->
	<div class="content">
		<div class="sub-section">
			<p>
                <asp:Repeater ID="SNBusinessRepeater" runat="server">
                    <ItemTemplate>
                        <span class="b"><%# ((Business)Container.DataItem).Text %>: </span><%# ((Business)Container.DataItem).Code %>
                    </ItemTemplate>
                    <SeparatorTemplate>
                        |
                    </SeparatorTemplate>
                </asp:Repeater>
            </p>
		</div>
					
		<div class="sub-section">
			<ul class="fiftyfifty-list">
				<li>
                    <span class="b"><EPiServer:Translate Text="/contactcompanysearch/companydetails/companybusiness/status" runat="server"/></span>
                    <span><%= MiscFunctions.GetXmlNodeText(WorksiteXmlDocument, "Bizbook-Show-W/BlockBaseWorksite/Status/Text") %></span>
                </li>
				<li>
                    <span class="b"><EPiServer:Translate Text="/contactcompanysearch/companydetails/companybusiness/headquarters" runat="server"/></span>
                    <span><%= GetBusinessCheckValue("RegInfo", "SateLan")%></span>
                </li>
				<li>
                    <span class="b"><EPiServer:Translate Text="/contactcompanysearch/companydetails/companybusiness/registrationdatecompany" runat="server"/></span>
                    <span><%= GetBusinessCheckValue("RegInfo", "BolReg") %></span>
                    </li>
				<li>
                    <span class="b"><EPiServer:Translate Text="/contactcompanysearch/companydetails/companybusiness/vat" runat="server"/></span>
                    <span><%= GetBusinessCheckValue("Maf", "Moms") == "1" ? Translate("/common/yes") : Translate("/common/no") %></span>
                </li>
				<li>
                    <span class="b"><EPiServer:Translate Text="/contactcompanysearch/companydetails/companybusiness/registrationdatecompanyname" runat="server"/></span>
                    <span><%= GetBusinessCheckValue("RegInfo", "FirmaReg")%></span>
                </li>
				<li>
                    <span class="b"><EPiServer:Translate Text="/contactcompanysearch/companydetails/companybusiness/employer" runat="server"/></span>
                    <span><%= GetBusinessCheckValue("Maf", "Arbetsgivare") == "1" ? Translate("/common/yes") : Translate("/common/no") %></span>
                </li>
				<li>
                    <span class="b"><EPiServer:Translate Text="/contactcompanysearch/companydetails/companybusiness/classftax" runat="server"/></span>
                    <span><%= GetBusinessCheckValue("Maf", "FSkatt") == "1" ? Translate("/common/yes") : Translate("/common/no") %></span>
                </li>
			</ul>
		</div>
					
		<div class="sub-section">
			<ul class="top-term-list three-col">
				<li>
					<span class="b"><EPiServer:Translate Text="/contactcompanysearch/companydetails/companybusiness/businessedscription" runat="server"/></span> 
                    <span><%= MiscFunctions.GetXmlNodeText(BusinessDescriptionXmlDocument, "Bizbook-PRV/Company/Description") %></span>
				</li>
				<li>
					<span class="b"><EPiServer:Translate Text="/contactcompanysearch/companydetails/companybusiness/authorizedsignatory" runat="server"/></span> 
                    <span><%= string.IsNullOrEmpty(GetBusinessCheckValue("FirmaTeck", "FirmaTeck")) ? Translate("/contactcompanysearch/companydetails/companybusiness/noinfo") : GetBusinessCheckValue("FirmaTeck", "FirmaTeck") %></span>
				</li>
				<li>
					<span class="b"><EPiServer:Translate Text="/contactcompanysearch/companydetails/companybusiness/auditorscomment" runat="server"/></span>
                    <span><%= GetCommentsForAnnualAccounting() %></span>
				</li>
			</ul>
		</div>
					
	</div>
	<!-- // Content -->
				
	<h3 class="box-header"><EPiServer:Translate Text="/contactcompanysearch/companydetails/companybusiness/board" runat="server"/></h3>
	<!-- Content -->
	<div class="content">
				
			<asp:Table ID="BoardTable" class="top-term" runat="server">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell CssClass="w300"><EPiServer:Translate Text="/contactcompanysearch/companydetails/companybusiness/name" runat="server"/></asp:TableHeaderCell>
                    <asp:TableHeaderCell CssClass="w300"><EPiServer:Translate Text="/contactcompanysearch/companydetails/companybusiness/function" runat="server"/> </asp:TableHeaderCell>
                    <asp:TableHeaderCell CssClass="w150"><EPiServer:Translate Text="/contactcompanysearch/companydetails/companybusiness/socialsecuritynumber" runat="server"/></asp:TableHeaderCell>
                    <asp:TableHeaderCell CssClass="w150"><EPiServer:Translate Text="/contactcompanysearch/companydetails/companybusiness/admittance" runat="server"/> </asp:TableHeaderCell>
                </asp:TableHeaderRow>
			</asp:Table>
	</div>

	<!-- // Content -->

	<h3 class="box-header"><EPiServer:Translate Text="/contactcompanysearch/companydetails/companybusiness/othercompanyinfo" runat="server"/></h3>
	<!-- Content -->
	<div class="content">
						
		<div class="sub-section no-border">
			<p>
                <asp:Repeater ID="OtherBusinessRepeater" runat="server">
                    <ItemTemplate>
                        <span class="b"><%# ((Business)Container.DataItem).Text %>: </span><%# ((Business)Container.DataItem).Code %>
                    </ItemTemplate>
                    <SeparatorTemplate>
                        |
                    </SeparatorTemplate>
                </asp:Repeater>
            </p>

            <p><span class="b"><EPiServer:Translate Text="/contactcompanysearch/companydetails/companybusiness/bicompanies/bicompanies" runat="server"/></span><br/></p>
            
            <asp:Repeater ID="BiCompanyRepeater" runat="server">
                <HeaderTemplate>
                    <table class="top-term">
				        <thead>
					        <tr>
                                <th class="w300"><EPiServer:Translate Text="/contactcompanysearch/companydetails/companybusiness/bicompanies/name" runat="server"/></th>
                                <th class="w300"><EPiServer:Translate Text="/contactcompanysearch/companydetails/companybusiness/bicompanies/businessdescription" runat="server"/></th>
                                <th class="w150"><EPiServer:Translate Text="/contactcompanysearch/companydetails/companybusiness/bicompanies/registrationdate" runat="server"/></th>                        
                            </tr>
				        </thead>
				        <tbody>
                </HeaderTemplate>

                <ItemTemplate>
                    <tr>
						<td><%# MiscFunctions.GetXmlNodeText((XmlNode)Container.DataItem, "CompanyName")%></td>
                        <td><%# MiscFunctions.GetXmlNodeText((XmlNode)Container.DataItem, "Description")%></td>
                        <td><%# MiscFunctions.GetXmlNodeText((XmlNode)Container.DataItem, "RegDate")%></td>
					</tr>
                </ItemTemplate>

                <FooterTemplate>
                        </tbody>
			        </table>
                </FooterTemplate>
            </asp:Repeater>
            <asp:Literal ID="HasNoBiCompaniesLiteral" Text="<%$ Resources: EPiServer, contactcompanysearch.companydetails.companybusiness.bicompanies.hasnobicompanies %>" Visible="false" runat="server"/>
		</div>
	</div>
	<!-- // Content -->				
</div>
<!-- // Section -->