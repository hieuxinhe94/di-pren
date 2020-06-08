<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master"  CodeBehind="PromotionalOffer.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.PromotionalOffer" %>
<%@ Register TagPrefix="di" TagName="Heading" Src="~/Templates/Public/Units/Placeable/Heading.ascx" %>
<%@ Register TagPrefix="di" TagName="PromotionalOffer" Src="~/Templates/Public/Units/Placeable/DiGoldMembershipFlow/PromotionalOffer.ascx" %>
<%@ Register TagPrefix="di" TagName="UserMessage" Src="~/Templates/Public/Units/Placeable/UserMessage.ascx" %>

<asp:Content ContentPlaceHolderID="HeadingContentPlaceHolder" runat="server">
    <di:Heading ID="HeadingControl" runat="server" />
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <di:UserMessage ID="UserMessageControl" runat="server" />

    <asp:PlaceHolder ID="PromotionalOfferPlaceHolder" runat="server">
         <di:PromotionalOffer ID="PromotionalOfferControl" runat="server" />

         <div class="button-wrapper">
            <asp:Button ID="OrderButton" Text="<%$ Resources: EPiServer, common.order %>" OnClick="OrderButton_Click" runat="server" />
	    </div>
    </asp:PlaceHolder>
</asp:Content>