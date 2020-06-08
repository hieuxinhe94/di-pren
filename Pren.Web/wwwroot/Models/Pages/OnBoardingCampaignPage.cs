using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Web;

namespace Pren.Web.Models.Pages
{
    [ContentType(DisplayName = "Onboarding kampanj", GUID = "a9d46370-fdd1-4bf7-be03-216c306cea36", Description = "Orderboardingflöde för företag", GroupName = "Kampanj")]
    public class OnBoardingCampaignPage : SitePageData
    {
        [Required]
        [UIHint(EPiServer.Web.UIHint.LongString)]
        [Display(
            Name = "Kampanj",
            Description = "Kampanj, JSON",
            Order = 40)]
        public virtual string Package { get; set; }

        [Required]
        [Display(
            Name = "Företagsid",
            Order = 50)]
        public virtual string CompanyId { get; set; }

        [Required]
        [UIHint(UIHint.Image)]
        [Display(
            Name = "Företagslogo",
            Order = 55)]
        public virtual ContentReference CompanyImage { get; set; }

        [Required]
        [UIHint(EPiServer.Web.UIHint.LongString)]
        [Display(
            Name = "Parametrar",
            Description = "Parametrar, JSON",
            Order = 60)]
        public virtual string Parameters { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Dölj iframe text",
            Description = "Om ni anger text i denna egenskap kommer iframen att döljas och texten visas istället",
            Order = 70)]
        public virtual XhtmlString HideIframeText { get; set; }

    }
}