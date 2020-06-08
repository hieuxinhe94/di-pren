<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PlaceOrder.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.SignUpFlow.PlaceOrder" %>
			
<!-- Page primary content goes here -->
<div class="form-box">
	<div class="product">
		
        <%--<img src="/templates/public/images/placeholders/gasell-ticket-small.png" alt="gasell-ticket" />--%>
        <asp:Image ID="ImageConfirmTicket" runat="server" />

		<p class="description">
            <asp:Literal ID="DesciptionLiteral" runat="server" />
        </p>
		<p class="price">
            <asp:Literal ID="PriceLiteral" runat="server" />
        </p>
	</div>
</div>		
<!-- // Page primary content goes here -->


