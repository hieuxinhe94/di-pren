using System.Web.Mvc;
using EPiServer;
using EPiServer.Web.Mvc;
using Pren.Web.Business.Attributes;
using Pren.Web.Models.Pages;
using Pren.Web.Models.ViewModels;

namespace Pren.Web.Controllers
{
    [NoCache]
    public class CampaignLandingPageController : PageController<CampaignLandingPage>
    {
        public ActionResult Index(CampaignLandingPage currentPage, string callback)
        {
            var model = new CampaignLandingPageViewModel(currentPage)
            {
                ButtonLeftUrl = GetCallbackUrl(currentPage.ButtonLeftUseCallback, callback, currentPage.ButtonLeftUrl),
                ButtonRightUrl = GetCallbackUrl(currentPage.ButtonRightUseCallback, callback, currentPage.ButtonRightUrl)
            };

            return View("Index",model); 
        }

        public Url GetCallbackUrl(bool useCallbackUrl, string callbackUrl, Url defaultUrl)
        {
            if (useCallbackUrl)
            {
                return string.IsNullOrEmpty(callbackUrl) ? defaultUrl : new Url(callbackUrl);
            }

            return defaultUrl;
        }
    }
}
