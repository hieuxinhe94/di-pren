using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Di.Common.Logging;
using Di.Common.Utils;
using Di.Common.Utils.Url;
using Di.ServicePlus.RedirectApi;
using Di.ServicePlus.RedirectApi.Orders;
using Di.Subscription.Logic.Campaign;
using Di.Subscription.Logic.Customer;
using EPiServer;
using EPiServer.Core;
using EPiServer.Web.Mvc;
using Pren.Web.Business.Address;
using Pren.Web.Business.Attributes;
using Pren.Web.Business.Campaign;
using Pren.Web.Business.Configuration;
using Pren.Web.Business.Detection;
using Pren.Web.Business.Helpers;
using Pren.Web.Business.Payment;
using Pren.Web.Business.ServicePlus.Logic;
using Pren.Web.Business.Session;
using Pren.Web.Business.Subscription;
using Pren.Web.Models.Blocks;
using Pren.Web.Models.CustomForms;
using Pren.Web.Models.Pages;
using Pren.Web.Models.ViewModels;

namespace Pren.Web.Controllers
{
    [NoCache]
    public class CampaignPageSplusController : PageControllerBase<CampaignPageSplus>
    {
        #region Constants

        private const string SelectedCampaignQueryStringName = "selectedCamp";
        private const string EmailQueryStringName = "email";       

        private const string AuthenticatedCheckSessionKey = "auth";
        private const string PostedFormSessionKey = "postedform";
        private const string QueryParameterCallBack = "callBack";
        private const string QueryParameterOfferOrigin = "offerOrigin";
        private const string QueryParameterSalesChannel = "salesChannel";
        private const string QueryParameterTargetGroup = "tg";

        public const string ErrMessDenyShortSub = "Du kan dessvärre bara prova på en gång per 3-månadersperiod. Enligt våra uppgifter har du redan haft en provperiod. Du kan alltid teckna dig för något av våra <a href='http://www.di.se/pren/kampanj/prensidorna/'>ordinarie erbjudanden</a>. Har du några frågor är du varmt välkommen att kontakta <a href='http://www.di.se/kund' target='_blank'>kundtjänst</a>.";
        public const string ErrMessDenySameSubType = "Vi ser att du redan har en aktiv pågående prenumeration och därför har inte denna beställning gått igenom. Har du några frågor är du varmt välkommen att kontakta <a href='http://www.di.se/kund' target='_blank'>kundtjänst</a>.";

        #endregion

        #region Fields
       
        private readonly IUrlHelper _urlHelper;
        private readonly ILogger _logService;
        private readonly IPageSpecificSession _pageSpecificSession;
        private readonly IDetectionHandler _detection;
        private readonly IContentRepository _contentRepository;        
        private readonly ISiteSettings _siteSettings;
        private readonly ICustomerHandler _customerHandler;
        private readonly IRedirectHandler _redirectHandler;
        private readonly IServicePlusFacade _servicePlusFacade;
        private readonly ISubscriptionChecker _subscriptionChecker;
        private readonly ICampaignHandler _campaignHandler;
        private readonly IMaskedAddressService _maskedAddressService;

        #endregion

        #region Constructor

        public CampaignPageSplusController(
            IUrlHelper urlHelper,
            ILogger logService, 
            IPageSpecificSession pageSpecificSession,
            IDetectionHandler detection,
            IContentRepository contentRepository,
            ISiteSettings siteSettings,
            ICustomerHandler customerHandler, IRedirectHandler redirectHandler, IServicePlusFacade servicePlusFacade, ISubscriptionChecker subscriptionChecker, ICampaignHandler campaignHandler, IMaskedAddressService maskedAddressService)
        {
            _urlHelper = urlHelper;
            _logService = logService;
            _pageSpecificSession = pageSpecificSession;
            _detection = detection;
            _contentRepository = contentRepository;      
            _siteSettings = siteSettings;
            _customerHandler = customerHandler;
            _redirectHandler = redirectHandler;
            _servicePlusFacade = servicePlusFacade;
            _subscriptionChecker = subscriptionChecker;
            _campaignHandler = campaignHandler;
            _maskedAddressService = maskedAddressService;
        }

        #endregion

        #region actions

