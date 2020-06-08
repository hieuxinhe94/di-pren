<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InvoiceRecipient.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.Subscription.InvoiceRecipient" %>
<%@ Register TagPrefix="di" TagName="Input" Src="~/Tools/Classes/WebControls/InputWithValidation.ascx"  %>

<div class="form-box">
  							
<!-- Street -->

<div class="section" id="form-street">
<div class="row">
	<div class="col">
        <di:Input ID="FirstNameInput" CssClass="text" Required="false" Name="firstname" TypeOfInput="Text" MaxValue="28" AutoComplete="true" Title="<%$ Resources: EPiServer, common.user.firstname %>" runat="server" />
	</div>
	<div class="col">
        <di:Input ID="LastNameInput" CssClass="text" Required="false" Name="lastname" TypeOfInput="Text" MaxValue="28" AutoComplete="true" Title="<%$ Resources: EPiServer, common.user.lastname %>" runat="server" />
	</div>						
</div>
					
<div class="divider"><hr /></div>
					
<div class="row">
	<div class="col">
        <di:Input ID="CompanyInput" CssClass="text" Required="false" Name="input-company" TypeOfInput="Text" MaxValue="28" AutoComplete="true" Title="<%$ Resources: EPiServer, common.company.company %>" runat="server" />
	</div>

	<div class="col">
        <di:Input ID="AttentionInput" CssClass="text" Required="false" Name="input-attention" TypeOfInput="Text" MaxValue="28" AutoComplete="true" Title="<%$ Resources: EPiServer, common.address.attention %>" runat="server" />
	</div>																
</div>					
					
<div class="row">
	<div class="col">
        <di:Input ID="CareOfInput" CssClass="text" Required="false" Name="input-co" TypeOfInput="Text" MaxValue="26" AutoComplete="true" Title="<%$ Resources: EPiServer, common.address.careof %>" runat="server" />
	</div>						
</div>						
					
<div class="row">
	<div class="col">
        <di:Input ID="StreetAddressInput" CssClass="text" Required="true" Name="input-street" TypeOfInput="Text" MaxValue="27" AutoComplete="true" Title="<%$ Resources: EPiServer, common.address.streetaddress %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.streetaddressrequired %>" runat="server" />
	</div>
	<div class="col">
		<div class="small">
            <di:Input ID="HouseNoInput" CssClass="text" Required="true" Name="input-number" TypeOfInput="Text" MaxValue="11" AutoComplete="true" Title="<%$ Resources: EPiServer, common.address.number %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.streetnumberrequired %>" runat="server" />
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
        <di:Input ID="PhoneDayTimeInput" CssClass="text" Required="true" Name="phone" TypeOfInput="Telephone" MinValue="7" MaxValue="20" AutoComplete="true" Title="<%$ Resources: EPiServer, common.user.phonedaytime %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.phonenumberrequired %>" runat="server" />
	</div>
</div>
					
<div class="divider"><hr /></div>	
					
<div class="row">
	<div class="col">
        <di:Input ID="CompanyNumberInput" CssClass="text" Name="companyno" MinValue="10" MaxValue="10" TypeOfInput="OrgNumber" AutoComplete="true" Title="<%$ Resources: EPiServer, common.company.companyno %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.companynumberrequired %>" runat="server" />
		<p class="description"><EPiServer:Translate Text="/common/company/formatforcompanyno" runat="server" /></p>
	</div>
</div>
</div>
<!-- // Street -->
				
</div>	
