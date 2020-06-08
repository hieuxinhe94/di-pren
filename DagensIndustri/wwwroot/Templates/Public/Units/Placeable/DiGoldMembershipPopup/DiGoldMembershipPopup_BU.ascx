<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DiGoldMembershipPopup_BU.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.DiGoldMembershipPopup.DiGoldMembershipPopup_BU" %>
<%@ Register TagPrefix="di" TagName="LoggedInPopup" Src="~/Templates/Public/Units/Placeable/DiGoldMembershipPopup/DiGoldMembershipPopupLoggedIn.ascx" %>
<%@ Register TagPrefix="di" TagName="NotLoggedInPopup" Src="~/Templates/Public/Units/Placeable/DiGoldMembershipPopup/DiGoldMembershipPopupNotLoggedIn.ascx" %>

<!-- Not Logged -->
<di:NotLoggedInPopup ID="NotLoggedInPopupControl" Visible="<%# !HttpContext.Current.User.Identity.IsAuthenticated %>" runat="server" />
<!-- // Login required popup -->

<!-- Logged in but not Di Gold member -->
<di:LoggedInPopup ID="LoggedInPopupControl" Visible="<%# HttpContext.Current.User.Identity.IsAuthenticated %>" runat="server" />
<!-- //Login required popup 2-->