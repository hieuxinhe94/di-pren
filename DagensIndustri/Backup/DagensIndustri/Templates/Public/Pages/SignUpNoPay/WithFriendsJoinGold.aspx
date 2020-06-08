<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="WithFriendsJoinGold.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.SignUpNoPay.WithFriendsJoinGold" %>
<%@ MasterType virtualpath="~/Templates/Public/MasterPages/MasterPage.Master" %>
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

<asp:Content ID="MainContentPlaceHolder1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

    <di:UserMessage ID="UserMessageControl" runat="server" />
    
    <di:mainintro ID="Mainintro1" runat="server" />
    <di:mainbody ID="Mainbody1" runat="server" />
    
    <asp:Literal ID="LiteralDoLoginLink" Visible="false" runat="server"></asp:Literal>


    <asp:PlaceHolder ID="PlaceHolderForm" runat="server">
        <p><b><asp:Label ID="LabelFormHeader" runat="server" Text="Label"></asp:Label></b><br /></p>
        
        <div class="form-nav">
  	        <ul>
  		        <li class="current">
                    <a href="javascript:void(0);"><EPiServer:Translate ID="Translate1" Text="/withfriendsjoingold/goldformheader" runat="server" /></a>
                </li>
  	        </ul>
        </div> 	
        
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
                <div class="row">
			        <div class="col">
                        <di:Input ID="SocialSecurityNoInput" CssClass="text" Required="true" Name="socialsecrityno" MinValue="8" MaxValue="8" TypeOfInput="SocialSecurityNumber" AutoComplete="true" Title="<%$ Resources: EPiServer, common.user.socialsecuritynumberformat %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.socialsecuritynumberrequired %>" runat="server" />
			        </div>
		        </div>

                <asp:PlaceHolder ID="PlaceHolderCheckBoxGoldTerms" runat="server">
                    <div class="row row-checkbox">
                        <span class="checkbox">
                            <di:Input ID="TermsAcceptedInput" TypeOfInput="CheckBox" Required="true" Name="terms" Title="<%# GetGoldTerms() %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.agreementoftermsrequired %>" runat="server" />
	                    </span>
                    </div>
                </asp:PlaceHolder>

            </div>
        </div>

        <asp:Label ID="LabelHeaderFriends" Visible="false" runat="server" Text="<%$ Resources: EPiServer, withfriendsjoingold.headerfriends %>"></asp:Label>
        
        <uc1:FriendForm ID="FriendForm1" Visible="false" runat="server" />
        <uc1:FriendForm ID="FriendForm2" Visible="false" runat="server" />
        <uc1:FriendForm ID="FriendForm3" Visible="false" runat="server" />
        <uc1:FriendForm ID="FriendForm4" Visible="false" runat="server" />
        <uc1:FriendForm ID="FriendForm5" Visible="false" runat="server" />
        <uc1:FriendForm ID="FriendForm6" Visible="false" runat="server" />
        <uc1:FriendForm ID="FriendForm7" Visible="false" runat="server" />
        <uc1:FriendForm ID="FriendForm8" Visible="false" runat="server" />
        <uc1:FriendForm ID="FriendForm9" Visible="false" runat="server" />
        <uc1:FriendForm ID="FriendForm10" Visible="false" runat="server" />


        <div id="divSubmitBtn" class="button-wrapper">
            <asp:Button ID="ButtonSave" CssClass="btn" Text="Skicka" OnClick="ButtonSave_Click" OnClientClick="return jsPreventDoublePost('divSubmitBtn', 'divFormSent');" runat="server" />
	    </div>

        <div id="divFormSent" style="float:right; visibility:hidden;">
            <img src="/Templates/Public/Images/ajax-loader.gif" alt="" />
            <i>&nbsp;<asp:Literal ID="Literal1" Text="<%$ Resources: EPiServer, common.sendingform %>" runat="server" /></i>
        </div>

        

        <div>
            <p>
                <i>
                    <br />
                    Evenemanget är kostnadsfritt men anmälan är bindande. Återbud ska ske senast dagen innan till 
                    <a href='mailto:returmarknad@di.se'>returmarknad@di.se</a>, annars debiteras du 200 kronor per outnyttjad plats.
                </i>
            </p>
        </div>

    </asp:PlaceHolder>


</asp:Content>

<%--<asp:Content ID="Content8" ContentPlaceHolderID="RightColumnPlaceHolder" runat="server">
</asp:Content>--%>

