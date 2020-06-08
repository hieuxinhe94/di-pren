<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SearchControl.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.ContactCompanySearch.SearchControl" %>
<%@ Register TagPrefix="di" TagName="Input" Src="~/Tools/Classes/WebControls/InputWithValidation.ascx" %>
<%@ Register TagPrefix="di" TagName="Paging" Src="~/Templates/Public/Units/Placeable/Paging.ascx" %>
<%@ Import Namespace="System.Xml" %>
<%@ Import Namespace="DIClassLib.Misc" %>

<div class="form-nav">
	<ul>
 		<li class="current">
            <a href="#simple-search"><EPiServer:Translate Text="/contactcompanysearch/search/searchsimple" runat="server" /></a>
        </li>
 	</ul>
</div>
			
<div class="form-box">
	<!-- Simple search -->
	<div class="section clearFix" id="simple-search">

		<div class="row">
			<div class="col medium">
                <di:Input ID="NameInput" CssClass="text medium" Name="name" TypeOfInput="Text" StripHtml="true" AutoComplete="true" Title="<%$ Resources: EPiServer, contactcompanysearch.search.name %>" runat="server" />
			</div>
			<div class="col medium">
                <di:Input ID="CompanyInput" CssClass="text small" Name="company" TypeOfInput="Text" StripHtml="true" AutoComplete="true" Title="<%$ Resources: EPiServer, contactcompanysearch.search.company %>" runat="server" />
			</div>	
			<div class="col medium">
                <di:Input ID="AddressInput" CssClass="text small" Name="address" TypeOfInput="Text" StripHtml="true" AutoComplete="true" Title="<%$ Resources: EPiServer, contactcompanysearch.search.address %>" runat="server" />					
			</div>
		</div>

	</div>
	<!-- // Simple search -->			
</div>
			
<div class="button-wrapper">
    <asp:Button ID="SearchButton" CssClass="btn search" Text="<%$ Resources: EPiServer, common.search %>" OnClick="SearchButton_Click" runat="server" />
</div>

