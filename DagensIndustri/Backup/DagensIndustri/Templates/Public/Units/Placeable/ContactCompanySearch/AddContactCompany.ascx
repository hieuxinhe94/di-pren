<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddContactCompany.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.ContactCompanySearch.AddContactCompany" %>
<%@ Register TagPrefix="di" TagName="Input" Src="~/Tools/Classes/WebControls/InputWithValidation.ascx" %>

<div class="form-box">				
    <h2 class="box-header"><EPiServer:Translate Text="/contactcompany/addcompany/add" runat="server" /></h2>

    <div class="row">
	    <div class="col">
            <di:Input ID="CompanyNameInput" TypeOfInput="Text" CssClass="text" Name="orgname" Required="true" StripHtml="true" AutoComplete="true" Title="<%$ Resources: EPiServer, contactcompany.common.companyname %>" DisplayMessage="<%$ Resources: EPiServer, contactcompany.common.requiredinformation %>" runat="server" />
	    </div>
	    <div class="col">
            <di:Input ID="OrganizationNumberInput" TypeOfInput="OrgNumber" CssClass="text" Name="orgnr" Required="true" StripHtml="true" AutoComplete="true" Title="<%$ Resources: EPiServer, contactcompany.addcompany.organizationnumber %>" DisplayMessage="<%$ Resources: EPiServer, contactcompany.common.invalidformat %>" runat="server" />
	    </div>
    </div>
				
    <div class="row">
	    <div class="col">
            <di:Input ID="DeliveryAddressInput" TypeOfInput="Text" CssClass="text" Name="delivery-address" Required="true" StripHtml="true" AutoComplete="true" Title="<%$ Resources: EPiServer, contactcompany.common.deliveryaddress %>" DisplayMessage="<%$ Resources: EPiServer, contactcompany.common.requiredinformation %>" runat="server" />
	    </div>
	    <div class="col">
		    <div class="small">
                <di:Input ID="DeliveryZipCodeInput" TypeOfInput="ZipCode" CssClass="text" Name="delivery-zip" Required="true" MaxValue="5" StripHtml="true" AutoComplete="true" Title="<%$ Resources: EPiServer, contactcompany.addcompany.zipcode %>" DisplayMessage="<%$ Resources: EPiServer, contactcompany.common.invalidformat %>" runat="server" />
		    </div>
		    <div class="medium">
                <di:Input ID="DeliveryCityInput" TypeOfInput="Text" CssClass="text" Name="delivery-city" Required="true" StripHtml="true" AutoComplete="true" Title="<%$ Resources: EPiServer, contactcompany.common.city %>" DisplayMessage="<%$ Resources: EPiServer, contactcompany.common.requiredinformation %>" runat="server" />
		    </div>
	    </div>
    </div>
				
    <div class="divider"><hr /></div>
				
    <div class="row text">
	    <h3>
            <EPiServer:Translate Text="/contactcompany/common/visitoraddress" runat="server" /> <span><EPiServer:Translate Text="/contactcompany/addcompany/notsameasdeliveryaddress" runat="server" /></span>
        </h3>
    </div>
				
    <div class="row">
	    <div class="col">
            <di:Input ID="VisitorStreetAddressInput" TypeOfInput="Text" CssClass="text" Name="visitor-street-address" StripHtml="true" AutoComplete="true" Title="<%$ Resources: EPiServer, contactcompany.addcompany.streetaddress %>" runat="server" />
	    </div>
	    <div class="col">
		    <div class="small">
			    <di:Input ID="VisitorZipCodeInput" TypeOfInput="ZipCode" CssClass="text" Name="visitor-zip" MaxValue="5" StripHtml="true" AutoComplete="true" Title="<%$ Resources: EPiServer, contactcompany.addcompany.zipcode %>" DisplayMessage="<%$ Resources: EPiServer, contactcompany.common.invalidformat %>" runat="server" />
		    </div>	
		    <div class="medium">
                <di:Input ID="VisitorCityInput" TypeOfInput="Text" CssClass="text" Name="visitor-city" StripHtml="true" AutoComplete="true" Title="<%$ Resources: EPiServer, contactcompany.common.city %>" runat="server" />
		    </div>
	    </div>
    </div>

    <div class="divider"><hr /></div>
		
    <div class="row text">
	    <h3><EPiServer:Translate Text="/contactcompany/addcompany/contactinformation" runat="server" /></h3>
    </div>
				
    <div class="row">
	    <div class="col">
            <di:Input ID="ContactInfoPhoneInput" TypeOfInput="Telephone" CssClass="text" Name="contactInfo-phone" StripHtml="true" AutoComplete="true" Title="<%$ Resources: EPiServer, contactcompany.common.phonenumber %>" DisplayMessage="<%$ Resources: EPiServer, contactcompany.common.invalidformat %>" runat="server" />
	    </div>

	    <div class="col">
            <di:Input ID="ContactInfoFaxInput" TypeOfInput="Telephone" CssClass="text" Name="contactInfo-fax" StripHtml="true" AutoComplete="true" Title="<%$ Resources: EPiServer, contactcompany.addcompany.fax %>" DisplayMessage="<%$ Resources: EPiServer, contactcompany.common.invalidformat %>" runat="server" />
	    </div>
    </div>
				
    <div class="row">
	    <div class="col">
            <di:Input ID="ContactInfoEmailInput" TypeOfInput="Email" CssClass="text" Name="contactInfo-email" StripHtml="true" AutoComplete="true" Title="<%$ Resources: EPiServer, contactcompany.common.email%>" DisplayMessage="<%$ Resources: EPiServer, contactcompany.common.invalidformat %>" runat="server" />
	    </div>
    </div>				
