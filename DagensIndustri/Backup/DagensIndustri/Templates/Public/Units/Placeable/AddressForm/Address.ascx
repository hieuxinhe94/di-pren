<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Address.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.AddressForm.Address" %>
<%@ Register TagPrefix="DI" TagName="Input" Src="~/Tools/Classes/WebControls/InputWithValidation.ascx" %>

<script type="text/javascript" src="/Templates/Public/js/PreventDoublePost.js"></script>


<div class="form-nav">
  	<ul>
  		<li class="current">
            <a href="#form-street"><%= Translate("/dicampaign/tabs/street") %></a>
        </li>
  		<li>
            <a href="#form-box"><%= Translate("/dicampaign/tabs/box") %></a>
        </li>						
  	</ul>
  	<p class="required"><%= Translate("/dicampaign/forms/mandatoryinformation")%></p>
</div>
  		
<div class="form-box">    				
    
    <!-- Street -->
    <div class="section" id="form-street">
	    
        <asp:PlaceHolder ID="PlaceHolderNameStr" runat="server">
            <div class="row">
		        <div class="col">
                    <DI:Input ID="FirstNameStr" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" TypeOfInput="Text" Name="firstname" Title='<%# Translate("/dicampaign/forms/firstname") %>' DisplayMessage='<%# Translate("/dicampaign/forms/firstname.message") %>' runat="server" />
		        </div>
		        <div class="col">
                    <DI:Input ID="LastNameStr" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" TypeOfInput="Text" Name="lastname" Title='<%# Translate("/dicampaign/forms/lastname") %>' DisplayMessage='<%# Translate("/dicampaign/forms/lastname.message") %>' runat="server" />
		        </div>						
	        </div>
	        <div class="divider"><hr /></div>
		</asp:PlaceHolder>


	    <div class="row">
		    <div class="col">
                <DI:Input ID="CareOfStr" CssClass="text" StripHtml="true" AutoComplete="true" TypeOfInput="Text" Name="co" Title='<%# Translate("/dicampaign/forms/careof") %>' runat="server" />
            </div>
		    <div class="col">
                <asp:PlaceHolder ID="PlaceHolderCompStr" runat="server">
                    <DI:Input ID="CompanyStr" CssClass="text" StripHtml="true" AutoComplete="true" TypeOfInput="Text" Name="comapny" Title='<%# Translate("/dicampaign/forms/company") %>' runat="server" />
                </asp:PlaceHolder>
		    </div>						
	    </div>						
					
	    <div class="row">
		    <div class="col">
                <DI:Input ID="StreetStr" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" TypeOfInput="Text" Name="street" Title='<%# Translate("/dicampaign/forms/street") %>' DisplayMessage='<%# Translate("/dicampaign/forms/street.message") %>' runat="server" />
		    </div>
		    <div class="col">
			    <div class="small">
                    <DI:Input ID="StreetNumStr" CssClass="text" StripHtml="true" AutoComplete="true" TypeOfInput="Text" Name="number" Title='<%# Translate("/dicampaign/forms/streetnumber") %>'  DisplayMessage='<%# Translate("/dicampaign/forms/streetnumber.message") %>' runat="server" />
			    </div>
			    <div class="small">
                    <DI:Input ID="EntranceStr" CssClass="text" StripHtml="true" AutoComplete="true" TypeOfInput="Text" Name="door" Title='<%# Translate("/dicampaign/forms/entrance") %>' runat="server" />
			    </div>							
		    </div>						
	    </div>	
					
	    <div class="row">
		    <div class="col">
			    <div class="small">
                    <DI:Input ID="StairsStr" CssClass="text" StripHtml="true" AutoComplete="true" Name="stairs" Title='<%# Translate("/dicampaign/forms/floor") %>' runat="server" />
			    </div>
			    <div class="small">
                    <DI:Input ID="ApartmentNumStr" CssClass="text" StripHtml="true" AutoComplete="true" Name="appartment" Title='<%# Translate("/dicampaign/forms/flatnumber") %>' runat="server" />
			    </div>							
		    </div>
		    <div class="col">
			    <div class="small">
                    <DI:Input ID="ZipStr" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" TypeOfInput="ZipCode" Name="zip" MaxValue="5" Title='<%# Translate("/dicampaign/forms/zip") %>' DisplayMessage='<%# Translate("/dicampaign/forms/zip.message") %>' runat="server" />
			    </div>
			    <div class="medium">
                    <DI:Input ID="CityStr" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" TypeOfInput="Text" Name="state" Title='<%# Translate("/dicampaign/forms/city") %>' DisplayMessage='<%# Translate("/dicampaign/forms/city.message") %>' runat="server" />
			    </div>							
		    </div>												
	    </div>
        
        
        <asp:PlaceHolder ID="PlaceHolderDatesStr" runat="server">
            <div class="row">
			    <div class="col">
				    <div class="medium">
					    <DI:Input ID="Date1Str" CssClass="text date" Required="true" Name="date-temp-from" TypeOfInput="Date" Title="Från och med <i>(YYYY-MM-DD)</i>" DisplayMessage="Ange datum" runat="server" />
				    </div>								
			    </div>
                <asp:PlaceHolder ID="PlaceHolderDate2Str" runat="server">
			        <div class="col">
				        <div class="medium">						
                            <DI:Input ID="Date2Str" CssClass="text date" Required="true" Name="date-temp-to" TypeOfInput="Date" Title="Till och med <i>(YYYY-MM-DD)</i>" DisplayMessage="Ange datum" runat="server" />
				        </div>								
			        </div>								
                </asp:PlaceHolder>
		    </div>
        </asp:PlaceHolder>

        <asp:PlaceHolder ID="PlaceHolderBoxer" runat="server">
            <div class="row" style="height:210px;">
                <div style="background-color:#f1f1f1; padding:10px; width:560px;">
                    <img src="http://www.boxer.se/Templates2/lib/img/logo-boxer-print.png" alt="" border="0" />
                    <p style="margin-top:10px; margin-bottom:10px;">
                        <b>Gratis boxer i sommar!</b><br />
                        Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam nec lorem nec urna adipiscing pretium. 
                        Donec ipsum lacus, venenatis convallis pellentesque vitae, varius in felis. Maecenas a adipiscing dolor. 
                        Nulla non arcu ut nisl vehicula blandit. Aliquam scelerisque consequat nisi. Etiam feugiat, leo eget condimentum pretium, purus lorem interdum lectus.
                        <br />
                    </p>
                    <asp:CheckBox ID="CheckBoxBoxer" runat="server" /> Jag vill ha och jag godkänner <a href="" target="_blank">villkoren</a>
                </div>
            </div>
        </asp:PlaceHolder>

	    <div class="button-wrapper">

            <div id="divSubmitBtn">
                <asp:Button ID="StreetFormButton" CssClass="btn" Text="Spara"  OnClick="StreetFormButton_Click" OnClientClick="return jsPreventDoublePost('divSubmitBtn', 'divFormSent');" runat="server" />
	        </div>
            
            <div id="divFormSent" style="float:right; visibility:hidden;">
                <img src="/Templates/Public/Images/loader.gif" alt="" />
                <i>&nbsp;<asp:Literal ID="Literal1" Text="<%$ Resources: EPiServer, common.sendingform %>" runat="server" /></i>
            </div>

	    </div>
    </div>
    <!-- /Street -->


    <!-- Box -->
    <div class="section" id="form-box">
					
	    <asp:PlaceHolder ID="PlaceHolderNameBox" runat="server">
            <div class="row">
		        <div class="col">
                    <DI:Input ID="FirstNameBox" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" TypeOfInput="Text" Name="firstname" Title='<%# Translate("/dicampaign/forms/firstname") %>' DisplayMessage='<%# Translate("/dicampaign/forms/firstname.message") %>' runat="server" />
		        </div>
		        <div class="col">
                    <DI:Input ID="LastNameBox" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" TypeOfInput="Text" Name="lastname" Title='<%# Translate("/dicampaign/forms/lastname") %>' DisplayMessage='<%# Translate("/dicampaign/forms/lastname.message") %>' runat="server" />
		        </div>						
	        </div>
	        <div class="divider"><hr /></div>
        </asp:PlaceHolder>
		

        <asp:PlaceHolder ID="PlaceHolderCompBox" runat="server">
	        <div class="row">
		        <div class="col">
                    <DI:Input ID="CompanyBox" CssClass="text" StripHtml="true" AutoComplete="true" TypeOfInput="Text" Name="comapny" Title='<%# Translate("/dicampaign/forms/company") %>' runat="server" />
		        </div>									
	        </div>					
        </asp:PlaceHolder>

					
	    <div class="row">
		    <div class="col">
                <DI:Input ID="StopOrBox" Required="true" AutoComplete="true" StripHtml="true" Name="box" CssClass="text" TypeOfInput="Text" Title='<%# Translate("/dicampaign/forms/postbox") %>' DisplayMessage='<%# Translate("/dicampaign/forms/postbox.message") %>' runat="server" />
		    </div>
		    <div class="col">
			    <div class="small">
                    <DI:Input ID="BoxNum" Required="true" AutoComplete="true" StripHtml="true" Name="number" CssClass="text" TypeOfInput="Text" Title='<%# Translate("/dicampaign/forms/streetnumber") %>' DisplayMessage='<%# Translate("/dicampaign/forms/streetnumber.message") %>' runat="server" />
			    </div>
		    </div>						
	    </div>	
					
	    <div class="row">
		    <div class="col">
			    <div class="small">
				    <DI:Input ID="ZipBox" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" TypeOfInput="ZipCode" Name="zip" Title="Postnummer" DisplayMessage="Postnummret är ogiltigt." runat="server" />
			    </div>
			    <div class="medium">
                    <DI:Input ID="CityBox" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" TypeOfInput="Text" Name="state" Title="Ort" DisplayMessage="Fyll i en ort." runat="server" />
			    </div>							
		    </div>										
	    </div>					
					
        <asp:PlaceHolder ID="PlaceHolderDatesBox" runat="server">
            <div class="row">
			    <div class="col">
				    <div class="medium">
					    <DI:Input ID="Date1Box" CssClass="text date" Required="true" Name="date-temp-from" TypeOfInput="Date" Title="Från och med <i>(YYYY-MM-DD)</i>" DisplayMessage="Ange datum" runat="server" />
				    </div>								
			    </div>
                <asp:PlaceHolder ID="PlaceHolderDate2Box" runat="server">
			        <div class="col">
				        <div class="medium">						
                            <DI:Input ID="Date2Box" CssClass="text date" Required="true" Name="date-temp-to" TypeOfInput="Date" Title="Till och med <i>(YYYY-MM-DD)</i>" DisplayMessage="Ange datum" runat="server" />
				        </div>								
			        </div>								
                </asp:PlaceHolder>
		    </div>
        </asp:PlaceHolder>

	    <div class="button-wrapper">

            <div id="divSubmitBtn2">
                <asp:Button ID="BoxFormButton" CssClass="btn" Text="Spara" OnClick="BoxFormButton_Click" OnClientClick="return jsPreventDoublePost('divSubmitBtn2', 'divFormSent2');" runat="server" />
	        </div>
            
            <div id="divFormSent2" style="float:right; visibility:hidden;">
                <img src="/Templates/Public/Images/loader.gif" alt="" />
                <i>&nbsp;<asp:Literal ID="Literal2" Text="<%$ Resources: EPiServer, common.sendingform %>" runat="server" /></i>
            </div>

	    </div>							

    </div>
    <!-- /Box -->			

</div>

