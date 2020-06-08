using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Bn.Subscription.RestApi.ResponseModels;
using Bn.Subscription.RestApi.ResponseModels.Issue;
using Di.Common.Cache;

namespace Bn.Subscription.RestApi.Requests.Issue
{
    public class Issue : RequestBase, IIssue
    {
        public Issue(string apiUrl, string apiClient, string apiSecret) : base(apiUrl, apiClient, apiSecret)
        {
        }

        public Issue(string apiUrl, string apiClient, string apiSecret, IRequestService requestService, IObjectCache objectCache) : base(apiUrl, apiClient, apiSecret, requestService, objectCache)
        {
        }

        public async Task<ApiResponse<List<IssueDateModel>>> GetPastIssueDatesAsync(string brand, long customerNumber, long subscriptionNumber, int numberOfIssues)
        {
            var parameters = new Dictionary<string, string>
            {
                {"subscriptionNumber", subscriptionNumber.ToString(CultureInfo.InvariantCulture)},
                {"numberOfIssues", numberOfIssues.ToString(CultureInfo.InvariantCulture)}
            };

            return await GetAsync<ApiResponse<List<IssueDateModel>>>($"api/{brand}/issue/pastissuedates/{customerNumber}", parameters);
        }
    }
}