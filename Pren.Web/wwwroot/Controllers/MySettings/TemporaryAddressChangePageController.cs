using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Di.Common.Logging;
using Di.Subscription.Logic.Address;
using Di.Subscription.Logic.Address.Types;
using Di.Subscription.Logic.IssueDate;
using EPiServer.Core;
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
    public class TemporaryAddressChangePageController : AddressChangeControllerBase<TemporaryAddressChangePage>
    {
        private readonly ILogger _logService;
        private readonly IAddressHandler _addressHandler;
        private readonly IIssueDateHandler _issueDateHandler;

        public TemporaryAddressChangePageController(ISessionData sessionData,
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
            issueDateHandler)
        {
            _logService = logService;
            _addressHandler = addressHandler;
            _issueDateHandler = issueDateHandler;
        }

        #region actions
        [AuthorizeUser(ValidateSubscriptionId = true)]
        public ActionResult Index(TemporaryAddressChangePage currentPage, string sid)
        {
            var model = GetViewModel(currentPage, sid);
            
            return View(model);
        }

        [AuthorizeUser]
        [ValidateAntiForgeryTokenAttribute]
        [HttpPost]
        public ActionResult CreateAddress(TemporaryAddressChangePage currentPage, StreetAddressFormModel addressForm, string sid)
        {
            var subscriber = GetSubscriberFromSession();
            var subscription = GetSubscriptionItem(subscriber, sid); 

            SetUpDates(subscription, addressForm);

            var message = ValidateInput(subscriber, subscription, addressForm);

            message = string.IsNullOrEmpty(message) ?
                CreateTemporaryAddressChange(currentPage, subscriber, subscription, addressForm) :
                message;   

            TempData["Message"] = string.IsNullOrEmpty(message) ? new Message("Din adressändring har sparats.") : new Message(message, MessageType.Danger);
            return RedirectToAction("Index", new { sid });
        }

        [AuthorizeUser]
        public ActionResult DeleteAddress(TemporaryAddressChangePage currentPage, string addressId, string sid)
        {
            var subscriber = GetSubscriberFromSession();
            var subscription = GetSubscriptionItem(subscriber, sid); 

            var message = DeleteTemporaryAddressChange(subscriber, subscription, addressId);

            TempData["Message"] = string.IsNullOrEmpty(message) ? new Message("Din adressändring har tagits bort.") : new Message(GetErrMess(), MessageType.Danger);

            return RedirectToAction("Index", new { sid });
        }

        [AuthorizeUser]        
        public ActionResult GetTempAddress(string addressId)
        {
            var subscriber = GetSubscriberFromSession();

            var addresses = GetSelectableAddresses(subscriber).FirstOrDefault(address => address.Id.Equals(addressId));

            if (addresses == null)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }

            var addressFormModel = new AddressFormModel();

            SetFieldsFromAddressMap(addresses, addressFormModel);

            return Json(addressFormModel, JsonRequestBehavior.AllowGet);
        }

        #endregion

        private void SetUpDates(SubscriptionItem subscription, StreetAddressFormModel addressForm)
        {
            var closestIssueDate = _issueDateHandler.Retriever.GetNextIssuedate(subscription.PaperCode, subscription.ProductNumber, DateTime.Now);

            if (addressForm.FromDate > closestIssueDate)
            {
                addressForm.FromDate = _issueDateHandler.Retriever.GetNextIssuedate(subscription.PaperCode, subscription.ProductNumber, addressForm.FromDate);
            }

            var tmpD2 = _issueDateHandler.Retriever.GetNextIssuedate(subscription.PaperCode, subscription.ProductNumber, addressForm.ToDate);
            if (tmpD2.Date > addressForm.ToDate.Date)
                addressForm.ToDate = tmpD2.AddDays(-1);
        }

        private string ValidateInput(Subscriber subscriber, SubscriptionItem subscription, StreetAddressFormModel addressForm)
        {
            if (!ModelState.IsValid)
            {
                return NotValidFormErrorMessage;
            }

            var closestIssueDate = _issueDateHandler.Retriever.GetNextIssuedate(subscription.PaperCode, subscription.ProductNumber, DateTime.Now);

            if (addressForm.FromDate == DateTime.MinValue)
            {
                return "Ange fråndatum";
            }

            if (addressForm.FromDate < closestIssueDate)
            {
                return "Tidigaste möjliga fråndatum är " + closestIssueDate + ".<br>Var god välj ett senare datum.";
            }

            if (addressForm.FromDate >= addressForm.ToDate)
            {
                return "Fråndatum måste vara ett tidigare datum än tilldatum.<br>Var god försök igen.";
            }

            var futureTemporaryAddresses = GetFutureTempAddresses(subscriber, subscription);

            if (!DateSpanOk(addressForm, futureTemporaryAddresses))
            {
                return "Datumintervallet kolliderar med tidigare sparad adressändring.<br>Var god försök igen";
            }

            return string.Empty;
        }

        private bool DateSpanOk(StreetAddressFormModel addressForm, IEnumerable<AddressChange> futureTempAddresses)
        {
            foreach (var old in futureTempAddresses)
            {
                //if (org != null && old.AreEqual(org) && old.StartDate == org.StartDate && old.EndDate == org.EndDate)
                //    continue;

                //StartDate in old interval
                if (addressForm.FromDate >= old.StartDate && addressForm.FromDate <= old.EndDate)
                    return false;

                //EndDate in old interval
                if (addressForm.ToDate >= old.StartDate && addressForm.ToDate <= old.EndDate)
                    return false;

                //overlapping entire old interval
                if (addressForm.FromDate < old.StartDate && addressForm.ToDate > old.EndDate)
                    return false;
            }

            return true;
        }

        private string CreateTemporaryAddressChange(TemporaryAddressChangePage currentPage, Subscriber subscriber, SubscriptionItem subscription, StreetAddressFormModel addressForm)
        {
            try
            {
                var result = _addressHandler.CreateTemporaryAddressChange(
                    subscriber.SelectedSubscription.CustomerNumber,
                    subscription.SubscriptionNumber,
                    GetLatestGenerationNumber(subscriber, subscription.SubscriptionNumber), //subscriber.GetTopExtno(subscription.SubscriptionNumber),
                    addressForm.FromDate,
                    addressForm.ToDate,
                    addressForm.StreetAddress ?? string.Empty,
                    addressForm.StreetNo ?? string.Empty,
                    addressForm.StairCase ?? string.Empty,
                    string.Empty, //floor
                    addressForm.Stairs + (!string.IsNullOrEmpty(addressForm.Stairs) && !addressForm.Stairs.ToUpper().EndsWith("TR") ? "TR" : string.Empty),
                    addressForm.Co + (!string.IsNullOrEmpty(addressForm.ApartmentNumber) ? " LGH" + addressForm.ApartmentNumber : string.Empty),
                    addressForm.Zip);

                if (result != "OK")
                {
                    _logService.Log("HandleAddressButtonClick() (tempAddrChagne) failed for cusno: " + subscriber.SelectedSubscription.CustomerNumber, LogLevel.Error, typeof(TemporaryAddressChangePageController));
                    return GetErrMess();
                }

                _logService.Log("Temporary address change made by customer " + subscriber.SelectedSubscription.CustomerNumber, LogLevel.Info, typeof(TemporaryAddressChangePageController));

                // Address created - Send mail and redirect to index view
                if (currentPage.MailConfirmText != null)
                {
                    SendCustMail(addressForm, subscriber, "tillfällig", currentPage.MailConfirmText.ToHtmlString());
                }
            }
            catch (Exception exception)
            {
                _logService.Log(exception, "HandleAddressButtonClick() (tempAddrChagne) exception for cusno: " + subscriber.SelectedSubscription.CustomerNumber, LogLevel.Error, typeof(TemporaryAddressChangePageController));
                return GetErrMess();
            }

            return string.Empty;
        }

        private int GetLatestGenerationNumber(Subscriber subscriber, long subscriptionNumber)
        {
            var generationNumber = 0;

            foreach (var subscriptionItem in subscriber.SelectedSubscription.SubscriptionItems)
            {
                if (subscriptionItem.SubscriptionNumber == subscriptionNumber &&
                    subscriptionItem.GenerationNumber > generationNumber)
                {
                    generationNumber = subscriptionItem.GenerationNumber;
                }                    
            }

            return generationNumber;
        }

        private string DeleteTemporaryAddressChange(Subscriber subscriber, SubscriptionItem subscription, string addressId)
        {
            try
            {
                var futureTemporaryAddresses = GetFutureTempAddresses(subscriber, subscription);
                var selectedAddress = futureTemporaryAddresses.SingleOrDefault(x => x.Id == addressId);

                var customerNumber = subscriber.SelectedSubscription.CustomerNumber;
                var subscriptionNumber = subscription.SubscriptionNumber;
                var externalNumber = GetLatestGenerationNumber(subscriber, subscription.SubscriptionNumber); //subscriber.GetTopExtno(subscription.Subsno);
                var startDate = selectedAddress.StartDate;


                var result = DeleteTemporaryAddress(customerNumber, subscriptionNumber, externalNumber, startDate);

                //on fail: retry with topExtno-1. (TopExtno might belong to a later sub generation).  //TODO: But why?? TKM
                if (result != "OK" && externalNumber > 0)
                {
                    externalNumber -= 1;
                    result = DeleteTemporaryAddress(customerNumber, subscriptionNumber, externalNumber, startDate);
                }

                if (result != "OK")
                {
                    _logService.Log("DeleteTemporaryAddressChange (tempAddrChagne) failed for cusno: " + subscriber.SelectedSubscription.CustomerNumber, LogLevel.Error, typeof(TemporaryAddressChangePageController));
                    return GetErrMess();
                }
            }
            catch (Exception exception)
            {
                _logService.Log(exception, "DeleteTemporaryAddressChange (tempAddrChagne) exception for cusno: " + subscriber.SelectedSubscription.CustomerNumber, LogLevel.Error, typeof(TemporaryAddressChangePageController));
                return GetErrMess();
            }

            return string.Empty;
        }

        private string DeleteTemporaryAddress(
            long customerNumber,
            long subscriptionNumber,
            int externalNumber,
            DateTime startDate)
        {
            return _addressHandler.DeleteTemporaryAddress(
                customerNumber, 
                subscriptionNumber, 
                externalNumber,
                startDate);
        }

        private TemporaryAddressChangePageViewModel GetViewModel(IContent currentPage, string sid)
        {
            var model = new TemporaryAddressChangePageViewModel(currentPage);

            var subscriber = GetSubscriberFromSession();
            var subscription = GetSubscriptionItem(subscriber, sid); 

            model.FutureTempAddresses = GetFutureTempAddresses(subscriber, subscription);
            model.AddressForm.SubscriptionNumber = sid;
            model.EarliestChangeDate = GetClosestIssueDate(subscriber.SelectedSubscription.SubscriptionItems, DateTime.Now);

            var selectableAddresses = GetSelectableAddresses(subscriber);
            model.TempAddresses = GetSelectableTempAddressesSelectItems(selectableAddresses);

            return model;
        }

        private IEnumerable<AddressChange> GetFutureTempAddresses(Subscriber subscriber, SubscriptionItem subscription)
        {
            var temporaryAddresses = _addressHandler.GetTemporaryAddressChanges(subscriber.SelectedSubscription.CustomerNumber,
                subscription.SubscriptionNumber).OrderBy(t => t.StartDate);

            return temporaryAddresses;
        }

        private IEnumerable<AddressChange> GetSelectableAddresses(Subscriber subscriber)
        {
            return _addressHandler.GetTemporaryAddresses(subscriber.SelectedSubscription.CustomerNumber);
        }

        private IEnumerable<SelectListItem> GetSelectableTempAddressesSelectItems(IEnumerable<AddressChange> addresses)
        {
            var tempAddresses = new List<SelectListItem> { new SelectListItem { Text = "Använd tidigare sparad adress" } };

            tempAddresses.AddRange(
                addresses.Select(
                    address => new SelectListItem
                    {
                        Value = address.Id, 
                        Text = address.StreetAddress + " " + address.StreetNumber + address.StairCase
                    })
            );

            return tempAddresses;
        }
    }
}