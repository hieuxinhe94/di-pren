<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master" CodeBehind="SubscriptionFlow.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.SubscriptionFlow" %>
<%@ Register TagPrefix="di" TagName="SubscriptionMatrix" Src="~/Templates/Public/Units/Placeable/Subscription/SubscriptionMatrix.ascx" %>
<%@ Register TagPrefix="di" TagName="SubscriberDetails" Src="~/Templates/Public/Units/Placeable/Subscription/SubscriberDetails.ascx" %>
<%@ Register TagPrefix="di" TagName="InvoiceRecipient" Src="~/Templates/Public/Units/Placeable/Subscription/InvoiceRecipient.ascx" %>
<%@ Register TagPrefix="di" TagName="PromotionalOffer" Src="~/Templates/Public/Units/Placeable/DiGoldMembershipFlow/PromotionalOffer.ascx" %>
<%@ Register TagPrefix="di" TagName="TopImage" Src="~/Templates/Public/Units/Placeable/Subscription/TopImage.ascx" %>
<%@ Register TagPrefix="di" TagName="UserMessage" Src="~/Templates/Public/Units/Placeable/UserMessage.ascx" %>

<asp:Content ContentPlaceHolderID="TopContentPlaceHolder" runat="server">
   <di:TopImage ID="TopImageControl" runat="server" />
</asp:Content>

<asp:Content ContentPlaceHolderID="HeadingContentPlaceHolder" runat="server">
    <asp:PlaceHolder ID="HeadingPlaceHolder" runat="server">
        <h1>
            <%= GetHeading() %>
        </h1>
    </asp:PlaceHolder>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <di:UserMessage ID="UserMessageControl" runat="server" />

    <asp:MultiView ID="SubscriptionMultiView" runat="server">
            <asp:View ID="SubscriptionMatrixView" runat="server">
                <di:SubscriptionMatrix ID="SubscriptionMatrixControl" runat="server" />
            </asp:View>

            <asp:View ID="SubscriberDetailsView" runat="server">
                <di:SubscriberDetails ID="SubscriberDetailsControl" runat="server" />
            </asp:View>

            <asp:View ID="InvoiceRecipientView" runat="server">
                <di:InvoiceRecipient ID="InvoiceRecipientControl" runat="server" />
            </asp:View>

            <asp:View ID="PromotionalOfferView" runat="server">
                <di:PromotionalOffer ID="PromotionalOfferControl" runat="server" />
            </asp:View>
            
        </asp:MultiView>

        <div class="button-wrapper">
            <asp:LinkButton ID="BackLinkButton" CssClass="more back" Text="<%$ Resources: EPiServer, common.back %>" OnClick="BackLinkButton_Click" Visible="false" runat="server" />
            <asp:Button ID="ContinueButton" CssClass="btn" Text="<%$ Resources: EPiServer, common.continue %>" OnClick="ContinueButton_Click" runat="server"/>
	    </div>

        <script type="text/javascript">
            var analytics_site_ids = analytics_site_ids || [];
            analytics_site_ids.push(100550322);
            (function () {
                var s = document.createElement('script');
                s.type = 'text/javascript';
                s.async = true;
                s.src = '//hello.staticstuff.net/w/analytics.js';
                (document.getElementsByTagName('head')[0] || document.getElementsByTagName('body')[0]).appendChild(s);
            })();
        </script>
        <noscript><p><img alt="Camelonta MyVisitors" width="1" height="1" src="//win.staticstuff.net/100550322ns.gif" /></p></noscript>

        <!-- GoogleCode for Kampanj/s&ouml;k -->
        <!-- Remarketing tags may not be associatedwithpersonallyidentifiable information or placed on pages relatedto sensitive categories. For instructions on addingthis tag and more information on the aboverequirements, read the setup guide: google.com/ads/remarketingsetup -->
        <script type="text/javascript">
            /* <![CDATA[ */
            var google_conversion_id = 1010240133;
            var google_conversion_label = "5NqwCOOPhQQQhZXc4QM";
            var google_custom_params = window.google_tag_params;
            var google_remarketing_only = true;
            /* ]]> */
        </script>
        <script type="text/javascript" src="//www.googleadservices.com/pagead/conversion.js"></script>
        <noscript>
            <div style="display:inline;">
                <imgheight="1" width="1" style="border-style:none;" alt="" src="//googleads.g.doubleclick.net/pagead/viewthroughconversion/1010240133/?value=0&amp;label=5NqwCOOPhQQQhZXc4QM&amp;guid=ON&amp;script=0"/>
            </div>
        </noscript>

</asp:Content>