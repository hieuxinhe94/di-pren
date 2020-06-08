<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DiGoldMembershipPopup.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.DiGoldMembershipPopup.DiGoldMembershipPopup" %>


<!-- popup - not logged in -->
<asp:Panel ID="NotLoggedInPopupControl" Visible="<%# !HttpContext.Current.User.Identity.IsAuthenticated %>" runat="server">
    <div class="ajax-popup" id="membership-required">
	    <div class="wrapper gold">
		    <div class="content">
			    <h2><%= EPiFunctions.SettingsPageSetting(CurrentPage, "DiGoldNotLoggedInPopupHeading")%></h2>
                <%= EPiFunctions.SettingsPageSetting(CurrentPage, "DiGoldNotLoggedInPopupMainBody")%>

                <%--<a class="btn btn" href="/login"><span>Gå till inloggninssidan</span></a>--%>
                <asp:Button ID="ButtonJoinGoldGoToLogin" Text="Gå till inloggningssidan" OnClick="ButtonJoinGoldGoToLogin_Click" runat="server" />

                <br><br><br><br><br>
                
				<h3><EPiServer:Translate ID="Translate1" Text="/digoldpopup/forsubscribers" runat="server" /></h3>
                <%= EPiFunctions.SettingsPageSetting(CurrentPage, "DiGoldNotLoggedInPopupPrenumerants")%>
                
                <asp:HyperLink ID="BecomeSubscriberHyperLink" CssClass="more" NavigateUrl="<%# EPiFunctions.GetSubscriptionPageUrl(CurrentPage) %>" Text="<%$ Resources: EPiServer, digoldpopup.becomesubscriber %>" runat="server"/>
							
                <asp:HyperLink ID="HyperLink1" CssClass="btn-close" Text="<%$ Resources: EPiServer, common.close %>" runat="server"/>
		    </div>
	    </div>
    </div>
</asp:Panel>
<!-- //popup - not logged in -->


<!-- popup - logged in - gold required -->
<asp:Panel ID="LoggedInPopupControl" Visible="<%# HttpContext.Current.User.Identity.IsAuthenticated %>" runat="server">
    <div class="ajax-popup" id="membership-required">
	    <div class="wrapper gold">
		    <div class="content">
			    <h2 class="subscriber"><%= EPiFunctions.SettingsPageSetting(CurrentPage, "DiGoldLoggedInPopupHeading") %></h2>
			    <%= EPiFunctions.SettingsPageSetting(CurrentPage, "DiGoldLoggedInPopupMainBody")%>
                <asp:Button ID="BecomeMemberButton" class="btn" Text="<%$ Resources: EPiServer, digoldpopup.becomemember%>" OnClick="BecomeMember_Click" runat="server" />

                <asp:HyperLink ID="HyperLink2" CssClass="more cancel" Text="<%$ Resources: EPiServer, common.cancel %>" runat="server"/>			
                <asp:HyperLink ID="HyperLink3" CssClass="btn-close" Text="<%$ Resources: EPiServer, common.close %>" runat="server"/>
		    </div>
	    </div>
    </div>
</asp:Panel>
<!-- //popup - logged in - gold required -->