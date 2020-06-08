<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SubscriberAddress.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.Subscription.SubscriberAddress" %>
<%@ Register TagPrefix="di" TagName="Input" Src="~/Tools/Classes/WebControls/InputWithValidation.ascx"  %>
<%@ Register TagPrefix="di" TagName="DiGold" Src="~/Templates/Public/Units/Placeable/DiGoldMembershipFlow/DiGoldPromotionalOfferAcceptance.ascx"  %>

<div class="section" id="form-street">
	<div class="row">
		<div class="col">
            <di:Input ID="FirstNameInput" CssClass="text" Required="true" Name="firstname" TypeOfInput="Text" MaxValue="20" AutoComplete="true" Title="<%$ Resources: EPiServer, common.user.firstname %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.firstnamerequired %>" runat="server" />
		</div>
		<div class="col">
			<di:Input ID="LastNameInput" CssClass="text" Required="true" Name="lastname" TypeOfInput="Text" MaxValue="20" AutoComplete="true" Title="<%$ Resources: EPiServer, common.user.lastname %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.lastnamerequired %>" runat="server" />
		</div>						
	</div>

	<div class="divider"><hr /></div>
					
	<div class="row">
		<div class="col">
			<di:Input ID="CareOfInput" CssClass="text" Required="false" Name="input-co" TypeOfInput="Text" MaxValue="22" AutoComplete="true" Title="<%$ Resources: EPiServer, common.address.careof %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.toomanycharacters %>" runat="server" />
		</div>
		<div class="col">
			<di:Input ID="CompanyInput" CssClass="text" Required="false" Name="input-company" TypeOfInput="Text" MaxValue="28" AutoComplete="true" Title="<%$ Resources: EPiServer, common.company.company %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.toomanycharacters %>" runat="server" />
		</div>						
	</div>						
					
	<div class="row">

		<div class="col">
            <di:Input ID="StreetAddressInput" CssClass="text" Required="true" Name="input-street" TypeOfInput="Text" MaxValue="27" AutoComplete="true" Title="<%$ Resources: EPiServer, common.address.streetaddress %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.streetaddressrequired %>" runat="server" />
		</div>
		<div class="col">
			<div class="small">
				<di:Input ID="HouseNoInput" CssClass="text" Name="input-number" TypeOfInput="Text" MaxValue="11" AutoComplete="true" Title="<%$ Resources: EPiServer, common.address.number %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.streetnumberrequired %>" runat="server" />
			</div>
			<div class="small">
				<di:Input ID="StairCaseInput" CssClass="text" Required="false" Name="input-staircase" TypeOfInput="Text" MaxValue="10" AutoComplete="true" Title="<%$ Resources: EPiServer, common.address.staircase %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.toomanycharacters %>" runat="server" />
			</div>
		</div>
	</div>
					
	<div class="row">
		<div class="col">
			<div class="small">
                <di:Input ID="StairsInput" CssClass="text" Required="false" Name="input-stairs" TypeOfInput="Text" MaxValue="3" AutoComplete="true" Title="<%$ Resources: EPiServer, common.address.stairs %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.toomanycharacters %>" runat="server" />				
			</div>
			<div class="small">
                <di:Input ID="AparmentNoInput" CssClass="text" Required="false" Name="input-appartment" TypeOfInput="Text" MaxValue="6" AutoComplete="true" Title="<%$ Resources: EPiServer, common.address.apartmentnumber %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.toomanycharacters %>" runat="server" />								
			</div>
		</div>

		<div class="col">
			<div class="small">
                <di:Input ID="ZipCodeInput" CssClass="text" Required="true" Name="input-zip" TypeOfInput="ZipCode" MaxValue="5" AutoComplete="true" Title="<%$ Resources: EPiServer, common.address.zip %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.zipcoderequired %>" runat="server" />
			</div>
			<div class="medium">
				<di:Input ID="CityInput" CssClass="text" Required="true" Name="input-city" TypeOfInput="Text" MaxValue="50" AutoComplete="true" Title="<%$ Resources: EPiServer, common.address.city %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.cityrequired %>" runat="server" />
			</div>
		</div>
	</div>
    		
	<div class="divider"><hr /></div>

	<div class="row">
		<div class="col">
			<di:Input ID="MobilePhoneInput" CssClass="text" Required="true" Name="mobile" TypeOfInput="Telephone" MinValue="7" MaxValue="20" AutoComplete="true" Title="<%$ Resources: EPiServer, common.user.mobiletelephone %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.mobilephonenumberrequired %>" runat="server" />
		</div>

		<div class="col">
            <di:Input ID="EmailInput" CssClass="text" Required="true" Name="email" TypeOfInput="Email" MinValue="6" AutoComplete="true" Title="<%$ Resources: EPiServer, common.user.personalemailaddress %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.emailrequired %>" runat="server" />
			<p class="description"><EPiServer:Translate Text="/subscription/subscriberdetails/emailforpasswordsent" runat="server" /></p>
		</div>
	</div>

	<div class="divider"><hr /></div>

	<div class="row">
		<div class="col">
            <di:Input ID="SocialSecurityNoInput" CssClass="text" Name="socialsecrityno" MinValue="8" MaxValue="8" TypeOfInput="SocialSecurityNumber" AutoComplete="true" Title="<%$ Resources: EPiServer, common.user.socialsecuritynumberformat %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.socialsecuritynumberrequired %>" runat="server" />
		</div>
		<div class="col">
			<di:Input ID="CompanyNumberInput" CssClass="text" Name="companyno" MinValue="10" MaxValue="10" TypeOfInput="OrgNumber" AutoComplete="true" Title="<%$ Resources: EPiServer, common.company.companyno %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.companynumberrequired %>" runat="server" />
		</div>
	</div>
    <div class="row">
        <div class="col">
            <div class="medium">
				<DI:Input ID="SubStartDate" CssClass="text date" Required="false" StripHtml="true" AutoComplete="true" Name="date" TypeOfInput="Date" MinValue="<%#MinSubstartDate%>" Title="Välj startdatum"  runat="server" Text="<%#MinSubstartDate %>" />
			</div>
		</div>
    </div>

	<di:DiGold ID="DiGoldControl" ShowPromotionalOffer="true" runat="server" />
</div>