<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ShopProductInfo.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.Shop.ShopProductInfo" %>
<%@ Register TagPrefix="DI" TagName="Heading" Src="~/Templates/Public/Units/Placeable/Heading.ascx" %>

<div id="shop-top">
    <div class="content">
			
		<!-- Page header -->
        <h1><%= ShopProduct.Heading %></h1>
			
		<!-- Subtitle -->
		<p class="author"><%= ShopProduct.Subtitle %></p>
						
		<div class="bottom">
			<!-- Additional info -->
			<p class="specifications">
                <%= ShopProduct.ProductInformation1 %>
				<br />
                <%= ShopProduct.ProductInformation2 %>
			</p>
			
            
			<!-- Price and buy -->
			<div class="buy">
				<p class="price"><%= string.Format("{0}: {1}", Translate("/shop/price"), ShopProduct.PriceWithCurrency) %></p>
                
                <asp:PlaceHolder ID="HidePlaceHolder" Visible="false" runat="server">
                <!-- Payment options -->
				<div class="paymentoptions">
					<strong><EPiServer:Translate Text="/shop/paywith" runat="server" /></strong>
					<ul>
						<li>
                            <asp:LinkButton ID="PayWithCreditCardLinkButton" Text="<%$ Resources: EPiServer, shop.creditcard %>" OnClick="PayWithCreditCardLinkButton_Click" runat="server" />
                        </li>
						<li>
                            <asp:LinkButton ID="PayWithInvoiceLinkButton" Text="<%$ Resources: EPiServer, shop.invoice %>" OnClick="PayWithInvoiceLinkButton_Click" runat="server" />
                        </li>
					</ul>
				</div>
               

                

                <asp:PlaceHolder ID="HasAccessPlaceHolder" runat="server" Visible='<%# EPiFunctions.IsUserDIGoldMember() %>'>
                    <asp:LinkButton ID="BuyLinkedButton" CssClass="btn" OnClick="BuyLinkedButton_Click" runat="server">
                        <span><EPiServer:Translate Text="/common/buy" runat="server" /></span>
                    </asp:LinkButton>                   
                </asp:PlaceHolder>
                    
                <asp:PlaceHolder ID="NoAccessPlaceHolder" runat="server" Visible='<%# !EPiFunctions.IsUserDIGoldMember() %>'>
                    <asp:HyperLink ID="MembershipRequiredBuyHyperLink" NavigateUrl="#membership-required" CssClass="ajax btn" runat="server">
                        <span><EPiServer:Translate Text="/common/buy" runat="server"/></span>
                    </asp:HyperLink>
                </asp:PlaceHolder>

				<div class="quantity">
                    <asp:TextBox ID="QuantityTextBox" CssClass="text" Text="1" runat="server" />
                    <asp:Label ID="QuantityLabel" AssociatedControlID="QuantityTextBox" runat="server"><%= ShopProduct.Quantity %></asp:Label>
				</div> 
                </asp:PlaceHolder>

                
                <asp:LinkButton CssClass="btn" ID="AdlibrisLinkButton" OnClick="AdLibrisButton_Click" runat="server">
                    <span>Köp</span>
                </asp:LinkButton>

			</div>
			<!-- // Price and buy -->
           
		</div>
	</div>
                
	<img src="<%= ShopProduct.ImageUrl %>" alt="<%= ShopProduct.ImageDescription %>" class="product-image" />
		
</div>