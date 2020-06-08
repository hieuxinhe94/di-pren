using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;

namespace Pren.Web.Models.Blocks.Di
{
    public abstract class AnchorBlockData : DiMySettingsBlockData
    {
        public string AnchorId;

        protected AnchorBlockData(string anchorId)
        {
            this.AnchorId = anchorId;
        }

        [CultureSpecific]
        [Display(
            Name = "Rubrik",
            GroupName = SystemTabNames.Content,
            Order = 10)]
        public virtual string Heading { get; set; }
        
        [Display(
            Name = "Text",
            GroupName = SystemTabNames.Content,
            Order = 25)]
        public virtual XhtmlString Editor { get; set; }

        [Required]
        [CultureSpecific]
        [Display(
            Name = "Rubrik i meny",
            Description = "Visas som rubrik i toppmenyn",
            GroupName = SystemTabNames.Content,
            Order = 50)]
        public virtual string MenuName { get; set; }
    }
}
