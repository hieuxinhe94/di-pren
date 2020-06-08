using System.ComponentModel.DataAnnotations;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;

namespace Pren.Web.Models.Pages
{
    [ContentType(DisplayName = "Kampanjsida Iframe", GUID = "e57c56dd-c91b-49e2-8f1f-7e8e6727bbe7", Description = "Kampanjsida Iframe", GroupName = "Kampanj")]
    [AvailableContentTypes(Availability.None)]
    public class CampaignPageIframe : CampaignPageSplus
    {
        [Display(
            Name = "Använd nya iframeflödet",
            Description = "Fungerar för både digitala- och papperskampanjer",
            GroupName = SystemTabNames.Content,
            Order = 6020)]
        public virtual bool UseLatestIframeFlow { get; set; }
    }
}