<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdditionalUserDetails.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.DiGoldMembershipFlow.AdditionalUserDetails" %>
<%@ Register TagPrefix="di" TagName="MainIntro" Src="~/Templates/Public/Units/Placeable/MainIntro.ascx" %>
<%@ Register TagPrefix="di" TagName="MainBody" Src="~/Templates/Public/Units/Placeable/MainBody.ascx" %>
<%@ Register TagPrefix="di" TagName="Input" Src="~/Tools/Classes/WebControls/InputWithValidation.ascx" %>
<%@ Register TagPrefix="di" TagName="DiGold" Src="~/Templates/Public/Units/Placeable/DiGoldMembershipFlow/DiGoldPromotionalOfferAcceptance.ascx"  %>

<di:MainIntro runat="server" />
<di:MainBody runat="server" />

<div class="form-nav">
  	<ul>
  		<li class="current">
            <a href="#form-check"><EPiServer:Translate Text="/digold/digoldmembership" runat="server" /></a>
        </li>
  	</ul>
  	<p class="required"><EPiServer:Translate Text="/common/requiredinformation" runat="server" /></p>
</div> 			
<div class="form-box">  							
	<!-- Check -->
	<div class="section" id="form-check">
		<div class="row">
			<div class="col">
                <di:Input ID="FirstNameInput" CssClass="text" Required="true" Name="firstname" TypeOfInput="Text" MaxValue="28" AutoComplete="true" Title="<%$ Resources: EPiServer, common.user.firstname %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.firstnamerequired %>" runat="server" />
			</div>

			<div class="col">
                <di:Input ID="LastNameInput" CssClass="text" Required="true" Name="lastname" TypeOfInput="Text" MaxValue="28" AutoComplete="true" Title="<%$ Resources: EPiServer, common.user.lastname %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.lastnamerequired %>" runat="server" />
			</div>
		</div>

		<div class="row">
			<div class="col">
                <di:Input ID="EmailInput" CssClass="text" Required="true" Name="email" TypeOfInput="Email" MinValue="6" AutoComplete="true" Title="<%$ Resources: EPiServer, common.user.personalemailaddress %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.emailrequired %>" runat="server" />
			</div>

			<div class="col">
                <di:Input ID="PhoneInput" CssClass="text" Required="true" Name="phone" TypeOfInput="Telephone" MinValue="7" MaxValue="20" AutoComplete="true" Title="<%$ Resources: EPiServer, common.user.mobilephone %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.mobilephonenumberrequired %>" runat="server" />
			</div>
		</div>	
					
		<div class="row">
			<div class="col">
                <di:Input ID="SocialSecurityNoInput" CssClass="text" Required="true" Name="socialsecrityno" MinValue="8" MaxValue="8" TypeOfInput="SocialSecurityNumber" AutoComplete="true" Title="<%$ Resources: EPiServer, common.user.socialsecuritynumberformat %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.socialsecuritynumberrequired %>" runat="server" />
			</div>
		</div>

        <di:DiGold ID="DiGoldControl" runat="server" />
        		
	</div>
	<!-- // Check -->
</div>