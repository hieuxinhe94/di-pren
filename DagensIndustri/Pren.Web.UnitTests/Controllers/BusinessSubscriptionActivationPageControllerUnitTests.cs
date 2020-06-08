using System;
using System.Web.Mvc;
using Di.Common.Logging;
using EPiServer.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pren.Web.Business.DataAccess;
using Pren.Web.Business.ServicePlus.Logic;
using Pren.Web.Business.ServicePlus.Models;
using Pren.Web.Controllers;
using Pren.Web.Models.Pages;
using Pren.Web.Models.ViewModels;
using Pren.Web.UnitTests.Fakes;

namespace Pren.Web.UnitTests.Controllers
{
    [TestClass]
    public class BusinessSubscriptionActivationPageControllerUnitTests
    {
        private const string FakeBizSubscriptionId = "fakeBizSubscriptionId";
        private const string FakeRegistrationCode = "fakeRegistrationCode";
        private const string FakeInvitedBizSubscriberEmail = "fake@email.se";
        private const string FakeBizSubscriptionCompanyName = "fakecompany";
        private const string FakeExceptionMessage = "FakeException";
        
        private const string LogExceptionInvalidQueryParemetersExcerpt = "Queryparameters are not valid";
        private const string LogExceptionNoSubscriberFoundExceprt = "Could not find subscriber";
        private const string LogExceptionAccountAlreadyExistsExcerpt = "S+ account already exists";
        private const string LogExceptionInvalidPostedFormExcerpt = "Posted form is not valid";

        private BusinessSubscriptionActivationPageController GetBusinessSubscriptionActivationPageController(
            IServicePlusFacade servicePlusFacade = null,
            ILogger logService = null)
        {
            var businessSubscriptionActivationPageController = new BusinessSubscriptionActivationPageController(
                servicePlusFacade ?? FakeService.GetFakeServicePlusFacade().Object,
                logService ?? FakeService.GetFakeLogService().Object,
                new Mock<IDataAccess>().Object);

            return businessSubscriptionActivationPageController;
        }

        private BusinessSubscriptionActivationPage GetFakeCurrentPage(int pageId = 1)
        {
            var fakeCurrentPage = new BusinessSubscriptionActivationPage();
            fakeCurrentPage.Property["PageLink"] = new PropertyPageReference(pageId);

            return fakeCurrentPage;
        }

        [TestMethod]
        public void Index_NoQueryStringParemeters_ModelDisplayErrorMessageIsTrueAndErrorIsLogged()
        {
            AssertMissingQueryStringDisplayErrorMessageIsTrueAndErrorIsLogged(null, null);
        }

        [TestMethod]
        public void Index_NoBizSubscriptionIdQueryStringParemeter_ModelDisplayErrorMessageIsTrueAndErrorIsLogged()
        {
            AssertMissingQueryStringDisplayErrorMessageIsTrueAndErrorIsLogged(null, FakeRegistrationCode);
        }

        [TestMethod]
        public void Index_NoRegistrationCodeQueryStringParemeter_ModelDisplayErrorMessageIsTrueAndErrorIsLogged()
        {
            AssertMissingQueryStringDisplayErrorMessageIsTrueAndErrorIsLogged(FakeBizSubscriptionId, null);
        }

        [TestMethod]
        public void Index_NoInvitedSubscriberFound_ModelDisplayErrorMessageIsTrueAndErrorIsLogged()
        {
            AssertMissingOrInvalidInvitedSubscriberDisplayErrorMessageIsTrueAndErrorIsLogged(null);
        }

        [TestMethod]
        public void Index_InvitedSubscriberFoundButHasNoEmail_ModelDisplayErrorMessageIsTrueAndErrorIsLogged()
        {
            AssertMissingOrInvalidInvitedSubscriberDisplayErrorMessageIsTrueAndErrorIsLogged(new BizSubscriber{Email = ""});
        }

