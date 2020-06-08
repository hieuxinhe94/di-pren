using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Di.Common.Logging;
using Di.Common.Utils.Url;
using Di.ServicePlus.RedirectApi;
using Di.Subscription.Logic.Campaign;
using DIClassLib.BonnierDigital;
using EPiServer.Web.Mvc;
using Pren.Web.Business.Attributes;
using Pren.Web.Business.BusinessSubscription;
using Pren.Web.Business.Configuration;
using Pren.Web.Business.DataAccess;
using Pren.Web.Business.Detection;
using Pren.Web.Business.Helpers;
using Pren.Web.Business.Mail;
using Pren.Web.Business.ServicePlus;
using Pren.Web.Business.ServicePlus.Logic;
using Pren.Web.Business.Session;
using Pren.Web.Business.Utils.Replace;
using Pren.Web.Models.Blocks;
using Pren.Web.Models.CustomForms;
using Pren.Web.Models.Pages;
using Pren.Web.Models.ViewModels;

namespace Pren.Web.Controllers
{
    [NoCache]
    public class BusinessCampaignPageController : PageController<BusinessCampaignPage>
    {
        private const string AuthenticatedCheckSessionKey = "auth";
        private const string PostedFormSessionKey = "postedform";

        private const string AuthenticationCheckedCallBackAction = "authchecked";

        private const string EmailQueryStringName = "email";

        private const string LogInAction = "login";
        private const string LogOutAction = "logout";

        private readonly IDetectionHandler _detectionHandler;
        private readonly IPageSpecificSession _pageSpecificSession;
        private readonly IUrlHelper _urlHelper;

        private readonly IRedirectHandler _redirectHandler;
        private readonly IServicePlusFacade _servicePlusFacade;

        private readonly ILogger _logService;

        private readonly IServicePlusHandler<UserOutput> _servicePlusHandler;

        private readonly IPriceHandler _priceHandler;

        private readonly ISessionData _sessionData;

        private readonly IDataAccess _dataAccess;

        private readonly ICampaignHandler _campaignHandler;

        private readonly IMailHandler _mailHandler;

        public BusinessCampaignPageController(
            IServicePlusHandler<UserOutput> servicePlusHandler, 
            IDetectionHandler detectionHandler,
            IPageSpecificSession pageSpecificSession,
            IUrlHelper urlHelper,
            IRedirectHandler redirectHandler,
            IServicePlusFacade servicePlusFacade,
            ILogger logService,
            IPriceHandler priceHandler, 
            ISessionData sessionData, IDataAccess dataAccess, 
            ICampaignHandler campaignHandler, 
            IMailHandler mailHandler)
        {
            _detectionHandler = detectionHandler;
            _pageSpecificSession = pageSpecificSession;
            _urlHelper = urlHelper;
            _redirectHandler = redirectHandler;
            _servicePlusFacade = servicePlusFacade;
            _logService = logService;

            _servicePlusHandler = servicePlusHandler;
            _priceHandler = priceHandler;
            _sessionData = sessionData;
            _dataAccess = dataAccess;
            _campaignHandler = campaignHandler;
            _mailHandler = mailHandler;
        }

        public ActionResult Index(BusinessCampaignPage currentPage)
        {


            // If in edit-mode no authentication check should be made so we create the model and return the view
            if (_detectionHandler.IsInEditMode())
            {
                return View(CreateCompanyCampaignPageViewModel(currentPage));
            }

            var authenticatedCheck = _pageSpecificSession.GetFromSession<AuthenticatedCheck>(currentPage, AuthenticatedCheckSessionKey);

            var shouldCheckIfAuthenticated = (authenticatedCheck == null);

            if (shouldCheckIfAuthenticated)
            {
                var callbackUrl = _urlHelper.GetContentUrlWithHost(currentPage.ContentLink) + AuthenticationCheckedCallBackAction;
                callbackUrl = UrlUtils.AddAllExistingQuerystrings(callbackUrl);

                return Redirect(_redirectHandler.GetCheckedLoginUrl(callbackUrl));
            }

            // Create view model
            var model = CreateCompanyCampaignPageViewModel(currentPage);

            // Add logged in data to view model
            if (!string.IsNullOrEmpty(authenticatedCheck.Token))
            {
                var user = _servicePlusFacade.GetUserByToken(authenticatedCheck.Token);

                if (user != null)
                {
                    model.SubscriptionForm.Email = user.Email;
                    model.SubscriptionForm.ServicePlusUserId = user.Id;
                }
            }

            //if no token clear authenticated check session so a check is made on every request 
            if (string.IsNullOrEmpty(authenticatedCheck.Token))
            {
                _pageSpecificSession.ClearSession(currentPage, AuthenticatedCheckSessionKey);
            }
            
            return View(model);
        }

