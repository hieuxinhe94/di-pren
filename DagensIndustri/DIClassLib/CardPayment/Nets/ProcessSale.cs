using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Xml.Linq;
using DIClassLib.DbHelpers;
using DIClassLib.Misc;


namespace DIClassLib.CardPayment.Nets
{
    /// <summary>
    /// Class processes a registred Nets sale. A process call with operation=SALE is made. 
    /// (After payment has been registred it has to be processed in order for us to collect the money).
    /// </summary>
    public class ProcessSale
    {
        #region nets return

        //return on success
        //--------------------
        //<ProcessResponse xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
        //  <AuthorizationId>122100</AuthorizationId>
        //  <BatchNumber>26</BatchNumber>
        //  <ExecutionTime>2014-04-28T13:41:08.4884378+02:00</ExecutionTime>
        //  <MerchantId>520893</MerchantId>
        //  <Operation>SALE</Operation>
        //  <ResponseCode>OK</ResponseCode>
        //  <TransactionId>a41ac00b74c949ba919134306701ae46</TransactionId>
        //</ProcessResponse>


        // return on fail eg 1
        //--------------------
        //<?xml version="1.0" encoding="utf-8"?>
        //<Exception xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
        //  <Error xsi:type="BBSException">
        //    <Message>Unable to sale</Message>
        //    <Result>
        //      <IssuerId>3</IssuerId>
        //      <ResponseCode>98</ResponseCode>
        //      <ResponseText>Transaction already processed</ResponseText>
        //      <ResponseSource>Netaxept</ResponseSource>
        //      <TransactionId>f045faa527c449c080db27d9009091c6</TransactionId>
        //      <ExecutionTime>2014-04-29T13:07:24.5666299+02:00</ExecutionTime>
        //      <MerchantId>200906</MerchantId>
        //      <MaskedPan />
        //    </Result>
        //  </Error>
        //</Exception>


        // return on fail eg 2
        //--------------------
        //<?xml version="1.0" encoding="utf-8"?>
        //<Exception xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
        //  <Error xsi:type="GenericError">
        //    <Message>Process setup has not been run on this transaction</Message>
        //  </Error>
        //</Exception>

        #endregion

        public bool PaymentOk { get { return (ResponseCode == "OK"); } }
        public string AuthorizationId { get; set; }
        public string BatchNumber { get; set; }
        public string ExecutionTime { get; set; }
        public string MerchantId { get; set; }
        public string Operation { get; set; }
        public string TransactionId { get; set; }
        public string ResponseCode { get; set; } 
        public string ResponseSource { get; set; }
        public string ResponseText { get; set; }
        public string Message { get; set; }
        public string IssuerId { get; set; }
        public string MaskedPan { get; set; }


        public ProcessSale(string transactionId)
        {
            try
            {
                var doc = MakeProcessSaleCall(transactionId);
                if (doc != null)
                {
                    AuthorizationId = TryGetDescendant(doc, "AuthorizationId");
                    BatchNumber = TryGetDescendant(doc, "BatchNumber");
                    ExecutionTime = TryGetDescendant(doc, "ExecutionTime");
                    MerchantId = TryGetDescendant(doc, "MerchantId");
                    Operation = TryGetDescendant(doc, "Operation");
                    TransactionId = TryGetDescendant(doc, "TransactionId");
                    ResponseCode = TryGetDescendant(doc, "ResponseCode");
                    ResponseSource = TryGetDescendant(doc, "ResponseSource");
                    ResponseText = TryGetDescendant(doc, "ResponseText");

                    #region try get data from Nets error XML
                    if (!PaymentOk)
                    {
                        var des = doc.Descendants("Exception").Descendants("Error").Descendants("Message");
                        if (des.Any())
                            Message = des.FirstOrDefault().Value;

                        des = doc.Descendants("Exception").Descendants("Error").Descendants("Result");
                        if (des.Any())
                        {
                            ResponseCode = des.Descendants("ResponseCode").FirstOrDefault().Value;
                            IssuerId = des.Descendants("IssuerId").FirstOrDefault().Value;
                            ResponseText = des.Descendants("ResponseText").FirstOrDefault().Value;
                            ResponseSource = des.Descendants("ResponseSource").FirstOrDefault().Value;
                            TransactionId = des.Descendants("TransactionId").FirstOrDefault().Value;
                            ExecutionTime = des.Descendants("ExecutionTime").FirstOrDefault().Value;
                            MerchantId = des.Descendants("MerchantId").FirstOrDefault().Value;
                            MaskedPan = des.Descendants("MaskedPan").FirstOrDefault().Value;
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                new Logger("ProcessSale() failed for transactionId: " + transactionId, ex.ToString());
            }
        }

        private string TryGetDescendant(XDocument doc, string element)
        {
            var des = doc.Descendants(element);
            if (des.Any())
                return des.FirstOrDefault().Value;

            return string.Empty;
        }

        private XDocument MakeProcessSaleCall(string transactionId)
        {
            var url = new StringBuilder();
            url.Append(Settings.Nets_UrlProcess);
            url.Append("?merchantId=" + Settings.Nets_MerchantId);
            url.Append("&token=" + Settings.Nets_Token);
            url.Append("&transactionId=" + transactionId);
            url.Append("&operation=SALE");

            try
            {
                var doc = XDocument.Load(url.ToString());
                return doc;
            }
            catch (Exception ex)
            {
                new Logger("MakeProcessSaleCall() failed for url: " + url.ToString(), ex.ToString());
            }

            return null;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("<b>ProcessSale objekt</b><br>");
            sb.Append("AuthorizationId: " + AuthorizationId + "<br>");
            sb.Append("BatchNumber: " + BatchNumber + "<br>");
            sb.Append("ExecutionTime: " + ExecutionTime + "<br>");
            sb.Append("MerchantId: " + MerchantId + "<br>");
            sb.Append("Operation: " + Operation + "<br>");
            sb.Append("TransactionId: " + TransactionId + "<br>");
            sb.Append("ResponseCode: " + ResponseCode + "<br>");
            sb.Append("ResponseSource: " + ResponseSource + "<br>");
            sb.Append("ResponseText: " + ResponseText + "<br>");
            sb.Append("Message (error): " + Message + "<br>");
            sb.Append("IssuerId: " + IssuerId + "<br>");
            sb.Append("MaskedPan: " + MaskedPan + "<br>");
            return sb.ToString();
        }

    }
}