        [TestMethod]
        public void Index_InvitedSubscriberAlreadyExistsInServicePlus_ModelDisplayErrorMessageIsTrueAndErrorIsLogged()
        {
            var logServiceMock = GetLogServiceMockWithExceptionTextCheck(LogExceptionAccountAlreadyExistsExcerpt);

            var servicePlusFacadeStub = GetServicePlusFacadeWithGetBizSubscriberByInviteCodeReturnValue(new BizSubscriber { Email = FakeInvitedBizSubscriberEmail });

            servicePlusFacadeStub = GetServicePlusFacadeWithGetUserByEmailReturnValue(new User {Email = FakeInvitedBizSubscriberEmail}, servicePlusFacadeStub);

            var businessSubscriptionActivationPageController = GetBusinessSubscriptionActivationPageController(servicePlusFacadeStub.Object, logServiceMock.Object);

            var actionResult = businessSubscriptionActivationPageController.Index(GetFakeCurrentPage(), FakeBizSubscriptionId, FakeRegistrationCode);

            var viewModel = (BusinessSubscriptionActivationPageViewModel)((ViewResult)actionResult).Model;

            Assert.IsTrue(viewModel.DisplayErrorMessage);
        }

        [TestMethod]
        public void Index_InvitedSubscriberFoundAndDoesNotExistsInServicePlus_ModelDisplayErrorMessageIsFalse()
        {
            var viewModel = GetViewModelResultWhenInvitedSubscriberFoundAndDoesNotExistsInServicePlus();

            Assert.IsFalse(viewModel.DisplayErrorMessage);
        }

        [TestMethod]
        public void Index_InvitedSubscriberFoundAndDoesNotExistsInServicePlus_ModelActivationFormEmailIsSetToInvitedSubscribersEmail()
        {
            var viewModel = GetViewModelResultWhenInvitedSubscriberFoundAndDoesNotExistsInServicePlus();

            Assert.AreEqual(FakeInvitedBizSubscriberEmail, viewModel.ActivationForm.Email);
        }

        [TestMethod]
        public void Index_InvitedSubscriberFoundAndDoesNotExistsInServicePlus_ModelActivationFormBizSubscriptionIdIsSetToQueryParameterValue()
        {
            var viewModel = GetViewModelResultWhenInvitedSubscriberFoundAndDoesNotExistsInServicePlus();

            Assert.AreEqual(FakeBizSubscriptionId, viewModel.ActivationForm.BizSubscriptionId);
        }

        [TestMethod]
        public void Index_BizSubscriptionFound_ModelActivationFormInvitingCompanyNameIsSetToInvitingCompanyName()
        {
            var servicePlusFacadeStub = GetServicePlusFacadeWithGetBizSubscriberByInviteCodeReturnValue(new BizSubscriber { Email = FakeInvitedBizSubscriberEmail });

            servicePlusFacadeStub = GetServicePlusFacadeWithGetUserByEmailReturnValue(null, servicePlusFacadeStub);

            servicePlusFacadeStub.Setup(sPlusFacade => sPlusFacade.GetBizSubscription(FakeBizSubscriptionId))
                .Returns(new BizSubscription {CompanyName = FakeBizSubscriptionCompanyName});

            var businessSubscriptionActivationPageController = GetBusinessSubscriptionActivationPageController(servicePlusFacadeStub.Object);

            var actionResult = businessSubscriptionActivationPageController.Index(GetFakeCurrentPage(), FakeBizSubscriptionId, FakeRegistrationCode);

            var viewModel = (BusinessSubscriptionActivationPageViewModel)((ViewResult)actionResult).Model;

            Assert.AreEqual(FakeBizSubscriptionCompanyName, viewModel.InvitingCompanyName);
        }

        [TestMethod]
        public void Index_BizSubscriptionNotFound_ModelActivationFormInvitingCompanyNameIsSetToEmptyString()
        {
            var servicePlusFacadeStub = GetServicePlusFacadeWithGetBizSubscriberByInviteCodeReturnValue(new BizSubscriber { Email = FakeInvitedBizSubscriberEmail });

            servicePlusFacadeStub = GetServicePlusFacadeWithGetUserByEmailReturnValue(null, servicePlusFacadeStub);

            servicePlusFacadeStub.Setup(sPlusFacade => sPlusFacade.GetBizSubscription(FakeBizSubscriptionId))
                .Returns((BizSubscription) null);

            var businessSubscriptionActivationPageController = GetBusinessSubscriptionActivationPageController(servicePlusFacadeStub.Object);

            var actionResult = businessSubscriptionActivationPageController.Index(GetFakeCurrentPage(), FakeBizSubscriptionId, FakeRegistrationCode);

            var viewModel = (BusinessSubscriptionActivationPageViewModel)((ViewResult)actionResult).Model;

            Assert.AreEqual(string.Empty, viewModel.InvitingCompanyName);
        }

