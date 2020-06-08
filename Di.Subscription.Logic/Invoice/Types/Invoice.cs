using System;

namespace Di.Subscription.Logic.Invoice.Types
{
    public class Invoice
    {
        public int InvoiceNumber { get; set; }

        public long SubscriptionNumber { get; set; }

        public DateTime InvoiceDate { get; set; }

        public DateTime ExpirationDate { get; set; }

        public string InvoiceType { get; set; }

        public decimal InvoiceAmount { get; set; }

        public decimal VatAmount { get; set; }

        public decimal OpenAmount { get; set; }

        public string InvoiceText { get; set; }

        public string ReferenceNumber { get; set; }

        public string InvoiceState { get; set; }
    }
}
