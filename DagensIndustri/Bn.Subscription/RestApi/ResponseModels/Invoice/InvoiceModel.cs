using System;
using Newtonsoft.Json;

namespace Bn.Subscription.RestApi.ResponseModels.Invoice
{
    public class InvoiceModel
    {
        [JsonProperty("invoiceNumber")]
        public string InvoiceNumber { get; set; }

        [JsonProperty("invoiceType")]
        public string InvoiceType { get; set; }

        [JsonProperty("invoiceDate")]
        public DateTime? InvoiceDate { get; set; }

        [JsonProperty("dueDate")]
        public DateTime? DueDate { get; set; }

        [JsonProperty("invoiceAmount")]
        public decimal? InvoiceAmount { get; set; }

        [JsonProperty("invoiceHash")]
        public string InvoiceHash { get; set; }

        [JsonProperty("invoicePayed")]
        public bool InvoicePayed { get; set; }
    }
}
