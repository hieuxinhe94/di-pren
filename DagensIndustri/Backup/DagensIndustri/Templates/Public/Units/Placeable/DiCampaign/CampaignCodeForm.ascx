<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CampaignCodeForm.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.DiCampaign.CampaignCodeForm" %>
<%@ Register TagPrefix="DI" TagName="Input" Src="~/Tools/Classes/WebControls/InputWithValidation.ascx" %>

<div id="content" class="campaign-code">
<%--    <h2>Hej Karl Nyström.</h2> --%>
    <h3>
        <%= Translate("/dicampaign/forms/codeform/heading") %>
    </h3> 
  		
    <div class="form-box"> 
  							
	    <!-- Code --> 
	    <div class="section" id="form-code"> 
		    <div class="row"> 
			    <div class="col">
                    <DI:Input ID="CodeInput" CssClass="text" StripHtml="true" Required="true" TypeOfInput="Text" AutoComplete="true" Name="code" Title='<%# Translate("/dicampaign/forms/codeform/personalcode") %>' DisplayMessage='<%# Translate("/dicampaign/forms/codeform/personalcode.message") %>' runat="server" />
			    </div> 
			    <div class="button-wrapper">
                    <asp:Button ID="CodeFormButton" CssClass="btn" Text="<%$ Resources: EPiServer, dicampaign.forms.next %>"  OnClick="CodeFormButton_Click" runat="server" />
			    </div>						
		    </div> 
	    </div> 
	    <!-- // Code --> 
				
    </div>
</div>