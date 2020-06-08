using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Pren.Web.Business;

namespace Pren.Web.Models.Pages.MySettings
{
    [ContentType(DisplayName = "Autodragning på kort", 
        GUID = "4304c7a9-05e0-474e-a6ab-0ca51259be86",
        Description = "Autodragning på kort",
        GroupName = GroupNameConstants.MySettings)]
    [AvailableContentTypes(Availability.None)]
    public class AutowithdrawalPage : MyPageData
    {
    }
}