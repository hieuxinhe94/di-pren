using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Pren.Web.Business.EpiSelfServiceContentApi.Models.Blocks;

namespace Pren.Web.Business.EpiSelfServiceContentApi.Models.Pages
{
    [ContentType(DisplayName = "Startsida för kundservice innehåll", GUID = "8F31BE08-8CD8-4298-BF62-EBBBD53F226F", Description = "Startsida för kundservice innehåll", GroupName = "Kundservice")]
    public class SelfServiceHomePage : PageData
    {
        [Display(
            Name = "Viktiga meddelanden",
            GroupName = SystemTabNames.Content,
            Order = 10)]
        [AllowedTypes(new[] { typeof(AlertBlock) })]
        public virtual ContentArea AlertsContentArea { get; set; }

        [Display(
            Name = "Teasers",
            GroupName = SystemTabNames.Content,
            Order = 20)]
        [AllowedTypes(new[] { typeof(TeaserBlock) })]
        public virtual ContentArea TeaserContentArea { get; set; }

        [Display(
            Name = "Codes",
            GroupName = SystemTabNames.Content,
            Order = 30)]
        [AllowedTypes(new[] { typeof(CodeBlock) })]
        public virtual ContentArea CodesContentArea { get; set; }

        [Display(
            Name = "Teasers i högerkolumn",
            GroupName = SystemTabNames.Content,
            Order = 40)]
        [AllowedTypes(new[] { typeof(RightColumnTeaserBlock) })]
        public virtual ContentArea RightColumnTeasersContentArea { get; set; }

        [Display(
            Name = "Användarvillkor",
            GroupName = SystemTabNames.Content,
            Order = 50)]
        public virtual XhtmlString UserTerms { get; set; }

        [Display(
            Name = "Användarvillkor arkiv",
            GroupName = SystemTabNames.Content,
            Order = 55)]
        public virtual XhtmlString UserTermsArchive { get; set; }

        [Display(
            Name = "Prenumerationsvillkor",
            GroupName = SystemTabNames.Content,
            Order = 60)]
        public virtual XhtmlString SubscriptionTerms { get; set; }

        [Display(
            Name = "Cookiesvillkor",
            GroupName = SystemTabNames.Content,
            Order = 70)]
        public virtual XhtmlString CookiesTerms { get; set; }

        [Display(
            Name = "PUL",
            GroupName = SystemTabNames.Content,
            Order = 80)]
        public virtual XhtmlString PulTerms { get; set; }

        [Display(
            Name = "Sidfotstext",
            GroupName = SystemTabNames.Content,
            Order = 90)]
        public virtual XhtmlString FooterText { get; set; }
    }
}