        public ActionResult AuthChecked(BusinessCampaignPage currentPage, string token)
        {
            var authenticatedCheck = new AuthenticatedCheck
            {
                Token = token,
            };

            _pageSpecificSession.SetInSession(currentPage, AuthenticatedCheckSessionKey, authenticatedCheck);

            return RedirectToAction("Index");
        }

        /// <summary>
        /// This action is called when the form is posted.
        /// The posted form is saved in session.
        /// If S+ account exists a redirection is made to CreateBizSubscription
        /// If S+ account does not exist a redirection is made to CreateServicePlusAccount
        /// </summary>
        public ActionResult PostForm(BusinessCampaignPage currentPage, BusinessCampaignSubscribeFormModel postedForm)
        {
            _pageSpecificSession.SetInSession(currentPage, PostedFormSessionKey, postedForm);

            var authenticatedCheck = _pageSpecificSession.GetFromSession<AuthenticatedCheck>(currentPage, AuthenticatedCheckSessionKey);

            if (authenticatedCheck != null && !string.IsNullOrEmpty(authenticatedCheck.Token))
            {
                var user = _servicePlusFacade.GetUserByToken(authenticatedCheck.Token);

                if (user != null)
                {
                    return RedirectToAction("CreateBizSubscription");
                }
            }

            return RedirectToAction("CreateServicePlusAccount");
        }

        [ValidateAntiForgeryToken]
        public ActionResult PostContactForm(BusinessCampaignPage currentPage, string phone, string email, string companyName, string title, string message)
        {
            var model = new BusinessCampaignPageViewModel(currentPage);

            try
            {
                var mailTo = currentPage.LicenceContactMail;
                var body = "<strong>Telefonnummer</strong>: " + phone +
                           "<br><strong>E-post</strong>: " + email +
                           "<br><strong>Företagsnamn</strong>: " + companyName +
                           "<br><strong>Titel</strong>: " + title +
                           "<br><strong>Meddelande</strong>: " + message;

                _mailHandler.SendMail(email, mailTo, "Företagsportalen licensfråga", body, true);
            }
            catch (Exception exception)
            {
                _logService.Log(exception, "PostContactForm - send mail error", LogLevel.Error, typeof(BusinessCampaignPageController));
                model.ErrorMessage = "Ett tekniskt fel uppstod. Vänligen kontakta kundtjänst";
                return View("Error", model);
            }

            return View("MailConfirm", model);
        }

        /// <summary>
        /// When this action is called we know that no S+ account exists so we create one
        /// </summary>
        public ActionResult CreateServicePlusAccount(BusinessCampaignPage currentPage)
        {
            // Get form from session
            var postedForm = _pageSpecificSession.GetFromSession<BusinessCampaignSubscribeFormModel>(currentPage, PostedFormSessionKey);

            var url = _servicePlusHandler.GetAutoRegisterUserUrlForBizSubscription(
                postedForm.Email,
                postedForm.FirstName,
                postedForm.LastName,
                postedForm.Phone,
                _urlHelper.GetContentUrlWithHost(currentPage.ContentLink)) + "CreateBizSubscription";

            return Redirect(url);
        }

        /// <summary>
        /// When this action is called we know that a S+ account exists
        /// </summary>
        public ActionResult CreateBizSubscription(BusinessCampaignPage currentPage)
        {
            // Get form from session
            var postedForm = _pageSpecificSession.GetFromSession<BusinessCampaignSubscribeFormModel>(currentPage, PostedFormSessionKey);

            var model = new BusinessCampaignPageViewModel(currentPage) { SubscriptionForm = postedForm };

            var successfullyCreatedSubscription = CreateBizSubscription(postedForm);

            if (successfullyCreatedSubscription)
            {
                var adminPageUrl = UrlUtils.AddQueryString(_urlHelper.GetContentUrlWithHost(currentPage.SubscriptionAdminPage), "reciept", "true");

                // Clear session so that a new check is made on my page
                _sessionData.Set(SessionConstants.SubscriberSessionKey, null);
                return Redirect(adminPageUrl);
            }

            // Something went wrong - set errormessage on model and return the error view
            model.ErrorMessage = "Ett fel uppstod....";
            return View("Error", model);
        }


        public ActionResult LogOut(BusinessCampaignPage currentPage, string selectedCamp)
        {
            _pageSpecificSession.SetInSession(currentPage, AuthenticatedCheckSessionKey, null);

            //todo: kj hur kan vi lösa det snyggare?
            // Clear session that is used on my page because if we have a session with another user on my page that one will be used.
            _sessionData.Set(SessionConstants.SubscriberSessionKey, null);

            var callbackUrl = _urlHelper.GetContentUrlWithHost(currentPage.ContentLink);

            return Redirect(_redirectHandler.GetLogoutUrl(callbackUrl));
        }

