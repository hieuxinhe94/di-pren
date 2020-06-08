using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bn.Subscription;
using Di.Common.Logging;
using Di.ServicePlus.RedirectApi;
using EPiServer;
using EPiServer.Core;
using EPiServer.Web.Mvc.Html;
using Pren.Web.Business.Attributes;
using Pren.Web.Business.Helpers;
using Pren.Web.Business.Invoice;
using Pren.Web.Business.Session;
using Pren.Web.Business.Subscription;
using Pren.Web.Models.Blocks.Di;
using Pren.Web.Models.Pages;
using Pren.Web.Models.Pages.MySettings;
using Pren.Web.Models.Pages.MySettings.Di;
using Pren.Web.Models.ViewModels.MySettings.Di;

namespace Pren.Web.Controllers.MySettings.Di
{
    // ReSharper disable Mvc.ViewNotResolved
    [NoCache]
    public class MyStartPageController :  PageControllerBase<MyStartPage>
    {
        private readonly ISubscriberFacade _subscriberFacade;
        private readonly ISessionData _sessionData;
        private readonly IConnectService _connectService;
        private readonly IInvoiceFacade _invoiceFacade;
        private readonly ILogger _logger;
        private readonly IUrlHelper _urlHelper;
        private readonly IRedirectHandler _redirectHandler;
        private readonly IContentRepository _contentRepository;
        private readonly ISubscriptionApi _subscriptionApi;

        public MyStartPageController(
            ISubscriberFacade subscriberFacade, 
            ISessionData sessionData, 
            IConnectService connectService, 
            IInvoiceFacade invoiceFacade, 
            ILogger logger, 
            IUrlHelper urlHelper, 
            IRedirectHandler redirectHandler, 
            IContentRepository contentRepository, ISubscriptionApi subscriptionApi)
        {
            _subscriberFacade = subscriberFacade;
            _sessionData = sessionData;
            _connectService = connectService;
            _invoiceFacade = invoiceFacade;
            _logger = logger;
            _urlHelper = urlHelper;
            _redirectHandler = redirectHandler;
            _contentRepository = contentRepository;
            _subscriptionApi = subscriptionApi;
        }

        [AuthorizeCheck]
        public ActionResult Index(MyStartPage currentPage, string anchor)
        {
            var subscriber = (Subscriber)_sessionData.Get(SessionConstants.SubscriberSessionKey);
            var model = new MySettingsViewModel(currentPage);

            ClearInvalidSession(subscriber);

            switch (_connectService.GetConnectStatus(subscriber))
            {
                case ConnectStatus.InvalidCode:
                case ConnectStatus.NothingToConnect:
                    return GetNotLoggedInView(currentPage,false);
                case ConnectStatus.ConnectExistingPrenWithServicePlus:
                case ConnectStatus.ConnectExistingServicePlusWithPren:
                case ConnectStatus.ConnectExistingServicePlusWithExistingPren:
                    //return GetNotLoggedInView(currentPage, true, "För att kunna administrera din prenumeration samt få tillgång till hela nyhetsutbudet behöver du koppla ditt konto. Gör det redan idag på <a href='http://www.di.se/koppla'>di.se/koppla</a>");
                    return GetNotLoggedInView(currentPage, true);
                case ConnectStatus.UnableToConnectPrenWithServicePlus:
                    return GetNotLoggedInView(currentPage, true, "Ditt Di-konto är kopplat till ett annat kundnummer. Vänligen kontakta kundservice.");
            }

            
            model.UserName = subscriber.ServicePlusUser.FirstName + " " + subscriber.ServicePlusUser.LastName;
            model.IsDebug = IsDebug();
            model.IsLoggedIn = true;
            model.TopMenuItems = GetTopMenuItems(currentPage.ContentArea);
            model.Anchor = TempData["anchor"] as string ?? anchor;

            var startPage = _contentRepository.Get<StartPage>(ContentReference.StartPage);
            var hasBizSubscription = subscriber.BusinessSubscription != null;

            model.BusinessSubscriptionPage = (hasBizSubscription && startPage.BusinessSubscriptionAdminPage != null) ? 
                _contentRepository.Get<BusinessSubscriptionPage>(startPage.BusinessSubscriptionAdminPage) : null;

            if (subscriber.SelectedSubscription.Type == SubscriptionType.Business && startPage.BusinessSubscriptionAdminPage != null)
            {
                return Redirect(Url.ContentUrl(startPage.BusinessSubscriptionAdminPage) +
                         "SwitchSubscriptionType?subscriptionType=business");
            }            

            return View(model);
        }

