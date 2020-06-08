using System.Collections.Generic;
using Di.ServicePlus.RestApi.ResponseModels.Entitlement;

namespace Pren.Web.UnitTests.Fakes.ServicePlus
{
    static class EntitlementsResponseFactory
    {
        public static EntitlementResponse FakeEntitlementResponseEntitlementStateValid()
        {
            return CreateFakeEntitlementResponse("VALID");
        }

        public static EntitlementResponse FakeEntitlementResponseEntitlementStateInvalid()
        {
            return CreateFakeEntitlementResponse("INVALID");
        }

        public static EntitlementResponse FakeEntitlementResponseEntitlementStateSyncing()
        {
            return CreateFakeEntitlementResponse("SYNCING");
        }

        public static EntitlementResponse FakeEntitlementResponseEntitlementStateUnhandled()
        {
            return CreateFakeEntitlementResponse("DUMMYVALUETHATISNOTHANDLED");
        }

        public static EntitlementResponse FakeEntitlementResponseEntitlementValidToDateInPast()
        {
            return CreateFakeEntitlementResponse(validTo: "1443650400000"); //2015-10-01 00:00:00
        }

        public static EntitlementResponse FakeEntitlementResponseEntitlementValidToDateInFuture()
        {
            return CreateFakeEntitlementResponse(validTo: "3155760000000"); //2070-01-01 00:00:00
        }

        public static EntitlementResponse FakeEntitlementResponseEntitlementValidFromDateInPast()
        {
            return CreateFakeEntitlementResponse(validFrom: "1443650400000"); //2015-10-01 00:00:00
        }

        public static EntitlementResponse FakeEntitlementResponseEntitlementValidFromDateInFuture()
        {
            return CreateFakeEntitlementResponse(validFrom: "3155760000000"); //2070-01-01 00:00:00
        }

        private static EntitlementResponse CreateFakeEntitlementResponse(string state = null, string validFrom = null, string validTo = null)
        {
            return new EntitlementResponse
            {
                HttpResponseCode = "200",
                RequestId = "4yhaZ1KZuUhMMmC9i27y6S",
                Entitlement = new Entitlement
                {
                    Id = "65gd7j0zDPzdK1SkjZXbQk",
                    Created = "1443185387599",
                    Updated = "1445500947419",
                    Location = "/65gd7j0zDPzdK1SkjZXbQk",
                    BrandId = "5DuzcZz0j8u0zArSNzZgHO",
                    ProductId = "6jeaCPI87Tj7ND1OgGyitf",
                    UserId = "1ZNTb68nmJ8hI4nyHbSkoW",
                    Renewable = "false",
                    Type = "PREMIUM_SUBSCRIPTION",
                    State = state ?? "VALID",
                    ProductTags = new List<string> {"DITABLET"},
                    ValidFrom = validFrom ?? "1443132000000",
                    ValidTo = validTo ?? "1443650400000",
                    ExternalSubscriptionId = "7047716",
                    ExternalSubscriberId = "4037002"
                }
            };
        }

        public static EntitlementResponse FakeEntitlementResponseNotFound()
        {
            return new EntitlementResponse
            {
                HttpResponseCode = "404",
                ErrorCode = "NOT_FOUND",
                RequestId = "13NHqVBfEtPynAh0Eh1Rst",
                ErrorMessage = "No such entity with id 65gd7j0zDPzdK1SkjfewfwefwefZXbQk"
            };
        }
    }
}
