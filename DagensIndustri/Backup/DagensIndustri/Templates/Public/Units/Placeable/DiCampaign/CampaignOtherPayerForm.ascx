<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CampaignOtherPayerForm.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.DiCampaign.CampaignOtherPayerForm" %>
<%@ Register TagPrefix="DI" TagName="Input" Src="~/Tools/Classes/WebControls/InputWithValidation.ascx" %>


<div id="content"> 
	<h2>
        <asp:Literal ID="AnswerCardLiteral" runat="server" />   
    </h2>
	<h3 class="ill-checkbox">
        <asp:Literal ID="AnswerCheckBoxLiteral" runat="server" />
    </h3>
			
	<div class="form-nav"> 
  	    <ul> 
  		    <li class="current"><a href="#form-street"><%= FormTitle%></a></li>					
  	    </ul> 
  	    <p class="required"><%= Translate("/dicampaign/forms/mandatoryinformation")%></p>
    </div>
 
  		
    <div class="form-box"> 						
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
                <DI:Input ID="AttentionInput" CssClass="text" StripHtml="true" AutoComplete="true" TypeOfInput="Text" Name="att" Title='<%# Translate("/dicampaign/forms/attention") %>' runat="server" />
			</div>					
		</div>	
					
		<div class="row"> 
			<div class="col"> 
				<DI:Input ID="CareOfInput" CssClass="text" StripHtml="true" AutoComplete="true" TypeOfInput="Text" Name="co" Title='<%# Translate("/dicampaign/forms/careof") %>' runat="server" />
			</div>						
		</div>											
					
		<div class="row"> 
			<div class="col">
                <DI:Input ID="StreetInput" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" TypeOfInput="Text" Name="street" Title='<%# Translate("/dicampaign/forms/street") %>' DisplayMessage='<%# Translate("/dicampaign/forms/street.number") %>' runat="server" />
			</div> 
			<div class="col"> 
				<div class="small"> 
                    <DI:Input ID="StreetNumberInput" CssClass="text" Required="false" StripHtml="true" AutoComplete="true" TypeOfInput="Text" Name="number" Title='<%# Translate("/dicampaign/forms/streetnumber") %>'  DisplayMessage='<%# Translate("/dicampaign/forms/streetnumber.message") %>' runat="server" />
				</div> 
			</div>						
		</div>	
					
		<div class="row"> 
			<div class="col"> 
				<div class="small"> 
					<DI:Input ID="ZipInput" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" TypeOfInput="ZipCode" Name="zip" Title='<%# Translate("/dicampaign/forms/zip") %>' DisplayMessage='<%# Translate("/dicampaign/forms/zip.message") %>' runat="server" />
				</div> 
				<div class="medium"> 
					<DI:Input ID="StateInput" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" TypeOfInput="Text" Name="state" Title='<%# Translate("/dicampaign/forms/city") %>' DisplayMessage='<%# Translate("/dicampaign/forms/city.message") %>' runat="server" />
				</div>							
			</div> 
			<div class="col"> 
				<DI:Input ID="TelephoneInput" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" TypeOfInput="Telephone" Name="mobile" Title='<%# Translate("/dicampaign/forms/mobile") %>' DisplayMessage='<%# Translate("/dicampaign/forms/mobile.message") %>' runat="server" />
			</div>																
		</div> 
					
		<div class="divider"><hr /></div> 
					
		<div class="row"> 
			<div class="col">
                <DI:Input ID="OrgNumberInput" CssClass="text" StripHtml="true" AutoComplete="true" TypeOfInput="OrgNumber"  Name="orgnr" Title='<%# Translate("/dicampaign/forms/orgnumber") %>' DisplayMessage='<%# Translate("/dicampaign/forms/orgnumber.message") %>' runat="server" />
			</div>						
		</div>		
					
		<div class="divider"><hr /></div>					
				
        <div class="button-wrapper">
            <asp:Button ID="OtherPayerFormButton" CssClass="btn" Text="<%$ Resources: EPiServer, dicampaign.forms.send %>"  OnClick="OtherPayerFormButton_Click" runat="server" />
	    </div>
	</div>	
</div> 

</div>