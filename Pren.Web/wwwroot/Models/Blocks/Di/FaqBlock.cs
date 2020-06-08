using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Pren.Web.Business;

namespace Pren.Web.Models.Blocks.Di
{
    [ContentType(DisplayName = "Faq", GUID = "6cec37de-7c6c-48ff-9c60-f7261810273d", Description = "Mitt Di - faq", GroupName = GroupNameConstants.MySettings)]
    public class FaqBlock : AnchorBlockData
    {

        public FaqBlock() : base("faq")
        {
            
        }
        
        [CultureSpecific]
        [Display(
            Name = "Text",
            GroupName = SystemTabNames.Content,
            Order = 25)]
        public virtual XhtmlString Editor { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Text under dropdown",
            GroupName = SystemTabNames.Content,
            Order = 30)]
        public virtual XhtmlString EditorBelowDropDown { get; set; }
    }
}