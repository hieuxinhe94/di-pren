using System;
using System.Text;
using System.Web.Mvc;
using Di.Common.Logging;
using Di.Common.Utils.Url;
using Di.ServicePlus.RedirectApi;
using DIClassLib.BonnierDigital;
using DIClassLib.CardPayment.Nets;
using DIClassLib.DbHandlers;
using DIClassLib.Misc;
using EPiServer;
using EPiServer.Core;
using EPiServer.Web.Mvc;
using Pren.Web.Business.Attributes;
using Pren.Web.Business.Campaign;
using Pren.Web.Business.Detection;
using Pren.Web.Business.Helpers;
using Pren.Web.Business.Payment;
using Pren.Web.Business.ServicePlus;
using Pren.Web.Business.ServicePlus.Logic;
using Pren.Web.Business.Session;
using Pren.Web.Business.Subscription;
using Pren.Web.Models.Blocks;
using Pren.Web.Models.CustomForms;
using Pren.Web.Models.Pages;
using Pren.Web.Models.ViewModels;
using DIClassLib.Subscriptions;
using Settings = DIClassLib.Misc.Settings;

namespace Pren.Web.Controllers
{
    [NoCache]
    public class CampaignPageController : PageControllerBase<CampaignPage>
    {
        #region Constants

        private const string SelectedCampaignQueryStringName = "selectedCamp";
        private const string PrePopulateCodeQueryStringName = "code";
        private const string EmailQueryStringName = "email";       

        private const string AuthenticationCheckedCallBackAction = "authchecked";
        private const string ServiceplusReturnAction = "serviceplusreturn";
        private const string CardPaymentReturnAction = "cardreturn";
        private const string CardFailedReturnAction = "cardfailed";
        private const string CardPaymentCancelResponseCode = "cancel";

        private const string LogInAction = "login";
        private const string LogOutAction = "logout";

        private const string AuthenticatedCheckSessionKey = "auth";
        private const string PostedFormSessionKey = "postedform";
        private const string SubscriptionSessionKey = "subscription";
        private const string NetsCardPayPrepareSessionKey = "netscardpayprepare";
        private const string NetsCardPayReturnSessionKey = "netscardpayreturn";
        private const string QueryParameterCallBack = "callBack";
        private const string QueryParameterOfferOrigin = "offerOrigin";

        #endregion

        #region Fields

        private readonly IServicePlusHandler<UserOutput> _servicePlusHandler;
        private readonly IUrlHelper _urlHelper;
        private readonly ILogger _logService;
        private readonly IPageSpecificSession _pageSpecificSession;
        private readonly IDetectionHandler _detection;
        private readonly IContentRepository _contentRepository;
        private readonly ISubscriptionService<Subscription, SubscriptionUser2> _subscriptionService;
        private readonly IRedirectHandler _redirectHandler;
        private readonly IServicePlusFacade _servicePlusFacade;

        #endregion

        #region Constructor

        public CampaignPageController(
            IServicePlusHandler<UserOutput> servicePlusHandler, 
            IUrlHelper urlHelper,            
            ILogger logService, 
            IPageSpecificSession pageSpecificSession,
            IDetectionHandler detection,
            IContentRepository contentRepository,
            ISubscriptionService<Subscription, SubscriptionUser2> subscriptionService, IRedirectHandler redirectHandler, IServicePlusFacade servicePlusFacade)
        {
            _servicePlusHandler = servicePlusHandler;
            _urlHelper = urlHelper;
            _logService = logService;
            _pageSpecificSession = pageSpecificSession;
            _detection = detection;
            _contentRepository = contentRepository;
            _subscriptionService = subscriptionService;
            _redirectHandler = redirectHandler;
            _servicePlusFacade = servicePlusFacade;
        }

        #endregion

        #region actions

