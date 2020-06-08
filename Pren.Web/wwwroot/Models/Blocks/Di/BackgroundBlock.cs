using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Web;
using Pren.Web.Business;

namespace Pren.Web.Models.Blocks.Di
{
    [ContentType(DisplayName = "Bakgrund", GUID = "57777887-9081-421e-a17a-978a87d2f392", Description = "Mitt Di - bakgrund parallax", GroupName = GroupNameConstants.MySettings)]
    public class BackgroundBlock : DiMySettingsBlockData
    {        
        [CultureSpecific]
        [UIHint(UIHint.Image)]
        [Display(
            Name = "Bakgrundsbild",
            GroupName = SystemTabNames.Content,
            Order = 30)]
        public virtual ContentReference Image { get; set; }

    }
}