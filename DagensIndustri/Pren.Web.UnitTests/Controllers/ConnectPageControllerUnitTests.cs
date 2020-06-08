using System.Web.Mvc;
using Di.ServicePlus.RedirectApi;
using Di.Subscription.Logic.Customer;
using EPiServer.ServiceLocation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pren.Web.Business.Detection;
using Pren.Web.Business.Helpers;
using Pren.Web.Business.Mail;
using Pren.Web.Business.Messaging;
using Pren.Web.Business.ServicePlus.Logic;
using Pren.Web.Business.Session;
using Pren.Web.Business.Subscription;
using Pren.Web.Controllers.MySettings;
using Pren.Web.Models.Pages.MySettings;
using Pren.Web.UnitTests.Fakes;
using Pren.Web.UnitTests.Fakes.Content;
using Pren.Web.UnitTests.Fakes.Subscription;

namespace Pren.Web.UnitTests.Controllers
{
    [TestClass]
    public class ConnectPageControllerUnitTests
    {
        #region Construct

        private ConnectPageController GetConnectPageController(
            IUrlHelper urlHelper = null,
            IMailHandler mailHandler = null,
            IDetectionHandler detectionHandler = null,
            ISessionData sessionData = null, 
            IRedirectHandler redirectHandler = null,
            IServicePlusFacade servicePlusFacade = null, 
            ICustomerHandler customerHandler = null)
        {
            var connectPageController = new ConnectPageController(
                urlHelper ?? FakeService.GetFakeUrlHelper().Object,
                mailHandler ?? FakeService.GetFakeMailHandler().Object,
                detectionHandler ?? FakeService.GetFakeDetectionHandler().Object,
                sessionData ?? FakeService.GetFakeSessionData().Object,              
                redirectHandler ?? FakeService.GetFakeRedirectHandler().Object,
                servicePlusFacade ?? FakeService.GetFakeServicePlusFacade().Object,
                customerHandler ?? FakeService.GetFakeCustomerHandler().Object);

            return connectPageController;
        }

        public Mock<IConnectService> FakeConnectMock;
        public Mock<ISubscriberFacade> FakeSubscriberFacaceMock;

        [TestInitialize]
        public void MockBaseDependencies()
        {
            var mockLocator = new Mock<IServiceLocator>();

            FakeConnectMock = FakeService.GetFakeConnectService();
            FakeSubscriberFacaceMock = FakeSubscriptionFactory.FakeSubscriberFacade();
            var fakeUrlHelper = FakeService.GetFakeUrlHelper();

            // Since MySettingsControllerBase uses ServiceLocator directly
            mockLocator.Setup(l => l.GetInstance<IUrlHelper>()).Returns(fakeUrlHelper.Object);
            mockLocator.Setup(l => l.GetInstance<IConnectService>()).Returns(FakeConnectMock.Object);
            mockLocator.Setup(l => l.GetInstance<ISubscriberFacade>()).Returns(FakeSubscriberFacaceMock.Object);

            ServiceLocator.SetLocator(mockLocator.Object);
        }

        #endregion

        private void SetUpFakeConnectMockWithConnectStatus(ConnectStatus status)
        {
            FakeConnectMock.Setup(connectService => connectService.GetConnectStatus(It.IsAny<Subscriber>()))
                .Returns(status);
        }

        private void SetSubscriberInSession(
            Subscriber subscriberInSession = null)
        {
            FakeSubscriberFacaceMock.Setup(facade => facade.GetSubscriberFromSession()).Returns(subscriberInSession ?? FakeSubscriptionFactory.FakeSubscriber());
        }

        //UnitOfWork_StateUnderTest_ExcpectedBehaviour
        [TestMethod]
        public void Index_StatusConnectExistingServicePlusWithExistingPren_ReturnsViewConnectExisting()
        {
            SetUpFakeConnectMockWithConnectStatus(ConnectStatus.ConnectExistingServicePlusWithExistingPren);

            var subscription = FakeSubscriptionFactory.FakeSubscription().WithKayakCustomer();
            var subscriber = FakeSubscriptionFactory.FakeSubscriber().WithSelectedSubcription(subscription).WithServicePlusUser();

            SetSubscriberInSession(subscriber);

            var connectPageController = GetConnectPageController();

            var result = connectPageController.Index(FakeContentFactory.GetFakePageContent<ConnectPage>(), string.Empty);

            Assert.AreEqual(((ViewResult)result).ViewName, "ConnectExisting");                
        }

