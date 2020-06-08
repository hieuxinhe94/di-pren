using System.Collections.Generic;
using System.Threading.Tasks;
using Pren.Web.Models.ViewModels.MySettings;

namespace Pren.Web.Business.Invoice
{
    public interface IInvoiceFacade
    {
        Task<List<CustomerInvoice>> GetArchivedInvoicesAsync(long customerNumber);

        List<CustomerInvoice> GetOpenInvoices(long customerNumber);

        Task<byte[]> GetArchivedInvoiceAsPdfBufferAsync(string customerNumber, string invoiceGuid);

        Task<List<CustomerInvoice>> GetAllInvoicesAsync(long customerNumber);
    }
}
