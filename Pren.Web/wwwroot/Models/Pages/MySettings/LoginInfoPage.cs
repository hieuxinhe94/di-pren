using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Pren.Web.Business;

namespace Pren.Web.Models.Pages.MySettings
{
    [ContentType(DisplayName = "Inloggningsuppgifter", 
        GUID = "585552a9-a1d4-45f9-ab8e-6c632fe622fe", 
        Description = "Inloggningsuppgifter",
        GroupName = GroupNameConstants.MySettings)]
    [AvailableContentTypes(Availability.None)]
    public class LoginInfoPage : SitePageData
    {
    }
}