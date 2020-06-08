using Di.Subscription.Logic.Invoice.Retrievers;

namespace Di.Subscription.Logic.Invoice
{
    public interface IInvoiceHandler
    {
        IInvoiceRetriever InvoiceRetriever { get; }
    }
}
