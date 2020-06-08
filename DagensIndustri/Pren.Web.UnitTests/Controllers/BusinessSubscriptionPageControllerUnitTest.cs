using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Castle.Components.DictionaryAdapter;
using Di.Common.Logging;
using EPiServer;
using EPiServer.ServiceLocation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pren.Web.Business.BusinessSubscription;
using Pren.Web.Business.Detection;
using Pren.Web.Business.Helpers;
using Pren.Web.Business.Messaging;
using Pren.Web.Business.ServicePlus.Logic;
using Pren.Web.Business.ServicePlus.Models;
using Pren.Web.Business.Session;
using Pren.Web.Business.Subscription;
using Pren.Web.Controllers.MySettings;
using Pren.Web.Models.Pages.MySettings;
using Pren.Web.Models.ViewModels.MySettings;
using Pren.Web.UnitTests.Fakes;
using Pren.Web.UnitTests.Fakes.Content;
using Pren.Web.UnitTests.Fakes.Subscription;
using Pren.Web.UnitTests.Fakes.Web;

namespace Pren.Web.UnitTests.Controllers
{
    [TestClass]
    public class BusinessSubscriptionPageControllerUnitTest
    {

        private BusinessSubscriptionPageController GetBusinessSubscriptionPageController(
            IServicePlusFacade servicePlusFacade = null,
            ISessionData sessionData = null,
            IPriceHandler priceHandler = null,
            ILogger logService = null,
            IDetectionHandler detection = null,
            IInviteImporter inviteImporter = null,
            IUrlHelper urlHelper = null)
        {
            var businessSubscriptionActivationPageController = new BusinessSubscriptionPageController(
                servicePlusFacade ?? FakeService.GetFakeServicePlusFacade().Object,
                sessionData ?? FakeService.GetFakeSessionData().Object,
                priceHandler ?? FakeService.GetFakePriceHandler().Object,
                logService ?? FakeService.GetFakeLogService().Object,
                detection ?? FakeService.GetFakeDetectionHandler().Object,
                inviteImporter ?? FakeService.GetFakeInviteImporter().Object,
                urlHelper ?? FakeService.GetFakeUrlHelper().Object,
                new Mock<IContentRepository>().Object);

            return businessSubscriptionActivationPageController;
        }

        public Mock<ISubscriberFacade> FakeSubscriberFacaceMock;

        [TestInitialize]
        public void MockBaseDependencies()
        {
            var mockLocator = new Mock<IServiceLocator>();

            var fakeConnectMock = FakeService.GetFakeConnectService();
            FakeSubscriberFacaceMock = FakeSubscriptionFactory.FakeSubscriberFacade();
            var fakeUrlHelper = FakeService.GetFakeUrlHelper();

            // Since MySettingsControllerBase uses ServiceLocator directly
            mockLocator.Setup(l => l.GetInstance<IUrlHelper>()).Returns(fakeUrlHelper.Object);
            mockLocator.Setup(l => l.GetInstance<IConnectService>()).Returns(fakeConnectMock.Object);
            mockLocator.Setup(l => l.GetInstance<ISubscriberFacade>()).Returns(FakeSubscriberFacaceMock.Object);

            ServiceLocator.SetLocator(mockLocator.Object);
        }

        private void SetSubscriberInSession(Subscriber subscriberInSession = null)
        {
            FakeSubscriberFacaceMock.Setup(facade => facade.GetSubscriberFromSession()).Returns(subscriberInSession ?? FakeSubscriptionFactory.FakeSubscriber());
        }

        [TestMethod]
        public async Task Index_PendingSubscriber_UpdateSubscriberFromSourcesIsCalled()
        {
            var subscription = FakeSubscriptionFactory.FakeSubscription(SubscriptionType.Business);
            subscription.CustomerNumber = 0;
            subscription.IsPending = true;
            var subscriber = FakeSubscriptionFactory.FakeSubscriber().WithSelectedSubcription(subscription);

            SetSubscriberInSession(subscriber);

            FakeSubscriberFacaceMock.Setup(facade => facade.GetSubscriber(It.IsAny<string>())).Returns(Task.FromResult(subscriber));

            var sessionDataMock = FakeService.GetFakeSessionData();

            var controller = GetBusinessSubscriptionPageController(sessionData: sessionDataMock.Object);

            await controller.Index(FakeContentFactory.GetFakePageContent<BusinessSubscriptionPage>(), It.IsAny<string>());

            Mock.Get(FakeSubscriberFacaceMock.Object)
                .Verify(x => x.GetSubscriber(It.IsAny<string>()), Times.Once);

            Mock.Get(sessionDataMock.Object)
                .Verify(x => x.Set(SessionConstants.SubscriberSessionKey, It.IsAny<object>(), null), Times.Once);
        }

