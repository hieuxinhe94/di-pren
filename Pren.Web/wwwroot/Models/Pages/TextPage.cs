using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using EPiServer.Web;
using Pren.Web.Models.Blocks;

namespace Pren.Web.Models.Pages
{
    [ContentType(DisplayName = "Textsida", GUID = "e7b379ff-e7c3-4ca2-9732-edb3e93a0ca4", Description = "Textsida med valfritt innehåll")]
    public class TextPage : SitePageData
    {
        [Display(
            Name = "Rubrik",
            Description = "Rubrik",
            GroupName = SystemTabNames.Content,
            Order = 100)]
        public virtual string Heading { get; set; }

        [Display(
            Name = "Innehåll",
            GroupName = SystemTabNames.Content,
            Order = 120)]
        public virtual XhtmlString MainBody { get; set; }

        [Display(
            Name = "Innehållsyta i sidfoten",
            GroupName = SystemTabNames.Content,
            Order = 5000)]
        [AllowedTypes(new[] { typeof(TextBlock) })]
        public virtual ContentArea FooterContentArea { get; set; }

    }
}