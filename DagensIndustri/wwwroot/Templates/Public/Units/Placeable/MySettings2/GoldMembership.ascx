<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GoldMembership.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.MySettings2.GoldMembership" %>
<%@ Register TagPrefix="DI" TagName="Input" Src="~/Tools/Classes/WebControls/InputWithValidation.ascx" %>


<h2><EPiServer:Translate ID="Translate1" Text="/mysettings/personal/digold/title" runat="server" /></h2>

<p>
Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed id nulla molestie massa lacinia aliquet rutrum sed felis. Duis eleifend dolor non nibh sollicitudin vulputate. Sed suscipit ultrices urna, id feugiat sem eleifend ac. Ut et purus nulla.
</p>

<%--<a href="#" class="edit"><EPiServer:Translate ID="Translate2" Text="/common/change" runat="server" /></a>--%>
<%--<p class="description"><EPiServer:Translate ID="Translate3" Text="/mysettings/personal/digold/description" runat="server" /></p>--%>
<%--<p class="value">UserIsDIGoldMember  = UserIsDIGoldMember ? Translate("/common/active") : Translate("/common/notactive") </p>        --%>

<%--<h5 class="label"><EPiServer:Translate ID="Translate4" Text="/mysettings/personal/digold/activeingold" runat="server"/></h5>--%>
<%--<p class="value">UserIsDIGoldMember  = UserIsDIGoldMember ? Translate("/common/yes") : Translate("/common/no")</p>--%>


<%-- # UserIsDIGoldMember --%>
<asp:Button ID="ActivateDiGoldButton" CssClass="btn" Text="<%$ Resources: EPiServer, mysettings.personal.digold.becomedigoldmember %>" Visible="true" OnClick="DIGoldMembership_Click" CommandArgument="START" runat="server" />
<asp:Button ID="DeactivateDiGoldButton" CssClass="btn" Text="<%$ Resources: EPiServer, mysettings.personal.digold.enddigoldmembership %>" Visible="false" OnClick="DIGoldMembership_Click" CommandArgument="END" runat="server" />
