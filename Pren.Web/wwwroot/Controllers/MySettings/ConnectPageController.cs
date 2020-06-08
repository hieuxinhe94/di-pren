using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Di.Common.Utils.Url;
using Di.ServicePlus.RedirectApi;
using Di.Subscription.Logic.Customer;
using EPiServer.Core;
using Pren.Web.Business.Attributes;
using Pren.Web.Business.Detection;
using Pren.Web.Business.Helpers;
using Pren.Web.Business.Mail;
using Pren.Web.Business.Messaging;
using Pren.Web.Business.ServicePlus.Logic;
using Pren.Web.Business.Session;
using Pren.Web.Business.Subscription;
using Pren.Web.Models.Pages.MySettings;
using Pren.Web.Models.ViewModels.MySettings;

namespace Pren.Web.Controllers.MySettings
{
    [NoCache]
    // ReSharper disable Mvc.ViewNotResolved
    public class ConnectPageController : MySettingsControllerBase<ConnectPage>
    {

        #region Fields

        private readonly IUrlHelper _urlHelper;
        private readonly IMailHandler _mailHandler;
        private readonly IDetectionHandler _detectionHandler;
        private readonly IRedirectHandler _redirectHandler;
        private readonly IServicePlusFacade _servicePlusFacade;
        private readonly ICustomerHandler _customerHandler;

        #endregion

        #region Constructor

        public ConnectPageController(
            IUrlHelper urlHelper,
            IMailHandler mailHandler,
            IDetectionHandler detectionHandler,
            ISessionData sessionData, 
            IRedirectHandler redirectHandler, 
            IServicePlusFacade servicePlusFacade, 
            ICustomerHandler customerHandler) : base(sessionData)
        {
            _urlHelper = urlHelper;
            _mailHandler = mailHandler;
            _redirectHandler = redirectHandler;
            _servicePlusFacade = servicePlusFacade;
            _customerHandler = customerHandler;
            _detectionHandler = detectionHandler;
        }

        #endregion

        #region Actions

        [AuthorizeCheck]
        public ActionResult Index(ConnectPage currentPage, string code)
        {
            var subscriber = GetSubscriberFromSession();

            ClearInvalidSession(subscriber);

            var model = new ConnectPageViewModel(currentPage);

            Message message = null;

            
            switch (GetConnectStatus(subscriber))
            {
                case ConnectStatus.ConnectExistingServicePlusWithExistingPren:
                    model.CustomerNumberPren = subscriber.SelectedSubscription.CustomerNumber;
                    model.CustomerNamePren = subscriber.SelectedSubscription.KayakCustomer.FullName; 
                    model.CustomerEmailServicePlus = subscriber.ServicePlusUser.Email;
                    model.CustomerNameServicePlus = subscriber.ServicePlusUser.FirstName + " " + subscriber.ServicePlusUser.LastName;

                    return View("ConnectExisting", model);
                case ConnectStatus.ConnectExistingPrenWithServicePlus:
                    return View("ConnectExistingPren", model);
                case ConnectStatus.InvalidCode:
                    message = new Message("Du har angivit en felaktig kod. Vänligen kontakta kundservice.", MessageType.Danger);
                    break;
                case ConnectStatus.UnableToConnectPrenWithServicePlus:
                    message = new Message("Ditt Di-konto kan tyvärr inte kopplas till din prenumeration. Vänligen kontakta kundservice.", MessageType.Danger);
                    break;
                case ConnectStatus.IsConnected:
                    message = new Message("Dina konton är kopplade.");
                    model.HideForms = true;
                    break;
            }

            if (message != null)
            {
                TempData["Message"] = message;    
            }
            

            return View(model);
        }

        public ActionResult Connect(ConnectPage currentPage)
        {                        
            var subscriber = GetSubscriberFromSession();

            var imported = ImportEntitlements(subscriber);

            if (imported)
            {
                SetSubscriberInSession(null);
            }
            else
            {
                TempData["Message"] = new Message("Prenumerationen kunde tyvärr inte kopplas till ditt Di-kontot. Var god kontakta kundtjänst. Ha gärna ditt kundnummer till hands.<br>Kundnummer: " + subscriber.SelectedSubscription.CustomerNumber, MessageType.Danger);
            }

            return RedirectToAction("Index");
        }

        public ActionResult LogInUser(ConnectPage currentPage)
        {
            var callbackUrl = GetCallbackUrl(currentPage);
            SetSubscriberInSession(null);

            return RedirectToAction("LogIn", new { callbackUrl });
        }

