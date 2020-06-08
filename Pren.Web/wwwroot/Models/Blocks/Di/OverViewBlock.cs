using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Pren.Web.Business;

namespace Pren.Web.Models.Blocks.Di
{
    [ContentType(DisplayName = "Översikt", GUID = "929247a1-aad4-4e1b-b594-9545a180d631", Description = "Mitt Di - Översikt", GroupName = GroupNameConstants.MySettings)]
    public class OverviewBlock : AnchorBlockData
    {

        public OverviewBlock() : base("overview")
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
            Name = "Visa koppla-information",
            GroupName = SystemTabNames.Content,
            Order = 30)]
        public virtual bool ShowConnectInfo { get; set; }

        [Display(
            Name = "Dashboard",
            GroupName = SystemTabNames.Content,
            Order = 5000)]
        //[AllowedTypes(new[] { typeof(TextBlock) })]
        public virtual ContentArea DashboardContentArea { get; set; }

    }
}