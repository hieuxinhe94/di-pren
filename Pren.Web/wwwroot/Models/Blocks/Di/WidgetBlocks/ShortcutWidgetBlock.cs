using System.ComponentModel.DataAnnotations;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using Pren.Web.Business.CustomProperties;

namespace Pren.Web.Models.Blocks.Di.WidgetBlocks
{
    [ContentType(
        DisplayName = "Genväg till funktion Widget", 
        GUID = "df0d91ab-e793-499e-a4f1-883e9c0fddcd", 
        Description = "Widget med genväg till funktioner. Funktioner som kan ha genväg: Uppehåll", 
        GroupName = "Widget")]
    public class ShortcutWidgetBlock : WidgetBlockData
    {
        [Required]
        [Display(
            Name = "Text på genvägsknapp",
            GroupName = SystemTabNames.Content,
            Order = 400)]
        public virtual string ShortcutLinkText { get; set; }

        [Required]
        [Display(
            Name = "Genväg för widgeten",
            GroupName = SystemTabNames.Content,
            Order = 500)]
        [SelectOne(SelectionFactoryType = typeof(ShortcutSelectionFactory))]
        public virtual string Shortcut { get; set; }
    }
}