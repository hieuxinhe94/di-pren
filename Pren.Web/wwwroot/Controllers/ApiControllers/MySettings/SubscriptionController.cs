using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Bn.Subscription;
using Di.Common.Logging;
using Di.Subscription.Logic.Address;
using Di.Subscription.Logic.Address.Types;
using Di.Subscription.Logic.HolidayStop;
using Di.Subscription.Logic.HolidayStop.Types;
using Di.Subscription.Logic.IssueDate;
using Di.Subscription.Logic.PublicationDays;
using Di.Subscription.Logic.Reclaim;
using Pren.Web.Business.Cache;
using Pren.Web.Business.Configuration;
using Pren.Web.Business.Controllers;
using Pren.Web.Business.Mail;
using Pren.Web.Business.Subscription;
using Pren.Web.Business.Utils.Translate;
using Pren.Web.Models.ViewModels.MySettings;

namespace Pren.Web.Controllers.ApiControllers.MySettings
{
    public class SubscriptionController : MySettingsApiControllerBase
    {
        private readonly SubscriberFacade _subscriberFacade;        
        private readonly ILogger _logService;
        private readonly IPublicationDaysHandler _publicationDaysHandler;
        private readonly IReclaimHandler _reclaimHandler;
        private readonly IHolidayStopHandler _holidayStopHandler;
        private readonly IAddressHandler _addressHandler;
        private readonly IIssueDateHandler _issueDateHandler;
        private readonly IObjectCache _objectCache;
        private readonly ISubscriptionApi _subscriptionApi;

        public SubscriptionController( 
            IApiReferrerCheck apiReferrerCheck, 
            SubscriberFacade subscriberFacade, 
            ILogger logService, 
            ISiteSettings siteSettings, 
            IAddressHandler addressHandler, 
            IMailHandler mailHandler, 
            IIssueDateHandler issueDateHandler, 
            IPublicationDaysHandler publicationDaysHandler, 
            IReclaimHandler reclaimHandler,
            IHolidayStopHandler holidayStopHandler,
            IObjectCache objectCache, 
            ISubscriptionApi subscriptionApi)
            : base(apiReferrerCheck, holidayStopHandler, addressHandler, mailHandler, siteSettings, objectCache, subscriptionApi)
        {
            _subscriberFacade = subscriberFacade;
            _logService = logService;
            _addressHandler = addressHandler;
            _issueDateHandler = issueDateHandler;
            _publicationDaysHandler = publicationDaysHandler;
            _reclaimHandler = reclaimHandler;
            _holidayStopHandler = holidayStopHandler;
            _objectCache = objectCache;
            _subscriptionApi = subscriptionApi;
        }

        #region Reclaim

        [HttpGet]
        public async Task<HttpResponseMessage> GetReclaimDetails(long subscriptionNumber)
        {
            var subscriber = _subscriberFacade.GetSubscriberFromSession();

            if (!VerifyDomain() || !VerifySubscriber(subscriber, subscriptionNumber))
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }

