using System;
using Di.ServicePlus.RestApi.Requests.OAuth;
using Di.ServicePlus.RestApi.ResponseModels.OAuthToken;
using Moq;
using Moq.Language.Flow;

namespace Pren.Web.UnitTests.Fakes.ServicePlus
{
    static class OAuthApiExtensions
    {
        private static ISetup<IOAuth, OAuthTokenResponse> CreateGetSystemUserAccessTokenSetup(this Mock<IOAuth> oAuth)
        {
            return oAuth.Setup(x => x.GetSystemUserAccessToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));
        }

        public static Mock<IOAuth> WithGetSystemUserAccessToken(this Mock<IOAuth> oAuth, OAuthTokenResponse oAuthTokenResponseToReturn)
        {
            oAuth.CreateGetSystemUserAccessTokenSetup()
                .Returns(oAuthTokenResponseToReturn);

            return oAuth;
        }

        public static Mock<IOAuth> WithGetSystemUserAccessTokenThatReturnsValidTokenResponse(this Mock<IOAuth> oAuth)
        {
            oAuth.WithGetSystemUserAccessToken(OAuthApiResponseFactory.FakeOAuthTokenResponseValidToken());

            return oAuth;
        }

        public static Mock<IOAuth> WithGetSystemUserAccessTokenThatReturnsFaultyTokenResponse(this Mock<IOAuth> oAuth)
        {
            oAuth.WithGetSystemUserAccessToken(OAuthApiResponseFactory.FakeOAuthTokenResponseOtherError());

            return oAuth;
        }

        public static Mock<IOAuth> WithGetSystemUserAccessTokenThatThrowsException(this Mock<IOAuth> oAuth)
        {
            oAuth.CreateGetSystemUserAccessTokenSetup()
                .Throws(new Exception("FakeException"));

            return oAuth;
        }
    }
}