        public ActionResult Index(CampaignPage currentPage, string selectedCamp, string code, string tg, string callBack, string offerOrigin, string sc)
        {
            _pageSpecificSession.ClearSession(currentPage, PostedFormSessionKey);

            // If in edit-mode no authentication check should be made so we create the model and return the view
            if (_detection.IsInEditMode())
            {
                return View(CreateCampaignPageViewModel(currentPage, null, selectedCamp));
            }

            // Save callBack query parameter in session for later use
            if (!string.IsNullOrEmpty(callBack))
            {
                _pageSpecificSession.AddQueryParameterToSession(currentPage, QueryParameterCallBack, callBack);
            }

            // Save offerOrigin query parameter in session for later use
            if (!string.IsNullOrEmpty(offerOrigin))
            {
                _pageSpecificSession.AddQueryParameterToSession(currentPage, QueryParameterOfferOrigin, offerOrigin);
            }

            // Save targetgroup query parameter in session for later use
            if (!string.IsNullOrEmpty(tg))
            {
                _pageSpecificSession.AddQueryParameterToSession(currentPage, QueryParameterTargetGroup, tg);
            }

            

            // Save salesChannel query parameter in session for later use
            if (!string.IsNullOrEmpty(sc))
            {
                _pageSpecificSession.AddQueryParameterToSession(currentPage, QueryParameterSalesChannel, sc);
            }

            var authenticatedCheck = _pageSpecificSession.GetFromSession<AuthenticatedCheck>(currentPage, AuthenticatedCheckSessionKey);
            var shouldCheckIfAuthenticated = (authenticatedCheck == null || !string.IsNullOrEmpty(code) || !string.IsNullOrEmpty(tg));

            if (shouldCheckIfAuthenticated)
            {
                var callbackUrl = _urlHelper.GetContentUrlWithHost(currentPage.ContentLink) + UrlConstants.AuthenticationCheckedCallBackAction;
                return Redirect(_redirectHandler.GetCheckedLoginUrl(UrlUtils.AddAllExistingQuerystrings(callbackUrl)));
            }

            var model = CreateCampaignPageViewModel(currentPage, authenticatedCheck, selectedCamp);

            //if no token clear authenticated check session so a check is made on every request 
            if (string.IsNullOrEmpty(authenticatedCheck.Token))
            {
                _pageSpecificSession.ClearSession(currentPage, AuthenticatedCheckSessionKey);
            }

            return View(model);
        }

