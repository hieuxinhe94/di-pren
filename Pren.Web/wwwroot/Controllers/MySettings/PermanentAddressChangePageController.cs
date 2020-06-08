using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Di.Common.Logging;
using Di.Subscription.Logic.Address;
using Di.Subscription.Logic.Address.Types;
using Di.Subscription.Logic.IssueDate;
using Pren.Web.Business.Attributes;
using Pren.Web.Business.Configuration;
using Pren.Web.Business.Detection;
using Pren.Web.Business.Mail;
using Pren.Web.Business.Messaging;
using Pren.Web.Business.Session;
using Pren.Web.Business.Subscription;
using Pren.Web.Models.CustomForms.MySettings;
using Pren.Web.Models.Pages.MySettings;
using Pren.Web.Models.ViewModels.MySettings;

namespace Pren.Web.Controllers.MySettings
{
    // ReSharper disable Mvc.ViewNotResolved
    public class PermanentAddressChangePageController : AddressChangeControllerBase<PermanentAddressChangePage>
    {
        private const string ShowAddressFormLinkText = "Ny permanent adress";

        private readonly ILogger _logService;
        private readonly IAddressHandler _addressHandler;

        public PermanentAddressChangePageController(
            ISessionData sessionData,
            IDetectionHandler detectionHandler,
            IMailHandler mailHandler,
            ILogger logService,
            ISiteSettings siteSettings,
            IAddressHandler addressHandler, IIssueDateHandler issueDateHandler)
            : base(
            sessionData,            
            detectionHandler,
            mailHandler,
            siteSettings,
            issueDateHandler
            )
        {
            _logService = logService;
            _addressHandler = addressHandler;
        }

        #region actions
        [AuthorizeUser]
        public ActionResult Index(PermanentAddressChangePage currentPage)
        {
            var model = GetViewModel(currentPage);
            return View(model);
        }

        [HttpPost]
        [AuthorizeUser]
        [ValidateAntiForgeryTokenAttribute]
        public ActionResult CreateAddress(PermanentAddressChangePage currentPage, StreetAddressFormModel addressForm)
        {
            var subscriber = GetSubscriberFromSession();

            SetUpDates(subscriber, addressForm);
            var message = ValidateInput(subscriber, addressForm);

            message = string.IsNullOrEmpty(message) ?
                CreatePermanentAddressChange(currentPage, subscriber, addressForm) :
                message;

            TempData["Message"] = string.IsNullOrEmpty(message) ? new Message("Din adressändring har sparats.") : new Message(message, MessageType.Danger);

            return RedirectToAction("Index");
        }

        [AuthorizeUser]
        public ActionResult EditAddress(PermanentAddressChangePage currentPage)
        {
            var model = GetViewModel(currentPage);

            SetFieldsFromAddressMap(model.FuturePermAddresses.FirstOrDefault(), model.AddressForm);

            return View("Index", model);
        }

        [AuthorizeUser]
        public ActionResult DeleteAddress(PermanentAddressChangePage currentPage)
        {
            var subscriber = GetSubscriberFromSession();

            var addressChange = GetFuturePermAddresses(subscriber).ToList()[0];

            var message = DeletePermanentAddressChange(subscriber, addressChange);

            TempData["Message"] = string.IsNullOrEmpty(message) ? new Message("Din adressändring har tagits bort.") : new Message(GetErrMess(), MessageType.Danger);

            return RedirectToAction("Index");
        }
        #endregion

        private void SetUpDates(Subscriber subscriber, StreetAddressFormModel addressForm)
        {
            var closestIssueDate = GetClosestIssueDate(subscriber.SelectedSubscription.SubscriptionItems, DateTime.Now);

            if (addressForm.FromDate > closestIssueDate)
            {
                addressForm.FromDate = GetClosestIssueDate(subscriber.SelectedSubscription.SubscriptionItems, addressForm.FromDate);
            }
        }

        private string ValidateInput(Subscriber subscriber, StreetAddressFormModel addressForm)
        {
            var closestIssueDate = GetClosestIssueDate(subscriber.SelectedSubscription.SubscriptionItems, DateTime.Now);

            if (!ModelState.IsValid)
            {
                return NotValidFormErrorMessage;
            }

            if (addressForm.FromDate == DateTime.MinValue)
            {
                return "Ange fråndatum";
            }

            if (addressForm.FromDate < closestIssueDate)
            {
                return "Tidigaste möjliga fråndatum är " + closestIssueDate + ".<br>Var god välj ett senare datum.";
            }

            return string.Empty;
        }