        [TestMethod]
        public void Index_StatusConnectExistingPrenWithServicePlus_ReturnsViewConnectExistingPren()
        {
            SetUpFakeConnectMockWithConnectStatus(ConnectStatus.ConnectExistingPrenWithServicePlus);

            SetSubscriberInSession();

            var connectPageController = GetConnectPageController();

            var result = connectPageController.Index(FakeContentFactory.GetFakePageContent<ConnectPage>(), string.Empty);

            Assert.AreEqual(((ViewResult)result).ViewName, "ConnectExistingPren");
        }

        [TestMethod]
        public void Index_StatusInvalidCode_ShowsErrorMessage()
        {
            SetUpFakeConnectMockWithConnectStatus(ConnectStatus.InvalidCode);

            SetSubscriberInSession();

            var connectPageController = GetConnectPageController();

            connectPageController.Index(FakeContentFactory.GetFakePageContent<ConnectPage>(), string.Empty);

            var message = connectPageController.TempData["Message"] as Message;

            Assert.IsTrue(message != null && message.Type == MessageType.Danger);
        }

        [TestMethod]
        public void Index_StatusUnableToConnectPrenWithServicePlus_ShowsErrorMessage()
        {
            SetUpFakeConnectMockWithConnectStatus(ConnectStatus.UnableToConnectPrenWithServicePlus);

            SetSubscriberInSession();

            var connectPageController = GetConnectPageController();

            connectPageController.Index(FakeContentFactory.GetFakePageContent<ConnectPage>(), string.Empty);

            var message = connectPageController.TempData["Message"] as Message;

            Assert.IsTrue(message != null && message.Type == MessageType.Danger);
        }

        [TestMethod]
        public void Index_StatusIsConnected_ShowsInfoMessage()
        {
            SetUpFakeConnectMockWithConnectStatus(ConnectStatus.IsConnected);

            SetSubscriberInSession();

            var connectPageController = GetConnectPageController();

            connectPageController.Index(FakeContentFactory.GetFakePageContent<ConnectPage>(), string.Empty);

            var message = connectPageController.TempData["Message"] as Message;

            Assert.IsTrue(message != null && message.Type == MessageType.Info);
        }

        [TestMethod]
        public void Connect_ImportedIsFalse_MessageIsSet()
        {                        
            var subscription = FakeSubscriptionFactory.FakeSubscription().WithKayakCustomer().WithSubscriptionItems();
            var subscriber = FakeSubscriptionFactory.FakeSubscriber().WithSelectedSubcription(subscription).WithServicePlusUser();

            SetSubscriberInSession(subscriber);

            var connectPageController = GetConnectPageController();

            connectPageController.Connect(FakeContentFactory.GetFakePageContent<ConnectPage>());
           
            Assert.IsTrue(connectPageController.TempData["Message"] != null, "Message is set");
        }

        [TestMethod]
        public void Connect_ImportedIsTrue_ResetSubscriber()
        {
            var subscription = FakeSubscriptionFactory.FakeSubscription().WithKayakCustomer().WithSubscriptionItems();
            var subscriber = FakeSubscriptionFactory.FakeSubscriber().WithSelectedSubcription(subscription).WithServicePlusUser();

            var servicePlusFacadeMock = FakeService.GetFakeServicePlusFacade();
            servicePlusFacadeMock.Setup(
                spf => spf.ImportEntitlement(It.IsAny<string>(), It.IsAny<long>(), It.IsAny<long>(), It.IsAny<string>(),It.IsAny<string>())).Returns(true);
  
            var sessionDataMock = FakeService.GetFakeSessionDataWithSubscriber(subscriber);

            SetSubscriberInSession(subscriber);

            var connectPageController = GetConnectPageController(sessionData: sessionDataMock.Object, servicePlusFacade: servicePlusFacadeMock.Object);

            connectPageController.Connect(FakeContentFactory.GetFakePageContent<ConnectPage>());

            // Verify that subscriptionUser is reset
            sessionDataMock.Verify(t => t.Set("SubscriberSessionKey", null, null), Times.Once());
        }
    }
}
