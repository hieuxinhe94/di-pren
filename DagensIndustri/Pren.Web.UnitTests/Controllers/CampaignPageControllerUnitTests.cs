using System.Collections.Generic;
using Di.Common.Logging;
using Di.ServicePlus.RedirectApi;
using DIClassLib.BonnierDigital;
using DIClassLib.CardPayment.Nets;
using DIClassLib.Subscriptions;
using EPiServer;
using EPiServer.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pren.Web.Business.Detection;
using Pren.Web.Business.Helpers;
using Pren.Web.Business.ServicePlus;
using Pren.Web.Business.ServicePlus.Logic;
using Pren.Web.Business.Session;
using Pren.Web.Business.Subscription;
using Pren.Web.Controllers;
using Pren.Web.Models.CustomForms;
using Pren.Web.Models.Pages;
using Pren.Web.UnitTests.Fakes;

namespace Pren.Web.UnitTests.Controllers
{
    [TestClass]
    public class CampaignPageControllerUnitTests
    {

        private CampaignPageController GetCampaignPageController(
            IServicePlusHandler<UserOutput> servicePlusHandler = null,
            IUrlHelper urlHelper = null,
            ILogger logService = null,
            IPageSpecificSession pageSpecificSession = null,
            IDetectionHandler detectionHandler = null,
            IContentRepository contentRepository = null,
            IRedirectHandler redirectHandler = null,
            ISubscriptionService<Subscription, SubscriptionUser2> subscriptionService = null,
            IServicePlusFacade servicePlusFacade = null)
        {
            var campaignPageController = new CampaignPageController(
                servicePlusHandler ?? GetFakeServicePlusHandler().Object,
                urlHelper ?? GetFakeUrlHelper().Object,
                logService ?? FakeService.GetFakeLogService().Object,
                pageSpecificSession ?? GetFakePageSpecificSession().Object,
                detectionHandler ?? GetFakeDetectionHandler().Object,
                contentRepository ?? GetFakeContentRepository().Object,
                subscriptionService ?? GetFakeSubscriptionService().Object,
                redirectHandler ?? GetFakeRedirectHandler().Object,
                servicePlusFacade ?? GetFakeServicePlusFacade().Object
                );

            return campaignPageController;
        }

        private Mock<IServicePlusFacade> GetFakeServicePlusFacade()
        {
            return new Mock<IServicePlusFacade>(); ;
        }

        private Mock<IRedirectHandler> GetFakeRedirectHandler()
        {
            var redirectHandlerMock = new Mock<IRedirectHandler>();
            return redirectHandlerMock;
        }

        private Mock<IPageSpecificSession> GetFakePageSpecificSession()
        {
            var pageSpecificSessionMock = new Mock<IPageSpecificSession>();
            return pageSpecificSessionMock;
        }

        private Mock<IServicePlusHandler<UserOutput>> GetFakeServicePlusHandler()
        {
            var servicePlusHandlerMock = new Mock<IServicePlusHandler<UserOutput>>();
            return servicePlusHandlerMock;
        }

        private Mock<IUrlHelper> GetFakeUrlHelper()
        {
            var urlHelperMock = new Mock<IUrlHelper>();
            return urlHelperMock;
        }

        private Mock<IDetectionHandler> GetFakeDetectionHandler()
        {
            var detectionHandlerMock = new Mock<IDetectionHandler>();
            return detectionHandlerMock;
        }

        private Mock<IContentRepository> GetFakeContentRepository()
        {
            var contentRepositoryMock = new Mock<IContentRepository>();
            return contentRepositoryMock;
        }

        private Mock<ISubscriptionService<Subscription, SubscriptionUser2>> GetFakeSubscriptionService()
        {
            var subscriptionServiceMock = new Mock<ISubscriptionService<Subscription, SubscriptionUser2>>();
            return subscriptionServiceMock;
        }

        private CampaignPage GetFakeCurrentPage(int pageId = 1)
        {
            var fakeCurrentPage = new CampaignPage();
            fakeCurrentPage.Property["PageLink"] = new PropertyPageReference(pageId);

            return fakeCurrentPage;
        }

