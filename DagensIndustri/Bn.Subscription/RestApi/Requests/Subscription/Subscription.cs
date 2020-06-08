using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Bn.Subscription.RestApi.ResponseModels;
using Bn.Subscription.RestApi.ResponseModels.Subscription;
using Di.Common.Cache;

namespace Bn.Subscription.RestApi.Requests.Subscription
{
    public class Subscription : RequestBase, ISubscription
    {
        public Subscription(string apiUrl, string apiClient, string apiSecret) : base(apiUrl, apiClient, apiSecret)
        {
        }

        public Subscription(string apiUrl, string apiClient, string apiSecret, IRequestService requestService, IObjectCache objectCache) : base(apiUrl, apiClient, apiSecret, requestService, objectCache)
        {
        }

        public async Task<ApiResponse<List<SubscriptionModel>>> GetSubscriptionsAsync(string brand, long customerNumber, bool includeNotActiveSubscriptions)
        {
            var parameters = new Dictionary<string, string>
            {
                {"includeNotActiveSubscriptions", includeNotActiveSubscriptions.ToString(CultureInfo.InvariantCulture)}
            };

            return await GetAsync<ApiResponse<List<SubscriptionModel>>>($"api/{brand}/subscription/{customerNumber}", parameters);
        }
    }
}