using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Di.Common.Logging;
using Di.Subscription.Logic.Customer;
using Di.Subscription.Logic.Subscription;
using Pren.Web.Business.Configuration;

namespace Pren.Web.Business.Subscription
{
    public interface ISubscriptionChecker
    {
        bool DenySubscriptionForPriceGroup(string priceGroup, string email, int months);
        bool DenySubscriptionOfSameType(string packageId, string email);
    }

    public class SubscriptionChecker : ISubscriptionChecker
    {
        private const int TimeoutInSeconds = 5;

        private readonly ICustomerHandler _customerHandler;
        private readonly ISubscriptionHandler _subscriptionHandler;
        private readonly ISiteSettings _siteSettings;
        private readonly ILogger _logService;

        public SubscriptionChecker(ISubscriptionHandler subscriptionHandler, ICustomerHandler customerHandler, ISiteSettings siteSettings, ILogger logService)
        {
            _subscriptionHandler = subscriptionHandler;
            _customerHandler = customerHandler;
            _siteSettings = siteSettings;
            _logService = logService;
        }

        /// <summary>
        /// Function will check for customers with same <see cref="email"/> and check for subscriptions with <see cref="priceGroup"/> within <see cref="months"/> back in time.
        /// </summary>
        /// <returns>If customer had a suscription within given criterias, return true.</returns>
        public bool DenySubscriptionForPriceGroup(string priceGroup, string email, int months)
        {
            var subscriptions = new List<Di.Subscription.Logic.Subscription.Types.Subscription>();

            var task = Task.Factory.StartNew(
                () =>
                {
                    try
                    {
                        var customerMatches = FindCustomerByEmail(email);

                        foreach (var customerMatch in customerMatches)
                        {
                            subscriptions.AddRange(_subscriptionHandler.GetSubscriptions(customerMatch,
                                DateTime.Now.AddMonths(-months)).
                                Where(t => t.PriceGroup == priceGroup && t.EndDate > DateTime.Now.AddMonths(-months)));
                        }
                    }
                    catch (Exception exception)
                    {
                        _logService.Log(exception, "DenySubscriptionForPriceGroup - failed", LogLevel.Error, typeof(SubscriptionChecker));         
                    }
                });

            if (!task.Wait(TimeSpan.FromSeconds(TimeoutInSeconds)))
            {
                return false;
            }            
            
            return subscriptions.Count > 0;
        }

        /// <summary>
        /// Function will check for customers with same <see cref="email"/> and check for active subscriptions with same <see cref="packageId"/> and not tidsbestämda
        /// </summary>
        /// <returns>If customer has a suscription within given criterias, return true.</returns>
        public bool DenySubscriptionOfSameType(string packageId, string email)
        {
            var subscriptions = new List<Di.Subscription.Logic.Subscription.Types.Subscription>();

            var task = Task.Factory.StartNew(
                () =>
                {
                    try
                    {
                        var customerNumbers = FindCustomerByEmail(email);

                        foreach (var customerNumber in customerNumbers)
                        {
                            subscriptions.AddRange(_subscriptionHandler.GetSubscriptions(customerNumber, DateTime.Now.AddMonths(-12))
                                .Where(t => t.PackageId == packageId) // Same kind
                                .Where(t => t.SubscriptionKind != _siteSettings.SubsKindTimed) // Not tidsbestämd, SubsKind_tidsbestamd = "02";
                                .Where(t => _siteSettings.SubsStateActiveValues.Contains(t.SubscriptionState))); // Is active, subsstate "00", "01", "02", "30"
                        }
                    }
                    catch (Exception exception)
                    {
                        _logService.Log(exception, "DenySubscriptionOfSameType - failed", LogLevel.Error, typeof(SubscriptionChecker));
                    }

                });

            if (!task.Wait(TimeSpan.FromSeconds(TimeoutInSeconds)))
            {
                return false;
            }     

            return subscriptions.Count > 0;
        }

        private IEnumerable<long> FindCustomerByEmail(string email)
        {
            return _customerHandler.FindCustomerNumbersByEmail(email).Distinct();
        }

    }
}
