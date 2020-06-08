using System.Collections.Generic;
using System.Threading.Tasks;
using Bn.Subscription.RestApi.ResponseModels;
using Bn.Subscription.RestApi.ResponseModels.Invoice;

namespace Bn.Subscription.RestApi.Requests.Invoice
{
    public interface IInvoice
    {

        Task<ApiResponse<List<InvoiceModel>>> GetInvoicesAsync(string brand, long customerNumber);

        Task<ApiResponse<byte[]>> GetInvoicePdfByteArrayAsync(string brand, long customerNumber, string hash);

    }
}
