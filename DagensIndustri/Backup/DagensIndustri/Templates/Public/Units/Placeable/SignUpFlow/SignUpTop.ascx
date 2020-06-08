<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SignUpTop.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.SignUpTop" %>

<div id="shop-top">
    <div class="content">
			
		<!-- Page header -->
		<h1>
            <EPiServer:Property ID="Property1" PropertyName="EventName" runat="server" />
        </h1>
			
		<!-- Subtitle -->
		<p class="author">
            <%= EPiFunctions.HasValue(CurrentPage, "DateStart") != true ? "" : ((DateTime)CurrentPage["DateStart"]).ToString("dddd, d MMMM yyyy")%>
        </p>
						
		<div class="bottom">
			<asp:PlaceHolder ID="HidePaymentOptions" runat="server">
			<!-- Price and buy -->
			<div class="buy">
				<p class="price">
                    <EPiServer:Translate ID="Translate1" Text="/signup/price" runat="server" />: <EPiServer:Property ID="Property5" PropertyName="Price" runat="server" />&nbsp;<EPiServer:Translate Text="/signup/SEK" runat="server"/>/<EPiServer:Translate ID="Translate4" Text="/signup/st" runat="server"/>
                    <%--<span class="moms"><EPiServer:Translate ID="Translate4" Text="/signup/exmoms" runat="server"/></span>--%>
                </p>
                <!-- Payment options -->
				<div class="paymentoptions">
					<strong><EPiServer:Translate ID="Translate2" Text="/signup/paywith" runat="server" /></strong>
					<ul>
                        <asp:PlaceHolder ID="PlaceHolderCreditCard" runat="server">
                            <li><asp:LinkButton ID="PayWithCreditCardLinkButton" Text="<%$ Resources: EPiServer, signup.creditcard %>" OnClick="PayWithCreditCardLinkButton_Click" runat="server" /></li>
						</asp:PlaceHolder>

                        <asp:PlaceHolder ID="PlaceHolderInvoice" runat="server">
                            <li><asp:LinkButton ID="PayWithInvoiceLinkButton" Text="<%$ Resources: EPiServer, signup.invoice %>" OnClick="PayWithInvoiceLinkButton_Click" runat="server" /></li>
                        </asp:PlaceHolder>

                        <%--<li>
                            <asp:LinkButton ID="PayWithDiscountCodeLinkButton" Text="<%$ Resources: EPiServer, signup.discountcode %>" runat="server" />
                        </li>--%>
					</ul>
				</div>

                <asp:LinkButton ID="BuyLinkedButton" CssClass="btn" OnClick="BuyLinkedButton_Click" runat="server">
                    <span><EPiServer:Translate ID="Translate3" Text="/signup/apply" runat="server" /></span>
                </asp:LinkButton>

				<div class="quantity">
                    <asp:TextBox ID="QuantityTextBox" CssClass="text" Text="1" runat="server" />
                    <asp:Label ID="QuantityLabel" AssociatedControlID="QuantityTextBox" runat="server">deltagare</asp:Label>
                    <%--<EPiServer:Property ID="Property6" PropertyName="Quantity" runat="server" />--%>
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
                
	<img src='<%= CurrentPage.Property["ImageStart"] %>' alt='' width="476" height="321" class="product-image" />
		
</div>