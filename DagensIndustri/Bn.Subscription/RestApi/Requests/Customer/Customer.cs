using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Bn.Subscription.RestApi.ResponseModels;
using Bn.Subscription.RestApi.ResponseModels.Customer;
using Di.Common.Cache;

namespace Bn.Subscription.RestApi.Requests.Customer
{
    public class Customer : RequestBase, ICustomer
    {
        public Customer(string apiUrl, string apiClient, string apiSecret) : base(apiUrl, apiClient, apiSecret)
        {

        }

        public Customer(string apiUrl, string apiClient, string apiSecret, IRequestService requestService, IObjectCache objectCache)
            : base(apiUrl, apiClient, apiSecret, requestService, objectCache)
        {
        }

        public async Task<ApiResponse<CustomerModel>> GetCustomerAsync(string brand, long customerNumber)
        {
            return await GetAsync<ApiResponse<CustomerModel>>($"api/{brand}/customer/{customerNumber}");
        }

        public async Task<ApiResponse<List<CustomerModel>>> GetCustomerAsync(string brand, string userToken)
        {
            return await GetAsync<ApiResponse<List<CustomerModel>>>($"api/{brand}/customer/token/{userToken}");
        }

        public async Task<ApiResponse<string>> UpdateEmailAsync(string brand, long customerNumber, string email, string userToken)
        {
            var parameters = new Dictionary<string, string>
            {
                {"email", email},
                {"userToken", userToken}
            };

            return await PutAsync<ApiResponse<string>>($"api/{brand}/customer/{customerNumber}/email/", parameters);
        }

        public async Task<ApiResponse<string>> UpdatePhoneAsync(string brand, long customerNumber, string phoneNumber, string userToken)
        {
            var parameters = new Dictionary<string, string>
            {
                {"phoneNumber", phoneNumber},
                {"userToken", userToken}
            };

            return await PutAsync<ApiResponse<string>>($"api/{brand}/customer/{customerNumber}/phone/", parameters);
        }
    }
}