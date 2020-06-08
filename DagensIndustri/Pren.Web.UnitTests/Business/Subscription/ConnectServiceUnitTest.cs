using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pren.Web.Business.Subscription;
using Pren.Web.UnitTests.Fakes.Subscription;
using SubscriptionType = Pren.Web.Business.Subscription.SubscriptionType;

namespace Pren.Web.UnitTests.Business.Subscription
{
    [TestClass]
    public class ConnectServiceUnitTest
    {
        private IConnectService GetConnectService()
        {
            return new ConnectService();
        }

        [TestMethod]
        public void GetConnectStatus_SubscriberIsNull_ReturnsNothingToConnect()
        {
            var connectService = GetConnectService();

            Assert.AreEqual(ConnectStatus.NothingToConnect, connectService.GetConnectStatus((Subscriber)null));
        }

        [TestMethod]
        public void GetConnectStatus_WithSelectedSubscriptionAndServicePlusUserIsNull_ReturnsConnectExistingPrenWithServicePlus()
        {
            var connectService = GetConnectService();
            var subscriber = FakeSubscriptionFactory.FakeSubscriber().WithSelectedSubcription();

            Assert.AreEqual(ConnectStatus.ConnectExistingPrenWithServicePlus, connectService.GetConnectStatus(subscriber));
        }

        [TestMethod]
        public void GetConnectStatus_ServicePlusUserIsNullAndSelectedSubscriptionIsNull_ReturnsNothingToConnect()
        {
            var connectService = GetConnectService();
            var subscriber = FakeSubscriptionFactory.FakeSubscriber();

            Assert.AreEqual(ConnectStatus.NothingToConnect, connectService.GetConnectStatus(subscriber));
        }

        [TestMethod]
        public void GetConnectStatus_WithServicePlusUserAndSelectedSubscriptionThatHasMultipleCustomerNumbers_ReturnsUnableToConnectPrenWithServicePlus()
        {
            var connectService = GetConnectService();
            var subscriber = FakeSubscriptionFactory.FakeSubscriber().WithServicePlusUser().WithSelectedSubcription(new UserSubscription{ HasMultipleCustomerNumbers = true });

            Assert.AreEqual(ConnectStatus.UnableToConnectPrenWithServicePlus, connectService.GetConnectStatus(subscriber));
        }

        [TestMethod]
        public void GetConnectStatus_WithServicePlusUserAndSelectedSubscriptionThatIsOfTypeBusinessSubscription_ReturnsUnableToConnectPrenWithServicePlus()
        {
            var connectService = GetConnectService();
            var subscriber = FakeSubscriptionFactory.FakeSubscriber().WithServicePlusUser().WithSelectedSubcription(new UserSubscription { Type = SubscriptionType.Business });

            Assert.AreEqual(ConnectStatus.UnableToConnectPrenWithServicePlus, connectService.GetConnectStatus(subscriber));
        }

        [TestMethod]
        public void GetConnectStatus_WithServicePlusUserAndSelectedSubscriptionIsNull_ReturnsConnectExistingServicePlusWithPren()
        {
            var connectService = GetConnectService();
            var subscriber = FakeSubscriptionFactory.FakeSubscriber().WithServicePlusUser();

            Assert.AreEqual(ConnectStatus.ConnectExistingServicePlusWithPren, connectService.GetConnectStatus(subscriber));
        }

        [TestMethod]
        public void GetConnectStatus_WithServicePlusUserAndSelectedSubscriptionIsConnected_ReturnsIsConnected()
        {
            var connectService = GetConnectService();
            var subscriber = FakeSubscriptionFactory.FakeSubscriber().WithServicePlusUser().WithSelectedSubcription(new UserSubscription{ IsConnected = true });

            Assert.AreEqual(ConnectStatus.IsConnected, connectService.GetConnectStatus(subscriber));
        }

        [TestMethod]
        public void GetConnectStatus_WithSelectedSubscriptionAndServicePlusUserThatIsNotAlreadyConnected_ReturnsConnectExistingServicePlusWithExistingPren()
        {
            var connectService = GetConnectService();
            var subscriber = FakeSubscriptionFactory.FakeSubscriber().WithServicePlusUser().WithSelectedSubcription();

            Assert.AreEqual(ConnectStatus.ConnectExistingServicePlusWithExistingPren, connectService.GetConnectStatus(subscriber));
        }

        
    }
}
