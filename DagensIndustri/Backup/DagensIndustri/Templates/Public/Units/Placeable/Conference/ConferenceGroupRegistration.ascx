<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ConferenceGroupRegistration.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.Conference.ConferenceGroupRegistration" %>
<%@ Register TagPrefix="DI" TagName="Input" Src="~/Tools/Classes/WebControls/InputWithValidation.ascx" %>

<asp:PlaceHolder ID="GroupRegistrationPlaceHolder" runat="server">
    <!-- Group -->
    <div class="section" id="conference-group">

	    <p>
		    <strong><%# Translate("/conference/forms/groupregistration/more") %></strong><br />
		    <%# Translate("/conference/forms/groupregistration/info") %>
	    </p>
	  				
	    <div class="divider"><hr /></div>					
					
	    <div class="row">
		    <div class="col">
                <DI:Input ID="FirstNameInput" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" Name="firstname" TypeOfInput="Text" Title='<%# Translate("/conference/forms/common/firstname") %>' DisplayMessage='<%# Translate("/conference/forms/common/firstname.message") %>' runat="server" />
		    </div>
		    <div class="col">
                <DI:Input ID="LastNameInput" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" Name="lastname" TypeOfInput="Text" Title='<%# Translate("/conference/forms/common/lastname") %>' DisplayMessage='<%# Translate("/conference/forms/common/lastname.message") %>' runat="server" />
		    </div>						
	    </div>
					
	    <div class="row">
		    <div class="col">
                <DI:Input ID="EmailInput" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" Name="mail" TypeOfInput="Email" Title='<%# Translate("/conference/forms/common/mail") %>' DisplayMessage='<%# Translate("/conference/forms/common/mail.message") %>' runat="server" />
		    </div>
		    <div class="col">
                <DI:Input ID="TelephoneInput" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" Name="phone" TypeOfInput="Telephone" Title='<%# Translate("/conference/forms/common/phone") %>' DisplayMessage='<%# Translate("/conference/forms/common/phone.message") %>' runat="server" />
		    </div>						
	    </div>	
									
	    <div class="row textarea">
            <DI:Input ID="MessageInput" Name="message" IsTextArea="true" Required="false" StripHtml="true" Title='<%# Translate("/conference/forms/common/message") %>' runat="server" />										
	    </div>
	    
       <div class="row">
        <div class="col">
            <asp:HiddenField ID="captchaNumber1" runat="server"></asp:HiddenField>
            <asp:HiddenField ID="captchaNumber2" runat="server"></asp:HiddenField>
            <DI:Input ID="txtCaptchaConf" CssClass="text captcha" Required="false" StripHtml="true" AutoComplete="false" TypeOfInput="Numeric" Name="captchaConf" Title="<%#CaptchaTitle%>" DisplayMessage="" runat="server" />
        </div>
      </div>

        <div class="button-wrapper">	
            <asp:Button ID="ConferenceGroupRegistrationButton" Text='<%# Translate("/conference/forms/common/next") %>' CssClass="btn" OnClick="ConferenceGroupRegistrationButton_Click" runat="server" />			
        </div>					
	
    </div>
    <!-- // Group -->
</asp:PlaceHolder>

<asp:PlaceHolder ID="FormHiddenTextPlaceHolder" runat="server" >
    <div class="section" id="conference-group">
        <asp:Literal ID="FormHiddenTextLiteral" runat="server" />
    </div>
</asp:PlaceHolder>