        [TestMethod]
        public void Index_NoServicePlusUser_GetBizSubscriptionsNotCalled()
        {
            var subscription = FakeSubscriptionFactory.FakeSubscription(SubscriptionType.Business);
            var subscriber = FakeSubscriptionFactory.FakeSubscriber().WithSelectedSubcription(subscription);

            SetSubscriberInSession(subscriber);

            var servicePlusFacadeMock = FakeService.GetFakeServicePlusFacade();

            var controller = GetBusinessSubscriptionPageController(servicePlusFacadeMock.Object);

            controller.Index(FakeContentFactory.GetFakePageContent<BusinessSubscriptionPage>(), It.IsAny<string>());

            Mock.Get(servicePlusFacadeMock.Object)
                .Verify(x => x.GetBizSubscriptions(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void Index_WithServicePlusUser_GetBizSubscriptionsIsCalled()
        {
            var subscription = FakeSubscriptionFactory.FakeSubscription(SubscriptionType.Business);
            var subscriber = FakeSubscriptionFactory.FakeSubscriber().WithSelectedSubcription(subscription).WithServicePlusUser();

            SetSubscriberInSession(subscriber);

            var servicePlusFacadeMock = FakeService.GetFakeServicePlusFacade();

            var controller = GetBusinessSubscriptionPageController(servicePlusFacadeMock.Object);

            controller.Index(FakeContentFactory.GetFakePageContent<BusinessSubscriptionPage>(), It.IsAny<string>());

            Mock.Get(servicePlusFacadeMock.Object)
                .Verify(x => x.GetBizSubscriptions(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void Index_NoBizSubscription_GetEntitlementNotCalled()
        {
            var subscription = FakeSubscriptionFactory.FakeSubscription(SubscriptionType.Business);
            var subscriber = FakeSubscriptionFactory.FakeSubscriber().WithSelectedSubcription(subscription).WithServicePlusUser();

            SetSubscriberInSession(subscriber);

            var servicePlusFacadeMock = FakeService.GetFakeServicePlusFacade();

            var controller = GetBusinessSubscriptionPageController(servicePlusFacadeMock.Object);

            controller.Index(FakeContentFactory.GetFakePageContent<BusinessSubscriptionPage>(), It.IsAny<string>());

            Mock.Get(servicePlusFacadeMock.Object)
                .Verify(x => x.GetEntitlement(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void Index_WithBizSubscription_GetEntitlementIsCalled()
        {
            var subscription = FakeSubscriptionFactory.FakeSubscription(SubscriptionType.Business);
            var subscriber = FakeSubscriptionFactory.FakeSubscriber().WithSelectedSubcription(subscription).WithServicePlusUser();

            SetSubscriberInSession(subscriber);

            var servicePlusFacadeMock = FakeService.GetFakeServicePlusFacade();
            servicePlusFacadeMock.Setup(facade => facade.GetBizSubscriptions(It.IsAny<string>())).Returns(
                new List<BizSubscription>
                {
                    new BizSubscription()
                });

            var controller = GetBusinessSubscriptionPageController(servicePlusFacadeMock.Object);

            controller.Index(FakeContentFactory.GetFakePageContent<BusinessSubscriptionPage>(), It.IsAny<string>());

            Mock.Get(servicePlusFacadeMock.Object)
                .Verify(x => x.GetEntitlement(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void Index_NotValidEntitlement_GetBizSubscriptionDefinitionNotCalled()
        {
            var subscription = FakeSubscriptionFactory.FakeSubscription(SubscriptionType.Business);
            var subscriber = FakeSubscriptionFactory.FakeSubscriber().WithSelectedSubcription(subscription).WithServicePlusUser();

            SetSubscriberInSession(subscriber);

            var servicePlusFacadeMock = FakeService.GetFakeServicePlusFacade();
            servicePlusFacadeMock.Setup(facade => facade.GetBizSubscriptions(It.IsAny<string>())).Returns(
                new List<BizSubscription>
                        {
                            new BizSubscription()
                        });

            servicePlusFacadeMock.Setup(facade => facade.GetEntitlement(It.IsAny<string>())).Returns(
                new Entitlement { ValidTo = DateTime.Now.AddMonths(-1) });

            var controller = GetBusinessSubscriptionPageController(servicePlusFacadeMock.Object);

            controller.Index(FakeContentFactory.GetFakePageContent<BusinessSubscriptionPage>(), It.IsAny<string>());

            Mock.Get(servicePlusFacadeMock.Object)
                .Verify(x => x.GetBizSubscriptionDefinition(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void Index_ValidEntitlement_GetBizSubscriptionDefinitionIsCalled()
        {
            var subscription = FakeSubscriptionFactory.FakeSubscription(SubscriptionType.Business);
            var subscriber = FakeSubscriptionFactory.FakeSubscriber().WithSelectedSubcription(subscription).WithServicePlusUser();

            SetSubscriberInSession(subscriber);

            var servicePlusFacadeMock = FakeService.GetFakeServicePlusFacade();
            servicePlusFacadeMock.Setup(facade => facade.GetBizSubscriptions(It.IsAny<string>())).Returns(
                new List<BizSubscription>
                        {
                            new BizSubscription()
                        });

            servicePlusFacadeMock.Setup(facade => facade.GetEntitlement(It.IsAny<string>())).Returns(
                new Entitlement { ValidTo = DateTime.Now.AddMonths(+1) });

            var controller = GetBusinessSubscriptionPageController(servicePlusFacadeMock.Object);

            controller.Index(FakeContentFactory.GetFakePageContent<BusinessSubscriptionPage>(), It.IsAny<string>());

            Mock.Get(servicePlusFacadeMock.Object)
                .Verify(x => x.GetBizSubscriptionDefinition(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public async Task Index_NoBusinessDefinition_ModelWithInActivatedBusinessSubscriberReturned()
        {
            var subscription = FakeSubscriptionFactory.FakeSubscription(SubscriptionType.Business);
            var subscriber = FakeSubscriptionFactory.FakeSubscriber().WithSelectedSubcription(subscription).WithServicePlusUser();

            SetSubscriberInSession(subscriber);

            var servicePlusFacadeMock = FakeService.GetFakeServicePlusFacade();
            servicePlusFacadeMock.Setup(facade => facade.GetBizSubscriptions(It.IsAny<string>())).Returns(
                new List<BizSubscription>
                        {
                            new BizSubscription()
                        });

            servicePlusFacadeMock.Setup(facade => facade.GetEntitlement(It.IsAny<string>())).Returns(
                new Entitlement { ValidTo = DateTime.Now.AddMonths(+1) });

            var controller = GetBusinessSubscriptionPageController(servicePlusFacadeMock.Object);

            var result = await controller.Index(FakeContentFactory.GetFakePageContent<BusinessSubscriptionPage>(), It.IsAny<string>());

            Assert.IsTrue(((BusinessSubscriptionPageViewModel)((ViewResult)result).Model).MasterUser.Activated == false);
        }

        [TestMethod]
        public async Task Index_WithBusinessDefinition_ModelWithActivatedBusinessSubscriberReturned()
        {
            var subscription = FakeSubscriptionFactory.FakeSubscription(SubscriptionType.Business);
            var subscriber = FakeSubscriptionFactory.FakeSubscriber().WithSelectedSubcription(subscription).WithServicePlusUser();

            SetSubscriberInSession(subscriber);

            var servicePlusFacadeMock = FakeService.GetFakeServicePlusFacade();
            servicePlusFacadeMock.Setup(facade => facade.GetBizSubscriptions(It.IsAny<string>())).Returns(
                new List<BizSubscription>
                        {
                            new BizSubscription()
                        });

            servicePlusFacadeMock.Setup(facade => facade.GetEntitlement(It.IsAny<string>())).Returns(
                new Entitlement { ValidTo = DateTime.Now.AddMonths(+1) });

            servicePlusFacadeMock.Setup(facade => facade.GetBizSubscriptionDefinition(It.IsAny<string>())).Returns(
                new BizSubscriptionDefinition());

            var controller = GetBusinessSubscriptionPageController(servicePlusFacadeMock.Object);

            var result = await controller.Index(FakeContentFactory.GetFakePageContent<BusinessSubscriptionPage>(), It.IsAny<string>());

            Assert.IsTrue(((BusinessSubscriptionPageViewModel)((ViewResult)result).Model).MasterUser.Activated);
        }

        [TestMethod]
        public void ImportFile_WrongFileExtension_ReturnsWarningMessage()
        {
            var inviteImporterMock = FakeService.GetFakeInviteImporter();
            inviteImporterMock.Setup(importer => importer.FileExtensionIsAccepted(It.IsAny<string>())).Returns(false);

            var controller = GetBusinessSubscriptionPageController(inviteImporter: inviteImporterMock.Object);
            var fakePostedFile = new FakePostedFileBase("test.xls");

            controller.ImportFile(FakeContentFactory.GetFakePageContent<BusinessSubscriptionPage>(), string.Empty, fakePostedFile);

            var message = controller.TempData["Message"] as Message;

            Assert.IsTrue(message != null && message.Type == MessageType.Danger);
        }

        [TestMethod]
        public void ImportFile_RowsWithInvalidEmailAdress_AreNotImported()
        {
            var fakePostedFile = new FakePostedFileBase("test.txt");
            var inviteImporterMock = FakeService.GetFakeInviteImporter();
            inviteImporterMock.Setup(importer => importer.FileExtensionIsAccepted(It.IsAny<string>())).Returns(true);
            inviteImporterMock.Setup(importer => importer.GetImportRows(fakePostedFile.InputStream))
                .Returns(new List<ImportRow>
                {
                    new ImportRow {Email = "fel@fel"}
                });

            var servicePlusFacadeMock = FakeService.GetFakeServicePlusFacade();
            var detectionMock = FakeService.GetFakeDetectionHandler();
            detectionMock.Setup(detection => detection.IsValidEmail(It.IsAny<string>())).Returns(false);

            var controller = GetBusinessSubscriptionPageController(inviteImporter: inviteImporterMock.Object, servicePlusFacade: servicePlusFacadeMock.Object, detection: detectionMock.Object);

            controller.ImportFile(FakeContentFactory.GetFakePageContent<BusinessSubscriptionPage>(), string.Empty, fakePostedFile);

            Mock.Get(servicePlusFacadeMock.Object)
                .Verify(x => x.InviteBizSubscriberByEmail(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void ImportFile_RowsWithValidEmailAdress_AreImported()
        {
            var fakePostedFile = new FakePostedFileBase("test.txt");
            var inviteImporterMock = FakeService.GetFakeInviteImporter();
            inviteImporterMock.Setup(importer => importer.FileExtensionIsAccepted(It.IsAny<string>())).Returns(true);
            inviteImporterMock.Setup(importer => importer.GetImportRows(fakePostedFile.InputStream))
                .Returns(new List<ImportRow>
                {
                    new ImportRow {Email = "korrekt@korrekt.se"}
                });

            var detectionMock = FakeService.GetFakeDetectionHandler();
            detectionMock.Setup(detection => detection.IsValidEmail(It.IsAny<string>())).Returns(true);

            var servicePlusFacadeMock = FakeService.GetFakeServicePlusFacade();

            var controller = GetBusinessSubscriptionPageController(inviteImporter: inviteImporterMock.Object, servicePlusFacade: servicePlusFacadeMock.Object, detection: detectionMock.Object);

            controller.ImportFile(FakeContentFactory.GetFakePageContent<BusinessSubscriptionPage>(), string.Empty, fakePostedFile);

            Mock.Get(servicePlusFacadeMock.Object)
                .Verify(x => x.InviteBizSubscriberByEmail(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
    }
}
