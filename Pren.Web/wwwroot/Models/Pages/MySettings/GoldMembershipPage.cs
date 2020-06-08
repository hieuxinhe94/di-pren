using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Pren.Web.Business;

namespace Pren.Web.Models.Pages.MySettings
{
    [ContentType(DisplayName = "Di Guld medlemskap", 
        GUID = "e18d9769-5e3a-4479-9d8e-a1bdb20d6f71", 
        Description = "Di Guld medlemskap",
        GroupName = GroupNameConstants.MySettings)]
    [AvailableContentTypes(Availability.None)]
    public class GoldMembershipPage : SitePageData
    {
    }
}