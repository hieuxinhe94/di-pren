using Di.ServicePlus.RestApi.ResponseModels.OAuthToken;

namespace Di.ServicePlus.RestApi.Requests.OAuth
{
    public interface IOAuth
    {
        OAuthTokenResponse GetSystemUserAccessToken(string clientId, string clientSecret, string grantType);
    }
}