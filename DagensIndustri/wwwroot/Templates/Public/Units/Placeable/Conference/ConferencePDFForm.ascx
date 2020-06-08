<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ConferencePDFForm.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.Conference.ConferencePDFForm" %>
<%@ Register TagPrefix="DI" TagName="Input" Src="~/Tools/Classes/WebControls/InputWithValidation.ascx" %>

<asp:PlaceHolder ID="PDFPlaceHolder" runat="server">
    <!-- PDF -->
    <div class="section" id="conference-pdf">

	    <p><%# Translate("/conference/forms/pdf/text") %></p>

	    <div class="divider"><hr /></div>					
					
	    <div class="row">
		    <div class="col">
                <DI:Input ID="NameInput" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" Name="name" TypeOfInput="Text" Title='<%# Translate("/conference/forms/common/name") %>' DisplayMessage='<%# Translate("/conference/forms/common/name.message") %>' runat="server" />
            </div>
		    <div class="col">
                <DI:Input ID="EmailInput" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" Name="mail" TypeOfInput="Email" Title='<%# Translate("/conference/forms/common/mail") %>' DisplayMessage='<%# Translate("/conference/forms/common/mail.message") %>' runat="server" />
		    </div>				
	    </div>			
	    <div class="row">
		    <div class="col">
                <DI:Input ID="TelephoneInput" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" Name="phone" TypeOfInput="Telephone" Title='<%# Translate("/conference/forms/common/phone") %>' DisplayMessage='<%# Translate("/conference/forms/common/phone.message") %>' runat="server" />
		    </div>						
	    </div>
    		
	    <div class="button-wrapper">
            <asp:Button ID="PDFFormButton" CssClass="btn" Text='Skicka' OnClick="PDFFormButton_Click" runat="server" />  <%--<%# Translate("/conference/forms/common/next") %>--%>
        </div>			
    </div>
    <!-- // PDF -->
</asp:PlaceHolder>

<asp:PlaceHolder ID="PDFMessagePlaceHolder" runat="server">
    <div class="section" id="conference-pdf">
        <p>
            <%= Translate("/conference/forms/pdf/nopdfmessage")%>
        </p>
    </div>
</asp:PlaceHolder>