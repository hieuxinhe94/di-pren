<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TopLogin.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Static.TopLogin" %>
<%@ Register TagPrefix="DI" TagName="Input" Src="~/Tools/Classes/WebControls/InputWithValidation.ascx" %>

<!-- Login form -->

<asp:LoginView ID="LoginView" runat="server">

    <AnonymousTemplate>
        <div class="login">
            <a href="#"><EPiServer:Translate Text="/dilogin/login" runat="server" /></a>

            <!-- Login form -->
            <asp:Login ID="LoginControl" OnAuthenticate="OnAuthenticate" OnLoginError="OnLoginError" OnLoggedIn="OnLoggedIn" DisplayRememberMe="true" VisibleWhenLoggedIn="false" FailureText="<%$ Resources: EPiServer, dilogin.error.loginfail %>" runat="server" >
                <LayoutTemplate >		            
		            <div id="header-form-login" class="form-login">
                    
			            <div class="wrapper">
				            <div class="form">
                                <div class="row">
                                    <DI:Input ID="UserName" Name="user" TypeOfInput="Text" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" Title="<%$ Resources: EPiServer, dilogin.username %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.usernamerequired %>" runat="server" />
                                </div>
                                
                                <div class="row">
                                    <DI:Input ID="Password" Name="password" TypeOfInput="Password" CssClass="text" Required="true" StripHtml="true" Title="<%$ Resources: EPiServer, dilogin.password %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.passwordrequired %>" runat="server" />
                                </div>

                                <div class="row">
				                    <asp:CheckBox ID="RememberMeCheckBox" CssClass="checkbox" Text="<%$ Resources: EPiServer, dilogin.rememberme %>" TextAlign="Right" runat="server" />
				                    <asp:Button ID="LoginButton" CommandName="Login" Text="<%$ Resources: EPiServer, dilogin.login %>" CssClass="btn" runat="server" />
                                </div>

                                <asp:HyperLink ID="ForgotPasswordHyperLink" NavigateUrl='<%# EPiFunctions.GetLoginPage(CurrentPage).LinkURL %>' CssClass="more" runat="server" Text="<%$ Resources: EPiServer, dilogin.forgotpassword %>" />
				            </div> 
				
				            <!-- Banner -->
                            <asp:PlaceHolder ID="NotLoggedInBannerPlaceHolder" Visible='<%# EPiFunctions.GetLoginBannerVisibility(CurrentPage) %>' runat="server">
                            <span class="banner">
                                <asp:PlaceHolder ID="BannerLinkPlaceHolder" runat="server">
                                <%--<asp:HyperLink ID="BannerHyperLink" NavigateUrl='<%# EPiFunctions.GetLoginBannerLink(CurrentPage) %>' runat="server">
                                    <asp:Image ID="BannerImage" ImageUrl='<%# EPiFunctions.GetLoginBanner(CurrentPage) %>' AlternateText="banner-login" Width="234" Height="103" runat="server"/>
                                </asp:HyperLink>--%>
                                </asp:PlaceHolder>
                            </span>
                            </asp:PlaceHolder>
				            <!-- // Banner -->
			            </div>
		            </div>                    
                </LayoutTemplate>
            </asp:Login>
        </div>
        <!-- // Login form --> 

    </AnonymousTemplate>

    <LoggedInTemplate>
        <!-- Account -->
		<div class="account">
			<a href="#"><EPiServer:Translate Text="/dilogin/loggedin/myaccount" runat="server" /></a>
            
			<div class="subnav">
                <ul>
                    <asp:Repeater ID="PageListRepeater" OnItemDataBound="PageListRepeater_ItemDataBound" runat="server">
                    <HeaderTemplate>
                        
                    </HeaderTemplate>

                    <ItemTemplate>
                        <li id="ListItem" runat="server">
                            <asp:HyperLink ID="PageHyperlink" NavigateUrl="<%# ((EPiServer.Core.PageData)Container.DataItem).LinkURL%>" Text='<%# ((EPiServer.Core.PageData)Container.DataItem)["Heading"] ?? ((EPiServer.Core.PageData)Container.DataItem).PageName%>' runat="server" />
                        </li>
                    </ItemTemplate>
                            
                    <FooterTemplate>

                    </FooterTemplate>
                </asp:Repeater>

                    <li><asp:LinkButton ID="LogoutLinkButton" Text="<%$ Resources: EPiServer, dilogin.logout %>" runat="server" OnClick="Logout_Click" /></li>
                    <asp:PlaceHolder ID="BannerPlaceHolder" Visible='<%# EPiFunctions.GetLoginBannerVisibility(CurrentPage) %>' runat="server">
                        <li class="banner">
                            <asp:PlaceHolder ID="LoggedInBannerLinkPlaceHolder" runat="server">
                            <%--<a href='<%= EPiFunctions.GetLoginBannerLink(CurrentPage) %>'>--%>
                            <%--<asp:HyperLink NavigateUrl='<%= EPiFunctions.GetLoginBannerLink(CurrentPage) %>' runat="server">
                                <asp:Image ID="LoggedInBannerImage" ImageUrl='<%# EPiFunctions.GetLoginBanner(CurrentPage) %>' Width="234" Height="103" runat="server"/>
                            </asp:HyperLink>--%>
                            <%-- </a>--%>
                            </asp:PlaceHolder>
                        </li>
                    </asp:PlaceHolder>
                </ul>
			</div>	
		</div>			
		<!-- // Account -->
    </LoggedInTemplate>
</asp:LoginView>	
<!-- // Login form --> 