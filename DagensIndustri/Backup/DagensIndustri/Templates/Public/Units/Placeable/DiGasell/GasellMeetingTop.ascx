<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GasellMeetingTop.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.GasellMeetingTop" %>

<div id="shop-top">
    <div class="content">
			
		<!-- Page header -->
		<h1>
            <EPiServer:Property ID="Property1" PropertyName="GasellCity" runat="server" />
        </h1>
			
		<!-- Subtitle -->
		<p class="author">
            <%= EPiFunctions.HasValue(CurrentPage, "Date") != true ? "" : ((DateTime)CurrentPage["Date"]).ToString("dddd, d MMMM yyyy")%>
        </p>
						
		<div class="bottom">
			<asp:PlaceHolder ID="HidePaymentOptions" runat="server">
			<!-- Price and buy -->
			<div class="buy">
				<p class="price">
                    <EPiServer:Translate ID="Translate1" Text="/gasell/price" runat="server" />: <EPiServer:Property ID="Property5" PropertyName="Price" runat="server" /> <EPiServer:Translate Text="/gasell/SEK" runat="server"/>
                    <span class="moms"><EPiServer:Translate ID="Translate4" Text="/gasell/exmoms" runat="server"/></span>
                </p>
                <!-- Payment options -->
				<div class="paymentoptions">
					<strong><EPiServer:Translate ID="Translate2" Text="/gasell/paywith" runat="server" /></strong>
					<ul>
						<li>
                            <asp:LinkButton ID="PayWithCreditCardLinkButton" Text="<%$ Resources: EPiServer, gasell.creditcard %>" OnClick="PayWithCreditCardLinkButton_Click" runat="server" />
                        </li>
						<li>
                            <asp:LinkButton ID="PayWithInvoiceLinkButton" Text="<%$ Resources: EPiServer, gasell.invoice %>" OnClick="PayWithInvoiceLinkButton_Click" runat="server" />
                        </li>
                        <li>
                            <asp:LinkButton ID="PayWithDiscountCodeLinkButton" Text="<%$ Resources: EPiServer, gasell.discountcode %>" OnClick="PayWithDiscountCodeLinkButton_Click" runat="server" />
                        </li>
					</ul>
				</div>

                <asp:LinkButton ID="BuyLinkedButton" CssClass="btn" OnClick="BuyLinkedButton_Click" runat="server">
                    <span><EPiServer:Translate ID="Translate3" Text="/gasell/apply" runat="server" /></span>
                </asp:LinkButton>

				<div class="quantity">
                    <asp:TextBox ID="QuantityTextBox" CssClass="text" Text="1" runat="server" />
                    <asp:Label ID="QuantityLabel" AssociatedControlID="QuantityTextBox" runat="server"><EPiServer:Property ID="Property6" PropertyName="Quantity" runat="server" /></asp:Label>
				</div>
			</div>
			<!-- // Price and buy -->
            </asp:PlaceHolder>
            <asp:PlaceHolder ID="FullBookedPlaceHolder" Visible="false" runat="server">
                <div class="buy">
				    <p class="price">
                        <EPiServer:Property PropertyName="FullBookedText" DisplayMissingMessage="false" runat="server" />
                    </p>
                </div>
            </asp:PlaceHolder>
		</div>
	</div>
                
	<img src='<%= CurrentPage.Property["Image"] %>' alt='<%= CurrentPage.Property["ImageDescription"] %>' width="476" height="321" class="product-image" />
		
</div>