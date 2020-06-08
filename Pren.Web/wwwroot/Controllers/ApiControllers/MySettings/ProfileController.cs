using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Bn.Subscription;
using Di.Common.Logging;
using Di.Common.Security.Encryption;
using Di.Common.Utils;
using Di.Subscription.Logic.Address;
using Di.Subscription.Logic.Address.Types;
using Di.Subscription.Logic.Customer;
using Di.Subscription.Logic.HolidayStop;
using Di.Subscription.Logic.HolidayStop.Types;
using Di.Subscription.Logic.IssueDate;
using Microsoft.Ajax.Utilities;
using Pren.Web.Business.Cache;
using Pren.Web.Business.Configuration;
using Pren.Web.Business.Controllers;
using Pren.Web.Business.Detection;
using Pren.Web.Business.Invoice;
using Pren.Web.Business.Mail;
using Pren.Web.Business.ServicePlus.Logic;
using Pren.Web.Business.Session;
using Pren.Web.Business.Subscription;
using Pren.Web.Business.Utils.Translate;
using Pren.Web.Models.ViewModels.MySettings;

namespace Pren.Web.Controllers.ApiControllers.MySettings
{
    public class ProfileController : MySettingsApiControllerBase
    {
        private readonly SubscriberFacade _subscriberFacade;        
        private readonly ILogger _logService;
        private readonly IServicePlusFacade _servicePlusFacade;
        private readonly IDetectionHandler _detectionHandler;
        private readonly ISiteSettings _siteSettings;
        private readonly ISessionData _sessionData;
        private readonly ICustomerHandler _customerHandler;
        private readonly IAddressHandler _addressHandler;
        private readonly IIssueDateHandler _issueDateHandler;
        private readonly IInvoiceFacade _invoiceFacade;
        private readonly ICryptographyService _cryptographyService;
        private readonly IObjectCache _objectCache;
        private readonly ISubscriptionApi _subscriptionApi;
        
        public ProfileController( 
            IApiReferrerCheck apiReferrerCheck, 
            SubscriberFacade subscriberFacade, 
            ILogger logService, 
            IServicePlusFacade servicePlusFacade, 
            IDetectionHandler detectionHandler, 
            ISiteSettings siteSettings, 
            ISessionData sessionData, 
            ICustomerHandler customerHandler, 
            IAddressHandler addressHandler, 
            IMailHandler mailHandler, 
            IIssueDateHandler issueDateHandler, 
            IInvoiceFacade invoiceFacade, 
            ICryptographyService cryptographyService, 
            IHolidayStopHandler holidayStopHandler,
            IObjectCache objectCache, ISubscriptionApi subscriptionApi) :
            base(apiReferrerCheck, holidayStopHandler, addressHandler, mailHandler, siteSettings, objectCache, subscriptionApi)
        {
            _subscriberFacade = subscriberFacade;
            _logService = logService;
            _servicePlusFacade = servicePlusFacade;
            _detectionHandler = detectionHandler;
            _siteSettings = siteSettings;
            _sessionData = sessionData;
            _customerHandler = customerHandler;
            _addressHandler = addressHandler;
            _issueDateHandler = issueDateHandler;
            _invoiceFacade = invoiceFacade;
            _cryptographyService = cryptographyService;
            _objectCache = objectCache;
            _subscriptionApi = subscriptionApi;
        }

        [HttpGet]
        public HttpResponseMessage GetProfile()
        {
            var subscriber = (Subscriber)_sessionData.Get(SessionConstants.SubscriberSessionKey);

            if (!VerifyDomain())
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }
            if(subscriber == null)
            {
                return GetResponseMessage(new Profile());
            }

