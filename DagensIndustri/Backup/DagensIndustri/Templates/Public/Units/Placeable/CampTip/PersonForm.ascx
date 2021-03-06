﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PersonForm.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.CampTip.PersonForm" %>
<%@ Register TagPrefix="DI" TagName="Input" Src="~/Tools/Classes/WebControls/InputWithValidation.ascx" %>
<%--<%@ Register TagPrefix="di" TagName="UserMessage" Src="~/Templates/Public/Units/Placeable/UserMessage.ascx" %>--%>
<%--<di:UserMessage ID="UserMessageControl" runat="server" />--%>

<div class="form-box">
	<div class="section">					
					
	    <div class="row">
		    <div class="col">
                <DI:Input ID="FirstNameInput" CssClass="text" Required="false" StripHtml="true" AutoComplete="true" Name="firstname" TypeOfInput="Text" Title='<%# Translate("/common/forms/firstname") %>' DisplayMessage='<%# Translate("/common/forms/firstname.message") %>' runat="server" />
		    </div>
		    <div class="col">
                <DI:Input ID="LastNameInput" CssClass="text" Required="false" StripHtml="true" AutoComplete="true" Name="lastname" TypeOfInput="Text" Title='<%# Translate("/common/forms/lastname") %>' DisplayMessage='<%# Translate("/common/forms/lastname.message") %>' runat="server" />
		    </div>						
	    </div>
        
        <div class="row">
		    <div class="col">
                <DI:Input ID="EmailInput" CssClass="text" Required="false" StripHtml="true" AutoComplete="true" Name="mail" TypeOfInput="Email" Title='<%# Translate("/common/forms/mail") %>' DisplayMessage='<%# Translate("/common/forms/mail.message") %>' runat="server" />
		    </div>
		    <%--<div class="col">
                <DI:Input ID="TelephoneInput" CssClass="text" Required="false" StripHtml="true" AutoComplete="true" Name="phone" TypeOfInput="Telephone" Title='<%# Translate("/common/forms/phone") %>' DisplayMessage='<%# Translate("/common/forms/phone.message") %>' runat="server" />
		    </div>--%>
	    </div>	
        
        <%--
        <div class="divider"><hr /></div>
        
	    <div class="row textarea">
            <DI:Input ID="MessageInput" Name="message" IsTextArea="true" Required="true" StripHtml="true" Title="Motivering" runat="server" />
	    </div>
        
        <div class="button-wrapper">
            <asp:Button ID="SimpleFormButtonButton" Text="<%$ Resources: EPiServer, common.forms.next %>" CssClass="btn" OnClick="SimpleFormButton_Click" runat="server" />			
        </div>
        --%>

    </div>				
</div>