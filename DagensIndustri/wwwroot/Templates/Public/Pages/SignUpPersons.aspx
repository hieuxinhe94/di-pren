<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="SignUpPersons.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.SignUpPersons" %>
<%@ Register TagPrefix="di" TagName="heading" Src="~/Templates/Public/Units/Placeable/Heading.ascx" %>
<%@ Register TagPrefix="di" TagName="mainintro" Src="~/Templates/Public/Units/Placeable/MainIntro.ascx" %>
<%@ Register TagPrefix="di" TagName="mainbody" Src="~/Templates/Public/Units/Placeable/MainBody.ascx" %>
<%@ Register TagPrefix="DI" TagName="Input" Src="~/Tools/Classes/WebControls/InputWithValidation.ascx" %>
<%@ Register Tagprefix="uc1" Tagname="FriendForm" src="~/Templates/Public/Units/Placeable/SignUpNoPay/FriendForm.ascx" %>
<%@ Register TagPrefix="di" TagName="UserMessage" Src="~/Templates/Public/Units/Placeable/UserMessage.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadingContentPlaceHolder" runat="server">
    <di:heading ID="Heading1" runat="server" />
    <script type="text/javascript" src="/Templates/Public/js/PreventDoublePost.js"></script>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

    <di:UserMessage ID="UserMessageControl" runat="server" />

    <di:mainintro ID="Mainintro1" runat="server" />

    <di:mainbody ID="Mainbody1" runat="server" />		
    
    <asp:PlaceHolder ID="PlaceHolderNeedLogin" runat="server" Visible='<%#(SignUpAccess == ACCESS_DIGOLD || SignUpAccess == ACCESS_SUBSCRIBERS) && !System.Web.HttpContext.Current.User.Identity.IsAuthenticated %>'>
        <div>
            <p> <%# string.Format("För att få tillgång till evenemanget behöver du vara inloggad <a href='{0}'>Klicka här</a> för att logga in.", EPiFunctions.GetLoginPageUrl(CurrentPage))%> </p>            
        </div>
        
    </asp:PlaceHolder>


    <asp:PlaceHolder ID="PlaceHolderSignUpForm" runat="server">
    
        <asp:Label ID="LabelHeaderFormCust" runat="server" Text="Label"></asp:Label>

         <div class="form-box">
	        <div class="section">	
                <div class="row">
			        <div class="col">
                        <di:Input ID="FirstNameInput" CssClass="text" Required="true" Name="firstname" TypeOfInput="Text" MaxValue="28" AutoComplete="true" Title="<%$ Resources: EPiServer, common.user.firstname %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.firstnamerequired %>" runat="server" />
			        </div>
			        <div class="col">
                        <di:Input ID="LastNameInput" CssClass="text" Required="true" Name="lastname" TypeOfInput="Text" MaxValue="28" AutoComplete="true" Title="<%$ Resources: EPiServer, common.user.lastname %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.lastnamerequired %>" runat="server" />
			        </div>
		        </div>
		        <div class="row">
			        <div class="col">
                        <di:Input ID="EmailInput" CssClass="text" Required="true" Name="email" TypeOfInput="Email" MinValue="6" AutoComplete="true" Title="<%$ Resources: EPiServer, common.user.personalemailaddress %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.emailrequired %>" runat="server" />
			        </div>
			        <div class="col">
                        <di:Input ID="PhoneInput" CssClass="text" Required="true" Name="phone" TypeOfInput="Telephone" MinValue="7" MaxValue="20" AutoComplete="true" Title="<%$ Resources: EPiServer, common.user.mobilephone %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.mobilephonenumberrequired %>" runat="server" />
			        </div>
		        </div>	
                <div class="row" id="socialSecurityRow" runat="server" visible='<%#SignUpAccess == ACCESS_DIGOLD %>' >
			        <div class="col">
                        <di:Input ID="SocialSecurityNoInput" CssClass="text" Required="true" Name="socialsecrityno" MinValue="8" MaxValue="8" TypeOfInput="SocialSecurityNumber" AutoComplete="true" Title="<%$ Resources: EPiServer, common.user.socialsecuritynumberformat %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.socialsecuritynumberrequired %>" runat="server"   />
			        </div>
		        </div>

                <asp:PlaceHolder ID="PlaceHolderCheckBoxGoldTerms" runat="server" Visible='<%#SignUpAccess == ACCESS_DIGOLD && !UserIsGoldMember %>'>
                    <div class="row row-checkbox">
                        <span class="checkbox">
                            <di:Input ID="TermsAcceptedInput" TypeOfInput="CheckBox" Required="true" Name="terms" Title="<%# GetGoldTerms() %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.agreementoftermsrequired %>" runat="server" />
	                    </span>
                    </div>
                </asp:PlaceHolder>


            </div>
        </div>

        <asp:Label ID="LabelHeaderFriends" runat="server" visible='<%#FriendNames.Count() > 0 %>' Text="<%$ Resources: EPiServer, withfriendsjoingold.headerfriends %>"></asp:Label>

        <div class="form-box" id="FormFriends" runat="server" visible='<%#FriendNames.Count() > 0 %>' >
            <div class="section">
                <asp:Repeater ID="RepeaterFriends" runat="server" DataSource='<%#FriendNames %>'>
                    <ItemTemplate>
                        <div class="form-box">
	                        <div class="section">										
	                            <div class="row">
		                            <div class="col">
                                        <label for="friend_firstname_<%#Container.ItemIndex %>"><%# Translate("/common/forms/firstname") %></label>
                                        <input type="text" name="friend_firstname_<%#Container.ItemIndex %>" class="text" value="<%#Eval("FirstName") %>" />                                        
		                            </div>
		                            <div class="col">
                                        <label for="friend_lastname_<%#Container.ItemIndex %>"><%# Translate("/common/forms/lastname") %></label>
                                        <input type="text" name="friend_lastname_<%#Container.ItemIndex %>" class="text" value="<%#Eval("LastName") %>" />
		                            </div>						
	                            </div>
                            </div>				
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
           
            
        <asp:Panel ID="PanelEditButtons" Visible='<%#(CurrentPerson != null) %>' runat="server">
             <div style="float:left; width:400px;">
                <asp:Button ID="ButtonUpdate" CssClass="btn" Text="Uppdatera bokning" OnClick="ButtonUpdate_Click" OnClientClick="return jsPreventDoublePost('divSubmitBtn', 'divFormSent');" runat="server" />
                <br /><br />
                (Avboka gäst genom att ta bort gästens för- och efternamn)
            </div>

            <div style="float:right; text-align:right;">
                <asp:Button ID="ButtonCancel" CssClass="btn" Text="Avboka hela sällskapet" OnClick="ButtonCancel_Click" OnClientClick="return jsPreventDoublePost('divSubmitBtn', 'divFormSent');" runat="server" />
            </div>
        </asp:Panel>
        
        <asp:Button ID="ButtonInsert" CssClass="btn" Text="Skicka" OnClick="ButtonInsert_Click" Visible='<%#(CurrentPerson == null) %>' OnClientClick="return jsPreventDoublePost('divSubmitBtn', 'divFormSent');" runat="server" />
	        

        <div id="divFormSent" style="float:right; visibility:hidden;">
            <img src="/Templates/Public/Images/ajax-loader.gif" alt="" />
            <i>&nbsp;<asp:Literal ID="Literal1" Text="<%$ Resources: EPiServer, common.sendingform %>" runat="server" /></i>
        </div>
    </asp:PlaceHolder>
    

</asp:Content>

