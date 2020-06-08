using System.Web.Routing;
using Di.Common.Utils.Url;
using EPiServer;
using EPiServer.Core;
using Pren.Web.Business.Campaign;
using Pren.Web.Business.Configuration;
using Pren.Web.Business.Detection;
using Pren.Web.Business.Helpers;
using Pren.Web.Business.MyPage;
using Pren.Web.Business.Subscription;
using Pren.Web.Models.Pages;
using Pren.Web.Models.Pages.MySettings;
using Pren.Web.Models.ViewModels;

namespace Pren.Web.Business
{
    public class PageViewContextFactory
    {
        private readonly IContentLoader _contentLoader;
        private readonly IUrlHelper _urlHelper;
        private readonly IDetectionHandler _detectionHandler;
        private readonly ISiteSettings _siteSettings;
        private readonly ISubscriberFacade _subscriberFacade;

        public PageViewContextFactory(
            IContentLoader contentLoader, 
            IUrlHelper urlHelper, 
            IDetectionHandler detectionHandler,
            ISiteSettings siteSettings, ISubscriberFacade subscriberFacade)
        {
            _contentLoader = contentLoader;
            _urlHelper = urlHelper;
            _detectionHandler = detectionHandler;
            _siteSettings = siteSettings;
            _subscriberFacade = subscriberFacade;
        }

        public virtual LayoutModel CreateLayoutModel(ContentReference currentContentLink, RequestContext requestContext)
        {
            var startPage = _contentLoader.Get<StartPage>(ContentReference.StartPage);

            return new LayoutModel
                {
                    Copyright = startPage.Copyright,
                    PrenTermsText = startPage.PrenTermsText,
                    FooterContentArea = startPage.FooterContentArea,
                    ChatText = startPage.CustomerServiceText,
                    MySettings = GetMySettingsLayout(currentContentLink, startPage),
                    IsMobile = _detectionHandler.IsMobileDevice(),
                    OptimizelyDesktopScriptSrc = _siteSettings.OptimizelyDesktopScriptSrc,
                    OptimizelyMobileScriptSrc = _siteSettings.OptimizelyMobileScriptSrc,
                    Campaign = GetCampaignLayout(currentContentLink, startPage)
                };
        }

        private CampaignLayout GetCampaignLayout(ContentReference currentContentLink, StartPage startPage)
        {
            var campaignLayout = new CampaignLayout();

            var page = _contentLoader.Get<IContent>(currentContentLink);

            var campaignPage = page as CampaignPageSplus;
            if (campaignPage == null) return campaignLayout;
            campaignLayout.ChooseCampaignPage = campaignPage.ChooseCampaignPage;
            campaignLayout.ShowDigitalLink = campaignPage.ShowDigitalLink;
            campaignLayout.DisclaimerText = startPage.DisclaimerText;

            return campaignLayout;
        }

        private MySettingsLayout GetMySettingsLayout(ContentReference currentContentLink, StartPage  startPage)
        {
            var mySettingsLayout = new MySettingsLayout();            

            var subscriber = _subscriberFacade.GetSubscriberFromSession();
            mySettingsLayout.IsLoggedIn = subscriber != null &&
                (subscriber.SelectedSubscription != null || subscriber.ServicePlusUser != null);

            var currentUrl = _urlHelper.GetContentUrlWithHost(currentContentLink);
            var loginUrl = currentUrl + UrlConstants.LoginAction;
            mySettingsLayout.LogInUrl = UrlUtils.AddAllExistingQuerystrings(loginUrl);
            mySettingsLayout.LogOutUrl = currentUrl + UrlConstants.LogoutAction;
            mySettingsLayout.CreateAccountUrl = currentUrl + UrlConstants.CreateAction;

            mySettingsLayout.UserName = (subscriber != null && subscriber.SelectedSubscription != null && subscriber.SelectedSubscription.KayakCustomer != null)
                ? subscriber.SelectedSubscription.KayakCustomer.FullName
                : string.Empty;

            var mySettingsStartpage = _contentLoader.Get<SitePageData>(startPage.MySettingsStartPage);

            mySettingsLayout.StartPage = mySettingsStartpage;
            mySettingsLayout.HideDbFunctions = startPage.HideDbFunctions;
            mySettingsLayout.FooterContentArea = mySettingsStartpage.GetOriginalType() == typeof (MySettingsStartPage)
                ? ((MySettingsStartPage) mySettingsStartpage).FooterContentArea
                : null;
            
            return mySettingsLayout;
        }

    }
}