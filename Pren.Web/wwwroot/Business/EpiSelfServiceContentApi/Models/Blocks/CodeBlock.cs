using System.ComponentModel.DataAnnotations;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Web;

namespace Pren.Web.Business.EpiSelfServiceContentApi.Models.Blocks
{
    [ContentType(DisplayName = "Kod", GUID = "ff5115de-5de1-41e1-b4c3-1ac88b1cbc32", Description = "Kod för kundservicewebben", GroupName = "Kundservice")]
    public class CodeBlock : BlockData
    {
        [Required]
        [CultureSpecific]
        [Display(
            Name = "Listid",
            GroupName = SystemTabNames.Content,
            Order = 10)]
        public virtual string CodeListId { get; set; }

        [Required]
        [CultureSpecific]
        [Display(
            Name = "Rubrik",
            GroupName = SystemTabNames.Content,
            Order = 20)]
        public virtual string Heading { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Bild",
            GroupName = SystemTabNames.Content,
            Order = 25)]
        [UIHint(UIHint.Image)]
        public virtual ContentReference Image { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Bild url",
            GroupName = SystemTabNames.Content,
            Order = 27)]
        public virtual Url ImageUrl { get; set; }

        [Required]
        [CultureSpecific]
        [Display(
            Name = "Instruktionstext",
            GroupName = SystemTabNames.Content,
            Order = 30)]
        public virtual XhtmlString InstructionText { get; set; }

        [Required]
        [CultureSpecific]
        [Display(
            Name = "Knapptext",
            GroupName = SystemTabNames.Content,
            Order = 40)]
        public virtual string ButtonText { get; set; }

        [Required]
        [CultureSpecific]
        [Display(
             Name = "Villkorstext",
             GroupName = SystemTabNames.Content,
             Order = 50)]
        public virtual XhtmlString TermsText { get; set; }
    }
}