using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Di.Common.Utils.Url;
using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using Pren.Web.Business.Detection;
using Pren.Web.Business.Helpers;
using Pren.Web.Business.Subscription;
using Pren.Web.Models.Pages;
using Pren.Web.Models.Pages.MySettings;

namespace Pren.Web.Business.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeUserAttribute : AuthorizeAttribute
    {
        public bool ValidateSubscriptionId { get; set; }

        private readonly ISubscriberFacade _subscriberFacade;
        private readonly IContentRepository _contentRepository;
        private readonly IUrlHelper _urlHelper;
        private readonly IDetectionHandler _detection;

        public AuthorizeUserAttribute()
        {
            _detection = ServiceLocator.Current.GetInstance<IDetectionHandler>();
            _contentRepository = ServiceLocator.Current.GetInstance<IContentRepository>();
            _urlHelper = ServiceLocator.Current.GetInstance<IUrlHelper>();
            _subscriberFacade = ServiceLocator.Current.GetInstance<ISubscriberFacade>();
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null || httpContext.Session == null) return false;

            var subscriber = _subscriberFacade.GetSubscriberFromSession();
            var sessionIsValid = subscriber != null && subscriber.SelectedSubscription != null;
            var validUser = sessionIsValid && IsAuthorizedToEditSubscription(subscriber, UrlUtils.GetQueryStringValue("sid", httpContext.ApplicationInstance.Context));

            return validUser;
        }

        private bool IsAuthorizedToEditSubscription(Subscriber subscriber, string subscriptionNumber)
        {
            if (_detection.IsInEditMode() || (!ValidateSubscriptionId && string.IsNullOrEmpty(subscriptionNumber)))
            {
                return true;
            }

            long subsno;
            long.TryParse(subscriptionNumber, out subsno);

            return subsno != 0 && subscriber.SelectedSubscription.SubscriptionItems.Any(sub => sub.SubscriptionNumber == subsno);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext) 
        {
            var startPage = _contentRepository.Get<StartPage>(ContentReference.StartPage);
            var settingsStartpage = _contentRepository.Get<SitePageData>(startPage.MySettingsStartPage);

            var currentUrl = filterContext.HttpContext.Request.Url;
            var returnUrl = currentUrl != null ?
                UrlUtils.AddQueryString(settingsStartpage.LinkURL, "callbackUrl", _urlHelper.ReplaceHostName(currentUrl).AbsoluteUri) :
                settingsStartpage.LinkURL;

            // If ValidateSubscriptionId and no sid in querystring, redirect to startpage without callback
            if (ValidateSubscriptionId && string.IsNullOrEmpty(UrlUtils.GetQueryStringValue("sid", filterContext.HttpContext.ApplicationInstance.Context)))
            {                
                returnUrl = settingsStartpage.LinkURL;
            }

            filterContext.Result = new RedirectResult(returnUrl);            
                     
        }


    }
}