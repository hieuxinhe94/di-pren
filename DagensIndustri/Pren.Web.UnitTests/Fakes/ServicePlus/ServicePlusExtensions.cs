using Di.ServicePlus.RestApi.Requests.Entitlements;
using Di.ServicePlus.RestApi.Requests.OAuth;
using Di.ServicePlus.RestApi.Requests.Users;
using Moq;
using Pren.Web.Business.ServicePlus.Logic;

namespace Pren.Web.UnitTests.Fakes.ServicePlus
{
    static class ServicePlusExtensions
    {
        public static Mock<IServicePlus> WithUsers(this Mock<IServicePlus> fakeServicePlus, Mock<IUsers> users)
        {
            var fakeServicePlusAPi = Mock.Get(fakeServicePlus.Object.RestApi);

            fakeServicePlusAPi.SetupGet(x => x.Users).Returns(users.Object ?? FakeServicePlusFactory.FakeUsersApi().Object);

            return fakeServicePlus;
        }

        public static Mock<IServicePlus> WithOAuth(this Mock<IServicePlus> fakeServicePlus, Mock<IOAuth> oAuth)
        {
            var fakeServicePlusAPi = Mock.Get(fakeServicePlus.Object.RestApi);

            fakeServicePlusAPi.SetupGet(x => x.OAuth).Returns(oAuth.Object ?? FakeServicePlusFactory.FakeOAuthApi().Object);

            return fakeServicePlus;
        }

        public static Mock<IServicePlus> WithEntitlements(this Mock<IServicePlus> fakeServicePlus, Mock<IEntitlements> entitlements)
        {
            var fakeServicePlusAPi = Mock.Get(fakeServicePlus.Object.RestApi);

            fakeServicePlusAPi.SetupGet(x => x.Entitlements).Returns(entitlements.Object ?? FakeServicePlusFactory.FakeEntitlementsApi().Object);

            return fakeServicePlus;
        }
    }
}