        //TODO: KJ REFACTOR TESTS!

        #region Index

        [TestMethod]
        public void Index_InvokedWithAnyParameters_PostedFormSessionIsCleared()
        {
            var fakeCurrentPage = GetFakeCurrentPage();
            var pageSpecificSessionMock = GetFakePageSpecificSession();
            var servicePlusHandlerMock = GetFakeServicePlusHandler();
            var urlHelperMock = GetFakeUrlHelper();
            var redirectHandlerMock = GetFakeRedirectHandler();

            // Make sure redirectHandler GetCheckLoggedInUrl returns something
            redirectHandlerMock.Setup(redirHandler => redirHandler.GetCheckedLoginUrl(It.IsAny<string>()))
                .Returns("fakeUrl");

            // Make sure urlhelper GetContentUrlWithHost and AddAllExistingQuerystrings returns something
            urlHelperMock.Setup(urlhelper => urlhelper.GetContentUrlWithHost(It.IsAny<ContentReference>(), false))
                .Returns("fakeUrl");

            var campaignPageController = GetCampaignPageController(
                servicePlusHandler: servicePlusHandlerMock.Object,
                urlHelper: urlHelperMock.Object, 
                pageSpecificSession: pageSpecificSessionMock.Object,
                redirectHandler: redirectHandlerMock.Object);

            campaignPageController.Index(fakeCurrentPage, null, null, null, null, null);

            pageSpecificSessionMock.Verify(pss => pss.ClearSession(fakeCurrentPage, "postedform"), Times.Once);
        }