            try
            {
                if (subscriber.SelectedSubscription == null || subscriber.SelectedSubscription.HasMultipleCustomerNumbers)
                {
                    // Return empty profile for not logged in users
                    return GetResponseMessage(new Profile());
                }

                var customer = subscriber.SelectedSubscription.KayakCustomer;

                var myCodesSettings = new MyCodesSettings
                {
                    UserId = _cryptographyService.EncryptString(subscriber.ServicePlusUser.Id, _siteSettings.CryptoKeyUserId, _siteSettings.CryptoIvUserId),
                    Token = _cryptographyService.EncryptString(subscriber.ServicePlusToken, _siteSettings.CryptoKeyToken, _siteSettings.CryptoIvToken)
                };

                var profile = new Profile
                {
                    CustomerNumber = customer.CustomerNumber,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Phone = customer.PhoneOffice,
                    Email = customer.Email,
                    AddressStreetName = customer.AddressStreetName,
                    AddressStreetNumber = customer.AddressStreetNumber,
                    AddressZip = customer.AddressZip,
                    AddressCity = customer.AddressCity,
                    AddressCareOf = customer.AddressCareOf,
                    AddressStairCase = customer.AddressStairCase,
                    AddressStairs = customer.AddressStairs,
                    ActiveSubscriptions = subscriber.SelectedSubscription.SubscriptionItems.OrderBy(t => t.IsDigitalSubscription),
                    MyCodesSettings = myCodesSettings
                };

                if (!MyPage.BnApiGetSubscriptions)
                {
                    foreach (var subscription in profile.ActiveSubscriptions)
                    {
                        var issueStartDate = subscription.StartDate > DateTime.Now ? subscription.StartDate : DateTime.Now;
                        subscription.ClosestIssueDate = _issueDateHandler.Retriever.GetNextIssuedate(subscription.PaperCode, subscription.ProductNumber, issueStartDate);
                    }
                }
                
                if (profile.ActiveSubscriptions.Any())
                {
                    profile.ClosestIssueDate = profile.ActiveSubscriptions.Select(t => t.ClosestIssueDate).Max();
                    profile.MaxEndDate = profile.ActiveSubscriptions.Select(t => t.EndDate).Max();
                }

                return GetResponseMessage(profile);
            }
            catch(Exception exception)
            {
                _logService.Log(exception, "Failed to get profile. Subscriber: " + subscriber.LogInfo(), LogLevel.Error, typeof(ProfileController));
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetProfileEvents()
        {
            var subscriber = (Subscriber)_sessionData.Get(SessionConstants.SubscriberSessionKey);

            if (!VerifyDomain() || subscriber == null)
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }

            try
            {
                var eventList = new List<ProfileEventList>();
                var subscriptions = subscriber.SelectedSubscription.SubscriptionItems.OrderBy(t => t.IsDigitalSubscription);


                var eventsForSubscriber = new ProfileEventList
                {
                    SubscriptionNumber = 0,
                    SubscriptionName = "Permanent adressändring",
                    Events = new List<ProfileEvent>()
                };

                var permAddressChanges = await GetFuturePermAddresses(subscriber);
                foreach (var permAddressChange in permAddressChanges)
                {
                    eventsForSubscriber.Events.Add(new ProfileEvent
                    {
                        EventName = "Permanent adressändring",
                        ShortCut = "permaddresschange",
                        StartDate = permAddressChange.StartDate,
                        EndDate = permAddressChange.EndDate,
                    });
                }

                eventList.Add(eventsForSubscriber);

                foreach (var subscriptionItem in subscriptions)
                {
                    var eventsForSubscription = new ProfileEventList
                    {
                        SubscriptionNumber = subscriptionItem.SubscriptionNumber,
                        SubscriptionName = subscriptionItem.ProductName,
                        Events = new List<ProfileEvent>()
                    };

                    var sleeps = new List<HolidayStopItem>();

                    if (MyPage.BnApiGetHolidayStop)
                    {
                        var sleepsList = await GetFutureSubscriptionSleepsAsync(subscriber.SelectedSubscription.CustomerNumber, subscriptionItem);
                        sleeps = sleepsList.ToList();
                    }
                    else
                    {
                        sleeps = GetFutureSubscriptionSleeps(subscriptionItem).ToList();
                    }
                    

                    foreach (var holidayStopItem in sleeps)
                    {
                        eventsForSubscription.Events.Add(new ProfileEvent
                        {
                            EventName = "Uppehåll",
                            ShortCut = "subssleep",
                            StartDate = holidayStopItem.StartDate,
                            EndDate = holidayStopItem.EndDate,   
                        });
                    }

                    var tempAddressChanges = await GetFutureTempAddresses(subscriber, subscriptionItem);

                    foreach (var tmpAddressChange in tempAddressChanges)
                    {
                        eventsForSubscription.Events.Add(new ProfileEvent
                        {
                            EventName = "Tillfällig adressändring",
                            ShortCut = "tmpaddresschange",
                            StartDate = tmpAddressChange.StartDate,
                            EndDate = tmpAddressChange.EndDate,
                        });
                    }

                    eventList.Add(eventsForSubscription);
                }

                return GetResponseMessage(eventList);
            }
            catch (Exception exception)
            {
                _logService.Log(exception, "Failed to get profile.", LogLevel.Error, typeof(ProfileController));
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }


        #region Contact info

        [HttpPost]
        public async Task<HttpResponseMessage> UpdateName(string firstName, string lastName)
        {
            var subscriber = _subscriberFacade.GetSubscriberFromSession();

            if (!VerifyDomain() || subscriber == null)
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }  

            try
            {
                var updated = _customerHandler.UpdateCustomerName(subscriber.SelectedSubscription.CustomerNumber,firstName, lastName);
                // Update subscriber from sources to get new updated values
                await UpdateSubscriberFromSources(subscriber);

                return GetResponseMessage(updated);
            }
            catch (Exception exception)
            {
                _logService.Log(exception, "Failed to update name ", LogLevel.Error, typeof(ProfileController));
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }

        }

