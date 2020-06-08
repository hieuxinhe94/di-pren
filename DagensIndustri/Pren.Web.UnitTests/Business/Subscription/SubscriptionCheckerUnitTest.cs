using System;
using System.Collections.Generic;
using Di.Common.Logging;
using Di.Subscription.Logic.Customer;
using Di.Subscription.Logic.Subscription;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pren.Web.Business.Configuration;
using Pren.Web.Business.Subscription;
using DiSubscription = Di.Subscription.Logic.Subscription.Types.Subscription;

namespace Pren.Web.UnitTests.Business.Subscription
{
    [TestClass]
    public class SubscriptionCheckerUnitTest
    {
        private ISubscriptionChecker GetSubscriptionChecker(
            ISubscriptionHandler subscriptionHandler = null,
            ICustomerHandler customerHandler = null,
            ISiteSettings siteSettings = null,
            ILogger logService = null)
        {
            return new SubscriptionChecker(
                subscriptionHandler ?? GetFakeSubscriptionHandler().Object, 
                customerHandler ?? GetFakeCustomerHandler().Object,
                siteSettings ?? GetFakeSiteSettings().Object,
                logService ?? GetFakeLogService().Object
                );
        }

        private Mock<ISubscriptionHandler> GetFakeSubscriptionHandler()
        {
            var subscriptionHandlerMock = new Mock<ISubscriptionHandler>();

            return subscriptionHandlerMock;
        }

        private Mock<ICustomerHandler> GetFakeCustomerHandler()
        {
            var customerHandlerMock = new Mock<ICustomerHandler>();

            return customerHandlerMock;
        }

        private Mock<ILogger> GetFakeLogService()
        {
            var logServiceHandlerMock = new Mock<ILogger>();

            return logServiceHandlerMock;
        }

        private Mock<ISiteSettings> GetFakeSiteSettings()
        {
            var siteSettingsMock = new Mock<ISiteSettings>();
            siteSettingsMock.Setup(t => t.SubsKindTimed).Returns("02");
            siteSettingsMock.Setup(t => t.SubsStateActiveValues).Returns(new List<string> {"00", "01", "02"});

            return siteSettingsMock;
        }

        [TestMethod]
        public void DenySubscriptionForPriceGroup_SubscriptionWithPriceGroup42Within3MonthsExist_ReturnTrue()
        {
            var customerHandlerMock = GetFakeCustomerHandler();
            customerHandlerMock.Setup(t => t.FindCustomerNumbersByEmail("test1@email.com")).Returns(
                () => new List<long> { 1234, 5678 });

            var subscriptionHandlerMock = GetFakeSubscriptionHandler();
            subscriptionHandlerMock.Setup(t => t.GetSubscriptions(1234, It.IsAny<DateTime>())).Returns(
                () =>
                {
                    var subscriptions = new List<DiSubscription>();
                    var sub1 = new DiSubscription
                    {
                        PriceGroup = "42",
                        StartDate = DateTime.Now.AddMonths(-6),
                        EndDate = DateTime.Now.AddMonths(-3).AddDays(1), //Should fail
                    };

                    var sub2 = new DiSubscription
                    {
                        PriceGroup = "00",
                        StartDate = DateTime.Now.AddMonths(-13),
                        EndDate = DateTime.Now.AddMonths(-1),
                    };

                    subscriptions.Add(sub1);
                    subscriptions.Add(sub2);

                    return subscriptions;
                });

            subscriptionHandlerMock.Setup(t => t.GetSubscriptions(5678, It.IsAny<DateTime>())).Returns(
                () =>
                {
                    var subscriptions = new List<DiSubscription>();
                    var sub1 = new DiSubscription
                    {
                        PriceGroup = "43",
                        StartDate = DateTime.Now.AddMonths(-6),
                        EndDate = DateTime.Now.AddMonths(-3),
                    };

                    var sub2 = new DiSubscription
                    {
                        PriceGroup = "00",
                        StartDate = DateTime.Now.AddMonths(-13),
                        EndDate = DateTime.Now.AddMonths(-1),
                    };

                    subscriptions.Add(sub1);
                    subscriptions.Add(sub2);

                    return subscriptions;
                });


            var subscriptionChecker = new SubscriptionChecker(subscriptionHandlerMock.Object, customerHandlerMock.Object, null, null);

            Assert.AreEqual(subscriptionChecker.DenySubscriptionForPriceGroup("42", "test1@email.com", 3), true);

        }


