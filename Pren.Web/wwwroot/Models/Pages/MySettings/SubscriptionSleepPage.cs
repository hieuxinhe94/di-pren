using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Pren.Web.Business;

namespace Pren.Web.Models.Pages.MySettings
{
    [ContentType(DisplayName = "Uppehåll", 
        GUID = "ce92c50e-0057-4371-aecd-a31f09893eeb", 
        Description = "Uppehåll", 
        GroupName = GroupNameConstants.MySettings)]
    [AvailableContentTypes(Availability.None)]
    public class SubscriptionSleepPage : MyPageData
    {
        [Display(
            Name = "Mail, bekräftelsetext",
            Description = "Platshållare: [fromdate], [todate]",
            GroupName = SystemTabNames.Content,
            Order = 200)]
        public virtual XhtmlString MailConfirmText { get; set; }
    }
}