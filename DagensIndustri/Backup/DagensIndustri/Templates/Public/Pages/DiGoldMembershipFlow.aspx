<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master" CodeBehind="DiGoldMembershipFlow.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.DiGoldMembershipFlow" %>
<%@ Register TagPrefix="di" TagName="AdditionalUserDetails" Src="~/Templates/Public/Units/Placeable/DiGoldMembershipFlow/AdditionalUserDetails.ascx" %>
<%@ Register TagPrefix="di" TagName="PromotionalOffer" Src="~/Templates/Public/Units/Placeable/DiGoldMembershipFlow/PromotionalOffer.ascx" %>
<%@ Register TagPrefix="di" TagName="UserMessage" Src="~/Templates/Public/Units/Placeable/UserMessage.ascx" %>

<asp:Content ContentPlaceHolderID="HeadingContentPlaceHolder" runat="server">
    <asp:PlaceHolder ID="HeadingPlaceHolder" runat="server">
        <h1>
            <%= GetHeading() %>
        </h1>
    </asp:PlaceHolder>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <di:UserMessage ID="UserMessageControl" runat="server" />

    <asp:PlaceHolder ID="MultiViewPlaceHolder" runat="server">
        <asp:MultiView ID="DiGoldFlowMultiView" runat="server">
            <asp:View ID="AdditionalUserDetailsView" runat="server">
                <di:AdditionalUserDetails ID="AdditionalUserDetailsControl" runat="server" />
            </asp:View>

            <asp:View ID="PromotionalOfferView" runat="server">
                <di:PromotionalOffer ID="PromotionalOfferControl" runat="server" />
            </asp:View>
        </asp:MultiView>

        <div class="button-wrapper">
            <asp:LinkButton ID="BackLinkButton" CssClass="more back" Text="<%$ Resources: EPiServer, common.back %>" OnClick="BackLinkButton_Click" Visible="false" runat="server" />
            <asp:Button ID="ContinueButton" Text="<%$ Resources: EPiServer, common.continue %>" OnClick="ContinueButton_Click" runat="server" />
	    </div>
    </asp:PlaceHolder>
</asp:Content>

<asp:Content ContentPlaceHolderID="RightColumnPlaceHolder" runat="server">
</asp:Content>