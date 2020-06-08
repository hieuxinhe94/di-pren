using System;
using System.Globalization;
using DIClassLib.Subscriptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pren.Web.Business.Detection;
using Pren.Web.Business.Subscription;

namespace Pren.Web.UnitTests.Business.Detection
{
    [TestClass]
    public class DetectionUnitTests
    {
        private Mock<ISubscriptionUser<SubscriptionUser2>> GetFakeSubscriptionUser()
        {
            var subscriptionUserMock = new Mock<ISubscriptionUser<SubscriptionUser2>>();

            return subscriptionUserMock;
        }

        private IDetectionHandler GetDetectionHandler()
        {
            return new DetectionHandler();
        }

        //UnitOfWork_StateUnderTest_ExcpectedBehaviour

        [TestMethod]
        public void IsNumeric_IsNumber_ReturnsTrue()
        {
            var detectionHandler = GetDetectionHandler();
                
            Assert.AreEqual(true, detectionHandler.IsNumeric("15864"));
        }

        [TestMethod]
        public void IsNumeric_NotNumber_ReturnsFalse()
        {
            var detectionHandler = GetDetectionHandler();

            Assert.AreEqual(false, detectionHandler.IsNumeric("abc123"));
        }

        [TestMethod]
        public void IsValidSwePhoneNum_ValidSwePhoneNumber_ReturnsTrue()
        {
            var detectionHandler = GetDetectionHandler();

            Assert.AreEqual(true, detectionHandler.IsValidSwePhoneNum("+46708747006"));
        }

        [TestMethod]
        public void IsValidSwePhoneNum_NotValidSwePhoneNumber_ReturnsFalse()
        {
            var detectionHandler = GetDetectionHandler();

            Assert.AreEqual(false, detectionHandler.IsValidSwePhoneNum("0708747006"));
        }

        [TestMethod]
        public void IsValidEmail_ValidEmails_ReturnsTrue()
        {
            var detectionHandler = GetDetectionHandler();

            Assert.AreEqual(true, detectionHandler.IsValidEmail("thorjorn.karlstrom@di.se"));
            Assert.AreEqual(true, detectionHandler.IsValidEmail("_t_k_m_@di.se"));
            Assert.AreEqual(true, detectionHandler.IsValidEmail("__@di.se"));
        }

        [TestMethod]
        public void IsValidEmail_NotValidEmails_ReturnsFalse()
        {
            var detectionHandler = GetDetectionHandler();

            Assert.AreEqual(false, detectionHandler.IsValidEmail("thorjorn.@di.se"));
            Assert.AreEqual(false, detectionHandler.IsValidEmail("thorben@.di.se"));
            Assert.AreEqual(false, detectionHandler.IsValidEmail(".thorben@di.se"));
        }

    }
}
