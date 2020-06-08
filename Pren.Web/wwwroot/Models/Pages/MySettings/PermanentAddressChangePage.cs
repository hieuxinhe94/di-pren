using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Pren.Web.Business;

namespace Pren.Web.Models.Pages.MySettings
{
    [ContentType(DisplayName = "Permanent adress", 
        GUID = "1fd776ea-0122-425d-80b7-5710a091e97b", 
        Description = "Permanent adress",
        GroupName = GroupNameConstants.MySettings)]
    [AvailableContentTypes(Availability.None)]
    public class PermanentAddressChangePage : MyPageData
    {
        [Display(
            Name = "Mail, bekräftelsetext",
            Description = "Platshållare: [fromdate], [address]",
            GroupName = SystemTabNames.Content,
            Order = 200)]
        public virtual XhtmlString MailConfirmText { get; set; }
    }
}