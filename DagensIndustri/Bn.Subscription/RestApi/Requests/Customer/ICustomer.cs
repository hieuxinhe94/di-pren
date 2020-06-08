using System.Collections.Generic;
using System.Threading.Tasks;
using Bn.Subscription.RestApi.ResponseModels;
using Bn.Subscription.RestApi.ResponseModels.Customer;

namespace Bn.Subscription.RestApi.Requests.Customer
{
    public interface ICustomer
    {
        Task<ApiResponse<CustomerModel>> GetCustomerAsync(string brand, long customerNumber);

        Task<ApiResponse<List<CustomerModel>>> GetCustomerAsync(string brand, string userToken);

        Task<ApiResponse<string>> UpdateEmailAsync(string brand, long customerNumber, string email, string userToken);

        Task<ApiResponse<string>> UpdatePhoneAsync(string brand, long customerNumber, string phoneNumber, string userToken);
    }
}
