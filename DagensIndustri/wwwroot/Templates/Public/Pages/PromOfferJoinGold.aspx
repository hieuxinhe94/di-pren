<%@ Page Language="C#" MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="PromOfferJoinGold.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.PromOfferJoinGold" %>
<%@ Register TagPrefix="di" TagName="Heading" Src="~/Templates/Public/Units/Placeable/Heading.ascx" %>
<%@ Register TagPrefix="di" TagName="AdditionalUserDetails" Src="~/Templates/Public/Units/Placeable/DiGoldMembershipFlow/AdditionalUserDetails.ascx" %>
<%@ Register TagPrefix="di" TagName="PromotionalOffer" Src="~/Templates/Public/Units/Placeable/DiGoldMembershipFlow/PromotionalOffer.ascx" %>
<%@ Register TagPrefix="di" TagName="UserMessage" Src="~/Templates/Public/Units/Placeable/UserMessage.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadingContentPlaceHolder" runat="server">
    <di:Heading ID="HeadingControl" runat="server" />
    <script type="text/javascript" src="/Templates/Public/js/PreventDoublePost.js"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    
    <di:UserMessage ID="UserMessageControl" runat="server" />

    <asp:PlaceHolder ID="PlaceHolderTexts" runat="server">
        <p class="intro"><%= IntroText %></p>
        <%= BreadText %>
    </asp:PlaceHolder>

    <di:AdditionalUserDetails ID="GoldForm" runat="server" />

    <asp:PlaceHolder ID="PlaceHolderAddressAndButton" runat="server">
         <di:PromotionalOffer ID="AddressForm" runat="server" />
        
        <div id="divSubmitBtn" class="button-wrapper">
            <asp:Button ID="OrderButton" Text="<%$ Resources: EPiServer, common.order %>" OnClick="OrderButton_Click" OnClientClick="return jsPreventDoublePost('divSubmitBtn', 'divFormSent');" runat="server" />
	    </div>

        <div id="divFormSent" style="float:right; visibility:hidden;">
            <img src="/Templates/Public/Images/ajax-loader.gif" alt="" />
            <i>&nbsp;<asp:Literal ID="Literal1" Text="<%$ Resources: EPiServer, common.sendingform %>" runat="server" /></i>
        </div>

    </asp:PlaceHolder>

    <asp:Label ID="LabelThankYou" Visible="false" runat="server"></asp:Label>

</asp:Content>