        private string CreatePermanentAddressChange(PermanentAddressChangePage currentPage, Subscriber subscriber, StreetAddressFormModel addressForm)
        {
            try
            {
                var futurePermanentAddress = GetFuturePermAddresses(subscriber).FirstOrDefault();

                if (futurePermanentAddress != null)
                {
                    var removeStatus = _addressHandler.DeletePermanentAddressChange(subscriber.SelectedSubscription.CustomerNumber, futurePermanentAddress.StartDate);

                    if (removeStatus != "OK")
                    {
                        return GetErrMess();
                    }
                }

                var result = _addressHandler.CreatePermanentAddressChange(
                    subscriber.SelectedSubscription.CustomerNumber,
                    addressForm.FromDate,
                    addressForm.StreetAddress ?? string.Empty,
                    addressForm.StreetNo ?? string.Empty,
                    addressForm.StairCase ?? string.Empty,
                    string.Empty, //floor
                    addressForm.Stairs + (!string.IsNullOrEmpty(addressForm.Stairs) && !addressForm.Stairs.ToUpper().EndsWith("TR") ? "TR" : string.Empty),
                    addressForm.Co + (!string.IsNullOrEmpty(addressForm.ApartmentNumber) ? " LGH" + addressForm.ApartmentNumber : string.Empty),
                    addressForm.Zip);

                if (result != "OK")
                {
                    _logService.Log("HandleAddressButtonClick() (permAddrChagne) failed for cusno: " + subscriber.SelectedSubscription.CustomerNumber + " - " + addressForm, LogLevel.Error, typeof(PermanentAddressChangePageController));
                    return GetErrMess();
                }

                if (currentPage.MailConfirmText != null)
                {
                    SendCustMail(addressForm, subscriber, "permanent", currentPage.MailConfirmText.ToHtmlString());
                }

                _logService.Log("Permament address change made for cusno " + subscriber.SelectedSubscription.CustomerNumber, LogLevel.Info, typeof(PermanentAddressChangePageController));
            }
            catch (Exception exception)
            {
                _logService.Log(exception, "HandleAddressButtonClick() (permAddrChagne) exception for cusno: " + subscriber.SelectedSubscription.CustomerNumber, LogLevel.Error, typeof(PermanentAddressChangePageController));                
            }

            return string.Empty;
        }

        private string DeletePermanentAddressChange(Subscriber subscriber, AddressChange addressChange)
        {
            try
            {
                var result = _addressHandler.DeletePermanentAddressChange(subscriber.SelectedSubscription.CustomerNumber, addressChange.StartDate);

                if (result != "OK")
                {
                    _logService.Log("DeletePermanentAddressChange failed for cusno: " + subscriber.SelectedSubscription.CustomerNumber, LogLevel.Error, typeof(PermanentAddressChangePageController));
                    return GetErrMess();
                }
            }
            catch (Exception exception)
            {
                _logService.Log(exception, "DeletePermanentAddressChange exception for cusno: " + subscriber.SelectedSubscription.CustomerNumber, LogLevel.Error, typeof(PermanentAddressChangePageController));
                return GetErrMess();
            }

            return string.Empty;
        }

        private PermanentAddressChangePageViewModel GetViewModel(PermanentAddressChangePage currentPage)
        {
            var subscriber = GetSubscriberFromSession();

            var model = new PermanentAddressChangePageViewModel(currentPage)
            {
                Subscriber = subscriber,
                AddressForm = { ShowFormLinkText = ShowAddressFormLinkText }
            };

            model.FuturePermAddresses = GetFuturePermAddresses(model.Subscriber);
            model.EarliestChangeDate = GetClosestIssueDate(model.Subscriber.SelectedSubscription.SubscriptionItems, DateTime.Now);

            return model;
        }

        private IEnumerable<AddressChange> GetFuturePermAddresses(Subscriber subscriber)
        {
            var addressChanges = new List<AddressChange>();
            var activeSubscriptions = subscriber.SelectedSubscription.SubscriptionItems;

            if (activeSubscriptions.Count > 0)
            {
                addressChanges.AddRange(_addressHandler.GetPermanentAddressChanges(subscriber.SelectedSubscription.CustomerNumber, activeSubscriptions[0].SubscriptionNumber));
            }

            return addressChanges.Where(t => t.StartDate > DateTime.Now.Date).ToList();
        }
    }
}