using System.Collections.Generic;
using System.Web;
using EPiServer.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pren.Web.Business.Session;
using Pren.Web.UnitTests.Fakes;
using Pren.Web.UnitTests.Fakes.Content;

namespace Pren.Web.UnitTests.Business.Session
{
    [TestClass]
    public class PageSpecificSessionUnitTests
    {
        private PageSpecificSession GetPageSpecificSession(ISessionData sessionData)
        {
            return new PageSpecificSession(sessionData ?? FakeService.GetFakeSessionData().Object);
        }

        [TestMethod]
        public void AddQueryParameterToSession_QueryParameterDictionaryDoesNotExistInSession_QueryParameterDictionaryWithGivenKeyAndValueGetsAddedInSession()
        {
            var fakeQueryParameterKey = "fakeQueryParameterKey";
            var fakeQueryParameterValue = "fakeQueryParameterValue";
            var fakeCurrentPage = FakeContentFactory.GetFakePageContent<PageData>();
            var sessionKey = fakeCurrentPage.ContentLink.ID + "_" + SessionConstants.QueryParameterDictionarySessionKey;
            var sessionDataMock = FakeService.GetFakeSessionData();

            // Make sure there is nothing returned from session
            sessionDataMock.Setup(sessionData => sessionData.Get(sessionKey, null))
                .Returns(null);

            sessionDataMock.Setup(sessionData => sessionData.Set(sessionKey, It.IsAny<Dictionary<string, string>>(), null))
                .Callback((string key, object value, HttpContextBase httpContextBase) =>
                {
                    Assert.IsTrue(((Dictionary<string, string>)value).ContainsKey(fakeQueryParameterKey));
                    Assert.AreEqual(fakeQueryParameterValue, ((Dictionary<string, string>)value)[fakeQueryParameterKey]);
                });

            var pageSpecificSession = GetPageSpecificSession(sessionDataMock.Object);

            pageSpecificSession.AddQueryParameterToSession(fakeCurrentPage, fakeQueryParameterKey, fakeQueryParameterValue);

            // Make sure sessiondata Set is only called once with the queryparameter dictionary key
            sessionDataMock.Verify(sessionData => sessionData.Set(sessionKey, It.IsAny<Dictionary<string, string>>(), null), Times.Once);
        }

        [TestMethod]
        public void AddQueryParameterToSession_QueryParameterDictionaryExistsInSessionWithGivenKey_QueryParameterDictionaryValueWithGivenKeyGetsOverwrittenInSession()
        {
            var fakeQueryParameterKey = "fakeQueryParameterKey";
            var fakeQueryParameterValue = "fakeQueryParameterValue";
            var fakeCurrentPage = FakeContentFactory.GetFakePageContent<PageData>();
            var sessionKey = fakeCurrentPage.ContentLink.ID + "_" + SessionConstants.QueryParameterDictionarySessionKey;
            var sessionDataMock = FakeService.GetFakeSessionData();

            var fakeQueryParameterDictionary = new Dictionary<string, string>();
            fakeQueryParameterDictionary.Add(fakeQueryParameterKey, "existingvalue");

            sessionDataMock.Setup(sessionData => sessionData.Get(sessionKey, null))
                .Returns(fakeQueryParameterDictionary);

            sessionDataMock.Setup(sessionData => sessionData.Set(sessionKey, It.IsAny<Dictionary<string, string>>(), null))
                .Callback((string key, object value, HttpContextBase httpContextBase) =>
                {
                    Assert.IsTrue(((Dictionary<string, string>)value).ContainsKey(fakeQueryParameterKey));
                    Assert.AreEqual(fakeQueryParameterValue, ((Dictionary<string, string>)value)[fakeQueryParameterKey]);
                });

            var pageSpecificSession = GetPageSpecificSession(sessionDataMock.Object);

            pageSpecificSession.AddQueryParameterToSession(fakeCurrentPage, fakeQueryParameterKey, fakeQueryParameterValue);

            // Make sure sessiondata Set is only called once with the queryparameter dictionary key
            sessionDataMock.Verify(sessionData => sessionData.Set(sessionKey, It.IsAny<Dictionary<string, string>>(), null), Times.Once);
        }

        [TestMethod]
        public void GetQueryParameterFromSession_QueryParameterDictionaryDoesNotExistInSession_ReturnsEmptyString()
        {
            var fakeQueryParameterKey = "fakeQueryParameterKey";
            var fakeCurrentPage = FakeContentFactory.GetFakePageContent<PageData>();
            var sessionKey = fakeCurrentPage.ContentLink.ID + "_" + SessionConstants.QueryParameterDictionarySessionKey;
            var sessionDataMock = FakeService.GetFakeSessionData();

            sessionDataMock.Setup(sessionData => sessionData.Get(sessionKey, null))
                .Returns(null);

            var pageSpecificSession = GetPageSpecificSession(sessionDataMock.Object);

            var queryParemeter = pageSpecificSession.GetQueryParameterFromSession(fakeCurrentPage, fakeQueryParameterKey);

            Assert.AreEqual(string.Empty, queryParemeter);
        }

        [TestMethod]
        public void GetQueryParameterFromSession_QueryParameterDictionaryExistsInSessionButNotWithGivenKey_ReturnsEmptyString()
        {
            var fakeQueryParameterKey = "fakeQueryParameterKey";
            var fakeCurrentPage = FakeContentFactory.GetFakePageContent<PageData>();
            var sessionKey = fakeCurrentPage.ContentLink.ID + "_" + SessionConstants.QueryParameterDictionarySessionKey;
            var sessionDataMock = FakeService.GetFakeSessionData();

            var fakeQueryParameterDictionary = new Dictionary<string, string>();
            fakeQueryParameterDictionary.Add("anotherkey", "existingvalue");

            sessionDataMock.Setup(sessionData => sessionData.Get(sessionKey, null))
                .Returns(fakeQueryParameterDictionary);

            var pageSpecificSession = GetPageSpecificSession(sessionDataMock.Object);

            var queryParemeter = pageSpecificSession.GetQueryParameterFromSession(fakeCurrentPage, fakeQueryParameterKey);

            Assert.AreEqual(string.Empty, queryParemeter);
        }

        [TestMethod]
        public void GetQueryParameterFromSession_QueryParameterDictionaryExistsInSessionWithUrlEncodedValueForGivenKey_ReturnsUrlDecodedValue()
        {
            var fakeQueryParameterKey = "fakeQueryParameterKey";
            var fakeQueryParameterValue = "http://www.di.se/";
            var fakeCurrentPage = FakeContentFactory.GetFakePageContent<PageData>();
            var sessionKey = fakeCurrentPage.ContentLink.ID + "_" + SessionConstants.QueryParameterDictionarySessionKey;
            var sessionDataMock = FakeService.GetFakeSessionData();

            var fakeQueryParameterDictionary = new Dictionary<string, string>();
            fakeQueryParameterDictionary.Add(fakeQueryParameterKey, HttpUtility.UrlEncode(fakeQueryParameterValue));

            sessionDataMock.Setup(sessionData => sessionData.Get(sessionKey, null))
                .Returns(fakeQueryParameterDictionary);

            var pageSpecificSession = GetPageSpecificSession(sessionDataMock.Object);

            var queryParemeter = pageSpecificSession.GetQueryParameterFromSession(fakeCurrentPage, fakeQueryParameterKey);

            Assert.AreEqual(fakeQueryParameterValue, queryParemeter);
        }
    }
}
