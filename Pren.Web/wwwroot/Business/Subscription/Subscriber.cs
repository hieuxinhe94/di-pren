using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Bn.Subscription;
using Di.Common.Logging;
using Di.Subscription.Logic.Customer;
using Di.Subscription.Logic.Customer.Types;
using Di.Subscription.Logic.Subscription;
using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using Pren.Web.Business.Configuration;
using Pren.Web.Business.Detection;
using Pren.Web.Business.ServicePlus.Logic;
using Pren.Web.Business.ServicePlus.Models;
using Pren.Web.Business.Session;
using Pren.Web.Models.Pages;
using Pren.Web.Models.Pages.MySettings.Di;

namespace Pren.Web.Business.Subscription
{
    public interface ISubscriberFacade
    {
        Subscriber GetSubscriberFromSession();
        Task<Subscriber> GetSubscriber(string token);
        Task<Subscriber> GetSubscriberByCode(string code, string token);
        Task<List<SubscriptionItem>> GetSubscriptionItems(long customerNumber);
    }

    public class SubscriberFacade : ISubscriberFacade
    {
        private readonly IServicePlusFacade _servicePlusFacade;
        private readonly ISubscriptionHandler _subscriptionHandler;
        private readonly ISiteSettings _siteSettings;
        private readonly ILogger _logger;
        private readonly ICustomerHandler _customerHandler;
        private readonly ISessionData _sessionData;
        private readonly IDetectionHandler _detection;
        private readonly ISubscriptionApi _subscriptionApi;

        public SubscriberFacade(
            IServicePlusFacade servicePlusFacade, 
            ISubscriptionHandler subscriptionHandler, 
            ISiteSettings siteSettings, 
            ILogger logger, 
            ICustomerHandler customerHandler, 
            IDetectionHandler detection, 
            ISessionData sessionData, 
            ISubscriptionApi subscriptionApi)
        {
            _servicePlusFacade = servicePlusFacade;
            _subscriptionHandler = subscriptionHandler;
            _siteSettings = siteSettings;
            _logger = logger;
            _customerHandler = customerHandler;
            _detection = detection;
            _sessionData = sessionData;
            _subscriptionApi = subscriptionApi;
        }

        /// <summary>
        /// Get Subscriber object saved in session. If in edit-mode, a dummy subscriber will be returned
        /// </summary>
        /// <returns>A Subscriber object</returns>
        public Subscriber GetSubscriberFromSession()
        {
            if (_detection.IsInEditMode())
            {
                return new SubscriberDummy();
            }

            return (Subscriber)_sessionData.Get(SessionConstants.SubscriberSessionKey);
        }

        /// <summary>
        /// Gets Subscriber object containing business and private subscriptions
        /// </summary>
        /// <param name="token">S+ token for user to get Subscriber object for</param>
        /// <returns>A Subscriber object containing business and private subscriptions</returns>
        public async Task<Subscriber> GetSubscriber(string token)
        {
            var subscriber = new Subscriber();

            subscriber = SetServicePlusUser(subscriber, token);

            if (subscriber.ServicePlusUser == null)
            {
                return subscriber;
            }

            // Check if user has Business Subscription
            var hasBusinessSubscription = _servicePlusFacade.HasBizSubscription(subscriber.ServicePlusUser.Id);

            if (hasBusinessSubscription)
            {
                subscriber.BusinessSubscription = await GetBusinessSubscription(subscriber);
            }

            // Only get private subscriptions if we don't have a pending biz subscription.
            // This is because it is possible that the business subscriptions processing is finished and we get the business subscription as a private because 
            // KayakBizSubscriptionCustomerNumber is 0 and business subscription entitlement is not filtered
            if (subscriber.BusinessSubscription == null || !subscriber.BusinessSubscription.IsPending)
            {
                subscriber.PrivateSubscription = await GetPrivateSubscription(token, subscriber);
            }
            
            //todo: behöver vi plocka ut pending subscriptions?

            //todo: enbart injuden företagsportal pren

            // Set selected subscription 
            subscriber.SelectedSubscription = GetSelectedSubscription(subscriber);

            return subscriber;
        }

