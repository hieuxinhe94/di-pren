<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ShopPlaceOrder.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.ShopFlow.ShopPlaceOrder" %>

<!-- Page primary content goes here -->
<div class="form-box">
	<div class="product">
		<img src="/templates/public/images/placeholders/gasell-ticket-small.png" alt="gasell-ticket" />
		<p class="description">
            <asp:Literal ID="DesciptionLiteral" runat="server" />
        </p>
		<p class="price">
            <asp:Literal ID="PriceLiteral" runat="server" />
        </p>
	</div>
</div>		
<!-- // Page primary content goes here -->