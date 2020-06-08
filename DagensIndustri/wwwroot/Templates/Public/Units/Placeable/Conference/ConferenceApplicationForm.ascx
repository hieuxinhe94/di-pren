<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ConferenceApplicationForm.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.Conference.ConferenceApplicationForm" %>
<%@ Register TagPrefix="di" TagName="registration" Src="~/Templates/Public/Units/Placeable/Conference/ConferenceRegistration.ascx" %>
<%@ Register TagPrefix="di" TagName="groupregistration" Src="~/Templates/Public/Units/Placeable/Conference/ConferenceGroupRegistration.ascx" %>
<%@ Register TagPrefix="di" TagName="pdfform" Src="~/Templates/Public/Units/Placeable/Conference/ConferencePDFForm.ascx" %>
<%@ Register TagPrefix="DI" TagName="UserMessage" Src="~/Templates/Public/Units/Placeable/UserMessage.ascx" %>

<!-- Errors -->
<DI:UserMessage ID="UserMessageControl" runat="server" />
<!-- // Errors -->

<div class="form-nav">
  	<ul id="myUL" runat="server">
        <li class="current" id="RegistrationListItem" runat="server">
            <asp:HyperLink ID="RegistrationHyperLink" NavigateUrl="#conference-registration" runat="server"><EPiServer:Translate ID="Translate1" Text="/conference/forms/tabs/registration" runat="server" /></asp:HyperLink>
        </li>
  		<li id="GroupRegistrationListItem" runat="server">
            <asp:HyperLink ID="GroupRegistrationHyperLink" NavigateUrl="#conference-group" runat="server"><EPiServer:Translate ID="Translate2" Text="/conference/forms/tabs/groupregistration" runat="server" /></asp:HyperLink>
        </li>
  		<li id="PDFFormListItem" runat="server">
            <asp:HyperLink ID="PDFFormHyperLink" NavigateUrl="#conference-pdf" runat="server"><EPiServer:Translate ID="Translate3" Text="/conference/forms/tabs/pdfform" runat="server" /></asp:HyperLink>
        </li>					        		
  	</ul>
  	<p class="required"><EPiServer:Translate ID="Translate4" Text="/common/requiredinformation" runat="server" /></p>
</div>
 
<div class="form-box">

    <di:registration id="RegistrationControl" runat="server" />
    <di:groupregistration id="GroupRegistrationControl" runat="server" />
    <di:pdfform id="PDFFormControl" runat="server" />
			
</div>

