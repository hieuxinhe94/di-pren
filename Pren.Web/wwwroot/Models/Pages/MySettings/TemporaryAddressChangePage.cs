using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Pren.Web.Business;

namespace Pren.Web.Models.Pages.MySettings
{
    [ContentType(DisplayName = "Tillfällig adressändring", 
        GUID = "7ef1fb5f-5abc-40c8-b3aa-544c8250f2ce", 
        Description = "Tillfällig adressändring",
        GroupName = GroupNameConstants.MySettings)]
    [AvailableContentTypes(Availability.None)]
    public class TemporaryAddressChangePage : MyPageData
    {
        [Display(
            Name = "Mail, bekräftelsetext",
            Description = "Platshållare: [fromdate], [todate], [address]",
            GroupName = SystemTabNames.Content,
            Order = 200)]
        public virtual XhtmlString MailConfirmText { get; set; }
    }
}