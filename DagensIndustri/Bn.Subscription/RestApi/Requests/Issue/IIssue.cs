using System.Collections.Generic;
using System.Threading.Tasks;
using Bn.Subscription.RestApi.ResponseModels;
using Bn.Subscription.RestApi.ResponseModels.Issue;

namespace Bn.Subscription.RestApi.Requests.Issue
{
    public interface IIssue
    {
        Task<ApiResponse<List<IssueDateModel>>> GetPastIssueDatesAsync(
            string brand,
            long customerNumber,
            long subscriptionNumber,
            int numberOfIssues);
    }
}