        [TestMethod]
        public void DenySubscriptionForPriceGroup_SubscriptionWithPriceGroup42NotWithin3MonthsExist_ReturnFalse()
        {
            var customerHandlerMock = GetFakeCustomerHandler();
            customerHandlerMock.Setup(t => t.FindCustomerNumbersByEmail("test1@email.com")).Returns(
                () => new List<long> { 1234, 5678 });

            var subscriptionHandlerMock = GetFakeSubscriptionHandler();
            subscriptionHandlerMock.Setup(t => t.GetSubscriptions(1234, It.IsAny<DateTime>())).Returns(
                () =>
                {
                    var subscriptions = new List<DiSubscription>();
                    var sub1 = new DiSubscription
                    {
                        PriceGroup = "42",
                        StartDate = DateTime.Now.AddMonths(-6),
                        EndDate = DateTime.Now.AddMonths(-3)
                    };

                    var sub2 = new DiSubscription
                    {
                        PriceGroup = "00",
                        StartDate = DateTime.Now.AddMonths(-13),
                        EndDate = DateTime.Now.AddMonths(-1),
                    };

                    subscriptions.Add(sub1);
                    subscriptions.Add(sub2);

                    return subscriptions;
                });

            subscriptionHandlerMock.Setup(t => t.GetSubscriptions(5678, It.IsAny<DateTime>())).Returns(
                () =>
                {
                    var subscriptions = new List<DiSubscription>();
                    var sub1 = new DiSubscription
                    {
                        PriceGroup = "01",
                        StartDate = DateTime.Now.AddMonths(-6),
                        EndDate = DateTime.Now.AddMonths(-3),
                    };

                    var sub2 = new DiSubscription
                    {
                        PriceGroup = "00",
                        StartDate = DateTime.Now.AddMonths(-13),
                        EndDate = DateTime.Now.AddMonths(-1),
                    };

                    subscriptions.Add(sub1);
                    subscriptions.Add(sub2);

                    return subscriptions;
                });


            var subscriptionChecker = new SubscriptionChecker(subscriptionHandlerMock.Object, customerHandlerMock.Object, null, null);

            Assert.AreEqual(subscriptionChecker.DenySubscriptionForPriceGroup("42", "test1@email.com", 3), false);

        }

        [TestMethod]
        public void DenySubscriptionForPriceGroup_SubscriptionWithoutPriceGroup42Within3MonthsExist_ReturnFalse()
        {
            var customerHandlerMock = GetFakeCustomerHandler();
            customerHandlerMock.Setup(t => t.FindCustomerNumbersByEmail("test1@email.com")).Returns(
                () => new List<long> { 1234, 5678 });

            var subscriptionHandlerMock = GetFakeSubscriptionHandler();
            subscriptionHandlerMock.Setup(t => t.GetSubscriptions(1234, It.IsAny<DateTime>())).Returns(
                () =>
                {
                    var subscriptions = new List<DiSubscription>();
                    var sub1 = new DiSubscription
                    {
                        PriceGroup = "43",
                        StartDate = DateTime.Now.AddMonths(-6),
                        EndDate = DateTime.Now.AddMonths(-1)
                    };

                    var sub2 = new DiSubscription
                    {
                        PriceGroup = "00",
                        StartDate = DateTime.Now.AddMonths(-13),
                        EndDate = DateTime.Now.AddMonths(-1),
                    };

                    subscriptions.Add(sub1);
                    subscriptions.Add(sub2);

                    return subscriptions;
                });

            subscriptionHandlerMock.Setup(t => t.GetSubscriptions(5678, It.IsAny<DateTime>())).Returns(
                () =>
                {
                    var subscriptions = new List<DiSubscription>();
                    var sub1 = new DiSubscription
                    {
                        PriceGroup = "01",
                        StartDate = DateTime.Now.AddMonths(-6),
                        EndDate = DateTime.Now.AddMonths(-3),
                    };

                    var sub2 = new DiSubscription
                    {
                        PriceGroup = "00",
                        StartDate = DateTime.Now.AddMonths(-13),
                        EndDate = DateTime.Now.AddMonths(-1),
                    };

                    subscriptions.Add(sub1);
                    subscriptions.Add(sub2);

                    return subscriptions;
                });


            var subscriptionChecker = new SubscriptionChecker(subscriptionHandlerMock.Object, customerHandlerMock.Object, null, null);

            Assert.AreEqual(subscriptionChecker.DenySubscriptionForPriceGroup("42", "test1@email.com", 3), false);

        }


