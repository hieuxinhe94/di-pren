<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MBAForm.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.MBAForm" %>
<%@ Register TagPrefix="DI" TagName="Input" Src="~/Tools/Classes/WebControls/InputWithValidation.ascx" %>
<%@ Register TagPrefix="di" TagName="DiGold" Src="~/Templates/Public/Units/Placeable/DiGoldMembershipFlow/DiGoldPromotionalOfferAcceptance.ascx"  %>


<asp:PlaceHolder ID="PlaceHolderAllContent" runat="server" Visible="true">

    <script type="text/javascript">
        $(document).ready(function () {
            $('.countable').jqEasyCounter({
                'maxChars': 2000,
                'maxCharsWarning': 1950,
                'msgFontSize': '12px',
                'msgFontColor': '#000',
                'msgTextAlign': 'right',
                'msgWarningColor': '#F00',
                'msgAppendMethod': 'insertAfter'
            });
        });
    </script>

 
     <asp:PlaceHolder ID="GoldFieldsPlaceHolder" runat="server" Visible='<%#!HttpContext.Current.User.IsInRole(DIClassLib.Membership.DiRoleHandler.RoleDiGold) && RegisterDiGoldMembershipOnSubmit %>'>
        <div class="form-nav"> 
  	        <ul> 
  		        <li class="current"><a href="#gold-registration">DI Guld medlemskap</a></li>					
  	        </ul> 
  			
            <p class="required">= obligatoriska uppgifter</p> 
        </div> 
        
        <div class="form-box">
            <div class="section" id="gold-registration"> 
                <div class="row">
			        <div class="col">
                        <di:Input ID="GoldFirstNameInput" CssClass="text" Required="true" Name="firstname" TypeOfInput="Text" MaxValue="28" AutoComplete="true" Title="<%$ Resources: EPiServer, common.user.firstname %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.firstnamerequired %>" runat="server" />
			        </div>

			        <div class="col">
                        <di:Input ID="GoldLastNameInput" CssClass="text" Required="true" Name="lastname" TypeOfInput="Text" MaxValue="28" AutoComplete="true" Title="<%$ Resources: EPiServer, common.user.lastname %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.lastnamerequired %>" runat="server" />
			        </div>
		        </div>

		        <div class="row">
			        <div class="col">
                        <di:Input ID="GoldEmailInput" CssClass="text" Required="true" Name="email" TypeOfInput="Email" MinValue="6" AutoComplete="true" Title="<%$ Resources: EPiServer, common.user.personalemailaddress %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.emailrequired %>" runat="server" />
			        </div>

			        <div class="col">
                        <di:Input ID="GoldPhoneInput" CssClass="text" Required="true" Name="phone" TypeOfInput="Telephone" MinValue="7" MaxValue="20" AutoComplete="true" Title="<%$ Resources: EPiServer, common.user.mobilephone %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.mobilephonenumberrequired %>" runat="server" />
			        </div>
		        </div>	
					
		        <div class="row">
			        <div class="col">
                        <di:Input ID="GoldSocialSecurityNoInput" CssClass="text" Required="true" Name="socialsecrityno" MinValue="8" MaxValue="8" TypeOfInput="SocialSecurityNumber" AutoComplete="true" Title="<%$ Resources: EPiServer, common.user.socialsecuritynumberformat %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.socialsecuritynumberrequired %>" runat="server" />
			        </div>
		        </div>
                <di:DiGold ID="DiGold1" runat="server" />
            </div>
        </div>
    </asp:PlaceHolder>


    <div class="form-nav"> 
  	    <ul> 
  		    <li class="current"><a href="#wineclub-registration">Intresseanmälan</a></li>					
  	    </ul> 
        <p class="required">= obligatoriska uppgifter</p> 
    </div> 

    <div class="form-box"> 
	    <!-- Registration --> 
	                                                                                                                                                                                                                                                                                                                                                                    <div class="section" id="wineclub-registration"> 
        <div class="row">
            <div class="col">
                <asp:Label AssociatedControlID="NominationRadioButtonList" runat="server"><%= Translate("/mba/form/nomination")%></asp:Label>
                <span class="checkbox"> 
                    <asp:RadioButtonList ID="NominationRadioButtonList" RepeatDirection="Horizontal" runat="server">
                        <asp:ListItem Selected="True" Text="Egen ansökan" Value="0" />
                        <asp:ListItem Text="Nominering" Value="1" />
                    </asp:RadioButtonList>
                </span>
            </div>
            <div class="col">
                <%--<%# Translate("/mba/form/birthdate") %>--%>
                <%--<%# Translate("/mba/form/birthdate.message") %>--%>
                <DI:Input ID="PersonalInput" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" Name="personal-number" TypeOfInput="SocialSecurityNumber" Title='Födelsedatum <i>(ÅÅÅÅMMDD)</i>' DisplayMessage='Fyll i födelsedatum (ÅÅÅÅMMDD)' runat="server" />
            </div>
        </div>
		<div class="row"> 
			<div class="col"> 
				<DI:Input ID="FirstNameInput" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" Name="firstname" TypeOfInput="Text" Title='<%# Translate("/mba/form/firstname") %>' DisplayMessage='<%# Translate("/mba/form/firstname.message") %>' runat="server" />
			</div>
            <div class="col"> 
				<DI:Input ID="LastNameInput" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" Name="lastname" TypeOfInput="Text" Title='<%# Translate("/mba/form/lastname") %>' DisplayMessage='<%# Translate("/mba/form/lastname.message") %>' runat="server" />
			</div>						
		</div> 
        <div class="row">
            <div class="col">
                <DI:Input ID="AddressInput" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" Name="address" TypeOfInput="Text" Title='<%# Translate("/mba/form/address") %>' DisplayMessage='<%# Translate("/mba/form/address.message") %>' runat="server" />
            </div>
            <div class="col">
                <DI:Input ID="ZipInput" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" Name="zip-code" TypeOfInput="ZipCode" Title='<%# Translate("/mba/form/zip") %>' DisplayMessage='<%# Translate("/mba/form/zip.message") %>' MaxValue="10" runat="server" />     
            </div>
        </div>
         <div class="row">
            <div class="col">
                <DI:Input ID="CityInput" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" Name="place" TypeOfInput="Text" Title='<%# Translate("/mba/form/city") %>' DisplayMessage='<%# Translate("/mba/form/city.message") %>' runat="server" />
            </div>
            <div class="col">
                 <DI:Input ID="TelephoneInput" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" Name="tel" TypeOfInput="Telephone" Title='<%# Translate("/mba/form/mobile") %>' DisplayMessage='<%# Translate("/mba/form/mobile.message") %>' runat="server" /> 
			</div> 
        </div>
        <div class="row"> 
			<div class="col"> 
				<DI:Input ID="EmailInput" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" Name="mail" TypeOfInput="Email" Title='<%# Translate("/mba/form/mail") %>' DisplayMessage='<%# Translate("/mba/form/mail.message") %>' runat="server" />
			</div> 
             <div class="col">
                <DI:Input ID="LinkedInInput" CssClass="text" Required="false" StripHtml="true" AutoComplete="true" Name="linkedin" TypeOfInput="Text" Title='<%# Translate("/mba/form/linkedin") %>' runat="server" />
            </div>
		</div>
        <div class="row">
            <div class="col">
                <DI:Input ID="CompanyInput" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" Name="company" TypeOfInput="Text" Title='<%# Translate("/mba/form/company") %>' DisplayMessage='<%# Translate("/mba/form/company.message") %>' runat="server" />    
            </div>
            <div class="col">
                <DI:Input ID="PositionInput" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" Name="position" TypeOfInput="Text" Title='<%# Translate("/mba/form/title") %>' DisplayMessage='<%# Translate("/mba/form/title.message") %>' runat="server" />     
            </div>
        </div>
        <div class="row">
            <div class="col">
                <DI:Input ID="AcademicEducationInput" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" Name="education" TypeOfInput="Text" Title='<%# Translate("/mba/form/academiceducation") %>' DisplayMessage='<%# Translate("/mba/form/academiceducation.message") %>' runat="server" />   
            </div>
            <div class="col">
                <DI:Input ID="AcademicPointsInput" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" Name="points" TypeOfInput="Text" Title='<%# Translate("/mba/form/academicpoints") %>' DisplayMessage='<%# Translate("/mba/form/academicpoints.message") %>' runat="server" />    
            </div>
        </div>
        <div class="row">
            <div class="col">
                <DI:Input ID="WorkYearsInput" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" Name="work-years" TypeOfInput="Text" Title='<%# Translate("/mba/form/workyears") %>' DisplayMessage='<%# Translate("/mba/form/workyears.message") %>' runat="server" />
            </div>
            <div class="col">
                <asp:Label runat="server" AssociatedControlID="DdlEnglishLevel"><%= Translate("/mba/form/englishlevel")%></asp:Label>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="form" ControlToValidate="DdlEnglishLevel" Display="Dynamic">
                </asp:RequiredFieldValidator>
                <asp:DropDownList runat="server" ID="DdlEnglishLevel" CssClass="select">
                    <asp:ListItem Text="1" Value="1" />
                    <asp:ListItem Text="2" Value="2"/>
                    <asp:ListItem Text="3" Value="3" />
                    <asp:ListItem Text="4" Value="4" />
                    <asp:ListItem Text="5" Value="5" />
                </asp:DropDownList>
                <span class="status required input-text">Obligatorisk</span>
            </div>
        </div>
        
	    <div class="row textarea">
            <DI:Input ID="MotivationInput" IsTextArea="true" CssClass="text countable" Required="true" MaxValue="2000" StripHtml="true" AutoComplete="true" Name="motivation" TypeOfInput="Text" Title='<%# Translate("/mba/form/motivation")%>' runat="server" />
        </div>
		<div class="button-wrapper">	
            <asp:Button ID="WineClubFormButton" Text='<%# Translate("/mba/form/send") %>' CssClass="btn" OnClick="MBAFormButton_Click" runat="server" />			
        </div>					
	</div> 
	    <!-- // Registration --> 								
    </div>

</asp:PlaceHolder>