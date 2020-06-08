using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Di.Subscription.Logic.Invoice;
using Microsoft.Ajax.Utilities;
using Pren.Web.Business.Configuration;
using Pren.Web.Business.Invoice.ExtendedModels;
using Pren.Web.Models.ViewModels.MySettings;

namespace Pren.Web.Business.Invoice
{
    public class InvoiceFacade : IInvoiceFacade
    {
        private readonly IInvoiceHandler _invoiceHandler;
        private readonly IInvoiceApi _invoiceApi;

        public InvoiceFacade(IInvoiceHandler invoiceHandler, ISiteSettings siteSettings)
        {
            _invoiceHandler = invoiceHandler;
            _invoiceApi = new InvoiceApi(
                siteSettings.InvoiceApiUrl,
                siteSettings.InvoiceApiStreamUrl,
                siteSettings.InvoiceApiUserName,
                siteSettings.InvoiceApiPassword,
                siteSettings.InvoiceEncryptKey,
                siteSettings.InvoiceEncryptIv);
        }

        public async Task<byte[]> GetArchivedInvoiceAsPdfBufferAsync(string customerNumber, string invoiceGuid)
        {
            return await _invoiceApi.GetInvoiceByteArrayAsync(customerNumber, invoiceGuid);
        }

        public List<CustomerInvoice> GetOpenInvoices(long customerNumber)
        {
            var invoices = _invoiceHandler.InvoiceRetriever.GetOpenInvoices(customerNumber);

            var kayakInvoices = invoices.Select(t => new KayakInvoice(t));

            var customerInvoices = new List<CustomerInvoice>();
            customerInvoices.AddRange(kayakInvoices);

            return customerInvoices;
        }

        public async Task<List<CustomerInvoice>> GetArchivedInvoicesAsync(long customerNumber)
        {
            var invoices = await _invoiceApi.GetInvoicesAsync(customerNumber);

            return invoices.ToList();
        }

        /// <summary>
        /// If invoice exists in open invoices and archived invoices, it's not paid. 
        /// If invoice exists only in archived invoices, it's paid.
        /// </summary>
        /// <param name="customerNumber">The customer number you wan't to retrieve invoices for</param>
        /// <returns>A list of customers invoices</returns>
        public async Task<List<CustomerInvoice>> GetAllInvoicesAsync(long customerNumber)
        {
            var openInvoices = GetOpenInvoices(customerNumber);
            var invoicesFromArchive = await GetArchivedInvoicesAsync(customerNumber);

            foreach (var invoice in openInvoices)
            {
                var invoiceFromArchive = invoicesFromArchive.Select(t => t).FirstOrDefault(t => t.InvoiceNumber == invoice.InvoiceNumber);

                if (invoiceFromArchive != null)
                {
                    // Get url from archive invoice
                    invoice.InvoicePathUrl = invoiceFromArchive.InvoicePathUrl;
                    invoice.InvoiceGuid = invoiceFromArchive.InvoiceGuid;
                }
            }

            return openInvoices.Union(invoicesFromArchive).DistinctBy(t => t.InvoiceNumber).ToList();
        }
    }
}