using System.Collections.Generic;

namespace Di.Subscription.DataAccess.Invoice
{
    public interface IInvoiceRepository
    {
        IEnumerable<Invoice> GetOpenInvoices(long customerNumber, string userId);
    }
}