            try
            {
                // Get customers existing reclaims
                var customerReclaims = new List<CustomerReclaim>();

                if (MyPage.BnApiGetReclaims)
                {
                    var apiResponse = await _subscriptionApi.Reclaim.GetReclaimsAsync(
                        Brand,
                        subscriber.SelectedSubscription.CustomerNumber);

                    if (apiResponse.Result != "success")
                    {
                        _logService.Log("Failed to GetReclaimsAsync. for subscriber " + subscriber.LogInfo(),
                            LogLevel.Error, typeof(SubscriptionController));
                    }
                    else
                    {
                        customerReclaims.AddRange(apiResponse.Data.Select(x => new CustomerReclaim
                        {
                            Id = x.Id,
                            ReclaimText = x.DisplayText,
                            ReclaimDate = x.ReclaimDate
                        }));
                    }


                }
                else
                {
                    customerReclaims.AddRange(
                            _reclaimHandler.GetCustomerReclaims(subscriber.SelectedSubscription.CustomerNumber)
                                .ToList()
                                .Select(x => new CustomerReclaim
                                {
                                    Id = x.Id.ToString(),
                                    ReclaimText = x.ReclaimText,
                                    ReclaimDate = x.ReclaimDate
                                }));
                }
                
                var subscription = GetSubscriptionItem(subscriber, subscriptionNumber);

                var reclaimItems = new List<ReclaimItem>();

                if (MyPage.BnApiGetReclaimTypes)
                {
                    var apiResponse = await _subscriptionApi.Reclaim.GetReclaimTypesAsync(Brand);

                    if (apiResponse.Result != "success")
                    {
                        _logService.Log("Failed to GetReclaimReasons.", LogLevel.Error, typeof(SubscriptionController));
                        return Request.CreateResponse(HttpStatusCode.InternalServerError);
                    }

                    reclaimItems.AddRange(apiResponse.Data.Select(x => new ReclaimItem(x.DisplayText, x.Id)));
                }
                else
                {
                    var reclaimTypeFilterForUiDictionary = GetReclaimTypeFilterForUiDictionary();
                    reclaimItems = GetReclaimItems(reclaimTypeFilterForUiDictionary).ToList();
                }


                var daysToReclaim = await GetAvailableDaysToReclaim(subscriber.SelectedSubscription.CustomerNumber, subscription, 5);

                var daysToReclaimList = daysToReclaim.ToList();

                foreach (var reclaimDay in daysToReclaimList)
                {
                    // If customer already made a reclaim, the day should be disabled
                    reclaimDay.Disabled =
                        customerReclaims.Any(
                            t => t.ReclaimDate.ToShortDateString() == reclaimDay.Date.ToShortDateString());
                }

                var response = new
                {
                    ReclaimItems = reclaimItems,
                    DaysToReclaim = daysToReclaimList
                };

                return GetResponseMessage(response);
            }
            catch (Exception exception)
            {
                _logService.Log(exception, "Failed to GetReclaimReasons.", LogLevel.Error, typeof(SubscriptionController));
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> SaveReclaim(Reclaim reclaim)
        {
            var subscriber = _subscriberFacade.GetSubscriberFromSession();

            if (!VerifyDomain() || !VerifySubscriber(subscriber, reclaim.SubscriptionId))
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }

            try
            {
                var subscription = GetSubscriptionItem(subscriber, reclaim.SubscriptionId);

                if (subscription.IsDigitalSubscription || !subscription.IsActive)
                    return GetResponseMessage(false);

                foreach (var date in reclaim.DaysToReclaim)
                {
                    try
                    {
                        if (MyPage.BnApiCreateReclaims)
                        {
                            var apiResponse = await _subscriptionApi.Reclaim.CreateReclaimAsync(
                                Brand,
                                subscriber.SelectedSubscription.CustomerNumber,
                                subscription.SubscriptionNumber,
                                DateTime.Parse(date),
                                reclaim.Reason);

                            if (apiResponse.Result != "success")
                            {
                                _logService.Log(string.Format("SubscriptionController.SaveReclaim Failed. Using BnApi " + apiResponse.Message + " Params: customerNumber: {0},  subscriptionNumber: {1}, extNo: {2}, paperCode: {3}, reasonId: {4}, date: {5}", subscriber.SelectedSubscription.CustomerNumber, subscription.SubscriptionNumber, subscription.GenerationNumber, subscription.PaperCode, reclaim.Reason, date), LogLevel.Error, typeof(SubscriptionController));
                                return GetResponseMessage(false);
                            }
                        }
                        else
                        {
                            var result = _reclaimHandler.CreateDeliveryReclaim(subscriber.SelectedSubscription.CustomerNumber, subscription.SubscriptionNumber, subscription.GenerationNumber, subscription.PaperCode, reclaim.Reason, DateTime.Parse(date));

                            if (result != "OK")
                                return GetResponseMessage(false);
                        }


                    }
                    catch (Exception exception)
                    {
                        _logService.Log(exception, string.Format("SubscriptionController.SaveReclaim Failed Params: customerNumber: {0},  subscriptionNumber: {1}, extNo: {2}, paperCode: {3}, reasonId: {4}, date: {5}", subscriber.SelectedSubscription.CustomerNumber, subscription.SubscriptionNumber, subscription.GenerationNumber, subscription.PaperCode, reclaim.Reason, date), LogLevel.Error, typeof(SubscriptionController));
                        return Request.CreateResponse(HttpStatusCode.InternalServerError);
                    }
                }

                return GetResponseMessage(true);

            }
            catch (Exception exception)
            {
                _logService.Log(exception, "Failed to SaveReclaim for Cusno:" + subscriber.SelectedSubscription.CustomerNumber +
                ", Email: " + subscriber.ServicePlusUser.Email + 
                ", Reclaim" + reclaim.ToLogString(), LogLevel.Error, typeof(SubscriptionController));
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        #endregion

        #region Subssleep

        [HttpGet]
        public async Task<HttpResponseMessage> GetSubscriptionSleeps(long subscriptionNumber)
        {
            var subscriber = _subscriberFacade.GetSubscriberFromSession();

            if (!VerifyDomain() || !VerifySubscriber(subscriber, subscriptionNumber))
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }

            try
            {
                var subscription = GetSubscriptionItem(subscriber, subscriptionNumber);

                List<HolidayStopItem> sleepingSubscriptions;

                if (MyPage.BnApiGetHolidayStop)
                {
                    var sleepsList = await GetFutureSubscriptionSleepsAsync(subscriber.SelectedSubscription.CustomerNumber, subscription);
                    sleepingSubscriptions = sleepsList.ToList();
                }
                else
                {
                    sleepingSubscriptions = GetFutureSubscriptionSleeps(subscription).ToList();
                }
                
                var sleepItem = new 
                {
                    SubscriptionSleeps = sleepingSubscriptions
                };

                return GetResponseMessage(sleepItem);
            }
            catch (Exception exception)
            {
                _logService.Log(exception, "Failed to GetSubscriptionSleeps.", LogLevel.Error, typeof(SubscriptionController));
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> SaveSubscriptionSleep(SubscriptionSleepItem subscriptionSleep)
        {
            var subscriber = _subscriberFacade.GetSubscriberFromSession();

            if (!VerifyDomain() || !VerifySubscriber(subscriber, subscriptionSleep.SubscriptionId))
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }

            try
            {
                var subscription = GetSubscriptionItem(subscriber, subscriptionSleep.SubscriptionId);

                if (subscription.IsDigitalSubscription) 
                    return GetResponseMessage(false);

                var message = await ValidateSubscriptionSleepAsync(subscriber.SelectedSubscription.CustomerNumber, subscription, subscriptionSleep);

                if (!string.IsNullOrEmpty(message))
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, message);
                }

                if (MyPage.BnApiCreateHolidayStop)
                {
                    var apiResult = await _subscriptionApi.HolidayStop.CreateHolidayStopAsync(
                        Brand,
                        subscriber.SelectedSubscription.CustomerNumber,
                        subscriptionSleep.SubscriptionId,
                        subscription.GenerationNumber,
                        subscriptionSleep.FromDate,
                        subscriptionSleep.ToDate);

                    if (apiResult.Result != "success")
                    {
                        _logService.Log(string.Format("CreateHolidayStop - failed. Cusno:{0}, UrlSubsno:{1}, DateStart:{2}, DateEnd:{3}, allowWebPaper:{4}, creditType:{5} - ",
                                subscriber.SelectedSubscription.CustomerNumber, subscriptionSleep.SubscriptionId,
                                subscriptionSleep.FromDate, subscriptionSleep.ToDate, "Y", "03") + apiResult.Message, LogLevel.Error, typeof(SubscriptionController));

                        return GetResponseMessage(false);
                    }
                }
                else
                {
                    var result = _holidayStopHandler.CreateHolidayStop(subscriptionSleep.FromDate, subscriptionSleep.ToDate, subscriptionSleep.SubscriptionId);

                    if (result != "OK")
                    {
                        _logService.Log(string.Format("CreateHolidayStop - failed. Cusno:{0}, UrlSubsno:{1}, DateStart:{2}, DateEnd:{3}, allowWebPaper:{4}, creditType:{5} - ", subscriber.SelectedSubscription.CustomerNumber, subscriptionSleep.SubscriptionId, subscriptionSleep.FromDate, subscriptionSleep.ToDate, "Y", "03") + result, LogLevel.Error, typeof(SubscriptionController));
                        return GetResponseMessage(false);
                    }
                }               

                _objectCache.RemoveFromCache(SubsSleepCacheKey + subscriptionSleep.SubscriptionId);
                return GetResponseMessage(true);
            }
            catch (Exception exception)
            {
                _logService.Log(exception, "Failed to SaveReclaim.", LogLevel.Error, typeof(SubscriptionController));
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }            
        }

