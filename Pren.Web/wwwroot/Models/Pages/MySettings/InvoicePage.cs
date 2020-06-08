using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Pren.Web.Business;

namespace Pren.Web.Models.Pages.MySettings
{
    [ContentType(DisplayName = "Faktura", 
        GUID = "37596200-e3ed-4c82-b8b2-2cd0b42294bf", 
        Description = "Faktura", 
        GroupName = GroupNameConstants.MySettings)]
    [AvailableContentTypes(Availability.None)]
    public class InvoicePage : MyPageData
    {
    }
}