        [TestMethod]
        public void Index_BizSubscriptionFoundButHasNoCompanyName_ModelActivationFormInvitingCompanyNameIsSetToEmptyString()
        {
            var servicePlusFacadeStub = GetServicePlusFacadeWithGetBizSubscriberByInviteCodeReturnValue(new BizSubscriber { Email = FakeInvitedBizSubscriberEmail });

            servicePlusFacadeStub = GetServicePlusFacadeWithGetUserByEmailReturnValue(null, servicePlusFacadeStub);

            servicePlusFacadeStub.Setup(sPlusFacade => sPlusFacade.GetBizSubscription(FakeBizSubscriptionId))
                .Returns(new BizSubscription());

            var businessSubscriptionActivationPageController = GetBusinessSubscriptionActivationPageController(servicePlusFacadeStub.Object);

            var actionResult = businessSubscriptionActivationPageController.Index(GetFakeCurrentPage(), FakeBizSubscriptionId, FakeRegistrationCode);

            var viewModel = (BusinessSubscriptionActivationPageViewModel)((ViewResult)actionResult).Model;

            Assert.AreEqual(string.Empty, viewModel.InvitingCompanyName);
        }

        [TestMethod]
        public void Index_GetBizSubscriberByInviteCodeThrowsException_ModelDisplayErrorMessageIsTrueAndErrorIsLogged()
        {
            var servicePlusFacadeStub = FakeService.GetFakeServicePlusFacade();

            servicePlusFacadeStub.Setup(sPlusFacade => sPlusFacade.GetBizSubscriberByInviteCode(FakeBizSubscriptionId, FakeRegistrationCode))
                .Throws(new Exception(FakeExceptionMessage));

            var logServiceMock = GetLogServiceMockWithExceptionTextCheck(FakeExceptionMessage);

            var businessSubscriptionActivationPageController = GetBusinessSubscriptionActivationPageController(servicePlusFacadeStub.Object, logServiceMock.Object);

            var actionResult = businessSubscriptionActivationPageController.Index(GetFakeCurrentPage(), FakeBizSubscriptionId, FakeRegistrationCode);

            var viewModel = (BusinessSubscriptionActivationPageViewModel)((ViewResult)actionResult).Model;

            Assert.IsTrue(viewModel.DisplayErrorMessage);
        }

        [TestMethod]
        public void Index_GetBizSubscriberByInviteCodeReturnsNull_ModelDisplayErrorMessageIsTrueAndErrorIsLogged()
        {
            var servicePlusFacadeStub = FakeService.GetFakeServicePlusFacade();

            servicePlusFacadeStub.Setup(sPlusFacade => sPlusFacade.GetBizSubscriberByInviteCode(FakeBizSubscriptionId, FakeRegistrationCode))
                .Returns((BizSubscriber) null);

            var logServiceMock = GetLogServiceMockWithExceptionTextCheck(FakeExceptionMessage);

            var businessSubscriptionActivationPageController = GetBusinessSubscriptionActivationPageController(servicePlusFacadeStub.Object, logServiceMock.Object);

            var actionResult = businessSubscriptionActivationPageController.Index(GetFakeCurrentPage(), FakeBizSubscriptionId, FakeRegistrationCode);

            var viewModel = (BusinessSubscriptionActivationPageViewModel)((ViewResult)actionResult).Model;

            Assert.IsTrue(viewModel.DisplayErrorMessage);
        }

        [TestMethod]
        public void Index_GetBizSubscriberByInviteCodeReturnsNull_ModelInviteExpiredIsTrue()
        {
            var servicePlusFacadeStub = FakeService.GetFakeServicePlusFacade();

            servicePlusFacadeStub.Setup(sPlusFacade => sPlusFacade.GetBizSubscriberByInviteCode(FakeBizSubscriptionId, FakeRegistrationCode))
                .Returns((BizSubscriber)null);

            var businessSubscriptionActivationPageController = GetBusinessSubscriptionActivationPageController(servicePlusFacadeStub.Object);

            var actionResult = businessSubscriptionActivationPageController.Index(GetFakeCurrentPage(), FakeBizSubscriptionId, FakeRegistrationCode);

            var viewModel = (BusinessSubscriptionActivationPageViewModel)((ViewResult)actionResult).Model;

            Assert.IsTrue(viewModel.InviteExpired);
        }

