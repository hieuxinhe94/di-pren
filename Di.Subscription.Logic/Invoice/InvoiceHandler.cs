using Di.Subscription.Logic.Invoice.Retrievers;

namespace Di.Subscription.Logic.Invoice
{
    public class InvoiceHandler : IInvoiceHandler
    {
        public IInvoiceRetriever InvoiceRetriever { get; private set; }

        public InvoiceHandler(IInvoiceRetriever invoiceRetriever)
        {
            InvoiceRetriever = invoiceRetriever;
        }
    }
}