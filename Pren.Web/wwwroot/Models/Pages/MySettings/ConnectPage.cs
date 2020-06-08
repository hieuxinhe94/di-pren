using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Pren.Web.Business;

namespace Pren.Web.Models.Pages.MySettings
{
    [ContentType(DisplayName = "Koppla",
        GUID = "92fc7c01-5a6f-4b1e-b3b4-626081c5858f",
        Description = "Koppla prenumeration med S+",
        GroupName = GroupNameConstants.MySettings)]
    [AvailableContentTypes(Availability.None)]
    public class ConnectPage : MyPageData
    {
        [Display(
            Name = "Innehåll, logga in eller skapa nytt konto",
            GroupName = SystemTabNames.Content,
            Order = 100)]
        public virtual XhtmlString ExistingPrenBody { get; set; }
    }
}