<asp:PlaceHolder ID="SearchResultPlaceHolder" Visible="false" runat="server">
    <div id="search-results">
        <h2 class="n">
            <asp:Literal ID="SearchResultLiteral" runat="server" />
        </h2>

        <asp:Repeater ID="PersonSearchResultRepeater" Visible="false" runat="server">
            <HeaderTemplate>
                <ul>
            </HeaderTemplate>
        
            <ItemTemplate>
                <li class="vcard">
			        <h3 class="org fn">
                        <asp:HyperLink NavigateUrl="<%# GetWorksiteContactDetailUrl((XmlNode)Container.DataItem) %>" Text='<%# string.Format("{0} {1}, {2}", MiscFunctions.GetXmlNodeText((XmlNode)Container.DataItem, "FirstName"), MiscFunctions.GetXmlNodeText((XmlNode)Container.DataItem, "LastName"), MiscFunctions.GetXmlNodeText((XmlNode)Container.DataItem, "MainJobTitle")) %>' runat="server" />
                    </h3>
                
                    <h4>
                        <asp:HyperLink NavigateUrl="<%# GetWorksiteDetailUrl((XmlNode)Container.DataItem) %>" Text='<%# MiscFunctions.GetXmlNodeText((XmlNode)Container.DataItem, "WorksiteName") %>' runat="server" />
                    </h4>

			        <ul class="meta-list">

				        <li class="adr">
                            <asp:PlaceHolder ID="AddressPlaceHolder" Visible='<%# !string.IsNullOrEmpty(MiscFunctions.GetXmlNodeText((XmlNode)Container.DataItem, "Address")) %>' runat="server">
                                <%# string.Format(@"{0}, ", MiscFunctions.GetXmlNodeText((XmlNode)Container.DataItem, "Address"))%>
                            </asp:PlaceHolder>

                            <%# string.Format(@"{0} {1}", MiscFunctions.GetXmlNodeText((XmlNode)Container.DataItem, "Zipcode"), MiscFunctions.GetXmlNodeText((XmlNode)Container.DataItem, "City")).Trim()%>
                        </li>

				        <li>
                            <asp:PlaceHolder ID="PhonePlaceHolder" Visible='<%# !string.IsNullOrEmpty(MiscFunctions.GetXmlNodeText((XmlNode)Container.DataItem, "WorksitePhone")) %>' runat="server">
                                <EPiServer:Translate Text="/contactcompanysearch/search/searchresult/telephonecentral" runat="server" /> 
                                <a class="tel" href="callto:<%# MiscFunctions.GetXmlNodeText((XmlNode)Container.DataItem, "WorksitePhone") %>"><%# MiscFunctions.FormatPhoneNumber(MiscFunctions.GetXmlNodeText((XmlNode)Container.DataItem, "WorksitePhone"))%></a>
                            </asp:PlaceHolder>
                        </li>				   

				        <li class="map">
                            <asp:HyperLink CssClass="more" NavigateUrl="<%# GetMapsUrl((XmlNode)Container.DataItem) %>" Target="_blank" Text="<%$ Resources: EPiServer, contactcompanysearch.search.searchresult.showinmap %>" runat="server" />
                        </li>
			        </ul>
		        </li>
            </ItemTemplate>
        
            <FooterTemplate>
                </ul>
            </FooterTemplate>
        </asp:Repeater>

        <asp:Repeater ID="WorkSiteSearchResultRepeater" Visible="false" runat="server" >
            <HeaderTemplate>
                <ul>
            </HeaderTemplate>
        
            <ItemTemplate>
                <li class="vcard">
			        <h3 class="org fn">
                        <asp:HyperLink NavigateUrl="<%# GetWorksiteDetailUrl((XmlNode)Container.DataItem) %>" Text='<%# MiscFunctions.GetXmlNodeText((XmlNode)Container.DataItem, "Name") %>' runat="server" />
                    </h3>

			        <ul class="meta-list">
				        <li class="adr">
                            <asp:PlaceHolder ID="AddressPlaceHolder" Visible='<%# !string.IsNullOrEmpty(MiscFunctions.GetXmlNodeText((XmlNode)Container.DataItem, "Address")) %>' runat="server">
                                <%# string.Format(@"{0}, ", MiscFunctions.GetXmlNodeText((XmlNode)Container.DataItem, "Address"))%>
                            </asp:PlaceHolder>

                            <%# string.Format(@"{0} {1}", MiscFunctions.GetXmlNodeText((XmlNode)Container.DataItem, "Zipcode"), MiscFunctions.GetXmlNodeText((XmlNode)Container.DataItem, "City")).Trim()%>
                        </li>

				        <li>
                            <asp:PlaceHolder ID="PhonePlaceHolder" Visible='<%# !string.IsNullOrEmpty(MiscFunctions.GetXmlNodeText((XmlNode)Container.DataItem, "Phone")) %>' runat="server">
                                <EPiServer:Translate Text="/contactcompanysearch/search/searchresult/phone" runat="server" /> 
                                <a class="tel" href="callto:<%# MiscFunctions.GetXmlNodeText((XmlNode)Container.DataItem, "Phone") %>"><%# MiscFunctions.FormatPhoneNumber(MiscFunctions.GetXmlNodeText((XmlNode)Container.DataItem, "Phone"))%></a>                                
                            </asp:PlaceHolder>
                        </li>

				        <li class="type">
                            <asp:PlaceHolder ID="TypePlaceHolder" Visible='<%# !string.IsNullOrEmpty(MiscFunctions.GetXmlNodeText((XmlNode)Container.DataItem, "Type")) %>' runat="server">
                                <EPiServer:Translate Text="/contactcompanysearch/search/searchresult/type" runat="server" /> <span class="b"><%# MiscFunctions.GetXmlNodeText((XmlNode)Container.DataItem, "Type")%></span>
                            </asp:PlaceHolder>
                        </li>

				        <li class="map">
                            <asp:HyperLink CssClass="more" NavigateUrl="<%# GetMapsUrl((XmlNode)Container.DataItem) %>" Target="_blank" Text="<%$ Resources: EPiServer, contactcompanysearch.search.searchresult.showinmap %>" runat="server" />
                        </li>
			        </ul>
		        </li>
            </ItemTemplate>
        
            <FooterTemplate>
                </ul>
            </FooterTemplate>
        </asp:Repeater>
    
    </div>
	
    <di:Paging ID="PagingControl" runat="server" Visible="false" />
</asp:PlaceHolder>