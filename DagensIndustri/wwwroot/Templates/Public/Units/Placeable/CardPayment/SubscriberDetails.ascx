<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SubscriberDetails.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.CardPayment.SubscriberDetails" %>
<%@ Register TagPrefix="di" TagName="SubscriberAddress" Src="~/Templates/Public/Units/Placeable/CardPayment/SubscriberAddress.ascx" %>
<%@ Register TagPrefix="di" TagName="PostalPlace" Src="~/Templates/Public/Units/Placeable/CardPayment/PostalPlace.ascx" %>

<script language="javascript" type="text/javascript">
    function jsShowStreet(bo) {

        document.getElementById("form-address-2").style.display = 'none';
        document.getElementById("form-box-2").style.display = 'none';

        if (bo == 'true')
            document.getElementById("form-address-2").style.display = 'block';
        else
            document.getElementById("form-box-2").style.display = 'block';

        return false;

    }
</script>


<div class="form-nav">
  	<ul>
  		<li id="StreetListItem" class="current" runat="server">
            <%--<a href="#form-street"><EPiServer:Translate Text="/common/address/street" runat="server" /></a>--%>
            <a onclick="jsShowStreet('true')" href="javascript:void(0)"><EPiServer:Translate ID="Translate1" Text="/common/address/street" runat="server" /></a>
        </li>
  		<li id="PostalPlaceListItem" runat="server">
            <%--<a href="#form-box"><EPiServer:Translate Text="/common/address/postalplace" runat="server" /></a>--%>
            <a onclick="jsShowStreet('false')" href="javascript:void(0)"><EPiServer:Translate ID="Translate2" Text="/common/address/postalplace" runat="server" /></a>
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

