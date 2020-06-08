using System.Collections.Generic;
using System.Linq;
using Di.Subscription.DataAccess.Invoice;

namespace Di.Subscription.Logic.Invoice.Retrievers
{
    public class InvoiceRetriever : IInvoiceRetriever
    {
        private readonly IInvoiceRepository _invoiceRepository;

        public InvoiceRetriever(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public IEnumerable<Types.Invoice> GetOpenInvoices(long customerNumber)
        {
            return _invoiceRepository.GetOpenInvoices(customerNumber, SubscriptionConstants.DefaultUserId).Select(GetInvoice);
        }

        private Types.Invoice GetInvoice(DataAccess.Invoice.Invoice invoice)
        {
            return new Types.Invoice
            {
                InvoiceDate = invoice.InvoiceDate,
                ExpirationDate = invoice.ExpirationDate,
                InvoiceAmount = invoice.InvoiceAmount,
                InvoiceNumber = invoice.InvoiceNumber,
                InvoiceState = invoice.InvoiceState,
                InvoiceText = invoice.InvoiceText,
                InvoiceType = invoice.InvoiceType,
                ReferenceNumber = invoice.ReferenceNumber,
                SubscriptionNumber = invoice.SubscriptionNumber,
                VatAmount = invoice.VatAmount,
                OpenAmount = invoice.OpenAmount
            };
        }
    }
}