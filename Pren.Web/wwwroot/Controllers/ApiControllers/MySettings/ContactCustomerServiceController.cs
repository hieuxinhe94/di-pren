using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Di.Common.Logging;
using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using Pren.Web.Business.Controllers;
using Pren.Web.Business.Mail;
using Pren.Web.Business.Session;
using Pren.Web.Models.Pages;
using Pren.Web.Models.Pages.MySettings.Di;
using Pren.Web.Business.Subscription;

namespace Pren.Web.Controllers.ApiControllers.MySettings
{
    public class ContactCustomerServiceController : ExtendedApiController
    {
        private readonly ILogger _logger;
        private readonly ISessionData _sessionData;
        private readonly IMailHandler _mailHandler;

        public ContactCustomerServiceController(
            IApiReferrerCheck apiReferrerCheck, 
            ISessionData sessionData, 
            IMailHandler mailHandler) 
            : base(apiReferrerCheck)
        {
            _sessionData = sessionData;
            _mailHandler = mailHandler;
            _logger = new Log4NetLogger();
        }

        [HttpPost]
        public HttpResponseMessage Contact(ContactMessage contactMessage)
        {
            try
            {
                var contentRepo = ServiceLocator.Current.GetInstance<IContentRepository>();
                var startPage = contentRepo.Get<StartPage>(ContentReference.StartPage);
                var myPage = contentRepo.Get<MyStartPage>(startPage.MySettingsStartPage);

                var mailBody = new StringBuilder();
                mailBody.Append("<strong>Namn: </strong>" + contactMessage.Name);
                mailBody.Append("<br />");
                mailBody.Append("<strong>E-postadress: </strong>" + contactMessage.Email);
                mailBody.Append("<br />");
                mailBody.Append("<strong>Telefonnummer: </strong>" + contactMessage.Phone);
                mailBody.Append("<br />");
                mailBody.Append("<strong>Kundnummer: </strong>" + contactMessage.CustomerNumber);
                mailBody.Append("<br />");
                mailBody.Append("<br />");
                mailBody.Append("<strong>Meddelande:</strong>");
                mailBody.Append("<br />");
                mailBody.Append(contactMessage.Message.Replace(Environment.NewLine, "<br />"));

                var subscriber = (Subscriber)_sessionData.Get(SessionConstants.SubscriberSessionKey);
                var subscription = subscriber?.PrivateSubscription;
                var mailToAddress = subscription != null && subscription.IsAgreementCustomer ?
                    "foretag@di.se" :
                    (contactMessage.IsReceiptOrder ? myPage.ReceiptEmailTo : myPage.ContactEmailTo);

                _mailHandler.SendMail(
                    !string.IsNullOrEmpty(myPage.ContactEmailFrom) ? myPage.ContactEmailFrom : contactMessage.Email,
                    mailToAddress,
                    contactMessage.CustomerNumber,
                    mailBody.ToString(),
                    true
                    );

                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception exception)
            {
                _logger.Log(exception, "Failed to send message to customerservice " + contactMessage.ToLogString(), LogLevel.Error, typeof(ContactCustomerServiceController));
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        public HttpResponseMessage Cancel(CancelMessage contactMessage)
        {
            try
            {
                var contentRepo = ServiceLocator.Current.GetInstance<IContentRepository>();
                var startPage = contentRepo.Get<StartPage>(ContentReference.StartPage);
                var myPage = contentRepo.Get<MyStartPage>(startPage.MySettingsStartPage);

                var subscriber = (Subscriber)_sessionData.Get(SessionConstants.SubscriberSessionKey);
                var suscriptionInfo = new StringBuilder();

                foreach (var subscription in subscriber?.PrivateSubscription?.SubscriptionItems)
                {
                    suscriptionInfo.Append("Prenumerationsnummer: " + subscription.SubscriptionNumber + "<br />");
                    suscriptionInfo.Append("Produkt: " + subscription.ProductName + ", " + subscription.ProductNumber + "<br />");
                    suscriptionInfo.Append("Startdatum: " + subscription.StartDate + "<br /><br />");
                }

                var mailBody = new StringBuilder();
                mailBody.Append("<strong>Namn: </strong>" + contactMessage.Name);
                mailBody.Append("<br />");
                mailBody.Append("<strong>E-postadress: </strong>" + contactMessage.Email);
                mailBody.Append("<br />");
                mailBody.Append("<strong>Telefonnummer: </strong>" + contactMessage.Phone);
                mailBody.Append("<br />");
                mailBody.Append("<strong>Kundnummer: </strong>" + contactMessage.CustomerNumber);
                mailBody.Append("<br />");
                mailBody.Append("<strong>Prenumerationer: </strong><br />" + suscriptionInfo.ToString());
                mailBody.Append("<br />");
                mailBody.Append("<strong>Anledning: </strong>" + (contactMessage.Reason.StartsWith("--") ? "-" : contactMessage.Reason));
                mailBody.Append("<br />");
                mailBody.Append("<br />");
                mailBody.Append("<strong>Meddelande:</strong>");
                mailBody.Append("<br />");
                mailBody.Append(contactMessage.Message.Replace(Environment.NewLine, "<br />"));

                _mailHandler.SendMail(
                    contactMessage.Email,
                    myPage.CancelSubscriptionEmailTo,
                    contactMessage.CustomerNumber,
                    mailBody.ToString(),
                    true
                    );

                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception exception)
            {
                _logger.Log(exception, "Failed to send message to customerservice " + contactMessage.ToLogString(), LogLevel.Error, typeof(ContactCustomerServiceController));
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

    }

    public class ContactMessage : Loggable
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string CustomerNumber { get; set; }
        public string Message { get; set; }
        public bool IsReceiptOrder { get; set; }
    }

    public class CancelMessage : Loggable
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string CustomerNumber { get; set; }
        public string Reason { get; set; }
        public string Message { get; set; }
    }
}
