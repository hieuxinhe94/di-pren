using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Web.Mvc;
using Di.Common.Logging;
using Microsoft.Ajax.Utilities;
using Pren.Web.Business.Attributes;
using Pren.Web.Business.Invoice;
using Pren.Web.Business.Messaging;
using Pren.Web.Business.Session;
using Pren.Web.Models.Pages.MySettings;
using Pren.Web.Models.ViewModels.MySettings;

namespace Pren.Web.Controllers.MySettings
{
    // ReSharper disable Mvc.ViewNotResolved
    [NoCache]
    public class InvoicePageController : MySettingsControllerBase<InvoicePage>
    {
        private readonly ILogger _logger;
        private readonly IInvoiceFacade _invoiceFacade;

        public InvoicePageController(
            ISessionData sessionData, 
            ILogger logger,
            IInvoiceFacade invoiceFacade) 
            : base(sessionData)
        {
            _logger = logger;
            _invoiceFacade = invoiceFacade;
        }

        [AuthorizeUser]
        public ActionResult Index(InvoicePage currentPage, long? cusno)
        {
            var model = new InvoicePageViewModel(currentPage);
            var subscriber = GetSubscriberFromSession();

            try
            {
                var customerNumber = cusno ?? subscriber.SelectedSubscription.CustomerNumber; //todo: under test, 3312516, TKM
                model.Invoices = GetInvoices(customerNumber);
                model.CustomerNumber = customerNumber;
            }            
            catch (Exception exception)
            {
                _logger.Log(exception, string.Format("Failed to get invoices for customer with customerNumber: '{0}', invoices is null", subscriber.SelectedSubscription.CustomerNumber), LogLevel.Error, typeof(InvoicePageController));
                TempData["Message"] = new Message("Just nu kan vi inte visa dina fakturor. Vänligen kom tillbaka vid ett senare tillfälle.", MessageType.Danger);
            }
            
            return View(model);
        }

        [AuthorizeUser]
        public ActionResult ShowInvoice(InvoicePage currentPage, string customerNumber, string invoiceGuid)
        {
            try
            {
                var pdfBuffer = _invoiceFacade.GetArchivedInvoiceAsPdfBufferAsync(customerNumber, invoiceGuid);

                var contentDisposition = new ContentDisposition
                {
                    FileName = "Faktura-" + invoiceGuid + ".pdf",
                    Inline = true //Show in browser
                };
                Response.AppendHeader("Content-Disposition", contentDisposition.ToString());
                return File(pdfBuffer.Result, "application/pdf");
            }
            catch (Exception exception)
            {
                _logger.Log(exception, string.Format("Failed to show invoice as pdf for customernumber: {0}, invoiceGid: {1}", customerNumber, invoiceGuid), LogLevel.Error, typeof(InvoicePageController));

                var model = new InvoicePageViewModel(currentPage);
                return View("PdfNotFound", model);
            }
        }

        /// <summary>
        /// If invoice exists in open invoices and archived invoices, it's not paid. 
        /// If invoice exists only in archived invoices, it's paid.
        /// </summary>
        /// <param name="customerNumber">The customer number you wan't to retrieve invoices for</param>
        /// <returns>A list of customers invoices</returns>
        private List<CustomerInvoice> GetInvoices(long customerNumber)
        {
            var openInvoices = _invoiceFacade.GetOpenInvoices(customerNumber);
            var invoicesFromArchive = _invoiceFacade.GetArchivedInvoicesAsync(customerNumber).Result;

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