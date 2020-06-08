using System.ComponentModel.DataAnnotations;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using EPiServer.Web;
using Pren.Web.Models.Blocks;

namespace Pren.Web.Models.Pages
{
    [ContentType(DisplayName = "Kampanj, landningssida", GUID = "599739c9-f02e-4aeb-bdf4-119d2104eceb", Description = "Sida för att länka besökare vidare till papper eller digital kampanj", GroupName = "Kampanj")]
    public class CampaignLandingPage : SitePageData
    {
        [CultureSpecific]
        [Display(
            Name = "Rubrik",
            Description = "Rubrik",
            GroupName = SystemTabNames.Content,
            Order = 100)]
        public virtual string Heading { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Innehåll",
            GroupName = SystemTabNames.Content,
            Order = 120)]
        public virtual XhtmlString MainBody { get; set; }


        [Required]
        [CultureSpecific]
        [UIHint(UIHint.Image)]
        [Display(
            Name = "Bild vänster",
            GroupName = SystemTabNames.Content,
            Order = 150)]
        public virtual ContentReference ImageLeft { get; set; }

        [Required]
        [CultureSpecific]
        [Display(
            Name = "Knapptext vänster",
            Description = "Text som visas på vänstra knappen",
            GroupName = SystemTabNames.Content,
            Order = 160)]
        public virtual string ButtonLeftText { get; set; }

        [Required]
        [CultureSpecific]
        [Display(
            Name = "Knappurl vänster",
            Description = "Url för den vänstra knappen",
            GroupName = SystemTabNames.Content,
            Order = 170)]
        public virtual Url ButtonLeftUrl { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Knappurl vänster, använd callback url",
            Description = "Kommer sätta url på knappen till värdet i urlparametern callback. Om tomt så används angiven url i egenskapen 'Knappurl vänster'",
            GroupName = SystemTabNames.Content,
            Order = 180)]
        public virtual bool ButtonLeftUseCallback { get; set; }

        [Required]
        [CultureSpecific]
        [UIHint(UIHint.Image)]
        [Display(
            Name = "Bild höger",
            GroupName = SystemTabNames.Content,
            Order = 200)]
        public virtual ContentReference ImageRight { get; set; }

        [Required]
        [CultureSpecific]
        [Display(
            Name = "Knapptext höger",
            Description = "Text som visas på högra knappen",
            GroupName = SystemTabNames.Content,
            Order = 210)]
        public virtual string ButtonRightText { get; set; }

        [Required]
        [CultureSpecific]
        [Display(
            Name = "Knappurl höger",
            Description = "Url för den högra knappen",
            GroupName = SystemTabNames.Content,
            Order = 220)]
        public virtual Url ButtonRightUrl { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Knappurl höger, använd callback url",
            Description = "Kommer sätta url på knappen till värdet i urlparametern callback. Om tomt så används angiven url i egenskapen 'Knappurl höger'",
            GroupName = SystemTabNames.Content,
            Order = 230)]
        public virtual bool ButtonRightUseCallback { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Innehållsyta i sidfoten",
            GroupName = SystemTabNames.Content,
            Order = 5000)]
        [AllowedTypes(new[] { typeof(TextBlock) })]
        public virtual ContentArea FooterContentArea { get; set; }

    }
}