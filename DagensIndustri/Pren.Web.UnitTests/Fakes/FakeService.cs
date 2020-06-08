using Di.Common.Logging;
using Di.ServicePlus.RedirectApi;
using Di.Subscription.Logic.Customer;
using Di.Subscription.Logic.Subscription;
using DIClassLib.BonnierDigital;
using DIClassLib.Subscriptions;
using Moq;
using Pren.Web.Business.BusinessSubscription;
using Pren.Web.Business.Configuration;
using Pren.Web.Business.DataAccess.Package;
using Pren.Web.Business.Detection;
using Pren.Web.Business.Helpers;
using Pren.Web.Business.Mail;
using Pren.Web.Business.ServicePlus;
using Pren.Web.Business.ServicePlus.Logic;
using Pren.Web.Business.Session;
using Pren.Web.Business.Subscription;

namespace Pren.Web.UnitTests.Fakes
{
    public class FakeService
    {

        public static Mock<IInviteImporter> GetFakeInviteImporter()
        {
            return new Mock<IInviteImporter>();
        }

        public static Mock<IPriceHandler> GetFakePriceHandler()
        {
            return new Mock<IPriceHandler>();
        }

        public static Mock<IPackageRelationItemDataHandler> GetFakePackageRelationItemDataHandler()
        {
            var packageRelationItemDataHandler = new Mock<IPackageRelationItemDataHandler>();
            return packageRelationItemDataHandler;
        }

        public static Mock<ISiteSettings> GetFakeSiteSettings()
        {
            var siteSettingsMock = new Mock<ISiteSettings>();
            return siteSettingsMock;
        }

        public static Mock<ISubscriptionHandler> GetFakeSubscriptionHandler()
        {
            var subscriptionHandlerMock = new Mock<ISubscriptionHandler>();

            return subscriptionHandlerMock;
        }

        public static Mock<IServicePlusHandler<UserOutput>> GetFakeServicePlusHandler()
        {
            var servicePlusHandlerMock = new Mock<IServicePlusHandler<UserOutput>>();
            return servicePlusHandlerMock;
        }

        public static Mock<ILogger> GetFakeLogService()
        {
            var logServiceMock = new Mock<ILogger>();
            return logServiceMock;
        }

        public static Mock<ISiteConfiguration> GetFakeSiteConfiguration()
        {
            var siteConfigurationMock = new Mock<ISiteConfiguration>();
            return siteConfigurationMock;
        }

        public static Mock<IUrlHelper> GetFakeUrlHelper()
        {
            var urlHelperMock = new Mock<IUrlHelper>();
            return urlHelperMock;
        }

        public static Mock<IMailHandler> GetFakeMailHandler()
        {
            var mailHandlerMock = new Mock<IMailHandler>();
            return mailHandlerMock;
        }

        public static Mock<ISessionData> GetFakeSessionData()
        {
            var sessionDataMock = new Mock<ISessionData>();
            return sessionDataMock;
        }

        public static Mock<IDetectionHandler> GetFakeDetectionHandler()
        {
            var detectionHandlerMock = new Mock<IDetectionHandler>();
            return detectionHandlerMock;
        }

        public static Mock<ISubscriptionUser<SubscriptionUser2>> GetFakeSubscriptionUser()
        {
            var subscriptionUserMock = new Mock<ISubscriptionUser<SubscriptionUser2>>();

            return subscriptionUserMock;
        }

        public static Mock<ISubscriptionUser<SubscriptionUser2>> GetFakeSubscriptionUserWithSubscriber()
        {
            var subscriptionUserMock = new Mock<ISubscriptionUser<SubscriptionUser2>>();

            var fakeSubscriptionUser2 = new SubscriptionUser2(string.Empty);
            
            subscriptionUserMock.Setup(subscriptionUser => subscriptionUser.Subscriber).Returns(fakeSubscriptionUser2);

            return subscriptionUserMock;
        }

        public static Mock<ISessionData> GetFakeSessionDataWithSubscriber(Subscriber subscriber)
        {
            var sessionDataMock = new Mock<ISessionData>();

            sessionDataMock.Setup(sessionData => sessionData.Get("SubscriberSessionKey", null)).Returns(subscriber);

            return sessionDataMock;
        }

        public static Mock<ISessionData> GetFakeSessionDataWithSubscriptionUser(Mock<ISubscriptionUser<SubscriptionUser2>> subscriptionUserMock)
        {
            var sessionDataMock = new Mock<ISessionData>();

            sessionDataMock.Setup(sessionData => sessionData.Get("subscriptionUser", null)).Returns(subscriptionUserMock.Object);

            return sessionDataMock;
        }

        public static Mock<IServicePlusFacade> GetFakeServicePlusFacade()
        {
            var fakeServicePlusFacade = new Mock<IServicePlusFacade>();

            return fakeServicePlusFacade;
        }

        public static Mock<ICustomerHandler> GetFakeCustomerHandler()
        {
            return new Mock<ICustomerHandler>();
        }

        public static Mock<IRedirectHandler> GetFakeRedirectHandler()
        {
            return new Mock<IRedirectHandler>();
        }

        public static Mock<IConnectService> GetFakeConnectService()
        {
            var connectServiceMock = new Mock<IConnectService>();
            return connectServiceMock;
        }
    }
}
