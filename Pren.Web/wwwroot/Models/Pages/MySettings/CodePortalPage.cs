using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using Pren.Web.Business;
using Pren.Web.Models.Blocks;

namespace Pren.Web.Models.Pages.MySettings
{
    [ContentType(
        DisplayName = "Kodportal", 
        GUID = "f0c4316c-3cec-46b6-b2bb-7d91bdf0ab7c", 
        Description = "",
        GroupName = GroupNameConstants.MySettings)]
    public class CodePortalPage : MyPageData
    {
        [Display(
            Name = "Innehållsyta för koderbjudanden",
            Description = "Innehållsyta för koderbjudanden",
            GroupName = SystemTabNames.Content,
            Order = 500)]
        [AllowedTypes(new[] { typeof(CodePortalOfferBlock) })]
        public virtual ContentArea CodePortalOfferContentArea { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Fallback om erbjudanden saknas",
            GroupName = CustomTabs.Fallback,
            Order = 30)]
        public virtual XhtmlString FallbackText { get; set; }
    }
}