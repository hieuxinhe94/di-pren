using Di.ServicePlus.RestApi.Requests;
using Di.ServicePlus.RestApi.Requests.BizSubscriptions;
using Di.ServicePlus.RestApi.Requests.Entitlements;
using Di.ServicePlus.RestApi.Requests.OAuth;
using Di.ServicePlus.RestApi.Requests.Offers;
using Di.ServicePlus.RestApi.Requests.Users;

namespace Di.ServicePlus
{
    public interface IServicePlusApi
    {
        IOAuth OAuth { get; }
        IEntitlements Entitlements { get; }
        IUsers Users { get; }
        IBizSubscriptions BizSubscriptions { get; }
        IOffers Offers { get; }
    }

    /// <summary>
    /// Entrypoint for accessing the ServicePlus REST Api
    /// </summary>
    public class ServicePlusApi : IServicePlusApi
    {
        public IOAuth OAuth { get; private set; }
        public IEntitlements Entitlements { get; private set; }
        public IUsers Users { get; private set; }
        public IBizSubscriptions BizSubscriptions { get; private set; }
        public IOffers Offers { get; private set; }

        /// <summary>
        /// Creates a new instance of the Service Plus REST Api Wrapper with deafault implementations.
        /// </summary>
        /// <param name="servicePlusApiUrl">The base url to Service Plus REST Api. Should look something like this: "http://api.qa.newsplus.se/v1/"</param>
        public ServicePlusApi(string servicePlusApiUrl)
            : this(
            RequestFactory.GetConcreteOAuth(servicePlusApiUrl),
            RequestFactory.GetConcreteEntitlements(servicePlusApiUrl),
            RequestFactory.GetConcreteUsers(servicePlusApiUrl),
            RequestFactory.GetConcreteBizSubscriptions(servicePlusApiUrl),
            RequestFactory.GetConcreteOffers(servicePlusApiUrl))
        {
        }

        /// <summary>
        /// Creates a new instance of the Service Plus REST Api Wrapper.
        /// We make it possible to provide own implementations of the request interfaces by Constructor Injection
        /// </summary>
        /// <param name="oAuth">A concrete implementation of the IOAuth interface</param>
        /// <param name="entitlements">A concrete implementation of the IEntitlements interface</param>
        /// <param name="users">A concrete implementation of the IUsers interface</param>
        /// <param name="bizSubscriptions">A concrete implementation of the IBizSubscriptions interface</param>
        /// <param name="offers">A concrete implementation of the IOffers interface</param>
        public ServicePlusApi(IOAuth oAuth, IEntitlements entitlements, IUsers users, IBizSubscriptions bizSubscriptions, IOffers offers)
        {
            OAuth = oAuth;
            Entitlements = entitlements;
            Users = users;
            BizSubscriptions = bizSubscriptions;
            Offers = offers;
        }
    }
}
