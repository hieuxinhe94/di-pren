using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

using DIClassLib.DbHelpers;
using DIClassLib.Misc;

namespace DIClassLib.CardPayment.Nets
{
    /// <summary>
    /// Class only reads payment facts from Nets. 
    /// (No insert/update/delete).
    /// </summary>
    public class QueryPayment
    {
        private XDocument doc { get; set; }
        
        public bool PaymentOk { get { return (string.IsNullOrEmpty(ErrorResponseCode)); } }

        /// <summary>
        /// ErrorResponseCode = "" on valid nets payment. 
        /// ErrorResponseCode = "-1" if transaction cannot be found in nets.
        /// </summary>
        public string ErrorResponseCode { get; set; }
        public string PanHash { get; set; }
        public string Issuer { get; set; }


        public QueryPayment(string transactionId)
        {
            doc = GetPaymentInfoDoc(transactionId);
            if (doc != null)
            {
                ErrorResponseCode = GetResponseCode();
                PanHash = GetPanHash();
                Issuer = GetIssuer();
            }
        }


        private XDocument GetPaymentInfoDoc(string transactionId)
        {
            #region test url:s
            //test url with PanHash
            //url.Append("https://epayment-test.bbs.no/Netaxept/Query.aspx?merchantId=520893&token=7g!DF2f-&TransactionID=2336ec6a29e744a9b65b75ddd3636315");

            //test url without PanHash
            //url.Append("https://epayment-test.bbs.no/Netaxept/Query.aspx?merchantId=520893&token=7g!DF2f-&TransactionID=3749ae4847634842acaa6a64083604bb");

            //test url with Error
            //https://epayment-test.bbs.no/Netaxept/Query.aspx?merchantId=520893&token=7g!DF2f-&TransactionID=6bafde3c774241e1ac270e2d146a305a
            #endregion

            var url = new StringBuilder();
            url.Append(Settings.Nets_UrlQuery);
            url.Append("?merchantId=" + Settings.Nets_MerchantId);
            url.Append("&token=" + Settings.Nets_Token);
            url.Append("&TransactionID=" + transactionId);

            try
            {
                var doc = XDocument.Load(url.ToString());
                return doc;
            }
            catch (Exception ex)
            {
                new Logger("GetPaymentInfoDoc() failed for url: " + url.ToString(), ex.ToString());
            }

            return null;
        }

        private string GetResponseCode()
        {
            #region example data
            //doc with error
            //<ErrorLog>
            //    <PaymentError>
            //        <DateTime>2014-04-08T15:44:30.837</DateTime>
            //        <Operation>Terminal</Operation>
            //        <ResponseCode>17</ResponseCode>
            //        <ResponseSource>Terminal</ResponseSource>
            //        <ResponseText>Cancelled by customer.</ResponseText>
            //    </PaymentError>
            //</ErrorLog>

            //doc without error
            //<ErrorLog/>
            #endregion

            //no xml document found in nets system for provided transId
            if (!doc.Descendants("TransactionId").Any())
                return Settings.Nets_Err_TransNotFoundInNets;

            var desc = doc.Descendants("ErrorLog").Descendants("PaymentError").Descendants("ResponseCode");
            if (desc.Any())
                return desc.FirstOrDefault().Value;

            return string.Empty;
        }

        private string GetPanHash()
        {
            #region example data
            //<Recurring>
            //    <PanHash>jfZ0epZqGCAy53P3TxZgCCXqGlE=</PanHash>
            //    <ExpiryDate/>
            //    <Frequency/>
            //    <Type>Store (Init)</Type>
            //</Recurring>
            #endregion

            var desc = doc.Descendants("Recurring").Descendants("PanHash");
            if (desc.Any())
                return desc.FirstOrDefault().Value;

            return string.Empty;
        }

        private string GetIssuer()
        {
            #region example data
            //<CardInformation>
            //    <ExpiryDate>1407</ExpiryDate>
            //    <Issuer>Visa</Issuer>
            //    <IssuerCountry>NO</IssuerCountry>
            //    <MaskedPAN>492500******0004</MaskedPAN>
            //    <PaymentMethod>Visa</PaymentMethod>
            //    <IssuerId>3</IssuerId>
            //</CardInformation>
            #endregion

            var desc = doc.Descendants("CardInformation").Descendants("Issuer");
            if (desc.Any())
                return desc.FirstOrDefault().Value;

            return string.Empty;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("<b>QueryPayment objekt</b><br>");
            sb.Append("ErrorResponseCode: " + ErrorResponseCode + "<br>");
            sb.Append("PanHash: " + PanHash + "<br>");
            sb.Append("Issuer: " + Issuer + "<br>");
            return sb.ToString();
        }
    }
}
