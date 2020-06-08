<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PromotionalOffer.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.DiGoldMembershipFlow.PromotionalOffer" %>
<%@ Register TagPrefix="di" TagName="MainIntro" Src="~/Templates/Public/Units/Placeable/MainIntro.ascx" %>
<%@ Register TagPrefix="di" TagName="MainBody" Src="~/Templates/Public/Units/Placeable/MainBody.ascx" %>
<%@ Register TagPrefix="di" TagName="Input" Src="~/Tools/Classes/WebControls/InputWithValidation.ascx" %>


<di:MainIntro runat="server" />
<di:MainBody runat="server" />

<div class="form-nav">
  	<ul>
  		<li class="current">
            <a href="#form-street"><%= PromotionalOfferName %></a>
        </li>
  	</ul>
  	<p class="required"><EPiServer:Translate Text="/common/requiredinformation" runat="server" /></p>
</div>

<div class="form-box">
	<!-- Street -->
	<div class="section" id="form-street">

        <div class="row">
			<div class="col">
                <di:Input ID="CompanyInput" CssClass="text" Required="false" Name="input-company" TypeOfInput="Text" MaxValue="50" AutoComplete="true" Title="<%$ Resources: EPiServer, common.company.company %>" runat="server" />
			</div>
			<div class="col">
                <di:Input ID="NameInput" CssClass="text" Required="true" Name="input-name" TypeOfInput="Text" MaxValue="50" AutoComplete="true" Title="<%$ Resources: EPiServer, common.user.name %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.namerequired %>" runat="server" />
			</div>
		</div>

		<div class="row">
			<div class="col">
                <di:Input ID="StreetAddressInput" CssClass="text" Required="true" Name="input-street" TypeOfInput="Text" MaxValue="27" AutoComplete="true" Title="<%$ Resources: EPiServer, common.address.streetaddress %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.streetaddressrequired %>" runat="server" />
			</div>
			<div class="col">
				<div class="small">
                    <di:Input ID="HouseNoInput" CssClass="text" Required="false" Name="input-number" TypeOfInput="Text" MaxValue="11" AutoComplete="true" Title="<%$ Resources: EPiServer, common.address.number %>" runat="server" />
				</div>
			</div>
		</div>
					
		<div class="row">
			<div class="col">
				<div class="small">
					<di:Input ID="ZipCodeInput" CssClass="text" Required="true" Name="input-zip" TypeOfInput="ZipCode" MaxValue="5" AutoComplete="true" Title="<%$ Resources: EPiServer, common.address.zip %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.zipcoderequired %>" runat="server" />
				</div>
				<div class="medium">
                    <di:Input ID="CityInput" CssClass="text" Required="true" Name="input-city" TypeOfInput="Text" MaxValue="50" AutoComplete="true" Title="<%$ Resources: EPiServer, common.address.city %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.cityrequired %>" runat="server" />
				</div>
			</div>
			<div class="col">
                <di:Input ID="CareOfInput" CssClass="text" Required="false" Name="input-co" TypeOfInput="Text" MaxValue="26" AutoComplete="true" Title="<%$ Resources: EPiServer, common.address.careof %>" runat="server" />
			</div>
		</div>
	</div>
</div>