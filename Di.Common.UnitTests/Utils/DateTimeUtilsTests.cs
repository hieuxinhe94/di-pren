using System;
using Di.Common.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Di.Common.UnitTests.Utils
{
    [TestClass]
    public class DateTimeUtilsTests
    {
        [TestMethod]
        public void GetUnixTimeStampInMilliseconds_Date20151026103200_Returns1445851920000()
        {
            var unixTimeInMilliSeconds =
                DateTimeUtils.GetUnixTimeStampInMilliseconds(new DateTime(2015, 10, 26, 10, 32, 00));

            Assert.AreEqual(unixTimeInMilliSeconds, 1445851920000);
        }

        [TestMethod]
        public void GetUnixTimeStampInMilliseconds_Date20121116112200_Returns1353061320000()
        {
            var unixTimeInMilliSeconds =
                DateTimeUtils.GetUnixTimeStampInMilliseconds(new DateTime(2012, 11, 16, 11, 22, 00));

            Assert.AreEqual(unixTimeInMilliSeconds, 1353061320000);
        }

        [TestMethod]
        public void UnixTimeStampToDateTime_TimeStampString1353061320000_Returns20121116112200()
        {
            var dateTime =
                DateTimeUtils.UnixTimeStampToDateTime("1353061320000");

            Assert.AreEqual(dateTime, new DateTime(2012,11,16,11,22,00));
        }

        [TestMethod]
        public void UnixTimeStampToDateTime_TimeStampStringError_ReturnsNull()
        {
            var dateTime =
                DateTimeUtils.UnixTimeStampToDateTime("Error");

            Assert.AreEqual(dateTime, null);
        }

        [TestMethod]
        public void UnixTimeStampToDateTime_TimeStampDouble1445851920000_Returns20151026103200()
        {
            var dateTime =
                DateTimeUtils.UnixTimeStampToDateTime(1445851920000);

            Assert.AreEqual(dateTime, new DateTime(2015, 10, 26, 10, 32, 00));
        }

        [TestMethod]
        public void TryParseToNullableDateTime_NotValidDateTime_ReturnsNull()
        {
            var parsedDate = DateTimeUtils.TryParseToNullableDateTime("not_a_valid_date");

            Assert.IsNull(parsedDate);
        }

        [TestMethod]
        public void TryParseToNullableDateTime_ValidDateTime_ReturnsConvertedDateTime()
        {
            var validDate = "2015-12-30";

            var parsedDate = DateTimeUtils.TryParseToNullableDateTime(validDate);

            Assert.AreEqual(validDate, ((DateTime)parsedDate).ToString("yyyy-MM-dd"));
        }
        
    }
}
