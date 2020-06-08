<%@ Page Language="C#" MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="ConferenceRoom.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.ConferenceRoom" %>
<%@ Register TagPrefix="DI" TagName="Heading" Src="~/Templates/Public/Units/Placeable/Heading.ascx" %>
<%@ Register tagname="UserMessage" tagprefix="uc1" src="../Units/Placeable/UserMessage.ascx" %>
<%@ Register tagname="MainIntro" tagprefix="uc3" src="../Units/Placeable/MainIntro.ascx" %>
<%@ Register tagname="MainBody" tagprefix="uc2" src="../Units/Placeable/MainBody.ascx" %>
<%@ Register TagPrefix="DI" TagName="Input" Src="~/Tools/Classes/WebControls/InputWithValidation.ascx" %>
<%@ Register TagPrefix="DI" TagName="UserMessage" Src="~/Templates/Public/Units/Placeable/UserMessage.ascx" %>
<%@ Register TagPrefix="di" TagName="sidebar" Src="~/Templates/Public/Units/Placeable/SideBarBoxes/SideBar.ascx" %>
<%@ Register TagPrefix="di" TagName="googlemaps" src="~/Templates/Public/Units/Placeable/SideBarBoxes/GoogleMaps.ascx" %>


<asp:Content ID="Content4" ContentPlaceHolderID="HeadingContentPlaceHolder" runat="server">
    <DI:Heading ID="HeadingControl" runat="server"/>
</asp:Content>


<asp:Content ID="Content6" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    
    <uc1:UserMessage ID="UserMessage1" runat="server" />
    
	<div id="content">
        <asp:PlaceHolder ID="PlaceHolderForm" runat="server">
		    
		    <p class="intro">
			    <uc3:MainIntro ID="MainIntro1" runat="server" />
		    </p>

		    <p>
			    <uc2:MainBody ID="MainBody1" runat="server" />
		    </p>

            <p></p>
 
            <asp:PlaceHolder ID="PlaceHolderThankYou" runat="server">
                <di:UserMessage ID="UserMessageControl1" runat="server" />
            </asp:PlaceHolder>

            <asp:PlaceHolder ID="PlaceHolderBookingForm" runat="server">

                <div class="form-nav">
  		        <ul>
  			        <li class="current">
                        <asp:HyperLink ID="HyperLinkConfRoom" NavigateUrl="#conferenceroom-registration" runat="server">
                            <EPiServer:Translate ID="Translate1" Text="/confroom/tabconf" runat="server" />
                        </asp:HyperLink>
                    </li>

                    <asp:PlaceHolder ID="PlaceHolderLoungeTab" runat="server">
                        <li>
                            <asp:HyperLink ID="HyperLinkLounge" NavigateUrl="#conferenceroom-lounge" runat="server">
                                <EPiServer:Translate ID="Translate2" Text="/confroom/tablounge" runat="server" />
                            </asp:HyperLink>
                        </li>
                    </asp:PlaceHolder>

  		        </ul>
  		        <p class="required">= obligatoriska uppgifter</p>
  	        </div>
                <div class="form-box">

                <div class="section" id="conferenceroom-registration">

			        <div class="row">
			            <div class="col">
                            <DI:Input ID="firstName" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" TypeOfInput="Text" Name="firstname" Title='<%# Translate("/common/user/firstname") %>' DisplayMessage='<%# Translate("/common/validation/firstnamerequired") %>' runat="server" />
			            </div>

			            <div class="col">
                            <DI:Input ID="lastName" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" TypeOfInput="Text" Name="lastname" Title='<%# Translate("/common/user/lastname") %>' DisplayMessage='<%# Translate("/common/validation/lastnamerequired") %>' runat="server" />
			            </div>						
		            </div>

			        <div class="row">
			            <div class="col">
				            <DI:Input ID="email" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" TypeOfInput="Email" Name="email" Title='<%# Translate("/common/user/emailaddress") %>' DisplayMessage='<%# Translate("/common/validation/emailrequired") %>' runat="server" />
			            </div>
			            <div class="col">
                            <DI:Input ID="phone" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" Name="phone" TypeOfInput="Telephone" Title='<%# Translate("/common/user/mobilephone") %>'  DisplayMessage='<%# Translate("/common/validation/phonenumberrequired") %>' runat="server" />
			            </div>
		            </div>	
	
                    <div class="row">
			            <div class="col">
				            <div class="medium">
					            <DI:Input ID="date" CssClass="text date" Required="true" StripHtml="true" AutoComplete="true" Name="date" TypeOfInput="Date" MinValue="<%#GetMinDate()%>" Title='<%# Translate("/confroom/selectdate") %>' DisplayMessage='<%# Translate("/confroom/selectdatemess") %>' runat="server" />
				            </div>
			            </div>
			            <div class="col">
				            <div class="medium">
					            <DI:Input ID="time" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" Name="time" TypeOfInput="Time" Title='<%# Translate("/confroom/selecttime") %>'  DisplayMessage='<%# Translate("/confroom/selecttimemess") %>' runat="server" />
				            </div>
			            </div>						
		            </div>
					
			        <div class="row">
			            <div class="col">
				            <div class="small">
					            <DI:Input ID="numHours" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" Name="hours" TypeOfInput="Numeric" Title='<%# Translate("/confroom/numhours") %>'  DisplayMessage='<%# Translate("/confroom/numhoursmess") %>' MinValue="0" MaxValue="99" runat="server" />
				            </div>
			            </div>
			            <div class="col">	
				            <div class="small">
                                <DI:Input ID="numPersons" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" Name="numPersons" TypeOfInput="Numeric" Title='<%# Translate("/confroom/numpers") %>'  DisplayMessage='<%# Translate("/confroom/numpersmess") %>' MinValue="1" MaxValue="999" runat="server" />
				            </div>							
			            </div>		
		            </div>
	
                    <div class="button-wrapper">
                        <asp:Button ID="ButtonSend" CssClass="btn" Text="<%$ Resources: EPiServer, common.send %>" OnClick="Send_Click" runat="server" />
                    </div>


		        </div>
		    
                <asp:PlaceHolder ID="PlaceHolderLoungeInfo" runat="server">
                    <div class="section" id="conferenceroom-lounge">
					    <p><img src='<%=(string)CurrentPage["LoungeImage"]%>' alt="" /></p>
					    <%=(string)CurrentPage["LoungeText"]%>
					
					    <div class="row">
						    <div class="col">
 						        <strong class="label"><EPiServer:Translate ID="Translate3" Text="/confroom/yourloungecode" runat="server" /></strong>
                                <asp:TextBox CssClass="text" ID="TextBoxLoungeCode" runat="server"></asp:TextBox>
						    </div>
					    </div>
 
					    <div class="button-wrapper">
                            <a href="#" class="btn print"><span><%# Translate("/common/print") %></span></a>
                        </div>
				    </div>
                </asp:PlaceHolder>
            
	        </div>

            </asp:PlaceHolder>

            <asp:HyperLink ID="ButtonLogin" runat="server" CssClass="btn ajax" NavigateUrl="#membership-required" Visible="false">
                <span>Boka nu</span>
            </asp:HyperLink>
        </asp:PlaceHolder>
    </div>
</asp:Content>

<asp:Content ContentPlaceHolderID="RightColumnPlaceHolder" runat="server">
    <di:sidebar runat="server" />
    <di:googlemaps runat="server" />
</asp:Content>