        /// <summary>
        /// Gets Subscriber object containing private subscriptions
        /// </summary>
        /// <param name="code">Code from url, ECustomerNumber</param>
        /// <param name="token">S+ token for user to get Subscriber object for</param>
        /// <returns>A Subscriber object containing private subscriptions</returns>
        public async Task<Subscriber> GetSubscriberByCode(string code, string token)
        {
            var subscriber = new Subscriber();                        

            long eCustomerNumber;

            if (string.IsNullOrEmpty(code) || !long.TryParse(code, out eCustomerNumber))
            {
                return subscriber;
            }

            var customerNumber = _customerHandler.GetCustomerNumberByEcusno(eCustomerNumber);

            if (customerNumber < 1)
            {
                return subscriber;
            }

            subscriber = SetServicePlusUser(subscriber, token);

            // Get private subscription based on entitlements in ServicePlus
            var privateSubscripion = subscriber.ServicePlusUser != null ? await GetPrivateSubscription(token, subscriber, customerNumber) : null;

            if (privateSubscripion != null)
            {
                // If subscription not connected, customer number from code and from token don't match
                if (!privateSubscripion.IsConnected)
                    return subscriber;
            }
            else
            {
                privateSubscripion = new UserSubscription
                    {
                        CustomerNumber = customerNumber,
                        SubscriptionItems = await GetSubscriptionItems(customerNumber),
                        Type = SubscriptionType.Private,
                        KayakCustomer = _customerHandler.GetCustomer(customerNumber),
                    };
            }

            privateSubscripion.ECustomerNumber = eCustomerNumber;

            subscriber.PrivateSubscription = privateSubscripion;

            // Set selected subscription 
            subscriber.SelectedSubscription = GetSelectedSubscription(subscriber);            

            return subscriber;
        }

        /// <summary>
        /// Get UserSubscription object with business subscriptions
        /// </summary>
        /// <param name="subscriber">The subscriber to get UserSubscription for</param>
        /// <returns>A UserSubscription object containing business subscriptions</returns>
        private async Task<UserSubscription> GetBusinessSubscription(Subscriber subscriber)
        {
            var businessSubscription = new UserSubscription();

            if (subscriber.ServicePlusUser.KayakBizSubscriptionCustomerNumber == 0)
            {
                //We know that user has a BizSubscription but if no KayakBizSubscriptionCustomerNumber it is not yet processed in Kayak - set as pending
                businessSubscription.IsPending = true;
                businessSubscription.Type = SubscriptionType.Business;
                return businessSubscription;
            }

            businessSubscription.CustomerNumber = subscriber.ServicePlusUser.KayakBizSubscriptionCustomerNumber;

            businessSubscription.SubscriptionItems = await GetSubscriptionItems(businessSubscription.CustomerNumber);
            businessSubscription.Type = SubscriptionType.Business;
            businessSubscription.KayakCustomer = _customerHandler.GetCustomer(businessSubscription.CustomerNumber);
            // TODO: stöd för ecusno? /TKM
            businessSubscription.IsConnected = true;

            return businessSubscription;
        }

        /// <summary>
        /// Gets UserSubscription object with private subscriptions
        /// </summary>
        /// <param name="token">The token to get externalIds for</param>
        /// <param name="subscriber">The subscriber to get UserSubscription for</param>
        /// <param name="externalCustomerNumber">
        /// ECustomernumber, if provided and not match with customer number based on token, the IsConnected flag will be set to false.</param>
        /// <returns></returns>
        private async Task<UserSubscription> GetPrivateSubscription(string token, Subscriber subscriber, long? externalCustomerNumber = null)
        {
            // Get External ids from S+ and get private subscription
            var externalIds = _servicePlusFacade.GetExternalIds(token).ToList();

            if (!externalIds.Any()) return null;

            var userSubscription = new UserSubscription();

            try
            {
                var privateSubscription = await GetPrivateSubscription(subscriber, externalIds);
                if (privateSubscription != null)
                {
                    privateSubscription.IsConnected = externalCustomerNumber == null ||
                                                      privateSubscription.CustomerNumber.Equals(externalCustomerNumber);
                }
                
                return privateSubscription;
            }
            catch (AmbiguousMatchException ambiguousMatchException) // This exception is thrown when there is more that one customer number in S+
            {
                userSubscription.HasMultipleCustomerNumbers = true;

                _logger.Log(
                    ambiguousMatchException,
                    $"ServicePlus email: {subscriber.ServicePlusUser.Email}, token: '{token}'",
                    LogLevel.Warn,
                    typeof(SubscriberFacade));

                return userSubscription;
            }
        }

