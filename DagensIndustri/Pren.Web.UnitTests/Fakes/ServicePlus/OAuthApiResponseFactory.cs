using Di.ServicePlus.RestApi.ResponseModels.OAuthToken;

namespace Pren.Web.UnitTests.Fakes.ServicePlus
{
    static class OAuthApiResponseFactory
    {
        public static OAuthTokenResponse FakeOAuthTokenResponseValidToken()
        {
            return new OAuthTokenResponse
            {
                HttpResponseCode = "200",
                AccessToken = "fakeaccesstoken",
                TokenType = "bearer",
                ExpiresIn = "28800",
                Scope = "api brandId:5DuzcZz0j8u0zArSNzZgHO externalId"
            };
        }

        public static OAuthTokenResponse FakeOAuthTokenResponseOtherError()
        {
            return new OAuthTokenResponse
            {
                HttpResponseCode = "405",
                ErrorCode = "OTHER_ERROR",
                RequestId = "6uhl6MeTBy03B8uCQ3G6sH"
            };
        }
    }
}