        [TestMethod]
        public void DenySubscriptionOfSameType_SubscriptionWithSamePackageIdAndIsActiveExist_ReturnTrue()
        {
            var customerHandlerMock = GetFakeCustomerHandler();
            customerHandlerMock.Setup(t => t.FindCustomerNumbersByEmail("test1@email.com")).Returns(
                () => new List<long> { 1234 });

            var subscriptionHandlerMock = GetFakeSubscriptionHandler();
            subscriptionHandlerMock.Setup(t => t.GetSubscriptions(1234, It.IsAny<DateTime>())).Returns(
                () =>
                {
                    var subscriptions = new List<DiSubscription>();
                    var sub1 = new DiSubscription
                    {
                        PackageId = "DI_01",
                        SubscriptionKind = "01",
                        SubscriptionState = "01"
                    };

                    var sub2 = new DiSubscription
                    {
                        PackageId = "DI_01",
                        SubscriptionKind = "02",
                        SubscriptionState = "02"
                    };

                    subscriptions.Add(sub1);
                    subscriptions.Add(sub2);

                    return subscriptions;
                });

            var subscriptionChecker = new SubscriptionChecker(subscriptionHandlerMock.Object, customerHandlerMock.Object, GetFakeSiteSettings().Object, null);

            Assert.AreEqual(subscriptionChecker.DenySubscriptionOfSameType("DI_01", "test1@email.com"), true);

        }

        [TestMethod]
        public void DenySubscriptionOfSameType_SubscriptionWithSamePackageIdAndNotActiveExist_ReturnFalse()
        {
            var customerHandlerMock = GetFakeCustomerHandler();
            customerHandlerMock.Setup(t => t.FindCustomerNumbersByEmail("test1@email.com")).Returns(
                () => new List<long> { 1234 });

            var subscriptionHandlerMock = GetFakeSubscriptionHandler();
            subscriptionHandlerMock.Setup(t => t.GetSubscriptions(1234, It.IsAny<DateTime>())).Returns(
                () =>
                {
                    var subscriptions = new List<DiSubscription>();
                    var sub1 = new DiSubscription
                    {
                        PackageId = "DI_01",
                        SubscriptionKind = "01",
                        SubscriptionState = "666"
                    };

                    var sub2 = new DiSubscription
                    {
                        PackageId = "DI_01",
                        SubscriptionKind = "02",
                        SubscriptionState = "666"
                    };

                    subscriptions.Add(sub1);
                    subscriptions.Add(sub2);

                    return subscriptions;
                });

            var subscriptionChecker = new SubscriptionChecker(subscriptionHandlerMock.Object, customerHandlerMock.Object, GetFakeSiteSettings().Object, null);

            Assert.AreEqual(subscriptionChecker.DenySubscriptionOfSameType("DI_01", "test1@email.com"), false);

        }

        [TestMethod]
        public void DenySubscriptionOfSameType_SubscriptionWithNotSamePackageIdAndIsActiveExist_ReturnFalse()
        {
            var customerHandlerMock = GetFakeCustomerHandler();
            customerHandlerMock.Setup(t => t.FindCustomerNumbersByEmail("test1@email.com")).Returns(
                () => new List<long> { 1234 });

            var subscriptionHandlerMock = GetFakeSubscriptionHandler();
            subscriptionHandlerMock.Setup(t => t.GetSubscriptions(1234, It.IsAny<DateTime>())).Returns(
                () =>
                {
                    var subscriptions = new List<DiSubscription>();
                    var sub1 = new DiSubscription
                    {
                        PackageId = "DI_02",
                        SubscriptionKind = "01",
                        SubscriptionState = "01"
                    };

                    var sub2 = new DiSubscription
                    {
                        PackageId = "DI_03",
                        SubscriptionKind = "01",
                        SubscriptionState = "01"
                    };

                    subscriptions.Add(sub1);
                    subscriptions.Add(sub2);

                    return subscriptions;
                });

            var subscriptionChecker = new SubscriptionChecker(subscriptionHandlerMock.Object, customerHandlerMock.Object, GetFakeSiteSettings().Object, null);

            Assert.AreEqual(subscriptionChecker.DenySubscriptionOfSameType("DI_01", "test1@email.com"), false);

        }

    }
}
