using System.Collections.Generic;
using System.Threading.Tasks;
using Bn.Subscription.RestApi.ResponseModels;
using Bn.Subscription.RestApi.ResponseModels.Subscription;

namespace Bn.Subscription.RestApi.Requests.Subscription
{
    public interface ISubscription
    {
        Task<ApiResponse<List<SubscriptionModel>>> GetSubscriptionsAsync(string brand, long customerNumber, bool includeNotActiveSubscriptions);
    }
}
