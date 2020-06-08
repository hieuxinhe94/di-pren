using System.Collections.Generic;
using System.Threading.Tasks;
using Bn.Subscription.RestApi.ResponseModels;
using Bn.Subscription.RestApi.ResponseModels.Invoice;
using Di.Common.Cache;

namespace Bn.Subscription.RestApi.Requests.Invoice
{
    public class Invoice : RequestBase, IInvoice
    {
        public Invoice(string apiUrl, string apiClient, string apiSecret) : base(apiUrl, apiClient, apiSecret)
        {

        }

        public Invoice(string apiUrl, string apiClient, string apiSecret, IRequestService requestService, IObjectCache objectCache)
            : base(apiUrl, apiClient, apiSecret, requestService, objectCache)
        {
        }

        public async Task<ApiResponse<List<InvoiceModel>>> GetInvoicesAsync(string brand, long customerNumber)
        {
            return await GetAsync<ApiResponse<List<InvoiceModel>>>($"api/{brand}/invoices/{customerNumber}");
        }

        public async Task<ApiResponse<byte[]>> GetInvoicePdfByteArrayAsync(string brand, long customerNumber, string hash)
        {
            return await GetAsync<ApiResponse<byte[]>>($"api/{brand}/invoices/{customerNumber}/pdf/{hash}");
        }
    }
}