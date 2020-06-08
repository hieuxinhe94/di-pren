using System.ComponentModel.DataAnnotations;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Web;
// ReSharper disable Mvc.TemplateNotResolved

namespace Pren.Web.Business.EpiSelfServiceContentApi.Models.Blocks
{
    [ContentType(DisplayName = "Teaser", GUID = "DC6CEB4C-04F6-402A-BAAD-2A54626F16D9", Description = "Teaser för kundservicewebben", GroupName = "Kundservice")]
    public class TeaserBlock : BlockData
    {
        [CultureSpecific]
        [Display(
            Name = "Titel",
            GroupName = SystemTabNames.Content,
            Order = 10)]
        public virtual string Title { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Bild",
            GroupName = SystemTabNames.Content,
            Order = 20)]
        [UIHint(UIHint.Image)]
        public virtual ContentReference Image { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Bild url",
            GroupName = SystemTabNames.Content,
            Order = 25)]
        public virtual Url ImageUrl { get; set; } 

        [CultureSpecific]
        [Display(
            Name = "Text",
            GroupName = SystemTabNames.Content,
            Order = 30)]
        public virtual XhtmlString Text { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Knapptext",
            GroupName = SystemTabNames.Content,
            Order = 40)]
        public virtual string ButtonText { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Knapplänk",
            GroupName = SystemTabNames.Content,
            Order = 50)]
        public virtual Url ButtonLinkUrl { get; set; }
    }
}