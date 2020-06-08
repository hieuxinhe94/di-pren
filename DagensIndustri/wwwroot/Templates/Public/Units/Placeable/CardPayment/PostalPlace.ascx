<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PostalPlace.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.CardPayment.PostalPlace" %>
<%@ Register TagPrefix="di" TagName="Input" Src="~/Tools/Classes/WebControls/InputWithValidation.ascx"  %>
<%--<%@ Import Namespace="DagensIndustri.Templates.Public.Pages" %>--%>


<div class="section" id="form-box-2">
					
	<div class="row">
		<div class="col">			
            <di:Input ID="CompanyInput" CssClass="text" Required="false" Name="input-company" TypeOfInput="Text" MaxValue="28" AutoComplete="true" Title="<%$ Resources: EPiServer, common.company.company %>" runat="server" />
		</div>
        <div class="col">
			<di:Input ID="CompanyNumberInput" CssClass="text" Name="companyno" MinValue="10" MaxValue="10" TypeOfInput="OrgNumber" AutoComplete="true" Title="<%$ Resources: EPiServer, common.company.companyno %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.companynumberrequired %>" runat="server" />
		</div>									
	</div>
					
	<div class="row">
		<div class="col">
            <di:Input ID="PostalplaceInput" CssClass="text" Required="true" Name="input-street" TypeOfInput="Text" MaxValue="27" AutoComplete="true" Title="<%$ Resources: EPiServer, common.address.postalplaceorbox %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.postalplaceorboxrequired %>" runat="server" />
		</div>
		<div class="col">

			<div class="small">
				<di:Input ID="PostalPlaceNoInput" CssClass="text" Name="input-number" TypeOfInput="Text" MaxValue="11" AutoComplete="true" Title="<%$ Resources: EPiServer, common.address.number %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.streetnumberrequired %>" runat="server" />
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
	</div>					
	
</div>