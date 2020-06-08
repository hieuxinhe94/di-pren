<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PayWithInvoice.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.GasellFlow.PayWithInvoice" %>
<%@ Register TagPrefix="DI" TagName="Input" Src="~/Tools/Classes/WebControls/InputWithValidation.ascx" %>

<div class="form-box">  							
    <!-- Invoice -->
    <div class="section" id="form-street">
	    <div class="row">
		    <div class="col">
                 <DI:Input ID="InvoiceAddressInput" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" Name="invoiceadd" TypeOfInput="Text" Title='<%# Translate("/gasell/flow/forms/invoiceaddress") %>' DisplayMessage='<%# Translate("/gasell/flow/forms/invoiceaddress.message") %>' runat="server" />
		    </div>
		    <div class="col">
            <DI:Input ID="InvoiceReferenceInput" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" Name="ref" TypeOfInput="Text" Title='<%# Translate("/gasell/flow/forms/invoicereference") %>' DisplayMessage='<%# Translate("/gasell/flow/forms/invoicereference.message") %>' runat="server" />
		    </div>						
	    </div>
					
	    <div class="row">
		    <div class="col">
                 <DI:Input ID="OrgNumberInput" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" TypeOfInput="OrgNumber" Name="personalnum" Title='<%# Translate("/gasell/flow/forms/orgnr") %>'  DisplayMessage='<%# Translate("/gasell/flow/forms/orgnr.message") %>'  runat="server" />
		    </div>
		    <div class="col">
			    <div class="small">
                    <DI:Input ID="ZipCodeInput" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" Name="zip" TypeOfInput="Numeric" Title='<%# Translate("/gasell/flow/forms/zip") %>' MaxValue="6" DisplayMessage='<%# Translate("/gasell/flow/forms/zip.message") %>' runat="server" />
			    </div>
			    <div class="medium">
                    <DI:Input ID="StateInput" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" Name="state" TypeOfInput="Text" Title='<%# Translate("/gasell/flow/forms/city") %>' DisplayMessage='<%# Translate("/gasell/flow/forms/city.message") %>' runat="server" />
			    </div>							
		    </div>	
	    </div>																										
    </div>
    <!-- // Invoice -->
</div>	
		