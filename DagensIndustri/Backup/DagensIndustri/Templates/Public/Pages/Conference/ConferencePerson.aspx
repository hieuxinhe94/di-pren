<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="ConferencePerson.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.Conference.ConferencePerson" %>
<%@ Register TagPrefix="DI" TagName="Heading" Src="~/Templates/Public/Units/Placeable/Heading.ascx" %>
<%@ Register TagPrefix="di" TagName="Mainintro" Src="~/Templates/Public/Units/Placeable/MainIntro.ascx" %>
<%@ Register TagPrefix="di" TagName="Mainbody" Src="~/Templates/Public/Units/Placeable/MainBody.ascx" %>
<%@ Register TagPrefix="di" TagName="UserMessage" Src="~/Templates/Public/Units/Placeable/UserMessage.ascx" %>
<%@ Register TagPrefix="DI" TagName="Input" Src="~/Tools/Classes/WebControls/InputWithValidation.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="HeadingContentPlaceHolder" runat="server">
    <DI:Heading ID="HeadingControl" runat="server"/>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
<asp:PlaceHolder ID="phDisableAllInputs" runat="server" Visible='<%#!ChangeAllowed %>'>
    <script type="text/javascript">
        $(function () {
            $("#update-conference-person input").attr("disabled", true);
        });
    </script>