        /// <summary>
        /// Gets UserSubscription object with private subscriptions
        /// </summary>
        /// <param name="subscriber">The subscriber to get UserSubscription for</param>
        /// <param name="externalIds">The users externalIds from S+</param>
        /// <returns>A UserSubscription object containing private subscriptions, null if no private subscription</returns>
        private async Task<UserSubscription> GetPrivateSubscription(Subscriber subscriber, IEnumerable<ExternalId> externalIds)
        {            
            var privateCustomerNumbers = new List<long>();

            foreach (var externalId in externalIds)
            {
                long externalSubscriberId;
                if (!long.TryParse(externalId.ExternalSubscriberId, out externalSubscriberId))
                {
                    continue;
                }

                // Do not add biz subscription customer number to the list
                if (externalSubscriberId == subscriber.ServicePlusUser.KayakBizSubscriptionCustomerNumber)
                {
                    continue;
                }

                privateCustomerNumbers.Add(externalSubscriberId);
            }

            if (privateCustomerNumbers.Count == 0)
            {
                return null;
            }

            var privateSubscripion = new UserSubscription();

            // If customer has more than one private customer number in S+ we don't know which one to use so an exception is thrown
            if (privateCustomerNumbers.Count > 1)
            {
                if (ServiceLocator.Current.GetInstance<IContentRepository>()
                        .Get<StartPage>(ContentReference.StartPage)
                        .CheckEntitlementBeforeAmbiguousMatchException)
                {
                    //If we have multiple customernumbers we do a check on the validity of the entitlements 
                    var entitlements = _servicePlusFacade.GetEntitlements(subscriber.ServicePlusUser.Id);

                    var validEntitlements = entitlements.Where(
                        e =>  e.SubscriberId > 0 && e.SubscriberId.ToString().Length <= 8 &&
                            e.State.Equals(EntitlementState.Valid) &&
                            (e.ValidFrom != null && e.ValidFrom < DateTime.Now) &&
                            (e.ValidTo != null && e.ValidTo > DateTime.Now));

                    var firstValidEntitlement = validEntitlements.OrderByDescending(e => e.Updated).FirstOrDefault();

                    privateCustomerNumbers = privateCustomerNumbers
                        .Where(pc => pc.Equals(firstValidEntitlement.SubscriberId))
                        .Select(x => x).ToList();

                    if (privateCustomerNumbers.Count == 0)
                    {
                        return null;
                    }
                }

                if (privateCustomerNumbers.Count > 1)
                {
                    throw new AmbiguousMatchException($"Customer has more than one private customer number in S+. CustomerNumbers: {privateCustomerNumbers.Aggregate(string.Empty, (current, privateCustomerNumber) => current + " " + privateCustomerNumber)}");
                }
            }

            privateSubscripion.CustomerNumber = privateCustomerNumbers.First();

            var customerProperties = _customerHandler.GetCustomerProperties(privateSubscripion.CustomerNumber);
            privateSubscripion.IsAgreementCustomer = customerProperties.Any(t => t.PropertyCode == "98"); //Avtalskund

            privateSubscripion.SubscriptionItems = await GetSubscriptionItems(privateSubscripion.CustomerNumber);

            if (UseBnApiToGetCustomer())
            {
                var apiResponse = await _subscriptionApi.Customer.GetCustomerAsync("di", privateSubscripion.CustomerNumber);

                if (apiResponse.Result != "success")
                {
                    _logger.Log(
                        $"Failed to get customer from Bn Api. Message: {apiResponse.Message} customerNumber: {privateSubscripion.CustomerNumber}",
                        LogLevel.Error, typeof(SubscriberFacade));
                }
                else
                {
                    var customerFromApi = apiResponse.Data;
                    privateSubscripion.KayakCustomer = new Customer
                    {
                        CustomerNumber = customerFromApi.CustomerNumber,
                        AddressCareOf = customerFromApi.Address.CareOf,
                        AddressCity = customerFromApi.Address.City,
                        AddressStairCase = customerFromApi.Address.StairCase,
                        AddressStairs = customerFromApi.Address.Stairs,
                        AddressStreetName = customerFromApi.Address.StreetName,
                        AddressStreetNumber = customerFromApi.Address.StreetNumber,
                        AddressZip = customerFromApi.Address.Zip,
                        CompanyName = customerFromApi.CompanyName,
                        CompanyNumber = customerFromApi.CompanyNumber,
                        Email = customerFromApi.Email,
                        FirstName = customerFromApi.FirstName,
                        LastName = customerFromApi.LastName,
                        FullName = customerFromApi.FullName,
                        PhoneOffice = customerFromApi.Phone
                    };
                }
            }
            else
            {
                privateSubscripion.KayakCustomer = _customerHandler.GetCustomer(privateSubscripion.CustomerNumber);
            }
            

            privateSubscripion.Type = SubscriptionType.Private;

            return privateSubscripion;
        }