        public ActionResult Index(CampaignPage currentPage, string selectedCamp, string code, string tg, string callBack, string offerOrigin)
        {
            // Add cache max-age=0 Because CDN adds a default max-age if no max-age is present
            //Response.Cache.SetMaxAge(new TimeSpan(0, 0, 0, 0)); 

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

            var authenticatedCheck = _pageSpecificSession.GetFromSession<AuthenticatedCheck>(currentPage, AuthenticatedCheckSessionKey);

            var shouldCheckIfAuthenticated = (authenticatedCheck == null || !string.IsNullOrEmpty(code) || !string.IsNullOrEmpty(tg));

            if (shouldCheckIfAuthenticated)
            {
                var callbackUrl = _urlHelper.GetContentUrlWithHost(currentPage.ContentLink) + AuthenticationCheckedCallBackAction;
                callbackUrl = UrlUtils.AddAllExistingQuerystrings(callbackUrl);

                return Redirect(_redirectHandler.GetCheckedLoginUrl(callbackUrl));
            }

            var model = CreateCampaignPageViewModel(currentPage, authenticatedCheck, selectedCamp);

            //if no token clear authenticated check session so a check is made on every request 
            if (string.IsNullOrEmpty(authenticatedCheck.Token))
            {
                _pageSpecificSession.ClearSession(currentPage, AuthenticatedCheckSessionKey);
            }

            return View(model);
        }

