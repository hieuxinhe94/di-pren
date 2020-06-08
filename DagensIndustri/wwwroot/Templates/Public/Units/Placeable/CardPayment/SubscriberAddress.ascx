<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SubscriberAddress.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.CardPayment.SubscriberAddress" %>
<%@ Register TagPrefix="di" TagName="Input" Src="~/Tools/Classes/WebControls/InputWithValidation.ascx"  %>


<div class="section" id="form-address-2">
					
	<div class="row">
		<div class="col">
			<di:Input ID="CompanyInput" CssClass="text" Required="false" Name="input-company" TypeOfInput="Text" MaxValue="28" AutoComplete="true" Title="<%$ Resources: EPiServer, common.company.company %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.toomanycharacters %>" runat="server" />
		</div>						
        <div class="col">
			<di:Input ID="CompanyNumberInput" CssClass="text" Name="companyno" MinValue="10" MaxValue="10" TypeOfInput="OrgNumber" AutoComplete="true" Title="<%$ Resources: EPiServer, common.company.companyno %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.companynumberrequired %>" runat="server" />
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
    
    <div class="row">		
        <div class="col">
		    <di:Input ID="CareOfInput" CssClass="text" Required="false" Name="input-co" TypeOfInput="Text" MaxValue="22" AutoComplete="true" Title="<%$ Resources: EPiServer, common.address.careof %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.toomanycharacters %>" runat="server" />
        </div>
    </div>

</div>