</asp:PlaceHolder>
    <!-- Errors -->
    <di:UserMessage ID="UserMessageControl" runat="server" />
    <asp:PlaceHolder ID="PlaceHolderConferenceUpdate" runat="server">
	<!-- // Errors -->
    <di:Mainintro runat="server" />

    <di:Mainbody runat="server" />	

    <div class="form-box" id="update-conference-person">

    <div class="row">
		    <div class="col">
                <DI:Input ID="FirstNameInput" Text='<%#CurrentPerson.FirstName %>' CssClass="text" Required="true" StripHtml="true" AutoComplete="true" TypeOfInput="Text" Name="firstname" Title='<%# Translate("/conference/forms/common/firstname") %>' DisplayMessage='<%# Translate("/conference/forms/common/firstname.message") %>' runat="server" />
		    </div>
		    <div class="col">
                <DI:Input ID="LastNameInput" Text='<%#CurrentPerson.LastName %>' CssClass="text" Required="true" StripHtml="true" AutoComplete="true" TypeOfInput="Text" Name="lastname" Title='<%# Translate("/conference/forms/common/lastname") %>' DisplayMessage='<%# Translate("/conference/forms/common/lastname.message") %>' runat="server" />
		    </div>						
	    </div>
					
	    <div class="row">
		    <div class="col">
                <DI:Input ID="CompanyInput" Text='<%#CurrentPerson.Company %>' CssClass="text" Required="true" StripHtml="true" AutoComplete="true" TypeOfInput="Text" Name="comapny" Title='<%# Translate("/conference/forms/registration/company") %>'  DisplayMessage='<%# Translate("/conference/forms/registration/company.message") %>' runat="server" />
		    </div>
		    <div class="col">
                <DI:Input ID="TitleInput" Text='<%#CurrentPerson.Title %>' CssClass="text" Required="true" StripHtml="true" AutoComplete="true" TypeOfInput="Text" Name="title" Title='<%# Translate("/conference/forms/registration/title") %>'  DisplayMessage='<%# Translate("/conference/forms/registration/title.message") %>' runat="server" />
		    </div>						
	    </div>	
					
	    <div class="row">
		    <div class="col">
                <DI:Input ID="OrgNumberInput" Text='<%#CurrentPerson.OrgNo %>' CssClass="text" Required="true" StripHtml="true" AutoComplete="true" TypeOfInput="Telephone" Name="orgnr" Title='<%# Translate("/conference/forms/registration/orgnr") %>'  DisplayMessage='<%# Translate("/conference/forms/registration/orgnr.message") %>'  runat="server" />
		    </div>
		    <div class="col">
                <DI:Input ID="TelephoneInput" Text='<%#CurrentPerson.Phone %>' CssClass="text" Required="true" StripHtml="true" AutoComplete="true" Name="phone" TypeOfInput="Telephone" Title='<%# Translate("/conference/forms/common/phone") %>'  DisplayMessage='<%# Translate("/conference/forms/common/phone.message") %>'  runat="server" />
		    </div>						
	    </div>	

	    <div class="row">
		    <div class="col">
                <DI:Input ID="EmailInput" Text='<%#CurrentPerson.Email %>' CssClass="text" Required="true" StripHtml="true" AutoComplete="true" Name="mail" TypeOfInput="Email" Title='<%# Translate("/conference/forms/common/mail") %>'   DisplayMessage='<%# Translate("/conference/forms/common/mail.message") %>'  runat="server" />
		    </div>
		    <div class="col">
                <DI:Input ID="InvoiceAddressInput" Text='<%#CurrentPerson.InvoiceAddress%>' CssClass="text" Required="true" StripHtml="true" AutoComplete="true" Name="invoiceadd" TypeOfInput="Text" Title='<%# Translate("/conference/forms/registration/invoiceaddress") %>' DisplayMessage='<%# Translate("/conference/forms/registration/invoiceaddress.message") %>' runat="server" />
		    </div>						
	    </div>	
					
	    <div class="row">
		    <div class="col">
                <DI:Input ID="InvoiceReferenceInput" Text='<%#CurrentPerson.InvoiceReference %>' CssClass="text" Required="true" StripHtml="true" AutoComplete="true" Name="invoice" TypeOfInput="Text" Title='<%# Translate("/conference/forms/registration/invoicereference") %>' DisplayMessage='<%# Translate("/conference/forms/registration/invoicereference.message") %>' runat="server" />
		    </div>
		    <div class="col">
			    <div class="small">
                    <DI:Input ID="ZipCodeInput" Text='<%#CurrentPerson.Zip %>' CssClass="text" Required="true" StripHtml="true" AutoComplete="true" Name="zip" TypeOfInput="Text" Title='<%# Translate("/conference/forms/registration/zip") %>' MaxValue="10" DisplayMessage='<%# Translate("/conference/forms/registration/zip.message") %>' runat="server" />
			    </div>
			    <div class="medium">
                    <DI:Input ID="StateInput" Text='<%#CurrentPerson.City %>' CssClass="text" Required="true" StripHtml="true" AutoComplete="true" Name="state" TypeOfInput="Text" Title='<%# Translate("/conference/forms/registration/city") %>' DisplayMessage='<%# Translate("/conference/forms/registration/city.message") %>' runat="server" />
			    </div>							
		    </div>												
	    </div>

        <div class="divider"><hr /></div>

    <asp:Repeater ID="ConferenceRepeater" OnItemDataBound="ConferenceRepeater_ItemDataBound" runat="server" DataSource='<%#GetConferenceEvents() %>'>      
        <ItemTemplate>
                <asp:Repeater ID="EventTimesRepeater" OnItemDataBound="EventTimesRepeater_ItemDatabound" runat="server" DataSource='<%#GetEventTimes((int)Eval("Id")) %>'>
                    <ItemTemplate>
                        <div class="row radiolist">               
                            <h4>
                                <%# ((DagensIndustri.Tools.Classes.Conference.ConferenceEvent)DataBinder.Eval(Container,"Parent.Parent.DataItem")).Name %>
                                <%# ((DagensIndustri.Tools.Classes.Conference.ConferenceEvent)DataBinder.Eval(Container,"Parent.Parent.DataItem")).Date.ToString("yyyy-MM-dd") %>                                
                                <span>
                                    <%# DataBinder.Eval(Container.DataItem, "TimeStart")%> - <%# DataBinder.Eval(Container.DataItem, "TimeEnd")%>
                                </span>
                            </h4>
                            <ul id="list">
                                <asp:Repeater ID="ActivityRepeater" runat="server" DataSource='<%#GetEventActivities((int)Eval("Id")) %>'>
                                    <ItemTemplate>
                                        <li>
                                            <span class="label">
                                                <input type="radio" id="activity_<%#Eval("Id") %>"  
                                                    name="time_<%# ((DagensIndustri.Tools.Classes.Conference.EventTime)DataBinder.Eval(Container,"Parent.Parent.DataItem")).Id %>" 
                                                    value="<%#Eval("Id") %>"
                                                    <%#GetActivityCheckedString((int)Eval("Id")) %>
                                                    <%#GetActivityDisabledString((bool)Eval("IsFull")) %>
                                                     />
                                                <label for="activity_<%#Eval("Id") %>">
                                                    <%#Eval("Name") %>
                                                    <asp:PlaceHolder ID="phFull" runat="server" Visible='<%#(bool)Eval("IsFull") %>'>
                                                        (Fullbokat)
                                                    </asp:PlaceHolder>
                                                </label>
                                                
                                            </span>
                                        </li>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ul>                            
                        </div>           
                    </ItemTemplate>
                </asp:Repeater>        
        </ItemTemplate>
    </asp:Repeater>
    
        <div class="button-wrapper">
            <asp:Button ID="ConferenceUpdateButton" Text="Spara" CssClass="btn" OnClick="PersonUpdateButton_Click" runat="server" /> 
        </div>
    </div>
    </asp:PlaceHolder>
</asp:Content>