        [HttpPost]
        public async Task<HttpResponseMessage> DeleteSubscriptionSleep(long subscriptionNumber, DateTime startDate, DateTime endDate)
        {
            var subscriber = _subscriberFacade.GetSubscriberFromSession();

            if (!VerifyDomain() || !VerifySubscriber(subscriber, subscriptionNumber))
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }

            try
            {
                var subscription = GetSubscriptionItem(subscriber, subscriptionNumber);

                if (subscription.IsDigitalSubscription)
                    return GetResponseMessage(false);

                bool deleteSuccess;

                if (MyPage.BnApiDeleteHolidayStop)
                {
                    var apiResult = await _subscriptionApi.HolidayStop.DeleteHolidayStopAsync(
                        Brand,
                        subscriber.SelectedSubscription.CustomerNumber,
                        subscription.SubscriptionNumber,
                        subscription.GenerationNumber,
                        startDate,
                        endDate);

                    deleteSuccess = apiResult.Result == "success";
                }
                else
                {
                    var result = _holidayStopHandler.DeleteHolidayStop(subscription.SubscriptionNumber, subscription.GenerationNumber, startDate);
                    deleteSuccess = result == "OK";
                }

                if (deleteSuccess)
                {
                    _objectCache.RemoveFromCache(SubsSleepCacheKey + subscriptionNumber);
                }
                else
                {
                    _logService.Log("Failed to DeleteReclaim. " + subscriber.LogInfo() + " subscriptionnumber: " + subscriptionNumber + " startdate " + startDate + " enddate " + endDate, LogLevel.Error, typeof(SubscriptionController));
                }

                return GetResponseMessage(deleteSuccess);
            }
            catch (Exception exception)
            {
                _logService.Log(exception, "Failed to DeleteReclaim. " + subscriber.LogInfo() + " subscriptionnumber: " + subscriptionNumber + " startdate " + startDate + " enddate " + endDate, LogLevel.Error, typeof(SubscriptionController));
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> ChangeSubscriptionSleep(SubscriptionSleepItem subscriptionSleep)
        {
            var subscriber = _subscriberFacade.GetSubscriberFromSession();

            if (!VerifyDomain() || !VerifySubscriber(subscriber, subscriptionSleep.SubscriptionId))
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }

            try
            {
                var subscription = GetSubscriptionItem(subscriber, subscriptionSleep.SubscriptionId);

                var message = await ValidateSubscriptionSleepAsync(subscriber.SelectedSubscription.CustomerNumber, subscription, subscriptionSleep);

                if (!string.IsNullOrEmpty(message))
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, message);
                }

