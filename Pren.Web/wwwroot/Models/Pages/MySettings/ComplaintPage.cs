using System.ComponentModel.DataAnnotations;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Pren.Web.Business;

namespace Pren.Web.Models.Pages.MySettings
{
    [ContentType(DisplayName = "Reklamation", 
        GUID = "0f354a0c-6615-44db-b24d-5e25e9012a3b", 
        Description = "Reklamation",
        GroupName = GroupNameConstants.MySettings)]
    [AvailableContentTypes(Availability.None)]
    public class ComplaintPage : MyPageData
    {
        [Display(
            Name = "Aktivera - reklamtioner Kayak",
            GroupName = SystemTabNames.Settings,
            Order = 200)]
        public virtual bool ActivateComplaintsKayak { get; set; }
    }
}