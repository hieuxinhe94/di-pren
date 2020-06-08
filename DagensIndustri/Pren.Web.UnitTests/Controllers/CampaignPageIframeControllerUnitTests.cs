using System.Web.Mvc;
using Di.Common.Logging;
using Di.Subscription.Logic.Campaign;
using EPiServer;
using EPiServer.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pren.Web.Business.Configuration;
using Pren.Web.Business.DataAccess;
using Pren.Web.Business.Detection;
using Pren.Web.Business.Helpers;
using Pren.Web.Business.Package;
using Pren.Web.Business.ServicePlus.Logic;
using Pren.Web.Controllers;
using Pren.Web.Models.Blocks;
using Pren.Web.Models.CustomForms;
using Pren.Web.Models.Pages;
using Pren.Web.Models.ViewModels;
using Pren.Web.UnitTests.Fakes;
using Pren.Web.UnitTests.Fakes.Content;

namespace Pren.Web.UnitTests.Controllers
{
    [TestClass]
    public class CampaignPageIframeControllerUnitTests
    {
        private const int CampaignContentId = 1;

        private CampaignPageIframe _fakeCurrentPage;

        private CampaignPageIframeController GetCampaignPageIframeController(
            ILogger logService = null,
            IContentRepository contentRepository = null,
            IContentLoader contentLoader = null)
        {
            var campignPageIframeController = new CampaignPageIframeController(
                    logService ?? FakeService.GetFakeLogService().Object,
                    new Mock<IDetectionHandler>().Object,
                    contentRepository ?? new Mock<IContentRepository>().Object,
                    new Mock<ISiteSettings>().Object,
                    new Mock<ICampaignHandler>().Object,
                    new Mock<IDataAccess>().Object,
                    contentLoader ?? new Mock<IContentLoader>().Object,
                    new Mock<IPackageRelationManager>().Object,
                    new Mock<IAntiForgeryValidator>().Object,
                    new Mock<IServicePlusFacade>().Object,
                    new Mock<IUrlHelper>().Object
                );

            return campignPageIframeController;
        } 

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeCurrentPage = FakeContentFactory.GetFakePageContent<CampaignPageIframe>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _fakeCurrentPage = null;
        }

        [TestMethod]
        public void Index_OfferOriginIsProvided_ProvidedOfferOriginIsReturnedToTheView()
        {
            const string offerOrigin = "test-offer-origin";

            var controller = GetCampaignPageIframeController();

            var actionResult = controller.Index(_fakeCurrentPage, null, null, offerOrigin, null, null);

            var viewModel = (CampaignPageOrderViewModel)((ViewResult)actionResult).Model;
            
            Assert.AreEqual(offerOrigin, viewModel.OfferOrigin);
        }

        [TestMethod]
        public void Index_OfferOriginIsNotProvided_EmptyOfferOriginIsReturnedToTheView()
        {
            var controller = GetCampaignPageIframeController();

            var actionResult = controller.Index(_fakeCurrentPage, null, null, null, null, null);

            var viewModel = (CampaignPageOrderViewModel)((ViewResult)actionResult).Model;

            Assert.AreEqual(null, viewModel.OfferOrigin);
        }

        [TestMethod]
        public void Order_NotPayWallCampaign_OfferOriginIsSetToDefault()
        {
            var controller = SetupControllerForOriginTests(true, false);

            var postedForm = GetPostedForm();

            var actionResult = controller.Order(_fakeCurrentPage, postedForm);

            var viewModel = (CampaignPageOrderViewModel) ((ViewResult) actionResult).Model;

            Assert.AreEqual("di-pren", viewModel.Parameters["origin"]);
        }

        [TestMethod]
        public void Order_NotPayWallCampaignAndPostedFormHasOfferOrigin_OfferOriginIsSetToDefault()
        {
            var controller = SetupControllerForOriginTests(true, false);

            var postedForm = GetPostedForm(offerOrigin: "offer-origin");

            var actionResult = controller.Order(_fakeCurrentPage, postedForm);

            var viewModel = (CampaignPageOrderViewModel)((ViewResult)actionResult).Model;

            Assert.AreEqual("di-pren", viewModel.Parameters["origin"]);
        }

        [TestMethod]
        public void Order_PayWallCampaignAndPostedFormHasOfferOrigin_OfferOriginIsSetToPostedFormOfferOrigin()
        {
            var controller = SetupControllerForOriginTests(true, true);

            var postedForm = GetPostedForm(offerOrigin: "offer-origin");

            var actionResult = controller.Order(_fakeCurrentPage, postedForm);

            var viewModel = (CampaignPageOrderViewModel)((ViewResult)actionResult).Model;

            Assert.AreEqual(postedForm.OfferOrigin, viewModel.Parameters["origin"]);
        }

        [TestMethod]
        public void Order_PayWallCampaignAndPostedFormDoesNotHaveOfferOrigin_OfferOriginIsSetToPlusFlowUnknownSite()
        {
            var controller = SetupControllerForOriginTests(true, true);

            var postedForm = GetPostedForm();

            var actionResult = controller.Order(_fakeCurrentPage, postedForm);

            var viewModel = (CampaignPageOrderViewModel)((ViewResult)actionResult).Model;

            Assert.AreEqual("di-pren-plusflow-unknown-site", viewModel.Parameters["origin"]);
        }

        [TestMethod]
        public void Order_StudentCampaign_StudentParamIsSetToTrue()
        {
            var controller = SetupControllerForStudentTests(true, true);

            var postedForm = GetPostedForm();

            var actionResult = controller.Order(_fakeCurrentPage, postedForm);

            var viewModel = (CampaignPageOrderViewModel)((ViewResult)actionResult).Model;

            Assert.AreEqual("true", viewModel.Parameters["student"]);
        }

