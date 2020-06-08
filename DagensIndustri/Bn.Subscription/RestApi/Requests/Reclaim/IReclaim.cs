using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bn.Subscription.RestApi.ResponseModels;
using Bn.Subscription.RestApi.ResponseModels.Reclaim;

namespace Bn.Subscription.RestApi.Requests.Reclaim
{
    public interface IReclaim
    {
        Task<ApiResponse<List<ReclaimTypeModel>>> GetReclaimTypesAsync(string brand);
        Task<ApiResponse<List<ReclaimModel>>> GetReclaimsAsync(string brand, long customerNumber);

        Task<ApiResponse<string>> CreateReclaimAsync(string brand, long customerNumber, long subscriptionNumber, DateTime issueDate, string reclaimTypeId);
    }
}