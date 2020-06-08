using Di.ServicePlus;
using Di.ServicePlus.RestApi.Requests.Entitlements;
using Di.ServicePlus.RestApi.Requests.OAuth;
using Di.ServicePlus.RestApi.Requests.Users;
using Moq;
using Pren.Web.Business.ServicePlus.Logic;

namespace Pren.Web.UnitTests.Fakes.ServicePlus
{
    internal class FakeServicePlusFactory
    {
        public static Mock<IServicePlus> FakeServicePlus()
        {
            var fakeServicePlus = new Mock<IServicePlus>();

            var fakeServicePlusAPi = new Mock<IServicePlusApi>();

            fakeServicePlus.SetupGet(x => x.RestApi).Returns(fakeServicePlusAPi.Object);

            return fakeServicePlus;
        }

        public static Mock<IServicePlusApi> FakeServicePlusApi()
        {
            var fakeServicePlusAPi = new Mock<IServicePlusApi>();

            fakeServicePlusAPi.SetupGet(x => x.Users).Returns(FakeUsersApi().Object);

            return fakeServicePlusAPi;
        }

        public static Mock<IUsers> FakeUsersApi()
        {
            var fakeUsers = new Mock<IUsers>();

            return fakeUsers;
        }

        public static Mock<IOAuth> FakeOAuthApi()
        {
            var fakeOAuth = new Mock<IOAuth>();

            return fakeOAuth;
        }

        public static Mock<IEntitlements> FakeEntitlementsApi()
        {
            var fakeEntitlements = new Mock<IEntitlements>();

            return fakeEntitlements;
        }
    }
}