</div>
			
<div class="form-box">	
    <h2 class="box-header"><EPiServer:Translate Text="/contactcompany/addcompany/addupdatedecisionmaker" runat="server" /></h2>
				
    <div class="row">
	    <div class="col">
            <di:Input ID="DecisionMakerFirstNameInput" TypeOfInput="Text" CssClass="text" Name="member-firstname" StripHtml="true" AutoComplete="true" Title="<%$ Resources: EPiServer, contactcompany.common.firstname %>" runat="server" />
	    </div>
	    <div class="col">
            <di:Input ID="DecisionMakerLastNameInput" TypeOfInput="Text" CssClass="text" Name="member-lastname" StripHtml="true" AutoComplete="true" Title="<%$ Resources: EPiServer, contactcompany.common.lastname %>" runat="server" />
	    </div>	
				
    </div>
    <div class="row">
	    <div class="col">
		    <di:Input ID="DecisionMakerPositionsInput" TypeOfInput="Text" CssClass="text" Name="member-role" StripHtml="true" AutoComplete="true" Title="<%$ Resources: EPiServer, contactcompany.addcompany.positions %>" runat="server" />
	    </div>
    </div>
				
    <div class="row">
	    <div class="col">
            <di:Input ID="DecisionMakerEmailInput" TypeOfInput="Email" CssClass="text" Name="member-email" StripHtml="true" AutoComplete="true" Title="<%$ Resources: EPiServer, contactcompany.common.email %>" runat="server" />
	    </div>
	    <div class="col">
		    <di:Input ID="DecisionMakerPhoneInput" TypeOfInput="Telephone" CssClass="text" Name="member-phone" StripHtml="true" AutoComplete="true" Title="<%$ Resources: EPiServer, contactcompany.common.phonenumber %>" runat="server" />
	    </div>
    </div>

    <div class="row textarea">
	    <di:Input ID="DecisionMakerChangesInput" TypeOfInput="Text" CssClass="text" Name="member-changes" IsTextArea="true" StripHtml="true" Title="<%$ Resources: EPiServer, contactcompany.addcompany.changes %>" runat="server" />					
    </div>
</div>
			
<div class="form-box">
				
    <h2 class="box-header"><EPiServer:Translate Text="/contactcompany/common/informationprovider" runat="server" /></h2>
        								
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