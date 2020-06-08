<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ForgotPassword.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.Login.ForgotPassword" %>
<%@ Register TagPrefix="di" TagName="Input" Src="~/Tools/Classes/WebControls/InputWithValidation.ascx" %>

<asp:HiddenField ID="SendLoginHiddenField" runat="server" />

<!-- Forgot password -->
<div class="section form-box">
	<div class="request-login">
		<strong><EPiServer:Translate Text="/dilogin/forgottenyourlogin" runat="server" /></strong>

		<p>
            <EPiServer:Translate Text="/dilogin/noworry" runat="server" /><br />
			<asp:HyperLink ID="SendLoginHyperLink" NavigateUrl="#request-form" CssClass="more ajax" runat="server" Text="<%$ Resources: EPiServer, dilogin.sendmyinfo %>" />
		</p>
		<p>
            <EPiServer:Translate Text="/dilogin/contactcustomerservice" runat="server" />
		</p>
                                                                       
		<div class="request-form no-popup" id="request-form">

            <asp:UpdatePanel ID="SendLoginUpdatePanel" runat="server" UpdateMode="Conditional">
	            <ContentTemplate>
		            <div class="row">
                        <asp:Label ID="RememberLabel" AssociatedControlID="RememberDropDownList" Text="<%$ Resources: EPiServer, dilogin.remembermy %>" runat="server" />
                        <asp:DropDownList ID="RememberDropDownList" runat="server" OnSelectedIndexChanged="RememberDropDownList_SelectedIndexChanged" AutoPostBack="true" />
					</div>

		            <div class="row">
                        <di:Input ID="RememberWhatInput" Name="what" CssClass="text" Required="true" StripHtml="true" Title="<%$ Resources: EPiServer, dilogin.rememberis %>" DisplayMessage="<%$ Resources: EPiServer, dilogin.error.rememberwhatrequired %>" runat="server" />
					</div>
	            </ContentTemplate>
	            <Triggers>
		            <asp:AsyncPostBackTrigger ControlID="RememberDropDownList" EventName="SelectedIndexChanged" />
	            </Triggers>
            </asp:UpdatePanel>

			<div class="button-wrapper">
                <asp:HyperLink ID="CancelSendLoginHyperLink" NavigateUrl="#" CssClass="cancel more" Text="<%$ Resources: EPiServer, common.cancel %>" runat="server" />

                <asp:Button ID="SendPasswordButton" Text="<%$ Resources: EPiServer, common.send %>" OnClick="SendPasswordButton_Click" runat="server" />
			</div>
			<div class="cover">&nbsp;</div>
		</div>
						
	</div>
</div>
<!-- // Forgot password -->
<br class="clear" />