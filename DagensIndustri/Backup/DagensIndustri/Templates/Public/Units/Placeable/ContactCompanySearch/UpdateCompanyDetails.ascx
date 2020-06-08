<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UpdateCompanyDetails.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.ContactCompanySearch.UpdateCompanyDetails" %>
<%@ Register TagPrefix="di" TagName="Input" Src="~/Tools/Classes/WebControls/InputWithValidation.ascx" %>

<div class="form-box">
	<h2 class="box-header"><EPiServer:Translate Text="/contactcompany/updatecompany/updatecompanyinfo" runat="server" /></h2>

	<div class="row three-col">
		<div class="col col1">						
		</div>
		<div class="col col2">
            <span class="b"><EPiServer:Translate Text="/contactcompany/updatecompany/currentinformation" runat="server" /></span>
		</div>
		<div class="col col3">
			<span class="b"><EPiServer:Translate Text="/contactcompany/updatecompany/changeto" runat="server" /></span>
		</div>
	</div>

	<div class="row three-col">
		<div class="col col1">
            <label onclick="<%= GetScript(CompanyNameInput.InputClientID) %>"><EPiServer:Translate Text="/contactcompany/common/companyname" runat="server" /></label>
		</div>
		<div class="col col2">
			<asp:Label ID="CompanyNameLabel" runat="server" />
		</div>
		<div class="col col3">
            <di:Input ID="CompanyNameInput" TypeOfInput="Text" CssClass="text" Name="company-name" Required="true" StripHtml="true" AutoComplete="true" DisplayMessage="<%$ Resources: EPiServer, contactcompany.common.requiredinformation %>" runat="server" />
		</div>
	</div>
				
	<div class="row three-col">
		<div class="col col1">
			<label onclick="<%= GetScript(CompanyCareOfInput.InputClientID) %>"><EPiServer:Translate Text="/contactcompany/updatecompany/careof" runat="server" /></label>
		</div>
		<div class="col col2">
			<asp:Label ID="CompanyCareOfLabel" runat="server" />
		</div>
		<div class="col col3">
			<di:Input ID="CompanyCareOfInput" TypeOfInput="Text" CssClass="text" Name="company-name-co" StripHtml="true" AutoComplete="true" runat="server" />
		</div>
	</div>
				
	<div class="row three-col">
		<div class="col col1">
			<label onclick="<%= GetScript(DeliveryAddressInput.InputClientID) %>"><EPiServer:Translate Text="/contactcompany/common/deliveryaddress" runat="server" /></label>
		</div>
		<div class="col col2">
			<asp:Label ID="DeliveryAddressLabel" runat="server" />
		</div>
		<div class="col col3">
            <di:Input ID="DeliveryAddressInput" TypeOfInput="Text" CssClass="text" Name="delivery-address" Required="true" StripHtml="true" AutoComplete="true" DisplayMessage="<%$ Resources: EPiServer, contactcompany.common.requiredinformation %>" runat="server" />
		</div>
	</div>
				
	<div class="row three-col">
		<div class="col col1">
			<label onclick="<%= GetScript(DeliveryZipCodeInput.InputClientID) %>"><EPiServer:Translate Text="/contactcompany/updatecompany/zipcode" runat="server" /></label>
		</div>
		<div class="col col2">
			<asp:Label ID="DeliveryZipCodeLabel" runat="server" />
		</div>
		<div class="col col3">
			<di:Input ID="DeliveryZipCodeInput" TypeOfInput="ZipCode" CssClass="text" Name="delivery-zip" Required="true" MaxValue="5" StripHtml="true" AutoComplete="true" DisplayMessage="<%$ Resources: EPiServer, contactcompany.common.invalidformat %>" runat="server" />
		</div>
	</div>
				
	<div class="row three-col">
		<div class="col col1">
			<label onclick="<%= GetScript(DeliveryCityInput.InputClientID) %>"><EPiServer:Translate Text="/contactcompany/common/city" runat="server" /></label>
		</div>
		<div class="col col2">
			<asp:Label ID="DeliveryCityLabel" runat="server" />
		</div>
		<div class="col col3">
			<di:Input ID="DeliveryCityInput" TypeOfInput="Text" CssClass="text" Name="delivery-city" Required="true" StripHtml="true" AutoComplete="true" DisplayMessage="<%$ Resources: EPiServer, contactcompany.common.requiredinformation %>" runat="server" />
		</div>
	</div>
				
	<div class="divider"><hr /></div>
				
	<div class="row text">
		<small><EPiServer:Translate Text="/contactcompany/updatecompany/addressdifference" runat="server" /></small>
	</div>
		
	<div class="row three-col">
		<div class="col col1">
			<label onclick="<%= GetScript(VisitorStreetAddressInput.InputClientID) %>"><EPiServer:Translate Text="/contactcompany/common/visitoraddress" runat="server" /></label>
		</div>
		<div class="col col2">
			<asp:Label ID="VisitorStreetAddressLabel" runat="server" />
		</div>
		<div class="col col3">
            <di:Input ID="VisitorStreetAddressInput" TypeOfInput="Text" CssClass="text" Name="visitor-street-address" StripHtml="true" AutoComplete="true" runat="server" />				
		</div>
	</div>
				
	<div class="row three-col">
		<div class="col col1">
			<label onclick="<%= GetScript(VisitorZipCodeInput.InputClientID) %>"><EPiServer:Translate Text="/contactcompany/updatecompany/zipcode" runat="server" /></label>
		</div>
		<div class="col col2">
            <asp:Label ID="VisitorZipCodeLabel" runat="server" />
		</div>
		<div class="col col3">
			<di:Input ID="VisitorZipCodeInput" TypeOfInput="ZipCode" CssClass="text" Name="visitor-zip" MaxValue="5" StripHtml="true" AutoComplete="true" DisplayMessage="<%$ Resources: EPiServer, contactcompany.common.invalidformat %>" runat="server" />
		</div>
	</div>
				
	<div class="row three-col">
		<div class="col col1">
			<label onclick="<%= GetScript(VisitorCityInput.InputClientID) %>"><EPiServer:Translate Text="/contactcompany/common/city" runat="server" /></label>
		</div>
		<div class="col col2">
			<asp:Label ID="VisitorCityLabel" runat="server" />
		</div>
		<div class="col col3">
			<di:Input ID="VisitorCityInput" TypeOfInput="Text" CssClass="text" Name="visitor-city" StripHtml="true" AutoComplete="true" runat="server" />
		</div>
	</div>

	<div class="row three-col">
		<div class="col col1">
			<label onclick="<%= GetScript(ContactInfoPhoneInput.InputClientID) %>"><EPiServer:Translate Text="/contactcompany/common/phonenumber" runat="server" /></label>
		</div>
		<div class="col col2">
			<asp:Label ID="ContactInfoPhoneLabel" runat="server" />
		</div>
		<div class="col col3">
            <di:Input ID="ContactInfoPhoneInput" TypeOfInput="Telephone" CssClass="text" Name="contactInfo-phone" StripHtml="true" AutoComplete="true" DisplayMessage="<%$ Resources: EPiServer, contactcompany.common.invalidformat %>" runat="server" />
		</div>
	</div>
				
	<div class="row three-col">
		<div class="col col1">
			<label onclick="<%= GetScript(ContactInfoFaxInput.InputClientID) %>"><EPiServer:Translate Text="/contactcompany/updatecompany/fax" runat="server" /></label>
		</div>
		<div class="col col2">
			<asp:Label ID="ContactInfoFaxLabel" runat="server" />
		</div>
		<div class="col col3">
            <di:Input ID="ContactInfoFaxInput" TypeOfInput="Telephone" CssClass="text" Name="contactInfo-fax" StripHtml="true" AutoComplete="true" DisplayMessage="<%$ Resources: EPiServer, contactcompany.common.invalidformat %>" runat="server" />
		</div>
	</div>
				
	<div class="row three-col">
		<div class="col col1">
			<label onclick="<%= GetScript(ContactInfoEmailInput.InputClientID) %>"><EPiServer:Translate Text="/contactcompany/common/email" runat="server" /></label>
		</div>
		<div class="col col2">
			<asp:Label ID="ContactInfoEmailLabel" runat="server" />
		</div>
		<div class="col col3">
            <di:Input ID="ContactInfoEmailInput" TypeOfInput="Email" CssClass="text" Name="contactInfo-email" StripHtml="true" AutoComplete="true" DisplayMessage="<%$ Resources: EPiServer, contactcompany.common.invalidformat %>" runat="server" />
		</div>
	</div>
				
	<div class="row three-col">
		<div class="col col1">
			<label onclick="<%= GetScript(HomePageUrlInput.InputClientID) %>"><EPiServer:Translate Text="/contactcompany/updatecompany/homepage" runat="server" /></label>
		</div>
		<div class="col col2">
			<asp:Label ID="HomepageUrlLabel" runat="server" />
		</div>
		<div class="col col3">
			<di:Input ID="HomePageUrlInput" TypeOfInput="Url" CssClass="text" Name="url" StripHtml="true" AutoComplete="true" DisplayMessage="<%$ Resources: EPiServer, contactcompany.common.invalidformat %>" runat="server" />
		</div>
	</div>
				
