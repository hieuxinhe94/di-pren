using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using Di.Common.Conversion;
using Di.Common.Utils;
using Di.Common.Utils.Url;
using Di.ServicePlus.Utils;

namespace Di.ServicePlus.RedirectApi.Orders
{
    internal class Orders : IOrders
    {
        private readonly IObjectConverter _objectConverter;

        public Orders(IObjectConverter objectConverter)
        {
            _objectConverter = objectConverter;
        }

        public string GetUrl(string appId, string productId, string secretKey, string email, string firstName, string lastName, string careOf,
            string phone, string streetAddress, string streetNumber, string zip, string city, string companyName, string personalNumber,
            string payerIdentificationNumber, string payerName, string payerAttention, string payerContactnumber,
            string payerStreetName, string payerStreetNumber, string payerZipCode, string payerCity, string payMethod,
            string targetGroup, string campNo, string priceList, string price, string productDescription, string callBackUrl, bool isPaperProduct, string subsKind, string salesChannel)
        {
            var timeStamp = DateTimeUtils.GetUnixTimeStampInMilliseconds(DateTime.Now);

            var paramValues = new Dictionary<string, string>
            {
                { "appId", appId}, //_siteConfiguration.GetSetting(SettingConstants.BonDigAppIdDagInd) }, 
                { "lc", "sv"},
                { "callback", callBackUrl },//required
                { "productId", productId }, //_siteConfiguration.GetSetting(SettingConstants.BonDigProductIdOrderFlow) }, 
                { "productDesc", productDescription},
                { "price", price},  //required TODO: Måste vara inklusive moms
                { "currency", "SEK"}, //required
                { "address", isPaperProduct.ToString().ToLower()}, //true = papperstidning
                { "personalNumber", personalNumber},
                //{ "externalSubscriberId", ""}, //TODO: Om kundnummer sätts kommer order skapas på den kunden. Frågan är hur den hanterar om det är ändrade adressuppgifter?
                { "paymentProviderType", payMethod},
                { "ts", timeStamp.ToString(CultureInfo.InvariantCulture)}, //required
                { "s",""}, //required
                { "kayak_lCampNo", campNo}, //required
                { "kayak_lPricelistno", priceList.ToString(CultureInfo.InvariantCulture)}, //required
                { "kayak_sTargetGroup", targetGroup}, //required
                { "subskind", subsKind},
                { "paymentWithoutLogin", true.ToString().ToLower()}, //if false user will be prompted to login in S+ flow
                { "email", email},
                { "firstName", firstName},
                { "lastName", lastName},
                { "careOf", careOf},
                { "phoneNumber", phone},
                { "streetName", streetAddress},
                { "streetNumber", streetNumber},
                { "zipCode", zip},
                { "city", city},
                { "country", "SE"},
                { "receiverCompany", companyName},

                { "payerIdentificationNumber", payerIdentificationNumber},
                { "payerName", payerName},
                { "payerAttention", payerAttention},
                { "payerContactnumber", payerContactnumber},
                { "invoiceStreetName", payerStreetName},
                { "invoiceStreetNumber", payerStreetNumber},
                { "invoiceZipCode", payerZipCode},
                { "invoiceCity", payerCity},
            };

            if (!string.IsNullOrEmpty(salesChannel))
            {
                paramValues.Add("salesChannel", salesChannel);
            }

            paramValues["s"] = CreateOrderSignature(secretKey, timeStamp, paramValues);

            return UrlUtils.AddDictionaryToQueryString(Settings.ServicePlusLoginPageUrl + "webpayment/process-order", paramValues);
        }

        public OrderResponse GetResponse(HttpRequestBase request)
        {
            var orderResponse = _objectConverter.ConvertFromQueryString<OrderResponse>(request);

            return orderResponse;
        }

        private string CreateOrderSignature(string secretKey, long timeStamp, Dictionary<string, string> paramValues)
        {
            var excludeArray = new[] { "s", "ts", "body", "authenticityToken", "callback", "cancel", "access_token", "email", "firstName", "lastName", "acceptTerms", "isEmbedded", "jsonpCallback" };
            var paramsForSignature = paramValues.Where(t => !excludeArray.Contains(t.Key)).ToDictionary(t => t.Key, t => t.Value);

            return ServicePlusSignatureUtils.Sign(secretKey, timeStamp, paramsForSignature);
        }

    }
}
