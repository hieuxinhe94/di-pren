using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Bn.Subscription.RestApi.ResponseModels;
using Bn.Subscription.RestApi.ResponseModels.Reclaim;
using Di.Common.Cache;

namespace Bn.Subscription.RestApi.Requests.Reclaim
{
    public class Reclaim : RequestBase, IReclaim
    {
        public Reclaim(string apiUrl, string apiClient, string apiSecret) : base(apiUrl, apiClient, apiSecret)
        {
        }

        public Reclaim(string apiUrl, string apiClient, string apiSecret, IRequestService requestService, IObjectCache objectCache) : base(apiUrl, apiClient, apiSecret, requestService, objectCache)
        {
        }

        public async Task<ApiResponse<List<ReclaimTypeModel>>> GetReclaimTypesAsync(string brand)
        {
            return await GetAsync<ApiResponse<List<ReclaimTypeModel>>>(GetReclaimTypeEndPointUrl(brand));
        }

        public async Task<ApiResponse<List<ReclaimModel>>> GetReclaimsAsync(string brand, long customerNumber)
        {
            return await GetAsync<ApiResponse<List<ReclaimModel>>>(GetReclaimEndPointUrl(brand, customerNumber));
        }

        public async Task<ApiResponse<string>> CreateReclaimAsync(
            string brand, 
            long customerNumber, 
            long subscriptionNumber, 
            DateTime issueDate,
            string reclaimTypeId)
        {
            var parameters = new Dictionary<string, string>
            {
                {"subscriptionNumber", subscriptionNumber.ToString()},
                {"issueDate", issueDate.ToString(CultureInfo.InvariantCulture)},
                {"reclaimTypeId", reclaimTypeId}
            };

            return await PostAsync<ApiResponse<string>>(GetReclaimEndPointUrl(brand, customerNumber), parameters);
        }

        private string GetReclaimEndPointUrl(string brand, long customerNumber)
        {
            return $"api/{brand}/reclaims/" + customerNumber;
        }

        private string GetReclaimTypeEndPointUrl(string brand)
        {
            return $"api/{brand}/reclaims/reclaimtypes";
        }
    }
}