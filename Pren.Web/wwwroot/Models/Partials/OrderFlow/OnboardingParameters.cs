using System.Collections.Generic;
using Newtonsoft.Json;

namespace Pren.Web.Models.Partials.OrderFlow
{
    public class OnboardingParameters
    {
        [JsonProperty("flowType")]
        public string FlowType { get; set; }

        [JsonProperty("companyAgreementId")]
        public string CompanyAgreementId { get; set; }

        [JsonProperty("resourceId")]
        public string ResourceId { get; set; }

        [JsonProperty("appId")]
        public string AppId { get; set; }

        [JsonProperty("paymentProviderType")]
        public string PaymentProviderType { get; set; }

        [JsonProperty("emailDomainWhitelist")]
        public string EmailDomainWhitelist { get; set; }

        [JsonProperty("productId")]
        public string ProductId { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("kayak_lCampNo")]
        public string KayakCampaign { get; set; }

        [JsonProperty("kayak_lPricelistno")]
        public string KayakPriceList { get; set; }

        [JsonProperty("kayak_sTargetGroup")]
        public string KayakTargetGroup { get; set; }

        [JsonProperty("paymentWithoutLogin")]
        public string PaymentWithoutLogin { get; set; }

        [JsonProperty("price")]
        public string Price { get; set; }

        [JsonProperty("productDesc")]
        public string ProductDescription { get; set; }

        [JsonProperty("subskind")]
        public string Subskind { get; set; }

        [JsonProperty("taxRate")]
        public string TaxRate { get; set; }

        [JsonProperty("payerCustomerNumber")]
        public List<PayerCustomerNumber> PayerCustomerNumber { get; set; }
    }

    public class PayerCustomerNumber
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("customerNumber")]
        public string CustomerNumber { get; set; }

        [JsonProperty("agreementProductRefRecId")]
        public string AgreementProductRefRecId { get; set; }
    }
}