using System.Collections.Generic;

namespace Di.Subscription.Logic.Invoice.Retrievers
{
    public interface IInvoiceRetriever
    {
        IEnumerable<Types.Invoice> GetOpenInvoices(long customerNumber);
    }
}
