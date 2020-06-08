<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="ApsisSubscriptionListGold.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.ApsisSubscriptionListGold" %>
<%@ Register TagPrefix="di" TagName="Heading" Src="~/Templates/Public/Units/Placeable/Heading.ascx" %>
<%@ Register TagPrefix="di" TagName="Input" Src="~/Tools/Classes/WebControls/InputWithValidation.ascx" %>
<%@ Register TagPrefix="di" TagName="UserMessage" Src="~/Templates/Public/Units/Placeable/UserMessage.ascx" %>
<%@ Register TagPrefix="di" TagName="DiGold" Src="~/Templates/Public/Units/Placeable/DiGoldMembershipFlow/DiGoldPromotionalOfferAcceptance.ascx"  %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="HeadingContentPlaceHolder" runat="server">
    <di:Heading ID="Heading1" runat="server" />
</asp:Content>

<asp:Content ID="Content7" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">


    <di:UserMessage ID="UserMessageControl1" runat="server" />

    <asp:MultiView ID="SubscribeMultiView" runat="server" ActiveViewIndex="0">
        
        <asp:View ID="FieldsView" runat="server">
                
                <%#CurrentPage["MainBody"] %>
                
                
                <asp:PlaceHolder ID="PlaceHolderSubscribe" runat="server">

                    <div class="form-box">  							
	                    <div class="section" id="form-check">
                
                            <asp:PlaceHolder ID="GoldFieldsPlaceHolder" runat="server" Visible='<%#!User.IsInRole(DIClassLib.Membership.DiRoleHandler.RoleDiGold) %>'>
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
                                <di:DiGold ID="DiGoldControl" runat="server" />
                            </asp:PlaceHolder>
                            <asp:PlaceHolder ID="JustSubscriptionPlaceHolder" runat="server" Visible='<%#User.IsInRole(DIClassLib.Membership.DiRoleHandler.RoleDiGold) %>'>
                                <div class="row">
			                        <div class="col">
                                        <di:Input ID="ApsisInputEmail" CssClass="text" Required="true" Name="email" TypeOfInput="Email" MinValue="6" AutoComplete="true" Title="<%$ Resources: EPiServer, common.user.personalemailaddress %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.emailrequired %>" runat="server" Visible='<%#!IsSmsList %>' />
                                        <di:Input ID="ApsisInputPhone" CssClass="text" Required="true" Name="phone" TypeOfInput="Telephone" MinValue="7" MaxValue="20" AutoComplete="true" Title="<%$ Resources: EPiServer, common.user.mobilephone %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.mobilephonenumberrequired %>" runat="server" Visible='<%#IsSmsList %>' />
                                    </div>
                                </div>
                            </asp:PlaceHolder>

                            <div class="row row-checkbox">
                                <span class="checkbox">
                                    <di:Input ID="Over20Checkbox" TypeOfInput="CheckBox" Required="true" Name="over20" Title="<%$ Resources: EPiServer, wineclub.form.over20 %>" DisplayMessage="<%$ Resources: EPiServer, wineclub.form.over20required %>" runat="server" />
	                            </span>
                            </div>

                            <div class="button-wrapper">
                                <asp:Button ID="ButtonSubscribe" runat="server" Text="Abonnera" onclick="ButtonSubscribe_Click" />
                            </div>
                        </div>
                    </div>
                </asp:PlaceHolder>

                <asp:PlaceHolder ID="PlaceHolderUnSubscribe" runat="server" Visible="false">
                    <asp:Button ID="ButtonUnSubscribe" runat="server" Text="Avsluta prenumereration" onclick="ButtonUnSubscribe_Click" />
                </asp:PlaceHolder>

        </asp:View>

        <asp:View ID="ThankYouView" runat="server">
             <%#CurrentPage["ThankYou"] %>
        </asp:View>
    </asp:MultiView>

</asp:Content>
