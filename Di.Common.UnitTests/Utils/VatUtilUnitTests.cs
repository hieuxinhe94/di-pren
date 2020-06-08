using Di.Common.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Di.Common.UnitTests.Utils
{
    [TestClass]
    public class VatUtilUnitTests
    {
        [TestMethod]
        public void GetAmountIncludingVat_Amount0AndVatPercent0_Returns0()
        {
            var result = VatUtil.GetAmountIncludingVat(0, 0);

            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void GetAmountIncludingVat_Amount100AndVatPercent25_Returns125()
        {
            var result = VatUtil.GetAmountIncludingVat(100, 25);

            Assert.AreEqual(125, result);
        }

        [TestMethod]
        public void GetAmountIncludingVat_Amount100AndVatPercent12_Returns112()
        {
            var result = VatUtil.GetAmountIncludingVat(100, 12);

            Assert.AreEqual(112, result);
        }

        [TestMethod]
        public void GetAmountIncludingVat_Amount100AndVatPercent6_Returns106()
        {
            var result = VatUtil.GetAmountIncludingVat(100, 6);

            Assert.AreEqual(106, result);
        }

        [TestMethod]
        public void GetAmountIncludingVat_Amount100AndVatPercent0_Returns100()
        {
            var result = VatUtil.GetAmountIncludingVat(100, 0);

            Assert.AreEqual(100, result);
        }

        [TestMethod]
        public void GetAmountIncludingVat_NotEvenResultThatShouldBeRoundedDown_ReturnsRoundedDownToClosestInteger()
        {
            var result = VatUtil.GetAmountIncludingVat(101, 6); // 101 * 1.06 = 107.06

            Assert.AreEqual(107, result);
        }

        [TestMethod]
        public void GetAmountIncludingVat_NotEvenResultThatShouldBeRoundedUp_ReturnsRoundedUpToClosestInteger()
        {
            var result = VatUtil.GetAmountIncludingVat(109, 6); // 109 * 1.06 = 115.54

            Assert.AreEqual(116, result);
        }

        [TestMethod]
        public void GetAmountIncludingVat_NotEvenResultExactlyInTheMiddle_ReturnsRoundedUpToClosestInteger()
        {
            var result = VatUtil.GetAmountIncludingVat(1, 50); // 1 * 1.5 = 1.5

            Assert.AreEqual(2, result);
        }

        [TestMethod]
        public void GetAmountIncludingVat_NegativeAmount_Returns0()
        {
            var result = VatUtil.GetAmountIncludingVat(-100, 6);

            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void GetAmountIncludingVat_NegativeVatPercent_Returns0()
        {
            var result = VatUtil.GetAmountIncludingVat(100, -6);

            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void GetAmountIncludingVat_NegativeAmountAndNegativeVatPercent_Returns0()
        {
            var result = VatUtil.GetAmountIncludingVat(-100, -6);

            Assert.AreEqual(0, result);
        }
    }
}