        public ActionResult AuthChecked(CampaignPage currentPage, string token, string selectedCamp, string code, string tg)
        {
            // Add cache max-age=0 Because CDN adds a default max-age if no max-age is present
            //Response.Cache.SetMaxAge(new TimeSpan(0, 0, 0, 0));

            var authenticatedCheck = new AuthenticatedCheck
            {
                PrePopulateCode = code,
                SelectedCampaign = selectedCamp,
                Token = token,
                TargetGroup = tg
            };

            _pageSpecificSession.SetInSession(currentPage, AuthenticatedCheckSessionKey, authenticatedCheck);
            
            return RedirectToAction("Index");
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult PostForm(CampaignPage currentPage, CampaignSubscribeFormModel postedForm)
        {
            // Add cache max-age=0 Because CDN adds a default max-age if no max-age is present
            //Response.Cache.SetMaxAge(new TimeSpan(0, 0, 0, 0));

            var model = new CampaignPageViewModel(currentPage) {SubscriptionForm = postedForm};          

            if (!ValidateForm(postedForm))
            {
                model.ErrorMessage = CampaignConstants.GeneralErrorMessage;
                return View("Error", model);
            }

            _pageSpecificSession.SetInSession(currentPage, PostedFormSessionKey, postedForm);

            var paymentMethod = GetPaymentMethod(postedForm);

            var subscription = _subscriptionService.InitiateSubscription(paymentMethod, postedForm.CampId, postedForm.PrenStart, postedForm.IsTrial, postedForm.IsTrialFree);

            if (!_subscriptionService.ValidateSubscription(subscription))
            {
                model.ErrorMessage = "Ett fel uppstod när kampanjen skulle läsas in. Var vänlig kontakta vår kundtjänst på tel 08-573 651 00.";
                return View("Error", model);
            }

            subscription.SetMembersByPayMethod(paymentMethod);

            subscription.TargetGroup = postedForm.TargetGroup;

            var servicePlusToken = string.Empty;
            var servicePlusUserId = string.Empty;

            var authenticatedCheck = _pageSpecificSession.GetFromSession<AuthenticatedCheck>(currentPage, AuthenticatedCheckSessionKey);
            if (authenticatedCheck != null && !string.IsNullOrEmpty(authenticatedCheck.Token))
            {
                servicePlusToken = authenticatedCheck.Token;

                var servicePlusUser = _servicePlusHandler.GetUserByToken(authenticatedCheck.Token);
                if (servicePlusUser != null && servicePlusUser.user != null)
                {
                    servicePlusUserId = servicePlusUser.user.id;
                }
            }
            
            subscription.Subscriber = GetPerson(false, postedForm, servicePlusToken, servicePlusUserId);
            subscription.SubscriptionPayer = paymentMethod == PaymentMethod.TypeOfPaymentMethod.InvoiceOtherPayer ? GetPerson(true, postedForm) : null;

            _subscriptionService.HandleDigitalSubscription(subscription, postedForm.IsDigital);

            _pageSpecificSession.SetInSession(currentPage, SubscriptionSessionKey, subscription);

            if (paymentMethod == PaymentMethod.TypeOfPaymentMethod.CreditCard ||
                paymentMethod == PaymentMethod.TypeOfPaymentMethod.CreditCardAutowithdrawal)
            {
                var err = string.Empty;
                if (SubscriptionController.DenyShortSub(subscription, out err))
                {
                    model.ErrorMessage = err;
                    return View("Error", model);
                }

                //redirect to Nets
                HandleCreditCardPayment(subscription, paymentMethod, _urlHelper.GetContentUrlWithHost(currentPage.ContentLink) + CardPaymentReturnAction);
                return null; //don't go further
            }

            return RedirectToAction("ServicePlusCheck");           
        }

        public ActionResult ServicePlusCheck(CampaignPage currentPage)
        {
            // Add cache max-age=0 Because CDN adds a default max-age if no max-age is present
            //Response.Cache.SetMaxAge(new TimeSpan(0, 0, 0, 0));

            var subscription = _pageSpecificSession.GetFromSession<Subscription>(currentPage, SubscriptionSessionKey);

            if (string.IsNullOrEmpty(subscription.Subscriber.ServicePlusUserId))
            {
                var autoRegisterUrl = GetAutoRegisterUserUrl(subscription, currentPage);
                Response.Redirect(autoRegisterUrl);
            }

            return RedirectToAction("CreateSubscription");
        }

        public ActionResult ServicePlusReturn(CampaignPage currentPage, string token)
        {
            // Add cache max-age=0 Because CDN adds a default max-age if no max-age is present
            //Response.Cache.SetMaxAge(new TimeSpan(0, 0, 0, 0));

            var subscription = _pageSpecificSession.GetFromSession<Subscription>(currentPage, SubscriptionSessionKey);

            var servicePlusUser = _servicePlusHandler.GetUserByToken(token);

            if (servicePlusUser == null || servicePlusUser.user == null)
            {
                _logService.Log("Error in CampaignPageController ServicePlusReturn - " + "_servicePlusHandler.GetUserByToken returned null for token " + token, LogLevel.Error, typeof(CampaignPageController));

                var model = new CampaignPageViewModel(currentPage)
                {
                    ErrorMessage = CampaignConstants.GeneralErrorMessage
                };
                return View("Error", model);
            }

            subscription.Subscriber.ServicePlusUserToken = token;
            subscription.Subscriber.ServicePlusUserId = servicePlusUser.user.id;

            _pageSpecificSession.SetInSession(currentPage, SubscriptionSessionKey, subscription);

            return RedirectToAction("CreateSubscription");
        }

        public ActionResult CreateSubscription(CampaignPage currentPage)
        {
            // Add cache max-age=0 Because CDN adds a default max-age if no max-age is present
            //Response.Cache.SetMaxAge(new TimeSpan(0, 0, 0, 0));

            var subscription = _pageSpecificSession.GetFromSession<Subscription>(currentPage, SubscriptionSessionKey);
            var postedForm = _pageSpecificSession.GetFromSession<CampaignSubscribeFormModel>(currentPage, PostedFormSessionKey);

            //If card payment these sessions will be set
            var prep = _pageSpecificSession.GetFromSession<NetsCardPayPrepare>(currentPage, NetsCardPayPrepareSessionKey);
            var ret = _pageSpecificSession.GetFromSession<NetsCardPayReturn>(currentPage, NetsCardPayReturnSessionKey);

            var model = new CampaignPageViewModel(currentPage) { SubscriptionForm = postedForm };

            var startPage = _contentRepository.Get<StartPage>(ContentReference.StartPage);
            var err = (prep != null && ret != null) ?
                _subscriptionService.SaveSubscription(subscription, prep.CustomerRefNo, prep.PayMethod, ret, startPage.CreateInvoiceAndPayment) : 
                _subscriptionService.SaveSubscription(subscription);

            if (String.IsNullOrEmpty(err))
            {
                SynqData(currentPage, subscription);

                if (postedForm.IsPayWall)
                {
                    // Get offerOrigin query parameter from session, if provided it is saved in session in the Index action
                    var offerOrigin = _pageSpecificSession.GetQueryParameterFromSession(currentPage, QueryParameterOfferOrigin);
                    _servicePlusFacade.CreateOrUpdateOffer(subscription.Subscriber.ServicePlusUserToken, offerOrigin);
                    _pageSpecificSession.ClearSession(currentPage, QueryParameterOfferOrigin);
                }

                _servicePlusFacade.UpdateUser(subscription.Subscriber.ServicePlusUserToken, postedForm.FirstName, postedForm.LastName, postedForm.Phone, "");
                //_servicePlusHandler.UpdateUser(subscription.Subscriber.ServicePlusUserToken, postedForm.FirstName, postedForm.LastName, postedForm.Phone);

                return RedirectToAction("ThankYou");
            }

            model.ErrorMessage = err;
            return View("Error", model);
        }

        public ActionResult CardReturn(CampaignPage currentPage, string transactionId, string responseCode)
        {
            // Add cache max-age=0 Because CDN adds a default max-age if no max-age is present
            //Response.Cache.SetMaxAge(new TimeSpan(0, 0, 0, 0));

            var model = new CampaignPageViewModel(currentPage);

            if (string.IsNullOrEmpty(responseCode))
            {
                model.ErrorMessage = CampaignConstants.GeneralErrorMessage;
                return View("Error", model);
            }

            var cardFailedUrl = _urlHelper.GetContentUrlWithHost(currentPage.ContentLink) + CardFailedReturnAction;

            var ret = new NetsCardPayReturn(cardFailedUrl);

            var prep = ret.NetsPreparePersisted;

            var subscription = (Subscription)prep.PersistedObj;

            var payOk = ret.HandleNetsReturn(string.Empty, subscription.Subscriber.Email); //Send empty string as successurl because we do not want to do the redirect

            if (!payOk) return null; //If payOk is false a redirect to the provided cardfailed url will be made   

            subscription.CreditCardPaymentOk = true;

            _pageSpecificSession.SetInSession(currentPage, SubscriptionSessionKey, subscription);
            _pageSpecificSession.SetInSession(currentPage, NetsCardPayPrepareSessionKey, prep);
            _pageSpecificSession.SetInSession(currentPage, NetsCardPayReturnSessionKey, ret);

            return RedirectToAction("ServicePlusCheck");         
        }

        public ActionResult CardFailed(CampaignPage currentPage, string transactionId, string responseCode)
        {
            // Add cache max-age=0 Because CDN adds a default max-age if no max-age is present
            //Response.Cache.SetMaxAge(new TimeSpan(0, 0, 0, 0));

            if (!string.IsNullOrEmpty(responseCode) && responseCode.ToLower().Equals(CardPaymentCancelResponseCode))
            {
                return RedirectToAction("Index");
            }

            var sbErrorMessage = new StringBuilder();
            sbErrorMessage.Append("Betalning med kort misslyckades.");

            if (!string.IsNullOrEmpty(transactionId))
            {
                var qp = new QueryPayment(transactionId);
                if (!qp.PaymentOk)
                    sbErrorMessage.Append("<br><br>Detaljerat fel:<br>" + MiscFunctions.GetNetsCardPayStatus("E", qp.ErrorResponseCode));
            }
        
            var model = new CampaignPageViewModel(currentPage)
            {
                ErrorMessage = sbErrorMessage.ToString()
            };

            return View("Error", model);
        }

        public ActionResult ThankYou(CampaignPage currentPage)
        {
            // Add cache max-age=0 Because CDN adds a default max-age if no max-age is present
            //Response.Cache.SetMaxAge(new TimeSpan(0, 0, 0, 0));

            var postedForm = _pageSpecificSession.GetFromSession<CampaignSubscribeFormModel>(currentPage, PostedFormSessionKey);
            var subscription = _pageSpecificSession.GetFromSession<Subscription>(currentPage, SubscriptionSessionKey);
            var authenticatedCheck = _pageSpecificSession.GetFromSession<AuthenticatedCheck>(currentPage, AuthenticatedCheckSessionKey);
            
            var selectedCampaign = (postedForm != null && postedForm.CampaignContentId > 0)
                ? _contentRepository.Get<CampaignBlock>(new ContentReference(postedForm.CampaignContentId))
                : null;

            var model = new CampaignPageViewModel(currentPage){SubscriptionForm = postedForm, SelectecedCampaign = selectedCampaign};

            AddTrackingInfo(selectedCampaign, subscription, authenticatedCheck);
            
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

        public ActionResult LogOut(CampaignPage currentPage, string selectedCamp)
        {
            // Add cache max-age=0 Because CDN adds a default max-age if no max-age is present
            //Response.Cache.SetMaxAge(new TimeSpan(0, 0, 0, 0));

            _pageSpecificSession.SetInSession(currentPage, AuthenticatedCheckSessionKey, null);

            var callbackUrl = UrlUtils.AddQueryString(_urlHelper.GetContentUrlWithHost(currentPage.ContentLink), SelectedCampaignQueryStringName, selectedCamp);

            Response.Redirect(_redirectHandler.GetLogoutUrl(callbackUrl));

            var model = new CampaignPageViewModel(currentPage);
            return View("index", model);
        }

        public ActionResult LogIn(CampaignPage currentPage, string selectedCamp, string code, string email)
        {
            // Add cache max-age=0 Because CDN adds a default max-age if no max-age is present
            //Response.Cache.SetMaxAge(new TimeSpan(0, 0, 0, 0));

            _pageSpecificSession.SetInSession(currentPage, AuthenticatedCheckSessionKey, null);

            var callbackUrl = UrlUtils.AddQueryString(_urlHelper.GetContentUrlWithHost(currentPage.ContentLink), SelectedCampaignQueryStringName, selectedCamp);
            callbackUrl = UrlUtils.AddQueryString(callbackUrl, PrePopulateCodeQueryStringName, code);            
            
            var loginUrl = _redirectHandler.GetLoginUrl(callbackUrl);
            // Add email to login page to prepopulate email address in S+ login form
            loginUrl = UrlUtils.AddQueryString(loginUrl, EmailQueryStringName, email);

            Response.Redirect(loginUrl);

            var model = new CampaignPageViewModel(currentPage);
            return View("index", model);
        }

        #endregion

        private CampaignPageViewModel CreateCampaignPageViewModel(CampaignPage currentPage, AuthenticatedCheck authenticatedCheck, string selectedCamp)
        {
            var model = new CampaignPageViewModel(currentPage)
            {
                IsMobileDevice = _detection.IsMobileDevice(),
                SubscriptionForm =
                {
                    TargetGroup = GetTargetGroup(currentPage, authenticatedCheck)
                },
                LoginUrl = UrlUtils.AddQueryString(_urlHelper.GetContentUrlWithHost(currentPage.ContentLink) + LogInAction, SelectedCampaignQueryStringName, selectedCamp),
                LogOutUrl = UrlUtils.AddQueryString(_urlHelper.GetContentUrlWithHost(currentPage.ContentLink) + LogOutAction, SelectedCampaignQueryStringName, selectedCamp)
            };

            if (authenticatedCheck != null)
            {                
                PopulateModelWithPrenData(authenticatedCheck.PrePopulateCode, model.SubscriptionForm);

                PopulateModelWithServicePlusData(authenticatedCheck.Token, model);

                if (!string.IsNullOrEmpty(authenticatedCheck.SelectedCampaign))
                {
                    model.SubscriptionForm.CampId = authenticatedCheck.SelectedCampaign;
                }

                if (!string.IsNullOrEmpty(authenticatedCheck.PrePopulateCode))
                {
                    model.LoginUrl = UrlUtils.AddQueryString(model.LoginUrl, PrePopulateCodeQueryStringName, authenticatedCheck.PrePopulateCode);
                }                
            }

            return model;
        }

        private Person GetPerson(bool otherPayer, CampaignSubscribeFormModel postedForm, string servicePlusToken = "", string servicePlusId = "")
        {
            var payMethod = GetPaymentMethod(postedForm);

            // Flag if a digital campaign where addressarea where not visible. Instead a form area with only first/lastname where visible.
            // Flag used to determine which inputs for firstname and lastname to use
            var nameFromDigitalArea = postedForm.IsDigital && payMethod != PaymentMethod.TypeOfPaymentMethod.Invoice;

            var firstName = nameFromDigitalArea ? postedForm.FirstNameDigital : postedForm.FirstName;
            var lastName = nameFromDigitalArea ? postedForm.LastNameDigital : postedForm.LastName;

            return new Person(!otherPayer,
                        false,
                        otherPayer ? string.Empty : firstName,
                        otherPayer ? string.Empty : lastName,
                        otherPayer ? string.Empty : postedForm.Co,
                        otherPayer ? postedForm.CompanyInvoice : postedForm.Company,
                        otherPayer ? postedForm.StreetAddressInvoice : postedForm.StreetAddress,
                        otherPayer ? postedForm.StreetNoInvoice : postedForm.StreetNo,
                        otherPayer ? string.Empty : postedForm.StairCase,
                        otherPayer ? string.Empty : postedForm.Stairs,
                        string.Empty,
                        otherPayer ? postedForm.ZipInvoice : postedForm.Zip,"", //formControl.City,
                        otherPayer ? postedForm.PhoneInvoice : postedForm.Phone, 
                        otherPayer ? string.Empty : postedForm.Email,
                        otherPayer ? string.Empty : postedForm.Ssn, //pno
                        otherPayer ? postedForm.SsnInvoice : string.Empty, //orgno 
                        otherPayer ? postedForm.AttentionInvoice : string.Empty,
                        otherPayer ? postedForm.PhoneInvoice : string.Empty, 
                        servicePlusToken,
                        servicePlusId); 
        }

        private void SynqData(CampaignPage currentPage, Subscription subscription)
        {            
            if (subscription != null && subscription.Subscriber.Cusno > 0)
            {
                // Add consent (samtycke)
                _subscriptionService.AddConsent(subscription.Subscriber.Cusno);

                // Add extra info
                var postedForm = _pageSpecificSession.GetFromSession<CampaignSubscribeFormModel>(currentPage, PostedFormSessionKey);
                var extraInfo = postedForm != null ? postedForm.ExtraInfo : string.Empty;
                
                if(!string.IsNullOrEmpty(currentPage.ExtraInfoHeading) && !string.IsNullOrEmpty(extraInfo) )
                {
                    _subscriptionService.InsertExtraInfo(currentPage.ContentLink.ID, subscription.Subscriber.Cusno, currentPage.ExtraInfoHeading, extraInfo);
                }
            }
        }

        private void HandleCreditCardPayment(Subscription subscription, PaymentMethod.TypeOfPaymentMethod payMethod, string callbackUrl)
        {
            var url = callbackUrl; 

            var goodsDescr = Settings.GetName_Product(subscription.PaperCode, subscription.ProductNo);
            var comment = "Ny prenumeration"; 
            var consumerName = string.Format("Subscriber {0} {1}", subscription.Subscriber.FirstName, subscription.Subscriber.LastName).Trim();
            var vatPct = SubscriptionController.GetProductVat(subscription.PaperCode, subscription.ProductNo);             

            new NetsCardPayPrepare(subscription.TotalPriceExVat, null, vatPct, false, false, url, goodsDescr, comment, consumerName, subscription.Subscriber.Email, null, payMethod, subscription);
        }

        private PaymentMethod.TypeOfPaymentMethod GetPaymentMethod(CampaignSubscribeFormModel postedForm)
        {
            switch (postedForm.PaymentMethod)
            {
                case PaymentConstants.Invoice:
                    if (postedForm.InvoiceOtherPayer)
                    {
                        return PaymentMethod.TypeOfPaymentMethod.InvoiceOtherPayer;
                    }
                    return PaymentMethod.TypeOfPaymentMethod.Invoice;
                case PaymentConstants.Card:
                    return PaymentMethod.TypeOfPaymentMethod.CreditCard;
                case PaymentConstants.Autowithdrawal:
                    return PaymentMethod.TypeOfPaymentMethod.CreditCardAutowithdrawal;
                case PaymentConstants.Autogiro:
                    return PaymentMethod.TypeOfPaymentMethod.DirectDebit;
                default:
                    throw new Exception("Paymentmethod not mapped");
            }
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
            if (authenticatedCheck != null && !string.IsNullOrEmpty(authenticatedCheck.TargetGroup))
                return authenticatedCheck.TargetGroup;

            return _detection.IsMobileDevice() ? currentPage.TargetGroupMobile : currentPage.TargetGroup;
        }

        private void PopulateModelWithPrenData(string code, CampaignSubscribeFormModel model)
        {
            if (string.IsNullOrEmpty(code))
            {
                return;
            }

            try
            {

                var cusNo = _subscriptionService.GetCustomerNumberByEcusno(code);

                if (cusNo > 0)
                {
                    var subscriber = new SubscriptionUser2(cusNo);

                    model.Company = subscriber.IsCompanyCust ? subscriber.RowText1 : string.Empty;

                    //TODO: IsCompanyCust verkar inte riktigt stämma? /TKM
                    //Tips från coachen (Janne): reverseline innehåller 0,1 eller 2. Om den har något annat värde än 0 så talar den om vilken av rowtext1 eller rowtext2 som innehåller personnamn.
                    //Den är dock inte heltäckande, är värdet 0 så betyder det att det inte är satt. Vilket inte är samma sak som att ingen rad innehåller person.
                    var name = subscriber.IsCompanyCust ? subscriber.RowText2 : subscriber.RowText1;

                    if (name != null)
                    {
                        var nameArray = name.Split(' ');

                        model.LastName = nameArray.Length > 0 ? nameArray[0] : string.Empty;
                        model.FirstName = nameArray.Length > 1 ? nameArray[1] : string.Empty;
                    }

                    model.Phone = subscriber.OPhone.Replace("+46", "0");
                    model.Email = subscriber.Email;
                    model.StreetAddress = subscriber.StreetName;
                    model.StreetNo = subscriber.HouseNo;
                    model.StairCase = subscriber.Staricase;
                    model.Stairs = subscriber.Apartment;

                    if (subscriber.Street2 != null)
                    {
                        foreach (var item in subscriber.Street2.Split(' '))
                        {
                            if (!item.ToLower().StartsWith("lgh"))
                                model.Co += item + " ";
                        }
                    }

                    model.Zip = subscriber.Zip;
                    model.City = subscriber.PostName;
                }
            }
            catch (Exception ex)
            {
                _logService.Log(ex, "CampaignForm.TryPopulateForm() for url code '" + code + "' failed", LogLevel.Error, typeof(CampaignPageController));
            }

        }

        private void PopulateModelWithServicePlusData(string token, CampaignPageViewModel model)
        {
            if (string.IsNullOrEmpty(token))
            {
                return;
            }

            var servicePlusUser = _servicePlusHandler.GetUserByToken(token);

            if (servicePlusUser == null || servicePlusUser.user == null)
            {
                return;
            }

            if (!string.IsNullOrEmpty(servicePlusUser.user.email))
            {
                model.SubscriptionForm.Email = servicePlusUser.user.email;
            }
            if (!string.IsNullOrEmpty(servicePlusUser.user.phoneNumber))
            {
                model.SubscriptionForm.Phone = servicePlusUser.user.phoneNumber.Replace("+46", "0");
            }

            model.SubscriptionForm.IsServicePlusUser = true;            
        }

        private void AddTrackingInfo(CampaignBlock selectedCampaign, Subscription subscription, AuthenticatedCheck authenticatedCheck)
        {
            ViewBag.LoggedIn = authenticatedCheck != null;
        
            if (selectedCampaign != null)
            {
                ViewBag.ProductName = selectedCampaign.Heading;
            }

            if (subscription != null)
            {
                var vatPct = SubscriptionController.GetProductVat(subscription.PaperCode, subscription.ProductNo);
                var vat = (vatPct / 100) * subscription.TotalPriceExVat;
                var totalPriceIncVat = subscription.TotalPriceExVat + vat;

                ViewBag.TransactionTotal = totalPriceIncVat;
                ViewBag.ProductSku = subscription.SubsNo;
                ViewBag.ProductCategory = subscription.SubsLength;
                ViewBag.ProductPrice = totalPriceIncVat;
                ViewBag.TransactionTax = (vatPct / 100) * subscription.TotalPriceExVat;
                ViewBag.PaymentMethod = subscription.PayMethod;
                ViewBag.TargetGroup = subscription.TargetGroup;
            }

            // GA-Tracking needs a unique transaction Id so we generate one
            var timeStamp = long.Parse(BonDigMisc.GetMsSince1970(DateTime.Now));
            var random = new Random().Next(0, 100);

            ViewBag.TransactionId = timeStamp + random;
        }

        private string GetAutoRegisterUserUrl(Subscription subscription, IContent currentPage)
        {
            var productId = _servicePlusHandler.GetProductId(subscription.PaperCode, subscription.ProductNo);

            var url = _servicePlusHandler.GetAutoRegisterUserUrl(
                subscription.Subscriber.Email, 
                subscription.Subscriber.FirstName, 
                subscription.Subscriber.LastName, 
                subscription.Subscriber.MobilePhone, 
                productId,
                _urlHelper.GetContentUrlWithHost(currentPage.ContentLink)) + ServiceplusReturnAction;

            return url;
        }
    }
}