        [TestMethod]
        public void Index_IsInEditMode_NoAuthenticationCheckIsMade()
        {
            //var fakeCurrentPage = GetFakeCurrentPage();
            //var servicePlusHandlerMock = GetFakeServicePlusHandler();
            //var detectionHandlerMock = GetFakeDetectionHandler();

            //// Make sure detectionhandler returns is in edit mode
            //detectionHandlerMock.Setup(detection => detection.IsInEditMode()).Returns(true);

            //var campaignPageController = GetCampaignPageController(
            //    servicePlusHandler: servicePlusHandlerMock.Object, 
            //    detectionHandler: detectionHandlerMock.Object);

            //campaignPageController.Index(fakeCurrentPage, null, null, null, null);

            //servicePlusHandlerMock.Verify(sPlusHandler => sPlusHandler.GetCheckLoggedInUrl(It.IsAny<string>()), Times.Never);

            var fakeCurrentPage = GetFakeCurrentPage();
            var redirectHandlerMock = GetFakeRedirectHandler();
            var detectionHandlerMock = GetFakeDetectionHandler();

            // Make sure detectionhandler returns is in edit mode
            detectionHandlerMock.Setup(detection => detection.IsInEditMode()).Returns(true);

            var campaignPageController = GetCampaignPageController(
                redirectHandler: redirectHandlerMock.Object,
                detectionHandler: detectionHandlerMock.Object);

            campaignPageController.Index(fakeCurrentPage, null, null, null, null, null);

            redirectHandlerMock.Verify(redirHandler => redirHandler.GetCheckedLoginUrl(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void Index_ValueInCallBackParameter_SavesCallBackParameterInSession()
        {
            var fakeCallBackValue = "http://www.di.se";

            var servicePlusHandlerMock = GetFakeServicePlusHandler();
            var urlHelperMock = GetFakeUrlHelper();
            //var sessionDataMock = GetFakeSessionData();
            var pageSpecificSessionMock = GetFakePageSpecificSession();
            var detectionHandlerMock = GetFakeDetectionHandler();
            var redirectHandlerMock = GetFakeRedirectHandler();

            var fakeCurrentPage = GetFakeCurrentPage();
           
            // Make sure redirectHandler GetCheckLoggedInUrl returns something
            redirectHandlerMock.Setup(redirHandler => redirHandler.GetCheckedLoginUrl(It.IsAny<string>()))
                .Returns("fakeUrl");

            // Make sure urlhelper GetContentUrlWithHost and AddAllExistingQuerystrings returns something
            urlHelperMock.Setup(urlhelper => urlhelper.GetContentUrlWithHost(It.IsAny<ContentReference>(), false))
                .Returns("fakeUrl");

            // Make sure detectionhandler returns not in edit mode
            detectionHandlerMock.Setup(detection => detection.IsInEditMode()).Returns(false);

            // Make sure there is nothing returned from session
            //sessionDataMock.Setup(sessionData => sessionData.Get("1_queryparametersdictionary", null))
            //    .Returns(null);

            pageSpecificSessionMock.Setup(
                pss => pss.AddQueryParameterToSession(It.IsAny<IContent>(), It.IsAny<string>(), It.IsAny<string>()))
                .Callback((IContent currentPage, string key, string value) =>
                {
                    Assert.AreEqual("callBack", key);
                    Assert.AreEqual(fakeCallBackValue, value);
                });

            // Make sure provided callback paremeter is saved in session
            //sessionDataMock.Setup(sessionData => sessionData.Set("1_queryparametersdictionary", It.IsAny<Dictionary<string, string>>(), null))
            //    .Callback((string key, object value, HttpContextBase httpContextBase) =>
            //    {
            //        Assert.IsTrue(((Dictionary<string, string>)value).ContainsKey("callBack"));
            //        Assert.AreEqual(fakeCallBackValue, ((Dictionary<string, string>)value)["callBack"]);
            //    });

            var campaignPageController = GetCampaignPageController(
                servicePlusHandler: servicePlusHandlerMock.Object,
                urlHelper: urlHelperMock.Object,
                pageSpecificSession: pageSpecificSessionMock.Object,
                detectionHandler: detectionHandlerMock.Object,
                redirectHandler: redirectHandlerMock.Object);

            campaignPageController.Index(fakeCurrentPage, null, null, null, fakeCallBackValue, null);

            // Make sure sessiondata Set is only called once with the queryparameter dictionary key
            pageSpecificSessionMock.Verify(pss => pss.AddQueryParameterToSession(fakeCurrentPage, "callBack", fakeCallBackValue), Times.Once);
            //sessionDataMock.Verify(sessionData => sessionData.Set("1_queryparametersdictionary", It.IsAny<Dictionary<string, string>>(), null), Times.Once);
        }
        #endregion

        #region CreateSubscription
        [TestMethod]
        public void CreateSubscription_IsPayWallCampaign_AddsServicePlusOffer()
        {
            var subscriptionServiceMock = GetFakeSubscriptionService();

            var pageSpecificSessionMock = GetFakePageSpecificSession();

            var fakeCurrentPage = GetFakeCurrentPage();

            var fakeSubscription = new Subscription();
            fakeSubscription.Subscriber = new Person{ServicePlusUserToken = "fakeToken"};

            pageSpecificSessionMock.Setup(pss => pss.GetFromSession<Subscription>(fakeCurrentPage, "subscription"))
                .Returns(fakeSubscription);

            var fakePostedForm = new CampaignSubscribeFormModel() {IsPayWall = true};

            pageSpecificSessionMock.Setup(pss => pss.GetFromSession<CampaignSubscribeFormModel>(fakeCurrentPage, "postedform"))
                .Returns(fakePostedForm);

            pageSpecificSessionMock.Setup(pss => pss.GetFromSession<NetsCardPayPrepare>(fakeCurrentPage, "netscardpayprepare"))
                .Returns((NetsCardPayPrepare) null);

            pageSpecificSessionMock.Setup(pss => pss.GetFromSession<NetsCardPayReturn>(fakeCurrentPage, "netscardpayreturn"))
                .Returns((NetsCardPayReturn) null);

            // Make subscriptionService SaveSubscription return no errors
            subscriptionServiceMock.Setup(ss => ss.SaveSubscription(It.IsAny<Subscription>(), null, It.IsAny<PaymentMethod.TypeOfPaymentMethod>(), null,false))
                .Returns(string.Empty);

            var servicePlusFacadeMock = GetFakeServicePlusFacade();
            servicePlusFacadeMock.Setup(spf => spf.CreateOrUpdateOffer(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(It.IsAny<bool>);

            var campaignPageController = GetCampaignPageController(
                servicePlusFacade: servicePlusFacadeMock.Object,
                pageSpecificSession: pageSpecificSessionMock.Object,
                subscriptionService: subscriptionServiceMock.Object);

            campaignPageController.CreateSubscription(fakeCurrentPage);

            servicePlusFacadeMock.Verify(spf => spf.CreateOrUpdateOffer(It.IsAny<string>(), It.IsAny<string>()), Times.Once);

            
        }

        [TestMethod]
        public void CreateSubscription_IsNotPayWallCampaign_DoesNotAddServicePlusOffer()
        {
            var subscriptionServiceMock = GetFakeSubscriptionService();

            var pageSpecificSessionMock = GetFakePageSpecificSession();

            var fakeCurrentPage = GetFakeCurrentPage();

            var fakeSubscription = new Subscription();
            fakeSubscription.Subscriber = new Person { ServicePlusUserToken = "fakeToken" };

            pageSpecificSessionMock.Setup(pss => pss.GetFromSession<Subscription>(fakeCurrentPage, "subscription"))
                .Returns(fakeSubscription);

            var fakePostedForm = new CampaignSubscribeFormModel() { IsPayWall = false };

            pageSpecificSessionMock.Setup(pss => pss.GetFromSession<CampaignSubscribeFormModel>(fakeCurrentPage, "postedform"))
                .Returns(fakePostedForm);

            pageSpecificSessionMock.Setup(pss => pss.GetFromSession<NetsCardPayPrepare>(fakeCurrentPage, "netscardpayprepare"))
                .Returns((NetsCardPayPrepare)null);

            pageSpecificSessionMock.Setup(pss => pss.GetFromSession<NetsCardPayReturn>(fakeCurrentPage, "netscardpayreturn"))
                .Returns((NetsCardPayReturn)null);

            // Make subscriptionService SaveSubscription return no errors
            subscriptionServiceMock.Setup(ss => ss.SaveSubscription(It.IsAny<Subscription>(), null, It.IsAny<PaymentMethod.TypeOfPaymentMethod>(), null,false))
                .Returns(string.Empty);

            var servicePlusFacadeMock = GetFakeServicePlusFacade();
            servicePlusFacadeMock.Setup(spf => spf.CreateOrUpdateOffer(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(It.IsAny<bool>);

            var campaignPageController = GetCampaignPageController(
                servicePlusFacade: servicePlusFacadeMock.Object,
                pageSpecificSession: pageSpecificSessionMock.Object,
                subscriptionService: subscriptionServiceMock.Object);

            campaignPageController.CreateSubscription(fakeCurrentPage);

            servicePlusFacadeMock.Verify(spf => spf.CreateOrUpdateOffer(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        #endregion

        #region ThankYou

        [TestMethod]
        public void ThankYou_CallBackParameterSavedInSession_SetsCallBackParameterValueInViewBag()
        {
            //var sessionDataMock = GetFakeSessionData();

            var pageSpecificSessionMock = GetFakePageSpecificSession();

            var fakeCurrentPage = GetFakeCurrentPage();

            var fakeQueryParameterDictionary = new Dictionary<string, string>();
            fakeQueryParameterDictionary.Add("callBack", "fakeCallBackUrl");

            //sessionDataMock.Setup(sessionData => sessionData.Get("1_queryparametersdictionary", null))
            //    .Returns(fakeQueryParameterDictionary);

            pageSpecificSessionMock.Setup(pss => pss.GetQueryParameterFromSession(fakeCurrentPage, "callBack"))
                .Returns("fakeCallBackUrl");

            var campaignPageController = GetCampaignPageController(pageSpecificSession: pageSpecificSessionMock.Object);

            campaignPageController.ThankYou(fakeCurrentPage);

            Assert.AreEqual("fakeCallBackUrl", campaignPageController.ViewBag.CallBackUrl);
        }
        #endregion
    }
}