        public ActionResult AuthChecked(CampaignPage currentPage, string token, string selectedCamp, string code, string tg, string message)
        {
            var authenticatedCheck = new AuthenticatedCheck
            {
                PrePopulateCode = code,
                SelectedCampaign = selectedCamp,
                Token = token,
                TargetGroup = tg,
                BipMessage = message                
            };

            _pageSpecificSession.SetInSession(currentPage, AuthenticatedCheckSessionKey, authenticatedCheck);
            
            return RedirectToAction("Index");
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult PostForm(CampaignPage currentPage, CampaignSubscribeFormModel postedForm)
        {
            try
            {
                _pageSpecificSession.SetInSession(currentPage, PostedFormSessionKey, postedForm);

                var model = new CampaignPageViewModel(currentPage) { SubscriptionForm = postedForm };

                if (!ValidateForm(postedForm)) return GetErrorView(model, CampaignConstants.GeneralErrorMessage);

                var isTrial = postedForm.IsTrial || postedForm.IsTrialFree;
                var payMethod = GetPaymentMethod(postedForm);
                var priceGroup = GetPiceGroup(postedForm, payMethod);

                // Check if user had trial subscription within the last 3 months
                if (isTrial && _subscriptionChecker.DenySubscriptionForPriceGroup(priceGroup, postedForm.Email, 3))
                {
                    _logService.Log("Trial denied for pricegroup " + _siteSettings.PriceGroupTrial + " - " + postedForm.ToLogString(), LogLevel.Info, typeof(CampaignPageSplusController));
                    return GetErrorView(model, ErrMessDenyShortSub);
                }                    

                var campaign = postedForm.CampNo < 1
                    ? _campaignHandler.GetCampaign(postedForm.CampId)
                    : _campaignHandler.GetCampaign(postedForm.CampNo);

                // Check if user have an active subscription of same type
                if (_subscriptionChecker.DenySubscriptionOfSameType(campaign.PackageId, postedForm.Email))
                {
                    _logService.Log("Same type denied - " + postedForm.ToLogString(), LogLevel.Info, typeof(CampaignPageSplusController));
                    return GetErrorView(model, ErrMessDenySameSubType);   
                }

                var originalAddressInfo = _maskedAddressService.DecryptAddressResult(postedForm.OriginalInfo);
                var originalAddressInfoInvoice = postedForm.InvoiceOtherPayer ? _maskedAddressService.DecryptAddressResult(postedForm.OriginalInfoInvoice) : null;

                UpdatePostedFormWithUnmaskedValues(postedForm, originalAddressInfo, originalAddressInfoInvoice);
                
                var servicePlusRedirectUrl = GetOrderUrl(currentPage, postedForm, campaign);
                
                return Redirect(servicePlusRedirectUrl);    
            }
            catch (Exception ex)
            {
                _logService.Log(ex, "Campaign.PostForm - failed", LogLevel.Error, typeof(CampaignPageSplusController));
                return GetErrorView(new CampaignPageViewModel(currentPage), CampaignConstants.GeneralErrorMessage);
            }
        }

        public ActionResult OrderProcessed(CampaignPage currentPage)
        {
            try
            {
                // TODO: lägg i facade och returnera b-objekt, inte orderResponse
                var orderResponse = _redirectHandler.Orders.GetResponse(Request);
                _pageSpecificSession.SetInSession(currentPage, "orderResponse", orderResponse);


                var postedForm = _pageSpecificSession.GetFromSession<CampaignSubscribeFormModel>(currentPage, PostedFormSessionKey);
                if (postedForm == null)
                {
                    throw new NullReferenceException("postedForm session is dead");
                }

                var selectedCampaign = GetSelectedCampaignBlock(postedForm);

                var model = new CampaignPageViewModel(currentPage) { SubscriptionForm = postedForm, SelectecedCampaign = selectedCampaign };

                if (orderResponse == null || !string.IsNullOrEmpty(orderResponse.Error))
                {
                    _logService.Log("Campaign.ThankYou - orderresponse error - " + (orderResponse != null ? orderResponse.Error : "orderResponse null"), LogLevel.Error, typeof(CampaignPageSplusController));
                    return GetErrorView(model, CampaignConstants.GeneralErrorMessage);
                }

                if (orderResponse.Result == "payment-cancelled")
                {
                    return RedirectToAction("index");
                }

                if (postedForm.IsPayWall && !string.IsNullOrEmpty(orderResponse.Token))
                {
                    // Get offerOrigin query parameter from session, if provided it is saved in session in the Index action
                    var offerOrigin = _pageSpecificSession.GetQueryParameterFromSession(currentPage, QueryParameterOfferOrigin);
                    AddServicePlusOffer(orderResponse.Token, offerOrigin);
                    _pageSpecificSession.ClearSession(currentPage, QueryParameterOfferOrigin);
                }

                return RedirectToAction("ThankYou");
            }
            catch (Exception exception)
            {
                _logService.Log(exception, "OrderProcessed - failed", LogLevel.Error, typeof(CampaignPageSplusController));
                return GetErrorView(new CampaignPageViewModel(currentPage), CampaignConstants.GeneralErrorMessage);                
            }
            
        }

        public ActionResult ThankYou(CampaignPage currentPage)
        {
            // TODO: handling if session is dead
            var orderResponse = _pageSpecificSession.GetFromSession<OrderResponse>(currentPage, "orderResponse");            
            var postedForm = _pageSpecificSession.GetFromSession<CampaignSubscribeFormModel>(currentPage, PostedFormSessionKey);
            var selectedCampaign = GetSelectedCampaignBlock(postedForm);

            var model = new CampaignPageViewModel(currentPage) { SubscriptionForm = postedForm, SelectecedCampaign = selectedCampaign };

            AddTrackingInfo(postedForm, orderResponse);

            // Get callback query parameter from session, if provided it is saved in session in the Index action
            var callBackQueryParameter = _pageSpecificSession.GetQueryParameterFromSession(currentPage, QueryParameterCallBack);

            // If we have a callback parameter in session, set it in the viewbag so we can use it in the view.
            if (!string.IsNullOrEmpty(callBackQueryParameter))
            {
                ViewBag.CallBackUrl = callBackQueryParameter;
            }

            _pageSpecificSession.ClearSession(currentPage, AuthenticatedCheckSessionKey);
            
            return View("ThankYou", model);
        }

        public ActionResult ThankYouVerify(CampaignPage currentPage, string campId)
        {

            var model = new CampaignPageViewModel(currentPage)
            {
                SubscriptionForm = new CampaignSubscribeFormModel
                {
                    CampId = campId,
                    Email = "testatacksida@di.se"
                },
                SelectecedCampaign = new CampaignBlock
                {
                    Heading = "Test av tacksida med campId: " + campId,
                    
                }
            };

            return View("ThankYou", model);
        }

        public ActionResult LogOut(CampaignPage currentPage, string selectedCamp)
        {
            _pageSpecificSession.SetInSession(currentPage, AuthenticatedCheckSessionKey, null);

            var callbackUrl = UrlUtils.AddQueryString(_urlHelper.GetContentUrlWithHost(currentPage.ContentLink), SelectedCampaignQueryStringName, selectedCamp);

            Response.Redirect(_redirectHandler.GetLogoutUrl(callbackUrl));

            var model = new CampaignPageViewModel(currentPage);
            return View("index", model);
        }

        public ActionResult LogIn(CampaignPage currentPage, string selectedCamp, string code, string email)
        {
            _pageSpecificSession.SetInSession(currentPage, AuthenticatedCheckSessionKey, null);

            var callbackUrl = UrlUtils.AddQueryString(_urlHelper.GetContentUrlWithHost(currentPage.ContentLink), SelectedCampaignQueryStringName, selectedCamp);
            callbackUrl = UrlUtils.AddQueryString(callbackUrl, UrlConstants.CodeQueryStringName, code);

            var loginUrl = _redirectHandler.GetLoginUrl(callbackUrl);
            // Add email to login page to prepopulate email address in S+ login form
            loginUrl = UrlUtils.AddQueryString(loginUrl, EmailQueryStringName, email);

            Response.Redirect(loginUrl);

            var model = new CampaignPageViewModel(currentPage);
            return View("index", model);
        }

        #endregion

        #region Private methods

        private void UpdatePostedFormWithUnmaskedValues(CampaignSubscribeFormModel postedForm, AddressResult originalAddressInfo, AddressResult originalAddressInfoInvoice)
        {
            var replaceWithOrg = new Func<string, string, string>(
                (postedFormValue, originalValue) =>
                    postedFormValue != null && postedFormValue.Contains("*") ?
                        originalValue :
                        postedFormValue);

            if (originalAddressInfo != null)
            {
                postedForm.FirstName = replaceWithOrg(postedForm.FirstName, originalAddressInfo.FirstNames);
                postedForm.LastName = replaceWithOrg(postedForm.LastName, originalAddressInfo.LastNames);
                postedForm.StreetAddress = replaceWithOrg(postedForm.StreetAddress, originalAddressInfo.StreetAddress);
                postedForm.StreetNo = replaceWithOrg(postedForm.StreetNo, originalAddressInfo.HouseNumber);
                postedForm.StairCase = replaceWithOrg(postedForm.StairCase, originalAddressInfo.StairCase);
                postedForm.Zip = replaceWithOrg(postedForm.Zip, originalAddressInfo.ZipCode);
                postedForm.City = replaceWithOrg(postedForm.City, originalAddressInfo.City);
                postedForm.Company = replaceWithOrg(postedForm.Company, originalAddressInfo.Name);  
            }                   
            
            //If search on personal number and otherpayer, addresses will be obfuscated
            if (postedForm.InvoiceOtherPayer && originalAddressInfoInvoice != null)
            {
                postedForm.StreetAddressInvoice = replaceWithOrg(postedForm.StreetAddressInvoice, originalAddressInfoInvoice.StreetAddress);
                postedForm.StreetNoInvoice = replaceWithOrg(postedForm.StreetNoInvoice, originalAddressInfoInvoice.HouseNumber);
                postedForm.ZipInvoice = replaceWithOrg(postedForm.ZipInvoice, originalAddressInfoInvoice.ZipCode);
                postedForm.City = replaceWithOrg(postedForm.CityInvoice, originalAddressInfoInvoice.City);
            }
        }

        private void AddServicePlusOffer(string token, string offerOrigin)
        {
            var updated = _servicePlusFacade.CreateOrUpdateOffer(token, offerOrigin);

            if (!updated)
            {
                _logService.Log("AddServicePlusOffer - failed for token - See webrequest log for details, token: " + token, LogLevel.Error, typeof(CampaignPageSplusController));
            }
        }

        private void AddTrackingInfo(CampaignSubscribeFormModel postedForm, OrderResponse orderResponse)
        {
            if (postedForm != null && orderResponse != null)
            {
                decimal amount;
                if (decimal.TryParse(orderResponse.Price, out amount))
                {
                    //since amount is in ören we need to divide with 100
                    amount = amount/100;
                }

                ViewBag.LoggedIn = postedForm.IsServicePlusUser;
                ViewBag.ProductName = orderResponse.ProductDesc;
                ViewBag.TransactionTotal = amount; 
                ViewBag.ProductPrice = amount; 
                ViewBag.PaymentMethod = orderResponse.PayMethod; 
                ViewBag.TargetGroup = orderResponse.TargetGroup;
            }
            
            // GA-Tracking needs a unique transaction Id so we generate one
            var timeStamp = DateTimeUtils.GetUnixTimeStampInMilliseconds(DateTime.Now);
            var random = new Random().Next(0, 100);

            ViewBag.TransactionId = timeStamp + random;
        }

        private CampaignPageViewModel CreateCampaignPageViewModel(CampaignPage currentPage, AuthenticatedCheck authenticatedCheck, string selectedCamp)
        {
            var model = new CampaignPageViewModel(currentPage)
            {
                IsMobileDevice = _detection.IsMobileDevice(),
                SubscriptionForm =
                {
                    TargetGroup = GetTargetGroup(currentPage, authenticatedCheck)
                },
                LoginUrl = UrlUtils.AddQueryString(_urlHelper.GetContentUrlWithHost(currentPage.ContentLink) + UrlConstants.LoginAction, SelectedCampaignQueryStringName, selectedCamp),
                LogOutUrl = UrlUtils.AddQueryString(_urlHelper.GetContentUrlWithHost(currentPage.ContentLink) + UrlConstants.LogoutAction, SelectedCampaignQueryStringName, selectedCamp)
            };

            if (authenticatedCheck != null)
            {
                model.BipMessage = authenticatedCheck.BipMessage;

                PopulateModelWithPrenData(authenticatedCheck.PrePopulateCode, model.SubscriptionForm);

                PopulateModelWithServicePlusData(authenticatedCheck.Token, model);

                if (!string.IsNullOrEmpty(authenticatedCheck.SelectedCampaign))
                {
                    model.SubscriptionForm.CampId = authenticatedCheck.SelectedCampaign;
                }

                if (!string.IsNullOrEmpty(authenticatedCheck.PrePopulateCode))
                {
                    model.LoginUrl = UrlUtils.AddQueryString(model.LoginUrl, UrlConstants.CodeQueryStringName, authenticatedCheck.PrePopulateCode);
                }
            }

            return model;
        }

        private string GetPaymentMethod(CampaignSubscribeFormModel postedForm)
        {
            switch (postedForm.PaymentMethod)
            {
                case PaymentConstants.Invoice:
                    return PayMethod.INVOICE.ToString();
                case PaymentConstants.Card:
                    return PayMethod.CREDIT_CARD.ToString();
                case PaymentConstants.Autowithdrawal:
                    return PayMethod.CREDIT_CARD.ToString();
                case PaymentConstants.Autogiro:
                    return PayMethod.AUTOGIRO.ToString();
                default:
                    throw new NotImplementedException("Not supported");
            }
        }

        private string GetPiceGroup(CampaignSubscribeFormModel postedForm, string payMethod)
        {
            if (postedForm.IsTrial)
                return _siteSettings.PriceGroupTrial;

            if (postedForm.IsTrialFree)
                return _siteSettings.PriceGroupTrialFree;

            return payMethod == PayMethod.AUTOGIRO.ToString() ? _siteSettings.PriceGroupDirectDebit : _siteSettings.PriceGroupRegular;
        }

        private bool ValidateForm(CampaignSubscribeFormModel postedForm)
        {
            if (postedForm == null)
            {
                return false;
            }
            if (string.IsNullOrEmpty(postedForm.Email))
            {
                return false;
            }
            if (string.IsNullOrEmpty(postedForm.CampId))
            {
                return false;
            }
            if (string.IsNullOrEmpty(postedForm.PaymentMethod))
            {
                return false;
            }

            return true;
        }

        private string GetTargetGroup(CampaignPage currentPage, AuthenticatedCheck authenticatedCheck)
        {
            var tgFromSession = _pageSpecificSession.GetQueryParameterFromSession(currentPage, QueryParameterTargetGroup);

            if (!string.IsNullOrEmpty(tgFromSession))
            {
                return tgFromSession;
            }

            if (authenticatedCheck != null && !string.IsNullOrEmpty(authenticatedCheck.TargetGroup))
                return authenticatedCheck.TargetGroup;

            return _detection.IsMobileDevice() ? currentPage.TargetGroupMobile : currentPage.TargetGroup;
        }

        private void PopulateModelWithPrenData(string code, CampaignSubscribeFormModel model)
        {
            long eCusno;

            if (string.IsNullOrEmpty(code) || !long.TryParse(code, out eCusno))
            {
                return;
            }

            try
            {
                var cusNo = _customerHandler.GetCustomerNumberByEcusno(eCusno);

                if (cusNo <= 0) return;

                var customer = _customerHandler.GetCustomer(cusNo); 

                model.Company = customer.CompanyName; 
                model.FirstName = customer.FirstName;
                model.LastName = customer.LastName;
                model.Phone = customer.PhoneOffice.Replace("+46", "0"); 
                model.Email = customer.Email;
                model.StreetAddress = customer.AddressStreetName; 
                model.StreetNo = customer.AddressStreetNumber; 
                model.StairCase = customer.AddressStairCase; 
                model.Stairs = customer.AddressStairs; 
                model.Co = customer.AddressCareOf;
                model.Zip = customer.AddressZip; 
                model.City = customer.AddressCity; 
            }
            catch (Exception ex)
            {
                _logService.Log(ex, "CampaignForm.PopulateModelWithPrenData() for url code '" + code + "' failed", LogLevel.Error, typeof(CampaignPageSplusController));
            }

        }

        private void PopulateModelWithServicePlusData(string token, CampaignPageViewModel model)
        {
            if (string.IsNullOrEmpty(token))
            {
                return;
            }

            var user = _servicePlusFacade.GetUserByToken(token);

            if (user == null)
            {
                return;
            }

            if (!string.IsNullOrEmpty(user.Email))
            {
                model.SubscriptionForm.Email = user.Email;
            }
            if (!string.IsNullOrEmpty(user.Phone))
            {
                model.SubscriptionForm.Phone = user.Phone.Replace("+46", "0");
            }

            model.SubscriptionForm.IsServicePlusUser = true;
        }

        private CampaignBlock GetSelectedCampaignBlock(CampaignSubscribeFormModel postedForm)
        {
            return (postedForm != null && postedForm.CampaignContentId > 0)
                ? _contentRepository.Get<CampaignBlock>(new ContentReference(postedForm.CampaignContentId))
                : null;
        }

        private CampaignPeriodBlock GetSelectedPeriodBlock(CampaignBlock campaignBlock, string campiId)
        {
            var isSelectedPeriod = new Func<CampaignPeriodBlock, string, bool>(
                (periodBlock, campId) => 
                    (periodBlock.CampaignCardAndInvoice == campId || 
                        (periodBlock.CampaignCardAndInvoice != null && periodBlock.CampaignCardAndInvoice.StartsWith(campId + "|"))) ||
                    (periodBlock.CampaignAutogiro == campId || 
                        (periodBlock.CampaignAutogiro != null && periodBlock.CampaignAutogiro.StartsWith(campId + "|"))) 
                );

            var campaignPeriods = new List<CampaignPeriodBlock>
            {
                campaignBlock.FirstCampaignPeriod,
                campaignBlock.SecondCampaignPeriod,
                campaignBlock.ThirdCampaignPeriod
            };

            return campaignPeriods.FirstOrDefault(t => isSelectedPeriod(t, campiId));
        }

        private ViewResult GetErrorView(CampaignPageViewModel model, string errorMessage)
        {
            model.ErrorMessage = errorMessage;
            return View("Error", model);  
        }

        private string GetOrderUrl(CampaignPage currentPage, CampaignSubscribeFormModel postedForm, Di.Subscription.Logic.Campaign.Types.Campaign campaign)
        {
            var paymethod = GetPaymentMethod(postedForm);
            var callbackUrl = _urlHelper.GetContentUrlWithHost(currentPage.ContentLink) + "OrderProcessed";
            var selectedCampaign = GetSelectedCampaignBlock(postedForm);
            var priceGroup = GetPiceGroup(postedForm, paymethod);
            var streetNumber = postedForm.StreetNo + " " + postedForm.StairCase;

            var isOtherPayer = postedForm.InvoiceOtherPayer && paymethod == PayMethod.INVOICE.ToString();

            var payerOrgNo = isOtherPayer ? postedForm.SsnInvoice : string.Empty;
            var payerCompany = isOtherPayer ? postedForm.CompanyInvoice : string.Empty;
            var payerAttention = isOtherPayer ? postedForm.AttentionInvoice : string.Empty;
            var payerPhone = isOtherPayer ? postedForm.PhoneInvoice : string.Empty;
            var payerStreetName = isOtherPayer ? postedForm.StreetAddressInvoice : string.Empty; 
            var payerStreetNumber = isOtherPayer ? postedForm.StreetNoInvoice : string.Empty;
            var payerZip = isOtherPayer ? postedForm.ZipInvoice : string.Empty;
            var payerCity = isOtherPayer ? postedForm.CityInvoice : string.Empty;

            var startPage = _contentRepository.Get<StartPage>(ContentReference.StartPage);

            var priceInOre = startPage.UsePartPayment ? (campaign.PriceForCustomerToPay * 100).ToString(CultureInfo.InvariantCulture) : (campaign.TotalPriceIncludningVat * 100).ToString(CultureInfo.InvariantCulture);

            var salesChannelFromSession = _pageSpecificSession.GetQueryParameterFromSession(currentPage, QueryParameterSalesChannel);
            var salesChannel = startPage.SendSalesChannelParameter
                ? (string.IsNullOrEmpty(salesChannelFromSession) 
                    ? currentPage.SalesChannel 
                    : salesChannelFromSession)
                : null;

            var subsKind = campaign.SubsKind;

            if (startPage.SendSubsKindParameter)
            {
                var selectedPeriod = GetSelectedPeriodBlock(selectedCampaign, postedForm.CampId);

                if (selectedPeriod?.SubsKind != null && selectedPeriod.SubsKind != "0")
                {
                    subsKind = selectedPeriod.SubsKind;
                }
            }

            return _redirectHandler.Orders.GetUrl(_siteSettings.ServicePlusAppId,
                _siteSettings.ServicePlusOrderProductId,
                _siteSettings.ServicePlusSecretKey,
                postedForm.Email, postedForm.FirstName, postedForm.LastName, postedForm.Co, postedForm.Phone,
                postedForm.StreetAddress, streetNumber, postedForm.Zip, postedForm.City, postedForm.Company, postedForm.Ssn,
                payerOrgNo, payerCompany, payerAttention, payerPhone, payerStreetName, payerStreetNumber, payerZip, payerCity,
                paymethod, postedForm.TargetGroup, campaign.CampaignNumber.ToString(CultureInfo.InvariantCulture),
                priceGroup, priceInOre,
                selectedCampaign.Heading, callbackUrl, !_siteSettings.IsDigitalSub(campaign.PackageId), subsKind, salesChannel);
        }

        #endregion
    }

    // ReSharper disable InconsistentNaming
    public enum PayMethod
    {
        INVOICE,
        CREDIT_CARD,
        AUTOGIRO
    }

    public class Test
    {
        public const string PaymentCancelled = "payment-cancelled";
    }
}