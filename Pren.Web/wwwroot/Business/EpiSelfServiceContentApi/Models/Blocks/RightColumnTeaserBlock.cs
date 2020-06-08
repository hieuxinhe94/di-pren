using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;

namespace Pren.Web.Business.EpiSelfServiceContentApi.Models.Blocks
{
    [ContentType(DisplayName = "Teaser, högerkolumn", GUID = "5690b347-d083-4ad6-a49f-08bad0b5019f", Description = "Teaser i högerkolumn för kundservicewebben", GroupName = "Kundservice")]
    public class RightColumnTeaserBlock : BlockData
    {
        [CultureSpecific]
        [Display(
            Name = "Titel",
            GroupName = SystemTabNames.Content,
            Order = 10)]
        public virtual string Title { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Text",
            GroupName = SystemTabNames.Content,
            Order = 20)]
        public virtual XhtmlString Text { get; set; }
    }
}