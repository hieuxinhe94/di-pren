<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ConferenceRegistration.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.Conference.ConferenceRegistration" %>
<%@ Register TagPrefix="DI" TagName="Input" Src="~/Tools/Classes/WebControls/InputWithValidation.ascx" %>

<asp:PlaceHolder ID="ConferenceRegistrationPlaceHolder" runat="server">
    <!-- Registration -->
    <div class="section" id="conference-registration">

	    <asp:Literal ID="RefisterFormTextLiteral" runat="server" />				        

	    <div class="divider"><hr /></div>					
					
	    <div class="row">
		    <div class="col">
              <DI:Input ID="FirstNameInput" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" TypeOfInput="Text" Name="firstname" Title='<%# Translate("/conference/forms/common/firstname") %>' DisplayMessage='<%# Translate("/conference/forms/common/firstname.message") %>' runat="server" />
		    </div>
		    <div class="col">
                <DI:Input ID="LastNameInput" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" TypeOfInput="Text" Name="lastname" Title='<%# Translate("/conference/forms/common/lastname") %>' DisplayMessage='<%# Translate("/conference/forms/common/lastname.message") %>' runat="server" />
		    </div>						
	    </div>
		
        <div class="row">
		    <div class="col">
                <DI:Input ID="EmailInput" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" Name="mail" TypeOfInput="Email" Title='<%# Translate("/conference/forms/common/mail") %>'   DisplayMessage='<%# Translate("/conference/forms/common/mail.message") %>'  runat="server" />
		    </div>
		    <div class="col">
                <DI:Input ID="TelephoneInput" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" Name="phone" TypeOfInput="Telephone" Title='<%# Translate("/conference/forms/common/phone") %>'  DisplayMessage='<%# Translate("/conference/forms/common/phone.message") %>'  runat="server" />
		    </div>						
	    </div>
        			
	    <div class="row">
		    <div class="col">
		      <%-- # Translate("/conference/forms/registration/title")  Befattning --%>
                <DI:Input ID="TitleInput" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" TypeOfInput="Text" Name="title" Title='<%#TitleTitle%>' DisplayMessage='<%#DispTitle%>' runat="server" />
		    </div>
		    <div class="col">
                <DI:Input ID="CodeInput" CssClass="text" Required="false" StripHtml="true" Name="code" TypeOfInput="Text" Title='<%# Translate("/conference/forms/registration/code") %>' runat="server" />
		    </div>						
	    </div>	
    	

	    <div class="row">
            <div class="col">
                
                <asp:PlaceHolder ID="PlaceHolderCompanyTop" Visible="false" runat="server">
                    <DI:Input ID="CompanyInputTop" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" TypeOfInput="Text" Name="comapny" Title='<%#TitleCompany%>'  DisplayMessage='<%#DispCompany%>' runat="server" />
                </asp:PlaceHolder>

		    </div>
            <div class="col">
			    <asp:Label ID="Label1" runat="server" AssociatedControlID="DdlInfoChannel"><%= Translate("/conference/forms/registration/information")%></asp:Label>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="form" ControlToValidate="DdlInfoChannel" Display="Dynamic">
                </asp:RequiredFieldValidator>
                <asp:DropDownList runat="server" ID="DdlInfoChannel" CssClass="select" OnPreRender="DdlInfoChannelOnPreRender" DataSourceID="GetInfoChannels" DataValueField="icid" DataTextField="ictext"></asp:DropDownList>                    
			    <asp:ObjectDataSource ID="GetInfoChannels" TypeName="DagensIndustri.Tools.Classes.Conference.ConferenceODS" runat="server"></asp:ObjectDataSource>
		    </div>									
	    </div>
		

        <asp:PlaceHolder ID="PlaceHolderInvoice" runat="server">

            <div class="divider"><hr /></div>
            <div style="margin-top: 10px; margin-left: 20px;">
                <h3><%= Translate("/conference/forms/registration/invoiceinfo")%></h3>
            </div>
            <div class="row">
                <div class="col">
                    <DI:Input ID="CompanyInput" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" TypeOfInput="Text" Name="comapny" Title='<%# Translate("/conference/forms/registration/company") %>' DisplayMessage='<%# Translate("/conference/forms/registration/company.message") %>' runat="server" />
                </div>
                <div class="col">
                    <DI:Input ID="OrgNumberInput" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" TypeOfInput="Telephone" Name="orgnr" Title='<%# Translate("/conference/forms/registration/orgnr") %>' DisplayMessage='<%# Translate("/conference/forms/registration/orgnr.message") %>' runat="server" />
                </div>
            </div>
            <div class="row">
                <div class="col">
                    <DI:Input ID="InvoiceReferenceInput" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" Name="invoice" TypeOfInput="Text" MaxValue="25" Title='<%#string.Format("{0} <i>({1})</i>", Translate("/conference/forms/registration/invoicereference"), Translate("/conference/forms/registration/invoicereference.limit"))%>' DisplayMessage='<%#string.Format("{0} ({1})", Translate("/conference/forms/registration/invoicereference.message"), Translate("/conference/forms/registration/invoicereference.limit"))%>' runat="server" />
                </div>
                <div class="col">
                </div>
            </div>
            <div class="row">
                <div class="col">
                    <DI:Input ID="InvoiceAddressInput" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" Name="invoiceadd" TypeOfInput="Text" Title='<%# Translate("/conference/forms/registration/invoiceaddress") %>' DisplayMessage='<%# Translate("/conference/forms/registration/invoiceaddress.message") %>' runat="server" />
                </div>
                <div class="col">
                    <div class="small">
                        <DI:Input ID="ZipCodeInput" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" Name="zip" TypeOfInput="Text" Title='<%# Translate("/conference/forms/registration/zip") %>' MaxValue="10" DisplayMessage='<%# Translate("/conference/forms/registration/zip.message") %>' runat="server" />
                    </div>
                    <div class="medium">
                        <DI:Input ID="StateInput" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" Name="state" TypeOfInput="Text" Title='<%# Translate("/conference/forms/registration/city") %>' DisplayMessage='<%# Translate("/conference/forms/registration/city.message") %>' runat="server" />
                    </div>
                </div>
            </div>

        </asp:PlaceHolder>

        			
	    <div class="divider" id="eventsDivider" runat="server"><hr /></div>

        <asp:Literal ID="EventsLiteral" runat="server" />										
	
        <asp:Repeater ID="ConferenceRepeater" OnItemDataBound="ConferenceRepeater_ItemDataBound" runat="server">      
            <ItemTemplate>
                <asp:HiddenField ID="ConferenceNameHiddenField" Value='<%#  DataBinder.Eval(Container.DataItem, "Name") %>' runat="server" />
                <asp:HiddenField ID="ConferenceDateHiddenField" Value='<%#  DataBinder.Eval(Container.DataItem, "Date") %>' runat="server" />
           	
                    <asp:Repeater ID="EventTimesRepeater" OnItemDataBound="EventTimesRepeater_ItemDatabound" runat="server">
                        <ItemTemplate>
                            <div class="row radiolist" id="RadioListRow" runat="server">               
                                <h4>
                                    <asp:Literal ID="NameLiteral" runat="server" /> <asp:Literal ID="DateLiteral" runat="server" /> 
                                    <span>
                                        <%# DataBinder.Eval(Container.DataItem, "TimeStart")%> - <%# DataBinder.Eval(Container.DataItem, "TimeEnd")%>
                                    </span>
                                </h4>
                                <%--<asp:PlaceHolder ID="RadioButtonPlaceHolder" Visible="false" runat="server">--%>
                                    <ul id="list" runat="server">
                        
                                    </ul>
                                <%--</asp:PlaceHolder>--%>
                            </div>           
                        </ItemTemplate>
                    </asp:Repeater>        
            </ItemTemplate>
        </asp:Repeater>
      
      <div class="row">
        <div class="col">
            <asp:HiddenField ID="captchaNumber1" runat="server"></asp:HiddenField>
            <asp:HiddenField ID="captchaNumber2" runat="server"></asp:HiddenField>
            <DI:Input ID="txtCaptchaConf" CssClass="text captcha" Required="false" StripHtml="true" AutoComplete="false" TypeOfInput="Numeric" Name="captchaConf" Title="<%#CaptchaTitle%>" DisplayMessage="" runat="server" />
        </div>
      </div>
       
        <div class="button-wrapper">
            <asp:Button ID="ConferenceRegistrationButton" Text='<%# Translate("/conference/forms/common/next") %>' CssClass="btn" OnClick="ConferenceRegistrationButton_Click" runat="server" /> 
        </div>
    </div>
    <!-- // Registration -->
</asp:PlaceHolder>


<asp:PlaceHolder ID="FormHiddenTextPlaceHolder" runat="server">
    <div class="section" id="conference-registration">
        <asp:Literal ID="FormHiddenTextLiteral" runat="server" />
    </div>
</asp:PlaceHolder>