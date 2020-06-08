using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Pren.Web.Business.EpiCompanyServiceContentApi.Models.Pages;
using Pren.Web.Business.Rendering;

namespace Pren.Web.Models.Pages
{
    [ContentType(DisplayName = "ContainerPage", GUID = "fe5f7c4e-8343-4cd1-8031-4dd572f839f6", Description = "Behållare")]
    [AvailableContentTypes(
        Availability.Specific, Include = new[]
        {
            typeof(CampaignPage), 
            typeof(BusinessCampaignPage),
            typeof(BusinessSubscriptionActivationPage),
            typeof(TextPage),
            typeof(CampaignLandingPage),
            typeof(OrderFlowCampaignPage),
            typeof(OnBoardingCampaignPage),
            typeof(CompanyServiceHomePage)
        })]
    public class ContainerPage : SitePageData, IContainerPage
    {

    }
}