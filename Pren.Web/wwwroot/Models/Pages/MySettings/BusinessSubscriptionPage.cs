using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;

namespace Pren.Web.Models.Pages.MySettings
{
    [ContentType(DisplayName = "BusinessSubscriptionPage", GUID = "03a79760-168a-41b9-8ab7-2c3dd09018a3", Description = "")]
    public class BusinessSubscriptionPage : MyPageData
    {
        [Required]
        [Display(
            Name = "Hjälptext flikar",
            GroupName = SystemTabNames.Content,
            Order = 150)]
        public virtual XhtmlString HelpTabsBody { get; set; }

        [Required]
        [Display(
            Name = "Hjälptext import",
            GroupName = SystemTabNames.Content,
            Order = 160)]
        public virtual XhtmlString HelpImportBody { get; set; }
    }
}