        private bool UseBnApiToGetCustomer()
        {
            try
            {
                var contentRepo = ServiceLocator.Current.GetInstance<IContentRepository>();
                var startPage = contentRepo.Get<StartPage>(ContentReference.StartPage);
                var myPage = contentRepo.Get<MyStartPage>(startPage.MySettingsStartPage);
                return myPage.BnApiGetCustomer;

            }
            catch (Exception)
            {
                return false;
            }
        }


        public async Task<List<SubscriptionItem>> GetSubscriptionItemsAsync(long customerNumber)
        {
            var subs = await _subscriptionApi.Subscription.GetSubscriptionsAsync(
                "di",
                customerNumber,
                false);


            var items = new List<SubscriptionItem>();

            items.AddRange(subs.Data.Select(x => new SubscriptionItem
            {
                SubscriptionNumber = x.SubscriptionNumber,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                IsDigitalSubscription = x.IsDigital,
                ProductName = x.ProductName,
                //PaperCode = subscription.PaperCode,
                //ProductNumber = subscription.ProductNumber,
                GenerationNumber = x.SequenceNumber,
                ClosestIssueDate = x.NextIssueDate
            }));

            return items;            
        }

        /// <summary>
        /// Gets a list of SubscriptionItems for a provided customerNumber
        /// </summary>
        /// <param name="customerNumber">The customerNumber to get SubscriptionItems from</param>
        /// <returns>List of SubscriptionItems for provided customerNumber</returns>
        public async Task<List<SubscriptionItem>> GetSubscriptionItems(long customerNumber)
        {
            if (UseBnApiToGetSubscriptions())
            {
                return await GetSubscriptionItemsAsync(customerNumber);
            }

            var subscriptionItems = new List<SubscriptionItem>();

            var subscriptions = _subscriptionHandler.GetSubscriptions(customerNumber, DateTime.Now).ToList();

            // Add active subscriptions to list
            subscriptionItems.AddRange(subscriptions.Where(subscription => _siteSettings.SubsStateActiveValues.Contains(subscription.SubscriptionState))
                .Select(CreateSubscriptionItem));

            // Get renewal subscriptions
            var renewalItems =
                subscriptions.Where(
                    subscription => subscription.SubscriptionState.Equals(_siteSettings.SubsStateRenewal)).OrderBy(t => t.ExternalNumber);

            // Change enddate on active subscriptions to endate on matching renewal subscriptions
            foreach (var renewalItem in renewalItems)
            {
                subscriptionItems.FindAll(t => t.SubscriptionNumber.Equals(renewalItem.SubscriptionNumber)).ForEach(t => t.EndDate = renewalItem.EndDate);
            }

            return subscriptionItems;
        }

