﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CampaignStreetForm.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.DiCampaign.CampaignStreetForm" %>
<%@ Register TagPrefix="DI" TagName="Input" Src="~/Tools/Classes/WebControls/InputWithValidation.ascx" %>
<%@ Register src="PayMethodsForm.ascx" tagname="PayMethodsForm" tagprefix="uc1" %>





<!-- Street -->
<div class="section" id="form-street">
	<div class="row">
		<div class="col">
            <DI:Input ID="FirstNameInput" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" TypeOfInput="Text" Name="firstname" Title='<%# Translate("/dicampaign/forms/firstname") %>' DisplayMessage='<%# Translate("/dicampaign/forms/firstname.message") %>' runat="server" />
		</div>
		<div class="col">
            <DI:Input ID="LastNameInput" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" TypeOfInput="Text" Name="lastname" Title='<%# Translate("/dicampaign/forms/lastname") %>' DisplayMessage='<%# Translate("/dicampaign/forms/lastname.message") %>' runat="server" />
		</div>						
	</div>
					
	<div class="divider"><hr /></div>
					
	<div class="row">
		<div class="col">
            <DI:Input ID="CompanyInput" CssClass="text" StripHtml="true" AutoComplete="true" TypeOfInput="Text" Name="comapny" Title='<%# Translate("/dicampaign/forms/company") %>' runat="server" />
        </div>
		<div class="col">
            <%--<asp:PlaceHolder ID="PlaceHolder2" Visible='<%#!IsDigitalCampaign%>' runat="server">--%>
            <div id="divAddress1">
                <DI:Input ID="CareOfInput" CssClass="text" StripHtml="true" AutoComplete="true" TypeOfInput="Text" Name="co" Title='<%# Translate("/dicampaign/forms/careof") %>' runat="server" />
            </div>
		</div>						
	</div>						

	
    <div id="divAddress2">
	    <div class="row">
		    <div class="col">
                <DI:Input ID="StreetInput" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" TypeOfInput="Text" Name="street" Title='<%# Translate("/dicampaign/forms/street") %>' DisplayMessage='<%# Translate("/dicampaign/forms/street.number") %>' runat="server" />
		    </div>
		    <div class="col">
			    <div class="small">
                    <DI:Input ID="StreetNumberInput" CssClass="text" StripHtml="true" AutoComplete="true" TypeOfInput="Text" Name="number" Title='<%# Translate("/dicampaign/forms/streetnumber") %>'  DisplayMessage='<%# Translate("/dicampaign/forms/streetnumber.message") %>' runat="server" />
			    </div>
			    <div class="small">
                    <DI:Input ID="DoorInput" CssClass="text" StripHtml="true" AutoComplete="true" TypeOfInput="Text" Name="door" Title='<%# Translate("/dicampaign/forms/entrance") %>' runat="server" />
			    </div>							
		    </div>						
	    </div>	
					
	    <div class="row">
		    <div class="col">
			    <div class="small">
                    <DI:Input ID="StairsInput" CssClass="text" StripHtml="true" AutoComplete="true" Name="stairs" Title='<%# Translate("/dicampaign/forms/floor") %>' runat="server" />
			    </div>
			    <div class="small">
                    <DI:Input ID="AppartmentInput" CssClass="text" StripHtml="true" AutoComplete="true" Name="appartment" Title='<%# Translate("/dicampaign/forms/flatnumber") %>' runat="server" />
			    </div>							
		    </div>
		    <div class="col">
			    <div class="small">
                    <DI:Input ID="ZipInput" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" TypeOfInput="ZipCode" Name="zip" Title='<%# Translate("/dicampaign/forms/zip") %>' DisplayMessage='<%# Translate("/dicampaign/forms/zip.message") %>' runat="server" />
			    </div>
			    <div class="medium">
                    <DI:Input ID="StateInput" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" TypeOfInput="Text" Name="state" Title='<%# Translate("/dicampaign/forms/city") %>' DisplayMessage='<%# Translate("/dicampaign/forms/city.message") %>' runat="server" />
			    </div>							
		    </div>												
	    </div>
    </div>
					

	<div class="divider"><hr /></div>
	
    				
	<div class="row">
		<div class="col">
            <DI:Input ID="TelephoneInput" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" TypeOfInput="Telephone" Name="mobile" Title='<%# Translate("/dicampaign/forms/mobile") %>' DisplayMessage='<%# Translate("/dicampaign/forms/mobile.message") %>' runat="server" />
		</div>
		<div class="col">
            <%--<%# Translate("/dicampaign/forms/email") %>  <%# Translate("/dicampaign/forms/email.message") %>  <i>(användarnamn till Di-konto)</i>--%>
            <DI:Input ID="EmailInput" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" TypeOfInput="Email" Name="email" Title='E-post' DisplayMessage='Ange e-post' runat="server" />
		</div>						
	</div>	
	
    <%--<asp:PlaceHolder ID="PlaceHolderPasswds" Visible='<%#IsDigitalCampaign%>' runat="server">
        <div class="row">
		    <div class="col">
                <DI:Input ID="PasswdInput1" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" TypeOfInput="Password" Name="passwd1" Title='Lösenord <i>(till Di-konto)</i>' DisplayMessage='Ange lösenord' runat="server" />
		    </div>
		    <div class="col">
                <DI:Input ID="PasswdInput2" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" TypeOfInput="Password" Name="passwd1" Title='Repetera lösenord' DisplayMessage='Ange lösenord' runat="server" />
		    </div>
	    </div>	
    </asp:PlaceHolder>--%>

    <asp:PlaceHolder ID="SocialSecurityPlaceHolder" Visible="false" runat="server">
    <div class="row">
		<div class="col">
            <DI:Input ID="SocialSecurityNoInput" CssClass="text" Name="socialsecrityno" MinValue="8" MaxValue="8" TypeOfInput="SocialSecurityNumber" AutoComplete="true" Title="<%$ Resources: EPiServer, common.user.socialsecuritynumberformat %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.socialsecuritynumberrequired %>" runat="server" />
        </div>
        <div class="col">
            <asp:Label ID="SocialSecurityNoInputErrMess" Visible="false" CssClass="formError" runat="server" Text="<%$ Resources: EPiServer, dicampaign.forms.socialsecurityerror %>"></asp:Label>
		</div>	
    </div>
    </asp:PlaceHolder>	
        		
    
    <%--student verification--%>
    <asp:PlaceHolder ID="BirthNoPlaceHolder" Visible="false" runat="server">
    <div class="row">
		<div class="col">
            <DI:Input ID="BirthNoInput" CssClass="text" Required="true" Name="birthno" MinValue="10" MaxValue="10" TypeOfInput="BirthNo" AutoComplete="true" Title="<%$ Resources: EPiServer, common.user.birthnoformat %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.birthnorequired %>" runat="server" />
        </div>
        <div class="col">
            <asp:Label ID="BirthNoErrMess" Visible="false" CssClass="formError" runat="server" Text="<%$ Resources: EPiServer, dicampaign.forms.studentverification %>"></asp:Label>
		</div>	
    </div>
    </asp:PlaceHolder>


	<div class="divider"><hr /></div>

    <asp:PlaceHolder ID="DiGoldPlaceHolder" Visible="false" runat="server">
        <div class="row row-checkbox">
		    <span class="checkbox">
                <di:Input ID="TermsAcceptedInput" TypeOfInput="CheckBox" Required="true" Name="terms" Title="<%# CampPage.GetTermsAndConditions() %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.agreementoftermsrequired %>" runat="server" />
		    </span>					
	    </div>					
	</asp:PlaceHolder>      
    
    
    <asp:PlaceHolder ID="DiGoldLandingPagePlaceHolder" Visible="false" runat="server">
        <div class="row ">
		    <asp:Label ID="DiGoldInfo" runat="server" />			
	    </div>					
	</asp:PlaceHolder>           


    <uc1:PayMethodsForm ID="PayMethodsForm1" runat="server" />

    			
	<div class="button-wrapper">
        <%--<asp:Button ID="StreetFormButton" CssClass="btn" Text="<%$ Resources: EPiServer, dicampaign.forms.send %>" OnClick="StreetFormButton_Click" OnClientClick="return jsCheckPasswds();" runat="server" />--%>
        <asp:Button ID="StreetFormButton" CssClass="btn" Text="<%$ Resources: EPiServer, dicampaign.forms.send %>" OnClick="StreetFormButton_Click" runat="server" />
	</div>
</div>
<!-- // Street -->


<script language="javascript" type="text/javascript">
    function jsShowAddressFields(bo) {

        if (bo == 'true') {
            $("#divAddress1").show();
            $("#divAddress2").show();
        }
        else {
            $("#divAddress1").hide();
            $("#divAddress2").hide();
        }
    }

    <% 
    if (PayMethodsForm1.HideAddressFields)
        Response.Write("jsShowAddressFields('false');");
    else
        Response.Write("jsShowAddressFields('true');");
    %>

//    function jsCheckPasswds() {
//        var p1 = $("#< %=PasswdInput1.ClientID% >_Input").val();
//        var p2 = $("#< %=PasswdInput2.ClientID% >_Input").val();

//        if (p1 != p2) {
//            alert("Lösenorden är inte identiska. Var god försök igen.");
//            return false;
//        }

//        return true;
//    }
</script>

