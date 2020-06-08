<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DiGoldMembershipPopupNotLoggedIn.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.DiGoldMembershipPopup.DiGoldMembershipPopupNotLoggedIn" %>
<%--<%@ Register TagPrefix="di" TagName="LoginControl" Src="~/Templates/Public/Units/Placeable/Login/LoginControl.ascx" %>--%>

<asp:HiddenField ID="ReturnURLHiddenField" runat="server" />

<!-- Login required popup -->
<div class="ajax-popup" id="membership-required">
	<div class="wrapper gold">
		<div class="content">
			<h2><%= EPiFunctions.SettingsPageSetting(CurrentPage, "DiGoldNotLoggedInPopupHeading")%></h2>
            <%= EPiFunctions.SettingsPageSetting(CurrentPage, "DiGoldNotLoggedInPopupMainBody")%>

			<!-- Login module -->            
			<div class="form-box login">  
                [Loginkontroll]
                <%--<di:LoginControl ID="LoginCtrl" runat="server" />--%>
			</div>
			<!-- // Login module -->
			
			<!-- Right -->
			<div class="right-box">
				<h3><EPiServer:Translate Text="/digoldpopup/forsubscribers" runat="server" /></h3>
                <%= EPiFunctions.SettingsPageSetting(CurrentPage, "DiGoldNotLoggedInPopupPrenumerants")%>
                <asp:HyperLink ID="BecomeSubscriberHyperLink" CssClass="more" NavigateUrl="<%# EPiFunctions.GetSubscriptionPageUrl(CurrentPage) %>" Text="<%$ Resources: EPiServer, digoldpopup.becomesubscriber %>" runat="server"/>
			</div>
			<!-- // Right -->
							
            <asp:HyperLink CssClass="btn-close" Text="<%$ Resources: EPiServer, common.close %>" runat="server"/>
		</div>
	</div>
</div>
<!-- // Login required popup -->
