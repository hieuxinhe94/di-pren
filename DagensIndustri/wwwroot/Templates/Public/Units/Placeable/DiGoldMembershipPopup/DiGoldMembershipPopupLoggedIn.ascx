<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DiGoldMembershipPopupLoggedIn.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.DiGoldMembershipPopup.DiGoldMembershipPopupLoggedIn" %>

<asp:HiddenField ID="ReturnURLHiddenField" runat="server" />

<!-- Login required popup logged in-->
<div class="ajax-popup" id="membership-required">
	<div class="wrapper gold">
		<div class="content">
			<h2 class="subscriber"><%= EPiFunctions.SettingsPageSetting(CurrentPage, "DiGoldLoggedInPopupHeading") %></h2>
			<%= EPiFunctions.SettingsPageSetting(CurrentPage, "DiGoldLoggedInPopupMainBody")%>
            <asp:Button ID="BecomeMemberButton" class="btn" Text="<%$ Resources: EPiServer, digoldpopup.becomemember%>" OnClick="BecomeMember_Click" runat="server" />

            <asp:HyperLink CssClass="more cancel" Text="<%$ Resources: EPiServer, common.cancel %>" runat="server"/>			
            <asp:HyperLink CssClass="btn-close" Text="<%$ Resources: EPiServer, common.close %>" runat="server"/>
		</div>
	</div>
</div>
<!-- // Login required popup logged in -->