</div>
			
<div class="form-box">
	<h2 class="box-header"><EPiServer:Translate Text="/contactcompany/updatecompany/changecomment" runat="server" /></h2>

	<div class="row textarea">
        <di:Input ID="ChangeCommentInput" TypeOfInput="Text" CssClass="text" Name="member-changes" IsTextArea="true" StripHtml="true" Title="<%$ Resources: EPiServer, contactcompany.updatecompany.commentexplanation %>" runat="server" />
	</div>
</div>
			
<div class="form-box">
	<h2 class="box-header"><EPiServer:Translate Text="/contactcompany/common/informationprovider" runat="server" /></h2>
				
	<div class="row text">
		<small><EPiServer:Translate Text="/contactcompany/updatecompany/informationproviderinfo" runat="server" /></small>
	</div>		
    <div class="row">
	    <div class="col">
            <di:Input ID="InfoProviderFirstNameInput" TypeOfInput="Text" CssClass="text" Name="submitter-firstname" Required="true" StripHtml="true" AutoComplete="true" Title="<%$ Resources: EPiServer, contactcompany.common.firstname %>" DisplayMessage="<%$ Resources: EPiServer, contactcompany.common.requiredinformation %>" runat="server" />
	    </div>
	    <div class="col">
		    <di:Input ID="InfoProviderLastNameInput" TypeOfInput="Text" CssClass="text" Name="submitter-lastname" StripHtml="true" Required="true" AutoComplete="true" Title="<%$ Resources: EPiServer, contactcompany.common.lastname %>" DisplayMessage="<%$ Resources: EPiServer, contactcompany.common.requiredinformation %>" runat="server" />
	    </div>
    </div>
				
    <div class="row">
	    <div class="col">		        
            <di:Input ID="InfoProviderEmailInput" TypeOfInput="Email" CssClass="text" Name="submitter-email" StripHtml="true" Required="true" AutoComplete="true" Title="<%$ Resources: EPiServer, contactcompany.common.email%>" DisplayMessage="<%$ Resources: EPiServer, contactcompany.common.invalidformat %>" runat="server" />
	    </div>
	    <div class="col">
		    <di:Input ID="InfoProviderPhoneInput" TypeOfInput="Telephone" CssClass="text" Name="submitter-phone" StripHtml="true" Required="true" AutoComplete="true" Title="<%$ Resources: EPiServer, contactcompany.common.phonenumber %>" DisplayMessage="<%$ Resources: EPiServer, contactcompany.common.invalidformat %>" runat="server" />
	    </div>
    </div>
</div>

<div class="button-wrapper">
    <asp:Button ID="SendButton" Text="<%$ Resources: EPiServer, contactcompany.common.send %>" OnClick="SendButton_Click" runat="server" />
</div>