        public ActionResult LogIn(BusinessCampaignPage currentPage, string selectedCamp, string code, string email)
        {
            _pageSpecificSession.SetInSession(currentPage, AuthenticatedCheckSessionKey, null);

            var callbackUrl = _urlHelper.GetContentUrlWithHost(currentPage.ContentLink);

            var loginUrl = _redirectHandler.GetLoginUrl(callbackUrl);
            loginUrl = UrlUtils.AddQueryString(loginUrl, EmailQueryStringName, email);

            return Redirect(loginUrl);
        }

        private BusinessCampaignPageViewModel CreateCompanyCampaignPageViewModel(BusinessCampaignPage currentPage)
        {
            var viewModel = new BusinessCampaignPageViewModel(currentPage)
            {
                LoginUrl = _urlHelper.GetContentUrlWithHost(currentPage.ContentLink) + LogInAction,
                LogOutUrl = _urlHelper.GetContentUrlWithHost(currentPage.ContentLink) + LogOutAction,
                BusinessOffers = GetBusinessOffers(currentPage.KayakCampaignNumber, currentPage.ServicePlusBizDefinitionProductId) 
            };

            try
            {
                ViewData.GetEditHints<CampaignBlockViewModel, CampaignBlock>().AddFullRefreshFor(block => block.UspProduct);

                if (currentPage.UspProduct > 0)
                {
                    var uspTexts = _dataAccess.UspHandler.GetUspTexts(currentPage.UspProduct).ToList();
                    if (uspTexts.Any())
                    {
                        viewModel.UspTexts = uspTexts.Select(uspText => ReplaceUtil.ReplacePlaceholderWithImage(uspText.Text)).ToList();
                    }
                }
            }
            catch (Exception exception)
            {
                // TODO Add logging when implemented /TKM
                //_dataAccess.Logger.Log(exception, "GetUspTexts failed", LogLevel.Error,  null);
            }

            try
            {
                var campign = _campaignHandler.GetCampaign(currentPage.KayakCampaignNumber);
                if (campign != null)
                {
                    viewModel.CampaignId = campign.CampaignId;
                }
            }
            catch (Exception exception)
            {
                _logService.Log(exception, "CompanyPortalCampaignPageController - Could not get camapign with campignNumber " + currentPage.KayakCampaignNumber, LogLevel.Error, typeof(BusinessCampaignPageController));
            }

            return viewModel;
        }

        private bool CreateBizSubscription(BusinessCampaignSubscribeFormModel postedForm)
        {
            try
            {
                return _servicePlusFacade.CreateBizSubscription(
                    postedForm.Company,
                    postedForm.BizSubscriptionDefinitionId,
                    postedForm.CompanyRegistrationNumber,
                    postedForm.Email,
                    postedForm.FirstName,
                    postedForm.LastName,
                    postedForm.StreetAddress,
                    postedForm.StreetNo,
                    postedForm.Zip,
                    postedForm.City,
                    postedForm.CampaignNumber,
                    postedForm.Phone);
            }
            catch (Exception exception)
            {
                _logService.Log(exception, "Could not create biz subscription", LogLevel.Error, typeof(BusinessCampaignPageController));
                return false;
            }
        }

        private IEnumerable<BusinessOffer> GetBusinessOffers(int campaignNumber, string servicePlusBizDefinitionProductId)
        {
            try
            {
                var bizSubscriptionDefinitions = _servicePlusFacade.GetBizSubscriptionDefinitionsByProductId(servicePlusBizDefinitionProductId)
                    .OrderBy(definition => definition.MinQuantity);

                return bizSubscriptionDefinitions.Select(bizSubscriptionDefinition => new BusinessOffer
                {
                    Id = bizSubscriptionDefinition.Id,
                    MinAccounts = bizSubscriptionDefinition.MinQuantity,
                    MaxAccounts = bizSubscriptionDefinition.MaxQuantity,
                    PricePerAccount = _priceHandler.GetPrice(bizSubscriptionDefinition.ExternalProductCode),
                    CampaignNumber = campaignNumber.ToString(CultureInfo.InvariantCulture)
                }).ToList();
            }
            catch (Exception exception)
            {
                _logService.Log(exception, "GetBusinessOffers failed", LogLevel.Error, typeof(BusinessCampaignPageController));
                return new List<BusinessOffer>();
            }
        }
    }
}