using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using EPiServer.Web;
using Pren.Web.Business.Descriptors.Editor;
using Pren.Web.Models.Blocks;

namespace Pren.Web.Models.Pages
{
    [ContentType(DisplayName = "Kampanjsida för företagsprenumeration", GUID = "29089cc5-9f6a-4cd5-8f9e-633cf9c9af28", Description = "Kampanjsida för företagsprenumeration", GroupName = "Företagsportal")]
    public class BusinessCampaignPage : SitePageData
    {
        [Display(
            Name = "Rubrik",
            Description = "Rubrik",
            GroupName = SystemTabNames.Content,
            Order = 100)]
        public virtual string Heading { get; set; }

        [Display(
            Name = "Innehållsyta för loggor",
            GroupName = SystemTabNames.Content,
            Order = 17)]        
        public virtual ContentArea BelowHeadingContentArea { get; set; }

        [Display(
            Name = "Info rubrik",
            GroupName = SystemTabNames.Content,
            Order = 18)]
        public virtual string InfoHeading { get; set; }

        [Display(
            Name = "Info, text",
            GroupName = SystemTabNames.Content,
            Order = 19)]
        public virtual XhtmlString InfoBody { get; set; }

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
            Name = "Kampanjnummer i Kayak",
            Description = "Ange det Kayak-kampanjnummer som ska skickas med till S+",
            GroupName = SystemTabNames.Content,
            Order = 30)]
        public virtual int KayakCampaignNumber { get; set; }

        [Required]
        [Display(
            Name = "ProduktId för definitioner i S+",
            Description = "Ange det produkt id som definitionerna ska hämtas från",
            GroupName = SystemTabNames.Content,
            Order = 35)]
        public virtual string ServicePlusBizDefinitionProductId { get; set; }

        [Required]
        [Display(
            Name = "Adminsida för företagsprenumeration",
            Description = "Peka ut adminsida för företagsprenumerationer",
            GroupName = SystemTabNames.Content,
            Order = 40)]
        public virtual PageReference SubscriptionAdminPage { get; set; }

        [Display(
            Name = "Erbjudande, rubrik",
            GroupName = SystemTabNames.Content,
            Order = 100)]
        public virtual string OfferHeading { get; set; }

        [EditorDescriptor(EditorDescriptorType = typeof(CampaignUspEditorDescriptor))]
        [Display(
            Name = "Usp Grupp",
            GroupName = SystemTabNames.Content,
            Order = 110)]
        public virtual int UspProduct { get; set; }

        [Display(
            Name = "Innehållsyta i sidfoten",
            GroupName = SystemTabNames.Content,
            Order = 5000)]
        [AllowedTypes(new[] { typeof(TextBlock) })]
        public virtual ContentArea FooterContentArea { get; set; }

        [Required]
        [Display(
            Name = "Epostadress, licensfrågor",
            GroupName = SystemTabNames.Content,
            Order = 120)]
        public virtual string LicenceContactMail { get; set; }

        [Required]
        [Display(
            Name = "Info, licensfrågor",
            GroupName = SystemTabNames.Content,
            Order = 130)]
        public virtual XhtmlString LicenceContactBody { get; set; }

        [Required]
        [Display(
            Name = "Mailsvar, licensfrågor",
            GroupName = SystemTabNames.Content,
            Order = 140)]
        public virtual XhtmlString LicenceMailConfirmBody { get; set; }

        [Required]
        [Display(
            Name = "Hjälptext",
            GroupName = SystemTabNames.Content,
            Order = 150)]
        public virtual XhtmlString HelpBody { get; set; }



    }
}