        [TestMethod]
        public void Index_GetUserByEmailThrowsException_ModelDisplayErrorMessageIsTrueAndErrorIsLogged()
        {
            var servicePlusFacadeStub = GetServicePlusFacadeWithGetBizSubscriberByInviteCodeReturnValue(new BizSubscriber{Email = FakeInvitedBizSubscriberEmail});

            servicePlusFacadeStub.Setup(sPlusFacade => sPlusFacade.GetUserByEmail(FakeInvitedBizSubscriberEmail))
                .Throws(new Exception(FakeExceptionMessage));

            var logServiceMock = GetLogServiceMockWithExceptionTextCheck(FakeExceptionMessage);

            var businessSubscriptionActivationPageController = GetBusinessSubscriptionActivationPageController(servicePlusFacadeStub.Object, logServiceMock.Object);

            var actionResult = businessSubscriptionActivationPageController.Index(GetFakeCurrentPage(), FakeBizSubscriptionId, FakeRegistrationCode);

            var viewModel = (BusinessSubscriptionActivationPageViewModel)((ViewResult)actionResult).Model;

            Assert.IsTrue(viewModel.DisplayErrorMessage);
        }

        [TestMethod]
        public void Index_GetBizSubscriptionThrowsException_ModelDisplayErrorMessageIsTrueAndErrorIsLogged()
        {
            var servicePlusFacadeStub = GetServicePlusFacadeWithGetBizSubscriberByInviteCodeReturnValue(new BizSubscriber { Email = FakeInvitedBizSubscriberEmail });

            servicePlusFacadeStub = GetServicePlusFacadeWithGetUserByEmailReturnValue(null, servicePlusFacadeStub);

            servicePlusFacadeStub.Setup(sPlusFacade => sPlusFacade.GetBizSubscription(FakeBizSubscriptionId))
                .Throws(new Exception(FakeExceptionMessage));

            var logServiceMock = GetLogServiceMockWithExceptionTextCheck(FakeExceptionMessage);

            var businessSubscriptionActivationPageController = GetBusinessSubscriptionActivationPageController(servicePlusFacadeStub.Object, logServiceMock.Object);

            var actionResult = businessSubscriptionActivationPageController.Index(GetFakeCurrentPage(), FakeBizSubscriptionId, FakeRegistrationCode);

            var viewModel = (BusinessSubscriptionActivationPageViewModel)((ViewResult)actionResult).Model;

            Assert.IsTrue(viewModel.DisplayErrorMessage);
        }

        [TestMethod]
        public void PostForm_PostedFormIsNull_ModelDisplayErrorMessageIsTrueAndErrorIsLogged()
        {
            var logServiceMock = GetLogServiceMockWithExceptionTextCheck(LogExceptionInvalidPostedFormExcerpt);

            logServiceMock = GetLogServiceMockWithExceptionTextCheck("postedForm is null", logServiceMock);

            var businessSubscriptionActivationPageController = GetBusinessSubscriptionActivationPageController(logService: logServiceMock.Object);

            var actionResult = businessSubscriptionActivationPageController.PostForm(GetFakeCurrentPage(), null);

            var viewModel = (BusinessSubscriptionActivationPageViewModel)((ViewResult)actionResult).Model;

            Assert.IsTrue(viewModel.DisplayErrorMessage);
        }

        private BusinessSubscriptionActivationPageViewModel GetViewModelResultWhenInvitedSubscriberFoundAndDoesNotExistsInServicePlus()
        {
            var servicePlusFacadeStub = GetServicePlusFacadeWithGetBizSubscriberByInviteCodeReturnValue(new BizSubscriber { Email = FakeInvitedBizSubscriberEmail });

            servicePlusFacadeStub = GetServicePlusFacadeWithGetUserByEmailReturnValue(null, servicePlusFacadeStub);

            var businessSubscriptionActivationPageController = GetBusinessSubscriptionActivationPageController(servicePlusFacadeStub.Object);

            var actionResult = businessSubscriptionActivationPageController.Index(GetFakeCurrentPage(), FakeBizSubscriptionId, FakeRegistrationCode);

            var viewModel = (BusinessSubscriptionActivationPageViewModel)((ViewResult)actionResult).Model;

            return viewModel;
        }

