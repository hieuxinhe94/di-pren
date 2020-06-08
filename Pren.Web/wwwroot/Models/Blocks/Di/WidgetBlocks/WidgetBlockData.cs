using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Pren.Web.Business.Rendering;

namespace Pren.Web.Models.Blocks.Di.WidgetBlocks
{
    public class WidgetBlockData : DiMySettingsBlockData, ICustomCssInContentArea
    {
        public WidgetBlockData()
        {
            ContentAreaCssClass = "dragend-item";            
        }

        [CultureSpecific]
        [Display(
            Name = "Rubrik",
            GroupName = SystemTabNames.Content,
            Order = 10)]
        public virtual string Heading { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Använd bakgrundsfärg",
            GroupName = SystemTabNames.Content,
            Order = 20)]
        public virtual bool UseBackgroundColor { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Text",
            GroupName = SystemTabNames.Content,
            Order = 100)]
        public virtual XhtmlString Editor { get; set; }

        public string ContentAreaCssClass { get; private set; }
    }
}
