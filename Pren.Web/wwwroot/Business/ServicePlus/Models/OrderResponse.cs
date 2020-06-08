
using Di.Common.Conversion.Attributes;
using Di.Common.Conversion.Types;

namespace Pren.Web.Business.ServicePlus.Models
{
    public class OrderResponse : IQueryStringObject
    {
        /// <summary>
        /// payment-successful/payment-cancelled
        /// </summary>
        [QueryString(Key = "result")]
        public string Result { get; set; }

        [QueryString(Key = "zipCode")]
        public string AddressZip { get; set; }

        [QueryString(Key = "country")]
        public string AddressCountry { get; set; }

        [QueryString(Key = "paymentWithoutLogin")]
        public bool PaymentWithoutLogin { get; set; }

        [QueryString(Key = "address")]
        public bool Address { get; set; }

        [QueryString(Key = "productId")]
        public string ProductId { get; set; }

        [QueryString(Key = "city")]
        public string AddressCity { get; set; }

        [QueryString(Key = "streetNumber")]
        public string AddressStreetNumber { get; set; }

        [QueryString(Key = "personalNumber")]
        public string PersonalNumber { get; set; }

        [QueryString(Key = "kayak_lPricelistno")]
        public string PriceGroup { get; set; }

        [QueryString(Key = "kayak_sTargetGroup")]
        public string TargetGroup { get; set; }
       
        [QueryString(Key = "productDesc")]
        public string ProductDesc { get; set; }

        [QueryString(Key = "access_token")]
        public string AccessToken { get; set; }

        [QueryString(Key = "streetName")]
        public string AddressStreetName { get; set; }

        [QueryString(Key = "paymentProviderType")]
        public string PayMethod { get; set; }

        [QueryString(Key = "price")]
        public string Price { get; set; }

        [QueryString(Key = "kayak_ICampNo")]
        public string CampaignNumber { get; set; }

        [QueryString(Key = "appId")]
        public string AppId { get; set; }

        [QueryString(Key = "currency")]
        public string Currency { get; set; }

        [QueryString(Key = "email")]
        public string Email { get; set; }

        /// <summary>
        /// Order transaction reference
        /// </summary>
        [QueryString(Key = "reference")]
        public string Reference { get; set; }

        [QueryString(Key = "orderId")]
        public string OrderId { get; set; }
        
        /// <summary>
        /// Point out whether this is the first payment of user or not
        /// </summary>
        [QueryString(Key = "firstPayment")]
        public bool FirstPayment { get; set; }
        
        /// <summary>
        /// S+ user access token
        /// </summary>
        [QueryString(Key = "token")]
        public string Token { get; set; }
        
        /// <summary>
        /// S+ user id
        /// </summary>
        [QueryString(Key = "id")]
        public string Id { get; set; }

        /// <summary>
        /// If there are errors in order parameters, returned URL will be appended with following query params:
        //  error=Invalid order: missing required parameters
        /// </summary>
        [QueryString(Key = "error")]
        public string Error { get; set; }
    }
}