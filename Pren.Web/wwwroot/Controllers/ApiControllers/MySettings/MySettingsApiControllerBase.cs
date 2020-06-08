using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using Bn.Subscription;
using Di.Common.Logging;
using Di.Subscription.Logic.Address;
using Di.Subscription.Logic.Address.Types;
using Di.Subscription.Logic.HolidayStop;
using Di.Subscription.Logic.HolidayStop.Types;
using EPiServer;
using EPiServer.Core;
using EPiServer.Framework.Cache;
using EPiServer.ServiceLocation;
using Pren.Web.Business.Cache;
using Pren.Web.Business.Configuration;
using Pren.Web.Business.Controllers;
using Pren.Web.Business.Mail;
using Pren.Web.Business.Subscription;
using Pren.Web.Models.Pages;
using Pren.Web.Models.Pages.MySettings.Di;

namespace Pren.Web.Controllers.ApiControllers.MySettings
{
    public class MySettingsApiControllerBase : ExtendedApiController
    {
        protected const string SubsSleepCacheKey = "MyPage_Subssleep_";
        protected const string TmpAddressCacheKey = "MyPage_TmpAddress";
        protected const string PermAddressCacheKey = "MyPage_PermAddress";

        protected const string Brand = "di";

        private readonly IHolidayStopHandler _holidayStopHandler;
        private readonly IAddressHandler _addressHandler;
        private readonly IMailHandler _mailHandler;
        private readonly ISiteSettings _siteSettings;
        private readonly IObjectCache _objectCache;
        private readonly ISubscriptionApi _subscriptionApi;

        protected readonly MyStartPage MyPage;

        public MySettingsApiControllerBase(
            IApiReferrerCheck apiReferrerCheck, 
            IHolidayStopHandler holidayStopHandler, 
            IAddressHandler addressHandler, 
            IMailHandler mailHandler, 
            ISiteSettings siteSettings, 
            IObjectCache objectCache, 
            ISubscriptionApi subscriptionApi) : base(apiReferrerCheck)
        {
            _holidayStopHandler = holidayStopHandler;
            _addressHandler = addressHandler;
            _mailHandler = mailHandler;
            _siteSettings = siteSettings;
            _objectCache = objectCache;
            _subscriptionApi = subscriptionApi;

            var contentRepo = ServiceLocator.Current.GetInstance<IContentRepository>();
            var startPage = contentRepo.Get<StartPage>(ContentReference.StartPage);
            MyPage = contentRepo.Get<MyStartPage>(startPage.MySettingsStartPage);
        }

        public HttpResponseMessage GetResponseMessage<T>(T response)
        {
            return new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new ObjectContent<T>(response, new JsonMediaTypeFormatter())
            };
        }

        public string GetAddressAsHtml(Subscriber subscriber, AddressChangeItem addressForm)
        {
            var sb = new StringBuilder();

            //tidigare skrevs rowtext2 ut efter rowtext1 men nu ligger den logiken i CustomerRetriever och ser lite annorlunda ut, bra att veta om vi får support på detta 
            sb.Append(subscriber.SelectedSubscription.KayakCustomer.FullName);

            sb.Append("<br>");

            if (!string.IsNullOrEmpty(addressForm.Co))
                sb.Append("C/O " + addressForm.Co.ToUpper() + "<br>");

            sb.Append(addressForm.StreetAddress.ToUpper());

            if (!string.IsNullOrEmpty(addressForm.StreetNo))
                sb.Append(" " + addressForm.StreetNo);

            if (!string.IsNullOrEmpty(addressForm.StairCase))
                sb.Append(" " + addressForm.StairCase.ToUpper());

            if (!string.IsNullOrEmpty(addressForm.Stairs))
                sb.Append(" " + addressForm.Stairs + "TR");

            if (!string.IsNullOrEmpty(addressForm.ApartmentNumber))
                sb.Append(" LGH" + addressForm.ApartmentNumber);

            sb.Append("<br>");

            sb.Append(addressForm.Zip + " " + addressForm.City.ToUpper());

            return sb.ToString();
        }

        public void SendCustMail(string emailAddress, string mailBody, Dictionary<string, string> replaceDictionary, string subject)
        {
            mailBody = _mailHandler.ReplaceMailPlaceHolders(replaceDictionary, mailBody);

            _mailHandler.SendMail(
                _siteSettings.MailNoReplyAddress,
                emailAddress,
                subject,
                mailBody,
                true);
        }

        public IEnumerable<HolidayStopItem> GetFutureSubscriptionSleeps(SubscriptionItem subscription)
        {
            var cacheKey = SubsSleepCacheKey + subscription.SubscriptionNumber;

            var subsSleeps = (IEnumerable<HolidayStopItem>)_objectCache.GetFromCache(cacheKey);

            if (subsSleeps != null)
            {
                return subsSleeps;
            }
            
            var sleepingSubscriptions = _holidayStopHandler.GetHolidayStops(subscription.SubscriptionNumber, subscription.GenerationNumber)
                .Where(t => t.EndDate >= DateTime.Now)
                .OrderBy(t => t.StartDate);

            _objectCache.AddToCache(cacheKey, sleepingSubscriptions, new CacheEvictionPolicy(new TimeSpan(1, 0, 0), CacheTimeoutType.Absolute));

            return sleepingSubscriptions;
        }

