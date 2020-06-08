using System;
using System.Collections.Generic;
using Di.Subscription.Logic.Customer.Types;
using Pren.Web.Business.ServicePlus.Models;
using Pren.Web.Business.Subscription;
using SubscriberObject = Pren.Web.Business.Subscription.Subscriber;

namespace Pren.Web.UnitTests.Fakes.Subscription
{
    static class SubscriberExtensions
    {
        public static SubscriberObject WithSelectedSubcription(this SubscriberObject subscriber, UserSubscription selectedSubscription = null)
        {
            subscriber.SelectedSubscription = selectedSubscription ?? new UserSubscription();

            return subscriber;
        }

        public static SubscriberObject WithServicePlusUser(this SubscriberObject subscriber, User servicePlusUser = null)
        {
            subscriber.ServicePlusUser = servicePlusUser ?? new User();

            return subscriber;
        }
    }

    static class UserSubscriptionExtensions
    {
        public static UserSubscription WithKayakCustomer(this UserSubscription subscription, Customer customer = null)
        {
            subscription.KayakCustomer = customer ?? new Customer();

            return subscription;
        }

        public static UserSubscription WithSubscriptionItems(this UserSubscription subscription, List<SubscriptionItem> subscriptionItems = null)
        {
            subscription.SubscriptionItems = subscriptionItems ?? new List<SubscriptionItem>()
            {
                new SubscriptionItem()
                {
                    EndDate = DateTime.Now.AddMonths(3),
                    GenerationNumber = 0,
                    IsDigitalSubscription = true,
                    PaperCode = "DI",
                    ProductName = "Di test",
                    ProductNumber = "01",
                    StartDate = DateTime.Now.AddMonths(-1),
                    SubscriptionNumber = 123456789
                }
            };

            return subscription;
        }

        
    }
}
