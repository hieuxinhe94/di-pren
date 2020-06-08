<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SubscriberDetails.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.Subscription.SubscriberDetails" %>
<%@ Register TagPrefix="di" TagName="SubscriberAddress" Src="~/Templates/Public/Units/Placeable/Subscription/SubscriberAddress.ascx" %>
<%@ Register TagPrefix="di" TagName="PostalPlace" Src="~/Templates/Public/Units/Placeable/Subscription/PostalPlace.ascx" %>
<%@ Register TagPrefix="di" TagName="PaymentMethod" Src="~/Templates/Public/Units/Placeable/Subscription/SubscriptionPaymentMethod.ascx" %>

<div class="form-nav">
  	<ul>
  		<li id="StreetListItem" class="current" runat="server">
            <a href="#form-street"><EPiServer:Translate Text="/common/address/street" runat="server" /></a>
        </li>
  		<li id="PostalPlaceListItem" runat="server">
            <a href="#form-box"><EPiServer:Translate Text="/common/address/postalplace" runat="server" /></a>
        </li>
  	</ul>
  	<p class="required"><EPiServer:Translate Text="/common/requiredinformation" runat="server" /></p>
</div>

<div class="form-box">

	<!-- Street -->
    <di:SubscriberAddress ID="SubscriberAddressControl" runat="server" />
    
    <!-- Box -->
    <di:PostalPlace ID="PostalPlaceControl" runat="server" />
	
</div>

<!-- Ways of payment -->
<di:PaymentMethod ID="PaymentMethodControl" runat="server" />
<!-- // Ways of payment -->