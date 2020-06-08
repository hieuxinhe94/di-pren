using System;
using System.Collections.Generic;
using Pren.Web.Models.Pages.MySettings;

namespace Pren.Web.Models.ViewModels.MySettings
{
    public class InvoicePageViewModel : PageViewModel<InvoicePage>
    {
        public InvoicePageViewModel(InvoicePage currentPage)
            : base(currentPage)
        {
            
        }

        public long CustomerNumber { get; set; }
        public List<CustomerInvoice> Invoices { get; set; }
    }

    public class CustomerInvoice
    {
        public string InvoiceNumber { get; set; }
        public InvoiceType InvoiceType { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public DateTime? DueDate { get; set; }
        public decimal? InvoiceAmount { get; set; }
        public string InvoicePathUrl { get; set; }
        public string InvoiceGuid { get; set; }
        public bool InvoicePayed { get; set; }
    }

    public enum InvoiceType
    {
        Normal,
        EInvoice,
        Autogiro,
        Reminder1,
        Reminder2,
        FinalInvoice,
        CreditInvoice,
        NotMapped
    }
}
