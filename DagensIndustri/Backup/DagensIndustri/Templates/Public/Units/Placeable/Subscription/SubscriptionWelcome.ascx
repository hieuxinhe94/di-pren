<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SubscriptionWelcome.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.Subscription.SubscriptionWelcome" %>

<div class="subscription-welcome">

	<h1><EPiServer:Translate Text="/subscription/landingpage/welcome" runat="server" /></h1>
	<p class="intro">
        <%= string.Format(Translate("/subscription/landingpage/firstpaperdate"), GetSubscriptionStartDate()) %>
    </p>

    <asp:PlaceHolder ID="DiGoldWelcomePlaceHolder" Visible="<%# EPiFunctions.IsUserDIGoldMember() %>" runat="server">
        <%= (string)EPiFunctions.SettingsPageSetting(CurrentPage, "DiGoldWelcomeInfo")  %>       
    </asp:PlaceHolder>
    
    <asp:PlaceHolder ID="NotDiGoldWelcomePlaceHolder" Visible="<%# !EPiFunctions.IsUserDIGoldMember() && !IsDiWeekend %>" runat="server">
        <%= (string)EPiFunctions.SettingsPageSetting(CurrentPage, "NotDiGoldWelcomeInfo")%>
        
        <asp:LinkButton ID="BecomeDiGoldMemberLinkButton" CssClass="btn" OnClick="BecomeDiGoldMemberLinkButton_Click" runat="server">
            <span><EPiServer:Translate Text="/digold/becomedigoldmember" runat="server" /></span>
        </asp:LinkButton>
	    <asp:HyperLink ID="CancelHyperLink" CssClass="more cancel" NavigateUrl="<%# EPiFunctions.GetFriendlyAbsoluteUrl(CurrentPage) %>" Text="<%$ Resources: EPiServer, common.cancel %>" runat="server" />
	</asp:PlaceHolder>
</div>