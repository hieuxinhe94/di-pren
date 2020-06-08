using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Pren.Web.Business;

namespace Pren.Web.Models.Pages.MySettings
{
    [ContentType(DisplayName = "Kontaktuppgifter", 
        GUID = "e624e5f3-0d9d-4e59-b4a3-c2e586b85b62",
        Description = "Kontaktuppgifter",
        GroupName = GroupNameConstants.MySettings)]
    [AvailableContentTypes(Availability.None)]
    public class PersonInfoPage : MyPageData
    {
    }
}