                bool changeSuccess;

                if (MyPage.BnApiChangeHolidayStop)
                {
                    var apiResult = await _subscriptionApi.HolidayStop.ChangeHolidayStopAsync(
                        Brand,
                        subscriber.SelectedSubscription.CustomerNumber,
                        subscription.SubscriptionNumber,
                        subscription.GenerationNumber,
                        subscriptionSleep.FromDate,
                        subscriptionSleep.FromDateOrg,
                        subscriptionSleep.ToDate,
                        subscriptionSleep.ToDateOrg);

                    changeSuccess = apiResult.Result == "success";
                }
                else
                {
                    var result = _holidayStopHandler.ChangeHolidayStop(
                        subscription.SubscriptionNumber,
                        subscription.GenerationNumber,
                        subscriptionSleep.FromDateOrg,
                        subscriptionSleep.FromDate,
                        subscriptionSleep.ToDateOrg,
                        subscriptionSleep.ToDate);

                    changeSuccess = result == "OK";
                }
                
                if (changeSuccess)
                {
                    _objectCache.RemoveFromCache(SubsSleepCacheKey + subscriptionSleep.SubscriptionId);
                }
                else
                {
                    _logService.Log("Failed to ChangeSubscriptionSleep. " + subscriber.LogInfo() + " " + subscriptionSleep.ToLogString(), LogLevel.Error, typeof(SubscriptionController));
                }