        public ActionResult CreateAccount(ConnectPage currentPage)
        {
            var callbackUrl = GetCallbackUrl(currentPage);
            var createAccountUrl = _redirectHandler.GetCreateAccountUrl(callbackUrl);

            return Redirect(createAccountUrl);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult IdentifyByCode(ConnectPage currentPage, string code)
        {
            return RedirectToAction("Index", new {code});
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult SendCode(ConnectPage currentPage, string emailOrCusno)
        {
            long customerNumber = 0;
            Message message;

            if (_detectionHandler.IsNumeric(emailOrCusno))
            {
                customerNumber = long.Parse(emailOrCusno);
            }
            else if (_detectionHandler.IsValidEmail(emailOrCusno))
            {
                var customerNumbers = _customerHandler.FindCustomerNumbersByEmail(emailOrCusno).ToList();

                if (customerNumbers.Count == 1)
                    customerNumber = customerNumbers[0];
                else
                {
                    var emailStatusText = (customerNumbers.Count == 0) ? "Angiven e-postadress återfanns inte" : "Angiven e-postadress var inte unik";
                    var errorMessage = emailStatusText + " i vårt system. Ange om möjligt kundnummer och försök igen. Var god kontakta kundtjänst om du inte kommer vidare.";                    

                    message = new Message(errorMessage, MessageType.Danger);
                    goto Return;
                }
            }
            
            if (customerNumber < 1)
            {
                message = new Message("Ingen kund hittades. Var god kontakta kundtjänst.", MessageType.Danger);
                goto Return;
            }

            var subscriber = _customerHandler.GetCustomer(customerNumber);

            if (subscriber != null && _detectionHandler.IsValidEmail(subscriber.Email))
            {                
                var eCustomerNumber = _customerHandler.GetEcusnoByCustomerNumber(customerNumber);
                SendCodeByMail(subscriber.Email,eCustomerNumber,currentPage);

                message = new Message("Koden har nu skickats till din e-postadress. Fyll i koden i rutan nedan för att aktivera dina digitala tjänster. Var god kontakta kundtjänst om du inte har fått ett mail inom kort.");
                goto Return;
            }
             
            message = new Message("Din kod kunde tyvärr inte skickas till dig. Ingen giltig e-postadress hittades för kundnummer " + customerNumber + ". Var god kontakta kundtjänst.", MessageType.Danger);
            
            Return:
            TempData["Message"] = message;
            return RedirectToAction("Index");
        }

        #endregion

        #region Methods

        /// <summary>
        /// Imports all SubscriptionItems on Subscriber object as entitlements in ServicePlus
        /// </summary>
        /// <param name="subscriber">The subscriber with subscriptionItems to import</param>
        /// <returns>True if all entitlements where successfully imported</returns>
        private bool ImportEntitlements(Subscriber subscriber)
        {
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var isImported in subscriber.SelectedSubscription.SubscriptionItems
                .Select(subscription =>
                    _servicePlusFacade.ImportEntitlement(subscriber.ServicePlusUser.Id, subscriber.SelectedSubscription.CustomerNumber, subscription.SubscriptionNumber, subscription.PaperCode, subscription.ProductNumber)))
            {
                if (!isImported)
                    return false;
            }

            return true;
        }

        private void SendCodeByMail(string emailAdress, long eCustomerNumber, IContent currentPage)
        {
            var sb = new StringBuilder();
            sb.Append("Hej,<br><br>");
            sb.Append("Du har begärt att få en engångskod för att aktivera dina digitala tjänster hos Dagens industri.<br><br>");
            sb.Append("Din kod är: <strong>" + eCustomerNumber + "</strong><br><br>");
            sb.Append("Skriv in koden eller kopiera koden och klistra in den på registreringssidan. ");
            sb.Append("För att hitta tillbaka till sidan där koden ska registreras <a href='" + _urlHelper.GetContentUrlWithHost(currentPage.ContentLink) + "'>klicka här</a> så kommer du till rätt sida.<br><br>");
            sb.Append("Om du inte begärt någon kod från Dagens industri så ber vi dig bortse från detta mail.<br><br>");
            sb.Append("MVH<br>");
            sb.Append("Dagens industri<br>");
            sb.Append("Tel: 08-573 651 00");

            _mailHandler.SendMail("no-reply@di.se", emailAdress, "Kod till Dagens industri", sb.ToString(), true);
        }

        private string GetCallbackUrl(IContent currentPage)
        {
            var subscriber = GetSubscriberFromSession();

            var pageUrl = _urlHelper.GetContentUrlWithHost(currentPage.ContentLink);

            return subscriber != null && subscriber.SelectedSubscription != null 
                ?  UrlUtils.AddQueryString(pageUrl, UrlConstants.CodeQueryStringName, subscriber.SelectedSubscription.ECustomerNumber.ToString(CultureInfo.InvariantCulture)) 
                : pageUrl;
        }

        #endregion

    }



}