        public async Task<IEnumerable<HolidayStopItem>> GetFutureSubscriptionSleepsAsync(long customerNumber, SubscriptionItem subscription)
        {
            var cacheKey = SubsSleepCacheKey + subscription.SubscriptionNumber;

            var subsSleeps = (IEnumerable<HolidayStopItem>)_objectCache.GetFromCache(cacheKey);

            if (subsSleeps != null)
            {
                return subsSleeps;
            }

            var apiResult = await _subscriptionApi.HolidayStop.GetHolidayStopsAsync(
                Brand,
                customerNumber,
                subscription.SubscriptionNumber,
                subscription.GenerationNumber);

            if (apiResult.Result != "success")
            {
                return null;
            }

            var sleepingSubscriptions = apiResult.Data
                .Where(x => x.StopDate >= DateTime.Now)
                .OrderBy(x => x.StartDate)
                .Select(x => new HolidayStopItem
                {
                    StartDate = x.StartDate,
                    EndDate = x.StopDate,
                    Id = subscription.SubscriptionNumber + "_" + x.StartDate + "_" + x.StopDate
                }).ToList();


            _objectCache.AddToCache(cacheKey, sleepingSubscriptions, new CacheEvictionPolicy(new TimeSpan(1, 0, 0), CacheTimeoutType.Absolute));

            return sleepingSubscriptions;
        }

        public async Task<IEnumerable<AddressChange>> GetFutureTempAddresses(Subscriber subscriber, SubscriptionItem subscription)
        {
            var cacheKey = TmpAddressCacheKey + subscription.SubscriptionNumber;

            var tmpAddresses = (IEnumerable<AddressChange>)_objectCache.GetFromCache(cacheKey);

            if (tmpAddresses != null)
            {
                return tmpAddresses;
            }

            var temporaryAddresses = new List<AddressChange>();

            if (MyPage.BnApiGetTemporaryAddress)
            {
                var apiResult = await _subscriptionApi.TemporaryAddress.GetTemporaryAddresses(
                    Brand,
                    subscriber.SelectedSubscription.CustomerNumber,
                    subscription.SubscriptionNumber);

                if (apiResult.Result != "success")
                {
                    return temporaryAddresses;
                }

                temporaryAddresses = apiResult.Data.Select(x => new AddressChange
                {
                    Id = x.Address.Pointer,
                    StreetAddress = x.Address.StreetName,
                    StreetNumber = x.Address.StreetNumber,
                    StairCase = x.Address.StairCase,
                    Apartment = x.Address.ApartmentNumber,
                    Zip = x.Address.Zip,
                    City = x.Address.City,
                    StartDate = x.StartDate,
                    EndDate = x.StopDate
                }).ToList();
            }
            else
            {
                temporaryAddresses = _addressHandler.GetTemporaryAddressChanges(
                    subscriber.SelectedSubscription.CustomerNumber,
                    subscription.SubscriptionNumber).OrderBy(t => t.StartDate).ToList();
            }

            _objectCache.AddToCache(cacheKey, temporaryAddresses, new CacheEvictionPolicy(new TimeSpan(1, 0, 0), CacheTimeoutType.Absolute));

            return temporaryAddresses;
        }

        public async Task<IEnumerable<AddressChange>> GetFuturePermAddresses(Subscriber subscriber)
        {
            var cacheKey = PermAddressCacheKey + subscriber.SelectedSubscription.CustomerNumber;

            var permAddresses = (IEnumerable<AddressChange>)_objectCache.GetFromCache(cacheKey);

            if (permAddresses != null)
            {
                return permAddresses;
            }

            var addressChanges = new List<AddressChange>();

            if (MyPage.BnApiGetPermanentAddress)
            {
                var apiResult = await _subscriptionApi.PermanentAddress.GetPermanenAddresses(Brand, subscriber.SelectedSubscription.CustomerNumber);

                if (apiResult.Result != "success")
                {
                    return permAddresses;
                }

                addressChanges.AddRange(apiResult.Data.Select(x => new AddressChange
                {
                    Id = x.Address.Pointer,
                    StreetAddress = x.Address.StreetName,
                    StreetNumber = x.Address.StreetNumber,
                    StairCase = x.Address.StairCase,
                    Apartment = x.Address.ApartmentNumber,
                    Zip = x.Address.Zip,
                    City = x.Address.City,
                    StartDate = x.StartDate,
                    EndDate = x.StopDate
                }));

                //todo: kj filtreringarna nedan - behövs dom när vi hämtar från apiet?
            }
            else
            {
                var activeSubscriptions = subscriber.SelectedSubscription.SubscriptionItems;

                if (activeSubscriptions.Count > 0)
                {
                    addressChanges.AddRange(_addressHandler.GetPermanentAddressChanges(subscriber.SelectedSubscription.CustomerNumber, activeSubscriptions[0].SubscriptionNumber));
                }

                addressChanges = addressChanges.Where(t => t.StartDate > DateTime.Now.Date).ToList();
            }

            _objectCache.AddToCache(cacheKey, addressChanges, new CacheEvictionPolicy(new TimeSpan(1, 0, 0), CacheTimeoutType.Absolute));

            return addressChanges;
        }
    }

    public class AddressChangeItem : Loggable
    {
        public long SubscriptionId { get; set; }
        public string Co { get; set; }
        public string StreetAddress { get; set; }
        public string StreetNo { get; set; }
        public string StairCase { get; set; }
        public string Stairs { get; set; }
        public string ApartmentNumber { get; set; }
        public string Zip { get; set; }
        public string City { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}