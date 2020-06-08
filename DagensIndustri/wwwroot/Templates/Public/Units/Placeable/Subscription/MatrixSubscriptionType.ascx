<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MatrixSubscriptionType.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.Subscription.MatrixSubscriptionType" %>
<%@ Register TagPrefix="di" TagName="Input" Src="~/Tools/Classes/WebControls/InputWithValidation.ascx" %>

<div class='<%= IsActive ? "wrapper active" : "wrapper" %>'>
	<div class="content">
		<h2><%= Name %></h2>
		<p class="price" style='<%# string.Format("background-image: url({0});", PriceImageUrl) %>'>
		</p>
		<div class="form">
            <asp:PlaceHolder ID="DiGoldPlaceholder" Visible="<%# ShowDiGold %>" runat="server">
                <span class="label">
                    <di:Input ID="DiGoldCheckBox" TypeOfInput="CheckBox" Name="DiGuld" Title="<%$ Resources: EPiServer, subscription.matrix.digold %>" runat="server" />
                </span>
            </asp:PlaceHolder>
            <%--The command argument consists of "SubscriptionType|User wants to become Di Gold member"--%>
            <asp:Button ID="ContinueButton" CssClass="btn" Text="<%$ Resources: EPiServer, common.continue %>" OnClick="ContinueButton_Click" CommandArgument='<%# string.Format("{0}|{1}|{2}|{3}", (int)SubscriptionType, CampaignNo1, CampaignNo2,  DiGoldCheckBox.Checked) %>' runat="server" />
		</div>
	</div>
</div>