        private void AssertMissingOrInvalidInvitedSubscriberDisplayErrorMessageIsTrueAndErrorIsLogged(BizSubscriber getBizSubscriberByInviteCodeReturnValue)
        {
            var logServiceMock = GetLogServiceMockWithExceptionTextCheck(LogExceptionNoSubscriberFoundExceprt);

            var servicePlusFacadeStub = GetServicePlusFacadeWithGetBizSubscriberByInviteCodeReturnValue(getBizSubscriberByInviteCodeReturnValue);

            var businessSubscriptionActivationPageController = GetBusinessSubscriptionActivationPageController(servicePlusFacadeStub.Object, logServiceMock.Object);

            var actionResult = businessSubscriptionActivationPageController.Index(GetFakeCurrentPage(), FakeBizSubscriptionId, FakeRegistrationCode);

            var viewModel = (BusinessSubscriptionActivationPageViewModel)((ViewResult)actionResult).Model;

            Assert.IsTrue(viewModel.DisplayErrorMessage);

            logServiceMock.Verify(logService => logService.Log(It.IsAny<string>(), It.IsAny<LogLevel>(), It.IsAny<Type>()), Times.Once);
        }

        private void AssertMissingQueryStringDisplayErrorMessageIsTrueAndErrorIsLogged(
            string bizSubscriptionId,
            string registrationCode)
        {
            var logServiceMock = GetLogServiceMockWithExceptionTextCheck(LogExceptionInvalidQueryParemetersExcerpt);

            var businessSubscriptionActivationPageController = GetBusinessSubscriptionActivationPageController(logService: logServiceMock.Object);

            // Invoke Index with provided query paremeters
            var actionResult = businessSubscriptionActivationPageController.Index(GetFakeCurrentPage(), bizSubscriptionId, registrationCode);

            var viewModel = (BusinessSubscriptionActivationPageViewModel)((ViewResult)actionResult).Model;

            Assert.IsTrue(viewModel.DisplayErrorMessage);
            logServiceMock.Verify(logService => logService.Log(It.IsAny<string>(), It.IsAny<LogLevel>(), It.IsAny<Type>()), Times.Once);
        }

        private Mock<IServicePlusFacade> GetServicePlusFacadeWithGetBizSubscriberByInviteCodeReturnValue(
            BizSubscriber getBizSubscriberByInviteCodeReturnValue,
            Mock<IServicePlusFacade> servicePlusFacadeMock = null)
        {
            var servicePlusFacadeStub = servicePlusFacadeMock ?? FakeService.GetFakeServicePlusFacade();
            servicePlusFacadeStub.Setup(sPlusFacade => sPlusFacade.GetBizSubscriberByInviteCode(FakeBizSubscriptionId, FakeRegistrationCode))
                .Returns(getBizSubscriberByInviteCodeReturnValue);

            return servicePlusFacadeStub;
        }

        private Mock<IServicePlusFacade> GetServicePlusFacadeWithGetUserByEmailReturnValue(
            User getUserByEmailReturnValue, 
            Mock<IServicePlusFacade> servicePlusFacadeMock = null)
        {
            var servicePlusFacadeStub = servicePlusFacadeMock ?? FakeService.GetFakeServicePlusFacade();
            servicePlusFacadeStub.Setup(sPlusFacade => sPlusFacade.GetUserByEmail(FakeInvitedBizSubscriberEmail))
                .Returns(getUserByEmailReturnValue);

            return servicePlusFacadeStub;
        } 

        private Mock<ILogger> GetLogServiceMockWithExceptionTextCheck(string exceptionTextExcerpt, Mock<ILogger> logServiceMock = null)
        {
            var fakeLogService = logServiceMock ?? FakeService.GetFakeLogService();

            // Make sure LogService.Log is called with proper info in exception text
            fakeLogService.Setup(logService => logService.Log(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<LogLevel>(), null))
                .Callback((Exception exception, string message, LogLevel logLevel, Type type) =>
                    Assert.IsTrue(message.Contains(exceptionTextExcerpt), String.Format("exception did not contain expected value: '{0}'. Actual exception: {1}", exceptionTextExcerpt, message)));

            return fakeLogService;
        } 
    }
}