                return GetResponseMessage(changeSuccess);   
            }
            catch (Exception exception)
            {
                _logService.Log(exception, "Failed to ChangeSubscriptionSleep. " + subscriber.LogInfo() + " " + subscriptionSleep.ToLogString(), LogLevel.Error, typeof(SubscriptionController));
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }         
        }

        #endregion

        #region Tmp address changes

        [HttpGet]
        public async Task<HttpResponseMessage> GetTemporaryAddressChanges(long subscriptionNumber)
        {
            var subscriber = _subscriberFacade.GetSubscriberFromSession();

            if (!VerifyDomain() || !VerifySubscriber(subscriber, subscriptionNumber))
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }

            try
            {
                var subscription = GetSubscriptionItem(subscriber, subscriptionNumber);

                var temporaryAddressChanges = await GetFutureTempAddresses(subscriber, subscription);

                var temporaryAddresses = new List<AddressChange>();

                if (MyPage.BnApiGetTemporaryAddressList)
                {
                    var apiResult = await _subscriptionApi.TemporaryAddress.GetTemporaryAddressesList(
                        Brand,
                        subscriber.SelectedSubscription.CustomerNumber);

                    if (apiResult.Result != "success")
                    {
                        _logService.Log($"Failed to GetTemporaryAddressChanges with Bn Api. message: {apiResult.Message} subscriber: {subscriber.LogInfo()} subscriptionNumber: {subscriptionNumber}", LogLevel.Error, typeof(SubscriptionController));
                        return Request.CreateResponse(HttpStatusCode.InternalServerError);
                    }

                    temporaryAddresses.AddRange(apiResult.Data.Select(x => new AddressChange
                    {
                        Id = x.Pointer,
                        StreetAddress = x.StreetName,
                        StreetNumber = x.StreetNumber,
                        StairCase = x.StairCase,
                        Apartment = x.ApartmentNumber,
                        Zip = x.Zip,
                        City = x.City
                    })); 
                }
                else
                {
                    temporaryAddresses = _addressHandler.GetTemporaryAddresses(subscriber.SelectedSubscription.CustomerNumber).ToList();
                }
                
                var addessChanges = new
                {
                    TmpChanges = temporaryAddressChanges,
                    TmpAddresses = temporaryAddresses
                };

                return GetResponseMessage(addessChanges);
            }
            catch (Exception exception)
            {
                _logService.Log(exception, "Failed to GetTemporaryAddressChanges.", LogLevel.Error, typeof(SubscriptionController));
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> ChangeTemporaryAddressChange(TemporaryAddressItem addressChange)
        {
            var subscriber = _subscriberFacade.GetSubscriberFromSession();
            var subscriptionNumber = addressChange.SubscriptionId;

            if (!VerifyDomain() || !VerifySubscriber(subscriber, subscriptionNumber))
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }

            try
            {
                var subscription = GetSubscriptionItem(subscriber, subscriptionNumber);

                var message = await ValidateTmpAddressChange(subscriber, subscription, new AddressChangeItem {FromDate = addressChange.FromDate, ToDate = addressChange.ToDate}, addressChange.FromDateOrg, addressChange.ToDateOrg);

                if (!string.IsNullOrEmpty(message))
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, message);
                }

                bool changeSuccess;

                if (MyPage.BnApiChangeTemporaryAddress)
                {
                    var apiResult = await _subscriptionApi.TemporaryAddress.ChangeTemporaryAddressChangeAsync(
                        Brand,
                        subscriber.SelectedSubscription.CustomerNumber,
                        subscriptionNumber,
                        subscription.GenerationNumber,
                        addressChange.Id,
                        addressChange.FromDate,
                        addressChange.FromDateOrg,
                        addressChange.ToDate,
                        addressChange.ToDateOrg);

                    changeSuccess = apiResult.Result == "success";
                }
                else
                {
                    var result = _addressHandler.ChangeTemporaryAddressChange(subscriber.SelectedSubscription.CustomerNumber, subscriptionNumber, subscription.GenerationNumber, addressChange.FromDateOrg, addressChange.FromDate, addressChange.ToDate, false, DateTime.Now);

                    changeSuccess = result == "OK";
                }
                
                if (changeSuccess)
                {
                    _objectCache.RemoveFromCache(TmpAddressCacheKey + addressChange.SubscriptionId);
                }

                return GetResponseMessage(changeSuccess);
            }
            catch (Exception exception)
            {
                _logService.Log(exception, "Failed to GetTemporaryAddressChanges.", LogLevel.Error, typeof(SubscriptionController));
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> DeleteTemporaryAddressChanges(TemporaryAddressItem addressChange)
        {
            var subscriber = _subscriberFacade.GetSubscriberFromSession();
            var subscriptionNumber = addressChange.SubscriptionId;

            if (!VerifyDomain() || !VerifySubscriber(subscriber, subscriptionNumber))
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }

            var customerNumber = subscriber.SelectedSubscription.CustomerNumber;

            try
            {
                var subscription = GetSubscriptionItem(subscriber, subscriptionNumber);

                var futureTemporaryAddresses = await GetFutureTempAddresses(subscriber, subscription);
                var selectedAddress = futureTemporaryAddresses.SingleOrDefault(x => x.Id == addressChange.Id); 

                if (selectedAddress == null)
                {
                    return GetResponseMessage(false);
                }
                
                var externalNumber = GetLatestGenerationNumber(subscriber, subscription.SubscriptionNumber);

                bool deleteSuccess;

                if (MyPage.BnApiDeleteTemporaryAddress) 
                {
                    var apiResult = await _subscriptionApi.TemporaryAddress.DeleteTemporaryAddressChangeAsync(
                        Brand,
                        customerNumber,
                        subscriptionNumber,
                        externalNumber,
                        selectedAddress.StartDate,
                        selectedAddress.EndDate);

                    deleteSuccess = apiResult.Result == "success";
                }
                else
                {                    
                    var result = DeleteTemporaryAddress(customerNumber, subscriptionNumber, externalNumber, selectedAddress.StartDate);

                    deleteSuccess = result == "OK";
                }
                
                if (deleteSuccess || externalNumber <= 0)
                {
                    _objectCache.RemoveFromCache(TmpAddressCacheKey + addressChange.SubscriptionId);
                    return GetResponseMessage(true);
                }
                
                //on fail: retry with topExtno-1. (TopExtno might belong to a later sub generation).  //TODO: But why?? TKM
                externalNumber -= 1;
                deleteSuccess = DeleteTemporaryAddress(customerNumber, subscriptionNumber, externalNumber, selectedAddress.StartDate) == "OK";

                if (deleteSuccess)
                {
                    _objectCache.RemoveFromCache(TmpAddressCacheKey + addressChange.SubscriptionId);
                }

                return GetResponseMessage(deleteSuccess);
            }
            catch (Exception exception)
            {
                _logService.Log(exception, "Failed to DeleteTemporaryAddressChanges for cusno: " + customerNumber + ", TemporaryAddressItem: " + addressChange.ToLogString(), LogLevel.Error, typeof(SubscriptionController));
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> SaveTemporaryAddressChanges(AddressChangeItem addressChange)
        {
            var subscriber = _subscriberFacade.GetSubscriberFromSession();
            var subscriptionNumber = addressChange.SubscriptionId;            

            if (!VerifyDomain() || !VerifySubscriber(subscriber, subscriptionNumber))
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }

            var customerNumber = subscriber.SelectedSubscription.CustomerNumber;

            try
            {
                var subscription = GetSubscriptionItem(subscriber, subscriptionNumber);

                if (subscription.IsDigitalSubscription)
                    return GetResponseMessage(false);

                var message = await ValidateTmpAddressChange(subscriber, subscription, addressChange);

                if (!string.IsNullOrEmpty(message))
                {
                    return GetResponseMessage(false);
                }

                if (MyPage.BnApiCreateTemporaryAddress)
                {
                    var apiResult = await _subscriptionApi.TemporaryAddress.CreateTemporaryAddressChange(
                        Brand,
                        customerNumber,
                        subscription.SubscriptionNumber,
                        GetLatestGenerationNumber(subscriber, subscription.SubscriptionNumber),
                        addressChange.StreetAddress,
                        addressChange.StreetNo,
                        addressChange.Zip,
                        addressChange.City,
                        addressChange.StairCase,
                        addressChange.Co,
                        addressChange.Stairs,
                        addressChange.ApartmentNumber,
                        addressChange.FromDate,
                        addressChange.ToDate);

                    if (apiResult.Result != "success")
                    {
                        _logService.Log("SaveTemporaryAddressChanges failed using Bn Api, message: " + apiResult.Message + " for cusno: " + customerNumber + ", AddressChangeItem: " + addressChange.ToLogString(), LogLevel.Error, typeof(SubscriptionController));
                        return GetResponseMessage(false);
                    }
                }
                else
                {
                    var result = _addressHandler.CreateTemporaryAddressChange(
                        customerNumber,
                        subscription.SubscriptionNumber,
                        GetLatestGenerationNumber(subscriber, subscription.SubscriptionNumber),
                        addressChange.FromDate,
                        addressChange.ToDate,
                        addressChange.StreetAddress ?? string.Empty,
                        addressChange.StreetNo ?? string.Empty,
                        addressChange.StairCase ?? string.Empty,
                        string.Empty, //floor
                        addressChange.Stairs + (!string.IsNullOrEmpty(addressChange.Stairs) && !addressChange.Stairs.ToUpper().EndsWith("TR") ? "TR" : string.Empty),
                        addressChange.Co + (!string.IsNullOrEmpty(addressChange.ApartmentNumber) ? " LGH" + addressChange.ApartmentNumber : string.Empty),
                        addressChange.Zip);

                    if (result != "OK")
                    {
                        _logService.Log("SaveTemporaryAddressChanges failed for cusno: " + customerNumber + ", AddressChangeItem: " + addressChange.ToLogString(), LogLevel.Error, typeof(SubscriptionController));
                        return GetResponseMessage(false);
                    }
                }                

                _objectCache.RemoveFromCache(TmpAddressCacheKey + addressChange.SubscriptionId);

                return GetResponseMessage(true);
            }
            catch (Exception exception)
            {
                _logService.Log(exception, "Failed to SaveTemporaryAddressChanges for cusno: " + customerNumber + ", AddressChangeItem: " + addressChange.ToLogString(), LogLevel.Error, typeof(SubscriptionController));
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        #endregion

        #region privates

        private async Task<IEnumerable<ReclaimDay>> GetAvailableDaysToReclaim(long customerNumber, SubscriptionItem subscription, int numberOfDaysBackFromToday)
        {
            var availableDates = new List<DateTime>();

            if (MyPage.BnApiGetPastIssueDates)
            {
                var apiResponse = await _subscriptionApi.Issue.GetPastIssueDatesAsync(
                    Brand,
                    customerNumber,
                    subscription.SubscriptionNumber,
                    5);

                if (apiResponse.Result != "success")
                {
                    return new List<ReclaimDay>();
                }

                return apiResponse.Data
                    .Where(x => x.Date >= DateTime.Now.AddDays(-10))
                    .OrderBy(x => x.Date)
                    .Take(numberOfDaysBackFromToday)
                    .Select(x => GetReclaimDay(x.Date));
            }

            var endDate = DateTime.Now;
            // You get publication days between two dates and for the startdate we go back 10 days to be sure we get some days.
            // If the startdate of the subscription is later that our calculated startdate we use that instead so user cannot reclaim days before the subscription was started
            var startDate = endDate.AddDays(-10);
            if (subscription.StartDate > startDate)
            {
                startDate = subscription.StartDate;
            }
            var issueDates = _publicationDaysHandler.GetPublicationDays(subscription.ProductNumber, startDate, endDate);

            availableDates.AddRange(issueDates.OrderByDescending(d => d.IssueDate).Take(numberOfDaysBackFromToday).Select(d => d.IssueDate));
            return availableDates.OrderBy(d => d.Date).Select(GetReclaimDay);
        }

        protected SubscriptionItem GetSubscriptionItem(Subscriber subscriber, long subscriptionNumber)
        {
            return subscriber.SelectedSubscription.Type == SubscriptionType.Dummy
                ? subscriber.SelectedSubscription.SubscriptionItems.First()
                : subscriber.SelectedSubscription.SubscriptionItems.FirstOrDefault(subscriptionItem => subscriptionItem.SubscriptionNumber == subscriptionNumber);
        }

        private IEnumerable<ReclaimItem> GetReclaimItems(IReadOnlyDictionary<int, string> filteredItemsDictionary)
        {
            var reclaimTypes = _reclaimHandler.GetReclaimTypes();

            return reclaimTypes
                .Where(reclaimType => filteredItemsDictionary.ContainsKey(reclaimType.Id))
                .Select(reclaimType => new ReclaimItem(filteredItemsDictionary[reclaimType.Id], reclaimType.Id.ToString()));

        }

        /// <summary>
        /// Gets a dictionary containing the id of all reclaimItems that should be visible in the UI, with related texts.
        /// </summary>
        /// <returns>A dictionary with reclaim item keys and text.</returns>
        public Dictionary<int, string> GetReclaimTypeFilterForUiDictionary()
        {
            return new Dictionary<int, string>
            {
                {890, "Utebliven tidning"},
                {901, "Sent levererad tidning"},
                {896, "Fel tidning levererad"},
                {914, "Bilaga saknas"},
                {911, "Blöt tidning"},
                {912, "Trasig tidning"}
            };
        }

        public ReclaimDay GetReclaimDay(DateTime date)
        {
            return new ReclaimDay
            {
                Date = date,
                DateFriendly = date.ToString("dddd d MMMM yyyy", new CultureInfo("sv-SE"))
            };
        }

        private async Task<string> ValidateSubscriptionSleepAsync(long customerNumber, SubscriptionItem subscriptionItem, SubscriptionSleepItem subscriptionSleep)
        {
            //// Do not check startdate if editing an ongoing sleep
            //if (string.IsNullOrEmpty(postedForm.SubscriptionSleepId) && postedForm.FromDate < postedForm.DateMinAddrChange)
            //{
            //    return "Tidigaste möjliga fråndatum är " + postedForm.DateMinAddrChange.ToString("yyyy-MM-dd") + ".<br>Var god välj ett senare datum.";
            //}

            if (subscriptionSleep.ToDate != DateTime.MinValue)
            {

                if (subscriptionSleep.FromDate >= subscriptionSleep.ToDate)
                {
                    return "Fråndatum måste vara ett tidigare datum än tilldatum.<br>Var god försök igen.";
                }

                var difference = subscriptionSleep.ToDate - subscriptionSleep.FromDate;
                if (difference.Days > 135)
                {
                    return "Uppehållet får max vara 135 dagar långt.<br>Var god försök igen.";
                }
            }

            List<HolidayStopItem> sleepingSubscriptions;

            if (MyPage.BnApiGetHolidayStop)
            {
                var sleepsList = await GetFutureSubscriptionSleepsAsync(customerNumber, subscriptionItem);
                sleepingSubscriptions = sleepsList.ToList();
            }
            else
            {
                sleepingSubscriptions = GetFutureSubscriptionSleeps(subscriptionItem).ToList();
            }

            if (!IsDateSpanValid(sleepingSubscriptions, subscriptionSleep))
            {
                return "Datumintervallet kolliderar med tidigare sparat uppehåll.<br>Var god försök igen.";
            }

            return string.Empty;
        }

        private async Task<string> ValidateTmpAddressChange(Subscriber subscriber, SubscriptionItem subscription, AddressChangeItem addressForm, DateTime? orgFromDate = null, DateTime? orgToDate = null)
        {
            //var closestIssueDate = _issueDateHandler.Retriever.GetNextIssuedate(subscription.PaperCode, subscription.ProductNumber, DateTime.Now);
            DateTime closestIssueDate;
            if (MyPage.BnApiGetSubscriptions)
            {
                closestIssueDate = subscription.ClosestIssueDate; //todo: kj funkar det här?
            }
            else
            {
                closestIssueDate = _issueDateHandler.Retriever.GetNextIssuedate(subscription.PaperCode, subscription.ProductNumber, DateTime.Now);
            }

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

            var difference = addressForm.ToDate - addressForm.FromDate;
            if (difference.Days > 135)
            {
                return "Adressändringen får max vara 135 dagar långt.<br>Var god försök igen.";
            }

            var futureTemporaryAddresses = await GetFutureTempAddresses(subscriber, subscription);

            if (!DateSpanOk(addressForm, futureTemporaryAddresses, orgFromDate, orgToDate))
            {
                return "Datumintervallet kolliderar med tidigare sparad adressändring.<br>Var god försök igen";
            }

            return string.Empty;
        }

        private bool DateSpanOk(AddressChangeItem addressForm, IEnumerable<AddressChange> futureTempAddresses, DateTime? orgFromDate = null, DateTime? orgToDate = null)
        {
            foreach (var old in futureTempAddresses)
            {
                if (orgFromDate != null && orgToDate != null && old.StartDate == orgFromDate && old.EndDate == orgToDate)
                    continue;

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

        private bool IsDateSpanValid(IEnumerable<HolidayStopItem> futureSubsSleeps, SubscriptionSleepItem subscriptionSleep)
        {
            foreach (var futureSubSleep in futureSubsSleeps.Where(futureSubSleep =>
                string.IsNullOrEmpty(subscriptionSleep.Id) || futureSubSleep.Id != subscriptionSleep.Id))
            {
                //dateStart in old interval
                if (subscriptionSleep.FromDate >= futureSubSleep.StartDate && subscriptionSleep.FromDate <= futureSubSleep.EndDate)
                    return false;

                //dateEnd in old interval
                if (subscriptionSleep.ToDate >= futureSubSleep.StartDate && subscriptionSleep.ToDate <= futureSubSleep.EndDate)
                    return false;

                //overlapping entire old interval
                if (subscriptionSleep.FromDate < futureSubSleep.StartDate && subscriptionSleep.ToDate > futureSubSleep.EndDate)
                    return false;
            }

            return true;
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
        #endregion
    }

    public class TemporaryAddressItem : Loggable
    {
        public string Id { get; set; }
        public long SubscriptionId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime FromDateOrg { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime ToDateOrg { get; set; }
    }

    public class SubscriptionSleepItem : Loggable
    {
        public string Id { get; set; }
        public long SubscriptionId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime FromDateOrg { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime ToDateOrg { get; set; }
    }

    public class Reclaim : Loggable
    {
        public long SubscriptionId { get; set; }
        public string[] DaysToReclaim { get; set; }       
        public string Reason { get; set; }
    }

    public class ReclaimDay
    {
        public DateTime Date { get; set; }
        public string DateFriendly { get; set; }
        public bool Disabled { get; set; }
    }

    public class CustomerReclaim
    {
        public string Id { get; set; }
        public string ReclaimText { get; set; }
        public DateTime ReclaimDate { get; set; }
    }

}
