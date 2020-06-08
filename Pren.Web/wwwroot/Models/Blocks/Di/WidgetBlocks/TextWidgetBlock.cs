using System.ComponentModel.DataAnnotations;
using EPiServer;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using Pren.Web.Business.CustomProperties;

namespace Pren.Web.Models.Blocks.Di.WidgetBlocks
{
    [ContentType(
        DisplayName = "Text Widget", 
        GUID = "6fff18b2-c01a-4ac0-8249-5516835d1c14", 
        Description = "Widget med rubrik, text och eventuell länk",
        GroupName = "Widget")]
    public class TextWidgetBlock : WidgetBlockData
    {
        [Display(
            Name = "Ikontyp för widgeten",
            GroupName = SystemTabNames.Content,
            Order = 5)]
        [SelectOne(SelectionFactoryType = typeof(IconSelectionFactory))]
        public virtual string IconType { get; set; }

        [Display(
            Name = "Länk",
            Description = "Länken visas som en knapp",
            GroupName = SystemTabNames.Content,
            Order = 110)]
        public virtual Url Link { get; set; }

        [Display(
            Name = "Länktext",
            Description = "Länken visas som en knapp",
            GroupName = SystemTabNames.Content,
            Order = 120)]
        public virtual string LinkText { get; set; }

        [Display(
            Name = "Öppna länk i ny flik",
            GroupName = SystemTabNames.Content,
            Order = 130)]
        public virtual bool OpenLinkInNewTab { get; set; }
    }
}