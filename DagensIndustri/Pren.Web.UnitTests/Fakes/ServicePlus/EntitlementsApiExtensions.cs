using System;
using Di.ServicePlus.RestApi.Requests.Entitlements;
using Di.ServicePlus.RestApi.ResponseModels.Entitlement;
using Moq;
using Moq.Language.Flow;

namespace Pren.Web.UnitTests.Fakes.ServicePlus
{
    static class EntitlementsApiExtensions
    {
        private static ISetup<IEntitlements, EntitlementResponse> CreateGetEntitlementSetup(this Mock<IEntitlements> entitlements)
        {
            return entitlements.Setup(x => x.GetEntitlement(It.IsAny<string>(), It.IsAny<string>()));
        }

        public static Mock<IEntitlements> WithGetEntitlement(this Mock<IEntitlements> entitlements, EntitlementResponse entitlementResponseToReturn)
        {
            entitlements.CreateGetEntitlementSetup()
                .Returns(entitlementResponseToReturn);

            return entitlements;
        }

        public static Mock<IEntitlements> WithGetEntitlementThatReturnsEntitlementWithStateValid(this Mock<IEntitlements> entitlements)
        {
            entitlements.WithGetEntitlement(EntitlementsResponseFactory.FakeEntitlementResponseEntitlementStateValid());

            return entitlements;
        }

        public static Mock<IEntitlements> WithGetEntitlementThatReturnsEntitlementWithStateInvalid(this Mock<IEntitlements> entitlements)
        {
            entitlements.WithGetEntitlement(EntitlementsResponseFactory.FakeEntitlementResponseEntitlementStateInvalid());

            return entitlements;
        }

        public static Mock<IEntitlements> WithGetEntitlementThatReturnsEntitlementWithStateSyncing(this Mock<IEntitlements> entitlements)
        {
            entitlements.WithGetEntitlement(EntitlementsResponseFactory.FakeEntitlementResponseEntitlementStateSyncing());

            return entitlements;
        }

        public static Mock<IEntitlements> WithGetEntitlementThatReturnsEntitlementWithUnhandledState(this Mock<IEntitlements> entitlements)
        {
            entitlements.WithGetEntitlement(EntitlementsResponseFactory.FakeEntitlementResponseEntitlementStateUnhandled());

            return entitlements;
        }

        public static Mock<IEntitlements> WithGetEntitlementThatReturnsEntitlementWithValidToDateInPast(this Mock<IEntitlements> entitlements)
        {
            entitlements.WithGetEntitlement(EntitlementsResponseFactory.FakeEntitlementResponseEntitlementValidToDateInPast());

            return entitlements;
        }

        public static Mock<IEntitlements> WithGetEntitlementThatReturnsEntitlementWithValidToDateInFuture(this Mock<IEntitlements> entitlements)
        {
            entitlements.WithGetEntitlement(EntitlementsResponseFactory.FakeEntitlementResponseEntitlementValidToDateInFuture());

            return entitlements;
        }

        public static Mock<IEntitlements> WithGetEntitlementThatReturnsEntitlementWithValidFromDateInPast(this Mock<IEntitlements> entitlements)
        {
            entitlements.WithGetEntitlement(EntitlementsResponseFactory.FakeEntitlementResponseEntitlementValidFromDateInPast());

            return entitlements;
        }

        public static Mock<IEntitlements> WithGetEntitlementThatReturnsEntitlementWithValidFromDateInFuture(this Mock<IEntitlements> entitlements)
        {
            entitlements.WithGetEntitlement(EntitlementsResponseFactory.FakeEntitlementResponseEntitlementValidFromDateInFuture());

            return entitlements;
        }

        public static Mock<IEntitlements> WithGetEntitlementThatReturnsEntitlementNotFound(this Mock<IEntitlements> entitlements)
        {
            entitlements.WithGetEntitlement(EntitlementsResponseFactory.FakeEntitlementResponseNotFound());

            return entitlements;
        }

        public static Mock<IEntitlements> WithGetEntitlementThatThrowsException(this Mock<IEntitlements> entitlements)
        {
            entitlements.CreateGetEntitlementSetup()
                .Throws(new Exception("FakeException"));

            return entitlements;
        }
    }
}
