using System;
using Di.Common.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Di.Common.UnitTests.Utils
{
    [TestClass]
    public class DecimalUtilsTests
    {
        [TestMethod]
        public void TryParseToNullableDecimal_NotValidDecimal_ReturnsNull()
        {
            var parsedDecimal = DecimalUtils.TryParseToNullableDecimal("not_valid_decimal");
            Assert.IsNull(parsedDecimal);
        }

        [TestMethod]
        public void TryParseToNullableDecimal_ValidDecimal_ReturnsConvertedDecimal()
        {
            var parsedDecimal = DecimalUtils.TryParseToNullableDecimal("100.1");
            Assert.AreEqual(new Decimal(100.1), parsedDecimal);
        }

    }
}
