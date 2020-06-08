<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PayWithDiscountCode.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.GasellFlow.PayWithDiscountCode" %>
<%@ Register TagPrefix="DI" TagName="Input" Src="~/Tools/Classes/WebControls/InputWithValidation.ascx" %>

<div class="form-box">  							
    <!-- Invoice -->
    <div class="section" id="form-street">
	    <div class="row">
		    <div class="col">
                <DI:Input ID="DiscountCodeInput" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" Name="code" TypeOfInput="Text" Title='<%# Translate("/gasell/discountcode") %>' DisplayMessage='<%# Translate("/gasell/flow/forms/discountcode.message") %>' runat="server" />
		    </div>						
	    </div>																								
    </div>
    <!-- // Invoice -->	
</div>