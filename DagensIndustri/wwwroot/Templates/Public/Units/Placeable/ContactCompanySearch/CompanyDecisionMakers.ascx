<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CompanyDecisionMakers.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.ContactCompanySearch.CompanyDecisionMakers" %>
<%@ Import Namespace="System.Xml.XPath" %>


<!-- Company Members -->
<div id="company-members">
	<h2 class="box-header"><EPiServer:Translate Text="/contactcompanysearch/companydetails/decisionmaker/title" runat="server" /></h2>

    <asp:Repeater ID="DecisionMakerRepeater" runat="server">
        <HeaderTemplate>
            <ul>
        </HeaderTemplate>
        <ItemTemplate>
            <asp:HiddenField ID="ContactIdHiddenField" Value='<%# ((XPathNavigator)Container.DataItem).GetAttribute("dbId", "") %>' runat="server" />

            <li id="DecisionMakerListItem" class="vcard" runat="server">
                <h3>
                    <asp:LinkButton ID="DecisionMakerLinkButton" CssClass="fn" Text='<%# string.Format("{0} {1}", GetValue((XPathNavigator)Container.DataItem, "FirstName"), GetValue((XPathNavigator)Container.DataItem, "LastName")) %>' OnClick="DecisionMakerLinkButton_Click" CommandArgument='<%# ((XPathNavigator)Container.DataItem).GetAttribute("dbId", "") %>' runat="server" /> 
                    <span class="title">
                        <%# GetValue((XPathNavigator)Container.DataItem, "MainJobTitle") %>
                    </span>
                </h3>
               
			    <div class="extra-info">
				    <ul>
					    <li>
						    <span class="b"><EPiServer:Translate Text="/contactcompanysearch/companydetails/decisionmaker/position" runat="server" /></span> 
                            <asp:Label ID="MainPositionLabel" CssClass="role" runat="server" />
					    </li>
					    <li>
						    <span class="b"><EPiServer:Translate Text="/contactcompanysearch/companydetails/details/email" runat="server" /></span>
                            <span><asp:HyperLink ID="EmailHyperLink" CssClass="email" runat="server" /></span>
					    </li>
					    <li>
                            <asp:PlaceHolder ID="OtherPositionPlaceHolder" runat="server">
						        <span class="b"><EPiServer:Translate Text="/contactcompanysearch/companydetails/decisionmaker/otherposition" runat="server" /></span>
                                <asp:Repeater ID="OtherPositionsRepeater" runat="server">
                                    <ItemTemplate>
                                        <span class="role"><%# Container.DataItem %></span>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </asp:PlaceHolder>
					    </li>
				    </ul>
			    </div>
		    </li>
        </ItemTemplate>
        <FooterTemplate>
            </ul>
        </FooterTemplate>
    </asp:Repeater>
</div>
<!-- // Company Members -->