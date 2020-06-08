using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bn.Subscription;
using Di.Common.Logging;
using Di.Subscription.Logic.Customer;
using Di.Subscription.Logic.Subscription;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pren.Web.Business.Configuration;
using Pren.Web.Business.ServicePlus.Logic;
using Pren.Web.Business.Subscription;
using Pren.Web.UnitTests.Fakes;
using DiSubscription = Di.Subscription.Logic.Subscription.Types.Subscription;

namespace Pren.Web.UnitTests.Business.Subscription
{
    [TestClass]
    public class SubscriberFacadeUnitTest
    {
        public DateTime ExpectedEndDate = DateTime.Now.AddMonths(12);

        private SubscriberFacade GetSubscriberFacade(IServicePlusFacade servicePlusFacade = null, ISubscriptionHandler subscriptionHandler = null, ISiteSettings siteSettings = null, ILogger logger = null, ICustomerHandler customerHandler = null)
        {
            return new SubscriberFacade(
                servicePlusFacade ?? FakeService.GetFakeServicePlusFacade().Object,
                subscriptionHandler ?? GetFakeSubscriptionHandlerWithSubscriptions().Object,
                siteSettings ?? GetFakeSiteSettingWithDefaultValues().Object,
                logger ?? FakeService.GetFakeLogService().Object,
                customerHandler ?? FakeService.GetFakeCustomerHandler().Object,
                FakeService.GetFakeDetectionHandler().Object,
                FakeService.GetFakeSessionData().Object,
                new Mock<ISubscriptionApi>().Object
                );
        }

        private Mock<ISiteSettings> GetFakeSiteSettingWithDefaultValues()
        {
            var siteSettingsMock = FakeService.GetFakeSiteSettings();

            siteSettingsMock.Setup(t => t.SubsStateActiveValues).Returns(new List<string> {"00", "01", "02"});
            siteSettingsMock.Setup(t => t.SubsStateRenewal).Returns("30");

            return siteSettingsMock;
        }

        private Mock<ISubscriptionHandler> GetFakeSubscriptionHandlerWithSubscriptions()
        {           
            var subscriptionHandlerMock = FakeService.GetFakeSubscriptionHandler();

            subscriptionHandlerMock.Setup(t => t.GetSubscriptions(It.IsAny<long>(), It.IsAny<DateTime>())).Returns(GetSubscriptions);

            return subscriptionHandlerMock;
        }

        private List<DiSubscription> GetSubscriptions()
        {
            return new List<DiSubscription>
            {
                new DiSubscription
                {
                    StartDate = DateTime.Now.AddMonths(-12),
                    EndDate = DateTime.Now.AddMonths(3),
                    SubscriptionNumber = 12345678,
                    SubscriptionState = "06",
                    ExternalNumber = 1
                },
                new DiSubscription
                {
                    StartDate = DateTime.Now.AddMonths(-12),
                    EndDate = DateTime.Now.AddMonths(3),
                    SubscriptionNumber = 12345678,
                    SubscriptionState = "02",
                    ExternalNumber = 2
                },
                new DiSubscription
                {
                    StartDate = DateTime.Now.AddMonths(-12),
                    EndDate = ExpectedEndDate,
                    SubscriptionNumber = 12345678,
                    SubscriptionState = "30",
                    ExternalNumber = 3
                },
                new DiSubscription
                {
                    StartDate = DateTime.Now.AddMonths(-12),
                    EndDate = DateTime.Now.AddMonths(3),
                    SubscriptionNumber = 1234567,
                    SubscriptionState = "01",
                    ExternalNumber = 1
                },
                new DiSubscription
                {
                    StartDate = DateTime.Now.AddMonths(3),
                    EndDate = ExpectedEndDate,
                    SubscriptionNumber = 1234567,
                    SubscriptionState = "30",
                    ExternalNumber = 3
                },
                new DiSubscription
                {
                    StartDate = DateTime.Now.AddMonths(3),
                    EndDate = DateTime.Now.AddMonths(6),
                    SubscriptionNumber = 1234567,
                    SubscriptionState = "30",
                    ExternalNumber = 2
                }               
            };
        }

        [TestMethod]
        public async Task GetSubscriptionItems_HasRenewals_ReturnOnlyActiveSubscriptionItems()
        {
            var subscriberFacade = GetSubscriberFacade();

            var subscriptionItems = await subscriberFacade.GetSubscriptionItems(It.IsAny<long>());

            // Only active items should be returned
            Assert.AreEqual(subscriptionItems.Count, 2);
        }

        [TestMethod]
        public async Task GetSubscriptionItems_HasRenewals_ReturnItemWithLatestRenewalsEndDate()
        {
            var subscriberFacade = GetSubscriberFacade();

            var subscriptionItems = await subscriberFacade.GetSubscriptionItems(It.IsAny<long>());

            // EndDate on subscriptionItem should be the enddate of the latest matching renewal
            Assert.IsTrue(subscriptionItems.All(t => t.EndDate.Equals(ExpectedEndDate)));
        }
    }
}