        [AuthorizeUser]
        public async Task<ActionResult> ShowInvoice(MyStartPage currentPage, string customerNumber, string invoiceGuid)
        {
            try
            {
                byte[] pdfBuffer;

                if (currentPage.BnApiGetInvoices)
                {
                    var result = await _subscriptionApi.Invoice.GetInvoicePdfByteArrayAsync("di", long.Parse(customerNumber), invoiceGuid);
                    pdfBuffer = result.Data;
                }
                else
                {
                    pdfBuffer = await _invoiceFacade.GetArchivedInvoiceAsPdfBufferAsync(customerNumber, invoiceGuid);
                }

                var contentDisposition = new ContentDisposition
                {
                    FileName = "Faktura-" + invoiceGuid + ".pdf",
                    Inline = true //Show in browser
                };
                Response.AppendHeader("Content-Disposition", contentDisposition.ToString());
                return File(pdfBuffer, "application/pdf");
            }
            catch (Exception exception)
            {
                _logger.Log(exception, string.Format("Failed to show invoice as pdf for customernumber: {0}, invoiceGid: {1}", customerNumber, invoiceGuid), LogLevel.Error, typeof(MyStartPageController));

                var model = new MySettingsViewModel(currentPage) {IsLoggedIn = true};
                return View("PdfNotFound", model);
            }
        }

        public ActionResult Logout(MyStartPage currentPage)
        {
            _sessionData.Set(SessionConstants.SubscriberSessionKey, null); 

            var callbackUrl = _urlHelper.GetContentUrlWithHost(currentPage.ContentLink);

            return Redirect(_redirectHandler.GetLogoutUrl(callbackUrl));
        }

        public ActionResult LogIn(MyStartPage currentPage)
        {
            var callbackUrl = _urlHelper.GetContentUrlWithHost(currentPage.ContentLink) + UrlConstants.AuthenticationCheckedCallBackAction;
       
            return Redirect(_redirectHandler.GetLoginUrl(callbackUrl));
        }

        public async Task<ActionResult> AuthChecked(MyStartPage currentPage, string token, string anchor)
        {
            var subscriber = await _subscriberFacade.GetSubscriber(token);
            _sessionData.Set(SessionConstants.SubscriberSessionKey, subscriber);

            TempData["anchor"] = anchor;

            return RedirectToAction("Index");
        }

        /// <summary>
        /// If Subscriber session exist, with no selectedSubscription and no S+ user. Session will be cleared so a check is made on every request 
        /// </summary>
        private void ClearInvalidSession(Subscriber subscriber)
        {
            if (subscriber != null && (subscriber.SelectedSubscription == null && subscriber.ServicePlusUser == null))
            {
                _sessionData.Set(SessionConstants.SubscriberSessionKey, null);     
            }
        }

        private List<TopMenuItem> GetTopMenuItems(ContentArea contentArea)
        {
            var topMenuItems = new List<TopMenuItem>();

            if (contentArea == null || !contentArea.FilteredItems.Any()) return topMenuItems;

            // ReSharper disable once SuspiciousTypeConversion.Global
            topMenuItems.AddRange(
                contentArea.FilteredItems
                .Select(item => item.GetContent())
                .OfType<AnchorBlockData>()
                .Select(block => new TopMenuItem(block.AnchorId, block.MenuName)));

            return topMenuItems;
        }

        private ViewResult GetNotLoggedInView(MyStartPage currentPage, bool isLoggedInSplus, string message = null)
        {
            var model = new MySettingsViewModel(currentPage)
            {
                IsDebug = IsDebug(),
                IsLoggedIn = isLoggedInSplus,
                TopMenuItems = GetTopMenuItems(currentPage.ContentAreaNotLoggedIn)
            };

            if (message != null)
            {
                ViewBag.Message = message;
            }

            return View("NotLoggedIn", model); //TODO: vad göra? /TKM
        }

        public static bool IsDebug()
        {
            #if DEBUG
                return true;
            #else
                return false;
            #endif
        }
    }
}