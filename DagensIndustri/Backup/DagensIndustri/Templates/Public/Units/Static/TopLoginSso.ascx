<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TopLoginSso.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Static.TopLoginSso" %>
<%@ Register TagPrefix="DI" TagName="Input" Src="~/Tools/Classes/WebControls/InputWithValidation.ascx" %>



<%--<%# !HttpContext.Current.User.Identity.IsAuthenticated %>--%>
<asp:PlaceHolder ID="PlaceHolderNotLoggedIn" runat="server">
    <div class="account">
        <a href="#">Logga in</a>
        <div class="subnav">
            <ul>
                <li>
                    <div style="margin-left:10px; margin-right:10px; margin-top:10px;">Ny inloggning till Di på nätet</div> 
                    <asp:HyperLink ID="HyperLinkToLogInPage" runat="server">Läs mer här</asp:HyperLink>
                </li>
                <li>
                    <asp:HyperLink ID="HyperLinkToLogInBonDig" runat="server">Logga in med ditt Di-konto</asp:HyperLink>
                </li>
            </ul>
        </div>
    </div>
</asp:PlaceHolder>


<asp:PlaceHolder ID="PlaceHolderLoggedIn" runat="server">
    <div class="account">
		<a href="#"><EPiServer:Translate ID="Translate2" Text="/dilogin/loggedin/myaccount" runat="server" /></a>
            
		<div class="subnav">
            <ul>
                <asp:Repeater ID="PageListRepeater" OnItemDataBound="PageListRepeater_ItemDataBound" runat="server">
                    <HeaderTemplate></HeaderTemplate>
                    <ItemTemplate>
                        <li id="ListItem" runat="server">
                            <asp:HyperLink ID="PageHyperlink" NavigateUrl="<%# ((EPiServer.Core.PageData)Container.DataItem).LinkURL%>" Text='<%# ((EPiServer.Core.PageData)Container.DataItem)["Heading"] ?? ((EPiServer.Core.PageData)Container.DataItem).PageName%>' runat="server" />
                        </li>
                    </ItemTemplate>
                    <FooterTemplate></FooterTemplate>
                </asp:Repeater>

                <li><asp:LinkButton ID="LogoutLinkButton" Text="<%$ Resources: EPiServer, dilogin.logout %>" runat="server" OnClick="Logout_Click" /></li>
                <asp:PlaceHolder ID="BannerPlaceHolder" Visible='<%# EPiFunctions.GetLoginBannerVisibility(CurrentPage) %>' runat="server">
                    <li class="banner">
                        <asp:PlaceHolder ID="LoggedInBannerLinkPlaceHolder" runat="server">
                        <%--<a href='<%= EPiFunctions.GetLoginBannerLink(CurrentPage) %>'>
                        <asp:HyperLink NavigateUrl='<%= EPiFunctions.GetLoginBannerLink(CurrentPage) %>' runat="server">
                            <asp:Image ID="LoggedInBannerImage" ImageUrl='<%# EPiFunctions.GetLoginBanner(CurrentPage) %>' Width="234" Height="103" runat="server"/>
                        </asp:HyperLink>
                         </a>--%>
                        </asp:PlaceHolder>
                    </li>
                </asp:PlaceHolder>
            </ul>
		</div>	
	</div>
</asp:PlaceHolder>

