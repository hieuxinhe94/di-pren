using System.Globalization;
using Pren.Web.Models.ViewModels.MySettings;

namespace Pren.Web.Business.Invoice.ExtendedModels
{
    public class KayakInvoice : CustomerInvoice
    {
        public KayakInvoice(Di.Subscription.Logic.Invoice.Types.Invoice invoice)
        {
            InvoiceNumber = invoice.InvoiceNumber.ToString(CultureInfo.InvariantCulture);
            InvoiceType = ConvertToInvoiceType(invoice.InvoiceType);
            InvoiceDate = invoice.InvoiceDate;
            DueDate = invoice.ExpirationDate;
            InvoiceAmount = invoice.OpenAmount;
            InvoicePayed = false;
        }

        private InvoiceType ConvertToInvoiceType(string invoiceType)
        {
            switch (invoiceType)
            {
                case "F":
                case "00":
                    return InvoiceType.Normal;
                case "E":
                    return InvoiceType.EInvoice;
                case "01":
                    return InvoiceType.Autogiro;
                case "02":
                    return InvoiceType.Reminder1;
                case "03":
                    return InvoiceType.Reminder2;
                case "05":
                    return InvoiceType.FinalInvoice;
                case "07":
                    return InvoiceType.CreditInvoice;
                default:
                    return InvoiceType.NotMapped;
            }
        }
    }
}
