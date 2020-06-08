using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Pren.Web.Business;

namespace Pren.Web.Models.Blocks.Di
{
    [ContentType(DisplayName = "Kodportal", GUID = "92294bc4-242d-4765-a91e-eb14d6f426c1", Description = "Mitt Di - Kodportal", GroupName = GroupNameConstants.MySettings)]
    public class MyCodesBlock : AnchorBlockData
    {
        public MyCodesBlock() : base("mycodes")
        {

        }

        [Display(
            Name = "Koderbjudanden",
            GroupName = SystemTabNames.Content,
            Order = 100)]
        [AllowedTypes(new[] { typeof(CodePortalOfferBlock), typeof(GiveAwayOfferBlock) })]
        public virtual ContentArea CodeOffersContentArea { get; set; }
    }
}
