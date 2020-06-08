using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;

namespace Pren.Web.Business.EpiSelfServiceContentApi.Models.Blocks
{
    [ContentType(DisplayName = "Viktigt meddelande", GUID = "EB5252FE-E900-4643-A073-C981EF6856D5", Description = "Vitkigt meddelande för kundservicewebben", GroupName = "Kundservice")]
    public class AlertBlock : BlockData
    {
        [Required]
        [CultureSpecific]
        [Display(
            Name = "Titel",
            GroupName = SystemTabNames.Content,
            Order = 10)]
        public virtual string Title { get; set; }

        [Required]
        [CultureSpecific]
        [Display(
            Name = "Text",
            GroupName = SystemTabNames.Content,
            Order = 20)]
        public virtual XhtmlString Text { get; set; }

        [Required]
        [CultureSpecific]
        [Display(
            Name = "Visa på kundwebben från",
            GroupName = SystemTabNames.Content,
            Order = 30)]
        public virtual DateTime VisibleFrom { get; set; }

        [Required]
        [CultureSpecific]
        [Display(
            Name = "Visa på kundwebben till",
            GroupName = SystemTabNames.Content,
            Order = 40)]
        public virtual DateTime VisibleTo { get; set; }
    }
}