<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WineClubForm.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.WineClubForm" %>
<%@ Register TagPrefix="DI" TagName="Input" Src="~/Tools/Classes/WebControls/InputWithValidation.ascx" %>

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
				<DI:Input ID="FirstNameInput" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" Name="firstname" TypeOfInput="Text" Title='<%# Translate("/wineclub/form/firstname") %>' DisplayMessage='<%# Translate("/wineclub/form/firstname.message") %>' runat="server" />
			</div>
            <div class="col"> 
				<DI:Input ID="LastNameInput" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" Name="lastname" TypeOfInput="Text" Title='<%# Translate("/wineclub/form/lastname") %>' DisplayMessage='<%# Translate("/wineclub/form/lastname.message") %>' runat="server" />
			</div>						
		</div> 
 
		<div class="row"> 
			<div class="col"> 
				<DI:Input ID="EmailInput" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" Name="mail" TypeOfInput="Email" Title='<%# Translate("/wineclub/form/mail") %>' DisplayMessage='<%# Translate("/wineclub/form/mail.message") %>' runat="server" />
			</div> 
			<div class="col">
                 <DI:Input ID="TelephoneInput" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" Name="tel" TypeOfInput="Telephone" Title='<%# Translate("/wineclub/form/phone") %>' DisplayMessage='<%# Translate("/wineclub/form/phone.message") %>' runat="server" /> 
			</div> 
		</div>	
	
		<div class="button-wrapper">	
            <asp:Button ID="WineClubFormButton" Text='<%# Translate("/wineclub/form/next") %>' CssClass="btn" OnClick="WineClubFormButton_Click" runat="server" />			
        </div>					
	
	</div> 
	<!-- // Registration --> 
								
</div>
			