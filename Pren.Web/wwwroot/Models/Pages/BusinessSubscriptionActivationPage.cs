using System.ComponentModel.DataAnnotations;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using EPiServer.Web;
using Pren.Web.Business;
using Pren.Web.Business.Descriptors.Editor;

namespace Pren.Web.Models.Pages
{
    [ContentType(DisplayName = "Aktiveringssida för företagsprenumeration", GUID = "e96d797b-c8b9-4003-9081-85893d03f11f", Description = "Aktiveringssida för företagsprenumeration", GroupName = "Företagsportal")]
    public class BusinessSubscriptionActivationPage : SitePageData
    {
        [CultureSpecific]
        [UIHint(UIHint.Image)]
        [Display(
            Name = "Bild",
            Description = "Kampanjens bild",
            GroupName = SystemTabNames.Content,
            Order = 20)]
        public virtual ContentReference Image { get; set; }

        [Required]
        [Display(
            Name = "Rubrik på aktiveringssida",
            GroupName = SystemTabNames.Content,
            Order = 50)]
        public virtual string ActivationFormHeading { get; set; }

        [Required]
        [Display(
            Name = "Text på aktiveringssida. Platshållare [foretag]",
            Description = "Platshållare [foretag]",
            GroupName = SystemTabNames.Content,
            Order = 60)]
        public virtual XhtmlString ActivationFormText { get; set; }

        [Required]
        [Display(
            Name = "Rubrik för erbjudande",
            GroupName = SystemTabNames.Content,
            Order = 65)]
        public virtual string OfferDetailsHeading { get; set; }

        [EditorDescriptor(EditorDescriptorType = typeof(CampaignUspEditorDescriptor))]
        [Display(
            Name = "Usp Grupp",
            GroupName = SystemTabNames.Content,
            Order = 67)]
        public virtual int UspProduct { get; set; }

        [Required]
        [Display(
            Name = "Rubrik vid lyckad aktivering",
            Description = "Rubriken visas när en aktivering lyckats",
            GroupName = CustomTabs.ThankYou,
            Order = 100)]
        public virtual string SuccessfullyActivatedHeading { get; set; }

        [Required]
        [Display(
            Name = "Text för lyckad aktivering, visas om ett S+ konto har skapats. Platshållare [email]",
            Description = "Platshållare [email]",
            GroupName = CustomTabs.ThankYou,
            Order = 200)]
        public virtual XhtmlString SuccessfullyActivatedText { get; set; }

        [Display(
            Name = "Knapptext",
            GroupName = CustomTabs.ThankYou,
            Order = 210)]
        public virtual string SuccessfullyActivatedButtonText { get; set; }

        [Display(
            Name = "Länk för knappen",
            GroupName = CustomTabs.ThankYou,
            Order = 220)]
        public virtual Url SuccessfullyActivatedButtonLink { get; set; }

        [Required]
        [Display(
            Name = "Rubrik vid misslyckad aktivering",
            Description = "Rubriken visas när en aktivering misslyckas",
            GroupName = CustomTabs.ThankYou,
            Order = 300)]
        public virtual string NotSuccessfullyActivatedHeading { get; set; }

        [Required]
        [Display(
            Name = "Text för misslyckad aktivering pga tekniskt fel",
            GroupName = CustomTabs.ThankYou,
            Order = 400)]
        public virtual XhtmlString NotSuccessfullyActivatedText { get; set; }

        [Required]
        [Display(
            Name = "Text för misslyckad aktivering pga inbjudan har utgått",
            GroupName = CustomTabs.ThankYou,
            Order = 400)]
        public virtual XhtmlString NotSuccessfullyActivatedTextInviteExpired { get; set; }
    }
}