        private bool UseBnApiToGetSubscriptions()
        {
            try
            {
                var contentRepo = ServiceLocator.Current.GetInstance<IContentRepository>();
                var startPage = contentRepo.Get<StartPage>(ContentReference.StartPage);
                var myPage = contentRepo.Get<MyStartPage>(startPage.MySettingsStartPage);
                return myPage.BnApiGetSubscriptions;

            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Create POCO object SubscriptionItem from a Subscription object
        /// </summary>
        /// <param name="subscription">Subscription object</param>
        /// <returns>SubscriptionItem with properties needed on My Settings pages</returns>
        private SubscriptionItem CreateSubscriptionItem(Di.Subscription.Logic.Subscription.Types.Subscription subscription)
        {
            return new SubscriptionItem
            {
                SubscriptionNumber = subscription.SubscriptionNumber,
                StartDate = subscription.StartDate,
                EndDate = subscription.EndDate,
                IsDigitalSubscription = _siteSettings.IsDigitalSub(subscription.PackageId),
                ProductName = _siteSettings.GetProductName(subscription.PackageId),
                PaperCode = subscription.PaperCode,
                ProductNumber = subscription.ProductNumber,
                GenerationNumber = subscription.ExternalNumber
            };
        }

        /// <summary>
        /// Get subscription that should be selected by default
        /// </summary>
        /// <param name="subscriber">Subscriber to get default subscription for</param>
        /// <returns>Private subscription as primary, business subscription as secondary, null if no private or business subcription</returns>
        private UserSubscription GetSelectedSubscription(Subscriber subscriber)
        {
            if (subscriber.PrivateSubscription != null)
            {
                return subscriber.PrivateSubscription;
            }
            if (subscriber.BusinessSubscription != null &&
                ((subscriber.BusinessSubscription.SubscriptionItems != null &&
                subscriber.BusinessSubscription.SubscriptionItems.Any()) ||
                subscriber.BusinessSubscription.IsPending))
            {
                return subscriber.BusinessSubscription;
            }

            return null;
        }

        /// <summary>
        /// Sets servicePlusUser on provided subscriber object
        /// </summary>
        /// <param name="subscriber">The subscriber object on which ServicePlusUser will be set</param>
        /// <param name="token">S+ user token</param>
        private Subscriber SetServicePlusUser(Subscriber subscriber, string token)
        {
            // Get ServicePlus User
            var servicePlusUser = _servicePlusFacade.GetUserByToken(token);

            // If no ServicePlus user we return an empty subscriber object
            if (servicePlusUser == null)
            {
                return subscriber;
            }

            subscriber.ServicePlusToken = token;
            subscriber.ServicePlusUser = servicePlusUser;

            return subscriber;
        }
    }

    public class SubscriberDummy : Subscriber
    {
        public SubscriberDummy()
        {
            ServicePlusUser =
                new User
                {
                    Email = "dummypren@di.se",
                    FirstName = "Dummy",
                    LastName = "Dummy",
                    Id = "Dummy",
                    Phone = "Dummy",
                };

            SelectedSubscription =
                new UserSubscription
                {
                    CustomerNumber = 0,
                    IsConnected = true,
                    Type = SubscriptionType.Dummy,
                    SubscriptionItems = new List<SubscriptionItem> {new SubscriptionItem { SubscriptionNumber = 0, PaperCode = "DI", ProductNumber = "01" }},
                    KayakCustomer = new Customer {FullName = "Dummy", Email = "dummypren@di.se", CustomerNumber = 0}
                };
        }
    }

    public class Subscriber
    {
        public string ServicePlusToken { get; set; }
        public User ServicePlusUser { get; set; }
        public UserSubscription PrivateSubscription { get; set; }
        public UserSubscription BusinessSubscription { get; set; }
        public UserSubscription SelectedSubscription { get; set; }

        /// <summary>
        /// Safe logging of properties on the <see cref="Subscriber" />
        /// </summary>
        /// <returns>Info about the subscriber</returns>
        public string LogInfo()
        {
            try
            {
                var sb = new StringBuilder();
                if (ServicePlusUser == null)
                {
                    sb.AppendLine("ServicePlusUser: '[NULL]'");
                }
                else
                {
                    sb.AppendLine($"ServicePlusUser.Email: '{ServicePlusUser.Email}'");
                }

                if (SelectedSubscription == null)
                {
                    sb.AppendLine("SelectedSubscription: '[NULL]'");
                }
                else
                {
                    sb.AppendLine($"SelectedSubscription.CustomerNumber: '{SelectedSubscription.CustomerNumber}'");
                }

                return sb.ToString();
            }
            catch (Exception)
            {
                return "Could not write loginfo";
            }
        }
    }
    
    public class UserSubscription
    {
        public bool HasMultipleCustomerNumbers { get; set; }
        public long CustomerNumber { get; set; }
        public long ECustomerNumber { get; set; }
        public SubscriptionType Type { get; set; }
        public List<SubscriptionItem> SubscriptionItems { get; set; }
        public Customer KayakCustomer { get; set; }
        public bool IsConnected { get; set; }
        public bool IsPending { get; set; }
        public bool IsAgreementCustomer { get; set; }
    }

    public class SubscriptionItem
    {
        public long SubscriptionNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime ClosestIssueDate { get; set; }    
        public bool IsDigitalSubscription { get; set; }
        public string ProductName { get; set; }
        public string PaperCode { get; set; }
        public string ProductNumber { get; set; }
        public int GenerationNumber { get; set; }
        public bool IsActive
        {
            get { return StartDate <= DateTime.Now; }
        }
    }

    public enum SubscriptionType
    {
        Private,
        Business,
        Dummy
    }
}
