<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DiGoldPromotionalOfferAcceptance.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.DiGoldMembershipFlow.DiGoldPromotionalOfferAcceptance" %>
<%@ Register TagPrefix="di" TagName="Input" Src="~/Tools/Classes/WebControls/InputWithValidation.ascx"  %>

<div class="divider"><hr /></div>

<asp:PlaceHolder ID="PromotionalOfferPlaceHolder" Visible='<%# HasPromotionalOffer %>' runat="server">
	<div class="row row-checkbox">
		<p class="label"><%# (string)EPiFunctions.GetPagePropertyValueBySettingsPage(CurrentPage, "DiGoldPromotionalOfferPage", "PromotionalOfferText") %></p>
		<span class="checkbox">
            <asp:CheckBox ID="AcceptPromotionalOfferCheckBox" Text='<%# (string)EPiFunctions.GetPagePropertyValueBySettingsPage(CurrentPage, "DiGoldPromotionalOfferPage", "AcceptPromotionalOfferText") %>' runat="server" />
		</span>
	</div>
					
	<div class="divider"><hr /></div>
</asp:PlaceHolder>
					
<div class="row row-checkbox">
    <span class="checkbox">
        <di:Input ID="TermsAcceptedInput" TypeOfInput="CheckBox" Required="true" Name="terms" Title="<%# GetTermsAndConditions() %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.agreementoftermsrequired %>" runat="server" />
	</span>
</div>