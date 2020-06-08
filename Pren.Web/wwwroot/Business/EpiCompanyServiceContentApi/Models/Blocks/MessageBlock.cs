using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using Pren.Web.Business.CustomProperties;

namespace Pren.Web.Business.EpiCompanyServiceContentApi.Models.Blocks
{
    [ContentType(DisplayName = "Viktigt meddelande", GUID = "B8755EA9-F173-4B0D-A261-64C204F59721", Description = "Vitkigt meddelande för företagsservice", GroupName = "Företagsservice")]
    public class MessageBlock : BlockData
    {
        [Required]
        [Display(
            Name = "MeddelandeTyp",
            GroupName = SystemTabNames.Content,
            Order = 5)]
        [SelectOne(SelectionFactoryType = typeof(MessageTypeSelectionFactory))]
        public virtual string MessageType { get; set; }

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
            Name = "Visa från",
            GroupName = SystemTabNames.Content,
            Order = 30)]
        public virtual DateTime VisibleFrom { get; set; }

        [Required]
        [CultureSpecific]
        [Display(
            Name = "Visa till",
            GroupName = SystemTabNames.Content,
            Order = 40)]
        public virtual DateTime VisibleTo { get; set; }
    }
}