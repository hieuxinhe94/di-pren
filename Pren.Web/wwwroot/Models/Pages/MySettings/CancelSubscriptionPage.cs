using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Pren.Web.Business;

namespace Pren.Web.Models.Pages.MySettings
{
    [ContentType(DisplayName = "Avsluta prenumeration", 
        GUID = "1166a262-01d4-428c-b860-e860bf095122",
        Description = "Avsluta prenumeration", 
        GroupName = GroupNameConstants.MySettings)]
    [AvailableContentTypes(Availability.None)]
    public class CancelSubscriptionPage : MyPageData
    {

    }
}