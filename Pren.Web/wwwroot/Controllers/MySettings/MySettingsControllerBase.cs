using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Di.Common.Utils.Url;
using Di.ServicePlus.RedirectApi;
using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using Pren.Web.Business.Attributes;
using Pren.Web.Business.Helpers;
using Pren.Web.Business.Session;
using Pren.Web.Business.Subscription;
using Pren.Web.Models.Pages;
using Pren.Web.Models.Pages.MySettings;

namespace Pren.Web.Controllers.MySettings
{
    [NoCache]
    public abstract class MySettingsControllerBase<T> : PageControllerBase<T> where T : SitePageData
    {

        #region Fields

        private readonly ISessionData _sessionData;
        private readonly IUrlHelper _urlHelper;
        private readonly IContentRepository _contentRepository;
        private readonly IConnectService _connectService;

        private readonly IRedirectHandler _redirectHandler;
        private readonly ISubscriberFacade _subscriberFacade;

        #endregion

        #region Constructor

        protected MySettingsControllerBase(
            ISessionData sessionData)
        {
            _sessionData = sessionData;
            _subscriberFacade = ServiceLocator.Current.GetInstance<ISubscriberFacade>();
            _urlHelper = ServiceLocator.Current.GetInstance<IUrlHelper>();
            _contentRepository = ServiceLocator.Current.GetInstance<IContentRepository>();
            _connectService = ServiceLocator.Current.GetInstance<IConnectService>();
            _redirectHandler = ServiceLocator.Current.GetInstance<IRedirectHandler>();
        }

        #endregion

        #region Actions

        public async Task<ActionResult> AuthChecked(SitePageData currentPage, string token, string code, string callbackUrl)
        {
            await SetSubscriptionUser(token, code);

            if (string.IsNullOrEmpty(callbackUrl)) return RedirectToAction("Index");

            callbackUrl = UrlUtils.RemoveQueryStringByKey(callbackUrl, "token");
            return Redirect(callbackUrl);
        }

        public ActionResult LogIn(SitePageData currentPage, string callbackUrl)
        {
            callbackUrl = string.IsNullOrEmpty(callbackUrl)
                ? _urlHelper.GetContentUrlWithHost(currentPage.ContentLink)
                : callbackUrl;

            return Redirect(_redirectHandler.GetLoginUrl(callbackUrl));
        }

        public ActionResult LogOut(SitePageData currentPage)
        {
            SetSubscriberInSession(null);

            var callbackUrl = _urlHelper.GetContentUrlWithHost(currentPage.ContentLink);

            return Redirect(_redirectHandler.GetLogoutUrl(callbackUrl));
        }

        public ActionResult Create(SitePageData currentPage)
        {
            var callbackUrl = _urlHelper.GetContentUrlWithHost(currentPage.ContentLink);

            return Redirect(_redirectHandler.GetCreateAccountUrl(callbackUrl));
        }

        [AuthorizeUser]
        public ActionResult SwitchSubscriptionType(SitePageData currentPage, string subscriptionType)
        {
            var subscriber = GetSubscriberFromSession();
            SitePageData redirectPage = GetMySettingsStartPage();

            if (subscriptionType == "private")
            {
                subscriber.SelectedSubscription = subscriber.PrivateSubscription;
            }

            if (subscriptionType == "business")
            {
                subscriber.SelectedSubscription = subscriber.BusinessSubscription;
                // Fallback to mysettingstartpage if BusinessSubscriptionAdminPage is not set
                redirectPage = GetBusinessSubscriptionAdminPage() ?? redirectPage;
            }

            SetSubscriberInSession(subscriber);

            return Redirect(redirectPage.LinkURL);
        }

        #endregion

        #region Methods

        private BusinessSubscriptionPage GetBusinessSubscriptionAdminPage()
        {
            var startPage = GetStartPage();

            return startPage.BusinessSubscriptionAdminPage != null ? _contentRepository.Get<BusinessSubscriptionPage>(startPage.BusinessSubscriptionAdminPage) : null;
        }

        private SitePageData GetMySettingsStartPage()
        {
            var startPage = GetStartPage();

            return _contentRepository.Get<SitePageData>(startPage.MySettingsStartPage);
        }

        private StartPage GetStartPage()
        {
            return _contentRepository.Get<StartPage>(ContentReference.StartPage);
        }

        protected ConnectPage GetConnectPage()
        {
            var startPage = GetStartPage();

            return _contentRepository.Get<ConnectPage>(startPage.ConnectPage);
        }

        protected ConnectStatus GetConnectStatus(Subscriber subscriber)
        {
            return _connectService.GetConnectStatus(subscriber);
        }

        /// <summary>
        /// Redirects to S+ to check if user is logged in. 
        /// </summary>
        /// <param name="currentContentReference">Current page reference.</param>
        protected void RedirectCheckLoggedIn(ContentReference currentContentReference)
        {
            var callbackUrl = _urlHelper.GetContentUrlWithHost(currentContentReference) + UrlConstants.AuthenticationCheckedCallBackAction;
            callbackUrl = UrlUtils.AddAllExistingQuerystrings(callbackUrl);

            Response.Redirect(_redirectHandler.GetCheckedLoginUrl(callbackUrl));
        }

        /// <summary>
        /// If Subscriber session exist, with no selectedSubscription and no S+ user. Session will be cleared so a check is made on every request 
        /// </summary>
        protected void ClearInvalidSession(Subscriber subscriber)
        {
            if (subscriber != null && (subscriber.SelectedSubscription == null && subscriber.ServicePlusUser == null))
            {
                SetSubscriberInSession(null);
            }
        }

        protected SubscriptionItem GetSubscriptionItem(Subscriber subscriber, string subscriptionNumber)
        {
            return subscriber.SelectedSubscription.Type == SubscriptionType.Dummy 
                ? subscriber.SelectedSubscription.SubscriptionItems.First() 
                : subscriber.SelectedSubscription.SubscriptionItems.FirstOrDefault(subscriptionItem => subscriptionItem.SubscriptionNumber == long.Parse(subscriptionNumber));
        }

        /// <summary>
        /// Sets the subscriber based on token and/or code.
        /// </summary>
        /// <param name="token">Token from S+</param>
        /// <param name="code">Code from url</param>
        protected async Task SetSubscriptionUser(string token, string code)
        {
            var subscriber = !string.IsNullOrEmpty(code)
                ? await _subscriberFacade.GetSubscriberByCode(code, token)
                : await _subscriberFacade.GetSubscriber(token);

            SetSubscriberInSession(subscriber);
        }

        protected Subscriber GetSubscriberFromSession()
        {            
            return _subscriberFacade.GetSubscriberFromSession();
        }

        protected async Task<Subscriber> UpdateSubscriberFromSources(Subscriber subscriberToUpdate)
        {
            var subscriber = await _subscriberFacade.GetSubscriber(subscriberToUpdate.ServicePlusToken);
            _sessionData.Set(SessionConstants.SubscriberSessionKey, subscriber);
            return subscriber;
        }

        protected void SetSubscriberInSession(Subscriber subscriber)
        {
            _sessionData.Set(SessionConstants.SubscriberSessionKey, subscriber);            
        }

        #endregion

    }
}
