using Di.Common.Utils;
using Di.Common.Utils.Url;
using Pren.Web.Business.Invoice.ResponseModels;
using Pren.Web.Models.ViewModels.MySettings;

namespace Pren.Web.Business.Invoice.ExtendedModels
{
    public class SfInvoice : CustomerInvoice
    {
        public SfInvoice(Result invoice, string encryptKey, string encryptIv)
        {
            InvoiceNumber = invoice.Document.Properties.InvoiceNumber;
            InvoiceType = ConvertToInvoiceType(invoice.Document.Properties.Doctype);
            InvoiceDate = invoice.Document.Properties.InvoiceDate;
            DueDate = invoice.Document.Properties.DueDate;
            InvoiceAmount = invoice.Document.Properties.Amount != null
                ? decimal.Divide(invoice.Document.Properties.Amount.Value, 100)
                : 0;

            // Get invoice id from querystring
            var invoiceId = UrlUtils.GetQueryStringValue("uuid", invoice.Document.Nodes.File);
            // encrypt id
            InvoiceGuid = EncryptUtil.EncryptString(invoiceId, encryptKey, encryptIv);
            InvoicePayed = true;
        }

        private InvoiceType ConvertToInvoiceType(string invoiceType)
        {
            switch (invoiceType.ToLower())
            {
                case "invoice":
                    return InvoiceType.Normal;
                default:
                    return InvoiceType.NotMapped;
            }
        }
    }
}
