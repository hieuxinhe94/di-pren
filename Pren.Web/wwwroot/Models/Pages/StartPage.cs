using System.ComponentModel.DataAnnotations;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Pren.Web.Business;
using Pren.Web.Models.Blocks;
using Pren.Web.Models.Pages.MySettings;
using Pren.Web.Models.Pages.MySettings.Di;

namespace Pren.Web.Models.Pages
{
    [ContentType(DisplayName = "StartPage", GUID = "e85e8c91-e672-48ce-b77f-250c31befca3", Description = "Startsida")]
    [AvailableContentTypes(
        Availability.Specific,
        Include = new[] { typeof(ContainerPage), typeof(CampaignPage), typeof(MySettingsStartPage), typeof(ConnectPage), typeof(TextPage), typeof(MyStartPage) },
        ExcludeOn = new[] { typeof(ContainerPage), typeof(MySettingsStartPage) })]
    public class StartPage : SitePageData
    {
        [Display(
            Name = "Copyrighttext",
            GroupName = SystemTabNames.Content,
            Order = 400)]
        public virtual string Copyright { get; set; }

        [Display(
            Name = "Disclaimertext",
            Description = "Visas under iframe i Klarnaflödet",
            GroupName = CustomTabs.Campaign,
            Order = 100)]
        public virtual XhtmlString DisclaimerText { get; set; }

        [Required]
        [Display(
            Name = "Default callback url",
            Description = "Default callback url i orderflödet för Klarna",
            GroupName = CustomTabs.Campaign,
            Order = 200)]
        public virtual Url DefaultCallbackUrl { get; set; }

        [Display(
            Name = "Prenumerationsvillkor",
            GroupName = SystemTabNames.Content,
            Order = 350)]
        public virtual XhtmlString PrenTermsText { get; set; }

        [Display(
            Name = "Chat, kundtjänst",
            Description = "Text som visas ovanför chat-knappen",
            GroupName = SystemTabNames.Content,
            Order = 360)]
        public virtual XhtmlString CustomerServiceText { get; set; }

        [Display(
            Name = "Innehållsyta i sidfoten",
            GroupName = SystemTabNames.Content,
            Order = 5000)]
        [AllowedTypes(new[] { typeof(TextBlock) })]
        public virtual ContentArea FooterContentArea { get; set; }

        [Required]
        [Display(
            Name = "StartSida för mina sidor",
            GroupName = SystemTabNames.Settings,
            Order = 5000)]
        public virtual ContentReference MySettingsStartPage { get; set; }

        [Required]
        [Display(
            Name = "Företagsadminsida",
            GroupName = SystemTabNames.Settings,
            Order = 5005)]
        public virtual ContentReference BusinessSubscriptionAdminPage { get; set; }

        [Required]
        [Display(
            Name = "Kopplasida",
            GroupName = SystemTabNames.Settings,
            Order = 5010)]
        public virtual ContentReference ConnectPage { get; set; }

        [Display(
            Name = "Mina sidor, dölj db-funktioner",
            GroupName = SystemTabNames.Settings,
            Order = 5020)]
        public virtual bool HideDbFunctions { get; set; }

        [Display(
            Name = "Skicka packageId som productId, Klarna",
            Description = "Default skickas PaperCode-ProductNumber",
            GroupName = SystemTabNames.Settings,
            Order = 5030)]
        public virtual bool PackageIdAsProductId { get; set; }

        [Display(
            Name = "Skicka student parameter, Klarna",
            GroupName = SystemTabNames.Settings,
            Order = 5090)]
        public virtual bool SendStudentParameter { get; set; }

        [Display(
            Name = "Skicka origin parameter, Klarna",
            GroupName = SystemTabNames.Settings,
            Order = 5040)]
        public virtual bool SendOriginParameter { get; set; }

        [Display(
            Name = "Skicka trackingurl parameter, Klarna",
            GroupName = SystemTabNames.Settings,
            Order = 5100)]
        public virtual bool SendTrackingUrlParameter { get; set; }

        [Display(
            Name = "Skapa faktura och betalning vid kortköp",
            GroupName = SystemTabNames.Settings,
            Order = 5030)]
        public virtual bool CreateInvoiceAndPayment { get; set; }

        [Display(
            Name = "Skicka IsDigital parameter, Klarna",
            GroupName = SystemTabNames.Settings,
            Order = 5110)]
        public virtual bool SendIsDigitalParameter { get; set; }

        [Display(
            Name = "Skicka campaignUrl parameter, Klarna",
            GroupName = SystemTabNames.Settings,
            Order = 5115)]
        public virtual bool SendCampaignUrlParameter { get; set; }

        [Display(
            Name = "Kolla Entitlement innan AmbiguousMatchException",
            GroupName = SystemTabNames.Settings,
            Order = 5120)]
        public virtual bool CheckEntitlementBeforeAmbiguousMatchException { get; set; }

        [Display(
            Name = "Starta chat från kontakta oss formulär",
            GroupName = SystemTabNames.Settings,
            Order = 5140)]
        public virtual bool StartChatOnContactSubmit { get; set; }

        [Display(
            Name = "Ta hänsyn till delbetalning i Kayak (dela totalpris på perioder)",
            GroupName = SystemTabNames.Settings,
            Order = 5145)]
        public virtual bool UsePartPayment { get; set; }

        [Display(
            Name = "E-postverifiering BIP (dibs-flöde)",
            GroupName = SystemTabNames.Settings,
            Order = 5200)]
        public virtual bool EmailBipVerification { get; set; }

        [Display(
            Name = "Skicka SubsKind parameter från property",
            GroupName = SystemTabNames.Settings,
            Order = 5250)]
        public virtual bool SendSubsKindParameter { get; set; }

        [Display(
            Name = "Skicka SalesChannel parameter",
            GroupName = SystemTabNames.Settings,
            Order = 5260)]
        public virtual bool SendSalesChannelParameter { get; set; }

        [Display(
            Name = "Skicka productExpl parameter",
            GroupName = SystemTabNames.Settings,
            Order = 5270)]
        public virtual bool SendProductExplParameter { get; set; }

        [Display(
            Name = "Använd pris från Kayak i nya iframeflödet (campaignIframe)",
            GroupName = SystemTabNames.Settings,
            Order = 5300)]
        public virtual bool UsePriceFromKayak { get; set; }

        [Display(
            Name = "Startsida DN kundwebb innehåll",
            GroupName = SystemTabNames.Settings,
            Order = 10000)]
        public virtual ContentReference DnSelfServiceStartPage { get; set; }

        [Display(
            Name = "Startsida Di företagsservice innehåll",
            GroupName = SystemTabNames.Settings,
            Order = 11000)]
        public virtual ContentReference DiCompanyServiceStartPage { get; set; }

        [Required]
        [Display(
            Name = "Sida för inaktiva kampanjer",
            GroupName = SystemTabNames.Settings,
            Order = 12000)]
        public virtual ContentReference CampaignNotActivePage { get; set; }
    }
}