        [TestMethod]
        public void Order_NotStudentCampaign_StudentParamIsSetToFalse()
        {
            var controller = SetupControllerForStudentTests(true, false);

            var postedForm = GetPostedForm();

            var actionResult = controller.Order(_fakeCurrentPage, postedForm);

            var viewModel = (CampaignPageOrderViewModel)((ViewResult)actionResult).Model;

            Assert.AreEqual("false", viewModel.Parameters["student"]);
        }

        [TestMethod]
        public void Order_AnyCampaign_TrackingScriptUrlParamIsSetAndEndsWithScriptAction()
        {
            var fakeStartPage = FakeContentFactory.GetFakePageContent<StartPage>(3)
                .WithPagePropertyValue(x => x.SendTrackingUrlParameter, true);

            var controller = SetupController(fakeStartPage);

            var postedForm = GetPostedForm();

            var actionResult = controller.Order(_fakeCurrentPage, postedForm);

            var viewModel = (CampaignPageOrderViewModel)((ViewResult)actionResult).Model;

            Assert.IsTrue(viewModel.Parameters["tracking-script-url"].EndsWith(UrlConstants.ScriptAction));
        }

        [TestMethod]
        public void Order_AnyCampaign_DefaultParamsAreSet()
        {
            var controller = SetupController();

            var postedForm = GetPostedForm();

            var actionResult = controller.Order(_fakeCurrentPage, postedForm);

            var viewModel = (CampaignPageOrderViewModel)((ViewResult)actionResult).Model;

            Assert.AreEqual("di.se", viewModel.Parameters["appId"]);
            Assert.AreEqual("sv", viewModel.Parameters["lc"]);
        }

        [TestMethod]
        public void Order_CallbackIsProvidedInPostedForm_CallbackIsSetToProvidedCallback()
        {
            const string callback = "providedcallback";

            var controller = SetupController();

            var postedForm = GetPostedForm(callback: callback);

            var actionResult = controller.Order(_fakeCurrentPage, postedForm);

            var viewModel = (CampaignPageOrderViewModel)((ViewResult)actionResult).Model;

            Assert.AreEqual(callback, viewModel.Parameters["callback"]);
        }

        [TestMethod]
        public void Order_CallbackIsNotProvidedInPostedForm_CallbackIsSetToDefaultCallback()
        {
            const string callback = "defaultcallback";

            var fakeStartPage = FakeContentFactory.GetFakePageContent<StartPage>(3)
                .WithPagePropertyValue(x => x.DefaultCallbackUrl, "defaultcallback");

            var controller = SetupController(fakeStartPage);

            var postedForm = GetPostedForm(callback: null);

            var actionResult = controller.Order(_fakeCurrentPage, postedForm);

            var viewModel = (CampaignPageOrderViewModel)((ViewResult)actionResult).Model;

            Assert.AreEqual(callback, viewModel.Parameters["callback"]);
        }

        private CampaignPageIframeController SetupController(
            StartPage startPage = null,
            CampaignBlock campaignBlock = null)
        {
            var fakeStartPage = startPage ?? FakeContentFactory.GetFakePageContent<StartPage>(3);

            var fakeCampaignBlock = campaignBlock ?? FakeContentFactory.GetFakeBlockContent<CampaignBlock>(10);

            var fakeContentLoader = new Mock<IContentLoader>();
            fakeContentLoader.Setup(c => c.Get<StartPage>(ContentReference.StartPage))
                .Returns(fakeStartPage);

            var fakeContentRepo = new Mock<IContentRepository>();
            fakeContentRepo.Setup(c => c.Get<CampaignBlock>(new ContentReference(CampaignContentId)))
                .Returns(fakeCampaignBlock);

            return GetCampaignPageIframeController(contentLoader: fakeContentLoader.Object, contentRepository: fakeContentRepo.Object);
        }

        private CampaignPageIframeController SetupControllerForOriginTests(bool sendOriginParam, bool isPayWallCampaign)
        {            
            var fakeStartPage = FakeContentFactory.GetFakePageContent<StartPage>(3)
                .WithPagePropertyValue(x => x.SendOriginParameter, sendOriginParam);

            var fakeCampaignBlock = FakeContentFactory.GetFakeBlockContent<CampaignBlock>(10)
                .WithBlockPropertyValue(x => x.IsPayWall, isPayWallCampaign);

            return SetupController(fakeStartPage, fakeCampaignBlock);
        }

        private CampaignPageIframeController SetupControllerForStudentTests(bool sendStudentParam, bool isStudentCampaign)
        {
            var fakeStartPage = FakeContentFactory.GetFakePageContent<StartPage>(3)
                .WithPagePropertyValue(x => x.SendStudentParameter, sendStudentParam);

            var fakeCampaignBlock = FakeContentFactory.GetFakeBlockContent<CampaignBlock>(10)
                .WithBlockPropertyValue(x => x.IsStudent, isStudentCampaign);

            return SetupController(fakeStartPage, fakeCampaignBlock);
        }

        private CampaignPageOrderFormModel GetPostedForm(
            int campaignContentId = CampaignContentId, 
            string offerOrigin = "",
            string callback = "")
        {
            var postedForm = new CampaignPageOrderFormModel
            {
                Callback = callback,
                CampaignContentId = campaignContentId,
                OfferOrigin = offerOrigin ?? "",
                TargetGroup = ""
            };

            return postedForm;
        }
    }
}
