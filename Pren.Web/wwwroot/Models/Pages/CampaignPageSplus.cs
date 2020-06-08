using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Pren.Web.Business;

namespace Pren.Web.Models.Pages
{
    [ContentType(DisplayName = "(Använd ej) Kampanjsida redirect", GUID = "3410f281-41c2-4ef3-859b-b03ebcce7c02", Description = "(Använd ej) Kampanjsida redirect", GroupName = "Kampanj")]
    [AvailableContentTypes(Availability.None)]
    public class CampaignPageSplus : CampaignPage
    {
        [Display(
           Name = "Försättssida",
           GroupName = CustomTabs.Advanced,
           Order = 5010)]
        public virtual ContentReference ChooseCampaignPage { get; set; }

        [Display(
           Name = "Visa digital länk",
           Description = "Styr vilken länktext som ska användas i länken till försättssidan.",
           GroupName = CustomTabs.Advanced,
           Order = 5020)]
        public virtual bool ShowDigitalLink { get; set; }
    }
}