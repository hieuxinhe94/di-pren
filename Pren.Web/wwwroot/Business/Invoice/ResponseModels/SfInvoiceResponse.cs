using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Pren.Web.Business.Invoice.ResponseModels
{
    public class SfInvoiceResponse
    {
        [JsonProperty("query")]
        public Query Query { get; set; }

    }
    public class Properties
    {
        [JsonProperty("company_registration_number")]
        public string CompanyRegistrationNumber { get; set; }

        [JsonProperty("phone_number")]
        public string PhoneNumber { get; set; }

        [JsonProperty("document_id")]
        public int DocumentId { get; set; }

        [JsonProperty("distribution_channel")]
        public string DistributionChannel { get; set; }

        [JsonProperty("invoice_number")]
        public string InvoiceNumber { get; set; }

        [JsonProperty("invoice_date")]
        public DateTime InvoiceDate { get; set; }

        [JsonProperty("personal_number")]
        public string PersonalNumber { get; set; }

        [JsonProperty("customer_number")]
        public string CustomerNumber { get; set; }

        [JsonProperty("ocr_number")]
        public string OcrNumber { get; set; }

        [JsonProperty("subscription_number")]
        public string SubscriptionNumber { get; set; }

        [JsonProperty("due_date")]
        public DateTime? DueDate { get; set; }

        [JsonProperty("doctype")]
        public string Doctype { get; set; }

        [JsonProperty("email_address")]
        public string EmailAddress { get; set; }

        [JsonProperty("full_name")]
        public string FullName { get; set; }

        [JsonProperty("amount")]
        public decimal? Amount { get; set; }
    }

    public class Infos
    {
        [JsonProperty("notecount")]
        public int NoteCount { get; set; }

        [JsonProperty("document_id")]
        public int DocumentId { get; set; }

        [JsonProperty("refcount")]
        public int RefCount { get; set; }

        [JsonProperty("state")]
        public int State { get; set; }

        [JsonProperty("mimetype")]
        public string Mimetype { get; set; }

        [JsonProperty("locked_by")]
        public string LockedBy { get; set; }

        [JsonProperty("locked_at")]
        public string LockedAt { get; set; }

        [JsonProperty("created")]
        public DateTime Created { get; set; }

        [JsonProperty("file_id")]
        public string FileId { get; set; }

        [JsonProperty("doctype")]
        public string Doctype { get; set; }
    }

    public class Nodes
    {
        [JsonProperty("file")]
        public string File { get; set; }

        [JsonProperty("references")]
        public string References { get; set; }

        [JsonProperty("notes")]
        public string Notes { get; set; }
    }

    public class Document
    {
        [JsonProperty("properties")]
        public Properties Properties { get; set; }

        [JsonProperty("infos")]
        public Infos Infos { get; set; }

        [JsonProperty("nodes")]
        public Nodes Nodes { get; set; }
    }

    public class Result
    {
        [JsonProperty("document")]
        public Document Document { get; set; }
    }

    public class Query
    {
        [JsonProperty("results")]
        public List<Result> Results { get; set; }
    }
}