        [HttpPost]
        public async Task<HttpResponseMessage> UpdateEmail(string email)
        {
            var subscriber = _subscriberFacade.GetSubscriberFromSession();

            if (!VerifyDomain() || subscriber == null)
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }  

            try
            {
                var isValidEmail = _detectionHandler.IsValidEmail(email);
                var currentEmail = subscriber.SelectedSubscription.KayakCustomer.Email ?? string.Empty;

                //email required (cannot be empty)
                if (!isValidEmail || email == currentEmail.ToLower())
                    return GetResponseMessage(false);


                if (MyPage.BnApiUpdateCustomerEmail)
                {
                    var apiResponse = await _subscriptionApi.Customer.UpdateEmailAsync(
                        Brand,
                        subscriber.SelectedSubscription.CustomerNumber,
                        email,
                        subscriber.ServicePlusToken);

                    if (apiResponse.Result != "success")
                    {
                        _logService.Log("Failed to update email using BnApi. Message: " + apiResponse.Message + " Customer: " + subscriber.LogInfo() + " provided email: " + email, LogLevel.Error, typeof(ProfileController));
                        return GetResponseMessage(false);
                    }
                }
                else
                {
                    var splusUpdated = _servicePlusFacade.UpdateUser(subscriber.ServicePlusToken, string.Empty, string.Empty, string.Empty, email);

                    if (!splusUpdated || !UpdateSubscriberEmail(subscriber, email))
                        return GetResponseMessage(false);
                }
                
                // Update subscriber from sources to get new updated values
                await UpdateSubscriberFromSources(subscriber);
                return GetResponseMessage(true);
            }
            catch (Exception exception)
            {
                _logService.Log(exception, "Failed to update email. Customer: " + subscriber.LogInfo() + " provided email: " + email, LogLevel.Error, typeof(ProfileController));
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }

        }

        [HttpPost]
        public async Task<HttpResponseMessage> UpdatePhone([FromUri] string phone)
        {
            var status = false;

            var subscriber = _subscriberFacade.GetSubscriberFromSession();

            if (!VerifyDomain() || subscriber == null)
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }  

