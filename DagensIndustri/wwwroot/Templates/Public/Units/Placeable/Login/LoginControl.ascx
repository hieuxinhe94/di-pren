<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LoginControl.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.Login.LoginControl" %>
<%@ Register TagPrefix="di" TagName="Input" Src="~/Tools/Classes/WebControls/InputWithValidation.ascx" %>
<%@ Register TagPrefix="di" TagName="ForgotPassword" Src="~/Templates/Public/Units/Placeable/Login/ForgotPassword.ascx" %>

<asp:HiddenField ID="ReturnURLHiddenField" runat="server" />

<asp:Login ID="LoginCtrl" OnAuthenticate="OnAuthenticate" OnLoginError="OnLoginError" OnLoggedIn="OnLoggedIn" DisplayRememberMe="true" VisibleWhenLoggedIn="false" FailureText="<%$ Resources: EPiServer, dilogin.error.loginfail %>" runat="server" >
    <LayoutTemplate >
		    <!-- Login -->
		    <div class="section form-box">
                <div class="row">
				    sps
                    <di:Input ID="UserName" Name="user" TypeOfInput="Text" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" Title="<%$ Resources: EPiServer, dilogin.username %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.usernamerequired %>" runat="server" />
			    </div>

			    <div class="row">
                    <di:Input ID="Password" Name="password" TypeOfInput="Password" CssClass="text" Required="true" StripHtml="true" Title="<%$ Resources: EPiServer, dilogin.password %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.passwordrequired %>" runat="server" />
			    </div>
					
			    <div class="button-wrapper">
                    <asp:CheckBox ID="RememberMeCheckBox" CssClass="checkbox" Text="<%$ Resources: EPiServer, dilogin.rememberme %>" TextAlign="Right" runat="server" />
                    <asp:Button ID="LoginButton" CommandName="Login" Text="<%$ Resources: EPiServer, dilogin.login %>" CssClass="btn" runat="server" />

                    <asp:PlaceHolder ID="ForgotPasswordHyperLinkPlaceHolder" Visible="<%# !ShowForgotPassword %>" runat="server">
		                <asp:HyperLink ID="ForgotPasswordHyperLink" NavigateUrl="<%# GetLoginPageUrl() %>" CssClass="more" Text="<%$ Resources: EPiServer, dilogin.forgotpassword %>" runat="server" />
                    </asp:PlaceHolder>
			    </div>
		    </div>
		    <!-- // Login -->
				
            <asp:PlaceHolder ID="ForgotPasswordPlaceHolder" Visible="<%# ShowForgotPassword %>" runat="server">
		        <di:ForgotPassword ID="ForgotPasswordControl" runat="server" />
            </asp:PlaceHolder>
    </LayoutTemplate>
</asp:Login>