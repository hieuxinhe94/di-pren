using Di.ServicePlus.RestApi.Requests.BizSubscriptions;
using Di.ServicePlus.RestApi.Requests.Entitlements;
using Di.ServicePlus.RestApi.Requests.OAuth;
using Di.ServicePlus.RestApi.Requests.Offers;
using Di.ServicePlus.RestApi.Requests.Users;

namespace Di.ServicePlus.RestApi.Requests
{
    /// <summary>
    /// Factory that provides default concrete implementations of the request interfaces
    /// </summary>
    internal class RequestFactory
    {
        /// <summary>
        /// Provides a default concrete implementation of the IBizSubscriptions interface
        /// </summary>
        /// <param name="servicePlusApiUrl">The base url to Service Plus REST Api. Should look something like this: "http://api.qa.newsplus.se/v1/"</param>
        /// <returns>Default concrete implementation of the IBizSubscriptions interface <see cref="BizSubscriptions"/></returns>
        internal static IBizSubscriptions GetConcreteBizSubscriptions(string servicePlusApiUrl)
        {
            return new BizSubscriptions.BizSubscriptions(servicePlusApiUrl);
        }

        /// <summary>
        /// Provides a default concrete implementation of the IEntitlements interface
        /// </summary>
        /// <param name="servicePlusApiUrl">The base url to Service Plus REST Api. Should look something like this: "http://api.qa.newsplus.se/v1/"</param>
        /// <returns>Default concrete implementation of the IEntitlements interface <see cref="Entitlements"/></returns>
        internal static IEntitlements GetConcreteEntitlements(string servicePlusApiUrl)
        {
            return new Entitlements.Entitlements(servicePlusApiUrl);
        }

        /// <summary>
        /// Provides a default concrete implementation of the IOAuth interface
        /// </summary>
        /// <param name="servicePlusApiUrl">The base url to Service Plus REST Api. Should look something like this: "http://api.qa.newsplus.se/v1/"</param>
        /// <returns>Default concrete implementation of the IOAuth interface <see cref="OAuth"/></returns>
        internal static IOAuth GetConcreteOAuth(string servicePlusApiUrl)
        {
            return new OAuth.OAuth(servicePlusApiUrl);
        }

        /// <summary>
        /// Provides a default concrete implementation of the IUsers interface
        /// </summary>
        /// <param name="servicePlusApiUrl">The base url to Service Plus REST Api. Should look something like this: "http://api.qa.newsplus.se/v1/"</param>
        /// <returns>Default concrete implementation of the IUsers interface <see cref="Users"/></returns>
        internal static IUsers GetConcreteUsers(string servicePlusApiUrl)
        {
            return new Users.Users(servicePlusApiUrl);
        }

        /// <summary>
        /// Provides a default concrete implementation of the IOffers interface
        /// </summary>
        /// <param name="servicePlusApiUrl">The base url to Service Plus REST Api. Should look something like this: "http://api.qa.newsplus.se/v1/"</param>
        /// <returns>Default concrete implementation of the IOffers interface <see cref="Offers"/></returns>
        internal static IOffers GetConcreteOffers(string servicePlusApiUrl)
        {
            return new Offers.Offers(servicePlusApiUrl);
        }
    }
}