            try
            {
                if (!string.IsNullOrEmpty(phone))
                {
                    phone = PhoneNumberUtils.FormatPhoneNumber(phone, _siteSettings.PhoneMaxNoOfDigits, true);
                }

                if (_detectionHandler.IsValidSwePhoneNum(phone) && (phone != subscriber.SelectedSubscription.KayakCustomer.PhoneOffice))
                {
                    if (MyPage.BnApiUpdateCustomerPhone)
                    {
                        var apiResponse = await _subscriptionApi.Customer.UpdatePhoneAsync(
                            Brand,
                            subscriber.SelectedSubscription.CustomerNumber,
                            phone,
                            subscriber.ServicePlusToken);

                        if (apiResponse.Result != "success")
                        {
                            _logService.Log(
                                "Failed to update phone using BnApi. Message: " + apiResponse.Message + " Customer: " +
                                subscriber.LogInfo() + " provided phone: " + phone, LogLevel.Error,
                                typeof(ProfileController));
                            status = false;
                        }
                        else
                        {
                            // Update subscriber from sources to get new updated values
                            await UpdateSubscriberFromSources(subscriber);
                            status = true;
                        }
                    }
                    else
                    {
                        if (UpdatePhoneMobile(subscriber, phone))
                        {
                            // Update subscriber from sources to get new updated values
                            await UpdateSubscriberFromSources(subscriber);
                            status = true;
                        }
                    }                    
                }

               return GetResponseMessage(status);
            }
            catch (Exception exception)
            {
                _logService.Log(exception, "Failed to update phone '" + phone + "' Subscriber: " + subscriber.LogInfo(), LogLevel.Error, typeof(ProfileController));
                return Request.CreateResponse(HttpStatusCode.InternalServerError);                

            }
        }

        #endregion

        #region Address

        [HttpGet]
        public async Task<HttpResponseMessage> GetPermanentAddressChanges()
        {
            var subscriber = _subscriberFacade.GetSubscriberFromSession();

            if (!VerifyDomain() || subscriber == null)
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }

            var addressChanges = await GetFuturePermAddresses(subscriber);

            return GetResponseMessage(addressChanges);
        }

        [HttpPost]
        public async Task<HttpResponseMessage> SavePermanentAddress(AddressChangeItem addressForm)
        {
            //TODO: mappa till befintligt Logic.AddressChange istället? TKM
            var subscriber = _subscriberFacade.GetSubscriberFromSession();

            if (!VerifyDomain() || subscriber == null)
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }

            try
            {
                var futurePermanentAddresses = await GetFuturePermAddresses(subscriber);
                var futurePermanentAddress = futurePermanentAddresses.FirstOrDefault();

                if (futurePermanentAddress != null)
                {
                    var removeStatus = _addressHandler.DeletePermanentAddressChange(subscriber.SelectedSubscription.CustomerNumber, futurePermanentAddress.StartDate);

                    if (removeStatus != "OK")
                    {
                        return GetResponseMessage(false);
                    }
                }

                if (MyPage.BnApiCreatePermanentAddress)
                {
                    var apiResultCreate = await _subscriptionApi.PermanentAddress.CreatePermanentAddressChange(
                        Brand,
                        subscriber.SelectedSubscription.CustomerNumber,
                        addressForm.FromDate,
                        addressForm.StreetAddress ?? String.Empty,
                        addressForm.StreetNo ?? string.Empty,
                        addressForm.Zip,
                        addressForm.City,
                        addressForm.StairCase ?? string.Empty,
                        addressForm.Co + (!string.IsNullOrEmpty(addressForm.ApartmentNumber) ? " LGH" + addressForm.ApartmentNumber : string.Empty),
                        addressForm.Stairs + (!string.IsNullOrEmpty(addressForm.Stairs) && !addressForm.Stairs.ToUpper().EndsWith("TR") ? "TR" : string.Empty),
                        addressForm.ApartmentNumber ?? string.Empty);

                    if (apiResultCreate.Result != "success")
                    {
                        _logService.Log("Permanent address change failed using BN Api. Message; " + apiResultCreate.Message + " Subscriber: " + subscriber.LogInfo() + " Posted form: " + addressForm.ToLogString(), LogLevel.Error, typeof(ProfileController));
                        return GetResponseMessage(false);
                    }
                }
                else
                {
                    var result = _addressHandler.CreatePermanentAddressChange(
                        subscriber.SelectedSubscription.CustomerNumber,
                        addressForm.FromDate,
                        addressForm.StreetAddress ?? string.Empty,
                        addressForm.StreetNo ?? string.Empty,
                        addressForm.StairCase ?? string.Empty,
                        string.Empty, //Floor
                        addressForm.Stairs + (!string.IsNullOrEmpty(addressForm.Stairs) && !addressForm.Stairs.ToUpper().EndsWith("TR") ? "TR" : string.Empty),
                        addressForm.Co + (!string.IsNullOrEmpty(addressForm.ApartmentNumber) ? " LGH" + addressForm.ApartmentNumber : string.Empty),
                        addressForm.Zip);

                    if (result != "OK")
                    {
                        _logService.Log("Permanent address change failed. Subscriber: " + subscriber.LogInfo() + " Posted form: " + addressForm.ToLogString(), LogLevel.Error, typeof(ProfileController));
                        return GetResponseMessage(false);
                    }
                }               

                _objectCache.RemoveFromCache(PermAddressCacheKey + subscriber.SelectedSubscription.CustomerNumber);

                return GetResponseMessage(true);
            }
            catch (Exception exception)
            {
                _logService.Log(exception, "Permanent address change failed. Subscriber: " + subscriber.LogInfo() + " Posted form: " + addressForm.ToLogString(), LogLevel.Error, typeof(ProfileController));
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> DeletePermanentAddress()
        {
            var subscriber = _subscriberFacade.GetSubscriberFromSession();

            if (!VerifyDomain() || subscriber == null)
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }

            try
            {
                var futureAddressChanges = await GetFuturePermAddresses(subscriber);

                var addressChange = futureAddressChanges.ToList()[0];

                if (MyPage.BnApiDeletePermanentAddress)
                {
                    var apiResult = await _subscriptionApi.PermanentAddress.DeletePermanentAddress(
                        Brand,
                        subscriber.SelectedSubscription.CustomerNumber, 
                        addressChange.Id, 
                        addressChange.StartDate);

                    if (apiResult.Result != "success")
                    {
                        _logService.Log("DeletePermanentAddressChange failed using Bn APi for cusno: " + subscriber.SelectedSubscription.CustomerNumber, LogLevel.Error, typeof(ProfileController));
                        return GetResponseMessage(false);
                    }
                }
                else
                {
                    var result = _addressHandler.DeletePermanentAddressChange(subscriber.SelectedSubscription.CustomerNumber, addressChange.StartDate);

                    if (result != "OK")
                    {
                        _logService.Log("DeletePermanentAddressChange failed for cusno: " + subscriber.SelectedSubscription.CustomerNumber, LogLevel.Error, typeof(ProfileController));
                        return GetResponseMessage(false);
                    }
                }



                _objectCache.RemoveFromCache(PermAddressCacheKey + subscriber.SelectedSubscription.CustomerNumber);

                return Request.CreateResponse(true);
            }
            catch (Exception exception)
            {
                _logService.Log(exception, "DeletePermanentAddressChange exception for cusno: " + subscriber.SelectedSubscription.CustomerNumber, LogLevel.Error, typeof(ProfileController));
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetEditPermanentAddress()
        {
            var subscriber = _subscriberFacade.GetSubscriberFromSession();

            if (!VerifyDomain() || subscriber == null)
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }

            var futureAddressChanges = await GetFuturePermAddresses(subscriber);

            var addressChange = futureAddressChanges.FirstOrDefault();
            var editAddress = GetEditAddress(addressChange);

            return GetResponseMessage(editAddress);
        }

        #endregion

        #region Invoices

        [HttpGet]
        public async Task<HttpResponseMessage> GetInvoices()
        {
            var subscriber = _subscriberFacade.GetSubscriberFromSession();

            if (!VerifyDomain() || !VerifySubscriber(subscriber))
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }

            try
            {
                IEnumerable<CustomerInvoice> invoices;

                if (MyPage.BnApiGetInvoices)
                {
                    //invoices = await GetInvoicesFromApi(4074328); //for test
                    var customerNumber = subscriber.SelectedSubscription.CustomerNumber;
                    invoices = await GetInvoicesFromApi(customerNumber); //invoices from pren and new archive
                }
                else
                {
                    //invoices = await _invoiceFacade.GetAllInvoicesAsync(4079832); //for test
                    invoices = await _invoiceFacade.GetAllInvoicesAsync(subscriber.SelectedSubscription.CustomerNumber);
                }

                return GetResponseMessage(invoices);
            }
            catch (Exception exception)
            {
                _logService.Log(exception, "Failed to GetInvoices. Subscriber: " + subscriber.LogInfo(), LogLevel.Error, typeof(SubscriptionController));
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }


        #endregion

        #region privates

        private async Task<IEnumerable<CustomerInvoice>> GetInvoicesFromApi(long customerNumber)
        {
            var invoices = await _subscriptionApi.Invoice.GetInvoicesAsync(Brand, customerNumber);

            return invoices.Data.Select(invoice => new CustomerInvoice
            {
                InvoiceNumber = invoice.InvoiceNumber,
                InvoiceDate = invoice.InvoiceDate,
                DueDate = invoice.DueDate,
                InvoiceAmount = invoice.InvoiceAmount,
                InvoicePayed = invoice.InvoicePayed,
                InvoiceGuid = invoice.InvoiceHash
            });
        }

        private async Task UpdateSubscriberFromSources(Subscriber subscriberToUpdate)
        {
            var subscriber = await _subscriberFacade.GetSubscriber(subscriberToUpdate.ServicePlusToken);
            _sessionData.Set(SessionConstants.SubscriberSessionKey, subscriber);
        }

        private bool UpdateSubscriberEmail(Subscriber subscriber, string email)
        {
            try
            {
                if (!string.IsNullOrEmpty(email))
                    email = email.ToLower();

                return _customerHandler.UpdateCustomerEmail(subscriber.SelectedSubscription.CustomerNumber, email);
            }
            catch (Exception ex)
            {
                _logService.Log(ex, "UpdateEmail() failed", LogLevel.Error, typeof(ProfileController));
            }

            return false;
        }

        private bool UpdatePhoneMobile(Subscriber subscriber, string phoneMob)
        {
            try
            {
                return _customerHandler.UpdateCustomerPhone(subscriber.SelectedSubscription.CustomerNumber, phoneMob);
            }
            catch (Exception ex)
            {
                _logService.Log(ex, "UpdatePhoneMobile() failed", LogLevel.Error, typeof(ProfileController));
            }
            return false;
        }

        protected AddressChangeItem GetEditAddress(AddressChange address)
        {
            var addressForm = new AddressChangeItem
            {
                Co = GetCareOf(address.Street2),
                StreetAddress = address.StreetAddress,
                StreetNo = address.StreetNumber,
                StairCase = address.StairCase,
                Stairs =
                    (string.IsNullOrEmpty(address.Apartment))
                        ? string.Empty
                        : address.Apartment.Replace("TR", "").Trim(),
                ApartmentNumber = GetApartmentNo(address.Street2),
                Zip = address.Zip,
                City = address.City,
                FromDate = address.StartDate,
                ToDate = DateTime.MinValue
            };

            if (address.EndDate > DateTime.MinValue && address.EndDate != new DateTime(2078, 12, 31))
            {
                addressForm.ToDate = address.EndDate;
            }

            return addressForm;
        }

        private string GetCareOf(string street2)
        {
            if (string.IsNullOrEmpty(street2))
                return string.Empty;

            var co = street2;
            var lghIndex = street2.IndexOf("LGH", StringComparison.Ordinal);
            if (lghIndex > -1)
                co = street2.Substring(0, lghIndex).Trim();

            return co;
        }

        private string GetApartmentNo(string street2)
        {
            if (string.IsNullOrEmpty(street2))
                return string.Empty;

            var appNo = "";
            var lghIndex = street2.IndexOf("LGH", StringComparison.Ordinal);
            if (lghIndex > -1)
                appNo = street2.Substring(lghIndex + 3).Trim();

            return appNo;
        }

        #endregion
    }

    public class ProfileEventList
    {
        public string SubscriptionName { get; set; }
        public long SubscriptionNumber { get; set; }
        public List<ProfileEvent> Events { get; set; }
    }

    public class ProfileEvent
    {
        public string EventName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string EventDetails { get; set; }
        public string ShortCut { get; set; }
    }

    public class Profile
    {
        public long CustomerNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string AddressStreetName { get; set; }
        public string AddressStreetNumber { get; set; }
        public string AddressStairCase { get; set; }
        public string AddressStairs { get; set; }
        public string AddressCareOf { get; set; }
        public string AddressZip { get; set; }
        public string AddressCity { get; set; }
        public DateTime ClosestIssueDate { get; set; }
        public DateTime MaxEndDate { get; set; }
        public IEnumerable<SubscriptionItem> ActiveSubscriptions { get; set; }
        public MyCodesSettings MyCodesSettings { get; set; }
    }

    public class MyCodesSettings
    {
        public string Token { get; set; }
        public string UserId { get; set; }
    }
}
