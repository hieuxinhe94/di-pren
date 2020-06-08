using System;
using System.Web.Mvc;
using Di.Common.Utils.Url;
using Di.ServicePlus.RedirectApi;
using EPiServer.Core;
using EPiServer.Editor;
using EPiServer.ServiceLocation;
using Pren.Web.Business.Helpers;
using Pren.Web.Business.Subscription;

namespace Pren.Web.Business.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class AuthorizeCheckAttribute : ActionFilterAttribute
    {
        private readonly ISubscriberFacade _subscriberFacade;
        private readonly IUrlHelper _urlHelper;
        private readonly IRedirectHandler _redirectHandler;

        public AuthorizeCheckAttribute() 
        {
            _redirectHandler = ServiceLocator.Current.GetInstance<IRedirectHandler>();
            _subscriberFacade = ServiceLocator.Current.GetInstance<ISubscriberFacade>();
            _urlHelper = ServiceLocator.Current.GetInstance<IUrlHelper>();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            var subscriber = _subscriberFacade.GetSubscriberFromSession();

            if (!DoLoginCheck(subscriber)) return;

            var pageRouteHelper = ServiceLocator.Current.GetInstance<EPiServer.Web.Routing.PageRouteHelper>();
            ContentReference currentPageLink = pageRouteHelper.PageLink;

            var redirectUrl = GetCheckedLoggedInUrl(currentPageLink);
            filterContext.Result = new RedirectResult(redirectUrl);
        }

        /// <summary>
        /// If a code exists, always redirect. Otherwise redirect if Subscriber session is dead.
        /// In edit-mode we never redirect.
        /// </summary>
        /// <returns>If a redirect should be made.</returns>
        private bool DoLoginCheck(Subscriber subscriber)
        {
            var code = UrlUtils.GetQueryStringValue(UrlConstants.CodeQueryStringName);

            return (subscriber == null || !string.IsNullOrEmpty(code)) && !PageEditing.PageIsInEditMode;
        }

        /// <summary>
        /// Redirects to S+ to check if user is logged in. 
        /// </summary>
        /// <param name="currentContentReference">Current page reference.</param>
        private string GetCheckedLoggedInUrl(ContentReference currentContentReference)
        {
            var callbackUrl = _urlHelper.GetContentUrlWithHost(currentContentReference) + UrlConstants.AuthenticationCheckedCallBackAction;
            callbackUrl = UrlUtils.AddAllExistingQuerystrings(callbackUrl);

            return _redirectHandler.GetCheckedLoginUrl(callbackUrl);
        }
    }  
}